namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — scenario review/publication state machine (Phase 5A, §13). Pure and unit-testable: it
/// only answers "is this review_state transition structurally allowed, and does it need the content
/// validator to pass first". The endpoint layers the real gates on top — the §14 validator for any move
/// toward publication, maker-checker (the approver must differ from the author) at approval, and the audit
/// trail. Keeping the graph here means the workflow can never drift between the API and its tests.
///
/// review_state is the AUTHORING lifecycle and is deliberately separate from the operational `status` that
/// decides what students are served (the endpoint keeps them in sync: publish → status 'published';
/// return-to-draft → 'draft'; retire → 'archived').
/// </summary>
public static class SimReview
{
    public const string Draft = "draft";
    public const string CalcReview = "calc_review";
    public const string LearningReview = "learning_review";
    public const string SafetyReview = "safety_review";
    public const string Pilot = "pilot";
    public const string Approved = "approved";
    public const string Published = "published";
    public const string Retired = "retired";

    /// <summary>The workflow in order. The happy path walks these left-to-right.</summary>
    public static readonly IReadOnlyList<string> States = new[]
    {
        Draft, CalcReview, LearningReview, SafetyReview, Pilot, Approved, Published, Retired,
    };

    static readonly string[] Forward =
    {
        Draft, CalcReview, LearningReview, SafetyReview, Pilot, Approved, Published,
    };

    public static bool IsState(string? s) => s is not null && States.Contains(s);

    /// <summary>Structural adjacency only (no content/permission judgement):
    ///  • one step forward along the workflow (draft → calc_review → … → approved → published);
    ///  • any in-flight state may be sent back to <see cref="Draft"/> (reject / return for edits);
    ///  • a published scenario may be <see cref="Retired"/>.
    /// Everything else (skipping a stage, resurrecting a retired version, self-loops) is refused.</summary>
    public static bool CanTransition(string? from, string? to)
    {
        if (!IsState(from) || !IsState(to) || from == to) return false;
        if (to == Draft) return from != Published && from != Retired;   // pull an in-review scenario back
        if (to == Retired) return from == Published;                    // retire only a live version
        var i = Array.IndexOf(Forward, from);
        return i >= 0 && i + 1 < Forward.Length && Forward[i + 1] == to;  // exactly one step forward
    }

    /// <summary>Moving to approved or published must pass the §14 content validator first — you cannot
    /// approve or publish a scenario the engine cannot grade.</summary>
    public static bool RequiresPublishable(string to) => to is Approved or Published;

    /// <summary>Approval is the maker-checker checkpoint: the approver must not be the author.</summary>
    public static bool RequiresDifferentChecker(string to) => to == Approved;
}
