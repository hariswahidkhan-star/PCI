using System.IO;
using System.Linq;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// P3 resilience gate — backup/restore integrity for the Simulation Lab. A disaster-recovery restore must
/// bring back the catalogue, in-flight student attempts, AND the version snapshots that make replay
/// immutable (the P0 guarantee): a restored attempt has to grade off its frozen config exactly as it did
/// before the backup, even though the live row has moved on. Uses SQLite's own backup command
/// (VACUUM INTO) to take a real, independent copy, then wipes the original to prove the restore stands on
/// its own. Backup-by-copy is a SQLite mechanism; production MySQL uses managed dump/restore, so this is
/// SQLite-only by design.
/// </summary>
public class SimBackupRestoreTests
{
    // A config unlike anything the pack ships: one EVM ask (CV = EV − AC = −50), so full marks against
    // THIS frozen config would grade wrong against the live pack row — the tell that replay used the snapshot.
    const string FrozenConfig = """
        {"task":"evm","prompt":"Pre-backup content.",
         "given":{"pv":100,"ev":50,"ac":100,"bac":200},
         "ask":[{"key":"cv","label":"Cost Variance (CV)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
        """;

    static double GradeCv(string configJson)
    {
        var config = JsonDocument.Parse(configJson).RootElement;
        var answers = JsonDocument.Parse("""{"cv":-50}""").RootElement;
        return SimGrade.Grade(config, answers, "training").Score;
    }

    [SqliteOnlyFact("backup-by-copy (VACUUM INTO) is a SQLite mechanism; production MySQL uses managed dump/restore")]
    public void A_restored_backup_preserves_the_catalogue_attempts_and_immutable_replay()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);

        // Fixture: an in-flight attempt pinned to a frozen config that differs from the live row, so replay
        // is only correct if the snapshot survives the restore.
        var row = db.QueryOne("SELECT id,version,config_json FROM simulation_scenarios WHERE scenario_code=?", "GL-EVM-001")!;
        var sid = Convert.ToInt64(row["id"]);
        var ver = Convert.ToInt64(row["version"]);
        var liveConfig = Convert.ToString(row["config_json"])!;
        Assert.NotEqual(FrozenConfig, liveConfig);

        db.Execute(@"INSERT INTO simulation_attempts(user_id,scenario_id,scenario_version,mode,status,seed,state_json)
            VALUES(42,?,?, 'training','in_progress',0,'{""answers"":{""cv"":-50}}')", sid, ver);
        SimVersion.Snapshot(db, sid, ver, FrozenConfig);
        var attemptId = db.Scalar<long>("SELECT id FROM simulation_attempts WHERE user_id=42 AND scenario_id=?", sid);
        Assert.Equal(100.0, GradeCv(SimVersion.PinnedConfig(db, sid, ver, liveConfig)!));   // frozen replay is correct

        var publishedBefore = db.Scalar<long>("SELECT COUNT(*) FROM simulation_scenarios WHERE status='published'");
        Assert.True(publishedBefore > 0);

        // ── Back up: a real, independent copy via SQLite's own backup command ──
        var backupPath = Path.Combine(TestEnv.Root, "sim-backup-" + Guid.NewGuid().ToString("n") + ".db");
        if (File.Exists(backupPath)) File.Delete(backupPath);
        db.Execute($"VACUUM INTO '{backupPath}'");
        Assert.True(File.Exists(backupPath), "the backup file should exist");

        // ── Disaster: wipe the live Lab data so the restore has to stand on its own ──
        db.Execute("DELETE FROM simulation_scenario_versions");
        db.Execute("DELETE FROM simulation_attempts");
        db.Execute("DELETE FROM simulation_scenarios");
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM simulation_scenarios"));

        // ── Restore: open the backup as an independent database and verify everything came back ──
        var restored = new Db(backupPath);

        Assert.Equal(publishedBefore,
            restored.Scalar<long>("SELECT COUNT(*) FROM simulation_scenarios WHERE status='published'"));

        // The in-flight attempt survived, with its state.
        var att = restored.QueryOne("SELECT user_id,scenario_id,scenario_version,status,state_json FROM simulation_attempts WHERE id=?", attemptId);
        Assert.NotNull(att);
        Assert.Equal(42L, Convert.ToInt64(att!["user_id"]));
        Assert.Equal(ver, Convert.ToInt64(att["scenario_version"]));

        // Immutable replay survives the restore: the pinned snapshot is intact and grades identically…
        var restoredLive = Convert.ToString(restored.QueryOne("SELECT config_json FROM simulation_scenarios WHERE id=?", sid)!["config_json"])!;
        var frozen = SimVersion.PinnedConfig(restored, sid, ver, restoredLive)!;
        Assert.Equal(FrozenConfig, frozen);
        Assert.Equal(100.0, GradeCv(frozen));
        // …and it is genuinely the snapshot doing the work, not the live row.
        Assert.NotEqual(100.0, GradeCv(restoredLive));
    }
}
