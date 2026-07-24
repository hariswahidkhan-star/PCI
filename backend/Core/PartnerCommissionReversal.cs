using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Partner commission reversals — Phase 2 of the Marketing Partner finance module.
///
/// When money goes back to the student, the commission that money earned must go back too. The original
/// transaction is NEVER edited or deleted: a reversal is a new, linked transaction carrying a negative
/// commission, so the ledger always shows what was earned, what was clawed back, and why.
///
///   full refund / chargeback → reverse the whole commission
///   partial refund           → reverse the refunded proportion of it
///   already paid out         → the negative balance stands as a recoverable, offset against future
///                              approved commission rather than hidden
///
/// Idempotent by construction: the reversal's dedupe_key encodes the source transaction and the refunded
/// amount, so a retried webhook (Stripe retries for days) records nothing further, and a *larger* refund
/// later reverses only the incremental difference.
/// </summary>
public static class PartnerCommissionReversal
{
    /// <summary>
    /// Bring the ledger into line with the current refunded state of a payment. Returns the id of the
    /// reversal written, or 0 when there is nothing to do (no commission, nothing refunded, or the
    /// refund has already been fully reversed).
    /// </summary>
    public static long EnsureForPayment(Db db, long paymentId, string cause = "refund")
    {
        var pay = db.QueryOne("SELECT id,final_amount,amount_refunded,payment_status FROM payments WHERE id=?", paymentId);
        if (pay is null) return 0;

        var source = db.QueryOne(@"SELECT * FROM partner_commission_transactions
            WHERE payment_id=? AND (reversal_of_transaction_id IS NULL OR reversal_of_transaction_id=0)
            ORDER BY id LIMIT 1", paymentId);
        if (source is null) return 0;                       // this payment never earned commission

        var status = (H.Str(pay["payment_status"]) ?? "").ToLowerInvariant();
        var netMinor = H.L(source["eligible_net_minor"]);
        var commissionMinor = H.L(source["commission_minor"]);
        if (commissionMinor == 0) return 0;

        // How much of the sale has gone back? A reversed/refunded status means all of it, whatever the
        // recorded amount says; a partial refund carries the amount actually returned.
        var refundedMinor = status is "refunded" or "reversed"
            ? netMinor
            : Math.Min(Money.ToMinor(pay["amount_refunded"]), netMinor);
        if (refundedMinor <= 0) return 0;

        // Commission that SHOULD stand given the refund, and therefore how much must be clawed back.
        var reverseTarget = Money.Apportion(commissionMinor, refundedMinor, netMinor);
        // Reversals carry negative commission, so negating the sum gives what has already been clawed back.
        var alreadyReversed = -db.Scalar<long>(@"SELECT COALESCE(SUM(commission_minor),0)
            FROM partner_commission_transactions WHERE reversal_of_transaction_id=?", H.L(source["id"]));
        var deltaMinor = reverseTarget - alreadyReversed;   // incremental: a bigger refund reverses the rest
        if (deltaMinor <= 0) return 0;

        var sourceId = H.L(source["id"]);
        var partnerId = H.L(source["partner_id"]);
        var full = refundedMinor >= netMinor;
        // The key carries the cumulative refunded amount, so a retried webhook is a no-op while a genuinely
        // larger refund later produces one further incremental reversal.
        var dedupe = $"rev:{sourceId}:{refundedMinor}";
        var reason = full
            ? $"Full {cause}: commission reversed."
            : $"Partial {cause}: {Money.Format(refundedMinor)} of {Money.Format(netMinor)} returned; commission reversed proportionally.";

        var (_, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO partner_commission_transactions
            (dedupe_key,partner_id,agreement_id,commission_rule_id,discount_code_id,code_redemption_id,payment_id,user_id,
             certification_id,route_key,product_type,currency,gross_minor,discount_minor,eligible_net_minor,
             commission_type,commission_rate_bp,commission_basis,commission_minor,status,earned_at,
             reversal_of_transaction_id,reason)
            VALUES(?,?,?,?,?,?,?,?, ?,?,?,?,?,?,?, ?,?,?,?,?,datetime('now'), ?,?)",
            dedupe, partnerId, source["agreement_id"], source["commission_rule_id"], source["discount_code_id"],
            source["code_redemption_id"], paymentId, source["user_id"],
            source["certification_id"], H.Str(source["route_key"]), H.Str(source["product_type"]), H.Str(source["currency"]),
            -H.L(source["gross_minor"]), -H.L(source["discount_minor"]), -refundedMinor,
            H.Str(source["commission_type"]), H.L(source["commission_rate_bp"]), H.Str(source["commission_basis"]),
            -deltaMinor, PartnerCommission.StatusReversed, sourceId, reason);
        if (changes == 0) return 0;

        var row = db.QueryOne("SELECT id FROM partner_commission_transactions WHERE dedupe_key=?", dedupe);
        if (row is null) return 0;
        var reversalId = H.L(row["id"]);
        db.Execute("UPDATE partner_commission_transactions SET txn_ref=? WHERE id=?", $"PCT-{reversalId:D6}", reversalId);
        PartnerCommission.LogEvent(db, reversalId, null, PartnerCommission.StatusReversed, "system", null, reason, $"payment:{paymentId}");

        // A fully reversed original that has not been paid is itself moved to reversed, so it can never be
        // picked up for settlement. One already paid out stays as it was — the money genuinely left — and
        // the negative reversal becomes a recoverable balance offset against future approved commission.
        var sourceStatus = H.Str(source["status"]);
        if (full && sourceStatus != PartnerCommission.StatusPaid && sourceStatus != PartnerCommission.StatusPartiallyPaid)
            PartnerCommission.Transition(db, sourceId, PartnerCommission.StatusReversed, "system", null, reason, $"reversal:{reversalId}");
        else if (sourceStatus == PartnerCommission.StatusPaid || sourceStatus == PartnerCommission.StatusPartiallyPaid)
            PartnerCommission.LogEvent(db, sourceId, sourceStatus, sourceStatus, "system", null,
                $"Reversed after payout — {Money.Format(deltaMinor)} recoverable against future commission.", $"reversal:{reversalId}");

        return reversalId;
    }

    /// <summary>
    /// Recoverable balance: commission already paid out that has since been reversed. Positive means the
    /// partner owes PCI that much, to be offset against future approved commission rather than invoiced.
    /// </summary>
    public static long RecoverableMinor(Db db, long partnerId)
    {
        return db.Scalar<long>(@"SELECT COALESCE(SUM(-r.commission_minor),0)
            FROM partner_commission_transactions r
            JOIN partner_commission_transactions s ON s.id=r.reversal_of_transaction_id
            WHERE r.partner_id=? AND s.status IN ('paid','partially_paid')", partnerId);
    }
}
