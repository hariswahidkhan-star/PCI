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

    /// <summary>Public base URL for links in emails: APP_BASE_URL/SITE_BASE_URL, else the
    /// origin of the triggering request (correct for single-service deployments).</summary>
    public static string BaseUrl(HttpRequest? req = null)
    {
        var b = E("APP_BASE_URL") ?? E("SITE_BASE_URL");
        if (!string.IsNullOrWhiteSpace(b)) return b.TrimEnd('/');
        if (req is not null) return $"{req.Scheme}://{req.Host}";
        return "";
    }

    /// <summary>The link a candidate opens to set (or reset) their password.</summary>
    public static string SetupLink(string baseUrl, string plaintextToken)
        => $"{baseUrl}/reset-password.html?token={plaintextToken}";

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
            var raw = k == "BODY" || k.EndsWith("_URL", StringComparison.OrdinalIgnoreCase) || k.EndsWith("_HTML", StringComparison.OrdinalIgnoreCase);
            var val = raw ? v : System.Net.WebUtility.HtmlEncode(v);
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
                var from = E("MAIL_FROM") ?? E("SMTP_FROM") ?? "PCI Global <onboarding@resend.dev>";
                using var reqMsg = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
                reqMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", resendKey);
                reqMsg.Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new { from, to = new[] { to }, subject, html }),
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
                var user = E("SMTP_USER");
                if (!string.IsNullOrEmpty(user)) client.Credentials = new System.Net.NetworkCredential(user, E("SMTP_PASS"));
                client.EnableSsl = !string.Equals(E("SMTP_SSL"), "false", StringComparison.OrdinalIgnoreCase);
                using var msg = new System.Net.Mail.MailMessage(from, to, subject, html) { IsBodyHtml = true };
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
                new Dictionary<string, string?> { ["student_name"] = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!, ["portal_link"] = baseUrl + "/app/" },
                "Welcome to PCI", "<p>Welcome to the Project Controls Institute. Your account is ready — sign in to get started.</p>",
                skipEmail: true);
        }
        catch { }
    }

    static IEnumerable<string> ExtractLinks(string html)
    {
        foreach (System.Text.RegularExpressions.Match m in
                 System.Text.RegularExpressions.Regex.Matches(html, "href=\"(http[^\"]+)\""))
            yield return m.Groups[1].Value;
    }
}
