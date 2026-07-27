using PCI.Backend.Data;
using static PCI.Backend.Core.CommunityMediaScanners;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Core;

/// <summary>
/// The image pipeline itself (docs/pciworld/CCP_PHASE2_DESIGN.md §6). Two halves, deliberately
/// separated by a durable queue:
///
///   <see cref="Upload"/>  — synchronous, cheap, fail-closed. Validates, sanitises, stores the
///                           original, records a PENDING media row and a PENDING message, and
///                           queues a scan. It publishes nothing.
///   <see cref="ScanOnce"/> — asynchronous. Claims one queued item, classifies it, and only then —
///                           inside one transaction — allocates a sequence and writes the broadcast.
///
/// WHY THE SPLIT IS A SAFETY PROPERTY AND NOT JUST ARCHITECTURE
///
/// A classifier call is network I/O against a third party. Doing it inside the upload request would
/// put a vendor's latency on a participant's send path, and doing it inside the transaction would
/// hold a database lock across that call — the exact failure the platform's vendor-stall regression
/// test exists to prevent. Separating them also makes the publication boundary explicit: the only
/// code that allocates a sequence is the code that has a verdict in hand.
///
/// THE ORDER OF WORK IN <see cref="ScanOnce"/> MATTERS
///
/// The derivative is produced from the stored original, scanned, and only WRITTEN TO STORAGE once
/// the verdict is allow — and the storage write happens BEFORE the transaction opens, never inside
/// it, for the same no-I/O-under-the-lock reason. A derivative stored for an item that then fails to
/// commit is an unreferenced blob: content-addressed, purgeable, and unreachable, because nothing
/// can serve bytes no row points at.
///
/// A restricted verdict goes further: the original MOVES to the restricted evidence store and the
/// media row's storage_ref is nulled, so no ordinary handler can resolve it, and no derivative is
/// ever produced at all (§28.7).
/// </summary>
public static class CommunityMediaPipeline
{
    public const string OriginalCategory = "pciworld-community-original";
    public const string DerivativeCategory = "pciworld-community-render";
    public const string RestrictedCategory = "pciworld-community-restricted";

    public static bool ImagesEnabled(Db db) => Settings.Bool(db, "pciworld_community_images_enabled", false);

    /// <summary>
    /// The configured scanner. Anything other than an explicitly recognised name yields
    /// <see cref="NullMediaScanner"/>, which classifies nothing and therefore publishes nothing.
    /// A typo in this setting must fail CLOSED — leaving a room publishing unscanned images because
    /// somebody misspelled a provider would be the worst possible reading of an unknown value.
    /// </summary>
    public static IMediaScanner ConfiguredScanner(Db db) =>
        Settings.Str(db, "pciworld_community_image_scanner", "none") switch
        {
            "deterministic" => new DeterministicMediaScanner(),
            _ => new NullMediaScanner(),
        };

    /// <summary>
    /// Whether a media row may be served to a participant. The single place that question is
    /// answered, so no handler can accidentally serve a pending, withheld or restricted item by
    /// checking only one of the two conditions.
    /// </summary>
    public static bool IsServable(Dictionary<string, object?>? media) =>
        media is not null
        && H.Str(media["state"]) == "allowed"
        && !string.IsNullOrEmpty(H.Str(media["derivative_ref"]));

    public record UploadResult(bool Ok, long MediaId, long MessageId, string State, string Reason)
    {
        public static UploadResult Rejected(string reason) => new(false, 0, 0, "rejected", reason);
    }

