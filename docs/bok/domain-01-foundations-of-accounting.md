# Domain 1 — Foundations of Accounting for Project Controls

> **Group:** Finance, accounting & reporting (Domain 1 of 4). **Target:** ~110 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md) — restate any symbol on use; British English; USD
> (+SAR where useful); worked examples in the five-line format (Spine §5).

## Why this domain exists

A project controls professional does not keep the books, but reads, reconciles and reports against them
every day. Cost extracted from an ERP, a variance explained to a project board, a forecast defended in an
assurance review, an accrual raised at month-end, a contract loss provided for — every one of these sits on
the accounting model. A controls professional who treats the ledger as a black box will misread what the
numbers mean, reconcile the wrong figures, and forecast on foundations they cannot see. This domain builds
that foundation from first principles: the accounting equation and double-entry mechanics (KA 1.1); the
financial statements they produce and how those statements articulate (KA 1.2); the accrual and matching
concepts that decide *when* cost and revenue land (KA 1.3); the provisions and accruals that recognise
obligations before they are paid (KA 1.4); and the chart of accounts and cost coding that connect every
posting to the work breakdown structure a controls professional actually manages (KA 1.5).

**Learning objectives.** After this domain a candidate can: apply the accounting equation and the debit/
credit rules to record a full transaction cycle; prepare and read the primary financial statements and
explain how they articulate; distinguish cash from accrual accounting and apply the matching concept through
period-end adjustments; recognise and measure provisions and accruals under IAS 37; and design and apply a
project chart of accounts and cost coding structure that maps to the WBS and the control accounts used in
earned value.

**A note on standards.** The mechanics in KAs 1.1–1.3 are the common grammar of financial accounting, not the
property of any one framework. Where a recognition or measurement rule *is* set by a standard — provisions
(IAS 37), presentation (IAS 1), inventories (IAS 2) — the standard is named and its principle described in
this reference's own words. No standard's wording is reproduced.

---

## Knowledge Area 1.1 — The accounting model

*Topics: 1.1.1 the accounting equation · 1.1.2 debit & credit rules by account type · 1.1.3 double-entry
mechanics · 1.1.4 the ledger and the trial balance · 1.1.5 a full transaction cycle worked end-to-end.*

### 1.1.1 The accounting equation

**Definition & purpose.** Every entity's finances obey one identity: the resources it controls equal the
claims against those resources. Resources are **assets**; claims are either those of outside parties
(**liabilities**) or those of the owners (**equity**). Hence the **accounting equation**:

```
Assets = Liabilities + Equity          →     A = L + E
```

The equation is not a rule an accountant chooses to follow; it is an identity that *must* hold after every
transaction, because every transaction is an exchange that is recorded from both sides. It is the reason the
system is called **double-entry**, and the reason a **balance sheet balances**.

**The expanded equation.** Equity is not static: it grows when the business earns income and shrinks when it
incurs expenses or distributes profit to owners. Expanding equity into its drivers gives the form that
connects the balance sheet to performance:

```
A = L + Contributed capital + Retained earnings
       where  Retained earnings = Opening retained earnings + Income − Expenses − Distributions
```

- **Income (`Rev`)** increases equity (the business is better off).
- **Expenses (`Exp`)** decrease equity (resources consumed to earn income).
- **Contributed capital** increases equity (owners put resources in).
- **Distributions** (dividends/drawings) decrease equity (resources returned to owners).

**Underlying principle — the *entity concept*.** The equation is written from the perspective of the
*business as a separate person*, distinct from its owners. Capital the owner injects is, to the business, a
liability-like claim ("we owe this to the owners") sitting in equity — which is why an owner's cash injection
*increases* equity rather than being income. Grasping the entity concept is what makes the sign of every
entry intuitive rather than memorised.

**Worked example 1.1.1 — the equation absorbs a transaction.**

1. **Setup.** *Meridian Project Controls* (a fictional consultancy used as the threaded example throughout
   this domain) starts with `A = 0, L = 0, E = 0`. The owners inject **USD 100,000** cash (≈ SAR 375,000 at
   an indicative USD 1 ≈ SAR 3.75).
2. **Formula.** `A = L + E` must hold after the event.
3. **Substitution.** Cash (an asset) rises by 100,000; contributed capital (equity) rises by 100,000:
   `100,000 = 0 + 100,000`.
4. **Result.** The equation holds: **`100,000 = 100,000`**.
5. **Interpretation.** The business now controls USD 100,000 of resources, all of it claimed by the owners.
   No income has arisen — an owner's contribution is a *financing* event, not performance.

> **Fig 1.1.1 — The accounting equation as a balance.** *Caption:* Assets on the left pan; liabilities and
> equity stacked on the right pan; the beam is level. *Underlying data:* left = Assets 100,000; right =
> Liabilities 0 + Equity 100,000. *Render-ready description:* a two-pan balance scale, left pan labelled
> "Assets = 100,000" in brand blue `#1D4ED8`, right pan a stacked bar "Liabilities 0" (grey) + "Equity
> 100,000" (blue), beam horizontal to signal equality; caption strip beneath in Plus Jakarta Sans.

### 1.1.2 Debit and credit rules by account type

**Definition & purpose.** *Debit* (Dr, left) and *credit* (Cr, right) are simply the two sides of every
account. They are **not** synonyms for "increase" and "decrease", nor for "good" and "bad" — a persistent
misconception. Whether a debit increases or decreases an account depends on the account's **type**, fixed by
where that type sits in the accounting equation.

**The rule, derived not memorised.** Split the equation across the "=" sign. Accounts on the **asset** side
(left of the equation) *increase* on the **debit** (left) side. Accounts on the **liability and equity** side
(right of the equation) *increase* on the **credit** (right) side. Income increases equity, so income
behaves like equity (increases on credit); expenses decrease equity, so they are the mirror (increase on
debit).

| Account type | Increases on | Decreases on | Normal balance |
|---|---|---|---|
| **Asset** | Debit | Credit | Debit |
| **Liability** | Credit | Debit | Credit |
| **Equity** (capital, retained earnings) | Credit | Debit | Credit |
| **Income / Revenue** | Credit | Debit | Credit |
| **Expense** | Debit | Credit | Debit |
| **Contra-asset** (e.g. accumulated depreciation) | Credit | Debit | Credit |

**Underlying principle — the *normal balance*.** An account's normal balance is the side on which it
increases; it is where you expect that account to sit. Cash (an asset) normally carries a **debit** balance;
a bank loan (a liability) normally carries a **credit** balance. When an account carries the *opposite* of
its normal balance, that is a signal worth investigating — a "credit balance in accounts receivable", for
instance, usually means a customer overpaid or was over-refunded, exactly the kind of anomaly a controls
professional reconciling billing should chase (cross-ref 1.5.2).

**Common pitfall.** Reading a bank *statement* and a *ledger* as if "debit" meant the same on both. A bank
statement is written from the **bank's** point of view: your deposit is *the bank's* liability to you, so the
bank *credits* your account when your cash goes up. In *your own* ledger the same deposit is a **debit** to
Cash. The two are mirror images; reconciling them is a core month-end control (cross-ref 1.4 and Domain 11).

### 1.1.3 Double-entry mechanics

**Definition & purpose.** **Double-entry** means every transaction is recorded with total debits equal to
total credits, touching at least two accounts. Because debits always equal credits, the accounting equation
is preserved automatically after every posting — the system is self-checking.

**The four transaction effects.** Every economic event resolves into a combination of these, always leaving
`A = L + E` intact:

1. An asset up, another asset down (e.g. buy equipment for cash) — total assets unchanged.
2. An asset up, a liability up (e.g. buy on credit) — both sides rise.
3. An asset up, equity up (e.g. earn revenue for cash, or owners inject capital).
4. A liability down, an asset down (e.g. pay a supplier) — both sides fall.

**Formula.** For any journal entry, `Σ Debits = Σ Credits`. This is the invariant a trial balance later
tests (1.1.4).

**Worked example 1.1.3 — a purchase on credit.**

1. **Setup.** Meridian buys office supplies for **USD 6,000 on 30-day credit** (no cash yet moves).
2. **Formula.** `Σ Dr = Σ Cr`; supplies (asset) increases, accounts payable (liability) increases.
3. **Substitution.**
   ```
   Dr  Office supplies (asset)        6,000
       Cr  Accounts payable (liability)     6,000
   ```
