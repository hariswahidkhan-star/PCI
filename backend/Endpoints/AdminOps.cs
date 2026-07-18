using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin operator toolkit:
///   POST /api/admin/students/{id}/mark-paid       — settle a fee offline (paid/comp/free) so the student can proceed
///   POST /api/admin/test-users                    — one-click fully-unlocked test account (no payment needed)
///   GET  /api/admin/test-users                    — list test accounts
///   POST /api/admin/test-users/{id}/delete        — remove a test account + its data
///   GET  /api/admin/members/{id}/journey          — the student's end-to-end pipeline: where they are / stuck & why
///   GET/POST /api/admin/certuvo                    — configure the Certuvo external practice integration
///   POST /api/admin/certuvo/{userId}/provision     — (re)provision one member's Certuvo account
/// Student-ops routes are gated by 'members'; Certuvo config by 'integrations'. All mutations are audit-logged.
/// </summary>
public static class AdminOps
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        // ---------- mark as paid / waive / free ----------
        app.MapPost("/api/admin/students/{id}/mark-paid", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var u = db.QueryOne("SELECT id,email FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var product = (H.GetS(b, "product") ?? "exam").Trim().ToLowerInvariant();
            if (product is not ("exam" or "membership" or "bundle")) return Results.Json(new { error = "bad_product", message = "product must be exam, membership or bundle" }, statusCode: 400);
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var amount = Math.Max(0, H.GetNum(b, "amount") ?? 0);                 // 0 = waiver / free
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 300) note = note[..300];
            var reference = (H.GetS(b, "reference") ?? "").Trim();
            if (reference.Length == 0) reference = (amount > 0 ? "MANUAL-" : "COMP-") + Security.RandomHex(5).ToUpperInvariant();
            var provider = amount > 0 ? "admin_manual" : "admin_waiver";
            // exam/bundle: refuse a duplicate live exam entitlement for the same certification.
            if (product is "exam" or "bundle")
            {
                var open = db.QueryOne(@"SELECT p.id FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id
                    WHERE p.user_id=? AND p.payment_status='paid' AND p.product_type IN ('exam','bundle')
                    AND COALESCE(e.certification_id,1)=? AND COALESCE(e.status,'available') IN ('available','booked')", id, certId);
                if (open is not null && product == "exam") return Results.Json(new { error = "already_entitled" }, statusCode: 409);
            }
            var payId = Settlement.Grant(db, id, H.Str(u["email"]), product, certId, amount, reference, provider);
            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Account', ?, ?, ?, ?)",
                id, amount > 0 ? "Payment recorded" : "Access granted",
                product == "membership" ? "Your PCI membership is active." : "Your exam access has been granted — you can schedule your sitting from Certifications.",
                product == "membership" ? "View membership" : "Schedule your exam", product == "membership" ? "/credentials" : "/certifications");
            log(id, "admin_mark_paid", $"{product} {amount:0.##} ref {reference} by {adm.Id}{(note.Length > 0 ? " — " + note : "")}");
            return J(new { ok = true, payment_id = payId, product, amount, reference, free = amount == 0 });
        }));

        // ---------- one-click test user ----------
        app.MapPost("/api/admin/test-users", (HttpContext ctx) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var stamp = Security.RandomHex(4).ToLowerInvariant();
            var email = (H.GetS(b, "email") ?? $"test.{stamp}@pci.test").Trim().ToLowerInvariant();
            if (db.QueryOne("SELECT id FROM users WHERE email=?", email) is not null) return Results.Json(new { error = "email_taken" }, statusCode: 409);
            var password = H.GetS(b, "password") is { Length: >= 6 } pw ? pw : "TestPass!" + stamp;
            var first = H.GetS(b, "first_name") ?? "Test"; var last = H.GetS(b, "last_name") ?? "User";
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var withExam = H.GetEl(b, "grant_exam") is not { ValueKind: JsonValueKind.False };       // default true
            var withMembership = H.GetEl(b, "grant_membership") is not { ValueKind: JsonValueKind.False };

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status,password_hash,is_test) VALUES(?,?,?, 'student','active',?,1)", email, first, last, hash);
            db.Execute("INSERT INTO student_profiles(user_id,country,city,mobile) VALUES(?, 'United Kingdom','London','+440000000000')", uid);
            // clear every eligibility gate: consents, an approved government ID
            foreach (var (type, ver) in Lifecycle.RequiredConsents)
                db.Execute("INSERT INTO candidate_consents(user_id,consent_type,policy_version,ip_address,user_agent) VALUES(?,?,?, 'test','test-user')", uid, type, ver);
            db.Execute("INSERT INTO identity_documents(user_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256,status,reviewed_at) VALUES(?, 'passport','test-id.png','image/png',64,'test','test','approved',datetime('now'))", uid);
            var product = withMembership && withExam ? "bundle" : withMembership ? "membership" : withExam ? "exam" : null;
            if (product is not null) Settlement.Grant(db, uid, email, product, certId, 0, "TESTUSER-" + stamp.ToUpperInvariant(), "admin_test_user");
            // Mint a ready student session so the operator can open the portal AS this test user immediately.
            var session = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+30 day'))", uid, Security.Sha(session));
            log(uid, "admin_test_user_create", $"{email} by {adm.Id}");
            return J(new { ok = true, id = uid, email, password, token = session, login_url = "/app/", note = "Fully-unlocked test account: consents accepted, profile complete, ID approved" + (product is not null ? ", " + product + " granted." : ".") });
        }));

        app.MapGet("/api/admin/test-users", (HttpContext ctx) => gate(ctx.Request, "members", _ =>
            J(new { rows = db.Query("SELECT id,email,first_name,last_name,status,created_at FROM users WHERE is_test=1 ORDER BY id DESC") })));

        app.MapPost("/api/admin/test-users/{id}/delete", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var u = db.QueryOne("SELECT id FROM users WHERE id=? AND is_test=1", id);
            if (u is null) return Results.Json(new { error = "not_found", message = "not a test user" }, statusCode: 404);
            // Delete children leaf-first so foreign keys never block the final user delete. Tables that have
            // no user_id column simply error and are skipped (try/catch).
            foreach (var t in new[] {
                "exam_score_snapshots", "proctor_events", "proctor_messages", "exam_readiness_checks", "exam_delivery_log", "exam_delivery_orders",
                "exam_attempts", "exam_bookings", "exam_entitlements", "payments",
                "candidate_consents", "identity_documents", "certuvo_accounts", "issued_credentials", "memberships", "student_profiles",
                "login_tokens", "login_events", "security_events", "notifications", "reviews", "cpd_entries", "accommodation_requests", "appeals", "support_attachments", "tickets", "practice_attempts", "admin_sessions" })
                try { db.Execute($"DELETE FROM {t} WHERE user_id=?", id); } catch { }
            db.Execute("DELETE FROM users WHERE id=? AND is_test=1", id);
            log(id, "admin_test_user_delete", $"by {adm.Id}");
            return J(new { ok = true });
        }));

        // ---------- student journey: where are they / where are they stuck ----------
        app.MapGet("/api/admin/members/{id}/journey", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", id);
            var membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", id);
            var outstanding = Lifecycle.OutstandingConsents(db, id);
            var idDoc = db.QueryOne("SELECT status,created_at,review_note FROM identity_documents WHERE user_id=? ORDER BY id DESC", id);
            var examPay = db.QueryOne("SELECT * FROM payments WHERE user_id=? AND payment_status='paid' AND product_type IN ('exam','bundle') ORDER BY id DESC", id);
            var booking = db.QueryOne("SELECT id,scheduled_at,timezone,status,certification_id FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            var certId = booking is not null ? H.L(booking["certification_id"]) : (examPay is not null ? 1L : 1L);
            var order = db.QueryOne(@"SELECT o.status,o.provider,o.confirmation_code,o.result_status,p.name provider_name
                                      FROM exam_delivery_orders o LEFT JOIN exam_delivery_providers p ON p.id=o.provider_id
                                      WHERE o.user_id=? ORDER BY o.id DESC", id);
            var attempt = db.QueryOne("SELECT id,status,result,result_status,submitted_at FROM exam_attempts WHERE user_id=? AND kind='exam' ORDER BY id DESC", id);
            var cred = db.QueryOne("SELECT credential_id,status,expires_at FROM issued_credentials WHERE user_id=? AND status='active' ORDER BY id DESC", id);
            var bookingBlockers = Lifecycle.BookingBlockers(db, id, examPay, profile);
            var externalMode = ExamDelivery.ModeFor(db, certId);
            var certuvo = db.QueryOne("SELECT status,provisioned_at,last_error FROM certuvo_accounts WHERE user_id=?", id);

            var stages = new List<object>();
            string? stuck = null; string? stuckReason = null;
            void Stage(string key, string label, string status, string detail)
            {
                stages.Add(new { key, label, status, detail });
                if (stuck is null && status is "blocked" or "action_required") { stuck = label; stuckReason = detail; }
            }

            Stage("account", "Account", H.Str(u["status"]) == "active" ? "done" : "blocked",
                H.Str(u["status"]) == "active" ? "Active" : $"Account is {H.Str(u["status"])}");
            Stage("consents", "Consents", outstanding.Count == 0 ? "done" : "action_required",
                outstanding.Count == 0 ? "All required consents accepted" : $"Outstanding: {string.Join(", ", outstanding)}");
            Stage("profile", "Profile", profile is not null && !string.IsNullOrWhiteSpace(H.Str(profile["country"])) ? "done" : "action_required",
                profile is not null && !string.IsNullOrWhiteSpace(H.Str(profile["country"])) ? "Complete" : "Country / key profile fields missing");
            var idStatus = H.Str(idDoc?["status"]);
            // 'submitted' is 'pending' (awaiting admin review) — it does NOT block booking, so it is not a
            // "stuck" state; only a missing or rejected ID blocks the student.
            Stage("identity", "Government ID", idStatus == "approved" ? "done" : idStatus == "submitted" ? "pending" : "blocked",
                idDoc is null ? "No ID uploaded" : idStatus == "approved" ? "Approved" : idStatus == "submitted" ? "Uploaded — awaiting admin review" : $"ID {idStatus}");
            Stage("payment", "Exam fee", examPay is not null ? "done" : "blocked",
                examPay is not null ? $"Paid ({H.Str(examPay["payment_provider"])}, ref {H.Str(examPay["reference"])})" : "Exam fee unpaid — use Mark paid / Waive to grant access");
            Stage("booking", "Exam scheduled", booking is not null ? "done" : examPay is not null && bookingBlockers.Count == 0 ? "action_required" : "pending",
                booking is not null ? $"Scheduled for {H.Str(booking["scheduled_at"])}" : bookingBlockers.Count > 0 ? $"Cannot book yet: {string.Join(", ", bookingBlockers)}" : "Ready to schedule");
            if (externalMode != ExamDelivery.InHouse)
                Stage("delivery", "Vendor delivery", order is null ? "pending" : H.Str(order["status"]) == "completed" ? "done" : "action_required",
                    order is null ? $"Will route to {externalMode} on booking" : $"{H.Str(order["provider_name"]) ?? H.Str(order["provider"])}: {H.Str(order["status"])}{(H.Str(order["confirmation_code"]) is { Length: > 0 } cc ? " · " + cc : "")}");
            Stage("exam", "Exam taken", attempt is not null ? (H.Str(attempt["status"]) == "submitted" ? "done" : "action_required") : "pending",
                attempt is null ? "Not attempted yet" : $"Attempt {H.Str(attempt["status"])}{(H.Str(attempt["result"]) is { Length: > 0 } rr ? " · " + rr : "")}");
            Stage("credential", "Credential", cred is not null ? "done" : "pending",
                cred is not null ? $"{H.Str(cred["credential_id"])} (active)" : "Not yet issued");
            if (CertuvoLink.Enabled(db))
                Stage("certuvo", "Certuvo practice access", certuvo is not null && H.Str(certuvo["status"]) == "active" ? "done" : membership is null ? "pending" : "action_required",
                    certuvo is null ? "Not provisioned" : H.Str(certuvo["status"]) == "active" ? "Provisioned" : $"{H.Str(certuvo["status"])}{(H.Str(certuvo["last_error"]) is { Length: > 0 } e ? " — " + e : "")}");

            return J(new
            {
                user = new { id = H.L(u["id"]), email = H.Str(u["email"]), name = $"{H.Str(u["first_name"])} {H.Str(u["last_name"])}".Trim(), status = H.Str(u["status"]), is_test = H.L(u["is_test"]) == 1 },
                stages, stuck_at = stuck, stuck_reason = stuckReason,
                membership_status = membership is null ? "none" : H.Str(membership["status"]),
                delivery_mode = externalMode,
            });
        }));

        // ---------- Certuvo external-practice integration config ----------
        app.MapGet("/api/admin/certuvo", (HttpContext ctx) => gate(ctx.Request, "integrations", _ =>
            J(new
            {
                enabled = Settings.Bool(db, "certuvo_enabled", false),
                api_base = Settings.Str(db, "certuvo_api_base", ""),
                provision_path = Settings.Str(db, "certuvo_provision_path", "/api/accounts"),
                login_url = Settings.Str(db, "certuvo_login_url", ""),
                auth_header = Settings.Str(db, "certuvo_auth_header", "Authorization"),
                has_api_key = Settings.Str(db, "certuvo_api_key", "").Length > 0,
                accounts = db.Query("SELECT u.id user_id,u.email,c.status,c.username,c.provisioned_at,c.last_error FROM certuvo_accounts c JOIN users u ON u.id=c.user_id ORDER BY c.id DESC LIMIT 100"),
            })));

        app.MapPost("/api/admin/certuvo", (HttpContext ctx) => gate(ctx.Request, "integrations", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            void Put(string key, string bodyKey) { if (H.GetS(b, bodyKey) is { } v) Settings.Put(db, key, v.Trim()); }
            if (H.GetEl(b, "enabled") is { } en) Settings.Put(db, "certuvo_enabled", (en.ValueKind == JsonValueKind.True || (en.ValueKind == JsonValueKind.String && en.GetString() is "1" or "true")) ? "1" : "0");
            Put("certuvo_api_base", "api_base"); Put("certuvo_provision_path", "provision_path");
            Put("certuvo_login_url", "login_url"); Put("certuvo_auth_header", "auth_header");
            if (H.GetS(b, "api_key") is { } k && k.Length > 0) Settings.Put(db, "certuvo_api_key", k);   // write-only
            log(null, "certuvo.config", $"by {adm.Id}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/certuvo/{userId}/provision", (HttpContext ctx, long userId) => gate(ctx.Request, "members", adm =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", userId) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            CertuvoLink.Provision(db, CertuvoLink.Http, userId).GetAwaiter().GetResult();
            var a = db.QueryOne("SELECT status,last_error FROM certuvo_accounts WHERE user_id=?", userId);
            log(userId, "certuvo.provision", $"by {adm.Id} → {H.Str(a?["status"])}");
            return J(new { ok = H.Str(a?["status"]) == "active", status = H.Str(a?["status"]), error = H.Str(a?["last_error"]) });
        }));
    }
}
