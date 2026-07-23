using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Direct unit coverage of the Simulation Lab's deterministic calculation engine
/// (<see cref="SimCalc"/>) — the spec's non-negotiable "critical calculations must be deterministic and
/// unit-tested". Every expected value here is hand-computed from first principles, so the test pins the
/// arithmetic itself rather than echoing the implementation. Pure functions: no database, no clock, no
/// randomness. The grading layer (Core/SimLab) computes authoritative answers through the very same
/// <see cref="SimCalc.Resolve"/> path these tests exercise.
/// </summary>
public class SimCalcTests
{
    static JsonElement J(string json) => JsonDocument.Parse(json).RootElement;

    // ── Earned Value ────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Evm_CoreMeasures_MatchHandComputedValues()
    {
        // PV 100k, EV 90k, AC 95k — behind schedule and over cost.
        var r = SimCalc.Evm(100_000, 90_000, 95_000);
        Assert.Equal(-10_000, r.SV, 4);       // EV − PV
        Assert.Equal(-5_000, r.CV, 4);        // EV − AC
        Assert.Equal(0.9, r.SPI, 4);          // EV / PV
        Assert.Equal(0.9474, r.CPI, 4);       // 90000/95000 = 0.947368…
        Assert.Null(r.EAC);                   // no BAC supplied ⇒ no forecast
        Assert.Null(r.VAC);
        Assert.Null(r.PercentComplete);
    }

    [Fact]
    public void Evm_WithBac_ComputesForecastMeasures()
    {
        var r = SimCalc.Evm(100_000, 90_000, 95_000, 200_000);
        // EAC = BAC / CPI = 200000 / (90000/95000) = 211111.1111…
        Assert.Equal(211_111.1111, r.EAC!.Value, 4);
        Assert.Equal(116_111.1111, r.ETC!.Value, 4);   // EAC − AC
        Assert.Equal(-11_111.1111, r.VAC!.Value, 4);   // BAC − EAC
        // TCPI = (BAC − EV) / (BAC − AC) = 110000 / 105000 = 1.047619…
        Assert.Equal(1.0476, r.TCPI!.Value, 4);
        Assert.Equal(0.45, r.PercentComplete!.Value, 4);  // EV / BAC
        Assert.Equal(0.475, r.PercentSpent!.Value, 4);    // AC / BAC
    }

    [Fact]
    public void Evm_ZeroDivisors_ReturnZeroRatherThanThrow()
    {
        var r = SimCalc.Evm(0, 0, 0, 0);
        Assert.Equal(0, r.SPI, 4);
        Assert.Equal(0, r.CPI, 4);
        Assert.Equal(0, r.TCPI!.Value, 4);   // remaining budget 0 ⇒ 0, not an exception
    }

    // ── Critical Path ───────────────────────────────────────────────────────────────────────────

    static readonly SimCalc.CpmActivity[] Network =
    {
        // A→{B,C}; {B,C}→D; D→E.  A(3) B(4) C(2) D(5) E(1)
        new("A", 3, new[] { "" }.Where(s => s != "").ToArray()),
        new("B", 4, new[] { "A" }),
        new("C", 2, new[] { "A" }),
        new("D", 5, new[] { "B", "C" }),
        new("E", 1, new[] { "D" }),
    };

    [Fact]
    public void CriticalPath_FindsDurationAndPath()
    {
        var r = SimCalc.CriticalPath(Network);
        Assert.Equal(13, r.ProjectDuration, 4);                 // 3+4+5+1 along A-B-D-E
        Assert.Equal(new[] { "A", "B", "D", "E" }, r.CriticalPath);
    }

    [Fact]
    public void CriticalPath_ComputesFloatForNonCriticalActivity()
    {
        var r = SimCalc.CriticalPath(Network);
        var c = r.Nodes.Single(n => n.Id == "C");
        Assert.Equal(2, c.TotalFloat, 4);   // C can slip 2 days without moving the finish
        Assert.False(c.Critical);
        var b = r.Nodes.Single(n => n.Id == "B");
        Assert.Equal(0, b.TotalFloat, 4);
        Assert.True(b.Critical);
    }