4. **Result.** Debits 6,000 = Credits 6,000; assets +6,000 and liabilities +6,000, so `A = L + E` holds.
5. **Interpretation.** The business now controls 6,000 more resources and owes 6,000 more — no effect on
   equity, because nothing has been earned or consumed yet. The supplies become an expense only when *used*
   (see the matching concept, 1.3.2).

### 1.1.4 The ledger and the trial balance

**Definition & purpose.** Journal entries are posted to the **general ledger** — the set of accounts, one
per line item, each often drawn as a **T-account** (debits on the left, credits on the right). Periodically,
the balance of every account is listed in a **trial balance**: all debit balances in one column, all credit
balances in another. Because every entry balanced, the two columns must be **equal**.

**What the trial balance does and does not prove.** A balanced trial balance proves the ledger is
*arithmetically* consistent (debits = credits). It does **not** prove the accounts are *correct*: an entry
posted to the wrong account, an entry omitted entirely, or a transaction recorded twice can all leave the
trial balance in balance while the numbers are wrong. This limitation is exactly why *reconciliation* to
independent sources (bank statements, the cost ledger, supplier statements) matters — a theme picked up in
1.5.2 and Domain 11.

**Worked example 1.1.4 — a bank reconciliation.**

1. **Setup.** Meridian's ledger shows cash of **USD 129,000** (per 1.1.5). The bank statement shows
   **USD 131,500**. Investigation finds **outstanding cheques of USD 4,500** (written, not yet presented)
   and a **deposit in transit of USD 2,000** (banked, not yet credited by the bank).
2. **Formula.** `Adjusted bank balance = statement balance − outstanding cheques + deposits in transit`; it
   must equal the ledger balance.
3. **Substitution.** `131,500 − 4,500 + 2,000 = 129,000`.
4. **Result.** The adjusted bank balance **USD 129,000** agrees with the ledger — reconciled; the
   differences are purely timing.
5. **Interpretation.** Reconciliation to an *independent* source is the control that covers the trial
   balance's blind spots (1.1.4) — a wholly omitted or duplicated cash entry would surface here as an
   unexplained difference, not a timing item. The same tie-to-independent-source discipline runs through
   cost reconciliation (1.5.2, Domain 5).

### 1.1.5 A full transaction cycle, worked end-to-end

This flagship worked example threads through the whole domain: it produces the trial balance used to build
statements in KA 1.2 and the adjustments in KA 1.3. Meridian's **first month** of trading:

| # | Transaction | Debit | Credit |
|---|---|---|---|
| 1 | Owners inject capital, USD 100,000 cash | Cash 100,000 | Share capital 100,000 |
| 2 | Buy equipment for cash, USD 24,000 | Equipment 24,000 | Cash 24,000 |
| 3 | Draw down a bank loan, USD 50,000 | Cash 50,000 | Loan payable 50,000 |
| 4 | Buy office supplies on credit, USD 6,000 | Office supplies 6,000 | Accounts payable 6,000 |
| 5 | Invoice a client for services, USD 40,000 (on credit) | Accounts receivable 40,000 | Service revenue 40,000 |
| 6 | Pay salaries for the month, USD 18,000 cash | Salaries expense 18,000 | Cash 18,000 |
| 7 | Receive USD 30,000 from the client on account | Cash 30,000 | Accounts receivable 30,000 |
| 8 | Pay supplier USD 4,000 against the payable | Accounts payable 4,000 | Cash 4,000 |
| 9 | Pay office rent, USD 5,000 cash | Rent expense 5,000 | Cash 5,000 |

**Cash T-account (worked).** `+100,000 − 24,000 + 50,000 − 18,000 + 30,000 − 4,000 − 5,000 = 129,000` debit.

**Trial balance at month-end (before period-end adjustments):**

| Account | Debit | Credit |
|---|---:|---:|
| Cash | 129,000 | |
| Accounts receivable | 10,000 | |
| Office supplies | 6,000 | |
| Equipment | 24,000 | |
| Accounts payable | | 2,000 |
| Loan payable | | 50,000 |
| Share capital | | 100,000 |
| Service revenue | | 40,000 |
| Salaries expense | 18,000 | |
| Rent expense | 5,000 | |
| **Totals** | **192,000** | **192,000** |

The columns agree at **192,000**, so the ledger is arithmetically consistent. Note the accounts receivable
balance of 10,000 (invoiced 40,000 less 30,000 received) and accounts payable of 2,000 (6,000 owed less
4,000 paid) — the kind of open balances a controls professional reconciles against billing and procurement
records.

> **Fig 1.1.2 — From transaction to trial balance.** *Caption:* the recording cycle. *Underlying data:* the
> nine transactions above. *Render-ready description:* a left-to-right process map — **Source document →
> Journal entry (Dr/Cr) → Ledger (T-accounts) → Trial balance → Financial statements** — five rounded nodes
> in brand blue with arrows; a callout on the "Trial balance" node reads "Debits 192,000 = Credits 192,000".
> *Animation storyboard (digital-only):* each transaction row flies in turn into its two T-accounts; the
> Cash T-account running balance updates on screen; on the final step the trial-balance columns tally and
> flash green when equal.

**AI in this KA.** General-purpose LLM assistants and ledger-automation tools can *propose* the double-entry
for a described transaction, draft narrations, and flag entries whose debits and credits do not balance or
whose account looks atypical (an expense coded to a balance-sheet account). They are genuinely useful for
speed and first-pass anomaly spotting. Their limits are real: an LLM can produce a *plausible* but wrong
entry (e.g. capitalising a cost that should be expensed), and it cannot know your entity's policies unless
told. **AI proposes, the professional disposes** — the entry is not correct because the model produced it;
it is correct because a professional has checked it against the equation, the policy and the evidence.

### Key terms — KA 1.1

| Term | Meaning |
|---|---|
| **Accounting equation** | `Assets = Liabilities + Equity`; holds after every transaction. |
| **Debit / Credit** | Left/right sides of an account; effect depends on account type. |
| **Normal balance** | The side on which an account type increases. |
| **Double-entry** | Recording every transaction with `Σ Dr = Σ Cr`. |
| **Ledger / T-account** | The set of accounts; each account drawn with debits left, credits right. |
| **Trial balance** | A list of all account balances proving `Σ Dr = Σ Cr` (not correctness). |
| **Entity concept** | The business is accounted for as separate from its owners. |

### Sample MCQs — KA 1.1

**MCQ 1.1-A `[1.1.2 · Recall]`** Which statement about debits is correct?
- A. A debit always increases an account.
- B. A debit increases assets and expenses, and decreases liabilities, equity and income. ✅
- C. A debit always means a decrease.
- D. Debits and credits are interchangeable labels for increases.

*Rationale:* Whether a debit increases or decreases depends on account type. Debits increase assets and
expenses (their normal balance) and decrease liabilities, equity and income. A and C over-generalise; D is
false — the two sides are not interchangeable.

**MCQ 1.1-B `[1.1.3 · Application]`** A firm buys equipment for USD 24,000, paying cash. The correct entry is:
- A. Dr Equipment 24,000; Cr Share capital 24,000
- B. Dr Cash 24,000; Cr Equipment 24,000
- C. Dr Equipment 24,000; Cr Cash 24,000 ✅
- D. Dr Equipment 24,000; Cr Accounts payable 24,000

*Rationale:* One asset (equipment) rises and another (cash) falls — effect type 1. C records that. B reverses
the sign; A and D misstate the funding source (no capital injected, no payable created since cash was paid).

**MCQ 1.1-C `[1.1.4 · Analysis]`** A trial balance balances. Which error would it still fail to detect?
- A. A debit of 500 posted as 5,000 with no matching credit change.
- B. A sales invoice omitted from the ledger entirely. ✅
- C. Total debits of 192,000 against total credits of 191,500.
- D. A credit balance recorded in the debit column.

*Rationale:* An entirely omitted transaction leaves both columns understated equally, so the trial balance
still balances — it proves arithmetic, not completeness. A, C and D each create a debit/credit inequality the
trial balance *would* reveal.

**MCQ 1.1-D `[1.1.5 · Application]`** Using Meridian's month, what is the cash balance after transactions 1–9?
- A. USD 108,000
- B. USD 129,000 ✅
- C. USD 134,000
- D. USD 159,000

