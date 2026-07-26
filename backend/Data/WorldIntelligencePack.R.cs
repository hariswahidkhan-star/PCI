namespace PCI.Backend.Data;

/// <summary>
/// PCI Project Intelligence — Year-1 RESERVE pack (PI-Y1-R001..R055 → WC-*-366..420).
/// The 55-slot reserve bank: substitution stock held to the same gates as the scheduled year —
/// authored TO the governed reserve slots, three hints, consequence + principle per option,
/// synthetic data, validator + language + leakage gates enforced in CI. Foundation dailies,
/// practitioner rescues and numeric rooms, negotiation dilemmas, order/rank sequences and two
/// executive capstones, per the approved reserve distribution.
/// </summary>
public static partial class WorldIntelligencePack
{
    static readonly (string Code, string Title, string Hook, string Industry, string Role, string Track,
        string Difficulty, int Minutes, string Competencies, string Config)[] ItemsR =
    {

        // ═════════════ RESERVE BANK — PI-Y1-R001..R055 ═════════════
        // Foundation dailies: first-job judgement calls, one decision each, quick band.

        ("WC-CHG-366", "Two versions of the recovery plan", "Your PM and the contractor's PM each have 'the' plan. You have both inboxes.",
            "Stadia & Venues", "Assistant Project Manager", "project_management", "foundation", 7,
            """["change_control","communication"]""",
            """
            {"context":"On the stadium fit-out, the recovery from a three-week delay has produced two documents: your project manager's recovery plan (issued Tuesday) and the contractor's own version (issued Thursday), which differs on four activity dates and one crew assumption. Both are circulating; the steel subcontractor has just emailed you asking which plan governs their Saturday shift. Your PM is in a review all afternoon.",
             "evidence":[
               {"label":"Plan A","value":"Your PM's recovery plan — issued Tuesday"},
               {"label":"Plan B","value":"Contractor's version — Thursday, 4 dates + 1 crew assumption differ"},
               {"label":"Live question","value":"Steel subcontractor: which plan governs Saturday?"},
               {"label":"Constraint","value":"Your PM unavailable until evening"}],
             "decisions":[
               {"key":"answer","prompt":"You reply:",
                "options":[
                  {"key":"hold_confirm","label":"Tell the steel sub that Tuesday's client-issued plan governs until formally revised, flag the four-date conflict to both PMs in one email, and ask for a single reconciled issue before the weekend","quality":100,
                   "consequence":"Saturday runs on one plan; the reconciliation happens Friday morning because your email put the conflict in both PMs' inboxes with a deadline attached.",
                   "principle":"When two plans circulate, the formally-issued one governs until revised — and the person who spots the fork's job is to force a merge, not to pick a side."},
                  {"key":"newest","label":"Tell them to follow the contractor's Thursday version — it is newer and the contractor knows its own crews best","quality":15,
                   "consequence":"Saturday's steel shift follows dates your PM's plan contradicts; Monday opens with two subcontractors sequenced against each other and your name on the email that chose.",
                   "principle":"Newest is not the same as authorised — currency of information and authority to instruct are different tests."},
                  {"key":"wait","label":"Tell them you'll confirm once your PM is out of the review — better no answer than a wrong one","quality":30,
                   "consequence":"The sub, unanswered by 3pm, books the crew on their own reading; the answer you eventually send at 6pm now has to unwind a booking instead of shaping one.",
                   "principle":"Silence doesn't pause the site — someone always decides; the only question is whether it is the person with the most context."}]}],
             "hints":["Ask which document has been formally issued, not which is newest.",
               "The four-date conflict is the real finding — it needs both PMs, one email, one deadline.",
               "Give the sub an answer that holds for Saturday; perfection can follow on Monday."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held one plan in force and forced two PMs to merge the fork — before Saturday's steel shift chose for them."}
            """),

        ("WC-PRC-367", "The early warning in the loading bay", "The pipework supplier's driver says more than the supplier's letters ever have.",
            "Water Utilities", "Graduate Quantity Surveyor", "project_finance", "foundation", 5,
            """["procurement","commercial_awareness"]""",
            """
            {"context":"Collecting delivery paperwork at the water treatment upgrade's loading bay, you chat with the pipework supplier's driver, who mentions the fabrication shop has 'gone to a three-day week' and two of your project's spool deliveries are 'being juggled'. Nothing formal has arrived from the supplier; their account manager assured your commercial manager 'all on programme' at last month's review. The next spool delivery gates a road-crossing shutdown booked with the highways authority in five weeks.",
             "evidence":[
               {"label":"Informal signal","value":"Driver: fabrication shop on a 3-day week, your spools 'being juggled'"},
               {"label":"Formal position","value":"Account manager last month: 'all on programme'"},
               {"label":"Exposure","value":"Next spools gate a highways shutdown in 5 weeks"}],
             "decisions":[
               {"key":"signal","prompt":"You:",
                "options":[
                  {"key":"escalate_verify","label":"Pass the signal to your commercial manager today, verbatim and labelled as informal — and suggest a specific verification: a delivery-confirmation request for the two gating spools, which forces the supplier to commit in writing either way","quality":100,
                   "consequence":"The confirmation request comes back hedged — 'reviewing schedule adherence' — which tells the commercial manager everything; a works visit is arranged within the week and the gating spools are re-prioritised while five weeks still exist.",
                   "principle":"Informal signals aren't evidence, but they tell you exactly which formal question to ask — the loading bay often knows before the account manager admits."},
                  {"key":"dismiss","label":"Note it but take no action — drivers grumble, the account manager's assurance is the formal record, and relaying gossip risks souring the relationship","quality":0,
                   "consequence":"The formal notice of delay arrives eighteen days later, past the point where the highways slot could be protected; the shutdown re-books for eleven weeks out, and the driver's remark is remembered by everyone you didn't tell.",
                   "principle":"The signal you sat on is the one that defines you — relaying an observation with its label ('informal, unverified') costs nothing and risks nothing."},
                  {"key":"confront","label":"Ring the supplier's account manager yourself and ask directly whether the fabrication shop is on short time","quality":25,
                   "consequence":"The account manager smoothly reassures you, mentions the call to your commercial manager before you do, and the enquiry now looks like a graduate going around the relationship — while the spools stay juggled.",
                   "principle":"Verification questions travel best through the channel that owns the relationship — your job is arming that channel fast, not testing your own."}]}],
             "hints":["Label the signal honestly — informal, unverified — and pass it up today, not eventually.",
               "Suggest the verification that forces a written commitment on the two spools that actually gate the shutdown.",
               "Watch the reply's shape: a hedge in response to a direct confirmation request is itself the answer."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a loading-bay remark into a written supplier commitment — five weeks before it became a formal delay notice."}
            """),

        ("WC-QLT-368", "The test evidence that predates the test", "The screenshot says Tuesday. The test environment says it wasn't built until Thursday.",
            "Technology Programmes", "Junior Test Analyst", "project_controls", "foundation", 6,
            """["quality_management","evidence_analysis"]""",
            """
            {"context":"Reviewing the core-system replacement's test evidence pack before a milestone sign-off, you notice something odd: the integration test results for the payments module are dated Tuesday, but you remember the test environment those results reference wasn't stood up until Thursday — you helped configure it. The test lead — experienced, respected, and under pressure to hit the milestone — signed the pack yesterday. The results themselves look plausible; the milestone payment is significant; sign-off goes to the client tomorrow.",
             "evidence":[
               {"label":"Anomaly","value":"Payments results dated Tuesday; referenced environment built Thursday"},
               {"label":"Your knowledge","value":"You helped configure the environment — the dates are yours"},
               {"label":"Status","value":"Pack signed by the test lead; client sign-off tomorrow"}],
             "decisions":[
               {"key":"anomaly","prompt":"You:",
                "options":[
                  {"key":"raise_direct","label":"Take the date conflict to the test lead today, privately, as a question not an accusation — 'these dates can't both be right; can you help me understand?' — and if the answer doesn't resolve it, say plainly that you can't stay silent while the pack goes to the client","quality":100,
                   "consequence":"The test lead checks, goes pale, and finds the truth: results copied forward from the previous environment's run 'as a placeholder' and never replaced. The real tests run Thursday night, two defects surface and fix over the weekend, and the pack that reaches the client is true — one day late.",
                   "principle":"Evidence anomalies get raised to the person who signed, first, as questions — most are process slips, not fraud, but every one of them must be resolved before the pack crosses an organisational boundary."},
                  {"key":"silent","label":"Let it go — the test lead is experienced, the results look right, and a junior questioning a signed pack the day before sign-off helps nobody","quality":0,
                   "consequence":"The placeholder results ship; the two defects they would have caught surface in production month one, the evidence pack is audited, and the date anomaly you noticed is found by someone who also finds out who configured the environment.",
                   "principle":"Knowing about an evidence defect and staying silent converts someone else's shortcut into your concealment — juniority is not immunity."},
                  {"key":"skip_up","label":"Report it straight to the programme's quality manager — dates that can't be true in a signed pack are above your pay grade to investigate","quality":30,
                   "consequence":"The quality manager investigates formally; the same placeholder explanation emerges in a week instead of an afternoon, the milestone slips further, and the test lead — who would have fixed it in a day if asked — learns you went around them first.",
                   "principle":"Escalation is the second move, not the first — the signer deserves the question before the system receives the report."}]}],
             "hints":["State the two facts side by side — the dates conflict, and only one of them can be right.",
               "Ask the signer first, as a question; escalate only if the answer doesn't resolve the conflict.",
               "The deadline is tomorrow, which is exactly why it must be raised today."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Asked one uncomfortable question about two impossible dates — and the pack that reached the client was true."}
            """),

        ("WC-AIA-369", "The tool that learned the old factory", "The layout changed on Saturday. The congestion model hasn't heard.",
            "Manufacturing", "Junior Planner", "project_controls", "foundation", 7,
            """["ai_assurance","schedule_analysis"]""",
            """
            {"context":"The factory relocation uses a vendor tool that predicts internal logistics congestion — it schedules machine moves to avoid aisle conflicts, trained on months of the old layout's movement data. This weekend the commissioning team re-routed the main aisle around the new paint line. Monday morning, the tool's move schedule for the week still shows green: no congestion predicted. Your walk to the coffee machine says otherwise — the re-routed aisle now takes fork-lifts past the busiest assembly cell. The move coordinator trusts the tool ('it's been right all year') and has released the week's schedule.",
             "evidence":[
               {"label":"Change","value":"Main aisle re-routed around the new paint line — this weekend"},
               {"label":"Tool","value":"Congestion model trained on the OLD layout; week's schedule shows green"},
               {"label":"Observation","value":"New route passes the busiest assembly cell"},
               {"label":"Status","value":"Schedule released; coordinator: 'right all year'"}],
             "decisions":[
               {"key":"tool","prompt":"You:",
                "options":[
                  {"key":"ground_truth","label":"Tell the coordinator the specific reason the green can't be trusted this week — the model has never seen the new aisle — and propose the cheap check: walk the two highest-traffic move windows against the new route on a plan, manually, before those moves run","quality":100,
                   "consequence":"The manual check finds one genuine conflict — Wednesday's press-bed move meets the assembly cell's shift change in the re-routed aisle — re-timed for the price of a conversation; the vendor is asked to retrain the model on the new layout that afternoon.",
                   "principle":"A model is only as current as its training data — when the world changes faster than the tool, the green light means 'no congestion in a layout that no longer exists'."},
                  {"key":"trust","label":"Defer to the tool and the coordinator — a year of accuracy has earned the benefit of the doubt, and the layout change may not matter as much as your walk suggests","quality":0,
                   "consequence":"Wednesday's press-bed move meets the shift change exactly where your coffee walk predicted; a fork-lift near-miss stops moves for two days of investigation, and 'the tool said green' convinces nobody at the safety review.",
                   "principle":"A track record earned in one world doesn't transfer to another — deference to a stale model is just deference to the past."},
                  {"key":"override_all","label":"Push to suspend the tool entirely and revert to manual move scheduling until it can be retrained","quality":30,
                   "consequence":"Manual scheduling of forty moves a week overwhelms the team by Thursday; the tool was wrong about one aisle, not forty moves, and the blanket suspension trades a specific known gap for general chaos.",
                   "principle":"A tool blind in one corner needs a manual check in that corner — not abandonment everywhere it still sees."}]}],
             "hints":["Ask what the model was trained on, and whether that world still exists.",
               "The layout change is days old; the training data is months old — the gap is the finding.",
               "Propose the targeted manual check on the changed corridor; keep the tool everywhere it is still current."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a congestion model predicting a factory that no longer existed — and fixed Wednesday with a walk and a plan."}
            """),

        ("WC-SAF-370", "The lesson that praised the luck", "The closeout report calls it 'excellent incident response'. It was a coin toss.",
            "Rail Systems", "Assistant Engineer", "project_management", "foundation", 5,
            """["safety_management","closeout"]""",
            """
            {"context":"Drafting the rail resignalling project's closeout lessons register, you review the season's near-miss file. One entry stands out: a possession overran at dawn and a maintenance trolley was still on the track as the first service approached — stopped in time because a signaller happened to query the possession's late give-back. The project's incident review, written the same week, praises the 'effective last-line response'. The draft lesson you've been handed to include says: 'Incident response procedures worked as intended.' The schedule pressure that caused the overrun — a give-back time set optimistically to protect the programme — appears nowhere in the file.",
             "evidence":[
               {"label":"Event","value":"Possession overran; trolley on track; stopped by a signaller's chance query"},
               {"label":"Review verdict","value":"'Effective last-line response'"},
               {"label":"Draft lesson","value":"'Procedures worked as intended'"},
               {"label":"Absent","value":"The optimistic give-back time that caused the overrun"}],
             "decisions":[
               {"key":"lesson","prompt":"In the closeout register, you:",
                "options":[
                  {"key":"rewrite","label":"Rewrite the lesson to name the system, not the save: the near-miss existed because the give-back time was set to protect the programme rather than the possession's actual work content — with the signaller's query recorded as luck that worked once, and the recommendation aimed at how give-back times get set","quality":100,
                   "consequence":"The rewritten lesson is challenged by the possession planner, survives on the timeline evidence, and changes the next project's give-back rule — possessions sized on work content plus verified margin. The signaller still gets the commendation, for the right reason.",
                   "principle":"A near-miss praised as a successful response teaches the organisation to rely on its luck — the lesson lives in what created the danger, not in what narrowly ended it."},
                  {"key":"asdrafted","label":"Include the lesson as drafted — the response DID work, the closeout is not the venue for relitigating a possession plan, and the incident review already ruled","quality":0,
                   "consequence":"The register ships its comfort; the next project inherits the same give-back optimism with a certified 'procedures work' lesson attached, and the next trolley meets a signaller who doesn't happen to query.",
                   "principle":"Lessons registers that record what went right about what went wrong are how organisations rehearse their next incident."},
                  {"key":"both_soft","label":"Add a second, softer lesson about 'reviewing possession planning assumptions' alongside the drafted one — both truths, no confrontation","quality":30,
                   "consequence":"The two lessons neutralise each other: future planners quote the first, auditors note the second, and the give-back practice continues under a register that technically mentioned it.",
                   "principle":"A lesson diluted to avoid an argument transmits the argument, not the lesson."}]}],
             "hints":["Separate the save from the cause — the register's job is the cause.",
               "Ask why the trolley was still there, not just how it was spotted.",
               "Praise the signaller in the commendation file; aim the lesson at how give-back times get set."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rewrote a closeout lesson that praised the luck — so the next possession is sized on work, not hope."}
            """),

        ("WC-GOV-371", "Split board, junior analyst, one slide", "Both camps want your utilisation data. Each wants a different version.",
            "Defence Estates", "Junior Business Analyst", "project_management", "foundation", 6,
            """["governance","reporting_integrity"]""",
            """
            {"context":"The defence facility upgrade's investment board is split on whether to approve the next phase, and both camps have separately asked you — the analyst who maintains the estate utilisation model — for supporting data. The sponsoring director's office wants 'the occupancy projections showing the consolidation benefits'; the sceptical finance member's office wants 'the historical utilisation actuals showing the estate is underused'. Both datasets are real, from the same model; each, alone, supports its requester's case. Your slide goes into the board pack under the programme office's name, due tomorrow.",
             "evidence":[
               {"label":"Request A","value":"Sponsor's office: projections showing consolidation benefits"},
               {"label":"Request B","value":"Finance member's office: actuals showing underuse"},
               {"label":"Fact","value":"Both are true extracts of the same model"},
               {"label":"Output","value":"One slide, programme office's name, due tomorrow"}],
             "decisions":[
               {"key":"slide","prompt":"You produce:",
                "options":[
                  {"key":"whole","label":"One slide with both series on one chart — actuals to date, projections forward, the assumption that bridges them stated in the caption — sent to both offices identically, with a note to your manager explaining the two requests and why the slide answers both","quality":100,
                   "consequence":"The board argues about the bridging assumption — which is the actual decision — instead of trading rival extracts; your manager forwards your note with approval, and both offices learn the model's keeper doesn't do curated versions.",
                   "principle":"When two sides request the halves of one truth, the analyst's protection and the board's need are the same object: the whole chart, identically distributed, with the assumption that connects the halves in writing."},
                  {"key":"comply_both","label":"Send each office the extract it asked for — both are true, requesters frame their own arguments, and an analyst doesn't referee a board dispute","quality":10,
                   "consequence":"The two extracts meet in the boardroom as duelling slides with the same source's name on both; the model's credibility — and yours — becomes the meeting's casualty, and the phase decision defers pending 'data reconciliation' of data that was never unreconciled.",
                   "principle":"Supplying curated halves to opposing camps doesn't keep you neutral — it makes the data the combatant and its keeper the first casualty."},
                  {"key":"defer_up","label":"Forward both requests to your manager and produce nothing until told which to fulfil","quality":35,
                   "consequence":"Safe and slow: your manager — in transit until late — answers at 9pm with 'do the combined view, obviously', and the slide is built at midnight to a standard the afternoon would have improved.",
                   "principle":"Escalating a conflict is right; escalating it empty-handed wastes the escalation — bring the recommended answer with the question."}]}],
             "hints":["Notice the two requests are halves of one chart — the conflict dissolves in the combined view.",
               "Identical distribution is the discipline: same slide, both offices, same minute.",
               "Tell your manager with the answer attached, not instead of one."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Answered two rival data requests with one whole chart — and let the board argue about the assumption instead of the analyst."}
            """),

        ("WC-CHG-372", "The variation that started itself", "The ward's contractor is already building the change. The paperwork hasn't started.",
            "Healthcare Estates", "Assistant Project Manager", "project_management", "foundation", 7,
            """["change_control","contract_management"]""",
            """
            {"context":"Walking the hospital ward refurbishment on Thursday, you find the contractor's joiners re-framing the nurse-station openings to a wider dimension — 'per the infection-control team's request from Tuesday's walkround'. You check: an infection-control nurse did indeed ask for wider openings during the walkround, verbally, to the site foreman. No instruction has been issued, no price agreed, no drawing revised; the design team hasn't heard of it. The work is perhaps a third done. The contractor's foreman is entirely unconcerned: 'clinical asked for it, so we're doing it'.",
             "evidence":[
               {"label":"Found","value":"Nurse-station openings being re-framed wider — ~1/3 complete"},
               {"label":"Basis","value":"Verbal request from infection-control nurse at Tuesday's walkround"},
               {"label":"Paperwork","value":"No instruction, no price, no drawing revision; design team unaware"},
               {"label":"Foreman","value":"'Clinical asked, so we're doing it'"}],
             "decisions":[
               {"key":"found_change","prompt":"You:",
                "options":[
                  {"key":"pause_regularise","label":"Ask the foreman to pause that work element today (nothing else), then run the change properly at speed: the design team checks the wider opening (fire strategy and door-set implications), the request is either confirmed as an instruction with a price or reversed while re-framing is cheap — and the walkround protocol gains a rule that clinical requests route through the change process, said kindly to the nurse who asked","quality":100,
                   "consequence":"The design check matters: the wider opening works at six of eight stations, but two would breach the fire-door schedule — caught at one-third framed instead of at inspection. The instruction issues Friday for six; the nurse gets what infection control needed, minus the two that would have failed certification.",
                   "principle":"Work born from a verbal request is a change already happening — pause the smallest possible scope, verify the design, and convert it to an instruction or reverse it while the cost is still lumber."},
                  {"key":"let_finish","label":"Let the work finish — pausing costs more than framing, the clinical need is real, and the paperwork can regularise what's already fact","quality":10,
                   "consequence":"All eight stations frame wide; the fire-strategy review at completion fails two, and the re-instatement — demolition of finished work, at finished-work prices — costs eight times Thursday's pause, plus a variation dispute about who authorised what.",
                   "principle":"Regularising a change after completion means paying to discover what a pause would have discovered at a third of the framing."},
                  {"key":"refuse_all","label":"Instruct the contractor to reverse the work immediately — unauthorised work is unauthorised, whatever clinical asked for","quality":25,
                   "consequence":"The reversal instruction is contractually clean and clinically deaf: infection control's request was sound for six stations, the reversal costs real money to undo work that mostly should have stayed, and the ward team learns the project treats its input as a compliance problem.",
                   "principle":"Unauthorised is a process verdict, not a design one — check whether the change is right before paying to undo it."}]}],
             "hints":["Pause only the affected element — the smallest intervention that stops the meter.",
               "The design check is the real question: verbal requests skip exactly the reviews that exist for a reason.",
               "Fix the channel kindly — clinical input is wanted, through the process that prices and verifies it."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Paused a self-starting variation at one-third framed — and saved two fire doors and one clinical relationship."}
            """),

        ("WC-PRC-373", "'Just get it delivered' is not a scope", "The instruction fits in one line. The invoice won't.",
            "Renewable Energy", "Graduate Commercial Assistant", "project_finance", "foundation", 5,
            """["procurement","contract_management"]""",
            """
            {"context":"The wind farm repowering's site manager, mid-crisis over a late blade delivery, forwards you an email he has just sent to the logistics contractor: 'Whatever it takes, just get the blades to site by Friday — we'll sort the commercials after.' The logistics contractor has replied within minutes: 'Confirmed, mobilising additional resources as instructed.' You are the commercial assistant on the package; your commercial manager is on leave; the contract has a defined variation procedure with rates for exactly this kind of acceleration.",
             "evidence":[
               {"label":"Instruction","value":"'Whatever it takes… sort the commercials after' — site manager to logistics contractor"},
               {"label":"Reply","value":"'Confirmed, mobilising additional resources as instructed' — within minutes"},
               {"label":"Contract","value":"Defined variation procedure with acceleration rates exists"},
               {"label":"Cover","value":"Commercial manager on leave; you are the commercial presence"}],
             "decisions":[
               {"key":"open_ended","prompt":"You:",
                "options":[
                  {"key":"bound_today","label":"Get a bounding instruction out today: draft a confirmation for the site manager to issue that references the contract's acceleration rates, defines what 'whatever it takes' includes (additional vehicles, escorts, weekend permits) and excludes, and requires daily cost records — the urgency stands, the blank cheque doesn't","quality":100,
                   "consequence":"The blades arrive Friday under an instruction with rates and records attached; the acceleration invoice, when it comes, prices to the contract's schedule instead of to the phrase 'whatever it takes' — the difference, your commercial manager notes on return, was likely five figures.",
                   "principle":"Urgency and open-endedness are separable — the fix for a blank-cheque instruction is a same-day bounding confirmation that keeps the speed and restores the rates."},
                  {"key":"after","label":"Let it run — the site manager owns the crisis, the phrase 'sort the commercials after' at least acknowledges a reckoning, and interrupting the recovery to talk paperwork reads badly","quality":0,
                   "consequence":"'After' arrives as an invoice at three times the contract's acceleration rates — premium vehicles, standby time, 'management fees' — all plausibly within 'whatever it takes', all confirmed in writing, all payable.",
                   "principle":"'Sort the commercials after' means 'price it yourself and tell us later' — the reckoning acknowledged is the reckoning surrendered."},
                  {"key":"countermand","label":"Email the logistics contractor yourself noting the instruction is subject to the contract's variation procedure and rates — correcting the record before mobilisation hardens","quality":30,
                   "consequence":"Contractually right, organisationally wrong: a graduate publicly qualifying the site manager's instruction to his contractor starts a status fight in the middle of a crisis; the correction was needed — issued by the instruction's author, which one phone call would have achieved.",
                   "principle":"The record needs correcting through the person who made it — speed matters, but so does whose name fixes what."}]}],
             "hints":["The urgency is legitimate; the open-endedness is the exposure — separate them.",
               "The contract already has acceleration rates: reference them the same day, before mobilisation prices itself.",
               "Route the bounding confirmation through the instruction's author — one call, his signature."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned 'whatever it takes' into a bounded instruction with rates — before Friday's blades became an unpriceable invoice."}
            """),

        ("WC-QLT-374", "The snag that outranks the list", "Item 214 of 300 is not like the others.",
            "Broadcast & Media", "Assistant Project Manager", "project_management", "foundation", 6,
            """["quality_management","commissioning"]""",
            """
            {"context":"The broadcast facility move's snagging list stands at 300 items, worked methodically by trade and area. Walking studio B with the list, you notice item 214 — 'acoustic door seal incomplete, studio B rear' — logged three weeks ago, still open, categorised 'minor: cosmetic/finishing'. But studio B's acceptance test — the acoustic isolation measurement the broadcaster's engineers run — is booked for Monday, and an incomplete rear door seal is exactly what fails an isolation test. The snagging supervisor works the list in order; at current rates, item 214 comes up in about two weeks.",
             "evidence":[
               {"label":"Item 214","value":"Acoustic door seal incomplete — logged 3 weeks, categorised 'minor'"},
               {"label":"Collision","value":"Studio B isolation test Monday; incomplete seal = failed test"},
               {"label":"Process","value":"List worked in order; 214 due in ~2 weeks"}],
             "decisions":[
               {"key":"snag","prompt":"You:",
                "options":[
                  {"key":"resequence","label":"Flag 214 to the snagging supervisor today as test-blocking, get it pulled forward ahead of Monday — and then ask the better question: which OTHER open items sit in the path of a booked acceptance test, because a list categorised by trade severity has no column for 'blocks a milestone'","quality":100,
                   "consequence":"The seal completes Thursday; the sweep you asked for finds two more test-blockers hiding as minors — a cable-tray bond and a door-closer adjustment — both cleared before their tests. Studio B passes Monday; the list gains a 'blocks' column by Friday.",
                   "principle":"Snag severity and schedule criticality are different rankings — a minor item in front of a booked test outranks a major one that blocks nothing, and only a cross-reference against the test calendar can see it."},
                  {"key":"in_order","label":"Leave the list to its order — the process is working, 300 items need discipline not exceptions, and the test team can note the seal as a known condition","quality":10,
                   "consequence":"Monday's isolation test fails on the rear door as physics requires; the broadcaster's engineers re-book for three weeks out — their next window — and a two-hour seal job costs twenty-one days of studio B's programme.",
                   "principle":"Discipline about list order is not a virtue when the order is blind to the calendar — 'known condition' doesn't pass tests."},
                  {"key":"do_it","label":"Get the seal fixed quietly yourself — ask the joiner directly, skip the supervisor and the list politics","quality":25,
                   "consequence":"The seal gets fixed and the system stays blind: the two other test-blockers you never looked for hold their list positions, and one of them fails its own test a fortnight later while the supervisor wonders why his list keeps being worked around.",
                   "principle":"Fixing the instance by bypass leaves the category undiscovered — the list's blindness was the finding, not the seal."}]}],
             "hints":["Cross-reference the open snags against the acceptance-test calendar — that's the ranking that matters this month.",
               "Item 214's category is 'minor'; its consequence is a failed test and a three-week re-book — argue the consequence.",
               "Fix the system too: the list needs a 'blocks a test' flag, not just trade severity."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Pulled one 'minor' snag ahead of Monday's isolation test — and gave a 300-item list the column it was missing."}
            """),

        ("WC-DQA-375", "Green on the dashboard, red in the room", "The framework's dashboard averages away the only number that matters.",
            "Framework Programmes", "Junior Reporting Analyst", "project_controls", "foundation", 7,
            """["data_quality","reporting_integrity"]""",
            """
            {"context":"You compile the framework programme's commissioning dashboard, which reports each facility's readiness as a single percentage — the average of six workstream scores. Facility 3 shows 82%, comfortably green. Preparing this month's pack, you notice the average conceals a zero: five workstreams in the nineties, but 'operational staffing' at 8% — the facility's operator has hired almost nobody, and hiring lead time exceeds the time to opening. The dashboard's method is inherited, documented and signed off; changing a reporting method mid-programme needs the reporting board's approval, which meets after this month's pack ships.",
             "evidence":[
               {"label":"Reported","value":"Facility 3: 82% — green"},
               {"label":"Reality","value":"Five workstreams 90s; operational staffing 8%"},
               {"label":"Physics","value":"Hiring lead time > time to opening"},
               {"label":"Constraint","value":"Method change needs board approval — after this pack ships"}],
             "decisions":[
               {"key":"average","prompt":"This month's pack:",
                "options":[
                  {"key":"annotate","label":"Ship the method as approved AND make the zero visible: the 82% stays (you can't change method unilaterally) with a called-out exception flag — 'staffing 8%, lead time exceeds time to opening, average not representative' — plus a method-change paper to the reporting board proposing minimum-workstream reporting alongside the average","quality":100,
                   "consequence":"The exception flag does what averages can't: facility 3's staffing goes to the programme director's Monday list, the operator's hiring escalates that week, and the board approves min-plus-average reporting at its next sitting — retiring the class of concealment, not just the instance.",
                   "principle":"When an approved method hides a truth, you follow the method and surface the truth beside it — annotation now, method change through the front door."},
                  {"key":"method","label":"Report 82% as the method requires — the method is signed off, exceptions are the workstream leads' job to raise, and analysts who edit approved reports stop being trusted with them","quality":0,
                   "consequence":"Green ships; the staffing zero surfaces eight weeks later as 'facility 3 cannot open', and the review that follows asks one question of the dashboard's compiler: when did you know?",
                   "principle":"An approved method is authority to calculate, not authority to conceal — the analyst who saw the zero owns the silence."},
                  {"key":"recalc","label":"Change the calculation quietly this month — report facility 3 at its minimum workstream score, since that's the honest number","quality":20,
                   "consequence":"Facility 3 turns red with no method note; three facility managers demand to know why their scores moved, the reporting board learns its method was edited without it, and the honest number arrives wrapped in a governance breach.",
                   "principle":"Fixing a method by stealth converts a data problem into a trust problem — the truth needs the front door."}]}],
             "hints":["Averages conceal minimums — ask what the worst workstream is before trusting any composite.",
               "You can't change the method this month; you can annotate the exception in plain sight.",
               "Take the method fix to the board that owns the method — min-plus-average, proposed properly."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Flagged the 8% hiding inside an 82% average — and got the method fixed through the door it owned."}
            """),

        ("WC-SAF-376", "The near miss the draft softened", "'Contact was avoided' is one way to describe a reversing wagon and a surveyor.",
            "Highways", "Graduate Engineer", "project_management", "foundation", 5,
            """["safety_management","reporting_integrity"]""",
            """
            {"context":"On the highway upgrade, you witnessed Tuesday's near miss: a reversing wagon in the compound came within a metre of a surveyor whose exclusion zone had been quietly encroached for weeks. You're asked to review the draft incident report before it goes to the principal contractor. The draft is technically accurate and consistently soft: 'contact was avoided' (by a shout), 'exclusion arrangements were in place' (on paper), 'refresher briefing delivered' (that afternoon, to whoever was in the cabin). The site agent who drafted it mentions the project's reportable-incident statistics are 'the best on the framework' and this 'doesn't need to become a number'.",
             "evidence":[
               {"label":"Event","value":"Reversing wagon, ~1m from surveyor, encroached exclusion zone — weeks of encroachment"},
               {"label":"Draft","value":"'Contact avoided' / 'arrangements in place' / 'briefing delivered'"},
               {"label":"Pressure","value":"'Best statistics on the framework — doesn't need to become a number'"}],
             "decisions":[
               {"key":"report","prompt":"You:",
                "options":[
                  {"key":"accurate","label":"Return the draft with the facts restored — the metre, the shout, the weeks of encroachment — and say plainly that you witnessed it and will be accurate in any record that carries your review; the classification is the system's call, but only on a truthful account","quality":100,
                   "consequence":"The report goes up accurate and is classified as the near miss it was; the investigation finds the compound layout made encroachment inevitable and re-designs it in a week — the fix the softened draft would have made unnecessary-looking.",
                   "principle":"Near-miss reports exist to buy the fix before the fatality — softening one converts the cheapest safety data there is into a statistic protected at the site's expense."},
                  {"key":"sign","label":"Let the draft stand — every statement in it is technically true, classification is the site agent's judgement, and a graduate contradicting the agent's account helps no one","quality":0,
                   "consequence":"The softened report files as a routine observation; the compound layout stays, the encroachment resumes within days, and the next wagon has no shout — the investigation that follows reads Tuesday's draft with your review noted on it.",
                   "principle":"'Technically true' assembled to minimise is the standard architecture of the report before the accident — reviewers who witnessed the event own what they let stand."},
                  {"key":"anonymous","label":"Let the draft go but raise the layout concern separately through the safety observation scheme, anonymously","quality":30,
                   "consequence":"The observation card triggers a leisurely layout review scheduled next quarter; the report of record still says nothing happened, and the two documents about the same metre never meet.",
                   "principle":"A side channel can raise a hazard; it cannot repair a record — the report is where the organisation's memory lives."}]}],
             "hints":["Compare what you saw with what each sentence implies — the gap is the finding.",
               "The statistic being protected is the exact reason the report matters.",
               "Restore facts, not adjectives: the metre, the shout, the weeks — classification follows honestly from there."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put the metre and the shout back into a softened near-miss report — and the compound got fixed instead of the statistic."}
            """),

        ("WC-GOV-377", "The benefit that sailed already", "The campaign's business case counts a saving the last campaign already claimed.",
            "Offshore Energy", "Junior Analyst", "project_management", "foundation", 6,
            """["governance","benefits_management"]""",
            """
            {"context":"Assembling the appendices for the offshore maintenance campaign's business case, you cross-check its benefits table against last year's completed campaign — and find the headline efficiency benefit, 'vessel utilisation improvement from combined workscopes', is the same saving the previous campaign's closeout report already claimed as delivered. Same mechanism, same baseline, substantially the same number. The case's author — a manager you like — waves it off: 'the improvement continues, so it counts again'. The case goes to the sanction board Friday.",
             "evidence":[
               {"label":"This case","value":"Headline benefit: vessel utilisation from combined workscopes"},
               {"label":"Last closeout","value":"Same mechanism, same baseline, claimed as delivered"},
               {"label":"Defence","value":"'The improvement continues, so it counts again'"},
               {"label":"Clock","value":"Sanction board Friday"}],
             "decisions":[
               {"key":"double","prompt":"You:",
                "options":[
                  {"key":"baseline","label":"Put the two documents side by side for the author: a continuing improvement is real but its baseline moved — last year's case banked the step change, so this case may only claim improvement BEYOND the new baseline; offer to recut the benefit honestly (the incremental gain from this campaign's specific workscope combinations) before Friday","quality":100,
                   "consequence":"The recut benefit is 40% of the original claim and survivable — the case still sanctions, on a number that won't be clawed back; the author, initially prickly, uses the two-document comparison at the board unprompted when a member asks the exact question you asked.",
                   "principle":"A benefit claimed twice from the same baseline is the commonest double-count in portfolio management — improvements continue, but baselines move; each case may claim only its own increment."},
                  {"key":"let_go","label":"Let it stand — benefits methodology is the author's call, the improvement genuinely continues, and juniors don't re-audit their managers' cases","quality":0,
                   "consequence":"A sanction-board member's analyst runs the same cross-check you ran, finds the same duplication, and the case bounces publicly for a rework that lands three weeks late — with the duplication now a story about the team's rigour rather than a fixable draft note.",
                   "principle":"The check you declined to raise privately gets raised publicly — cross-checks against prior claims are exactly what boards' analysts do."},
                  {"key":"footnote","label":"Suggest a footnote acknowledging the prior claim while keeping the headline number — transparency without a Friday rewrite","quality":25,
                   "consequence":"The footnote does its quiet work: a board member reads it, asks what the number would be WITHOUT the previously-claimed portion, and the author improvises the recut at the podium — the honest number arriving in the worst possible way.",
                   "principle":"A footnote that undermines the headline is a question planted for the worst moment — recut the number or defend it; annotating the contradiction is neither."}]}],
             "hints":["Cross-check this case's benefits against what prior cases already claimed delivered — same mechanism, same baseline is the tell.",
               "'The improvement continues' concedes the point: the baseline moved when it was banked.",
               "Offer the recut, not just the problem — the incremental claim usually survives sanction."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a benefit being banked twice from the same baseline — and recut it to the increment before the board's analyst could."}
            """),

        ("WC-CHG-378", "Read the diary before the claim does", "The contractor photographs their daily diary. Yours went unfilled for a month.",
            "Technology Programmes", "Assistant Project Manager", "project_management", "foundation", 7,
            """["change_control","evidence_analysis"]""",
            """
            {"context":"On the network modernisation, the cabling contractor has begun — politely, professionally — building a delay narrative: their weekly reports now carry phrases like 'access to comms rooms again constrained by others' and their site diary, you notice, is photographed and filed daily. Checking your own side's records for the same period, you find the client diary was last completed five weeks ago — the site supervisor 'got busy'. From your own walkrounds you know the access story is half true at best: two genuine clashes with the electrical contractor, but most days the rooms sat available. The period in question is still running.",
             "evidence":[
               {"label":"Their records","value":"Daily diary photographed and filed; weekly reports seeding 'access constrained'"},
               {"label":"Your records","value":"Client diary unfilled for 5 weeks"},
               {"label":"Your knowledge","value":"2 genuine clashes; most days rooms available"},
               {"label":"Timing","value":"The period is still running"}],
             "decisions":[
               {"key":"records","prompt":"You:",
                "options":[
                  {"key":"restart_now","label":"Restart the client record today and make it specific where it matters: daily comms-room availability logged by room with times, the two genuine clashes recorded honestly (they happened; pretending otherwise poisons the record), and the contractor's 'again constrained' phrasing answered in the weekly meeting minutes with the availability log attached — contemporaneously, while the period is still live","quality":100,
                   "consequence":"The narrative meets arithmetic while both are fresh: faced with a room-by-room availability log, the contractor's reports drop 'again' within a fortnight, the two real clashes settle as a small priced variation — and the claim that was being assembled never files, because its evidence now has a contemporaneous rival.",
                   "principle":"Delay narratives are built in the present tense from the only records that exist — the party that stops keeping records has voted for the other side's version of events."},
                  {"key":"backfill","label":"Reconstruct the five missing weeks from emails, walkround photos and memory — a complete record beats a punctual one","quality":20,
                   "consequence":"The reconstruction is done honestly and reads exactly like what it is: a record created after the narrative it answers, in different ink; at adjudication it is weighed against a photographed daily diary and loses on provenance alone. The forward log you also started is what saves the position.",
                   "principle":"Records derive their power from when they were made, not how complete they are — backfill explains; only contemporaneity proves."},
                  {"key":"challenge","label":"Raise the access narrative formally now — a letter rejecting the 'constrained access' characterisation before it hardens","quality":25,
                   "consequence":"The letter, unsupported by any client-side record of what access actually was, reads as assertion against their diary; the contractor's measured reply attaches three photographed pages, and the exchange strengthens the narrative it meant to stop.",
                   "principle":"Never open an evidence contest before you hold evidence — the letter can wait a fortnight; the log cannot wait a day."}]}],
             "hints":["Check whose records exist for the disputed period before deciding anything — that is the whole battlefield.",
               "Record the two real clashes honestly; a log that admits the true 10% is believed on the other 90%.",
               "Answer 'again constrained' in minutes with the availability log attached — contemporaneously, not retrospectively."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Restarted a dead site diary while the delay narrative was still being drafted — and the claim never found reason to file."}
            """),

        ("WC-PRC-379", "The answer that widened the job", "One helpful reply to one bidder, and suddenly the tender means more than it says.",
            "Advanced Manufacturing", "Graduate Buyer", "project_finance", "foundation", 5,
            """["procurement","governance"]""",
            """
            {"context":"You administer the clarification inbox for the pilot production ramp's installation tender. A bidder emails asking whether 'commissioning support' in the scope includes operator training during the ramp phase. The package engineer, cc'd, replies directly to that bidder within the hour: 'Yes — bidders should include for training operators through ramp to full rate.' The scope document doesn't say that; the other four bidders haven't seen the answer; and the tender rules require clarifications to be anonymised and circulated to all bidders. Submissions close in six days.",
             "evidence":[
               {"label":"The answer","value":"'Include operator training through ramp' — sent to ONE bidder"},
               {"label":"Scope document","value":"Says 'commissioning support'; training not defined"},
               {"label":"Rules","value":"Clarifications must be anonymised and circulated to all"},
               {"label":"Clock","value":"Submissions close in 6 days"}],
             "decisions":[
               {"key":"leak","prompt":"You:",
                "options":[
                  {"key":"circulate","label":"Repair it through the process the same day: issue the question and answer, anonymised, to all five bidders as a formal clarification — the engineer's answer becomes everyone's answer, the scope effectively amends equally, and six days is enough for all bids to price it; note to the engineer, kindly, that answers route through the inbox precisely so this repair is never needed","quality":100,
                   "consequence":"All five bids price the training scope; the spread at opening is explicable and the award unchallengeable on information grounds — and the engineer, shown how close one helpful email came to a challenge, becomes the inbox's most disciplined user.",
                   "principle":"A single-bidder answer is an information asymmetry with a timestamp — circulated same-day it becomes a clarification; left until opening it becomes a challenge."},
                  {"key":"quiet","label":"Leave it — one email, one bidder, probably immaterial to the outcome, and formalising it now embarrasses the engineer and invites questions about the inbox","quality":0,
                   "consequence":"The informed bidder prices training; the winner — a different bidder — doesn't, and the training gap surfaces as a post-award variation. The losing informed bidder, who priced honestly on the answer it was given, requests the clarification log and finds its own question missing from it.",
                   "principle":"Asymmetries don't stay quiet — they surface in the bid spread, the variations, or the challenge; the repair is only cheap on the day it happens."},
                  {"key":"retract","label":"Ask the engineer to retract the answer — tell the bidder to rely on the published scope only, restoring equality by subtraction","quality":25,
                   "consequence":"The retraction restores formal equality and destroys real information: the training question was genuine and the scope genuinely silent; five bidders now price five different guesses, the spread at opening is chaos, and the ambiguity lands post-award as a dispute with whoever won on the cheapest guess.",
                   "principle":"Equality of ignorance is equality of a kind — but the tender's job was to price a defined scope, and the definition existed the moment the engineer wrote it; circulate it, don't erase it."}]}],
             "hints":["The answer's content is fine — its audience is the defect; fix the audience.",
               "Anonymise, circulate to all, same day: the rules' mechanism exists for exactly this repair.",
               "Six days is enough for everyone to price it; opening day is too late for anyone to."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned one bidder's private answer into everyone's clarification — six days before it became a challenge."}
            """),

        ("WC-QLT-380", "The week the scanner was down", "Seven days of inspections exist on paper. The system says the site was silent.",
            "Aviation", "Assistant Quality Coordinator", "project_controls", "foundation", 6,
            """["quality_management","data_quality"]""",
            """
            {"context":"The airport baggage upgrade records inspections through a QR-scan system: inspect, scan, photo, upload. Preparing the monthly quality report, you find a seven-day hole in March — zero scanned inspections across the whole conveyor package. Asking around, the answer is mundane: the scanner app's certificate expired that week and 'everyone kept paper in the meantime'. The paper exists — a folder of signed inspection sheets — but nobody uploaded them, and the digital record, which is what the client's auditors sample, shows a silent week on a package that was pouring first-stage concrete at the time.",
             "evidence":[
               {"label":"Hole","value":"7 days, zero scanned inspections, whole conveyor package"},
               {"label":"Cause","value":"App certificate expired; crews kept paper"},
               {"label":"Paper","value":"Signed sheets exist in a folder, never uploaded"},
               {"label":"Stakes","value":"Digital record is what auditors sample; concrete poured that week"}],
             "decisions":[
               {"key":"hole","prompt":"You:",
                "options":[
                  {"key":"reconcile","label":"Close the hole honestly and visibly: scan the paper sheets into the system with their true dates AND an upload note explaining the certificate outage (so the record shows late upload, not backdated inspection), cross-check the sheets against that week's pours for completeness, and flag the certificate-expiry failure mode to whoever owns the app — before the auditors find the silence themselves","quality":100,
                   "consequence":"The record shows an honest outage with a complete paper trail behind it — the auditors, sampling March, note the transparent handling as a positive; the cross-check finds one pour whose sheet is genuinely missing, and THAT inspection question gets answered by the batching records while the concrete is still young.",
                   "principle":"A gap in the system of record is closed by uploading the truth with its true chronology — the outage was innocent; only silence or backdating could make it look otherwise."},
                  {"key":"leave","label":"Leave it — the paper exists if anyone asks, the outage wasn't your fault, and stirring the file draws audit attention to a week that was actually fine","quality":0,
                   "consequence":"The auditors' sample lands on the silent week as silent weeks attract samples; the paper folder is produced under pressure, the one genuinely-missing sheet is found by them instead of you, and 'the site knew and left it' becomes the finding.",
                   "principle":"A known gap left unclosed converts an IT outage into an integrity question — records problems age like concrete, not like wine."},
                  {"key":"quiet_dates","label":"Upload the sheets with upload-date timestamps only — closing the gap without drawing attention to the outage with explanatory notes","quality":20,
                   "consequence":"The record now shows seven days of inspections apparently performed in one afternoon in April; the auditor's timeline query lands on exactly the ambiguity the honest note would have dissolved, and the explanation given late sounds like the excuse it never needed to be.",
                   "principle":"Half-transparent is its own red flag — records repair cleanly only when the repair explains itself."}]}],
             "hints":["The paper is the evidence; the upload with true chronology and an outage note is the repair.",
               "Cross-check sheets against that week's pours — the honest audit of your own gap finds what matters.",
               "Report the certificate failure mode upward: this outage will otherwise repeat on renewal day."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed a seven-day hole in the inspection record with the truth and a timestamp — before the auditors sampled the silence."}
            """),

        ("WC-AIA-381", "The projection nobody can explain", "The demand forecast drives the whole cutover plan. Its author left no trail.",
            "Public Sector Technology", "Junior Programme Analyst", "project_controls", "foundation", 7,
            """["ai_assurance","evidence_analysis"]""",
            """
            {"context":"The census-scale IT programme's commissioning plan sizes its cutover support — helpdesk seats, floor-walkers, overtime budget — on a demand projection produced by a forecasting notebook a contractor data scientist built before rolling off. The projection says week-one call volumes will be 'manageable with 60 seats'. Preparing the commissioning pack, you try to trace the number: the notebook is on a shared drive, undocumented, and references a training dataset from a different department's rollout four years ago — smaller population, different demographic, mandatory rather than voluntary interaction. Nobody currently on the programme can run the notebook. The 60-seat plan is already in the operator's mobilisation contract.",
             "evidence":[
               {"label":"Number","value":"'60 seats manageable' — drives helpdesk, floor-walkers, overtime"},
               {"label":"Provenance","value":"Undocumented notebook; author rolled off; nobody can run it"},
               {"label":"Training data","value":"Different department, smaller population, mandatory vs voluntary — 4 years old"},
               {"label":"Status","value":"60 seats already contracted"}],
             "decisions":[
               {"key":"provenance","prompt":"You:",
                "options":[
                  {"key":"triangulate","label":"Flag the provenance gap with a cheap triangulation attached: benchmark week-one contact rates from two comparable public rollouts (published post-implementation reviews exist), scale to this programme's population, and present the range beside the orphan 60 — if the range brackets 60, the plan holds with evidence; if it doesn't, the mobilisation contract's flex options need exercising now, while notice periods still allow it","quality":100,
                   "consequence":"The benchmarks say 85–110 seats for a voluntary-interaction population this size; the contract's flex clause — exercisable with six weeks' notice, which exists — takes mobilisation to 90, and week one peaks at 96 with floor-walkers absorbing the rest. The orphan notebook is archived with a warning label.",
                   "principle":"An unexplainable number that sizes real contracts gets triangulated, not trusted or merely doubted — two independent benchmarks beat one orphan model, and the check costs an afternoon."},
                  {"key":"trust","label":"Leave it — the projection was produced by a specialist with the tools for it, the contract is signed, and re-opening seat numbers now costs money on a hunch","quality":0,
                   "consequence":"Week one opens at 60 seats against demand the benchmarks would have predicted; hold times pass twenty minutes by Tuesday, the programme makes the evening news, and emergency seats cost four times the flex-clause rate — the notebook is finally examined, by the inquiry.",
                   "principle":"'A specialist built it' is provenance, not validation — the model's training data was the wrong world, and nobody who could have said so was ever asked."},
                  {"key":"rebuild","label":"Commission a proper demand model — new data, documented method, run by the programme's own analysts before commissioning locks","quality":30,
                   "consequence":"The proper model is proper and slow: procurement of analytical support takes five weeks, the model lands after the mobilisation contract's flex-notice window, and its answer — 90 seats — arrives as an unaffordable truth the benchmark triangulation would have delivered inside the window for free.",
                   "principle":"The gold-standard rebuild is right for next time; this decision needed an answer inside the contract's notice period — fit the analysis to the window that exists."}]}],
             "hints":["Trace the training data before trusting the output — a different population four years ago is a different world.",
               "Triangulate with published comparators: two independent benchmarks beat one orphan notebook, in an afternoon.",
               "Check the mobilisation contract's flex-notice window first — the analysis must land inside it to matter."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Benchmarked an orphan forecast nobody could run — and exercised the seat-flex clause while the notice period still existed."}
            """),

        ("WC-SAF-382", "The carbon line in the closeout", "The concrete saved six weeks. The report needn't mention what it cost.",
            "Life Sciences", "Graduate Sustainability Coordinator", "project_management", "foundation", 5,
            """["sustainability","closeout","reporting_integrity"]""",
            """
            {"context":"Closing out the clinical trials expansion, you compile the sustainability appendix. Mid-project, a programme crisis was solved by switching the laboratory floor slabs from the specified low-carbon mix (GGBS replacement, slower cure) to a rapid-set high-cement mix — saving six weeks, and adding roughly 40% embodied carbon on those elements against the design commitment the planning submission quoted. The switch was properly approved at the time as a programme decision; its carbon consequence was noted in one email and never quantified. The closeout template has an embodied-carbon-versus-target line. Your manager suggests reporting 'in line with design intent, minor material substitutions excepted'.",
             "evidence":[
               {"label":"Event","value":"Slab mix switched: low-carbon → rapid-set; 6 weeks saved"},
               {"label":"Consequence","value":"~40% embodied carbon increase on those elements vs the committed design"},
               {"label":"Paper trail","value":"Programme decision approved; carbon noted in one email, never quantified"},
               {"label":"Suggestion","value":"'In line with design intent, minor substitutions excepted'"}],
             "decisions":[
               {"key":"line","prompt":"You report:",
                "options":[
                  {"key":"quantify","label":"The number, with its story: quantify the substitution's carbon delta against target (the mix data exists), report the line honestly as an exceedance with its cause — a documented programme trade-off — and add the lesson the template exists to harvest: carbon consequences of substitutions should be quantified AT the decision, so future trade-offs are made with both numbers on the table","quality":100,
                   "consequence":"The honest line survives review precisely because it arrives with its cause attached — a decision, not a failure; the lesson lands in the next project's change-control template as a carbon-impact field, and the first substitution it prices gets decided differently.",
                   "principle":"A trade-off properly made deserves to be properly reported — hiding an approved decision's consequence converts good governance into bad accounting."},
                  {"key":"phrase","label":"Use the suggested phrasing — the substitution was approved, 'minor' is arguable for two slab packages, and the closeout is not the venue for reopening a settled programme decision","quality":0,
                   "consequence":"The appendix ships its comfort; the planning authority's post-completion carbon check — a condition of the consent nobody re-read — requests the as-built quantities, and the 40% delta surfaces under someone else's arithmetic with 'design intent' on your appendix beside it.",
                   "principle":"'Excepted' is doing a 40% delta's work in that sentence — closeout language that a quantity check can falsify is not phrasing, it is exposure."},
                  {"key":"defer","label":"Report the line as 'quantification in progress' and leave the analysis for the estates team post-handover","quality":25,
                   "consequence":"Post-handover, no one owns it: the estates team inherits a line item with no data, no budget and no author, and 'in progress' calcifies into the appendix's permanent state — until the consent check finds it first.",
                   "principle":"Deferring a known number to a team that doesn't know the story isn't caution, it is abandonment with a status label."}]}],
             "hints":["The substitution was legitimate — the reporting question is only whether its consequence gets a number.",
               "Check what the planning consent's carbon commitment actually requires at completion.",
               "The lesson worth harvesting: quantify carbon at the decision, not at the closeout."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reported an approved trade-off's real carbon number — and put a carbon field into the next project's change form."}
            """),

        ("WC-GOV-383", "Two mandates, one coastline", "The sponsor chairs the flood board and the growth board. The scheme serves both — differently.",
            "Coastal Resilience", "Assistant Programme Analyst", "project_management", "foundation", 6,
            """["governance","concept_planning"]""",
            """
            {"context":"Preparing the coastal resilience scheme's concept-stage options paper, you notice the sponsoring executive's two roles pulling the appraisal in different directions. As chair of the flood-risk board, she has endorsed option criteria weighted toward protection standard and speed. As chair of the regional growth board, she has separately endorsed criteria — for the same scheme — weighted toward unlocking development land behind the defences. Options that score best on one set score worst on the other. Both endorsements are minuted; the options paper you are drafting must state its appraisal criteria on page one; and your programme manager has told you to 'use whichever set keeps her happy'.",
             "evidence":[
               {"label":"Hat 1","value":"Flood board: criteria weight protection standard + speed"},
               {"label":"Hat 2","value":"Growth board: criteria weight development land unlocked"},
               {"label":"Conflict","value":"Options rank oppositely under the two sets"},
               {"label":"Instruction","value":"'Use whichever set keeps her happy'"}],
             "decisions":[
               {"key":"criteria","prompt":"You draft:",
                "options":[
                  {"key":"expose","label":"A paper that shows both rankings side by side and asks the sponsor — in her sponsor capacity — to settle the weighting as an explicit decision: the criteria conflict is stated neutrally as two endorsed positions needing one owner, with a recommended composite weighting as a starting point, so the appraisal proceeds on a decided basis rather than a diplomatic one","quality":100,
                   "consequence":"The sponsor, shown her own two endorsements colliding, does what conflicted sponsors usually do when asked cleanly: takes the composite to both boards, gets a joint weighting agreed in three weeks, and the options paper lands on criteria nobody can later disown — including her.",
                   "principle":"When one person's two mandates conflict, the analyst's job is to surface the collision as a decision for the person who owns both — papering over it just moves the collision to a more expensive page."},
                  {"key":"happy","label":"Follow the instruction — pick the set that matches her more recent endorsement and note the other informally as 'context'","quality":10,
                   "consequence":"The paper sails through the flood board and detonates at the growth board, where the sponsor — publicly confronted with criteria contradicting her own endorsement there — distances herself from the paper; the appraisal restarts, and 'who chose these criteria' has your initials in the draft history.",
                   "principle":"Choosing between a sponsor's contradictions on her behalf means owning whichever one she disowns — the kindness of 'keeping her happy' lasts until the second board meets."},
                  {"key":"average","label":"Blend the two sets into a composite weighting yourself and present it as the appraisal basis without flagging the conflict","quality":25,
                   "consequence":"The composite is actually reasonable — and unowned: when the ranking it produces disappoints the growth board, no decision-maker ever chose the weights, and the appraisal's foundation turns out to be an analyst's quiet compromise that both boards feel free to reject.",
                   "principle":"A sensible weighting nobody decided is weaker than a rough one somebody owns — analysis can propose the blend; only governance can adopt it."}]}],
             "hints":["Both endorsements are minuted — the conflict is documentary, not interpersonal; present it that way.",
               "Ask the sponsor to decide as sponsor — the one role that owns both of her hats.",
               "Propose the composite as a recommendation; let governance adopt the weights that will govern."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put a sponsor's two colliding mandates on one page — and got the weighting decided before the appraisal spent it."}
            """),

        ("WC-CHG-384", "The re-baseline the stage crew remembers", "New plan, clean slate — and a folder of old dates the producers still quote.",
            "Events & Venues", "Assistant Planner", "project_management", "foundation", 7,
            """["change_control","baseline_management"]""",
            """
            {"context":"The festival build's schedule was re-baselined last month after the main-stage redesign — properly approved, cleanly versioned. Your job now is the weekly look-ahead, and you keep hitting the same problem: half the delivery partners still work to dates from the old plan. The stage rigging company's production manager quotes 'the 14th' for power-on (old baseline; the new one says the 19th); the broadcast compound's fit-out arrives on old dates in two emails this week; and the site logistics tracker someone exported before the re-baseline is circulating as 'the schedule' in at least one contractor's WhatsApp. The re-baseline notice went out — one email, four weeks ago, to the distribution list.",
             "evidence":[
               {"label":"Fact","value":"Re-baseline approved and versioned; power-on moved 14th → 19th"},
               {"label":"Drift","value":"Riggers, broadcast fit-out and a WhatsApp'd export all on old dates"},
               {"label":"Notice","value":"One email, 4 weeks ago, to the list"}],
             "decisions":[
               {"key":"drift","prompt":"You:",
                "options":[
                  {"key":"actively_kill","label":"Treat the old baseline as a live hazard and kill it actively: this week's look-ahead issued with a dated banner ('supersedes all prior versions — key moves: power-on 19th'), direct calls to the three partners you KNOW hold old dates confirming their next milestone verbally, the stray export chased and its holder re-pointed — and the weekly look-ahead becomes the single working document everyone gets, every week, so stale copies age out by routine","quality":100,
                   "consequence":"The rigger's call catches a crew booking already made for the 14th — moved for a phone call this week versus standby costs on site next month; the look-ahead's weekly rhythm does the rest, and by the third issue nobody quotes the old plan because everyone holds a newer page than any export.",
                   "principle":"A re-baseline isn't communicated when the notice is sent — it is communicated when every party's working dates have verifiably changed; old baselines die by replacement rhythm, not by announcement."},
                  {"key":"resend","label":"Re-send the re-baseline notice to the full list, marked important, with the version number bolded","quality":20,
                   "consequence":"The second email performs like the first: read by the people who read the first one, filtered by the people who filtered it — and the rigging crew's booking for the 14th survives to become a standby claim, defended with 'we never saw a new schedule', which is, in their inbox's terms, true enough.",
                   "principle":"A channel that failed once fails twice — drift discovered in specific places needs specific correction, not louder broadcast."},
                  {"key":"blame","label":"Log the partners' use of superseded dates as their contractual risk — the notice was issued; working to old dates is their failure to manage document control","quality":10,
                   "consequence":"Contractually arguable, operationally absurd: being right about whose fault the standby costs are does not stop the standby costs, and the festival's opening night doesn't move for the outcome of the argument.",
                   "principle":"On a fixed-date event, allocating blame for a preventable collision is a hobby — preventing it is the job."}]}],
             "hints":["Assume the notice failed until each partner's actual working dates prove otherwise — verify, don't re-announce.",
               "Call the three you KNOW are stale, this week; the general fix is rhythm, the urgent fix is specific.",
               "Make the weekly look-ahead the only document worth holding — stale exports die of obsolescence, not memos."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Hunted down every copy of a dead baseline before the 14th could invoice the 19th — then let weekly rhythm keep it dead."}
            """),

        ("WC-PRC-385", "Sixty percent of the estimate", "The transformation's lowest bid is a gift, a mistake, or a strategy. Pick before you award.",
            "Enterprise Programmes", "Graduate Commercial Analyst", "project_finance", "foundation", 5,
            """["procurement","commercial_awareness"]""",
            """
            {"context":"The enterprise transformation's change-management partner tender has returned four bids. Three cluster within 10% of each other and of the pre-tender estimate; the fourth is at 60% of the estimate — from a credible mid-size firm. You are asked to draft the evaluation summary. Reading the low bid closely, you notice its resourcing model assumes the client provides 'programme-side change agents in each business unit' — a plausible reading of one ambiguous scope paragraph that the other three bidders priced as the supplier's own staff. The low bid is compliant as written; the assumption is stated, in a table, on page 40.",
             "evidence":[
               {"label":"Spread","value":"Three bids cluster near estimate; one at 60%"},
               {"label":"Found","value":"Low bid assumes CLIENT provides business-unit change agents — page 40, stated"},
               {"label":"Others","value":"Priced the same paragraph as supplier staff"},
               {"label":"Status","value":"Compliant as written; you draft the evaluation summary"}],
             "decisions":[
               {"key":"lowbid","prompt":"Your evaluation summary:",
                "options":[
                  {"key":"surface","label":"Surfaces the divergence as the finding it is: the four bids priced two different readings of one ambiguous paragraph, so the comparison is not yet like-for-like — recommend a clarification to all bidders fixing the intended reading, with re-pricing of that element only, so the evaluation compares one scope; the low bid may still win, but on the same job as everyone else","quality":100,
                   "consequence":"The clarification fixes the reading (supplier provides the agents); the low bid re-prices to 85% of estimate — still lowest, now real — and wins an award that survives both the losing bidders' scrutiny and, more importantly, delivery: the change agents exist, because somebody priced them.",
                   "principle":"A bid far below the cluster has usually priced a different question — find the divergent assumption before comparing numbers, because the award is only as sound as the likeness of what was compared."},
                  {"key":"take_gift","label":"Recommends the low bid as evaluated — it is compliant, the assumption is disclosed in the document, and evaluation rules score what was submitted, not what evaluators wish had been","quality":10,
                   "consequence":"Award lands; month two establishes that no client business unit has budget or headcount for change agents nobody told them to provide, and the 'saving' returns as a variation at the winner's post-award rates — plus a delivery gap while it is negotiated.",
                   "principle":"Compliant-as-written with a divergent assumption is a variation order wearing a discount — the page-40 table was the price of the gap, not the price of the job."},
                  {"key":"mark_down","label":"Recommends scoring the low bid down on risk grounds — the client-side assumption makes it undeliverable as priced","quality":25,
                   "consequence":"The markdown, applied to a compliant bid for an assumption the ambiguous scope permitted, is exactly what procurement challenges are made of; the low bidder's letter arrives within days of the award notice, and the process pauses for a review that the clarification route would never have faced.",
                   "principle":"Punishing a bidder for reading your ambiguity plausibly is evaluating your own scope failure onto their score — fix the ambiguity for everyone; don't score it against someone."}]}],
             "hints":["A far-below-cluster bid priced a different question — hunt the assumption before judging the number.",
               "Page 40's table is legitimate: the ambiguity is the client's own paragraph doing its work.",
               "The lawful repair is a clarification to all with targeted re-pricing — same scope, then compare."],
             "profile_map":{"decision":"Commercial Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Traced a 60% bid to one ambiguous paragraph — and made four bids price the same job before anyone won it."}
            """),

        // ───────────── Reserve · Project Rescue · practitioner · multi-stage ─────────────

        ("WC-RSC-386", "The abutment nobody wants to pour", "Every trade's preferred order starts on the far bank.",
            "River Crossings", "Section Planner", "project_management", "professional", 13,
            """["schedule_analysis","stakeholder_communication"]""",
            """
            {"context":"The river crossing's execution phase has stalled in sequencing argument. The north abutment sits in the flood-warning zone: it can only be poured in the low-river season, which closes in nine weeks. But every subcontractor's preferred sequence starts elsewhere — the piling crew wants the south bank first (easier access, no cofferdam), the formwork contractor wants the pier bases (repetitive learning), and your own site agent wants 'momentum work' first to show progress. The north abutment needs six weeks of preparation before its pour; if it misses this season, it waits ten months.",
             "evidence":[
               {"label":"Constraint","value":"North abutment: low-river season closes in 9 weeks; 6 weeks prep needed"},
               {"label":"Preferences","value":"Piling: south bank. Formwork: pier bases. Site agent: 'momentum work'"},
               {"label":"Miss cost","value":"10-month wait for the next season"}],
             "decisions":[
               {"key":"sequence","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"window_first","label":"Rule the sequence by the window: north abutment preparation starts this week — the nine-week season minus six weeks prep leaves three weeks of float, which is the project's scarcest resource — and the preference arguments get honest answers: everything else can be re-ordered later; the flood window cannot","quality":100,
                   "consequence":"Prep starts with three weeks in hand; a two-week piling delay eats most of it, and the pour lands with four days of season left — tight, and possible only because the window ruled from day one.",
                   "principle":"Sequence disputes are settled by the constraint that doesn't negotiate — seasonal windows outrank access convenience, learning curves and optics, always."},
                  {"key":"compromise","label":"Split resources: start the south bank as the piling crew prefers while a second small crew begins abutment prep in parallel","quality":25,
                   "consequence":"The 'small crew' proves too small for cofferdam works; abutment prep runs eight weeks not six, the pour misses the season by days, and ten months of standing cofferdam costs follow the compromise everyone liked.",
                   "principle":"Splitting resources to soften a sequencing decision usually understaffs the activity the constraint needed most."},
                  {"key":"momentum","label":"Take the site agent's advice — early visible progress builds the client confidence that later sequencing battles will need","quality":10,
                   "consequence":"Momentum looks excellent for five weeks; the abutment conversation restarts in week six with the season arithmetic now impossible, and the client's confidence meets a ten-month explanation.",
                   "principle":"Optics bought with a perishable window is the most expensive confidence there is."}]},
               {"key":"protect","prompt":"Stage 2 — protecting the pour date, you:",
                "options":[
                  {"key":"trigger","label":"Build the season plan with dated triggers: a week-by-week countdown with go/no-go checkpoints, weather-window monitoring, and a pre-decided fallback (cofferdam winterisation scope, priced now) if the final checkpoint fails — so a miss, if it comes, costs a planned winterisation instead of an improvised one","quality":100,
                   "consequence":"The pour makes it; the priced winterisation is never used — and cost nothing but a quotation, versus the six-figure improvisation a miss would have forced.",
                   "principle":"A hard window deserves a countdown with pre-decided exits — hope is not a fallback plan."},
                  {"key":"push","label":"Drive the prep hard and handle a miss if it happens — pricing fallbacks now signals doubt to the team","quality":15,
                   "consequence":"The team reads confidence; the river reads the calendar. The near-miss panic of the final fortnight — resolvable because the pour just made it — previews what an actual miss would have cost unpriced.",
                   "principle":"Preparing for failure doesn't cause it — the fallback you price never gets dearer, and the one you don't always does."}]}],
             "hints":["Find the constraint that doesn't negotiate — the river's season outranks every preference in the room.",
               "Count backwards: nine weeks minus six weeks prep is the project's real float.",
               "Price the fallback now; a quotation is the cheapest insurance a hard window can buy."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Let the river write the sequence three trades wanted to vote on — and poured with four days of season to spare."}
            """),

        ("WC-RSC-387", "The month the forecast forgave", "Energisation is close, the numbers are kind, and every kindness leans the same way.",
            "Energy Networks", "Package Cost Manager", "project_finance", "professional", 18,
            """["cost_management","forecasting","commissioning"]""",
            """
            {"context":"The grid reinforcement project approaches energisation, and this month's cost report — your first since taking the package over — reads suspiciously well. Working the file, you find the kindnesses: the cable contractor's final account is carried at the contractor's own July offer, though correspondence shows they've since signalled two further claims; commissioning contingency has been released to the forecast 'as commissioning is nearly complete' — with the senior authority engineer's witness tests, the phase's riskiest window, still ahead; and an insurance recovery for storm damage is netted in as certain, though the insurer's last letter 'reserves position on the betterment element'. Each treatment has a defensible note. Together they present a package at budget that is more likely 4–6% over.",
             "evidence":[
               {"label":"Final account","value":"Held at contractor's July offer; two further claims signalled since"},
               {"label":"Contingency","value":"Released 'nearly complete' — witness tests still ahead"},
               {"label":"Insurance","value":"Recovery netted as certain; insurer 'reserves position'"},
               {"label":"Net effect","value":"Reported at budget; honest range ~4–6% over"}],
             "decisions":[
               {"key":"report","prompt":"Stage 1 — your first report:",
                "options":[
                  {"key":"recast","label":"Recast at expected values with the inherited treatments named: final account at assessed likely outcome including signalled claims, contingency restored until the witness tests pass, insurance at a risked recovery — and the movement explained as a basis correction on taking over the package, not as deterioration, because that is exactly what it is","quality":100,
                   "consequence":"The report shows the package 5% over with three named drivers and their resolution dates; the project manager, braced for worse once the treatments are laid out, takes the position to the client this month — where it lands as inheritance honestly declared rather than overrun discovered later under your name.",
                   "principle":"The first report after a handover is the one moment a basis correction is free — every month you carry the inherited kindnesses, they become yours."},
                  {"key":"carry","label":"Carry the treatments one more cycle — each is defensible, energisation may resolve the claims and tests favourably, and a new cost manager's first report shouldn't be a bombshell","quality":0,
                   "consequence":"The witness tests find a protection-relay issue, the claims land, the insurer holds its reservation — and the 5% arrives in month three as YOUR deterioration, with the file showing you read the correspondence in month one.",
                   "principle":"Optimism inherited and re-signed is optimism owned — the defensible notes protect their author, not their reader."},
                  {"key":"quiet_fix","label":"Correct the treatments gradually — one per month, so no single report jumps","quality":15,
                   "consequence":"Three months of managed drips reads, in hindsight, as three months of knowing; the client's quantity surveyor plots the movements, asks for the correspondence dates, and the gradualism becomes the finding.",
                   "principle":"Truth released on a schedule is concealment with a payment plan."}]},
               {"key":"forward","prompt":"Stage 2 — you also:",
                "options":[
                  {"key":"basis_note","label":"Attach a standing forecast-basis note to the package: how final accounts, contingency release and recoveries are valued, so every future report is auditable against a stated method — and the commissioning contingency gets a release rule tied to the witness-test milestones, not to the word 'nearly'","quality":100,
                   "consequence":"The relay issue, when it comes, moves the forecast exactly as the basis note's method says it should — no argument, no surprise; the client's QS starts citing your basis note to other packages.",
                   "principle":"A forecast is trustworthy when its method is stated — basis notes convert monthly judgement calls into standing rules that outlive their author."},
                  {"key":"informal","label":"Keep the corrected values and manage the basis informally — method documents invite audits of judgement calls that need room to breathe","quality":20,
                   "consequence":"The next handover — they always come — hands your successor the same archaeology you just did, and the package's forecast credibility resets to zero with each custodian.",
                   "principle":"Undocumented method is personal property; projects need forecasts that survive their forecaster."}]}],
             "hints":["Audit each kind treatment alone, then notice they all lean one way — stacked optimism is a single bias.",
               "The handover month is the free correction — after it, the inheritance is yours.",
               "Tie contingency release to milestones, not adverbs: 'nearly complete' is not a test result."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Recast an inherited forecast's three kindnesses in my first report — while the correction was still free."}
            """),

        ("WC-RSC-388", "Everyone knew about the gateway", "The migration's single point of failure has been on every slide for a year. That was the mitigation.",
            "Technology Programmes", "Delivery Manager", "project_management", "professional", 16,
            """["risk_management","closeout","evidence_analysis"]""",
            """
            {"context":"The platform migration's final cutover is five weeks out, and closing the risk register for the readiness review, you stop at risk 12: 'Legacy authentication gateway — single point of failure during coexistence period'. It has been red for a year. Its mitigation history is a study in ritual: 'monitoring in place' (month 2), 'vendor support contract confirmed' (month 5), 'runbook drafted' (month 8), 'accepted — coexistence period ending soon' (month 11). What has never happened: a test of the failover the runbook describes, or an answer to the question of what actually happens to the 40,000 daily sessions if the gateway fails during the final cutover weekend itself — when both old and new systems depend on it simultaneously.",
             "evidence":[
               {"label":"Risk 12","value":"Legacy auth gateway SPOF — red for a year"},
               {"label":"Mitigation history","value":"Monitoring, support contract, runbook, 'accepted' — sequentially"},
               {"label":"Never done","value":"Failover never tested; cutover-weekend failure mode never analysed"},
               {"label":"Exposure","value":"40,000 daily sessions; both systems depend on it during cutover"}],
             "decisions":[
               {"key":"confront","prompt":"Stage 1 — five weeks out, you:",
                "options":[
                  {"key":"test_now","label":"Convert the ritual into evidence while time exists: schedule a controlled failover test of the gateway in week one (out of hours, rollback-ready), analyse the cutover-weekend failure mode specifically — because 'accepted' was only ever tolerable for normal operations, not for the one weekend both systems hang off it — and let the test's result decide whether cutover proceeds as planned or gains a gateway-specific contingency","quality":100,
                   "consequence":"The test fails — the documented failover has a certificate dependency nobody knew, dead for months — and five weeks is enough: fixed, re-tested, and the cutover runbook gains a gateway watch with a proven recovery. The readiness review hears 'tested' for the first time in the risk's life.",
                   "principle":"A mitigation that has never run is a hypothesis wearing a status — and a SPOF 'accepted' for normal operations is not accepted for the weekend that doubles its load; test while the calendar still sells repairs."},
                  {"key":"accept_history","label":"Close it as accepted — a year of governance reviewed those mitigations, the coexistence period is nearly over, and re-opening a settled risk five weeks out destabilises a review that needs confidence","quality":0,
                   "consequence":"Saturday night, mid-cutover, the gateway drops under the doubled session load; the runbook's failover meets its dead certificate in production, and the recovery — improvised at 3am — takes the exact shape the week-one test would have found, plus nine hours of outage and one career.",
                   "principle":"Settled by review is not the same as settled by test — the register's most dangerous entry is always the red one everyone has stopped reading."},
                  {"key":"paper_up","label":"Strengthen the paper: get the vendor to certify the failover configuration and the architects to sign the coexistence design, so acceptance rests on named professional assurance","quality":20,
                   "consequence":"The vendor certifies the configuration as documented — including, invisibly, the dead certificate, which certification-by-inspection cannot see; the signatures multiply the people wrong about the same thing, and Saturday's failure now has a distribution list.",
                   "principle":"Assurance that inspects documents catches documentation errors — only execution catches execution errors; signatures are not a substitute for a test."}]},
               {"key":"class2","prompt":"Stage 2 — the register habit itself, you:",
                "options":[
                  {"key":"verb_audit","label":"Sweep the register for the same pattern before the review: every red or high risk whose mitigations are all nouns ('monitoring', 'contract', 'runbook') and no verbs ('tested on', 'exercised', 'proven') gets a verification action or an honest re-rating — the review should see which mitigations are evidence and which are furniture","quality":100,
                   "consequence":"The sweep finds two more furniture mitigations — a database restore 'procedure' never rehearsed and an 'agreed' business continuity workaround the business has never heard of; one tests clean, one gets fixed, and the readiness review's risk section is, for once, about risks.",
                   "principle":"Registers rot toward nouns — the audit that matters asks each mitigation when it last actually happened."},
                  {"key":"single","label":"Fix risk 12 and leave the sweep — five weeks before cutover is not the moment for a register-wide excavation","quality":25,
                   "consequence":"Risk 12 is fixed; the unrehearsed database restore procedure gets its first rehearsal during the cutover weekend itself, involuntarily, and mostly works — 'mostly' costing the recovery ninety minutes the sweep would have cost a Tuesday.",
                   "principle":"When one ritual mitigation is found, the finding is the ritual — instances cluster, and the cheapest time to find the siblings is before the weekend they share."}]}],
             "hints":["Read the mitigation history's verbs: monitored, contracted, drafted, accepted — never tested.",
               "The cutover weekend is a different risk from normal operations — both systems on one gateway, doubled load.",
               "Five weeks buys a controlled test and a repair; Saturday 3am buys neither."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Tested a year-old 'accepted' single point of failure five weeks before cutover — and found the dead certificate on a Tuesday."}
            """),

        ("WC-RSC-389", "Faster, said the business case", "The overhaul can be accelerated. The question is whether the case survives the price of it.",
            "Manufacturing", "Project Manager", "project_management", "professional", 14,
            """["change_claims_recovery","concept_planning","cost_management"]""",
            """
            {"context":"The equipment overhaul's business case is in final draft, and the sponsoring operations director wants one change before sanction: compress the outage from six weeks to four, 'because every week down is a week of lost contribution the case itself quantifies'. The case's own numbers make her argument: lost contribution per outage week is large and real. Your delivery analysis makes the counter-argument: compression to four weeks needs double shifts (priced), airfreighted spares (priced), and — the part nobody has priced — abandoning the planned condition-based scope discovery window, meaning the overhaul commits to a fixed scope before the strip-down reveals the machines' actual condition. The last two overhauls in the company's fleet both grew scope materially after strip-down.",
             "evidence":[
               {"label":"Ask","value":"6 weeks → 4: 'every down week is quantified lost contribution'"},
               {"label":"Priced","value":"Double shifts + airfreight — both real, both in the compression estimate"},
               {"label":"Unpriced","value":"Fixed scope committed before strip-down reveals condition"},
               {"label":"History","value":"Last two fleet overhauls: material scope growth after strip-down"}],
             "decisions":[
               {"key":"case","prompt":"Stage 1 — you take the director:",
                "options":[
                  {"key":"price_risk","label":"The whole trade, priced: the compression's visible costs, PLUS the scope-commitment risk quantified from the fleet's own strip-down history (probability of material scope growth, cost of discovering it with the schedule already compressed and the crews already double-shifted) — presented as two honest scenarios so the case chooses between a six-week outage and a four-week bet whose downside the last two overhauls actually rolled","quality":100,
                   "consequence":"The fleet history prices the bet at worse-than-even; the director — argued with her own data rather than resisted — lands on five weeks: compression funded, plus a 72-hour strip-down-first window that lets scope commit AFTER inspection. The case sanctions with the trade visible in it.",
                   "principle":"Acceleration asks are answered with the full price of speed — including the risk the compression silently commits to; the sponsor's own history is the most persuasive actuary available."},
                  {"key":"comply","label":"Update the case to four weeks as directed — the lost-contribution math is hers to weigh, the visible costs are priced, and delivery's job is to deliver the case the sponsor wants","quality":10,
                   "consequence":"Sanctioned at four weeks; the strip-down finds what strip-downs find, the fixed scope meets the machines' actual condition in week two, and the outage runs seven weeks — the worst of both cases, with the double-shift premium spent on top.",
                   "principle":"A case compressed by omission isn't faster, it is unpriced — delivery's silence about the missing line is authorship of it."},
                  {"key":"resist","label":"Hold the case at six weeks and escalate if pressed — the fleet history makes compression professionally indefensible","quality":25,
                   "consequence":"The standoff goes to the sanction board as sponsor-versus-delivery, where sponsors win; four weeks sanctions anyway, now with the delivery team on record as overruled rather than as the authors of the five-week option nobody built.",
                   "principle":"Refusing a sponsor's trade without pricing a better one vacates the chair where the answer should have sat."}]},
               {"key":"window","prompt":"Stage 2 — the 72-hour strip-down window's discipline:",
                "options":[
                  {"key":"pre_decide","label":"Pre-decide the window's outputs: condition categories mapped to scope decisions in advance (what finding triggers what addition, at what pre-agreed price and time), the decision meeting calendared for hour 60 with the director attending — so the window produces committed decisions at speed, not a renegotiation under outage pressure","quality":100,
                   "consequence":"Strip-down finds one category-B condition (bearing housings) — the pre-mapped decision adds four days' scope at the pre-agreed rate in a twenty-minute meeting; the outage completes in five weeks and three days, and the case's arithmetic holds.",
                   "principle":"A discovery window works only if its discoveries have pre-decided consequences — otherwise it is a 72-hour argument scheduled at the worst possible moment."},
                  {"key":"open","label":"Keep the window's response flexible — condition findings vary too much to pre-map, and the team will decide best with real data in hand","quality":20,
                   "consequence":"The real data arrives at hour 50; the 'flexible' response takes four days of meetings, quotes and director's diary — consumed from the outage itself — and the scope decision lands with the compression's savings already spent on deciding.",
                   "principle":"Flexibility at the decision point is latency by another name — the variance belongs in the findings, never in the mechanism."}]}],
             "hints":["Price what the compression silently commits to — fixed scope before inspection is the unpriced line.",
               "The fleet's own strip-down history is the actuary: argue with the sponsor's data, not against her goal.",
               "A middle option usually exists: fund the speed AND keep a decision window — with pre-mapped consequences."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced the bet hiding inside a two-week compression — and built the five-week option the sanction meeting actually needed."}
            """),

        ("WC-RSC-390", "Four hundred vans, one master date", "The fleet plan's milestones were set in a workshop the depots never attended.",
            "Fleet & Logistics", "Programme Planner", "project_management", "professional", 15,
            """["schedule_planning","stakeholder_communication"]""",
            """
            {"context":"The fleet transition's master schedule — vehicle deliveries, charger installations, driver training, diesel disposal across nine depots — was built top-down in head-office workshops and is now, at definition stage, meeting the depots for the first time. The meetings are not going well. Depot managers point out that the plan schedules charger civils during the parcel peak (their yard is full of trailers), driver training in the leave-heavy summer weeks, and the diesel disposal tranche before the replacement vans' delivery dates in two regions — a plan that would briefly leave one depot four vans short of its routes. Head office's programme director is reluctant to 're-open a signed-off baseline for local convenience'.",
             "evidence":[
               {"label":"Plan","value":"Top-down master schedule, signed off, depots never consulted"},
               {"label":"Collisions","value":"Civils in parcel peak; training in leave season; disposal before delivery in 2 regions"},
               {"label":"Consequence","value":"One depot briefly 4 vans short of its routes"},
               {"label":"Resistance","value":"PD: won't 're-open a baseline for local convenience'"}],
             "decisions":[
               {"key":"baseline","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"classify","label":"Separate the depot feedback into constraint violations versus preferences, and re-frame the decision for the PD: the disposal-before-delivery sequence is a plan ERROR (it takes vans off routes — no baseline authority can wish that away), the peak-season civils and leave-season training are feasibility constraints deserving re-phasing, and genuine convenience asks get parked — so the baseline change is scoped, evidenced and small, not 'reopened for local convenience'","quality":100,
                   "consequence":"The PD, shown a four-van route gap rather than a grumble list, approves a targeted change covering the error and the two constraint classes in one governance pass; the depots, seeing their operational facts change the plan, start volunteering constraints the workshops never knew to ask about.",
                   "principle":"Bottom-up feedback lands when it is triaged — errors, constraints, preferences — because a baseline defends itself against convenience but cannot defend itself against arithmetic."},
                  {"key":"hold_line","label":"Support the PD — the baseline is signed, depots always resist central plans, and early concessions invite nine depots' worth of renegotiation","quality":0,
                   "consequence":"The plan executes as signed: the four-van depot hires temporary diesels at spot rates to cover its routes, the peak-season civils crew stands idle behind a yard of trailers, and the 'firm baseline' is re-planned anyway in month four — from a position of demonstrated wrongness.",
                   "principle":"A baseline that contradicts depot physics isn't firm, it is pre-failed — signatures don't move trailers or deliver vans."},
                  {"key":"open_all","label":"Run full re-planning workshops at all nine depots and rebuild the schedule bottom-up — the top-down plan has proven itself unconsultative","quality":25,
                   "consequence":"Nine workshops and a rebuilt schedule take eleven weeks — during which procurement and training bookings float unanchored; the rebuilt plan is better and late, and 80% of it matches the original with the three fixes the triage would have delivered in a fortnight.",
                   "principle":"The cure for an unconsulted plan is targeted correction, not ceremonial reconstruction — most of a wrong plan is usually right."}]},
               {"key":"forward","prompt":"Stage 2 — for the remaining tranches, you:",
                "options":[
                  {"key":"gate_check","label":"Add a depot-feasibility gate to each tranche's mobilisation: eight weeks before a depot's tranche starts, its manager signs the tranche plan against a short constraint checklist (yard capacity by season, leave calendar, route coverage through the swap) — catching the next collision while it is a planning edit","quality":100,
                   "consequence":"Tranche three's gate catches a charger delivery scheduled during a depot's floor-resurfacing closure nobody in head office knew about — moved for free, eight weeks out; the gate's checklist grows one line and the programme stops learning depot facts from depot failures.",
                   "principle":"Central plans meet local reality on a schedule — a standing feasibility gate makes that meeting happen before mobilisation instead of during it."},
                  {"key":"liaison","label":"Appoint a depots liaison to head office so local knowledge flows into planning continuously","quality":30,
                   "consequence":"The liaison is good and singular: nine depots' constraints funnel through one calendar, the parcel-peak collision of tranche five is mentioned in a meeting but reaches no plan, and the role becomes the polite explanation for why it still happened.",
                   "principle":"A person is not a process — continuous liaison without a forcing gate transmits knowledge exactly until it doesn't."}]}],
             "hints":["Triage the pushback: error, constraint, preference — each deserves a different answer.",
               "Lead with the four-van route gap; arithmetic re-opens baselines that grievances cannot.",
               "Install the depot sign-off gate per tranche — local facts, captured eight weeks early, are free."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sorted nine depots' pushback into errors, constraints and asks — and fixed a signed baseline with arithmetic, not workshops."}
            """),

        ("WC-RSC-391", "The pot the trust already spent", "Mobilising the second scheme, you find the first one's contingency paid for it.",
            "Healthcare Estates", "Programme Cost Manager", "project_finance", "professional", 13,
            """["cost_management","governance","procurement_mobilization"]""",
            """
            {"context":"The hospital trust's capital plan runs schemes back-to-back, and mobilising the second scheme — theatres refurbishment — you reconcile its budget and find a hole with a history. The first scheme's overrun was covered, quietly, by 'borrowing' the second scheme's contingency allocation: a capital-plan-level virement, approved at the time as 'to be replenished from year-end flexibility'. Year-end came; the flexibility didn't. The theatres scheme now mobilises with 2% contingency against a norm of 8, its procurement strategy was written assuming the norm, and the trust's capital committee — three members new since the virement — believes the theatres budget is whole. Tender documents go out in three weeks.",
             "evidence":[
               {"label":"Hole","value":"Theatres contingency raided for scheme one's overrun — 'to be replenished'"},
               {"label":"Reality","value":"Never replenished; 2% against an 8% norm"},
               {"label":"Blindness","value":"Capital committee (3 new members) believes the budget whole"},
               {"label":"Clock","value":"Tenders out in 3 weeks"}],
             "decisions":[
               {"key":"disclose","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"paper_now","label":"Put the true position to the capital committee before tenders issue: the virement's history stated factually, the 2% reality against the 8% norm, and the three honest options priced — replenish from the current programme, de-scope the theatres scheme to fit its real cover, or knowingly proceed thin with the risk register carrying the decision — because issuing tenders against a budget the committee believes is whole makes every later discovery a scandal instead of a choice","quality":100,
                   "consequence":"The committee — irritated at the history, grateful for the timing — replenishes half from a slipped scheme's profile and de-scopes one theatre's equipment tranche for the rest; tenders issue two weeks late against a budget that is real, and the virement practice gains a replenishment-or-report rule.",
                   "principle":"A raided budget is survivable; a governing committee that doesn't know it was raided is not — the disclosure is only cheap before commitments are made against the fiction."},
                  {"key":"proceed","label":"Proceed to tender — the virement was properly approved at the time, replenishment remains the plan of record, and stalling the theatres scheme to relitigate old decisions helps no patient","quality":0,
                   "consequence":"Tenders return within budget; the first ground-condition surprise consumes the 2% by month three, and the emergency funding paper must now explain both the surprise AND the two-year-old raid — to a committee learning both from the same document, with your mobilisation sign-off in the file.",
                   "principle":"Proceeding on a budget you know is hollow converts someone else's old decision into your current concealment — the plan of record is not a fact of record."},
                  {"key":"pad","label":"Rebuild the missing cover invisibly — price the risk into the tender documents as client-held provisional sums, restoring protection without a committee confrontation","quality":20,
                   "consequence":"The padded provisional sums inflate every bid; the committee, comparing returns against the (believed-whole) budget, asks why theatres pricing is 6% above benchmark, and the answer unravels the history anyway — now wearing a disguise that looks like manipulation.",
                   "principle":"Hidden repairs to hidden holes compound the concealment — money moved in the dark eventually asks for light at the worst moment."}]},
               {"key":"class3","prompt":"Stage 2 — the practice, you:",
                "options":[
                  {"key":"rule","label":"Propose the capital-plan rule the incident proves necessary: inter-scheme virements carry a funded replenishment plan with a date, appear on the committee's standing report until repaid, and lapse into a formal budget change (with the receiving scheme re-approved at its true budget) if the date passes — so 'borrowing' either repays or becomes honest","quality":100,
                   "consequence":"Two other quiet virements surface in the rule's first standing report — one repays from year-end as promised, one converts to a formal de-scope; the committee's picture of its own programme is true for the first time in three years.",
                   "principle":"Virement is a legitimate tool with an illegitimate default — undated repayment; the rule that forces repay-or-restate keeps the tool and kills the fiction."},
                  {"key":"memo","label":"Circulate a finance memo reminding scheme managers that virements require replenishment plans","quality":20,
                   "consequence":"The memo joins its predecessors; the next crisis borrows the next pot with a replenishment intention as sincere and unfunded as this one's.",
                   "principle":"A reminder about a rule with no enforcement mechanism is a rehearsal of the next exception."}]}],
             "hints":["Reconcile the budget you're mobilising against where its money actually is — not where the plan says it should be.",
               "Disclosure is cheap exactly once: before tenders commit the fiction.",
               "Bring the committee options, not just history — replenish, de-scope, or knowingly thin."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Told the committee its theatres budget was 2% real before tenders made it everyone's problem — with three priced ways out."}
            """),

        ("WC-RSC-392", "The risk that retired with her", "The register says 'managed — see containment plan'. The containment plan says 'ask Priya'.",
            "Life Sciences", "Project Risk Coordinator", "project_controls", "professional", 18,
            """["risk_management","resources_leadership","execution_control"]""",
            """
            {"context":"Mid-execution on the vaccine plant upgrade, a routine register review stops at risk 31: cross-contamination during phased construction beside live production — the project's highest-consequence risk. Status: 'managed — containment strategy in place, see plan'. Following the reference, the containment plan's operational sections repeatedly cite one person: the senior process safety engineer who wrote it — who left the company five weeks ago. Her leaver's handover was a one-hour call. The containment measures still physically exist (barriers, pressure regimes, monitoring), but the judgement layer — what readings mean trouble, when to stop work, how the regime changes as phases advance — lived in her, and the register's 'managed' has quietly become 'was managed, by someone who is gone'.",
             "evidence":[
               {"label":"Risk 31","value":"Cross-contamination beside live production — highest consequence on register"},
               {"label":"Status","value":"'Managed — see containment plan'"},
               {"label":"Reality","value":"Plan's judgement layer personified in an engineer who left 5 weeks ago"},
               {"label":"Handover","value":"One hour, one call"}],
             "decisions":[
               {"key":"orphan","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"rebuild","label":"Re-rate the risk honestly TODAY (the control's operating mind is gone — 'managed' is false until a named successor actually holds it), then rebuild the judgement layer deliberately: a successor engineer appointed with protected time, the departed engineer engaged for paid structured knowledge-capture sessions (leavers usually will), the plan's tacit rules written down as explicit thresholds and phase-change procedures — with an interim conservative operating rule until the successor signs the plan as their own","quality":100,
                   "consequence":"The capture sessions surface two thresholds that existed nowhere on paper — including the pressure-differential reading that had once stopped work for a weekend; the successor signs an explicit plan in six weeks, and phase three's regime change happens under a document instead of a memory.",
                   "principle":"A control whose judgement layer lives in one head is one resignation from fiction — when the head leaves, the honest status is 'unmanaged', and the repair is paid knowledge capture plus a successor who signs, not a plan that cites a ghost."},
                  {"key":"paper_ok","label":"Keep the status — the physical controls exist, the plan is documented, and the site's process safety team provides general coverage; upgrading a register status into a crisis helps nobody","quality":0,
                   "consequence":"Phase three's regime change arrives; the general-coverage engineer follows the plan's written sections, misses the unwritten threshold, and the pressure excursion that follows shuts live production for nine days — the investigation's first finding is the register's 'managed', dated after her leaving date.",
                   "principle":"Barriers and monitors without an operating mind are scenery — a risk register that cites a departed person's plan is documenting its own next incident."},
                  {"key":"consult","label":"Retain the departed engineer on a standing consultancy — she knows the plan, and an on-call arrangement restores the judgement layer at lowest cost","quality":25,
                   "consequence":"The on-call arrangement works until it matters: the phase-three excursion needs judgement in minutes, and she is another company's employee in another city; the retainer had quietly become the reason no successor was ever grown.",
                   "principle":"An on-call ghost is a comfort, not a control — retention arrangements buy transition time, never permanence; the judgement must move house."}]},
               {"key":"class4","prompt":"Stage 2 — the class, you:",
                "options":[
                  {"key":"person_audit","label":"Audit the register for person-shaped controls: every risk whose mitigation cites an individual's judgement, plan or presence gets a named successor, a documentation debt entry, or an honest re-rating — and leaver processes for control-owning roles gain a mandatory structured handover of their risk-relevant knowledge, sized by consequence, not the departing person's notice period","quality":100,
                   "consequence":"The audit finds four more personified controls — two trivial, one documented in an afternoon, one (the legacy steam system's isolation knowledge) triggering a capture programme with its sixty-year-old owner eighteen months before he retires, on his schedule instead of his leaving date's.",
                   "principle":"Every organisation runs on controls that are secretly people — the audit that asks 'who IS this mitigation' finds them while they still have calendars, not leaving dates."},
                  {"key":"hr_fix","label":"Strengthen the leaver checklist — control-owning roles get longer handover requirements in the exit process","quality":25,
                   "consequence":"The next leaver's longer handover is scheduled inside their notice period anyway — four weeks to transfer four years, to a successor recruited after they announced; the checklist improved the ritual, not the outcome.",
                   "principle":"Knowledge transfer sized by notice periods arrives pre-failed — succession starts before resignation or it isn't succession."}]}],
             "hints":["Follow the register's references to their ends — 'see plan' that resolves to 'see person' is the finding.",
               "Re-rate first, rebuild second: the status must be true while the repair runs.",
               "Paid structured capture with the leaver beats a retainer — the goal is moving the judgement, not renting it."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the project's biggest risk was 'managed' by someone who left five weeks ago — and moved the judgement out of her head in six."}
            """),

        ("WC-RSC-393", "Minuted into the megawatts", "The charging rollout's scope grew by committee — now commissioning has to build it.",
            "Climate Infrastructure", "Delivery Manager", "project_management", "professional", 16,
            """["change_claims_recovery","commissioning","governance"]""",
            """
            {"context":"Taking over commissioning of the EV charging rollout's first tranche, you reconcile what the sites are being commissioned AGAINST — and find the answer is: minutes. Through delivery, the rollout's stakeholder forum (councils, the DNO, accessibility groups) generated decisions recorded as agreements: bays widened for accessibility 'at all flagship sites', an extra rapid unit added at two park-and-rides 'in response to demand data', lighting upgraded 'per the safety audit'. None went through change control; the installers built from marked-up drawings; the commissioning checklists, energisation applications and O&M documentation all still describe the ORIGINAL scope. Sites are physically finished to one standard and documented to another — and the DNO's witness inspections, starting in two weeks, will test what the paperwork says against what the wall shows.",
             "evidence":[
               {"label":"Built","value":"Widened bays, extra rapid unit ×2, upgraded lighting — from forum minutes and marked-ups"},
               {"label":"Documented","value":"Checklists, energisation applications, O&M — all original scope"},
               {"label":"Gap","value":"Physical standard ≠ paper standard, tranche-wide"},
               {"label":"Clock","value":"DNO witness inspections in 2 weeks"}],
             "decisions":[
               {"key":"reconcile","prompt":"Stage 1 — you:",
                "options":[
                  {"key":"asbuilt_first","label":"Reconcile paper to wall before the DNO tests either: a rapid as-built survey of the tranche against the marked-ups, energisation applications amended NOW for the sites whose electrical scope changed (the extra rapid units change the DNO's numbers — discovering that at witness stage fails the inspection), commissioning checklists re-issued to the as-built standard, and the scope growth regularised through change control retrospectively but honestly — priced, owned and visible","quality":100,
                   "consequence":"The survey finds the two extra rapids DID change the connection loading at one park-and-ride — the amended application processes in nine working days, one day inside the window; inspections pass against paperwork that describes the actual walls, and the retrospective change file prices the forum's generosity at a number the sponsor needed to see anyway.",
                   "principle":"Commissioning tests the match between record and reality — when scope grew off-book, the survey-and-amend must run BEFORE the inspector arrives, because a witness inspection is the most expensive place to discover your own improvements."},
                  {"key":"inspect_through","label":"Let the inspections proceed on the existing applications — the built standard exceeds the documented one, and inspectors pass better-than-declared work all the time","quality":0,
                   "consequence":"'Exceeds' is not how the DNO reads an extra 150kW unit its application never mentioned: the park-and-ride fails witness inspection on undeclared load, the tranche's energisation programme suspends pending re-application — eight weeks — and the forum's improvements become the reason no site energises this quarter.",
                   "principle":"To an inspector, undocumented betterment is just non-conformance with nicer hardware — connection paperwork tests declarations, not generosity."},
                  {"key":"paper_only","label":"Fast-track the checklist and O&M updates but leave the energisation applications — re-opening DNO paperwork two weeks out invites the delay you're avoiding","quality":15,
                   "consequence":"The checklists pass; the DNO's inspector, comparing nameplate totals against the application at the park-and-ride, finds the discrepancy the amendment would have declared — and an inspector's discovery triggers the full re-application the voluntary amendment would have made a nine-day formality.",
                   "principle":"Selective reconciliation fixes the paperwork you control and leaves the paperwork that controls you — amend toward the regulator first, always."}]},
               {"key":"class5","prompt":"Stage 2 — tranche two's forum still meets monthly. You:",
                "options":[
                  {"key":"price_gate","label":"Wire the forum to change control without silencing it: forum agreements become 'recommendations pending change control', each priced and impact-checked (including connection implications) within ten days, with a standing feedback slot reporting what was adopted, adapted or declined and why — the forum keeps its voice, the programme keeps its record, and the DNO's numbers stay true from the start","quality":100,
                   "consequence":"Tranche two's forum recommends bay widening again — this time it routes, prices, and lands in the drawings, the applications and the budget in the same fortnight; the accessibility group, shown the adopted-with-reasons report, becomes the process's defender rather than its workaround.",
                   "principle":"Stakeholder forums generate scope because nothing prices their agreements in the room — the fix is a fast formal lane, not a slower forum; voices heard through change control stay heard and stay built-as-documented."},
                  {"key":"downgrade","label":"Re-charter the forum as consultative only — no agreements, no minuted decisions, recommendations to the programme board alone","quality":20,
                   "consequence":"The re-chartered forum's councils and accessibility groups, demoted from partners to audience, route their asks through members' planning objections instead — where each one costs more than the change control it was denied.",
                   "principle":"Influence denied a legitimate fast lane finds an expensive slow one — the answer to off-book scope is pricing the book faster, not closing it."}]}],
             "hints":["Find what commissioning will be tested against — then make record and reality match before the inspector does it for you.",
               "The extra rapid units are the urgent thread: connection applications describe loads, and loads changed.",
               "Tranche two's fix is a fast pricing lane for forum agreements — keep the voices, capture the scope."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Surveyed a tranche built from committee minutes and amended the DNO's paperwork nine days before the inspector tested it."}
            """),

        // ───────────── Reserve · Risk Room · practitioner · numeric ─────────────

        ("WC-RSK-394", "The move window and the reserve", "Price the storm before you price the calm.",
            "Corporate Relocations", "Risk Analyst", "project_controls", "professional", 12,
            """["risk_management","numeric_analysis"]""",
            """
            {"context":"The headquarters relocation's final move weekend is booked for late autumn — server hall, trading floor, three hundred desks. Closing out the planning phase, you quantify the five weather-and-logistics risks the move committee has been discussing qualitatively for months, to test whether the retained reserve is honest.",
             "evidence":[
               {"label":"M1 — Storm closes crane window","value":"probability 0.20, impact −350,000"},
               {"label":"M2 — Motorway closure reroutes fleet","value":"probability 0.30, impact −90,000"},
               {"label":"M3 — Server hall cooling fails on cutover","value":"probability 0.10, impact −600,000"},
               {"label":"M4 — Early completion releases hotel block","value":"probability 0.40, impact +50,000"},
               {"label":"M5 — Second lift crew unavailable","value":"probability 0.25, impact −120,000"},
               {"label":"Retained reserve","value":"140,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"M1","probability":0.2,"impact":-350000},{"id":"M2","probability":0.3,"impact":-90000},
               {"id":"M3","probability":0.1,"impact":-600000},{"id":"M4","probability":0.4,"impact":50000},
               {"id":"M5","probability":0.25,"impact":-120000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_M3","label":"EMV of M3 — cooling failure","type":"number"},
               {"key":"emv_M1","label":"EMV of M1 — storm/crane window","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"reserve","prompt":"The reserve conversation with the move committee:",
                "options":[
                  {"key":"gap","label":"Show the net EMV against the retained reserve and recommend closing the gap — part funding, part mitigation spend on the largest single exposures (a standby chiller for M3, a protected crane booking for M1)","quality":100,
                   "consequence":"The committee funds the standby chiller — which converts the register's biggest line into a rental fee — and tops the reserve to the recalculated net; the storm that arrives on the Saturday meets a protected booking.",
                   "principle":"A reserve is honest when it covers the quantified expectation — and mitigation spend that shrinks the expectation is often cheaper than funding it."},
                  {"key":"fund_all","label":"Recommend reserving the worst case — the sum of all negative impacts — because a one-weekend move gets no second chance","quality":30,
                   "consequence":"The worst-case ask is politically dead on arrival and the committee, offered only an extreme, retreats to the existing reserve unchanged — the analysis discredited by its own caution.",
                   "principle":"Worst-case reserving on independent risks prices a conjunction nobody expects — and usually buys a rejection instead of a reserve."},
                  {"key":"asis","label":"Report the EMV as information and leave the reserve decision with the committee — analysts quantify, committees decide","quality":25,
                   "consequence":"The number lands without a recommendation and the meeting moves on; the gap between reserve and expectation survives to the weekend it was calculated to prevent.",
                   "principle":"Quantification without a recommendation is a weather report — the analysis earns its keep when it proposes the response."}]}],
             "hints":["Each line's expected value is probability times impact — mind the one positive line.",
               "Compare the net expectation against the retained reserve before judging either.",
               "The biggest single exposures respond to mitigation spend, not just funding — price both routes."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced five qualitative move-weekend worries into one honest reserve conversation."}
            """),

        ("WC-RSK-395", "The risk nobody would put a number on", "It has been discussed in adjectives for a year. Concept sanction needs arithmetic.",
            "Capital Programmes", "Risk Analyst", "project_controls", "professional", 9,
            """["risk_management","numeric_analysis"]""",
            """
            {"context":"The multi-site capital programme's concept case goes to sanction next month, and one risk has lived in adjectives all year: 'client-side decision delays' — discussed, minuted, never quantified, because quantifying it means putting a number on the sponsor's own governance. You assemble the evidence from the pilot site's approval log and build the register line nobody wanted to write, alongside the three risks everyone was happy to price.",
             "evidence":[
               {"label":"D1 — Client decision delays (from pilot log)","value":"probability 0.60, impact −400,000"},
               {"label":"D2 — Ground conditions at northern sites","value":"probability 0.30, impact −250,000"},
               {"label":"D3 — Market escalation beyond allowance","value":"probability 0.35, impact −300,000"},
               {"label":"D4 — Utility diversions early completion","value":"probability 0.20, impact +80,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"D1","probability":0.6,"impact":-400000},{"id":"D2","probability":0.3,"impact":-250000},
               {"id":"D3","probability":0.35,"impact":-300000},{"id":"D4","probability":0.2,"impact":80000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_D1","label":"EMV of D1 — decision delays","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"present","prompt":"Presenting the register at sanction:",
                "options":[
                  {"key":"data","label":"Present D1 exactly like the others — probability from the pilot's own approval log, impact from its measured delay costs — and let the arithmetic show it is the register's dominant line, with decision service-levels proposed as its mitigation","quality":100,
                   "consequence":"The sponsor argues with the adjective and loses to the log: D1's probability is her own governance's measured record. The case sanctions with decision SLAs attached — the only mitigation that was ever going to work.",
                   "principle":"The unnameable risk is disarmed by sourcing: a probability from the sponsor's own records is not an accusation, it is a citation."},
                  {"key":"bury","label":"Fold D1 into a general 'programme delivery risks' allowance so the number exists without the label","quality":10,
                   "consequence":"The allowance passes unexamined and unmitigated — the money exists but the decision SLAs don't, so the delays happen anyway and consume the allowance that was supposed to be for everything else.",
                   "principle":"A risk hidden inside an allowance is funded but not treated — the label was the mitigation's trigger."},
                  {"key":"omit","label":"Leave D1 qualitative one more cycle — sanction month is the wrong moment to put a number on the sponsor's governance","quality":0,
                   "consequence":"The case sanctions against a register missing its largest line; the delays arrive on the pilot's own schedule, and the year of minutes discussing the risk in adjectives becomes the review's Exhibit A.",
                   "principle":"A risk discussed for a year and never quantified is not undiscovered — it is suppressed, and registers under-count exactly what organisations won't say."}]}],
             "hints":["Source D1's probability and impact from the pilot's approval log — measured, not asserted.",
               "Work each line's expectation and compare: the uncomfortable line is the dominant one.",
               "Propose the mitigation that matches the cause: decision service-levels, not more contingency."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put the sponsor's own approval log behind the risk nobody would name — and it sanctioned with service-levels attached."}
            """),

        ("WC-RSK-396", "Reading a register everyone stopped reading", "Forty lines, four that matter — arithmetic finds them faster than meetings.",
            "Urban Development", "Risk Analyst", "project_controls", "professional", 10,
            """["risk_management","numeric_analysis"]""",
            """
            {"context":"The mixed-use development's definition-stage register has grown to forty lines and lost its audience — the monthly review skims it in four minutes. Asked to make it decision-useful again, you quantify the top of the register properly and discover the ranking everyone assumes (by RAG colour and recency of discussion) is not the ranking the arithmetic supports.",
             "evidence":[
               {"label":"U1 — Rights-of-light claims (much discussed)","value":"probability 0.25, impact −200,000"},
               {"label":"U2 — Basement dewatering variation","value":"probability 0.45, impact −380,000"},
               {"label":"U3 — Pre-let covenant triggers on delay","value":"probability 0.30, impact −500,000"},
               {"label":"U4 — Facade package supplier failure","value":"probability 0.15, impact −700,000"},
               {"label":"Current attention","value":"U1 dominates every meeting; U2–U4 'noted'"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"U1","probability":0.25,"impact":-200000},{"id":"U2","probability":0.45,"impact":-380000},
               {"id":"U3","probability":0.3,"impact":-500000},{"id":"U4","probability":0.15,"impact":-700000}]},
             "ask":[
               {"key":"emv","label":"Net EMV of the four lines","type":"number"},
               {"key":"emv_U2","label":"EMV of U2 — dewatering","type":"number"},
               {"key":"emv_U1","label":"EMV of U1 — rights of light","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"rank","prompt":"Your recommendation to the review:",
                "options":[
                  {"key":"rerank","label":"Re-rank the register by quantified expectation and re-shape the meeting around the top three by EMV — with U1, the meeting's favourite, shown honestly as the smallest of the four — and the forty-line tail summarised as an aggregate line with a threshold trigger","quality":100,
                   "consequence":"The dewatering risk gets the ground investigation it needed two months ago; U1's neighbours settle for a fraction of the meeting time it was consuming — and the review, given a ranked top-three plus an aggregate, reads the register again.",
                   "principle":"Attention follows discussion, not exposure — quantification is how a register wrestles the agenda back from the most interesting risk to the most expensive one."},
                  {"key":"keep_rag","label":"Keep the RAG ranking — the colours encode judgement the arithmetic can't see, and re-ranking by EMV disrespects the workshop consensus","quality":15,
                   "consequence":"The meetings continue debating rights of light; the dewatering variation lands at its measured probability, unmitigated, and costs what the register always said it would — to the month.",
                   "principle":"Judgement the colours encode is judgement the numbers should survive — a ranking that can't face arithmetic is a seating plan, not an assessment."},
                  {"key":"prune","label":"Cut the register to ten lines by deleting the long tail — forty lines is the disease and brevity the cure","quality":30,
                   "consequence":"The register reads better; three deleted tail risks mature in the next quarter, unowned because unlisted, and the pruning is remembered as the month the register stopped being trusted in the other direction.",
                   "principle":"The tail needs aggregation and thresholds, not deletion — risks leave a register by resolution, never by tidying."}]}],
             "hints":["Work the expectation of each line before trusting any colour or any meeting's habits.",
               "Compare the most-discussed line's expectation against the least-discussed — that gap is the finding.",
               "Aggregate the tail with a threshold trigger; forty lines die of neglect, four plus one aggregate get read."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-ranked a forty-line register by arithmetic instead of airtime — and the quiet dewatering risk finally got its investigation."}
            """),

        ("WC-RSK-397", "Drawdown against the risks that remain", "The request is for money the remaining register may still need.",
            "Energy Utilities", "Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","cost_management"]""",
            """
            {"context":"Mobilising the substation replacement's main works, the enabling contractor requests a contingency drawdown of 180,000 for asbestos found in the decommissioned control building — genuine, evidenced, payable. Before approving, you re-price what contingency must still cover: the three live risks that survive into the main works.",
             "evidence":[
               {"label":"Drawdown request","value":"180,000 — asbestos, evidenced"},
               {"label":"S1 — Outage overrun into winter constraints","value":"probability 0.30, impact −450,000"},
               {"label":"S2 — Cable route obstruction","value":"probability 0.25, impact −160,000"},
               {"label":"S3 — Protection settings rework at commissioning","value":"probability 0.20, impact −90,000"},
               {"label":"Contingency remaining before drawdown","value":"400,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"S1","probability":0.3,"impact":-450000},{"id":"S2","probability":0.25,"impact":-160000},
               {"id":"S3","probability":0.2,"impact":-90000}]},
             "ask":[
               {"key":"emv","label":"EMV of the remaining register","type":"number"},
               {"key":"emv_S1","label":"EMV of S1 — outage overrun","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"drawdown","prompt":"Your recommendation:",
                "options":[
                  {"key":"pay_flag","label":"Approve the drawdown — it is evidenced — and report in the same paper that the post-drawdown balance now sits below the remaining register's expectation, with the gap named and options tabled (top-up, or funded mitigation of S1's winter exposure)","quality":100,
                   "consequence":"The drawdown pays without drama and the gap gets decided in daylight: the project funds an early outage-planning study that cuts S1's probability, and the balance ends the month smaller and sufficient.",
                   "principle":"A drawdown decision is two questions — is the claim good, and what must the pot still cover; answering only the first is how contingency dies before the risks do."},
                  {"key":"pay_only","label":"Approve and move on — the claim is legitimate and the register's expectation is a forecast, not an invoice","quality":15,
                   "consequence":"The pot pays the asbestos; the winter outage overrun arrives at its measured probability, finds the balance short, and the emergency funding paper must explain why nobody compared the pot to the register in month one.",
                   "principle":"Legitimacy of the claim says nothing about sufficiency of what remains — the comparison is the analyst's whole job."},
                  {"key":"refuse","label":"Refuse the drawdown to protect the balance against the remaining register","quality":10,
                   "consequence":"An evidenced claim refused sours the enabling contractor's cooperation on the interfaces the main works depend on — and the refusal protected a balance the comparison would have shown needed restructuring, not hoarding.",
                   "principle":"Protecting a pot by refusing valid claims treats the symptom — the gap between pot and register needs governance, not gatekeeping."}]}],
             "hints":["Two separate questions: the claim's validity, and the remaining register's expectation against the balance.",
               "Work the remaining lines' expectation before looking at the request.",
               "If a gap opens, table it with options the same day — top-up or mitigation, decided in daylight."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Paid a valid claim and priced what the pot still owed the register — in the same paper."}
            """),

        ("WC-RSK-398", "The delay with a silver lining", "Quantify the upside before the window closes on it.",
            "Data Centres", "Risk Analyst", "project_controls", "professional", 11,
            """["risk_management","commercial_awareness"]""",
            """
            {"context":"The data-centre build has taken an eight-week utility delay, and the risk process — properly — is re-pricing the threats it creates. But walking the updated register, you notice nobody has priced what the delay enables: three genuine opportunities with short windows. You quantify all five movements so the register tells the whole story of the delay, not just its dark half.",
             "evidence":[
               {"label":"T1 — Fit-out compression premium (threat)","value":"probability 0.40, impact −220,000"},
               {"label":"T2 — Standby generation hire extension (threat)","value":"probability 0.70, impact −150,000"},
               {"label":"O1 — Re-tender mechanical package into softer market","value":"probability 0.50, impact +260,000"},
               {"label":"O2 — Early access re-sequencing for commissioning","value":"probability 0.35, impact +120,000"},
               {"label":"O3 — Defer capacity tranche to match demand","value":"probability 0.30, impact +400,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"T1","probability":0.4,"impact":-220000},{"id":"T2","probability":0.7,"impact":-150000},
               {"id":"O1","probability":0.5,"impact":260000},{"id":"O2","probability":0.35,"impact":120000},
               {"id":"O3","probability":0.3,"impact":400000}]},
             "ask":[
               {"key":"emv","label":"Net EMV — threats and opportunities together","type":"number"},
               {"key":"emv_O3","label":"EMV of O3 — capacity deferral","type":"number"},
               {"key":"emv_T2","label":"EMV of T2 — generation hire","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"upside","prompt":"Taking the balanced register to the project board:",
                "options":[
                  {"key":"pursue","label":"Present the net position and recommend funded pursuit of the opportunities on their own windows — the re-tender needs a decision this month, the deferral needs the client's demand data — with owners and dates exactly as the threats get","quality":100,
                   "consequence":"The re-tender lands most of its priced upside; the deferral decision, made with demand data instead of momentum, banks the rest. The delay ends net-neutral — an outcome the threats-only register would have called impossible.",
                   "principle":"Opportunities are risks with expiry dates — they deserve the same probability, impact, owner and deadline machinery, or they decay into anecdotes about what might have been."},
                  {"key":"threats_first","label":"Report the threats now and hold the opportunities for a separate paper once the delay's damage is contained","quality":20,
                   "consequence":"The containment work proceeds; the re-tender window — this month — closes while the opportunity paper waits its turn, and the largest upside line expires unexamined.",
                   "principle":"Sequencing opportunities behind threats ignores the one thing they don't share: threats wait for you, windows don't."},
                  {"key":"net_only","label":"Report only the net figure — one number keeps the board conversation simple","quality":15,
                   "consequence":"The single number hides that most of the upside needs active decisions this month; the board notes a tolerable net and acts on nothing, which converts the net to its threats-only value by default.",
                   "principle":"A net of active opportunities and passive threats is not one number — half of it happens by itself, half only happens on purpose."}]}],
             "hints":["Price the enabled opportunities with the same discipline as the created threats.",
               "Mind the signs: the net tells one story, the components tell the decisions.",
               "Each opportunity has a window — attach owners and dates or the arithmetic is a eulogy."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a delay's three silver linings with the same rigour as its threats — and the eight weeks ended net-neutral."}
            """),

        ("WC-RSK-399", "Twenty greens, one shared assumption", "The heat map averages what commissioning is about to correlate.",
            "Advanced Manufacturing", "Risk Analyst", "project_controls", "professional", 12,
            """["risk_management","numeric_analysis"]""",
            """
            {"context":"The production line install enters commissioning with a comfortable heat map: the four commissioning risks are individually modest. Reviewing the methodology, you find all four assume the vendor's control software build is stable — the same build, the same assumption, four times. The vendor shipped a major patch last week. You price the register both ways: as scored, and with the correlation made explicit — if the software assumption fails, all four fire together.",
             "evidence":[
               {"label":"C1 — Sensor calibration drift","value":"probability 0.20, impact −110,000"},
               {"label":"C2 — Conveyor handshake faults","value":"probability 0.25, impact −140,000"},
               {"label":"C3 — Batch recipe validation rework","value":"probability 0.20, impact −200,000"},
               {"label":"C4 — Performance test re-runs","value":"probability 0.25, impact −180,000"},
               {"label":"Shared assumption","value":"All four scored assuming a stable control build — patched last week"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"C1","probability":0.2,"impact":-110000},{"id":"C2","probability":0.25,"impact":-140000},
               {"id":"C3","probability":0.2,"impact":-200000},{"id":"C4","probability":0.25,"impact":-180000}]},
             "ask":[
               {"key":"emv","label":"Register EMV as scored (independent)","type":"number"},
               {"key":"emv_C3","label":"EMV of C3 — recipe validation","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"correlation","prompt":"Your report to the commissioning readiness meeting:",
                "options":[
                  {"key":"expose","label":"Report the as-scored expectation AND the correlated scenario beside it: if the patched build is unstable, the four risks are one event with the summed impact — so the cheap test is a soak-test of the new build on the vendor's rig before commissioning starts, converting the shared assumption into a verified fact","quality":100,
                   "consequence":"The soak test finds two handshake regressions in the patch — fixed on the rig in four days, where the correlated scenario would have found them across all four risk lines simultaneously, on the line, in commissioning week.",
                   "principle":"Independence is an assumption with a price — when four scores share one premise, the register's real exposure is the premise, and testing it directly is almost always cheaper than funding the correlation."},
                  {"key":"as_scored","label":"Report as scored — the patch passed the vendor's own regression suite and re-scoring on suspicion punishes them for maintaining their software","quality":10,
                   "consequence":"The vendor's suite, it turns out, doesn't cover your line's conveyor topology; the handshake faults fire C2, then C3 and C4 as the build's instability cascades, and the 'independent' register pays its correlated worst month.",
                   "principle":"A vendor's own regression suite tests the vendor's assumptions — the shared premise under YOUR register needs your test."},
                  {"key":"inflate","label":"Raise all four probabilities to reflect the patch uncertainty and re-issue the heat map","quality":30,
                   "consequence":"The inflated map buys contingency for the correlation without testing the premise; the money sits ready to pay for failures a four-day soak test would have prevented, and the readiness meeting reads redder without being wiser.",
                   "principle":"Funding a correlated exposure you could verify away is insurance where engineering was on offer."}]}],
             "hints":["Ask what the four scores share before trusting any of them separately.",
               "Work the as-scored expectation, then imagine the shared premise failing — the delta is the real finding.",
               "The cheap response to a shared assumption is a direct test of it, before the phase that correlates everything."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a heat map's hidden correlation and bought a four-day soak test instead of a correlated commissioning month."}
            """),

        // ───────────── Reserve · Schedule Strategy · practitioner · numeric ─────────────

        ("WC-RES-400", "What the overtime actually bought", "Three months of extended shifts. Weigh the progress before praising the hours.",
            "Transport Depots", "Project Controls Analyst", "project_controls", "professional", 9,
            """["progress_measurement","resource_management"]""",
            """
            {"context":"Closing out the depot modernisation, you audit what the final quarter's blanket overtime actually achieved. The recovery narrative says 'the extended shifts brought the job home'. The work-package data says something more specific: compute the true budget-weighted physical progress at the point overtime started versus what the narrative claimed, and let the closeout record the arithmetic.",
             "evidence":[
               {"label":"WP1 — Trackwork","value":"weight 1,800,000 · percent complete at overtime start: 88"},
               {"label":"WP2 — Maintenance shed M&E","value":"weight 1,200,000 · percent complete: 64"},
               {"label":"WP3 — Stabling sidings","value":"weight 600,000 · percent complete: 95"},
               {"label":"WP4 — Control systems","value":"weight 400,000 · percent complete: 40"},
               {"label":"Narrative at the time","value":"'Project is about 85% done — a push gets us over the line'"}],
             "task":"progress",
             "given":{"nodes":[
               {"id":"WP1","name":"Trackwork","weight":1800000,"percent":88},
               {"id":"WP2","name":"Shed M&E","weight":1200000,"percent":64},
               {"id":"WP3","name":"Sidings","weight":600000,"percent":95},
               {"id":"WP4","name":"Control systems","weight":400000,"percent":40}]},
             "ask":[{"key":"overall_percent","label":"Budget-weighted physical progress (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"lesson","prompt":"The closeout lesson you write:",
                "options":[
                  {"key":"measure","label":"Record that the overtime decision was made against an eyeballed 85% when the weighted figure was materially lower and concentrated in two packages — so the lesson is measurement discipline before resource decisions, with blanket overtime replaced by package-targeted intervention next time","quality":100,
                   "consequence":"The lesson survives challenge because it is arithmetic, not opinion; the framework's next recovery decision starts with a weighted progress cut by package, and the overtime bill that follows is half this one's size.",
                   "principle":"Overall percent-complete is a weighted fact, not a felt one — resource decisions made against a guessed number buy effort where the eye lands, not where the work remains."},
                  {"key":"praise","label":"Record that the overtime achieved completion and recommend it as a proven recovery lever for the framework","quality":10,
                   "consequence":"The lesson canonises the most expensive tool in the box; the next project reaches for blanket overtime at the first slip and skips the measurement that would have shown where the gap actually lived.",
                   "principle":"A recovery that worked expensively teaches nothing unless the record shows what a measured alternative would have cost."},
                  {"key":"neutral","label":"Record the completion date and costs without judgement — closeouts document, they don't editorialise","quality":25,
                   "consequence":"The numbers file without their meaning; the next recovery debate quotes this project both ways, and the arithmetic that could have settled it stays uncomputed in an appendix.",
                   "principle":"A closeout that declines to interpret its own data exports the analysis to whoever argues loudest later."}]}],
             "hints":["Weight each package's percent by its budget share before trusting any overall figure.",
               "Compare the weighted result with the number the decision was actually made on.",
               "Aim the lesson at the decision process — measured progress before resource spend."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Re-weighed the progress claim behind a quarter of blanket overtime — and wrote the lesson in arithmetic."}
            """),

        ("WC-SCH-401", "The trend the business case ignores", "The draft case assumes delivery pace the pilot never achieved. Read the earned schedule.",
            "Justice Estates", "Programme Analyst", "project_controls", "professional", 10,
            """["schedule_analysis","forecasting"]""",
            """
            {"context":"The courts modernisation programme's full business case assumes the national rollout runs at the plan's pace. The pilot tranche just finished month 4 of its planned 6, and you run the earned-schedule numbers to test the assumption before the case commits to it. The pilot's plan and earned value are below; the draft case's rollout schedule assumes SPI(t) of 1.0.",
             "evidence":[
               {"label":"Planned duration","value":"6 months"},
               {"label":"Actual time (AT)","value":"month 4"},
               {"label":"Earned value to date","value":"430"},
               {"label":"Cumulative plan","value":"m1: 90 · m2: 210 · m3: 350 · m4: 500 · m5: 640 · m6: 760"},
               {"label":"Draft case","value":"Rollout schedule assumes plan pace throughout"}],
             "task":"earned_schedule",
             "given":{"planned_duration":6,"at":4,"ev":430,
               "plan":[{"period":1,"pv":90},{"period":2,"pv":210},{"period":3,"pv":350},{"period":4,"pv":500},
                       {"period":5,"pv":640},{"period":6,"pv":760}]},
             "ask":[
               {"key":"es","label":"Earned Schedule (months)","type":"number"},
               {"key":"sv_time","label":"Time variance SV(t)","type":"number"},
               {"key":"spi_time","label":"Time index SPI(t)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"case","prompt":"Your note to the case's author:",
                "options":[
                  {"key":"evidence_pace","label":"Show the pilot's measured time index and recommend the rollout schedule be built on demonstrated pace — with the assumed-pace version kept as an upside scenario contingent on named fixes for the pilot's slippage causes","quality":100,
                   "consequence":"The case re-times on measured pace and survives its review; the named fixes (approval latency, access windows) get funded because the gap between demonstrated and assumed pace finally has a price on it.",
                   "principle":"A business case that assumes the pace its own pilot disproved is not optimistic, it is unfounded — demonstrated SPI(t) is the only rollout speed the evidence owns."},
                  {"key":"assume","label":"Leave the case at plan pace — pilots always run slow, the rollout teams will have the learning curve behind them","quality":10,
                   "consequence":"The rollout inherits the pilot's causes without its lessons priced; by tranche two the programme is re-baselining publicly, and the case's pace assumption is the review's first finding.",
                   "principle":"'The next phase will be faster' is a claim that needs a mechanism — learning curves are earned by fixing causes, not by asserting them."},
                  {"key":"pad","label":"Add a blanket schedule contingency to the case instead of re-timing it — protection without a confrontation over the assumption","quality":25,
                   "consequence":"The padded case passes and the padding is consumed by the exact slippage the index predicted — with no fix funded, because the pad hid the gap the fixes needed to justify themselves.",
                   "principle":"Contingency spent concealing a measured trend buys time to not fix it."}]}],
             "hints":["Read ES off the cumulative plan: the month the plan expected today's earned value.",
               "SV(t) and SPI(t) compare that against actual months elapsed — time against time.",
               "Build the rollout on the measured index; keep plan pace as an upside with named, funded fixes."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Tested a business case's pace assumption against its own pilot's earned schedule — and re-timed it on evidence."}
            """),

        ("WC-RES-402", "Staff the ward build before you promise it", "Three hiring stages, three estimates each — the definition plan needs one honest number.",
            "Life Sciences Construction", "Planning Analyst", "project_controls", "professional", 8,
            """["resource_management","estimating"]""",
            """
            {"context":"The sterile facility build's definition plan assumes the specialist cleanroom crew is 'onboarded by month 5'. Nobody has estimated the onboarding chain properly: recruit the supervisors, clear site inductions and GMP training, then run the supervised probation the pharma client requires. You collect optimistic, most-likely and pessimistic durations for each stage from the two contractors who have done it before, and run the numbers against the plan's month-5 promise (22 weeks).",
             "evidence":[
               {"label":"H1 — Recruit supervisors","value":"optimistic 4 · most likely 7 · pessimistic 14 (weeks)"},
               {"label":"H2 — Inductions + GMP training","value":"optimistic 3 · most likely 5 · pessimistic 9"},
               {"label":"H3 — Supervised probation","value":"optimistic 6 · most likely 8 · pessimistic 12"},
               {"label":"Plan assumption","value":"Crew ready by week 22"}],
             "task":"pert",
             "given":{"activities":[
               {"id":"H1","o":4,"m":7,"p":14},{"id":"H2","o":3,"m":5,"p":9},{"id":"H3","o":6,"m":8,"p":12}],
               "deadline":22},
             "ask":[
               {"key":"expected_duration","label":"PERT expected chain duration (weeks)","type":"number"},
               {"key":"std_dev","label":"Chain standard deviation (weeks)","type":"number"},
               {"key":"prob_on_time","label":"Probability of ready by week 22 (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"plan","prompt":"Your recommendation to the definition team:",
                "options":[
                  {"key":"start_early","label":"Report the expected duration and the on-time probability honestly, and recommend starting the recruiting stage weeks earlier rather than re-dating the milestone — the chain's variance is dominated by recruitment, which is the one stage the project can start ahead of need","quality":100,
                   "consequence":"Recruitment opens the week the recommendation lands; the chain completes with margin against week 22, and the definition plan's promise becomes a fact instead of a hope with a distribution.",
                   "principle":"When a chain's expected duration threatens a promise, the cheapest fix is starting the longest-variance stage earlier — probability improves by calendar, not by optimism."},
                  {"key":"accept","label":"Keep the plan at week 22 — the most-likely estimates sum comfortably inside it and the pessimistic cases are precisely pessimistic","quality":10,
                   "consequence":"The most-likely world fails to occur, as it does roughly half the time; the crew readies late, the pharma client's witness schedule re-books at eight weeks' notice, and the 'comfortable' sum is quoted at the review.",
                   "principle":"Summing most-likelies ignores the asymmetry the estimates themselves declare — expected values and variances exist because 'probably fine' is not a plan."},
                  {"key":"pad_date","label":"Move the milestone to a very safe week and re-plan everything downstream of it","quality":25,
                   "consequence":"The safe date cascades through the downstream plan, lengthening the programme for a risk that starting recruitment early would have absorbed for free — safety bought at schedule prices.",
                   "principle":"Re-dating a milestone is the expensive response to variance when the chain's front end can simply start sooner."}]}],
             "hints":["Each stage's expected value weights the most-likely fourfold; variances add along the chain.",
               "Compare the chain's distribution against week 22 — the probability is the honest answer to 'are we fine'.",
               "The fix lives at the front: recruitment can start before the plan needs it; probation cannot compress."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ran the onboarding chain's real distribution against a month-5 promise — and bought the probability with an earlier start, not a later date."}
            """),

        ("WC-SCH-403", "The handover network behind the promise", "Two dates have died. Build the logic before announcing a third.",
            "Renewable Energy", "Planning Analyst", "project_controls", "professional", 11,
            """["schedule_analysis","commissioning"]""",
            """
            {"context":"The solar portfolio's first site has missed two announced handover dates — both set by negotiation rather than network. Before the third is announced, you build the actual logic from grid witness to portfolio handover and run it. The mobilisation board wants to promise 'three weeks from today'; the network will say what is true.",
             "evidence":[
               {"label":"A — Grid witness re-test","value":"4 days, no predecessors"},
               {"label":"B — SCADA point-to-point completion","value":"6 days, after A"},
               {"label":"C — Civil reinstatement + fencing","value":"9 days, no predecessors"},
               {"label":"D — O&M documentation assembly","value":"5 days, after B"},
               {"label":"E — Client inspection & snag clearance","value":"4 days, after C and D"},
               {"label":"F — Handover certification","value":"2 days, after E"},
               {"label":"Board instinct","value":"'Three weeks from today' (15 working days)"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":4,"preds":[]},{"id":"B","dur":6,"preds":["A"]},{"id":"C","dur":9,"preds":[]},
               {"id":"D","dur":5,"preds":["B"]},{"id":"E","dur":4,"preds":["C","D"]},{"id":"F","dur":2,"preds":["E"]}]},
             "ask":[
               {"key":"project_duration","label":"Network duration (working days)","type":"number"},
               {"key":"float_C","label":"Total float on C — civils (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"announce","prompt":"The date recommendation you take to the board:",
                "options":[
                  {"key":"network_date","label":"Announce the network's date, not the instinct's — with the driving path named (witness → SCADA → documentation → inspection) so every acceleration conversation targets the activities that actually set the date, and the civils' float declared so nobody wastes money accelerating it","quality":100,
                   "consequence":"The third announced date is the first one built on logic; it holds, the client's confidence begins its slow repair, and the one acceleration the board buys — parallel documentation assembly — comes off the driving path because the network showed where to spend.",
                   "principle":"After broken promises, the only credible date is one a network produced — and the path that drives it is the only place acceleration money is real."},
                  {"key":"promise_15","label":"Back the board's three weeks — the team responds to stretch targets and the network's tail activities usually compress in practice","quality":0,
                   "consequence":"The third date dies like the first two, but publicly and with a network on file showing it was never achievable; the client stops accepting dates from the board at all and imposes its own completion audit.",
                   "principle":"A stretch target set below the network's arithmetic is not motivation, it is the next broken promise with better intentions."},
                  {"key":"pad_5","label":"Announce the network date plus a five-day buffer — after two misses, only an unmissable date will do","quality":30,
                   "consequence":"The padded date holds trivially and costs standing time across the demobilising site; the client's commercial team, comparing effort to calendar, quietly re-prices the relationship's trust either way.",
                   "principle":"Padding rebuilds certainty at the price of pace — after a network exists, buffer belongs on named risks, not smeared across the whole promise."}]}],
             "hints":["Trace both entry paths to E — the longer one drives the date.",
               "C's float is the gap between its finish and when E actually needs it.",
               "Name the driving path in the announcement; acceleration off-path is money spent on nothing."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Built the network behind a third handover promise — and announced the first date the logic could defend."}
            """),

        ("WC-RES-404", "One team, three ballrooms", "The refit's crews are split across sites. Weigh the real progress before the client walk-through.",
            "Events & Venues", "Project Controls Analyst", "project_controls", "professional", 12,
            """["progress_measurement","resource_management"]""",
            """
            {"context":"The conference-centre refit runs one specialist fit-out team rotating across three halls plus a stores/logistics package. Ahead of Friday's client walk-through, the site managers' verbal updates disagree with each other and with the joinery invoices. You compute the budget-weighted position from the package data so the walk-through opens with one number everybody can stand behind.",
             "evidence":[
               {"label":"P1 — Hall A fit-out","value":"weight 950,000 · percent complete 80"},
               {"label":"P2 — Hall B fit-out","value":"weight 700,000 · percent complete 45"},
               {"label":"P3 — Hall C fit-out","value":"weight 550,000 · percent complete 25"},
               {"label":"P4 — Stores & logistics","value":"weight 300,000 · percent complete 90"},
               {"label":"Verbal claims","value":"'roughly two-thirds done' (site) vs 'barely half' (client's rep)"}],
             "task":"progress",
             "given":{"nodes":[
               {"id":"P1","name":"Hall A","weight":950000,"percent":80},
               {"id":"P2","name":"Hall B","weight":700000,"percent":45},
               {"id":"P3","name":"Hall C","weight":550000,"percent":25},
               {"id":"P4","name":"Stores","weight":300000,"percent":90}]},
             "ask":[{"key":"overall_percent","label":"Budget-weighted progress (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"walkthrough","prompt":"Opening the walk-through, you:",
                "options":[
                  {"key":"weighted","label":"Present the weighted figure with its package breakdown — settling the two verbal claims with arithmetic — and steer the discussion to the crew-rotation decision the breakdown exposes: the team's next fortnight belongs to the heaviest-weight unfinished hall, not the one nearest done","quality":100,
                   "consequence":"The walk-through argument evaporates — both verbal claims were wrong in opposite directions — and the rotation decision moves the crew to Hall B, where each percent is worth the most; Friday ends with a plan instead of a dispute.",
                   "principle":"When site instinct and client instinct disagree, the budget-weighted number is the referee — and its breakdown tells the resource where to stand next."},
                  {"key":"nearest_done","label":"Recommend finishing Hall A first — 'complete something' momentum plays well in walk-throughs and the client sees a finished room","quality":25,
                   "consequence":"Hall A finishes photogenic and early; the weighted position barely moves, the client's rep re-runs the arithmetic in the car park, and the completion theatre reads afterwards as exactly that.",
                   "principle":"Finishing the nearest-done package optimises the photograph, not the progress — percent points are worth their weights."},
                  {"key":"split_diff","label":"Report a figure between the two verbal claims to keep the meeting moving — precision can follow the walk-through","quality":0,
                   "consequence":"The split number satisfies nobody and binds you to it; when the real figure emerges it differs from your diplomatic one, and the question becomes why the controls analyst negotiated arithmetic.",
                   "principle":"A progress number is computed or it is worthless — averaging two guesses produces a third guess with your signature."}]}],
             "hints":["Weight each hall's percent by its budget share — the verbal claims skipped this step in both directions.",
               "The breakdown matters more than the total: where is the largest unfinished weight?",
               "Point the crew at the heaviest remaining package, not the most finishable one."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Settled a two-way progress argument with a budget-weighted number — and pointed the crew where the weight was."}
            """),

        ("WC-SCH-405", "Float, promised to three owners", "Before commissioning starts, find out who actually holds the slack.",
            "Portfolio Management", "Portfolio Planning Analyst", "project_controls", "professional", 9,
            """["schedule_analysis","governance"]""",
            """
            {"context":"The portfolio review before commissioning season, and three project managers have each separately told the review they are 'covered by float' on the shared commissioning-team dependency. You build the season's network once, properly, across the four gating activity chains, and compute where the float actually is — before three projects spend the same slack.",
             "evidence":[
               {"label":"A — Project 1 pre-commissioning","value":"5 days, no predecessors"},
               {"label":"B — Project 1 commissioning slot","value":"4 days, after A"},
               {"label":"C — Project 2 pre-commissioning","value":"8 days, no predecessors"},
               {"label":"D — Project 2 commissioning slot","value":"6 days, after C and B (shared team follows P1)"},
               {"label":"E — Project 3 pre-commissioning","value":"3 days, no predecessors"},
               {"label":"F — Project 3 commissioning slot","value":"5 days, after E and D (team follows P2)"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":5,"preds":[]},{"id":"B","dur":4,"preds":["A"]},{"id":"C","dur":8,"preds":[]},
               {"id":"D","dur":6,"preds":["C","B"]},{"id":"E","dur":3,"preds":[]},{"id":"F","dur":5,"preds":["E","D"]}]},
             "ask":[
               {"key":"project_duration","label":"Season duration (working days)","type":"number"},
               {"key":"float_A","label":"Total float on A — P1 pre-commissioning (days)","type":"number"},
               {"key":"float_E","label":"Total float on E — P3 pre-commissioning (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"season","prompt":"Your report to the portfolio review:",
                "options":[
                  {"key":"one_network","label":"Publish the integrated network with float stated per chain — showing which project's 'we're covered' is true, which is marginal and which is spending slack that belongs to the chain, with the shared commissioning team's sequence governed from the portfolio network rather than three private schedules","quality":100,
                   "consequence":"Two of the three float claims dissolve on contact with the shared-team logic; the season re-sequences once, in the review, instead of three times on site — and the commissioning team stops being triple-promised by people who never saw the same page.",
                   "principle":"Float against a shared resource only exists at the integrated level — three private schedules can all be right and their sum still wrong."},
                  {"key":"trust_pms","label":"Accept the three managers' float positions — they know their projects, and portfolio-level re-planning of project schedules breeds resentment","quality":10,
                   "consequence":"Project 1 slips three days it 'had float for'; the shared team's start on Project 2 slides, Project 3's window compresses against a fixed season end, and the cascade the network would have shown for free arrives at standby rates.",
                   "principle":"Deference to local knowledge fails exactly where the knowledge is local — nobody's private schedule can see the float they share."},
                  {"key":"buffer_team","label":"Add a portfolio buffer to the commissioning team's calendar and leave the project schedules alone","quality":30,
                   "consequence":"The buffer absorbs the first slip and hides the structure; nobody learns which chain is critical, the buffer refills nothing, and next season starts with the same three claims plus a precedent that the portfolio pays for them.",
                   "principle":"Buffering a shared resource without exposing the chain logic subsidises the fiction that caused the collision."}]}],
             "hints":["Build one network: the shared team's sequence links the three projects whether their schedules admit it or not.",
               "Float on each chain is measured against the integrated critical path, not each project's private one.",
               "Publish who holds real slack and who holds a rumour — then govern the team's sequence from the shared page."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Computed where three projects' 'we have float' actually lived — one network, one truth, one re-sequence."}
            """),

        // ───────────── Reserve · Cost & Value · foundation · numeric ─────────────

        ("WC-CHG-406", "Roll only what was approved", "The final account starts with the change register — read the status column first.",
            "Flood Defence", "Assistant Quantity Surveyor", "project_finance", "foundation", 10,
            """["change_control","cost_management"]""",
            """
            {"context":"Closing out the flood defence scheme, you prepare the baseline reconciliation for the final account. The change register lists five changes; the contractor's draft final account has quietly rolled all five onto the baseline, including one still 'proposed' and one formally rejected. Compute the honest revised position — approved changes only — before the draft becomes the negotiation's anchor.",
             "evidence":[
               {"label":"Baseline","value":"BAC 2,400,000 · duration 180 days"},
               {"label":"V1 — Revetment stone upgrade","value":"approved · cost +120,000 · schedule +10"},
               {"label":"V2 — Pump station redesign","value":"approved · cost +85,000 · schedule +6"},
               {"label":"V3 — Access road diversion","value":"rejected · cost +60,000 · schedule +8"},
               {"label":"V4 — Habitat mitigation extension","value":"approved · cost +45,000 · schedule 0"},
               {"label":"V5 — Winter working premium","value":"proposed · cost +150,000 · schedule +15"}],
             "task":"change",
             "given":{"baseline_bac":2400000,"baseline_duration":180,"changes":[
               {"id":"V1","title":"Revetment stone","status":"approved","cost_delta":120000,"schedule_delta":10},
               {"id":"V2","title":"Pump station","status":"approved","cost_delta":85000,"schedule_delta":6},
               {"id":"V3","title":"Access road","status":"rejected","cost_delta":60000,"schedule_delta":8},
               {"id":"V4","title":"Habitat mitigation","status":"approved","cost_delta":45000,"schedule_delta":0},
               {"id":"V5","title":"Winter premium","status":"proposed","cost_delta":150000,"schedule_delta":15}]},
             "ask":[
               {"key":"revised_bac","label":"Revised BAC (approved only)","type":"number"},
               {"key":"revised_duration","label":"Revised duration (days)","type":"number"},
               {"key":"approved_count","label":"Number of approved changes","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"account","prompt":"Responding to the contractor's draft:",
                "options":[
                  {"key":"split","label":"Issue the approved-only reconciliation as the account's baseline, with the proposed item assessed on its merits through the change process and the rejected one answered by reference to its rejection — separate ledgers for separate statuses","quality":100,
                   "consequence":"The negotiation anchors on the honest baseline; the winter premium gets a fair hearing as a change (and partly succeeds on its evidence), while the rejected diversion stays rejected instead of resurrecting inside a total.",
                   "principle":"A final account is arithmetic on statuses — 'approved' is the only column that moves the baseline, and everything else negotiates as itself, not inside the sum."},
                  {"key":"anchor","label":"Negotiate from the contractor's five-change total downward — starting high is how accounts open, and the haggle will find the middle","quality":10,
                   "consequence":"The middle of a padded total is still padded: the rejected item earns a settlement share purely for being included, and the register's statuses turn out to have been advisory.",
                   "principle":"Negotiating from an anchor that includes rejected scope pays for the audacity, not the work."},
                  {"key":"all_out","label":"Reject the draft wholesale until the contractor re-submits on approved changes only","quality":30,
                   "consequence":"Procedurally clean, practically slow: the re-submission takes six weeks the closeout didn't have, for a correction your own reconciliation could have tabled at the first meeting.",
                   "principle":"When you can compute the honest position yourself, table it — the fastest correction of a bad anchor is a good one."}]}],
             "hints":["Filter by status before summing anything — approved moves the baseline; proposed and rejected do not.",
               "Schedule deltas roll the same way: only approved days extend the duration.",
               "Answer the other statuses in their own lanes — merits for proposed, the rejection record for rejected."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rebuilt a padded final account onto its approved-only baseline — and let the other statuses argue as themselves."}
            """),

        ("WC-CST-407", "Two mixes, one honest comparison", "The value engineering pitch quotes a rate. Price the bill before the meeting does.",
            "Energy & Process", "Assistant Cost Engineer", "project_finance", "foundation", 8,
            """["cost_management","value_engineering"]""",
            """
            {"context":"At concept stage for the refinery turnaround's scaffolding strategy, a value engineering pitch claims the alternative system 'roughly halves the access cost'. The claim quotes one headline rate. You price the actual bill of quantities for the alternative before the decision meeting, so the comparison is a total against a total, not a rate against an impression.",
             "evidence":[
               {"label":"SC-1 — System scaffold to columns","value":"qty 2,400 m² · rate 38"},
               {"label":"SC-2 — Suspended access to pipe rack","value":"qty 800 m² · rate 95"},
               {"label":"SC-3 — Mobile towers, exchanger bays","value":"qty 60 uses · rate 420"},
               {"label":"SC-4 — Rope access, flare line","value":"qty 340 hours · rate 88"},
               {"label":"Pitch claim","value":"'Roughly half the access cost of last turnaround'"}],
             "task":"boq",
             "given":{"lines":[
               {"id":"SC-1","qty":2400,"rate":38},{"id":"SC-2","qty":800,"rate":95},
               {"id":"SC-3","qty":60,"rate":420},{"id":"SC-4","qty":340,"rate":88}]},
             "ask":[
               {"key":"total","label":"Alternative system total","type":"number"},
               {"key":"line_count","label":"Number of bill lines","type":"number"},
               {"key":"average_rate","label":"Simple mean of the four rates","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"meeting","prompt":"At the decision meeting:",
                "options":[
                  {"key":"totals","label":"Table the priced bill and compare totals like-for-like against last turnaround's access outturn — with the caveat stated that the simple mean of rates is NOT the job's blended rate, because the cheap lines carry most of the quantity","quality":100,
                   "consequence":"The priced comparison shows a real but far smaller saving than 'roughly half'; the decision proceeds on the honest number, and the pitch's author — to their credit — adopts the bill format for the next proposal.",
                   "principle":"Value claims are settled by totals over the actual quantities — a headline rate is one line's truth wearing the whole job's costume."},
                  {"key":"headline","label":"Accept the pitch's framing — the detailed pricing can follow once the direction is agreed in principle","quality":10,
                   "consequence":"Direction agreed, the detailed pricing later halves the promised saving; but 'agreed in principle' has hardened, and the turnaround budget carries the gap between the impression and the bill.",
                   "principle":"Deciding on an impression and pricing afterwards prices the decision, not the options."},
                  {"key":"avg_rate","label":"Compare the two systems on average rate — one number per system keeps the meeting simple","quality":25,
                   "consequence":"The average flatters whichever system has expensive niche lines and cheap bulk ones; the meeting picks confidently on a statistic that weighs a 60-use tower line equally with 2,400 square metres.",
                   "principle":"An unweighted mean of rates answers a question nobody asked — totals over quantities are the only comparable currency."}]}],
             "hints":["Total is the sum of quantity times rate, line by line.",
               "The mean of the rates ignores quantities — say so when you report it.",
               "Compare total against total, same scope basis, before any 'roughly' survives the meeting."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a 'roughly half' value pitch into an honest bill total — and the decision met the real saving."}
            """),

        ("WC-PRC-408", "The deposit schedule behind the discount", "The long-lead quote is cheaper — and wants your cash a year earlier.",
            "Technology Programmes", "Assistant Commercial Analyst", "project_finance", "foundation", 11,
            """["procurement","cashflow"]""",
            """
            {"context":"The national rollout's definition phase is choosing between network-hardware suppliers, and the cheaper quote carries a stiff payment schedule: heavy deposits against manufacturing slots long before delivery. Before the recommendation is written, you roll the offer's payment profile against the programme's drawdown funding to see what the discount really costs the cash position.",
             "evidence":[
               {"label":"Q1","value":"funding in 400,000 · payments out 900,000 (slot deposits)"},
               {"label":"Q2","value":"funding in 600,000 · payments out 500,000"},
               {"label":"Q3","value":"funding in 800,000 · payments out 300,000 (pre-delivery)"},
               {"label":"Q4","value":"funding in 700,000 · payments out 650,000 (delivery balance)"},
               {"label":"Funding rule","value":"Programme may not exceed its drawn funding at any quarter-end"}],
             "task":"cashflow",
             "given":{"periods":[
               {"period":1,"inflow":400000,"outflow":900000},
               {"period":2,"inflow":600000,"outflow":500000},
               {"period":3,"inflow":800000,"outflow":300000},
               {"period":4,"inflow":700000,"outflow":650000}]},
             "ask":[
               {"key":"final_position","label":"Closing cumulative position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"},
               {"key":"cumulative_2","label":"Cumulative position after Q2","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"recommend","prompt":"Your note on the cheaper quote:",
                "options":[
                  {"key":"price_cash","label":"Report that the offer breaches the funding rule in its first quarter and price the fix into the comparison — either the funding profile advances (a treasury conversation with a cost) or the payment schedule renegotiates (a supplier conversation with a price) — so the two quotes compare on total cost including money","quality":100,
                   "consequence":"The supplier, asked to re-profile, trades half the deposit for a small price adjustment; the quote stays cheapest by a thinner honest margin, and the recommendation survives finance review because the cash question arrives answered.",
                   "principle":"A discount with a deposit schedule is two prices — the invoice and the cash curve; only the pair can be compared, and the funding rule is part of the arithmetic."},
                  {"key":"cheapest","label":"Recommend the cheaper quote on price and flag the payment schedule as an implementation detail for finance to manage","quality":10,
                   "consequence":"Finance discovers the quarter-one breach after award, when the leverage to re-profile has been signed away; the emergency funding advance costs more than the discount, in the exact currency the recommendation ignored.",
                   "principle":"'Finance will manage it' after award means paying rack rate for the flexibility you gave up at recommendation."},
                  {"key":"safe","label":"Recommend the dearer quote — its gentler schedule fits the funding envelope and cash certainty is worth the premium","quality":30,
                   "consequence":"Safe and unexamined: the cheaper supplier would have re-profiled for a fraction of the premium — nobody asked — and the programme pays the full difference for a conversation that never happened.",
                   "principle":"Rejecting a fixable breach at full premium prices the problem, not the fix — ask before you pay."}]}],
             "hints":["Roll the quarters cumulatively; the funding rule is tested at each quarter-end, not at the end of the year.",
               "Peak funding is the deepest cumulative deficit, read as a positive number.",
               "The breach has two fixes with two prices — advance the funding or re-profile the payments; compare including one."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rolled a discount's deposit schedule against the funding rule — and bought the fix instead of the breach."}
            """),

        ("WC-CST-409", "The month the plant pays for itself", "Mobilisation's cash curve decides whether the expansion survives its own spring.",
            "Advanced Manufacturing", "Assistant Cost Engineer", "project_finance", "foundation", 12,
            """["cashflow","cost_management"]""",
            """
            {"context":"The plant expansion mobilises against a corporate facility with a hard ceiling, and the delivery plan's spring is front-loaded: steel, presses and enabling works all call for cash before the expansion's first revenue-generating line starts contributing in the summer. You roll the six-month mobilisation cashflow to find the position and the peak, before the facility paperwork is finalised at a number someone guessed.",
             "evidence":[
               {"label":"M1","value":"in 300,000 · out 550,000 (enabling works)"},
               {"label":"M2","value":"in 300,000 · out 700,000 (steel deposits)"},
               {"label":"M3","value":"in 400,000 · out 800,000 (press progress payments)"},
               {"label":"M4","value":"in 500,000 · out 450,000"},
               {"label":"M5","value":"in 900,000 · out 400,000 (first line contributes)"},
               {"label":"M6","value":"in 1,000,000 · out 350,000"}],
             "task":"cashflow",
             "given":{"periods":[
               {"period":1,"inflow":300000,"outflow":550000},
               {"period":2,"inflow":300000,"outflow":700000},
               {"period":3,"inflow":400000,"outflow":800000},
               {"period":4,"inflow":500000,"outflow":450000},
               {"period":5,"inflow":900000,"outflow":400000},
               {"period":6,"inflow":1000000,"outflow":350000}]},
             "ask":[
               {"key":"final_position","label":"Closing position after M6","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"},
               {"key":"cumulative_3","label":"Cumulative position after M3","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"facility","prompt":"Setting the facility, you recommend:",
                "options":[
                  {"key":"peak_plus","label":"Size the facility on the computed peak plus a margin for the two payments most likely to move early (steel and press milestones), with the peak month named so treasury sees when the exposure bites and when it unwinds","quality":100,
                   "consequence":"The facility signs once, at a defensible number; when the press milestone lands three weeks early the margin absorbs it, and the summer unwind releases the facility on schedule instead of by renegotiation.",
                   "principle":"Facilities are sized on the cumulative curve's deepest point plus the variance of what feeds it — the closing position is a footnote; the peak month is the decision."},
                  {"key":"final_only","label":"Size it near the closing position — the expansion ends the half-year close to even, so the exposure is temporary and modest","quality":0,
                   "consequence":"The spring's cumulative trough smashes through the undersized ceiling in month three; supplier payments hold, the steel slot wobbles, and the emergency facility extension prices the panic.",
                   "principle":"A cash curve that ends fine and dips deep is a solvency problem wearing a happy ending — the minimum, not the finish, sizes the money."},
                  {"key":"worst","label":"Size it on the sum of all outflows — no scenario can breach a facility that assumes no income at all","quality":25,
                   "consequence":"The maximal facility carries commitment fees on unused headroom all year, and the request's size triggers a corporate review that delays mobilisation by a month — caution priced as delay plus fees.",
                   "principle":"Ignoring committed inflows buys certainty the arithmetic already offered cheaper — the peak is the honest worst point of the real curve."}]}],
             "hints":["Roll the months cumulatively and find the deepest point — the story lives in the trough, not the finish.",
               "The peak funding requirement is that trough as a positive number; note which month it lands in.",
               "Size on peak plus the variance of the payments that feed it — and name the unwind month for treasury."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the month the expansion's cash curve bit hardest — and sized the facility on the trough, not the finish."}
            """),

        ("WC-PRC-410", "The damages the dashboard can't see", "Before levying delay damages, measure whose delay it is.",
            "Ports & Marine", "Assistant Project Controls Engineer", "project_controls", "foundation", 9,
            """["earned_value","contract_management"]""",
            """
            {"context":"The port expansion's quay package is late, and the client team is drafting a liquidated damages notice on the strength of the dashboard's red completion date. Before the notice issues, you run the package's earned-value position at the data date — because the contract's damages clause bites on contractor-culpable delay, and the indices are the first honest cut of what kind of late this is.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"3,600,000"},
               {"label":"Earned Value (EV)","value":"3,240,000"},
               {"label":"Actual Cost (AC)","value":"3,180,000"},
               {"label":"Budget at Completion (BAC)","value":"9,000,000"},
               {"label":"Context","value":"Client-instructed variations issued through the period; notice drafted on the dashboard date alone"}],
             "task":"evm","given":{"pv":3600000,"ev":3240000,"ac":3180000,"bac":9000000},
             "ask":[
               {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
               {"key":"cv","label":"Cost Variance (CV)","type":"number"},
               {"key":"spi","label":"SPI","type":"number"},
               {"key":"cpi","label":"CPI","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"notice","prompt":"Your advice on the damages notice:",
                "options":[
                  {"key":"attribute_first","label":"Hold the notice until the schedule variance is attributed — the package is behind plan but cost-efficient, a signature more consistent with instructed-variation delay than contractor failure; run the delay attribution against the variation account first, because a notice that ignores the client's own instructions invites the counterclaim it funds","quality":100,
                   "consequence":"Attribution shows most of the slippage tracks the client's variation instructions; the notice issues later, smaller, for the genuinely culpable residue — and survives, where the dashboard-driven version would have detonated into a global claim.",
                   "principle":"Damages follow culpability, not lateness — the indices don't settle attribution, but they tell you loudly when the easy story is the wrong one."},
                  {"key":"issue","label":"Issue on the dashboard date — the contract's completion date is objective, the package is behind it, and attribution arguments are the contractor's to make in defence","quality":10,
                   "consequence":"The contractor's defence arrives as a fully-assembled variation-delay counterclaim with your own instruction log as its exhibits; the damages recover a fraction of their face value and the relationship pays the rest.",
                   "principle":"Issuing first and attributing later hands the analysis to the party with the incentive to do it against you."},
                  {"key":"waive","label":"Advise against damages entirely — the cost efficiency shows a well-run contractor and the relationship matters more than the clause","quality":25,
                   "consequence":"Generous and unearned in part: the attribution, never run, would have shown a culpable residue worth pursuing; the waiver prices goodwill with money the client was owed.",
                   "principle":"Waiving unmeasured entitlement is not relationship management, it is rounding in one direction — measure, then decide what mercy costs."}]}],
             "hints":["Work the four indices; notice what behind-plan-but-under-cost usually means.",
               "The damages clause bites on culpable delay — the variance must be attributed before it is invoiced.",
               "Check the variation instructions against the slippage window before any notice names a number."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ran the earned-value cut that stopped a dashboard-driven damages notice — and the smaller, attributed one stuck."}
            """),

        // ───────────── Reserve · Stakeholder Dilemmas · practitioner · negotiation ─────────────

        ("WC-STK-411", "Both heads want the same engineer", "Two headteachers, one structural specialist, and a half-term that fits only one survey.",
            "Public Sector Estates", "Programme Manager", "project_management", "professional", 7,
            """["resource_management","negotiation","stakeholder_communication"]""",
            """
            {"context":"The schools estate programme's commissioning autumn has produced a collision: two schools' remediation surveys both need the programme's one approved structural specialist during the same half-term week — the only week either head will grant full access. Each headteacher has separately been told 'the specialist will be with you at half-term'; both promises trace to your own team. The heads have discovered the double-booking, and both are now on today's call, each armed with the promise and neither inclined to blink.",
             "evidence":[
               {"label":"Constraint","value":"One specialist; both surveys want the same half-term week"},
               {"label":"Promises","value":"Both schools separately told 'half-term' — by your team"},
               {"label":"Access","value":"Heads will only grant full access in the school holiday"},
               {"label":"Now","value":"Both heads on the call, promises in hand"}],
             "decisions":[
               {"key":"call","prompt":"On the call, you:",
                "options":[
                  {"key":"own_solve","label":"Own the double promise plainly, then move the conversation from whose promise wins to what each survey actually needs: the intrusive structural work needs the specialist in person, the preliminary measurements don't — so one school gets the specialist at half-term, the other gets an instrumented preliminary visit now plus the specialist's full survey in the Christmas break, with the sequencing choice made on building risk, not on who shouted first","quality":100,
                   "consequence":"The risk logic picks the school with the suspect transfer beams for half-term; the other head, given a real date and an honest apology instead of a diplomatic fog, accepts the Christmas slot — and both surveys happen inside the term the programme needed.",
                   "principle":"When your own team promised one resource twice, the repair starts with owning it — then re-cutting the work so the constraint serves need in risk order, not promise order."},
                  {"key":"first_promise","label":"Honour whichever promise was made first — chronological fairness is the only defensible tiebreak between identical commitments","quality":20,
                   "consequence":"The email archaeology crowns a winner on a timestamp; the losing head escalates to the trust board with the promise attached, and the programme's autumn is spent explaining why building risk ranked below inbox order.",
                   "principle":"Chronology is a tiebreak for equal needs — when the buildings differ in risk, 'first promised' is fairness about the wrong thing."},
                  {"key":"split_week","label":"Split the half-term week between the schools — three days each, both promises technically kept","quality":10,
                   "consequence":"Neither survey completes: intrusive investigations don't halve, both schools reopen with scaffolding still up, and both heads now hold a broken promise instead of one holding an honest re-plan.",
                   "principle":"Splitting an indivisible resource keeps the promises and loses the purpose — some work is whole or it is worthless."}]}],
             "hints":["Start by owning the double promise — the negotiation cannot start while blame is still in question.",
               "Decompose the surveys: which parts truly need the specialist, and which need only instruments and access?",
               "Rank by building risk and give the second school a real, dated alternative — not a diplomatic fog."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Owned a double-booked specialist on a call with both headteachers — and re-cut the work so risk, not volume, decided."}
            """),

        ("WC-STK-412", "The sponsor wants a warmer ending", "The closeout review is drafted. The sponsor would like it to say something else.",
            "Life Sciences", "Project Manager", "project_management", "professional", 5,
            """["governance","professional_ethics","stakeholder_communication"]""",
            """
            {"context":"The laboratory relocation is complete and your closeout review is drafted: delivery solid, but the case's consolidation benefit — the reason the move was funded — is unrealised because the second lab's closure was quietly cancelled mid-project. The sponsor, who owns both the project and next year's funding round, calls to suggest the review 'focus on the successful delivery' and treat the benefits position as 'evolving'. The review goes to the audit committee, over your name, on Thursday.",
             "evidence":[
               {"label":"Draft","value":"Delivery strong; consolidation benefit unrealised — closure cancelled mid-project"},
               {"label":"Ask","value":"'Focus on delivery; benefits are evolving'"},
               {"label":"Stakes","value":"Sponsor owns next year's funding; review carries your name"}],
             "decisions":[
               {"key":"respond","prompt":"You:",
                "options":[
                  {"key":"hold_offer","label":"Hold the review's substance and offer the sponsor something legitimate instead: the benefits section stays factual (closure cancelled, benefit unrealised, decision needed), but you'll gladly add the sponsor's forward plan for the second lab as a formal response within the same paper — their story beside your facts, both signed by their authors","quality":100,
                   "consequence":"The sponsor, offered a dignified channel instead of a fight, writes the response section; the audit committee reads facts and plan together, funds the closure decision properly — and your reviews keep the signature that makes them worth reading.",
                   "principle":"The answer to 'soften the facts' is almost never yes and rarely just no — it is a structure where the facts stay yours and the narrative stays theirs, in the open."},
                  {"key":"soften","label":"Accommodate the ask — 'evolving' is arguably true, the sponsor's goodwill funds next year, and closeout reviews are remembered for a quarter at most","quality":0,
                   "consequence":"The committee approves against 'evolving'; the second lab runs on for two more years of double costs, and when the FOI-able review is eventually read beside the cancellation memo's date, 'evolving' has your name under it.",
                   "principle":"A fact softened at a sponsor's request is not diplomacy, it is authorship of their version — and reviews outlive the goodwill they were bent for."},
                  {"key":"escalate","label":"Refuse and copy the audit committee chair on the exchange — pressure on a review's author is itself reportable","quality":25,
                   "consequence":"Technically defensible, relationally scorched: the sponsor's suggestion was improper but recoverable, and the escalation converts a negotiable moment into a formal grievance that outlasts both the review and the relationship.",
                   "principle":"Escalate pressure that persists after refusal — not the first ask, which deserves the chance to become a legitimate response section."}]}],
             "hints":["Separate what must stay yours (the facts) from what can be theirs (the forward narrative).",
               "Offer the formal-response structure before treating the ask as misconduct.",
               "Sign nothing you'd need to explain beside a dated memo later."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept the unrealised benefit in the closeout and gave the sponsor a signed response section instead of a softer fact."}
            """),

        ("WC-STK-413", "Two rooms, one retrofit", "The concept board wants ambition. The residents' panel wants candour. You write both slides tonight.",
            "Climate & Retrofit", "Engagement Lead", "project_management", "professional", 6,
            """["stakeholder_communication","concept_planning","reporting_integrity"]""",
            """
            {"context":"The retrofit programme's concept case goes to two audiences this week with the same underlying numbers: the investment board, which wants the ambitious scenario (2,000 homes, full-street approach), and the residents' panel, which was burned by the pilot's disruption and wants plain answers about what happens to their street. The programme director suggests 'tailoring': the board pack leads with the 2,000-home ambition, the residents' pack describes a gentler 'phased approach' — the same programme, dressed differently for each room. You drift the drafts and realise the two packs, side by side, read as two different programmes.",
             "evidence":[
               {"label":"Audience 1","value":"Investment board — wants the 2,000-home full-street scenario"},
               {"label":"Audience 2","value":"Residents' panel — wants disruption candour after a bruising pilot"},
               {"label":"Suggestion","value":"'Tailor' the packs — ambition for one room, gentleness for the other"},
               {"label":"Test","value":"Side by side, the drafts describe two different programmes"}],
             "decisions":[
               {"key":"packs","prompt":"You build:",
                "options":[
                  {"key":"one_truth","label":"One fact base, two depths: both packs carry the same programme — 2,000 homes, full-street method, the pilot's honest disruption data and what changes because of it — with the board pack going deep on investment logic and the residents' pack going deep on street-level sequence and standards; different emphasis, zero contradiction, and either room could read the other's pack without surprise","quality":100,
                   "consequence":"A panel member's councillor sits on the investment board — as councillors do — and reads both packs in the same week; finding one programme in two depths instead of two programmes, she becomes the scheme's advocate in both rooms.",
                   "principle":"Tailoring is depth and emphasis; two stories is a time bomb — write every pack as if the other room will read it, because in any real programme, it will."},
                  {"key":"tailor","label":"Follow the direction — audiences hear what they can act on, and the phased framing for residents is arguably just sensitive sequencing of the same facts","quality":0,
                   "consequence":"The packs cross within a fortnight — a resident FOIs the board papers — and 'the programme that tells each room what it wants to hear' becomes the story; the panel's trust, half-rebuilt after the pilot, resets to zero with interest.",
                   "principle":"Two versions of one programme is a comparison waiting to be made — and the audience that finds the gentler version was for them learns exactly what your candour is worth."},
                  {"key":"delay_res","label":"Send the board pack now and hold the residents' pack until the board chooses a scenario — no contradiction if only one document exists at a time","quality":25,
                   "consequence":"The residents' meeting proceeds pack-less on rumour and the board papers' leak; the panel reads the ambition raw, without the disruption mitigation the residents' pack would have carried, and the vacuum writes the worst version.",
                   "principle":"Sequencing disclosure to avoid contradiction just lets the loudest document speak alone — the second room hears the first room's version, unaccompanied."}]}],
             "hints":["Test the drafts side by side: same programme, or two? That is the whole question.",
               "Tailor depth and emphasis, never facts — assume every pack reaches every room.",
               "The pilot's disruption data belongs in BOTH packs — candour is only credible where it is inconvenient."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Wrote one programme at two depths instead of two programmes — and the councillor who read both packs proved why."}
            """),

        ("WC-STK-414", "The joiners were promised to the gallery", "Definition isn't finished, but the specialist crew's diary already is.",
            "Cultural Projects", "Project Manager", "project_management", "professional", 7,
            """["resource_management","negotiation","definition_planning"]""",
            """
            {"context":"The museum gallery rebuild is in definition, and you discover the specialist heritage joinery crew — the project's scarcest trade — has been verbally promised twice for the same spring window: by your side, to the gallery's showcase joinery (the definition schedule assumes it), and by the joinery firm's own director, to a cathedral restoration that 'has been in our book for a year'. The firm is small, family-run, genuinely excellent and genuinely embarrassed; there is no comparable crew within sensible distance; and your definition schedule, benefits case and opening season all quietly assume the spring window holds.",
             "evidence":[
               {"label":"Collision","value":"Same crew, same spring window: your showcase joinery vs a cathedral's year-old booking"},
               {"label":"Firm","value":"Small, excellent, embarrassed — and holding two promises"},
               {"label":"Market","value":"No comparable crew within sensible distance"},
               {"label":"Exposure","value":"Definition schedule + opening season assume spring"}],
             "decisions":[
               {"key":"negotiate","prompt":"With the firm's director, you:",
                "options":[
                  {"key":"reshape","label":"Negotiate the work's shape, not the diary's owner: ask what the showcase joinery actually needs the full crew for and when — then re-cut your definition schedule so the workshop fabrication (which the firm can do around the cathedral job) starts early, and the on-site installation takes a shorter, later window the cathedral's programme can release; formalise the revised booking in writing, because the collision existed precisely because nothing was written","quality":100,
                   "consequence":"The fabrication starts in the firm's workshop within the month; the installation lands in a four-week autumn window both jobs can honour, the opening season holds by re-sequencing the gallery fit-out around it — and the firm, saved from breaking a promise to a cathedral or a museum, does its best work for both.",
                   "principle":"When a scarce crew is promised twice, the winning move is usually re-shaping the demand, not winning the allocation — fabrication and installation rarely need the same window, and paper fixes what memory caused."},
                  {"key":"outbid","label":"Secure the crew commercially — offer a retainer premium for the spring window and let the firm manage its own cathedral problem","quality":15,
                   "consequence":"The family firm, forced to choose money over a year-old promise, resents the position more than it enjoys the premium; the cathedral's diocese — a small world — remembers, and the crew's spring work arrives punctual and joyless, with the best carver 'unavailable'.",
                   "principle":"Outbidding a small firm against its own word buys the diary and poisons the workmanship — scarce crafts run on relationships that premiums can rent but not replace."},
                  {"key":"hold_promise","label":"Hold the firm to your promise — your project's assumption was made in good faith and definition schedules cannot re-plan around every supplier's double-booking","quality":10,
                   "consequence":"'Your promise' turns out to be a phone call your predecessor made against the cathedral's written booking; the firm honours the paper, your spring window collapses with three months' notice instead of nine, and the re-plan happens anyway — late, and with the relationship spent.",
                   "principle":"A verbal assumption is not a booking — insisting on it against someone else's paper just schedules the disappointment for later."}]}],
             "hints":["Establish what was actually promised, in what form, before arguing about whose promise wins.",
               "Split the work: fabrication and installation almost never need the same window.",
               "Get the revised booking in writing — the collision was born verbal; don't let the fix be."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Un-double-booked a heritage crew by re-shaping the work instead of outbidding a cathedral — and put the fix on paper."}
            """),

        // ───────────── Reserve · Logic & Sequence · foundation · order/rank ─────────────

        ("WC-QLT-415", "Progress, or rework wearing its badge", "Four dashboard claims — rank how real they are before the JV quotes them.",
            "Joint Ventures", "Assistant Quality Engineer", "project_controls", "foundation", 5,
            """["quality_management","progress_measurement"]""",
            """
            {"context":"The joint venture's mobilisation dashboard shows the enabling package at 70% and climbing, and the JV board will quote it on Thursday. Walking the work with the records, you find the percentage is built from four kinds of claims, and they are not equally real. Rank the four claims from most to least trustworthy as progress, so the dashboard's number can be rebuilt on the honest ones.",
             "evidence":[
               {"label":"Claim A","value":"Piling verified by independent test results — signed, filed"},
               {"label":"Claim B","value":"Drainage runs installed and inspected — but 30% failed inspection and are queued for rework, still counted as done"},
               {"label":"Claim C","value":"Ducting 'complete' per the subcontractor's own unverified return"},
               {"label":"Claim D","value":"Fencing physically complete, walked and photographed last week"}],
             "decisions":[
               {"key":"rank","prompt":"Most trustworthy to least, you rank:",
                "options":[
                  {"key":"a_d_c_b","label":"A (independently verified), then D (physically evidenced), then C (claimed, unverified), then B (counted work that inspection has already rejected) — because verified beats witnessed, witnessed beats claimed, and rework queued is progress already proven false","quality":100,
                   "consequence":"Rebuilt on A and D with C flagged for verification and B stripped to its passed fraction, the honest figure lands well under 70; Thursday's board hears the real number with its evidence tiers — and orders the ducting verification instead of quoting the fiction.",
                   "principle":"Progress claims rank by their evidence: independent verification, then physical witness, then self-declaration — and work that failed inspection is not low-grade progress, it is measured non-progress."},
                  {"key":"b_high","label":"Keep B high in the ranking — the drainage is physically installed, and rework is a quality matter that shouldn't contaminate the progress measure","quality":10,
                   "consequence":"The 'installed' drainage is excavated twice — once physically, once from the dashboard — and the board learns that the progress measure and the quality records described the same trench differently for a quarter.",
                   "principle":"Separating progress from quality counts the same work twice: once going in, once coming out — installed-and-failed is a rework liability, not an achievement."},
                  {"key":"c_high","label":"Rank C beside D — the subcontractor's return is contractually certified and disbelieving it by default sours the relationship","quality":25,
                   "consequence":"The ducting verification, when it finally runs, finds the 'complete' runs 60% pulled; the gap surfaces in commissioning week, and the relationship the ranking protected sours anyway — over a bigger number, later.",
                   "principle":"A self-declared return is a claim awaiting evidence — treating it as witnessed fact just moves the discovery to the most expensive date."}]}],
             "hints":["Sort by evidence type first: independent test, physical witness, self-declaration.",
               "Work that failed inspection is not partial progress — it is proven non-progress queued for repayment.",
               "Rebuild the number from the trustworthy tiers and flag the rest for verification, not for belief."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ranked four dashboard claims by their evidence — and rebuilt 70% into a number the board could safely quote."}
            """),

        ("WC-SCO-416", "The de-scoped stand returns at half-time", "The exclusion is back, mid-build. Sequence the response before it sequences you.",
            "Stadia & Venues", "Assistant Project Manager", "project_management", "foundation", 6,
            """["scope_management","change_control"]""",
            """
            {"context":"Mid-execution on the stadium fit-out, the client's operations director announces that the north concourse catering stands — de-scoped at contract to save money, with the exclusion documented — are 'back on, the board found the budget'. The stands touch live work: services routed through their zone assumed the void stayed empty, and the concourse slab sequence is three weeks from closing over. Five responses are on your desk; the order you take them in decides whether this lands as a change or a crisis. Sequence: (A) instruct a hold on closing the affected slab zone; (B) issue a formal change enquiry to price and programme the stands; (C) design check — can the routed services and the stands coexist; (D) tell the client the exclusion means the request must wait for the next project; (E) accept verbally and let the site squeeze it in while the teams are mobilised.",
             "evidence":[
               {"label":"Fact","value":"De-scoped stands reinstated by client board — funding found"},
               {"label":"Physics","value":"Services routed through the stands' void; slab closes in 3 weeks"},
               {"label":"Options","value":"A hold slab zone · B change enquiry · C design check · D refuse · E absorb informally"}],
             "decisions":[
               {"key":"order","prompt":"Your sequence:",
                "options":[
                  {"key":"a_c_b","label":"A, then C, then B — hold the closing slab zone today (the only irreversible clock), run the coexistence design check so the change enquiry prices a buildable answer, then issue the formal enquiry; D and E never happen, because refusal ignores a funded client instruction and informal absorption unprices it","quality":100,
                   "consequence":"The hold costs four days of re-sequenced slab work; the design check finds the services need one relocated run, priced into a change the client signs in a fortnight — the stands arrive as a governed variation instead of a demolition story.",
                   "principle":"When returning scope touches closing work, sequence by irreversibility: stop the concrete first, establish buildability second, price formally third — never refuse a funded instruction, never absorb one informally."},
                  {"key":"b_first","label":"B first — the change process is the front door, and holds or design checks before a signed enquiry put the cart before the horse","quality":20,
                   "consequence":"The enquiry processes properly for eleven days while the slab zone closes over the void; the priced change comes back buildable-no-more, and the correct process delivers the expensive answer.",
                   "principle":"Process order and physical order are different clocks — the change process governs the money; only a site instruction can stop the concrete."},
                  {"key":"e_fast","label":"E — the teams are mobilised, the void exists this week, and a pragmatic site accommodation now beats weeks of paperwork","quality":0,
                   "consequence":"The stands get squeezed in unpriced and undesigned; the services clash surfaces at commissioning, the 'found budget' was never contractually captured, and the final account fights about work nobody instructed.",
                   "principle":"Absorbing returned scope informally builds it twice — once on site, once in the dispute."}]}],
             "hints":["Find the irreversible clock first — the slab closing is the only step that can't be undone.",
               "Buildability before price: an enquiry issued before the design check prices a guess.",
               "A funded client instruction is never refused and never absorbed — it is held, checked, then formally priced."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held the slab, checked the clash, then priced the change — and a returning exclusion landed as paperwork, not demolition."}
            """),

        ("WC-SCH-417", "Which path gets the weekend", "Two chains race to the commissioning date. Order the recovery moves before buying any.",
            "Water Utilities", "Assistant Planner", "project_controls", "foundation", 7,
            """["schedule_analysis","commissioning"]""",
            """
            {"context":"The water treatment upgrade's commissioning date is promised to the regulator, and two activity chains both claim to drive it: the mechanical chain (pump installation → pipework → pressure test) and the electrical chain (MCC delivery → cabling → point-to-point testing). The site manager wants to buy weekend working, but for which chain? The plan data: mechanical chain has 26 working days of remaining duration; electrical has 24 — but the MCC delivery within it is a supplier date with a history of slipping, and the pressure test needs the electrical point-to-point complete first, linking the chains at the end. Rank the recovery moves: (A) weekend working for the mechanical chain; (B) expediting visit to the MCC supplier; (C) weekend working for electrical cabling; (D) re-sequencing the pressure test procedure to start partial testing early.",
             "evidence":[
               {"label":"Mechanical chain","value":"26 days remaining — pumps → pipework → pressure test"},
               {"label":"Electrical chain","value":"24 days remaining — MCC delivery → cabling → point-to-point"},
               {"label":"Link","value":"Pressure test needs point-to-point complete — chains merge at the end"},
               {"label":"Wildcard","value":"MCC supplier date has a slipping history"}],
             "decisions":[
               {"key":"rank","prompt":"Your recovery priority order:",
                "options":[
                  {"key":"b_d_then","label":"B first (the MCC date is the one input that can silently move both chains — an expediting visit converts a rumour into a date), D second (partial pressure testing de-links the chains' merge for free), and only then A or C — bought against whichever chain the firmed-up MCC date says is actually critical","quality":100,
                   "consequence":"The expediting visit finds the MCC three weeks adrift of its promise — the real critical path nobody's plan showed; the recovered picture puts the weekend money on cabling AFTER delivery, the partial testing claws back four days, and the regulator's date survives on facts instead of overtime guesses.",
                   "principle":"Before buying acceleration, firm up the inputs that can move the answer — the cheapest recovery move is information, and the second cheapest is logic; overtime is what you buy third, aimed by the first two."},
                  {"key":"a_first","label":"A first — mechanical is the longer chain at 26 days, and the longest chain is the critical path by definition","quality":15,
                   "consequence":"The weekend crews compress mechanical beautifully while the MCC slips three silent weeks; the merged end-date moves anyway, and the overtime bought float on a path that stopped mattering.",
                   "principle":"The critical path is only as real as its least reliable input — a supplier date with a slipping history outranks a two-day duration difference."},
                  {"key":"c_first","label":"C first — electrical is nearly as long and the point-to-point gates the pressure test, so compressing cabling protects the merge","quality":25,
                   "consequence":"The cabling compresses toward an MCC that hasn't arrived; the weekend crews wait on materials mid-shift, and the acceleration spend converts to standing time with a premium rate.",
                   "principle":"Accelerating downstream of an unconfirmed delivery buys idle crews — sequence recovery behind the constraint, never in front of it."}]}],
             "hints":["Ask which input could move the whole answer — the slipping supplier date outranks the two-day difference.",
               "Cheap moves first: information (expedite), then logic (partial testing), then money (weekends).",
               "Aim any overtime with the firmed-up picture, not the plan's current guess."],
             "profile_map":{"decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ranked information and logic ahead of overtime — and found the real critical path at the MCC factory, not on the plan."}
            """),

        ("WC-QLT-418", "The hold point and the go-live party", "Closeout wants the certificate signed. The witness step it certifies hasn't happened.",
            "Technology Programmes", "Assistant Quality Analyst", "project_controls", "foundation", 5,
            """["quality_management","closeout","professional_ethics"]""",
            """
            {"context":"The core-system replacement's closeout is days from its celebration: the completion certificate pack is assembled, and one line is blocking — the data-reconciliation witness test, a contractual hold point requiring the client's representative to witness the final ledger comparison. The client rep has been unavailable for two weeks; the delivery lead suggests four ways forward and wants your ranking today: (A) run the reconciliation now, record it fully, and hold the certificate line open until the rep can witness a re-run or formally waive the requirement; (B) sign the line as complete — 'the reconciliation numbers are fine, witnessing is a formality'; (C) escalate to the client that their own rep's unavailability is now blocking their certificate, with dates offered; (D) quietly drop the line from the pack — it was arguably gold-plating in the original spec.",
             "evidence":[
               {"label":"Blocker","value":"Contractual hold point: client-witnessed ledger reconciliation"},
               {"label":"Status","value":"Client rep unavailable 2 weeks; numbers themselves look clean"},
               {"label":"Pressure","value":"Certificate pack otherwise ready; go-live celebration scheduled"},
               {"label":"Options","value":"A run + hold open · B sign as done · C escalate with dates · D drop the line"}],
             "decisions":[
               {"key":"rank","prompt":"Your ranking:",
                "options":[
                  {"key":"c_a","label":"C first and A alongside — escalate the rep's unavailability to the client with offered dates (their hold point, their blocker, their call to witness or formally waive), while running and fully recording the reconciliation now so the witnessed re-run is a half-day formality; B and D are not options in any order, because one certifies a witnessing that didn't happen and the other deletes a contract term by preference","quality":100,
                   "consequence":"The client, shown that its own rep is the blocker, produces a substitute witness in three days; the pre-run reconciliation makes the witnessed session trivial, the certificate signs whole — and the pack contains no line anyone ever has to hope goes unread.",
                   "principle":"A blocked hold point is escalated to whoever owns it and prepared for in parallel — never signed around, never deleted; the certificate's value IS the truth of its lines."},
                  {"key":"b_prag","label":"B — the numbers are verifiably clean, the witnessing adds no information, and blocking a go-live on a diary problem serves nobody","quality":0,
                   "consequence":"The certificate signs with a false line; eighteen months later a ledger dispute sends auditors to the reconciliation evidence, and the signature certifying an unwitnessed witnessing becomes the finding that outgrows the dispute.",
                   "principle":"Signing that something happened which didn't is never a pragmatic shortcut — it is the one act that converts a diary problem into an integrity one."},
                  {"key":"d_tidy","label":"D — challenge the line's necessity through the closeout review; if it was over-specified, removing it is housekeeping, not concealment","quality":20,
                   "consequence":"The 'housekeeping' needs the client's agreement to vary the contract — which is option C with extra steps and worse optics; raised as a quiet deletion instead, it reads exactly like what it would be.",
                   "principle":"A contractual requirement leaves the contract by agreement, in daylight — a closeout pack is the last place to edit one quietly."}]}],
             "hints":["Whose hold point is it? The blocker belongs to its owner — escalate with dates, don't absorb.",
               "Prepare in parallel: run and record now so the witnessed session is trivial when it lands.",
               "Any option that signs or deletes the requirement answers itself — rank it last and say why."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Escalated a blocked hold point to its owner and pre-ran the evidence — the certificate signed whole, three days later."}
            """),

        // ───────────── Reserve · Executive Missions · capstone ─────────────

        ("WC-CAP-419", "Sign here, factory by factory", "The relocation's approvals were built for one site. You are about to run nine.",
            "Manufacturing", "Programme Director", "project_management", "expert", 26,
            """["governance","delegation","concept_planning"]""",
            """
            {"context":"You direct the factory relocation programme at concept stage: nine production sites consolidating into three over four years. The delegation framework you inherit was written for the company's single-site projects: every contract award above a modest threshold goes to the group investment committee, every design change above a smaller one to the programme board. Applied to nine parallel site moves, the arithmetic is absurd — your planners estimate 140 committee items a year, a committee that meets six times. The CFO's instinct is to raise the thresholds ('same framework, bigger numbers'). Your delivery directors want full delegation to site teams ('trust the people'). The concept case must fix the governance design before sanction, because the framework you take into delivery is the one you will drown in or be burned by.",
             "evidence":[
               {"label":"Inherited","value":"Single-site delegation: modest thresholds, all roads lead to group committee"},
               {"label":"Arithmetic","value":"~140 committee items/year against 6 meetings"},
               {"label":"CFO instinct","value":"Same framework, bigger thresholds"},
               {"label":"Directors' ask","value":"Full delegation to site teams"}],
             "decisions":[
               {"key":"design","prompt":"Stage 1 — the governance design you take to sanction:",
                "options":[
                  {"key":"tiered","label":"Redesign by decision type, not just size: site teams own execution decisions inside a site's approved envelope (any size, within scope); the programme board owns anything crossing sites — sequence changes, shared-resource allocation, inter-site scope moves — because cross-site is where relocation programmes actually die; the group committee keeps envelope changes and the three consolidation gate decisions, with a monthly delegation report showing every above-threshold call made below it","quality":100,
                   "consequence":"Sanction passes with a governance annex the committee actually reads; year one runs 30 committee items instead of 140 — the envelope changes and gates that deserve the room — while the cross-site collisions the old framework would have missed (two sites bidding the same rigging window) surface at the programme board that now exists to catch them.",
                   "principle":"Delegation scales by routing decisions to where their consequences live — size thresholds alone route big-but-local noise upward and let small-but-systemic risks straight through."},
                  {"key":"bigger","label":"Take the CFO's route — proven framework, thresholds multiplied to fit programme scale, no new governance to design or defend","quality":15,
                   "consequence":"The committee load drops to a manageable 40 — and the raised thresholds now delegate the genuinely dangerous decisions too: a site team re-sequences its own move around a shared commissioning crew, inside its new threshold, and sites two and three inherit the collision at full speed.",
                   "principle":"Scaling thresholds scales both the relief and the blindness — the single-site framework's real flaw was routing by size, and multiplication preserves it perfectly."},
                  {"key":"trust","label":"Take the directors' route — full site delegation with a light programme-office dashboard; nine empowered teams beat one bottlenecked committee","quality":10,
                   "consequence":"Empowered sites optimise locally and brilliantly; the shared mould-shop equipment gets promised to two moves, the group's banking covenants meet a quarter where three sites' capital calls landed together unseen, and the re-centralisation that follows is harsher than the framework it replaces.",
                   "principle":"Full delegation without a cross-cutting tier doesn't distribute governance, it deletes the only level that can see the programme."}]},
               {"key":"teeth","prompt":"Stage 2 — making the design survive its first crisis:",
                "options":[
                  {"key":"stress_test","label":"Stress-test the framework before sanction with three named scenarios — a site overspend discovered late, a shared-resource collision, a supplier failure spanning sites — walked through the tiers on paper with the CFO and directors in the room, so the first real crisis follows a rehearsed route instead of triggering the reflex re-centralisation that kills delegated frameworks","quality":100,
                   "consequence":"The walkthrough finds two routing gaps (supplier failure had no owner; the escalation clock was undefined) — fixed in the annex for the cost of an afternoon; when the real supplier failure arrives in month seven, it follows the rehearsed route, and the framework's credibility compounds instead of collapsing.",
                   "principle":"Delegation frameworks die at their first unrehearsed crisis — the reflex is always to grab everything back; scenarios walked on paper are the vaccine."},
                  {"key":"review_later","label":"Launch the framework and review it after six months of live running — real experience beats hypothetical scenarios","quality":25,
                   "consequence":"The six-month review is pre-empted at month four by the supplier failure, which finds its routing gap live; the CFO's grab-back instinct wins the crisis meeting, and the review, when it comes, documents a framework already half-recentralised.",
                   "principle":"A framework's first crisis arrives on its own schedule, not the review cycle's — rehearse before launch or amend after the funeral."}]}],
             "hints":["Count the committee arithmetic first — a framework that cannot physically meet its own load is already broken.",
               "Route by decision type: local-any-size down, cross-site up — size thresholds alone are blind to system risk.",
               "Stress-test the design on paper with the sceptics in the room before sanction — rehearsed routes survive crises."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Redesigned a single-site delegation framework for nine parallel moves — routed by consequence, stress-tested before sanction."}
            """),

        ("WC-CAP-420", "The plan of record, resignalled", "Four years, three re-plans, one board — design the baseline regime before the definition signs.",
            "Rail Systems", "Programme Director", "project_management", "expert", 28,
            """["governance","baseline_management","definition_planning"]""",
            """
            {"context":"You direct definition of the rail resignalling programme: four years, forty possessions, a regulator, an operator and a board that meets quarterly. The last programme this organisation ran taught everyone the wrong lesson twice: its baseline was held rigid for two years ('no re-baseline without board approval' meant none was sought), so reporting became fiction against a dead plan; then, over-corrected, its successor re-planned so fluidly that 'green' meant nothing for a different reason. Your definition must now write the baseline-management regime this programme will live under — the rules for what the plan of record is, who may change it, at what size, with what memory. The board wants 'firm baselines'; the delivery teams want 'realistic ones'; both are scarred, and both are right about the failure they saw.",
             "evidence":[
               {"label":"Scar 1","value":"Predecessor held a dead baseline 2 years — reporting became fiction"},
               {"label":"Scar 2","value":"Its successor re-planned fluidly — 'green' meant nothing"},
               {"label":"Parties","value":"Quarterly board, regulator, operator, 40 possessions, 4 years"},
               {"label":"Task","value":"Write the baseline regime into definition, before sanction"}],
             "decisions":[
               {"key":"regime","prompt":"Stage 1 — the regime you write:",
                "options":[
                  {"key":"threshold_memory","label":"A materiality ladder with memory: below-threshold variances absorb into the plan with monthly disclosure (the plan stays alive); above-threshold changes — completion dates, possession strategy, regulated milestones — re-baseline ONLY through the board, on a standing fast-track paper; and every re-baseline is versioned with a closure analysis, so performance is always reported against both the original sanction and the current plan of record — the two scars answered by the two numbers","quality":100,
                   "consequence":"Year two's flood-driven re-plan takes eleven days through the fast-track instead of festering unsought; the board sees drift-from-sanction and performance-against-current on one page for four years, and neither scar reopens — the plan stays real AND the history stays honest.",
                   "principle":"Baseline regimes fail at the extremes: rigidity breeds fiction, fluidity breeds amnesia — the cure is a materiality ladder for change and a version memory for truth, so 'green' means something against a plan that means something."},
                  {"key":"firm","label":"Give the board its firm baseline — annual re-baseline windows only, everything else reports as variance; discipline is what the first scar demanded","quality":15,
                   "consequence":"Month seven's possession-strategy change waits five months for the annual window; the teams, needing a live plan to work to, quietly maintain one anyway — and the programme is running on two plans by year two, which is the first scar with better stationery.",
                   "principle":"A re-baseline calendar that ignores when reality changes doesn't prevent re-planning — it just unlicenses it."},
                  {"key":"realistic","label":"Give the teams their realistic baseline — the plan of record updates monthly to the working schedule, and the board reads honest current dates instead of stale ones","quality":10,
                   "consequence":"Honest and bottomless: with the baseline tracking the working schedule, variance is definitionally near zero, the completion date walks eleven months right in eight monthly steps nobody ever decided, and the regulator asks — reasonably — what the sanction approved, exactly.",
                   "principle":"A baseline that follows the schedule cannot measure it — drift absorbed monthly is drift nobody ever chose, which is the second scar verbatim."}]},
               {"key":"regulator","prompt":"Stage 2 — the regulator and operator hold milestone protections. You:",
                "options":[
                  {"key":"tiered_external","label":"Write the external interface into the regime: regulated and operator-facing milestones sit in the top materiality tier — unchangeable without the external party's process, with the programme's internal ladder feeding early-warning notices at defined drift thresholds BEFORE any formal change — so external trust is built on advance notice, and the regime's internal flexibility never silently consumes an external commitment","quality":100,
                   "consequence":"When year three's drift approaches a regulated milestone, the early-warning notice lands six months out; the regulator, warned early by design rather than late by discovery, agrees a revised milestone without enforcement — the first time this organisation has changed a regulated date without a formal breach.",
                   "principle":"External commitments need their own tier and their own clock — a baseline regime that treats the regulator's milestone like an internal date will eventually spend it like one."},
                  {"key":"buffer_ext","label":"Protect external milestones with internal buffers instead — hold the regulated dates loose inside padded internal targets and manage the gap privately","quality":25,
                   "consequence":"The private padding works until it doesn't: the buffer erodes in undisclosed increments, and when the regulated milestone finally moves, the regulator learns the programme knew for fourteen months — the enforcement reflects the silence more than the slip.",
                   "principle":"Buffers protect dates; only disclosure protects relationships — external parties forgive drift they were warned of and punish drift they discover."}]}],
             "hints":["Name the two failure modes first — rigidity's fiction and fluidity's amnesia — and design against both at once.",
               "The ladder handles change; the version memory handles truth: report against sanction AND current, always.",
               "External milestones get their own tier and early-warning clock — advance notice is the currency regulators accept."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Wrote a baseline regime with a materiality ladder and a memory — and changed a regulated date without a breach for the first time."}
            """),
    };
}
