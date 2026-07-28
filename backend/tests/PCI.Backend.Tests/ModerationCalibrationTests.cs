using PCI.Backend.Core;
using Xunit;
using static PCI.Backend.Core.ModerationCalibration;

namespace PCI.Backend.Tests;

/// <summary>
/// Band calibration and the certification gate (Core/ModerationCalibration.cs) — the engineering
/// limb of CCP-P1-004 (§8.4.1, §28.5).
///
/// Most of these tests try to get an UNCERTIFIED band set past the gate, because that is the
/// failure that matters. Anyone can pick thresholds; what §28.5 forbids is letting untuned ones
/// drive an irreversible sanction, and the way that happens in practice is not malice — it is a
/// small corpus, or an English-only one, or one category with four examples, quietly clearing an
/// aggregate check.
///
/// So the bars are asserted individually and the refusals are asserted to be *complete*: a person
/// preparing a corpus is told all of what is missing in one pass, not one shortfall per run.
/// </summary>
public class ModerationCalibrationTests
{
    static readonly Dictionary<string, Thresholds> Bands = new(StringComparer.OrdinalIgnoreCase)
    {
        [CommunityModeration.Categories.Hate] = new(0.5, 0.9),
        [CommunityModeration.Categories.TargetedAbuse] = new(0.5, 0.9),
    };

    /// <summary>Build a corpus that clears every bar, so an individual test can spoil exactly one
    /// thing and prove that bar is doing work.</summary>
    static List<Scored> GoodCorpus(int perCategory = 120, double highScore = 0.97)
    {
        var list = new List<Scored>();
        var locales = new[] { "en", "ar", "ur" };
        foreach (var category in new[] { CommunityModeration.Categories.Hate, CommunityModeration.Categories.TargetedAbuse })
            for (var i = 0; i < perCategory; i++)
            {
                var harmful = i % 2 == 0;
                list.Add(new Scored(
                    new Sample($"sample-{category}-{i}", locales[i % locales.Length], category,
                               ExpectedHarmful: harmful, Obfuscated: i % 5 == 0),
                    // Harmful examples score above the High threshold; benign ones well below, so a
                    // clean corpus yields perfect precision and the bars can be tested one at a time.
                    harmful ? highScore : 0.05));
            }
        return list;
    }

    static CalibrationStatus Certify(IEnumerable<Scored> corpus) =>
        ModerationCalibration.Certify(Score("acme", "1", corpus, Bands));

    // ── the default is refusal ────────────────────────────────────────────────────────────────

    [Fact]
    public void NothingIsCalibratedUntilACorpusSaysSo()
    {
        // The safe answer is what happens when nobody has done the work yet.
        var status = Certify(Array.Empty<Scored>());
        Assert.False(status.Calibrated);
        Assert.False(MaySanctionOn(status));
        Assert.NotEmpty(status.Shortfalls);
    }

    [Fact]
    public void AGoodCorpusCertifies()
    {
        // The positive case has to exist, or every other test would pass against a gate that simply
        // refuses everything.
        var status = Certify(GoodCorpus());
        Assert.True(status.Calibrated, string.Join(" | ", status.Shortfalls));
        Assert.True(MaySanctionOn(status));
        Assert.Empty(status.Shortfalls);
    }

    // ── each bar, spoiled one at a time ───────────────────────────────────────────────────────

    [Fact]
    public void ATinyCorpusCannotCertify()
    {
        // A handful of examples makes any threshold look perfect.
        var status = Certify(GoodCorpus(perCategory: 20));
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls, s => s.Contains("samples", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void AnEnglishOnlyCorpusCannotCertify()
    {
        // §22 names Arabic and Urdu. An aggregate over English-only data would certify a classifier
        // that fails everyone else, and it would look fine doing it.
        var corpus = GoodCorpus().Select(s => s with { Sample = s.Sample with { Locale = "en" } }).ToList();
        var status = Certify(corpus);
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls, s => s.Contains("'ar'"));
        Assert.Contains(status.Shortfalls, s => s.Contains("'ur'"));
    }

    [Fact]
    public void RegionalVariantsStillCountAsTheirLanguage()
    {
        // 'ar-AE' is Arabic coverage. Requiring the exact tag would be pedantry that pushes people
        // toward mislabelling their corpus.
        var corpus = GoodCorpus().Select((s, i) => s with
        {
            Sample = s.Sample with { Locale = i % 3 == 0 ? "en-GB" : i % 3 == 1 ? "ar-AE" : "ur-PK" },
        }).ToList();
        Assert.True(Certify(corpus).Calibrated);
    }

    [Fact]
    public void ACorpusWithNoDisguisedExamplesCannotCertify()
    {
        // Otherwise the bands are certified against text nobody was trying to sneak past them.
        var corpus = GoodCorpus().Select(s => s with { Sample = s.Sample with { Obfuscated = false } }).ToList();
        var status = Certify(corpus);
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls, s => s.Contains("obfuscated"));
    }

