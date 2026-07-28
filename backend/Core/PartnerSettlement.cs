using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Partner settlements — Phase 3 of the Marketing Partner finance module.
///
/// Replaces the free-form payout row (any amount, one person, no link to what it paid for) with a
/// controlled batch: prepare → approve (by a DIFFERENT person) → pay, with every included commission
/// allocated explicitly.
///
/// The two properties that make overpayment and double payment impossible are enforced in the database,
/// not by a UI check:
///   · a transaction can be allocated at most once per settlement (UNIQUE settlement_id+transaction_id);
///   · the total allocated to a transaction across ALL settlements can never exceed its commission —
///     re-checked inside the transaction that writes the allocation.
///
/// Maker-checker is enforced inside the handler rather than by the permission gate, because the gate has
/// an owner bypass: an owner would otherwise be able to approve their own batch. Unlike the platform's
/// existing maker-checker precedents, this one is FAIL-CLOSED — an unknown or missing preparer is
/// refused rather than waved through.
/// </summary>
public static class PartnerSettlement
{
    public const string StatusDraft = "draft";
    public const string StatusPendingApproval = "pending_approval";
    public const string StatusApproved = "approved";
    public const string StatusScheduled = "scheduled";
    public const string StatusPaid = "paid";
    public const string StatusCancelled = "cancelled";

    public sealed class Result
    {
        public bool Ok;
        public string? Error;
        public string? Message;
        public long Id;
        public object? Data;
        public static Result Fail(string error, string? message = null) => new() { Ok = false, Error = error, Message = message };
    }

    /// <summary>The platform's timestamp format — the ledger stores plain 'yyyy-MM-dd HH:mm:ss' throughout.</summary>
    static string Now() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Move a transaction, treating "already there" as done. Cancelling a batch that was never approved
    /// asks its transactions to return to a state they never left; that is a no-op, not a failure, and
    /// reporting it as one would bury the genuine exceptions the callers surface.
    /// </summary>
    static bool MoveOrAlreadyThere(Db db, long txnId, string to, long actorId, string reason, string reference)
    {
        var current = H.Str(db.QueryOne("SELECT status FROM partner_commission_transactions WHERE id=?", txnId)?["status"]);
        if (current == to) return true;
        return PartnerCommission.Transition(db, txnId, to, "admin", actorId, reason, reference);
    }

