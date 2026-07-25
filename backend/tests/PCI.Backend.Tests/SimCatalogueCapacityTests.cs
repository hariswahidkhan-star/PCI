using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// P3 capacity gate for the student catalogue (Core.SimLab.Catalogue — the server-side search/facet path
/// the SPA drives). Seeds the catalogue to the 300-scenario capacity target and proves the single-pass
/// build stays correct (facets + whole-catalogue summary over the FULL set, rows carry only the matches)
/// and fast (no O(n²)/N+1 regression) at that scale. Pure Core over a temp DB — no HTTP, no wall-clock
/// flakiness beyond a deliberately generous ceiling.
/// </summary>
public class SimCatalogueCapacityTests
{
    const int N = 300;                          // the P3 capacity target for published scenarios
    static readonly string[] Difficulties = { "foundation", "intermediate", "advanced", "expert" };
    static readonly string[] Kinds = { "guided_lab", "skill_drill", "scenario", "capstone" };
    static readonly string[] Industries = { "Construction", "Energy", "Rail", "Defence", "Water" };

    // A large, varied published catalogue on top of whatever SimLabSchema seeds. Every 4th scenario
    // exercises earned_value so competency filtering has a substantial, predictable match set.
    static (Db db, int seeded) SeedScale()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);
        var seeded = db.Scalar<long>("SELECT COUNT(*) FROM simulation_scenarios WHERE status='published'");
        db.Transaction(() =>
        {
            for (var i = 0; i < N; i++)
            {
                var comps = i % 4 == 0 ? "[\"earned_value\",\"forecasting\"]" : "[\"scheduling\"]";
                db.Execute(@"INSERT INTO simulation_scenarios
                    (scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,summary,config_json,status,sort_order)
                    VALUES(?,?,?,?,?,?,?,?,?, 'published', ?)",
                    $"CAP-{i:D4}",
                    $"Capacity scenario {i} — recovery drill",
                    Kinds[i % Kinds.Length],
                    Industries[i % Industries.Length],
                    Difficulties[i % Difficulties.Length],
                    10 + i % 40,
                    comps,
                    $"Synthetic capacity scenario {i}.",
                    "{}",                                  // interactive=true (non-empty config)
                    i);
            }
        });
        return (db, (int)seeded);
    }

    static JsonElement Cat(Db db, string? q = null, string? difficulty = null, string? kind = null,
        string? industry = null, string? competency = null)
        => JsonSerializer.SerializeToElement(SimLab.Catalogue(db, 999_999, q, difficulty, kind, industry, competency));

    [Fact]
    public void Catalogue_is_correct_at_capacity_scale()
    {
        var (db, seeded) = SeedScale();
        var total = seeded + N;

        // Unfiltered: total == matched == every published scenario; the summary is full-set; a user with
        // no attempts has zero progress and an empty resume list.
        var all = Cat(db);
        Assert.Equal(total, all.GetProperty("total").GetInt32());
        Assert.Equal(total, all.GetProperty("matched").GetInt32());
        Assert.Equal(total, all.GetProperty("rows").GetArrayLength());
        Assert.Equal(total, all.GetProperty("summary").GetProperty("published").GetInt32());
        Assert.Equal(0, all.GetProperty("summary").GetProperty("completed").GetInt32());
        Assert.Equal(0, all.GetProperty("summary").GetProperty("in_progress").GetInt32());
        Assert.Equal(JsonValueKind.Null, all.GetProperty("summary").GetProperty("avg_score").ValueKind);
        Assert.Empty(all.GetProperty("resume").EnumerateArray());

        // Facets are computed over the full set and cover all four difficulties.
        var diffs = all.GetProperty("facets").GetProperty("difficulties").EnumerateArray().Select(e => e.GetString()).ToList();
        Assert.All(Difficulties, d => Assert.Contains(d, diffs));
        Assert.Contains("earned_value", all.GetProperty("facets").GetProperty("competencies").EnumerateArray().Select(e => e.GetString()));

        // Difficulty filter: a strict subset, and every returned row matches. total/summary stay full-set.
        var expert = Cat(db, difficulty: "expert");
        var expertRows = expert.GetProperty("rows").EnumerateArray().ToList();
        Assert.Equal(N / 4, expertRows.Count(r => r.GetProperty("scenario_code").GetString()!.StartsWith("CAP-")));
        Assert.All(expertRows, r => Assert.Equal("expert", r.GetProperty("difficulty").GetString()));
        Assert.True(expert.GetProperty("matched").GetInt32() < total, "a filter must narrow the matches");
        Assert.Equal(total, expert.GetProperty("total").GetInt32());
        Assert.Equal(total, expert.GetProperty("summary").GetProperty("published").GetInt32());   // invariant under filter

        // Competency filter: every returned row exercises the competency; count matches the seeding rule.
        var ev = Cat(db, competency: "earned_value");
        var evRows = ev.GetProperty("rows").EnumerateArray().ToList();
        Assert.Equal(N / 4, evRows.Count(r => r.GetProperty("scenario_code").GetString()!.StartsWith("CAP-")));
        Assert.All(evRows, r => Assert.Contains("earned_value",
            r.GetProperty("competencies").EnumerateArray().Select(e => e.GetString())));

        // Free-text search over the seeded set — case-insensitive, matches the title.
        var q = Cat(db, q: "RECOVERY drill");
        Assert.True(q.GetProperty("matched").GetInt32() >= N, "search should find the capacity scenarios");
        Assert.All(q.GetProperty("rows").EnumerateArray(),
            r => Assert.Contains("recovery", r.GetProperty("title").GetString()!.ToLowerInvariant()));
    }

    [Fact]
    public void Catalogue_build_stays_fast_at_capacity_scale()
    {
        var (db, _) = SeedScale();
        // Warm the query plan / connection, then time repeated builds. A single-pass O(n) build over ~300
        // rows is a few ms; the ceiling is deliberately generous so it only trips on a gross regression
        // (an accidental per-row query or O(n²) scan), never on ordinary CI jitter.
        _ = Cat(db);
        var sw = Stopwatch.StartNew();
        const int iters = 20;
        for (var i = 0; i < iters; i++) _ = SimLab.Catalogue(db, 999_999, null, null, null, null, null);
        sw.Stop();
        Assert.True(sw.ElapsedMilliseconds < 2000,
            $"{iters} catalogue builds at capacity took {sw.ElapsedMilliseconds}ms (>2000ms suggests an N+1/O(n²) regression)");
    }
}
