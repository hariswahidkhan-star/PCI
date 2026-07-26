using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI Project Intelligence — taxonomy, classification and Year-1 plan gates
/// (docs/pciworld/PROJECT_INTELLIGENCE.md).
///
/// These tests are the reason coverage can never be claimed before it is real: the committed
/// Year-1 plan must carry exactly 420 slots in the approved code ranges, every distribution table
/// must match the approved counts EXACTLY, every mapped slot must agree with the C# classification
/// and point at a published bank challenge, and no learner-facing string may describe the
/// programme in game/puzzle language. The taxonomy backfill must be idempotent and must never
/// touch config_json or a version snapshot (replay immutability).
/// </summary>
public class WorldIntelligenceTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static WorldIntelligence.YearPlan Plan()
    {
        var plan = WorldIntelligence.LoadYearOne();
        Assert.NotNull(plan);
        return plan!;
    }

    // ───────────────────────── taxonomy + classification ─────────────────────────

    [Fact]
    public void Vocabularies_have_the_approved_cardinalities()
    {
        Assert.Equal(8, WorldIntelligence.ExperienceTypes.Count);
        Assert.Equal(12, WorldIntelligence.Domains.Count);
        Assert.Equal(6, WorldIntelligence.LifecycleStages.Count);
        Assert.Equal(10, WorldIntelligence.Sectors.Count);
        Assert.Equal(6, WorldIntelligence.Interactions.Count);
        Assert.Equal(4, WorldIntelligence.DurationBands.Count);
        Assert.Equal(4, WorldIntelligence.DifficultyBands.Count);
    }

    [Fact]
    public void Every_house_challenge_is_classified_with_valid_vocabulary()
    {
        var db = NewWorldDb();
        var rows = db.Query("SELECT code,pi_type,pi_domain,pi_lifecycle,pi_sector,pi_interaction FROM pciworld_challenges WHERE author_id IS NULL");
        Assert.Equal(WorldContentPack.Count + WorldIntelligencePack.Count, rows.Count);
        Assert.Equal(226, rows.Count);
        foreach (var r in rows)
        {
            var code = H.Str(r["code"])!;
            Assert.True(WorldIntelligence.Classification.ContainsKey(code), $"{code} has no classification");
            var m = WorldIntelligence.Classification[code];
            Assert.Equal(m.Type, H.Str(r["pi_type"]));
            Assert.Equal(m.Domain, H.Str(r["pi_domain"]));
            Assert.Equal(m.Lifecycle, H.Str(r["pi_lifecycle"]));
            Assert.Equal(m.Sector, H.Str(r["pi_sector"]));
            Assert.Equal(m.Interaction, H.Str(r["pi_interaction"]));
            Assert.Empty(WorldIntelligence.ValidateMeta(m.Type, m.Domain, m.Lifecycle, m.Sector, m.Interaction));
        }
    }

    [Fact]
    public void Backfill_is_idempotent_and_never_touches_config_or_versions()
    {
        var db = NewWorldDb();
        var beforeConfigs = db.Query("SELECT code,config_json FROM pciworld_challenges ORDER BY code");
        var beforeVersions = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_challenge_versions");
        WorldIntelligence.Backfill(db);   // second run on an already-backfilled bank
        WorldIntelligence.Backfill(db);
        var afterConfigs = db.Query("SELECT code,config_json FROM pciworld_challenges ORDER BY code");
        Assert.Equal(beforeVersions, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_challenge_versions"));
        Assert.Equal(beforeConfigs.Count, afterConfigs.Count);
        for (var i = 0; i < beforeConfigs.Count; i++)
            Assert.Equal(H.Str(beforeConfigs[i]["config_json"]), H.Str(afterConfigs[i]["config_json"]));
    }

    [Fact]
    public void Operator_authored_rows_are_never_reclassified()
    {
        var db = NewWorldDb();
        db.Execute("INSERT INTO pciworld_admin_users(email,name,role,password_hash) VALUES('a@w','A','author','x')");
        var adminId = db.Scalar<long>("SELECT id FROM pciworld_admin_users WHERE email='a@w'");
        // An operator row that reuses a classified code (post-takeover edit): backfill must skip it.
        db.Execute("UPDATE pciworld_challenges SET author_id=?, pi_type='stakeholder_dilemma' WHERE code='WC-EVM-001'", adminId);
        WorldIntelligence.Backfill(db);
        Assert.Equal("stakeholder_dilemma",
            H.Str(db.QueryOne("SELECT pi_type FROM pciworld_challenges WHERE code='WC-EVM-001'")!["pi_type"]));
    }

    [Fact]
    public void Premium_language_gate_flags_banned_terms_and_passes_the_live_bank()
    {
        Assert.NotEmpty(WorldIntelligence.LanguageIssues("A fun mini-game about EVM"));
        Assert.NotEmpty(WorldIntelligence.LanguageIssues("Daily trivia puzzle"));
        Assert.Empty(WorldIntelligence.LanguageIssues("A governance escalation — the display plays out over weeks"));
        var db = NewWorldDb();
        foreach (var r in db.Query("SELECT code,title,hook FROM pciworld_challenges"))
            Assert.Empty(WorldIntelligence.LanguageIssues(H.Str(r["title"]), H.Str(r["hook"])));
    }

    // ───────────────────────── progressive hints (PI-US-051) ─────────────────────────

    [Fact]
    public void Every_year1_pack_item_carries_exactly_three_hints_that_pass_the_gates()
    {
        var db = NewWorldDb();
        var rows = db.Query("SELECT code,title,hook,config_json FROM pciworld_challenges WHERE author_id IS NULL");
        var packCodes = WorldIntelligencePack.Codes.ToHashSet(StringComparer.Ordinal);
        Assert.Equal(WorldIntelligencePack.Count, packCodes.Count);
        foreach (var r in rows.Where(r => packCodes.Contains(H.Str(r["code"])!)))
        {
            var hints = WorldContent.Hints(H.Str(r["config_json"])!);
            Assert.True(hints.Count == 3, $"{r["code"]}: expected 3 hints, found {hints.Count}");
            foreach (var h in hints)
            {
                Assert.False(string.IsNullOrWhiteSpace(h));
                Assert.Empty(WorldIntelligence.LanguageIssues(h));
            }
        }
    }

    [Fact]
    public void Hint_validation_requires_exactly_three_and_scans_for_leakage()
    {
        // Two hints → invalid.
        var two = """
            {"context":"Ctx with PV 100.","share_line":"s",
             "task":"evm","given":{"pv":100,"ev":50,"ac":100,"bac":200},
             "ask":[{"key":"cv","label":"CV","type":"number"}],"tolerance":0.01,
             "hints":["one","two"]}
            """;
        var issues = WorldContent.Validate(new WorldContent.ChallengeInput(
            "WC-TST-901", "T", "H", "I", "project_controls", "foundation", 8, true, two));
        Assert.Contains(issues, i => i.Code == "hints" && i.Sev == WorldContent.Severity.Error);

        // A hint that hands over the reference value (cv = -50 ... use a config whose answer is long enough to scan).
        var leak = """
            {"context":"Ctx.","share_line":"s",
             "task":"evm","given":{"pv":900000,"ev":750000,"ac":800000,"bac":2000000},
             "ask":[{"key":"cv","label":"CV","type":"number"}],"tolerance":0.01,
             "hints":["EV minus AC","Look closely","The answer is -50000"]}
            """;
        var issues2 = WorldContent.Validate(new WorldContent.ChallengeInput(
            "WC-TST-902", "T", "H", "I", "project_controls", "foundation", 8, true, leak));
        Assert.Contains(issues2, i => i.Code == "leak" && i.Sev == WorldContent.Severity.Error);
    }

    [Fact]
    public void PublicView_exposes_hint_count_but_never_hint_text()
    {
        var db = NewWorldDb();
        var row = db.QueryOne("SELECT config_json FROM pciworld_challenges WHERE code='WC-GOV-053'");
        var json = System.Text.Json.JsonSerializer.Serialize(WorldContent.PublicView(H.Str(row!["config_json"])!));
        var view = System.Text.Json.JsonDocument.Parse(json).RootElement;
        Assert.Equal(3, view.GetProperty("hints_available").GetInt32());
        foreach (var hint in WorldContent.Hints(H.Str(row["config_json"])!))
            Assert.DoesNotContain(hint, json, StringComparison.Ordinal);
    }

    [Fact]
    public void Attempts_track_hint_reveals_in_a_guarded_column()
    {
        var db = NewWorldDb();
        Assert.Contains("hints_used", db.Columns("pciworld_attempts"));
        db.Execute("INSERT INTO pciworld_sessions(token_sha) VALUES('t')");
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE code='WC-GOV-053'")!;
        var att = db.ExecuteReturningId("INSERT INTO pciworld_attempts(session_id,challenge_id,version,answers_json) VALUES(1,?,?, '{}')",
            H.L(ch["id"]), H.L(ch["current_version"]));
        // The guarded increment: only one of two concurrent identical updates wins.
        var n1 = db.Execute("UPDATE pciworld_attempts SET hints_used=? WHERE id=? AND hints_used=?", 1, att, 0);
        var n2 = db.Execute("UPDATE pciworld_attempts SET hints_used=? WHERE id=? AND hints_used=?", 1, att, 0);
        Assert.Equal(1, n1);
        Assert.Equal(0, n2);
    }

    // ───────────────────────── Year-1 plan: structure ─────────────────────────

    [Fact]
    public void Year1_plan_has_exactly_420_slots_in_the_approved_code_ranges()
    {
        var plan = Plan();
        Assert.Equal(365, plan.Scheduled.Count);
        Assert.Equal(55, plan.Reserve.Count);
        var codes = plan.Scheduled.Select(e => e.Code).Concat(plan.Reserve.Select(e => e.Code)).ToList();
        Assert.Equal(420, codes.Distinct(StringComparer.Ordinal).Count());
        for (var d = 1; d <= 365; d++)
        {
            Assert.Equal($"PI-Y1-D{d:000}", plan.Scheduled[d - 1].Code);
            Assert.Equal(d, plan.Scheduled[d - 1].Day);
        }
        for (var r = 1; r <= 55; r++) Assert.Equal($"PI-Y1-R{r:000}", plan.Reserve[r - 1].Code);
    }

    [Fact]
    public void Year1_monthly_counts_match_the_approved_calendar()
    {
        var plan = Plan();
        var monthDays = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        var day = 1;
        for (var m = 0; m < 12; m++)
        {
            var slice = plan.Scheduled.Where(e => e.Day >= day && e.Day < day + monthDays[m]).ToList();
            Assert.Equal(monthDays[m], slice.Count);
            day += monthDays[m];
        }
    }

    static void AssertDistribution(IEnumerable<WorldIntelligence.PlanEntry> entries,
        Func<WorldIntelligence.PlanEntry, string> facet, Dictionary<string, int> approved)
    {
        var got = entries.GroupBy(facet).ToDictionary(g => g.Key, g => g.Count());
        foreach (var (k, v) in approved)
            Assert.True(got.TryGetValue(k, out var n) && n == v, $"'{k}': approved {v}, plan has {(got.TryGetValue(k, out var x) ? x : 0)}");
        Assert.Equal(approved.Values.Sum(), got.Values.Sum());
    }

    [Fact]
    public void Year1_experience_type_distribution_is_exact()
    {
        var plan = Plan();
        AssertDistribution(plan.Scheduled, e => e.Type, new()
        {
            ["daily_decision"] = 120, ["project_rescue"] = 50, ["risk_room"] = 43,
            ["schedule_strategy"] = 43, ["cost_value"] = 37, ["stakeholder_dilemma"] = 33,
            ["logic_sequence"] = 26, ["executive_mission"] = 13,
        });
        AssertDistribution(plan.Reserve, e => e.Type, new()
        {
            ["daily_decision"] = 20, ["project_rescue"] = 8, ["risk_room"] = 6,
            ["schedule_strategy"] = 6, ["cost_value"] = 5, ["stakeholder_dilemma"] = 4,
            ["logic_sequence"] = 4, ["executive_mission"] = 2,
        });
    }

    [Fact]
    public void Year1_domain_distribution_is_exact()
    {
        AssertDistribution(Plan().Scheduled, e => e.Domain, new()
        {
            ["integration_governance"] = 45, ["scope_requirements"] = 32, ["schedule_planning"] = 45,
            ["cost_commercial"] = 42, ["risk_uncertainty"] = 42, ["quality_assurance"] = 26,
            ["resources_leadership"] = 28, ["stakeholders_communication"] = 38,
            ["procurement_contracts"] = 28, ["change_claims_recovery"] = 20,
            ["safety_sustainability"] = 10, ["digital_data_ai"] = 9,
        });
    }

    [Fact]
    public void Year1_difficulty_duration_interaction_lifecycle_sector_distributions_are_exact()
    {
        var plan = Plan();
        AssertDistribution(plan.Scheduled, e => e.DifficultyBand, new()
        { ["foundation"] = 105, ["practitioner"] = 150, ["advanced"] = 90, ["executive"] = 20 });
        AssertDistribution(plan.Scheduled, e => e.DurationBand, new()
        { ["quick"] = 150, ["standard"] = 130, ["deep"] = 65, ["capstone"] = 20 });
        AssertDistribution(plan.Scheduled, e => e.Interaction, new()
        {
            ["single_decision"] = 140, ["order_rank"] = 45, ["numeric_calculation"] = 65,
            ["multi_stage_decision"] = 55, ["evidence_diagnosis"] = 40, ["negotiation_communication"] = 20,
        });
        AssertDistribution(plan.Scheduled, e => e.Lifecycle, new()
        {
            ["concept_business_case"] = 45, ["definition_planning"] = 85, ["procurement_mobilization"] = 45,
            ["execution_control"] = 130, ["commissioning_handover"] = 35, ["closeout_lessons"] = 25,
        });
        AssertDistribution(plan.Scheduled, e => e.Sector, new()
        {
            ["cross_sector"] = 105, ["construction_infrastructure"] = 55, ["energy_utilities"] = 40,
            ["technology_digital"] = 40, ["manufacturing_industrial"] = 30, ["transport_logistics"] = 25,
            ["public_sector"] = 25, ["healthcare_life_sciences"] = 20, ["climate_sustainability"] = 15,
            ["professional_services_other"] = 10,
        });
    }

    [Fact]
    public void Year1_every_slot_uses_approved_vocabulary_and_consistent_bands()
    {
        var plan = Plan();
        foreach (var e in plan.Scheduled.Concat(plan.Reserve))
        {
            Assert.Empty(WorldIntelligence.ValidateMeta(e.Type, e.Domain, e.Lifecycle, e.Sector, e.Interaction));
            Assert.True(WorldIntelligence.DifficultyBands.ContainsKey(e.DifficultyBand), $"{e.Code}: bad difficulty band");
            Assert.True(WorldIntelligence.DurationBands.ContainsKey(e.DurationBand), $"{e.Code}: bad duration band");
            Assert.Equal(WorldIntelligence.DurationBand(e.EstMinutes), e.DurationBand);
            Assert.False(string.IsNullOrWhiteSpace(e.Title), $"{e.Code}: empty title");
            Assert.Empty(WorldIntelligence.LanguageIssues(e.Title));
        }
        // Working titles must be materially distinct — a plan of copies is not a plan.
        var titles = plan.Scheduled.Concat(plan.Reserve).Select(e => e.Title).ToList();
        Assert.Equal(titles.Count, titles.Distinct(StringComparer.Ordinal).Count());
    }

    // ───────────────────────── Year-1 plan: mapped slots vs the live bank ─────────────────────────

    [Fact]
    public void Year1_mapped_slots_agree_with_the_classification_and_the_published_bank()
    {
        var db = NewWorldDb();
        var plan = Plan();
        var mapped = plan.Scheduled.Where(e => e.Status == "mapped").ToList();
        Assert.Equal(226, mapped.Count);
        Assert.All(plan.Reserve, e => Assert.Equal("planned", e.Status));
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var e in mapped)
        {
            Assert.NotNull(e.SourceChallenge);
            Assert.True(seen.Add(e.SourceChallenge!), $"{e.SourceChallenge} mapped twice");
            var ch = db.QueryOne("SELECT * FROM pciworld_challenges WHERE code=? AND current_version>=1 AND retired=0", e.SourceChallenge);
            Assert.True(ch is not null, $"{e.Code}: source {e.SourceChallenge} is not a servable bank challenge");
            var m = WorldIntelligence.Classification[e.SourceChallenge!];
            Assert.Equal(m.Type, e.Type);
            Assert.Equal(m.Domain, e.Domain);
            Assert.Equal(m.Lifecycle, e.Lifecycle);
            Assert.Equal(m.Sector, e.Sector);
            Assert.Equal(m.Interaction, e.Interaction);
            Assert.Equal(WorldIntelligence.DifficultyBand(H.Str(ch!["difficulty"])), e.DifficultyBand);
            Assert.Equal(WorldIntelligence.DurationBand(H.L(ch["est_minutes"])), e.DurationBand);
            Assert.Equal(H.Str(ch["title"]), e.Title);
        }
        // Every classified bank challenge appears in the plan exactly once.
        Assert.Equal(WorldIntelligence.Classification.Count, mapped.Count);
    }

    // ───────────────────────── coverage report ─────────────────────────

    [Fact]
    public void Coverage_report_counts_honestly_and_raises_the_runway_alert()
    {
        var db = NewWorldDb();
        var plan = Plan();
        var cov = WorldIntelligence.Coverage(db, plan);
        var t = cov.GetType();
        object G(string name) => t.GetProperty(name)!.GetValue(cov)!;
        Assert.Equal(365, (int)G("scheduled_total"));
        Assert.Equal(55, (int)G("reserve_total"));
        Assert.Equal(420, (int)G("bank_total"));
        Assert.Equal(226, (int)G("mapped"));
        Assert.Equal(139, (int)G("planned"));
        Assert.Equal(226, (int)G("backed_by_published_challenge"));
        // January–July are fully authored: 212 consecutive backed days from day 1, with August
        // day 213 planned. The 60-day alert stays cleared — with real published content.
        Assert.Equal(212, (int)G("runway_days"));
        Assert.False((bool)G("runway_alert"));
    }
}