    [Fact]
    public void APoorlyPerformingHighBandCannotCertify()
    {
        // The number that matters for a band that drives a sanction: of what we flagged, how much
        // was genuinely harmful. Here half the benign samples also score high.
        // Make the BENIGN samples score high too, so the band flags them and precision collapses.
        // (My first attempt raised the score on the harmful ones, which left precision at 1.0 and
        // proved nothing — the fixture has to break the thing the test claims to measure.)
        var corpus = GoodCorpus()
            .Select(s => s.Sample.ExpectedHarmful ? s : s with { RawScore = 0.97 })
            .ToList();
        var status = Certify(corpus);
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls, s => s.Contains("precision"));
    }

    [Fact]
    public void AThinlyCoveredCategoryCannotCertifyEvenWhenTheAggregateIsLarge()
    {
        // The trap this exists for: one enormous well-covered category carrying a second that has
        // four examples. The aggregate looks fine; the thin category has not been measured.
        var corpus = GoodCorpus();
        var trimmed = corpus.Where(s => s.Sample.Category != CommunityModeration.Categories.TargetedAbuse)
            .Concat(corpus.Where(s => s.Sample.Category == CommunityModeration.Categories.TargetedAbuse).Take(5))
            .ToList();

        var status = Certify(trimmed);
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls,
            s => s.Contains(CommunityModeration.Categories.TargetedAbuse) && s.Contains("samples"));
    }

    [Fact]
    public void AnUnconfiguredCategoryIsNotScoredAsGoodPerformance()
    {
        // A category the thresholds do not mention must not be counted as a pile of correct Low
        // verdicts — a missing configuration would then look like excellent behaviour.
        var corpus = GoodCorpus().Select(s => s with
        {
            Sample = s.Sample with { Category = CommunityModeration.Categories.Grooming },
        }).ToList();
        var report = Score("acme", "1", corpus, Bands);
        Assert.Empty(report.HighBandByCategory);

        var status = ModerationCalibration.Certify(report);
        Assert.False(status.Calibrated);
        Assert.Contains(status.Shortfalls, s => s.Contains("no category was scored"));
    }

    // ── the report tells you everything at once ───────────────────────────────────────────────

    [Fact]
    public void EveryShortfallIsReportedTogetherRatherThanOneAtATime()
    {
        // Someone preparing a corpus should not have to discover the bars by running into them
        // sequentially over a week.
        var poor = GoodCorpus(perCategory: 10)
            .Select(s => s with { Sample = s.Sample with { Locale = "en", Obfuscated = false } })
            .ToList();
        var status = Certify(poor);
        Assert.False(status.Calibrated);
        Assert.True(status.Shortfalls.Count >= 4,
            "expected size, locale and obfuscation shortfalls together: " + string.Join(" | ", status.Shortfalls));
    }

    // ── the arithmetic itself ─────────────────────────────────────────────────────────────────

    [Fact]
    public void ThresholdsMapScoresToTheBandTheyName()
    {
        var t = new Thresholds(0.5, 0.9);
        Assert.Equal(CommunityModeration.Band.Low, t.BandFor(0.49));
        Assert.Equal(CommunityModeration.Band.Medium, t.BandFor(0.5));
        Assert.Equal(CommunityModeration.Band.Medium, t.BandFor(0.89));
        Assert.Equal(CommunityModeration.Band.High, t.BandFor(0.9));
        Assert.Equal(CommunityModeration.Band.High, t.BandFor(1.0));
    }

    [Fact]
    public void PrecisionAndRecallAreZeroRatherThanUndefinedWhenNothingWasFlagged()
    {
        // A divide-by-zero here would surface as NaN, and NaN compares false against every
        // threshold — which would silently mean "not calibrated" for the right reason by accident.
        // Being explicit costs nothing and removes the accident.
        var empty = new Counts(0, 0, 0, 0);
        Assert.Equal(0, empty.Precision);
        Assert.Equal(0, empty.Recall);
    }

    [Fact]
    public void ThresholdsFromOneProviderAreNotAssumedToMeanTheSameForAnother()
    {
        // §8.4.1's warning, made concrete: the same scores under different thresholds give
        // different bands, which is exactly why the mapping is per-provider data.
        var strict = new Thresholds(0.6, 0.95);
        var loose = new Thresholds(0.2, 0.4);
        Assert.Equal(CommunityModeration.Band.Medium, strict.BandFor(0.8));
        Assert.Equal(CommunityModeration.Band.High, loose.BandFor(0.8));
    }

    [Fact]
    public void TheReportNamesTheProviderAndVersionItWasProducedAgainst()
    {
        var report = Score("acme-safety", "2026.07", GoodCorpus(), Bands);
        Assert.Equal("acme-safety", report.Provider);
        Assert.Equal("2026.07", report.ProviderVersion);
    }
}
