namespace PCI.Backend.Data;

/// <summary>
/// PCI Project Intelligence — Year-1 Q3 authored pack (July onward).
/// July theme: delivery control, change and recovery — the Project Rescue month.
/// Same contract as Q1/Q2: authored TO plan slots, three hints, consequence + principle per
/// option, synthetic data, validator + gates enforced in CI.
/// </summary>
public static partial class WorldIntelligencePack
{
    static readonly (string Code, string Title, string Hook, string Industry, string Role, string Track,
        string Difficulty, int Minutes, string Competencies, string Config)[] ItemsQ3 =
    {
        // ═════════════ JULY — delivery control, change and recovery ═════════════
        // ───────────── Project Rescue · practitioner · multi-stage ─────────────

        ("WC-RSC-201", "Eight weeks down, one story up", "The integration failure is fixed. The programme it derailed is not.",
            "Enterprise Programmes", "Recovery Programme Manager", "project_management", "professional", 15,
            """["recovery_management","change_control","stakeholder_communication"]""",
            """
            {"context":"You are parachuted into a customer-onboarding programme eight weeks late after a failed integration layer stalled three dependent workstreams. The failed component is now fixed. The programme still runs the original plan with 'catch-up' assumed; teams are demoralised; the sponsor has stopped attending steering. Your recovery mandate starts Monday.",
             "evidence":[
               {"label":"Status","value":"8 weeks late; root cause fixed last week"},
               {"label":"Plan","value":"Original baseline still in force, 'catch-up' assumed"},
               {"label":"People","value":"3 workstreams demoralised; overtime already heavy"},
               {"label":"Governance","value":"Sponsor has stopped attending steering"}],
             "decisions":[
               {"key":"first","prompt":"Stage 1 — your first week's focus?",
                "options":[
                  {"key":"replan","label":"Build the honest position: what the 8 weeks actually did to each workstream, what catch-up is physically possible, and a recoverable-vs-lost analysis — before promising anyone anything","quality":100,
                   "consequence":"The analysis shows 3 of 8 weeks recoverable at reasonable cost; every later decision stands on that floor instead of on the word 'catch-up'.",
                   "principle":"Recovery starts with an honest map of the hole — plans made before the measurement are morale, not management."},
                  {"key":"drive","label":"Drive the teams hard immediately — momentum and visible urgency first, analysis alongside","quality":15,
                   "consequence":"Two weeks of urgency theatre on the old plan burns the remaining goodwill; the honest position, when it finally arrives, now also has to explain the wasted fortnight.",
                   "principle":"Urgency without a validated target is how late programmes get later and angrier."},
                  {"key":"restructure","label":"Reorganise the workstreams first — the structure failed, so fix the structure","quality":25,
                   "consequence":"A reorg during recovery adds its own six-week disruption to the eight you inherited; the structure was a symptom.",
                   "principle":"Never run a reorganisation and a recovery simultaneously unless the structure caused the failure."}]},
               {"key":"promise","prompt":"Stage 2 — what does the new commitment look like?",
                "options":[
                  {"key":"floor","label":"Re-baseline at the analysis's recoverable position — 5 weeks late — with the 3-week recovery funded and shown as stretch, and the change governed through the board","quality":100,
                   "consequence":"The programme commits to a date it can hit and beats it by a week; the stretch mechanism recovers what the arithmetic said it could.",
                   "principle":"Commit at the floor, chase the stretch — a recovery that promises the ceiling relapses in public."},
                  {"key":"hold","label":"Hold the original date — re-baselining rewards failure and the client contract points there","quality":10,
                   "consequence":"The impossible date stands for six more weeks of missed reports, then moves anyway — having converted a planning problem into a credibility one.",
                   "principle":"A date the arithmetic has disproven is not a target; it is a scheduled disappointment."},
                  {"key":"pad","label":"Re-baseline with generous padding — never miss twice","quality":30,
                   "consequence":"The padded date holds trivially; the client's commercial team, comparing effort to calendar, quietly re-prices the relationship's trust either way.",
                   "principle":"Over-padding after a failure trades schedule credibility for schedule safety — you need both."}]},
               {"key":"sponsor","prompt":"Stage 3 — the absent sponsor?",
                "options":[
                  {"key":"reengage","label":"A direct conversation: what the recovery needs from them specifically (two decisions, one client call), meeting cadence halved but attendance non-negotiable — their absence IS a programme risk, said plainly","quality":100,
                   "consequence":"The sponsor — avoiding the programme because it only brought bad news — returns for a role that is concrete and finite; their client call unlocks the third workstream's dependency.",
                   "principle":"Sponsors abandon programmes that only export problems — re-engage them with a defined, winnable role."},
                  {"key":"around","label":"Work around them — escalate needed decisions to the board directly","quality":20,
                   "consequence":"The board notices it is doing the sponsor's job; the eventual conversation about that happens above your head and around your recovery.",
                   "principle":"Routing around a broken governance role institutionalises the breakage."},
                  {"key":"replace","label":"Ask the board to appoint a new sponsor","quality":30,
                   "consequence":"Sometimes right — but as a first move it spends a quarter on succession politics the recovery timeline doesn't have; try re-engagement first.",
                   "principle":"Replace a sponsor after re-engagement fails, not instead of attempting it."}]}],
             "hints":["Measure the hole before promising the climb out of it.",
               "Commit where the arithmetic is safe; chase the rest as funded stretch.",
               "Diagnose WHY the sponsor left — the answer is usually the programme's own reporting."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Rebuilt an eight-weeks-late programme on an honest floor and a funded stretch."}
            """),

        ("WC-RSC-202", "The wall that moved", "The retaining wall is out of tolerance. The apartments above it are sold.",
            "Construction", "Project Recovery Lead", "project_management", "professional", 13,
            """["recovery_management","change_control"]""",
            """
            {"context":"Survey confirms a residential development's retaining wall has deflected past its design tolerance — construction sequence error, not design. The remedial options range from ground anchors (8 weeks, works within footprint) to partial rebuild (14 weeks, tower crane back). Fifty apartments above are pre-sold with contractual long-stop dates ~20 weeks out; the client is talking about 'whose insurance pays' before the fix is even chosen.",
             "evidence":[
               {"label":"Defect","value":"Wall deflection past tolerance — sequencing error in construction"},
               {"label":"Options","value":"Ground anchors: 8wks · Partial rebuild: 14wks"},
               {"label":"Commercial","value":"50 pre-sales, long-stops ~20 weeks out"},
               {"label":"Client","value":"Already litigating 'whose insurance' in meetings"}],
             "decisions":[
               {"key":"fix","prompt":"Stage 1 — choosing the remedial scheme?",
                "options":[
                  {"key":"engineering","label":"Let the designer certify which schemes restore FULL design life — then choose among certified options on programme and cost; anchors only if the designer signs them as permanent works, not because 8 beats 14","quality":100,
                   "consequence":"The designer certifies anchors with a monitoring condition; the 8-week option proceeds as a certified fix rather than a convenient one — a distinction the building's insurers spend a day confirming.",
                   "principle":"On structural remediation, the schedule chooses among certified options — it never certifies them."},
                  {"key":"fastest","label":"Anchors, decided today — every week matters against the long-stops","quality":20,
                   "consequence":"The designer, consulted after the decision, adds conditions that eat three of the six weeks saved; sequence matters even in a hurry.",
                   "principle":"A fix chosen before certification is a proposal wearing a schedule."},
                  {"key":"strongest","label":"Partial rebuild — never anchor what you can rebuild; remove all doubt","quality":30,
                   "consequence":"Maximum certainty, six extra weeks against 20-week long-stops: doubt is removed from the wall and installed in fifty purchase contracts.",
                   "principle":"Over-fixing one risk while a bigger one matures beside it is not caution."}]},
               {"key":"commercial","prompt":"Stage 2 — the 'whose insurance' conversation?",
                "options":[
                  {"key":"sequence","label":"Park liability by agreement: a joint without-prejudice protocol — fix first under reserved rights, evidence preserved jointly, quantum after — so the remediation never waits for the lawyers","quality":100,
                   "consequence":"Both sides' insurers accept the protocol (it protects their evidence too); the anchors start on schedule while liability resolves in parallel over months nobody is counting.",
                   "principle":"Fix-first-fight-later is a negotiable structure — propose it before the fight delays the fix."},
                  {"key":"fight","label":"Establish liability first — whoever pays should choose and control the fix","quality":5,
                   "consequence":"Six weeks of correspondence later nothing has been anchored; the long-stops arrive mid-argument and the purchasers' lawyers join the thread.",
                   "principle":"Liability sequencing that delays remediation grows the quantum it is arguing about."},
                  {"key":"absorb","label":"Fund the fix silently, claim later — possession is momentum","quality":35,
                   "consequence":"The fix proceeds; the later claim, made without a reserved-rights record, meets 'you fixed it voluntarily' as its first defence.",
                   "principle":"Pay under protest, in writing — generosity and waiver look identical in hindsight."}]}],
             "hints":["Certification first, then schedule — the order is the discipline.",
               "The long-stop dates are the real clock; audit every choice against them.",
               "Fix-first protocols exist precisely for this — propose the structure early."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Anchored a failing wall behind a certified fix and a fight-later protocol."}
            """),

        ("WC-RSC-203", "Sixty percent adopted, one hundred percent spent", "The ERP is live. Whether anyone uses it is another matter.",
            "Enterprise Programmes", "Adoption Recovery Lead", "project_management", "professional", 18,
            """["recovery_management","stakeholder_communication","benefits_management"]""",
            """
            {"context":"Nine months after go-live, the ERP's adoption sits at 60%: finance and procurement live in it; operations runs shadow spreadsheets; field teams re-key data through admin staff. The benefits case assumed 90% by month six. The budget is spent, the vendor's hypercare has ended, and leadership debates 'relaunch' versus 'mandate compliance'. You are asked for the recovery strategy.",
             "evidence":[
               {"label":"Adoption","value":"60% overall — finance/procurement high, operations/field low"},
               {"label":"Assumed","value":"90% by month 6; benefits model built on it"},
               {"label":"Shadow work","value":"Spreadsheets + re-keying via admin staff"},
               {"label":"Budget","value":"Spent; hypercare ended"}],
             "decisions":[
               {"key":"diagnose","prompt":"Stage 1 — what does the 60% actually mean?",
                "options":[
                  {"key":"segment","label":"Diagnose by segment before choosing anything: WHY does operations shadow-work? Ride along with field teams for a week — the answer (workflow mismatch? mobile UX? training? habit?) determines everything downstream","quality":100,
                   "consequence":"The ride-alongs find the field workflow takes 11 screens where the paper form took one page — this is a product-fit problem wearing an adoption costume, and mandates would have criminalised the evidence.",
                   "principle":"Non-adoption is data about the system, not about the users — read it before overriding it."},
                  {"key":"mandate","label":"Mandate compliance — the organisation bought a system, using it is not optional","quality":10,
                   "consequence":"Shadow spreadsheets go underground rather than away; data quality craters as resentful compliance games the mandatory fields, and the benefits case dies with clean-looking numbers.",
                   "principle":"Mandating the use of a system that doesn't fit the work converts non-adoption into bad data."},
                  {"key":"relaunch","label":"Relaunch: new training wave, new comms, executive sponsorship refresh","quality":25,
                   "consequence":"The relaunch re-trains people in a workflow that still takes 11 screens; month three post-relaunch looks like month nine pre-relaunch, minus the relaunch budget.",
                   "principle":"Repeating the rollout louder assumes the rollout was the problem — check first."}]},
               {"key":"fix","prompt":"Stage 2 — the recovery investment (unbudgeted)?",
                "options":[
                  {"key":"targeted","label":"A minimal, evidence-targeted case: fix the field workflow (config + mobile forms, not re-implementation), funded by quantifying what the shadow-work costs in admin re-keying — the recovery pays for itself in the business case","quality":100,
                   "consequence":"The re-keying audit shows 11 FTE-equivalents of hidden cost; the workflow fix costs a fraction of that and adoption follows the friction out of the system.",
                   "principle":"Fund recovery from the measured cost of the failure — the money is already being spent, just invisibly."},
                  {"key":"nothing","label":"No new money — optimise within current config and accept the plateau","quality":15,
                   "consequence":"The plateau holds; the benefits case quietly writes off a third of its value, and the 11 hidden FTEs continue re-keying forever, off every business case.",
                   "principle":"'No budget' usually means the cost has moved somewhere unmeasured, not that it stopped."},
                  {"key":"big","label":"A full phase-2 programme — do it properly this time","quality":20,
                   "consequence":"An organisation nine months into disappointment funds 'properly this time' reluctantly and cancels it at the first slip; the targeted fix inside it was the only part that mattered.",
                   "principle":"After a credibility loss, the smallest self-funding fix beats the grandest plan."}]},
               {"key":"benefits","prompt":"Stage 3 — the benefits case that assumed 90%?",
                "options":[
                  {"key":"restate","label":"Restate it honestly: current-trajectory benefits, fixed-workflow benefits, and the write-off if nothing changes — the three futures priced, board chooses knowingly","quality":100,
                   "consequence":"The board funds the workflow fix off the comparison and — for the first time in the programme's history — trusts its benefits numbers.",
                   "principle":"A benefits case is only useful while it describes a future someone still believes."},
                  {"key":"hold","label":"Keep reporting against the original case — restating admits failure","quality":5,
                   "consequence":"Each report's gap-to-case grows until an internal audit does the restating for you, with commentary.",
                   "principle":"The original case admits failure either way; restating chooses who narrates it."},
                  {"key":"drop","label":"Stop benefits reporting — the system is live, move on","quality":10,
                   "consequence":"The one instrument that could fund the recovery is switched off; the shadow-work cost stays invisible forever.",
                   "principle":"Benefit tracking is most valuable exactly when the news is bad."}]}],
             "hints":["Go watch the non-adopters work before choosing any strategy.",
               "Find the hidden cost of the workaround — it usually funds the fix.",
               "Restate the benefits case as three priced futures, not one embarrassing gap."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Diagnosed a 60% adoption plateau as an 11-screen workflow, and funded the fix from its own waste."}
            """),

        ("WC-RSC-204", "Third time, or never", "Two failed migrations. One remaining maintenance window this year.",
            "Technology Programmes", "Migration Recovery Director", "project_management", "professional", 16,
            """["recovery_management","risk_management","commercial_management"]""",
            """
            {"context":"The core-banking data migration has failed twice: attempt one on reconciliation mismatches (2.3M records), attempt two — after fixes — on cutover-window overrun (the reconciliation ran 9 hours over). The vendor blames data quality; your team blames the vendor's tooling throughput. One migration window remains this year (a 4-day holiday weekend, 11 weeks out); missing it pushes the programme into next year's regulatory freeze.",
             "evidence":[
               {"label":"Attempt 1","value":"Failed: 2.3M reconciliation mismatches"},
               {"label":"Attempt 2","value":"Failed: reconciliation ran 9 hours past the window"},
               {"label":"Blame","value":"Vendor: data quality · Your team: vendor tooling throughput"},
               {"label":"Window","value":"One left: 4-day weekend, 11 weeks out, then regulatory freeze"}],
             "decisions":[
               {"key":"truth","prompt":"Stage 1 — resolving the blame deadlock?",
                "options":[
                  {"key":"instrument","label":"Commission a joint instrumented dry-run in week 2: full-volume rehearsal on production-copy data, both parties' engineers present, throughput and mismatch causes MEASURED — the deadlock dies by instrumentation, not negotiation","quality":100,
                   "consequence":"The dry-run shows both were right: 400k genuine data-quality mismatches AND tooling throughput 30% below the vendor's claims — two fixable problems that blame had kept bundled as one unfixable argument.",
                   "principle":"When two parties hold half the truth each, measurement is the only mediator that scales."},
                  {"key":"vendor","label":"Formal vendor escalation — their tooling, their fix, their contract exposure","quality":15,
                   "consequence":"The vendor lawyers up and slows down; the 11 weeks shrink while the correspondence grows, and the data-quality half of the problem — yours — stays untouched.",
                   "principle":"Escalating half a problem delays the whole one."},
                  {"key":"replace","label":"Replace the vendor's migration tooling with an alternative","quality":10,
                   "consequence":"New tooling, new learning curve, same 400k dirty records — attempt three now carries attempt-one's risks plus a novelty premium, inside a fixed window.",
                   "principle":"Swapping tools eleven weeks before the last window trades known problems for unknown ones."}]},
               {"key":"design","prompt":"Stage 2 — designing attempt three?",
                "options":[
                  {"key":"derisk","label":"Change the shape, not just the parts: pre-migrate the static 80% the weekend BEFORE (reversible, verified at leisure), leaving only the active delta for the 4-day window — with rehearsed go/no-go gates at hour 24 and 48 and a tested rollback","quality":100,
                   "consequence":"The window's workload drops by three-quarters; hour-24's gate passes with 11 hours' slack, and the rollback plan retires unused — the third attempt succeeds mostly because it needed less luck.",
                   "principle":"After two failures at full scope, shrink the scope the window must carry — de-risking the shape beats perfecting the execution."},
                  {"key":"harder","label":"Same design, fixed defects, more rehearsal — the plan was sound, the execution wasn't","quality":20,
                   "consequence":"Better execution of a shape that has failed twice: the third run finishes 40 minutes inside the window, which is a definition of success only gamblers use.",
                   "principle":"A plan that needs everything to go right has already told you twice what it thinks of that requirement."},
                  {"key":"split","label":"Abandon big-bang: migrate by customer segment over six monthly windows","quality":35,
                   "consequence":"Genuinely lower risk per event — but six windows don't exist before the freeze, and dual-running two banking cores for a year has its own regulator conversation.",
                   "principle":"Incremental migration is the right answer to a different constraint set — check the calendar before adopting it."}]},
               {"key":"stake","prompt":"Stage 3 — what do you tell the board about the freeze risk?",
                "options":[
                  {"key":"contingent","label":"The honest tree: attempt three's design, its measured probability after the dry-run, and the pre-built freeze-year contingency (extended dual-running costs, regulator pre-engagement) IF it fails — decision made once, in advance","quality":100,
                   "consequence":"The board approves attempt three knowing the fallback's price; when it succeeds, the unused contingency plan still earned its keep as the reason nobody panicked at hour 30.",
                   "principle":"Take the board the whole tree — a plan whose failure mode is 'we'll see' is half a plan."},
                  {"key":"confident","label":"Project confidence — boards fund conviction, and doubt is contagious","quality":10,
                   "consequence":"Conviction is funded; the freeze scenario, unplanned, is priced in real time at 2am by whoever answers the phone.",
                   "principle":"Confidence without a contingency is a loan against the outcome."},
                  {"key":"hedge","label":"Recommend deferring to next year now — the freeze is survivable and attempt three is a coin flip","quality":25,
                   "consequence":"The measured dry-run said better than a coin flip; deferring by assumption donates a year to a risk the instrumentation had already shrunk.",
                   "principle":"Don't price the risk by its history when you've just changed its causes — price the new design."}]}],
             "hints":["Deadlocked blame yields to joint instrumentation — measure, don't mediate.",
               "Shrink what the window must carry; the best third attempt needs the least luck.",
               "Bring the board the failure branch pre-priced — contingency is cheapest before it's needed."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Designed a third migration attempt that needed less luck instead of more discipline."}
            """),

        ("WC-RSC-205", "The administrators answer the phone now", "Your facade contractor went insolvent at 70% complete. The scaffolding is theirs too.",
            "Enterprise Programmes", "Contract Recovery Manager", "project_management", "professional", 14,
            """["recovery_management","commercial_management"]""",
            """
            {"context":"Your office-tower facade subcontractor entered administration overnight at 70% complete. On site: their scaffold wraps the building (hired in THEIR name), 400 facade panels worth 900k sit in their yard (you've paid milestones covering ~600k of them), and their site team — who know every bracket — are this morning unemployed. The administrators' first email offers 'discussions regarding completion options'.",
             "evidence":[
               {"label":"Progress","value":"Facade 70% complete"},
               {"label":"Scaffold","value":"Wraps the building; hired in the insolvent company's name"},
               {"label":"Materials","value":"400 panels (~900k) in their yard; ~600k milestone-paid"},
               {"label":"People","value":"Their site team unemployed as of this morning"}],
             "decisions":[
               {"key":"triage","prompt":"Stage 1 — the first 72 hours?",
                "options":[
                  {"key":"secure","label":"Secure the physical position in order of evaporation risk: novate the scaffold hire to your name TODAY (before the hirer strips it), assert your vesting/title claim on the paid panels in writing to the administrators with the certificates attached, and get employment conversations open with the key site supervisors this week","quality":100,
                   "consequence":"The scaffold hire transfers before the hire company's recovery crew is dispatched; the vesting certificates hold 380 of 400 panels; two supervisors join your direct payroll carrying the bracket schedule in their heads.",
                   "principle":"In a counterparty insolvency, physical possession and people evaporate faster than legal rights — sequence the first 72 hours by what disappears first."},
                  {"key":"legal","label":"Instruct lawyers to establish your full contractual position before acting on anything","quality":15,
                   "consequence":"The advice arrives in six days, excellent and moot: the scaffold came down on day four (hirer's recovery), and the best supervisor is on a competitor's site.",
                   "principle":"Legal certainty that arrives after the assets leave is a well-documented loss."},
                  {"key":"deal","label":"Engage the administrators' completion-options offer first — continuity beats improvisation","quality":25,
                   "consequence":"The administrators' job is maximising THEIR estate — the discussions price your urgency by the day while the scaffold clock runs; talk, but never as the first or only move.",
                   "principle":"An administrator is a counterparty with opposite incentives and better information about the clock."}]},
               {"key":"complete","prompt":"Stage 2 — the completion route for the remaining 30%?",
                "options":[
                  {"key":"hybrid","label":"Direct-engage the known quantity: hire the supervisors, buy the panels from the estate, and contract a facade contractor for labour-and-plant completion under YOUR materials and supervision — competition where it exists, continuity where it can't","quality":100,
                   "consequence":"The completion prices 15% over the original rate — painful, not catastrophic — because the panels, the knowledge and the scaffold never left; the tower drylines on a five-week delay instead of a five-month one.",
                   "principle":"Completion after insolvency is assembled from parts — keep what only the failed contractor had, compete what the market can price."},
                  {"key":"retender","label":"Full retender of the remaining works — clean contract, clean liability","quality":20,
                   "consequence":"Clean and slow: bidders price 30% completion risk premium on someone else's work, the retender consumes nine weeks, and the winner's first RFI asks for the bracket schedule that left with the supervisors.",
                   "principle":"A clean-sheet retender pays the market's price for everything the incumbent knew."},
                  {"key":"estate","label":"Fund the administrators to complete using the existing team 'in administration'","quality":30,
                   "consequence":"Sometimes viable for weeks of work; for months of it, you are funding an entity with no warranty future and staff interviewing elsewhere daily.",
                   "principle":"Trading completion speed for a counterparty that legally cannot stand behind the work is a short-term tool only."}]}],
             "hints":["Rank the first-72-hours actions by what evaporates fastest: hired plant, people, then paperwork.",
               "Vesting certificates and milestone payment records are your title evidence — deploy them in writing, immediately.",
               "The completion route can be assembled: people direct, materials from the estate, labour competed."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Cost Guardian"},
             "share_line":"Held the scaffold, the panels and the people through a subcontractor's overnight insolvency."}
            """),

        ("WC-RSC-206", "The fault that ate the outage", "The cable failed on test. The window to fix it closes faster than the repair.",
            "Energy Networks", "Outage Recovery Manager", "project_management", "professional", 15,
            """["recovery_management","schedule_analysis","stakeholder_communication"]""",
            """
            {"context":"Day 5 of an 8-day circuit outage to install a new 132kV cable: the commissioning pressure test has failed — a joint defect, location identified, repair estimate 4 days. The arithmetic is brutal: 4 days of repair into 3 days of window. The system operator's next available outage is in 9 weeks (winter constraints); energisation this window was contractually promised to a connecting data-centre customer.",
             "evidence":[
               {"label":"Failure","value":"Joint defect on pressure test; located; repair ≈ 4 days"},
               {"label":"Window","value":"3 days remain of 8"},
               {"label":"Next outage","value":"9 weeks (winter constraints)"},
               {"label":"Customer","value":"Data centre energisation contractually promised this window"}],
             "decisions":[
               {"key":"technical","prompt":"Stage 1 — the technical response to 4-into-3?",
                "options":[
                  {"key":"compress_real","label":"Interrogate the 4-day estimate with the jointing contractor TODAY: what does a 24-hour shift pattern, pre-staged joint kit and parallel prep actually yield? If the honest compressed answer is ≤3 days with margin, go; if not, stop spending the window on hope","quality":100,
                   "consequence":"The jointers' honest answer: 3 days with double shifts IF the replacement joint kit arrives by tomorrow — it can, by dedicated courier; the repair completes with 7 hours of window left, tested and energised.",
                   "principle":"Compress an estimate WITH the people who own it — a real 3-day answer exists or it doesn't, and either answer beats an assumed one."},
                  {"key":"push","label":"Order the repair and drive it — windows have been stretched before and the operator can be pressured at day 8","quality":10,
                   "consequence":"Day 8 arrives mid-jointing; the operator — legally obliged to restore the network — takes the outage back with the cable open, and week 10's restart adds re-testing to the repair.",
                   "principle":"Planning to overrun someone else's statutory window is not a schedule strategy; it is a hostage-taking that fails."},
                  {"key":"abandon","label":"Stand down now, preserve the work done, take the 9 weeks","quality":30,
                   "consequence":"Orderly and possibly premature: nobody asked the jointers the compression question, and the 9-week delay's cost — customer damages included — dwarfs one day of intense interrogation.",
                   "principle":"Retreat is a decision that deserves the same rigour as attack."}]},
               {"key":"operator","prompt":"Stage 2 — the system operator conversation?",
                "options":[
                  {"key":"early_options","label":"Call them TODAY with the situation and two structured asks: any restoration-safe extension of hours (even 12 helps the margin), and a provisional booking of the earliest fallback SLOT now — so failure at day 8 costs 9 weeks, not 9 weeks plus a queue","quality":100,
                   "consequence":"The operator finds 18 hours of restoration-safe flex (a demand forecast revision) and pencils the fallback; the transparency deposits credibility the programme draws on for years.",
                   "principle":"Tell the outage's owner the truth early and bring structured asks — operators reward candour with flexibility and punish surprises with process."},
                  {"key":"quiet","label":"Say nothing unless the repair slips — no need to alarm them prematurely","quality":5,
                   "consequence":"Day 7's 'surprise' request meets the operator's emergency process at its least flexible; the pencilled fallback slot that candour would have secured is gone.",
                   "principle":"With statutory counterparties, early bad news is a request; late bad news is an incident."},
                  {"key":"escalate","label":"Escalate immediately to regulatory/political level for a window extension","quality":15,
                   "consequence":"The heavyweight lever, pulled first, wins 24 hours and a permanent entry in the operator's institutional memory under 'goes over our heads'.",
                   "principle":"Escalation above the working level is a card you play once — never as the opening."}]},
               {"key":"customer","prompt":"Stage 3 — the contractually-promised data centre?",
                "options":[
                  {"key":"parallel_truth","label":"Brief them in parallel with the recovery, not after it: the defect, the compressed plan's honest odds, and jointly-developed bridging options (temporary supply arrangement) if day 8 fails — their contingency planning starts NOW either way","quality":100,
                   "consequence":"The customer activates their bridging option as a precaution and stands it down unused; the relationship survives on the strength of having been treated as a partner during the worst week.",
                   "principle":"A customer told at day 5 has options; one told at day 8 has lawyers."},
                  {"key":"after","label":"Tell them the outcome when there is one — briefing mid-crisis just spreads the panic","quality":10,
                   "consequence":"Success would have hidden the gamble; the alternative — learning at day 8 that their energisation moved 9 weeks with zero warning — converts a schedule failure into a bad-faith claim.",
                   "principle":"Withholding known risk from the party who bears it is a choice courts get to characterise later."},
                  {"key":"concede","label":"Concede the delay now and negotiate compensation before knowing if it's needed","quality":20,
                   "consequence":"The compensation conversation, opened before the repair verdict, prices a failure that then doesn't happen — and cannot be unpriced.",
                   "principle":"Never pay for an outcome the next 72 hours might delete."}]}],
             "hints":["Compression questions belong to the people holding the tools — ask them before deciding anything.",
               "Book the fallback while fighting for the primary; the two moves are allies.",
               "Everyone who bears the risk plans better the earlier they hear — operator and customer alike."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Executive Communicator"},
             "share_line":"Compressed a four-day repair into a three-day window by asking the jointers, not the calendar."}
            """),

        ("WC-RSC-207", "Two metres of water, twelve weeks of plan", "The flood was insured. The recovery sequence is up to you.",
            "Construction", "Site Recovery Manager", "project_management", "professional", 13,
            """["recovery_management","schedule_analysis"]""",
            """
            {"context":"A river burst its banks and put two metres of water through your half-complete leisure-centre site for three days. The water is gone; the damage assessment is in: pool-hall slab undermined (8 weeks to remediate), M&E plant delivered-but-not-installed written off (14-week reorder leads), all site records survived (cloud), the frame is certified sound. The insurer has accepted the claim in principle. Sequence the recovery.",
             "evidence":[
               {"label":"Slab","value":"Undermined — 8 weeks remediation, gates all pool-hall trades"},
               {"label":"M&E plant","value":"Written off — 14-week reorder lead times"},
               {"label":"Frame","value":"Certified sound; dry-side works CAN continue"},
               {"label":"Insurance","value":"Accepted in principle; quantum process ~12 weeks"}],
             "decisions":[
               {"key":"sequence","prompt":"Stage 1 — the recovery sequence?",
                "options":[
                  {"key":"critical_first","label":"Sequence by lead time and gating, not by visibility: M&E reorders placed THIS WEEK (14-week lead is the true critical path), slab remediation mobilised second (8 weeks, gates the hall), dry-side works continue throughout — and the insurer's quantum process runs in parallel, never as a predecessor","quality":100,
                   "consequence":"The reorders land two days before the remediated slab can receive them — the recovery's critical path was the purchase orders, and someone noticed in week one instead of week nine.",
                   "principle":"Recovery sequencing is ordinary critical-path logic applied under emotion — the longest lead gates, however undramatic it looks."},
                  {"key":"visible","label":"Slab first and visibly — the site needs to SEE recovery happening to believe in it","quality":25,
                   "consequence":"Morale rises with the new concrete; week 16 finds a perfect slab waiting six more weeks for air-handling units nobody reordered until the slab was done.",
                   "principle":"Sequencing for morale delivers morale now and a gap later — communicate for morale, sequence for leads."},
                  {"key":"insurer_first","label":"Wait for the insurer's quantum agreement before committing recovery spend","quality":10,
                   "consequence":"Twelve weeks of nothing while the claim processes; the 14-week reorders start in week 13, and the insurer's own loss adjuster asks why mitigation didn't start sooner — mitigation being YOUR duty under the policy.",
                   "principle":"'Accepted in principle' plus the policy's mitigation duty means spend-and-document, not wait-and-see."}]},
               {"key":"claim","prompt":"Stage 2 — running the insurance interface?",
                "options":[
                  {"key":"protocol","label":"Agree the working protocol with the loss adjuster in week one: evidence standards, approval thresholds for recovery spend, betterment treatment for the replacement plant — so every recovery decision is pre-cleared in category, not negotiated item by item","quality":100,
                   "consequence":"Recovery spend flows against the agreed protocol; the one genuine betterment argument (newer AHU models — the old ones are discontinued) is settled by the framework in a week instead of stalling the reorder.",
                   "principle":"Turn the adjuster from an auditor of past decisions into a signatory of the decision framework — the claim then travels WITH the recovery."},
                  {"key":"maximal","label":"Claim aggressively — include every arguable item and negotiate down from strength","quality":15,
                   "consequence":"The padded claim triggers the insurer's forensic review; ALL payments — including the undisputed core — slow to the pace of the weakest line item.",
                   "principle":"A claim's weakest item sets the speed of its strongest."},
                  {"key":"minimal","label":"Keep the claim conservative and simple to keep the money moving","quality":40,
                   "consequence":"The money moves; the legitimately claimable acceleration costs and extended prelims — real, evidenced, unclaimed — fund the insurer's Christmas party instead of your recovery.",
                   "principle":"Conservative claiming is a virtue about honesty, not a strategy about completeness — claim everything evidenced, nothing padded."}]}],
             "hints":["Find the longest lead time in the damage list — that is the recovery's real front.",
               "The policy's mitigation duty means early spend is protected — document and proceed.",
               "Frameworks beat line-item fights with adjusters; agree the rules once."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced a flood recovery by lead times and let the insurance run beside it, not ahead of it."}
            """),

        ("WC-RSC-208", "All red at once", "Fourteen projects, one bad quarter, and a portfolio review that changes everything or nothing.",
            "Portfolio & PMO", "Portfolio Recovery Director", "project_management", "professional", 18,
            """["recovery_management","governance","prioritization"]""",
            """
            {"context":"The infrastructure portfolio's quarterly review lands with nine of fourteen projects reporting red or amber-red — a step change from four last quarter. Causes cited: a common design-resource shortage, two supplier failures, inflation eating contingencies, and 'optimism correction' after a new PMO reporting standard. The board wants a recovery plan for everything; the delivery directors each want protection for their own. You hold the pen on the portfolio response.",
             "evidence":[
               {"label":"Position","value":"9 of 14 red/amber-red, up from 4"},
               {"label":"Cited causes","value":"Design-resource shortage · 2 supplier failures · inflation vs contingency · new honest reporting"},
               {"label":"Board","value":"Wants 'a recovery plan for everything'"},
               {"label":"Directors","value":"Each lobbying to protect their own"}],
             "decisions":[
               {"key":"read","prompt":"Stage 1 — reading the step change?",
                "options":[
                  {"key":"decompose","label":"Decompose before responding: how much of the red is the REPORTING correction (was always true, now visible), how much is the common resource constraint (one cause, nine symptoms), how much is genuinely project-specific? Three different problems need three different instruments","quality":100,
                   "consequence":"The decomposition shows: 3 projects were always red (now honest), 4 share the design-resource constraint, 2 have real singular crises — and 'nine reds' collapses into three tractable problems instead of one intractable panic.",
                   "principle":"A portfolio step-change is a mixture — decompose it into common-cause, revealed-truth and genuine-crisis before prescribing anything."},
                  {"key":"all","label":"Recovery plans for all nine as the board asked — comprehensive beats clever","quality":15,
                   "consequence":"Nine recovery plans compete for the same design resource that caused four of them; the plans are internally coherent and collectively impossible.",
                   "principle":"Parallel recoveries that share a constraint are one recovery wearing nine hats."},
                  {"key":"worst","label":"Triage to the three biggest reds and stabilise those first","quality":30,
                   "consequence":"Reasonable instinct, wrong cut: two of the 'biggest' were the honest-reporting reds (stable, just visible now), while the common resource constraint quietly reddens two more projects during the triage.",
                   "principle":"Size of red is not urgency of cause — triage by causal structure, not by dashboard colour."}]},
               {"key":"constraint","prompt":"Stage 2 — the shared design-resource constraint?",
                "options":[
                  {"key":"portfolio_level","label":"Solve it ONCE at portfolio level: a single prioritised design-resource schedule across the four affected projects (sequenced by consequence-of-delay), supplemented by a framework design partner onboarded for the overflow — individual project 'recoveries' for this cause are suspended as noise","quality":100,
                   "consequence":"The four projects' competing escalations become one queue with one owner; the framework partner absorbs the overflow within six weeks, and three of the four reds turn amber by the next review.",
                   "principle":"A common-cause constraint is portfolio property — solving it four times separately is how it never gets solved."},
                  {"key":"market","label":"Let the four projects compete for the resource — internal markets allocate better than committees","quality":10,
                   "consequence":"The competition is won by the loudest director, not the most consequential project; the queue reorders itself weekly by escalation stamina.",
                   "principle":"Internal markets for a scarce shared resource allocate by politics and call it price discovery."},
                  {"key":"hire","label":"Recruit permanent design capacity — solve the shortage, not the allocation","quality":25,
                   "consequence":"The requisitions are approved and the market delivers senior designers in five months; the projects needed the constraint managed in five weeks.",
                   "principle":"Hiring fixes next year's constraint; allocation fixes this quarter's."}]},
               {"key":"board","prompt":"Stage 3 — the board narrative?",
                "options":[
                  {"key":"structured","label":"The decomposed truth: which reds are honest reporting (a control improvement, claimed as one), which share the constraint (one plan, portfolio-owned), which are genuine crises (two focused recoveries) — with the next review's expected shape forecast","quality":100,
                   "consequence":"The board hears order instead of alarm, funds the framework partner in one sitting — and when the next review lands as forecast, the PMO's credibility compounds.",
                   "principle":"Boards can absorb any amount of bad news that arrives structured, owned and forecast — what they cannot absorb is a wall of undifferentiated red."},
                  {"key":"soften","label":"Present six reds as 'improving' to avoid overwhelming the board","quality":5,
                   "consequence":"The optimism correction that caused half the reds is quietly reversed by the correction's own author; the next quarter's step-change is bigger and less forgivable.",
                   "principle":"Re-softening reports that just became honest is the one move that burns both directions."},
                  {"key":"crisis","label":"Declare a portfolio crisis and request emergency governance powers","quality":20,
                   "consequence":"The powers are granted and the word 'crisis' escapes the boardroom; two funders open urgent reviews, and the label costs more than the latitude bought.",
                   "principle":"Crisis declarations are irreversible speech acts — spend them on crises, not on bad quarters."}]}],
             "hints":["Decompose the reds by cause before counting them.",
               "One shared constraint masquerades as many project problems — solve it where it lives.",
               "Structure is what makes bad news fundable — arrive with the mixture already separated."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Turned nine simultaneous reds into three tractable problems and one funded fix."}
            """),

        ("WC-RSC-209", "Sixty percent of nameplate", "The line runs. It just doesn't run like the business case said.",
            "Industrial Manufacturing", "Ramp-Up Recovery Manager", "project_management", "professional", 16,
            """["recovery_management","commercial_management","evidence_analysis"]""",
            """
            {"context":"Twelve weeks after handover, the new bottling line runs at 60% of nameplate throughput. The integrator's warranty covers 'demonstrated capability at acceptance' — and acceptance was passed at nameplate during a 4-hour test with the integrator's own engineers driving. Since then: three shifts of your operators, real product variety (the test ran one SKU), and micro-stoppage patterns your team logs but can't diagnose. The integrator's position: 'the line demonstrated capability; operations is an operator matter'.",
             "evidence":[
               {"label":"Performance","value":"60% of nameplate, plateaued for 4 weeks"},
               {"label":"Acceptance","value":"Passed at nameplate — 4-hour test, single SKU, integrator's engineers driving"},
               {"label":"Reality","value":"3 shifts, full SKU mix, undiagnosed micro-stoppages"},
               {"label":"Integrator","value":"'Capability demonstrated; operations is your matter'"}],
             "decisions":[
               {"key":"diagnose","prompt":"Stage 1 — cracking the 60% plateau?",
                "options":[
                  {"key":"data","label":"Instrument before arguing: two weeks of stoppage data cut by SKU, shift, station and fault code — the pattern (which products, which stations, which crews) decides whether this is a machine problem, a changeover problem or a training problem, and each has a different owner","quality":100,
                   "consequence":"The cut shows 70% of losses at two stations, on three narrow-neck SKUs, on ALL shifts equally — machine-configuration territory, not operator territory, and now provable as such.",
                   "principle":"A throughput plateau is an attribution question — instrument it before either side's theory spends money."},
                  {"key":"blame_machine","label":"Open the warranty claim now — the line doesn't perform, the integrator must fix it","quality":15,
                   "consequence":"The integrator replays the acceptance certificate; without the SKU-cut data the claim is 'your operators' versus 'your machine' at lawyer rates.",
                   "principle":"A claim without diagnostic data is an opinion with legal fees."},
                  {"key":"blame_ops","label":"Commission operator retraining — new lines always ramp slowly and the test proved the machine","quality":10,
                   "consequence":"Six weeks of retraining moves the plateau to 63%; the narrow-neck configuration issue trains no better, and the warranty window shortens throughout.",
                   "principle":"'The test proved the machine' — a 4-hour, one-SKU, vendor-driven test proved a 4-hour, one-SKU, vendor-driven machine."}]},
               {"key":"integrator","prompt":"Stage 2 — re-engaging the integrator?",
                "options":[
                  {"key":"evidence_deal","label":"Take the data, not the anger: present the station/SKU pattern, invoke the warranty for the configuration items it clearly covers, and propose a joint ramp-up sprint (their engineers, your crews, shared targets) for the contested remainder — settlement shaped like collaboration","quality":100,
                   "consequence":"Faced with attributable data, the integrator's engineers fix the two stations under warranty in a fortnight; the joint sprint finds a changeover procedure worth another 12 points, and the line crosses 90% before the season peak.",
                   "principle":"Data converts a warranty standoff into a scope conversation — and a joint sprint lets the vendor help without confessing."},
                  {"key":"formal","label":"Formal rejection of the line and demand for full remediation at nameplate","quality":20,
                   "consequence":"The acceptance certificate you signed is the first exhibit against you; the formal route runs eight months, and the line runs at 60% the whole way.",
                   "principle":"Rejecting what you accepted is the hardest legal road — check what you signed before choosing it."},
                  {"key":"quiet","label":"Fix it yourselves and preserve the vendor relationship for the next project","quality":15,
                   "consequence":"Your team reverse-engineers the configuration over a quarter; the 'preserved' relationship prices the next project knowing you absorb their gaps.",
                   "principle":"Absorbing a vendor's shortfall to protect the relationship teaches them the relationship absorbs shortfalls."}]},
               {"key":"acceptance","prompt":"Stage 3 — the lesson for the NEXT acceptance test?",
                "options":[
                  {"key":"regime","label":"Rewrite the standard: acceptance requires sustained multi-shift runs across the production SKU mix with YOUR operators driving, ramp-up support obligations surviving handover, and payment milestones tied to sustained (not demonstrated) rates","quality":100,
                   "consequence":"The next line's acceptance takes four days instead of four hours — and its ramp curve reaches nameplate in five weeks because the test had already forced the configuration work.",
                   "principle":"An acceptance test is a prophecy about operations — write it to predict YOUR factory, not the vendor's demonstration."},
                  {"key":"tolerate","label":"Accept that ramps are always slow — build longer ramps into future business cases","quality":25,
                   "consequence":"Honest, and surrendered: the business cases now pre-forgive vendor shortfalls that a better test would have priced to the vendor.",
                   "principle":"Planning around a fixable failure mode makes it a permanent one."},
                  {"key":"penalties","label":"Add heavier performance penalties to the next contract","quality":30,
                   "consequence":"Penalties against the same weak test just re-litigate the same ambiguity at higher stakes; the TEST is the control, the penalty only prices it.",
                   "principle":"A penalty is only as good as the measurement that triggers it."}]}],
             "hints":["Cut the stoppage data by SKU, station and shift — the pattern names the owner.",
               "Bring the vendor evidence and an exit ramp, not just a claim.",
               "The durable fix is the acceptance regime — tests should rehearse reality."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Cracked a 60% ramp plateau with a data cut that named the owner of every lost point."}
            """),

        ("WC-RSC-210", "Rollback is also a decision", "Cutover weekend, hour 30: half the services migrated, half didn't, and Monday is coming.",
            "Public Programmes", "Cutover Command Lead", "project_management", "professional", 14,
            """["recovery_management","risk_management"]""",
            """
            {"context":"Hour 30 of a 48-hour benefits-system cutover: 6 of 11 service modules migrated and verified, module 7 (payments calculation) has failed verification twice with a data-mapping defect, modules 8–11 depend on 7. The rollback plan — tested — needs 10 hours to execute safely. Monday 8am, 4,000 caseworkers log in to SOMETHING. Your decision point, per the runbook, is now.",
             "evidence":[
               {"label":"State","value":"6/11 verified · module 7 failed twice · 8–11 depend on 7"},
               {"label":"Defect","value":"Data-mapping error in payments calculation; fix estimate 'hours, unclear how many'"},
               {"label":"Rollback","value":"Tested; needs 10 hours to execute safely"},
               {"label":"Immovable","value":"Monday 8am: 4,000 caseworkers, live benefit payments"}],
             "decisions":[
               {"key":"call","prompt":"Stage 1 — the hour-30 call?",
                "options":[
                  {"key":"gate","label":"Set the real deadline and hold it: rollback needs 10 hours + verification, so the go/no-go on module 7's fix is hour 34 — the fix team gets four more hours with a hard stop, rollback prep starts NOW in parallel, and 'unclear how many hours' is treated as the answer it is","quality":100,
                   "consequence":"At hour 34 the fix is close-but-unverified; rollback executes with time to verify, and Monday opens on the old system, boringly — the enquiry that never happened is the deliverable.",
                   "principle":"The rollback window defines the decision point — protect the ability to retreat and the retreat stays a choice instead of a crash."},
                  {"key":"press","label":"Press on — the fix is 'hours away' and rolling back wastes six verified modules","quality":5,
                   "consequence":"'Hours away' consumes the rollback window at hour 38; Monday opens on a half-migrated system paying benefits from an unverified calculation module, and the word for what follows is 'inquiry'.",
                   "principle":"Sunk verified modules are not a reason to spend the retreat — they will still be verified next window."},
                  {"key":"partial","label":"Go live with modules 1–6 and run 7–11 on the legacy system — hybrid Monday","quality":25,
                   "consequence":"Feasible only if the seam between new-6 and old-7 was ever designed or tested — it wasn't; improvising an integration boundary at hour 30 is a third migration, unrehearsed.",
                   "principle":"A hybrid nobody rehearsed is not a fallback; it is a new project scheduled for the worst possible night."}]},
               {"key":"after","prompt":"Stage 2 — Monday morning, after the rollback?",
                "options":[
                  {"key":"honest_fast","label":"Own the weekend publicly and fast: staff briefing before 8am (old system today, why, what next), the defect's root cause into a dated fix plan, and the next window negotiated THIS week while the rehearsal's lessons are hot","quality":100,
                   "consequence":"Caseworkers open the familiar system with an explanation instead of a rumour; the mapping defect — reproducible now under no time pressure — is fixed and re-verified in nine days, and window two succeeds quietly.",
                   "principle":"A clean rollback plus honest communication is a recoverable event; silence converts it into a confidence problem."},
                  {"key":"minimise","label":"Describe it as 'planned maintenance completed' — no need to advertise a failed cutover","quality":5,
                   "consequence":"4,000 caseworkers heard the cutover was this weekend; the euphemism travels to a select committee via a staff forum screenshot.",
                   "principle":"An organisation that lies about small failures pre-authorises disbelief about big ones."},
                  {"key":"postpone","label":"Take a quarter to 'do it properly' before attempting another window","quality":30,
                   "consequence":"The defect needed nine days, not ninety; the quarter of caution lets the fix team scatter and the rehearsal knowledge decay — window two inherits staleness instead of momentum.",
                   "principle":"After a clean rollback, the asset is a hot, rehearsed team — spend it while it exists."}]}],
             "hints":["Work backwards from the rollback duration — that arithmetic sets the decision hour, not hope.",
               "'Unclear how many hours' is data: it means the estimate does not exist.",
               "After a rollback, speed of honesty determines whether it reads as control or chaos."],
             "profile_map":{"decision":"Recovery Leader","balanced":"Governance Steward"},
             "share_line":"Called an hour-34 rollback that made Monday boring — the best possible outcome."}
            """),

        // ───────────── July · Cost & Value dailies · foundation ─────────────

        ("WC-CST-211", "The index in the invoice", "The fuel escalation claim cites an index. Check which one before you pay.",
            "Transport Infrastructure", "Cost Verification Analyst", "project_controls", "foundation", 9,
            """["cost_control","commercial_management"]""",
            """
            {"context":"The haulage contractor's quarterly escalation claim applies a 14% fuel uplift across all transport rates, citing 'published index movement'. Your contract's escalation clause names a specific index (the national bulk-diesel series) applied to the FUEL ELEMENT of rates only — which the pricing schedule sets at 30% of the rate. The claim applies 14% to the whole rate. The invoice is due for certification Friday.",
             "evidence":[
               {"label":"Claim","value":"14% uplift on ALL transport rates"},
               {"label":"Contract","value":"Named diesel index, applied to fuel element only"},
               {"label":"Pricing schedule","value":"Fuel element = 30% of rate"},
               {"label":"Correct arithmetic","value":"14% × 30% = 4.2% of rate"}],
             "decisions":[
               {"key":"certify","prompt":"Friday's certification?",
                "options":[
                  {"key":"correct","label":"Certify at the contractual arithmetic — the named index on the fuel element — with a one-page derivation attached, and offer a session to reconcile methods before next quarter's claim","quality":100,
                   "consequence":"The contractor's QS, shown the derivation, concedes the mechanism in one call ('worth a try'); next quarter's claim arrives pre-calculated correctly.",
                   "principle":"Escalation clauses are formulas, not vibes — certify the formula and show the working."},
                  {"key":"pay","label":"Certify as claimed — fuel genuinely rose and the relationship matters","quality":5,
                   "consequence":"A 10-point over-certification on every transport rate, compounding quarterly, discovered at final account by an auditor who asks who checked quarter one.",
                   "principle":"Paying a mis-applied formula once re-writes the formula for the whole job."},
                  {"key":"reject","label":"Reject the claim outright for misapplication","quality":30,
                   "consequence":"The legitimate 4.2% inside the inflated 14% is now late too; the contractor escalates a payment dispute you could have certified around.",
                   "principle":"Certify what is due, contest what is not — rejection of the whole punishes the valid part."}]}],
             "hints":["Read the escalation clause for three things: which index, applied to what, from when.",
               "The fuel element percentage is the multiplier everyone forgets.",
               "Certify the correct amount rather than rejecting the incorrect claim."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Certified the formula, not the claim — and taught next quarter's invoice to arrive correct."}
            """),

        ("WC-CST-212", "The quarter ends Friday. The invoices don't.", "What you accrue tonight decides whether the report is true.",
            "Enterprise Programmes", "Assistant Management Accountant", "project_controls", "foundation", 10,
            """["cost_control","reporting"]""",
            """
            {"context":"Quarter-end cutoff is Friday. Work performed but not yet invoiced: roughly 700k across three subcontractors (site records support it). The delivery director suggests accruing 'only what's invoiced — keeps the quarter's number down and the rest lands next quarter anyway'. The programme's quarterly report feeds a covenant calculation at group level.",
             "evidence":[
               {"label":"Performed, uninvoiced","value":"~700k, supported by site records"},
               {"label":"Suggestion","value":"Accrue invoiced-only; 'lands next quarter anyway'"},
               {"label":"Stakes","value":"Quarterly report feeds a group covenant calculation"},
               {"label":"Your role","value":"You prepare the accrual schedule"}],
             "decisions":[
               {"key":"accrue","prompt":"Your accrual schedule shows:",
                "options":[
                  {"key":"full","label":"The full evidenced accrual — cost is recognised when work is performed, and the site records are the evidence; the director's schedule concern answered separately, with a forecast note","quality":100,
                   "consequence":"The quarter shows the true position; the covenant calculation is tight but honest, and the group CFO learns of the tightness from the report rather than from a restatement.",
                   "principle":"Accruals follow performance, not invoicing convenience — especially when a covenant is downstream of the number."},
                  {"key":"invoiced","label":"Invoiced-only as suggested — it is a timing difference, not a misstatement","quality":0,
                   "consequence":"The 700k 'timing difference' flatters a covenant test; when next quarter carries double cost, the group's auditors trace the pattern to your schedule and the word used is not 'timing'.",
                   "principle":"A deliberate understatement that improves a covenant test has a name, and it is not 'timing'."},
                  {"key":"partial","label":"Accrue half — a prudent middle that softens both problems","quality":10,
                   "consequence":"A number supported by neither the invoices nor the site records — the only indefensible option on the table.",
                   "principle":"Splitting the difference between right and wrong produces auditable wrongness."}]}],
             "hints":["Ask what event recognises cost: performance, or paperwork?",
               "Note what the number feeds — covenant inputs raise the stakes of 'timing'.",
               "The director's real concern is optics; answer it with narrative, not with the ledger."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Accrued what the site records proved, not what the invoices had gotten around to."}
            """),

        ("WC-CST-213", "The donor's wing", "A benefactor added a floor. Nobody added the running costs.",
            "Healthcare Estates", "Capital Programme Analyst", "project_controls", "foundation", 8,
            """["cost_control","governance"]""",
            """
            {"context":"Mid-construction, a benefactor has pledged 2M to add a rooftop therapy garden to the hospital's new ward block — fully covering the capital works. The trust's executive is delighted and wants it instructed immediately. Your check: the pledge covers construction only; the garden adds ~180k/year in maintenance, staffing and compliance costs that sit in nobody's revenue budget, forever.",
             "evidence":[
               {"label":"Pledge","value":"2M — capital works fully covered"},
               {"label":"Gap","value":"~180k/yr revenue costs (maintenance, staffing, compliance) — unbudgeted"},
               {"label":"Pressure","value":"Executive wants it instructed now"},
               {"label":"Nature","value":"A gift, politically delicate to question"}],
             "decisions":[
               {"key":"gift","prompt":"Your advice before instruction?",
                "options":[
                  {"key":"whole_life","label":"Support acceptance WITH the whole-life case attached: the 180k/yr named, a revenue owner secured before instruction, and the benefactor asked — graciously — whether an endowment element could accompany the capital gift","quality":100,
                   "consequence":"The benefactor, asked well, endows five years of running costs ('nobody ever mentions maintenance — thank you'); the garden opens with an owner and a budget, and stays open.",
                   "principle":"A capital gift with unfunded revenue is a subscription someone else signed you up for — complete the gift before accepting it."},
                  {"key":"accept","label":"Instruct it — questioning a 2M gift over 180k of running costs is ingratitude with a spreadsheet","quality":10,
                   "consequence":"The garden opens to fanfare; year two's budget round closes it quietly ('unfunded pressure'), and the shuttered gift becomes the benefactor's last.",
                   "principle":"Accepting the capital while orphaning the revenue doesn't honour the gift — it schedules its failure."},
                  {"key":"decline","label":"Advise declining — scope additions mid-construction are poor practice regardless of funding","quality":15,
                   "consequence":"Process-pure and tone-deaf: a fundable, wanted facility dies for want of the conversation that option one had.",
                   "principle":"'Poor practice' is an argument for doing it properly, not for not doing it."}]}],
             "hints":["Every capital gift casts a revenue shadow — measure it before accepting.",
               "Benefactors respond well to whole-life honesty; many will fund what they're shown.",
               "Secure the revenue owner BEFORE instruction — afterwards, it is everyone's and no one's."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Executive Communicator"},
             "share_line":"Completed a generous gift with the running costs nobody had mentioned."}
            """),

        ("WC-CST-214", "Provisional no longer", "Three provisional sums, two-thirds spent, and the certainty to fix them exists at last.",
            "Construction", "Quantity Surveyor", "project_controls", "foundation", 11,
            """["cost_control","commercial_management"]""",
            """
            {"context":"Your school-refurbishment contract carries three provisional sums — asbestos removal (200k), structural repairs (150k), services diversions (100k) — of which 290k of 450k is already expended, mostly on asbestos. All three scopes are now fully surveyed and designable: the uncertainty the sums existed for is gone. The contractor is comfortable leaving them provisional ('flexibility for everyone').",
             "evidence":[
               {"label":"Sums","value":"Asbestos 200k · structural 150k · diversions 100k"},
               {"label":"Spent","value":"290k of 450k, expenditure running ahead of substantiation"},
               {"label":"Change","value":"All three scopes now surveyed — uncertainty resolved"},
               {"label":"Contractor","value":"Happy to keep them provisional"}],
             "decisions":[
               {"key":"convert","prompt":"Your move on the sums?",
                "options":[
                  {"key":"firm","label":"Instruct firm quotations for all three remaining scopes NOW that they are designable — converting provisional to fixed while competition-by-comparison (the survey data) still gives you a benchmark","quality":100,
                   "consequence":"The firm prices land 8% under the provisional allowances; the remaining 160k of provisional flexibility becomes 147k of fixed certainty, and monthly valuations stop being negotiations.",
                   "principle":"A provisional sum outliving its uncertainty is a blank cheque with a survey attached — convert the moment the unknown becomes known."},
                  {"key":"keep","label":"Keep them provisional — flexibility genuinely helps both parties mid-job","quality":10,
                   "consequence":"'Flexibility' prices every remaining instruction at cost-plus in a market the contractor reads better than you; the final account's biggest arguments are all provisional-sum expenditure.",
                   "principle":"Flexibility in pricing is an asset to exactly one party, and it is not the paying one."},
                  {"key":"audit","label":"Audit the 290k spent before touching the remaining structure","quality":40,
                   "consequence":"Worthwhile — and sequenced wrong: the audit takes six weeks during which another 80k flows through the unconverted sums; do both, conversion first.",
                   "principle":"Stop the open tap before measuring what already ran through it."}]}],
             "hints":["Ask what uncertainty each provisional sum still represents — if none, it has expired.",
               "The survey data is your pricing benchmark; it is worth most at conversion time.",
               "Convert forward-looking spend first; audit historic spend second."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Converted three expired provisional sums to firm prices while the benchmark was fresh."}
            """),

        ("WC-CST-215", "Capitalise the argument", "Finance wants it opex. The programme wants it capex. The rules want the truth.",
            "Technology Programmes", "Programme Finance Analyst", "project_controls", "foundation", 12,
            """["cost_control","governance"]""",
            """
            {"context":"Quarter-end review of the platform programme's 3.2M spend to date: the programme director wants the 800k of 'configuration and process design' work capitalised ('it builds the asset — and it protects this year's opex budget'). Your reading of the capitalisation policy: the development work qualifies; the process-redesign and training elements (roughly 300k of the 800k) plainly don't. The auditors sampled this programme last year.",
             "evidence":[
               {"label":"Contested","value":"800k 'configuration and process design'"},
               {"label":"Your analysis","value":"~500k qualifies (development); ~300k doesn't (process redesign, training)"},
               {"label":"Motive offered","value":"'Protects this year's opex budget'"},
               {"label":"History","value":"Auditors sampled this programme last year"}],
             "decisions":[
               {"key":"classify","prompt":"Your classification memo says:",
                "options":[
                  {"key":"split","label":"The split the policy supports — 500k capitalised with the mapping to policy criteria documented per work order, 300k expensed — and the opex pressure escalated as a budget issue, not solved as an accounting one","quality":100,
                   "consequence":"The auditors' sample hits three of your work orders and finds the mapping already documented; the opex gap gets a real conversation with finance instead of a hidden one with the balance sheet.",
                   "principle":"Classification follows the nature of the work — budget pressure is a reason to talk to finance, never a reason to move cost between statements."},
                  {"key":"all","label":"Capitalise the full 800k — it is all 'part of building the capability' on a broad reading","quality":5,
                   "consequence":"The broad reading meets last year's audit sample pattern; the adjustment, when it comes, restates the quarter AND flags the programme for full-scope review.",
                   "principle":"A classification chosen for its budget effect is the first thing an auditor is trained to find."},
                  {"key":"defer","label":"Park it in a holding account and classify at year-end with more information","quality":20,
                   "consequence":"The holding account grows for three quarters; year-end classifies it under time pressure with less memory, worse records and the same rules.",
                   "principle":"Deferring classification defers nothing except the quality of the answer."}]}],
             "hints":["Map each work order to the policy's actual criteria — the split usually writes itself.",
               "The stated motive ('protects opex') is the red flag, not the argument.",
               "Solve budget pressure in the budget process; the ledger is not a pressure valve."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Split an 800k classification by the policy, not the budget pressure behind it."}
            """),

        ("WC-CST-216", "The euro moved. The invoice didn't yet.", "Six months until the presses ship. The exchange rate is shipping daily.",
            "Enterprise Programmes", "Commercial Analyst", "project_controls", "foundation", 9,
            """["cost_control","commercial_management"]""",
            """
            {"context":"Your print-facility programme ordered two presses from a German manufacturer at 4.2M EUR, payable on shipment in six months. Your budget was set at the order-date rate. Since then the currency has moved 4% against you — a 168k-equivalent hole if it holds — and your organisation has no hedging policy for programme-level exposure; finance says 'projects carry their own FX risk'.",
             "evidence":[
               {"label":"Exposure","value":"4.2M EUR, payable on shipment (~6 months)"},
               {"label":"Movement","value":"4% against you since order — ~168k if it holds"},
               {"label":"Policy","value":"None at programme level; 'projects carry FX risk'"},
               {"label":"Options","value":"Forward contract via treasury · do nothing · renegotiate currency"}],
             "decisions":[
               {"key":"fx","prompt":"Your recommendation?",
                "options":[
                  {"key":"hedge","label":"Ask treasury to book a forward for the shipment payment NOW — locking today's known 4% pain and eliminating the unknown tail — and report the crystallised variance honestly this quarter","quality":100,
                   "consequence":"The forward locks certainty at a known cost; when the currency moves another 3% before shipment, the programme's variance is already old news instead of new crisis.",
                   "principle":"A programme's job is delivering scope, not trading currency — hedge the exposure, book the truth, move on."},
                  {"key":"ride","label":"Do nothing — the rate may recover, and locking in a loss makes it real","quality":10,
                   "consequence":"The rate does not consult the programme's hopes; shipment day's spot rate adds another 130k, and the review asks why a known, hedgeable exposure rode unhedged for six months.",
                   "principle":"Not hedging IS a currency position — taken by someone unqualified to hold one."},
                  {"key":"renegotiate","label":"Ask the manufacturer to re-denominate the contract in your currency","quality":25,
                   "consequence":"The manufacturer agrees — at a rate 5% worse than today's forward, because they price THEIR new FX risk with a margin; you've paid a vendor to do treasury's job.",
                   "principle":"Currency risk moved to a counterparty comes back priced, with their profit on it."}]}],
             "hints":["Separate the sunk movement (already real) from the open exposure (still decidable).",
               "'No policy' doesn't mean no instruments — treasury can book forwards for a defined payment.",
               "Whoever holds FX risk prices it; the question is who prices it cheapest."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed a six-month currency exposure the day it was noticed, not the day it was paid."}
            """),

        ("WC-CST-217", "Claim it in March, earn it in May", "The grant milestone pays on commissioning. Commissioning is seven weeks late.",
            "Renewables", "Grant Compliance Officer", "project_controls", "foundation", 10,
            """["cost_control","governance"]""",
            """
            {"context":"Your community-solar programme's grant pays 600k on 'commissioning of phase 2' — defined in the grant agreement as successful G99 witness testing. The claim window for this funding year closes 31 March; witness testing, delayed by the network operator, is realistically mid-May. The delivery director suggests claiming in March anyway: 'the panels are physically installed, commissioning is a formality, and losing the window costs the community 600k'.",
             "evidence":[
               {"label":"Milestone","value":"600k on 'commissioning' = successful G99 witness test"},
               {"label":"Reality","value":"Witness testing ~mid-May (operator delay)"},
               {"label":"Window","value":"Funding-year claims close 31 March"},
               {"label":"Suggestion","value":"Claim in March — 'commissioning is a formality'"}],
             "decisions":[
               {"key":"claim","prompt":"Your position?",
                "options":[
                  {"key":"engage","label":"No false claim — but no silent forfeit either: approach the grant body NOW with the evidence of operator-caused delay and request a window extension or accrual treatment; funders have mechanisms for exactly this, and honesty is their trigger","quality":100,
                   "consequence":"The grant body — which fights fraud, not physics — grants a 90-day extension on the documented operator delay; the 600k pays in June, and the programme's compliance record stays spotless.",
                   "principle":"The choice is never 'false claim or lose the money' — the funder's flexibility exists, and candour is its price of admission."},
                  {"key":"claim_march","label":"Claim in March as suggested — the equipment exists and the community shouldn't lose out to an operator's queue","quality":0,
                   "consequence":"A claim certifying a test that hasn't happened is grant fraud, whatever the motive; the May witness test fails first time (they sometimes do), and the March certificate is now evidence.",
                   "principle":"A milestone claimed before its defining event is not early — it is false, and 'formality' is what people call tests before they fail them."},
                  {"key":"forfeit","label":"Accept the window loss — rules are rules and the money returns to the fund","quality":20,
                   "consequence":"Principled and passive: the extension that one honest letter would have secured goes unrequested, and the community loses 600k to a conversation that never happened.",
                   "principle":"Compliance without advocacy leaves legitimate money on the table."}]}],
             "hints":["Read the milestone's DEFINITION — the claim certifies that event, nothing else.",
               "Funders distinguish delay from deceit; documented third-party delay is their easiest case.",
               "Ask for the extension before the window closes — afterwards it is an appeal, not a request."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Saved a 600k grant with an honest letter instead of an early signature."}
            """),

        ("WC-CST-218", "Time and a half, every week", "Overtime became the plan. Nobody decided that.",
            "Energy Networks", "Project Cost Analyst", "project_controls", "foundation", 8,
            """["cost_control","resource_management"]""",
            """
            {"context":"Reviewing the substation project's labour costs, you find overtime running at 22% of hours for nine consecutive weeks — up from a 6% norm — concentrated in the cabling crews. The premium cost to date: ~140k over plan. The site manager's explanation: 'we're holding programme, aren't we?' The programme IS being held. The question is what the 22% is telling you.",
             "evidence":[
               {"label":"Overtime","value":"22% of hours, 9 weeks running (norm 6%)"},
               {"label":"Where","value":"Concentrated in cabling crews"},
               {"label":"Cost","value":"~140k premium over plan"},
               {"label":"Defence","value":"'We're holding programme'"}],
             "decisions":[
               {"key":"read","prompt":"Your reading and recommendation?",
                "options":[
                  {"key":"diagnose","label":"Treat sustained overtime as a symptom with a cause: is the cabling scope under-resourced (crew size vs quantities), under-productive (access/sequencing problems), or under-estimated? Diagnose which, then fix the cause — 22% for nine weeks is a structure, not a surge","quality":100,
                   "consequence":"The diagnosis finds cable-drum deliveries arriving unsorted, costing each crew ~90 minutes daily re-handling; a logistics fix drops overtime to 9% in three weeks — the 140k was buying a materials-handling problem's silence.",
                   "principle":"Sustained overtime is the most expensive way to not find out what is actually wrong."},
                  {"key":"accept","label":"Accept it — programme protection is worth 140k and the crews want the hours","quality":10,
                   "consequence":"The premium runs to 300k by completion; worse, week 14 brings the fatigue-related quality escape that nine weeks of six-day weeks statistically promised.",
                   "principle":"Overtime as a standing plan buys today's schedule with tomorrow's money and safety margin."},
                  {"key":"cap","label":"Cap overtime at 8% immediately — costs must return to plan","quality":15,
                   "consequence":"The cap lands on the symptom; the drum-handling problem still eats 90 minutes per crew per day, so the programme slips instead of the budget — the cause didn't read the memo.",
                   "principle":"Capping a symptom re-routes the cost; only the cause can cancel it."}]}],
             "hints":["Nine weeks is a pattern, not an emergency — patterns have structural causes.",
               "Ask what the crews are DOING in the premium hours; the answer usually names the fix.",
               "Neither paying silently nor capping blindly diagnoses anything."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Traced nine weeks of overtime to ninety minutes of daily drum-sorting."}
            """),

        // ───────────── July · other dailies · foundation ─────────────

        ("WC-GOV-219", "Signed above the line", "The variation was approved by someone whose limit it exceeds. Now what?",
            "Enterprise Programmes", "PMO Analyst", "project_management", "foundation", 5,
            """["governance","change_control"]""",
            """
            {"context":"Your monthly delegation-compliance check finds variation V-31 — 85k — approved solely by a workstream lead whose delegated limit is 50k. The work is done and paid; the variation was, on its merits, sensible and fairly priced. The lead says the project manager was on leave and 'it couldn't wait'. This is the first breach you've found; the check has only existed for two months.",
             "evidence":[
               {"label":"Breach","value":"85k approved under a 50k delegation"},
               {"label":"Merits","value":"Work sensible, fairly priced, complete"},
               {"label":"Explanation","value":"PM on leave; 'couldn't wait'"},
               {"label":"Context","value":"First breach found; check is new"}],
             "decisions":[
               {"key":"handle","prompt":"You:",
                "options":[
                  {"key":"regularise","label":"Record it as a breach, obtain retrospective ratification at the right level (documenting the merits), and fix the gap the excuse revealed — a deputisation rule for approver absence — in the same memo","quality":100,
                   "consequence":"The breach is on the record with its ratification; the deputisation rule kills the 'couldn't wait' excuse forever, and the control's first catch strengthened it instead of embarrassing everyone.",
                   "principle":"A control's first breach sets its reputation: regularise the instance, repair the gap, punish only patterns."},
                  {"key":"quiet","label":"Let it pass — the decision was good and flagging it punishes initiative","quality":10,
                   "consequence":"The unflagged breach becomes the precedent; six months later the same lead approves 140k 'because last time was fine', and the control is decoration.",
                   "principle":"A limit unenforced at 85k has already been renegotiated to whatever comes next."},
                  {"key":"formal","label":"Report it as a formal disciplinary matter — limits are limits","quality":20,
                   "consequence":"The lead who acted reasonably in a genuine gap is made the example; the workforce learns to let urgent things wait, which was never the goal.",
                   "principle":"Enforcing the letter against good-faith actors teaches bad faith to the rest."}]}],
             "hints":["Separate the instance (ratifiable), the gap (fixable) and the pattern (absent).",
               "The excuse is the most useful evidence — it names the missing rule.",
               "The first catch calibrates whether the control is feared, ignored or respected."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a delegation breach into a ratification, a deputy rule and a stronger control."}
            """),

        ("WC-QLT-220", "Severity two, says who", "The defect triage meeting has become a negotiation. Today's case decides the norm.",
            "Enterprise Programmes", "Test Manager", "project_management", "foundation", 7,
            """["quality_management","governance"]""",
            """
            {"context":"System-test triage on a claims platform: defect D-412 — postcode lookup fails for ~2% of addresses, workaround exists (manual entry) — is graded severity-2 by the test team ('core function impaired') and disputed by the delivery lead who wants severity-3 ('workaround exists, ship it'). The severity definitions are written; D-412 genuinely sits near the boundary. Go-live gating counts sev-2s.",
             "evidence":[
               {"label":"D-412","value":"Postcode lookup fails ~2% of addresses; manual workaround"},
               {"label":"Definitions","value":"Sev-2: core function impaired · Sev-3: minor with workaround"},
               {"label":"Stakes","value":"Go-live gate counts sev-2s"},
               {"label":"Dynamic","value":"Delivery lead pressing for the downgrade"}],
             "decisions":[
               {"key":"grade","prompt":"Your ruling as triage chair?",
                "options":[
                  {"key":"criteria","label":"Grade it by the USER's experience, not the negotiator's need: 2% of address lookups failing in a claims-entry flow impairs a core function — sev-2 stands, AND the definitions get a boundary example added so the next D-412 isn't a negotiation","quality":100,
                   "consequence":"The gate counts one more sev-2, honestly; the fix takes four days, and the amended definitions turn future boundary fights into lookups.",
                   "principle":"Severity describes the defect's effect on users — the go-live gate's appetite is not an input to the grading."},
                  {"key":"pressure","label":"Concede sev-3 — a workaround exists and the definitions arguably allow it","quality":10,
                   "consequence":"The precedent is set at triage speed: every boundary defect now arrives with a delivery-lead advocate, and 'severity' becomes a synonym for 'schedule position'.",
                   "principle":"The first severity graded by negotiation reclassifies the whole register as negotiable."},
                  {"key":"split","label":"Log it sev-2 but exclude it from the gate count 'by exception'","quality":15,
                   "consequence":"An exception process invented mid-triage for one defect; the gate now has a side door, and side doors widen.",
                   "principle":"A gate with ad-hoc exceptions is a gate in name only."}]}],
             "hints":["Grade from the affected user's seat, not the release calendar's.",
               "Boundary cases are definition-improvement opportunities — capture them.",
               "Watch what precedent the ruling sets for the next negotiation."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a severity line and upgraded the definitions so it never needed holding again."}
            """),

        ("WC-COM-221", "The minister wants a hard hat", "A ministerial visit, five days' notice, and a site in its worst week.",
            "Public Programmes", "Programme Communications Manager", "project_management", "foundation", 6,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"The department's private office requests a ministerial site visit to your hospital-build programme next Thursday — five days away, which happens to be the week the site is recovering from the ground-floor slab problem: extra plant, visible rework, and a site team working flat out. The programme director's instinct: 'decline — we can't show a minister a recovery week'. The private office does not receive 'no' often.",
             "evidence":[
               {"label":"Request","value":"Ministerial visit Thursday — 5 days' notice"},
               {"label":"Site state","value":"Slab rework week: extra plant, visible recovery work"},
               {"label":"Director instinct","value":"'Decline — wrong week'"},
               {"label":"Reality","value":"Private offices remember refusals"}],
             "decisions":[
               {"key":"visit","prompt":"Your advice?",
                "options":[
                  {"key":"shape","label":"Accept and SHAPE it: route the visit through the completed wing and the apprentice training hub, brief the minister's team honestly that recovery works are underway ('you may see them — here's the one-line answer'), and protect the site team's hours with a tight 45-minute format","quality":100,
                   "consequence":"The visit lands well — ministers see construction sites, not showrooms; the honest pre-brief means the rework question, when asked, has a confident answer, and the private office logs the programme as 'easy to work with'.",
                   "principle":"Shape visits, don't dodge them — a well-briefed truth beats a hidden one, and access refused is remembered longer than imperfection shown."},
                  {"key":"decline","label":"Decline as the director wants — offer 'a better week' next month","quality":15,
                   "consequence":"The visit goes to a rival programme; the private office's note says 'declined at short notice', which reads worse in Whitehall than any slab problem reads on site.",
                   "principle":"To an important stakeholder, 'not this week' sounds like 'something to hide' — and there's no photo of the week you offered instead."},
                  {"key":"stage","label":"Accept and sanitise — pause the rework for the day, move the plant out of sight","quality":10,
                   "consequence":"A day of recovery lost to stagecraft, a site team that watched it happen, and one journalist's question — 'is the programme on schedule?' — now sitting on a staged set.",
                   "principle":"Sanitising a site for a visit trades a day of progress for a lie that photographs well."}]}],
             "hints":["The choice isn't accept/decline — it's shaped/unshaped.",
               "Pre-brief the awkward thing; surprises are the only true visit failures.",
               "Guard the site team's time with format, not refusal."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Hosted a minister in recovery week — shaped, briefed, and better for it."}
            """),

        // ───────────── July · Stakeholder Dilemmas · practitioner ─────────────

        ("WC-STK-222", "The recovery is working. The people aren't.", "Week nine of six-day weeks. The schedule says push. The signals say stop.",
            "Enterprise Programmes", "Recovery Workstream Lead", "project_management", "professional", 6,
            """["resource_management","leadership"]""",
            """
            {"context":"Your recovery plan is succeeding: four of six lost weeks recovered, two to go, on the back of sustained six-day working. Week nine's signals: two resignations in the wider team, a near-miss error in deployment (caught in review), and your best engineer asking to 'talk about options'. The remaining recovery needs roughly five more weeks at current intensity — or eight at normal hours.",
             "evidence":[
               {"label":"Recovery","value":"4 of 6 weeks recovered; 2 to go"},
               {"label":"Cost signals","value":"2 resignations · a caught deployment near-miss · top engineer 'wants options'"},
               {"label":"Arithmetic","value":"5 more weeks at six-day pace, or 8 at normal"},
               {"label":"Decision","value":"Yours — the intensity is your call"}],
             "decisions":[
               {"key":"pace","prompt":"You:",
                "options":[
                  {"key":"downshift","label":"Bank the recovery and downshift NOW: normal hours from Monday, the three extra weeks presented to the sponsor as the price of a team that still exists — with the near-miss as exhibit A","quality":100,
                   "consequence":"The sponsor, shown the near-miss and the resignation pattern, takes the eight-week path without argument; the top engineer stays, and the recovery completes with a team instead of a casualty list.",
                   "principle":"A recovery that arrives with a broken team has moved the failure, not fixed it — bank gains and downshift before the signals become events."},
                  {"key":"push","label":"Push the final five weeks — so close, and easing off now wastes the momentum","quality":10,
                   "consequence":"Week eleven's error is not caught in review; the incident costs three weeks and the engineer, and the recovery ends later than the honest downshift would have — with a worse story.",
                   "principle":"Fatigue compounds silently until it converts to incidents — the last stretch at full burn is where recoveries break."},
                  {"key":"rotate","label":"Keep the pace but rotate fresh people in from other teams","quality":30,
                   "consequence":"The fresh people need three weeks of context to be useful — the recovery's specialised tail is exactly where rotation helps least; meanwhile the donor teams start their own overtime.",
                   "principle":"Rotation works for interchangeable work; recovery tails are rarely that."}]}],
             "hints":["A caught near-miss is the cheapest warning you will ever receive — price it.",
               "Recoveries are judged by their endings; teams remember the last month most.",
               "Take the intensity decision to the sponsor as a priced choice, not a fait accompli."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Banked a recovery's gains and downshifted before the near-miss became the incident."}
            """),

        ("WC-STK-223", "The clerk of works and the site team", "The client's inspector is technically right, procedurally relentless, and destroying morale.",
            "Construction", "Project Manager", "project_management", "professional", 7,
            """["stakeholder_communication","conflict_management"]""",
            """
            {"context":"The client's clerk of works on your school project is diligent and technically sound — and operates by ambush: verbal instructions to operatives (bypassing your supervision), snag lists issued at 5pm Fridays, and a running commentary to the client that frames every routine query as a 'quality concern'. Your site team now avoids him; two foremen have asked to move projects. The relationship is damaging the job both ways — his findings ARE often valid.",
             "evidence":[
               {"label":"The person","value":"Technically sound, findings often valid"},
               {"label":"The methods","value":"Verbal instructions to operatives · 5pm Friday snag lists · escalatory commentary"},
               {"label":"The damage","value":"Site team avoidance; 2 foremen requesting transfers"},
               {"label":"The client","value":"Hears 'quality concerns' weekly"}],
             "decisions":[
               {"key":"reset","prompt":"Your move?",
                "options":[
                  {"key":"protocol","label":"Fix the CHANNEL, honour the content: agree a written inspection protocol with him and the client — findings via the daily register (not verbal instructions to operatives), joint weekly walk-downs, responses within 48 hours — presenting it as taking his findings MORE seriously, because it is","quality":100,
                   "consequence":"He agrees — ambush was his lever for being heard, and the protocol guarantees hearing; findings keep coming, now through a door the site team can answer, and the transfer requests quietly lapse.",
                   "principle":"With a difficult-but-right stakeholder, formalise the channel and honour the content — most ambush behaviour is a badly-expressed demand to be taken seriously."},
                  {"key":"counter","label":"Push back through the client: his conduct is disrupting the works and must be reined in","quality":15,
                   "consequence":"The client hears their diligent inspector being attacked by the contractor he inspects; his commentary now has a persecution narrative attached, and his findings keep being valid.",
                   "principle":"Attacking a valid critic's manner reads as attacking his findings — and loses both arguments."},
                  {"key":"absorb","label":"Coach the site team to tolerate it — client's inspector, client's rules","quality":10,
                   "consequence":"The foremen transfer; their replacements inherit the dynamic plus the vacancy gap, and the verbal-instruction channel eventually produces a genuinely dangerous miscommunication.",
                   "principle":"Tolerating a broken channel because the content is valid keeps the content and doubles the breakage."}]}],
             "hints":["Separate what he finds (keep) from how he transmits it (fix).",
               "Design the protocol so it serves HIS need to be heard — that is why he'll sign it.",
               "Verbal instructions to operatives is the safety-critical piece; lead with it."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Rechannelled a relentless clerk of works without losing a single valid finding."}
            """),

        ("WC-STK-224", "Two supervisors, one night shift", "The night shift's output halved. The two people running it aren't speaking.",
            "Industrial Manufacturing", "Operations Project Lead", "project_management", "professional", 5,
            """["resource_management","conflict_management"]""",
            """
            {"context":"Your line-conversion project's night shift — critical to the changeover window — has halved its output over two weeks. The cause, according to everyone except the two people involved: the mechanical supervisor and the E&I supervisor have stopped speaking after a dispute about a lifted access instruction, and their crews now work in silos, each waiting for the other's 'formal handover'. Both are good at their jobs. The window has three weeks left.",
             "evidence":[
               {"label":"Output","value":"Halved over 2 weeks"},
               {"label":"Cause","value":"Two supervisors not speaking since an access-instruction dispute"},
               {"label":"Effect","value":"Crews in silos, waiting on 'formal handovers'"},
               {"label":"Clock","value":"3 weeks left in the changeover window"}],
             "decisions":[
               {"key":"fix","prompt":"You:",
                "options":[
                  {"key":"mediate_structure","label":"Deal with the dispute AND the dependency: a direct three-way tonight to settle the access-instruction question on its merits, then a joint shift-start coordination meeting (15 minutes, both crews) instituted as permanent structure — the feud loses its mechanism","quality":100,
                   "consequence":"The instruction dispute takes twenty minutes to settle (both were half-right); the shift-start meeting makes silo-working structurally impossible, and output recovers inside a week.",
                   "principle":"Settle the grievance on its merits, then remove the structure that let two people's silence become a shift's throughput."},
                  {"key":"separate","label":"Move one supervisor to days — separation ends the standoff instantly","quality":20,
                   "consequence":"The standoff ends; the night shift now has one supervisor covering two disciplines, and the days-shift inherits a resentful specialist plus the story.",
                   "principle":"Separation resolves the symptom by redistributing the competence the shift needed."},
                  {"key":"order","label":"Instruct both in writing to cooperate — professional behaviour is not optional","quality":15,
                   "consequence":"They comply, in writing, minimally: cooperation becomes a documented performance, and the 'formal handovers' get MORE formal.",
                   "principle":"Ordered cooperation produces compliance-shaped friction — the dispute needs settling, not suppressing."}]}],
             "hints":["The dispute has actual merits — hear them before prescribing anything.",
               "Look for the structural mechanism that turned a feud into a throughput loss.",
               "Fifteen minutes of forced joint planning per shift beats any memo about attitude."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Settled the argument and deleted the structure that let it halve a shift."}
            """),

        // ───────────── July · sequencing pair ─────────────

        ("WC-SCO-225", "Build the claim file backwards", "The claim is strong. The file, currently, is a drawer.",
            "Events & Venues", "Commercial Coordinator", "project_controls", "foundation", 6,
            """["change_control","commercial_management"]""",
            """
            {"context":"Your exhibition-hall fit-out suffered five weeks of disruption from the venue's overrunning base-build works — a strong prolongation claim in principle. The evidence, currently: site diaries (complete), photos (unsorted, thousands), emails (scattered across four inboxes), a delay analysis (not started), and a quantum build-up (not started). The claim notice deadline under the contract is in three weeks. Order the work.",
             "evidence":[
               {"label":"Entitlement story","value":"Venue's base-build overran into your possession — strong in principle"},
               {"label":"Evidence state","value":"Diaries ✓ · photos unsorted · emails scattered · analysis not started · quantum not started"},
               {"label":"Deadline","value":"Contractual claim NOTICE due in 3 weeks"},
               {"label":"Resource","value":"You, plus a planner two days a week"}],
             "decisions":[
               {"key":"order","prompt":"The right sequence?",
                "options":[
                  {"key":"notice_first","label":"NOTICE first — it needs the event and the heads of claim, not the finished analysis: serve a compliant notice week one, then build the delay analysis (it structures which photos/emails matter), then assemble evidence to the analysis's skeleton, then quantum last","quality":100,
                   "consequence":"The notice lands with two weeks' margin, preserving the entitlement; the analysis-first evidence assembly means the thousands of photos are sifted once, against a structure, instead of thrice against guesses.",
                   "principle":"Serve the notice the contract requires, then build the file the tribunal would want — deadline items first, structure before bulk."},
                  {"key":"evidence_first","label":"Sort all the evidence first — a claim is only as good as its file","quality":10,
                   "consequence":"Three weeks of photo-sorting produces a beautiful archive and a missed notice deadline; the time-bar defence writes itself, and the archive documents a claim that no longer exists.",
                   "principle":"A perfect file behind a lapsed notice is a well-organised nothing."},
                  {"key":"quantum_first","label":"Start with the money — quantum gets attention and drives urgency","quality":15,
                   "consequence":"A quantum without a delay analysis is a number without a cause; it gets rebuilt after the analysis exists, which is to say twice.",
                   "principle":"Quantum is the LAST layer — it prices what the analysis proves about what the notice preserved."}]}],
             "hints":["Find the deadline item — contractual notices are time-bombs, everything else is homework.",
               "The delay analysis is the file's spine; evidence sorts fastest against a structure.",
               "Quantum prices conclusions; write the conclusions first."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Served the notice first and built the claim file to its spine, not its drawer."}
            """),

        ("WC-SCH-226", "Rank the rescues", "Five recovery options, one steering decision, and only the sequence makes them comparable.",
            "Technology Programmes", "Recovery Planning Analyst", "project_controls", "professional", 11,
            """["schedule_analysis","recovery_management"]""",
            """
            {"context":"Your delivery programme is six weeks late. Five recovery options are on the table: (A) add a second testing environment — buys 2 weeks, ready in 3; (B) descope the reporting module to phase 2 — buys 3 weeks, decision needed this week while the design is still uncommitted; (C) parallel-run two integration phases — buys 2 weeks, needs option A in place first; (D) overtime across the build teams — buys 1 week, available immediately, fatigue-limited to ~6 weeks' use; (E) accelerate vendor deliverables via a commercial incentive — buys 2 weeks, needs 4 weeks' vendor notice. Steering wants a ranked package, not a menu.",
             "evidence":[
               {"label":"A — 2nd test env","value":"+2wks · ready in 3"},
               {"label":"B — descope reporting","value":"+3wks · decision expires this week"},
               {"label":"C — parallel integration","value":"+2wks · REQUIRES A first"},
               {"label":"D — overtime","value":"+1wk · immediate · ~6wks safe use"},
               {"label":"E — vendor incentive","value":"+2wks · 4wks' notice"}],
             "decisions":[
               {"key":"package","prompt":"Your ranked package?",
                "options":[
                  {"key":"sequenced","label":"Rank by decision-expiry and dependency, not by weeks-bought: B decided THIS WEEK (expiring option, biggest single gain), E's notice served now (long fuse), A started now (enables C), C follows A, and D held as the reserve for whatever slips — a sequence, not a shopping list","quality":100,
                   "consequence":"B and E — the two clock-driven options — are captured before their windows close; A-then-C lands the dependent pair, and D's reserve week covers the one slippage that occurs. Total recovered: 9 of the needed 6, with margin.",
                   "principle":"Recovery options are ranked by when their doors close and what unlocks what — value-per-week is the SECOND sort key."},
                  {"key":"biggest","label":"Rank by weeks bought: B(3), then A/C/E(2 each), then D(1)","quality":25,
                   "consequence":"Right about B by luck; but E's four-week notice starts late (its two weeks arrive after they're needed), and C is scheduled before its prerequisite A — the ranking mistook a dependency graph for a league table.",
                   "principle":"Value-ranking without expiry-and-dependency sequencing wastes the options whose clocks were running."},
                  {"key":"cheap","label":"Start with free-and-immediate: D first, then decide the rest under less pressure","quality":10,
                   "consequence":"Overtime starts week one of a multi-month recovery, spending its six safe weeks on the least critical stretch — and B's descope window closes during the 'less pressure' that never came.",
                   "principle":"Playing the immediate card first burns your reserve at the moment you least need it."}]}],
             "hints":["For each option, find its clock: when does the choice expire or the fuse need lighting?",
               "Map the dependencies — an enabler ranks by what it unlocks, not what it buys alone.",
               "Hold the fatigue-limited option as the reserve, not the opener."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ranked five rescue options by their expiry dates and dependencies, not their headline weeks."}
            """),
    };
}