    [Fact]
    public void CriticalPath_RejectsCycle()
    {
        var cyclic = new[]
        {
            new SimCalc.CpmActivity("X", 1, new[] { "Y" }),
            new SimCalc.CpmActivity("Y", 1, new[] { "X" }),
        };
        Assert.Throws<ArgumentException>(() => SimCalc.CriticalPath(cyclic));
    }

    [Fact]
    public void CriticalPath_RejectsUnknownPredecessor()
    {
        var bad = new[] { new SimCalc.CpmActivity("A", 1, new[] { "ghost" }) };
        Assert.Throws<ArgumentException>(() => SimCalc.CriticalPath(bad));
    }

    // ── WBS roll-up / 100% rule ───────────────────────────────────────────────────────────────────

    static readonly SimCalc.WbsInputNode[] Tree =
    {
        new("1", null, null),
        new("1.1", "1", 40_000),
        new("1.2", "1", null),
        new("1.2.1", "1.2", 30_000),
        new("1.2.2", "1.2", 20_000),
    };

    [Fact]
    public void Wbs_RollsUpLeavesToRoot()
    {
        var r = SimCalc.Wbs(Tree);
        Assert.Equal(90_000, r.RootTotal, 4);                       // 40k + (30k+20k)
        Assert.Equal(50_000, r.Nodes.Single(n => n.Id == "1.2").Value, 4);
        Assert.True(r.HundredPercentValid);
        Assert.Empty(r.Violations);
    }

    [Fact]
    public void Wbs_FlagsHundredPercentViolation()
    {
        // Declare a parent value that disagrees with its children's roll-up.
        var withDeclared = new[]
        {
            new SimCalc.WbsInputNode("1", null, null),
            new SimCalc.WbsInputNode("1.1", "1", 40_000),
            new SimCalc.WbsInputNode("1.2", "1", 60_000),   // children only sum to 50k
            new SimCalc.WbsInputNode("1.2.1", "1.2", 30_000),
            new SimCalc.WbsInputNode("1.2.2", "1.2", 20_000),
        };
        var r = SimCalc.Wbs(withDeclared);
        Assert.False(r.HundredPercentValid);
        Assert.Contains("1.2", r.Violations);
    }

    [Fact]
    public void Wbs_RejectsUnknownParent()
    {
        var orphan = new[] { new SimCalc.WbsInputNode("x", "nope", 1) };
        Assert.Throws<ArgumentException>(() => SimCalc.Wbs(orphan));
    }

    // ── Resolve: the single answer-source the grader uses ─────────────────────────────────────────

    [Fact]
    public void Resolve_Evm_ReturnsNumericMeasures()
    {
        var given = J("{\"pv\":100000,\"ev\":90000,\"ac\":95000,\"bac\":200000}");
        Assert.Equal(0.9, (double)SimCalc.Resolve("evm", "spi", given)!, 4);
        Assert.Equal(211_111.1111, (double)SimCalc.Resolve("evm", "eac", given)!, 4);
        Assert.Null(SimCalc.Resolve("evm", "not_a_measure", given));
    }

    [Fact]
    public void Resolve_Cpm_ReturnsDurationAndPathSet()
    {
        var given = J("{\"activities\":[" +
            "{\"id\":\"A\",\"dur\":3,\"preds\":[]}," +
            "{\"id\":\"B\",\"dur\":4,\"preds\":[\"A\"]}," +
            "{\"id\":\"C\",\"dur\":2,\"preds\":[\"A\"]}," +
            "{\"id\":\"D\",\"dur\":5,\"preds\":[\"B\",\"C\"]}," +
            "{\"id\":\"E\",\"dur\":1,\"preds\":[\"D\"]}]}");
        Assert.Equal(13, (double)SimCalc.Resolve("cpm", "project_duration", given)!, 4);
        Assert.Equal(new[] { "A", "B", "D", "E" }, (string[])SimCalc.Resolve("cpm", "critical_path", given)!);
        Assert.Equal(2, (double)SimCalc.Resolve("cpm", "float_C", given)!, 4);
    }

