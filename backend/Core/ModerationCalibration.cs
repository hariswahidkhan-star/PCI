namespace PCI.Backend.Core;

/// <summary>
/// Turning a provider's raw scores into the calibrated bands the policy matrix keys on — the
/// engineering limb of CCP-P1-004 (§8.4, §8.4.1).
///
/// WHY THIS EXISTS AS A MODULE RATHER THAN A CONSTANT.
///
/// §8.4.1 warns that a raw score from one provider does not mean what another's does. 0.8 from one
/// vendor may be a near-certainty and from another a shrug. So the mapping from score to band is
/// per-provider data, versioned, and — this is the part that matters — it is only allowed to be
/// called "calibrated" when a labelled corpus says the thresholds actually behave.
///
/// THE GATE IS THE POINT, NOT THE MAPPING.
///
/// Anyone can pick thresholds. What §28.5 forbids is letting an untuned high band drive an
/// irreversible sanction, and the failure mode that produces is dull and predictable: someone sets
/// thresholds by eye, the code starts calling them calibrated because a field says so, and the
/// system begins ejecting people on a number nobody validated.
///
/// <see cref="Certify"/> exists to make that impossible to do by accident. A band set is certified
/// only if a corpus run clears explicit bars, and <see cref="CalibrationStatus.Calibrated"/> is
/// computed from evidence rather than asserted by configuration. A caller that wants to know
/// whether it may sanction asks <see cref="MaySanctionOn"/>, which answers false for everything
/// uncertified — so the safe behaviour is what happens when nobody has done the work yet.
///
/// WHAT THIS MODULE CANNOT DO.
///
/// It cannot supply the corpus. A benchmark for English, Arabic, Urdu, code-switched and obfuscated
/// text is Trust &amp; Safety's to author and label, and no amount of engineering substitutes for
/// real labelled examples. What is built here is the harness, the scoring, the thresholds and the
/// refusal — so that the day a corpus and a contracted provider exist, calibrating is running a
/// report rather than writing a system.
/// </summary>
public static class ModerationCalibration
{
    /// <summary>One labelled example. <paramref name="ExpectedHarmful"/> is the human label — what a
    /// trained reviewer said — and is the ground truth everything is scored against.</summary>
    /// <param name="Text">The example. Kept out of logs and telemetry by callers.</param>
    /// <param name="Locale">BCP-47-ish tag. Present so coverage can be checked per language rather
    /// than assumed from an aggregate that an English-only corpus would pass.</param>
    /// <param name="Category">The policy category the label belongs to.</param>
    /// <param name="ExpectedHarmful">True when a reviewer judged this a genuine violation.</param>
    /// <param name="Obfuscated">Marks deliberately disguised examples, so coverage of evasion is
    /// checked separately — a corpus of only plain text would certify a classifier that any
    /// determined author defeats.</param>
    public readonly record struct Sample(
        string Text, string Locale, string Category, bool ExpectedHarmful, bool Obfuscated);

    /// <summary>What the provider said about one sample: a raw score in [0,1].</summary>
    public readonly record struct Scored(Sample Sample, double RawScore);

    /// <summary>The score boundaries for one category. Below <paramref name="Medium"/> is Low.</summary>
    public readonly record struct Thresholds(double Medium, double High)
    {
        public CommunityModeration.Band BandFor(double score) =>
            score >= High ? CommunityModeration.Band.High
            : score >= Medium ? CommunityModeration.Band.Medium
            : CommunityModeration.Band.Low;
    }

    public readonly record struct Counts(int TruePositive, int FalsePositive, int TrueNegative, int FalseNegative)
    {
        public int Total => TruePositive + FalsePositive + TrueNegative + FalseNegative;
        /// <summary>Of what we flagged at this band, how much was genuinely harmful. This is the
        /// number that matters for a band that drives a sanction — a low precision means punishing
        /// innocent people.</summary>
        public double Precision => TruePositive + FalsePositive == 0 ? 0 : (double)TruePositive / (TruePositive + FalsePositive);
        /// <summary>Of what was genuinely harmful, how much we caught.</summary>
        public double Recall => TruePositive + FalseNegative == 0 ? 0 : (double)TruePositive / (TruePositive + FalseNegative);
    }

    /// <summary>The outcome of running a corpus through a provider at a given set of thresholds.</summary>
    public readonly record struct Report(
        string Provider, string ProviderVersion, int SampleCount,
        IReadOnlyDictionary<string, Counts> HighBandByCategory,
        IReadOnlySet<string> Locales, int ObfuscatedSamples);

    public readonly record struct CalibrationStatus(bool Calibrated, IReadOnlyList<string> Shortfalls)
    {
        /// <summary>Whether an automated sanction may be driven by these bands. False whenever the
        /// evidence is missing, which is the whole point: the safe answer is the default.</summary>
        public bool MaySanction => Calibrated;
    }

    // ── the bars ──────────────────────────────────────────────────────────────────────────────
    //
    // Stated as named constants rather than inline numbers so a change to any of them is a visible
    // decision in a diff, not a tweak buried in a condition.

