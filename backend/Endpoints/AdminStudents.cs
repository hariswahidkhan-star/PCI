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
            // Credential hygiene: the password hash never leaves the database — not to dashboards,
            // logs, APIs or exports. Support resets credentials; it never reads them.
            u.Remove("password_hash");
            return J(new { user = u,
                profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", id),
                membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", id),
                payments = db.Query("SELECT * FROM payments WHERE user_id=? ORDER BY id DESC", id),
                credentials = db.Query("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", id),
                identity_documents = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,status,review_note,reviewed_at,created_at FROM identity_documents WHERE user_id=? ORDER BY id DESC", id),
                sessions = db.Query("SELECT id,current_step,session_status,selected_product,last_activity_at,reminders_sent FROM enrollment_sessions WHERE user_id=? OR email=? ORDER BY id DESC", id, u["email"]),
                emails = db.Query("SELECT * FROM email_logs WHERE user_id=? OR email=? ORDER BY id DESC LIMIT 50", id, u["email"]) });
        }));

        // Changing a student's account status is a member-management action; gate it on the 'members'
        // section like every other member endpoint (previously auth-only, letting any admin — even a
        // viewer — deactivate/activate any student).
        app.MapPost("/api/admin/members/{id}/status", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status");
            if (status is not ("pending" or "active" or "deactivated")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var prev = db.Scalar<string>("SELECT status FROM users WHERE id=?", id);
            db.Execute("UPDATE users SET status=?, updated_at=datetime('now') WHERE id=?", status, id);
            if (prev == "deactivated" && status == "active")
                try { db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Account', 'Your account has been unlocked', 'Your PCI account is active again — you can sign in as normal.')", id); } catch { }
            log(adm.Id, "member_status", $"subject {id} → {status}");
            return J(new { ok = true });
        }));

        // Minting a password-setup link for a student is account-takeover-capable; gate on 'members'
        // (previously auth-only — any admin could obtain a set-password link for any account).
        app.MapPost("/api/admin/members/{id}/resend-setup", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var u = db.QueryOne("SELECT * FROM users WHERE id=?", id);
            if (u is null) return Results.Json(new { error = "no_user" }, statusCode: 404);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", id, Security.Sha(token));
            // Deliver the link (previously the token was minted and then silently lost), and also
            // return it to the authenticated admin so onboarding works even before SMTP is set up.
            var baseUrl = Mailer.BaseUrl(ctx.Request);
            var setupUrl = Mailer.SetupLink(baseUrl, token);
            Mailer.SendWelcome(db, id, (string)u["email"]!, H.Str(u["first_name"]), setupUrl, baseUrl);
            log(adm.Id, "resend_setup", $"subject {id}");
            return J(new { ok = true, setup_url = setupUrl });
        }));

        // Manual onboarding: create a student account without a payment (comps, corporate seats,
        // support cases). Deliberately grants NOTHING beyond the account itself — no membership,
        // no exam entitlement, no credential (payment stays the only path to those). Gated on the
        // 'members' section (owner + student_manager). Returns the setup link so the admin can
        // hand it over directly even before SMTP is configured.
        app.MapPost("/api/admin/members", (HttpContext ctx) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM users WHERE email=?", email) is not null)
                return Results.Json(new { error = "email_exists" }, statusCode: 409);
            var first = H.GetS(b, "first_name", "first") ?? "";
            var last = H.GetS(b, "last_name", "last") ?? "";
            var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?, 'student','active')", email, first, last);
            db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", uid, Security.Sha(token));
            var baseUrl = Mailer.BaseUrl(ctx.Request);
            var setupUrl = Mailer.SetupLink(baseUrl, token);
            Mailer.SendWelcome(db, uid, email, first, setupUrl, baseUrl);
            log(adm.Id, "member_created_by_admin", $"user {uid} ({email}) by admin {adm.Id}");
            return J(new { ok = true, id = uid, setup_url = setupUrl });
        }));

        // Creating a referral discount code for a student is a member action; gate on 'members'.
        app.MapPost("/api/admin/members/{id}/referral-code", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var existing = db.QueryOne("SELECT code FROM discount_codes WHERE owner_user_id=? AND code_type='referral' AND active=1", id);
            if (existing is not null) return J(new { code = existing["code"], existing = true });
            var code = "REF-" + Security.RandomHex(4).ToUpperInvariant();
            try { db.Execute("INSERT INTO discount_codes(code,code_type,owner_user_id,discount_type,discount_value,applies_to,active) VALUES(?, 'referral',?, 'percent',10,'all',1)", code, id); } catch { }
            return J(new { code, existing = false });
        }));

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
                identity_documents = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,status,review_note,reviewed_at,created_at FROM identity_documents WHERE user_id=? ORDER BY id DESC", id),
                attempts = db.Query("SELECT id,booking_id,kind,status,percent,result,violations,identity_result,evidence_count,review_status,client_kind,started_at,submitted_at,last_heartbeat_at FROM exam_attempts WHERE user_id=? ORDER BY id DESC", id),
                cpd, cpd_total = cpdTotal,
                tickets = has("tickets") ? db.Query("SELECT * FROM tickets WHERE user_id=? ORDER BY id DESC", id) : new(),
                logins = has("login_events") ? db.Query("SELECT * FROM login_events WHERE user_id=? ORDER BY id DESC LIMIT 20", id) : new(),
                emails = db.Query("SELECT * FROM email_logs WHERE user_id=? OR email=? ORDER BY id DESC LIMIT 50", id, u["email"]) });
        }));

        app.MapPatch("/api/admin/students/{id}/profile", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
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
            log(adm.Id, "admin_edit_profile", "user " + id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/cpd", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
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
            log(adm.Id, "admin_add_cpd", $"user {id} +{hours}h");
            return J(new { ok = true });
        }));

        app.MapDelete("/api/admin/students/{id}/cpd/{cid}", (HttpContext ctx, long id, long cid) => gate(ctx.Request, "members", _ =>
        {
            db.Execute("DELETE FROM cpd_entries WHERE id=? AND user_id=?", cid, id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/membership", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
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
            log(adm.Id, "admin_edit_membership", $"user {id} {status}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/booking", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var when = H.GetS(b, "scheduled_at");
            if (string.IsNullOrEmpty(when)) return Results.Json(new { error = "scheduled_at_required" }, statusCode: 400);
            var tz = H.GetS(b, "timezone") ?? "UTC";
            var open = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            if (open is not null) db.Execute("UPDATE exam_bookings SET scheduled_at=?, timezone=?, updated_at=datetime('now') WHERE id=?", when, tz, open["id"]);
            else db.Execute("INSERT INTO exam_bookings(user_id,scheduled_at,timezone,status) VALUES(?,?,?, 'scheduled')", id, when, tz);
            log(adm.Id, "admin_set_booking", $"user {id} @ {when}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/booking/cancel", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var open = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", id);
            if (open is not null) db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE id=?", open["id"]);
            log(adm.Id, "admin_cancel_booking", "user " + id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/students/{id}/revoke-sessions", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session'", id);
            log(adm.Id, "admin_revoke_sessions", "user " + id);
            return J(new { ok = true });
        }));

        // Admin recovery: clear a student's two-factor (TOTP) enrolment so a candidate who has lost their
        // authenticator can sign in and re-enrol. Also revokes active sessions (any 2FA-bound token dies).
        // Gated by 'members' and audited — attributed to the acting admin.
        app.MapPost("/api/admin/students/{id}/reset-2fa", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var su = db.QueryOne("SELECT id,email,first_name FROM users WHERE id=?", id);
            if (su is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE users SET two_factor_enabled=0, totp_secret=NULL, totp_last_step=NULL WHERE id=?", id);
            db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session'", id);
            log(adm.Id, "admin_reset_student_2fa", "user " + id);
            // Fire through the Communications Centre (email + in-app per the trigger config). This event
            // previously sent nothing, so wiring it here is purely additive — no double-send.
            Comms.Fire(db, "account.totp_disabled", id, H.Str(su["email"]), null,
                new Dictionary<string, string?> { ["student_name"] = H.Str(su["first_name"]) ?? "there", ["portal_link"] = "/app/" },
                "Two-factor authentication was reset on your PCI account",
                "<p>Dear {{student_name}},</p><p>An administrator has reset the two-factor authentication (2FA) on your PCI account. " +
                "You can sign in and re-enrol an authenticator from your profile at any time. If you did not expect this change, " +
                "please contact support immediately.</p><p>— Project Controls Institute</p>",
                dedupSuffix: DateTime.UtcNow.Ticks.ToString());
            return J(new { ok = true, message = "Two-factor authentication has been reset for this student." });
        }));

        // ---- exam fee waiver: grant an exam entitlement without payment (scholarships, comps,
        // corporate seats). Mirrors exactly what the Stripe webhook grants — a paid $0 payment row
        // plus a formal entitlement — so every downstream gate (booking, launch, submit, one-attempt
        // -per-entitlement) applies unchanged. Undo = mark the payment refunded in Payments.
        app.MapPost("/api/admin/students/{id}/exam-waiver", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var cert = Certs.ById(db, certId);
            if (cert is null) return Results.Json(new { error = "bad_certification" }, statusCode: 400);
            // One live entitlement per certification: refuse if an unconsumed paid one already exists.
            var open = db.QueryOne(@"SELECT p.id FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id
                WHERE p.user_id=? AND p.payment_status IN ('paid','waived') AND p.product_type IN ('exam','bundle')
                AND COALESCE(e.certification_id,1)=? AND COALESCE(e.status,'available') IN ('available','booked')", id, certId);
            if (open is not null) return Results.Json(new { error = "already_entitled" }, statusCode: 409);
            var reference = "WAIVE-" + Security.RandomHex(5).ToUpperInvariant();
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 300) note = note[..300];
            // A waiver is recorded as payment_status='waived' — never a fabricated 'paid' transaction.
            var listPrice = Settlement.ListPrice(db, "exam");
            var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference,exam_schedule_deadline,waived_amount,note,recorded_by)
                VALUES(?, 'exam', ?, 0, 'USD', 'admin_waiver', 'waived', datetime('now'), ?, datetime('now','+1 year'),?,?,?)", id, listPrice, reference, listPrice, note.Length > 0 ? note : null, adm.Id);
            db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) VALUES(?,?, 'exam', ?, 'available', datetime('now','+1 year'))", id, payId, certId);
            db.Execute("INSERT INTO fee_waivers(user_id,product_type,certification_id,kind,original_amount,waived_amount,final_amount,reason,note,approved_by,payment_id) VALUES(?, 'exam', ?, 'full', ?, ?, 0, 'exam_fee_waiver', ?, ?, ?)",
                id, certId, listPrice, listPrice, note.Length > 0 ? note : null, adm.Id, payId);
            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Exams', 'Exam fee waived', ?, 'Schedule your exam', '/certifications')",
                id, $"The institute has granted you exam access for {H.Str(cert["name"])} at no charge. You can schedule your sitting from the Certifications page once your eligibility items are complete.");
            log(adm.Id, "exam_fee_waived", $"subject {id} cert {certId} by admin {adm.Id} ref {reference}{(note.Length > 0 ? " note: " + note : "")}");
            return J(new { ok = true, reference, certification_id = certId, payment_id = payId });
        }));

        // ---- identity documents: review + view (the student-side gate lives in Lifecycle) ----
        app.MapPost("/api/admin/students/{id}/identity-document/{docId}/review", (HttpContext ctx, long id, long docId) => gate(ctx.Request, "members", adm =>
        {
            var d = db.QueryOne("SELECT * FROM identity_documents WHERE id=? AND user_id=?", docId, id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status");
            if (status is not ("verified" or "rejected")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 500) note = note[..500];
            db.Execute("UPDATE identity_documents SET status=?, review_note=?, reviewed_by=?, reviewed_at=datetime('now') WHERE id=?", status, note, adm.Id, docId);
            // Tell the student — a rejection re-blocks exam booking until they upload a new document.
            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Identity', ?, ?, 'View', '/certifications')",
                id,
                status == "verified" ? "Identity document verified" : "Identity document rejected",
                status == "verified"
                    ? "Your government-issued ID has been verified. No further action is needed."
                    : $"Your government-issued ID could not be accepted{(note.Length > 0 ? ": " + note : "")}. Please upload a new document to keep exam scheduling available.");
            log(adm.Id, "identity_document_" + status, $"subject {id} doc {docId} by admin {adm.Id}");
            return J(new { ok = true, status });
        }));
        app.MapGet("/api/admin/students/{id}/identity-document/{docId}/file", (HttpContext ctx, long id, long docId) => gate(ctx.Request, "members", _ =>
        {
            var d = db.QueryOne("SELECT storage_ref FROM identity_documents WHERE id=? AND user_id=?", docId, id);
            var got = d is null ? null : Storage.Get(H.Str(d["storage_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return Results.File(got.Value.bytes!, got.Value.mime);
        }));

        // ─────────────────────────── GDPR erasure requests (admin) ───────────────────────────
        // A student's "delete my data" request lands here as a tracked ticket with a 30-day due date.
        app.MapGet("/api/admin/erasure-requests", (HttpContext ctx) => gate(ctx.Request, "members", _ =>
        {
            var status = (ctx.Request.Query["status"].ToString() ?? "").Trim();
            var where = status.Length > 0 ? " WHERE r.status=?" : "";
            var args = status.Length > 0 ? new object?[] { status } : Array.Empty<object?>();
            var rows = db.Query(@"SELECT r.id,r.user_id,r.email,r.reason,r.status,r.requested_at,r.due_at,r.acknowledged_at,
                r.reviewed_at,r.admin_note,r.completed_at, u.first_name,u.last_name,u.status user_status
                FROM erasure_requests r LEFT JOIN users u ON u.id=r.user_id" + where + " ORDER BY (r.status IN ('pending','acknowledged')) DESC, r.id DESC LIMIT 300", args);
            // overdue = still open past its 30-day due date.
            var outRows = rows.Select(r => { var o = new Dictionary<string, object?>(r); o["overdue"] = H.Str(r["status"]) is "pending" or "acknowledged" && H.Str(r["due_at"]) is { } d && H.IsPast(d); return o; }).ToList();
            return J(new { rows = outRows, open = outRows.Count(r => H.Str(r["status"]) is "pending" or "acknowledged") });
        }));

        app.MapPost("/api/admin/erasure-requests/{id}/acknowledge", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var r = db.QueryOne("SELECT * FROM erasure_requests WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) != "pending") return Results.Json(new { error = "bad_state", message = "Only a pending request can be acknowledged." }, statusCode: 400);
            db.Execute("UPDATE erasure_requests SET status='acknowledged', acknowledged_at=datetime('now'), reviewed_by=?, reviewed_at=datetime('now') WHERE id=?", adm.Id, id);
            log(adm.Id, "erasure_acknowledged", $"request {id} by {adm.Id} (subject {H.Ln(r["user_id"])})");
            try { Comms.Fire(db, "privacy.under_review", H.L(r["user_id"]), H.Str(r["email"]), null,
                new Dictionary<string, string?> { ["student_name"] = H.Str(r["email"]) }, "Your data deletion request is under review",
                "<p>Your data deletion request is now under review. We will confirm once it has been actioned.</p>"); } catch { }
            return J(new { ok = true, status = "acknowledged" });
        }));

        app.MapPost("/api/admin/erasure-requests/{id}/reject", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var note = (H.GetS(b, "note", "admin_note") ?? "").Trim();
            if (note.Length < 5) return Results.Json(new { error = "note_required", message = "Give a reason the request is being declined (e.g. a legal retention basis)." }, statusCode: 400);
            var r = db.QueryOne("SELECT * FROM erasure_requests WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) is "completed" or "rejected") return Results.Json(new { error = "bad_state", message = "This request is already closed." }, statusCode: 400);
            db.Execute("UPDATE erasure_requests SET status='rejected', admin_note=?, reviewed_by=?, reviewed_at=datetime('now') WHERE id=?", note, adm.Id, id);
            log(adm.Id, "erasure_rejected", $"request {id} by {adm.Id} (subject {H.Ln(r["user_id"])})");
            return J(new { ok = true, status = "rejected" });
        }));

        // ─────────────────────────── Membership grade upgrades / Fellowship nominations ───────────────────────────
        app.MapGet("/api/admin/membership-upgrades", (HttpContext ctx) => gate(ctx.Request, "members", _ =>
        {
            var status = (ctx.Request.Query["status"].ToString() ?? "").Trim();
            var where = status.Length > 0 ? " WHERE mu.status=?" : "";
            var args = status.Length > 0 ? new object?[] { status } : Array.Empty<object?>();
            var rows = db.Query(@"SELECT mu.id,mu.user_id,mu.from_grade,mu.to_grade,mu.statement,mu.status,mu.admin_note,mu.created_at,mu.decided_at,
                u.email,u.first_name,u.last_name FROM membership_upgrades mu LEFT JOIN users u ON u.id=mu.user_id" + where +
                " ORDER BY (mu.status='pending') DESC, mu.id DESC LIMIT 300", args);
            return J(new { rows, pending = db.Scalar<long>("SELECT COUNT(*) FROM membership_upgrades WHERE status='pending'") });
        }));

        app.MapPost("/api/admin/membership-upgrades/{id}/decide", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var action = H.GetS(b, "action") ?? "";
            if (action is not ("approve" or "reject")) return Results.Json(new { error = "bad_action" }, statusCode: 400);
            var note = H.GetS(b, "note", "admin_note");
            var r = db.QueryOne("SELECT * FROM membership_upgrades WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) != "pending") return Results.Json(new { error = "bad_state", message = "This application is already decided." }, statusCode: 400);
            var uid = H.L(r["user_id"]); var to = H.Str(r["to_grade"]) ?? "";
            if (action == "approve")
            {
                if (!MembershipGrades.SetGrade(db, uid, to)) return Results.Json(new { error = "no_membership", message = "The member has no active membership to grade." }, statusCode: 400);
                db.Execute("UPDATE membership_upgrades SET status='approved', admin_note=?, reviewed_by=?, reviewed_at=datetime('now'), decided_at=datetime('now') WHERE id=?", note, adm.Id, id);
                log(adm.Id, "grade_upgrade_approved", $"{to} (#{id}) by {adm.Id} (subject {uid})");
                var g = MembershipGrades.ByKey(to);
                try { Comms.Fire(db, "membership.activated", uid, H.Str(r["email"]), null,
                    new Dictionary<string, string?> { ["student_name"] = H.Str(r["email"]) }, $"You have been admitted as {g.Label} ({g.PostNominal})",
                    $"<p>Congratulations — you have been admitted to the {g.Label} grade. You may use the post-nominal {g.PostNominal}.</p>"); } catch { }
            }
            else
            {
                db.Execute("UPDATE membership_upgrades SET status='rejected', admin_note=?, reviewed_by=?, reviewed_at=datetime('now'), decided_at=datetime('now') WHERE id=?", note, adm.Id, id);
                log(adm.Id, "grade_upgrade_rejected", $"{to} (#{id}) by {adm.Id} (subject {uid})");
            }
            return J(new { ok = true, status = action == "approve" ? "approved" : "rejected" });
        }));

        // Direct grade set (e.g. an admin/board conferring Fellowship without a self-nomination).
        app.MapPost("/api/admin/students/{id}/grade", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var grade = H.GetS(b, "grade") ?? "";
            if (!MembershipGrades.IsGrade(grade)) return Results.Json(new { error = "bad_grade" }, statusCode: 400);
            if (!MembershipGrades.SetGrade(db, id, grade)) return Results.Json(new { error = "no_membership", message = "This member has no membership to grade." }, statusCode: 400);
            log(adm.Id, "grade_set", $"{grade} by {adm.Id} (subject {id})");
            return J(new { ok = true, grade });
        }));

        // Complete the erasure: ANONYMISE the member (see Core.Erasure). Requires explicit confirmation.
        app.MapPost("/api/admin/erasure-requests/{id}/complete", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            if (H.GetBool(b, "confirm") != true) return Results.Json(new { error = "confirm_required", message = "Set confirm=true to permanently anonymise this member's personal data." }, statusCode: 400);
            var r = db.QueryOne("SELECT * FROM erasure_requests WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) is "completed" or "rejected") return Results.Json(new { error = "bad_state", message = "This request is already closed." }, statusCode: 400);
            var userId = H.L(r["user_id"]);
            var email = H.Str(r["email"]);
            // Notify BEFORE anonymising, while we still have the address on file.
            try { Comms.Fire(db, "privacy.completed", userId, email, null,
                new Dictionary<string, string?> { ["student_name"] = email }, "Your data deletion request is complete",
                "<p>Your personal data has been deleted or anonymised, except records we are legally required to retain. Your account is now closed.</p>"); } catch { }
            object summary;
            try { summary = Erasure.Anonymise(db, userId); }
            catch (Exception e) { Console.Error.WriteLine($"[erasure] request {id} failed: {e.Message}"); return Results.Json(new { error = "erasure_failed", message = "The anonymisation could not be completed. No changes were committed." }, statusCode: 500); }
            db.Execute("UPDATE erasure_requests SET status='completed', completed_at=datetime('now'), reviewed_by=?, reviewed_at=datetime('now') WHERE id=?", adm.Id, id);
            log(adm.Id, "erasure_completed", $"request {id} by {adm.Id} (subject {userId})");
            return J(new { ok = true, status = "completed", summary });
        }));
    }
}
