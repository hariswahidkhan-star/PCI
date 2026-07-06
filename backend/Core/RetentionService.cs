using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Daily scheduled retention: purges stored binary artefacts (evidence/attachments) older than the
/// configured <c>evidence_retention_days</c>. Metadata rows are kept for audit; only the bytes are removed.
/// This is the scheduled counterpart to the owner-only manual endpoint POST /api/admin/storage/purge —
/// both call the same Storage.PurgeOlderThan, so behaviour is identical whether triggered by hand or cron.
/// A no-op for non-local providers until object storage is wired.
/// </summary>
public sealed class RetentionService : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    public RetentionService(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Run once shortly after boot, then every 24h, until the host stops.
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try
            {
                var days = (int)Settings.Num(_db, "evidence_retention_days", 365);
                var removed = Storage.PurgeOlderThan(days);
                if (removed > 0)
                    Console.WriteLine($"[retention] purged {removed} artefact(s) older than {days}d");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[retention] purge failed: {e.Message}");
            }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    // WaitForNextTickAsync throws OperationCanceledException on shutdown; translate that to a clean stop.
    private static async Task<bool> WaitAsync(PeriodicTimer timer, CancellationToken ct)
    {
        try { return await timer.WaitForNextTickAsync(ct); }
        catch (OperationCanceledException) { return false; }
    }
}