*Rationale:* `100,000 − 24,000 + 50,000 − 18,000 + 30,000 − 4,000 − 5,000 = 129,000`. A stops before the
client receipt and later payments; C omits the rent payment; D ignores the equipment purchase.

### Self-check — KA 1.1

1. State the accounting equation and explain why an owner's cash injection increases equity rather than
   income. *(Answer: `A = L + E`; under the entity concept the injection is a claim by owners on the
   business — financing, not earned performance.)*
2. On which side does a liability increase, and what is its normal balance? *(Answer: credit; normal balance
   credit.)*
3. Give one error a balanced trial balance cannot detect. *(Answer: a wholly omitted transaction; a
   transaction posted to the wrong account of the same type; a duplicated entry.)*

---

## Knowledge Area 1.2 — Components of the financial statements

*Topics: 1.2.1 the statement of financial position · 1.2.2 the statement of profit or loss & OCI · 1.2.3 the
statement of cash flows · 1.2.4 the statement of changes in equity · 1.2.5 the notes · 1.2.6 how the
statements articulate.*

### 1.2.1 The statement of financial position (SOFP)

**Definition & purpose.** The **statement of financial position** (historically the *balance sheet*) is the
accounting equation made into a report at a **point in time**: it lists the entity's assets, liabilities and
equity as at the reporting date. Under **IAS 1 (presentation of financial statements)** assets and
liabilities are normally split into **current** (expected to be realised/settled within twelve months or the
operating cycle) and **non-current**. That current/non-current split is what a controls professional reads
for **liquidity** (can the entity pay what falls due soon?) and is the anchor for working-capital analysis in
the business cycles (Domain 11).

### 1.2.2 The statement of profit or loss and other comprehensive income (SOPL & OCI)

**Definition & purpose.** The **statement of profit or loss** reports **performance over a period**: income
earned less expenses incurred, giving **profit or loss**. **Other comprehensive income (OCI)** captures gains
and losses that standards route around profit (e.g. certain revaluations); together they give **total
comprehensive income**. For most project-controls work the profit-or-loss section is what matters: revenue,
cost of sales, gross profit, operating expenses, operating profit, finance costs, and profit before/after
tax. The distinction between **profit** and **cash** — a period's profit is *not* its cash generated — is one
of the most consequential ideas in this domain and is developed in 1.2.6 and KA 1.3.

### 1.2.3 The statement of cash flows

**Definition & purpose.** The **statement of cash flows** explains the change in cash over the period,
classified into three activities:

- **Operating** — cash from the trading operations (receipts from customers, payments to suppliers and
  staff).
- **Investing** — cash for acquiring/disposing of long-term assets (buying equipment).
- **Financing** — cash from/to providers of capital (share issues, loans drawn or repaid, dividends).

It can be presented by the **direct method** (actual cash flows listed) or the **indirect method** (profit
adjusted for non-cash items and working-capital movements). Both reach the same net change in cash. For a
controls professional this statement is the bridge between the *accrual* story of the SOPL and the *cash*
reality that funds the project — the same bridge cash-flow forecasting builds forward (cross-ref Domain 3,
KA 3.5).

**Worked example 1.2.3 — the indirect method, line by line.**

1. **Setup.** Derive net operating cash by the **indirect method** from Meridian's profit of **USD 17,000**
   (the month-one figures of 1.2.6: receivables up 10,000, office supplies up 6,000, payables up 2,000; no
   depreciation in the pre-adjustment view).
2. **Formula.** `Operating cash = profit + non-cash expenses − increases in current assets + increases in
   current liabilities`.
3. **Substitution.** `17,000 + 0 − 10,000 (receivables) − 6,000 (supplies) + 2,000 (payables) = 3,000`.
4. **Result.** **USD 3,000** — identical to the direct method's total in 1.2.6, as it must be.
5. **Interpretation.** The indirect method explains the profit-to-cash gap line by line: 10,000 is sitting
   in unpaid invoices, 6,000 in unused supplies, offset by 2,000 not yet paid to suppliers. Boards are
   usually shown the indirect form precisely because it makes the working-capital story visible (cross-ref
   1.2.6).

### 1.2.4 The statement of changes in equity (SOCE)

**Definition & purpose.** The **statement of changes in equity** reconciles opening to closing equity,
showing each mover: profit or loss for the period (added to retained earnings), other comprehensive income,
new capital contributed, and distributions to owners. It is where the **profit from the SOPL flows into the
SOFP** — the clearest single view of the articulation described in 1.2.6.

### 1.2.5 The notes

**Definition & purpose.** The **notes** disclose the accounting policies applied and the detail behind the
face numbers (breakdowns, maturities, judgements, contingencies). A controls professional who wants to know
*how* revenue was recognised on a long-term contract, or what a provision comprises, reads the notes, not the
face of the statements. Disclosure is not decoration: under IFRS the notes are an integral part of the
financial statements (cross-ref the IFRS 15 disclosure requirements, Domain 2 KA 2.2).

### 1.2.6 How the statements articulate

**The principle.** The four statements are **not** four independent documents; they *articulate* — they lock
together through shared figures:

- **Profit** from the SOPL increases **retained earnings** in the SOCE, which feeds **equity** on the SOFP.
- The **closing cash** on the SOFP equals the **cash** the statement of cash flows ends on.
- **Working-capital movements** on the SOFP (changes in receivables, payables, inventory) are exactly the
  adjustments the indirect cash-flow statement makes to reconcile profit to operating cash.

**Worked example 1.2.6 — build all statements from Meridian's trial balance.** Using the KA 1.1.5 trial
balance (before adjustments), and treating equipment as long-term:

*Statement of profit or loss (for the month):*

| | USD |
|---|---:|
| Service revenue | 40,000 |
| Salaries expense | (18,000) |
| Rent expense | (5,000) |
| **Profit for the period** | **17,000** |

*Statement of financial position (at month-end):*

| | USD | | | USD |
|---|---:|---|---|---:|
| **Non-current assets** | | | **Equity** | |
| Equipment | 24,000 | | Share capital | 100,000 |
| **Current assets** | | | Retained earnings | 17,000 |
| Office supplies | 6,000 | | **Total equity** | **117,000** |
| Accounts receivable | 10,000 | | **Non-current liabilities** | |
| Cash | 129,000 | | Loan payable | 50,000 |
| | | | **Current liabilities** | |
| | | | Accounts payable | 2,000 |
| **Total assets** | **169,000** | | **Total equity & liabilities** | **169,000** |

*Statement of cash flows (direct method, for the month):*

| | USD |
|---|---:|
| **Operating** | |
| Receipts from customers | 30,000 |
| Payments to staff | (18,000) |
| Payments for rent | (5,000) |
| Payments to suppliers | (4,000) |
| Net cash from operating activities | 3,000 |
| **Investing** | |
| Purchase of equipment | (24,000) |
| **Financing** | |
| Capital contributed | 100,000 |
| Loan drawn | 50,000 |
| **Net increase in cash** | **129,000** |

**The articulation checks (worked).**
- Profit 17,000 → retained earnings 17,000 → equity 117,000 (= share capital 100,000 + 17,000). ✓
- Closing cash on the SOFP = 129,000 = the net increase in cash (opening cash was nil). ✓
- **Indirect reconciliation:** profit 17,000, less the increase in receivables (10,000), less the increase in
  supplies (6,000), plus the increase in payables (2,000) = **3,000** = net operating cash. ✓ This is the
  single most instructive check in the domain: the USD 14,000 gap between 17,000 profit and 3,000 operating
  cash *is* the working capital tied up in unpaid invoices and unused supplies, net of what the firm has not
  yet paid its own supplier.

> **Fig 1.2.1 — How the four statements articulate.** *Caption:* the statements lock together. *Underlying
> data:* Meridian's figures above. *Render-ready description:* four panels — SOPL, SOCE, SOFP, Cash flows —
> with arrows: SOPL "Profit 17,000" → SOCE "Retained earnings 17,000" → SOFP "Equity 117,000"; Cash-flow
> "Closing cash 129,000" → SOFP "Cash 129,000". Arrows in brand blue; the two tie-points ("17,000",
> "129,000") highlighted. *Animation storyboard (digital-only):* profit animates out of the SOPL and lands
> in the SOCE, then flows into SOFP equity; separately the cash-flow total travels into the SOFP cash line;
> both tie-points pulse when matched.

