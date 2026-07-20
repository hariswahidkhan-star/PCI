using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Remaining routes: legacy /students, admin tickets, codes-v2/generate/redemptions,
/// reports, and enrolment/session save/resume/start (completes 123/123 parity).</summary>
public static class AdminExtra
{
    static string GenCode(string prefix)
    {
        var p = System.Text.RegularExpressions.Regex.Replace((prefix ?? "").ToUpperInvariant(), "[^A-Z0-9]", "");
        return (p.Length > 0 ? p + "-" : "") + Security.RandomHex(4).ToUpperInvariant();
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult? Deny(HttpRequest req, string section)
        {
            var a = adminFromReq(req);
            if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            if (!a.IsOwner && !a.Perms.Contains(section)) return Results.Json(new { error = "forbidden", section }, statusCode: 403);
            return null;
        }
        var rx = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // ---------- legacy /students (flat roster) ----------
        app.MapGet("/api/admin/students", (HttpRequest req) => gate(req, "members", _ => J(db.Query(@"
            SELECT u.id,u.first_name,u.last_name,u.email,u.status,m.membership_type,m.status AS membership_status,
              u.created_at, sp.profile_completion_percentage AS profile,
              (SELECT final_amount FROM payments p WHERE p.user_id=u.id AND p.payment_status='paid' ORDER BY p.id DESC LIMIT 1) AS amount_paid,
              (SELECT reference FROM payments p WHERE p.user_id=u.id ORDER BY p.id DESC LIMIT 1) AS payment_ref
            FROM users u LEFT JOIN memberships m ON m.user_id=u.id LEFT JOIN student_profiles sp ON sp.user_id=u.id ORDER BY u.id DESC"))));

        // ---------- admin tickets ----------
        app.MapGet("/api/admin/tickets", (HttpRequest req) => gate(req, "tickets", _ =>
        {
            var status = req.Query["status"].ToString();
            var w = string.IsNullOrEmpty(status) ? "" : "WHERE t.status=?";
            var args = string.IsNullOrEmpty(status) ? Array.Empty<object?>() : new object?[]{ status };
            return J(new { rows = db.Query($@"SELECT t.*, u.email, u.first_name, u.last_name,
                (SELECT COUNT(*) FROM ticket_messages m WHERE m.ticket_id=t.id) msg_count
                FROM tickets t JOIN users u ON u.id=t.user_id {w} ORDER BY t.updated_at DESC", args) });
        }));
        app.MapGet("/api/admin/tickets/{id}", (HttpRequest req, long id) =>
        {
            var g = Deny(req, "tickets"); if (g is not null) return g;
            var t = db.QueryOne("SELECT t.*, u.email,u.first_name,u.last_name FROM tickets t JOIN users u ON u.id=t.user_id WHERE t.id=?", id);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var copy = new Dictionary<string, object?>(t) { ["messages"] = db.Query("SELECT sender,body,created_at FROM ticket_messages WHERE ticket_id=? ORDER BY id", id) };
            return J(copy);
        });
        app.MapPost("/api/admin/tickets/{id}/reply", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "tickets"); if (g is not null) return g;
            var b = await H.Body(req);
            var message = H.GetS(b, "message");
            if (string.IsNullOrEmpty(message)) return Results.Json(new { error = "missing_message" }, statusCode: 400);
            if (message.Length > 4000) message = message[..4000];
            db.Execute("INSERT INTO ticket_messages(ticket_id,sender,body) VALUES(?,?,?)", id, "admin", message);
            db.Execute("UPDATE tickets SET status='awaiting_student', updated_at=datetime('now'), first_response_at=COALESCE(first_response_at, datetime('now')) WHERE id=?", id);
            log(adminFromReq(req)?.Id, "ticket_replied", id.ToString());
            return J(new { ok = true });
        });
        app.MapPost("/api/admin/tickets/{id}/status", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "tickets"); if (g is not null) return g;
            var b = await H.Body(req);
            var status = H.GetS(b, "status");
            if (status is not ("open" or "awaiting_student" or "resolved" or "closed")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE tickets SET status=?, updated_at=datetime('now') WHERE id=?", status, id);
            log(adminFromReq(req)?.Id, "ticket_status", $"{id} → {status}");
            return J(new { ok = true });
        });

        // ---------- codes v2 / generate / redemptions ----------
        app.MapGet("/api/admin/codes-v2", (HttpRequest req) =>
        {
            var g = Deny(req, "codes"); if (g is not null) return g;
            var type = req.Query["type"].ToString(); var q = req.Query["q"].ToString(); var batch = req.Query["batch"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(type)) { where.Add("c.code_type=?"); args.Add(type); }
            if (!string.IsNullOrEmpty(batch)) { where.Add("c.batch_id=?"); args.Add(batch); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(c.code LIKE ? OR c.org_name LIKE ?)"); args.Add("%" + q + "%"); args.Add("%" + q + "%"); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($@"SELECT c.*, u.first_name||' '||u.last_name AS owner_name,
                (SELECT COUNT(*) FROM code_redemptions r WHERE r.code_id=c.id) redemptions,
                (SELECT COALESCE(SUM(r.discount_amount),0) FROM code_redemptions r WHERE r.code_id=c.id) discount_given,
                (SELECT COALESCE(SUM(p.final_amount),0) FROM code_redemptions r JOIN payments p ON p.id=r.payment_id WHERE r.code_id=c.id) revenue_attributed
                FROM discount_codes c LEFT JOIN users u ON u.id=c.owner_user_id {w} ORDER BY c.id DESC", args.ToArray()) });
        });
        app.MapPost("/api/admin/codes/generate", async (HttpRequest req) =>
        {
            var g = Deny(req, "codes"); if (g is not null) return g;
            var b = await H.Body(req);
            var count = (int)Math.Min(Math.Max(1, H.GetNum(b, "count") ?? 10), 1000);
            var prefix = H.GetS(b, "prefix") ?? "PCI";
            var discountType = H.GetS(b, "discount_type") ?? "percentage";
            var discountValue = H.GetNum(b, "discount_value") ?? 10;
            var appliesTo = H.GetS(b, "applies_to") ?? "all";
            var codeType = H.GetS(b, "code_type") ?? "campaign";
            var orgName = H.GetS(b, "org_name");
            object? maxUses = H.GetNum(b, "max_uses") ?? 1;
            object? perUser = H.GetNum(b, "per_user_limit") ?? 1;
            var endDate = H.GetS(b, "end_date");
            var notes = H.GetS(b, "notes");
            var batch = System.Text.RegularExpressions.Regex.Replace((prefix).ToUpperInvariant(), "[^A-Z0-9]", "");
            batch = (batch.Length > 12 ? batch[..12] : batch) + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("X");
            var outCodes = new List<string>(); int guard = 0;
            while (outCodes.Count < count && guard < count * 20)
            {
                guard++;
                try
                {
                    var c = GenCode(prefix);
                    db.Execute(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,end_date,max_uses,per_user_limit,active,code_type,org_name,batch_id,notes) VALUES(?,?,?,?,?,?,?,1,?,?,?,?)",
                        c, discountType, discountValue, appliesTo, endDate, maxUses, perUser, codeType, orgName, batch, notes);
                    outCodes.Add(c);
                }
                catch { }
            }
            log(adminFromReq(req)?.Id, "codes_generated", $"{batch} x{outCodes.Count}");
            return J(new { batch_id = batch, count = outCodes.Count, codes = outCodes });
        });
        app.MapGet("/api/admin/codes/{id}/redemptions", (HttpRequest req, long id) => gate(req, "codes", _ => J(new
        {
            rows = db.Query("SELECT r.*, p.reference, p.final_amount FROM code_redemptions r LEFT JOIN payments p ON p.id=r.payment_id WHERE r.code_id=? ORDER BY r.id DESC", id)
        })));

        // ---------- reports ----------
        app.MapGet("/api/admin/reports", (HttpRequest req) =>
        {
            var g = Deny(req, "reports"); if (g is not null) return g;
            var radm = adminFromReq(req)!;
            var dOK = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{2}-\d{2}$");
            var toQ = req.Query["to"].ToString(); var fromQ = req.Query["from"].ToString();
            var to = dOK.IsMatch(toQ) ? toQ : DateTime.UtcNow.ToString("yyyy-MM-dd");
            var from = dOK.IsMatch(fromQ) ? fromQ : DateTime.UtcNow.AddDays(-29).ToString("yyyy-MM-dd");
            // Test accounts (users.is_test=1) never reach financial reporting — their settlements are
            // real rows for workflow purposes but must not count as revenue, orders, or funnel results.
            string P0(string a) => $"{a}payment_status='paid' AND date({a}payment_date) BETWEEN ? AND ? AND COALESCE({a}user_id,0) NOT IN (SELECT id FROM users WHERE is_test=1)";
            var P = P0(""); var Pp = P0("p.");
            return J(new
            {
                from, to,
                totals = db.QueryOne($"SELECT COUNT(*) payments, COALESCE(SUM(final_amount),0) revenue, COALESCE(SUM(discount_code_amount),0) code_discounts, COALESCE(SUM(default_discount_amount),0) standard_discounts, COALESCE(AVG(final_amount),0) avg_order FROM payments WHERE {P}", from, to),
                revenue_daily = db.Query($"SELECT date(payment_date) d, COALESCE(SUM(final_amount),0) revenue, COUNT(*) n FROM payments WHERE {P} GROUP BY d ORDER BY d", from, to),
                by_product = db.Query($"SELECT product_type, COUNT(*) n, COALESCE(SUM(final_amount),0) revenue FROM payments WHERE {P} GROUP BY product_type ORDER BY revenue DESC", from, to),
                // Revenue attributed to each certification (payments carry it through exam_entitlements; one
                // entitlement per payment, so the sum is exact). Payments with no entitlement show as Unattributed.
                by_certification = db.Query($@"SELECT COALESCE(c.acronym, c.code, 'Unattributed (membership/other)') certification,
                        COUNT(*) n, COALESCE(SUM(p.final_amount),0) revenue
                    FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id LEFT JOIN certifications c ON c.id=e.certification_id
                    WHERE {Pp}{radm.CertFilterSql("e.certification_id")} GROUP BY COALESCE(e.certification_id,0) ORDER BY revenue DESC", from, to),
                // Certificates issued per credential in the window.
                certificates_by_certification = db.Query($@"SELECT COALESCE(c.acronym, c.code, '—') certification, COUNT(*) issued
                    FROM issued_credentials ic LEFT JOIN certifications c ON c.id=ic.certification_id
                    WHERE date(ic.issued_at) BETWEEN ? AND ?{radm.CertFilterSql("ic.certification_id")} GROUP BY ic.certification_id ORDER BY issued DESC", from, to),
                codes_perf = db.Query("SELECT c.code, c.code_type, c.org_name, COUNT(r.id) uses, COALESCE(SUM(r.discount_amount),0) discount_given, COALESCE(SUM(p.final_amount),0) revenue FROM code_redemptions r JOIN discount_codes c ON c.id=r.code_id JOIN payments p ON p.id=r.payment_id WHERE date(r.redeemed_at) BETWEEN ? AND ? GROUP BY c.id ORDER BY revenue DESC LIMIT 25", from, to),
                top_referrers = db.Query("SELECT u.first_name||' '||u.last_name name, u.email, c.code, COUNT(r.id) referrals, COALESCE(SUM(p.final_amount),0) revenue FROM discount_codes c JOIN users u ON u.id=c.owner_user_id LEFT JOIN code_redemptions r ON r.code_id=c.id LEFT JOIN payments p ON p.id=r.payment_id WHERE c.code_type='referral' GROUP BY c.id HAVING referrals>0 ORDER BY referrals DESC LIMIT 15"),
                by_country = db.Query($"SELECT COALESCE(NULLIF(sp.country,''),'Unknown') country, COUNT(*) n, COALESCE(SUM(p.final_amount),0) revenue FROM payments p LEFT JOIN student_profiles sp ON sp.user_id=p.user_id WHERE {Pp} GROUP BY country ORDER BY revenue DESC LIMIT 12", from, to),
                funnel = new
                {
                    started = db.Scalar<long>("SELECT COUNT(*) FROM enrollment_sessions WHERE date(created_at) BETWEEN ? AND ?", from, to),
                    paid = db.Scalar<long>($"SELECT COUNT(*) FROM payments WHERE {P}", from, to)
                },
                new_members = db.Scalar<long>("SELECT COUNT(*) FROM users WHERE date(created_at) BETWEEN ? AND ? AND COALESCE(is_test,0)=0", from, to)
            });
        });

        // ---------- enrolment sessions: start / save / resume (public wizard) ----------
        app.MapPost("/api/session/start", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant().Trim();
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            var user = db.QueryOne("SELECT * FROM users WHERE email=?", email);
            var session = db.QueryOne("SELECT * FROM enrollment_sessions WHERE email=? AND session_status='in_progress' ORDER BY id DESC LIMIT 1", email);
            var token = Security.RandomHex(32);
            var expiry = H.IsoFromMillis(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 7L * 86400_000);
            long sessionId; long step;
            if (session is not null)
            {
                sessionId = H.L(session["id"]); step = H.L(session["current_step"]);
                db.Execute("UPDATE enrollment_sessions SET resume_token_hash=?, resume_token_expiry=?, last_activity_at=datetime('now') WHERE id=?", Security.Sha(token), expiry, sessionId);
            }
            else
            {
                sessionId = db.ExecuteReturningId("INSERT INTO enrollment_sessions(email,user_id,current_step,session_status,resume_token_hash,resume_token_expiry) VALUES(?,?,?, 'in_progress',?,?)",
                    email, user?["id"], 1, Security.Sha(token), expiry);
                step = 1;
            }
            return J(new { session_id = sessionId, current_step = step, resumed = step != 0 });
        });
        app.MapPost("/api/session/save", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant();
            var s = db.QueryOne("SELECT * FROM enrollment_sessions WHERE email=? AND session_status='in_progress' ORDER BY id DESC LIMIT 1", email);
            if (s is null) return Results.Json(new { error = "no_session" }, statusCode: 404);
            var snap = H.GetEl(b, "pricing_snapshot");
            db.Execute("UPDATE enrollment_sessions SET current_step=?, selected_product=?, pricing_snapshot=?, last_activity_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                H.GetNum(b, "current_step") ?? H.D(s["current_step"]), H.GetS(b, "selected_product") ?? H.Str(s["selected_product"]),
                snap is not null ? snap.Value.GetRawText() : H.Str(s["pricing_snapshot"]), s["id"]);
            return J(new { ok = true });
        });
        app.MapGet("/api/session/resume", (HttpRequest req) =>
        {
            var s = db.QueryOne("SELECT * FROM enrollment_sessions WHERE resume_token_hash=? AND resume_token_expiry > datetime('now')", Security.Sha(req.Query["token"].ToString()));
            if (s is null) return Results.Json(new { error = "invalid_or_expired" }, statusCode: 404);
            object? snap = null; try { snap = JsonSerializer.Deserialize<object>(H.Str(s["pricing_snapshot"]) ?? "null"); } catch { }
            return J(new { email = s["email"], current_step = s["current_step"], selected_product = s["selected_product"], pricing_snapshot = snap });
        });

        // ---------- enrolment save / resume (v2 wizard) ----------
        app.MapPost("/api/enrollment/save", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant().Trim();
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            var step = (long)Math.Max(1, Math.Min(9, H.GetNum(b, "step") ?? 1));
            var snap = H.GetEl(b, "data");
            var snapStr = snap is not null ? snap.Value.GetRawText() : "{}";
            var ex = db.QueryOne("SELECT id,resume_token_hash FROM enrollment_sessions WHERE email=? AND session_status='in_progress' ORDER BY id DESC", email);
            string? token = H.GetS(b, "resume_token");
            // #10 — to UPDATE an existing in-progress session the caller must present its resume token
            // (matched by hash). Without a valid token we never touch an existing row keyed only on email;
            // instead we start a fresh session with a new token. This stops one email from overwriting
            // another party's saved enrolment.
            var tokenMatches = ex is not null && !string.IsNullOrEmpty(token)
                && string.Equals(H.Str(ex["resume_token_hash"]), Security.Sha(token!), StringComparison.Ordinal);
            if (ex is not null && tokenMatches)
                db.Execute("UPDATE enrollment_sessions SET current_step=?, pricing_snapshot=?, last_activity_at=datetime('now') WHERE id=?", step, snapStr, ex["id"]);
            else { token = Security.RandomHex(20); db.Execute("INSERT INTO enrollment_sessions(email,current_step,session_status,resume_token_hash,pricing_snapshot) VALUES(?,?, 'in_progress', ?, ?)", email, step, Security.Sha(token), snapStr); }
            return J(new { ok = true, step, resume_token = token, message = "Your progress is saved. Keep the resume link to return later." });
        });
        app.MapGet("/api/enrollment/resume", (HttpRequest req) =>
        {
            var token = req.Query["token"].ToString();
            // SECURITY: resume requires a valid, unexpired token. Email alone must never resume a
            // session (that would let anyone resume another person's enrolment by guessing an email).
            if (string.IsNullOrWhiteSpace(token)) return Results.Json(new { error = "token_required" }, statusCode: 400);
            var row = db.QueryOne("SELECT * FROM enrollment_sessions WHERE resume_token_hash=? AND session_status='in_progress' AND (resume_token_expiry IS NULL OR resume_token_expiry > datetime('now'))", Security.Sha(token));
            if (row is null) return Results.Json(new { error = "invalid_or_expired" }, statusCode: 404);
            object? data = null; try { data = JsonSerializer.Deserialize<object>(H.Str(row["pricing_snapshot"]) ?? "{}"); } catch { }
            return J(new { email = row["email"], step = row["current_step"], data, last_activity_at = row["last_activity_at"] });
        });
    }
}