    [Fact]
    public void Resolve_Wbs_ReturnsRootTotalAndValidity()
    {
        var given = J("{\"nodes\":[" +
            "{\"id\":\"1\",\"parent\":null}," +
            "{\"id\":\"1.1\",\"parent\":\"1\",\"value\":40000}," +
            "{\"id\":\"1.2\",\"parent\":\"1\"}," +
            "{\"id\":\"1.2.1\",\"parent\":\"1.2\",\"value\":30000}," +
            "{\"id\":\"1.2.2\",\"parent\":\"1.2\",\"value\":20000}]}");
        Assert.Equal(90_000, (double)SimCalc.Resolve("wbs", "root_total", given)!, 4);
        Assert.Equal(true, SimCalc.Resolve("wbs", "hundred_percent_valid", given));
    }

    // ── Forecasting (three EAC methods) ───────────────────────────────────────────────────────────

    [Fact]
    public void Forecast_ThreeEacMethods_MatchHandComputedValues()
    {
        // PV 200k, EV 180k, AC 200k, BAC 400k → CPI 0.9, SPI 0.9.
        var f = SimCalc.Forecast(200_000, 180_000, 200_000, 400_000);
        Assert.Equal(0.9, f.CPI, 4);
        Assert.Equal(0.9, f.SPI, 4);
        Assert.Equal(444_444.4444, f.EacCpi, 4);        // BAC/CPI = 400000/0.9
        // AC + (BAC−EV)/(CPI·SPI) = 200000 + 220000/0.81 = 471604.9383
        Assert.Equal(471_604.9383, f.EacComposite, 4);
        Assert.Equal(420_000, f.EacBudget, 4);          // AC + (BAC−EV) = 200000 + 220000
        Assert.Equal(244_444.4444, f.Etc, 4);           // EacCpi − AC
        Assert.Equal(-44_444.4444, f.Vac, 4);           // BAC − EacCpi
    }

    [Fact]
    public void Resolve_Evm_ReturnsForecastingEacVariants()
    {
        var given = J("{\"pv\":200000,\"ev\":180000,\"ac\":200000,\"bac\":400000}");
        Assert.Equal(444_444.4444, (double)SimCalc.Resolve("evm", "eac_cpi", given)!, 4);
        Assert.Equal(471_604.9383, (double)SimCalc.Resolve("evm", "eac_composite", given)!, 4);
        Assert.Equal(420_000, (double)SimCalc.Resolve("evm", "eac_budget", given)!, 4);
    }

    // ── CBS cost roll-up + variance ───────────────────────────────────────────────────────────────

    static readonly SimCalc.CbsInputNode[] CostTree =
    {
        new("1", null, null, null),
        new("1.1", "1", 50_000, 55_000),   // over by 5k
        new("1.2", "1", 30_000, 25_000),   // under by 5k
        new("1.3", "1", 20_000, 22_000),   // over by 2k
    };

    [Fact]
    public void Cbs_RollsUpBudgetActualAndVariance()
    {
        var r = SimCalc.Cbs(CostTree);
        Assert.Equal(100_000, r.TotalBudget, 4);
        Assert.Equal(102_000, r.TotalActual, 4);
        Assert.Equal(-2_000, r.Variance, 4);            // budget − actual → 2k over
        Assert.Equal(-5_000, r.Nodes.Single(n => n.Id == "1.1").Variance, 4);
    }

    [Fact]
    public void Resolve_Cbs_ReturnsRootBudgetActualVariance()
    {
        var given = J("{\"nodes\":[" +
            "{\"id\":\"1\",\"parent\":null}," +
            "{\"id\":\"1.1\",\"parent\":\"1\",\"budget\":50000,\"actual\":55000}," +
            "{\"id\":\"1.2\",\"parent\":\"1\",\"budget\":30000,\"actual\":25000}," +
            "{\"id\":\"1.3\",\"parent\":\"1\",\"budget\":20000,\"actual\":22000}]}");
        Assert.Equal(100_000, (double)SimCalc.Resolve("cbs", "root_budget", given)!, 4);
        Assert.Equal(102_000, (double)SimCalc.Resolve("cbs", "root_actual", given)!, 4);
        Assert.Equal(-2_000, (double)SimCalc.Resolve("cbs", "root_variance", given)!, 4);
        Assert.Equal(-5_000, (double)SimCalc.Resolve("cbs", "variance_1.1", given)!, 4);
    }

    [Fact]
    public void Cbs_RejectsUnknownParent()
    {
        var bad = new[] { new SimCalc.CbsInputNode("x", "nope", 1, 1) };
        Assert.Throws<ArgumentException>(() => SimCalc.Cbs(bad));
    }

