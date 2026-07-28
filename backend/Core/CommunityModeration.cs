namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — the moderation policy engine (spec §8.4/§8.4.1;
/// docs/pciworld/CCP_PHASE1_DESIGN.md §6).
///
/// THE ONE RULE THIS FILE EXISTS TO ENFORCE: no guest text reaches another participant before an
/// authoritative allow verdict. Everything here is arranged so that the failure modes all land on
/// "not published" rather than "published unchecked".
///
/// The policy is DATA, not control flow. A decision resolves
/// <c>content type × category × severity × confidence band × context × repetition → outcome</c>
/// against versioned rows, and records WHICH row produced it. That is what makes "why was this
/// blocked in March?" answerable against the policy that was actually live at the time, and what
/// lets a safety lead restage thresholds without a deploy. Scattering this across if-statements
/// would make both impossible.
///
/// FAIL-CLOSED IS STRUCTURAL, NOT CONDITIONAL. Three distinct things can go wrong — no provider is
/// configured, the provider errored or timed out, or no rule matched — and all three resolve to a
/// non-publishing outcome. There is deliberately no code path in which an absent, broken or
/// unmatched classification yields Allow (§15, §28.4).
///
/// WHAT THIS IS NOT. Confidence bands are only as good as their calibration, and nothing is
/// calibrated until a provider is contracted and benchmarked on PCI's own English/Arabic/Urdu and
/// obfuscation corpora (CCP-P1-004). Until then the shipped matrix is a defensible default, not a
/// tuned one, and §28.5 forbids letting a low-confidence signal produce an irreversible sanction —
/// which is why every Low band in the default matrix quarantines for human review rather than ejects.
/// </summary>
public static class CommunityModeration
{
    // ── Contracts ─────────────────────────────────────────────────────────────────────────────

    /// <summary>What the policy decided. Block/Quarantine/Eject/Escalate all mean "not published";
    /// they differ in what happens to the AUTHOR and to the review queue, not to the message.</summary>
    public enum Outcome
    {
        /// <summary>Publish. The only outcome that ever reaches another participant.</summary>
        Allow,
        /// <summary>Never published. The author is told, with a safe reason code.</summary>
        Block,
        /// <summary>Never published, but invisible rather than refused: queued for human review with
        /// bounded context. The outcome for genuine uncertainty (§8.4).</summary>
        Quarantine,
        /// <summary>Blocked, and the author's room session ends. Reserved for high-confidence severe
        /// violations — an irreversible-feeling action that a low-confidence signal must never reach.</summary>
        Eject,
        /// <summary>Blocked and routed to restricted legal/safety handling, OUT of the ordinary
        /// moderator queue. Evidence is not shown in normal review surfaces (§8.7, §28.7).</summary>
        Escalate,
    }

    /// <summary>Calibrated confidence. Deliberately three coarse bands rather than a raw score:
    /// §8.4.1 warns a provider's raw number does not mean the same thing as another's, so the
    /// mapping from score to band belongs in the provider adapter, versioned, not in this engine.</summary>
    public enum Band { Low, Medium, High }

    /// <summary>A classifier's read on one piece of text.</summary>
    /// <param name="Category">Policy category, e.g. <c>ordinary</c>, <c>hate</c>, <c>grooming</c>.</param>
    /// <param name="Severity">Provider-reported severity: low|medium|high. Recorded, not decisive.</param>
    /// <param name="Confidence">The calibrated band the outcome actually turns on.</param>
    /// <param name="ContextRule">A recognised mitigating context (<c>educational</c>, <c>quoted</c>,
    /// <c>news</c>) or null. Never overrides a severe rule — see the default matrix.</param>
    /// <param name="Usable">False when the provider could not produce a verdict at all: absent,
    /// errored, timed out. The engine treats this as "unknown", which fails closed.</param>
    public readonly record struct Signal(
        string Category, string Severity, Band Confidence, string? ContextRule, bool Usable)
    {
        public static Signal Unavailable => new("unknown", "unknown", Band.Low, null, false);
        public static Signal Clean(Band confidence = Band.High) => new("ordinary", "low", confidence, null, true);
    }

    /// <summary>One row of the versioned policy matrix.</summary>
    public readonly record struct Rule(
        long Id, string ContentType, string Category, string Severity, Band Confidence,
        string? ContextRule, int RepetitionMin, Outcome Outcome, string ReasonCode, int Sort);

    /// <summary>The engine's verdict, carrying everything a decision row needs so the audit trail can
    /// reconstruct the reasoning without re-running the classifier.</summary>
    public readonly record struct Decision(
        Outcome Outcome, string ReasonCode, long? RuleId, string Category, Band Confidence,
        string? ContextRule, int Repetition)
    {
        /// <summary>Whether this verdict permits publication. The single place that question is
        /// answered, so a caller cannot accidentally treat Quarantine as publishable.</summary>
        public bool Publishes => Outcome == Outcome.Allow;
    }

    /// <summary>The provider seam. A real vendor, the deterministic test double and the
    /// nothing-configured default all sit behind this, so swapping one is configuration.</summary>
    public interface ITextModerator
    {
        string ProviderName { get; }
        string ProviderVersion { get; }
        /// <summary>Classify already-normalised text. Implementations must NOT throw: an internal
        /// failure is reported as <see cref="Signal.Unavailable"/> so it fails closed rather than
        /// crashing the send path.</summary>
        Signal Classify(string normalizedText, string locale);
    }

    // ── Resolution ────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Resolve a signal against the active policy. Pure: no database, no clock, no I/O, so the whole
    /// matrix is unit-testable row by row.
    ///
    /// Matching is most-specific-first via <c>Sort</c>. A rule with a ContextRule only matches a
    /// signal carrying that context; a rule without one matches any context. RepetitionMin lets a
    /// repeat offender escalate deterministically without the classifier changing its mind.
    /// </summary>
    public static Decision Resolve(IReadOnlyList<Rule> rules, Signal signal, int repetition = 0, string contentType = "text")
    {
        // 1. No usable classification — absent provider, error, timeout. The room should already be
        //    read-only when this is systemic (§15), but a single unusable verdict must never publish.
        if (!signal.Usable)
            return new Decision(Outcome.Quarantine, "moderation_unavailable", null,
                                signal.Category, signal.Confidence, signal.ContextRule, repetition);

        Rule? best = null;
        foreach (var r in rules)
        {
            if (!string.Equals(r.ContentType, contentType, StringComparison.OrdinalIgnoreCase)) continue;
            if (!string.Equals(r.Category, signal.Category, StringComparison.OrdinalIgnoreCase)) continue;
            if (r.Confidence != signal.Confidence) continue;
            if (r.RepetitionMin > repetition) continue;
            // A context-specific rule requires that exact context; a general rule accepts anything.
            if (r.ContextRule is not null
                && !string.Equals(r.ContextRule, signal.ContextRule, StringComparison.OrdinalIgnoreCase)) continue;
            if (best is null || r.Sort < best.Value.Sort
                || (r.Sort == best.Value.Sort && r.RepetitionMin > best.Value.RepetitionMin))
                best = r;
        }

        // 2. The classifier produced a category the live policy has no row for. That is a policy
        //    gap, not permission: a category nobody has ruled on is exactly the case a human should
        //    see. Quarantine, and the reason code makes the gap visible in the admin queue.
        if (best is null)
            return new Decision(Outcome.Quarantine, "no_policy_rule", null,
                                signal.Category, signal.Confidence, signal.ContextRule, repetition);

        var rule = best.Value;
        return new Decision(rule.Outcome, rule.ReasonCode, rule.Id,
                            signal.Category, signal.Confidence, signal.ContextRule, repetition);
    }

    // ── The default matrix (§8.4.1) ───────────────────────────────────────────────────────────

    /// <summary>Policy categories the engine and the seeded matrix agree on.</summary>
    public static class Categories
    {
        public const string Ordinary = "ordinary";
        public const string ProfanityMild = "profanity_mild";
        public const string TargetedAbuse = "targeted_abuse";
        public const string Hate = "hate";
        public const string Sexual = "sexual";
        public const string Violence = "violence";
        public const string SelfHarm = "self_harm";
        public const string Grooming = "grooming";
        public const string IllegalGoods = "illegal_goods";
        public const string ScamSpam = "scam_spam";
        public const string PiiDoxxing = "pii_doxxing";
        public const string Copyright = "copyright";

        public static readonly string[] All =
        {
            Ordinary, ProfanityMild, TargetedAbuse, Hate, Sexual, Violence,
            SelfHarm, Grooming, IllegalGoods, ScamSpam, PiiDoxxing, Copyright,
        };
    }

    /// <summary>
    /// The strict guest-room default from §8.4.1, as data. Ids are assigned by the seeder; Sort
    /// orders specificity (lower wins), so the context-qualified rows must sort ahead of the
    /// general ones.
    ///
    /// Read the shape rather than the individual rows: Low confidence never ejects anywhere, High
    /// confidence never merely quarantines a severe category, and the mitigating-context rows exist
    /// only at Low/Medium — a genuine grooming or credible-threat signal is not talked out of by
    /// claiming the message was educational (§8.4.1's final row).
    /// </summary>
    public static IReadOnlyList<Rule> DefaultMatrix()
    {
        var rows = new List<Rule>();
        var id = 0L;
        void Add(string category, Band band, Outcome outcome, string reason,
                 string? context = null, int repetitionMin = 0, int sort = 100)
            => rows.Add(new Rule(++id, "text", category, SeverityFor(band), band,
                                 context, repetitionMin, outcome, reason, sort));

        // Ordinary professional content publishes at every band. Without this row the engine would
        // quarantine everything as an unmatched category — correct as a fail-closed default, useless
        // as a product.
        Add(Categories.Ordinary, Band.Low, Outcome.Allow, "ok");
        Add(Categories.Ordinary, Band.Medium, Outcome.Allow, "ok");
        Add(Categories.Ordinary, Band.High, Outcome.Allow, "ok");

        // Mild profanity. Quoted or educational use is the classic false positive, so it is held for
        // a human rather than refused; a confident direct hit is blocked, and in a strict guest room
        // a high-confidence hit ends the session.
        Add(Categories.ProfanityMild, Band.Low, Outcome.Quarantine, "profanity_review");
        Add(Categories.ProfanityMild, Band.Medium, Outcome.Quarantine, "profanity_context_review", context: "quoted", sort: 50);
        Add(Categories.ProfanityMild, Band.Medium, Outcome.Quarantine, "profanity_context_review", context: "educational", sort: 50);
        Add(Categories.ProfanityMild, Band.Medium, Outcome.Block, "profanity_blocked");
        Add(Categories.ProfanityMild, Band.High, Outcome.Eject, "profanity_severe");

        // Targeted abuse and hate. Repetition escalates deterministically: a second medium-confidence
        // hit ejects without needing the classifier to become more certain.
        Add(Categories.TargetedAbuse, Band.Low, Outcome.Quarantine, "abuse_review");
        Add(Categories.TargetedAbuse, Band.Medium, Outcome.Block, "abuse_blocked");
        Add(Categories.TargetedAbuse, Band.Medium, Outcome.Eject, "abuse_repeated", repetitionMin: 2, sort: 40);
        Add(Categories.TargetedAbuse, Band.High, Outcome.Eject, "abuse_severe");

        Add(Categories.Hate, Band.Low, Outcome.Quarantine, "hate_review");
        Add(Categories.Hate, Band.Medium, Outcome.Block, "hate_blocked");
        Add(Categories.Hate, Band.High, Outcome.Escalate, "hate_severe");

        // Sexual content. High confidence escalates rather than merely ejecting, because the minors
        // and grooming adjacency is exactly what §19.3 requires trained handling for.
        Add(Categories.Sexual, Band.Low, Outcome.Quarantine, "sexual_review");
        Add(Categories.Sexual, Band.Medium, Outcome.Block, "sexual_blocked");
        Add(Categories.Sexual, Band.High, Outcome.Escalate, "sexual_severe");

        // Violence. News and educational discussion of violence is legitimate professional content;
        // a credible threat is not, and no context row exists at High to excuse one.
        Add(Categories.Violence, Band.Low, Outcome.Allow, "violence_context_ok", context: "news", sort: 50);
        Add(Categories.Violence, Band.Low, Outcome.Quarantine, "violence_review");
        Add(Categories.Violence, Band.Medium, Outcome.Block, "violence_blocked");
        Add(Categories.Violence, Band.High, Outcome.Escalate, "violence_credible_threat");

        // Self-harm. The one category where the safe default is NOT suppression: people discussing
        // their own struggle need support, not silence. Only encouragement and instruction are
        // blocked, and the severe band routes to the crisis runbook rather than punishing the author.
        Add(Categories.SelfHarm, Band.Low, Outcome.Allow, "self_harm_support_ok");
        Add(Categories.SelfHarm, Band.Medium, Outcome.Quarantine, "self_harm_review");
        Add(Categories.SelfHarm, Band.High, Outcome.Escalate, "self_harm_crisis");

        // Grooming and child-safety indicators. Restricted handling at every band above Low, and
        // NEVER an ordinary block — this evidence must not appear in the routine queue (§8.7).
        Add(Categories.Grooming, Band.Low, Outcome.Quarantine, "grooming_review");
        Add(Categories.Grooming, Band.Medium, Outcome.Escalate, "grooming_restricted");
        Add(Categories.Grooming, Band.High, Outcome.Escalate, "grooming_severe");

        // Illegal goods. Legitimate professional and news discussion is allowed; solicitation is not.
        Add(Categories.IllegalGoods, Band.Low, Outcome.Allow, "illegal_context_ok", context: "news", sort: 50);
        Add(Categories.IllegalGoods, Band.Low, Outcome.Quarantine, "illegal_review");
        Add(Categories.IllegalGoods, Band.Medium, Outcome.Block, "illegal_blocked");
        Add(Categories.IllegalGoods, Band.High, Outcome.Eject, "illegal_solicitation");

        // Scam and spam. Coordinated abuse ejects; a single suspected promo is blocked.
        Add(Categories.ScamSpam, Band.Low, Outcome.Quarantine, "spam_review");
        Add(Categories.ScamSpam, Band.Medium, Outcome.Block, "spam_blocked");
        Add(Categories.ScamSpam, Band.Medium, Outcome.Eject, "spam_repeated", repetitionMin: 3, sort: 40);
        Add(Categories.ScamSpam, Band.High, Outcome.Eject, "spam_coordinated");

        // Personal data and doxxing. Blocked even at Low, because the harm of publishing someone's
        // phone number is immediate and not undone by a later review — the author is told what to do
        // instead. This is the one place the matrix is stricter at Low than elsewhere, deliberately.
        Add(Categories.PiiDoxxing, Band.Low, Outcome.Block, "pii_blocked");
        Add(Categories.PiiDoxxing, Band.Medium, Outcome.Block, "pii_blocked");
        Add(Categories.PiiDoxxing, Band.High, Outcome.Escalate, "pii_doxxing_severe");

        // Rights concerns are held for review rather than punished — a rights question is a
        // publication decision, not participant misconduct.
        Add(Categories.Copyright, Band.Low, Outcome.Quarantine, "rights_review");
        Add(Categories.Copyright, Band.Medium, Outcome.Quarantine, "rights_review");
        Add(Categories.Copyright, Band.High, Outcome.Block, "rights_refused");

        return rows;
    }

    /// <summary>
    /// The image half of the matrix (Phase 2 — docs/pciworld/CCP_PHASE2_DESIGN.md §6.2). Same engine,
    /// same <see cref="Resolve"/>, <c>contentType="image"</c>; only the rows differ.
    ///
    /// TWO DELIBERATE DIFFERENCES FROM THE TEXT MATRIX, both of which are safety properties rather
    /// than taste:
    ///
    /// 1. **No image row ejects.** §28.5 forbids turning a classifier result into an irreversible
    ///    automatic sanction, and image classification is materially less reliable than text
    ///    classification — a photograph is ambiguous in ways a slur is not. A confident severe image
    ///    signal therefore withholds the image and opens a case for a human; ending someone's session
    ///    on an image signal alone is a decision a person makes, not one this table makes. There is a
    ///    test asserting this over the whole table, so it cannot be eroded a row at a time.
    ///
    /// 2. **No mitigating-context rows.** "Educational" or "quoted" are claims a text classifier can
    ///    find evidence for in the surrounding words. For a bare image there are no surrounding words,
    ///    so a context row here would be an excuse with nothing behind it — the weakest link in the
    ///    table, and precisely the one an abuser would aim at.
    ///
    /// The bands these rows key on are UNCALIBRATED until a provider is contracted and benchmarked
    /// (CCP-P1-004). That is why nothing here is irreversible.
    /// </summary>
    public static IReadOnlyList<Rule> DefaultImageMatrix()
    {
        var rows = new List<Rule>();
        var id = 0L;
        void Add(string category, Band band, Outcome outcome, string reason, int sort = 100)
            => rows.Add(new Rule(++id, "image", category, SeverityFor(band), band,
                                 null, 0, outcome, reason, sort));

        // An ordinary image publishes. Without this the engine would withhold everything as an
        // unmatched category — the right fail-closed default, and a useless product.
        Add(Categories.Ordinary, Band.Low, Outcome.Allow, "ok");
        Add(Categories.Ordinary, Band.Medium, Outcome.Allow, "ok");
        Add(Categories.Ordinary, Band.High, Outcome.Allow, "ok");

        // Sexual imagery. High confidence escalates to restricted handling rather than sitting in the
        // ordinary queue: the minors adjacency is exactly what §19.3 requires trained people for, and
        // §28.7 forbids such material appearing in a routine admin thumbnail.
        Add(Categories.Sexual, Band.Low, Outcome.Quarantine, "sexual_review");
        Add(Categories.Sexual, Band.Medium, Outcome.Quarantine, "sexual_review");
        Add(Categories.Sexual, Band.High, Outcome.Escalate, "sexual_restricted");

        // Child-safety indicators go to restricted handling from Medium up and are NEVER an ordinary
        // block, so this evidence never reaches the routine moderator queue (§8.7).
        Add(Categories.Grooming, Band.Low, Outcome.Quarantine, "child_safety_review");
        Add(Categories.Grooming, Band.Medium, Outcome.Escalate, "child_safety_restricted");
        Add(Categories.Grooming, Band.High, Outcome.Escalate, "child_safety_restricted");

        Add(Categories.Hate, Band.Low, Outcome.Quarantine, "hate_review");
        Add(Categories.Hate, Band.Medium, Outcome.Block, "hate_blocked");
        Add(Categories.Hate, Band.High, Outcome.Escalate, "hate_severe");

        Add(Categories.Violence, Band.Low, Outcome.Quarantine, "violence_review");
        Add(Categories.Violence, Band.Medium, Outcome.Block, "violence_blocked");
        Add(Categories.Violence, Band.High, Outcome.Escalate, "violence_graphic");

        // Self-harm imagery is treated differently from self-harm TEXT, on purpose. Text describing
        // someone's own struggle is a request for support and suppressing it is a harm; a graphic
        // image of self-injury is not the same artefact and is not published while a human looks.
        Add(Categories.SelfHarm, Band.Low, Outcome.Quarantine, "self_harm_review");
        Add(Categories.SelfHarm, Band.Medium, Outcome.Quarantine, "self_harm_review");
        Add(Categories.SelfHarm, Band.High, Outcome.Escalate, "self_harm_crisis");

        // A screenshot of somebody's passport, payslip or address does its damage the moment it is
        // visible, and a later review does not undo it — so it is withheld at every band, exactly as
        // the text matrix is stricter at Low for doxxing.
        Add(Categories.PiiDoxxing, Band.Low, Outcome.Block, "pii_blocked");
        Add(Categories.PiiDoxxing, Band.Medium, Outcome.Block, "pii_blocked");
        Add(Categories.PiiDoxxing, Band.High, Outcome.Escalate, "pii_doxxing_severe");

        Add(Categories.IllegalGoods, Band.Low, Outcome.Quarantine, "illegal_review");
        Add(Categories.IllegalGoods, Band.Medium, Outcome.Block, "illegal_blocked");
        Add(Categories.IllegalGoods, Band.High, Outcome.Block, "illegal_blocked");

        Add(Categories.ScamSpam, Band.Low, Outcome.Quarantine, "spam_review");
        Add(Categories.ScamSpam, Band.Medium, Outcome.Block, "spam_blocked");
        Add(Categories.ScamSpam, Band.High, Outcome.Block, "spam_blocked");

        // A rights question is a publication decision, not participant misconduct.
        Add(Categories.Copyright, Band.Low, Outcome.Quarantine, "rights_review");
        Add(Categories.Copyright, Band.Medium, Outcome.Quarantine, "rights_review");
        Add(Categories.Copyright, Band.High, Outcome.Block, "rights_refused");

        return rows;
    }

    static string SeverityFor(Band band) => band switch
    {
        Band.High => "high",
        Band.Medium => "medium",
        _ => "low",
    };
}
