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

        // ═════════════ NOVEMBER — integrated project controls and executive trade-offs ═════════════
        // ───────────── Daily Decisions · governance · advanced · deep ─────────────

        ("WC-GOV-312", "A delegation question with teeth", "The limits were written for a smaller programme. Today they bite a real decision.",
            "Framework Programmes", "Programme Director", "project_management", "advanced", 13,
            """["governance","decision_quality","delegation"]""",
            """
            {"context":"Your framework programme's delegation schedule authorises you to approve changes to £250k; above that, the quarterly investment board. Today a supplier's insolvency requires novating its three live packages within ten working days — total value £1.9m, but the novation itself costs £180k in assignment fees and re-warranting. Legal advice: delay past ten days and the administrator re-tenders the packages, costing months. The next board is five weeks away. The schedule's author left years ago; the vice-chair says by phone that 'the fees are the decision and they're within your limit'; the finance director, also by phone, says 'you are moving £1.9m of contractual obligation — that is a board matter'. Both will stand by their view in writing. The clock runs either way.",
             "evidence":[
               {"label":"Delegation","value":"You: £250k. Investment board: above. Next board: 5 weeks"},
               {"label":"Decision","value":"Novate 3 packages (£1.9m obligations) in 10 working days; direct fees £180k"},
               {"label":"Vice-chair","value":"'Fees are the decision — within your limit'"},
               {"label":"Finance director","value":"'£1.9m of obligation moves — board matter'"}],
             "decisions":[
               {"key":"authority","prompt":"You:",
                "options":[
                  {"key":"urgent_provision","label":"Use the governance the schedule actually provides for its own gaps: invoke the urgent-decision provision (chair's action or emergency written resolution of board members), present the novation with both readings of the limit stated, and obtain a decision at board authority inside the ten days — while recording the ambiguity for the schedule's revision","quality":100,
                   "consequence":"Written resolutions land in six days; the novation completes with board-level cover under either reading, and the delegation schedule gains an obligation-value test at its next revision — closing the gap the insolvency found.",
                   "principle":"When a limit is ambiguous, the safe harbour is the higher authority's confirmation — and mature schedules contain an urgent route to it precisely because clocks don't wait for quarters."},
                  {"key":"take_fees","label":"Proceed on the vice-chair's reading — the cash decision is £180k, it is within your limit, and ten days leaves no practical alternative","quality":15,
                   "consequence":"The novation holds until one package later fails, at which point the £1.9m reading is argued by insurers, and the phone-call authority you acted on has become 'a conversation the vice-chair recalls differently'.",
                   "principle":"An ambiguous limit read in your own favour, on oral advice, is exactly the file that looks worst in hindsight."},
                  {"key":"wait","label":"Hold for the board — moving £1.9m of obligation on a contested reading is precisely what limits exist to prevent","quality":25,
                   "consequence":"Principled and avoidable: the administrator re-tenders on day eleven, the packages fragment across new suppliers at a five-month delay, and the board you protected asks why its urgent-decision provision was never used.",
                   "principle":"Escalation that ignores the express route arrives as abdication — the duty is to obtain a fast decision, not to decline a slow one."}]}],
             "hints":["Separate the cash amount from the obligation amount — the two readings price the same decision differently.",
               "Look for what the delegation schedule says about urgency before concluding it says nothing.",
               "Whatever happens in ten days, the file should show a decision made at, or confirmed by, the higher authority."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the urgent-decision route through an ambiguous delegation limit — six days inside a ten-day clock."}
            """),

        ("WC-GOV-315", "The baseline the board never saw", "Every report says 'on plan'. The plan it means is not the one the board approved.",
            "Climate & Retrofit", "Programme Director", "project_management", "advanced", 18,
            """["governance","baseline_management","reporting_integrity"]""",
            """
            {"context":"Taking over the housing retrofit programme, you trace its reporting lineage. The board approved a baseline three years ago: 18,000 homes, £310m, four years. Since then, the delivery team has 're-planned' three times — each a sensible response to real events (a supplier failure, a specification change forced by new regulations, an access-rate discovery), each approved at delivery-group level, none taken back to the board. Current reports show 'green — on plan' against the third re-plan: 11,400 homes, £298m, five and a half years. The board believes it is funding the original. The board papers you inherit for next month contain the same green. Your predecessor's handover note says only: 'reporting basis is settled practice — do not reopen'.",
             "evidence":[
               {"label":"Approved baseline","value":"18,000 homes, £310m, 4 years — board, 3 years ago"},
               {"label":"Current basis","value":"Re-plan #3: 11,400 homes, £298m, 5.5 years — never board-approved"},
               {"label":"Reports","value":"'Green — on plan' against re-plan #3"},
               {"label":"Handover","value":"'Reporting basis is settled practice — do not reopen'"}],
             "decisions":[
               {"key":"baseline","prompt":"Your first board paper:",
                "options":[
                  {"key":"rebase","label":"Reopens it: present the full lineage — approved baseline, three re-plans, what each changed and why — report current performance against BOTH the board's baseline and the working plan, and ask the board to formally adopt, amend or reject a re-baseline so that 'green' regains a meaning","quality":100,
                   "consequence":"The meeting is the worst of your tenure and the turning point of the programme: the board adopts a revised baseline at 12,100 homes with a funded access-rate workaround, two members note they would have challenged re-plan #2 — and every future green means what it says.",
                   "principle":"A baseline is the board's property; performance reported against an unapproved plan is not reporting, it is the delivery team marking its own homework at 37% scope loss."},
                  {"key":"settle","label":"Follows the handover — the re-plans were each defensible, the practice is established, and reopening three years of reporting destabilises a programme that is genuinely delivering","quality":0,
                   "consequence":"An audit committee member eventually asks the innocent question — 'green against what?' — and the answer unravels in public: three years of reports, your name on the recent ones, all green against a plan the board never saw.",
                   "principle":"Settled practice that the governing body doesn't know about is not settled — it is undiscovered."},
                  {"key":"quiet_fix","label":"Adopts the working plan as the new baseline via delivery-group approval — regularising the practice through the same route that created it, without a board confrontation","quality":10,
                   "consequence":"A fourth unapproved re-plan now papers over the first three; the mechanism that caused the drift has been used as its remedy, and the gap between board belief and delivery reality is one re-plan wider.",
                   "principle":"You cannot cure an authority gap with another act at the wrong authority — the route that caused the disease is not its treatment."}]},
               {"key":"forward","prompt":"To prevent recurrence, you also propose:",
                "options":[
                  {"key":"control","label":"A baseline-change control with a materiality threshold: re-plans below it are delivery-group business reported to the board as variances; anything touching scope, completion date or approved cost above threshold goes to the board before the plan changes — with the reporting basis named on every dashboard","quality":100,
                   "consequence":"Re-plan #4 — and there is one, within the year — takes eleven days through the new control instead of zero through the old habit, and the board approves it knowing exactly what it trades.",
                   "principle":"The fix for baseline drift is not heroic honesty once — it is a threshold that makes the next drift impossible to do quietly."},
                  {"key":"trust","label":"A commitment that the delivery group will 'escalate significant re-plans' — keeping flexibility without bureaucracy","quality":15,
                   "consequence":"'Significant' is judged by the people doing the re-planning, which is how 18,000 homes became 11,400 without a board paper; the commitment restates the failure as a promise.",
                   "principle":"A control whose trigger is the controlled party's own judgement is a description of the status quo."}]}],
             "hints":["Trace what the board actually approved, then what each report's 'plan' actually refers to.",
               "Report against both baselines in one paper — the gap between them IS the message.",
               "The lasting fix is a materiality threshold on baseline changes, not a promise to behave."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put three years of quiet re-plans back in front of the board — and gave 'green' its meaning back."}
            """),

        ("WC-GOV-318", "Strategy says yes, capacity says no", "The board wants the relocation accelerated. The people who'd do it are already double-booked.",
            "Life Sciences", "Programme Director", "project_management", "advanced", 16,
            """["governance","resource_management","decision_quality"]""",
            """
            {"context":"The laboratory relocation is strategically urgent: the new owner wants the consolidated campus operational a year early, and the executive has publicly committed to explore acceleration. Your capacity analysis is unambiguous the other way: the relocation's critical resource is not money but a small pool of validation scientists — eleven people who must re-validate every assay in the new facility — and that pool is already committed at 110% through the original schedule, borrowed against by two other programmes. Acceleration on paper is easy; every acceleration scenario your planners model either strips validation (regulatory suicide), hires seniors who take a year to be effective, or cannibalises the other two programmes the same executive also sponsors. The steering committee expects an acceleration plan next week.",
             "evidence":[
               {"label":"Ask","value":"Campus operational a year early — public executive commitment to explore"},
               {"label":"Constraint","value":"11 validation scientists at 110% commitment; hiring lead time ~1 year to effectiveness"},
               {"label":"Scenarios","value":"Strip validation / hire seniors / cannibalise 2 sibling programmes"},
               {"label":"Forum","value":"Steering committee expects an acceleration plan in a week"}],
             "decisions":[
               {"key":"present","prompt":"To the steering committee you bring:",
                "options":[
                  {"key":"tradeoff","label":"The constraint itself, priced as an executive trade-off: the validation pool is the programme's true critical path, so acceleration is possible only by taking scientists from named sibling programmes with named consequences — presented as a portfolio decision for the executive who sponsors all three, alongside the one real mitigation (contract validation capacity with a 5-month qualification lead, buying ~4 months, not 12)","quality":100,
                   "consequence":"The executive, seeing for the first time that 'accelerate' means choosing between her own programmes, takes the 4-month contract option and re-frames the public commitment around phased occupancy — strategy amended by capacity fact, which is the system working.",
                   "principle":"When strategy and capacity conflict, the professional act is to price the conflict and hand it to the only level that can resolve it — not to promise the strategy or hoard the constraint."},
                  {"key":"comply","label":"An acceleration plan built on the least-bad scenario — stretch the validation pool with overtime and re-prioritised assays — because the committee asked for a plan, not a problem","quality":0,
                   "consequence":"The plan is approved and fails in month four exactly as the analysis predicted: validation backlog, two burned-out resignations from the eleven, and an acceleration that ends net slower than the original schedule.",
                   "principle":"A plan you know the constraint will defeat is not a plan — it is a scheduled disappointment with your name on the cover."},
                  {"key":"refuse","label":"A recommendation against acceleration, with the capacity analysis attached — the numbers speak for themselves","quality":30,
                   "consequence":"The numbers do not speak; they are spoken over. The committee reads refusal as inflexibility, commissions an external 'deliverability review' that takes eight weeks to find your analysis was right, and the relationship damage outlives the vindication.",
                   "principle":"'No' without options invites someone else to say yes on your behalf — constraints persuade only when priced into choices."}]},
               {"key":"protect","prompt":"Whatever the committee chooses, you also:",
                "options":[
                  {"key":"visible","label":"Make the validation pool a permanently visible, portfolio-level resource: its commitments across all three programmes on one view, owned above programme level, so no future strategy is formed in ignorance of the constraint that governs all of them","quality":100,
                   "consequence":"Six months later a fourth programme's business case is amended at draft stage because the pool's view shows no capacity — the first strategic decision the constraint has informed before being discovered.",
                   "principle":"A shared constraint managed inside one programme will be promised by all of them — critical capacity belongs on the portfolio's books, not in a planner's spreadsheet."},
                  {"key":"local","label":"Keep the pool's management inside the relocation where the expertise sits, sharing the analysis when asked","quality":20,
                   "consequence":"'When asked' turns out to be after the next commitment; the pool is re-promised within two quarters by a programme that never knew to ask.",
                   "principle":"Constraints that are only visible on request are invisible at the moment strategies are made."}]}],
             "hints":["Find the real critical path — it is people, not money, and it is shared with two other programmes.",
               "Price each acceleration scenario in consequences the executive owns, not in planning objections.",
               "The deliverable is a trade-off the right level can decide — plus permanent visibility of the constraint."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a public acceleration promise against eleven validation scientists — and let the executive choose with open eyes."}
            """),

        ("WC-GOV-321", "The go/no-go with missing evidence", "Two of nine readiness criteria are unproven. The window closes tonight either way.",
            "Advanced Manufacturing", "Programme Director", "project_management", "advanced", 14,
            """["governance","decision_quality","risk_management"]""",
            """
            {"context":"The plant expansion's tie-in to the live production line happens in tonight's maintenance window or in eleven weeks — the next scheduled shutdown. The go/no-go board convenes at 14:00. Seven of nine readiness criteria are green with evidence. Two are not: the reverse-flow interlock test could not be completed because the test rig failed this morning (the interlock itself is installed and visually verified, but unproven under load), and the operations team's competency sign-off covers only two of the three shifts — the night shift's assessor was ill. The commercial pressure is real: eleven weeks of delay costs seven figures and a customer commitment. The tie-in, if it goes wrong, stops the existing line — the one currently earning all the money.",
             "evidence":[
               {"label":"Window","value":"Tonight, or +11 weeks (next shutdown)"},
               {"label":"Green","value":"7 of 9 criteria evidenced"},
               {"label":"Open","value":"Interlock unproven under load (rig failed); night-shift competency unsigned"},
               {"label":"Stakes","value":"Delay: 7 figures + customer promise. Failure: stops the earning line"}],
             "decisions":[
               {"key":"gonogo","prompt":"As the accountable chair, you:",
                "options":[
                  {"key":"criteria","label":"Interrogate the two open criteria for what they actually protect and whether equivalent evidence or containment exists tonight: an alternative load test via the commissioning skid for the interlock, and restricting tonight's work to the two signed-off shifts with the tie-in's night activities re-sequenced — go only if both criteria are genuinely satisfied by other means, and say no if either can't be","quality":100,
                   "consequence":"The commissioning skid proves the interlock by 19:00; the re-sequence puts night-shift work under day-shift supervision overlap. The tie-in proceeds with nine criteria met — two by routes the checklist never imagined — and the record shows why each was equivalent.",
                   "principle":"Readiness criteria protect specific failure modes, not paperwork completeness — the honest question is never 'are all boxes ticked' but 'is each failure mode actually closed tonight'."},
                  {"key":"go_risk","label":"Go — seven of nine is strong, the interlock is installed and inspected, and eleven weeks against two soft criteria is not a serious trade","quality":0,
                   "consequence":"The tie-in works; the culture doesn't. 'Seven of nine is a go' enters the plant's folklore, and the next go/no-go — with different criteria open — cites tonight as precedent for the shortcut that eventually finds the failure mode a criterion existed to catch.",
                   "principle":"Every criterion waived without equivalent evidence teaches the organisation which criteria are decorative — a lesson it will apply when you are not in the chair."},
                  {"key":"no_go","label":"No-go — the criteria exist precisely for this moment, incomplete is incomplete, and the eleven weeks is the price of a rig that failed","quality":35,
                   "consequence":"Defensible, expensive, and less rigorous than it looks: equivalent evidence for both criteria was available by evening, and the seven-figure delay bought no risk reduction the alternatives couldn't have — rigidity mistaken for rigour.",
                   "principle":"Treating the checklist as the safety case — rather than the failure modes behind it — fails in both directions; automatic no is judgement abdicated just as surely as automatic go."}]}],
             "hints":["Ask what each open criterion protects against — then whether that protection can be achieved another way tonight.",
               "Distinguish evidence that is missing from evidence that is late — a failed rig is not a failed interlock.",
               "Whatever you decide, the record must show failure modes closed, not boxes negotiated."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed two open readiness criteria by equivalent evidence before a one-night window — and kept the checklist honest."}
            """),

        ("WC-GOV-323", "The second gate that split the board", "Same deadlock, higher stakes — and this time the criteria were yours.",
            "Enterprise Transformation", "Programme Director", "project_management", "advanced", 15,
            """["governance","decision_quality","benefits_management"]""",
            """
            {"context":"The enterprise transformation's release-two gate has deadlocked its board — and this time the gate criteria are the improved ones you introduced after last year's near-miss: benefits measured at original dates by an independent owner. The measurement has duly landed, and it is genuinely ambiguous: the efficiency benefit is real but 40% smaller than the case; the revenue benefit is above case but driven substantially by a market change no one predicted; the adoption metric passed its threshold the week of measurement after two months below it. Half the board reads this as 'the case is broken — pause and re-plan'; half as 'the portfolio effect is positive — proceed'. Your criteria produced honest numbers; they did not produce a verdict. All eyes turn to the programme director.",
             "evidence":[
               {"label":"Efficiency benefit","value":"Real, but 40% below case"},
               {"label":"Revenue benefit","value":"Above case — driven largely by an unforecast market change"},
               {"label":"Adoption","value":"Passed threshold in measurement week, after 2 months below"},
               {"label":"Board","value":"Split: 'case broken — pause' vs 'portfolio positive — proceed'"}],
             "decisions":[
               {"key":"verdict","prompt":"Your counsel:",
                "options":[
                  {"key":"recast","label":"Separate what the numbers settle from what they cannot: attribution — recommend proceeding with release two re-scoped to the efficiency shortfall's root cause (the numbers show where it lives), the revenue benefit re-based so the case no longer claims the market's work as the programme's, and adoption put on a 3-month confirmation measure rather than a single-week pass","quality":100,
                   "consequence":"Both camps get the half they were right about: delivery continues where evidence supports it, the case stops flattering itself with borrowed revenue, and the adoption question resolves itself on the confirmation measure two months later — downward, vindicating the check.",
                   "principle":"Honest measurement often returns a mixed verdict — the gate's job then is decomposition, not adjudication: proceed where the evidence is real, re-base where it is borrowed, re-measure where it is thin."},
                  {"key":"proceed","label":"Advise proceeding on the aggregate — total benefits exceed the case, and boards that relitigate composition when the total is positive teach programmes to stop measuring honestly","quality":20,
                   "consequence":"The aggregate holds until the market change reverses eighteen months later, at which point the case's real performance — efficiency 40% light, adoption soft — stands exposed without the borrowed revenue that had been covering it.",
                   "principle":"An aggregate that mixes earned and lucky is not a verdict, it is a coincidence with a total."},
                  {"key":"pause","label":"Advise pausing — a 40% efficiency shortfall and a one-week adoption pass are exactly what the improved criteria were built to catch","quality":30,
                   "consequence":"The pause treats the strongest evidence (real, above-case revenue delivery capability) identically to the weakest (the adoption blip); four months of re-planning later, the plan restarts substantially unchanged, minus momentum and two key engineers.",
                   "principle":"Criteria built to catch weak evidence should not be used to discard strong evidence that arrives in the same envelope."}]}],
             "hints":["Decompose before you adjudicate: which number is earned, which is borrowed, which is thin?",
               "A benefit driven by an unforecast market change belongs in the case's honesty, not its victory column.",
               "One-week threshold passes after two months below are measured again, not celebrated."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Split a mixed benefits verdict into proceed, re-base and re-measure — instead of forcing it to one word."}
            """),

        ("WC-GOV-325", "Benefits in the ground", "The road opened on time. The case it was built on opens later — if anyone checks.",
            "Highways", "Programme Director", "project_management", "advanced", 13,
            """["governance","benefits_management","reporting_integrity"]""",
            """
            {"context":"The highway upgrade opened four months ago, on time and within budget — the celebration is over and the delivery team largely demobilised. The business case promised three benefits: journey-time savings (the headline), casualty reduction, and unlocked development land at two junctions. Your closure obligations include a benefits plan handover — and the uncomfortable discovery is that nobody remains to receive it: the council's transport strategy team, named as benefits owner in the case, was reorganised out of existence last year. Early data is mixed: journey times improved less than modelled because traffic grew to fill the new capacity faster than forecast; casualty data needs three years to mean anything; the development land sits in a planning process the case never mentioned. The path of least resistance is a closure report noting benefits 'transferred to the council' — which is technically the case's own wording.",
             "evidence":[
               {"label":"Delivery","value":"Opened on time, within budget, team demobilising"},
               {"label":"Named owner","value":"Council strategy team — reorganised out of existence"},
               {"label":"Early data","value":"Journey times below model (induced demand); casualties need 3 years; land in unmentioned planning process"},
               {"label":"Easy path","value":"Closure notes benefits 'transferred to the council' — the case's own words"}],
             "decisions":[
               {"key":"handover","prompt":"Your closure report:",
                "options":[
                  {"key":"real_owner","label":"Refuses the fictional transfer: escalate that the named benefits owner no longer exists, secure a real successor owner with the council's executive before closure, and hand over a live measurement plan — induced-demand-adjusted journey metrics, the casualty measurement dates, and the planning dependency the case omitted, stated plainly","quality":100,
                   "consequence":"Closure takes six weeks longer and means something: the successor owner exists, the three-year casualty measurement is calendared with funding, and the induced-demand finding feeds the next scheme's model instead of being buried with this one's.",
                   "principle":"A benefit handed to an owner that doesn't exist has been abandoned, not transferred — closure's last duty is making sure the case's promises land on a desk that is real."},
                  {"key":"transfer","label":"Uses the case's wording — benefits transfer to the council at closure, the delivery organisation's accountability ends at the ribbon, and the owner's reorganisation is the council's problem to solve","quality":0,
                   "consequence":"No one measures anything; three years later a road-safety FOI request finds no casualty evaluation was ever done on a scheme part-justified by casualty reduction, and the resulting coverage names the closure report that made it nobody's job.",
                   "principle":"'Technically transferred' to a void is the mechanism by which most promised benefits are never measured at all."},
                  {"key":"extend","label":"Keeps benefits measurement inside the programme — extend a small team three years to do the measurement properly rather than trust the council","quality":25,
                   "consequence":"Well-intentioned overreach: the sponsor declines to fund a delivery body doing a strategy body's job for three years, closure stalls in the argument, and the actual fix — a real council owner — is delayed by the attempt to substitute for one.",
                   "principle":"Delivery organisations are scaffolding; the answer to a missing owner is finding one, not becoming one."}]}],
             "hints":["Check whether the named benefits owner still exists before writing 'transferred'.",
               "The induced-demand shortfall is a finding worth handing over, not an embarrassment worth burying.",
               "Closure's test: three years from now, is each measurement someone's funded, calendared job?"],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a road scheme's closure open until its promised benefits had a real owner and a funded measurement plan."}
            """),

        // ───────────── Risk Room · advanced · deep · evidence diagnosis ─────────────

        ("WC-RSK-313", "The single point of failure everyone knew", "It's in three risk workshops' outputs. It's in nobody's plan.",
            "River Crossings", "Programme Risk Analyst", "project_controls", "advanced", 16,
            """["risk_management","evidence_analysis","concept_planning"]""",
            """
            {"context":"At concept stage for the river crossing, you review the risk inheritance from three optioneering workshops. One item appears in all three outputs, worded differently each time: the entire scheme depends on a single grid connection through one substation for the tunnel-boring phase — 'power resilience (parked pending route decision)', 'TBM supply — single feed, discuss with DNO', 'energy SPOF — carried forward'. Each workshop noted it; each parked it; none owned it. The concept design freezes in eight weeks, and the design as drawn provides one connection point, because providing for a second was 'a detail for detailed design'. Your enquiry to the distribution operator reveals the fact nobody had collected: a second feed is possible from the southern network — but only if the concept safeguards a cable corridor that the current preferred site layout builds over.",
             "evidence":[
               {"label":"Pattern","value":"Same single-feed dependency parked in all 3 workshop outputs, differently worded"},
               {"label":"Design","value":"Concept freezes in 8 weeks with one connection point"},
               {"label":"DNO fact","value":"Second feed possible — needs a cable corridor the preferred layout builds over"},
               {"label":"Status","value":"No owner, no plan line, 'detail for detailed design'"}],
             "decisions":[
               {"key":"diagnose","prompt":"The finding you escalate:",
                "options":[
                  {"key":"window","label":"This is a concept-stage decision disguised as a detail: the single point of failure is only cheap to fix before the layout freezes — safeguarding the corridor costs a layout adjustment now, versus a network-priced diversion or an accepted months-long TBM outage risk forever after — and it needs an owner and a decision in the next eight weeks","quality":100,
                   "consequence":"The layout adjusts — one compound moves 40 metres — and the corridor is safeguarded at negligible cost; the risk that three workshops parked closes for the price of a drawing revision, which is what concept stage is for.",
                   "principle":"Risks 'parked for detailed design' that depend on concept-stage geometry are not parked, they are being decided by default — the window to fix them cheaply is exactly the window in which they look ignorable."},
                  {"key":"carry","label":"Carry it properly at last: register it with an owner, a probability, a TBM-outage impact, and a mitigation study in detailed design — correcting the process failure without disrupting the concept freeze","quality":15,
                   "consequence":"The register is finally honest and the layout freezes over the corridor; detailed design's mitigation study duly reports that the second feed is now a seven-figure diversion, and the register carries a permanently expensive risk that was briefly a free one.",
                   "principle":"Registering a risk after its cheap window closes is bookkeeping, not management — the workshop-parking failure repeats itself in better handwriting."},
                  {"key":"procure","label":"Attack resilience directly: specify on-site generation capacity for the TBM phase as a design requirement, making the grid question moot","quality":35,
                   "consequence":"Feasible and steep: continuous TBM-scale generation means a compound, consenting burden, fuel logistics and a noise fight — a seven-figure standing answer to a question the corridor safeguard answers for a drawing change.",
                   "principle":"Engineering your way around a constraint you could design your way through is valour where the job wanted judgement."}]},
               {"key":"process","prompt":"On the pattern itself — three workshops parking the same risk — you:",
                "options":[
                  {"key":"aging","label":"Add a parked-risk aging rule to the programme's risk procedure: anything parked twice, or parked across a stage boundary, is force-escalated to the stage gate with a named owner — because parking is a legitimate act with an illegitimate half-life","quality":100,
                   "consequence":"The next gate surfaces four multi-parked items; three are genuinely fine and one is the ground-investigation scope everyone assumed someone else had commissioned. The rule pays for itself before the crossing leaves concept.",
                   "principle":"Workshops park risks in good faith; the failure is systemic only if nothing ever forces the parked list to be re-read — aging rules convert parking from oblivion into deferral."},
                  {"key":"note","label":"Note the lesson in the workshop guidance — facilitators should assign owners to parked items in future","quality":20,
                   "consequence":"The guidance is followed at the next workshop and forgotten by the third; guidance without a forcing mechanism regresses to the mean that created this finding.",
                   "principle":"A lesson written into guidance changes behaviour until the first busy afternoon; a lesson written into the gate criteria changes it permanently."}]}],
             "hints":["Ask why the same risk was parked three times — then ask what makes it cheap to fix, and until when.",
               "The DNO enquiry is the evidence nobody collected: the fix depends on geometry the concept is about to freeze.",
               "Escalate it as a dated decision with a price that changes at freeze, not as a register entry."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed a three-times-parked single point of failure for the price of a drawing change — eight weeks before it froze expensive."}
            """),

        ("WC-RSK-316", "A residual risk with no owner", "The mitigation is complete. What's left over is looking for a home.",
            "Framework Programmes", "Programme Risk Analyst", "project_controls", "advanced", 16,
            """["risk_management","governance","closeout"]""",
            """
            {"context":"Closing the framework programme's risk register, you audit what 'closed' has meant. Most closures are clean. One is not: the ground-gas risk at the remediated depot site was mitigated by a membrane-and-venting design, installed, verified — and closed as 'mitigated'. But the design documentation states the membrane's protection assumes the venting layer is inspected annually and the site's end users never penetrate the slab without gas-safe procedures. The framework is ending; the site transferred to a community trust eighteen months ago; the register's closure note names no residual controls, no inspection regime, no owner. The trust's building manager — you telephone to check — has never heard of the venting layer and is currently pricing quotes to anchor new play equipment through the slab.",
             "evidence":[
               {"label":"Closure","value":"Ground gas 'mitigated — closed': membrane + venting, installed and verified"},
               {"label":"Design assumption","value":"Annual venting inspection; no slab penetration without gas-safe procedures"},
               {"label":"Transfer","value":"Site with community trust for 18 months; no controls communicated"},
               {"label":"Live fact","value":"Trust pricing quotes to anchor play equipment through the slab"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your reading of the situation:",
                "options":[
                  {"key":"residual","label":"The risk was never 'closed' — it was converted from an event risk into a standing dependency on controls nobody transferred; the immediate hazard is the slab penetration, and the finding is a class defect: every 'mitigated-closed' entry whose mitigation carries operating assumptions needs the same audit before the programme dissolves","quality":100,
                   "consequence":"The trust's contractor is stopped two weeks before drilling; the audit finds three more closures with orphaned operating assumptions across the framework — a fire-curtain maintenance regime, a drainage easement, an anchor-testing schedule — each rehomed while an organisation still exists to rehome them.",
                   "principle":"Mitigation by engineered control doesn't close a risk, it transforms it into a dependency on the control's upkeep — and a dependency without an owner is the original risk on a timer."},
                  {"key":"single","label":"An urgent one-off: stop the drilling, brief the trust, hand over the inspection regime — and close the register with a corrected note","quality":40,
                   "consequence":"The depot site is made safe; the fire-curtain regime, drainage easement and anchor schedule — same failure, other sites — dissolve with the framework and are found by their consequences.",
                   "principle":"When an audit finds one orphaned control, the finding is the class, not the instance — programmes rarely make a filing error exactly once."},
                  {"key":"legal","label":"Primarily a liability question: refer the transfer documentation to legal to establish whether the disclosure obligation was met at handover","quality":10,
                   "consequence":"Legal review confirms the paperwork ambiguity in eleven weeks; the drilling was quoted for week three. The liability analysis is thorough and the hazard never read it.",
                   "principle":"Who should have told them is a question for after someone has told them."}]},
               {"key":"act","prompt":"Before the framework's closure date, you:",
                "options":[
                  {"key":"rehome","label":"Run a residual-controls handover as formal closure scope: every engineered mitigation with operating assumptions gets a written transfer — the control, its regime, its consequences of neglect — accepted by a named owner in the receiving organisation, with the trust's transfer done first and in person","quality":100,
                   "consequence":"Four control regimes land with named owners and budget lines; the trust's building manager, walked through the venting layer on site, becomes its most reliable inspector. The framework dissolves owing nothing to the future.",
                   "principle":"A programme's last product is the set of standing obligations it leaves behind — closure is complete when each has an owner who has said yes in writing, not when the register says closed."},
                  {"key":"letter","label":"Issue a residual-risks letter to each receiving organisation listing the controls and assumptions, discharging the duty before the closure date","quality":25,
                   "consequence":"The letters arrive at front desks of organisations that don't know why they matter; two are filed unread. Sent is not received, and received is not owned — the depot near-miss already proved that chain breaks.",
                   "principle":"Disclosure transfers information; only acceptance transfers responsibility."}]}],
             "hints":["Read the mitigation's design documentation for the words 'assumes', 'annually' and 'provided that'.",
               "Ask who, today, is performing each operating assumption — a control nobody operates is a risk nobody closed.",
               "One orphaned control found late in a programme is a class finding: audit every engineered closure the same way."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Stopped a slab-drilling near-miss and rehomed four orphaned control regimes before the programme dissolved."}
            """),

        ("WC-RSK-319", "The weather window and the reserve", "One calm fortnight, one contingency pot, two claims on both.",
            "Broadcast & Media", "Programme Risk Analyst", "project_controls", "advanced", 16,
            """["risk_management","schedule_analysis","cost_management"]""",
            """
            {"context":"The broadcast facility move — studios, transmission chain, master control — pivots on one constraint: the transmission mast's antenna swap needs a ten-day calm-weather window, and the reliable calm season at the site is a six-week band starting in nine weeks. Mobilisation planning surfaces a collision. The rigging contractor wants an early mobilisation premium to guarantee readiness for the front of the calm band; the transmission-chain vendor, running late on factory acceptance, wants funded acceleration to avoid arriving after the band. Both requests draw on the same programme reserve, and the reserve covers one of them fully, or both thinly. The schedule shows the antenna swap cannot happen before the transmission chain is accepted — but the risk model shows the calm band historically delivers two usable ten-day windows in seven years out of ten, and one window in the rest.",
             "evidence":[
               {"label":"Constraint","value":"10 calm days needed; 6-week calm band starts in 9 weeks"},
               {"label":"Riggers","value":"Early-mobilisation premium to guarantee front-of-band readiness"},
               {"label":"Vendor","value":"Late on acceptance; wants funded acceleration to make the band"},
               {"label":"Reserve","value":"Covers one request fully or both thinly; historically band gives 2 windows (70%) or 1 (30%)"}],
             "decisions":[
               {"key":"diagnose","prompt":"Working the logic and the weather statistics together, the sound allocation is:",
                "options":[
                  {"key":"sequence","label":"Fund the vendor's acceleration fully and hold the riggers to standard mobilisation for mid-band: acceptance gates the swap, so rigger earliness buys nothing while the chain is late — the risk that matters is missing the band entirely, and the acceleration attacks it directly while mid-band readiness still reaches the second historical window in most years","quality":100,
                   "consequence":"The chain accepts three days before the band opens; weather takes the first window away and grants the second, as it does most years — the swap completes on day 34 of the band, and the premium that would have bought idle riggers at the front of it was never spent.",
                   "principle":"Reserve follows the binding constraint: money spent making a successor early while its predecessor is late buys queue position in a queue that isn't moving."},
                  {"key":"both","label":"Fund both thinly — each risk is real, and partial cover on both beats full cover on one","quality":10,
                   "consequence":"Thin acceleration compresses the vendor's plan without funding its overtime, delivering acceptance mid-band anyway; the thinned rigger premium buys a readiness date the weather ignores. Both risks arrive, both mitigations underfunded.",
                   "principle":"Splitting reserve across sequential risks ignores the sequence — a chain is funded at its binding link, not averaged across its length."},
                  {"key":"riggers","label":"Fund the riggers' premium — the weather is the uncontrollable, so guaranteed front-of-band readiness protects against the 30% one-window years","quality":25,
                   "consequence":"The riggers stand ready at the band's front; the transmission chain, unaccelerated, accepts in week four of six. The one asset money couldn't influence — weather — was insured, while the one it could — the vendor — ran uninsured and consumed the band.",
                   "principle":"Insuring the uncontrollable while the controllable runs late is risk theatre — spend where spending changes the probability."}]},
               {"key":"protect","prompt":"You also recommend:",
                "options":[
                  {"key":"trigger","label":"A dated decision trigger and a protected fallback: if factory acceptance isn't achieved by the band's minus-two-weeks point, pre-book the next calm season's rigging slot and re-plan broadcast continuity for the interim — a decision made by calendar, not by hope, while fallback slots still exist","quality":100,
                   "consequence":"Acceptance makes the date and the trigger never fires — but the pre-negotiated fallback slot, held for a modest fee, is the reason nobody spends the band's final fortnight improvising a continuity plan under pressure.",
                   "principle":"A weather-gated plan needs its abandon-criteria written before the season starts — the worst place to design a fallback is inside the closing window."},
                  {"key":"watch","label":"Weekly monitoring of vendor progress against the band, with escalation if the margin erodes","quality":20,
                   "consequence":"The margin erodes in increments too small to trigger any single escalation; week minus-one arrives with everyone informed, nobody decided, and the fallback slots gone.",
                   "principle":"Monitoring without a pre-committed decision point observes the failure in high resolution."}]}],
             "hints":["Draw the dependency: which request gates the other? Earliness behind a late predecessor is worthless.",
               "Use the window statistics — mid-band readiness still catches the second window in most years.",
               "Write the abandon-trigger date now: fallback options exist only before they're needed."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put the whole reserve on the binding constraint and wrote the abandon-date before the calm season opened."}
            """),

        // ───────────── Daily Decisions · cost & commercial · advanced · deep · evidence diagnosis ─────────────

        ("WC-CST-327", "Contingency, spent in silence", "The pot is half gone. The register says nothing happened.",
            "Capital Programmes", "Cost Manager", "project_finance", "advanced", 18,
            """["cost_management","governance","evidence_analysis"]""",
            """
            {"context":"Mobilising the capital programme's second tranche, you inherit the cost book. Contingency drawn to date: 48% — against a programme 30% complete. The drawdown log shows the mechanics but not the story: no drawdown links to a risk register entry; the register's top risks all show 'no change' since baseline; and the largest single draw — for 'design development' across four sites — was approved in one line at a delivery meeting whose minutes record no discussion. The programme director's explanation is candid: 'contingency was doing its job — absorbing the small stuff so we didn't drown the change process'. The small stuff, summed, is not small. Tranche two's contingency is sized on the same assumptions as tranche one's.",
             "evidence":[
               {"label":"Position","value":"48% contingency drawn at 30% complete"},
               {"label":"Traceability","value":"No draw links to a register entry; top risks 'no change' since baseline"},
               {"label":"Largest draw","value":"'Design development', 4 sites, one line, no minuted discussion"},
               {"label":"Director","value":"'Absorbing the small stuff so the change process didn't drown'"}],
             "decisions":[
               {"key":"diagnose","prompt":"Reading the cost book, the drawdown log and the static register together, your finding is:",
                "options":[
                  {"key":"blind","label":"The programme has been running without its early-warning system: untraced drawdown means the risks actually occurring never updated the register, so the register's 'no change' is silence, not stability — and tranche two's contingency is sized on assumptions tranche one has already disproven, which is the finding that outranks the burn rate itself","quality":100,
                   "consequence":"Reconstructing the draws against causes shows a pattern: 70% trace to design maturity, concentrated at two sites — a specific, insurable, tranche-two risk the register never learned. Tranche two re-sizes with eyes open, and drawdown gains a register-link rule.",
                   "principle":"Contingency drawdown is the programme's risk data speaking — spend it untraced and you fund your surprises while deleting the evidence they were arriving."},
                  {"key":"burn","label":"The burn rate is the headline: 48% at 30% complete forecasts exhaustion near 62% complete, and tranche two must not start until the run-rate is explained and arrested","quality":40,
                   "consequence":"The arithmetic is right and shallow: the run-rate analysis triggers a spending freeze that punishes current work, while the actual finding — which risks were occurring and whether tranche two inherits them — stays undone inside the frozen numbers.",
                   "principle":"A burn rate is a symptom reading; diagnosis is knowing what the money treated."},
                  {"key":"process","label":"A change-control failure: draws without linked justification are unauthorised in substance, and the one-line design-development approval should go to audit","quality":20,
                   "consequence":"Audit confirms in nine weeks what the book showed in one: weak linkage. The forward question — what the spending pattern means for tranche two's sizing — is not in audit's terms of reference and is answered by tranche two's overruns instead.",
                   "principle":"Referring the past to audit is not the same as extracting its information for the future."}]},
               {"key":"act","prompt":"For tranche two, you require:",
                "options":[
                  {"key":"rewire","label":"Reconstruct tranche one's draws into a cause-coded history, re-size tranche two's contingency from that evidence (not the original assumptions), and wire the mechanics together permanently: no drawdown without a register linkage, and a monthly reconciliation of contingency movement against risk movement","quality":100,
                   "consequence":"The reconstruction re-prices tranche two's contingency upward at the two design-immature sites and downward elsewhere — a defensible number that survives its first board challenge because every pound traces to a named, evidenced cause.",
                   "principle":"The cheapest risk data a programme will ever own is the money it has already spent — refuse to size the future on assumptions the past has priced."},
                  {"key":"topup","label":"Restore tranche one's contingency to plan via a change request, keeping tranche two's sizing as approved","quality":10,
                   "consequence":"The pot refills; the blindness that drained it remains plumbed in, and the refilled contingency drains on the same untraced pattern — now with a precedent that exhaustion is met by top-up.",
                   "principle":"Refilling an instrumentless tank prepares it to empty the same way twice."}]}],
             "hints":["Put three artifacts side by side: drawdown log, risk register, minutes — the story is in what doesn't connect.",
               "A static register during heavy drawdown is the tell: the risks were happening, unrecorded.",
               "Reconstruct draws by cause before sizing anything — tranche one already priced tranche two's risks."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read half a spent contingency pot as risk data — and re-sized the next tranche from what the money already knew."}
            """),

        ("WC-CST-329", "A value engineering call under pressure", "The saving is real. So is what it quietly deletes.",
            "Water Utilities", "Cost Manager", "project_finance", "advanced", 16,
            """["cost_management","value_engineering","evidence_analysis"]""",
            """
            {"context":"The water treatment upgrade is over budget at final design, and the value engineering workshop has produced its shortlist. The headline item: substituting the specified stainless dosing pipework for coated carbon steel — a substantial, genuine capital saving, vendor-verified, programme-neutral. The papers recommend approval. Cross-checking the operations submissions from a year ago, you find the specification's origin: the works' chlorine dosing regime was uprated two years back after a compliance incident, and the stainless specification was the corrosion engineer's response — the coated alternative was explicitly assessed then and rejected on maintenance-outage grounds, because recoating requires draining a process street the works can only spare in winter. The workshop paper does not mention this history; its whole-life cost line reads 'comparable — subject to maintenance regime'.",
             "evidence":[
               {"label":"Saving","value":"Stainless → coated carbon steel: large, verified, programme-neutral"},
               {"label":"History","value":"Spec originated from a compliance incident; coated option assessed and rejected on outage grounds"},
               {"label":"Constraint","value":"Recoating needs a process street drained — winter-only at this works"},
               {"label":"Paper","value":"Whole-life line: 'comparable — subject to maintenance regime'; history absent"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your assessment of the VE item:",
                "options":[
                  {"key":"history","label":"The item re-litigates a settled engineering decision without disclosing it: the 'subject to maintenance regime' caveat is carrying the entire question, because the maintenance regime is exactly what the works cannot deliver outside winter — the substitution must go back through the corrosion engineer with the incident history on the table, and the workshop's other items screened for the same pattern","quality":100,
                   "consequence":"The corrosion engineer, consulted rather than bypassed, kills the substitution in a day — and identifies a different, real saving (duplex grade only on the four wetted runs) worth 60% of the original claim with the outage constraint respected. The screen finds one more history-blind item.",
                   "principle":"Value engineering that doesn't know why the specification exists isn't engineering value — it is rediscovering the incident that wrote the spec, at the price of repeating it."},
                  {"key":"take","label":"Approve it — the saving is verified, the programme needs it, and maintenance regimes are operations' concern to manage after handover","quality":0,
                   "consequence":"The budget closes; four years later a summer recoating need meets a works that cannot drain the street, the dosing line runs coated-and-failing through a compliance season, and the incident that wrote the original spec repeats with the VE paper in its audit trail.",
                   "principle":"A capital saving whose cost lands in operations' constraint calendar hasn't been saved — it has been posted to a different department with interest."},
                  {"key":"wlc","label":"Commission a full independent whole-life cost study on the substitution before deciding — the caveated line is too thin to approve or reject on","quality":35,
                   "consequence":"The study takes seven weeks to conclude what the operations archive said in an afternoon: the outage constraint dominates. Right answer, wrong price — the design freeze slips a month for want of reading the file.",
                   "principle":"Before commissioning new analysis, exhaust the old — the cheapest study is the one the last incident already paid for."}]},
               {"key":"act","prompt":"On the VE process itself, you:",
                "options":[
                  {"key":"provenance","label":"Add a specification-provenance step to every VE item: before costing an alternative, the workshop must state why the current specification exists and who owns that reason — with the owning engineer's response filed with the item, so no saving is booked against an undisclosed history","quality":100,
                   "consequence":"The next workshop runs slower and lands better: two items die at the provenance step, one is strengthened by it, and the programme's VE savings stop being challenged at design review because the challenges have already happened inside the process.",
                   "principle":"Every specification is the answer to a question — value engineering is only safe when it knows what the question was."},
                  {"key":"trust","label":"Circulate the dosing-pipework lesson to future workshops as a cautionary example","quality":20,
                   "consequence":"The example is memorable for two workshops and mythology by the fourth; the next history-blind substitution arrives in a different discipline, where the story about pipework never felt relevant.",
                   "principle":"A cautionary tale changes the people who heard it; a process step changes the people who didn't."}]}],
             "hints":["Ask why the specification says stainless before pricing what doesn't — specs have authors and reasons.",
               "The caveat 'subject to maintenance regime' is doing all the work: test it against the works' actual outage calendar.",
               "Route the decision through the engineer who owns the original reason — bypass is how incidents recur."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Traced a verified saving back to the compliance incident that forbade it — and found a better saving in the file."}
            """),

        ("WC-CST-331", "The cash curve that crossed the facility", "The forecast is fine annually. Monthly, it breaks the bank in March.",
            "Defence Estates", "Cost Manager", "project_finance", "advanced", 14,
            """["cost_management","cashflow","evidence_analysis"]""",
            """
            {"context":"The defence facility upgrade's funding is annual: a fixed in-year allocation, spend it or lose it, with a hard rule against exceeding it in any month cumulatively. Assembling the mobilisation cost plan, you overlay three curves nobody had put on one page: the contractor's resourced programme (heavily front-loaded — piling and long-lead steel orders), the client's allocation profile (flat twelfths), and the security-clearance pipeline for site inductions (which throttles early manpower and pushes the contractor's real curve right). The overlay shows two problems in opposite directions: the contract's payment schedule crosses the cumulative allocation ceiling in month five — a breach — while the clearance throttle means the programme will likely underspend the first quarter, risking an end-of-year clawback under the spend-it-or-lose-it rule. Both facts are invisible in the annual view everyone has been managing to.",
             "evidence":[
               {"label":"Funding rule","value":"Annual allocation, flat profile, hard monthly cumulative ceiling, year-end clawback"},
               {"label":"Contract curve","value":"Front-loaded: piling + long-lead steel; ceiling breach at month 5"},
               {"label":"Clearance throttle","value":"Induction pipeline delays early manpower — real curve shifts right"},
               {"label":"Net position","value":"Q1 underspend risk AND month-5 breach risk, simultaneously"}],
             "decisions":[
               {"key":"diagnose","prompt":"The reconciliation that matters:",
                "options":[
                  {"key":"replan","label":"The two problems partially solve each other, but only if engineered deliberately: the clearance throttle's rightward shift can be used to re-phase the payment schedule under the ceiling — re-sequencing the steel order into a vesting arrangement that spreads its cash while protecting the delivery slot — and the Q1 headroom redirected to clearance-independent work brought forward, attacking the clawback with real progress rather than manufactured spend","quality":100,
                   "consequence":"The re-cut curve clears the ceiling with margin, Q1 lands within clawback tolerance on genuinely useful early works, and the steel slot survives inside a vesting certificate — three curves, one page, one plan.",
                   "principle":"Cashflow, programme and constraints are one system — managed on separate pages they generate contradictory crises that a single overlay dissolves."},
                  {"key":"breach_first","label":"The month-5 breach is the hard-rule violation, so the priority is renegotiating the payment schedule down; the Q1 underspend is next quarter's problem","quality":30,
                   "consequence":"The payment schedule flattens, the breach clears — and the untreated Q1 underspend matures into a year-end clawback that permanently shrinks the allocation the flattened schedule now depends on.",
                   "principle":"Sequential treatment of simultaneous constraints fixes the loud one and funds the quiet one's damage."},
                  {"key":"annual","label":"The annual position is balanced, and the funder's own review is annual — flag both wrinkles in the risk register and manage in-year with virements as they arise","quality":0,
                   "consequence":"Month five arrives; the cumulative breach triggers the funding body's automatic escalation, payments suspend for six weeks of process, and the piling contractor's standing time costs more than the steel vesting arrangement would have.",
                   "principle":"A hard monthly rule managed on an annual view is a breach with a scheduled date."}]},
               {"key":"act","prompt":"You institutionalise the finding by:",
                "options":[
                  {"key":"overlay","label":"Making the three-curve overlay the programme's standing cash instrument — contract payments, allocation ceiling and constraint-adjusted progress on one monthly view, owned in the cost report, with breach and clawback trip-wires calculated forward twelve weeks at every issue","quality":100,
                   "consequence":"In month seven the overlay flags a new ceiling approach eleven weeks out — a design-change payment landing early — and the re-phase happens in a routine meeting instead of an emergency one.",
                   "principle":"The instrument that found the problem is the instrument that prevents its siblings — one-off overlays decay; standing ones govern."},
                  {"key":"monthly","label":"Adding a monthly cashflow commentary to the existing cost report against the annual profile","quality":20,
                   "consequence":"The commentary narrates the curves it doesn't overlay; month seven's early payment is commented on the month after it approaches the ceiling.",
                   "principle":"Commentary describes; instruments warn."}]}],
             "hints":["Put all three curves on one page: contract payments, the funding ceiling, and clearance-adjusted reality.",
               "Notice the two risks lean opposite ways — the shift that causes the underspend can be used against the breach.",
               "Attack the clawback with real early work, never manufactured spend — and vest the steel rather than pay it early."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Overlaid three curves nobody had put on one page — and turned two opposite funding crises into one plan."}
            """),

        ("WC-CST-333", "Unit rates that stopped meaning anything", "The benchmark says fine. The basis of every number in it has drifted.",
            "Technology Programmes", "Cost Manager", "project_finance", "advanced", 15,
            """["cost_management","estimating","evidence_analysis"]""",
            """
            {"context":"The core-system replacement prices its remaining releases using the programme's 'calibrated' unit rates — cost per integrated interface, per migrated data domain, per regression cycle — refreshed quarterly from actuals and benchmarked green against industry. Preparing the release-four estimate, you audit the rates' construction. The interface rate's denominator has quietly changed twice: early releases counted every interface including trivial file transfers, recent ones count only 'complex' interfaces after a definition change — so the rate rose while apparent productivity 'held steady'. The migration rate excludes the data-cleansing labour that was moved to a 'business readiness' budget line in release two. And the regression rate is calibrated on releases one to three, all of which ran on the legacy scheduler — release four is first on the new platform. The benchmark comparison uses none of these footnotes.",
             "evidence":[
               {"label":"Interface rate","value":"Denominator redefined twice — 'all interfaces' → 'complex only'"},
               {"label":"Migration rate","value":"Cleansing labour moved to a different budget line in R2"},
               {"label":"Regression rate","value":"Calibrated on legacy scheduler; R4 is first on new platform"},
               {"label":"Benchmark","value":"Green — footnote-free"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your finding on the estimating basis:",
                "options":[
                  {"key":"basis","label":"The rates have suffered silent basis drift — each individually explicable, collectively a broken instrument: rebuild the three rates on a stated, versioned basis (one interface definition applied retrospectively, cleansing cost re-included, regression rate flagged uncalibrated for the new platform with an explicit uncertainty range), and re-estimate release four on the rebuilt basis before it is priced","quality":100,
                   "consequence":"The rebuilt basis moves the release-four estimate up materially — and honestly; the board approves it grumbling about the number and complimenting the footnotes, which is the correct way round. The old basis would have surfaced as an overrun instead.",
                   "principle":"A unit rate is a ratio, and a ratio with a drifting denominator measures nothing — an estimate's authority lives entirely in the stability of its basis."},
                  {"key":"green","label":"The rates pass external benchmark and internal trend — the definitional changes were each approved at the time, and re-litigating them re-opens three settled quarters","quality":0,
                   "consequence":"Release four, priced on rates that exclude cleansing and assume the old scheduler, overruns by the sum of the footnotes that weren't written; the post-mortem reconstructs the basis drift in a week and asks who owned the rates.",
                   "principle":"Individually approved changes can still compound into a collectively false instrument — approval history is not accuracy."},
                  {"key":"contingency","label":"Keep the rates, price release four with an enlarged contingency to absorb the definitional uncertainty","quality":25,
                   "consequence":"The padded estimate is challenged, halved in negotiation — pads always are — and the release inherits the original error at half protection, with the rates' drift still uncorrected for release five.",
                   "principle":"Contingency is for uncertainty you can't remove, not inaccuracy you won't — padding a broken basis rents time and fixes nothing."}]},
               {"key":"act","prompt":"To keep the instrument honest, you:",
                "options":[
                  {"key":"version","label":"Put the rate book under basis control: every rate carries its definition, inclusions, calibration set and version; any basis change re-states history or opens a new versioned series — and the benchmark comparison discloses basis differences or doesn't run","quality":100,
                   "consequence":"The next definitional pressure — a vendor proposing to reclassify defect-fix effort — arrives as a visible version decision debated for a week, instead of a silent drift discovered in an estimate three releases later.",
                   "principle":"Rates are instruments; instruments need calibration control — a rate book without basis governance is a rumour with decimals."},
                  {"key":"owner","label":"Assign the rate book a named owner responsible for challenging future definitional changes","quality":30,
                   "consequence":"The owner catches the next drift and misses the one after — ownership without a versioning mechanism depends on one person's memory of three quarters of footnotes.",
                   "principle":"Name an owner and give them the machinery; either alone is half a control."}]}],
             "hints":["Audit each rate's denominator across its history — drift hides in definitions, not arithmetic.",
               "Check what left the numerator too: costs moved to other budget lines still get spent.",
               "A rate calibrated on the old platform is a hypothesis on the new one — price the uncertainty explicitly."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rebuilt three quietly-drifted unit rates before they priced a release — and put the rate book under basis control."}
            """),

        ("WC-CST-335", "The commitment ledger nobody reconciled", "Spend says affordable. Commitments say otherwise. Both are 'the number'.",
            "Rail Systems", "Cost Manager", "project_finance", "advanced", 13,
            """["cost_management","governance","evidence_analysis"]""",
            """
            {"context":"Mobilising the resignalling project's supply chain, you are asked to confirm affordability of the next two orders — the interlocking hardware and the detection subsystem. The finance system says yes comfortably: spend to date is well inside budget. But finance records cost at invoice. Assembling the commitment picture from contract awards, purchase orders, signed variations and two letters of intent the project director issued directly, you build the ledger nobody keeps: committed-but-uninvoiced obligations that consume most of the apparent headroom. One letter of intent — for the temporary signalling during stagework — was never converted to a PO and sits in no system at all; the supplier has mobilised against it. On the commitment basis, the two orders are affordable only if the stagework letter's scope is descoped, novated or funded by change.",
             "evidence":[
               {"label":"Finance view","value":"Invoice-based spend — comfortable headroom"},
               {"label":"Commitment ledger","value":"Awards + POs + variations + 2 letters of intent ≈ headroom consumed"},
               {"label":"Orphan","value":"Stagework LOI: no PO, in no system, supplier mobilised against it"},
               {"label":"Ask","value":"Confirm affordability of interlocking + detection orders"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your affordability answer:",
                "options":[
                  {"key":"commit_basis","label":"Answer on the commitment basis or not at all: the orders are affordable only against decisions not yet made — regularise the orphan letter first (it is a live obligation whether or not a system holds it), then present affordability as commitments-versus-budget with the invoice view demoted to a cash-timing report","quality":100,
                   "consequence":"The stagework letter converts to a priced PO — larger than anyone remembered promising — and the two orders proceed re-phased by a month to stay inside genuine headroom. The project's first true affordability statement is its least comfortable and most useful.",
                   "principle":"Affordability is a question about obligations, not invoices — a budget minus its commitments is the only headroom that exists."},
                  {"key":"confirm","label":"Confirm against the finance system — it is the system of record, the orders are within its headroom, and the commitment work can proceed in parallel","quality":0,
                   "consequence":"Both orders place; the stagework supplier invoices against its letter three months later, and the project discovers it has spent the same headroom twice — in the exact month the interlocking milestone payment lands.",
                   "principle":"Confirming affordability on the invoice view is spending money that is already promised to someone who hasn't billed yet."},
                  {"key":"halt","label":"Refuse to confirm and freeze all new orders until a full commitment reconciliation is complete across the project","quality":30,
                   "consequence":"Prudence at panic pricing: the interlocking slot — the one order with a genuine factory queue — is lost in the freeze, costing eleven weeks, when the reconciliation needed for these two orders was three days of work.",
                   "principle":"Reconcile at the speed of the decision that needs it — a targeted answer this week beats a perfect ledger after the slot is gone."}]},
               {"key":"act","prompt":"Structurally, you:",
                "options":[
                  {"key":"ledger","label":"Stand up the commitment ledger as the project's affordability instrument — every obligation-creating instrument (award, PO, variation, letter, side agreement) registered at signature, reconciled monthly to finance, with letters of intent expiring by default unless converted — and affordability sign-offs referencing it by version","quality":100,
                   "consequence":"The next direct-issued letter — habits persist — is caught at the monthly reconciliation instead of at invoice, and its author now routes commitments through the ledger because that is where affordability answers come from.",
                   "principle":"Obligations are created by signatures, not by systems — a project that only tracks what its systems ingest will always have a second, invisible ledger, and it is the one that ruins them."},
                  {"key":"policy","label":"Issue a policy that letters of intent require commercial sign-off, addressing the root cause directly","quality":25,
                   "consequence":"The policy governs future letters; the existing invisible commitments stay invisible, and the affordability question that started this remains answered on the wrong basis.",
                   "principle":"Policy prevents the next orphan; only the ledger finds the current ones."}]}],
             "hints":["Ask what basis the 'affordable' answer uses — invoiced, accrued or committed — before trusting it.",
               "Hunt for obligation-creating paper outside the systems: letters, side agreements, instructed variations.",
               "A letter of intent a supplier has mobilised against is a commitment, whatever the ledger says."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Built the commitment ledger nobody kept — and found the headroom had already been promised away."}
            """),

        // ───────────── Project Rescue · advanced · deep · multi-stage ─────────────

        ("WC-RSC-311", "An estimate at completion with three authors", "Three EACs, three owners, one board that wants one number.",
            "Portfolio Management", "Recovery Cost Director", "project_finance", "advanced", 18,
            """["cost_management","forecasting","governance"]""",
            """
            {"context":"You are parachuted into a troubled flagship programme whose estimate at completion has become a war. The delivery director's EAC assumes recovery of current productivity to plan levels 'as the learning curve completes'. The client-side cost assurance team's EAC extrapolates cumulative performance to date, mechanically. The main contractor's commercial team submits a third figure that anticipates successful claims recoveries. The spread between lowest and highest is 30% of budget, each number has an institutional author defending it, and the portfolio board — which must decide whether to re-scope the programme — has responded by averaging the three, which satisfies nobody and informs nothing.",
             "evidence":[
               {"label":"EAC 1 — delivery","value":"Assumes productivity recovers to plan as learning completes"},
               {"label":"EAC 2 — assurance","value":"Mechanical extrapolation of cumulative performance"},
               {"label":"EAC 3 — contractor","value":"Nets off anticipated claims recoveries"},
               {"label":"Board","value":"Spread = 30% of budget; board currently averaging the three"}],
             "decisions":[
               {"key":"untangle","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"assumptions","label":"Stop the number war by decomposing it: force each EAC onto a common structure — same scope basis, same treatment of claims (gross, with recoveries shown as a separate risked line), and each forecast's productivity assumption stated as a testable claim against the last six periods' actual trend","quality":100,
                   "consequence":"On a common basis the three numbers become one disagreement: the productivity recovery assumption. Six periods of data show partial recovery on repetitive work, none on complex — a fact that replaces two of the three institutional positions.",
                   "principle":"Competing forecasts rarely disagree about arithmetic — decompose them to the assumption where they diverge and test that, instead of negotiating between totals."},
                  {"key":"independent","label":"Commission a fourth, independent EAC from an external quantity surveyor — an umpire figure with no institutional stake","quality":25,
                   "consequence":"Eight weeks and a fee later, the fourth number lands between the others and inherits their problem: a fourth set of assumptions to argue about, now with less programme knowledge behind it.",
                   "principle":"Adding an umpire number to a forecast dispute adds a forecast, not an answer — the disagreement lives in assumptions only decomposition can reach."},
                  {"key":"pick","label":"Adopt the assurance team's extrapolation as the governing figure — it is the only one free of advocacy","quality":35,
                   "consequence":"Mechanically pure and knowably wrong: pure extrapolation prices the learning that has demonstrably occurred on repetitive work at zero, and the board re-scopes against a number the next three periods disprove upward.",
                   "principle":"Freedom from advocacy is not accuracy — an assumption of no change is still an assumption, just an unexamined one."}]},
               {"key":"number","prompt":"Stage 2 — the evidence supports partial recovery. The EAC you take to the board:",
                "options":[
                  {"key":"range","label":"One evidence-based EAC with its structure showing: demonstrated productivity by work type (not hoped), claims shown gross with recoveries as a separately-risked line, and the residual uncertainty expressed as a range with the two or three variables that drive it named — plus what each variable's resolution date is","quality":100,
                   "consequence":"The board re-scopes against the range's prudent end and — for the first time — knows which three facts to watch quarterly; the EAC stops being a war because it has stopped being a negotiation.",
                   "principle":"A decision-grade EAC is a structure, not a number: what is demonstrated, what is claimed, what is uncertain, and when each uncertainty resolves."},
                  {"key":"single","label":"A single point figure at the evidence-based central estimate — boards decide on numbers, not ranges","quality":30,
                   "consequence":"The point lands, the board decides — and every subsequent variance from the point re-opens the credibility question the range would have priced in; by quarter three the forecast war has new authors.",
                   "principle":"A point estimate spends its credibility on the first variance; a structured range banks it."}]},
               {"key":"institution","prompt":"Stage 3 — so the war doesn't restart, you:",
                "options":[
                  {"key":"onebasis","label":"Institute a single forecast basis owned by one accountable forecaster: contributions from delivery, assurance and the contractor arrive as inputs on the common structure, disagreements are logged against named assumptions with evidence requirements, and the board sees one EAC with its dissent register — not three EACs with none","quality":100,
                   "consequence":"The next quarter's genuine disagreement — a claims recovery the contractor rates high — appears as a risked line with both positions and a resolution path, and the board spends its time on the decision instead of the arithmetic.",
                   "principle":"Organisations get forecast wars when the forecast has no single owner — accountability plus a dissent register beats consensus plus three numbers."},
                  {"key":"committee","label":"Create a monthly EAC reconciliation committee where the three teams align their figures before each board","quality":20,
                   "consequence":"The committee negotiates convergence; the published number becomes a diplomatic artifact that drifts from all three teams' honest views, which resume divergence in private.",
                   "principle":"Reconciliation by negotiation produces agreement, not accuracy — and the board can no longer see either."}]}],
             "hints":["Put the three EACs on one scope and claims basis before comparing anything.",
               "The real disagreement is one assumption — find it and test it against period actuals by work type.",
               "Take the board a structure with a range and named drivers, owned by one forecaster with a dissent register."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ended a three-way forecast war by finding the one assumption the numbers actually disagreed about."}
            """),

        ("WC-RSC-314", "The long-lead item with no float", "Ordered on time, promised to a date the rest of the plan quietly left behind.",
            "Joint Ventures", "Recovery Programme Manager", "project_management", "advanced", 14,
            """["procurement","schedule_analysis","recovery_management"]""",
            """
            {"context":"The joint venture's delivery office calls you in when someone finally connects two documents. The custom switchgear — the project's longest-lead item, 54 weeks — was ordered early and diligently against the original schedule, deliverable to site in week 61. Since then, the civils programme re-baselined twice; the switchgear room is now ready in week 74. Storage looks like the easy answer, but the order was placed ex-works with the manufacturer's standard terms: warranty starts at delivery, the JV has no suitable storage for HV equipment, and the manufacturer's 'extended factory storage' quote arrives priced at 2% of contract value per month with warranty suspension. Meanwhile the manufacturer hints that another customer would happily take the production slot — with a delivery next year that is worse than even the storage option.",
             "evidence":[
               {"label":"Mismatch","value":"Switchgear lands week 61; room ready week 74"},
               {"label":"Terms","value":"Ex-works, warranty from delivery, no JV HV storage"},
               {"label":"Factory storage","value":"2%/month + warranty suspension"},
               {"label":"Pressure","value":"Manufacturer hints at reselling the slot; next slot lands later than any option"}],
             "decisions":[
               {"key":"frame","prompt":"Stage 1 — you frame the problem as:",
                "options":[
                  {"key":"system","label":"A schedule-procurement integration failure with a 13-week gap to close from BOTH ends: re-examine the civils path for what actually gates the switchgear room (not the whole building), and re-open the delivery terms as a package — staged delivery, factory-retained warranty, storage, commissioning support — rather than accepting the storage quote as the only lever","quality":100,
                   "consequence":"The room's true gate is its dedicated slab and envelope — separable from the main civils path and accelerable by 8 weeks for modest cost; the residual 5 weeks becomes a negotiated staged delivery with warranty preserved. The gap closes from both ends at a fraction of the storage quote.",
                   "principle":"A gap between an early order and a late site is closed from both ends — the room's real readiness and the delivery's real flexibility are both softer than their headline dates."},
                  {"key":"storage","label":"A storage procurement problem: negotiate the factory storage rate down and secure warranty continuation — the schedule is what it is","quality":20,
                   "consequence":"The rate negotiates down to 1.4% with partial warranty; thirteen weeks of it still costs multiples of the slab acceleration nobody priced, because the schedule was treated as fixed when it was merely unexamined.",
                   "principle":"Accepting one side of a gap as immovable doubles the price of closing it."},
                  {"key":"slot","label":"A slot retention crisis: the manufacturer's hint is the emergency — secure the production slot contractually before optimising anything","quality":30,
                   "consequence":"The slot was never truly at risk — the hint was negotiating leverage — and the retention deed signed in haste concedes the storage rate and delivery terms that were the real money.",
                   "principle":"A counterparty's manufactured urgency is aimed at exactly this decision — verify the threat before paying to remove it."}]},
               {"key":"prevent","prompt":"Stage 2 — the JV has forty more procurement packages. You:",
                "options":[
                  {"key":"link","label":"Wire long-lead procurement to the schedule permanently: every package above a lead-time threshold carries a need-by date derived from the live schedule, re-validated at every re-baseline as a mandatory checklist item, with delivery flexibility (staging, storage, warranty terms) negotiated at order time when leverage is highest — not at crisis time when it is gone","quality":100,
                   "consequence":"The next re-baseline flags two more orders drifting toward the same mismatch — both re-sequenced by letter while the changes cost nothing; the switchgear lesson becomes machinery instead of folklore.",
                   "principle":"Orders placed against a schedule that later moves are orphans unless something re-marries them at every move — flexibility is bought cheap at order and dear at delivery."},
                  {"key":"review","label":"Add long-lead items as a standing agenda item at the monthly delivery review","quality":25,
                   "consequence":"The agenda item surfaces mismatches when someone happens to notice them — which is the process that noticed this one in week 58 of 61.",
                   "principle":"Agenda items observe; only baseline-change triggers catch drift at the moment it happens."}]}],
             "hints":["Measure the real gap: what gates the switchgear room specifically, not the building.",
               "Re-open delivery as a package — staging, warranty, support — while the manufacturer still wants the relationship.",
               "The class fix: need-by dates re-validated at every re-baseline, and flexibility negotiated at order time."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed a 13-week early-order gap from both ends — for a fraction of the storage quote on the table."}
            """),

        ("WC-RSC-317", "The change that arrived as a fact", "The turbines are already different. The change control starts today, apparently.",
            "Renewable Energy", "Recovery Programme Manager", "project_management", "advanced", 15,
            """["change_control","project_finance","governance"]""",
            """
            {"context":"On the wind farm repowering, you inherit a mess with rotating blades. The turbine supplier, mid-manufacture, substituted an upgraded nacelle variant — better output curve, different mass, different loads — and notified the project through a technical bulletin that nobody routed into change control. Eleven of twenty-eight units are already delivered; four are installed. The substitution surfaced when the foundations designer, reviewing an unrelated query, noticed the delivered mass exceeds the certified foundation loads' basis. The supplier's position: the bulletin constituted notice, the variant is 'equal or better', and the contract's fitness-for-purpose clause covers it. The certification body has not been told. The next four units install in three weeks.",
             "evidence":[
               {"label":"Fact","value":"Upgraded nacelle variant substituted mid-manufacture; 11 delivered, 4 installed"},
               {"label":"Trigger","value":"Delivered mass exceeds certified foundation-load basis"},
               {"label":"Supplier","value":"'Bulletin was notice; equal or better; fitness-for-purpose covers it'"},
               {"label":"Clock","value":"Certifier not told; next 4 units install in 3 weeks"}],
             "decisions":[
               {"key":"triage","prompt":"Stage 1 — your first 72 hours:",
                "options":[
                  {"key":"engineering","label":"Engineering truth before commercial position: hold the three-week installations, commission the foundations designer to assess the four installed units against actual delivered loads as the urgent case, and notify the certification body yourself — because a certificate based on superseded loads is everyone's problem and nobody's bargaining chip","quality":100,
                   "consequence":"The assessment shows the installed foundations carry the new loads inside their material margins — relief, documented; two of the remaining foundation designs need rebar revisions before their units install. The certifier, told early by you, treats it as a managed variance instead of a discovered concealment.",
                   "principle":"When a change arrives as a fact, sequence is everything: make the physical state safe and the certification honest first — the commercial argument keeps; the engineering risk doesn't."},
                  {"key":"commercial","label":"Commercial position first: reject the bulletin as notice, put the supplier formally in breach, and freeze payments while liability for the substitution is established","quality":15,
                   "consequence":"The breach letter is legally sound and operationally hollow: installations pause indefinitely while lawyers exchange positions, the certification gap sits unaddressed for six weeks, and the supplier's engineers — the people who know the variant — disengage behind their own counsel.",
                   "principle":"Leading with breach converts the people who understand the change into witnesses for the other side, while the physical risk waits."},
                  {"key":"absorb","label":"Accept the upgrade pragmatically — better turbines, and a retrospective change order regularising it keeps the programme moving","quality":10,
                   "consequence":"The retrospective order signs away the review that would have caught the two deficient foundation designs; unit seventeen's foundation shows early distress in year two, and the 'pragmatic' acceptance is the document the insurers read aloud.",
                   "principle":"Regularising a fait accompli without engineering review doesn't manage the change — it co-signs it."}]},
               {"key":"commercial2","prompt":"Stage 2 — foundations assessed, certifier engaged. The commercial settlement you drive:",
                "options":[
                  {"key":"package","label":"A single change order that prices the whole event honestly in both directions: the supplier funds the foundation reassessments, rebar revisions, delay and recertification (their unnotified change caused them); the project credits the genuine output uplift at its evidenced value; and the bulletin-as-notice doctrine is explicitly extinguished for the remaining seventeen units","quality":100,
                   "consequence":"The settlement lands in five weeks because each line traces to evidence; the supplier pays real costs, banks a fair share of the uplift, and the next bulletin — there is one — arrives through change control with a cover note requesting review.",
                   "principle":"A change that arrived wrongly can still be settled rightly: price the harm to its causer, the benefit at its evidence, and kill the doctrine that let it happen."},
                  {"key":"punitive","label":"Maximum recovery: claim every cost including the output uplift's value — the supplier's process failure forfeits its benefit","quality":25,
                   "consequence":"The claim overreaches into arbitration; eighteen months later the award lands near what the package settlement offered, minus fees and a supplier relationship the operations phase needed.",
                   "principle":"Pricing a counterparty's failure above the harm it caused converts a strong position into a long dispute."}]}],
             "hints":["Order of operations: physical safety, certification honesty, then commercial settlement.",
               "The installed units are the urgent engineering case — assess against actual loads before the next four go in.",
               "Settle both directions on evidence: their failure's real costs, the upgrade's real value, and the notice doctrine dead."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned an already-installed unauthorised change into a certified, fairly-priced one — in that order."}
            """),

        ("WC-RSC-320", "The exclusion whose premise died", "The words that kept the estimate low are about to keep the ward shut.",
            "Healthcare Estates", "Recovery Project Manager", "project_management", "advanced", 13,
            """["scope_management","stakeholder_communication","concept_planning"]""",
            """
            {"context":"The hospital ward refurbishment's business case sailed through approval eighteen months ago — partly, you now see, because its scope carried a quiet exclusion: 'ventilation plant serving the ward is excluded, being subject to the Trust's separate plant replacement programme'. That separate programme has since been deferred indefinitely by the Trust's capital squeeze. The refurbished ward's clinical model — respiratory step-down — requires air change rates the existing plant cannot deliver. Design is 60% complete; the clinical team assumed ventilation was in scope ('it's a respiratory ward'); the project board is chaired by the executive who signed the case with the exclusion in it. Without the plant, the ward as designed cannot open for its intended use.",
             "evidence":[
               {"label":"Exclusion","value":"Ventilation excluded — assigned to a plant programme since deferred indefinitely"},
               {"label":"Requirement","value":"Respiratory step-down needs air changes existing plant can't deliver"},
               {"label":"Status","value":"Design 60%; clinical team assumed in-scope"},
               {"label":"Politics","value":"Board chair signed the case containing the exclusion"}],
             "decisions":[
               {"key":"surface","prompt":"Stage 1 — you take this to the board as:",
                "options":[
                  {"key":"decision_paper","label":"A scope-integrity decision with options priced: the exclusion's premise has failed, so the case must choose — fund the plant inside this project (priced), re-scope the ward to a clinical model the existing air handles (priced, with the clinical team's assessment), or pause at a defined design milestone — presented without prosecuting who wrote the exclusion","quality":100,
                   "consequence":"The chair, given options instead of an accusation, funds the plant through a case addendum — the arithmetic of an unusable ward makes the argument no confrontation needed to make. Design proceeds with eleven days' slip.",
                   "principle":"An exclusion is a bet that someone else will do the work — when the bet fails, the honest move is repricing the choice, not relitigating the bet."},
                  {"key":"quiet","label":"Work the problem at officer level first — get estates to resurrect a minimal plant scheme inside the deferred programme, avoiding a board confrontation entirely","quality":20,
                   "consequence":"Estates sympathises and cannot conjure deferred capital; six officer-level weeks pass, design reaches 85% around an unresolved hole, and the board learns of the issue late — with less design flexibility and more sunk cost than the day you found it.",
                   "principle":"Problems that need a governance decision only age at officer level — the workaround hunt is usually the delay dressed as diplomacy."},
                  {"key":"design_on","label":"Direct the design team to complete with ventilation 'interfaces provided for' — protecting programme while the plant question resolves in parallel","quality":10,
                   "consequence":"'Provision for' hardens into a fiction: the ward completes beautiful and unopenable for its clinical purpose, and the completed-but-idle ward becomes a news story the Trust answers for a year.",
                   "principle":"Designing around a failed premise doesn't hold the programme — it delivers the failure on schedule."}]},
               {"key":"class","prompt":"Stage 2 — before the case addendum signs, you:",
                "options":[
                  {"key":"audit","label":"Audit every exclusion and dependency in the business case the same way: each one names what it depends on, that dependency's current status is verified with its owner, and any failed premise is surfaced in the addendum now — one governance moment instead of serial surprises","quality":100,
                   "consequence":"The audit finds one more failed premise — the 'existing nurse-call infrastructure to be reused' assumption died with an IT refresh — caught at addendum stage for a fraction of its later cost. The addendum passes containing both, credibility intact.",
                   "principle":"Exclusions are a business case's load-bearing assumptions in disguise — when one fails, the only professional response is to test them all."},
                  {"key":"single","label":"Handle the ventilation addendum cleanly on its own — one issue, one fix, minimal governance noise","quality":25,
                   "consequence":"The addendum passes; the nurse-call premise fails at commissioning, and the second surprise costs more than money — the board now reads every paper from the project looking for the third.",
                   "principle":"Serial surprises compound reputationally in a way one thorough audit never does."}]}],
             "hints":["Find why the exclusion existed — then check whether its premise still stands.",
               "Bring the board priced options, not a post-mortem: the chair signed the exclusion.",
               "One failed premise means the case's other exclusions need the same verification — now, in one pass."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-priced a business-case exclusion whose premise had quietly died — before the ward became beautifully unopenable."}
            """),

        ("WC-RSC-322", "Sequencing the work nobody wants first", "Every stakeholder's preferred order starts with someone else's disruption.",
            "Manufacturing", "Recovery Planning Lead", "project_management", "advanced", 18,
            """["schedule_analysis","stakeholder_communication","logistics"]""",
            """
            {"context":"The factory relocation — 140 machines, three production halls, a hard exit date on the old lease — has stalled in its mobilisation planning because no stakeholder will accept going first. Production wants machines moved in reverse order of utilisation ('idle ones first' — but the idle machines are the deep foundations jobs that need the new slab cured longest). Sales has promised customers no line stops longer than a fortnight, which every sequence violates for someone. The fit-out contractor wants zone-by-zone completion of the new facility, which strands early-moved machines without services. Facilities wants the hazardous plating line moved last ('fewest permits while both sites run') — but the plating line's permits take five months and gate the products that feed two other lines. Fourteen weeks of workshops have produced three mutually exclusive 'agreed' sequences and no plan.",
             "evidence":[
               {"label":"Constraints","value":"140 machines, 3 halls, hard lease exit; plating permits 5 months and gate 2 lines"},
               {"label":"Production","value":"Idle-first — but idle machines need longest slab cure"},
               {"label":"Sales","value":"No line stop > 2 weeks — violated by every sequence for someone"},
               {"label":"Standoff","value":"3 mutually exclusive 'agreed' sequences after 14 weeks"}],
             "decisions":[
               {"key":"reframe","prompt":"Stage 1 — you break the deadlock by:",
                "options":[
                  {"key":"constraints_first","label":"Taking sequencing away from preference entirely: build the sequence from the physics and law outward — permit lead times, slab cure, service availability, product-flow dependencies — as a constraint network that admits only feasible orders, then let stakeholders optimise their interests inside the feasible set, not compete to define it","quality":100,
                   "consequence":"The network reveals what fourteen weeks of workshops hid: the plating line's permit clock makes it first or the exit date fails — a fact, not a preference. Within the feasible set, sales' fortnight promise survives for all but one line, which gets a negotiated build-ahead stock instead.",
                   "principle":"Sequence disputes persist while hard constraints and preferences argue in the same forum — separate what physics decides from what people may, and most of the fight evaporates."},
                  {"key":"mediate","label":"Running a decision workshop with executive sponsorship where the three factions trade concessions to a compromise sequence","quality":15,
                   "consequence":"Workshop four produces compromise sequence four — politically balanced, physically wrong: it schedules the plating move at month nine of a five-month permit runway that needed starting at month one.",
                   "principle":"Compromise between preferences cannot repair a violation of physics — some orders are wrong regardless of who agrees to them."},
                  {"key":"impose","label":"Imposing the least-bad of the three existing sequences by programme authority — fourteen weeks of consensus-seeking is the actual problem","quality":25,
                   "consequence":"Decisiveness lands well for a month — until the imposed sequence (built from production's preference) strands the deep-foundation machines on uncured slab, and authority spent on the wrong plan is unavailable for the right one.",
                   "principle":"Imposing an infeasible plan quickly is not decisiveness — the constraint analysis was the missing ingredient, not the willpower."}]},
               {"key":"protect","prompt":"Stage 2 — the feasible sequence makes the plating line first, into a partially fitted-out facility. You:",
                "options":[
                  {"key":"enable","label":"Make going-first survivable by design: complete the plating zone's services out of zone order (the fit-out contractor re-sequences for a priced variation), pre-build the two dependent lines' buffer stock against the plating outage, and give the plating team the relocation's best support — because the first move's success recruits every reluctant stakeholder behind moves two through onwards","quality":100,
                   "consequence":"The plating line moves on permit-day-one into a zone fitted for it; its four-week outage is bridged by the buffer stock; and the sequence's loudest opponent tours the new line and asks when theirs goes. The remaining 139 machines follow a proven playbook.",
                   "principle":"Whoever goes first is bearing risk for the whole sequence — resource them like it, because the first move is also the argument for the rest."},
                  {"key":"asis","label":"Hold the fit-out contractor to its zone-by-zone plan — re-sequencing costs money and the plating team can work around temporary services","quality":10,
                   "consequence":"Temporary services for a hazardous plating line fail their permit inspection — as temporary services for hazardous processes do — and the five-month permit clock restarts against a lease exit that cannot move.",
                   "principle":"Sending the sequence's most regulated process into its least-ready zone converts the schedule's one immovable constraint into its likeliest failure."}]}],
             "hints":["List what is physically and legally immovable — permits, cure times, service dependencies — before any preference.",
               "The five-month permit runway is the sequence's real author: work backward from the lease exit.",
               "Fund and favour whoever goes first — their success is the plan's best persuasion."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Let physics write the move sequence three factions couldn't agree — then made going first the best job in the factory."}
            """),

        ("WC-RSC-324", "Liquidated damages, tested", "The delay is real, the clause is clear, and applying it might be the expensive choice.",
            "Urban Development", "Recovery Commercial Director", "project_finance", "advanced", 14,
            """["contract_management","commercial_awareness","decision_quality"]""",
            """
            {"context":"The mixed-use development's shell-and-core contractor is eight weeks late into the fit-out handover, with six more forecast. The contract's liquidated damages are clear, capped, and — your quantity surveyor confirms — enforceable: about £40k per week. The board wants them applied in full, immediately, 'as a matter of principle'. Your review of the wider position complicates the principle: the contractor's delay analysis attributes three of the eight weeks to late employer design decisions (arguably right on one, weak on two); the same contractor holds the fit-out contract's only viable price for the north block, currently in negotiation; and the contractor's parent company is — market intelligence suggests — deciding which of its regional businesses to recapitalise this quarter. LDs applied in full may be the event that tips this one from 'support' to 'manage decline'.",
             "evidence":[
               {"label":"Position","value":"8 weeks late + 6 forecast; LDs clear, capped, enforceable at ~£40k/week"},
               {"label":"Counter","value":"Contractor attributes 3 weeks to employer decisions — 1 arguable, 2 weak"},
               {"label":"Entanglement","value":"Same contractor holds the only viable north-block fit-out price"},
               {"label":"Fragility","value":"Parent deciding this quarter which regional businesses to recapitalise"}],
             "decisions":[
               {"key":"posture","prompt":"Stage 1 — your recommendation on the LDs:",
                "options":[
                  {"key":"instrument","label":"Apply them as an instrument, not a verdict: levy LDs from week one net of the one arguable employer week (rejecting the two weak ones with reasons), but structure application to serve the outcome — deductions phased against a completion-incentive agreement that returns a portion for hitting the recovered date, with the whole package negotiated alongside, but not traded against, the north-block price","quality":100,
                   "consequence":"The contractor's parent sees a client that enforces contracts and structures paths to survival — the regional business makes the recapitalisation list; completion lands two weeks inside the incentive date; and the north-block negotiation proceeds on its own merits, LDs having proven the client reads its own contracts.",
                   "principle":"Liquidated damages are compensation machinery, not a virtue signal — applied with structure they change behaviour; applied 'on principle' they mostly test whether your counterparty can survive your principles."},
                  {"key":"full","label":"Apply in full from week one, no netting, no structure — the clause is clear and the board's principle is right: contracts mean what they say","quality":15,
                   "consequence":"Legally impeccable; commercially self-harming: the parent reads the full-force deduction as the signal to manage decline, the contractor thins resources on your site to stem losses, completion drifts five more weeks — LD-capped — and the north-block price arrives 12% higher from the only other bidder.",
                   "principle":"Enforcement that ignores the counterparty's response function isn't rigour — a clause can be applied correctly and still be the most expensive available decision."},
                  {"key":"waive","label":"Hold LDs in reserve as negotiating leverage for the north-block price — the entanglement is the real commercial event","quality":10,
                   "consequence":"The unapplied clause reads as unappliable; the delay extends unpriced, the north-block negotiation now carries an implicit LD-waiver the auditors later call what it is, and the board's principle returns as a governance finding about yours.",
                   "principle":"Trading enforcement of one contract for price on another corrupts both — and teaches every counterparty that your clauses are opening positions."}]},
               {"key":"delay_truth","prompt":"Stage 2 — on the contractor's three claimed employer weeks, you:",
                "options":[
                  {"key":"adjudicate","label":"Deal with them now, on the evidence, at project level: accept the arguable week with its records, reject the two weak ones in a reasoned response, and offer the contract's dispute ladder if they disagree — because unresolved attribution compounds into exactly the global claim that outlives completion","quality":100,
                   "consequence":"The contractor takes the one week, tests one rejection up the ladder half-heartedly, and drops it; completion arrives with attribution settled and no claims tail — the quiet victory nobody notices because nothing happens.",
                   "principle":"Delay attribution settles cheapest in the present tense — every unresolved week is an invoice maturing in a claims consultant's drawer."},
                  {"key":"defer","label":"Park attribution until completion — the recovery needs everyone building, not arguing records","quality":20,
                   "consequence":"The parked three weeks return at final account as fourteen, professionally assembled, with the contemporaneous records you didn't contest now curated into their exhibit bundle.",
                   "principle":"Deferred attribution doesn't stay the same size — it is compound interest in someone else's favour."}]}],
             "hints":["Ask what the LDs are for — compensation and behaviour — then structure application to get both.",
               "Model the counterparty's response: a clause can be enforceable and still ruinous to enforce naively.",
               "Settle the attribution argument now, on records, at project level — parked weeks multiply."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Applied liquidated damages as machinery, not principle — and kept both the completion date and the counterparty alive."}
            """),

        ("WC-RSC-326", "A claim built from diary pages", "Ninety-one days of handwriting, one number at the end, and a vessel on standby.",
            "Offshore Energy", "Recovery Commercial Director", "project_finance", "advanced", 15,
            """["change_control","claims_management","evidence_analysis"]""",
            """
            {"context":"The offshore maintenance campaign ends with the contractor's claim on your desk: a substantial sum for weather standby and access delays, substantiated — in the claim's words — 'by contemporaneous site diaries'. The diaries are real: ninety-one days of the barge master's handwriting. Reading them against the claim's schedule is instructive. The diaries record weather honestly, but the claim counts every 'WOW' (waiting on weather) day at full spread rate — including fourteen days when the diary also records the crew executing deferred inspection work; the claim's access-delay days include five when the diary notes the contractor's own permit paperwork as the blocker; and the claimed vessel day-rate is the storm-season contingency rate, applied across all ninety-one days rather than the contracted split. The claim is not fabricated — it is real records, aggregated in one direction. Your own client-side records for the period are thinner than the diaries.",
             "evidence":[
               {"label":"Claim basis","value":"91 days of genuine barge-master diaries"},
               {"label":"Aggregation","value":"14 WOW days show productive deferred work; 5 access days show contractor's own permit failures"},
               {"label":"Rate","value":"Storm-season contingency rate applied to all days, not the contracted split"},
               {"label":"Your records","value":"Thinner than the diaries"}],
             "decisions":[
               {"key":"respond","prompt":"Stage 1 — your response to the claim:",
                "options":[
                  {"key":"their_records","label":"Assess it from the contractor's own diaries, line by line: build the counter-schedule from the same ninety-one pages — WOW days netted for recorded productive work, access days attributed per the diary's own notes, rates mapped to the contracted split — and respond with a reasoned valuation that cites their document on every adjusted line","quality":100,
                   "consequence":"The valuation lands at roughly 60% of the claim, and the contractor's commercial team finds itself arguing against its own barge master's handwriting; settlement concludes in six weeks at close to your figure, without a formal dispute.",
                   "principle":"The strongest answer to a records-based claim is the records — a claim's own evidence, read completely instead of selectively, is usually the best counter-schedule available."},
                  {"key":"reject","label":"Reject it globally — the double-counting you've spotted taints the whole submission, and a claim with padded lines deserves no line-by-line dignity","quality":15,
                   "consequence":"Global rejection of genuinely-evidenced weather days converts a 60% settlement into a formal dispute; the adjudicator, unimpressed by wholesale rejection of real diaries, awards nearer 85% — plus costs.",
                   "principle":"A claim that is 60% good and 40% aggregation is beaten by arithmetic, not indignation — global rejection surrenders the strong ground to defend the weak."},
                  {"key":"negotiate","label":"Go straight to commercial settlement — open at 40%, land near half, and save the analysis cost; final accounts are horse-trading anyway","quality":25,
                   "consequence":"The deal lands at 55% and teaches the market that your claims process is a bazaar; next campaign's claim arrives 30% padded in anticipation of the same haircut, from a contractor who now writes thinner diaries.",
                   "principle":"Settling unanalysed claims by percentage prices your future claims at whatever the other side chooses to open with."}]},
               {"key":"records","prompt":"Stage 2 — your own records were the weakness. For the next campaign, you:",
                "options":[
                  {"key":"joint","label":"Institute joint daily records: one shared log — weather, access, activity, delays and their causes — signed by both representatives each shift, with disagreements noted in the log itself the day they occur, as a condition of the next campaign's contract","quality":100,
                   "consequence":"The next campaign's final account settles in nine days: there is nothing to argue about, because every standby day was attributed the day it happened by both signatures. The claims consultant's fee is not incurred.",
                   "principle":"Claims are won and lost at the daily record, months before anyone writes the word 'claim' — a jointly-signed log is the cheapest dispute resolution ever invented."},
                  {"key":"own","label":"Strengthen your own side's daily reporting — a client representative logging independently on every shift","quality":40,
                   "consequence":"Better, and half the fix: the next dispute features two divergent unilateral records instead of one, and the argument moves from 'what happened' to 'whose log is better' — closer, but still an argument.",
                   "principle":"Parallel records dispute each other; joint records prevent the dispute."}]}],
             "hints":["Read all ninety-one pages, not the claim's summary of them — the diary is both their evidence and yours.",
               "Net the WOW days against recorded productive work, and attribute access delays by the diary's own causes.",
               "The forward fix is a jointly-signed daily log — attribution agreed the day it happens, not the year after."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Answered a diary-based claim with the same diaries, read completely — and settled at the arithmetic, not the anger."}
            """),

        ("WC-RSC-328", "Acceptance criteria, written after the fact", "The system is built. Now they'd like to define what 'working' means.",
            "Technology Programmes", "Recovery Delivery Director", "project_management", "advanced", 13,
            """["scope_management","requirements_management","negotiation"]""",
            """
            {"context":"The network modernisation's build is complete, and user acceptance has stalled into trench warfare. The contract's acceptance annex was left as 'criteria to be agreed during delivery' — and never was. Now the operator's acceptance team, staffed late in the programme by people who inherited none of the design compromises, is writing acceptance criteria retrospectively: their draft demands performance levels the design was never scoped to meet ('sub-50ms failover site-to-site' — the approved design targets 200ms), test conditions that never applied ('acceptance under simultaneous dual-site failure'), and documentation formats from their previous vendor's standard. Meanwhile the built system demonstrably meets the design specification it was actually built to, go-live is holding a revenue date, and each week of standoff runs parallel-operation costs.",
             "evidence":[
               {"label":"Gap","value":"Acceptance annex: 'to be agreed during delivery' — never agreed"},
               {"label":"Retro draft","value":"Sub-50ms failover (design: 200ms); dual-failure test conditions; alien doc formats"},
               {"label":"Fact","value":"System meets the approved design specification"},
               {"label":"Cost","value":"Go-live holds revenue; every standoff week burns parallel-running"}],
             "decisions":[
               {"key":"anchor","prompt":"Stage 1 — you re-anchor acceptance by:",
                "options":[
                  {"key":"design_basis","label":"Proposing the only defensible basis: acceptance criteria derived from the approved design specification and the operational requirements it traceably implements — with the acceptance team's draft triaged against it into three lists: covered by design (accept-testable now), genuine operational needs the design missed (change candidates, priced), and imported preferences (declined with reasons)","quality":100,
                   "consequence":"The triage converts warfare into a work plan: 70% of the draft maps to the design and tests within a month; two items are real gaps the operator funds as fast-follower changes; the dual-failure test and the doc formats die as imports. Go-live holds.",
                   "principle":"When acceptance criteria arrive after the build, the design specification is the contract's memory — acceptance tests what was commissioned, and change control prices what was wished for."},
                  {"key":"negotiate_all","label":"Negotiating the draft as a whole — meet them partway on failover, split the difference on test conditions, concede the documentation — because go-live needs their signature more than it needs doctrinal purity","quality":10,
                   "consequence":"Splitting the difference on sub-50ms yields a 'commitment' to 125ms the design cannot physically meet; the system fails its own negotiated acceptance, and the standoff resumes with your signature on the impossible number.",
                   "principle":"Acceptance criteria negotiated as concessions rather than derived from the design produce targets unmoored from the machine — which then fails them."},
                  {"key":"escalate","label":"Escalating to the contract executive: the annex gap is a mutual contract-management failure, and criteria imposed retrospectively are unenforceable — let the executives instruct the acceptance team","quality":30,
                   "consequence":"The executives duly instruct 'pragmatic acceptance' — with no basis specified; the acceptance team pragmatically re-tables 80% of its draft, and the standoff resumes one level down, four weeks later.",
                   "principle":"Escalation without a proposed basis returns as an instruction to agree — which was the problem, not the answer."}]},
               {"key":"protect","prompt":"Stage 2 — on the two genuine gaps the triage found, you:",
                "options":[
                  {"key":"decouple","label":"Decouple them from go-live explicitly: accept the system against the design basis now, with the two funded changes scheduled as a post-live release on a committed date and the residual risk of the interim state assessed and signed by the operator — go-live proceeds, gaps close on a plan, nothing is quietly waived","quality":100,
                   "consequence":"Revenue starts on the held date; the two changes land in the first quarterly release; and the acceptance file shows exactly what was accepted, what was deferred, and who owned each — the closeout audit finds nothing to find.",
                   "principle":"Real gaps deserve real treatment — funded, dated and risk-assessed — not a hostage position across a go-live that neither closes them faster nor prices them honestly."},
                  {"key":"gate","label":"Hold go-live until both gaps close — accepting a system with known operational shortfalls transfers their risk to the operator's night shifts","quality":25,
                   "consequence":"Twelve more weeks of parallel-running costs exceed both changes' price several times over, and the 'shortfalls' — a reporting view and a failover alarm refinement — were never night-shift-critical; the caution was real, its price-check wasn't.",
                   "principle":"Gaps gate go-live only when their operational risk outweighs the delay's cost — that comparison must actually be run, not assumed."}]}],
             "hints":["Find the document with contractual memory: the approved design specification is the anchor.",
               "Triage the retro draft into design-covered, genuine gap, and import — each list gets different machinery.",
               "Genuine gaps get funded dates and signed risk, not hostage status across go-live."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-anchored after-the-fact acceptance criteria to the design the system was actually built to — and held the revenue date."}
            """),

        ("WC-RSC-330", "Two projects, one senior engineer", "Both critical paths run through the same person. Both boards were promised her.",
            "Public Sector Technology", "Recovery Programme Manager", "project_management", "advanced", 14,
            """["resource_management","governance","stakeholder_communication"]""",
            """
            {"context":"The census-scale IT programme's two flagship projects — the data-processing platform and the field-operations system — are both in delivery trouble, and unpicking the schedules reveals the same name on both critical paths: the programme's principal data architect. The platform needs her full-time for the schema-migration design (twelve weeks); field operations needs her full-time for the integration security model (nine weeks); both windows are now, both project boards have separately been assured of her availability, and both project managers have been quietly booking her — she has been working sixty-hour weeks splitting the difference and telling neither side. There is no second person with her clearance-plus-domain combination in the organisation; contractors with equivalent clearance take four months to onboard. The statutory census date moves for nobody.",
             "evidence":[
               {"label":"Collision","value":"Same architect on both critical paths — 12-week and 9-week full-time windows, both now"},
               {"label":"Governance","value":"Both boards separately promised her; both PMs booking her"},
               {"label":"Human cost","value":"60-hour weeks, splitting the difference, telling neither side"},
               {"label":"Market","value":"No internal substitute; cleared contractors: 4 months to onboard"}],
             "decisions":[
               {"key":"truth","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"joint","label":"Surface the collision to both boards at once, as one paper with the census date as arbiter: her time is a single programme resource, the two windows must be sequenced or restructured by census-criticality (the schema migration gates the statutory processing chain; the security model has a documented interim-accreditation path), and her working pattern is named in the paper as a risk being managed, not a buffer being spent","quality":100,
                   "consequence":"The boards, seeing one truth instead of two promises, sequence her: platform first with a defined handoff, field ops on the interim-accreditation path with her at one structured day a week. Her hours return to human; the census date holds on a plan that acknowledges arithmetic.",
                   "principle":"A person on two critical paths is a programme decision wearing a rota problem — the fix is one paper to all the promised parties, sequenced by what the immovable date actually needs."},
                  {"key":"optimise","label":"Rebuild both schedules around fractional allocation — 60/40 with protected deep-work blocks, tightly managed — before troubling two boards with a resourcing matter","quality":15,
                   "consequence":"The optimised split fails the way splits fail: both design tasks need sustained immersion, both proceed at 30% effective, and six weeks later the collision reaches the boards anyway — larger, later, and with her resignation letter attached as context.",
                   "principle":"Fractionalising a person across two immersion-depth tasks is not allocation, it is scheduled failure of both — some resources genuinely don't divide."},
                  {"key":"hire","label":"Treat it as a capacity emergency: start the four-month contractor clearance now and bridge with her overtime formalised and compensated","quality":30,
                   "consequence":"Right long-term move, wrong sole move: the contractor lands after both windows close, and 'formalised overtime' converts an unsustainable pattern into an approved one — the census date is protected by hoping she doesn't break.",
                   "principle":"A pipeline fix for a present-tense collision protects the next crisis, not this one — and paying for burnout is still burnout."}]},
               {"key":"structure","prompt":"Stage 2 — beyond the sequencing decision, you:",
                "options":[
                  {"key":"depth","label":"Attack the single point of failure structurally: her twelve platform weeks are re-scoped to include a named deputy shadowing the schema design with explicit knowledge-transfer deliverables, the contractor clearance starts anyway for the operations phase, and 'individuals on more than one critical path' becomes a standing programme-level resource check at every re-plan","quality":100,
                   "consequence":"By week eight the deputy is fielding platform queries alone; the cleared contractor arrives in time for census operations; and the next re-plan's resource check catches a network engineer double-booked across two cutovers — caught at planning, costing nothing.",
                   "principle":"The lasting fix for a keystone person is never a better rota — it is deliberate redundancy: shadows, pipelines, and a standing check that finds the next keystone before both boards promise them."},
                  {"key":"ringfence","label":"Ring-fence her formally to the programme's disposal — a resource-control decision preventing either project from booking her directly again","quality":25,
                   "consequence":"The booking symptom is cured; the keystone disease remains — she is still the only person who can do either task, and the ring-fence's first real test is her first week of illness.",
                   "principle":"Controlling access to a single point of failure is custody, not resilience."}]}],
             "hints":["Two separate promises to two boards is the actual failure — reunify the truth before optimising the rota.",
               "Sequence by what the statutory date needs: one path gates it directly, one has an interim route.",
               "Then break the keystone pattern: shadow, pipeline, and a re-plan check for double-booked names."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reunified two boards' incompatible promises of the same architect — and then made sure no one person gated the census again."}
            """),

        ("WC-RSC-332", "The slide that reached the wrong room", "One slide about delay, three hundred furious ground staff, and a retraction that made it worse.",
            "Aviation", "Recovery Programme Director", "project_management", "advanced", 15,
            """["stakeholder_communication","governance","concept_planning"]""",
            """
            {"context":"The airport baggage-system upgrade needed to tell its stakeholders about a four-month delay. What happened instead: a programme-office slide deck — drafted for the executive steering group, stating that the delay 'enables workforce transition planning for the automation phase' — was forwarded, unredacted, to the airline community distribution list, and from there to the ground-handlers' union. Three hundred baggage staff read, in a bullet point, that the project automating their work was using its delay to plan their 'transition'. A hasty programme-office retraction called the slide 'poorly worded and not reflective of any agreed position' — which the union read, reasonably, as confirmation plus evasion. Two handling agents have paused their operational data-sharing with the programme; the airport's CEO wants a recovery plan for the relationship, not just the schedule; and the underlying truth is uncomfortable: automation WILL reduce baggage-hall headcount, and the workforce planning bullet was accurate.",
             "evidence":[
               {"label":"Leak","value":"Steering-group slide → airline list → union; 'workforce transition' bullet"},
               {"label":"Retraction","value":"'Poorly worded, not an agreed position' — read as confirmation plus evasion"},
               {"label":"Damage","value":"2 handling agents paused data-sharing; CEO wants relationship recovery"},
               {"label":"Truth","value":"Automation will reduce headcount; the bullet was accurate"}],
             "decisions":[
               {"key":"posture","prompt":"Stage 1 — the recovery's foundation:",
                "options":[
                  {"key":"honest","label":"Stop retracting the truth and start owning it: the programme's leadership meets the union and handling agents directly with the real position — what automation changes, what it doesn't, the honest headcount trajectory with dates, and the transition commitments (retraining, redeployment, timeline guarantees) that the leaked bullet gestured at but nobody had actually built — with the workforce package now developed WITH the union rather than about it","quality":100,
                   "consequence":"The first meeting is brutal and the second is a negotiation: the union has known automation was coming since the business case leaked years ago — what it couldn't accept was planning conducted about its members in slides they weren't meant to see. Data-sharing resumes against the joint working group's first output.",
                   "principle":"A leak of an uncomfortable truth cannot be repaired by disowning the truth — it is repaired by upgrading the people who read it from subjects of the plan to parties in it."},
                  {"key":"contain","label":"Contain and rebuild slowly: let the retraction stand, restore relationships bilaterally through the handling agents' commercial channels, and fold workforce messaging into the consultation the HR workstream had scheduled for next year","quality":0,
                   "consequence":"The union, holding a slide that says one thing and a retraction that says nothing, fills the silence: a work-to-rule in the baggage hall meets the programme's next integration test window, and next year's 'scheduled consultation' opens with zero credibility against a story the union now owns.",
                   "principle":"Between a leaked truth and a scheduled consultation lies a vacuum — and vacuums are filled by whoever is angriest."},
                  {"key":"blame","label":"Make it a leak-control matter first: investigate the forwarding chain, tighten distribution controls, and let steering decide the workforce narrative once the channel is secure","quality":10,
                   "consequence":"The investigation finds what leak investigations find — a well-meaning forward — while three hundred people's question ('what happens to us?') goes unanswered for six more weeks; the channel gets secured around a relationship that no longer exists.",
                   "principle":"When the leaked content is true, the leak is not the problem — treating disclosure as the failure confirms every suspicion about what else is being withheld."}]},
               {"key":"machinery","prompt":"Stage 2 — so communication stops being the programme's leading risk, you:",
                "options":[
                  {"key":"audience","label":"Rebuild communications around audiences instead of documents: every material message — starting with the delay itself — gets an audience map (who is affected, what they need to know, in what order, from whom), impacted-workforce audiences hear consequential news from named leaders before wider circulation, and anything drafted for one audience is written as if every audience will read it — because this programme now knows they will","quality":100,
                   "consequence":"The next hard message — a phased go-live that changes shift patterns — lands through the joint working group, face first, slides later; it produces questions instead of grievances, and the union rep's summary to members is more accurate than the programme's own slide would have been.",
                   "principle":"In a multi-stakeholder programme, there is no such thing as an internal document — sequencing who hears hard news from whom IS the communications strategy; the slides are just its residue."},
                  {"key":"approvals","label":"Introduce a communications approval gate: nothing leaves the programme office without sign-off against a sensitivity checklist","quality":30,
                   "consequence":"The gate catches the next careless forward and adds four days to every message; what it cannot catch is the pattern that caused this one — true things written about people who were never meant to be in the room.",
                   "principle":"Approval gates filter documents; the failure was in audiences — a checklist cannot fix who the plan was talking about instead of to."}]}],
             "hints":["Separate the leak from the truth it leaked — only one of them can be fixed, and it isn't the leak.",
               "The retraction failed because it disowned an accurate statement — recovery starts by owning it, with commitments attached.",
               "Rebuild around audiences: the affected hear it first, from leaders, and every document is written as if everyone reads it."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Recovered a leaked automation slide by making its uncomfortable truth the opening offer of a real negotiation."}
            """),

        ("WC-RSC-334", "The supplier who asked for help early", "The letter says 'we have a problem'. The contract says that's their problem. Reality disagrees.",
            "Flood Defence", "Recovery Commercial Director", "project_finance", "advanced", 13,
            """["procurement","supply_chain","commercial_awareness"]""",
            """
            {"context":"Four weeks into mobilisation of the flood defence scheme, the precast supplier — sole source for the specialist wave-return units, 40% of the programme's critical path — writes candidly: input cost movements on steel fibre and cement have opened a loss on its fixed-price contract; it can complete, but doing so 'will require decisions about production priorities across our order book', and it is raising this 'now, while options exist, rather than at the point of failure'. The contract is watertight: fixed price, no fluctuation provision, supplier bears input risk — your board's procurement adviser notes the letter 'has no contractual foundation' and recommends holding the supplier to its bid. Market checks confirm both halves of the story: input costs genuinely moved beyond any bidder's forecast, and the two alternative suppliers quote 30% higher with 14-week mould lead times. The letter is either the beginning of a negotiation or the beginning of an insolvency — possibly both.",
             "evidence":[
               {"label":"Letter","value":"Candid early warning: loss-making, can complete, 'raising now while options exist'"},
               {"label":"Contract","value":"Fixed price, no fluctuation clause — input risk is the supplier's"},
               {"label":"Market","value":"Input moves genuine; alternatives +30% with 14-week mould lead times"},
               {"label":"Exposure","value":"Sole source, 40% of critical path"}],
             "decisions":[
               {"key":"respond","prompt":"Stage 1 — your response to the letter:",
                "options":[
                  {"key":"engage","label":"Treat the early warning as the asset it is: verify the loss with open-book access (their claim, their books), then structure relief that buys something real — a price adjustment tied to verified input indices, in exchange for enhanced security (vesting of moulds and WIP, step-in rights, priority production slots) — priced against the true alternative, which is 30%-plus-14-weeks, not the contract's comfortable zero","quality":100,
                   "consequence":"Open book verifies a genuine 9% loss, not the feared negotiation theatre; relief lands at 6% with vesting and slot guarantees the original contract never had. The units arrive on programme — and when a second supplier fails industry-wide that winter, yours doesn't.",
                   "principle":"A supplier who signals distress early is handing you options — the contract says you needn't pay, but the critical path says you can't afford their failure; buy the relief with protections, priced against the real alternative."},
                  {"key":"hold","label":"Hold them to the bid — the adviser is right, the contract allocated input risk deliberately, and repricing fixed-price contracts on request destroys the meaning of tendering","quality":10,
                   "consequence":"The supplier, refused, does what its letter foreshadowed: reprioritises its order book toward paying customers. Deliveries slip 'for production reasons' that never quite constitute breach; by the time default is provable, the 14-week mould lead time is your programme's problem, at 30% premium, in flood season.",
                   "principle":"Contractual rightness and commercial survival are different questions — a watertight contract with an insolvent counterparty is watertight paper."},
                  {"key":"retender","label":"Start the alternative suppliers' qualification now and let the incumbent's letter compete with real quotes — negotiate, if at all, from a dual-source position","quality":35,
                   "consequence":"Sensible as insurance, ruinous as a lead strategy: the 14-week mould lead time means the 'alternative' matures after the critical path needs units flowing, and the incumbent — reading the qualification enquiries in a small market within days — stops seeing a partnership worth losing money for.",
                   "principle":"Building alternatives is prudent; brandishing unready ones is not leverage, it is a countdown the other side can read."}]},
               {"key":"govern","prompt":"Stage 2 — the board asks how relief squares with procurement discipline. You:",
                "options":[
                  {"key":"paper","label":"Put it through governance as the priced decision it is: the relief's cost versus the verified alternative (premium, lead time, flood-season exposure), the protections obtained, an index-linked mechanism replacing ad-hoc mercy — and an honest note that the tender's risk allocation failed against unforecastable input moves, feeding the next procurement's fluctuation-clause design","quality":100,
                   "consequence":"The board approves a decision instead of ratifying a fait accompli; the audit committee later cites the paper as model practice — relief bounded by verification, bought with security, and converted into a lesson the next tender actually implements.",
                   "principle":"Discretionary relief survives scrutiny only as a documented value-for-money decision — verified need, priced alternative, purchased protections, and a fixed mechanism replacing repeatable mercy."},
                  {"key":"quiet","label":"Handle it as commercial management within delegated authority — boards ratify outcomes, and a 6% adjustment inside contingency needs no governance theatre","quality":15,
                   "consequence":"The adjustment surfaces at audit as an undocumented concession on a watertight contract; the absent paperwork converts a defensible decision into a finding, and the officer who made it into its subject.",
                   "principle":"The decision that is right and unrecorded is indistinguishable, later, from the one that was wrong."}]}],
             "hints":["Price the real alternative first: 30% premium plus 14 weeks plus flood season — that is what 'hold them to it' costs.",
               "Verify before relieving: open book converts a letter into evidence, or exposes it as theatre.",
               "Buy relief, don't grant it — vesting, step-in rights and priority slots are what the distress is worth."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Bought verified relief with vesting and step-in rights — because the watertight contract's alternative was 30% and fourteen weeks."}
            """),

        ("WC-RSC-336", "The calendar the site never agreed to", "Head office planned the transformation by quarters. The plant runs by campaigns.",
            "Enterprise Programmes", "Recovery Planning Lead", "project_management", "advanced", 18,
            """["schedule_analysis","stakeholder_communication","concept_planning"]""",
            """
            {"context":"The enterprise transformation — new ERP, new maintenance regime, new reporting spine across nine manufacturing sites — is eighteen months in and site adoption has collapsed at the three largest plants. Unpicking why, you find the programme calendar was built in head office around fiscal quarters: releases land at quarter boundaries, training in the weeks before, cutover on quarter-start weekends. The plants run on a different clock entirely: the largest runs continuous campaigns with shutdown windows twice a year (already booked two years out); the second is seasonal, at 130% capacity all summer; the third schedules maintenance around its largest customer's certification audits. Each plant, asked to cut over mid-campaign, has quietly deferred — 'go-live achieved' in programme reports means the software is on and the old spreadsheets still run everything. The steering group believes seven of nine sites are live. Two are.",
             "evidence":[
               {"label":"Design","value":"Calendar built on fiscal quarters — releases, training, quarter-start cutovers"},
               {"label":"Reality","value":"Campaign cycles, seasonal peaks, audit-driven maintenance windows — none aligned to quarters"},
               {"label":"Adoption","value":"'Live' sites running parallel spreadsheets; real adoption 2 of 9"},
               {"label":"Governance","value":"Steering believes 7 of 9"}],
             "decisions":[
               {"key":"truth","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"recalendar","label":"Tell steering the real number and bring the fix in the same paper: adoption re-measured by an honest definition (old tools retired, not software installed), and the rollout re-planned around each site's operational calendar — cutover in shutdown windows, training in low-season, the fiscal-quarter cadence retained only for what head office actually controls — with the schedule consequence stated: longer to full rollout, shorter to real adoption","quality":100,
                   "consequence":"Steering absorbs '2 of 9' badly and 'here is the plan that makes it real' well; the largest plant's booked shutdown window in month 4 becomes the first genuine cutover, and its campaign restarts on the new system — the reference case that recruits the other six.",
                   "principle":"A rollout calendar is a claim about other people's time — where the programme's clock and the operation's clock disagree, the operation's wins, because adoption happens on site, not in head office."},
                  {"key":"enforce","label":"Recover the plan as approved: escalate the deferrals as non-compliance, mandate cutover dates through the executive, and dismantle the parallel spreadsheets by policy","quality":0,
                   "consequence":"The mandate lands mid-campaign at the largest plant; the forced cutover meets its first stock reconciliation during a production run, output drops for nine days, and the plant director's account of why — the booked shutdown was four months away — ends the programme director's tenure, not the spreadsheets.",
                   "principle":"Mandating adoption against an operational calendar doesn't defeat the resistance — it schedules the incident that proves the resistance right."},
                  {"key":"soften","label":"Keep the reported position and quietly fix forward — re-phase the remaining sites sensibly while 'maturing' the seven declared sites through an adoption workstream","quality":15,
                   "consequence":"The quiet fix inherits a loud problem: steering keeps making decisions — decommissioning legacy licences, releasing support staff — against seven live sites that are two; the licence decommissioning hits a 'live' plant's actual system of record in month three.",
                   "principle":"A false adoption number is not a reporting nicety — it is an input other decisions are consuming right now."}]},
               {"key":"design","prompt":"Stage 2 — re-planning around nine operational calendars, you:",
                "options":[
                  {"key":"windows","label":"Build the plan from the sites' windows outward: every plant's shutdown, low-season and audit calendar collected as planning constraints, cutovers assigned to real windows with site sign-off as a plan input (not a communication afterward), a repeatable cutover kit so each window needs less programme presence — and the fiscal-quarter reporting recut to track window-readiness, since windows, not quarters, are now the unit of progress","quality":100,
                   "consequence":"The re-cut plan runs eleven months longer on paper and delivers real adoption faster than the old plan's fiction; by the fifth site the cutover kit runs in a long weekend, and two plants ask to move earlier — into windows the programme would never have found from head office.",
                   "principle":"Nine sites means nine calendars — a deliverable rollout is designed from the windows the operation already protects, and progress is measured in windows made, not quarters passed."},
                  {"key":"hybrid","label":"Compromise: keep quarterly release trains for efficiency, with sites choosing which train to board — flexibility inside the existing cadence","quality":25,
                   "consequence":"The trains run on time and half-empty: sites whose windows fall between quarters still defer, now with a legitimate-sounding reason ('waiting for the Q3 train'), and the cadence's efficiency serves the programme office's convenience at the adoption rate's expense.",
                   "principle":"Letting sites choose among the programme's dates is not the same as planning around the sites' dates — the constraint never moved."}]}],
             "hints":["Audit what 'live' means in the reports before planning anything — retired old tools, or installed new ones?",
               "Collect the nine operational calendars first: shutdowns, seasons, audits — those windows are the real plan.",
               "Recut progress reporting to windows, not quarters — you deliver in the operation's units or not at all."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Replanned a nine-site rollout around the calendars the plants actually run on — and made '2 of 9' into the honest start of 9 of 9."}
            """),

        // ═════════════ DECEMBER — handover, closeout, lessons and capstone missions ═════════════
        // ───────────── Project Rescue · stakeholders & story · advanced · deep ─────────────

        ("WC-RSC-337", "A community meeting with one answer left", "Two years of assurances are in the room. The truth has to walk in alone.",
            "Capital Programmes", "Recovery Programme Director", "project_management", "advanced", 18,
            """["stakeholder_communication","governance","professional_ethics"]""",
            """
            {"context":"The multi-site capital programme's community liaison history reaches you as an inheritance: for two years, quarterly public meetings have been told the town's leisure centre — the programme's most visible site — would open 'next summer'. Each assurance was sincere when given and dead within months; the site has consumed three re-plans. The honest current answer is twenty months, driven by a ground-contamination discovery the previous team chose to describe publicly as 'routine groundworks'. Tonight's meeting was called by the council after a local paper photographed the site standing still. Three hundred residents expected; the council leader will share the platform; your communications lead has drafted 'lines to take' that are technically accurate and materially evasive. You have been in post five weeks.",
             "evidence":[
               {"label":"History","value":"Two years of 'next summer' — each sincere, each dead within months"},
               {"label":"Truth","value":"20 months, driven by contamination described publicly as 'routine groundworks'"},
               {"label":"Tonight","value":"300 residents, council leader on the platform, press present"},
               {"label":"Draft","value":"'Lines to take' — technically accurate, materially evasive"}],
             "decisions":[
               {"key":"meeting","prompt":"Stage 1 — tonight you:",
                "options":[
                  {"key":"truth","label":"Retire the lines to take and give the room the whole account: the twenty months and why, the contamination called by its name with the health facts alongside, the history of failed assurances acknowledged as failures — and one commitment sized to be unbreakable: a public milestone plan updated monthly, starting with three near-term milestones the programme can hit","quality":100,
                   "consequence":"The meeting is loud, long and strangely settling: the anger is at the two years of 'next summer', not the twenty months. The paper's headline is harsh and accurate; three months and three met milestones later, the programme has something it hasn't had in two years — a believed date.",
                   "principle":"After serial broken assurances, credibility restarts only at the full truth plus the smallest promise you cannot fail to keep — anything grander is another 'next summer'."},
                  {"key":"lines","label":"Deliver the drafted lines — accurate, calm, professionally managed — and let the detailed recovery plan carry the hard news in written form afterwards","quality":0,
                   "consequence":"A resident reads the 'routine groundworks' line back against the contamination report — obtained by FOI that afternoon — and the meeting becomes about concealment; every future statement now inherits the evening's video clip.",
                   "principle":"Technically-accurate evasion in front of three hundred people is a concealment performed live — and rooms can smell the difference between managed and honest."},
                  {"key":"defer","label":"Use tonight for listening only — acknowledge concerns, take questions away, and return with the full recovery plan at a follow-up in six weeks","quality":20,
                   "consequence":"'They came with nothing' writes itself; the vacuum fills with the FOI'd contamination report anyway, and the six-week follow-up opens against a story the programme no longer authors.",
                   "principle":"A called meeting is a demand for an answer — bringing process to a truth-shaped hole makes the hole the story."}]},
               {"key":"repair","prompt":"Stage 2 — beyond tonight, you rebuild the liaison on:",
                "options":[
                  {"key":"machinery","label":"Consequence-grade machinery, not warmer messaging: the public milestone plan as a standing artifact with misses explained in public, the community given a named senior owner with authority to answer, and every future public commitment pre-cleared against the programme's own risk register — so the organisation can no longer promise what its own analysis doubts","quality":100,
                   "consequence":"The pre-clearing rule bites within a quarter — a well-meaning 'open by Easter' line is stopped because the register carries a live utilities risk against it; the promise that isn't made is the first one in two years that couldn't have been broken.",
                   "principle":"Serial over-promising is a systems failure, not a phrasing failure — the fix is wiring public commitments to the programme's own risk knowledge."},
                  {"key":"cadence","label":"A richer engagement cadence — more frequent meetings, drop-in sessions, a newsletter — so the community hears more, sooner","quality":25,
                   "consequence":"More channels carry the same untested promises faster; volume was never the failure mode, and the Easter line would have gone out in the newsletter too.",
                   "principle":"Increasing the bandwidth of an unreliable source doesn't build trust — it industrialises the disappointment."}]}],
             "hints":["Name what actually broke: not the tone of the assurances, but their relationship to the programme's own risk knowledge.",
               "Restart credibility with the full account plus the smallest unbreakable commitment — not the grandest.",
               "Wire future public promises to the risk register so the organisation cannot say what its analysis doubts."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Walked the whole truth into a room owed two years of 'next summer' — and left with the first believed date."}
            """),

        ("WC-RSC-339", "The regulator's question you saw coming", "Eight months ago it was a finding. Tonight it's a letter with a deadline.",
            "Portfolio Management", "Recovery Portfolio Director", "project_management", "advanced", 16,
            """["governance","stakeholder_communication","assurance"]""",
            """
            {"context":"The regulator's letter asks the portfolio a precise question: how many of its safety-critical maintenance projects have verified — not planned, verified — competence records for contractor supervisors, and what the portfolio's assurance regime does when verification fails. The question is not a surprise: an internal audit eight months ago flagged exactly this gap, its recommendation ('verify, don't accept attestations') was accepted, and the implementing action has sat at 60% complete for two quarters while the portfolio's attention went to a funding crisis. The honest current answer is: verified at four projects of eleven, attestation-only at seven, and the regime's response to failure is untested. The letter allows four weeks. The portfolio board's first instinct, visible in the pre-meeting emails, is to 'present the trajectory' — lead with the four, the programme of work, the intent.",
             "evidence":[
               {"label":"Question","value":"Verified supervisor competence: how many projects, and what happens on failure?"},
               {"label":"History","value":"Internal audit flagged it 8 months ago; accepted action at 60% for two quarters"},
               {"label":"Truth","value":"Verified 4 of 11; attestation-only 7; failure response untested"},
               {"label":"Instinct","value":"Board emails: 'present the trajectory'"}],
             "decisions":[
               {"key":"answer","prompt":"Stage 1 — the response you put to the board:",
                "options":[
                  {"key":"straight","label":"Answer the question asked, in its own structure: four verified, seven attestation-only, failure response untested — followed by the completion plan with dates, the audit trail showing the gap was self-identified, and an offer of a follow-up review when the eleven are done; the trajectory appears after the answer, never instead of it","quality":100,
                   "consequence":"The regulator's reply is pointed about the two stalled quarters and notably uninterested in escalation: self-identified, honestly stated, credibly planned is the profile they de-prioritise. The follow-up review, six months later, closes the file.",
                   "principle":"Regulators grade the answer's honesty before its content — a precise question answered in its own structure signals an organisation that can be left to fix itself."},
                  {"key":"trajectory","label":"Present the trajectory — open with the four verified and the completion programme, characterise the seven as 'transitioning to verified status', and keep the untested failure response for the follow-up conversation","quality":0,
                   "consequence":"The regulator, who wrote a precise question precisely to test for this reflex, reads 'transitioning' as the evasion it is; the response triggers the site inspections it was crafted to avoid, and the inspectors' first request is the internal audit — which shows the portfolio knew, eight months ago, in its own words.",
                   "principle":"Spinning a question you were asked precisely is the one move that converts a regulator from reviewer to investigator — they wrote the question knowing your answer's shape."},
                  {"key":"lawyer","label":"Route the response through external counsel — the question touches enforcement territory, and every word should be privileged and minimal","quality":20,
                   "consequence":"The lawyered minimal answer is technically compliant and relationally ruinous: a cooperative regulator meets a defensive posture, recalibrates accordingly, and the routine follow-up becomes a formal information notice with statutory teeth.",
                   "principle":"Legal minimalism is for adversaries — deploying it against a regulator still treating you as a partner is how you acquire an adversary."}]},
               {"key":"root","prompt":"Stage 2 — on the two stalled quarters, you:",
                "options":[
                  {"key":"why","label":"Fix the class, not the instance: accepted audit actions get delivery-grade treatment — owner, resource, milestone reporting to the audit committee, and an aging alarm when any action stalls — because the real finding is that the portfolio's assurance actions compete for attention with delivery and lose","quality":100,
                   "consequence":"The aging report's first run surfaces five other accepted-and-stalled actions, two of them regulator-visible; all five complete inside the quarter, and the next letter from any regulator finds a portfolio whose self-identified gaps actually close.",
                   "principle":"An accepted audit action that stalls is a promise the organisation made to itself and broke — treat assurance actions as delivery commitments or they will always lose to delivery."},
                  {"key":"sprint","label":"Crash the remaining seven verifications in the four-week window so the response can say eleven of eleven","quality":30,
                   "consequence":"Ten verify; the eleventh — rushed — accepts a records package later found incomplete, and the crash leaves the stalling disease untreated for the next accepted action, which is already aging.",
                   "principle":"Crashing the instance to beautify the answer treats the letter as the problem — the letter was the symptom."}]}],
             "hints":["Answer the precise question in its own structure before adding anything — regulators design questions to test for spin.",
               "Self-identified, honestly stated, credibly planned is the profile that avoids escalation; 'transitioning' is the one that invites it.",
               "The class failure is stalled assurance actions — give them delivery-grade owners, milestones and aging alarms."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Answered the regulator's precise question precisely — four of eleven — and watched honesty de-escalate what spin would have inflamed."}
            """),

        ("WC-RSC-341", "An escalation drafted in anger", "The letter is accurate, devastating, and about to cost the project its last ally.",
            "Energy Networks", "Recovery Programme Manager", "project_management", "advanced", 14,
            """["stakeholder_communication","negotiation","professional_judgement"]""",
            """
            {"context":"Your grid reinforcement project's relationship with the distribution operator's consenting team has curdled, and this morning it produced an artifact: your consents manager's draft escalation letter to the operator's executive, copied to the regulator's liaison office. The letter is factually meticulous — seven documented instances of the operator's team missing its own response deadlines, each with dates and consequences, culminating in the current nine-week stall on the final circuit's approval. It is also unmistakably written in fury: words like 'obstructive', 'bad faith' and 'systemic incompetence' carry the argument. The nine-week stall is real and is now the critical path. The consents manager — your best, and the author of every one of those seven documented instances — wants it sent today. You know one more fact the letter doesn't use: the operator's consenting team lost three of its five engineers this year, and its manager privately asked your consents manager for patience in the spring.",
             "evidence":[
               {"label":"Letter","value":"7 documented missed deadlines, meticulous — and worded in fury, cc regulator"},
               {"label":"Stall","value":"9 weeks on the final circuit; now critical path"},
               {"label":"Context","value":"Operator's team lost 3 of 5 engineers; private plea for patience in spring"},
               {"label":"Author","value":"Your best consents manager, wants it sent today"}],
             "decisions":[
               {"key":"letter","prompt":"Stage 1 — on the draft:",
                "options":[
                  {"key":"rebuild","label":"Keep the evidence, delete the adjectives, change the ask: a same-day rewrite that presents the seven instances neutrally, names the mutual problem (their capacity, your critical path), and proposes the specific machinery — a joint weekly consents surgery, your engineers preparing pack submissions to their format, executive sponsors on both sides — with the regulator copy held in reserve, unmentioned","quality":100,
                   "consequence":"The operator's executive, handed facts plus a workable proposal instead of an accusation, staffs the surgery within a fortnight; the final circuit clears in six weeks with your engineers doing the pack preparation their three missing engineers would have done. The regulator never needed to exist.",
                   "principle":"An escalation's power is its evidence and its exit — the adjectives spend your leverage on feelings, and the cc line converts a counterpart who could help into a defendant who can't."},
                  {"key":"send","label":"Send it — the record is accurate, nine weeks is indefensible whatever their staffing, and the regulator copy is what finally makes operators move","quality":10,
                   "consequence":"The operator's executive, publicly accused of bad faith before the regulator, does what accused executives do: hands the file to compliance, where every future response is lawyered to the maximum timescale. The nine-week stall becomes a by-the-book eighteen, unappealable because every step is now procedurally perfect.",
                   "principle":"Escalation-as-punishment gets you procedure; only escalation-as-proposal gets you speed — and you rarely get to choose twice."},
                  {"key":"soften_only","label":"Soften the language but keep the structure and the regulator copy — firm but professional, with the pressure intact","quality":30,
                   "consequence":"Politeness doesn't neutralise the cc line: the regulator copy is the escalation, whatever the adjectives, and the compliance reflex triggers anyway — courteously.",
                   "principle":"The most aggressive word in the letter is in the address block — tone is not the same decision as audience."}]},
               {"key":"author","prompt":"Stage 2 — with your consents manager, you:",
                "options":[
                  {"key":"honour","label":"Honour the record and redirect the fury: their seven documented instances are the reason the rewrite works — say so — and put them in the room as the surgery's co-chair, owner of the fix their evidence made possible; the anger was earned, and ownership is what it converts into","quality":100,
                   "consequence":"The consents manager runs the surgery with the same rigour that built the seven-instance file; by the third week the operator's manager — the one who asked for patience in spring — is sharing draft responses early. The relationship that produced the letter produces the recovery.",
                   "principle":"The person angriest at a failing interface is usually the one who cares most about it working — overrule the letter, never the evidence, and give the author the fix."},
                  {"key":"overrule","label":"Overrule the letter and take the operator relationship into your own hands — the interface has become too personal for its current owner","quality":20,
                   "consequence":"The interface transfers along with none of its documented history's nuance; your best consents manager updates a CV, and the operator's team quietly notes that documenting failures gets people removed.",
                   "principle":"Removing the author of an uncomfortable record teaches everyone to keep worse records."}]}],
             "hints":["Separate the letter's three components: the evidence, the adjectives, the audience — only one of them helps.",
               "Ask what you want the operator's executive to DO — then write the letter that makes doing it easy.",
               "The author's anger is earned and useful: give it ownership of the fix, not a rebuke."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rewrote a furious-but-accurate escalation into a proposal — and the nine-week stall cleared in six."}
            """),

        ("WC-RSC-343", "The steering pack with two audiences", "One deck, two truths — and both readers are in the same meeting now.",
            "Technology Programmes", "Recovery Delivery Director", "project_management", "advanced", 15,
            """["governance","reporting_integrity","stakeholder_communication"]""",
            """
            {"context":"Investigating why the platform migration's problems reached crisis before governance noticed, you find the mechanism in the steering pack's own version history. For a year, the programme office has maintained two decks per cycle: an internal working version — candid, red-flecked, listing blockers by name — and the steering version, in which each cycle's reds arrive 'contextualised': a red dependency becomes 'amber — mitigations in flight', a slipped milestone becomes 'rephased to protect quality'. No single edit was a lie; the cumulative translation kept the steering committee two quarters behind reality for a year. The practice predates you; the programme office defends it as 'appropriate summarisation for a senior audience'. Next week's steering meeting will be the first since the crisis broke — and the client's CIO has asked, pointedly, to see 'the reporting the programme actually runs on'.",
             "evidence":[
               {"label":"Mechanism","value":"Two decks per cycle for a year: candid internal, 'contextualised' steering"},
               {"label":"Translation","value":"Red dependency → 'amber, mitigations in flight'; slip → 'rephased for quality'"},
               {"label":"Effect","value":"Steering ran two quarters behind reality into a crisis"},
               {"label":"Ask","value":"CIO wants 'the reporting the programme actually runs on'"}],
             "decisions":[
               {"key":"meeting","prompt":"Stage 1 — for next week's meeting, you:",
                "options":[
                  {"key":"one_deck","label":"Kill the dual-deck system in the open: give the CIO the working version as requested, present the two decks' year-long divergence yourself as a governance finding with the mechanism named, and commit to one deck from now on — the candid one, with summarisation done by ordering and emphasis, never by translation of status","quality":100,
                   "consequence":"The meeting is bruising — two quarters of surprises land at once — and it is the last bruising meeting: with one deck, steering starts engaging blockers while they are cheap, and the CIO, who expected to have to excavate the truth, becomes the programme's most useful sponsor.",
                   "principle":"When a reporting system has two versions of the truth, the cover-up is already structural — the only exit is presenting the divergence yourself, before it is presented about you."},
                  {"key":"merge","label":"Produce a converged deck for next week — the working version's substance in the steering version's tone — and quietly retire the dual practice without drawing attention to its history","quality":15,
                   "consequence":"The converged deck reads as a sudden inexplicable deterioration; the CIO, comparing it against the year of green-amber history, asks the obvious question, and the quiet retirement becomes a loud discovery — with you now inside the practice's authorship.",
                   "principle":"You cannot silently exit a misreporting pattern — the correction is itself the disclosure, and it reads far better as a confession than as a discovery."},
                  {"key":"defend","label":"Provide the working version but defend the summarisation practice — senior audiences need synthesis, and steering was never entitled to raw operational noise","quality":10,
                   "consequence":"The CIO lines up three 'contextualised' items against their working-version originals and asks which of the pair was the synthesis; the defence of the practice becomes the meeting's story, and the programme's reporting is placed under client-side review.",
                   "principle":"Summarisation compresses detail; translation changes status — defending the second as the first convinces exactly nobody who has seen both decks."}]},
               {"key":"design","prompt":"Stage 2 — the reporting you rebuild:",
                "options":[
                  {"key":"honest_by_design","label":"One source, tiered by depth, honest at every tier: statuses and blockers identical from working level to steering summary, escalation thresholds written down (what steering must hear regardless of narrative), and the pack's first page always the delta — what changed, what worsened, what needs a decision — so candour is the format, not a personality trait","quality":100,
                   "consequence":"Steering meetings shorten and sharpen: the delta page does in four minutes what the old deck avoided in forty, and when a new red appears in month three it reaches steering the same fortnight it reached the war room — the interval the old system measured in quarters.",
                   "principle":"Reports are honest by architecture or dishonest by drift — one source of truth with depth tiers makes the candid version and the senior version the same document at different zoom."},
                  {"key":"assure","label":"Keep the programme office drafting and add an assurance check — an independent reviewer comparing internal and external versions each cycle","quality":30,
                   "consequence":"The reviewer catches translations for three cycles, goes on leave for one, and the amber drift resumes — the check polices the gap instead of closing it, forever, at a cost.",
                   "principle":"An audit on a two-truth system is a tax on dishonesty, not a cure — remove the second version and there is nothing to police."}]}],
             "hints":["Compare the two decks item-by-item for one cycle — the mechanism is in the translations, not any single edit.",
               "Present the divergence yourself, as a finding — discovered candour reads as confession; excavated candour reads as cover-up.",
               "Rebuild as one source with depth tiers: senior summaries compress detail, never status."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Retired a two-truth steering pack by presenting its year of divergence myself — and made candour the format."}
            """),

        ("WC-RSC-345", "Re-baselining without losing the story", "The new plan is honest. The old plan's history is about to be deleted with it.",
            "Construction", "Recovery Planning Lead", "project_management", "advanced", 13,
            """["change_control","baseline_management","reporting_integrity"]""",
            """
            {"context":"The stadium fit-out's recovery has reached the sane conclusion: the original baseline is unachievable and a re-baseline is approved in principle. Then you see the implementation plan. The planning team — competent, exhausted, and keen for relief — proposes to re-baseline in the scheduling tool by overwriting: new dates become the baseline, variance counters reset to zero, and next month's report will show the project 'on programme' for the first time in a year. The client's project director has verbally blessed this ('a clean start helps everyone'). The proposal deletes, as a side effect, the only quantified record of where the eleven months went: the as-built history of which packages slipped, by how much, against which causes — the evidence base for the delay claim the contractor has intimated, for the client's own counterclaims, and for the next project's estimating.",
             "evidence":[
               {"label":"Approved","value":"Re-baseline agreed in principle — the old plan is unachievable"},
               {"label":"Proposal","value":"Overwrite in the tool: variances reset, next report shows 'on programme'"},
               {"label":"Blessing","value":"Client PD: 'a clean start helps everyone'"},
               {"label":"At stake","value":"The only quantified slip history — claims, counterclaims, estimating"}],
             "decisions":[
               {"key":"method","prompt":"Stage 1 — you implement the re-baseline as:",
                "options":[
                  {"key":"preserve","label":"A versioned re-baseline with the story intact: the original baseline archived as an immutable version with its full variance history, a closure analysis written now — where the eleven months went, by package and cause, agreed line-by-line with the contractor while memories and records are fresh — and the new baseline opening with variance zero but lineage explicit: 'Baseline 2, succeeding Baseline 1, closure analysis ref A'","quality":100,
                   "consequence":"Eight months later the contractor's delay claim arrives, built on selective recollection — and meets a jointly-agreed closure analysis signed before anyone knew which lines would matter. The claim settles in weeks at a fraction of its ask; the estimating team mines the same document for the next bid.",
                   "principle":"A re-baseline is a succession, not an amnesty — the new plan can start clean only because the old plan's story is closed, agreed and filed, not deleted."},
                  {"key":"overwrite","label":"As proposed — overwrite and reset; the history exists in old report PDFs if anyone ever needs it, and the team's morale is worth more than archival tidiness","quality":0,
                   "consequence":"The claim arrives built on the contractor's contemporaneous records — which survived — against your PDFs, which reconstruct nothing at package level; the quantum expert's fee alone exceeds the cost of the closure analysis nobody wrote, and the settlement reflects who kept the better story.",
                   "principle":"In delay disputes, the party with the intact history writes the narrative — resetting your own variance record is unilateral disarmament with a morale benefit."},
                  {"key":"dual","label":"Run both baselines in parallel — report progress against the new one but maintain the old one live for claims purposes","quality":25,
                   "consequence":"Two live baselines produce two answers to every question within a month; the claims value of the old baseline decays anyway as its maintenance becomes perfunctory, and the planning team's relief — the re-baseline's one human benefit — never arrives.",
                   "principle":"History should be closed and preserved, not kept artificially alive — an archived truth outlasts a neglected one."}]},
               {"key":"story","prompt":"Stage 2 — the closure analysis is drafted. The contractor balks at agreeing causes line-by-line. You:",
                "options":[
                  {"key":"agree_facts","label":"Split facts from fault: agree the factual record now — what slipped, when, what preceded what — with causation positions noted where disputed rather than resolved, because the facts are perishable and the arguments are not; both parties sign the chronology, reserving their interpretations","quality":100,
                   "consequence":"The contractor signs a facts-only chronology it couldn't refuse — its own records built half of it — and when the claim comes, the dispute is confined to interpretation of an agreed record: the cheap kind of dispute, resolved by negotiation instead of experts.",
                   "principle":"You will never agree fault while money hangs on it — agree the chronology while it is cheap, and let the argument, if it must come, stand on shared facts."},
                  {"key":"force","label":"Make line-by-line causation agreement a condition of the re-baseline — no clean start without a clean allocation of blame","quality":20,
                   "consequence":"The condition stalls the re-baseline itself for nine weeks — the project keeps reporting against a fiction everyone has abandoned — and the causation fight happens anyway, early, angrier, and with the recovery's momentum as collateral.",
                   "principle":"Holding the plan hostage to a blame settlement gets you neither — sequence the perishable before the contestable."}]}],
             "hints":["A re-baseline changes the plan; ask what it does to the record — versioning and overwriting are different acts.",
               "Write the closure analysis now, while records are fresh and nobody knows which lines will matter in a claim.",
               "Agree facts line-by-line and let causation positions sit noted — chronology is perishable, argument keeps."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave a broken baseline a clean succession instead of an amnesty — and the delay claim met a signed chronology."}
            """),

        // ───────────── Risk Room · closeout & capstone · evidence and sequencing ─────────────

        ("WC-RSK-338", "Quantifying the risk nobody would name", "Every workshop danced around it. The data has been naming it all along.",
            "Life Sciences", "Programme Risk Analyst", "project_controls", "advanced", 14,
            """["risk_management","evidence_analysis","organisational_culture"]""",
            """
            {"context":"Closing out the clinical trials expansion's risk process, you reconcile the register against what actually happened. The programme's four material delay events share a root cause no register entry ever carried: the sponsor organisation's own decision latency. Approvals that the plan assumed would take a week averaged five; the average is calculable from the programme's own approval logs. Workshop transcripts show the risk being approached and abandoned four times — 'governance responsiveness' proposed and softened to 'stakeholder alignment', once explicitly parked after a steering member said naming it would be 'career-adventurous'. The closeout report is your last artifact; the sponsor's next programme — same governance, triple the size — mobilises in eight weeks.",
             "evidence":[
               {"label":"Pattern","value":"4 material delays; shared root cause: sponsor decision latency"},
               {"label":"Data","value":"Planned 1-week approvals averaged 5 — from the programme's own logs"},
               {"label":"Suppression","value":"Risk approached and softened 4× in workshops; once parked as 'career-adventurous'"},
               {"label":"Stakes","value":"Successor programme, same governance, 3× size, mobilises in 8 weeks"}],
             "decisions":[
               {"key":"closeout","prompt":"Your closeout report:",
                "options":[
                  {"key":"name_it","label":"Names it with the data carrying the weight: approval latency presented as measured fact — planned versus actual, from the sponsor's own logs, with its delay contribution quantified — framed as a system property to design for, not a criticism of individuals, and carried forward as the successor programme's highest-rated inherited risk with a concrete mitigation: decision service-levels designed into its governance","quality":100,
                   "consequence":"The report survives review because averages from the sponsor's own logs are unarguable in a way adjectives never are; the successor programme mobilises with decision SLAs and a delegation matrix — and its first quarter runs approvals at nine days, not twenty-five.",
                   "principle":"The risk nobody will name can still be measured — data does the naming impersonally, and a quantified system property survives politics that would kill an opinion."},
                  {"key":"soften","label":"Carries it as 'stakeholder alignment complexity' one last time — the finding's substance with survivable wording, because a closeout report that antagonises the sponsor helps nobody","quality":10,
                   "consequence":"The euphemism transfers perfectly: the successor programme inherits 'alignment complexity', staffs a stakeholder engagement plan, and rediscovers decision latency the expensive way — at triple scale, in its own four delay events.",
                   "principle":"A risk renamed to be survivable has been disarmed, not transferred — euphemism is how organisations arrange to learn the same lesson twice."},
                  {"key":"escalate_person","label":"Takes the four workshop suppressions to the sponsor's audit chair as a risk-culture finding — the process failure outranks the risk itself","quality":30,
                   "consequence":"The culture finding is true, unquantified, and lands as an accusation; the audit chair opens a governance review that reports after the successor programme has mobilised — without the one number that would have changed its design.",
                   "principle":"Culture findings without data arrive as blame and leave as process; the log-derived average was the finding that could act in eight weeks."}]},
               {"key":"mechanism","prompt":"For the successor programme's design, you propose:",
                "options":[
                  {"key":"sla","label":"Decision latency managed as a first-class schedule quantity: every approval in the plan carries the measured historical duration (not the aspirational one), governance commits to service-levels with a delegation route when they're missed, and latency is reported monthly beside delivery progress — the sponsor's speed made as visible as the supplier's","quality":100,
                   "consequence":"Planning on measured latency adds six honest weeks to the successor's schedule — and removes the twenty dishonest ones its predecessor lost; by quarter two, two governance boards have delegated approval classes rather than appear in the latency report again.",
                   "principle":"What the plan assumes about its own sponsor is a risk like any other — plan on measured behaviour, and make the measurement public enough to improve the behaviour."},
                  {"key":"buffer","label":"Schedule buffers sized to absorb historical latency without the visibility apparatus — protection without provocation","quality":25,
                   "consequence":"The buffers absorb the latency invisibly, so the latency grows to fill them — approvals that averaged five weeks average seven by year-end, unexamined, because nothing any longer makes slowness cost anyone anything.",
                   "principle":"Buffering a behavioural risk without measuring it is a subsidy — what you absorb silently, you encourage."}]}],
             "hints":["The unnameable risk is usually measurable — check the approval logs against the plan's assumptions.",
               "Let averages do the accusing: a system property survives review; an adjective does not.",
               "Carry it forward as the successor's top inherited risk, with decision service-levels as the mitigation."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Named the career-adventurous risk with the sponsor's own approval logs — and the successor programme planned on measured truth."}
            """),

        ("WC-RSK-340", "The reserve the business case never had", "The rollout's contingency was spent before day one — by the estimate itself.",
            "Climate Infrastructure", "Programme Risk Analyst", "project_controls", "advanced", 24,
            """["risk_management","cost_management","concept_planning"]""",
            """
            {"context":"The EV charging rollout's full business case is three weeks from submission, and you are its risk reviewer. The case is polished: 2,400 charge points, four years, quantified benefits, and a contingency of 8% presented as 'aligned with comparator programmes'. Your review reassembles where that 8% came from — and finds it was reverse-engineered: an early draft carried 15%, derived from a proper quantified risk assessment; when the benefit-cost ratio dipped below the funding threshold, the contingency was stepped down — 15, 12, 10, 8 — across four drafts, with the QRA never re-run. The risks the QRA priced have not changed: grid connection cost volatility (the dominant driver), wayleave negotiation rates, and ground condition variance across 2,400 sites. Meanwhile the delivery team's own pilot data — 60 sites — shows connection costs running 22% above the case's central assumption. The SRO, who signed the drafts down, believes the case 'holds together at 8%'.",
             "evidence":[
               {"label":"History","value":"QRA said 15%; stepped to 8% across four drafts; QRA never re-run"},
               {"label":"Driver","value":"BCR dipped below funding threshold at 15%"},
               {"label":"Pilot data","value":"60 sites: connection costs +22% vs the case's central assumption"},
               {"label":"Position","value":"SRO believes it 'holds together at 8%'"}],
             "decisions":[
               {"key":"review","prompt":"Stage 1 — your review finding:",
                "options":[
                  {"key":"reprice","label":"State it structurally: the contingency was set by the BCR target, not the risk — re-run the QRA with the pilot's 22% connection data (which will push above 15%, not restore it), and present the SRO with the honest set of options: re-scope to fewer sites, phase against evidence gates, negotiate the grid volatility with the DNOs — anything but a reserve number chosen for the spreadsheet's answer","quality":100,
                   "consequence":"The re-run QRA lands at 17%; the case re-scopes to 2,100 points with a DNO framework agreement capping connection volatility, passes the threshold honestly at 13% — and when connection costs bite in year two, the reserve holds, because it was sized by the risk and not the wish.",
                   "principle":"Contingency derived backward from an approval threshold is not a reserve, it is a rounding of hope — the case that cannot afford its own risk assessment cannot afford its own delivery."},
                  {"key":"note","label":"Flag the 15-to-8 history as a review observation with a recommendation to 'keep contingency under review post-approval' — the funding window is real and the case is otherwise sound","quality":0,
                   "consequence":"The case approves at 8%; year two's connection invoices consume the reserve by site 700, and the emergency re-approval — sought mid-programme, credibility spent — costs a nine-month pause that the honest 13% case would never have needed.",
                   "principle":"'Under review post-approval' is where under-provisioned cases send their reckoning — the reserve you couldn't defend at approval is the crisis you meet at delivery."},
                  {"key":"block","label":"Refuse sign-off outright — a reverse-engineered contingency is a governance integrity failure and the case should not proceed in any form this cycle","quality":30,
                   "consequence":"The blocked case misses the funding window; the programme loses a year, the reviewer wins the argument — and the re-submission, next cycle, contains the same re-scoping options that were available three weeks before this cycle's deadline.",
                   "principle":"A reviewer's job is making the honest case possible, not just making the dishonest one impossible — blocking without the repair path spends a year to prove a point."}]},
               {"key":"evidence","prompt":"Stage 2 — the SRO resists: 'the pilot is only 60 sites'. You:",
                "options":[
                  {"key":"gates","label":"Turn the objection into the mechanism: if 60 sites is too few to re-price the case, it is too few to justify 2,400 — propose evidence-gated phasing where tranche one's actuals formally update the QRA and unlock tranche two, making the case's own delivery its risk model's data source","quality":100,
                   "consequence":"The SRO — offered a structure where scepticism about the pilot cuts both ways — accepts the gates; tranche one's 400 sites confirm the connection premium at 19%, the QRA updates, and tranche two proceeds on numbers nobody can call small-sample.",
                   "principle":"When a sponsor discounts your evidence as thin, agree — and make thickening it a condition the programme's structure enforces."},
                  {"key":"argue","label":"Defend the pilot statistically — 60 sites against a stable cost driver is ample, and the sample-size objection is motivated reasoning","quality":25,
                   "consequence":"The statistics are right and the meeting is lost: the argument becomes reviewer-versus-SRO on methodology, the one framing where seniority wins, and the case proceeds at 8% with your objection minuted and inert.",
                   "principle":"A methodological argument you win on paper and lose in the room changes nothing — structure beats statistics when the audience owns the decision."}]}],
             "hints":["Trace the contingency's history across drafts — a number that steps down without the QRA re-running was set by the answer.",
               "The pilot's 22% is the case's most important number: it moves the QRA up, not back to where it was.",
               "If the sponsor calls the evidence thin, build gates that thicken it — delivery as the risk model's data source."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a contingency reverse-engineered from the funding threshold — and rebuilt the case so its own delivery re-priced the risk."}
            """),

        ("WC-RSK-342", "Sequencing a season that cannot slip", "Nine risks, one immovable opening night — rank what gets attacked first.",
            "Events & Venues", "Risk & Readiness Director", "project_controls", "expert", 26,
            """["risk_management","schedule_analysis","decision_quality"]""",
            """
            {"context":"You direct risk for a national festival's build: a six-week construction window on parkland, an opening night fixed by broadcast contracts, and this year a doubled main arena. Your consolidated register holds nine credible threats to opening night, and your mitigation capacity — crews, senior attention, money — can seriously attack perhaps five before the window opens. The nine: (1) main-stage roof engineering sign-off pending a wind-load re-analysis; (2) the parkland's drainage under a wet-season forecast; (3) a single supplier for the doubled arena's seating, already flagging capacity; (4) power distribution redesign unapproved by the licensing authority; (5) crew accommodation shortfall in a resort town at peak season; (6) the broadcast compound's fibre route crossing a protected tree line; (7) volunteer stewarding recruitment at 60% of plan; (8) a ticketing platform migration scheduled mid-build; (9) local road closures contested by two parish councils. Rank your attack — what gets the five serious slots — knowing that opening night moves for nothing.",
             "evidence":[
               {"label":"Window","value":"6-week build; opening night contractually immovable"},
               {"label":"Capacity","value":"Serious mitigation available for ~5 of 9 risks"},
               {"label":"Hard gates","value":"Roof sign-off, licensing approval, seating supply — each can stop the build or the show"},
               {"label":"Soft-looking","value":"Stewarding 60%, ticketing migration, parish councils, accommodation, drainage, fibre"}],
             "decisions":[
               {"key":"rank","prompt":"Stage 1 — the ranking principle for the five slots:",
                "options":[
                  {"key":"gates_first","label":"Rank by show-stopping power × lead time: the four that can legally or physically cancel opening night and have the longest resolution clocks — roof sign-off (1), licensing approval (4), sole-source seating (3), drainage under the wet forecast (2) — take four slots now; the fifth goes to stewarding (7), because a licensed, built festival without safe crowd management is also a cancelled one, and recruitment has the slowest compounding clock of the rest","quality":100,
                   "consequence":"Roof re-analysis surfaces a bracing change while steel is still at the fabricator; licensing approves the redesign with two weeks' margin; the seating supplier — engaged seriously — takes a part-order to a second fabricator; drainage matting is down before the rain. The unfunded four cost money and sleep; none costs the show.",
                   "principle":"With an immovable date, priority is stop-power times lead time — attack what can cancel the show and takes longest to fix, and accept noisy discomfort from what merely hurts."},
                  {"key":"probability","label":"Rank by probability × impact as the register scores them — the top five expected-value risks get the slots, whatever their character","quality":25,
                   "consequence":"Expected value promotes the likely-and-medium — accommodation, stewarding, parish councils — over the unlikely-and-fatal; the licensing redesign, scored low-probability because 'the authority has always approved', returns with conditions in week four, and no slot was holding it.",
                   "principle":"Expected value is blind to the difference between expensive and fatal — an immovable date changes the maths from averages to gates."},
                  {"key":"loud","label":"Rank by current escalation heat: the seating supplier and the parish councils are the two shouting today, and momentum with live conflicts beats analysis of quiet ones","quality":10,
                   "consequence":"The loud two absorb the best month; the quiet wind-load re-analysis — nobody was shouting about physics — lands its bracing change with the steel already erected, and the change happens at height, at night, at triple cost, one week from gates-open.",
                   "principle":"Volume is a measure of stakeholder discomfort, not of stop-power — the fatal risks are usually the quiet ones with long clocks."}]},
               {"key":"unfunded","prompt":"Stage 2 — the four unfunded risks still exist. You:",
                "options":[
                  {"key":"cheap_moves","label":"Give each a cheap structural move instead of a serious attack: ticketing migration (8) deferred past the festival by decision (costless); fibre route (6) re-surveyed onto the service road alignment (one consultant-day); parish councils (9) handed to the council's own events officer with the traffic order evidence pack (delegation); accommodation (5) part-solved with a crew campsite option priced now, triggered only if the shortfall persists at week two","quality":100,
                   "consequence":"Three of the four never mature — the deferral, the re-route and the delegation cost less than a day of senior attention combined; the campsite trigger fires at week two and houses forty crew adequately. The five serious attacks were never diluted.",
                   "principle":"Unfunded risks don't get ignored, they get structure: defer, re-route, delegate, pre-decide — the moves that cost decisions rather than capacity."},
                  {"key":"spread","label":"Thin the five serious attacks to seven — nobody can defend leaving four credible risks wholly unattacked before an immovable date","quality":15,
                   "consequence":"Seven attacks at five attacks' capacity: the seating supplier engagement loses the commercial depth that found the second fabricator, and the drainage work loses a week — the two dilutions surface, respectively, as a seating shortfall scare and a flooded car park on opening weekend.",
                   "principle":"Spreading serious capacity to look defensible is the least defensible move available — thin attacks fail exactly like no attack, but cost more."}]}],
             "hints":["Separate what can cancel the show from what can merely hurt it — the date moves for nothing.",
               "Multiply stop-power by lead time: the fatal risk with the longest fix clock owns the first slot.",
               "The unfunded tail gets structural moves — defer, re-route, delegate, pre-decide — never silence."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave five serious slots to the risks that could cancel opening night — and structure, not silence, to the four that couldn't."}
            """),

        ("WC-RSK-344", "Order the recovery, not just the claim", "The depot change blew up mid-mobilisation. Six moves are right — in one order only.",
            "Transport Depots", "Recovery Commercial Director", "project_finance", "expert", 22,
            """["change_control","claims_management","decision_quality"]""",
            """
            {"context":"Mobilisation of the depot modernisation has hit a compound event: the train operator's late stabling requirement — issued as a directive, never priced — has forced re-design of the maintenance shed layout; the shed contractor has stopped detailed design, claiming it cannot proceed at risk; the steelwork order sits at the fabricator, unreleased, with the mill slot expiring in three weeks; the operator disputes that its directive constitutes a change at all ('clarification of existing requirements'); your own quantity surveyor has assembled a persuasive global claim; and the project board wants 'the commercial position resolved' before any money moves. Six actions are on your desk, all sound: (A) release the steel in the current design's sizes, protected by the fabricator's confirmation that section changes remain possible at cost until week 6; (B) instruct the contractor to resume design under the contract's change-pending mechanism; (C) issue formal change notice against the operator's directive; (D) submit the global claim; (E) negotiate the directive's status with the operator's commercial head; (F) re-baseline the mobilisation programme. Sequence them.",
             "evidence":[
               {"label":"Clock","value":"Mill slot expires in 3 weeks; design stopped; directive disputed"},
               {"label":"Board","value":"Wants 'commercial position resolved' before money moves"},
               {"label":"Six moves","value":"A: release steel · B: resume design (change-pending) · C: change notice · D: global claim · E: negotiate status · F: re-baseline"},
               {"label":"Trap","value":"Every move is sound; several orders are fatal"}],
             "decisions":[
               {"key":"sequence","prompt":"Stage 1 — your opening sequence:",
                "options":[
                  {"key":"protect_first","label":"C, B, A — then E: change notice first (it starts the contractual clock and converts the dispute into a governed process), design resumed under change-pending the same day (the mechanism exists precisely so disputes don't stop work), steel released inside the fabricator's protected window (the only irreversible deadline on the desk) — and THEN negotiate status with the operator, from inside a running process rather than instead of one","quality":100,
                   "consequence":"The notice transforms the operator's posture — 'clarification' is a position you hold until a contractual clock is running against it; design resumes with eleven days lost, not eleven weeks; the steel makes the mill slot with two section changes at week 5, at cost, as the fabricator promised. The negotiation, held inside the process, settles the directive as a change in five weeks.",
                   "principle":"Sequence by irreversibility and clock-starting power: notify to start the process, resume work under the mechanism built for disputes, commit the wasting asset inside its protection window — and negotiate from within a structure, never in place of one."},
                  {"key":"resolve_first","label":"E, then C if negotiation fails, then B and A once status is clear — the board's instinct is right: commit nothing while the change's very existence is disputed","quality":10,
                   "consequence":"The operator, facing no clock, negotiates at the speed of the comfortable; week 3 expires with the steel unreleased, the re-booked mill slot lands four months out, and 'resolve before committing' has quietly converted a priceable change into an unpriceable delay.",
                   "principle":"Negotiating before starting the contractual clock hands the other side your deadlines as leverage — the wasting assets don't wait for the argument to finish."},
                  {"key":"claim_first","label":"D leading — the global claim consolidates every entitlement into one commanding position, and its weight brings the operator to the table faster than piecemeal notices","quality":0,
                   "consequence":"The global claim lands as a declaration of war during mobilisation: the operator's lawyers take over the interface, the change-pending mechanism — which needed day-to-day cooperation — freezes with the design, and the steel dies in the crossfire. The claim itself, built on a stopped project, grows monthly.",
                   "principle":"A global claim is an endgame instrument — fired at mobilisation it doesn't accelerate resolution, it replaces the working relationship the recovery needed with a litigation posture nobody can work inside."}]},
               {"key":"close","prompt":"Stage 2 — the change settles; F and D remain on the desk. You:",
                "options":[
                  {"key":"rebase_retire","label":"Re-baseline now with the settled change priced in — and retire the global claim deliberately: its valid strands were absorbed into the change settlement, and the residual items go through ordinary contractual channels item-by-item, with the QS's assembly work archived as the record that made the settlement honest","quality":100,
                   "consequence":"The re-baseline gives the board its resolved position on real numbers; the retired claim's two residual items settle at measured value inside a month; and the operator relationship — never litigated — carries the depot through commissioning, where it is needed most.",
                   "principle":"A claim assembled and never fired often earns more than one submitted — it priced the settlement; retiring it, visibly, buys the relationship the next phase runs on."},
                  {"key":"keep_live","label":"Re-baseline, but keep the global claim live and updating as leverage for the commissioning phase's inevitable frictions","quality":20,
                   "consequence":"The 'live' claim leaks — claims always do — and commissioning opens with the operator reading every cooperative gesture as claim-building; the leverage never gets used and the suspicion never gets retired.",
                   "principle":"A standing claim is not leverage, it is ambient hostility with a document number — leverage you can't visibly retire poisons what it was kept to protect."}]}],
             "hints":["Find the wasting assets first: the mill slot and the stopped design are losing value daily; the argument isn't.",
               "The change notice isn't escalation — it is the clock that makes negotiation happen at your speed instead of theirs.",
               "The global claim is an endgame tool: let it price the settlement from the drawer, then retire it visibly."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced six sound moves in the one order that saved the steel, the design and the settlement — and retired the claim unfired."}
            """),

        ("WC-RSC-347", "The shadow project in the minor-works log", "Ninety-one 'minor' items, none worth escalating, jointly worth eleven weeks.",
            "Joint Ventures", "Recovery Planning Lead", "project_management", "advanced", 18,
            """["scope_management","schedule_analysis","governance"]""",
            """
            {"context":"The joint venture's delivery office reports every project 'substantially on programme' — and every completion date keeps drifting. Your diagnostic finds the mechanism: a snagging-and-residuals backlog that has become a shadow project. Across the JV's four sites, ninety-one items sit in the 'minor works' log — punch-list carry-overs, small design reconciliations, as-built documentation gaps, warranty-condition fixes. Each was individually triaged as 'not schedule-critical' at the moment it arose, which was true. Collectively they now consume 30% of the finishing trades' capacity, gate seven handover sequences, and — because the log sits outside the schedule — appear in no critical path, no report and no forecast. The completion forecasts assume finishing capacity the backlog has already spent. Nobody decided this; ninety-one people decided one ninety-first of it.",
             "evidence":[
               {"label":"Log","value":"91 'minor' items across 4 sites — each individually non-critical at triage"},
               {"label":"Reality","value":"30% of finishing capacity consumed; 7 handover sequences gated"},
               {"label":"Visibility","value":"Outside the schedule — no critical path, no report, no forecast"},
               {"label":"Effect","value":"Forecasts assume capacity the backlog already spent"}],
             "decisions":[
               {"key":"surface","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"schedule_it","label":"Bring the shadow project into the light as work: load all ninety-one items into the schedule with durations, trade demands and logic links to the handovers they actually gate, re-run the network, and publish the honest completion picture — the backlog managed as a scoped, resourced workstream with its own burn-down, not a log of regrettable dust","quality":100,
                   "consequence":"The re-run network moves two completion dates by five and seven weeks — the drift that was already happening, now visible in advance instead of monthly in arrears; the backlog workstream, properly resourced, burns down 60 items in eight weeks because finishing crews stop being ambushed by it.",
                   "principle":"Work that consumes capacity but appears in no schedule is not minor, it is invisible — and invisible work doesn't stop eating the plan just because nobody wrote it down."},
                  {"key":"blitz","label":"Order a backlog blitz — a dedicated fortnight, all sites, clear the log before it grows again — and keep the schedule as is","quality":30,
                   "consequence":"The blitz clears 40 items and generates 15 — blitzed work snags too — and without the schedule integration, the survivors and newcomers resume their invisible feeding; the fortnight bought relief, not repair.",
                   "principle":"A purge without a system change is a diet without a habit — the log refills at the rate the process that filled it still runs."},
                  {"key":"threshold","label":"Tighten the triage: raise the bar for what may enter the minor-works log, forcing more items into formal change and schedule control at birth","quality":25,
                   "consequence":"Future items route better; the standing ninety-one continue gating seven handovers from their off-books limbo, and the completion forecasts stay wrong for exactly as long as the existing backlog stays unscheduled.",
                   "principle":"Fixing the intake while ignoring the inventory manages the next problem instead of the present one."}]},
               {"key":"prevent","prompt":"Stage 2 — to stop the shadow re-forming, you:",
                "options":[
                  {"key":"aggregate","label":"Give the class a guardian: minor items still triage individually, but the log carries standing aggregate metrics — total trade-hours, capacity share, handovers gated — reviewed monthly with a tripwire that forces schedule integration when the aggregate crosses a threshold; individually-rational decisions get a collectively-rational check","quality":100,
                   "consequence":"Four months later the aggregate tripwire fires at 12% of finishing capacity — a third of the level that caused the crisis — and that month's review schedules the offending items while they are still two weeks of work instead of eleven.",
                   "principle":"Death-by-aggregation is defeated by measuring the aggregate — no individual triage can see what only the sum knows."},
                  {"key":"owner","label":"Appoint a backlog owner per site with authority to chase items to closure","quality":30,
                   "consequence":"The owners chase energetically inside the same blindness: items close faster individually while the aggregate — still unmeasured — regrows to critical mass between anecdotes.",
                   "principle":"Ownership without aggregate measurement is diligence applied to the wrong unit."}]}],
             "hints":["Sum the log: trade-hours, capacity share, gated handovers — the aggregate is the finding.",
               "Load the items into the network and let the critical path say what 'minor' has been costing.",
               "The lasting control measures the class: aggregate tripwires, because no single triage can see the sum."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Scheduled a 91-item shadow project the reports couldn't see — and the drift stopped being a monthly surprise."}
            """),

        ("WC-RSC-348", "A forecast the trend does not support", "The plan says the rate will double. The rate has been flat for nineteen weeks.",
            "Coastal Resilience", "Recovery Planning Lead", "project_management", "advanced", 16,
            """["schedule_analysis","forecasting","evidence_analysis"]""",
            """
            {"context":"The coastal resilience scheme — 14 kilometres of revetment and wall — reports a completion forecast that requires placement rates to double from next month and hold at the doubled rate for the remaining two seasons. The stated basis: 'full mobilisation of the second rock delivery route and learning-curve maturity'. Your review of the actuals: placement has run at a stable rate for nineteen weeks, through the first route's 'full mobilisation' announcement, two learning-curve claims, and one previous forecast that also required a doubling that never came. The physical constraints are structural: tidal working windows, a single haul road shared with the public in summer, and rock supply that arrives by sea state's permission. The second delivery route is real but feeds the same haul road. The forecast's author — the delivery contractor's planner — privately concedes the number is 'the date the client can hear, worked backwards'.",
             "evidence":[
               {"label":"Forecast","value":"Requires placement rate ×2 from next month, held for two seasons"},
               {"label":"Actuals","value":"Flat rate for 19 weeks — through one prior 'doubling' forecast"},
               {"label":"Constraints","value":"Tidal windows, one shared haul road, sea-state-gated rock supply; new route feeds same road"},
               {"label":"Author","value":"'The date the client can hear, worked backwards'"}],
             "decisions":[
               {"key":"confront","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"capacity","label":"Rebuild the forecast from demonstrated capacity: the nineteen-week rate as the base case, uplift only where a named constraint verifiably changes (the second route earns its increment only if haul-road modelling shows headroom), the tidal and sea-state math done explicitly — and take the client the honest date with its drivers, plus the two capacity investments that could genuinely improve it: a second discharge point bypassing the haul road, and winter storage buying independence from sea state","quality":100,
                   "consequence":"The honest forecast lands eleven weeks later than the fiction and survives contact with reality — the client, angry for a fortnight, funds the discharge point, which does what no forecast ever did: moves the actual rate 40%. The scheme finishes four weeks inside the honest date.",
                   "principle":"A forecast is a claim about capacity, and capacity is demonstrated, not asserted — rates that have been flat through two mobilisation announcements don't double because a date needs them to."},
                  {"key":"monitor","label":"Accept the forecast provisionally with a hard checkpoint: if month one's doubling doesn't materialise, the forecast re-opens — trust, but verify on a deadline","quality":20,
                   "consequence":"Month one delivers 15% — 'ramp-up effects' — and the checkpoint slides a month, as checkpoints against hope do; the honest re-forecast finally happens in autumn, after the season it could have redesigned.",
                   "principle":"A checkpoint on a forecast its own author disbelieves isn't verification, it is scheduled postponement of the argument."},
                  {"key":"escalate","label":"Report the planner's private admission to the client — a forecast worked backwards from an acceptable date is misreporting, and the client is entitled to know","quality":15,
                   "consequence":"The admission, weaponised, gets the planner removed and the contractor's guard up; the next forecast is produced by someone more careful — about what they say near you — and is worked backwards just the same.",
                   "principle":"Shooting the honest messenger of a dishonest system gets you a dishonest system with silent messengers."}]},
               {"key":"regime","prompt":"Stage 2 — the forecasting regime you leave behind:",
                "options":[
                  {"key":"demonstrated","label":"Rate-based forecasting as the contract's shared method: forecasts derive from rolling demonstrated rates by activity, uplifts require a named, dated, verifiable capacity change signed by both planners, and the client sees the rate chart beside every date — so the next 'doubling' has to argue with a graph in the room","quality":100,
                   "consequence":"The regime's first test comes at the winter re-forecast: a proposed 25% uplift is halved in the joint session because the storage capacity behind it is only two-thirds built — a correction that costs one meeting instead of one season.",
                   "principle":"The antidote to backwards-worked dates is a forward-working method both sides own — demonstrated rates make optimism prove itself before the plan spends it."},
                  {"key":"penalty","label":"Recommend the client hold the contractor commercially to the doubled-rate forecast — accountability for numbers stops numbers being invented","quality":20,
                   "consequence":"The commercial pressure produces commercially-shaped reporting: placement gets recounted to flatter the rate (part-placed sections claimed whole), and the client learns the true position later than ever, from a survey.",
                   "principle":"Penalising a fictional forecast doesn't make it true — it makes the reporting fictional too."}]}],
             "hints":["Plot the demonstrated rate against every past forecast's required rate — the graph is the argument.",
               "Uplifts must name their constraint change: the second route feeds the same haul road; the math must say so.",
               "Sell the honest date with the investments that could beat it — capacity moves rates; dates don't."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Replaced a worked-backwards date with a demonstrated-rate forecast — and the investment it justified beat both."}
            """),

        ("WC-RSC-349", "The crew that was promised twice", "Commissioning starts Monday. The specialists arriving are the ones leaving.",
            "Advanced Manufacturing", "Recovery Programme Manager", "project_management", "advanced", 15,
            """["resource_management","commissioning","stakeholder_communication"]""",
            """
            {"context":"The pilot production ramp enters commissioning on Monday, and the vendor's specialist crew — six process engineers who install, calibrate and prove the coating line — is double-booked. The vendor's scheduler, it emerges, promised the same crew to your ramp and to a customer in another country whose penalty clauses are harsher; the vendor's proposal, delivered Friday, is to split: three engineers to you, three abroad, 'with remote support making up the difference'. Your commissioning plan assumed six on-site for four weeks: the calibration sequences are pair-worked for verification, the proving runs need the vendor's sign-off authority present, and your own operators' training rides on shadowing the full crew. The ramp feeds a customer qualification window in nine weeks that your commercial team calls 'reputationally binary'.",
             "evidence":[
               {"label":"Plan","value":"6 vendor engineers, 4 weeks on-site: paired calibration, sign-off authority, operator shadowing"},
               {"label":"Offer","value":"3 on-site + 3 abroad + 'remote support'"},
               {"label":"Cause","value":"Vendor double-booked; other customer has harsher penalties"},
               {"label":"Stakes","value":"Customer qualification window in 9 weeks — 'reputationally binary'"}],
             "decisions":[
               {"key":"respond","prompt":"Stage 1 — your response to the vendor:",
                "options":[
                  {"key":"resequence","label":"Reject the flat split and renegotiate the shape: re-sequence commissioning so the genuinely pair-dependent and sign-off-dependent work compresses into a front-loaded fortnight with all six engineers (the vendor's other customer's own critical fortnight, their scheduler admits, starts later), then accept the 3+3 split for the single-stream verification tail — with the vendor's sign-off authority formally delegated to their lead on your site and the remote support tested against a named task list before the crew halves","quality":100,
                   "consequence":"The vendor, offered a shape that solves both bookings instead of a grievance that solves neither, moves its other mobilisation eleven days; the paired fortnight completes calibration, the delegated sign-off keeps proving runs legal, and the split tail loses three days against plan — inside the nine-week window with margin.",
                   "principle":"When a resource is promised twice, the negotiation isn't about your entitlement to six — it is about re-shaping both demands so the constraint is shared instead of won; the counterparty's calendar has slack yours can use."},
                  {"key":"enforce","label":"Hold the vendor to the contract: six engineers, four weeks, on-site, or formal default with the qualification window's consequences on their account","quality":15,
                   "consequence":"The vendor — arithmetic being what it is — defaults on you rather than the harsher-penalty customer, offers liquidated compensation, and the money arrives punctually while the qualification window closes; the contract was right and the ramp is still late.",
                   "principle":"Enforcement that pushes a cornered counterparty toward the cheaper breach wins the account and loses the objective — check whose penalties they fear before you posture."},
                  {"key":"accept","label":"Accept the 3+3 with remote support — half the specialist crew plus modern remote tooling is workable, and goodwill with the vendor matters across the fleet purchase to come","quality":10,
                   "consequence":"The unpaired calibrations run at half speed with double the rework; the sign-off authority is abroad and asleep at the two moments the proving runs need decisions; the window is missed by eight days and the goodwill purchased turns out to be one-directional.",
                   "principle":"Accepting a resourcing fiction to preserve a relationship preserves neither — the vendor remembers who absorbed their failure quietly, and prices it in next time."}]},
               {"key":"train","prompt":"Stage 2 — the operator-training loss from the halved tail, you:",
                "options":[
                  {"key":"structure","label":"Restructure rather than mourn: shadowing concentrated into the all-six fortnight with your best operators pulled onto shifts matching the vendor's work, the tail's remote sessions converted into recorded, task-indexed procedure captures your trainers own afterwards, and the vendor's return visit for the 90-day service contractually re-scoped into a training residency","quality":100,
                   "consequence":"The operators absorb more in the concentrated fortnight than the original diffuse month would have delivered; the recorded captures become the plant's induction library, and the training residency at day 90 lands exactly when the operators know which questions matter.",
                   "principle":"Training capacity lost in one shape can be rebuilt in another — concentration, capture and a re-scoped return visit beat mourning the original plan's diffusion."},
                  {"key":"later","label":"Defer operator training to a post-qualification vendor visit — commissioning is the priority and training can follow the pressure","quality":25,
                   "consequence":"Post-qualification, the line runs production and can't be given to trainees; the operators run a process they never shadowed, the vendor visit becomes troubleshooting, and the first quarter's yield pays the tuition.",
                   "principle":"Training deferred past the moment the plant goes live isn't deferred, it is converted into operating error."}]}],
             "hints":["Find which activities truly need all six — pair-work and sign-off authority — and compress those into the shared window.",
               "Check the other customer's real calendar before believing the split is fifty-fifty forever.",
               "Delegated sign-off in writing, remote support proven against named tasks — before the crew halves, not after."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-shaped a double-booked crew into a front-loaded fortnight — and made the split week the plan, not the failure."}
            """),

        ("WC-RSC-350", "An instruction the contract never defined", "'Make it operational' — three words, no clause, and the meter already running.",
            "Framework Programmes", "Recovery Commercial Director", "project_finance", "advanced", 13,
            """["contract_management","commissioning","negotiation"]""",
            """
            {"context":"Three weeks before the framework programme's flagship facility hands over, the client's operations director issued a site instruction: 'contractor to make the building operational for day-one service, including all activities necessary'. The contractor complied enthusiastically and is now performing — and recording — work the contract never scoped: furniture assembly, IT peripheral setup, staff way-finding signage, even stocking consumables. The framework contract defines completion by a technical criteria schedule; 'operational for day-one service' appears nowhere. The contractor's commercial team, smelling a time-and-materials bonanza, has booked 40 people against the instruction and submits daily records for signature. The operations director meant, it turns out, 'finish the snagging and make sure the doors unlock'. The daily records are politely worded, contractually careful — and already total a five-figure sum growing daily.",
             "evidence":[
               {"label":"Instruction","value":"'Make operational for day-one service, including all activities necessary' — no contractual definition"},
               {"label":"Response","value":"40 people booked T&M: furniture, IT peripherals, signage, consumables"},
               {"label":"Intent","value":"Ops director meant: finish snagging, doors unlock"},
               {"label":"Meter","value":"Daily records submitted for signature; five figures and growing"}],
             "decisions":[
               {"key":"stop","prompt":"Stage 1 — today, you:",
                "options":[
                  {"key":"supersede","label":"Close the open instruction with a defined one: issue a superseding instruction referencing the contract's completion criteria, itemising exactly what remains (the snag list, by number), expressly excluding the rest — while dealing honestly with the interregnum: work already performed under the ambiguous instruction gets valued under the contract's variation rules, because the client's own instruction created the ambiguity and pretending otherwise loses at adjudication","quality":100,
                   "consequence":"The meter stops at day four's total; the contractor, whose position was 'we did what the instruction said', accepts variation-rule valuation for the interregnum without a fight — it is roughly what honest measurement gives them — and handover proceeds on the criteria schedule everyone can read.",
                   "principle":"An open-ended instruction is closed by a defined one, not by an argument about what the first one meant — and the ambiguity's cost belongs to its author, which is the fact that makes settling it cheap."},
                  {"key":"repudiate","label":"Reject the daily records wholesale: the instruction was plainly never intended to scope furniture assembly, no reasonable contractor could think otherwise, and signature is refused pending withdrawal of the claims","quality":10,
                   "consequence":"'What the client plainly intended' meets 'what the client's authorised representative signed' at adjudication, and loses; the award covers the records plus interest plus a finding about instruction discipline that follows the framework into its next procurement.",
                   "principle":"Contracts are performed in words, not intentions — repudiating your own side's signed instruction is the one argument weaker than the bonanza it challenges."},
                  {"key":"negotiate_only","label":"Skip instruments and go straight to a commercial settlement meeting — cap the total, split the difference, move on to handover","quality":30,
                   "consequence":"The settlement caps this instruction's cost and prices the precedent generously: the framework has six facilities to go, and every future ambiguity now has a market rate — the contractor's estimators have filed the mechanism under 'repeatable'.",
                   "principle":"Settling an ambiguity without closing it buys one quiet handover and teaches the supply chain where the till is."}]},
               {"key":"discipline","prompt":"Stage 2 — the framework has six more handovers. You institute:",
                "options":[
                  {"key":"instrument_control","label":"Instruction discipline with teeth: site instructions issue only against the contract's defined terms, anything touching scope or money routes through commercial review before issue (same-day service, so operations never has a reason to bypass it), a standard handover-scope instruction template built from the completion criteria — and the ops directors briefed not with a rule but with this instruction's invoice","quality":100,
                   "consequence":"The invoice makes the briefing unforgettable; the same-day commercial review clears 30 instructions across the next two handovers, catches two scope-touching ones, and the framework's handover costs stop containing surprises.",
                   "principle":"Instruction discipline fails when the compliant route is slower than the bypass — make review same-day and the rule enforces itself; the past invoice is the only training aid anyone remembers."},
                  {"key":"authority","label":"Withdraw site-instruction authority from operations staff entirely — commercial issues all instructions from now on","quality":25,
                   "consequence":"Operations, stripped of authority mid-mobilisation, routes urgent needs through informal 'requests' the contractor treats exactly like instructions — same ambiguity, now without even the paper.",
                   "principle":"Removing authority without replacing the service pushes the need underground — the fix is faster good instructions, not fewer people who can issue them."}]}],
             "hints":["The exposure isn't what was meant — it's what was signed; close the open instruction with a defined one.",
               "Value the interregnum honestly under the variation rules: the ambiguity's author pays, which is why it settles cheap.",
               "Fix the system with speed, not prohibition: same-day commercial review beats withdrawn authority."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Stopped a T&M bonanza by superseding the instruction that fed it — and paid honestly for the ambiguity we authored."}
            """),

        ("WC-RSC-351", "The acceleration that cost more than the delay", "Every measure worked. The programme still finished later — and poorer.",
            "Healthcare Estates", "Recovery Programme Director", "project_management", "advanced", 18,
            """["change_claims_recovery","cost_management","commissioning"]""",
            """
            {"context":"The hospital trust's capital plan flagship — a surgical block — was eight weeks late at the structural milestone, and the board bought an acceleration package: extended hours, weekend working, additional gangs, out-of-sequence fit-out. Six months on, you are asked to review what happened. The findings are uncomfortable: the acceleration measures each 'worked' by their own metric (hours were worked, gangs were added), but the block is now eleven weeks late and the acceleration premium is spent. The mechanics are visible in the records: out-of-sequence fit-out generated rework when the sequence's logic reasserted itself (ceilings closed before above-ceiling services passed inspection — twice); extended hours produced a documented productivity slide and a quality dip that fed the snag list; the additional gangs diluted supervision until an NCR spike forced a re-inspection regime that consumed the very hours the premium had bought. The commissioning phase — clinical validation, deep cleans, equipment calibration — cannot be accelerated at any price and was never part of the analysis. The board wants to know 'whether to accelerate again' for the remaining phases.",
             "evidence":[
               {"label":"Package","value":"Extended hours, weekend gangs, out-of-sequence fit-out — premium fully spent"},
               {"label":"Outcome","value":"8 weeks late → 11 weeks late, six months on"},
               {"label":"Mechanics","value":"Rework from broken sequence (×2), productivity slide, NCR spike → re-inspection burden"},
               {"label":"Ignored","value":"Commissioning phase can't be accelerated at any price — never analysed"}],
             "decisions":[
               {"key":"verdict","prompt":"Stage 1 — your review's answer to 'accelerate again':",
                "options":[
                  {"key":"anatomy","label":"Answer with the anatomy, not a yes or no: acceleration failed here because it attacked activities' durations while breaking the things that actually govern completion — sequence logic, supervision ratios, first-time quality — so the question for remaining phases is which of their constraints respond to money at all; the honest map shows commissioning is incompressible, the fit-out tail responds to sequence protection (not hours), and the one genuine opportunity is early procurement of the commissioning-gating equipment — cheap, unglamorous, and worth more weeks than any weekend shift","quality":100,
                   "consequence":"The board, shown why the last premium bought negative time, funds the equipment early-procurement and a sequence-protection rule instead of a second premium; the remaining phases hold their dates without a single extended shift, and the trust's next project inherits the anatomy as standard gateway analysis.",
                   "principle":"Acceleration is a claim that duration is the binding constraint — when sequence, supervision and quality govern instead, buying hours buys rework; diagnose what actually responds to money before spending it on time."},
                  {"key":"better","label":"Recommend accelerating again but managed properly this time — the measures were right, the execution was loose; with tighter sequence control and supervision ratios, the same tools will work","quality":15,
                   "consequence":"The second premium, 'managed properly', meets the same physics with better paperwork: supervision ratios hold, the NCR spike is smaller, and the block gains one week for a seven-figure spend — because the binding constraint was never hours the second time either.",
                   "principle":"Repeating an intervention with better hygiene doesn't fix a wrong diagnosis — 'this time we'll do it carefully' is what the first premium said too."},
                  {"key":"never","label":"Recommend against acceleration as a class — the evidence shows it destroys value, and the remaining phases should run at natural pace with dates moved honestly","quality":30,
                   "consequence":"Honest, and one notch too sweeping: the dates move, the board accepts it — and the early-procurement opportunity that would have genuinely recovered four commissioning weeks goes unexamined, because 'acceleration' was rejected as a category instead of dissected as a mechanism.",
                   "principle":"The lesson of failed acceleration isn't that time can't be bought — it is that only the binding constraint's price matters; categorical refusal throws out the one purchase that works."}]},
               {"key":"claims","prompt":"Stage 2 — the acceleration premium's commercial tail: the contractor claims the rework arose from 'employer-directed out-of-sequence working'. You:",
                "options":[
                  {"key":"honest_split","label":"Split it on the record's own lines: the direction to work out-of-sequence was the employer's — its rework consequences are legitimately claimable and cheaper acknowledged than arbitrated; the productivity slide and NCR spike under the contractor's own extended-hours resourcing are theirs, and the records distinguish the two cleanly — settle on that split, and fold the settlement into the board's education about what the premium really bought","quality":100,
                   "consequence":"The split settles in three weeks because it follows the documents rather than the positions; the board's final acceleration accounting — premium, rework, settlement, minus one week gained — becomes the most-quoted page of the review, and the trust's gateway process gains an acceleration-anatomy checklist.",
                   "principle":"When an acceleration goes wrong, its costs sort by who directed what — settling on the record's split is cheaper than defending the indefensible half to protect the defensible one."},
                  {"key":"resist","label":"Resist the claim in full: the contractor executed the measures and owns their consequences — the employer bought outcomes, not activities","quality":20,
                   "consequence":"The employer's own instruction to break sequence is Exhibit A; the global defence loses at adjudication, taking the legitimately-defensible extended-hours costs down with it, and the award reads as though the employer learned nothing — which the local press quotes.",
                   "principle":"Defending your own directions as if they were the contractor's ideas converts a partial liability into a total one."}]}],
             "hints":["Audit each measure against its own metric AND completion — 'worked' and 'helped' are different findings.",
               "Find the binding constraint per phase: sequence, supervision, quality and commissioning respond to different currencies.",
               "The claims split follows who directed what — the record already knows; settle on its lines."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Autopsied an acceleration that bought negative three weeks — and found the four free ones nobody had priced."}
            """),

        ("WC-RSC-352", "Charging points, uncharged responsibilities", "Four hundred vehicles arrive in spring. One requirement is still looking for a parent.",
            "Fleet & Logistics", "Recovery Programme Manager", "project_management", "advanced", 16,
            """["scope_management","commissioning","stakeholder_communication"]""",
            """
            {"context":"The fleet transition — four hundred electric vans replacing diesel across nine depots — reaches commissioning with a hole where a requirement should be. The programme's scope documents all say the same three words: 'depot charging infrastructure'. What nobody owns: the load-management software that shares each depot's grid connection across its chargers. The charger vendor supplies 'local load balancing per charger bank'; the energy contractor delivered 'connection capacity per the agreed schedule'; the fleet software team scoped 'telematics and route optimisation'. The gap is invisible until you model a winter evening: every van returning 5–7pm, every charger demanding full rate, and six of nine depots' connections tripping on aggregate demand the moment utilisation passes 60%. Vehicles arrive in fourteen weeks. The load-management market is real but lead times run twelve to sixteen weeks, and every vendor's product assumes integration hooks into charger firmware that the installed chargers may or may not expose.",
             "evidence":[
               {"label":"Gap","value":"Site-level load management: vendor has 'per-bank', energy has 'capacity', software has 'telematics' — nobody has the depot"},
               {"label":"Physics","value":"Winter 5–7pm return: 6 of 9 depots trip above ~60% utilisation"},
               {"label":"Clock","value":"Vans in 14 weeks; load-management lead times 12–16 weeks"},
               {"label":"Unknown","value":"Installed chargers' firmware integration hooks — unverified"}],
             "decisions":[
               {"key":"own","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"parent","label":"Give the orphan a parent today and run two clocks at once: assign the requirement to the programme (not a vendor) with a named engineer, start the firmware-hook verification on the installed chargers this week — it gates every option — and pursue procurement and an operational fallback in parallel: staggered return-and-charge schedules per depot, which the fleet team can design now and which makes 60% utilisation survivable even if software slips","quality":100,
                   "consequence":"The firmware audit finds hooks on seven depots' chargers and a retrofit path on two; procurement lands week 13 with the fallback schedules already tested at the pilot depot — spring arrival runs on managed charging at six depots and managed timetables at three, and no connection trips.",
                   "principle":"An orphaned requirement discovered late needs an owner, a technical unblocking, and an operational fallback simultaneously — the parallel paths are the plan; any single path is a bet."},
                  {"key":"procure_fast","label":"Straight to emergency procurement — the lead time consumes the whole window, so the order must be placed this week, integration questions resolved during delivery","quality":20,
                   "consequence":"The order ships against unverified hooks; at week 11 the vendor's integration survey finds two depots' chargers expose nothing usable, and the workaround — firmware upgrades through the charger vendor — joins the queue behind the arrival date. The fallback nobody designed gets improvised in April.",
                   "principle":"Procurement against an unverified integration assumption isn't speed, it is a deferred discovery with a delivery date."},
                  {"key":"defer_fleet","label":"Slow the vehicle arrivals to match the infrastructure — phase deliveries depot-by-depot as load management commissions","quality":30,
                   "consequence":"Contractually possible at a price — the OEM's slot penalties are real — and strategically timid: the operational fallback that made full delivery workable was never explored, and the programme pays to avoid a problem the fleet team could have scheduled around.",
                   "principle":"Slowing the mission to match the infrastructure is the answer only after operational mitigations are exhausted — depots have timetables before they have software."}]},
               {"key":"class","prompt":"Stage 2 — the class problem: three contracts, each complete, jointly incomplete. You:",
                "options":[
                  {"key":"integration_scope","label":"Run an integration-scope review across the whole programme now: every requirement that lives BETWEEN contracts — data flows, control handoffs, aggregate behaviours no single vendor owns — mapped on one page with an owner each, because a programme that found one orphan at commissioning has more, and cheaper places to find them than the next winter evening","quality":100,
                   "consequence":"The review finds two more orphans: billing-data reconciliation between charger vendor and energy supplier, and the depot fire-safety interlock's interface to charging control. Both get owners while they are paperwork; neither becomes April's surprise.",
                   "principle":"Contracts scope what vendors sell; nobody's contract scopes the space between them — integration requirements need a deliberate owner-mapping pass, not faith in the sum of the parts."},
                  {"key":"vendor_fix","label":"Amend the charger vendor's contract to absorb site-level load management — closest scope wins the orphan","quality":25,
                   "consequence":"The vendor accepts the variation, prices its captivity accordingly, and delivers its own product — which manages its own chargers beautifully and treats the two depots with mixed charger estates as someone else's problem. The class question was never asked.",
                   "principle":"Assigning an integration requirement to one party's commercial interest solves the instance and re-buries the class."}]}],
             "hints":["Model the aggregate: per-charger and per-bank behaviour say nothing about what the depot's connection sees at 6pm.",
               "Verify the firmware hooks before ordering anything — every product's promise assumes them.",
               "The fallback is operational, not technical: return-and-charge timetables make the physics survivable while software arrives."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the requirement three complete contracts jointly forgot — and ran owner, firmware audit and fallback timetables in parallel."}
            """),

        ("WC-RSC-353", "The handover date that moved twice", "Third time telling the operator a date. There will not be a fourth.",
            "Highways", "Recovery Project Manager", "project_management", "advanced", 14,
            """["schedule_planning","commissioning","stakeholder_communication"]""",
            """
            {"context":"The highway upgrade's handover to the maintenance operator has been missed twice — each time announced with confidence, each time broken by the same class of event: surprises in the technology commissioning (first the tunnel ventilation integration, then the incident-detection cameras' acceptance failing on night-time false positives). The operator has now mobilised winter crews twice for nothing, billed standby both times, and its contract director opens the recovery meeting with the only demand that matters: 'a date you will actually hit'. Your commissioning team's honest position: the remaining work is testable in seven weeks IF nothing else surfaces — but the two misses came precisely from things that surfaced. The systems integrator, chastened, has produced a 900-line outstanding-works register; nobody has risk-weighted it. Political pressure wants the earliest defensible date; the operator's trust wants the latest certain one; both pressures meet in you at Thursday's joint board.",
             "evidence":[
               {"label":"History","value":"Two missed handover dates — both broken by commissioning surprises"},
               {"label":"Cost","value":"Operator mobilised twice, standby billed twice, trust spent"},
               {"label":"Position","value":"7 weeks IF nothing surfaces; 900-line register, un-risk-weighted"},
               {"label":"Demand","value":"'A date you will actually hit' — Thursday"}],
             "decisions":[
               {"key":"date","prompt":"Stage 1 — what you bring Thursday:",
                "options":[
                  {"key":"criteria","label":"Not a date — a mechanism that produces one: the 900 lines triaged into handover-blocking versus post-handover items (jointly with the operator's engineers, so the split is theirs too), the blocking set risk-weighted, and handover re-defined as a criteria-complete event with a rolling three-week confidence window — the operator mobilises on criteria-met, not on promises, and sees the burn-down weekly","quality":100,
                   "consequence":"The joint triage marks 640 lines post-handover — the operator's engineers, in the room, agree more readily than their contract director would ever have — and the criteria-based handover lands nine weeks later at the first date nobody announced and everybody hit. Standby claims stop, because mobilisation followed evidence.",
                   "principle":"After serial missed dates, the currency isn't a better date — it is a visible mechanism the other side co-owns; certainty is rebuilt from criteria and burn-down, never from confidence."},
                  {"key":"padded","label":"Bring twelve weeks — seven plus honest contingency for the surprise class — and commit to it as the final, padded, unbreakable date","quality":25,
                   "consequence":"The padding absorbs the third surprise (a fibre-loop failure, week 9) but not the fourth (winter weather closing the night-closure windows); the 'unbreakable' date breaks by six days, and six days after two misses costs more trust than six weeks would have cost at the start.",
                   "principle":"Padding defends against the surprises you can size — serial-surprise programmes need mechanisms that absorb the unknown, not margins that bet on its magnitude."},
                  {"key":"early","label":"Bring seven weeks with an acceleration package behind it — the political pressure is real, and the operator responds to momentum, not process","quality":0,
                   "consequence":"The third announced date meets the third surprise in week five; the operator's contract director stops attending joint boards and starts writing to the client's chief executive, and every future letter opens with the list of three.",
                   "principle":"Announcing the same bet a third time isn't confidence, it is a character reference for the fourth letter."}]},
               {"key":"surprise","prompt":"Stage 2 — the surprise class itself (integration unknowns), you:",
                "options":[
                  {"key":"hunt","label":"Hunt the remaining unknowns instead of waiting for them: a two-week integration stress campaign — night-condition tests, failure-mode injections, cross-system scenario runs designed jointly with the operator's maintenance engineers — run NOW, in parallel with the burn-down, so the fourth surprise happens in week two under controlled conditions instead of in week eight under handover pressure","quality":100,
                   "consequence":"The campaign surfaces two genuine finds — a power-fail recovery sequence that strands the ventilation dampers, and a camera-to-control-room latency spike under full load — both fixed inside the window; handover week is, for the first time in the project's life, boring.",
                   "principle":"Programmes bitten twice by 'things that surface' should go surface things — adversarial integration testing converts ambush into schedule."},
                  {"key":"monitor","label":"Strengthen the watch: daily commissioning stand-ups, escalation triggers, the integrator's register reviewed weekly at your level","quality":30,
                   "consequence":"The watching is impeccable and passive; the fourth surprise — the damper sequence — surfaces on its own schedule in week eight, is escalated within the hour, and delays handover anyway. Attention is not detection.",
                   "principle":"Vigilance finds surprises faster; only testing finds them earlier — the difference is the schedule."}]}],
             "hints":["The operator's demand is really for a mechanism, not a number — co-owned criteria beat announced dates.",
               "Triage the 900 lines jointly: the operator's engineers will bless a split their contract director never would.",
               "Go hunting: adversarial integration tests turn week-eight ambushes into week-two findings."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Replaced a third promised date with co-owned criteria and a surprise hunt — and hit the first date nobody announced."}
            """),

        ("WC-RSC-354", "A handover the team was not ready for", "The asset is perfect. The people receiving it have never seen one.",
            "Energy Utilities", "Recovery Project Manager", "project_management", "advanced", 13,
            """["resources_leadership","commissioning","stakeholder_communication"]""",
            """
            {"context":"The substation replacement is technically ready: commissioned, snag-free, documentation exemplary. The handover review exposes the problem nobody scheduled: the receiving operations team isn't ready to operate it. The new substation is digital — IEC 61850 protection, remote switching, condition-based monitoring — and the operator's local team has spent thirty years on the electromechanical predecessor. Their training records show two of eight engineers completed the vendor's course (the other six were 'released when operationally possible', which never came); the team's own risk assessment for live switching operations hasn't been rewritten for the new interfaces; and the shift supervisor privately tells you his people will 'work around the digital layer and operate it like the old one' — which the protection philosophy specifically forbids. The client's asset management office wants the handover certificate signed this month to close its capital-year books; the operations director, three levels up from the shift team, has already accepted the date.",
             "evidence":[
               {"label":"Asset","value":"Commissioned, snag-free, documented — technically handover-ready"},
               {"label":"People","value":"2 of 8 engineers trained; live-switching risk assessment unrewritten"},
               {"label":"Signal","value":"Supervisor: team will 'operate it like the old one' — which the protection design forbids"},
               {"label":"Pressure","value":"Certificate wanted this month for capital-year close; ops director already accepted"}],
             "decisions":[
               {"key":"handover","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"split","label":"Split what the pressure has fused: propose commercial handover this month (certificate signed, capital books closed, asset transferred) with operational transition staged behind it — the project's commissioning engineers embedded with the shift team for eight weeks as authorised operators, the six outstanding training releases now scheduled as a condition, and the risk assessment rewritten jointly before unsupervised live switching — with the supervisor's 'work around it' remark, anonymised, as the paper's exhibit A","quality":100,
                   "consequence":"The books close on time, which evaporates the pressure's only real constituency; the embedded eight weeks converts the shift team from resisters to owners — their input catches two interface improvements the designers adopt — and unsupervised operation begins with trained people and a rewritten risk assessment nobody had to die for.",
                   "principle":"Handover is two transfers wearing one certificate — ownership and operability; when the calendar needs the first, split it from the second rather than counterfeiting both."},
                  {"key":"sign","label":"Sign — the asset meets every technical criterion, training is the operator's own statutory duty, and a project cannot withhold a certificate to manage another organisation's competence","quality":0,
                   "consequence":"Legally accurate; the near-miss arrives in month two — an engineer defeats a digital interlock to 'operate it like the old one' during a fault — and the investigation's timeline note that the project knew, in writing, lands your certificate signature beside the supervisor's remark.",
                   "principle":"A handover you know lands on an unready team isn't completed risk transfer, it is documented foresight — the file remembers who knew what the day the certificate was signed."},
                  {"key":"refuse","label":"Refuse to certify until the operator demonstrates readiness — the project's duty of care outranks the capital calendar","quality":30,
                   "consequence":"Principled and jurisdictionally clumsy: the refusal is overruled above your head within a fortnight — the ops director already accepted — and the staged-transition idea that would have actually fixed the readiness gap never got tabled because the meeting became about your authority.",
                   "principle":"When you lack the power to block, blocking is theatre — the influential move is the structured alternative that gives the powerful a better yes."}]},
               {"key":"legacy","prompt":"Stage 2 — for the programme's five remaining substations, you:",
                "options":[
                  {"key":"orat","label":"Make operational readiness a tracked workstream from day one: each site's handover plan carries people-criteria beside technical ones — training completions, rewritten procedures, supervised-operation hours — reported at the same board as construction progress, so the next 'released when operationally possible' shows up as a red milestone eighteen months early instead of a supervisor's aside eighteen days late","quality":100,
                   "consequence":"Site two's training releases slip in month three — visible now — and the board trades two engineers' release against a contractor backfill while it costs a rota adjustment; sites three through six hand over with the operational-readiness line green before the technical one.",
                   "principle":"Operational readiness fails slowly and gets discovered suddenly — track the people-milestones with the concrete ones and the discovery moves upstream to where it is cheap."},
                  {"key":"contract","label":"Push readiness obligations into the operator's contract — training completion as a formal precondition of each future handover date","quality":25,
                   "consequence":"The obligation is agreed, unmonitored, and discovered failed at handover four — contractually the operator's breach, operationally everyone's delay; the clause allocated the blame the tracking would have prevented.",
                   "principle":"A precondition nobody tracks is a lawsuit scheduled for the moment it matters — obligations need dashboards, not just clauses."}]}],
             "hints":["Name the two transfers inside 'handover' — the books need one, the switchgear needs the other.",
               "The supervisor's aside is the most important line in the file: unready teams operate new assets like old ones.",
               "Give the calendar its certificate and the team its eight embedded weeks — then track people-readiness like concrete."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Signed the certificate the calendar needed and staged the transition the team needed — and stopped fusing the two."}
            """),

        ("WC-RSC-355", "The tender clarification that changed the job", "Answer 47 of 212 quietly rewrote the cooling load. Nobody re-read the estimate.",
            "Data Centres", "Recovery Commercial Director", "project_finance", "advanced", 18,
            """["procurement","concept_planning","governance"]""",
            """
            {"context":"The data-centre build's main contract is three weeks from award, and your pre-award assurance review finds the problem in the clarification log. During tender, bidders asked 212 questions. Answer 47 — issued by the client's technical advisor in week two — responded to a cooling query by stating the facility 'shall accommodate future rack densities of up to 40kW'. The employer's requirements said 25kW; the business case was built on 25kW; the technical advisor 'answered aspirationally' after a conversation with the client's sales team that nobody minuted. Under the tender rules, clarifications amend the requirements. Two of three bidders re-priced for 40kW-capable cooling infrastructure (the spread between them suddenly explicable); the third — the current lowest — appears to have missed it and priced 25kW. Award recommendation is drafted for the lowest bidder. The client's sales team, consulted now, says 40kW capability 'would be commercially transformative but was never budgeted'.",
             "evidence":[
               {"label":"Answer 47","value":"'Up to 40kW rack densities' — issued as clarification, amends the requirements"},
               {"label":"Baseline","value":"ERs and business case: 25kW; the 40kW answer never re-entered either"},
               {"label":"Bids","value":"Two priced 40kW cooling; lowest appears to have priced 25kW"},
               {"label":"Client","value":"Sales: 'transformative but never budgeted'"}],
             "decisions":[
               {"key":"award","prompt":"Stage 1 — on the award, you:",
                "options":[
                  {"key":"reset","label":"Stop the award and repair the competition's basis before anyone signs: the client must first decide 25 or 40 as a business-case decision (priced, at board level, this fortnight) — then the tender concludes on the chosen basis with all bidders holding equal information: confirmation-of-requirement to all three, the mispriced bidder given the same opportunity to re-price the delta as the others had, under the tender rules' clarification-correction provisions","quality":100,
                   "consequence":"The board, seeing 40kW priced honestly for the first time, chooses 25kW with a defined upgrade path — a decision, not a drift; the re-based tender keeps all three bidders, the 'lowest' bid rises to second once it prices what the others had, and the award survives the losing bidder's lawyers because the process visibly repaired itself.",
                   "principle":"A clarification that changed the requirements changed the competition — award on a basis the client hasn't chosen and the bidders don't share, and the contract starts with a defect nothing downstream can cure."},
                  {"key":"award_low","label":"Proceed with the lowest bidder — their price met the published requirements as they read them, the 40kW answer was ultra vires aspiration, and re-opening a concluded tender three weeks from award invites challenge from everyone","quality":10,
                   "consequence":"Award lands; month two's design review forces the 40kW question into the open, and the answer — a seven-figure change order to the winner, or a formal de-scope the other bidders' lawyers read as proof the competition ran on unequal information — arrives with the challenge window still open.",
                   "principle":"An information asymmetry in a tender doesn't dissolve at award — it matures into either a change order or a challenge, and usually both."},
                  {"key":"quiet_descope","label":"Award to the lowest and issue a day-one instruction confirming 25kW — formally retiring answer 47 before it can bite","quality":20,
                   "consequence":"The two 40kW bidders, comparing notes at an industry event as losers do, reconstruct the story: they priced an amended requirement, the winner didn't, and the employer papered it over post-award. The procurement challenge is filed with the correspondence attached.",
                   "principle":"Retiring the asymmetry after using it is the version of the story that reads worst in a courtroom — repair before award or explain after it."}]},
               {"key":"advisor","prompt":"Stage 2 — the mechanism that produced answer 47, you:",
                "options":[
                  {"key":"control","label":"Put clarification authority under change control for the remaining packages: technical answers that touch scope, capacity or performance route through the same approval as a requirements change (same-day turnaround, so the log keeps moving), every answer cross-referenced against the business case by a named reviewer — and the advisor's 'aspirational answer' becomes the briefing example, not a disciplinary case, because the next one is prevented by process, not fear","quality":100,
                   "consequence":"The remaining two packages process 300+ clarifications without incident; the control catches one more scope-touching answer at draft — a substation capacity 'confirmation' — which routes to the board in days rather than surfacing in a bid spread months later.",
                   "principle":"Clarification logs are a requirements-change channel wearing a Q&A costume — anything that can amend the contract needs the contract-change discipline, at Q&A speed."},
                  {"key":"muzzle","label":"Restrict clarification answers to verbatim quotes from the published requirements — no interpretation, no additions, nothing to control","quality":20,
                   "consequence":"Bidders, denied real answers, price their uncertainty: the next package's bids carry visible risk premiums and its clarification log fills with 'unable to confirm' — the asymmetry problem is solved by making everyone equally ignorant, at the portfolio's cost.",
                   "principle":"The cure for uncontrolled answers is controlled answers, not none — tenders price what you refuse to clarify."}]}],
             "hints":["Read the clarification log as contract text — under most tender rules, that is exactly what it is.",
               "The client owes the first decision: 25 or 40 is a business-case question, not a procurement one.",
               "Repair equality before award: all bidders, same information, same re-pricing opportunity — the order of operations is the defence."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a tender clarification that had quietly rewritten the cooling load — and repaired the competition before award instead of the contract after it."}
            """),

        ("WC-RSC-356", "Scope growth in committee minutes", "No change order ever signed. Forty decisions, each 'noted', jointly a second project.",
            "Capital Programmes", "Recovery Commercial Director", "project_finance", "advanced", 16,
            """["change_claims_recovery","governance","cost_management"]""",
            """
            {"context":"Mobilising the capital programme's delivery phase, you reconcile the contracted scope against what the design actually shows — and find the delta lives in eighteen months of committee minutes. The programme's design review committee, meeting fortnightly through development, made decisions recorded as 'noted' or 'agreed': a service yard 'upgraded to adoptable standard following member feedback', community rooms 'enlarged per consultation response', a facade specification 'enhanced in line with the design panel's recommendation', photovoltaic coverage 'extended to all viable roof area'. None went through change control; each was minuted as governance, absorbed by the design team as instruction, and priced by nobody. The aggregate, your quantity surveyor estimates, is 8–11% of budget. The design is now the tender basis; the funding envelope is the original one; and the committee's chair — a client executive — genuinely believes the committee 'never changed the scope, only refined the design'.",
             "evidence":[
               {"label":"Mechanism","value":"40+ committee decisions over 18 months — 'noted', 'agreed', absorbed, never priced"},
               {"label":"Aggregate","value":"QS estimate: 8–11% of budget now baked into the tender-basis design"},
               {"label":"Envelope","value":"Funding unchanged from original scope"},
               {"label":"Belief","value":"Chair: 'never changed scope, only refined the design'"}],
             "decisions":[
               {"key":"surface","prompt":"Stage 1 — before tender, you:",
                "options":[
                  {"key":"reconcile","label":"Convert the minutes into a priced reconciliation and hand the client a real decision: every committee-absorbed change itemised with its minute reference and cost, presented not as an accusation but as the affordability question it is — fund the enhanced design (the honest number), de-scope back toward the envelope (itemised candidates, with the consultation-commitments flagged as politically expensive to reverse), or re-phase — decided BEFORE the tender bakes the unfunded 10% into every bid","quality":100,
                   "consequence":"The chair's 'refinement' theory dissolves against forty priced line items bearing his own minutes' references; the board funds half the delta, de-scopes a quarter (none of it consultation-committed), and value-engineers the rest — and the tender goes out on a design the envelope can actually buy.",
                   "principle":"Scope that grew through governance must be settled through governance — priced, itemised and decided before tender; the alternative is discovering the same 10% in the bid spread, when every option costs more."},
                  {"key":"tender_out","label":"Let the tender proceed and use the market's pricing as the forcing function — real bids make the affordability gap undeniable in a way internal estimates never do","quality":15,
                   "consequence":"The bids land 9% over envelope, as predicted for free by your QS; the 'forcing function' now costs a re-tender: de-scoping after bid means redesign, twelve weeks, and bidders who re-price the second round with a suspicion premium.",
                   "principle":"Using the market to prove what your own numbers already show buys the same fact at the most expensive counter — with the schedule as the currency."},
                  {"key":"absorb","label":"Take the delta into the programme's contingency and management reserve — the changes are politically embedded, relitigating them burns the client relationship, and 10% is absorbable across a programme this size","quality":0,
                   "consequence":"The reserve opens the delivery phase 80% committed to sunk design decisions; the first real construction risk arrives in month seven and finds the cupboard bare, and the emergency funding request has to explain both the risk AND the quiet 10% — now someone else's discovery.",
                   "principle":"Contingency spent concealing an affordability gap is unavailable for actual contingencies — and the concealment converts a governance failure into your personal one."}]},
               {"key":"mechanism","prompt":"Stage 2 — the committee continues into delivery. You:",
                "options":[
                  {"key":"pricing_gate","label":"Wire the committee to change control without demoting it: every committee recommendation with physical or cost consequence gets a 48-hour pricing note before ratification — 'agreed, subject to change control' becomes the minute's standard form — so members keep their design authority and acquire, for the first time, sight of what each refinement costs before it becomes irreversible","quality":100,
                   "consequence":"The committee's behaviour changes in one meeting: shown that a proposed canopy 'enhancement' costs a six-figure sum, members decline it themselves — the first time in the programme's history that governance has said no to its own good idea, because it finally knew the price.",
                   "principle":"Committees absorb scope because nothing shows them the meter — attach pricing to ratification and design governance becomes affordability governance without losing its authority."},
                  {"key":"strip","label":"Strip the committee's ability to direct design — advisory only, all instructions via the change process","quality":25,
                   "consequence":"The committee, demoted, becomes a grievance forum; its members — client executives and community representatives — route their influence through the sponsor directly, and the instructions resume by a channel with even less paperwork than the minutes had.",
                   "principle":"Influence doesn't disappear when you close its channel — it reroutes; the fix is pricing the channel, not damming it."}]}],
             "hints":["Read eighteen months of minutes with a QS beside you — 'noted' and 'agreed' are the change orders.",
               "Present affordability options, not blame: fund, de-scope, or re-phase — decided before the market prices it for you.",
               "Keep the committee's authority and attach a meter: pricing before ratification changes behaviour faster than any rule."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced forty minuted 'refinements' into the 10% they always were — and put a meter on the committee that wrote them."}
            """),

        ("WC-RSK-346", "The go-live register, read backwards", "Two hundred risks retired the month before cutover. Three of them retired themselves.",
            "Enterprise Programmes", "Programme Risk Analyst", "project_controls", "advanced", 15,
            """["risk_management","commissioning","evidence_analysis"]""",
            """
            {"context":"The enterprise transformation enters its commissioning month, and the go-live readiness pack proudly reports the risk register down from 214 open items to 31 — 'a demonstration of maturity'. Reading the closure log backwards, you find the shape of the reduction: 60% of the closures happened in a single fortnight, coinciding with the readiness review's announcement; the closure rationale field says 'no longer applicable' or 'superseded by go-live planning' on 96 of them; and spot-checking twelve closures against their owning workstreams, you find three whose closure nobody in the workstream can explain — including the payroll parallel-run variance risk, closed 'superseded', while the parallel run it referred to is still producing unexplained variances in this week's cycle. The programme director sees a maturing register; the pattern you see is a register being groomed to pass a review. Cutover is in nineteen days and the readiness board meets Thursday.",
             "evidence":[
               {"label":"Reduction","value":"214 → 31 open; 60% of closures in one fortnight before the readiness review"},
               {"label":"Rationale","value":"96 closures: 'no longer applicable' / 'superseded by go-live planning'"},
               {"label":"Spot-check","value":"3 of 12 closures unexplainable by their own workstreams — incl. live payroll variance risk"},
               {"label":"Clock","value":"Cutover in 19 days; readiness board Thursday"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your finding for Thursday:",
                "options":[
                  {"key":"audit","label":"Report the closure pattern as the risk: a targeted re-validation of the fortnight's closures before cutover — each 'superseded' item confirmed against the live evidence by its workstream, reopened where the evidence disagrees (the payroll variance risk first, since its 'superseding' parallel run is still misbehaving) — presented not as an accusation but as what readiness reviews are for: the register's honesty is a go-live criterion, because a groomed register means the board is deciding cutover on curated ignorance","quality":100,
                   "consequence":"The re-validation reopens eleven risks, three material — the payroll variance traces to a rounding rule the migration mapped wrongly, found and fixed in day twelve of the nineteen; the readiness board, deciding on a register that means something again, approves cutover with two conditions instead of discovering the rounding rule in the first live payroll.",
                   "principle":"A register that improves dramatically on review-announcement schedule is reporting the review, not the risk — closure quality is auditable, and the fortnight's pattern is the audit's address."},
                  {"key":"celebrate","label":"Accept the reduction — registers should shrink approaching go-live as planning risks retire naturally, and re-opening closed items nineteen days out signals panic to a board that needs confidence","quality":0,
                   "consequence":"Cutover proceeds on 31 visible risks; the payroll rounding defect ships inside the first live run, mispaying four thousand staff by small amounts in both directions — and the incident review finds the risk that predicted it, closed 'superseded', fourteen days before it happened.",
                   "principle":"Registers do shrink toward go-live — by evidence, not by fortnight; confidence built on curated closures is the exact ignorance readiness boards exist to refuse."},
                  {"key":"process","label":"Flag the closure-rationale quality as a process finding for the lessons log, and re-examine only the three unexplainable closures — proportionate scrutiny at a sensitive moment","quality":25,
                   "consequence":"The three re-examinations catch the payroll risk — luck, since the spot-check happened to include it — while the other 93 fortnight-closures ship unvalidated; two more surface as month-one incidents whose register history reads 'no longer applicable', and the lessons log duly records the lesson nobody applied.",
                   "principle":"When a pattern is systematic, sampling it is a lottery — the fortnight is the population, and nineteen days is enough to re-validate what one fortnight closed."}]},
               {"key":"mechanism","prompt":"On the grooming incentive itself, you:",
                "options":[
                  {"key":"closure_gate","label":"Change what the readiness review rewards: closure requires evidence cited in the record (what fact retired this risk), 'superseded' requires naming the superseding control and its owner, and the review's register metric becomes closure QUALITY — sampled and scored — rather than open-item count, so the number the workstreams optimise is the honest one","quality":100,
                   "consequence":"The next gateway's register shrinks slower and means more; two workstreams' closure-quality scores expose exactly the pair who had groomed hardest — handled as coaching, since the metric that misled them has been retired along with their closures.",
                   "principle":"Teams optimise the number the review counts — count open items and closures get groomed; count closure evidence and the register becomes worth reading again."},
                  {"key":"discipline","label":"Trace the fortnight's closures to their approvers and make the grooming a conduct matter — registers are assurance records and curating them is falsification","quality":20,
                   "consequence":"The investigation finds no instruction, no conspiracy — just forty people responding to a metric — and the conduct framing teaches the programme to groom more carefully next time; the payroll defect's nineteen-day window closes mid-inquiry.",
                   "principle":"When forty people commit the same sin on the same schedule, the sinner is the metric — punish it, not them, and fix the register before the inquiry."}]}],
             "hints":["Read the closure log by date — a register that matures on review-announcement schedule is answering the review.",
               "'Superseded' is a claim with an address: name the superseding control and check it is alive.",
               "Re-validate the fortnight's closures against live evidence — the payroll parallel run is still talking."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read a go-live register backwards, reopened eleven groomed closures — and caught the payroll defect twelve days before it paid anyone."}
            """),

        // ───────────── December · capstone tail — the year's disciplines, integrated ─────────────

        ("WC-RSC-357", "One deck, three dictionaries", "The bridge means one thing to the engineer, another to the funder, a third to the town.",
            "River Crossings", "Recovery Project Manager", "project_management", "advanced", 14,
            """["scope_management","concept_planning","stakeholder_communication"]""",
            """
            {"context":"The river crossing's funding agreement, concept design report and community consultation boards all describe the same deliverable — 'a fully accessible crossing serving pedestrians, cyclists and mobility users'. At concept gateway, you discover the phrase supports three engineering realities. The funder's accessibility standard requires gradients no steeper than 1:20 — which, at the navigation clearance the port authority demands, means approach ramps three hundred metres long on each bank, consuming a car park and a corner of the memorial garden. The concept design assumed 1:12 with rest platforms — compliant with a different code the designer considers current. The consultation boards showed an elegant short-ramped bridge that satisfies neither. Each document has an owner who believes theirs is THE definition; detailed design procurement starts in six weeks; and the memorial garden's friends' association has already seen the long-ramp sketch via a planning portal upload nobody meant to publish.",
             "evidence":[
               {"label":"Funder","value":"1:20 gradients → 300m approach ramps, car park + memorial garden corner"},
               {"label":"Designer","value":"1:12 with rest platforms — different code, shorter ramps"},
               {"label":"Public","value":"Consultation boards show a short-ramp bridge satisfying neither"},
               {"label":"Clock","value":"Detailed design procurement in 6 weeks; long-ramp sketch already leaked"}],
             "decisions":[
               {"key":"definition","prompt":"You:",
                "options":[
                  {"key":"adjudicate","label":"Force the definition question to its owner before procurement: a decision paper to the funder establishing which standard governs (theirs — it is a funding condition, however the designer reads the codes), the physical consequences drawn honestly at 1:20 including every option that shortens the ramps (raised approach, lift-assisted route, realigned deck), and the community shown the truthful geometry before the leaked sketch hardens into 'the plan they hid'","quality":100,
                   "consequence":"The funder confirms 1:20 and — shown the memorial garden consequence — funds the realigned-deck option that spares it; the friends' association, engaged with truth two weeks after the leak instead of two years, becomes the realignment's public advocate. Procurement starts on one definition, three weeks late and never revisited.",
                   "principle":"When one phrase supports three designs, the question isn't which reading is best — it is which document has the authority to answer, and getting that owner to answer while geometry is still cheap."},
                  {"key":"designer","label":"Back the designer's 1:12 reading — it is code-defensible, the ramps fit the site, and the funder's standard can be argued at gateway as gold-plating","quality":10,
                   "consequence":"The funder's compliance review, nine months into detailed design, applies its own condition as written; the redesign to 1:20 costs the fee twice and the programme a year, and the argument that was 'defensible' turns out to be with the party holding the money.",
                   "principle":"A code argument against a funding condition isn't an engineering position, it is a bet that the payer won't read their own agreement — they always do, eventually."},
                  {"key":"procure_flex","label":"Procure detailed design with the gradient question open — instruct the designer to develop both geometries until the funder rules","quality":25,
                   "consequence":"Both geometries at detailed-design cost is 60% more fee for the abandoned one; worse, 'both options live' leaks to the friends' association as indecision, and the campaign that forms opposes ramps in general rather than choosing between them.",
                   "principle":"Carrying incompatible geometries into detailed design doesn't preserve options — it funds one redundant design and one public controversy."}]}],
             "hints":["Rank the documents by authority, not by engineering preference — a funding condition outranks a code debate.",
               "Draw the true 1:20 geometry before defending anything — the consequences are the decision paper.",
               "The leak decides your community timeline: truth within weeks, or 'the hidden plan' forever."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Made one phrase mean one bridge — by finding which of three documents had the right to decide."}
            """),

        ("WC-RSC-358", "The float ledger, read at mobilisation", "Four projects' schedules share a pool of slack that exists once and is claimed thrice.",
            "Portfolio Management", "Recovery Planning Lead", "project_management", "advanced", 15,
            """["schedule_planning","evidence_analysis","governance"]""",
            """
            {"context":"Mobilising the portfolio's delivery year, you audit the four flagship schedules as a set — something no one has done, since each schedule has its own planner, board and reporting line. Read together, they tell one story three of them don't know: all four critical paths route through the same regional utility's connections team in the same two quarters; three schedules show comfortable float that exists only because each assumes IT cutover resources 'available on request' from a shared pool sized for one project at a time; and the portfolio's flagship — the one with executive attention — has quietly booked the shared commissioning rig for both its priority weeks AND its contingency weeks, sterilising the other projects' fallback without anyone deciding that. Each schedule, alone, is competent. The set is fiction. The portfolio board reviews 'schedule confidence' next week, using four green ratings produced by four planners who have never sat in the same room.",
             "evidence":[
               {"label":"Utility","value":"4 critical paths → same connections team, same 2 quarters"},
               {"label":"IT pool","value":"3 schedules' float assumes exclusive access to a shared pool sized for 1"},
               {"label":"Rig","value":"Flagship booked priority AND contingency weeks — others' fallback sterilised"},
               {"label":"Governance","value":"4 green ratings; planners have never met as a group"}],
             "decisions":[
               {"key":"diagnose","prompt":"Your finding to the portfolio board:",
                "options":[
                  {"key":"cross_load","label":"Present the set-level truth with the cross-loads made visible: one integrated view of the shared constraints — utility windows, IT pool, rig calendar — with each project's float re-stated net of the others' claims, and the honest confidence picture: one green, two ambers, one red — plus the mechanism that keeps it true: a portfolio-level constraint ledger, owned by one planner, that every project schedule must reconcile against at each update","quality":100,
                   "consequence":"The board's first sight of net float reorders the year: the utility windows get negotiated as a portfolio (the connections team, offered one coordinated programme, finds capacity it never offers four competing claimants), the rig's contingency weeks release, and the red project re-baselines eight months before it would have failed in public.",
                   "principle":"Schedules that share resources aren't independent documents — their float is a common pool, and only a set-level ledger can say who actually holds it."},
                  {"key":"flagship","label":"Fix the worst instance quietly first — release the flagship's contingency rig weeks back to the pool, and let the utility issue surface through normal escalation when it becomes concrete","quality":20,
                   "consequence":"The rig fix helps; the utility collision 'becomes concrete' in Q3 as four simultaneous escalations to a connections team that now holds all the leverage, and the board learns of the structural issue from the utility's account manager rather than its own planners.",
                   "principle":"Fixing instances of a structural problem one at a time means the structure introduces you to the next instance at its own convenience."},
                  {"key":"challenge","label":"Commission an independent schedule assurance review of all four projects — the findings will carry more weight arriving from outside","quality":30,
                   "consequence":"The review takes eleven weeks to produce the analysis you already hold, at consultancy prices; the utility windows — the perishable finding — age past their negotiability while the reviewers interview the planners you could have convened on Tuesday.",
                   "principle":"Outsourcing a finding you already have buys weight at the cost of the timing that made it valuable."}]},
               {"key":"mechanism","prompt":"The board asks how this never surfaced. You:",
                "options":[
                  {"key":"ledger","label":"Answer structurally and install the fix: no forum ever saw the schedules as a set — so the constraint ledger becomes standing governance (shared resources, cross-project claims, net float, reviewed monthly by the four planners together), and 'schedule confidence' at board level is re-defined to mean confidence net of the ledger — green is no longer a self-assessment","quality":100,
                   "consequence":"The monthly planners' session becomes the portfolio's cheapest control: month three catches a fifth collision forming around a cybersecurity sign-off team nobody had listed as a shared resource — added to the ledger while the fix is an email.",
                   "principle":"'How did nobody see it' has a structural answer — nobody was looking at the set; the remedy is a standing set-level view, not sharper individual planners."},
                  {"key":"process","label":"Tighten each project's schedule assurance checklist to include shared-resource assumptions","quality":25,
                   "consequence":"Four better checklists, still filled in separately: each project duly lists its assumptions about shared resources, and nobody reconciles the lists — the same fiction, now documented in quadruplicate.",
                   "principle":"Per-project rigour cannot see a portfolio-level fact — reconciliation is a place, not a checkbox."}]}],
             "hints":["Read the four schedules as one document — shared constraints only exist at the set level.",
               "Re-state each project's float net of the others' claims on the same pools; green may not survive.",
               "Negotiate the utility as one portfolio programme while the windows are still moveable."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read four green schedules as one fiction — and gave the portfolio the float ledger that made confidence mean something."}
            """),

        ("WC-RSC-359", "The specialist who became the bottleneck", "Every tower crane lift, every facade panel, every sign-off — one man's diary.",
            "Urban Development", "Recovery Programme Director", "project_management", "advanced", 22,
            """["resources_leadership","procurement_mobilization","evidence_analysis"]""",
            """
            {"context":"The mixed-use development's mobilisation review lands on a single name appearing everywhere. The facade package — the project's cost and programme heart — depends on a facade access consultant whose sign-off gates every panel type approval, every crane lift plan over public streets, and every temporary works interface with the heritage structure next door. He is genuinely exceptional; that is how his firm's scope grew from an advisory appointment into a de facto approval authority across three contracts, none of which name him personally. Your mobilisation schedule analysis shows his diary is the critical path: at his current committed availability — two days a week, shared with two other developments — the facade programme extends fourteen weeks beyond plan. His firm proposes 'prioritisation within existing arrangements'. The three contracts that route through him were let by three different package managers who each believed they had him exclusively; nobody has told the client; and the facade contractor has already priced a claim narrative around 'approval delays' that its commercial team updates weekly.",
             "evidence":[
               {"label":"Concentration","value":"One consultant gates panel approvals, lift plans and heritage interfaces — across 3 contracts"},
               {"label":"Arithmetic","value":"At 2 days/week shared, facade extends 14 weeks past plan"},
               {"label":"Blindness","value":"3 package managers each believed they had him exclusively; client untold"},
               {"label":"Exposure","value":"Facade contractor pre-building an 'approval delays' claim, updated weekly"}],
             "decisions":[
               {"key":"structure","prompt":"Stage 1 — you attack the bottleneck by:",
                "options":[
                  {"key":"decompose","label":"Decomposing the role before renegotiating the diary: audit which of his gates actually need HIM (the heritage interface judgement — probably) versus which need his firm's method applied by someone competent (panel approvals against a completed type-matrix — almost certainly), then restructure: he authors the approval frameworks and standard details in a concentrated four-week engagement, a named deputy applies them day-to-day, he retains personal sign-off only where judgement is irreplaceable — and the three contracts are amended to name the structure, not the man","quality":100,
                   "consequence":"The decomposition shows 70% of his gating was framework-applicable; the four-week authoring sprint plus deputy clears the panel-approval queue in six weeks, the fourteen-week extension collapses to three, and the claim narrative dies for lack of delays to point at. The heritage judgement stays his — which is where his two days a week always belonged.",
                   "principle":"A specialist bottleneck is usually a role that conflated judgement with application — decompose it, put the scarce person on the irreplaceable fraction, and codify the rest into frameworks others can run."},
                  {"key":"buy","label":"Buying more of him — renegotiate his firm's engagement to four days a week with a premium and exclusivity, paid for from the fourteen weeks the schedule saves","quality":25,
                   "consequence":"His firm accepts the premium, his other two developments escalate to THEIR clients, and the exclusivity lasts five weeks before a compromise re-shares him at three days; the structure that made one diary the critical path survives intact, now at premium rates.",
                   "principle":"Buying more of a bottleneck deepens the dependency it doesn't fix — the diary is the symptom; the role design is the disease."},
                  {"key":"replace","label":"Breaking the dependency outright — procure a second facade access consultancy to take over two of the three gate streams","quality":15,
                   "consequence":"The market's honest answer takes nine weeks to arrive: the two credible alternatives are conflicted out or unavailable, and the incumbent — now aware he is being replaced — becomes precisely as cooperative as his contract requires. The fourteen weeks grow while the procurement proves what a capability audit would have said in days.",
                   "principle":"You cannot competitively procure your way out of a scarce-judgement dependency on a live critical path — restructure what you have before shopping for what may not exist."}]},
               {"key":"govern","prompt":"Stage 2 — on the system that let three contracts each assume exclusivity, you:",
                "options":[
                  {"key":"register","label":"Institute a keystone-dependency register at programme level: every external party whose capacity is assumed by more than one contract, with their real committed availability verified at award — reviewed at each package letting, because the facade consultant is the instance and 'three package managers, three assumptions, zero reconciliation' is the class — and brief the client now, with the fix attached, rather than after the claim would have","quality":100,
                   "consequence":"The register's first pass finds the same pattern forming around the tower crane coordinator and the party-wall surveyor — both re-based while amendment costs a letter; the client, briefed with solutions in hand, adds the register to its own assurance standard for the next scheme.",
                   "principle":"Shared-dependency blindness is a procurement design flaw, not a personality problem — verify capacity at award, register it across packages, and the next keystone person never becomes a critical path."},
                  {"key":"process_note","label":"Add an exclusivity check to the package-letting checklist and move on — the register is bureaucracy for a one-off","quality":20,
                   "consequence":"The checklist catches assumptions at future lettings; the seven packages already let keep their unreconciled dependencies, and the crane coordinator collision arrives in month five exactly as the register would have predicted.",
                   "principle":"A forward-only control on a live portfolio manages the projects you haven't started and abandons the ones you have."}]}],
             "hints":["Split the role: which gates need his judgement, which need his firm's method applied by anyone competent?",
               "The contracts name a firm; the bottleneck is a person — amend toward frameworks and named deputies.",
               "The class fix is a keystone register: shared external dependencies, verified capacity, reconciled across packages."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Decomposed a one-man critical path into frameworks, a deputy and four judgement days — and the claim narrative lost its subject."}
            """),

        ("WC-RSC-360", "The price that answered a different question", "The winning bid is real, deliverable, and for a job the JV no longer runs.",
            "Joint Ventures", "Recovery Commercial Director", "project_finance", "advanced", 26,
            """["procurement","concept_planning","evidence_analysis"]""",
            """
            {"context":"You lead commercial recovery of the joint venture's biggest procurement: the delivery partner framework that will build out its five-year pipeline. The evaluation is complete, the recommendation drafted — and your pre-award review finds the problem is not in the bids but in the question. The tender was shaped eighteen months ago around the JV's then-strategy: large self-contained packages, contractor-led design, lump-sum risk transfer. Since then, the JV's board — responding to two parent-company reorganisations and a funding re-profile — has shifted the pipeline toward smaller phased releases with client-led design and open-book targets. The winning bidder's strengths are precisely the old question's: balance-sheet depth for lump-sum risk, an integrated design house, self-delivery scale. The runner-up — 4% higher on the old question's scoring — is structurally built for the new reality: collaborative contracting record, phased mobilisation, open-book systems. The evaluation cannot lawfully be re-scored against criteria the tender never published. Award expires in five weeks; re-tendering costs nine months against a pipeline whose first release is funded and politically dated.",
             "evidence":[
               {"label":"Drift","value":"Tender shaped for lump-sum/contractor-design; strategy now phased/client-led/open-book"},
               {"label":"Bids","value":"Winner optimised for the old question; runner-up (+4%) built for the new one"},
               {"label":"Law","value":"Cannot re-score against unpublished criteria"},
               {"label":"Clock","value":"Award expires in 5 weeks; re-tender = 9 months against a funded, dated first release"}],
             "decisions":[
               {"key":"award","prompt":"Stage 1 — your recommendation to the JV board:",
                "options":[
                  {"key":"scope_award","label":"Use the lawful degrees of freedom the tender actually contains: award to the evaluated winner — the process permits nothing else — but award the framework at its minimum committed scope (the funded first release, shaped as the tender described it), NOT the five-year pipeline; run the strategy question honestly in parallel as a new procurement designed around the new operating model for subsequent releases, with the framework's extension mechanisms exercised only if the winner demonstrates adaptation on release one — and tell both bidders exactly this, in the debrief, in writing","quality":100,
                   "consequence":"Release one delivers under the winner on the terms it genuinely bid; the new-model procurement, run without time pressure, is won eighteen months later by the former runner-up on criteria that finally match the strategy — and no lawyer ever gets a letter, because every party was told the truth about which question their bid had answered.",
                   "principle":"When strategy moves after tender, the lawful path is to shrink the award to what the old question still validly covers — and put the new question to the market as itself, not as a re-interpretation of scores."},
                  {"key":"full_award","label":"Award the full framework as evaluated — the process is clean, the winner is capable, and frameworks are relationships: the operating model can be evolved inside it through instructions and variations","quality":10,
                   "consequence":"The winner, contracted for lump-sum scale, prices every 'evolution' toward open-book phasing as the variation it contractually is; by year two the JV is paying change-order rates to unwind its own procurement, with a partner whose business model the pipeline no longer feeds — the relationship sours on structural, not personal, grounds.",
                   "principle":"A framework is a five-year bet on the question you asked — awarding a superseded question in full buys years of paying the difference between what you procured and what you meant."},
                  {"key":"collapse","label":"Let the award expire and re-tender against the new strategy — nine months is the honest price of asking the right question, and the first release can be bridged with an interim direct appointment","quality":25,
                   "consequence":"The bridge appointment — direct, negotiated, time-pressured — costs 12% over market and draws the losing bidders' scrutiny far more than any award would have; the re-tender is clean, the first release is late and expensive, and the board notes that a minimum-scope award would have bought the same clean re-tender without either.",
                   "principle":"Re-asking the question is right; paying a distressed premium to bridge the gap you created is not — expiry is the honest path only when no lawful narrower award exists."}]},
               {"key":"class","prompt":"Stage 2 — so procurement stops answering dead questions, you:",
                "options":[
                  {"key":"checkpoint","label":"Install a strategy-currency checkpoint in the procurement gateway: any tender older than six months re-validates its shaping assumptions against current strategy before award recommendation — a one-page attestation by the SRO, not a re-evaluation — so drift is caught at the cheap moment, between evaluation and award, where scope and structure can still lawfully flex","quality":100,
                   "consequence":"The checkpoint's second use catches a facilities tender whose energy-services assumptions predate the parents' net-zero commitment — re-shaped before award at the cost of a three-week pause, against the alternative this framework just demonstrated.",
                   "principle":"Tenders age at the speed of strategy — a dated attestation between evaluation and award is the cheapest control ever placed on the most expensive class of error."},
                  {"key":"faster","label":"Mandate shorter procurements — six months maximum from issue to award, so strategy has less time to move underneath them","quality":25,
                   "consequence":"The mandate compresses genuine evaluation on the complex packages that most need it; strategy, indifferent to the mandate, moves anyway — the next drift happens under a faster process with less thinking time and the same blind spot.",
                   "principle":"You cannot outrun strategy drift with process speed — you can only check for it; the control is a question, not a stopwatch."}]}],
             "hints":["Locate the failure precisely: the bids are honest; the question predates the strategy.",
               "Find the lawful flex: minimum committed scope now, the new question procured as itself later.",
               "Tell both bidders the truth in writing, and add a strategy-currency check between evaluation and award — clarity is the challenge-proofing."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Awarded a superseded tender at its minimum lawful scope — and put the real question back to the market as itself."}
            """),

        ("WC-RSC-361", "The recovery plan with two owners", "The turnaround has a client plan and a contractor plan. The plant has neither.",
            "Energy & Process", "Recovery Programme Director", "project_management", "advanced", 28,
            """["change_claims_recovery","procurement_mobilization","governance"]""",
            """
            {"context":"The refinery turnaround overran its window by eleven days last spring — a compound failure of late scope growth, materials logistics and permit congestion — and the recriminations have now produced two competing recovery plans for next year's event. The owner's plan, written by its projects group, rebuilds the turnaround around client-controlled scope freeze dates, client-held float and step-in rights. The incumbent contractor's plan — they hold a three-event term contract with two events remaining — rebuilds it around contractor-led integrated planning, early scope involvement and shared incentives. Each plan is competent; each allocates the OTHER party's behaviour as the primary risk; each is being briefed separately upward — the owner's to its executive, the contractor's to the term-contract review board. Meanwhile the actual next turnaround's mobilisation clock is running: long-lead materials need ordering in six weeks against a scope neither plan agrees on, and the two planning teams have stopped sharing working files. The term contract's early-termination clause is priced so punitively that separation is theoretical. You are appointed jointly — by both parties — to end the war.",
             "evidence":[
               {"label":"Event","value":"11-day overrun last spring: scope growth + logistics + permit congestion"},
               {"label":"Two plans","value":"Owner: control, freeze dates, step-in. Contractor: integration, early involvement, incentives"},
               {"label":"Reality","value":"Long-lead orders due in 6 weeks; planning teams no longer share files"},
               {"label":"Structure","value":"Term contract, 2 events left, separation punitively priced — the marriage continues"}],
             "decisions":[
               {"key":"unify","prompt":"Stage 1 — your first move as joint appointee:",
                "options":[
                  {"key":"failure_first","label":"Refuse to adjudicate between the plans and force both parties back to the evidence: a joint root-cause review of the eleven days — day by day, cause by cause, with both teams' records on one wall — because the two plans encode two blame theories, and the actual failure anatomy (which the records will show is shared: client-late scope AND contractor-thin logistics AND a permit process both sides knew was undersized) is the only foundation a joint plan can stand on; the long-lead scope decision runs in parallel on a provisional freeze both sides sign precisely because it is provisional","quality":100,
                   "consequence":"The wall exercise takes nine days and ends the war more effectively than any ruling: the eleven days decompose into four client-owned, three contractor-owned and four jointly-owned causes, in front of both executives — and the joint plan that follows assigns controls to whoever owns the cause, which neither unilateral plan could do. The long-lead orders place on week five against the provisional freeze.",
                   "principle":"Competing recovery plans are competing blame theories — adjudicating between them crowns one theory; joint root-cause on the shared record dissolves both into the only thing that governs well: causes with owners."},
                  {"key":"merge","label":"Merge the plans diplomatically — take the owner's freeze dates and step-in rights, the contractor's integrated planning and incentives, and issue the combined document as the joint plan both boards can approve","quality":20,
                   "consequence":"The merged document approves smoothly and governs nothing: freeze dates the contractor never believed in are missed with contractual politeness, incentives the owner never believed in pay out on technicalities, and next year's event runs on the same two shadow plans with a better cover sheet.",
                   "principle":"Merging positions without merging the underlying theory of failure produces a treaty, not a plan — both sides comply with the words and keep their own map."},
                  {"key":"rule","label":"Adjudicate — you were appointed to decide; pick the owner's plan as the asset-holder's prerogative, with the contractor's incentive mechanism grafted on as consolation","quality":10,
                   "consequence":"The contractor's team, ruled against by the joint appointee, retreats into contractual performance — every early-involvement behaviour the recovery actually needed becomes a priced extra, and the term contract's two remaining events run correct, cold and slow.",
                   "principle":"In a marriage without exit, a ruling produces a loser who still runs half the plant — authority that cannot separate the parties must reconcile them instead."}]},
               {"key":"structure2","prompt":"Stage 2 — the joint plan needs a keeper. You:",
                "options":[
                  {"key":"joint_cell","label":"Stand up a single integrated turnaround-planning cell — co-located, both parties' planners, one plan of record, one file structure, led by a jointly-appointed event director with a charter both executives sign; the cell owns the freeze dates, the logistics model and the permit-capacity fix (the cause everyone forgot because neither plan owned it), and reports ONE status to both boards through one document","quality":100,
                   "consequence":"The cell's first output is the thing neither side had: a permit-capacity model showing the authority's processing rate needs doubling for the planned scope — fixed by pre-submission agreements while it is a meeting, not a crisis. Next year's event lands two days early; both boards quote the same numbers for the first time in the contract's history.",
                   "principle":"A joint plan kept by two organisations decays back into two plans — the keeper must be a single integrated cell with one file, one status and a charter both sides signed, or the war resumes by version control."},
                  {"key":"liaison","label":"Keep the two planning teams and bridge them properly — weekly integration meetings, a shared milestone tracker, and a liaison manager from each side","quality":25,
                   "consequence":"The bridge works while the weather is fair; at the first hard scope dispute the teams retreat behind their liaisons, the shared tracker forks into 'draft' and 'agreed' versions, and the file-sharing freeze of last spring re-runs with better minutes.",
                   "principle":"Liaison between rival structures preserves the rivalry — integration is a room and a file structure, not a meeting."}]}],
             "hints":["Two plans means two blame theories — take both parties to the shared record before taking either plan seriously.",
               "Decompose the eleven days into causes with owners, and order long-leads against a provisional freeze both sides sign BECAUSE it is provisional.",
               "One cell, one file, one status — jointly chartered; anything less forks back into war."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ended a two-plan war with a nine-day wall of shared evidence — and one planning cell that made next year's event boring."}
            """),

        ("WC-RSC-362", "Order the ward's words", "Ten fixes for one ambiguous scope statement — sequence them before the concrete ships.",
            "Life Sciences", "Recovery Programme Manager", "project_management", "advanced", 24,
            """["scope_requirements","concept_planning","decision_quality"]""",
            """
            {"context":"The vaccine plant upgrade's business case contains the sentence that will govern a nine-figure project: 'The facility shall be upgraded to enable flexible multi-product manufacturing to current regulatory expectations.' At concept gateway, your scope review finds four live interpretations (which products define 'flexible'; whether 'current' means at approval or at completion, four years apart; which regulator's expectations — the domestic agency's or the export markets'; and whether 'enable' means install or merely not-preclude). Ten corrective actions are on your list, all sensible: (A) commission a regulatory-basis paper fixing which agencies and which vintage; (B) run a product-portfolio workshop with the commercial team to define the flexibility envelope; (C) issue a holding definition so concept design can proceed; (D) price each interpretation as a design option; (E) rewrite the scope statement and take it back through the case's approval chain; (F) freeze concept design until resolution; (G) interview the sentence's author; (H) benchmark competitor facilities' flexibility standards; (I) get the export-market question answered by the sales pipeline owners; (J) put the ambiguity on the risk register. You cannot do ten things at once; the concrete order for the first structural package ships in nineteen weeks. Sequence the attack.",
             "evidence":[
               {"label":"Sentence","value":"'Flexible multi-product manufacturing to current regulatory expectations' — 4 live readings"},
               {"label":"Stakes","value":"Nine figures; readings diverge by ~30% of capital cost"},
               {"label":"Clock","value":"First structural concrete ships in 19 weeks"},
               {"label":"List","value":"10 sensible actions, capacity for a sequence, not a broadside"}],
             "decisions":[
               {"key":"sequence","prompt":"Stage 1 — your opening sequence:",
                "options":[
                  {"key":"decide_inputs","label":"G+I+B in parallel week one (the author's intent, the export answer and the product envelope are INPUTS to any definition — cheap, fast, and each owned by people who already know), then A (the regulatory-basis paper, scoped by what B and I found), then C (a holding definition derived from real inputs, letting concept design proceed on declared assumptions), with D pricing only the readings still live after A — and E, the formal rewrite through the approval chain, riding on evidence rather than opening the argument","quality":100,
                   "consequence":"The author interview kills one reading in an hour ('enable' meant install — it is in his drafting notes); the export answer kills another (two target markets, known agencies). The holding definition issues in week four on two live readings instead of four, concept design never stops, and the rewrite passes the approval chain in week fifteen because every choice arrives pre-evidenced. The concrete ships on schedule to a design that means one thing.",
                   "principle":"Ambiguity resolves by sequencing inputs before definitions and definitions before approvals — interrogate the cheap knowledge first, hold a declared assumption so work continues, and let the formal rewrite arrive as a conclusion, not a question."},
                  {"key":"freeze_first","label":"F then E — freeze concept design immediately (every drawn line against a four-way ambiguity is potential rework) and drive the formal rewrite through the approval chain as the single priority","quality":15,
                   "consequence":"The freeze protects against rework and guarantees delay: the approval chain, asked to adjudicate four readings with no evidence assembled, commissions its own analysis — which is actions G, I, B and A run by slower people — and the concrete date dies in week eleven while the chain deliberates.",
                   "principle":"Freezing work to await a decision no one has been equipped to make converts ambiguity into schedule loss with certainty — decisions need inputs before they need urgency."},
                  {"key":"register_price","label":"J then D — register the ambiguity honestly, then price all four interpretations as design options so the eventual decision is fully informed on cost","quality":25,
                   "consequence":"Four fully-priced options take twelve weeks of design effort — two of them on readings the author's drafting notes and the sales pipeline would have killed in week one for the price of two meetings; the decision arrives informed, late, and 40% of the pricing work was archaeology.",
                   "principle":"Pricing interpretations before interrogating them is rigour pointed backwards — kill the killable readings with cheap knowledge before funding the survivors' analysis."}]},
               {"key":"holding","prompt":"Stage 2 — the holding definition (C) needs a discipline. You:",
                "options":[
                  {"key":"declared","label":"Issue it as a governed artifact: the assumed readings stated explicitly, every design output stamped against it, a change-impact log tracking what reversal would cost as design matures — so when the formal rewrite lands, the delta between assumption and decision is a priced, known quantity, and the approval chain can see exactly what its four-week deliberation would buy or break","quality":100,
                   "consequence":"The rewrite confirms the holding definition on one reading and reverses it on one detail — the reversal costs eleven drawings, known in advance from the log, absorbed in a fortnight; nobody discovers in year three that the plant was built on an assumption nobody remembered making.",
                   "principle":"A holding assumption is safe exactly as long as it is loud — declared, stamped on every output, and carrying a running price of its own reversal."},
                  {"key":"informal","label":"Let the design team proceed on the most probable reading without formal apparatus — the definition is temporary and the paperwork would outlive its purpose","quality":10,
                   "consequence":"The 'temporary' reading hardens silently into the design basis; by the time the rewrite lands, reversal costs a redesign nobody priced, and the approval chain is told — accurately — that its decision was pre-empted by drawings.",
                   "principle":"Undeclared assumptions don't stay temporary — they compound into the design until the decision they replaced becomes unaffordable to make."}]}],
             "hints":["Sort the ten actions into inputs, definitions and approvals — the sequence is the answer.",
               "The cheapest killers of a false reading: the author's notes, the sales pipeline, the product owners — week one, in parallel.",
               "Work continues on a declared holding assumption with a running reversal price — never on silence, never frozen."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced ten fixes for one nine-figure sentence — inputs first, holding definition loud, rewrite arriving as a conclusion."}
            """),

        ("WC-RSC-363", "Two critical paths, one overhaul window", "Rank the seven protections before the plant stops earning.",
            "Manufacturing", "Turnaround Director", "project_management", "expert", 22,
            """["schedule_planning","procurement_mobilization","decision_quality"]""",
            """
            {"context":"The equipment overhaul — the press line and the finishing train, both in one six-week production shutdown — has two critical paths within eighteen hours of each other, and your mobilisation review must rank seven candidate protections with budget for perhaps four: (A) pre-shutdown trial-fit of the press crown's replacement bearings at the manufacturer's works; (B) a resident OEM technician for the finishing train's control migration, on site for the full window; (C) doubling the rigging crew for the press path's heaviest week; (D) airfreight provision for the finishing train's two longest-lead spares; (E) a full 3D laser scan of both machine halls now, to kill fit-up surprises; (F) night-shift working pre-approved with the union for weeks four to six; (G) a bonded on-site store holding both paths' critical spares a month early. The window cannot extend: the order book resumes on day 43 with contractual deliveries. Last overhaul, nine years ago, overran by eight days — the file blames 'unforeseen fit-up conditions and spares logistics'.",
             "evidence":[
               {"label":"Paths","value":"Press line and finishing train — critical paths 18 hours apart, one 6-week window"},
               {"label":"Choices","value":"7 protections, budget for ~4"},
               {"label":"History","value":"Last overhaul +8 days: 'fit-up conditions and spares logistics'"},
               {"label":"Hard stop","value":"Order book resumes day 43, contractual"}],
             "decisions":[
               {"key":"rank","prompt":"Stage 1 — your four, and the principle that picked them:",
                "options":[
                  {"key":"uncertainty_kill","label":"E, G, A, B — rank by what kills uncertainty before the clock starts versus what merely adds capacity after it: the laser scan (E) and early bonded spares (G) attack exactly the two failure modes the last overhaul's file names, before day one; the trial-fit (A) converts the press path's biggest unknown into a rehearsed operation; the resident technician (B) de-risks the finishing train's only novel scope (the control migration) for its whole duration — while C and F are capacity responses purchasable mid-window if needed, and D becomes cheap insurance to hold as an option rather than a provision","quality":100,
                   "consequence":"The scan finds three foundation-bolt conflicts and a clearance problem — redesigned before shutdown instead of discovered on day 9; the trial-fit surfaces a machining error at the manufacturer's works, fixed on their premises and their account; the migration wobbles twice with the technician present and neither wobble reaches the path. The window closes on day 41 — the first overhaul in the plant's history to return early.",
                   "principle":"With an immovable window, spend on certainty before the clock starts, not capacity after it slips — the protections that convert unknowns into rehearsals outrank every crew you could add once the surprise has already happened."},
                  {"key":"capacity","label":"C, F, B, D — protect throughput and response: doubled rigging and pre-approved nights give schedule recovery power, the technician covers the novel scope, airfreight covers logistics; surprises are inevitable, so buy the ability to absorb them","quality":20,
                   "consequence":"The absorbing capacity performs as bought — recovering from each surprise at premium rates — but the surprises themselves arrive unreduced: the fit-up conflicts the scan would have caught cost six days that doubled rigging can only partially claw back, and the window closes at day 45, two days into the order book at liquidated rates.",
                   "principle":"Buying recovery capacity while leaving discovery on the table plans to have the emergency — absorption is what you want left over after prevention, not instead of it."},
                  {"key":"split_even","label":"A, C for the press path; B, D for the finishing train — two protections each, symmetric, defensible to both engineering teams","quality":15,
                   "consequence":"The symmetry satisfies the teams and ignores the evidence: neither path got the scan or the early spares — the two named killers from the last overhaul — and both paths meet their fit-up surprises democratically, in week two, eighteen hours apart.",
                   "principle":"Allocating protection by fairness between paths instead of by failure-mode evidence is politics wearing a planning hat — the last overhaul's file already ranked the risks; read it."}]},
               {"key":"holdback","prompt":"Stage 2 — the three unfunded protections, you:",
                "options":[
                  {"key":"options_ready","label":"Convert to pre-negotiated options with trigger criteria: the rigging double-crew (C) priced and crewed on 72-hour call, the night-shift agreement (F) signed now with activation by notice, airfreight (D) as standing quotes with booking triggers tied to the spares tracker — costing option fees instead of provisions, exercisable by criteria instead of panic","quality":100,
                   "consequence":"One option fires: a bearing journal's condition on strip-down triggers the night-shift notice for week five, activated in a day because the agreement existed. The other two expire unexercised, having cost 4% of their provision price to hold.",
                   "principle":"The protections you can't fund you can still arm — options with pre-agreed triggers deliver capacity at decision speed for a fraction of standing cost; the alternative is negotiating with a union in week five."},
                  {"key":"drop","label":"Release them cleanly — four funded protections properly resourced beat seven diluted ones, and the contingency budget stays whole for genuine surprises","quality":30,
                   "consequence":"Clean, until the bearing journal: the night-shift negotiation opens in week five from a standing start, costs nine days of union process for a three-night need, and the 'whole' contingency pays the overrun it existed to prevent.",
                   "principle":"Unarmed capacity takes weeks to summon that armed options deliver in hours — dropping the option fee saves pennies against the day the trigger fires."}]}],
             "hints":["Read the last overhaul's file first — it already names the killers: fit-up and spares logistics.",
               "Rank by uncertainty killed before day one, not capacity added after a slip.",
               "What you can't fund, arm: pre-negotiated options with trigger criteria beat standing provisions and panic buys."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Spent the overhaul budget on certainty before day one — and closed a six-week window two days early."}
            """),

        ("WC-RSC-364", "Overtime as a plan, again", "The recovery proposal is the same one that failed. Rank the alternatives it ignored.",
            "Public Sector Technology", "Recovery Programme Director", "project_management", "expert", 28,
            """["resources_leadership","procurement_mobilization","decision_quality"]""",
            """
            {"context":"The courts modernisation programme — case management rollout across 80 sites — is twelve weeks behind at the mobilisation gateway, and the delivery director's recovery proposal is one line long: sustained overtime across the deployment teams, 'as successfully applied in the pilot phase'. Your review of the pilot's own records tells a different story: overtime in the pilot ran eight weeks, output per hour fell 23% by week six, the two most experienced deployment leads resigned within a month of it ending, and the defect rate of pilot-phase configurations — now visible in live-service tickets — runs double the estimate. The twelve weeks are real, the political date (a ministerial commitment to the first fifty sites) is real, and the deployment workforce is the constraint: trained configuration specialists, four months to grow, currently 31 against a plan that assumed 38. Six alternatives sit unexamined in your review notes: (A) descope the configuration variant count per site — 60% of effort goes to local variations of marginal value; (B) resequence the eighty sites so the thirty simplest go first, banking political sites early; (C) split roles — separate the scarce configuration work from the trainable site-logistics work the specialists currently also do; (D) buy capacity from the software vendor's professional services at a premium; (E) accept a dated slip of the ministerial commitment now, traded for a credible plan; (F) the proposed overtime, bounded and targeted rather than sustained and general.",
             "evidence":[
               {"label":"Proposal","value":"Sustained overtime, 'as in the pilot'"},
               {"label":"Pilot record","value":"Output −23% by week 6; two lead resignations; double defect rate now in live tickets"},
               {"label":"Constraint","value":"31 configuration specialists vs 38 planned; 4 months to grow one"},
               {"label":"Politics","value":"Ministerial commitment: first 50 sites, dated"}],
             "decisions":[
               {"key":"rank","prompt":"Stage 1 — your recovery architecture, ranked:",
                "options":[
                  {"key":"constraint_design","label":"C then A then B as the core — attack the constraint's design before its utilisation: role-splitting (C) returns ~30% of specialist hours immediately by moving logistics work to trainable staff; variant descoping (A) cuts the work itself, and its 60% figure means politics can keep its date while the sites keep only variations that earn their cost; resequencing (B) banks the commitment's optics with the simplest sites while complexity concentrates where the recovered capacity can meet it — with D as a bounded top-up for the peak quarter only, and F reduced to surge overtime on named weeks with the pilot's fatigue data as its explicit boundary; E stays in reserve, honestly priced, in case the rebuilt plan still misses","quality":100,
                   "consequence":"Role-splitting frees the equivalent of nine specialists inside six weeks — more than the staffing gap — and the variant cull, fought for a fortnight by every site's 'unique requirements', survives on the pilot's own defect data: most variants were defects waiting to configure. The first fifty sites land eleven days inside the ministerial date without a single sustained-overtime week, and the two remaining leads stay.",
                   "principle":"When people are the constraint, redesign the work before working the people — role-splitting, scope surgery and sequencing recover capacity structurally; overtime recovers it briefly, at compound interest the pilot has already priced."},
                  {"key":"buy_out","label":"D as the backbone — vendor professional services close the specialist gap at premium but with certainty, overlaid with B's resequencing; internal structural change (A, C) waits until after the ministerial date, when there is room to breathe","quality":25,
                   "consequence":"The vendor's twelve consultants arrive certified and site-blind; their configurations pass acceptance and generate the same local-variant defect pattern the pilot did, at four times the internal day-rate — and the structural fixes 'after the date' meet a workforce that has watched its work outsourced and its problems deferred. The date is met; year two inherits the bill twice.",
                   "principle":"Bought capacity applied to unreformed work buys the same defects at consultancy rates — the premium is justified only after the work itself has been made worth doing."},
                  {"key":"honest_slip","label":"E leading — the pilot data makes any accelerated plan a gamble, and the professional move is trading the ministerial date now for a plan with genuine confidence, before more money burns","quality":20,
                   "consequence":"Honest, and prematurely defeatist: the minister's office, offered a slip before the structural options were even tried, reads the programme as unmanageable and imposes a delivery taskforce — which spends its first month discovering options A through C in your own review notes, then implements them under worse conditions with new management.",
                   "principle":"Recommending the political price before exhausting the structural options makes the slip look like the plan — reserve honesty about the date for after the redesign has been given its arithmetic."}]},
               {"key":"overtime2","prompt":"Stage 2 — the delivery director still wants the overtime line. You:",
                "options":[
                  {"key":"bounded","label":"Grant it as an instrument with the pilot's data as its governor: surge overtime only, named weeks tied to specific site clusters, capped at the duration the pilot showed output holding, defect rates tracked weekly against the pilot curve with automatic stand-down at the threshold — and the delivery director owns the tracking, so the person who proposed the tool operates its limits","quality":100,
                   "consequence":"Surge weeks fire four times across the fifty sites, each inside the fatigue boundary; the defect tracker stays under the pilot curve throughout, and the delivery director — running the governor on their own proposal — becomes its most careful operator and quotes the stand-down threshold in the lessons-learned session.",
                   "principle":"The answer to a failed tool proposed again is rarely prohibition — it is the tool bounded by the data from its own failure, operated by its advocate; limits enforced by their proposer outlast limits imposed on them."},
                  {"key":"refuse","label":"Refuse it entirely — the pilot's resignations and defect data make overtime a proven value-destroyer, and the structural plan needs no crutch","quality":30,
                   "consequence":"The refusal holds until week nine, when a site cluster's asbestos survey compresses three deployments into one fortnight — the exact surge case — and the overtime that then happens anyway happens ungoverned, unbounded and untracked, because the mechanism that would have bounded it was refused on principle.",
                   "principle":"Banning a tool doesn't remove the conditions that summon it — the surge case always comes, and it arrives governed or it arrives feral."}]}],
             "hints":["Read the pilot's own records before honouring its legend — output curve, resignations, live defect tickets.",
               "Redesign before utilisation: split the scarce role, cut the variant scope, resequence for the political date — and hold the honest slip in reserve, priced.",
               "Overtime survives only as a bounded surge instrument with its own failure data as governor — operated by its proposer."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Answered 'overtime, again' with role-splitting, scope surgery and sequencing — and kept the surge tool bounded by its own failure data."}
            """),

        ("WC-RSC-365", "Order the counter-offer", "The vendor's deadline lands Friday. Six moves, one sequence, national stakes.",
            "Technology Programmes", "Recovery Commercial Director", "project_finance", "expert", 24,
            """["procurement","negotiation","decision_quality"]""",
            """
            {"context":"The national rollout's platform vendor has tabled a variation with a deadline: a 34% uplift on the remaining licence and deployment tranches, justified as 'sustained input cost inflation and scope evolution', acceptance required by Friday or 'delivery scheduling cannot be guaranteed for Q1 sites'. The programme is mid-flight: 190 of 400 sites live, the vendor's platform is the system of record for all of them, and Q1's sites include the two flagship regions a select committee has been promised. Your analysis this week has produced six sound moves: (A) commission an independent benchmark of the licence pricing against comparable government frameworks — ten working days; (B) formally request the contractual basis of the uplift under the contract's open-book provisions — the variation cites clauses that, your reading says, cover a fraction of the 34%; (C) open a parallel conversation with the vendor's UK managing director, who inherited this account and its relationship damage last year; (D) map the genuine switching costs and timeline — eighteen months minimum, but the map's existence changes the negotiation; (E) brief the select committee's sponsoring department now, before Friday, on the position and strategy; (F) reject the Friday deadline explicitly and in writing as incompatible with public procurement obligations. Sequence the week.",
             "evidence":[
               {"label":"Demand","value":"+34% on remaining tranches, accept by Friday, Q1 scheduling threatened"},
               {"label":"Leverage facts","value":"190 sites live on their platform; switching ≥18 months; committee promise on Q1 regions"},
               {"label":"Cracks","value":"Uplift's cited clauses cover a fraction of 34%; new UK MD inherited the account"},
               {"label":"Moves","value":"A benchmark · B open-book request · C MD channel · D switching map · E brief department · F reject deadline"}],
             "decisions":[
               {"key":"sequence","prompt":"Stage 1 — the week's sequence:",
                "options":[
                  {"key":"structure_week","label":"F and B Monday, E Tuesday, A and D launched in parallel, C opened Thursday: reject the deadline first in writing (a deadline accepted is a deadline obeyed — and public-procurement duties make the rejection unanswerable), the open-book request the same day so the vendor spends the week justifying rather than pressuring, the department briefed before it can be surprised, the benchmark and switching map running as the evidence engines — and the MD conversation opened only after the formal machinery is visibly in motion, so it is a relationship channel above a working process, not a substitute for one","quality":100,
                   "consequence":"The deadline dies by Wednesday — Q1 scheduling is 'under review' but no site slips, because the open-book request has the vendor's commercial team answering for clauses that support 9%, not 34%. The benchmark lands in ten days showing mid-market pricing; the MD, meeting a structured counterparty instead of a panicking one, settles at 11% with a two-year price-certainty clause — and the committee's regions go live on schedule, never knowing there was a week like this.",
                   "principle":"Against a manufactured deadline, sequence is the strategy: kill the clock first, put the burden of proof on the demand, arm your principals before the pressure reaches them, run the evidence in parallel — and open the senior channel only once the machinery makes it a conversation between equals."},
                  {"key":"relationship_first","label":"C leading — the new MD is the real opportunity; open there Monday, hold the formal machinery in reserve so the conversation starts collaborative rather than adversarial, and deploy B and A only if the human channel fails","quality":20,
                   "consequence":"The MD is charming, sympathetic and structurally unable to concede: with no open-book request on the table, his commercial team owes no numbers, and Friday arrives mid-relationship-building with the deadline intact and nothing in writing — the week spent on rapport bought the vendor seven days of pressure for free.",
                   "principle":"A senior channel opened before the formal machinery exists isn't collaboration, it is unilateral disarmament with meetings — executives concede to processes, not to pleasantness."},
                  {"key":"comply_protect","label":"Accept under protest by Friday with an express reservation of rights, then pursue B and A from inside the preserved schedule — Q1's political stakes are too high to test whether the scheduling threat is bluff","quality":10,
                   "consequence":"The acceptance, however reserved, reprices the remaining 210 sites at +34% and teaches the vendor's account team what a deadline achieves; the reserved rights produce a partial clawback of 6% eleven months later through a dispute that costs more than it recovers — and the next 'final offer' arrives with a shorter deadline.",
                   "principle":"Paying a manufactured deadline to protect a political date protects neither — it funds the next deadline and converts your leverage facts into their pricing assumptions."}]},
               {"key":"aftermath","prompt":"Stage 2 — settled at 11%, you close the class: ",
                "options":[
                  {"key":"never_again","label":"Convert the week into permanent structure: price-certainty and open-book triggers into the contract at this settlement, the benchmark refreshed annually as standing market intelligence, the switching map maintained as a live document (its existence, not its execution, is the leverage), and a programme-level rule that no supplier deadline inside the contract's own variation process is ever answered by its date — with the week's chronology written up for the department as the playbook it asked for","quality":100,
                   "consequence":"The vendor's next variation, eighteen months later, arrives deadline-free with open-book workings attached — the account team's own retrospective concluded manufactured urgency now costs them credibility at no gain; the department circulates the playbook to two other programmes facing the same vendor tactic.",
                   "principle":"A negotiation won once is a tactic; its mechanisms embedded in the contract, the intelligence refreshed, and the counterparty's incentives repriced — that is the class closed."},
                  {"key":"move_on","label":"Bank the 11% and move on — the settlement speaks for itself, the relationship is repaired through the MD, and institutionalising war-machinery signals distrust the account doesn't need","quality":25,
                   "consequence":"The settlement speaks until the account team rotates; the next variation, from new hands, opens at +28% with a deadline — the tactic cost them nothing last time that anyone still remembers, and the switching map is eighteen months stale.",
                   "principle":"Counterparties institutionalise what worked; if you don't institutionalise what beat it, the next round starts from their memory, not yours."}]}],
             "hints":["Kill the clock before anything else — a deadline engaged with on its own terms has already won.",
               "Make the demand prove itself, and brief your principals before the pressure reaches them: open-book request, benchmark and switching map as parallel evidence engines.",
               "Senior channels work only above visible machinery — open the MD conversation once the process exists."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced six counter-moves against a Friday ultimatum — the deadline died Wednesday and the 34% settled at 11."}
            """),
    };
}
