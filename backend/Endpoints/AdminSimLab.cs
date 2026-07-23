using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Simulation Lab (Phase 1 foundation). A read-only operator view of the guided-lab /
/// scenario catalogue — every scenario across all statuses (draft, published, suspended, archived), its
/// competencies and interactivity, and how much practice each has seen (attempt + completion counts).
/// Authoring / publishing lifecycle arrives in a later increment; this gives operators visibility now.
///
/// Gated by the existing 'content' permission (the Lab is educational content). Read-only, self-contained,
/// no external credentials, and it never exposes student-identifying data — only per-scenario aggregates.
/// </summary>
public static class AdminSimLab
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        // ---- scenario catalogue (all statuses) + per-scenario practice aggregates ----
        app.MapGet("/api/admin/lab/scenarios", (HttpRequest req) => gate(req, "content", _ =>
        {
            // Per-scenario attempt + completion counts (aggregate only — no student identity).
            var stats = new Dictionary<long, (long attempts, long completed)>();
            foreach (var r in db.Query(@"SELECT scenario_id, COUNT(*) n,
                    SUM(CASE WHEN status IN ('completed','passed','failed') THEN 1 ELSE 0 END) c
                FROM simulation_attempts GROUP BY scenario_id"))
                stats[H.L(r["scenario_id"])] = (H.L(r["n"]), H.L(r["c"]));

            var rows = new List<object>();
            var published = 0;
            foreach (var s in db.Query(@"SELECT id,scenario_code,title,kind,industry,difficulty,est_minutes,
                    competencies_json,status,version,sort_order,config_json,updated_at
                FROM simulation_scenarios ORDER BY sort_order ASC, id ASC"))
            {
                var id = H.L(s["id"]);
                var status = H.Str(s["status"]);
                if (status == "published") published++;
                var has = stats.TryGetValue(id, out var st);
                rows.Add(new
                {
                    id,
                    scenario_code = H.Str(s["scenario_code"]),
                    title = H.Str(s["title"]),
                    kind = H.Str(s["kind"]),
                    industry = H.Str(s["industry"]),
                    difficulty = H.Str(s["difficulty"]),
                    est_minutes = H.L(s["est_minutes"]),
                    competencies = ParseArray(H.Str(s["competencies_json"])),
                    status,
                    version = H.L(s["version"]),
                    interactive = !string.IsNullOrWhiteSpace(H.Str(s["config_json"])),
                    attempts = has ? st.attempts : 0,
                    completed = has ? st.completed : 0,
                    updated_at = H.Str(s["updated_at"]),
                });
            }
            return Results.Json(new { rows, total = rows.Count, published });
        }));

        // ---- content-quality validation for one scenario (§14 publication gate) ----
        // Runs the deterministic SimContent validator: metadata completeness, retired-name check, and the
        // reference-solver pass (every asked measure must resolve through the engine). Read-only.
        app.MapGet("/api/admin/lab/scenarios/{id}/validate", (HttpRequest req, long id) => gate(req, "content", _ =>
        {
            var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
            if (s is null) return Results.NotFound(new { error = "not_found" });

            var others = new List<string>();
            foreach (var r in db.Query("SELECT scenario_code FROM simulation_scenarios WHERE id<>?", id))
                others.Add(H.Str(r["scenario_code"]) ?? "");

            var input = new SimContent.ScenarioInput(
                H.Str(s["scenario_code"]) ?? "",
                H.Str(s["title"]),
                H.Str(s["summary"]),
                H.Str(s["difficulty"]),
                s.TryGetValue("certification_id", out var cv) && cv is not null ? H.L(cv) : (long?)null,
                H.Str(s["competencies_json"]),
                H.Str(s["config_json"]),
                s.TryGetValue("synthetic_declared", out var sd) && sd is not null && H.L(sd) == 1);

            var issues = SimContent.Validate(input, others);
            return Results.Json(new
            {
                id,
                scenario_code = input.ScenarioCode,
                review_state = s.TryGetValue("review_state", out var rs) ? H.Str(rs) : "draft",
                publishable = SimContent.Publishable(issues),
                errors = issues.Count(i => i.Severity == SimContent.Severity.Error),
                warnings = issues.Count(i => i.Severity == SimContent.Severity.Warning),
                issues = issues.Select(i => new { severity = i.Severity.ToString().ToLowerInvariant(), code = i.Code, message = i.Message }),
            });
        }));
    }

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
