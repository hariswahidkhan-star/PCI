using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// MySQL-8-safe / SQLite-safe atomic claiming for background workers (EXT-P0-05).
/// Workers must not SELECT-then-UPDATE separately: two instances can both select the same due
/// row and duplicate outbound provider actions. Instead, claim with a single conditional UPDATE that
/// only succeeds for one winner (rows-affected &gt; 0). Lease expiry recovers stranded work after a crash.
/// </summary>
public static class WorkerLease
{
    /// <summary>Stable-ish owner id for this process (hostname + short guid). Persisted on claimed rows.</summary>
    public static string NewOwner()
    {
        var host = Environment.MachineName;
        if (string.IsNullOrWhiteSpace(host)) host = "pci";
        if (host.Length > 40) host = host[..40];
        return host + ":" + Guid.NewGuid().ToString("N")[..12];
    }

    /// <summary>
    /// Atomically claim row <paramref name="id"/> in <paramref name="table"/> when it is still in a
    /// claimable status and any prior lease has expired (or never existed). On success the row is marked
    /// <c>processing</c> with a fresh lease. Returns true only for the winning worker.
    /// </summary>
    public static bool TryClaim(Db db, string table, long id, string owner,
        string claimableInSql = "'queued','scheduled','retrying','pending'",
        int leaseMinutes = 5, string? setStatus = "processing")
    {
        leaseMinutes = Math.Clamp(leaseMinutes, 1, 60);
        // Table/column names are caller-controlled constants from our own dispatchers — never user input.
        var statusSet = setStatus is null ? "" : "status='" + setStatus.Replace("'", "") + "', ";
        var (_, changes) = db.ExecuteWithChanges(
            $"UPDATE {table} SET {statusSet}lease_owner=?, lease_until=datetime('now','+{leaseMinutes} minutes'), updated_at=datetime('now') " +
            $"WHERE id=? AND status IN ({claimableInSql}) AND (lease_until IS NULL OR lease_until<=datetime('now'))",
            owner, id);
        return changes > 0;
    }

    /// <summary>Clear the lease after terminal success/failure (or before re-queueing as retrying).</summary>
    public static void Clear(Db db, string table, long id) =>
        db.Execute($"UPDATE {table} SET lease_owner=NULL, lease_until=NULL WHERE id=?", id);

    /// <summary>Recover rows stuck in <c>processing</c> whose lease expired (worker crash / deploy).</summary>
    public static int RecoverExpired(Db db, string table, string requeueStatus = "retrying")
    {
        return db.Execute(
            $"UPDATE {table} SET status=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') " +
            $"WHERE status='processing' AND lease_until IS NOT NULL AND lease_until<=datetime('now')",
            requeueStatus);
    }
}
