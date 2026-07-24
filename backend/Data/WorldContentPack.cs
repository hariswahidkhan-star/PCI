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

    static void Upsert(Db db, Pilot p)
    {
        var existing = db.QueryOne("SELECT id,author_id,status,current_version,config_json FROM pciworld_challenges WHERE code=?", p.Code);
        if (existing is null)
        {
            var id = db.ExecuteReturningId(@"INSERT INTO pciworld_challenges
                    (code,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,synthetic_declared,
                     config_json,status,current_version,published_at)
                VALUES(?,?,?,?,?,?,?,?,?,1,?, 'published', 1, datetime('now'))",
                p.Code, p.Title, p.Hook, p.Industry, p.Role, p.Track, p.Difficulty, p.Minutes, p.CompetenciesJson, p.ConfigJson);
            db.Execute(@"INSERT INTO pciworld_challenge_versions
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
             "share_line":"Diagnosed a metro fit-out's true cost and schedule position from raw EVM data."}
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
             "share_line":"Ran the critical path on a data-centre commissioning network and put acceleration money where it works."}
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
             "share_line":"Priced an offshore wind risk register and split expected value from tail protection."}
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
             "share_line":"Sized the real funding gap on a profitable project from its month-by-month cash curve."}
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
             "share_line":"Rebuilt a hospital project baseline from its change register — approved changes only."}
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
             "share_line":"Measured real weighted progress on a production line — and defended it."}
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
             "share_line":"Exposed a hidden schedule slip with Earned Schedule after cost-based SPI said 'fine'."}
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
             "share_line":"Turned three-point estimates into a defensible cutover commitment with a stated confidence."}
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
             "share_line":"Forecast a government programme's year-end position from six months of earned-value trend."}
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
             "share_line":"Audited an AI-drafted cost forecast against the measured data — and corrected it before the CFO saw it."}
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
             "share_line":"Measured real crew productivity on a façade package and found the true constraint."}
            """),

        new("WC-BOQ-012", "Price the bill before you sign it",
            "Four lines, one total, and a rate that deserves a second look.",
            "Real Estate", "Quantity Surveyor", "project_controls", "foundation", 7,
            """["cost_control","procurement"]""",
            """
            {"context":"A tenant fit-out bill of quantities arrives for sign-off. Recompute the bill total and the weighted average rate before it goes to the client.",
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
               {"key":"average_rate","label":"Weighted average rate","type":"number"}],
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
             "share_line":"Recomputed a fit-out bill of quantities and caught the rate that didn't belong."}
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
             "share_line":"Quantified a crew overload inside a fixed outage window and levelled it with float, not overtime."}
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
             "share_line":"Separated real critical delay from float noise when a 30-day supplier slip hit a port project."}
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
             "share_line":"Scored a five-project capital portfolio and defended the model when politics leaned on it."}
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
             "share_line":"Scored three haul-road options on cost, time and risk — and kept the trade-offs on the table."}
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
             "share_line":"Audited a roll-out data feed before the AI forecast ran — and shipped it with stated confidence."}
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
             "share_line":"Reported a refinery turnaround's true earned-value position under pressure to soften it."}
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
             "share_line":"Sized the funding trough for a live-events season before the bridge facility was signed."}
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
             "share_line":"Priced a dam-upgrade risk register and pointed the board at the tail, not the average."}
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
             "share_line":"Put a defensible confidence on a warship integration window — with the fallback pre-agreed."}
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
             "share_line":"Ran a cleanroom commissioning network and sent the acceleration where the path actually was."}
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
             "share_line":"Held a campus baseline together by keeping every change on the one road through the gate."}
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
             "share_line":"Forecast a digital programme's real completion in time units — and brought options, not apologies."}
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
             "share_line":"Turned five 'normal' months of data-centre variance into one undeniable trend."}
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
             "share_line":"Rolled up a hangar WBS and gave a homeless scope item an address before it billed anyone."}
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
             "share_line":"Rolled up a depot cost structure and reported the net without hiding the gross."}
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
             "share_line":"Certified a ship refit's real weighted progress against a more flattering impression."}
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
             "share_line":"Caught an AI 'recovery trend' that no time series supported — before it reached the board."}
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
             "share_line":"Took an offshore wind programme's eight-period trend to the investment committee with a funded recovery case."}
            """),
    };
}
