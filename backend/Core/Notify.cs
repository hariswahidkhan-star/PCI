using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Reusable notification service. Today it delivers by EMAIL (through <see cref="Mailer"/>, which is
/// itself provider-agnostic — Resend / SMTP / console). SMS and in-app are deliberate seams: add a
/// branch to a future Send(channel,…) without touching callers. Every attempt is recorded in
/// notification_history for auditability. Recipients, the admin alert address, and per-event enable
/// flags are read from site_settings, so nothing (addresses, SMTP, on/off) is hardcoded.
/// </summary>
public static class Notify
{
    public enum Channel { Email, Sms, InApp }

    /// <summary>Configured admin/board recipient for operational alerts. Falls back through a chain of
    /// existing contact settings so a fresh install still reaches someone; empty ⇒ no admin alert sent.</summary>
    public static string AdminEmail(Db db)
    {
        foreach (var k in new[] { "notify_admin_email", "contact_email", "web_contact_email", "org_email" })
        {
            var v = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k);
            if (!string.IsNullOrWhiteSpace(v)) return v.Trim();
        }
        return "";
    }

    /// <summary>Per-event on/off switch, e.g. Enabled(db, "honorary") reads notify_honorary_enabled.</summary>
    public static bool Enabled(Db db, string eventKey) => Settings.Bool(db, "notify_" + eventKey + "_enabled", true);

    static readonly System.Text.RegularExpressions.Regex EmailRx =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>The full set of operational-alert recipients: the multi-address <c>notify_recipients</c>
    /// list (comma / semicolon / newline separated) merged with the single <see cref="AdminEmail"/>.
    /// De-duplicated case-insensitively, validated, capped. This is how an owner "adds their email" and
    /// how work is fanned out to assignees — put every address that should hear about events here.</summary>
    public static List<string> Recipients(Db db, IEnumerable<string>? extra = null)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var outp = new List<string>();
        void Add(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return;
            foreach (var part in raw.Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var e = part.Trim();
                if (EmailRx.IsMatch(e) && seen.Add(e) && outp.Count < 25) outp.Add(e);
            }
        }
        Add(db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", "notify_recipients"));
        Add(AdminEmail(db));
        if (extra is not null) foreach (var x in extra) Add(x);
        return outp;
    }

    /// <summary>Fan an operational alert out to every configured recipient (plus any per-item assignees).
    /// Honours the per-event enable flag, records each attempt, and NEVER throws — a mail failure must not
    /// break the triggering request. Returns how many messages were dispatched (0 if disabled / no recipients).</summary>
    public static int Alert(Db db, string eventKey, string subject, string html, string relatedType, long? relatedId, IEnumerable<string>? assignees = null)
    {
        if (!Enabled(db, eventKey)) return 0;
        var recips = Recipients(db, assignees);
        var sent = 0;
        foreach (var to in recips)
        {
            try { Mailer.Send(db, null, to, "notification", subject, html); Record(db, "email", to, subject, "sent", relatedType, relatedId); sent++; }
            catch { Record(db, "email", to, subject, "failed", relatedType, relatedId); }
        }
        if (recips.Count == 0) Record(db, "email", null, subject, "skipped", relatedType, relatedId);
        return sent;
    }

    /// <summary>Deliver an email notification and record it. Never throws — a mail failure must not break
    /// the triggering request. Returns the recorded status (sent | failed | skipped).</summary>
    public static string Email(Db db, long? userId, string? to, string subject, string html, string relatedType, long? relatedId)
    {
        string status;
        if (string.IsNullOrWhiteSpace(to)) status = "skipped";
        else { try { Mailer.Send(db, userId, to!, "notification", subject, html); status = "sent"; } catch { status = "failed"; } }
        Record(db, "email", to, subject, status, relatedType, relatedId);
        return status;
    }

    /// <summary>Append one row to the notification ledger. Best-effort; never breaks the caller.</summary>
    public static void Record(Db db, string channel, string? recipient, string? subject, string status, string relatedType, long? relatedId)
    {
        try
        {
            db.Execute("INSERT INTO notification_history(channel,recipient,subject,status,related_type,related_id) VALUES(?,?,?,?,?,?)",
                channel, recipient, subject, status, relatedType, relatedId);
        }
        catch { /* history is auxiliary — swallow */ }
    }
}
