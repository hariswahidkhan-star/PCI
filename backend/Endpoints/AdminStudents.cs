using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Admin student oversight + management actions (student-360). All gated by 'members'.</summary>
public static class AdminStudents
{
    static string Like(string? q) => "%" + (q ?? "") + "%";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        app.MapGet("/api/admin/members", (HttpContext ctx) => gate(ctx.Request, "members", _ =>
        {
            var q = ctx.Request.Query["q"].ToString();
            var status = ctx.Request.Query["status"].ToString();
            long.TryParse(ctx.Request.Query["limit"], out var limit); if (limit == 0) limit = 200;
            long.TryParse(ctx.Request.Query["offset"], out var offset);
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("u.status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(u.email LIKE ? OR u.first_name LIKE ? OR u.last_name LIKE ?)"); args.Add(Like(q)); args.Add(Like(q)); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            var rows = db.Query($@"SELECT u.id,u.first_name,u.last_name,u.email,u.status,u.created_at,
                m.membership_type,m.status membership_status,m.expiry_date, sp.profile_completion_percentage profile,
                (SELECT COALESCE(SUM(final_amount),0) FROM payments p WHERE p.user_id=u.id AND p.payment_status='paid') paid_total,
                (SELECT COUNT(*) FROM issued_credentials c WHERE c.user_id=u.id AND c.status='active') credentials
                FROM users u LEFT JOIN memberships m ON m.user_id=u.id LEFT JOIN student_profiles sp ON sp.user_id=u.id
                {w} ORDER BY u.id DESC LIMIT ? OFFSET ?", args.Append((object?)limit).Append((object?)offset).ToArray());
            var total = db.Scalar<long>($"SELECT COUNT(*) FROM users u {w}", args.ToArray());
            return J(new { rows, total });
        }));

        app.MapGet("/api/admin/members/{id}", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new { user = u,
                profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", id),
                membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", id),
                payments = db.Query("SELECT * FROM payments WHERE user_id=? ORDER BY id DESC", id),
                credentials = db.Query("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", id),
                sessions = db.Query("SELECT id,current_step,session_status,selected_product,last_activity_at,reminders_sent FROM enrollment_sessions WHERE user_id=? OR email=? ORDER BY id DESC", id, u["email"]),
                emails = db.Query("SELECT * FROM email_logs WHERE user_id=? OR email=? ORDER BY id DESC LIMIT 50", id, u["email"]) });
        }));

        // status uses bare admin in Node; keep parity (any admin)
        app.MapPost("/api/admin/members/{id}/status", async (HttpContext ctx, long id) =>
        {
            var a = Core.Auth.AdminFromReq(ctx.Request, db); if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var status = H.GetS(b, "status");
            if (status is not ("pending" or "active" or "deactivated")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE users SET status=?, updated_at=datetime('now') WHERE id=?", status, id);
            log(id, "member_status", status);
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/members/{id}/resend-setup", (HttpContext ctx, long id) =>
        {
            var a = Core.Auth.AdminFromReq(ctx.Request, db); if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "no_user" }, statusCode: 404);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", id, Security.Sha(token));
            log(id, "resend_setup", "");
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/members/{id}/referral-code", (HttpContext ctx, long id) =>
        {
            var a = Core.Auth.AdminFromReq(ctx.Request, db); if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var existing = db.QueryOne("SELECT code FROM discount_codes WHERE owner_user_id=? AND code_type='referral' AND active=1", id);
            if (existing is not null) return J(new { code = existing["code"], existing = true });
            var code = "REF-" + Security.RandomHex(4).ToUpperInvariant();
            try { db.Execute("INSERT INTO discount_codes(code,code_type,owner_user_id,discount_type,discount_value,applies_to,active) VALUES(?, 'referral',?, 'percent',10,'all',1)", code, id); } catch { }
            return J(new { code, existing = false });
        });

        // ---- student-360 panel ----
        app.MapGet("/api/admin/students/{id}/panel", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            bool has(string t) => db.Columns(t).Count > 0;
            var cpd = has("cpd_entries") ? db.Query("SELECT * FROM cpd_entries WHERE user_id=? ORDER BY id DESC", id) : new();
            var cpdTotal = cpd.Sum(cc => H.D(cc["hours"]));
            return J(new { user = u,
                profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", id),
                membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", id),
                payments = db.Query("SELECT * FROM payments WHERE user_id=? ORDER BY id DESC", id),
                credentials = db.Query("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", id),
                bookings = db.Query("SELECT * FROM exam_bookings WHERE user_id=? ORDER BY id DESC", id),
                attempts = db.Query("SELECT id,booking_id,kind,status,percent,result,violations,identity_result,evidence_count,review_status,client_kind,started_at,submitted_at,last_heartbeat_at FROM exam_attempts WHERE user_id=? ORDER BY id DESC", id),
                cpd, cpd_total = cpdTotal,
                tickets = has("tickets") ? db.Query("SELECT * FROM tickets WHERE user_id=? ORDER BY id DESC", id) : new(),
                logins = has("login_events") ? db.Query("SELECT * FROM login_events WHERE user_id=? ORDER BY id DESC LIMIT 20", id) : new(),
                emails = db.Query("SELECT * FROM email_logs WHERE user_id=? OR email=? ORDER BY id DESC LIMIT 50", id, u["email"]) });
        }));

        app.MapPatch("/api/admin/students/{id}/profile", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var uf = new[]{ "first_name","last_name","email" }.Where(k => b.ContainsKey(k)).ToList();
            if (uf.Count > 0) db.Execute($"UPDATE users SET {string.Join(", ", uf.Select(k => k + "=?"))} WHERE id=?", uf.Select(k => (object?)H.GetS(b, k)).Append(id).ToArray());
            var pcols = db.Columns("student_profiles");
            var pf = b.Keys.Where(k => pcols.Contains(k) && k != "user_id" && k != "id").ToList();
            if (pf.Count > 0)
            {
                if (db.QueryOne("SELECT 1 FROM student_profiles WHERE user_id=?", id) is not null)
                    db.Execute($"UPDATE student_profiles SET {string.Join(", ", pf.Select(k => k + "=?"))} WHERE user_id=?", pf.Select(k => (object?)H.GetS(b, k)).Append(id).ToArray());
                else
                    db.Execute($"INSERT INTO student_profiles(user_id,{string.Join(",", pf)}) VALUES(?,{string.Join(",", pf.Select(_ => "?"))})", (new object?[]{ id }).Concat(pf.Select(k => (object?)H.GetS(b, k))).ToArray());
            }
            log(0, "admin_edit_profile", "user " + id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/cpd", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var c = db.Columns("cpd_entries");
            var title = (H.GetS(b, "title", "activity") ?? "CPD activity"); if (title.Length > 200) title = title[..200];
            var hours = Math.Max(0, H.GetNum(b, "hours") ?? 0);
            var date = H.GetS(b, "date", "activity_date") ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
            var category = H.GetS(b, "category") ?? "General";
            var map = new Dictionary<string, object?> { ["title"] = title, ["activity"] = title, ["hours"] = hours, ["date"] = date, ["activity_date"] = date, ["category"] = category, ["description"] = title, ["user_id"] = id };
            var use = map.Keys.Where(k => c.Contains(k)).ToList();
            db.Execute($"INSERT INTO cpd_entries({string.Join(",", use)}) VALUES({string.Join(",", use.Select(_ => "?"))})", use.Select(k => map[k]).ToArray());
            log(0, "admin_add_cpd", $"user {id} +{hours}h");
            return J(new { ok = true });
        }));

        app.MapDelete("/api/admin/students/{id}/cpd/{cid}", (HttpContext ctx, long id, long cid) => gate(ctx.Request, "members", _ =>
        {
            db.Execute("DELETE FROM cpd_entries WHERE id=? AND user_id=?", cid, id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/membership", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var c = db.Columns("memberships");
            var status = H.GetS(b, "status") ?? "active";
            var renew = H.GetS(b, "renews_at", "expires_at");
            var ex = db.QueryOne("SELECT 1 FROM memberships WHERE user_id=?", id);
            var set = new List<string>(); var val = new List<object?>();
            if (c.Contains("status")) { set.Add("status=?"); val.Add(status); }
            if (c.Contains("renews_at") && renew is not null) { set.Add("renews_at=?"); val.Add(renew); }
            else if (c.Contains("expiry_date") && renew is not null) { set.Add("expiry_date=?"); val.Add(renew); }
            if (ex is not null && set.Count > 0) db.Execute($"UPDATE memberships SET {string.Join(", ", set)} WHERE user_id=?", val.Append((object?)id).ToArray());
            else if (ex is null) db.Execute("INSERT INTO memberships(user_id,status) VALUES(?,?)", id, status);
            log(0, "admin_edit_membership", $"user {id} {status}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/booking", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var when = H.GetS(b, "scheduled_at");
            if (string.IsNullOrEmpty(when)) return Results.Json(new { error = "scheduled_at_required" }, statusCode: 400);
            var tz = H.GetS(b, "timezone") ?? "UTC";
            var open = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            if (open is not null) db.Execute("UPDATE exam_bookings SET scheduled_at=?, timezone=?, updated_at=datetime('now') WHERE id=?", when, tz, open["id"]);
            else db.Execute("INSERT INTO exam_bookings(user_id,scheduled_at,timezone,status) VALUES(?,?,?, 'scheduled')", id, when, tz);
            log(0, "admin_set_booking", $"user {id} @ {when}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/booking/cancel", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var open = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            if (open is not null) db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE id=?", open["id"]);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/revoke-sessions", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session'", id);
            log(0, "admin_revoke_sessions", "user " + id);
            return J(new { ok = true });
        }));
    }
}