    /// <summary>Minimum samples before a report means anything. A handful of examples can make any
    /// threshold look perfect.</summary>
    public const int MinSamples = 200;

    /// <summary>Minimum samples per category that the High band is allowed to act on. A category
    /// with three examples has not been measured, whatever the aggregate says.</summary>
    public const int MinSamplesPerCategory = 30;

    /// <summary>Precision the High band must reach before it may drive a sanction. High because a
    /// false positive here costs somebody their session or their standing.</summary>
    public const double MinHighBandPrecision = 0.95;

    /// <summary>Languages the corpus must actually contain. §22 names these, and an aggregate over
    /// an English-only corpus would certify a classifier that fails everyone else.</summary>
    public static readonly string[] RequiredLocales = { "en", "ar", "ur" };

    /// <summary>Minimum deliberately-disguised examples. A corpus of only plain text certifies a
    /// classifier that any determined author defeats by spacing out a word.</summary>
    public const int MinObfuscatedSamples = 30;

    /// <summary>
    /// Score a corpus at the given thresholds. Pure: no clock, no database, no network — the
    /// provider has already been called and its scores are the input, so the arithmetic is testable
    /// on fixtures.
    /// </summary>
    public static Report Score(string provider, string providerVersion,
                               IEnumerable<Scored> scored,
                               IReadOnlyDictionary<string, Thresholds> thresholds)
    {
        var byCategory = new Dictionary<string, Counts>(StringComparer.OrdinalIgnoreCase);
        var locales = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var total = 0;
        var obfuscated = 0;

        foreach (var s in scored)
        {
            total++;
            locales.Add(Language(s.Sample.Locale));
            if (s.Sample.Obfuscated) obfuscated++;

            // A category with no configured thresholds is not scored as "everything is Low" — it is
            // not scored at all, so a missing configuration cannot look like good performance.
            if (!thresholds.TryGetValue(s.Sample.Category, out var t)) continue;

            var flaggedHigh = t.BandFor(s.RawScore) == CommunityModeration.Band.High;
            byCategory.TryGetValue(s.Sample.Category, out var c);
            byCategory[s.Sample.Category] = (flaggedHigh, s.Sample.ExpectedHarmful) switch
            {
                (true, true) => c with { TruePositive = c.TruePositive + 1 },
                (true, false) => c with { FalsePositive = c.FalsePositive + 1 },
                (false, true) => c with { FalseNegative = c.FalseNegative + 1 },
                (false, false) => c with { TrueNegative = c.TrueNegative + 1 },
            };
        }

        return new Report(provider, providerVersion, total, byCategory, locales, obfuscated);
    }

    /// <summary>
    /// Decide whether these bands may be called calibrated. Returns every shortfall rather than the
    /// first, so somebody preparing a corpus is told all of what is missing in one pass instead of
    /// discovering it one run at a time.
    /// </summary>
    public static CalibrationStatus Certify(Report report)
    {
        var shortfalls = new List<string>();

        if (report.SampleCount < MinSamples)
            shortfalls.Add($"corpus has {report.SampleCount} samples; at least {MinSamples} are needed");

        foreach (var locale in RequiredLocales)
            if (!report.Locales.Contains(locale))
                shortfalls.Add($"corpus contains no '{locale}' samples (§22)");

        if (report.ObfuscatedSamples < MinObfuscatedSamples)
            shortfalls.Add($"corpus has {report.ObfuscatedSamples} obfuscated samples; at least {MinObfuscatedSamples} are needed");

        if (report.HighBandByCategory.Count == 0)
            shortfalls.Add("no category was scored — thresholds are not configured for anything in the corpus");

        foreach (var (category, counts) in report.HighBandByCategory.OrderBy(kv => kv.Key))
        {
            if (counts.Total < MinSamplesPerCategory)
            {
                shortfalls.Add($"'{category}' has {counts.Total} samples; at least {MinSamplesPerCategory} are needed");
                continue;
            }
            if (counts.Precision < MinHighBandPrecision)
                shortfalls.Add(
                    $"'{category}' high-band precision is {counts.Precision:P1}; at least {MinHighBandPrecision:P0} is required");
        }

        return new CalibrationStatus(shortfalls.Count == 0, shortfalls);
    }

    /// <summary>
    /// Whether an automated, irreversible sanction may be driven by this provider's High band.
    ///
    /// The answer is NO unless a corpus run certified it. §28.5 forbids turning a low-confidence
    /// result into an irreversible automatic sanction, and "we never ran the corpus" is the lowest
    /// confidence there is.
    /// </summary>
    public static bool MaySanctionOn(CalibrationStatus status) => status.MaySanction;

    /// <summary>Primary language subtag, so 'ar-AE' and 'ar' count as the same coverage.</summary>
    static string Language(string? locale)
    {
        var l = (locale ?? "").Trim();
        var dash = l.IndexOfAny(new[] { '-', '_' });
        return (dash > 0 ? l[..dash] : l).ToLowerInvariant();
    }
}
