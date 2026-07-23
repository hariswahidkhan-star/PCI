using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Background worker that drives exam-delivery orders (EXT-P0-04/05): claims pending/failed orders with
/// an atomic lease and runs the candidate→authorize→schedule pipeline. Provisioning is never inlined
/// on the student booking request, so a vendor timeout cannot leave an unretriable orphan.
/// </summary>
public sealed class ExamDeliveryDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);
    public ExamDeliveryDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { await ExamDelivery.DrainDue(_db, ExamDelivery.Http, 10); }
            catch (Exception e) { Console.Error.WriteLine($"[exam-delivery] drain failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }
}
