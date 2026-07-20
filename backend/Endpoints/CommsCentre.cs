using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Unified Communications Centre admin API + inbound webhooks. Everything is gated by the 'comms'
/// permission (owner always allowed) and audited. Provider SECRETS are read from env only and never
/// returned — the provider-settings endpoint reports booleans (configured / not) alone.
/// </summary>
public static class CommsCentre
{
    const string SECTION = "comms";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(Dictionary<string, JsonElement> b, params string[] k) => (H.GetS(b, k) ?? "").Trim();
        int I(Dictionary<string, JsonElement> b, string k) => (int)(H.GetNum(b, k) ?? 0);

        // ───────── dashboard + provider settings ─────────
        app.MapGet("/api/admin/comms/overview", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            var byStatus = db.Query("SELECT status, COUNT(*) AS n FROM comm_outbox GROUP BY status");
            return J(new
            {
                outbox = byStatus,
                queued = db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE status IN ('queued','scheduled','retrying','processing')"),
                failed = db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE status='failed'"),
                sent_today = db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE status='sent' AND sent_at>=datetime('now','-1 day')"),
                open_conversations = db.Scalar<long>("SELECT COUNT(*) FROM comm_conversations WHERE status='open'"),
                sender_profiles = db.Scalar<long>("SELECT COUNT(*) FROM comm_sender_profiles"),
                whatsapp_accounts = db.Scalar<long>("SELECT COUNT(*) FROM comm_whatsapp_accounts"),
                templates = db.Scalar<long>("SELECT COUNT(*) FROM comm_templates"),
                triggers = db.Scalar<long>("SELECT COUNT(*) FROM comm_triggers"),
            });
        }));

        // Booleans only — never the secret values.
        app.MapGet("/api/admin/comms/providers", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            bool Set(string k) => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(k));
            return J(new
            {
                email_resend = Set("RESEND_API_KEY"), email_smtp = Set("SMTP_HOST"),
                email_from = Set("MAIL_FROM") || Set("SMTP_FROM"),
                whatsapp_token = Set("WHATSAPP_ACCESS_TOKEN"), whatsapp_verify = Set("WHATSAPP_VERIFY_TOKEN"),
                email_webhook_secret = Set("EMAIL_WEBHOOK_SECRET"),
            });
        }));

        // ───────── sender profiles ─────────
        app.MapGet("/api/admin/comms/senders", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_sender_profiles ORDER BY key") })));
        app.MapPost("/api/admin/comms/senders", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var key = S(b, "key"); if (key.Length == 0) return Results.Json(new { error = "key_required" }, statusCode: 400);
                var existing = db.QueryOne("SELECT id FROM comm_sender_profiles WHERE key=?", key);
                if (existing is null)
                    db.Execute(@"INSERT INTO comm_sender_profiles(key,name,display_name,from_email,reply_to,purpose,category,provider,domain_verified,permitted_roles,is_default,active,approval_status,owner,effective_date,expiry_date)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                        key, S(b, "name"), S(b, "display_name"), S(b, "from_email"), S(b, "reply_to"), S(b, "purpose"),
                        S(b, "category"), S(b, "provider"), I(b, "domain_verified"), S(b, "permitted_roles"),
                        I(b, "is_default"), b.ContainsKey("active") ? I(b, "active") : 1, S(b, "approval_status") is { Length: > 0 } a ? a : "approved",
                        S(b, "owner"), S(b, "effective_date"), S(b, "expiry_date"));
                else
                    db.Execute(@"UPDATE comm_sender_profiles SET name=?,display_name=?,from_email=?,reply_to=?,purpose=?,category=?,provider=?,domain_verified=?,permitted_roles=?,is_default=?,active=?,approval_status=?,owner=?,effective_date=?,expiry_date=?,updated_at=datetime('now') WHERE key=?",
                        S(b, "name"), S(b, "display_name"), S(b, "from_email"), S(b, "reply_to"), S(b, "purpose"), S(b, "category"),
                        S(b, "provider"), I(b, "domain_verified"), S(b, "permitted_roles"), I(b, "is_default"), I(b, "active"),
                        S(b, "approval_status"), S(b, "owner"), S(b, "effective_date"), S(b, "expiry_date"), key);
                if (I(b, "is_default") == 1) db.Execute("UPDATE comm_sender_profiles SET is_default=0 WHERE key<>?", key);
                log(adm.Id, "comm_sender_saved", key);
                return J(new { ok = true });
            });
        });
        app.MapPost("/api/admin/comms/senders/{id:long}/test", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var p = db.QueryOne("SELECT * FROM comm_sender_profiles WHERE id=?", id);
                if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var to = S(b, "to"); if (to.Length == 0) to = adm.Email;
                var oid = Comms.Enqueue(db, "email", $"test:{id}:{to}:{DateTime.UtcNow:yyyyMMddHHmmss}", null, to, null,
                    "PCI Communications Centre — test message",
                    "<p>This is a test message sent from the PCI Communications Centre to verify the sender profile.</p>",
                    senderKey: H.Str(p["key"]), category: "operational", triggerCode: "test.sender", createdBy: adm.Id);
                log(adm.Id, "comm_sender_test", $"{p["key"]} -> {to}");
                return J(new { ok = true, queued = oid is not null, outbox_id = oid });
            });
        });

        // ───────── WhatsApp accounts ─────────
        app.MapGet("/api/admin/comms/whatsapp", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_whatsapp_accounts ORDER BY key") })));
        app.MapPost("/api/admin/comms/whatsapp", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var key = S(b, "key"); if (key.Length == 0) return Results.Json(new { error = "key_required" }, statusCode: 400);
                var existing = db.QueryOne("SELECT id FROM comm_whatsapp_accounts WHERE key=?", key);
                if (existing is null)
                    db.Execute(@"INSERT INTO comm_whatsapp_accounts(key,name,display_name,phone_number,provider,provider_account_id,token_env,purpose,country,permitted_categories,permitted_roles,business_hours,escalation_rule,verification_status,is_default,active,owner)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                        key, S(b, "name"), S(b, "display_name"), S(b, "phone_number"), S(b, "provider") is { Length: > 0 } pv ? pv : "meta_cloud",
                        S(b, "provider_account_id"), S(b, "token_env") is { Length: > 0 } te ? te : "WHATSAPP_ACCESS_TOKEN", S(b, "purpose"),
                        S(b, "country"), S(b, "permitted_categories"), S(b, "permitted_roles"), S(b, "business_hours"), S(b, "escalation_rule"),
                        S(b, "verification_status") is { Length: > 0 } vs ? vs : "unverified", I(b, "is_default"), b.ContainsKey("active") ? I(b, "active") : 1, S(b, "owner"));
                else
                    db.Execute(@"UPDATE comm_whatsapp_accounts SET name=?,display_name=?,phone_number=?,provider=?,provider_account_id=?,token_env=?,purpose=?,country=?,permitted_categories=?,permitted_roles=?,business_hours=?,escalation_rule=?,verification_status=?,is_default=?,active=?,owner=?,updated_at=datetime('now') WHERE key=?",
                        S(b, "name"), S(b, "display_name"), S(b, "phone_number"), S(b, "provider"), S(b, "provider_account_id"), S(b, "token_env"),
                        S(b, "purpose"), S(b, "country"), S(b, "permitted_categories"), S(b, "permitted_roles"), S(b, "business_hours"),
                        S(b, "escalation_rule"), S(b, "verification_status"), I(b, "is_default"), I(b, "active"), S(b, "owner"), key);
                if (I(b, "is_default") == 1) db.Execute("UPDATE comm_whatsapp_accounts SET is_default=0 WHERE key<>?", key);
                log(adm.Id, "comm_whatsapp_saved", key);
                return J(new { ok = true });
            });
        });

        // ───────── templates ─────────
        app.MapGet("/api/admin/comms/templates", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_templates ORDER BY kind,key,version DESC") })));
        app.MapPost("/api/admin/comms/templates", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var idNum = (long)(H.GetNum(b, "id") ?? 0);
                var key = S(b, "key"); if (key.Length == 0) return Results.Json(new { error = "key_required" }, statusCode: 400);
                if (idNum > 0)
                {
                    var cur = db.QueryOne("SELECT * FROM comm_templates WHERE id=?", idNum);
                    if (cur is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                    // Save the previous body as a version, then update (bump version, back to draft).
                    db.Execute("INSERT INTO comm_template_versions(template_id,version,subject,body,wa_template_name,saved_by) VALUES(?,?,?,?,?,?)",
                        idNum, cur["version"], cur["subject"], cur["body"], cur["wa_template_name"], adm.Id);
                    db.Execute(@"UPDATE comm_templates SET name=?,kind=?,category=?,subject=?,body=?,wa_template_name=?,certification_id=?,route_key=?,language=?,required_vars=?,version=version+1,status='draft',updated_at=datetime('now') WHERE id=?",
                        S(b, "name"), S(b, "kind"), S(b, "category"), S(b, "subject"), S(b, "body"), S(b, "wa_template_name"),
                        H.GetNum(b, "certification_id"), S(b, "route_key"), S(b, "language") is { Length: > 0 } lg ? lg : "en", S(b, "required_vars"), idNum);
                    log(adm.Id, "comm_template_updated", $"#{idNum} {key}");
                    return J(new { ok = true, id = idNum });
                }
                var newId = db.ExecuteReturningId(@"INSERT INTO comm_templates(key,name,kind,category,subject,body,wa_template_name,certification_id,route_key,language,required_vars,status,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?, 'draft', ?)",
                    key, S(b, "name"), S(b, "kind") is { Length: > 0 } kd ? kd : "email", S(b, "category"), S(b, "subject"), S(b, "body"),
                    S(b, "wa_template_name"), H.GetNum(b, "certification_id"), S(b, "route_key"), S(b, "language") is { Length: > 0 } l2 ? l2 : "en",
                    S(b, "required_vars"), adm.Id);
                log(adm.Id, "comm_template_created", $"#{newId} {key}");
                return J(new { ok = true, id = newId });
            });
        });
        app.MapPost("/api/admin/comms/templates/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var st = S(b, "status");
                if (st is not ("draft" or "approved" or "published" or "archived")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
                var t = db.QueryOne("SELECT subject,body,required_vars FROM comm_templates WHERE id=?", id);
                if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (st == "published")
                {
                    // Validate that every declared required variable actually appears in the template.
                    var req = (H.Str(t["required_vars"]) ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    var content = (H.Str(t["subject"]) ?? "") + " " + (H.Str(t["body"]) ?? "");
                    var missing = req.Where(v => !content.Contains("{{" + v + "}}") && !content.Contains("{{ " + v + " }}")).ToList();
                    if (missing.Count > 0) return Results.Json(new { error = "missing_variables", missing }, statusCode: 400);
                    db.Execute("UPDATE comm_templates SET status='published', published_at=datetime('now'), approved_by=?, updated_at=datetime('now') WHERE id=?", adm.Id, id);
                }
                else db.Execute("UPDATE comm_templates SET status=?, updated_at=datetime('now') WHERE id=?", st, id);
                log(adm.Id, "comm_template_status", $"#{id} -> {st}");
                return J(new { ok = true, status = st });
            });
        });

        // ───────── triggers ─────────
        app.MapGet("/api/admin/comms/triggers", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_triggers ORDER BY event_group,name") })));
        app.MapMethods("/api/admin/comms/triggers/{id:long}", new[] { "PATCH", "PUT" }, async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var t = db.QueryOne("SELECT id FROM comm_triggers WHERE id=?", id);
                if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var sets = new List<string>(); var args = new List<object?>();
                void Set(string c, object? v) { sets.Add($"{c}=?"); args.Add(v); }
                foreach (var (field, col) in new[] { ("email_enabled", "email_enabled"), ("whatsapp_enabled", "whatsapp_enabled"), ("inapp_enabled", "inapp_enabled"), ("active", "active"), ("approval_required", "approval_required"), ("delay_minutes", "delay_minutes"), ("dedup_window_minutes", "dedup_window_minutes") })
                    if (b.ContainsKey(field)) Set(col, I(b, field));
                foreach (var (field, col) in new[] { ("email_template_key", "email_template_key"), ("whatsapp_template_key", "whatsapp_template_key"), ("sender_profile_key", "sender_profile_key"), ("whatsapp_account_key", "whatsapp_account_key"), ("consent_category", "consent_category"), ("certification_scope", "certification_scope"), ("route_scope", "route_scope"), ("reminder_sequence", "reminder_sequence"), ("conditions", "conditions"), ("effective_date", "effective_date"), ("expiry_date", "expiry_date") })
                    if (b.ContainsKey(field)) Set(col, S(b, field));
                if (sets.Count == 0) return J(new { ok = true });
                sets.Add("updated_at=datetime('now')"); args.Add(id);
                db.Execute($"UPDATE comm_triggers SET {string.Join(",", sets)} WHERE id=?", args.ToArray());
                log(adm.Id, "comm_trigger_updated", $"#{id}");
                return J(new { ok = true });
            });
        });

        // ───────── outbox / delivery queue ─────────
        app.MapGet("/api/admin/comms/outbox", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            var status = req.Query["status"].ToString(); var channel = req.Query["channel"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(channel)) { where.Add("channel=?"); args.Add(channel); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            return J(new { rows = db.Query($"SELECT id,dedup_key,channel,trigger_code,category,user_id,to_email,to_phone,sender_profile_key,subject,status,attempts,max_attempts,last_error,provider,provider_message_id,scheduled_at,sent_at,created_at FROM comm_outbox {w} ORDER BY id DESC LIMIT 500", args.ToArray()) });
        }));
        app.MapGet("/api/admin/comms/outbox/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, _ =>
        {
            var r = db.QueryOne("SELECT * FROM comm_outbox WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var attempts = db.Query("SELECT attempt,status,detail,created_at FROM comm_delivery_attempts WHERE outbox_id=? ORDER BY id", id);
            return J(new { message = r, attempts });
        }));
        app.MapPost("/api/admin/comms/outbox/{id:long}/retry", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var r = db.QueryOne("SELECT status FROM comm_outbox WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) is "sent" or "delivered") return Results.Json(new { error = "already_sent" }, statusCode: 409);
            db.Execute("UPDATE comm_outbox SET status='queued', next_attempt_at=NULL, updated_at=datetime('now') WHERE id=?", id);
            try { OutboxDispatcher.DrainOnce(db, 5); } catch { }
            log(adm.Id, "comm_outbox_retry", $"#{id}");
            return J(new { ok = true });
        }));
        app.MapPost("/api/admin/comms/outbox/{id:long}/cancel", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var r = db.QueryOne("SELECT status FROM comm_outbox WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(r["status"]) is "sent" or "delivered") return Results.Json(new { error = "already_sent" }, statusCode: 409);
            db.Execute("UPDATE comm_outbox SET status='cancelled', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "comm_outbox_cancel", $"#{id}");
            return J(new { ok = true });
        }));
        app.MapPost("/api/admin/comms/outbox/drain", (HttpRequest req) => gate(req, SECTION, adm =>
        {
            try { OutboxDispatcher.DrainOnce(db, 50); } catch (Exception e) { return Results.Json(new { error = "drain_failed", message = e.Message }, statusCode: 500); }
            return J(new { ok = true });
        }));

        // ───────── manual compose ─────────
        app.MapPost("/api/admin/comms/compose", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var channel = S(b, "channel") is { Length: > 0 } ch ? ch : "email";
                var subject = S(b, "subject"); var body = S(b, "body");
                if (body.Length == 0) return Results.Json(new { error = "body_required" }, statusCode: 400);
                var senderKey = S(b, "sender_profile_key");
                var category = S(b, "category") is { Length: > 0 } cat ? cat : "operational";
                var queued = new List<long>();
                // Recipients: explicit user ids, or a raw address.
                var userIds = new List<long>();
                if (H.GetEl(b, "user_ids") is { ValueKind: JsonValueKind.Array } arr)
                    foreach (var e in arr.EnumerateArray()) if (e.TryGetInt64(out var uid)) userIds.Add(uid);
                if (userIds.Count > 0)
                {
                    foreach (var uid in userIds.Distinct())
                    {
                        var u = db.QueryOne("SELECT id,email FROM users WHERE id=?", uid);
                        if (u is null) continue;
                        var oid = Comms.Enqueue(db, channel, $"manual:{adm.Id}:{uid}:{DateTime.UtcNow.Ticks}", uid,
                            H.Str(u["email"]), null, subject, body, senderKey: senderKey, category: category, triggerCode: "manual", createdBy: adm.Id);
                        if (oid is not null) queued.Add(oid.Value);
                    }
                }
                else if (S(b, "to").Length > 0)
                {
                    var to = S(b, "to");
                    var oid = Comms.Enqueue(db, channel, $"manual:{adm.Id}:{to}:{DateTime.UtcNow.Ticks}", null,
                        channel == "whatsapp" ? null : to, channel == "whatsapp" ? to : null, subject, body,
                        senderKey: senderKey, category: category, triggerCode: "manual", createdBy: adm.Id);
                    if (oid is not null) queued.Add(oid.Value);
                }
                else return Results.Json(new { error = "no_recipient" }, statusCode: 400);
                try { OutboxDispatcher.DrainOnce(db, 25); } catch { }
                log(adm.Id, "comm_manual_sent", $"{channel} x{queued.Count}");
                return J(new { ok = true, queued = queued.Count, outbox_ids = queued });
            });
        });

        // ───────── suppression ─────────
        app.MapGet("/api/admin/comms/suppression", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_suppression ORDER BY id DESC LIMIT 500") })));
        app.MapPost("/api/admin/comms/suppression", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var addr = S(b, "address"); if (addr.Length == 0) return Results.Json(new { error = "address_required" }, statusCode: 400);
                db.Execute("INSERT INTO comm_suppression(channel,address,reason,category,source) VALUES(?,?,?,?, 'manual')",
                    S(b, "channel") is { Length: > 0 } c ? c : "email", addr, S(b, "reason"), S(b, "category"));
                log(adm.Id, "comm_suppression_add", addr);
                return J(new { ok = true });
            });
        });
        app.MapDelete("/api/admin/comms/suppression/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            db.Execute("DELETE FROM comm_suppression WHERE id=?", id);
            log(adm.Id, "comm_suppression_remove", $"#{id}");
            return J(new { ok = true });
        }));

        // ───────── unified inbox (conversations) ─────────
        app.MapGet("/api/admin/comms/conversations", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            var status = req.Query["status"].ToString();
            var w = string.IsNullOrEmpty(status) ? "" : "WHERE status=?";
            var rows = string.IsNullOrEmpty(status)
                ? db.Query("SELECT * FROM comm_conversations ORDER BY COALESCE(last_message_at,created_at) DESC LIMIT 300")
                : db.Query("SELECT * FROM comm_conversations WHERE status=? ORDER BY COALESCE(last_message_at,created_at) DESC LIMIT 300", status);
            return J(new { rows });
        }));
        app.MapGet("/api/admin/comms/conversations/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, _ =>
        {
            var c = db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var messages = db.Query("SELECT id,direction,channel,from_addr,to_addr,body,is_internal_note,author_admin_id,created_at FROM comm_inbound_messages WHERE conversation_id=? ORDER BY id", id);
            return J(new { conversation = c, messages });
        }));
        app.MapPost("/api/admin/comms/conversations/{id:long}/reply", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var c = db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", id);
                if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var body = S(b, "body"); if (body.Length == 0) return Results.Json(new { error = "body_required" }, statusCode: 400);
                var isNote = I(b, "internal_note") == 1;
                db.Execute("INSERT INTO comm_inbound_messages(conversation_id,direction,channel,from_addr,to_addr,body,is_internal_note,author_admin_id) VALUES(?, 'out', ?, ?, ?, ?, ?, ?)",
                    id, H.Str(c["channel"]), adm.Email, H.Str(c["customer_email"]) ?? H.Str(c["customer_phone"]), body, isNote ? 1 : 0, adm.Id);
                db.Execute("UPDATE comm_conversations SET last_message_at=datetime('now'), status=CASE WHEN status='resolved' THEN 'open' ELSE status END, updated_at=datetime('now') WHERE id=?", id);
                if (!isNote)
                {
                    var channel = H.Str(c["channel"]) ?? "email";
                    Comms.Enqueue(db, channel == "whatsapp" ? "whatsapp" : "email",
                        $"reply:{id}:{DateTime.UtcNow.Ticks}", c["user_id"] as long?,
                        H.Str(c["customer_email"]), H.Str(c["customer_phone"]),
                        "Re: " + (H.Str(c["subject"]) ?? "Your enquiry"), body,
                        senderKey: "support", category: "support", triggerCode: "support.agent_replied", conversationId: id, createdBy: adm.Id);
                    try { OutboxDispatcher.DrainOnce(db, 5); } catch { }
                }
                log(adm.Id, isNote ? "comm_note_added" : "comm_reply_sent", $"conv #{id}");
                return J(new { ok = true });
            });
        });
        app.MapPost("/api/admin/comms/conversations/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var st = S(b, "status");
                if (st is not ("open" or "pending" or "resolved" or "escalated" or "closed")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
                db.Execute("UPDATE comm_conversations SET status=?, assigned_admin_id=COALESCE(?,assigned_admin_id), updated_at=datetime('now') WHERE id=?",
                    st, b.ContainsKey("assign_self") && I(b, "assign_self") == 1 ? adm.Id : (object?)null, id);
                log(adm.Id, "comm_conversation_status", $"#{id} -> {st}");
                return J(new { ok = true });
            });
        });

        // ───────── inbound webhooks (public, secret-verified) ─────────
        // Generic email-inbound (Resend/Mailgun/SendGrid parse). Verified by a shared secret in the query
        // or the X-Webhook-Secret header — matched against EMAIL_WEBHOOK_SECRET. Attaches to a conversation
        // by an existing reference token in the subject ([PCI-CONV-<id>]) or creates a new one.
        app.MapPost("/api/webhooks/email-inbound", async (HttpContext ctx) =>
        {
            var secret = Environment.GetEnvironmentVariable("EMAIL_WEBHOOK_SECRET");
            var given = ctx.Request.Query["secret"].ToString();
            if (string.IsNullOrEmpty(given)) given = ctx.Request.Headers["X-Webhook-Secret"].ToString();
            if (string.IsNullOrEmpty(secret) || !Security.FixedTimeEquals(given, secret))
                return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var from = (H.GetS(b, "from", "sender", "From") ?? "").Trim();
            var to = (H.GetS(b, "to", "recipient", "To") ?? "").Trim();
            var subject = (H.GetS(b, "subject", "Subject") ?? "(no subject)").Trim();
            var text = H.GetS(b, "text", "body-plain", "body", "stripped-text") ?? "";
            var convId = MatchConversation(db, subject) ?? UpsertConversation(db, "email", from, subject, to);
            db.Execute("INSERT INTO comm_inbound_messages(conversation_id,direction,channel,from_addr,to_addr,body,provider_message_id) VALUES(?, 'in', 'email', ?, ?, ?, ?)",
                convId, from, to, text.Length > 20000 ? text[..20000] : text, H.GetS(b, "message_id", "Message-Id"));
            db.Execute("UPDATE comm_conversations SET last_message_at=datetime('now'), status=CASE WHEN status IN('resolved','closed') THEN 'open' ELSE status END, updated_at=datetime('now') WHERE id=?", convId);
            AutoAck(db, convId);
            return J(new { ok = true, conversation_id = convId });
        });

        // WhatsApp Cloud webhook: GET verification challenge + POST inbound messages.
        app.MapGet("/api/webhooks/whatsapp", (HttpRequest req) =>
        {
            var verify = Environment.GetEnvironmentVariable("WHATSAPP_VERIFY_TOKEN");
            var mode = req.Query["hub.mode"].ToString();
            var token = req.Query["hub.verify_token"].ToString();
            var challenge = req.Query["hub.challenge"].ToString();
            if (mode == "subscribe" && !string.IsNullOrEmpty(verify) && Security.FixedTimeEquals(token, verify))
                return Results.Text(challenge);
            return Results.StatusCode(403);
        });
        app.MapPost("/api/webhooks/whatsapp", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            try
            {
                if (H.GetEl(b, "entry") is { ValueKind: JsonValueKind.Array } entries)
                    foreach (var entry in entries.EnumerateArray())
                        if (entry.TryGetProperty("changes", out var changes) && changes.ValueKind == JsonValueKind.Array)
                            foreach (var change in changes.EnumerateArray())
                                if (change.TryGetProperty("value", out var val) && val.TryGetProperty("messages", out var msgs) && msgs.ValueKind == JsonValueKind.Array)
                                    foreach (var m in msgs.EnumerateArray())
                                    {
                                        var fromPhone = m.TryGetProperty("from", out var fp) ? fp.GetString() : null;
                                        var textBody = m.TryGetProperty("text", out var tx) && tx.TryGetProperty("body", out var tb) ? tb.GetString() : "(non-text message)";
                                        var mid = m.TryGetProperty("id", out var idp) ? idp.GetString() : null;
                                        if (string.IsNullOrEmpty(fromPhone)) continue;
                                        var convId = db.QueryOne("SELECT id FROM comm_conversations WHERE customer_phone=? AND channel='whatsapp' ORDER BY id DESC LIMIT 1", fromPhone) is { } ex
                                            ? H.L(ex["id"]) : UpsertConversation(db, "whatsapp", fromPhone, "WhatsApp message", null);
                                        db.Execute("INSERT INTO comm_inbound_messages(conversation_id,direction,channel,from_addr,body,provider_message_id) VALUES(?, 'in', 'whatsapp', ?, ?, ?)",
                                            convId, fromPhone, textBody, mid);
                                        db.Execute("UPDATE comm_conversations SET last_message_at=datetime('now'), status='open', updated_at=datetime('now') WHERE id=?", convId);
                                        AutoAck(db, convId);
                                    }
            }
            catch (Exception e) { Console.Error.WriteLine($"[comms] whatsapp webhook parse: {e.Message}"); }
            return Results.Ok(new { ok = true }); // always 200 so the provider doesn't retry-storm
        });
    }

    // Find a conversation referenced by a [PCI-CONV-<id>] token in the subject (our outbound reply tag).
    static long? MatchConversation(Db db, string subject)
    {
        var m = System.Text.RegularExpressions.Regex.Match(subject ?? "", @"PCI-CONV-(\d+)");
        if (m.Success && long.TryParse(m.Groups[1].Value, out var id) && db.QueryOne("SELECT id FROM comm_conversations WHERE id=?", id) is not null)
            return id;
        return null;
    }

    static long UpsertConversation(Db db, string channel, string fromAddr, string subject, string? receivedAddr)
    {
        // Link to a known user by email where possible (never cross-links different customers).
        long? userId = null;
        if (channel == "email")
        {
            var email = System.Text.RegularExpressions.Regex.Match(fromAddr ?? "", @"[^<>\s]+@[^<>\s]+").Value.ToLowerInvariant();
            if (email.Length > 0 && db.QueryOne("SELECT id FROM users WHERE lower(email)=?", email) is { } u) userId = H.L(u["id"]);
        }
        var reference = "PCI-CONV-" + DateTime.UtcNow.ToString("yyMMdd") + "-" + Security.RandomHex(3).ToUpperInvariant();
        var email2 = channel == "email" ? fromAddr : null;
        var phone = channel == "whatsapp" ? fromAddr : null;
        return db.ExecuteReturningId(@"INSERT INTO comm_conversations(reference,channel,subject,customer_email,customer_phone,user_id,received_address,status,last_message_at)
            VALUES(?,?,?,?,?,?,?, 'open', datetime('now'))", reference, channel, subject, email2, phone, userId, receivedAddr);
    }

    // Send a single automatic acknowledgement per conversation (idempotent via a dedup key).
    static void AutoAck(Db db, long convId)
    {
        var c = db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", convId);
        if (c is null) return;
        var channel = H.Str(c["channel"]) ?? "email";
        var dedup = $"autoack:{convId}";
        var reference = H.Str(c["reference"]) ?? ("PCI-CONV-" + convId);
        var body = channel == "email"
            ? $"<p>Thank you for contacting PCI. Your request has been received under reference <strong>{reference}</strong>. Our support team will review it and respond as soon as possible.</p>"
            : $"Thank you for contacting PCI. Your request has been received under reference {reference}. Our support team will respond as soon as possible.";
        Comms.Enqueue(db, channel == "whatsapp" ? "whatsapp" : "email", dedup, c["user_id"] as long?,
            H.Str(c["customer_email"]), H.Str(c["customer_phone"]),
            $"We've received your message [{reference}]", body,
            senderKey: "support", category: "support", triggerCode: "support.auto_ack", conversationId: convId);
    }
}
