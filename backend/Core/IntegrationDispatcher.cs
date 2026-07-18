using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Background delivery worker for the integrations outbox (Phase 9). Every ~20 seconds it drains due
/// deliveries (pending, past their backoff) and POSTs each to its connector's endpoint, signed. Uses
/// the same <see cref="Integrations.DeliverOne"/> path as the admin test/retry actions, so there is one
/// delivery code path. Failures are retried with exponential backoff up to the attempt cap; nothing here
/// throws out of the loop, so a bad connector can never stop the worker.
/// </summary>
public sealed class IntegrationDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);
    // Egress-guarded: connector endpoints are admin-supplied, so deliveries must never reach
    // loopback/private/metadata addresses (see Core/Egress.cs; INTEGRATIONS_ALLOW_PRIVATE_EGRESS opts out).
    private static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(15));

    public IntegrationDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { await Integrations.DeliverDue(_db, Http, 25); }
            catch (Exception e) { Console.Error.WriteLine($"[integrations] dispatch failed: {e.Message}"); }
            // Certuvo provisioning retries ride the same loop: failed accounts whose backoff has
            // elapsed are re-attempted here, so a transient Certuvo outage heals without operator action.
            try { await CertuvoLink.RetryDue(_db, CertuvoLink.Http, 5); }
            catch (Exception e) { Console.Error.WriteLine($"[certuvo] retry sweep failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    private static async Task<bool> WaitAsync(PeriodicTimer timer, CancellationToken ct)
    {
        try { return await timer.WaitForNextTickAsync(ct); }
        catch (OperationCanceledException) { return false; }
    }
}
