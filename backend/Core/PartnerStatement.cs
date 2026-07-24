using System.Globalization;
using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Partner statements — Phase 3 of the Marketing Partner finance module.
///
/// A statement is a period view of the immutable ledger: opening balance, every commission earned,
/// every reversal, every settlement paid, and the closing balance. It is DERIVED on demand and never
/// stored, so a statement can never drift from the ledger it describes.
///
/// Three renderings share one builder, so the CSV a partner reconciles against, the PDF they file and
/// the JSON the dashboard draws are guaranteed to agree:
///   · <see cref="Build"/>   — the numbers;
///   · <see cref="Csv"/>     — spreadsheet-ready, one row per transaction;
///   · <see cref="Pdf"/>     — selectable text via <see cref="SimplePdf"/> (no new dependency, no fonts
///                             to install, and never a scanned image, so it stays accessible).
///
/// Partner-facing output deliberately omits user_id and the student's identity: a partner is entitled to
/// know that a code was redeemed and what it earned, not who redeemed it.
/// </summary>
public static class PartnerStatement
{
    public sealed class Line
    {
        public long Id;
        public string? Ref;
        public string? Date;
        public string? Kind;            // commission | reversal
        public string? Code;
        public string? Product;
        public string? Currency;
        public long GrossMinor;
        public long DiscountMinor;
        public long NetMinor;
        public long RateBp;
        public long CommissionMinor;
        public string? Status;
    }

    public sealed class Statement
    {
        public long PartnerId;
        public string PartnerName = "";
        public string PeriodStart = "";
        public string PeriodEnd = "";
        public string Currency = "USD";
        public long OpeningMinor;
        public long EarnedMinor;
        public long ReversedMinor;      // negative
        public long PaidMinor;
        public long ClosingMinor;
        public long RecoverableMinor;
        public List<Line> Lines = new();
        public List<Dictionary<string, object?>> Settlements = new();
    }

