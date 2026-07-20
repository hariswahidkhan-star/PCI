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
        n += ExamSchedulingDeadline(db);
        return n;
    }

    // Candidates with a paid/waived exam whose schedule-by deadline is within 7 days and who have not yet
    // booked — one reminder per (payment, deadline). A deadline extension changes the key, so an extended
    // deadline correctly earns a fresh reminder later.
    static int ExamSchedulingDeadline(Db db)
    {
        var rows = db.Query(@"SELECT p.user_id, p.exam_schedule_deadline, p.id AS payment_id, u.email, u.first_name
            FROM payments p JOIN users u ON u.id=p.user_id
            WHERE p.product_type IN ('exam','bundle') AND p.payment_status IN ('paid','waived')
              AND p.exam_schedule_deadline IS NOT NULL
              AND p.exam_schedule_deadline <= datetime('now','+7 day') AND p.exam_schedule_deadline > datetime('now')
              AND u.status='active'
              AND NOT EXISTS (SELECT 1 FROM exam_bookings b WHERE b.payment_id=p.id AND b.status IN ('scheduled','completed'))");
        var n = 0;
        foreach (var r in rows)
        {
            var uid = H.L(r["user_id"]);
            var dl = H.Str(r["exam_schedule_deadline"]) ?? "";
            var when = dl.Length >= 10 ? dl[..10] : dl;
            try
            {
                Comms.Fire(db, "exam.scheduling_deadline_approaching", uid, H.Str(r["email"]), null,
                    new Dictionary<string, string?> { ["student_name"] = H.Str(r["first_name"]) ?? "there", ["deadline"] = when, ["portal_link"] = "/app/" },
                    "Schedule your PCI exam soon",
                    $"<p>Your exam must be scheduled by {when}. Sign in to your portal to book your sitting before the deadline.</p>",
                    dedupSuffix: $"sched:{H.L(r["payment_id"])}:{when}");
                n++;
            }
            catch { }
        }
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
