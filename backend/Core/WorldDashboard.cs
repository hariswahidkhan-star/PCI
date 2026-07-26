using PCI.Backend.Data;
using PCI.Backend.Endpoints;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — the participant dashboard aggregate (journey repair P1-01 / §9 of the product
/// specification). ONE server-side snapshot answers the questions a returning learner actually
/// has — who am I, what should I do next, what is today's state, what's unfinished, what did I
/// complete recently, where does my Passport stand — so the client makes one call instead of an
/// N+1 waterfall, and the "one primary action" rule is computed HERE, not re-derived by every UI.
///
/// Priority order (§9.1, the states that exist today):
///   verify_email → continue_today → view_result → start_today → continue_passport → recommended
/// Exactly one primary action, always.
/// </summary>
public static class WorldDashboard
{
    public sealed record Today(long? PeriodId, string? DayKey, string Timezone, string ChangesAtUtc, bool Paused,
        string? Code, string? Title, string? Difficulty, long? EstMinutes, long Version,
        string State, long? AttemptId);

    public sealed record PassportState(string State, bool Public, string? ExpiresAt, bool EmailVerified,
        bool HasDisplayName, long VisibleEvidence);

    public sealed record Snapshot(string PrimaryAction, long? PrimaryAttemptId, string? PrimaryCode,
        Today TodayState, PassportState Passport, WorldAccount.Stats Progress,
        List<Dictionary<string, object?>> RecentResults, List<Dictionary<string, object?>> InProgress,
        string? RecommendedCode, string? RecommendedTitle, long StreakDays,
        long WeekDone, long? WeekTarget);

    /// <summary>Daily completions over the last seven rotation days (rolling, honest label:
    /// "the last 7 days" — no calendar-week gymnastics), measured against the participant's
    /// chosen weekly target when one is set (PW-US-030/P1-07).</summary>
    public static long WeekDone(Db db, long userId, DateTime utcNow)
    {
        var today = WorldRotation.DayKey(db, utcNow);
        var days = db.Query(@"SELECT p.day_key,
                MAX(CASE WHEN a.id IS NULL THEN 0 ELSE 1 END) AS done
            FROM pciworld_rotation_periods p
            LEFT JOIN pciworld_attempts a ON a.rotation_period_id=p.id
                AND a.user_id=? AND a.status='completed' AND a.parent_attempt_id IS NULL
            WHERE p.day_key<=?
            GROUP BY p.day_key ORDER BY p.day_key DESC LIMIT 7", userId, today);
        return days.Count(r => H.L(r["done"]) == 1);
    }

    /// <summary>
    /// The optional practice streak (§7.5 / PW-US-028), derived from the rotation LEDGER: one
    /// qualifying completion per rotation day — an attempt that recorded its period (a genuine
    /// daily play; invitations and retakes never carry one). Consecutive days counted back from
    /// today; a today not yet completed doesn't break the run mid-day (no-blame grace), and a
    /// substituted period still counts through any of the day's revisions.
    /// </summary>
    public static long Streak(Db db, long userId, DateTime utcNow)
    {
        var today = WorldRotation.DayKey(db, utcNow);
        var days = db.Query(@"SELECT p.day_key,
                MAX(CASE WHEN a.id IS NULL THEN 0 ELSE 1 END) AS done
            FROM pciworld_rotation_periods p
            LEFT JOIN pciworld_attempts a ON a.rotation_period_id=p.id
                AND a.user_id=? AND a.status='completed' AND a.parent_attempt_id IS NULL
            WHERE p.day_key<=?
            GROUP BY p.day_key ORDER BY p.day_key DESC LIMIT 366", userId, today);
        long streak = 0;
        for (var i = 0; i < days.Count; i++)
        {
            var done = H.L(days[i]["done"]) == 1;
            if (done) { streak++; continue; }
            // The only forgivable miss is TODAY, still in progress — and only at the head.
            if (i == 0 && H.Str(days[i]["day_key"]) == today) continue;
            break;
        }
        return streak;
    }

    public static Snapshot Build(Db db, WorldAccount.UserCtx u, DateTime utcNow)
    {
        // ── today, from the one version authority (the rotation period) ──
        var zone = Settings.Str(db, "world_rotation_timezone", "UTC");
        var changesAt = WorldRotation.NextBoundaryUtc(WorldRotation.Zone(db), utcNow).ToString("yyyy-MM-ddTHH:mm:ssZ");
        var paused = WorldRotation.Paused(db);
        var period = WorldRotation.CurrentPeriod(db, utcNow);
        Dictionary<string, object?>? ch = null, snapshot = null;
        if (period is not null)
        {
            ch = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", period["challenge_id"]);
            snapshot = WorldLifecycle.PinnedVersion(db, H.L(period["challenge_id"]), H.L(period["version"]))
                       ?? (ch is null ? null : WorldLifecycle.LiveVersion(db, H.L(period["challenge_id"])));
        }

        var todayState = "not_started";
        long? todayAttempt = null;
        if (period is not null && ch is not null && snapshot is not null)
        {
            // The account's attempt on today's pinned (challenge, version) — daily state is per
            // OWNER, and a completed attempt outranks a stray later in-progress one for state.
            var att = db.QueryOne(@"SELECT id, status FROM pciworld_attempts
                WHERE user_id=? AND challenge_id=? AND version=?
                ORDER BY CASE status WHEN 'completed' THEN 0 ELSE 1 END, id DESC LIMIT 1",
                u.Id, H.L(period["challenge_id"]), H.L(snapshot["version"]));
            if (att is not null)
            {
                todayState = H.Str(att["status"]) == "completed" ? "completed" : "in_progress";
                todayAttempt = H.L(att["id"]);
            }
        }
        var today = new Today(
            period is null ? null : H.L(period["id"]), period is null ? null : H.Str(period["day_key"]),
            zone, changesAt, paused,
            ch is null ? null : H.Str(ch["code"]), snapshot is null ? null : H.Str(snapshot["title"]),
            snapshot is null ? null : H.Str(snapshot["difficulty"]),
            snapshot is null ? null : H.L(snapshot["est_minutes"]),
            snapshot is null ? 0 : H.L(snapshot["version"]),
            todayState, todayAttempt);

        // ── passport readiness (§7.6): private_empty → private_incomplete → private_ready →
        //    published_active | expired ──
        var me = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", u.Id)!;
        var visible = db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_attempts WHERE user_id=? AND status='completed' AND passport_visible=1", u.Id);
        var progress = WorldAccount.EvidenceStats(db, u.Id, visibleOnly: false);
        var expired = WorldPassport.Expired(me);
        var passportState =
            u.PassportPublic && !expired ? "published_active"
            : u.PassportPublic && expired ? "expired"
            : progress.Completed == 0 ? "private_empty"
            : u.EmailVerified && !string.IsNullOrWhiteSpace(u.DisplayName) && visible > 0 ? "private_ready"
            : "private_incomplete";
        var passport = new PassportState(passportState, u.PassportPublic,
            H.Str(me["passport_expires_at"])?.Split(' ')[0], u.EmailVerified,
            !string.IsNullOrWhiteSpace(u.DisplayName), visible);

        // ── recent + unfinished work, account-scoped ──
        var recent = db.Query(@"SELECT a.id, a.score, a.profile_key, a.completed_at, a.passport_visible,
                c.code, a.version, v.title, v.difficulty
            FROM pciworld_attempts a
            JOIN pciworld_challenges c ON c.id=a.challenge_id
            JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
            WHERE a.user_id=? AND a.status='completed'
            ORDER BY a.completed_at DESC, a.id DESC LIMIT 5", u.Id);
        var open = db.Query(@"SELECT a.id, a.updated_at, c.code, a.version, v.title
            FROM pciworld_attempts a
            JOIN pciworld_challenges c ON c.id=a.challenge_id
            JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
            WHERE a.user_id=? AND a.status='in_progress'
            ORDER BY a.updated_at DESC LIMIT 5", u.Id);

        // ── a rule-based recommendation, honestly labelled as such by the client: the next
        //    servable challenge this account has not completed, ordered by the participant's
        //    CHOSEN goal (certification preparation prefers harder material; exploration prefers
        //    industries not yet practised). Still a rule, still says so. ──
        var canonical = WorldIdentity.CanonicalUserFor(db, u.Id);
        var participant = canonical is null ? null
            : db.QueryOne("SELECT goal, weekly_target FROM pciworld_participants WHERE user_id=?", canonical);
        var goal = participant is null ? null : H.Str(participant["goal"]);
        var order = goal switch
        {
            "certification_prep" =>
                "ORDER BY CASE difficulty WHEN 'professional' THEN 0 WHEN 'advanced' THEN 0 WHEN 'expert' THEN 1 ELSE 2 END, id",
            "explore" =>
                @"ORDER BY CASE WHEN industry IN (SELECT v.industry FROM pciworld_attempts a
                    JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
                    WHERE a.user_id=? AND a.status='completed') THEN 1 ELSE 0 END, id",
            _ => "ORDER BY id",
        };
        var recSql = $@"SELECT code, title FROM pciworld_challenges
            WHERE current_version>=1 AND retired=0
              AND id NOT IN (SELECT challenge_id FROM pciworld_attempts WHERE user_id=? AND status='completed')
            {order} LIMIT 1";
        var rec = goal == "explore" ? db.QueryOne(recSql, u.Id, u.Id) : db.QueryOne(recSql, u.Id);

        // ── exactly ONE primary action (§9.1) ──
        string action; long? actionAttempt = null; string? actionCode = today.Code;
        if (!u.EmailVerified) action = "verify_email";
        else if (todayState == "in_progress") { action = "continue_today"; actionAttempt = todayAttempt; }
        else if (todayState == "completed") { action = "view_result"; actionAttempt = todayAttempt; }
        else if (today.Code is not null) action = "start_today";
        else if (passportState is "private_ready" or "private_incomplete") action = "continue_passport";
        else if (rec is not null) { action = "recommended"; actionCode = H.Str(rec["code"]); }
        else action = "browse_archive";

        return new Snapshot(action, actionAttempt, actionCode, today, passport, progress,
            recent, open, rec is null ? null : H.Str(rec["code"]), rec is null ? null : H.Str(rec["title"]),
            Streak(db, u.Id, utcNow),
            WeekDone(db, u.Id, utcNow),
            participant?["weekly_target"] is null ? null : H.L(participant!["weekly_target"]));
    }
}
