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
    };
}
