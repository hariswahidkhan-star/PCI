using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Unified Communications Centre core: template rendering, the outbox enqueue (with duplicate prevention),
/// and the per-channel provider seam (email via Resend/SMTP/console; WhatsApp via the Meta Cloud API).
/// Provider SECRETS are read from environment variables only — never from the database or the client.
/// Every outbound message is a row in comm_outbox; the OutboxDispatcher drains it with retries, so a
/// provider outage never loses the business event.
/// </summary>
public static class Comms
{
    static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(20) };
    static string? E(string k) { var v = Environment.GetEnvironmentVariable(k); return string.IsNullOrWhiteSpace(v) ? null : v; }

    /// <summary>Substitute {{variable}} tokens. Unknown tokens are left blank (never leaked as literals).</summary>
    public static string Render(string? tpl, IDictionary<string, string?>? vars)
    {
        if (string.IsNullOrEmpty(tpl)) return "";
        return System.Text.RegularExpressions.Regex.Replace(tpl, @"\{\{\s*([a-zA-Z0-9_]+)\s*\}\}", m =>
        {
            var key = m.Groups[1].Value;
            return vars is not null && vars.TryGetValue(key, out var v) ? (v ?? "") : "";
        });
    }

    /// <summary>Enqueue one outbox row. Returns the new id, or null when a matching dedup_key already exists
    /// (duplicate suppressed) or the recipient is suppressed/opted-out for a marketing message.</summary>
    public static long? Enqueue(Db db, string channel, string? dedupKey, long? userId, string? toEmail, string? toPhone,
        string subject, string body, string? templateKey = null, string? senderKey = null, string? waAccountKey = null,
        string category = "operational", string? triggerCode = null, long? certId = null, long? createdBy = null,
        long? conversationId = null, long? campaignId = null, string? scheduledAt = null)
    {
        // Duplicate prevention: a non-empty dedup_key that already exists means the event was already queued.
        if (!string.IsNullOrEmpty(dedupKey) && db.QueryOne("SELECT id FROM comm_outbox WHERE dedup_key=?", dedupKey) is not null)
            return null;
        // Suppression / consent: marketing to a suppressed or non-consented recipient is dropped (recorded).
        var addr = channel == "whatsapp" ? toPhone : toEmail;
        if (IsMarketing(category))
        {
            if (!string.IsNullOrEmpty(addr) && db.QueryOne("SELECT id FROM comm_suppression WHERE channel=? AND lower(address)=lower(?)", channel, addr) is not null)
                return null;
            if (userId is not null && !HasMarketingConsent(db, userId.Value, channel)) return null;
        }
        try
        {
            return db.ExecuteReturningId(@"INSERT INTO comm_outbox
                (dedup_key,channel,trigger_code,category,user_id,conversation_id,campaign_id,to_email,to_phone,
                 sender_profile_key,whatsapp_account_key,subject,body,template_key,certification_id,status,scheduled_at,created_by)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?, ?, ?, ?)",
                string.IsNullOrEmpty(dedupKey) ? null : dedupKey, channel, triggerCode, category, userId, conversationId, campaignId,
                toEmail, toPhone, senderKey, waAccountKey, subject, body, templateKey, certId,
                string.IsNullOrEmpty(scheduledAt) ? "queued" : "scheduled", scheduledAt, createdBy);
        }
        catch { return null; /* unique dedup collision under a race — treat as duplicate */ }
    }

    /// <summary>Record an email the platform ALREADY sent (via Mailer.Send) as a history row in the
    /// communications outbox, so every transactional/automated message appears in the Communications
    /// Centre history + monitoring. This never re-sends — it writes a terminal row with the real status.
    /// Best-effort: any failure here must never break the caller's send.</summary>
    public static void MirrorSent(Db db, long? userId, string? toEmail, string emailType, string subject, string status)
    {
        try
        {
            var mapped = status switch { "sent" => "sent", "console" => "sent", "failed" => "failed", _ => status };
            db.Execute(@"INSERT INTO comm_outbox(dedup_key,channel,trigger_code,category,user_id,to_email,subject,status,provider,attempts,sent_at,created_at)
                VALUES(?, 'email', ?, 'transactional', ?, ?, ?, ?, 'mailer', 1, datetime('now'), datetime('now'))",
                $"mailer:{emailType}:{toEmail}:{DateTime.UtcNow.Ticks}", emailType, userId, toEmail, subject, mapped, "mailer");
        }
        catch { /* history mirror is best-effort */ }
    }

    static bool IsMarketing(string category) => category is "marketing" or "newsletter" or "surveys";

    /// <summary>Fire a platform event through the Communications Centre: reads the trigger's config,
    /// honours the per-channel (email / WhatsApp / in-app) toggles, renders the assigned published template
    /// (falling back to the caller-supplied content when none is set/published), checks marketing consent,
    /// and enqueues one deduped outbox row per enabled channel. The OutboxDispatcher then delivers each.
    /// Safe to call anywhere — if the trigger is missing/inactive it is a no-op, and dedup keys prevent a
    /// double-fire from a retried event. Uses the outbox email path (not Mailer), so it never double-sends
    /// with the Mailer history mirror.</summary>
    public static void Fire(Db db, string triggerCode, long? userId, string? toEmail, string? toPhone,
        IDictionary<string, string?> vars, string fallbackSubject, string fallbackHtml,
        string? waText = null, long? certId = null, string? dedupSuffix = null)
    {
        Dictionary<string, object?>? t;
        try { t = db.QueryOne("SELECT * FROM comm_triggers WHERE code=? AND active=1", triggerCode); }
        catch { return; }
        if (t is null) return;
        var category = H.Str(t["consent_category"]) ?? "transactional";
        var senderKey = H.Str(t["sender_profile_key"]);
        var waKey = H.Str(t["whatsapp_account_key"]);
        if (string.IsNullOrEmpty(toEmail) && userId is not null)
            try { toEmail = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", userId)?["email"]); } catch { }
        var who = userId?.ToString() ?? toEmail ?? toPhone ?? "?";
        var baseKey = $"{triggerCode}:{who}:{dedupSuffix ?? ""}";

        if (H.B(t["email_enabled"]) && !string.IsNullOrEmpty(toEmail))
        {
            var (subj, html) = ResolveEmail(db, H.Str(t["email_template_key"]), vars, fallbackSubject, fallbackHtml);
            Enqueue(db, "email", baseKey + ":email", userId, toEmail, null, subj, html,
                senderKey: senderKey, category: category, triggerCode: triggerCode, certId: certId);
        }
        if (H.B(t["whatsapp_enabled"]) && !string.IsNullOrEmpty(toPhone))
        {
            var body = ResolveText(db, H.Str(t["whatsapp_template_key"]), vars, waText ?? StripHtml(fallbackHtml));
            Enqueue(db, "whatsapp", baseKey + ":wa", userId, null, toPhone, fallbackSubject, body,
                waAccountKey: waKey, category: category, triggerCode: triggerCode, certId: certId);
        }
        if (H.B(t["inapp_enabled"]) && userId is not null)
            Enqueue(db, "inapp", baseKey + ":inapp", userId, null, null, fallbackSubject, StripHtml(fallbackHtml),
                category: category, triggerCode: triggerCode, certId: certId);
    }

    static (string subject, string html) ResolveEmail(Db db, string? templateKey, IDictionary<string, string?> vars, string fbSubject, string fbHtml)
    {
        if (!string.IsNullOrEmpty(templateKey))
            try
            {
                var tpl = db.QueryOne("SELECT subject,body FROM comm_templates WHERE key=? AND kind='email' AND status='published' ORDER BY version DESC LIMIT 1", templateKey);
                if (tpl is not null) return (Render(H.Str(tpl["subject"]) ?? fbSubject, vars), Render(H.Str(tpl["body"]) ?? fbHtml, vars));
            }
            catch { }
        return (Render(fbSubject, vars), Render(fbHtml, vars));
    }
    static string ResolveText(Db db, string? templateKey, IDictionary<string, string?> vars, string fbText)
    {
        if (!string.IsNullOrEmpty(templateKey))
            try
            {
                var tpl = db.QueryOne("SELECT body FROM comm_templates WHERE key=? AND kind='whatsapp' AND status='published' ORDER BY version DESC LIMIT 1", templateKey);
                if (tpl is not null) return Render(H.Str(tpl["body"]) ?? fbText, vars);
            }
            catch { }
        return Render(fbText, vars);
    }
    static string StripHtml(string? html) => string.IsNullOrEmpty(html) ? ""
        : System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", " ").Replace("&nbsp;", " ").Trim();

    static bool HasMarketingConsent(Db db, long userId, string channel)
    {
        var p = db.QueryOne("SELECT email_marketing,whatsapp_marketing FROM comm_preferences WHERE user_id=?", userId);
        if (p is null) return false;
        return H.B(p[channel == "whatsapp" ? "whatsapp_marketing" : "email_marketing"]);
    }

    // ─────────── provider seam (used by the dispatcher) ───────────

    public record SendResult(string Status, string? ProviderMessageId, string? Response, string Provider);

    public static SendResult SendEmail(string from, string to, string? replyTo, string subject, string html)
    {
        var resendKey = E("RESEND_API_KEY");
        var host = E("SMTP_HOST");
        if (!string.IsNullOrEmpty(resendKey))
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", resendKey);
                object payload = string.IsNullOrEmpty(replyTo)
                    ? new { from, to = new[] { to }, subject, html }
                    : new { from, to = new[] { to }, subject, html, reply_to = replyTo };
                req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                using var resp = _http.Send(req);
                using var sr = new StreamReader(resp.Content.ReadAsStream());
                var bodyStr = sr.ReadToEnd();
                string? id = null;
                try { if (JsonDocument.Parse(bodyStr).RootElement.TryGetProperty("id", out var idEl)) id = idEl.GetString(); } catch { }
                return new SendResult(resp.IsSuccessStatusCode ? "sent" : "failed", id, Clip(bodyStr), "resend");
            }
            catch (Exception ex) { return new SendResult("failed", null, ex.Message, "resend"); }
        }
        if (!string.IsNullOrEmpty(host))
        {
            try
            {
                using var client = new System.Net.Mail.SmtpClient(host, int.TryParse(E("SMTP_PORT"), out var p) ? p : 587);
                var user = E("SMTP_USER");
                if (!string.IsNullOrEmpty(user)) client.Credentials = new System.Net.NetworkCredential(user, E("SMTP_PASS"));
                client.EnableSsl = !string.Equals(E("SMTP_SSL"), "false", StringComparison.OrdinalIgnoreCase);
                using var msg = new System.Net.Mail.MailMessage(from, to, subject, html) { IsBodyHtml = true };
                if (!string.IsNullOrEmpty(replyTo)) msg.ReplyToList.Add(replyTo);
                client.Send(msg);
                return new SendResult("sent", null, null, "smtp");
            }
            catch (Exception ex) { return new SendResult("failed", null, ex.Message, "smtp"); }
        }
        // No provider configured — console sink so history still records the intent (dev / not-yet-wired).
        Console.WriteLine($"[comm→console:email] to={to} from={from} subject=\"{subject}\"");
        return new SendResult("console", null, "No email provider configured (RESEND_API_KEY / SMTP_HOST).", "console");
    }

    /// <summary>WhatsApp Cloud API (Meta). Token from the account's env var, phone_number_id from the account
    /// row. When not configured, returns 'console' so nothing crashes and history records the intent.</summary>
    public static SendResult SendWhatsApp(string? phoneNumberId, string? tokenEnv, string toPhone, string body, string? waTemplateName, string? lang)
    {
        var token = E(tokenEnv ?? "WHATSAPP_ACCESS_TOKEN");
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(phoneNumberId))
        {
            Console.WriteLine($"[comm→console:whatsapp] to={toPhone} body=\"{Clip(body)}\"");
            return new SendResult("console", null, "WhatsApp not configured (access token / phone_number_id).", "console");
        }
        try
        {
            var digits = new string(toPhone.Where(char.IsDigit).ToArray());
            object payload = string.IsNullOrEmpty(waTemplateName)
                ? new { messaging_product = "whatsapp", to = digits, type = "text", text = new { body } }
                : new { messaging_product = "whatsapp", to = digits, type = "template",
                        template = new { name = waTemplateName, language = new { code = string.IsNullOrEmpty(lang) ? "en" : lang } } };
            using var req = new HttpRequestMessage(HttpMethod.Post, $"https://graph.facebook.com/v20.0/{phoneNumberId}/messages");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = _http.Send(req);
            using var sr = new StreamReader(resp.Content.ReadAsStream());
            var bodyStr = sr.ReadToEnd();
            string? id = null;
            try { var root = JsonDocument.Parse(bodyStr).RootElement;
                  if (root.TryGetProperty("messages", out var ms) && ms.ValueKind == JsonValueKind.Array && ms.GetArrayLength() > 0
                      && ms[0].TryGetProperty("id", out var idEl)) id = idEl.GetString(); } catch { }
            return new SendResult(resp.IsSuccessStatusCode ? "sent" : "failed", id, Clip(bodyStr), "meta_cloud");
        }
        catch (Exception ex) { return new SendResult("failed", null, ex.Message, "meta_cloud"); }
    }

    static string Clip(string? s, int max = 1000) => string.IsNullOrEmpty(s) ? "" : (s.Length > max ? s[..max] : s);

    // ─────────── one-click unsubscribe tokens ───────────
    static string UnsubSecret =>
        (Environment.GetEnvironmentVariable("UNSUBSCRIBE_SECRET")
         ?? Environment.GetEnvironmentVariable("CREDENTIAL_ENCRYPTION_KEY")
         ?? "pci-unsubscribe-v1") + "|pci-unsub";

    /// <summary>A signed, self-verifying unsubscribe token for a user (safe to embed in an email link).</summary>
    public static string UnsubToken(long userId) => $"{userId}.{Security.Sha($"{userId}.{UnsubSecret}")[..24]}";

    /// <summary>Verify an unsubscribe token; returns the userId or null.</summary>
    public static long? VerifyUnsub(string? token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        var p = token.Split('.');
        if (p.Length != 2 || !long.TryParse(p[0], out var uid)) return null;
        return Security.FixedTimeEquals(p[1], Security.Sha($"{uid}.{UnsubSecret}")[..24]) ? uid : null;
    }
}
