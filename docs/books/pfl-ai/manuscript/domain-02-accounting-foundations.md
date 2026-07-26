# Domain 2 — Accounting and Financial-Statement Foundations

> **Group:** Foundations (Domain 2 of 4 in Part One). **Target:** ~70 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain builds the accrual-to-cash bridge Domain 1 promised and
> supplies the statement vocabulary every later domain assumes — including the `CFADS` that
> Domain 10 turns into coverage ratios. Standards are named and described in this book's own words;
> no standard's text is reproduced, and nothing here is accounting advice for a specific entity
> or jurisdiction. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domain 1 established that **cash, not profit, is the binding constraint** — and then left an
obligation outstanding: if profit is not the thing that pays debt service, why does anyone compute
it, and how do you get from one to the other? This domain discharges that obligation. It builds
the accrual model and what recognition actually means (KA 2.1); assembles the three statements and
the articulation that binds them into one system (KA 2.2); works the treatments that decide a
project's reported numbers — working capital, revenue and cost recognition, capital versus
operating expenditure, provisions (KA 2.3); and closes with ratio interpretation and the
interfaces between project reporting and corporate accounts (KA 2.4). A project finance leader is
not an accountant, and this domain does not try to make one. It makes something more specific: a
leader who can read a set of statements, find the cash inside them, and tell when an accounting
choice has changed the picture without changing the economics.

**Learning objectives.** After this domain a candidate can: explain accrual accounting and the
recognition tests that drive it, and quantify the divergence between the two bases on one period's
trading; describe each of the three statements and what question it answers; **articulate** a
statement set — complete a balance sheet from the movements, prove it balances, and derive the
cash-flow statement by both the indirect and the direct method to the same figure; explain how
working capital consumes cash, compute its effect on `CFADS` and therefore on a coverage ratio, and
restate a coverage covenant as a collection-period threshold; measure progress on an over-time
contract by an input and an output method and compute the recognition consequence, including the
immediate charge on an onerous contract; distinguish capital from operating expenditure, quantify
the profit effect while showing pre-tax cash is unaffected, and identify the one circumstance in
which cash is *not* unaffected; measure a decommissioning provision at present value and compute
its first-year accretion and depreciation; interpret a full ratio set, including the leverage
identity that links return on capital to return on equity and the comparison against Domain 9's
`WACC`; quantify the five meanings of "spend" and reconcile them to one identity; explain deferred
tax and compute a deferred tax liability; and govern AI-assisted analysis of financial statements.

