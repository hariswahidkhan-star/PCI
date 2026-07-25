using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Question-bank answer-key validation (calc-validation Phase 2 gate).
///
/// The three committed MCQ banks — the Certuvo practice pack (certuvo_seed.json, 40 items), the
/// opt-in demo live-exam pack (demo_exam_seed.json, 24 items) and the public study download
/// (docs/downloads/sample-questions.md, 16 items) — previously had NO validation that their stored
/// answer keys are actually correct. These tests close that gap two ways:
///
///  1. Structural gates: every item has a question, four non-empty pairwise-distinct options, an
///     in-range answer_index, a domain, and (practice pack) an explanation; no distractor can
///     silently equal the keyed answer; no retired certification name appears in served text.
///
///  2. Independent golden derivations: every CALCULATION question's expected result is re-derived
///     here from first principles with decimal arithmetic (never by calling platform code and never
///     by trusting the stored key), and the key must point at — and only at — the option carrying
///     that independently computed value. If anyone edits a key, an option value, or the question's
///     inputs, this fails loudly naming the item.
///
/// Methodology note: the stored answer key is treated as the CLAIM and this file as the independent
/// second derivation, per the platform's "never assume the stored answer key is correct" standard.
/// </summary>
public class QuestionBankTests
{
    // Mirrors SimContent.RetiredNames: legacy credential codes that must never be served.
    static readonly Regex RetiredNames =
        new(@"\b(PDL[-_ ]?AI|CPMD[-_ ]?AI|CPMD|PCP[-_ ]?AI|PFIP[-_ ]?AI|PFIP)\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    // ---------- file location (same walk-up pattern as SimLabContentSeedTests) ----------

    static string RepoFile(params string[] relative)
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(new[] { dir.FullName }.Concat(relative).ToArray());
            if (File.Exists(candidate)) return candidate;
        }
        throw new FileNotFoundException(string.Join('/', relative) + " not found from " + AppContext.BaseDirectory);
    }

    sealed record Mcq(string Question, string[] Options, int AnswerIndex, string? Explanation, string Domain, string Difficulty);

    static List<Mcq> LoadJsonBank(string fileName)
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(RepoFile("backend", fileName)));
        var list = new List<Mcq>();
        foreach (var q in doc.RootElement.GetProperty("questions").EnumerateArray())
        {
            list.Add(new Mcq(
                q.GetProperty("question").GetString() ?? "",
                new[]
                {
                    q.GetProperty("option_a").GetString() ?? "",
                    q.GetProperty("option_b").GetString() ?? "",
                    q.GetProperty("option_c").GetString() ?? "",
                    q.GetProperty("option_d").GetString() ?? "",
                },
                q.GetProperty("answer_index").GetInt32(),
                q.TryGetProperty("explanation", out var e) ? e.GetString() : null,
                q.GetProperty("domain").GetString() ?? "",
                q.GetProperty("difficulty").GetString() ?? ""));
        }
        return list;
    }

    // ---------- 1. structural gates ----------

    static void AssertStructure(string bank, List<Mcq> items, bool explanationRequired)
    {
        Assert.True(items.Count > 0, $"{bank}: empty bank");
        foreach (var (q, i) in items.Select((q, i) => (q, i)))
        {
            var id = $"{bank}[{i}] \"{q.Question[..Math.Min(60, q.Question.Length)]}…\"";
            Assert.False(string.IsNullOrWhiteSpace(q.Question), $"{id}: question text missing");
            Assert.InRange(q.AnswerIndex, 0, 3);
            Assert.False(string.IsNullOrWhiteSpace(q.Domain), $"{id}: domain missing");
            Assert.All(q.Options, o => Assert.False(string.IsNullOrWhiteSpace(o), $"{id}: blank option"));
            // No distractor may equal the keyed answer (or any other option) — ambiguity guard.
            Assert.Equal(4, q.Options.Select(o => o.Trim().ToUpperInvariant()).Distinct().Count());
            if (explanationRequired)
                Assert.False(string.IsNullOrWhiteSpace(q.Explanation), $"{id}: explanation missing");
            foreach (var text in q.Options.Append(q.Question).Append(q.Explanation ?? ""))
                Assert.False(RetiredNames.IsMatch(text), $"{id}: retired certification name in served text");
        }
    }

    [Fact]
    public void Certuvo_practice_pack_is_structurally_valid()
    {
        var items = LoadJsonBank("certuvo_seed.json");
        Assert.Equal(40, items.Count);
        AssertStructure("certuvo", items, explanationRequired: true);
        // 8 BoK domains x 5 questions, as the seeder and practice UI assume.
        Assert.Equal(8, items.Select(q => q.Domain).Distinct().Count());
        Assert.All(items.GroupBy(q => q.Domain), g => Assert.Equal(5, g.Count()));
    }

    [Fact]
    public void Demo_exam_pack_is_structurally_valid()
    {
        var items = LoadJsonBank("demo_exam_seed.json");
        Assert.Equal(24, items.Count);
        AssertStructure("demo_exam", items, explanationRequired: false);
    }

    // ---------- 2. independent golden derivations ----------

    /// <summary>One independently re-derived calculation answer. <paramref name="Fragments"/> are the
    /// exact value strings that must appear in the keyed option; no OTHER option may contain all of
    /// them (single-fragment goldens therefore demand uniqueness). <paramref name="Exact"/> instead
    /// requires the keyed option to BE the value verbatim (used where numeric options are substrings
    /// of one another, e.g. 60,000 inside 660,000).</summary>
    sealed record Golden(string KeyFragment, decimal Derived, decimal Expected, string[] Fragments, string? Exact = null);

    static void AssertGoldens(string bank, List<Mcq> items, Golden[] goldens)
    {
        foreach (var g in goldens)
        {
            // The independent derivation itself must hold (decimal arithmetic, full precision).
            Assert.True(g.Derived == g.Expected, $"{bank} \"{g.KeyFragment}\": derivation {g.Derived} != expected {g.Expected}");

            var q = Assert.Single(items, x => x.Question.Contains(g.KeyFragment, StringComparison.Ordinal));
            var correct = q.Options[q.AnswerIndex];
            if (g.Exact is not null)
            {
                Assert.True(string.Equals(correct.Trim(), g.Exact, StringComparison.Ordinal),
                    $"{bank} \"{g.KeyFragment}\": keyed option \"{correct}\" is not the derived answer \"{g.Exact}\"");
                Assert.All(q.Options.Where((_, i) => i != q.AnswerIndex),
                    o => Assert.False(string.Equals(o.Trim(), g.Exact, StringComparison.Ordinal),
                        $"{bank} \"{g.KeyFragment}\": distractor equals the derived answer"));
            }
            else
            {
                foreach (var f in g.Fragments)
                    Assert.True(correct.Contains(f, StringComparison.Ordinal),
                        $"{bank} \"{g.KeyFragment}\": keyed option \"{correct}\" lacks derived value \"{f}\"");
                Assert.All(q.Options.Where((_, i) => i != q.AnswerIndex),
                    o => Assert.False(g.Fragments.All(f => o.Contains(f, StringComparison.Ordinal)),
                        $"{bank} \"{g.KeyFragment}\": a distractor also carries the derived answer"));
            }
        }
    }

    [Fact]
    public void Certuvo_calculation_keys_match_independent_derivations()
    {
        var items = LoadJsonBank("certuvo_seed.json");
        AssertGoldens("certuvo", items, new[]
        {
            // CV = EV - AC; SV = EV - PV (PV 200k, EV 180k, AC 190k)
            new Golden("planned value is 200,000", 180_000m - 190_000m, -10_000m,
                new[] { "CV = -10,000", "SV = -20,000" }),
            // CPI = EV/AC = 500/550 -> 0.91 (2 dp); SPI = EV/PV = 500/625 = 0.80
            new Golden("earned value of 500,000", Math.Round(500_000m / 550_000m, 2), 0.91m,
                new[] { "CPI = 0.91", "SPI = 0.80" }),
            // EAC = BAC / CPI = 1,000,000 / 0.80
            new Golden("budget at completion of 1,000,000 and a current CPI of 0.80", 1_000_000m / 0.80m, 1_250_000m,
                new[] { "1,250,000" }),
            // VAC = BAC - EAC = 1,000,000 - 1,250,000
            new Golden("estimate at completion of 1,250,000, the sponsor asks", 1_000_000m - 1_250_000m, -250_000m,
                new[] { "VAC = -250,000" }),
            // EMV = P x I = 0.30 x 200,000 (options are bare numbers -> exact match; 60,000 is a substring of 660,000)
            new Golden("probability of occurrence of 30 percent", 0.30m * 200_000m, 60_000m,
                Array.Empty<string>(), Exact: "60,000"),
            // Contingency to P80 = P80 - base = 54.0 - 48.0
            new Golden("P50 of 50.0 million and a P80 of 54.0 million", 54.0m - 48.0m, 6.0m,
                Array.Empty<string>(), Exact: "6.0 million"),
        });
    }

    [Fact]
    public void Demo_exam_calculation_keys_match_independent_derivations()
    {
        var items = LoadJsonBank("demo_exam_seed.json");
        AssertGoldens("demo_exam", items, new[]
        {
            // Equity = Assets - Liabilities
            new Golden("total assets of $4,200,000", 4_200_000m - 1_500_000m, 2_700_000m,
                Array.Empty<string>(), Exact: "$2,700,000"),
            // CV = EV - AC (EV 90k, AC 110k)
            new Golden("What is the cost variance (CV)?", 90_000m - 110_000m, -20_000m,
                Array.Empty<string>(), Exact: "-$20,000"),
            // CPI = EV / AC = 90/120
            new Golden("What is the cost performance index (CPI)?", 90_000m / 120_000m, 0.75m,
                Array.Empty<string>(), Exact: "0.75"),
            // SPI = EV / PV = 90/100
            new Golden("What is the schedule performance index (SPI)?", 90_000m / 100_000m, 0.90m,
                Array.Empty<string>(), Exact: "0.90"),
            // EAC = BAC / CPI = 1,000,000 / 0.80
            new Golden("what is the estimate at completion (EAC = BAC / CPI)?", 1_000_000m / 0.80m, 1_250_000m,
                Array.Empty<string>(), Exact: "$1,250,000"),
            // Cost-to-cost percent complete = 600k / 2,000k
            new Golden("Using the input (cost-to-cost) method", 600_000m / 2_000_000m, 0.30m,
                Array.Empty<string>(), Exact: "30%"),
            // Straight-line depreciation = (50,000 - 5,000) / 5
            new Golden("residual value of $5,000 and a useful life of 5 years", (50_000m - 5_000m) / 5m, 9_000m,
                Array.Empty<string>(), Exact: "$9,000"),
            // EMV = 0.20 x 50,000
            new Golden("A risk has a 20% probability", 0.20m * 50_000m, 10_000m,
                Array.Empty<string>(), Exact: "$10,000"),
            // Velocity = 24 points / 3 sprints
            new Golden("24 story points across three sprints", 24m / 3m, 8m,
                Array.Empty<string>(), Exact: "8 points per sprint"),
            // TCPI(BAC) = (BAC - EV) / (BAC - AC) = 600k / 500k
            new Golden("What is the to-complete performance index (TCPI)", (1_000_000m - 400_000m) / (1_000_000m - 500_000m), 1.20m,
                Array.Empty<string>(), Exact: "1.20"),
            // VAC = BAC - EAC
            new Golden("What is the variance at completion (VAC)?", 1_000_000m - 1_250_000m, -250_000m,
                Array.Empty<string>(), Exact: "-$250,000"),
            // ETC = EAC - AC
            new Golden("What is the estimate to complete (ETC = EAC - AC)?", 1_250_000m - 500_000m, 750_000m,
                Array.Empty<string>(), Exact: "$750,000"),
        });
    }

    // ---------- 3. public study download (markdown) ----------

    [Fact]
    public void Public_sample_questions_markdown_keys_are_unique_and_calc_answers_verified()
    {
        var text = File.ReadAllText(RepoFile("docs", "downloads", "sample-questions.md"));
        var blocks = text.Split("**MCQ ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
        Assert.Equal(16, blocks.Count);

        foreach (var block in blocks)
        {
            var id = block[..block.IndexOf('`')].Trim();
            var optionLines = block.Split('\n').Where(l => l.TrimStart().StartsWith("- ")).ToList();
            Assert.True(optionLines.Count >= 4, $"MCQ {id}: fewer than 4 options");
            // Exactly one option must be marked correct.
            Assert.True(optionLines.Count(l => l.Contains('\u2705')) == 1, $"MCQ {id}: expected exactly one \u2705 key");
        }

        // Independent numeric derivations for the three calculation items (decimal arithmetic):
        //   5.1-A  overhead absorbed = (600,000 / 30,000) x 28,000 = 560,000
        //   6.1-B  50/50 rule EV on a started 100,000 package     = 50,000
        //   10.1-A PERT tE = (4 + 4x6 + 14) / 6                   = 7
        var goldens = new (string BlockId, decimal Derived, decimal Expected, string Fragment)[]
        {
            ("5.1-A", 600_000m / 30_000m * 28_000m, 560_000m, "560,000"),
            ("6.1-B", 100_000m * 0.5m, 50_000m, "50,000"),
            ("10.1-A", (4m + 4m * 6m + 14m) / 6m, 7m, "7 days"),
        };
        foreach (var g in goldens)
        {
            Assert.True(g.Derived == g.Expected, $"MCQ {g.BlockId}: derivation {g.Derived} != {g.Expected}");
            var block = Assert.Single(blocks, b => b.StartsWith(g.BlockId, StringComparison.Ordinal));
            var keyed = Assert.Single(block.Split('\n'), l => l.TrimStart().StartsWith("- ") && l.Contains('\u2705'));
            Assert.True(keyed.Contains(g.Fragment, StringComparison.Ordinal),
                $"MCQ {g.BlockId}: keyed option \"{keyed.Trim()}\" lacks independently derived \"{g.Fragment}\"");
            Assert.All(block.Split('\n').Where(l => l.TrimStart().StartsWith("- ") && !l.Contains('\u2705')),
                l => Assert.False(l.Contains(g.Fragment, StringComparison.Ordinal),
                    $"MCQ {g.BlockId}: distractor also carries the derived answer"));
        }
    }
}
