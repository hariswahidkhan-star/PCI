using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin operator toolkit.
///
/// Finance (gate 'finance' — granted explicitly, never bundled into a job-title role):
///   POST /api/admin/students/{id}/mark-paid        — record an offline payment (bank transfer / PO) with full evidence
///   POST /api/admin/students/{id}/waive            — waive a fee fully (immediate access) or partially (student-locked code)
///   POST /api/admin/payments/{id}/reverse          — reverse a manual settlement (mandatory reason)
///   GET  /api/admin/payments/reconciliation        — every payment with its downstream state + exception reason
///   POST /api/admin/payments/{id}/reprocess        — idempotently re-apply downstream effects (missed callbacks)
///
/// Test users (gate 'test_users'):
///   POST /api/admin/test-users                     — scenario-based test account (never counted in reports)
///   GET  /api/admin/test-users                     — list
///   POST /api/admin/test-users/{id}/reset          — wipe progress, keep the account, re-apply the scenario
///   POST /api/admin/test-users/{id}/delete         — remove account + all its data
///
/// Impersonation (gate 'impersonate'):
///   POST /api/admin/members/{id}/impersonate       — mint a 60-minute view-as-student session (reason required)
///   POST /api/admin/members/{id}/impersonate/end   — revoke all impersonation sessions for the student
///
/// Diagnosis (gate 'members'):
///   GET  /api/admin/members/{id}/journey           — full pipeline: every stage, where they're stuck, what to do
///
/// Certuvo (config gate 'integrations'; per-member actions gate 'members'):
///   GET/POST /api/admin/certuvo                    — configuration (secrets write-only)
///   POST /api/admin/certuvo/{userId}/provision     — (re)provision; body {reactivate:true} to lift suspend/revoke
///   POST /api/admin/certuvo/{userId}/suspend|revoke|resend
///   POST /api/certuvo/webhook                      — inbound (secret-checked): activation / first-login confirmations
///
/// All mutations are audit-logged with the acting admin.
/// </summary>
public static class AdminOps
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult Err(int code, string error, string? message = null) => Results.Json(new { error, message }, statusCode: code);

        // ================= mark as paid (offline settlement) =================
        app.MapPost("/api/admin/students/{id}/mark-paid", (HttpContext ctx, long id) => gate(ctx.Request, "finance", adm =>
        {
            var u = db.QueryOne("SELECT id,email FROM users WHERE id=?", id);
            if (u is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var product = (H.GetS(b, "product") ?? "exam").Trim().ToLowerInvariant();
            if (product is not ("exam" or "membership" or "bundle")) return Err(400, "bad_product", "product must be exam, membership or bundle");
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var amount = Math.Max(0, H.GetNum(b, "amount") ?? 0);                 // 0 = waiver / free
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 300) note = note[..300];
            var reference = (H.GetS(b, "reference") ?? "").Trim();
            if (reference.Length == 0) reference = (amount > 0 ? "MANUAL-" : "COMP-") + Security.RandomHex(5).ToUpperInvariant();
            var provider = amount > 0 ? "admin_manual" : "admin_waiver";
            var gatewayRef = (H.GetS(b, "gateway_reference") ?? "").Trim();

            // Duplicate-payment guards. A same-gateway-reference row is always refused; an already-live
            // entitlement/membership is refused unless the operator explicitly overrides.
            if (gatewayRef.Length > 0 && db.QueryOne("SELECT id FROM payments WHERE gateway_reference=? AND payment_status IN ('paid','waived')", gatewayRef) is { } dup)
                return Results.Json(new { error = "duplicate_reference", message = $"Gateway reference already recorded on payment #{H.L(dup["id"])}." }, statusCode: 409);
            var allowDup = H.GetEl(b, "allow_duplicate") is { ValueKind: JsonValueKind.True };
            if (product is "exam" or "bundle")
            {
                var open = db.QueryOne(@"SELECT p.id FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id
                    WHERE p.user_id=? AND p.payment_status IN ('paid','waived') AND p.product_type IN ('exam','bundle')
                    AND COALESCE(e.certification_id,1)=? AND COALESCE(e.status,'available') IN ('available','booked')", id, certId);
                if (open is not null && product == "exam" && !allowDup) return Err(409, "already_entitled");
            }
            if (product == "membership" && !allowDup
                && db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", id) is not null)
                return Err(409, "membership_already_active");

            // Mismatch warning: the operator sees when the recorded amount differs from the list price.
            var listPrice = Settlement.ListPrice(db, product);
            var mismatch = amount > 0 && Math.Abs(amount - listPrice) > 0.005;

            var meta = new Settlement.Meta
            {
                Method = H.GetS(b, "method"), BankReference = H.GetS(b, "bank_reference"), GatewayReference = gatewayRef.Length > 0 ? gatewayRef : null,
                ReceiptNo = H.GetS(b, "receipt_no"), Note = note.Length > 0 ? note : null, RecordedBy = adm.Id,
                PaidAt = H.GetS(b, "paid_at"), OriginalAmount = listPrice > 0 ? listPrice : null,
            };
            var payId = Settlement.Grant(db, id, H.Str(u["email"]), product, certId, amount, reference, provider, meta);
            if (product is "exam" && certId > 1) Settlement.RetargetEntitlement(db, payId, certId);
            if (amount <= 0)
                db.Execute("INSERT INTO fee_waivers(user_id,product_type,certification_id,kind,original_amount,waived_amount,final_amount,reason,note,approved_by,payment_id) VALUES(?,?,?, 'full', ?, ?, 0, 'complimentary', ?, ?, ?)",
                    id, product, certId, listPrice, listPrice, note.Length > 0 ? note : null, adm.Id, payId);

            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Account', ?, ?, ?, ?)",
                id, amount > 0 ? "Payment recorded" : "Access granted",
                product == "membership" ? "Your PCI membership is active." : "Your exam access has been granted — you can schedule your sitting from Certifications.",
                product == "membership" ? "View membership" : "Schedule your exam", product == "membership" ? "/credentials" : "/certifications");
            try { Notify.Alert(db, "finance", "Manual payment recorded",
                $"<p>{(amount > 0 ? $"Offline payment of ${amount:0.##}" : "Complimentary access")} recorded for member #{id} ({System.Net.WebUtility.HtmlEncode(H.Str(u["email"]) ?? "")}), product {product}, ref {reference}, by admin #{adm.Id}.{(mismatch ? $" <b>Amount differs from the ${listPrice:0.##} list price.</b>" : "")}</p>",
                "payment", payId); } catch { }
            log(id, "admin_mark_paid", $"{product} {amount:0.##} ref {reference} method {meta.Method ?? "-"} by {adm.Id}{(mismatch ? $" MISMATCH list {listPrice:0.##}" : "")}{(note.Length > 0 ? " — " + note : "")}");
            return J(new { ok = true, payment_id = payId, product, amount, reference, free = amount == 0, mismatch, list_price = listPrice });
        }));

        // ================= waive a fee (full or partial) =================
        app.MapPost("/api/admin/students/{id}/waive", (HttpContext ctx, long id) => gate(ctx.Request, "finance", adm =>
        {
            var u = db.QueryOne("SELECT id,email FROM users WHERE id=?", id);
            if (u is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var product = (H.GetS(b, "product") ?? "exam").Trim().ToLowerInvariant();
            if (product is not ("exam" or "membership" or "bundle")) return Err(400, "bad_product");
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length == 0) return Err(400, "reason_required", "Every waiver must record a reason (scholarship, sponsorship, promotion, test, …).");
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 300) note = note[..300];
            var percent = Math.Clamp(H.GetNum(b, "percent") ?? 100, 1, 100);
            var expires = (H.GetS(b, "expires") ?? "").Trim();       // yyyy-MM-dd for partial-waiver codes
            var listPrice = Settlement.ListPrice(db, product);

            if (percent >= 100)
            {
                // Full waiver — the same immediate settlement as mark-paid amount 0.
                var reference = "WAIVE-" + Security.RandomHex(5).ToUpperInvariant();
                var payId = Settlement.Grant(db, id, H.Str(u["email"]), product, certId, 0, reference, "admin_waiver",
                    new Settlement.Meta { Note = note.Length > 0 ? note : reason, RecordedBy = adm.Id, OriginalAmount = listPrice });
                if (product is "exam" && certId > 1) Settlement.RetargetEntitlement(db, payId, certId);
                db.Execute("INSERT INTO fee_waivers(user_id,product_type,certification_id,kind,original_amount,waived_amount,final_amount,reason,note,approved_by,payment_id) VALUES(?,?,?, 'full', ?, ?, 0, ?, ?, ?, ?)",
                    id, product, certId, listPrice, listPrice, reason, note.Length > 0 ? note : null, adm.Id, payId);
                db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Account', 'Your fee has been waived', ?)", id,
                    $"The institute has waived your {product} fee in full ({reason}). Your access is active — no payment is needed.");
                log(id, "fee_waiver_full", $"{product} 100% ({reason}) by {adm.Id} pay {payId}");
                return J(new { ok = true, kind = "full", payment_id = payId, waived_amount = listPrice, payable = 0 });
            }

            // Partial waiver — a single-use discount code locked to this student's email; they pay the
            // remainder through the normal checkout, so the money still flows through the real pipeline.
            var codeStr = "WVR-" + Security.RandomHex(5).ToUpperInvariant();
            var criteria = JsonSerializer.Serialize(new { email = H.Str(u["email"]) });
            var codeId = db.ExecuteReturningId(@"INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,end_date,max_uses,single_use_per_email,active,code_type,notes,per_user_limit,criteria_json)
                VALUES(?, 'percentage', ?, ?, ?, 1, 1, 1, 'waiver', ?, 1, ?)",
                codeStr, percent, product == "bundle" ? "all" : product, expires.Length > 0 ? expires : null, $"Partial waiver: {reason} (admin {adm.Id})", criteria);
            var waivedAmt = Math.Round(listPrice * percent / 100 * 100) / 100;
            db.Execute("INSERT INTO fee_waivers(user_id,product_type,certification_id,kind,original_amount,waived_amount,final_amount,reason,note,approved_by,code_id,expires_at) VALUES(?,?,?, 'partial', ?, ?, ?, ?, ?, ?, ?, ?)",
                id, product, certId, listPrice, waivedAmt, Math.Max(0, listPrice - waivedAmt), reason, note.Length > 0 ? note : null, adm.Id, codeId, expires.Length > 0 ? expires : null);
            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Account', 'A fee reduction has been applied for you', ?, 'Go to billing', '/billing')", id,
                $"The institute has granted you a {percent:0.#}% reduction on your {product} fee ({reason}). Use code {codeStr} at checkout{(expires.Length > 0 ? $" before {expires}" : "")}.");
            log(id, "fee_waiver_partial", $"{product} {percent:0.#}% code {codeStr} ({reason}) by {adm.Id}");
            return J(new { ok = true, kind = "partial", code = codeStr, percent, original = listPrice, waived_amount = waivedAmt, payable = Math.Max(0, listPrice - waivedAmt), expires = expires.Length > 0 ? expires : null });
        }));

        // ================= reverse a manual settlement =================
        app.MapPost("/api/admin/payments/{pid}/reverse", (HttpContext ctx, long pid) => gate(ctx.Request, "finance", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length == 0) return Err(400, "reason_required", "A reversal must record why.");
            var prev = db.QueryOne("SELECT payment_status,user_id FROM payments WHERE id=?", pid);
            var result = Settlement.Reverse(db, pid, reason, adm.Id);
            var doc = JsonSerializer.SerializeToElement(result);
            if (doc.TryGetProperty("ok", out var okEl) && okEl.ValueKind == JsonValueKind.True)
                log(H.L(prev?["user_id"]), "payment_reversed", $"payment {pid} {H.Str(prev?["payment_status"])}→refunded by {adm.Id}: {reason}");
            return J(result);
        }));

        // ================= reconciliation =================
        // Every payment with its downstream state, so "gateway says paid but the student has nothing"
        // is visible on one screen — with a one-click, idempotent reprocess.
        app.MapGet("/api/admin/payments/reconciliation", (HttpContext ctx) => gate(ctx.Request, "finance", _ =>
        {
            var rows = db.Query(@"SELECT p.id,p.user_id,u.email,u.is_test,p.product_type,p.final_amount,p.waived_amount,p.currency,p.payment_provider,p.provider_payment_id,
                       p.gateway_reference,p.payment_status,p.payment_date,p.reference,p.method,p.recorded_by,p.reversal_reason,
                       (SELECT COUNT(*) FROM exam_entitlements e WHERE e.payment_id=p.id) has_entitlement,
                       (SELECT m.status FROM memberships m WHERE m.user_id=p.user_id) membership_status,
                       (SELECT c.status FROM certuvo_accounts c WHERE c.user_id=p.user_id) certuvo_status
                FROM payments p LEFT JOIN users u ON u.id=p.user_id ORDER BY p.id DESC LIMIT 200");
            var certuvoOn = CertuvoLink.Enabled(db);
            var outRows = rows.Select(r =>
            {
                var status = H.Str(r["payment_status"]);
                var product = (H.Str(r["product_type"]) ?? "").ToLowerInvariant();
                string? exception = null;
                if (status is "paid" or "waived")
                {
                    if (H.L(r["user_id"]) == 0) exception = "no_linked_student";
                    else if (product is "exam" or "bundle" && H.L(r["has_entitlement"]) == 0) exception = "entitlement_missing";
                    else if (product is "membership" or "bundle" && H.Str(r["membership_status"]) != "active") exception = "membership_not_active";
                    else if (certuvoOn && product is "membership" or "bundle" && H.Str(r["certuvo_status"]) is not ("active" or "suspended" or "revoked")) exception = "certuvo_not_provisioned";
                }
                return new
                {
                    id = H.L(r["id"]), user_id = H.L(r["user_id"]), email = H.Str(r["email"]), is_test = H.L(r["is_test"]) == 1,
                    product, amount = H.D(r["final_amount"]), waived_amount = r["waived_amount"] is null ? (double?)null : H.D(r["waived_amount"]),
                    provider = H.Str(r["payment_provider"]), gateway_id = H.Str(r["provider_payment_id"]) ?? H.Str(r["gateway_reference"]),
                    status, date = H.Str(r["payment_date"]), reference = H.Str(r["reference"]), method = H.Str(r["method"]),
                    entitlement_ok = H.L(r["has_entitlement"]) > 0, membership_status = H.Str(r["membership_status"]), certuvo_status = H.Str(r["certuvo_status"]),
                    reconciled = exception is null, exception,
                };
            }).ToList();
            return J(new { rows = outRows, exceptions = outRows.Count(r => !r.reconciled) });
        }));

        app.MapPost("/api/admin/payments/{pid}/reprocess", (HttpContext ctx, long pid) => gate(ctx.Request, "finance", adm =>
        {
            var result = Settlement.EnsureDownstream(db, pid);
            log(null, "payment_reprocessed", $"payment {pid} by {adm.Id}");
            return J(result);
        }));

        // ================= test users =================
        // Scenarios pre-configure an account at a chosen point in the journey, so every workflow —
        // including the broken ones — can be exercised repeatedly without paying or touching real data.
        var scenarios = new[] { "ready", "unpaid", "member", "waived", "incomplete_profile", "no_id", "certuvo_failed" };
        void ApplyScenario(long uid, string email, string scenario, long certId)
        {
            var stamp = Security.RandomHex(3).ToUpperInvariant();
            var withConsents = scenario is not "unpaid";
            var withProfile = scenario is not ("incomplete_profile" or "unpaid");
            var withId = scenario is "ready" or "member" or "waived" or "certuvo_failed";
            if (withProfile) db.Execute("INSERT INTO student_profiles(user_id,country,city,mobile) VALUES(?, 'United Kingdom','London','+440000000000')", uid);
            else db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
            if (withConsents)
                foreach (var (type, ver) in Lifecycle.RequiredConsents)
                    db.Execute("INSERT INTO candidate_consents(user_id,consent_type,policy_version,ip_address,user_agent) VALUES(?,?,?, 'test','test-user')", uid, type, ver);
            if (withId)
                db.Execute("INSERT INTO identity_documents(user_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256,status,reviewed_at) VALUES(?, 'passport','test-id.png','image/png',64,'test','test','approved',datetime('now'))", uid);
            switch (scenario)
            {
                case "ready": Settlement.Grant(db, uid, email, "bundle", certId, 0, "TESTUSER-" + stamp, "admin_test_user"); break;
                case "member": case "certuvo_failed": Settlement.Grant(db, uid, email, "membership", certId, 0, "TESTUSER-" + stamp, "admin_test_user"); break;
                case "waived": Settlement.Grant(db, uid, email, "exam", certId, 0, "TESTWAIVE-" + stamp, "admin_waiver"); break;
            }
            if (scenario == "certuvo_failed")
            {
                db.Execute("INSERT OR IGNORE INTO certuvo_accounts(user_id,status) VALUES(?, 'pending')", uid);
                db.Execute("UPDATE certuvo_accounts SET status='error', last_error='Simulated provisioning failure (test scenario)', retry_count=1, next_retry_at=datetime('now','+5 minutes'), updated_at=datetime('now') WHERE user_id=?", uid);
            }
        }

        app.MapPost("/api/admin/test-users", (HttpContext ctx) => gate(ctx.Request, "test_users", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var stamp = Security.RandomHex(4).ToLowerInvariant();
            var email = (H.GetS(b, "email") ?? $"test.{stamp}@pci.test").Trim().ToLowerInvariant();
            if (db.QueryOne("SELECT id FROM users WHERE email=?", email) is not null) return Err(409, "email_taken");
            var password = H.GetS(b, "password") is { Length: >= 6 } pw ? pw : "TestPass!" + stamp;
            var first = H.GetS(b, "first_name") ?? "Test"; var last = H.GetS(b, "last_name") ?? "User";
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var scenario = (H.GetS(b, "scenario") ?? "").Trim().ToLowerInvariant();
            // Back-compat: the original grant flags map onto scenarios.
            if (scenario.Length == 0)
            {
                var withExam = H.GetEl(b, "grant_exam") is not { ValueKind: JsonValueKind.False };
                var withMembership = H.GetEl(b, "grant_membership") is not { ValueKind: JsonValueKind.False };
                scenario = withExam && withMembership ? "ready" : withMembership ? "member" : withExam ? "waived" : "unpaid";
            }
            if (!scenarios.Contains(scenario)) return Err(400, "bad_scenario", "scenario must be one of: " + string.Join(", ", scenarios));

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status,password_hash,is_test) VALUES(?,?,?, 'student','active',?,1)", email, first, last, hash);
            ApplyScenario(uid, email, scenario, certId);
            // Mint a ready student session so the operator can open the portal AS this test user immediately.
            var session = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+30 day'))", uid, Security.Sha(session));
            log(uid, "admin_test_user_create", $"{email} scenario {scenario} by {adm.Id}");
            return J(new { ok = true, id = uid, email, password, token = session, login_url = "/app/", scenario,
                note = "Test account — not a real candidate. Excluded from revenue reports and the public credential register." });
        }));

        app.MapGet("/api/admin/test-users", (HttpContext ctx) => gate(ctx.Request, "test_users", _ =>
            J(new { rows = db.Query("SELECT id,email,first_name,last_name,status,created_at FROM users WHERE is_test=1 ORDER BY id DESC"), scenarios })));

        void WipeUserData(long id, bool keepUser)
        {
            foreach (var t in new[] {
                "exam_score_snapshots", "proctor_events", "proctor_messages", "exam_readiness_checks", "exam_delivery_log", "exam_delivery_orders",
                "exam_attempts", "exam_bookings", "exam_entitlements", "payments", "fee_waivers",
                "candidate_consents", "identity_documents", "certuvo_accounts", "issued_credentials", "memberships", "student_profiles",
                "login_tokens", "login_events", "security_events", "notifications", "reviews", "cpd_entries", "accommodation_requests", "appeals", "support_attachments", "tickets", "practice_attempts", "admin_sessions" })
                try { db.Execute($"DELETE FROM {t} WHERE user_id=?", id); } catch { }
            if (!keepUser) db.Execute("DELETE FROM users WHERE id=? AND is_test=1", id);
        }

        app.MapPost("/api/admin/test-users/{id}/reset", (HttpContext ctx, long id) => gate(ctx.Request, "test_users", adm =>
        {
            var u = db.QueryOne("SELECT id,email FROM users WHERE id=? AND is_test=1", id);
            if (u is null) return Err(404, "not_found", "not a test user");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var scenario = (H.GetS(b, "scenario") ?? "ready").Trim().ToLowerInvariant();
            if (!scenarios.Contains(scenario)) return Err(400, "bad_scenario");
            WipeUserData(id, keepUser: true);
            ApplyScenario(id, H.Str(u["email"]) ?? "", scenario, Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert")));
            var session = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+30 day'))", id, Security.Sha(session));
            log(id, "admin_test_user_reset", $"scenario {scenario} by {adm.Id}");
            return J(new { ok = true, id, scenario, token = session, login_url = "/app/" });
        }));

        app.MapPost("/api/admin/test-users/{id}/delete", (HttpContext ctx, long id) => gate(ctx.Request, "test_users", adm =>
        {
            var u = db.QueryOne("SELECT id FROM users WHERE id=? AND is_test=1", id);
            if (u is null) return Err(404, "not_found", "not a test user");
            WipeUserData(id, keepUser: false);
            log(id, "admin_test_user_delete", $"by {adm.Id}");
            return J(new { ok = true });
        }));

        // ================= impersonation: view as student =================
        app.MapPost("/api/admin/members/{id}/impersonate", (HttpContext ctx, long id) => gate(ctx.Request, "impersonate", adm =>
        {
            var u = db.QueryOne("SELECT id,email,status,is_test FROM users WHERE id=?", id);
            if (u is null) return Err(404, "not_found");
            if (H.Str(u["status"]) != "active") return Err(409, "not_active", "Only active accounts can be viewed as the student.");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length == 0) return Err(400, "reason_required", "State why you need to view this account (e.g. the ticket reference).");
            var token = Security.RandomHex(32);
            var tokenSha = Security.Sha(token);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'impersonation', datetime('now','+1 hours'))", id, tokenSha);
            db.Execute("INSERT INTO impersonation_sessions(token_sha,admin_id,user_id,reason) VALUES(?,?,?,?)", tokenSha, adm.Id, id, reason);
            log(id, "impersonation_started", $"admin {adm.Id} ({adm.Email}) — {reason}");
            return J(new { ok = true, token, login_url = "/app/#t=" + token, expires_minutes = 60,
                note = "The portal shows a permanent support-view banner; payment and password actions are disabled for this session." });
        }));

        app.MapPost("/api/admin/members/{id}/impersonate/end", (HttpContext ctx, long id) => gate(ctx.Request, "impersonate", adm =>
        {
            var n = db.ExecuteWithChanges("DELETE FROM login_tokens WHERE user_id=? AND purpose='impersonation'", id);
            db.Execute("UPDATE impersonation_sessions SET ended_at=datetime('now') WHERE user_id=? AND ended_at IS NULL", id);
            log(id, "impersonation_ended", $"admin {adm.Id} revoked {n} session(s)");
            return J(new { ok = true, revoked = n });
        }));

        // The impersonation ledger: sessions with who/why/when + every page the staff session touched.
        app.MapGet("/api/admin/members/{id}/impersonations", (HttpContext ctx, long id) => gate(ctx.Request, "impersonate", _ =>
        {
            var sessions = db.Query(@"SELECT s.id,s.admin_id,a.email admin_email,s.reason,s.started_at,s.ended_at,s.last_seen_at,
                (SELECT COUNT(*) FROM impersonation_events e WHERE e.session_id=s.id) events
                FROM impersonation_sessions s LEFT JOIN admin_users a ON a.id=s.admin_id WHERE s.user_id=? ORDER BY s.id DESC LIMIT 20", id);
            var latest = sessions.Count > 0 ? db.Query("SELECT method,path,at FROM impersonation_events WHERE session_id=? ORDER BY id DESC LIMIT 100", sessions[0]["id"]) : new();
            return J(new { sessions, latest_session_events = latest });
        }));

        // ================= student journey: where are they / where are they stuck =================
        app.MapGet("/api/admin/members/{id}/journey", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Err(404, "not_found");
            var profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", id);
            var membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", id);
            var membershipPay = db.QueryOne("SELECT id,payment_provider,payment_status,reference FROM payments WHERE user_id=? AND payment_status IN ('paid','waived') AND product_type IN ('membership','bundle') ORDER BY id DESC", id);
            var outstanding = Lifecycle.OutstandingConsents(db, id);
            var idDoc = db.QueryOne("SELECT status,created_at,review_note FROM identity_documents WHERE user_id=? ORDER BY id DESC", id);
            var examPay = db.QueryOne("SELECT * FROM payments WHERE user_id=? AND payment_status IN ('paid','waived') AND product_type IN ('exam','bundle') ORDER BY id DESC", id);
            var booking = db.QueryOne("SELECT id,scheduled_at,timezone,status,certification_id FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            var certId = booking is not null ? H.L(booking["certification_id"]) : 1L;
            var order = db.QueryOne(@"SELECT o.status,o.provider,o.confirmation_code,o.result_status,p.name provider_name
                                      FROM exam_delivery_orders o LEFT JOIN exam_delivery_providers p ON p.id=o.provider_id
                                      WHERE o.user_id=? ORDER BY o.id DESC", id);
            var attempt = db.QueryOne("SELECT id,status,result,result_status,submitted_at FROM exam_attempts WHERE user_id=? AND kind='exam' ORDER BY id DESC", id);
            var cred = db.QueryOne("SELECT credential_id,status,expires_at FROM issued_credentials WHERE user_id=? AND status='active' ORDER BY id DESC", id);
            var bookingBlockers = Lifecycle.BookingBlockers(db, id, examPay, profile);
            var externalMode = ExamDelivery.ModeFor(db, certId);
            var certuvo = db.QueryOne("SELECT status,provisioned_at,activated_at,credentials_sent_at,last_error,retry_count,next_retry_at FROM certuvo_accounts WHERE user_id=?", id);
            var openTickets = db.Scalar<long>("SELECT COUNT(*) FROM tickets WHERE user_id=? AND status IN ('open','awaiting_student')", id);

            var stages = new List<object>();
            string? stuck = null; string? stuckReason = null; string? stuckAction = null;
            void Stage(string key, string label, string status, string detail, string? action = null, object? refs = null)
            {
                stages.Add(new { key, label, status, detail, recommended_action = action, refs });
                if (stuck is null && status is "blocked" or "action_required") { stuck = label; stuckReason = detail; stuckAction = action; }
            }

            Stage("account", "Account", H.Str(u["status"]) == "active" ? "done" : "blocked",
                H.Str(u["status"]) == "active" ? "Active" : $"Account is {H.Str(u["status"])}",
                H.Str(u["status"]) == "active" ? null : "Re-activate the account from the status buttons above, or confirm why it was deactivated.");
            Stage("consents", "Consents", outstanding.Count == 0 ? "done" : "action_required",
                outstanding.Count == 0 ? "All required consents accepted" : $"Outstanding: {string.Join(", ", outstanding)}",
                outstanding.Count == 0 ? null : "The student must accept the outstanding consents on their portal home page.");
            Stage("profile", "Profile", profile is not null && !string.IsNullOrWhiteSpace(H.Str(profile["country"])) ? "done" : "action_required",
                profile is not null && !string.IsNullOrWhiteSpace(H.Str(profile["country"])) ? "Complete" : "Country / key profile fields missing",
                profile is not null && !string.IsNullOrWhiteSpace(H.Str(profile["country"])) ? null : "Ask the student to complete Profile → Personal details (country is required for exam booking).");
            var idStatus = H.Str(idDoc?["status"]);
            Stage("identity", "Government ID", idStatus == "approved" ? "done" : idStatus == "submitted" ? "pending" : "blocked",
                idDoc is null ? "No ID uploaded" : idStatus == "approved" ? "Approved" : idStatus == "submitted" ? "Uploaded — awaiting admin review" : $"ID {idStatus}",
                idStatus == "approved" ? null : idStatus == "submitted" ? "Review the uploaded document below (Identity documents)." : "The student must upload a government ID from their portal (Profile → Identity).");
            Stage("membership", "Membership", membership is not null && H.Str(membership["status"]) == "active" ? "done" : membershipPay is not null ? "action_required" : "pending",
                membership is null ? "No membership yet" : $"{H.Str(membership["status"])}{(membershipPay is not null ? $" ({H.Str(membershipPay["payment_provider"])}, ref {H.Str(membershipPay["reference"])})" : "")}",
                membership is not null && H.Str(membership["status"]) == "active" ? null : membershipPay is not null ? "A membership settlement exists but the membership is not active — use Reprocess on the payment." : "Optional: grant membership via Mark as paid / Waive if this student should have it.",
                membershipPay is not null ? new { payment_id = H.L(membershipPay["id"]) } : null);
            Stage("payment", "Exam fee", examPay is not null ? "done" : "blocked",
                examPay is not null ? (H.Str(examPay["payment_status"]) == "waived" ? $"Waived (ref {H.Str(examPay["reference"])})" : $"Paid ({H.Str(examPay["payment_provider"])}, ref {H.Str(examPay["reference"])})") : "Exam fee unpaid — use Mark paid / Waive to grant access",
                examPay is not null ? null : "Record an offline payment (Mark as paid) or waive the fee to unblock scheduling.",
                examPay is not null ? new { payment_id = H.L(examPay["id"]) } : null);
            Stage("booking", "Exam scheduled", booking is not null ? "done" : examPay is not null && bookingBlockers.Count == 0 ? "action_required" : "pending",
                booking is not null ? $"Scheduled for {H.Str(booking["scheduled_at"])}" : bookingBlockers.Count > 0 ? $"Cannot book yet: {string.Join(", ", bookingBlockers)}" : "Ready to schedule",
                booking is not null ? null : bookingBlockers.Count > 0 ? "Clear the listed blockers — each maps to a stage above." : "The student can now schedule from Certifications; nothing is blocking them.");
            if (externalMode != ExamDelivery.InHouse)
                Stage("delivery", "Vendor delivery", order is null ? "pending" : H.Str(order["status"]) == "completed" ? "done" : "action_required",
                    order is null ? $"Will route to {externalMode} on booking" : $"{H.Str(order["provider_name"]) ?? H.Str(order["provider"])}: {H.Str(order["status"])}{(H.Str(order["confirmation_code"]) is { Length: > 0 } cc ? " · " + cc : "")}",
                    order is null ? null : "Manage the vendor order from Examinations → Exam delivery vendors → Bookings.");
            Stage("exam", "Exam taken", attempt is not null ? (H.Str(attempt["status"]) == "submitted" ? "done" : "action_required") : "pending",
                attempt is null ? "Not attempted yet" : $"Attempt {H.Str(attempt["status"])}{(H.Str(attempt["result"]) is { Length: > 0 } rr ? " · " + rr : "")}",
                attempt is null || H.Str(attempt["status"]) == "submitted" ? null : "An attempt is in progress or held — see Proctoring & sessions.");
            Stage("credential", "Credential", cred is not null ? "done" : "pending",
                cred is not null ? $"{H.Str(cred["credential_id"])} (active)" : "Not yet issued",
                null);
            if (CertuvoLink.Enabled(db))
            {
                var cs = H.Str(certuvo?["status"]);
                Stage("certuvo", "Certuvo practice access",
                    cs == "active" ? "done" : cs is "suspended" or "revoked" ? "blocked" : membership is null || H.Str(membership["status"]) != "active" ? "pending" : cs == "error" ? "action_required" : "pending",
                    certuvo is null ? (membership is not null && H.Str(membership["status"]) == "active" ? "Eligible — not provisioned yet" : "Not eligible until membership is active")
                        : cs == "active" ? $"Provisioned{(H.Str(certuvo["credentials_sent_at"]) is { Length: > 0 } sent ? " · credentials sent " + sent : "")}{(H.Str(certuvo["activated_at"]) is { Length: > 0 } act ? " · first login " + act : "")}"
                        : cs == "error" ? $"Failed (retry {H.L(certuvo["retry_count"])}{(H.Str(certuvo["next_retry_at"]) is { Length: > 0 } nr ? ", next retry " + nr : ", automatic retries exhausted")}) — {H.Str(certuvo["last_error"])}"
                        : cs is "suspended" or "revoked" ? $"Access {cs}" : "Pending provisioning",
                    cs == "error" ? "Use Re-provision (below or in Integrations → Certuvo). If it keeps failing, check the API base/key config." :
                    cs is "suspended" or "revoked" ? "Access was administratively removed; re-provision with reactivate to restore." : null);
            }

            return J(new
            {
                user = new { id = H.L(u["id"]), email = H.Str(u["email"]), name = $"{H.Str(u["first_name"])} {H.Str(u["last_name"])}".Trim(), status = H.Str(u["status"]), is_test = H.L(u["is_test"]) == 1 },
                stages, stuck_at = stuck, stuck_reason = stuckReason, stuck_action = stuckAction,
                membership_status = membership is null ? "none" : H.Str(membership["status"]),
                delivery_mode = externalMode,
                open_tickets = openTickets,
            });
        }));

        // ================= Certuvo external-practice integration =================
        app.MapGet("/api/admin/certuvo", (HttpContext ctx) => gate(ctx.Request, "integrations", _ =>
            J(new
            {
                enabled = Settings.Bool(db, "certuvo_enabled", false),
                api_base = Settings.Str(db, "certuvo_api_base", ""),
                provision_path = Settings.Str(db, "certuvo_provision_path", "/api/accounts"),
                deactivate_path = Settings.Str(db, "certuvo_deactivate_path", ""),
                login_url = Settings.Str(db, "certuvo_login_url", ""),
                auth_header = Settings.Str(db, "certuvo_auth_header", "Authorization"),
                has_api_key = Settings.Str(db, "certuvo_api_key", "").Length > 0,
                has_webhook_secret = Settings.Str(db, "certuvo_webhook_secret", "").Length > 0,
                requires = Settings.Str(db, "certuvo_requires", "membership"),
                retry_max = (int)Settings.Num(db, "certuvo_retry_max", 8),
                accounts = db.Query(@"SELECT u.id user_id,u.email,u.is_test,c.status,c.username,c.external_id,c.provisioned_at,c.activated_at,c.credentials_sent_at,
                    c.last_error,c.retry_count,c.next_retry_at,c.suspended_at,c.revoked_at
                    FROM certuvo_accounts c JOIN users u ON u.id=c.user_id ORDER BY c.id DESC LIMIT 100"),
            })));

        app.MapPost("/api/admin/certuvo", (HttpContext ctx) => gate(ctx.Request, "integrations", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            void Put(string key, string bodyKey) { if (H.GetS(b, bodyKey) is { } v) Settings.Put(db, key, v.Trim()); }
            if (H.GetEl(b, "enabled") is { } en) Settings.Put(db, "certuvo_enabled", (en.ValueKind == JsonValueKind.True || (en.ValueKind == JsonValueKind.String && en.GetString() is "1" or "true")) ? "1" : "0");
            Put("certuvo_api_base", "api_base"); Put("certuvo_provision_path", "provision_path");
            Put("certuvo_deactivate_path", "deactivate_path");
            Put("certuvo_login_url", "login_url"); Put("certuvo_auth_header", "auth_header");
            if (H.GetS(b, "requires") is { } rq && rq is "membership" or "membership_and_enrolment") Settings.Put(db, "certuvo_requires", rq);
            if (H.GetNum(b, "retry_max") is { } rm) Settings.Put(db, "certuvo_retry_max", ((int)Math.Clamp(rm, 0, 50)).ToString());
            if (H.GetS(b, "api_key") is { } k && k.Length > 0) Settings.Put(db, "certuvo_api_key", k);                     // write-only
            if (H.GetS(b, "webhook_secret") is { } ws && ws.Length > 0) Settings.Put(db, "certuvo_webhook_secret", ws);    // write-only
            log(null, "certuvo.config", $"by {adm.Id}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/certuvo/{userId}/provision", (HttpContext ctx, long userId) => gate(ctx.Request, "members", adm =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", userId) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var reactivate = H.GetEl(b, "reactivate") is { ValueKind: JsonValueKind.True };
            CertuvoLink.Provision(db, CertuvoLink.Http, userId, reactivate).GetAwaiter().GetResult();
            var a = db.QueryOne("SELECT status,last_error,retry_count,next_retry_at FROM certuvo_accounts WHERE user_id=?", userId);
            log(userId, "certuvo.provision", $"by {adm.Id} → {H.Str(a?["status"])}");
            return J(new { ok = H.Str(a?["status"]) == "active", status = H.Str(a?["status"]), error = H.Str(a?["last_error"]), retry_count = H.L(a?["retry_count"]), next_retry_at = H.Str(a?["next_retry_at"]) });
        }));

        app.MapPost("/api/admin/certuvo/{userId}/suspend", (HttpContext ctx, long userId) => gate(ctx.Request, "members", adm =>
        {
            var r = CertuvoLink.Deactivate(db, CertuvoLink.Http, userId, revoke: false).GetAwaiter().GetResult();
            log(userId, "certuvo.suspend", $"by {adm.Id}");
            return J(r);
        }));

        app.MapPost("/api/admin/certuvo/{userId}/revoke", (HttpContext ctx, long userId) => gate(ctx.Request, "members", adm =>
        {
            var r = CertuvoLink.Deactivate(db, CertuvoLink.Http, userId, revoke: true).GetAwaiter().GetResult();
            log(userId, "certuvo.revoke", $"by {adm.Id}");
            return J(r);
        }));

        app.MapPost("/api/admin/certuvo/{userId}/resend", (HttpContext ctx, long userId) => gate(ctx.Request, "members", adm =>
        {
            var ok = CertuvoLink.SendAccessInstructions(db, userId);
            log(userId, "certuvo.resend", $"by {adm.Id} → {(ok ? "sent" : "not_active")}");
            return ok ? J(new { ok = true }) : Err(409, "not_active", "Instructions can only be re-sent for an active account.");
        }));

        // Inbound Certuvo webhook — shared-secret checked. Lets Certuvo confirm activation / first login
        // so PCI's tracker shows "Certuvo first login confirmed" without polling.
        app.MapPost("/api/certuvo/webhook", async (HttpContext ctx) =>
        {
            // 404 (not 5xx) while unconfigured: the route does not exist as far as callers are concerned,
            // and the 500-sweep gate treats any 5xx as a defect.
            var secret = Settings.Str(db, "certuvo_webhook_secret", "");
            if (secret.Length == 0) return Results.Json(new { error = "webhook_not_configured" }, statusCode: 404);
            var given = ctx.Request.Headers["X-Certuvo-Secret"].ToString();
            if (!Security.FixedTimeEquals(given, secret)) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var type = H.GetS(b, "type") ?? H.GetS(b, "event") ?? "";
            var extRef = H.GetS(b, "external_ref");
            var extId = H.GetS(b, "external_id") ?? H.GetS(b, "account_id") ?? H.GetS(b, "id");
            var row = extRef is { Length: > 0 } && long.TryParse(extRef, out var uidFromRef)
                ? db.QueryOne("SELECT user_id FROM certuvo_accounts WHERE user_id=?", uidFromRef)
                : extId is { Length: > 0 } ? db.QueryOne("SELECT user_id FROM certuvo_accounts WHERE external_id=?", extId) : null;
            if (row is null) return Results.Json(new { error = "unknown_account" }, statusCode: 404);
            var uid = H.L(row["user_id"]);
            switch (type)
            {
                case "account.activated": case "login.first":
                    db.Execute("UPDATE certuvo_accounts SET activated_at=COALESCE(activated_at, datetime('now')), updated_at=datetime('now') WHERE user_id=?", uid);
                    break;
                case "account.suspended":
                    db.Execute("UPDATE certuvo_accounts SET status='suspended', suspended_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?", uid);
                    break;
                default:
                    return Results.Json(new { ok = true, ignored = type });
            }
            log(uid, "certuvo.webhook", type);
            return Results.Json(new { ok = true });
        });
    }
}