    // ── Weighted physical progress ────────────────────────────────────────────────────────────────

    [Fact]
    public void Progress_IsTheBudgetWeightedAverageOfPackageProgress()
    {
        var nodes = new[]
        {
            new SimCalc.ProgressInputNode("1.1", 40_000, 100),
            new SimCalc.ProgressInputNode("1.2", 30_000, 50),
            new SimCalc.ProgressInputNode("1.3", 30_000, 0),
        };
        var r = SimCalc.Progress(nodes);
        Assert.Equal(55, r.OverallPercent, 4);      // (40k·100 + 30k·50 + 30k·0) / 100k
        Assert.Equal(100_000, r.TotalWeight, 4);
    }

    [Fact]
    public void Resolve_Progress_ReturnsOverallPercent()
    {
        var given = J("{\"nodes\":[" +
            "{\"id\":\"1.1\",\"weight\":40000,\"percent\":100}," +
            "{\"id\":\"1.2\",\"weight\":30000,\"percent\":50}," +
            "{\"id\":\"1.3\",\"weight\":30000,\"percent\":0}]}");
        Assert.Equal(55, (double)SimCalc.Resolve("progress", "overall_percent", given)!, 4);
    }

    // ── Risk: Expected Monetary Value ─────────────────────────────────────────────────────────────

    [Fact]
    public void Emv_SumsProbabilityTimesImpact()
    {
        var risks = new[]
        {
            new SimCalc.RiskItem("R1", 0.30, -20000),   // threat
            new SimCalc.RiskItem("R2", 0.50, -10000),   // threat
            new SimCalc.RiskItem("R3", 0.20, 15000),    // opportunity
        };
        var r = SimCalc.Emv(risks);
        // 0.3·-20000 + 0.5·-10000 + 0.2·15000 = -6000 - 5000 + 3000 = -8000
        Assert.Equal(-8000, r.Total, 4);
        Assert.Equal(-6000, r.Items.Single(i => i.id == "R1").emv, 4);
    }

    [Fact]
    public void Resolve_Risk_ReturnsEmvTotalAndPerRisk()
    {
        var given = J("{\"risks\":[" +
            "{\"id\":\"R1\",\"probability\":0.3,\"impact\":-20000}," +
            "{\"id\":\"R2\",\"probability\":0.5,\"impact\":-10000}," +
            "{\"id\":\"R3\",\"probability\":0.2,\"impact\":15000}]}");
        Assert.Equal(-8000, (double)SimCalc.Resolve("risk", "emv", given)!, 4);
        Assert.Equal(-6000, (double)SimCalc.Resolve("risk", "emv_R1", given)!, 4);
    }

    // ── Risk: three-point (PERT) analysis ─────────────────────────────────────────────────────────

    [Fact]
    public void Pert_ComputesExpectedAndStandardDeviation()
    {
        var r = SimCalc.Pert(2, 4, 6);                 // (2 + 16 + 6)/6 = 4; sd = (6-2)/6 = 0.6667
        Assert.Equal(4, r.Expected, 4);
        Assert.Equal(0.6667, r.StdDev, 4);
    }

    [Fact]
    public void PertPath_SumsExpectedAndVarianceAndComputesProbability()
    {
        var acts = new[] { (2.0, 4.0, 6.0), (3.0, 5.0, 13.0), (1.0, 2.0, 3.0) };
        var r = SimCalc.PertPath(acts, 14);
        // expected 4 + 6 + 2 = 12; variances 0.4444 + 2.7778 + 0.1111 = 3.3333; sd = 1.8257
        Assert.Equal(12, r.Expected, 4);
        Assert.Equal(1.8257, r.StdDev, 4);
        // Z = (14-12)/1.8257 = 1.0954 → Φ ≈ 0.8632 → ~86.3%
        Assert.InRange(r.ProbOnTimePercent!.Value, 86.0, 87.0);
    }

    [Fact]
    public void NormalCdf_MatchesKnownValues()
    {
        Assert.Equal(0.5, SimCalc.NormalCdf(0), 3);
        Assert.Equal(0.8413, SimCalc.NormalCdf(1), 3);     // Φ(1)
        Assert.Equal(0.1587, SimCalc.NormalCdf(-1), 3);    // Φ(-1)
    }

