namespace PCI.Backend.Data;

/// <summary>
/// PCI Project Intelligence — Year-1 January authored pack (Phase B slice 1;
/// docs/pciworld/PROJECT_INTELLIGENCE.md §3).
///
/// The 28 experiences that complete January of the governed Year-1 plan: every item is authored
/// TO its plan slot (experience type, domain, difficulty band, duration, interaction, lifecycle,
/// sector fixed by the manifest) and every item carries the full Project Intelligence editorial
/// contract this phase introduces — three progressive hints, consequence + principle per option,
/// a factual share line. All data is synthetic. Seeding reuses WorldContentPack.UpsertHouse, so
/// the replay-immutability discipline is identical: a config change is a NEW immutable version.
///
/// January theme: decision foundations and governance, at concept / business-case stage.
/// </summary>
public static class WorldIntelligencePack
{
    public static int Count => Items.Length;

    public static void Seed(Db db)
    {
        foreach (var (code, title, hook, industry, role, track, difficulty, minutes, competencies, config) in Items)
            WorldContentPack.UpsertHouse(db, code, title, hook, industry, role, track, difficulty, minutes, competencies, config);
    }

    static readonly (string Code, string Title, string Hook, string Industry, string Role, string Track,
        string Difficulty, int Minutes, string Competencies, string Config)[] Items =
    {
        // ───────────── Daily Decision · integration & governance · foundation ─────────────

        ("WC-GOV-053", "The gate you cannot half-pass", "A stage gate with one green column and two amber ones. The board wants a recommendation, not a shrug.",
            "Capital Programmes", "PMO Analyst", "project_management", "foundation", 6,
            """["governance","decision_quality"]""",
            """
            {"context":"A multi-site capital programme reaches Gate 2. The business case is green on benefits, amber on cost confidence (estimate maturity is Class 4) and amber on delivery capacity (two of five sites have no named project manager). The gate panel asks the PMO for a single recommendation this afternoon.",
             "evidence":[
               {"label":"Benefits case","value":"Green — benefits model independently reviewed"},
               {"label":"Cost confidence","value":"Amber — Class 4 estimate, ±30%"},
               {"label":"Delivery capacity","value":"Amber — 2 of 5 sites unstaffed"},
               {"label":"Gate options","value":"Pass, conditional pass, hold"}],
             "decisions":[
               {"key":"gate","prompt":"What do you recommend to the gate panel?",
                "options":[
                  {"key":"pass","label":"Pass — benefits are proven and the ambers will resolve in delivery","quality":15,
                   "consequence":"The ambers travel into delivery unpriced; six months later the cost range and the staffing gap surface as variances nobody owns.",
                   "principle":"A gate that passes known gaps converts governance into ceremony."},
                  {"key":"conditional","label":"Conditional pass — proceed with funded estimate maturation and a staffing plan, both re-reviewed in 90 days","quality":100,
                   "consequence":"Design continues, the two conditions get owners and dates, and the panel keeps a real control point.",
                   "principle":"Conditions with owners and dates keep momentum without surrendering control."},
                  {"key":"hold","label":"Hold — no programme should pass a gate with two ambers","quality":45,
                   "consequence":"Three months of standing-army cost buys information a conditional pass would have bought anyway.",
                   "principle":"A hold is for gaps that delivery would make worse, not for gaps delivery can close."}]}],
             "hints":["Look at what each amber actually needs in order to close — money, people, or time.",
               "Ask which option keeps a genuine control point for the panel rather than ending its involvement.",
               "Compare the cost of pausing everything with the cost of maturing the estimate while design continues."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Recommended a defensible stage-gate position on a multi-site capital programme."}
            """),

        ("WC-GOV-054", "The benefits slide nobody measured", "The sponsor wants the benefits slide to stay. The data behind it left months ago.",
            "Portfolio & PMO", "Portfolio Analyst", "project_management", "foundation", 7,
            """["governance","benefits_management"]""",
            """
            {"context":"Preparing a quarterly portfolio review, you find the flagship initiative still reports the benefits figure from its original business case, although the scope that generated a third of those benefits was descoped two quarters ago. The sponsor, presenting tomorrow, asks you to keep the original figure 'for continuity'.",
             "evidence":[
               {"label":"Reported benefit","value":"Business-case figure, unchanged since approval"},
               {"label":"Descope decision","value":"Approved two quarters ago; removed a benefit-bearing workstream"},
               {"label":"Audience","value":"Investment committee, quarterly review"},
               {"label":"Your role","value":"You compile the pack; the sponsor presents it"}],
             "decisions":[
               {"key":"pack","prompt":"What goes in tomorrow's pack?",
                "options":[
                  {"key":"keep","label":"Keep the original figure — continuity matters and the committee dislikes churn","quality":5,
                   "consequence":"The committee later reconciles benefits to scope and finds the gap; every number you have ever compiled is now suspect.",
                   "principle":"A benefits figure that survives its own scope is not continuity — it is misstatement."},
                  {"key":"restate","label":"Restate the benefit to current scope, with a one-line bridge from the original figure","quality":100,
                   "consequence":"The committee sees a smaller, traceable number and approves the review without drama; the bridge answers the only question they ask.",
                   "principle":"Restate early, bridge visibly: credibility is cheaper to keep than to rebuild."},
                  {"key":"footnote","label":"Keep the figure but add a footnote about the descope","quality":35,
                   "consequence":"Nobody reads the footnote until an auditor does, and then the headline number is the story.",
                   "principle":"Material corrections belong in the number, not beneath it."}]}],
             "hints":["Ask what the committee would decide differently if it knew the current benefits position.",
               "Consider who owns the consequence when the pack's headline number is later shown to be stale.",
               "A bridge from the old number to the new one answers the continuity concern honestly."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept a portfolio benefits figure honest after a descope changed the case."}
            """),

        ("WC-GOV-055", "Two mandates, one road", "The council wants traffic relief. The transport authority wants asset renewal. You have one budget.",
            "Highways", "Project Sponsor's Deputy", "project_management", "foundation", 5,
            """["governance","stakeholder_alignment"]""",
            """
            {"context":"A highway upgrade at business-case stage has two funding sponsors. The council's mandate is congestion relief at the junction; the transport authority's mandate is renewing the life-expired carriageway. The single budget covers one full mandate or a reduced version of both. The business case must name a primary objective this week.",
             "evidence":[
               {"label":"Council mandate","value":"Junction congestion relief"},
               {"label":"Authority mandate","value":"Carriageway renewal (life-expired)"},
               {"label":"Budget","value":"Covers one full mandate, or ~70% of each"},
               {"label":"Deadline","value":"Primary objective due in the case this week"}],
             "decisions":[
               {"key":"objective","prompt":"How do you set the primary objective?",
                "options":[
                  {"key":"split","label":"Blend both at ~70% each and call the objective 'corridor improvement'","quality":20,
                   "consequence":"Every later trade-off reopens the argument, because nobody agreed what the scheme is actually for.",
                   "principle":"A blended objective defers the conflict to every future decision."},
                  {"key":"convene","label":"Convene both sponsors, present the trade-off explicitly, and record one primary and one secondary objective","quality":100,
                   "consequence":"The sponsors rank renewal first with congestion as a constrained secondary; later scope decisions cite the record instead of relitigating it.",
                   "principle":"Sponsors, not the project, must own the ranking of their mandates — in writing."},
                  {"key":"pick","label":"Pick the authority's mandate — they contribute more funding","quality":40,
                   "consequence":"Technically defensible, but the council learns its mandate was demoted from the published case, and its planning cooperation cools.",
                   "principle":"A ranking imposed on a sponsor is a decision they will unmake later."}]}],
             "hints":["Whose decision is the ranking of objectives — yours, or the sponsors'?",
               "Think about which option prevents the same argument recurring at every change board.",
               "A recorded primary-and-secondary structure can honour both mandates without blending them."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Forced a two-sponsor highway scheme to rank its mandates before the case was signed."}
            """),

        ("WC-GOV-056", "The case that kept growing", "Every partner added one requirement. The business case now promises everything and prices nothing.",
            "Joint Ventures", "Business Case Lead", "project_management", "foundation", 6,
            """["governance","scope_discipline"]""",
            """
            {"context":"A joint-venture delivery office is drafting the business case for a shared logistics hub. In three review cycles, each of the four partners has added requirements; the case now carries 40% more scope than the option the feasibility study priced, with the original budget line unchanged. Final sign-off is in two weeks.",
             "evidence":[
               {"label":"Feasibility option","value":"Priced scope, agreed by all partners"},
               {"label":"Current draft","value":"+40% scope, budget unchanged"},
               {"label":"Additions","value":"One or more from each of the four partners"},
               {"label":"Sign-off","value":"Two weeks away"}],
             "decisions":[
               {"key":"case","prompt":"What do you do with the draft?",
                "options":[
                  {"key":"submit","label":"Submit as drafted — the additions have partner support and re-pricing would delay sign-off","quality":10,
                   "consequence":"The case is approved and immediately undeliverable; the first cost report becomes the moment the partnership starts distrusting the office.",
                   "principle":"An unpriced promise approved on schedule is a delay with a signature on it."},
                  {"key":"reprice","label":"Re-price the enlarged scope and present cost-rated additions for the partners to keep or drop","quality":100,
                   "consequence":"Two additions survive at priced cost, the rest return to a phase-2 list; sign-off slips ten days and the approved case is deliverable.",
                   "principle":"Every requirement earns its place by carrying its own price."},
                  {"key":"freeze","label":"Strike all additions and submit the feasibility scope","quality":40,
                   "consequence":"Clean, but four partners learn their requirements were removed without a hearing, and the additions return as day-one change requests.",
                   "principle":"Scope discipline that skips the conversation just relocates the scope fight."}]}],
             "hints":["Compare what each option makes the partners confront now versus after approval.",
               "Ask what an approved but undeliverable case would do to the delivery office's standing.",
               "Pricing each addition turns an argument about opinions into a decision about money."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rescued a joint-venture business case from unpriced scope growth before sign-off."}
            """),

        ("WC-GOV-057", "Sign here, or send it up?", "A supplier wants a commitment today. Your delegation says no — almost.",
            "Energy Networks", "Project Manager", "project_management", "foundation", 7,
            """["governance","delegation_of_authority"]""",
            """
            {"context":"On a grid reinforcement project at business-case stage, the transformer supplier offers to hold current pricing and a delivery slot if a reservation agreement is signed within five working days. The reservation is 4% of project value; your delegated authority at this stage is 3%. Your director is trekking, reachable in roughly four days, and the deputy director is empowered for 'urgent operational matters'.",
             "evidence":[
               {"label":"Offer","value":"Price + slot held if signed within 5 working days"},
               {"label":"Reservation value","value":"4% of project value"},
               {"label":"Your delegation","value":"3% at pre-sanction stage"},
               {"label":"Cover","value":"Director away ~4 days; deputy holds urgent-matters authority"}],
             "decisions":[
               {"key":"authority","prompt":"How do you handle the signature?",
                "options":[
                  {"key":"sign","label":"Sign it yourself — 1% over delegation is trivial against the saving","quality":5,
                   "consequence":"The saving is real, and so is the precedent: your signature above delegation is now a fact in every future audit of the project.",
                   "principle":"Delegation limits are not priced in percentages saved — they are the control itself."},
                  {"key":"deputy","label":"Brief the deputy director today with the value case and obtain their authorised signature","quality":100,
                   "consequence":"The deputy signs within delegation, the slot is held, and the decision trail shows the control working under time pressure.",
                   "principle":"Time pressure is what escalation routes are for; use the route, not the shortcut."},
                  {"key":"wait","label":"Wait for the director — five days probably has slack in it","quality":30,
                   "consequence":"'Probably' turns out to be wrong for the delivery slot, and the project's first schedule risk was created by its own caution.",
                   "principle":"Declining to use an available escalation route is a decision, and it has a price."}]}],
             "hints":["Check exactly what authority exists and who holds it before concluding you are stuck.",
               "Weigh the precedent cost of one over-delegation signature against its one-off saving.",
               "An escalation route that works under pressure is evidence your governance is real."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a delegation limit under supplier time pressure on a grid project."}
            """),

        ("WC-GOV-058", "The baseline that skipped the board", "Delivery re-planned around the slip. The board still holds the old dates.",
            "Technology Programmes", "Programme Office Lead", "project_management", "foundation", 5,
            """["governance","baseline_control"]""",
            """
            {"context":"A platform migration at concept-to-definition transition has slipped eight weeks in its internal plan. The delivery team has quietly re-planned around the slip and reports 'on track' against the new internal dates. The board's approved baseline still shows the original dates. The next board meeting is Thursday.",
             "evidence":[
               {"label":"Approved baseline","value":"Original dates, held by the board"},
               {"label":"Internal plan","value":"Re-planned, 8 weeks later, reported 'on track'"},
               {"label":"Reporting","value":"RAG has stayed green throughout"},
               {"label":"Next board","value":"Thursday"}],
             "decisions":[
               {"key":"report","prompt":"What does Thursday's board see?",
                "options":[
                  {"key":"green","label":"Green against the internal plan — the re-plan absorbed the slip","quality":5,
                   "consequence":"The board discovers months later that 'on track' meant a different track; the programme's reporting is retro-audited line by line.",
                   "principle":"Progress is measured against the baseline the governing body approved, not the one delivery prefers."},
                  {"key":"rebaseline","label":"Report the 8-week variance against the approved baseline and table a formal re-baseline request","quality":100,
                   "consequence":"An uncomfortable meeting, then an approved baseline that means something again — and a board that trusts the next green.",
                   "principle":"A baseline change is the board's decision to make, which is precisely what makes the baseline useful."},
                  {"key":"defer","label":"Report amber with 'schedule under review' and re-baseline next quarter","quality":30,
                   "consequence":"Three months of ambiguity in which every date in the pack means nothing in particular.",
                   "principle":"An unanchored amber postpones the truth without reducing its price."}]}],
             "hints":["Ask which baseline the board believes it is governing against.",
               "Consider what the delivery team's 'on track' would mean to someone holding the approved dates.",
               "A formal re-baseline request is bad news once; unanchored reporting is bad news forever."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put an eight-week slip back in front of the board that owned the baseline."}
            """),

        ("WC-GOV-059", "Yes to the strategy, no to the start date", "The framework fits the strategy perfectly. The delivery teams are already full.",
            "Frameworks & Alliances", "Portfolio Manager", "project_management", "foundation", 6,
            """["governance","capacity_planning"]""",
            """
            {"context":"A framework programme's next tranche — three depot retrofits — aligns squarely with strategy and the client wants all three started this quarter. Your honest capacity assessment: the delivery organisation can properly start one now, a second in ten weeks, and the third only when a contract manager returns from another framework in four months.",
             "evidence":[
               {"label":"Strategic fit","value":"All three retrofits score highest in the portfolio"},
               {"label":"Client ask","value":"Start all three this quarter"},
               {"label":"Capacity","value":"1 now, +1 in 10 weeks, +1 in ~4 months"},
               {"label":"History","value":"Last over-committed tranche delivered late on every job"}],
             "decisions":[
               {"key":"commit","prompt":"What do you commit to?",
                "options":[
                  {"key":"all","label":"Start all three — strategy demands it and capacity usually stretches","quality":10,
                   "consequence":"All three limp; the client's next framework review cites 'systemic delay' and prices your capacity for you.",
                   "principle":"Committing beyond capacity converts a strategy into three apologies."},
                  {"key":"stagger","label":"Propose the staggered start with named teams and dates, and show the client the capacity evidence","quality":100,
                   "consequence":"The client challenges, sees the evidence, and takes the stagger; three jobs deliver on their committed dates.",
                   "principle":"A capacity-honest commitment is the only one strategy can actually cash."},
                  {"key":"outsource","label":"Start all three by bringing in an untested subcontractor for two of them","quality":40,
                   "consequence":"Possible — but you have converted a schedule risk into a delivery-quality risk on a strategic client, unpriced.",
                   "principle":"New capacity is a risk decision, not an arithmetic fix; treat it as one."}]}],
             "hints":["Separate what the strategy wants from what the organisation can staff this quarter.",
               "Ask what evidence would let the client accept a staggered start as commitment rather than reluctance.",
               "Check what happened the last time capacity was assumed rather than assessed."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Matched a framework tranche to real delivery capacity — and kept the client."}
            """),

        ("WC-GOV-060", "Go, no-go, or not yet", "The ground survey is six weeks late. The approval window closes in two.",
            "Bridges & Crossings", "Development Manager", "project_management", "foundation", 7,
            """["governance","decision_quality"]""",
            """
            {"context":"A river crossing scheme is due its investment go/no-go. The geotechnical survey — the main cost-risk evidence — is six weeks from completion, but the funding programme's approval window closes in two weeks; missing it defers the scheme a full year. Preliminary bores at the east bank were consistent with the estimate; the west bank is the unknown.",
             "evidence":[
               {"label":"Approval window","value":"Closes in 2 weeks; next window in 12 months"},
               {"label":"Geotech survey","value":"Complete in ~6 weeks"},
               {"label":"East bank bores","value":"Consistent with estimate"},
               {"label":"West bank","value":"No data yet; foundations are 30% of cost"}],
             "decisions":[
               {"key":"gonogo","prompt":"What do you put to the investment committee?",
                "options":[
                  {"key":"go","label":"Full go — east bank data is encouraging and a year's delay is unacceptable","quality":25,
                   "consequence":"If the west bank disappoints, the scheme re-opens its own approval with a cost increase it predicted and ignored.",
                   "principle":"Encouraging partial evidence is not evidence about the part that matters."},
                  {"key":"conditional","label":"Seek approval in-window with a stated west-bank cost range and a mandatory review point when the survey lands","quality":100,
                   "consequence":"The committee approves against the honest range; six weeks later the review point either confirms or triggers the recorded contingency plan.",
                   "principle":"When the calendar and the evidence disagree, structure the decision — do not pretend the gap away."},
                  {"key":"defer","label":"No-go this window — never approve ahead of the ground truth","quality":40,
                   "consequence":"Defensible, but a year of inflation and a demobilised design team were the price of six weeks of patience the committee might have structured instead.",
                   "principle":"Waiting has a cost line too; compare it before defaulting to delay."}]}],
             "hints":["Identify exactly which cost element the missing evidence covers, and how big it is.",
               "Ask whether the approval can carry a range and a review point instead of a single number.",
               "Price the deferral honestly before treating it as the safe option."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Structured a go/no-go around missing ground data instead of gambling on it."}
            """),

        ("WC-GOV-061", "The gate review with one author", "The line readiness review was written by the line's own project manager. It is glowing.",
            "Industrial Automation", "Manufacturing Programme Lead", "project_management", "foundation", 5,
            """["governance","assurance"]""",
            """
            {"context":"A production line install is at its pre-order gate: sign the equipment order this week and the line lands in the summer shutdown. The readiness review attached to the gate papers is thorough and positive — and written entirely by the project manager who will place the order. No independent eyes have touched it.",
             "evidence":[
               {"label":"Gate","value":"Equipment order commitment (largest single commitment)"},
               {"label":"Review author","value":"The project's own PM"},
               {"label":"Independent check","value":"None"},
               {"label":"Timing","value":"Order this week hits the summer shutdown window"}],
             "decisions":[
               {"key":"assurance","prompt":"How do you treat the gate?",
                "options":[
                  {"key":"accept","label":"Accept the review — the PM knows the project best and the window is real","quality":15,
                   "consequence":"Probably fine — until the one gate where it is not, and the order that needed a challenge got a rubber stamp.",
                   "principle":"Self-review at a commitment gate is optimism with a signature."},
                  {"key":"peer","label":"Commission a 48-hour peer challenge by a PM from another line before signature","quality":100,
                   "consequence":"The peer confirms most of the review and catches one unverified utility assumption — fixed in a day, order signed in-window.",
                   "principle":"Independence at gates is cheap; its absence is only expensive when it matters most."},
                  {"key":"defer","label":"Hold the gate for a full assurance audit next month","quality":30,
                   "consequence":"The audit finds little; the shutdown window is missed, and the line waits half a year for proportionality's sake.",
                   "principle":"Assurance must be scaled to the decision — a gate needs a challenge, not a season."}]}],
             "hints":["Ask who, other than the author, has tested the review's key assumptions.",
               "Scale the assurance to the size and reversibility of the commitment.",
               "A short structured peer challenge preserves both independence and the calendar."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Added independent challenge to an equipment-order gate without losing the shutdown window."}
            """),

        ("WC-GOV-062", "Benefits now, capability later", "Cut the change-management budget and the case looks better. For a while.",
            "Enterprise Transformation", "Transformation Office Analyst", "project_management", "foundation", 6,
            """["governance","benefits_management"]""",
            """
            {"context":"An enterprise transformation's draft business case is 6% over its funding envelope. The sponsor proposes closing the gap by removing the adoption and change-management workstream — 'the tools deliver the benefit; training is a nice-to-have'. The benefits model assumes 80% staff adoption within a year; comparable programmes without funded adoption averaged 45%.",
             "evidence":[
               {"label":"Funding gap","value":"6% over envelope"},
               {"label":"Proposed cut","value":"Adoption & change management workstream"},
               {"label":"Benefits assumption","value":"80% adoption in year 1"},
               {"label":"Comparator data","value":"~45% adoption where adoption was unfunded"}],
             "decisions":[
               {"key":"cut","prompt":"How do you respond to the sponsor's proposal?",
                "options":[
                  {"key":"agree","label":"Take the cut — the case must fit the envelope and tools do drive benefit","quality":10,
                   "consequence":"The case fits; the benefits do not. At 45% adoption the programme underdelivers by more than the cut saved.",
                   "principle":"Cutting the mechanism that delivers the benefit is a benefits cut wearing a cost hat."},
                  {"key":"model","label":"Show the sponsor the benefits model re-run at unfunded-adoption rates, then find the 6% elsewhere together","quality":100,
                   "consequence":"The re-run shows the cut destroys three times its own value; the sponsor trims scope elsewhere and the adoption line survives.",
                   "principle":"Make the case argue with itself: model the cut's benefit impact before accepting its cost saving."},
                  {"key":"split","label":"Halve the adoption budget as a compromise","quality":35,
                   "consequence":"A number that satisfies the meeting and neither funds adoption nor closes the gap — the worst properties of both options.",
                   "principle":"Compromising on arithmetic is not the same as compromising on judgment."}]}],
             "hints":["Trace which workstream the 80% adoption assumption actually depends on.",
               "Re-run the benefits model with the comparator adoption rate before discussing the cut.",
               "Bring the sponsor an alternative source for the 6%, not just an objection."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Defended the benefits engine of a transformation case with its own model."}
            """),

        ("WC-GOV-063", "One estate, two ministries", "The education ministry counts classrooms. The finance ministry counts cost per pupil. Same programme.",
            "Public Estates", "Programme Development Officer", "project_management", "foundation", 6,
            """["governance","stakeholder_alignment"]""",
            """
            {"context":"A schools estate programme's outline case goes to two approvers: the education ministry, whose mandate is places delivered by September intake, and the finance ministry, whose mandate is cost per pupil place. The draft optimises for September — modular construction, 8% higher cost per place. Both approvals are required and each ministry has rejected the other's favoured drafts before.",
             "evidence":[
               {"label":"Education mandate","value":"Places ready for September intake"},
               {"label":"Finance mandate","value":"Cost per pupil place within benchmark"},
               {"label":"Current draft","value":"Modular: hits September, +8% cost per place"},
               {"label":"History","value":"Each ministry has vetoed the other's preference before"}],
             "decisions":[
               {"key":"case","prompt":"How do you take the case to approval?",
                "options":[
                  {"key":"asis","label":"Submit the September-optimised draft to both and let finance object if it wants","quality":15,
                   "consequence":"Finance wants. The case bounces, the redraft misses September anyway, and both mandates are now missed.",
                   "principle":"A case that picks a winner between its approvers usually loses twice."},
                  {"key":"options","label":"Present both ministries a joint options table — modular-for-September vs standard-for-benchmark — with the trade-off priced per school","quality":100,
                   "consequence":"The ministries split the estate: modular where intake pressure is acute, standard elsewhere. Both mandates are met where each matters most.",
                   "principle":"When approvers hold different mandates, sell them the trade-off, not a side."},
                  {"key":"average","label":"Redesign to a mid-cost hybrid that partially hits both targets","quality":35,
                   "consequence":"Every school is slightly late and slightly over benchmark; both ministries approve reluctantly and neither defends the programme later.",
                   "principle":"Meeting two mandates halfway can mean meeting neither."}]}],
             "hints":["Map which schools are actually driven by the September constraint and which are not.",
               "Consider giving the approvers a decision to make together rather than a draft to veto separately.",
               "Price the trade-off per school — portfolios can mix answers that a single project cannot."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a two-ministry veto trap into a priced options decision on a schools estate."}
            """),

        // ───────────── Stakeholder Dilemma · communication & negotiation · practitioner ─────────────

        ("WC-STK-064", "The update that landed badly", "Your milestone email said 'minor slippage'. The Ministry of Justice read 'crisis'.",
            "Justice & Courts", "Stakeholder & Comms Lead", "project_management", "professional", 7,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"On a courts modernisation programme at business-case refresh, your routine update to funders described a pilot court's delayed fit-out as 'minor slippage with no programme impact'. The ministry's private office read it alongside a press story about court backlogs and has asked the SRO 'why the programme is in trouble'. The SRO wants a response plan from you within the hour — and wants to know how a routine line became an incident.",
             "evidence":[
               {"label":"Your line","value":"'Minor slippage, no programme impact' (accurate)"},
               {"label":"Context you missed","value":"Backlog press story the same morning"},
               {"label":"Ministry reaction","value":"Private office asking the SRO if the programme is in trouble"},
               {"label":"Ask","value":"Response plan within the hour"}],
             "decisions":[
               {"key":"respond","prompt":"What is your immediate response plan?",
                "options":[
                  {"key":"correct","label":"Send a fuller written note proving the original line was accurate","quality":30,
                   "consequence":"Accurate and beside the point: the ministry's anxiety was about the press context, which your note does not touch.",
                   "principle":"When a message misfires, the fix is rarely a longer version of the same message."},
                  {"key":"call","label":"Offer the private office a 15-minute call today: the pilot's facts, the backlog question they are actually worried about, and the date of the next hard evidence","quality":100,
                   "consequence":"The call reveals their real concern is a parliamentary question; you give them a defensible line and the temperature drops the same day.",
                   "principle":"Answer the anxiety, not the sentence — and do it in the channel anxiety trusts."},
                  {"key":"escalate","label":"Route everything through the SRO from now on and say nothing further yourself","quality":15,
                   "consequence":"The SRO becomes a bottleneck for routine facts and the ministry learns the programme goes quiet under pressure.",
                   "principle":"Retreating from a stakeholder under stress teaches them the worst possible lesson."}]},
               {"key":"prevent","prompt":"And to stop the next routine line detonating?",
                "options":[
                  {"key":"context","label":"Add a same-day context check: what else lands on this reader today?","quality":100,
                   "consequence":"Two later updates get re-timed or re-framed; neither becomes an incident.",
                   "principle":"A message is read in the reader's day, not the writer's."},
                  {"key":"less","label":"Reduce update frequency so there is less to misread","quality":10,
                   "consequence":"The vacuum fills with corridor versions, which are worse than anything you would have written.",
                   "principle":"Silence is also a message, and you do not control its content."},
                  {"key":"legal","label":"Have every future update reviewed by the legal team","quality":25,
                   "consequence":"Updates become slow, defensive and unread — safe from misreading because nobody reads them.",
                   "principle":"Defensive prose protects the writer, not the relationship."}]}],
             "hints":["Separate what your line said from what the reader was primed to hear that morning.",
               "Ask what the private office actually needs in order to feel in control of the story.",
               "The channel of the repair matters as much as its content — anxious readers trust conversations."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Defused a ministerial misreading of a routine programme update in one afternoon."}
            """),

        ("WC-STK-065", "Forty residents, one unanswerable question", "The depot consultation is tonight. One question has no good answer yet.",
            "Transport Depots", "Community Engagement Manager", "project_management", "professional", 5,
            """["stakeholder_communication","negotiation"]""",
            """
            {"context":"Tonight's consultation on a depot modernisation will draw about forty residents. Your team can answer every expected question except one: night-time construction noise, where the acoustic assessment is three weeks from completion. The residents' association chair — constructive so far — has told you privately that noise is the only question that matters to her members.",
             "evidence":[
               {"label":"Meeting","value":"Tonight, ~40 residents"},
               {"label":"Ready answers","value":"All topics except night-noise"},
               {"label":"Acoustic assessment","value":"3 weeks out"},
               {"label":"Chair's signal","value":"Noise is the only question that matters"}],
             "decisions":[
               {"key":"meeting","prompt":"How do you handle the noise question tonight?",
                "options":[
                  {"key":"assure","label":"Reassure: 'noise will be within statutory limits' — it almost certainly will be","quality":15,
                   "consequence":"Three weeks later the assessment's detail differs from your reassurance in one respect, and the association now audits every sentence you say.",
                   "principle":"A guess dressed as an answer costs more than the pause it saved."},
                  {"key":"frame","label":"Say plainly the assessment is 3 weeks out, commit to a follow-up session on that date, and invite the chair to help set its agenda","quality":100,
                   "consequence":"The room grumbles and accepts; the chair co-owns the follow-up and turns her members from opponents into reviewers.",
                   "principle":"An honest 'not yet, and here is exactly when' outperforms a confident maybe."},
                  {"key":"postpone","label":"Postpone the whole consultation until the assessment is done","quality":30,
                   "consequence":"Three weeks of 'what are they hiding?' — the postponement becomes the story.",
                   "principle":"Cancelling contact to avoid one hard question concedes the narrative entirely."}]},
               {"key":"chair","prompt":"What do you do with the chair's private signal?",
                "options":[
                  {"key":"partner","label":"Meet her before the session and agree how the noise question will be handled tonight","quality":100,
                   "consequence":"She opens the noise discussion herself, framed as 'here is when we get the real answer' — the room follows her lead.",
                   "principle":"A constructive stakeholder offered a real role usually takes your side of the table."},
                  {"key":"ignore","label":"Treat it as one input among many — no special handling","quality":20,
                   "consequence":"Her goodwill was an asset with a shelf life, and tonight was its expiry date.",
                   "principle":"Signals from allies are perishable; unused, they convert to distrust."}]}],
             "hints":["Decide what you can commit to tonight that is entirely within your control.",
               "Think about what role the chair could play in the meeting, not just what she asked.",
               "A dated follow-up with a co-set agenda converts an unanswerable question into a process."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Held a depot consultation steady without the one answer the room wanted."}
            """),

        ("WC-STK-066", "The question you knew was coming", "The planning authority asked exactly what you expected. Your draft answer just became obsolete.",
            "Urban Development", "Consents Manager", "project_management", "professional", 6,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"A mixed-use development is in pre-application discussions. The planning authority has formally asked how the scheme's affordable-housing ratio will survive 'viability review' — the exact question you prepared for. But yesterday your cost consultant advised that regearing since the draft was written has moved the viability model: the prepared answer's ratio is no longer certain. A formal written response is expected within ten days; the relationship with the case officer is good.",
             "evidence":[
               {"label":"Formal question","value":"How does the affordable ratio survive viability review?"},
               {"label":"Prepared answer","value":"Written before the cost update"},
               {"label":"New position","value":"Viability moved; ratio no longer certain"},
               {"label":"Deadline","value":"Written response in 10 days; good case-officer relationship"}],
             "decisions":[
               {"key":"answer","prompt":"What do you send?",
                "options":[
                  {"key":"send","label":"Send the prepared answer — it was right when drafted and re-opening it invites trouble","quality":5,
                   "consequence":"The viability review finds the gap; the authority now treats every submission from you as advocacy rather than evidence.",
                   "principle":"An answer you know is stale is a misrepresentation with a timestamp."},
                  {"key":"call_first","label":"Call the case officer, flag the cost movement honestly, and agree a two-week extension for a re-modelled answer","quality":100,
                   "consequence":"The officer prefers a right answer to a fast one, grants the extension, and privately marks the scheme as one that self-corrects.",
                   "principle":"With regulators, credibility compounds — and it is deposited before it is needed."},
                  {"key":"hedge","label":"Send the prepared answer with a caveat that figures are subject to ongoing review","quality":30,
                   "consequence":"The caveat renders the answer useless for the officer's report while still committing you to the stale ratio politically.",
                   "principle":"A hedged answer often carries the costs of both honesty and error, and the benefits of neither."}]},
               {"key":"internal","prompt":"Your development director wants the original ratio defended 'as a negotiating position'. You:",
                "options":[
                  {"key":"push",   "label":"Explain that a knowingly stale figure to a planning authority is not a position, it is a liability — and propose the re-modelled range as the opening position instead","quality":100,
                   "consequence":"The director accepts once the regulatory risk is spelled out; the re-modelled range still lands the scheme's case credibly.",
                   "principle":"Negotiating room must be built from defensible numbers, not expired ones."},
                  {"key":"comply","label":"Defend the original ratio as instructed — the director owns the relationship risk","quality":15,
                   "consequence":"The instruction was the director's; the signature on the response is yours, and the authority remembers signatures.",
                   "principle":"'I was instructed' has never repaired a professional reputation."}]}],
             "hints":["Ask what the case officer needs your answer FOR — their report has its own audience.",
               "Weigh a ten-day answer that is wrong against a twenty-four-day answer that is right.",
               "Regulatory relationships are repeat interactions; price this response accordingly."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Chose the credible answer over the fast one with a planning authority."}
            """),

        ("WC-STK-067", "The email you should not send", "Third slipped handover. The draft escalation is furious, accurate — and addressed to everyone.",
            "Data Centres", "Delivery Manager", "project_management", "professional", 7,
            """["stakeholder_communication","conflict_management"]""",
            """
            {"context":"On a data-centre build in early works, the power utility has slipped its substation design handover for the third time — six weeks total, now threatening your energisation-driven business case dates. Your draft escalation email is factually accurate and coldly furious, addressed to the utility's programme director and cc'ing your sponsor, their regulator liaison, and both legal teams. Your finger is on send. The utility remains the only possible power provider for this site — and for your next site.",
             "evidence":[
               {"label":"Slippage","value":"3rd slip, 6 weeks cumulative, hits energisation date"},
               {"label":"Draft email","value":"Accurate, angry, cc: sponsor, regulator liaison, legal ×2"},
               {"label":"Dependency","value":"Sole possible power provider — this site and the next"},
               {"label":"Unknown","value":"WHY the utility keeps slipping"}],
             "decisions":[
               {"key":"send","prompt":"What do you do with the draft?",
                "options":[
                  {"key":"fire","label":"Send it — three slips have earned every cc on that list","quality":10,
                   "consequence":"The utility goes formal: every future interaction is minuted, lawyered and slow. You won the email and lost the relationship you depend on twice over.",
                   "principle":"Never escalate in writing what you have not first tried to solve in person — especially with a monopoly counterparty."},
                  {"key":"call","label":"Hold the email; request a same-week principals' meeting to understand why the slips keep happening, with the escalation as your known alternative","quality":100,
                   "consequence":"The meeting surfaces the real cause — their design team is stuck behind another project's regulator query. You re-sequence, they commit a recovery date, and the unsent email did its work as leverage.",
                   "principle":"Diagnose before you detonate: the reason for a slip determines the right response to it."},
                  {"key":"soften","label":"Soften the wording, keep the full cc list, send today","quality":25,
                   "consequence":"Politer words, same blast radius — the cc list, not the tone, was the escalation.",
                   "principle":"An audience is a decision; the cc line escalates harder than the prose."}]},
               {"key":"protect","prompt":"Whatever else happens, your energisation date is exposed. You also:",
                "options":[
                  {"key":"record","label":"Record the dependency slippage formally in the risk register and notify your sponsor through the normal report","quality":100,
                   "consequence":"When the date moves, governance saw it coming through the front door — no ambush, no blame hunt.",
                   "principle":"Escalation and honest reporting are different instruments; the second is never optional."},
                  {"key":"absorb","label":"Quietly re-plan around the slip and keep the report green while the meeting plays out","quality":10,
                   "consequence":"If the recovery fails you will explain both the slip and the silence.",
                   "principle":"Protecting a relationship never justifies blinding your own governance."}]}],
             "hints":["Notice what you do not yet know: the cause of the slips has never been diagnosed.",
               "Count the audiences on the cc line and ask what each will do with the email.",
               "Leverage held in reserve is still leverage; leverage spent in anger is just damage."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Turned a third supplier slip into a diagnosis instead of a war."}
            """),

        ("WC-STK-068", "One pack, two rooms", "The investment committee wants candour. The partner board wants confidence. Same slides, same afternoon.",
            "Portfolio & PMO", "Portfolio Reporting Lead", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"Your quarterly portfolio review pack goes to the internal investment committee at 2pm and, unchanged by tradition, to the external partner board at 4pm. This quarter it contains an honest amber-red on the flagship initiative: recoverable, but only with a decision the internal committee must make today. The partners have co-invested and are contractually entitled to 'material performance information' — but they have never seen a red anything, and your CFO has asked whether the 4pm pack could 'present the position more constructively'.",
             "evidence":[
               {"label":"2pm audience","value":"Internal committee — must decide the recovery today"},
               {"label":"4pm audience","value":"Co-investing partners — entitled to material information"},
               {"label":"The rating","value":"Amber-red, recoverable with today's decision"},
               {"label":"CFO ask","value":"'More constructive' presentation at 4pm"}],
             "decisions":[
               {"key":"packs","prompt":"What does the 4pm pack contain?",
                "options":[
                  {"key":"two","label":"Rebuild the 4pm pack with the amber-red reframed as 'managed watch item'","quality":5,
                   "consequence":"The partners eventually compare packs — co-investors always do — and 'why did ours differ?' becomes a legal question.",
                   "principle":"Two truths for two rooms is one falsehood with witnesses."},
                  {"key":"same_plus","label":"Same facts, same rating in both rooms — with the 4pm pack adding the recovery decision the committee took at 2pm","quality":100,
                   "consequence":"The partners see a red AND the working governance that caught it; two of them offer help rather than complaint.",
                   "principle":"One truth, staged so the second room sees the response as well as the problem."},
                  {"key":"delay","label":"Pull the flagship slide from the 4pm pack and brief the partners after recovery is under way","quality":25,
                   "consequence":"'Material performance information' has a contractual meaning; withholding it for tactical timing is a breach dressed as sequencing.",
                   "principle":"Entitled audiences must not learn material facts on your schedule of convenience."}]},
               {"key":"cfo","prompt":"And your response to the CFO?",
                "options":[
                  {"key":"reframe","label":"Agree that framing matters — and show the CFO a 4pm narrative that is candid AND constructive, then hold the line on the facts","quality":100,
                   "consequence":"The CFO's real concern was tone, not truth; they take the framed-but-honest pack gladly.",
                   "principle":"Most requests to soften facts are actually requests to be helped with the framing."},
                  {"key":"refuse","label":"Refuse flatly and copy the general counsel","quality":30,
                   "consequence":"You win a fight you were not necessarily in and lose a CFO who was probably persuadable.",
                   "principle":"Escalate positions, not conversations you have not finished."}]}],
             "hints":["Establish what the partners are contractually entitled to before deciding what is discretionary.",
               "Sequence is a legitimate tool: what changes between 2pm and 4pm that the partners could see?",
               "Test whether the CFO wants different facts or different framing — they are different requests."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Kept one truth across two boardrooms and let governance do the reassuring."}
            """),

        ("WC-STK-069", "Say it first, say it whole", "The JV's flagship saving is gone. Three partners are about to hear it — from someone.",
            "Joint Ventures", "JV Delivery Office Director", "project_management", "professional", 6,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"Your joint-venture delivery office has confirmed that the shared-procurement saving underpinning this year's JV value story — publicised to all three partner boards — will not materialise: the framework supplier's insolvency removed the discount structure. The gap is 4% of JV operating value. You have known for 48 hours; the quarterly partner call is in five days; your own chair suggests 'folding it into the quarterly narrative'.",
             "evidence":[
               {"label":"Fact","value":"Flagship saving eliminated by supplier insolvency"},
               {"label":"Materiality","value":"~4% of JV operating value; publicised to all 3 boards"},
               {"label":"Known for","value":"48 hours"},
               {"label":"Chair's preference","value":"Fold into the quarterly call in 5 days"}],
             "decisions":[
               {"key":"timing","prompt":"When and how do the partners hear?",
                "options":[
                  {"key":"quarterly","label":"Per the chair: a balanced item inside the quarterly narrative","quality":25,
                   "consequence":"One partner's own supply-chain team spots the insolvency in day two of your five-day wait; that partner now wonders what else waits for a quarterly slot.",
                   "principle":"Material bad news has a discovery clock running that you do not control."},
                  {"key":"now","label":"Brief each partner's nominated director within 24 hours: the fact, the number, the recovery options for the quarterly call","quality":100,
                   "consequence":"All three hear it from you first, framed with options; the quarterly call becomes a recovery discussion instead of an ambush inquiry.",
                   "principle":"Be the first and fullest source of your own bad news — the framing premium goes to whoever speaks first."},
                  {"key":"fix_first","label":"Spend two weeks lining up a replacement procurement route, then announce problem and solution together","quality":35,
                   "consequence":"The 'complete story' gamble: if discovery beats you, the two silent weeks become the story instead of the solution.",
                   "principle":"Solve-then-tell only works when you control the discovery clock — you rarely do."}]},
               {"key":"chair","prompt":"Your chair preferred the quarterly route. You:",
                "options":[
                  {"key":"persuade","label":"Take the discovery-risk argument to the chair privately and get their blessing for the 24-hour briefings","quality":100,
                   "consequence":"The chair, shown the discovery risk, not only agrees but makes two of the calls personally.",
                   "principle":"Chairs change position on evidence; they rarely forgive being bypassed."},
                  {"key":"bypass","label":"Brief the partners anyway — the duty to them outranks the chair's preference","quality":20,
                   "consequence":"Right message, broken authority: the chair's trust in you is now the JV's newest risk.",
                   "principle":"Doing the right thing through the wrong door still breaks the door."}]}],
             "hints":["Estimate who else could discover this independently, and how fast.",
               "Compare the value of a complete story later with a first-source story now.",
               "Your chair is a stakeholder in the method, not just the message — bring them the evidence."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Delivered a JV's worst number of the year first, whole, and with options."}
            """),

        ("WC-STK-070", "The partner who heard it in the market", "Your alliance partner learned about the scope change from a subcontractor. Now they are asking what else they missed.",
            "Downstream Energy", "Turnaround Alliance Manager", "project_management", "professional", 7,
            """["stakeholder_communication","conflict_management"]""",
            """
            {"context":"Planning a refinery turnaround under an alliance contract, your organisation decided last month to pull the exchanger-bundle work from the shared scope into a direct contract — defensible on schedule grounds, and formally within your rights. Nobody told the alliance partner; they heard it this week from a subcontractor who was asked to quote both ways. Their commercial director has requested 'a conversation about transparency', and their planning cooperation — which you need daily — has gone conspicuously slow.",
             "evidence":[
               {"label":"The decision","value":"Exchanger scope pulled to direct contract — contractually permitted"},
               {"label":"The failure","value":"Partner not told; heard via a subcontractor"},
               {"label":"The signal","value":"'A conversation about transparency' + cooperation slowing"},
               {"label":"The dependency","value":"You need their planning input daily for 6 more months"}],
             "decisions":[
               {"key":"meeting","prompt":"How do you open the transparency conversation?",
                "options":[
                  {"key":"rights","label":"Open with the contract: the change was within your rights and no notification was required","quality":10,
                   "consequence":"True, and the partner agrees — then matches you: everything they do for the next six months will also be exactly what the contract requires, no more.",
                   "principle":"Winning on rights while losing on trust is how alliances become contracts again."},
                  {"key":"own","label":"Own the communication failure without relitigating the decision: it was ours to make, and it was also ours to tell you first — then agree a notification protocol for future scope moves","quality":100,
                   "consequence":"The commercial director, braced for a rights argument, gets an apology and a protocol; cooperation resumes within the week.",
                   "principle":"Separate the decision from the discourtesy — defend the first, apologise for the second."},
                  {"key":"deflect","label":"Express regret that 'communications fell through the cracks' and move to the planning agenda","quality":25,
                   "consequence":"A passive-voice apology is filed as no apology; the slow cooperation continues at exactly its current speed.",
                   "principle":"Ownership has a grammar; 'mistakes were made' is not it."}]},
               {"key":"future","prompt":"The partner asks for veto rights over future scope transfers. You:",
                "options":[
                  {"key":"protocol","label":"Decline the veto, offer a binding early-notification and consultation window before any future transfer","quality":100,
                   "consequence":"They wanted the veto to force the conversation; guaranteed early consultation gives them the conversation without giving away your scope rights.",
                   "principle":"Behind most demands for control is a demand to be consulted — price them differently."},
                  {"key":"veto","label":"Concede a limited veto to rebuild goodwill quickly","quality":15,
                   "consequence":"Goodwill rebuilt, flexibility gone: the next schedule-critical transfer now needs their signature.",
                   "principle":"Never repair a communication failure by paying with governance rights."}]}],
             "hints":["Distinguish the decision (defensible) from the way it travelled (not).",
               "Ask what daily behaviour you need back, and which opening makes that behaviour likely.",
               "When remedies are demanded, look for the interest underneath the mechanism."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Repaired an alliance after a scope change travelled by rumour."}
            """),

        ("WC-STK-071", "The corridor promise", "The client director thanked you for agreeing to the extra hospitality boxes. You never did.",
            "Sports & Venues", "Fit-out Project Director", "project_management", "professional", 5,
            """["stakeholder_communication","negotiation"]""",
            """
            {"context":"At a stadium fit-out steering meeting's coffee break, the client director described adding two hospitality boxes to the west stand as 'that small addition we discussed' and thanked you for 'being flexible'. You recall a hypothetical chat weeks ago, not an agreement. The work is ~2% of contract value, unpriced, off-baseline, and the steering meeting — with both organisations' executives — resumes in ten minutes.",
             "evidence":[
               {"label":"The claim","value":"'The small addition we discussed' — framed as agreed"},
               {"label":"Your recollection","value":"A hypothetical conversation, no commitment"},
               {"label":"Value","value":"~2% of contract, unpriced, off-baseline"},
               {"label":"Setting","value":"Executives reconvene in 10 minutes"}],
             "decisions":[
               {"key":"room","prompt":"The meeting resumes. The director repeats the thanks in front of the executives. You:",
                "options":[
                  {"key":"contradict","label":"Correct the record immediately and publicly: no such agreement exists","quality":30,
                   "consequence":"Factually necessary, publicly humiliating — the director spends the rest of the project proving you wrong on everything else.",
                   "principle":"Public correction of a senior stakeholder is sometimes required; it is never free."},
                  {"key":"bridge","label":"Acknowledge the conversation warmly, and route the commitment: 'happy to take the boxes through change control this week so pricing and programme impact are on the table'","quality":100,
                   "consequence":"The director keeps face, the executives hear 'yes, via process', and the unpriced promise becomes a priced change request — which the client then halves.",
                   "principle":"Convert corridor commitments into process without converting the stakeholder into an enemy."},
                  {"key":"silence","label":"Let it pass in the meeting and untangle it privately afterwards","quality":20,
                   "consequence":"The minutes now record executive-level thanks for an agreement; your private untangling starts from a written disadvantage.",
                   "principle":"Silence in the room is ratification in the minutes."}]},
               {"key":"pattern","prompt":"This is the director's third corridor-to-commitment attempt. Longer term you:",
                "options":[
                  {"key":"channel","label":"Agree with the director a single change channel, and confirm every hallway conversation by same-day email","quality":100,
                   "consequence":"Two later 'small additions' arrive as proper change requests; the pattern stops paying and therefore stops.",
                   "principle":"Behaviour you make unprofitable, you make rare."},
                  {"key":"avoid","label":"Avoid informal settings with the director entirely","quality":15,
                   "consequence":"The informal channel is where half the project's real information lives; you just cut yourself off from it.",
                   "principle":"The answer to channel abuse is channel discipline, not channel closure."}]}],
             "hints":["Protect two things at once: the baseline and the stakeholder's face — the best answer costs neither.",
               "Ask what the minutes will say if you say nothing.",
               "Same-day written confirmation is the quiet cure for corridor commitments."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Rerouted an executive corridor promise into change control without a casualty."}
            """),

        // ───────────── Risk Room · risk & uncertainty · practitioner · EMV ─────────────

        ("WC-RSK-072", "The register nobody opened", "Four risks, last reviewed two quarters ago. The committee wants one number and one confession.",
            "Capital Programmes", "Programme Risk Lead", "project_controls", "professional", 9,
            """["risk_management","governance"]""",
            """
            {"context":"Preparing a multi-site capital programme for sanction, you find its risk register untouched for two quarters. Before the investment committee will set contingency, it wants the register's net Expected Monetary Value as it stands — and your professional view on whether the register can be trusted at all.",
             "evidence":[
               {"label":"R1 — Multi-site design rework","value":"probability 0.30, impact -600,000"},
               {"label":"R2 — Utility connection delays","value":"probability 0.25, impact -400,000"},
               {"label":"R3 — Contractor insolvency (single shared frame)","value":"probability 0.10, impact -1,500,000"},
               {"label":"R4 — Early site release bonus","value":"probability 0.50, impact +200,000"},
               {"label":"Register last reviewed","value":"Two quarters ago"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.3,"impact":-600000},{"id":"R2","probability":0.25,"impact":-400000},
               {"id":"R3","probability":0.1,"impact":-1500000},{"id":"R4","probability":0.5,"impact":200000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — design rework","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — contractor insolvency","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"trust","prompt":"And your view on the register itself?",
                "options":[
                  {"key":"present","label":"Present the EMV as computed — the arithmetic is sound","quality":20,
                   "consequence":"The committee funds to a number built on two-quarter-old probabilities; the first materialised risk shows the register was stale, and the contingency with it.",
                   "principle":"EMV inherits the staleness of its inputs; arithmetic cannot refresh judgment."},
                  {"key":"caveat","label":"Present the EMV with its review date, and make sanction conditional on a facilitated register refresh within 30 days","quality":100,
                   "consequence":"The committee sanctions with a dated caveat; the refresh moves two probabilities materially and contingency is corrected before it is spent.",
                   "principle":"A risk number carries its review date or it carries false confidence."},
                  {"key":"refuse","label":"Refuse to present any number until the register is refreshed","quality":40,
                   "consequence":"Principled, but sanction slips a cycle and the committee learns risk management as the function that blocks rather than informs.",
                   "principle":"The perfect register a month late can cost more than the caveated one on time."}]}],
             "hints":["EMV of each line is probability × impact; opportunities carry a positive sign.",
               "The net register EMV is simply the sum of the four lines, threats and opportunities together.",
               "Before trusting any register EMV, check when its probabilities were last challenged."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a stale risk register honestly — number, review date and all."}
            """),

        ("WC-RSK-073", "Drawdown or discipline", "The project wants 40% of the contingency for a risk that has not happened yet.",
            "Energy Networks", "Risk & Contingency Manager", "project_controls", "professional", 10,
            """["risk_management","cost_control"]""",
            """
            {"context":"A substation replacement at sanction holds contingency set at the register's net EMV. Four months from mobilisation, the delivery team requests a drawdown of 40% of contingency to pre-order steelwork — mitigating R1 before it materialises. The panel wants the register's current numbers first: net EMV, the exposure being mitigated, and the largest remaining line.",
             "evidence":[
               {"label":"R1 — Steel price escalation at order date","value":"probability 0.40, impact -500,000"},
               {"label":"R2 — Outage window refusal by system operator","value":"probability 0.20, impact -900,000"},
               {"label":"R3 — Ground contamination in cable route","value":"probability 0.15, impact -2,000,000"},
               {"label":"R4 — Recovered-copper resale opportunity","value":"probability 0.30, impact +150,000"},
               {"label":"Drawdown request","value":"40% of contingency, to pre-order steel now"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.4,"impact":-500000},{"id":"R2","probability":0.2,"impact":-900000},
               {"id":"R3","probability":0.15,"impact":-2000000},{"id":"R4","probability":0.3,"impact":150000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — steel escalation","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — ground contamination","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"drawdown","prompt":"Your recommendation on the drawdown?",
                "options":[
                  {"key":"grant","label":"Grant it — mitigation is what contingency is for","quality":30,
                   "consequence":"The steel is secured; the register still carries the contamination tail with most of its cover gone, which the panel discovers only when asked.",
                   "principle":"Contingency spent on one risk is protection removed from every other."},
                  {"key":"structured","label":"Support the pre-order, funded as a priced change to base scope, with contingency retained against the register that justifies it","quality":100,
                   "consequence":"The mitigation is real, so it earns a place in base cost; contingency stays sized to the risks that remain, and the panel can see both moves.",
                   "principle":"A mitigation you have decided to do is scope, not risk — move the money through the front door."},
                  {"key":"refuse","label":"Refuse — contingency is only for materialised risks","quality":25,
                   "consequence":"Doctrinally tidy; the steel window closes and R1's full impact duly arrives, costing more than the mitigation would have.",
                   "principle":"Contingency doctrine that forbids cheap prevention is guarding the wrong thing."}]}],
             "hints":["Compute each line as probability × impact, keeping the opportunity positive.",
               "Compare R1's expected exposure with the cost of mitigating it — that ratio is the panel's real question.",
               "Ask what stands behind R2 and R3 if 40% of the contingency leaves with R1."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Separated risk mitigation from contingency raiding on an energy project."}
            """),

        ("WC-RSK-074", "The delay that pays", "A supplier slip everyone is mourning may be the best financial news this quarter.",
            "Pharma Facilities", "Project Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","opportunity_management"]""",
            """
            {"context":"A sterile-filling facility at business-case stage learns its isolator supplier will deliver twelve weeks late. The team treats it as pure threat — but the slip also opens two priced opportunities: the cleanroom contractor has offered a discount to fill the idle window with another client's work, and the delayed start allows a cheaper validation season. The case needs the register's honest net position.",
             "evidence":[
               {"label":"R1 — Extended prelims through the idle window","value":"probability 0.35, impact -700,000"},
               {"label":"R2 — Cleanroom contractor window discount","value":"probability 0.60, impact +250,000"},
               {"label":"R3 — Product launch penalty if slip extends past Q3","value":"probability 0.20, impact -1,100,000"},
               {"label":"R4 — Off-peak validation season saving","value":"probability 0.45, impact +180,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.35,"impact":-700000},{"id":"R2","probability":0.6,"impact":250000},
               {"id":"R3","probability":0.2,"impact":-1100000},{"id":"R4","probability":0.45,"impact":180000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R2","label":"EMV of R2 — window discount","type":"number"},
               {"key":"emv_R4","label":"EMV of R4 — validation saving","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"posture","prompt":"How does the case present the slip?",
                "options":[
                  {"key":"threat","label":"As a threat with mitigation — opportunities are speculative and shouldn't dilute the warning","quality":25,
                   "consequence":"The committee prices pure downside; the discount and the season lapse unclaimed because nobody was tasked with capturing them.",
                   "principle":"An unowned opportunity has a probability of zero, whatever the register says."},
                  {"key":"both","label":"As a net position: threats mitigated AND both opportunities assigned owners with capture actions and dates","quality":100,
                   "consequence":"The discount is contracted and the validation slot booked; the slip ends the year cheaper than the original plan.",
                   "principle":"Risk management manages the distribution, not just its left tail."},
                  {"key":"upside","label":"Lead with the silver lining to soften the schedule news","quality":10,
                   "consequence":"The committee hears spin, discounts the genuine opportunities along with the framing, and trust in the register drops.",
                   "principle":"Opportunities earn belief through owners and actions, not through narrative placement."}]}],
             "hints":["Opportunities enter the EMV sum with positive sign — same arithmetic, opposite direction.",
               "Net the four lines before judging whether the slip is bad news overall.",
               "For each opportunity, ask who owns its capture and by when — unowned upside is fiction."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the payable upside inside a twelve-week supplier slip."}
            """),

        ("WC-RSK-075", "Five greens and a cliff", "The heat map shows one red dot. The arithmetic shows where the money really is.",
            "Renewables", "Portfolio Risk Manager", "project_controls", "professional", 11,
            """["risk_management","quantitative_analysis"]""",
            """
            {"context":"A five-site solar portfolio build-out is at final investment decision. The qualitative heat map shows R1 and R2 as the 'big' risks — high probability, visible, much-discussed. R3, a grid-code change that would force inverter retrofits across all five sites, sits in a green corner: 'unlikely'. The committee asks for the register EMV and the two largest single exposures, then wants to know why the heat map and the money disagree.",
             "evidence":[
               {"label":"R1 — Module delivery slippage","value":"probability 0.50, impact -300,000"},
               {"label":"R2 — EPC crew availability","value":"probability 0.40, impact -350,000"},
               {"label":"R3 — Grid-code change forcing retrofits (all sites)","value":"probability 0.08, impact -2,500,000"},
               {"label":"R4 — Land access legal challenge","value":"probability 0.25, impact -800,000"},
               {"label":"R5 — Panel price fall before final order","value":"probability 0.30, impact +120,000"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.5,"impact":-300000},{"id":"R2","probability":0.4,"impact":-350000},
               {"id":"R3","probability":0.08,"impact":-2500000},{"id":"R4","probability":0.25,"impact":-800000},
               {"id":"R5","probability":0.3,"impact":120000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — grid-code retrofits","type":"number"},
               {"key":"emv_R4","label":"EMV of R4 — land access challenge","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"tail","prompt":"What do you tell the committee about the heat map?",
                "options":[
                  {"key":"defend","label":"The heat map is fine — R3 is genuinely unlikely and maps should reflect likelihood","quality":15,
                   "consequence":"The committee funds to the map's intuition; if the code change lands, it costs more than every 'big' risk combined, uncovered.",
                   "principle":"Heat maps rank conversation, not exposure; money follows probability × impact."},
                  {"key":"expose","label":"Show EMV per line beside the map, and recommend a separate tail-risk reserve plus a monitoring trigger for the R3 consultation process","quality":100,
                   "consequence":"The committee sees the cliff behind the greens, funds expected value and tail separately, and tasks someone to watch the code consultation.",
                   "principle":"Low-probability, portfolio-wide impacts deserve their own line of defence, not a green corner."},
                  {"key":"inflate","label":"Raise R3's probability in the register so the map turns amber","quality":5,
                   "consequence":"The map looks right and the register is now dishonest; every future probability is negotiable.",
                   "principle":"Never fix a presentation problem by corrupting the data."}]}],
             "hints":["Compute the EMV line by line before trusting any visual ranking.",
               "Notice which single line dominates the total despite its low probability.",
               "Expected value funds the register's centre; the tail needs its own instrument and its own watcher."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Showed a solar committee where the money hides behind the heat map."}
            """),

        ("WC-RSK-076", "Everyone knew about the gantry", "One shared crane serves six framework projects. Its failure is on nobody's register.",
            "Framework Programmes", "Framework Risk Coordinator", "project_controls", "professional", 9,
            """["risk_management","interface_management"]""",
            """
            {"context":"Reviewing risk across a six-project framework programme at annual re-sanction, you find every project's register is clean — and none of them carries the one risk every project manager mentions in interviews: the single shared launching gantry, whose failure would stop four of the six projects at once. You quantify the programme-level register properly for the first time.",
             "evidence":[
               {"label":"R1 — Gantry major failure (stops 4 projects)","value":"probability 0.15, impact -1,800,000"},
               {"label":"R2 — Design resource contention across projects","value":"probability 0.60, impact -250,000"},
               {"label":"R3 — Common material spec change","value":"probability 0.35, impact -400,000"},
               {"label":"R4 — Shared logistics depot early handback","value":"probability 0.20, impact +300,000"},
               {"label":"Register owner for R1","value":"None — falls between six projects"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.15,"impact":-1800000},{"id":"R2","probability":0.6,"impact":-250000},
               {"id":"R3","probability":0.35,"impact":-400000},{"id":"R4","probability":0.2,"impact":300000}]},
             "ask":[
               {"key":"emv","label":"Net programme EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — gantry failure","type":"number"},
               {"key":"emv_R2","label":"EMV of R2 — resource contention","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"ownership","prompt":"What do you recommend for R1?",
                "options":[
                  {"key":"share","label":"Add R1 to all six project registers so everyone carries it","quality":20,
                   "consequence":"Six registers now show the risk and still nobody owns the gantry's maintenance regime; shared ownership rounds to none.",
                   "principle":"A risk on every register is a risk on no one's desk."},
                  {"key":"programme","label":"Hold R1 once at programme level with a named owner, a funded inspection regime, and a standby-equipment option priced against its EMV","quality":100,
                   "consequence":"The programme director owns it, the inspection regime starts, and the standby option is priced — cheaper than one month of four stopped projects.",
                   "principle":"Cross-cutting risks live where the authority to treat them lives."},
                  {"key":"insure","label":"Transfer it: buy plant breakdown insurance and close the register line","quality":35,
                   "consequence":"The premium covers replacement cost — not four projects' standing time, which the policy excludes and the register no longer shows.",
                   "principle":"Transfer moves the impact you contracted for; the rest stays home, register line or not."}]}],
             "hints":["Work each line's EMV first — including the opportunity's positive contribution.",
               "Compare the frequent small risk with the rare stopping one, in expected-value terms.",
               "For shared assets, ask who has the authority to fund treatment — that is where the risk belongs."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave a six-project framework's biggest shared risk a single owner and a price."}
            """),

        ("WC-RSK-077", "Accepted, says who", "The register's biggest line is marked 'accepted'. No one can say who accepted it.",
            "Technology Rollouts", "Programme Assurance Analyst", "project_controls", "professional", 11,
            """["risk_management","governance"]""",
            """
            {"context":"A national point-of-sale rollout is at readiness review. The register's largest threat — legacy store wiring failing survey across the estate — is marked 'response: accept', but no acceptance record exists: no name, no date, no rationale. The review panel asks for the quantified position, then for your view on the acceptance.",
             "evidence":[
               {"label":"R1 — Legacy wiring fails survey at scale","value":"probability 0.45, impact -600,000"},
               {"label":"R2 — Installer availability in peak season","value":"probability 0.30, impact -500,000"},
               {"label":"R3 — Payment-scheme certification slip","value":"probability 0.12, impact -1,600,000"},
               {"label":"R4 — Bulk hardware price break","value":"probability 0.50, impact +100,000"},
               {"label":"R1 status","value":"'Accepted' — no name, no date, no rationale"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.45,"impact":-600000},{"id":"R2","probability":0.3,"impact":-500000},
               {"id":"R3","probability":0.12,"impact":-1600000},{"id":"R4","probability":0.5,"impact":100000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — wiring failures","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — certification slip","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"accept","prompt":"Your view on R1's 'accepted' status?",
                "options":[
                  {"key":"stand","label":"Let it stand — acceptance is a legitimate response and the rollout is time-critical","quality":10,
                   "consequence":"The wiring risk duly materialises mid-rollout; the search for who accepted it finds only your review, which let it stand.",
                   "principle":"An acceptance without an owner is just an unmanaged risk with better paperwork."},
                  {"key":"reopen","label":"Void the orphan acceptance; require a named owner to re-accept with rationale at the right authority level, or fund a survey-first pilot as mitigation","quality":100,
                   "consequence":"Confronted with its own EMV, the sponsor declines to accept; a 40-store survey pilot re-prices the probability with data and the response is finally a decision.",
                   "principle":"Accepting a risk is an authority decision — it has a name, a date, a rationale and a review trigger, or it has not happened."},
                  {"key":"mitigate","label":"Overrule to 'mitigate' yourself and add survey costs to the plan","quality":35,
                   "consequence":"Possibly the right response, imposed by the wrong person: assurance just took over a decision that belonged to the risk owner.",
                   "principle":"Assurance challenges decisions; it must not quietly make them."}]}],
             "hints":["Quantify all four lines first — the acceptance question needs the EMV in front of it.",
               "Ask what evidence sits behind the 0.45 — a pilot survey converts opinion into data.",
               "Acceptance is legitimate only as an authorised, recorded, reviewable decision."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Voided an orphan risk acceptance and re-priced it with evidence."}
            """),

        // ───────────── Schedule Strategy · schedule & planning · practitioner · CPM ─────────────

        ("WC-CPM-078", "The float that was already spent", "The landowner wants the culvert crew for two extra weeks. Whether you can say yes lives in the network.",
            "Flood Defence", "Senior Planner", "project_controls", "professional", 12,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A flood defence scheme's enabling phase has six activities. A neighbouring landowner has asked to keep the culvert crew (activity C) on their land two extra weeks for a private works favour the sponsor would love to grant. The sponsor asks: 'C has float, doesn't it?' Before answering, run the network — and remember the embankment fill (activity E) is already consuming C's path.",
             "evidence":[
               {"label":"A — Site establishment","value":"4 days, no predecessors"},
               {"label":"B — Access road","value":"6 days, after A"},
               {"label":"C — Culvert works","value":"5 days, after A"},
               {"label":"D — Sheet piling","value":"7 days, after B"},
               {"label":"E — Embankment fill","value":"4 days, after C"},
               {"label":"F — Phase handover","value":"3 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":4,"preds":[]},{"id":"B","dur":6,"preds":["A"]},{"id":"C","dur":5,"preds":["A"]},
               {"id":"D","dur":7,"preds":["B"]},{"id":"E","dur":4,"preds":["C"]},{"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Phase duration (days)","type":"number"},
               {"key":"float_C","label":"Total float of C — culvert works (days)","type":"number"},
               {"key":"float_E","label":"Total float of E — embankment fill (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"favour","prompt":"So what do you tell the sponsor about the two extra weeks?",
                "options":[
                  {"key":"yes","label":"Yes — C is not on the critical path, so the favour is free","quality":10,
                   "consequence":"Ten working days of extension against four days of float: the fill, then handover, then the in-river consent window all slide.",
                   "principle":"'Has float' is a quantity, not a permission — spend beyond it and the path moves."},
                  {"key":"quantified","label":"Show the float arithmetic: the path through C and E carries four days — offer four, not ten, or re-sequence at the landowner's cost","quality":100,
                   "consequence":"The sponsor grants a four-day favour with the numbers in hand, and the landowner pays for the crane move that buys the rest.",
                   "principle":"Float is a budget; answer requests against it with its balance, not its existence."},
                  {"key":"no","label":"No — never lend float to third parties","quality":30,
                   "consequence":"Safe for the schedule, expensive for the sponsor's landowner relationship — which the scheme needs at the next consent.",
                   "principle":"A blanket 'no' protects the plan and starves the relationships the plan depends on."}]}],
             "hints":["Compute both paths through the network to F before answering anything.",
               "Total float of C is how late C can finish without moving F — check the path C→E→F against B→D.",
               "Answer the favour in float-days: what is free, what costs, and who pays for the difference."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a sponsor's favour in float-days on a flood defence scheme."}
            """),

        ("WC-CPM-079", "Two paths, both critical", "The steering group wants to crash the mechanical line. The network says that buys nothing.",
            "Industrial Plants", "Planning Engineer", "project_controls", "professional", 10,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A plant expansion's definition-stage schedule has six activities. The steering group, watching the mechanical procurement line (activity B), proposes paying its supplier for expedited delivery to pull the completion date in. Run the network first: this schedule has a property the steering group has not noticed.",
             "evidence":[
               {"label":"A — Detailed design freeze","value":"3 days, no predecessors"},
               {"label":"B — Mechanical long-leads","value":"5 days, after A"},
               {"label":"C — Civils package","value":"6 days, after A"},
               {"label":"D — Mechanical install plan","value":"4 days, after B"},
               {"label":"E — Civils mobilisation","value":"3 days, after C"},
               {"label":"F — Integrated readiness review","value":"2 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},{"id":"B","dur":5,"preds":["A"]},{"id":"C","dur":6,"preds":["A"]},
               {"id":"D","dur":4,"preds":["B"]},{"id":"E","dur":3,"preds":["C"]},{"id":"F","dur":2,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Schedule duration (days)","type":"number"},
               {"key":"float_B","label":"Total float of B — mechanical long-leads (days)","type":"number"},
               {"key":"float_C","label":"Total float of C — civils package (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"crash","prompt":"Your advice on the expedite payment?",
                "options":[
                  {"key":"pay","label":"Pay it — mechanical is on the critical path, so days bought there are days saved","quality":15,
                   "consequence":"Days are bought on one of TWO critical paths; the civils path holds the date exactly where it was, and the expedite fee bought a report line.",
                   "principle":"Crashing one of two parallel critical paths moves money, not milestones."},
                  {"key":"both_or_neither","label":"Show both paths are critical: compression must pair mechanical AND civils moves, or the money stays in the budget","quality":100,
                   "consequence":"The steering group sees the twin paths, pairs the expedite with an early civils mobilisation, and the date actually moves.",
                   "principle":"Compression is bought path by path — every parallel critical path must shorten together."},
                  {"key":"buffer","label":"Advise spending the money on schedule contingency instead of compression","quality":35,
                   "consequence":"Prudent but unresponsive: the group asked how to get earlier, and float-buying answers a different question.",
                   "principle":"Contingency and compression are both legitimate — but they are answers to different questions."}]}],
             "hints":["Compute the duration of each path through the network separately.",
               "A schedule can have more than one critical path — check the floats on both branches.",
               "Money spent crashing must shorten EVERY currently-critical path to move the end date."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Stopped a steering group paying to crash one of two critical paths."}
            """),

        // ───────────── Cost & Value · cost & commercial · foundation · EVM ─────────────

        ("WC-EVM-080", "The pilot that flattered the case", "The pilot phase is 'basically on plan', says the draft business case. The numbers say otherwise.",
            "Enterprise Transformation", "Assistant Cost Analyst", "project_controls", "foundation", 8,
            """["earned_value","cost_control"]""",
            """
            {"context":"An enterprise transformation's funded pilot phase is the evidence base for the full-rollout business case now being drafted. The case's authors describe the pilot as 'broadly on plan'. You pull the period data: Planned Value 1,200,000; Earned Value 1,050,000; Actual Cost 1,150,000; pilot Budget at Completion 4,800,000. The case's cost model scales pilot performance across the rollout, so what these numbers honestly say matters far beyond the pilot.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"1,200,000"},
               {"label":"Earned Value (EV)","value":"1,050,000"},
               {"label":"Actual Cost (AC)","value":"1,150,000"},
               {"label":"Pilot BAC","value":"4,800,000"},
               {"label":"Draft case wording","value":"'Pilot broadly on plan'"}],
             "task":"evm","given":{"pv":1200000,"ev":1050000,"ac":1150000,"bac":4800000},
             "ask":[
               {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
               {"key":"cv","label":"Cost Variance (CV)","type":"number"},
               {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"case","prompt":"What do you tell the business-case authors?",
                "options":[
                  {"key":"soften","label":"Let 'broadly on plan' stand — the variances are modest and the case has momentum","quality":10,
                   "consequence":"The rollout model inherits the pilot's optimism at scale; the gap that was six figures in the pilot becomes eight in delivery.",
                   "principle":"An error scaled by a business case multiplies; honesty is cheapest at pilot size."},
                  {"key":"indices","label":"Give them the measured indices and insist the rollout cost model scale from measured CPI, not from plan","quality":100,
                   "consequence":"The case's cost line rises defensibly; the committee funds a rollout that can actually be delivered at its stated number.",
                   "principle":"Pilots exist to replace assumptions with measurements — use the measurement."},
                  {"key":"wait","label":"Suggest waiting another period for the pilot trend to 'settle' before updating the case","quality":30,
                   "consequence":"The case drafts on regardless with the flattering line inside; your later correction now fights an approved narrative.",
                   "principle":"Data you have beats data you hope for — report what is measured, when it is measured."}]}],
             "hints":["SV is EV minus PV; CV is EV minus AC — signs tell you which way each story leans.",
               "The indices are ratios: SPI = EV/PV and CPI = EV/AC; below 1.0 means behind or over.",
               "Ask what the rollout model assumes about efficiency — then compare it with the measured CPI."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Stopped a pilot's optimism scaling into a full-rollout business case."}
            """),
    };
}
