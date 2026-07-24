using PCI.Backend.Core;
using Xunit;
using R = PCI.Backend.Core.PartnerCodeRules;

namespace PCI.Backend.Tests;

/// <summary>
/// Partner finance Phase 4 — the rules governing a partner's own discount codes.
///
/// Two behaviours are the point of this suite, because both were previously wrong:
///
///   · <b>Reject, never clamp.</b> The portal used to run every percentage through Math.Clamp(1..100), so a
///     partner who typed 150 silently got a live 100% full-sponsorship code and a partner who typed 0 got 1%.
///     Both are terms the partner never agreed to and had no way of noticing.
///   · <b>A used code cannot be withdrawn.</b> Cancellation used to be allowed on a code with redemptions
///     against it, stranding the redemptions, commission transactions and entitlements that reference it.
/// </summary>
public class PartnerCodeRulesTests
{
    // ── reject, never clamp ───────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(0)]        // would previously have become 1%
    [InlineData(-5)]
    [InlineData(100.01)]
    [InlineData(150)]      // would previously have become a 100% full-sponsorship code
    public void An_out_of_range_percentage_is_refused_rather_than_clamped(double percent)
    {
        var r = R.PercentError(percent);
        Assert.NotNull(r);
        Assert.Equal("bad_percent", r!.Value.Error);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]      // legal here; the agreement ceiling decides whether it is permitted
    public void A_percentage_inside_the_range_is_accepted(double percent)
        => Assert.Null(R.PercentError(percent));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2.5)]      // a fractional number of uses is meaningless
    [InlineData(2_000_000)]
    public void An_impossible_use_count_is_refused(double uses)
    {
        var r = R.MaxUsesError(uses);
        Assert.NotNull(r);
        Assert.Equal("bad_max_uses", r!.Value.Error);
    }

    [Fact]
    public void A_whole_number_of_uses_is_accepted() => Assert.Null(R.MaxUsesError(250));

    [Fact]
    public void An_unknown_applies_to_target_is_refused()
    {
        Assert.Null(R.AppliesToError("all"));
        Assert.Null(R.AppliesToError("membership"));
        Assert.Null(R.AppliesToError("exam"));
        Assert.Equal("bad_applies_to", R.AppliesToError("everything")!.Value.Error);
    }

    // ── date windows ──────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void A_window_that_ends_before_it_starts_is_refused()
        => Assert.Equal("bad_date_window", R.DateWindowError("2026-06-01", "2026-05-01")!.Value.Error);

    [Fact]
    public void A_window_of_a_single_day_is_allowed()
        => Assert.Null(R.DateWindowError("2026-06-01", "2026-06-01"));

    [Theory]
    [InlineData("2026-13-01")]     // no thirteenth month
    [InlineData("2026-02-30")]     // no such day
    [InlineData("01/06/2026")]     // right date, wrong format
    [InlineData("2026-6-1")]
    public void A_date_that_is_not_a_real_iso_date_is_refused(string bad)
        => Assert.NotNull(R.DateWindowError(bad, null));

    [Fact]
    public void Either_end_of_the_window_may_be_omitted()
    {
        Assert.Null(R.DateWindowError(null, "2026-06-01"));
        Assert.Null(R.DateWindowError("2026-06-01", null));
        Assert.Null(R.DateWindowError(null, null));
        Assert.Null(R.DateWindowError("  ", ""));       // blank is "not set", not "invalid"
    }

    [Fact]
    public void A_leap_day_is_a_real_date_in_a_leap_year_only()
    {
        Assert.True(R.IsDate("2028-02-29"));
        Assert.False(R.IsDate("2026-02-29"));
    }

    // ── scope values ──────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Scope_values_must_make_sense()
    {
        Assert.Equal("bad_min_transaction", R.ScopeValueError(-1, null, null)!.Value.Error);
        Assert.Equal("bad_max_discount", R.ScopeValueError(null, 0, null)!.Value.Error);
        Assert.Equal("bad_per_user_limit", R.ScopeValueError(null, null, 0)!.Value.Error);
        Assert.Equal("bad_per_user_limit", R.ScopeValueError(null, null, 1.5)!.Value.Error);
        Assert.Null(R.ScopeValueError(0, 250, 1));
        Assert.Null(R.ScopeValueError(null, null, null));
    }

    [Fact]
    public void An_empty_country_list_means_no_restriction_rather_than_an_empty_restriction()
    {
        Assert.Null(R.NormaliseCountries(null));
        Assert.Null(R.NormaliseCountries("   "));
        Assert.Null(R.NormaliseCountries(",,"));
        Assert.Equal("Pakistan,United Arab Emirates", R.NormaliseCountries(" Pakistan , United Arab Emirates ,, "));
    }

    // ── agreement ceilings ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void A_code_above_the_agreed_discount_ceiling_is_refused()
    {
        var r = R.LimitError(maxDiscountPercent: 25, allowFullSponsorship: false, maxUsesPerCode: null,
            maxCodes: null, totalAllocation: null, percent: 30, maxUses: 1, existingCodeCount: 0, committedUses: 0);
        Assert.Equal("over_percent_limit", r!.Value.Error);
        Assert.Contains("25%", r.Value.Message);
    }

    [Fact]
    public void Full_sponsorship_requires_the_agreement_to_include_it()
    {
        Assert.Equal("full_sponsorship_not_allowed",
            R.LimitError(null, false, null, null, null, 100, 1, 0, 0)!.Value.Error);
        Assert.Null(R.LimitError(null, true, null, null, null, 100, 1, 0, 0));
    }

    [Fact]
    public void The_per_code_and_total_allocation_ceilings_are_both_enforced()
    {
        Assert.Equal("over_uses_limit", R.LimitError(null, true, 50, null, null, 10, 51, 0, 0)!.Value.Error);
        // 900 already committed against a 1000 allocation leaves room for 100, not 200.
        Assert.Equal("over_total_allocation", R.LimitError(null, true, null, null, 1000, 10, 200, 0, 900)!.Value.Error);
        Assert.Null(R.LimitError(null, true, null, null, 1000, 10, 100, 0, 900));
    }

    [Fact]
    public void The_code_count_ceiling_counts_existing_codes()
    {
        Assert.Equal("over_code_limit", R.LimitError(null, true, null, 5, null, 10, 1, 5, 0)!.Value.Error);
        Assert.Null(R.LimitError(null, true, null, 5, null, 10, 1, 4, 0));
    }

    [Fact]
    public void A_partner_with_no_configured_ceilings_is_unrestricted()
        => Assert.Null(R.LimitError(null, true, null, null, null, 100, 1_000, 999, 999_999));

    // ── a used code cannot be withdrawn ───────────────────────────────────────────────────────────

    [Fact]
    public void A_code_students_have_used_cannot_be_cancelled()
    {
        var r = R.CancelError(R.StatusActive, usedCount: 3);
        Assert.NotNull(r);
        Assert.Equal("code_in_use", r!.Value.Error);
        // The refusal has to point somewhere: stopping the code is still possible, just not by erasing it.
        Assert.Contains("Suspend", r.Value.Message);
    }

    [Fact]
    public void An_unused_code_can_be_cancelled_and_cancelling_twice_is_harmless()
    {
        Assert.Null(R.CancelError(R.StatusActive, 0));
        Assert.Null(R.CancelError(R.StatusCancelled, 0));
        // Even a used code stays cancellable-as-a-no-op once it is already cancelled.
        Assert.Null(R.CancelError(R.StatusCancelled, 9));
    }

    [Fact]
    public void Suspension_is_available_precisely_where_cancellation_is_not()
    {
        Assert.Null(R.SuspendError(R.StatusActive));
        Assert.Equal("bad_status", R.SuspendError(R.StatusCancelled)!.Value.Error);
        Assert.Equal("bad_status", R.SuspendError(R.StatusRejected)!.Value.Error);
        Assert.Null(R.ResumeError(R.StatusSuspended));
        Assert.Equal("bad_status", R.ResumeError(R.StatusActive)!.Value.Error);
    }

    // ── editing ───────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Only_a_code_that_has_not_gone_live_is_editable()
    {
        Assert.Null(R.EditError(R.StatusDraft, 0));
        Assert.Null(R.EditError(R.StatusPendingApproval, 0));
        Assert.Null(R.EditError(R.StatusRejected, 0));
        Assert.Equal("not_editable", R.EditError(R.StatusActive, 0)!.Value.Error);
        Assert.Equal("not_editable", R.EditError(R.StatusSuspended, 0)!.Value.Error);
    }

    [Fact]
    public void A_used_code_is_not_editable_whatever_its_status_says()
        => Assert.Equal("code_in_use", R.EditError(R.StatusPendingApproval, 1)!.Value.Error);

    [Fact]
    public void Only_a_draft_or_rejected_code_can_be_submitted_for_approval()
    {
        Assert.Null(R.SubmitError(R.StatusDraft));
        Assert.Null(R.SubmitError(R.StatusRejected));
        Assert.Equal("bad_status", R.SubmitError(R.StatusActive)!.Value.Error);
        Assert.Equal("bad_status", R.SubmitError(R.StatusPendingApproval)!.Value.Error);
    }

    [Fact]
    public void A_pre_approved_partners_submission_goes_straight_live()
    {
        Assert.Equal(R.StatusActive, R.SubmittedStatus(autoApprove: true));
        Assert.Equal(R.StatusPendingApproval, R.SubmittedStatus(autoApprove: false));
    }

    // ── extension is additive only ────────────────────────────────────────────────────────────────

    [Fact]
    public void An_extension_must_move_the_end_date_forwards()
    {
        Assert.Null(R.ExtendError(R.StatusActive, "2026-06-30", "2026-01-01", "2026-12-31", null));
        // Students holding the code were told it runs to 2026-06-30; pulling that in would break the promise.
        Assert.Equal("not_an_extension", R.ExtendError(R.StatusActive, "2026-06-30", null, "2026-05-01", null)!.Value.Error);
        Assert.Equal("not_an_extension", R.ExtendError(R.StatusActive, "2026-06-30", null, "2026-06-30", null)!.Value.Error);
    }

    [Fact]
    public void A_code_with_no_end_date_can_be_given_one()
        => Assert.Null(R.ExtendError(R.StatusActive, null, null, "2026-12-31", null));

    [Fact]
    public void A_code_cannot_outlive_the_agreement_it_sits_under()
    {
        var r = R.ExtendError(R.StatusActive, "2026-06-30", null, "2027-06-30", agreementEnd: "2026-12-31");
        Assert.Equal("beyond_agreement", r!.Value.Error);
        Assert.Contains("2026-12-31", r.Value.Message);
        Assert.Null(R.ExtendError(R.StatusActive, "2026-06-30", null, "2026-12-31", "2026-12-31"));
    }

    [Fact]
    public void A_closed_code_cannot_be_extended()
    {
        Assert.Equal("bad_status", R.ExtendError(R.StatusCancelled, null, null, "2026-12-31", null)!.Value.Error);
        Assert.Equal("bad_status", R.ExtendError(R.StatusRejected, null, null, "2026-12-31", null)!.Value.Error);
    }

    [Fact]
    public void An_extension_needs_a_real_date()
    {
        Assert.Equal("bad_end_date", R.ExtendError(R.StatusActive, null, null, null, null)!.Value.Error);
        Assert.Equal("bad_end_date", R.ExtendError(R.StatusActive, null, null, "soon", null)!.Value.Error);
    }

    [Fact]
    public void An_extension_cannot_land_before_the_start_date()
        => Assert.Equal("bad_date_window",
            R.ExtendError(R.StatusActive, null, "2026-07-01", "2026-06-01", null)!.Value.Error);
}
