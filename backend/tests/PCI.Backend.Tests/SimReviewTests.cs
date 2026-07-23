using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — review/publication state-machine tests (Phase 5A, §13). Pins the workflow graph so the
/// API and its guarantees can never drift: one step forward along the lifecycle, return-to-draft from any
/// in-review state, retire only from published, and the publish/maker-checker gate points.
/// </summary>
public class SimReviewTests
{
    [Theory]
    [InlineData("draft", "calc_review")]
    [InlineData("calc_review", "learning_review")]
    [InlineData("learning_review", "safety_review")]
    [InlineData("safety_review", "pilot")]
    [InlineData("pilot", "approved")]
    [InlineData("approved", "published")]
    public void One_step_forward_is_allowed(string from, string to) =>
        Assert.True(SimReview.CanTransition(from, to));

    [Theory]
    [InlineData("draft", "approved")]      // skipping stages
    [InlineData("draft", "published")]
    [InlineData("calc_review", "pilot")]
    [InlineData("pilot", "published")]
    public void Skipping_stages_is_refused(string from, string to) =>
        Assert.False(SimReview.CanTransition(from, to));

    [Theory]
    [InlineData("calc_review")]
    [InlineData("learning_review")]
    [InlineData("safety_review")]
    [InlineData("pilot")]
    [InlineData("approved")]
    public void Any_in_review_state_can_return_to_draft(string from) =>
        Assert.True(SimReview.CanTransition(from, "draft"));

    [Fact]
    public void Published_and_retired_cannot_return_to_draft()
    {
        Assert.False(SimReview.CanTransition("published", "draft"));
        Assert.False(SimReview.CanTransition("retired", "draft"));
    }

    [Fact]
    public void Retire_is_only_from_published()
    {
        Assert.True(SimReview.CanTransition("published", "retired"));
        Assert.False(SimReview.CanTransition("approved", "retired"));
        Assert.False(SimReview.CanTransition("draft", "retired"));
    }

    [Fact]
    public void Self_and_unknown_transitions_are_refused()
    {
        Assert.False(SimReview.CanTransition("draft", "draft"));
        Assert.False(SimReview.CanTransition("draft", "nonsense"));
        Assert.False(SimReview.CanTransition(null, "draft"));
    }

    [Fact]
    public void Only_approve_and_publish_require_the_content_validator()
    {
        Assert.True(SimReview.RequiresPublishable("approved"));
        Assert.True(SimReview.RequiresPublishable("published"));
        Assert.False(SimReview.RequiresPublishable("pilot"));
        Assert.False(SimReview.RequiresPublishable("calc_review"));
    }

    [Fact]
    public void Only_approval_is_the_maker_checker_checkpoint()
    {
        Assert.True(SimReview.RequiresDifferentChecker("approved"));
        Assert.False(SimReview.RequiresDifferentChecker("published"));
        Assert.False(SimReview.RequiresDifferentChecker("pilot"));
    }
}