    /// <summary>
    /// Recoverable commission that has NOT yet been recouped through a settlement.
    ///
    /// <see cref="PartnerCommissionReversal.RecoverableMinor"/> reports the gross amount ever clawed back
    /// after payout, and it stays reported forever — the source transaction remains 'paid', because it was.
    /// Netting a recoverable off one settlement therefore has to be recorded, or the same debt would be
    /// deducted again from every settlement that followed and the partner would be underpaid indefinitely.
    /// The record is the settlement's own negative adjustments_minor, so the arithmetic stays inside the
    /// ledger rather than in a mutable running balance.
    /// </summary>
    public static long UnrecoveredMinor(Db db, long partnerId)
    {
        var gross = PartnerCommissionReversal.RecoverableMinor(db, partnerId);
        if (gross <= 0) return 0;
        // A cancelled batch releases its offset along with its allocations.
        var applied = -db.Scalar<long>(@"SELECT COALESCE(SUM(adjustments_minor),0) FROM partner_settlements
            WHERE partner_id=? AND status<>'cancelled' AND adjustments_minor<0", partnerId);
        return Math.Max(0, gross - applied);
    }

    /// <summary>
    /// A settlement transaction whose allocation now exceeds what the commission is worth, or null.
    ///
    /// Worth is the source's commission NET of every reversal linked to it — reversals never edit the
    /// source row, so a refund that arrives after a batch was prepared leaves the frozen allocation
    /// silently too large. Reversals of a paid/partially-paid source are excluded here: money for those
    /// genuinely left, and their claw-back is the recoverable balance (see UnrecoveredMinor), so netting
    /// them here as well would deduct the same debt twice.
    /// </summary>
    static Dictionary<string, object?>? StaleAllocation(Db db, long settlementId)
    {
        return db.QueryOne(@"SELECT t.id FROM partner_commission_transactions t
            JOIN partner_settlement_items i ON i.transaction_id=t.id
            JOIN partner_settlements s2 ON s2.id=i.settlement_id AND s2.status<>'cancelled'
            LEFT JOIN (SELECT reversal_of_transaction_id rid, SUM(commission_minor) rev
                       FROM partner_commission_transactions
                       WHERE reversal_of_transaction_id IS NOT NULL AND reversal_of_transaction_id<>0
                       GROUP BY reversal_of_transaction_id) rv ON rv.rid=t.id
            WHERE t.id IN (SELECT transaction_id FROM partner_settlement_items WHERE settlement_id=?)
            GROUP BY t.id, t.commission_minor, t.status, rv.rev
            HAVING SUM(i.amount_allocated_minor) > t.commission_minor
                 + CASE WHEN t.status IN ('paid','partially_paid') THEN 0 ELSE COALESCE(rv.rev,0) END
            LIMIT 1", settlementId);
    }

    /// <summary>Commission that is approved for payment and not yet fully allocated to a settlement.</summary>
    public static long PayableMinor(Db db, long partnerId)
    {
        var approved = db.Scalar<long>(@"SELECT COALESCE(SUM(t.commission_minor),0)
            FROM partner_commission_transactions t
            WHERE t.partner_id=? AND t.status IN ('approved_for_payment','scheduled','partially_paid')", partnerId);
        // Reversals of a commission that has NOT been paid out reduce what that commission is worth
        // directly (there is no money to recover — it never left). They carry status 'reversed', so the
        // sum above never sees them. Reversals of a partially-paid source are excluded: those are the
        // recoverable balance already deducted below, and counting them twice would understate payable.
        var reversedOfUnpaid = db.Scalar<long>(@"SELECT COALESCE(SUM(r.commission_minor),0)
            FROM partner_commission_transactions r
            JOIN partner_commission_transactions t ON t.id=r.reversal_of_transaction_id
            WHERE t.partner_id=? AND t.status IN ('approved_for_payment','scheduled')", partnerId);
        // The two sums MUST cover the same population. Allocations belonging to transactions that have
        // already reached 'paid' are not deducted here, because those transactions are equally absent from
        // the 'approved' sum above — counting them on one side only would subtract every historic payout a
        // second time and drive the payable balance permanently negative.
        var allocated = db.Scalar<long>(@"SELECT COALESCE(SUM(i.amount_allocated_minor),0)
            FROM partner_settlement_items i
            JOIN partner_commission_transactions t ON t.id=i.transaction_id
            JOIN partner_settlements s ON s.id=i.settlement_id
            WHERE t.partner_id=? AND s.status<>'cancelled'
              AND t.status IN ('approved_for_payment','scheduled','partially_paid')", partnerId);
        // Commission already paid out and since reversed is owed back to PCI, so it reduces what is payable
        // — but only the part not already recouped through an earlier settlement.
        return approved + reversedOfUnpaid - allocated - UnrecoveredMinor(db, partnerId);
    }

    /// <summary>
    /// Prepare a draft settlement over every approved, unallocated commission for the partner, up to an
    /// optional cap. Nothing is paid here — this is the maker's step.
    /// </summary>
    public static Result Prepare(Db db, long partnerId, long preparedBy, string? periodStart, string? periodEnd, long? capMinor, string? note)
    {
        var partner = db.QueryOne("SELECT id,name FROM training_partners WHERE id=?", partnerId);
        if (partner is null) return Result.Fail("partner_not_found");
        if (preparedBy <= 0) return Result.Fail("actor_required", "The preparing administrator could not be identified.");

        // Candidate transactions: approved for payment, positive, and not already allocated to a live batch.
        // 'reversed' carries the sum of reversals linked to the candidate: a refund before payout shrinks
        // what the commission is worth, and an allocation must be sized off that NET worth or the batch
        // pays money the partner never finally earned. (For a partially-paid source the claw-back is the
        // recoverable balance instead — netting it here too would deduct the same debt twice.)
        var candidates = db.Query(@"SELECT t.id, t.commission_minor, t.currency, t.status,
                COALESCE((SELECT SUM(i.amount_allocated_minor) FROM partner_settlement_items i
                          JOIN partner_settlements s2 ON s2.id=i.settlement_id
                          WHERE i.transaction_id=t.id AND s2.status<>'cancelled'),0) allocated,
                COALESCE((SELECT SUM(r.commission_minor) FROM partner_commission_transactions r
                          WHERE r.reversal_of_transaction_id=t.id),0) reversed
            FROM partner_commission_transactions t
            WHERE t.partner_id=? AND t.status IN ('approved_for_payment','scheduled','partially_paid')
              AND t.commission_minor>0
            ORDER BY t.id ASC", partnerId);

        string? currency = null;
        var picked = new List<(long id, long amount)>();
        var skippedCurrencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        long total = 0;
        foreach (var c in candidates)
        {
            var netWorth = H.L(c["commission_minor"])
                + (H.Str(c["status"]) == PartnerCommission.StatusPartiallyPaid ? 0 : H.L(c["reversed"]));
            var remaining = netWorth - H.L(c["allocated"]);
            if (remaining <= 0) continue;

            // One settlement pays in one currency. Summing across currencies would produce a number that
            // means nothing, so a second currency is left for its own batch rather than silently mixed in.
            var rowCurrency = H.Str(c["currency"]);
            if (string.IsNullOrWhiteSpace(rowCurrency)) rowCurrency = "USD";
            currency ??= rowCurrency;
            if (!string.Equals(currency, rowCurrency, StringComparison.OrdinalIgnoreCase))
            {
                skippedCurrencies.Add(rowCurrency);
                continue;
            }

            if (capMinor is { } cap && total + remaining > cap)
            {
                var room = cap - total;
                if (room <= 0) break;
                remaining = room;          // partial allocation of this transaction
            }
            picked.Add((H.L(c["id"]), remaining));
            total += remaining;
            if (capMinor is { } cap2 && total >= cap2) break;
        }

        if (picked.Count == 0)
            return Result.Fail("nothing_payable", "No approved commission is waiting to be settled for this partner.");
        currency ??= "USD";

        var recoverable = UnrecoveredMinor(db, partnerId);
        if (recoverable > 0 && total <= recoverable)
            return Result.Fail("offset_by_recoverable",
                $"This partner owes {Money.Format(recoverable)} from reversed commission, which exceeds the {Money.Format(total)} payable. Nothing is due.");
        var net = total - Math.Max(0, recoverable);

        long settlementId = 0;
        db.Transaction(() =>
        {
            // settlement_no is UNIQUE, and two batches prepared in the same second would otherwise collide
            // and abort the transaction. A suffix is added only when the plain number is already taken.
            var stem = $"PS-{partnerId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            var no = stem;
            for (var n = 2; n <= 50 && db.QueryOne("SELECT id FROM partner_settlements WHERE settlement_no=?", no) is not null; n++)
                no = $"{stem}-{n}";
            settlementId = db.ExecuteReturningId(@"INSERT INTO partner_settlements
                (settlement_no,partner_id,period_start,period_end,currency,eligible_commission_minor,adjustments_minor,
                 amount_approved_minor,status,prepared_by,internal_note)
                VALUES(?,?,?,?,?,?,?,?, 'pending_approval',?,?)",
                no, partnerId, periodStart, periodEnd, currency, total, -Math.Max(0, recoverable), net, preparedBy, note);
            foreach (var (txnId, amount) in picked)
                db.Execute(@"INSERT OR IGNORE INTO partner_settlement_items(settlement_id,transaction_id,amount_allocated_minor)
                    VALUES(?,?,?)", settlementId, txnId, amount);
        });

        return new Result
        {
            Ok = true,
            Id = settlementId,
            Data = new
            {
                amount = Money.ToDecimal(net),
                items = picked.Count,
                currency,
                // Surfaced, not swallowed: the operator needs to know another batch is owed.
                skipped_currencies = skippedCurrencies.ToArray(),
            },
        };
    }

    /// <summary>
    /// Approve a prepared batch. The approver must differ from the preparer — enforced here, because the
    /// permission gate lets an owner past every section check.
    /// </summary>
    public static Result Approve(Db db, long settlementId, long approvedBy)
    {
        var s = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", settlementId);
        if (s is null) return Result.Fail("not_found");
        var status = H.Str(s["status"]);
        if (status != StatusPendingApproval) return Result.Fail("bad_status", $"Only a settlement awaiting approval can be approved (this one is '{status}').");
        if (approvedBy <= 0) return Result.Fail("actor_required");

        // Fail closed: an unrecorded preparer cannot be shown to differ from the approver.
        var preparedBy = s["prepared_by"] is null ? 0 : H.L(s["prepared_by"]);
        if (preparedBy == 0)
            return Result.Fail("maker_unknown", "This settlement has no recorded preparer, so it cannot be approved. Prepare it again.");
        if (preparedBy == approvedBy)
            return Result.Fail("maker_checker", "The approver must be different from the person who prepared the settlement.");

        var partnerId = H.L(s["partner_id"]);
        var amount = H.L(s["amount_approved_minor"]);
        if (amount <= 0) return Result.Fail("bad_amount", "A settlement must approve a positive amount.");

        // A refund can land between prepare and approve; the frozen allocations do not see it. Refuse
        // here rather than at payment, while re-preparing costs nothing.
        if (StaleAllocation(db, settlementId) is not null)
            return Result.Fail("stale_allocation",
                "A commission in this batch has been reversed (refunded) since the batch was prepared, so its allocation exceeds what it is now worth. Cancel this settlement and prepare it again.");

        var stranded = new List<long>();
        var conflict = false;
        db.Transaction(() =>
        {
            // Guarded on the status this approval was decided against: a concurrent approve/cancel of the
            // same batch makes this a zero-row update and the whole approval is refused, not doubled.
            var (_, changed) = db.ExecuteWithChanges(
                "UPDATE partner_settlements SET status='approved', approved_by=?, updated_at=? WHERE id=? AND status='pending_approval'",
                approvedBy, Now(), settlementId);
            if (changed == 0) { conflict = true; return; }
            foreach (var i in db.Query("SELECT transaction_id FROM partner_settlement_items WHERE settlement_id=?", settlementId))
            {
                var txnId = H.L(i["transaction_id"]);
                if (!MoveOrAlreadyThere(db, txnId, PartnerCommission.StatusScheduled, approvedBy,
                        "Included in an approved settlement.", $"settlement:{settlementId}"))
                    stranded.Add(txnId);
            }
        });
        if (conflict)
            return Result.Fail("concurrent_update", "The settlement changed while this approval was being recorded — re-check its state.");
        return new Result
        {
            Ok = true,
            Id = settlementId,
            Data = new { partner_id = partnerId, amount = Money.ToDecimal(amount), unmoved_transactions = stranded.ToArray() },
        };
    }

    /// <summary>
    /// Record payment of an approved batch. Refuses to pay more than was approved, refuses a duplicate
    /// payment reference for the same partner, and re-checks every allocation against its transaction so
    /// a transaction can never be paid beyond its worth.
    /// </summary>
    public static Result Pay(Db db, long settlementId, long paidBy, long amountMinor, string? method, string? reference, string? proofRef, string? partnerNote)
    {
        var s = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", settlementId);
        if (s is null) return Result.Fail("not_found");
        var status = H.Str(s["status"]);
        if (status is not (StatusApproved or StatusScheduled))
            return Result.Fail("bad_status", $"Only an approved settlement can be paid (this one is '{status}').");
        if (amountMinor <= 0) return Result.Fail("bad_amount", "A payment must be a positive amount.");

        var approved = H.L(s["amount_approved_minor"]);
        var alreadyPaid = H.L(s["amount_paid_minor"]);
        if (alreadyPaid + amountMinor > approved)
            return Result.Fail("over_approved",
                $"That would pay {Money.Format(alreadyPaid + amountMinor)} against an approved {Money.Format(approved)}.");

        var partnerId = H.L(s["partner_id"]);
        if (!string.IsNullOrWhiteSpace(reference))
        {
            // The SAME settlement is deliberately included: a retried Pay call carrying the reference of a
            // transfer already recorded (timeout, double-click, replayed job) must not add a second
            // increment for one movement of money. A genuine second instalment is a second bank transfer
            // and arrives under its own reference.
            var dup = db.QueryOne(@"SELECT id FROM partner_settlements WHERE partner_id=? AND payment_reference=?
                AND (id<>? OR amount_paid_minor>0)", partnerId, reference, settlementId);
            if (dup is not null) return Result.Fail("duplicate_reference",
                H.L(dup["id"]) == settlementId
                    ? "A payment with that reference is already recorded on this settlement — this looks like a retry of the same transfer."
                    : "That payment reference is already recorded against another settlement for this partner.");
        }

        // Belt and braces: no transaction in this batch may be allocated — across ALL live settlements,
        // not just this one — for more than it is NOW worth (net of reversals: a refund between prepare
        // and pay shrinks the commission while the frozen allocation stays the same size). Summing only
        // this settlement's own items would miss the same commission placed in two batches.
        if (StaleAllocation(db, settlementId) is not null)
            return Result.Fail("stale_allocation",
                "A commission in this batch is allocated for more than it is now worth — a refund may have reversed part of it since the batch was prepared. Cancel this settlement and prepare it again.");

        var totalPaid = alreadyPaid + amountMinor;
        var full = totalPaid >= approved;
        var stranded = new List<long>();
        var conflict = false;
        db.Transaction(() =>
        {
            // paid_at is stamped in C# rather than by a SQL CASE: MySQL and SQLite disagree on conditional
            // date expressions, and COALESCE over a parameter behaves identically on both.
            // The WHERE clause repeats the amount and status this payment was decided against: if a
            // concurrent Pay committed in between, this becomes a zero-row update and the whole payment is
            // refused — the over-approved check above ran on a stale read and cannot be trusted.
            var (_, changed) = db.ExecuteWithChanges(@"UPDATE partner_settlements SET amount_paid_minor=?, status=?, paid_at=COALESCE(?,paid_at),
                    payment_method=COALESCE(?,payment_method), payment_reference=COALESCE(?,payment_reference),
                    proof_storage_ref=COALESCE(?,proof_storage_ref), partner_note=COALESCE(?,partner_note),
                    closing_balance_minor=?, updated_at=?
                WHERE id=? AND amount_paid_minor=? AND status IN ('approved','scheduled')",
                totalPaid, full ? StatusPaid : StatusApproved, full ? Now() : null,
                method, reference, proofRef, partnerNote,
                approved - totalPaid, Now(), settlementId, alreadyPaid);
            if (changed == 0) { conflict = true; return; }
            foreach (var i in db.Query("SELECT transaction_id FROM partner_settlement_items WHERE settlement_id=?", settlementId))
            {
                var txnId = H.L(i["transaction_id"]);
                var moved = MoveOrAlreadyThere(db, txnId,
                    full ? PartnerCommission.StatusPaid : PartnerCommission.StatusPartiallyPaid, paidBy,
                    full ? "Settlement paid." : $"Settlement part-paid ({Money.Format(totalPaid)} of {Money.Format(approved)}).",
                    $"settlement:{settlementId}");
                // A refused move means the transaction left the expected state (disputed, reversed) after it
                // was scheduled. The money still moved, so the payment stands and the exception is reported.
                if (!moved) stranded.Add(txnId);
            }
        });
        if (conflict)
            return Result.Fail("concurrent_update", "The settlement changed while this payment was being recorded — re-check its state before retrying.");
        return new Result
        {
            Ok = true,
            Id = settlementId,
            Data = new
            {
                paid = Money.ToDecimal(totalPaid),
                outstanding = Money.ToDecimal(approved - totalPaid),
                fully_paid = full,
                unmoved_transactions = stranded.ToArray(),
            },
        };
    }

    /// <summary>Cancel an unpaid batch, releasing its allocations. A paid settlement is never deleted.</summary>
    public static Result Cancel(Db db, long settlementId, long actorId, string reason)
    {
        var s = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", settlementId);
        if (s is null) return Result.Fail("not_found");
        if (H.L(s["amount_paid_minor"]) > 0 || H.Str(s["status"]) == StatusPaid)
            return Result.Fail("already_paid", "A paid settlement cannot be cancelled — reverse the underlying commission instead.");
        if (string.IsNullOrWhiteSpace(reason)) return Result.Fail("reason_required", "Cancelling a settlement requires a reason.");

        // The preparer's note is part of the record; the cancellation reason is appended to it, not written over it.
        var note = string.Join("\n", new[] { H.Str(s["internal_note"]), $"[{Now()}] Cancelled: {reason.Trim()}" }
            .Where(x => !string.IsNullOrWhiteSpace(x)));

        var stranded = new List<long>();
        db.Transaction(() =>
        {
            foreach (var i in db.Query("SELECT transaction_id FROM partner_settlement_items WHERE settlement_id=?", settlementId))
            {
                var txnId = H.L(i["transaction_id"]);
                if (!MoveOrAlreadyThere(db, txnId, PartnerCommission.StatusApproved, actorId,
                        $"Settlement cancelled: {reason.Trim()}", $"settlement:{settlementId}"))
                    stranded.Add(txnId);
            }
            db.Execute("DELETE FROM partner_settlement_items WHERE settlement_id=?", settlementId);
            db.Execute("UPDATE partner_settlements SET status='cancelled', internal_note=?, updated_at=? WHERE id=?",
                note, Now(), settlementId);
        });
        return new Result { Ok = true, Id = settlementId, Data = new { unmoved_transactions = stranded.ToArray() } };
    }
}