**Common pitfall.** Equating **profit** with **cash**. Meridian earned 17,000 of profit but generated only
3,000 of operating cash this month — a profitable business can still be short of cash if its receivables and
inventory grow faster than its payables. Mistaking one for the other is the root of many project cash crises;
it is why forecasting must model *both* (Domain 3).

**AI in this KA.** Statement-preparation and disclosure-drafting assistants can generate a first draft of a
cash-flow statement from a trial balance and prior period, draft note disclosures, and run consistency checks
(does the closing cash tie to the SOFP? does the SOCE reconcile?). The professional retains accountability:
AI classification of a cash flow as operating vs financing can be wrong, and disclosures drafted by a model
must be checked against the actual policy and the standard — **AI proposes, the professional disposes.**

### Key terms — KA 1.2

| Term | Meaning |
|---|---|
| **SOFP** | Statement of financial position — assets, liabilities, equity at a point in time. |
| **SOPL & OCI** | Statement of profit or loss and other comprehensive income — performance over a period. |
| **Statement of cash flows** | Change in cash split into operating, investing and financing. |
| **SOCE** | Statement of changes in equity — reconciles opening to closing equity. |
| **Articulation** | The way the statements interlock through shared figures (profit, cash, working capital). |
| **Current vs non-current** | The IAS 1 split by expected realisation/settlement within ~12 months. |

### Sample MCQs — KA 1.2

**MCQ 1.2-A `[1.2.6 · Analysis]`** A company reports profit of USD 17,000 but net operating cash of USD
3,000 in the same period. The most likely explanation is:
- A. An accounting error, since profit should equal operating cash.
- B. Growth in working capital — receivables and inventory rose faster than payables. ✅
- C. The company paid a dividend of USD 14,000.
- D. Depreciation of USD 14,000 was charged.

*Rationale:* Profit and operating cash differ by non-cash items and working-capital movements. Here the
USD 14,000 gap is receivables (+10,000) and supplies (+6,000) tying up cash, net of payables (+2,000). A is
wrong — the two need not be equal. C is a financing (not operating) flow; D would make cash *higher* than
profit, not lower.

**MCQ 1.2-B `[1.2.3 · Application]`** Purchasing equipment for cash appears in the cash-flow statement as:
- A. An operating outflow.
- B. An investing outflow. ✅
- C. A financing outflow.
- D. It does not appear, being non-cash.

*Rationale:* Buying a long-term asset is an investing activity. It is a genuine cash outflow, so B. A and C
misclassify it; D is wrong because cash did move.

**MCQ 1.2-C `[1.2.6 · Recall]`** Through which statement does the period's profit reach equity on the SOFP?
- A. The statement of cash flows.
- B. The notes.
- C. The statement of changes in equity. ✅
- D. The trial balance.

*Rationale:* The SOCE reconciles opening to closing equity and is where profit is added to retained
earnings. The cash-flow statement and notes do not carry profit into equity; the trial balance is not a
primary statement.

**MCQ 1.2-D `[1.2.1 · Recall]`** Under IAS 1, the SOFP normally classifies assets and liabilities as:
- A. Tangible vs intangible.
- B. Current vs non-current. ✅
- C. Monetary vs non-monetary.
- D. Operating vs financing.

*Rationale:* IAS 1's normal presentation splits by current/non-current (a liquidity view). The others are
real distinctions but not the standard face-of-SOFP classification.

### Self-check — KA 1.2

1. Name the four primary financial statements and what each communicates. *(SOFP — position at a point;
   SOPL & OCI — performance over a period; cash flows — change in cash by activity; SOCE — movement in
   equity.)*
2. Explain one way the statements articulate. *(e.g. profit → retained earnings → equity; or closing cash
   ties SOFP to the cash-flow statement.)*
3. Why can a profitable period still be cash-negative? *(Working capital — growth in receivables/inventory
   exceeding payables — and investing/financing outflows consume cash the accrual profit does not reflect.)*

---

## Knowledge Area 1.3 — Accrual accounting and the matching concept

*Topics: 1.3.1 accrual vs cash basis · 1.3.2 the matching principle · 1.3.3 the four period-end adjustments ·
1.3.4 depreciation as systematic matching · 1.3.5 why controls professionals care about the cut-off.*

### 1.3.1 Accrual basis versus cash basis

**Definition & purpose.** Under the **cash basis**, income and expense are recognised when **cash** is
received or paid. Under the **accrual basis** — the basis required by IFRS for general-purpose financial
statements — they are recognised when the underlying **economic event** occurs (the service is delivered, the
resource is consumed), regardless of when cash moves. Meridian's month shows the difference starkly: on a
cash basis its "profit" would be the operating cash of **3,000**; on the accrual basis it is **17,000**,
because it earned revenue it has not yet collected and consumed resources it has not yet paid for. Accrual
accounting gives the truer picture of performance — but it requires **judgement about timing**, and that
judgement is where a controls professional's month-end work lives.

### 1.3.2 The matching principle

**Definition & purpose.** The **matching concept** requires that expenses be recognised in the **same period
as the income they help to earn**. If a cost is incurred to generate this period's revenue, it belongs in
this period's profit or loss — not the period the invoice happens to be paid. Matching is the reason unused
supplies sit as an *asset* until consumed (1.1.3), and the reason the cost of a long-lived asset is spread
across the periods it helps produce revenue (depreciation, 1.3.4).

**Underlying principle.** Matching, together with the accrual basis, is what makes profit a measure of
*performance* rather than *cash timing*. It also introduces the risk the controls professional must guard
against: because recognition depends on judgement, it can be *managed* — costs deferred to flatter this
period, revenue pulled forward — which is why cut-off discipline and reconciliation matter (1.3.5, Domain 11).

### 1.3.3 The four period-end adjustments

At period-end, entries are made to bring the accounts onto the accrual basis. There are four canonical types,
best learned as a 2×2: is it an **expense or income**, and is the cash **before or after** the recognition?

| | Cash paid/received *after* recognition (accrue) | Cash paid/received *before* recognition (defer) |
|---|---|---|
| **Expense** | **Accrued expense** — expense incurred, not yet invoiced/paid (Dr expense, Cr accrued liability) | **Prepayment** — cash paid in advance, expense not yet incurred (asset until used) |
| **Income** | **Accrued income** — revenue earned, not yet billed (Dr accrued asset, Cr revenue) | **Deferred income** — cash received in advance, revenue not yet earned (liability until earned) |

**Worked example 1.3.3 — Meridian's four adjustments.** At month-end, before finalising:

1. **Accrued expense — salaries.** Staff earned **USD 2,000** in the final days, unpaid at month-end.
   `Dr Salaries expense 2,000 / Cr Accrued liabilities 2,000`. Profit falls by 2,000; a current liability
   appears.
2. **Prepayment — insurance.** Meridian paid **USD 3,600** for twelve months' insurance at the start of the
   month. Only one month is used: expense `3,600 / 12 = 300`. `Dr Insurance expense 300 / Cr Prepaid
   insurance (asset) 3,300` at recognition (or: recognise the whole 3,600 as a prepaid asset on payment,
   then `Dr Insurance expense 300 / Cr Prepaid insurance 300` at month-end, leaving a 3,300 asset). Only 300
   hits this month's profit.
3. **Accrued income — retainer.** Meridian has earned **USD 1,500** of an advisory retainer not yet billed.
   `Dr Accrued income (asset) 1,500 / Cr Service revenue 1,500`. Revenue and profit rise by 1,500.
4. **Deferred income — prepaid training.** A client paid **USD 4,000** in advance for a workshop not yet
   delivered. `Dr Cash 4,000 / Cr Deferred income (liability) 4,000` on receipt; **no** revenue is
   recognised until the workshop is delivered.

**Net effect on profit of adjustments 1–3** (adjustment 4 was recorded on receipt and changes no revenue
this period): `− 2,000 (salaries) − 300 (insurance) + 1,500 (accrued income) = − 800`. Adjusted profit =
`17,000 − 800 = 16,200` before depreciation and supplies (see 1.3.4).

### 1.3.4 Depreciation as systematic matching

