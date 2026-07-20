using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Institution (training-partner) portal + the admin-side approval and fraud queues.
///
/// Partner-facing (own bearer auth via partner_users/partner_sessions; every read is scoped to the
/// session's partner_id — there is no cross-institution path):
///   POST /api/partner/auth/login | logout | password
///   GET  /api/partner/me
///   GET  /api/partner/dashboard          — code/usage/registration metrics + notices + limits
///   GET  /api/partner/codes              — own codes with status/usage
///   POST /api/partner/codes              — create a code WITHIN the PCI-configured limits
///   POST /api/partner/codes/{id}/cancel  — cancel an own unused code
///   GET  /api/partner/students           — privacy-masked sponsored registrations (own codes only)
///
/// Admin-facing:
///   GET/POST /api/admin/training-partners/{id}/users        — gate 'partners': manage institution logins
///   POST     /api/admin/partner-users/{id}/reset-password   — gate 'partners' (returns a new temp password)
///   POST     /api/admin/partner-users/{id}/status           — gate 'partners': active | suspended
///   GET      /api/admin/code-approvals                      — gate 'codes': pending institution codes
///   POST     /api/admin/code-approvals/{id}/approve|reject  — gate 'codes'
///   GET      /api/admin/fraud-flags                         — gate 'codes': abuse review queue
///   POST     /api/admin/fraud-flags/{id}/action             — gate 'codes': clear | suspend_code
///
/// Institutions never see passwords, card data, support conversations, other institutions' students,
/// or unmasked personal data beyond the configured privacy fields.
/// </summary>
public static class PartnerPortal
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult Err(int code, string error, string? message = null) => Results.Json(new { error, message }, statusCode: code);
        IResult? Need(HttpContext ctx, out PartnerCtx? p)
        {
            p = Auth.PartnerFromReq(ctx.Request, db);
            return p is null ? Results.Json(new { error = "unauthorized" }, statusCode: 401) : null;
        }
        static string MaskEmail(string? email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@')) return "•••";
            var at = email.IndexOf('@');
            var local = email[..at];
            return (local.Length <= 2 ? local[..1] + "•••" : local[..2] + "•••") + email[at..];
        }

        // ================= partner auth =================
        app.MapPost("/api/partner/auth/login", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            var password = H.GetS(b, "password") ?? "";
            var pu = db.QueryOne("SELECT * FROM partner_users WHERE email=?", email);
            if (pu is null) { LoginGuard.BurnTime(password); return Results.Json(new { error = "invalid_credentials" }, statusCode: 401); }
            if (LoginGuard.IsLocked(db, "partner_users", pu["id"]))
                return Results.Json(new { error = "account_locked", message = "Too many failed attempts. Please try again in a few minutes." }, statusCode: 429);
            if (!Security.VerifyPassword(password, pu["password_hash"] as string))
            {
                LoginGuard.OnFail(db, "partner_users", pu["id"]);
                return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
            }
            LoginGuard.OnSuccess(db, "partner_users", pu["id"]);
            if ((pu["status"] as string) != "active") return Results.Json(new { error = "account_suspended" }, statusCode: 403);
            var partner = db.QueryOne("SELECT id,name,status,agreement_end FROM training_partners WHERE id=?", pu["partner_id"]);
            if (partner is null || (partner["status"] as string ?? "active") != "active")
                return Results.Json(new { error = "institution_inactive", message = "Your institution's access is not currently active. Contact PCI." }, statusCode: 403);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO partner_sessions(partner_user_id,token,expires_at) VALUES(?,?, datetime('now','+12 hours'))", pu["id"], Security.Sha(token));
            db.Execute("UPDATE partner_users SET last_login_at=datetime('now') WHERE id=?", pu["id"]);
            log(null, "partner_login", $"partner_user {H.L(pu["id"])} ({email})");
            return J(new { token, name = H.Str(pu["name"]), role = H.Str(pu["role"]), institution = H.Str(partner["name"]),
                must_change_pw = H.L(pu["must_change_pw"]) == 1 });
        });

        app.MapPost("/api/partner/auth/logout", (HttpContext ctx) =>
        {
            var bearer = ctx.Request.Headers.Authorization.ToString();
            if (bearer.StartsWith("Bearer ")) db.Execute("DELETE FROM partner_sessions WHERE token=?", Security.Sha(bearer[7..]));
            return J(new { ok = true });
        });

        app.MapPost("/api/partner/auth/password", async (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var pw = H.GetS(b, "new_password") ?? "";
            if (pw.Length < 10) return Err(400, "weak_password", "Use at least 10 characters.");
            db.Execute("UPDATE partner_users SET password_hash=?, must_change_pw=0 WHERE id=?", BCrypt.Net.BCrypt.HashPassword(pw), p!.Id);
            // Sign out every OTHER session so a stolen token dies when the password changes (parity with admin/student).
            var self = ctx.Request.Headers.Authorization.ToString();
            var selfSha = self.StartsWith("Bearer ") ? Security.Sha(self[7..]) : "";
            db.Execute("DELETE FROM partner_sessions WHERE partner_user_id=? AND token<>?", p!.Id, selfSha);
            return J(new { ok = true });
        });

        app.MapGet("/api/partner/me", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var partner = db.QueryOne(@"SELECT id,name,tier,status,institution_type,agreement_start,agreement_end,
                max_discount_percent,max_codes,max_uses_per_code,total_allocation,allow_full_sponsorship,auto_approve_codes
                FROM training_partners WHERE id=?", p!.PartnerId);
            return J(new { user = new { p.Email, p.Name, p.Role, must_change_pw = p.MustChangePw }, institution = partner });
        });

        // ================= partner dashboard =================
        app.MapGet("/api/partner/dashboard", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var pid = p!.PartnerId;
            var codes = db.Query("SELECT id,code,status,active,discount_value,applies_to,max_uses,used_count,end_date FROM discount_codes WHERE partner_id=?", pid);
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            bool Live(Dictionary<string, object?> c) =>
                (H.Str(c["status"]) is null or "active") && H.L(c["active"]) == 1
                && (H.Str(c["end_date"]) is not { Length: > 0 } ed || string.Compare(ed, today, StringComparison.Ordinal) >= 0);
            var usedTotal = codes.Sum(c => H.L(c["used_count"]));
            var partner = db.QueryOne("SELECT total_allocation,max_codes,max_discount_percent,max_uses_per_code FROM training_partners WHERE id=?", pid);
            var allocation = partner?["total_allocation"] is null ? (long?)null : H.L(partner["total_allocation"]);
            var redemptions = db.Query(@"SELECT r.product_type, COUNT(*) n, COALESCE(SUM(r.discount_amount),0) discount_given
                FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id WHERE dc.partner_id=? GROUP BY r.product_type", pid);
            var trend = db.Query(@"SELECT date(r.redeemed_at) d, COUNT(*) n FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id
                WHERE dc.partner_id=? AND r.redeemed_at >= datetime('now','-90 days') GROUP BY d ORDER BY d", pid);
            var topCodes = codes.OrderByDescending(c => H.L(c["used_count"])).Take(5)
                .Select(c => new { code = H.Str(c["code"]), uses = H.L(c["used_count"]) }).ToList();
            var notices = db.Query("SELECT id,title,body,read_at,created_at FROM partner_notices WHERE partner_id=? ORDER BY id DESC LIMIT 10", pid);
            return J(new
            {
                codes_total = codes.Count,
                codes_active = codes.Count(Live),
                codes_expired = codes.Count(c => H.Str(c["end_date"]) is { Length: > 0 } ed && string.Compare(ed, today, StringComparison.Ordinal) < 0),
                permitted_uses = codes.Sum(c => c["max_uses"] is null ? 0 : H.L(c["max_uses"])),
                used_total = usedTotal,
                allocation, remaining = allocation is { } a ? Math.Max(0, a - usedTotal) : (long?)null,
                registrations = usedTotal,
                by_product = redemptions, trend, top_codes = topCodes, notices,
                discount_given = redemptions.Sum(r => H.D(r["discount_given"])),
            });
        });

        // ================= partner codes =================
        app.MapGet("/api/partner/codes", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            return J(new { rows = db.Query(@"SELECT id,code,status,active,discount_type,discount_value,applies_to,max_uses,used_count,
                start_date,end_date,campaign_name,rejection_reason,notes FROM discount_codes WHERE partner_id=? ORDER BY id DESC", p!.PartnerId) });
        });

        app.MapPost("/api/partner/codes", async (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            if (p!.Role is not ("admin" or "finance")) return Err(403, "role_forbidden", "Only institution admin/finance users can create codes.");
            var partner = db.QueryOne("SELECT * FROM training_partners WHERE id=?", p.PartnerId);
            if (partner is null) return Err(404, "institution_not_found");
            var b = await H.Body(ctx.Request);
            var percent = Math.Clamp(H.GetNum(b, "percent") ?? 0, 1, 100);
            var maxUses = (long)Math.Max(1, H.GetNum(b, "max_uses") ?? 1);
            var appliesTo = (H.GetS(b, "applies_to") ?? "all").Trim().ToLowerInvariant();
            if (appliesTo is not ("all" or "membership" or "exam")) appliesTo = "all";

            // The same ceilings PCI configured — validated server-side before anything is saved.
            if (partner["max_discount_percent"] is not null && percent > H.D(partner["max_discount_percent"]))
                return Results.Json(new { error = "over_percent_limit", limit = H.D(partner["max_discount_percent"]),
                    message = $"Your agreement allows codes up to {H.D(partner["max_discount_percent"]):0.#}%." }, statusCode: 422);
            if (percent >= 100 && H.L(partner["allow_full_sponsorship"]) != 1)
                return Results.Json(new { error = "full_sponsorship_not_allowed", message = "Your agreement does not include 100% sponsorship." }, statusCode: 422);
            if (partner["max_uses_per_code"] is not null && maxUses > H.L(partner["max_uses_per_code"]))
                return Results.Json(new { error = "over_uses_limit", limit = H.L(partner["max_uses_per_code"]) }, statusCode: 422);
            var codeCount = db.Scalar<long>("SELECT COUNT(*) FROM discount_codes WHERE partner_id=? AND COALESCE(status,'active') NOT IN ('rejected','cancelled')", p.PartnerId);
            if (partner["max_codes"] is not null && codeCount >= H.L(partner["max_codes"]))
                return Results.Json(new { error = "over_code_limit", limit = H.L(partner["max_codes"]) }, statusCode: 422);
            if (partner["total_allocation"] is not null)
            {
                var committed = db.Scalar<long>("SELECT COALESCE(SUM(COALESCE(max_uses,0)),0) FROM discount_codes WHERE partner_id=? AND COALESCE(status,'active') IN ('active','pending_approval')", p.PartnerId);
                if (committed + maxUses > H.L(partner["total_allocation"]))
                    return Results.Json(new { error = "over_total_allocation", limit = H.L(partner["total_allocation"]), committed }, statusCode: 422);
            }

            var codeStr = (H.GetS(b, "code") ?? "").Trim().ToUpperInvariant();
            if (codeStr.Length == 0) codeStr = "INST-" + Security.RandomHex(4).ToUpperInvariant();
            if (codeStr.Length > 40 || db.QueryOne("SELECT id FROM discount_codes WHERE code=?", codeStr) is not null)
                return Err(409, "code_taken");
            var draft = H.GetEl(b, "draft") is { ValueKind: JsonValueKind.True };
            var autoApprove = H.L(partner["auto_approve_codes"]) == 1;
            var status = draft ? "draft" : autoApprove ? "active" : "pending_approval";
            var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,start_date,end_date,max_uses,active,code_type,org_name,partner_id,created_by_partner_user,campaign_name,notes,status)
                VALUES(?, 'percentage', ?, ?, ?, ?, ?, ?, 'institution', ?, ?, ?, ?, ?, ?)",
                codeStr, percent, appliesTo, H.GetS(b, "start_date"), H.GetS(b, "end_date"), maxUses,
                status == "active" ? 1 : 0, H.Str(partner["name"]), p.PartnerId, p.Id,
                H.GetS(b, "campaign_name"), H.GetS(b, "notes"), status);
            if (status == "pending_approval")
                try { Notify.Alert(db, "partners", $"Institution code awaiting approval: {codeStr}",
                    $"<p>{System.Net.WebUtility.HtmlEncode(H.Str(partner["name"]) ?? "")} submitted code <b>{codeStr}</b> ({percent:0.#}% × {maxUses}) for approval.</p>", "discount_code", codeId); } catch { }
            log(null, "partner_code_created", $"partner {p.PartnerId} code {codeStr} {percent:0.#}% x{maxUses} status {status}");
            return J(new { ok = true, id = codeId, code = codeStr, status,
                note = status switch { "active" => "Code is live.", "pending_approval" => "Submitted for PCI approval — you will be notified.", _ => "Saved as a draft." } });
        });

        app.MapPost("/api/partner/codes/{id}/cancel", (HttpContext ctx, long id) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var c = db.QueryOne("SELECT id,used_count FROM discount_codes WHERE id=? AND partner_id=?", id, p!.PartnerId);
            if (c is null) return Err(404, "not_found");
            db.Execute("UPDATE discount_codes SET status='cancelled', active=0 WHERE id=?", id);
            log(null, "partner_code_cancelled", $"code {id} by partner_user {p.Id}");
            return J(new { ok = true });
        });

        // ================= partner students (privacy-masked) =================
        app.MapGet("/api/partner/students", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var partner = db.QueryOne("SELECT privacy_fields FROM training_partners WHERE id=?", p!.PartnerId);
            var extra = new HashSet<string>();
            try
            {
                if (H.Str(partner?["privacy_fields"]) is { Length: > 0 } pf)
                    foreach (var el in JsonDocument.Parse(pf).RootElement.EnumerateArray())
                        if (el.GetString() is { } f) extra.Add(f);
            }
            catch { }
            var rows = db.Query(@"SELECT r.redeemed_at, r.code, r.product_type, r.discount_amount, r.amount_before,
                       u.id uid, u.first_name, u.last_name, u.email, p2.payment_status,
                       (SELECT m.status FROM memberships m WHERE m.user_id=u.id) membership_status
                FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id
                LEFT JOIN users u ON u.id=r.user_id LEFT JOIN payments p2 ON p2.id=r.payment_id
                WHERE dc.partner_id=? ORDER BY r.redeemed_at DESC LIMIT 500", p.PartnerId);
            // Minimum data by default: masked email + programme + status. Names and certification status
            // appear only when PCI has switched those fields on for this institution.
            var outRows = rows.Select(r => new Dictionary<string, object?>
            {
                ["registered_at"] = r["redeemed_at"],
                ["code"] = r["code"],
                ["programme"] = r["product_type"],
                ["email_masked"] = MaskEmail(H.Str(r["email"])),
                ["discount_amount"] = r["discount_amount"],
                ["payment_status"] = r["payment_status"],
                ["membership_status"] = r["membership_status"],
                ["name"] = extra.Contains("name") ? ($"{H.Str(r["first_name"])} {H.Str(r["last_name"])}".Trim()) : null,
            }).ToList();
            return J(new { rows = outRows, fields_authorised = extra.ToArray() });
        });

        // ================= admin: institution logins =================
        app.MapGet("/api/admin/training-partners/{id}/users", (HttpContext ctx, long id) => gate(ctx.Request, "partners", _ =>
            J(new { rows = db.Query("SELECT id,email,name,role,status,must_change_pw,last_login_at,created_at FROM partner_users WHERE partner_id=? ORDER BY id", id) })));

        app.MapPost("/api/admin/training-partners/{id}/users", (HttpContext ctx, long id) => gate(ctx.Request, "partners", adm =>
        {
            if (db.QueryOne("SELECT id FROM training_partners WHERE id=?", id) is null) return Err(404, "institution_not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            if (!email.Contains('@')) return Err(400, "bad_email");
            if (db.QueryOne("SELECT id FROM partner_users WHERE email=?", email) is not null) return Err(409, "email_taken");
            var role = (H.GetS(b, "role") ?? "admin").Trim().ToLowerInvariant();
            if (role is not ("admin" or "finance" or "reporting" or "support")) return Err(400, "bad_role");
            var temp = Security.RandomHex(5);
            var uid = db.ExecuteReturningId("INSERT INTO partner_users(partner_id,email,name,role,password_hash,created_by) VALUES(?,?,?,?,?,?)",
                id, email, H.GetS(b, "name"), role, BCrypt.Net.BCrypt.HashPassword(temp), adm.Id);
            log(null, "partner_user_created", $"{email} ({role}) for partner {id} by {adm.Id}");
            return J(new { ok = true, id = uid, email, role, temp_password = temp, portal_url = "/partner.html" });
        }));

        app.MapPost("/api/admin/partner-users/{id}/reset-password", (HttpContext ctx, long id) => gate(ctx.Request, "partners", adm =>
        {
            if (db.QueryOne("SELECT id FROM partner_users WHERE id=?", id) is null) return Err(404, "not_found");
            var temp = Security.RandomHex(5);
            db.Execute("UPDATE partner_users SET password_hash=?, must_change_pw=1 WHERE id=?", BCrypt.Net.BCrypt.HashPassword(temp), id);
            db.Execute("DELETE FROM partner_sessions WHERE partner_user_id=?", id);
            log(null, "partner_user_pw_reset", $"{id} by {adm.Id}");
            return J(new { ok = true, temp_password = temp });
        }));

        app.MapPost("/api/admin/partner-users/{id}/status", (HttpContext ctx, long id) => gate(ctx.Request, "partners", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = (H.GetS(b, "status") ?? "").Trim();
            if (status is not ("active" or "suspended")) return Err(400, "bad_status");
            db.Execute("UPDATE partner_users SET status=? WHERE id=?", status, id);
            if (status == "suspended") db.Execute("DELETE FROM partner_sessions WHERE partner_user_id=?", id);
            log(null, "partner_user_status", $"{id} → {status} by {adm.Id}");
            return J(new { ok = true });
        }));

        // ================= admin: code approval queue =================
        app.MapGet("/api/admin/code-approvals", (HttpContext ctx) => gate(ctx.Request, "codes", _ =>
            J(new { rows = db.Query(@"SELECT dc.id,dc.code,dc.discount_value,dc.applies_to,dc.max_uses,dc.start_date,dc.end_date,dc.campaign_name,dc.notes,
                tp.name institution, pu.email requested_by
                FROM discount_codes dc LEFT JOIN training_partners tp ON tp.id=dc.partner_id LEFT JOIN partner_users pu ON pu.id=dc.created_by_partner_user
                WHERE dc.status='pending_approval' ORDER BY dc.id") })));

        app.MapPost("/api/admin/code-approvals/{id}/approve", (HttpContext ctx, long id) => gate(ctx.Request, "codes", adm =>
        {
            var c = db.QueryOne("SELECT * FROM discount_codes WHERE id=? AND status='pending_approval'", id);
            if (c is null) return Err(404, "not_pending");
            db.Execute("UPDATE discount_codes SET status='active', active=1, approved_by=?, approved_at=datetime('now') WHERE id=?", adm.Id, id);
            if (c["partner_id"] is not null)
                db.Execute("INSERT INTO partner_notices(partner_id,title,body) VALUES(?,?,?)", H.L(c["partner_id"]),
                    $"Code {H.Str(c["code"])} approved", "Your discount code has been approved by PCI and is now live.");
            log(null, "code_approved", $"{H.Str(c["code"])} by {adm.Id}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/code-approvals/{id}/reject", (HttpContext ctx, long id) => gate(ctx.Request, "codes", adm =>
        {
            var c = db.QueryOne("SELECT * FROM discount_codes WHERE id=? AND status='pending_approval'", id);
            if (c is null) return Err(404, "not_pending");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length == 0) return Err(400, "reason_required");
            db.Execute("UPDATE discount_codes SET status='rejected', active=0, rejection_reason=? WHERE id=?", reason, id);
            if (c["partner_id"] is not null)
                db.Execute("INSERT INTO partner_notices(partner_id,title,body) VALUES(?,?,?)", H.L(c["partner_id"]),
                    $"Code {H.Str(c["code"])} rejected", $"PCI rejected this code: {reason}. You can amend and resubmit it.");
            log(null, "code_rejected", $"{H.Str(c["code"])} by {adm.Id}: {reason}");
            return J(new { ok = true });
        }));

        // ================= admin: fraud review queue =================
        app.MapGet("/api/admin/fraud-flags", (HttpContext ctx) => gate(ctx.Request, "codes", _ =>
            J(new { rows = db.Query(@"SELECT f.*, dc.code FROM fraud_flags f LEFT JOIN discount_codes dc ON dc.id=f.code_id
                WHERE f.status='open' ORDER BY f.id DESC LIMIT 100") })));

        // ================= admin: discount utilisation report (+ audited CSV export) =================
        app.MapGet("/api/admin/reports/discounts", (HttpContext ctx) => gate(ctx.Request, "reports", adm =>
        {
            var rows = db.Query(@"SELECT dc.code, dc.code_type, COALESCE(tp.name, dc.org_name) institution, dc.discount_value, dc.applies_to,
                       COALESCE(dc.status, CASE WHEN dc.active=1 THEN 'active' ELSE 'inactive' END) status,
                       dc.max_uses, dc.used_count, dc.end_date,
                       COALESCE((SELECT SUM(r.discount_amount) FROM code_redemptions r WHERE r.code_id=dc.id),0) discount_given,
                       COALESCE((SELECT SUM(p.final_amount) FROM code_redemptions r JOIN payments p ON p.id=r.payment_id WHERE r.code_id=dc.id AND p.payment_status='paid'),0) revenue
                FROM discount_codes dc LEFT JOIN training_partners tp ON tp.id=dc.partner_id
                ORDER BY dc.used_count DESC, dc.id DESC LIMIT 500");
            if (ctx.Request.Query["format"] == "csv")
            {
                // Personal/financial exports are themselves auditable events.
                log(null, "export", $"discount report CSV by admin {adm.Id}");
                var sb = new System.Text.StringBuilder("code,type,institution,percent,applies_to,status,max_uses,used,expires,discount_given,revenue\n");
                foreach (var r in rows)
                {
                    string C(object? v) { var s2 = H.Str(v) ?? v?.ToString() ?? ""; return s2.Contains(',') || s2.Contains('"') ? "\"" + s2.Replace("\"", "\"\"") + "\"" : s2; }
                    sb.Append(string.Join(',', new[] { C(r["code"]), C(r["code_type"]), C(r["institution"]), C(r["discount_value"]), C(r["applies_to"]), C(r["status"]), C(r["max_uses"]), C(r["used_count"]), C(r["end_date"]), C(r["discount_given"]), C(r["revenue"]) })).Append('\n');
                }
                return Results.Text(sb.ToString(), "text/csv");
            }
            var unused = rows.Count(r => H.L(r["used_count"]) == 0);
            var expired = rows.Count(r => H.Str(r["end_date"]) is { Length: > 0 } ed && string.Compare(ed, DateTime.UtcNow.ToString("yyyy-MM-dd"), StringComparison.Ordinal) < 0);
            return J(new { rows, totals = new { codes = rows.Count, unused, expired,
                discount_given = rows.Sum(r => H.D(r["discount_given"])), revenue = rows.Sum(r => H.D(r["revenue"])) } });
        }));

        app.MapPost("/api/admin/fraud-flags/{id}/action", (HttpContext ctx, long id) => gate(ctx.Request, "codes", adm =>
        {
            var f = db.QueryOne("SELECT * FROM fraud_flags WHERE id=? AND status='open'", id);
            if (f is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var action = (H.GetS(b, "action") ?? "").Trim();
            if (action is not ("clear" or "suspend_code")) return Err(400, "bad_action", "action must be clear or suspend_code");
            if (action == "suspend_code" && f["code_id"] is not null)
                db.Execute("UPDATE discount_codes SET status='suspended', active=0 WHERE id=?", H.L(f["code_id"]));
            db.Execute("UPDATE fraud_flags SET status=?, actioned_by=?, actioned_at=datetime('now') WHERE id=?",
                action == "clear" ? "cleared" : "actioned", adm.Id, id);
            log(null, "fraud_flag_" + action, $"flag {id} by {adm.Id}");
            return J(new { ok = true, action });
        }));
    }
}
