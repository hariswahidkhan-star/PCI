using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class SimCalcNextReleaseTests
{
    static JsonElement J(string json) => JsonDocument.Parse(json).RootElement;

    [Fact]
    public void Productivity_MatchesHandComputedValues()
    {
        var r = SimCalc.Productivity(100, 50, 90, 60);
        Assert.Equal(2.0, r.PlannedProductivity, 4);
        Assert.Equal(1.5, r.ActualProductivity, 4);
        Assert.Equal(0.75, r.Factor, 4);
        Assert.Equal(10, r.HoursVariance, 4);
        Assert.Equal(-10, r.QtyVariance, 4);
    }

    [Fact]
    public void Resolve_Boq_SumsQtyTimesRate()
    {
        var given = J("""{"lines":[{"id":"L1","qty":10,"rate":50},{"id":"L2","qty":4,"rate":80}]}""");
        Assert.Equal(820, (double)SimCalc.Resolve("boq", "total", given)!, 4);
        Assert.Equal(2, (double)SimCalc.Resolve("boq", "line_count", given)!, 4);
        Assert.Equal(65, (double)SimCalc.Resolve("boq", "average_rate", given)!, 4);
    }

    [Fact]
    public void Resolve_Resource_ComputesOverload()
    {
        var given = J("""{"periods":[{"period":1,"demand":8,"capacity":10},{"period":2,"demand":14,"capacity":10},{"period":3,"demand":12,"capacity":10}]}""");
        Assert.Equal(14, (double)SimCalc.Resolve("resource", "peak_demand", given)!, 4);
        Assert.Equal(4, (double)SimCalc.Resolve("resource", "peak_overload", given)!, 4);
        Assert.Equal(6, (double)SimCalc.Resolve("resource", "total_overdemand", given)!, 4);
        Assert.Equal(2, (double)SimCalc.Resolve("resource", "overloaded_periods", given)!, 4);
    }

    [Fact]
    public void Resolve_Procurement_ConsumesFloatThenCriticalDelay()
    {
        var given = J("""{"project_duration":100,"remaining_float":5,"supplier_delay_days":12}""");
        Assert.Equal(7, (double)SimCalc.Resolve("procurement", "critical_delay", given)!, 4);
        Assert.Equal(107, (double)SimCalc.Resolve("procurement", "new_project_duration", given)!, 4);
        Assert.Equal(5, (double)SimCalc.Resolve("procurement", "float_consumed", given)!, 4);
    }

    [Fact]
    public void Resolve_Portfolio_RanksByWeightedScore()
    {
        var given = J("""{"w_npv":0.5,"w_risk":0.3,"w_fit":0.2,"projects":[{"id":"A","npv":100,"risk":0.2,"fit":0.8},{"id":"B","npv":80,"risk":0.1,"fit":0.9}]}""");
        Assert.Equal("A", (string)SimCalc.Resolve("portfolio", "top_id", given)!);
        Assert.Equal(180, (double)SimCalc.Resolve("portfolio", "total_npv", given)!, 4);
    }

    [Fact]
    public void Resolve_Decision_PicksLowestWeightedImpact()
    {
        var given = J("""{"w_cost":1,"w_sched":1,"w_risk":1,"options":[{"id":"crash","cost":50,"schedule":-10,"risk":0.4},{"id":"hold","cost":0,"schedule":20,"risk":0.2}]}""");
        Assert.Equal("hold", (string)SimCalc.Resolve("decision", "best_option", given)!);
    }

    [Fact]
    public void Resolve_DataQuality_CountsAnomaliesAndCompleteness()
    {
        var given = J("""{"threshold":5,"rows":[{"value":10,"expected":10},{"value":20,"expected":10},{"value":null,"expected":5}]}""");
        Assert.Equal(1, (double)SimCalc.Resolve("data_quality", "anomaly_count", given)!, 4);
        Assert.Equal(66.6667, (double)SimCalc.Resolve("data_quality", "completeness_pct", given)!, 3);
        Assert.Equal(3, (double)SimCalc.Resolve("data_quality", "record_count", given)!, 4);
    }
}
