using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;

namespace PCI.Backend.Core;

/// <summary>
/// Background worker that turns a SCHEDULED blog post live at its appointed time. Every ~60s it finds posts
/// with status='scheduled' whose scheduled_at has passed and publishes each through the SAME
/// ContentCentre.PublishPost path the admin /publish endpoint uses (version snapshot, sitemap bump, IndexNow
/// ping, staff notice) — so a scheduled publish is byte-for-byte a normal publish, just fired by the system.
/// Separation of duties was enforced when the human scheduled the post (a person cannot schedule their own
/// article), so the worker publishes as a system actor (adminId=null). Failures are logged and retried on the
/// next tick; a single bad post never blocks the others. Without this worker a scheduled post would sit at
/// status='scheduled' forever.
/// </summary>
public sealed class ScheduledPublisher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(60);
    public ScheduledPublisher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { PublishDue(_db); }
            catch (Exception e) { Console.Error.WriteLine($"[schedule] publish sweep failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    /// <summary>
    /// Publish every post whose scheduled time has arrived (up to a batch cap). Also callable directly by an
    /// admin "run now" or the test harness. Returns the number of posts published.
    /// </summary>
    public static int PublishDue(Db db)
    {
        // System-actor audit line (no HttpContext here) — mirrors the app-wide logFn shape.
        Action<long?, string, string?> log = (uid, action, details) =>
        { try { db.Execute("INSERT INTO audit_logs(user_id,action,details) VALUES(?,?,?)", uid, action, details ?? ""); } catch { } };

        var due = db.Query(@"SELECT id FROM blog_posts WHERE status='scheduled'
            AND scheduled_at IS NOT NULL AND scheduled_at<=datetime('now') ORDER BY scheduled_at, id LIMIT 50");
        int done = 0;
        foreach (var r in due)
        {
            var id = H.L(r["id"]);
            try
            {
                var (ok, error, _) = ContentCentre.PublishPost(db, id, null, log);
                if (ok) done++;
                else Console.Error.WriteLine($"[schedule] post {id} not published: {error}");
            }
            catch (Exception e) { Console.Error.WriteLine($"[schedule] post {id} publish failed: {e.Message}"); }
        }
        return done;
    }
}
