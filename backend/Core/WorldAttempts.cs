using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — attempt lifecycle core (start / submit), extracted from the endpoint lambdas so the
/// ownership, idempotency and version-pinning rules are directly testable.
///
/// The rules this class owns (journey-repair P0-01 / P0-04 / P0-05):
///
///   • OWNERSHIP AT CREATION. An attempt started while signed in carries the account's user_id from
///     the INSERT — never "claimed later by the next login". Resuming an unowned in-progress attempt
///     from the same anonymous session while signed in adopts it (claim-only-unowned; ownership is
///     never reassigned from one account to another).
///
///   • COMPLETED IS TERMINAL AND RECOVERABLE. Starting a challenge whose latest attempt is already
///     completed returns that completed attempt instead of silently creating a duplicate — the
///     lost-response reload journey. A fresh attempt after completion is an explicit retake, which
///     records its parent for lineage.
///
///   • IDEMPOTENT SUBMIT. Re-submitting a completed attempt with the same payload returns the stored
///     result (the network-lost-the-response retry). A DIFFERENT payload against a completed attempt
///     is refused: results are immutable once graded.
///
///   • DAILY VERSION PIN. Starting today's featured challenge plays the version the rotation period
///     pinned when the day opened, not the challenge's mutable current_version — a midday publish
///     never changes the challenge later participants face. Invitations continue to pin the exact
///     version the inviter played, which outranks the daily pin.
/// </summary>
public static class WorldAttempts
{
    public sealed record StartOutcome(string? Error, long AttemptId, bool Resumed, bool Completed,
        string? AnswersJson, long ChallengeId, long Version);

    public sealed record SubmitOutcome(string? Error, Dictionary<string, object?>? Attempt, bool Replayed);

