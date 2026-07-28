using PCI.Backend.Core;
using PCI.Backend.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using static PCI.Backend.Core.CommunityMediaScanners;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// The image pipeline end to end (Core/CommunityMediaPipeline.cs;
/// docs/pciworld/CCP_PHASE2_DESIGN.md §6, §8).
///
/// The tests that carry the weight here are the FAILURE ones. A pipeline that publishes correctly
/// when everything works is easy; what this phase has to guarantee is that a classifier outage, a
/// worker crash, a duplicate delivery or an unreadable original all leave the image UNPUBLISHED
/// rather than published. So most of what follows deliberately breaks something and then asserts
/// that nothing reached the room.
///
/// Every fixture is an ordinary generated PNG. What makes one "restricted" for a test is that its
/// hash was registered as such with the test scanner — there is no real harmful material anywhere
/// near this suite, and there never needs to be.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityMediaPipelineTests
{
    static readonly IReadOnlyList<Rule> Policy = DefaultImageMatrix();

    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        Settings.Put(db, "world_community_enabled", "1");
        Settings.Put(db, "pciworld_community_images_enabled", "1");
        return db;
    }

    static byte[] Png(int seed)
    {
        // Vary the pixels so each fixture has its own hash — otherwise every test would share one
        // registered verdict and they would silently interfere.
        using var img = new Image<Rgba32>(24, 16);
        img[0, 0] = new Rgba32((byte)seed, (byte)(seed * 7), (byte)(seed * 13), 255);
        using var ms = new MemoryStream();
        img.Save(ms, new PngEncoder());
        return ms.ToArray();
    }

    static Dictionary<string, object?> Room(Db db, string slug, bool imagesAllowed = true)
    {
        db.Execute(
            "INSERT INTO pciworld_community_rooms(slug,title,state,image_allowed,next_sequence) VALUES(?,?,'open',?,1)",
            slug, "Room " + slug, imagesAllowed ? 1 : 0);
        return CommunityRooms.BySlug(db, slug)!;
    }

    static long Guest(Db db, long roomId, string name) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_guest_sessions(token_sha,room_id,display_name,display_name_folded,
                  active_name_key,rules_version_accepted,status)
              VALUES(?,?,?,?,?,'v1','active')",
            Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"), roomId, name, name, name);

    static Dictionary<string, object?> Media(Db db, long id) =>
        db.QueryOne("SELECT * FROM pciworld_community_media WHERE id=?", id)!;

    // ── nothing is published on upload ────────────────────────────────────────────────────────

    [Fact]
    public void AnUploadPublishesNothing()
    {
        // The rule the phase exists to keep. An accepted upload is a PENDING handle: no sequence, no
        // renderable copy, nothing another participant could load.
        var db = NewDb();
        var room = Room(db, "p-upload");
        var guest = Guest(db, H.L(room["id"]), "uploader");

        var r = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(1), "image/png", "A chart");
        Assert.True(r.Ok, r.Reason);
        Assert.Equal("pending", r.State);

        var m = Media(db, r.MediaId);
        Assert.Null(m["derivative_ref"]);
        Assert.Null(db.QueryOne("SELECT sequence FROM pciworld_community_messages WHERE id=?", r.MessageId)!["sequence"]);
    }

    [Fact]
    public void ASecondParticipantCannotSeeAPendingImage()
    {
        // The image restatement of E2E-014, asserted through the SAME reader participants use.
        var db = NewDb();
        var room = Room(db, "p-invisible");
        var guest = Guest(db, H.L(room["id"]), "invisible-uploader");

        CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(2), "image/png", "A chart");

        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void AnImageOnlyBecomesVisibleAfterTheAllowVerdict()
    {
        var db = NewDb();
        var room = Room(db, "p-allow");
        var guest = Guest(db, H.L(room["id"]), "allow-uploader");
        var bytes = Png(3);

        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));

        var outcome = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy);
        Assert.True(outcome.Claimed);
        Assert.Equal("allowed", outcome.State);
        Assert.NotNull(outcome.Sequence);

        Assert.Single(CommunityRooms.Since(db, H.L(room["id"]), 0));
        Assert.NotNull(Media(db, up.MediaId)["derivative_ref"]);
    }

    // ── failure directions ────────────────────────────────────────────────────────────────────

    [Fact]
    public void WithNoProviderConfiguredNothingIsEverPublished()
    {
        // NullMediaScanner is the production default. An operator who enables images without
        // choosing a provider must get a feature that visibly does not publish — not an unscanned
        // image feed. The retries are bounded, so this settles rather than spinning.
        var db = NewDb();
        var room = Room(db, "p-null");
        var guest = Guest(db, H.L(room["id"]), "null-uploader");
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(4), "image/png", "A chart");

        for (var i = 0; i < 8; i++)
        {
            db.Execute("UPDATE pciworld_media_scan_queue SET next_attempt_at=NULL WHERE media_id=?", up.MediaId);
            CommunityMediaPipeline.ScanOnce(db, new NullMediaScanner(), Policy);
        }

        var m = Media(db, up.MediaId);
        Assert.Equal("withheld", H.Str(m["state"]));
        Assert.Null(m["derivative_ref"]);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void AClassifierOutageRetriesRatherThanRefusingOrPublishing()
    {
        // A provider blip must not permanently refuse someone's photo — and must certainly not let
        // it through. Unpublished throughout is the part that matters.
        var db = NewDb();
        var room = Room(db, "p-outage");
        var guest = Guest(db, H.L(room["id"]), "outage-uploader");
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(5), "image/png", "A chart");

        var outcome = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(failEverything: true), Policy);
        Assert.Equal("classifier_unavailable", outcome.Reason);
        Assert.Equal("pending", H.Str(Media(db, up.MediaId)["state"]));
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));

        // And the failure is recorded, not swallowed: the scan row is what later explains the delay.
        Assert.True(db.Scalar<long>("SELECT COUNT(*) FROM pciworld_media_scans WHERE media_id=?", up.MediaId) > 0);
    }

    [Fact]
    public void AWorkerThatDiesMidScanStrandsNothing()
    {
        // Publication only happens in the committing transaction, so a worker that vanishes after
        // claiming has published nothing. Lease expiry must return the item to the queue.
        var db = NewDb();
        var room = Room(db, "p-crash");
        var guest = Guest(db, H.L(room["id"]), "crash-uploader");
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(6), "image/png", "A chart");

        // Claim it and then "die": mark the lease already expired.
        var queueId = db.Scalar<long>("SELECT id FROM pciworld_media_scan_queue WHERE media_id=?", up.MediaId);
        Assert.True(WorkerLease.TryClaim(db, "pciworld_media_scan_queue", queueId, "dead-worker",
                                         claimableInSql: "'queued','retrying'"));
        db.Execute("UPDATE pciworld_media_scan_queue SET lease_until=? WHERE id=?", H.StampInMinutes(-10), queueId);

        Assert.Equal("pending", H.Str(Media(db, up.MediaId)["state"]));
        Assert.Equal(1, CommunityMediaPipeline.RecoverStranded(db));

        db.Execute("UPDATE pciworld_media_scan_queue SET next_attempt_at=NULL WHERE id=?", queueId);
        Assert.Equal("allowed", CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy).State);
    }

    [Fact]
    public void ADuplicateScanDeliveryConvergesOnOneOutcomeAndOneSequence()
    {
        // E2E-028's image case. Re-running the scan must not allocate a second sequence, which would
        // put the same image in the transcript twice and break ordered reconnect recovery.
        var db = NewDb();
        var room = Room(db, "p-dup");
        var guest = Guest(db, H.L(room["id"]), "dup-uploader");
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(7), "image/png", "A chart");

        var first = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy);
        Assert.Equal("allowed", first.State);

        // Re-queue the same item, as a duplicate delivery would.
        db.Execute("UPDATE pciworld_media_scan_queue SET status='queued', next_attempt_at=NULL WHERE media_id=?", up.MediaId);
        var second = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy);

        Assert.Equal("already_settled", second.Reason);
        Assert.Single(CommunityRooms.Since(db, H.L(room["id"]), 0));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_messages WHERE media_id=? AND sequence IS NOT NULL", up.MediaId));
    }

    [Fact]
    public void AnUnreadableOriginalIsRetriedAndNeverPublished()
    {
        var db = NewDb();
        var room = Room(db, "p-lost");
        var guest = Guest(db, H.L(room["id"]), "lost-uploader");
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(8), "image/png", "A chart");

        db.Execute("UPDATE pciworld_community_media SET storage_ref='local:nope/nope.png' WHERE id=?", up.MediaId);
        var outcome = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy);

        Assert.Equal("original_unreadable", outcome.Reason);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    // ── withheld and restricted ───────────────────────────────────────────────────────────────

    [Fact]
    public void AWithheldImageGetsNoSequenceAndNoRenderableCopy()
    {
        var db = NewDb();
        var room = Room(db, "p-withheld");
        var guest = Guest(db, H.L(room["id"]), "withheld-uploader");
        var bytes = Png(9);
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");

        var scanner = new DeterministicMediaScanner();
        // Register against the DERIVATIVE, which is what the scanner is shown.
        scanner.Register(CommunityMedia.Sanitise(bytes, "image/png").Derivative!, Categories.Hate, Band.Medium);

        Assert.Equal("withheld", CommunityMediaPipeline.ScanOnce(db, scanner, Policy).State);

        var m = Media(db, up.MediaId);
        Assert.Null(m["derivative_ref"]);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void ARestrictedVerdictLeavesNothingRenderableAnywhere()
    {
        // §28.7. The original moves out to the restricted store and the media row's reference is
        // nulled, so no ordinary handler can resolve it; no derivative is ever produced. There is
        // nothing for an admin thumbnail to show because nothing exists.
        var db = NewDb();
        var room = Room(db, "p-restricted");
        var guest = Guest(db, H.L(room["id"]), "restricted-uploader");
        var bytes = Png(10);
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");

        var scanner = new DeterministicMediaScanner();
        scanner.Register(CommunityMedia.Sanitise(bytes, "image/png").Derivative!, Categories.Grooming, Band.High);

        Assert.Equal("restricted", CommunityMediaPipeline.ScanOnce(db, scanner, Policy).State);

        var m = Media(db, up.MediaId);
        Assert.Equal("restricted", H.Str(m["state"]));
        Assert.Null(m["storage_ref"]);
        Assert.Null(m["derivative_ref"]);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));

        var evidence = db.QueryOne("SELECT * FROM pciworld_restricted_evidence WHERE media_id=?", up.MediaId);
        Assert.NotNull(evidence);
        // Preserved by default: an escalation must not be quietly deleted by a retention sweep.
        Assert.True(H.B(evidence!["legal_hold"]));
        Assert.NotNull(evidence["storage_ref"]);
    }

    [Fact]
    public void ARetentionSweepDoesNotDestroyEscalatedEvidence()
    {
        // "Never delete production data" enforced by a check rather than a convention. The bytes are
        // MOVED to the restricted category on escalation, and that category is exempt from the
        // sweep — because an escalation carries a legal hold, and material a hold exists to preserve
        // must not age out on a schedule. Deleting it is a deliberate, recorded act, never a side
        // effect of a nightly job.
        var db = NewDb();
        var room = Room(db, "p-hold");
        var guest = Guest(db, H.L(room["id"]), "hold-uploader");
        var bytes = Png(20);
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");

        var scanner = new DeterministicMediaScanner();
        scanner.Register(CommunityMedia.Sanitise(bytes, "image/png").Derivative!, Categories.Grooming, Band.High);
        Assert.Equal("restricted", CommunityMediaPipeline.ScanOnce(db, scanner, Policy).State);

        var evidenceRef = H.Str(db.QueryOne(
            "SELECT storage_ref FROM pciworld_restricted_evidence WHERE media_id=?", up.MediaId)!["storage_ref"]);
        Assert.NotNull(evidenceRef);
        Assert.NotNull(Storage.Get(evidenceRef)?.bytes);

        // A sweep aggressive enough to remove everything eligible.
        Storage.PurgeOlderThan(-1);

        Assert.NotNull(Storage.Get(evidenceRef)?.bytes);
    }

    [Fact]
    public void TheOrdinaryCopyOfEscalatedMediaIsNotLeftBehindInAPurgeableCategory()
    {
        // Recording the reference and leaving the file where it was would have left preserved
        // evidence in a category the sweep does delete — the hold would have been a comment.
        var db = NewDb();
        var room = Room(db, "p-moved");
        var guest = Guest(db, H.L(room["id"]), "moved-uploader");
        var bytes = Png(21);
        var up = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");

        var originalRef = H.Str(Media(db, up.MediaId)["storage_ref"]);
        Assert.NotNull(originalRef);

        var scanner = new DeterministicMediaScanner();
        scanner.Register(CommunityMedia.Sanitise(bytes, "image/png").Derivative!, Categories.Grooming, Band.High);
        CommunityMediaPipeline.ScanOnce(db, scanner, Policy);

        var evidenceRef = H.Str(db.QueryOne(
            "SELECT storage_ref FROM pciworld_restricted_evidence WHERE media_id=?", up.MediaId)!["storage_ref"]);
        Assert.NotEqual(originalRef, evidenceRef);
        Assert.Contains(CommunityMediaPipeline.RestrictedCategory, evidenceRef);
        Assert.Null(Storage.Get(originalRef)?.bytes);
    }

    [Fact]
    public void ARestrictedVerdictDoesNotEndTheParticipantsSession()
    {
        // §28.5: no low-confidence-or-otherwise classifier result becomes an irreversible automatic
        // sanction. The image is withheld and a human is brought in; the person is not ejected by a
        // machine.
        var db = NewDb();
        var room = Room(db, "p-nosanction");
        var guest = Guest(db, H.L(room["id"]), "nosanction-uploader");
        var bytes = Png(11);
        CommunityMediaPipeline.Upload(db, room, guest, null, "u1", bytes, "image/png", "A chart");

        var scanner = new DeterministicMediaScanner();
        scanner.Register(CommunityMedia.Sanitise(bytes, "image/png").Derivative!, Categories.Grooming, Band.High);
        CommunityMediaPipeline.ScanOnce(db, scanner, Policy);

        Assert.Equal("active", H.Str(db.QueryOne("SELECT status FROM pciworld_guest_sessions WHERE id=?", guest)!["status"]));
    }

    // ── the gates ─────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void ImagesAreRefusedWhileTheFeatureIsOff()
    {
        var db = NewDb();
        Settings.Put(db, "pciworld_community_images_enabled", "0");
        var room = Room(db, "p-off");
        var guest = Guest(db, H.L(room["id"]), "off-uploader");

        Assert.Equal("images_disabled",
            CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(12), "image/png", "A chart").Reason);
    }

    [Fact]
    public void BothGatesAreRequiredNotJustOne()
    {
        // The site setting is the kill switch; the room column is the per-room grant. A room that
        // has not been granted images must refuse them even with the feature globally on.
        var db = NewDb();
        var room = Room(db, "p-roomoff", imagesAllowed: false);
        var guest = Guest(db, H.L(room["id"]), "roomoff-uploader");

        Assert.Equal("images_not_allowed_here",
            CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(13), "image/png", "A chart").Reason);
    }

    [Fact]
    public void AlternativeTextIsRequired()
    {
        // An image nobody can describe excludes every blind participant, and "we'll add alt text
        // later" never happens. It is a precondition of upload, not a nice-to-have.
        var db = NewDb();
        var room = Room(db, "p-alt");
        var guest = Guest(db, H.L(room["id"]), "alt-uploader");

        Assert.Equal("alt_text_required",
            CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(14), "image/png", "   ").Reason);
    }

    [Fact]
    public void AHostileUploadIsRefusedRatherThanStored()
    {
        var db = NewDb();
        var room = Room(db, "p-hostile");
        var guest = Guest(db, H.L(room["id"]), "hostile-uploader");
        var svg = System.Text.Encoding.UTF8.GetBytes("<svg xmlns=\"http://www.w3.org/2000/svg\"><script/></svg>");

        var r = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", svg, "image/png", "Not an image");
        Assert.Equal("svg_not_accepted", r.Reason);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_media"));
    }

    [Fact]
    public void ARetriedUploadReturnsTheOriginalOutcomeAndQueuesNoSecondScan()
    {
        // A second scan would be a second vendor call paid for and a second chance to race the
        // verdict.
        var db = NewDb();
        var room = Room(db, "p-retry");
        var guest = Guest(db, H.L(room["id"]), "retry-uploader");

        var first = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(15), "image/png", "A chart");
        var second = CommunityMediaPipeline.Upload(db, room, guest, null, "u1", Png(15), "image/png", "A chart");

        Assert.Equal("duplicate", second.Reason);
        Assert.Equal(first.MediaId, second.MediaId);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_media_scan_queue WHERE media_id=?", first.MediaId));
    }

    [Fact]
    public void AnIdleQueueIsNotAnError()
    {
        var db = NewDb();
        var outcome = CommunityMediaPipeline.ScanOnce(db, new DeterministicMediaScanner(), Policy);
        Assert.False(outcome.Claimed);
        Assert.Equal("idle", outcome.Reason);
    }
}
