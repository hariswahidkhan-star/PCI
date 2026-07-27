using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Core;

/// <summary>
/// The two <see cref="CommunityModeration.ITextModerator"/> implementations that ship in Phase 1.
/// Neither is a content-safety vendor, and neither pretends to be — a real provider is a third
/// implementation added once one is contracted and benchmarked (CCP-P1-004). §28 forbids describing
/// a wordlist as adequate moderation, so the limits are stated here rather than implied away.
/// </summary>
public static class CommunityModerators
{
    /// <summary>
    /// The default when nothing is configured: it classifies NOTHING.
    ///
    /// This is not a stub that got left in — it is the fail-closed posture. Every verdict is
    /// <see cref="Signal.Unavailable"/>, which the policy engine resolves to quarantine, so with no
    /// provider wired up a guest room accepts messages and publishes none of them. An operator who
    /// has not chosen a moderation provider does not get an unmoderated public room; they get a room
    /// that visibly does not work, which is the correct failure (§15, §28.4).
    /// </summary>
    public sealed class NullModerator : ITextModerator
    {
        public string ProviderName => "none";
        public string ProviderVersion => "0";
        public Signal Classify(string normalizedText, string locale) => Signal.Unavailable;
    }

    /// <summary>
    /// A deterministic, pattern-based moderator used to exercise the state machine end to end —
    /// ejection, quarantine, escalation, appeal — without a vendor account or network call.
    ///
    /// **This is a test double, not a safety control.** It recognises a small set of unambiguous
    /// English terms. It does not cover Arabic, Urdu, code-switching, novel slurs, sarcasm, implied
    /// threats, or anything requiring judgement, and a determined author defeats it trivially. Its
    /// value is that the pipeline around it is provably correct before a real classifier arrives;
    /// wiring it to a production room would be a misuse, which is why the endpoint layer selects it
    /// only under an explicit test/dev setting.
    ///
    /// Matching runs against BOTH folded forms from <see cref="CommunityNames"/>: whole-word matching
    /// on the word-preserving fold (so "classic" is not a hit for a substring), and containment on
    /// the fully collapsed skeleton (so "f u c k" and "f-u-c-k" are). That pairing is the same
    /// obfuscation discipline the display-name rules use.
    /// </summary>
    public sealed class DeterministicModerator : ITextModerator
    {
        public string ProviderName => "deterministic";
        public string ProviderVersion => "1";

        // Terms are written plainly and folded at match time, so one entry covers its disguises.
        static readonly (string[] Terms, string Category, Band Band)[] Table =
        {
            (new[] { "kill yourself", "kys", "go die" },              Categories.SelfHarm,      Band.High),
            (new[] { "i want to die", "i cant go on", "hopeless" },   Categories.SelfHarm,      Band.Medium),
            (new[] { "nigger", "faggot", "kike", "spic", "chink" },   Categories.Hate,          Band.High),
            (new[] { "i will kill you", "ill kill you", "kill you" }, Categories.Violence,      Band.High),
            (new[] { "idiot", "moron", "stupid bitch", "loser" },     Categories.TargetedAbuse, Band.Medium),
            (new[] { "fuck", "shit", "cunt", "bastard", "asshole" },  Categories.ProfanityMild, Band.High),
            (new[] { "send nudes", "sexy pics" },                     Categories.Sexual,        Band.High),
            (new[] { "how old are you", "are your parents home",
                     "dont tell anyone", "our little secret" },       Categories.Grooming,      Band.Medium),
            (new[] { "buy followers", "click this link", "free crypto",
                     "guaranteed returns" },                          Categories.ScamSpam,      Band.Medium),
            (new[] { "buy weed", "sell guns", "fake passport" },      Categories.IllegalGoods,  Band.Medium),
        };

        // A quoting or teaching marker downgrades an otherwise-confident hit into the mitigating
        // context the matrix understands. Crude on purpose: the point is to prove the context path
        // resolves, not to detect pedagogy.
        static readonly string[] ContextMarkers =
        {
            "for example", "the word", "quoting", "he said", "she said",
            "in the article", "according to", "study found",
        };

        public Signal Classify(string normalizedText, string locale)
        {
            // Never throw out of a moderator: the send path treats an exception-free unusable verdict
            // as fail-closed, but an exception would surface as a 500 and lose the message entirely.
            try
            {
                var words = CommunityNames.FoldWords(normalizedText);
                var collapsed = CommunityNames.Fold(normalizedText);
                if (words.Length == 0) return Signal.Clean();

                var padded = " " + words + " ";
                var context = ContextMarkers.Any(m => words.Contains(CommunityNames.FoldWords(m), StringComparison.Ordinal))
                    ? "quoted" : null;

                foreach (var (terms, category, band) in Table)
                    foreach (var term in terms)
                    {
                        var w = CommunityNames.FoldWords(term);
                        var c = CommunityNames.Fold(term);
                        var hit = padded.Contains(" " + w + " ", StringComparison.Ordinal)
                                  || collapsed.Contains(c, StringComparison.Ordinal);
                        if (!hit) continue;

                        // A mitigating context lowers confidence one band rather than clearing the
                        // finding. It can turn a block into a review; it can never turn a severe
                        // category into publication, because the matrix has no context row at High
                        // for grooming, hate, sexual or credible threats.
                        var effective = context is not null && band == Band.High ? Band.Medium : band;
                        return new Signal(category, band.ToString().ToLowerInvariant(), effective, context, true);
                    }

                return Signal.Clean();
            }
            catch
            {
                return Signal.Unavailable;
            }
        }
    }
}