**Definition & purpose.** A long-lived asset (Meridian's equipment) helps earn revenue across many periods,
so its cost is **spread** across those periods rather than expensed at purchase — this is **depreciation**,
a direct application of matching. The **straight-line** method charges an equal amount each period:

```
Annual depreciation = (Cost − Residual value) / Useful life
```
- `Cost` — capitalised cost of the asset (currency).
- `Residual value` — estimated proceeds at end of life (currency).
- `Useful life` — periods over which the asset is used (years).

Depreciation is a **contra-asset** (accumulated depreciation, credit balance) offsetting the asset's cost;
the net is the **carrying amount**. (The related IFRS on property, plant and equipment, IAS 16, is developed
in Domain 2 KA 2.4; here it is used purely to illustrate matching.)

**Worked example 1.3.4 — depreciate Meridian's equipment.**

1. **Setup.** Equipment cost **USD 24,000**, estimated **residual value USD 0**, **useful life 3 years**.
   Compute the **monthly** charge.
2. **Formula.** `Annual depreciation = (Cost − Residual) / Life`; monthly = annual / 12.
3. **Substitution.** `(24,000 − 0) / 3 = 8,000` per year; `8,000 / 12 = 666.67` per month.
4. **Result.** **USD 667** per month (to the nearest whole unit). `Dr Depreciation expense 667 / Cr
   Accumulated depreciation 667`. Carrying amount = `24,000 − 667 = 23,333`.
5. **Interpretation.** This month bears 667 of the equipment's cost — the share matched to the revenue it
   helped earn — not the full 24,000. The full amount was an *investing* cash outflow (1.2.3); the *expense*
   is spread.

**Supplies consumed (matching, worked).** Of the USD 6,000 supplies, **USD 2,500** remain at month-end, so
**USD 3,500** were consumed: `Dr Supplies expense 3,500 / Cr Office supplies 3,500`. The remaining 2,500
stays as an asset. Bringing everything together, Meridian's **fully adjusted profit** for the month:

| | USD |
|---|---:|
| Profit before adjustments (1.2.6) | 17,000 |
| Accrued salaries | (2,000) |
| Insurance used | (300) |
| Accrued advisory income | 1,500 |
| Depreciation | (667) |
| Supplies consumed | (3,500) |
| **Adjusted profit for the month** | **12,033** |

### 1.3.5 Why the cut-off matters to controls professionals

**The professional angle.** *Cut-off* is the discipline of recording each transaction in the **correct
period**. It is where accounting meets project controls most directly: an accrual for work **performed** but
not yet **invoiced** by a subcontractor is exactly the figure a cost engineer must include to state true
cost-to-date — otherwise cost is understated, `CPI` looks artificially healthy, and the forecast is built on
sand (cross-ref Domain 6, EVM, and Domain 5, the cost-control cycle: commitment → accrual → actual). A
controls professional who understands accruals raises them proactively at month-end rather than discovering
the cost two months later as a "surprise" overrun.

> **Fig 1.3.1 — Accrual vs cash timing on one line.** *Caption:* when the same USD 2,000 of work hits profit
> under each basis. *Underlying data:* work performed in Month 1; invoiced Month 2; paid Month 3.
> *Render-ready description:* a three-month timeline; a blue marker "Accrual basis — expense recognised" on
> Month 1 (when work is performed); a grey marker "Cash basis — expense recognised" on Month 3 (when paid);
> a bracket spanning Months 1–3 labelled "accrual + reconciliation window". *Animation storyboard
> (digital-only):* the USD 2,000 block slides along the timeline; under "accrual" it drops into Month 1's
> profit immediately; under "cash" it waits and drops into Month 3 — visually showing the timing difference.

**Common pitfall.** Treating the *invoice date* as the recognition date. Under accrual accounting the trigger
is the **economic event** (work performed, goods received, service consumed), not the arrival of a document.
A cost incurred on 29 March but invoiced 5 April belongs to **March**.

**AI in this KA.** Accrual-assist tools can scan open purchase orders, goods-received-not-invoiced reports
and timesheets to *propose* month-end accruals, and flag costs whose service date and invoice date straddle a
period boundary. This is high-value — cut-off errors are laborious to find manually. But the accrual *amount*
often needs professional estimation (how much of a part-delivered service was performed?), and a model that
accrues from a document date rather than a service date will reproduce the very pitfall above. **AI proposes,
the professional disposes.**

### Key terms — KA 1.3

| Term | Meaning |
|---|---|
| **Accrual basis** | Recognise economic events when they occur, not when cash moves. |
| **Matching principle** | Recognise expenses in the same period as the income they help earn. |
| **Accrued expense / income** | Recognised before the cash is paid / received. |
| **Prepayment / deferred income** | Cash paid / received before the expense / income is recognised. |
| **Depreciation** | Systematic spreading of a long-lived asset's cost over its useful life. |
| **Carrying amount** | Cost less accumulated depreciation (and impairment). |
| **Cut-off** | Recording each transaction in the correct period. |

### Sample MCQs — KA 1.3

**MCQ 1.3-A `[1.3.4 · Application]`** Equipment costs USD 24,000, residual USD 0, life 3 years, straight-line.
The monthly depreciation is:
- A. USD 8,000
- B. USD 2,000
- C. USD 667 ✅
- D. USD 24,000

*Rationale:* `(24,000 − 0)/3 = 8,000` per year; `8,000/12 ≈ 667` per month. A is the *annual* charge; B and D
confuse the period or expense the whole cost at once.

**MCQ 1.3-B `[1.3.3 · Application]`** A client pays USD 4,000 in advance for a workshop not yet delivered. On
receipt the entry is:
- A. Dr Cash 4,000; Cr Service revenue 4,000
- B. Dr Cash 4,000; Cr Deferred income 4,000 ✅
- C. Dr Deferred income 4,000; Cr Cash 4,000
- D. Dr Accrued income 4,000; Cr Cash 4,000

*Rationale:* Cash received before the service is earned creates a **liability** (deferred income), released to
revenue only on delivery. A recognises revenue too early; C and D reverse or misname the accounts.

**MCQ 1.3-C `[1.3.5 · Analysis]`** A subcontractor performed work on 29 March, invoiced 5 April, paid 30
April. On the accrual basis the cost belongs in:
- A. March ✅
- B. April (invoice date)
- C. April (payment date)
- D. Split evenly across March and April

*Rationale:* Accrual recognition follows the **economic event** — work performed on 29 March — so the cost is
a March accrual. The invoice and payment dates are irrelevant to the period of recognition. This is exactly
the accrual a cost engineer must raise to state true cost-to-date.

**MCQ 1.3-D `[1.3.1 · Analysis]`** Meridian's accrual profit is USD 17,000 but operating cash is USD 3,000.
On a pure **cash basis**, the period's profit would be closest to:
- A. USD 17,000
- B. USD 3,000 ✅
- C. USD 20,000
- D. USD 14,000

*Rationale:* Cash-basis profit approximates net operating cash — here USD 3,000. A is the accrual figure; D is
the *gap* between the two, not either measure; C double-counts.

### Self-check — KA 1.3

1. State the matching principle and give an example from Meridian's month. *(Recognise expense with the
   income it earns; e.g. only USD 3,500 of supplies consumed is expensed, the rest stays an asset.)*
2. Classify each: rent paid in advance; interest owed but unpaid; a deposit received for future work.
   *(Prepayment/asset; accrued expense/liability; deferred income/liability.)*
3. Why does an accrual for uninvoiced subcontractor work matter to the EVM cost figure? *(Omitting it
   understates actual cost `AC`, inflating `CPI` and corrupting the forecast — cross-ref Domain 6.)*

---

## Knowledge Area 1.4 — Cost provisions and cost accruals (IAS 37)

*Topics: 1.4.1 accrual vs provision · 1.4.2 the IAS 37 recognition tests · 1.4.3 measurement (best estimate,
expected value, discounting) · 1.4.4 contingent liabilities and assets · 1.4.5 onerous (loss-making)
contracts · 1.4.6 remeasurement and reversal.*

### 1.4.1 Accrual versus provision

