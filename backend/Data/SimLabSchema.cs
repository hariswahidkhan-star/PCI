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
        FreezeAttemptedVersions(db);   // must run BEFORE Seed: pin history before any seeder rewrites rows
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

        // ── Governance / quality-lifecycle columns (Phase 5A). Additive & idempotent so pre-existing
        //    installations upgrade in place (parity with Migrate.AddCol). `review_state` is the AUTHORING
        //    workflow (§13) — draft → calc_review → learning_review → safety_review → pilot → approved →
        //    published → retired — and is deliberately separate from the operational `status` that decides
        //    what is served (draft | published | suspended | archived). The remaining fields carry the §14
        //    publishable-scenario evidence: synthetic-data declaration, learning objectives, provenance,
        //    disclaimers, maker/checker reviewer attribution, approval/publish/review/expiry dates, a
        //    version change log, pilot metrics and the Training-Mode worked solution (never the graded
        //    answer key — that stays derived from `given` at grade time). ──
        void AddCol(string table, string col, string ddl)
        {
            var have = db.Columns(table);
            if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}");
        }
        AddCol("simulation_scenarios", "review_state", "review_state VARCHAR(24) NOT NULL DEFAULT 'draft'");
        AddCol("simulation_scenarios", "synthetic_declared", "synthetic_declared INTEGER DEFAULT 0");
        AddCol("simulation_scenarios", "objectives_json", "objectives_json TEXT");
        AddCol("simulation_scenarios", "provenance", "provenance TEXT");
        AddCol("simulation_scenarios", "disclaimers", "disclaimers TEXT");
        AddCol("simulation_scenarios", "worked_solution", "worked_solution TEXT");
        AddCol("simulation_scenarios", "authored_by", "authored_by INTEGER");
        AddCol("simulation_scenarios", "calc_reviewed_by", "calc_reviewed_by INTEGER");
        AddCol("simulation_scenarios", "learning_reviewed_by", "learning_reviewed_by INTEGER");
        AddCol("simulation_scenarios", "safety_reviewed_by", "safety_reviewed_by INTEGER");
        AddCol("simulation_scenarios", "approved_by", "approved_by INTEGER");
        AddCol("simulation_scenarios", "approved_at", "approved_at TEXT");
        AddCol("simulation_scenarios", "published_at", "published_at TEXT");
        AddCol("simulation_scenarios", "review_due", "review_due TEXT");
        AddCol("simulation_scenarios", "expires_at", "expires_at TEXT");
        AddCol("simulation_scenarios", "changelog_json", "changelog_json TEXT");
        AddCol("simulation_scenarios", "pilot_json", "pilot_json TEXT");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simscenarios_review ON simulation_scenarios(review_state)");

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

        // ── Append-only attempt audit / state events (start, autosave, hint, submit, coach, period).
        //    Never UPDATE/DELETE; resume and grading lease evidence read the latest relevant row. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_attempt_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            attempt_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            event_type VARCHAR(32) NOT NULL,
            period INTEGER DEFAULT 0,
            payload_json TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simevents_attempt ON simulation_attempt_events(attempt_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_simevents_user ON simulation_attempt_events(user_id)");

        // ── Immutable per-version config snapshots (Core/SimVersion — the P0 replay guarantee). Written
        //    once when a (scenario, version) pair is first served to an attempt and NEVER updated, so a
        //    Studio revision or a content-pack deploy that rewrites simulation_scenarios.config_json in
        //    place cannot change how a historical attempt replays, grades or coaches. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS simulation_scenario_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            scenario_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            config_json TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_simversions_scenario ON simulation_scenario_versions(scenario_id, version)");
    }

    /// <summary>Backfill: attempts recorded before simulation_scenario_versions existed have no snapshot,
    /// so freeze every attempted (scenario, version) pair at the live row's CURRENT config — exactly what
    /// those attempts were being served — before any seeder runs this boot. INSERT OR IGNORE makes this
    /// one-shot per pair: later boots and later content changes can no longer move historical replay.</summary>
    static void FreezeAttemptedVersions(Db db)
    {
        db.Execute(@"INSERT OR IGNORE INTO simulation_scenario_versions(scenario_id,version,config_json)
            SELECT DISTINCT a.scenario_id, COALESCE(a.scenario_version,1), s.config_json
            FROM simulation_attempts a
            JOIN simulation_scenarios s ON s.id=a.scenario_id
            WHERE s.config_json IS NOT NULL AND s.config_json<>''");
    }

    static void Seed(Db db)
    {
        // Operator-configurable feature flags (owner-editable in Admin → Settings; never hardcoded).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('sp_simlab_enabled','1')");
        // Access rule: membership | membership_or_exam | membership_and_enrolment | open (mirrors certuvo_requires).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('simlab_requires','membership_or_exam')");
        // AI Coach: on by default; degrades to a deterministic explainer when no provider key is configured.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('simlab_coach_enabled','1')");

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
        // Phase 2 — forecasting + cost control.
        SeedScenario(db, "SD-FCT-001", "Forecast the final cost (three EAC methods)", "skill_drill", "Energy", "advanced", 12,
            "[\"forecasting\",\"earned_value\"]", "Compute the Estimate at Completion by the CPI, composite (CPI×SPI) and budget-rate methods.",
            ConfigForecast);
        SeedScenario(db, "GL-CBS-001", "Roll up a cost breakdown and read the variance", "guided_lab", "Construction", "intermediate", 15,
            "[\"cost_control\"]", "Roll budget and actual cost up a cost breakdown structure and compute the variance at the root.",
            ConfigCbs);
        SeedScenario(db, "SC-EVM-001", "Read the S-curve and compute earned value", "scenario", "Technology", "intermediate", 18,
            "[\"earned_value\",\"forecasting\"]", "Interpret a five-month cost/value S-curve and compute the earned-value measures at the current period.",
            ConfigScurve);
        SeedScenario(db, "GL-PRG-001", "Measure weighted physical progress", "guided_lab", "Manufacturing", "foundation", 12,
            "[\"progress_measurement\"]", "Compute a project's overall percent-complete as the budget-weighted average of its work packages.",
            ConfigProgress);
        // Phase 3 — risk.
        SeedScenario(db, "SD-RSK-001", "Value a risk register (EMV)", "skill_drill", "Oil & Gas", "intermediate", 10,
            "[\"risk_management\"]", "Compute the Expected Monetary Value of a quantified risk register (threats and opportunities net off).",
            ConfigRisk);
        SeedScenario(db, "GL-PRT-001", "Three-point estimate and confidence (PERT)", "guided_lab", "Aerospace", "advanced", 20,
            "[\"schedule_analysis\",\"risk_management\"]", "Run a PERT analysis on a critical path: expected duration, standard deviation, and the probability of meeting a deadline.",
            ConfigPert);
        SeedScenario(db, "SC-MC-001", "PERT vs Monte-Carlo: read the distribution", "scenario", "Pharmaceutical", "advanced", 22,
            "[\"risk_management\",\"schedule_analysis\"]", "Compute the PERT normal-approximation for a chain, then compare it with a Monte-Carlo simulation of the finish date.",
            ConfigMonteCarlo);
        // Phase 3 — change control + cash flow.
        SeedScenario(db, "GL-CHG-001", "Apply the change register to the baseline", "guided_lab", "Defence", "intermediate", 14,
            "[\"change_control\"]", "Roll only the APPROVED changes onto the baseline to get the revised budget and duration.",
            ConfigChange);
        SeedScenario(db, "GL-CASH-001", "Project cash flow and peak funding", "guided_lab", "Utilities", "intermediate", 16,
            "[\"cash_flow\"]", "Roll a time-phased cash flow into its cumulative position and find the peak funding requirement.",
            ConfigCash);
        // Phase 3 — time-driven simulation: step a project period by period and read its trajectory.
        SeedScenario(db, "SC-EVT-001", "Track a project month by month (EVM trend)", "scenario", "Infrastructure", "advanced", 20,
            "[\"earned_value\",\"forecasting\"]", "Step a six-month project period by period: find where performance was worst and forecast the final cost from the cumulative CPI.",
            ConfigTimeline);
        // Phase 3 — earned schedule: schedule performance read in the time dimension, not cost.
        SeedScenario(db, "SD-ESC-001", "Measure schedule performance in time (Earned Schedule)", "scenario", "Software", "advanced", 18,
            "[\"schedule_analysis\",\"forecasting\"]", "Read Earned Schedule off the planned-value curve: schedule variance and index in time units, and an independent time forecast.",
            ConfigEarnedSchedule);

        // Full house content pack. The original starter seeds above remain for backward compatibility;
        // the pack then densifies house-authored rows and adds the rest of the catalogue without touching
        // operator-authored scenarios (authored_by IS NOT NULL).
        SimLabContentPack.Seed(db);
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
        // House content is synthetic and shipped published — mark it governed so it satisfies the §14
        // validator (SimContent.Validate). Only touches rows still at the authoring default, so an operator
        // who moves a scenario back into review keeps their state. Idempotent on both providers.
        db.Execute(@"UPDATE simulation_scenarios
            SET review_state='published', synthetic_declared=1,
                published_at=COALESCE(published_at, datetime('now'))
            WHERE scenario_code=? AND (review_state IS NULL OR review_state='draft')", code);
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
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"],
         "variant":{"vary":{
           "pv":{"min":80000,"max":120000,"step":1000},
           "ev":{"min":70000,"max":110000,"step":1000},
           "ac":{"min":80000,"max":115000,"step":1000}}}}
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

    const string ConfigChange = """
        {"task":"change",
         "prompt":"A project baseline is BAC 500000 over 100 days. The change register below lists four changes with cost and schedule deltas. Roll ONLY the approved changes onto the baseline: give the revised BAC, the revised duration, and how many changes were approved.",
         "given":{"baseline_bac":500000,"baseline_duration":100,"changes":[
           {"id":"C1","title":"Extra test rig","status":"approved","cost_delta":30000,"schedule_delta":5},
           {"id":"C2","title":"Gold-plating request","status":"rejected","cost_delta":50000,"schedule_delta":10},
           {"id":"C3","title":"Descope legacy module","status":"approved","cost_delta":-10000,"schedule_delta":-2},
           {"id":"C4","title":"Client-requested finish","status":"pending","cost_delta":20000,"schedule_delta":3}]},
         "ask":[
           {"key":"revised_bac","label":"Revised BAC","type":"number"},
           {"key":"revised_duration","label":"Revised duration (days)","type":"number"},
           {"key":"approved_count","label":"Number of changes approved","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["change_control"]}
        """;

    const string ConfigCash = """
        {"task":"cashflow",
         "prompt":"A project's monthly cash inflows and outflows are below. Roll them into the cumulative net position: give the final (closing) position and the peak funding requirement (the deepest cumulative deficit, as a positive amount).",
         "given":{"periods":[
           {"period":1,"inflow":0,"outflow":50000},
           {"period":2,"inflow":20000,"outflow":60000},
           {"period":3,"inflow":80000,"outflow":40000},
           {"period":4,"inflow":120000,"outflow":30000}]},
         "ask":[
           {"key":"final_position","label":"Final (closing) cash position","type":"number"},
           {"key":"peak_funding","label":"Peak funding requirement","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["cash_flow"]}
        """;

    const string ConfigTimeline = """
        {"task":"timeline",
         "prompt":"A six-month infrastructure package reports cumulative Planned Value, Earned Value and Actual Cost each month against a 600000 BAC. Read the trajectory rather than a single snapshot: in which month was schedule performance (SPI) at its worst, what is the cumulative CPI at the finish, and — using the CPI method (EAC = BAC / CPI) — what is the forecast final cost and the resulting Variance at Completion (VAC = BAC − EAC)?",
         "given":{"bac":600000,
           "series":[
             {"period":1,"pv":100000,"ev":90000,"ac":100000},
             {"period":2,"pv":220000,"ev":200000,"ac":230000},
             {"period":3,"pv":350000,"ev":300000,"ac":360000},
             {"period":4,"pv":470000,"ev":420000,"ac":500000},
             {"period":5,"pv":560000,"ev":520000,"ac":610000},
             {"period":6,"pv":600000,"ev":580000,"ac":680000}]},
         "ask":[
           {"key":"worst_spi_period","label":"Month of worst schedule performance (lowest SPI)","type":"number"},
           {"key":"final_cpi","label":"Cumulative CPI at completion","type":"number"},
           {"key":"final_eac","label":"Estimate at Completion (EAC = BAC / CPI)","type":"number"},
           {"key":"vac","label":"Variance at Completion (VAC = BAC - EAC)","type":"number"}],
         "tolerance":0.01,"pass_pct":75,"competencies":["earned_value","forecasting"]}
        """;

    const string ConfigEarnedSchedule = """
        {"task":"earned_schedule",
         "prompt":"A six-month release plan has the cumulative Planned Value below (BAC 1000, planned duration 6 months). At the end of month 4 the team has earned EV 500. Classic SPI is heading to 1.0 as the project ends, so use Earned Schedule instead: read ES off the plan (the time the plan meant to have earned 500), then give the schedule variance in time (SV(t) = ES - AT), the time-based index (SPI(t) = ES / AT), and the independent time forecast (IEAC(t) = PD / SPI(t)).",
         "given":{"planned_duration":6,"at":4,"ev":500,
           "plan":[
             {"period":1,"pv":100},
             {"period":2,"pv":250},
             {"period":3,"pv":450},
             {"period":4,"pv":650},
             {"period":5,"pv":830},
             {"period":6,"pv":1000}]},
         "ask":[
           {"key":"es","label":"Earned Schedule (months)","type":"number"},
           {"key":"sv_time","label":"Schedule variance in time (SV(t) = ES - AT)","type":"number"},
           {"key":"spi_time","label":"Time-based schedule index (SPI(t) = ES / AT)","type":"number"},
           {"key":"eac_time","label":"Independent time forecast (IEAC(t) = PD / SPI(t))","type":"number"}],
         "tolerance":0.01,"pass_pct":75,"competencies":["schedule_analysis","forecasting"]}
        """;

    const string ConfigMonteCarlo = """
        {"task":"pert",
         "prompt":"A three-activity chain has optimistic/most-likely/pessimistic estimates. Compute the PERT expected duration and the probability of finishing by day 22 (the normal approximation), then compare your answer with the Monte-Carlo distribution of the finish date shown below.",
         "given":{"activities":[
           {"id":"A","o":4,"m":6,"p":8,"preds":[]},
           {"id":"B","o":5,"m":8,"p":17,"preds":["A"]},
           {"id":"C","o":2,"m":3,"p":10,"preds":["B"]}],
           "deadline":22,"seed":424242,"iterations":4000},
         "ask":[
           {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
           {"key":"prob_on_time","label":"Probability of finishing by day 22 (%)","type":"number"}],
         "tolerance":0.02,"pass_pct":70,"competencies":["risk_management","schedule_analysis"]}
        """;

    const string ConfigRisk = """
        {"task":"risk",
         "prompt":"A project's risk register lists three quantified risks. Negative impacts are threats; positive impacts are opportunities. Compute the register's Expected Monetary Value (threats and opportunities net off).",
         "given":{"risks":[
           {"id":"R1","name":"Supplier delay","probability":0.3,"impact":-20000},
           {"id":"R2","name":"Scope creep","probability":0.5,"impact":-10000},
           {"id":"R3","name":"Early-handover bonus","probability":0.2,"impact":15000}]},
         "ask":[{"key":"emv","label":"Expected Monetary Value of the register","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["risk_management"]}
        """;

    const string ConfigPert = """
        {"task":"pert",
         "prompt":"Three activities on the critical path have optimistic (O), most-likely (M) and pessimistic (P) estimates in days. Compute the path's PERT expected duration, its standard deviation, and the probability of finishing by day 14 (%).",
         "given":{"activities":[
           {"id":"A","o":2,"m":4,"p":6},
           {"id":"B","o":3,"m":5,"p":13},
           {"id":"C","o":1,"m":2,"p":3}],"deadline":14},
         "ask":[
           {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
           {"key":"std_dev","label":"Path standard deviation (days)","type":"number"},
           {"key":"prob_on_time","label":"Probability of finishing by day 14 (%)","type":"number"}],
         "tolerance":0.02,"pass_pct":70,"competencies":["schedule_analysis","risk_management"]}
        """;

    const string ConfigProgress = """
        {"task":"progress",
         "prompt":"A production line has three work packages, each with a budget weight and its own percent-complete. Compute the project's overall physical progress as the budget-weighted average.",
         "given":{"nodes":[
           {"id":"1.1","name":"Tooling","weight":40000,"percent":100},
           {"id":"1.2","name":"Assembly","weight":30000,"percent":50},
           {"id":"1.3","name":"Commissioning","weight":30000,"percent":0}]},
         "ask":[
           {"key":"overall_percent","label":"Overall percent-complete (budget-weighted)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["progress_measurement"]}
        """;

    const string ConfigScurve = """
        {"task":"evm",
         "prompt":"A data-centre fit-out has run for five months. The S-curve shows cumulative Planned Value, Earned Value and Actual Cost. Using the month-5 figures (PV 500000, EV 430000, AC 460000, BAC 900000), compute the earned-value measures.",
         "given":{"pv":500000,"ev":430000,"ac":460000,"bac":900000,
           "series":[
             {"period":1,"pv":100000,"ev":90000,"ac":95000},
             {"period":2,"pv":220000,"ev":195000,"ac":205000},
             {"period":3,"pv":340000,"ev":300000,"ac":320000},
             {"period":4,"pv":430000,"ev":370000,"ac":395000},
             {"period":5,"pv":500000,"ev":430000,"ac":460000}]},
         "ask":[
           {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
           {"key":"cv","label":"Cost Variance (CV)","type":"number"},
           {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
           {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
           {"key":"eac","label":"Estimate at Completion (EAC, CPI method)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value","forecasting"]}
        """;

    const string ConfigForecast = """
        {"task":"evm",
         "prompt":"A wind-farm package reports PV 200000, EV 180000 and AC 200000 against a BAC of 400000. Forecast the final cost three ways: the CPI method, the composite (CPI×SPI) method, and the budget-rate method.",
         "given":{"pv":200000,"ev":180000,"ac":200000,"bac":400000},
         "ask":[
           {"key":"eac_cpi","label":"EAC — CPI method (BAC ÷ CPI)","type":"number"},
           {"key":"eac_composite","label":"EAC — composite (AC + (BAC−EV) ÷ (CPI×SPI))","type":"number"},
           {"key":"eac_budget","label":"EAC — budget rate (AC + (BAC−EV))","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["forecasting","earned_value"]}
        """;

    const string ConfigCbs = """
        {"task":"cbs",
         "prompt":"A refurbishment project has three cost accounts with budgeted and actual costs. Roll them up and report the total budget, the total actual, and the variance (budget − actual) at the root.",
         "given":{"nodes":[
           {"id":"1","parent":null,"name":"Refurbishment"},
           {"id":"1.1","parent":"1","name":"Demolition","budget":50000,"actual":55000},
           {"id":"1.2","parent":"1","name":"Structure","budget":30000,"actual":25000},
           {"id":"1.3","parent":"1","name":"Finishes","budget":20000,"actual":22000}]},
         "ask":[
           {"key":"root_budget","label":"Total budget (root roll-up)","type":"number"},
           {"key":"root_actual","label":"Total actual (root roll-up)","type":"number"},
           {"key":"root_variance","label":"Variance at the root (budget − actual)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["cost_control"]}
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