    [Fact]
    public void Resolve_Pert_ReturnsPathExpectedStdDevAndProbability()
    {
        var given = J("{\"activities\":[" +
            "{\"id\":\"A\",\"o\":2,\"m\":4,\"p\":6}," +
            "{\"id\":\"B\",\"o\":3,\"m\":5,\"p\":13}," +
            "{\"id\":\"C\",\"o\":1,\"m\":2,\"p\":3}],\"deadline\":14}");
        Assert.Equal(12, (double)SimCalc.Resolve("pert", "expected_duration", given)!, 4);
        Assert.Equal(1.8257, (double)SimCalc.Resolve("pert", "std_dev", given)!, 4);
        Assert.InRange((double)SimCalc.Resolve("pert", "prob_on_time", given)!, 86.0, 87.0);
    }

    // ── Monte-Carlo schedule simulation ───────────────────────────────────────────────────────────

    static readonly SimCalc.McActivity[] McChain =
    {
        new("A", 2, 4, 6, Array.Empty<string>()),
        new("B", 3, 5, 13, new[] { "A" }),
        new("C", 1, 2, 3, new[] { "B" }),
    };

    [Fact]
    public void MonteCarlo_IsDeterministicForAGivenSeed()
    {
        var a = SimCalc.MonteCarlo(McChain, seed: 12345, iterations: 3000);
        var b = SimCalc.MonteCarlo(McChain, seed: 12345, iterations: 3000);
        Assert.Equal(a.P50, b.P50, 6);      // identical run-to-run — the seeded PRNG guarantees it
        Assert.Equal(a.P80, b.P80, 6);
        Assert.Equal(a.Mean, b.Mean, 6);
    }

    [Fact]
    public void MonteCarlo_PercentilesAreOrderedAndBracketTheMean()
    {
        var r = SimCalc.MonteCarlo(McChain, seed: 7, iterations: 5000);
        Assert.True(r.Min <= r.P10 && r.P10 <= r.P50 && r.P50 <= r.P80 && r.P80 <= r.P90 && r.P90 <= r.Max);
        Assert.InRange(r.Mean, r.Min, r.Max);
        Assert.Equal(5000, r.Histogram.Sum(b => b.Count));   // every iteration lands in exactly one bucket
    }

    [Fact]
    public void MonteCarlo_MeanApproachesTheTriangularExpectation()
    {
        // Each activity's triangular mean is (O+M+P)/3: A=4, B=7, C=2 → chain mean 13 (the PERT expected
        // duration is 12; the divergence between the two is exactly what the dashboard illustrates).
        var r = SimCalc.MonteCarlo(McChain, seed: 99, iterations: 20000);
        Assert.InRange(r.Mean, 12.7, 13.3);
    }

    // ── Change control ────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void ChangeControl_AppliesOnlyApprovedChanges()
    {
        var changes = new[]
        {
            new SimCalc.ChangeItem("C1", "approved", 30000, 5),
            new SimCalc.ChangeItem("C2", "rejected", 50000, 10),   // ignored
            new SimCalc.ChangeItem("C3", "approved", -10000, -2),
            new SimCalc.ChangeItem("C4", "pending", 20000, 3),     // ignored
        };
        var r = SimCalc.ChangeControl(500000, 100, changes);
        Assert.Equal(520000, r.RevisedBac, 4);          // +30000 −10000
        Assert.Equal(103, r.RevisedDuration, 4);        // +5 −2
        Assert.Equal(20000, r.ApprovedCostDelta, 4);
        Assert.Equal(2, r.ApprovedCount);
    }

    [Fact]
    public void Resolve_Change_ReturnsRevisedBaselineAndApprovedCount()
    {
        var given = J("{\"baseline_bac\":500000,\"baseline_duration\":100,\"changes\":[" +
            "{\"id\":\"C1\",\"status\":\"approved\",\"cost_delta\":30000,\"schedule_delta\":5}," +
            "{\"id\":\"C2\",\"status\":\"rejected\",\"cost_delta\":50000,\"schedule_delta\":10}," +
            "{\"id\":\"C3\",\"status\":\"approved\",\"cost_delta\":-10000,\"schedule_delta\":-2}," +
            "{\"id\":\"C4\",\"status\":\"pending\",\"cost_delta\":20000,\"schedule_delta\":3}]}");
        Assert.Equal(520000, (double)SimCalc.Resolve("change", "revised_bac", given)!, 4);
        Assert.Equal(103, (double)SimCalc.Resolve("change", "revised_duration", given)!, 4);
        Assert.Equal(2, (double)SimCalc.Resolve("change", "approved_count", given)!, 4);
    }

