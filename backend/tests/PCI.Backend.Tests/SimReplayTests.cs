using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — historical-replay immutability (the P0 guarantee). An attempt pins
/// (scenario_id, scenario_version, seed); simulation_scenario_versions freezes what that version MEANT.
/// These tests prove the three ways a house row's config can move — a content-pack deploy, a repeated
/// boot, and the pre-snapshot legacy window — never change how an existing attempt replays or grades:
/// the pack bumps `version` when (and only when) config changes, snapshots are write-once, and the boot
/// backfill freezes legacy attempts BEFORE any seeder runs.
/// </summary>
public class SimReplayTests
{
    // A deliberately different config from anything the pack ships: one EVM ask (CV = EV − AC = −50),
    // so a full-marks answer against THIS config grades wrong against the pack's densified GL-EVM-001.
    const string OldConfig = """
        {"task":"evm","prompt":"Legacy deploy content.",
         "given":{"pv":100,"ev":50,"ac":100,"bac":200},
         "ask":[{"key":"cv","label":"Cost Variance (CV)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
        """;

    static Db NewLabDb()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);
        return db;
    }

    static (long Id, long Version, string Config) Row(Db db, string code)
    {
        var r = db.QueryOne("SELECT id,version,config_json FROM simulation_scenarios WHERE scenario_code=?", code)!;
        return (Convert.ToInt64(r["id"]), Convert.ToInt64(r["version"]), Convert.ToString(r["config_json"]) ?? "");
    }

    static double GradeCv(string configJson)
    {
        var config = JsonDocument.Parse(configJson).RootElement;
        var answers = JsonDocument.Parse("""{"cv":-50}""").RootElement;
        return SimGrade.Grade(config, answers, "training").Score;
    }

    [Fact]
    public void Reseeding_identical_content_never_bumps_a_version()
    {
        var db = NewLabDb();
        var before = db.Query("SELECT scenario_code,version FROM simulation_scenarios")
            .ToDictionary(r => Convert.ToString(r["scenario_code"])!, r => Convert.ToInt64(r["version"]));
        Assert.NotEmpty(before);

        SimLabSchema.Ensure(db);   // second boot, same shipped content

        foreach (var r in db.Query("SELECT scenario_code,version FROM simulation_scenarios"))
            Assert.Equal(before[Convert.ToString(r["scenario_code"])!], Convert.ToInt64(r["version"]));
    }

    [Fact]
    public void Pack_config_change_becomes_a_new_version_and_history_replays_identically()
    {
        var db = NewLabDb();
        var (id, v1, packConfig) = Row(db, "GL-EVM-001");
        Assert.NotEqual(OldConfig, packConfig);

        // Rewind the row to an older deploy's content and record an attempt against it, snapshotting at
        // start exactly as the endpoint does.
        db.Execute("UPDATE simulation_scenarios SET config_json=? WHERE id=?", OldConfig, id);
        db.Execute(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json)
            VALUES(1,?,?, 'training','in_progress',0,'{}')", id, v1);
        SimVersion.Snapshot(db, id, v1, OldConfig);
        Assert.Equal(100.0, GradeCv(SimVersion.PinnedConfig(db, id, v1, OldConfig)!));

        // The next deploy boots: the pack restores its config — as a NEW version, not a rewrite of v1.
        SimLabSchema.Ensure(db);
        var (_, v2, liveConfig) = Row(db, "GL-EVM-001");
        Assert.Equal(v1 + 1, v2);
        Assert.Equal(packConfig, liveConfig);

        // The historical attempt still replays and grades from its frozen v1 — full marks, unchanged —
        // while the live row would grade those answers differently.
        var frozen = SimVersion.PinnedConfig(db, id, v1, liveConfig)!;
        Assert.Equal(OldConfig, frozen);
        Assert.Equal(100.0, GradeCv(frozen));
        Assert.NotEqual(100.0, GradeCv(liveConfig));

        // New attempts pin the new version; a further identical boot bumps nothing.
        SimVersion.Snapshot(db, id, v2, liveConfig);
        Assert.Equal(liveConfig, SimVersion.PinnedConfig(db, id, v2, liveConfig));
        SimLabSchema.Ensure(db);
        Assert.Equal(v2, Row(db, "GL-EVM-001").Version);
    }

    [Fact]
    public void Boot_backfill_freezes_legacy_attempts_before_any_seeder_runs()
    {
        var db = NewLabDb();
        var (id, v1, packConfig) = Row(db, "GL-WBS-001");

        // A legacy attempt: recorded while the row carried older content, with NO snapshot (the feature
        // didn't exist yet).
        db.Execute("UPDATE simulation_scenarios SET config_json=? WHERE id=?", OldConfig, id);
        db.Execute(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json)
            VALUES(1,?,?, 'training','in_progress',0,'{}')", id, v1);

        // The upgrade boot must freeze the attempt at what it was being served BEFORE the pack upsert
        // restores the shipped config in the same boot.
        SimLabSchema.Ensure(db);
        var (_, v2, liveConfig) = Row(db, "GL-WBS-001");
        Assert.Equal(v1 + 1, v2);
        Assert.Equal(packConfig, liveConfig);
        Assert.Equal(OldConfig, SimVersion.PinnedConfig(db, id, v1, liveConfig));
    }

    [Fact]
    public void Snapshots_are_write_once_and_missing_versions_fall_back_to_live()
    {
        var db = NewLabDb();
        var (id, _, packConfig) = Row(db, "GL-EVM-001");

        SimVersion.Snapshot(db, id, 7, "{\"task\":\"evm\",\"first\":true}");
        SimVersion.Snapshot(db, id, 7, "{\"task\":\"evm\",\"second\":true}");   // must lose: first write wins
        Assert.Equal("{\"task\":\"evm\",\"first\":true}", SimVersion.PinnedConfig(db, id, 7, packConfig));

        SimVersion.Snapshot(db, id, 8, null);                                   // nothing to pin
        SimVersion.Snapshot(db, id, 8, "");
        Assert.Equal(packConfig, SimVersion.PinnedConfig(db, id, 8, packConfig));   // falls back to live
        Assert.Null(SimVersion.PinnedConfig(db, id, 999, null));
    }
}
