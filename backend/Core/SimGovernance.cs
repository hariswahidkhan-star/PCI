using System.Globalization;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — scenario review-due / expiry governance (Phase 5B, §13/§14). A pure, unit-testable
/// evaluation of the two governance dates the authoring schema already carries (<c>review_due</c> and
/// <c>expires_at</c>): when is a published scenario next due for re-review, and has it passed its expiry?
///
/// This is deliberately separate from the authoring <c>review_state</c> machine (Core/SimReview) and from
/// the content validator (Core/SimContent): those decide whether a scenario is structurally allowed to
/// publish; this decides whether a LIVE scenario is still within its review window. It computes NO
/// project-controls arithmetic and touches no database or clock — the caller passes "now" — so the same
/// inputs always yield the same status and the whole thing is exercised directly from tests.
/// </summary>
public static class SimGovernance
{
    /// <summary>Within its review window and not expired — nothing to action.</summary>
    public const string Ok = "ok";
    /// <summary>Review date is within <see cref="DueSoonWindowDays"/> — flag it to operators.</summary>
    public const string ReviewDueSoon = "review_due_soon";
    /// <summary>Review date has passed — the scenario is overdue a governance re-review.</summary>
    public const string ReviewOverdue = "review_overdue";
    /// <summary>Expiry date has passed — the scenario should no longer be treated as current.</summary>
    public const string Expired = "expired";

    /// <summary>How far ahead a review date counts as "due soon" (advisory amber window).</summary>
    public const int DueSoonWindowDays = 30;

    /// <summary>The evaluated governance position for one scenario. <c>DaysToReview</c>/<c>DaysToExpiry</c>
    /// are floored whole days (negative once the date has passed); null when the corresponding date is
    /// unset. The original date strings are echoed back so a caller can render them without re-reading.</summary>
    public sealed record Status(string Code, int? DaysToReview, int? DaysToExpiry, string? ReviewDue, string? ExpiresAt);

    /// <summary>Parse an authored governance date. Accepts a plain <c>YYYY-MM-DD</c> or a full timestamp
    /// (e.g. the <c>datetime('now')</c> shape), interpreting a bare value as UTC. Returns null for
    /// null/blank/unparseable input so callers can treat "unset" and "malformed" alike.</summary>
    public static DateTime? ParseDate(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        return DateTime.TryParse(s, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt)
            ? dt
            : (DateTime?)null;
    }

    /// <summary>True when the value is unset (null/blank) OR a parseable date — i.e. safe to store. A
    /// present-but-unparseable value is rejected by the endpoint layer before it reaches the column.</summary>
    public static bool IsValidDate(string? s) => string.IsNullOrWhiteSpace(s) || ParseDate(s) is not null;

    /// <summary>Evaluate the governance status of a scenario against <paramref name="nowUtc"/>. Expiry
    /// dominates (an expired scenario is <see cref="Expired"/> even if its review date is fine); otherwise
    /// a passed review date is <see cref="ReviewOverdue"/>, a near one is <see cref="ReviewDueSoon"/>, and
    /// everything else (including a scenario with no dates set) is <see cref="Ok"/>.</summary>
    public static Status Evaluate(string? reviewDue, string? expiresAt, DateTime nowUtc)
    {
        var rd = ParseDate(reviewDue);
        var ex = ParseDate(expiresAt);
        int? daysToReview = rd is DateTime r ? (int)Math.Floor((r - nowUtc).TotalDays) : (int?)null;
        int? daysToExpiry = ex is DateTime e ? (int)Math.Floor((e - nowUtc).TotalDays) : (int?)null;

        string code;
        if (ex is DateTime ee && ee <= nowUtc) code = Expired;
        else if (rd is DateTime rr && rr <= nowUtc) code = ReviewOverdue;
        else if (rd is DateTime rs && (rs - nowUtc).TotalDays <= DueSoonWindowDays) code = ReviewDueSoon;
        else code = Ok;

        return new Status(code, daysToReview, daysToExpiry, reviewDue, expiresAt);
    }
}
