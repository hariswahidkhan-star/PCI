namespace PCI.Backend.Core;

/// <summary>
/// The rules governing a marketing partner's own discount codes — Phase 4 of the Marketing Partner module.
///
/// These are pure decisions, deliberately separated from the HTTP handlers in <see cref="Endpoints.PartnerPortal"/>
/// so that "can this code be cancelled?" is answerable — and testable — without standing up the web app.
/// The handlers stay thin: authenticate, gather, ask here, respond.
///
/// Two principles run through all of it:
///
///   · <b>Reject, never clamp.</b> Silently rewriting a partner's input into something legal means they are
///     bound by terms they never entered. Every out-of-range value is refused with the reason.
///   · <b>The past is not editable.</b> A code that students have redeemed carries commitments — redemptions,
///     commission transactions, entitlements — so it can be stopped going forward but never rewritten or
///     withdrawn. Suspension exists precisely so "stop this code" does not have to mean "pretend it never
///     happened".
/// </summary>
public static class PartnerCodeRules
{
    public const string StatusDraft = "draft";
    public const string StatusPendingApproval = "pending_approval";
    public const string StatusActive = "active";
    public const string StatusSuspended = "suspended";
    public const string StatusRejected = "rejected";
    public const string StatusCancelled = "cancelled";

    /// <summary>A refusal: a stable code for the client to branch on, and a sentence for a human to read.</summary>
    public readonly record struct Refusal(string Error, string Message);

    public static bool IsDate(string? s) => s is { Length: 10 } && DateTime.TryParseExact(s, "yyyy-MM-dd",
        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);

