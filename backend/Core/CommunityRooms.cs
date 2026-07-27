using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — rooms, guest sessions and the message accept path
/// (docs/pciworld/CCP_PHASE1_DESIGN.md §2–§4).
///
/// This is where the slice's safety guarantee is actually kept. The policy engine decides WHETHER
/// text may be published; this decides HOW that verdict is committed, and the difference matters:
/// a correct verdict written non-atomically still leaks.
///
/// THREE INVARIANTS, each with a specific failure it prevents:
///
///   1. A message row and its moderation decision commit in ONE transaction. There is no window in
///      which a message exists without a verdict, so a crash between the two cannot leave a row that
///      a later reader mistakes for approved.
///
///   2. A room sequence is allocated INSIDE that transaction, by a conditional UPDATE that only one
///      writer can win — the same rows-affected discipline WorkerLease uses. Two concurrent accepts
///      cannot receive the same sequence, and a sequence is never consumed by a message that was not
///      published. Ordered reconnect recovery depends entirely on this.
///
///   3. The broadcast event is written to the outbox in that same transaction, but DELIVERED after
///      commit. Delivery is never part of the database transaction (§28.18), so a slow or dead
///      broadcast cannot roll back an accepted message, and a message that survived the commit is
///      recoverable by sequence even if every broadcast attempt failed.
///
/// Read <see cref="Accept"/> as the single entry point. Nothing else may write a published message.
/// </summary>
public static class CommunityRooms
{
    /// <summary>Whether the community feature is switched on at all. Seeded false; see CCP-P1-003
    /// (legal prerequisites) and CCP-P1-004 (moderation provider) before enabling.</summary>
    public static bool Enabled(Db db) => Settings.Bool(db, "world_community_enabled", false);

    // ── Rooms ─────────────────────────────────────────────────────────────────────────────────

    /// <summary>Rooms a visitor may see listed. Draft and archived rooms are not discoverable, and
    /// an undiscoverable room stays reachable by direct slug for a linked/scheduled session.</summary>
    public static List<Dictionary<string, object?>> Discoverable(Db db) =>
        db.Query(@"SELECT id,slug,title,description,topic,category,locale,room_type,state,capacity,
                          guest_allowed,slow_mode_seconds,pinned_welcome,opens_at,closes_at,timezone
                   FROM pciworld_community_rooms
                   WHERE discoverable=1 AND state IN ('scheduled','open','slow_mode','read_only')
                   ORDER BY state='open' DESC, title");

    public static Dictionary<string, object?>? BySlug(Db db, string slug) =>
        db.QueryOne("SELECT * FROM pciworld_community_rooms WHERE slug=?", slug);

    /// <summary>Whether a room currently accepts new messages. Emergency state is checked FIRST and
    /// independently of the schedule: a kill switch must win over an open room, and must not be
    /// undone by the room's own opening hours.</summary>
    public static bool AcceptsMessages(Dictionary<string, object?> room)
    {
        if (H.Str(room["emergency_state"]) is { Length: > 0 }) return false;
        return H.Str(room["state"]) is "open" or "slow_mode";
    }

    /// <summary>Live participant count — active guest sessions only. Presence is a count, never a
    /// roster: §8.2 requires truthful counts without exposing who is present.</summary>
    public static long Presence(Db db, long roomId) =>
        db.Scalar<long>("SELECT COUNT(*) FROM pciworld_guest_sessions WHERE room_id=? AND status='active'", roomId);

    // ── Guest sessions ────────────────────────────────────────────────────────────────────────

    public readonly record struct JoinResult(bool Ok, string? ErrorCode, string? Token, long SessionId, string? Suggestion)
    {
        public static JoinResult Fail(string code, string? suggestion = null) => new(false, code, null, 0, suggestion);
    }

    /// <summary>
    /// Issue a guest session for a room. The display name is validated by <see cref="CommunityNames"/>
    /// (pure rules), then uniqueness among ACTIVE participants is enforced by the database, not by a
    /// read-then-write — two visitors typing the same name at once must not both get in.
    ///
    /// The returned token is random and stored only as SHA-256, matching every other opaque token in
    /// this product. A typed display name is not an identity; this token is.
    /// </summary>
    public static JoinResult Join(Db db, long roomId, string? rawName, string rulesVersion,
                                  string? riskKey, string locale = "en", int sessionHours = 12)
    {
        var verdict = CommunityNames.Validate(rawName);
        if (!verdict.Ok) return JoinResult.Fail(verdict.ReasonCode!, verdict.Suggestion);

        // A risk key under an unexpired restriction cannot re-enter. Ban evasion by clearing a
        // cookie or switching network remains possible and is documented as such (§7.1) — this is a
        // deterrent, not enforcement, and must never be described as more.
        if (riskKey is { Length: > 0 } && IsRestricted(db, riskKey, roomId))
            return JoinResult.Fail("access_restricted");

        var token = Security.RandomHex(32);
        var folded = verdict.Folded!;
        // The expiry is computed HERE and bound as a value, never written as
        // datetime('now','+' || ? || ' hours'). Db.Translate rewrites only the LITERAL datetime form
        // for MySQL by regex, so a modifier arriving as a bound parameter is invisible to it and the
        // statement reaches MariaDB as SQLite syntax it cannot evaluate. SQLite accepts it happily,
        // which is exactly what makes this class of bug provider-specific and easy to miss — the
        // repository's own TestEnv.Stamp carries the same warning.
        var expiresAt = DateTime.UtcNow.AddHours(Math.Clamp(sessionHours, 1, 24))
                                       .ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            var id = db.ExecuteReturningId(
                @"INSERT INTO pciworld_guest_sessions
                      (token_sha,room_id,display_name,display_name_folded,active_name_key,locale,
                       rules_version_accepted,risk_key,status,expires_at)
                  VALUES(?,?,?,?,?,?,?,?,'active',?)",
                Security.Sha(token), roomId, verdict.Display, folded, folded, locale,
                rulesVersion, riskKey, expiresAt);
            return new JoinResult(true, null, token, id, null);
        }
        catch (Exception)
        {
            // The unique index on (room_id, active_name_key) is the authority for "name taken", so
            // a failed insert USUALLY means another session won the race. But only usually — a
            // blanket catch here would report every database fault as "name_taken", which is how a
            // MySQL-only datetime bug first presented itself as a phantom duplicate name and hid
            // its real cause. Confirm the name is genuinely held before claiming that; otherwise
            // let the real error surface.
            var taken = db.Scalar<long>(
                "SELECT COUNT(*) FROM pciworld_guest_sessions WHERE room_id=? AND active_name_key=?",
                roomId, folded) > 0;
            if (!taken) throw;
            return JoinResult.Fail("name_taken", CommunityNames.SuggestAlternative(verdict.Display!, 2));
        }
    }

