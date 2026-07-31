using PCI.Backend.Core;

namespace PCI.Backend.Data;

/// <summary>
/// PCI World — pilot challenge pack (Phase 1 slice: 10 reviewed challenges; docs/pciworld/PLAN.md).
///
/// All data is synthetic. Every challenge passes WorldContent.Validate and a full reference solve
/// in CI (WorldContentTests) before it can ship. Seeding follows the replay-immutability
/// discipline from day one: house rows (author_id NULL, working copy untouched) upsert with a NEW
/// immutable version when — and only when — config_json changes, so a deploy can never rewrite
/// what a historical attempt was served. Operator-owned or in-revision rows are never touched.
/// </summary>
public static class WorldContentPack
{
    sealed record Pilot(string Code, string Title, string Hook, string Industry, string Role,
        string Track, string Difficulty, int Minutes, string CompetenciesJson, string ConfigJson);

    public static int Count => Pilots.Length;

    public static void Seed(Db db)
    {
        foreach (var p in Pilots) Upsert(db, p);
    }

    /// <summary>House-content upsert for sibling packs (WorldIntelligencePack): same replay-immutability
    /// discipline — a config change is a NEW immutable version, operator rows are never touched.</summary>
    internal static void UpsertHouse(Db db, string code, string title, string hook, string industry, string role,
        string track, string difficulty, int minutes, string competenciesJson, string configJson) =>
        Upsert(db, new Pilot(code, title, hook, industry, role, track, difficulty, minutes, competenciesJson, configJson));

    static void Upsert(Db db, Pilot p)
    {
        var existing = db.QueryOne("SELECT id,author_id,status,current_version,config_json FROM pciworld_challenges WHERE code=?", p.Code);
        if (existing is null)
        {
            // Check-then-act, raced by two boots against one database (CCP-P2-008) — the loser died
            // on `UNIQUE constraint failed: pciworld_challenges.code`. Same shape and same reasoning
            // as WorldArticlePack.Upsert: `OR IGNORE` makes the losing write a no-op, and the id is
            // re-read rather than taken from the insert, because last_insert_rowid() after an
            // IGNORED insert is whatever that connection last inserted and not this row.
            //
            // It matters more here than for an article. A version snapshot is the SOLE replay
            // authority for every attempt against a challenge, so a version row keyed off a
            // borrowed id would be an attempt replaying somebody else's problem. The unique index
            // on (challenge_id, version) happens to absorb it today; that is not a reason to write
            // the wrong id.
            var (inserted, created) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO pciworld_challenges
                    (code,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,synthetic_declared,
                     config_json,status,current_version,published_at)
                VALUES(?,?,?,?,?,?,?,?,?,1,?, 'published', 1, datetime('now'))",
                p.Code, p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.ConfigJson);

            var id = created > 0
                ? inserted
                : H.L(db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", p.Code)?["id"]);
            if (id <= 0) return;

            db.Execute(@"INSERT OR IGNORE INTO pciworld_challenge_versions
                    (challenge_id,version,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,config_json)
                VALUES(?,1,?,?,?,?,?,?,?,?,?)",
                id, p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.ConfigJson);
            return;
        }
        // Never touch operator-authored rows or a working copy that is mid-revision.
        if (existing["author_id"] is not null || H.Str(existing["status"]) != "published") return;
        var changed = (Convert.ToString(existing["config_json"]) ?? "") != p.ConfigJson;
        if (!changed)
        {
            db.Execute(@"UPDATE pciworld_challenges SET title=?, hook=?, industry=?, role=?, track=?, difficulty=?,
                    est_minutes=?, competencies_json=?, updated_at=datetime('now') WHERE code=? AND author_id IS NULL",
                p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.Code);
            return;
        }
        // A config change is a NEW immutable version — never an in-place rewrite of a served one.
        var id2 = H.L(existing["id"]);
        var next = H.L(existing["current_version"]) + 1;
        db.Execute(@"UPDATE pciworld_challenges SET title=?, hook=?, industry=?, role=?, track=?, difficulty=?,
                est_minutes=?, competencies_json=?, config_json=?, current_version=?, updated_at=datetime('now')
            WHERE id=? AND author_id IS NULL AND status='published'",
            p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.ConfigJson, next, id2);
        db.Execute(@"INSERT INTO pciworld_challenge_versions
                (challenge_id,version,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,config_json)
            VALUES(?,?,?,?,?,?,?,?,?,?,?)",
            id2, next, p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.ConfigJson);
    }

