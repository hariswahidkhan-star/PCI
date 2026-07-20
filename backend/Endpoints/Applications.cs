using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Shared exam-entitlement grant for sponsor-funded access. The candidate-facing "apply for a certification
/// through a route" surface and its admin review were retired from the portal: certification applications
/// now arrive only through the dedicated public flows — Honorary from the public website, Founding via
/// access codes — and the standard route is simply the paid exam checkout (which requires an active
/// membership). This helper remains because <see cref="Partners"/> grants a fully-waived exam entitlement
/// when a partner-sponsored discount code is redeemed.
/// </summary>
public static class Applications
{
    /// <summary>Grant a fresh, fully-waived exam entitlement scoped to a certification (mirrors the
    /// founding-waiver grant: a paid-at-zero payments row plus an exam_entitlements ledger row with a
    /// one-year scheduling window). Guarded so a candidate never stacks two open entitlements for the
    /// same certification. Returns the payment reference, or null if an open entitlement already exists.</summary>
    public static string? GrantExamEntitlement(Db db, long userId, long certId, string feeMode, string routeKey)
    {
        var hasOpen = db.QueryOne(@"SELECT p.id FROM payments p
                JOIN exam_entitlements e ON e.payment_id=p.id
                WHERE p.user_id=? AND p.payment_status='paid' AND p.product_type IN ('exam','bundle')
                AND e.certification_id=? AND COALESCE(e.status,'available') IN ('available','booked')", userId, certId) is not null;
        if (hasOpen) return null;
        var reference = "APP-" + Security.RandomHex(5).ToUpperInvariant();
        var provider = feeMode == "sponsored" ? "application_sponsored" : "application_waiver";
        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference,exam_schedule_deadline)
            VALUES(?, 'exam',0,0,'USD',?, 'paid', datetime('now'), ?, datetime('now','+1 year'))", userId, provider, reference);
        db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,route_key,status,valid_until) VALUES(?,?, 'exam', ?, ?, 'available', datetime('now','+1 year'))",
            userId, payId, certId, routeKey);
        return reference;
    }
}