    public static Dictionary<string, object?>? SessionByToken(Db db, string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        return db.QueryOne(
            @"SELECT * FROM pciworld_guest_sessions
              WHERE token_sha=? AND status='active'
                AND (expires_at IS NULL OR expires_at > datetime('now'))", Security.Sha(token));
    }

    /// <summary>End a session and release its display name for reuse. Clearing active_name_key is
    /// what frees the name — see the index note in CommunitySchema.</summary>
    public static void EndSession(Db db, long sessionId, string status, string reason)
        => db.Execute(@"UPDATE pciworld_guest_sessions
                        SET status=?, active_name_key=NULL, ended_at=datetime('now'), ended_reason=?
                        WHERE id=? AND status='active'", status, reason, sessionId);

    public static bool IsRestricted(Db db, string riskKey, long roomId) =>
        db.Scalar<long>(
            @"SELECT COUNT(*) FROM pciworld_risk_restrictions
              WHERE risk_key=? AND (expires_at IS NULL OR expires_at > datetime('now'))
                AND (scope='world' OR room_id=?)", riskKey, roomId) > 0;

    // ── The accept path ───────────────────────────────────────────────────────────────────────

    public readonly record struct AcceptResult(
        bool Published, long MessageId, long? Sequence, string Status, string ReasonCode, bool Ejected)
    {
        public static AcceptResult Rejected(string reason) => new(false, 0, null, "rejected", reason, false);
    }

    /// <summary>
    /// The ONLY way a message enters a room. Moderates first, then commits the message, its decision
    /// and (when published) its sequence and broadcast event atomically.
    ///
    /// Ordering note: moderation runs BEFORE the transaction opens, deliberately. A classifier call
    /// is network-bound and can take seconds; holding a write transaction across it would serialise
    /// every room behind the slowest provider response. Nothing is persisted before the verdict, so
    /// there is no partial state to leak.
    /// </summary>
    public static AcceptResult Accept(Db db, Dictionary<string, object?> room, long? guestSessionId,
                                      long? worldUserId, string clientMessageId, string body,
                                      CommunityModeration.ITextModerator moderator,
                                      IReadOnlyList<CommunityModeration.Rule> policy,
                                      long? replyTo = null, int repetition = 0,
                                      string? correlationId = null)
    {
        var roomId = H.L(room["id"]);

        // Idempotency BEFORE any work: a retried request or a double click must return the original
        // outcome, not moderate and store a second copy. The unique index is the backstop; this is
        // the fast path that keeps a retry cheap.
        var existing = db.QueryOne(
            "SELECT id,sequence,status FROM pciworld_community_messages WHERE room_id=? AND client_message_id=?",
            roomId, clientMessageId);
        if (existing is not null)
        {
            var st = H.Str(existing["status"]) ?? "pending";
            return new AcceptResult(st == "allowed", H.L(existing["id"]),
                                    existing["sequence"] is null ? null : H.L(existing["sequence"]),
                                    st, "duplicate", false);
        }

        if (!AcceptsMessages(room)) return AcceptResult.Rejected("room_not_accepting");
        var trimmed = (body ?? "").Trim();
        if (trimmed.Length == 0) return AcceptResult.Rejected("empty_message");
        if (trimmed.Length > 2000) return AcceptResult.Rejected("message_too_long");

        // Moderate. A moderator that throws is treated as unavailable rather than crashing the send
        // path — an exception here would lose the message entirely, which is worse than failing closed.
        CommunityModeration.Signal signal;
        try { signal = moderator.Classify(trimmed, H.Str(room["locale"]) ?? "en"); }
        catch { signal = CommunityModeration.Signal.Unavailable; }

        var decision = CommunityModeration.Resolve(policy, signal, repetition);
        var status = decision.Outcome switch
        {
            CommunityModeration.Outcome.Allow => "allowed",
            CommunityModeration.Outcome.Quarantine => "quarantined",
            _ => "blocked",
        };
        var ejected = decision.Outcome is CommunityModeration.Outcome.Eject;

        long messageId = 0, decisionId = 0;
        long? sequence = null;

        db.Transaction(() =>
        {
            // A sequence is allocated ONLY for a message that is actually publishing, and only by a
            // writer that wins the conditional update. Reading next_sequence and then writing it
            // back would let two accepts read the same value; incrementing and returning the prior
            // value under the row lock cannot.
            if (decision.Publishes)
            {
                var current = db.Scalar<long>("SELECT next_sequence FROM pciworld_community_rooms WHERE id=?", roomId);
                var changed = db.Execute(
                    "UPDATE pciworld_community_rooms SET next_sequence=next_sequence+1 WHERE id=? AND next_sequence=?",
                    roomId, current);
                if (changed != 1) throw new InvalidOperationException("sequence_contended");
                sequence = current;
            }

            messageId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_community_messages
                      (room_id,sequence,guest_session_id,world_user_id,client_message_id,body,
                       body_normalized,reply_to_message_id,status,published_at)
                  VALUES(?,?,?,?,?,?,?,?,?,?)",
                roomId, sequence, guestSessionId, worldUserId, clientMessageId, trimmed,
                CommunityNames.FoldWords(trimmed), replyTo, status,
                decision.Publishes ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") : null);

            // The decision carries a HASH of the content, never the content. Blocked and quarantined
            // text must not be copied into audit rows or telemetry (§8.7); the message row already
            // holds the body under the room's retention class.
            decisionId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_moderation_decisions
                      (scope,subject_ref,content_hash,policy_version_id,rule_id,provider,provider_version,
                       category,severity,confidence_band,context_rule,repetition_count,outcome,reason_code,correlation_id)
                  VALUES('message',?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                messageId.ToString(), Security.Sha(trimmed), room["policy_version_id"], decision.RuleId,
                moderator.ProviderName, moderator.ProviderVersion, decision.Category, signal.Severity,
                decision.Confidence.ToString().ToLowerInvariant(), decision.ContextRule, repetition,
                decision.Outcome.ToString().ToLowerInvariant(), decision.ReasonCode, correlationId);

            db.Execute("UPDATE pciworld_community_messages SET decision_id=? WHERE id=?", decisionId, messageId);

            // The broadcast event is written here and delivered later. Only a published message
            // produces one — there is nothing to broadcast about text that was never published.
            if (decision.Publishes)
                db.Execute(
                    @"INSERT INTO pciworld_community_outbox(event_type,room_id,payload_json,correlation_id)
                      VALUES('CommunityMessageAllowed',?,?,?)",
                    roomId,
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        messageId,
                        roomId,
                        sequence,
                        clientMessageId,
                    }),
                    correlationId);

            if (guestSessionId is { } gsid)
                db.Execute(
                    "UPDATE pciworld_guest_sessions SET message_count=message_count+1, last_message_at=datetime('now') WHERE id=?",
                    gsid);
        });

        return new AcceptResult(decision.Publishes, messageId, sequence, status, decision.ReasonCode, ejected);
    }

    /// <summary>
    /// The ordered history a client replays after reconnecting. Published messages only, strictly
    /// after the sequence the client last acknowledged — so a dropped broadcast, a crashed worker or
    /// a node restart all converge on one history rather than a gap (§8.3, PW-US-036).
    /// </summary>
    public static List<Dictionary<string, object?>> Since(Db db, long roomId, long afterSequence, int limit = 200)
        => db.Query(
            @"SELECT m.id, m.sequence, m.body, m.reply_to_message_id, m.published_at,
                     g.display_name AS author_name
              FROM pciworld_community_messages m
              LEFT JOIN pciworld_guest_sessions g ON g.id = m.guest_session_id
              WHERE m.room_id=? AND m.status='allowed' AND m.sequence > ?
              ORDER BY m.sequence
              LIMIT ?", roomId, afterSequence, Math.Clamp(limit, 1, 500));
}
