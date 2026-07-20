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
        static string? Nz(string s) => string.IsNullOrWhiteSpace(s) ? null : s;

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
            J(new { rows = db.Query("SELECT * FROM comm_sender_profiles ORDER BY `key`") })));
        app.MapPost("/api/admin/comms/senders", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var key = S(b, "key"); if (key.Length == 0) return Results.Json(new { error = "key_required" }, statusCode: 400);
                var existing = db.QueryOne("SELECT id FROM comm_sender_profiles WHERE `key`=?", key);
                if (existing is null)
                    db.Execute(@"INSERT INTO comm_sender_profiles(`key`,name,display_name,from_email,reply_to,purpose,category,provider,domain_verified,permitted_roles,is_default,active,approval_status,owner,effective_date,expiry_date)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                        key, S(b, "name"), S(b, "display_name"), S(b, "from_email"), S(b, "reply_to"), S(b, "purpose"),
                        S(b, "category"), S(b, "provider"), I(b, "domain_verified"), S(b, "permitted_roles"),
                        I(b, "is_default"), b.ContainsKey("active") ? I(b, "active") : 1, S(b, "approval_status") is { Length: > 0 } a ? a : "approved",
                        S(b, "owner"), S(b, "effective_date"), S(b, "expiry_date"));
                else
                    db.Execute(@"UPDATE comm_sender_profiles SET name=?,display_name=?,from_email=?,reply_to=?,purpose=?,category=?,provider=?,domain_verified=?,permitted_roles=?,is_default=?,active=?,approval_status=?,owner=?,effective_date=?,expiry_date=?,updated_at=datetime('now') WHERE `key`=?",
                        S(b, "name"), S(b, "display_name"), S(b, "from_email"), S(b, "reply_to"), S(b, "purpose"), S(b, "category"),
                        S(b, "provider"), I(b, "domain_verified"), S(b, "permitted_roles"), I(b, "is_default"), I(b, "active"),
                        S(b, "approval_status"), S(b, "owner"), S(b, "effective_date"), S(b, "expiry_date"), key);
                if (I(b, "is_default") == 1) db.Execute("UPDATE comm_sender_profiles SET is_default=0 WHERE `key`<>?", key);
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
            J(new { rows = db.Query("SELECT * FROM comm_whatsapp_accounts ORDER BY `key`") })));
        app.MapPost("/api/admin/comms/whatsapp", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var key = S(b, "key"); if (key.Length == 0) return Results.Json(new { error = "key_required" }, statusCode: 400);
                var existing = db.QueryOne("SELECT id FROM comm_whatsapp_accounts WHERE `key`=?", key);
                if (existing is null)
                    db.Execute(@"INSERT INTO comm_whatsapp_accounts(`key`,name,display_name,phone_number,provider,provider_account_id,token_env,purpose,country,permitted_categories,permitted_roles,business_hours,escalation_rule,verification_status,is_default,active,owner)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                        key, S(b, "name"), S(b, "display_name"), S(b, "phone_number"), S(b, "provider") is { Length: > 0 } pv ? pv : "meta_cloud",
                        S(b, "provider_account_id"), S(b, "token_env") is { Length: > 0 } te ? te : "WHATSAPP_ACCESS_TOKEN", S(b, "purpose"),
                        S(b, "country"), S(b, "permitted_categories"), S(b, "permitted_roles"), S(b, "business_hours"), S(b, "escalation_rule"),
                        S(b, "verification_status") is { Length: > 0 } vs ? vs : "unverified", I(b, "is_default"), b.ContainsKey("active") ? I(b, "active") : 1, S(b, "owner"));
                else
                    db.Execute(@"UPDATE comm_whatsapp_accounts SET name=?,display_name=?,phone_number=?,provider=?,provider_account_id=?,token_env=?,purpose=?,country=?,permitted_categories=?,permitted_roles=?,business_hours=?,escalation_rule=?,verification_status=?,is_default=?,active=?,owner=?,updated_at=datetime('now') WHERE `key`=?",
                        S(b, "name"), S(b, "display_name"), S(b, "phone_number"), S(b, "provider"), S(b, "provider_account_id"), S(b, "token_env"),
                        S(b, "purpose"), S(b, "country"), S(b, "permitted_categories"), S(b, "permitted_roles"), S(b, "business_hours"),
                        S(b, "escalation_rule"), S(b, "verification_status"), I(b, "is_default"), I(b, "active"), S(b, "owner"), key);
                if (I(b, "is_default") == 1) db.Execute("UPDATE comm_whatsapp_accounts SET is_default=0 WHERE `key`<>?", key);
                log(adm.Id, "comm_whatsapp_saved", key);
                return J(new { ok = true });
            });
        });

        // ───────── templates ─────────
        app.MapGet("/api/admin/comms/templates", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT * FROM comm_templates ORDER BY kind,`key`,version DESC") })));
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
                var newId = db.ExecuteReturningId(@"INSERT INTO comm_templates(`key`,name,kind,category,subject,body,wa_template_name,certification_id,route_key,language,required_vars,status,created_by)
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

        // ───────── bulk campaigns ─────────
        // Resolve the audience for a campaign's filters → list of (userId, email). Defensive: unknown
        // columns/tables are skipped, never throw. Marketing campaigns require email-marketing consent.
        List<(long id, string email)> Audience(Dictionary<string, JsonElement>? filters, bool marketing)
        {
            var where = new List<string> { "u.email IS NOT NULL", "u.email<>''" };
            var args = new List<object?>();
            string? Ff(string k) => filters is not null && filters.TryGetValue(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
            bool Fb(string k) => filters is not null && filters.TryGetValue(k, out var v) && (v.ValueKind == JsonValueKind.True || (v.ValueKind == JsonValueKind.String && v.GetString() is "1" or "true"));
            if (Fb("active_only")) where.Add("u.status='active'");
            if (Ff("registered_after") is { Length: > 0 } ra) { where.Add("u.created_at>=?"); args.Add(ra); }
            if (Ff("registered_before") is { Length: > 0 } rb) { where.Add("u.created_at<=?"); args.Add(rb); }
            if (Fb("has_membership")) where.Add("EXISTS(SELECT 1 FROM memberships m WHERE m.user_id=u.id)");
            if (Ff("certification") is { Length: > 0 } cert) { where.Add("EXISTS(SELECT 1 FROM held_certifications h WHERE h.user_id=u.id AND h.name LIKE ?)"); args.Add("%" + cert + "%"); }
            if (marketing) where.Add("EXISTS(SELECT 1 FROM comm_preferences p WHERE p.user_id=u.id AND p.email_marketing=1)");
            try
            {
                return db.Query($"SELECT u.id, u.email FROM users u WHERE {string.Join(" AND ", where)} GROUP BY u.id LIMIT 20000", args.ToArray())
                    .Select(r => (H.L(r["id"]), (H.Str(r["email"]) ?? "").ToLowerInvariant()))
                    .Where(x => x.Item2.Length > 0).Distinct().ToList();
            }
            catch { return new(); }
        }
        static string Mask(string email)
        {
            var at = email.IndexOf('@'); if (at <= 1) return "***" + (at >= 0 ? email[at..] : "");
            return email[0] + new string('*', Math.Min(5, at - 1)) + email[at..];
        }

        app.MapGet("/api/admin/comms/campaigns", (HttpRequest req) => gate(req, SECTION, _ =>
            J(new { rows = db.Query("SELECT id,name,channel,category,status,total,queued,scheduled_at,approved_at,created_at FROM comm_campaigns ORDER BY id DESC LIMIT 300") })));
        app.MapGet("/api/admin/comms/campaigns/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, _ =>
        {
            var c = db.QueryOne("SELECT * FROM comm_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // Live delivery breakdown from the outbox rows this campaign produced.
            var breakdown = db.Query("SELECT status, COUNT(*) AS n FROM comm_outbox WHERE campaign_id=? GROUP BY status", id);
            return J(new { campaign = c, delivery = breakdown });
        }));
        app.MapPost("/api/admin/comms/campaigns", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var filtersJson = H.GetEl(b, "filters") is { ValueKind: JsonValueKind.Object } fj ? fj.GetRawText() : "{}";
                var idNum = (long)(H.GetNum(b, "id") ?? 0);
                if (idNum > 0)
                {
                    db.Execute(@"UPDATE comm_campaigns SET name=?,channel=?,category=?,subject=?,body=?,sender_profile_key=?,filters=?,scheduled_at=?,updated_at=datetime('now') WHERE id=? AND status IN('draft','pending_approval')",
                        name, S(b, "channel"), S(b, "category"), S(b, "subject"), S(b, "body"), S(b, "sender_profile_key"), filtersJson, S(b, "scheduled_at"), idNum);
                    log(adm.Id, "comm_campaign_updated", $"#{idNum}");
                    return J(new { ok = true, id = idNum });
                }
                var newId = db.ExecuteReturningId(@"INSERT INTO comm_campaigns(name,channel,category,subject,body,sender_profile_key,filters,scheduled_at,status,created_by)
                    VALUES(?,?,?,?,?,?,?,?, 'draft', ?)",
                    name, S(b, "channel") is { Length: > 0 } ch ? ch : "email", S(b, "category") is { Length: > 0 } ca ? ca : "operational",
                    S(b, "subject"), S(b, "body"), S(b, "sender_profile_key") is { Length: > 0 } sp ? sp : "marketing", filtersJson, S(b, "scheduled_at"), adm.Id);
                log(adm.Id, "comm_campaign_created", $"#{newId} {name}");
                return J(new { ok = true, id = newId });
            });
        });
        app.MapPost("/api/admin/comms/campaigns/{id:long}/preview", (HttpRequest req, long id) => gate(req, SECTION, _ =>
        {
            var c = db.QueryOne("SELECT * FROM comm_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var filters = H.Str(c["filters"]) is { Length: > 0 } fj ? H.ToMap(JsonDocument.Parse(fj).RootElement) : null;
            var marketing = H.Str(c["category"]) is "marketing" or "newsletter";
            var aud = Audience(filters, marketing);
            // Suppression removes already-opted-out addresses from the count shown.
            var suppressed = 0;
            var eligible = aud.Where(x => { var sup = db.QueryOne("SELECT id FROM comm_suppression WHERE channel='email' AND lower(address)=?", x.email) is not null; if (sup) suppressed++; return !sup; }).ToList();
            return J(new
            {
                total = eligible.Count, suppressed, raw = aud.Count,
                sample = eligible.Take(5).Select(x => Mask(x.email)).ToList(),
                note = "Recipient addresses are masked and are never exposed to other recipients — each message is sent separately.",
            });
        }));
        app.MapPost("/api/admin/comms/campaigns/{id:long}/test", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var c = db.QueryOne("SELECT * FROM comm_campaigns WHERE id=?", id);
                if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var to = S(b, "to"); if (to.Length == 0) to = adm.Email;
                Comms.Enqueue(db, "email", $"camptest:{id}:{to}:{DateTime.UtcNow.Ticks}", null, to, null,
                    "[TEST] " + (H.Str(c["subject"]) ?? "Campaign"), H.Str(c["body"]) ?? "", senderKey: H.Str(c["sender_profile_key"]),
                    category: "operational", triggerCode: "campaign.test", campaignId: id, createdBy: adm.Id);
                try { OutboxDispatcher.DrainOnce(db, 5); } catch { }
                log(adm.Id, "comm_campaign_test", $"#{id} -> {to}");
                return J(new { ok = true, sent_to = to });
            });
        });
        app.MapPost("/api/admin/comms/campaigns/{id:long}/approve", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var c = db.QueryOne("SELECT status FROM comm_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE comm_campaigns SET status='approved', approved_by=?, approved_at=datetime('now'), updated_at=datetime('now') WHERE id=?", adm.Id, id);
            log(adm.Id, "comm_campaign_approved", $"#{id}");
            return J(new { ok = true });
        }));
        app.MapPost("/api/admin/comms/campaigns/{id:long}/send", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var c = db.QueryOne("SELECT * FROM comm_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(c["status"]) != "approved") return Results.Json(new { error = "not_approved", message = "Approve the campaign before sending." }, statusCode: 409);
            var filters = H.Str(c["filters"]) is { Length: > 0 } fj ? H.ToMap(JsonDocument.Parse(fj).RootElement) : null;
            var marketing = H.Str(c["category"]) is "marketing" or "newsletter";
            var aud = Audience(filters, marketing);
            db.Execute("UPDATE comm_campaigns SET status='sending', updated_at=datetime('now') WHERE id=?", id);
            int queued = 0;
            foreach (var (uid, email) in aud)
            {
                // One separate message per recipient (privacy-safe). Enqueue enforces suppression + consent
                // + a per-recipient dedup key, so a re-run never double-sends to the same person.
                var oid = Comms.Enqueue(db, "email", $"campaign:{id}:{email}", uid, email, null,
                    H.Str(c["subject"]) ?? "", H.Str(c["body"]) ?? "", senderKey: H.Str(c["sender_profile_key"]),
                    category: H.Str(c["category"]) ?? "operational", triggerCode: "campaign.send", campaignId: id, createdBy: adm.Id);
                if (oid is not null) queued++;
            }
            db.Execute("UPDATE comm_campaigns SET status='sent', total=?, queued=?, updated_at=datetime('now') WHERE id=?", aud.Count, queued, id);
            try { OutboxDispatcher.DrainOnce(db, 100); } catch { }
            log(adm.Id, "comm_campaign_sent", $"#{id} queued={queued}/{aud.Count}");
            return J(new { ok = true, audience = aud.Count, queued });
        }));
        app.MapPost("/api/admin/comms/campaigns/{id:long}/{action}", (HttpRequest req, long id, string action) => gate(req, SECTION, adm =>
        {
            if (action is not ("pause" or "resume" or "cancel")) return Results.Json(new { error = "bad_action" }, statusCode: 400);
            var st = action switch { "pause" => "paused", "resume" => "approved", _ => "cancelled" };
            db.Execute("UPDATE comm_campaigns SET status=?, updated_at=datetime('now') WHERE id=?", st, id);
            if (action == "cancel") db.Execute("UPDATE comm_outbox SET status='cancelled' WHERE campaign_id=? AND status IN('queued','scheduled','retrying')", id);
            log(adm.Id, "comm_campaign_" + action, $"#{id}");
            return J(new { ok = true, status = st });
        }));

        // ───────── analytics ─────────
        app.MapGet("/api/admin/comms/analytics", (HttpRequest req) => gate(req, SECTION, _ => J(new
        {
            by_status = db.Query("SELECT status, COUNT(*) AS n FROM comm_outbox GROUP BY status ORDER BY n DESC"),
            by_channel = db.Query("SELECT channel, COUNT(*) AS n FROM comm_outbox GROUP BY channel"),
            by_day = db.Query("SELECT substr(COALESCE(sent_at,created_at),1,10) AS day, COUNT(*) AS n FROM comm_outbox GROUP BY day ORDER BY day DESC LIMIT 14"),
            top_triggers = db.Query("SELECT trigger_code, COUNT(*) AS n FROM comm_outbox WHERE trigger_code IS NOT NULL GROUP BY trigger_code ORDER BY n DESC LIMIT 12"),
            failures = db.Query("SELECT last_error, COUNT(*) AS n FROM comm_outbox WHERE status='failed' AND last_error IS NOT NULL GROUP BY last_error ORDER BY n DESC LIMIT 10"),
        })));

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

        // ───────── customer-service routing rules + FAQ auto-response config (Phase 5) ─────────
        app.MapGet("/api/admin/comms/routing", (HttpRequest req) => gate(req, SECTION, _ => J(new
        {
            rows = db.Query("SELECT * FROM comm_routing_rules ORDER BY priority, id"),
            faq_mode = Settings.Str(db, "comms_faq_mode", "suggest"),
            sensitive_keywords = Settings.Str(db, "comms_sensitive_keywords", ""),
        })));
        app.MapPost("/api/admin/comms/routing", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var id = db.ExecuteReturningId(@"INSERT INTO comm_routing_rules
                    (name,priority,match_channel,match_received_address,match_keywords,match_certification_id,
                     set_category,set_priority,assign_admin_id,sla_hours,add_tags,escalate,active,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    name, I(b, "priority") == 0 ? 100 : I(b, "priority"),
                    Nz(S(b, "match_channel")), Nz(S(b, "match_received_address")), Nz(S(b, "match_keywords")),
                    b.ContainsKey("match_certification_id") ? (long?)I(b, "match_certification_id") : null,
                    Nz(S(b, "set_category")), Nz(S(b, "set_priority")),
                    b.ContainsKey("assign_admin_id") && I(b, "assign_admin_id") > 0 ? (long?)I(b, "assign_admin_id") : null,
                    b.ContainsKey("sla_hours") && I(b, "sla_hours") > 0 ? (long?)I(b, "sla_hours") : null,
                    Nz(S(b, "add_tags")), I(b, "escalate") == 1 ? 1 : 0, 1, adm.Id);
                log(adm.Id, "comm_routing_rule_add", $"#{id} {name}");
                return J(new { ok = true, id });
            });
        });
        app.MapPost("/api/admin/comms/routing/{id:long}", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                if (db.QueryOne("SELECT id FROM comm_routing_rules WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                void Set(string col, object? v) => db.Execute($"UPDATE comm_routing_rules SET {col}=?, updated_at=datetime('now') WHERE id=?", v, id);
                if (b.ContainsKey("name") && S(b, "name").Length > 0) Set("name", S(b, "name"));
                if (b.ContainsKey("priority")) Set("priority", I(b, "priority"));
                if (b.ContainsKey("match_channel")) Set("match_channel", Nz(S(b, "match_channel")));
                if (b.ContainsKey("match_received_address")) Set("match_received_address", Nz(S(b, "match_received_address")));
                if (b.ContainsKey("match_keywords")) Set("match_keywords", Nz(S(b, "match_keywords")));
                if (b.ContainsKey("match_certification_id")) Set("match_certification_id", I(b, "match_certification_id") > 0 ? (long?)I(b, "match_certification_id") : null);
                if (b.ContainsKey("set_category")) Set("set_category", Nz(S(b, "set_category")));
                if (b.ContainsKey("set_priority")) Set("set_priority", Nz(S(b, "set_priority")));
                if (b.ContainsKey("assign_admin_id")) Set("assign_admin_id", I(b, "assign_admin_id") > 0 ? (long?)I(b, "assign_admin_id") : null);
                if (b.ContainsKey("sla_hours")) Set("sla_hours", I(b, "sla_hours") > 0 ? (long?)I(b, "sla_hours") : null);
                if (b.ContainsKey("add_tags")) Set("add_tags", Nz(S(b, "add_tags")));
                if (b.ContainsKey("escalate")) Set("escalate", I(b, "escalate") == 1 ? 1 : 0);
                if (b.ContainsKey("active")) Set("active", I(b, "active") == 1 ? 1 : 0);
                log(adm.Id, "comm_routing_rule_update", $"#{id}");
                return J(new { ok = true });
            });
        });
        app.MapDelete("/api/admin/comms/routing/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            db.Execute("DELETE FROM comm_routing_rules WHERE id=?", id);
            log(adm.Id, "comm_routing_rule_delete", $"#{id}");
            return J(new { ok = true });
        }));
        // FAQ auto-response mode + sensitive-keyword floor.
        app.MapPost("/api/admin/comms/autoresponse", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                if (b.ContainsKey("faq_mode"))
                {
                    var m = S(b, "faq_mode");
                    if (m is not ("suggest" or "auto" or "escalate_all")) return Results.Json(new { error = "bad_mode" }, statusCode: 400);
                    Settings.Put(db, "comms_faq_mode", m);
                }
                if (b.ContainsKey("sensitive_keywords")) Settings.Put(db, "comms_sensitive_keywords", S(b, "sensitive_keywords"));
                log(adm.Id, "comm_autoresponse_config", Settings.Str(db, "comms_faq_mode", "suggest"));
                return J(new { ok = true });
            });
        });
        // Accept the drafted (suggested) FAQ response on a conversation: sends it and clears the draft.
        app.MapPost("/api/admin/comms/conversations/{id:long}/accept-suggestion", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var c = db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var draft = H.Str(c["suggested_response"]);
            if (string.IsNullOrWhiteSpace(draft)) return Results.Json(new { error = "no_suggestion" }, statusCode: 400);
            var channel = H.Str(c["channel"]) ?? "email";
            db.Execute("INSERT INTO comm_inbound_messages(conversation_id,direction,channel,from_addr,to_addr,body,author_admin_id) VALUES(?, 'out', ?, ?, ?, ?, ?)",
                id, channel, adm.Email, H.Str(c["customer_email"]) ?? H.Str(c["customer_phone"]), draft, adm.Id);
            Comms.Enqueue(db, channel == "whatsapp" ? "whatsapp" : "email", $"suggestion:{id}:{DateTime.UtcNow.Ticks}",
                c["user_id"] as long?, H.Str(c["customer_email"]), H.Str(c["customer_phone"]),
                "Re: " + (H.Str(c["subject"]) ?? "Your enquiry"), channel == "email" ? $"<p>{System.Net.WebUtility.HtmlEncode(draft)}</p>" : draft,
                senderKey: "support", category: "support", triggerCode: "support.suggestion_sent", conversationId: id, createdBy: adm.Id);
            db.Execute("UPDATE comm_conversations SET suggested_response=NULL, last_message_at=datetime('now'), status=CASE WHEN status='resolved' THEN 'open' ELSE status END, updated_at=datetime('now') WHERE id=?", id);
            try { OutboxDispatcher.DrainOnce(db, 5); } catch { }
            log(adm.Id, "comm_suggestion_sent", $"conv #{id}");
            return J(new { ok = true });
        }));

        // ───────── public one-click unsubscribe (signed token — no login) ─────────
        // GET renders a tiny confirmation page; both GET and POST perform the unsubscribe idempotently.
        void DoUnsub(long uid)
        {
            db.Execute("INSERT INTO comm_preferences(user_id) SELECT ? WHERE NOT EXISTS(SELECT 1 FROM comm_preferences WHERE user_id=?)", uid, uid);
            db.Execute("UPDATE comm_preferences SET email_marketing=0, newsletter=0, surveys=0, events=0, withdrawn_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?", uid);
            var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", uid)?["email"]);
            if (!string.IsNullOrEmpty(email))
                db.Execute("INSERT INTO comm_suppression(channel,address,reason,category,source) VALUES('email',?, 'unsubscribe','marketing','one_click')", email.ToLowerInvariant());
            log(uid, "comm_unsubscribed", "one_click");
        }
        app.MapGet("/api/comms/unsubscribe", (HttpRequest req) =>
        {
            var uid = Comms.VerifyUnsub(req.Query["token"].ToString());
            if (uid is null) return Results.Content("<h2>Unsubscribe link invalid or expired.</h2>", "text/html");
            DoUnsub(uid.Value);
            return Results.Content("<html><body style='font-family:sans-serif;max-width:520px;margin:60px auto;text-align:center'><h2>You've been unsubscribed</h2><p>You will no longer receive PCI marketing or newsletter emails. Essential account, application, payment, exam and certificate messages will still be sent. You can re-subscribe any time from your portal preferences.</p></body></html>", "text/html");
        });
        app.MapPost("/api/comms/unsubscribe", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var uid = Comms.VerifyUnsub(H.GetS(b, "token"));
            if (uid is null) return Results.Json(new { error = "invalid_token" }, statusCode: 400);
            DoUnsub(uid.Value);
            return J(new { ok = true });
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
            CommsRouting.Apply(db, convId, text);      // auto-triage: classify, route, and FAQ-answer where safe
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
                                        CommsRouting.Apply(db, convId, textBody ?? "");   // auto-triage + FAQ answer where safe
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
