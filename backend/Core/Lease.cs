using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Atomic claim/lease for outbox-style background work (P0-5). A worker takes ownership of one row with a
/// single conditional UPDATE: a claimable (or crashed/expired-lease) row is transitioned to 'processing'
/// under this owner atomically, so when several worker instances drain the same queue concurrently only ONE
/// wins the row (<see cref="Claim"/> returns true exactly once); the losers skip it. A crashed worker leaves
/// its row 'processing' with a past <c>lease_expires_at</c>, which a later drain reclaims. Combined with the
/// existing per-row idempotency (unique keys / de-dupe) this makes duplicate, concurrent and retry deliveries
/// safe. Table names, statuses and the lease duration are compile-time constants — never request input — so
/// inlining them carries no injection risk.
/// </summary>
public static class Lease
{
    /// <summary>A stable-per-process, unique-per-claim owner tag (host:pid:rand) for the ledger/audit.</summary>
    public static string NewOwner() => $"{Environment.MachineName}:{Environment.ProcessId}:{Guid.NewGuid():N}"[..40];

    /// <summary>Atomically claim row <paramref name="id"/> in <paramref name="table"/>: flip a claimable row
    /// (or a crashed row whose lease has expired) to 'processing' owned by <paramref name="owner"/>. Returns
    /// true iff this caller won the row (exactly one concurrent caller can).</summary>
    public static bool Claim(Db db, string table, long id, string owner, string[] claimableStatuses, int leaseSeconds = 300)
    {
        var inList = string.Join(",", claimableStatuses.Select(s => "'" + s + "'"));
        var (_, changes) = db.ExecuteWithChanges(
            $"UPDATE {table} SET status='processing', lease_owner=?, lease_expires_at=datetime('now','+{leaseSeconds} seconds'), updated_at=datetime('now') " +
            $"WHERE id=? AND (status IN ({inList}) OR (status='processing' AND lease_expires_at IS NOT NULL AND lease_expires_at<datetime('now')))",
            owner, id);
        return changes == 1;
    }

    /// <summary>Idempotently add the lease bookkeeping columns to the outbox tables (existing databases).
    /// Fresh schemas may already carry them; a missing table is skipped. Safe to run on every boot.</summary>
    public static void EnsureLeaseColumns(Db db)
    {
        foreach (var t in new[] { "comm_outbox", "integration_deliveries", "mkt_jobs" })
        {
            try
            {
                var cols = db.Columns(t);
                if (cols.Count == 0) continue;   // table not created (yet) — nothing to alter
                if (!cols.Contains("lease_owner")) db.Exec($"ALTER TABLE {t} ADD COLUMN lease_owner TEXT");
                if (!cols.Contains("lease_expires_at")) db.Exec($"ALTER TABLE {t} ADD COLUMN lease_expires_at TEXT");
            }
            catch (Exception e) { Console.Error.WriteLine($"[lease] ensure columns on {t}: {e.Message}"); }
        }
    }
}
