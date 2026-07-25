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
recognition tests that drive it; describe each of the three statements and what question it
answers; **articulate** a statement set — derive the cash-flow statement from profit and the
balance-sheet movements, and prove it reconciles; explain how working capital consumes cash and
compute its effect on `CFADS` and therefore on a coverage ratio; distinguish capital from
operating expenditure and quantify the profit effect of the choice while showing cash is
unaffected; explain provisions and contingent liabilities in principle; interpret the core ratios
including interest cover; describe the interfaces between project cost systems and corporate
reporting; and govern AI-assisted analysis of financial statements.

**The master statements.** Kestrel Water SPC — whose loan, appraisal and financing decision
Domains 1, 3 and 4 built — now reports its **first full operating year**. The plant cost
**USD 60,000,000** (Domain 4's `I₀`), depreciated straight-line over **25 years**. The senior loan
is Domain 3's **USD 42,000,000 at 6.0 % over 12 years**, so year-one interest is
**USD 2,520,000** and the annual instalment is **USD 5,009,635**. Revenue is
**USD 12,000,000**, cash operating costs **USD 4,500,000**, and tax is charged at **20 %**. Every
figure in KA 2.2–2.4 derives from these.

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

### Self-check — KA 2.1

1. *Which basis governs debt service, and which governs covenants defined on profit?* — Cash for
   debt service; accrual for profit-defined covenants. A leader needs both.
2. *Why is an intention never a liability?* — Recognition requires a present obligation arising
   from a past event.
3. *What does a failure to articulate tell you?* — That there is an error or omission, and where
   the break occurs localises it.

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
   in project financial conversation.

### 2.2.2 The balance sheet

**What it answers:** what does the entity own and owe at a point in time? For an SPV the structure
is unusually clean — one asset of consequence and one financing structure:

| | USD |
|---|---|
| Plant, net (60,000,000 − 2,400,000 depreciation) | 57,600,000 |
| Receivables | 900,000 |
| Cash | *balancing* |
| **Total assets** | |
| Payables | 300,000 |
| Senior debt (42,000,000 − 2,489,635 year-one principal) | 39,510,365 |
| Equity (contributed + retained) | *balancing* |

Two project-specific points. **The asset is consumed on paper while it produces cash** —
depreciation reduces the carrying amount without any payment. And **the debt balance falls by
principal only**: of Kestrel's USD 5,009,635 instalment, USD 2,520,000 is interest (an expense)
and **USD 2,489,635 is principal** (a balance-sheet movement, not an expense) — exactly Domain 3's
schedule. That split is why debt service never appears as a single line in the income statement,
and why a reader who looks only at profit cannot see whether the loan is being repaid.

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

> **Fig 2.2.1 — Kestrel's accrual-to-cash bridge.** Waterfall chart, y-axis USD 0–5m. Bars left to
> right: Net income 2,064,000 (start) · +Depreciation 2,400,000 (rising, brand blue) ·
> −Receivables increase 900,000 (falling, crimson) · +Payables increase 300,000 (rising, blue) ·
> **Operating cash flow 3,864,000** (total bar, ink). Each bar labelled with its value; a bracket
> above the middle three annotated "the accrual adjustments — none of them cash decisions of this
> period". Source: PCI original. Alt text: waterfall from net income of just over two million,
> lifted by depreciation and payables and reduced by receivables, to operating cash flow of
> USD 3.86 million.

### Key terms — KA 2.2

| Term | Meaning |
|---|---|
| **`EBITDA`** | Earnings before interest, tax, depreciation and amortisation. |
| **`EBIT`** | Operating profit after depreciation; performance including capital consumption. |
| **Depreciation** | (cost − residual)/useful life; a non-cash charge for asset consumption. |
| **Indirect method** | Deriving operating cash flow from profit by undoing accruals. |
| **Working-capital movement** | Change in receivables, inventory and payables; absorbs or releases cash. |
| **Principal versus interest** | Principal is a balance-sheet movement; only interest is an expense. |

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

### Self-check — KA 2.2

1. *Why is `EBITDA` USD 7,500,000 while net income is USD 2,064,000?* — Depreciation, interest and
   tax lie between them; on a capital-intensive levered project that gap is structural.
2. *State the accrual-to-cash bridge for Kestrel.* — 2,064,000 + 2,400,000 depreciation
   − 900,000 receivables + 300,000 payables = 3,864,000.
3. *What obligations does operating cash flow not cover?* — Debt principal and capex; hence
   Domain 10's coverage ratios.

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
5. **Interpretation.** Twelve basis points of coverage may decide whether a covenant holds
   (typical senior covenants sit at 1.20–1.30, so this project is comfortable at 1.39 and much
   closer to the line at 1.27). The lesson is definitional rather than arithmetical: **`CFADS` is
   a defined term in the finance documents, and whether it is struck before or after
   working-capital movements changes the ratio it produces.** A leader who quotes a `DSCR` without
   knowing its `CFADS` definition is quoting an opinion. Domain 10 builds the full machinery;
   Domain 13's model audit checks that the model implements the *documented* definition.

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
| **Input / output progress measures** | Cost-based vs delivery-based measures of performance to date. |
| **Onerous contract** | An expected-loss contract; the loss is recognised immediately and in full. |
| **Capex / opex** | Capitalised and depreciated vs expensed in the period; same cash, different profit. |
| **Provision / contingent liability** | Recognised (present obligation, probable, estimable) vs disclosed. |

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

### Self-check — KA 2.3

1. *Why can two projects with identical economics report different `DSCR`s?* — Because `CFADS` is
   a defined term; its definition (e.g. before or after working capital) changes the ratio.
2. *What does the capex/opex choice change, and what does it not?* — Reported profit and the asset
   base; not cash.
3. *When is a provision recognised rather than disclosed?* — Present obligation from a past event,
   probable settlement, reliable estimate.

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
case is whether its cash flow services its debt (Domain 1, KA 1.1.2).

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
material amounts.

### Key terms — KA 2.4

| Term | Meaning |
|---|---|
| **Interest cover** | `EBIT` ÷ interest; an accrual coverage measure that ignores principal. |
| **Debt/`EBITDA`** | A leverage measure; tolerable level depends on revenue certainty. |
| **Capitalisation boundary** | Which costs and which dates form part of the asset. |
| **Cut-off** | Assigning a transaction to exactly one period. |
| **Commitment vs actual vs paid** | Distinct measures of "spend" that must never be conflated. |

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

### Self-check — KA 2.4

1. *Why does project finance emphasise coverage over return ratios?* — A ring-fenced SPV's credit
   case is whether its cash services its debt.
2. *What does interest cover ignore?* — Principal repayment; hence `DSCR`.
3. *Name the five meanings of "spend".* — Committed, incurred, invoiced, paid, capitalised.

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

### 2.A.2 Leases and off-balance-sheet intuitions

Under current thinking a lessee generally recognises a right-of-use asset and a lease liability,
so the older intuition that operating leases keep obligations off the balance sheet no longer
holds (IFRS 16 is the reference framework, in principle). Two consequences for a finance leader:
leverage ratios computed across periods spanning the change are not comparable, and covenant
definitions written before it may capture or exclude lease liabilities in ways nobody intended —
a live reason to read the *definitions* in finance documents rather than assume them.

### 2.A.3 The reviewer's statement eye

Invariants to test on any statement set before relying on it: the balance sheet balances; closing
cash on the cash-flow statement equals balance-sheet cash; closing equity equals opening plus
profit less distributions plus contributions; depreciation in the cash-flow statement equals the
income-statement charge; the movement in debt equals drawings less principal repaid; principal
appears in financing and interest in operating (or as disclosed); working-capital movements
reconcile to balance-sheet deltas; and any ratio quoted can be recomputed from the face of the
statements. A set that fails any of these has an error, an omission, or a policy that needs
explaining — and the failure point localises it.

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
**1.25** covenant.

**The problem.** The facility's definition of `CFADS` is struck **after** movements in working
capital. Kestrel's receivables had grown USD 900,000 as the offtaker's payment process settled
into a slower rhythm than modelled, against a USD 300,000 rise in payables — a net USD 600,000
absorbed. On the documented definition, `CFADS` is USD 6,384,000 and `DSCR` is **1.27**, not 1.39.
Headroom against the 1.25 covenant is USD 0.02 of ratio — roughly **USD 100,000 of cash** — not
the comfortable margin reported.

**What was done.** The report was corrected before issue. More consequentially, the near-miss
changed three things: collections became a monitored operational metric with a named owner (not a
finance afterthought); the model's working-capital assumptions were re-based on nine months of
actual collection behaviour rather than the financial-close assumption; and the treasury team
sized a working-capital facility as a liquidity buffer. The following year's `DSCR` came in at
1.41 on the documented definition.

**What the domain teaches here.** The arithmetic here is trivial; the professional content is
definitional. A ratio is only as good as the defined term inside it, and the definition lives in
the finance documents, not in convention. It also shows the accrual/cash divergence of KA 2.1.1
doing real damage — a genuinely profitable year came within USD 100,000 of a covenant breach
because of a balance-sheet movement no one was watching.

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
  with covenant consequences, owned by named people, not finance hygiene.
- **The interface discipline.** Insisting project and ledger reconcile, and never letting the five
  meanings of "spend" circulate interchangeably.

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
difference **USD 2,100,000**; cash difference **nil**. Common error: assuming a cash difference
because the profit effect is large.

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

## Exam preparation — Domain 2

**The traps.** Expensing the whole debt instalment instead of interest only (MCQ 2.2-C) · taxing
`EBIT` rather than PBT (Exercise 2.1) · signing payables the wrong way in the cash bridge
(Exercise 2.2) · using net income in `CFADS` and double-counting interest (Exercise 2.3) ·
assuming a cash difference from a capex/opex choice (Exercise 2.4) · quoting a `DSCR` without its
`CFADS` definition (2.3.1) · recognising a provision on estimability alone without probability
(2.3.4) · treating accounting tax as cash tax (2.A.1) · comparing leverage across a period spanning
a policy change (2.A.2).

**Reflection questions.**
1. For your current financing: what exactly does `CFADS` include, and where is that written down?
2. Which of your covenants could be moved by an accounting judgment rather than a change in
   economics — and who reviews those judgments?
3. When someone last told you what a project had "spent", which of the five measures was it — and
   did you ask?

## Domain 2 summary

Accrual accounting records effects when they occur and cash accounting records money when it
moves; both matter, because covenants are written on each and they diverge in ways that are
information rather than noise. Recognition and measurement are governed by tests, and the policies
chosen shape reported figures without touching economics — which is why statements are read
sceptically and why the three of them, locked together by articulation, are stronger evidence than
any one alone. Kestrel's first year demonstrates the machinery end to end: `EBITDA` 7,500,000
descending through depreciation, interest and tax to net income 2,064,000, then bridged back to
operating cash flow of 3,864,000 by adding non-cash depreciation and deducting the 600,000 that
working capital absorbed — a bridge that also explains why only interest is an expense while the
2,489,635 of principal is a balance-sheet movement. The project-relevant treatments each carry a
professional edge: working capital moved Kestrel's `DSCR` from 1.39 to 1.27 on the documented
`CFADS` definition, within USD 100,000 of a covenant; the capex/opex choice moved a year's profit
by 1,080,000 on 1,200,000 of spend while cash was unchanged; revenue recognition follows
performance and must agree with the progress evidence delivery uses; and provisions are recognised
on tests, never created as reserves. Ratios are comparisons whose value lies in what they are
compared against, coverage dominates in project finance, and the five meanings of "spend" must
never circulate interchangeably. Domain 3 supplies the discounting these statements are valued
with; Domain 6 turns them into a model; Domain 10 turns `CFADS` into the covenants a lender
actually enforces.
