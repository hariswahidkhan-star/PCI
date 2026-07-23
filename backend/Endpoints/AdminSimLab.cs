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
    }

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
