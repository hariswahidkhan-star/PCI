using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Partner commission engine — Phase 1 of the Marketing Partner finance module.
/// Design: docs/finance/PARTNER_FINANCE_PHASE0_AUDIT.md.
///
/// A settled, partner-attributed payment produces exactly ONE immutable commission transaction whose
/// rate, basis and rule are snapshotted at that moment. Re-running is a no-op (a UNIQUE dedupe_key does
/// the work, not an application check), so the webhook, an admin reprocess and the backfill can all call
/// <see cref="EnsureForPayment"/> freely. Nothing here ever recomputes an existing row: a correction is
/// always a new, linked transaction.
///
/// All arithmetic is integer minor units via <see cref="Money"/> — no floating point in the money path.
/// </summary>
public static class PartnerCommission
{
    // ---- lifecycle -------------------------------------------------------------------------------
    // The states a commission moves through. Partners may READ these; only Finance may move them.
    public const string StatusPaymentReceived = "payment_received";
    public const string StatusOnRefundHold = "on_refund_hold";
    public const string StatusDue = "due";
    public const string StatusPendingReview = "pending_review";
    public const string StatusApproved = "approved_for_payment";
    public const string StatusScheduled = "scheduled";
    public const string StatusPartiallyPaid = "partially_paid";
    public const string StatusPaid = "paid";
    public const string StatusOnHold = "on_hold";
    public const string StatusDisputed = "disputed";
    public const string StatusReversed = "reversed";

    /// <summary>Every legal transition. Anything not listed is refused server-side.</summary>
    private static readonly Dictionary<string, string[]> Allowed = new()
    {
        [StatusPaymentReceived] = new[] { StatusOnRefundHold, StatusDue, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusOnRefundHold] = new[] { StatusDue, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusDue] = new[] { StatusPendingReview, StatusApproved, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusPendingReview] = new[] { StatusApproved, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusApproved] = new[] { StatusScheduled, StatusPartiallyPaid, StatusPaid, StatusOnHold, StatusDisputed, StatusReversed },
        // StatusApproved is reachable from StatusScheduled so that cancelling a settlement releases its
        // commissions back into the payable pool instead of stranding them as permanently 'scheduled'.
        [StatusScheduled] = new[] { StatusApproved, StatusPartiallyPaid, StatusPaid, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusPartiallyPaid] = new[] { StatusPartiallyPaid, StatusPaid, StatusOnHold, StatusDisputed, StatusReversed },
        [StatusPaid] = new[] { StatusDisputed, StatusReversed },
        [StatusOnHold] = new[] { StatusDue, StatusPendingReview, StatusApproved, StatusDisputed, StatusReversed },
        [StatusDisputed] = new[] { StatusDue, StatusOnHold, StatusApproved, StatusReversed },
        [StatusReversed] = Array.Empty<string>(),   // terminal
    };

