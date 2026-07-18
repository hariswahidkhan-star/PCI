using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Honorary Fellow (PCI) — Route C of the three-route access model. A board-conferred recognition
/// that is deliberately and permanently SEPARATE from the examined PCP-AI credential: no exam, no
/// entitlement, no issued_credentials row, its own PCI-HON award number, and owner-only conferral.
/// Nothing here may ever set an exam, result or credential field.
/// </summary>
public static class Honorary
{
    public const string AwardPrefix = "PCI-HON";

    /// <summary>Mint a unique PCI-HON award number and insert the honorary_awards row, returning the
    /// award number (null if generation failed after retries). Shared by manual board conferral and by
    /// approved honorary-route applications, so both produce identical, publicly-verifiable awards.
    /// Distinct number space from every certification prefix; widens to 5 digits after collisions.</summary>
    public static string? ConferAward(Db db, string recipientName, long? userId, string citation, long adminId)
    {
        var yr = DateTime.UtcNow.Year;
        var rnd = new Random();
        for (int i = 0; i < 20; i++)
        {
            var candidate = i < 10 ? $"{AwardPrefix}-{yr}-{1000 + rnd.Next(0, 8999)}" : $"{AwardPrefix}-{yr}-{10000 + rnd.Next(0, 89999)}";
            try
            {
                db.Execute("INSERT INTO honorary_awards(award_no,recipient_name,user_id,citation,conferred_by) VALUES(?,?,?,?,?)",
                    candidate, recipientName, userId, citation, adminId);
                // Render the honorary certificate PDF (best-effort; regenerated on first download if it fails).
                CertIssue.EnsureHonoraryPdf(db, candidate);
                return candidate;
            }
            catch { /* award_no collision → retry */ }
        }
        return null;
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        // Board/owner only — stricter than section RBAC: honorary recognition is a governance act.
        (AdminCtx? adm, IResult? deny) Owner(HttpRequest req)
        {
            var a = Auth.AdminFromReq(req, db);
            if (a is null) return (null, Results.Json(new { error = "unauthorised" }, statusCode: 401));
            if (!a.IsOwner) return (null, Results.Json(new { error = "forbidden" }, statusCode: 403));
            return (a, null);
        }

        app.MapPost("/api/admin/honorary", async (HttpContext ctx) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var name = (H.GetS(b, "recipient_name", "name") ?? "").Trim();
            if (name.Length < 2) return Results.Json(new { error = "recipient_name_required" }, statusCode: 400);
            if (name.Length > 200) name = name[..200];
            var citation = (H.GetS(b, "citation") ?? "").Trim(); if (citation.Length > 1000) citation = citation[..1000];
            // optional account link: explicit user_id, or an email resolved server-side
            long? userId = null;
            var uidEl = H.GetEl(b, "user_id");
            if (uidEl is { ValueKind: System.Text.Json.JsonValueKind.Number }) userId = uidEl.Value.GetInt64();
            var email = (H.GetS(b, "user_email", "email") ?? "").Trim().ToLowerInvariant();
            if (userId is null && email.Length > 0)
            {
                var u = db.QueryOne("SELECT id FROM users WHERE email=?", email);
                if (u is null) return Results.Json(new { error = "user_not_found", message = "No account exists for that email — leave it blank to confer without a linked account." }, statusCode: 404);
                userId = H.L(u["id"]);
            }
            if (userId is not null && db.QueryOne("SELECT id FROM users WHERE id=?", userId) is null)
                return Results.Json(new { error = "user_not_found" }, statusCode: 404);

            var awardNo = ConferAward(db, name, userId, citation, adm!.Id);
            if (awardNo is null) return Results.Json(new { error = "award_no_generation_failed" }, statusCode: 500);
            if (userId is not null)
                db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Recognition', 'Honorary Fellow (PCI)', ?)",
                    userId, $"The board has conferred on you the designation Honorary Fellow (PCI) — award number {awardNo}. This is an honorary recognition, distinct from PCI's examined certification credentials.");
            log(userId, "honorary_conferred", $"{awardNo} to \"{name}\" by admin {adm!.Id}");
            return J(new { ok = true, award_no = awardNo, designation = "Honorary Fellow (PCI)" });
        });

        app.MapGet("/api/admin/honorary", (HttpContext ctx) =>
        {
            var (_, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            return J(new { rows = db.Query(@"SELECT h.*, u.email AS linked_email FROM honorary_awards h LEFT JOIN users u ON u.id=h.user_id ORDER BY h.id DESC") });
        });

        app.MapPost("/api/admin/honorary/{id}/revoke", async (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne("SELECT * FROM honorary_awards WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(a["status"]) == "revoked") return Results.Json(new { error = "already_revoked" }, statusCode: 409);
            var b = await H.Body(ctx.Request);
            var reason = (H.GetS(b, "reason") ?? "").Trim(); if (reason.Length > 500) reason = reason[..500];
            // Revokes ONLY the honorary record — never any exam-earned credential.
            db.Execute("UPDATE honorary_awards SET status='revoked', revoked_by=?, revoked_at=datetime('now'), revoke_reason=? WHERE id=?", adm!.Id, reason, id);
            log(H.Ln(a["user_id"]), "honorary_revoked", $"{H.Str(a["award_no"])} by admin {adm.Id}{(reason.Length > 0 ? ": " + reason : "")}");
            return J(new { ok = true });
        });
    }
}
