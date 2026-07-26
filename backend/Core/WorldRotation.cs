using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — the daily rotation engine (docs/pciworld/EXPANSION_PHASE0.md §5, §9, §12).
///
/// What this replaces: the daily challenge used to be `servable[DayOfYear % count]`, recomputed on
/// every request. That had four defects this engine exists to fix — it changed mid-day whenever the
/// catalogue changed, it recorded nothing (so "what was featured last Tuesday?" was unanswerable),
/// it repeated across the year boundary, and it could never reach more than ~366 challenges, which
/// at a 10,000-challenge bank meant 96% of the content was unreachable forever.
///
/// The model is a PERIOD LEDGER. One row per rotation day, written once, never updated:
///
///   pciworld_rotation_order    the materialised running order for a cycle (a deterministic,
///                              seeded shuffle of the eligible bank — computed once per cycle,
///                              never recomputed over a live query)
///   pciworld_rotation_periods  the immutable record of what was featured on each day, carrying
///                              its cycle, its position, its source and the version that was live
///   pciworld_rotation_runs     what the scheduler did each time it woke, including when it did
///                              nothing and why
///
/// Invariants:
///   • Idempotent — running the boundary twice creates one period. UNIQUE(day_key, revision) plus
///     INSERT OR IGNORE is the enforcement, not a check-then-write race.
///   • Single-writer — concurrent instances elect one winner through WorkerLease before any period
///     is created, so a scaled deployment cannot double-advance the cycle.
///   • Append-only — a substitution writes a NEW revision for the day and supersedes the previous
///     one; nothing is ever deleted, so the ledger stays auditable.
///   • Learner history is untouched. Rotation decides what is FEATURED. Attempts pin their own
///     (challenge_id, version) and replay from the immutable snapshot regardless of what rotates
///     afterwards — a period boundary crossing an attempt in progress changes nothing about it.
/// </summary>
public static class WorldRotation
{
    public const int MaxCatchUpDays = 60;
    const string LockTable = "pciworld_rotation_lock";

    // ───────────────────────────── configuration ─────────────────────────────

    public static bool Paused(Db db) => !Settings.Bool(db, "world_rotation_enabled", true);
    public static bool ShuffleEnabled(Db db) => Settings.Bool(db, "world_rotation_shuffle", true);

    /// <summary>
    /// The timezone whose midnight is the rotation boundary. Accepts an IANA/Windows id when the
    /// host has timezone data, or a fixed "+HH:MM" offset, and falls back to UTC. It never throws:
    /// a container without tzdata must still rotate rather than fail to serve a daily challenge.
    /// </summary>
    public static TimeZoneInfo Zone(Db db)
    {
        var id = Settings.Str(db, "world_rotation_timezone", "UTC").Trim();
        if (id.Length == 0 || id.Equals("UTC", StringComparison.OrdinalIgnoreCase)) return TimeZoneInfo.Utc;
        try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
        catch { }
        if ((id.StartsWith('+') || id.StartsWith('-')) && TimeSpan.TryParse(id.TrimStart('+'), out var off))
        {
            if (id.StartsWith('-')) off = off.Negate();
            try { return TimeZoneInfo.CreateCustomTimeZone("world-offset", off, "world-offset", "world-offset"); }
            catch { }
        }
        return TimeZoneInfo.Utc;
    }

