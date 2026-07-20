using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Customer-service portal backend + student support tooling.
///
/// Error references (spec: student-facing error visibility):
///   POST /api/errors                                — public/student capture (rate-limited); returns the PCI reference
///   GET  /api/admin/errors[?ref=|user_id=|status=]  — gate 'inbox': search by reference / student / status
///   POST /api/admin/errors/{id}/status              — gate 'inbox': open | investigating | resolved (+note)
///
/// Unified inbox (gate 'inbox'):
///   GET  /api/support/inbox                         — tickets + live chats + website enquiries, one queue
///   GET  /api/support/metrics                       — status counts, overdue vs SLA, first-response/resolution averages, CSAT
///   GET  /api/support/tickets/{id}                  — conversation + internal notes + student context links
///   POST /api/support/tickets/{id}/reply            — public reply (stamps first_response_at, notifies the student)
///   POST /api/support/tickets/{id}/note             — internal note (mentions notify the named admins)
///   POST /api/support/tickets/{id}/assign           — assign/transfer to an admin
///   POST /api/support/tickets/{id}/status           — open|awaiting_student|pending_internal|escalated|resolved|closed|spam
///   POST /api/support/tickets/{id}/priority         — low|normal|high|urgent  (escalation notifies supervisors)
///   POST /api/support/tickets/{id}/tags             — set tags
///   POST /api/support/tickets/{id}/followup         — schedule a follow-up date
///   GET/POST /api/support/templates[...]            — canned replies (manage: gate 'support_admin')
///   GET  /api/support/kb?q=                         — search the approved knowledge base (read-only)
///   POST /api/support/kb/suggest                    — draft a KB suggestion (enabled=0 until approved by content)
///
/// Student side:
///   POST /api/me/tickets/{id}/rate                  — 1-5 satisfaction after resolve/close
///
/// SLA targets are settings: sla_first_response_mins (default 240), sla_resolution_mins (default 2880).
/// </summary>
public static class Support
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult Err(int code, string error, string? message = null) => Results.Json(new { error, message }, statusCode: code);

        // ================= error references =================
        // Public capture: a failing page in either SPA (or the website) reports what the student saw.
        // Rate-limited per IP via the shared fixed-window map in Program (path added there).
        app.MapPost("/api/errors", async (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);      // optional — anonymous errors are allowed
            var b = await H.Body(ctx.Request);
            var reference = ErrorRefs.Capture(db, u?.Id,
                H.GetS(b, "page"), H.GetS(b, "category") ?? "client",
                H.GetS(b, "message") ?? "Something went wrong.",
                H.GetS(b, "detail") ?? "", ctx.Request.Headers.UserAgent.ToString(), H.GetS(b, "app_version"));
            return J(new { ok = true, reference,
                message = $"Please contact support and quote Error Reference: {reference}." });
        });

        app.MapGet("/api/admin/errors", (HttpContext ctx) => gate(ctx.Request, "inbox", _ =>
        {
            var q = ctx.Request.Query;
            var where = new List<string>(); var args = new List<object?>();
            if (q["ref"].ToString() is { Length: > 0 } r) { where.Add("upper(e.reference)=?"); args.Add(r.Trim().ToUpperInvariant()); }
            if (long.TryParse(q["user_id"], out var uid)) { where.Add("e.user_id=?"); args.Add(uid); }
            if (q["status"].ToString() is { Length: > 0 } st) { where.Add("e.status=?"); args.Add(st); }
            var sql = $@"SELECT e.*, u.email FROM error_reports e LEFT JOIN users u ON u.id=e.user_id
                {(where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "")} ORDER BY e.id DESC LIMIT 100";
            return J(new { rows = db.Query(sql, args.ToArray()) });
        }));

        app.MapPost("/api/admin/errors/{id}/status", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = (H.GetS(b, "status") ?? "").Trim();
            if (status is not ("open" or "investigating" or "resolved")) return Err(400, "bad_status");
            db.Execute("UPDATE error_reports SET status=?, resolution_note=COALESCE(?,resolution_note) WHERE id=?", status, H.GetS(b, "note"), id);
            log(adm.Id, "error_report_status", $"{id} → {status} by {adm.Id}");
            return J(new { ok = true });
        }));

        // ================= member security view (support diagnostics) =================
        // Everything support needs to answer "why can't this student log in / what happened":
        // login history, failed attempts, active sessions, notifications — never the password hash.
        app.MapGet("/api/admin/members/{id}/security", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            if (db.QueryOne("SELECT id FROM users WHERE id=?", id) is null) return Err(404, "not_found");
            List<Dictionary<string, object?>> Safe(string sql, params object?[] a) { try { return db.Query(sql, a); } catch { return new(); } }
            return J(new
            {
                logins = Safe("SELECT * FROM login_events WHERE user_id=? ORDER BY id DESC LIMIT 30", id),
                security_events = Safe("SELECT * FROM security_events WHERE user_id=? ORDER BY id DESC LIMIT 30", id),
                active_sessions = db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='session' AND expires_at>datetime('now')", id),
                impersonation_sessions = db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='impersonation' AND expires_at>datetime('now')", id),
                pending_setup_links = db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='set_password' AND expires_at>datetime('now') AND used_at IS NULL", id),
                notifications = db.Query("SELECT id,category,title,read_at,created_at FROM notifications WHERE user_id=? ORDER BY id DESC LIMIT 15", id),
                error_reports = db.Query("SELECT reference,page,category,status,created_at FROM error_reports WHERE user_id=? ORDER BY id DESC LIMIT 15", id),
                open_tickets = db.Query("SELECT id,reference,subject,status,priority,updated_at FROM tickets WHERE user_id=? AND status NOT IN ('closed') ORDER BY id DESC LIMIT 10", id),
            });
        }));

        // ================= unified inbox =================
        app.MapGet("/api/support/inbox", (HttpContext ctx) => gate(ctx.Request, "inbox", _ =>
        {
            var tickets = db.Query(@"SELECT t.id, t.reference, t.subject, t.status, t.priority, t.tags, t.escalated, t.assigned_to,
                       a.email assigned_email, t.user_id, u.email student_email, u.is_test, t.updated_at, t.first_response_at,
                       (SELECT COUNT(*) FROM ticket_messages m WHERE m.ticket_id=t.id) messages
                FROM tickets t LEFT JOIN users u ON u.id=t.user_id LEFT JOIN admin_users a ON a.id=t.assigned_to
                WHERE t.status NOT IN ('closed','spam') ORDER BY CASE t.priority WHEN 'urgent' THEN 0 WHEN 'high' THEN 1 ELSE 2 END, t.updated_at DESC LIMIT 100");
            var chats = db.Query(@"SELECT c.id, c.visitor_name, c.status, c.assigned_to, a.email assigned_email, c.linked_user_id,
                       u.email student_email, c.last_activity_at
                FROM chat_sessions c LEFT JOIN admin_users a ON a.id=c.assigned_to LEFT JOIN users u ON u.id=c.linked_user_id
                WHERE c.status IN ('waiting','live') ORDER BY c.last_activity_at DESC LIMIT 50");
            List<Dictionary<string, object?>> enquiries;
            try { enquiries = db.Query("SELECT id, name, email, subject, status, created_at FROM inquiries WHERE status NOT IN ('closed') ORDER BY id DESC LIMIT 50"); }
            catch { enquiries = new(); }
            return J(new { tickets, chats, enquiries,
                unassigned = tickets.Count(t => t["assigned_to"] is null) + chats.Count(c => c["assigned_to"] is null) });
        }));

        app.MapGet("/api/support/metrics", (HttpContext ctx) => gate(ctx.Request, "inbox", _ =>
        {
            var frMins = (int)Settings.Num(db, "sla_first_response_mins", 240);
            var resMins = (int)Settings.Num(db, "sla_resolution_mins", 2880);
            long N(string sql, params object?[] a) => db.Scalar<long>(sql, a);
            var byStatus = db.Query("SELECT status, COUNT(*) n FROM tickets GROUP BY status");
            // Overdue = open with no first response past the target, or unresolved past the resolution target.
            var overdueFirst = N($"SELECT COUNT(*) FROM tickets WHERE status IN ('open','pending_internal','escalated') AND first_response_at IS NULL AND created_at < datetime('now','-{frMins} minutes')");
            var overdueRes = N($"SELECT COUNT(*) FROM tickets WHERE status NOT IN ('resolved','closed','spam') AND created_at < datetime('now','-{resMins} minutes')");
            // Averages computed in C# from the raw timestamps — identical on SQLite and MySQL.
            double? AvgMins(string col)
            {
                var rows = db.Query($"SELECT created_at, {col} done FROM tickets WHERE {col} IS NOT NULL ORDER BY id DESC LIMIT 500");
                var mins = rows.Select(r => (DateTime.TryParse(H.Str(r["created_at"]), out var c) && DateTime.TryParse(H.Str(r["done"]), out var d))
                        ? (double?)(d - c).TotalMinutes : null)
                    .Where(m => m is >= 0).Select(m => m!.Value).ToList();
                return mins.Count > 0 ? Math.Round(mins.Average(), 1) : null;
            }
            var avgFirst = AvgMins("first_response_at");
            var avgRes = AvgMins("resolved_at");
            var csatRows = db.Query("SELECT rating FROM tickets WHERE rating IS NOT NULL");
            var csat = csatRows.Count > 0 ? (double?)Math.Round(csatRows.Average(r => (double)H.L(r["rating"])), 2) : null;
            var workload = db.Query(@"SELECT a.email, COUNT(*) n FROM tickets t JOIN admin_users a ON a.id=t.assigned_to
                WHERE t.status NOT IN ('resolved','closed','spam') GROUP BY a.email ORDER BY n DESC");
            return J(new
            {
                by_status = byStatus,
                escalated = N("SELECT COUNT(*) FROM tickets WHERE escalated=1 AND status NOT IN ('resolved','closed')"),
                unassigned = N("SELECT COUNT(*) FROM tickets WHERE assigned_to IS NULL AND status NOT IN ('resolved','closed','spam')"),
                waiting_chats = N("SELECT COUNT(*) FROM chat_sessions WHERE status='waiting'"),
                overdue_first_response = overdueFirst, overdue_resolution = overdueRes,
                avg_first_response_mins = avgFirst, avg_resolution_mins = avgRes,
                csat_avg = csat, csat_count = N("SELECT COUNT(*) FROM tickets WHERE rating IS NOT NULL"),
                sla = new { first_response_mins = frMins, resolution_mins = resMins },
                agent_workload = workload,
            });
        }));

        // ================= ticket operations =================
        Dictionary<string, object?>? Ticket(long id) => db.QueryOne("SELECT * FROM tickets WHERE id=?", id);

        app.MapGet("/api/support/tickets/{id}", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", _ =>
        {
            var t = Ticket(id); if (t is null) return Err(404, "not_found");
            var uid = H.L(t["user_id"]);
            return J(new
            {
                ticket = t,
                messages = db.Query("SELECT * FROM ticket_messages WHERE ticket_id=? ORDER BY id", id),
                notes = db.Query("SELECT n.*, a.email admin_email FROM ticket_notes n LEFT JOIN admin_users a ON a.id=n.admin_id WHERE n.ticket_id=? ORDER BY n.id", id),
                student = db.QueryOne("SELECT id,email,first_name,last_name,status,is_test,registration_no FROM users WHERE id=?", uid),
                // context links: what this student's account looks like right now
                membership_status = H.Str(db.QueryOne("SELECT status FROM memberships WHERE user_id=?", uid)?["status"]),
                recent_payments = db.Query("SELECT id,product_type,final_amount,payment_status,payment_date,reference FROM payments WHERE user_id=? ORDER BY id DESC LIMIT 5", uid),
                error_reports = db.Query("SELECT reference,page,status,created_at FROM error_reports WHERE user_id=? ORDER BY id DESC LIMIT 5", uid),
            });
        }));

        app.MapPost("/api/support/tickets/{id}/reply", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            var t = Ticket(id); if (t is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var body = (H.GetS(b, "body") ?? "").Trim();
            if (body.Length == 0) return Err(400, "body_required");
            db.Execute("INSERT INTO ticket_messages(ticket_id,sender,body) VALUES(?, 'admin', ?)", id, body);
            db.Execute("UPDATE tickets SET status='awaiting_student', updated_at=datetime('now'), first_response_at=COALESCE(first_response_at, datetime('now')) WHERE id=?", id);
            var uid = H.L(t["user_id"]);
            try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Support', 'Support replied to your ticket', ?, 'View reply', '/support')",
                uid, $"Ticket {H.Str(t["reference"])}: our team has replied."); } catch { }
            try
            {
                var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", uid)?["email"]);
                if (email is { Length: > 0 }) Notify.Email(db, uid, email, $"PCI support — reply on {H.Str(t["reference"])}",
                    "<p>Our support team has replied to your ticket. Sign in to your portal to read it.</p>", "ticket", id);
            }
            catch { }
            log(uid, "support_reply", $"ticket {id} by {adm.Id}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/support/tickets/{id}/note", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            if (Ticket(id) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var body = (H.GetS(b, "body") ?? "").Trim();
            if (body.Length == 0) return Err(400, "body_required");
            // @mentions: emails of admins to ping. Internal notes are NEVER shown to the student.
            var mentions = (H.GetS(b, "mentions") ?? "").Trim();
            db.Execute("INSERT INTO ticket_notes(ticket_id,admin_id,body,mentions) VALUES(?,?,?,?)", id, adm.Id, body, mentions.Length > 0 ? mentions : null);
            foreach (var m in mentions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                try { Notify.Email(db, null, m, $"You were mentioned on support ticket #{id}",
                    $"<p>{System.Net.WebUtility.HtmlEncode(adm.Email)} mentioned you in an internal note on ticket #{id}.</p>", "ticket", id); } catch { }
            return J(new { ok = true });
        }));

        app.MapPost("/api/support/tickets/{id}/assign", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            if (Ticket(id) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            long? target = H.GetNum(b, "admin_id") is { } aid ? (long)aid : null;
            if (target is { } t2 && db.QueryOne("SELECT id,email FROM admin_users WHERE id=? AND status='active'", t2) is null) return Err(404, "admin_not_found");
            db.Execute("UPDATE tickets SET assigned_to=?, updated_at=datetime('now') WHERE id=?", target, id);
            if (target is { } t3)
                try { Notify.Email(db, null, H.Str(db.QueryOne("SELECT email FROM admin_users WHERE id=?", t3)?["email"]),
                    $"Support ticket #{id} assigned to you", "<p>A support conversation has been assigned to you in the PCI console.</p>", "ticket", id); } catch { }
            log(adm.Id, "support_assign", $"ticket {id} → admin {target?.ToString() ?? "unassigned"} by {adm.Id}");
            return J(new { ok = true });
        }));

        var ticketStatuses = new[] { "open", "awaiting_student", "pending_internal", "escalated", "resolved", "closed", "spam" };
        app.MapPost("/api/support/tickets/{id}/status", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            var t = Ticket(id); if (t is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = (H.GetS(b, "status") ?? "").Trim();
            if (!ticketStatuses.Contains(status)) return Err(400, "bad_status", "status must be one of: " + string.Join(", ", ticketStatuses));
            db.Execute("UPDATE tickets SET status=?, escalated=CASE WHEN ?='escalated' THEN 1 ELSE escalated END, resolved_at=CASE WHEN ? IN ('resolved','closed') THEN COALESCE(resolved_at, datetime('now')) ELSE resolved_at END, updated_at=datetime('now') WHERE id=?",
                status, status, status, id);
            if (status == "escalated")
                try { Notify.Alert(db, "support", $"Support ticket {H.Str(t["reference"])} escalated",
                    $"<p>Ticket #{id} ({System.Net.WebUtility.HtmlEncode(H.Str(t["subject"]) ?? "")}) was escalated by {System.Net.WebUtility.HtmlEncode(adm.Email)}.</p>", "ticket", id); } catch { }
            if (status == "resolved")
                try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Support', 'Your ticket was resolved', 'Let us know how we did — you can rate the conversation from the Support page.', 'Rate it', '/support')", H.L(t["user_id"])); } catch { }
            log(H.L(t["user_id"]), "support_status", $"ticket {id} → {status} by {adm.Id}");
            return J(new { ok = true, status });
        }));

        app.MapPost("/api/support/tickets/{id}/priority", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", adm =>
        {
            if (Ticket(id) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var priority = (H.GetS(b, "priority") ?? "").Trim();
            if (priority is not ("low" or "normal" or "high" or "urgent")) return Err(400, "bad_priority");
            db.Execute("UPDATE tickets SET priority=?, updated_at=datetime('now') WHERE id=?", priority, id);
            log(adm.Id, "support_priority", $"ticket {id} → {priority} by {adm.Id}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/support/tickets/{id}/tags", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", _ =>
        {
            if (Ticket(id) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var tags = (H.GetS(b, "tags") ?? "").Trim(); if (tags.Length > 300) tags = tags[..300];
            db.Execute("UPDATE tickets SET tags=?, updated_at=datetime('now') WHERE id=?", tags.Length > 0 ? tags : null, id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/support/tickets/{id}/followup", (HttpContext ctx, long id) => gate(ctx.Request, "inbox", _ =>
        {
            if (Ticket(id) is null) return Err(404, "not_found");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            db.Execute("UPDATE tickets SET followup_at=? WHERE id=?", H.GetS(b, "at"), id);
            return J(new { ok = true });
        }));

        // ================= canned templates =================
        app.MapGet("/api/support/templates", (HttpContext ctx) => gate(ctx.Request, "inbox", _ =>
            J(new { rows = db.Query("SELECT * FROM support_templates WHERE active=1 ORDER BY category, title") })));

        app.MapPost("/api/support/templates", (HttpContext ctx) => gate(ctx.Request, "support_admin", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var title = (H.GetS(b, "title") ?? "").Trim(); var body = (H.GetS(b, "body") ?? "").Trim();
            if (title.Length == 0 || body.Length == 0) return Err(400, "title_and_body_required");
            var id = db.ExecuteReturningId("INSERT INTO support_templates(title,body,category,created_by) VALUES(?,?,?,?)", title, body, H.GetS(b, "category"), adm.Id);
            return J(new { ok = true, id });
        }));

        app.MapPost("/api/support/templates/{id}/delete", (HttpContext ctx, long id) => gate(ctx.Request, "support_admin", _ =>
        {
            db.Execute("UPDATE support_templates SET active=0 WHERE id=?", id);
            return J(new { ok = true });
        }));

        // ================= knowledge base (agent view) =================
        app.MapGet("/api/support/kb", (HttpContext ctx) => gate(ctx.Request, "inbox", _ =>
        {
            var q = ctx.Request.Query["q"].ToString().Trim();
            var rows = q.Length > 0
                ? db.Query("SELECT id,question,answer FROM chat_kb WHERE enabled=1 AND (question LIKE ? OR answer LIKE ? OR keywords LIKE ?) ORDER BY sort_order LIMIT 20", $"%{q}%", $"%{q}%", $"%{q}%")
                : db.Query("SELECT id,question,answer FROM chat_kb WHERE enabled=1 ORDER BY sort_order LIMIT 20");
            return J(new { rows });
        }));

        // Draft suggestions only — agents never publish directly; a content editor enables the row.
        app.MapPost("/api/support/kb/suggest", (HttpContext ctx) => gate(ctx.Request, "inbox", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var qn = (H.GetS(b, "question") ?? "").Trim(); var an = (H.GetS(b, "answer") ?? "").Trim();
            if (qn.Length == 0 || an.Length == 0) return Err(400, "question_and_answer_required");
            var id = db.ExecuteReturningId("INSERT INTO chat_kb(question,answer,keywords,enabled,sort_order) VALUES(?,?,?,0,999)", qn, an, "suggested by " + adm.Email);
            log(adm.Id, "kb_suggested", $"{id} by {adm.Id}");
            return J(new { ok = true, id, note = "Saved as a draft — a content editor reviews and enables it." });
        }));

        // ================= SLA settings =================
        app.MapPost("/api/support/sla", (HttpContext ctx) => gate(ctx.Request, "support_admin", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            if (H.GetNum(b, "first_response_mins") is { } fr) Settings.Put(db, "sla_first_response_mins", ((int)Math.Clamp(fr, 5, 10080)).ToString());
            if (H.GetNum(b, "resolution_mins") is { } rs) Settings.Put(db, "sla_resolution_mins", ((int)Math.Clamp(rs, 30, 43200)).ToString());
            log(adm.Id, "sla_updated", $"by {adm.Id}");
            return J(new { ok = true });
        }));

        // ================= student CSAT rating =================
        app.MapPost("/api/me/tickets/{id}/rate", (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var t = db.QueryOne("SELECT id,status FROM tickets WHERE id=? AND user_id=?", id, u.Id);
            if (t is null) return Err(404, "not_found");
            if (H.Str(t["status"]) is not ("resolved" or "closed")) return Err(409, "not_resolved", "You can rate a conversation once it has been resolved.");
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var rating = (int)Math.Clamp(H.GetNum(b, "rating") ?? 0, 1, 5);
            db.Execute("UPDATE tickets SET rating=? WHERE id=?", rating, id);
            return J(new { ok = true, rating });
        });
    }
}
