using System;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — review-due / expiry governance tests (Phase 5B, §13). Pins the pure status evaluation
/// so operators can trust the amber/overdue/expired signals: expiry dominates a review flag, the due-soon
/// window is exact at its edge, unset dates read as OK, and malformed dates never crash the evaluation.
/// A fixed "now" keeps every case deterministic.
/// </summary>
public class SimGovernanceTests
{
    static readonly DateTime Now = new(2026, 07, 23, 12, 00, 00, DateTimeKind.Utc);

    [Fact]
    public void No_dates_set_is_ok()
    {
        var s = SimGovernance.Evaluate(null, null, Now);
        Assert.Equal(SimGovernance.Ok, s.Code);
        Assert.Null(s.DaysToReview);
        Assert.Null(s.DaysToExpiry);
    }

    [Fact]
    public void Review_far_in_future_is_ok()
    {
        var s = SimGovernance.Evaluate("2026-12-31", null, Now);
        Assert.Equal(SimGovernance.Ok, s.Code);
        Assert.True(s.DaysToReview > SimGovernance.DueSoonWindowDays);
    }

    [Fact]
    public void Review_within_window_is_due_soon()
    {
        var s = SimGovernance.Evaluate("2026-08-10", null, Now);   // ~18 days out
        Assert.Equal(SimGovernance.ReviewDueSoon, s.Code);
        Assert.InRange(s.DaysToReview!.Value, 0, SimGovernance.DueSoonWindowDays);
    }

    [Fact]
    public void Review_exactly_at_window_edge_is_due_soon()
    {
        var edge = Now.AddDays(SimGovernance.DueSoonWindowDays).ToString("yyyy-MM-dd HH:mm:ss");
        Assert.Equal(SimGovernance.ReviewDueSoon, SimGovernance.Evaluate(edge, null, Now).Code);
    }

    [Fact]
    public void Review_in_past_is_overdue()
    {
        var s = SimGovernance.Evaluate("2026-07-01", null, Now);
        Assert.Equal(SimGovernance.ReviewOverdue, s.Code);
        Assert.True(s.DaysToReview < 0);
    }

    [Fact]
    public void Expiry_in_past_is_expired()
    {
        var s = SimGovernance.Evaluate(null, "2026-07-20", Now);
        Assert.Equal(SimGovernance.Expired, s.Code);
        Assert.True(s.DaysToExpiry < 0);
    }

    [Fact]
    public void Expiry_dominates_a_healthy_review_date()
    {
        // Review is comfortably in the future, but the scenario has already expired → Expired wins.
        var s = SimGovernance.Evaluate("2027-01-01", "2026-07-22", Now);
        Assert.Equal(SimGovernance.Expired, s.Code);
    }

    [Fact]
    public void Future_expiry_does_not_trip_when_review_is_fine()
    {
        var s = SimGovernance.Evaluate("2027-01-01", "2027-06-01", Now);
        Assert.Equal(SimGovernance.Ok, s.Code);
        Assert.True(s.DaysToExpiry > 0);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("2026-07-23", true)]
    [InlineData("2026-07-23 09:30:00", true)]
    [InlineData("not-a-date", false)]
    [InlineData("2026-13-40", false)]
    public void IsValidDate_accepts_blank_or_parseable_only(string? value, bool expected) =>
        Assert.Equal(expected, SimGovernance.IsValidDate(value));

    [Fact]
    public void Malformed_dates_are_treated_as_unset_not_crashing()
    {
        var s = SimGovernance.Evaluate("garbage", "also-garbage", Now);
        Assert.Equal(SimGovernance.Ok, s.Code);
        Assert.Null(s.DaysToReview);
        Assert.Null(s.DaysToExpiry);
    }
}