    /// <summary>The rotation day a UTC instant belongs to, in the configured zone. Kept separate
    /// from Zone() so tests can drive a synthetic zone (including DST rules) deterministically.</summary>
    public static string DayKeyFor(TimeZoneInfo zone, DateTime utc) =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), zone).ToString("yyyy-MM-dd");

    public static string DayKey(Db db, DateTime utcNow) => DayKeyFor(Zone(db), utcNow);

    /// <summary>The UTC instant at which the given rotation day ends (its successor's midnight).
    /// Used to tell participants when today's challenge changes.</summary>
    public static DateTime NextBoundaryUtc(TimeZoneInfo zone, DateTime utc)
    {
        var local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), zone);
        var nextLocalMidnight = local.Date.AddDays(1);
        // Across a DST spring-forward the nominal midnight may not exist; step forward until it does.
        for (var i = 0; i < 4 && zone.IsInvalidTime(nextLocalMidnight); i++) nextLocalMidnight = nextLocalMidnight.AddHours(1);
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(nextLocalMidnight, DateTimeKind.Unspecified), zone);
    }

    // ───────────────────────────── eligibility ─────────────────────────────

    /// <summary>
    /// Challenges the rotation may feature, in id order. Servable means published at least once and
    /// not retired; on top of that the engine excludes explicitly suspended content and content
    /// carrying enough open reports to be considered flagged — a challenge the community keeps
    /// reporting for a calculation error must stop being the thing we put on the front page.
    /// Selection returns ids only: the old implementation loaded every row's full config blob.
    /// </summary>
    public static List<long> Eligible(Db db)
    {
        var threshold = Math.Max(1, (int)Settings.Num(db, "world_rotation_flag_threshold", 3));
        var rows = db.Query(@"SELECT c.id FROM pciworld_challenges c
            WHERE c.current_version>=1 AND c.retired=0 AND c.status<>'suspended'
              AND (SELECT COUNT(*) FROM pciworld_reports r WHERE r.challenge_id=c.id AND r.status='open') < ?
            ORDER BY c.id ASC", threshold);
        return rows.Select(r => H.L(r["id"])).ToList();
    }

    // ───────────────────────────── running order ─────────────────────────────

    /// <summary>
    /// Deterministic per-cycle order. The sort key is SHA-256(cycle:challengeId), so the same cycle
    /// always produces the same order on every instance and after any restart — no PRNG state to
    /// persist and no dependence on row order — while consecutive cycles look unrelated. With
    /// shuffling disabled the order is the catalogue order, which is what an operator who wants a
    /// curated sequence expects.
    /// </summary>
    public static List<long> OrderFor(long cycle, List<long> eligible, bool shuffle, long? avoidFirst)
    {
        var ordered = shuffle
            ? eligible.OrderBy(id => Security.Sha($"{cycle}:{id}"), StringComparer.Ordinal).ToList()
            : new List<long>(eligible);
        // No-immediate-repeat across the cycle boundary: yesterday's challenge must not also be the
        // first of the new cycle. Swapping with the runner-up keeps the order deterministic.
        if (avoidFirst is { } avoid && ordered.Count > 1 && ordered[0] == avoid)
            (ordered[0], ordered[1]) = (ordered[1], ordered[0]);
        return ordered;
    }

    /// <summary>Materialise a cycle's order if it does not exist yet. Returns its length.</summary>
    public static int EnsureOrder(Db db, long cycle, long? avoidFirst)
    {
        var have = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_order WHERE cycle_no=?", cycle);
        if (have > 0) return (int)have;
        var eligible = Eligible(db);
        if (eligible.Count == 0) return 0;
        var ordered = OrderFor(cycle, eligible, ShuffleEnabled(db), avoidFirst);
        for (var i = 0; i < ordered.Count; i++)
            db.Execute("INSERT OR IGNORE INTO pciworld_rotation_order(cycle_no,seq_no,challenge_id) VALUES(?,?,?)",
                cycle, i, ordered[i]);
        return ordered.Count;
    }

    // ───────────────────────────── period ledger ─────────────────────────────

    /// <summary>The period in force for a day: the highest revision written for it. NULL when the
    /// day has not been opened yet.</summary>
    public static Dictionary<string, object?>? ActivePeriod(Db db, string dayKey) =>
        db.QueryOne("SELECT * FROM pciworld_rotation_periods WHERE day_key=? ORDER BY revision DESC LIMIT 1", dayKey);

    public static Dictionary<string, object?>? LastPeriod(Db db) =>
        db.QueryOne("SELECT * FROM pciworld_rotation_periods ORDER BY day_key DESC, revision DESC LIMIT 1");

    public sealed record RunOutcome(int Created, int Skipped, string Detail);

    /// <summary>
    /// Open every rotation day from the last recorded one up to <paramref name="utcNow"/>'s day,
    /// then stop. This IS the catch-up path: a deployment that was down for three days creates the
    /// three missing periods in order, with correct cycle arithmetic, rather than skipping content.
    ///
    /// Safe to call from anywhere and as often as you like: it takes the rotation lease first, so
    /// only one instance advances the ledger, and each day's insert is idempotent regardless.
    /// </summary>
    public static RunOutcome RunDue(Db db, DateTime utcNow)
    {
        var started = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var owner = WorkerLease.NewOwner();
        var today = DayKey(db, utcNow);

        if (Paused(db))
        {
            Record(db, today, "paused", 0, "rotation paused by an administrator", owner, started);
            return new RunOutcome(0, 0, "paused");
        }

        db.Execute("INSERT OR IGNORE INTO pciworld_rotation_lock(id,status) VALUES(1,'queued')");
        WorkerLease.RecoverExpired(db, LockTable, "queued");
        if (!WorkerLease.TryClaim(db, LockTable, 1, owner, "'queued','retrying'", leaseMinutes: 5))
        {
            // Another instance is mid-run. Recording the skip is the point: an operator looking at
            // the run log must be able to tell "nothing happened" from "someone else did it".
            Record(db, today, "skipped_locked", 0, "another instance holds the rotation lease", owner, started);
            return new RunOutcome(0, 1, "locked");
        }

        try
        {
            var last = LastPeriod(db);
            var days = new List<string>();
            if (last is null) days.Add(today);
            else
            {
                var cursor = DateTime.ParseExact(H.Str(last["day_key"])!, "yyyy-MM-dd", null).AddDays(1);
                var end = DateTime.ParseExact(today, "yyyy-MM-dd", null);
                var truncated = 0;
                while (cursor <= end)
                {
                    days.Add(cursor.ToString("yyyy-MM-dd"));
                    cursor = cursor.AddDays(1);
                    if (days.Count > MaxCatchUpDays) { truncated++; break; }
                }
                if (truncated > 0)
                {
                    // Never silently skip: say what was dropped, then jump to today so the product
                    // is current. A gap in the ledger is honest; a fabricated backfill is not.
                    days.Clear();
                    days.Add(today);
                    Record(db, today, "catch_up_truncated", 0,
                        $"more than {MaxCatchUpDays} days missing since {H.Str(last["day_key"])} — jumped to today, the gap is intentional", owner, started);
                }
            }

            var created = 0; var skipped = 0; string? note = null;
            foreach (var day in days)
            {
                var (ok, detail) = OpenPeriod(db, day);
                if (ok) created++; else { skipped++; note ??= detail; }
            }
            var outcome = created > 0 ? (days.Count > 1 ? "catch_up" : "created") : (note ?? "skipped_exists");
            Record(db, today, outcome, created, note, owner, started);
            return new RunOutcome(created, skipped, outcome);
        }
        catch (Exception e)
        {
            Record(db, today, "error", 0, e.Message, owner, started);
            throw;
        }
        finally
        {
            db.Execute($"UPDATE {LockTable} SET status='queued', lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=1");
        }
    }

    /// <summary>
    /// Open one rotation day. Returns (created, detail). Idempotent: a day that already has a period
    /// is left exactly as it is — re-running the boundary never re-picks and never advances the cycle.
    /// </summary>
    static (bool created, string? detail) OpenPeriod(Db db, string dayKey)
    {
        if (ActivePeriod(db, dayKey) is not null) return (false, "skipped_exists");

        var last = LastPeriod(db);
        long cycle = last is null ? 1 : H.L(last["cycle_no"]);
        long nextSeq = last is null ? 0 : H.L(last["seq_no"]) + 1;
        long? previous = last is null ? null : H.L(last["challenge_id"]);

        // An administrator's calendar entry outranks the computed order — but only if the challenge
        // is still eligible. A calendar row pointing at retired content used to fail silently.
        var cal = db.QueryOne("SELECT challenge_id FROM pciworld_calendar WHERE day_utc=?", dayKey);
        if (cal is not null)
        {
            var wanted = H.L(cal["challenge_id"]);
            if (Eligible(db).Contains(wanted))
                return (Insert(db, dayKey, wanted, cycle, nextSeq, "calendar", "scheduled by an administrator", null), null);
            // Fall through to the computed pick, and say so rather than pretending it was honoured.
            Record(db, dayKey, "calendar_ineligible", 0,
                $"calendar entry for {dayKey} points at challenge #{wanted}, which is retired, suspended or flagged — using the rotation order instead",
                "system", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        var len = EnsureOrder(db, cycle, previous);
        if (len == 0) return (false, "no_content");
        if (nextSeq >= len)
        {
            // The cycle is exhausted: start the next one. This is the behaviour the modulo could
            // never have — the bank is consumed in full before anything repeats, at any bank size.
            cycle += 1; nextSeq = 0;
            len = EnsureOrder(db, cycle, previous);
            if (len == 0) return (false, "no_content");
        }

        // Walk forward past anything that became ineligible since the order was materialised.
        for (var probe = nextSeq; probe < len; probe++)
        {
            var row = db.QueryOne("SELECT challenge_id FROM pciworld_rotation_order WHERE cycle_no=? AND seq_no=?", cycle, probe);
            if (row is null) continue;
            var id = H.L(row["challenge_id"]);
            if (!Eligible(db).Contains(id)) continue;
            var reason = probe == nextSeq ? null : $"positions {nextSeq}–{probe - 1} skipped (no longer eligible)";
            return (Insert(db, dayKey, id, cycle, probe, "auto", reason, null), null);
        }
        return (false, "no_content");
    }

    static bool Insert(Db db, string dayKey, long challengeId, long cycle, long seq, string source, string? reason, long? adminId)
    {
        var ch = db.QueryOne("SELECT current_version FROM pciworld_challenges WHERE id=?", challengeId);
        if (ch is null) return false;
        var (_, changes) = db.ExecuteWithChanges(
            @"INSERT OR IGNORE INTO pciworld_rotation_periods(day_key,revision,challenge_id,version,cycle_no,seq_no,source,reason,created_by)
              VALUES(?,1,?,?,?,?,?,?,?)",
            dayKey, challengeId, H.L(ch["current_version"]), cycle, seq, source, reason, adminId);
        return changes > 0;
    }

    /// <summary>
    /// Replace the challenge featured on a day WITHOUT erasing what it replaced: a new revision is
    /// appended and becomes the active period, and the superseded row stays in the ledger. The
    /// previous same-day behaviour (DELETE then INSERT on the calendar) destroyed the record of what
    /// had been displaced, which is exactly what an incident review needs to see.
    /// </summary>
    public static string? Substitute(Db db, string dayKey, long challengeId, long adminId, string? reason)
    {
        if (!Eligible(db).Contains(challengeId)) return "not_eligible";
        var current = ActivePeriod(db, dayKey);
        var ch = db.QueryOne("SELECT current_version FROM pciworld_challenges WHERE id=?", challengeId);
        if (ch is null) return "not_found";
        var revision = current is null ? 1 : H.L(current["revision"]) + 1;
        // Keep the cycle position of the day being replaced so the running order is not corrupted:
        // a substitution changes WHAT is shown, never where the rotation is.
        var cycle = current is null ? 1 : H.L(current["cycle_no"]);
        var seq = current is null ? 0 : H.L(current["seq_no"]);
        db.Execute(@"INSERT INTO pciworld_rotation_periods(day_key,revision,challenge_id,version,cycle_no,seq_no,source,reason,created_by)
                     VALUES(?,?,?,?,?,?, 'substitution', ?, ?)",
            dayKey, revision, challengeId, H.L(ch["current_version"]), cycle, seq, reason, adminId);
        if (current is not null)
            db.Execute("UPDATE pciworld_rotation_periods SET superseded_at=datetime('now') WHERE id=? AND superseded_at IS NULL", current["id"]);
        return null;
    }

    static void Record(Db db, string dayKey, string outcome, int created, string? detail, string owner, long startedMs)
    {
        try
        {
            db.Execute(@"INSERT INTO pciworld_rotation_runs(day_key,outcome,periods_created,detail,owner,duration_ms)
                         VALUES(?,?,?,?,?,?)",
                dayKey, outcome, created, detail, owner,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startedMs);
        }
        catch { }
    }

    // ───────────────────────────── read paths ─────────────────────────────

    /// <summary>
    /// Today's featured challenge row. Reads the ledger; opens today's period on demand if the
    /// scheduler has not yet (first boot, or a request that beats the worker). Stable for the whole
    /// day by construction — publishing or retiring other challenges cannot move it, which is the
    /// property the modulo implementation lacked.
    /// </summary>
    public static Dictionary<string, object?>? Current(Db db, DateTime utcNow)
    {
        var period = CurrentPeriod(db, utcNow);
        return period is null ? null : db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", period["challenge_id"]);
    }

    /// <summary>
    /// The period record in force right now — the AUTHORITY for both the featured challenge id AND
    /// the pinned version it plays. Serving from the challenge's mutable current_version instead of
    /// this row meant a midday publish changed the challenge later participants faced; every read
    /// surface (home, Today API, attempt start) must pin from here.
    ///
    /// Pause semantics: pausing rotation stops the boundary from opening NEW days, it does not take
    /// the product offline. When today has no period because rotation is paused, the previous active
    /// period keeps serving — exactly what the admin copy promises ("the current challenge remains
    /// featured"). Only a ledger with no periods at all serves nothing.
    /// </summary>
    public static Dictionary<string, object?>? CurrentPeriod(Db db, DateTime utcNow)
    {
        var day = DayKey(db, utcNow);
        var period = ActivePeriod(db, day);
        if (period is null)
        {
            try { RunDue(db, utcNow); } catch (Exception e) { Console.Error.WriteLine($"[world-rotation] on-demand open failed: {e.Message}"); }
            period = ActivePeriod(db, day);
        }
        // Paused across the boundary: keep the last featured challenge on the air rather than
        // serving an empty product. The ledger is untouched — resume opens today normally.
        if (period is null && Paused(db)) period = LastPeriod(db);
        if (period is null) return null;

        // A period is a record, not a promise to keep serving broken content: if the featured
        // challenge was retired, suspended or flagged after the day opened, substitute automatically
        // and record why. The ledger keeps the superseded row, so what was displaced is still visible.
        var id = H.L(period["challenge_id"]);
        if (!Eligible(db).Contains(id))
        {
            var replacement = Eligible(db).FirstOrDefault(x => x != id);
            if (replacement == 0) return null;
            Substitute(db, H.Str(period["day_key"])!, replacement, 0, "automatic: the featured challenge is no longer eligible to be served");
            period = ActivePeriod(db, H.Str(period["day_key"])!);
            if (period is null) return null;
        }
        return period;
    }

    /// <summary>What the engine would feature next, for the admin console. Read-only: previewing
    /// must never advance the rotation.</summary>
    public static Dictionary<string, object?>? PeekNext(Db db, DateTime utcNow)
    {
        var last = LastPeriod(db);
        if (last is null) return null;
        var tomorrow = DateTime.ParseExact(DayKey(db, utcNow), "yyyy-MM-dd", null).AddDays(1).ToString("yyyy-MM-dd");
        var cal = db.QueryOne("SELECT challenge_id FROM pciworld_calendar WHERE day_utc=?", tomorrow);
        var eligible = Eligible(db);
        if (cal is not null && eligible.Contains(H.L(cal["challenge_id"])))
            return db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", cal["challenge_id"]);

        var cycle = H.L(last["cycle_no"]);
        var seq = H.L(last["seq_no"]) + 1;
        var len = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_order WHERE cycle_no=?", cycle);
        if (len == 0) return null;
        if (seq >= len) return null;   // next cycle's order is not materialised until the day opens
        for (var probe = seq; probe < len; probe++)
        {
            var row = db.QueryOne("SELECT challenge_id FROM pciworld_rotation_order WHERE cycle_no=? AND seq_no=?", cycle, probe);
            if (row is null) continue;
            if (!eligible.Contains(H.L(row["challenge_id"]))) continue;
            return db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", row["challenge_id"]);
        }
        return null;
    }
}

/// <summary>
/// The rotation boundary worker. Wakes every few minutes rather than at midnight exactly: a
/// timezone-aware boundary, a deployment restart or a clock skew must not be able to miss the day.
/// Every wake is idempotent and lease-guarded, so waking often is free and waking late self-heals
/// through the catch-up path. Follows the platform's ScheduledPublisher/OutboxDispatcher shape —
/// try/catch inside the loop so one bad tick never kills the worker for the life of the process.
/// </summary>
public sealed class WorldRotationService : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);
    public WorldRotationService(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try
            {
                if (Settings.Bool(_db, "world_enabled", true)) WorldRotation.RunDue(_db, DateTime.UtcNow);
            }
            catch (Exception e) { Console.Error.WriteLine($"[world-rotation] sweep failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }
}

/// <summary>
/// PCI World retention sweep. Nothing in the world realm expired before this existed: anonymous
/// sessions accumulated for ever with no `expires_at`, expired account sessions and single-use
/// tokens were never collected, and `pciworld_events` — a row per page view — grew unbounded.
///
/// Keeping a pseudonymous session id indefinitely is keeping personal data indefinitely, so this
/// is a privacy control, not only a housekeeping one. It deletes ONLY continuity and analytics
/// rows: attempts, scores and Passport evidence are learner history and are never touched here.
/// Windows are operator-settable; 0 disables a sweep (the platform's retention convention).
/// </summary>
public sealed class WorldRetentionService : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);
    public WorldRetentionService(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        // Wait a full interval before the first sweep (while, not do-while), like RetentionService:
        // a fresh boot must never delete anything before an operator can review the configuration.
        while (await WaitAsync(timer, stoppingToken))
        {
            try { var n = Sweep(_db); if (n > 0) Console.WriteLine($"[world-retention] removed {n} expired row(s)"); }
            catch (Exception e) { Console.Error.WriteLine($"[world-retention] sweep failed: {e.Message}"); }
        }
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    /// <summary>One sweep. Returns rows removed. Also callable directly by tests.</summary>
    public static int Sweep(Db db)
    {
        var removed = 0;
        // Expired sign-in sessions and used/expired single-use tokens: no reason to keep either.
        removed += db.Execute("DELETE FROM pciworld_user_sessions WHERE expires_at<=datetime('now')");
        removed += db.Execute("DELETE FROM pciworld_admin_sessions WHERE expires_at<=datetime('now')");
        removed += db.Execute("DELETE FROM pciworld_user_tokens WHERE expires_at<=datetime('now')");
        // Handoff codes live two minutes and are single-use — anything expired or consumed is
        // pure residue (P0-02 hygiene: dead security artefacts must not accumulate).
        removed += db.Execute("DELETE FROM pciworld_handoff_codes WHERE expires_at<=datetime('now') OR consumed_at IS NOT NULL");

        // Dormant anonymous sessions. An anonymous session is browser continuity; once it has been
        // idle this long it can only serve to link past activity together. Sessions that still own
        // an attempt are kept — deleting those would orphan learner history.
        var sessionDays = (int)Settings.Num(db, "world_session_retention_days", 180);
        if (sessionDays > 0)
            removed += db.Execute($@"DELETE FROM pciworld_sessions
                WHERE last_seen_at <= datetime('now','-{sessionDays} days')
                  AND id NOT IN (SELECT session_id FROM pciworld_attempts)");

        // Analytics events: the aggregate is what matters, the individual rows are not kept for ever.
        var eventDays = (int)Settings.Num(db, "world_event_retention_days", 400);
        if (eventDays > 0)
            removed += db.Execute($"DELETE FROM pciworld_events WHERE created_at <= datetime('now','-{eventDays} days')");

        return removed;
    }
}
