using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — daily rotation engine (docs/pciworld/EXPANSION_PHASE0.md §12 acceptance matrix).
///
/// Each test here maps to a numbered acceptance criterion. The engine replaced a stateless
/// `DayOfYear % count`, so these tests exist to prove the four properties that computation could
/// not have: the day is RECORDED, it is IDEMPOTENT, it is SINGLE-WRITER under concurrency, and the
/// whole bank is consumed before anything repeats. The last test is the one that matters most —
/// rotation must never touch a learner's history.
/// </summary>
public class WorldRotationTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static DateTime Utc(int y, int m, int d, int h = 12) => new(y, m, d, h, 0, 0, DateTimeKind.Utc);

    static long PeriodCount(Db db) => db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_periods");
    static long RunCount(Db db, string outcome) =>
        db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_runs WHERE outcome=?", outcome);

    // §12.1 — a boundary run creates exactly one period per day; reruns create zero.
    [Fact]
    public void Boundary_run_is_idempotent()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 8, 1);

        var first = WorldRotation.RunDue(db, day);
        Assert.Equal(1, first.Created);
        Assert.Equal(1, PeriodCount(db));

        for (var i = 0; i < 5; i++)
        {
            var again = WorldRotation.RunDue(db, day);
            Assert.Equal(0, again.Created);
        }
        Assert.Equal(1, PeriodCount(db));
        Assert.True(RunCount(db, "skipped_exists") >= 5, "every no-op run must still be recorded");
    }

    // §12.2 — two concurrent runners: one winner via the lease, identical outcome, a recorded skip.
    [Fact]
    public void Concurrent_runners_elect_one_winner_and_record_the_skip()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 8, 2);

        // Hold the lease as another instance would, then run: this instance must decline to advance
        // the ledger rather than open the day a second time.
        db.Execute("INSERT OR IGNORE INTO pciworld_rotation_lock(id,status) VALUES(1,'queued')");
        Assert.True(WorkerLease.TryClaim(db, "pciworld_rotation_lock", 1, "other-instance", "'queued','retrying'"));

        var blocked = WorldRotation.RunDue(db, day);
        Assert.Equal(0, blocked.Created);
        Assert.Equal("locked", blocked.Detail);
        Assert.Equal(0, PeriodCount(db));
        Assert.Equal(1, RunCount(db, "skipped_locked"));

        // Once the other instance finishes, the next run proceeds normally — and only once.
        db.Execute("UPDATE pciworld_rotation_lock SET status='queued', lease_owner=NULL, lease_until=NULL WHERE id=1");
        Assert.Equal(1, WorldRotation.RunDue(db, day).Created);
        Assert.Equal(0, WorldRotation.RunDue(db, day).Created);
        Assert.Equal(1, PeriodCount(db));
    }

    // §12.3 — downtime is caught up in order, with correct cycle arithmetic.
    [Fact]
    public void Downtime_is_caught_up_in_order()
    {
        var db = NewWorldDb();
        WorldRotation.RunDue(db, Utc(2026, 8, 3));
        Assert.Equal(1, PeriodCount(db));

        // Three days of downtime, then the service comes back.
        var caught = WorldRotation.RunDue(db, Utc(2026, 8, 6));
        Assert.Equal(3, caught.Created);
        Assert.Equal(4, PeriodCount(db));

        var days = db.Query("SELECT day_key, seq_no FROM pciworld_rotation_periods ORDER BY day_key")
                     .Select(r => H.Str(r["day_key"])).ToList();
        Assert.Equal(new[] { "2026-08-03", "2026-08-04", "2026-08-05", "2026-08-06" }, days);

        // Positions advance one per day — the gap consumed content rather than skipping it.
        var seqs = db.Query("SELECT seq_no FROM pciworld_rotation_periods ORDER BY day_key")
                     .Select(r => H.L(r["seq_no"])).ToList();
        Assert.Equal(new long[] { 0, 1, 2, 3 }, seqs);
    }

    // §12.3b — a gap longer than the catch-up bound jumps to today AND says so. Silent truncation
    // would read as "we featured content on those days" when we did not.
    [Fact]
    public void Excessive_downtime_truncates_loudly()
    {
        var db = NewWorldDb();
        WorldRotation.RunDue(db, Utc(2026, 1, 1));
        var resumed = WorldRotation.RunDue(db, Utc(2026, 6, 1));   // ~150 days later

        Assert.Equal(1, resumed.Created);
        Assert.Equal(1, RunCount(db, "catch_up_truncated"));
        Assert.NotNull(WorldRotation.ActivePeriod(db, "2026-06-01"));
        Assert.Null(WorldRotation.ActivePeriod(db, "2026-03-01"));   // the gap is real and visible
    }

    // §12.4 — the bank is consumed in full, then the cycle restarts, reshuffled, with no repeat
    // across the boundary. This is the property the modulo could never have.
    [Fact]
    public void Cycle_wraps_with_a_reshuffle_and_no_immediate_repeat()
    {
        var db = NewWorldDb();
        var bank = WorldRotation.Eligible(db).Count;
        Assert.True(bank >= 30);

        var start = new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc);
        for (var i = 0; i < bank + 2; i++) WorldRotation.RunDue(db, start.AddDays(i));

        var periods = db.Query("SELECT day_key, challenge_id, cycle_no, seq_no FROM pciworld_rotation_periods ORDER BY day_key");
        Assert.Equal(bank + 2, periods.Count);

        // Cycle 1 uses every challenge exactly once — nothing repeats until the bank is exhausted.
        var cycle1 = periods.Where(r => H.L(r["cycle_no"]) == 1).Select(r => H.L(r["challenge_id"])).ToList();
        Assert.Equal(bank, cycle1.Count);
        Assert.Equal(bank, cycle1.Distinct().Count());

        // Cycle 2 opened and is a genuinely different order, and its first pick is not a repeat of
        // the last day of cycle 1.
        var cycle2 = periods.Where(r => H.L(r["cycle_no"]) == 2).Select(r => H.L(r["challenge_id"])).ToList();
        Assert.Equal(2, cycle2.Count);
        Assert.NotEqual(cycle1[^1], cycle2[0]);

        var order1 = db.Query("SELECT challenge_id FROM pciworld_rotation_order WHERE cycle_no=1 ORDER BY seq_no")
                       .Select(r => H.L(r["challenge_id"])).ToList();
        var order2 = db.Query("SELECT challenge_id FROM pciworld_rotation_order WHERE cycle_no=2 ORDER BY seq_no")
                       .Select(r => H.L(r["challenge_id"])).ToList();
        Assert.Equal(order1.OrderBy(x => x), order2.OrderBy(x => x));   // same content...
        Assert.NotEqual(order1, order2);                                // ...different order
    }

    // The order is a pure function of (cycle, ids): the same cycle always materialises identically,
    // on every instance and after any restart. No PRNG state to persist, no dependence on row order.
    [Fact]
    public void Order_is_deterministic_for_a_cycle()
    {
        var ids = Enumerable.Range(1, 40).Select(i => (long)i).ToList();
        var a = WorldRotation.OrderFor(7, ids, shuffle: true, avoidFirst: null);
        var b = WorldRotation.OrderFor(7, ids.OrderByDescending(x => x).ToList(), shuffle: true, avoidFirst: null);
        Assert.Equal(a, b);
        Assert.NotEqual(a, WorldRotation.OrderFor(8, ids, shuffle: true, avoidFirst: null));

        // No-immediate-repeat: if the previous day's challenge would lead, it is swapped down.
        var avoided = WorldRotation.OrderFor(7, ids, shuffle: true, avoidFirst: a[0]);
        Assert.NotEqual(a[0], avoided[0]);
        Assert.Equal(a.OrderBy(x => x), avoided.OrderBy(x => x));

        // Shuffling off means catalogue order, for an operator who wants a curated sequence.
        Assert.Equal(ids, WorldRotation.OrderFor(7, ids, shuffle: false, avoidFirst: null));
    }

    // §12.5 — retired, suspended and heavily-reported challenges are never selected.
    [Fact]
    public void Ineligible_challenges_are_never_selected()
    {
        var db = NewWorldDb();
        var all = WorldRotation.Eligible(db);
        var retired = all[0]; var suspended = all[1]; var flagged = all[2];

        WorldLifecycle.Retire(db, retired);
        db.Execute("UPDATE pciworld_challenges SET status='suspended' WHERE id=?", suspended);
        for (var i = 0; i < 3; i++)
            db.Execute("INSERT INTO pciworld_reports(challenge_id,category,message,status) VALUES(?,'calculation','Reported.','open')", flagged);

        var eligible = WorldRotation.Eligible(db);
        Assert.DoesNotContain(retired, eligible);
        Assert.DoesNotContain(suspended, eligible);
        Assert.DoesNotContain(flagged, eligible);
        Assert.Equal(all.Count - 3, eligible.Count);

        // Resolving the reports makes the flagged challenge featurable again — the exclusion tracks
        // the OPEN queue, it is not a permanent mark.
        db.Execute("UPDATE pciworld_reports SET status='resolved' WHERE challenge_id=?", flagged);
        Assert.Contains(flagged, WorldRotation.Eligible(db));

        // And nothing ineligible reaches the ledger over a full run of days.
        var start = new DateTime(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        for (var i = 0; i < 20; i++) WorldRotation.RunDue(db, start.AddDays(i));
        var featured = db.Query("SELECT DISTINCT challenge_id FROM pciworld_rotation_periods")
                         .Select(r => H.L(r["challenge_id"])).ToList();
        Assert.DoesNotContain(retired, featured);
        Assert.DoesNotContain(suspended, featured);
    }

    // A calendar entry pointing at withdrawn content used to fail silently. It must fall back to
    // the computed order AND leave a record explaining why the schedule was not honoured.
    [Fact]
    public void Ineligible_calendar_entry_falls_back_and_is_recorded()
    {
        var db = NewWorldDb();
        var target = WorldRotation.Eligible(db)[0];
        WorldLifecycle.Retire(db, target);
        db.Execute("INSERT INTO pciworld_calendar(day_utc,challenge_id) VALUES('2026-11-05',?)", target);

        WorldRotation.RunDue(db, Utc(2026, 11, 5));
        var period = WorldRotation.ActivePeriod(db, "2026-11-05")!;
        Assert.NotEqual(target, H.L(period["challenge_id"]));
        Assert.Equal("auto", H.Str(period["source"]));
        Assert.Equal(1, RunCount(db, "calendar_ineligible"));
    }

    // §12.6 — substitution is permissioned at the endpoint, audited, and never corrupts the order:
    // it appends a revision and supersedes, so the ledger still shows what was displaced.
    [Fact]
    public void Substitution_supersedes_without_erasing_and_keeps_the_cycle_position()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 12, 1);
        WorldRotation.RunDue(db, day);
        var before = WorldRotation.ActivePeriod(db, "2026-12-01")!;
        var replacement = WorldRotation.Eligible(db).First(id => id != H.L(before["challenge_id"]));

        Assert.Null(WorldRotation.Substitute(db, "2026-12-01", replacement, adminId: 1, "reported calculation error"));

        var after = WorldRotation.ActivePeriod(db, "2026-12-01")!;
        Assert.Equal(replacement, H.L(after["challenge_id"]));
        Assert.Equal(2L, H.L(after["revision"]));
        Assert.Equal("substitution", H.Str(after["source"]));
        // Position preserved: a substitution changes WHAT is shown, never where the rotation is.
        Assert.Equal(H.L(before["cycle_no"]), H.L(after["cycle_no"]));
        Assert.Equal(H.L(before["seq_no"]), H.L(after["seq_no"]));
        // The displaced day survives, stamped — nothing was deleted.
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_periods WHERE day_key='2026-12-01'"));
        var old = db.QueryOne("SELECT superseded_at FROM pciworld_rotation_periods WHERE day_key='2026-12-01' AND revision=1")!;
        Assert.NotNull(H.Str(old["superseded_at"]));

        // The next day continues from the preserved position, not from the substitution.
        WorldRotation.RunDue(db, day.AddDays(1));
        var next = WorldRotation.ActivePeriod(db, "2026-12-02")!;
        Assert.Equal(H.L(before["seq_no"]) + 1, H.L(next["seq_no"]));

        // Ineligible content cannot be substituted in.
        WorldLifecycle.Retire(db, replacement);
        Assert.Equal("not_eligible", WorldRotation.Substitute(db, "2026-12-02", replacement, 1, "no"));
    }

    // §12.7 — THE product-law test. A period boundary crossing an attempt in progress changes
    // nothing about that attempt: it stays pinned to its own version and replays from the immutable
    // snapshot. Rotation decides what is featured; it has no authority over learner history.
    [Fact]
    public void Rotation_never_alters_an_attempt_that_spans_the_boundary()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 5, 10);
        WorldRotation.RunDue(db, day);
        var featured = WorldRotation.Current(db, day)!;
        var challengeId = H.L(featured["id"]);
        var version = H.L(featured["current_version"]);

        db.Execute("INSERT INTO pciworld_sessions(token_sha) VALUES('rotation-boundary-test')");
        var sessionId = db.Scalar<long>("SELECT id FROM pciworld_sessions WHERE token_sha='rotation-boundary-test'");
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,answers_json)
                     VALUES(?,?,?,'in_progress','{""sv"":-1}')", sessionId, challengeId, version);
        var attemptId = db.Scalar<long>("SELECT id FROM pciworld_attempts WHERE session_id=?", sessionId);
        var before = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", attemptId)!;

        // Cross several boundaries, retire the challenge underneath the attempt, and substitute.
        for (var i = 1; i <= 5; i++) WorldRotation.RunDue(db, day.AddDays(i));
        WorldLifecycle.Retire(db, challengeId);
        WorldRotation.Current(db, day.AddDays(5));

        var after = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", attemptId)!;
        Assert.Equal(H.L(before["challenge_id"]), H.L(after["challenge_id"]));
        Assert.Equal(H.L(before["version"]), H.L(after["version"]));
        Assert.Equal(H.Str(before["answers_json"]), H.Str(after["answers_json"]));
        Assert.Equal(H.Str(before["status"]), H.Str(after["status"]));
        // The pinned snapshot is still replayable even though the challenge is now retired.
        Assert.NotNull(WorldLifecycle.PinnedVersion(db, challengeId, version));
        // And no learner rows were removed by any of it.
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts"));
    }

    // §12.8 — the boundary follows the configured timezone, including across a DST shift. The zone
    // is synthesised rather than read from the host so the test is identical on every machine.
    [Fact]
    public void Boundary_honours_the_configured_timezone_across_dst()
    {
        // UTC+1 in winter, UTC+2 in summer, changing on the last Sunday of March at 01:00 UTC.
        var rule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
            DateTime.MinValue.Date, DateTime.MaxValue.Date, TimeSpan.FromHours(1),
            TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 3, 5, DayOfWeek.Sunday),
            TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0), 10, 5, DayOfWeek.Sunday));
        var zone = TimeZoneInfo.CreateCustomTimeZone("test-dst", TimeSpan.FromHours(1), "test-dst", "test-dst", "test-dst-dt",
            new[] { rule });

        // 23:30 UTC on 15 March is already the 16th locally (UTC+1) — the day key must follow the
        // configured zone, not UTC, or a whole timezone gets yesterday's challenge all evening.
        Assert.Equal("2026-03-16", WorldRotation.DayKeyFor(zone, new DateTime(2026, 3, 15, 23, 30, 0, DateTimeKind.Utc)));
        // In summer the offset is +2, so 22:30 UTC has already rolled over.
        Assert.Equal("2026-07-16", WorldRotation.DayKeyFor(zone, new DateTime(2026, 7, 15, 22, 30, 0, DateTimeKind.Utc)));
        Assert.Equal("2026-07-15", WorldRotation.DayKeyFor(zone, new DateTime(2026, 7, 15, 21, 30, 0, DateTimeKind.Utc)));

        // The advertised next boundary is a real instant, and it moves with the offset.
        var winterNext = WorldRotation.NextBoundaryUtc(zone, new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc));
        Assert.Equal(new DateTime(2026, 1, 15, 23, 0, 0, DateTimeKind.Utc), winterNext);
        var summerNext = WorldRotation.NextBoundaryUtc(zone, new DateTime(2026, 7, 15, 12, 0, 0, DateTimeKind.Utc));
        Assert.Equal(new DateTime(2026, 7, 15, 22, 0, 0, DateTimeKind.Utc), summerNext);
    }

    // An unresolvable timezone must degrade to UTC rather than throw: a container without tzdata
    // still has to serve a daily challenge.
    [Fact]
    public void Unresolvable_timezone_falls_back_to_utc()
    {
        var db = NewWorldDb();
        Settings.Put(db, "world_rotation_timezone", "Not/AZone");
        Assert.Equal(TimeZoneInfo.Utc.Id, WorldRotation.Zone(db).Id);
        Assert.Equal("2026-04-02", WorldRotation.DayKey(db, Utc(2026, 4, 2)));

        Settings.Put(db, "world_rotation_timezone", "+05:30");
        Assert.Equal(TimeSpan.FromMinutes(330), WorldRotation.Zone(db).BaseUtcOffset);
        Assert.Equal("2026-04-03", WorldRotation.DayKey(db, new DateTime(2026, 4, 2, 19, 0, 0, DateTimeKind.Utc)));
    }

    // §12.9 — pausing freezes the rotation without taking the product down, and records the reason.
    [Fact]
    public void Pausing_freezes_the_rotation_and_is_recorded()
    {
        var db = NewWorldDb();
        WorldRotation.RunDue(db, Utc(2026, 2, 10));
        var frozen = WorldRotation.ActivePeriod(db, "2026-02-10")!;

        Settings.Put(db, "world_rotation_enabled", "0");
        Assert.True(WorldRotation.Paused(db));
        Assert.Equal(0, WorldRotation.RunDue(db, Utc(2026, 2, 11)).Created);
        Assert.Null(WorldRotation.ActivePeriod(db, "2026-02-11"));
        Assert.Equal(1, RunCount(db, "paused"));
        // The already-open day keeps serving — pausing is not an outage.
        Assert.Equal(H.L(frozen["challenge_id"]), H.L(WorldRotation.ActivePeriod(db, "2026-02-10")!["challenge_id"]));

        Settings.Put(db, "world_rotation_enabled", "1");
        Assert.True(WorldRotation.RunDue(db, Utc(2026, 2, 11)).Created > 0);
    }

    // The read path must open the day on demand (first boot, or a request that beats the worker)
    // and must never be the thing that makes the selection unstable.
    [Fact]
    public void Read_path_opens_the_day_on_demand_and_stays_stable()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 3, 20);
        Assert.Equal(0, PeriodCount(db));

        var first = WorldRotation.Current(db, day)!;
        Assert.Equal(1, PeriodCount(db));

        // Publishing more content mid-day cannot move today's challenge — the defect that made the
        // old modulo unusable, since every publish reshuffled every ordinal.
        for (var i = 0; i < 5; i++) Assert.Equal(H.L(first["id"]), H.L(WorldRotation.Current(db, day)!["id"]));
        db.Execute(@"INSERT INTO pciworld_challenges(code,title,status,current_version,retired,synthetic_declared)
                     VALUES('WC-NEW-999','Late arrival','published',1,0,1)");
        Assert.Equal(H.L(first["id"]), H.L(WorldRotation.Current(db, day)!["id"]));
        Assert.Equal(1, PeriodCount(db));
    }

    // An empty bank must be reported, not crashed on or papered over.
    [Fact]
    public void Empty_bank_reports_no_content()
    {
        var db = NewWorldDb();
        db.Execute("UPDATE pciworld_challenges SET retired=1");
        var run = WorldRotation.RunDue(db, Utc(2026, 4, 4));
        Assert.Equal(0, run.Created);
        Assert.Equal(0, PeriodCount(db));
        Assert.Null(WorldRotation.Current(db, Utc(2026, 4, 4)));
        // Both the scheduled run and the read path's on-demand attempt record the reason.
        Assert.True(RunCount(db, "no_content") >= 1, "an empty bank must be recorded, not silent");
    }
}