    static readonly Pilot[] Pilots =
    {
        new("WC-EVM-001", "The metro package is bleeding money — or is it?",
            "Month 4 on a metro station fit-out. The numbers look bad. How bad, exactly?",
            "Rail", "Assistant Project Controller", "project_controls", "foundation", 8,
            """["earned_value","stakeholder_communication"]""",
            """
            {"context":"You are the assistant project controller on a metro station fit-out package. At the month-4 data date the package reports Planned Value 4200000, Earned Value 3780000 and Actual Cost 4100000 against a Budget at Completion of 12600000. The project manager wants one honest paragraph for the steering pack tonight.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"4,200,000"},
               {"label":"Earned Value (EV)","value":"3,780,000"},
               {"label":"Actual Cost (AC)","value":"4,100,000"},
               {"label":"Budget at Completion (BAC)","value":"12,600,000"},
               {"label":"Data date","value":"End of month 4 of 12"}],
             "task":"evm","given":{"pv":4200000,"ev":3780000,"ac":4100000,"bac":12600000},
             "ask":[
               {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
               {"key":"cv","label":"Cost Variance (CV)","type":"number"},
               {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"brief","prompt":"What goes in tonight's steering pack?",
                "options":[
                  {"key":"a","label":"Report a cost overrun and request additional contingency immediately","quality":40,
                   "consequence":"The board grants contingency but asks why no recovery options were tabled — credibility dips.",
                   "principle":"Escalation without analysis transfers the problem, not the insight."},
                  {"key":"b","label":"Report the measured indices with a quantified recovery watchlist for the two worst work packages","quality":100,
                   "consequence":"The board accepts the position and endorses the watchlist — you own the narrative.",
                   "principle":"Report performance as measured, paired with the actions the numbers justify."},
                  {"key":"c","label":"Hold the figures one more month until the trend is clearer","quality":10,
                   "consequence":"The variance surfaces anyway through finance — a month late and no longer yours to frame.",
                   "principle":"Withheld bad news compounds; controlled bad news builds trust."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Diagnosed a metro fit-out's true cost and schedule position from raw EVM data.",
             "hints":[
               "Earned value is the anchor: both variances start from what the work actually earned, not from what was spent or planned.",
               "Schedule measures compare earned against planned; cost measures compare earned against actual — subtraction gives the variance, division gives the index.",
               "Negative variances and indices below one point the same way; compute all four from the same three cumulative figures and let the paragraph follow the arithmetic."]}
            """),

        new("WC-SCH-002", "Find the path that actually matters",
            "Six activities, one deadline, and only one sequence that decides it.",
            "Data Centres", "Planning Engineer", "project_controls", "developing", 10,
            """["schedule_analysis"]""",
            """
            {"context":"A data-centre commissioning sequence has six activities. The operations director believes activity D — the switchgear tests — is driving the end date and wants to crash it. Before any money moves, run the network.",
             "evidence":[
               {"label":"A — Enabling works","value":"3 days, no predecessors"},
               {"label":"B — Power train install","value":"5 days, after A"},
               {"label":"C — Controls install","value":"4 days, after A"},
               {"label":"D — Switchgear tests","value":"2 days, after B"},
               {"label":"E — Integration tests","value":"6 days, after C"},
               {"label":"F — Handover","value":"3 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},{"id":"B","dur":5,"preds":["A"]},{"id":"C","dur":4,"preds":["A"]},
               {"id":"D","dur":2,"preds":["B"]},{"id":"E","dur":6,"preds":["C"]},{"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Project duration (days)","type":"number"},
               {"key":"critical_path","label":"Critical path (comma-separated activity IDs)","type":"set"},
               {"key":"float_D","label":"Total float of activity D (days)","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"crash","prompt":"The director offers budget to accelerate one activity. Where does it do the most good?",
                "options":[
                  {"key":"d","label":"Crash D — the switchgear tests the director is worried about","quality":15,
                   "consequence":"D had float; the end date does not move and the budget is spent.",
                   "principle":"Acceleration off the critical path buys nothing."},
                  {"key":"e","label":"Crash E — integration tests on the critical path","quality":100,
                   "consequence":"Each day saved on E moves handover a day earlier, up to the point the paths converge.",
                   "principle":"Compression only works on the critical path, and only until float elsewhere is consumed."},
                  {"key":"none","label":"Decline the budget — the schedule cannot be improved","quality":25,
                   "consequence":"A real acceleration opportunity on E is missed.",
                   "principle":"The network, not instinct, says whether acceleration is possible."}]}],
             "profile_map":{"calculation":"Schedule Detective","decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ran the critical path on a data-centre commissioning network and put acceleration money where it works.",
             "hints":[
               "Work the network forward for earliest dates, then backward for latest — the duration is fixed by the longest chain of dependent activities.",
               "An activity's total float is the gap between its latest and earliest start; the critical path is the chain where that gap is zero.",
               "Before agreeing to crash anything, check whether the activity in question carries float — money spent accelerating a non-critical activity buys no finish date at all."]}
            """),

        new("WC-RSK-003", "Price the storm before it hits",
            "Five register lines. One honest number for the contingency debate.",
            "Renewable Energy", "Risk Analyst", "cross_functional", "professional", 10,
            """["risk_management","cost_control"]""",
            """
            {"context":"An offshore wind marshalling-harbour upgrade is heading to investment committee. The register below is quantified; the committee wants the register's Expected Monetary Value and the largest single threat exposure before it will discuss the contingency line.",
             "evidence":[
               {"label":"R1 — Vessel weather downtime","value":"probability 0.35, impact -800,000"},
               {"label":"R2 — Quay wall settlement","value":"probability 0.20, impact -450,000"},
               {"label":"R3 — Grid connection slip","value":"probability 0.15, impact -1,200,000"},
               {"label":"R4 — Early turbine delivery bonus","value":"probability 0.40, impact +300,000"},
               {"label":"R5 — Regulatory redesign","value":"probability 0.10, impact -2,000,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.35,"impact":-800000},{"id":"R2","probability":0.2,"impact":-450000},
               {"id":"R3","probability":0.15,"impact":-1200000},{"id":"R4","probability":0.4,"impact":300000},
               {"id":"R5","probability":0.1,"impact":-2000000}]},
             "ask":[
               {"key":"emv","label":"Register EMV (net)","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — vessel weather downtime","type":"number"},
               {"key":"emv_R5","label":"EMV of R5 — regulatory redesign","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"contingency","prompt":"How do you present the contingency recommendation?",
                "options":[
                  {"key":"emv","label":"Recommend contingency equal to the net EMV, with the register attached","quality":80,
                   "consequence":"Defensible and transparent — though the committee notes EMV underfunds low-probability, high-impact tails.",
                   "principle":"EMV is the honest floor for contingency, not the ceiling."},
                  {"key":"tail","label":"Recommend EMV plus a stated allowance for the R5 tail, both shown separately","quality":100,
                   "consequence":"The committee funds both lines because each is separately justified.",
                   "principle":"Expected value and tail protection are different questions — price them separately."},
                  {"key":"gut","label":"Recommend a round 10% of budget — committees prefer simple numbers","quality":15,
                   "consequence":"The number survives one meeting, then fails its first audit question.",
                   "principle":"A contingency that cannot be traced to the register cannot be defended."}]}],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced an offshore wind risk register and split expected value from tail protection.",
             "hints":[
               "Each register line's expected value is probability multiplied by impact, keeping the sign — threats negative, opportunities positive.",
               "The register value nets every line's expected value, threats against opportunities.",
               "The largest single exposure is read line by line before netting; a healthy net figure can hide one line the committee should see on its own."]}
            """),

        new("WC-CSH-004", "The project that ran out of cash while profitable",
            "Margin on paper, empty account in month two. Find the funding gap.",
            "Construction", "Project Finance Analyst", "project_finance", "professional", 10,
            """["cash_flow","forecasting"]""",
            """
            {"context":"A design-and-build school project is profitable at completion, but the contractor is paid in arrears while paying suppliers monthly. The finance director wants the cumulative position and the peak funding requirement before signing the facility agreement.",
             "evidence":[
               {"label":"Month 1","value":"inflow 0, outflow 350,000"},
               {"label":"Month 2","value":"inflow 150,000, outflow 400,000"},
               {"label":"Month 3","value":"inflow 500,000, outflow 300,000"},
               {"label":"Month 4","value":"inflow 700,000, outflow 250,000"},
               {"label":"Month 5","value":"inflow 400,000, outflow 150,000"}],
             "task":"cashflow",
             "given":{"periods":[
               {"period":1,"inflow":0,"outflow":350000},{"period":2,"inflow":150000,"outflow":400000},
               {"period":3,"inflow":500000,"outflow":300000},{"period":4,"inflow":700000,"outflow":250000},
               {"period":5,"inflow":400000,"outflow":150000}]},
             "ask":[
               {"key":"final_position","label":"Final (closing) cash position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"},
               {"key":"cumulative_2","label":"Cumulative position at end of month 2","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"facility","prompt":"What do you tell the finance director about the facility?",
                "options":[
                  {"key":"peak","label":"Size the facility on the peak cumulative deficit plus headroom, and show the month it bites","quality":100,
                   "consequence":"The facility clears the trough with margin; no emergency drawdown later.",
                   "principle":"Fund the trough, not the average — projects fail at the deepest month."},
                  {"key":"avg","label":"Size it on the average monthly net outflow — cheaper commitment fees","quality":20,
                   "consequence":"Month 2 breaches the facility; an emergency extension costs more than the fee saved.",
                   "principle":"Averages hide the exact month that kills you."},
                  {"key":"none","label":"No facility — the project is profitable overall","quality":0,
                   "consequence":"Payroll misses in month 2. Profitability at completion never got a chance.",
                   "principle":"Profit is an opinion; cash is a fact with a date on it."}]}],
             "profile_map":{"calculation":"Finance Thinker","decision":"Finance Thinker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sized the real funding gap on a profitable project from its month-by-month cash curve.",
             "hints":[
               "Run the months in order: each period's net movement is inflow minus outflow, and the cumulative line carries the running total.",
               "The peak funding requirement is the deepest point the cumulative line reaches below zero — not the largest single monthly outflow.",
               "Profit at completion and cash through the middle are different questions; the facility is sized by the worst cumulative point, and the closing position confirms the profit."]}
            """),

        new("WC-CHG-005", "The scope wants to grow. The budget doesn't.",
            "Four changes on the register. Only the approved ones move the baseline.",
            "Healthcare Capital Projects", "Project Manager", "project_management", "developing", 8,
            """["change_control","governance"]""",
            """
            {"context":"A hospital imaging-wing project holds a baseline of 8000000 over 220 days. The change register lists four items in different states. The board pack must show the revised baseline — built from approved changes only — and you must decide what happens to the pending item.",
             "evidence":[
               {"label":"Baseline","value":"BAC 8,000,000 · 220 days"},
               {"label":"C1 — Shielding upgrade","value":"APPROVED · cost +450,000 · schedule +15 days"},
               {"label":"C2 — Second MRI suite","value":"REJECTED · cost +900,000 · schedule +30 days"},
               {"label":"C3 — Descope interim ward","value":"APPROVED · cost -120,000 · schedule -5 days"},
               {"label":"C4 — Revised air handling","value":"PENDING · cost +200,000 · schedule +10 days"}],
             "task":"change",
             "given":{"baseline_bac":8000000,"baseline_duration":220,"changes":[
               {"id":"C1","status":"approved","cost_delta":450000,"schedule_delta":15},
               {"id":"C2","status":"rejected","cost_delta":900000,"schedule_delta":30},
               {"id":"C3","status":"approved","cost_delta":-120000,"schedule_delta":-5},
               {"id":"C4","status":"pending","cost_delta":200000,"schedule_delta":10}]},
             "ask":[
               {"key":"revised_bac","label":"Revised BAC (approved changes only)","type":"number"},
               {"key":"revised_duration","label":"Revised duration in days","type":"number"},
               {"key":"approved_count","label":"Number of approved changes","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"pending","prompt":"The clinical lead asks you to start the C4 air-handling work now — approval is 'a formality'.",
                "options":[
                  {"key":"wait","label":"Hold the work; expedite the C4 decision through the change board this week","quality":100,
                   "consequence":"C4 is approved three days later with funding attached; no unauthorised commitment existed at any point.",
                   "principle":"The baseline only moves through the gate — expedite the gate, never bypass it."},
                  {"key":"start","label":"Start the work; paperwork can catch up","quality":0,
                   "consequence":"C4 is approved at a reduced amount; the difference becomes an unfunded commitment on your name.",
                   "principle":"Work done before approval is a donation until proven otherwise."},
                  {"key":"absorb","label":"Do the work quietly inside another work package's budget","quality":5,
                   "consequence":"The audit trail eventually surfaces it as misallocation — worse than the original problem.",
                   "principle":"Hiding scope in other budgets converts a cost issue into an integrity issue."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Strategic Project Controller","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rebuilt a hospital project baseline from its change register — approved changes only.",
             "hints":[
               "Only changes in an approved state move the baseline; pending, rejected and withdrawn items stay out of it entirely.",
               "Apply each approved item's cost delta to the baseline budget and its time delta to the baseline duration — nothing else in the register touches either.",
               "Count the approved items as you apply them; if the count disagrees with the deltas you applied, one register line has been mis-read."]}
            """),

        new("WC-PRG-006", "Percent complete, or percent hoped?",
            "Four work packages, four opinions of progress. Weight them and find the truth.",
            "Manufacturing", "Project Controls Analyst", "project_controls", "foundation", 7,
            """["progress_measurement"]""",
            """
            {"context":"A production-line installation reports progress by work package. The site manager quotes 'about 70% done' from walking the floor. Compute the budget-weighted physical progress and see whether the floor agrees with the money.",
             "evidence":[
               {"label":"Piling & foundations","value":"weight 500,000 · 100% complete"},
               {"label":"Structural steel","value":"weight 750,000 · 60% complete"},
               {"label":"MEP installation","value":"weight 600,000 · 20% complete"},
               {"label":"Commissioning","value":"weight 150,000 · 0% complete"}],
             "task":"progress",
             "given":{"nodes":[
               {"id":"1.1","name":"Piling & foundations","weight":500000,"percent":100},
               {"id":"1.2","name":"Structural steel","weight":750000,"percent":60},
               {"id":"1.3","name":"MEP installation","weight":600000,"percent":20},
               {"id":"1.4","name":"Commissioning","weight":150000,"percent":0}]},
             "ask":[
               {"key":"overall_percent","label":"Overall percent complete (budget-weighted)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"report","prompt":"The site manager wants '70%' in the monthly report. Your weighted figure is lower.",
                "options":[
                  {"key":"measured","label":"Report the weighted figure with the work-package table attached","quality":100,
                   "consequence":"The gap between floor-feel and measured progress becomes this month's most useful conversation.",
                   "principle":"Progress is what the measurement system says, weighted by what the work is worth."},
                  {"key":"split","label":"Report a range — 'between the two views'","quality":30,
                   "consequence":"Both numbers lose authority; next month everyone negotiates progress.",
                   "principle":"A control system that averages opinions stops being a control system."},
                  {"key":"defer","label":"Use 70% this month and true-up later","quality":5,
                   "consequence":"The true-up lands in the same month as the first schedule slip — twice the bad news, half the trust.",
                   "principle":"Optimistic progress borrows credibility at compound interest."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Measured real weighted progress on a production line — and defended it.",
             "hints":[
               "Weight each package's percent complete by its budget share — walking the floor counts big and small packages equally, the money does not.",
               "Multiply each package's completion by its budget, sum the results, and divide by the total budget.",
               "If the weighted figure disagrees with the walked impression, the difference usually sits in the heaviest package — check the biggest budget lines first."]}
            """),

        new("WC-ESC-007", "The schedule says fine. Time says otherwise.",
            "SPI is drifting back to 1.0 as the deadline slips. Read the schedule in time units.",
            "Software", "Programme Controls Lead", "project_controls", "advanced", 12,
            """["schedule_analysis","forecasting"]""",
            """
            {"context":"A platform-migration programme is at the end of month 5 of a planned 8. Cumulative Earned Value is 340 against the plan below (BAC 900). Cost-based SPI is creeping back toward 1.0 — as it always does near the end — so the steering group thinks recovery is happening. Read the schedule in TIME: earned schedule, time-based variance and the time-based index.",
             "evidence":[
               {"label":"Planned value curve","value":"M1 60 · M2 140 · M3 240 · M4 360 · M5 480 · M6 620 · M7 760 · M8 900"},
               {"label":"Earned value to date","value":"340 at end of month 5"},
               {"label":"Planned duration","value":"8 months"}],
             "task":"earned_schedule",
             "given":{"planned_duration":8,"at":5,"ev":340,
               "plan":[{"period":1,"pv":60},{"period":2,"pv":140},{"period":3,"pv":240},{"period":4,"pv":360},
                       {"period":5,"pv":480},{"period":6,"pv":620},{"period":7,"pv":760},{"period":8,"pv":900}]},
             "ask":[
               {"key":"es","label":"Earned Schedule (months)","type":"number"},
               {"key":"sv_time","label":"Schedule variance in time — SV(t)","type":"number"},
               {"key":"spi_time","label":"Time-based index — SPI(t)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"steer","prompt":"How do you brief the steering group?",
                "options":[
                  {"key":"time","label":"Present SV(t)/SPI(t) and an independent time forecast; recommend re-planning the last three months","quality":100,
                   "consequence":"The group sees the real slip for the first time and funds the re-plan while options still exist.",
                   "principle":"Near completion, only time-based measures tell the schedule truth."},
                  {"key":"cost","label":"Present the improving cost-based SPI — the trend is technically real","quality":10,
                   "consequence":"The group relaxes; the slip surfaces at month 7 when no options remain.",
                   "principle":"Cost-based SPI converging on 1.0 at the end is arithmetic, not recovery."},
                  {"key":"both","label":"Show both, let the group choose the story it prefers","quality":35,
                   "consequence":"The group prefers the comfortable number. So did the last programme that missed by a quarter.",
                   "principle":"Presenting contradictory measures without a recommendation is delegation of judgement, not transparency."}]}],
             "profile_map":{"calculation":"Schedule Detective","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Exposed a hidden schedule slip with Earned Schedule after cost-based SPI said 'fine'.",
             "hints":[
               "Earned schedule asks when the plan expected today's earned value — project the earned figure back onto the planned curve and read the time.",
               "Find the period whose cumulative plan brackets the earned value, then interpolate within it for the fraction.",
               "Time variance is earned schedule minus time elapsed, and the time index divides the two — near the end of a plan, only the time-based pair keeps telling the truth."]}
            """),

        new("WC-PRT-008", "Three estimates, one promise",
            "Optimistic, most likely, pessimistic. The airport wants one date and a confidence.",
            "Aviation & Airports", "Senior Planner", "cross_functional", "professional", 10,
            """["schedule_analysis","risk_management"]""",
            """
            {"context":"A baggage-system cutover has three critical activities, each estimated three ways by the people who will do the work. The airport operator will only accept the cutover window if you state the probability of finishing within 38 days.",
             "evidence":[
               {"label":"A — Decommission & strip-out","value":"optimistic 10 · most likely 14 · pessimistic 24"},
               {"label":"B — Install & terminate","value":"optimistic 8 · most likely 10 · pessimistic 18"},
               {"label":"C — Test & handover","value":"optimistic 5 · most likely 7 · pessimistic 15"},
               {"label":"Operator's window","value":"38 days"}],
             "task":"pert",
             "given":{"activities":[
               {"id":"A","o":10,"m":14,"p":24},{"id":"B","o":8,"m":10,"p":18},{"id":"C","o":5,"m":7,"p":15}],
               "deadline":38},
             "ask":[
               {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
               {"key":"std_dev","label":"Path standard deviation (days)","type":"number"},
               {"key":"prob_on_time","label":"Probability of finishing within 38 days (%)","type":"number"}],
             "tolerance":0.02,
             "decisions":[
               {"key":"commit","prompt":"The operator asks: 'So can you commit to 38 days — yes or no?'",
                "options":[
                  {"key":"prob","label":"Commit to the window WITH the stated confidence and a trigger plan if A overruns its P50","quality":100,
                   "consequence":"The operator accepts a probabilistic commitment with a named tripwire — grown-up contracting.",
                   "principle":"A date without a confidence is a wish; a confidence without a trigger plan is a lecture."},
                  {"key":"yes","label":"Say yes — the expected duration is inside the window","quality":20,
                   "consequence":"The one-in-several chance lands; 'you committed' is the only sentence anyone remembers.",
                   "principle":"Committing at the mean means being late roughly half the time."},
                  {"key":"pad","label":"Quote 45 days to be safe","quality":30,
                   "consequence":"The operator books the longer possession; the airline recovers the unused days from your reputation.",
                   "principle":"Silent padding is schedule contingency without governance."}]}],
             "profile_map":{"calculation":"Risk Strategist","decision":"Commercial Negotiator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned three-point estimates into a defensible cutover commitment with a stated confidence.",
             "hints":[
               "Each activity's expected duration weights the most likely estimate four times against one each of optimistic and pessimistic, divided by six.",
               "Path variance adds the activity variances — each is the pessimistic-minus-optimistic span divided by six, then squared — and the standard deviation is its square root.",
               "The probability comes from the normal curve: how many standard deviations sit between the expected duration and the promised window."]}
            """),

        new("WC-TML-009", "Read the trend, not the month",
            "Six months of cumulative data. The story is in the trajectory.",
            "Government Programmes", "Programme Analyst", "project_controls", "advanced", 12,
            """["earned_value","forecasting","governance"]""",
            """
            {"context":"A government service-transformation programme reports cumulative PV, EV and AC for six months against a 45000000 budget. The permanent secretary asks for the year-end position: the final indices, the forecast at completion on current performance, and the variance that forecast implies.",
             "evidence":[
               {"label":"Month 1","value":"PV 3,000,000 · EV 2,700,000 · AC 3,100,000"},
               {"label":"Month 2","value":"PV 7,500,000 · EV 6,900,000 · AC 7,900,000"},
               {"label":"Month 3","value":"PV 13,500,000 · EV 12,200,000 · AC 14,100,000"},
               {"label":"Month 4","value":"PV 21,000,000 · EV 19,000,000 · AC 22,300,000"},
               {"label":"Month 5","value":"PV 30,000,000 · EV 27,500,000 · AC 32,000,000"},
               {"label":"Month 6","value":"PV 38,000,000 · EV 35,600,000 · AC 41,200,000"},
               {"label":"BAC","value":"45,000,000"}],
             "task":"timeline",
             "given":{"bac":45000000,"series":[
               {"period":1,"pv":3000000,"ev":2700000,"ac":3100000},
               {"period":2,"pv":7500000,"ev":6900000,"ac":7900000},
               {"period":3,"pv":13500000,"ev":12200000,"ac":14100000},
               {"period":4,"pv":21000000,"ev":19000000,"ac":22300000},
               {"period":5,"pv":30000000,"ev":27500000,"ac":32000000},
               {"period":6,"pv":38000000,"ev":35600000,"ac":41200000}]},
             "ask":[
               {"key":"final_cpi","label":"Cumulative CPI at month 6","type":"number"},
               {"key":"final_spi","label":"Cumulative SPI at month 6","type":"number"},
               {"key":"final_eac","label":"EAC on current cost performance (BAC ÷ CPI)","type":"number"},
               {"key":"vac","label":"Variance at Completion (BAC − EAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"forecast","prompt":"Which forecast goes to the permanent secretary?",
                "options":[
                  {"key":"cpi","label":"The CPI-based EAC, with the assumption stated and a downside case beside it","quality":100,
                   "consequence":"The department plans against a defensible number and knows exactly what would make it worse.",
                   "principle":"A forecast is an assumption made visible — state it, bound it, own it."},
                  {"key":"bac","label":"Hold the original budget as the forecast — performance may recover","quality":10,
                   "consequence":"Six months of measured under-performance is overruled by hope; the overrun arrives unbudgeted.",
                   "principle":"BAC as EAC after sustained variance is a decision to be surprised later."},
                  {"key":"blend","label":"Average the CPI forecast and the budget to avoid alarming anyone","quality":15,
                   "consequence":"An arithmetic compromise nobody can defend at the select committee.",
                   "principle":"Forecasts are derived, not negotiated."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Forecast a government programme's year-end position from six months of earned-value trend.",
             "hints":[
               "Use the cumulative figures at the data date — the monthly increments are the story, the cumulative line is the position.",
               "The completion forecast divides the total budget by cumulative cost performance; variance at completion is the budget minus that forecast.",
               "Compute the indices first and carry them unrounded into the forecast — rounding before the division moves the year-end answer by more than the tolerance."]}
            """),

        new("WC-AIA-010", "The AI says relax. The data says don't.",
            "An AI assistant wrote the forecast. Your name goes on it. Audit it first.",
            "Technology Transformation", "AI Assurance Reviewer", "governed_ai", "expert", 15,
            """["governed_ai","earned_value","forecasting"]""",
            """
            {"context":"An AI assistant drafted this month's cost note for an ERP consolidation: 'Performance is stabilising. We forecast completion near budget at 7,950,000, as most variance to date is timing-related.' The underlying data: PV 2600000, EV 2210000, AC 2550000, BAC 7800000. Audit the recommendation before it reaches the CFO: compute the real indices and forecast, then judge the AI's reasoning.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"2,600,000"},
               {"label":"Earned Value (EV)","value":"2,210,000"},
               {"label":"Actual Cost (AC)","value":"2,550,000"},
               {"label":"Budget at Completion (BAC)","value":"7,800,000"},
               {"label":"AI draft forecast","value":"'completion near budget at 7,950,000; variance is mostly timing-related'"}],
             "task":"evm","given":{"pv":2600000,"ev":2210000,"ac":2550000,"bac":7800000},
             "ask":[
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
               {"key":"eac","label":"EAC on measured performance (BAC ÷ CPI)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"flaw","prompt":"What is the decisive flaw in the AI's note?",
                "options":[
                  {"key":"ignore","label":"It asserts 'timing-related variance' without evidence, while measured CPI contradicts the near-budget forecast","quality":100,
                   "consequence":"You name the unsupported assumption AND the arithmetic gap — the audit stands on both legs.",
                   "principle":"AI output is audited like any estimate: every claim needs evidence, every number needs a method."},
                  {"key":"tone","label":"The tone is too confident for a draft","quality":15,
                   "consequence":"True but cosmetic — the CFO would have accepted a confidently wrong number.",
                   "principle":"Style critique is not assurance."},
                  {"key":"nothing","label":"Nothing — 7,950,000 is close enough to budget","quality":0,
                   "consequence":"The measured forecast is materially higher; 'close enough' just signed it.",
                   "principle":"Proximity to budget is not evidence of accuracy."}]},
               {"key":"action","prompt":"What do you do with the note?",
                "options":[
                  {"key":"replace","label":"Replace the forecast with the measured EAC, keep the AI's readable structure, and log the correction","quality":100,
                   "consequence":"The CFO gets a defensible number in a clear format; the audit trail shows human assurance worked.",
                   "principle":"Use AI for drafting, never for judgement — and log where judgement overrode it."},
                  {"key":"send","label":"Send it — the AI has been right before","quality":0,
                   "consequence":"Past performance of the assistant is not evidence for this forecast. The gap surfaces in Q3.",
                   "principle":"Trust in a tool is not transferable to an unverified output."},
                  {"key":"ban","label":"Escalate to ban AI drafting entirely","quality":25,
                   "consequence":"The drafting productivity is lost and the ungoverned spreadsheets it replaced come back.",
                   "principle":"The failure was missing assurance, not the existence of the tool."}]}],
             "profile_map":{"calculation":"AI Assurance Reviewer","decision":"AI Assurance Reviewer","balanced":"AI Assurance Reviewer"},
             "share_line":"Audited an AI-drafted cost forecast against the measured data — and corrected it before the CFO saw it.",
             "hints":[
               "Ignore the narrative and re-derive the position: earned against actual gives cost performance, and the forecast follows from it.",
               "The forecast on measured performance divides the full budget by the cost index — not by hope that variance is timing-related.",
               "Compare the derived forecast with the assistant's figure; the gap between them is the size of the claim being walked past the CFO."]}
            """),

        // ── Release-2 batch (WC-…-011…030): the remaining engine families + broader industries. ──

        new("WC-PRD-011", "The crew is working hard. Is the work working?",
            "Hours are up, quantities are down. Measure productivity before blaming anyone.",
            "Construction", "Field Controls Engineer", "project_controls", "foundation", 8,
            """["resource_management","progress_measurement"]""",
            """
            {"context":"A façade package planned to install 1200 square metres of cladding in 800 crew-hours. At the cut-off the crew has booked 840 hours and installed 1050 square metres, and the superintendent insists the crew is 'flat out'. Measure it before the Friday meeting.",
             "evidence":[
               {"label":"Planned quantity","value":"1,200 m²"},
               {"label":"Planned crew-hours","value":"800"},
               {"label":"Installed (earned) quantity","value":"1,050 m²"},
               {"label":"Actual crew-hours booked","value":"840"}],
             "task":"productivity","given":{"planned_qty":1200,"planned_hours":800,"earned_qty":1050,"actual_hours":840},
             "ask":[
               {"key":"planned_productivity","label":"Planned productivity (m² per hour)","type":"number"},
               {"key":"actual_productivity","label":"Actual productivity (m² per hour)","type":"number"},
               {"key":"factor","label":"Productivity factor (actual ÷ planned)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"friday","prompt":"What goes to the Friday production meeting?",
                "options":[
                  {"key":"measured","label":"The measured factor with a joint crew/engineering review of the three worst elevations","quality":100,
                   "consequence":"The review finds access delays, not effort — the fix is scaffolding logistics, not pressure.",
                   "principle":"Productivity is measured output per input, never perceived effort."},
                  {"key":"blame","label":"Report that the crew is underperforming and needs supervision","quality":10,
                   "consequence":"Morale drops, the access problem stays, next month's factor is worse.",
                   "principle":"A factor without a cause analysis is an accusation, not a control."},
                  {"key":"hide","label":"Report hours only — quantities are 'still being verified'","quality":0,
                   "consequence":"The overrun surfaces at the next valuation with two months of momentum behind it.",
                   "principle":"Hours without quantities is spend, not progress."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Measured real crew productivity on a façade package and found the true constraint.",
             "hints":[
               "Productivity is measured output per input hour — planned quantity over planned hours, and installed quantity over booked hours.",
               "The factor divides actual productivity by planned; effort and appearance play no part in either number.",
               "A factor below one with heavy booked hours says the constraint is in the conditions, not the crew — look for what is stealing the hours before Friday."]}
            """),

        new("WC-BOQ-012", "Price the bill before you sign it",
            "Four lines, one total, and a rate that deserves a second look.",
            "Real Estate", "Quantity Surveyor", "project_controls", "foundation", 7,
            """["cost_control","procurement"]""",
            """
            {"context":"A tenant fit-out bill of quantities arrives for sign-off. Recompute the bill total and the average line rate before it goes to the client.",
             "evidence":[
               {"label":"A — Partitions","value":"120 m² at 85 per m²"},
               {"label":"B — Raised floor","value":"45 m² at 240 per m²"},
               {"label":"C — Ceiling tiles","value":"300 m² at 18 per m²"},
               {"label":"D — Glazed doors","value":"60 units at 150 each"}],
             "task":"boq","given":{"lines":[
               {"id":"A","qty":120,"rate":85},{"id":"B","qty":45,"rate":240},
               {"id":"C","qty":300,"rate":18},{"id":"D","qty":60,"rate":150}]},
             "ask":[
               {"key":"total","label":"Bill total","type":"number"},
               {"key":"line_count","label":"Number of bill lines","type":"number"},
               {"key":"average_rate","label":"Average line rate (simple mean of the line rates)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"rate","prompt":"Line B's rate is 40% above the framework schedule. What now?",
                "options":[
                  {"key":"query","label":"Query line B with the framework schedule attached before certifying anything","quality":100,
                   "consequence":"The supplier corrects a specification mix-up; the bill drops before signature.",
                   "principle":"Certify measured quantities at agreed rates — anomalies are queried, not absorbed."},
                  {"key":"sign","label":"Sign it — one line won't matter","quality":5,
                   "consequence":"The certified rate becomes the precedent for every future variation on that item.",
                   "principle":"A signed rate is an agreed rate, forever."},
                  {"key":"cut","label":"Unilaterally reduce line B to the framework rate and pay that","quality":30,
                   "consequence":"The supplier disputes the deduction; the relationship sours over what a query would have fixed.",
                   "principle":"Correction goes through agreement, not through the payment run."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Commercial Negotiator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Recomputed a fit-out bill of quantities and caught the rate that didn't belong.",
             "hints":[
               "Each line extends quantity times rate; the bill total is the sum of the extensions.",
               "The average asked for here is the simple mean across the line rates — a reasonableness check on pricing, not a priced quantity.",
               "Count the priced lines as you extend them, and hold any rate that looks out of family against the framework schedule before certifying."]}
            """),

        new("WC-RES-013", "Five weeks, one crew, too much work",
            "The histogram says the plan needs more people than exist.",
            "Energy & Utilities", "Resource Planner", "project_management", "developing", 9,
            """["resource_management","schedule_analysis"]""",
            """
            {"context":"A substation refurbishment runs five weeks with a single certified jointing crew. Weekly demand versus available capacity is below. Quantify the overload before committing the outage dates.",
             "evidence":[
               {"label":"Week 1","value":"demand 6 · capacity 8"},
               {"label":"Week 2","value":"demand 9 · capacity 8"},
               {"label":"Week 3","value":"demand 11 · capacity 10"},
               {"label":"Week 4","value":"demand 7 · capacity 10"},
               {"label":"Week 5","value":"demand 12 · capacity 10"}],
             "task":"resource","given":{"periods":[
               {"period":1,"demand":6,"capacity":8},{"period":2,"demand":9,"capacity":8},
               {"period":3,"demand":11,"capacity":10},{"period":4,"demand":7,"capacity":10},
               {"period":5,"demand":12,"capacity":10}]},
             "ask":[
               {"key":"peak_demand","label":"Peak weekly demand","type":"number"},
               {"key":"peak_overload","label":"Worst single-week overload","type":"number"},
               {"key":"overloaded_periods","label":"Number of overloaded weeks","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"level","prompt":"The outage window cannot move. How do you resolve the overload?",
                "options":[
                  {"key":"shift","label":"Level within float: pull week-5 work into the week-4 trough and re-check the histogram","quality":100,
                   "consequence":"Two of three overloads clear inside existing float; only one genuine augmentation remains to buy.",
                   "principle":"Use the float you own before the overtime you rent."},
                  {"key":"overtime","label":"Blanket overtime across all five weeks","quality":25,
                   "consequence":"Cost rises everywhere including the weeks that had spare capacity; fatigue risk enters week 3.",
                   "principle":"Paying for capacity you already had is the most expensive kind."},
                  {"key":"hope","label":"Publish the plan as-is — crews always find a way","quality":0,
                   "consequence":"Week 3 slips inside a fixed outage window; the utility's penalty clause finds you.",
                   "principle":"A plan that needs more resources than exist is a wish with a date."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Quantified a crew overload inside a fixed outage window and levelled it with float, not overtime.",
             "hints":[
               "Compare demand with capacity week by week; an overload exists only where demand exceeds what is available.",
               "The worst single week is the largest demand-minus-capacity gap, and the overloaded count is how many weeks show any gap at all.",
               "Peak demand and peak overload are different readings — the busiest week and the most overloaded week need not be the same one."]}
            """),

        new("WC-PRC-014", "The crane is late. Is the project?",
            "A supplier slips 30 days. The network decides what that costs.",
            "Ports & Logistics", "Procurement Coordinator", "cross_functional", "developing", 8,
            """["procurement","schedule_analysis"]""",
            """
            {"context":"The ship-to-shore crane for a terminal expansion slips 30 days at the factory. The receiving works that depend on it hold 12 days of float against the 180-day project. Compute the real schedule damage before the claim letters start.",
             "evidence":[
               {"label":"Project duration (baseline)","value":"180 days"},
               {"label":"Float on the crane-dependent path","value":"12 days"},
               {"label":"Supplier-notified delay","value":"30 days"}],
             "task":"procurement","given":{"project_duration":180,"remaining_float":12,"supplier_delay_days":30},
             "ask":[
               {"key":"critical_delay","label":"Delay beyond available float (days)","type":"number"},
               {"key":"new_project_duration","label":"New project duration (days)","type":"number"},
               {"key":"float_consumed","label":"Float consumed (days)","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"response","prompt":"What is the first response?",
                "options":[
                  {"key":"mitigate","label":"Price expedited shipping and a re-sequenced quay fit-out against the computed critical delay","quality":100,
                   "consequence":"Mitigation costs less than the terminal's delay damages — and the comparison is documented.",
                   "principle":"Mitigation is a purchase: compare its price with the delay it removes."},
                  {"key":"claim","label":"Go straight to the delay claim — the supplier is liable","quality":30,
                   "consequence":"Liability may be theirs; the opening delay is still yours. Claims recover money, not weeks.",
                   "principle":"Mitigate first, claim after — the duty and the leverage both point that way."},
                  {"key":"absorb","label":"Absorb it quietly — 30 days on 180 is manageable","quality":5,
                   "consequence":"The delay lands on the critical path at full force; the operator learns from the newspaper.",
                   "principle":"Only float absorbs delay; the rest lands on the completion date."}]}],
             "profile_map":{"calculation":"Schedule Detective","decision":"Commercial Negotiator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Separated real critical delay from float noise when a 30-day supplier slip hit a port project.",
             "hints":[
               "Float absorbs delay before the completion date moves; only the slippage beyond the available float reaches the project.",
               "Critical delay is the supplier slip minus the float held by the receiving works, never less than zero.",
               "The new duration adds only the critical portion to the baseline — and the float consumed can never exceed the float that existed."]}
            """),

        new("WC-PTF-015", "Five projects, one envelope",
            "Score the portfolio the way the investment committee should.",
            "Portfolio & PMO", "Portfolio Analyst", "project_finance", "professional", 12,
            """["portfolio_management","decision_analysis"]""",
            """
            {"context":"A capital committee can fund three of five candidate projects. Score every candidate with the stated weights — NPV 0.5, risk 0.3, strategic fit 0.2 — and total the portfolio NPV before the debate starts.",
             "evidence":[
               {"label":"A — Warehouse automation","value":"NPV 4,200,000 · risk 0.30 · fit 0.80"},
               {"label":"B — New cold-chain hub","value":"NPV 6,800,000 · risk 0.50 · fit 0.70"},
               {"label":"C — Fleet telematics","value":"NPV 2,900,000 · risk 0.20 · fit 0.90"},
               {"label":"D — Cross-dock expansion","value":"NPV 5,400,000 · risk 0.35 · fit 0.75"},
               {"label":"E — Solar canopies","value":"NPV 3,600,000 · risk 0.25 · fit 0.60"},
               {"label":"Weights","value":"NPV 0.5 · risk 0.3 · fit 0.2"}],
             "task":"portfolio","given":{"w_npv":0.5,"w_risk":0.3,"w_fit":0.2,"projects":[
               {"id":"A","npv":4200000,"risk":0.3,"fit":0.8},{"id":"B","npv":6800000,"risk":0.5,"fit":0.7},
               {"id":"C","npv":2900000,"risk":0.2,"fit":0.9},{"id":"D","npv":5400000,"risk":0.35,"fit":0.75},
               {"id":"E","npv":3600000,"risk":0.25,"fit":0.6}]},
             "ask":[
               {"key":"total_npv","label":"Total candidate NPV","type":"number"},
               {"key":"top_score","label":"Highest weighted score","type":"number"},
               {"key":"score_A","label":"Weighted score for project A","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"present","prompt":"A director wants project E promoted because 'sustainability is priority one'. What do you do?",
                "options":[
                  {"key":"reweight","label":"Offer to re-run the model with a committee-agreed sustainability weight, shown beside the current ranking","quality":100,
                   "consequence":"The committee changes the weights in the open; E rises on the record, not in the corridor.",
                   "principle":"Change the model, never the answer — and change it in public."},
                  {"key":"bump","label":"Quietly move E up one place — it is close anyway","quality":0,
                   "consequence":"The audit committee later asks why the published ranking disagrees with the model. There is no good answer.",
                   "principle":"A scoring model you override silently is worse than no model."},
                  {"key":"refuse","label":"Refuse: the weights are the weights","quality":40,
                   "consequence":"Technically clean, politically deaf — the committee overrides the whole framework next cycle.",
                   "principle":"Frameworks survive by absorbing governance, not resisting it."}]}],
             "profile_map":{"calculation":"Finance Thinker","decision":"Strategic Project Controller","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Scored a five-project capital portfolio and defended the model when politics leaned on it.",
             "hints":[
               "Score each candidate with the stated weights exactly as agreed — normalise where the method requires, then weight and sum.",
               "The portfolio value asked for totals every candidate's appraised worth, not only the funded ones.",
               "Rank by the weighted score; a plea for one criterion is an argument to change the weights before scoring, never to override the ranking after it."]}
            """),

        new("WC-DEC-016", "Three ways up the mountain",
            "Cost, time and risk pull in different directions. Weigh them, then decide.",
            "Mining", "Project Engineer", "project_management", "professional", 10,
            """["decision_analysis","risk_management"]""",
            """
            {"context":"A haul-road realignment has three options. The stated decision weights are equal for cost, schedule and risk. Compute the weighted scores, then handle the pushback.",
             "evidence":[
               {"label":"A — Ridge route","value":"cost 500,000 · 20 weeks · risk 0.40"},
               {"label":"B — Tunnel spur","value":"cost 800,000 · 10 weeks · risk 0.20"},
               {"label":"C — Valley switchbacks","value":"cost 300,000 · 35 weeks · risk 0.60"},
               {"label":"Weights","value":"cost 1 · schedule 1 · risk 1"}],
             "task":"decision","given":{"w_cost":1,"w_sched":1,"w_risk":1,"options":[
               {"id":"A","cost":500000,"schedule":20,"risk":0.4},
               {"id":"B","cost":800000,"schedule":10,"risk":0.2},
               {"id":"C","cost":300000,"schedule":35,"risk":0.6}]},
             "ask":[
               {"key":"best_score","label":"Best weighted score","type":"number"},
               {"key":"score_A","label":"Weighted score — option A","type":"number"},
               {"key":"score_B","label":"Weighted score — option B","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"model","prompt":"The mine manager says: 'Just pick the cheapest, the model is spreadsheet games.'",
                "options":[
                  {"key":"show","label":"Walk the manager through what the cheap option costs in schedule and risk terms, using the scores","quality":100,
                   "consequence":"The manager keeps the model — and owns the trade-off consciously instead of by default.",
                   "principle":"A decision model exists to make trade-offs visible, not to make the decision."},
                  {"key":"cheap","label":"Take option C — the manager signs the cheques","quality":15,
                   "consequence":"Fifteen extra weeks of haulage on the old road quietly cost more than the saving.",
                   "principle":"Cheapest capital cost is rarely cheapest project cost."},
                  {"key":"silent","label":"Submit the model's answer without discussion","quality":35,
                   "consequence":"The recommendation is right and ignored — analysis without engagement changes nothing.",
                   "principle":"An unexplained model is an unused model."}]}],
             "profile_map":{"calculation":"Evidence-Based Decision Maker","decision":"Commercial Negotiator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Scored three haul-road options on cost, time and risk — and kept the trade-offs on the table.",
             "hints":[
               "Apply the equal weights to each option's normalised cost, schedule and risk measures, then sum per option.",
               "Keep the normalisation direction consistent — a better option must score better, whichever way the raw scales run.",
               "Compute both named options before naming the winner; the model's defence is that anyone applying the same weights reaches the same answer."]}
            """),

        new("WC-DQA-017", "Garbage in, forecast out",
            "Before the model runs, someone has to look at the data. Today that is you.",
            "Telecommunications", "Data Quality Analyst", "governed_ai", "professional", 10,
            """["data_quality","governed_ai"]""",
            """
            {"context":"A fibre roll-out feeds weekly completion counts into an AI forecast. Before this week's run, audit the eight submitted records against the design quantities: anomalies beyond the agreed ±5 tolerance, completeness, and the error level.",
             "evidence":[
               {"label":"Zone 1","value":"expected 100 · reported 102"},
               {"label":"Zone 2","value":"expected 95 · reported 103"},
               {"label":"Zone 3","value":"expected 110 · reported (missing)"},
               {"label":"Zone 4","value":"expected 120 · reported 119"},
               {"label":"Zone 5","value":"expected 80 · reported 94"},
               {"label":"Zone 6","value":"expected 105 · reported 104"},
               {"label":"Zone 7","value":"expected 90 · reported (missing)"},
               {"label":"Zone 8","value":"expected 100 · reported 100"},
               {"label":"Anomaly tolerance","value":"±5"}],
             "task":"data_quality","given":{"threshold":5,"rows":[
               {"expected":100,"value":102},{"expected":95,"value":103},{"expected":110},
               {"expected":120,"value":119},{"expected":80,"value":94},{"expected":105,"value":104},
               {"expected":90},{"expected":100,"value":100}]},
             "ask":[
               {"key":"record_count","label":"Records expected","type":"number"},
               {"key":"anomaly_count","label":"Anomalies beyond tolerance","type":"number"},
               {"key":"completeness_pct","label":"Completeness (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"run","prompt":"The forecast is due at 09:00. Run it?",
                "options":[
                  {"key":"flag","label":"Run it with the two anomalous zones excluded and the gaps flagged, and open a data query with the field teams","quality":100,
                   "consequence":"The forecast ships with a stated confidence and a shrinking exclusion list — trust in it grows.",
                   "principle":"A model's output inherits the quality of its worst unflagged input."},
                  {"key":"all","label":"Run it on everything — more data beats clean data","quality":10,
                   "consequence":"The two bad zones drag the regional forecast; a month later nobody trusts the tool.",
                   "principle":"More data is only better when it is not wrong."},
                  {"key":"stop","label":"Refuse to run anything until every record is perfect","quality":30,
                   "consequence":"Perfection never arrives; planning reverts to gut feel, which is dirtier than the data was.",
                   "principle":"Data quality is managed with flags and queries, not ultimatums."}]}],
             "profile_map":{"calculation":"AI Assurance Reviewer","decision":"AI Assurance Reviewer","balanced":"AI Assurance Reviewer"},
             "share_line":"Audited a roll-out data feed before the AI forecast ran — and shipped it with stated confidence.",
             "hints":[
               "Audit against the design quantities: a record is anomalous only when its deviation exceeds the agreed tolerance.",
               "Completeness compares records received against records expected, expressed as a percentage.",
               "Count expected records from the design, not from what arrived — a missing record is a completeness failure, not an anomaly."]}
            """),

        new("WC-EVM-018", "Turnaround truth at 3 a.m.",
            "A refinery shutdown burns money by the hour. Where does it actually stand?",
            "Oil & Gas", "Turnaround Controls Lead", "project_controls", "foundation", 8,
            """["earned_value","progress_measurement"]""",
            """
            {"context":"Midway through a refinery turnaround the daily report shows Planned Value 5200000, Earned Value 4680000 and Actual Cost 5000000 against a 15600000 budget. The shutdown manager wants the position, plain.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"5,200,000"},
               {"label":"Earned Value (EV)","value":"4,680,000"},
               {"label":"Actual Cost (AC)","value":"5,000,000"},
               {"label":"Budget at Completion (BAC)","value":"15,600,000"}],
             "task":"evm","given":{"pv":5200000,"ev":4680000,"ac":5000000,"bac":15600000},
             "ask":[
               {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
               {"key":"cv","label":"Cost Variance (CV)","type":"number"},
               {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
               {"key":"percent_complete","label":"Percent complete (EV ÷ BAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"night","prompt":"The night-shift superintendent offers to 'book more progress' on scaffolding to improve the morning numbers.",
                "options":[
                  {"key":"refuse","label":"Refuse, and re-verify the scaffold progress basis at shift handover","quality":100,
                   "consequence":"The numbers stay believable — which is what makes the recovery plan fundable.",
                   "principle":"Earned value is earned, not negotiated."},
                  {"key":"accept","label":"Accept it — morale needs a good morning report","quality":0,
                   "consequence":"The inflated progress unwinds during systems completion, at the worst possible hour.",
                   "principle":"Borrowed progress is repaid with interest at handover."},
                  {"key":"ignore","label":"Say nothing and let the booking stand unexamined","quality":10,
                   "consequence":"You did not inflate it, but you certified it — the audit will not see the difference.",
                   "principle":"Silence over a known misstatement is authorship."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Strategic Project Controller","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reported a refinery turnaround's true earned-value position under pressure to soften it.",
             "hints":[
               "All five readings come from the same three cumulative figures at the data date — earned, planned and actual — against the full budget.",
               "Variances subtract from earned value; indices divide it by planned and by actual; percent complete divides earned by the budget.",
               "If a proposed improvement changes earned value without work being done, every downstream number becomes fiction — compute from what was verifiably earned."]}
            """),

        new("WC-CSH-019", "The show opens in five months",
            "Ticket money arrives late. Stage builds are paid early. Mind the gap.",
            "Events & Mega-Projects", "Commercial Analyst", "project_finance", "developing", 9,
            """["cash_flow","forecasting"]""",
            """
            {"context":"A stadium concert series pays production suppliers monthly, while ticketing revenue settles later. Roll the five months below into the cumulative position and find the peak funding need before the promoter signs the bridge facility.",
             "evidence":[
               {"label":"Month 1","value":"inflow 0 · outflow 450,000"},
               {"label":"Month 2","value":"inflow 200,000 · outflow 500,000"},
               {"label":"Month 3","value":"inflow 900,000 · outflow 400,000"},
               {"label":"Month 4","value":"inflow 600,000 · outflow 350,000"},
               {"label":"Month 5","value":"inflow 300,000 · outflow 200,000"}],
             "task":"cashflow","given":{"periods":[
               {"period":1,"inflow":0,"outflow":450000},{"period":2,"inflow":200000,"outflow":500000},
               {"period":3,"inflow":900000,"outflow":400000},{"period":4,"inflow":600000,"outflow":350000},
               {"period":5,"inflow":300000,"outflow":200000}]},
             "ask":[
               {"key":"final_position","label":"Final (closing) cash position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"},
               {"key":"cumulative_3","label":"Cumulative position at end of month 3","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"bridge","prompt":"The promoter asks: 'Can we skip the facility if we just delay the stage contractor a month?'",
                "options":[
                  {"key":"honest","label":"Show both curves: delayed payment shifts the trough but adds late-payment risk to the one supplier who can sink the opening","quality":100,
                   "consequence":"The promoter funds the facility once the supplier-failure scenario is priced next to the fee.",
                   "principle":"Stretching creditors is financing too — price its risk like any other facility."},
                  {"key":"yes","label":"Yes — stretching payables is free money","quality":10,
                   "consequence":"The stage contractor deprioritises the build; opening week gets very exciting.",
                   "principle":"A supplier financing your project involuntarily is a risk, not a source of funds."},
                  {"key":"no","label":"No — never touch payment terms","quality":40,
                   "consequence":"Defensible, but a genuine negotiation opportunity with willing suppliers goes unexplored.",
                   "principle":"Terms are negotiable; surprises are not."}]}],
             "profile_map":{"calculation":"Finance Thinker","decision":"Commercial Negotiator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sized the funding trough for a live-events season before the bridge facility was signed.",
             "hints":[
               "Roll the months in sequence — each period's net movement joins the running cumulative position.",
               "The peak funding need is the deepest cumulative shortfall across the run, wherever it falls.",
               "Test any proposed deferral by rebuilding the cumulative line with the payment moved — the shortfall usually moves with it rather than shrinking."]}
            """),

        new("WC-RSK-020", "The spillway question",
            "Four risks, one register, and a board that wants a single number.",
            "Water", "Risk Coordinator", "cross_functional", "foundation", 8,
            """["risk_management"]""",
            """
            {"context":"A dam spillway upgrade carries the quantified register below. The board wants the net Expected Monetary Value and the two lines they should actually watch.",
             "evidence":[
               {"label":"W1 — Wet-season access loss","value":"probability 0.25 · impact -600,000"},
               {"label":"W2 — Concrete supply interruption","value":"probability 0.40 · impact -250,000"},
               {"label":"W3 — Geotechnical surprise at anchor block","value":"probability 0.10 · impact -1,500,000"},
               {"label":"W4 — Early completion incentive","value":"probability 0.30 · impact +200,000"}],
             "task":"risk","given":{"risks":[
               {"id":"W1","probability":0.25,"impact":-600000},{"id":"W2","probability":0.4,"impact":-250000},
               {"id":"W3","probability":0.1,"impact":-1500000},{"id":"W4","probability":0.3,"impact":200000}]},
             "ask":[
               {"key":"emv","label":"Register EMV (net)","type":"number"},
               {"key":"emv_W2","label":"EMV of W2 — concrete supply","type":"number"},
               {"key":"emv_W4","label":"EMV of W4 — completion incentive","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"watch","prompt":"Which line deserves the board's attention beyond the EMV table?",
                "options":[
                  {"key":"tail","label":"W3 — low probability, but an impact the project cannot absorb if it lands","quality":100,
                   "consequence":"The board funds early ground investigation; the tail either closes or gets priced properly.",
                   "principle":"EMV averages away exactly the events that end projects — read the tail separately."},
                  {"key":"biggest","label":"W2 — it has the highest probability, so it is the biggest risk","quality":30,
                   "consequence":"W2 is real but survivable; the register's true monster stays unexamined.",
                   "principle":"Probability ranks frequency, not danger."},
                  {"key":"upside","label":"W4 — lead with the good news","quality":10,
                   "consequence":"A pleasant meeting, an unprepared project.",
                   "principle":"Opportunities season a risk review; they must not headline it."}]}],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a dam-upgrade risk register and pointed the board at the tail, not the average.",
             "hints":[
               "Value each line as probability times signed impact — threats carry a negative sign, incentives and opportunities a positive one.",
               "The register value nets all lines; the individual lines asked for are read before any netting.",
               "The lines worth watching are the ones whose single-occurrence impact dwarfs their expected value — expectation understates what one bad day can do."]}
            """),

        new("WC-PRT-021", "The integration window",
            "Radar, software, and a frigate that sails on the tide, not the plan.",
            "Defence", "Integration Planner", "project_controls", "advanced", 12,
            """["schedule_analysis","risk_management"]""",
            """
            {"context":"A shipborne radar integration has three sequential activities, each three-point estimated by the integration teams. The vessel's next availability window closes 33 days after start. Give the programme office the expected duration, the variance, and the probability of making the window.",
             "evidence":[
               {"label":"A — Dockside installation","value":"optimistic 6 · most likely 9 · pessimistic 18"},
               {"label":"B — Power-on and BIT","value":"optimistic 4 · most likely 6 · pessimistic 8"},
               {"label":"C — Sea acceptance trials","value":"optimistic 8 · most likely 12 · pessimistic 22"},
               {"label":"Availability window","value":"33 days"}],
             "task":"pert","given":{"activities":[
               {"id":"A","o":6,"m":9,"p":18},{"id":"B","o":4,"m":6,"p":8},{"id":"C","o":8,"m":12,"p":22}],
               "deadline":33},
             "ask":[
               {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
               {"key":"variance","label":"Path variance","type":"number"},
               {"key":"prob_on_time","label":"Probability of finishing within the window (%)","type":"number"}],
             "tolerance":0.02,
             "decisions":[
               {"key":"window","prompt":"The programme office asks for 'a commitment, not a distribution'.",
                "options":[
                  {"key":"trigger","label":"Commit to the window at the computed confidence, with a pre-agreed descope of trial scenarios if A exceeds its most-likely","quality":100,
                   "consequence":"The window holds because the descope decision was made before it was needed.",
                   "principle":"Confidence plus a pre-authorised fallback is what a real commitment looks like."},
                  {"key":"flat","label":"Commit flat — the expected duration fits","quality":15,
                   "consequence":"The distribution's right tail meets the tide table; the next window is four months away.",
                   "principle":"Committing at the mean books the miss in advance, roughly half the time."},
                  {"key":"refuse","label":"Refuse to commit to anything probabilistic","quality":20,
                   "consequence":"The programme office finds a planner who will — with worse numbers.",
                   "principle":"Declining to quantify uncertainty does not remove it; it just removes you."}]}],
             "profile_map":{"calculation":"Risk Strategist","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put a defensible confidence on a warship integration window — with the fallback pre-agreed.",
             "hints":[
               "Weight each activity's most likely duration four times against the optimistic and pessimistic, divide by six, then sum the path.",
               "The path variance sums each activity's squared sixth-span; leave it squared — this question asks for variance, not deviation.",
               "For the window probability, take the square root to get the deviation, measure how far the window sits from the expected finish, and read the normal curve."]}
            """),

        new("WC-CPM-022", "The lab that opens in seventeen days — or doesn't",
            "Five activities between you and a validated cleanroom.",
            "Pharmaceutical", "Commissioning Planner", "project_controls", "developing", 9,
            """["schedule_analysis"]""",
            """
            {"context":"A sterile-suite commissioning sequence has five activities. Quality insists activity B — document review — is the bottleneck. Run the pass before anyone reorganises the plan around a guess.",
             "evidence":[
               {"label":"A — Mechanical completion checks","value":"4 days · no predecessors"},
               {"label":"B — Document review","value":"3 days · after A"},
               {"label":"C — HVAC balancing","value":"6 days · after A"},
               {"label":"D — Particle qualification","value":"5 days · after B and C"},
               {"label":"E — Handover certification","value":"2 days · after D"}],
             "task":"cpm","given":{"activities":[
               {"id":"A","dur":4,"preds":[]},{"id":"B","dur":3,"preds":["A"]},{"id":"C","dur":6,"preds":["A"]},
               {"id":"D","dur":5,"preds":["B","C"]},{"id":"E","dur":2,"preds":["D"]}]},
             "ask":[
               {"key":"project_duration","label":"Sequence duration (days)","type":"number"},
               {"key":"critical_path","label":"Critical path (comma-separated activity IDs)","type":"set"},
               {"key":"float_B","label":"Total float of activity B (days)","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"bottleneck","prompt":"Quality wants two extra reviewers to accelerate document review. Approve?",
                "options":[
                  {"key":"no","label":"Show that B carries float — offer the reviewers to HVAC balancing support instead","quality":100,
                   "consequence":"The real driver gets the help; document review finishes comfortably inside its float anyway.",
                   "principle":"Resources follow the critical path, not the loudest concern."},
                  {"key":"yes","label":"Approve — quality knows their own workload","quality":15,
                   "consequence":"B finishes early and waits for C regardless; the qualification date does not move.",
                   "principle":"Accelerating a floating activity buys idle time, not schedule."},
                  {"key":"both","label":"Add people to both B and C to be safe","quality":40,
                   "consequence":"The date improves at twice the necessary cost; 'to be safe' becomes the budget's epitaph.",
                   "principle":"Targeted acceleration beats blanket acceleration everywhere it matters."}]}],
             "profile_map":{"calculation":"Schedule Detective","decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ran a cleanroom commissioning network and sent the acceleration where the path actually was.",
             "hints":[
               "Forward pass for earliest dates, backward pass for latest — the sequence duration is the longest dependent chain.",
               "Total float is latest start minus earliest start per activity; the critical path is where it vanishes.",
               "Before adding reviewers anywhere, read the float on the activity in question — acceleration only moves the end date on the zero-float chain."]}
            """),

        new("WC-CHG-023", "The campus that grew in committee",
            "Approved, pending, rejected: only one column changes the baseline.",
            "Education & Government", "Assistant Project Manager", "project_management", "foundation", 8,
            """["change_control"]""",
            """
            {"context":"A college campus refurbishment holds a baseline of 3,500,000 over 150 days. The change register lists four items. Build the revised position from approved changes only, then handle the classic corridor request.",
             "evidence":[
               {"label":"Baseline","value":"3,500,000 · 150 days"},
               {"label":"G1 — Accessibility upgrades","value":"APPROVED · +180,000 · +8 days"},
               {"label":"G2 — Lecture-capture AV","value":"PENDING · +90,000 · +4 days"},
               {"label":"G3 — Reuse existing lab benches","value":"APPROVED · -40,000 · 0 days"},
               {"label":"G4 — Rooftop terrace","value":"REJECTED · +250,000 · +12 days"}],
             "task":"change","given":{"baseline_bac":3500000,"baseline_duration":150,"changes":[
               {"id":"G1","status":"approved","cost_delta":180000,"schedule_delta":8},
               {"id":"G2","status":"pending","cost_delta":90000,"schedule_delta":4},
               {"id":"G3","status":"approved","cost_delta":-40000,"schedule_delta":0},
               {"id":"G4","status":"rejected","cost_delta":250000,"schedule_delta":12}]},
             "ask":[
               {"key":"revised_bac","label":"Revised budget (approved changes only)","type":"number"},
               {"key":"approved_cost_delta","label":"Net approved cost delta","type":"number"},
               {"key":"approved_count","label":"Approved change count","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"corridor","prompt":"The dean mentions the rejected terrace 'could just be folded into the roof repairs package, surely'.",
                "options":[
                  {"key":"route","label":"Explain the change route and offer to sponsor a properly costed resubmission","quality":100,
                   "consequence":"The terrace returns as an honest change with funding attached — or dies an honest death.",
                   "principle":"There is one road onto the baseline, and it has a gate with minutes."},
                  {"key":"fold","label":"Fold it in — the dean chairs the steering group after all","quality":0,
                   "consequence":"The 'roof repairs' overrun is discovered by internal audit, with your initials on the instruction.",
                   "principle":"Scope hidden inside another package is misallocation with a paper trail."},
                  {"key":"ignore","label":"Smile, nod, do nothing","quality":30,
                   "consequence":"The dean asks the contractor directly next week; now it is everyone's problem.",
                   "principle":"An unanswered improper request finds a weaker door."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Strategic Project Controller","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a campus baseline together by keeping every change on the one road through the gate.",
             "hints":[
               "Approved items only: pending, rejected and folded-in-quietly items never touch the baseline.",
               "The net approved delta sums the approved items' signed cost changes; the revised budget applies that delta to the original baseline.",
               "A rejected item absorbed into another package is scope without approval — the register, not the corridor, is the bridge."]}
            """),

        new("WC-ESC-024", "Ten sprints, one deadline, and a curve that doesn't lie",
            "The burn-up says on-track. Earned Schedule says otherwise.",
            "Technology Transformation", "Delivery Controls Analyst", "project_controls", "advanced", 12,
            """["schedule_analysis","forecasting"]""",
            """
            {"context":"A digital-service migration plans ten periods to earn 1000 points of value. At the end of period 6 the team has earned 400 against the plan below. Read the schedule in time: earned schedule, the time-based index, and the independent completion forecast.",
             "evidence":[
               {"label":"Planned value curve","value":"P1 50 · P2 120 · P3 210 · P4 320 · P5 450 · P6 600 · P7 760 · P8 880 · P9 950 · P10 1000"},
               {"label":"Earned to date","value":"400 at end of period 6"},
               {"label":"Planned duration","value":"10 periods"}],
             "task":"earned_schedule","given":{"planned_duration":10,"at":6,"ev":400,
               "plan":[{"period":1,"pv":50},{"period":2,"pv":120},{"period":3,"pv":210},{"period":4,"pv":320},
                       {"period":5,"pv":450},{"period":6,"pv":600},{"period":7,"pv":760},{"period":8,"pv":880},
                       {"period":9,"pv":950},{"period":10,"pv":1000}]},
             "ask":[
               {"key":"es","label":"Earned Schedule (periods)","type":"number"},
               {"key":"spi_time","label":"Time-based index — SPI(t)","type":"number"},
               {"key":"eac_time","label":"Independent time forecast — IEAC(t)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"replan","prompt":"The forecast lands well past the funded end date. The programme board meets Thursday.",
                "options":[
                  {"key":"options","label":"Table the time forecast with three costed responses: descope, extend, or re-sequence the release plan","quality":100,
                   "consequence":"The board chooses with numbers on the table — and owns the choice it makes.",
                   "principle":"Bad schedule news arrives best already attached to its decision options."},
                  {"key":"velocity","label":"Report that velocity is improving and the team feels confident","quality":10,
                   "consequence":"Feelings meet arithmetic in period 9. Arithmetic wins.",
                   "principle":"A trend in effort is not a trend in earned value."},
                  {"key":"quiet","label":"Wait two more periods for certainty before alarming the board","quality":5,
                   "consequence":"The two periods consume the exact options the board needed to choose between.",
                   "principle":"Certainty about a slip arrives after the last chance to fix it."}]}],
             "profile_map":{"calculation":"Schedule Detective","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Forecast a digital programme's real completion in time units — and brought options, not apologies.",
             "hints":[
               "Find when the plan expected the current earned points — that period, plus the interpolated fraction, is the earned schedule.",
               "The time-based index divides earned schedule by the periods elapsed.",
               "The independent forecast divides the planned length by the time index — it assumes the pace to date continues, which is exactly the assumption to surface on Thursday."]}
            """),

        new("WC-TML-025", "Five months of drift",
            "Each month looked survivable. Together they are a trend.",
            "Data Centres", "Cost Engineer", "project_controls", "professional", 11,
            """["earned_value","forecasting"]""",
            """
            {"context":"A hyperscale fit-out reports five months of cumulative PV, EV and AC against a 24,000,000 budget. Compute the month-5 position and the completion forecast on measured performance.",
             "evidence":[
               {"label":"Month 1","value":"PV 2,000,000 · EV 1,900,000 · AC 2,100,000"},
               {"label":"Month 2","value":"PV 5,000,000 · EV 4,700,000 · AC 5,200,000"},
               {"label":"Month 3","value":"PV 9,000,000 · EV 8,400,000 · AC 9,500,000"},
               {"label":"Month 4","value":"PV 14,000,000 · EV 13,000,000 · AC 14,800,000"},
               {"label":"Month 5","value":"PV 19,000,000 · EV 17,600,000 · AC 20,200,000"},
               {"label":"BAC","value":"24,000,000"}],
             "task":"timeline","given":{"bac":24000000,"series":[
               {"period":1,"pv":2000000,"ev":1900000,"ac":2100000},
               {"period":2,"pv":5000000,"ev":4700000,"ac":5200000},
               {"period":3,"pv":9000000,"ev":8400000,"ac":9500000},
               {"period":4,"pv":14000000,"ev":13000000,"ac":14800000},
               {"period":5,"pv":19000000,"ev":17600000,"ac":20200000}]},
             "ask":[
               {"key":"final_cpi","label":"Cumulative CPI at month 5","type":"number"},
               {"key":"final_spi","label":"Cumulative SPI at month 5","type":"number"},
               {"key":"final_eac","label":"EAC on measured performance (BAC ÷ CPI)","type":"number"},
               {"key":"vac","label":"Variance at Completion (BAC − EAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"trend","prompt":"The client notes each individual month was 'within normal variation'. Your answer?",
                "options":[
                  {"key":"cusum","label":"Show the cumulative indices month by month: five small variances, one direction, zero reversals","quality":100,
                   "consequence":"The client sees drift, not noise — and funds the recovery review this month instead of quarter-end.",
                   "principle":"Noise alternates; drift accumulates. The cumulative curve tells them apart."},
                  {"key":"agree","label":"Agree — no single month breached the threshold","quality":5,
                   "consequence":"Technically true, managerially blind; month 8 breaches everything at once.",
                   "principle":"Thresholds on single periods are exactly what slow drift walks under."},
                  {"key":"panic","label":"Declare a cost crisis and demand a stop-work review","quality":25,
                   "consequence":"A 6% drift gets a 100% response; credibility spends faster than money.",
                   "principle":"Calibrate the response to the measured trend, not to the adrenaline."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned five 'normal' months of data-centre variance into one undeniable trend.",
             "hints":[
               "Read the cumulative position at the latest month — the monthly rows exist to build the cumulative line.",
               "Forecast at completion divides the budget by cumulative cost performance; the variance at completion is budget minus forecast.",
               "Months individually within normal variation can still compound into a material drift — the cumulative indices are where the trend stops hiding."]}
            """),

        new("WC-WBS-026", "Every dollar needs an address",
            "A hangar project's budget, rolled up from the bottom.",
            "Aerospace", "Project Controls Trainee", "project_controls", "foundation", 7,
            """["scope_structuring"]""",
            """
            {"context":"An MRO hangar upgrade has the work breakdown below with leaf-level budgets. Roll the structure up to the root and confirm it satisfies the 100% rule before cost accounts are opened.",
             "evidence":[
               {"label":"1.1 Design","value":"250,000"},
               {"label":"1.2 Build (parent)","value":"—"},
               {"label":"1.2.1 Structure","value":"450,000"},
               {"label":"1.2.2 Systems","value":"380,000"},
               {"label":"1.3 Certification","value":"120,000"}],
             "task":"wbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"Hangar upgrade"},
               {"id":"1.1","parent":"1","name":"Design","value":250000},
               {"id":"1.2","parent":"1","name":"Build"},
               {"id":"1.2.1","parent":"1.2","name":"Structure","value":450000},
               {"id":"1.2.2","parent":"1.2","name":"Systems","value":380000},
               {"id":"1.3","parent":"1","name":"Certification","value":120000}]},
             "ask":[
               {"key":"root_total","label":"Total project budget (root roll-up)","type":"number"},
               {"key":"hundred_percent_valid","label":"Does the WBS satisfy the 100% rule? (yes/no)","type":"bool"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"orphan","prompt":"The avionics-bay tooling scope appears in no work package. The engineer says 'it's covered under Systems, spiritually'.",
                "options":[
                  {"key":"add","label":"Add it explicitly — as a new package or a documented widening of 1.2.2's dictionary entry","quality":100,
                   "consequence":"The scope has an address, a budget and an owner before a single invoice arrives.",
                   "principle":"The 100% rule is not bureaucracy; it is where unfunded scope goes to be found early."},
                  {"key":"spirit","label":"Accept the spiritual coverage","quality":0,
                   "consequence":"Six months later two packages both assume the other one paid for the tooling.",
                   "principle":"Scope covered 'spiritually' is billed literally."},
                  {"key":"later","label":"Note it for the next baseline update","quality":20,
                   "consequence":"Procurement starts before the next update; the address arrives after the mail.",
                   "principle":"Structure precedes spend, or spend invents its own structure."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rolled up a hangar WBS and gave a homeless scope item an address before it billed anyone.",
             "hints":[
               "Roll the leaves upward: every parent is the sum of its children, level by level, to the root.",
               "The hundred-percent test asks whether every scope element lives in exactly one work package — orphaned scope fails it.",
               "Scope covered only in spirit is scope with no budget address; if it appears in no package, the structure does not satisfy the rule."]}
            """),

        new("WC-CBS-027", "Where the depot money went",
            "Three cost accounts, one honest roll-up.",
            "Rail", "Cost Analyst", "project_controls", "developing", 8,
            """["cost_control"]""",
            """
            {"context":"A rolling-stock depot refurbishment has three cost accounts. Roll budget and actuals to the root and state the variance the monthly report must carry.",
             "evidence":[
               {"label":"1.1 Track & pits","value":"budget 900,000 · actual 875,000"},
               {"label":"1.2 Overhead line","value":"budget 700,000 · actual 760,000"},
               {"label":"1.3 Depot systems","value":"budget 400,000 · actual 415,000"}],
             "task":"cbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"Depot refurbishment"},
               {"id":"1.1","parent":"1","name":"Track & pits","budget":900000,"actual":875000},
               {"id":"1.2","parent":"1","name":"Overhead line","budget":700000,"actual":760000},
               {"id":"1.3","parent":"1","name":"Depot systems","budget":400000,"actual":415000}]},
             "ask":[
               {"key":"root_budget","label":"Total budget (root)","type":"number"},
               {"key":"root_actual","label":"Total actual (root)","type":"number"},
               {"key":"root_variance","label":"Variance at the root (budget − actual)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"netting","prompt":"The overhead-line overrun nets against the track underrun at the root. Report which number?",
                "options":[
                  {"key":"both","label":"Report the net position AND both gross variances, with the OLE overrun explained","quality":100,
                   "consequence":"The board sees a controlled project: honest at the root, explained at the account.",
                   "principle":"Netting is arithmetic; reporting only the net is concealment."},
                  {"key":"net","label":"Report the net — that is what the root says","quality":20,
                   "consequence":"The OLE trend continues unexamined under the tidy net; next month it outgrows the offset.",
                   "principle":"Underruns lent to overruns are rarely repaid."},
                  {"key":"move","label":"Transfer budget from track to OLE so both lines report zero","quality":5,
                   "consequence":"The baseline dissolves one transfer at a time until variance means nothing.",
                   "principle":"Budget transfers follow change control, not reporting convenience."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Strategic Project Controller","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rolled up a depot cost structure and reported the net without hiding the gross.",
             "hints":[
               "Roll budget and actuals separately to the root, then take the variance at the top.",
               "Variance is budget minus actual at every level — the sign tells you which way the money moved.",
               "Netting at the root is arithmetic, not absolution: the report carries the rolled figure and the account-level story that explains it."]}
            """),

        new("WC-PRG-028", "How finished is the ship?",
            "Four blocks, four percentages, one honest answer.",
            "Marine & Shipbuilding", "Progress Engineer", "project_controls", "foundation", 7,
            """["progress_measurement"]""",
            """
            {"context":"A survey vessel refit reports progress by block. Weight each block by its budget and compute the vessel's true physical progress for the owner's monthly certificate.",
             "evidence":[
               {"label":"Hull & structure","value":"weight 1,200,000 · 80% complete"},
               {"label":"Outfitting","value":"weight 900,000 · 35% complete"},
               {"label":"Systems & electrical","value":"weight 700,000 · 20% complete"},
               {"label":"Trials & handover","value":"weight 200,000 · 0% complete"}],
             "task":"progress","given":{"nodes":[
               {"id":"B1","name":"Hull & structure","weight":1200000,"percent":80},
               {"id":"B2","name":"Outfitting","weight":900000,"percent":35},
               {"id":"B3","name":"Systems & electrical","weight":700000,"percent":20},
               {"id":"B4","name":"Trials & handover","weight":200000,"percent":0}]},
             "ask":[
               {"key":"overall_percent","label":"Overall percent complete (budget-weighted)","type":"number"},
               {"key":"total_weight","label":"Total progress weight","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"certificate","prompt":"The yard manager wants the certificate to read 'over half done' because the hull looks nearly finished.",
                "options":[
                  {"key":"weighted","label":"Certify the weighted figure and walk the owner through the block table","quality":100,
                   "consequence":"The owner's payments track reality; the yard's credibility survives the outfitting phase.",
                   "principle":"A progress certificate is a financial instrument — it certifies measurement, not impressions."},
                  {"key":"round","label":"Round up — the difference is a few points","quality":10,
                   "consequence":"The 'few points' are the owner's money, advanced against work that does not exist yet.",
                   "principle":"Rounding a certificate is lending someone else's cash."},
                  {"key":"hull","label":"Report hull progress as the headline — it is the biggest block","quality":15,
                   "consequence":"The headline crashes in the systems phase, where every refit is actually won or lost.",
                   "principle":"The biggest block is not the whole ship."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Certified a ship refit's real weighted progress against a more flattering impression.",
             "hints":[
               "Weight each block's completion by its budget; the hull's visual finish carries only its budget share.",
               "Sum the weighted completions and divide by the total weight — and confirm the weights themselves sum to the declared total.",
               "Where the weighted figure and the visual impression disagree, the certificate follows the measured figure — the difference belongs in the narrative, not in the number."]}
            """),

        new("WC-AIA-029", "The optimistic algorithm",
            "The AI found a recovery trend. Verify it exists.",
            "Software", "AI Assurance Reviewer", "governed_ai", "advanced", 14,
            """["governed_ai","earned_value","forecasting"]""",
            """
            {"context":"An AI assistant reviewing an ERP data-migration reports: 'Schedule performance is recovering and cost remains stable; completion near 10,300,000 is supportable.' The measured cumulative data: PV 3400000, EV 3060000, AC 3300000, BAC 10200000. Compute the real indices and the composite forecast, then audit the claim.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"3,400,000"},
               {"label":"Earned Value (EV)","value":"3,060,000"},
               {"label":"Actual Cost (AC)","value":"3,300,000"},
               {"label":"Budget at Completion (BAC)","value":"10,200,000"},
               {"label":"AI assessment","value":"'schedule recovering, cost stable, completion near 10,300,000 supportable'"}],
             "task":"evm","given":{"pv":3400000,"ev":3060000,"ac":3300000,"bac":10200000},
             "ask":[
               {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
               {"key":"eac_composite","label":"EAC — composite method (AC + (BAC−EV) ÷ (CPI×SPI))","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"claim","prompt":"Which failure matters most in the AI's assessment?",
                "options":[
                  {"key":"unsupported","label":"'Recovering' and 'stable' are trend claims made from a single cumulative snapshot — no time series supports them","quality":100,
                   "consequence":"You reject the note for the right reason: not its number, its unsupported claims.",
                   "principle":"A trend claim requires a series; a snapshot supports only a position."},
                  {"key":"rounding","label":"The forecast is rounded too neatly","quality":10,
                   "consequence":"True and trivial; the assessment fails on evidence, not presentation.",
                   "principle":"Cosmetic critique lets substantive error through."},
                  {"key":"fine","label":"Nothing — the number is close to the budget, which is plausible","quality":0,
                   "consequence":"The composite forecast is materially above the AI's figure; plausibility just approved an error.",
                   "principle":"Plausible and verified are different words for a reason."}]},
               {"key":"disposition","prompt":"Disposition of the AI note?",
                "options":[
                  {"key":"correct","label":"Replace with the measured indices and composite EAC, attach the period series as required evidence for any future trend language","quality":100,
                   "consequence":"The note ships correct, and the assistant's next draft has a standard to meet.",
                   "principle":"Every correction is also a specification for the tool."},
                  {"key":"forward","label":"Forward with a caveat footnote","quality":15,
                   "consequence":"Nobody reads footnotes under a confident headline. The headline wins.",
                   "principle":"A caveat that must be read to matter, won't be."},
                  {"key":"suppress","label":"Suppress the note and say nothing upstream","quality":10,
                   "consequence":"The gap this month becomes a surprise next month, now with your fingerprints on the silence.",
                   "principle":"Assurance that hides its findings is complicity with better paperwork."}]}],
             "profile_map":{"calculation":"AI Assurance Reviewer","decision":"AI Assurance Reviewer","balanced":"AI Assurance Reviewer"},
             "share_line":"Caught an AI 'recovery trend' that no time series supported — before it reached the board.",
             "hints":[
               "Re-derive both indices from the cumulative data before reading any narrative about them.",
               "The composite forecast adds the remaining work — budget minus earned — divided by the product of both indices, onto actual cost.",
               "A schedule index below one and a cost index below one compound in the composite method; a claim of recovery and stability must survive that product to be supportable."]}
            """),

        new("WC-CAP-030", "The wind farm at the crossroads",
            "Eight periods in, everything is measurable and nothing is comfortable. Decide.",
            "Renewable Energy", "Programme Controls Director", "cross_functional", "expert", 18,
            """["earned_value","forecasting","governance","decision_analysis"]""",
            """
            {"context":"An offshore wind balance-of-plant programme reports eight periods of cumulative data against a 60,000,000 budget. The investment committee meets Monday to choose between recovery injection, descope, or continuing to plan. Compute the position and forecast, then make both calls a director must make.",
             "evidence":[
               {"label":"Period 1","value":"PV 4,000,000 · EV 3,600,000 · AC 4,200,000"},
               {"label":"Period 2","value":"PV 9,500,000 · EV 8,600,000 · AC 10,100,000"},
               {"label":"Period 3","value":"PV 16,000,000 · EV 14,500,000 · AC 17,200,000"},
               {"label":"Period 4","value":"PV 23,000,000 · EV 21,000,000 · AC 24,900,000"},
               {"label":"Period 5","value":"PV 31,000,000 · EV 28,200,000 · AC 33,500,000"},
               {"label":"Period 6","value":"PV 39,000,000 · EV 35,800,000 · AC 42,600,000"},
               {"label":"Period 7","value":"PV 47,000,000 · EV 43,600,000 · AC 51,500,000"},
               {"label":"Period 8","value":"PV 54,000,000 · EV 50,200,000 · AC 59,300,000"},
               {"label":"BAC","value":"60,000,000"}],
             "task":"timeline","given":{"bac":60000000,"series":[
               {"period":1,"pv":4000000,"ev":3600000,"ac":4200000},
               {"period":2,"pv":9500000,"ev":8600000,"ac":10100000},
               {"period":3,"pv":16000000,"ev":14500000,"ac":17200000},
               {"period":4,"pv":23000000,"ev":21000000,"ac":24900000},
               {"period":5,"pv":31000000,"ev":28200000,"ac":33500000},
               {"period":6,"pv":39000000,"ev":35800000,"ac":42600000},
               {"period":7,"pv":47000000,"ev":43600000,"ac":51500000},
               {"period":8,"pv":54000000,"ev":50200000,"ac":59300000}]},
             "ask":[
               {"key":"final_cpi","label":"Cumulative CPI at period 8","type":"number"},
               {"key":"final_spi","label":"Cumulative SPI at period 8","type":"number"},
               {"key":"final_eac","label":"EAC on measured performance (BAC ÷ CPI)","type":"number"},
               {"key":"vac","label":"Variance at Completion (BAC − EAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"posture","prompt":"Monday's recommendation to the investment committee:",
                "options":[
                  {"key":"recover","label":"A funded recovery case: the measured EAC as the base, a targeted injection on the two worst packages, and a descope option priced beside it","quality":100,
                   "consequence":"The committee funds recovery against a believable base — because you gave them a real alternative to reject.",
                   "principle":"A recovery case earns trust when the descope option beside it is real."},
                  {"key":"steady","label":"Continue to plan — eight periods is too early to abandon the budget","quality":5,
                   "consequence":"Eight consecutive periods of one-directional variance were the definition of 'not too early'.",
                   "principle":"The budget is a target; the trend is the evidence. Plan to evidence."},
                  {"key":"slash","label":"Recommend maximum descope immediately to protect the number","quality":30,
                   "consequence":"The number survives; the wind farm loses the export capacity that justified it.",
                   "principle":"Protecting the budget by deleting the benefit is failure with better optics."}]},
               {"key":"message","prompt":"The chair asks for one sentence the board can repeat to investors.",
                "options":[
                  {"key":"honest","label":"'Costs are running measurably above plan; a funded recovery is in execution and the revised completion estimate is stated with its assumptions.'","quality":100,
                   "consequence":"Investors hear control, not comfort — the distinction that keeps capital in the project.",
                   "principle":"Markets forgive measured bad news; they never forgive discovered bad news."},
                  {"key":"soft","label":"'The programme is experiencing headwinds but the team is confident in the plan.'","quality":10,
                   "consequence":"The sentence survives one earnings call; the correction that follows costs more than candour would have.",
                   "principle":"Confidence language without numbers is a promissory note against your credibility."},
                  {"key":"defer","label":"'We will update the market when the picture stabilises.'","quality":15,
                   "consequence":"Silence is a number too — the market fills it with a worse one.",
                   "principle":"In the absence of your estimate, everyone uses their own."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Strategic Project Controller"},
             "share_line":"Took an offshore wind programme's eight-period trend to the investment committee with a funded recovery case.",
             "hints":[
               "Establish the measured position first: cumulative indices at the data date, the forecast on measured performance, and the variance it implies.",
               "Divide the budget by cost performance for the forecast; the variance at completion is what the committee is actually deciding about.",
               "The recommendation follows the arithmetic: a variance beyond held contingency is a decision for the committee, not a footnote under it."]}
            """),

        // ───────────────────────── Gate A additions (031–050) ─────────────────────────
        // Twenty flagship challenges taking the bank to 50 (EXPANSION_GOVERNANCE §3, Gate A).
        // Every engine is now exercised at least twice, every difficulty band is populated, and the
        // industry spread widens to cover the sectors project controls actually works in. All data
        // is synthetic. Each one reference-solves in CI before it can ship.

        new("WC-EVM-031", "The data hall that ran out of runway",
            "A hyperscale fit-out is spending faster than it is earning. Somebody has to say by how much.",
            "Data Centres", "Project Controls Manager", "project_controls", "advanced", 11,
            """["earned_value","forecasting","cost_control"]""",
            """
            {"context":"Hall 3 of a hyperscale campus is at the halfway review. The contractor says the overspend is 'timing'. The client's finance director wants the two numbers that decide whether the contingency is enough: how far cost performance has slipped, and what efficiency the remaining work would have to achieve to still land on the budget.",
             "evidence":[
               {"label":"Planned value to date (PV)","value":"4,200,000"},
               {"label":"Earned value to date (EV)","value":"3,600,000"},
               {"label":"Actual cost to date (AC)","value":"4,050,000"},
               {"label":"Budget at completion (BAC)","value":"9,000,000"},
               {"label":"Contingency remaining","value":"620,000"}],
             "task":"evm","given":{"pv":4200000,"ev":3600000,"ac":4050000,"bac":9000000},
             "ask":[
               {"key":"cpi","label":"Cost Performance Index","type":"number"},
               {"key":"tcpi","label":"To-Complete Performance Index against the BAC","type":"number"},
               {"key":"vac","label":"Variance at completion (BAC minus the CPI-based EAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"contingency","prompt":"The forecast overrun is larger than the contingency left. What goes in the report?",
                "options":[
                  {"key":"state","label":"State the forecast overrun and that contingency no longer covers it, with the TCPI as the test of any recovery claim","quality":100,
                   "consequence":"The client funds a recovery while there is still work left to recover with.",
                   "principle":"A forecast that arrives while options still exist is worth more than a precise one that arrives after."},
                  {"key":"timing","label":"Report the contractor's 'timing' explanation and revisit next month","quality":5,
                   "consequence":"Next month the same trend is a month more expensive and the explanation is unchanged.",
                   "principle":"'Timing' is a hypothesis; an index is a measurement. Do not report the hypothesis as the measurement."},
                  {"key":"average","label":"Average the last three months so the trend looks less severe","quality":0,
                   "consequence":"The smoothing works exactly until the money runs out, which it does on schedule.",
                   "principle":"Smoothing a trend does not smooth the cash."}]},
               {"key":"recovery","prompt":"The contractor proposes to hit the BAC by improving efficiency on the remaining scope.",
                "options":[
                  {"key":"test","label":"Test the claim against the TCPI — ask what specific change delivers that efficiency, given they have never achieved it","quality":100,
                   "consequence":"The conversation moves from optimism to a named, costed intervention.",
                   "principle":"A recovery plan that requires unprecedented performance is a wish with a schedule attached."},
                  {"key":"accept","label":"Accept the commitment and hold them to it contractually","quality":25,
                   "consequence":"The commitment is met with a claim rather than with productivity.",
                   "principle":"Contracts allocate the cost of failure; they do not prevent it."},
                  {"key":"cut","label":"Instruct an immediate scope cut to close the gap","quality":40,
                   "consequence":"Scope leaves the project before anyone has established which scope the client can do without.",
                   "principle":"Cutting before diagnosing trades a cost problem for a capability problem."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Strategic Project Controller"},
             "share_line":"Told a hyperscale client the truth about a half-built data hall: what it will cost, and what a real recovery would have to look like.",
             "hints":[
               "Two efficiencies answer the director: the one achieved so far, and the one the remaining work must achieve to land on budget.",
               "The to-complete index divides work remaining by funds remaining — budget minus earned, over budget minus actual.",
               "Compare the two indices: when the required efficiency sits far above the achieved one, timing is not an explanation, it is the gap."]}
            """),

        new("WC-CPM-032", "Cold chain, hot deadline",
            "A vaccine rollout has one network and no room for a wrong critical path.",
            "Pharmaceuticals", "Planning Engineer", "project_controls", "professional", 10,
            """["critical_path","float_management","schedule_analysis"]""",
            """
            {"context":"A national cold-chain rollout must be operational before the winter programme. The logic below is agreed; the durations are the current estimates. Before the team accelerates anything, establish what the schedule actually says.",
             "evidence":[
               {"label":"A — Site surveys (no predecessors)","value":"5 days"},
               {"label":"B — Freezer procurement (after A)","value":"8 days"},
               {"label":"C — Power upgrade design (after A)","value":"6 days"},
               {"label":"D — Freezer install (after B)","value":"4 days"},
               {"label":"E — Power works (after C)","value":"9 days"},
               {"label":"F — Commissioning (after D and E)","value":"3 days"}],
             "task":"cpm","given":{"activities":[
               {"id":"A","dur":5,"preds":[]},
               {"id":"B","dur":8,"preds":["A"]},
               {"id":"C","dur":6,"preds":["A"]},
               {"id":"D","dur":4,"preds":["B"]},
               {"id":"E","dur":9,"preds":["C"]},
               {"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Project duration (days)","type":"number"},
               {"key":"critical_path","label":"Critical path (activity ids, comma-separated)","type":"set"},
               {"key":"float_B","label":"Total float on B (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"crash","prompt":"The programme director wants two days off the finish and offers overtime on the freezer procurement, because it is the biggest single cost.",
                "options":[
                  {"key":"path","label":"Spend the overtime on the critical path instead, and show why accelerating B buys nothing","quality":100,
                   "consequence":"The two days arrive because the money went where the schedule was actually constrained.",
                   "principle":"Acceleration off the critical path buys float you already had."},
                  {"key":"comply","label":"Accelerate B as asked — it is the director's call","quality":10,
                   "consequence":"Freezers arrive early and wait in a car park for power that is not ready.",
                   "principle":"Doing the wrong analysis politely is still doing the wrong analysis."},
                  {"key":"both","label":"Accelerate both paths to be safe","quality":45,
                   "consequence":"The finish moves, at roughly twice the necessary cost.",
                   "principle":"Insurance you did not need is still a payment you made."}]},
               {"key":"float","prompt":"The freezer supplier asks to use the float on B for a cheaper shipping route.",
                "options":[
                  {"key":"trade","label":"Trade it explicitly — record who now owns the float and what the schedule looks like without it","quality":100,
                   "consequence":"The saving is banked and everyone knows the buffer is spent.",
                   "principle":"Float is a project asset. Give it away deliberately or lose it accidentally."},
                  {"key":"free","label":"Agree — the float is spare capacity","quality":20,
                   "consequence":"When power works slip, the buffer that would have absorbed it is on a ship.",
                   "principle":"'Spare' float is the float you have not yet needed."},
                  {"key":"refuse","label":"Refuse to release any float","quality":50,
                   "consequence":"A real saving is declined to protect a buffer nothing is threatening.",
                   "principle":"Protecting float unconditionally is hoarding, not control."}]}],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Strategic Project Controller"},
             "share_line":"Found the real critical path on a national cold-chain rollout before anyone spent money accelerating the wrong activity.",
             "hints":[
               "Run both passes before touching the plan — the network, not seniority, names the driving chain.",
               "The critical path is the zero-float chain; read the named activity's float directly from the passes.",
               "Overtime on an activity with float shortens nothing; check where the offered acceleration sits before spending it."]}
            """),

        new("WC-RSK-033", "The tie-in nobody priced",
            "A subsea campaign's risk register, valued honestly for the first time.",
            "Subsea Engineering", "Risk Analyst", "project_controls", "professional", 9,
            """["risk_quantification","expected_monetary_value","contingency"]""",
            """
            {"context":"A subsea tie-in campaign has three live register entries and a contingency line that was set by precedent rather than analysis. Value the register so the contingency conversation has a number in it. Impacts are signed: threats are negative, opportunities positive.",
             "evidence":[
               {"label":"R1 — Weather window closes early","value":"probability 30%, impact -450,000"},
               {"label":"R2 — Vessel mobilisation slips past the campaign","value":"probability 15%, impact -1,200,000"},
               {"label":"R3 — Adjacent operator shares survey data","value":"probability 50%, impact +200,000"},
               {"label":"Contingency currently held","value":"300,000"}],
             "task":"risk","given":{"risks":[
               {"id":"R1","probability":0.30,"impact":-450000},
               {"id":"R2","probability":0.15,"impact":-1200000},
               {"id":"R3","probability":0.50,"impact":200000}]},
             "ask":[
               {"key":"emv","label":"Expected monetary value of the register","type":"number"},
               {"key":"emv_R2","label":"Expected monetary value of R2 alone","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"contingency","prompt":"The register's expected value exceeds the contingency held. What do you recommend?",
                "options":[
                  {"key":"raise","label":"Present the gap and the two entries that drive it, and ask for contingency to be set against the analysis","quality":100,
                   "consequence":"Contingency stops being a habit and starts being a decision with reasoning attached.",
                   "principle":"Contingency set by precedent protects the precedent, not the project."},
                  {"key":"opportunity","label":"Net the opportunity against the threats and report the contingency as adequate","quality":15,
                   "consequence":"An opportunity that requires another company's goodwill is spent before it is earned.",
                   "principle":"Never fund a threat with an opportunity you do not control."},
                  {"key":"silent","label":"Report the register unchanged — EMV is a modelling artefact, not a budget","quality":25,
                   "consequence":"The board is surprised by an exposure that was quantified and then not mentioned.",
                   "principle":"An analysis you decline to act on still has to be disclosed."}]},
               {"key":"single","prompt":"R2 alone is the largest single exposure. The vessel broker offers a paid option to hold the slot.",
                "options":[
                  {"key":"compare","label":"Compare the option's cost against R2's expected value and the cost of the delay it prevents","quality":100,
                   "consequence":"The decision is made on the arithmetic rather than on how the risk feels.",
                   "principle":"Buy a risk down when the price is below the exposure — and be able to show it."},
                  {"key":"buy","label":"Take the option; vessel slips are notorious","quality":45,
                   "consequence":"Probably a good outcome, arrived at with no reasoning anyone can audit or repeat.",
                   "principle":"A right answer without a reason cannot be relied on twice."},
                  {"key":"decline","label":"Decline — the probability is low","quality":20,
                   "consequence":"A low-probability, high-impact event is precisely the kind you insure against.",
                   "principle":"Probability alone never decides; exposure does."}]}],
             "profile_map":{"calculation":"Risk Quantifier","decision":"Evidence-Based Decision Maker","balanced":"Strategic Project Controller"},
             "share_line":"Put a defensible number on a subsea risk register and showed why the contingency behind it was set by habit.",
             "hints":[
               "Signed impacts: threats negative, opportunities positive — probability times impact per line.",
               "Net the lines for the register value; read the single line asked for before netting.",
               "Precedent is not analysis: compare the register's expected value with the held contingency and let the difference drive the recommendation."]}
            """),

        new("WC-CSH-034", "The month the money ran out",
            "A development that is profitable on paper and insolvent in March.",
            "Property Development", "Commercial Manager", "project_finance", "developing", 8,
            """["cash_flow","funding","working_capital"]""",
            """
            {"context":"A mixed-use development is profitable across its life and still needs a facility to survive the middle of it. The four-period plan below is agreed. Work out where the project ends up and how much funding it needs at its worst point.",
             "evidence":[
               {"label":"Period 1 — receipts / payments","value":"0 in, 800,000 out"},
               {"label":"Period 2 — receipts / payments","value":"1,500,000 in, 1,100,000 out"},
               {"label":"Period 3 — receipts / payments","value":"0 in, 950,000 out"},
               {"label":"Period 4 — receipts / payments","value":"2,600,000 in, 700,000 out"},
               {"label":"Facility currently arranged","value":"1,000,000"}],
             "task":"cashflow","given":{"periods":[
               {"period":1,"inflow":0,"outflow":800000},
               {"period":2,"inflow":1500000,"outflow":1100000},
               {"period":3,"inflow":0,"outflow":950000},
               {"period":4,"inflow":2600000,"outflow":700000}]},
             "ask":[
               {"key":"final_position","label":"Closing cash position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement (the deepest cumulative shortfall)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"facility","prompt":"The arranged facility is smaller than the peak requirement. The finance director points out the project is profitable overall.",
                "options":[
                  {"key":"extend","label":"Show that profitability and liquidity are different questions, and ask for the facility to cover the peak","quality":100,
                   "consequence":"The facility is extended before the month it is needed, at ordinary rates.",
                   "principle":"Projects are not killed by their final position; they are killed by their worst one."},
                  {"key":"profit","label":"Rely on the closing position — the money arrives in the end","quality":0,
                   "consequence":"Period 3 arrives first, and it does not care about period 4.",
                   "principle":"Cash is a sequence, not a total."},
                  {"key":"delay","label":"Delay period 3 payments to fit inside the facility","quality":35,
                   "consequence":"The shortfall is pushed onto subcontractors, who price it back in next time.",
                   "principle":"Deferring payment is borrowing from your supply chain at an undisclosed rate."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Commercial Realist","balanced":"Strategic Project Controller"},
             "share_line":"Found the month a profitable development would have run out of cash, and sized the facility before it did.",
             "hints":[
               "Build the cumulative line period by period from the net movements.",
               "The peak funding requirement is the deepest cumulative shortfall — the worst point, not the last.",
               "Life-of-project profit and mid-life cash are separate truths; the facility must cover the worst cumulative point regardless of the happy ending."]}
            """),

        new("WC-BOQ-035", "Twelve kilometres of earth",
            "A rail earthworks bill, measured properly before it is priced.",
            "Rail", "Quantity Surveyor", "project_controls", "foundation", 6,
            """["measurement","bill_of_quantities","estimating"]""",
            """
            {"context":"An earthworks package on a rail alignment has been measured. Before the bill goes out to tender, confirm the total and how many priced lines it contains — a bill that does not add up is a claim waiting to be written.",
             "evidence":[
               {"label":"EX-01 Bulk excavation","value":"42,000 m3 at 18.50 per m3"},
               {"label":"FL-02 Engineered fill","value":"12,500 m3 at 33.75 per m3"},
               {"label":"DR-03 Trackside drainage","value":"3,200 m at 96.00 per m"}],
             "task":"boq","given":{"lines":[
               {"id":"EX-01","qty":42000,"rate":18.50},
               {"id":"FL-02","qty":12500,"rate":33.75},
               {"id":"DR-03","qty":3200,"rate":96.00}]},
             "ask":[
               {"key":"total","label":"Bill total","type":"number"},
               {"key":"line_count","label":"Number of priced lines","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"provisional","prompt":"The designer expects the fill quantity to grow but cannot say by how much yet.",
                "options":[
                  {"key":"provisional","label":"Keep the measured quantity and flag the line as provisional, with the remeasurement rule stated","quality":100,
                   "consequence":"The tender is comparable between bidders and the growth is priced at an agreed rate.",
                   "principle":"Uncertainty belongs in the mechanism, not in the number."},
                  {"key":"pad","label":"Add a margin to the fill quantity to be safe","quality":10,
                   "consequence":"Every bidder prices the padding, and the client buys it whether or not the fill appears.",
                   "principle":"A padded quantity is a real payment for an imaginary risk."},
                  {"key":"omit","label":"Leave the line out until the design settles","quality":5,
                   "consequence":"The work appears anyway, at a rate negotiated with no competitive tension.",
                   "principle":"Scope omitted from a bill is scope purchased at the contractor's price."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Commercial Realist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Measured a rail earthworks bill and put the uncertain quantity into a mechanism instead of into the number.",
             "hints":[
               "Extend each line — quantity times rate — and sum the extensions for the bill total.",
               "Count only priced lines; a line without a rate is not yet part of the bill's arithmetic.",
               "Where a quantity is expected to grow but cannot yet be stated, the discipline is a provisional quantity flagged as such — not a silent allowance inside another line."]}
            """),

        new("WC-PRD-036", "The lining crew that looked fast",
            "A tunnel crew is ahead on hours and behind on metres.",
            "Tunnelling", "Cost Engineer", "project_controls", "developing", 8,
            """["productivity","labour_control","performance_measurement"]""",
            """
            {"context":"A segmental lining crew reports that it is 'running well'. The hours booked and the metres installed are below. Establish the productivity factor and the hours position before the next crew is mobilised on the same assumption.",
             "evidence":[
               {"label":"Planned quantity","value":"1,800 metres of lining"},
               {"label":"Planned hours","value":"5,400 hours"},
               {"label":"Quantity actually installed (earned)","value":"1,260 metres"},
               {"label":"Hours actually booked","value":"4,320 hours"}],
             "task":"productivity","given":{"planned_qty":1800,"planned_hours":5400,"earned_qty":1260,"actual_hours":4320},
             "ask":[
               {"key":"factor","label":"Productivity factor (actual productivity divided by planned)","type":"number"},
               {"key":"hours_variance","label":"Hours variance (actual minus planned hours)","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"mobilise","prompt":"A second crew is due to mobilise on the same planned rate.",
                "options":[
                  {"key":"reprice","label":"Re-plan the second crew at the demonstrated rate and say what the whole-package hours become","quality":100,
                   "consequence":"The overrun is forecast once, early, instead of discovered twice.",
                   "principle":"The rate you have achieved predicts better than the rate you assumed."},
                  {"key":"same","label":"Mobilise on the original rate — the first crew was still learning","quality":30,
                   "consequence":"A learning-curve argument is reasonable and needs evidence; without it the same variance simply doubles.",
                   "principle":"A learning curve is a measurement, not a hope."},
                  {"key":"pressure","label":"Mobilise on the original rate and hold both crews to it","quality":10,
                   "consequence":"Pressure applied to an unachievable rate produces optimistic reporting, not metres.",
                   "principle":"Targets that the process cannot deliver corrupt the data before they change the output."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Strategic Project Controller"},
             "share_line":"Showed a tunnelling team that a crew booking fewer hours than planned was still losing ground per metre.",
             "hints":[
               "Compute planned and actual productivity as output per hour before believing any impression of pace.",
               "The factor divides actual productivity by planned; the hours variance subtracts planned hours from booked hours.",
               "A crew can look fast and still run behind plan — mobilising a second crew on an unproven rate doubles the assumption, not the output."]}
            """),

        new("WC-RES-037", "The shutdown that needed two of everyone",
            "A refinery turnaround where the histogram says what the plan will not.",
            "Refining", "Resource Planner", "project_controls", "professional", 9,
            """["resource_management","levelling","capacity_planning"]""",
            """
            {"context":"A refinery turnaround has been scheduled to the shortest possible duration. The resource histogram below compares the plan's demand for qualified pipefitters against the crews actually available. Quantify the overload before the shutdown window opens.",
             "evidence":[
               {"label":"Week 1 — demand / available","value":"120 / 150"},
               {"label":"Week 2 — demand / available","value":"210 / 150"},
               {"label":"Week 3 — demand / available","value":"265 / 150"},
               {"label":"Week 4 — demand / available","value":"140 / 150"}],
             "task":"resource","given":{"periods":[
               {"period":1,"demand":120,"capacity":150},
               {"period":2,"demand":210,"capacity":150},
               {"period":3,"demand":265,"capacity":150},
               {"period":4,"demand":140,"capacity":150}]},
             "ask":[
               {"key":"peak_overload","label":"Peak overload in a single week (people)","type":"number"},
               {"key":"total_overdemand","label":"Total unfulfilled demand across the shutdown (person-weeks)","type":"number"},
               {"key":"overloaded_periods","label":"Number of overloaded weeks","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"plan","prompt":"The schedule is only achievable with resource the site does not have. The turnaround manager wants to keep the dates.",
                "options":[
                  {"key":"level","label":"Level the schedule to available capacity and state the honest duration, alongside the cost of hiring to the peak","quality":100,
                   "consequence":"The business chooses between a longer shutdown and a bigger crew, with both prices visible.",
                   "principle":"A schedule that assumes resource you do not have is a wish list with dates."},
                  {"key":"keep","label":"Keep the dates and manage the shortfall week by week","quality":5,
                   "consequence":"Week 3 arrives, the crews do not, and the recovery costs more than either option would have.",
                   "principle":"An overload you have quantified and not resolved is a decision to fail on schedule."},
                  {"key":"overtime","label":"Cover the peak with overtime on the existing crews","quality":40,
                   "consequence":"Plausible for a small peak; at this size it buys fatigue and rework in a safety-critical environment.",
                   "principle":"Overtime is a buffer with a declining exchange rate."}]}],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Delivery Realist","balanced":"Strategic Project Controller"},
             "share_line":"Quantified the crew shortfall in a refinery turnaround before the shutdown window opened, not during it.",
             "hints":[
               "Overload is demand minus capacity per week, floored at zero — weeks with spare capacity contribute nothing.",
               "The total unfulfilled demand sums the weekly overloads; the count is how many weeks are overloaded at all.",
               "A plan that needs people who do not exist is a duration statement in disguise — quantify the gap first, then talk about dates."]}
            """),

        new("WC-CBS-038", "Where the ward money went",
            "A hospital wing's cost accounts, rolled up and told straight.",
            "Healthcare", "Cost Controller", "project_controls", "developing", 8,
            """["cost_breakdown","variance_analysis","cost_control"]""",
            """
            {"context":"A hospital wing has three cost accounts reporting to the project. The monthly pack shows each account separately and never rolls them up, so nobody has stated the project position. Do it.",
             "evidence":[
               {"label":"1.1 Structure — budget / actual","value":"1,200,000 / 1,310,000"},
               {"label":"1.2 Mechanical and electrical — budget / actual","value":"900,000 / 820,000"},
               {"label":"1.3 Medical fit-out — budget / actual","value":"400,000 / 465,000"}],
             "task":"cbs","given":{"nodes":[
               {"id":"1","parent":null},
               {"id":"1.1","parent":"1","budget":1200000,"actual":1310000},
               {"id":"1.2","parent":"1","budget":900000,"actual":820000},
               {"id":"1.3","parent":"1","budget":400000,"actual":465000}]},
             "ask":[
               {"key":"root_budget","label":"Total project budget","type":"number"},
               {"key":"root_actual","label":"Total actual cost","type":"number"},
               {"key":"root_variance","label":"Project variance (budget minus actual)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"offset","prompt":"The mechanical account is underspent. The project manager wants to use it to absorb the fit-out overrun and report the project as broadly on budget.",
                "options":[
                  {"key":"report","label":"Report the project variance AND the account-level movements, so the offset is visible rather than implied","quality":100,
                   "consequence":"The net is honest and the two real problems keep their names.",
                   "principle":"A net position hides two facts unless you publish the gross alongside it."},
                  {"key":"net","label":"Report the net position — it is the number the board asked for","quality":20,
                   "consequence":"The board learns about the fit-out overrun when the underspend runs out.",
                   "principle":"Offsetting is a presentation choice that quietly becomes a forecasting error."},
                  {"key":"transfer","label":"Transfer the budget formally between accounts, then report on budget","quality":45,
                   "consequence":"Legitimate if the transfer is approved and disclosed; corrosive if it is used to make variances disappear.",
                   "principle":"Moving budget is a decision that needs an owner, not a formatting step."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rolled up a hospital wing's cost accounts and refused to let an underspend hide an overrun.",
             "hints":[
               "Roll each account's budget and actual to the project root before stating any position.",
               "The project variance is rolled budget minus rolled actual — signed, not absolute.",
               "An underspend can lawfully absorb an overrun only in the roll-up; the pack still shows each account, because the offset is the story."]}
            """),

        new("WC-PRT-039", "The launch window will not wait",
            "Three-point estimates, and the probability nobody had calculated.",
            "Space Systems", "Schedule Risk Analyst", "project_controls", "professional", 10,
            """["three_point_estimating","schedule_risk","probability"]""",
            """
            {"context":"An instrument integration campaign has three sequential activities, each estimated optimistic / most likely / pessimistic. The launch window closes 38 working days from now. The programme board has been told the plan 'fits'. Establish what the estimates actually imply.",
             "evidence":[
               {"label":"Integration — O / M / P","value":"8 / 10 / 18 days"},
               {"label":"Environmental test — O / M / P","value":"5 / 6 / 13 days"},
               {"label":"Verification and shipping — O / M / P","value":"12 / 15 / 24 days"},
               {"label":"Working days until the launch window closes","value":"38"}],
             "task":"pert","given":{"activities":[
               {"o":8,"m":10,"p":18},
               {"o":5,"m":6,"p":13},
               {"o":12,"m":15,"p":24}],"deadline":38},
             "ask":[
               {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
               {"key":"std_dev","label":"Standard deviation of the path (days)","type":"number"},
               {"key":"prob_on_time","label":"Probability of meeting the window (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"report","prompt":"How should the probability be presented to a board that has only ever seen a single-date plan?",
                "options":[
                  {"key":"range","label":"Give the expected date, the spread, and the probability — and say which activity's uncertainty dominates it","quality":100,
                   "consequence":"The board can decide whether to buy certainty, and knows exactly where to spend to get it.",
                   "principle":"A single date is a probability statement with the probability deleted."},
                  {"key":"point","label":"Report the expected duration as the plan date","quality":20,
                   "consequence":"The expected duration is met roughly half the time, which is not what 'the plan date' means to a board.",
                   "principle":"An expected value is the middle of the outcomes, not a commitment."},
                  {"key":"pessimistic","label":"Plan to the pessimistic sum so the date is safe","quality":25,
                   "consequence":"The programme buys a date nobody believes, and the padding is spent because it exists.",
                   "principle":"Summing pessimistic estimates produces a date with a probability nobody calculated either."}]},
               {"key":"reduce","prompt":"There is budget to reduce uncertainty on exactly one activity.",
                "options":[
                  {"key":"widest","label":"Spend it on the activity contributing the most variance, and show the effect on the probability","quality":100,
                   "consequence":"The spread narrows where it is widest and the probability moves measurably.",
                   "principle":"Uncertainty reduction pays where the variance is, not where the duration is."},
                  {"key":"longest","label":"Spend it on the longest activity","quality":35,
                   "consequence":"Sometimes the same answer, often not — length and uncertainty are different properties.",
                   "principle":"The biggest number is not automatically the biggest risk."},
                  {"key":"spread","label":"Spread the budget across all three","quality":25,
                   "consequence":"Three small reductions move the total spread very little.",
                   "principle":"Variance adds in squares; treating it evenly wastes most of the effort."}]}],
             "profile_map":{"calculation":"Risk Quantifier","decision":"Executive Communicator","balanced":"Strategic Project Controller"},
             "share_line":"Turned three-point estimates into a launch-window probability, and named the activity whose uncertainty owned it.",
             "hints":[
               "Expected duration per activity weights the most likely four to one against each extreme, divided by six; sum along the path.",
               "The path deviation is the square root of the summed squared sixth-spans.",
               "Present the window as a probability read from the normal curve — a single date without its probability is the claim the board has been living on."]}
            """),

        new("WC-CHG-040", "Four changes and a baseline",
            "A water utility's change log, applied correctly for once.",
            "Water Utilities", "Change Manager", "project_controls", "developing", 7,
            """["change_control","baseline_management","configuration"]""",
            """
            {"context":"A treatment works upgrade has four change requests at various stages. The monthly report has been adding all of them to the forecast, approved or not. Apply the change log properly and state the revised baseline.",
             "evidence":[
               {"label":"Baseline budget","value":"6,400,000"},
               {"label":"Baseline duration","value":"240 days"},
               {"label":"CR-01 Additional inlet screening — APPROVED","value":"+180,000, +10 days"},
               {"label":"CR-02 Upgraded SCADA package — REJECTED","value":"+500,000, +30 days"},
               {"label":"CR-03 Deleted spare pump — APPROVED","value":"-60,000, 0 days"},
               {"label":"CR-04 Extended commissioning — PENDING","value":"+250,000, +15 days"}],
             "task":"change","given":{"baseline_bac":6400000,"baseline_duration":240,"changes":[
               {"id":"CR-01","status":"approved","cost_delta":180000,"schedule_delta":10},
               {"id":"CR-02","status":"rejected","cost_delta":500000,"schedule_delta":30},
               {"id":"CR-03","status":"approved","cost_delta":-60000,"schedule_delta":0},
               {"id":"CR-04","status":"pending","cost_delta":250000,"schedule_delta":15}]},
             "ask":[
               {"key":"revised_bac","label":"Revised budget at completion","type":"number"},
               {"key":"approved_cost_delta","label":"Net approved cost change","type":"number"},
               {"key":"approved_count","label":"Number of approved changes","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"pending","prompt":"CR-04 is very likely to be approved. Where does it belong this month?",
                "options":[
                  {"key":"separate","label":"Keep the baseline to approved changes only, and show pending changes as a separately labelled exposure","quality":100,
                   "consequence":"The baseline stays a control reference and the board still sees what is coming.",
                   "principle":"A baseline that moves with expectation cannot measure anything."},
                  {"key":"include","label":"Include it — it is going to be approved anyway","quality":10,
                   "consequence":"The month it is rejected, every variance in the report is wrong and nobody can say by how much.",
                   "principle":"'Certain to be approved' is a forecast; the baseline records decisions."},
                  {"key":"ignore","label":"Leave it out entirely until it is decided","quality":40,
                   "consequence":"Technically correct on the baseline, and it lets a known exposure surprise people.",
                   "principle":"Excluding something from the baseline is not a reason to exclude it from the report."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rebuilt a utility's baseline from its change log and put pending changes where they belong — visible, but not in the baseline.",
             "hints":[
               "The baseline moves on approval and at no other moment — forecast and baseline are different documents.",
               "Net the approved items' signed deltas into the budget; count them as you go.",
               "A change very likely to be approved belongs in the forecast narrative and the risk view this month — the baseline waits for the signature."]}
            """),

        new("WC-ESC-041", "The fab that was behind in time, not money",
            "Earned schedule on a semiconductor build, where the SPI had stopped meaning anything.",
            "Semiconductors", "Project Controls Lead", "project_controls", "advanced", 11,
            """["earned_schedule","schedule_performance","forecasting"]""",
            """
            {"context":"A fab tool-install programme is at the end of period five of a planned eight. The classic schedule performance index has been drifting back towards one as the plan tails off, which the team is reading as recovery. Use earned schedule to say where the programme actually is in time.",
             "evidence":[
               {"label":"Cumulative planned value by period","value":"P1 400,000 · P2 900,000 · P3 1,500,000 · P4 2,200,000"},
               {"label":"Cumulative planned value (continued)","value":"P5 3,000,000 · P6 3,900,000 · P7 4,800,000 · P8 5,600,000"},
               {"label":"Earned value at the data date","value":"2,600,000"},
               {"label":"Actual time elapsed","value":"period 5 of a planned 8"}],
             "task":"earned_schedule","given":{"plan":[
               {"period":1,"pv":400000},{"period":2,"pv":900000},{"period":3,"pv":1500000},{"period":4,"pv":2200000},
               {"period":5,"pv":3000000},{"period":6,"pv":3900000},{"period":7,"pv":4800000},{"period":8,"pv":5600000}],
               "ev":2600000,"at":5,"planned_duration":8},
             "ask":[
               {"key":"es","label":"Earned schedule (periods)","type":"number"},
               {"key":"spi_time","label":"Time-based schedule performance index","type":"number"},
               {"key":"eac_time","label":"Time estimate at completion (periods)","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"which","prompt":"The steering group has been shown the classic SPI all year. What do you present now?",
                "options":[
                  {"key":"explain","label":"Show both, explain why the classic index converges to one at completion regardless of lateness, and forecast in time","quality":100,
                   "consequence":"The group understands the previous reports were not wrong so much as structurally optimistic near the end.",
                   "principle":"A cost-denominated index cannot measure time once the plan runs out of planned value."},
                  {"key":"switch","label":"Switch to the time-based index without comment","quality":30,
                   "consequence":"The numbers change between reports with no explanation, and the credibility of both is damaged.",
                   "principle":"Changing a measure silently costs more trust than the measure was worth."},
                  {"key":"keep","label":"Keep reporting the classic index for consistency","quality":5,
                   "consequence":"Consistency is maintained all the way to a completion date nobody forecast.",
                   "principle":"Consistent use of a measure that cannot answer the question is consistent failure."}]}],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Executive Communicator","balanced":"Strategic Project Controller"},
             "share_line":"Used earned schedule to show a fab programme that a recovering SPI was arithmetic, not progress.",
             "hints":[
               "Map the earned value back onto the planned curve — the period where the plan expected it, plus the fraction, is the earned schedule.",
               "The time-based index divides earned schedule by elapsed periods; unlike the classic index, it does not drift back toward one as the plan tails off.",
               "The time estimate at completion scales the planned length by the inverse of the time index — present it beside the classic index and let the contrast do the talking."]}
            """),

        new("WC-TML-042", "Six periods, one bad month",
            "An automotive line installation, replayed period by period.",
            "Automotive", "Project Controls Analyst", "project_controls", "advanced", 11,
            """["earned_value","trend_analysis","forecasting"]""",
            """
            {"context":"A body-shop line installation has six periods of measured performance. The programme has been reported as 'consistently slightly behind' throughout. Replay the trend, find the worst period, and forecast the outturn.",
             "evidence":[
               {"label":"P1 — PV / EV / AC","value":"1,500,000 / 1,400,000 / 1,450,000"},
               {"label":"P2 — PV / EV / AC","value":"3,200,000 / 2,900,000 / 3,100,000"},
               {"label":"P3 — PV / EV / AC","value":"5,000,000 / 4,300,000 / 4,800,000"},
               {"label":"P4 — PV / EV / AC","value":"6,900,000 / 5,800,000 / 6,600,000"},
               {"label":"P5 — PV / EV / AC","value":"8,800,000 / 7,500,000 / 8,500,000"},
               {"label":"P6 — PV / EV / AC","value":"10,500,000 / 9,100,000 / 10,200,000"},
               {"label":"Budget at completion","value":"12,000,000"}],
             "task":"timeline","given":{"bac":12000000,"series":[
               {"period":1,"pv":1500000,"ev":1400000,"ac":1450000},
               {"period":2,"pv":3200000,"ev":2900000,"ac":3100000},
               {"period":3,"pv":5000000,"ev":4300000,"ac":4800000},
               {"period":4,"pv":6900000,"ev":5800000,"ac":6600000},
               {"period":5,"pv":8800000,"ev":7500000,"ac":8500000},
               {"period":6,"pv":10500000,"ev":9100000,"ac":10200000}]},
             "ask":[
               {"key":"final_spi","label":"Schedule performance index at the data date","type":"number"},
               {"key":"final_cpi","label":"Cost performance index at the data date","type":"number"},
               {"key":"worst_cpi_period","label":"Period with the worst cost performance","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"cause","prompt":"The worst period coincides with a robot-cell delivery slip that the team already considers closed.",
                "options":[
                  {"key":"verify","label":"Check whether performance recovered after that period or merely stopped deteriorating — and report which","quality":100,
                   "consequence":"The team learns the difference between an event that ended and its effect that did not.",
                   "principle":"A closed cause and a recovered trend are different findings."},
                  {"key":"attribute","label":"Attribute the whole variance to the delivery slip and close the item","quality":15,
                   "consequence":"A single visible cause absorbs a variance that four other periods also contributed to.",
                   "principle":"The most memorable cause is rarely the whole cause."},
                  {"key":"ignore","label":"Report the current indices without the period analysis","quality":30,
                   "consequence":"The numbers are right and the reader has no idea whether things are improving.",
                   "principle":"A position without a trend cannot support a decision about the future."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Strategic Project Controller"},
             "share_line":"Replayed six periods of an automotive line install and found the month that set the outturn.",
             "hints":[
               "The data-date indices come from the cumulative line; the worst period comes from reading each period's own performance.",
               "Compute period cost performance period by period — cumulative smoothing is exactly what hides one bad month.",
               "A closed issue with an open variance is not closed; tie the worst period to its cause and check whether the effect is still in the forecast."]}
            """),

        new("WC-PFO-043", "Three good projects, one budget",
            "A public portfolio where the biggest return is not the best choice.",
            "Government", "Portfolio Manager", "project_management", "expert", 12,
            """["portfolio_management","prioritisation","investment_appraisal"]""",
            """
            {"context":"A department can fund one programme this cycle. Each candidate has an appraised net present value, a delivery-risk score from 0 to 1, and a strategic-fit score from 0 to 1. The agreed weighting is 0.5 on normalised NPV, 0.3 on risk (as a penalty) and 0.2 on strategic fit. Score them and be ready to defend the ranking.",
             "evidence":[
               {"label":"P1 Regional flood defence — NPV / risk / fit","value":"12,000,000 / 0.6 / 0.8"},
               {"label":"P2 Schools decarbonisation — NPV / risk / fit","value":"8,000,000 / 0.2 / 0.9"},
               {"label":"P3 Motorway junction upgrade — NPV / risk / fit","value":"15,000,000 / 0.85 / 0.5"},
               {"label":"Agreed weights","value":"NPV 0.5, risk 0.3 (penalty), fit 0.2; NPV normalised against the largest"}],
             "task":"portfolio","given":{"w_npv":0.5,"w_risk":0.3,"w_fit":0.2,"projects":[
               {"id":"P1","npv":12000000,"risk":0.6,"fit":0.8},
               {"id":"P2","npv":8000000,"risk":0.2,"fit":0.9},
               {"id":"P3","npv":15000000,"risk":0.85,"fit":0.5}]},
             "ask":[
               {"key":"score_P2","label":"Weighted score for P2","type":"number"},
               {"key":"score_P3","label":"Weighted score for P3","type":"number"},
               {"key":"total_npv","label":"Total NPV of the candidate portfolio","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"defend","prompt":"The highest-NPV programme does not rank first. A minister asks why the department is turning down the biggest return.",
                "options":[
                  {"key":"model","label":"Show the weighting the department agreed, and that the ranking follows from delivery risk the appraisal already recorded","quality":100,
                   "consequence":"The conversation moves to whether the weights are right — which is the decision a minister should actually be making.",
                   "principle":"A transparent model turns an argument about answers into an argument about assumptions."},
                  {"key":"npv","label":"Re-rank on NPV alone; it is the objective measure","quality":10,
                   "consequence":"The department funds its riskiest delivery and discovers the risk score was objective too.",
                   "principle":"Choosing the measure that gives the desired answer is not objectivity."},
                  {"key":"reweight","label":"Adjust the weights until the biggest programme wins","quality":0,
                   "consequence":"The model becomes a justification engine, and the next ranking it produces convinces nobody.",
                   "principle":"A model you tune to the conclusion has stopped being evidence."}]},
               {"key":"risk","prompt":"The high-risk programme's sponsor argues the risk score is 'just a judgement'.",
                "options":[
                  {"key":"sensitivity","label":"Run the ranking across a range of risk scores and report at what value the answer changes","quality":100,
                   "consequence":"Everyone can see whether the decision is robust or genuinely balanced on a knife edge.",
                   "principle":"When an input is contested, publish the value at which the decision flips."},
                  {"key":"defend","label":"Defend the score as recorded","quality":40,
                   "consequence":"Defensible, but it leaves the sponsor's real question — how much it matters — unanswered.",
                   "principle":"Standing by a number is weaker than showing how much it moves the outcome."},
                  {"key":"drop","label":"Remove the risk weighting since it is subjective","quality":5,
                   "consequence":"Deliverability stops being a criterion for choosing what to deliver.",
                   "principle":"Difficult to measure is not the same as safe to ignore."}]}],
             "profile_map":{"calculation":"Portfolio Strategist","decision":"Governance Steward","balanced":"Strategic Project Controller"},
             "share_line":"Ranked a public portfolio on its agreed weights and defended turning down the biggest headline return.",
             "hints":[
               "Normalise the appraised values before weighting — the weights apply to comparable scales, and risk enters as a penalty.",
               "Score each programme with the agreed weights, and total the appraised value across all candidates for the portfolio figure.",
               "The biggest return can lawfully rank second once risk and fit are priced in — the defence is the agreed weighting, applied before the ranking was known."]}
            """),

        new("WC-DEC-044", "Haul road, three ways",
            "A mine access decision where the cheapest option is not the best score.",
            "Mining", "Project Manager", "project_management", "professional", 9,
            """["decision_analysis","trade_off","options_appraisal"]""",
            """
            {"context":"A mine expansion needs access to a new pit. Three options are on the table, each with a capital cost, a schedule impact in days, and a delivery-risk score from 0 to 1. The agreed weighting is 0.000001 per unit of cost, 0.02 per day of schedule and 1.0 on risk — chosen so a million of capital, fifty days and a full risk point weigh roughly the same. Lower weighted impact is better.",
             "evidence":[
               {"label":"Option A — new haul road: cost / days / risk","value":"4,200,000 / 45 / 0.6"},
               {"label":"Option B — upgrade the existing route: cost / days / risk","value":"3,100,000 / 90 / 0.35"},
               {"label":"Option C — conveyor and short road: cost / days / risk","value":"5,000,000 / 20 / 0.5"},
               {"label":"Agreed weights","value":"cost 0.000001 per unit, schedule 0.02 per day, risk 1.0"}],
             "task":"decision","given":{"w_cost":0.000001,"w_sched":0.02,"w_risk":1.0,"options":[
               {"id":"A","cost":4200000,"schedule":45,"risk":0.6},
               {"id":"B","cost":3100000,"schedule":90,"risk":0.35},
               {"id":"C","cost":5000000,"schedule":20,"risk":0.5}]},
             "ask":[
               {"key":"score_A","label":"Weighted impact score for option A","type":"number"},
               {"key":"score_B","label":"Weighted impact score for option B","type":"number"},
               {"key":"best_score","label":"Score of the best option","type":"number"}],
             "tolerance":0.001,
             "decisions":[
               {"key":"weights","prompt":"The winning option changes if schedule is weighted more heavily. The general manager has strong views about the ore date.",
                "options":[
                  {"key":"flip","label":"Report the ranking and the schedule weight at which it flips, then ask the GM to set the weight explicitly","quality":100,
                   "consequence":"The value judgement is made by the person entitled to make it, on the record.",
                   "principle":"Separate the arithmetic from the values, then let the right person own each."},
                  {"key":"guess","label":"Pick the weight you think reflects the GM's view and present a single answer","quality":15,
                   "consequence":"A value judgement is smuggled into an analysis and inherits its false authority.",
                   "principle":"Assumed preferences become invisible decisions."},
                  {"key":"equal","label":"Weight everything equally to stay neutral","quality":30,
                   "consequence":"Equal weighting is itself a strong assumption — that a day and a dollar matter alike.",
                   "principle":"There is no neutral weighting, only an unexamined one."}]}],
             "profile_map":{"calculation":"Portfolio Strategist","decision":"Evidence-Based Decision Maker","balanced":"Strategic Project Controller"},
             "share_line":"Scored three mine-access options and handed the value judgement back to the person who owned it.",
             "hints":[
               "This model scores impact: apply the agreed per-unit weights to each option's cost, schedule and risk, and sum.",
               "Lower weighted impact is better here — the best option carries the smallest score, not the largest.",
               "If a stakeholder wants a different winner, the honest lever is the weights, changed openly before rescoring — never the scores."]}
            """),

        new("WC-DQ-045", "The dashboard that was confidently wrong",
            "A logistics programme where the reporting data failed before the project did.",
            "Logistics", "Data Analyst", "governed_ai", "developing", 8,
            """["data_quality","assurance","reporting_integrity"]""",
            """
            {"context":"A distribution-network programme reports weekly throughput from an automated feed. Someone has noticed the dashboard disagrees with the depot. Six records were expected this week; the feed delivered what is below, against the planned value for each. Anything more than 5 units from plan counts as an anomaly worth investigating.",
             "evidence":[
               {"label":"Depot 1 — reported / planned","value":"98 / 100"},
               {"label":"Depot 2 — reported / planned","value":"104 / 100"},
               {"label":"Depot 3 — reported / planned","value":"no value received / 100"},
               {"label":"Depot 4 — reported / planned","value":"140 / 100"},
               {"label":"Depot 5 — reported / planned","value":"52 / 100"},
               {"label":"Depot 6 — reported / planned","value":"101 / 100"},
               {"label":"Anomaly threshold","value":"more than 5 units from plan"}],
             "task":"data_quality","given":{"threshold":5,"rows":[
               {"value":98,"expected":100},
               {"value":104,"expected":100},
               {"expected":100},
               {"value":140,"expected":100},
               {"value":52,"expected":100},
               {"value":101,"expected":100}]},
             "ask":[
               {"key":"anomaly_count","label":"Number of anomalous records","type":"number"},
               {"key":"completeness_pct","label":"Completeness of the feed (%)","type":"number"},
               {"key":"mean_abs_error","label":"Mean absolute error across the records received","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"publish","prompt":"The weekly pack is due in an hour. The feed is incomplete and two depots look wrong.",
                "options":[
                  {"key":"caveat","label":"Publish with the completeness figure and the anomalies flagged, and say what is being checked","quality":100,
                   "consequence":"Readers can weigh the numbers properly, and the correction lands as follow-through rather than as a retraction.",
                   "principle":"Report the quality of the data alongside the data, or the reader assumes it is perfect."},
                  {"key":"clean","label":"Drop the missing depot and the two outliers so the trend is readable","quality":0,
                   "consequence":"The cleanest week in the report is the one where the data was worst.",
                   "principle":"Silently removing inconvenient records is not cleaning; it is editing."},
                  {"key":"hold","label":"Hold the pack until the feed is fixed","quality":45,
                   "consequence":"Defensible once. As a habit it means the pack appears only when the news is good.",
                   "principle":"Delay is a legitimate choice you must be willing to make consistently."}]},
               {"key":"root","prompt":"One depot's value is far above plan and another far below, in the same week the feed dropped a record.",
                "options":[
                  {"key":"pipeline","label":"Investigate the pipeline first — correlated anomalies plus a missing record point at the feed, not at three depots","quality":100,
                   "consequence":"The integration fault is found before three depot managers are asked to explain numbers they never reported.",
                   "principle":"When several independent things fail together, suspect the thing they share."},
                  {"key":"depots","label":"Ask each depot to explain its own number","quality":30,
                   "consequence":"Three separate investigations eventually converge on one broken feed.",
                   "principle":"Investigating symptoms in parallel is slower than investigating the thing they have in common."},
                  {"key":"accept","label":"Accept the values — depots do vary","quality":5,
                   "consequence":"The variance is designed into next quarter's targets before anyone notices it was never real.",
                   "principle":"Bad data does not stay in the report; it becomes a plan."}]}],
             "profile_map":{"calculation":"Data Investigator","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a broken reporting feed before its numbers became next quarter's targets.",
             "hints":[
               "An anomaly is a received record whose gap from plan exceeds the agreed threshold; missing records are a completeness matter, not anomalies.",
               "Completeness compares received against expected as a percentage; the mean absolute error averages the absolute gaps across records actually received.",
               "Publish-or-hold is a data-quality decision: state completeness and the suspect depots alongside any figure you release."]}
            """),

        new("WC-PRC-046", "The valve that was late enough to matter",
            "A shipyard delay, measured against the float that was actually there.",
            "Shipbuilding", "Procurement Coordinator", "project_controls", "foundation", 6,
            """["procurement","float_management","delay_analysis"]""",
            """
            {"context":"A main engine control valve is late. The supplier is apologetic and the yard is anxious. Before anyone escalates, work out how much of the delay the schedule can absorb and what the completion date becomes.",
             "evidence":[
               {"label":"Current project duration","value":"300 days"},
               {"label":"Remaining float on the valve delivery path","value":"12 days"},
               {"label":"Supplier's notified delay","value":"27 days"}],
             "task":"procurement","given":{"project_duration":300,"remaining_float":12,"supplier_delay_days":27},
             "ask":[
               {"key":"critical_delay","label":"Delay that reaches the project completion date (days)","type":"number"},
               {"key":"new_project_duration","label":"Revised project duration (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"escalate","prompt":"The yard manager wants to issue a formal default notice immediately.",
                "options":[
                  {"key":"quantify","label":"Quantify the critical delay first, then escalate proportionately with the number attached","quality":100,
                   "consequence":"The supplier receives a specific, evidenced claim, which is the kind that gets settled.",
                   "principle":"Escalate with the measurement, not ahead of it."},
                  {"key":"notice","label":"Issue the notice on the full notified delay","quality":25,
                   "consequence":"The claim overstates the impact, the supplier disputes all of it, and the recoverable part is lost in the argument.",
                   "principle":"An overstated claim invites a defence of the whole."},
                  {"key":"absorb","label":"Absorb it quietly — the yard has coped with worse","quality":10,
                   "consequence":"The float is spent, no record exists, and the next delay starts from zero buffer with no history.",
                   "principle":"Undocumented absorption is a gift you cannot prove you gave."}]}],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Commercial Realist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Measured a shipyard delay against real float before anyone sent a letter about it.",
             "hints":[
               "The schedule absorbs the slip up to the float held by the receiving works; only the excess reaches completion.",
               "Critical delay is supplier slip minus available float, floored at zero; add only that to the baseline duration.",
               "Escalation should follow the measured damage — a default notice sized to the harmless part of a delay reads as temper, not control."]}
            """),

        new("WC-PRG-047", "How much of the campus is finished?",
            "Three work packages, three different answers, one honest number.",
            "Education Estates", "Project Controls Trainee", "project_controls", "foundation", 6,
            """["progress_measurement","weighting","reporting"]""",
            """
            {"context":"A university campus refurbishment reports progress by package. The steering group keeps hearing three numbers and wants one. Each package carries a weight reflecting its share of the work; each has a measured percentage complete.",
             "evidence":[
               {"label":"Package A — Enabling works","value":"weight 35, ninety per cent complete"},
               {"label":"Package B — Teaching block","value":"weight 40, fifty-five per cent complete"},
               {"label":"Package C — External works","value":"weight 25, twenty per cent complete"}],
             "task":"progress","given":{"nodes":[
               {"id":"A","weight":35,"percent":90},
               {"id":"B","weight":40,"percent":55},
               {"id":"C","weight":25,"percent":20}]},
             "ask":[
               {"key":"overall_percent","label":"Overall weighted progress (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"simple","prompt":"A steering-group member suggests simply averaging the three package percentages instead.",
                "options":[
                  {"key":"weighted","label":"Explain that a straight average treats the small external works as equal to the teaching block, and show both numbers once","quality":100,
                   "consequence":"The group sees the difference weighting makes and stops asking for the simpler version.",
                   "principle":"Unweighted progress measures packages, not projects."},
                  {"key":"average","label":"Use the straight average — it is easier to explain","quality":10,
                   "consequence":"The reported figure flatters the project whenever the small packages are the advanced ones.",
                   "principle":"Ease of explanation is not a reason to answer a different question."},
                  {"key":"both","label":"Report both numbers every month without comment","quality":35,
                   "consequence":"Two progress figures with no guidance invites each reader to pick their preferred one.",
                   "principle":"Publishing alternatives without a recommendation moves the judgement to the least-informed reader."}]}],
             "profile_map":{"calculation":"Progress Measurer","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave a university steering group one honest progress number instead of three convenient ones.",
             "hints":[
               "Weight each package's completion by its declared share — a simple average treats unequal packages as equals.",
               "Multiply, sum, and divide by the total weight; that single figure is the steering answer.",
               "If the simple average and the weighted figure differ, the difference is the size of the distortion the suggestion would print."]}
            """),

        new("WC-WBS-048", "The parent that did not add up",
            "A network rollout where a summary number outlived the detail beneath it.",
            "Telecommunications", "Project Controls Analyst", "project_controls", "developing", 7,
            """["scope_structuring","baseline_integrity","assurance"]""",
            """
            {"context":"A fibre rollout's work breakdown carries budgets at the leaves and, unusually, an explicit figure on one parent as well. That parent was budgeted early and never revisited when the detail was developed. Roll the structure up and test whether it still holds together.",
             "evidence":[
               {"label":"1.1 Survey and wayleaves (leaf)","value":"600,000"},
               {"label":"1.2 Network build (parent, with its own stated figure)","value":"1,000,000"},
               {"label":"1.2.1 Duct and chamber","value":"700,000"},
               {"label":"1.2.2 Cable and splice","value":"250,000"}],
             "task":"wbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"Fibre rollout"},
               {"id":"1.1","parent":"1","name":"Survey and wayleaves","value":600000},
               {"id":"1.2","parent":"1","name":"Network build","value":1000000},
               {"id":"1.2.1","parent":"1.2","name":"Duct and chamber","value":700000},
               {"id":"1.2.2","parent":"1.2","name":"Cable and splice","value":250000}]},
             "ask":[
               {"key":"root_total","label":"Total from the roll-up","type":"number"},
               {"key":"hundred_percent_valid","label":"Does every parent equal the sum beneath it? (yes/no)","type":"bool"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"gap","prompt":"The parent figure and the sum beneath it disagree. Which is the budget?",
                "options":[
                  {"key":"reconcile","label":"Treat the difference as unallocated scope and reconcile it before the baseline is set — either the detail is short or the parent was optimistic","quality":100,
                   "consequence":"The gap is resolved while it is still an estimating question rather than a variance.",
                   "principle":"A parent that disagrees with its children is telling you something is missing. Find out which."},
                  {"key":"parent","label":"Keep the parent figure; it was approved","quality":20,
                   "consequence":"The approved number survives and the work packages that must deliver it are underfunded.",
                   "principle":"Approval makes a number official, not achievable."},
                  {"key":"children","label":"Overwrite the parent with the roll-up and move on","quality":45,
                   "consequence":"The structure becomes consistent, and the reason for the difference is lost with it.",
                   "principle":"Reconciling by deletion removes the discrepancy and the information."}]}],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found a fibre rollout's summary budget that no longer matched the work beneath it — before it became a variance.",
             "hints":[
               "Roll from the leaves; a parent's stated figure is a claim to be tested against the sum beneath it.",
               "The structure holds only if every parent equals its children — one stale parent fails the whole test.",
               "When parent and children disagree, the children's sum is the budget — the parent figure is history that was never revisited."]}
            """),

        new("WC-EVM-049", "Three forecasts, one reactor",
            "A nuclear programme where the choice of EAC method is the whole argument.",
            "Nuclear", "Head of Project Controls", "project_controls", "expert", 13,
            """["forecasting","earned_value","assurance"]""",
            """
            {"context":"A nuclear island installation is a third of the way through its budget and behind on both cost and schedule. Three conventional estimate-at-completion methods are available and they disagree by a wide margin. The regulator's assurance panel wants to know which one the programme is using and why.",
             "evidence":[
               {"label":"Planned value to date (PV)","value":"22,000,000"},
               {"label":"Earned value to date (EV)","value":"19,800,000"},
               {"label":"Actual cost to date (AC)","value":"23,100,000"},
               {"label":"Budget at completion (BAC)","value":"60,000,000"},
               {"label":"Programme position","value":"behind plan on both cost and schedule"}],
             "task":"evm","given":{"pv":22000000,"ev":19800000,"ac":23100000,"bac":60000000},
             "ask":[
               {"key":"eac_budget","label":"EAC assuming remaining work runs at the budgeted rate","type":"number"},
               {"key":"eac_cpi","label":"EAC assuming current cost performance continues","type":"number"},
               {"key":"eac_composite","label":"EAC assuming both cost and schedule performance continue","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"method","prompt":"Which forecast does the programme adopt for the assurance submission?",
                "options":[
                  {"key":"range","label":"Present all three as a range, name the assumption behind each, and state which one the evidence currently supports","quality":100,
                   "consequence":"The panel sees a programme that understands its own uncertainty, and the chosen figure is defensible under challenge.",
                   "principle":"A forecast is a conditional statement. Publish the condition or you have published nothing."},
                  {"key":"budget","label":"Adopt the budgeted-rate forecast — the causes of the overrun are being addressed","quality":10,
                   "consequence":"The most optimistic method is selected at the moment there is least evidence for it.",
                   "principle":"'It will improve' is the assumption that needs the most evidence and usually gets the least."},
                  {"key":"composite","label":"Adopt the composite forecast to be conservative","quality":55,
                   "consequence":"Prudent, and still a single number presented without the assumption that generated it.",
                   "principle":"Conservatism chosen without reasoning is as arbitrary as optimism chosen without reasoning."}]},
               {"key":"panel","prompt":"A panel member asks what would have to be true for the most optimistic forecast to be correct.",
                "options":[
                  {"key":"answer","label":"Answer directly: the remaining work would have to run at the budgeted rate, which the programme has not achieved in any period so far","quality":100,
                   "consequence":"The honest answer costs one uncomfortable meeting and buys the panel's trust for the rest of the programme.",
                   "principle":"The strongest position in front of an assurance panel is knowing your own weakest assumption first."},
                  {"key":"deflect","label":"Note that forecasting at this stage is inherently uncertain","quality":15,
                   "consequence":"A true statement used to avoid a specific question, which the panel recognises immediately.",
                   "principle":"Generic uncertainty is not an answer to a specific challenge."},
                  {"key":"commit","label":"Commit to the optimistic figure and to a recovery plan that delivers it","quality":25,
                   "consequence":"The programme is now accountable for an outcome its own data does not support.",
                   "principle":"Do not convert a forecast you doubt into a commitment you must keep."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Governance Steward","balanced":"Strategic Project Controller"},
             "share_line":"Put three EAC methods in front of a nuclear assurance panel and named the assumption behind each.",
             "hints":[
               "Three forecasts, three assumptions: remaining work at the budgeted rate, at achieved cost performance, or at cost and schedule performance combined.",
               "Each starts from actual cost plus the remaining work — the divisor is what changes: nothing, the cost index, or the product of both indices.",
               "For an assurance submission, the honest answer states the chosen method's assumption and shows the spread — a single number without its assumption is advocacy."]}
            """),

        new("WC-CPM-050", "The terminal that had two ways to be late",
            "An airport expansion with a near-critical path nobody was watching.",
            "Airports", "Planning Manager", "project_controls", "expert", 12,
            """["critical_path","near_critical_analysis","schedule_risk"]""",
            """
            {"context":"A terminal expansion has the logic below. The programme has been managed around a single critical path for a year. Before the next acceleration decision, establish the duration, the true critical path, and how much protection the baggage-system route actually has.",
             "evidence":[
               {"label":"A — Enabling works (no predecessors)","value":"6 weeks"},
               {"label":"B — Pier substructure (after A)","value":"10 weeks"},
               {"label":"C — Terminal frame (after A)","value":"7 weeks"},
               {"label":"D — Pier fit-out (after B)","value":"5 weeks"},
               {"label":"E — Terminal envelope and MEP (after C)","value":"12 weeks"},
               {"label":"F — Systems integration (after D and E)","value":"4 weeks"},
               {"label":"G — Baggage system install (after B)","value":"8 weeks"},
               {"label":"H — Trial operations (after F and G)","value":"3 weeks"}],
             "task":"cpm","given":{"activities":[
               {"id":"A","dur":6,"preds":[]},
               {"id":"B","dur":10,"preds":["A"]},
               {"id":"C","dur":7,"preds":["A"]},
               {"id":"D","dur":5,"preds":["B"]},
               {"id":"E","dur":12,"preds":["C"]},
               {"id":"F","dur":4,"preds":["D","E"]},
               {"id":"G","dur":8,"preds":["B"]},
               {"id":"H","dur":3,"preds":["F","G"]}]},
             "ask":[
               {"key":"project_duration","label":"Project duration (weeks)","type":"number"},
               {"key":"critical_path","label":"Critical path (activity ids, comma-separated)","type":"set"},
               {"key":"float_G","label":"Total float on the baggage system install (weeks)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"accelerate","prompt":"There is funding to take four weeks out of one activity. The envelope contractor and the baggage supplier both say they can deliver it.",
                "options":[
                  {"key":"critical","label":"Take it from the critical path, then re-run the network — because removing four weeks may promote the baggage route to critical","quality":100,
                   "consequence":"The acceleration lands and the new constraint is identified in the same analysis.",
                   "principle":"Every successful acceleration creates a new critical path. Find it before the next decision, not after."},
                  {"key":"baggage","label":"Accelerate the baggage system — it is the highest-risk supplier","quality":30,
                   "consequence":"Risk on a path with float is reduced; the completion date does not move at all.",
                   "principle":"Reducing risk and reducing duration are different objectives with different targets."},
                  {"key":"stop","label":"Take the four weeks and hold the plan there","quality":45,
                   "consequence":"The saving is real and the programme now manages to a critical path it has not recalculated.",
                   "principle":"An out-of-date critical path is worse than none, because it is trusted."}]},
               {"key":"report","prompt":"How is the baggage route described in the monthly report?",
                "options":[
                  {"key":"near","label":"Report it as near-critical with its float stated, so a slip larger than that float is visibly a completion-date issue","quality":100,
                   "consequence":"When the supplier slips, everyone already knows the threshold that matters.",
                   "principle":"Name the float and you have named the moment a delay becomes everyone's problem."},
                  {"key":"noncritical","label":"Report it as non-critical","quality":15,
                   "consequence":"Technically accurate, and it invites the reader to stop watching a route that is weeks from becoming critical.",
                   "principle":"'Non-critical' describes today; float describes how long that stays true."},
                  {"key":"critical","label":"Report it as critical to keep the pressure on the supplier","quality":5,
                   "consequence":"Everything is critical, so nothing is, and the schedule stops informing any decision.",
                   "principle":"Criticality inflation destroys the only signal a schedule sends."}]}],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Delivery Realist","balanced":"Strategic Project Controller"},
             "share_line":"Found the near-critical path in an airport expansion before four weeks of acceleration made it the critical one.",
             "hints":[
               "Run the full passes — a network managed around one remembered path for a year usually has another chain closing in.",
               "Read the named route's float from the backward pass; protection is float, not reputation.",
               "Acceleration money belongs on the zero-float chain as it stands today — buying weeks on a protected route buys nothing at completion."]}
            """),

        // ── Traceability pair (051–052): the discipline of walking any reported number back to its
        //    source — the baseline through the change register, and the cost pack through the ledger.
        new("WC-TRC-051", "Walk me back to the signed number",
            "An auditor, a baseline, and a change register that must explain the difference.",
            "Museums & Culture", "Project Controls Engineer", "project_controls", "developing", 9,
            """["change_control","baseline_traceability","audit_readiness"]""",
            """
            {"context":"A museum wing restoration signed at a baseline of 8,200,000 over 300 days. An external auditor has asked the question every baseline must survive: walk me from the signed number to the number in this month's report. The change register below is the only bridge. Build the revised position from approved changes only, then answer the auditor.",
             "evidence":[
               {"label":"Signed baseline","value":"8,200,000 · 300 days"},
               {"label":"T1 — Structural steel reinforcement","value":"APPROVED · +240,000 · +10 days"},
               {"label":"T2 — Climate-controlled archive store","value":"REJECTED · +410,000 · +15 days"},
               {"label":"T3 — Reuse salvaged stone facings","value":"APPROVED · -60,000 · 0 days"},
               {"label":"T4 — Accessibility lift upgrade","value":"PENDING · +180,000 · +8 days"},
               {"label":"T5 — Fire-suppression rerouting","value":"APPROVED · +95,000 · +5 days"}],
             "task":"change","given":{"baseline_bac":8200000,"baseline_duration":300,"changes":[
               {"id":"T1","status":"approved","cost_delta":240000,"schedule_delta":10},
               {"id":"T2","status":"rejected","cost_delta":410000,"schedule_delta":15},
               {"id":"T3","status":"approved","cost_delta":-60000,"schedule_delta":0},
               {"id":"T4","status":"pending","cost_delta":180000,"schedule_delta":8},
               {"id":"T5","status":"approved","cost_delta":95000,"schedule_delta":5}]},
             "ask":[
               {"key":"revised_bac","label":"Revised budget (approved changes only)","type":"number"},
               {"key":"approved_cost_delta","label":"Net approved cost delta","type":"number"},
               {"key":"revised_duration","label":"Revised duration (days)","type":"number"},
               {"key":"approved_count","label":"Approved change count","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"audit","prompt":"The auditor is in the room. How do you walk them from the signed baseline to this month's reported budget?",
                "options":[
                  {"key":"register","label":"Reconcile it live: signed baseline plus each approved change equals the current baseline, with the pending item shown separately as exposure — then hand over the register extract","quality":100,
                   "consequence":"The walk takes four minutes, every step has a decision date and a signature, and the audit note records a clean trace.",
                   "principle":"A baseline is only a control if every step from the signed number to today's number has a name, a date and a decision."},
                  {"key":"forecast","label":"Present the current forecast as the baseline — it is the most realistic number in the room","quality":10,
                   "consequence":"The gap between the contract and the report now has no explanation on paper, and the auditor writes that down instead.",
                   "principle":"A number you cannot trace is an assertion, not a position."},
                  {"key":"fold","label":"Fold the pending lift upgrade into the baseline first, so the report matches what the site is actually building","quality":0,
                   "consequence":"The register and the baseline now disagree by exactly one unapproved change — which is the first thing a register is designed to catch.",
                   "principle":"Pending means pending. The moment an unapproved change moves the baseline, the audit trail is fiction."}]}],
             "profile_map":{"calculation":"Cost Guardian","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Walked an auditor from a museum restoration's signed baseline to the current report through a clean change register.",
             "hints":[
               "The bridge from signed baseline to reported budget is the approved-change ledger — every step is a signed item with a delta.",
               "Apply approved cost and time deltas only; the count of approved items is part of the audit trail.",
               "Walk the auditor item by item: baseline, plus each approved change in order, equals the reported figure — anything the walk cannot reach does not belong in the report."]}
            """),

        new("WC-TRC-052", "Every number in this pack has a source — prove it",
            "A stage-gate cost pack, a finance ledger, and eight figures that must agree.",
            "Medical Devices", "Cost Assurance Analyst", "cross_functional", "professional", 10,
            """["data_quality","traceability","assurance","reporting_integrity"]""",
            """
            {"context":"A device-serialisation compliance programme reaches its stage gate. The month-end cost pack cites eight control accounts; gate rules say every reported figure must trace to the finance ledger, and a discrepancy above 1,000 breaks the trace. The reconciliation below is what came back. Establish how traceable the pack really is, then decide what goes to the gate.",
             "evidence":[
               {"label":"CA-01 — reported / ledger","value":"118,400 / 118,400"},
               {"label":"CA-02 — reported / ledger","value":"76,250 / 74,830"},
               {"label":"CA-03 — reported / ledger","value":"no ledger reference cited / 63,750"},
               {"label":"CA-04 — reported / ledger","value":"205,000 / 205,380"},
               {"label":"CA-05 — reported / ledger","value":"41,900 / 44,150"},
               {"label":"CA-06 — reported / ledger","value":"9,800 / 9,800"},
               {"label":"CA-07 — reported / ledger","value":"152,610 / 152,100"},
               {"label":"CA-08 — reported / ledger","value":"no ledger reference cited / 88,000"},
               {"label":"Traceability threshold","value":"a gap above 1,000 breaks the trace"}],
             "task":"data_quality","given":{"threshold":1000,"rows":[
               {"value":118400,"expected":118400},
               {"value":76250,"expected":74830},
               {"expected":63750},
               {"value":205000,"expected":205380},
               {"value":41900,"expected":44150},
               {"value":9800,"expected":9800},
               {"value":152610,"expected":152100},
               {"expected":88000}]},
             "ask":[
               {"key":"anomaly_count","label":"Accounts whose reported figure breaks the trace","type":"number"},
               {"key":"completeness_pct","label":"Accounts with a ledger citation (%)","type":"number"},
               {"key":"mean_abs_error","label":"Mean absolute gap across the cited accounts","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"gate","prompt":"The gate board meets tomorrow morning. Two accounts break the trace and two cite no ledger entry at all.",
                "options":[
                  {"key":"annex","label":"Submit the pack with a traceability annex: every figure keyed to its ledger entry, each exception named with an owner and a correction date","quality":100,
                   "consequence":"The board passes the gate conditionally against the annex — and the two broken traces are fixed by people who were named in a document, not blamed in a meeting.",
                   "principle":"A pack the reader can trace line by line survives scrutiny; a clean-looking pack survives only until someone checks."},
                  {"key":"correct","label":"Correct the two figures to the ledger values yourself and submit a clean pack","quality":25,
                   "consequence":"Tomorrow's numbers are right and nobody knows why last month's were wrong — so the same feed breaks the same way next month.",
                   "principle":"A silent correction deletes the evidence that a correction was needed."},
                  {"key":"asis","label":"Submit as reported — finance signed the ledger, the pack cites the report, and the gate is about progress anyway","quality":5,
                   "consequence":"The board cross-checks one figure at random, it fails, and every other number in the pack is now suspect.",
                   "principle":"One untraceable number costs the credibility of all the traceable ones."}]},
               {"key":"root","prompt":"Both broken traces and both missing citations come from accounts that pass through the same monthly spreadsheet re-keying step.",
                "options":[
                  {"key":"handoff","label":"Fix the handoff: replace the re-keying step with a direct extract, and make the ledger reference a required field in the pack template","quality":100,
                   "consequence":"Next month's reconciliation is boring — which is exactly what a reconciliation should be.",
                   "principle":"When every failure shares a step, fix the step, not the failures."},
                  {"key":"check","label":"Add a second person to check the re-keying each month","quality":40,
                   "consequence":"The error rate drops and the cost of producing the pack rises, permanently, for a step that should not exist.",
                   "principle":"A second pair of eyes on a broken process is a slower broken process."},
                  {"key":"people","label":"Ask the two account owners to be more careful","quality":15,
                   "consequence":"They are careful for two months; the fourth month looks exactly like this one.",
                   "principle":"Blaming people for a process defect guarantees the defect a long career."}]}],
             "profile_map":{"calculation":"Data Investigator","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Proved a stage-gate cost pack against the ledger line by line, and put names and dates on every broken trace.",
             "hints":[
               "Traceability is testable: an account breaks the trace when its reported figure differs from the ledger beyond the stated threshold.",
               "Completeness counts the accounts holding a ledger citation; the mean gap averages the absolute differences across cited accounts.",
               "The gate answer is the measured state — how many trace, how many cite, how large the gaps — with the fixes named; certainty theatre is what the gate exists to catch."]}
            """),
    };
}
