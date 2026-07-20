using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Scheduled reminder sequences (Communications Centre §13). A daily sweep fires time-based reminders
/// through Comms.Fire, so they honour the same per-trigger channel toggles, consent gating and dedup as
/// every other event. Each reminder carries a dedup key tied to the thing being reminded about (e.g. the
/// membership expiry date), so the daily cadence can never send the same reminder twice.
///
/// Deliberately conservative: only conditions with an unambiguous data signal are swept, and the trigger
/// must be active (an admin can switch any reminder off in the Triggers tab). Nothing throws out of the
/// loop; a bad row can't stop the sweep.
/// </summary>
public sealed class CommsReminderService : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);
    public CommsReminderService(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Small startup delay so the first sweep doesn't race boot-time migrations/seeds.
        try { await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken); } catch { return; }
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { SweepOnce(_db); }
            catch (Exception e) { Console.Error.WriteLine($"[comms-reminders] sweep failed: {e.Message}"); }
        }
        while (await SafeWait(timer, stoppingToken));
    }
    static async Task<bool> SafeWait(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    /// <summary>Run every reminder rule once. Also callable directly (admin "run reminders now").</summary>
    public static int SweepOnce(Db db)
    {
        var n = 0;
        n += MembershipExpiry(db);
        return n;
    }

    // Members whose active membership expires within the next 30 days — one reminder per expiry date.
    static int MembershipExpiry(Db db)
    {
        var rows = db.Query(@"SELECT m.user_id, m.expiry_date, u.email, u.first_name
            FROM memberships m JOIN users u ON u.id=m.user_id
            WHERE m.status='active' AND m.expiry_date IS NOT NULL
              AND m.expiry_date <= datetime('now','+30 day') AND m.expiry_date > datetime('now')
              AND u.status='active'");
        var n = 0;
        foreach (var r in rows)
        {
            var uid = H.L(r["user_id"]);
            var expiry = H.Str(r["expiry_date"]) ?? "";
            var when = expiry.Length >= 10 ? expiry[..10] : expiry;
            try
            {
                Comms.Fire(db, "membership.expiry_reminder", uid, H.Str(r["email"]), null,
                    new Dictionary<string, string?> { ["student_name"] = H.Str(r["first_name"]) ?? "there", ["expiry_date"] = when, ["portal_link"] = "/app/billing" },
                    "Your PCI membership is expiring soon",
                    $"<p>Your PCI membership expires on {when}. Renew from your portal to keep your benefits without interruption.</p>",
                    dedupSuffix: $"expiry:{uid}:{when}");
                n++;
            }
            catch { }
        }
        return n;
    }
}