**Definition & purpose.** Both an **accrual** and a **provision** recognise an obligation before it is paid,
but they differ in **certainty**. An **accrual** is a liability to pay for goods or services **received**,
where the amount and timing are known or readily estimable (last week's electricity, uninvoiced subcontractor
work). A **provision**, under **IAS 37 (provisions, contingent liabilities and contingent assets)**, is a
liability of **uncertain timing or amount** — a warranty obligation, a decommissioning cost, a probable
claim. The distinction matters because provisions carry recognition tests and measurement rules that simple
accruals do not.

### 1.4.2 The IAS 37 recognition tests

**The principle.** Under IAS 37 a provision is recognised only when **all three** tests are met:

1. There is a **present obligation** (legal or constructive) arising from a **past event**;
2. It is **probable** (more likely than not) that an outflow of resources will be required to settle it; and
3. A **reliable estimate** can be made of the amount.

If any test fails, no provision is recognised — instead there may be a **contingent liability** to *disclose*
(1.4.4). A **constructive** obligation arises where the entity's established practice or public statements
create a valid expectation in others (e.g. a published policy of honouring warranties beyond the strict legal
term) — relevant where a controls or commercial function has committed the organisation in practice.

**Common pitfall — provisioning for future operating losses.** IAS 37 does **not** permit a provision for
*future* operating losses: there is no present obligation from a past event — the losses have not yet
happened and can be avoided by future action. (An *onerous contract*, 1.4.5, is different: the obligating
past event is signing the contract.) Provisioning for expected future losses generally is a classic error and
a favourite exam trap.

### 1.4.3 Measurement — best estimate, expected value, discounting

**The principle.** A provision is measured at the **best estimate** of the expenditure required to settle the
obligation at the reporting date. How the best estimate is computed depends on the population:

- For a **large population** of similar obligations (e.g. warranties across thousands of units), the best
  estimate is the **expected value** — the probability-weighted average of outcomes.
- For a **single** obligation, the best estimate may be the **most likely** outcome, adjusted for other
  possible outcomes.
- Where the time value of money is material (settlement is years away), the provision is **discounted** to
  present value.

**Formulae.**
```
Expected value  =  Σ ( probability_i × outcome_i )
Present value   =  Future amount / (1 + r)^n
```
- `probability_i`, `outcome_i` — the chance and cost of scenario *i*.
- `r` — discount rate per period (a pre-tax rate reflecting current market assessments and the risks specific
  to the liability); `n` — number of periods to settlement.

**Worked example 1.4.3a — warranty provision by expected value (large population).**

1. **Setup.** Meridian's instrumentation arm delivers **1,000** monitoring units under a one-year warranty.
   Experience: **6 %** of units require a repair averaging **USD 80** each; the rest cost nothing.
2. **Formula.** `Expected value = Σ (probability × outcome)`.
3. **Substitution.** `1,000 × 6 % × USD 80 = 1,000 × 0.06 × 80`.
4. **Result.** **USD 4,800** provision. `Dr Warranty expense 4,800 / Cr Warranty provision 4,800`.
5. **Interpretation.** The obligation exists now (units are sold under warranty — a past event), an outflow is
   probable across the population, and it is reliably estimable, so a provision — not a mere disclosure — is
   required this period, matched against the revenue from the sales.

**Worked example 1.4.3b — single obligation, most-likely with discounting.**

1. **Setup.** Meridian faces one disputed claim. Outcomes: **75 %** it settles for **USD 0**, **20 %** for
   **USD 50,000**, **5 %** for **USD 200,000**; expected settlement in **3 years**; discount rate **8 %**.
2. **Formula.** `Expected value = Σ(p × outcome)`, then `PV = amount / (1 + r)^n`.
3. **Substitution.** `Expected value = 0.75×0 + 0.20×50,000 + 0.05×200,000 = 0 + 10,000 + 10,000 = 20,000`.
   Discount: `1.08^3 = 1.259712`; `PV = 20,000 / 1.259712`.
4. **Result.** Undiscounted best estimate **USD 20,000**; discounted **USD 15,877** (`20,000 / 1.259712 =
   15,876.6`). The provision is carried at **USD 15,877**.
5. **Interpretation.** Because settlement is three years out and the amounts are material, the time value of
   money reduces the present obligation from 20,000 to ~15,877. As settlement approaches, the discount
   *unwinds* — the provision is increased each year through a finance cost until it reaches 20,000 (1.4.6).

**Worked example 1.4.3c — a decommissioning provision and its discount unwind.**

1. **Setup.** Meridian has a **legal obligation** to decommission a facility at the end of its use,
   estimated at **USD 500,000** payable in **4 years**; discount rate **7 %**.
2. **Formula.** `Present value = Future amount / (1 + r)^n`. On recognition the discounted amount is added
   both to the **asset** (PPE) and to a **provision**; each year thereafter the discount **unwinds** as a
   finance cost.
3. **Substitution.** `1.07^4 = 1.310796`; `PV = 500,000 / 1.310796 = 381,447`. Entry: `Dr Decommissioning
   asset 381,447 / Cr Provision 381,447`.
4. **Result.** Provision recognised at **USD 381,447**. Year-1 unwind: `381,447 × 7 % = 26,701`, so the
   provision grows to **USD 408,148** at the end of Year 1 (`Dr Finance cost 26,701 / Cr Provision 26,701`).
5. **Interpretation.** The provision rises each year through the discount unwind until it reaches USD
   500,000 at settlement. A controls professional tracking such a provision should expect its carrying
   amount to move from **both** re-estimation and unwind, and be able to explain each movement (1.4.6).

### 1.4.4 Contingent liabilities and contingent assets

**The principle.** Where an obligation is only **possible** (not probable), or is present but **cannot be
reliably measured**, IAS 37 requires it to be **disclosed as a contingent liability**, not recognised. A
**contingent asset** (a possible inflow, e.g. a claim the entity has made against another party) is *not*
recognised while it is merely possible, and is disclosed only when an inflow is **probable**; it is
recognised only when the inflow is **virtually certain** (at which point it is no longer contingent). The
asymmetry — quicker to book bad news than good — reflects **prudence**: the framework does not let entities
recognise gains that may never arrive.

### 1.4.5 Onerous (loss-making) contracts

**Definition & purpose.** An **onerous contract** is one in which the **unavoidable costs** of meeting the
obligations **exceed the economic benefits** expected from it. Under IAS 37 the **present obligation under the
contract** is recognised as a **provision** — the expected loss is booked **immediately**, not spread over
the remaining life. This is directly relevant to project controls: the moment a forecast shows a fixed-price
job's cost-to-complete will push total cost above contract value, the loss is recognised now.

**Worked example 1.4.5 — provide for a contract loss.**

1. **Setup.** Meridian holds a **fixed-price** controls contract, price **USD 500,000**. Cost incurred to
   date **USD 300,000**; estimated **cost to complete USD 280,000**. Assume the costs are unavoidable.
2. **Formula.** `Forecast total cost = cost to date + cost to complete`; `Forecast loss = price − forecast
   total cost`; provide for the loss **not yet recognised** in results to date.
3. **Substitution.** `Forecast total cost = 300,000 + 280,000 = 580,000`; `Forecast loss = 500,000 − 580,000
   = (80,000)`.
4. **Result.** The **full USD 80,000 loss is recognised now** as a provision, to the extent not already
   reflected in costs expensed to date. `Dr Contract-loss expense / Cr Provision for onerous contract`
   (for the unrecognised portion of the 80,000).
5. **Interpretation.** Prudence and IAS 37 forbid carrying a known future loss forward to the period it is
   paid — once the contract is expected to lose money, the whole expected loss hits current profit. For a
   controls professional this is the accounting consequence of a forecast EAC exceeding contract value
   (cross-ref Domain 6, EAC; Domain 7, contract types and risk allocation).

> **Fig 1.4.1 — IAS 37 recognition decision tree.** *Caption:* provision, disclose, or ignore. *Underlying
> data:* the three recognition tests. *Render-ready description:* a decision tree — "Present obligation from a
> past event?" → No → *do nothing*; Yes → "Outflow probable?" → No → "Only possible?" → Yes → *disclose
> contingent liability* / No → *do nothing*; Yes (probable) → "Reliably estimable?" → No → *disclose* / Yes →
> **recognise provision (measure per 1.4.3)**. Decision nodes as brand-blue diamonds; the three terminal
> outcomes colour-coded. *Animation storyboard (digital-only):* the warranty case (1.4.3a) is dropped in at
> the top and traced down the "Yes/Yes/Yes" path to "recognise provision", then the disputed-claim case is
> traced separately, illustrating why one is provided and one might only be disclosed.

