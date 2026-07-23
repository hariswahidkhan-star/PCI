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
        // scenario_code (WHERE NOT EXISTS, the house seed pattern). Synthetic content only. `config_json`
        // task payloads are filled out in later Phase-1 bricks (the calc/grading engine); these rows make
        // the catalogue non-empty so /api/me/lab/catalogue and the student page have real data.
        SeedScenario(db, "GL-WBS-001", "Structure a project WBS", "guided_lab", "Construction", "foundation", 15,
            "[\"scope_structuring\"]", "Build a compliant, fully-decomposed Work Breakdown Structure for a small project and validate the roll-up.");
        SeedScenario(db, "GL-EVM-001", "Calculate the core EVM measures", "guided_lab", "Energy", "foundation", 15,
            "[\"earned_value\"]", "Given PV, EV and AC, compute SV, CV, SPI, CPI and interpret project health.");
        SeedScenario(db, "SD-EVM-001", "EVM six-measure drill", "skill_drill", "Rail", "intermediate", 10,
            "[\"earned_value\",\"forecasting\"]", "A short, focused drill: compute six EVM/forecast measures against the clock.");
        SeedScenario(db, "GL-SCH-001", "Identify the critical path", "guided_lab", "Infrastructure", "intermediate", 20,
            "[\"schedule_analysis\"]", "Run the forward/backward pass on a small network and identify the critical path and total float.");
    }

    static void SeedScenario(Db db, string code, string title, string kind, string industry, string difficulty,
        int minutes, string competenciesJson, string summary)
    {
        db.Execute(@"INSERT INTO simulation_scenarios(scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,summary,status,version)
            SELECT ?,?,?,?,?,?,?,?, 'published', 1
            WHERE NOT EXISTS(SELECT 1 FROM simulation_scenarios WHERE scenario_code=?)",
            code, title, kind, industry, difficulty, minutes, competenciesJson, summary, code);
    }
}
