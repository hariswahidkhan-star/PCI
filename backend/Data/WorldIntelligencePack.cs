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

    /// <summary>Codes of every Year-1 pack item — the tests' authoritative list.</summary>
    public static IEnumerable<string> Codes => Items.Select(i => i.Code);

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

        // ═════════════ FEBRUARY — scope, requirements and stakeholder alignment ═════════════
        // ───────────── Logic & Sequence · scope & requirements · foundation ─────────────

        ("WC-SCO-081", "The requirement nobody owned", "Every workstream assumed another one had it. Roll up the structure and find the hole.",
            "Capital Programmes", "Requirements Analyst", "project_controls", "foundation", 6,
            """["scope_structuring","requirements_management"]""",
            """
            {"context":"A multi-site capital programme is closing its definition phase. The interface-management requirement appears in the requirements register but in no work package below. Roll up the draft WBS to check the budget, test the 100% rule, and decide what happens to the orphan requirement.",
             "evidence":[
               {"label":"1.1 Programme design","value":"300,000"},
               {"label":"1.2 Site delivery (parent)","value":"—"},
               {"label":"1.2.1 North sites","value":"520,000"},
               {"label":"1.2.2 South sites","value":"260,000"},
               {"label":"1.3 Commissioning support","value":"140,000"},
               {"label":"Orphan","value":"Interface management — in the register, in no package"}],
             "task":"wbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"Programme"},
               {"id":"1.1","parent":"1","name":"Design","value":300000},
               {"id":"1.2","parent":"1","name":"Site delivery"},
               {"id":"1.2.1","parent":"1.2","name":"North","value":520000},
               {"id":"1.2.2","parent":"1.2","name":"South","value":260000},
               {"id":"1.3","parent":"1","name":"Commissioning","value":140000}]},
             "ask":[
               {"key":"root_total","label":"Programme budget (root roll-up)","type":"number"},
               {"key":"hundred_percent_valid","label":"Does the WBS satisfy the 100% rule? (yes/no)","type":"bool"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"orphan","prompt":"The orphan interface requirement — what is the correct sequence of actions?",
                "options":[
                  {"key":"assign","label":"Assign it to a package with a named owner and budget, THEN baseline, THEN open cost accounts","quality":100,
                   "consequence":"The requirement has an address before any money moves; the register and the WBS reconcile line for line.",
                   "principle":"Structure first, baseline second, spend third — the order is the control."},
                  {"key":"baseline","label":"Baseline now to hold the date, assign the orphan in the first change cycle","quality":25,
                   "consequence":"The baseline is born incomplete; the first change cycle starts with a correction rather than a change.",
                   "principle":"A baseline that omits known scope is a schedule for surprise."},
                  {"key":"note","label":"Record it as an assumption and move on — interfaces always sort themselves out","quality":0,
                   "consequence":"Two site teams each assume the other holds the interface budget; the gap surfaces as a claim.",
                   "principle":"An unowned requirement is billed by whoever finds it first."}]}],
             "hints":["Sum the leaf packages — parents roll up from children, never the other way around.",
               "The 100% rule asks whether every child's value is fully captured under its parent structure.",
               "For the orphan, think about which step must come first for the register and WBS to reconcile."],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the unowned requirement in a programme WBS before it billed anyone."}
            """),

        ("WC-SCO-082", "One deliverable, three definitions", "The client, the designer and the operator each signed a different sentence.",
            "Enterprise Programmes", "Scope Manager", "project_management", "foundation", 7,
            """["requirements_management","scope_discipline"]""",
            """
            {"context":"During definition of an enterprise reporting platform, 'the executive dashboard' is a deliverable in three signed documents — with three different definitions. The client's contract says 'real-time performance dashboard'; the design specification says 'daily-refreshed KPI summary'; the operator's service agreement says 'monthly board pack automation'. All three parties believe their version is the deliverable.",
             "evidence":[
               {"label":"Contract","value":"'Real-time performance dashboard'"},
               {"label":"Design spec","value":"'Daily-refreshed KPI summary'"},
               {"label":"Service agreement","value":"'Monthly board pack automation'"},
               {"label":"Stage","value":"Definition — build has not started"}],
             "decisions":[
               {"key":"sequence","prompt":"What is the right order of moves?",
                "options":[
                  {"key":"converge","label":"Table the three texts side by side → facilitate one agreed definition → amend all three documents → then let design proceed","quality":100,
                   "consequence":"One uncomfortable workshop now; one definition everywhere; the build starts against a single sentence.",
                   "principle":"Reconcile definitions where they are cheapest — before anything is built to the wrong one."},
                  {"key":"design","label":"Let the design spec govern — it is the most detailed — and fix the other documents at handover","quality":20,
                   "consequence":"The client discovers at acceptance that 'real-time' quietly became 'daily'; the argument now has an invoice attached.",
                   "principle":"The most detailed document is not automatically the agreed one."},
                  {"key":"defer","label":"Build the daily version as a 'phase 1' and treat the differences as future enhancements","quality":35,
                   "consequence":"Pragmatic-sounding, but nobody agreed the phasing — three parties still hold three expectations, now with dates.",
                   "principle":"Phasing is a scope decision; unilateral phasing is a scope dispute on a timer."}]},
               {"key":"record","prompt":"Once converged, where does the single definition live?",
                "options":[
                  {"key":"dictionary","label":"In the WBS dictionary entry, referenced by contract, spec and service agreement alike","quality":100,
                   "consequence":"Every later document points at one source; the next ambiguity has nowhere to hide.",
                   "principle":"One definition, one home, many references."},
                  {"key":"minutes","label":"In the workshop minutes","quality":15,
                   "consequence":"Minutes are read once; contracts are read at every dispute.",
                   "principle":"A definition that lives in minutes dies in a claim."}]}],
             "hints":["Compare what each document's author would expect at acceptance — that is where the versions collide.",
               "Ask which sequencing makes the disagreement visible while it is still cheap to resolve.",
               "A converged definition needs one authoritative home that all three documents cite."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Converged three signed definitions of one deliverable before build began."}
            """),

        ("WC-SCO-083", "In scope, out of scope, says who", "Six items from the workshop flipchart. Each needs an address before Friday.",
            "Residential Development", "Assistant Project Manager", "project_management", "foundation", 5,
            """["scope_discipline","requirements_management"]""",
            """
            {"context":"A mixed-use development's scope workshop ended with six flipchart items nobody classified. The scope statement freezes Friday. Your job is to classify each item — in scope, out of scope, or needs a decision — and to route the ones that need deciding.",
             "evidence":[
               {"label":"1","value":"Public realm landscaping to the site boundary"},
               {"label":"2","value":"Landscaping BEYOND the boundary the council hinted at"},
               {"label":"3","value":"Tenant fit-out of the retail units"},
               {"label":"4","value":"Utility diversions discovered in survey"},
               {"label":"5","value":"A marketing suite the sales team assumed"},
               {"label":"6","value":"EV charging — in the planning condition, in nobody's budget"}],
             "decisions":[
               {"key":"classify","prompt":"Which classification is right for the ambiguous items?",
                "options":[
                  {"key":"routed","label":"2 and 5 → sponsor decision this week; 6 → in scope (planning condition binds); 3 → explicit exclusion; 1 and 4 → in scope","quality":100,
                   "consequence":"Friday's scope statement carries decisions, exclusions and inclusions — not silence.",
                   "principle":"Every item gets exactly one of three addresses: in, out, or decided-by-name-and-date."},
                  {"key":"generous","label":"Include everything — descoping later is easier than adding","quality":10,
                   "consequence":"The budget absorbs a marketing suite and off-site landscaping nobody approved; descoping later means disappointing people in writing.",
                   "principle":"Scope added silently is budget spent silently."},
                  {"key":"strict","label":"Exclude everything ambiguous — the workshop should have been clearer","quality":25,
                   "consequence":"The planning-condition EV charging is now formally out of scope, which the council will enjoy pointing out.",
                   "principle":"Obligations do not become optional by being ambiguous."}]},
               {"key":"exclusions","prompt":"How do the exclusions appear in the scope statement?",
                "options":[
                  {"key":"named","label":"Named, with the reason and the owning party for each","quality":100,
                   "consequence":"Nobody later claims the retail fit-out was 'obviously included' — the sentence that says otherwise has a date on it.",
                   "principle":"An exclusion is only real if it is written where the reader of the claim will look."},
                  {"key":"blanket","label":"A blanket line: 'anything not listed is excluded'","quality":30,
                   "consequence":"Legally comforting, practically useless — every stakeholder still believes their item was listed in spirit.",
                   "principle":"Blanket exclusions exclude nothing anyone actually expected."}]}],
             "hints":["Sort the six into obligations, assumptions, and wishes before classifying.",
               "Check which items are bound by external commitments — those are not yours to exclude.",
               "For each ambiguous item, name who decides and by when — that is also a classification."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave six orphan scope items an address before the statement froze."}
            """),

        ("WC-SCO-084", "Where the project ends and the asset begins", "The fit-out and the operator disagree about one wall's worth of scope.",
            "Corporate Real Estate", "Project Controls Analyst", "project_controls", "foundation", 6,
            """["scope_structuring","interface_management"]""",
            """
            {"context":"A headquarters fit-out's WBS is being finalised. The operator's facilities team maintains the building systems after handover — and both budgets currently include the access-control head-end: the project WBS under Systems, and the operator's asset plan as 'day-one upgrade'. Roll up the WBS, check the 100% rule, and fix the boundary.",
             "evidence":[
               {"label":"1.1 Enabling works","value":"180,000"},
               {"label":"1.2 Fit-out (parent)","value":"—"},
               {"label":"1.2.1 Interiors","value":"340,000"},
               {"label":"1.2.2 Systems","value":"410,000"},
               {"label":"1.3 Migration","value":"90,000"},
               {"label":"Conflict","value":"Access-control head-end funded in BOTH budgets"}],
             "task":"wbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"HQ fit-out"},
               {"id":"1.1","parent":"1","name":"Enabling","value":180000},
               {"id":"1.2","parent":"1","name":"Fit-out"},
               {"id":"1.2.1","parent":"1.2","name":"Interiors","value":340000},
               {"id":"1.2.2","parent":"1.2","name":"Systems","value":410000},
               {"id":"1.3","parent":"1","name":"Migration","value":90000}]},
             "ask":[
               {"key":"root_total","label":"Project budget (root roll-up)","type":"number"},
               {"key":"hundred_percent_valid","label":"Does the WBS satisfy the 100% rule? (yes/no)","type":"bool"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"boundary","prompt":"The double-funded head-end — what fixes the boundary?",
                "options":[
                  {"key":"agree","label":"Agree the boundary with the operator in writing, keep it in ONE budget, and record the handover interface in both WBS dictionaries","quality":100,
                   "consequence":"One funder, one owner, one dated interface record — and a freed budget line on the other side.",
                   "principle":"Every scope boundary is an agreement between two structures, recorded in both."},
                  {"key":"keep","label":"Keep it in both — redundancy is safer than a gap","quality":10,
                   "consequence":"Double funding survives until an auditor adds the two budgets together; the correction lands mid-delivery.",
                   "principle":"Double-funded scope is a gap wearing a disguise — the money is real, the control is not."},
                  {"key":"drop","label":"Drop it from the project — the operator claimed it, let them have it","quality":30,
                   "consequence":"The operator's 'day-one upgrade' turns out to assume the project delivers the containment first; the gap appears at handover.",
                   "principle":"Conceding a boundary without mapping its dependencies just moves the gap downstream."}]}],
             "hints":["Roll up the leaves; the parent rows carry no value of their own.",
               "The 100% rule is about the project's own structure — the operator's plan is a separate structure with its own rule.",
               "A boundary is fixed when both sides' documents point at the same dated agreement."],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Un-double-funded a scope boundary between a fit-out and its future operator."}
            """),

        ("WC-SCO-085", "The exclusion that came back", "Descoped in the business case. Reappeared in the design review. Now what?",
            "Energy Networks", "Change & Scope Coordinator", "project_controls", "foundation", 7,
            """["scope_discipline","change_control"]""",
            """
            {"context":"A grid reinforcement scheme formally excluded the second transformer bay at business-case stage — recorded, signed, and priced out. Six months later the design review 'assumes' the second bay's civil works 'to avoid abortive work later'. The designer has already drawn it. The sponsor is sympathetic. The exclusion is still in force.",
             "evidence":[
               {"label":"Business case","value":"Second bay EXCLUDED — signed decision"},
               {"label":"Design review","value":"Second-bay civils 'assumed' and drawn"},
               {"label":"Designer's argument","value":"'Avoids abortive work later'"},
               {"label":"Sponsor","value":"Sympathetic but hasn't decided anything"}],
             "decisions":[
               {"key":"route","prompt":"What is the correct sequence for the returning scope?",
                "options":[
                  {"key":"change","label":"Stop the drawing → raise a change with the civils priced and the 'abortive work avoided' benefit quantified → sponsor decides → then design","quality":100,
                   "consequence":"The sponsor approves a priced, deliberate reversal — or doesn't. Either way the baseline and the drawings agree.",
                   "principle":"Excluded scope re-enters through the same gate it left by — priced, decided, recorded."},
                  {"key":"drift","label":"Let the design continue — the change paperwork can ratify what engineering has already judged sensible","quality":5,
                   "consequence":"The 'assumption' hardens into steel and concrete; the change board is eventually asked to approve history.",
                   "principle":"Design that precedes decision converts governance into stenography."},
                  {"key":"strip","label":"Instruct the designer to strip the second bay from the drawings immediately, no discussion","quality":35,
                   "consequence":"Compliant — and the possibly-genuine 'abortive work' argument dies unexamined, maybe expensively.",
                   "principle":"Enforcing the baseline and evaluating a good idea are not mutually exclusive."}]},
               {"key":"root","prompt":"Why do exclusions keep 'coming back' on this scheme?",
                "options":[
                  {"key":"visibility","label":"Exclusions aren't visible where designers work — put them in the design brief and review checklist, not just the business case","quality":100,
                   "consequence":"The next designer sees the exclusion beside the requirement, not six documents away.",
                   "principle":"Controls live where the work happens, or they do not live."},
                  {"key":"discipline","label":"Designers keep overstepping — escalate the individual to their manager","quality":15,
                   "consequence":"One designer chastened; the systemic invisibility of exclusions untouched; the next 'assumption' is already drawn.",
                   "principle":"Blaming the person leaves the process exactly as it found them."}]}],
             "hints":["Check what status the exclusion legally has right now — it is still a signed decision.",
               "Separate the process question (how scope re-enters) from the merits question (is the bay worth it).",
               "Ask where a designer would have had to look to know about the exclusion — that is the real defect."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Routed a returning exclusion back through the gate it left by."}
            """),

        ("WC-SCO-086", "Acceptance criteria, after the fact", "The module is built. Now the client wants to write what 'done' means.",
            "Software Delivery", "Delivery Lead", "project_management", "foundation", 5,
            """["requirements_management","quality_management"]""",
            """
            {"context":"On a platform migration, the reporting module was built from a two-line requirement: 'migrate all standard reports with equivalent functionality'. At handover the client's new operations manager produces a page of acceptance criteria — written last week — including three behaviours the legacy system never had. The team is due to demo tomorrow.",
             "evidence":[
               {"label":"Original requirement","value":"'Migrate all standard reports with equivalent functionality'"},
               {"label":"New criteria","value":"One page, written post-build"},
               {"label":"Delta","value":"3 behaviours the legacy system never had"},
               {"label":"Demo","value":"Tomorrow"}],
             "decisions":[
               {"key":"handle","prompt":"How do you handle tomorrow?",
                "options":[
                  {"key":"split","label":"Demo against 'equivalent functionality' evidence; log the 3 new behaviours as change candidates; agree criteria-before-build for every remaining module","quality":100,
                   "consequence":"The module passes what was asked; the new asks get priced honestly; the process leak is plugged for the rest of the programme.",
                   "principle":"Accept against what was agreed; price what was added; fix why it was late."},
                  {"key":"absorb","label":"Quietly build the 3 behaviours before the demo — they're small and the relationship matters","quality":15,
                   "consequence":"Tomorrow goes smoothly; the precedent that criteria can arrive post-build, free of charge, costs you every remaining module.",
                   "principle":"Absorbing unpriced scope buys one good meeting at compound interest."},
                  {"key":"refuse","label":"Reject the document entirely — acceptance criteria written after build have no standing","quality":30,
                   "consequence":"Contractually crisp; the operations manager who will run this system for years is now your opponent at every demo.",
                   "principle":"Standing on process is sometimes right and rarely sufficient."}]},
               {"key":"forward","prompt":"For the remaining modules, 'done' is defined:",
                "options":[
                  {"key":"before","label":"Jointly, before build starts, signed by the person who will accept","quality":100,
                   "consequence":"Every later demo argues about evidence, not definitions.",
                   "principle":"Acceptance criteria are requirements — they exist before the work or they arrive as changes."},
                  {"key":"template","label":"By your team, from a standard template, issued for information","quality":25,
                   "consequence":"Criteria exist and bind nobody; the next operations manager writes their own page again.",
                   "principle":"Criteria the acceptor didn't sign are opinions with formatting."}]}],
             "hints":["Separate what was agreed pre-build from what arrived post-build — they have different price tags.",
               "Consider who has to live with the system, and what tomorrow costs their trust either way.",
               "The durable fix is about WHEN criteria get written, and by whom."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Separated built-to-spec from added-last-week at a migration handover."}
            """),

        ("WC-SCO-087", "The backlog that ate the baseline", "Two hundred 'small' items, one fixed envelope, and a definition phase that will not close.",
            "Framework Programmes", "Programme Planner", "project_management", "foundation", 6,
            """["scope_discipline","prioritization"]""",
            """
            {"context":"A framework programme's definition phase has collected a 200-item improvement backlog from workshops across five client departments. Every item is individually 'small'; collectively they are half the programme's envelope. The definition phase cannot close until the backlog has a disposition, and the departments are watching which items survive.",
             "evidence":[
               {"label":"Backlog","value":"~200 items from 5 departments"},
               {"label":"Collective size","value":"~50% of the envelope"},
               {"label":"Gate condition","value":"Definition cannot close without a disposition"},
               {"label":"Politics","value":"Departments tracking whose items survive"}],
             "decisions":[
               {"key":"triage","prompt":"What disposition process closes the phase honestly?",
                "options":[
                  {"key":"criteria","label":"Publish scoring criteria first → score all 200 with department reps in the room → fund the top slice to the envelope → park the rest in a governed, revisitable backlog","quality":100,
                   "consequence":"Departments argue about criteria once instead of about items two hundred times; the parked items have a real route back.",
                   "principle":"Prioritise by published criteria, not by meeting stamina."},
                  {"key":"fair","label":"Give each department an equal share of the envelope to spend on its own items","quality":30,
                   "consequence":"Politically peaceful; the programme funds each department's pet mediocrity while cross-cutting winners die of shared parentage.",
                   "principle":"Equal shares is a peace treaty, not a prioritisation."},
                  {"key":"defer","label":"Close the phase with the backlog attached 'for delivery to absorb'","quality":5,
                   "consequence":"Delivery inherits 200 unpriced expectations; the baseline is eaten from day one, one small item at a time.",
                   "principle":"A backlog without a disposition is scope creep with a spreadsheet."}]},
               {"key":"parked","prompt":"The parked items — what keeps them honest?",
                "options":[
                  {"key":"governed","label":"A named owner, a review cadence, and re-entry only through change control with current pricing","quality":100,
                   "consequence":"The backlog is a managed asset, not a haunted attic; three items return next year, priced and chosen.",
                   "principle":"Parked scope is still scope — it needs an owner and a gate like everything else."},
                  {"key":"list","label":"A shared spreadsheet anyone can add to","quality":10,
                   "consequence":"The list doubles in a quarter and its items start appearing in delivery conversations as 'already agreed'.",
                   "principle":"An ungoverned list is a rumour mill with columns."}]}],
             "hints":["The fight is cheapest at the criteria level — have it once, before any item is scored.",
               "Watch for the difference between individually-small and collectively-material.",
               "Parked is a state with rules, not a euphemism for forgotten."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed a definition phase over a 200-item backlog without feeding it the baseline."}
            """),

        ("WC-SCO-088", "Roll it up before you promise it", "The crossing's cost accounts open Monday. One branch of the WBS does not add up.",
            "Bridges & Crossings", "Cost Engineer", "project_controls", "foundation", 7,
            """["scope_structuring","cost_control"]""",
            """
            {"context":"A river crossing's definition-phase WBS is due for baseline Monday, when cost accounts open. Finance has asked for the rolled-up total and confirmation that the structure obeys the 100% rule. The south-approach team has been reorganising its packages all week and something below 1.2 looks off.",
             "evidence":[
               {"label":"1.1 Design & consents","value":"220,000"},
               {"label":"1.2 Approaches (parent)","value":"—"},
               {"label":"1.2.1 North approach","value":"640,000"},
               {"label":"1.2.2 South approach","value":"310,000"},
               {"label":"1.3 Main span enabling","value":"150,000"},
               {"label":"South team note","value":"'Utilities scope moved out of 1.2.2 — destination TBD'"}],
             "task":"wbs","given":{"nodes":[
               {"id":"1","parent":null,"name":"Crossing"},
               {"id":"1.1","parent":"1","name":"Design","value":220000},
               {"id":"1.2","parent":"1","name":"Approaches"},
               {"id":"1.2.1","parent":"1.2","name":"North","value":640000},
               {"id":"1.2.2","parent":"1.2","name":"South","value":310000},
               {"id":"1.3","parent":"1","name":"Enabling","value":150000}]},
             "ask":[
               {"key":"root_total","label":"Rolled-up total for Monday","type":"number"},
               {"key":"hundred_percent_valid","label":"Does the structure satisfy the 100% rule? (yes/no)","type":"bool"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"tbd","prompt":"The 'destination TBD' utilities scope — Monday is two days away. You:",
                "options":[
                  {"key":"resolve","label":"Resolve the destination NOW with the south team — into an existing package or a new one — before the baseline, even if Monday slips a day","quality":100,
                   "consequence":"The baseline opens with every scope element addressed; a one-day slip nobody remembers beats a homeless scope everybody meets again.",
                   "principle":"Never baseline a structure containing the letters TBD."},
                  {"key":"park","label":"Baseline without it and add the utilities scope by early change next week","quality":25,
                   "consequence":"The change lands, but the baseline's opening act is a correction — and finance now samples your other packages for more TBDs.",
                   "principle":"A baseline's credibility is set by its first week."},
                  {"key":"pad","label":"Add an allowance line under 1.2 to cover 'whatever the utilities scope turns out to be'","quality":10,
                   "consequence":"An unscoped allowance becomes everyone's favourite funding source; the utilities work still has no owner.",
                   "principle":"Money without scope definition is an invitation, not a control."}]}],
             "hints":["Only leaf packages carry value — roll them up through the parents.",
               "The 100% rule fails the moment known scope has no package — 'TBD' is exactly that.",
               "Weigh a small, visible delay against baselining a known hole."],
             "profile_map":{"calculation":"Strategic Project Controller","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Refused to baseline a crossing WBS with TBD in it — and fixed it in a day."}
            """),

        ("WC-SCO-089", "The spare-parts scope nobody claimed", "Commissioning assumes stores has it. Stores assumes procurement. Procurement assumes commissioning.",
            "Industrial Manufacturing", "Definition Phase Planner", "project_management", "foundation", 5,
            """["scope_structuring","interface_management"]""",
            """
            {"context":"A production line install is closing definition. Three teams each assume another holds the commissioning spares scope: commissioning ('stores buys spares'), stores ('procurement's package'), procurement ('commissioning specifies, so commissioning owns'). The scope statement, WBS and procurement plan are all silent. Definition closes this week.",
             "evidence":[
               {"label":"Commissioning","value":"'Stores buys spares — always has'"},
               {"label":"Stores","value":"'That's in procurement's package'"},
               {"label":"Procurement","value":"'Commissioning specifies it, they own it'"},
               {"label":"Documents","value":"Scope statement, WBS, procurement plan: all silent"}],
             "decisions":[
               {"key":"fix","prompt":"What closes this properly before the phase gate?",
                "options":[
                  {"key":"raci","label":"Convene the three leads → assign ONE owning package → write the spares scope, budget and interfaces into the WBS dictionary → then close the phase","quality":100,
                   "consequence":"The line starts up with spares on the shelf because someone was paid and named to put them there.",
                   "principle":"Triangular assumptions are resolved by a named owner, not by a fourth meeting."},
                  {"key":"gate","label":"Close the phase on time and let the three teams sort it out in delivery","quality":5,
                   "consequence":"The first breakdown after start-up waits nine weeks for a bearing no one bought.",
                   "principle":"A gap you can name at the gate is a gap you chose to keep."},
                  {"key":"budget","label":"Add a spares budget line at programme level without assigning an owner","quality":25,
                   "consequence":"Funded and still unowned: three teams now assume one of the OTHERS will spend the new budget.",
                   "principle":"Budgets don't buy things; owners do."}]},
               {"key":"pattern","prompt":"This is the third 'everyone assumed' gap this phase. The systemic fix is:",
                "options":[
                  {"key":"matrix","label":"A scope-to-owner matrix walked through at every phase review — every scope line, exactly one owner, gaps visible as blank cells","quality":100,
                   "consequence":"The fourth gap is found by the matrix in review, not by delivery in month six.",
                   "principle":"Make ownership visible as a structure and gaps become findable instead of discoverable."},
                  {"key":"emails","label":"Ask all leads to confirm their scope by email each month","quality":20,
                   "consequence":"Everyone confirms what they already believed — including the contradictory parts.",
                   "principle":"Confirmation without cross-checking just notarises the assumptions."}]}],
             "hints":["Notice the shape: each team's assumption is individually reasonable and collectively circular.",
               "One owner per scope line — 'shared' ownership is how this gap was born.",
               "The durable fix makes gaps visible structurally, before anyone has to be clever."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Broke a three-way scope assumption loop before the phase gate closed on it."}
            """),

        // ───────────── Stakeholder Dilemma · alignment on scope & requirements · practitioner ─────────────

        ("WC-STK-090", "The workshop that would not converge", "Operations wants resilience. Finance wants the smaller number. The room wants to go home.",
            "Enterprise Programmes", "Requirements Workshop Facilitator", "project_management", "professional", 6,
            """["stakeholder_communication","requirements_management"]""",
            """
            {"context":"Hour three of the requirements workshop for a shared-services programme. Operations insists on dual-site resilience as a must-have; finance insists the business case only works single-site; both directors are in the room and neither will move. Fourteen other requirements are waiting behind this one, and the workshop is your last scheduled session before the requirements freeze.",
             "evidence":[
               {"label":"Operations position","value":"Dual-site resilience is a MUST"},
               {"label":"Finance position","value":"Case only closes single-site"},
               {"label":"Queue","value":"14 requirements still to review"},
               {"label":"Calendar","value":"Last session before the freeze"}],
             "decisions":[
               {"key":"room","prompt":"What do you do with the deadlock, right now?",
                "options":[
                  {"key":"park","label":"Park it with a precise disagreement statement both directors approve — what each believes and what evidence would move them — then clear the other 14","quality":100,
                   "consequence":"The 14 flow in ninety minutes; the resilience question leaves the room as a well-formed decision for the sponsor, not a grudge.",
                   "principle":"A precisely stated disagreement is progress; an argued-out room is not."},
                  {"key":"push","label":"Keep the room on it until someone concedes — freezes exist for a reason","quality":15,
                   "consequence":"Hour five produces a resentful pseudo-agreement that unravels by email on Thursday, and the 14 waiting requirements miss the freeze.",
                   "principle":"Fatigue produces signatures, not alignment."},
                  {"key":"split","label":"Write 'dual-site capable, single-site funded' and move on — both sides can read it their way","quality":25,
                   "consequence":"Both sides do read it their way; the ambiguity is now baselined, with interest accruing.",
                   "principle":"A requirement both sides read differently is a dispute with a requirement ID."}]},
               {"key":"escalate","prompt":"The parked question goes to the sponsor. How?",
                "options":[
                  {"key":"framed","label":"As a priced choice: the resilience premium, the outage exposure without it, and each director's evidence — recommendation optional","quality":100,
                   "consequence":"The sponsor decides in one sitting because the decision arrived shaped like one.",
                   "principle":"Escalate decisions, not arguments."},
                  {"key":"raw","label":"Forward the workshop minutes and let the sponsor read the debate","quality":20,
                   "consequence":"The sponsor reads two pages, calls a meeting, and the workshop reconvenes — with the same people and the same positions.",
                   "principle":"An unshaped escalation returns as a boomerang."}]}],
             "hints":["Ask what the other fourteen requirements cost while two directors repeat themselves.",
               "A disagreement written precisely — positions, evidence, what would change minds — is a deliverable.",
               "Sponsors decide between priced options; they mediate arguments badly."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Turned a deadlocked requirements workshop into one clean sponsor decision."}
            """),

        ("WC-STK-091", "Two departments, one front door", "Housing wants walk-in access. Social care wants appointments only. The building has one entrance.",
            "Civic Buildings", "Stakeholder & Requirements Lead", "project_management", "professional", 7,
            """["stakeholder_communication","requirements_management"]""",
            """
            {"context":"Defining a shared civic services hub, the housing department requires open walk-in access ('our users won't book'); adult social care requires controlled appointment-only entry ('our users need privacy and safeguarding'). Both cite statutory duties. The concept has one entrance, the requirements freeze in three weeks, and each department head has separately asked you to 'hold the line' for them.",
             "evidence":[
               {"label":"Housing","value":"Walk-in access — statutory homelessness duty cited"},
               {"label":"Social care","value":"Appointment-only — safeguarding duty cited"},
               {"label":"Concept design","value":"Single shared entrance"},
               {"label":"Both heads","value":"Each privately asked you to hold their line"}],
             "decisions":[
               {"key":"conflict","prompt":"How do you handle the collision?",
                "options":[
                  {"key":"reframe","label":"Take both duties to the designers as constraints, not solutions — ask for options that satisfy safeguarding AND walk-in (zoned entry, dual reception, scheduling by time-of-day) — then let the heads choose between real options","quality":100,
                   "consequence":"The designers return three workable configurations; the heads pick zoned entry in one meeting, each satisfied their duty is met.",
                   "principle":"Stakeholders collide on solutions; they align on constraints — negotiate at the level where both can win."},
                  {"key":"senior","label":"Escalate immediately to the chief executive to pick a department","quality":20,
                   "consequence":"A winner is declared; the losing department's cooperation on everything else quietly evaporates for a year.",
                   "principle":"Escalating a solvable design conflict buys one answer at the price of the relationship."},
                  {"key":"average","label":"Specify walk-in mornings and appointments afternoons as a compromise nobody asked for","quality":30,
                   "consequence":"Both duties are half-met; both heads disown the schedule the first time it inconveniences their users.",
                   "principle":"A compromise invented by the facilitator belongs to no one who has to defend it."}]},
               {"key":"private","prompt":"And the two private 'hold the line' requests?",
                "options":[
                  {"key":"transparent","label":"Tell each head, kindly and identically: you will represent their DUTY faithfully and will not run a private line for either","quality":100,
                   "consequence":"Both heads grumble; both trust the process — and you — more than before.",
                   "principle":"Neutrality is only credible when both sides hear you decline in the same words."},
                  {"key":"both","label":"Assure each head privately that you're on their side — it keeps them engaged","quality":0,
                   "consequence":"They compare notes at the freeze meeting. Your usefulness on this programme ends there.",
                   "principle":"Two private promises make one public liar."}]}],
             "hints":["Look for the level at which the two positions stop contradicting — duties, not door policies.",
               "Ask the designers for options before asking an executive for a ruling.",
               "Handle the identical private requests identically — and say so."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Aligned two statutory duties on one front door without an executive casualty."}
            """),

        ("WC-STK-092", "The regulator reads it differently", "Your scope says 'refurbish'. The port authority's inspector just used the word 'rebuild'.",
            "Ports & Marine", "Consents & Compliance Manager", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"Defining a port expansion, your scope treats the west quay works as refurbishment — permitted under the existing licence. In a routine liaison call, the harbour authority's inspector refers to the same works as 'effectively a rebuild', which would trigger a full new consent process adding months. The inspector hasn't put anything in writing. Your licence submission is due in four weeks.",
             "evidence":[
               {"label":"Your scope","value":"West quay: refurbishment (existing licence)"},
               {"label":"Inspector's phrase","value":"'Effectively a rebuild' — verbal, routine call"},
               {"label":"If rebuild","value":"Full consent process, months added"},
               {"label":"Submission","value":"Due in 4 weeks"}],
             "decisions":[
               {"key":"respond","prompt":"What do you do with the inspector's remark?",
                "options":[
                  {"key":"engage","label":"Request a pre-submission meeting now: present the engineering definition of the works and ask the authority to confirm the classification before you submit","quality":100,
                   "consequence":"The meeting surfaces one genuinely borderline element; you adjust its specification, the authority confirms 'refurbishment' in writing, and the submission lands safe.",
                   "principle":"A regulator's stray phrase is free intelligence — spend it before submission, not in appeal."},
                  {"key":"proceed","label":"Submit as planned — one inspector's verbal aside is not the authority's position","quality":10,
                   "consequence":"The aside turns out to be the authority's draft position; your submission is rejected and the consent clock starts at zero, months later than a meeting would have.",
                   "principle":"Hoping a regulator didn't mean it is not a consenting strategy."},
                  {"key":"redesign","label":"Preemptively redesign the works to be unambiguously refurbishment, before anyone asks","quality":35,
                   "consequence":"Scope shrinks to fit a classification nobody had actually ruled on; you paid the rebuild price in capability without being asked.",
                   "principle":"Never concede to an objection that has not been made."}]},
               {"key":"inside","prompt":"Internally, the project director says 'don't poke the regulator — meetings invite scrutiny'. You:",
                "options":[
                  {"key":"case","label":"Show the director the two timelines: a pre-submission meeting risking early scrutiny, versus a rejected submission risking the programme — and recommend the meeting","quality":100,
                   "consequence":"The director takes the meeting once the asymmetry is visible on one page.",
                   "principle":"'Don't poke the regulator' usually means 'let the regulator surprise us later'."},
                  {"key":"comply","label":"Follow the instruction and submit quietly","quality":15,
                   "consequence":"The rejection arrives with the director's name on the covering letter and your silence in the file.",
                   "principle":"Complying with a mistake you saw coming is a shared authorship."}]}],
             "hints":["Weigh what the remark costs to check now against what it costs to be right later.",
               "Regulators respond to engineering definitions better than to hopeful labels.",
               "Bring your director the asymmetry of outcomes, not just the recommendation."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Chased down a regulator's stray phrase four weeks before it became a rejection."}
            """),

        ("WC-STK-093", "Just a clarification", "The sponsor's 'clarified' requirement is twice the size of the original. The freeze was last month.",
            "Portfolio & PMO", "Programme Scope Manager", "project_management", "professional", 6,
            """["stakeholder_communication","scope_discipline"]""",
            """
            {"context":"A month after the requirements freeze on a portfolio consolidation programme, the sponsor circulates a note 'clarifying' requirement R-31: 'management reporting' apparently 'always meant' self-service analytics for 400 users, not the 12 scheduled reports the estimate priced. The note thanks the team for its flexibility. The delivery partner has already flagged, informally, that the difference is several hundred days of effort.",
             "evidence":[
               {"label":"R-31 as frozen","value":"'Management reporting' — priced as 12 scheduled reports"},
               {"label":"The 'clarification'","value":"Self-service analytics, 400 users"},
               {"label":"Partner's informal flag","value":"Several hundred days of effort difference"},
               {"label":"Tone","value":"Sponsor thanks the team for flexibility"}],
             "decisions":[
               {"key":"respond","prompt":"How do you respond to the sponsor's note?",
                "options":[
                  {"key":"price","label":"Warmly and promptly: treat the clarified intent as a candidate change, bring the sponsor its price and options (full, phased, descoped elsewhere) within the week","quality":100,
                   "consequence":"The sponsor — who genuinely believed it was a clarification — sees the delta, phases the analytics, and the baseline stays true.",
                   "principle":"Never argue about the word 'clarification'; price the difference and let the number do the talking."},
                  {"key":"accept","label":"Accept it — sponsors define intent, and the note is now on the record","quality":5,
                   "consequence":"Several hundred unpriced days enter the plan silently; the overrun, when it surfaces, belongs to the delivery team that 'agreed'.",
                   "principle":"Whoever absorbs a change silently adopts it, cost and all."},
                  {"key":"contest","label":"Reply that R-31 is frozen and the note has no contractual effect","quality":25,
                   "consequence":"Technically true, relationally expensive: the sponsor now describes the programme as 'lawyering its own sponsor'.",
                   "principle":"Being right about the freeze is not a communication strategy."}]},
               {"key":"partner","prompt":"The delivery partner's flag was informal. You:",
                "options":[
                  {"key":"formalise","label":"Ask the partner to put their impact estimate in writing NOW, before the sponsor conversation","quality":100,
                   "consequence":"The sponsor meeting runs on a real number instead of 'the partner seems worried'.",
                   "principle":"Informal flags evaporate exactly when you need them — bank the estimate while it is fresh."},
                  {"key":"hold","label":"Keep it informal to avoid alarming the sponsor prematurely","quality":20,
                   "consequence":"When you finally need the number, the partner's memory of it has strategically improved.",
                   "principle":"An estimate that was never written down was never given."}]}],
             "hints":["Don't litigate the label — quantify the gap between what was priced and what is now described.",
               "Assume good faith: sponsors often genuinely believe the larger reading was always the intent.",
               "Get the partner's impact in writing before, not after, the sponsor conversation."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a 'clarification' before it became a silent baseline change."}
            """),

        ("WC-STK-094", "The design review that became a duel", "The architect and the cost consultant are fighting in front of the client. You chair the meeting.",
            "Flood Defence", "Design Stage Project Manager", "project_management", "professional", 7,
            """["stakeholder_communication","conflict_management"]""",
            """
            {"context":"Mid design review for a flood defence scheme, with the client's director present, the architect and the cost consultant have stopped reviewing and started duelling: the architect accuses the consultant of 'pricing out every piece of design quality'; the consultant accuses the architect of 'gold-plating a flood wall'. The disputed item — a visitor viewpoint structure — is 3% of scheme cost. The client director is watching you, the chair, with interest.",
             "evidence":[
               {"label":"Dispute","value":"Viewpoint structure — ~3% of scheme cost"},
               {"label":"Architect","value":"'Quality is being priced out'"},
               {"label":"Cost consultant","value":"'It's gold-plating'"},
               {"label":"Audience","value":"Client director, watching the chair"}],
             "decisions":[
               {"key":"chair","prompt":"As chair, right now, you:",
                "options":[
                  {"key":"structure","label":"Stop the exchange, restate the item as a client value decision — amenity benefit versus 3% cost — task both advisers to produce a one-page joint options note, and move the review on","quality":100,
                   "consequence":"The review recovers its agenda; the client director gets a decision shaped for them within the week, built by both duellists together.",
                   "principle":"The chair's job is to convert heat into a decision for the person who owns the trade-off."},
                  {"key":"side","label":"Back the cost consultant in the room — the business case is tight and the client will appreciate the discipline","quality":15,
                   "consequence":"The architect goes quiet — in this meeting and, more expensively, in the three later reviews where their challenge would have caught real problems.",
                   "principle":"Publicly picking a winner between advisers teaches the loser to stop advising."},
                  {"key":"offline","label":"Let them finish the argument — clients deserve to see the real debate","quality":25,
                   "consequence":"The client director sees twenty minutes of professionals interrupting each other and concludes the scheme's governance needs 'strengthening'.",
                   "principle":"Unmanaged conflict in front of the client is not transparency; it is a confidence leak."}]},
               {"key":"after","prompt":"After the meeting, with each adviser separately, you:",
                "options":[
                  {"key":"reset","label":"Reset expectations with both: challenge is wanted, in review papers and options — not as courtroom drama in front of the client","quality":100,
                   "consequence":"The next review gets the same rigour with none of the theatre; both advisers privately thank you.",
                   "principle":"Protect the disagreement, change its venue."},
                  {"key":"report","label":"Report both to their firms' partners","quality":10,
                   "consequence":"Two defensive firms, two carefully lawyered advisers, no more spontaneous professional judgment in your reviews.",
                   "principle":"Escalating a behaviour you could have coached converts colleagues into counsel."}]}],
             "hints":["Identify who actually owns the disputed trade-off — it is neither adviser.",
               "Notice what the client director is really evaluating: the item, or the governance.",
               "Keep the disagreement's content and kill its format."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Turned an advisers' duel into a one-page client decision."}
            """),

        ("WC-STK-095", "The must-have that isn't in scope", "Operations' number one requirement was never in the approved case. Someone has to tell them.",
            "Energy Networks", "Engagement Manager", "project_management", "professional", 5,
            """["stakeholder_communication","scope_discipline"]""",
            """
            {"context":"The substation replacement's approved business case covers like-for-like replacement. Operations' engagement survey response lists remote-switching capability as their top requirement — 'assumed it was obviously included'. It never was: it was evaluated and dropped at options stage for cost, a decision recorded in a paper operations never saw. You are the one who has to close the loop with the operations manager, whose team will run this asset for thirty years.",
             "evidence":[
               {"label":"Approved case","value":"Like-for-like replacement only"},
               {"label":"Ops top requirement","value":"Remote switching — 'assumed included'"},
               {"label":"History","value":"Evaluated and dropped at options stage, for cost"},
               {"label":"Relationship","value":"Ops runs the asset for 30 years"}],
             "decisions":[
               {"key":"tell","prompt":"How do you close the loop?",
                "options":[
                  {"key":"full","label":"Meet the ops manager: show the options-stage evaluation, own that ops should have been consulted then, and offer the legitimate route — a costed change proposal they can sponsor","quality":100,
                   "consequence":"The manager is annoyed about the process and respects the honesty; the change proposal loses on cost, and ops accepts it because this time they were in the room.",
                   "principle":"People accept 'no' when they can see the evaluation and were offered the door back in."},
                  {"key":"soft","label":"Say the requirement is 'being considered for a future phase' to keep the relationship warm","quality":10,
                   "consequence":"There is no future phase; the manager plans staffing around a capability that never comes, and finds out from an as-built drawing.",
                   "principle":"A comfortable ambiguity today is a betrayal with a delivery date."},
                  {"key":"deflect","label":"Point out the case was approved by their own directorate — the miss is on their side","quality":20,
                   "consequence":"Technically arguable, and now the thirty-year operator of your asset opens every future conversation from a grudge.",
                   "principle":"Winning the attribution argument loses the operating decades."}]},
               {"key":"systemic","prompt":"So requirements stop being 'assumed included':",
                "options":[
                  {"key":"traceout","label":"Publish dropped-options summaries to affected stakeholders at each stage gate — what was considered, what was excluded, and why","quality":100,
                   "consequence":"The next 'obviously included' assumption dies at options stage, in daylight, cheaply.",
                   "principle":"Exclusions communicated at decision time cost a paragraph; discovered at delivery they cost trust."},
                  {"key":"survey","label":"Run the engagement survey earlier next time","quality":30,
                   "consequence":"Earlier surveys, same blindness: stakeholders still can't react to exclusions nobody showed them.",
                   "principle":"Asking earlier doesn't help if the answers still never travel back."}]}],
             "hints":["Bring the actual options-stage record — evidence turns a rejection into an explanation.",
               "Offer the legitimate re-entry route even when you expect it to fail; the offer is the respect.",
               "Fix the loop that let an exclusion stay invisible to the people it excluded."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Delivered a five-year-old 'no' honestly — and kept the thirty-year relationship."}
            """),

        ("WC-STK-096", "Whose signature accepts the ward", "The nurses' requirements have three authors and no owner. The refurbishment can't freeze without one.",
            "Healthcare Estates", "Clinical Liaison Project Manager", "project_management", "professional", 6,
            """["stakeholder_communication","requirements_management"]""",
            """
            {"context":"A hospital ward refurbishment is trying to freeze clinical requirements. The senior sister, the infection-control lead and the clinical director have each edited the requirements document — sometimes overwriting each other — and none will sign as THE clinical owner: each says sign-off belongs to one of the others. The freeze is blocking the design contract award, now eight days away.",
             "evidence":[
               {"label":"Document","value":"3 editors, conflicting edits, no owner"},
               {"label":"Senior sister","value":"'Infection control must sign'"},
               {"label":"Infection control","value":"'The clinical director signs'"},
               {"label":"Clinical director","value":"'The ward team signs — they live there'"},
               {"label":"Design award","value":"Blocked; 8 days"}],
             "decisions":[
               {"key":"unblock","prompt":"How do you unblock sign-off?",
                "options":[
                  {"key":"structure","label":"Split the signature to match real authority: sister signs operational requirements, IC lead signs infection-control requirements, director countersigns the whole — one page, three scopes, this week","quality":100,
                   "consequence":"Each signs what they actually own within days; the circular deference ends because nobody is being asked to own someone else's expertise.",
                   "principle":"When nobody will own everything, partition the ownership to match the authority that already exists."},
                  {"key":"chase","label":"Book a joint meeting and keep the three in the room until one signs","quality":20,
                   "consequence":"The meeting reproduces the deference loop with better biscuits; day six, still no signature.",
                   "principle":"A room cannot create an authority that the organisation hasn't defined."},
                  {"key":"proxy","label":"Have the project director sign 'on behalf of clinical stakeholders' to protect the award date","quality":5,
                   "consequence":"The award proceeds; at handover the ward rejects the layout 'nobody clinical ever approved', with the signature to prove it.",
                   "principle":"A proxy signature converts a requirements gap into an acceptance dispute."}]},
               {"key":"edits","prompt":"And the conflicting overwrites in the document?",
                "options":[
                  {"key":"reconcile","label":"Reconcile visibly: walk the conflicts with all three, record each resolution against its owner's new scope","quality":100,
                   "consequence":"The frozen document is one everyone recognises; no author later disowns a sentence.",
                   "principle":"Freeze agreements, not the last person's save."},
                  {"key":"latest","label":"Freeze the latest version — someone has to pick","quality":15,
                   "consequence":"The 'latest' happens to be the sister's Tuesday edits; infection control discovers their overwritten clause at the design review, loudly.",
                   "principle":"Version-by-timestamp is arbitration by accident."}]}],
             "hints":["Notice the deference is circular because the signature is monolithic — no one owns ALL of it.",
               "Match each signature to an authority that already exists in the organisation.",
               "A frozen document must be one all its authors can recognise as theirs."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Partitioned a clinical sign-off so three non-signers became three owners."}
            """),

        ("WC-STK-097", "The demo that sold what we didn't spec", "The vendor's pre-sales demo showed features your requirements never asked for. The users now require them.",
            "Technology Programmes", "Business Change Lead", "project_management", "professional", 7,
            """["stakeholder_communication","requirements_management"]""",
            """
            {"context":"During definition of a workflow platform, the shortlisted vendor ran a 'capability demo' for your user community — showing, among the specified features, an AI-assisted triage screen that is NOT in your requirements and not in the priced configuration. The user representatives now list the triage screen among their expectations; two department heads have referenced it in planning documents. The vendor's account manager calls it 'a natural phase 2 conversation'.",
             "evidence":[
               {"label":"Requirements","value":"No AI triage — never specified, never priced"},
               {"label":"The demo","value":"Vendor showed it to your user community"},
               {"label":"Effect","value":"Users expect it; 2 department heads planning around it"},
               {"label":"Vendor","value":"'A natural phase 2 conversation'"}],
             "decisions":[
               {"key":"users","prompt":"With the user community, you:",
                "options":[
                  {"key":"reset","label":"Publish a clear scope statement — what is bought and configured versus what was demonstration-only — and route the triage screen into the governed backlog with an honest evaluation date","quality":100,
                   "consequence":"Expectations correct within a fortnight; the triage feature gets a real evaluation instead of a resentful haunting.",
                   "principle":"Correct an inflated expectation the week it forms — its price doubles every planning cycle it survives."},
                  {"key":"ride","label":"Let the enthusiasm ride — engaged users are hard-won and the feature might land in phase 2 anyway","quality":10,
                   "consequence":"Go-live day delivers the specified system to users measuring it against the demo; adoption reads as 'the cheap version arrived'.",
                   "principle":"Unmanaged expectations grade your delivery against someone else's demo."},
                  {"key":"blame","label":"Tell users the vendor oversold — expectations are the vendor's problem to walk back","quality":25,
                   "consequence":"The vendor shrugs, users hear the project bad-mouthing its own supplier, and the expectation stays exactly where it was.",
                   "principle":"Attribution doesn't deflate an expectation; information does."}]},
               {"key":"vendor","prompt":"With the vendor, before contract award, you:",
                "options":[
                  {"key":"discipline","label":"Require demo content to match the priced configuration in all future sessions, in writing — and get the triage screen's phase-2 price banked now, while competition still exists","quality":100,
                   "consequence":"Demos stop selling futures; the phase-2 price obtained pre-award is 40% below what it would cost post-lock-in.",
                   "principle":"Price the dream while you can still walk away from it."},
                  {"key":"friendly","label":"Mention it informally — no need to sour the relationship before award","quality":15,
                   "consequence":"The next demo shows two more unpriced features; the account manager has learned exactly what informal means.",
                   "principle":"A boundary that costs nothing to cross will be crossed on schedule."}]}],
             "hints":["Time matters twice: expectations harden with every cycle, and leverage dies at contract award.",
               "Give users the truth plus a legitimate route for the wish — not just the truth.",
               "Whatever the vendor showed, YOUR scope statement defines what arrives at go-live."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Deflated a demo-inflated expectation and banked the phase-2 price before award."}
            """),

        ("WC-STK-098", "The requirement announced to the press", "A board member told a journalist the programme includes something it does not.",
            "Portfolio & PMO", "Programme Communications & Scope Lead", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"A board member, interviewed about your portfolio modernisation programme, told a trade journalist it would deliver 'full public self-service by next year'. That capability was explicitly deferred from scope at the last investment review — a decision the same board member attended. The article runs tomorrow; the journalist has asked the programme office to 'confirm details'; your sponsor is travelling until Thursday.",
             "evidence":[
               {"label":"Public claim","value":"'Full public self-service by next year'"},
               {"label":"Actual scope","value":"Deferred at last investment review"},
               {"label":"Awkward fact","value":"The board member attended that review"},
               {"label":"Clock","value":"Article runs tomorrow; sponsor back Thursday"}],
             "decisions":[
               {"key":"journalist","prompt":"The journalist wants confirmation today. You:",
                "options":[
                  {"key":"accurate","label":"Provide the accurate approved-scope description in neutral language — what IS being delivered and when — without commentary on the board member's phrasing","quality":100,
                   "consequence":"The article prints the accurate scope beside the quote; the gap is visible but unexplained, which is the board member's problem to manage, not yours to widen.",
                   "principle":"Correct the record with facts; never audit a board member in the press."},
                  {"key":"confirm","label":"Confirm the board member's version — contradicting a board member publicly is above your pay grade","quality":5,
                   "consequence":"The programme is now publicly committed to deferred scope; the next investment review must either fund it unplanned or un-say it in print.",
                   "principle":"A confirmed misstatement becomes a commitment with your office's name on it."},
                  {"key":"silence","label":"Decline all comment until the sponsor returns Thursday","quality":25,
                   "consequence":"The article runs on the board member's version alone, now uncorrected AND unchallenged — Thursday's options are all worse.",
                   "principle":"In a running news cycle, silence is a version of confirmation."}]},
               {"key":"member","prompt":"And the board member?",
                "options":[
                  {"key":"brief","label":"Same day, through the sponsor's office: a courteous note with the approved scope summary and an offer of a standing briefing pack for future interviews","quality":100,
                   "consequence":"The member — who had genuinely conflated aspiration with scope — takes the pack; future interviews quote it.",
                   "principle":"Equip the voice you cannot control; most misstatements are briefing failures, not malice."},
                  {"key":"report","label":"Formally report the misstatement to the board secretary as a governance breach","quality":15,
                   "consequence":"Procedurally defensible; politically, the programme office just filed a complaint against its own board — Thursday's sponsor conversation is now about YOU.",
                   "principle":"Choose the remedy that fixes the next interview, not the one that wins this one."}]}],
             "hints":["Separate correcting the public record from managing the person — different channels, different tones.",
               "The programme office's only sustainable currency with the press is accuracy without commentary.",
               "Ask why the board member misspoke — the fix for a briefing failure is a briefing."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Corrected a public scope misstatement without starting a boardroom war."}
            """),

        // ───────────── Cost & Value · foundation ─────────────

        ("WC-EVM-099", "Three letters the board reads first", "PV, EV, AC — and the forecast the definition team promised the board it could hold.",
            "Enterprise Programmes", "Junior Cost Analyst", "project_controls", "foundation", 10,
            """["earned_value","forecasting"]""",
            """
            {"context":"A shared-services programme's definition phase runs as its own funded project. At period 6 the definition budget shows Planned Value 2,000,000; Earned Value 1,800,000; Actual Cost 1,950,000 against the phase Budget at Completion of 6,000,000. The board pack's forecast line still shows the phase completing on budget. Compute the standard forecast set and decide what the pack should say.",
             "evidence":[
               {"label":"Planned Value (PV)","value":"2,000,000"},
               {"label":"Earned Value (EV)","value":"1,800,000"},
               {"label":"Actual Cost (AC)","value":"1,950,000"},
               {"label":"Phase BAC","value":"6,000,000"},
               {"label":"Pack forecast","value":"'Completing on budget' — unchanged since period 1"}],
             "task":"evm","given":{"pv":2000000,"ev":1800000,"ac":1950000,"bac":6000000},
             "ask":[
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
               {"key":"eac","label":"Estimate at Completion (EAC = BAC/CPI)","type":"number"},
               {"key":"etc","label":"Estimate to Complete (ETC)","type":"number"},
               {"key":"vac","label":"Variance at Completion (VAC)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"pack","prompt":"What does the board pack's forecast line say this period?",
                "options":[
                  {"key":"hold","label":"Keep 'on budget' — definition phases always catch up in the back half","quality":5,
                   "consequence":"Period 9 forces the correction anyway, now with three periods of 'on budget' to explain alongside the overrun.",
                   "principle":"'It always catches up' is a hope wearing a forecast's clothes."},
                  {"key":"measured","label":"Show the CPI-based EAC with the VAC, plus the two recovery options that could close part of the gap","quality":100,
                   "consequence":"The board sees the measured position early enough to act on it — and approves the smaller of the recovery options.",
                   "principle":"A forecast is what the measurement says, adjusted by funded actions — not by optimism."},
                  {"key":"range","label":"Show a range from 'on budget' to the CPI forecast, letting readers pick","quality":30,
                   "consequence":"Everyone reads the end of the range they prefer; the pack has technically informed and practically decided nothing.",
                   "principle":"A range without a stated most-likely is a mirror, not a forecast."}]}],
             "hints":["CPI is EV divided by AC — efficiency of money already spent.",
               "The standard EAC divides BAC by CPI: past efficiency projected forward.",
               "ETC is EAC minus AC; VAC is BAC minus EAC — the gap the board actually cares about."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Replaced an 'on budget' hope with a measured forecast the board could act on."}
            """),

        ("WC-CHG-100", "Where the contingency actually went", "The plant expansion 'never used its contingency'. The change register tells a different story.",
            "Industrial Manufacturing", "Assistant Project Controller", "project_controls", "foundation", 12,
            """["change_control","cost_control"]""",
            """
            {"context":"A plant expansion enters delivery planning with a proud claim: 'contingency untouched'. You reconcile the change register against the baseline of 5,600,000 and 180 days. Four changes exist in different states — and the pending one is being worked on site 'in anticipation'. Build the revised baseline from approved changes only, then deal with what you find.",
             "evidence":[
               {"label":"Baseline","value":"BAC 5,600,000 · 180 days"},
               {"label":"C1 — Line-speed upgrade","value":"APPROVED · cost +240,000 · schedule +10 days"},
               {"label":"C2 — Robot cell addition","value":"PENDING · cost +380,000 · schedule +20 days · 'work started in anticipation'"},
               {"label":"C3 — Descope spare conveyor","value":"APPROVED · cost -90,000 · schedule -5 days"},
               {"label":"C4 — Mezzanine extension","value":"REJECTED · cost +150,000 · schedule +8 days"}],
             "task":"change",
             "given":{"baseline_bac":5600000,"baseline_duration":180,"changes":[
               {"id":"C1","status":"approved","cost_delta":240000,"schedule_delta":10},
               {"id":"C2","status":"pending","cost_delta":380000,"schedule_delta":20},
               {"id":"C3","status":"approved","cost_delta":-90000,"schedule_delta":-5},
               {"id":"C4","status":"rejected","cost_delta":150000,"schedule_delta":8}]},
             "ask":[
               {"key":"revised_bac","label":"Revised BAC (approved only)","type":"number"},
               {"key":"revised_duration","label":"Revised duration (days)","type":"number"},
               {"key":"approved_count","label":"Approved change count","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"anticipation","prompt":"C2's robot cell work has started on site while the change is pending. You:",
                "options":[
                  {"key":"stop","label":"Flag it to the change board this week: stop-work or emergency approval, but not limbo — and quantify the exposure already committed","quality":100,
                   "consequence":"The board approves C2 at a negotiated amount; the committed exposure is inside the decision instead of underneath it.",
                   "principle":"Work-in-anticipation is an unapproved commitment; surface it while it is still small enough to decide about."},
                  {"key":"assume","label":"Treat C2 as effectively approved — the board always approves what's already built","quality":5,
                   "consequence":"The board, resentful of being pre-empted, approves C2 at cost and starts auditing what else is 'anticipated'.",
                   "principle":"Boards that discover they are rubber stamps stop stamping."},
                  {"key":"bury","label":"Absorb the early works into C1's approved budget — they're adjacent scopes","quality":0,
                   "consequence":"C1 overruns 'inexplicably'; the reconciliation you were hired to do is now the thing you did.",
                   "principle":"Moving unapproved cost into approved lines is not control; it is concealment."}]}],
             "hints":["Only APPROVED changes move the baseline — pending and rejected do not, whatever site is doing.",
               "Apply cost and schedule deltas with their signs; descopes reduce.",
               "The claim 'contingency untouched' should be tested against commitments, not just approvals."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reconciled a 'contingency untouched' claim against the change register that said otherwise."}
            """),

        // ───────────── Executive Mission · capstone · multi-stage ─────────────

        ("WC-CAP-101", "The definition gateway", "Three decisions in one afternoon set the programme's shape for five years.",
            "Capital Programmes", "Programme Director (acting)", "project_management", "expert", 24,
            """["governance","strategy_execution","stakeholder_communication"]""",
            """
            {"context":"You are acting programme director for a multi-site modernisation programme approaching its definition gateway. In one afternoon session the gateway panel expects your position on three linked questions: the delivery scope option, the funding structure, and the governance model for delivery. The analysis pack is honest but incomplete — as definition packs always are. Whatever you choose shapes the next five years.",
             "evidence":[
               {"label":"Scope options","value":"A: all 8 sites in one wave · B: 3 lead sites then rolling waves · C: 8 sites, descoped specification"},
               {"label":"Analysis","value":"Option B's lead sites cover 70% of benefit variance; A maximises theoretical NPV; C minimises capital"},
               {"label":"Funding","value":"Single sanction now vs staged sanction per wave"},
               {"label":"Delivery governance","value":"Central programme team vs site-led with programme assurance"},
               {"label":"Known unknowns","value":"Site-condition surveys complete at only 3 of 8 sites"}],
             "decisions":[
               {"key":"scope","prompt":"Stage 1 — which scope option do you take to the panel?",
                "options":[
                  {"key":"waves","label":"Option B: three surveyed lead sites first, later waves shaped by lead-site evidence","quality":100,
                   "consequence":"The programme commits hard where it has evidence and keeps options where it does not; wave 2's design is measurably cheaper for it.",
                   "principle":"Commit to what is surveyed; buy options on what is not."},
                  {"key":"all","label":"Option A: all eight sites — the NPV is highest and the political moment is now","quality":25,
                   "consequence":"Five unsurveyed sites deliver their surprises simultaneously in year 2; the NPV that justified the wave was never real.",
                   "principle":"A theoretical optimum built on unsurveyed ground is a liability with a discount rate."},
                  {"key":"cheap","label":"Option C: everything, thinner — spread the capital across all sites","quality":15,
                   "consequence":"Eight sites each get too little to transform anything; the programme delivers percentages instead of outcomes.",
                   "principle":"Descoping the specification everywhere is how programmes fail everywhere at once."}]},
               {"key":"funding","prompt":"Stage 2 — funding structure?",
                "options":[
                  {"key":"staged","label":"Staged sanction: full funding for wave 1, committed envelope with per-wave release gates for the rest","quality":100,
                   "consequence":"Wave 2's sanction, informed by wave 1 actuals, passes in twenty minutes; the programme never carries idle capital.",
                   "principle":"Match the money's commitment profile to the evidence's arrival profile."},
                  {"key":"all_now","label":"Single full sanction — going back to the board per wave invites re-litigation of the whole programme","quality":30,
                   "consequence":"Full capital is locked against year-4 estimates that year-1 evidence immediately invalidates; the re-litigation happens anyway, as a variance inquiry.",
                   "principle":"Money sanctioned ahead of evidence converts learning into embarrassment."},
                  {"key":"minimal","label":"Fund wave 1 only, with no committed envelope beyond it","quality":35,
                   "consequence":"Wave 1 delivers; waves 2-4 die in the next budget cycle to a competing priority, exactly as the supply chain predicted when it priced wave 1 accordingly.",
                   "principle":"A programme with no committed horizon pays spot prices for everything, including attention."}]},
               {"key":"govern","prompt":"Stage 3 — delivery governance?",
                "options":[
                  {"key":"hybrid","label":"Site-led delivery inside a programme-set framework: common standards, gates and reporting; local delivery authority within them","quality":100,
                   "consequence":"Sites own their delivery and the programme owns comparability; wave-2 sites reuse wave-1 learning because the framework made it legible.",
                   "principle":"Centralise the standards, devolve the delivery, and make learning the thing that travels."},
                  {"key":"central","label":"A strong central team delivering all sites directly","quality":30,
                   "consequence":"Consistent, and slow: every site decision queues in the centre, and site ownership — needed at handover — never forms.",
                   "principle":"A centre that does everything becomes the queue everything waits in."},
                  {"key":"loose","label":"Each site delivers its own way, with light programme assurance","quality":15,
                   "consequence":"Eight delivery models, incomparable reports, and wave-2 sites repeating wave-1 mistakes with local variations.",
                   "principle":"Without common structure, a programme is just eight projects sharing a logo."}]}],
             "hints":["Trace where the evidence actually exists — surveys, benefit variance — and align commitment to it.",
               "Each stage's answer should make the next stage easier: scope shapes funding shapes governance.",
               "Ask of every option: what does this commit irreversibly, and on what evidence?"],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Set a five-year programme's scope, funding and governance in one gateway afternoon."}
            """),

        ("WC-CAP-102", "Seven platforms, one future", "The consolidation everyone wants in principle and nobody wants in their department.",
            "Technology Programmes", "Consolidation Programme Lead", "project_management", "expert", 22,
            """["governance","strategy_execution","stakeholder_communication"]""",
            """
            {"context":"Your organisation runs seven overlapping workflow platforms across departments; the board has mandated consolidation to 'at most two' and handed you the definition. Every department agrees consolidation is overdue — and each has nominated its own platform as the survivor. You must take a position on the target architecture, the migration sequence, and what happens to the departments whose platforms retire.",
             "evidence":[
               {"label":"Estate","value":"7 platforms; 2 cover ~80% of use cases between them"},
               {"label":"Politics","value":"Each department nominates its own platform"},
               {"label":"Costs","value":"Licence + support saving ~ 40% at 2 platforms"},
               {"label":"Risk","value":"Two departments run regulated processes on retiring platforms"},
               {"label":"Mandate","value":"Board: 'at most two', definition due"}],
             "decisions":[
               {"key":"target","prompt":"Stage 1 — how do you pick the surviving platforms?",
                "options":[
                  {"key":"criteria","label":"Published capability/cost/risk criteria, scored openly with department architects in the room — nominations welcome as evidence, not votes","quality":100,
                   "consequence":"The two survivors are the ones the criteria pick; three departments are disappointed in the criteria rather than defeated by rivals — which turns out to matter enormously.",
                   "principle":"Selection by open criteria converts losers into participants; selection by influence converts them into insurgents."},
                  {"key":"incumbent","label":"Pick the two biggest platforms by user count — simplest defensible line","quality":30,
                   "consequence":"User count rewarded past procurement, not fitness; one 'winner' needs replacing within three years, restarting this entire exercise.",
                   "principle":"Installed base measures history, not suitability."},
                  {"key":"neutral","label":"Buy a new eighth platform nobody currently owns — perfectly neutral","quality":10,
                   "consequence":"Seven migrations instead of five, no internal expertise anywhere, and the neutrality premium is paid by every department equally — in years.",
                   "principle":"Neutrality that maximises total migration is not a compromise; it is a surrender to politics priced in delivery."}]},
               {"key":"sequence","prompt":"Stage 2 — migration sequence?",
                "options":[
                  {"key":"riskled","label":"Non-regulated processes first to prove the route; the two regulated migrations go last, each behind a rehearsed cutover with fallback","quality":100,
                   "consequence":"By the time the regulated processes move, the route has been walked eight times; the regulator's questions have rehearsal logs as answers.",
                   "principle":"Sequence so that your riskiest step is your most practised."},
                  {"key":"bigbang","label":"All migrations in one coordinated window — shorter total disruption","quality":10,
                   "consequence":"One window, seven simultaneous failure modes, and the regulated fallback plan turns out to depend on a platform already switched off.",
                   "principle":"A big bang converts independent risks into one correlated one."},
                  {"key":"easyfirst","label":"Easiest departments first to build momentum, hardest last, no other logic","quality":35,
                   "consequence":"Momentum builds, then stalls exactly at the hard cases — now with the programme's budget mostly spent on the easy ones.",
                   "principle":"Momentum is a by-product of sequencing, not a substitute for it."}]},
               {"key":"losers","prompt":"Stage 3 — the departments whose platforms retire?",
                "options":[
                  {"key":"invest","label":"A funded transition package per department: capability parity analysis, feature-gap remediation on the surviving platform, and their experts seconded into the migration team","quality":100,
                   "consequence":"The 'losing' departments' experts become the migration's best engineers — they know the edge cases — and adoption follows their credibility.",
                   "principle":"The people who lose the platform decision must visibly win the transition, or they will quietly re-run the decision forever."},
                  {"key":"mandate","label":"The board mandated it; departments comply — transition support is business-as-usual training","quality":15,
                   "consequence":"Compliance is achieved and adoption is not: shadow spreadsheets bloom, and the 40% saving erodes into workaround costs.",
                   "principle":"Mandates move systems; only investment moves behaviour."},
                  {"key":"exempt","label":"Grant the two loudest departments temporary exemptions to keep the peace","quality":20,
                   "consequence":"'Temporary' calcifies; three platforms persist, the saving halves, and every future consolidation cites your exemptions as precedent.",
                   "principle":"An exemption to end an argument is a lease the argument signs on your building."}]}],
             "hints":["Design the selection so its losers can respect it — that is worth more than the selection itself.",
               "Put your rehearsals before your regulated risk, not after it.",
               "Fund the losing departments' transition as seriously as the winning platforms' scale-up."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Consolidated seven platforms to two without creating seven enemies."}
            """),

        // ───────────── Risk Room · practitioner · EMV ─────────────

        ("WC-RSK-103", "The register from one workshop", "Ninety minutes, one wall of sticky notes, four quantified lines. Is that a register?",
            "Logistics & Warehousing", "Project Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","facilitation"]""",
            """
            {"context":"A distribution warehouse project's definition-stage risk register was produced in a single ninety-minute workshop. Four risks were quantified before the room was needed for something else. The steering group wants the net EMV for the funding paper — and asks whether one workshop's output deserves the word 'register'.",
             "evidence":[
               {"label":"R1 — Ground improvement underestimate","value":"probability 0.30, impact -450,000"},
               {"label":"R2 — Racking supplier failure","value":"probability 0.25, impact -800,000"},
               {"label":"R3 — Early tenant fit-out revenue","value":"probability 0.50, impact +160,000"},
               {"label":"R4 — Planning judicial review","value":"probability 0.10, impact -1,300,000"},
               {"label":"Provenance","value":"One 90-minute workshop, operations not present"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.3,"impact":-450000},{"id":"R2","probability":0.25,"impact":-800000},
               {"id":"R3","probability":0.5,"impact":160000},{"id":"R4","probability":0.1,"impact":-1300000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R2","label":"EMV of R2 — racking supplier","type":"number"},
               {"key":"emv_R4","label":"EMV of R4 — judicial review","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"depth","prompt":"Your answer to 'is this a register?'",
                "options":[
                  {"key":"honest","label":"It is a first pass: present the EMV labelled as workshop-stage, with a completion plan — operations interviews, supplier assessment, a planning-counsel view on R4 — before sanction","quality":100,
                   "consequence":"The funding paper carries a number with its provenance attached; the completion pass adds two risks the workshop missed, both material.",
                   "principle":"Label the maturity of every number; a first pass presented as a register is a forecast presented as fact."},
                  {"key":"yes","label":"Yes — four quantified risks with an EMV is more than most projects have at this stage","quality":20,
                   "consequence":"The register's gaps surface as 'new' risks after sanction, when they are no longer fundable from the contingency the EMV sized.",
                   "principle":"Comparing to worse practice is how immature numbers get sanctioned."},
                  {"key":"no","label":"No — refuse to provide the EMV until a full risk process has run","quality":35,
                   "consequence":"The funding paper proceeds with NO risk number, which is the only thing worse than a labelled early one.",
                   "principle":"Withholding an imperfect number rarely produces a better one in time."}]}],
             "hints":["EMV each line as probability × impact; the tenant-revenue line is an opportunity.",
               "Note which single line carries the largest exposure despite its low probability.",
               "The question behind the question: what maturity label does this number deserve?"],
             "profile_map":{"calculation":"Risk Strategist","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a workshop-stage risk register — and labelled its maturity honestly."}
            """),

        ("WC-RSK-104", "Contingency before the case closes", "The business case wants one contingency number. The register wants to be believed first.",
            "Enterprise Programmes", "Programme Risk Manager", "project_controls", "professional", 9,
            """["risk_management","cost_control"]""",
            """
            {"context":"An enterprise transformation's business case closes next week and needs its contingency line. The definition-stage register holds four quantified entries. Finance proposes 'the usual 10%'; the sponsor asks what the register itself justifies. Compute the net EMV and the key exposures, then recommend the contingency basis.",
             "evidence":[
               {"label":"R1 — Data migration complexity","value":"probability 0.40, impact -350,000"},
               {"label":"R2 — Core-team attrition mid-programme","value":"probability 0.20, impact -1,200,000"},
               {"label":"R3 — Licence renegotiation slips","value":"probability 0.35, impact -300,000"},
               {"label":"R4 — Early decommissioning saving","value":"probability 0.25, impact +240,000"},
               {"label":"Finance proposal","value":"'The usual 10% of programme cost'"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.4,"impact":-350000},{"id":"R2","probability":0.2,"impact":-1200000},
               {"id":"R3","probability":0.35,"impact":-300000},{"id":"R4","probability":0.25,"impact":240000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — data migration","type":"number"},
               {"key":"emv_R2","label":"EMV of R2 — attrition","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"basis","prompt":"Your contingency recommendation for the case?",
                "options":[
                  {"key":"register","label":"Contingency built from the register EMV plus a stated allowance for the R2 tail — with the 10% benchmark shown alongside as a sanity cross-check","quality":100,
                   "consequence":"The case carries a contingency that can answer 'why this number?' line by line — which the investment committee duly asks.",
                   "principle":"Benchmarks sanity-check a contingency; only the register can justify one."},
                  {"key":"percent","label":"Take finance's 10% — benchmarks exist because registers are always incomplete anyway","quality":25,
                   "consequence":"The 10% turns out generous in year 1 and hopeless in year 2 when R2 half-materialises; nobody can say what the number was FOR.",
                   "principle":"A percentage is a number about other projects; the register is a number about yours."},
                  {"key":"both","label":"Take whichever of the two numbers is larger, to be safe","quality":35,
                   "consequence":"Prudent-sounding, unjustifiable: the case now carries a contingency defined by 'whichever', which the committee trims arbitrarily because it was set arbitrarily.",
                   "principle":"A contingency without a basis invites cuts without a basis."}]}],
             "hints":["Net the four lines with signs — the decommissioning saving is a genuine opportunity.",
               "Compare the EMV total with what 10% of programme cost would be — the gap IS the conversation.",
               "Expected value funds the middle of the distribution; name the tail separately."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Built a business-case contingency the register could defend line by line."}
            """),

        // ───────────── Schedule Strategy · practitioner · CPM ─────────────

        ("WC-CPM-105", "The enabling works nobody scheduled", "Five solar sites, one enabling network, and a definition team arguing about the wrong activity.",
            "Renewables", "Programme Planner", "project_controls", "professional", 9,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A solar portfolio's shared enabling works — one network feeding all five sites — is being planned at definition. The grid team insists activity B (the substation bay) is 'obviously critical' and wants it accelerated. Run the network before anyone spends money on that instinct.",
             "evidence":[
               {"label":"A — Consents package","value":"2 days, no predecessors"},
               {"label":"B — Substation bay works","value":"4 days, after A"},
               {"label":"C — Access & haul roads","value":"3 days, after A"},
               {"label":"D — Cable route civils","value":"5 days, after B"},
               {"label":"E — Site compounds","value":"5 days, after C"},
               {"label":"F — Enabling handover","value":"2 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":2,"preds":[]},{"id":"B","dur":4,"preds":["A"]},{"id":"C","dur":3,"preds":["A"]},
               {"id":"D","dur":5,"preds":["B"]},{"id":"E","dur":5,"preds":["C"]},{"id":"F","dur":2,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Enabling duration (days)","type":"number"},
               {"key":"float_C","label":"Total float of C — access roads (days)","type":"number"},
               {"key":"float_E","label":"Total float of E — site compounds (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"instinct","prompt":"The grid team's 'accelerate B' instinct — your finding?",
                "options":[
                  {"key":"confirm","label":"Confirm B IS on the critical path — but show the compounds path is one day behind it, so any acceleration of B beyond a day just moves the criticality, not the date","quality":100,
                   "consequence":"The team buys exactly one day of B acceleration instead of three, and watches the near-critical path like it now deserves.",
                   "principle":"Acceleration is bounded by the NEXT path, not by enthusiasm for the current one."},
                  {"key":"agree","label":"Agree fully — critical is critical, accelerate B as much as the budget allows","quality":20,
                   "consequence":"Three days bought on B; the handover moves one day, because the compounds path quietly became the constraint after day one.",
                   "principle":"Crashing past the near-critical path donates money to the schedule gods."},
                  {"key":"dismiss","label":"Dismiss the instinct — instincts have no place in planning","quality":30,
                   "consequence":"The instinct happened to be half-right; dismissing it costs you the grid team's engagement with the plan they must deliver.",
                   "principle":"Test instincts with the network — validated instinct is how planners earn allies."}]}],
             "hints":["Trace both paths through to F and compare their lengths.",
               "An activity's float is how far it can slip before the end date moves.",
               "When you accelerate the critical path, check which path becomes critical next — and when."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Schedule Analyst","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Bounded an acceleration instinct with the near-critical path that would inherit it."}
            """),

        ("WC-CPM-106", "Planning the turnaround backwards", "The refinery gives you a fixed restart date. Everything else is yours to arrange.",
            "Downstream Energy", "Turnaround Planning Engineer", "project_controls", "professional", 11,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A refinery turnaround's definition plan has six major blocks and a contractually fixed restart date. The maintenance superintendent wants to know the minimum turnaround duration, which blocks can flex, and where the plan's real pressure point is — before the outage window is booked with the commercial team.",
             "evidence":[
               {"label":"A — Shutdown & decontamination","value":"3 days, no predecessors"},
               {"label":"B — Exchanger bundle pulls","value":"6 days, after A"},
               {"label":"C — Column internals inspection","value":"5 days, after A"},
               {"label":"D — Bundle repairs & retube","value":"2 days, after B"},
               {"label":"E — Column repairs & reassembly","value":"4 days, after C"},
               {"label":"F — Reinstatement & restart prep","value":"3 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},{"id":"B","dur":6,"preds":["A"]},{"id":"C","dur":5,"preds":["A"]},
               {"id":"D","dur":2,"preds":["B"]},{"id":"E","dur":4,"preds":["C"]},{"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Minimum turnaround duration (days)","type":"number"},
               {"key":"float_B","label":"Total float of B — bundle pulls (days)","type":"number"},
               {"key":"float_D","label":"Total float of D — bundle repairs (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"window","prompt":"Booking the outage window with commercial, you recommend:",
                "options":[
                  {"key":"buffered","label":"The computed minimum plus a visible, owned buffer sized to turnaround discovery risk — presented as two numbers, not one","quality":100,
                   "consequence":"Commercial books the buffered window knowing exactly what the buffer is for; when inspection finds two surprise repairs, the buffer absorbs them in daylight.",
                   "principle":"A buffer that is visible and owned gets spent on risk; one hidden inside activities gets spent on comfort."},
                  {"key":"minimum","label":"The computed minimum exactly — buffers invite Parkinson's law","quality":20,
                   "consequence":"The first discovery repair blows the window; the restart slips against a contractual date, which costs more per day than the whole buffer would have.",
                   "principle":"A fixed-date commitment at the theoretical minimum is a bet that inspection finds nothing."},
                  {"key":"padded","label":"The minimum with 30% silently added into each block's duration","quality":25,
                   "consequence":"Every block expands to its padded duration — Parkinson delivers — and when real discovery work appears there is somehow still no room.",
                   "principle":"Hidden padding is consumed by work; visible buffer is consumed by risk."}]}],
             "hints":["Compute both branch durations from A to F to find the driving path.",
               "Float on the shorter branch is the difference between the two branches.",
               "Decide separately: the network's minimum, and the risk buffer the WINDOW needs."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sized a refinery outage window with its buffer in daylight."}
            """),

        // ═════════════ MARCH — planning, sequencing and critical-path judgment ═════════════
        // ───────────── Schedule Strategy · practitioner ─────────────

        ("WC-SCH-107", "Float is a budget, not a rumour", "Two teams have both promised away the same four days.",
            "Capital Programmes", "Programme Planner", "project_controls", "professional", 9,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A programme integration schedule has six activities. The systems team told the client integration testing (E) 'has slack if needed'; separately, the design team promised the same slack to a late supplier feeding C. Run the network to find how much float actually exists on that branch — and who, if anyone, can spend it.",
             "evidence":[
               {"label":"A — Mobilise","value":"3 days, no predecessors"},
               {"label":"B — Core build","value":"5 days, after A"},
               {"label":"C — Interface design","value":"4 days, after A"},
               {"label":"D — Core commissioning","value":"6 days, after B"},
               {"label":"E — Integration testing","value":"4 days, after C"},
               {"label":"F — Programme handover","value":"2 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},{"id":"B","dur":5,"preds":["A"]},{"id":"C","dur":4,"preds":["A"]},
               {"id":"D","dur":6,"preds":["B"]},{"id":"E","dur":4,"preds":["C"]},{"id":"F","dur":2,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Programme duration (days)","type":"number"},
               {"key":"float_C","label":"Total float of C — interface design (days)","type":"number"},
               {"key":"float_E","label":"Total float of E — integration testing (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"ledger","prompt":"Both promises draw on the same branch. Your ruling?",
                "options":[
                  {"key":"single","label":"Publish the branch's float as ONE shared budget with an owner — spends are logged against it, and the two promises are reconciled to what remains","quality":100,
                   "consequence":"The supplier gets three days, testing keeps one, and the fourth promise dies in a meeting instead of on the critical path.",
                   "principle":"Float on a path is one budget however many activities sit on it — double-promising it is double-spending."},
                  {"key":"both","label":"Honour both promises — each activity 'has float', after all","quality":10,
                   "consequence":"Both teams spend the same days; the branch overruns the merge point and the handover slips with everyone technically correct.",
                   "principle":"Per-activity float readings hide the fact that a path shares one pool."},
                  {"key":"neither","label":"Revoke both promises — float belongs to the programme, not to teams","quality":40,
                   "consequence":"Defensible, but two commitments made in good faith are broken at once, and teams learn to stop telling the planner anything.",
                   "principle":"Reclaiming float without a spend process just drives float trading underground."}]}],
             "hints":["Work both paths through to F — the branch through C and E is one chain.",
               "Total float on that chain is shared: C and E do not each get their own copy.",
               "Treat the computed float as a budget: who owns it, what has been pledged, what remains."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reconciled two promises drawn on the same four days of float."}
            """),

        ("WC-SCH-108", "The shorter path the client watches", "The client's favourite milestone is not on the critical path. Nobody has told them.",
            "Framework Programmes", "Senior Planner", "project_controls", "professional", 10,
            """["schedule_analysis","stakeholder_communication"]""",
            """
            {"context":"A framework programme's definition schedule has six activities. The client tracks one milestone obsessively: completion of the pilot conversion (D). Your network says the pilot branch is not what drives the end date — the approvals branch is. The client has asked for 'everything possible' to accelerate D.",
             "evidence":[
               {"label":"A — Programme setup","value":"4 days, no predecessors"},
               {"label":"B — Pilot design","value":"4 days, after A"},
               {"label":"C — Approvals & consents","value":"7 days, after A"},
               {"label":"D — Pilot conversion","value":"5 days, after B"},
               {"label":"E — Approvals implementation","value":"4 days, after C"},
               {"label":"F — Definition close-out","value":"3 days, after D and E"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":4,"preds":[]},{"id":"B","dur":4,"preds":["A"]},{"id":"C","dur":7,"preds":["A"]},
               {"id":"D","dur":5,"preds":["B"]},{"id":"E","dur":4,"preds":["C"]},{"id":"F","dur":3,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Schedule duration (days)","type":"number"},
               {"key":"float_B","label":"Total float of B — pilot design (days)","type":"number"},
               {"key":"float_D","label":"Total float of D — pilot conversion (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"client","prompt":"The client wants D accelerated. You:",
                "options":[
                  {"key":"redirect","label":"Show the client the two branches: D can finish earlier but the END DATE is owned by approvals — offer to spend acceleration money where it moves what they actually care about","quality":100,
                   "consequence":"The client redirects the budget to expediting approvals and still gets the pilot two days early from its existing float.",
                   "principle":"Accelerate what drives the date the stakeholder cares about, not the milestone they happen to watch."},
                  {"key":"comply","label":"Accelerate D as asked — the client is paying and it's their milestone","quality":20,
                   "consequence":"D finishes early into its own float; the end date does not move; the client concludes acceleration 'doesn't work' just before you need them to fund a real one.",
                   "principle":"Money spent off the critical path buys a nicer-looking Gantt chart and nothing else."},
                  {"key":"quiet","label":"Accelerate approvals instead without explaining the switch","quality":30,
                   "consequence":"The right work gets faster and the client feels ignored; the next instruction arrives in writing, with less trust attached.",
                   "principle":"Doing the right thing without the explanation converts good planning into bad faith."}]}],
             "hints":["Compute both branches to F and see which one sets the duration.",
               "Float on the pilot branch tells you what acceleration there would actually buy.",
               "Answer the client's goal — the end date — not just their instruction."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Redirected a client's acceleration budget from the milestone they watch to the path that matters."}
            """),

        ("WC-SCH-109", "A milestone without a parent", "The contract lists a date no activity produces. The network has an opinion.",
            "Water Infrastructure", "Contract Planner", "project_controls", "professional", 8,
            """["schedule_analysis","governance"]""",
            """
            {"context":"A flood-alleviation contract carries a milestone — 'diversion channel operational, day 12' — inherited from the tender programme. In the definition schedule nobody can say which activities produce it: it sits in the plan with no predecessors, a date with no logic. Build the real network and see what the milestone's true date is.",
             "evidence":[
               {"label":"A — Site establishment","value":"2 days, no predecessors"},
               {"label":"B — Channel excavation","value":"5 days, after A"},
               {"label":"C — Lining works","value":"3 days, after B"},
               {"label":"D — Inlet structure","value":"4 days, after B"},
               {"label":"E — Diversion commissioning","value":"3 days, after C and D"},
               {"label":"F — Operational handover","value":"1 day, after E"},
               {"label":"Contract milestone","value":"'Diversion operational, day 12' — no logic behind it"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":2,"preds":[]},{"id":"B","dur":5,"preds":["A"]},{"id":"C","dur":3,"preds":["B"]},
               {"id":"D","dur":4,"preds":["B"]},{"id":"E","dur":3,"preds":["C","D"]},{"id":"F","dur":1,"preds":["E"]}]},
             "ask":[
               {"key":"project_duration","label":"Days to operational handover","type":"number"},
               {"key":"float_C","label":"Total float of C — lining works (days)","type":"number"},
               {"key":"float_D","label":"Total float of D — inlet structure (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"milestone","prompt":"The network says the contract's day-12 milestone is not achievable. You:",
                "options":[
                  {"key":"notify","label":"Notify the client NOW with the logic-linked programme attached: the true date, the drivers, and the compression options with prices","quality":100,
                   "consequence":"An awkward conversation in week one instead of a dispute in week three; the client buys one compression option and re-dates the milestone.",
                   "principle":"A milestone nobody's logic produces is a claim in incubation — surface it before it hatches."},
                  {"key":"hold","label":"Keep the milestone in the plan as a target and 'manage to it' — tender dates are commitments","quality":10,
                   "consequence":"The team works to a date the network never supported; day 12 arrives, the channel doesn't, and the failure is now yours to explain.",
                   "principle":"Managing to an impossible date is spelled d-e-l-a-y with extra steps."},
                  {"key":"resequence","label":"Quietly overlap lining and inlet works to chase the date without telling anyone the plan changed","quality":25,
                   "consequence":"The overlap creates a workspace clash the method statements prohibit; safety review halts the works and finds the unapproved resequence.",
                   "principle":"Logic changes that dodge review dodge the review that exists for a reason."}]}],
             "hints":["Trace the longest chain from A to F — both middle branches feed E.",
               "The milestone's earliest honest date is the network's, not the tender's.",
               "The gap between the contract date and the computed one is a decision for daylight."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave a logicless contract milestone its true date in week one."}
            """),

        ("WC-SCH-110", "The recovery that raided the test window", "The plan says month 6. Earned schedule says month 5's work just arrived.",
            "Enterprise Programmes", "Programme Controls Analyst", "project_controls", "professional", 11,
            """["earned_schedule","schedule_analysis"]""",
            """
            {"context":"A programme is at month 6 of a planned 8. The delivery lead's recovery plan 'holds the end date' — by compressing the final testing window. Before anyone signs, measure the schedule honestly in TIME: the plan's cumulative PV curve is below; earned value to date is 580.",
             "evidence":[
               {"label":"Planned duration","value":"8 months"},
               {"label":"Now","value":"End of month 6"},
               {"label":"Cumulative planned value","value":"M1: 80 · M2: 180 · M3: 300 · M4: 440 · M5: 580 · M6: 700 · M7: 800 · M8: 880 (thousands)"},
               {"label":"Earned value to date","value":"580 (thousands)"},
               {"label":"Recovery proposal","value":"Hold month 8 by halving the test window"}],
             "task":"earned_schedule",
             "given":{"planned_duration":8,"at":6,"ev":580,
               "plan":[{"period":1,"pv":80},{"period":2,"pv":180},{"period":3,"pv":300},{"period":4,"pv":440},
                       {"period":5,"pv":580},{"period":6,"pv":700},{"period":7,"pv":800},{"period":8,"pv":880}]},
             "ask":[
               {"key":"es","label":"Earned Schedule — ES (months)","type":"number"},
               {"key":"sv_time","label":"Time-based schedule variance SV(t)","type":"number"},
               {"key":"spi_time","label":"Time-based index SPI(t)","type":"number"},
               {"key":"eac_time","label":"Forecast duration EAC(t)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"recovery","prompt":"Your position on the hold-the-date recovery?",
                "options":[
                  {"key":"honest","label":"Present the measured EAC(t) and reject test-window compression as the mechanism — offer real options: scope re-phasing, resource addition, or an honest re-date","quality":100,
                   "consequence":"The board picks re-phasing plus a partial re-date; the test window survives, and so does the system it later catches two defects in.",
                   "principle":"A recovery that funds itself from assurance time isn't recovery — it is risk transfer to the go-live."},
                  {"key":"sign","label":"Sign the recovery — the end date is contractual and testing always squeezes anyway","quality":10,
                   "consequence":"The date holds on paper until testing finds what testing finds, with half the time to fix it; the slip arrives anyway, now WITH defects.",
                   "principle":"Compressing the window that proves the work does not compress the work."},
                  {"key":"average","label":"Split the difference: compress testing by a quarter and hope performance improves","quality":30,
                   "consequence":"SPI(t) has been stable for four months; hoping it improves is not a plan input, and the quarter-compression buys less than one month.",
                   "principle":"Trends are evidence; hope is not a schedule variable."}]}],
             "hints":["ES is the point on the PLAN curve where cumulative PV equals today's EV.",
               "SV(t) is ES minus actual time; SPI(t) is ES divided by actual time.",
               "EAC(t) projects the planned duration over SPI(t) — compare it with the proposal's promise."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Measured a slip in months, not currency — and saved the test window from paying for it."}
            """),

        ("WC-SCH-111", "Do the ugly work first", "Everyone wants to start with the clean scope. The network disagrees.",
            "Energy Networks", "Outage Planner", "project_controls", "professional", 12,
            """["schedule_analysis","sequencing"]""",
            """
            {"context":"Planning a substation outage's enabling phase, both site teams want to begin with the well-understood scaffold and cabling work and leave the contaminated-insulation removal — slow, unpopular, permit-heavy — until 'the works are flowing'. Run the network: the removal gates the weld repairs, which gate everything.",
             "evidence":[
               {"label":"A — Permits & isolations","value":"2 days, no predecessors"},
               {"label":"B — Scaffold access","value":"3 days, after A"},
               {"label":"C — Insulation removal (contaminated)","value":"4 days, after A"},
               {"label":"D — Weld repairs","value":"6 days, after B and C"},
               {"label":"E — Reinstatement","value":"2 days, after D"},
               {"label":"Preference","value":"Both teams want C late — 'once the works are flowing'"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":2,"preds":[]},{"id":"B","dur":3,"preds":["A"]},{"id":"C","dur":4,"preds":["A"]},
               {"id":"D","dur":6,"preds":["B","C"]},{"id":"E","dur":2,"preds":["D"]}]},
             "ask":[
               {"key":"project_duration","label":"Enabling duration (days)","type":"number"},
               {"key":"float_B","label":"Total float of B — scaffold (days)","type":"number"},
               {"key":"float_C","label":"Total float of C — insulation removal (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"sequence","prompt":"The teams push to start C late anyway. Your call?",
                "options":[
                  {"key":"front","label":"Hold C at its early start — it is the zero-float gate to everything — and show the teams the network that says so","quality":100,
                   "consequence":"The removal starts day one behind the permits; when it uncovers extra contamination, the discovery happens with the whole float budget still alive.",
                   "principle":"Front-load the work most likely to surprise you — discovery is cheapest when the schedule is youngest."},
                  {"key":"defer","label":"Let C start after B — team morale is worth two days","quality":15,
                   "consequence":"C starts late, finds the extra contamination late, and the outage extension request lands exactly when the system operator has no flexibility left.",
                   "principle":"Deferring uncertain work moves its surprises to where they cost the most."},
                  {"key":"split","label":"Split C into a survey slice now and removal later","quality":55,
                   "consequence":"Better than pure deferral — the survey de-risks the estimate — but the removal duration itself still lands late in the window.",
                   "principle":"Sampling uncertainty early helps; it is still not the same as retiring it early."}]}],
             "hints":["Find which branch into D is longer — that branch owns the outage's start-critical work.",
               "Zero float on C means every day it waits is a day the whole outage extends.",
               "Ask which activity is most likely to produce surprises, and when you want to meet them."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Scheduled the unpopular work first, where its surprises were affordable."}
            """),

        ("WC-SCH-112", "Three estimates for one calendar", "The site swears ten weeks. The office swears six. PERT holds the pen.",
            "Technology Rollouts", "Rollout Planning Analyst", "project_controls", "professional", 9,
            """["schedule_analysis","estimating"]""",
            """
            {"context":"A national rollout's definition plan needs a duration for the three-stage store-conversion pipeline, and the site and office teams are four weeks apart on gut feel. You gather proper three-point estimates for the three sequential stages and let the arithmetic speak — then decide what date to put in the plan. The client wants a commitment at 27 days.",
             "evidence":[
               {"label":"A — Survey & make-ready","value":"optimistic 6, most likely 9, pessimistic 18 days"},
               {"label":"B — Conversion","value":"optimistic 4, most likely 6, pessimistic 8 days"},
               {"label":"C — Commission & handback","value":"optimistic 5, most likely 8, pessimistic 11 days"},
               {"label":"Client ask","value":"Commit the pipeline at 27 days"}],
             "task":"pert",
             "given":{"activities":[
               {"id":"A","o":6,"m":9,"p":18},{"id":"B","o":4,"m":6,"p":8},{"id":"C","o":5,"m":8,"p":11}],
               "deadline":27},
             "ask":[
               {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
               {"key":"std_dev","label":"Pipeline standard deviation (days)","type":"number"},
               {"key":"prob_on_time","label":"Probability of finishing within 27 days (%)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"commit","prompt":"What commitment goes to the client?",
                "options":[
                  {"key":"confidence","label":"Offer the expected duration for planning and a commitment date at a stated confidence level — with the probability arithmetic on the table","quality":100,
                   "consequence":"The client chooses the higher-confidence date once they can see what 27 days actually buys; the first ten stores all land inside it.",
                   "principle":"Commit at a confidence level you can name, not at a number someone liked."},
                  {"key":"expected","label":"Commit at the expected duration exactly — it is the statistically fair number","quality":30,
                   "consequence":"Fair, and roughly a coin flip per store: half the pipeline runs 'late' against a commitment that was never a commitment.",
                   "principle":"The expected value is a planning number; a commitment needs headroom you chose on purpose."},
                  {"key":"client","label":"Accept the client's 27 days — the probability is high enough and the relationship matters","quality":45,
                   "consequence":"Workable this time — but nobody recorded that 27 was a probability choice, so the next negotiation starts from it as a fact.",
                   "principle":"If you accept a probabilistic date, record the probability you accepted."}]}],
             "hints":["Each stage's expected duration weights the most likely estimate four times: (o + 4m + p) / 6.",
               "Stage variances add along the path; the path standard deviation is the square root of the sum.",
               "The client's 27 days is a point on a distribution — find out which point before promising it."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a four-week estimating argument into one distribution and a chosen confidence."}
            """),

        ("WC-SCH-113", "The trend already voted", "The plan says the programme recovers next quarter. Six months of SPI(t) say otherwise.",
            "Portfolio & PMO", "Schedule Assurance Analyst", "project_controls", "professional", 10,
            """["earned_schedule","forecasting"]""",
            """
            {"context":"A portfolio's flagship workstream is at month 5 of a planned 8, and its report claims the end date holds 'based on planned acceleration'. You run the time-based measure from the PV curve: earned value to date is 520. The steering group asks for the honest forecast before it endorses the report.",
             "evidence":[
               {"label":"Planned duration","value":"8 months"},
               {"label":"Now","value":"End of month 5"},
               {"label":"Cumulative planned value","value":"M1: 100 · M2: 220 · M3: 360 · M4: 520 · M5: 700 · M6: 860 · M7: 980 · M8: 1060 (thousands)"},
               {"label":"Earned value to date","value":"520 (thousands)"},
               {"label":"Report claim","value":"'End date holds, based on planned acceleration'"}],
             "task":"earned_schedule",
             "given":{"planned_duration":8,"at":5,"ev":520,
               "plan":[{"period":1,"pv":100},{"period":2,"pv":220},{"period":3,"pv":360},{"period":4,"pv":520},
                       {"period":5,"pv":700},{"period":6,"pv":860},{"period":7,"pv":980},{"period":8,"pv":1060}]},
             "ask":[
               {"key":"es","label":"Earned Schedule — ES (months)","type":"number"},
               {"key":"spi_time","label":"Time-based index SPI(t)","type":"number"},
               {"key":"eac_time","label":"Forecast duration EAC(t)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"endorse","prompt":"Does the steering group endorse 'the end date holds'?",
                "options":[
                  {"key":"trend","label":"No — present the measured EAC(t) as the base forecast, and admit the 'planned acceleration' only if it comes with named resources, funded actions and a revised curve","quality":100,
                   "consequence":"The workstream returns a week later with a real acceleration package for half the gap and a re-dated remainder — a forecast the group can actually govern.",
                   "principle":"A trend is overturned by funded actions, never by adjectives."},
                  {"key":"endorse","label":"Yes — the team knows its work and acceleration plans deserve good faith","quality":10,
                   "consequence":"Three reports later the date moves by exactly what SPI(t) predicted today, plus the credibility of everyone who endorsed it.",
                   "principle":"Good faith is for people; forecasts get evidence."},
                  {"key":"midpoint","label":"Endorse a date halfway between the report's and the measured forecast","quality":20,
                   "consequence":"A number with no mechanism behind it satisfies the meeting and binds nobody; both dates are now wrong in the minutes.",
                   "principle":"Averaging a measurement with a wish produces neither."}]}],
             "hints":["Find where the plan curve reaches today's EV — that month is the ES.",
               "SPI(t) = ES over actual months elapsed; below one means the calendar is losing.",
               "Project the planned duration over SPI(t) and compare with the report's claim."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Let six months of SPI(t) outvote one adjective in a steering pack."}
            """),

        ("WC-SCH-114", "The handover date, third edition", "Twice moved, once by hope. This time the network sets it.",
            "Urban Development", "Development Planner", "project_controls", "professional", 8,
            """["schedule_analysis","critical_path"]""",
            """
            {"context":"A mixed-use development's phase-one handover date has moved twice — both times reset by negotiation rather than analysis. The incoming tenant's lawyers want a third date they can put liquidated damages against. This time, build it from the network.",
             "evidence":[
               {"label":"A — Complete core works","value":"3 days, no predecessors"},
               {"label":"B — Lobby fit-out","value":"4 days, after A"},
               {"label":"C — Building systems integration","value":"6 days, after A"},
               {"label":"D — Fit-out completion","value":"3 days, after B"},
               {"label":"E — Systems acceptance testing","value":"2 days, after C"},
               {"label":"F — Handover certification","value":"2 days, after D and E"},
               {"label":"History","value":"Two previous dates, both negotiated, both missed"}],
             "task":"cpm",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},{"id":"B","dur":4,"preds":["A"]},{"id":"C","dur":6,"preds":["A"]},
               {"id":"D","dur":3,"preds":["B"]},{"id":"E","dur":2,"preds":["C"]},{"id":"F","dur":2,"preds":["D","E"]}]},
             "ask":[
               {"key":"project_duration","label":"Days to certified handover","type":"number"},
               {"key":"float_B","label":"Total float of B — lobby fit-out (days)","type":"number"},
               {"key":"float_D","label":"Total float of D — fit-out completion (days)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"date","prompt":"The date that goes to the tenant's lawyers is:",
                "options":[
                  {"key":"network_risked","label":"The network date plus a disclosed risk allowance sized to the systems-testing uncertainty — with the build-up shown to the tenant","quality":100,
                   "consequence":"The tenant's lawyers, shown how the date was built, accept it first time; the third date becomes the last date.",
                   "principle":"A date that can show its arithmetic survives negotiation; a date that cannot, restarts it."},
                  {"key":"network_exact","label":"The bare network date — allowances just invite Parkinson's law","quality":25,
                   "consequence":"Systems testing finds what testing finds, the bare date breaks by two days, and the LDs you invited apply to you.",
                   "principle":"A commitment with liquidated damages attached is priced risk — price it."},
                  {"key":"negotiate","label":"Whatever date the tenant's side will accept — dates are commercial, not technical","quality":10,
                   "consequence":"The third negotiated date joins the first two, for the same reason: nothing underneath it.",
                   "principle":"A negotiated date without a network under it is a countdown to the fourth negotiation."}]}],
             "hints":["The longer branch through systems work drives certification — compute both.",
               "Floats on the fit-out branch tell you what is genuinely flexible in the tenant conversation.",
               "A contractual date needs the network date PLUS a risk allowance you can defend line by line."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Built a tenant handover date that could show its own arithmetic."}
            """),

        ("WC-SCH-115", "Eighty percent done, twice", "The line conversion reported 80% in April. And in June. Weighted progress explains why.",
            "Industrial Manufacturing", "Project Controls Engineer", "project_controls", "professional", 11,
            """["progress_measurement","schedule_analysis"]""",
            """
            {"context":"A production-line conversion reported '80% complete' two months running — the site counts activities, not value. You rebuild the measure with budget-weighted progress from the four control accounts. The steering meeting expects the honest number and an explanation of why it moved backwards from the site's version.",
             "evidence":[
               {"label":"1.1 Strip-out & services","value":"weight 400,000 · 100% complete"},
               {"label":"1.2 Line equipment install","value":"weight 900,000 · 70% complete"},
               {"label":"1.3 Controls & integration","value":"weight 500,000 · 30% complete"},
               {"label":"1.4 Commissioning & ramp","value":"weight 200,000 · 0% complete"},
               {"label":"Site figure","value":"'80% complete' (activity count)"}],
             "task":"progress",
             "given":{"nodes":[
               {"id":"1.1","name":"Strip-out","weight":400000,"percent":100},
               {"id":"1.2","name":"Equipment","weight":900000,"percent":70},
               {"id":"1.3","name":"Controls","weight":500000,"percent":30},
               {"id":"1.4","name":"Commissioning","weight":200000,"percent":0}]},
             "ask":[
               {"key":"overall_percent","label":"Budget-weighted percent complete","type":"number"},
               {"key":"total_weight","label":"Total weighting (budget)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"explain","prompt":"How do you land the corrected number at the steering meeting?",
                "options":[
                  {"key":"method","label":"Present both numbers WITH the method difference — activity count versus value weighting — and adopt the weighted measure as the single go-forward basis","quality":100,
                   "consequence":"The room understands why '80%' was true-ish and useless; the weighted measure becomes the standard and the next report is comparable.",
                   "principle":"Correct a measure by replacing the method, not just the number — or the old method regrows."},
                  {"key":"silent","label":"Just report the weighted figure without mentioning the site's 80%","quality":25,
                   "consequence":"The site team, blindsided by the 'drop', spends the meeting defending its old number instead of discussing the work.",
                   "principle":"An unexplained correction reads as an accusation."},
                  {"key":"blend","label":"Report a blended figure between the two methods to soften the transition","quality":5,
                   "consequence":"A number computable by no one is now in the minutes; next month's 'progress' depends on which method regressed less.",
                   "principle":"Two methods averaged is zero methods."}]}],
             "hints":["Weight each account's percent by its budget share, then sum.",
               "Notice where the remaining value sits — the heavy accounts are barely started.",
               "The meeting needs the METHOD change explained, or the number change will be litigated."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Replaced an activity-count 80% with a value-weighted truth."}
            """),

        // ───────────── Cost & Value · foundation ─────────────

        ("WC-CBS-116", "Where the estate money stands", "Three cost accounts, one rolled-up truth for the estates board.",
            "Public Estates", "Estates Cost Analyst", "project_controls", "foundation", 12,
            """["cost_control","reporting"]""",
            """
            {"context":"A schools estate programme's definition-stage cost report rolls up three cost accounts for the estates board. Roll the structure up, state the variance at the root — and handle the fact that the worst account's manager has asked you to 'present the overspend as timing'.",
             "evidence":[
               {"label":"1.1 Condition surveys & design","value":"budget 1,200,000 · actual 1,180,000"},
               {"label":"1.2 Early enabling works","value":"budget 800,000 · actual 860,000"},
               {"label":"1.3 Programme management","value":"budget 500,000 · actual 505,000"},
               {"label":"Request","value":"1.2's manager: 'present it as timing'"}],
             "task":"cbs",
             "given":{"nodes":[
               {"id":"1","parent":null,"name":"Estate programme"},
               {"id":"1.1","parent":"1","name":"Surveys & design","budget":1200000,"actual":1180000},
               {"id":"1.2","parent":"1","name":"Enabling works","budget":800000,"actual":860000},
               {"id":"1.3","parent":"1","name":"Programme mgmt","budget":500000,"actual":505000}]},
             "ask":[
               {"key":"root_budget","label":"Total budget (root)","type":"number"},
               {"key":"root_actual","label":"Total actual (root)","type":"number"},
               {"key":"root_variance","label":"Root variance (budget − actual)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"timing","prompt":"The 'present it as timing' request. You:",
                "options":[
                  {"key":"test","label":"Test the claim: if it IS timing, the account's commitments and forecast will show the recovery — ask for that evidence, and report whichever story the evidence supports","quality":100,
                   "consequence":"The evidence shows half timing, half genuine overspend; the report says exactly that, and the board trusts the next one.",
                   "principle":"'Timing' is a testable claim, not a euphemism — test it."},
                  {"key":"comply","label":"Report it as timing — account managers know their accounts","quality":10,
                   "consequence":"The 'timing' never reverses; three reports later the board asks when the money is coming back, and the answer implicates the reporter.",
                   "principle":"Every unverified 'timing' variance is an overspend on deferred disclosure."},
                  {"key":"refuse","label":"Report raw numbers only — narratives are spin by definition","quality":30,
                   "consequence":"Accurate and unhelpful: the board sees a variance with no explanation and invents a worse one.",
                   "principle":"Numbers without narrative don't prevent stories; they just outsource them."}]}],
             "hints":["Roll budget and actual up separately, then take the variance at the root.",
               "Watch the signs: an actual above budget is a negative variance in this convention.",
               "A timing claim predicts its own reversal — ask to see the forecast that shows it."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rolled up an estate cost report and tested a 'timing' story against evidence."}
            """),

        ("WC-CSH-117", "The programme that must borrow to breathe", "Profitable on paper, hungry in month two. Find the peak before the bank does.",
            "Capital Programmes", "Programme Finance Analyst", "project_controls", "foundation", 10,
            """["cash_flow","cost_control"]""",
            """
            {"context":"A capital programme's definition-stage funding paper needs its cash profile. The five-period forecast below is profitable overall — but the funding committee needs the peak funding requirement, because that, not the profit, is what the facility must cover.",
             "evidence":[
               {"label":"Period 1","value":"inflow 0 · outflow 300,000"},
               {"label":"Period 2","value":"inflow 100,000 · outflow 450,000"},
               {"label":"Period 3","value":"inflow 600,000 · outflow 350,000"},
               {"label":"Period 4","value":"inflow 800,000 · outflow 200,000"},
               {"label":"Period 5","value":"inflow 300,000 · outflow 100,000"}],
             "task":"cashflow",
             "given":{"periods":[
               {"period":1,"inflow":0,"outflow":300000},{"period":2,"inflow":100000,"outflow":450000},
               {"period":3,"inflow":600000,"outflow":350000},{"period":4,"inflow":800000,"outflow":200000},
               {"period":5,"inflow":300000,"outflow":100000}]},
             "ask":[
               {"key":"final_position","label":"Final cash position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"},
               {"key":"cumulative_2","label":"Cumulative position, end of period 2","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"facility","prompt":"What does the funding paper request?",
                "options":[
                  {"key":"peak_plus","label":"A facility sized on the computed peak plus a stated headroom for receipt timing risk — with the cumulative curve shown","quality":100,
                   "consequence":"The committee sees exactly why the number is what it is; when period-3 receipts land two weeks late, the headroom absorbs it.",
                   "principle":"Facilities are sized on the trough of the curve, plus the honesty to admit receipts slip."},
                  {"key":"final","label":"A small facility — the programme ends cash-positive, after all","quality":5,
                   "consequence":"The programme is solvent in period 5 and insolvent in period 2, which is the only period that matters to an unpaid contractor.",
                   "principle":"Profitability is an end-state; liquidity is every Tuesday."},
                  {"key":"round","label":"A round number comfortably above any scenario, to avoid going back twice","quality":30,
                   "consequence":"The committee funds it, notices the padding at first drawdown review, and trims the NEXT programme's honest request in revenge.",
                   "principle":"Unexplained headroom spends credibility that explained headroom would have banked."}]}],
             "hints":["Run the cumulative position period by period: prior balance plus inflow minus outflow.",
               "The peak funding requirement is the deepest negative point of that running balance.",
               "The final position answers a different question from the peak — the paper needs both."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sized a programme funding facility on the trough, not the happy ending."}
            """),

        ("WC-BOQ-118", "Price the alternative before you praise it", "The value-engineering option sounds cheaper. The bill will decide.",
            "Healthcare Estates", "Cost Planner", "project_controls", "foundation", 11,
            """["estimating","value_engineering"]""",
            """
            {"context":"A hospital ward refurbishment's definition estimate is under pressure, and the design team proposes a value-engineering alternative for the wall protection package: fewer, higher-grade panels. Enthusiasm is high. Price the alternative's bill properly before anyone calls it a saving.",
             "evidence":[
               {"label":"Line A — Impact-rated panels","value":"250 m² at rate 120"},
               {"label":"Line B — Corner & door protection","value":"80 units at rate 310"},
               {"label":"Line C — Standard hygiene cladding","value":"500 m² at rate 22"},
               {"label":"Line D — Specialist fixings & trims","value":"40 sets at rate 450"},
               {"label":"Baseline package","value":"Current estimate carries 91,500"}],
             "task":"boq",
             "given":{"lines":[
               {"id":"A","qty":250,"rate":120},{"id":"B","qty":80,"rate":310},
               {"id":"C","qty":500,"rate":22},{"id":"D","qty":40,"rate":450}]},
             "ask":[
               {"key":"total","label":"Alternative bill total","type":"number"},
               {"key":"line_count","label":"Number of bill lines","type":"number"},
               {"key":"average_rate","label":"Average line rate (mean of the four rates)","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"verdict","prompt":"The bill total versus the 91,500 baseline — your recommendation?",
                "options":[
                  {"key":"whole_life","label":"Report the capital comparison honestly AND ask for the maintenance-cycle comparison before the VE decision is made — panels with different lives are different costs","quality":100,
                   "consequence":"The alternative costs more capital but halves the repaint cycle; the whole-life view approves it for the corridors and rejects it for the plant rooms.",
                   "principle":"Value engineering compares value, which has a time axis — not just this year's bill."},
                  {"key":"reject","label":"Reject the alternative — the bill says it is not cheaper, end of analysis","quality":30,
                   "consequence":"Arithmetically clean; the maintenance saving nobody priced walks out the door with the decision.",
                   "principle":"A capital-only comparison is a partial answer delivered with full confidence."},
                  {"key":"approve","label":"Approve it — the design team's enthusiasm reflects real site experience","quality":10,
                   "consequence":"Enthusiasm meets arithmetic at the next cost report, and loses; the estimate absorbs the difference silently.",
                   "principle":"Enthusiasm is a reason to price something, never a substitute for pricing it."}]}],
             "hints":["Each line is quantity times rate; the bill is the sum of the lines.",
               "The average line rate here is the simple mean of the four rates — quantities do not weight it.",
               "Compare against the baseline — then ask what the comparison leaves out (lives, cycles, maintenance)."],
             "profile_map":{"calculation":"Cost Guardian","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Priced a value-engineering option before its enthusiasm priced itself."}
            """),

        // ───────────── Risk Room · practitioner ─────────────

        ("WC-RSK-119", "The register meets the review board", "Four lines, one challenge session, and a probability someone made up in a lift.",
            "Enterprise Programmes", "Risk Review Facilitator", "project_controls", "professional", 9,
            """["risk_management","governance"]""",
            """
            {"context":"A transformation programme's planning-stage register faces its first independent review board. Before the challenge session, the board wants the quantified position — net EMV and the two dominant exposures. During the session, R2's owner admits the 0.15 probability 'came from a corridor conversation'.",
             "evidence":[
               {"label":"R1 — Integration partner underperformance","value":"probability 0.35, impact -400,000"},
               {"label":"R2 — Regulatory rule change mid-build","value":"probability 0.15, impact -1,400,000"},
               {"label":"R3 — Data cleansing underestimate","value":"probability 0.45, impact -200,000"},
               {"label":"R4 — Early licence retirement saving","value":"probability 0.40, impact +180,000"},
               {"label":"Session admission","value":"R2's probability 'came from a corridor conversation'"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.35,"impact":-400000},{"id":"R2","probability":0.15,"impact":-1400000},
               {"id":"R3","probability":0.45,"impact":-200000},{"id":"R4","probability":0.4,"impact":180000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R2","label":"EMV of R2 — regulatory change","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — data cleansing","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"corridor","prompt":"R2's corridor-sourced probability dominates the register. The board should:",
                "options":[
                  {"key":"evidence","label":"Commission a proper basis for R2 — regulatory counsel's view, consultation-stage analysis — and mark the register EMV as provisional on that line until it lands","quality":100,
                   "consequence":"Counsel's view moves the probability materially; the register's biggest number now stands on something other than a lift chat.",
                   "principle":"The biggest line in the register deserves the best-evidenced probability, not the most convenient one."},
                  {"key":"keep","label":"Keep 0.15 — expert intuition is a legitimate estimating basis and the owner is experienced","quality":25,
                   "consequence":"Intuition is legitimate as a starting point; unexamined at this exposure it is the register's single largest unaudited assumption.",
                   "principle":"Expert judgment earns its place through challenge, not through seniority."},
                  {"key":"zero","label":"Strike R2 until it has a defensible probability — no basis, no line","quality":15,
                   "consequence":"The register's dominant threat vanishes from the arithmetic entirely, which is a stronger claim than any probability would have been.",
                   "principle":"Removing a poorly-estimated risk asserts a probability of zero — the least defensible estimate of all."}]}],
             "hints":["Net the four lines, opportunity included, before the challenge session.",
               "Notice which single line drives the total — that is where estimating quality matters most.",
               "A weak basis is fixed by better evidence, not by deletion or deference."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put the register's biggest probability on evidence instead of a corridor."}
            """),

        ("WC-RSK-120", "Drawdown at definition", "The project hasn't started and already wants the contingency.",
            "Bridges & Crossings", "Programme Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","cost_control"]""",
            """
            {"context":"A crossing scheme in definition holds contingency sized at its register's net EMV. The design team requests an immediate drawdown of a third of it — to fund additional ground investigation that would 'retire R3 before sanction'. Quantify the register first; then judge the request.",
             "evidence":[
               {"label":"R1 — Approach settlement design rework","value":"probability 0.50, impact -280,000"},
               {"label":"R2 — Environmental survey season slip","value":"probability 0.25, impact -700,000"},
               {"label":"R3 — Foundation conditions worse than desk study","value":"probability 0.10, impact -1,900,000"},
               {"label":"R4 — Early contractor engagement saving","value":"probability 0.35, impact +140,000"},
               {"label":"Request","value":"Draw a third of contingency for extra ground investigation"}],
             "task":"risk",
             "given":{"risks":[
               {"id":"R1","probability":0.5,"impact":-280000},{"id":"R2","probability":0.25,"impact":-700000},
               {"id":"R3","probability":0.1,"impact":-1900000},{"id":"R4","probability":0.35,"impact":140000}]},
             "ask":[
               {"key":"emv","label":"Net register EMV","type":"number"},
               {"key":"emv_R1","label":"EMV of R1 — settlement rework","type":"number"},
               {"key":"emv_R3","label":"EMV of R3 — foundation conditions","type":"number"}],
             "tolerance":0.01,
             "decisions":[
               {"key":"request","prompt":"Your recommendation on the drawdown?",
                "options":[
                  {"key":"scope","label":"Support the investigation, funded as definition scope through change control — and re-run the register when its results land, resizing contingency to what remains","quality":100,
                   "consequence":"The boreholes go in as priced scope; R3's probability halves on the results, and contingency is resized from evidence, not raided in advance.",
                   "principle":"Buying information is scope; contingency is for the risks the information hasn't retired yet."},
                  {"key":"grant","label":"Grant the drawdown — retiring the tail risk is exactly what the money is for","quality":30,
                   "consequence":"The investigation happens, but contingency is now down a third with every register line still open — the arithmetic of the remaining cover no longer matches any register anyone approved.",
                   "principle":"Spending contingency on mitigation unsizes it from the register that justified it."},
                  {"key":"deny","label":"Deny — nothing draws on contingency before sanction, as a matter of principle","quality":20,
                   "consequence":"The principle holds and the boreholes wait for sanction — after which their findings arrive too late to shape the design they were meant to inform.",
                   "principle":"A rule that delays cheap information until it is expensive information is protecting the wrong thing."}]}],
             "hints":["EMV each line with signs; the contractor-engagement line is an opportunity.",
               "Note the gap between R3's expected value and its full impact — that is the tail the investigation targets.",
               "Distinguish paying to LEARN about a risk from paying because a risk HAPPENED."],
             "profile_map":{"calculation":"Risk Strategist","decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Funded the boreholes as scope and kept the contingency honest."}
            """),

        ("WC-RSK-121", "Three documents, one real risk", "A register line, a trend chart and a supplier letter. Only one of them is shouting.",
            "Technology Rollouts", "Programme Risk Analyst", "project_controls", "professional", 12,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"A national rollout's planning review gives you three artifacts. The register's top-rated line is installer availability (red, much-discussed). A buried monthly trend shows survey-to-install conversion falling four months straight — 92, 84, 71, 63 percent. And a routine letter from the sole certified hardware supplier 'notes' that its component allocation is under review at group level. The review board asks: what is this programme's real emerging risk?",
             "evidence":[
               {"label":"Register top line","value":"Installer availability — red, well-staffed with actions"},
               {"label":"Trend (buried)","value":"Survey-to-install conversion: 92 → 84 → 71 → 63 percent over 4 months"},
               {"label":"Supplier letter","value":"Sole certified supplier: component allocation 'under review at group level'"},
               {"label":"Board question","value":"'What is the real emerging risk here?'"}],
             "decisions":[
               {"key":"diagnose","prompt":"Reading the three artifacts together, the signal that most demands escalation is:",
                "options":[
                  {"key":"supplier","label":"The supplier letter — a sole-source allocation 'under review' is a polite notice of possible supply loss, with no register line, no owner and no plan","quality":100,
                   "consequence":"Engagement with the supplier's group confirms allocations are being cut for a larger customer; caught now, an alternative certification path starts eight months before it is needed.",
                   "principle":"The loudest risk is rarely the newest one — read routine correspondence as evidence, not filing."},
                  {"key":"register","label":"The register's red line — it is the register's own top rating and ratings exist to be believed","quality":15,
                   "consequence":"The well-staffed known risk gets more staffing; the unregistered supply risk matures quietly into a stoppage.",
                   "principle":"A register describes yesterday's analysis; emerging risk lives in what the register hasn't met yet."},
                  {"key":"trend","label":"The conversion trend — four consecutive drops is the only quantified deterioration on the table","quality":55,
                   "consequence":"Real and worth chasing — but its likeliest root cause surfaces in the survey backlog data within weeks either way; the supply signal has no such self-announcing mechanism.",
                   "principle":"Prefer the signal that will NOT surface itself if you ignore it."}]},
               {"key":"respond","prompt":"For the signal you escalated, the right first move is:",
                "options":[
                  {"key":"engage","label":"Direct engagement at the right level to turn the ambiguity into facts — scope, timing, alternatives — feeding a new register line with an owner","quality":100,
                   "consequence":"Facts replace the polite phrase; the register gains its most important line of the quarter, with a mitigation already moving.",
                   "principle":"An emerging risk's first treatment is always the same: convert it from a phrase into facts with an owner."},
                  {"key":"monitor","label":"Add it to the watch list and revisit at next month's review","quality":20,
                   "consequence":"The next review inherits the same sentence with four fewer weeks of options attached.",
                   "principle":"Watching an ambiguity is not an action; it is scheduled inaction."}]}],
             "hints":["Ask of each artifact: if this signal is real, what is the blast radius, and who would tell you in time?",
               "Sole-source plus 'allocation under review' is a supply risk wearing courtesy language.",
               "Escalation priority follows detectability as much as impact — some risks won't announce themselves twice."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the programme's real risk in a polite letter nobody had filed as one."}
            """),

        // ───────────── Executive Mission · capstone ─────────────

        ("WC-CAP-122", "The corridor decade", "One transport corridor, ten years, three decisions that decide the rest.",
            "Transport Corridors", "Corridor Programme Director", "project_management", "expert", 24,
            """["governance","strategy_execution","schedule_analysis"]""",
            """
            {"context":"You direct the definition of a ten-year multimodal transport corridor programme: junction rebuilds, rail capacity works and active-travel routes sharing one corridor, one funding envelope and one weary set of communities. The definition gateway wants your position on phasing strategy, the delivery-market approach, and what to commit to the communities in writing.",
             "evidence":[
               {"label":"Phasing options","value":"Geographic (end-to-end by section) · Modal (rail, then road, then active travel) · Outcome-led (worst bottlenecks first, any mode)"},
               {"label":"Analysis","value":"Three bottlenecks cause 60% of corridor delay; each involves two modes"},
               {"label":"Market","value":"Regional contractors can deliver sections; only two national players could take the whole corridor"},
               {"label":"Communities","value":"Consultation fatigue high; two previous schemes over-promised"},
               {"label":"Funding","value":"Envelope confirmed for years 1–4 only; later years indicative"}],
             "decisions":[
               {"key":"phasing","prompt":"Stage 1 — phasing strategy?",
                "options":[
                  {"key":"outcome","label":"Outcome-led: attack the three multi-modal bottlenecks first, sequenced so each phase's disruption windows are shared across modes","quality":100,
                   "consequence":"Sixty percent of the corridor's pain is addressed inside the funded four years; later phases inherit momentum and a public that has seen results.",
                   "principle":"Phase by the outcomes the funding horizon can actually reach, not by the tidiness of maps or modes."},
                  {"key":"geographic","label":"Geographic: complete the corridor section by section, end to end","quality":25,
                   "consequence":"Section one is beautiful and the worst bottleneck — in section four — waits six years; the programme's public case erodes annually.",
                   "principle":"Geographic tidiness delivers benefit in the order of the map, not the order of the need."},
                  {"key":"modal","label":"Modal: rail first — its consents are longest — then road, then active travel","quality":35,
                   "consequence":"Defensible on consent logic, but every community experiences three separate waves of disruption at the same junctions over a decade.",
                   "principle":"A sequencing that is efficient for the programme and brutal for its neighbours is not efficient."}]},
               {"key":"market","prompt":"Stage 2 — delivery-market approach?",
                "options":[
                  {"key":"mixed","label":"A framework of regional contractors for section works, with the three bottleneck phases tendered as integrated multi-modal packages the nationals can lead","quality":100,
                   "consequence":"Regional capacity is kept warm and competitive for a decade; the complex packages get the integrators they need without handing the corridor to a duopoly.",
                   "principle":"Match package shape to market depth — and never let one procurement decision define a decade of leverage."},
                  {"key":"mega","label":"One corridor-wide contract with a national player — single accountability","quality":15,
                   "consequence":"Year one is smooth; by year four the corridor has one indispensable supplier, and every variation is priced accordingly.",
                   "principle":"Single accountability at corridor scale is another name for single dependency."},
                  {"key":"atomised","label":"Tender every scheme separately for maximum competition","quality":30,
                   "consequence":"Forty procurements, forty interfaces, and the multi-modal bottleneck jobs fail to attract bidders who can integrate them.",
                   "principle":"Competition per package is not the same as value across a programme."}]},
               {"key":"communities","prompt":"Stage 3 — the written commitment to communities?",
                "options":[
                  {"key":"honest","label":"Commit to the funded phase-one outcomes with dates, name the later phases as unfunded intentions, and promise per-phase disruption budgets the programme will publish and track","quality":100,
                   "consequence":"The corridor's communities get their first keepable promise in a decade; the disruption budgets become the programme's most-watched metric and its best discipline.",
                   "principle":"Promise what is funded, label what is hoped, and give the community a number it can hold you to."},
                  {"key":"vision","label":"Publish the full ten-year vision with indicative dates throughout — ambition builds support","quality":20,
                   "consequence":"Year six's funding squeeze turns the indicative dates into the third broken promise; opposition to phase four is organised around your own leaflet.",
                   "principle":"An indicative date reads as a promise the day it is printed."},
                  {"key":"minimal","label":"Commit to nothing beyond statutory consultation — promises are hostages","quality":10,
                   "consequence":"The vacuum fills with the two previous schemes' ghosts; consent processes triple in length for want of a constituency that believes anything.",
                   "principle":"Refusing to promise is also a message, and communities hear it clearly."}]}],
             "hints":["Map every option against the funded horizon — years one to four are the only real money.",
               "The three bottlenecks are multi-modal: notice what that implies for both phasing and package shape.",
               "Communities have been over-promised twice; design the commitment they can verify."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Set a decade-long corridor's phasing, market and public promises in one gateway."}
            """),

        ("WC-CAP-123", "The outage portfolio", "Nine ageing assets, five summers, one grid that must stay on.",
            "Energy Networks", "Asset Renewal Programme Director", "project_management", "expert", 22,
            """["governance","strategy_execution","risk_management"]""",
            """
            {"context":"You direct definition for a transmission asset-renewal portfolio: nine substation renewals over five summers, each needing outages the system operator grants reluctantly. Your gateway position must cover the bundling strategy, the outage philosophy, and how to handle the one asset — the oldest — whose condition data is too poor to plan confidently.",
             "evidence":[
               {"label":"Portfolio","value":"9 substation renewals, 5 summer windows"},
               {"label":"Outages","value":"System operator grants ~2 major windows per summer, hates surprises"},
               {"label":"Bundling options","value":"Per-site contracts · one portfolio partner · regional pairs"},
               {"label":"The ninth asset","value":"Oldest unit; condition data sparse; could be worst or fine"},
               {"label":"Supply chain","value":"Transformer lead times ~2 years and lengthening"}],
             "decisions":[
               {"key":"bundle","prompt":"Stage 1 — bundling strategy?",
                "options":[
                  {"key":"pairs","label":"Regional pairs with common design: four paired packages plus the ninth held separate, long-lead items ordered portfolio-wide NOW","quality":100,
                   "consequence":"Paired sites share designs, spares and crews; the portfolio-wide transformer order beats the lengthening lead times by a full summer.",
                   "principle":"Bundle where repetition pays, separate what is genuinely different, and buy the long-lead risk out first."},
                  {"key":"one","label":"One portfolio partner for all nine — maximum learning-curve capture","quality":30,
                   "consequence":"The learning curve is real; so is the year-three renegotiation, conducted with a partner who knows there is no alternative mobilised.",
                   "principle":"Learning-curve savings quoted at tender have a habit of returning as leverage by mid-portfolio."},
                  {"key":"nine","label":"Nine separate contracts — each site gets a fresh competition","quality":20,
                   "consequence":"Nine mobilisations, nine design variants, and no bidder invests in the tooling that repetition would have justified.",
                   "principle":"Treating a portfolio as nine strangers pays nine times for what repetition sells once."}]},
               {"key":"outage","prompt":"Stage 2 — outage philosophy with the system operator?",
                "options":[
                  {"key":"partnership","label":"A five-year outage masterplan agreed WITH the operator now — windows, fallbacks and a no-surprises protocol — refreshed every winter","quality":100,
                   "consequence":"The operator, treated as a planning partner, starts offering windows the programme didn't know existed; year one banks a spare window it later desperately needs.",
                   "principle":"The scarcest resource in the programme is the outage calendar — govern it jointly with its owner or queue for it like a stranger."},
                  {"key":"annual","label":"Request outages annually, season by season — flexibility beats a rigid masterplan","quality":25,
                   "consequence":"Every spring becomes a negotiation from zero; by year three the portfolio is sequenced around whatever windows were left over.",
                   "principle":"Flexibility without a framework means the other party's calendar wins by default."},
                  {"key":"buffer","label":"Design every renewal for live-adjacent working to minimise outage dependency","quality":35,
                   "consequence":"Outage need drops usefully — at a safety-case and cost premium that two of the nine sites cannot actually support.",
                   "principle":"Engineering around a constraint is worth it selectively, not as a philosophy."}]},
               {"key":"ninth","prompt":"Stage 3 — the data-poor ninth asset?",
                "options":[
                  {"key":"investigate","label":"A funded condition-assessment campaign this year, with the ninth's delivery slot held provisionally in summer four and a trigger plan if the data comes back bad","quality":100,
                   "consequence":"The assessment finds the asset worse than hoped but better than feared; the trigger plan moves it to summer three calmly, inside a portfolio built to flex.",
                   "principle":"For the asset you cannot plan, buy the data first and hold the option open — uncertainty scheduled last is uncertainty compounded."},
                  {"key":"first","label":"Do it first — worst-case assets should be retired soonest","quality":30,
                   "consequence":"The least-understood job becomes the portfolio's opening act; its surprises consume the contingency the other eight were counting on.",
                   "principle":"Leading with your blindest asset teaches the whole portfolio to pay for one site's lessons."},
                  {"key":"last","label":"Do it last — by summer five the team will be at its best","quality":25,
                   "consequence":"Team capability peaks as planned; unfortunately the asset's condition was also compounding, and summer five meets a unit with fewer options left.",
                   "principle":"Deferring the unknown does not preserve the option — the asset keeps ageing while you wait."}]}],
             "hints":["Look for what repetition genuinely buys across nine similar assets — and what it doesn't.",
               "The outage calendar belongs to someone else: design the relationship, not just the requests.",
               "For the ninth asset, price the difference between deciding now and deciding after data."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Shaped a five-summer renewal portfolio around its scarcest resource: the outage calendar."}
            """),

        // ───────────── Stakeholder Dilemma · resources & leadership ─────────────

        ("WC-STK-124", "Two projects, one principal engineer", "Both planning teams built their schedules around the same person. Neither asked her.",
            "Capital Programmes", "Resource & Planning Manager", "project_management", "professional", 6,
            """["resource_management","stakeholder_communication"]""",
            """
            {"context":"Reviewing two definition-stage schedules, you find both the tunnel project and the stations project have planned their critical design phases around the same principal geotechnical engineer — full-time, in the same quarter. Neither project director consulted the other, or her. She reports to a functional head who guards allocation decisions jealously, and both schedules are due at gateways within a month.",
             "evidence":[
               {"label":"Tunnel schedule","value":"Principal engineer: full-time, Q3, critical path"},
               {"label":"Stations schedule","value":"Principal engineer: full-time, Q3, critical path"},
               {"label":"The engineer","value":"Not consulted by either project"},
               {"label":"Her line manager","value":"Functional head, guards allocation authority"},
               {"label":"Gateways","value":"Both schedules due within a month"}],
             "decisions":[
               {"key":"resolve","prompt":"How do you resolve the double-booking?",
                "options":[
                  {"key":"convene","label":"Bring the functional head the facts FIRST and convene both directors with her: real availability, the two critical paths side by side, and options — staggering, a supporting engineer under her review, external support for one scope","quality":100,
                   "consequence":"The functional head, given the allocation decision that is genuinely hers, staggers the phases and assigns a senior under supervision to the second project; both gateways pass with honest resourcing.",
                   "principle":"Resolve resource conflicts through the person who owns the resource — with the facts arranged so the decision is easy to make well."},
                  {"key":"first","label":"First gateway wins: the tunnel schedule submits first, so stations re-plans","quality":20,
                   "consequence":"A scheduling accident becomes an allocation policy; the stations director appeals over your head, and the functional head learns two projects planned her engineer without asking.",
                   "principle":"Sequence of submission is not a resourcing principle, and everyone involved knows it."},
                  {"key":"split","label":"Allocate her half-time to each — fair, and it unblocks both gateways","quality":15,
                   "consequence":"Two critical paths now depend on half a person context-switching weekly; both phases run late in a way neither schedule can explain.",
                   "principle":"Halving a critical resource doubles the meetings and delivers neither path."}]},
               {"key":"systemic","prompt":"To stop the next silent double-booking:",
                "options":[
                  {"key":"register","label":"A named-resource demand register for scarce specialists, checked at every schedule submission — conflicts surface at planning, not at mobilisation","quality":100,
                   "consequence":"The next collision is caught as two lines in a register three months early, and resolved in one email.",
                   "principle":"Scarce people are constraints like outages and cranes — plan them in one visible ledger."},
                  {"key":"policy","label":"A policy memo requiring directors to consult functional heads before naming individuals","quality":30,
                   "consequence":"The memo is agreed with, filed, and unenforced — there is still no mechanism that would surface a violation.",
                   "principle":"A rule without a checkpoint is a wish with a letterhead."}]}],
             "hints":["Identify who actually owns the allocation decision — it is neither project director.",
               "Prepare options before convening: staggering, supervised delegation, external support.",
               "The durable fix is a visible demand ledger for scarce named people, not a politeness rule."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Un-double-booked a principal engineer through the person who actually owned her time."}
            """),
    };
}
