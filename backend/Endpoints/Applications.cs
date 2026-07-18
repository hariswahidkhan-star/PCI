using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Per-certification application submission + review (Phase 4b). A candidate applies for a specific
/// certification through a specific route; the record is keyed by (user, certification_id, route_key)
/// so an application for one credential can never be confused with another. Routes that require approval
/// enter review; auto-approved routes are approved on submit. Admin surface is gated on "members"
/// (the same section as the Students admin area).
/// </summary>
public static class Applications
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        // ── Student: submit an application ──
        app.MapPost("/api/me/applications", async (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var certId = Certs.Resolve(db, H.GetS(b, "certification") ?? H.GetS(b, "certification_id"));
            var cert = Certs.ById(db, certId);
            if (cert is null) return Results.Json(new { error = "bad_certification" }, statusCode: 400);
            var routeKey = (H.GetS(b, "route") ?? H.GetS(b, "route_key") ?? "standard").Trim().ToLowerInvariant();
            var route = Routes.Get(db, certId, routeKey);
            if (route is null || !H.B(route["enabled"]))
                return Results.Json(new { error = "route_unavailable", message = "That application route is not available for this certification." }, statusCode: 400);
            // one live application per (candidate, certification, route)
            var dup = db.Scalar<long>("SELECT COUNT(*) FROM certification_applications WHERE user_id=? AND certification_id=? AND route_key=? AND status NOT IN ('rejected','withdrawn')", u.Id, certId, routeKey);
            if (dup > 0) return Results.Json(new { error = "already_applied", message = "You already have an active application for this certification and route." }, statusCode: 409);
            var dataJson = b.TryGetValue("data", out var dv) && dv.ValueKind != JsonValueKind.Null ? dv.GetRawText() : "{}";
            var reqApproval = H.B(route["requires_approval"]);
            var id = db.ExecuteReturningId("INSERT INTO certification_applications(user_id,certification_id,route_key,status,workflow_stage,data_json) VALUES(?,?,?,?,?,?)",
                u.Id, certId, routeKey, reqApproval ? "submitted" : "approved", reqApproval ? "application_submitted" : "approved", dataJson);
            var appNo = $"PCI-APP-{DateTime.UtcNow.Year}-{100000 + id}";
            db.Execute("UPDATE certification_applications SET application_no=? WHERE id=?", appNo, id);
            log(u.Id, "application_submitted", $"{appNo} cert {certId} route {routeKey}");
            try { Notify.Alert(db, "application", $"New {H.Str(cert["acronym"]) ?? H.Str(cert["code"])} application", $"<p>Application <strong>{appNo}</strong> submitted via the {routeKey} route.</p>", "application", id, null); } catch { }
            return J(new { ok = true, id, application_no = appNo, status = reqApproval ? "submitted" : "approved" });
        });

        // ── Student: my applications ──
        app.MapGet("/api/me/applications", (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query(@"SELECT a.id,a.application_no,a.certification_id,a.route_key,a.status,a.workflow_stage,a.blocker,a.admin_note,a.created_at,
                    c.acronym cert_acronym, c.name cert_name
                FROM certification_applications a LEFT JOIN certifications c ON c.id=a.certification_id
                WHERE a.user_id=? ORDER BY a.id DESC", u.Id) });
        });

        // ── Admin: list applications (filterable by certification + status) ──
        app.MapGet("/api/admin/applications", (HttpRequest req) => gate(req, "members", _ =>
        {
            var w = new List<string>(); var args = new List<object?>();
            if (long.TryParse(req.Query["certification_id"].ToString(), out var cid)) { w.Add("a.certification_id=?"); args.Add(cid); }
            var st = req.Query["status"].ToString();
            if (!string.IsNullOrEmpty(st)) { w.Add("a.status=?"); args.Add(st); }
            var route = req.Query["route_key"].ToString();
            if (!string.IsNullOrEmpty(route)) { w.Add("a.route_key=?"); args.Add(route); }
            var where = w.Count > 0 ? "WHERE " + string.Join(" AND ", w) : "";
            return J(new { rows = db.Query($@"SELECT a.*, c.acronym cert_acronym, c.name cert_name, u.email, u.first_name, u.last_name
                FROM certification_applications a LEFT JOIN certifications c ON c.id=a.certification_id LEFT JOIN users u ON u.id=a.user_id
                {where} ORDER BY a.id DESC LIMIT 500", args.ToArray()) });
        }));

        // ── Admin: decide an application ──
        app.MapPost("/api/admin/applications/{id}/decision", (HttpRequest req, long id) => gate(req, "members", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var action = (H.GetS(b, "action") ?? "").ToLowerInvariant();
            var note = H.GetS(b, "note");
            var status = action switch
            {
                "approve" => "approved", "reject" => "rejected",
                "request_info" => "info_requested", "under_review" => "under_review", _ => null,
            };
            if (status is null) return Results.Json(new { error = "bad_action" }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM certification_applications WHERE id=?", id) is null)
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE certification_applications SET status=?, workflow_stage=?, admin_note=?, decided_by=?, decided_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                status, status, note, adm.Id, id);
            log(adm.Id, "application_decision", $"{id} -> {status}");
            return J(new { ok = true, status });
        }));
    }
}
