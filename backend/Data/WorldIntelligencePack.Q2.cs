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
    };
}
