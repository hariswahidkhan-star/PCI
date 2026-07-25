using System.Text.Json;
using System.Text.Json.Nodes;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — deterministic scenario variant engine (Phase 5A, §6).
///
/// One authored scenario yields 3–10 meaningfully-different practice instances from controlled seeds, so a
/// learner who retries (or a cohort working the same scenario) does not see the identical numbers — without
/// pretending each variant is a separately-authored scenario. A scenario opts in by carrying a
/// <c>variant</c> block in its <c>config_json</c>:
/// <code>
///   "variant": { "vary": { "pv": {"min":80000,"max":120000,"step":1000}, ... } }
/// </code>
/// <see cref="Derive"/> is a PURE function of (config, seed): the same pair always produces the same variant,
/// on any platform, using integer-only mixing — so an attempt need only store its <c>seed</c> and the exact
/// instance is reproducible for serving and for deterministic re-grading. A scenario with no <c>variant</c>
/// block, or seed 0, is returned unchanged (the canonical instance). This layer only reshapes the synthetic
/// <c>given</c> inputs; SimCalc still derives every answer, so the answer key is never stored.
/// </summary>
public static class SimVariant
{
    /// <summary>How many variants an author is expected to be able to generate — the validator proves each
    /// of seeds 1..<see cref="ValidationSeeds"/> stays gradable (§14 "impossible variant" rejection).</summary>
    public const int ValidationSeeds = 6;

    /// <summary>True when the scenario declares a non-empty <c>variant.vary</c> block.</summary>
    public static bool HasVariants(JsonElement config) =>
        config.ValueKind == JsonValueKind.Object
        && config.TryGetProperty("variant", out var v) && v.ValueKind == JsonValueKind.Object
        && v.TryGetProperty("vary", out var vary) && vary.ValueKind == JsonValueKind.Object
        && vary.EnumerateObject().Any();

    /// <summary>Derive the variant of <paramref name="configJson"/> for <paramref name="seed"/>. Returns the
    /// config JSON with each varied <c>given</c> value replaced by a deterministic pick inside its authored
    /// [min,max] range snapped to <c>step</c>. Seed 0, a config with no variant block, or any malformed
    /// input returns the original JSON unchanged (fail-safe: never invents a broken instance).</summary>
    public static string Derive(string? configJson, long seed)
    {
        if (string.IsNullOrWhiteSpace(configJson) || seed == 0) return configJson ?? "";
        JsonNode? root;
        try { root = JsonNode.Parse(configJson); }
        catch { return configJson; }
        if (root is not JsonObject obj) return configJson;

        if (obj["variant"] is not JsonObject variant || variant["vary"] is not JsonObject vary) return configJson;
        if (obj["given"] is not JsonObject given) return configJson;

        // Sorted keys so the seed→value mapping is stable regardless of JSON member order.
        var keys = vary.Select(kv => kv.Key).OrderBy(k => k, StringComparer.Ordinal).ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
            if (vary[key] is not JsonObject spec) continue;
            double? min = Num(spec["min"]);
            double? max = Num(spec["max"]);
            if (min is not double lo || max is not double hi || hi < lo) continue;
            double step = Num(spec["step"]) is double st && st > 0 ? st : 1;

            // Deterministic draw for this (seed, key-index): pick one of the discrete steps in [lo, hi].
            var steps = (long)Math.Floor((hi - lo) / step + 1e-9);
            var draw = Mix((ulong)seed * 0x100000001B3UL + (ulong)(i + 1));
            var idx = steps <= 0 ? 0 : (long)(draw % (ulong)(steps + 1));
            var value = lo + idx * step;
            given[key] = Round(value);
        }
        return obj.ToJsonString();
    }

    // SplitMix64-style finalizer — a fast, well-distributed integer hash. Deterministic and platform-neutral.
    static ulong Mix(ulong z)
    {
        z += 0x9E3779B97F4A7C15UL;
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    static double? Num(JsonNode? n)
    {
        if (n is null) return null;
        try { return n.GetValue<double>(); } catch { }
        try { return (double)n.GetValue<long>(); } catch { }
        return null;
    }

    // Keep whole-number ranges whole; otherwise round to 4 dp (parity with SimCalc.R rounding granularity).
    static JsonNode Round(double v)
    {
        var r = Math.Round(v, 4, MidpointRounding.AwayFromZero);
        return r == Math.Floor(r) ? JsonValue.Create((long)r) : JsonValue.Create(r);
    }
}
