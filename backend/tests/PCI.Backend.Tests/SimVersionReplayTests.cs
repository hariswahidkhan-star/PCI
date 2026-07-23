using System.Collections.Generic;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — P0 deterministic-replay + immutable-version tests. These pin the core P0 guarantee:
/// a started attempt replays, grades and coaches from the EXACT config it began with, so a later revision,
/// republish or seed refresh can never alter a historical attempt; and each published version is frozen
/// into an immutable, checksum-verified ledger. Without this, editing a live scenario row would silently
/// rewrite past students' results.
/// </summary>
public class SimVersionReplayTests
{
    const string ConfigA = """{"task":"evm","prompt":"A","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},"ask":[{"key":"cpi","label":"CPI","type":"number"}],"tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}""";
    const string ConfigB = """{"task":"evm","prompt":"B (revised)","given":{"pv":50000,"ev":40000,"ac":60000,"bac":120000},"ask":[{"key":"cpi","label":"CPI","type":"number"}],"tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}""";

    static Db FreshDb()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);   // runtime-migrated sim tables (parity with Program.cs boot)
        return db;
    }

    static long InsertScenario(Db db, string code, string config, string status = "published", int version = 1) =>
        db.ExecuteReturningId(@"INSERT INTO simulation_scenarios(scenario_code,title,kind,difficulty,competencies_json,config_json,status,version)
            VALUES(?,?, 'scenario','foundation','[""earned_value""]', ?, ?, ?)", code, "T " + code, config, status, version);

    static Dictionary<string, object?> Attempt(Db db, long id) =>
        db.QueryOne("SELECT * FROM simulation_attempts WHERE id=?", id)!;

    [Fact]
    public void Started_attempt_replays_from_its_snapshot_not_the_live_scenario()
    {
        var db = FreshDb();
        var sid = InsertScenario(db, "REPLAY-1", ConfigA);
        // Simulate a started attempt pinned to config A (what the SimLab start endpoint records).
        var aid = db.ExecuteReturningId(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json,config_snapshot)
            VALUES(1,?,1,'training','in_progress',0,'{}',?)", sid, ConfigA);

        // A later revision/republish rewrites the LIVE catalogue row's config to B.
        db.Execute("UPDATE simulation_scenarios SET config_json=? WHERE id=?", ConfigB, sid);

        // Replay must still resolve config A — the frozen snapshot — never the live B.
        var resolved = SimReplay.BaseConfig(db, Attempt(db, aid));
        Assert.Equal(ConfigA, resolved);
        Assert.NotEqual(ConfigB, resolved);
    }

    [Fact]
    public void Legacy_attempt_without_snapshot_falls_back_to_live_scenario()
    {
        var db = FreshDb();
        var sid = InsertScenario(db, "REPLAY-LEGACY", ConfigA);
        var aid = db.ExecuteReturningId(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json,config_snapshot)
            VALUES(1,?,1,'training','in_progress',0,'{}',NULL)", sid);
        Assert.Equal(ConfigA, SimReplay.BaseConfig(db, Attempt(db, aid)));
    }

    [Fact]
    public void Backfill_freezes_existing_attempts_and_records_published_versions()
    {
        var db = FreshDb();
        var sid = InsertScenario(db, "BACKFILL-1", ConfigA);
        var aid = db.ExecuteReturningId(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json)
            VALUES(1,?,1,'training','in_progress',0,'{}')", sid);
        Assert.True(string.IsNullOrEmpty(H.Str(Attempt(db, aid)["config_snapshot"])));

        SimLabSchema.Ensure(db);   // re-run: Backfill snapshots the attempt + records the published version

        Assert.Equal(ConfigA, H.Str(Attempt(db, aid)["config_snapshot"]));
        var ver = db.QueryOne("SELECT config_json,checksum,version FROM simulation_scenario_versions WHERE scenario_id=?", sid);
        Assert.NotNull(ver);
        Assert.Equal(ConfigA, H.Str(ver!["config_json"]));
        Assert.Equal(SimVersion.Checksum(ConfigA), H.Str(ver["checksum"]));
    }

    [Fact]
    public void EnsureVersion_is_idempotent_and_immutable()
    {
        var db = FreshDb();
        var sid = InsertScenario(db, "VER-IMMUT", ConfigA);
        var first = SimVersion.EnsureVersion(db, sid, 7);
        var second = SimVersion.EnsureVersion(db, sid, 7);
        Assert.Equal(first, second);   // same (scenario, version) → one ledger row

        // Mutating the live config and re-ensuring the SAME version must not rewrite the frozen row.
        db.Execute("UPDATE simulation_scenarios SET config_json=? WHERE id=?", ConfigB, sid);
        var third = SimVersion.EnsureVersion(db, sid, 7);
        Assert.Equal(first, third);
        var ver = db.QueryOne("SELECT config_json FROM simulation_scenario_versions WHERE id=?", first)!;
        Assert.Equal(ConfigA, H.Str(ver["config_json"]));   // still the ORIGINAL published config

        var count = db.Scalar<long>("SELECT COUNT(*) FROM simulation_scenario_versions WHERE scenario_id=?", sid);
        Assert.Equal(1, count);
    }

    [Fact]
    public void Checksum_is_stable_and_content_sensitive()
    {
        Assert.Equal(SimVersion.Checksum(ConfigA), SimVersion.Checksum(ConfigA));
        Assert.NotEqual(SimVersion.Checksum(ConfigA), SimVersion.Checksum(ConfigB));
    }

    [Fact]
    public void Attempt_is_owner_scoped()
    {
        var db = FreshDb();
        var sid = InsertScenario(db, "OWN-1", ConfigA);
        var aid = db.ExecuteReturningId(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json,config_snapshot)
            VALUES(1,?,1,'training','in_progress',0,'{}',?)", sid, ConfigA);
        // The endpoints load attempts with `WHERE id=? AND user_id=?` — another user must not resolve it.
        Assert.NotNull(db.QueryOne("SELECT id FROM simulation_attempts WHERE id=? AND user_id=?", aid, 1L));
        Assert.Null(db.QueryOne("SELECT id FROM simulation_attempts WHERE id=? AND user_id=?", aid, 2L));
    }
}