    public static bool CanTransition(string? from, string? to)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to)) return false;
        return Allowed.TryGetValue(from, out var next) && Array.IndexOf(next, to) >= 0;
    }

    /// <summary>Move a transaction's status, recording an append-only event. Fail-closed on an illegal move.</summary>
    public static bool Transition(Db db, long txnId, string to, string actorType, long? actorId, string? reason = null, string? reference = null)
    {
        var row = db.QueryOne("SELECT status FROM partner_commission_transactions WHERE id=?", txnId);
        if (row is null) return false;
        var from = H.Str(row["status"]);
        if (!CanTransition(from, to)) return false;
        db.Execute("UPDATE partner_commission_transactions SET status=?, updated_at=datetime('now') WHERE id=?", to, txnId);
        if (to == StatusApproved)
            db.Execute("UPDATE partner_commission_transactions SET approved_at=datetime('now'), approved_by=? WHERE id=?", actorId, txnId);
        LogEvent(db, txnId, from, to, actorType, actorId, reason, reference);
        return true;
    }

    public static void LogEvent(Db db, long txnId, string? from, string to, string actorType, long? actorId, string? reason = null, string? reference = null)
        => db.Execute(@"INSERT INTO partner_commission_events(transaction_id,from_status,to_status,actor_type,actor_id,reason,reference)
            VALUES(?,?,?,?,?,?,?)", txnId, from, to, actorType, actorId, reason, reference);

    // ---- rule resolution -------------------------------------------------------------------------
    public sealed class Resolved
    {
        public long? AgreementId;
        public long? RuleId;
        public string CommissionType = "percentage";
        public long RateBp;
        public long FixedMinor;
        public string Basis = "net_after_discount";
        public string Currency = "USD";
        public int RefundHoldDays = 30;
        /// <summary>True when no agreement/rule covered this payment and the partner's current
        /// commission_pct was snapshotted instead — the row is flagged for Finance to confirm (D3).</summary>
        public bool NeedsReview;
    }

    /// <summary>
    /// Pick the rule that governs a payment: active, effective on the payment date, every non-NULL
    /// dimension matching. Ordering is priority (lower wins), then specificity (more dimensions matched),
    /// then most recent. When nothing matches, fall back to the partner's current commission_pct and flag
    /// the transaction for Finance review rather than silently valuing it at zero.
    /// </summary>
    public static Resolved ResolveRule(Db db, long partnerId, string onDate, long? certificationId, string? routeKey, string? productType, string? country)
    {
        var day = (onDate ?? "").Length >= 10 ? onDate![..10] : DateTime.UtcNow.ToString("yyyy-MM-dd");
        var result = new Resolved();

        var agreement = db.QueryOne(@"SELECT * FROM partner_agreements
            WHERE partner_id=? AND status='active'
              AND effective_from<=?
              AND (effective_to IS NULL OR effective_to='' OR effective_to>=?)
            ORDER BY effective_from DESC, id DESC LIMIT 1", partnerId, day, day);

        if (agreement is not null)
        {
            result.AgreementId = H.L(agreement["id"]);
            result.Currency = H.Str(agreement["currency"]) ?? "USD";
            result.RefundHoldDays = (int)H.L(agreement["refund_hold_days"]);
        }

        var candidates = db.Query(@"SELECT * FROM partner_commission_rules
            WHERE partner_id=? AND active=1", partnerId);

        Dictionary<string, object?>? best = null;
        var bestScore = -1;
        var bestPriority = long.MaxValue;
        foreach (var r in candidates)
        {
            // Effective window (blank/NULL = unbounded).
            var from = H.Str(r["effective_from"]);
            var to = H.Str(r["effective_to"]);
            if (!string.IsNullOrEmpty(from) && string.CompareOrdinal(from, day) > 0) continue;
            if (!string.IsNullOrEmpty(to) && string.CompareOrdinal(to, day) < 0) continue;
            // If an agreement is in force, only its own rules apply.
            if (result.AgreementId is { } aid && r["agreement_id"] is not null && H.L(r["agreement_id"]) != aid) continue;

            var score = 0;
            if (r["certification_id"] is not null)
            {
                if (certificationId is null || H.L(r["certification_id"]) != certificationId.Value) continue;
                score += 8;
            }
            if (!string.IsNullOrEmpty(H.Str(r["product_type"])))
            {
                if (!string.Equals(H.Str(r["product_type"]), productType, StringComparison.OrdinalIgnoreCase)) continue;
                score += 4;
            }
            if (!string.IsNullOrEmpty(H.Str(r["route_key"])))
            {
                if (!string.Equals(H.Str(r["route_key"]), routeKey, StringComparison.OrdinalIgnoreCase)) continue;
                score += 2;
            }
            if (!string.IsNullOrEmpty(H.Str(r["country"])))
            {
                if (!string.Equals(H.Str(r["country"]), country, StringComparison.OrdinalIgnoreCase)) continue;
                score += 1;
            }

            var priority = H.L(r["priority"]);
            if (best is null || priority < bestPriority || (priority == bestPriority && score > bestScore))
            {
                best = r; bestScore = score; bestPriority = priority;
            }
        }

        if (best is not null)
        {
            result.RuleId = H.L(best["id"]);
            if (best["agreement_id"] is not null) result.AgreementId = H.L(best["agreement_id"]);
            result.CommissionType = H.Str(best["commission_type"]) ?? "percentage";
            result.RateBp = H.L(best["commission_rate_bp"]);
            result.FixedMinor = H.L(best["commission_fixed_minor"]);
            result.Basis = H.Str(best["commission_basis"]) ?? "net_after_discount";
            return result;
        }

        // No rule: snapshot today's headline percentage so the ledger is complete from day one, and mark
        // it for Finance to confirm. Never guess silently (D3).
        var partner = db.QueryOne("SELECT commission_pct FROM training_partners WHERE id=?", partnerId);
        result.RateBp = Money.PercentToBasisPoints(H.D(partner?["commission_pct"]));
        result.NeedsReview = true;
        return result;
    }

    // ---- creation --------------------------------------------------------------------------------
    /// <summary>
    /// Create the commission transaction for a settled, partner-attributed payment. Idempotent: a repeat
    /// call inserts nothing (UNIQUE dedupe_key). Returns the transaction id, or 0 when the payment earns
    /// no commission (not settled, not attributed to a partner, sponsored route, or zero value).
    /// </summary>
    public static long EnsureForPayment(Db db, long paymentId)
    {
        var pay = db.QueryOne("SELECT * FROM payments WHERE id=?", paymentId);
        if (pay is null) return 0;
        // Only money PCI actually collected earns commission. A waiver/sponsorship collected nothing.
        if (H.Str(pay["payment_status"]) != "paid") return 0;

        var red = db.QueryOne(@"SELECT r.id, r.code_id, r.amount_before, r.discount_amount, dc.partner_id, dc.certification_id, dc.route_key
            FROM code_redemptions r JOIN discount_codes dc ON dc.id=r.code_id
            WHERE r.payment_id=? AND dc.partner_id IS NOT NULL LIMIT 1", paymentId);
        if (red is null) return 0;

        var partnerId = H.L(red["partner_id"]);
        if (partnerId == 0) return 0;

        var routeKey = H.Str(red["route_key"]);
        // Sponsorship is the opposite money direction — the partner is funding the candidate, not
        // introducing one, so it never earns commission (D7).
        if (string.Equals(routeKey, "sponsored", StringComparison.OrdinalIgnoreCase)) return 0;

        var productType = (H.Str(pay["product_type"]) ?? "").ToLowerInvariant();
        var userId = H.L(pay["user_id"]);
        long? certId = red["certification_id"] is null ? null : H.L(red["certification_id"]);
        var country = H.Str(db.QueryOne("SELECT country FROM student_profiles WHERE user_id=?", userId)?["country"]);
        // The platform stores timestamps as 'yyyy-MM-dd HH:mm:ss'; keep the fallback in that shape too
        // (H.IsoNow is round-trip ISO and would make earned_at inconsistent with every other column).
        var paidOn = H.Str(pay["payment_date"])
            ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

        var rule = ResolveRule(db, partnerId, paidOn, certId, routeKey, productType, country);

        var netMinor = Money.ToMinor(pay["final_amount"]);
        var discountMinor = Money.ToMinor(red["discount_amount"]);
        var grossMinor = red["amount_before"] is not null ? Money.ToMinor(red["amount_before"]) : netMinor + discountMinor;

        // Basis decides what the rate is applied to. net_after_tax has no distinct meaning until tax is
        // modelled, so it currently equals net_after_discount — stated, not silently conflated.
        var basisMinor = rule.Basis switch
        {
            "gross" => grossMinor,
            _ => netMinor,
        };
        var commissionMinor = rule.CommissionType == "fixed"
            ? (basisMinor > 0 ? rule.FixedMinor : 0)
            : Money.ApplyRate(basisMinor, rule.RateBp);

        var dedupe = $"pay:{paymentId}:p:{partnerId}";
        // hold_until is computed in C#, not in SQL: MySQL has no datetime(x,y) function and Db.Translate
        // only rewrites the literal datetime('now'[,modifier]) forms, so an expression here would not port.
        var holdUntil = HoldUntil(paidOn, rule.RefundHoldDays);
        var status = rule.RefundHoldDays > 0 ? StatusOnRefundHold : StatusDue;

        var (_, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO partner_commission_transactions
            (dedupe_key,partner_id,agreement_id,commission_rule_id,discount_code_id,code_redemption_id,payment_id,user_id,
             certification_id,route_key,product_type,currency,gross_minor,discount_minor,eligible_net_minor,
             commission_type,commission_rate_bp,commission_basis,commission_minor,status,earned_at,hold_until,requires_finance_review)
            VALUES(?,?,?,?,?,?,?,?, ?,?,?,?,?,?,?, ?,?,?,?,?,?,?,?)",
            dedupe, partnerId, rule.AgreementId, rule.RuleId, H.L(red["code_id"]), H.L(red["id"]), paymentId, userId,
            certId, routeKey, productType, rule.Currency, grossMinor, discountMinor, netMinor,
            rule.CommissionType, rule.RateBp, rule.Basis, commissionMinor,
            status, paidOn, holdUntil, rule.NeedsReview ? 1 : 0);
        if (changes == 0) return 0;   // already recorded — a replay, by design a no-op

        var row = db.QueryOne("SELECT id FROM partner_commission_transactions WHERE dedupe_key=?", dedupe);
        if (row is null) return 0;
        var txnId = H.L(row["id"]);
        db.Execute("UPDATE partner_commission_transactions SET txn_ref=? WHERE id=?", $"PCT-{txnId:D6}", txnId);
        LogEvent(db, txnId, null, status, "system", null,
            rule.NeedsReview ? "No agreement rule matched; partner commission_pct snapshotted (needs Finance review)." : null,
            $"payment:{paymentId}");
        return txnId;
    }

    /// <summary>Settlement date + hold days, as the platform's canonical 'yyyy-MM-dd HH:mm:ss' string.
    /// Returns null when there is no hold. Unparseable input falls back to now + hold.</summary>
    public static string? HoldUntil(string? settledAt, int holdDays)
    {
        if (holdDays <= 0) return null;
        var basis = DateTime.TryParse(settledAt, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed)
            ? parsed
            : DateTime.UtcNow;
        return basis.AddDays(holdDays).ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
    }

    // ---- reads -----------------------------------------------------------------------------------
    /// <summary>
    /// Ledger summary for one partner, derived entirely from immutable transactions. Amounts are minor
    /// units; the caller renders them. `pending` is everything not yet payable (hold/review), `due` is
    /// payable-but-unapproved, `approved` is cleared for settlement, `paid` is settled.
    /// </summary>
    public static Dictionary<string, long> Totals(Db db, long partnerId)
    {
        var totals = new Dictionary<string, long>
        {
            ["gross"] = 0, ["net"] = 0, ["commission"] = 0,
            ["pending"] = 0, ["due"] = 0, ["approved"] = 0, ["paid"] = 0, ["reversed"] = 0,
        };
        foreach (var r in db.Query(@"SELECT status, COALESCE(SUM(gross_minor),0) g, COALESCE(SUM(eligible_net_minor),0) n,
                    COALESCE(SUM(commission_minor),0) c
                FROM partner_commission_transactions WHERE partner_id=? GROUP BY status", partnerId))
        {
            var status = H.Str(r["status"]) ?? "";
            var g = H.L(r["g"]); var n = H.L(r["n"]); var c = H.L(r["c"]);
            totals["gross"] += g; totals["net"] += n; totals["commission"] += c;
            switch (status)
            {
                case StatusPaymentReceived: case StatusOnRefundHold: case StatusPendingReview: case StatusOnHold: case StatusDisputed:
                    totals["pending"] += c; break;
                case StatusDue:
                    totals["due"] += c; break;
                case StatusApproved: case StatusScheduled: case StatusPartiallyPaid:
                    totals["approved"] += c; break;
                case StatusPaid:
                    totals["paid"] += c; break;
                case StatusReversed:
                    totals["reversed"] += c; break;
            }
        }
        return totals;
    }

    /// <summary>True once any transaction exists for this partner — used to decide whether the ledger or
    /// the legacy derived calculation answers a read during the migration window.</summary>
    public static bool HasLedger(Db db, long partnerId)
        => db.Scalar<long>("SELECT COUNT(*) FROM partner_commission_transactions WHERE partner_id=?", partnerId) > 0;
}
