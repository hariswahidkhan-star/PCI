namespace PCI.Backend.Data;

/// <summary>
/// PCI Project Intelligence — Year-1 Q2 authored pack (April onward).
/// April theme: cost, value, forecasting and commercial awareness, at execution stage.
/// Same contract as the Q1 partial: every item authored TO its plan slot, three progressive
/// hints, consequence + principle per option, synthetic data, validator + gates enforced in CI.
/// </summary>
public static partial class WorldIntelligencePack
{
    static readonly (string Code, string Title, string Hook, string Industry, string Role, string Track,
        string Difficulty, int Minutes, string Competencies, string Config)[] ItemsQ2 =
    {
        // ═════════════ APRIL — cost, value, forecasting and commercial awareness ═════════════
        // ───────────── Cost & Value · foundation · judgment decisions ─────────────

        ("WC-CST-125", "The savings that existed only in the forecast", "The EAC holds — thanks to three savings nobody has actually banked.",
            "Enterprise Programmes", "Cost Engineer", "project_controls", "foundation", 9,
            """["cost_control","forecasting"]""",
            """
            {"context":"Month 7 of a shared-services build. The cost report's estimate at completion still equals budget — but only because it nets off three 'identified savings': a renegotiation not yet agreed, a descope not yet approved, and an efficiency 'expected' from a tool not yet deployed. Without them the EAC is 6% over. The report is due tonight.",
             "evidence":[
               {"label":"EAC as drafted","value":"On budget — includes 3 unbanked savings"},
               {"label":"Saving 1","value":"Supplier renegotiation — meeting is next month"},
               {"label":"Saving 2","value":"Descope — change request not yet raised"},
               {"label":"Saving 3","value":"Tool efficiency — tool not yet deployed"},
               {"label":"Without them","value":"EAC ~6% over budget"}],
             "decisions":[
               {"key":"eac","prompt":"What EAC goes in tonight's report?",
                "options":[
                  {"key":"gross","label":"The 6%-over EAC, with the three savings shown separately as opportunities with owners and dates","quality":100,
                   "consequence":"The report shows an honest gap and a credible plan against it; two of the three savings later land and the EAC improves in public.",
                   "principle":"A saving is a forecast input the day it is banked, not the day it is hoped."},
                  {"key":"net","label":"The on-budget EAC — the savings are genuinely being pursued","quality":10,
                   "consequence":"Two savings land, one dies, and the EAC 'suddenly' jumps 2% in month 10 — the surprise, not the overrun, is what the board remembers.",
                   "principle":"Netting hopes into a forecast converts your future good news into future bad news."},
                  {"key":"footnote","label":"On-budget EAC with a footnote listing the assumptions","quality":30,
                   "consequence":"The footnote is technically disclosure and practically invisible; the headline number still promises what the work has not.",
                   "principle":"Material uncertainty belongs in the number or beside it in equal type — not beneath it."}]}],
             "hints":["Ask of each saving: has anyone signed it, scheduled it, or deployed it?",
               "Separate the measured position from the improvement plan — both belong in the report, distinctly.",
               "Boards forgive gaps with plans; they do not forgive surprises with histories."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept three hopeful savings out of an EAC until they were real."}
            """),

        ("WC-CST-126", "The contingency that bled quietly", "No single drawdown was material. The total is.",
            "Portfolio & PMO", "Portfolio Cost Analyst", "project_controls", "foundation", 10,
            """["cost_control","governance"]""",
            """
            {"context":"Reviewing a portfolio's flagship project mid-execution, you find contingency at 35% consumed by fourteen small drawdowns — each under the project manager's individual approval limit, none linked to a register risk, several labelled 'minor scope trims'. The project is 40% complete. The PM calls it 'normal working'.",
             "evidence":[
               {"label":"Contingency used","value":"35% via 14 sub-threshold drawdowns"},
               {"label":"Register linkage","value":"None of the 14 cite a register risk"},
               {"label":"Progress","value":"40% complete"},
               {"label":"PM's view","value":"'Normal working'"}],
             "decisions":[
               {"key":"pattern","prompt":"Your finding to the portfolio board?",
                "options":[
                  {"key":"expose","label":"Report the pattern: burn rate vs progress, the register disconnect, and a recommendation — drawdowns above a cumulative threshold trigger board visibility and must cite a risk or a change","quality":100,
                   "consequence":"The board tightens the mechanism, not the PM; the remaining contingency lasts precisely because its spending became visible.",
                   "principle":"Contingency governance fails in increments below the approval limit — govern the cumulative, not just the individual."},
                  {"key":"accept","label":"Accept it — every drawdown was within delegated authority","quality":15,
                   "consequence":"Authority was respected and the money still left; month 14 finds a materialised register risk with nothing behind it.",
                   "principle":"Fourteen authorised spends can still be one unauthorised pattern."},
                  {"key":"freeze","label":"Recommend freezing all further drawdowns pending review","quality":35,
                   "consequence":"The freeze punishes the next legitimate risk response for the last fourteen convenient ones, and the PM routes around it via change requests.",
                   "principle":"Fixing a visibility problem with a blockade converts it into a routing problem."}]}],
             "hints":["Compare contingency burn with earned progress, not with calendar.",
               "Check what each drawdown was FOR — contingency spent off-register is budget by another name.",
               "The durable control is cumulative visibility, not smaller individual limits."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a contingency bleeding out in sub-threshold drawdowns."}
            """),

        ("WC-CST-127", "The cheaper panel arrives tomorrow", "A mid-build substitution offer: 8% off, 'equivalent spec', decision needed today.",
            "Construction", "Site Commercial Manager", "project_controls", "foundation", 8,
            """["value_engineering","cost_control"]""",
            """
            {"context":"Mid-execution on an office development, the cladding subcontractor offers a substitute rainscreen panel: 8% cheaper, 'equivalent specification', available tomorrow against a three-week wait for the specified product. The programme would love the three weeks. The specification was part of the planning approval's design statement.",
             "evidence":[
               {"label":"Offer","value":"Substitute panel — 8% cheaper, available tomorrow"},
               {"label":"Specified product","value":"3-week lead time"},
               {"label":"Claim","value":"'Equivalent specification' — subcontractor's words"},
               {"label":"Constraint","value":"Original spec cited in the planning design statement"}],
             "decisions":[
               {"key":"substitute","prompt":"Your call on the substitution?",
                "options":[
                  {"key":"verify","label":"Hold acceptance until the substitute passes a defined equivalence check — fire rating, warranty, planning compliance — expedited this week, with the saving and programme gain banked only if it passes","quality":100,
                   "consequence":"The check takes four days: fire rating equivalent, but the finish triggers a planning condition — the designer secures a minor amendment first and the substitution lands safely.",
                   "principle":"'Equivalent' is a test result, not a sales adjective — and cladding is the last place to take the adjective."},
                  {"key":"take","label":"Take it — 8% and three weeks is exactly the kind of win execution exists to find","quality":5,
                   "consequence":"The panel goes up; the planning officer's site visit finds the unapproved finish; the recladding costs twelve times the saving.",
                   "principle":"A substitution that skips its compliance chain converts a discount into a defect."},
                  {"key":"refuse","label":"Refuse — never substitute a specified product mid-build","quality":30,
                   "consequence":"Safe, and the three legitimate weeks of programme sit unclaimed on the table for doctrine's sake.",
                   "principle":"A blanket no is a decision not to do the analysis that a yes or no deserves."}]}],
             "hints":["List what the original specification was doing — performance, warranty, AND consent obligations.",
               "Price the downside of a failed substitution against the 8% before deciding how fast to decide.",
               "An expedited verification is usually available; an expedited regret is not."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned an 'equivalent' cladding offer into a verified decision, not a gamble."}
            """),

        ("WC-CST-128", "The month the curve crossed the line", "The cash forecast touches the facility ceiling in six weeks. Nobody upstream knows yet.",
            "Enterprise Programmes", "Programme Accountant", "project_controls", "foundation", 11,
            """["cash_flow","stakeholder_communication"]""",
            """
            {"context":"Your programme's rolling cash forecast now shows the cumulative position touching the funding facility's ceiling in six weeks — driven by a customer receipt slipping and two supplier milestones landing early. It recovers the month after. Treasury reviews facilities quarterly; the next review is nine weeks away. Exceeding the facility, even briefly, breaches a covenant.",
             "evidence":[
               {"label":"Forecast","value":"Cumulative position touches facility ceiling in ~6 weeks"},
               {"label":"Drivers","value":"Receipt slipped + two supplier milestones early"},
               {"label":"Recovery","value":"Position recovers the following month"},
               {"label":"Constraint","value":"Facility breach = covenant breach; treasury review in 9 weeks"}],
             "decisions":[
               {"key":"act","prompt":"With six weeks of warning, you:",
                "options":[
                  {"key":"early","label":"Alert treasury NOW with the curve and the two levers you control — negotiating the supplier milestone dates and chasing the receipt — asking for a temporary headroom only if the levers fall short","quality":100,
                   "consequence":"One supplier agrees to re-date for a small consideration; the peak clears the ceiling with room to spare, and treasury notes the programme as one that warns early.",
                   "principle":"A funding problem flagged with six weeks and two levers is management; flagged with six days it is a crisis."},
                  {"key":"manage","label":"Manage it quietly — delay supplier payments informally and it probably never touches","quality":10,
                   "consequence":"'Probably' meets an early supplier invoice; the ceiling is breached by a rounding error, and the covenant conversation happens anyway — after the fact, without goodwill.",
                   "principle":"Informal payment stretching is borrowing from suppliers without telling your bank."},
                  {"key":"wait","label":"Hold it for the quarterly review — the forecast may improve by then","quality":20,
                   "consequence":"The review is three weeks after the projected breach date; the calendar was the one fact not in dispute.",
                   "principle":"Never schedule the disclosure after the event it discloses."}]}],
             "hints":["Read the peak against the ceiling AND the calendar of who can act by when.",
               "Identify the levers you own — receipt chasing, milestone timing — before asking for headroom.",
               "Facilities reward early warning; covenants punish surprises of any size."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Executive Communicator"},
             "share_line":"Flagged a facility breach six weeks early — and then made it not happen."}
            """),

        ("WC-CST-129", "Rates from another market", "The schedule of rates was priced in a different world. Every variation reopens the argument.",
            "Energy Networks", "Commercial Manager", "project_controls", "foundation", 12,
            """["commercial_management","cost_control"]""",
            """
            {"context":"Two years into a grid framework, market prices for cable and copper work have moved ~20% above the contract's schedule of rates. The contractor now disputes the rates on every variation, works slowly on varied scope, and has hinted at 'commercial reprioritisation' of your jobs. The framework has three years to run and reprocurement would cost a year.",
             "evidence":[
               {"label":"Contract rates","value":"Priced 2 years ago; market ~20% higher today"},
               {"label":"Behaviour","value":"Every variation disputed; varied work slow"},
               {"label":"Signal","value":"'Commercial reprioritisation' hinted"},
               {"label":"Alternatives","value":"3 years left; reprocurement ≈ 1 year"}],
             "decisions":[
               {"key":"rates","prompt":"Your commercial strategy?",
                "options":[
                  {"key":"reopen","label":"Propose a structured rate review: indexed uplift on the demonstrably-moved trades, evidence-based, in exchange for restored performance commitments and a no-dispute protocol on variations","quality":100,
                   "consequence":"Rates rise where the market genuinely moved, performance recovers, and the framework's remaining three years are governed by a mechanism instead of a grudge.",
                   "principle":"A contract the market has left behind gets renegotiated once, deliberately — or renegotiated daily, badly, through disputes."},
                  {"key":"hold","label":"Hold the contract line — rates are rates, and reopening invites endless reopening","quality":25,
                   "consequence":"Legally impeccable; operationally the contractor's B-team delivers your jobs at contract rates and contract speed for three long years.",
                   "principle":"You can enforce a price; you cannot enforce enthusiasm."},
                  {"key":"concede","label":"Pay the disputed rates case by case to keep the work moving","quality":10,
                   "consequence":"Every variation now opens at fantasy prices because conceding is your pattern; the total drift exceeds what a structured review would have cost.",
                   "principle":"Case-by-case concession is a rate review run by the other side."}]}],
             "hints":["Separate trades where the market genuinely moved from opportunistic claims — evidence exists for one.",
               "Price the whole-framework cost of each option, including performance, not just the rates.",
               "Trade the uplift for something: performance, protocols, exclusivity — never give it away unpriced."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Renegotiated a market-stranded schedule of rates once — instead of daily."}
            """),

        ("WC-CST-130", "The ledger nobody reconciled", "Committed, accrued, invoiced, paid — four numbers that should relate, and don't.",
            "Technology Programmes", "Assistant Commercial Analyst", "project_controls", "foundation", 9,
            """["cost_control","reporting"]""",
            """
            {"context":"Preparing a platform programme's quarterly cost pack, you find the commitment ledger has not been reconciled to the finance system for five months: purchase orders show 2.1M committed, finance shows 1.6M accrued, and neither ties to the invoices actually paid. The cost report has been built on the finance number throughout.",
             "evidence":[
               {"label":"PO ledger","value":"2.1M committed"},
               {"label":"Finance accruals","value":"1.6M"},
               {"label":"Last reconciliation","value":"5 months ago"},
               {"label":"Cost reports","value":"Built on the finance number throughout"}],
             "decisions":[
               {"key":"reconcile","prompt":"What do you do before the quarterly pack goes out?",
                "options":[
                  {"key":"reconcile_first","label":"Run the reconciliation now — even roughly — and report the verified position with the gap explained, flagging any restatement of prior months","quality":100,
                   "consequence":"The gap turns out to be 0.3M of unaccrued committed work; the pack restates once, cleanly, and the monthly reconciliation becomes routine.",
                   "principle":"A cost report built on an unreconciled ledger is a rumour with a template."},
                  {"key":"finance","label":"Use the finance number again — it is the auditable system of record","quality":20,
                   "consequence":"Auditable and incomplete: the unaccrued commitments land over the next two quarters as 'unexpected' costs that were on a PO ledger all along.",
                   "principle":"System-of-record status describes where a number lives, not whether it is whole."},
                  {"key":"average","label":"Report a figure between the two ledgers while the reconciliation is arranged","quality":0,
                   "consequence":"A number that exists in neither system is now in a board pack; the eventual reconciliation has to explain three positions instead of two.",
                   "principle":"Never publish a number no system can reproduce."}]}],
             "hints":["Commitments precede accruals precede invoices — the gap between them is information, not noise.",
               "Ask what cost has been INCURRED versus recorded — reports forecast from the former.",
               "One clean restatement beats a quarter of quiet drift every time."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reconciled a five-month ledger gap before it became next quarter's surprise."}
            """),

        ("WC-CST-131", "One project, three estimates at completion", "Delivery says on budget. Commercial says 4% over. The PMO model says 9%. One goes in the pack.",
            "Enterprise Programmes", "Senior Cost Analyst", "project_controls", "foundation", 10,
            """["forecasting","governance"]""",
            """
            {"context":"A data-platform build's monthly review has three EACs: delivery's bottom-up estimate (on budget — 'the team knows the remaining work'), commercial's commitment-based view (4% over), and the PMO's performance-factored model (9% over, from measured CPI). Each owner defends their method. The steering pack takes one number.",
             "evidence":[
               {"label":"Delivery bottom-up","value":"On budget — 'we know the remaining work'"},
               {"label":"Commercial commitments","value":"+4% — booked commitments plus known claims"},
               {"label":"PMO performance model","value":"+9% — measured efficiency projected forward"},
               {"label":"History","value":"Delivery's estimate has been 'on budget' every month"}],
             "decisions":[
               {"key":"which","prompt":"What does the pack present?",
                "options":[
                  {"key":"triangulate","label":"The performance-based EAC as the reporting number, with the bottom-up as the TARGET and the gap between them named as the recovery challenge","quality":100,
                   "consequence":"The pack shows a 9% risk and a plan to beat it; when the year ends at 5% over, everyone can see which levers closed the gap.",
                   "principle":"Report from measured performance; manage toward the bottom-up; never confuse the two roles."},
                  {"key":"bottomup","label":"Delivery's number — nobody knows the work better than the people doing it","quality":15,
                   "consequence":"The people who know the work best have also been on budget every month while CPI said otherwise; optimism is not cured by proximity.",
                   "principle":"Bottom-up estimates inherit the estimator's incentives along with their knowledge."},
                  {"key":"middle","label":"Present all three and let the steering group choose","quality":30,
                   "consequence":"The group chooses the friendliest, predictably; presenting a menu was a decision dressed as neutrality.",
                   "principle":"Analysis that ends in a menu has outsourced its one job."}]}],
             "hints":["Ask what each method is FOR: measurement, commitment tracking, and work planning are different jobs.",
               "Check each method's track record against outturn — one of them has been graded monthly.",
               "A reporting number and a management target can differ honestly if the gap is named."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Gave three competing EACs their proper jobs — and the pack one honest number."}
            """),

        ("WC-CST-132", "Small yeses, thin margin", "No single concession cost more than a courtesy. Together they cost the quarter.",
            "Construction", "Package Commercial Lead", "project_controls", "foundation", 8,
            """["commercial_management","cost_control"]""",
            """
            {"context":"Reviewing a fit-out package mid-delivery, you tally the quarter's 'small yeses' to the client's team: out-of-scope fixture moves, free attendance for their direct contractors, absorbed design tweaks, a weekend possession never charged. Individually courteous; together 2.5% of package value, none recorded as change. The client's project manager now treats each as precedent.",
             "evidence":[
               {"label":"The tally","value":"~2.5% of package value in unrecorded concessions"},
               {"label":"Pattern","value":"Fixture moves, free attendance, absorbed tweaks, uncharged possession"},
               {"label":"Records","value":"No change requests raised for any"},
               {"label":"Client behaviour","value":"Each concession now cited as precedent"}],
             "decisions":[
               {"key":"reset","prompt":"How do you stop the bleed without souring the relationship?",
                "options":[
                  {"key":"visible","label":"Start recording every accommodation as a valued change — often at zero charge, explicitly waived — so goodwill becomes visible, countable and finite","quality":100,
                   "consequence":"The next 'small favour' arrives as a signed zero-cost change; three months later the waived-value ledger funds a fair conversation about the one that isn't waived.",
                   "principle":"Goodwill that is invoiced at zero is generosity; goodwill that is invisible is erosion."},
                  {"key":"stop","label":"Stop all concessions immediately — the contract is the contract from today","quality":25,
                   "consequence":"The client experiences a relationship cliff nobody explained; cooperation on YOUR next favour — early access, sequencing flexibility — dries up in reply.",
                   "principle":"An unannounced policy change reads as a mood, and moods get reciprocated."},
                  {"key":"backbill","label":"Raise a consolidated variation for the quarter's accumulated concessions","quality":10,
                   "consequence":"A retrospective invoice for things given freely converts every past courtesy into a grievance — theirs, now.",
                   "principle":"You cannot re-price a gift after it is opened."}]}],
             "hints":["Add up the quarter's favours before judging any one of them.",
               "The problem is invisibility, not generosity — find the mechanism that makes giving visible.",
               "Zero-cost recorded changes preserve both the margin ledger and the relationship."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Executive Communicator"},
             "share_line":"Made a quarter of quiet concessions visible without a single awkward invoice."}
            """),

        ("WC-CST-133", "The milestone that was paid early", "The payment cert says complete. The commissioning records say almost.",
            "Industrial Manufacturing", "Project Cost Controller", "project_controls", "foundation", 11,
            """["cost_control","governance"]""",
            """
            {"context":"On a packaging-line install, this month's payment certificate includes the 'line commissioning complete' milestone — signed by the site manager to 'keep the vendor's cash flow healthy through no fault of theirs'. The commissioning records show two of seven acceptance tests outstanding, re-scheduled a fortnight out. The vendor has been cooperative throughout; the milestone is 12% of contract value.",
             "evidence":[
               {"label":"Certificate","value":"'Commissioning complete' milestone included — 12% of contract"},
               {"label":"Records","value":"2 of 7 acceptance tests outstanding, ~2 weeks out"},
               {"label":"Site manager's reason","value":"Vendor cash flow, 'no fault of theirs'"},
               {"label":"Vendor","value":"Cooperative throughout"}],
             "decisions":[
               {"key":"cert","prompt":"The certificate is on your desk for processing. You:",
                "options":[
                  {"key":"split","label":"Decline the milestone as certified; offer the legitimate route — an assessed interim payment for the five passed tests under the contract's valuation clause, with the milestone certified when the records support it","quality":100,
                   "consequence":"The vendor gets most of the cash this month through the front door; the milestone's meaning — and your payment controls — survive intact.",
                   "principle":"Solve the cash-flow problem with the contract's tools, never with the certificate's integrity."},
                  {"key":"process","label":"Process it — two tests and a fortnight is immaterial, and the vendor has earned goodwill","quality":5,
                   "consequence":"One outstanding test fails; the leverage that would have fixed it quickly was paid out last month, and the 'immaterial' fortnight becomes eleven weeks.",
                   "principle":"A milestone certified early is leverage donated at the moment it was about to be needed."},
                  {"key":"escalate","label":"Escalate the site manager's signature to programme leadership as a controls breach","quality":30,
                   "consequence":"Technically warranted; but the first move being escalation — before offering the legitimate alternative — burns a site relationship the fix did not require.",
                   "principle":"Escalate patterns; fix instances at the level where they occur."}]}],
             "hints":["Check what the milestone certifies against what the records evidence — that gap is the whole question.",
               "Look for the contract's own mechanism for paying part-complete work honestly.",
               "Ask what enforcement tool you still hold if the last tests fail — and who just spent it."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Paid a cooperative vendor honestly without certifying a milestone that wasn't."}
            """),

        ("WC-CST-134", "Spend it by March", "Year-end money is available — for anything that can invoice in six weeks.",
            "Public Programmes", "Programme Finance Officer", "project_controls", "foundation", 12,
            """["cost_control","governance"]""",
            """
            {"context":"Six weeks before financial year-end, your directorate offers the courts-modernisation programme an extra 400,000 of underspend — provided it is INVOICED this financial year. The programme's genuine needs are next year: the training rollout and the second data migration. Things that could invoice in six weeks: bringing forward hardware purchases, prepaying licences, or a consultancy 'discovery study' nobody asked for.",
             "evidence":[
               {"label":"Offer","value":"400,000 — must invoice within 6 weeks"},
               {"label":"Real needs","value":"Training rollout + migration — both next year"},
               {"label":"Six-week candidates","value":"Early hardware, licence prepayment, an unwanted study"},
               {"label":"Rule","value":"Unspent offers return to the centre"}],
             "decisions":[
               {"key":"spend","prompt":"What do you do with the offer?",
                "options":[
                  {"key":"value","label":"Take only what maps to real need at real timing — the licence prepayment that genuinely discounts, decline the rest, and formally request the balance as next-year budget cover for the training rollout","quality":100,
                   "consequence":"120,000 of genuine value is banked; the declined balance strengthens rather than weakens next year's case, because finance remembers who spends honestly.",
                   "principle":"Absorb year-end money only where value and timing genuinely meet — a declined offer is cheaper than a wasted one."},
                  {"key":"all","label":"Take all 400,000 — money returned is money lost, and the programme will find uses","quality":10,
                   "consequence":"Hardware bought early sits depreciating in a store; the study is shelved on arrival; next year's genuine request is met with 'you had 400k in March'.",
                   "principle":"Spending to a calendar instead of a need converts budget into inventory and credibility into questions."},
                  {"key":"decline","label":"Decline it all — year-end spending is bad practice, full stop","quality":35,
                   "consequence":"Principled, and the genuinely discounted licence prepayment — real value, real need — returns to the centre along with the waste.",
                   "principle":"A rule against bad spending should not require refusing good spending."}]}],
             "hints":["Test each candidate against need AND natural timing, not against the deadline.",
               "Count the hidden costs of early purchases: storage, warranty clocks, version risk.",
               "How you handle found money this year prices your credibility next year."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Took the year-end money that made sense — and only that."}
            """),

        // ───────────── Daily Decision · quality, procurement, governance · foundation ─────────────

        ("WC-QLT-135", "Signed, sealed, not inspected", "Forty hold-point signatures. Six with no records behind them.",
            "Healthcare Estates", "Quality Coordinator", "project_management", "foundation", 7,
            """["quality_management","governance"]""",
            """
            {"context":"Auditing a ward refurbishment's inspection and test plan mid-execution, you find six hold-point signatures — all from one subcontractor's supervisor — with no test records, photos or measurements behind them. The work in question is now behind finished walls. The supervisor says the checks 'were absolutely done, just not written up'.",
             "evidence":[
               {"label":"Finding","value":"6 of 40 hold points signed, zero records behind them"},
               {"label":"Signatory","value":"One supervisor, one subcontractor"},
               {"label":"Status","value":"Work now concealed behind finished walls"},
               {"label":"Explanation","value":"'Done, just not written up'"}],
             "decisions":[
               {"key":"respond","prompt":"What happens next?",
                "options":[
                  {"key":"verify","label":"Treat the six as unverified: risk-rank them, open targeted inspection points for the highest-consequence two, and require records-at-signature from that supervisor's chain from today","quality":100,
                   "consequence":"One opened wall shows compliant work, the other a missing fire-stopping collar — found now for hundreds, not at the fire officer's inspection for tens of thousands.",
                   "principle":"An unrecorded check is not a check with paperwork missing — it is a claim, and concealed work is where claims go to be tested."},
                  {"key":"accept","label":"Accept the explanation — opening walls over paperwork is disproportionate","quality":5,
                   "consequence":"The building handover file carries six signatures the fire officer's sampling happens to include; the retrospective opening-up is now their instruction, at their scale.",
                   "principle":"Proportionality is judged by consequence, not by the cost of the check."},
                  {"key":"all","label":"Order all six locations opened and re-inspected — signatures without records are void","quality":45,
                   "consequence":"Defensible, thorough, and four low-consequence openings spend the quality budget that risk-ranking would have saved.",
                   "principle":"Risk-blind rigour is rigour spent where it was easiest to specify, not where it was needed."}]}],
             "hints":["Rank the six by what failure behind that wall would cost — they are not equal.",
               "The supervisor's honesty is not the question; verifiability is.",
               "Fix the forward process at the same moment you address the backward gap."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Risk-ranked six unverifiable signatures before the walls kept their secrets."}
            """),

        ("WC-PRC-136", "Forty percent below the next bid", "The O&M tender's winner is implausibly cheap. Everyone wants to believe it.",
            "Renewables", "Procurement Lead", "project_management", "foundation", 6,
            """["procurement","commercial_management"]""",
            """
            {"context":"Tender returns for a solar portfolio's O&M contract: the lowest bid is 40% below the next, from a contractor new to the region. The evaluation panel is delighted — the saving would fund the monitoring upgrade. Reference checks are clean but thin. Your abnormally-low-tender procedure allows clarification before award.",
             "evidence":[
               {"label":"Spread","value":"Winner 40% below second place"},
               {"label":"Bidder","value":"New to the region; thin but clean references"},
               {"label":"Panel mood","value":"Delighted — saving already earmarked"},
               {"label":"Procedure","value":"Abnormally-low-tender clarification available"}],
             "decisions":[
               {"key":"award","prompt":"Before award, you:",
                "options":[
                  {"key":"probe","label":"Run the abnormally-low procedure properly: require the bidder to evidence its cost build-up — staffing model, response times, spares strategy — against the specification's hard requirements","quality":100,
                   "consequence":"The build-up reveals a staffing model that meets the letter of the spec with half the response capability; the bidder revises upward and still wins — at a price that can actually deliver.",
                   "principle":"An abnormally low bid is a claim about the future; test the claim before you depend on it."},
                  {"key":"award_now","label":"Award it — competition worked, and second-guessing low bids punishes efficiency","quality":10,
                   "consequence":"Year one is fine; year two the under-resourced contractor slow-fails every response-time commitment, and portfolio availability pays the 40% back with interest.",
                   "principle":"The cheapest bid that cannot perform is the most expensive contract you can sign."},
                  {"key":"exclude","label":"Exclude it as abnormally low — 40% gaps are never real","quality":25,
                   "consequence":"Sometimes they are real — a new entrant pricing for entry; exclusion without examination buys the incumbent's price and a possible challenge.",
                   "principle":"Suspicion is a reason to examine, not a finding."}]}],
             "hints":["A 40% gap means someone misread the specification — find out which party.",
               "Ask what the bid's staffing and response model would look like at that price.",
               "The procedure exists precisely for this moment — use it before award, when you still have options."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Tested a too-good bid's arithmetic before the contract depended on it."}
            """),

        ("WC-GOV-137", "The new sponsor wants a new baseline", "Three weeks in post, the incoming sponsor calls the plan 'inherited fiction'.",
            "Portfolio & PMO", "Programme Manager", "project_management", "foundation", 7,
            """["governance","baseline_control"]""",
            """
            {"context":"Your programme's founding sponsor has moved on. The successor, three weeks in, calls the baseline 'inherited fiction', wants everything re-planned around their priorities, and suggests pausing reporting 'until the new baseline stabilises'. The programme is mid-execution, performing at 92% schedule adherence, and its teams are watching how you respond.",
             "evidence":[
               {"label":"New sponsor","value":"3 weeks in post; wants full re-plan + reporting pause"},
               {"label":"Programme state","value":"Mid-execution, 92% schedule adherence"},
               {"label":"Label used","value":"'Inherited fiction'"},
               {"label":"Watching","value":"Delivery teams and the board"}],
             "decisions":[
               {"key":"respond","prompt":"Your response to the sponsor?",
                "options":[
                  {"key":"channel","label":"Welcome the strategic review — offer a structured re-baseline through the change process, priorities mapped to impact, while current reporting continues uninterrupted against the current baseline","quality":100,
                   "consequence":"The review changes two priorities and confirms the rest; the programme absorbs a real sponsor's real preferences without ever going dark.",
                   "principle":"New sponsors get to change the destination — through the gate, with the lights on."},
                  {"key":"comply","label":"Pause reporting and re-plan as asked — sponsors own programmes","quality":10,
                   "consequence":"Eight weeks of darkness while the re-plan wanders; the board asks why performance reporting stopped, and 'the sponsor asked' protects no one.",
                   "principle":"Reporting is owed to governance, not to any single stakeholder's comfort."},
                  {"key":"resist","label":"Defend the baseline — it was approved and the numbers are good","quality":25,
                   "consequence":"You win the meeting and acquire a sponsor who describes the programme as 'resistant' in every forum that matters.",
                   "principle":"A sponsor persuaded is an asset; a sponsor defeated is a haunting."}]}],
             "hints":["Separate the sponsor's right to redirect from the method of redirection.",
               "Ask what continues unchanged while any review runs — reporting should top that list.",
               "Convert 'fiction' into specifics: which assumptions, which priorities, what evidence."],
             "profile_map":{"decision":"Governance Steward","balanced":"Executive Communicator"},
             "share_line":"Gave a new sponsor a real review without letting the programme go dark."}
            """),

        ("WC-QLT-138", "The snag that wasn't a snag", "Reclassify the defect and the handover happens Friday. The defect stays either way.",
            "Construction", "Section Engineer", "project_management", "foundation", 5,
            """["quality_management","governance"]""",
            """
            {"context":"A bridge deck's waterproofing has a lapped joint outside specification — a non-conformance the resident engineer has raised formally. Your project manager suggests reclassifying it as a 'snagging item' so sectional handover can complete Friday, with repair 'in the defects period'. The joint will be under surfacing by then.",
             "evidence":[
               {"label":"Defect","value":"Waterproofing lap outside spec — formal NCR raised"},
               {"label":"Proposal","value":"Reclassify as snag; hand over Friday; fix later"},
               {"label":"Complication","value":"Joint will be beneath surfacing by the defects period"},
               {"label":"Pressure","value":"Sectional completion bonus tied to Friday"}],
             "decisions":[
               {"key":"classify","prompt":"Your position?",
                "options":[
                  {"key":"hold","label":"It stays an NCR: propose the engineered options — repair now before surfacing, or a concession application with the designer's assessment — and let Friday move if it must","quality":100,
                   "consequence":"The designer grants a concession with a monitoring condition in two days; handover happens Monday with the defect either fixed or formally accepted — not renamed.",
                   "principle":"Classification is a technical fact, not a scheduling tool — a defect renamed is a defect concealed."},
                  {"key":"snag","label":"Agree the reclassification — it is minor, and the defects period exists for exactly this","quality":0,
                   "consequence":"Under surfacing, the 'snag' becomes unrepairable without excavation; the leak in year two costs a deck's worth of investigation, and the paper trail shows who renamed it.",
                   "principle":"A defect that will be buried is the most urgent defect on the job, whatever it is called."},
                  {"key":"escalate","label":"Refuse and report the PM's suggestion to the quality director","quality":35,
                   "consequence":"The immediate suggestion dies; so does the working relationship — over a conversation that a firm 'no, and here are the real options' would have resolved.",
                   "principle":"Offer the legitimate route before the escalation route; keep the second in reserve."}]}],
             "hints":["Ask what happens to repairability once the surfacing goes down.",
               "The concession process exists for defects that might be acceptable — renaming does not.",
               "Hold the technical line AND offer a path to Friday-adjacent — both, not either."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept a burial-bound defect on the record with a route to handover."}
            """),

        ("WC-PRC-139", "Priced at midnight, mid-outage", "The variation quote is triple the estimate. The outage clock does not care.",
            "Energy Networks", "Outage Commercial Manager", "project_management", "foundation", 7,
            """["procurement","commercial_management"]""",
            """
            {"context":"Hour 30 of a 72-hour substation outage, opened switchgear reveals corroded busbar supports needing replacement before re-energisation. The contractor — the only one mobilised, inside a live outage — quotes triple your engineer's estimate, 'take it or leave it, the clock is running'. Every outage hour beyond the window costs the operator dearly.",
             "evidence":[
               {"label":"Discovery","value":"Corroded supports; must replace before re-energisation"},
               {"label":"Quote","value":"~3× the engineer's estimate — 'clock is running'"},
               {"label":"Leverage","value":"Only mobilised contractor, live outage"},
               {"label":"Context","value":"Overrun hours are very expensive"}],
             "decisions":[
               {"key":"price","prompt":"With the clock running, you:",
                "options":[
                  {"key":"instruct_records","label":"Instruct the work immediately on a records basis — timesheets, materials, plant, all verified daily — with the price determined later under the contract's valuation rules","quality":100,
                   "consequence":"The work proceeds within the window; the valued cost lands at 1.4× estimate, not 3×, because the records replaced the ransom.",
                   "principle":"When leverage is against you, decouple proceeding from pricing — instruct on records, value under the contract."},
                  {"key":"pay","label":"Accept the quote — three times a small number beats overrun hours","quality":15,
                   "consequence":"The arithmetic holds tonight; the precedent prices every future mid-outage discovery at ransom rates, and there will be discoveries.",
                   "principle":"Paying the ransom once establishes the ransom as the rate card."},
                  {"key":"negotiate","label":"Negotiate the quote down before authorising any work","quality":25,
                   "consequence":"Ninety minutes of haggling inside a 72-hour window; you save a third of the quote and spend outage hours worth more.",
                   "principle":"Never negotiate a price while the thing you are negotiating with is burning."}]}],
             "hints":["Separate the two decisions bundled in the quote: whether to proceed, and at what price.",
               "Check the contract for its valuation-of-instructed-work mechanism — it exists for this.",
               "Records discipline TONIGHT is what makes the later valuation stick."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Beat a mid-outage ransom quote with a records-basis instruction."}
            """),

        ("WC-GOV-140", "Fifteen percent bigger, same business case", "The scope grew a change at a time. The approved case never noticed.",
            "Industrial Manufacturing", "PMO Lead", "project_management", "foundation", 5,
            """["governance","change_control"]""",
            """
            {"context":"Preparing a mid-execution review of a plant automation project, you find cumulative approved changes have grown scope ~15% by value — each change properly approved individually — while the business case of record still describes the original scope, cost and benefits. The sponsor considers the case 'historic paperwork'.",
             "evidence":[
               {"label":"Scope drift","value":"+15% by value via properly-approved changes"},
               {"label":"Business case","value":"Unamended since original approval"},
               {"label":"Sponsor's view","value":"'Historic paperwork'"},
               {"label":"Trigger","value":"Mid-execution review imminent"}],
             "decisions":[
               {"key":"case","prompt":"Ahead of the review, you recommend:",
                "options":[
                  {"key":"refresh","label":"A case refresh: current scope, cost and — critically — whether the benefits still clear the investment bar at the new size, presented to the review as the honest test","quality":100,
                   "consequence":"The refreshed case still clears the bar, at a thinner ratio the sponsor now actually knows; the next change request gets asked a question nobody had been asking.",
                   "principle":"A business case is the running justification for spend, or it is nothing — every material change re-asks its question."},
                  {"key":"historic","label":"Agree with the sponsor — the changes were approved; the case served its purpose at sanction","quality":10,
                   "consequence":"The review panel asks what investment logic currently governs the spend; 'each change individually' turns out to be an uncomfortable answer said aloud.",
                   "principle":"Individually approved changes can sum to a project nobody ever approved."},
                  {"key":"threshold","label":"Skip the refresh but propose a rule: cumulative drift beyond 10% triggers one automatically next time","quality":40,
                   "consequence":"A good rule adopted while the current 15% sails past it — the review notices the irony.",
                   "principle":"A control proposed to avoid applying it now is a confession with a process attached."}]}],
             "hints":["Sum the approved changes and re-read the case as if new — does it still describe this project?",
               "The question is not whether changes were approved but whether their SUM still pays back.",
               "Propose the threshold rule AND apply its spirit to the present case."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Made a 15%-bigger project re-earn its business case."}
            """),

        // ───────────── Risk Room · evidence diagnosis · practitioner ─────────────

        ("WC-RSK-141", "The register says calm. The depot says otherwise.", "Three artifacts from one port project. One of them is telling the truth loudest.",
            "Ports & Logistics", "Risk Analyst", "project_controls", "professional", 9,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"A port depot expansion's monthly risk review gives you three artifacts: a register unchanged for three cycles (top risk: 'weather delays', amber, static), a site diary noting the piling contractor has twice demobilised crews to another job mid-week, and a procurement log showing the fender system order — a 26-week lead item — still unplaced 30 weeks before its need date.",
             "evidence":[
               {"label":"Register","value":"Unchanged 3 cycles; top risk 'weather', amber, static"},
               {"label":"Site diary","value":"Piling crews demobilised to another job, twice"},
               {"label":"Procurement log","value":"Fender system: 26-week lead, unplaced, need date in 30 weeks"},
               {"label":"Ask","value":"'What should this month's review actually discuss?'"}],
             "decisions":[
               {"key":"signal","prompt":"The signal that most needs the review's attention:",
                "options":[
                  {"key":"fender","label":"The unplaced fender order — 4 weeks of float against a 26-week lead is a near-certain schedule strike with a simple fix that expires weekly","quality":100,
                   "consequence":"The order is expedited inside a fortnight; the risk that would have arrived in month eight with total certainty quietly never happens.",
                   "principle":"The best risk to escalate is the one that is cheap to kill this week and expensive to meet later."},
                  {"key":"piling","label":"The crew demobilisations — a contractor drifting away mid-week is a delivery risk in motion","quality":50,
                   "consequence":"Real and worth a commercial conversation — but its impact is visible, incremental and recoverable; the fender's is silent, binary and dated.",
                   "principle":"Rank moving risks by irreversibility, not by visibility."},
                  {"key":"register","label":"The stale register itself — a three-cycle-static register is the meta-risk behind both","quality":40,
                   "consequence":"True, and fixing the process this month while the fender order sits unplaced wins the audit and loses the quay.",
                   "principle":"Process reform is never urgent in the week a concrete risk is."}]},
               {"key":"process","prompt":"And the register's staleness?",
                "options":[
                  {"key":"live","label":"Rebuild the review around live evidence feeds — diary, procurement, commercial logs — with the register updated FROM them each cycle","quality":100,
                   "consequence":"Next month's register contains what the project is actually experiencing; 'weather, amber, static' finally has company.",
                   "principle":"A register fed by meetings describes the meetings; one fed by evidence describes the project."},
                  {"key":"chase","label":"Ask risk owners to update their entries before each review","quality":25,
                   "consequence":"Owners update adjectives; the artifacts that actually knew things remain unread.",
                   "principle":"Chasing owners refreshes the paint, not the picture."}]}],
             "hints":["Convert each signal into time: what is its deadline for cheap action?",
               "26-week lead minus 30-week need date leaves four weeks of shrinking option.",
               "A static register is a symptom — treat the patient this month, the process every month."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the port project's real risk in the procurement log, not the register."}
            """),

        ("WC-RSK-142", "The drawdown for the wrong risk", "The claim cites the risk that didn't happen. The money is for the one that did.",
            "Energy Networks", "Contingency Board Analyst", "project_controls", "professional", 10,
            """["risk_management","cost_control"]""",
            """
            {"context":"A grid project's delivery team requests a major contingency drawdown citing register risk R-07, 'ground conditions'. You review the evidence pack: the geotechnical reports show conditions within design assumptions; the cost overrun actually traces to the team's own re-sequencing decision three months ago, which doubled crane mobilisations. The overrun is real and needs funding. The citation is not.",
             "evidence":[
               {"label":"Claim","value":"Drawdown against R-07 'ground conditions'"},
               {"label":"Geotech reports","value":"Conditions within design assumptions"},
               {"label":"Actual cause","value":"Team's own re-sequencing → doubled crane mobilisations"},
               {"label":"Reality","value":"The cost is real and must be funded somehow"}],
             "decisions":[
               {"key":"finding","prompt":"Your recommendation to the contingency board?",
                "options":[
                  {"key":"reroute","label":"Fund the overrun through the honest route — a management-decision variance, board-approved — and decline the R-07 citation, keeping the register's data clean","quality":100,
                   "consequence":"The money flows, correctly labelled; the register's ground-conditions line keeps meaning something, and re-sequencing decisions now carry a visible price tag.",
                   "principle":"Contingency mis-cited is data corrupted — fund the truth under its own name."},
                  {"key":"approve","label":"Approve as cited — the money is needed and R-07 has budget against it","quality":10,
                   "consequence":"The books balance and lie: the register 'learns' that ground risk materialised (it didn't), and re-sequencing 'cost nothing' (it didn't) — both lessons shape the next project wrongly.",
                   "principle":"Every mislabelled drawdown teaches the organisation two false lessons at once."},
                  {"key":"refuse","label":"Refuse the drawdown entirely — mis-citation forfeits the request","quality":25,
                   "consequence":"The unfunded overrun starts starving the remaining work; punishing the label leaves the project bleeding through the delay.",
                   "principle":"Reject the citation, never the arithmetic — the cost exists whatever it is called."}]}],
             "hints":["Trace the overrun to its actual causal event, not its most convenient register line.",
               "Ask what the register will 'learn' if this citation is approved as written.",
               "Separate the funding decision (yes, somehow) from the attribution decision (accurately)."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Governance Steward"},
             "share_line":"Funded a real overrun under its real name — and kept the register honest."}
            """),

        ("WC-RSK-143", "Twelve weeks late, one door open", "The delay is confirmed. Three responses are on the table. The evidence prefers one.",
            "Enterprise Programmes", "Programme Risk Manager", "project_controls", "professional", 8,
            """["risk_management","opportunity_management"]""",
            """
            {"context":"A core-system vendor has confirmed a twelve-week delivery slip on your integration programme. Three response papers reach you the same afternoon: delivery proposes standing teams down to save cost; the architecture lead proposes using the window to fix the data-quality debt that has plagued testing; commercial proposes claiming the delay costs and banking the recovery. The evidence pack shows testing defect rates tripled by bad data, team remobilisation historically taking six weeks, and the contract's delay clause paying out at 60% of actual costs.",
             "evidence":[
               {"label":"Option A — stand down","value":"Saves burn; remobilisation historically ~6 weeks"},
               {"label":"Option B — data-quality fix","value":"Defect rates ×3 attributed to bad data"},
               {"label":"Option C — claim only","value":"Delay clause pays ~60% of actual costs"},
               {"label":"Slip","value":"12 weeks, vendor-confirmed"}],
             "decisions":[
               {"key":"respond","prompt":"Reading the evidence together, you recommend:",
                "options":[
                  {"key":"convert","label":"Keep teams on and execute the data-quality fix inside the window — AND pursue the claim for the incremental costs — because the evidence says remobilisation would eat half the 'saving' and dirty data is the programme's biggest measured drag","quality":100,
                   "consequence":"Testing resumes twelve weeks later against clean data; defect rates fall by more than the window cost, and the claim funds most of it.",
                   "principle":"A slip is capacity — spend it on the constraint the evidence has already measured."},
                  {"key":"save","label":"Stand teams down — cash preserved is the only certain benefit on the table","quality":20,
                   "consequence":"Six of the twelve weeks are later consumed remobilising; the data debt waits, and testing resumes at triple defect rates as before.",
                   "principle":"A saving that costs its own second half is a headline, not a benefit."},
                  {"key":"claim","label":"Focus on the claim — recover the money and keep everything else unchanged","quality":30,
                   "consequence":"Sixty percent of costs return; one hundred percent of the window passes unused on the programme's best-documented problem.",
                   "principle":"Recovery of cost is not recovery of time — the window closes either way."}]}],
             "hints":["Weigh each option against the pack's MEASURED numbers, not its adjectives.",
               "Check whether the options are actually exclusive — some combine.",
               "The best use of unexpected time is the constraint you already knew about."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Spent a twelve-week vendor slip on the programme's measured constraint."}
            """),

        ("WC-RSK-144", "The green map and the red festival", "Every risk is 'managed'. The site visit says otherwise.",
            "Events & Venues", "Event Delivery Risk Lead", "project_controls", "professional", 11,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"Nine weeks before a three-day festival's build, the delivery board reviews a reassuring heat map — every risk amber or green, all 'managed'. Your site visit the same week found: the main-stage roof supplier's rigging crew booked on another event until four days before your build starts; the traffic management plan awaiting a council officer who is on leave for three weeks; and the medical provider's contract still unsigned because of an indemnity clause. The board asks whether the map reflects reality.",
             "evidence":[
               {"label":"Heat map","value":"All risks amber/green, 'managed'"},
               {"label":"Site finding 1","value":"Stage roof riggers double-booked until build-minus-4 days"},
               {"label":"Site finding 2","value":"Traffic plan awaiting an officer on leave 3 weeks"},
               {"label":"Site finding 3","value":"Medical contract unsigned — indemnity clause"}],
             "decisions":[
               {"key":"triage","prompt":"Which finding is the show-stopper to escalate first?",
                "options":[
                  {"key":"medical","label":"The unsigned medical contract — without it the safety case fails and the licence conditions cannot be met; no medical cover, no event, and legal clauses do not negotiate themselves under deadline","quality":100,
                   "consequence":"Legal escalation unsticks the indemnity wording in a week; the licence submission proceeds — the one absolute veto on the event is retired first.",
                   "principle":"Escalate first the risk that is a licence condition — everything else assumes the event exists."},
                  {"key":"riggers","label":"The double-booked riggers — a four-day margin on the main stage is a build schedule with no immune system","quality":45,
                   "consequence":"Genuinely serious and worth immediate commercial pressure — but a late stage degrades the event; a missing medical contract cancels it.",
                   "principle":"Sort veto risks from degradation risks before ranking by discomfort."},
                  {"key":"traffic","label":"The traffic plan — councils on leave do not accelerate for anyone","quality":30,
                   "consequence":"A deputy officer path exists and is found with one phone call; the three-week wait was a queue, not a wall.",
                   "principle":"Test whether a blockage is structural or personnel before escalating it as existential."}]},
               {"key":"map","prompt":"And the all-green heat map?",
                "options":[
                  {"key":"evidence","label":"Require every 'managed' rating to cite its evidence — a contract, a booking, a signature — and re-grade the three findings on the spot","quality":100,
                   "consequence":"The map turns honestly patchy overnight; the board starts trusting it precisely because it stopped being uniformly green.",
                   "principle":"'Managed' is a claim about documents, not a colour preference."},
                  {"key":"note","label":"Note the discrepancies and ask owners to review their ratings","quality":20,
                   "consequence":"Owners review, ratings survive, and the map stays green until the site does not.",
                   "principle":"Self-review of one's own optimism has a known outcome."}]}],
             "hints":["Separate risks that would CANCEL the event from those that would degrade it.",
               "A licence condition unmet outranks every schedule risk on the board.",
               "Ratings backed by documents can be audited; ratings backed by confidence cannot."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the festival's one true show-stopper behind an all-green map."}
            """),

        // ───────────── Stakeholder Dilemma · resources & leadership · practitioner ─────────────

        ("WC-STK-145", "One engineer, two emergencies", "Both projects hit their crisis in the same week. Both 'must have' the same person.",
            "Capital Programmes", "Delivery Director's Deputy", "project_management", "professional", 6,
            """["resource_management","conflict_management"]""",
            """
            {"context":"Mid-execution, both of your portfolio's tunnelling projects hit geotechnical trouble in the same week — and both project managers demand the portfolio's principal geotechnical engineer full-time, immediately. Project A faces a face-loss risk with crews standing; Project B has a settlement trend threatening a rail possession decision due Friday. The engineer, asked informally, says both are real and she cannot do both properly.",
             "evidence":[
               {"label":"Project A","value":"Face-loss risk; crews standing; cost of delay immediate"},
               {"label":"Project B","value":"Settlement trend; possession decision due Friday"},
               {"label":"The engineer","value":"'Both real. I cannot do both properly.'"},
               {"label":"You","value":"Hold the allocation call this week"}],
             "decisions":[
               {"key":"allocate","prompt":"Your allocation?",
                "options":[
                  {"key":"triage","label":"Sequence by decision deadline: the engineer leads B's possession assessment through Friday with A receiving her review of its monitoring data nightly plus an external specialist mobilised NOW as her support on A","quality":100,
                   "consequence":"Friday's possession decision is made on proper analysis; A's standing time is covered by structured remote review until the specialist lands Monday — both PMs grumble, neither is abandoned.",
                   "principle":"Allocate scarce expertise by decision deadline and irreversibility — and buy the second-best cover for the other front immediately."},
                  {"key":"loudest","label":"Full-time to A — standing crews are burning cash by the hour","quality":25,
                   "consequence":"A's burn is visible and finite; B's possession decision, made without her, is deferred a month by the rail authority — a cost dwarfing A's standing time.",
                   "principle":"The most visible cost is rarely the largest one on the table."},
                  {"key":"split","label":"Half-days on each — both projects get her, fairness is preserved","quality":15,
                   "consequence":"Context-switching between two live geotechnical crises produces two shallow analyses; the engineer flags the risk in writing, correctly.",
                   "principle":"Splitting a specialist across simultaneous crises is how both get her worst work."}]},
               {"key":"after","prompt":"After the week ends, you:",
                "options":[
                  {"key":"bench","label":"Stand up a retained external geotechnical bench for the portfolio — pre-qualified, framework rates, 48-hour mobilisation","quality":100,
                   "consequence":"The next simultaneous crisis is a phone call, not a Sophie's choice.",
                   "principle":"A single point of expertise across a portfolio is a risk you have already met — treat it like one."},
                  {"key":"nothing","label":"Nothing — weeks like that are rare","quality":10,
                   "consequence":"Rare, and the third one arrives during her annual leave.",
                   "principle":"'Rare' events at portfolio scale are called 'quarterly'."}]}],
             "hints":["Rank by decision deadline and reversibility, not by decibels or burn rate.",
               "Ask what partial-but-structured support to the second front looks like.",
               "The systemic answer is bench depth, not better triage next time."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Triaged one principal engineer across two live crises by deadline, not decibels."}
            """),

        ("WC-STK-146", "The implementation team that exists twice", "The vendor promised your rollout the A-team. Another customer got the same promise.",
            "Technology Programmes", "Implementation Programme Manager", "project_management", "professional", 5,
            """["resource_management","commercial_management"]""",
            """
            {"context":"Your ERP rollout's execution phase depends on the vendor's named implementation team — written into the statement of work after hard negotiation. Through a partner-community contact you learn the same named team is committed to another customer's go-live in the same eight-week window. The vendor's account manager, asked casually, says scheduling is 'being optimised'.",
             "evidence":[
               {"label":"Your SOW","value":"Named team, named window — negotiated hard"},
               {"label":"Intelligence","value":"Same team committed to another go-live, same window"},
               {"label":"Vendor line","value":"Scheduling 'being optimised'"},
               {"label":"Your exposure","value":"Cutover rehearsals begin in 3 weeks"}],
             "decisions":[
               {"key":"confront","prompt":"How do you play it?",
                "options":[
                  {"key":"formal","label":"Invoke the SOW formally but constructively: request the named team's confirmed allocation calendar for the window within five days, flagging the contractual remedy path if it cannot be evidenced","quality":100,
                   "consequence":"The vendor, forced to choose in daylight, assigns your rehearsals the named leads plus vetted seconds for the overlap — a real plan replacing an 'optimisation'.",
                   "principle":"A contractual promise about people is verified with a calendar, not an adjective."},
                  {"key":"trust","label":"Take 'optimised' at face value — vendors juggle; they usually land it","quality":10,
                   "consequence":"Week one of rehearsals arrives with strangers who ask for the context pack; the named team is at the other customer, whose escalation was louder earlier.",
                   "principle":"When two customers hold one promise, the one who verifies first holds it last."},
                  {"key":"nuclear","label":"Escalate to vendor leadership citing anticipatory breach, copying legal","quality":30,
                   "consequence":"You win the team and lose the goodwill that implementation quality quietly depends on; the A-team arrives resentful.",
                   "principle":"Deploy the legal register at the escalation's SECOND step, not its first."}]}],
             "hints":["A promise about named people is testable — ask for the thing that would prove it.",
               "Move before the conflict is resolved in the other customer's favour by default.",
               "Keep the contractual lever visible and undrawn — its shadow does most of its work."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Converted a vendor's 'optimised' scheduling into a verified team calendar."}
            """),

        ("WC-STK-147", "Take the keys, or leave them", "Operations says the team isn't ready to run it. The project needs to close.",
            "Enterprise Programmes", "Transition Manager", "project_management", "professional", 6,
            """["resource_management","stakeholder_communication"]""",
            """
            {"context":"A logistics-automation project is complete and stable — four weeks of clean hypercare metrics. The operations manager refuses to sign the transfer to business-as-usual: her team 'is not ready', two of five support staff having only just finished training. Every week of delay keeps expensive project resources on hypercare, and two other projects wait for them. She has a history of being right about readiness.",
             "evidence":[
               {"label":"System","value":"Stable — 4 weeks clean hypercare metrics"},
               {"label":"Ops position","value":"'Team not ready' — 2 of 5 just trained"},
               {"label":"Cost of delay","value":"Project team held; two projects queued behind"},
               {"label":"Track record","value":"She has been right about readiness before"}],
             "decisions":[
               {"key":"transfer","prompt":"Your proposal?",
                "options":[
                  {"key":"staged","label":"A staged transfer with objective exit criteria: ops takes ownership NOW, two project engineers remain embedded on a defined taper tied to measured ticket-resolution competence, not calendar","quality":100,
                   "consequence":"Ownership transfers with a safety net that shrinks on evidence; the ops manager signs because the criteria are hers to meet, and the project team is fully released in five weeks.",
                   "principle":"Readiness disputes are resolved with criteria and tapers, not with standoffs about dates."},
                  {"key":"force","label":"Escalate for a directed transfer — the metrics are clean and the delay cost is real","quality":20,
                   "consequence":"The transfer is directed; the first significant incident is handled badly by an unready team, and 'we were forced' becomes the permanent story of the system.",
                   "principle":"A forced handover makes the receiving team's failure everyone's future."},
                  {"key":"wait","label":"Extend hypercare until she is comfortable — her record has earned deference","quality":30,
                   "consequence":"Comfort without criteria has no arrival date; week nine finds the same conversation with three projects now queued.",
                   "principle":"Deference to judgment still needs a definition of done."}]}],
             "hints":["Convert 'not ready' into measurable criteria — whose achievement she controls.",
               "Separate ownership transfer from support withdrawal; they can move at different speeds.",
               "Respect the track record by building it into the criteria, not by abandoning the timeline."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a readiness standoff into a criteria-based taper both sides signed."}
            """),

        // ───────────── Executive Mission · capstone · execution stage ─────────────

        ("WC-CAP-148", "The quarter everything slipped", "Three workstreams red in the same review. One afternoon to set the response.",
            "Enterprise Programmes", "Programme Director", "project_management", "expert", 24,
            """["governance","strategy_execution","cost_control"]""",
            """
            {"context":"Your customer-platform programme's quarterly review lands three reds at once: the integration workstream is six weeks behind after a vendor slip; the data migration failed its second dress rehearsal on reconciliation errors; and the branch-readiness stream reports training completion at 40% against an 80% plan. The board meets Thursday and expects a coherent response, not three separate excuses. Cash burn is on plan; the go-live is contractually promised to a major client in five months.",
             "evidence":[
               {"label":"Integration","value":"6 weeks behind — vendor slip, recovery plan credible but unfunded"},
               {"label":"Migration","value":"2nd dress rehearsal failed — reconciliation errors traced to source data"},
               {"label":"Readiness","value":"Training 40% vs 80% plan — trainer capacity, not content"},
               {"label":"Constraint","value":"Contractual go-live in 5 months; burn on plan"},
               {"label":"Board","value":"Thursday — expects one coherent response"}],
             "decisions":[
               {"key":"diagnose","prompt":"Stage 1 — what is the true shape of the problem?",
                "options":[
                  {"key":"linked","label":"Treat the three reds as one system: migration data quality is the binding constraint — it gates integration testing AND meaningful training — so the response is sequenced around fixing data first","quality":100,
                   "consequence":"The single-constraint framing survives challenge: with clean data, integration tests against reality and training uses the live system, collapsing three recoveries into one.",
                   "principle":"Three simultaneous reds usually share one upstream cause — find the constraint before funding three cures."},
                  {"key":"parallel","label":"Three independent problems, three parallel recovery plans, maximum simultaneous progress","quality":25,
                   "consequence":"Three plans compete for the same architects and test environments; the interference costs more than the parallelism gains.",
                   "principle":"Parallel recovery of coupled problems is how programmes thrash."},
                  {"key":"worst","label":"All effort onto the contractual risk — integration — the other two can queue","quality":30,
                   "consequence":"Integration accelerates into testing against dirty data, and its defects send it backwards; the queue was load-bearing.",
                   "principle":"The loudest deadline is not necessarily the binding constraint."}]},
               {"key":"board","prompt":"Stage 2 — Thursday's board message?",
                "options":[
                  {"key":"one_story","label":"One narrative: the constraint diagnosis, the sequenced recovery with its funding ask, the go-live confidence range with its assumptions — and the client-communication plan for the risk of a short slip","quality":100,
                   "consequence":"The board funds the recovery in one sitting and pre-authorises the client conversation; nobody manages the programme by rumour.",
                   "principle":"Boards can govern one honest system; they cannot govern three competing stories."},
                  {"key":"soften","label":"Report two reds recovering and hold the migration story until the third rehearsal","quality":10,
                   "consequence":"The third rehearsal's outcome arrives before the next board — as a leak; the softened report becomes the story.",
                   "principle":"Partial candour discovered is total credibility spent."},
                  {"key":"blame","label":"Lead with the vendor's slip as the proximate cause of the quarter","quality":20,
                   "consequence":"Accurate about one red, silent on two; the board notices the arithmetic and the deflection at the same moment.",
                   "principle":"An external excuse that covers a third of the problem indicts the rest."}]},
               {"key":"client","prompt":"Stage 3 — the contractual client?",
                "options":[
                  {"key":"early_frame","label":"Brief the client NOW at executive level: the recovery, the confidence range, and a jointly-owned contingency plan for a phased go-live if the range's tail materialises","quality":100,
                   "consequence":"The client, treated as a partner five months out, negotiates a phased-scope option that later saves the relationship when three weeks of the tail arrive.",
                   "principle":"Contractual dates survive contact with reality only when both parties manage the reality together."},
                  {"key":"hold","label":"Say nothing yet — five months is long and recovery may hold the date","quality":15,
                   "consequence":"Recovery holds most of the date; the three-week shortfall lands on a client with no warning and a contract in hand.",
                   "principle":"Hope is not a client-communication strategy."},
                  {"key":"renegotiate","label":"Request a formal date change now while there is time","quality":30,
                   "consequence":"Premature: you trade away a date the recovery might have held, and the client banks the concession before you know if you needed it.",
                   "principle":"Never concede a date before the evidence says you must — but never hide the range either."}]}],
             "hints":["Look for the upstream cause the three reds share before designing three fixes.",
               "Boards fund constraints and sequences; they cannot fund three competing narratives.",
               "The client conversation has a best window: after diagnosis, before rumour."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Collapsed three red workstreams into one constraint and one credible recovery."}
            """),

        ("WC-CAP-149", "Ten percent less, same promise", "The funding cut is final. What you do with it is not.",
            "Construction", "Framework Programme Director", "project_management", "expert", 22,
            """["governance","strategy_execution","cost_control"]""",
            """
            {"context":"Mid-delivery of a six-town flood-resilience framework, your funder confirms a permanent 10% budget cut effective next quarter — macroeconomic, non-negotiable, applied to the remaining envelope. Public commitments exist for all six towns. Three schemes are in construction, two in detailed design, one in early design. Your response sets the framework's shape and its politics for years.",
             "evidence":[
               {"label":"Cut","value":"10% of remaining envelope, permanent, next quarter"},
               {"label":"In construction","value":"3 towns — stopping mid-works costs more than finishing"},
               {"label":"In detailed design","value":"2 towns — committed publicly, not contractually"},
               {"label":"Early design","value":"1 town — least committed, highest benefit-cost ratio of the six"},
               {"label":"Politics","value":"All six towns were promised 'their scheme'"}],
             "decisions":[
               {"key":"strategy","prompt":"Stage 1 — the allocation strategy?",
                "options":[
                  {"key":"protect_optimise","label":"Protect the three in construction; take the cut through value engineering on the two in detailed design (where change is still cheap) plus a re-scoped, phased version of the sixth — explicitly preserving its high-BCR core","quality":100,
                   "consequence":"The cut lands where change costs least; all six towns keep a scheme, two slightly leaner, one phased — and the framework's benefit total drops by only 4% against a 10% cut.",
                   "principle":"Absorb cuts where designs are still soft; never where concrete is already poured or where the best benefits live."},
                  {"key":"salami","label":"Apply 10% evenly across all six schemes — equal pain, defensible everywhere","quality":20,
                   "consequence":"The in-construction schemes pay via scope compromises that cost more to change than they save; the equal-pain story is fair and value-destroying in equal measure.",
                   "principle":"Uniform cuts are politically simple because they are analytically empty."},
                  {"key":"drop","label":"Cancel the sixth town outright — cleanest arithmetic, one grievance instead of six","quality":25,
                   "consequence":"The arithmetic works; the cancelled town — with the best benefit-cost ratio — becomes the campaign story, and the funder asks why you cut the best value first.",
                   "principle":"The least-committed scheme and the least-valuable scheme are not the same scheme."}]},
               {"key":"towns","prompt":"Stage 2 — the public narrative for six towns?",
                "options":[
                  {"key":"differentiated","label":"Town-by-town honesty: what changes, what does not, and when — delivered locally by named leaders with the revised scope maps, before any press release","quality":100,
                   "consequence":"Five towns absorb the news as adjustment, not betrayal; the phased town's council becomes an advocate for the core scheme it helped re-shape.",
                   "principle":"Communities forgive smaller schemes; they do not forgive discovering them in the newspaper."},
                  {"key":"central","label":"One framework-level statement: 'efficiencies will protect all commitments'","quality":15,
                   "consequence":"The statement survives until the first revised drawing leaks; 'protect all commitments' is then quoted back at every meeting for two years.",
                   "principle":"A reassurance that outruns the facts becomes the opposition's best exhibit."},
                  {"key":"delay","label":"Communicate after the revised designs are finished — accuracy first","quality":25,
                   "consequence":"Three months of accurate silence, filled by rumour of cancellations worse than reality; the eventual truth fights uphill against the vacuum's version.",
                   "principle":"In public programmes, silence is content — and someone else writes it."}]},
               {"key":"funder","prompt":"Stage 3 — the relationship with the funder?",
                "options":[
                  {"key":"evidence_back","label":"Deliver the cut AND a benefits ledger: what the 10% costs in outcomes, documented per town — feeding the next funding round's evidence base","quality":100,
                   "consequence":"The framework takes its medicine credibly; when a recovery budget appears two years later, your documented forgone benefits are first in the queue.",
                   "principle":"Absorb the cut professionally and invoice it in evidence — funders remember who showed them the price."},
                  {"key":"fight","label":"Contest the cut through every available channel before implementing anything","quality":15,
                   "consequence":"The macro decision does not bend; two quarters of contesting delay the re-planning the cut required anyway.",
                   "principle":"Fighting a decided macro cut spends the influence you will need for the next round."},
                  {"key":"silent","label":"Implement quietly — good soldiers get remembered kindly","quality":25,
                   "consequence":"The cut lands smoothly and invisibly; at the next round, the framework that absorbed 10% 'without impact' is offered another 10%.",
                   "principle":"A cut absorbed silently is a cut priced at zero — expect repeat orders."}]}],
             "hints":["Map where change is still cheap — that is where cuts belong.",
               "Check which scheme carries the best benefit-cost ratio before choosing a casualty.",
               "Deliver the cut and document its price — both, visibly."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Landed a permanent 10% cut across six towns without cutting the best value first."}
            """),

        // ═════════════ MAY — risk, opportunity, uncertainty and contingency ═════════════
        // ───────────── Risk Room · evidence diagnosis · practitioner ─────────────

        ("WC-RSK-150", "Four warranty claims, one pattern", "Individually routine. Together, a message from the future.",
            "Enterprise Programmes", "Programme Risk Analyst", "project_controls", "professional", 9,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"Reviewing a rollout programme's monthly evidence, you find four warranty claims on installed control units — different sites, different installers, same capacitor failure mode, all within 14 months of a 24-month warranty. Six hundred more units are installed; two hundred are still to install from the same batch stock. The supplier calls the claims 'within normal failure rates'.",
             "evidence":[
               {"label":"Claims","value":"4 units, 4 sites, SAME failure mode, months 9–14"},
               {"label":"Exposure","value":"600 installed + 200 in stock, same batch"},
               {"label":"Supplier","value":"'Within normal failure rates'"},
               {"label":"Warranty","value":"24 months from install"}],
             "decisions":[
               {"key":"read","prompt":"What does the evidence actually say?",
                "options":[
                  {"key":"pattern","label":"A common-mode signal: same component, same mode, early-life — commission an independent failure analysis on the four units NOW and quarantine the uninstalled batch pending results","quality":100,
                   "consequence":"The analysis finds a batch-level electrolyte defect; the 200 quarantined units are exchanged free under warranty and the field-failure curve for the 600 gets a monitoring plan before it steepens.",
                   "principle":"Identical failure modes across independent sites is a batch talking — statistics of small numbers do not apply to common causes."},
                  {"key":"normal","label":"Accept the supplier's framing — four failures in six hundred is under 1%","quality":10,
                   "consequence":"The rate is fine; the MODE is the message. Month 20 brings thirty more failures as the batch ages together, half out of warranty.",
                   "principle":"A failure RATE reassures; a failure MODE warns — read the one that carries information."},
                  {"key":"claim","label":"Process the four claims commercially and move on — that is what warranty is for","quality":25,
                   "consequence":"Four replacements arrive from the same batch stock; the warranty process works perfectly while the underlying risk compounds.",
                   "principle":"Warranty is a remedy for instances, not a treatment for causes."}]},
               {"key":"register","prompt":"On the register, this becomes:",
                "options":[
                  {"key":"quantified","label":"A quantified line: probability informed by the failure analysis, impact modelled on out-of-warranty replacement across the fleet, owner named, review dated","quality":100,
                   "consequence":"When the analysis lands, the line re-prices in a day and the mitigation is already costed.",
                   "principle":"Field evidence is the best probability data a register ever gets — wire it in."},
                  {"key":"watch","label":"A watch-list note pending more failures","quality":20,
                   "consequence":"More failures dutifully arrive to satisfy the evidence threshold — each one now out of quarantine's reach.",
                   "principle":"Waiting for more data when the data is components failing is called 'paying for certainty retail'."}]}],
             "hints":["Distinguish the failure rate from the failure mode — one is noise, one is signal.",
               "Ask what the four claims share that independent random failures would not.",
               "Count the exposure still in your control: the uninstalled batch is the cheap decision."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read four routine warranty claims as one batch defect — before 800 units aged into it."}
            """),

        ("WC-RSK-151", "The trend and the threshold", "Settlement is at 61% of the alarm limit and climbing. The method statement says keep going.",
            "Urban Construction", "Monitoring & Risk Engineer", "project_controls", "professional", 10,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"An excavation beside a listed terrace has settlement monitoring: alarm at 25mm, work stops at 30mm. Readings are at 15.2mm — 61% of alarm — but the last six weekly readings show steady acceleration: 0.8, 0.9, 1.2, 1.5, 1.9, 2.4mm per week. The method statement only mandates action at the alarm level. The basement pour that loads the retaining system further is scheduled Monday.",
             "evidence":[
               {"label":"Current","value":"15.2mm vs alarm 25mm, stop 30mm"},
               {"label":"Weekly movement","value":"0.8 → 0.9 → 1.2 → 1.5 → 1.9 → 2.4 mm/week"},
               {"label":"Method statement","value":"Action required only at alarm level"},
               {"label":"Monday","value":"Basement pour adds load to the system"}],
             "decisions":[
               {"key":"read","prompt":"Your reading of the evidence?",
                "options":[
                  {"key":"trend","label":"The trend IS the alarm: at current acceleration the limit arrives in ~3 weeks — pause Monday's pour, convene the designer for a root-cause review of the acceleration BEFORE adding load","quality":100,
                   "consequence":"The review finds a dewatering issue amplifying movement; fixed in a week, the curve flattens, and the pour proceeds onto a system that is behaving.",
                   "principle":"Thresholds are for instruments; engineers read derivatives — an accelerating trend under incoming load is the event, early."},
                  {"key":"threshold","label":"Continue per the method statement — 61% of alarm is a green condition by definition","quality":10,
                   "consequence":"Monday's load meets an accelerating system; the alarm level arrives in nine days with the pour cured in place and remediation options halved.",
                   "principle":"A method statement encodes yesterday's assumptions; the readings are today's facts."},
                  {"key":"monitor","label":"Increase monitoring to daily and proceed with the pour — better data, no delay","quality":30,
                   "consequence":"Daily readings beautifully document the acceleration the extra load produces; measurement was never the missing ingredient.",
                   "principle":"More frequent measurement of an unmanaged risk is surveillance, not treatment."}]},
               {"key":"system","prompt":"Longer-term, the trigger regime should:",
                "options":[
                  {"key":"rate","label":"Add rate-of-change triggers alongside absolute levels, with defined responses for each","quality":100,
                   "consequence":"The next acceleration trips a defined review at 40% of the limit instead of relying on someone noticing.",
                   "principle":"Instrument regimes should encode the derivative, not just the level — that is where the warning lives."},
                  {"key":"lower","label":"Lower the alarm threshold to be safe","quality":25,
                   "consequence":"A slow steady drift now alarms early while a fast acceleration below the new level still wouldn't — wrong variable, tightened.",
                   "principle":"Tightening the wrong parameter buys alarm fatigue, not safety."}]}],
             "hints":["Plot the weekly increments, not just the total — what is the curve doing?",
               "Project the trend against the calendar of load-adding activities.",
               "Ask what a review NOW costs versus what remediation after the pour costs."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read the settlement derivative before Monday's pour made it history."}
            """),

        ("WC-RSK-152", "The month nobody nearly got hurt", "Near-miss reports fell to zero. That is not good news.",
            "Enterprise Programmes", "HSE Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","safety_culture"]""",
            """
            {"context":"A logistics-network build's monthly dashboard shows near-miss reports at zero for the second month — down from a steady 8–12. The same period: two new subcontractors mobilised, night shifts doubled, and the principal contractor introduced a 'safety league table' ranking gangs by incident-free days, with a bonus attached. Site management presents the zero as evidence the safety push is working.",
             "evidence":[
               {"label":"Near-miss reports","value":"8–12/month steady → 0 for two months"},
               {"label":"Same period","value":"2 new subcontractors, night shifts doubled"},
               {"label":"New scheme","value":"League table + bonus for incident-free days"},
               {"label":"Management view","value":"'The safety push is working'"}],
             "decisions":[
               {"key":"read","prompt":"Your reading of the zero?",
                "options":[
                  {"key":"suppress","label":"Reporting has been suppressed, not risk: the bonus punishes reporting — recommend decoupling rewards from report counts, rewarding reporting itself, and walking the night shift to hear what stopped being written down","quality":100,
                   "consequence":"The walk finds three unreported events in a fortnight; the scheme is rebuilt around leading indicators, and reports return — with the information they carry.",
                   "principle":"When you pay people for the absence of bad news, you purchase silence, not safety."},
                  {"key":"accept","label":"Take the win — culture initiatives do reduce near-misses","quality":5,
                   "consequence":"The silence holds until the incident that no near-miss data warned about; the investigation finds the missing reports in workers' memories.",
                   "principle":"Risk exposure that doubled while reports vanished is not improvement — it is darkness."},
                  {"key":"audit","label":"Commission a formal reporting-culture audit before concluding anything","quality":40,
                   "consequence":"Sound instinct, slow instrument: eight weeks of audit while the bonus keeps buying silence on doubled night work.",
                   "principle":"When the causal mechanism is visible and reversible, reverse it first and audit second."}]},
               {"key":"metric","prompt":"The dashboard metric should become:",
                "options":[
                  {"key":"leading","label":"Reporting RATE tracked as a health indicator — with falling reports during rising activity flagged as a risk in itself","quality":100,
                   "consequence":"The dashboard now reads silence as a warning; the next suppression pattern is caught in one cycle.",
                   "principle":"Near-miss reporting is a sensor; its output falling to zero means the sensor failed, not the hazard."},
                  {"key":"keep","label":"Keep counting near-misses as before — changing metrics mid-year breaks trends","quality":15,
                   "consequence":"Trend continuity is preserved for a number that no longer measures anything.",
                   "principle":"A consistent measurement of nothing is consistently nothing."}]}],
             "hints":["Ask what changed in the same period as the number — incentives are evidence.",
               "Zero reports during doubled exposure has two explanations; only one is plausible.",
               "Fix the incentive before commissioning studies of its effects."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Read a too-perfect safety dashboard as the sensor failure it was."}
            """),

        ("WC-RSK-153", "The register the leaver left behind", "Eleven risks, one owner — and her badge was returned on Friday.",
            "Portfolio & PMO", "Risk Process Lead", "project_controls", "professional", 11,
            """["risk_management","governance"]""",
            """
            {"context":"The programme's commercial director left on Friday — amicably, quickly. Monday's register review finds her named as owner of eleven risks, including the top two by exposure. Of her eleven, six have open mitigation actions with due dates in the next month; two mitigation actions turn out, on inspection, to exist only as her calendar entries; nobody else can describe risk R-04's status at all.",
             "evidence":[
               {"label":"Orphaned","value":"11 risks incl. top 2 by exposure"},
               {"label":"Actions due","value":"6 mitigation actions due within a month"},
               {"label":"Discovery","value":"2 'actions' were only her calendar entries"},
               {"label":"Worse","value":"R-04's current status unknown to anyone"}],
             "decisions":[
               {"key":"triage","prompt":"Monday afternoon's move?",
                "options":[
                  {"key":"handover","label":"An exposure-ranked triage with her (paid handover day if needed): status, real action state and context per risk — reassigned to named owners with the two phantom actions rebuilt as real ones","quality":100,
                   "consequence":"One structured day recovers ten of eleven risks' context; R-04 needs a fresh assessment — known within the week rather than discovered at materialisation.",
                   "principle":"Risk knowledge lives in people; when the person leaves, the knowledge has a half-life measured in days."},
                  {"key":"reassign","label":"Bulk-reassign all eleven to her successor when they start next month","quality":15,
                   "consequence":"The successor inherits eleven labels with no context, five weeks stale; two mitigation windows close unworked during the gap.",
                   "principle":"Reassigning a name field transfers accountability, not understanding."},
                  {"key":"pmo","label":"The PMO holds them centrally 'for continuity' until things settle","quality":25,
                   "consequence":"The PMO can chase actions but cannot make commercial judgments; ownership without authority is a filing arrangement.",
                   "principle":"A risk owner must be able to DECIDE about the risk — custody is not ownership."}]},
               {"key":"systemic","prompt":"So the next leaver doesn't orphan a register:",
                "options":[
                  {"key":"design","label":"Concentration limits and deputies: no individual owns more than N of the top exposures, every top risk has a named deputy, and leaver checklists include a risk handover","quality":100,
                   "consequence":"The next departure transfers a quarter of the load with a deputy already fluent in it.",
                   "principle":"Key-person risk applies to the risk process itself — govern the concentration you would flag anywhere else."},
                  {"key":"docs","label":"Mandate fuller written risk records so ownership matters less","quality":30,
                   "consequence":"Records improve; the judgment, relationships and unwritten context still walk out with the next badge.",
                   "principle":"Documentation reduces the loss; only succession design prevents it."}]}],
             "hints":["Rank the orphans by exposure and action urgency before doing anything uniform.",
               "The leaver is the cheapest source of context this week and unavailable next month.",
               "Notice the concentration itself as a finding — how did one person own the top two?"],
             "profile_map":{"decision":"Risk Strategist","balanced":"Governance Steward"},
             "share_line":"Recovered an orphaned register while its context was still recoverable."}
            """),

        ("WC-RSK-154", "The supplier's quiet quarter", "Nothing failed. Three signals say something might.",
            "Energy Networks", "Supply Chain Risk Analyst", "project_controls", "professional", 12,
            """["risk_management","supply_chain"]""",
            """
            {"context":"Your HV cable supplier — sole qualified source for two remaining projects — has delivered on time all quarter. The same quarter's soft signals: their parent group's credit rating outlook moved to negative; two senior engineers you work with left for a competitor; and their finance team asked — for the first time in five years — for a milestone payment restructure 'to smooth their cash profile'. Nothing has breached. The board asks for your supply-chain risk view.",
             "evidence":[
               {"label":"Delivery","value":"On time all quarter, quality unchanged"},
               {"label":"Signal 1","value":"Parent group credit outlook → negative"},
               {"label":"Signal 2","value":"Two senior engineers left for a competitor"},
               {"label":"Signal 3","value":"First-ever request to restructure milestone payments"},
               {"label":"Exposure","value":"Sole qualified source, 2 projects remaining"}],
             "decisions":[
               {"key":"read","prompt":"Your risk view to the board?",
                "options":[
                  {"key":"composite","label":"The three signals compose one hypothesis — financial stress — worth acting on while relations are good: enhanced financial monitoring by agreement, escrow or vesting for materials paid ahead, and a second-source qualification programme started NOW (it takes 9 months either way)","quality":100,
                   "consequence":"The supplier, approached respectfully, accepts vesting certificates; the second source qualifies two months before the supplier's group enters restructuring — your projects are the ones that don't stop.",
                   "principle":"Insolvency never announces itself through the delivery metrics until it is too late to act — read the finance, people and behaviour signals as one instrument."},
                  {"key":"performance","label":"Report green — supply risk is measured by delivery performance, which is flawless","quality":10,
                   "consequence":"Performance stays flawless right up until the administrators' letter; the 9-month second-source clock starts that day instead of this one.",
                   "principle":"A distressed supplier performs perfectly for exactly as long as it can — the cliff has no gradient."},
                  {"key":"confront","label":"Demand audited financials and contractual assurances immediately, citing the signals","quality":30,
                   "consequence":"The demand leaks internally as 'the client thinks we're going under' — morale and then people follow the two engineers out; suspicion helped cause what it feared.",
                   "principle":"Protective moves against a fragile counterparty must not be the shove that topples them."}]},
               {"key":"payments","prompt":"On their payment-restructure request specifically:",
                "options":[
                  {"key":"secured","label":"Accommodate it — WITH security: vesting of materials, verified milestones, step-in rights refreshed","quality":100,
                   "consequence":"They get the cash-flow relief that may keep them healthy; you get title to what you've paid for if it doesn't.",
                   "principle":"Help a stressed supplier survive AND secure your position — the two are allies, not alternatives."},
                  {"key":"refuse","label":"Refuse — the contract's payment terms exist for a reason","quality":20,
                   "consequence":"The terms hold; the cash gap they were trying to smooth arrives anyway, as your delivery risk.",
                   "principle":"Enforcing terms against a failing counterparty collects the failure, not the money."}]}],
             "hints":["Combine the three signals into one hypothesis before judging any singly.",
               "Ask how much warning delivery metrics give before an insolvency — and how much you need.",
               "Every protective action has a relationship cost; sequence the quiet ones first."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Started the nine-month second-source clock before the supplier's cliff, not after."}
            """),

        ("WC-RSK-155", "Seventeen findings and a launch date", "The penetration test came back. So did the pressure to summarise it kindly.",
            "Technology Programmes", "Delivery Risk Manager", "project_controls", "professional", 9,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"Ten days before a customer-portal launch, the penetration test returns: two critical findings (an authentication bypass on a legacy endpoint; unencrypted PII in an error-logging path), five high, ten medium. The workstream lead's draft summary for the go/no-go board reads 'no blockers; hardening items scheduled post-launch'. The two criticals have remediation estimates of four and six days respectively.",
             "evidence":[
               {"label":"Criticals","value":"Auth bypass (legacy endpoint) · PII in error logs"},
               {"label":"Volume","value":"2 critical, 5 high, 10 medium"},
               {"label":"Remediation","value":"Criticals: 4 and 6 days' work"},
               {"label":"Draft summary","value":"'No blockers; hardening post-launch'"}],
             "decisions":[
               {"key":"board","prompt":"What reaches the go/no-go board?",
                "options":[
                  {"key":"honest","label":"The findings as graded, with the real trade: both criticals fixed pre-launch (6 days parallel — inside the window), highs risk-assessed individually, mediums scheduled — and the summary corrected before it ships","quality":100,
                   "consequence":"Launch slips zero days — the criticals fit the window once someone insisted they use it; the board decided on facts and knows it.",
                   "principle":"Security findings reach decision-makers at their tested severity — 'hardening' is not a synonym for 'critical'."},
                  {"key":"soft","label":"Let the summary stand — the lead owns the workstream and criticals in legacy endpoints rarely get exploited quickly","quality":0,
                   "consequence":"The auth bypass is found by a researcher in week three; the disclosure timeline shows the board was told 'no blockers', and the incident review reads the draft summary aloud.",
                   "principle":"Relabelled severity is the one risk decision that always gets audited eventually."},
                  {"key":"block","label":"Recommend a blanket launch hold until all seventeen findings are closed","quality":30,
                   "consequence":"A six-week hold for ten mediums that carried ordinary risk; the criticals needed six days, and proportionality was the analysis you were asked for.",
                   "principle":"Treating all findings as blockers is not rigour — it is refusing to do the risk assessment."}]},
               {"key":"legacy","prompt":"The auth bypass lives in a legacy endpoint 'nobody uses'. You:",
                "options":[
                  {"key":"verify","label":"Verify with traffic data, and if truly unused, kill the endpoint — removal beats remediation","quality":100,
                   "consequence":"Logs show eleven calls a month from one forgotten integration; it is migrated in a day and the endpoint dies — smallest possible attack surface, smallest possible work.",
                   "principle":"'Nobody uses it' is a testable claim, and removal is the one patch that never regresses."},
                  {"key":"assume","label":"Take 'unused' at face value and deprioritise accordingly","quality":15,
                   "consequence":"The eleven monthly calls included the attacker's reconnaissance.",
                   "principle":"Unverified 'unused' is how legacy endpoints become front doors."}]}],
             "hints":["Compare the criticals' remediation estimates against the actual days remaining.",
               "Severity grading exists so boards don't need to be security experts — preserve it end to end.",
               "For legacy surface, ask whether deletion is cheaper than defence."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Governance Steward"},
             "share_line":"Got two critical findings to the board at their real severity — and still launched on time."}
            """),

        ("WC-RSK-156", "Ninety-six percent of the chart", "Every lift succeeded. The margins tell a different story.",
            "Construction", "Lifting Operations Risk Reviewer", "project_controls", "professional", 10,
            """["risk_management","evidence_analysis"]""",
            """
            {"context":"A quarterly review of a stadium roof project's lifting operations: 240 lifts, zero incidents. Buried in the crane telemetry: 31 lifts exceeded 90% of rated capacity, 9 exceeded 96%, and the three heaviest all occurred on days with recorded gusts near the operational wind limit. The appointed person says the numbers show 'the planning is accurate — we use the crane we pay for'.",
             "evidence":[
               {"label":"Headline","value":"240 lifts, zero incidents"},
               {"label":"Telemetry","value":"31 lifts >90% capacity; 9 >96%"},
               {"label":"Compound","value":"3 heaviest lifts on near-wind-limit days"},
               {"label":"AP's view","value":"'Planning is accurate — we use what we pay for'"}],
             "decisions":[
               {"key":"read","prompt":"Your finding for the review?",
                "options":[
                  {"key":"margins","label":"The system is consuming its safety margin as efficiency: 9 lifts at >96% plus wind-limit compounding means routine operations have colonised the contingency — recommend a margin policy (planning cap below rated, compound-condition rules) before the heavier phase-two lifts","quality":100,
                   "consequence":"Phase two's heavier lifts arrive with a 90% planning cap and wind-compound rules; the first out-of-tolerance load cell reading has somewhere to go besides an incident report.",
                   "principle":"Zero incidents at shrinking margins is not safety — it is unexpired luck; margins exist to absorb the estimate errors that telemetry proves you make."},
                  {"key":"accurate","label":"Agree with the AP — utilisation near capacity is what accurate planning looks like","quality":10,
                   "consequence":"Phase two's heavier loads meet the same philosophy; the first 4% estimating error meets a 96% lift on a gusty Tuesday.",
                   "principle":"Running at 96% of rated means a 5% surprise is an overload — and lifting is a domain of 5% surprises."},
                  {"key":"stop","label":"Suspend lifting pending investigation of the 9 high-utilisation lifts","quality":25,
                   "consequence":"A suspension for lifts that were individually compliant reads as panic; the schedule pays for a policy question a review could have answered.",
                   "principle":"When nothing was non-compliant, fix the policy that made 'compliant' too thin — don't retrofit alarm."}]},
               {"key":"wind","prompt":"And the wind-compounding specifically?",
                "options":[
                  {"key":"matrix","label":"A compound-condition matrix: capacity utilisation caps that TIGHTEN as wind approaches limits — two near-limit conditions never stack","quality":100,
                   "consequence":"The next heavy lift on a marginal day gets split or postponed by rule, not by whoever feels brave.",
                   "principle":"Independent limits each at 95% is one combined limit at somewhere past 100%."},
                  {"key":"trust","label":"Leave it to operator judgment — that is what competence means","quality":20,
                   "consequence":"Competent operators under schedule pressure judge exactly the way the incentives point.",
                   "principle":"Rules exist for the days when judgment is most needed and least available."}]}],
             "hints":["Look past the incident count to the margin distribution — where is the tail?",
               "Ask what happens when two independently-acceptable conditions occur together.",
               "Phase two's loads are heavier: today's margins are tomorrow's overloads."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the consumed safety margin inside a zero-incident quarter."}
            """),

        ("WC-RSK-157", "The exclusion in the renewal", "Same premium, same broker, one quietly vanished peril.",
            "Enterprise Programmes", "Insurance & Risk Analyst", "project_controls", "professional", 8,
            """["risk_management","commercial_management"]""",
            """
            {"context":"The programme's construction all-risks policy renews next month. The renewal terms arrive at last year's premium — 'a great result in this market', says the broker's covering note. In the schedule of exclusions, new this year: damage arising from 'ground movement in made ground'. Your two remaining sites are both on remediated industrial land — made ground — with eighteen months of foundation work ahead.",
             "evidence":[
               {"label":"Premium","value":"Flat vs last year — 'great result'"},
               {"label":"New exclusion","value":"Ground movement in made ground"},
               {"label":"Your sites","value":"Both on remediated (made) ground; 18 months of foundations ahead"},
               {"label":"Renewal","value":"Next month"}],
             "decisions":[
               {"key":"respond","prompt":"Your move on the renewal?",
                "options":[
                  {"key":"challenge","label":"Treat the flat premium as the decoy it is: instruct the broker to negotiate the exclusion out or price its removal, in parallel get market alternatives, and quantify the retained exposure for the board if cover truly isn't available","quality":100,
                   "consequence":"The exclusion prices at a 12% premium uplift with a survey condition — real money that is a rounding error against one uninsured foundation failure; the board buys it knowingly.",
                   "principle":"An insurance renewal is re-underwriting in disguise — the premium is the headline, the exclusions are the contract."},
                  {"key":"accept","label":"Bind the renewal — flat premium in a hard market shouldn't be pushed","quality":5,
                   "consequence":"Month eleven: differential settlement in remediated fill damages a core; the claim meets the exclusion that made the premium flat.",
                   "principle":"A premium that didn't rise in a hard market paid for itself somewhere — find where before you sign."},
                  {"key":"selfinsure","label":"Accept the exclusion and add the exposure to the risk register as self-insured","quality":40,
                   "consequence":"Honest — but the register entry happened before anyone tested whether transferable cover existed at a viable price.",
                   "principle":"Retain a risk after the transfer market says no — not instead of asking it."}]},
               {"key":"process","prompt":"To catch the next quiet exclusion:",
                "options":[
                  {"key":"diff","label":"A mandatory year-on-year policy diff — exclusions, conditions, definitions — mapped against the current risk register before any renewal binds","quality":100,
                   "consequence":"Next year's renewal review takes an hour longer and reads every word that moved.",
                   "principle":"Compare policies like contracts, because they are."},
                  {"key":"broker","label":"Ask the broker to flag material changes going forward","quality":25,
                   "consequence":"The broker's definition of material and yours diverge exactly when it matters.",
                   "principle":"Outsourcing vigilance to the seller's agent has a known failure mode."}]}],
             "hints":["Map every new exclusion against your actual sites and remaining scope.",
               "Ask why the premium DIDN'T rise — the answer is usually in the wording.",
               "Price the exclusion's removal before deciding to live with it."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Cost Guardian"},
             "share_line":"Caught the exclusion that made a flat premium 'a great result'."}
            """),

        ("WC-RSK-158", "The milestone that was true from a distance", "The FOI response is due Friday. The milestone it asks about was 'achieved' creatively.",
            "Public Programmes", "Programme Risk & Assurance Lead", "project_controls", "professional", 11,
            """["risk_management","governance"]""",
            """
            {"context":"A freedom-of-information request asks for evidence behind a public statement that your courts-digitisation programme 'achieved its Phase 2 milestone in March'. Internally, you know Phase 2 was declared achieved with two acceptance criteria waived by the sponsor — legitimately, through governance, but the waivers were not mentioned in the public statement. Legal says a minimal response is defensible. The response is due Friday.",
             "evidence":[
               {"label":"FOI request","value":"Evidence behind 'Phase 2 achieved in March'"},
               {"label":"Reality","value":"Achieved WITH two criteria waived — via proper governance"},
               {"label":"Public statement","value":"No mention of the waivers"},
               {"label":"Legal view","value":"Minimal response defensible"}],
             "decisions":[
               {"key":"respond","prompt":"Your recommendation for the response?",
                "options":[
                  {"key":"full","label":"Release the milestone certificate AND the waiver decisions with their rationale — the governance was proper, so show it working — and brief the sponsor's comms team before Friday, not after","quality":100,
                   "consequence":"The story runs as 'programme waived two criteria through proper process' — a one-day item; the alternative story, 'programme concealed waivers', was a one-month item with a select-committee coda.",
                   "principle":"When the process was defensible, disclosure is the defence — concealment converts a governance footnote into a cover-up headline."},
                  {"key":"minimal","label":"Take legal's route — answer the question asked, precisely and no more","quality":15,
                   "consequence":"The requester, whose next FOI is sharper, obtains the waivers in round two; the two-step disclosure now IS the story.",
                   "principle":"Minimal responses to persistent requesters are instalment plans for bigger stories."},
                  {"key":"resist","label":"Apply exemptions and decline the substantive parts","quality":5,
                   "consequence":"The ICO overturns the exemption in four months; the release arrives with a regulator's criticism stapled to it.",
                   "principle":"An exemption that won't survive review is a delay purchased at compound interest."}]},
               {"key":"upstream","prompt":"And the underlying practice?",
                "options":[
                  {"key":"align","label":"Fix the source: public milestone statements must disclose material waivers at the time — a one-line standard for the comms protocol","quality":100,
                   "consequence":"The next waived criterion appears in the announcement's second sentence, and no FOI can ever make it a story.",
                   "principle":"The cheapest FOI response is a public record that was complete the first time."},
                  {"key":"tighten","label":"Route future FOI responses through programme leadership for message control","quality":10,
                   "consequence":"Slower responses, same gaps, plus a new appearance of central message management for the next requester to write about.",
                   "principle":"Controlling the answers is a poor substitute for improving the statements."}]}],
             "hints":["Separate what was done (defensible) from what was said (incomplete) — the risk lives in the gap.",
               "Assume the requester already suspects the answer; respond to round two's question in round one.",
               "The systemic fix is at the announcement, not the response."],
             "profile_map":{"decision":"Governance Steward","balanced":"Executive Communicator"},
             "share_line":"Answered an FOI with the waivers — and turned a scandal into a footnote."}
            """),

        ("WC-RSK-159", "One rig, three lines", "The test rig everyone shares is the single point of failure nobody scheduled.",
            "Industrial Manufacturing", "Manufacturing Risk Analyst", "project_controls", "professional", 12,
            """["risk_management","interface_management"]""",
            """
            {"context":"Reviewing a plant expansion's commissioning evidence, you notice all three new production lines' acceptance tests route through one environmental test rig — a 14-year-old chamber with a proprietary controller whose manufacturer exited the business. The rig's calendar shows 92% utilisation for the next five months. Maintenance records show two controller faults this year, each fixed by 'the contractor who knows it'. He is one man, and he is 71.",
             "evidence":[
               {"label":"Dependency","value":"3 lines' acceptance tests → 1 test rig"},
               {"label":"The rig","value":"14 years old; controller manufacturer defunct"},
               {"label":"Load","value":"92% booked for 5 months"},
               {"label":"Support","value":"2 faults this year, both fixed by one 71-year-old contractor"}],
             "decisions":[
               {"key":"assess","prompt":"How do you frame this for the programme board?",
                "options":[
                  {"key":"compound","label":"As one compound single-point-of-failure — asset, controller, knowledge — with layered treatment: commission a controller-replacement study, contract the specialist to document and train NOW, qualify an external test house as surge capacity, and de-peak the rig calendar","quality":100,
                   "consequence":"The specialist's documented month becomes the controller retrofit's spec; when the chamber faults in month four, the external test house absorbs one line's tests and the schedule bends instead of breaking.",
                   "principle":"A single point of failure with a single point of repair is one bad Tuesday from a programme stop — treat the asset, the knowledge and the capacity as three risks wearing one coat."},
                  {"key":"reliability","label":"As an ageing-asset line: schedule a major service and continue — two faults a year is manageable","quality":20,
                   "consequence":"The service helps the mechanics; the defunct controller and the 71-year-old's memory — the actual risks — are outside a service's scope.",
                   "principle":"Servicing the metal does not service the obsolescence or the knowledge."},
                  {"key":"replace","label":"As a capital item: replace the rig — everything else is patching","quality":40,
                   "consequence":"Right destination, wrong bridge: the replacement's 11-month lead time leaves five months of 92% utilisation on the old rig with no interim treatment at all.",
                   "principle":"A capital fix without a bridging plan is a plan to be lucky until delivery."}]},
               {"key":"knowledge","prompt":"The specialist's knowledge specifically:",
                "options":[
                  {"key":"capture","label":"A paid documentation-and-shadowing engagement this quarter — fault trees, spares list, a trained deputy — while he is available and willing","quality":100,
                   "consequence":"The knowledge outlives the dependency; his eventual retirement is an event on a calendar instead of a crisis in a corridor.",
                   "principle":"Knowledge held in one head is leased, not owned — and this lease has no renewal option."},
                  {"key":"retainer","label":"Put him on a retainer with response-time commitments","quality":30,
                   "consequence":"Availability secured, single point unchanged; retainers do not transfer what happens at 71-plus-one.",
                   "principle":"A retainer buys time with the risk, not freedom from it."}]}],
             "hints":["Stack the dependencies: asset condition × controller obsolescence × knowledge concentration × calendar load.",
               "Ask what absorbs the load during ANY treatment — surge capacity is part of the fix.",
               "The knowledge risk has the hardest deadline; sequence it first."],
             "profile_map":{"decision":"Risk Strategist","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Unstacked a rig, a dead controller and one man's memory before three lines needed all of them."}
            """),

        // ───────────── May · Daily Decisions · quality & procurement · foundation ─────────────

        ("WC-QLT-160", "The survey kit with the expired sticker", "Six weeks of setting-out data from an instrument overdue its calibration.",
            "Rail Infrastructure", "Assistant Engineer", "project_management", "foundation", 6,
            """["quality_management","evidence_analysis"]""",
            """
            {"context":"During a routine equipment check on a resignalling project you find the site's total station is six weeks past its calibration due date — and it has been used daily for setting out signal bases and cable-route pegs throughout. The instrument seems fine; the surveyor calls the calibration cycle 'conservative by design'.",
             "evidence":[
               {"label":"Instrument","value":"Total station, 6 weeks past calibration"},
               {"label":"Usage","value":"Daily setting-out for 6 weeks — signal bases, cable pegs"},
               {"label":"Surveyor's view","value":"'Cycles are conservative by design'"},
               {"label":"Works status","value":"Some bases already concreted"}],
             "decisions":[
               {"key":"act","prompt":"What do you do?",
                "options":[
                  {"key":"verify","label":"Calibrate now and use the result to decide backwards: if it passes within tolerance, records close the gap; if it is out, quantify the drift and re-check the concreted bases against independent control","quality":100,
                   "consequence":"The check finds it marginally out in one axis; two of forty bases need re-survey and one needs a shim — found before track furniture arrived, at trivial cost.",
                   "principle":"An overdue calibration converts six weeks of measurements into six weeks of claims — test the instrument, then let the result grade the data."},
                  {"key":"quiet","label":"Book the calibration and say nothing about the gap — the kit is 'probably fine'","quality":5,
                   "consequence":"'Probably' meets the signal sighting committee, whose checks find the drift and then the calibration record's date.",
                   "principle":"A closed gap with an open secret is not closed."},
                  {"key":"redo","label":"Order full re-survey of everything set out in the six weeks","quality":35,
                   "consequence":"Thorough, expensive, and premature — the calibration result would have told you whether any of it was necessary.",
                   "principle":"Verify the instrument before condemning its output."}]}],
             "hints":["The calibration result is evidence about the past six weeks, not just the future.",
               "Rank what was set out by consequence-of-error before deciding what to re-check.",
               "Fix the recall system that let the sticker expire — that is the repeat-preventer."],
             "profile_map":{"decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned an expired calibration sticker into a bounded, evidenced re-check."}
            """),

        ("WC-QLT-161", "The shortcut through the cleanroom", "The validation protocol says three runs. The schedule has room for one.",
            "Pharma Facilities", "CQV Coordinator", "project_management", "foundation", 7,
            """["quality_management","governance"]""",
            """
            {"context":"A sterile-suite HVAC qualification requires three consecutive conforming test runs per the validation master plan. The first run passed cleanly. The commissioning manager proposes counting supplier factory tests as the 'other two runs' to hand the suite over this week — 'the FAT data is the same test, same rig, same result'.",
             "evidence":[
               {"label":"Protocol","value":"3 consecutive conforming runs, on site"},
               {"label":"Status","value":"Run 1 passed"},
               {"label":"Proposal","value":"Count factory tests as runs 2 and 3"},
               {"label":"Context","value":"Client's QA audits validation records annually"}],
             "decisions":[
               {"key":"runs","prompt":"Your position?",
                "options":[
                  {"key":"protocol","label":"Run the remaining two on site as written — offering the real accelerators: night runs, pre-staged instruments, QA witness booked now","quality":100,
                   "consequence":"Runs 2 and 3 complete four days later than the shortcut promised; run 3 catches a damper actuator drift the factory rig could never have seen — which was the protocol's whole point.",
                   "principle":"Site qualification exists to test the installed system, which is the only one the product will ever meet."},
                  {"key":"fat","label":"Accept the FAT substitution — same test, same rig, engineering equivalence is real","quality":5,
                   "consequence":"The annual QA audit reads 'three consecutive runs', finds one, and invalidates the qualification; the suite re-validates during production, at production's cost.",
                   "principle":"A validation record that reinterprets its own protocol is a finding waiting for an auditor."},
                  {"key":"deviate","label":"Raise a formal deviation to amend the protocol to one site run plus FAT data","quality":45,
                   "consequence":"The legitimate route — and QA rejects it in review because installed-condition effects are the protocol's rationale; days spent, runs still owed.",
                   "principle":"The deviation process is the right door, but it opens onto the same technical question."}]}],
             "hints":["Ask what the site runs test that the factory runs cannot.",
               "Auditability is a property of records, not of engineering arguments made in corridors.",
               "Compress the schedule around the protocol, never through it."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held a three-run protocol and let run three earn its keep."}
            """),

        ("WC-QLT-162", "The finding that got friendlier", "Major nonconformity at the audit. 'Opportunity for improvement' in the report.",
            "Enterprise Programmes", "Assurance Analyst", "project_management", "foundation", 6,
            """["quality_management","governance"]""",
            """
            {"context":"You supported last month's internal audit of the programme's document-control process, where the lead auditor verbally graded the missing-approvals issue a major nonconformity. The published report — issued after 'management review' — records it as an 'opportunity for improvement' with no corrective-action deadline. The auditor has moved to another division and the audit manager says the wording was 'calibrated for tone'.",
             "evidence":[
               {"label":"Fieldwork","value":"Verbal grading: MAJOR nonconformity (missing approvals)"},
               {"label":"Published report","value":"'Opportunity for improvement', no CA deadline"},
               {"label":"Explanation","value":"Wording 'calibrated for tone' in management review"},
               {"label":"Your role","value":"You hold the fieldwork notes"}],
             "decisions":[
               {"key":"respond","prompt":"You do what with the discrepancy?",
                "options":[
                  {"key":"restore","label":"Raise it formally with the audit manager, fieldwork notes attached: either the evidence supports a major (restore it) or it doesn't (document why) — grading moves on evidence, not tone","quality":100,
                   "consequence":"The grading is restored with a 30-day corrective action; more usefully, 'management review' loses its quiet power to regrade findings without stating reasons.",
                   "principle":"An audit system where findings soften after fieldwork is an audit system that audits nothing."},
                  {"key":"accept","label":"Let it stand — tone management is a legitimate leadership prerogative","quality":5,
                   "consequence":"The unapproved-documents practice continues on its friendly timeline; the external certification audit six months later grades the same issue — and the internal report's wording — as two findings.",
                   "principle":"A softened finding postpones the fix and doubles the eventual embarrassment."},
                  {"key":"whistle","label":"Escalate straight to the audit committee as suppression","quality":30,
                   "consequence":"Possibly where it ends up — but skipping the audit manager converts a correctable process failure into an accusation, and your notes into ammunition rather than evidence.",
                   "principle":"Escalate through the chain the first time; over it only when the chain fails."}]}],
             "hints":["Compare what the evidence supported with what the report says — that gap is the issue.",
               "The precedent matters more than the instance: what may 'management review' change?",
               "Sequence the challenge: evidence first, chain of command second, committee if needed."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Restored an audit finding's grade — and closed the tone-calibration loophole."}
            """),

        ("WC-PRC-163", "Working on a letter, waiting on a contract", "The LOI expired nine days ago. The steel keeps arriving.",
            "Construction", "Assistant Commercial Manager", "project_management", "foundation", 5,
            """["procurement","commercial_management"]""",
            """
            {"context":"The steel frame subcontractor mobilised under a letter of intent capped at 250,000 while the subcontract was finalised. You notice the LOI expired nine days ago, the cap was passed last week (current committed value ~310,000), and the subcontract is still unsigned over two disputed clauses. Deliveries and erection continue daily.",
             "evidence":[
               {"label":"LOI","value":"Expired 9 days ago; cap 250,000"},
               {"label":"Committed value","value":"~310,000 and rising daily"},
               {"label":"Subcontract","value":"Unsigned — 2 clauses disputed"},
               {"label":"Site","value":"Deliveries and erection continuing"}],
             "decisions":[
               {"key":"act","prompt":"Today you:",
                "options":[
                  {"key":"regularise","label":"Flag it for same-day action: an extended, re-capped LOI signed by BOTH parties this week, a dated escalation path for the two clauses, and no further orders released beyond the new cap until the subcontract signs","quality":100,
                   "consequence":"Cover is restored within 48 hours; the clause dispute, now attached to a deadline, settles in a fortnight — and the exposure window closes at 310k instead of 500k.",
                   "principle":"Work continuing past an expired instrument is uninsured commerce — re-paper it the day you notice, not the week the dispute matures."},
                  {"key":"drift","label":"Let it run — the subcontract will sign eventually and everyone is behaving well","quality":5,
                   "consequence":"The disputed clause dispute hardens; with no instrument in force, the subcontractor's quantum claim starts from quantum meruit, which is their best position and your worst.",
                   "principle":"Every day worked without terms is negotiated leverage transferred to whoever gets paid."},
                  {"key":"stop","label":"Instruct the subcontractor to stop work until the subcontract is signed","quality":25,
                   "consequence":"Legally tidy; a demobilised steel gang and a crane on standby cost more per week than the clause dispute is worth, and goodwill leaves with the gang.",
                   "principle":"Stopping the work to fix the paper is usually dearer than fixing the paper at speed."}]}],
             "hints":["Establish what instrument, if any, currently governs the work — the answer may be 'none'.",
               "Size the exposure by its growth rate, not its current number.",
               "Put a deadline on the clause dispute — open-ended negotiation is what expired the LOI."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Re-papered an expired LOI before the exposure priced itself."}
            """),

        ("WC-PRC-164", "One quote, familiar face", "The renewal is due, the incumbent is good, and nobody asked the market.",
            "Enterprise Programmes", "Category Support Analyst", "project_management", "foundation", 7,
            """["procurement","commercial_management"]""",
            """
            {"context":"The programme's site-logistics contract — security, welfare, waste — renews next month at ~800,000/year. The delivery team has prepared a single-source renewal with the incumbent: 'excellent service, market testing would risk it, and there's no time anyway'. The incumbent's proposed uplift is 9%. Procurement policy allows single-source with justification; last market test was four years ago.",
             "evidence":[
               {"label":"Renewal","value":"~800,000/yr; incumbent proposes +9%"},
               {"label":"Justification","value":"'Excellent service; testing risks it; no time'"},
               {"label":"Policy","value":"Single-source permitted with justification"},
               {"label":"Last market test","value":"4 years ago"}],
             "decisions":[
               {"key":"renew","prompt":"Your recommendation?",
                "options":[
                  {"key":"benchmark","label":"A rapid benchmark, not a retender: three market price checks on the main service lines inside a fortnight — renew with the incumbent at a rate the evidence supports, and diarise a proper test for next cycle","quality":100,
                   "consequence":"The benchmark shows the market at +3–5%; the incumbent — told honestly — settles at 4% and keeps the contract everyone wanted them to keep, at a price the file can defend.",
                   "principle":"Single-source is a relationship decision; the PRICE still has to come from the market."},
                  {"key":"accept","label":"Accept the 9% — continuity of a good supplier is worth a premium","quality":15,
                   "consequence":"This year's 9% becomes the baseline for next year's 8%; by cycle three the 'continuity premium' compounds past what a full retender would have cost.",
                   "principle":"An unbenchmarked uplift teaches the incumbent what unchallenged means."},
                  {"key":"retender","label":"Insist on a full competitive retender — four years is too long regardless","quality":30,
                   "consequence":"Principled, late and rushed: a six-week tender for an 800k service produces thin bids, a grumpy incumbent, and a saving smaller than the disruption.",
                   "principle":"The right rigour at the wrong notice is just a different way to overpay."}]}],
             "hints":["Separate the WHO decision (defensible) from the HOW MUCH decision (untested).",
               "A price benchmark takes days; a retender takes months — match the tool to the clock.",
               "Book the real market test now for next cycle — urgency is only an excuse once."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Renewed the right supplier at a price the market actually supported."}
            """),

        ("WC-PRC-165", "The part that changed inside the box", "Same part number, same paperwork, different component.",
            "Industrial Manufacturing", "Supplier Quality Coordinator", "project_management", "foundation", 6,
            """["procurement","quality_management"]""",
            """
            {"context":"Goods-in inspection on a conveyor upgrade flags that the latest batch of drive couplings — same part number, same certificates — visibly differs from previous deliveries: different casting marks, lighter by 8%. The supplier, asked informally, says they 'optimised the supply chain' and the parts are 'fully equivalent'. Forty of the previous batch are already installed.",
             "evidence":[
               {"label":"Finding","value":"Same part number; different casting, -8% weight"},
               {"label":"Supplier","value":"'Optimised supply chain — fully equivalent'"},
               {"label":"Paperwork","value":"Certificates unchanged, no change notification"},
               {"label":"Exposure","value":"40 prior-batch units installed"}],
             "decisions":[
               {"key":"act","prompt":"Your move?",
                "options":[
                  {"key":"quarantine","label":"Quarantine the new batch, invoke the contract's change-notification clause, and require the equivalence evidence — material certs, dimensional and load test data — before any new unit is fitted","quality":100,
                   "consequence":"The evidence arrives and mostly holds — except a reduced fatigue rating that matters for two high-cycle positions; those keep the original spec, the rest proceed, and the supplier relearns what 'notification' means.",
                   "principle":"An unnotified change to a certified part voids the certainty the certificate existed to provide — equivalence is demonstrated, never declared."},
                  {"key":"trust","label":"Accept the assurance — suppliers optimise constantly and the paperwork is in order","quality":5,
                   "consequence":"The paperwork describes the old part; a lighter coupling in a high-cycle position fails at month seven, and the investigation's first finding is the unexamined substitution.",
                   "principle":"Paperwork that didn't change when the part did is not evidence — it is camouflage."},
                  {"key":"reject","label":"Reject the batch outright and demand the original component","quality":30,
                   "consequence":"Possibly unobtainable — the 'optimisation' may mean the original casting no longer exists; rejection without examining the evidence forfeits the workable path.",
                   "principle":"Refusing to evaluate a change is not the same as controlling it."}]}],
             "hints":["Treat the discrepancy as an unnotified change, not a quality defect — different clause, different lever.",
               "Ask which installed positions are sensitive to the property that changed.",
               "Equivalence has a test plan; ask for it by name."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Caught a silent part substitution at goods-in, not at month seven."}
            """),

        // ───────────── May · Stakeholder Dilemmas · single decision · practitioner ─────────────

        ("WC-STK-166", "The team that stopped saying no", "Delivery is on time. The people delivering it are not okay.",
            "Enterprise Programmes", "Workstream Lead", "project_management", "professional", 7,
            """["resource_management","leadership"]""",
            """
            {"context":"Your data-migration team has hit every milestone for four months. You also notice: weekend commits are now routine, two engineers have quietly cancelled leave, the team's meeting contributions have gone monosyllabic, and its best engineer asked HR — informally — about 'sabbatical policies'. The programme director, reading only the milestone reports, has just publicly praised the team as 'the model for the programme' and proposed giving them the next hard workstream too.",
             "evidence":[
               {"label":"Delivery","value":"4 months, every milestone hit"},
               {"label":"Signals","value":"Routine weekend commits · cancelled leave · silence in meetings"},
               {"label":"Sharpest signal","value":"Best engineer asking about sabbaticals"},
               {"label":"Incoming","value":"Director proposes adding the next hard workstream"}],
             "decisions":[
               {"key":"act","prompt":"Before the next workstream lands on them, you:",
                "options":[
                  {"key":"intervene","label":"Take the burnout evidence to the director as a delivery risk with numbers — overtime data, leave balances, attrition cost of the engineer at the door — and propose a deliberate recovery: enforced leave, a paced next assignment, and backfill hired before it, not after","quality":100,
                   "consequence":"The director, shown the same rigour used for schedule risks, staggers the next workstream and funds backfill; the engineer takes three weeks off and stays — the milestone streak survives because it stopped being paid for in people.",
                   "principle":"A team spending its people to hit dates is running an unbooked loan — report it like any other liability before it is called in."},
                  {"key":"ride","label":"Let the streak run — teams find their own rhythm, and interfering insults their achievement","quality":10,
                   "consequence":"The next workstream lands; the best engineer's sabbatical question becomes a resignation letter, two others follow within a quarter, and the model team becomes the cautionary tale.",
                   "principle":"Burnout never invoices in advance — by the time it costs you, it has already compounded."},
                  {"key":"shield","label":"Quietly decline the next workstream on the team's behalf without raising the burnout evidence","quality":30,
                   "consequence":"The immediate load is deflected; the director, unshown the reason, reads the decline as sandbagging and assigns the workstream anyway with a note about ambition.",
                   "principle":"Protecting a team secretly protects them once; making the cost visible protects them structurally."}]}],
             "hints":["Convert the soft signals into the hard currency the director already respects: risk and cost.",
               "The sharpest indicator is who is quietly pricing the exits.",
               "Recovery is a planned activity with resources — not a hope between milestones."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reported a team's burnout like the delivery risk it was — before the invoice arrived."}
            """),

        ("WC-STK-167", "The counteroffer conversation", "Your platform lead resigned this morning. You have one meeting to respond.",
            "Technology Programmes", "Delivery Manager", "project_management", "professional", 5,
            """["resource_management","leadership"]""",
            """
            {"context":"Your platform lead — architecture knowledge nobody else holds, cutover in nine weeks — resigned this morning for a 20% rise elsewhere. HR has approved a matching counteroffer if you want it. In the exit conversation she mentions, carefully, that the money 'was only part of it': she has flagged twice this year that she is the only person who can safely change the integration layer, and nothing happened.",
             "evidence":[
               {"label":"Resignation","value":"Platform lead; +20% offer elsewhere"},
               {"label":"Timing","value":"Cutover in 9 weeks"},
               {"label":"HR position","value":"Matching counteroffer approved"},
               {"label":"Her signal","value":"Flagged single-person dependency twice; nothing happened"}],
             "decisions":[
               {"key":"respond","prompt":"In the meeting, you:",
                "options":[
                  {"key":"address_cause","label":"Offer the match AND the thing she actually asked for: a funded plan — two engineers assigned to shadow the integration layer this quarter, her role redefined toward architecture leadership — with the first assignment made this week as proof","quality":100,
                   "consequence":"She stays — for the plan, not the money — and says so; nine months later the integration layer has three fluent engineers and her next move, whenever it comes, is survivable.",
                   "principle":"A counteroffer that only matches the money re-purchases the resignation's causes at a higher price."},
                  {"key":"match","label":"Make the clean counteroffer — money resigned her, money can retain her","quality":25,
                   "consequence":"She accepts; the dependency she warned about twice is still total, and the industry statistic about counteroffer acceptances finds her within the year — at ten weeks' notice instead of nine.",
                   "principle":"Retention bought without fixing the stated cause is a rental, and the rate goes up."},
                  {"key":"release","label":"Wish her well — counteroffers set bad precedents and nobody is irreplaceable","quality":15,
                   "consequence":"Principled, and the nine-week cutover now depends on a knowledge transfer compressed into a notice period; 'irreplaceable' turns out to have been an empirical claim.",
                   "principle":"Precedent arguments are luxuries; single-person dependencies at cutover are not."}]}],
             "hints":["Read the resignation as the third delivery of a message sent twice before.",
               "Price the nine-week exposure before pricing the salary match.",
               "Whatever she decides, the dependency plan is needed — start it this week regardless."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Answered a resignation with the fix she'd asked for twice — and kept both."}
            """),

        ("WC-STK-168", "The map that went viral", "The campaign group's flood map is wrong. Their followers don't know that.",
            "Climate Adaptation", "Programme Communications Lead", "project_management", "professional", 6,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"A local campaign group opposing your coastal-resilience scheme has published a map claiming the works will 'redirect flooding' toward two villages. The map misreads the model outputs — it uses the pre-scheme baseline as the post-scheme prediction. It has 40,000 shares; two parish councils have demanded meetings; a journalist has asked for comment by 5pm. Your modelling team is certain, and can show, the claim is wrong.",
             "evidence":[
               {"label":"The claim","value":"Works 'redirect flooding' to two villages — 40k shares"},
               {"label":"The error","value":"Pre-scheme baseline mislabelled as post-scheme prediction"},
               {"label":"Reactions","value":"2 parish councils demand meetings; journalist deadline 5pm"},
               {"label":"Your position","value":"Modelling team certain and able to demonstrate"}],
             "decisions":[
               {"key":"respond","prompt":"Your response strategy?",
                "options":[
                  {"key":"correct_respect","label":"Correct the record without attacking the correctors: publish the two maps side-by-side with a plain-language explanation, offer the campaign group a technical session WITH your modellers, and give the journalist the comparison — not a rebuttal of the group","quality":100,
                   "consequence":"The journalist runs the side-by-side; the campaign group, offered respect instead of ridicule, sends two members to the session and quietly amends its post — the villages' actual concern gets a real answer.",
                   "principle":"Correct the map, never the people holding it — a community shown respect can climb down; one shown contempt digs in."},
                  {"key":"rebut","label":"Issue a firm rebuttal naming the group's error and questioning their competence","quality":10,
                   "consequence":"Technically correct, strategically fatal: the story becomes 'programme attacks residents', the group's shares double, and every future consultation starts from the insult.",
                   "principle":"Winning the technical point by humiliating the community loses the programme."},
                  {"key":"ignore","label":"Don't amplify it — misinformation dies faster without official oxygen","quality":20,
                   "consequence":"Sometimes true for fringe claims; at 40,000 shares and two councils, the vacuum where your answer should be becomes the confirmation.",
                   "principle":"Past a visibility threshold, silence reads as inability to answer."}]}],
             "hints":["Diagnose the error precisely before choosing the tone — this one is demonstrable in one image.",
               "Separate the map's authors from its 40,000 sharers; you need the sharers.",
               "The parish councils are the real audience — the correction is for them."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Corrected a viral flood map without making 40,000 enemies."}
            """),

        ("WC-STK-169", "The secondee's other employer", "Half your planning capability is on loan. The lender wants it back mid-crisis.",
            "Public Programmes", "Deputy Programme Director", "project_management", "professional", 7,
            """["resource_management","stakeholder_communication"]""",
            """
            {"context":"Your census-systems programme runs on secondees; the planning function's deputy head — seconded from the statistics office — is the person holding the recovery plan for the current data-pipeline crisis. This morning her home department recalled her with two weeks' notice, citing its own inspection emergency. The secondment agreement allows recall with 'reasonable notice'. Your crisis peaks in roughly four weeks.",
             "evidence":[
               {"label":"Recall","value":"2 weeks' notice, contractually permitted"},
               {"label":"Her role","value":"Holds the pipeline-crisis recovery plan"},
               {"label":"Home department's reason","value":"Its own inspection emergency"},
               {"label":"Your crisis","value":"Peaks in ~4 weeks"}],
             "decisions":[
               {"key":"respond","prompt":"You:",
                "options":[
                  {"key":"negotiate","label":"Go director-to-director with a specific ask: a four-week extension OR a split arrangement (three days here through the peak), backed by what the census slippage costs THEIR minister too — and start an accelerated handover in parallel either way","quality":100,
                   "consequence":"The departments share her for three weeks — both emergencies are real, and framed as a joint ministerial exposure the split becomes obvious; the parallel handover means week four holds even if the deal collapses.",
                   "principle":"Secondment disputes are settled at the level that owns both risks — go there with a specific, split-able ask and a fallback already running."},
                  {"key":"comply","label":"Accept the recall — the agreement is clear and inter-departmental goodwill matters","quality":15,
                   "consequence":"Goodwill is preserved and the recovery plan leaves in a fortnight with its author; the crisis peak is managed from her handover notes, badly.",
                   "principle":"Contractual clarity about the notice does not resolve operational reality about the knowledge."},
                  {"key":"refuse","label":"Escalate a formal objection and refuse to release her before the peak","quality":10,
                   "consequence":"You cannot actually refuse — the agreement permits recall — so the objection burns the relationship AND loses the person, achieving the worst of both.",
                   "principle":"Never fight on ground the contract has already given away; trade instead."}]}],
             "hints":["Identify what the home department's emergency actually needs — it may not be all of her.",
               "Price your crisis in terms their leadership also owns — shared exposure funds shared solutions.",
               "Whatever the outcome, the handover starts today; negotiation and preparation are parallel tracks."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Split a recalled secondee across two emergencies — and kept the recovery plan's author through the peak."}
            """),

        ("WC-STK-170", "Forty-eight hours of darkness", "The upgrade needs the outage. The businesses need the notice they didn't get.",
            "Energy Networks", "Community & Stakeholder Manager", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"A substation upgrade requires a 48-hour planned outage for a trading estate — 34 businesses including two cold-storage operators. The notification letters, you discover today, were never sent: a mail-merge failure six weeks ago that nobody caught. The outage is in nine days; rescheduling it means losing the system operator's window and a three-month delay; the licence requires 21 days' notice.",
             "evidence":[
               {"label":"Outage","value":"48 hours, 34 businesses incl. 2 cold-storage"},
               {"label":"Failure","value":"Notification letters never sent — discovered today"},
               {"label":"Clock","value":"Outage in 9 days; licence requires 21 days' notice"},
               {"label":"Alternative","value":"Reschedule = lose window = 3-month delay"}],
             "decisions":[
               {"key":"respond","prompt":"Your recommendation?",
                "options":[
                  {"key":"honest_mitigate","label":"Report the breach to the licensing team TODAY, then door-knock all 34 businesses within 48 hours with tailored mitigation — generators for the cold stores, scheduling around trading patterns — and let the operator decide on rescheduling WITH the affected businesses' actual responses in hand","quality":100,
                   "consequence":"Thirty businesses accept with mitigation; the two cold stores get generators; the regulator, notified first by you, treats it as a self-reported process failure with exemplary response — the window survives.",
                   "principle":"When your process fails a community, the recovery is personal notice plus real mitigation — and the regulator hears it from you before anyone else."},
                  {"key":"proceed","label":"Proceed quietly — nine days is most of the notice period and letters get lost anyway","quality":0,
                   "consequence":"A cold-storage operator loses stock, complains to the regulator, and the investigation finds the unsent letters AND the decision to stay quiet about them — the second being the career-defining one.",
                   "principle":"A process failure is an incident; concealing one is a character reference."},
                  {"key":"delay","label":"Reschedule immediately — the licence breach makes proceeding impossible, full stop","quality":35,
                   "consequence":"Compliant and unexamined: the regulator's own guidance allows short-notice outages with consent and mitigation, which nobody asked the 34 businesses about before spending three months.",
                   "principle":"Read what the rule actually permits before paying its worst-case price."}]}],
             "hints":["Sequence matters: regulator first, businesses second, decision third.",
               "The two cold stores are the real risk — mitigation for them is the heart of any proceed case.",
               "Check whether consent-plus-mitigation routes exist before accepting the three-month price."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Governance Steward"},
             "share_line":"Recovered a failed outage notification with door-knocks, generators and the regulator on side."}
            """),

        // ───────────── May · Logic & Sequence · scope · foundation ─────────────

        ("WC-SCO-173", "The emergency that rewrote the scope", "The fault is fixed. The paperwork is a crime scene.",
            "Energy Networks", "Change Coordinator", "project_controls", "foundation", 6,
            """["change_control","scope_discipline"]""",
            """
            {"context":"A cable fault during your substation project forced emergency works last weekend: the site team, correctly, acted first — replacing a section outside the project's scope to restore supply. Monday brings the aftermath: unrecorded scope executed, materials drawn from project stock, 60 hours of unbooked labour, and an operations director asking whether 'the project can just absorb it since you were there anyway'. Put the record straight — in the right order.",
             "evidence":[
               {"label":"The works","value":"Out-of-scope cable section replaced — correctly, in emergency"},
               {"label":"Loose ends","value":"Unrecorded scope · project stock used · 60 unbooked hours"},
               {"label":"The ask","value":"'Absorb it — you were there anyway'"},
               {"label":"Governance","value":"Emergency-works provision exists in the contract"}],
             "decisions":[
               {"key":"sequence","prompt":"What is the right ORDER of repair?",
                "options":[
                  {"key":"record_first","label":"1) Capture the as-done record while memories are fresh — photos, hours, materials; 2) raise the emergency-works change to regularise it under the contract's provision; 3) route the cost question to the change board — absorb or recharge is THEIR call, made on a complete record","quality":100,
                   "consequence":"The record is captured in a day, the change regularises the works within the week, and the board recharges operations 70% — a decision nobody disputes because the evidence preceded the argument.",
                   "principle":"After an emergency: record, regularise, THEN argue about money — every step out of order loses evidence the last step needs."},
                  {"key":"absorb","label":"Absorb it as asked — goodwill with operations is worth 60 hours and some cable","quality":10,
                   "consequence":"The absorbed cost surfaces at the next cost review as an unexplained variance; 'goodwill' has no line in the cost breakdown structure, and the next emergency arrives with expectations attached.",
                   "principle":"Absorbing unrecorded scope converts one emergency into a standing invitation."},
                  {"key":"invoice","label":"Lead with the recharge demand to operations before regularising anything","quality":25,
                   "consequence":"The money argument starts with no agreed record behind it; operations disputes the hours, and the evidence that would have settled it is three weeks staler by the time anyone captures it.",
                   "principle":"An invoice built on an unrecorded event is an opinion with a total."}]}],
             "hints":["Ask which step's evidence decays fastest — that step goes first.",
               "The contract's emergency provision is the legitimate door; use it before the cost fight.",
               "Whose decision is absorb-or-recharge? Route it there with the record attached."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Put an emergency's paperwork back in the right order: record, regularise, then the money."}
            """),

        ("WC-SCO-174", "Thirty requests, one intake", "The change queue has become a suggestion box. Triage it properly.",
            "Enterprise Programmes", "Change Analyst", "project_controls", "foundation", 5,
            """["change_control","prioritization"]""",
            """
            {"context":"Your programme's change intake has accumulated thirty untriaged requests spanning: two contractual client instructions, five defect fixes mislabelled as changes, a dozen genuine scope-change candidates, eight 'improvements' with no sponsor, and three duplicates of items already decided. The change board meets Thursday and cannot process thirty items. Design the triage.",
             "evidence":[
               {"label":"Queue","value":"30 items, untriaged, oldest 7 weeks"},
               {"label":"Mix","value":"2 client instructions · 5 mislabelled defects · 12 real candidates · 8 unsponsored ideas · 3 duplicates"},
               {"label":"Board","value":"Thursday; realistic capacity ~10 items"},
               {"label":"Rule","value":"Client instructions have contractual response deadlines"}],
             "decisions":[
               {"key":"triage","prompt":"The correct sorting?",
                "options":[
                  {"key":"classify","label":"Route by NATURE first: instructions to the contractual track today (deadlines run); defects to the defect process (they were never changes); duplicates closed with references; unsponsored ideas returned for a sponsor; the twelve genuine candidates prioritised for Thursday","quality":100,
                   "consequence":"Thursday's board sees twelve real changes and decides ten; the instructions meet their deadlines; the queue's next thirty arrive pre-sorted because the intake now asks the routing questions.",
                   "principle":"Triage sorts by what each item IS before what it is worth — half of most change queues is not change."},
                  {"key":"fifo","label":"Oldest first — seven weeks of queue is unfair to early requesters","quality":10,
                   "consequence":"The board spends Thursday on elderly unsponsored suggestions while a client instruction's contractual deadline expires in the queue behind them.",
                   "principle":"First-in-first-out is fair to items and reckless with obligations."},
                  {"key":"value","label":"Biggest value impact first — the board's time should go to the biggest numbers","quality":30,
                   "consequence":"Sensible for the twelve genuine candidates, but applied to all thirty it ranks a mislabelled defect above a client instruction because someone typed a big number in the impact field.",
                   "principle":"Value-ranking is the SECOND sort; nature-routing is the first."}]}],
             "hints":["Look at what each item actually is before what it claims to be worth.",
               "Which items carry deadlines that run whether or not the board meets?",
               "An idea without a sponsor is not yet a change request — return it, don't rank it."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sorted a 30-item change queue by nature before value — and met the deadlines that mattered."}
            """),

        // ───────────── May · Executive Missions · capstone ─────────────

        ("WC-CAP-171", "The appetite reset", "After the near-miss, the board wants 'no more risk'. Your job is to translate that.",
            "Portfolio & PMO", "Portfolio Director", "project_management", "expert", 24,
            """["governance","risk_management","strategy_execution"]""",
            """
            {"context":"A near-miss on your portfolio's flagship project — a contractor insolvency caught days before a major payment — has shaken the board. The chair's instruction: 'we want no more risk of this kind'. You know literal risk elimination would stop the portfolio. Your mission: translate the board's fear into a workable risk-appetite reset, re-tool the portfolio to it, and rebuild the board's confidence in the machinery.",
             "evidence":[
               {"label":"Trigger","value":"Contractor insolvency near-miss; payment stopped days before"},
               {"label":"Board instruction","value":"'No more risk of this kind'"},
               {"label":"Portfolio reality","value":"14 projects, all with counterparty exposure of some size"},
               {"label":"Current framework","value":"Risk appetite last stated 3 years ago, in generalities"}],
             "decisions":[
               {"key":"translate","prompt":"Stage 1 — what do you take back to the board?",
                "options":[
                  {"key":"appetite","label":"A concrete appetite proposal: counterparty exposure limits by supplier rating band, mandatory financial-health monitoring above a threshold, and the COST of each tightening shown — letting the board choose its position on an informed curve","quality":100,
                   "consequence":"The board chooses tighter-but-priced limits, discovers 'no risk' would have cost 20% capacity, and owns the trade-off it made — which is what appetite means.",
                   "principle":"Boards say 'no more risk' when they mean 'show us the dial' — bring the dial, priced."},
                  {"key":"literal","label":"Implement the instruction literally: highest-rating counterparties only, parent guarantees everywhere, payment terms tightened across the board","quality":15,
                   "consequence":"Half the supply chain no longer qualifies; prices rise 12% as survivors price the terms; the board discovers the cost of its sentence eight months late and blames the translator.",
                   "principle":"Executing an emotional instruction literally is malpractice dressed as obedience."},
                  {"key":"wait","label":"Reassure the board the near-miss was caught by the controls working, and change nothing","quality":25,
                   "consequence":"True — and tone-deaf: the board's fear, unanswered, resurfaces as ad-hoc interventions in individual project decisions, which is appetite-setting by anecdote.",
                   "principle":"A board's fear is data; unprocessed, it becomes governance noise."}]},
               {"key":"retool","prompt":"Stage 2 — retooling the portfolio to the new appetite?",
                "options":[
                  {"key":"risk_based","label":"Tiered application: immediate review of the five largest counterparty exposures, new limits applied to new procurements at once, existing contracts brought in line at natural break points — with a small central capability to run the monitoring","quality":100,
                   "consequence":"The big exposures are treated in a quarter; nothing is torn up mid-delivery; the monitoring catches a second wobbling supplier within the year — this time as routine, not luck.",
                   "principle":"Apply new appetite with the grain of existing commitments — retrofit at break points, not mid-span."},
                  {"key":"big_bang","label":"Apply the new limits to everything at once, renegotiating existing contracts to comply","quality":10,
                   "consequence":"Forced renegotiations reopen fourteen settled deals; suppliers price the coercion, and two invoke termination clauses the portfolio wasn't ready to absorb.",
                   "principle":"Reopening settled contracts to satisfy a new policy converts policy risk into delivery risk."},
                  {"key":"new_only","label":"New procurements only — existing exposure ages out naturally","quality":35,
                   "consequence":"Clean and slow: the flagship's actual near-miss exposure — an existing contract — remains untreated for two more years by design.",
                   "principle":"A policy that exempts the risk that triggered it is a press release."}]},
               {"key":"confidence","prompt":"Stage 3 — rebuilding board confidence?",
                "options":[
                  {"key":"instrument","label":"A quarterly counterparty-health report the board actually reads: exposure vs limits, watch-list movements, actions taken — plus an annual appetite review with the same priced-dial format","quality":100,
                   "consequence":"The board's anxiety converts into literacy; a year later it LOOSENS one limit knowingly — the surest sign the machinery is trusted.",
                   "principle":"Confidence is rebuilt by instrumentation the board can read, not by assurances it must take on faith."},
                  {"key":"assure","label":"Commission an external assurance review to certify the new framework","quality":30,
                   "consequence":"The certificate reassures for a quarter; instrumentation would have reassured for a tenure — and cost less.",
                   "principle":"External assurance is a supplement to visibility, never a substitute."},
                  {"key":"quiet","label":"Let results speak — a quiet year will do more than any reporting","quality":15,
                   "consequence":"The quiet year happens; the board, unable to see WHY it was quiet, attributes it to luck and keeps intervening.",
                   "principle":"Invisible success builds no confidence — the machinery must be seen working."}]}],
             "hints":["Translate emotion into a priced dial before anyone implements a sentence.",
               "Sequence the retrofit with the grain of existing contracts — break points, not breakage.",
               "Design the reporting that lets the board watch the machinery work."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Translated a board's 'no more risk' into a priced appetite the portfolio could live with."}
            """),

        ("WC-CAP-172", "The incident on someone else's project", "A roof collapsed 200 miles away. Your programme uses the same design approach.",
            "Construction", "Programme Technical Director", "project_management", "expert", 22,
            """["governance","risk_management","stakeholder_communication"]""",
            """
            {"context":"A long-span roof under construction by another contractor — no connection to your programme — partially collapsed yesterday; early reporting points to the erection-stage temporary works methodology. Three of your programme's eight sites use a similar long-span approach, one with the roof erection starting in twelve days. Clients, insurers and your own engineers are all calling. The investigation will take months; your decisions cannot.",
             "evidence":[
               {"label":"The incident","value":"Partial collapse, another contractor, erection-stage temporary works implicated (early, unofficial)"},
               {"label":"Your exposure","value":"3 of 8 sites use similar long-span approach"},
               {"label":"Clock","value":"One roof erection starts in 12 days"},
               {"label":"Pressure","value":"Clients, insurers, engineers all calling; official findings months away"}],
             "decisions":[
               {"key":"technical","prompt":"Stage 1 — the technical response?",
                "options":[
                  {"key":"targeted","label":"A structured precautionary review of your three sites' temporary-works designs against the incident's KNOWN features — independent checker, defined scope, 10-day deadline — with the 12-day erection proceeding only on its specific clearance","quality":100,
                   "consequence":"The review clears two sites and finds a load-sequence assumption at the third worth strengthening — a two-day fix; erection starts on day 13 with an evidence trail every stakeholder can read.",
                   "principle":"Respond to someone else's incident with a targeted differential review — 'are WE exposed to THAT mechanism?' — not with paralysis or bravado."},
                  {"key":"continue","label":"Continue as planned — your designs were checked, and speculation about others' failures is not engineering","quality":10,
                   "consequence":"Technically defensible until the interim bulletin names the failure mechanism and a client asks, in writing, what you did in the twelve days you had it in the news.",
                   "principle":"'Our checks were fine' answers yesterday's question; an incident asks today's."},
                  {"key":"stop_all","label":"Suspend all three sites' roof works until the official investigation reports","quality":25,
                   "consequence":"Months of suspension for an investigation with no deadline; the precaution outspends the risk it addresses and two clients invoke delay clauses.",
                   "principle":"Indefinite suspension pending someone else's findings is outsourcing your engineering judgment to a timetable nobody controls."}]},
               {"key":"comms","prompt":"Stage 2 — clients and insurers?",
                "options":[
                  {"key":"proactive","label":"Brief all three clients and your insurers TODAY: the incident, your differential review's scope and deadline, and the decision rule for the imminent erection — before they ask","quality":100,
                   "consequence":"Every stakeholder hears method instead of silence; the insurer notes the response in your favour at renewal, and no client feels the need to impose its own review on top of yours.",
                   "principle":"In a sector-wide scare, the first credible plan a stakeholder hears becomes the reference — make sure it is yours."},
                  {"key":"reactive","label":"Respond fully to whoever asks, volunteer nothing — no need to alarm the quiet ones","quality":20,
                   "consequence":"The quiet ones hear it from the trade press instead, with your name absent from the responders; two impose client-side reviews with worse deadlines than yours.",
                   "principle":"Stakeholders you didn't brief get briefed by the situation."},
                  {"key":"reassure","label":"Issue a general assurance that your designs meet all standards","quality":15,
                   "consequence":"So did the collapsed roof's, presumably — the assurance ages badly against every subsequent finding.",
                   "principle":"Generic reassurance during an active investigation is a hostage to its findings."}]},
               {"key":"institutional","prompt":"Stage 3 — the longer arc?",
                "options":[
                  {"key":"learn","label":"Stand up a standing external-incident protocol: monitored sources, a differential-review template, decision rules and comms triggers — so the next sector incident is a procedure, not an improvisation","quality":100,
                   "consequence":"Eighteen months later a formwork incident elsewhere triggers the protocol; the review is done and clients briefed before the trade press calls — nobody remembers it as an event.",
                   "principle":"Other people's incidents are the cheapest safety data you will ever get — build the machinery that spends them well."},
                  {"key":"file","label":"Wait for the official report and update standards then","quality":30,
                   "consequence":"The report lands in fourteen months and is excellent; the machinery for reacting FAST to the next early signal still doesn't exist.",
                   "principle":"Learning only from final reports means always learning late."},
                  {"key":"distance","label":"Note it as another contractor's failure — different company, different controls","quality":5,
                   "consequence":"The mechanism didn't care whose logo was on the hoarding.",
                   "principle":"'It was them, not us' is the sentence every subsequent inquiry quotes."}]}],
             "hints":["Frame the review as differential: their mechanism versus your designs, scoped and dated.",
               "Move toward stakeholders before they move toward you — the first plan heard wins.",
               "Convert the scramble into a protocol while the motivation is fresh."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Answered someone else's collapse with a dated review, briefed clients and a standing protocol."}
            """),

        // ═════════════ JUNE — procurement, contracts, claims and supplier decisions ═════════════
        // ───────────── Daily Decisions · procurement · foundation ─────────────

        ("WC-PRC-175", "The answer that changed the question", "Your clarification response just made the tender mean something different.",
            "Enterprise Programmes", "Tender Manager", "project_management", "foundation", 6,
            """["procurement","governance"]""",
            """
            {"context":"Mid-tender for a facilities integration package, a bidder's clarification question exposes an ambiguity: the specification can be read as including OR excluding the legacy-system decommissioning — a 15% swing in scope. Your drafted answer resolves it (included). Three of six bidders have already priced; the deadline is in eight days.",
             "evidence":[
               {"label":"Ambiguity","value":"Decommissioning in or out — ~15% of scope"},
               {"label":"Your answer","value":"Resolves it: included"},
               {"label":"State","value":"3 of 6 bidders have already priced"},
               {"label":"Deadline","value":"8 days"}],
             "decisions":[
               {"key":"issue","prompt":"How do you issue the resolution?",
                "options":[
                  {"key":"all_extend","label":"Issue the clarification to ALL bidders as a formal addendum, with a deadline extension proportionate to the scope impact","quality":100,
                   "consequence":"All six price the same job; the extension costs a week and buys an award that survives challenge — because every bid answered the same question.",
                   "principle":"A clarification that changes scope is an addendum, and an addendum resets the clock for everyone equally."},
                  {"key":"asker","label":"Answer only the bidder who asked — the others read the spec the obvious way anyway","quality":0,
                   "consequence":"One bidder prices the real scope, five price a guess; the losing bidders' debrief discovers the asymmetry and the award dies in challenge.",
                   "principle":"Information given to one bidder is a competition given to one bidder."},
                  {"key":"silent","label":"Hold the answer — resolving it post-award with the winner avoids re-pricing chaos","quality":10,
                   "consequence":"The winner priced the exclusion reading; the 15% arrives as the contract's first variation, at post-award rates, with the ambiguity email in the claim bundle.",
                   "principle":"Ambiguity deferred to post-award is scope bought at monopoly prices."}]}],
             "hints":["Test whether the answer changes what bidders would PRICE — that is the addendum threshold.",
               "Whatever is said must reach all bidders identically and auditably.",
               "Price the extension against the cost of an award challenge or a day-one variation."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Turned a scope-changing clarification into a clean addendum for all six bidders."}
            """),

        ("WC-PRC-176", "The bid with the asterisk", "Lowest price, best quality score — and one quietly excluded obligation.",
            "Construction", "Procurement Coordinator", "project_management", "foundation", 7,
            """["procurement","commercial_management"]""",
            """
            {"context":"Evaluating groundworks bids, the leading tender — best price, best quality — carries one qualification: it excludes 'dealing with unexpected obstructions', which the contract places squarely with the contractor. The instructions to tenderers said qualifications 'may render bids non-compliant'. The evaluation panel wants to 'note it and sort it in contract finalisation'.",
             "evidence":[
               {"label":"Leading bid","value":"Best price AND quality — one exclusion"},
               {"label":"The exclusion","value":"Unexpected obstructions — contractually the contractor's risk"},
               {"label":"ITT wording","value":"Qualifications 'may render bids non-compliant'"},
               {"label":"Panel mood","value":"'Note it, sort it in finalisation'"}],
             "decisions":[
               {"key":"handle","prompt":"Your recommendation to the panel?",
                "options":[
                  {"key":"normalise","label":"Put the qualification through the tender's own machinery: require its withdrawal or a priced adjustment BEFORE evaluation concludes, so all bids are compared on the same risk allocation","quality":100,
                   "consequence":"The bidder withdraws the exclusion for a 2% adjustment; still cheapest, now comparable — and the ranking would survive any losing bidder's lawyer.",
                   "principle":"Bids are only comparable on identical risk allocations — normalise before ranking, never after award."},
                  {"key":"later","label":"Award as evaluated and negotiate the exclusion out during contract finalisation","quality":10,
                   "consequence":"Post-award, the leverage is gone: the 'negotiation' prices the obstruction risk at 6%, and second place — who included it — has grounds to ask why compliance was optional.",
                   "principle":"Whatever is unresolved at award is resolved at the winner's prices afterwards."},
                  {"key":"exclude","label":"Rule the bid non-compliant and evaluate the rest","quality":30,
                   "consequence":"Within the rules — and the best bid dies for a defect the ITT's 'may' gave you discretion to cure; the estate pays the second-best price for procedural tidiness.",
                   "principle":"Where discretion to cure exists, curing beats excluding — use the 'may' you wrote."}]}],
             "hints":["Ask what the exclusion would cost if it materialised — that is the bid's real price gap.",
               "Check what discretion the ITT wording actually gives you.",
               "Whatever you do with one qualification becomes precedent for every future bidder."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Governance Steward"},
             "share_line":"Normalised a qualified bid before ranking instead of paying for it after award."}
            """),

        ("WC-PRC-177", "Lunch with the incumbent", "A friendly invitation, a live tender, and a probity line you can't unsee.",
            "Enterprise Programmes", "Category Manager", "project_management", "foundation", 5,
            """["procurement","governance"]""",
            """
            {"context":"Your managed-services contract is out to competitive tender; the incumbent is bidding. Their account director — a genuinely good professional relationship over four years — invites you to 'a catch-up lunch, nothing about the tender, promise'. Your procurement policy requires all bidder contact during live tenders to go through the tender inbox. Declining feels rude; accepting feels wrong; the relationship matters whoever wins.",
             "evidence":[
               {"label":"Status","value":"Live tender; incumbent bidding"},
               {"label":"Invitation","value":"'Catch-up lunch, nothing about the tender'"},
               {"label":"Policy","value":"All bidder contact via tender channels during live process"},
               {"label":"Relationship","value":"4 good years; valuable whoever wins"}],
             "decisions":[
               {"key":"lunch","prompt":"Your reply?",
                "options":[
                  {"key":"decline_warm","label":"Decline warmly and explicitly: 'during the tender everything goes through the process — protects your bid as much as us; lunch is on me the week it concludes' — and log the contact per policy","quality":100,
                   "consequence":"The account director — a professional — respects it instantly; if their bid wins, it is unchallengeable, and if it loses, the lunch happens anyway.",
                   "principle":"Probity rules protect the compliant bidder most of all — declining properly is a favour to the relationship, not an insult."},
                  {"key":"accept","label":"Accept — grown professionals can have lunch without discussing a tender","quality":5,
                   "consequence":"Nothing improper is said; the losing bidder's challenge doesn't need anything to have been said — the lunch's existence, logged by whoever saw you, is the exhibit.",
                   "principle":"In a live tender, the appearance of access IS the breach — intent never gets a hearing."},
                  {"key":"ghost","label":"Quietly not reply until the tender concludes","quality":25,
                   "consequence":"The silence reads as coldness to a four-year relationship and teaches nothing about why; the next incumbent tries harder.",
                   "principle":"An unexplained no protects you once; an explained no protects the process permanently."}]}],
             "hints":["Ask how the invitation would look in a losing bidder's challenge bundle.",
               "The rule protects the incumbent's bid too — say so when declining.",
               "Log the contact; probity is a records discipline, not just a behaviour."],
             "profile_map":{"decision":"Governance Steward","balanced":"Executive Communicator"},
             "share_line":"Declined the incumbent's lunch in a way that strengthened both the process and the relationship."}
            """),

        ("WC-PRC-178", "Ninety days, ninety-one", "The bids expire tomorrow. The approval meeting is next week.",
            "Enterprise Programmes", "Procurement Analyst", "project_management", "foundation", 6,
            """["procurement","governance"]""",
            """
            {"context":"The security-services tender's bids carry a 90-day validity that expires tomorrow; the award approval — delayed twice by committee scheduling — is next Wednesday. Three bidders remain. Steel prices and wage rates have moved since submission. Your options memo is due today.",
             "evidence":[
               {"label":"Validity","value":"90 days, expiring tomorrow"},
               {"label":"Approval","value":"Committee next Wednesday — delayed twice already"},
               {"label":"Market","value":"Input prices moved since submission"},
               {"label":"Field","value":"3 bidders remain"}],
             "decisions":[
               {"key":"validity","prompt":"Your memo recommends:",
                "options":[
                  {"key":"extend_now","label":"Request written validity extensions from all three TODAY — same terms, same prices, defined new date — before expiry, noting any refusal as evaluation-relevant fact","quality":100,
                   "consequence":"Two extend unconditionally; the third extends with a 1.5% indexation caveat the committee can see and weigh — the competition survives intact and informed.",
                   "principle":"Ask for the extension while the bids are still alive — an expired bid is a negotiation, a valid one is a commitment."},
                  {"key":"lapse","label":"Let them lapse — bidders always honour their prices rather than lose the work","quality":10,
                   "consequence":"Two honour; the leader, whose costs genuinely moved, 'reconfirms' at +4% — legally free to, because you let the commitment die.",
                   "principle":"After expiry every price is voluntary, and voluntary prices track the market, not the tender."},
                  {"key":"rush","label":"Force an emergency approval today to beat the expiry","quality":30,
                   "consequence":"A same-day paper to a twice-delayed committee gets a deferral anyway — governance does not accelerate for procurement's calendar mistakes, and now the bids are dead too.",
                   "principle":"Fix the validity clock you control before gambling on the committee clock you don't."}]}],
             "hints":["Check who holds the commitment after expiry — the answer changes at midnight.",
               "An extension request is routine; a lapsed-bid reconfirmation is a renegotiation.",
               "A conditioned extension is information — put it in front of the decision-makers."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Kept three bids alive through a committee delay with a same-day extension request."}
            """),

        ("WC-PRC-179", "Money before metal", "The transformer maker wants 30% up front. The last supplier who asked went bust holding it.",
            "Energy Networks", "Contracts Officer", "project_management", "foundation", 7,
            """["procurement","commercial_management"]""",
            """
            {"context":"Your substation project's transformer supplier requests a 30% advance payment at order — 'standard for long-lead plant, covers our copper purchase'. It genuinely is common in the market. It is also how your organisation lost 400,000 two years ago when a different supplier entered administration holding an advance. The order must be placed within three weeks to hold the delivery slot.",
             "evidence":[
               {"label":"Request","value":"30% advance at order — 'covers copper purchase'"},
               {"label":"Market reality","value":"Advances genuinely common for long-lead plant"},
               {"label":"History","value":"400k lost to a supplier insolvency holding an advance"},
               {"label":"Clock","value":"Order within 3 weeks to hold the slot"}],
             "decisions":[
               {"key":"advance","prompt":"Your position on the advance?",
                "options":[
                  {"key":"secured","label":"Agree the advance AGAINST security: an advance-payment bond from their bank, or vesting of the copper and work-in-progress with marked storage and inspection rights","quality":100,
                   "consequence":"The supplier provides a bond for 1.1% — a price both sides consider cheap; the order lands in the window, and the advance is money with a parachute.",
                   "principle":"Advances are a normal market feature; UNSECURED advances are a normal insolvency loss — separate the two."},
                  {"key":"refuse","label":"Refuse all advances — corporate memory of 400k should mean something","quality":20,
                   "consequence":"The supplier, needing the copper cash flow, reprices +5% or slips the slot; the blanket rule pays more than the bond would have.",
                   "principle":"A scar is a reason for security, not a substitute for analysis."},
                  {"key":"pay","label":"Pay it plain — the supplier is reputable and the slot matters","quality":5,
                   "consequence":"Probably fine — as it was probably fine last time; 'reputable' is what every counterparty is until the administrators write.",
                   "principle":"Reputation is not collateral."}]}],
             "hints":["The question is never 'advance or no advance' — it is 'secured how'.",
               "Price the bond against the repricing that refusal would trigger.",
               "Vesting plus inspection turns paid-for materials into YOUR materials in an insolvency."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Secured a 30% advance with a bond instead of relying on reputation."}
            """),

        ("WC-PRC-180", "Clause 14.3, quietly evergreen", "The SaaS contract renews itself forever unless someone remembers a date.",
            "Technology Programmes", "Vendor Contracts Analyst", "project_management", "foundation", 5,
            """["procurement","commercial_management"]""",
            """
            {"context":"Reviewing the analytics platform contract ahead of signature, you find clause 14.3: automatic 12-month renewals at 'then-current list pricing' unless terminated with 6 months' notice — a notice window that will always fall mid-delivery, when nobody is reading contracts. Procurement wants signature this week; the vendor calls the clause 'completely standard'.",
             "evidence":[
               {"label":"Clause 14.3","value":"Auto-renewal at list price; 6-month notice to exit"},
               {"label":"Trap","value":"Notice window lands mid-delivery, every year"},
               {"label":"Vendor","value":"'Completely standard'"},
               {"label":"Pressure","value":"Signature wanted this week"}],
             "decisions":[
               {"key":"clause","prompt":"Before signature, you:",
                "options":[
                  {"key":"amend","label":"Negotiate the two words that matter: renewal at CAPPED pricing (index or fixed %) and notice reduced to 90 days — accepting the auto-renewal mechanism itself","quality":100,
                   "consequence":"The vendor concedes both in a day — they were priced for negotiation — and the contract renews conveniently instead of expensively; a diary entry covers the rest.",
                   "principle":"Auto-renewal is convenience; auto-renewal at LIST price with a long notice tail is a pricing mechanism wearing convenience's coat."},
                  {"key":"sign","label":"Sign as drafted — it is genuinely standard and the schedule wants the platform","quality":10,
                   "consequence":"Year 2 renews at list — 18% above the negotiated rate — during a cutover month when nobody saw the notice window close.",
                   "principle":"'Standard' describes how often a clause appears, not who it serves."},
                  {"key":"strike","label":"Demand removal of auto-renewal entirely — contracts should end when they end","quality":30,
                   "consequence":"The vendor resists hard (renewal is their revenue model), the negotiation eats three weeks, and manual renewal later lapses the service by accident.",
                   "principle":"Fight the price mechanism, not the convenience — pick the two words that move money."}]}],
             "hints":["Find where the clause moves money — it is rarely the renewal itself.",
               "Cap the price and shorten the notice; concede the mechanism.",
               "Whatever survives, diarise the notice window somewhere that outlives the project team."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Renegotiated the two words in an evergreen clause that actually moved money."}
            """),

        ("WC-PRC-181", "Start Monday, sign eventually", "The novation is 'a formality'. The subcontractor's insurer hasn't heard of it.",
            "Construction", "Assistant Contracts Manager", "project_management", "foundation", 6,
            """["procurement","governance"]""",
            """
            {"context":"Your design-and-build project inherits the client's steelwork subcontractor via novation — agreed in principle, papers with the lawyers. The programme wants the subcontractor mobilised Monday; the novation will complete 'in a week or two'. You check: until it completes, the subcontractor's contract — and its insurance obligations — still run to the CLIENT, not to you. On your site, under your CDM duties.",
             "evidence":[
               {"label":"Novation","value":"Agreed in principle; papers with lawyers, '1–2 weeks'"},
               {"label":"Programme","value":"Wants mobilisation Monday"},
               {"label":"Gap","value":"Until completion, their contract & insurances run to the client"},
               {"label":"Site","value":"Yours — your CDM duties, your incident book"}],
             "decisions":[
               {"key":"gap","prompt":"Monday's mobilisation?",
                "options":[
                  {"key":"bridge","label":"Mobilise WITH a bridge: a short interim works order between you and the subcontractor — insurances confirmed to you, site rules bound, terms mirroring the novation — dying automatically when the novation completes","quality":100,
                   "consequence":"Steel starts Monday; the week-three crane clip incident lands inside a contract that names you — instead of inside a legal seminar about whose subcontractor was on whose site.",
                   "principle":"Never let bodies mobilise ahead of the paper that answers 'who is responsible when it goes wrong TODAY'."},
                  {"key":"go","label":"Mobilise Monday — the novation is agreed in principle and everyone is acting in good faith","quality":5,
                   "consequence":"Good faith survives until the incident; the insurer's first question is contractual privity, and 'in principle' is not an answer they underwrite.",
                   "principle":"An agreement in principle allocates risk to whoever can least afford the ambiguity — usually you."},
                  {"key":"wait","label":"Hold mobilisation until the novation completes — no paper, no boots","quality":35,
                   "consequence":"Clean, and the 'week or two' becomes three; the steel window slips behind a following trade and the programme pays more than the bridge's drafting would have cost.",
                   "principle":"When a bridging instrument exists, refusing to build it is choosing the delay."}]}],
             "hints":["Ask who the subcontractor's obligations run TO on Monday morning, as drafted.",
               "The test scenario is an incident on day three — walk the liability chain.",
               "Interim instruments exist precisely for novation gaps; they take a day to draft."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Bridged a novation gap before the steel — and the liability — arrived on site."}
            """),

        ("WC-PRC-182", "The debrief that becomes evidence", "The losing bidder wants to know why. Every word you choose is discoverable.",
            "Enterprise Programmes", "Senior Procurement Officer", "project_management", "foundation", 7,
            """["procurement","governance"]""",
            """
            {"context":"The integration-partner award is decided; the runner-up — 2 points behind on 100 — has requested a debrief and is known to litigate procurements. Their bid scored lower on delivery methodology; one evaluator's scoring notes, you discover while preparing, contain a comment comparing the bidder unfavourably to 'their usual standard' — a criterion that appears nowhere in the ITT.",
             "evidence":[
               {"label":"Margin","value":"2 points in 100; methodology drove it"},
               {"label":"Requester","value":"Runner-up, litigious history"},
               {"label":"Discovery","value":"One evaluator's note references 'their usual standard' — not an ITT criterion"},
               {"label":"Task","value":"Debrief to prepare and deliver"}],
             "decisions":[
               {"key":"note","prompt":"The off-criterion note — what do you do with it?",
                "options":[
                  {"key":"test","label":"Test its materiality BEFORE the debrief: re-run that evaluator's scoring against the ITT criteria only — if the ranking holds, document the exercise; if it flips, escalate to the process owner before any award letter goes out","quality":100,
                   "consequence":"The re-score holds the ranking by 1 point — thin but genuine; the debrief proceeds on documented, criterion-based reasons, and the note has a recorded answer if it ever surfaces.",
                   "principle":"An off-criterion comment is a defect; whether it is a FATAL defect is a question you answer yourself, in writing, before your opponent asks it."},
                  {"key":"ignore","label":"Deliver the standard debrief — one stray comment in one evaluator's notes changes nothing","quality":5,
                   "consequence":"Disclosure in the ensuing challenge produces the note; 'changes nothing' becomes YOUR counsel's burden to prove, at hearing rates, with the process suspended.",
                   "principle":"The document you decided not to examine is the one the tribunal examines first."},
                  {"key":"rescore","label":"Quietly have the evaluator amend their notes to remove the comment","quality":0,
                   "consequence":"The amendment's metadata outlives everyone's career.",
                   "principle":"There is no procurement problem that document tampering does not convert into a personal one."}]}],
             "hints":["Read your own file as the challenger's lawyer would, before they do.",
               "Materiality is testable: does the ranking survive criterion-only scoring?",
               "Debrief content should be criterion-referenced, specific, and boringly consistent with the record."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Found the weak note in my own tender file and answered it before the challenger could."}
            """),

        ("WC-PRC-183", "Promises by the tonne", "The winning bid's social value is wonderful, local and arithmetically impossible.",
            "Public Programmes", "Social Value Officer", "project_management", "foundation", 5,
            """["procurement","governance"]""",
            """
            {"context":"The leisure-centre contract's preferred bidder scored heavily on social value: 14 local apprenticeships, 60% local-SME spend, and a schools programme. Reviewing before award, you calculate: the apprenticeship number exceeds what a contract of this size can host under the training body's supervision ratios, and the local-SME figure exceeds the region's relevant supply capacity. The commitments won the tender; the tender rules make them contractual.",
             "evidence":[
               {"label":"Commitments","value":"14 apprenticeships · 60% local-SME spend · schools programme"},
               {"label":"Your check","value":"Both figures exceed structural capacity (ratios; regional supply)"},
               {"label":"Status","value":"Preferred bidder; commitments become contractual at award"},
               {"label":"Scoring","value":"Social value decided the ranking"}],
             "decisions":[
               {"key":"act","prompt":"Before award, you:",
                "options":[
                  {"key":"verify","label":"Require a pre-award delivery plan for the commitments — ratios, named partners, phasing — through the tender's verification provisions; award proceeds when the plan reconciles or the bid is re-scored on what is actually deliverable","quality":100,
                   "consequence":"The bidder's plan reconciles 9 of 14 apprenticeships and 45% SME spend; re-scoring holds their win by a smaller margin — and the contract starts with commitments someone can actually keep.",
                   "principle":"A commitment that cannot be delivered is a scoring device, not social value — verify before it becomes both contractual and fictional."},
                  {"key":"award","label":"Award as scored — monitoring will catch shortfalls and remedies exist","quality":10,
                   "consequence":"Year one delivers 4 apprenticeships; the remedies clause was written for shortfalls, not for arithmetic impossibility, and the council member who championed the scoring asks who checked.",
                   "principle":"Monitoring an impossible promise just schedules the disappointment."},
                  {"key":"disqualify","label":"Recommend disqualification for material misrepresentation","quality":25,
                   "consequence":"Heavy-handed for what may be optimism rather than deceit — and the verification route the ITT already contains would have distinguished the two.",
                   "principle":"Use the process that separates ambition from misrepresentation before choosing which to allege."}]}],
             "hints":["Check the commitments against structural constraints, not just against other bids.",
               "The tender's own verification clauses are the legitimate pre-award instrument.",
               "Re-score on the deliverable version — the ranking must survive reality."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Reconciled an award-winning social-value promise with arithmetic before it became a contract."}
            """),

        ("WC-PRC-184", "The damages nobody believes", "The preferred bidder wants the LDs halved — 'they'd never survive anyway'.",
            "Transport Infrastructure", "Commercial Officer", "project_management", "foundation", 6,
            """["procurement","commercial_management"]""",
            """
            {"context":"In pre-contract finalisation for a depot electrification package, the preferred bidder challenges the liquidated damages rate: 'It's a penalty, it would never survive in court — halve it and we'll sign this week.' The rate was calculated from the operator's genuine standing costs — replacement bus hire, staff, lost grant milestones — and the calculation is on file. Signature pressure is real: the possession calendar needs the contract signed within a fortnight.",
             "evidence":[
               {"label":"Challenge","value":"'A penalty — halve it and we sign this week'"},
               {"label":"Your file","value":"LD rate built from documented standing costs"},
               {"label":"Law","value":"Genuine pre-estimates of loss are enforceable; penalties are not"},
               {"label":"Clock","value":"Possession calendar wants signature in 2 weeks"}],
             "decisions":[
               {"key":"lds","prompt":"Your response?",
                "options":[
                  {"key":"show","label":"Show the build-up: walk them through the standing-cost calculation, invite them to challenge any line — and hold the rate that survives that walk-through","quality":100,
                   "consequence":"Confronted with an evidenced pre-estimate, the 'penalty' argument evaporates — it only works against round numbers; they sign inside the week, and the LDs now carry MORE force for having been examined.",
                   "principle":"A liquidated damages rate you can derive is a rate you can keep — the challenge tests the file, not the number."},
                  {"key":"halve","label":"Halve it — LDs are rarely levied anyway and the signature matters more","quality":5,
                   "consequence":"Signed this week; eight months later the electrification slips six weeks and the halved rate recovers half the operator's real standing costs — the other half is your variance now.",
                   "principle":"Discounting damages you calculated honestly is donating the difference to the counterparty's schedule risk."},
                  {"key":"trade","label":"Trade it: hold the rate but add a longer grace period before LDs bite","quality":45,
                   "consequence":"Workable if the grace period is priced against the possession calendar — less workable discovered afterwards that the grace period covers exactly the slip they were already planning.",
                   "principle":"Trade structure for structure only when you know what the other side already knows."}]}],
             "hints":["The penalty argument attacks unjustified numbers — check whether yours is one.",
               "An LD rate with a derivation file is an asset; use the file, not your negotiating voice.",
               "Any concession here re-prices the counterparty's lateness, not your generosity."],
             "profile_map":{"decision":"Cost Guardian","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Defended a liquidated damages rate with its own derivation file."}
            """),

        // ───────────── June · quality & governance dailies · foundation ─────────────

        ("WC-QLT-185", "The certificate that lapsed in transit", "The sterile-consumables supplier's ISO cert expired between award and mobilization.",
            "Healthcare Estates", "Supplier Quality Officer", "project_management", "foundation", 5,
            """["quality_management","procurement"]""",
            """
            {"context":"Mobilising the hospital project's sterile-consumables supply contract, a routine document check finds the supplier's ISO 13485 certificate expired five weeks ago — after tender evaluation but before contract start. The supplier says recertification audit is 'booked and routine'; the certificate was a condition of award; first deliveries are due in twelve days.",
             "evidence":[
               {"label":"Finding","value":"ISO 13485 expired 5 weeks ago"},
               {"label":"Timeline","value":"Valid at evaluation; expired before start"},
               {"label":"Supplier","value":"'Recert audit booked, routine'"},
               {"label":"Contract","value":"Certification is a condition; deliveries in 12 days"}],
             "decisions":[
               {"key":"cert","prompt":"You:",
                "options":[
                  {"key":"bridge_verify","label":"Verify the audit booking with the certification body directly, require the supplier's interim quality evidence (last audit report, CAPA status), and gate first deliveries on either the renewed certificate or an enhanced incoming-inspection regime agreed with clinical quality","quality":100,
                   "consequence":"The body confirms the audit in 8 days; two deliveries run under enhanced inspection, the certificate renews, and the file shows the condition was managed rather than waived.",
                   "principle":"A lapsed certificate is a verification problem with a bridge, not a shrug — gate the risk, don't suspend the world or ignore it."},
                  {"key":"trust","label":"Accept 'booked and routine' — certificates lapse administratively all the time","quality":5,
                   "consequence":"The 'routine' audit raises two majors; the recertification takes eleven weeks, during which uncertified sterile products entered a hospital on your acceptance.",
                   "principle":"A condition of award that lapses quietly was either never needed or is being ignored — find out which before deliveries, not after."},
                  {"key":"suspend","label":"Suspend the contract until the certificate is renewed","quality":30,
                   "consequence":"Contractually available and clinically expensive: the wards' consumables gap is covered by spot purchases at triple cost, for a lapse a bridge could have managed.",
                   "principle":"Proportionality: verify and bridge before you suspend."}]}],
             "hints":["Verify with the certification body, not the certified.",
               "Ask what evidence substitutes for the certificate DURING the gap.",
               "The condition of award still matters — document how it is being met, not waived."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Bridged a lapsed ISO certificate with verification instead of trust or paralysis."}
            """),

        ("WC-QLT-186", "Approve it from the photos", "The benchmark mock-up is ready. The approver is offered a slideshow.",
            "Construction", "Quality Engineer", "project_management", "foundation", 6,
            """["quality_management","governance"]""",
            """
            {"context":"The facade package requires client approval of a physical benchmark mock-up before mass fabrication — the contract's quality anchor for 14,000 m² of cladding. The mock-up is ready at the fabricator's works, 300 km away. The architect proposes approving 'from the photo set and video walkthrough' to save the trip; fabrication slots are booked for Monday.",
             "evidence":[
               {"label":"Requirement","value":"Physical benchmark approval before mass fabrication"},
               {"label":"Stake","value":"Anchors quality for 14,000 m² of cladding"},
               {"label":"Proposal","value":"Approve from photos + video, skip the 300 km"},
               {"label":"Pressure","value":"Fabrication slots booked Monday"}],
             "decisions":[
               {"key":"benchmark","prompt":"Your advice?",
                "options":[
                  {"key":"attend","label":"Hold the physical review — colour, texture, joint tolerance and panel flatness are exactly the properties cameras flatter — and book the visit for Friday so Monday holds","quality":100,
                   "consequence":"Friday's inspection catches a sealant colour mismatch invisible in every photo; corrected in the benchmark for hundreds instead of across 14,000 m² for a number nobody says aloud.",
                   "principle":"A benchmark approved remotely benchmarks the photography, not the facade."},
                  {"key":"photos","label":"Accept the photo approval — modern imaging is good and the slots are booked","quality":5,
                   "consequence":"Mass fabrication reproduces what the camera could not show; the dispute at first delivery is now anchored to an approval YOUR file says was informed.",
                   "principle":"The approver who skipped the viewing owns what the viewing would have caught."},
                  {"key":"delegate","label":"Send a junior engineer with a checklist instead of the approver","quality":40,
                   "consequence":"Better than photos — but the contract names the client's approver for a reason: the judgment calls (acceptable variation in stone veining) aren't delegable by checklist.",
                   "principle":"Presence can be delegated; approval judgment mostly cannot."}]}],
             "hints":["List which properties the approval exists to judge — then ask if a camera transmits them.",
               "The cost comparison is one site visit versus 14,000 m² of reproduced error.",
               "Solve the calendar (go Friday) rather than the requirement (skip it)."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Insisted the facade benchmark be approved by eyes, not lenses."}
            """),

        ("WC-QLT-187", "Nine welders, six tickets", "Mobilization audit, day two: the quals folder is thinner than the crew.",
            "Energy Networks", "Site Quality Coordinator", "project_management", "foundation", 5,
            """["quality_management","governance"]""",
            """
            {"context":"Day two of pipeline-tie-in mobilization, your audit of the welding contractor finds nine welders on site and six current qualification records: two tickets expired last month, one welder's paperwork is 'following from the last job'. Production welding starts Thursday; the contractor's supervisor proposes the three 'crack on with fit-up work while the paperwork catches up'.",
             "evidence":[
               {"label":"Found","value":"9 welders, 6 current quals"},
               {"label":"Gaps","value":"2 expired last month; 1 'paperwork following'"},
               {"label":"Proposal","value":"Unverified three do fit-up while papers 'catch up'"},
               {"label":"Clock","value":"Production welding Thursday"}],
             "decisions":[
               {"key":"welders","prompt":"Your ruling?",
                "options":[
                  {"key":"gate","label":"The three do no welding — including tacks on production joints — until verified: expired pair sit requalification tests on site this week, the third's records verified with the issuing body TODAY; fit-up that involves no arc is fine","quality":100,
                   "consequence":"One requalifies Wednesday, one fails on overhead position — better discovered on a test coupon than a tie-in weld; the third's 'following' paperwork turns out to expire next week too.",
                   "principle":"A weld by an unverified welder is a defect with good intentions — the arc waits for the ticket, not the other way round."},
                  {"key":"flex","label":"Allow tacking and non-critical welds while paperwork catches up — Thursday matters","quality":5,
                   "consequence":"'Non-critical' tacks get buried in production joints by Friday; the NDT contractor's records now contain welds with no qualified welder against them, permanently.",
                   "principle":"There is no such thing as a temporary weld by an unqualified welder — metal doesn't know about paperwork timelines."},
                  {"key":"reject","label":"Stand down all nine until the contractor's quality system is re-audited","quality":25,
                   "consequence":"Six verified welders idle for three days over a records gap affecting three — the schedule pays for rigour the risk didn't ask for.",
                   "principle":"Gate the unverified, work the verified — precision beats blanket."}]}],
             "hints":["Separate the verified six from the unverified three — the ruling need not be uniform.",
               "On-site requalification is usually days, not weeks — start it immediately.",
               "'Paperwork following' is a claim; the issuing body answers phones."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Held three unverified welders at the gate and kept six working."}
            """),

        ("WC-GOV-188", "Award now, fund later", "The approval paper asks for a contract signature before the money exists.",
            "Enterprise Programmes", "Governance Analyst", "project_management", "foundation", 6,
            """["governance","procurement"]""",
            """
            {"context":"The network-refresh tender is evaluated and the award recommendation drafted — but the capital release for years 2 and 3 of the contract sits in next quarter's investment committee, not yet approved. The delivery director wants the three-year contract signed now to lock the pricing: 'the funding is a formality; it always comes through'. The contract has no break clause at year one.",
             "evidence":[
               {"label":"Award","value":"3-year contract ready; pricing locked if signed now"},
               {"label":"Funding","value":"Years 2–3 capital not yet approved — next quarter"},
               {"label":"Director's view","value":"'Funding is a formality'"},
               {"label":"Contract","value":"No year-one break clause as drafted"}],
             "decisions":[
               {"key":"commit","prompt":"Your governance advice?",
                "options":[
                  {"key":"structure","label":"Sign WITH the funding reality built in: a year-one break exercisable if the capital release fails, or funding-conditional years 2–3 — the vendor keeps most pricing certainty, the organisation never commits money it doesn't have","quality":100,
                   "consequence":"The vendor accepts a break clause for 1% on the option years; the capital release does come through — and the file shows the organisation never bet on it.",
                   "principle":"Commit to the term you have funded; buy an option on the term you haven't."},
                  {"key":"sign","label":"Sign the full term — the funding has never not come through","quality":5,
                   "consequence":"The quarter the funding pattern breaks — a spending freeze — the organisation holds a three-year obligation against one year of money, and the signature page holds your advice.",
                   "principle":"'It always comes through' is a base rate, not an authority."},
                  {"key":"wait","label":"Hold the award until the funding is approved — clean sequencing","quality":35,
                   "consequence":"Correct and costly: the pricing validity lapses in the wait, and next quarter's signature is 4% dearer than the break clause would have been.",
                   "principle":"Perfect sequencing has a price too — compare it with the structured alternative."}]}],
             "hints":["Match the commitment term to the approved funding term — then bridge the gap contractually.",
               "Price the break clause against both the repricing risk AND the unfunded-commitment risk.",
               "Whose risk appetite is 'it always comes through'? Not yours to sign on."],
             "profile_map":{"decision":"Governance Steward","balanced":"Cost Guardian"},
             "share_line":"Matched a three-year signature to one year of approved money — with a priced option on the rest."}
            """),

        // ───────────── June · Logic & Sequence · scope at procurement · foundation ─────────────

        ("WC-SCO-189", "Freeze it before you price it", "Five things must happen before tender issue. The team wants to do them in the wrong order.",
            "Enterprise Programmes", "Procurement Planning Analyst", "project_controls", "foundation", 7,
            """["scope_discipline","procurement"]""",
            """
            {"context":"The workplace-refit package goes to tender next month. Five pre-issue steps remain: design freeze sign-off, employer's-requirements drafting, scope-boundary agreement with the FM contract, cost-plan reconciliation, and tender-document assembly. The team, racing the date, proposes assembling tender documents now and 'slotting the rest in as they land'.",
             "evidence":[
               {"label":"Steps outstanding","value":"Design freeze · ERs drafting · FM boundary · cost reconciliation · document assembly"},
               {"label":"Proposal","value":"Assemble documents first, slot the rest in"},
               {"label":"Constraint","value":"ERs must reflect the frozen design; boundary feeds the ERs"},
               {"label":"Date","value":"Tender issue next month"}],
             "decisions":[
               {"key":"order","prompt":"The right sequence is:",
                "options":[
                  {"key":"logic","label":"Boundary agreement → design freeze → ERs drafted FROM the frozen design → cost plan reconciled against it → documents assembled last (they are the OUTPUT of the other four)","quality":100,
                   "consequence":"Each step feeds the next; the tender issues three days later than the racing plan promised and zero addenda follow — the compressed alternative historically averaged five.",
                   "principle":"Tender documents are the last step because they are the record of the other steps — assembling them first just documents the unfinished."},
                  {"key":"parallel","label":"Run all five in parallel — dependencies are for waterfall thinkers","quality":15,
                   "consequence":"The ERs are drafted against a design that then changes at freeze; the documents assemble two versions of the truth, and bidders find both.",
                   "principle":"Parallel work on sequential dependencies manufactures internal contradictions at speed."},
                  {"key":"assemble","label":"As proposed — assemble now, patch as things land","quality":5,
                   "consequence":"The tender issues on time containing TBCs; the addenda that follow re-price the job twice and the extension eats the time 'saved'.",
                   "principle":"Issuing on time with holes is issuing late with extra steps."}]}],
             "hints":["Trace which step's output is which step's input — the sequence writes itself.",
               "Ask what bidders would find if the documents assembled today.",
               "Three honest days late beats five addenda — count the whole cycle."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced five pre-tender steps by their data dependencies, not their deadlines."}
            """),

        ("WC-SCO-190", "Addendum or answer", "Six queries from bidders. Three change the job. Three don't. Sort them.",
            "Renewables", "Tender Coordinator", "project_controls", "foundation", 5,
            """["scope_discipline","procurement"]""",
            """
            {"context":"Mid-tender on a battery-storage balance-of-plant package, six bidder queries await response: (1) confirm the grid-connection voltage stated in the spec; (2) may HDPE duct substitute for the specified steel in the cable route?; (3) what is the site access route?; (4) does the employer's insurance cover delivery transit?; (5) can the completion date move two weeks for procurement lead times?; (6) clarify a drawing-note typo. Classify before answering: which are clarifications, which are CHANGES needing an addendum to all bidders?",
             "evidence":[
               {"label":"Q1, Q3, Q6","value":"Restate existing information (voltage, access, typo)"},
               {"label":"Q2","value":"Material substitution — changes the specification"},
               {"label":"Q4","value":"Risk allocation — changes who insures transit"},
               {"label":"Q5","value":"Changes the completion date — the schedule basis"}],
             "decisions":[
               {"key":"classify","prompt":"The correct routing?",
                "options":[
                  {"key":"split","label":"Q1/Q3/Q6 answered as clarifications to all; Q2, Q4 and Q5 treated as change candidates — decided internally first, then issued as addenda (or declined) to ALL bidders with deadline impact assessed","quality":100,
                   "consequence":"Bidders price one consistent job; the substitution is accepted as an addendum, the insurance stays as drafted, the date holds — all on the record, all symmetric.",
                   "principle":"A query that changes spec, risk or schedule is not answered — it is DECIDED, then published symmetrically."},
                  {"key":"answer_all","label":"Answer all six as clarifications — bidders asked questions, questions get answers","quality":10,
                   "consequence":"The 'answer' to Q2 quietly amends the specification for whoever read that response most carefully; the award inherits an asymmetry audit will find.",
                   "principle":"Answering a change as a clarification changes the tender for some bidders and not others."},
                  {"key":"defer_all","label":"Defer Q2/Q4/Q5 to post-award discussion with the winner","quality":20,
                   "consequence":"Bidders price three unknowns three different ways; the 'discussions' happen at single-supplier leverage on all three.",
                   "principle":"Whatever is undecided at tender close is negotiated after it — at worse prices."}]}],
             "hints":["Sort by what each answer would change: information, specification, risk, or schedule.",
               "Changes are decisions with owners — route them to the owner before any bidder hears.",
               "Symmetry is everything: all bidders, same information, same moment."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sorted six bidder queries into answers and addenda before replying to any."}
            """),

        ("WC-SCO-191", "Ready is a checklist, not a feeling", "Mobilization Monday. Order the readiness gates so the ones that gate actually gate.",
            "Public Estates", "Mobilization Coordinator", "project_controls", "foundation", 7,
            """["scope_discipline","governance"]""",
            """
            {"context":"A schools maintenance framework mobilises Monday. Outstanding readiness items: signed contract (with legal, 'imminent'); contractor's insurances verified; DBS clearances for operative access to occupied schools; asset-data handover from the outgoing contractor; helpdesk phone lines transferred. The incoming contractor proposes starting with 'low-risk jobs' Monday regardless — 'we'll be ready by the time it matters'.",
             "evidence":[
               {"label":"Outstanding","value":"Contract signature · insurance verification · DBS clearances · asset data · helpdesk transfer"},
               {"label":"Context","value":"Occupied schools — children present"},
               {"label":"Proposal","value":"Start low-risk jobs Monday, ready 'by the time it matters'"},
               {"label":"Reality","value":"Any job in an occupied school needs access; access needs DBS"}],
             "decisions":[
               {"key":"gates","prompt":"Which items actually gate Monday, in order?",
                "options":[
                  {"key":"ranked","label":"Hard gates first: signed contract and verified insurance gate ANY work; DBS gates any occupied-school access — so Monday's scope is exactly the jobs those three permit (empty sites, cleared operatives); asset data and helpdesk are service-level items managed on a dated plan","quality":100,
                   "consequence":"Monday starts legally, insured, and safeguarded — on a narrower front than hoped; the helpdesk transfers Wednesday and nobody ever has to explain an uncleared operative in a school corridor.",
                   "principle":"Readiness items divide into gates and gradients — know which is which before promising Monday."},
                  {"key":"start","label":"Start as proposed — momentum matters and the paperwork is genuinely imminent","quality":0,
                   "consequence":"An uninsured, uncontracted operative without DBS clearance changes a ballast in an occupied primary school on Monday morning; every word of that sentence is a separate governance failure.",
                   "principle":"'Low-risk jobs' is a claim about the work; the gates are about the worker, the contract and the child."},
                  {"key":"delay_all","label":"Delay the whole mobilization until every item is complete","quality":30,
                   "consequence":"Safe, and the outgoing contractor's demob date doesn't move — three days of no maintenance cover for 40 schools over items that never gated work at empty sites.",
                   "principle":"Blanket delay treats gradients as gates and pays gate prices for them."}]}],
             "hints":["For each item ask: what specifically cannot happen without it?",
               "Safeguarding items are absolute gates in occupied settings — no proportionality argument exists.",
               "Scope Monday to what the completed gates permit — a narrow start beats a late one or an illegal one."],
             "profile_map":{"decision":"Governance Steward","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sorted mobilization readiness into hard gates and managed gradients — and started safely on Monday."}
            """),

        // ───────────── June · Schedule Strategy · sequencing · practitioner ─────────────

        ("WC-SCH-192", "Order the orders", "Six packages, one design team, and lead times that don't care about your preferences.",
            "Construction", "Procurement Planner", "project_controls", "professional", 9,
            """["schedule_analysis","procurement"]""",
            """
            {"context":"A distribution-centre project must sequence six procurement packages against design-release capacity (one package's employer's requirements per fortnight) and site need dates. The packages: steel frame (26-week lead, needed week 40), roof & cladding (16-week, week 46), dock equipment (30-week, week 52), MEP (20-week, week 48), sprinklers (14-week, week 50), external works (8-week, week 44). The commercial team wants to tender 'biggest value first'.",
             "evidence":[
               {"label":"Design capacity","value":"1 package's ERs per fortnight"},
               {"label":"Steel","value":"26wk lead, need wk40 → order by wk14"},
               {"label":"Dock equipment","value":"30wk lead, need wk52 → order by wk22"},
               {"label":"MEP","value":"20wk lead, need wk48 → order by wk28"},
               {"label":"Roof/cladding · sprinklers · externals","value":"16/14/8wk leads, needs wk46/50/44"},
               {"label":"Commercial preference","value":"'Biggest value first'"}],
             "decisions":[
               {"key":"sequence","prompt":"The tendering order should be set by:",
                "options":[
                  {"key":"latest_start","label":"Required-order-date (need date minus lead time), scheduled back through the design-release constraint: steel first (wk14), dock equipment second (wk22), MEP third (wk28), then roof, sprinklers, externals by their own latest dates","quality":100,
                   "consequence":"Every package orders inside its window with design capacity never double-booked; the sequence looks odd to commercial — dock equipment before cladding — and works perfectly.",
                   "principle":"Procurement sequence is a scheduling calculation: latest order date through the release constraint — value has no lead time."},
                  {"key":"value","label":"Biggest value first — commercial attention where the money is","quality":10,
                   "consequence":"Steel (biggest) is fine by luck; dock equipment (fourth by value, longest lead) tenders in week 30 against a week-22 deadline, and the building finishes before its doors can load a lorry.",
                   "principle":"Ranking by value answers 'what matters most', not 'what must move first' — the schedule only asks the second."},
                  {"key":"easy","label":"Simplest packages first to build tendering momentum","quality":15,
                   "consequence":"External works tenders beautifully in week 10, twenty-six weeks before anyone could use the result; steel starts late and everything behind it inherits the slip.",
                   "principle":"Momentum on the wrong items is motion, not progress."}]}],
             "hints":["Compute each package's latest order date: need date minus lead time.",
               "Layer the design-release constraint: one ER set per fortnight is a queue — who must be first in it?",
               "Check the counterintuitive results twice; long leads on 'minor' packages surprise everyone."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Sequenced six tenders by latest-order-date arithmetic, not by size or ease."}
            """),

        ("WC-SCH-193", "First fence, then everything", "Twelve mobilization activities, one correct spine.",
            "Enterprise Programmes", "Site Mobilization Planner", "project_controls", "professional", 10,
            """["schedule_analysis","sequencing"]""",
            """
            {"context":"Planning a data-centre campus mobilization, twelve activities compete to be 'first': hoarding/fencing, site accommodation, power connection for the compound, drainage survey, topsoil strip, access road base, security systems, welfare facilities, LV distribution, spoil storage setup, wheel wash, signage. The subcontractors all want their item prioritised. Build the spine: what genuinely precedes what?",
             "evidence":[
               {"label":"Hard rules","value":"No workforce without welfare · no welfare without power & accommodation · no accommodation before fencing (security) · no muck-shift before wheel wash & spoil setup"},
               {"label":"Utilities","value":"Compound power precedes accommodation fit-out; drainage survey precedes topsoil strip"},
               {"label":"Everyone's claim","value":"'Ours is first'"}],
             "decisions":[
               {"key":"spine","prompt":"The mobilization spine is:",
                "options":[
                  {"key":"secure_power_people","label":"Fence → access road base → compound power → accommodation & welfare → security/signage → drainage survey → wheel wash & spoil setup → topsoil strip — secure the boundary, energise the compound, house the people, THEN move the earth","quality":100,
                   "consequence":"Each trade lands on a site that can legally and practically receive it; the earthworks start a week 'late' by the loudest subcontractor's reckoning and zero days late by the programme's.",
                   "principle":"Mobilization has a physics: boundary, then services, then people, then production — lobbying doesn't change which activities consume which."},
                  {"key":"parallel_all","label":"Mobilise everything in parallel — twelve crews, twelve fronts, maximum speed","quality":10,
                   "consequence":"The accommodation arrives before its power, the earthmovers before the wheel wash, and the unfenced compound loses a generator on night three.",
                   "principle":"Parallelism without precedence is how mobilizations mobilise chaos."},
                  {"key":"production_first","label":"Topsoil strip first — visible progress reassures the client","quality":15,
                   "consequence":"Muck leaves an unfenced site past no wheel wash onto a public road; the council's enforcement call reassures nobody.",
                   "principle":"Visible progress that violates its own prerequisites is visible liability."}]}],
             "hints":["List what each activity CONSUMES (security, power, welfare, access) — providers precede consumers.",
               "The workforce-welfare-power chain is usually the spine's core.",
               "Client-visible progress is a communication problem; solve it with communication, not sequence."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Built a mobilization spine from what-consumes-what, not who-shouts-loudest."}
            """),

        ("WC-SCH-194", "The possession chain", "Four approvals, one weekend, zero slack. Order them or lose the window.",
            "Rail Infrastructure", "Possession Planning Engineer", "project_controls", "professional", 8,
            """["schedule_analysis","sequencing"]""",
            """
            {"context":"Early works for a depot connection need a 54-hour weekend possession in 16 weeks. The dependency chain to secure it: possession application to the network operator (12-week statutory lead), which requires an approved safe-work pack, which requires the temporary works design, which requires the topographical survey — and the survey itself needs 2 weeks' track access via a separate minor-access request (3-week lead). The team has been treating these as five parallel tasks.",
             "evidence":[
               {"label":"Possession","value":"In 16 weeks; application lead 12 weeks"},
               {"label":"Chain","value":"Application ← safe-work pack ← temp works design ← survey"},
               {"label":"Survey","value":"Needs track access: separate request, 3-week lead + 2-week execution"},
               {"label":"Current state","value":"All five 'in progress in parallel'"}],
             "decisions":[
               {"key":"chain","prompt":"Reading the arithmetic, you:",
                "options":[
                  {"key":"backpass","label":"Run the backward pass and act on it TODAY: the access request must go in this week (3+2 weeks of survey + design + pack inside the 4 weeks before the application deadline) — the chain has days of float, not weeks, and the access request is the trigger everyone thought was parallel","quality":100,
                   "consequence":"The access request files Tuesday; the chain lands the application with three days' float, and the possession confirms — the 'parallel' plan would have missed the deadline by a fortnight discovered in week 10.",
                   "principle":"Approval chains are series circuits wearing parallel clothes — backward-pass them from the immovable date before believing any 'in progress'."},
                  {"key":"parallel","label":"Keep all five moving in parallel — overlap is how deadlines get met","quality":5,
                   "consequence":"Design 'progresses' against an unsurveyed alignment and reworks when the survey lands; the application misses the 12-week line and the possession moves to next quarter's calendar.",
                   "principle":"Work done ahead of its inputs is inventory of guesses."},
                  {"key":"expedite","label":"Ask the network operator to waive the 12-week lead given the project's importance","quality":15,
                   "consequence":"The operator's answer is the same one they give every 'important' project; the week spent asking was the week the access request needed.",
                   "principle":"Statutory leads are the fixed points — plan back from them, don't negotiate with them."}]}],
             "hints":["Anchor on the immovable date and walk backwards through every dependency.",
               "Find the item with the longest total chain — that is today's action, whatever its apparent urgency.",
               "'In progress' on a task whose inputs don't exist is not progress."],
             "profile_map":{"calculation":"Schedule Analyst","decision":"Evidence-Based Decision Maker","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Backward-passed a possession chain and found this week's hidden deadline."}
            """),

        // ───────────── June · Stakeholder Dilemmas · practitioner ─────────────

        ("WC-STK-195", "The mobilization team that hasn't finished demobilizing", "Your new project's A-team is still someone else's A-team for six more weeks.",
            "Enterprise Programmes", "Incoming Project Manager", "project_management", "professional", 6,
            """["resource_management","stakeholder_communication"]""",
            """
            {"context":"Your logistics-platform project mobilises in three weeks with a named core team — the same five people currently closing out another director's project, which has slipped six weeks. That director, senior to you, has told resourcing the team 'finishes what it started'. Your contract with the client names three of the five as key personnel from day one.",
             "evidence":[
               {"label":"Your mobilization","value":"3 weeks; 3 of 5 named as key personnel in the client contract"},
               {"label":"Their status","value":"Closing out a project that slipped 6 weeks"},
               {"label":"Other director","value":"Senior; 'the team finishes what it started'"},
               {"label":"Resourcing","value":"Caught between two instructions"}],
             "decisions":[
               {"key":"resolve","prompt":"You:",
                "options":[
                  {"key":"broker","label":"Convene both directors with a taper proposal: the three key-personnel names join you on contract day (client obligation), the other two follow at closeout milestones — with a named closeout support engineer backfilling the old project's tail","quality":100,
                   "consequence":"The contract obligation — the one fact neither director can argue with — anchors the deal; the old project's tail gets adequate cover, and neither director had to lose.",
                   "principle":"Resource standoffs settle fastest around external obligations — find the immovable fact and build the taper against it."},
                  {"key":"escalate","label":"Escalate to the portfolio director to rule between the two projects","quality":30,
                   "consequence":"A ruling arrives — for you, mostly — along with a senior colleague who lost a resourcing fight in front of their boss and remembers it at every future interface.",
                   "principle":"A brokered deal both sides own beats a ruling one side resents — escalate only when brokering fails."},
                  {"key":"wait","label":"Let the six weeks run — starting with a partial team is normal","quality":10,
                   "consequence":"Normal, except for the contract: the client's first governance meeting notes two absent key personnel, and your project opens with a breach letter instead of a kickoff.",
                   "principle":"Contractual key-personnel clauses convert resourcing preferences into obligations — read yours before conceding."}]}],
             "hints":["Find the fact that binds regardless of seniority — the key-personnel clause is it.",
               "A taper with backfill lets both projects be right at different times.",
               "Save the escalation card; a brokered outcome spends less relationship."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Split a contested team along a contract clause both directors had to respect."}
            """),

        ("WC-STK-196", "The village heard it from the winner", "The contractor announced their win on LinkedIn. The parish council had heard nothing.",
            "Energy Networks", "Consents & Community Manager", "project_management", "professional", 7,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"Your battery-storage project's construction contract was awarded Tuesday. Thursday morning, the winning contractor's LinkedIn post — 'thrilled to deliver this exciting project' — reaches the village Facebook group before your planned community letter, which was scheduled with the newsletter cycle for next week. The parish council chair, who chaired two constructive liaison meetings during consenting, emails: 'We had an understanding about hearing things first.'",
             "evidence":[
               {"label":"The leak","value":"Contractor's LinkedIn post, 2 days after award"},
               {"label":"Your plan","value":"Community letter scheduled next week with newsletter"},
               {"label":"The relationship","value":"2 constructive liaison meetings; an 'understanding'"},
               {"label":"The email","value":"'We had an understanding about hearing things first'"}],
             "decisions":[
               {"key":"repair","prompt":"Your response?",
                "options":[
                  {"key":"own_fast","label":"Call the chair TODAY — own the sequencing failure without blaming the contractor, bring forward the letter to tomorrow with construction-phase liaison arrangements, and add a communications protocol to the contractor's contract administration this week","quality":100,
                   "consequence":"The chair, called before lunch, stays a partner; the contractor gets a comms clause conversation; the village learns the details from your letter a day later — order restored, one apology cheap.",
                   "principle":"When a communication sequence breaks, repair the RELATIONSHIP first, the schedule second, the process third — same week, that order."},
                  {"key":"blame","label":"Reply explaining the contractor posted without authorisation — factually true","quality":20,
                   "consequence":"True and useless to the chair, who didn't ask whose fault it was; the village reads 'they can't control their own contractor' — a worse story than the original.",
                   "principle":"Attribution answers your need, not the stakeholder's — they asked whether the understanding still stands."},
                  {"key":"schedule","label":"Keep the letter on its newsletter cycle — accelerating it rewards the leak with panic","quality":10,
                   "consequence":"Five more days of the contractor's LinkedIn post being the village's only official information; the liaison meetings' capital drains by the day.",
                   "principle":"After a leak, your planned schedule is already broken — the only question is who fills the gap."}]}],
             "hints":["The chair's email is about the relationship, not the mechanics — answer that first.",
               "Speed of repair matters more than elegance; a call today beats a perfect letter next week.",
               "Fix the class of failure: contractor comms protocols belong in contract administration."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Executive Communicator"},
             "share_line":"Repaired a broken heard-it-first promise inside one news cycle."}
            """),

        ("WC-STK-197", "Two tenders, one bid manager", "Both directors booked the same person for the same fortnight. Neither will blink.",
            "Enterprise Programmes", "Head of Bid Management", "project_management", "professional", 7,
            """["resource_management","conflict_management"]""",
            """
            {"context":"Your best bid manager is claimed by two directors for overlapping must-win tenders: a framework renewal (deadline day 12) and a new-market bid (deadline day 14). Both cite prior claims; both have escalated to you in writing within an hour of each other. The bid manager, consulted privately, says the framework renewal is 'a known machine — strong second could run it with my review', while the new-market bid 'needs original thinking daily'.",
             "evidence":[
               {"label":"Clash","value":"Two must-wins, deadlines day 12 and 14"},
               {"label":"Directors","value":"Both claim priority, both escalated in writing"},
               {"label":"Expert's own read","value":"Renewal = 'known machine, second could run it' · New market = 'needs original thinking daily'"},
               {"label":"Bench","value":"One strong second-chair bid manager available"}],
             "decisions":[
               {"key":"allocate","prompt":"Your allocation?",
                "options":[
                  {"key":"fit","label":"Follow the work's nature, not the directors' volume: the second chairs the framework renewal with the expert's structured review at three checkpoints; the expert leads the new-market bid daily — and both directors hear the reasoning together, once","quality":100,
                   "consequence":"The renewal — genuinely a known machine — scores its usual marks under the second; the new-market bid gets the originality it needed and shortlists; the joint explanation prevents a season of relitigating.",
                   "principle":"Allocate scarce expertise by what the work actually requires — the person doing the work usually knows, if anyone asks."},
                  {"key":"seniority","label":"The framework renewal wins — it protects existing revenue and its director is senior","quality":20,
                   "consequence":"The expert polishes a machine that ran itself while the new-market bid — the one needing invention — gets the second chair, and the shortlist announcement shows it.",
                   "principle":"Seniority-based allocation optimises for the org chart, not the outcome."},
                  {"key":"split_days","label":"Split the fortnight — alternate days on each bid","quality":10,
                   "consequence":"Two bids each get half a distracted expert at their most intense phase; both directors, briefly placated, jointly furious by day 10.",
                   "principle":"Splitting a person across two deadline sprints delivers two half-sprints."}]}],
             "hints":["Ask the expert which work actually needs THEM — the answer is usually honest and specific.",
               "Structured review of a known process is delegable; daily invention is not.",
               "Announce the reasoning to both claimants at once — separate explanations breed separate grievances."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Evidence-Based Decision Maker"},
             "share_line":"Allocated a contested expert by the shape of the work, not the seniority of the shouting."}
            """),

        ("WC-STK-198", "The local firm and the local paper", "The hometown bidder lost fairly. Tomorrow's front page won't say so.",
            "Events & Venues", "Programme Director's Adviser", "project_management", "professional", 5,
            """["stakeholder_communication","governance"]""",
            """
            {"context":"The conference-centre refit contract went to a national firm; the losing local contractor — a prominent employer whose owner sits on the chamber of commerce — has told the local paper the process 'favoured outsiders'. The journalist calls for comment on tomorrow's story. The evaluation was clean: the local bid scored lower on programme certainty and price. A standard debrief was offered and not yet taken up.",
             "evidence":[
               {"label":"The claim","value":"'Process favoured outsiders' — to local press"},
               {"label":"The record","value":"Clean evaluation; local bid lower on certainty and price"},
               {"label":"Status","value":"Debrief offered, not yet taken"},
               {"label":"Deadline","value":"Journalist wants comment today"}],
             "decisions":[
               {"key":"respond","prompt":"Your advice on the comment?",
                "options":[
                  {"key":"process_facts","label":"A short factual comment — criteria published in advance, evaluation followed them, debrief offered and remains open — plus a same-day private call to the losing owner renewing the debrief offer BEFORE the story runs","quality":100,
                   "consequence":"The story runs balanced ('council defends process; firm offered debrief'); the owner takes the debrief, learns exactly where the bid fell short, and bids better — and quieter — next time.",
                   "principle":"Answer a fairness attack with the process's own artifacts, and give the aggrieved party a private route to the facts before the public one hardens."},
                  {"key":"detail","label":"Rebut in detail — release the scores showing exactly why the local bid lost","quality":10,
                   "consequence":"Publishing a bidder's weaknesses to their hometown paper converts a process complaint into a public humiliation — and breaches the confidence every future bidder relies on.",
                   "principle":"Winning the argument by disclosing a bidder's failings loses every future bidder's trust."},
                  {"key":"decline","label":"'No comment on procurement matters' — dignified silence","quality":20,
                   "consequence":"The story runs on the complaint alone; 'declined to comment' sits under the headline doing exactly what it always does.",
                   "principle":"In a fairness story, silence is scored as evidence for the accuser."}]}],
             "hints":["The public answer defends the PROCESS; the private call serves the PERSON — do both, today.",
               "Never litigate a specific bid's weaknesses in public, even winning.",
               "The debrief offer is your best exhibit — renew it where the journalist can see."],
             "profile_map":{"decision":"Executive Communicator","balanced":"Governance Steward"},
             "share_line":"Answered a hometown-bias headline with process facts and a private door."}
            """),

        // ───────────── June · Executive Missions · capstone ─────────────

        ("WC-CAP-199", "Make, buy, or regret", "The plant expansion's central question, asked properly for once.",
            "Industrial Manufacturing", "Programme Procurement Director", "project_management", "expert", 24,
            """["procurement","strategy_execution","governance"]""",
            """
            {"context":"Your food-processing group's three-plant expansion needs its delivery model decided: the process lines are the group's crown jewels (in-house engineering knows them cold), but the group has never delivered three sites in parallel. The gateway wants your position on the make-or-buy split, the contract strategy for whatever is bought, and how to protect the group's process IP through whichever route wins.",
             "evidence":[
               {"label":"Capability","value":"In-house team: deep process knowledge, never run 3 parallel sites"},
               {"label":"Market","value":"Two integrators could deliver turnkey; both work for competitors too"},
               {"label":"Scope shape","value":"Process lines (differentiating) · utilities/buildings (commodity)"},
               {"label":"Constraint","value":"All three sites needed inside 30 months"}],
             "decisions":[
               {"key":"split","prompt":"Stage 1 — the make-or-buy split?",
                "options":[
                  {"key":"core_edge","label":"Split by differentiation: process-line design and commissioning stay in-house (the crown jewels), buildings and utilities bought as packages, with integrator support bought AS CAPACITY under your team's direction for the parallel-site load","quality":100,
                   "consequence":"The in-house team leads what makes the group different and rents arms and legs for what doesn't; three sites run in parallel without the crown jewels ever leaving the building.",
                   "principle":"Make what differentiates, buy what doesn't, and rent capacity — not control — for the surge."},
                  {"key":"turnkey","label":"Buy turnkey — three parallel sites is exactly what integrators are for","quality":15,
                   "consequence":"The integrator delivers competently and now understands your process lines as well as you do; their next client's plant looks familiar in ways nobody can litigate.",
                   "principle":"A turnkey contract for differentiating scope is a technology transfer with invoicing."},
                  {"key":"inhouse","label":"All in-house — hire up for the parallel load; nobody outside touches anything","quality":25,
                   "consequence":"The hiring market delivers half the engineers the plan needs; month 10 finds three sites sharing two teams, and the 30-month window closing on the slowest.",
                   "principle":"Protecting everything with capacity you don't have protects nothing on schedule."}]},
               {"key":"contract","prompt":"Stage 2 — contract strategy for the bought scope?",
                "options":[
                  {"key":"portfolio","label":"One buildings/utilities framework across all three sites — common design, sequenced awards, volume leverage — with site-level call-offs and shared learning provisions","quality":100,
                   "consequence":"Site 2's building prices 8% under site 1's on repetition; the framework's shared-learning clause moves the site-1 snag list into site-2's kickoff.",
                   "principle":"Parallel similar sites are a portfolio — contract them like one and collect the repetition dividend."},
                  {"key":"separate","label":"Three separate site contracts — local markets, local competition, clean interfaces","quality":30,
                   "consequence":"Three competitions, three learning curves, zero shared lessons; site 3 makes site 1's mistakes at site 1's prices.",
                   "principle":"Treating repeatable work as unique forfeits the only discount parallel delivery offers."},
                  {"key":"single_mega","label":"One contractor for everything at all three sites — one throat to choke","quality":20,
                   "consequence":"The single throat prices its indispensability by month 12; 'choke' turns out to be mutual.",
                   "principle":"Concentration for convenience becomes leverage against you at the first variation."}]},
               {"key":"ip","prompt":"Stage 3 — protecting the process IP through delivery?",
                "options":[
                  {"key":"architecture","label":"Protect by ARCHITECTURE, not just paper: integrator capacity works on your systems under your engineers' direction, process-critical parameters compartmentalised, contracts carrying confidentiality WITH audit rights and key-personnel non-poaching","quality":100,
                   "consequence":"The rented engineers see pieces, never the recipe; two years later a competitor's integrator-built plant conspicuously lacks the yield the group's parameters deliver.",
                   "principle":"IP leaks through work organisation before it leaks through documents — design the work so the secret never assembles outside your walls."},
                  {"key":"nda","label":"Strong NDAs and confidentiality clauses — the standard legal armour","quality":25,
                   "consequence":"The NDAs are signed and sincere; the knowledge walks anyway, in the heads of engineers who genuinely can't un-know how the lines balance.",
                   "principle":"Paper protects against bad faith; work design protects against human memory."},
                  {"key":"secrecy","label":"Share nothing — integrator staff work from redacted drawings only","quality":15,
                   "consequence":"Redacted drawings produce redacted quality: the capacity you rented can't do useful work blind, and the schedule pays for the paranoia.",
                   "principle":"Protection that prevents the work isn't protection; it is a slower way to fail."}]}],
             "hints":["Split scope by what differentiates the business, not by what is biggest.",
               "Three similar sites are one portfolio — price the repetition.",
               "IP protection is mostly work design: who sees enough to reassemble the recipe?"],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Split a three-plant expansion into make, buy and rent — and kept the recipe home."}
            """),

        ("WC-CAP-200", "The platform and the exit", "Choosing the vendor is easy. Choosing how you'll leave them is the decision.",
            "Technology Programmes", "Chief Procurement Adviser", "project_management", "expert", 22,
            """["procurement","strategy_execution","governance"]""",
            """
            {"context":"Your organisation is procuring the core operations platform for the next decade — a two-horse race between an integrated suite (deep functionality, deep lock-in) and a composable best-of-breed stack (flexibility, integration burden). The board asks for your recommendation on the architecture choice, the commercial protections whichever wins, and the honest answer to 'how would we ever leave?'",
             "evidence":[
               {"label":"Option A","value":"Integrated suite — best functionality fit, proprietary data model, few reference exits"},
               {"label":"Option B","value":"Composable stack — standard interfaces, 30% more integration effort, swappable parts"},
               {"label":"Horizon","value":"10-year platform decision"},
               {"label":"Board question","value":"'How would we ever leave?'"}],
             "decisions":[
               {"key":"architecture","prompt":"Stage 1 — the architecture recommendation?",
                "options":[
                  {"key":"fit_with_exits","label":"Recommend on TOTAL lifecycle terms: the suite's functionality lead is real, so recommend it — CONDITIONAL on contractually-secured exit assets (data model documentation, standard-format export rights, escrow) priced into the deal before signature, when competition still exists","quality":100,
                   "consequence":"The suite wins on merits with exit rights bought at tender prices — the vendor concedes in competition what they would never concede in renewal; the board gets function AND a documented door.",
                   "principle":"Choose on fit; contract for exit — and buy the exit while the vendor still has a rival in the room."},
                  {"key":"flexibility","label":"Recommend the composable stack — lock-in risk outweighs functionality on a 10-year horizon","quality":35,
                   "consequence":"Defensible — and the 30% integration burden compounds annually into its own lock-in: by year 4 the bespoke integration layer is harder to leave than any vendor.",
                   "principle":"Composability's exit story assumes someone maintains the doors; integration debt welds them shut just as surely."},
                  {"key":"function","label":"Recommend the suite on functionality — lock-in is tomorrow's problem and every platform has it","quality":15,
                   "consequence":"Tomorrow arrives at first renewal: +22%, take it or leave it, and 'leave it' has no documented meaning.",
                   "principle":"A ten-year decision made on year-one criteria is a nine-year regret."}]},
               {"key":"commercial","prompt":"Stage 2 — the commercial protections?",
                "options":[
                  {"key":"renewal_math","label":"Fix the renewal arithmetic NOW: price caps indexed on renewals, usage-band pricing agreed to year 10, benchmarking rights with teeth, and service credits that escalate — all while two bidders still exist","quality":100,
                   "consequence":"Year-5's renewal is a formula, not a negotiation; the benchmarking clause gets exercised once, corrects pricing 9%, and is never needed again — its existence did the work.",
                   "principle":"Every protection is cheap at tender and unbuyable at renewal — the competition window is the only leverage you will ever have."},
                  {"key":"standard","label":"Take the vendor's enterprise agreement with legal's standard amendments","quality":10,
                   "consequence":"The standard amendments protect against standard problems; the renewal cliff, usage redefinitions and module re-bundling were all non-standard by design.",
                   "principle":"The vendor's paper is optimised for the vendor's decade, not yours."},
                  {"key":"short","label":"Sign short — 3 years, renegotiate often, stay agile","quality":25,
                   "consequence":"Three years is exactly when switching costs peak and alternatives haven't matured: each 'renegotiation' is a renewal with theatre.",
                   "principle":"Short terms without exit assets just schedule your weakest negotiating moments more often."}]},
               {"key":"exit","prompt":"Stage 3 — the honest answer to 'how would we leave?'",
                "options":[
                  {"key":"rehearsed","label":"An exit that is MAINTAINED, not just contracted: annual data-export verification, the exit runbook kept current, switching costs re-estimated at each renewal and reported to the board as a standing figure","quality":100,
                   "consequence":"The board's annual pack carries a real number for 'cost to leave'; the vendor, aware the number exists and is current, prices renewals against a credible alternative for a decade.",
                   "principle":"An exit you never rehearse is a clause, not a capability — the credible THREAT of leaving is what you're actually maintaining."},
                  {"key":"contracted","label":"'The contract gives us export rights and escrow — we can leave if we must'","quality":30,
                   "consequence":"Year 7 tests the theory: the export runs for the first time ever, produces a format nobody has parsed, and the 'if we must' turns out to cost 18 months.",
                   "principle":"Exit rights unexercised decay like any other untested capability."},
                  {"key":"honest_never","label":"'Realistically, we never leave — platforms this deep are marriages; let's optimise the relationship instead'","quality":20,
                   "consequence":"Honest, and self-fulfilling: the vendor's account team hears it within a quarter, and every subsequent negotiation prices your candour.",
                   "principle":"Declaring you'll never leave sets the price of staying."}]}],
             "hints":["The moment of maximum leverage is before signature — inventory what must be bought then.",
               "Lock-in comes from data models and integration debt as much as contracts.",
               "An exit is a capability with a maintenance schedule, not a clause with a signature."],
             "profile_map":{"decision":"Strategic Programme Leader","balanced":"Strategic Programme Leader"},
             "share_line":"Bought the platform on fit and the exit on competition — then kept the exit alive."}
            """),
    };
}
