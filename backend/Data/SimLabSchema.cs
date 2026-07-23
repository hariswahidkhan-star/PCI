namespace PCI.Backend.Data;

/// <summary>
/// PCI AI Project Controls Simulation Lab — data model installer (Phase 1 foundation).
///
/// Incremental extension of the existing PCI platform (see docs/simulation-lab/PHASE_0_AUDIT.md). The Lab
/// lets students practise APPLIED project-controls work (WBS, schedule, cost, EVM, forecasting, risk,
/// change, cash flow, reporting) on synthetic, time-driven scenarios — distinct from Certuvo, which owns
/// externally-delivered MCQ exam-prep. Simulation records are their OWN tables and never touch
/// exam_attempts, entitlements or issued credentials.
///
/// Follows the MarketingSchema pattern: a self-contained, idempotent Ensure(db) using the SQLite dialect
/// that Db.cs auto-translates to MySQL. Runs on every boot on BOTH providers (so migration-integrity
/// table-set parity holds); not declared in schema.sql. Scores/percentages are REAL (→ DOUBLE); there are
/// no money columns here, so the schema.mysql.sql DECIMAL hand-fix does not apply to this module.
/// </summary>
public static class SimLabSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Seed(db);
    }

    static void Tables(Db db)
    {
        // ── Scenario / lab catalogue. One row per publishable learning artifact (guided lab, skill drill,
        //    scenario, capstone, team). Published rows are immutable for existing attempts — a revision
        //    bumps `version` (enforced in the service layer). `config_json` carries the scenario dataset. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_scenarios(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            scenario_code VARCHAR(64) UNIQUE NOT NULL,
            title TEXT NOT NULL,
            kind VARCHAR(24) NOT NULL DEFAULT 'guided_lab',   -- guided_lab | skill_drill | scenario | capstone | team
            industry VARCHAR(64),
            project_type VARCHAR(64),
            difficulty VARCHAR(16) DEFAULT 'foundation',      -- foundation | intermediate | advanced | expert
            est_minutes INTEGER DEFAULT 15,
            competencies_json TEXT,                           -- JSON array of competency keys (see §25.1)
            certification_id INTEGER,                         -- COALESCE(certification_id,1) → PCL-AI default; NULL = any
            summary TEXT,
            brief TEXT,
            config_json TEXT,                                 -- scenario dataset / task definition (synthetic data only)
            status VARCHAR(16) NOT NULL DEFAULT 'draft',      -- draft | published | suspended | archived
            version INTEGER DEFAULT 1,
            sort_order INTEGER DEFAULT 0,
            created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simscenarios_status ON simulation_scenarios(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simscenarios_kind ON simulation_scenarios(kind)");

        // ── Explicit Lab entitlement grants (admin / complimentary / sponsored / institution / marketing),
        //    with a start/expiry window. Access is ALSO granted live off an active membership or exam
        //    entitlement (Core/SimLab.Eligible) — this table only records explicit, non-purchase grants. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_entitlements(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            source VARCHAR(24) NOT NULL DEFAULT 'admin',      -- admin | complimentary | sponsored | institution | marketing
            status VARCHAR(16) NOT NULL DEFAULT 'active',     -- active | revoked
            starts_at TEXT,
            expires_at TEXT,
            granted_by INTEGER,
            note TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simentitlements_user ON simulation_entitlements(user_id)");

        // ── A student's attempt at a scenario/lab. Deterministic replay keys: scenario_version + seed +
        //    the recorded decisions reproduce the same numeric result. state_json holds the working set. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_attempts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            scenario_id INTEGER NOT NULL,
            scenario_version INTEGER DEFAULT 1,
            mode VARCHAR(16) NOT NULL DEFAULT 'training',     -- training | challenge | assessment | sandbox
            status VARCHAR(24) NOT NULL DEFAULT 'in_progress',-- in_progress | submitted | completed | passed | failed | paused | expired
            seed INTEGER DEFAULT 0,
            period INTEGER DEFAULT 0,
            score REAL,
            hints_used INTEGER DEFAULT 0,
            state_json TEXT,
            started_at TEXT DEFAULT (datetime('now')),
            submitted_at TEXT,
            completed_at TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simattempts_user ON simulation_attempts(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simattempts_scenario ON simulation_attempts(scenario_id)");

        // ── Per-attempt competency evidence (score 0-100 + mastery level). Mastery is derived from MANY
        //    pieces of evidence in the service layer, never one isolated score (§25.4). ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_competency(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            attempt_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            competency VARCHAR(48) NOT NULL,                  -- e.g. earned_value | schedule_analysis | forecasting
            score REAL,
            level VARCHAR(16),                                -- introduced | developing | competent | proficient | advanced
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simcompetency_user ON simulation_competency(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simcompetency_attempt ON simulation_competency(attempt_id)");
    }

    static void Seed(Db db)
    {
        // Operator-configurable feature flags (owner-editable in Admin → Settings; never hardcoded).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('sp_simlab_enabled','1')");
        // Access rule: membership | membership_or_exam | membership_and_enrolment | open (mirrors certuvo_requires).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('simlab_requires','membership_or_exam')");

        // Starter catalogue — a few PUBLISHED guided labs + one skill drill, seeded idempotently by
        // scenario_code (WHERE NOT EXISTS, the house seed pattern). Synthetic content only. Each carries a
        // `config_json` task the deterministic calc/grading engine (Core/SimCalc + Core/SimGrade) computes
        // and scores against — the answer key is never stored, it is derived from `given` at grade time.
        SeedScenario(db, "GL-WBS-001", "Structure a project WBS", "guided_lab", "Construction", "foundation", 15,
            "[\"scope_structuring\"]", "Roll a small project's Work Breakdown Structure up from its leaf budgets and confirm the 100% rule.",
            ConfigWbs);
        SeedScenario(db, "GL-EVM-001", "Calculate the core EVM measures", "guided_lab", "Energy", "foundation", 15,
            "[\"earned_value\"]", "Given PV, EV, AC and BAC, compute SV, CV, SPI, CPI and the EAC forecast.",
            ConfigEvm);
        SeedScenario(db, "SD-EVM-001", "EVM six-measure drill", "skill_drill", "Rail", "intermediate", 10,
            "[\"earned_value\",\"forecasting\"]", "A short, focused drill: compute six EVM/forecast measures against the clock.",
            ConfigEvmDrill);
        SeedScenario(db, "GL-SCH-001", "Identify the critical path", "guided_lab", "Infrastructure", "intermediate", 20,
            "[\"schedule_analysis\"]", "Run the forward/backward pass on a small network and identify the critical path and total float.",
            ConfigCpm);
    }

    static void SeedScenario(Db db, string code, string title, string kind, string industry, string difficulty,
        int minutes, string competenciesJson, string summary, string configJson)
    {
        db.Execute(@"INSERT INTO simulation_scenarios(scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,summary,config_json,status,version)
            SELECT ?,?,?,?,?,?,?,?,?, 'published', 1
            WHERE NOT EXISTS(SELECT 1 FROM simulation_scenarios WHERE scenario_code=?)",
            code, title, kind, industry, difficulty, minutes, competenciesJson, summary, configJson, code);
        // Backfill: a scenario seeded by an earlier build (before the task engine) has a NULL config — set
        // it once, without disturbing any operator edits (only when empty). Idempotent on both providers.
        db.Execute(@"UPDATE simulation_scenarios SET config_json=?
            WHERE scenario_code=? AND (config_json IS NULL OR config_json='')", configJson, code);
    }

    // ── Scenario task definitions (synthetic data only). `given` holds the inputs; `ask` lists the
    //    measures the student computes; the engine (SimCalc.Resolve) derives the authoritative answers. ──

    const string ConfigEvm = """
        {"task":"evm",
         "prompt":"At the end of month 3, a solar-farm package reports Planned Value (PV) 100000, Earned Value (EV) 90000 and Actual Cost (AC) 95000, against a Budget at Completion (BAC) of 200000. Compute the core earned-value measures (indices to 2 decimal places).",
         "given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
         "ask":[
           {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
           {"key":"cv","label":"Cost Variance (CV)","type":"number"},
           {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
           {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
           {"key":"eac","label":"Estimate at Completion (EAC, CPI method)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
        """;

    const string ConfigEvmDrill = """
        {"task":"evm",
         "prompt":"Rapid drill. A rail package reports PV 250000, EV 220000, AC 240000, against a BAC of 600000. Compute all six measures.",
         "given":{"pv":250000,"ev":220000,"ac":240000,"bac":600000},
         "ask":[
           {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
           {"key":"cv","label":"Cost Variance (CV)","type":"number"},
           {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
           {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
           {"key":"eac","label":"Estimate at Completion (EAC, CPI method)","type":"number"},
           {"key":"tcpi","label":"To-Complete Performance Index (TCPI to BAC)","type":"number"}],
         "tolerance":0.01,"pass_pct":80,"competencies":["earned_value","forecasting"]}
        """;

    const string ConfigCpm = """
        {"task":"cpm",
         "prompt":"Run the forward and backward pass on this small activity network (durations in days) and identify the critical path and total float.",
         "given":{"activities":[
           {"id":"A","dur":3,"preds":[]},
           {"id":"B","dur":4,"preds":["A"]},
           {"id":"C","dur":2,"preds":["A"]},
           {"id":"D","dur":5,"preds":["B","C"]},
           {"id":"E","dur":1,"preds":["D"]}]},
         "ask":[
           {"key":"project_duration","label":"Project duration (days)","type":"number"},
           {"key":"critical_path","label":"Critical path (comma-separated activity IDs)","type":"set"},
           {"key":"float_C","label":"Total float of activity C (days)","type":"number"}],
         "tolerance":0.001,"pass_pct":70,"competencies":["schedule_analysis"]}
        """;

    const string ConfigWbs = """
        {"task":"wbs",
         "prompt":"A small office fit-out has the work breakdown below with leaf-level budgets. Roll the costs up to the root and confirm the structure satisfies the 100% rule.",
         "given":{"nodes":[
           {"id":"1","parent":null,"name":"Office fit-out"},
           {"id":"1.1","parent":"1","name":"Design","value":40000},
           {"id":"1.2","parent":"1","name":"Build"},
           {"id":"1.2.1","parent":"1.2","name":"Structure","value":30000},
           {"id":"1.2.2","parent":"1.2","name":"Services","value":20000},
           {"id":"1.3","parent":"1","name":"Handover","value":10000}]},
         "ask":[
           {"key":"root_total","label":"Total project budget (root roll-up)","type":"number"},
           {"key":"hundred_percent_valid","label":"Does the WBS satisfy the 100% rule? (yes/no)","type":"bool"}],
         "pass_pct":70,"competencies":["scope_structuring"]}
        """;
}
