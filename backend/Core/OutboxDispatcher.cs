using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Background worker that drains the Communications outbox (comm_outbox). Every ~15 seconds it claims a
/// batch of due rows (queued/retrying, past any schedule and backoff), renders + sends each through the
/// per-channel provider seam, and records the outcome + a delivery-attempt row. Failures are retried with
/// exponential backoff up to max_attempts, then marked failed. Nothing throws out of the loop, so one bad
/// message can never stop the worker, and a provider outage leaves rows queued (never lost).
/// </summary>
public sealed class OutboxDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(15);
    public OutboxDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { DrainOnce(_db, 25); }
            catch (Exception e) { Console.Error.WriteLine($"[comms] outbox drain failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    /// <summary>Process up to <paramref name="limit"/> due rows. Also callable directly by an admin retry.</summary>
    public static void DrainOnce(Db db, int limit)
    {
        // Recover crash-stranded processing rows whose lease expired, then claim each due row atomically
        // so two dispatcher instances cannot both send the same message (EXT-P0-05).
        try { WorkerLease.RecoverExpired(db, "comm_outbox", "retrying"); } catch { }
        var rows = db.Query(@"SELECT id FROM comm_outbox
            WHERE status IN ('queued','scheduled','retrying')
              AND (scheduled_at IS NULL OR scheduled_at<=datetime('now'))
              AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))
              AND (lease_until IS NULL OR lease_until<=datetime('now'))
            ORDER BY id LIMIT ?", limit);
        var owner = WorkerLease.NewOwner();
        foreach (var r in rows)
        {
            var id = H.L(r["id"]);
            if (!WorkerLease.TryClaim(db, "comm_outbox", id, owner, "'queued','scheduled','retrying'")) continue;
            var full = db.QueryOne("SELECT * FROM comm_outbox WHERE id=?", id);
            if (full is null) continue;
            try { DeliverOne(db, full); }
            catch (Exception e)
            {
                WorkerLease.Clear(db, "comm_outbox", id);
                db.Execute("UPDATE comm_outbox SET status='failed', last_error=?, updated_at=datetime('now') WHERE id=?", e.Message, id);
            }
        }
    }

    static void DeliverOne(Db db, Dictionary<string, object?> r)
    {
        var id = H.L(r["id"]);
        // Status already 'processing' via WorkerLease.TryClaim — do not re-claim here.
        var channel = H.Str(r["channel"]) ?? "email";
        var attempt = (int)H.L(r["attempts"]) + 1;
        var maxAttempts = (int)Math.Max(1, H.L(r["max_attempts"]));

        Comms.SendResult res;
        if (channel == "inapp")
        {
            // In-app: write a notification row for the recipient (no external provider).
            if (r["user_id"] is not null)
                db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Message', ?, ?)",
                    r["user_id"], H.Str(r["subject"]) ?? "Notification", H.Str(r["body"]) ?? "");
            res = new Comms.SendResult(r["user_id"] is null ? "failed" : "delivered", null, null, "inapp");
        }
        else if (channel == "whatsapp")
        {
            var acct = db.QueryOne("SELECT * FROM comm_whatsapp_accounts WHERE `key`=? OR is_default=1 ORDER BY (`key`=?) DESC LIMIT 1",
                H.Str(r["whatsapp_account_key"]) ?? "", H.Str(r["whatsapp_account_key"]) ?? "");
            res = Comms.SendWhatsApp(H.Str(acct?["provider_account_id"]), H.Str(acct?["token_env"]),
                H.Str(r["to_phone"]) ?? "", H.Str(r["body"]) ?? "", null, "en");
        }
        else
        {
            var prof = db.QueryOne("SELECT * FROM comm_sender_profiles WHERE `key`=? AND active=1", H.Str(r["sender_profile_key"]) ?? "")
                       ?? db.QueryOne("SELECT * FROM comm_sender_profiles WHERE is_default=1 AND active=1 LIMIT 1");
            var fromName = H.Str(prof?["display_name"]);
            var fromEmail = H.Str(prof?["from_email"]) ?? "no-reply@projectcontrolsinstitute.org";
            var from = string.IsNullOrEmpty(fromName) ? fromEmail : $"{fromName} <{fromEmail}>";
            // Block production sending from an unverified/unapproved profile (console still records intent).
            if (prof is not null && !H.B(prof["domain_verified"]) && Environment.GetEnvironmentVariable("RESEND_API_KEY") is { Length: > 0 })
            {
                res = new Comms.SendResult("rejected", null, "Sender profile domain not verified.", "policy");
            }
            else
            {
                res = Comms.SendEmail(from, H.Str(r["to_email"]) ?? "", H.Str(prof?["reply_to"]),
                    H.Str(r["subject"]) ?? "", H.Str(r["body"]) ?? "");
            }
        }

        db.Execute("INSERT INTO comm_delivery_attempts(outbox_id,attempt,status,detail) VALUES(?,?,?,?)",
            id, attempt, res.Status, Trim(res.Response));

        // 'console' counts as delivered-for-history (no provider wired); 'sent'/'delivered' are terminal-ok.
        var ok = res.Status is "sent" or "delivered" or "console";
        if (ok)
        {
            db.Execute(@"UPDATE comm_outbox SET status=?, attempts=?, provider=?, provider_message_id=?,
                provider_response=?, sent_at=datetime('now'), last_error=NULL, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?",
                res.Status == "console" ? "sent" : "sent", attempt, res.Provider, res.ProviderMessageId, Trim(res.Response), id);
        }
        else if (res.Status is "rejected" or "hard_bounced" or "unsubscribed")
        {
            db.Execute("UPDATE comm_outbox SET status=?, attempts=?, provider=?, last_error=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?",
                res.Status, attempt, res.Provider, Trim(res.Response), id);
        }
        else if (attempt >= maxAttempts)
        {
            db.Execute("UPDATE comm_outbox SET status='failed', attempts=?, provider=?, last_error=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?",
                attempt, res.Provider, Trim(res.Response), id);
        }
        else
        {
            var backoffMin = Math.Min(60, (int)Math.Pow(2, attempt)); // 2,4,8,16,32,60 min
            // The next-attempt instant is computed here rather than as datetime('now','+' || ? || ' minutes'):
            // that expression is SQLite-only, and because DrainOnce dead-letters a row whose delivery throws,
            // its failure on MySQL turned every FIRST transient failure into a permanent one.
            db.Execute(@"UPDATE comm_outbox SET status='retrying', attempts=?, provider=?, last_error=?,
                next_attempt_at=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?",
                attempt, res.Provider, Trim(res.Response), H.StampInMinutes(backoffMin), id);
        }
    }

    static string? Trim(string? s) => string.IsNullOrEmpty(s) ? null : (s.Length > 2000 ? s[..2000] : s);

    private static async Task<bool> WaitAsync(PeriodicTimer timer, CancellationToken ct)
    {
        try { return await timer.WaitForNextTickAsync(ct); }
        catch (OperationCanceledException) { return false; }
    }
}