### 1.4.6 Remeasurement and reversal

**The principle.** Provisions are **reviewed at each reporting date** and adjusted to the current best
estimate. If an outflow is no longer probable, the provision is **reversed** (released to profit). Where a
provision was discounted, the **unwinding of the discount** — the increase in present value as settlement
nears — is recognised as a **finance cost** each period. A controls professional tracking a provision (say,
for a claim or a decommissioning obligation) should expect its carrying amount to *move* period to period,
both from re-estimation and from discount unwind, and should be able to explain each movement.

**AI in this KA.** AI can *support* provisioning: mining claims histories and warranty-return data to inform
probability and cost estimates, scanning contract portfolios to flag candidates for onerous-contract review
where forecast cost approaches price, and checking that disclosures are internally consistent. It cannot
*make the judgement* — whether an obligation is "probable", whether costs are "unavoidable", whether a
constructive obligation exists — which is a matter for professional and often legal judgement, auditable and
signed off by a person. Over-reliance risks both under-provisioning (missing a probable outflow) and
over-provisioning (booking a loss the standard would not permit). **AI proposes, the professional disposes.**

### Key terms — KA 1.4

| Term | Meaning |
|---|---|
| **Accrual** | Liability for goods/services received, amount/timing reasonably certain. |
| **Provision (IAS 37)** | Liability of uncertain timing or amount, meeting the three recognition tests. |
| **Present obligation** | A legal or constructive duty arising from a past event. |
| **Expected value** | Probability-weighted average of outcomes (for large populations). |
| **Contingent liability / asset** | A possible obligation/inflow — disclosed, not recognised (subject to probability). |
| **Onerous contract** | Unavoidable costs exceed expected benefits; the loss is provided immediately. |
| **Discount unwind** | The increase in a discounted provision as settlement nears, charged as finance cost. |

### Sample MCQs — KA 1.4

**MCQ 1.4-A `[1.4.3 · Application]`** 2,000 units are sold under warranty; 5 % are expected to need a repair
costing USD 120 on average. The warranty provision is:
- A. USD 240,000
- B. USD 12,000 ✅
- C. USD 120
- D. USD 6,000

*Rationale:* `2,000 × 0.05 × 120 = 12,000` (expected value). A ignores the 5 % probability; C is a single
repair; D halves the rate or the cost in error.

**MCQ 1.4-B `[1.4.2 · Analysis]`** Which is **not** permitted to be recognised as a provision under IAS 37?
- A. A warranty obligation on units already sold.
- B. Expected operating losses of the next financial year. ✅
- C. A probable, reliably estimable legal claim from a past event.
- D. The unavoidable loss on an onerous contract already signed.

*Rationale:* IAS 37 prohibits provisioning for **future operating losses** — there is no present obligation
from a past event. A, C and D all arise from past events (sales, an incident, signing a contract) and can
meet the tests.

**MCQ 1.4-C `[1.4.5 · Application]`** A fixed-price contract has a price of USD 500,000, cost to date USD
300,000 and estimated cost to complete USD 280,000. The loss to recognise now is:
- A. USD 0 — recognise it as costs are incurred.
- B. USD 80,000 ✅
- C. USD 280,000
- D. USD 200,000

*Rationale:* Forecast total cost `300,000 + 280,000 = 580,000` exceeds the price by **80,000**, an onerous
contract — the whole expected loss is recognised now. A defers a known loss (not permitted); C and D confuse
cost-to-complete or cost-to-date with the loss.

**MCQ 1.4-D `[1.4.3 · Application]`** A single obligation's best estimate is USD 20,000, payable in 3 years;
the discount rate is 8 %. The provision's present value is closest to:
- A. USD 20,000
- B. USD 15,877 ✅
- C. USD 25,194
- D. USD 18,519

*Rationale:* `20,000 / 1.08^3 = 20,000 / 1.259712 ≈ 15,877`. A ignores discounting; C *compounds* instead of
discounting; D discounts only one year (`20,000/1.08`).

**MCQ 1.4-E `[1.4.4 · Recall]`** A contingent asset is recognised in the financial statements when an inflow
is:
- A. Possible.
- B. Probable.
- C. Virtually certain. ✅
- D. Merely estimable.

*Rationale:* Prudence means a contingent asset is only *recognised* when the inflow is virtually certain
(and thus no longer contingent); it is *disclosed* when probable, and ignored when only possible.

### Self-check — KA 1.4

1. State the three IAS 37 recognition tests. *(Present obligation from a past event; probable outflow;
   reliable estimate.)*
2. When is expected value the right measurement basis, and when is most-likely-outcome? *(Expected value for
   a large population of similar items; most-likely for a single obligation, adjusted for other outcomes.)*
3. A forecast shows a signed fixed-price job will cost USD 80,000 more than its price. What does IAS 37
   require, and when? *(Recognise the full USD 80,000 expected loss immediately as an onerous-contract
   provision.)*

---

## Knowledge Area 1.5 — Chart of accounts and cost coding for projects

*Topics: 1.5.1 the chart of accounts · 1.5.2 cost extraction, coding and reconciliation · 1.5.3 the cost
breakdown structure and its link to the WBS · 1.5.4 control accounts (the WBS×OBS intersection) · 1.5.5
designing a project cost code.*

### 1.5.1 The chart of accounts

**Definition & purpose.** The **chart of accounts (CoA)** is the structured list of every account in the
ledger, each with a code and a type (asset, liability, equity, income, expense). A well-designed CoA is what
lets an organisation both prepare statutory statements *and* analyse cost by project, phase and cost element
— the same postings, aggregated different ways. For project controls the CoA is the backbone that connects a
financial posting to a *piece of work*.

**A typical account-code range (illustrative).**

| Range | Account class |
|---|---|
| 1000–1999 | Assets |
| 2000–2999 | Liabilities |
| 3000–3999 | Equity |
| 4000–4999 | Income / revenue |
| 5000–5999 | Cost of sales / direct project cost |
| 6000–6999 | Operating expenses (labour, overhead) |

### 1.5.2 Cost extraction, coding and reconciliation

**The professional core.** In practice a controls professional spends real time on three linked tasks:
**extracting** cost from the ERP/source systems, **coding** each cost to the right project structure, and
**reconciling** the extracted cost back to the general ledger so the controls report and the financial
accounts tell the same story. Discrepancies — a cost booked to the wrong project, a commitment not yet
accrued, a duplicate — are precisely what reconciliation surfaces (recall that a trial balance alone cannot,
1.1.4). This is the accounting-side counterpart of the cost-control cycle (**commitment → accrual → actual**)
developed in Domain 5, KA 5.2.

**Common pitfall — coding to the account but not the *project*.** A cost correctly classified as "labour"
(account 6100) but coded to the wrong project or WBS element is *right* in the financial accounts and *wrong*
in the project accounts. Statutory reporting looks fine while project cost is misstated, `CPI` is distorted,
and the variance is chased on the wrong job. Robust coding at source — not reclassification after the fact —
is the control.

### 1.5.3 The cost breakdown structure and its link to the WBS

**Definition & purpose.** The **cost breakdown structure (CBS)** decomposes a project's cost by **cost
element/type** (labour, materials, plant, subcontract, overhead). The **work breakdown structure (WBS)**
decomposes the **scope of work** into deliverables and work packages. Project cost is coded to the
**intersection** of the two: *this cost element, on this work package*. That intersection is what lets a
controls professional answer both "how much have we spent on **foundations**?" (a WBS view) and "how much
have we spent on **subcontract labour** across the project?" (a CBS view) from the same postings.

### 1.5.4 Control accounts — the WBS×OBS intersection

**Definition & purpose.** A **control account (CA)** is a management-control point where **scope, budget,
actual cost and schedule integrate** — formally, the intersection of a **WBS element** (a piece of scope) and
an **OBS element** (the organisational unit accountable for it). The control account is the level at which
earned value is measured and performance is managed; it is the hinge between this accounting foundation and
the EVM machinery of Domain 6. A cost code that carries both a WBS reference and an accountable-unit
reference *is* a posting to a control account.