    /// <summary>Inclusive period. Dates are 'YYYY-MM-DD'; the end date's whole day is included.</summary>
    public static Statement Build(Db db, long partnerId, string periodStart, string periodEnd)
    {
        var partner = db.QueryOne("SELECT id,name FROM training_partners WHERE id=?", partnerId);
        var st = new Statement
        {
            PartnerId = partnerId,
            PartnerName = H.Str(partner?["name"]) ?? $"Partner {partnerId}",
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
        };
        // 'earned_at' is stored as 'YYYY-MM-DD HH:MM:SS', so comparing against the end date plus a time
        // ceiling includes the final day in full on both providers without any date-function dialect.
        var endInclusive = periodEnd + " 23:59:59";

        // Opening balance: everything earned (net of reversals) before the period, less everything settled
        // before it. This is the ledger's own arithmetic, not a stored running total that could drift.
        st.OpeningMinor =
            db.Scalar<long>(@"SELECT COALESCE(SUM(commission_minor),0) FROM partner_commission_transactions
                WHERE partner_id=? AND COALESCE(earned_at,created_at) < ?", partnerId, periodStart)
            - db.Scalar<long>(@"SELECT COALESCE(SUM(amount_paid_minor),0) FROM partner_settlements
                WHERE partner_id=? AND status='paid' AND COALESCE(paid_at,created_at) < ?", partnerId, periodStart);

        var rows = db.Query(@"SELECT t.id, t.txn_ref, t.earned_at, t.created_at, t.currency, t.product_type,
                t.gross_minor, t.discount_minor, t.eligible_net_minor, t.commission_rate_bp, t.commission_minor,
                t.status, t.reversal_of_transaction_id, dc.code
            FROM partner_commission_transactions t
            LEFT JOIN discount_codes dc ON dc.id=t.discount_code_id
            WHERE t.partner_id=? AND COALESCE(t.earned_at,t.created_at) >= ? AND COALESCE(t.earned_at,t.created_at) <= ?
            ORDER BY COALESCE(t.earned_at,t.created_at) ASC, t.id ASC", partnerId, periodStart, endInclusive);

        foreach (var r in rows)
        {
            var commission = H.L(r["commission_minor"]);
            var isReversal = H.L(r["reversal_of_transaction_id"]) > 0;
            st.Lines.Add(new Line
            {
                Id = H.L(r["id"]),
                Ref = H.Str(r["txn_ref"]),
                Date = H.Str(r["earned_at"]) ?? H.Str(r["created_at"]),
                Kind = isReversal ? "reversal" : "commission",
                Code = H.Str(r["code"]),
                Product = H.Str(r["product_type"]),
                Currency = H.Str(r["currency"]) ?? "USD",
                GrossMinor = H.L(r["gross_minor"]),
                DiscountMinor = H.L(r["discount_minor"]),
                NetMinor = H.L(r["eligible_net_minor"]),
                RateBp = H.L(r["commission_rate_bp"]),
                CommissionMinor = commission,
                Status = H.Str(r["status"]),
            });
            if (commission >= 0) st.EarnedMinor += commission; else st.ReversedMinor += commission;
            if (st.Lines.Count == 1) st.Currency = H.Str(r["currency"]) ?? st.Currency;
        }

        st.Settlements = db.Query(@"SELECT id, settlement_no, currency, amount_approved_minor, amount_paid_minor,
                status, paid_at, payment_method, payment_reference, period_start, period_end
            FROM partner_settlements
            WHERE partner_id=? AND status='paid' AND COALESCE(paid_at,created_at) >= ? AND COALESCE(paid_at,created_at) <= ?
            ORDER BY COALESCE(paid_at,created_at) ASC, id ASC", partnerId, periodStart, endInclusive);
        foreach (var s in st.Settlements) st.PaidMinor += H.L(s["amount_paid_minor"]);

        st.RecoverableMinor = PartnerCommissionReversal.RecoverableMinor(db, partnerId);
        st.ClosingMinor = st.OpeningMinor + st.EarnedMinor + st.ReversedMinor - st.PaidMinor;
        return st;
    }

    /// <summary>
    /// The issuing organisation's name for document headers and footers. Configurable through
    /// site_settings so a rebrand does not require a deployment, with the registered legal name as the
    /// fallback — a statement must always name who issued it.
    /// </summary>
    public static string OrgName(Db db)
    {
        var v = (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='org_legal_name'") ?? "").Trim();
        return v.Length > 0 ? v : "Project Controls Institute Global, Inc.";
    }

    /// <summary>The default period: the calendar month containing <paramref name="anchor"/>.</summary>
    public static (string start, string end) MonthOf(DateTime anchor)
    {
        var first = new DateTime(anchor.Year, anchor.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var last = first.AddMonths(1).AddDays(-1);
        return (first.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), last.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Quote a CSV cell through the platform's shared helper, which also neutralises the leading
    /// =/+/-/@ that a spreadsheet would execute as a formula. Codes and campaign names are
    /// partner-supplied text, so this file must not hand-roll its own quoting.
    /// </summary>
    static string Esc(string? s) => Csv.Field(s);

    public static string Csv(Statement st)
    {
        var sb = new StringBuilder();
        sb.Append("Statement,").Append(Esc(st.PartnerName)).Append('\n');
        sb.Append("Period,").Append(st.PeriodStart).Append(" to ").Append(st.PeriodEnd).Append('\n');
        sb.Append("Currency,").Append(Esc(st.Currency)).Append('\n');
        sb.Append("Opening balance,").Append(Money.Format(st.OpeningMinor)).Append('\n');
        sb.Append("Commission earned,").Append(Money.Format(st.EarnedMinor)).Append('\n');
        sb.Append("Reversals,").Append(Money.Format(st.ReversedMinor)).Append('\n');
        sb.Append("Settled and paid,").Append(Money.Format(st.PaidMinor)).Append('\n');
        sb.Append("Closing balance,").Append(Money.Format(st.ClosingMinor)).Append('\n');
        sb.Append('\n');
        sb.Append("Reference,Date,Type,Code,Product,Currency,Gross,Discount,Eligible net,Rate %,Commission,Status\n");
        foreach (var l in st.Lines)
        {
            sb.Append(Esc(l.Ref)).Append(',').Append(Esc(l.Date)).Append(',').Append(Esc(l.Kind)).Append(',')
              .Append(Esc(l.Code)).Append(',').Append(Esc(l.Product)).Append(',').Append(Esc(l.Currency)).Append(',')
              .Append(Money.Format(l.GrossMinor)).Append(',').Append(Money.Format(l.DiscountMinor)).Append(',')
              .Append(Money.Format(l.NetMinor)).Append(',')
              .Append(Money.BasisPointsToPercent(l.RateBp).ToString("0.##", CultureInfo.InvariantCulture)).Append(',')
              .Append(Money.Format(l.CommissionMinor)).Append(',').Append(Esc(l.Status)).Append('\n');
        }
        if (st.Settlements.Count > 0)
        {
            sb.Append('\n').Append("Settlement,Paid on,Method,Reference,Amount\n");
            foreach (var s in st.Settlements)
                sb.Append(Esc(H.Str(s["settlement_no"]))).Append(',').Append(Esc(H.Str(s["paid_at"]))).Append(',')
                  .Append(Esc(H.Str(s["payment_method"]))).Append(',').Append(Esc(H.Str(s["payment_reference"]))).Append(',')
                  .Append(Money.Format(H.L(s["amount_paid_minor"]))).Append('\n');
        }
        return sb.ToString();
    }

    public static byte[] Pdf(Statement st, string orgName)
    {
        var b = new List<(SimplePdf.Block, string)>
        {
            (SimplePdf.Block.H1, "Summary"),
            (SimplePdf.Block.Body, $"Opening balance: {st.Currency} {Money.Format(st.OpeningMinor)}"),
            (SimplePdf.Block.Body, $"Commission earned this period: {st.Currency} {Money.Format(st.EarnedMinor)}"),
            (SimplePdf.Block.Body, $"Reversals and refunds: {st.Currency} {Money.Format(st.ReversedMinor)}"),
            (SimplePdf.Block.Body, $"Settled and paid: {st.Currency} {Money.Format(st.PaidMinor)}"),
            (SimplePdf.Block.Body, $"Closing balance: {st.Currency} {Money.Format(st.ClosingMinor)}"),
        };
        if (st.RecoverableMinor > 0)
            b.Add((SimplePdf.Block.Body,
                $"Recoverable from previously settled commission: {st.Currency} {Money.Format(st.RecoverableMinor)}. " +
                "This is offset against future settlements rather than invoiced."));

        b.Add((SimplePdf.Block.Space, ""));
        b.Add((SimplePdf.Block.H1, "Commission detail"));
        if (st.Lines.Count == 0)
            b.Add((SimplePdf.Block.Body, "No commission activity was recorded in this period."));
        foreach (var l in st.Lines)
        {
            var rate = l.RateBp > 0 ? $" at {Money.BasisPointsToPercent(l.RateBp).ToString("0.##", CultureInfo.InvariantCulture)}%" : "";
            b.Add((SimplePdf.Block.Bullet,
                $"{l.Date}  ·  {l.Ref ?? ("#" + l.Id)}  ·  {(l.Kind == "reversal" ? "Reversal" : "Commission")}  ·  " +
                $"code {l.Code ?? "—"}  ·  {l.Product ?? "—"}  ·  eligible net {Money.Format(l.NetMinor)}{rate}  ·  " +
                $"{l.Currency} {Money.Format(l.CommissionMinor)}  ·  {l.Status}"));
        }

        if (st.Settlements.Count > 0)
        {
            b.Add((SimplePdf.Block.Space, ""));
            b.Add((SimplePdf.Block.H1, "Settlements paid"));
            foreach (var s in st.Settlements)
                b.Add((SimplePdf.Block.Bullet,
                    $"{H.Str(s["settlement_no"])}  ·  paid {H.Str(s["paid_at"]) ?? "—"}  ·  " +
                    $"{H.Str(s["payment_method"]) ?? "—"}  ·  ref {H.Str(s["payment_reference"]) ?? "—"}  ·  " +
                    $"{H.Str(s["currency"]) ?? st.Currency} {Money.Format(H.L(s["amount_paid_minor"]))}"));
        }

        b.Add((SimplePdf.Block.Space, ""));
        b.Add((SimplePdf.Block.Body,
            "This statement is generated from the commission ledger at the time of download. Amounts still on " +
            "refund hold are shown as earned but are not payable until the hold expires. If anything here does " +
            "not match your records, raise a dispute from the partner portal rather than amending your own copy."));

        return SimplePdf.Build(
            $"Commission statement — {st.PartnerName}",
            $"{st.PeriodStart} to {st.PeriodEnd}  ·  {st.Currency}  ·  {orgName}",
            b,
            $"{orgName} · partner {st.PartnerId} · generated {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
    }

    /// <summary>
    /// Remittance advice for one settlement: what was paid, how, and exactly which commission lines it
    /// covered. This is the document that lets a partner tie a bank credit to individual sales.
    /// </summary>
    public static byte[]? RemittancePdf(Db db, long settlementId, string orgName)
    {
        var s = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", settlementId);
        if (s is null) return null;
        var partnerId = H.L(s["partner_id"]);
        var name = H.Str(db.QueryOne("SELECT name FROM training_partners WHERE id=?", partnerId)?["name"]) ?? $"Partner {partnerId}";
        var currency = H.Str(s["currency"]) ?? "USD";

        var items = db.Query(@"SELECT i.amount_allocated_minor, t.txn_ref, t.earned_at, t.product_type,
                t.commission_minor, dc.code
            FROM partner_settlement_items i
            JOIN partner_commission_transactions t ON t.id=i.transaction_id
            LEFT JOIN discount_codes dc ON dc.id=t.discount_code_id
            WHERE i.settlement_id=? ORDER BY t.earned_at ASC, t.id ASC", settlementId);

        var b = new List<(SimplePdf.Block, string)>
        {
            (SimplePdf.Block.H1, "Payment"),
            (SimplePdf.Block.Body, $"Approved: {currency} {Money.Format(H.L(s["amount_approved_minor"]))}"),
            (SimplePdf.Block.Body, $"Paid: {currency} {Money.Format(H.L(s["amount_paid_minor"]))}"),
            (SimplePdf.Block.Body, $"Outstanding on this settlement: {currency} {Money.Format(H.L(s["closing_balance_minor"]))}"),
            (SimplePdf.Block.Body, $"Method: {H.Str(s["payment_method"]) ?? "—"}"),
            (SimplePdf.Block.Body, $"Reference: {H.Str(s["payment_reference"]) ?? "—"}"),
            (SimplePdf.Block.Body, $"Paid on: {H.Str(s["paid_at"]) ?? "—"}"),
        };
        var adjustments = H.L(s["adjustments_minor"]);
        if (adjustments != 0)
            b.Add((SimplePdf.Block.Body,
                $"Adjustments applied: {currency} {Money.Format(adjustments)} (commission previously settled and since reversed)."));

        b.Add((SimplePdf.Block.Space, ""));
        b.Add((SimplePdf.Block.H1, "Commission covered by this payment"));
        if (items.Count == 0)
            b.Add((SimplePdf.Block.Body, "No individual commission lines are allocated to this settlement."));
        foreach (var i in items)
            b.Add((SimplePdf.Block.Bullet,
                $"{H.Str(i["earned_at"]) ?? "—"}  ·  {H.Str(i["txn_ref"]) ?? "—"}  ·  code {H.Str(i["code"]) ?? "—"}  ·  " +
                $"{H.Str(i["product_type"]) ?? "—"}  ·  earned {Money.Format(H.L(i["commission_minor"]))}  ·  " +
                $"allocated {currency} {Money.Format(H.L(i["amount_allocated_minor"]))}"));

        if (!string.IsNullOrWhiteSpace(H.Str(s["partner_note"])))
        {
            b.Add((SimplePdf.Block.Space, ""));
            b.Add((SimplePdf.Block.H1, "Note"));
            b.Add((SimplePdf.Block.Body, H.Str(s["partner_note"])!));
        }

        return SimplePdf.Build(
            $"Remittance advice {H.Str(s["settlement_no"])}",
            $"{name}  ·  {H.Str(s["period_start"]) ?? "—"} to {H.Str(s["period_end"]) ?? "—"}  ·  {orgName}",
            b,
            $"{orgName} · settlement {H.Str(s["settlement_no"])} · generated {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
    }
}
