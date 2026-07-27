using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// Simulation Lab — scenario content library (Phase 5A studio-pipeline output). Loads
/// simlab_scenarios_seed.json — a synthetic, machine-validated practice library authored at scale across
/// every engine task family (EVM, CPM, WBS, CBS, progress, risk/EMV, PERT, change control, cash flow,
/// EVM timeline, earned schedule) — into simulation_scenarios as governed, published rows. Every entry
/// passed the §14 publication validator (SimContent.Validate: metadata, retired-name scan, reference-solver
/// answer check, variant-seed gradability) before shipping, and SimLabContentSeedTests re-proves that on
/// every CI run, so the pack can never drift from the engine.
///
/// Applied ONCE per seed version (site_settings 'simlab_content_seed_version'); rows are additionally
/// idempotent by UNIQUE scenario_code (INSERT … WHERE NOT EXISTS) and the governance backfill only touches
/// rows still at the authoring default — operator edits, unpublishes and retirements are never overwritten.
/// </summary>
public static class SimLabContentSeed
{
    // v2: every scenario carries a certification_id (1/2/3) — 24 rows shipped without one in v1, so v2
    // re-applies to backfill them on installs that already loaded the library.
    // v3: expansion pack — the seven engine tasks added after v1 (productivity, BoQ, resource, procurement,
    // portfolio, decision, data quality) plus the advanced/expert difficulty bands the first pass never
    // generated. Re-applies so installs already holding v2 pick up the new scenarios (existing rows are
    // untouched: inserts are guarded by scenario_code and the governance backfill only fills NULLs).
    // v4: P3 content-scale batch — 15 EVM-family forecast scenarios (GL/SD/SC-EVM-3xx) across four
    // difficulty bands, lifting the published catalogue past the market-launch target (>=120 scenarios /
    // 1000+ tasks). Re-applies so installs on v3 pick them up; existing rows are untouched.
    const int Version = 4;

    public static void Apply(Db db)
    {
        try
        {
            var applied = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='simlab_content_seed_version'");
            if (int.TryParse(applied, out var v) && v >= Version) return;

            var path = File.Exists("simlab_scenarios_seed.json") ? "simlab_scenarios_seed.json"
                     : Path.Combine(AppContext.BaseDirectory, "simlab_scenarios_seed.json");
            if (!File.Exists(path)) return;

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("scenarios", out var rows) || rows.ValueKind != JsonValueKind.Array) return;
            var n = 0;
            db.Transaction(() =>
            {
                foreach (var s in rows.EnumerateArray())
                {
                    string? S(string k) => s.TryGetProperty(k, out var el) && el.ValueKind == JsonValueKind.String ? el.GetString() : null;
                    string? Raw(string k) => s.TryGetProperty(k, out var el) && el.ValueKind is JsonValueKind.Array or JsonValueKind.Object ? el.GetRawText() : null;
                    var code = S("scenario_code");
                    if (string.IsNullOrWhiteSpace(code)) continue;
                    long? cert = s.TryGetProperty("certification_id", out var c) && c.ValueKind == JsonValueKind.Number ? c.GetInt64() : null;
                    var minutes = s.TryGetProperty("est_minutes", out var m) && m.TryGetInt32(out var mi) ? mi : 15;
                    db.Execute(@"INSERT INTO simulation_scenarios(
                            scenario_code,title,kind,industry,project_type,difficulty,est_minutes,
                            competencies_json,certification_id,summary,brief,config_json,status,version)
                        SELECT ?,?,?,?,?,?,?,?,?,?,?,?, 'published', 1
                        WHERE NOT EXISTS(SELECT 1 FROM simulation_scenarios WHERE scenario_code=?)",
                        code, S("title"), S("kind") ?? "guided_lab", S("industry"), S("project_type"),
                        S("difficulty") ?? "foundation", minutes, Raw("competencies"), cert,
                        S("summary"), S("brief"), Raw("config"), code);
                    // Governance evidence (§14): synthetic declaration, objectives, provenance, disclaimers,
                    // worked solution and the published state — set only while the row still sits at the
                    // authoring default, so an operator who pulls a scenario back into review keeps their state.
                    db.Execute(@"UPDATE simulation_scenarios
                        SET review_state='published', synthetic_declared=1,
                            published_at=COALESCE(published_at, datetime('now')),
                            objectives_json=COALESCE(objectives_json, ?),
                            provenance=COALESCE(provenance, ?),
                            disclaimers=COALESCE(disclaimers, ?),
                            worked_solution=COALESCE(worked_solution, ?)
                        WHERE scenario_code=? AND (review_state IS NULL OR review_state='draft')",
                        Raw("objectives"), S("provenance"), S("disclaimers"), S("worked_solution"), code);
                    // Certification mapping is catalogue metadata the coverage gate requires (§4): backfill
                    // rows a v1 seed left unmapped, but never overwrite an operator's assignment.
                    if (cert is not null)
                        db.Execute("UPDATE simulation_scenarios SET certification_id=? WHERE scenario_code=? AND certification_id IS NULL",
                            cert, code);
                    n++;
                }
                db.Execute("DELETE FROM site_settings WHERE skey='simlab_content_seed_version'");
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('simlab_content_seed_version',?)", Version.ToString());
            });
            Console.WriteLine($"[seed] Simulation Lab content library loaded: {n} scenarios");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] SimLab content library skipped: {e.Message}"); }
    }
}