    // ── Cash flow ─────────────────────────────────────────────────────────────────────────────────

    static readonly SimCalc.CashPeriod[] Cash =
    {
        new(1, 0, 50000),        // cum −50000
        new(2, 20000, 60000),    // cum −90000  (deepest)
        new(3, 80000, 40000),    // cum −50000
        new(4, 120000, 30000),   // cum +40000
    };

    [Fact]
    public void CashFlow_ComputesFinalPositionAndPeakFunding()
    {
        var r = SimCalc.CashFlow(Cash);
        Assert.Equal(40000, r.FinalPosition, 4);        // closing cumulative
        Assert.Equal(90000, r.PeakFunding, 4);          // deepest deficit as a positive magnitude
        Assert.Equal(-90000, r.Series.Single(s => s.Period == 2).Cumulative, 4);
    }

    [Fact]
    public void Resolve_CashFlow_ReturnsFinalPositionPeakFundingAndCumulative()
    {
        var given = J("{\"periods\":[" +
            "{\"period\":1,\"inflow\":0,\"outflow\":50000}," +
            "{\"period\":2,\"inflow\":20000,\"outflow\":60000}," +
            "{\"period\":3,\"inflow\":80000,\"outflow\":40000}," +
            "{\"period\":4,\"inflow\":120000,\"outflow\":30000}]}");
        Assert.Equal(40000, (double)SimCalc.Resolve("cashflow", "final_position", given)!, 4);
        Assert.Equal(90000, (double)SimCalc.Resolve("cashflow", "peak_funding", given)!, 4);
        Assert.Equal(-90000, (double)SimCalc.Resolve("cashflow", "cumulative_2", given)!, 4);
    }

    // ── Time-driven EVM timeline ────────────────────────────────────────────────────────────────────

    static readonly SimCalc.TimelineInput[] Timeline =
    {
        // Cumulative PV/EV/AC over six reporting periods against a 600k BAC. Worst SPI and CPI both fall in
        // period 3; the CPI-method forecast (EAC = BAC ÷ CPI) settles well above budget by the finish.
        new(1, 100000, 90000, 100000),
        new(2, 220000, 200000, 230000),
        new(3, 350000, 300000, 360000),
        new(4, 470000, 420000, 500000),
        new(5, 560000, 520000, 610000),
        new(6, 600000, 580000, 680000),
    };

    [Fact]
    public void EvmTimeline_ReplaysPeriodsAndFindsWorstPerformance()
    {
        var r = SimCalc.EvmTimeline(Timeline, 600000);
        Assert.Equal(6, r.Series.Count);
        // Period-3 snapshot: SPI 300000/350000, CPI 300000/360000, EAC 600000/CPI.
        var p3 = r.Series.Single(s => s.Period == 3);
        Assert.Equal(0.8571, p3.Spi, 4);
        Assert.Equal(0.8333, p3.Cpi, 4);
        Assert.Equal(720000, p3.Eac, 4);
        // Whole-project reading.
        Assert.Equal(0.8571, r.WorstSpi, 4);
        Assert.Equal(3, r.WorstSpiPeriod);
        Assert.Equal(0.8333, r.WorstCpi, 4);
        Assert.Equal(3, r.WorstCpiPeriod);
        Assert.Equal(0.9667, r.FinalSpi, 4);
        Assert.Equal(0.8529, r.FinalCpi, 4);
        Assert.Equal(703448.2759, r.FinalEac, 4);
        Assert.Equal(-103448.2759, r.Vac, 4);
    }

