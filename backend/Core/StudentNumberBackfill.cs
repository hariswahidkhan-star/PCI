using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Phase 2 of the canonical Student Number rollout: find every account that predates transactional
/// issuance, report honestly on what is wrong before touching anything, and then fill the gaps in
/// resumable, idempotent batches.
///
/// The ordering here is the whole point. Uniqueness cannot be enforced until the data is clean, and
/// the data cannot be cleaned until somebody can see what is actually in it — so <see cref="Health"/>
/// and <see cref="Preview"/> are pure reads that must be run and read before <see cref="Run"/> is.
/// Nothing in this file rewrites a number that already exists: a valid historic value is preserved
/// exactly, and a conflicting one is quarantined for a human rather than silently renumbered.
/// </summary>
public static class StudentNumberBackfill
{
    /// <summary>legacy_v1 shape: PCI-{4-digit year}-{at least 6 digits}. Anything else is reported
    /// as malformed and left alone — reporting is not permission to rewrite somebody's identity.</summary>
    static readonly System.Text.RegularExpressions.Regex Legacy =
        new(@"^PCI-\d{4}-\d{6,}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>Accounts that are expected to carry a Student Number: real student accounts that
    /// have not been erased. Admin, service and erased rows are deliberately excluded — issuing a
    /// number to them would create identities that no person owns.</summary>
    const string EligibleWhere = "role='student' AND status<>'erased'";

    /// <summary>
    /// A counts-only integrity report. Never returns personal data — the brief is explicit that a
    /// dry run reports counts, not unredacted dumps of who is affected — so this is safe to expose
    /// on an admin dashboard and safe to paste into a ticket.
    /// </summary>
    public static Dictionary<string, object?> Health(Db db)
    {
        long N(string sql, params object?[] a) { try { return db.Scalar<long>(sql, a); } catch { return -1; } }

        // Malformed has to be counted in memory: the providers disagree on regex SQL, and this set
        // is small by construction (only non-null values, only eligible accounts).
        long malformed = 0;
        try
        {
            foreach (var r in db.Query($"SELECT registration_no FROM users WHERE {EligibleWhere} AND registration_no IS NOT NULL AND registration_no<>''"))
                if (!Legacy.IsMatch(H.Str(r["registration_no"]) ?? "")) malformed++;
        }
        catch { malformed = -1; }

        return new Dictionary<string, object?>
        {
            ["eligible_students"] = N($"SELECT COUNT(*) FROM users WHERE {EligibleWhere}"),
            ["missing_number"] = N($"SELECT COUNT(*) FROM users WHERE {EligibleWhere} AND (registration_no IS NULL OR registration_no='')"),
            ["blank_number"] = N($"SELECT COUNT(*) FROM users WHERE {EligibleWhere} AND registration_no=''"),
            ["duplicate_numbers"] = N(@"SELECT COUNT(*) FROM (SELECT registration_no FROM users
                WHERE registration_no IS NOT NULL GROUP BY registration_no HAVING COUNT(*)>1) d"),
            ["malformed_numbers"] = malformed,
            // Registry drift, both directions. A number on a user with no reservation means the
            // ledger cannot vouch for it; a reservation pointing at a user who no longer carries it
            // means the projection moved without the ledger. Either is a reason not to enforce
            // uniqueness yet, and both are invisible without looking for them explicitly.
            ["projection_without_reservation"] = N(@"SELECT COUNT(*) FROM users u
                WHERE u.registration_no IS NOT NULL AND u.registration_no<>'' AND NOT EXISTS(
                    SELECT 1 FROM pci_student_number_registry r WHERE r.student_number=u.registration_no)"),
            ["reservation_without_projection"] = N(@"SELECT COUNT(*) FROM pci_student_number_registry r
                WHERE r.state='issued' AND NOT EXISTS(
                    SELECT 1 FROM users u WHERE u.registration_no=r.student_number)"),
            ["retired_reservations"] = N("SELECT COUNT(*) FROM pci_student_number_registry WHERE state='retired'"),
            ["quarantined_reservations"] = N("SELECT COUNT(*) FROM pci_student_number_registry WHERE state='quarantined'"),
            // Identity-shape faults that a Student Number cannot fix but must not hide.
            ["students_without_profile"] = N(@"SELECT COUNT(*) FROM users u
                WHERE u.role='student' AND u.status<>'erased'
                AND NOT EXISTS(SELECT 1 FROM student_profiles p WHERE p.user_id=u.id)"),
            // The uniqueness index is created by Migrate only once the data is clean; this is the
            // same predicate, so an operator can see whether the next boot will pick it up.
            ["projection_index_eligible"] = N(@"SELECT COUNT(*) FROM (SELECT registration_no FROM users
                WHERE registration_no IS NOT NULL GROUP BY registration_no HAVING COUNT(*)>1) d") == 0,
        };
    }

    /// <summary>
    /// What <see cref="Run"/> would do, without doing any of it. Returns the number that WOULD be
    /// issued for each affected account so a reviewer can sanity-check the derivation — including
    /// that the year follows the account's created_at — before a single row is written.
    /// </summary>
    public static Dictionary<string, object?> Preview(Db db, int limit = 100)
    {
        limit = Math.Clamp(limit, 1, 500);
        var rows = db.Query($@"SELECT id,created_at FROM users
            WHERE {EligibleWhere} AND (registration_no IS NULL OR registration_no='')
            ORDER BY id LIMIT {limit}");

        var sample = new List<object>();
        foreach (var r in rows)
        {
            var uid = H.L(r["id"]);
            var number = StudentNumbers.Format(uid, H.Str(r["created_at"]));
            // A would-be number that is already reserved to somebody else is a collision, not a
            // candidate. Surfacing it here is the whole reason a preview exists.
            var held = db.QueryOne("SELECT resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?", number);
            var conflict = held is not null && H.L(held["resolves_to_user_id"]) != uid;
            sample.Add(new { user_id = uid, created_at = H.Str(r["created_at"]), would_issue = number, conflict });
        }

        return new Dictionary<string, object?>
        {
            ["total_missing"] = db.Scalar<long>($"SELECT COUNT(*) FROM users WHERE {EligibleWhere} AND (registration_no IS NULL OR registration_no='')"),
            ["sample_size"] = sample.Count,
            ["sample"] = sample,
            ["writes"] = 0,
        };
    }

    /// <summary>
    /// Issue numbers for one batch of accounts that have none.
    ///
    /// Resumable and idempotent by construction rather than by bookkeeping: the query selects rows
    /// that still lack a number, so stopping and restarting simply picks up where it left off, and
    /// re-running after completion selects nothing. Each account is its own transaction — one
    /// collision quarantines that account and the batch carries on, instead of a single bad row
    /// rolling back thousands of good ones.
    /// </summary>
    public static Dictionary<string, object?> Run(Db db, int batchSize = 200, string? correlationId = null)
    {
        batchSize = Math.Clamp(batchSize, 1, 1000);
        var rows = db.Query($@"SELECT id FROM users
            WHERE {EligibleWhere} AND (registration_no IS NULL OR registration_no='')
            ORDER BY id LIMIT {batchSize}");

        long issued = 0, quarantined = 0;
        var failures = new List<object>();
        long highWater = 0;

        foreach (var r in rows)
        {
            var uid = H.L(r["id"]);
            highWater = uid;
            try
            {
                db.Transaction(() => StudentNumbers.GetOrIssue(db, uid, "phase2_backfill", correlationId));
                issued++;
            }
            catch (Exception ex)
            {
                // The account keeps no number and the ledger records why. Quarantine is a state a
                // human resolves; it is never resolved by guessing another number for this person.
                quarantined++;
                failures.Add(new { user_id = uid, reason = ex.Message });
                try
                {
                    db.Execute(@"INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,
                            resolves_to_user_id,state,reason_code,correlation_id)
                        VALUES(?,?,?,NULL,'quarantined','backfill_conflict',?)",
                        $"QUARANTINE-{uid}", StudentNumbers.FormatVersion, uid, correlationId);
                }
                catch { /* a repeat run re-quarantining the same account is not itself a failure */ }
            }
        }

        return new Dictionary<string, object?>
        {
            ["examined"] = rows.Count,
            ["issued"] = issued,
            ["quarantined"] = quarantined,
            ["high_water_user_id"] = highWater,
            ["remaining"] = db.Scalar<long>($"SELECT COUNT(*) FROM users WHERE {EligibleWhere} AND (registration_no IS NULL OR registration_no='')"),
            ["failures"] = failures,
        };
    }

    /// <summary>
    /// Reconcile the ledger against the projection. Returns drift counts and a bounded sample of the
    /// offending numbers — never the people behind them, so a reconciliation report can be handled
    /// by whoever is on call without granting them access to member records.
    /// </summary>
    public static Dictionary<string, object?> Reconcile(Db db)
    {
        var orphanProjection = db.Query(@"SELECT u.registration_no AS n FROM users u
            WHERE u.registration_no IS NOT NULL AND u.registration_no<>'' AND NOT EXISTS(
                SELECT 1 FROM pci_student_number_registry r WHERE r.student_number=u.registration_no)
            ORDER BY u.id LIMIT 50");
        var orphanReservation = db.Query(@"SELECT r.student_number AS n FROM pci_student_number_registry r
            WHERE r.state='issued' AND NOT EXISTS(
                SELECT 1 FROM users u WHERE u.registration_no=r.student_number)
            ORDER BY r.id LIMIT 50");

        return new Dictionary<string, object?>
        {
            ["projection_without_reservation"] = orphanProjection.Count,
            ["reservation_without_projection"] = orphanReservation.Count,
            ["sample_projection_without_reservation"] = orphanProjection.Select(r => H.Str(r["n"])).ToList(),
            ["sample_reservation_without_projection"] = orphanReservation.Select(r => H.Str(r["n"])).ToList(),
            ["reconciled"] = orphanProjection.Count == 0 && orphanReservation.Count == 0,
        };
    }

    /// <summary>
    /// Permanently retire a number so erasure or account closure releases the PERSON without
    /// releasing the NUMBER. Called from the erasure path, which clears users.registration_no; the
    /// reservation staying behind in state 'retired' is the only thing standing between that and a
    /// future account being issued a dead person's identifier.
    /// </summary>
    public static void Retire(Db db, string? number, string reasonCode, long? adminId = null, string? correlationId = null)
    {
        if (string.IsNullOrWhiteSpace(number)) return;
        var changed = db.Execute(@"UPDATE pci_student_number_registry
            SET state='retired', changed_at=datetime('now'), reason_code=?, changed_by_admin_id=?,
                correlation_id=?, row_version=row_version+1
            WHERE student_number=? AND state<>'retired'", reasonCode, adminId, correlationId, number);
        // A number issued before the registry existed has no row to retire. Insert the reservation
        // now rather than leaving the value unclaimed and therefore reissuable — retiring an
        // unrecorded number is exactly the case the ledger exists to close.
        if (changed == 0 && db.QueryOne("SELECT id FROM pci_student_number_registry WHERE student_number=?", number) is null)
            try
            {
                db.Execute(@"INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,
                        resolves_to_user_id,state,reason_code,changed_by_admin_id,correlation_id)
                    VALUES(?,?,0,NULL,'retired',?,?,?)",
                    number, StudentNumbers.FormatVersion, reasonCode, adminId, correlationId);
            }
            catch { /* a concurrent retire got there first — the number is reserved either way */ }
    }
}
