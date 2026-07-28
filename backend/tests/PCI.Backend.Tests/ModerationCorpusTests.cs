using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The benchmark corpus loader (Core/ModerationCorpus.cs) — CCP-P1-004.
///
/// The failure this guards against is a corpus that silently loses rows. Half a corpus that looks
/// whole would produce a calibration report over data nobody intended, and that report would look
/// exactly as convincing as a correct one. So a malformed line is an error carrying its line number,
/// never a row quietly dropped — and every problem is reported at once, because fixing a corpus one
/// discovered error at a time is how people give up on it.
///
/// Fixtures here are ordinary professional sentences. Labels are structural, not real: this file
/// tests the LOADER, and synthesising labelled harmful text would produce a benchmark that measures
/// nothing while looking like it measures something.
/// </summary>
public class ModerationCorpusTests
{
    const string Good = """
        # a comment explaining a labelling decision survives the loader
        {"text":"The float on activity B is eight days.","locale":"en","category":"ordinary","harmful":false}
        {"text":"تقرير التقدم جاهز","locale":"ar","category":"ordinary","harmful":false}
        {"text":"رپورٹ تیار ہے","locale":"ur","category":"ordinary","harmful":false}
        {"text":"s p a c e d   o u t","locale":"en","category":"scam_spam","harmful":true,"obfuscated":true}
        """;

    [Fact]
    public void AWellFormedCorpusLoads()
    {
        var r = ModerationCorpus.Load(Good);
        Assert.True(r.Ok, string.Join(" | ", r.Problems));
        Assert.Equal(4, r.Samples.Count);
    }

    [Fact]
    public void CommentsAndBlankLinesAreIgnoredRatherThanRejected()
    {
        // A corpus should be able to carry the reasoning behind a labelling decision — exactly the
        // context that is lost when the person who made it moves on.
        var r = ModerationCorpus.Load("# why this row is labelled harmful\n\n" + Good);
        Assert.True(r.Ok, string.Join(" | ", r.Problems));
    }

    [Fact]
    public void NonAsciiTextSurvivesIntact()
    {
        // §22 names Arabic and Urdu. A loader that mangled them would produce a corpus that looks
        // like coverage and is not.
        var r = ModerationCorpus.Load(Good);
        Assert.Contains(r.Samples, s => s.Text == "تقرير التقدم جاهز" && s.Locale == "ar");
        Assert.Contains(r.Samples, s => s.Text == "رپورٹ تیار ہے" && s.Locale == "ur");
    }

    // ── nothing is dropped silently ───────────────────────────────────────────────────────────

    [Fact]
    public void AMalformedLineIsAnErrorWithItsLineNumber()
    {
        var r = ModerationCorpus.Load("{\"text\":\"ok\",\"locale\":\"en\",\"category\":\"ordinary\",\"harmful\":false}\nnot json at all\n");
        Assert.False(r.Ok);
        Assert.Contains(r.Problems, p => p.Contains("line 2"));
    }

    [Fact]
    public void AMissingHumanLabelIsRefusedRatherThanDefaulted()
    {
        // The label IS the ground truth. Defaulting it would invent a reviewer's judgement, which is
        // the one thing a corpus loader must never do.
        var r = ModerationCorpus.Load("{\"text\":\"ok\",\"locale\":\"en\",\"category\":\"ordinary\"}");
        Assert.False(r.Ok);
        Assert.Contains(r.Problems, p => p.Contains("harmful"));
        Assert.Empty(r.Samples);
    }

    [Fact]
    public void AMissingLocaleIsRefusedBecauseCoverageCouldNotBeChecked()
    {
        var r = ModerationCorpus.Load("{\"text\":\"ok\",\"category\":\"ordinary\",\"harmful\":false}");
        Assert.False(r.Ok);
        Assert.Contains(r.Problems, p => p.Contains("locale"));
    }

    [Fact]
    public void AMistypedCategoryIsCaughtAtLoadRatherThanBecomingAMystery()
    {
        // Left to the calibration gate, a typo becomes "no category was scored" — technically
        // correct and completely unhelpful to the person who wrote 'hatespeech'.
        var r = ModerationCorpus.Load("{\"text\":\"x\",\"locale\":\"en\",\"category\":\"hatespeech\",\"harmful\":true}");
        Assert.False(r.Ok);
        Assert.Contains(r.Problems, p => p.Contains("hatespeech"));
        Assert.Empty(r.Samples);
    }

    [Fact]
    public void EveryProblemIsReportedInOnePass()
    {
        // Fixing a corpus one discovered error at a time is how people give up on it.
        var bad = """
            {"text":"","locale":"en","category":"ordinary","harmful":false}
            {"text":"x","category":"ordinary","harmful":false}
            {"text":"y","locale":"en","category":"nonsense","harmful":false}
            broken
            """;
        var r = ModerationCorpus.Load(bad);
        Assert.False(r.Ok);
        Assert.True(r.Problems.Count >= 4, "expected all four problems: " + string.Join(" | ", r.Problems));
    }

    [Fact]
    public void AnEmptyCorpusIsAProblemNotAnEmptySuccess()
    {
        var r = ModerationCorpus.Load("\n# only comments\n\n");
        Assert.False(r.Ok);
        Assert.Empty(r.Samples);
    }

    // ── it hands straight to the calibration gate ─────────────────────────────────────────────

    [Fact]
    public void ALoadedCorpusFeedsTheCalibrationGateDirectly()
    {
        // The two halves have to fit: a loaded corpus is exactly what Score() consumes, so the path
        // from "T&S drops in a file" to "the gate certifies or refuses" has no adapter in it.
        var r = ModerationCorpus.Load(Good);
        var scored = r.Samples.Select(s => new ModerationCalibration.Scored(s, s.ExpectedHarmful ? 0.99 : 0.01));
        var report = ModerationCalibration.Score("acme", "1", scored,
            new Dictionary<string, ModerationCalibration.Thresholds>(StringComparer.OrdinalIgnoreCase)
            {
                [CommunityModeration.Categories.ScamSpam] = new(0.5, 0.9),
            });

        Assert.Equal(4, report.SampleCount);
        // Four samples cannot certify anything, and the gate says so rather than being flattered by
        // a perfect score on a tiny set.
        Assert.False(ModerationCalibration.Certify(report).Calibrated);
    }

    [Fact]
    public void DescribeReportsShapeWithoutClaimingTheLabelsAreRight()
    {
        var r = ModerationCorpus.Load(Good);
        var text = ModerationCorpus.Describe(r.Samples);
        Assert.Contains("4 samples", text);
        Assert.Contains("ar=1", text);
        Assert.Contains("ur=1", text);
        Assert.Contains("obfuscated: 1", text);
    }
}
