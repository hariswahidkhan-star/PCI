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

            var input = InputFrom(s);
            var issues = SimContent.Validate(input, OtherCodes(db, id));
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

        // ---- create a DRAFT scenario (§13 authoring). Records the author for maker-checker; starts in the
        //      draft review state and is never served to students until it walks the review workflow. ----
        app.MapPost("/api/admin/lab/scenarios", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "content", adm =>
            {
                var code = (H.GetS(b, "scenario_code") ?? "").Trim();
                var title = (H.GetS(b, "title") ?? "").Trim();
                if (code.Length == 0 || title.Length == 0)
                    return Results.Json(new { error = "bad_input", message = "scenario_code and title are required." }, statusCode: 400);
                if (db.QueryOne("SELECT id FROM simulation_scenarios WHERE scenario_code=?", code) is not null)
                    return Results.Json(new { error = "duplicate_code" }, statusCode: 409);

                var competencies = H.GetEl(b, "competencies") is { ValueKind: JsonValueKind.Array } ca
                    ? JsonSerializer.Serialize(ca.EnumerateArray().Select(e => e.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)))
                    : (H.GetS(b, "competencies_json") ?? "[]");
                var config = H.GetEl(b, "config_json") is { ValueKind: JsonValueKind.Object } cj ? cj.GetRawText() : H.GetS(b, "config_json");
                long? certId = H.GetNum(b, "certification_id") is { } cn ? (long)cn : null;

                var id = db.ExecuteReturningId(@"INSERT INTO simulation_scenarios
                    (scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,certification_id,
                     summary,config_json,status,review_state,version,synthetic_declared,authored_by,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?, 'draft','draft', 1, ?, ?, ?)",
                    code, title, (H.GetS(b, "kind") ?? "scenario").Trim(), (H.GetS(b, "industry") ?? "").Trim(),
                    (H.GetS(b, "difficulty") ?? "foundation").Trim(), (int)(H.GetNum(b, "est_minutes") ?? 15),
                    competencies, certId, H.GetS(b, "summary"), config,
                    Truthy(H.GetEl(b, "synthetic_declared")) ? 1 : 0, adm.Id, adm.Id);
                log(adm.Id, "sim_scenario_create", $"{code} · #{id}");
                return Results.Json(new { id, scenario_code = code, review_state = "draft", status = "draft" });
            });
        });

        // ---- advance a scenario through the review workflow (§13). Structural moves are gated by
        //      SimReview; approve/publish additionally require the §14 validator to pass; approval enforces
        //      maker-checker (approver != author). Keeps the operational status in sync and audits each move.
        app.MapPost("/api/admin/lab/scenarios/{id}/review", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "content", adm =>
            {
                var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
                if (s is null) return Results.NotFound(new { error = "not_found" });
                var from = (s.TryGetValue("review_state", out var rs0) ? H.Str(rs0) : null) ?? "draft";
                var to = (H.GetS(b, "to") ?? "").Trim();
                if (!SimReview.IsState(to)) return Results.Json(new { error = "bad_state" }, statusCode: 400);
                if (!SimReview.CanTransition(from, to))
                    return Results.Json(new { error = "bad_transition", from, to }, statusCode: 409);

                if (SimReview.RequiresPublishable(to))
                {
                    var issues = SimContent.Validate(InputFrom(s), OtherCodes(db, id));
                    if (!SimContent.Publishable(issues))
                        return Results.Json(new
                        {
                            error = "not_publishable",
                            errors = issues.Count(i => i.Severity == SimContent.Severity.Error),
                            issues = issues.Where(i => i.Severity == SimContent.Severity.Error)
                                .Select(i => new { code = i.Code, message = i.Message }),
                        }, statusCode: 409);
                }
                if (SimReview.RequiresDifferentChecker(to))
                {
                    var author = s.TryGetValue("authored_by", out var ab) && ab is not null ? H.L(ab) : 0;
                    if (author != 0 && author == adm.Id)
                        return Results.Json(new { error = "maker_checker", message = "The approver must be different from the author." }, statusCode: 409);
                }

                db.Execute("UPDATE simulation_scenarios SET review_state=?, updated_at=datetime('now') WHERE id=?", to, id);
                // Whoever advances OUT of a review stage signs it off.
                if (from == SimReview.CalcReview) db.Execute("UPDATE simulation_scenarios SET calc_reviewed_by=? WHERE id=?", adm.Id, id);
                else if (from == SimReview.LearningReview) db.Execute("UPDATE simulation_scenarios SET learning_reviewed_by=? WHERE id=?", adm.Id, id);
                else if (from == SimReview.SafetyReview) db.Execute("UPDATE simulation_scenarios SET safety_reviewed_by=? WHERE id=?", adm.Id, id);
                if (to == SimReview.Approved) db.Execute("UPDATE simulation_scenarios SET approved_by=?, approved_at=datetime('now') WHERE id=?", adm.Id, id);
                if (to == SimReview.Published) db.Execute("UPDATE simulation_scenarios SET status='published', published_at=COALESCE(published_at, datetime('now')) WHERE id=?", id);
                else if (to == SimReview.Retired) db.Execute("UPDATE simulation_scenarios SET status='archived' WHERE id=?", id);
                else if (to == SimReview.Draft) db.Execute("UPDATE simulation_scenarios SET status='draft' WHERE id=?", id);

                log(adm.Id, "sim_scenario_review", $"{H.Str(s["scenario_code"])} {from}→{to}");
                return Results.Json(new { id, review_state = to, from });
            });
        });
    }

    static SimContent.ScenarioInput InputFrom(Dictionary<string, object?> s) => new(
        H.Str(s["scenario_code"]) ?? "", H.Str(s["title"]), H.Str(s["summary"]), H.Str(s["difficulty"]),
        s.TryGetValue("certification_id", out var cv) && cv is not null ? H.L(cv) : (long?)null,
        H.Str(s["competencies_json"]), H.Str(s["config_json"]),
        s.TryGetValue("synthetic_declared", out var sd) && sd is not null && H.L(sd) == 1);

    static List<string> OtherCodes(Db db, long id)
    {
        var others = new List<string>();
        foreach (var r in db.Query("SELECT scenario_code FROM simulation_scenarios WHERE id<>?", id))
            others.Add(H.Str(r["scenario_code"]) ?? "");
        return others;
    }

    static bool Truthy(JsonElement? el) => el is { } e && (e.ValueKind == JsonValueKind.True
        || (e.ValueKind == JsonValueKind.String && e.GetString() is "1" or "true")
        || (e.ValueKind == JsonValueKind.Number && e.TryGetInt32(out var i) && i != 0));

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