    /// <summary>Start (or resume, or recover) an attempt. <paramref name="userId"/> is the signed-in
    /// PCI World account, when the request carried one — attempts it creates are owned immediately.</summary>
    public static StartOutcome Start(Db db, long sessionId, long? userId, string code, string? inviteToken, bool retake, DateTime utcNow)
    {
        var ch = db.QueryOne("SELECT * FROM pciworld_challenges WHERE code=? AND current_version>=1 AND retired=0", code);
        if (ch is null) return new("not_found", 0, false, false, null, 0, 0);
        var challengeId = H.L(ch["id"]);
        var version = H.L(ch["current_version"]);

        // Daily pin: when this is today's featured challenge, play the version the period pinned at
        // the boundary — the rotation ledger is the authority, not the mutable working copy.
        var period = WorldRotation.CurrentPeriod(db, utcNow);
        if (period is not null && H.L(period["challenge_id"]) == challengeId &&
            WorldLifecycle.PinnedVersion(db, challengeId, H.L(period["version"])) is not null)
            version = H.L(period["version"]);

        long? inviteId = null;
        if (!string.IsNullOrEmpty(inviteToken))
        {
            var inv = db.QueryOne(@"SELECT i.id, a.challenge_id, a.version FROM pciworld_invites i
                JOIN pciworld_attempts a ON a.id=i.attempt_id
                WHERE i.token_sha=? AND i.revoked=0", Security.Sha(inviteToken));
            if (inv is not null && H.L(inv["challenge_id"]) == challengeId)
            { version = H.L(inv["version"]); inviteId = H.L(inv["id"]); }
        }
        if (WorldLifecycle.PinnedVersion(db, challengeId, version) is null)
            return new("not_found", 0, false, false, null, 0, 0);

        // Resume an in-progress attempt: this browser session's, or (cross-device) the account's.
        var existing = db.QueryOne($@"SELECT * FROM pciworld_attempts
            WHERE challenge_id=? AND version=? AND status='in_progress'
              AND (session_id=?{(userId is null ? "" : " OR user_id=?")})
            ORDER BY id DESC LIMIT 1",
            userId is null
                ? new object?[] { challengeId, version, sessionId }
                : new object?[] { challengeId, version, sessionId, userId });
        if (existing is not null)
        {
            // Adopt-on-resume: an unowned attempt from this session becomes the account's the moment
            // the signed-in owner touches it. Claim-only-unowned — never reassign between accounts.
            if (userId is not null && existing["user_id"] is null)
                db.Execute("UPDATE pciworld_attempts SET user_id=? WHERE id=? AND user_id IS NULL", userId, existing["id"]);
            return new(null, H.L(existing["id"]), true, false, H.Str(existing["answers_json"]), challengeId, version);
        }

        // No in-progress work. If the latest attempt here is already completed, return IT rather
        // than silently opening a duplicate — the reload-after-lost-response journey. A new attempt
        // after completion is an explicit retake, linked to its parent.
        var completed = db.QueryOne($@"SELECT * FROM pciworld_attempts
            WHERE challenge_id=? AND version=? AND status='completed'
              AND (session_id=?{(userId is null ? "" : " OR user_id=?")})
            ORDER BY id DESC LIMIT 1",
            userId is null
                ? new object?[] { challengeId, version, sessionId }
                : new object?[] { challengeId, version, sessionId, userId });
        if (completed is not null && !retake)
            return new(null, H.L(completed["id"]), true, true, H.Str(completed["answers_json"]), challengeId, version);

        var attemptId = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,invite_id,answers_json,user_id,parent_attempt_id)
            VALUES(?,?,?,?, '{}', ?, ?)",
            sessionId, challengeId, version, inviteId, userId,
            retake && completed is not null ? H.L(completed["id"]) : (long?)null);
        return new(null, attemptId, false, false, null, challengeId, version);
    }

    /// <summary>Submit an attempt. Ownership is the session that started it or the account that owns
    /// it. Idempotent: a repeat of the same payload against a completed attempt replays the stored
    /// result; a different payload is refused — graded results are immutable.</summary>
    public static SubmitOutcome Submit(Db db, long sessionId, long? userId, long attemptId, JsonElement answersEl)
    {
        var att = Owned(db, sessionId, userId, attemptId);
        if (att is null) return new("not_found", null, false);
        var answersRaw = answersEl.ValueKind == JsonValueKind.Object ? answersEl.GetRawText() : "{}";

        if (H.Str(att["status"]) == "completed") return CompletedReplay(att, answersRaw);

        var snapshot = WorldLifecycle.PinnedVersion(db, H.L(att["challenge_id"]), H.L(att["version"]));
        if (snapshot is null) return new("not_found", null, false);

        var config = JsonDocument.Parse(H.Str(snapshot["config_json"])!).RootElement;
        var r = WorldScore.Grade(config, answersEl);
        var dims = JsonSerializer.Serialize(new { calculation = r.Calculation, decision = r.Decision });

        // First submit wins the UPDATE; a concurrent duplicate lands in the 0-rows branch below and
        // is answered from the stored row instead of with a dead-end 409.
        var n = db.Execute(@"UPDATE pciworld_attempts SET status='completed', answers_json=?, score=?,
                dimensions_json=?, profile_key=?, completed_at=datetime('now'), updated_at=datetime('now')
            WHERE id=? AND status='in_progress'",
            answersRaw, r.Score, dims, r.ProfileKey, attemptId);
        if (n == 0)
        {
            var raced = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", attemptId);
            return raced is null || H.Str(raced["status"]) != "completed"
                ? new("not_found", null, false)
                : CompletedReplay(raced, answersRaw);
        }
        // Ownership at completion: a signed-in submit of a still-unowned attempt adopts it, so work
        // finished mid-sign-in never goes missing from the account.
        if (userId is not null && att["user_id"] is null)
            db.Execute("UPDATE pciworld_attempts SET user_id=? WHERE id=? AND user_id IS NULL", userId, attemptId);
        return new(null, db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", attemptId), false);
    }

    /// <summary>The attempt, when the caller may act on it: the session that started it, or the
    /// signed-in account that owns it (cross-device). A client-supplied id alone is never authority.</summary>
    public static Dictionary<string, object?>? Owned(Db db, long sessionId, long? userId, long attemptId) =>
        db.QueryOne($@"SELECT * FROM pciworld_attempts
            WHERE id=? AND (session_id=?{(userId is null ? "" : " OR user_id=?")})",
            userId is null
                ? new object?[] { attemptId, sessionId }
                : new object?[] { attemptId, sessionId, userId });

    static SubmitOutcome CompletedReplay(Dictionary<string, object?> att, string answersRaw)
    {
        var stored = H.Str(att["answers_json"]) ?? "{}";
        // The same payload (or an empty retry) is the lost-response journey: replay the stored,
        // immutable result. A different payload is an attempt to change a graded result: refused.
        return answersRaw == "{}" || JsonEquivalent(stored, answersRaw)
            ? new(null, att, true)
            : new("already_submitted", null, false);
    }

    /// <summary>Structural JSON equality — a retry serialized by a different client must not be
    /// refused over key order or whitespace.</summary>
    public static bool JsonEquivalent(string a, string b)
    {
        if (a == b) return true;
        try
        {
            using var da = JsonDocument.Parse(a);
            using var db_ = JsonDocument.Parse(b);
            return ElementEquals(da.RootElement, db_.RootElement);
        }
        catch { return false; }
    }

    static bool ElementEquals(JsonElement a, JsonElement b)
    {
        if (a.ValueKind != b.ValueKind) return false;
        switch (a.ValueKind)
        {
            case JsonValueKind.Object:
                var propsA = a.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);
                var propsB = b.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);
                return propsA.Count == propsB.Count &&
                       propsA.All(kv => propsB.TryGetValue(kv.Key, out var v) && ElementEquals(kv.Value, v));
            case JsonValueKind.Array:
                var arrA = a.EnumerateArray().ToList();
                var arrB = b.EnumerateArray().ToList();
                return arrA.Count == arrB.Count && arrA.Zip(arrB).All(p => ElementEquals(p.First, p.Second));
            default:
                return a.GetRawText() == b.GetRawText();
        }
    }
}
