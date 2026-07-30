using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community images (Phase 2) — the schema-level guarantees, asserted against whichever
/// provider TEST_DB_PROVIDER selects so the MySQL leg proves parity rather than assuming it
/// (Data/CommunityMediaSchema.cs; docs/pciworld/CCP_PHASE2_DESIGN.md §4–§5).
///
/// These test STRUCTURE, not behaviour, because the structure is what makes the two prohibitions
/// hard to violate later: an image cannot be served before its verdict if no renderable copy exists
/// until then (§28.4), and suspected illegal material cannot appear in an ordinary admin thumbnail
/// if no derivative is ever produced for it (§28.7). A behavioural check would pass just as well
/// against code that happens to be correct today; these fail the moment the shape changes.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityMediaSchemaTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long NewRoom(Db db, string slug) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_community_rooms(slug,title,state) VALUES(?,?,'open')", slug, "Room " + slug);

    static long NewGuest(Db db, long roomId, string folded) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_guest_sessions(token_sha,room_id,display_name,display_name_folded,
                  active_name_key,rules_version_accepted,status)
              VALUES(?,?,?,?,?,'v1','active')",
            Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"), roomId, folded, folded, folded);

    static long NewMedia(Db db, long roomId, long sessionId, string uploadId, string state = "pending") =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_community_media(room_id,guest_session_id,client_upload_id,state,
                  declared_mime,sniffed_mime,byte_size,content_sha256)
              VALUES(?,?,?,?,'image/png','image/png',1024,?)",
            roomId, sessionId, uploadId, state, Guid.NewGuid().ToString("n"));

    // ── Upload idempotency ────────────────────────────────────────────────────────────────────

    [Fact]
    public void ARetriedUploadCannotCreateASecondMediaRow()
    {
        // A double-tap, a retried POST or a reconnect must produce one image, not two — and must
        // not cause a second scan to be paid for. The client's own key is what makes retry safe,
        // exactly as ux_wcmsg_client does for Phase 1 text.
        var db = NewWorldDb();
        var room = NewRoom(db, "m-idem");
        var guest = NewGuest(db, room, "uploader");
        NewMedia(db, room, guest, "upload-1");

        Assert.ThrowsAny<Exception>(() => NewMedia(db, room, guest, "upload-1"));
    }

    [Fact]
    public void TheSameUploadKeyIsFreeForADifferentSession()
    {
        // Two participants who both happen to generate 'upload-1' are not each other's duplicates.
        var db = NewWorldDb();
        var room = NewRoom(db, "m-idem-2");
        var a = NewGuest(db, room, "uploader-a");
        var b = NewGuest(db, room, "uploader-b");

        NewMedia(db, room, a, "upload-1");
        NewMedia(db, room, b, "upload-1");

        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_media WHERE room_id=?", room));
    }

    // ── The publication boundary lives in the columns ─────────────────────────────────────────

    [Fact]
    public void APendingUploadHasNoRenderableCopy()
    {
        // The rule this phase exists to keep: nothing a second participant could load exists until
        // the verdict does. derivative_ref is the only thing ever served, and it is unset here.
        var db = NewWorldDb();
        var room = NewRoom(db, "m-pending");
        var guest = NewGuest(db, room, "pending-uploader");
        var media = NewMedia(db, room, guest, "u-1");

        var row = db.QueryOne("SELECT state,derivative_ref FROM pciworld_community_media WHERE id=?", media);
        Assert.NotNull(row);
        Assert.Equal("pending", PCI.Backend.Core.H.Str(row!["state"]));
        Assert.Null(row!["derivative_ref"]);
    }

    [Fact]
    public void TheMediaTableCarriesTheOriginalAndTheDerivativeSeparately()
    {
        // Two references, not one. Collapsing them into a single column is precisely the change
        // that would make "serve the bytes we were given" a one-line mistake.
        var cols = NewWorldDb().Columns("pciworld_community_media");
        Assert.Contains("storage_ref", cols);
        Assert.Contains("derivative_ref", cols);
    }

    // ── Restricted evidence (§28.7): the control is absence ───────────────────────────────────

    [Fact]
    public void TheRestrictedEvidenceStoreHasNoRenderableColumnAtAll()
    {
        // §28.7 forbids suspected illegal media appearing in ordinary admin thumbnails. That is
        // enforced here by there being nothing to thumbnail: no derivative is ever produced for a
        // restricted item, so the table has no column that could hold one. If someone adds one,
        // this test fails and the review conversation happens before the exposure does.
        var cols = NewWorldDb().Columns("pciworld_restricted_evidence");
        Assert.DoesNotContain("derivative_ref", cols);
        Assert.DoesNotContain("thumbnail_ref", cols);
        Assert.DoesNotContain("preview_ref", cols);
    }

    [Fact]
    public void OneEvidenceRecordPerMediaItem()
    {
        // A second escalation of the same item must reuse the record — and therefore its legal
        // hold. A parallel record is a row a retention purge could miss while believing the item
        // was held.
        var db = NewWorldDb();
        var room = NewRoom(db, "m-evid");
        var guest = NewGuest(db, room, "evid-uploader");
        var media = NewMedia(db, room, guest, "u-evid", "restricted");

        db.Execute("INSERT INTO pciworld_restricted_evidence(media_id,reason) VALUES(?,'escalated')", media);
        Assert.ThrowsAny<Exception>(() =>
            db.Execute("INSERT INTO pciworld_restricted_evidence(media_id,reason) VALUES(?,'escalated again')", media));
    }

    [Fact]
    public void EvidenceCarriesTheHoldFieldsRetentionMustHonour()
    {
        // "Never delete production data" is enforced by the purge consulting these, and by the test
        // that fails if it does not. The columns existing is the precondition for both.
        var cols = NewWorldDb().Columns("pciworld_restricted_evidence");
        Assert.Contains("legal_hold", cols);
        Assert.Contains("preserved_until", cols);
    }

    [Fact]
    public void EvidenceAccessIsShapedForTwoPeople()
    {
        // Maker-checker: a request and a DISTINCT approval, both recorded. Server-side enforcement
        // comes later; without the columns it could not be recorded at all.
        var cols = NewWorldDb().Columns("pciworld_restricted_evidence");
        Assert.Contains("requested_by", cols);
        Assert.Contains("approved_by", cols);
        Assert.Contains("accessed_count", cols);
    }

    // ── Scan history is append-only, including the failures ───────────────────────────────────

    [Fact]
    public void ScanFailuresAreRecordedAlongsideVerdicts()
    {
        // A timeout row is not noise — it is the evidence that explains why an image was withheld.
        // A schema that only stored verdicts would make "withheld, no reason given" unanswerable.
        var db = NewWorldDb();
        var room = NewRoom(db, "m-scan");
        var guest = NewGuest(db, room, "scan-uploader");
        var media = NewMedia(db, room, guest, "u-scan");

        db.Execute("INSERT INTO pciworld_media_scans(media_id,provider,outcome,error_text) VALUES(?,'null','timeout','deadline exceeded')", media);
        db.Execute("INSERT INTO pciworld_media_scans(media_id,provider,outcome,band) VALUES(?,'null','unsupported','')", media);

        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_media_scans WHERE media_id=?", media));
    }

    // ── Message linkage reuses Phase 1's ordering rules ───────────────────────────────────────

    [Fact]
    public void ImageMessagesReusePhase1sMessageTable()
    {
        // Deliberately not a parallel table: ordering, idempotency and the sequence-is-publication
        // rule are inherited unchanged rather than reimplemented (and re-broken) for images.
        var cols = NewWorldDb().Columns("pciworld_community_messages");
        Assert.Contains("message_kind", cols);
        Assert.Contains("media_id", cols);
    }

    // ── Install-time behaviour ────────────────────────────────────────────────────────────────

    [Fact]
    public void ImagesShipDisabled()
    {
        // CCP-P1-003 (legal/child-safety prerequisites) and CCP-P1-004 (provider and calibrated
        // bands) are unresolved and are not this repository's to resolve. A fresh database must not
        // have image upload reachable.
        var db = NewWorldDb();
        Assert.False(PCI.Backend.Core.Settings.Bool(db, "pciworld_community_images_enabled", false));
    }

    [Fact]
    public void AnOperatorsChoiceSurvivesAReboot()
    {
        // A deploy must not turn images on for an operator who has not satisfied the prerequisites,
        // nor off for one who has.
        var db = NewWorldDb();
        PCI.Backend.Core.Settings.Put(db, "pciworld_community_images_enabled", "1");
        CommunityMediaSchema.Ensure(db);
        Assert.True(PCI.Backend.Core.Settings.Bool(db, "pciworld_community_images_enabled", false));
    }

    [Fact]
    public void EnsureIsIdempotent()
    {
        // It runs on every boot — including the guarded ALTERs that add message_kind and media_id,
        // which are exactly the kind of upgrade that throws the second time if it is not guarded.
        var db = NewWorldDb();
        CommunityMediaSchema.Ensure(db);
        CommunityMediaSchema.Ensure(db);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_media"));
    }
}
