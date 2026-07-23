using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI AI Project Controls Simulation Lab — student surface (Phase 1 foundation).
///
///   GET /api/me/lab/access     — whether the student can enter the Lab, and why (live entitlement)
///   GET /api/me/lab/catalogue  — the published guided labs / drills / scenarios, with the student's
///                                latest attempt status folded in
///
/// Reuses the existing account (Core.Auth.UserFromReq) — no separate Lab login. Gated by the operator
/// flag `sp_simlab_enabled` and the `simlab_requires` entitlement rule (Core.SimLab). Applied simulation
/// only; never touches exam_attempts, entitlements or issued credentials. Distinct from Certuvo (external
/// MCQ exam-prep).
/// </summary>
public static class SimLab
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        UserCtx? Auth(HttpContext ctx) => Core.Auth.UserFromReq(ctx.Request, db);
        IResult J(object o) => Results.Json(o);

        // Auth + operator-flag gate. (Access-level entitlement is reported by /access itself, not gated
        // here, so the student can always learn *why* they don't yet have access.)
        IResult? Gate(HttpContext ctx, out UserCtx? u)
        {
            u = Auth(ctx);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!Core.SimLab.Enabled(db)) return Results.Json(new { error = "lab_disabled" }, statusCode: 403);
            return null;
        }

        // ---- access: live entitlement descriptor (never a raw error; friendly student-facing copy) ----
        app.MapGet("/api/me/lab/access", (HttpContext ctx) =>
        {
            var u = Auth(ctx);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(Core.SimLab.AccessFor(db, u.Id));
        });

        // ---- catalogue: published labs/drills/scenarios + this student's latest attempt per scenario ----
        app.MapGet("/api/me/lab/catalogue", (HttpContext ctx) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            if (!Core.SimLab.Eligible(db, u!.Id))
                return Results.Json(new { error = "no_access", access = Core.SimLab.AccessFor(db, u.Id) }, statusCode: 403);

            // The student's latest attempt per scenario (for status/score badges), keyed by scenario_id.
            var latest = new Dictionary<long, (string status, object? score)>();
            foreach (var a in db.Query(@"SELECT scenario_id,status,score FROM simulation_attempts
                WHERE user_id=? ORDER BY id ASC", u.Id))
                latest[H.L(a["scenario_id"])] = (H.Str(a["status"]) ?? "in_progress", a["score"]);

            var rows = new List<object>();
            foreach (var s in db.Query(@"SELECT id,scenario_code,title,kind,industry,project_type,difficulty,
                est_minutes,competencies_json,certification_id,summary,version FROM simulation_scenarios
                WHERE status='published' ORDER BY sort_order ASC, id ASC"))
            {
                var sid = H.L(s["id"]);
                var has = latest.TryGetValue(sid, out var at);
                rows.Add(new
                {
                    id = sid,
                    scenario_code = H.Str(s["scenario_code"]),
                    title = H.Str(s["title"]),
                    kind = H.Str(s["kind"]),
                    industry = H.Str(s["industry"]),
                    project_type = H.Str(s["project_type"]),
                    difficulty = H.Str(s["difficulty"]),
                    est_minutes = H.L(s["est_minutes"]),
                    competencies = ParseArray(H.Str(s["competencies_json"])),
                    certification_id = s["certification_id"] is null ? (long?)null : H.L(s["certification_id"]),
                    summary = H.Str(s["summary"]),
                    version = H.L(s["version"]),
                    attempt_status = has ? at.status : (string?)null,
                    score = has ? at.score : null,
                });
            }
            return J(new { rows });
        });
    }

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