**The master statements.** Kestrel Water SPC — whose loan, appraisal and financing decision
Domains 1, 3 and 4 built — now reports its **first full operating year**. The plant cost
**USD 60,000,000** (Domain 4's `I₀`), depreciated straight-line over **25 years**. The senior loan
is Domain 3's **USD 42,000,000 at 6.0 % over 12 years**, so year-one interest is
**USD 2,520,000** and the annual instalment is **USD 5,009,635.23**, of which **2,489,635.23** is
principal. Revenue is **USD 12,000,000**, cash operating costs **USD 4,500,000**, and tax is charged
at **20 %**. The opening balance sheet at the commercial operations date is plant **60,000,000**,
cash **nil**, senior debt **42,000,000** and equity **18,000,000**, with no receivables or payables.
Every figure in KA 2.1–2.4 derives from these.

---

## Knowledge Area 2.1 — The accrual model

*Topics: 2.1.1 accrual versus cash accounting · 2.1.2 recognition · 2.1.3 the statements as one
system.*

### 2.1.1 Accrual versus cash accounting

**Definitions.** **Cash accounting** records a transaction when money moves. **Accrual
accounting** records the *effects* of transactions when they occur, regardless of when money
moves — revenue when it is earned, expenses when they are incurred. Accrual is the basis of
general-purpose financial reporting because cash timing is a poor guide to performance: a project
that invoices in December and collects in February did the work in December, and a cash-basis
account would report a loss followed by a windfall.

The two bases answer different questions, and a finance leader needs both:

| Basis | Answers | Where it governs |
|---|---|---|
| **Accrual** | Did we perform profitably in this period? | Statutory accounts, covenants defined on profit, tax |
| **Cash** | Can we pay what falls due? | Debt service, drawdowns, liquidity, `CFADS` (Domain 10) |

**The professional consequence.** Accrual and cash diverge, and the divergence is *information*,
not noise. A project reporting healthy profit while operating cash falls is telling you something
precise — usually that working capital is absorbing the difference (KA 2.3.1). Domain 1's
worked example was exactly this case; this domain now shows the machinery that produces it.

**Worked example 2.1.1 — the same year on both bases, quarter by quarter.**

1. **Setup.** Kestrel's first operating year, before financing and tax, so that the two bases are
   compared on trading alone. Revenue accrues evenly at **USD 3,000,000** a quarter and cash
   operating costs at **USD 1,125,000** a quarter, giving accrual `EBITDA` of **USD 1,875,000**
   every quarter and **USD 7,500,000** for the year. Collections lag: receivables close the four
   quarters at **1,200,000 · 1,050,000 · 950,000 · 900,000**, having opened at nil, and payables at
   **450,000 · 400,000 · 350,000 · 300,000**, also from nil. Compute `EBITDA` on a cash basis for
   each quarter and for the year.
2. **Formula.** Cash received = revenue − Δreceivables. Cash paid = cash operating cost −
   Δpayables. Cash-basis `EBITDA` = cash received − cash paid. Divergence = cash-basis −
   accrual `EBITDA`.
3. **Substitution.** Quarter one: `3,000,000 − 1,200,000` received; `1,125,000 − 450,000` paid.
   Quarter two: `3,000,000 − (1,050,000 − 1,200,000)` received; `1,125,000 − (400,000 − 450,000)`
   paid. Quarters three and four follow the same pattern on their own movements.
4. **Result.**

   | Quarter | Accrual `EBITDA` | Cash received | Cash paid | Cash-basis `EBITDA` | Divergence |
   |---|---|---|---|---|---|
   | Q1 | 1,875,000 | 1,800,000 | 675,000 | 1,125,000 | **(750,000)** |
   | Q2 | 1,875,000 | 3,150,000 | 1,175,000 | 1,975,000 | **+100,000** |
   | Q3 | 1,875,000 | 3,100,000 | 1,175,000 | 1,925,000 | **+50,000** |
   | Q4 | 1,875,000 | 3,050,000 | 1,175,000 | 1,875,000 | **nil** |
   | **Year** | **7,500,000** | **11,100,000** | **4,200,000** | **6,900,000** | **(600,000)** |

5. **Interpretation.** The two bases disagree by **600,000** over the year, and the disagreement is
   not an opinion about performance — it is the closing net working-capital position, receivables
   900,000 less payables 300,000, to the dollar. That is the invariant worth carrying out of this
   Knowledge Area: **cumulative accrual result minus cumulative cash result equals the net
   working-capital balance on the balance sheet**, which is why an unexplained divergence is always
   findable and always sits in a named account. Three further readings. First, the *pattern* of the
   quarters is the information, not the annual total: quarter one diverges by 750,000 because the
   trading cycle is being built from nothing, and quarters two to four converge as it stabilises —
   a start-up profile, not a deterioration, and a reader who saw only quarter one would draw the
   opposite conclusion. Second, **quarter four is the quarter in which the two bases agree, and it
   agrees for a reason that has nothing to do with performance**: receivables and payables each fell
   by 50,000, so the net movement was nil. Agreement between profit and cash is evidence that
   working capital did not move, and nothing more. Third, cash accounting is not the conservative
   choice it is often taken to be — it is merely **late**. It reported 600,000 less than accrual
   this year, and it will report 600,000 more in the year the trading cycle stops growing or
   unwinds, because over the life of the project the two bases must sum to the same number. The
   professional caution is the mirror image: a project whose cash result flatters its accrual
   result is often a project in decline, releasing working capital as volumes fall — the effect
   Domain 15 (KA 15.1.2) tracks through the operating phase, where it makes a deteriorating project
   report an improving covenant ratio.

### 2.1.2 Recognition

**Definition.** **Recognition** is recording an item in the statements as an asset, liability,
income or expense. It is governed by tests, not by preference, and the tests are the substance of
what accounting standards do. Described in principle (IFRS is the framework referenced throughout
this book by name):

- An **asset** is a present economic resource controlled by the entity as a result of past events —
  recognised when it exists and can be measured reliably. Control, not legal title, is the
  operative idea.
- A **liability** is a present obligation to transfer an economic resource as a result of past
  events. A *future* intention is not a liability, however certain it feels.
- **Income and expenses** are recognised as the underlying changes in assets and liabilities
  occur, which is why revenue recognition follows performance (KA 2.3.2) rather than invoicing.

**Measurement** then asks *at what amount* — historical cost, amortised cost, fair value — and the
choice materially changes reported figures without changing any cash flow. That sentence is the
whole reason a finance leader reads statements sceptically: **recognition and measurement policies
are part of the answer, so they must be part of the question.**

**Applying the tests.** The tests are worth more as a procedure than as prose, because the same
four questions dispose of almost every item a project throws up. Take them in order — *is there a
present obligation or resource? did a past event create it? is settlement or inflow probable? can
it be measured reliably?* — and stop at the first "no". Six items from Kestrel's own first year,
each disposed of by a different failed test:

| Item | Present obligation / resource? | From a past event? | Probable? | Reliably measurable? | Treatment |
|---|---|---|---|---|---|
| Unpaid supplier invoices for delivered chemicals | yes | yes | yes | yes | **Recognise** a payable |
| Membrane replacement expected in year seven | no — no obligation to a third party yet | — | — | — | **Nothing**; a future cost, disclosed only in commentary |
| Site-restoration obligation created by the concession | yes | yes, on construction | yes | yes, at present value | **Recognise** a provision (KA 2.3.4) |
| Offtaker's disputed abatement claim assessed as possible | yes, contingent | yes | **no** — possible, not probable | yes | **Disclose** as a contingent liability |
| Insurance recovery on a pump failure, insurer not yet confirmed | resource, contingent | yes | probable but not virtually certain | yes | **Disclose** as a contingent asset; do not recognise |
| Board's resolution to fund a year-two upgrade | **no** — an intention | — | — | — | **Nothing** |

Two points the table makes better than a paragraph can. The **asymmetry is deliberate**: the
contingent liability is disclosed on *probable* while the contingent asset is recognised only on
*virtually certain*, so an entity in an identical factual position on both sides of a dispute
reports the downside earlier than the upside. And **the failing test names the argument**: when two
professionals disagree about an item, they are almost never disagreeing about all four questions,
and identifying which one is contested — usually probability, occasionally measurement — converts
an impasse into a specific evidential question. Which framework applies, and how these tests are
expressed within it, is a matter for the entity's finance function and auditors; the *procedure* is
transferable, the conclusion is not.

### 2.1.3 The statements as one system

The three primary statements are not three reports; they are three views of one set of facts,
locked together by identities:

```
Assets = Liabilities + Equity                              (the balance sheet identity)
Closing equity = Opening equity + Profit − Distributions   (+ capital contributed)
Closing cash   = Opening cash + Operating + Investing + Financing cash flows
```

Because they share one underlying record, a change in one propagates to all three — the property
called **articulation**, demonstrated arithmetically in KA 2.2.4. Articulation is the reader's
single most powerful tool: a statement set that does not articulate contains an error or an
omission, and finding where it fails localises the problem immediately.

### AI in this KA

Statement analysis is a genuine AI strength — extracting line items, restating across periods,
flagging unusual movements — and it has a specific, dangerous blind spot: a model reads the
*numbers* fluently and the *policies* not at all. Two projects with identical economics and
different capitalisation or revenue-recognition policies produce different statements, and an
assistant comparing them will report a difference in performance that does not exist. The governed
habit: ask what policy produced each figure before comparing any two entities or periods, and
verify extracted line items against the source statement. **AI proposes; the professional
verifies, decides and remains accountable.**

### Key terms — KA 2.1

| Term | Meaning |
|---|---|
| **Accrual basis** | Recognising the effects of transactions when they occur, not when cash moves. |
| **Recognition** | Recording an item in the statements, subject to definition and measurement tests. |
| **Measurement** | The amount at which a recognised item is carried (cost, amortised cost, fair value). |
| **Articulation** | The locking of the three statements to one underlying record via identities. |
| **Accounting policy** | The permitted choice that shapes reported figures without changing cash. |

### Sample MCQs — KA 2.1

**MCQ 2.1-A `[2.1.1 · Analysis]`** A project reports rising profit and falling operating cash
flow over three quarters, with no change in accounting policy. The most likely explanation is:
- A. the profit figures must be erroneous
- B. working capital is absorbing cash — receivables and/or inventory are growing faster than payables ✅
- C. depreciation has increased
- D. the two measures are unrelated, so no explanation is needed

*Rationale:* Accrual profit and operating cash diverge principally through working-capital
movements (KA 2.3.1). C would *raise* operating cash relative to profit (a non-cash charge added
back), and D denies the articulation the statements are built on.

**MCQ 2.1-B `[2.1.2 · Application]`** A sponsor board has firmly resolved to fund a plant upgrade
next year. At this year end this is:
- A. a liability, because the decision is certain
- B. not a liability — there is no present obligation from a past event; an intention is not an obligation ✅
- C. a provision, because the amount is estimable
- D. a contingent asset

*Rationale:* Liability recognition requires a *present* obligation arising from a past event
(2.1.2). Certainty of intent is irrelevant; a provision (C) still requires an obligation to exist,
and D inverts the direction entirely.

**MCQ 2.1-C `[2.1.3 · Recall]`** A statement set where the cash-flow statement's closing cash does
not equal the balance sheet's cash indicates:
- A. a normal difference in presentation
- B. an error or omission — the statements articulate to one record, so they must reconcile ✅
- C. the use of accrual rather than cash accounting
- D. a foreign-currency effect that requires no action

*Rationale:* Articulation is an identity (2.1.3). FX and presentation differences are disclosed
and reconciled, not left unbalanced — an unreconciled set is a defect, and locating the break is
the reader's fastest diagnostic.

**MCQ 2.1-D `[2.1.1 · Analysis]`** Over its first year a project reports accrual `EBITDA` of
7,500,000 and cash-basis `EBITDA` of 6,900,000. Receivables closed at 900,000 and payables at
300,000, both having opened at nil. The 600,000 difference is best described as:
- A. an error, since the two measures should agree over a full year
- B. the closing net working-capital balance — the divergence is the balance sheet, and it reverses when the trading cycle stops growing ✅
- C. evidence that the accrual figures are optimistic
- D. a timing difference that will never reverse

*Rationale:* Cumulative accrual result less cumulative cash result equals net working capital
(2.1.1), so the divergence is locatable in named accounts and is a growth profile rather than a
quality-of-earnings verdict. A denies the identity; C reads a timing effect as a judgment; D is
wrong because the balances unwind — cash accounting is late, not conservative.

**MCQ 2.1-E `[2.1.2 · Evaluation]`** An insurer has indicated it will probably meet a claim, and
the amount is reliably estimable. The entity also faces a counterclaim it assesses as probable and
estimable. The correct treatment of the pair is:
- A. recognise both, since both are probable and estimable
- B. recognise neither, since both are disputed
- C. recognise the counterclaim as a provision; disclose the insurance recovery, which requires virtual certainty to be recognised ✅
- D. net the two and recognise the difference

*Rationale:* The recognition thresholds are deliberately asymmetric — probable for an obligation,
virtually certain for a contingent asset (2.1.2) — so an entity in the same factual position on
both sides reports the downside first. A applies one threshold to both; B ignores that the
counterclaim passes its tests; D offsets two items that arise from different events, which the
recognition tests are applied to individually.

### Self-check — KA 2.1

1. *Which basis governs debt service, and which governs covenants defined on profit?* — Cash for
   debt service; accrual for profit-defined covenants. A leader needs both.
2. *Why is an intention never a liability?* — Recognition requires a present obligation arising
   from a past event.
3. *What does a failure to articulate tell you?* — That there is an error or omission, and where
   the break occurs localises it.
4. *What does the cumulative gap between accrual and cash results equal?* — The net
   working-capital balance: 900,000 receivables less 300,000 payables = 600,000.
5. *When profit and cash agree in a period, what has that told you?* — That working capital did
   not move. It is not evidence of quality either way.

---

## Knowledge Area 2.2 — The three statements and their articulation

*Topics: 2.2.1 the income statement · 2.2.2 the balance sheet · 2.2.3 the cash-flow statement ·
2.2.4 articulation demonstrated.*

### 2.2.1 The income statement

**What it answers:** did the entity perform profitably in the period? Its project-finance-relevant
structure runs down from revenue through the layers a lender reads:

**Worked example 2.2.1 — Kestrel's first operating year.**

1. **Setup.** Revenue USD 12,000,000; cash operating costs USD 4,500,000; plant USD 60,000,000
   depreciated straight-line over 25 years; senior interest USD 2,520,000 (Domain 3's year-one
   figure); tax 20 %.
2. **Formula.** `EBITDA` = revenue − cash operating costs; `EBIT` = `EBITDA` − depreciation;
   profit before tax = `EBIT` − interest; net income = PBT − tax. Depreciation =
   (cost − residual)/useful life.
3. **Substitution.** `EBITDA = 12,000,000 − 4,500,000`; depreciation `= 60,000,000/25`;
   `EBIT = 7,500,000 − 2,400,000`; `PBT = 5,100,000 − 2,520,000`; tax `= 2,580,000 × 0.20`.
4. **Result.**

   | Line | USD |
   |---|---|
   | Revenue | 12,000,000 |
   | Cash operating costs | (4,500,000) |
   | **EBITDA** | **7,500,000** |
   | Depreciation | (2,400,000) |
   | **EBIT** | **5,100,000** |
   | Interest | (2,520,000) |
   | **Profit before tax** | **2,580,000** |
   | Tax at 20 % | (516,000) |
   | **Net income** | **2,064,000** |

5. **Interpretation.** Four layers, four different questions. **`EBITDA`** approximates the cash
   the operation generates before financing, tax and the accounting for past capital spend — which
   is why lenders start there (Domain 10). **`EBIT`** charges the asset's consumption, so it
   measures operating performance including capital intensity. **PBT** is after the cost of the
   capital structure. **Net income** is the shareholders' accounting return — and note it is
   USD 2,064,000 while `EBITDA` is USD 7,500,000: on a capital-intensive, levered project the gap
   between the two is mostly depreciation and interest, and confusing them is the commonest error
   in project financial conversation. The margins make the shape explicit: **62.50 % `EBITDA`
   margin, 17.20 % net margin** — the 45.30 points between them are the price of the asset and the
   loan, and they are almost entirely fixed, which is what makes the statement so revenue-sensitive.

   That sensitivity is computable and it is the most useful thing this statement will tell a
   leader. Because every line below revenue except tax is fixed in the short run, a **1 % revenue
   fall (120,000) costs 96,000 of net income after tax relief — 4.6512 % of it.** Net income is
   therefore **4.6512 times** as revenue-elastic as revenue itself, and any conversation that treats
   a "small" revenue miss as a small profit miss is out by that factor. Push the same arithmetic to
   its two breakevens and the domain's central claim falls out as a comparison:

   - **Profit breakeven.** Net income reaches nil when `EBITDA` covers depreciation and interest:
     `2,400,000 + 2,520,000 = 4,920,000`, so revenue of **USD 9,420,000** — a **21.50 %** fall.
   - **Cash breakeven.** `DSCR` reaches 1.00 when `CFADS` equals debt service. With `CFADS` =
     `0.8 × EBITDA + 984,000 − 600,000`, that needs `EBITDA` of **USD 5,782,044.04** and revenue of
     **USD 10,282,044.04** — a **14.32 %** fall.

   **The cash constraint binds first, and by a wide margin: 14.32 % against 21.50 %.** A project
   can be comfortably profitable and unable to pay, which is Domain 1's thesis restated as two
   numbers on one income statement, and it is the reason every covenant that matters in Domain 10 is
   struck on cash. Two cautions before the figures are used. The cash breakeven above holds
   depreciation, interest, tax rate and the working-capital movement constant while revenue moves,
   which is a first-order approximation and not a forecast: in practice receivables fall with
   revenue, so the true cash breakeven sits a little further out (Domain 15, KA 15.1.2 measures that
   feedback). And the 20 % tax charge is a charge on *positive* profit — below the profit breakeven
   the relationship bends, because relief for losses is jurisdiction-specific and may be deferred,
   capped or unavailable (KA 2.A.1; Domain 6, KA 6.2.3). A reviewer should check which of the two
   breakevens a management pack is quoting, because they are seven revenue points apart and only one
   of them causes a default.

### 2.2.2 The balance sheet

**What it answers:** what does the entity own and owe at a point in time? For an SPV the structure
is unusually clean — one asset of consequence and one financing structure — which makes it the ideal
place to see that **a balance sheet is not assembled, it is derived**: given the opening position
and the period's movements, every closing line including cash is determined, and the balance is a
proof rather than a presentation.

**Worked example 2.2.2 — completing Kestrel's balance sheet, and proving it.**

1. **Setup.** Opening position at the commercial operations date: plant **60,000,000**, cash
   **nil**, senior debt **42,000,000**, equity **18,000,000**, no receivables or payables. Year-one
   movements from Worked example 2.2.1 and the schedule in Domain 3: depreciation **2,400,000**,
   net income **2,064,000**, receivables up **900,000**, payables up **300,000**, principal repaid
   **2,489,635.23**, no distributions and no further contributions. Derive every closing line,
   including cash, and prove the sheet balances.
2. **Formula.** Plant, net = cost − accumulated depreciation. Closing debt = opening debt −
   principal repaid. Closing equity = opening equity + net income − distributions + contributions.
   Closing cash = operating cash flow − capex − principal − distributions. Then test
   `assets = liabilities + equity`.
3. **Substitution.** Plant `60,000,000 − 2,400,000`; debt `42,000,000 − 2,489,635.23`; equity
   `18,000,000 + 2,064,000 − 0 + 0`; cash `3,864,000 − 0 − 2,489,635.23 − 0`.
4. **Result.**

   | Assets | USD | Liabilities and equity | USD |
   |---|---|---|---|
   | Plant, net | 57,600,000.00 | Payables | 300,000.00 |
   | Receivables | 900,000.00 | Senior debt | 39,510,364.77 |
   | Cash | **1,374,364.77** | Equity (18,000,000 + 2,064,000) | 20,064,000.00 |
   | **Total assets** | **59,874,364.77** | **Total** | **59,874,364.77** |

   The sheet balances to the cent, with **nothing plugged**: cash was derived from the cash-flow
   statement and equity from the profit, and the equality is the consequence.

5. **Interpretation.** The single most valuable thing in the table is the cash figure, because it
   arrives twice by different routes and the two routes are an identity worth memorising:

   ```
   Operating cash flow − principal repaid  =  3,864,000.00 − 2,489,635.23  =  1,374,364.77
   CFADS − total debt service              =  6,384,000.00 − 5,009,635.23  =  1,374,364.77
   ```

   The two are equal because `CFADS` exceeds operating cash flow by exactly the interest paid, and
   debt service exceeds principal by exactly the same interest — so the interest cancels. That is
   the arithmetic bridge between the accountant's statement and the lender's ratio, and it is why
   Domain 9 records **1,374,364.77** as Kestrel's annual distributable cash at 70 % gearing
   (KA 9.1.4) while this domain records it as the closing cash balance: they are one number seen
   from two disciplines. Three consequences follow, and each is a reviewer's check.

   **The cash balance is a claim already spoken for.** Domain 6 (KA 6.2.2) takes the same year
   forward with the facility's reserve requirement in place and splits this exact balance into a
   **1,252,408.81** debt-service-reserve instalment and a **121,955.96** distribution — and
   `1,252,408.81 + 121,955.96 = 1,374,364.77`, to the cent. A leader reading this balance sheet as
   free liquidity would be reading 1,374,364.77 of cash of which **91.1264 %** is contractually
   restricted before anyone decides anything, leaving **8.8736 %** genuinely at the sponsors'
   disposal.

   **The asset is consumed on paper while it produces cash.** Depreciation reduced the carrying
   amount by 2,400,000 with no payment, so the balance sheet reports the asset getting smaller in
   the year the project first performed. Over the 12-year loan the plant's carrying amount falls to
   **31,200,000** while the debt falls to nil — the two lines converge for unrelated reasons, and
   any covenant expressed as a ratio of debt to book asset value moves every year without a
   transaction. In a regulated utility a second asset value — the regulatory asset base — runs on its
   own depreciation profile beside this one, and the two must never be conflated (see the industry
   variations at the end of this domain).

   **The debt balance falls by principal only.** Of the 5,009,635.23 instalment, 2,520,000 is
   interest (an expense) and **2,489,635.23** is principal (a balance-sheet movement, not an
   expense) — exactly Domain 3's schedule. That split is why debt service never appears as a single
   line in the income statement, and why a reader who looks only at profit cannot see whether the
   loan is being repaid. It also explains a figure that surprises boards: equity rose by 2,064,000
   while capital employed **fell** by 425,635.23, because the principal repaid exceeded the retained
   profit. Growth in equity and growth in the business are different things, and on an amortising
   project financing they routinely point in opposite directions.

### 2.2.3 The cash-flow statement

**What it answers:** where did cash come from and go? Three sections: **operating** (the trading
cycle), **investing** (asset purchases and disposals), **financing** (debt drawn and repaid,
equity contributed, distributions paid). The indirect method — the one a project leader will
usually meet — starts from profit and undoes the accruals:

```
Net income
  + non-cash charges (depreciation, amortisation, provisions)
  − increases in receivables and inventory      (cash absorbed)
  + increases in payables                       (cash released)
  = operating cash flow
```

The **direct method** instead reports the gross cash flows themselves — collected from customers,
paid to suppliers, paid in interest, paid in tax. It is rarer in published accounts and far more
useful in a project, because a lender's questions are about the gross flows and the indirect method
conceals every one of them behind a net movement.

**Worked example 2.2.3 — the same operating cash flow, built the other way.**

1. **Setup.** Kestrel's year one as above: revenue 12,000,000, cash operating costs 4,500,000,
   receivables up 900,000, payables up 300,000, interest paid 2,520,000, tax paid 516,000.
   Depreciation of 2,400,000 does not appear at all, because no cash moved. Build operating cash
   flow directly.
2. **Formula.** Cash collected from customers = revenue − Δreceivables. Cash paid to suppliers and
   employees = cash operating costs − Δpayables. Operating cash flow = collections − payments −
   interest paid − tax paid.
3. **Substitution.** `12,000,000 − 900,000`; `4,500,000 − 300,000`; then
   `11,100,000 − 4,200,000 − 2,520,000 − 516,000`.
4. **Result.**

   | Direct method | USD |
   |---|---|
   | Cash collected from customers | 11,100,000 |
   | Cash paid to suppliers and employees | (4,200,000) |
   | Interest paid | (2,520,000) |
   | Tax paid | (516,000) |
   | **Operating cash flow** | **3,864,000** |

   Identical to the indirect method's **3,864,000** (KA 2.2.4). Two methods, one number, no
   depreciation anywhere in sight.

5. **Interpretation.** The two methods cannot disagree — they are the same account read from
   opposite ends — so the value of building both is diagnostic, and it is considerable. **The direct
   method surfaces a collection ratio the indirect method hides:** Kestrel collected
   **92.50 %** of the revenue it recognised (11,100,000 of 12,000,000). That single percentage is
   the operational question behind the whole of KA 2.3.1, and it is invisible in a statement whose
   only working-capital line is a net 600,000. It also gives the days figures directly —
   **27.3750 days** of sales outstanding and **24.3333 days** of payables on cash costs, at a
   365-day convention — which is what makes a covenant translatable into an instruction to a
   collections team (KA 2.3.1B).

   The more consequential point is about the **interest line**, and it is where careless comparison
   does real damage. Kestrel's presentation classifies interest paid within operating activities. Had
   it been classified within financing — a presentation choice permitted in some framework
   applications and mandatory in none universally — operating cash flow would have been
   `3,864,000 + 2,520,000 =` **6,384,000**, which is *precisely* the documented `CFADS` figure.
   Nothing about the project changed; a single classification decision moved reported operating cash
   flow by **65.2174 %** of itself. Three professional consequences. **Never compare operating cash
   flow across two entities without checking where each puts interest** — the difference is one line
   and it is larger than most of the differences analysts write about. **The identity `CFADS` =
   operating cash flow + interest paid holds only on the operating-classification presentation**;
   Domain 6 (KA 6.2.2) uses it as a model check, and a reviewer applying it to a financing-classified
   statement would double-count the interest and overstate `CFADS` by 2,520,000. And **the direct
   method is the right internal format for a project**, because the four gross lines it reports —
   collections, supplier payments, interest, tax — are the four things a monthly review can actually
   act on. The indirect method's virtue is only that it starts where the income statement stops,
   which is a convenience for the preparer and a cost to the reader.

### 2.2.4 Articulation demonstrated

**Worked example 2.2.4 — from profit to cash, and proving it ties.**

1. **Setup.** Kestrel's income statement above, plus the balance-sheet movements: receivables rose
   **USD 900,000**, payables rose **USD 300,000**. Derive operating cash flow.
2. **Formula.** Operating cash = net income + depreciation − Δreceivables + Δpayables.
3. **Substitution.** `2,064,000 + 2,400,000 − 900,000 + 300,000`.
4. **Result.** **Operating cash flow USD 3,864,000** — against net income of USD 2,064,000. The
   reconciliation: depreciation adds back **+2,400,000** (a charge that moved no cash), working
   capital absorbs **−600,000** net.
5. **Interpretation.** Read the bridge in both directions. Profit *understates* the period's cash
   by the depreciation of an asset paid for years ago; working capital *takes back* 600,000 of it
   to fund growth in the trading cycle. Both facts are invisible in the income statement and
   decisive for a lender. Note also what operating cash flow is **not**: it is before debt
   principal (a financing flow) and before capex — so a project can show positive operating cash
   and still fail to cover its obligations. Domain 10's coverage ratios exist precisely to test
   the thing this statement does not.

   The ratio between the two ends of the bridge is worth naming and worth distrusting. Kestrel's
   **cash conversion — operating cash flow ÷ net income — is 1.8721**, and a reader trained to
   regard a figure above 1.0 as healthy would file that as reassuring. It is not a quality signal at
   all: it is arithmetically guaranteed to exceed 1.0 on any capital-intensive project whose
   depreciation charge (2,400,000) exceeds its working-capital absorption (600,000), and it would
   stay above 1.0 in a year of collapsing collections. **A high cash conversion on an
   infrastructure asset measures capital intensity, not performance.** The diagnostic content is in
   the two components separately — the non-cash add-back, which is fixed by an accounting policy set
   years ago, and the working-capital movement, which is this period's operational news.

   Three checks a reviewer should run on any such bridge before relying on it. **Tie the add-back to
   the income statement**: depreciation in the cash-flow statement must equal the income-statement
   charge to the dollar, and a difference means either an impairment, a disposal or an error, all
   three of which need explaining. **Tie the working-capital lines to the balance-sheet deltas**: the
   −900,000 and +300,000 must reconcile to the movement in the receivable and payable accounts, and
   a "net working capital" single line that cannot be decomposed is the commonest place a modelling
   plug hides (Domain 6, KA 6.4.1). And **check the sign convention on payables**, which is the
   error the exercises at the end of this domain are built to catch: an increase in payables is
   supplier credit and therefore a cash *source*, and reversing it turns the working-capital
   absorption from 600,000 into 1,200,000, taking `CFADS` to 5,784,000 and the reported `DSCR` from
   **1.2743 to 1.1546** — a covenant breach and a lock-up, on a project whose trading did not
   change. One sign is worth **0.1198** of coverage.

   Finally, one line further on, the bridge continues past operating cash flow to the only figure a
   sponsor ultimately cares about: `3,864,000 − 2,489,635.23 =` **1,374,364.77** of cash left after
   the loan has been serviced (KA 2.2.2). Everything Domain 15 does with waterfalls and lock-ups
   happens inside that number.

> **Fig 2.2.1 — Kestrel's accrual-to-cash bridge.** Waterfall chart, y-axis USD 0–5m. Bars left to
> right: Net income 2,064,000 (start) · +Depreciation 2,400,000 (rising, brand blue) ·
> −Receivables increase 900,000 (falling, crimson) · +Payables increase 300,000 (rising, blue) ·
> **Operating cash flow 3,864,000** (total bar, ink). Each bar labelled with its value; a bracket
> above the middle three annotated "the accrual adjustments — none of them cash decisions of this
> period". Source: PCI original. Alt text: waterfall from net income of just over two million,
> lifted by depreciation and payables and reduced by receivables, to operating cash flow of
> USD 3.86 million.

### AI in this KA

**Where it earns its place.** The articulation checks of this Knowledge Area are ideal machine work,
because each is a stated identity with a numeric answer: does the sheet balance, does closing cash
tie to the cash-flow statement, does closing equity equal opening plus profit less distributions,
does the depreciation add-back equal the income-statement charge, do the working-capital lines
reconcile to the balance-sheet deltas. An assistant can run all of them across every period of a
statement set in seconds and report where the first break occurs, which is precisely the diagnostic
KA 2.1.3 says localises a problem — and it is tedious enough that humans skip it.

**Where it fails, specifically.** It cannot tell you whether the *right* number balanced. Two
failures recur. A model will happily reconcile a statement set in which interest paid is classified
in financing and then apply the `CFADS` = operating cash flow + interest identity anyway,
overstating `CFADS` by the whole interest charge (2.2.3) — the identity is presentation-dependent and
the presentation is not detectable from the arithmetic. And where a working-capital movement is
presented as a single net line, an assistant asked to reconcile it will often accept the net figure as
evidence rather than reporting that the decomposition is missing, which is where a plug hides
(Domain 6, KA 6.4.1). Require the check to name its inputs, and check the classification yourself.
**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 2.2

| Term | Meaning |
|---|---|
| **`EBITDA`** | Earnings before interest, tax, depreciation and amortisation. |
| **`EBIT`** | Operating profit after depreciation; performance including capital consumption. |
| **Depreciation** | (cost − residual)/useful life; a non-cash charge for asset consumption. |
| **Indirect method** | Deriving operating cash flow from profit by undoing accruals. |
| **Direct method** | Reporting the gross operating cash flows: collections, supplier payments, interest, tax. |
| **Working-capital movement** | Change in receivables, inventory and payables; absorbs or releases cash. |
| **Principal versus interest** | Principal is a balance-sheet movement; only interest is an expense. |
| **Cash conversion** | Operating cash flow ÷ net income; on a capital-intensive asset it measures capital intensity, not quality. |
| **Capital employed** | Debt plus equity; the base a return on capital is measured against. |

### Sample MCQs — KA 2.2

**MCQ 2.2-A `[2.2.1 · Application]`** Revenue 12,000,000; cash operating costs 4,500,000;
depreciation 2,400,000; interest 2,520,000; tax 20 %. Net income is:
- A. USD 2,580,000
- B. USD 2,064,000 ✅
- C. USD 4,080,000
- D. USD 7,500,000

*Rationale:* `PBT 2,580,000 × (1 − 0.20) = 2,064,000`. A is PBT before tax; C taxes `EBIT`
instead of PBT; D is `EBITDA`.

**MCQ 2.2-B `[2.2.4 · Application]`** Net income 2,064,000; depreciation 2,400,000; receivables
+900,000; payables +300,000. Operating cash flow is:
- A. USD 4,464,000
- B. USD 3,864,000 ✅
- C. USD 3,264,000
- D. USD 2,064,000

*Rationale:* `2,064,000 + 2,400,000 − 900,000 + 300,000 = 3,864,000`. A omits the working-capital
movements; C subtracts the payables increase instead of adding it (supplier credit is a cash
*source*); D stops at profit.

**MCQ 2.2-C `[2.2.2 · Analysis]`** Kestrel's annual instalment is 5,009,635, of which 2,520,000 is
interest. The income statement expense and the balance-sheet effect are:
- A. expense 5,009,635; debt falls by 5,009,635
- B. expense 2,520,000; debt falls by 2,489,635 ✅
- C. expense 2,489,635; debt falls by 2,520,000
- D. expense 5,009,635; debt unchanged

*Rationale:* Only interest is an expense; principal is a balance-sheet movement. A expenses the
whole instalment (the classic error), C reverses the two components, D ignores repayment.

**MCQ 2.2-D `[2.2.3 · Recall]`** Repayment of debt principal appears in the cash-flow statement
under:
- A. operating activities
- B. investing activities
- C. financing activities ✅
- D. it does not appear, being a balance-sheet movement

*Rationale:* Principal flows are financing. D confuses the *income statement's* silence on
principal with absence from the cash-flow statement, where every cash movement appears.

**MCQ 2.2-E `[2.2.2 · Application]`** Opening cash nil; operating cash flow 3,864,000; principal
repaid 2,489,635.23; no capex, distributions or contributions. Plant is 57,600,000 net, receivables
900,000, payables 300,000 and closing debt 39,510,364.77. Closing cash and total assets are:
- A. cash 1,374,364.77; total assets 59,874,364.77 ✅
- B. cash 3,864,000.00; total assets 62,364,000.00
- C. cash (1,145,635.23); total assets 57,354,364.77
- D. cash 1,374,364.77; total assets 58,500,000.00

*Rationale:* Cash = 3,864,000 − 2,489,635.23, and total assets are the three lines summed. B omits
the principal repayment (treating debt service as invisible to cash); C deducts the whole
5,009,635.23 instalment, double-counting the interest already inside operating cash flow; D omits
the cash balance from the asset total.

**MCQ 2.2-F `[2.2.3 · Evaluation]`** Two otherwise identical projects report operating cash flow of
3,864,000 and 6,384,000. The second classifies interest paid within financing activities. The
soundest conclusion is:
- A. the second project generates 2,520,000 more cash from operations
- B. the figures are not comparable: the 2,520,000 difference is a classification of interest, and restating one presentation makes them identical ✅
- C. the second project must have lower interest costs
- D. the first project has a working-capital problem

*Rationale:* The classification of interest paid moves reported operating cash flow by the whole
interest figure without changing any cash (2.2.3). A and C read a presentation choice as an economic
difference — the specific error the `CFADS` identity is exposed to; D invents a cause the statements
do not support.

### Self-check — KA 2.2

1. *Why is `EBITDA` USD 7,500,000 while net income is USD 2,064,000?* — Depreciation, interest and
   tax lie between them; on a capital-intensive levered project that gap is structural.
2. *State the accrual-to-cash bridge for Kestrel.* — 2,064,000 + 2,400,000 depreciation
   − 900,000 receivables + 300,000 payables = 3,864,000.
3. *What obligations does operating cash flow not cover?* — Debt principal and capex; hence
   Domain 10's coverage ratios.
4. *Derive Kestrel's closing cash two ways.* — Operating cash flow 3,864,000 less principal
   2,489,635.23, or `CFADS` 6,384,000 less debt service 5,009,635.23; both give 1,374,364.77
   because the interest cancels.
5. *Which two revenue breakevens does the income statement carry, and which binds?* — Profit
   breakeven at revenue 9,420,000 (−21.50 %) and cash breakeven at 10,282,044.04 (−14.32 %); cash
   binds first.
6. *Why is a cash conversion ratio above 1.0 not reassuring on its own?* — Because depreciation
   exceeding the working-capital movement guarantees it on a capital-intensive asset.

---

## Knowledge Area 2.3 — Project-relevant treatments

*Topics: 2.3.1 working capital · 2.3.2 revenue and cost recognition · 2.3.3 capital versus
operating expenditure · 2.3.4 provisions and contingencies.*

### 2.3.1 Working capital and why lenders care

**Definition.** Working capital is the cash tied up in the trading cycle — receivables plus
inventory less payables. Growth in it **consumes** cash; reduction **releases** it. For a project,
the dominant driver is the gap between doing work and being paid for it, which is why payment
terms are a financing decision (Domain 1's cash discipline; Domain 14's drawdowns).

**Worked example 2.3.1 — what working capital does to a coverage ratio.**

1. **Setup.** Kestrel's `EBITDA` USD 7,500,000; tax USD 516,000; the net working-capital increase
   of USD 600,000 from KA 2.2.4. Annual debt service USD 5,009,635 (Domain 3). Compute the
   `DSCR` — Domain 10's central ratio — first ignoring working capital, then including it.
2. **Formula.** `CFADS` = `EBITDA` − tax − Δworking capital; `DSCR` = `CFADS` ÷ debt service.
3. **Substitution.** Ignoring WC: `7,500,000 − 516,000 = 6,984,000`; `÷ 5,009,635`.
   Including WC: `6,984,000 − 600,000 = 6,384,000`; `÷ 5,009,635`.
4. **Result.** `DSCR` **1.39** ignoring working capital; **1.27** including it — a fall of
   **0.12** from one balance-sheet movement.
5. **Interpretation.** A fall of **0.12 in the coverage ratio** may decide whether a covenant holds
   (typical senior covenants sit at 1.20–1.30, so this project is comfortable at 1.39 and much
   closer to the line at 1.27). The lesson is definitional rather than arithmetical: **`CFADS` is
   a defined term in the finance documents, and whether it is struck before or after
   working-capital movements changes the ratio it produces.** A leader who quotes a `DSCR` without
   knowing its `CFADS` definition is quoting an opinion. Domain 10 builds the full machinery;
   Domain 13's model audit checks that the model implements the *documented* definition.

   Two refinements make the point operational rather than merely cautionary. First, the sensitivity
   is linear and easy to carry: because debt service is fixed at 5,009,635.23, **every 50,096.35 of
   working-capital absorption costs 0.01 of `DSCR`** — so the ratio can be managed in the same units
   as a collections target. Second, the direction of the definitional choice is not neutral between
   the parties. Excluding working capital produces the *higher* ratio in a growing project and the
   *lower* ratio in a shrinking one, which means a sponsor arguing for exclusion at financial close
   is arguing for a definition that will turn against it in the first year of decline. The
   definitional fight is therefore not "which number is bigger" but **which number is stable**, and
   stability is what a lender is buying.

**Worked example 2.3.1B — the covenant restated as a collection period.**

1. **Setup.** The same year. Kestrel's `CFADS` before working capital is **6,984,000** and debt
   service **5,009,635.23**. Payables are held at **300,000**. The facility carries a **1.20×**
   financial covenant, a **1.25×** distribution condition and a **1.15×** lock-up (Domain 10,
   KA 10.4). Revenue is 12,000,000 and receivables opened at nil, so closing receivables *are* the
   year's absorption. Express each threshold as a maximum days-sales-outstanding figure at a 365-day
   convention, and compute the position at a 45-day collection period.
2. **Formula.** `CFADS` trigger = threshold × debt service. Allowable net absorption = 6,984,000 −
   trigger. Allowable closing receivables = allowable absorption + payables. `DSO` = receivables ÷
   revenue × 365.
3. **Substitution.** For the 1.20× test: `1.20 × 5,009,635.23 = 6,011,562.28`;
   `6,984,000 − 6,011,562.28 = 972,437.72`; `972,437.72 + 300,000 = 1,272,437.72`;
   `1,272,437.72 / 12,000,000 × 365`.
4. **Result.**

   | Test | Threshold | `CFADS` trigger | Allowable receivables | Maximum `DSO` | Days of headroom |
   |---|---|---|---|---|---|
   | Distribution condition | 1.25× | 6,262,044.04 | 1,021,955.96 | **31.0845 days** | **3.7095** |
   | Financial covenant | 1.20× | 6,011,562.28 | 1,272,437.72 | **38.7033 days** | **11.3283** |
   | Distribution lock-up | 1.15× | 5,761,080.51 | 1,522,919.49 | **46.3221 days** | **18.9471** |

   Actual `DSO` is **27.3750 days**. At a 45-day collection period receivables would be
   **1,479,452.05**, absorption **1,179,452.05**, `CFADS` **5,804,547.95** and `DSCR` **1.1587** —
   below the covenant and inside the lock-up.

5. **Interpretation.** This is the translation that makes an accounting statement usable by the
   people whose behaviour determines it. **The distribution condition is a 31-day collection
   covenant.** Nothing in the finance documents says so, no operations dashboard reports it, and the
   entire margin between a dividend and no dividend is **3.7095 days** of collection — less than one
   invoice cycle, and well inside the ordinary variability of a public-sector offtaker's payment run.
   That figure, not the 1.25 in the term sheet, is the number a finance director should put in front
   of the collections team.

   Read the sensitivity as a unit rate and it becomes a management metric: at 12,000,000 of revenue
   **one day of `DSO` is 32,876.71 of cash and 0.006563 of `DSCR`**, so a one-week slip in payment
   behaviour costs **0.0459** of coverage. Three cautions on over-applying the result. It assumes
   payables stay at 300,000; a project that funds a receivables slip by stretching its own suppliers
   converts a covenant problem into a supply-chain problem and a possible default under the operating
   contracts, which is a worse trade than it looks. It assumes revenue is unchanged, so it isolates
   *collection* risk from *demand* risk — the two are separated in Domain 7 for exactly this reason,
   and a fall in revenue reduces receivables and flatters the ratio at the same time as it destroys
   the cash. And the 365-day convention matters: the finance documents may compute on 360 days,
   which tightens every threshold in the table by **1.3889 %** — the 1.25× test becomes
   **30.6587 days** rather than 31.0845 — so the convention belongs on the defined-terms sheet
   (Toolkit 2.T.1) rather than in a modeller's habit. The remedy Case study A
   reaches — a working-capital facility sized against the slip, and collections given a named owner
   — is the operational answer to an accounting problem, which is the whole reason this Knowledge
   Area exists.

> **Fig 2.3.1 — The covenant as a collection period.** Line chart, x-axis days sales outstanding
> 20–50, y-axis `DSCR` 1.10–1.35. One descending line for `DSCR` = (6,984,000 − (12,000,000 ×
> `DSO`/365 − 300,000)) ÷ 5,009,635.23, plotted in brand blue. Horizontal reference lines at
> **1.25** (distribution condition) and **1.20** (financial covenant) in slate, with a crimson band
> below 1.20. Marked points: the actual position at **27.3750 days / 1.2743**, the distribution
> crossing at **31.0845 days / 1.2500** and the covenant crossing at **38.7033 days / 1.2000**, plus
> a hollow marker at **45 days / 1.1587**. A crimson bracket inside the plot, above the line,
> spanning 27.3750 to 31.0845 days and annotated "3.7095 days — the whole dividend". Source: PCI original. Alt text: a straight
> downward-sloping line showing coverage falling as the collection period lengthens, crossing the
> distribution condition after under four extra days and the financial covenant after eleven.

### 2.3.2 Revenue and cost recognition

**The principle.** Revenue is recognised as performance occurs — when control of the promised
goods or services transfers to the customer — not when the invoice is raised or the cash arrives.
For long-duration work this means recognising **over time** as the obligation is satisfied, which
requires a defensible measure of progress. IFRS 15 is the reference framework, described here in
principle only.

**Measuring progress** uses either **input** methods (costs incurred relative to total expected
costs — the familiar percentage-of-completion approach) or **output** methods (units delivered,
milestones surveyed). The choice matters and is not free: an input measure can be distorted by
inefficiency (spending more "earns" more revenue) and an output measure by lumpy deliverables.
This is the same measurement-integrity problem the delivery book handles as earned value
(PML-AI, Domain 7, KA 7.3.1) — and the two disciplines should use the *same* progress evidence,
because a project claiming 60 % complete for revenue and 48 % for earned value has a story to
explain.

**Costs** follow performance symmetrically: costs of fulfilling the obligation are expensed as
incurred, certain fulfilment costs may be capitalised, and **expected losses are recognised
immediately and in full** the moment a contract is expected to be loss-making — a deliberate
asymmetry that stops a known loss being spread quietly across future periods.

**Worked example 2.3.2 — the same progress, two measures, and then a loss.**

1. **Setup.** This example sits on the *other side* of Kestrel's construction contract: the EPC
   contractor's own accounts, not the SPV's. The lump-sum price is **USD 48,000,000** (Domain 14's
   master construction). At the quarter-five data date the contractor has incurred costs of
   **USD 27,000,000** and estimates **USD 15,000,000** to complete; the SPV has certified **61 %**
   of the milestone schedule (Domain 14, KA 14.2.1). Compute revenue and profit to date on a
   cost-input measure and on a milestone-output measure. Then suppose a re-forecast puts the cost to
   complete at **USD 22,500,000**.
2. **Formula.** Input progress = costs incurred ÷ expected total costs. Revenue to date = price ×
   progress. Profit to date = revenue to date − costs incurred. Onerous test: expected total costs >
   price ⇒ recognise the whole expected loss immediately; the period charge is the swing from the
   cumulative position already recognised.
3. **Substitution.** Expected total cost `27,000,000 + 15,000,000 = 42,000,000`; input progress
   `27,000,000 / 42,000,000`; input revenue `48,000,000 × 0.642857…`; output revenue
   `48,000,000 × 0.61`. Revised total cost `27,000,000 + 22,500,000`.
4. **Result.**

   | Measure | Progress | Revenue to date | Cost to date | Profit to date |
   |---|---|---|---|---|
   | Cost input | **64.2857 %** | 30,857,142.86 | 27,000,000 | **3,857,142.86** |
   | Milestone output | **61.0000 %** | 29,280,000.00 | 27,000,000 | **2,280,000.00** |
   | **Difference** | **3.2857 points** | **1,577,142.86** | — | **1,577,142.86** |

   On the re-forecast, expected total cost of **49,500,000** against a price of 48,000,000 makes the
   contract **onerous** by **1,500,000**. The cumulative position must move from +3,857,142.86 to
   −1,500,000, so the charge in the period is **USD 5,357,142.86** — **3.5714 times** the loss
   itself.

5. **Interpretation.** Three results, each with a professional edge. **The two progress measures
   differ by 1,577,142.86 of profit on an identical set of facts**, which is **26.2857 %** of the
   contract's whole expected margin of 6,000,000 — so the choice of measure is not a presentational
   detail, it is a quarter of the job's reported profitability. And the direction is diagnostic
   rather than arbitrary: the input measure is *ahead* because costs have run faster than
   certifications, which is precisely the pathology KA 2.3.2 warns about — under a cost-input
   measure, **inefficiency earns revenue**. The gap between 64.2857 % and 61 % is the contractor
   spending 3.2857 points of the job without producing 3.2857 points of it, and reporting the
   difference as performance.

   **The onerous-contract swing is the number that surprises boards.** A 1,500,000 loss produced a
   5,357,142.86 charge, because recognising a loss also requires *unwinding the profit already
   taken*. The multiple is not a quirk of these figures: it is `1 + profit recognised ÷ loss`, so the
   further into a contract the reversal comes, the larger the swing, and a contract that turns at
   90 % complete produces a charge that can dwarf the loss it reports. This is the mechanism behind
   the well-known pattern of construction results being stable and then violently negative, and it is
   why a lender to a contractor reads the contract portfolio's cost-to-complete forecasts and not
   its profit history.

   **The two disciplines must reconcile.** The SPV's earned value on this scope shows `CPI` = 1.0000
   by construction, because on a fixed-price milestone certification the amount certified *is* the
   amount budgeted (Domain 14, KA 14.2.1) — so the cost overrun that is plainly visible in the
   contractor's books is structurally invisible in the SPV's. Neither party is wrong and neither
   number is useless; what is dangerous is a monitoring regime that reads only one of them. The
   discipline is to require the *same progress evidence* to support both, and to treat a divergence
   between the certified percentage and the contractor's cost-based percentage as the leading
   indicator it is (the delivery-side treatment is PML-AI, Domain 7, KA 7.3.1). Whether an input or
   an output measure is permitted, and how a loss-making contract must be presented, is a matter for
   the entity's framework, finance function and auditors; the arithmetic above is transferable, the
   conclusion is not.

### 2.3.3 Capital versus operating expenditure

**The distinction.** **Capital expenditure (capex)** creates or enhances an asset with benefits
beyond the current period; it is capitalised to the balance sheet and depreciated over its useful
life. **Operating expenditure (opex)** is consumed in the period and expensed immediately. The
same cash outflow, classified differently, produces very different profit.

**Worked example 2.3.3 — the same USD 1,200,000, two treatments.**

1. **Setup.** Kestrel spends USD 1,200,000 on a control-system overhaul. If it extends the asset's
   capability it is capex, depreciated over 10 years; if it restores existing performance it is
   maintenance opex.
2. **Formula.** Capex year-one charge = spend ÷ useful life. Opex year-one charge = full spend.
3. **Substitution.** Capex: `1,200,000/10 = 120,000`. Opex: `1,200,000`.
4. **Result.** Year-one profit is **USD 1,080,000 lower** under the opex treatment. **Cash is
   identical in both cases** — USD 1,200,000 leaves the account either way.
5. **Interpretation.** This is the clearest demonstration in the domain that accounting choice
   moves reported performance without moving economics, and it cuts both ways. Capitalising
   flatters current profit and burdens future periods with depreciation; it also inflates the
   asset base against which returns are measured. Because the classification is judgment applied
   to facts — does this enhance or merely restore? — it is a standing area of audit attention, and
   a leader should expect to justify it rather than assert it. Note the covenant consequence:
   profit-based covenants respond to the choice while cash-based ones (`DSCR`) do not, which is
   part of why lenders prefer cash-based tests (Domain 10).

   Two refinements, and the second is the one a careful reader should insist on. **Over the ten
   years the two treatments charge the identical 1,200,000**, so the choice is entirely about
   *timing*: capitalising defers 1,080,000 of charge into years two to ten, at 120,000 a year, and by
   year ten the cumulative profit under both treatments is the same to the dollar. Anyone presenting
   capitalisation as an improvement in performance is presenting a nine-year loan against the future,
   and the interest on it is paid in depreciation.

   **And the claim that "cash is identical" holds only where the accounting choice does not drive the
   tax deduction.** Kestrel's illustration assumes tax depreciation follows accounting depreciation —
   a simplification stated as one (KA 2.A.1; Domain 6, KA 6.2.3). Where instead the deduction follows
   the accounting treatment, at a 20 % tax rate and an 8 % discount rate the two paths give:
   relief of **240,000 in year one** if expensed, against **120,000 × 20 % = 24,000** a year for ten
   years if capitalised, whose present value at `AF(8 %, 10) = 6.710081` is **161,041.95**. The
   expensing route is worth **USD 78,958.05** of present value — **6.5798 %** of the spend — and that
   *is* a cash difference, arising purely from a classification argument. The professional position is
   therefore precise rather than slogan-like: **the capex/opex choice does not change the pre-tax cash
   flow, and may well change the after-tax cash flow.** Whether it does is jurisdiction-specific,
   because in many regimes the tax base is computed independently of the accounts, and the answer for
   a particular entity is a matter for qualified tax advice rather than for this book. What a leader
   should never do is argue the classification *because of* the tax outcome; the classification
   follows the facts, and the tax follows the classification.

### 2.3.4 Provisions and contingencies

**Definitions.** A **provision** is a liability of uncertain timing or amount, recognised when a
present obligation exists from a past event, settlement is probable, and a reliable estimate can
be made (IAS 37 is the reference framework, in principle). A **contingent liability** fails one of
those tests — typically because settlement is only possible rather than probable — and is
**disclosed, not recognised**. A **contingent asset** is treated still more cautiously: disclosed
when probable, recognised only when virtually certain, an asymmetry that deliberately resists
optimism.

Project-relevant instances: **decommissioning and site-restoration obligations** (recognised at
the present value of the expected cost when the obligation arises — often at construction, decades
before the spend, and a major balance-sheet item for energy and resources projects); **warranty and
defect obligations**; **onerous contracts** (the loss recognised in full, per 2.3.2); and
**disputed claims**, where the provision/disclosure boundary is genuinely contested and legal
advice governs. The leader's discipline here is narrow and important: **a provision is not a
reserve to be created in good years and released in bad ones** — that is earnings management, and
the recognition tests exist to prevent it.

**Worked example 2.3.4 — measuring and unwinding a restoration obligation.**

1. **Setup.** The master statements in KA 2.2 present Kestrel without a restoration obligation, for
   exposition. Suppose instead the concession requires the intake and outfall works to be removed at
   the end of the 25-year life, at an expected cost of **USD 4,500,000** in the money of year 25, and
   that a **5.0 %** discount rate is applied. The obligation arises on construction. Compute the
   amount recognised, the first-year charges, the cash effect, and the sensitivity to the rate.
2. **Formula.** Provision at recognition = expected cost ÷ (1 + r)ⁿ. The same amount is added to the
   asset's cost and depreciated over the life. Each year: accretion (a finance charge) = opening
   provision × r; depreciation of the restoration asset = initial provision ÷ life. Closing
   provision = opening + accretion.
3. **Substitution.** `4,500,000 / 1.05²⁵`; accretion `1,328,862.47 × 0.05`; depreciation
   `1,328,862.47 / 25`.
4. **Result.**

   | Line | USD |
   |---|---|
   | Provision recognised at construction | **1,328,862.47** |
   | Added to the asset's cost (60,000,000 → 61,328,862.47) | 1,328,862.47 |
   | Year-one accretion (finance charge) | **66,443.12** |
   | Year-one depreciation of the restoration asset | **53,154.50** |
   | **Year-one charge against profit** | **119,597.62** |
   | **Year-one cash effect** | **nil** |
   | Provision at the end of year one | 1,395,305.60 |

5. **Interpretation.** Start with the invariant, because it disciplines everything else:
   **depreciation of 1,328,862.47 over the life plus accretion of 3,171,137.53 over the life sums to
   exactly 4,500,000 — the cash eventually paid.** Accrual accounting has not invented a cost; it has
   allocated a known future payment across the periods that caused it, and the provision accretes to
   precisely the settlement amount in the year it falls due. That is the cleanest available
   demonstration of what recognition does and does not do.

   The professional content is in three places. **The charge is real and the cash is nil**, so a
   profit-based covenant is exposed to this obligation from the first year of operation while
   `DSCR` is untouched for twenty-five years: 119,597.62 is **5.7945 %** of Kestrel's net income,
   enough to matter in a tight profit test and invisible in every coverage ratio the lender computes.
   Domain 8 (KA 8.4) makes the complementary point on the cash side — the spend falls *after* the loan
   matures, so it reduces the tail that Domain 10's `PLCR` of **1.9431** measures without appearing in
   any ratio the lender tests.

   **The balance sheet then moves with interest rates rather than with the obligation.** Re-measure at
   **4.0 %** and the provision becomes **1,688,025.61** — **27.0279 %** higher; at **6.0 %** it becomes
   **1,048,493.84**, **21.0984 %** lower. Nothing about the works, the cost or the concession has
   changed. A reader comparing two projects' balance sheets, or one project across two years, must
   establish the discount rate before treating the movement as news; where the rate is re-measured
   annually the provision line is one of the noisiest on an infrastructure balance sheet.

   **And the tax treatment is where the arithmetic stops being transferable.** Many regimes deny a
   deduction until the expenditure is actually incurred, which would make this charge non-deductible
   for twenty-five years and create a deferred tax asset whose recoverability depends on there being
   taxable profit in the year of settlement — by which time the project may have none. The
   recognition trigger, the discount rate basis, the treatment of subsequent changes in estimate and
   the deductibility are all framework- and jurisdiction-specific and are matters for the entity's
   auditors and qualified tax advisers; and because the *existence* of the obligation usually turns on
   the concession or permit wording, its scope is a question for counsel before it is a question for
   accountants.

### AI in this KA

These four treatments are exactly where AI assistance is least reliable, because each turns on
*judgment applied to specific facts*: is this capex or maintenance, is settlement probable, does
control transfer over time. A model will produce a confident answer that reads like a conclusion
and is actually a summary of how similar questions are usually answered. The governed position:
use AI to marshal the facts, locate the relevant framework and draft the argument on both sides;
never to reach the conclusion. Conclusions on recognition and measurement belong to the entity's
finance function and auditors, and jurisdiction-specific treatment to qualified advisers — the
boundary this book applies to itself (Domain 1, KA 1.3.1).

### Key terms — KA 2.3

| Term | Meaning |
|---|---|
| **Working capital** | Receivables + inventory − payables; growth consumes cash. |
| **`CFADS`** | Cash flow available for debt service — a *defined* term whose definition changes the ratio. |
| **Days sales outstanding (`DSO`)** | Receivables ÷ revenue × days in the convention; the collection period a covenant can be restated in. |
| **Input / output progress measures** | Cost-based vs delivery-based measures of performance to date. |
| **Onerous contract** | An expected-loss contract; the loss is recognised immediately and in full. |
| **Capex / opex** | Capitalised and depreciated vs expensed in the period; identical pre-tax cash, different profit. |
| **Provision / contingent liability** | Recognised (present obligation, probable, estimable) vs disclosed. |
| **Accretion** | The finance charge that unwinds the discount on a provision, taking it to the settlement amount. |

### Sample MCQs — KA 2.3

**MCQ 2.3-A `[2.3.1 · Application]`** `EBITDA` 7,500,000; tax 516,000; working-capital increase
600,000; debt service 5,009,635. `DSCR` including the working-capital movement is:
- A. 1.39
- B. 1.27 ✅
- C. 1.50
- D. 1.20

*Rationale:* `CFADS = 7,500,000 − 516,000 − 600,000 = 6,384,000`; `÷ 5,009,635 = 1.27`. A excludes
the working-capital movement (the definitional point of the example); C ignores tax as well;
D is a typical covenant threshold, not this calculation.

**MCQ 2.3-B `[2.3.3 · Application]`** USD 1,200,000 is spent on an overhaul. Capitalised over
10 years versus expensed immediately, the year-one differences are:
- A. profit lower by 1,200,000 if capitalised; cash differs
- B. profit lower by 1,080,000 if expensed; cash identical ✅
- C. profit identical; cash lower by 1,080,000 if expensed
- D. profit lower by 120,000 if expensed; cash identical

*Rationale:* Expensing charges 1,200,000 against capitalising's 120,000 — a 1,080,000 difference —
and the cash outflow is the same either way. A inverts which treatment charges more; C confuses
which statement is affected; D states the capitalised charge as the difference.

**MCQ 2.3-C `[2.3.4 · Analysis]`** A contractor faces a claim it considers *possible* but not
probable, with a reliably estimable amount. The treatment is:
- A. recognise a provision, since the amount is estimable
- B. disclose as a contingent liability; recognition requires probable settlement ✅
- C. no disclosure, since settlement is not probable
- D. recognise a contingent asset

*Rationale:* Estimability alone is insufficient — probability of settlement is the failed test, so
disclosure rather than recognition (2.3.4). C hides information users need; D reverses the
direction.

**MCQ 2.3-D `[2.3.2 · Analysis]`** A contractor recognises revenue on 60 % completion using a
cost-input measure while its earned-value system reports 48 % complete. The soundest reading is:
- A. no issue — the two systems serve different purposes
- B. the divergence needs explaining: an input measure inflated by inefficiency can recognise revenue ahead of performance ✅
- C. the earned-value figure must be wrong
- D. revenue should be restated to 48 % automatically

*Rationale:* Cost-input measures reward spending, so inefficiency can raise apparent progress
(2.3.2) — the gap is a signal to investigate, not to dismiss (A) or resolve by assumption (C, D).

**MCQ 2.3-E `[2.3.1 · Application]`** `CFADS` before working capital 6,984,000; payables held at
300,000; revenue 12,000,000; receivables opened at nil; debt service 5,009,635.23. The maximum days
sales outstanding consistent with a 1.20× covenant, at a 365-day convention, is:
- A. 27.3750 days
- B. 31.0845 days
- C. 38.7033 days ✅
- D. 45.0000 days

*Rationale:* The 1.20× trigger is `CFADS` 6,011,562.28, allowing 972,437.72 of absorption and
therefore receivables of 1,272,437.72 — `1,272,437.72/12,000,000 × 365`. A is the actual position,
not the limit; B is the 1.25× *distribution* threshold, the commonest confusion because both appear
in the same clause; D is the scenario that breaches, at `DSCR` 1.1587.

**MCQ 2.3-F `[2.3.4 · Application]`** A restoration obligation of 4,500,000 falls due in 25 years and
is discounted at 5.0 %. The amount recognised and the first-year charge against profit are:
- A. provision 4,500,000; charge 180,000
- B. provision 1,328,862.47; charge 119,597.62 ✅
- C. provision 1,328,862.47; charge 66,443.12
- D. provision 1,328,862.47; charge nil, since no cash moves

*Rationale:* The provision is recognised at present value, and the first year carries both accretion
66,443.12 and depreciation of the capitalised restoration asset 53,154.50. A recognises the
undiscounted amount; C takes the accretion alone and forgets the asset it created; D confuses a nil
cash effect with a nil charge.

**MCQ 2.3-G `[2.3.2 · Analysis]`** A contractor has recognised 3,857,142.86 of cumulative profit on a
48,000,000 contract when a re-forecast makes expected total costs 49,500,000. The charge in the period
is:
- A. 1,500,000 — the expected loss
- B. 5,357,142.86 — the expected loss plus the reversal of profit already recognised ✅
- C. 1,500,000 spread over the remaining life of the contract
- D. nil until the loss is actually incurred

*Rationale:* The cumulative position must move from +3,857,142.86 to −1,500,000, so the period charge
is the whole swing (2.3.2). A forgets the reversal; C is precisely what immediate recognition exists
to prevent; D applies a cash-basis instinct to an accrual test.

### Self-check — KA 2.3

1. *Why can two projects with identical economics report different `DSCR`s?* — Because `CFADS` is
   a defined term; its definition (e.g. before or after working capital) changes the ratio.
2. *What does the capex/opex choice change, and what does it not?* — Reported profit and the asset
   base; pre-tax cash is unchanged, and after-tax cash may not be.
3. *When is a provision recognised rather than disclosed?* — Present obligation from a past event,
   probable settlement, reliable estimate.
4. *State Kestrel's 1.25× distribution condition as a collection period.* — A maximum `DSO` of
   31.0845 days against an actual 27.3750 — headroom of 3.7095 days.
5. *Why does an onerous-contract charge exceed the loss?* — Because the profit already recognised
   must be reversed as well: `1 + profit recognised ÷ loss`, here 3.5714 times.
6. *What does a provision's accretion plus its depreciation sum to over the life?* — The settlement
   amount, 4,500,000; accrual accounting allocates timing, not totals.

---

## Knowledge Area 2.4 — Ratio interpretation and project interfaces

*Topics: 2.4.1 the ratio families · 2.4.2 interest cover and the leverage view · 2.4.3 project
systems and corporate reporting.*

### 2.4.1 The ratio families

Ratios are comparisons, and their only value is in what they are compared *against* — the same
entity over time, or a genuine peer on the same policies (KA 2.1.2's warning). Four families:

| Family | Asks | Examples |
|---|---|---|
| **Liquidity** | Can it pay what falls due soon? | Current ratio, quick ratio |
| **Leverage** | How much of the funding is debt? | Debt/equity, debt/`EBITDA` |
| **Coverage** | Can earnings or cash service the debt? | Interest cover, `DSCR` (Domain 10) |
| **Return** | What is earned on the capital used? | Return on equity, return on capital employed |

For project finance the **coverage** family dominates, because a ring-fenced SPV's whole credit
case is whether its cash flow services its debt (Domain 1, KA 1.1.2). But a set read together says
more than any member of it, and the completed balance sheet of KA 2.2.2 makes it possible to compute
the whole set and then test it against something external — the cost of capital Domain 9 derived.

**Worked example 2.4.1 — Kestrel's full ratio set, and what it is worth against the cost of capital.**

1. **Setup.** From the completed statements: `EBITDA` 7,500,000; `EBIT` 5,100,000; net income
   2,064,000; receivables 900,000; cash 1,374,364.77; payables 300,000; senior debt 39,510,364.77;
   equity 20,064,000. Opening debt was 42,000,000 and opening equity 18,000,000. The senior rate is
   6.0 % and the tax rate 20 %. Domain 9 (KA 9.1.4) derived the project `WACC` at **7.9860 %**, the
   cost of equity at 70 % gearing at **15.42 %** and the equity `IRR` at **12.5311 %**.
2. **Formula.** Current ratio = current assets ÷ current liabilities. Debt/equity = debt ÷ equity.
   Gearing = debt ÷ (debt + equity). Net debt = debt − cash. Return on capital employed =
   `EBIT` ÷ (debt + equity), and after tax = `EBIT`(1 − T) ÷ (debt + equity). Return on equity = net
   income ÷ equity. After-tax cost of debt = rate × (1 − T). The leverage identity is
   `ROE = ROCE_after-tax + (D/E) × (ROCE_after-tax − k_d after tax)`.
3. **Substitution.** `(900,000 + 1,374,364.77) / 300,000`; `39,510,364.77 / 20,064,000`;
   `39,510,364.77 / 59,574,364.77`; `39,510,364.77 − 1,374,364.77`; `5,100,000 / 59,574,364.77`;
   `2,064,000 / 20,064,000`; and on opening bases `4,080,000 / 60,000,000` and `2,064,000 / 18,000,000`.
4. **Result.**

   | Ratio | Value | Basis |
   |---|---|---|
   | Current ratio | **7.5812** | no inventory; cash is 60.4285 % of current assets |
   | Debt/equity | **1.9692** | closing balances |
   | Gearing | **66.3211 %** | closing; 70.00 % at financial close |
   | Net debt | **38,136,000.00** | debt less cash |
   | Net debt/`EBITDA` | **5.0848** | against gross 5.2680 |
   | `ROCE`, pre-tax | **8.5607 %** | `EBIT` ÷ closing capital employed |
   | `ROCE`, after tax, opening capital | **6.8000 %** | 4,080,000 ÷ 60,000,000 |
   | `ROE`, closing equity | **10.2871 %** | |
   | `ROE`, average equity | **10.8449 %** | 0.5578 points higher on the same profit |
   | `ROE`, opening equity | **11.4667 %** | the basis on which the leverage identity is exact |
   | After-tax cost of debt | **4.8000 %** | 6.0 % × (1 − 0.20) |

5. **Interpretation.** Four readings, in ascending order of usefulness.

   **The leverage identity holds exactly — but only on one basis.** On opening balances,
   `6.8000 % + 2.3333 × (6.8000 % − 4.8000 %) = 11.4667 %`, which is the return on opening equity to
   the digit. That is the whole of what gearing does: it earns the spread between the return on
   capital and the after-tax cost of debt, 2.0000 points here, and multiplies it by the debt-to-equity
   ratio. Two corollaries follow immediately. Leverage is accretive only while `ROCE` exceeds the
   after-tax cost of debt, and it becomes *destructive* below it at the same multiple — the reason
   Domain 9 treats gearing as a risk transfer rather than a value creator. And **the identity breaks
   the moment the bases are mixed**: computed on closing equity the same year's `ROE` is 10.2871 %, on
   average equity 10.8449 %, on opening equity 11.4667 % — a spread of **1.1796 points on identical
   performance**, purely from a choice of denominator. A reviewer's first question about any return
   ratio is which balance it uses, and the answer is right in a footnote or it is nowhere.

   **The accounting return is below the cost of capital, and the project is still worth doing.**
   After-tax `ROCE` of 6.8000 % against a `WACC` of 7.9860 % is a shortfall of **1.1860 points**, and
   yet Domain 4 computed an `NPV` of **+16,179,360** at 8 % and an `IRR` of **12.19 %** on the same
   project. Both are correct. A first-year accounting return is a single-period ratio on a
   fully-depreciating asset at its maximum carrying amount, and it rises mechanically as the asset
   depreciates and the debt amortises: with `EBIT` unchanged, year two's after-tax `ROCE` is already
   **6.8486 %**, because capital employed fell 425,635.23 without anything happening. **Accounting
   returns are the wrong instrument for judging project economics** — that is what Domains 3 and 4
   exist for — and a board comparing a project's first-year `ROCE` with a corporate hurdle rate will
   reject investments it should make.

   **`ROE` is not the equity return either.** The 10.2871 % here sits **5.1329 points below** the
   15.42 % Domain 9 identifies as the cost of equity at this gearing, and **2.2440 points below** the
   12.5311 % equity `IRR` the same structure actually delivers over twelve years. An accounting `ROE`
   on one early year of a long-dated asset is not comparable with either, and quoting it against a
   required return is a category error, not a conservative estimate.

   **The liquidity ratios are the least informative in the set and the most often quoted.** A current
   ratio of 7.5812 looks extraordinary and means very little: 60.4285 % of those current assets is a
   cash balance of which **91.1264 % is contractually restricted** (KA 2.2.2), and the payables it is
   measured against are a single month of chemicals and power. On a ring-fenced SPV with a reserve
   account and a defined waterfall, liquidity is governed by the finance documents, not by a ratio —
   which is precisely why lenders covenant coverage and leave the current ratio to general-purpose
   analysis.

> **Fig 2.4.1 — Kestrel's return ladder, year one.** Horizontal bar chart, x-axis per cent 0–16,
> six bars in ascending order with the brand palette: after-tax cost of debt **4.8000** (slate),
> after-tax `ROCE` on opening capital **6.8000** (blue), `WACC` **7.9860** (slate, dashed outline —
> the external benchmark), `ROE` on opening equity **11.4667** (blue), equity `IRR` **12.5311**
> (ink), cost of equity **15.42** (crimson). A bracket between 6.8000 and 11.4667 annotated
> "the leverage wedge: 2.3333 × 2.0000 points = **4.6667**", and a crimson gap marker between
> 6.8000 and 7.9860
> annotated "first-year accounting return is 1.1860 points below `WACC` — and `NPV` is still
> +16,179,360". Source: PCI original. Alt text: six ascending bars from the after-tax cost of debt
> at under five per cent to the cost of equity at over fifteen, with the accounting return on capital
> sitting just below the weighted average cost of capital.

### 2.4.2 Interest cover and the leverage view

**Worked example 2.4.2 — Kestrel's cover and gearing.**

1. **Setup.** `EBIT` USD 5,100,000; interest USD 2,520,000; senior debt at year end
   USD 39,510,365; `EBITDA` USD 7,500,000.
2. **Formula.** Interest cover = `EBIT` ÷ interest. Debt/`EBITDA` = debt ÷ `EBITDA`.
3. **Substitution.** `5,100,000 / 2,520,000`; `39,510,365 / 7,500,000`.
4. **Result.** Interest cover **2.02×**; debt/`EBITDA` **5.27×**.
5. **Interpretation.** Both are true and they point in different directions, which is the point of
   reading a *set*. Cover of 2.02× says earnings comfortably absorb the interest charge; debt of
   5.27× `EBITDA` says the balance sheet is heavily geared — normal for contracted infrastructure
   and alarming for a merchant business, because what makes high leverage tolerable is **revenue
   certainty**, not the ratio itself (Domain 7's revenue models; Domain 11's risk allocation). Note
   too that interest cover is an *accrual* measure and ignores principal entirely: Kestrel must
   also find USD 2,489,635 of principal, which is why `DSCR` (1.27 including working capital,
   2.3.1) is the ratio a lender actually covenants.

   Three refinements turn this from a pair of numbers into a judgment. **There are two interest
   covers and they differ materially.** On `EBIT` the ratio is **2.0238×**; on `EBITDA` — the
   variant often called cash interest cover — it is **2.9762×**, because depreciation of 2,400,000 is
   added back. Neither is wrong and the gap between them is 0.9524 of coverage, so a covenant
   defined on "interest cover" without specifying the numerator has left almost a full turn of
   coverage to interpretation. That is the same definitional exposure as `CFADS` (KA 2.3.1), on a
   ratio nobody thinks of as contestable.

   **The tighter covenant is not the one that looks tighter.** Suppose the facility carried both a
   1.20× `DSCR` covenant and a 2.00× interest-cover covenant — an entirely ordinary pairing. The
   `DSCR` test bites at `CFADS` of 6,011,562.28, a headroom of **372,437.72**, which at Kestrel's
   0.80 cash-to-revenue gearing is **465,547.16** of revenue, or **3.8796 %**. The interest-cover
   test bites at `EBIT` of `2.00 × 2,520,000 =` **5,040,000**, a headroom of only **60,000** of
   `EBIT` and therefore of revenue — **0.5000 %**. The `DSCR` covenant tolerates **7.7591 times**
   as large a revenue miss as the interest-cover covenant does. A lender adding an interest-cover
   test "for comfort" beside a coverage test has, on these numbers, created the binding covenant and
   quite possibly not noticed; a sponsor negotiating hard on the `DSCR` threshold while conceding the
   interest-cover threshold has negotiated the wrong clause. **Which covenant binds is an arithmetic
   question to be answered at close, not a matter of convention** — and the answer belongs on the
   covenant dashboard (Domain 10, Toolkit 10.T.2) in revenue units, not ratio units.

   **Finally, leverage measured on book values decays without any transaction.** Debt/`EBITDA` of
   5.2680× falls every year as principal amortises, even if the project never improves: it is a
   statement about an amortisation schedule as much as about the business. Net debt/`EBITDA` of
   **5.0848×** is lower again, and the 0.1832 difference is entirely the cash balance — **91.1264 %
   of which is a restricted reserve** (KA 2.2.2). Whether a covenant is struck on gross or net debt,
   and whether restricted cash counts, is worth more than most of the basis-point negotiation that
   surrounds it.

### 2.4.3 Project systems and corporate reporting

A project's cost system and the corporate accounts describe the same money in different languages,
and the interfaces are where reconciliation errors and disputes live. The four that matter:

- **Commitments versus actuals.** A purchase order commits funds; accounting recognises cost on
  delivery or performance. Project systems track both; the ledger recognises one (PML-AI,
  Domain 7, KA 7.2.1).
- **Accruals at period end.** Work received but not invoiced must reach the ledger, or cost
  performance flatters and then lurches — the same defect PML-AI diagnoses as a stepped `CPI`.
- **Capitalisation boundaries.** Which project costs form part of the asset (and from what date
  capitalisation begins and ends) determines both the depreciation base and reported profit
  (2.3.3). Borrowing costs during construction are a specific instance — capitalised into the
  asset while it is being built, expensed thereafter.
- **Cut-off and period discipline.** A cost is in one period only. Projects run continuously and
  ledgers close monthly; every reconciliation dispute is ultimately a cut-off question.

**The leader's obligation** is not to run these reconciliations but to insist they exist and to
know which number is being quoted. "The project has spent USD 40 million" can mean committed,
incurred, invoiced, paid or capitalised — five different figures, all defensible, differing by
material amounts. How material is worth computing once, because the answer changes how the question
is asked ever afterwards.

**Worked example 2.4.3 — five answers to "how much has the project spent?".**

1. **Setup.** Kestrel's construction at the quarter-five data date, on Domain 14's figures.
   Cumulative certified spend is **33,945,403** (KA 14.2.1: `AC` of 33,480,000 on the control
   accounts plus 465,403 of certified variations). Remaining committed contract value is
   **18,720,000** and approved-but-uncertified variations **840,000**. Of the certified value,
   **1,205,403** has been received but not yet invoiced. Against invoices raised, **1,200,000** of
   retention is withheld (the 5 % regime capped at 2.5 % of the contract price — Domain 14,
   KA 14.3.2), **358,000** of trade invoices are approved and unpaid, and of the **4,800,000**
   advance payment made at contract signature **61 %** has been recovered against certifications.
   Of the amounts incurred, **620,000** of owner's general and administrative costs fail the
   capitalisation test, and capitalised interest incurred to date is **677,923** (the 2,114,597 total
   less the 1,436,674 remaining — KA 14.1.1). Produce all five figures and reconcile them.
2. **Formula.** Committed = incurred + open commitments. Invoiced = incurred − accrued not invoiced.
   Cash paid = invoiced − advance recovery applied − retention withheld − unpaid invoices + the
   advance itself. Capitalised = incurred − non-capitalisable costs + capitalised interest. Then test
   the reconciliation.
3. **Substitution.** Open commitments `18,720,000 + 840,000 = 19,560,000`; invoiced
   `33,945,403 − 1,205,403`; advance recovered `4,800,000 × 0.61 = 2,928,000`; paid
   `32,740,000 − 2,928,000 − 1,200,000 − 358,000 + 4,800,000`; capitalised
   `33,945,403 − 620,000 + 677,923`.
4. **Result.**

   | Measure | USD | What it is |
   |---|---|---|
   | **Committed** | **53,505,403** | Contractual entitlement placed, whether performed or not |
   | **Capitalised** | **34,003,326** | The carrying amount added to the asset |
   | **Incurred** | **33,945,403** | Value received and certified, invoiced or not |
   | **Paid** | **33,054,000** | Cash out of the account, including the unrecovered advance |
   | **Invoiced** | **32,740,000** | Supplier claims received into the ledger |

   The reconciliation, which must close exactly:

   ```
   Cash paid                                33,054,000
   + retention withheld                      1,200,000
   + trade invoices approved and unpaid         358,000
   + certified value not yet invoiced         1,205,403
   + open commitments                        19,560,000
   − unrecovered advance payment            ( 1,872,000)
   = committed                              53,505,403   ✓
   ```

5. **Interpretation.** The headline is the **spread of 20,451,403** between the largest and smallest
   answer — **34.0857 %** of the whole 60,000,000 envelope — on a single date, from one ledger, with
   every figure defensible and auditable. That is the arithmetic behind the discipline: a question
   answered with a number rather than with a measure is not an answer.

   Three specific traps sit inside the reconciliation, and each has cost projects money.

   **Capitalised exceeds incurred.** By **57,923** here, which offends the intuition that the
   balance-sheet figure must be a subset of what has been spent. It exceeds because capitalised
   interest of 677,923 enters the asset without any supplier ever invoicing for it, while 620,000 of
   owner's costs leave through the income statement. A reviewer reconciling a fixed-asset note to a
   cost report and expecting the asset to be the smaller number will look for an error that is not
   there — and, worse, may not look for the 620,000 that genuinely did fail the capitalisation test
   (KA 2.3.3), which is the item with a covenant consequence.

   **The two figures that look closest are the least related.** Incurred (33,945,403) and paid
   (33,054,000) differ by only **891,403**, which reads as near-agreement and is nothing of the kind:
   it is the net of four unrelated items — retention, unpaid invoices, uninvoiced value and an
   unrecovered advance — totalling **4,635,403** gross, or **5.2001 times** the apparent gap. Two of
   those items push one way and two the other, and a change in any of them moves cash without moving
   cost. A cash forecast built by adjusting the cost report for "the usual lag" is built on the
   coincidence that the four currently offset.

   **The advance payment carries the opposite sign to everything else.** The unrecovered 1,872,000 is
   cash gone and value not yet received — a prepayment asset, not a cost — so it *reduces* committed
   spend when reconciling from cash. It is the single most common sign error in a construction cash
   reconciliation, and the reason Domain 14 (KA 14.3.2) treats the advance and its recovery as a named
   line rather than a netting adjustment.

   The leader's habit that follows is small and non-negotiable: **name the measure in the sentence**.
   "The project has incurred 33.9 million, paid 33.1 million and committed 53.5 million" takes nine
   more words than "the project has spent 34 million" and forecloses an entire class of dispute. The
   figures also serve different decisions — the in-balance test uses commitments (Domain 14,
   KA 14.2.3), the drawdown uses certified value, the accounts use the capitalised amount, and
   treasury uses cash — so there is no single "right" answer to be settled on, only a right measure
   for each question. The split between capitalisable and non-capitalisable cost illustrated here is
   indicative; where the capitalisation boundary falls for a particular project is a
   framework-and-facts question for the entity's finance function and auditors.

### AI in this KA

**Where it earns its place.** Ratio computation across many periods and entities, restatement onto a
common basis, and — most valuably — the translation this Knowledge Area is built on: taking a covenant
threshold and expressing it in revenue, days or driver units so that the binding test can be
identified rather than assumed. That is mechanical arithmetic on stated definitions, it is the work
practitioners most often leave undone, and it is verifiable line by line.

**Where it fails, specifically.** Three failures, each of which produces a confident wrong answer
rather than an error message. An assistant asked for "return on equity" will pick a denominator
without saying which, and opening, average and closing bases differ by **1.1796 points** on Kestrel's
single year (2.4.1) — the leverage identity then appears to fail and the model will rationalise the
discrepancy rather than report the inconsistency. An assistant comparing two entities' ratios will not
ask what policies produced the inputs, so a capitalisation difference (2.3.3) reads as a performance
difference. And asked how much a project has spent, it will return whichever of the five measures the
source document happened to contain, with no indication that four others exist (2.4.3). The governed
habit is to require every ratio to arrive with its numerator, denominator, basis and defining document
named — and to treat a ratio that cannot state them as unreported. **AI proposes; the professional
verifies, decides and remains accountable.**

### Key terms — KA 2.4

| Term | Meaning |
|---|---|
| **Interest cover** | `EBIT` ÷ interest; an accrual coverage measure that ignores principal. |
| **Cash interest cover** | `EBITDA` ÷ interest; the same test with depreciation added back — 0.9524 higher for Kestrel. |
| **Debt/`EBITDA`** | A leverage measure; tolerable level depends on revenue certainty. |
| **`ROCE` / `ROE`** | Return on capital employed and on equity; linked by the leverage identity, exact only on consistent bases. |
| **Leverage wedge** | `(D/E) × (ROCE after tax − k_d after tax)`; the amount gearing adds to `ROE`, and subtracts when the spread is negative. |
| **Capitalisation boundary** | Which costs and which dates form part of the asset. |
| **Cut-off** | Assigning a transaction to exactly one period. |
| **Commitment vs actual vs paid** | Distinct measures of "spend" that must never be conflated. |
| **Unrecovered advance** | Cash paid for value not yet received; a prepayment asset that reconciles with the opposite sign. |

### Sample MCQs — KA 2.4

**MCQ 2.4-A `[2.4.2 · Application]`** `EBIT` 5,100,000 and interest 2,520,000. Interest cover is:
- A. 1.27×
- B. 2.02× ✅
- C. 2.98×
- D. 0.49×

*Rationale:* `5,100,000/2,520,000 = 2.02`. A is the `DSCR` from 2.3.1; C uses `EBITDA` in the
numerator; D inverts the ratio.

**MCQ 2.4-B `[2.4.2 · Analysis]`** A project shows interest cover 2.02× and debt/`EBITDA` 5.27×.
The soundest interpretation is:
- A. the ratios contradict each other, so one must be wrong
- B. earnings service the interest comfortably while leverage is high — tolerable given contracted revenue, but dependent on that certainty ✅
- C. the project is over-leveraged regardless of revenue structure
- D. interest cover is the only relevant measure for a lender

*Rationale:* The two measure different things and both are true (2.4.2); what makes high leverage
acceptable is revenue certainty. D ignores principal, which is why `DSCR` is covenanted.

**MCQ 2.4-C `[2.4.3 · Analysis]`** A sponsor asks "how much has the project spent?". The
professional response is:
- A. quote the paid figure, as it is the most conservative
- B. ask which measure is meant — committed, incurred, invoiced, paid or capitalised — since they differ materially and all are defensible ✅
- C. quote the committed figure, as it is the most complete
- D. quote the capitalised figure, since it appears in the accounts

*Rationale:* Five defensible figures exist (2.4.3); answering without establishing which one is
meant guarantees a later dispute. Each of A, C and D picks one arbitrarily.

**MCQ 2.4-D `[2.4.1 · Evaluation]`** A project's first operating year shows after-tax return on
capital employed of 6.8000 % against a `WACC` of 7.9860 %, while the appraisal recorded an `NPV` of
+16,179,360 and an `IRR` of 12.19 %. The soundest conclusion is:
- A. the appraisal was over-optimistic and should be revisited
- B. a single early-year accounting return on a fully-carried asset is not comparable with a lifetime discounted return; both figures are correct ✅
- C. the project is destroying value and should be restructured
- D. the `WACC` must be wrong, since the project was approved

*Rationale:* Accounting `ROCE` in year one is measured on the asset at its maximum carrying amount
and rises mechanically as it depreciates (2.4.1) — year two is already 6.8486 % on unchanged `EBIT`.
A and C treat a single-period ratio as a verdict on lifetime economics, the error Domains 3 and 4
exist to prevent; D reverses the logic of appraisal.

**MCQ 2.4-E `[2.4.2 · Analysis]`** A facility carries both a 1.20× `DSCR` covenant and a 2.00×
interest-cover covenant. `CFADS` is 6,384,000, debt service 5,009,635.23, `EBIT` 5,100,000, interest
2,520,000, revenue 12,000,000 and the cash-to-revenue gearing 0.80. The binding covenant is:
- A. the `DSCR` covenant, because coverage tests are stricter in project finance
- B. the interest-cover covenant: it tolerates a 0.5000 % revenue fall against the `DSCR` covenant's 3.8796 % ✅
- C. neither — they bite at the same point by construction
- D. it cannot be determined without the lock-up threshold

*Rationale:* Restating both in revenue units gives 60,000 of headroom on interest cover against
465,547.16 on `DSCR`, a factor of 7.7591 (2.4.2). A substitutes convention for arithmetic; C asserts
a relationship that does not exist; D treats a separate distribution test as necessary to a covenant
comparison.

**MCQ 2.4-F `[2.4.3 · Analysis]`** At a construction data date a project has incurred 33,945,403 and
capitalised 34,003,326. The most likely explanation is:
- A. an error, since capitalised cost cannot exceed cost incurred
- B. capitalised interest entered the asset without a supplier invoice, more than offsetting costs that failed the capitalisation test ✅
- C. the asset has been revalued upwards
- D. retention withheld has been added to the asset

*Rationale:* Capitalised interest of 677,923 exceeds the 620,000 of non-capitalisable owner's costs,
so the asset legitimately carries 57,923 more than the cost report shows (2.4.3). A applies an
intuition the reconciliation disproves; C invents a transaction; D confuses a payment timing item
with a cost.

### Self-check — KA 2.4

1. *Why does project finance emphasise coverage over return ratios?* — A ring-fenced SPV's credit
   case is whether its cash services its debt.
2. *What does interest cover ignore?* — Principal repayment; hence `DSCR`.
3. *Name the five meanings of "spend".* — Committed, incurred, invoiced, paid, capitalised.
4. *State the leverage identity and where it fails.* —
   `ROE = ROCE_at + (D/E)(ROCE_at − k_d,at)`; exact on consistent (opening) bases, and broken by
   mixing opening, average and closing denominators — worth 1.1796 points of `ROE` here.
5. *Which of Kestrel's two possible covenants binds, and by how much?* — The 2.00× interest-cover
   test, tolerating a 0.5000 % revenue fall against the `DSCR` covenant's 3.8796 %.
6. *Why can capitalised cost exceed cost incurred?* — Capitalised interest enters the asset without
   an invoice, while non-capitalisable costs leave through the income statement.

---

## Advanced topics — Domain 2

### 2.A.1 Deferred tax, in principle

Accounting profit and taxable profit differ — most commonly because tax depreciation (capital
allowances) runs on a different profile from accounting depreciation. Where the difference is
**temporary**, deferred tax recognises the future consequence: accelerated tax depreciation
reduces tax now and creates a **deferred tax liability** that unwinds as the difference reverses.
For project models the practical significance is that **cash tax and accounting tax are different
lines with different timing**, and it is cash tax that enters `CFADS` (Domain 6's model, Domain 10's
ratios). Modelling accounting tax as if it were cash tax is a standard model-audit finding.

**Worked example 2.A.1 — the deferred tax liability Kestrel's first year creates.**

1. **Setup.** The master statements assume tax depreciation equals accounting depreciation, so no
   temporary difference arises and tax of 516,000 is both charged and paid. Suppose instead the
   jurisdiction grants **declining-balance capital allowances at 15 %** on the 60,000,000 base, with
   losses carried forward — the assumption Domain 6 (KA 6.2.3) prices on the cash side. Accounting
   depreciation stays at 2,400,000 and the tax rate at 20 %. To isolate the fixed-asset difference,
   assume no deferred tax asset is recognised on the carried-forward loss — an assumption stated
   because it is itself a recognition judgment, and one returned to below. Compute the temporary
   difference, the deferred tax liability, the accounting tax charge and the resulting net income, and
   identify when the difference begins to reverse.
2. **Formula.** Temporary difference = carrying amount − tax written-down value. Deferred tax
   liability = temporary difference × tax rate. Total tax charge = current (cash) tax + movement in
   the deferred tax liability. The difference begins to reverse in the first year the capital
   allowance falls below accounting depreciation.
3. **Substitution.** Year-one allowance `60,000,000 × 0.15 = 9,000,000`, so tax written-down value
   `51,000,000` against a carrying amount of `57,600,000`; difference `6,600,000`; liability
   `6,600,000 × 0.20`. Domain 6 computes the year's cash tax as **nil**, the allowance and interest
   having produced a tax loss of 4,020,000 carried forward.
4. **Result.**

   | Line | Master assumption | 15 % declining balance |
   |---|---|---|
   | Accounting depreciation | 2,400,000 | 2,400,000 |
   | Capital allowance claimed | 2,400,000 | **9,000,000** |
   | Carrying amount / tax written-down value | 57,600,000 / 57,600,000 | 57,600,000 / **51,000,000** |
   | Temporary difference | nil | **6,600,000** |
   | **Deferred tax liability** | nil | **1,320,000** |
   | Current (cash) tax | 516,000 | **nil** |
   | **Total tax charge** | 516,000 | **1,320,000** |
   | **Net income** | 2,064,000 | **1,260,000** |
   | `CFADS` / `DSCR` | 6,384,000 / 1.2743 | 6,900,000 / **1.3773** |

5. **Interpretation.** The two columns describe the same project in the same year, and **they move in
   opposite directions**: net income falls by **804,000** while `DSCR` rises by **0.1030**. That
   opposition is the whole content of this topic. Accelerated allowances defer cash tax, which is
   good for coverage; the deferred tax charge that records the future consequence is bad for profit;
   and an entity reporting both is neither better nor worse off than the accounting suggests. A
   sponsor presenting the profit column to a board and the coverage column to lenders is presenting
   two true statements and one misleading impression.

   Three disciplines follow. **The liability is not a debt and must not be treated as one.** It is
   not owed to anyone on any date, carries no interest and is not enforceable; leverage covenants
   defined on "total liabilities" rather than on financial indebtedness can capture it, and a
   covenant that tightens because a tax authority granted an allowance is a drafting accident worth
   finding before signature. **The reversal is datable, and the date matters.** With a 15 % declining
   balance the allowance falls below 2,400,000 for the first time in **year 10** (2,084,552.52
   against 2,400,000), the temporary difference peaks in **year 9** at **24,502,983.22** — a deferred
   tax liability of **4,900,596.64** — and unwinds thereafter. So the profile is a decade of profit
   suppressed by deferred tax followed by years of profit flattered by its release, on a project whose
   trading may be perfectly flat: any trend read off the profit line across that turning point is an
   artefact. **And the deferred tax asset side is where optimism hides.** Where the allowance creates
   a tax loss, recognising a deferred tax asset on it requires convincing evidence of future taxable
   profit against which to use it, which is a forecast, not a fact. Whether such an allowance regime
   exists, whether losses carry forward and for how long, whether a deferred tax asset may be
   recognised, and at what rate the balances are measured are all jurisdiction-specific and subject to
   legislative change over a twelve-year loan; the arithmetic above is transferable, the treatment is a
   matter for qualified tax advice, and Domain 11's risk register — not the model's base case — is
   where a legislative assumption belongs.

### 2.A.2 Leases and off-balance-sheet intuitions

Under current thinking a lessee generally recognises a right-of-use asset and a lease liability,
so the older intuition that operating leases keep obligations off the balance sheet no longer
holds (IFRS 16 is the reference framework, in principle). Two consequences for a finance leader:
leverage ratios computed across periods spanning the change are not comparable, and covenant
definitions written before it may capture or exclude lease liabilities in ways nobody intended —
a live reason to read the *definitions* in finance documents rather than assume them.

The size of the effect is worth computing, because the intuition is usually wrong in an instructive
way. Suppose Kestrel leases its intake corridor and site access for **USD 500,000** a year over ten
years, discounted at **6.0 %**. At `AF(6 %, 10) = 7.360087` the liability recognised is
**USD 3,680,043.53** — **9.3141 %** of the senior debt, a substantial-looking addition. Yet the
leverage ratio barely moves, because the same change removes 500,000 from operating costs and
replaces it with depreciation and interest:

| Measure | Before | After |
|---|---|---|
| Debt for the ratio | 39,510,364.77 | **43,190,408.30** |
| `EBITDA` | 7,500,000 | **8,000,000** |
| Debt/`EBITDA` | **5.2680×** | **5.3988×** |

A liability of 3,680,043.53 moved the ratio by **0.1308** — because the numerator and the denominator
both rose. Compute it the way an unwary analyst would, adding the liability while leaving `EBITDA`
untouched, and the answer is **5.7587×**, an apparent deterioration of 0.4907 that is **3.75 times**
the real one. Two conclusions, and neither is about leases. **A ratio is only comparable when every
line in it is restated consistently**, which is the same discipline as KA 2.4.1's return bases. And
**the covenant definition decides the outcome, not the accounting framework**: where the test is
struck on "financial indebtedness" as defined in the facility agreement, a lease liability recognised
under a reporting framework may not enter the covenant at all, and the ratio the lender enforces does
not move by 0.1308 or by anything else. Which of those applies to a given facility is a question for
the finance documents and for counsel, and is exactly the sort of matter Toolkit 2.T.1 exists to
record.

### 2.A.3 The reviewer's statement eye

Invariants to test on any statement set before relying on it: the balance sheet balances; closing
cash on the cash-flow statement equals balance-sheet cash; closing equity equals opening plus
profit less distributions plus contributions; depreciation in the cash-flow statement equals the
income-statement charge; the movement in debt equals drawings less principal repaid; principal
appears in financing and interest in operating (or as disclosed); working-capital movements
reconcile to balance-sheet deltas; and any ratio quoted can be recomputed from the face of the
statements. A set that fails any of these has an error, an omission, or a policy that needs
explaining — and the failure point localises it.

### 2.A.4 Distributable reserves — the constraint that is not in the waterfall

A cash waterfall says what money is *available* to distribute (Domain 15, KA 15.2.3). It does not say
what an entity is *permitted* to distribute, and in most company-law systems those are different
questions with different answers. The permission side typically turns on **accumulated realised
profits** — a balance-sheet test on retained earnings, not a cash test — so an SPV can hold cash the
finance documents release and still be unable lawfully to pay it out.

The point is easiest to see at the end of construction, where it bites hardest. The master statements
in KA 2.2 capitalise the whole 60,000,000 envelope into the plant, for exposition; take instead Worked
example 2.4.3's illustration, in which **620,000** of owner's general and administrative costs fail the
capitalisation test and are expensed, so that plant is carried at **59,380,000** and retained earnings
open at **−620,000**. Kestrel then reaches its commercial operations date with no distributable reserve
whatever, while holding cash and having satisfied every condition to a drawdown. Year one's net income of 2,064,000 restores the balance to **1,444,000**, and
Domain 6's first-year distribution of **121,955.96** is **8.4457 %** of it — comfortable, but only
because the year was profitable. Three consequences a leader should hold.

**The binding test can switch between cash and reserves without warning.** In year one the
constraint is cash and the waterfall: 121,955.96 of distributable cash against 1,444,000 of reserves.
In a year with an accounting loss — a deferred tax charge (2.A.1), a decommissioning re-measurement
(2.3.4), an impairment — the reserves can become the binding constraint while `CFADS` is untouched, so
a project passing its `DSCR` test comfortably makes no distribution at all. That is the mirror image
of Case study B and it surprises sponsors more, because nothing in the finance documents predicts it.

**Accounting judgments therefore have a distribution consequence even where covenants are cash-based.**
The capitalisation boundary decided how much of the owner's cost was expensed; expensing it created the
negative opening reserve. A decision taken on technical accounting grounds during construction set the
date on which equity could first be paid.

**And this is a legal question before it is an accounting one.** What constitutes a distributable
profit, whether interim accounts may support a distribution, what solvency or net-asset test applies
alongside it, and what the consequences of an unlawful distribution are for the directors who declared
it and the shareholders who received it, vary materially between jurisdictions and are matters for
counsel in the jurisdiction of incorporation. The practical discipline is narrow: **include a
distributable-reserves line beside the distributable-cash line in every distribution paper**, and
confirm the legal test with counsel once per financing rather than once per dividend.

---

## Industry variations — Domain 2

- **Energy and resources.** Decommissioning provisions are first-order balance-sheet items,
  recognised decades before the cash flows; commodity revenue makes recognition timing and
  hedge accounting material.
- **Transport concessions and PPPs.** The central question is what the operator actually holds — a
  financial asset (an unconditional right to cash from the grantor), an intangible right to charge
  users, or a mixture — and the answer changes the whole statement profile for identical
  economics. Framework treatment is specialised and advice-led.
- **Water and regulated utilities.** Regulatory asset bases and their depreciation profiles may
  diverge from statutory accounting; two "asset values" coexist and must never be conflated.
- **Digital infrastructure.** Shorter useful lives and heavy refresh capex make the capex/opex
  boundary (2.3.3) a recurring judgment with large profit consequences.
- **Construction and EPC.** Over-time revenue recognition and onerous-contract provisions are the
  operative treatments, and the input-versus-output progress choice (2.3.2) is where accounting
  and project controls must agree or explain.

## Case study — Domain 2: the profitable year that nearly broke a covenant (water)

**Situation.** Kestrel Water SPC's first operating year closes on the figures above: net income
USD 2,064,000, `EBITDA` USD 7,500,000 — a good year by any account. The finance director's draft
lender report quotes `DSCR` **1.39** and describes headroom as comfortable against the facility's
**1.25** distribution condition.

**Which threshold, and why it matters that the report picked one.** Kestrel's facility tests coverage
at the three levels tabulated in KA 2.3.1B — a 1.25× distribution condition, a 1.20× financial
covenant and a 1.15× lock-up trigger, whose consequences Domain 10 (KA 10.4) builds. The draft report
compared its figure against the distribution condition alone, which is the least severe of the three
and the only one whose failure costs a dividend rather than triggering an event of default.

**The problem.** The facility's definition of `CFADS` is struck **after** movements in working
capital. Kestrel's receivables had grown USD 900,000 as the offtaker's payment process settled
into a slower rhythm than modelled, against a USD 300,000 rise in payables — a net USD 600,000
absorbed. On the documented definition, `CFADS` is USD 6,384,000 and `DSCR` is **1.27**, not 1.39.
Headroom against the 1.25 distribution condition is USD 0.02 of ratio — **USD 121,956 of cash** —
not the comfortable margin reported. Against the 1.20 covenant the headroom is **USD 372,438**, or
**5.8 %** of `CFADS`, which is the figure Domain 10 (WE 10.2.1) computes and the one a board paper
should carry. And note what the definitional choice cost: on the pre-working-capital figure the
covenant headroom would have looked like **USD 972,438**, so **USD 600,000 — 61.7 % of the apparent
margin — was never there.**

**The margin, restated in the units that caused it.** The board's question — how close was this? —
has a better answer than 0.02 of ratio. Kestrel's `DSCR` of 1.2743 corresponds to a collection period
of **27.3750 days**; the 1.25 distribution condition is reached at **31.0845 days** and the 1.20
covenant at **38.7033 days** (KA 2.3.1B). The comfortable margin the draft report described was
therefore **3.7095 days of collection** before the dividend stopped, and 11.3283 days before the
covenant failed. At **32,876.71 of cash and 0.006563 of `DSCR` per day**, the offtaker's payment run
slipping by a single week would have cost 0.0459 of coverage on its own. Expressed that way the
finding was actionable inside a fortnight; expressed as two decimal places of a ratio it had sat in
three consecutive board packs.

**What was done.** The report was corrected before issue. More consequentially, the near-miss
changed three things: collections became a monitored operational metric with a named owner (not a
finance afterthought), reported weekly in days rather than quarterly in dollars; the model's
working-capital assumptions were re-based on nine months of actual collection behaviour rather than
the financial-close assumption; and the treasury team sized a working-capital facility as a liquidity
buffer, calibrated on the observed distribution of the offtaker's payment dates against the
**1,021,955.96** of receivables that the 1.25× condition tolerates. The following year's `DSCR` came
in at 1.41 on the documented definition.

**What the domain teaches here.** The professional content is definitional, and then it is
translational. A ratio is only as good as the defined term inside it, and the definition lives in
the finance documents, not in convention — that is the first lesson and the one that corrected the
report. The second is the one that changed the company: **a covenant that is not expressed in a unit
someone owns is not being managed.** "Maintain `DSCR` above 1.25" is owned by nobody; "keep `DSO`
below 31 days" is owned by a named person with a weekly number. It also shows the accrual/cash
divergence of KA 2.1.1 doing real damage — a genuinely profitable year came within USD 100,000 of a
covenant breach because of a balance-sheet movement no one was watching, and the movement was 3.7
days long.

## Case study B — Domain 2: capitalised into a better-looking year (digital infrastructure)

**Situation.** A data-centre operator overhauled cooling across three sites for USD 9,000,000,
capitalised as an enhancement and depreciated over 12 years — a year-one charge of USD 750,000
rather than USD 9,000,000, lifting reported profit by USD 8,250,000 relative to expensing it.

**What happened.** The auditors challenged the classification: on inspection, roughly two-thirds
of the spend restored the original design capacity rather than extending it, which is maintenance.
The restatement moved USD 6,000,000 into operating expense, turned a reported profit into a loss,
and breached a profit-based covenant in a holding-company facility. No cash flow changed at any
point, and the project's `DSCR` — a cash-based test — was unaffected throughout.

**What the domain teaches here.** Accounting classification is judgment applied to facts, and it
is auditable. The wider lesson is why lenders to projects prefer cash-based covenants: the
`DSCR` was indifferent to a classification argument that flipped the profit-based test from
comfortable to breached. A leader should know which of their covenants are exposed to accounting
judgment and which are not.

## Case study C — Domain 2: the quarter the progress measures disagreed (construction / EPC)

**Situation.** The contractor building Kestrel's plant reports quarterly on a **USD 48,000,000**
lump-sum contract. At the quarter-five data date it had incurred **USD 27,000,000** and forecast
**USD 15,000,000** to complete, so on its cost-input measure the job was **64.2857 %** done and it
recognised revenue of **30,857,142.86** and cumulative profit of **3,857,142.86**. The SPV, on the
same date, had certified **61 %** of the milestone schedule — output revenue of 29,280,000 and
cumulative profit of 2,280,000 on the same costs. Two systems, one job, **1,577,142.86** apart, which
is **26.2857 %** of the contract's whole expected margin.

**How the gap was read, and misread.** The contractor's commercial team treated the 3.2857-point
difference as a certification lag: the works were in place and the SPV's engineer had not yet signed.
The SPV's technical adviser read the same gap the other way — costs were running ahead of physical
progress on the marine works, where a seabed condition had required additional grouting that was not a
variation. Both explanations fit the numbers, and the systems could not adjudicate between them,
because on a fixed-price milestone certification the SPV's own earned value shows `CPI` = 1.0000 by
construction (Domain 14, KA 14.2.1) and can therefore never see a contractor cost overrun at all.
What settled it was neither system but the evidence underneath: grouting quantities against the
design, which supported the adviser.

**What followed.** The re-forecast raised the cost to complete to **USD 22,500,000**, taking expected
total costs to **49,500,000** against the 48,000,000 price. The contract was **onerous by
1,500,000**, and because 3,857,142.86 of profit had already been recognised, the charge in the quarter
was **USD 5,357,142.86** — **3.5714 times** the loss it reported. The contractor's quarterly result
moved from a modest profit to a substantial loss on a job whose physical progress had not changed at
all that quarter. Nothing in the SPV's accounts moved: the price was fixed, so the SPV's cost, asset
and `DSCR` were untouched, and its exposure was entirely to the contractor's solvency and to the
delay that the additional works implied — which is where a lender's monitoring should have been
looking, and was not.

**What the domain teaches here.** Three things, in order of how often they are got wrong. **A
divergence between an input and an output progress measure is evidence, and the two disciplines must
be made to reconcile against the same underlying quantities** — neither an accounting percentage nor a
certified percentage is capable of settling the question on its own (KA 2.3.2; PML-AI, Domain 7,
KA 7.3.1). **The onerous-contract charge is dominated by the reversal, not the loss**, so the later a
contract turns, the more violent the reported result, which is why contractor profit history is a poor
predictor and cost-to-complete forecasts are the thing to read. And **a fixed price protects the SPV's
statements and not the SPV**: the counterparty risk that a loss-making contract creates does not appear
anywhere in the SPV's accounts, so it has to be monitored through the contractor's, which is a
diligence and covenant design question (Domain 12, KA 12.1; Domain 14, KA 14.2.1) rather than an
accounting one.

---

## Executive perspective — Domain 2

What a project finance director cannot delegate in this domain:

- **The definitions.** `CFADS`, `EBITDA`, "net debt", "spend" — the director asks what the defined
  term is *in the documents* before quoting any ratio built on it (Case study A).
- **The accrual/cash bridge.** Being able to explain, unprompted, why profit and cash differ this
  period — and what is absorbing the difference.
- **The policy exposures.** Which covenants can be moved by an accounting judgment (capex/opex,
  revenue timing, provisions) and which cannot.
- **Working capital as an operational matter.** Collections and payment terms are cash decisions
  with covenant consequences, owned by named people, not finance hygiene — expressed as a
  days target (31 days, not 1.25×) so that somebody can act on it.
- **The interface discipline.** Insisting project and ledger reconcile, and never letting the five
  meanings of "spend" circulate interchangeably.
- **Knowing which covenant binds.** Not which covenant sounds strictest: the director can state, in
  revenue units, which single test fails first — for Kestrel a 0.5000 % revenue fall on interest
  cover against 3.8796 % on `DSCR`.
- **Refusing accounting returns as project verdicts.** A first-year `ROCE` below `WACC` on a project
  with a +16,179,360 `NPV` is arithmetic, not news, and a director who cannot say why will approve
  and reject the wrong things.
- **The permission to distribute, not just the cash to distribute.** Distributable reserves and
  distributable cash are different tests with different answers (2.A.4), and only one of them is in
  the waterfall.

## Calculation exercises — Domain 2

**Exercise 2.1** Revenue 15,000,000; cash operating costs 5,800,000; asset 75,000,000 depreciated
over 30 years; interest 2,100,000; tax 20 %. Build the income statement to net income.
*Solution.* `EBITDA` **9,200,000**; depreciation `75,000,000/30 =` **2,500,000**; `EBIT`
**6,700,000**; PBT **4,600,000**; tax **920,000**; net income **3,680,000**. Common error: taxing
`EBIT` rather than PBT (giving 1,340,000 and overstating tax by 420,000).

**Exercise 2.2** From Exercise 2.1, receivables rose 1,100,000, inventory rose 200,000 and
payables rose 450,000. Compute operating cash flow.
*Solution.* `3,680,000 + 2,500,000 − 1,100,000 − 200,000 + 450,000 =` **USD 5,330,000**. Common
error: signing payables negative — supplier credit is a cash source.

**Exercise 2.3** Using Exercise 2.1–2.2 and annual debt service of 4,400,000, compute `DSCR` both
before and after working-capital movements (`CFADS` = `EBITDA` − tax [− ΔWC]).
*Solution.* Before: `(9,200,000 − 920,000)/4,400,000 = 8,280,000/4,400,000 =` **1.88**. After:
ΔWC `1,100,000 + 200,000 − 450,000 = 850,000`; `7,430,000/4,400,000 =` **1.69**. Common error:
using net income instead of `EBITDA` in `CFADS` (double-counting interest, which debt service
already includes).

**Exercise 2.4** USD 2,400,000 is spent on plant modification, capitalised over 8 years versus
expensed. State the year-one profit difference and the cash difference.
*Solution.* Capitalised charge `2,400,000/8 =` **300,000**; expensed **2,400,000**; profit
difference **USD 2,100,000**; cash difference **nil** before tax. Common error: assuming a cash
difference because the profit effect is large — and the opposite error of asserting there can never
be one, since where the tax deduction follows the accounting treatment the timing of relief differs
(2.3.3).

**Exercise 2.5** Continue Exercises 2.1–2.3. The project opened the year with plant 75,000,000, cash
nil, senior debt 52,500,000 and equity 22,500,000, and no receivables, inventory or payables. Debt
service was 4,400,000 including 2,100,000 of interest. There were no distributions, contributions or
capex. Complete the closing balance sheet and prove it balances.
*Solution.* Principal `4,400,000 − 2,100,000 =` **2,300,000**; closing cash
`5,330,000 − 2,300,000 =` **3,030,000**; plant net **72,500,000**; closing debt **50,200,000**;
closing equity `22,500,000 + 3,680,000 =` **26,180,000**. Assets
`72,500,000 + 1,100,000 + 200,000 + 3,030,000 =` **76,830,000**; liabilities and equity
`450,000 + 50,200,000 + 26,180,000 =` **76,830,000**. Check by the second route: `CFADS` −
debt service `= 7,430,000 − 4,400,000 =` **3,030,000**, the same cash. Common error: deducting the
whole 4,400,000 instalment from operating cash flow, which double-counts the interest already inside
it and understates cash by 2,100,000.

**Exercise 2.6** The same project's facility carries a **1.60×** `DSCR` covenant, with `CFADS`
struck after working capital. Inventory stays at 200,000 and payables at 450,000. Revenue is
15,000,000 and receivables opened at nil. Express the covenant as a maximum days-sales-outstanding
figure at a 365-day convention, and state the headroom in days.
*Solution.* `CFADS` before working capital `9,200,000 − 920,000 =` **8,280,000**; trigger
`1.60 × 4,400,000 =` **7,040,000**; allowable absorption **1,240,000**; allowable receivables
`1,240,000 − 200,000 + 450,000 =` **1,490,000**; maximum `DSO`
`1,490,000/15,000,000 × 365 =` **36.2567 days** against an actual
`1,100,000/15,000,000 × 365 =` **26.7667 days** — headroom **9.4900 days**. Actual `DSCR` after
working capital is **1.6886**. Common error: forgetting that inventory consumes and payables release,
so the allowable *receivables* figure is not the allowable *absorption* figure.

**Exercise 2.7** A concession requires site restoration costing USD 6,000,000 in 20 years, discounted
at 6.0 %. Compute the provision recognised, the year-one accretion, the year-one depreciation of the
restoration asset, the total year-one charge and the year-one cash effect.
*Solution.* Provision `6,000,000/1.06²⁰ =` **1,870,828.36**; accretion
`1,870,828.36 × 0.06 =` **112,249.70**; depreciation `1,870,828.36/20 =` **93,541.42**; total charge
**205,791.12**; cash effect **nil**. The provision closes year one at **1,983,078.06** and the total
charged over the 20 years is **6,000,000** — the settlement amount. Common error: charging accretion
alone and forgetting that recognising the provision also created a depreciable asset.

**Exercise 2.8** From Exercises 2.1 and 2.5, compute the current ratio, debt/equity, net debt,
net debt/`EBITDA`, pre-tax `ROCE` on closing capital employed, interest cover, and then test the
leverage identity on opening balances.
*Solution.* Current ratio `4,330,000/450,000 =` **9.6222**; debt/equity
`50,200,000/26,180,000 =` **1.9175**; net debt `50,200,000 − 3,030,000 =` **47,170,000**; net
debt/`EBITDA` **5.1272**; `ROCE` `6,700,000/76,380,000 =` **8.7719 %**; interest cover
`6,700,000/2,100,000 =` **3.1905×**. Identity: after-tax `ROCE` on opening capital
`5,360,000/75,000,000 =` **7.1467 %**; after-tax cost of debt
`2,100,000/52,500,000 × 0.80 =` **3.2000 %**; `D/E` **2.3333**;
`7.1467 + 2.3333 × (7.1467 − 3.2000) =` **16.3556 %**, equal to `3,680,000/22,500,000 =` **16.3556 %**
to the digit. Common error: testing the identity with `ROCE` on closing capital and `ROE` on closing
equity, which breaks it and produces a "discrepancy" that is only a change of denominator.

**Exercise 2.9** At a construction data date a project reports committed 24,000,000, cash paid
14,600,000, retention withheld 700,000, approved unpaid invoices 260,000, certified value not yet
invoiced 540,000 and an unrecovered advance payment of 900,000. Compute open commitments, incurred
and invoiced.
*Solution.* Open commitments
`24,000,000 − 14,600,000 − 700,000 − 260,000 − 540,000 + 900,000 =` **8,800,000**; incurred
`14,600,000 + 700,000 + 260,000 + 540,000 − 900,000 =` **15,200,000**; invoiced
`15,200,000 − 540,000 =` **14,660,000**. Common error: adding the unrecovered advance rather than
deducting it — it is cash paid for value not yet received, so it reconciles with the opposite sign to
every other item.

**Exercise 2.10** An asset costs 75,000,000, is depreciated straight-line over 30 years and attracts
capital allowances at 20 % on a declining balance. The tax rate is 20 %. Compute the deferred tax
liability at the end of year two and the year-two deferred tax charge.
*Solution.* Year-one allowance **15,000,000**, tax written-down value **60,000,000**, carrying amount
**72,500,000**, difference **12,500,000**, liability **2,500,000**. Year-two allowance
`0.20 × 60,000,000 =` **12,000,000**, tax written-down value **48,000,000**, carrying amount
**70,000,000**, cumulative difference **22,000,000**, liability **4,400,000**. The year-two charge is
the movement, **1,900,000**, which equals `0.20 × (12,000,000 − 2,500,000)`. Common error: computing
the liability on the year's difference in charges rather than on the cumulative difference between
carrying amount and tax written-down value — the two coincide in year one and diverge every year
after, so the error is invisible exactly once.

**Exercise 2.11** A contractor on a 30,000,000 fixed-price contract has incurred 12,000,000 and
forecasts 13,000,000 to complete. Compute progress, revenue and cumulative profit on a cost-input
measure. Then the forecast to complete rises to 20,000,000: state the expected loss and the charge in
that period.
*Solution.* Expected total cost **25,000,000**; progress `12,000,000/25,000,000 =` **48.0000 %**;
revenue `30,000,000 × 0.48 =` **14,400,000**; profit to date **2,400,000**. Revised total cost
**32,000,000**, so the contract is onerous by **2,000,000**; the cumulative position must move from
+2,400,000 to −2,000,000, giving a period charge of **4,400,000** — **2.2000 times** the loss. Common
error: recognising the 2,000,000 loss and leaving the 2,400,000 of recognised profit in place, which
overstates the cumulative result by 2,400,000 and spreads a known loss into the future.

## Practitioner's toolkit — Domain 2

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 2.T.1 — Defined-terms sheet (one per financing)

For each term used in a covenant or report — `CFADS`, `EBITDA`, net debt, distributable cash,
project costs — record: the **document and clause** defining it, the definition in plain words,
what is included and excluded (working capital? cash tax or accrued? maintenance capex?), the
model line implementing it, and the person who confirmed the match. A ratio quoted without a row
here is not reportable (Case study A).

### Toolkit 2.T.2 — Accrual-to-cash bridge (standing monthly schedule)

Net income · + depreciation and amortisation · + other non-cash charges (provisions) ·
− Δreceivables · − Δinventory · + Δpayables · = operating cash flow · − capex · − principal ·
− distributions · = movement in cash, **tied to the cash balance**. Rule: it is published even in
good months, because its purpose is to make divergence visible before it is material.

### Toolkit 2.T.3 — Statement-integrity checklist

- [ ] Balance sheet balances; closing cash ties to the cash-flow statement.
- [ ] Closing equity = opening + profit − distributions + contributions.
- [ ] Depreciation in the cash-flow statement equals the income-statement charge.
- [ ] Debt movement = drawings − principal repaid; only interest is expensed.
- [ ] Working-capital movements reconcile to balance-sheet deltas.
- [ ] Every quoted ratio recomputable from the face of the statements, on its defined terms.
- [ ] Capitalisation boundary and any policy change disclosed and explained.
- [ ] Return ratios state their basis (opening, average or closing) — the leverage identity holds on
      one basis only, and mixing them moved Kestrel's `ROE` by 1.1796 points.
- [ ] Interest paid identified as operating or financing, and the `CFADS` identity applied only to
      the operating presentation.
- [ ] Cash split into unrestricted and reserve balances; 91.1264 % of Kestrel's was restricted.
- [ ] Each covenant restated in revenue units, so the binding test is known rather than assumed.
- [ ] Distributable reserves stated beside distributable cash in any distribution paper.

## Exam preparation — Domain 2

**The traps.** Expensing the whole debt instalment instead of interest only (MCQ 2.2-C) · taxing
`EBIT` rather than PBT (Exercise 2.1) · signing payables the wrong way in the cash bridge
(Exercise 2.2) · using net income in `CFADS` and double-counting interest (Exercise 2.3) ·
assuming a cash difference from a capex/opex choice, or asserting there can never be one
(Exercise 2.4) · deducting the whole instalment from operating cash flow when deriving closing cash
(Exercise 2.5) · confusing the distribution threshold's `DSO` limit with the covenant's
(MCQ 2.3-E) · quoting a `DSCR` without its `CFADS` definition (2.3.1) · reading an interest
classification as an economic difference in operating cash flow (MCQ 2.2-F) · recognising a
provision on estimability alone without probability (2.3.4) · charging a provision's accretion and
forgetting the asset it created (Exercise 2.7) · recognising an onerous loss without reversing the
profit already taken (MCQ 2.3-G, Exercise 2.11) · mixing opening, average and closing bases in a
return ratio, which breaks the leverage identity (Exercise 2.8) · computing a deferred tax liability
on the year's difference in charges rather than the cumulative temporary difference (Exercise 2.10) ·
adding rather than deducting an unrecovered advance in a spend reconciliation (Exercise 2.9) ·
assuming capitalised cost cannot exceed cost incurred (MCQ 2.4-F) · treating accounting tax as cash
tax (2.A.1) · comparing leverage across a period spanning a policy change, or restating only the
numerator (2.A.2) · treating distributable cash as permission to distribute (2.A.4).

**Reflection questions.**
1. For your current financing: what exactly does `CFADS` include, and where is that written down?
2. Which of your covenants could be moved by an accounting judgment rather than a change in
   economics — and who reviews those judgments?
3. When someone last told you what a project had "spent", which of the five measures was it — and
   did you ask?
4. Restate your tightest covenant in a unit somebody owns — days of collection, points of
   availability, a unit cost. Who is that person, and do they know?
5. If your project were to report an accounting loss in a year of unchanged cash flow, could it
   still lawfully distribute — and who has confirmed that?

## Domain 2 summary

Accrual accounting records effects when they occur and cash accounting records money when it
moves; both matter, because covenants are written on each and they diverge in ways that are
information rather than noise. Recognition and measurement are governed by tests, and the policies
chosen shape reported figures without touching economics — which is why statements are read
sceptically and why the three of them, locked together by articulation, are stronger evidence than
any one alone. Kestrel's first year demonstrates the machinery end to end: `EBITDA` 7,500,000
descending through depreciation, interest and tax to net income 2,064,000, then bridged back to
operating cash flow of 3,864,000 by adding non-cash depreciation and deducting the 600,000 that
working capital absorbed — the same 600,000 that separates the year's accrual and cash results and
sits, findable, as the closing net working-capital balance. The same bridge built by the direct
method gives the identical 3,864,000 out of gross flows that are far more useful: 11,100,000
collected, **92.50 %** of revenue recognised. From there the statements close themselves. Deducting
the 2,489,635.23 of principal — a balance-sheet movement, not an expense — leaves cash of
**1,374,364.77**, which is also `CFADS` less debt service, which is also Domain 9's annual
distributable cash, which Domain 6 then splits exactly into a 1,252,408.81 reserve instalment and a
121,955.96 distribution; and the balance sheet totals **59,874,364.77** on both sides with nothing
plugged. Two breakevens sit inside the same income statement and they settle the book's opening
claim: profit reaches nil at a 21.50 % revenue fall and coverage reaches 1.00 at 14.32 %, so **the
cash constraint binds first, by seven revenue points**.

The project-relevant treatments each carry a professional edge. Working capital moved Kestrel's
`DSCR` from 1.39 to 1.27 on the documented `CFADS` definition, within USD 100,000 of a covenant —
and restated in the unit that caused it, the whole dividend rested on **3.7095 days** of collection,
against a covenant reached at 38.7033 days. The capex/opex choice moved a year's profit by 1,080,000
on 1,200,000 of spend while pre-tax cash was unchanged, though where the deduction follows the
accounting the after-tax difference is a real **78,958.05** of present value. Revenue recognition
follows performance, and an input measure running 3.2857 points ahead of certification was worth
1,577,142.86 of profit before a re-forecast turned it into a 5,357,142.86 charge — **3.5714 times**
the 1,500,000 loss it reported, because the profit already taken must be reversed. Provisions are
measured, not chosen: a 4,500,000 restoration obligation is recognised at **1,328,862.47**, charges
**119,597.62** in its first year with no cash effect, moves 27.0279 % on a 100-basis-point change of
rate, and accretes to exactly the amount eventually paid.

Ratios are comparisons whose value lies in what they are compared against. Read as a set they yield
the leverage identity — `6.8000 % + 2.3333 × (6.8000 % − 4.8000 %) = 11.4667 %`, exact on opening
bases and broken by mixing them — a first-year after-tax `ROCE` **1.1860 points below** Domain 9's
`WACC` on a project with a +16,179,360 `NPV`, and the discovery that a 2.00× interest-cover covenant
would bind **7.7591 times** sooner than the 1.20× `DSCR` covenant beside it. And the five meanings of
"spend" differ by **20,451,403** — 34.0857 % of the envelope — on one date, reconciling to a single
identity in which the unrecovered advance carries the opposite sign and the capitalised figure
legitimately exceeds the incurred one. Domain 3 supplies the discounting these statements are valued
with; Domain 6 turns them into a model and adds the reserve account that spends the closing cash
balance; Domain 10 turns `CFADS` into the covenants a lender actually enforces; and Domain 15 watches
the working-capital line move against the project in both directions.
