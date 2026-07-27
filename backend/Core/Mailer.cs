using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The platform's one email sender, in provider precedence order:
///   1. RESEND_API_KEY — Resend's HTTPS API (one env var; MAIL_FROM/SMTP_FROM sets the sender,
///      which must be a verified domain on the Resend account, or their onboarding sender for tests);
///   2. SMTP_HOST — classic SMTP (SMTP_PORT/SMTP_USER/SMTP_PASS/SMTP_FROM/SMTP_SSL);
///   3. neither — prints the complete message to the console, exactly as the boot banner promises.
/// Every attempt is recorded in email_logs (status: sent | console | failed) so the admin
/// Email-log section reflects reality.
///
/// This class existed in the Node original but was never ported: tokens for welcome/setup and
/// password-reset links were being minted and then silently lost, so a paying customer could
/// never receive the link that lets them set a password and log in.
/// </summary>
public static class Mailer
{
    static string? E(string k) => Environment.GetEnvironmentVariable(k);
    // one shared client (socket hygiene); auth header goes per-request so a key rotation applies live
    static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };

    /// <summary>Resend's shared test sender. It delivers only to the account owner's own address, so a
    /// deployment silently left on it looks configured while no candidate ever receives mail.</summary>
    public const string ResendTestSender = "PCI Global <onboarding@resend.dev>";

    /// <summary>The configured sender, exactly as the send path resolves it — including the fallback
    /// each provider would use. Surfaced by system-check so an operator can see what recipients see.</summary>
    public static string ResolvedSender()
    {
        if (!string.IsNullOrWhiteSpace(E("RESEND_API_KEY")))
            return E("MAIL_FROM") ?? E("SMTP_FROM") ?? ResendTestSender;
        return E("SMTP_FROM") ?? E("SMTP_USER") ?? "no-reply@projectcontrolsinstitute.org";
    }

    /// <summary>Which provider a send would actually use (precedence: Resend, then SMTP, then console).</summary>
    public static string ActiveProvider() =>
        !string.IsNullOrWhiteSpace(E("RESEND_API_KEY")) ? "resend"
        : !string.IsNullOrWhiteSpace(E("SMTP_HOST")) ? "smtp"
        : "console";

    /// <summary>Optional Reply-To for transactional mail (MAIL_REPLY_TO). Without it, replies go to the
    /// sender address, which is usually a no-reply mailbox nobody reads.</summary>
    public static string? ReplyTo() => E("MAIL_REPLY_TO") is { Length: > 0 } r ? r.Trim() : null;

    /// <summary>An address is usable if it parses — "Name &lt;a@b.c&gt;" and bare "a@b.c" both do.</summary>
    public static bool SenderParses(string? address)
    {
        if (string.IsNullOrWhiteSpace(address)) return false;
        try { _ = new System.Net.Mail.MailAddress(address); return true; }
        catch { return false; }
    }

    /// <summary>Public base URL for links in emails: APP_BASE_URL/SITE_BASE_URL, else the
    /// origin of the triggering request (correct for single-service deployments).</summary>
    public static string BaseUrl(HttpRequest? req = null)
    {
        var b = E("APP_BASE_URL") ?? E("SITE_BASE_URL");
        if (!string.IsNullOrWhiteSpace(b)) return b.TrimEnd('/');
        if (req is not null) return $"{req.Scheme}://{req.Host}";
        return "";
    }

    /// <summary>
    /// Base URL for links into the STUDENT PANEL. With domain separation configured (RES-013) this is
    /// the portal domain (mypci.org); otherwise it is the ordinary site base, so single-domain
    /// deployments are unchanged. Use this for anything a signed-in student opens; keep
    /// <see cref="BaseUrl"/> for public marketing links.
    /// </summary>
    public static string PortalBaseUrl(HttpRequest? req = null)
        => PortalDomain.Enabled ? PortalDomain.BaseUrl! : BaseUrl(req);

    /// <summary>An absolute link into the student panel (e.g. "/app/billing").</summary>
    public static string PortalLink(string path, HttpRequest? req = null)
    {
        if (!path.StartsWith('/')) path = "/" + path;
        return PortalBaseUrl(req).TrimEnd('/') + path;
    }

    /// <summary>The link a candidate opens to set (or reset) their password. This is a portal
    /// destination: the form posts a credential, so it must live on the portal origin when one is
    /// configured rather than bouncing the student across domains mid-flow.</summary>
    public static string SetupLink(string baseUrl, string plaintextToken)
        => $"{(PortalDomain.Enabled ? PortalDomain.BaseUrl! : baseUrl.TrimEnd('/'))}/reset-password.html?token={plaintextToken}";

    /// <summary>Load emails/&lt;name&gt;.html and substitute {KEY} and {{KEY}} placeholders.
    /// Falls back to a minimal HTML shell if the template file is missing.</summary>
    public static string Template(string name, Dictionary<string, string> vars)
    {
        string html;
        var path = Path.Combine(AppContext.BaseDirectory, "emails", name + ".html");
        if (!File.Exists(path)) path = Path.Combine("emails", name + ".html");
        if (File.Exists(path)) html = File.ReadAllText(path);
        else html = "<html><body>{{BODY}}</body></html>";
        // HTML-encode substituted values so user-controlled text (e.g. a name) can't inject markup or
        // script into the email body. App-constructed URLs and any explicitly-HTML value (_URL / _HTML
        // suffix, or BODY) are substituted raw — encoding is only skipped for values the app controls.
        foreach (var (k, v) in vars)
        {
            // RES-013 — a site-relative student-panel path in an email is a dead link once opened in a
            // mail client, and points at the wrong host once the panel has its own domain. Absolutise
            // /app paths to the portal domain (falls back to the site base when separation is off).
            var value = v is { Length: > 0 } && v.StartsWith("/app", StringComparison.OrdinalIgnoreCase)
                ? PortalLink(v)
                : v;
            var raw = k == "BODY" || k.EndsWith("_URL", StringComparison.OrdinalIgnoreCase) || k.EndsWith("_HTML", StringComparison.OrdinalIgnoreCase);
            var val = raw ? value : System.Net.WebUtility.HtmlEncode(value);
            html = html.Replace("{{" + k + "}}", val).Replace("{" + k + "}", val);
        }
        return html;
    }

    /// <summary>Send (or console-print) an email and record it in email_logs. Never throws —
    /// a mail failure must not break the transaction that triggered it (payment settlement,
    /// password reset); the failure is visible in email_logs and the server log instead.</summary>
    public static void Send(Db db, long? userId, string to, string emailType, string subject, string html)
    {
        var host = E("SMTP_HOST");
        var resendKey = E("RESEND_API_KEY");
        string status;
        if (!string.IsNullOrWhiteSpace(resendKey))
        {
            // Resend HTTPS API: the simplest production path (no SMTP ports/credentials).
            try
            {
                var from = E("MAIL_FROM") ?? E("SMTP_FROM") ?? ResendTestSender;
                using var reqMsg = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
                reqMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", resendKey);
                var payload = new Dictionary<string, object?> { ["from"] = from, ["to"] = new[] { to }, ["subject"] = subject, ["html"] = html };
                if (ReplyTo() is { } rt) payload["reply_to"] = rt;
                reqMsg.Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8, "application/json");
                using var resp = _http.Send(reqMsg);
                status = resp.IsSuccessStatusCode ? "sent" : "failed";
                if (!resp.IsSuccessStatusCode)
                {
                    using var body = new StreamReader(resp.Content.ReadAsStream());
                    Console.Error.WriteLine($"[email] resend API {(int)resp.StatusCode} for {to} ({emailType}): {body.ReadToEnd()}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[email] resend send failed to {to} ({emailType}): {ex.Message}");
                status = "failed";
            }
        }
        else if (string.IsNullOrWhiteSpace(host))
        {
            // Console sink (development / SMTP not yet configured) — print enough to act on,
            // including any links, exactly as the boot message promises.
            Console.WriteLine($"[email→console] to={to} type={emailType} subject=\"{subject}\"");
            foreach (var link in ExtractLinks(html)) Console.WriteLine($"[email→console]   link: {link}");
            status = "console";
        }
        else
        {
            try
            {
                var from = E("SMTP_FROM") ?? E("SMTP_USER") ?? "no-reply@projectcontrolsinstitute.org";
                using var client = new System.Net.Mail.SmtpClient(host, int.TryParse(E("SMTP_PORT"), out var p) ? p : 587);
                // Explicit timeout so a hung SMTP peer cannot block a worker forever (EXT-P0-06).
                client.Timeout = int.TryParse(E("SMTP_TIMEOUT_MS"), out var tms) ? Math.Clamp(tms, 3_000, 60_000) : 20_000;
                var user = E("SMTP_USER");
                if (!string.IsNullOrEmpty(user)) client.Credentials = new System.Net.NetworkCredential(user, E("SMTP_PASS"));
                client.EnableSsl = !string.Equals(E("SMTP_SSL"), "false", StringComparison.OrdinalIgnoreCase);
                using var msg = new System.Net.Mail.MailMessage(from, to, subject, html) { IsBodyHtml = true };
                if (ReplyTo() is { } rt) { try { msg.ReplyToList.Add(new System.Net.Mail.MailAddress(rt)); } catch { } }
                client.Send(msg);
                status = "sent";
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[email] send failed to {to} ({emailType}): {ex.Message}");
                status = "failed";
            }
        }
        try { db.Execute("INSERT INTO email_logs(user_id,email,email_type,subject,status) VALUES(?,?,?,?,?)", userId, to, emailType, subject, status); }
        catch { /* the log row must never break the caller */ }
        // Mirror into the Communications Centre so every platform email appears in unified history + monitoring.
        Comms.MirrorSent(db, userId, to, emailType, subject, status);
    }

    /// <summary>Convenience: welcome email with the password-setup link (used by the payment
    /// webhook, admin member creation and resend-setup).</summary>
    public static void SendWelcome(Db db, long userId, string to, string? firstName, string setupUrl, string baseUrl)
    {
        var html = Template("welcome", new()
        {
            ["FIRST_NAME"] = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!,
            ["LOGIN_URL"] = setupUrl,
            ["DOWNLOADS_URL"] = baseUrl + "/downloads.html",
        });
        Send(db, userId, to, "welcome", "Welcome to PCI — set your password", html);
        // Additional channels (WhatsApp + in-app) for the welcome event via the Communications Centre,
        // governed by the account.welcome trigger toggles. Email is sent above (and mirrored), so skip it.
        try
        {
            Comms.Fire(db, "account.welcome", userId, to, null,
                new Dictionary<string, string?> { ["student_name"] = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!, ["portal_link"] = PortalLink("/app/") },
                "Welcome to PCI", "<p>Welcome to the Project Controls Institute. Your account is ready — sign in to get started.</p>",
                skipEmail: true);
        }
        catch { }
    }

    /// <summary>Enqueue the critical welcome/setup email on the durable Communications outbox
    /// (EXT-P0-06). Prefer this inside payment settlement transactions: delivery is retried with
    /// backoff by <see cref="OutboxDispatcher"/>, so a Resend/SMTP outage cannot lose credentials.</summary>
    public static long? EnqueueWelcome(Db db, long userId, string to, string? firstName, string setupUrl, string baseUrl, string? dedupSuffix = null)
    {
        var html = Template("welcome", new()
        {
            ["FIRST_NAME"] = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!,
            ["LOGIN_URL"] = setupUrl,
            ["DOWNLOADS_URL"] = baseUrl + "/downloads.html",
        });
        var dedup = "welcome:" + userId + ":" + (dedupSuffix ?? to);
        var id = Comms.Enqueue(db, "email", dedup, userId, to, null,
            "Welcome to PCI — set your password", html,
            category: "transactional", triggerCode: "account.welcome");
        try
        {
            Comms.Fire(db, "account.welcome", userId, to, null,
                new Dictionary<string, string?> { ["student_name"] = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!, ["portal_link"] = PortalLink("/app/") },
                "Welcome to PCI", "<p>Welcome to the Project Controls Institute. Your account is ready — sign in to get started.</p>",
                skipEmail: true, dedupSuffix: dedupSuffix);
        }
        catch { }
        return id;
    }

    static IEnumerable<string> ExtractLinks(string html)
    {
        foreach (System.Text.RegularExpressions.Match m in
                 System.Text.RegularExpressions.Regex.Matches(html, "href=\"(http[^\"]+)\""))
            yield return m.Groups[1].Value;
    }
}
