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

        // ═════════════ AUGUST — leadership, teams, communication and conflict ═════════════
        // ───────────── Daily Decisions · communication · practitioner ─────────────

        ("WC-COM-227", "The councillor was in the room", "Your engineer called the council's process 'a joke'. The council heard.",
            "Public Programmes", "Programme Manager", "project_management", "professional", 6,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"In a public liaison meeting about your leisure-centre programme, your senior engineer — frustrated by a real permitting delay — described the council's approval process as 'frankly, a joke'. A councillor was in the audience. The engineer is your best; the frustration was legitimate; the sentence is tomorrow's minutes. The council's planning cooperation matters for two more phases.",
             "evidence":[
               {"label":"The words","value":"'Frankly, a joke' — about the council's process, publicly"},
               {"label":"The audience","value":"Included a councillor; meeting was minuted"},
               {"label":"The person","value":"Your best engineer; the underlying delay is real"},
               {"label":"The stake","value":"Council cooperation needed for 2 more phases"}],
             "decisions":[
               {"key":"respond","prompt":"Your next 24 hours?",
                "options":[
                  {"key":"repair_both","label":"Call the council's programme contact TODAY — acknowledge the comment was unprofessional, without disowning the underlying delay concern, which you then raise properly — and debrief the engineer privately: the frustration was right, the venue was not","quality":100,
                   "consequence":"The council hears an organisation that corrects itself; the permitting delay finally gets the formal escalation it needed, and the engineer — corrected without humiliation — self-edits thereafter.",
                   "principle":"Repair the relationship AND keep the substance: apologise for the register, escalate the issue, coach the person — three separate acts, all needed."},
                  {"key":"defend","label":"Let it stand — the process IS slow and the council should hear honest feedback","quality":10,
                   "consequence":"The 'honest feedback' framing reaches the planning department as institutional contempt; phase 3's approvals move at a pace that feels personal, because it is.",
                   "principle":"Being right about the substance never licenses the register — bureaucracies forgive challenges and remember insults."},
                  {"key":"punish","label":"Formally discipline the engineer and copy the council on the outcome","quality":15,
                   "consequence":"The council gets its pound of flesh; your best engineer gets a file note for one hot sentence, and the team learns that honesty about delays is career-limiting.",
                   "principle":"Sacrificing your own person to appease a stakeholder buys less goodwill than it costs trust."}]}],
             "hints":["Separate the register (wrong) from the concern (right) — handle each in its proper channel.",
               "Speed matters: the call made today is an apology; next week it is damage control.",
               "Coach in private, repair in public — never the reverse."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Turned a hot sentence into a cool apology and a proper escalation."}
            """),

        ("WC-COM-228", "The all-hands after the axe", "The workstream is cancelled. Forty people want to know what it means for them.",
            "Enterprise Programmes", "Programme Director", "project_management", "professional", 7,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"The board cancelled your programme's analytics workstream yesterday — a strategy change, not a performance issue. Twelve people are directly affected: eight redeploy within the programme, three move to another division, one role is at risk. The all-hands is tomorrow; rumours are already ahead of you ('the whole programme is next'). HR has cleared what you can say about individuals: nothing specific.",
             "evidence":[
               {"label":"Fact","value":"Workstream cancelled — strategy, not performance"},
               {"label":"People","value":"8 redeploy internally · 3 to another division · 1 at risk"},
               {"label":"Rumour","value":"'The whole programme is next'"},
               {"label":"Constraint","value":"Nothing specific about individuals"}],
             "decisions":[
               {"key":"allhands","prompt":"Tomorrow's all-hands leads with:",
                "options":[
                  {"key":"straight","label":"The decision and its reason, plainly; the numbers (8/3/1) without names; the timeline for individual conversations (this week, direct, private); and a direct answer to the rumour — the programme's mandate, reconfirmed in writing by the board","quality":100,
                   "consequence":"The room gets facts before corridor fiction hardens; the twelve hear their situations privately within days, and the rumour dies against the board's written reconfirmation.",
                   "principle":"After a cancellation: facts in public, futures in private, and kill the meta-rumour with evidence — the gap you leave is the story you get."},
                  {"key":"upbeat","label":"Frame it as 'exciting refocusing' — morale needs protecting and the language matters","quality":10,
                   "consequence":"Forty people hear a colleague's at-risk role described as exciting; the euphemism becomes a screenshot, and every future announcement is translated by cynics first.",
                   "principle":"Spin at an all-hands converts one bad day into a permanent credibility tax."},
                  {"key":"minimal","label":"Announce only the cancellation; defer all people questions to 'appropriate channels'","quality":20,
                   "consequence":"The room's only question — 'am I the one?' — goes unanswered for a week; productivity halts while forty people privately calculate their odds.",
                   "principle":"Withholding the shape of the people-impact doesn't protect anyone — it just privatises the anxiety."}]}],
             "hints":["The room's real question is 'am I safe?' — answer its shape even when names must wait.",
               "Address the rumour explicitly; ignored rumours read as confirmed.",
               "The 8/3/1 numbers are sayable and calming — specificity without names."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Delivered a cancellation all-hands that answered the question everyone actually had."}
            """),

        ("WC-COM-229", "The group chat that knows too much", "Handover photos, patient corridors, and forty members who aren't all staff.",
            "Healthcare Estates", "Project Manager", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"You discover the ward-refurbishment site team runs a WhatsApp group — 40+ members including subcontractors and two people nobody can identify — used for genuinely useful coordination AND containing progress photos that include ward corridors, a patient's name-board visible in one, plus candid commentary about hospital staff. It formed organically two years ago; it works; it is also an information-governance incident in waiting (or already).",
             "evidence":[
               {"label":"The group","value":"40+ members incl. subbies and 2 unidentified"},
               {"label":"Content","value":"Useful coordination + corridor photos (one with a patient name-board) + staff commentary"},
               {"label":"Context","value":"Hospital site — information governance rules apply"},
               {"label":"Value","value":"It genuinely coordinates the job"}],
             "decisions":[
               {"key":"channel","prompt":"You:",
                "options":[
                  {"key":"replace","label":"Replace, don't just ban: stand up a managed channel (project tool, verified membership, no-photo-in-clinical-areas rule) within the week, migrate the coordination value, close the WhatsApp group — and report the name-board photo to the trust's IG office yourself, today","quality":100,
                   "consequence":"The coordination survives in a channel with a membership list; the IG office, told by you first, treats it as self-reported and proportionate — the alternative discovery path had a very different tone.",
                   "principle":"Ban a useful-but-dangerous channel only by replacing its usefulness — and self-report the breach you found before it reports you."},
                  {"key":"ban","label":"Order the group closed immediately — the risk is obvious and the rules are clear","quality":25,
                   "consequence":"The group closes; a new one forms within days, smaller and hidden, minus you — the coordination need didn't read the order.",
                   "principle":"Communication bans without replacements create shadow channels with worse hygiene."},
                  {"key":"quiet_fix","label":"Ask admins to delete the photos and tighten membership — keep what works, no fuss","quality":10,
                   "consequence":"The photos are deleted from the group, not from forty phones; when the name-board image surfaces elsewhere, 'we quietly deleted it' reads exactly like concealment.",
                   "principle":"A known breach handled informally becomes YOUR breach the day it resurfaces."}]}],
             "hints":["The group exists because it solves a real problem — solve the problem in the replacement.",
               "The name-board photo is already an incident; the only choice is who reports it, and when.",
               "Verified membership is the difference between a channel and a leak."],
             "profile_map":{"decision":"Governance Steward","balanced":"Executive Communicator"},
             "share_line":"Replaced a leaky group chat with a governed channel — and self-reported the breach it held."}
            """),

        ("WC-COM-230", "Briefed, in what language", "The toolbox talk was delivered. A third of the gang didn't understand it.",
            "Construction", "Site Delivery Manager", "project_management", "professional", 6,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"Investigating a near-miss (a gang working under a load path that morning's briefing had prohibited), you find the root cause: the toolbox talk was delivered in rapid technical English to a gang where a third have limited English — they nodded, signed the sheet, and misunderstood the exclusion zone. The signing sheet says 'briefed'. The reality says otherwise. This is your best-performing subcontractor.",
             "evidence":[
               {"label":"Near-miss","value":"Gang under a prohibited load path"},
               {"label":"Root cause","value":"Briefing not understood — language barrier; sheet signed anyway"},
               {"label":"Scale","value":"~1/3 of the gang has limited English"},
               {"label":"Context","value":"Best-performing subcontractor on site"}],
             "decisions":[
               {"key":"fix","prompt":"Beyond closing the near-miss, you:",
                "options":[
                  {"key":"comprehension","label":"Rebuild briefing around COMPREHENSION, not attendance: visual exclusion-zone maps, briefings delivered bilingually via the gang's own bilingual chargehand (paid role), and spot-check understanding with questions, not signatures — rolled out site-wide, not just this gang","quality":100,
                   "consequence":"The next audit finds workers who can explain the exclusion zones rather than just point at their signatures; the bilingual chargehand model spreads to two other sites via the subcontractor themselves.",
                   "principle":"A signature proves presence; only a question proves understanding — brief for the second, audit the second."},
                  {"key":"paper","label":"Require translated written briefings for all languages on site","quality":30,
                   "consequence":"Six translated documents appear; the workers who couldn't follow spoken technical English get written technical English in translation — literacy and comprehension were the issue, not the alphabet.",
                   "principle":"Translating the paperwork translates the compliance, not the understanding."},
                  {"key":"blame","label":"Raise a nonconformance against the subcontractor for signing unbriefed workers","quality":15,
                   "consequence":"Technically valid; the subcontractor tightens signatures, workers learn to nod more convincingly, and the comprehension gap survives with better paperwork.",
                   "principle":"Punishing the record-keeping fixes the records — the near-miss came from the understanding."}]}],
             "hints":["Ask what the signature was actually evidence OF — presence, not comprehension.",
               "The gang's own bilingual members are the untapped channel — formalise and pay the role.",
               "Fix it site-wide; the audited gang is never the only one."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rebuilt site briefings around questions answered, not sheets signed."}
            """),

        ("WC-COM-231", "The wrong number, said proudly", "Your analyst told the community meeting the wrong CO2 figure. Confidently. On camera.",
            "Climate Adaptation", "Engagement Programme Lead", "project_management", "professional", 7,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"At last night's community meeting about your retrofit programme, your junior analyst — presenting for the first time — stated the scheme's carbon saving as 'forty thousand tonnes a year'. The real figure is four thousand. The error (a units slip in her notes) was confident, clear, and recorded on the council's livestream. A local campaign group has already clipped it. She realised this morning and is mortified.",
             "evidence":[
               {"label":"Error","value":"40,000 tCO2/yr stated; real figure 4,000"},
               {"label":"Visibility","value":"Council livestream; campaign group has the clip"},
               {"label":"The person","value":"Junior, first public presentation, mortified, self-reported"},
               {"label":"Risk","value":"Programme credibility on its central claim"}],
             "decisions":[
               {"key":"correct","prompt":"The correction strategy?",
                "options":[
                  {"key":"fast_owned","label":"Correct publicly TODAY, as the programme (not the person): a note to attendees and the council with the right figure and the cause ('a units error in our presentation — the assessment itself is unchanged, here it is') — and back the analyst visibly, including her delivering the corrected figure at the next meeting","quality":100,
                   "consequence":"The correction outruns the clip; the campaign group's 'they inflated by 10x' story dies against a same-day self-correction, and the analyst — publicly backed — becomes the programme's most careful presenter.",
                   "principle":"Correct at the speed and scale of the error, own it institutionally, and never feed the presenter to the crowd — audiences forgive corrected mistakes and remember abandoned juniors."},
                  {"key":"quiet","label":"Correct it in the next scheduled newsletter — a formal correction now amplifies the clip","quality":10,
                   "consequence":"Three weeks of the clip circulating uncorrected; by newsletter day the 40,000 figure has been quoted in an objection letter, and the correction now looks extracted rather than offered.",
                   "principle":"A correction delayed until convenient is a concealment with a publication date."},
                  {"key":"blame","label":"Issue the correction noting it was 'a presenter's error, not the programme's assessment'","quality":15,
                   "consequence":"Technically true, publicly cowardly: the sentence throws one mortified junior under the campaign group's wheels, and your best people quietly stop volunteering to present.",
                   "principle":"The organisation that distances itself from its junior's slip teaches everyone what backing means here."}]}],
             "hints":["The clip is already moving — your correction competes on speed, not elegance.",
               "Institutional ownership ('our presentation') costs nothing and protects everything.",
               "How you treat the mistaken junior is watched by everyone who might present next."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Outran a viral wrong number with a same-day, institution-owned correction."}
            """),

        ("WC-COM-232", "The retro that drew blood", "Anonymous feedback named a name. The room is waiting to see what you do.",
            "Energy Networks", "Delivery Team Lead", "project_management", "professional", 5,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"Your team's quarterly retrospective uses anonymous input cards. Today's batch includes one that names your commissioning engineer: 'X hoards information and makes everyone beg for handover data — biggest blocker on this team.' It is harsh, personal — and, you privately suspect, partly true. The team saw you read it. X is in the room.",
             "evidence":[
               {"label":"The card","value":"Names X: 'hoards information… biggest blocker'"},
               {"label":"Truth content","value":"Partly true, in your judgment"},
               {"label":"Format","value":"Anonymous input, read live, team watching"},
               {"label":"X","value":"In the room"}],
             "decisions":[
               {"key":"room","prompt":"In the moment, you:",
                "options":[
                  {"key":"depersonalise","label":"Depersonalise live, keep the issue: 'Named feedback doesn't belong in anonymous cards — but information flow at handover is a real theme; let's work THAT as a process problem now' — then talk to X privately this week about the underlying pattern","quality":100,
                   "consequence":"The room sees the norm defended AND the issue kept; the process discussion surfaces two fixes X actually supports, and the private conversation lands because it wasn't a public trial.",
                   "principle":"Protect people in public, address patterns in private, and never let a norm violation kill a true signal — three moves, one moment."},
                  {"key":"read_on","label":"Address it as written — the feedback is data and X is an adult","quality":10,
                   "consequence":"X endures a public critique via anonymous accusation; the retro format dies that day — next quarter's cards are all blank, because everyone watched what candour costs.",
                   "principle":"Processing an anonymous attack live teaches the team that retros are ambush venues."},
                  {"key":"suppress","label":"Skip the card entirely — anonymous personal attacks get no airtime","quality":25,
                   "consequence":"The norm is defended and the signal buried; the handover bottleneck — real — continues, and whoever wrote the card concludes feedback goes nowhere.",
                   "principle":"Suppressing the venue without harvesting the truth loses the half that mattered."}]}],
             "hints":["Two things are true at once: the format was violated AND the signal may be real.",
               "The room is learning the retro's rules from your next sentence.",
               "The person conversation happens — later, privately, about the pattern."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Defended the retro's rules and rescued the true signal inside a personal attack."}
            """),

        ("WC-COM-233", "Present in the room, absent from the decision", "Hybrid meetings work fine — for the people in the building.",
            "Technology Programmes", "Programme Delivery Manager", "project_management", "professional", 6,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"Your platform programme's core team is hybrid: six in the office, five remote (two other cities, one other timezone). You notice a pattern: the last four significant decisions were effectively made in post-meeting huddles — the office six talking at the whiteboard after the call ended. Remote members learn outcomes from the notes. Two of your strongest engineers are remote; one has started saying 'whatever the office decides' in planning.",
             "evidence":[
               {"label":"Pattern","value":"4 recent decisions made in post-call office huddles"},
               {"label":"Effect","value":"Remote members reading decisions in the notes"},
               {"label":"Signal","value":"'Whatever the office decides' from a strong remote engineer"},
               {"label":"Team","value":"6 office, 5 remote incl. 1 other timezone"}],
             "decisions":[
               {"key":"fix","prompt":"You:",
                "options":[
                  {"key":"structural","label":"Change the decision mechanics, not the exhortations: decisions of consequence get made IN the documented channel (proposal written, comments async, decision recorded) — the huddle can discuss but the channel decides; and you visibly route your OWN next two decisions that way","quality":100,
                   "consequence":"The whiteboard huddles keep happening — as discussions; the decisions move to where all eleven can shape them, and the 'whatever the office decides' engineer ships the quarter's best design proposal three weeks later.",
                   "principle":"Hybrid equity is an architecture, not an attitude — put the decision where the whole team is, and let the room be just a room."},
                  {"key":"exhort","label":"Raise it at the next team meeting: huddle decisions must stop, everyone deserves a voice","quality":20,
                   "consequence":"Everyone agrees warmly; the next huddle happens anyway — organic conversation doesn't obey memos, and the remote five watch the gap between speech and structure widen.",
                   "principle":"Norms announced without mechanics are wishes — the huddle is natural; only the decision's HOME can change."},
                  {"key":"office","label":"Mandate office days for decision-heavy phases — co-location fixes what process can't","quality":15,
                   "consequence":"The two other cities can't comply and the other timezone can't exist differently; the mandate reads as 'remote careers end here', and the strongest remote engineer's next call is with a recruiter.",
                   "principle":"Solving hybrid friction by unwinding hybrid selects for proximity over talent."}]}],
             "hints":["The huddle isn't the problem — the decision living there is.",
               "Fix the mechanics (where decisions are made) before the manners (who talks in meetings).",
               "Model it yourself first; teams copy routing, not requests."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Moved decisions from the whiteboard huddle to where all eleven people are."}
            """),

        ("WC-COM-234", "Two executives, one team, opposite orders", "The CFO says freeze spend. The COO says accelerate. Your team holds both emails.",
            "Enterprise Programmes", "Workstream Lead", "project_management", "professional", 7,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"Monday morning: the CFO's office emails all programme teams to freeze discretionary spend pending quarter-end review. Monday afternoon: the COO — your programme's sponsor — emails your team directly to 'accelerate the warehouse rollout, whatever it takes'. Both are genuine instructions from people who outrank everyone you can easily reach. Your team is asking which email to obey; procurement has two POs waiting.",
             "evidence":[
               {"label":"Instruction 1","value":"CFO: freeze discretionary spend (all programmes)"},
               {"label":"Instruction 2","value":"COO/sponsor: 'accelerate, whatever it takes' (your team, directly)"},
               {"label":"Pending","value":"2 POs with procurement"},
               {"label":"Team","value":"Asking which to obey"}],
             "decisions":[
               {"key":"conflict","prompt":"You:",
                "options":[
                  {"key":"surface","label":"Force the collision upward TODAY, in writing to both offices: 'These instructions conflict for these 2 POs — we hold both until you align; here is the cost of each day held' — while your team continues all non-spend acceleration","quality":100,
                   "consequence":"The two offices — genuinely unaware of the collision — align within a day (rollout exempted from the freeze, in writing); your team never had to gamble on which executive to disobey.",
                   "principle":"Never resolve your superiors' contradiction by guessing — surface it to its owners, priced, and keep moving on everything uncontradicted."},
                  {"key":"sponsor","label":"Follow the COO — they're your sponsor and the more specific instruction wins","quality":15,
                   "consequence":"Defensible logic, discovered at quarter-end review as 'the team that ignored the freeze'; the CFO's office doesn't process 'specificity doctrine' as an excuse.",
                   "principle":"Picking a winner between executives makes YOU the author of the choice — and its consequences."},
                  {"key":"freeze","label":"Follow the CFO — finance instructions trump line instructions, always","quality":15,
                   "consequence":"Equally defensible, symmetrical outcome: the sponsor finds their direct instruction ignored and their rollout stalled by a subordinate's doctrine of precedence.",
                   "principle":"Both 'safe' choices are bets; the only non-bet is making the owners collide."}]}],
             "hints":["The contradiction belongs to its authors — deliver it back to them, together.",
               "Price the delay per day; executives align faster around numbers.",
               "Separate what's actually contradicted (2 POs) from what isn't (everything else) — keep the rest moving."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Governance Steward"},
             "share_line":"Returned two contradictory executive orders to their owners, priced by the day."}
            """),

        ("WC-COM-235", "Slide forty-one, someone else's logo", "Your team's analysis just won the steering group's applause — presented by another department.",
            "Enterprise Programmes", "Analysis Team Lead", "project_management", "professional", 5,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"At programme steering, the transformation office presented 'their' options analysis for the logistics decision — nine slides your team built last month, shared 'for input', now carrying the transformation office's branding and no attribution. The analysis won the room. Your two analysts who built it were watching on the call. The transformation office's director is a peer you work with weekly.",
             "evidence":[
               {"label":"The work","value":"9 slides, your team's analysis, shared 'for input'"},
               {"label":"The presentation","value":"Rebranded, unattributed, well received"},
               {"label":"Witnesses","value":"Your two analysts, on the call"},
               {"label":"Relationship","value":"Peer director, weekly working contact"}],
             "decisions":[
               {"key":"respond","prompt":"You:",
                "options":[
                  {"key":"direct_private","label":"Raise it with the peer directly and today — assume sloppiness before theft ('your team presented our analysis unattributed; fix the record with the steering secretariat') — and tell your analysts, before they ask, exactly what you've done about it","quality":100,
                   "consequence":"The peer, embarrassed, corrects the attribution in the minutes and credits the team at the next steering; your analysts learn their lead defends their work at the speed it gets taken.",
                   "principle":"Credit theft is corrected peer-to-peer, fast and factually — and your team must SEE the correction happen, or they'll stop showing you their best work."},
                  {"key":"escalate","label":"Raise it with the programme director — attribution is a governance matter","quality":20,
                   "consequence":"The director shrugs it into 'sort it between yourselves'; you've spent an escalation on something a direct conversation would have fixed, and the peer now knows you go upstairs first.",
                   "principle":"Escalating what a peer conversation can fix converts a correction into a feud."},
                  {"key":"let_go","label":"Let it pass — the programme benefited and internal credit games are beneath the work","quality":10,
                   "consequence":"Magnanimous, and observed: your analysts saw their work taken and their lead silent; the next brilliant analysis stays in a drawer until 'it's protected'.",
                   "principle":"Uncorrected credit theft is a tax on your team's future openness — the audience for your response is them, not the peer."}]}],
             "hints":["Assume incompetence before malice — but correct the record either way.",
               "Your primary audience is your own team watching what you do.",
               "The correction wants to be factual and boring, not dramatic."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Got a stolen analysis re-attributed within a day — where the team could see it."}
            """),

        ("WC-COM-236", "The talk nobody hears anymore", "Same briefing, every morning, word for word. Compliance is total. Attention is zero.",
            "Construction", "HSE & Communications Lead", "project_management", "professional", 6,
            """["stakeholder_communication","leadership"]""",
            """
            {"context":"Your viaduct project's daily pre-start briefing has calcified: the same supervisor reads the same generic script ('watch out for plant movements, hydrate, report hazards') every morning to a yard of workers checking phones. Attendance: 100%. Attention: none. This week's actual hazards — a new crane radius, a reversed traffic route, live cable pulls — were technically 'covered' by the generic words. An audit would pass it. A near-miss wouldn't.",
             "evidence":[
               {"label":"Format","value":"Same generic script daily; supervisor reads, yard scrolls"},
               {"label":"Compliance","value":"100% attendance, signed"},
               {"label":"This week's reality","value":"New crane radius · reversed traffic route · live cable pulls"},
               {"label":"Status","value":"Audit-proof and attention-free"}],
             "decisions":[
               {"key":"revive","prompt":"You:",
                "options":[
                  {"key":"specific","label":"Rebuild around TODAY's delta: two minutes, only what changed since yesterday (the crane radius, the route, the pulls), delivered at the work front by the relevant chargehand with one check-question — the generic content moves to induction where it belongs","quality":100,
                   "consequence":"Briefings drop from ten minutes to three and the yard starts listening — because the content is finally news; the cable-pull exclusion question gets a wrong answer on day 2, corrected on the spot, which is the system working.",
                   "principle":"Attention follows information content — brief the delta, at the front, from the person who owns it, and test one answer."},
                  {"key":"enforce","label":"Ban phones at briefings and require eye contact — attention is a discipline matter","quality":10,
                   "consequence":"Phones vanish, eyes point forward, and the same generic words wash over a now-resentful yard; the crane radius still surprises someone Thursday.",
                   "principle":"Compelled attention to content-free communication produces theatre, not safety."},
                  {"key":"materials","label":"Invest in better materials — visual boards, videos, translated cards","quality":30,
                   "consequence":"Handsome boards display the same generic hazards in four languages; production values rose, information content didn't.",
                   "principle":"Presentation upgrades to a stale message polish the staleness."}]}],
             "hints":["Ask what in the briefing was NEW today — that fraction is its whole value.",
               "Move the briefing to where the hazard is; abstraction is the enemy of attention.",
               "One check-question converts listeners from audience to participants."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Cut a dead ten-minute briefing to three live minutes of what actually changed."}
            """),

        // ───────────── August · Schedule Strategy · resources & leadership · order/rank ─────────────

        ("WC-RES-237", "Eight starters, one absorbing team", "The reinforcements arrive Monday. Sequenced wrong, they'll slow you down for a quarter.",
            "Enterprise Programmes", "Delivery Capability Lead", "project_management", "professional", 9,
            """["resource_management","leadership"]""",
            """
            {"context":"Your scaling programme receives eight new hires on Monday — two senior engineers, four mid-level, two juniors — into a delivery team of twelve already at full stretch. Onboarding capacity: each existing senior can properly mentor one starter at a time (there are four seniors). The instinct from above: 'get them all productive immediately, we hired them for the backlog'.",
             "evidence":[
               {"label":"Incoming","value":"2 senior · 4 mid · 2 junior, all Monday"},
               {"label":"Absorbers","value":"4 existing seniors, 1 mentee each properly"},
               {"label":"Team state","value":"12 people, fully stretched"},
               {"label":"Pressure","value":"'All productive immediately'"}],
             "decisions":[
               {"key":"sequence","prompt":"Your onboarding sequence?",
                "options":[
                  {"key":"waves","label":"Sequence by absorption arithmetic: wave 1 = the 2 senior hires (near-self-sufficient, and they become mentors in 4 weeks, doubling capacity) + 2 mids; wave 2 (week 4) = remaining mids + juniors, mentored partly by wave-1 seniors — with waves 2's start dates moved formally, not left ambiguous","quality":100,
                   "consequence":"Wave 1 lands cleanly; by week five there are six mentors, and the full eight are genuinely productive by week nine — faster than the all-at-once plan's honest trajectory, with zero mentor burnout.",
                   "principle":"Onboarding is constrained by absorbers, not desks — sequence arrivals to GROW absorption capacity, seniors first."},
                  {"key":"all","label":"All eight Monday as demanded — spread them across the team and let osmosis work","quality":10,
                   "consequence":"Four seniors try to mentor eight people while delivering; velocity DROPS for six weeks, two starters flounder invisibly, and one junior resigns in month three citing 'no support'.",
                   "principle":"Overloading the absorbers converts reinforcements into drag — the backlog gets slower service from twenty people than it had from twelve."},
                  {"key":"selfserve","label":"All eight Monday with a self-service onboarding pack instead of mentors","quality":20,
                   "consequence":"The pack is good; the tacit knowledge (which tests lie, whose approval matters, where the bodies are buried) isn't in it, and the mids build confidently on wrong assumptions.",
                   "principle":"Documentation onboards people to the explicit 20% — the mentored 80% is why onboarding takes people."}]}],
             "hints":["Count the absorbers, not the desks — mentoring capacity is the constraint.",
               "Sequence to grow the constraint: who becomes a mentor soonest?",
               "Formalise the staggered start; ambiguity reads as chaos to people who just joined."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced eight starters by mentor arithmetic and beat the all-at-once plan by weeks."}
            """),

        ("WC-RES-238", "The stalled front's crews", "Sixty operatives, no work face, four other fronts begging. Reallocate — in the right order.",
            "Construction", "Construction Delivery Manager", "project_management", "professional", 10,
            """["resource_management","schedule_analysis"]""",
            """
            {"context":"A design query has stalled your viaduct project's north abutment for an estimated three weeks — idling 60 operatives (piling gang, steel-fixers, formwork carpenters, general operatives). Four other fronts want them: the south abutment (can absorb steel-fixers now, carpenters in a week), the access structures (general operatives, immediately), a neighbouring project (anything, but 2-week minimum commitment), and 'hold some for restart' (the site agent's plea — remobilising scattered crews historically takes a fortnight).",
             "evidence":[
               {"label":"Idle","value":"60 operatives: piling · steel-fixing · formwork · general"},
               {"label":"Stall","value":"~3 weeks (design query)"},
               {"label":"Demands","value":"South abutment (partial fit) · access works (general) · other project (2wk min) · hold-for-restart"},
               {"label":"History","value":"Remobilising scattered crews ≈ 2 weeks"}],
             "decisions":[
               {"key":"allocate","prompt":"Your allocation sequence?",
                "options":[
                  {"key":"fit_and_restart","label":"Match by trade AND protect the restart: steel-fixers to south now, general operatives to access works, carpenters follow south in a week — the PILING gang (needed first at restart, hardest to re-source) stays on productive standby prep (maintenance, pre-fabrication) on site; nobody goes to the 2-week external commitment given a 3-week stall estimate's error bars","quality":100,
                   "consequence":"Three fronts accelerate; when the design query clears in 2.5 weeks, the piling gang restarts the abutment within two days — the fortnight remobilisation that killed the last stall never happens.",
                   "principle":"Reallocate idle crews by trade fit — but ring-fence the restart's critical first trade, and never lend crews for longer than the stall's LOWER error bar."},
                  {"key":"everyone","label":"Deploy everyone to whoever can use them, external project included — idle hours are pure waste","quality":15,
                   "consequence":"All 60 are productive somewhere within days; the query clears early and the restart waits eleven days for the piling gang to unwind from the neighbour's 2-week commitment.",
                   "principle":"Zero idle time today at the cost of a stalled restart is efficiency theatre — the critical path pays for the utilisation chart."},
                  {"key":"hold","label":"Hold everyone on site — three weeks isn't long and scattered crews never come back right","quality":20,
                   "consequence":"The restart is instant and immaculate; 180 crew-weeks of paid standby preceded it, and three other fronts slipped for want of trades that sat visible and idle.",
                   "principle":"Protecting the restart by freezing everything pays restart insurance at a rate nobody would quote."}]}],
             "hints":["Sort the crews by trade fit against the demands — not everyone is interchangeable.",
               "Identify which trade the RESTART needs first; that one has a different calculus.",
               "Lend nothing for longer than the stall's optimistic estimate — stalls end early sometimes."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reallocated a stalled front's crews by trade — and kept the restart's key gang ready."}
            """),

        ("WC-RES-239", "Five asks, one analyst", "Everyone's request is urgent. Her week is still forty hours.",
            "Enterprise Programmes", "PMO Manager", "project_management", "professional", 8,
            """["resource_management","prioritization"]""",
            """
            {"context":"Your programme's only data analyst has five requests for the same week: (1) the sponsor wants a benefits dashboard refresh for a board meeting Thursday; (2) delivery needs defect-trend analysis for a go/no-go Friday; (3) finance wants cost-model support 'this month'; (4) an auditor has asked for data extracts, deadline in two weeks; (5) her own overdue automation work that would halve future extract time. Each requester believes theirs is first.",
             "evidence":[
               {"label":"1 Sponsor","value":"Dashboard for Thursday's board"},
               {"label":"2 Delivery","value":"Defect trends for Friday go/no-go"},
               {"label":"3 Finance","value":"'This month'"},
               {"label":"4 Auditor","value":"Extracts, 2-week deadline"},
               {"label":"5 Her own","value":"Automation halving future extract effort"}],
             "decisions":[
               {"key":"rank","prompt":"The week's ranking?",
                "options":[
                  {"key":"decision_dated","label":"Rank by decision-date and consequence: (2) go/no-go first — an irreversible decision feeds on it; (1) board dashboard second; then start (5) the automation THIS week precisely because (4)'s audit extracts land easier with it; (3) finance gets a scheduled slot next week — each requester told the ranking and why, once, together","quality":100,
                   "consequence":"Both decision-fed deliverables land; the automation, started early, turns the audit request from three days' work into four hours — and the published ranking logic means next week's five requests arrive pre-triaged.",
                   "principle":"Rank single-resource demand by what DECISIONS consume it and when — and notice when investing in the tool beats serving the queue."},
                  {"key":"seniority","label":"Sponsor first, always — then take the rest in seniority order","quality":15,
                   "consequence":"The dashboard shines Thursday; Friday's go/no-go proceeds on gut feel because the defect analysis wasn't ready, and the decision it produced costs more than every dashboard ever refreshed.",
                   "principle":"Seniority-ranking optimises for who complains, not what the organisation is about to decide."},
                  {"key":"fifo","label":"Strict first-come-first-served — the only defensible neutral rule","quality":10,
                   "consequence":"Finance's month-loose request (first submitted) consumes Monday–Wednesday; both decision deadlines miss, defended by a queue discipline nobody thanked.",
                   "principle":"FIFO is fair to requests and blind to consequences — queues serve tickets, judgment serves outcomes."}]}],
             "hints":["Find which requests feed IRREVERSIBLE decisions, and when those decisions occur.",
               "Check whether any request is really an investment that shrinks the others.",
               "Publish the ranking and its logic — triage transparency is next week's time-saver."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Ranked five urgent asks by decision dates — and funded the tool that shrank the queue."}
            """),

        ("WC-RES-240", "Roster the shutdown, mind the humans", "Ten days, round-the-clock, and fatigue rules that don't bend for optimism.",
            "Energy Networks", "Shutdown Resource Planner", "project_management", "professional", 11,
            """["resource_management","safety_management"]""",
            """
            {"context":"Your substation replacement's 10-day shutdown runs 24/7. Available: 3 authorised senior persons (APs — required for every switching operation, max 12-hour shifts, 11-hour minimum rest by rule), 6 fitter crews (12-hour shift pattern), 2 commissioning engineers (needed heavily in days 8–10). Your first-draft roster has AP coverage failing on nights 6–7 (leave commitments) and both commissioning engineers working 16 days straight if testing slips. The plan review is tomorrow.",
             "evidence":[
               {"label":"APs","value":"3 total · every switching op needs one · 12h max, 11h rest"},
               {"label":"Gap","value":"AP night cover fails nights 6–7"},
               {"label":"Commissioning","value":"2 engineers, heavy days 8–10, 16 straight days if slip"},
               {"label":"Review","value":"Tomorrow"}],
             "decisions":[
               {"key":"roster","prompt":"Your roster strategy?",
                "options":[
                  {"key":"constraint_first","label":"Build the roster around the LEGAL constraint first: schedule all switching operations into AP-covered windows (nights 6–7 get NO switching — plan those nights' work to need none), borrow a 4th AP from a sister project for contingency only, and pre-plan the commissioning slip scenario with a rest-protected handover split between the two engineers","quality":100,
                   "consequence":"The switching plan bends to the AP calendar instead of hoping; nights 6–7 run mechanical-only smoothly, and when testing does slip a day, the pre-split commissioning pattern absorbs it without anyone's 16th consecutive day.",
                   "principle":"Roster from the hardest constraint outward — the work plan serves the fatigue rules, because the alternative is the fatigue rules failing at 3am."},
                  {"key":"hope","label":"Roster as drafted and manage nights 6–7 'dynamically' if switching is needed","quality":5,
                   "consequence":"Night 6 needs an unplanned isolation; the nearest rested AP is four hours away, the shutdown holds for a shift, and the dynamic management is a phone tree of tired people.",
                   "principle":"'Manage it dynamically' is roster language for 'the gap is now the night shift's problem'."},
                  {"key":"extend","label":"Ask the APs to flex their hours for the two nights — they're professionals and it's two nights","quality":10,
                   "consequence":"Two APs agree because professionals do; the rule existed because switching errors cluster in hour 13, and the near-miss report writes itself.",
                   "principle":"Fatigue limits that flex under schedule pressure aren't limits — they're suggestions with consequences."}]}],
             "hints":["Identify the constraint with legal force — the roster is built outward from it.",
               "If coverage can't move to the work, move the WORK to the coverage.",
               "Pre-plan the slip scenario now; day 8 is too late to invent rest-compliant patterns."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Governance Steward"},
             "share_line":"Rostered a 10-day shutdown from its legal constraints outward — and the slip never broke it."}
            """),

        ("WC-RES-241", "One squad, two releases", "Splitting the team is wrong. Not splitting the team is wrong. Choose the right wrong.",
            "Technology Programmes", "Engineering Delivery Lead", "project_management", "professional", 12,
            """["resource_management","leadership"]""",
            """
            {"context":"Your 9-person platform squad must deliver two releases in the same six-week window: the regulatory reporting release (fixed statutory date, moderate complexity, well-understood) and the customer-portal release (commercially promised, higher complexity, novel work). The squad works best together; splitting it breaks pairing and review chains. Not splitting it means sequencing — and one release's window doesn't fit behind the other's.",
             "evidence":[
               {"label":"Release A","value":"Regulatory — statutory date, moderate, well-understood"},
               {"label":"Release B","value":"Portal — promised, complex, novel"},
               {"label":"Squad","value":"9 people, strong pairing/review culture"},
               {"label":"Arithmetic","value":"Sequential doesn't fit; parallel means splitting"}],
             "decisions":[
               {"key":"split","prompt":"Your structure for the six weeks?",
                "options":[
                  {"key":"asymmetric","label":"Split ASYMMETRICALLY by work nature: 3 people (including one senior) take the well-understood regulatory release as a tight sub-team; 6 keep the novel portal work with the pairing culture intact — with one shared daily sync and the explicit promise the split dies at week six","quality":100,
                   "consequence":"The regulatory three run a known playbook without needing the full squad's creativity; the portal six keep the collaborative density the novel work actually requires, and both releases land — the temporary structure dissolving on schedule.",
                   "principle":"Split by what the work needs, not by fairness: routine work travels in small teams; novel work is what the culture was built for."},
                  {"key":"even","label":"Split 5/4 evenly — both releases matter, both get half the squad","quality":20,
                   "consequence":"The portal's novel work loses critical mass for its design debates while the regulatory release carries more people than its playbook needs; even was equitable and wrong for both.",
                   "principle":"Symmetric splits of asymmetric work shortchange the hard half and pad the easy one."},
                  {"key":"hero","label":"Keep the squad whole on the portal; give the regulatory release to two contractors","quality":15,
                   "consequence":"The contractors, new to the codebase, take three weeks to move safely; the statutory date arrives with the release at 70% and no one inside the squad fluent in its state.",
                   "principle":"Statutory dates are the last place for onboarding risk — outsource novel exploration never, routine-but-critical rarely."}]}],
             "hints":["Classify each release: playbook work or discovery work? They need different structures.",
               "Protect collaborative density where the problems are unsolved.",
               "Make the split explicitly temporary — reversibility is what makes it acceptable."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Split a squad asymmetrically — playbook three, discovery six — and landed both releases."}
            """),

        ("WC-RES-242", "Train the trainers first", "Four hundred users, six weeks, and a training team of three.",
            "Enterprise Programmes", "Business Change Manager", "project_management", "professional", 9,
            """["resource_management","change_management"]""",
            """
            {"context":"Your CRM rollout needs 400 users trained across five departments in six weeks. Your training team: three people. The maths of direct delivery fails (400 users ÷ 12-person sessions ÷ 3 trainers = impossible). Department heads propose their own sequences: sales first (biggest team), or claims first (most change-resistant), or 'whoever's free'. Meanwhile 20 volunteers from the pilot phase know the system well.",
             "evidence":[
               {"label":"Load","value":"400 users, 5 departments, 6 weeks"},
               {"label":"Capacity","value":"3 trainers; direct delivery maths fails"},
               {"label":"Asset","value":"20 pilot-phase power users, system-fluent"},
               {"label":"Politics","value":"Each department claims first slot"}],
             "decisions":[
               {"key":"model","prompt":"Your delivery model and sequence?",
                "options":[
                  {"key":"cascade","label":"Weeks 1–2: the three trainers train the 20 pilot users as departmental trainers (materials, practice sessions, certification); weeks 3–6: 23 trainers cascade to 400, sequenced by GO-LIVE ORDER (not politics) — with the core team floating as quality support","quality":100,
                   "consequence":"The cascade delivers 400 trained users with local trainers who stay after week six as floor-walking support — the thing no central team could ever have provided; go-live order sequencing means no one is trained months before or after they need it.",
                   "principle":"When delivery capacity fails the arithmetic, train multipliers first — and sequence by when knowledge will be USED, not by who lobbies loudest."},
                  {"key":"blitz","label":"Direct delivery, bigger sessions — 40-person lectures fit the maths","quality":15,
                   "consequence":"The arithmetic works and the learning doesn't: 40-person lectures on a hands-on system produce sign-in sheets, not capability, and go-live's helpdesk queue proves it.",
                   "principle":"Scaling class size to fit the calendar trades attendance for competence at 1:1."},
                  {"key":"external","label":"Contract an external training provider to multiply capacity","quality":25,
                   "consequence":"Eight external trainers learn your configuration for two weeks (of six), deliver generic-flavoured sessions, and leave at week six with everything they learned — the pilot users were free and permanent.",
                   "principle":"Renting capacity you could grow internally pays twice: once in fees, once in the departed knowledge."}]}],
             "hints":["When the delivery maths fails, look for the multiplier model.",
               "The 20 pilot users are the asset the org chart doesn't show.",
               "Sequence training by go-live order — knowledge decays from the moment it's taught."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned 3 trainers into 23 and sequenced 400 users by go-live, not by lobbying."}
            """),

        ("WC-RES-243", "Three fronts, two scaffold gangs", "Scaffolding is suddenly the constraint. Sequence it like one.",
            "Construction", "Site Resource Coordinator", "project_management", "professional", 10,
            """["resource_management","schedule_analysis"]""",
            """
            {"context":"Your hospital-wing project has three work fronts needing scaffold work in the same fortnight: the facade (5 days' scaffold adaptation, gates the cladding crew arriving Monday week 2), the plant room (3 days' access scaffold, gates M&E second fix — 12 fitters currently on other work until it's ready), and the atrium (8 days' birdcage, gates ceiling works that have 3 weeks' float). Two scaffold gangs available; a third possible from the supplier in a week at premium rates.",
             "evidence":[
               {"label":"Facade","value":"5 days · gates cladding crew arriving Mon wk2"},
               {"label":"Plant room","value":"3 days · releases 12 fitters to second fix"},
               {"label":"Atrium","value":"8 days · gated work has 3 weeks' float"},
               {"label":"Capacity","value":"2 gangs now · 3rd possible in a week, premium"}],
             "decisions":[
               {"key":"sequence","prompt":"Gang allocation, week one?",
                "options":[
                  {"key":"gated_first","label":"Sequence by what each front RELEASES: gang 1 to the facade (hard external date — the cladding crew's arrival), gang 2 to the plant room first (3 days releases 12 fitters — the biggest labour unlock per scaffold day), then gang 2 joins the atrium; decline the premium third gang — the atrium's float absorbs the sequencing","quality":100,
                   "consequence":"The cladding crew lands on ready scaffold; the fitters mobilise Thursday instead of week 3; the atrium birdcage finishes with float to spare — and the premium gang was never needed because the float was doing its job.",
                   "principle":"Sequence a constrained resource by what each assignment UNLOCKS — external commitments and labour releases first, floated work last, and spend money only when float runs out."},
                  {"key":"biggest","label":"Both gangs to the atrium first — 8 days is the longest job, start the longest first","quality":10,
                   "consequence":"The birdcage finishes early into its three weeks of float; the cladding crew stands down Monday week 2 at full cost, and twelve fitters wait a fortnight for a three-day scaffold.",
                   "principle":"'Longest first' optimises the scaffold programme and wrecks everyone else's — duration is not priority."},
                  {"key":"third","label":"Hire the premium third gang and run all three fronts at once","quality":25,
                   "consequence":"Everything proceeds in parallel at premium cost — solving with money what the float would have solved for free; the atrium's early finish buys nothing that was for sale.",
                   "principle":"Buying capacity to avoid sequencing is only right when no front can wait — check the float before the phone."}]}],
             "hints":["For each front, ask what its scaffold RELEASES and what it costs to wait.",
               "External arrival dates and big labour unlocks outrank internal duration.",
               "Float exists to absorb exactly this — spend it before spending money."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced two scaffold gangs by what each front unlocked — and let float pay for the third."}
            """),

        ("WC-RES-244", "Kill the meetings, keep the decisions", "Thirty-one recurring meetings. The team delivers in the gaps between them.",
            "Enterprise Programmes", "Team Effectiveness Lead", "project_management", "professional", 8,
            """["resource_management","leadership"]""",
            """
            {"context":"A calendar audit of your 15-person delivery team finds 31 recurring meetings consuming 28% of team hours: 4 are decision forums with named authorities; 9 are status meetings that duplicate the dashboard; 6 are 'syncs' between people who sit together; 5 are stakeholder updates with declining attendance; 4 are technical design sessions (well-rated); 3 nobody could explain. The team's ask: 'give us our week back — but don't break anything'.",
             "evidence":[
               {"label":"Load","value":"31 recurring meetings, 28% of team hours"},
               {"label":"Mix","value":"4 decision · 9 status · 6 adjacent-desk syncs · 5 fading updates · 4 valued design · 3 unexplained"},
               {"label":"Constraint","value":"'Don't break anything'"},
               {"label":"Instrument","value":"Dashboard already duplicates the status content"}],
             "decisions":[
               {"key":"cull","prompt":"The cull order?",
                "options":[
                  {"key":"by_function","label":"Cull by FUNCTION, protect by function: the 3 unexplained die today; the 9 status meetings collapse into the dashboard with one weekly exceptions-only slot; the 6 syncs are cancelled (the desks are adjacent); the 5 updates merge to 2 with fresh formats; the 4 decision forums and 4 design sessions — the ones doing irreplaceable work — remain untouched. Review in 6 weeks for regrowth","quality":100,
                   "consequence":"Team hours recovered: ~19% — with zero broken decisions, because the cull never touched a meeting that DECIDED or DESIGNED anything; the six-week review catches two status meetings quietly regrowing and kills them again.",
                   "principle":"Meetings are killed by function, not by count: anything that merely TRANSFERS information dies into a tool; anything that DECIDES or CREATES survives on merit."},
                  {"key":"halve","label":"Mandate a 50% cut and let each meeting owner defend their slot","quality":20,
                   "consequence":"The best-defended meetings survive — which selects for owners' attachment, not function; a decision forum dies to a well-argued status meeting, and the exercise is remembered as theatre.",
                   "principle":"Uniform cut targets select for advocacy; function tests select for value."},
                  {"key":"gentle","label":"Shorten everything to 25 minutes instead of cancelling — less disruptive","quality":15,
                   "consequence":"31 meetings still fragment the week into confetti; the 28% falls to 21%, and the context-switching cost — the real tax — doesn't fall at all.",
                   "principle":"The cost of a meeting-heavy week is fragmentation, not just minutes — shortening everything shortens nothing that matters."}]}],
             "hints":["Classify by what each meeting DOES: decide, create, transfer, or nothing.",
               "Information transfer belongs in tools; only deciding and creating need rooms.",
               "Schedule the regrowth review now — culled meetings reseed within a quarter."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Culled 31 meetings by function and gave a team back a fifth of its week — decisions intact."}
            """),

        ("WC-RES-245", "Change the shift, keep the depot", "The new pattern is better on paper. The transition is where depots break.",
            "Transport Operations", "Depot Transition Manager", "project_management", "professional", 11,
            """["resource_management","change_management"]""",
            """
            {"context":"Your depot modernisation includes moving maintenance from a 2-shift to a 3-shift pattern — agreed with the union, better for asset availability, effective in eight weeks. The transition risk: the depot must keep its nightly maintenance quota THROUGH the change, staff need rota re-bidding (seniority rules), 12 fitters need nights training (they've never worked unsupervised nights), and the supervisors' pattern changes a week before everyone else's under the agreement. Sequence the transition.",
             "evidence":[
               {"label":"Change","value":"2-shift → 3-shift in 8 weeks; union-agreed"},
               {"label":"Constraint","value":"Nightly maintenance quota must hold throughout"},
               {"label":"People","value":"Rota re-bid (seniority rules) · 12 fitters night-trained · supervisors move 1 week early"},
               {"label":"Risk","value":"Transitions are where depots miss quotas"}],
             "decisions":[
               {"key":"sequence","prompt":"The transition sequence?",
                "options":[
                  {"key":"dependency","label":"Sequence by dependency and lead time: rota re-bid FIRST (weeks 1–3, it determines who needs night training), night training for the actual night-bidders (weeks 3–7, supervised on existing nights), supervisors transition week 7 (their early week becomes the new pattern's shakedown), full cutover week 8 — with quota tracked nightly and a two-week overlap staffing buffer costed and approved upfront","quality":100,
                   "consequence":"The re-bid surfaces that only 9 of the 12 assumed fitters actually bid nights — caught in week 2 (trainable) instead of week 8 (crisis); the supervisors' shakedown week catches two handover-process gaps, and the quota never dips.",
                   "principle":"Sequence people transitions by information dependency — the re-bid tells you who to train, so it goes first, however administratively dull it looks."},
                  {"key":"train_first","label":"Start night training immediately — it's the longest task, so it starts first","quality":20,
                   "consequence":"Twelve assumed fitters train for five weeks; the re-bid then assigns three of them to days (seniority), and three untrained fitters to nights — the longest task ran first and trained partly the wrong people.",
                   "principle":"'Longest first' fails when an upstream decision determines WHO the task applies to."},
                  {"key":"bigbang","label":"Do everything in the final fortnight — short transitions minimise the disruption window","quality":10,
                   "consequence":"Re-bid, training and cutover compress into two weeks; the quota drops 30% for a month, and the union files a dispute about training adequacy that the eight-week plan existed to avoid.",
                   "principle":"Compressing a people-transition compresses its learning, not its risk."}]}],
             "hints":["Find the step whose OUTPUT other steps consume — the re-bid names the trainees.",
               "Use the supervisors' early week as deliberate shakedown, not accidental oddity.",
               "The quota is the invariant: staff the overlap, don't hope through it."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced a depot shift change by information dependency — and never missed a nightly quota."}
            """),

        ("WC-RES-246", "The matrix nobody staffed", "Maintenance lends the project its people. The plant still has to run.",
            "Industrial Manufacturing", "Site Resource Planner", "project_management", "professional", 12,
            """["resource_management","stakeholder_communication"]""",
            """
            {"context":"Your line-upgrade project draws its electricians and instrument techs from the plant's maintenance department — a matrix arrangement agreed 'in principle' at kickoff. Reality, month 3: the maintenance manager pulls people back for every breakdown (rightly — production pays everyone's wages), project tasks restart constantly, and your schedule has quietly slipped 3 weeks through a hundred small withdrawals. Both of you report to the site director, who 'expects you to work it out'.",
             "evidence":[
               {"label":"Arrangement","value":"Matrix — project borrows maintenance trades, agreed 'in principle'"},
               {"label":"Reality","value":"Constant recalls for breakdowns; tasks restart repeatedly"},
               {"label":"Damage","value":"3 weeks' slip via a hundred small withdrawals"},
               {"label":"Governance","value":"Shared boss 'expects you to work it out'"}],
             "decisions":[
               {"key":"structure","prompt":"Your proposal to the maintenance manager?",
                "options":[
                  {"key":"protected_blocks","label":"Replace the always-borrowable pool with PROTECTED BLOCKS: named people, agreed 4-hour minimum blocks, a jointly-held escalation rule for genuine plant emergencies (defined: line-down, not routine), and a visible shared dashboard of withdrawals — so the cost of each recall is counted, not felt","quality":100,
                   "consequence":"Recalls drop 70% — most 'breakdowns' turn out to be routine work that waits four hours perfectly well; the genuine line-down emergencies still get instant response, and the slip stabilises then recovers.",
                   "principle":"Matrix resourcing fails through unpriced interruptions — protect blocks, define 'emergency', and make every withdrawal visible and countable."},
                  {"key":"escalate","label":"Take the 3-week slip to the site director for a ruling on priority","quality":20,
                   "consequence":"The director rules 'production first, obviously' — now formalised AGAINST the project; the 'work it out' instruction was the offer of a better deal than any ruling would be.",
                   "principle":"Escalating a structural problem to a boss who said 'work it out' buys a worse structure with an audience."},
                  {"key":"dedicate","label":"Demand dedicated project electricians — hire or transfer, end the matrix","quality":25,
                   "consequence":"The business case for six dedicated trades on a five-month project fails in finance; three months of requisition argument later, the matrix is unchanged and unimproved.",
                   "principle":"Demanding the org-chart solution to a working-agreement problem swaps a fixable friction for an unwinnable budget fight."}]}],
             "hints":["Count the interruptions — a hundred small withdrawals is a structure, not bad luck.",
               "Define 'emergency' jointly; most recalls fail their own definition.",
               "Make the borrowing visible on a shared instrument — counted costs behave differently from felt ones."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Executive Communicator"},
             "share_line":"Fixed a leaking matrix with protected blocks and a definition of 'emergency'."}
            """),

        ("WC-SCH-247", "The nine days before the possession", "Everything must be ready by Friday week. Order the readiness work backwards.",
            "Rail Infrastructure", "Possession Readiness Planner", "project_controls", "professional", 9,
            """["schedule_analysis","sequencing"]""",
            """
            {"context":"Your junction-renewal possession starts in nine days. Outstanding readiness items: materials to site compound (3 days' delivery lead), plant acceptance inspections (1 day, needs the plant on site), possession staff briefings (must be within 5 days of the work by rule), the practice lift for the crane move (needs the crane accepted and materials present), the method-statement briefing cascade (after any practice-lift learnings), and welfare setup (2 days, independent). The team is treating it as a checklist. It is a network.",
             "evidence":[
               {"label":"Items","value":"Materials (3d lead) · plant acceptance (needs plant) · briefings (within 5d of work) · practice lift (needs crane+materials) · MS cascade (after practice lift) · welfare (2d, independent)"},
               {"label":"Clock","value":"9 days to possession"},
               {"label":"Team habit","value":"Treating it as a checklist, not a network"}],
             "decisions":[
               {"key":"order","prompt":"The backward-passed order?",
                "options":[
                  {"key":"network","label":"Chain it: materials ordered TODAY (3-day lead) → plant arrives and is accepted days 3–4 → practice lift day 5 → method statements updated with its learnings day 6 → briefing cascade days 6–8 (inside the 5-day rule) → welfare runs parallel days 1–2. The practice lift is the pivot — everything upstream feeds it, everything downstream learns from it","quality":100,
                   "consequence":"The practice lift on day 5 finds the crane mat position fouls a signal cable route — corrected in the method statement everyone is then briefed on; the possession's actual lift takes 40 minutes, boring and right.",
                   "principle":"Readiness is a dependency network wearing a checklist's clothes — find the pivot item that both consumes and produces, and hang the calendar on it."},
                  {"key":"checklist","label":"Work the checklist by owner availability — everything done by day 9 is success","quality":10,
                   "consequence":"Briefings happen day 2 (owner was free) — outside the 5-day rule, so they repeat day 8, now WITHOUT practice-lift learnings that arrived day 7 late because materials ordered day 4; three items done twice, one rule broken.",
                   "principle":"Checklists hide sequence errors until the items collide — by-availability ordering is collision scheduling."},
                  {"key":"buffer","label":"Compress everything into days 6–9 so information is freshest for the possession","quality":15,
                   "consequence":"Freshness achieved, margin destroyed: the materials delivery slips one day (they do) and the whole compressed stack lands on the possession weekend itself.",
                   "principle":"Back-loading for freshness deletes the recovery time that front-loading exists to buy."}]}],
             "hints":["Find the item with the longest lead — its clock started yesterday.",
               "Find the pivot: what both needs several inputs AND changes what follows?",
               "Rules with time windows (briefings) are placed, not slotted."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a possession-readiness checklist back into the network it always was."}
            """),

        // ───────────── August · governance & quality dailies · practitioner ─────────────

        ("WC-GOV-248", "Decided, says the corridor", "Half the programme's decisions have no home, no record and no owner.",
            "Enterprise Programmes", "Governance Manager", "project_management", "professional", 7,
            """["governance","decision_quality"]""",
            """
            {"context":"Preparing for a gateway review, you try to reconstruct how five significant decisions were made this quarter — the data-residency choice, the vendor short-list cut, the phase-2 descope, the test-environment budget, the go-live criteria change. Findings: two live in meeting minutes (different meetings, different attendees), one in an email thread, one 'was agreed with the sponsor' (no record), one nobody can source at all. All five are being acted on.",
             "evidence":[
               {"label":"Reconstruction","value":"5 significant decisions: 2 in scattered minutes · 1 in email · 1 unrecorded 'sponsor agreement' · 1 unsourceable"},
               {"label":"Status","value":"All five being acted on"},
               {"label":"Trigger","value":"Gateway review imminent"},
               {"label":"Culture","value":"Fast-moving, informal, allergic to bureaucracy"}],
             "decisions":[
               {"key":"fix","prompt":"Your fix, sized for a bureaucracy-allergic culture?",
                "options":[
                  {"key":"log_light","label":"A single lightweight decision log — one line per decision: what, who, when, where recorded — populated backwards for the five NOW (getting the unsourced one actually re-decided), maintained forward as a 5-minute weekly discipline; no new meetings, no templates beyond the one line","quality":100,
                   "consequence":"The backfill exposes that the 'sponsor agreement' descope was never actually agreed — re-decided properly before the gateway asks; the one-line log costs minutes a week and answers every future 'who decided this?' in seconds.",
                   "principle":"Decision governance is an INDEX, not a process — the minimum record that makes decisions findable, owned and real."},
                  {"key":"process","label":"Introduce a decision-paper template and approval workflow for all significant decisions","quality":15,
                   "consequence":"The bureaucracy-allergic culture routes around the workflow within a month — decisions now happen in corridors AND avoid the record deliberately; the cure taught evasion.",
                   "principle":"Over-weighted governance in a fast culture doesn't slow decisions — it drives them underground."},
                  {"key":"accept","label":"Accept it — fast informal decisions are why the programme moves; the gateway will understand","quality":10,
                   "consequence":"The gateway does not understand; 'nobody can source the descope decision' becomes the review's headline finding, and the imposed remedy is the heavyweight process option one avoided.",
                   "principle":"Ungoverned decision-making eventually gets governed by someone else, on worse terms."}]}],
             "hints":["The problem is findability and ownership, not formality — size the fix to that.",
               "Backfill first: one of the five will turn out never to have been decided at all.",
               "The best control for a fast culture is the one that costs less than the corridor."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Indexed a programme's corridor decisions in one line each — and caught the one that never happened."}
            """),

        ("WC-GOV-249", "Around the architecture board", "The team found a faster route to production. It goes through no reviews at all.",
            "Technology Programmes", "Technical Governance Lead", "project_management", "professional", 7,
            """["governance","quality_management"]""",
            """
            {"context":"Your platform programme's architecture board reviews all production changes — turnaround averaging 12 days, a real drag. You discover a delivery team has been shipping 'configuration changes' through the ops change process (2-day turnaround) — including, last month, a new authentication flow and a data-retention change that are architecture by any definition. Their lead is unapologetic: 'the board is where velocity goes to die; we shipped safely'. They did ship safely. So far.",
             "evidence":[
               {"label":"Bypass","value":"Architecture shipped as 'config' via ops process"},
               {"label":"Examples","value":"New auth flow · data-retention change"},
               {"label":"Their defence","value":"'Board takes 12 days; we shipped safely'"},
               {"label":"Fact","value":"They DID ship safely — and the board IS slow"}],
             "decisions":[
               {"key":"respond","prompt":"You:",
                "options":[
                  {"key":"both","label":"Close the loophole AND fix its cause together: the bypassed changes get retrospective review this week (the retention change has a compliance issue — found, fixed); simultaneously, the board adopts the tiered model the bypass was crying out for — pre-approved patterns ship at ops speed, only genuine novelty gets full review","quality":100,
                   "consequence":"The retention issue is caught before the regulator's audit; the tiered model cuts median review to 3 days, and the bypassing team becomes the new model's loudest advocate — their complaint was correct even though their route wasn't.",
                   "principle":"A governance bypass is two findings: a violation to close and a signal to heed — teams route around friction that exceeds its value; fix both or the next bypass is smarter."},
                  {"key":"enforce","label":"Shut the loophole hard: mandatory review for everything, disciplinary note for the lead","quality":15,
                   "consequence":"The loophole closes; the 12-day queue doubles with re-routed traffic, delivery slows programme-wide, and the next bypass is designed not to be discoverable.",
                   "principle":"Enforcing slow governance harder makes evasion a competence — the queue was the co-author of the crime."},
                  {"key":"bless","label":"Legitimise it — if they shipped safely for months, the ops route evidently works for most changes","quality":10,
                   "consequence":"The retention change's compliance issue — unreviewed — surfaces in the audit; 'we legitimised the bypass' is a sentence the CIO gets to say to the regulator.",
                   "principle":"'It worked so far' is how every unreviewed risk describes itself until it doesn't."}]}],
             "hints":["Audit the bypassed changes first — at least one will justify the review's existence.",
               "Treat the bypass as user research on your governance's cost/value ratio.",
               "Tiered review — patterns fast, novelty deep — is the durable settlement."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Closed an architecture bypass and fixed the 12-day queue that created it."}
            """),

        ("WC-GOV-250", "Skip the gate, just this once", "The sponsor wants definition closed by Friday. The gate review is the only thing in the way.",
            "Construction", "PMO Lead", "project_management", "professional", 5,
            """["governance","decision_quality"]""",
            """
            {"context":"Your depot-redevelopment project's definition gate review is scheduled for Friday week. The sponsor — under pressure to show the board progress — asks you to 'close definition administratively' this Friday instead: 'the review's a formality; the documents are all basically done; we'll do a retrospective gate if anyone asks'. The documents are 80% done. The gate's purpose is testing the 20%.",
             "evidence":[
               {"label":"Ask","value":"Close the gate 'administratively', a week early, review skipped"},
               {"label":"Reason","value":"Sponsor wants board-visible progress"},
               {"label":"Reality","value":"Documents 80% done; the gate exists to test the missing 20%"},
               {"label":"Offer","value":"'Retrospective gate if anyone asks'"}],
             "decisions":[
               {"key":"gate","prompt":"Your response to the sponsor?",
                "options":[
                  {"key":"alternative","label":"Give the sponsor what they NEED without breaking the gate: a board-ready progress statement Friday ('definition substantially complete, gate review [date] confirmed'), the review held as scheduled — offering to compress it to a half-day if the panel agrees — and a plain sentence about what 'administrative closure' would make YOUR signature mean","quality":100,
                   "consequence":"The board hears crisp progress Friday; the gate a week later catches a ground-risk allocation gap in the missing 20% — the sponsor, watching it get fixed pre-contract, never asks for an administrative closure again.",
                   "principle":"Serve the sponsor's real need (visible progress) without serving the request (a hollow gate) — most gate-skipping asks are progress-optics problems wearing governance clothes."},
                  {"key":"comply","label":"Close it as asked — sponsors own their programmes and the retrospective offer covers you","quality":5,
                   "consequence":"The 20% contains the ground-risk gap; it's discovered post-contract at contract prices, and the 'retrospective gate' — never held, naturally — is now the first exhibit in the lessons-learned review.",
                   "principle":"A gate closed administratively certifies nothing except who was willing to close it."},
                  {"key":"refuse","label":"Refuse flatly — gates are gates, and the sponsor should know better","quality":25,
                   "consequence":"The gate survives; the relationship doesn't — a sponsor rebuffed without an alternative escalates over you, and someone more flexible inherits your chair by year-end.",
                   "principle":"Defending governance without solving the need behind the attack wins the battle and loses the appointment."}]}],
             "hints":["Ask what the sponsor actually needs Friday — it's rarely the gate itself.",
               "The 20% undone is precisely what gates exist to examine.",
               "Offer speed (compressed review), never substitution (skipped review)."],
             "profile_map":{"decision":"Governance Steward","balanced":"Executive Communicator"},
             "share_line":"Gave a sponsor Friday's headline without selling Friday's gate."}
            """),

        ("WC-QLT-251", "Audited to a standstill", "Four audits in six weeks, all asking the same questions. The team has stopped working to be audited.",
            "Energy Networks", "Quality & Assurance Manager", "project_management", "professional", 6,
            """["quality_management","governance"]""",
            """
            {"context":"Your grid-connection project has hosted four audits in six weeks: the client's technical audit, your own corporate QA audit, the ISO surveillance visit, and the funder's assurance review. Each consumed 2–4 days of the same key people; overlap between their document requests was ~70%. A fifth (the regulator's) is scheduled next month. The site team's phrase: 'we stop working to be audited about the work we've stopped doing'.",
             "evidence":[
               {"label":"Load","value":"4 audits / 6 weeks · 2–4 days each of key people"},
               {"label":"Overlap","value":"~70% same document requests"},
               {"label":"Incoming","value":"Regulator's audit next month"},
               {"label":"Effect","value":"Delivery time consumed by assurance of delivery"}],
             "decisions":[
               {"key":"fix","prompt":"You:",
                "options":[
                  {"key":"single_pack","label":"Build the evidence architecture once: a maintained assurance pack (the 70% overlap — certs, registers, records — kept current in one indexed repository with read access for auditors), a coordinated forward audit calendar shared with all five bodies, and pre-audit alignment calls offering each auditor the pack FIRST so visits focus on their unique 30%","quality":100,
                   "consequence":"The regulator's audit takes a day and a half instead of four — they arrived having read the pack; two of next year's audits agree to share findings under the calendar, and the team's audit load halves without a single scope reduction.",
                   "principle":"You cannot refuse assurance, but you can architect it: one evidence source, one calendar, and every audit spends its days on what only IT asks."},
                  {"key":"pushback","label":"Escalate the audit burden to the sponsor and ask for two of the five to be waived","quality":15,
                   "consequence":"The client's and funder's audits are contractual, ISO is certification, the regulator is the regulator, and your own QA is your own — the waiver request returns with zero waivers and a note about attitude to assurance.",
                   "principle":"Audit demand from five independent authorities doesn't negotiate — only its COST does."},
                  {"key":"absorb","label":"Assign a full-time audit liaison to shield the team — bodies between auditors and workers","quality":30,
                   "consequence":"The liaison helps logistics and can't answer technical questions — the key people still get pulled in for the substance, now with an extra relay in the middle.",
                   "principle":"A human buffer relocates the disruption; only shared evidence architecture reduces it."}]}],
             "hints":["Measure the overlap — 70% common requests is an architecture opportunity.",
               "Auditors accept ready evidence gladly; their days are costly to them too.",
               "The calendar is leverage: bodies coordinate when someone offers the coordination."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Governance Steward"},
             "share_line":"Halved a project's audit burden with one evidence pack and one shared calendar."}
            """),

        ("WC-QLT-252", "The quality plan from the last job", "Search-and-replace quality: the plan says 'the bridge' in three places. This is a school.",
            "Public Estates", "Quality Reviewer", "project_management", "professional", 7,
            """["quality_management","governance"]""",
            """
            {"context":"Reviewing the definition-stage quality plan your delivery partner submitted for the schools programme, you find it is their standard template lightly edited: 'the bridge' survives in three paragraphs, the inspection regime references NDT welding protocols (there is no structural steel in phase 1), and the roles section names people who left the partner last year. It was signed by their quality director. The plan is contractually required before mobilisation — which everyone wants next month.",
             "evidence":[
               {"label":"Artefacts","value":"'The bridge' ×3 · NDT welding regime (no steel in scope) · departed staff named"},
               {"label":"Signature","value":"Partner's quality director signed it"},
               {"label":"Status","value":"Contractual prerequisite for mobilisation, wanted next month"},
               {"label":"Question","value":"Reject, accept, or something better"}],
             "decisions":[
               {"key":"respond","prompt":"Your response?",
                "options":[
                  {"key":"reject_specific","label":"Reject with a SPECIFIC schedule of deficiencies (the three artefacts plus the real test: 'show us the inspection and test regime for THIS scope — masonry, roofing, M&E in occupied sites'), a resubmission date that protects mobilisation, and a required session where their quality lead walks YOUR team through the resubmission","quality":100,
                   "consequence":"The resubmission is genuinely scope-specific — the walk-through requirement made copy-paste impossible; more usefully, the occupied-sites inspection regime it forced into existence catches two real gaps in month 2.",
                   "principle":"Reject template quality with specifics and a walk-through — a plan someone must EXPLAIN aloud cannot be a search-and-replace."},
                  {"key":"accept_note","label":"Accept it with comments — the errors are cosmetic and mobilisation shouldn't wait on typos","quality":10,
                   "consequence":"'The bridge' was cosmetic; the welding-inspection regime standing in for an occupied-schools regime was not — the plan's first real test (dust control failure near a classroom) finds it silent, because it was written for a bridge.",
                   "principle":"Template artefacts are the visible symptom; the disease is that nobody thought about THIS project's risks."},
                  {"key":"escalate","label":"Escalate to the partner's account director as a professional-standards failure","quality":25,
                   "consequence":"An apology arrives with impressive speed, followed by a corrected plan produced under the same conditions that produced the first one — pressure without the specificity that would force actual thought.",
                   "principle":"Escalating volume without specifying substance gets you a faster template."}]}],
             "hints":["The copy-paste artefacts are evidence of process, not just carelessness.",
               "Specify what a scope-specific plan must demonstrate — occupied sites are the real test here.",
               "Require the plan be presented aloud; explanation is the enemy of boilerplate."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Rejected a bridge's quality plan on behalf of a school — with specifics that forced real thought."}
            """),

        // ───────────── August · Logic & Sequence · practitioner ─────────────

        ("WC-SCO-253", "The festival's fifty asks", "Three weeks out, the stakeholder wish-list needs sorting into contract, change and no.",
            "Events & Venues", "Event Delivery Coordinator", "project_controls", "professional", 5,
            """["scope_discipline","change_control"]""",
            """
            {"context":"Three weeks before a food festival's build, the venue, headline sponsor, council and community group have collectively submitted ~50 requests: extra sponsor branding positions, a community stage slot, changed traffic marshalling hours, more accessible viewing platforms, a drone display, extended bar hours, additional waste points. Some are contractual obligations already; some are priced changes; some are safety-relevant; some are simply new. The team is answering them in email-arrival order.",
             "evidence":[
               {"label":"Volume","value":"~50 requests, 4 stakeholder groups, 3 weeks out"},
               {"label":"Mix","value":"Existing obligations · priced changes · safety-relevant · plain new asks"},
               {"label":"Current method","value":"Email-arrival order"},
               {"label":"Constraint","value":"Licence conditions & safety case are fixed"}],
             "decisions":[
               {"key":"triage","prompt":"The sorting rule?",
                "options":[
                  {"key":"classify","label":"Classify before answering: (1) already-contractual items get scheduled, not debated; (2) safety/licence-touching items (traffic hours, drone, bar extension) route to the safety advisor group THIS week — they have external clocks; (3) genuine changes get priced and offered; (4) the rest get a courteous no with reasons — and every stakeholder sees the same published triage","quality":100,
                   "consequence":"The safety-relevant items hit the safety advisory meeting with days to spare (the drone needs an aviation notification that takes 21 days — caught exactly in time); the sponsor pays for two branding changes, and the email queue stops governing the event.",
                   "principle":"Sort stakeholder asks by their GOVERNING regime — obligation, safety, change, or courtesy — before any individual answer; the sort finds the hidden deadlines."},
                  {"key":"stakeholder","label":"Answer by stakeholder importance: sponsor first, venue second, council third, community last","quality":15,
                   "consequence":"The sponsor's branding positions are settled beautifully while the council's traffic-hours item — carrying a statutory consultation clock — waits in third place until the clock wins.",
                   "principle":"Ranking by stakeholder power misses that some requests carry deadlines no power can extend."},
                  {"key":"continue","label":"Keep arrival order — it's fair and everyone can see the queue","quality":10,
                   "consequence":"Request 34 (the drone) reaches the top of the fair queue six days too late for its aviation notification; fairness to emails was unfairness to physics.",
                   "principle":"Arrival order treats a licence condition and a bar-hours wish as the same species — they are not."}]}],
             "hints":["First sort by governing regime, not by requester or arrival.",
               "Hunt for external clocks — notifications, consultations, licence variations.",
               "Publish the triage; visible rules stop the relitigating."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sorted fifty festival asks by regime and caught the 21-day drone clock in time."}
            """),

        ("WC-SCO-254", "Two hundred requirements, forty duplicates", "Before the backlog is ordered, it must stop describing the same thing six ways.",
            "Enterprise Programmes", "Requirements Analyst", "project_controls", "professional", 5,
            """["requirements_management","scope_discipline"]""",
            """
            {"context":"Consolidating requirements from five departments for the case-management platform, you hold ~200 statements. Sampling shows the real problem isn't volume but redundancy-in-disguise: 'audit trail of all changes', 'history of who edited what', 'tamper-evident record keeping' and three similar statements are one requirement wearing six outfits — with subtly different acceptance implications. The programme wants a prioritised backlog by Friday. Prioritising duplicates ranks the same thing six times.",
             "evidence":[
               {"label":"Volume","value":"~200 statements, 5 departments"},
               {"label":"Sample finding","value":"Requirement families: same need, 4–6 phrasings, subtly different acceptance edges"},
               {"label":"Ask","value":"Prioritised backlog by Friday"},
               {"label":"Trap","value":"Prioritising before deduplicating ranks ghosts"}],
             "decisions":[
               {"key":"order","prompt":"The consolidation order?",
                "options":[
                  {"key":"dedupe_first","label":"Cluster → merge → THEN prioritise: group by underlying need (the audit-trail family becomes ONE requirement whose acceptance criteria union the six variants' real edges, each variant's author confirming their edge survived), then rank the ~120 genuine requirements — Friday delivers fewer, truer, ranked items","quality":100,
                   "consequence":"The 200 collapse to 118; the audit-trail merge surfaces that one department needed legal-hold semantics the others didn't — an acceptance edge that would have been lost in six separate medium-priority duplicates.",
                   "principle":"Deduplicate by NEED before prioritising by value — merging is where the real requirements emerge, and the variants' edges are the treasure, not the trash."},
                  {"key":"rank_all","label":"Prioritise all 200 as submitted — departments wrote what they meant; merging is presumptuous","quality":10,
                   "consequence":"The audit-trail need appears at ranks 12, 31, 48, 77, 90 and 130; the build team implements rank 12's phrasing, and the legal-hold edge (rank 90's version) is discovered missing at UAT.",
                   "principle":"A duplicated requirement's priority is meaningless — its highest-ranked phrasing wins and its edges lose."},
                  {"key":"per_dept","label":"Keep five departmental backlogs — merging across departments causes fights","quality":20,
                   "consequence":"Five tidy backlogs, one platform: the build sequences by department, rebuilding the audit trail three times as each department's variant arrives.",
                   "principle":"Departmental backlogs for one shared platform schedule the duplication into the build itself."}]}],
             "hints":["Cluster by the NEED under the words — phrasing families are one requirement.",
               "The variants' differences are acceptance criteria in disguise; union them, don't discard.",
               "Have each variant's author confirm their edge survived the merge — that's the fight worth having."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Collapsed 200 requirements to 118 real ones — and found the legal-hold edge hiding in a duplicate."}
            """),

        ("WC-SCH-255", "An agenda is a schedule", "The planning workshop has one day to produce a plan. Sequence the day like you'd sequence a project.",
            "Industrial Manufacturing", "Planning Workshop Facilitator", "project_controls", "professional", 6,
            """["schedule_analysis","facilitation"]""",
            """
            {"context":"You're facilitating the one-day planning workshop for the packaging-line relocation — 14 attendees including the ops manager (mornings only), the design lead (needed for constraints AND sequencing) and the logistics contractor (joining remotely, 2-hour window at 13:00). Agenda items: scope walkthrough, constraints capture, milestone definition, activity sequencing, risk identification, and 'parking lot' resolution. Draft agenda currently runs them in that listed order.",
             "evidence":[
               {"label":"People","value":"Ops manager AM only · design lead needed twice · logistics 13:00–15:00 remote"},
               {"label":"Items","value":"Scope · constraints · milestones · sequencing · risks · parking lot"},
               {"label":"Dependency","value":"Sequencing needs constraints; milestones anchor sequencing; risks emerge FROM sequencing"},
               {"label":"Draft","value":"Listed order, people ignored"}],
             "decisions":[
               {"key":"agenda","prompt":"The right agenda?",
                "options":[
                  {"key":"resource_levelled","label":"Schedule it like a project — dependencies AND resource calendars: scope + constraints in the morning (ops manager present for both), milestones before lunch (anchored while ops is still there), sequencing at 13:00 when logistics joins (their window aligned to the item needing them), risks harvested DURING sequencing, parking lot last — the agenda is a resource-levelled network","quality":100,
                   "consequence":"Every item runs with its essential people present; the logistics contractor's two hours land exactly on the sequencing they inform, and the risk list emerges rich because it was captured where risks actually surface — mid-sequencing.",
                   "principle":"An agenda is a schedule with people as resources — level it against attendee calendars and item dependencies like any other plan."},
                  {"key":"listed","label":"Run the listed order — logical enough, and reshuffling confuses attendees","quality":10,
                   "consequence":"Sequencing starts at 14:30 — logistics' window half gone, ops manager long departed; the afternoon's plan is built missing the two voices it most needed, and Tuesday's follow-up meeting is born.",
                   "principle":"An agenda that ignores its attendees' calendars schedules their absence into the output."},
                  {"key":"flexible","label":"No fixed agenda — work the items dynamically as energy and attendance allow","quality":15,
                   "consequence":"Dynamic facilitation meets fourteen opinions about what's next; the day produces excellent discussion of three items and a parking lot containing the other three.",
                   "principle":"Improvised sequencing under attendance constraints just moves the collisions inside the room."}]}],
             "hints":["List who each item NEEDS, then read the attendance calendar — the agenda writes itself.",
               "Fixed windows (the remote contractor) are immovable resources; place their item first.",
               "Some items produce others' inputs — risks live inside sequencing, not after it."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Resource-levelled a workshop agenda like a project — and finished the plan in the room."}
            """),
    };
}
