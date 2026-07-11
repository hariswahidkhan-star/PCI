using System.Text;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Remaining admin surface: generic CRUD factory (8 tables), overview, codes, pricing,
/// credentials, inquiries, emails, audit, export, exams, abandoned, content/pages/subscribers/forms.</summary>
public static class AdminMgmt
{
    static string Like(string? q) => "%" + System.Text.RegularExpressions.Regex.Replace(q ?? "", "[%_]", "") + "%";
    // Interpret a JSON value as a boolean flag robustly: true/1/"true"/"1"/"yes" => true; everything
    // else (false, 0, "false", "0", "", null) => false. A raw JsonValueKind.False check alone lets
    // {"active":0} or {"active":"false"} slip through as "not false".
    static bool JsonFlag(JsonElement v) => v.ValueKind switch
    {
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Number => v.TryGetInt64(out var n) && n != 0,
        JsonValueKind.String => (v.GetString() ?? "").Trim().ToLowerInvariant() is "1" or "true" or "yes",
        _ => false,
    };
    static string CsvEsc(object? v)
    {
        var s = v?.ToString() ?? "";
        if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^[=+\-@]")) s = "'" + s;
        return System.Text.RegularExpressions.Regex.IsMatch(s, "[\",\n]") ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }
    static string ToCsv(List<Dictionary<string, object?>> rows)
    {
        if (rows.Count == 0) return "";
        var cols = rows[0].Keys.ToList();
        var sb = new StringBuilder();
        sb.Append(string.Join(",", cols)).Append('\n');
        foreach (var r in rows) sb.Append(string.Join(",", cols.Select(k => CsvEsc(r.GetValueOrDefault(k))))).Append('\n');
        return sb.ToString().TrimEnd('\n');
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        // bare-admin guard (parity with `admin` middleware): any authenticated admin
        IResult? Bare(HttpRequest req)
        {
            var a = adminFromReq(req);
            return a is null ? Results.Json(new { error = "unauthorized" }, statusCode: 401) : null;
        }
        // Inline section gate for async-lambda handlers: null = allowed, else the 401/403 result.
        IResult? Deny(HttpRequest req, string section)
        {
            var a = adminFromReq(req);
            if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            if (!a.IsOwner && !a.Perms.Contains(section)) return Results.Json(new { error = "forbidden", section }, statusCode: 403);
            return null;
        }

        // ---------- generic CRUD factory (mirror of crud() in server.js) ----------
        // Every CRUD verb is gated by the content type's RBAC section (not just "any admin").
        // Mutations bump the public renderers: several of these tables (faqs, nav_items, news,
        // resources, bok_domains, governance_roles) are served INTO the public pages server-side.
        void Crud(string name, string[] cols, string order, string section)
        {
            void Changed() { ListSections.Bump(); PageContent.Bump(); CertCatalogue.Bump(); PriceTags.Bump(); }
            // Backtick identifiers so reserved words (e.g. media_assets.`usage`) are valid on BOTH providers
            // — SQLite accepts backtick-quoted identifiers for MySQL compatibility. cols/name are code
            // constants (not request input), so this is not an injection surface.
            string Q(string c) => "`" + c + "`";
            var colList = string.Join(",", cols.Select(Q));
            app.MapGet($"/api/admin/{name}", (HttpRequest req) => gate(req, section, _ => J(new { rows = db.Query($"SELECT * FROM `{name}` ORDER BY {order}") })));
            app.MapPost($"/api/admin/{name}", (HttpRequest req) => gate(req, section, adm =>
            {
                var b = H.Body(req).GetAwaiter().GetResult();
                var vals = cols.Select(c => (object?)(b.TryGetValue(c, out var v) && v.ValueKind != JsonValueKind.Null
                    ? (v.ValueKind == JsonValueKind.String ? v.GetString() : v.ValueKind == JsonValueKind.True ? 1 : v.ValueKind == JsonValueKind.False ? 0 : v.ToString())
                    : null)).ToArray();
                try
                {
                    var id = db.ExecuteReturningId($"INSERT INTO `{name}`({colList}) VALUES({string.Join(",", cols.Select(_ => "?"))})", vals);
                    log(adm.Id, name + "_create", id.ToString());
                    Changed();
                    return J(new { id });
                }
                catch { return Results.Json(new { error = "constraint", message = "Could not save — a required field is blank or a unique value is already in use." }, statusCode: 409); }
            }));
            app.MapPatch($"/api/admin/{name}/{{id}}", (HttpRequest req, long id) => gate(req, section, adm =>
            {
                var b = H.Body(req).GetAwaiter().GetResult();
                var set = cols.Where(c => b.ContainsKey(c)).ToList();
                if (set.Count == 0) return J(new { ok = true });
                var vals = set.Select(c => { var v = b[c]; return (object?)(v.ValueKind == JsonValueKind.String ? v.GetString() : v.ValueKind == JsonValueKind.True ? 1 : v.ValueKind == JsonValueKind.False ? 0 : v.ValueKind == JsonValueKind.Null ? null : v.ToString()); }).Append((object?)id).ToArray();
                try
                {
                    db.Execute($"UPDATE `{name}` SET {string.Join(",", set.Select(c => Q(c) + "=?"))} WHERE id=?", vals);
                    log(adm.Id, name + "_update", id.ToString());
                    Changed();
                    return J(new { ok = true });
                }
                catch { return Results.Json(new { error = "constraint", message = "Could not save — a unique value is already in use." }, statusCode: 409); }
            }));
            app.MapDelete($"/api/admin/{name}/{{id}}", (HttpRequest req, long id) => gate(req, section, adm =>
            {
                db.Execute($"DELETE FROM `{name}` WHERE id=?", id);
                log(adm.Id, name + "_delete", id.ToString());
                Changed();
                return J(new { ok = true });
            }));
        }
        Crud("faqs", new[]{ "question","answer","category","sort_order","published" }, "sort_order, id", "faqs");
        Crud("bok_domains", new[]{ "code","name","weight","description","bullets","sort_order" }, "sort_order, id", "bok");
        Crud("sample_questions", new[]{ "question","options","option_a","option_b","option_c","option_d","answer_index","domain","published","sort_order","is_practice","certification_id" }, "sort_order, id", "sampleq");

        // ---------- certifications (multi-credential management; exam section) ----------
        // Deliberately NOT the generic Crud: a certification that has ever issued a credential or
        // has a question bank must never be hard-deleted — deactivate instead (active=0). The
        // founding certification (id 1) is permanent.
        app.MapGet("/api/admin/certifications", (HttpRequest req) => gate(req, "exams", _ =>
            J(new { rows = db.Query(@"SELECT c.*,
                    (SELECT COUNT(*) FROM sample_questions q WHERE COALESCE(q.certification_id,1)=c.id AND q.is_practice=0) bank_size,
                    (SELECT COUNT(*) FROM exam_entitlements e WHERE COALESCE(e.certification_id,1)=c.id) entitlements,
                    (SELECT COUNT(*) FROM issued_credentials ic WHERE COALESCE(ic.certification_id,1)=c.id) credentials
                FROM certifications c ORDER BY c.sort_order, c.id") })));
        app.MapPost("/api/admin/certifications", (HttpRequest req) => gate(req, "exams", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var codeV = (H.GetS(b, "code") ?? "").Trim().ToUpperInvariant();
            var nameV = (H.GetS(b, "name") ?? "").Trim();
            if (codeV.Length is < 2 or > 20 || !System.Text.RegularExpressions.Regex.IsMatch(codeV, @"^[A-Z0-9][A-Z0-9\-]*$"))
                return Results.Json(new { error = "bad_code", message = "Code must be 2–20 characters: letters, digits and hyphens (e.g. PCP-COST)." }, statusCode: 400);
            if (nameV.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
            if (Certs.ByCode(db, codeV) is not null) return Results.Json(new { error = "code_exists" }, statusCode: 409);
            // PCI-HON is the honorary-award number space: a certification prefix colliding with it
            // would make earned credentials unverifiable (/api/verify routes that prefix to the
            // honorary registry first). Reserved outright.
            var prefixV = (H.GetS(b, "credential_prefix") ?? "").Trim().ToUpperInvariant();
            if (codeV.StartsWith(Honorary.AwardPrefix, StringComparison.Ordinal) || prefixV.StartsWith(Honorary.AwardPrefix, StringComparison.Ordinal))
                return Results.Json(new { error = "prefix_reserved", message = "PCI-HON is reserved for honorary awards and cannot be used for a certification." }, statusCode: 400);
            var id = db.ExecuteReturningId(@"INSERT INTO certifications(code,name,description,credential_prefix,pass_mark_pct,duration_minutes,expiry_years,exam_price,active,sort_order)
                VALUES(?,?,?,?,?,?,?,?,?,?)",
                codeV, nameV, H.GetS(b, "description"), H.GetS(b, "credential_prefix"),
                H.GetNum(b, "pass_mark_pct"), H.GetNum(b, "duration_minutes"),
                H.GetNum(b, "expiry_years") ?? 3, H.GetNum(b, "exam_price"),
                b.TryGetValue("active", out var av) && !JsonFlag(av) ? 0 : 1,
                H.GetNum(b, "sort_order") ?? 0);
            log(adm.Id, "certification_create", $"{codeV} (id {id})");
            CertCatalogue.Bump(); PriceTags.Bump();   // the new credential is live on the public site immediately
            return J(new { ok = true, id });
        }));
        app.MapPatch("/api/admin/certifications/{id}", (HttpRequest req, long id) => gate(req, "exams", adm =>
        {
            if (Certs.ById(db, id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(req).GetAwaiter().GetResult();
            var allowed = new[]{ "name","description","credential_prefix","pass_mark_pct","duration_minutes","expiry_years","exam_price","active","sort_order" };
            var set = allowed.Where(c => b.ContainsKey(c)).ToList();
            if (id == 1 && set.Contains("active") && !JsonFlag(b["active"]))
                return Results.Json(new { error = "founding_cert_permanent", message = "The founding certification cannot be deactivated." }, statusCode: 400);
            if (set.Contains("credential_prefix") && (b["credential_prefix"].ValueKind == JsonValueKind.String
                && (b["credential_prefix"].GetString() ?? "").Trim().ToUpperInvariant().StartsWith(Honorary.AwardPrefix, StringComparison.Ordinal)))
                return Results.Json(new { error = "prefix_reserved", message = "PCI-HON is reserved for honorary awards and cannot be used for a certification." }, statusCode: 400);
            if (set.Count == 0) return J(new { ok = true });
            var vals = set.Select(c => { var v = b[c]; if (c == "active") return (object?)(JsonFlag(v) ? 1 : 0); return (object?)(v.ValueKind == JsonValueKind.String ? v.GetString() : v.ValueKind == JsonValueKind.True ? 1 : v.ValueKind == JsonValueKind.False ? 0 : v.ValueKind == JsonValueKind.Null ? null : v.ToString()); }).Append((object?)id).ToArray();
            db.Execute($"UPDATE certifications SET {string.Join(",", set.Select(c => c + "=?"))}, updated_at=datetime('now') WHERE id=?", vals);
            log(adm.Id, "certification_update", id.ToString());
            CertCatalogue.Bump(); PriceTags.Bump();
            return J(new { ok = true });
        }));
        app.MapDelete("/api/admin/certifications/{id}", (HttpRequest req, long id) => gate(req, "exams", adm =>
        {
            if (id == 1) return Results.Json(new { error = "founding_cert_permanent" }, statusCode: 400);
            var refs = db.Scalar<long>(@"SELECT (SELECT COUNT(*) FROM issued_credentials WHERE certification_id=?)
                + (SELECT COUNT(*) FROM exam_attempts WHERE certification_id=?)
                + (SELECT COUNT(*) FROM exam_entitlements WHERE certification_id=?)
                + (SELECT COUNT(*) FROM sample_questions WHERE certification_id=?)", id, id, id, id);
            if (refs > 0) return Results.Json(new { error = "in_use", message = "This certification has history (credentials, attempts, entitlements or a question bank). Deactivate it instead of deleting." }, statusCode: 409);
            db.Execute("DELETE FROM certifications WHERE id=?", id);
            log(adm.Id, "certification_delete", id.ToString());
            CertCatalogue.Bump(); PriceTags.Bump();
            return J(new { ok = true });
        }));
        Crud("governance_roles", new[]{ "role","holder","status","remit","sort_order" }, "sort_order, id", "governance");
        Crud("resources", new[]{ "title","category","doc_type","url","description","published","sort_order" }, "sort_order, id", "resources");
        Crud("news", new[]{ "title","body","published_date","url","published","sort_order" }, "published_date DESC, id DESC", "news");
        Crud("nav_items", new[]{ "label","url","nav_group","sort_order","visible" }, "nav_group, sort_order, id", "nav");
        Crud("media_assets", new[]{ "filename","alt","width","height","usage" }, "id DESC", "media");
        // Base pricing (per product type). A certification's own exam_price still overrides the exam
        // rule for that credential. Edits flow everywhere: checkout, catalogue cards and every price
        // mentioned in page copy (data-price tokens).
        Crud("pricing_rules", new[]{ "currency","product_type","standard_price","default_discount_percentage","active" }, "id", "settings");

        // ---------- overview ----------
        app.MapGet("/api/admin/overview", (HttpRequest req) =>
        {
            var g = Bare(req); if (g is not null) return g;
            long n(string sql) => db.Scalar<long>(sql);
            double s(string sql) => db.Scalar<double>(sql);
            var k = new {
                members = n("SELECT COUNT(*) FROM users"),
                activeMembers = n("SELECT COUNT(*) FROM users WHERE status='active'"),
                pendingMembers = n("SELECT COUNT(*) FROM users WHERE status='pending'"),
                inProgress = n("SELECT COUNT(*) FROM enrollment_sessions WHERE session_status='in_progress'"),
                paidSessions = n("SELECT COUNT(*) FROM enrollment_sessions WHERE session_status='paid'"),
                abandoned = n("SELECT COUNT(*) FROM enrollment_sessions WHERE session_status='in_progress' AND last_activity_at <= datetime('now','-72 hours')"),
                payCount = n("SELECT COUNT(*) FROM payments WHERE payment_status='paid'"),
                revenue = s("SELECT COALESCE(SUM(final_amount),0) FROM payments WHERE payment_status='paid'"),
                revenueMonth = s("SELECT COALESCE(SUM(final_amount),0) FROM payments WHERE payment_status='paid' AND payment_date >= date('now','start of month')"),
                refunded = n("SELECT COUNT(*) FROM payments WHERE payment_status='refunded'"),
                failedPay = n("SELECT COUNT(*) FROM payments WHERE payment_status='failed'"),
                credActive = n("SELECT COUNT(*) FROM issued_credentials WHERE status='active'"),
                credTotal = n("SELECT COUNT(*) FROM issued_credentials"),
                openInq = n("SELECT COUNT(*) FROM inquiries WHERE status!='closed'"),
                examsDue = n("SELECT COUNT(*) FROM payments WHERE product_type IN ('exam','bundle') AND payment_status='paid' AND exam_schedule_deadline >= datetime('now')"),
            };
            var revSeries = db.Query("SELECT strftime('%Y-%m', payment_date) ym, COALESCE(SUM(final_amount),0) amount, COUNT(*) n FROM payments WHERE payment_status='paid' AND payment_date IS NOT NULL GROUP BY ym ORDER BY ym DESC LIMIT 12");
            revSeries.Reverse();
            var productMix = db.Query("SELECT product_type, COUNT(*) n, COALESCE(SUM(final_amount),0) amount FROM payments WHERE payment_status='paid' GROUP BY product_type");
            var funnel = new { started = n("SELECT COUNT(*) FROM enrollment_sessions"), in_progress = k.inProgress, paid = k.paidSessions };
            var recent = db.Query("SELECT created_at ts, action, details FROM audit_logs ORDER BY id DESC LIMIT 12");
            return J(new { kpis = k, revSeries, productMix, funnel, recent });
        });

        // ---------- abandoned + resend ----------
        app.MapGet("/api/admin/abandoned", (HttpRequest req) => gate(req, "enrollments", _ => J(db.Query(
            "SELECT id,email,current_step,selected_product,last_activity_at,(resume_token_hash IS NOT NULL) AS resume_link_issued FROM enrollment_sessions WHERE session_status='in_progress' ORDER BY last_activity_at DESC"))));
        app.MapPost("/api/admin/resend-resume", async (HttpRequest req) =>
        {
            var g = Deny(req, "enrollments"); if (g is not null) return g;
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant();
            var sN = db.QueryOne("SELECT * FROM enrollment_sessions WHERE email=? ORDER BY id DESC LIMIT 1", email);
            if (sN is null) return Results.Json(new { error = "no_session" }, statusCode: 404);
            var token = Security.RandomHex(32);
            db.Execute("UPDATE enrollment_sessions SET resume_token_hash=?, resume_token_expiry=datetime('now','+7 day') WHERE id=?", Security.Sha(token), sN["id"]);
            return J(new { ok = true });
        });
        app.MapPost("/api/admin/resend-welcome", async (HttpRequest req) =>
        {
            var g = Deny(req, "members"); if (g is not null) return g;
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant();
            var u = db.QueryOne("SELECT * FROM users WHERE email=?", email);
            if (u is null) return Results.Json(new { error = "no_user" }, statusCode: 404);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", u["id"], Security.Sha(token));
            return J(new { ok = true });
        });

        // ---------- codes ----------
        app.MapGet("/api/admin/codes", (HttpRequest req) => gate(req, "codes", _ => J(db.Query("SELECT * FROM discount_codes ORDER BY id DESC"))));
        // Founding fields ride on discount_codes (route/grants/window/caps/auto-approve/criteria) —
        // one codes table, one admin surface, audited like every code change.
        app.MapPost("/api/admin/codes", async (HttpContext ctx) =>
        {
            var req = ctx.Request;
            var g = Deny(req, "codes"); if (g is not null) return g;
            var b = await H.Body(req);
            // Three-route model: ONE founding route. Legacy tier names are accepted and normalised
            // forward so older clients keep working, but every founding code behaves identically:
            // membership + study + exam granted together (grants stay editable per code).
            var route = H.GetS(b, "founding_route");
            if (route is "founding_member" or "founding_candidate") route = "founding";
            if (route is not (null or "" or "founding"))
                return Results.Json(new { error = "bad_founding_route" }, statusCode: 400);
            bool founding = route == "founding";
            int B(string k, bool dflt = false) => (b.ContainsKey(k) ? H.B(b[k].GetRawText()) : dflt) ? 1 : 0;
            var id = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,start_date,end_date,max_uses,single_use_per_email,active,
                founding_route,grants_membership,grants_exam,grants_study_access,requires_application,auto_approve,membership_months,criteria_json)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                (H.GetS(b, "code") ?? "").ToUpperInvariant(), H.GetS(b, "discount_type"), H.GetNum(b, "discount_value"), H.GetS(b, "applies_to") ?? "all",
                H.GetS(b, "start_date"), H.GetS(b, "end_date"), H.GetNum(b, "max_uses"), B("single_use_per_email") , B("active"),
                founding ? route : null,
                founding ? B("grants_membership", true) : 0,
                founding ? B("grants_exam", true) : 0,
                founding ? B("grants_study_access", true) : 0,
                founding ? B("requires_application") : 0,
                founding ? B("auto_approve", true) : 1,
                (int)(H.GetNum(b, "membership_months") ?? 12),
                founding ? H.GetS(b, "criteria_json") : null);
            var adm = adminFromReq(req);
            log(null, "code_created", $"{(H.GetS(b, "code") ?? "").ToUpperInvariant()}{(founding ? " founding:" + route : "")} by admin {adm?.Id}");
            return J(new { id });
        });
        app.MapPatch("/api/admin/codes/{id}", async (HttpContext ctx, long id) =>
        {
            var req = ctx.Request;
            var g = Deny(req, "codes"); if (g is not null) return g;
            var b = await H.Body(req);
            // Only a key PRESENT in the body is updated, so an explicit null CLEARS the column
            // (blanking "Max total uses" → unlimited, clearing an end_date → open-ended window,
            // clearing criteria_json → drop thresholds). A COALESCE-everything UPDATE silently
            // ignored nulls, making those un-set affordances a no-op.
            var set = new List<string>(); var vals = new List<object?>();
            void Str(string col) { if (b.ContainsKey(col)) { set.Add(col + "=?"); vals.Add(H.GetS(b, col)); } }
            void Num(string col) { if (b.ContainsKey(col)) { set.Add(col + "=?"); vals.Add(H.GetNum(b, col)); } }
            void Flag(string col) { if (b.ContainsKey(col)) { set.Add(col + "=?"); vals.Add(H.B(b[col].GetRawText()) ? 1 : 0); } }
            Flag("active"); Str("start_date"); Str("end_date"); Num("max_uses");
            Flag("auto_approve"); Num("membership_months"); Str("criteria_json");
            Flag("grants_membership"); Flag("grants_exam"); Flag("grants_study_access"); Flag("requires_application");
            if (set.Count > 0)
            {
                vals.Add(id);
                db.Execute($"UPDATE discount_codes SET {string.Join(",", set)} WHERE id=?", vals.ToArray());
            }
            var adm = adminFromReq(req);
            log(null, "code_updated", $"code {id} by admin {adm?.Id}: {string.Join(",", b.Keys)}");
            return J(new { ok = true });
        });

        // ---------- pricing ----------
        app.MapGet("/api/admin/pricing", (HttpRequest req) => gate(req, "pricing", _ => J(db.Query("SELECT * FROM pricing_rules ORDER BY id"))));
        app.MapPatch("/api/admin/pricing/{id}", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "pricing"); if (g is not null) return g;
            var b = await H.Body(req);
            object? act = b.ContainsKey("active") ? (H.B(b["active"].GetRawText()) ? 1 : 0) : null;
            db.Execute("UPDATE pricing_rules SET standard_price=COALESCE(?,standard_price), default_discount_percentage=COALESCE(?,default_discount_percentage), active=COALESCE(?,active), updated_at=datetime('now') WHERE id=?",
                H.GetNum(b, "standard_price"), H.GetNum(b, "default_discount_percentage"), act, id);
            CertCatalogue.Bump();   // exam pricing feeds the public catalogue cards
            return J(new { ok = true });
        });
        app.MapPost("/api/admin/run-reminders", (HttpRequest req) => gate(req, "enrollments", _ => J(new { ok = true, reminders_sent = 0 })));

        // ---------- enrollments + payments ----------
        app.MapGet("/api/admin/enrollments", (HttpRequest req) => gate(req, "enrollments", _ =>
        {
            var status = req.Query["status"].ToString(); var q = req.Query["q"].ToString();
            long.TryParse(req.Query["limit"], out var limit); if (limit == 0) limit = 200;
            long.TryParse(req.Query["offset"], out var offset);
            var where = new List<string>(); var args = new List<object?>();
            // "abandoned" is a computed condition (never a stored session_status), mirroring the overview
            // KPI: an in-progress session idle for 72h+. Other statuses match the column literally.
            if (status == "abandoned") where.Add("session_status='in_progress' AND last_activity_at <= datetime('now','-72 hours')");
            else if (!string.IsNullOrEmpty(status)) { where.Add("session_status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(q)) { where.Add("email LIKE ?"); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            var rows = db.Query($"SELECT id,email,current_step,session_status,selected_product,selected_membership,last_activity_at,created_at,reminders_sent,last_reminder_at,(resume_token_hash IS NOT NULL) resume_link_issued FROM enrollment_sessions {w} ORDER BY last_activity_at DESC LIMIT ? OFFSET ?", args.Append((object?)limit).Append((object?)offset).ToArray());
            var total = db.Scalar<long>($"SELECT COUNT(*) FROM enrollment_sessions {w}", args.ToArray());
            return J(new { rows, total });
        }));
        app.MapPost("/api/admin/enrollments/{id}/remind", (HttpRequest req, long id) => gate(req, "enrollments", _ => J(new { ok = true })));

        app.MapGet("/api/admin/payments", (HttpRequest req) => gate(req, "payments", _ =>
        {
            var status = req.Query["status"].ToString(); var product = req.Query["product"].ToString(); var q = req.Query["q"].ToString();
            long.TryParse(req.Query["limit"], out var limit); if (limit == 0) limit = 200;
            long.TryParse(req.Query["offset"], out var offset);
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("p.payment_status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(product)) { where.Add("p.product_type=?"); args.Add(product); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(p.reference LIKE ? OR u.email LIKE ?)"); args.Add(Like(q)); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            var rows = db.Query($"SELECT p.*, u.email,u.first_name,u.last_name FROM payments p LEFT JOIN users u ON u.id=p.user_id {w} ORDER BY p.id DESC LIMIT ? OFFSET ?", args.Append((object?)limit).Append((object?)offset).ToArray());
            var totals = db.QueryOne($"SELECT COALESCE(SUM(CASE WHEN p.payment_status='paid' THEN final_amount END),0) paid, COUNT(*) n, COALESCE(SUM(CASE WHEN p.payment_status='refunded' THEN final_amount END),0) refunded FROM payments p LEFT JOIN users u ON u.id=p.user_id {w}", args.ToArray());
            return J(new { rows, totals });
        }));
        app.MapGet("/api/admin/payments/{id}", (HttpRequest req, long id) => gate(req, "payments", _ =>
        {
            var p = db.QueryOne("SELECT p.*, u.email,u.first_name,u.last_name FROM payments p LEFT JOIN users u ON u.id=p.user_id WHERE p.id=?", id);
            return p is null ? Results.Json(new { error = "not_found" }, statusCode: 404) : J(p);
        }));

        // ---------- exams ----------
        app.MapGet("/api/admin/exams", (HttpRequest req) => gate(req, "exams", _ => J(new { rows = db.Query(
            "SELECT p.id,p.reference,p.product_type,p.payment_date,p.exam_schedule_deadline,u.email,u.first_name,u.last_name, CAST(julianday(p.exam_schedule_deadline)-julianday('now') AS INT) days_left FROM payments p LEFT JOIN users u ON u.id=p.user_id WHERE p.product_type IN ('exam','bundle') AND p.payment_status='paid' ORDER BY p.exam_schedule_deadline ASC") })));

        // ---------- credentials ----------
        app.MapGet("/api/admin/credentials", (HttpRequest req) => gate(req, "credentials", _ =>
        {
            var status = req.Query["status"].ToString(); var q = req.Query["q"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(credential_id LIKE ? OR holder_name LIKE ?)"); args.Add(Like(q)); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($"SELECT * FROM issued_credentials {w} ORDER BY id DESC", args.ToArray()) });
        }));
        app.MapPost("/api/admin/credentials", async (HttpRequest req) =>
        {
            var g = Deny(req, "credentials"); if (g is not null) return g;
            var b = await H.Body(req);
            var cid = H.GetS(b, "credential_id"); var holder = H.GetS(b, "holder_name");
            if (string.IsNullOrEmpty(cid) || string.IsNullOrEmpty(holder)) return Results.Json(new { error = "missing_fields" }, statusCode: 400);
            // PCI-HON is the honorary registry's number space; /api/verify routes that prefix there
            // first, so a credential issued under it would be unverifiable. Reserved (same rule as the
            // certifications create/update path).
            if (cid.Trim().ToUpperInvariant().StartsWith(Honorary.AwardPrefix, StringComparison.Ordinal))
                return Results.Json(new { error = "prefix_reserved", message = "PCI-HON is reserved for honorary awards." }, statusCode: 400);
            // Attribute the credential to the chosen certification (defaults to the founding cert). The
            // credential label defaults to that certification's prefix so /api/verify shows the right name.
            var credCertId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification"));
            var credCert = Certs.ById(db, credCertId);
            var credLabel = H.GetS(b, "credential") ?? (credCert is not null ? Certs.Prefix(credCert) : "PCP-AI");
            try { var id = db.ExecuteReturningId("INSERT INTO issued_credentials(credential_id,user_id,certification_id,holder_name,credential,status,expires_at) VALUES(?,?,?,?,?, 'active',?)",
                cid.ToUpperInvariant(), H.GetNum(b, "user_id"), credCertId, holder, credLabel, H.GetS(b, "expires_at"));
                log(H.Ln(H.GetNum(b, "user_id")), "credential_issued", cid); return J(new { id }); }
            catch { return Results.Json(new { error = "duplicate_or_invalid" }, statusCode: 400); }
        });
        app.MapPost("/api/admin/credentials/{id}/status", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "credentials"); if (g is not null) return g;
            var b = await H.Body(req); var status = H.GetS(b, "status");
            if (status is not ("active" or "expired" or "revoked")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE issued_credentials SET status=? WHERE id=?", status, id);
            log(null, "credential_" + status, id.ToString());
            return J(new { ok = true });
        });

        // ---------- inquiries ----------
        app.MapGet("/api/admin/inquiries", (HttpRequest req) => gate(req, "inquiries", _ =>
        {
            var type = req.Query["type"].ToString(); var status = req.Query["status"].ToString(); var q = req.Query["q"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(type)) { where.Add("type=?"); args.Add(type); }
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(email LIKE ? OR org LIKE ? OR topic LIKE ?)"); args.Add(Like(q)); args.Add(Like(q)); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($"SELECT * FROM inquiries {w} ORDER BY id DESC", args.ToArray()) });
        }));
        app.MapPost("/api/admin/inquiries/{id}/status", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "inquiries"); if (g is not null) return g;
            var b = await H.Body(req); var status = H.GetS(b, "status");
            if (status is not ("new" or "in_progress" or "closed")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE inquiries SET status=? WHERE id=?", status, id);
            log(null, "inquiry_" + status, id.ToString());
            return J(new { ok = true });
        });

        // ---------- emails + audit ----------
        app.MapGet("/api/admin/emails", (HttpRequest req) =>
        {
            var g = Deny(req, "emails"); if (g is not null) return g;
            var type = req.Query["type"].ToString(); var status = req.Query["status"].ToString(); var q = req.Query["q"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(type)) { where.Add("email_type=?"); args.Add(type); }
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(q)) { where.Add("email LIKE ?"); args.Add(Like(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($"SELECT * FROM email_logs {w} ORDER BY id DESC LIMIT 300", args.ToArray()) });
        });
        app.MapGet("/api/admin/audit", (HttpRequest req) => gate(req, "audit", _ => J(new { rows = db.Query("SELECT * FROM audit_logs ORDER BY id DESC LIMIT 300") })));

        // ---------- CSV export ----------
        app.MapGet("/api/admin/export", (HttpRequest req) =>
        {
            var g = Deny(req, "reports"); if (g is not null) return g;
            var map = new Dictionary<string, string> {
                ["members"] = "SELECT id,first_name,last_name,email,status,created_at FROM users ORDER BY id DESC",
                ["payments"] = "SELECT id,reference,user_id,product_type,final_amount,currency,payment_status,payment_date FROM payments ORDER BY id DESC",
                ["enrollments"] = "SELECT id,email,current_step,session_status,selected_product,last_activity_at FROM enrollment_sessions ORDER BY id DESC",
                ["credentials"] = "SELECT credential_id,holder_name,credential,status,issued_at,expires_at FROM issued_credentials ORDER BY id DESC",
                ["inquiries"] = "SELECT id,type,email,org,topic,reference,status,created_at FROM inquiries ORDER BY id DESC",
                ["redemptions"] = "SELECT r.code,r.email,r.product_type,r.discount_amount,p.final_amount,p.reference,r.redeemed_at FROM code_redemptions r LEFT JOIN payments p ON p.id=r.payment_id ORDER BY r.id DESC",
                ["subscribers"] = "SELECT email,status,created_at FROM newsletter_subscribers ORDER BY id DESC" };
            var entity = req.Query["entity"].ToString();
            if (!map.TryGetValue(entity, out var sql)) return Results.Json(new { error = "bad_entity" }, statusCode: 400);
            var rows = db.Query(sql);
            // Bulk export of PII / financial data is a sensitive action — record who exported what and
            // how many rows, so a mass data pull is attributable in the audit trail.
            log(adminFromReq(req)?.Id, "data_export", $"{entity} ({rows.Count} rows)");
            return Results.Text(ToCsv(rows), "text/csv");
        });

        // ---------- pages / content / subscribers / forms ----------
        app.MapGet("/api/admin/pages", (HttpRequest req) => gate(req, "pages", _ => J(new { rows = db.Query("SELECT * FROM pages ORDER BY nav_group, title") })));
        app.MapPatch("/api/admin/pages/{id}", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "pages"); if (g is not null) return g;
            var b = await H.Body(req);
            object? noindex = b.ContainsKey("noindex") ? (H.B(b["noindex"].GetRawText()) ? 1 : 0) : null;
            object? published = b.ContainsKey("published") ? (H.B(b["published"].GetRawText()) ? 1 : 0) : null;
            db.Execute("UPDATE pages SET title=COALESCE(?,title), meta_description=COALESCE(?,meta_description), noindex=COALESCE(?,noindex), published=COALESCE(?,published), updated_at=datetime('now') WHERE id=?",
                H.GetS(b, "title"), H.GetS(b, "meta_description"), noindex, published, id);
            PCI.Backend.Core.PageContent.Bump();
            log(null, "page_update", id.ToString());
            return J(new { ok = true });
        });
        app.MapGet("/api/admin/content", (HttpRequest req) => gate(req, "content", _ => J(new { rows = db.Query("SELECT * FROM site_content ORDER BY cgroup, id") })));
        app.MapPatch("/api/admin/content/{id}", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "content"); if (g is not null) return g;
            var b = await H.Body(req);
            db.Execute("UPDATE site_content SET cvalue=?, updated_at=datetime('now') WHERE id=?", H.GetS(b, "cvalue") ?? "", id);
            PCI.Backend.Core.PageContent.Bump();
            log(null, "content_update", id.ToString());
            return J(new { ok = true });
        });

        // ---- per-page content blocks (fully dynamic content) ----
        app.MapGet("/api/admin/page-blocks", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var slug = req.Query["slug"].ToString();
            var rows = string.IsNullOrEmpty(slug)
                ? db.Query("SELECT * FROM page_blocks ORDER BY slug, sort_order, id")
                : db.Query("SELECT * FROM page_blocks WHERE slug=? ORDER BY sort_order, id", slug);
            return J(new { rows });
        }));
        app.MapPost("/api/admin/page-blocks", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var slug = (H.GetS(b, "slug") ?? "").Trim();
            var key = (H.GetS(b, "block_key") ?? "").Trim();
            if (slug == "" || key == "") return Results.Json(new { error = "slug_and_key_required" }, statusCode: 400);
            // idempotent upsert on (slug, block_key)
            var id = db.ExecuteReturningId(@"INSERT OR IGNORE INTO page_blocks(slug,block_key,label,ctype,cvalue,sort_order) VALUES(?,?,?,?,?,?)",
                slug, key, H.GetS(b, "label"), H.GetS(b, "ctype") ?? "text", H.GetS(b, "cvalue"), (long)(H.GetNum(b, "sort_order") ?? 0));
            db.Execute("UPDATE page_blocks SET label=COALESCE(?,label), cvalue=?, updated_at=datetime('now') WHERE slug=? AND block_key=?",
                H.GetS(b, "label"), H.GetS(b, "cvalue"), slug, key);
            PCI.Backend.Core.PageContent.Bump();
            log(null, "page_block_upsert", $"{slug}:{key}");
            return J(new { ok = true, id });
        }));
        app.MapPatch("/api/admin/page-blocks/{id}", (HttpRequest req, long id) => gate(req, "pages", _ =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            db.Execute("UPDATE page_blocks SET cvalue=?, label=COALESCE(?,label), sort_order=COALESCE(?,sort_order), updated_at=datetime('now') WHERE id=?",
                H.GetS(b, "cvalue") ?? "", H.GetS(b, "label"), b.ContainsKey("sort_order") ? (object)(long)(H.GetNum(b, "sort_order") ?? 0) : null, id);
            PCI.Backend.Core.PageContent.Bump();
            log(null, "page_block_update", id.ToString());
            return J(new { ok = true });
        }));
        app.MapDelete("/api/admin/page-blocks/{id}", (HttpRequest req, long id) => gate(req, "pages", _ =>
        {
            db.Execute("DELETE FROM page_blocks WHERE id=?", id);
            PCI.Backend.Core.PageContent.Bump();
            log(null, "page_block_delete", id.ToString());
            return J(new { ok = true });
        }));
        app.MapGet("/api/admin/subscribers", (HttpRequest req) => gate(req, "subscribers", _ => J(new { rows = db.Query("SELECT * FROM newsletter_subscribers ORDER BY id DESC") })));
        app.MapPatch("/api/admin/subscribers/{id}", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "subscribers"); if (g is not null) return g;
            var b = await H.Body(req);
            var subStatus = H.GetS(b, "status");
            if (subStatus is not ("subscribed" or "unsubscribed" or "bounced" or "complained"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE newsletter_subscribers SET status=? WHERE id=?", subStatus, id);
            return J(new { ok = true });
        });
        app.MapGet("/api/admin/form_submissions", (HttpRequest req) =>
        {
            var g = Deny(req, "submissions"); if (g is not null) return g;
            var type = req.Query["type"].ToString(); var status = req.Query["status"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(type)) { where.Add("form_type=?"); args.Add(type); }
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($"SELECT * FROM form_submissions {w} ORDER BY id DESC", args.ToArray()) });
        });
        app.MapPost("/api/admin/form_submissions/{id}/status", async (HttpRequest req, long id) =>
        {
            var g = Deny(req, "submissions"); if (g is not null) return g;
            var b = await H.Body(req);
            var fsStatus = H.GetS(b, "status");
            if (fsStatus is not ("new" or "in_progress" or "resolved" or "archived" or "spam"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE form_submissions SET status=? WHERE id=?", fsStatus, id);
            return J(new { ok = true });
        });
    }
}
