using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — deterministic variant engine tests (Phase 5A, §6). The engine must be a pure function of
/// (config, seed): reproducible so an attempt can store just its seed and replay/re-grade identically;
/// bounded so a variant never leaves the authored ranges; and answer-preserving so every generated instance
/// stays gradable by SimCalc. Seed 0 and configs without a variant block are the canonical instance.
/// </summary>
public class SimVariantTests
{
    const string EvmVariantConfig = """
        {"task":"evm",
         "prompt":"Compute the EVM measures.",
         "given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
         "ask":[{"key":"cpi","type":"number"},{"key":"eac","type":"number"},{"key":"spi","type":"number"}],
         "variant":{"vary":{
           "pv":{"min":80000,"max":120000,"step":1000},
           "ev":{"min":70000,"max":110000,"step":1000},
           "ac":{"min":80000,"max":115000,"step":1000}}}}
        """;

    const string NoVariantConfig = """
        {"task":"evm","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
         "ask":[{"key":"cpi","type":"number"}]}
        """;

    static JsonElement Given(string configJson)
    {
        using var doc = JsonDocument.Parse(configJson);
        return doc.RootElement.GetProperty("given").Clone();
    }

    [Fact]
    public void Seed_zero_returns_the_canonical_config_unchanged()
    {
        Assert.Equal(EvmVariantConfig, SimVariant.Derive(EvmVariantConfig, 0));
    }

    [Fact]
    public void Config_without_a_variant_block_is_returned_unchanged()
    {
        Assert.Equal(NoVariantConfig, SimVariant.Derive(NoVariantConfig, 7));
    }

    [Fact]
    public void Derivation_is_deterministic_for_the_same_seed()
    {
        Assert.Equal(SimVariant.Derive(EvmVariantConfig, 42), SimVariant.Derive(EvmVariantConfig, 42));
    }

    [Fact]
    public void Different_seeds_produce_more_than_one_distinct_instance()
    {
        var distinct = new HashSet<string>();
        for (long s = 1; s <= 6; s++) distinct.Add(SimVariant.Derive(EvmVariantConfig, s));
        Assert.True(distinct.Count >= 2, "seeded variants should not all be identical");
    }

    [Fact]
    public void Every_varied_value_stays_within_its_authored_range_and_on_step()
    {
        (double min, double max, double step)[] bounds = { (80000, 120000, 1000) };
        for (long s = 1; s <= 20; s++)
        {
            var g = Given(SimVariant.Derive(EvmVariantConfig, s));
            foreach (var (key, min, max, step) in new[]
                     {
                         ("pv", 80000.0, 120000.0, 1000.0),
                         ("ev", 70000.0, 110000.0, 1000.0),
                         ("ac", 80000.0, 115000.0, 1000.0),
                     })
            {
                var v = g.GetProperty(key).GetDouble();
                Assert.InRange(v, min, max);
                Assert.True(Math.Abs((v - min) % step) < 1e-6, $"{key}={v} is off-step for seed {s}");
            }
            // bac is not in the vary set — it must stay fixed.
            Assert.Equal(200000, g.GetProperty("bac").GetDouble());
        }
    }

    [Fact]
    public void Every_variant_remains_gradable_by_the_engine()
    {
        for (long s = 1; s <= SimVariant.ValidationSeeds; s++)
        {
            var g = Given(SimVariant.Derive(EvmVariantConfig, s));
            foreach (var key in new[] { "cpi", "eac", "spi" })
            {
                var ans = SimCalc.Resolve("evm", key, g);
                Assert.True(ans is double d && !double.IsNaN(d) && !double.IsInfinity(d),
                    $"variant seed {s} left measure '{key}' ungradable");
            }
        }
    }

    [Fact]
    public void Malformed_config_is_returned_unchanged_without_throwing()
    {
        Assert.Equal("{not json", SimVariant.Derive("{not json", 3));
    }
}
