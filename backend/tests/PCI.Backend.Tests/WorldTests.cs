using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — Phase 0/1 slice tests (docs/pciworld/PLAN.md test plan).
///
/// Proves the structural product rules: every pilot challenge passes the publication gate and a
/// full reference solve; the workspace payload cannot leak answers; scoring and profiles are
/// deterministic; the lifecycle enforces maker-checker and immutable versions (a revision never
/// changes what a pinned attempt replays); rotation/calendar behave; the admin realm is wholly
/// separate (a PCI admin token is worthless here); RBAC is a fixed matrix; public tokens are
/// stored only as SHA-256.
/// </summary>
public class WorldTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    const string MinimalConfig = """
        {"context":"A tiny project situation for lifecycle tests, with inputs PV 100 and AC 100.",
         "evidence":[{"label":"PV","value":"100"}],
         "task":"evm","given":{"pv":100,"ev":50,"ac":100,"bac":200},
         "ask":[{"key":"cv","label":"Cost Variance","type":"number"}],
         "tolerance":0.01,
         "decisions":[{"key":"d1","prompt":"Call it.","options":[
            {"key":"a","label":"Good call","quality":100,"consequence":"It works.","principle":"P."},
            {"key":"b","label":"Bad call","quality":0,"consequence":"It fails.","principle":"P."}]}],
         "share_line":"Solved the tiny test project."}
        """;

    static WorldContent.ChallengeInput Input(string code = "WC-TST-001", string? config = null) =>
        new(code, "Test", "Hook", "Testing", "project_controls", "foundation", 8, true, config ?? MinimalConfig);

    // ───────────────────────── content: pack + validator + leakage ─────────────────────────

    [Fact]
    public void Every_pilot_challenge_is_published_validated_and_fully_solvable()
    {
        var db = NewWorldDb();
        var rows = db.Query("SELECT * FROM pciworld_challenges WHERE author_id IS NULL");
        Assert.Equal(WorldContentPack.Count, rows.Count);
        Assert.Equal(30, rows.Count);

        // Release-2 gates: every difficulty and every track is represented, across a broad industry set.
        string S2(Dictionary<string, object?> r, string k) => Convert.ToString(r[k]) ?? "";
        foreach (var d in WorldContent.Difficulties) Assert.Contains(rows, r => S2(r, "difficulty") == d);
        foreach (var t in WorldContent.Tracks) Assert.Contains(rows, r => S2(r, "track") == t);
        Assert.True(rows.Select(r => S2(r, "industry")).Distinct().Count() >= 15,
            "expected broad industry coverage in the 30-challenge library");

        foreach (var r in rows)
        {
            Assert.Equal("published", Convert.ToString(r["status"]));
            Assert.True(Convert.ToInt64(r["current_version"]) >= 1);
            var issues = WorldContent.Validate(WorldLifecycle.InputFor(r));
            Assert.True(WorldContent.Publishable(issues),
                $"{r["code"]}: {string.Join("; ", issues.Where(i => i.Sev == WorldContent.Severity.Error).Select(i => i.Message))}");

            // The immutable snapshot exists and matches the working copy at seed time.
            var snap = WorldLifecycle.PinnedVersion(db, Convert.ToInt64(r["id"]), Convert.ToInt64(r["current_version"]));
            Assert.NotNull(snap);
            Assert.Equal(Convert.ToString(r["config_json"]), Convert.ToString(snap!["config_json"]));
        }
    }

    [Fact]
    public void Validator_rejects_each_error_class()
    {
        string[] bads =
        {
            """{"context":"c","share_line":"s","task":"nonsense","given":{},"ask":[{"key":"x"}],"tolerance":0.01}""",
            """{"context":"c","share_line":"s","task":"evm","given":{"pv":1,"ev":1,"ac":1},"ask":[{"key":"does_not_exist"}],"tolerance":0.01}""",
            """{"context":"c","share_line":"s","task":"evm","given":{"pv":1,"ev":1,"ac":1},"ask":[{"key":"cv"}],"tolerance":0}""",
            """{"context":"c","share_line":"s","decisions":[{"key":"d","prompt":"p","options":[{"key":"a","label":"only one","quality":50,"consequence":"c"}]}]}""",
            """{"context":"c","share_line":"s","decisions":[{"key":"d","prompt":"p","options":[{"key":"a","label":"a","quality":50,"consequence":"c"},{"key":"b","label":"b","quality":50,"consequence":"c"}]}]}""",
            """{"context":"c","share_line":"s"}""",
            """{"share_line":"s","task":"evm","given":{"pv":1,"ev":1,"ac":1},"ask":[{"key":"cv"}],"tolerance":0.01}""",
            "not json at all",
        };
        foreach (var bad in bads)
            Assert.False(WorldContent.Publishable(WorldContent.Validate(Input(config: bad))), bad);

        // Missing synthetic declaration blocks publication even with a valid config.
        var noSynthetic = Input() with { SyntheticDeclared = false };
        Assert.False(WorldContent.Publishable(WorldContent.Validate(noSynthetic)));

        // Answer leakage: the CV reference (-50) placed into visible evidence must be caught.
        var leaky = MinimalConfig.Replace("{\"label\":\"PV\",\"value\":\"100\"}",
            "{\"label\":\"Hint\",\"value\":\"the answer is -50 exactly\"}");
        Assert.Contains(WorldContent.Validate(Input(config: leaky)), i => i.Code == "leak");

        Assert.True(WorldContent.Publishable(WorldContent.Validate(Input())));
    }

    [Fact]
    public void Workspace_payload_is_an_allowlist_that_cannot_leak_answers()
    {
        var db = NewWorldDb();
        foreach (var r in db.Query("SELECT id,current_version FROM pciworld_challenges WHERE author_id IS NULL"))
        {
            var snap = WorldLifecycle.PinnedVersion(db, Convert.ToInt64(r["id"]), Convert.ToInt64(r["current_version"]))!;
            var view = JsonSerializer.Serialize(WorldContent.PublicView(Convert.ToString(snap["config_json"])!));
            // Guard the STRUCTURE: no rubric/solver field may be serialized as a key. (Prose may
            // legitimately mention e.g. a Quality department — keys are what leak answers.)
            Assert.DoesNotContain("\"quality\":", view);
            Assert.DoesNotContain("\"consequence\":", view);
            Assert.DoesNotContain("\"principle\":", view);
            Assert.DoesNotContain("\"share_line\":", view);
            Assert.DoesNotContain("\"given\":", view);
            Assert.DoesNotContain("\"profile_map\":", view);
            Assert.DoesNotContain("\"tolerance\":", view);
        }
    }

    // ───────────────────────── scoring + profiles ─────────────────────────

    [Fact]
    public void Scoring_is_deterministic_with_tolerance_partial_credit_and_sets()
    {
        var config = JsonDocument.Parse("""
            {"context":"c","share_line":"s","task":"cpm",
             "given":{"activities":[{"id":"A","dur":3,"preds":[]},{"id":"B","dur":5,"preds":["A"]},
                                    {"id":"C","dur":4,"preds":["A"]},{"id":"D","dur":2,"preds":["B"]},
                                    {"id":"E","dur":6,"preds":["C"]},{"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[{"key":"project_duration","label":"Duration","type":"number"},
                    {"key":"critical_path","label":"Path","type":"set"},
                    {"key":"float_D","label":"Float D","type":"number"}],
             "tolerance":0.001,
             "decisions":[{"key":"go","prompt":"p","options":[
               {"key":"a","label":"a","quality":100,"consequence":"c","principle":"p"},
               {"key":"b","label":"b","quality":40,"consequence":"c","principle":"p"}]}]}
            """).RootElement;

        var answers = JsonDocument.Parse("""{"project_duration":"16","critical_path":"a, c, e, f","float_D":"3","decision_go":"b"}""").RootElement;
        var r1 = WorldScore.Grade(config, answers);
        var r2 = WorldScore.Grade(config, answers);
        Assert.Equal(JsonSerializer.Serialize(r1), JsonSerializer.Serialize(r2));   // determinism

        Assert.Equal(100, r1.Calculation);          // duration + case-insensitive set + float all correct
        Assert.Equal(40, r1.Decision);              // chose the 40-quality option
        Assert.Equal(Math.Round((100.0 * 3 + 40) / 4, 1), r1.Score);   // count-weighted overall

        // Wrong set order fails; near-miss numeric inside tolerance passes.
        var near = JsonDocument.Parse("""{"project_duration":16.0005,"critical_path":"F,E,C,A","float_D":9}""").RootElement;
        var r3 = WorldScore.Grade(config, near);
        Assert.True(r3.Measures.Single(m => m.Key == "project_duration").Correct);
        Assert.False(r3.Measures.Single(m => m.Key == "critical_path").Correct);
        Assert.False(r3.Measures.Single(m => m.Key == "float_D").Correct);

        // Missing answers earn nothing but never throw.
        var r4 = WorldScore.Grade(config, default);
        Assert.Equal(0, r4.Calculation);
        Assert.Equal(0, r4.Decision);
    }

    [Fact]
    public void Decision_profiles_derive_from_dimensions_and_explain_themselves()
    {
        var config = JsonDocument.Parse(MinimalConfig).RootElement;

        // Calculation dominant (correct number, bad decision).
        var calc = WorldScore.Grade(config, JsonDocument.Parse("""{"cv":-50,"decision_d1":"b"}""").RootElement);
        Assert.Equal("Analytical Controller", calc.ProfileKey);
        Assert.Contains("calculation", calc.ProfileReason, StringComparison.OrdinalIgnoreCase);

        // Decision dominant (wrong number, best decision).
        var deci = WorldScore.Grade(config, JsonDocument.Parse("""{"cv":999,"decision_d1":"a"}""").RootElement);
        Assert.Equal("Decisive Project Leader", deci.ProfileKey);

        // Balanced (both perfect) — and the authored profile_map wins when present.
        var mapped = JsonDocument.Parse(MinimalConfig.Replace("\"share_line\"",
            "\"profile_map\":{\"balanced\":\"Strategic Project Controller\"},\"share_line\"")).RootElement;
        var both = WorldScore.Grade(mapped, JsonDocument.Parse("""{"cv":-50,"decision_d1":"a"}""").RootElement);
        Assert.Equal("Strategic Project Controller", both.ProfileKey);
        Assert.NotEqual("—", both.ImprovementArea);
    }

    // ───────────────────────── lifecycle: maker-checker + immutable replay ─────────────────────────

    static long Admin(Db db, string email, string role) =>
        db.ExecuteReturningId("INSERT INTO pciworld_admin_users(email,role,password_hash) VALUES(?,?,?)",
            email, role, "x");

    static long Draft(Db db, long authorId, string code = "WC-TST-100")
    {
        return db.ExecuteReturningId(@"INSERT INTO pciworld_challenges
            (code,title,hook,industry,track,difficulty,est_minutes,synthetic_declared,config_json,author_id)
            VALUES(?, 'Test', 'Hook', 'Testing', 'project_controls', 'foundation', 8, 1, ?, ?)",
            code, MinimalConfig, authorId);
    }

    [Fact]
    public void Lifecycle_enforces_maker_checker_and_validation_gates()
    {
        var db = NewWorldDb();
        var author = Admin(db, "author@t.local", "author");
        var reviewer = Admin(db, "reviewer@t.local", "reviewer");
        var id = Draft(db, author);

        Assert.Equal("not_in_review", WorldLifecycle.Approve(db, id, reviewer));   // must go through review
        Assert.Null(WorldLifecycle.SubmitReview(db, id));
        Assert.Equal("not_approved", WorldLifecycle.Publish(db, id, reviewer));    // no publish before approval
        Assert.Equal("maker_checker", WorldLifecycle.Approve(db, id, author));     // author cannot self-approve
        Assert.Null(WorldLifecycle.Approve(db, id, reviewer));
        Assert.Null(WorldLifecycle.Publish(db, id, reviewer));                     // approved → published, snapshot v1
        Assert.NotNull(WorldLifecycle.PinnedVersion(db, id, 1));

        // An invalid draft can never pass approve.
        var bad = Draft(db, author, "WC-TST-101");
        db.Execute("UPDATE pciworld_challenges SET config_json='{}' WHERE id=?", bad);
        WorldLifecycle.SubmitReview(db, bad);
        Assert.Equal("not_publishable", WorldLifecycle.Approve(db, bad, reviewer));
    }

    [Fact]
    public void Published_versions_are_immutable_and_pinned_attempts_replay_identically()
    {
        var db = NewWorldDb();
        var author = Admin(db, "a2@t.local", "author");
        var reviewer = Admin(db, "r2@t.local", "reviewer");
        var id = Draft(db, author, "WC-TST-200");
        WorldLifecycle.SubmitReview(db, id);
        WorldLifecycle.Approve(db, id, reviewer);
        WorldLifecycle.Publish(db, id, reviewer);

        // A participant pins v1 and completes with full marks.
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s1')");
        var att = db.ExecuteReturningId(
            "INSERT INTO pciworld_attempts(session_id,challenge_id,version,answers_json,status) VALUES(?,?,1,?, 'completed')",
            sess, id, """{"cv":-50,"decision_d1":"a"}""");
        double GradePinned()
        {
            var snap = WorldLifecycle.PinnedVersion(db, id, 1)!;
            var cfg = JsonDocument.Parse(Convert.ToString(snap["config_json"])!).RootElement;
            var ans = JsonDocument.Parse(Convert.ToString(
                db.QueryOne("SELECT answers_json FROM pciworld_attempts WHERE id=?", att)!["answers_json"])!).RootElement;
            return WorldScore.Grade(cfg, ans).Score;
        }
        Assert.Equal(100, GradePinned());

        // Revise → change the numbers → approve → publish v2.
        Assert.Null(WorldLifecycle.Revise(db, id));
        db.Execute("UPDATE pciworld_challenges SET config_json=? WHERE id=?",
            MinimalConfig.Replace("\"ev\":50", "\"ev\":80"), id);
        WorldLifecycle.SubmitReview(db, id);
        WorldLifecycle.Approve(db, id, reviewer);
        Assert.Null(WorldLifecycle.Publish(db, id, reviewer));

        var live = WorldLifecycle.LiveVersion(db, id)!;
        Assert.Equal(2L, Convert.ToInt64(live["version"]));
        Assert.Contains("\"ev\":80", Convert.ToString(live["config_json"]));

        // The pinned v1 snapshot is untouched and the historical attempt still grades 100.
        Assert.Contains("\"ev\":50", Convert.ToString(WorldLifecycle.PinnedVersion(db, id, 1)!["config_json"]));
        Assert.Equal(100, GradePinned());

        // Draft edits are refused once published again (only drafts are editable).
        Assert.False(WorldLifecycle.CanEdit("published"));
        Assert.False(WorldLifecycle.CanEdit("approved"));
        Assert.True(WorldLifecycle.CanEdit("draft"));
    }

    [Fact]
    public void Reseeding_the_pack_is_idempotent_and_bumps_only_on_config_change()
    {
        var db = NewWorldDb();
        var before = db.Query("SELECT code,current_version FROM pciworld_challenges WHERE author_id IS NULL")
            .ToDictionary(r => Convert.ToString(r["code"])!, r => Convert.ToInt64(r["current_version"]));
        WorldSchema.Ensure(db);   // second boot, identical content
        foreach (var r in db.Query("SELECT code,current_version FROM pciworld_challenges WHERE author_id IS NULL"))
            Assert.Equal(before[Convert.ToString(r["code"])!], Convert.ToInt64(r["current_version"]));

        // Simulate an older deploy's row: hand-rewrite one pilot's config, reseed → new version,
        // old snapshot intact.
        var row = db.QueryOne("SELECT id,current_version,config_json FROM pciworld_challenges WHERE code='WC-EVM-001'")!;
        var id = Convert.ToInt64(row["id"]);
        var v = Convert.ToInt64(row["current_version"]);
        db.Execute("UPDATE pciworld_challenges SET config_json=? WHERE id=?", MinimalConfig, id);
        db.Execute("DELETE FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?", id, v);
        db.Execute(@"INSERT INTO pciworld_challenge_versions(challenge_id,version,title,config_json)
            VALUES(?,?, 'old', ?)", id, v, MinimalConfig);
        WorldSchema.Ensure(db);
        var after = db.QueryOne("SELECT current_version FROM pciworld_challenges WHERE id=?", id)!;
        Assert.Equal(v + 1, Convert.ToInt64(after["current_version"]));
        Assert.Equal(MinimalConfig, Convert.ToString(WorldLifecycle.PinnedVersion(db, id, v)!["config_json"]));
    }

    // ───────────────────────── rotation + calendar ─────────────────────────

    [Fact]
    public void Daily_rotation_is_deterministic_and_calendar_overrides_it()
    {
        var db = NewWorldDb();
        var day = new DateTime(2026, 7, 24, 12, 0, 0, DateTimeKind.Utc);
        var a = WorldLifecycle.Today(db, day);
        var b = WorldLifecycle.Today(db, day);
        Assert.NotNull(a);
        Assert.Equal(Convert.ToInt64(a!["id"]), Convert.ToInt64(b!["id"]));   // same day, same challenge

        // Calendar override wins; a retired challenge cannot serve.
        var target = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code='WC-AIA-010'")!;
        db.Execute("INSERT INTO pciworld_calendar(day_utc,challenge_id) VALUES(?,?)",
            day.ToString("yyyy-MM-dd"), target["id"]);
        Assert.Equal(Convert.ToInt64(target["id"]), Convert.ToInt64(WorldLifecycle.Today(db, day)!["id"]));

        WorldLifecycle.Retire(db, Convert.ToInt64(target["id"]));
        Assert.NotEqual(Convert.ToInt64(target["id"]), Convert.ToInt64(WorldLifecycle.Today(db, day)!["id"]));
        WorldLifecycle.Restore(db, Convert.ToInt64(target["id"]));
    }

    // ───────────────────────── content reports ─────────────────────────

    [Fact]
    public void Content_reports_open_resolve_once_and_keep_the_audit_trail()
    {
        var db = NewWorldDb();
        var ch = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code='WC-EVM-001'")!;
        var id = db.ExecuteReturningId(
            "INSERT INTO pciworld_reports(challenge_id,category,message) VALUES(?, 'calculation', 'The tolerance note could state units.')",
            ch["id"]);

        Assert.Contains("calculation", Endpoints.World.WorldReportCategories);
        Assert.Equal(5, Endpoints.World.WorldReportCategories.Length);

        // First resolve wins; a second resolve finds nothing open (same guard the endpoint uses).
        var reviewer = Admin(db, "rep-rev@t.local", "reviewer");
        var n1 = db.Execute(@"UPDATE pciworld_reports SET status='resolved', resolution='Wording clarified in v2.',
            resolved_by=?, resolved_at=datetime('now') WHERE id=? AND status='open'", reviewer, id);
        var n2 = db.Execute(@"UPDATE pciworld_reports SET status='resolved', resolution='dup',
            resolved_by=?, resolved_at=datetime('now') WHERE id=? AND status='open'", reviewer, id);
        Assert.Equal(1, n1);
        Assert.Equal(0, n2);
        var row = db.QueryOne("SELECT * FROM pciworld_reports WHERE id=?", id)!;
        Assert.Equal("resolved", Convert.ToString(row["status"]));
        Assert.Equal("Wording clarified in v2.", Convert.ToString(row["resolution"]));
        Assert.Equal(reviewer, Convert.ToInt64(row["resolved_by"]));
    }

    // ───────────────────────── realm separation + RBAC + tokens ─────────────────────────

    [Fact]
    public void Rbac_matrix_is_fixed_and_unknown_roles_get_nothing()
    {
        Assert.True(WorldRbac.Allowed("owner", "admin"));
        Assert.True(WorldRbac.Allowed("author", "author"));
        Assert.False(WorldRbac.Allowed("author", "review"));      // maker cannot check
        Assert.False(WorldRbac.Allowed("author", "publish"));
        Assert.True(WorldRbac.Allowed("reviewer", "review"));
        Assert.False(WorldRbac.Allowed("reviewer", "publish"));
        Assert.True(WorldRbac.Allowed("publisher", "publish"));
        Assert.False(WorldRbac.Allowed("publisher", "author"));
        Assert.False(WorldRbac.Allowed("viewer", "author"));
        Assert.True(WorldRbac.Allowed("viewer", "read"));
        Assert.False(WorldRbac.Allowed("stranger", "read"));
        Assert.False(WorldRbac.Allowed(null, "read"));
    }

    [Fact]
    public void World_admin_realm_rejects_pci_admin_tokens_and_stores_only_hashes()
    {
        var db = NewWorldDb();
        // A live PCI-admin session token must be meaningless in the PCI World realm.
        var raw = Security.RandomHex(32);
        var pciAdmin = db.ExecuteReturningId(
            "INSERT INTO admin_users(email,name,role,password_hash,status) VALUES('boss@pci.local','B','owner','x','active')");
        db.Execute("INSERT INTO admin_sessions(admin_id,token,expires_at) VALUES(?,?, datetime('now','+8 hours'))",
            pciAdmin, Security.Sha(raw));
        var ctx = new DefaultHttpContext();
        ctx.Request.Headers.Authorization = "Bearer " + raw;
        Assert.Null(Endpoints.WorldAdmin.FromReq(ctx.Request, db));

        // A genuine world-admin session resolves — and only via its own table.
        var wa = db.ExecuteReturningId(
            "INSERT INTO pciworld_admin_users(email,role,password_hash) VALUES('w@pciworld.local','publisher','x')");
        var wraw = Security.RandomHex(32);
        db.Execute("INSERT INTO pciworld_admin_sessions(admin_id,token,expires_at) VALUES(?,?, datetime('now','+8 hours'))",
            wa, Security.Sha(wraw));
        var wctx = new DefaultHttpContext();
        wctx.Request.Headers.Authorization = "Bearer " + wraw;
        var resolved = Endpoints.WorldAdmin.FromReq(wctx.Request, db);
        Assert.NotNull(resolved);
        Assert.Equal("publisher", resolved!.Role);

        // Raw tokens never appear in storage — sessions hold SHA-256 only.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_sessions WHERE token=?", wraw));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_sessions WHERE token=?", Security.Sha(wraw)));

        // The bootstrap owner exists with a bcrypt hash, never a plaintext password.
        var owner = db.QueryOne("SELECT * FROM pciworld_admin_users WHERE email='owner@pciworld.local'")!;
        Assert.StartsWith("$2", Convert.ToString(owner["password_hash"]));
    }
}
