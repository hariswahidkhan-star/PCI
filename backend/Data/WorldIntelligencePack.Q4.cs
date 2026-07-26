namespace PCI.Backend.Data;

/// <summary>
/// PCI Project Intelligence — Year-1 Q4 authored pack (October onward).
/// October theme: digital delivery, data quality and responsible AI — every item advanced-band,
/// because by month ten the practitioner is expected to interrogate the machinery, not obey it.
/// Same contract as Q1–Q3: authored TO plan slots, three hints, consequence + principle per
/// option, synthetic data, validator + gates enforced in CI.
/// </summary>
public static partial class WorldIntelligencePack
{
    static readonly (string Code, string Title, string Hook, string Industry, string Role, string Track,
        string Difficulty, int Minutes, string Competencies, string Config)[] ItemsQ4 =
    {
        // ═════════════ OCTOBER — digital delivery, data quality and responsible AI ═════════════
        // ───────────── Daily Decisions · digital, data & AI · advanced ─────────────

        ("WC-AIA-284", "The model that was confidently wrong", "The forecast tool has been right for two years. This month it can't be.",
            "Capital Programmes", "Programme Controls Lead", "project_controls", "advanced", 6,
            """["ai_assurance","forecasting","professional_judgement"]""",
            """
            {"context":"Your multi-site capital programme uses a machine-learning forecast tool trained on two years of its own delivery data. It has been impressively accurate — and the board now quotes it directly. This month it forecasts on-time completion for all six sites, at high confidence. But three of the six sites have just switched to a new modular construction method the tool has never seen a single example of. The monthly report is due; the tool's output is already pasted into the draft.",
             "evidence":[
               {"label":"Tool record","value":"Two years of strong accuracy on conventional builds"},
               {"label":"This month","value":"All six sites forecast on-time, high confidence"},
               {"label":"Change","value":"Three sites switched to a modular method — zero examples in the training data"},
               {"label":"Draft report","value":"Tool output already pasted in as the programme forecast"}],
             "decisions":[
               {"key":"forecast","prompt":"The forecast section of the board report should say:",
                "options":[
                  {"key":"caveat","label":"Report the tool's forecast for the three conventional sites, and replace the modular three with a first-principles forecast plus an explicit statement that the model has no training data for the new method","quality":100,
                   "consequence":"The board sees a forecast that knows the edge of its own competence; when the modular sites run long on crane logistics, the report predicted the uncertainty rather than the wrong number.",
                   "principle":"A model's confidence is a statement about its training data, not about your project — novelty resets it to zero."},
                  {"key":"asis","label":"Publish the tool's forecast as usual — two years of accuracy has earned it the benefit of the doubt","quality":0,
                   "consequence":"The modular sites drift six weeks in ways the model structurally cannot see; the board learns the tool was extrapolating, and stops trusting it on the sites where it is genuinely strong.",
                   "principle":"Track record on familiar work says nothing about unfamiliar work — that is precisely what 'out of distribution' means."},
                  {"key":"suppress","label":"Pull the tool from the report entirely this month and forecast all six sites manually","quality":40,
                   "consequence":"Safe but wasteful: three sites had a valid machine forecast you discarded, and the board reads the tool's sudden absence as a scandal rather than a scoping judgement.",
                   "principle":"The remedy for a partly-blind instrument is to state where it sees, not to smash it."}]}],
             "hints":["Ask what the model was trained on before asking what it predicts.",
               "Three of the six sites now use a method the training data has never contained.",
               "The strongest report states the boundary: machine forecast where the model has seen the work, human forecast where it has not."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a high-confidence forecast extrapolating beyond its training data — and reported the boundary instead of the number."}
            """),

        ("WC-DQA-285", "A dashboard with two sources of truth", "Same metric, two numbers, one board meeting in an hour.",
            "Highways", "Senior Project Manager", "project_controls", "advanced", 7,
            """["data_quality","reporting_integrity","governance"]""",
            """
            {"context":"An hour before the highway upgrade's board meeting, you notice the earned-value dashboard and the commercial system disagree on period progress — one shows the scheme ahead, the other behind. Both are 'live'. Digging shows why: the dashboard ingests site quantities daily from the survey feed, while the commercial system recognises progress only when quantities are certified — a two-week lag. Neither number is wrong; they measure different events. The board pack was built from the dashboard; the client's own pack, you learn, is built from the certified data.",
             "evidence":[
               {"label":"Dashboard","value":"Ahead of plan — daily survey-feed quantities"},
               {"label":"Commercial system","value":"Behind plan — certified quantities, ~2-week lag"},
               {"label":"Board pack","value":"Built on the dashboard figure"},
               {"label":"Client pack","value":"Built on the certified figure"}],
             "decisions":[
               {"key":"board","prompt":"With an hour in hand, you:",
                "options":[
                  {"key":"reconcile","label":"Present both figures as one story — physical progress ahead on the survey feed, certification lagging by a stated two weeks — and table a single reconciliation rule for which figure governs which decision from next period","quality":100,
                   "consequence":"The board hears one coherent account before the client can weaponise the gap; the reconciliation rule ends a divergence that had been quietly widening for three periods.",
                   "principle":"Two honest measurements of different events are not a contradiction — until nobody states the difference."},
                  {"key":"dashboard","label":"Stand by the dashboard — it is the more current data and the pack is already printed","quality":10,
                   "consequence":"The client opens with the certified figure; the meeting becomes an argument about whose number is real, which is the one argument that damages both sides' credibility.",
                   "principle":"A number you cannot reconcile to the other side's number is not a position, it is an ambush waiting for you."},
                  {"key":"delay","label":"Ask to postpone the progress item until the two systems are aligned","quality":25,
                   "consequence":"The postponement reads as concealment; the systems cannot 'align' anyway, because they legitimately measure different events on different clocks.",
                   "principle":"You cannot schedule away a definitional difference — you can only govern it."}]}],
             "hints":["Check what event each system actually records before deciding which is 'right'.",
               "One measures work done on the ground; the other measures work certified — the lag between them is structural.",
               "The durable fix is a governance rule naming which figure drives which decision — not a one-off correction."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned two contradicting progress figures into one story and one reconciliation rule — an hour before the board."}
            """),

        ("WC-AIA-286", "The forecast the algorithm inherited", "The number looks authoritative. Its author left no working.",
            "Portfolio Management", "Portfolio Analyst", "project_controls", "advanced", 5,
            """["ai_assurance","forecasting","evidence_analysis"]""",
            """
            {"context":"Preparing the quarterly portfolio review, you find the cost-at-completion figure for the largest programme was produced by a forecasting service the previous analyst configured before leaving. No documentation: the model type, the training window, the assumptions — all unknown. The figure has been rolling forward, lightly adjusted, for three quarters. The portfolio director wants the pack finalised today and considers the figure 'settled — it's been stable for nine months'.",
             "evidence":[
               {"label":"Figure","value":"Cost-at-completion from an undocumented forecasting service"},
               {"label":"Provenance","value":"Configured by a departed analyst; no record of model or assumptions"},
               {"label":"History","value":"Rolled forward, lightly adjusted, three quarters"},
               {"label":"Director","value":"'Settled — it's been stable for nine months'"}],
             "decisions":[
               {"key":"figure","prompt":"You:",
                "options":[
                  {"key":"flag","label":"Keep the figure in the pack but flag its provenance honestly — undocumented model, unknown assumptions — and commission a parallel bottom-up forecast to validate or retire it by next quarter","quality":100,
                   "consequence":"The parallel forecast lands 8% adrift and exposes a stale escalation assumption baked in before the market shifted; the service is reconfigured with documented assumptions and the pack's credibility survives.",
                   "principle":"Stability is not validity — an unexamined number can be reliably, consistently wrong."},
                  {"key":"accept","label":"Leave it — nine months of stability is its own evidence, and reopening it now creates noise before the review","quality":0,
                   "consequence":"The stale escalation assumption surfaces mid-review through a director-level question you cannot answer; 'we don't know how it's calculated' becomes the quote of the quarter.",
                   "principle":"A figure whose working nobody can produce is an opinion wearing a number's clothes."},
                  {"key":"strip","label":"Pull the figure from the pack until the model is fully reverse-engineered","quality":30,
                   "consequence":"The review proceeds blind on its largest programme — a worse outcome than a flagged estimate — and reverse-engineering an undocumented service takes months you didn't need to spend before saying anything.",
                   "principle":"An honest caveat now beats a perfect answer after the decision."}]}],
             "hints":["Ask what would happen if someone at the review asked how the number is calculated.",
               "'Stable for nine months' — distinguish a stable process from a stable error.",
               "You can keep the number and challenge it at once: flag provenance, run a parallel estimate against it."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Refused to let a number's nine-month stability stand in for knowing where it came from."}
            """),

        ("WC-DQA-287", "Data quality as a delivery risk", "The join was broken for a year. Every report since was built on it.",
            "Energy Networks", "Programme Controls Manager", "project_controls", "advanced", 6,
            """["data_quality","risk_management","reporting_integrity"]""",
            """
            {"context":"A new analyst on the grid reinforcement programme discovers that the join between the works-order system and the cost ledger drops any order whose reference contains a revision suffix — a convention one delivery partner has used for a year. Result: that partner's committed costs have been under-reported in every integrated report for twelve months. Corrected, programme commitment rises materially — not because anything changed on the ground, but because the reports finally see it. The quarterly review is in two weeks; the correction will look like a sudden deterioration.",
             "evidence":[
               {"label":"Defect","value":"System join silently drops works orders with revision-suffixed references"},
               {"label":"Exposure","value":"One partner's commitments under-reported for ~12 months"},
               {"label":"Correction effect","value":"Reported commitment rises materially; ground truth unchanged"},
               {"label":"Timing","value":"Quarterly review in two weeks"}],
             "decisions":[
               {"key":"correction","prompt":"You:",
                "options":[
                  {"key":"restate","label":"Correct the join now, restate the affected periods, and lead the quarterly review with the data-quality finding itself — cause, exposure window, restated trend, and the control added so a silent join failure cannot recur","quality":100,
                   "consequence":"The review is uncomfortable and credible: the restated trend shows the programme was never as underspent as it appeared, and the new reconciliation control catches a second, smaller feed defect within the month.",
                   "principle":"When the data was wrong, the correction is the news — burying it inside a normal report converts an error into a concealment."},
                  {"key":"smooth","label":"Phase the correction in over three reporting periods so no single report shows a jump","quality":0,
                   "consequence":"Each phased report is knowingly misstated; an auditor later reconstructs the true correction date, and the phasing — not the original defect — becomes the governance finding.",
                   "principle":"Smoothing a known error is not presentation, it is misreporting with a schedule."},
                  {"key":"defer","label":"Hold the correction until after the quarterly review to avoid destabilising it","quality":10,
                   "consequence":"The review approves decisions against numbers you knew were wrong; the deferral surfaces anyway — corrections always do — with your sign-off date attached.",
                   "principle":"A review protected from the truth is not stabilised, it is disarmed."}]}],
             "hints":["Separate what changed on the ground (nothing) from what changed in visibility (everything).",
               "Any option that involves knowingly publishing a wrong number has already answered itself.",
               "Lead with cause, window, restatement and the new control — the four things a review needs to keep trusting the data."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Restated a year of under-reported commitment and made the data-quality finding the headline, not the footnote."}
            """),

        ("WC-AIA-288", "An AI recommendation without provenance", "The tool says descope. It won't say why.",
            "Technology Programmes", "Delivery Director", "project_management", "advanced", 7,
            """["ai_assurance","governance","professional_judgement"]""",
            """
            {"context":"Your platform migration runs a vendor 'delivery intelligence' tool that ingests the programme's plan, tickets and finances. This week it recommends descoping the customer-data workstream — flagged 'high risk of failure, low benefit contribution'. The vendor cannot explain the recommendation: the model is proprietary, the feature weights are not disclosed, and the same tool rated the workstream healthy last month with no material data change you can identify. The steering committee has seen the recommendation and two members arrive convinced.",
             "evidence":[
               {"label":"Recommendation","value":"Descope customer-data workstream — 'high risk, low benefit'"},
               {"label":"Explainability","value":"None — proprietary model, undisclosed weights"},
               {"label":"Consistency","value":"Same workstream rated healthy last month; no clear data change"},
               {"label":"Committee","value":"Two members arrive already convinced"}],
             "decisions":[
               {"key":"steer","prompt":"At the steering committee you:",
                "options":[
                  {"key":"interrogate","label":"Treat the recommendation as a hypothesis, not a decision: require the descope case to be made from inspectable evidence — the workstream's own delivery data, dependency map and benefit trace — and note the tool's month-on-month flip as a reliability question for the vendor","quality":100,
                   "consequence":"The inspectable review finds the workstream sound; the flip traces to a vendor-side retraining that quietly changed the model's risk thresholds — which becomes a contract conversation about change control on the tool itself.",
                   "principle":"An unexplainable recommendation can start an investigation; it can never end one."},
                  {"key":"follow","label":"Accept the recommendation — the tool sees patterns across hundreds of programmes that no committee can","quality":0,
                   "consequence":"The descoped workstream turns out to underpin the regulatory migration path; reinstating it costs a quarter, and 'the AI told us to' survives in the lessons-learned register verbatim.",
                   "principle":"Scale of training data is not a substitute for a reason — especially from a model that changed its mind without new evidence."},
                  {"key":"ban","label":"Reject the recommendation and propose removing the tool from governance inputs entirely","quality":35,
                   "consequence":"Cathartic but wasteful: the tool's anomaly detection has genuine value as a prompt for questions; the committee loses a useful tripwire because it was misused once as an oracle.",
                   "principle":"The failure mode was treating a signal as a verdict — fix the treatment, not the signal."}]}],
             "hints":["Ask what changed in the programme's data to flip the rating — and whether anything did.",
               "A recommendation that cannot be explained can be investigated: the underlying delivery evidence is inspectable even if the model is not.",
               "Position the tool as a source of questions for governance, never a source of answers."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Demoted an unexplainable AI verdict to a hypothesis — and found the real change was in the model, not the programme."}
            """),

        // ───────────── Project Rescue · schedule · advanced · multi-stage ─────────────

        ("WC-RSC-289", "The float that was spent three times", "The schedule says ten weeks of float. Three teams have already promised it away.",
            "Life Sciences Construction", "Recovery Planning Lead", "project_management", "advanced", 15,
            """["schedule_analysis","recovery_management","stakeholder_communication"]""",
            """
            {"context":"You are brought in to stabilise a sterile-facility build whose completion date is 'protected by ten weeks of float' — according to the master schedule. Reality: the cleanroom contractor has verbally promised four of those weeks to absorb its HVAC redesign; the validation team has planned its protocol writing assuming six weeks of early access the float was supposed to provide; and the client's operations group has told the regulator commissioning starts on the float-free date. The same ten weeks has been informally spent three times over. Nobody has told the schedule.",
             "evidence":[
               {"label":"Master schedule","value":"10 weeks float protecting completion"},
               {"label":"Cleanroom contractor","value":"4 weeks verbally absorbed for HVAC redesign"},
               {"label":"Validation team","value":"Plan assumes 6 weeks early access from the same float"},
               {"label":"Client ops","value":"Regulator told commissioning starts on the float-free date"}],
             "decisions":[
               {"key":"truth","prompt":"Stage 1 — your first move is to:",
                "options":[
                  {"key":"ledger","label":"Build a float ledger: put every informal claim on the ten weeks in one view, dated and attributed, and re-run the schedule with each claim modelled as a real commitment","quality":100,
                   "consequence":"The re-run shows the 'protected' date is actually three weeks exposed — a fact nobody could see while each claim lived in a different conversation.",
                   "principle":"Float that has been promised is not float — the first act of rescue is finding out who owns what everyone thinks is spare."},
                  {"key":"enforce","label":"Declare the schedule authoritative: no float commitments exist unless they are in it, so all three claims are void","quality":20,
                   "consequence":"Legally tidy, practically false — the HVAC redesign still needs its four weeks whether the schedule acknowledges the promise or not, and the teams now hide their assumptions from you.",
                   "principle":"Declaring reality void does not reschedule it."},
                  {"key":"escalate","label":"Take the conflict straight to the client executive as a dispute between the three teams","quality":40,
                   "consequence":"Premature: without a quantified ledger the executive hears three plausible stories and picks by seniority — the regulator commitment wins, and the physical constraints lose.",
                   "principle":"Escalate a quantified position, not a quarrel."}]},
               {"key":"allocate","prompt":"Stage 2 — the ledger shows claims totalling 13 weeks against 10. You:",
                "options":[
                  {"key":"prioritise","label":"Allocate float by schedule logic, not seniority: the HVAC redesign gets its four weeks (it is physically on the critical path), validation gets restructured for phased access instead of six clear weeks, and the regulator date moves via the client — with the ledger as the evidence","quality":100,
                   "consequence":"Validation's phased protocol work proves workable with two weeks' handover overlap; the client renegotiates the regulator date once, early, from evidence — instead of missing it once, late, from surprise.",
                   "principle":"When claims exceed float, allocation follows the physics of the critical path — everything else is negotiable, the load-bearing work is not."},
                  {"key":"split","label":"Split the ten weeks pro-rata across the three claims to keep all parties partially satisfied","quality":10,
                   "consequence":"The HVAC redesign gets 3 of the 4 weeks it physically needs — so the critical path slips anyway, and all three parties are both disappointed and delayed.",
                   "principle":"Fairness between claims is meaningless when one claim is a physical constraint and the others are preferences."},
                  {"key":"defend","label":"Refuse all claims and hold the full ten weeks as management reserve","quality":25,
                   "consequence":"The redesign happens anyway — unplanned, absorbed as delay rather than allocation — and the reserve you defended evaporates without ever being 'spent'.",
                   "principle":"Reserve that ignores committed reality is not protected, it is pre-spent without accounting."}]},
               {"key":"institutionalise","prompt":"Stage 3 — to stop this recurring, you:",
                "options":[
                  {"key":"control","label":"Make float an owned, change-controlled resource: a named owner, a visible ledger in the schedule narrative, and a rule that any commitment of float — verbal included — is a schedule change requiring the same approval as a date change","quality":100,
                   "consequence":"Two months later the fit-out contractor requests float through the process instead of around it — the request is half-granted, the schedule stays true, and the completion date's protection is finally real.",
                   "principle":"Float is a resource; anything a project can spend, it must be able to account for."},
                  {"key":"hide","label":"Stop publishing float figures so teams cannot plan against them","quality":15,
                   "consequence":"Teams infer float from the dates instead — less accurately — and the informal market in schedule slack continues, now with worse information.",
                   "principle":"Opacity does not end informal claims; it just ends your visibility of them."}]}],
             "hints":["Count how many times the same ten weeks has been separately promised.",
               "Rank the three claims by which one is a physical critical-path constraint rather than a planning preference.",
               "The lasting fix treats float like money: owned, visible, and spent only through change control."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found ten weeks of float spent three times over — and turned it into an owned, change-controlled resource."}
            """),

        ("WC-RSC-290", "Two critical paths, one promise", "The energisation date was promised on a schedule that no longer exists.",
            "Energy Utilities", "Recovery Planning Lead", "project_management", "advanced", 13,
            """["schedule_analysis","recovery_management","governance"]""",
            """
            {"context":"A substation replacement has drifted into trouble: a cable-route consent delay has created a second critical path through the civils, running within days of the original path through switchgear delivery. The outage window for energisation — booked with the system operator a year ago and politically expensive to move — sits eleven weeks out. The programme manager's draft recovery plan compresses the civils path back below the switchgear path and declares the outage window safe. It assumes consent lands in two weeks and dry weather for the duct runs.",
             "evidence":[
               {"label":"Paths","value":"Civils path (consent-delayed) and switchgear path within days of each other"},
               {"label":"Outage window","value":"Energisation slot with system operator, 11 weeks out, costly to move"},
               {"label":"Draft recovery","value":"Compresses civils below switchgear; declares window safe"},
               {"label":"Assumptions","value":"Consent in 2 weeks + dry weather for duct runs"}],
             "decisions":[
               {"key":"assess","prompt":"Stage 1 — reviewing the draft recovery plan, you:",
                "options":[
                  {"key":"nearcritical","label":"Reject 'the critical path is safe' as the wrong frame: with two paths within days of each other, manage both as critical, and test the plan against consent at 4 weeks and a wet month — the plausible bad case, not the hopeful one","quality":100,
                   "consequence":"The stress test shows the window fails under either assumption slipping alone — which changes the conversation from 'we are fine' to 'we need contingency now', eleven weeks early instead of two.",
                   "principle":"A near-critical path is a critical path that hasn't been unlucky yet — plans that only survive their own assumptions are forecasts, not plans."},
                  {"key":"accept","label":"Accept the plan — it restores the original critical path and the assumptions are each individually reasonable","quality":0,
                   "consequence":"Consent takes five weeks; the 'restored' path was fiction by week three, and the outage renegotiation happens in the worst possible month with the least possible notice.",
                   "principle":"Individually reasonable assumptions multiply into a collectively unreasonable plan."},
                  {"key":"resequence","label":"Direct a full re-baseline of the schedule before any recovery decisions","quality":30,
                   "consequence":"Four weeks of re-baselining consumes the very time the recovery needed; the answer arrives polished and too late to act on.",
                   "principle":"Rescue needs a decision-grade view fast, not a perfect baseline slowly."}]},
               {"key":"window","prompt":"Stage 2 — the stress test says the window is at genuine risk. On the outage commitment, you:",
                "options":[
                  {"key":"twotrack","label":"Run two tracks openly: drive the recovery actions that could still make the window, while formally opening the fallback conversation with the system operator now — trigger dates, partial-energisation options, next available slots — before you need any of it","quality":100,
                   "consequence":"The operator, approached early, reveals a partial-energisation option nobody had asked about; when consent lands at week five, the programme takes it — recovering the commitment in substance if not in original form.",
                   "principle":"The moment a promise is at risk, the cheapest option is the conversation you open before you must."},
                  {"key":"silent","label":"Hold the commitment publicly and say nothing to the operator until the recovery either works or fails","quality":0,
                   "consequence":"The fallback options that existed at eleven weeks' notice do not exist at two; the late renegotiation costs a six-month slot and the operator's trust with it.",
                   "principle":"Protecting a date by hiding its risk destroys the very options that could have protected it."},
                  {"key":"surrender","label":"Move the outage now and remove the schedule pressure entirely","quality":25,
                   "consequence":"The pressure disappears — and so does the focus; the recovery actions that would have made the window drift, and the programme pays the full political cost of the move without having tried to avoid it.",
                   "principle":"Abandoning a recoverable commitment is not prudence, it is pre-emptive failure."}]}],
             "hints":["Two paths within days of each other — ask what 'the critical path is safe' even means.",
               "Test the recovery against the plausible bad case on both assumptions, not the hopeful case on each.",
               "Approach the outage owner while options still exist — early conversations find fallbacks that late ones cannot."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Stress-tested a hopeful recovery plan, managed both near-critical paths, and found the fallback before it was needed."}
            """),

        ("WC-RSC-291", "A milestone with no logic behind it", "The opening date drives everything. Nothing in the schedule drives the opening date.",
            "Cultural Projects", "Recovery Planning Lead", "project_management", "advanced", 18,
            """["schedule_analysis","recovery_management","stakeholder_communication"]""",
            """
            {"context":"A museum gallery rebuild is 'on track for the October opening' — a date announced publicly two years ago. Reviewing the schedule, you find the opening milestone is not logic-linked to anything: it floats at the end of the plan, and the activities that must precede it — exhibition fit-out, environmental stabilisation of the conservation spaces, object installation by specialist couriers — are tracked in a separate curatorial spreadsheet with its own dates. The construction schedule ends at 'practical completion' six weeks before opening; everyone assumed six weeks was enough. The curatorial spreadsheet, read properly, needs fourteen.",
             "evidence":[
               {"label":"Opening milestone","value":"Public date, announced 2 years ago — no predecessors in the schedule"},
               {"label":"Construction schedule","value":"Ends at practical completion, 6 weeks before opening"},
               {"label":"Curatorial spreadsheet","value":"Fit-out, stabilisation, installation — needs 14 weeks, tracked separately"},
               {"label":"Gap","value":"8 weeks between assumption and requirement, invisible to both plans"}],
             "decisions":[
               {"key":"integrate","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"onelogic","label":"Build one integrated logic from practical completion to opening — construction, environmental stabilisation, fit-out, installation — with the curatorial activities as first-class scheduled work, and let the integrated network say what the opening date actually needs","quality":100,
                   "consequence":"The integrated network confirms the 8-week gap but also finds 3 weeks of genuine overlap (stabilisation can start zone-by-zone behind the builders) — the real problem is 5 weeks, not 8, and now it is visible and ownable.",
                   "principle":"A milestone with no predecessors is not a plan, it is a hope with a date — integration is what converts it back into a plan."},
                  {"key":"pressure","label":"Direct the curatorial team to compress fourteen weeks to six — the date is public and non-negotiable","quality":0,
                   "consequence":"Environmental stabilisation cannot be compressed by instruction — conservation-grade humidity takes the time it takes; objects install into unstable spaces, the lenders' couriers refuse handover, and the opening slips anyway, chaotically.",
                   "principle":"Physics and lender contracts do not attend steering committees — compression by decree only relocates the slip."},
                  {"key":"separate","label":"Keep the plans separate but add a coordination meeting between the two teams","quality":20,
                   "consequence":"The meeting discusses the gap monthly without owning it; two plans with different truths continue, now with minutes.",
                   "principle":"Coordination without integration is how this gap was created, not how it closes."}]},
               {"key":"recover","prompt":"Stage 2 — the integrated network shows 5 weeks to find. You:",
                "options":[
                  {"key":"resequence","label":"Attack the network, not the teams: phase practical completion zone-by-zone so stabilisation and fit-out start early in finished zones, resequence installation by courier availability, and buy the remaining fortnight with targeted acceleration of the two construction activities that gate the first zones","quality":100,
                   "consequence":"Zone-phased handover recovers three and a half weeks; courier resequencing and the two accelerations close the rest — the opening holds, on logic rather than luck.",
                   "principle":"Recovery lives in the network's structure — overlap, sequence, and selective acceleration — before it lives in anyone's overtime."},
                  {"key":"delay","label":"Recommend moving the public opening date now — five weeks is too much to recover credibly","quality":30,
                   "consequence":"Defensible but premature: the zone-phasing option had not been tested, and a public date was surrendered that the network could have held; the institution pays reputationally for a slip that analysis could have closed.",
                   "principle":"Recommend moving a public commitment only after the network says you must — not before you have asked it."},
                  {"key":"parallel","label":"Order all remaining work to run in parallel and accept the congestion","quality":10,
                   "consequence":"Builders, conservators and couriers occupy the same spaces simultaneously; dust ruins the stabilisation, a loaned object is damaged in the congestion, and the resulting stand-down costs more than the five weeks.",
                   "principle":"Parallelism without logic is not acceleration, it is collision."}]},
               {"key":"govern","prompt":"Stage 3 — going forward, the opening date is governed by:",
                "options":[
                  {"key":"integrated","label":"The integrated network, owned by one planner, reported against one critical path that runs from construction through curatorial work to opening — with the public date's protection stated as measured float on that path","quality":100,
                   "consequence":"For the first time the trustees see one number for the opening's protection — and when a courier slot later slips, the effect is visible in days, not discovered in weeks.",
                   "principle":"One promise deserves one network — anything that must happen before the date belongs in the logic that protects it."},
                  {"key":"buffer","label":"A fixed management buffer added before the opening, sized generously, with the plans otherwise unchanged","quality":25,
                   "consequence":"The buffer absorbs the unknown for a while — but with two unintegrated plans still feeding it, nobody can say how much buffer remains at any moment, which is the original disease with better padding.",
                   "principle":"A buffer protecting an unintegrated plan is insulation on a house with no walls."}]}],
             "hints":["Find what the opening milestone is actually linked to in the schedule — then ask why.",
               "The gap is between an assumed handover period and a calculated one — integrate the two plans and measure it.",
               "Recovery order: overlap zones first, resequence second, accelerate the two gating activities last."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave a floating public milestone its missing logic — and held the date through structure, not overtime."}
            """),

        ("WC-RSC-292", "The recovery plan that borrowed from testing", "The plan gets the dates back. It pays for them out of the test window.",
            "Technology Rollouts", "Recovery Planning Lead", "project_management", "advanced", 14,
            """["schedule_analysis","recovery_management","quality_management"]""",
            """
            {"context":"A national point-of-sale rollout is six weeks late on software integration. The vendor's recovery plan restores the pilot-store date — by cutting system test from eight weeks to three and moving 'non-critical test scenarios' into the pilot itself, where 'real stores will surface real issues'. The retail client's board likes the plan: the dates are back. You are asked, as the closing act of the recovery review, whether the programme should adopt it. The rollout's lessons-learned register from a previous phase records that the last defect that escaped testing cost four weeks of store disruption to fix in production.",
             "evidence":[
               {"label":"Slip","value":"6 weeks late on software integration"},
               {"label":"Recovery plan","value":"Pilot date restored by cutting system test 8 weeks → 3"},
               {"label":"Mechanism","value":"'Non-critical' scenarios deferred into the live pilot"},
               {"label":"History","value":"Last escaped defect: 4 weeks of store disruption to fix in production"}],
             "decisions":[
               {"key":"verdict","prompt":"Stage 1 — your recommendation on the vendor plan:",
                "options":[
                  {"key":"expose","label":"Reject the plan as presented and re-state it honestly: it does not recover six weeks, it converts a visible schedule slip into an invisible quality debt — then require any test compression to be justified scenario-by-scenario against defect risk, not asserted in aggregate","quality":100,
                   "consequence":"Scenario-level review shows 2 of the 5 cut weeks are genuinely low-risk regression re-runs; the other 3 cover payment reconciliation — the exact area of the previous escape. The honest plan recovers 2 weeks, not 5, and says so.",
                   "principle":"A recovery plan that moves risk instead of removing it has not recovered anything — it has refinanced the slip at a worse rate."},
                  {"key":"adopt","label":"Support the plan — pilots exist to find issues, and the board's confidence is itself worth protecting","quality":0,
                   "consequence":"The pilot stores become the test environment; a reconciliation defect corrupts three days of live takings, and the recovery of confidence costs precisely the four production weeks the register predicted.",
                   "principle":"Deferring tests into production does not test less, it tests later, in front of customers, at production prices."},
                  {"key":"block","label":"Insist the full eight-week test window is untouchable and the pilot date must move by the full slip","quality":35,
                   "consequence":"Defensibly cautious, but it concedes two recoverable weeks and hands the vendor the argument that governance, not engineering, is now the critical path.",
                   "principle":"Test windows deserve scenario-level defence, not blanket sanctity — rigour that won't itemise invites override."}]},
               {"key":"protect","prompt":"Stage 2 — to keep this from recurring in the next phase, you:",
                "options":[
                  {"key":"gate","label":"Make test scope a governed baseline: the scenario set is change-controlled like the budget, any deferral names the risk it accepts and who accepts it, and 'recovered' dates are reported alongside the debt they carry","quality":100,
                   "consequence":"The next squeeze — and there is one — arrives as a signed risk-acceptance decision by the client sponsor rather than a quiet line in a vendor plan; two deferrals are approved, one is refused, and everyone can see why.",
                   "principle":"What can be silently cut will be — the defence is making the cut loud, owned and priced."},
                  {"key":"trust","label":"Rely on the vendor's test manager to hold the line next time — they agreed with your analysis privately","quality":15,
                   "consequence":"The test manager holds the line until the next date crisis, when the same commercial pressure that wrote this plan writes the next one over their head.",
                   "principle":"A control that depends on one person's resistance to their own employer's pressure is not a control."}]}],
             "hints":["Ask what the plan actually does with the six weeks — recover them, or relocate them?",
               "Check the deferred scenarios against where the last production escape came from.",
               "Demand scenario-level justification: some test compression is real; the aggregate claim is where the debt hides."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-priced a recovery plan that paid for its dates out of the test window — and made quality debt visible in governance."}
            """),

        // ───────────── Risk Room · advanced · evidence diagnosis ─────────────

        ("WC-RSK-293", "The register that stopped being read", "Forty-one risks, faithfully updated, and the one that matters isn't in it.",
            "Transport Depots", "Programme Risk Analyst", "project_controls", "advanced", 10,
            """["risk_management","evidence_analysis","governance"]""",
            """
            {"context":"The depot modernisation's risk register holds forty-one live risks, updated monthly, RAG-rated, actioned. Yet reading three months of other artifacts side-by-side tells a different story: the site diary logs seven separate visits by the train operator's engineering inspectors — double the usual cadence; the correspondence log shows the operator has begun copying its legal team on routine letters; and the commissioning manager's weekly notes twice mention the operator 'reserving its position' on the acceptance test scope. None of this appears in the register. The register's own top risk remains 'possession overruns', comfortably actioned for a year.",
             "evidence":[
               {"label":"Register","value":"41 risks, monthly updates, top risk 'possession overruns' — well actioned"},
               {"label":"Site diary","value":"Operator inspection visits at twice normal cadence over 3 months"},
               {"label":"Correspondence","value":"Operator now copying legal on routine letters"},
               {"label":"Commissioning notes","value":"Operator twice 'reserving its position' on acceptance test scope"}],
             "decisions":[
               {"key":"diagnose","prompt":"Read together, the artifacts most strongly indicate:",
                "options":[
                  {"key":"acceptance","label":"The operator is quietly building a case to contest acceptance — inspection cadence, legal visibility and 'reserved positions' are the paper trail of a party preparing to dispute, and the register has no line for it","quality":100,
                   "consequence":"Raised now, a joint acceptance-criteria workshop surfaces the operator's real concern — a maintenance-access clause it believes the new layout breaches — while it is still a design conversation rather than a refusal at handover.",
                   "principle":"Registers record the risks people are willing to say; behaviour records the ones they are preparing to act on."},
                  {"key":"routine","label":"Heightened but routine assurance — operators always increase scrutiny near commissioning, and legal on copy is standard governance hygiene","quality":10,
                   "consequence":"Three months later the operator declines to accept the depot, citing concerns 'raised repeatedly through inspection'; the paper trail you dismissed becomes their evidence and your surprise.",
                   "principle":"When a counterparty's behaviour changes on three independent channels at once, 'routine' is the least likely explanation."},
                  {"key":"register","label":"A register process failure — the real finding is that the risk process needs an audit and refresh","quality":40,
                   "consequence":"True but secondary: the audit takes six weeks and finds what you already knew, while the acceptance dispute matures unattended. Process findings do not answer live signals.",
                   "principle":"Fix the instance before the process — a maturing risk outranks the machinery that missed it."}]},
               {"key":"act","prompt":"Your next move:",
                "options":[
                  {"key":"engage","label":"Register the risk with an owner, then engage the operator directly and openly at the right level — name the observed pattern, ask what sits behind it, and propose jointly re-confirming the acceptance criteria before commissioning","quality":100,
                   "consequence":"The conversation is uncomfortable for a week and saves a quarter: the access-clause issue gets a design answer, and the operator's legal team drops off the correspondence within a month.",
                   "principle":"A risk you can name to the counterparty is a risk you can still manage together; one you only track is already a dispute."},
                  {"key":"harden","label":"Quietly harden your own position — tighten records, brief your legal team, prepare the acceptance file for a contest","quality":30,
                   "consequence":"Prudent paperwork, wrong posture: both sides now prepare for the dispute neither has voiced, and the depot opens late under a settlement that a single early conversation would have made unnecessary.",
                   "principle":"Mirroring a counterparty's escalation confirms it — someone has to convert signal into speech, and it may as well be you."}]}],
             "hints":["Compare what the register says with what the diary, correspondence and notes are doing.",
               "Three independent channels changed at once — ask what single preparation would explain all three.",
               "The move that beats a maturing dispute is naming it to the other party while it is still a conversation."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read the risk the register couldn't hold — in the inspection cadence, the cc line, and two reserved positions."}
            """),

        ("WC-RSK-294", "A contingency drawdown decision", "The request is legitimate. So is the reason to refuse it.",
            "Advanced Manufacturing", "Programme Risk Analyst", "project_controls", "advanced", 12,
            """["risk_management","cost_management","evidence_analysis"]""",
            """
            {"context":"The production-line install is at month 7 of 12. The mechanical contractor requests a contingency drawdown for out-of-sequence working caused by late steel — a genuine, evidenced cost. The request would consume 40% of remaining contingency. Your drawdown analysis shows: contingency spent to date maps almost entirely to schedule-driven causes; the risk register's remaining exposure is weighted toward the commissioning phase — the part of the job that has not started; and the register's commissioning risks have barely moved in probability since baseline, because nobody has updated them since the line design was value-engineered in month 3. The value engineering removed a buffer conveyor that the original commissioning plan quietly relied on.",
             "evidence":[
               {"label":"Request","value":"Legitimate, evidenced out-of-sequence working claim — 40% of remaining contingency"},
               {"label":"Spend pattern","value":"Contingency to date consumed by schedule-driven causes"},
               {"label":"Remaining exposure","value":"Register-weighted toward commissioning — not yet started"},
               {"label":"Stale inputs","value":"Commissioning risks unrevised since month-3 value engineering removed a buffer conveyor"}],
             "decisions":[
               {"key":"diagnose","prompt":"The analysis's most important finding is:",
                "options":[
                  {"key":"stale","label":"The remaining-exposure figure is unreliable in the dangerous direction — commissioning risks were never re-scored after the value engineering removed the buffer conveyor, so the 'affordable' drawdown is being measured against an understated future","quality":100,
                   "consequence":"Re-scoring with the commissioning team lifts remaining exposure materially — the conveyor's removal turned two medium risks into one large one — and the drawdown decision changes character before it is made.",
                   "principle":"A drawdown is only as sound as the remaining-exposure estimate it is measured against — and exposure estimates rot fastest right after a design change."},
                  {"key":"pattern","label":"The spend pattern — contingency is being consumed by schedule causes, which suggests the schedule reserve, not cost contingency, is the right pot for this claim","quality":45,
                   "consequence":"A fair governance point that tidies the accounting — but it leaves the real problem untouched: whichever pot pays, the future exposure it is measured against is stale.",
                   "principle":"Which pot pays matters less than whether anyone knows what the pot must still cover."},
                  {"key":"legit","label":"The claim's legitimacy — it is evidenced, so it should be paid and the analysis is complete","quality":10,
                   "consequence":"The claim is paid; at commissioning, the understated risks land against a contingency that can no longer answer them, and month 11 becomes a funding crisis that month 7 quietly authorised.",
                   "principle":"A legitimate claim can still be an unaffordable drawdown — legitimacy and affordability are separate questions."}]},
               {"key":"act","prompt":"On the drawdown request itself, you recommend:",
                "options":[
                  {"key":"sequence","label":"Re-score commissioning exposure first — a focused half-day with the commissioning lead, not a full register refresh — then size the drawdown against the corrected remainder, splitting the claim if needed between contingency and a change against the value-engineering decision","quality":100,
                   "consequence":"The re-score takes two days; the claim is paid in part from contingency and in part as a change that finally prices the conveyor's removal honestly. Remaining contingency ends the month smaller but truthful.",
                   "principle":"Sequence matters: correct the denominator before approving the withdrawal."},
                  {"key":"pay","label":"Approve now — the contractor's claim is time-sensitive and the register can be refreshed at next month's review","quality":15,
                   "consequence":"Next month's refresh delivers the bad news four weeks after the money left; the programme spends its commissioning phase negotiating emergency funding instead of commissioning.",
                   "principle":"'Refresh later' after 'spend now' is the standard sequence of every contingency crisis in history."},
                  {"key":"refuse","label":"Refuse the drawdown to protect the commissioning phase, and direct the contractor to absorb or claim elsewhere","quality":25,
                   "consequence":"An evidenced claim refused without analysis sours the contractor relationship the commissioning phase depends on — and the refusal was based on the same stale exposure figure as an approval would have been.",
                   "principle":"A decision made against bad data is bad in both directions — refusal is not the safe error."}]}],
             "hints":["Ask when the remaining-exposure figure was last re-scored — and what changed since.",
               "The value engineering in month 3 changed the commissioning risk profile; check whether the register noticed.",
               "Fix the estimate of what contingency must still cover before deciding what it can afford to release."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a 40% contingency drawdown until the future it was measured against had been re-priced."}
            """),

        ("WC-RSK-295", "The opportunity hidden in the delay", "Everyone is managing the slip. Nobody is reading it.",
            "Data Centres", "Programme Risk Analyst", "project_controls", "advanced", 8,
            """["risk_management","evidence_analysis","commercial_awareness"]""",
            """
            {"context":"The data-centre build has taken a twelve-week delay: the utility company has pushed back the permanent power connection. The programme has responded competently — resequencing fit-out, negotiating delay costs, updating forecasts. Reading the delay's paperwork alongside the market intelligence pack, you notice something nobody has raised: the utility's letter attributes its slip to 'reprioritisation of grid capacity works in the region' — and the market pack notes the region's other hyperscale campus has just paused its phase two. The client's commercial team, you know, abandoned an option on the adjacent plot last year because grid capacity was fully subscribed.",
             "evidence":[
               {"label":"Delay","value":"12 weeks — utility pushed back permanent power connection"},
               {"label":"Utility letter","value":"Cause: 'reprioritisation of grid capacity works in the region'"},
               {"label":"Market pack","value":"Neighbouring hyperscale campus has paused its phase two"},
               {"label":"History","value":"Client dropped adjacent-plot option last year — grid fully subscribed"}],
             "decisions":[
               {"key":"diagnose","prompt":"The signal worth escalating beyond the delay itself is:",
                "options":[
                  {"key":"capacity","label":"Grid capacity in the region may be un-subscribing — the neighbour's pause plus the utility's 'reprioritisation' suggests the constraint that killed the adjacent-plot option may be reopening, which is a strategic opportunity with a short window","quality":100,
                   "consequence":"Escalated to the client's commercial team, the enquiry confirms capacity has become available; the adjacent-plot option is re-secured at pre-boom terms weeks before the market notices — an outcome worth more than the delay cost.",
                   "principle":"A delay's stated cause is information about the world, not just about your schedule — read it as intelligence, not only as impact."},
                  {"key":"further","label":"The utility's slip may recur — 'reprioritisation' implies your connection could be deprioritised again, so the real issue is securing the new date contractually","quality":50,
                   "consequence":"Worth doing and duly done — but it is defensive value only; the strategic signal sitting in the same two documents expires unread while the contract letters are exchanged.",
                   "principle":"Protecting your date is table stakes; the rarer skill is noticing what the cause of the delay says about everything else."},
                  {"key":"cost","label":"The delay-cost negotiation is the priority — twelve weeks of prolongation is the largest quantified exposure on the table","quality":20,
                   "consequence":"The negotiation concludes acceptably, as it would have anyway; the capacity window closes with the opportunity unexamined, and the client later pays boom-market terms for expansion capacity that was briefly available at par.",
                   "principle":"The largest number on the table is not always the most valuable decision on the table."}]},
               {"key":"act","prompt":"You take the capacity signal forward by:",
                "options":[
                  {"key":"escalate","label":"Passing it to the client's commercial director as a time-boxed opportunity with the evidence assembled — the letter, the market note, the option history — and a recommended enquiry route that doesn't reveal the client's interest prematurely","quality":100,
                   "consequence":"The enquiry runs discreetly through the client's energy consultant; capacity is optioned inside a month. The programme's risk function is thereafter invited to things risk functions rarely see.",
                   "principle":"An opportunity is a risk with a deadline — it deserves the same evidence pack, owner and urgency as a threat."},
                  {"key":"register","label":"Logging it as an opportunity in the risk register for review at the next monthly meeting","quality":15,
                   "consequence":"The monthly cycle is four weeks; the window, it turns out, was three. The register entry is closed 'no longer applicable' — accurately.",
                   "principle":"Registers hold opportunities the way calendars hold sunsets — recording one is not the same as catching it."}]}],
             "hints":["Read the utility's stated cause as market information, not just schedule impact.",
               "Connect three artifacts: the reprioritisation, the neighbour's pause, and why the adjacent plot was dropped.",
               "Opportunities expire on their own clock — route this one to whoever can act inside its window."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found a land-and-power opportunity hiding inside a twelve-week delay letter — and moved it before the window shut."}
            """),

        ("WC-RSK-296", "Exposure the heat map understated", "Twenty greens, every one of them leaning on the same assumption.",
            "Public Sector Estates", "Programme Risk Analyst", "project_controls", "advanced", 10,
            """["risk_management","evidence_analysis","governance"]""",
            """
            {"context":"The schools estate programme's quarterly heat map looks healthy: of sixty risks across twelve school sites, only three are red, and the twenty structural-survey risks are all green — each site individually assessed as 'low likelihood of adverse survey findings'. Reading the survey contractor's methodology statement, you find all twenty assessments rest on the same desk-based assumption: that the 1960s system-built blocks across the estate used the standard concrete specification of their era. The programme's own technical note, filed separately, records that the estate's builder was later found — on another authority's estate — to have substituted a cheaper aggregate in that exact period. No individual site risk is wrong; the correlation between them is invisible on the map.",
             "evidence":[
               {"label":"Heat map","value":"60 risks, 3 red; all 20 structural-survey risks green"},
               {"label":"Methodology","value":"All 20 greens rest on one desk-based assumption: era-standard concrete spec"},
               {"label":"Technical note","value":"Same builder substituted cheaper aggregate on another authority's estate, same period"},
               {"label":"Presentation","value":"Correlation invisible — the map scores sites independently"}],
             "decisions":[
               {"key":"diagnose","prompt":"The finding that must reach the programme board is:",
                "options":[
                  {"key":"correlated","label":"The twenty greens are one risk wearing twenty disguises — a single shared assumption, contradicted by the programme's own technical note, correlates them; if it fails, it fails across the estate at once, and the true exposure is portfolio-scale, not site-scale","quality":100,
                   "consequence":"Two intrusive surveys, prioritised at the highest-occupancy sites, are commissioned as an assumption test; one finds the substituted aggregate. The remediation programme starts with a term's notice instead of an emergency closure.",
                   "principle":"Heat maps assume independence — the risks that break programmes are the correlated ones the map renders as reassuring confetti."},
                  {"key":"process","label":"The survey contractor's methodology is inadequate — desk-based assessment should be escalated to a procurement and assurance issue","quality":35,
                   "consequence":"A fair finding that triggers a methodology review reporting in eight weeks — while children sit in buildings whose shared assumption remains untested. The process fix matters; it is not the urgent object.",
                   "principle":"When an assumption might be false across an estate, test the assumption before you re-procure the assessor."},
                  {"key":"fine","label":"The map is sound — each site's assessment is individually defensible and the technical note concerns a different authority's estate","quality":0,
                   "consequence":"A routine refurbishment core-drill later finds the aggregate; the discovery arrives mid-term, unplanned, and closes three schools in a week — the exact correlated event the map said was twenty independent greens.",
                   "principle":"'Individually defensible' is how correlated exposure always presents — right up until it happens everywhere at once."}]},
               {"key":"act","prompt":"Your recommendation to the board:",
                "options":[
                  {"key":"test","label":"Commission targeted intrusive surveys at two or three sites selected to test the shared assumption directly — highest occupancy, most representative construction — with the twenty risks re-scored as one correlated exposure until results land","quality":100,
                   "consequence":"The board funds the surveys the same week; the correlated re-score also reshapes the contingency conversation, which had been sized off the map's cheerful independence.",
                   "principle":"The cheapest response to a shared assumption is a direct test of it at the few sites that answer for all."},
                  {"key":"all","label":"Recommend immediate intrusive surveys at all twelve sites — the exposure justifies estate-wide certainty","quality":40,
                   "consequence":"Six times the cost and four months of scheduling for information the first two surveys would have given; the funding wrangle delays the answer the targeted option delivered in three weeks.",
                   "principle":"When one assumption underlies everything, a few well-chosen tests buy nearly all the certainty at a fraction of the price."}]}],
             "hints":["Ask what all twenty green assessments have in common before trusting any of them individually.",
               "Cross-reference the methodology's shared assumption with the programme's own technical note on the builder.",
               "Correlated exposure is tested by attacking the assumption directly — a few sites can answer for the whole estate."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-read twenty independent greens as one correlated exposure — and tested the assumption before it tested the estate."}
            """),

        // ───────────── Daily Decisions · quality, governance, scope, commercial · advanced ─────────────

        ("WC-QLT-297", "The inspection that was signed, not done", "The record is perfect. The inspector was somewhere else that day.",
            "Joint Ventures", "Quality Director", "project_management", "advanced", 5,
            """["quality_management","governance","professional_ethics"]""",
            """
            {"context":"In the joint-venture delivery office, a junior engineer mentions — almost in passing — that a batch of steelwork inspection records was signed by an inspector who was, on the recorded dates, attending a training course in another city. The records are complete, plausible and closed. The steelwork has since been encased. The inspector is one of the JV partner's most experienced people; the partner's quality manager, told informally, replies that the inspections 'were done by his team under his supervision, and the signature reflects accountability, not attendance'.",
             "evidence":[
               {"label":"Records","value":"Batch of steel inspections — complete, signed, closed"},
               {"label":"Conflict","value":"Signatory verifiably elsewhere on the recorded dates"},
               {"label":"Status","value":"Steelwork since encased"},
               {"label":"Partner's line","value":"'Signature reflects accountability, not attendance'"}],
             "decisions":[
               {"key":"records","prompt":"You:",
                "options":[
                  {"key":"investigate","label":"Treat it as a records-integrity event, not a personnel spat: quarantine the batch's certification status, establish who physically inspected what, verify by alternative evidence where possible, and put the 'signature as accountability' doctrine itself in front of JV governance","quality":100,
                   "consequence":"Two of the nine inspections prove to have been genuinely performed by a competent deputy; the rest need NDT verification through access hatches — costly, but the JV's certification remains worth the paper it is printed on.",
                   "principle":"A signature that doesn't mean what it claims poisons every record that shares its format — the doctrine, not the individual, is the finding."},
                  {"key":"accept","label":"Accept the quality manager's explanation — supervision-based sign-off is common practice and the records are otherwise sound","quality":0,
                   "consequence":"The doctrine stands unexamined until a regulator's audit samples attendance against signatures across the whole project — and reads your informal awareness, dated today, as the moment the JV chose not to act.",
                   "principle":"'Common practice' is what every falsified record is called by the person defending it."},
                  {"key":"personnel","label":"Refer the inspector for disciplinary action and close the matter there","quality":25,
                   "consequence":"The individual is sanctioned; the encased steelwork's actual verification status remains unknown, and the practice — which was the partner's culture, not one person's shortcut — continues under other signatures.",
                   "principle":"Punishing the signature without verifying the steel answers the wrong question."}]}],
             "hints":["Separate the personnel question from the verification question — which one is safety-bearing?",
               "Ask what the signature is claiming, and whether any alternative evidence can stand in for it now.",
               "The partner's defence is a doctrine — test the doctrine at governance level, not in a corridor."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Quarantined a batch of perfect-looking inspection records — and put the signing doctrine, not just the signer, on trial."}
            """),

        ("WC-QLT-298", "A non-conformance on the critical path", "Fixing it properly costs the programme. Not fixing it costs more, later, maybe.",
            "River Crossings", "Senior Project Manager", "project_management", "advanced", 6,
            """["quality_management","schedule_analysis","professional_judgement"]""",
            """
            {"context":"On the river crossing, a pile cap has failed inspection: cover to reinforcement is below specification on one face. The designer's preliminary view is that structural adequacy is 'likely demonstrable by calculation' but durability over the 120-year design life 'requires assessment'. Break-out and recast costs eleven days — directly on the critical path, jeopardising the flood-season cofferdam removal date. The concession route (accept with calculation) costs three days of designer time. The contractor, who would bear the recast cost, is pressing hard for the concession; the flood-season constraint is real and immovable.",
             "evidence":[
               {"label":"Defect","value":"Pile cap cover below spec on one face"},
               {"label":"Designer","value":"Strength likely provable; 120-year durability 'requires assessment'"},
               {"label":"Recast","value":"11 days, on the critical path, threatens cofferdam removal before flood season"},
               {"label":"Concession","value":"3 days of designer assessment; contractor pressing for it"}],
             "decisions":[
               {"key":"nc","prompt":"You:",
                "options":[
                  {"key":"assess","label":"Run the concession assessment properly and in parallel prepare the recast: give the designer the three days for a durability verdict with no schedule thumb on the scale, mobilise break-out resources meanwhile, and let the assessment — not the flood calendar — make the call","quality":100,
                   "consequence":"The durability assessment lands honestly mixed: adequate with a protective coating and one inspection covenant added to the maintenance regime. The concession is granted on engineering terms, the recast crew stands down, and the file shows the schedule never voted.",
                   "principle":"Concessions are engineering decisions that happen under schedule pressure — the discipline is making sure the pressure funds the assessment's urgency, never its answer."},
                  {"key":"concede","label":"Grant the concession now on the preliminary view — the designer says 'likely demonstrable' and the flood window cannot wait for process","quality":0,
                   "consequence":"The durability question, never properly assessed, resurfaces at the 12-year principal inspection as chloride ingress on the substandard face; the remediation happens over water, at forty times the recast cost.",
                   "principle":"'Likely demonstrable' is a hypothesis, not a concession basis — durability defects bill the asset owner decades after the schedule pressure is forgotten."},
                  {"key":"recast","label":"Order the recast immediately — non-conformance on a 120-year structure is not negotiable","quality":35,
                   "consequence":"Principled but unexamined: the assessment would have granted a sound concession, and the eleven days push cofferdam removal into flood season — creating a genuine safety exposure to cure a durability question that had an engineering answer.",
                   "principle":"Automatic rejection is not rigour — it is the mirror image of automatic acceptance, and here it traded a paper risk for a real one."}]}],
             "hints":["Separate the strength question from the durability question — they have different evidence and different clocks.",
               "Buy the assessment time without losing the recast option: parallel preparation is what the critical path is for.",
               "Whatever is decided, the file must show engineering decided it — not the flood calendar."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held the line between a real flood constraint and a 120-year durability call — and let engineering, not the calendar, decide."}
            """),

        ("WC-QLT-299", "The audit trail with a missing week", "Seven days of records, gone. The work they covered, already built on.",
            "Justice Estates", "Programme Assurance Lead", "project_controls", "advanced", 9,
            """["quality_management","governance","evidence_analysis"]""",
            """
            {"context":"Preparing the courts modernisation programme for its gateway review, you discover the electronic quality-management system has a seven-day gap: a migration error in month 4 destroyed all inspection and test records for one week across every active site. The work of that week — including drainage runs now buried and first-fix electrical now boarded over — was inspected at the time; the records existed and were signed. They are simply gone, unrecoverable. The gateway review is in three weeks. The programme director suggests the gap 'need not feature' in the submission since the inspections genuinely happened.",
             "evidence":[
               {"label":"Gap","value":"7 days of QMS records destroyed by migration error, month 4, all sites"},
               {"label":"Affected work","value":"Includes buried drainage and boarded-over first fix"},
               {"label":"Reality","value":"Inspections genuinely performed and signed at the time"},
               {"label":"Director","value":"Suggests the gap 'need not feature' at the gateway"}],
             "decisions":[
               {"key":"gap","prompt":"You:",
                "options":[
                  {"key":"disclose","label":"Disclose the gap at the gateway with a completed response attached: the reconstruction already done (site diaries, photos, supplier test certificates re-collected), the residual list of work verifiable only by re-inspection or covenant, and the backup control that now makes the failure unrepeatable","quality":100,
                   "consequence":"The review notes the gap and commends the response; two drainage runs get CCTV re-surveys, the rest is covered by reconstructed evidence, and the programme's assurance rating survives on the strength of how it handled its own failure.",
                   "principle":"Gateways assess whether a programme can be trusted — and nothing demonstrates trustworthiness like a disclosed failure with a finished response."},
                  {"key":"silent","label":"Follow the director's suggestion — the inspections happened, the gap is an IT artefact, and volunteering it invites disproportionate scrutiny","quality":0,
                   "consequence":"A reviewer's routine sample requests records from the missing week within the first hour; the gap becomes a concealment, the concealment becomes the finding, and the programme's leadership — not its records — is now the assurance question.",
                   "principle":"Reviews forgive what fails; they do not forgive what hides — and samples land where they land."},
                  {"key":"delay","label":"Request the gateway be postponed until the records question is fully resolved","quality":20,
                   "consequence":"The postponement itself must be explained, and 'records issue' without the response attached reads worse than the disclosure would have; the programme buys eight weeks of suspicion to avoid one uncomfortable page.",
                   "principle":"Postponing scrutiny converts a technical failure into a governance story."}]}],
             "hints":["The inspections happened; the evidence didn't survive — name which problem you actually have.",
               "Reconstruction first, disclosure with the response attached: reviews judge the handling, not just the hole.",
               "Ask where a routine evidence sample would land, and what that means for any 'need not feature' strategy."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Took a seven-day hole in the audit trail to the gateway myself — with the reconstruction already done."}
            """),

        ("WC-GOV-300", "The stage gate that split the board", "Half the board says proceed. Half says stop. The gate says neither.",
            "Framework Programmes", "Programme Director", "project_management", "advanced", 8,
            """["governance","decision_quality","stakeholder_communication"]""",
            """
            {"context":"The framework programme's tranche-two gate has split the investment board three against three. The proceed camp points to delivery evidence: tranche one landed within tolerance and the supply chain is mobilised. The stop camp points to the benefits ledger: tranche one's benefits are 'on track' only because their measurement dates were moved twice. The gate criteria — written three years ago — genuinely support both readings: delivery criteria are met, benefits criteria are 'partially evidenced'. The chair, who holds the casting vote, asks you privately what the gate is actually for.",
             "evidence":[
               {"label":"Split","value":"3–3: delivery evidence vs benefits scepticism"},
               {"label":"Delivery case","value":"Tranche one within tolerance; supply chain mobilised"},
               {"label":"Benefits case","value":"'On track' only via twice-moved measurement dates"},
               {"label":"Criteria","value":"Written 3 years ago; genuinely support both readings"}],
             "decisions":[
               {"key":"gate","prompt":"Your counsel to the chair:",
                "options":[
                  {"key":"conditional","label":"Recommend a conditional proceed that resolves the actual dispute: tranche two funds in two releases, the second contingent on tranche-one benefits measured at the original dates by an owner outside the delivery team — and the gate criteria updated so 'partially evidenced' can never again mean both yes and no","quality":100,
                   "consequence":"The board votes five to one for the structure; the independent measurement lands mixed — two benefits real, one illusory — and the second release proceeds re-scoped, which is the outcome the gate existed to produce.",
                   "principle":"A split board is usually arguing about evidence quality, not direction — the gate's job is to buy the missing evidence, not to guess."},
                  {"key":"proceed","label":"Advise proceeding cleanly — delivery is the harder half to prove and it is proven; benefits always lag and the supply chain cannot hold","quality":15,
                   "consequence":"Tranche two mobilises on the moved dates' comfort; the illusory benefit is discovered at tranche three's gate, now with twice the sunk cost and a board that remembers who advised waving it through.",
                   "principle":"Benefits that are only 'on track' because the track was moved are the gate's business — that is the difference between a gate and a milestone."},
                  {"key":"stop","label":"Advise stopping until benefits are fully evidenced at the original measurement dates","quality":30,
                   "consequence":"Defensible in the file, expensive in the world: the mobilised supply chain demobilises, the restart costs eight months, and the eventual measurement shows two of three benefits were real — a stop that a conditional structure would have made unnecessary.",
                   "principle":"When evidence is partial, the options are rarely just go and stop — gates can buy information as well as grant passage."}]}],
             "hints":["Name what the two camps are really disagreeing about — direction, or evidence quality?",
               "Twice-moved measurement dates are themselves evidence — of what?",
               "Look for the gate structure that funds delivery while forcing the missing measurement to happen."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a 3–3 gate deadlock into a two-release structure that bought the evidence both camps were missing."}
            """),

        ("WC-SCO-304", "The provision nobody priced", "Forty megawatts of solar, and one sentence no party thinks is theirs.",
            "Renewable Energy", "Senior Project Manager", "project_management", "advanced", 11,
            """["scope_management","requirements_management","stakeholder_communication"]""",
            """
            {"context":"Across the solar portfolio's five sites, the employer's requirements contain one sentence: 'The works shall include provision for future battery storage integration.' Site one is nearing design freeze, and you discover no party has scoped it: the EPC contractor priced 'provision' as spare duct routes; the grid consultant assumed it meant DC-coupling readiness at the inverters — a materially different design; the client's energy strategist, who wrote the sentence, meant reserved land, export capacity headroom and a consented substation footprint. Three interpretations, three prices, and design freeze in three weeks. The delta between cheapest and fullest reading is significant at portfolio scale.",
             "evidence":[
               {"label":"Requirement","value":"'Provision for future battery storage integration' — one sentence, five sites"},
               {"label":"EPC reading","value":"Spare duct routes"},
               {"label":"Grid consultant reading","value":"DC-coupling readiness at the inverters"},
               {"label":"Author's intent","value":"Reserved land + export headroom + consented substation footprint"}],
             "decisions":[
               {"key":"req","prompt":"With design freeze in three weeks, you:",
                "options":[
                  {"key":"define","label":"Force the requirement to be owned and decided before freeze: convene the author, funder and both designers to convert the sentence into measurable acceptance criteria, price the readings as explicit options, and have the client choose — recording what 'provision' now means and what it deliberately excludes","quality":100,
                   "consequence":"The client, seeing the price of each reading for the first time, chooses land-plus-headroom and drops DC-coupling; site one freezes on time with a requirement that finally means one thing, and sites two through five inherit the definition instead of the ambiguity.",
                   "principle":"An unowned requirement is a dispute with a delivery date — the cheapest moment to give it one meaning is before anyone's design hardens around a different one."},
                  {"key":"cheapest","label":"Let the EPC's duct-route reading stand — it is priced, it is in the contract's spirit, and freeze cannot wait for a portfolio workshop","quality":0,
                   "consequence":"Two years on, the storage retrofit finds no export headroom and no substation land; 'provision' is relitigated as a claim, and the retrofit costs at site one alone exceed what the fullest reading would have cost across the portfolio.",
                   "principle":"The cheapest interpretation of an ambiguous requirement is a loan from the future at compound interest."},
                  {"key":"freeze_later","label":"Delay site one's design freeze until the requirement is resolved portfolio-wide","quality":30,
                   "consequence":"The freeze slips six weeks and the EPC claims prolongation — for a resolution that, run with urgency, fitted inside the original three; the portfolio pays schedule for what was really a decision-making failure.",
                   "principle":"Ambiguity is resolved by forcing a decision, not by stopping the clock while nobody makes one."}]}],
             "hints":["Get all three interpretations priced and on one page — ambiguity survives best unpriced.",
               "Find who owns the sentence: the author's intent, the funder's appetite and the designers' assumptions are different things.",
               "The record of what 'provision' excludes is as valuable as what it includes — future claims live in the gap."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Converted one ambiguous sentence into a priced, owned decision three weeks before it froze into five designs."}
            """),

        ("WC-GOV-301", "Benefits on paper, pressure in the room", "The realisation report is due. The realisation isn't.",
            "Enterprise Transformation", "Programme Director", "project_management", "advanced", 9,
            """["governance","benefits_management","professional_ethics"]""",
            """
            {"context":"The enterprise transformation's benefits realisation report goes to the executive committee next week. The automation workstream's headline benefit — 340 roles' worth of effort released — exists on paper: the processes are live and the effort is genuinely released, but the business units have quietly reabsorbed the freed capacity into unbudgeted local work rather than the cost reduction the business case promised. The CFO's office has signalled it expects the report to 'confirm the run-rate saving'. Your benefits manager has drafted two versions: one reports effort released (true, flattering), one reports cost actually removed (true, damning).",
             "evidence":[
               {"label":"Business case","value":"340 roles of effort → run-rate cost reduction"},
               {"label":"Reality","value":"Effort genuinely released; capacity reabsorbed into unbudgeted local work"},
               {"label":"CFO signal","value":"Expects the report to 'confirm the run-rate saving'"},
               {"label":"Drafts","value":"Version A: effort released. Version B: cost removed."}],
             "decisions":[
               {"key":"report","prompt":"You submit:",
                "options":[
                  {"key":"both","label":"One report carrying both truths as one finding: capacity released as designed, harvest not occurring — with the reabsorption quantified by unit, the decision the executive actually owns (harvest, redeploy formally, or revise the case) stated plainly, and neither draft's flattery nor its blame","quality":100,
                   "consequence":"The committee meeting is uncomfortable and productive: two units get headcount reductions, one gets its reabsorbed work formally funded because it turned out to matter, and the benefits framework gains a harvest-decision step every future case will use.",
                   "principle":"Benefits reporting exists to force the harvest decision — a report that lets 'released' impersonate 'realised' has abolished its own purpose."},
                  {"key":"flatter","label":"Submit the effort-released version — it is true, the committee expects it, and the harvest question can be managed offline with the units","quality":0,
                   "consequence":"The run-rate saving is booked into next year's budget on the report's strength; when the accounts show costs flat, the gap between reported and real lands as a finance investigation with your signature at its start.",
                   "principle":"A true statement selected to create a false impression is not a reporting choice, it is the mechanism of every benefits scandal on record."},
                  {"key":"damn","label":"Submit the cost-removed version — the hard number is the honest one and the units deserve the pressure","quality":35,
                   "consequence":"Technically honest, strategically lazy: the report reads as failure without explaining that the capacity exists and the decision is harvestable; the committee cuts the next tranche of a programme whose benefits were one governance decision from real.",
                   "principle":"Reporting the damning number without the decision that could redeem it is candour without usefulness."}]}],
             "hints":["Both drafts are true — ask what impression each is selected to create.",
               "The missing object is a decision, not a number: who chooses whether released capacity becomes saving?",
               "Quantify the reabsorption by unit — the committee can only harvest what it can see."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Refused the choice between a flattering truth and a damning one — and reported the decision the executive actually owned."}
            """),

        ("WC-SCO-305", "A deliverable with three definitions", "Everyone agreed to deliver it. No two contracts agree on what it is.",
            "Urban Development", "Senior Project Manager", "project_management", "advanced", 10,
            """["scope_management","contract_management","stakeholder_communication"]""",
            """
            {"context":"The mixed-use development's 'public realm package' appears in three contracts: the infrastructure contractor's (hard landscaping and drainage to adoptable standard), the residential builder's (streetscape 'to marketing suite quality' around its plots), and the commercial fit-out contract ('activation-ready' public space at the retail frontage). Walking the interfaces, you find a strip of land — the spine street's eastern footway — that all three definitions arguably include and all three contractors have excluded from their programmes, each believing another owns it. Utilities want to open the ground there in five weeks; whoever owns the footway must coordinate them.",
             "evidence":[
               {"label":"Definitions","value":"Adoptable standard vs 'marketing suite quality' vs 'activation-ready' — three contracts"},
               {"label":"Gap","value":"Eastern footway: arguably in all three, programmed by none"},
               {"label":"Belief","value":"Each contractor assumes another owns it"},
               {"label":"Clock","value":"Utilities open the ground in five weeks; owner must coordinate"}],
             "decisions":[
               {"key":"gap","prompt":"You:",
                "options":[
                  {"key":"assign","label":"Close the gap by decision, not discovery: map the three definitions onto one interface drawing, assign the footway to the infrastructure contractor by instruction (its scope is objectively closest), price the variation honestly, and fix the definition boundaries for every remaining interface in the same exercise","quality":100,
                   "consequence":"The variation costs real money and the utilities window is met; the interface drawing surfaces two more orphaned strips nobody had noticed, both assigned while they are still cheap paper changes rather than site standoffs.",
                   "principle":"Scope gaps between contracts are closed by an instruction someone pays for — the alternative is closing them by dispute, which costs the same money plus the delay."},
                  {"key":"negotiate","label":"Get the three contractors in a room to agree ownership among themselves — they know the interfaces best","quality":20,
                   "consequence":"Three commercial teams, each with a contract that lets them decline, decline; the meeting produces an action to 'review definitions', the utilities date arrives unowned, and the footway is trenched with no reinstatement standard agreed.",
                   "principle":"Contractors cannot give each other scope none of them has priced — gaps are the client side's to assign, with money attached."},
                  {"key":"split","label":"Split the footway three ways at the natural frontage boundaries so each definition governs its own stretch","quality":35,
                   "consequence":"Contractually tidy, physically absurd: one footway built to three specifications by three programmes, with two new interfaces where there had been none — and the utilities coordination still needs the single owner the split just abolished.",
                   "principle":"A linear asset wants one owner — subdividing a gap multiplies the interfaces that created it."}]}],
             "hints":["Draw the three definitions on one plan — the gap is only visible where the words become geometry.",
               "Ask which contract's scope is objectively closest, and what an instruction plus variation costs against a standoff.",
               "The footway is a symptom: audit every interface for more orphaned scope while the exercise is open."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the strip of land three contracts described and nobody owned — and assigned it before the ground opened."}
            """),

        ("WC-PRC-308", "The bid that was too good", "Eighteen percent under the next bidder. The evaluation says compliant. Your experience says something else.",
            "Capital Programmes", "Commercial Director", "project_finance", "advanced", 11,
            """["procurement","commercial_awareness","risk_management"]""",
            """
            {"context":"The multi-site capital programme's main works tender has returned five bids. The lowest is 18% under the next bidder and 22% under the pre-tender estimate — from a capable, solvent contractor. The evaluation panel scores it compliant and technically sound; procurement rules point to award. Line-by-line comparison shows the gap concentrates in preliminaries and design development allowances — the areas where a contractor who intends to claim aggressively would bid thin. The contractor's recent history on a neighbouring authority's framework includes two projects that ended in substantial settled claims. Award recommendation is due to the board in a week.",
             "evidence":[
               {"label":"Spread","value":"Low bid: −18% vs next, −22% vs estimate; bidder capable and solvent"},
               {"label":"Concentration","value":"Gap sits in preliminaries and design development allowances"},
               {"label":"History","value":"Two recent projects elsewhere ended in substantial settled claims"},
               {"label":"Process","value":"Scored compliant; award recommendation due in one week"}],
             "decisions":[
               {"key":"award","prompt":"Your recommendation:",
                "options":[
                  {"key":"probe","label":"Use the abnormally-low-tender procedure before any award: require the bidder to substantiate the thin areas line-by-line against a resourced programme, verify the allowances can deliver the design development the employer's requirements actually need, and put the claims history in front of the panel as context for reading the answers","quality":100,
                   "consequence":"The substantiation meeting is revealing: the bidder holds its price on preliminaries but concedes the design allowance assumed employer-led novation it was never offered. The corrected bid is still lowest — by 6% — and the award proceeds on a price that means what it says.",
                   "principle":"An abnormally low bid is a question, not a gift — the procedure exists to make the bidder answer it before award, when you still have leverage."},
                  {"key":"take","label":"Award as evaluated — the bid is compliant, the bidder is solvent, and second-guessing a completed evaluation exposes the authority to challenge","quality":0,
                   "consequence":"Mobilisation is smooth; the claims begin at month 5, precisely from the thin allowances, prosecuted by a contractor whose real tender was the claims strategy. Outturn passes the second bidder's price by year two.",
                   "principle":"The cheapest bid and the cheapest project are different things — buying a claims strategy at a discount is still buying a claims strategy."},
                  {"key":"exclude","label":"Recommend passing over the low bid on the strength of the claims history and the implausible pricing","quality":20,
                   "consequence":"Exclusion without due process invites a procurement challenge the authority loses; the re-run costs six months, and the tribunal notes the panel scored the bid compliant before commercial instinct overrode it.",
                   "principle":"Suspicion is grounds for scrutiny, never for exclusion — the procedure converts instinct into either evidence or award."}]}],
             "hints":["Locate where the 18% actually lives — a genuine efficiency spreads; a strategy concentrates.",
               "Thin preliminaries and design allowances are the classic launch pads for claim-led delivery.",
               "The abnormally-low-tender procedure is the lawful middle path between naive award and unlawful exclusion."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Made an 18%-low bidder substantiate its price before award — and found the assumption hiding in the discount."}
            """),

        ("WC-GOV-302", "One sponsor, two mandates", "The same executive is telling two boards two different stories. Both are yours.",
            "Portfolio Management", "Portfolio Director", "project_management", "advanced", 12,
            """["governance","stakeholder_communication","professional_ethics"]""",
            """
            {"context":"Your portfolio's sponsoring executive chairs two boards. At the investment committee, she presents the data-platform programme as the portfolio's cost-reduction engine — its case rests on decommissioning forty legacy systems. At the divisional board, she assures the operating divisions that 'no system any division depends on will be switched off without its agreement' — an assurance the divisions are treating as a veto. Both statements are minuted. The programme cannot deliver its business case if even a quarter of the divisions exercise the veto, and the first decommissioning wave is due in four months. You report to her.",
             "evidence":[
               {"label":"Investment committee","value":"Case rests on decommissioning 40 legacy systems"},
               {"label":"Divisional board","value":"'Nothing switched off without divisional agreement' — read as a veto"},
               {"label":"Maths","value":"A quarter of divisions vetoing breaks the business case"},
               {"label":"Clock","value":"First decommissioning wave in 4 months; she is your boss"}],
             "decisions":[
               {"key":"sponsor","prompt":"You:",
                "options":[
                  {"key":"confront","label":"Take the contradiction to her privately, framed as arithmetic rather than accusation: the two minuted positions cannot both hold at wave one, here is the exposure by division, and here are the honest options — a governed exception process instead of a veto, or a re-based case that prices the assurance she gave","quality":100,
                   "consequence":"She chooses the exception process and — critically — announces the reconciliation herself at both boards, converting a brewing credibility crisis into an ordinary governance design. Wave one proceeds with two negotiated deferrals instead of nine vetoes.",
                   "principle":"When a sponsor holds contradictory positions, the kindest and safest act is showing them the collision before their boards do — arithmetic in private beats exposure in public."},
                  {"key":"drift","label":"Let it run — sponsors manage their own politics, and the contradiction may resolve itself as divisions engage with wave one","quality":0,
                   "consequence":"Wave one surfaces the collision in the worst room: a division cites her assurance at the investment committee itself; the case unravels publicly, and her recollection of who knew what, and when, features you.",
                   "principle":"A contradiction you can quantify and chose not to raise becomes yours the day it detonates."},
                  {"key":"boards","label":"Brief the investment committee's secretariat on the exposure so governance can resolve what governance created","quality":20,
                   "consequence":"Procedurally defensible, relationally fatal: she learns her own portfolio director escalated around her; the contradiction gets fixed and your effectiveness with her ends the same week.",
                   "principle":"Going around a sponsor is the last resort after they have declined to act — not the first move before they have been told."}]}],
             "hints":["Put both minuted statements side by side and do the arithmetic of the veto.",
               "The frame that works is exposure and options, not inconsistency and blame.",
               "Whoever announces the reconciliation owns the credibility — make sure it can be her."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Showed a sponsor her two minuted promises colliding — four months before her boards would have."}
            """),

        ("WC-SCO-306", "The scope statement that meant four things", "One paragraph, four readings, and the money is already allocated.",
            "Flood Defence", "Senior Project Manager", "project_management", "advanced", 9,
            """["scope_management","requirements_management","governance"]""",
            """
            {"context":"The flood defence scheme's funding approval contains the sentence: 'The scheme shall provide a 1-in-100-year standard of protection to the community, including betterment of existing assets where practicable.' Four parties have four readings. The funding body counts 340 properties as 'the community'; the council's bid counted 410 by including a planned housing allocation. 'Betterment where practicable' is being read by the operations team as like-for-like refurbishment, and by the community's flood action group — who have seen the sentence — as a commitment to raise every existing wall. Detailed design starts in a month, and the design brief must state numbers.",
             "evidence":[
               {"label":"Sentence","value":"'1-in-100 protection to the community, including betterment where practicable'"},
               {"label":"Community","value":"Funder counts 340 properties; council bid counted 410 with housing allocation"},
               {"label":"Betterment","value":"Ops read: like-for-like refurb. Action group read: raise every wall"},
               {"label":"Clock","value":"Design brief must state numbers in one month"}],
             "decisions":[
               {"key":"scope","prompt":"You:",
                "options":[
                  {"key":"resolve","label":"Resolve the words before the design does: a decision paper to the funder and council fixing the property count and defining 'practicable' as a stated cost-benefit test, then a community session presenting what the scheme will and will not do — before the brief is written, so the design opens against one reading","quality":100,
                   "consequence":"The funder holds at 340 with the housing allocation listed as a priced future phase; 'practicable' becomes a transparent test that raises eleven walls and refurbishes the rest. The action group dislikes the answer and respects the honesty — and the brief states numbers nobody can relitigate.",
                   "principle":"A scope statement that can mean four things will be built to one and litigated by the other three — the design brief is the last cheap place to make it mean one."},
                  {"key":"design","label":"Let detailed design proceed on the operations team's reading — it is the fundable one, and the other interpretations can be managed as the scheme progresses","quality":0,
                   "consequence":"The action group discovers the like-for-like reading at the planning consultation and mobilises; the housing-allocation gap surfaces in the local press as 'homes left unprotected'; the scheme spends a year in objections that one early decision paper would have prevented.",
                   "principle":"Ambiguity managed 'as the scheme progresses' is ambiguity discovered by the public at the worst moment."},
                  {"key":"maximal","label":"Design to the fullest reading — 410 properties, all walls raised — and let the funder cut it back if unaffordable","quality":15,
                   "consequence":"The maximal design overshoots the funding envelope by a third; the funder's cutback lands late, redesign consumes the flood-season deadline, and the community watches promised walls vanish from drawings — worse than never having drawn them.",
                   "principle":"Designing beyond the funding to force a decision doesn't force a decision — it forces a redesign."}]}],
             "hints":["List the four readings against who holds each — and what each costs.",
               "Two ambiguities hide in one sentence: the population served, and the meaning of 'practicable'.",
               "Fix the words with the funder first, then face the community with one honest answer — that order matters."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Made one four-way-ambiguous sentence mean one thing — a month before it became concrete."}
            """),

        ("WC-PRC-309", "A variation priced under duress", "Sign by Friday or the ship sails. The price assumes you can't check it.",
            "Ports & Marine", "Commercial Manager", "project_finance", "advanced", 8,
            """["procurement","contract_management","negotiation"]""",
            """
            {"context":"The port expansion's dredging contractor has tabled a variation: additional rock encountered, priced at a lump sum, with a condition — agreement by Friday, because its specialist cutter-suction dredger demobilises to another hemisphere on Monday and re-mobilisation would cost multiples of the variation. The technical facts are real: rock was encountered and the dredger is booked elsewhere. But the lump sum is three times what your quantity surveyor's first-principles build-up suggests, the contractor has declined to provide substantiation 'in the time available', and the contract entitles you to records of actual quantities. The Friday deadline is engineered to prevent exactly the analysis the price cannot survive.",
             "evidence":[
               {"label":"Variation","value":"Additional rock — real. Lump sum: ~3× QS first-principles build-up"},
               {"label":"Deadline","value":"Agree by Friday or dredger demobilises; re-mob costs multiples"},
               {"label":"Substantiation","value":"Declined 'in the time available'; contract entitles you to quantity records"},
               {"label":"Structure","value":"Deadline engineered against analysis"}],
             "decisions":[
               {"key":"variation","prompt":"You:",
                "options":[
                  {"key":"decouple","label":"Split the decision the deadline welded together: instruct the work to continue under the contract's valuation-by-records mechanism — securing the dredger past Friday — while expressly reserving the price for measurement, and record that the lump sum was declined precisely because substantiation was withheld","quality":100,
                   "consequence":"The dredger stays — its Monday booking, it emerges, had a fortnight's float the deadline never mentioned. Measured valuation lands near the QS build-up, and the contractor's next variation arrives with substantiation attached, unprompted.",
                   "principle":"An engineered deadline welds a work decision to a price decision — the contract's valuation machinery exists to cut them apart."},
                  {"key":"sign","label":"Sign the lump sum — the re-mobilisation cost dwarfs the possible over-payment and Friday is real","quality":0,
                   "consequence":"The premium is banked, and the method is validated: three further 'deadline variations' arrive over the following year, each engineered against analysis, each citing the first as precedent for how this project does business.",
                   "principle":"Paying a duress premium once purchases every future duress premium — counterparties reprice what worked."},
                  {"key":"callbluff","label":"Refuse outright and let the dredger sail — no variation signed without full substantiation, whatever the consequence","quality":25,
                   "consequence":"Principled, but the principle had a cheaper vehicle: the work genuinely needed doing, the demobilisation is real enough to hurt, and re-mobilisation costs land on a project that had a contractual route to keep the vessel and the analysis both.",
                   "principle":"When the contract offers a mechanism that defeats the duress, refusal is not the strong move — it is the expensive one."}]}],
             "hints":["Separate what must be decided by Friday (the work) from what needn't be (the price).",
               "Find the contract's mechanism for proceeding at a valuation to be measured — that is what it is for.",
               "Record why the lump sum was declined; the substantiation refusal is your evidence, not your problem."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept the dredger and refused the ransom — by cutting the work decision loose from the price."}
            """),

        ("WC-GOV-303", "The programme that outgrew its case", "Every gate passed. Every assumption dead. Nobody has asked the only question.",
            "Joint Ventures", "Programme Director", "project_management", "advanced", 11,
            """["governance","benefits_management","decision_quality"]""",
            """
            {"context":"Closing out the joint-venture delivery office's five-year programme, you assemble the final review. Delivery is genuinely strong: every gate passed, tolerances held. But laying the original business case beside the world of today is uncomfortable: the case's core assumption — that the two parent organisations would merge their operations onto the delivered platform — was quietly abandoned in year three when one parent was acquired. The platform runs; one parent uses 80% of it, the other 15% and falling. Continuation funding for a 'phase two' has momentum: teams are in place, the pipeline is drafted, both parents' delivery directors support it. The closure report you sign will either normalise phase two or force the question no gate ever asked: what is this programme now for?",
             "evidence":[
               {"label":"Delivery record","value":"All gates passed, tolerances held, platform live"},
               {"label":"Dead assumption","value":"Parent-merger premise abandoned in year 3 (acquisition)"},
               {"label":"Usage","value":"Parent A: 80%. Parent B: 15% and falling"},
               {"label":"Momentum","value":"Phase two staffed, pipelined, supported by both delivery directors"}],
             "decisions":[
               {"key":"closure","prompt":"Your closure report:",
                "options":[
                  {"key":"question","label":"Reports the delivery success honestly and then refuses to let it answer the strategic question: state that the case's premise died in year three, quantify the usage asymmetry, and make phase-two funding conditional on a fresh business case owned by the parents' executives — not the delivery organisation that wants to continue existing","quality":100,
                   "consequence":"The fresh case is smaller and honest: parent A funds a scaled continuation for its own operations; parent B exits cleanly with a licensed arrangement. Phase two as drafted never happens — because it was a staffing plan wearing a strategy.",
                   "principle":"Delivery organisations are structurally incapable of recommending their own dissolution — closure reports exist to hand that question back to the people who own the money."},
                  {"key":"continue","label":"Endorses phase two — the capability is built, the teams perform, and dismantling a working delivery engine to relitigate a five-year-old assumption wastes the programme's real asset","quality":10,
                   "consequence":"Phase two runs three years on institutional momentum; parent B's usage reaches 4% before its new owner forces the exit question the closure report declined to ask — with three more years of its money spent.",
                   "principle":"A working engine attached to a dead purpose is not an asset — it is the most expensive kind of liability, the kind that passes its gates."},
                  {"key":"neutral","label":"Reports delivery performance only — strategy questions belong to the parents, and the closure report should stay in its lane","quality":30,
                   "consequence":"Technically correct and practically decisive: with no report raising the question, no forum ever does; phase two proceeds by default, which is the outcome 'staying in your lane' always quietly selects.",
                   "principle":"When a report is the only document that could raise a question, declining to raise it is not neutrality — it is a decision wearing modesty."}]}],
             "hints":["Lay the original case's core assumption beside today's reality and date its death.",
               "Ask who benefits from phase two proceeding by default — and who owns the money.",
               "Success at delivery and validity of purpose are separate audits; the closure report must run both."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Signed a closure report that praised the delivery and put the programme's reason to exist back on trial."}
            """),

        ("WC-SCO-307", "Where the fit-out ends and the asset begins", "Turnaround scope and capital scope share a plant, a shutdown, and no boundary.",
            "Energy & Process", "Senior Project Manager", "project_management", "advanced", 16,
            """["scope_management","interface_management","cost_management"]""",
            """
            {"context":"The refinery turnaround — maintenance-funded, 28 days, schedule-critical — shares its shutdown window with your capital project: a new hydrotreater tie-in. Walking the plot with both scope registers, you find the boundary between the two is folklore, not documentation. Six work packages sit in the gap: a pipe-rack extension the turnaround assumes is capital, valve replacements capital assumes are maintenance, scaffolding both scopes claim (and both have priced), and insulation neither has. The turnaround's cost is opex, expensed this year; the capital project capitalises over decades — so every gap item is also an accounting classification question the finance team has strong views about. The shutdown starts in six weeks; unowned scope discovered during it will be priced at shutdown rates.",
             "evidence":[
               {"label":"Boundary","value":"Turnaround vs capital scope: undocumented, six packages in the gap"},
               {"label":"Gap contents","value":"Pipe-rack extension, valve sets, scaffolding (double-priced), insulation (unpriced)"},
               {"label":"Money","value":"Opex vs capex classification attaches to every item"},
               {"label":"Clock","value":"Shutdown in 6 weeks; gap scope found during it costs shutdown rates"}],
             "decisions":[
               {"key":"boundary","prompt":"You:",
                "options":[
                  {"key":"matrix","label":"Build the boundary as a signed artifact in the next fortnight: a joint scope-boundary matrix listing every interface item with one owner, one funding classification agreed with finance, and the double-priced scaffolding converted to a shared-services package — then walk the plot once more against the signed matrix before the shutdown freeze","quality":100,
                   "consequence":"The matrix surfaces nine gap items, not six; finance rules on each classification in one sitting instead of six arguments; the second walk-down catches an orphaned tie-in spool that would have been a day-three shutdown discovery at ten times the price.",
                   "principle":"Scope boundaries between funding regimes must exist as signed documents before the work mixes — during a shutdown, every unowned item is priced by whoever is standing nearest."},
                  {"key":"field","label":"Resolve the gap items pragmatically during the shutdown — the teams overlap on site and can allocate work as it arises","quality":0,
                   "consequence":"Day-to-day allocation works until the insulation gap idles a critical-path crew for two shifts while managers argue funding codes; the auditors later unwind three field allocations that breached capitalisation rules, restating the project's cost base.",
                   "principle":"'Pragmatic in the field' means decisions made under the highest time pressure by the people with the least authority to make them."},
                  {"key":"absorb","label":"Take all six gap items into the capital project's scope — it has contingency, and ownership arguments waste the six weeks","quality":25,
                   "consequence":"Fast, and doubly wrong: the maintenance valve sets breach capitalisation rules the auditors enforce with restatement, and the capital contingency — spent buying peace — is missing at commissioning when it was needed for actual risk.",
                   "principle":"Buying boundary peace with contingency solves the argument and corrupts both the accounts and the risk cover in one move."}]}],
             "hints":["Walk the plot with both registers open — the boundary only exists where the documents disagree.",
               "Every gap item carries two questions: who does the work, and which money — answer both, in writing.",
               "Double-priced scope is as dangerous as unpriced: two crews, one scaffold, one shutdown clock."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a folklore boundary between opex and capex into a signed matrix — six weeks before the shutdown would have priced it."}
            """),

        ("WC-CST-310", "The forecast that flattered the month", "The cost report is technically defensible. Next month it won't be.",
            "Stadia & Venues", "Cost Manager", "project_finance", "advanced", 15,
            """["cost_management","forecasting","professional_ethics"]""",
            """
            {"context":"Closing out the stadium fit-out's monthly cost report, you review the forecast-at-completion. This month's figure holds within budget — but only through three choices stacked in the same direction: the seating contractor's tabled claim is carried at the lowest of three assessed outcomes; the provisional sums for the hospitality areas remain at tender values despite every package so far settling above them; and September's adverse escalation indices arrived two days after cut-off, so the forecast still uses August's. Each choice is individually defensible. Together they conceal a probable overrun that next month's report cannot avoid revealing — after the client's board approves the operator's launch-marketing budget against this month's number.",
             "evidence":[
               {"label":"Claim","value":"Carried at lowest of three assessed outcomes"},
               {"label":"Provisional sums","value":"Held at tender despite every settlement above tender"},
               {"label":"Indices","value":"Adverse September data arrived after cut-off; August's used"},
               {"label":"Timing","value":"Board approves launch-marketing spend against this month's figure"}],
             "decisions":[
               {"key":"fac","prompt":"You:",
                "options":[
                  {"key":"restate","label":"Re-cast the forecast at expected values — mid-case claim, settlement-trend provisional sums, September indices — and present this month's figure with the movement explained as three corrections, before the board commits spending against the flattering version","quality":100,
                   "consequence":"The report shows the overrun eight weeks before it was unavoidable; the board trims the launch budget and re-scopes two hospitality packages while options exist. The client's cost confidence drops for a month and compounds for years.",
                   "principle":"A forecast is a probability statement, not a negotiating position — three defensible optimisms stacked in one direction are a single indefensible bias."},
                  {"key":"hold","label":"Publish as prepared — each treatment follows the reporting rules, and next month's data will justify next month's movement","quality":0,
                   "consequence":"The board commits the launch budget; next month's forecast jumps in one report by the full concealed amount, and the client's forensic review of 'what was known when' finds the September indices in your inbox, dated before publication.",
                   "principle":"Technically defensible line items do not defend a report whose author knew its total was wrong."},
                  {"key":"partial","label":"Update the indices — that one is unambiguous data — but hold the claim and provisional-sum treatments to avoid over-correcting in a single month","quality":30,
                   "consequence":"Half the truth arrives this month and the other half next; the two-step reveal reads as either incompetence or drip-fed bad news, and the board makes its launch decision against a number still knowingly flattered.",
                   "principle":"Correcting the easiest third of a known bias is not caution — it is choosing which part of the truth can wait, on the reader's behalf, without telling them."}]}],
             "hints":["Test each treatment alone, then notice they all lean the same way — bias hides in the stacking.",
               "Ask what decision will be made against this number, and by whom, before next month's correction.",
               "Forecast at expected value and let the narrative explain the movement — the jump is coming either way; the only choice is whether it arrives with your explanation or without it."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Unstacked three defensible optimisms before the board spent against their total."}
            """),
    };
}
