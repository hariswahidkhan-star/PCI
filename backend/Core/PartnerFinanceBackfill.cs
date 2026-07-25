using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Historical backfill for the partner commission ledger — Phase 2.
///
/// Every paid, partner-attributed payment that predates the ledger gets its commission transaction, so
/// the ledger is complete from the first day it exists rather than starting at zero. The existing derived
/// history is not discarded: legacy partner_payouts rows are imported as settled settlements so opening
/// balances reconcile.
///
/// Two properties make this safe to run against production data:
///   · DRY RUN by default — it reports exactly what it would write and changes nothing;
///   · resumable and repeatable — creation is idempotent through the same UNIQUE dedupe_key the live
///     path uses, so a re-run after an interruption completes the remainder and re-writes nothing.
///
/// Where the historic rate cannot be proven from an agreement, the partner's current percentage is
/// snapshotted and the row is flagged requires_finance_review. It is never guessed silently, and never
/// left unvalued.
/// </summary>
public static class PartnerFinanceBackfill
{
    public sealed class Report
    {
        public bool DryRun;
        public int PaymentsExamined;
        public int CommissionsCreated;
        public int CommissionsAlreadyPresent;
        public int ReversalsCreated;
        public int NeedsFinanceReview;
        public int LegacyPayoutsImported;
        public long CommissionMinor;
        public List<string> Exceptions = new();
    }