    /// <summary>
    /// Accept an image for inspection. Returns a PENDING handle — never a URL, never a sequence, and
    /// never anything another participant could load. Rejections are ordinary results with a reason
    /// code, not exceptions: a hostile upload must not be able to produce a 500.
    /// </summary>
    public static UploadResult Upload(
        Db db, Dictionary<string, object?> room, long? guestSessionId, long? worldUserId,
        string clientUploadId, byte[]? bytes, string? declaredMime, string? altText,
        string? correlationId = null)
    {
        var roomId = H.L(room["id"]);

        // Both gates, not one. The site setting is the kill switch; the room column is the per-room
        // grant. Neither alone is enough to publish an image anywhere.
        if (!ImagesEnabled(db)) return UploadResult.Rejected("images_disabled");
        if (!H.B(room["image_allowed"])) return UploadResult.Rejected("images_not_allowed_here");
        if (!CommunityRooms.AcceptsMessages(room)) return UploadResult.Rejected("room_not_accepting");

        if (string.IsNullOrWhiteSpace(clientUploadId)) return UploadResult.Rejected("missing_upload_id");

        // Idempotency before any work — same reasoning as the text path: a retry must return the
        // original outcome rather than paying for a second sanitise and a second scan.
        var existing = db.QueryOne(
            "SELECT id,state,message_id FROM pciworld_community_media WHERE guest_session_id=? AND client_upload_id=?",
            guestSessionId, clientUploadId);
        if (existing is not null)
            return new UploadResult(false, H.L(existing["id"]),
                                    existing["message_id"] is null ? 0 : H.L(existing["message_id"]),
                                    H.Str(existing["state"]) ?? "pending", "duplicate");

        // Alternative text is required, not optional. An image nobody can describe is an image a
        // blind participant is simply excluded from — and "we'll add alt text later" never happens.
        var alt = (altText ?? "").Trim();
        if (alt.Length == 0) return UploadResult.Rejected("alt_text_required");
        if (alt.Length > 500) return UploadResult.Rejected("alt_text_too_long");

        var accepted = CommunityMedia.Sanitise(bytes, declaredMime);
        if (!accepted.Ok) return UploadResult.Rejected(accepted.Error ?? "rejected");

        // Storage is I/O: it happens BEFORE the transaction opens, never inside it.
        var stored = Storage.Put(accepted.Original!, accepted.Mime, OriginalCategory);

        long mediaId = 0, messageId = 0;
        db.Transaction(() =>
        {
            mediaId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_community_media
                      (room_id,guest_session_id,world_user_id,client_upload_id,state,declared_mime,
                       sniffed_mime,byte_size,width,height,content_sha256,alt_text,storage_ref)
                  VALUES(?,?,?,?,'pending',?,?,?,?,?,?,?,?)",
                roomId, guestSessionId, worldUserId, clientUploadId, accepted.Mime, accepted.Mime,
                accepted.Original!.LongLength, accepted.Width, accepted.Height, accepted.Sha256,
                alt, stored.Reference);

            // The message is created NOW, with sequence NULL. Phase 1's reader already filters on
            // `sequence IS NOT NULL`, so a pending image is invisible to every participant without
            // one new query predicate. The safest change to a publication path is no change at all.
            messageId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_community_messages
                      (room_id,sequence,guest_session_id,world_user_id,client_message_id,body,
                       status,message_kind,media_id)
                  VALUES(?,NULL,?,?,?,?,'pending','image',?)",
                roomId, guestSessionId, worldUserId, clientUploadId, alt, mediaId);

            db.Execute("UPDATE pciworld_community_media SET message_id=? WHERE id=?", messageId, mediaId);

            db.Execute(
                "INSERT INTO pciworld_media_scan_queue(media_id,correlation_id) VALUES(?,?)",
                mediaId, correlationId);
        });

        return new UploadResult(true, mediaId, messageId, "pending", "queued_for_review");
    }

    public record ScanOutcome(bool Claimed, long MediaId, string State, string Reason, long? Sequence);

    /// <summary>
    /// Claim and process at most one queued scan. Returns <c>Claimed=false</c> when there was
    /// nothing to do, so a caller can drain in a loop without special-casing an empty queue.
    ///
    /// Crash safety: the claim is a single conditional UPDATE via <see cref="WorkerLease"/>, so two
    /// workers cannot both take the same item, and <see cref="RecoverStranded"/> returns items whose
    /// lease expired because their worker died. An item processed twice must converge on the same
    /// outcome, which it does — the terminal states are only written for an item still in 'pending'.
    /// </summary>
    public static ScanOutcome ScanOnce(Db db, IMediaScanner scanner, IReadOnlyList<Rule> policy,
                                       string? owner = null)
    {
        owner ??= WorkerLease.NewOwner();
        var due = db.QueryOne(
            @"SELECT id,media_id FROM pciworld_media_scan_queue
              WHERE status IN ('queued','retrying')
                AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))
              ORDER BY id LIMIT 1");
        if (due is null) return new ScanOutcome(false, 0, "", "idle", null);

        var queueId = H.L(due["id"]);
        var mediaId = H.L(due["media_id"]);
        if (!WorkerLease.TryClaim(db, "pciworld_media_scan_queue", queueId, owner,
                                  claimableInSql: "'queued','retrying'"))
            return new ScanOutcome(false, mediaId, "", "lost_claim", null);

        var media = db.QueryOne(
            "SELECT id,room_id,message_id,state,storage_ref,declared_mime FROM pciworld_community_media WHERE id=?",
            mediaId);
        if (media is null || H.Str(media["state"]) != "pending")
        {
            // Already terminal — a duplicate delivery, or an item an operator resolved by hand.
            // Converging silently is correct; re-deciding it would be the bug.
            Finish(db, queueId, "done", null);
            return new ScanOutcome(true, mediaId, H.Str(media?["state"]) ?? "missing", "already_settled", null);
        }

        // Rebuild the derivative from the stored original. It is what a participant would actually
        // see, so it is what the verdict should be about — and it carries none of the uploader's
        // metadata, so nothing extra is handed to a third-party classifier along with the picture.
        var fetched = Storage.Get(H.Str(media["storage_ref"]));
        if (fetched?.bytes is null)
        {
            Retry(db, queueId, "original_unreadable");
            return new ScanOutcome(true, mediaId, "pending", "original_unreadable", null);
        }

        var rebuilt = CommunityMedia.Sanitise(fetched.Value.bytes, H.Str(media["declared_mime"]));
        if (!rebuilt.Ok)
        {
            // The stored bytes no longer sanitise. Withhold — never publish something we can no
            // longer vouch for.
            Settle(db, mediaId, queueId, "withheld", rebuilt.Error ?? "unrecognised_image",
                   scanner, band: null, confidence: null);
            return new ScanOutcome(true, mediaId, "withheld", rebuilt.Error ?? "unrecognised_image", null);
        }

        Signal signal;
        try
        {
            signal = scanner.Classify(new MediaSubject(
                rebuilt.Derivative!, rebuilt.Mime, rebuilt.Width, rebuilt.Height, rebuilt.Sha256));
        }
        catch { signal = Signal.Unavailable; }

        var decision = Resolve(policy, signal, contentType: "image");

        // An unusable verdict is a RETRY, not a terminal withhold: a provider blip should not
        // permanently refuse someone's photo. It stays unpublished throughout, which is the part
        // that matters. Retries are bounded so a genuinely dead provider does not spin for ever.
        if (!signal.Usable)
        {
            var attempts = H.L(db.Scalar<long>("SELECT attempts FROM pciworld_media_scan_queue WHERE id=?", queueId));
            RecordScan(db, mediaId, scanner, "unavailable", null, null, "classifier unavailable");
            if (attempts < 5)
            {
                Retry(db, queueId, "classifier_unavailable");
                return new ScanOutcome(true, mediaId, "pending", "classifier_unavailable", null);
            }
            Settle(db, mediaId, queueId, "withheld", "moderation_unavailable", scanner, null, null);
            return new ScanOutcome(true, mediaId, "withheld", "moderation_unavailable", null);
        }

        var band = decision.Confidence.ToString().ToLowerInvariant();
        RecordScan(db, mediaId, scanner, decision.Outcome == Outcome.Allow ? "allowed" : "refused",
                   band, null, null);

        // ── Restricted: out of the ordinary pipeline entirely, and no derivative is ever made. ──
        if (decision.Outcome == Outcome.Escalate)
        {
            // The bytes genuinely MOVE — they are re-stored under the restricted category, which the
            // retention sweep never touches, and the ordinary copy is deleted. Recording the old
            // reference and leaving the file where it was would have left preserved evidence sitting
            // in a purgeable category, so a retention sweep would eventually have destroyed exactly
            // the material a legal hold exists to keep. I/O first, transaction after.
            var moved = Storage.Put(fetched.Value.bytes, rebuilt.Mime, RestrictedCategory).Reference;
            Storage.DeleteOne(H.Str(media["storage_ref"]));
            Escalate(db, mediaId, queueId, moved, media, decision, scanner, band);
            return new ScanOutcome(true, mediaId, "restricted", decision.ReasonCode, null);
        }

        if (!decision.Publishes)
        {
            Settle(db, mediaId, queueId, "withheld", decision.ReasonCode, scanner, band, null);
            return new ScanOutcome(true, mediaId, "withheld", decision.ReasonCode, null);
        }

        // ── Allowed. Store the render copy OUTSIDE the transaction, then commit the reference. ──
        var renderRef = Storage.Put(rebuilt.Derivative!, rebuilt.Mime, DerivativeCategory).Reference;

        long? sequence = null;
        var roomId = H.L(media["room_id"]);
        var messageId = media["message_id"] is null ? (long?)null : H.L(media["message_id"]);

        db.Transaction(() =>
        {
            // Identical allocation to the text path, and deliberately so: read the counter, then
            // increment it with a conditional UPDATE and require exactly one row changed. Two
            // concurrent publishes cannot both win.
            var current = db.Scalar<long>("SELECT next_sequence FROM pciworld_community_rooms WHERE id=?", roomId);
            var changed = db.Execute(
                "UPDATE pciworld_community_rooms SET next_sequence=next_sequence+1 WHERE id=? AND next_sequence=?",
                roomId, current);
            if (changed != 1) throw new InvalidOperationException("sequence_contended");
            sequence = current;

            db.Execute(
                @"UPDATE pciworld_community_media
                  SET state='allowed', derivative_ref=?, scan_provider=?, scan_provider_version=?,
                      scan_band=?, verdict_at=datetime('now'), updated_at=datetime('now'), version=version+1
                  WHERE id=? AND state='pending'",
                renderRef, scanner.ProviderName, scanner.ProviderVersion, band, mediaId);

            if (messageId is { } mid)
                db.Execute(
                    "UPDATE pciworld_community_messages SET sequence=?, status='allowed', published_at=datetime('now') WHERE id=?",
                    sequence, mid);

            db.Execute(
                @"INSERT INTO pciworld_community_outbox(event_type,room_id,payload_json)
                  VALUES('CommunityMediaAllowed',?,?)",
                roomId,
                System.Text.Json.JsonSerializer.Serialize(new { mediaId, messageId, roomId, sequence }));

            db.Execute(
                "UPDATE pciworld_media_scan_queue SET status='done', lease_owner=NULL, lease_until=NULL, completed_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                queueId);
        });

        return new ScanOutcome(true, mediaId, "allowed", decision.ReasonCode, sequence);
    }

    /// <summary>Return items whose worker died mid-scan. Their lease expired; nothing was published,
    /// because publication only happens in the committing transaction.</summary>
    public static int RecoverStranded(Db db) =>
        WorkerLease.RecoverExpired(db, "pciworld_media_scan_queue", "retrying");

    // ── internals ─────────────────────────────────────────────────────────────────────────────

    static void RecordScan(Db db, long mediaId, IMediaScanner scanner, string outcome,
                           string? band, double? confidence, string? error) =>
        db.Execute(
            @"INSERT INTO pciworld_media_scans
                  (media_id,provider,provider_version,requested_at,completed_at,outcome,band,confidence,error_text)
              VALUES(?,?,?,datetime('now'),datetime('now'),?,?,?,?)",
            mediaId, scanner.ProviderName, scanner.ProviderVersion, outcome, band, confidence, error);

    static void Settle(Db db, long mediaId, long queueId, string state, string reason,
                       IMediaScanner scanner, string? band, double? confidence)
    {
        db.Transaction(() =>
        {
            // Guarded on state='pending' so a duplicate delivery converges instead of re-deciding.
            db.Execute(
                @"UPDATE pciworld_community_media
                  SET state=?, withheld_reason=?, scan_provider=?, scan_provider_version=?, scan_band=?,
                      scan_confidence=?, verdict_at=datetime('now'), updated_at=datetime('now'), version=version+1
                  WHERE id=? AND state='pending'",
                state, reason, scanner.ProviderName, scanner.ProviderVersion, band, confidence, mediaId);
            db.Execute(
                "UPDATE pciworld_community_messages SET status='blocked' WHERE media_id=? AND sequence IS NULL",
                mediaId);
            Finish(db, queueId, "done", null);
        });
    }

    static void Escalate(Db db, long mediaId, long queueId, string restrictedRef,
                         Dictionary<string, object?> media,
                         Decision decision, IMediaScanner scanner, string band)
    {
        db.Transaction(() =>
        {
            db.Execute(
                @"UPDATE pciworld_community_media
                  SET state='restricted', withheld_reason=?, scan_provider=?, scan_provider_version=?,
                      scan_band=?, storage_ref=NULL, derivative_ref=NULL,
                      verdict_at=datetime('now'), updated_at=datetime('now'), version=version+1
                  WHERE id=? AND state='pending'",
                decision.ReasonCode, scanner.ProviderName, scanner.ProviderVersion, band, mediaId);

            // The reference moves here and is removed from the media row above, so no ordinary
            // handler can resolve the bytes. There is no derivative column on this table at all —
            // §28.7 is enforced by there being nothing to thumbnail.
            db.Execute(
                @"INSERT OR IGNORE INTO pciworld_restricted_evidence(media_id,storage_ref,content_sha256,reason,legal_hold)
                  VALUES(?,?,?,?,1)",
                mediaId, restrictedRef, media.TryGetValue("content_sha256", out var sha) ? sha : null,
                decision.ReasonCode);

            db.Execute(
                "UPDATE pciworld_community_messages SET status='blocked' WHERE media_id=? AND sequence IS NULL",
                mediaId);
            Finish(db, queueId, "done", null);
        });
    }

    static void Finish(Db db, long queueId, string status, string? error) =>
        db.Execute(
            @"UPDATE pciworld_media_scan_queue
              SET status=?, last_error=?, lease_owner=NULL, lease_until=NULL,
                  completed_at=datetime('now'), updated_at=datetime('now')
              WHERE id=?",
            status, error, queueId);

    static void Retry(Db db, long queueId, string error) =>
        // H.StampInMinutes, never a literal SQL datetime modifier: Db.Translate only rewrites
        // LITERAL modifiers, so `datetime('now','+' || ? || ' minutes')` silently fails on MySQL.
        // That exact bug has been paid for twice in this increment already.
        db.Execute(
            @"UPDATE pciworld_media_scan_queue
              SET status='retrying', attempts=attempts+1, last_error=?, next_attempt_at=?,
                  lease_owner=NULL, lease_until=NULL, updated_at=datetime('now')
              WHERE id=?",
            error, H.StampInMinutes(2), queueId);
}
