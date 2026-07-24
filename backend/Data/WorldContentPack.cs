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
    };
}