    /// <summary>
    /// Walk every partner-attributed payment oldest-first and ensure its ledger rows exist.
    /// </summary>
    public static Report Run(Db db, bool dryRun = true, long? onlyPartnerId = null, int max = 5000)
    {
        var report = new Report { DryRun = dryRun };

        var rows = db.Query(@"SELECT p.id, p.payment_status, p.amount_refunded, dc.partner_id
            FROM code_redemptions r
            JOIN discount_codes dc ON dc.id=r.code_id
            JOIN payments p ON p.id=r.payment_id
            WHERE dc.partner_id IS NOT NULL
              AND (? IS NULL OR dc.partner_id=?)
            ORDER BY p.id ASC LIMIT ?", onlyPartnerId, onlyPartnerId, max);

        foreach (var row in rows)
        {
            report.PaymentsExamined++;
            var payId = H.L(row["id"]);
            var status = (H.Str(row["payment_status"]) ?? "").ToLowerInvariant();
            var partnerId = H.L(row["partner_id"]);

            var existing = db.QueryOne(@"SELECT id,commission_minor,requires_finance_review
                FROM partner_commission_transactions
                WHERE payment_id=? AND (reversal_of_transaction_id IS NULL OR reversal_of_transaction_id=0) LIMIT 1", payId);
            if (existing is not null)
            {
                report.CommissionsAlreadyPresent++;
                if (H.L(existing["requires_finance_review"]) == 1) report.NeedsFinanceReview++;
                report.CommissionMinor += H.L(existing["commission_minor"]);
                // Even an already-present commission may be missing its reversal (refunded before the
                // ledger existed) — reconcile that below.
            }
            else if (status == "paid")
            {
                if (dryRun)
                {
                    // Value it exactly as the live path would, without writing.
                    var preview = PreviewCommission(db, payId, partnerId);
                    if (preview.commissionMinor != 0 || preview.valued)
                    {
                        report.CommissionsCreated++;
                        report.CommissionMinor += preview.commissionMinor;
                        if (preview.needsReview) report.NeedsFinanceReview++;
                    }
                }
                else
                {
                    var txnId = PartnerCommission.EnsureForPayment(db, payId);
                    if (txnId > 0)
                    {
                        report.CommissionsCreated++;
                        var t = db.QueryOne("SELECT commission_minor,requires_finance_review FROM partner_commission_transactions WHERE id=?", txnId);
                        report.CommissionMinor += H.L(t?["commission_minor"]);
                        if (H.L(t?["requires_finance_review"]) == 1) report.NeedsFinanceReview++;
                    }
                }
            }
            else if (status is "waived")
            {
                // Nothing was collected, so nothing is owed — recorded as an observation, not an error.
                continue;
            }

            // Refunded/chargebacked history must carry its reversal too.
            if (status is "refunded" or "reversed" or "partially_refunded")
            {
                if (dryRun)
                {
                    if (existing is not null && H.L(existing["commission_minor"]) != 0) report.ReversalsCreated++;
                }
                else
                {
                    if (PartnerCommissionReversal.EnsureForPayment(db, payId, "backfill") > 0) report.ReversalsCreated++;
                }
            }
        }

        report.LegacyPayoutsImported = ImportLegacyPayouts(db, dryRun, onlyPartnerId, report);
        return report;
    }

    /// <summary>Value a payment exactly as the live path would, without writing anything.</summary>
    private static (long commissionMinor, bool needsReview, bool valued) PreviewCommission(Db db, long paymentId, long partnerId)
    {
        var pay = db.QueryOne("SELECT product_type,final_amount,payment_date,user_id FROM payments WHERE id=?", paymentId);
        if (pay is null) return (0, false, false);
        var red = db.QueryOne(@"SELECT r.amount_before, r.discount_amount, dc.certification_id, dc.route_key
            FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id WHERE r.payment_id=? LIMIT 1", paymentId);
        if (red is null) return (0, false, false);
        var routeKey = H.Str(red["route_key"]);
        if (string.Equals(routeKey, "sponsored", StringComparison.OrdinalIgnoreCase)) return (0, false, false);

        var productType = (H.Str(pay["product_type"]) ?? "").ToLowerInvariant();
        var country = H.Str(db.QueryOne("SELECT country FROM student_profiles WHERE user_id=?", H.L(pay["user_id"]))?["country"]);
        long? certId = red["certification_id"] is null ? null : H.L(red["certification_id"]);
        var paidOn = H.Str(pay["payment_date"]) ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

        var rule = PartnerCommission.ResolveRule(db, partnerId, paidOn, certId, routeKey, productType, country);
        var netMinor = Money.ToMinor(pay["final_amount"]);
        var grossMinor = red["amount_before"] is not null ? Money.ToMinor(red["amount_before"]) : netMinor + Money.ToMinor(red["discount_amount"]);
        var basisMinor = rule.Basis == "gross" ? grossMinor : netMinor;
        var commission = rule.CommissionType == "fixed"
            ? (basisMinor > 0 ? rule.FixedMinor : 0)
            : Money.ApplyRate(basisMinor, rule.RateBp);
        return (commission, rule.NeedsReview, true);
    }

    /// <summary>
    /// Import legacy partner_payouts as settled settlements so the ledger's opening balance reconciles to
    /// the money that actually left. They are labelled LEGACY- and carry no item allocations — inventing
    /// allocations would fabricate a link between a payout and transactions it never referenced.
    /// </summary>
    private static int ImportLegacyPayouts(Db db, bool dryRun, long? onlyPartnerId, Report report)
    {
        var imported = 0;
        var payouts = db.Query(@"SELECT id,partner_id,amount,currency,note,paid_at,paid_by FROM partner_payouts
            WHERE (? IS NULL OR partner_id=?) ORDER BY id ASC", onlyPartnerId, onlyPartnerId);
        foreach (var p in payouts)
        {
            var legacyId = H.L(p["id"]);
            var no = $"LEGACY-{legacyId}";
            if (db.QueryOne("SELECT id FROM partner_settlements WHERE settlement_no=?", no) is not null) continue;
            imported++;
            if (dryRun) continue;
            var minor = Money.ToMinor(p["amount"]);
            try
            {
                db.Execute(@"INSERT OR IGNORE INTO partner_settlements
                    (settlement_no,partner_id,currency,amount_approved_minor,amount_paid_minor,status,paid_at,internal_note,legacy_payout_id)
                    VALUES(?,?,?,?,?, 'paid',?,?,?)",
                    no, H.L(p["partner_id"]), H.Str(p["currency"]) ?? "USD", minor, minor,
                    H.Str(p["paid_at"]),
                    $"Imported from partner_payouts #{legacyId} during the Phase 2 backfill. {H.Str(p["note"]) ?? ""}".Trim(),
                    legacyId);
            }
            catch (Exception e)
            {
                report.Exceptions.Add($"legacy payout {legacyId}: {e.Message}");
            }
        }
        return imported;
    }

    /// <summary>
    /// Reconciliation: compare what the ledger says against the payments and redemptions it derives from.
    /// Discrepancies are REPORTED, never auto-corrected — silently "fixing" financial history is exactly
    /// what an audit trail exists to prevent.
    /// </summary>
    public static object Reconcile(Db db, long? onlyPartnerId = null)
    {
        var attributedPaid = db.Scalar<long>(@"SELECT COUNT(*)
            FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id JOIN payments p ON p.id=r.payment_id
            WHERE dc.partner_id IS NOT NULL AND p.payment_status='paid' AND (? IS NULL OR dc.partner_id=?)",
            onlyPartnerId, onlyPartnerId);
        var originals = db.Scalar<long>(@"SELECT COUNT(*) FROM partner_commission_transactions
            WHERE (reversal_of_transaction_id IS NULL OR reversal_of_transaction_id=0) AND (? IS NULL OR partner_id=?)",
            onlyPartnerId, onlyPartnerId);

        // Paid, attributed payments with no commission row at all.
        var missing = db.Query(@"SELECT p.id, dc.partner_id
            FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id JOIN payments p ON p.id=r.payment_id
            WHERE dc.partner_id IS NOT NULL AND p.payment_status='paid' AND (? IS NULL OR dc.partner_id=?)
              AND NOT EXISTS (SELECT 1 FROM partner_commission_transactions t
                              WHERE t.payment_id=p.id AND (t.reversal_of_transaction_id IS NULL OR t.reversal_of_transaction_id=0))
            ORDER BY p.id LIMIT 200", onlyPartnerId, onlyPartnerId);

        // Refunded payments whose commission was never reversed.
        var unreversed = db.Query(@"SELECT p.id, t.id txn_id, t.commission_minor
            FROM payments p JOIN partner_commission_transactions t ON t.payment_id=p.id
            WHERE p.payment_status IN ('refunded','reversed')
              AND (t.reversal_of_transaction_id IS NULL OR t.reversal_of_transaction_id=0)
              AND t.commission_minor <> 0
              AND (? IS NULL OR t.partner_id=?)
              AND NOT EXISTS (SELECT 1 FROM partner_commission_transactions r2 WHERE r2.reversal_of_transaction_id=t.id)
            ORDER BY p.id LIMIT 200", onlyPartnerId, onlyPartnerId);

        var needsReview = db.Scalar<long>(@"SELECT COUNT(*) FROM partner_commission_transactions
            WHERE requires_finance_review=1 AND (? IS NULL OR partner_id=?)", onlyPartnerId, onlyPartnerId);

        // Over-allocation: a transaction settled for more than it is worth.
        var overAllocated = db.Query(@"SELECT t.id, t.commission_minor, COALESCE(SUM(i.amount_allocated_minor),0) allocated
            FROM partner_commission_transactions t
            JOIN partner_settlement_items i ON i.transaction_id=t.id
            WHERE (? IS NULL OR t.partner_id=?)
            GROUP BY t.id, t.commission_minor
            HAVING COALESCE(SUM(i.amount_allocated_minor),0) > t.commission_minor
            LIMIT 200", onlyPartnerId, onlyPartnerId);

        return new
        {
            attributed_paid_payments = attributedPaid,
            commission_transactions = originals,
            missing_commission = missing.Count,
            missing_commission_sample = missing,
            refunded_without_reversal = unreversed.Count,
            refunded_without_reversal_sample = unreversed,
            over_allocated = overAllocated.Count,
            over_allocated_sample = overAllocated,
            requires_finance_review = needsReview,
            clean = missing.Count == 0 && unreversed.Count == 0 && overAllocated.Count == 0,
        };
    }
}