    /// <summary>Blank becomes NULL, so "no end date" is one value rather than two that compare differently.</summary>
    public static string? Nz(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    /// <summary>Normalise a comma-separated country list; returns null when nothing usable was supplied.</summary>
    public static string? NormaliseCountries(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length == 0 ? null : string.Join(",", parts);
    }

    /// <summary>The discount percentage a partner entered, refused rather than clamped when out of range.</summary>
    public static Refusal? PercentError(double percent)
        => percent is >= 1 and <= 100 ? null
            : new Refusal("bad_percent", "The discount percentage must be between 1 and 100.");

    public static Refusal? MaxUsesError(double maxUses)
        => maxUses >= 1 && maxUses <= 1_000_000 && maxUses == Math.Floor(maxUses) ? null
            : new Refusal("bad_max_uses", "The number of uses must be a whole number of at least 1.");

    public static Refusal? AppliesToError(string appliesTo)
        => appliesTo is "all" or "membership" or "exam" ? null
            : new Refusal("bad_applies_to", "A code applies to 'all', 'membership' or 'exam'.");

    /// <summary>Date window: both dates real, and the end not before the start.</summary>
    public static Refusal? DateWindowError(string? startDate, string? endDate)
    {
        var start = Nz(startDate);
        var end = Nz(endDate);
        if (start is not null && !IsDate(start))
            return new Refusal("bad_start_date", "The start date must be a real date in YYYY-MM-DD form.");
        if (end is not null && !IsDate(end))
            return new Refusal("bad_end_date", "The end date must be a real date in YYYY-MM-DD form.");
        if (start is not null && end is not null && string.CompareOrdinal(end, start) < 0)
            return new Refusal("bad_date_window", "The end date cannot be before the start date.");
        return null;
    }

    /// <summary>The numeric scope limits a partner may put on a code (all enforced at redemption).</summary>
    public static Refusal? ScopeValueError(double? minTransaction, double? maxDiscount, double? perUserLimit)
    {
        if (minTransaction is { } mt && mt < 0)
            return new Refusal("bad_min_transaction", "The minimum transaction cannot be negative.");
        if (maxDiscount is { } md && md <= 0)
            return new Refusal("bad_max_discount", "The maximum discount cap must be greater than zero.");
        if (perUserLimit is { } pl && (pl < 1 || pl != Math.Floor(pl)))
            return new Refusal("bad_per_user_limit", "The per-student limit must be a whole number of at least 1.");
        return null;
    }

    /// <summary>
    /// The agreement ceilings PCI configured. Passing the already-counted totals in keeps this pure — the
    /// caller does the two COUNT/SUM queries, this decides. A null ceiling means "not capped".
    /// </summary>
    public static Refusal? LimitError(double? maxDiscountPercent, bool allowFullSponsorship, long? maxUsesPerCode,
        long? maxCodes, long? totalAllocation, double percent, long maxUses, long existingCodeCount, long committedUses)
    {
        if (maxDiscountPercent is { } cap && percent > cap)
            return new Refusal("over_percent_limit", $"Your agreement allows codes up to {cap:0.#}%.");
        if (percent >= 100 && !allowFullSponsorship)
            return new Refusal("full_sponsorship_not_allowed", "Your agreement does not include 100% sponsorship.");
        if (maxUsesPerCode is { } perCode && maxUses > perCode)
            return new Refusal("over_uses_limit", $"Your agreement allows up to {perCode} uses per code.");
        if (maxCodes is { } codeCap && existingCodeCount >= codeCap)
            return new Refusal("over_code_limit", $"Your agreement allows {codeCap} codes.");
        if (totalAllocation is { } alloc && committedUses + maxUses > alloc)
            return new Refusal("over_total_allocation",
                $"Your allocation is {alloc} uses and {committedUses} are already committed.");
        return null;
    }

    /// <summary>
    /// Cancellation withdraws a code entirely, so it is only available while the code has done nothing.
    /// Once students have redeemed it, the redemptions, commission transactions and entitlements that
    /// reference it would be stranded — suspension is the correct instrument, and the message says so.
    /// </summary>
    public static Refusal? CancelError(string? status, long usedCount)
    {
        if (status == StatusCancelled) return null;                 // idempotent: already where the caller wants it
        if (usedCount > 0)
            return new Refusal("code_in_use",
                $"This code has been used {usedCount} time(s), so it cannot be cancelled. Suspend it instead to stop further use.");
        return null;
    }

    public static Refusal? SuspendError(string? status)
        => status is StatusCancelled or StatusRejected
            ? new Refusal("bad_status", "This code is already closed.")
            : null;

    public static Refusal? ResumeError(string? status)
        => status == StatusSuspended ? null
            : new Refusal("bad_status", "Only a suspended code can be resumed.");

    /// <summary>Terms are editable only before the code has gone anywhere.</summary>
    public static Refusal? EditError(string? status, long usedCount)
    {
        if (status is not (StatusDraft or StatusPendingApproval or StatusRejected))
            return new Refusal("not_editable",
                "Only a draft, pending or rejected code can be edited. Use 'extend' to change the end date of a live code.");
        if (usedCount > 0)
            return new Refusal("code_in_use", "This code has already been used and can no longer be edited.");
        return null;
    }

    public static Refusal? SubmitError(string? status)
        => status is StatusDraft or StatusRejected ? null
            : new Refusal("bad_status", "Only a draft or rejected code can be submitted.");

    /// <summary>Where a submitted code lands: straight to live when PCI pre-approved this partner.</summary>
    public static string SubmittedStatus(bool autoApprove) => autoApprove ? StatusActive : StatusPendingApproval;

    /// <summary>
    /// Extension is additive only. Students holding the code were told it runs to the date it currently
    /// shows, so it can be pushed out but never pulled in — and never past the agreement it sits under.
    /// </summary>
    public static Refusal? ExtendError(string? status, string? currentEnd, string? startDate, string? newEnd, string? agreementEnd)
    {
        if (status is StatusCancelled or StatusRejected)
            return new Refusal("bad_status", "A closed code cannot be extended.");
        var end = Nz(newEnd);
        if (end is null || !IsDate(end))
            return new Refusal("bad_end_date", "Provide the new end date as YYYY-MM-DD.");
        if (Nz(currentEnd) is { } cur && string.CompareOrdinal(end, cur) <= 0)
            return new Refusal("not_an_extension", $"The new end date must be after the current one ({cur}).");
        if (Nz(startDate) is { } sd && string.CompareOrdinal(end, sd) < 0)
            return new Refusal("bad_date_window", "The end date cannot be before the start date.");
        if (Nz(agreementEnd) is { } ae && string.CompareOrdinal(end, ae) > 0)
            return new Refusal("beyond_agreement", $"Your agreement ends on {ae}; a code cannot run past it.");
        return null;
    }
}