> **Fig 1.5.1 — WBS × CBS × OBS: how a cost is coded.** *Caption:* one cost, three coordinates.
> *Underlying data:* a labour cost of USD 12,000 for a planner on the "Foundations" work package, owned by
> the "Civils" team. *Render-ready description:* a cube/matrix diagram — one axis WBS (Foundations,
> Structure, Fit-out), one axis CBS (Labour, Materials, Subcontract, Plant), one axis OBS (Civils, M&E,
> Controls); a single highlighted cell at (Foundations, Labour, Civils) labelled "USD 12,000 → control
> account CA-Civils-Foundations". Brand-blue highlight on the intersecting cell. *Animation storyboard
> (digital-only):* the USD 12,000 posting enters and slides along each axis in turn until it lands in the one
> highlighted cell, showing how the code pins it to scope, cost type and accountability at once.

### 1.5.5 Designing a project cost code — worked

**Worked example 1.5.5 — build and apply a cost code.**

1. **Setup.** Design a segmented cost code for a portfolio, then code one transaction: a planner's time,
   **USD 12,000**, on the *Foundations* work package of project *1420*, owned by *Civils*.
2. **Structure.** Segments — `Company (2) – Project (4) – WBS (3) – Cost element (4) – Resource (3)`:

   | Segment | Value | Meaning |
   |---|---|---|
   | Company | `01` | Meridian (entity) |
   | Project | `1420` | The controls contract |
   | WBS | `120` | Foundations work package |
   | Cost element | `6100` | Direct labour |
   | Resource | `210` | Planning engineer |

3. **Code.** `01-1420-120-6100-210`. Entry: `Dr 01-1420-120-6100-210 (Labour, Foundations) 12,000 / Cr Cash
   (or accrued payroll) 12,000`.
4. **Result.** The one posting now supports every rollup: by **project** (`1420`), by **work package**
   (`120`, a WBS/CBS view), by **cost element** (`6100`, total labour), and by **resource** (`210`), and it
   maps to the control account owned by Civils.
5. **Interpretation.** Coding discipline at the point of entry is what makes controls reporting, statutory
   reporting and earned value reconcile automatically — rather than being stitched together by hand each
   month. A code designed once, applied consistently, is worth more than any amount of downstream
   reclassification.

**AI in this KA.** This is one of the highest-value, lowest-risk AI applications in the domain. Classification
models and LLM assistants can **propose** the cost code for a described or free-text transaction (mapping a
supplier invoice narrative to project/WBS/cost element), **detect anomalies** (a cost coded to a closed WBS
element, an outlier against the work package's run-rate, a probable duplicate), and **accelerate
reconciliation** by matching extracted cost to ledger balances and flagging the exceptions. The governance
line still holds: an auto-coded cost must be reviewable and correctable, mis-coding propagates into every
report downstream, and the professional owns the mapping rules and the exceptions. **AI proposes, the
professional disposes.**

### 1.5.6 Sector mini-case — a contractor's month-end accrual pack

At month-end a civils contractor's cost engineer finds that a subcontractor has **performed USD 240,000**
of work on a package but has **invoiced only USD 180,000** — the paperwork is a month behind the shovels.
If only the USD 180,000 of invoices on file is recorded, cost-to-date is understated by **USD 60,000**: the
ledger says the work was cheaper than it was, purely because of invoice timing. The cost engineer raises a
**USD 60,000 accrual** (`Dr Subcontract cost 60,000 / Cr Accrued liabilities 60,000`) so that cost-to-date
reflects work *performed*, not work *invoiced*.

The earned-value consequence makes the stakes concrete: with `AC` understated by 60,000, `CPI = EV/AC`
would be flattered — the job would look more cost-efficient than it is, and the truth would surface later
as an unexplained deterioration when the invoices caught up. The accrual is what keeps the earned-value
cost figure honest (cross-ref 1.3.5 on cut-off, and Domain 6). A disciplined month-end accrual pack —
every known-but-uninvoiced cost identified, estimated and coded to the right control accounts (1.5.4) — is
the bridge between the ledger and trustworthy project cost.

### Key terms — KA 1.5

| Term | Meaning |
|---|---|
| **Chart of accounts (CoA)** | The structured list of all ledger accounts, coded by class. |
| **Cost extraction / reconciliation** | Pulling cost from source systems and tying it back to the ledger. |
| **Cost breakdown structure (CBS)** | Decomposition of cost by element/type. |
| **Work breakdown structure (WBS)** | Decomposition of scope into deliverables/work packages. |
| **Control account (CA)** | The WBS×OBS intersection where scope, budget, cost and schedule integrate. |
| **Cost code** | A segmented code pinning a posting to project, scope, cost element and resource. |

### Sample MCQs — KA 1.5

**MCQ 1.5-A `[1.5.4 · Recall]`** A control account is best described as the intersection of:
- A. A cost element and a resource.
- B. A WBS element and an OBS element. ✅
- C. Two ledger accounts.
- D. A project and a company code.

*Rationale:* A control account is where scope (WBS) meets accountability (OBS) and is the level at which
earned value is managed. A describes a coding detail; C and D are aggregations, not the control-account
definition.

**MCQ 1.5-B `[1.5.2 · Analysis]`** A labour cost is correctly coded to account 6100 (labour) but to the wrong
project. The consequence is:
- A. The trial balance will not balance.
- B. Statutory totals are wrong but project cost is right.
- C. Statutory totals are right but project cost — and any CPI derived from it — is wrong. ✅
- D. No consequence; account classification is what matters.

*Rationale:* The cost is the right *type*, so financial statements aggregate correctly, but it lands on the
wrong job, misstating project cost and distorting `CPI`. A is false (debits still equal credits); B reverses
the effect; D ignores the project view entirely.

**MCQ 1.5-C `[1.5.5 · Application]`** In the code `01-1420-120-6100-210`, which segment identifies the *scope*
the cost belongs to?
- A. `1420` (project)
- B. `120` (WBS work package) ✅
- C. `6100` (cost element)
- D. `210` (resource)

*Rationale:* The WBS segment (`120`, Foundations) pins the cost to a piece of *scope*. The project is the
whole job; `6100` is the cost *type*; `210` is the resource — none of these is the scope element.

### Self-check — KA 1.5

1. Give one thing a WBS view of cost tells you that a CBS view does not, and vice versa. *(WBS: spend by
   deliverable/work package, e.g. Foundations; CBS: spend by cost type, e.g. total subcontract, across the
   project.)*
2. Why is coding correctly *at source* better than reclassifying later? *(Mis-coding propagates into every
   downstream report and distorts project cost/CPI before it is caught; source discipline prevents it.)*
3. How does a control account link this domain to earned value? *(It is the WBS×OBS point where budget, cost
   and schedule integrate — the level at which EV is measured in Domain 6.)*

---

## Domain 1 summary

The accounting model is one identity — `A = L + E` — recorded from both sides (double-entry), producing four
articulating statements that separate *performance* (accrual profit) from *cash*. Accrual accounting and the
matching concept decide *when* cost and revenue land, through period-end adjustments and depreciation, and the
cut-off discipline they demand is the same discipline a controls professional applies when raising accruals
for uninvoiced work. IAS 37 governs the obligations recognised before they are paid — provisions measured at a
best estimate (expected value or most-likely, discounted where material), the immediate recognition of onerous
-contract losses, and the disclose-don't-recognise treatment of contingencies. Finally, the chart of accounts
and a well-designed cost code connect every posting to the WBS, the CBS and the control account — the bridge
from this financial foundation to cost control (Domain 5) and earned value (Domain 6).

**Cross-references.** Depreciation and IAS 16 → 2.4; IFRS 15 revenue recognition and its link to billing →
2.2, 7.5; cash-flow forecasting → 3.5; the cost-control cycle (commitment → accrual → actual) → 5.2; control
accounts and earned value → 6.1, 6.4; onerous contracts and contract types → 7.1–7.2; business process cycles
and reconciliation controls → Domain 11.

**AI across Domain 1 (recap).** AI is most valuable, and lowest-risk, in *coding and reconciliation* (KA 1.5)
and *accrual/cut-off proposal* (KA 1.3); it is genuinely useful but higher-judgement in *provisioning* (KA
1.4) and *disclosure drafting* (KA 1.2). In every case the same principle governs: **AI proposes, the
professional disposes** — the professional remains accountable for the entry, the estimate and the sign-off.

*Domain 1 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