    [Fact]
    public void EvmTimeline_TieOnWorstKeepsTheEarlierPeriod()
    {
        // Two periods share the minimum CPI (0.8); the earlier one must win for determinism.
        var pts = new[]
        {
            new SimCalc.TimelineInput(1, 100, 80, 100),   // CPI 0.8
            new SimCalc.TimelineInput(2, 200, 180, 200),  // CPI 0.9
            new SimCalc.TimelineInput(3, 300, 240, 300),  // CPI 0.8 (tie, but later)
        };
        var r = SimCalc.EvmTimeline(pts, 300);
        Assert.Equal(1, r.WorstCpiPeriod);
    }

    [Fact]
    public void Resolve_Timeline_ReturnsTrendAndForecast()
    {
        var given = J("{\"bac\":600000,\"series\":[" +
            "{\"period\":1,\"pv\":100000,\"ev\":90000,\"ac\":100000}," +
            "{\"period\":2,\"pv\":220000,\"ev\":200000,\"ac\":230000}," +
            "{\"period\":3,\"pv\":350000,\"ev\":300000,\"ac\":360000}," +
            "{\"period\":4,\"pv\":470000,\"ev\":420000,\"ac\":500000}," +
            "{\"period\":5,\"pv\":560000,\"ev\":520000,\"ac\":610000}," +
            "{\"period\":6,\"pv\":600000,\"ev\":580000,\"ac\":680000}]}");
        Assert.Equal(0.8529, (double)SimCalc.Resolve("timeline", "final_cpi", given)!, 4);
        Assert.Equal(703448.2759, (double)SimCalc.Resolve("timeline", "final_eac", given)!, 4);
        Assert.Equal(-103448.2759, (double)SimCalc.Resolve("timeline", "vac", given)!, 4);
        Assert.Equal(3, (double)SimCalc.Resolve("timeline", "worst_spi_period", given)!, 4);
    }

    // ── Earned Schedule ─────────────────────────────────────────────────────────────────────────────

    static readonly (int period, double pv)[] Plan =
    {
        (1, 100), (2, 250), (3, 450), (4, 650), (5, 830), (6, 1000),
    };

    [Fact]
    public void EarnedSchedule_InterpolatesEsFromThePlannedValueCurve()
    {
        // At the end of month 4, EV is 500. The plan reached 450 at month 3 and 650 at month 4, so ES lands a
        // quarter of the way into month 4: 3 + (500-450)/(650-450) = 3.25. AT is 4, planned duration 6.
        var r = SimCalc.EarnedSchedule(Plan, evNow: 500, atNow: 4, plannedDuration: 6);
        Assert.Equal(3.25, r.Es, 4);
        Assert.Equal(-0.75, r.SvTime, 4);       // ES − AT
        Assert.Equal(0.8125, r.SpiTime, 4);     // ES ÷ AT
        Assert.Equal(7.3846, r.EacTime!.Value, 4); // PD ÷ SPI(t)
    }

    [Fact]
    public void EarnedSchedule_OnPlanGivesUnitIndexAndNoTimeVariance()
    {
        // EV exactly on the month-3 planned value → ES 3, AT 3, SPI(t) 1, SV(t) 0.
        var r = SimCalc.EarnedSchedule(Plan, evNow: 450, atNow: 3, plannedDuration: 6);
        Assert.Equal(3, r.Es, 4);
        Assert.Equal(0, r.SvTime, 4);
        Assert.Equal(1, r.SpiTime, 4);
    }

    [Fact]
    public void Resolve_EarnedSchedule_ReturnsEsAndTimeIndices()
    {
        var given = J("{\"planned_duration\":6,\"at\":4,\"ev\":500,\"plan\":[" +
            "{\"period\":1,\"pv\":100},{\"period\":2,\"pv\":250},{\"period\":3,\"pv\":450}," +
            "{\"period\":4,\"pv\":650},{\"period\":5,\"pv\":830},{\"period\":6,\"pv\":1000}]}");
        Assert.Equal(3.25, (double)SimCalc.Resolve("earned_schedule", "es", given)!, 4);
        Assert.Equal(0.8125, (double)SimCalc.Resolve("earned_schedule", "spi_time", given)!, 4);
        Assert.Equal(-0.75, (double)SimCalc.Resolve("earned_schedule", "sv_time", given)!, 4);
        Assert.Equal(7.3846, (double)SimCalc.Resolve("earned_schedule", "eac_time", given)!, 4);
    }
}
