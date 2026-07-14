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

**Worked example 1.1.4b — a suspense account, opened and cleared.**

1. **Setup.** Meridian's trial balance disagrees: debits exceed credits by **USD 900**. A **suspense
   account** is opened with a 900 credit so period-end work can continue while the difference is traced.
   Investigation finds that a **USD 450 payment** was credited to Cash correctly but was *also credited* —
   instead of debited — to the expense account: a **reversal error**.
2. **Formula.** A reversal error disturbs the trial balance by **twice** the amount:
   `Trial balance difference = 2 × amount posted to the wrong side`.
3. **Substitution.** `2 × 450 = 900` — the single error explains the whole difference.
4. **Result.** Correction: `Dr Expense 900 / Cr Suspense 900`. The 900 debit removes the wrong-side 450
   credit *and* supplies the missing 450 debit; the suspense account returns to **nil**.
5. **Interpretation.** A suspense account is a workflow device, never a resting place — anything left in
   suspense at period-end is an unexplained number in the statements. And reversal errors always throw the
   trial balance out by **twice** the amount, a signature worth knowing when chasing differences (cross-ref
   the limits of the trial balance, 1.1.4).

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

**MCQ 1.1-E `[1.1.1 · Application]`** A business controls assets of USD 250,000 and owes liabilities of
USD 90,000. Its equity is:
- A. USD 90,000
- B. USD 160,000 ✅
- C. USD 250,000
- D. USD 340,000

*Rationale:* Rearranging `A = L + E` gives `E = 250,000 − 90,000 = 160,000` — the owners' residual claim.
A is the liabilities figure itself; C ignores the outside claims entirely; D adds liabilities to assets
instead of deducting them.

**MCQ 1.1-F `[1.1.4 · Analysis]`** A bank statement shows USD 86,500. Outstanding cheques total USD 5,000
and a deposit in transit is USD 2,500. If the differences are purely timing, the ledger cash balance the
reconciliation should agree to is:
- A. USD 79,000
- B. USD 84,000 ✅
- C. USD 89,000
- D. USD 94,000

*Rationale:* `Adjusted bank balance = 86,500 − 5,000 + 2,500 = 84,000`, which must equal the ledger if only
timing items exist. A subtracts the deposit in transit as well as the cheques; C adds the cheques instead of
deducting them; D adds both adjustments.

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

**Worked example 1.2.4 — Meridian's second month SOCE.**

1. **Setup.** Meridian opens Month 2 with equity of **USD 117,000** (Month 1's closing SOFP, 1.2.6). During
   Month 2 it earns a profit of **USD 14,000**, declares and pays a dividend of **USD 5,000**, and raises no
   new capital.
2. **Formula.** `Closing equity = Opening equity + Profit for the period − Dividends`.
3. **Substitution.** `117,000 + 14,000 − 5,000 = 126,000`.
4. **Result.** Closing equity **USD 126,000**, reconciled line by line:

   | SOCE line | USD |
   |---|---:|
   | Opening equity | 117,000 |
   | Profit for the period | 14,000 |
   | Dividends | (5,000) |
   | **Closing equity** | **126,000** |

5. **Interpretation.** The SOCE is where performance (SOPL) and distributions meet the balance sheet. A
   dividend is **not** an expense — it never touches profit; it reduces equity directly, a return of
   resources to owners (the entity concept again, 1.1.1).

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

**MCQ 1.2-E `[1.2.3 · Application]`** A company reports profit of USD 25,000, depreciation of USD 4,000, an
increase in receivables of USD 9,000 and an increase in payables of USD 3,000. Under the indirect method,
net operating cash is:
- A. USD 17,000
- B. USD 19,000
- C. USD 23,000 ✅
- D. USD 41,000

*Rationale:* `25,000 + 4,000 (non-cash) − 9,000 (receivables up) + 3,000 (payables up) = 23,000`. A deducts
the payables increase instead of adding it; B omits the depreciation add-back; D adds the receivables
increase instead of deducting it.

**MCQ 1.2-F `[1.2.5 · Recall]`** A controls professional wants to know *how* revenue was recognised on a
long-term contract. That accounting policy is set out in:
- A. The face of the statement of profit or loss.
- B. The statement of changes in equity.
- C. The notes to the financial statements. ✅
- D. The statement of cash flows.

*Rationale:* The notes disclose the accounting policies applied and the detail behind the face numbers, and
under IFRS they are an integral part of the statements. The SOPL shows the revenue *figure*, not the policy;
the SOCE and the cash-flow statement carry equity movements and cash, not policies.

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

**Worked example 1.3.5 — a missed accrual distorts CPI.**

1. **Setup.** Month-end cost report for a control account. Earned value **EV = USD 2,200,000**.
   Invoiced-to-date cost is **USD 1,850,000**; a subcontractor has performed a further **USD 240,000** of
   work not yet invoiced.
2. **Formula.** `CPI = EV / AC`, where the true `AC` = invoiced cost **plus** the accrual for work performed
   but not yet invoiced.
3. **Substitution.** True AC = `1,850,000 + 240,000 = 2,090,000`, so `CPI = 2,200,000 / 2,090,000 ≈ 1.05`.
   With the accrual missed, `AC = 1,850,000` and `CPI = 2,200,000 / 1,850,000 ≈ 1.19` — flattering by
   fourteen points.
4. **Result.** Report **CPI ≈ 1.05** on the accrued basis. Next month, when the USD 240,000 invoice lands,
   the un-accrued version would show a false "overrun" spike of exactly that amount.
5. **Interpretation.** The cut-off is not bookkeeping hygiene — it is the difference between a performance
   index that means something and one that whipsaws with invoice timing; earned value (Domain 6) is only as
   honest as the accruals beneath its `AC`. Cross-ref the contractor's accrual pack in 1.5.6 and `CPI` in
   Domain 6 (KA 6.2).

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

**MCQ 1.3-E `[1.3.3 · Application]`** A firm pays USD 12,000 at the start of a month for a twelve-month
insurance policy. Three months later, the prepaid insurance asset remaining on the SOFP is:
- A. USD 0
- B. USD 3,000
- C. USD 9,000 ✅
- D. USD 12,000

*Rationale:* The monthly charge is `12,000 / 12 = 1,000`; after three months `3,000` has been expensed,
leaving a prepaid asset of `12,000 − 3,000 = 9,000`. B is the *expense* recognised to date, not the asset;
A expenses the whole policy immediately; D releases nothing despite three months' cover being consumed.

**MCQ 1.3-F `[1.3.2 · Recall]`** The matching concept requires that an expense be recognised:
- A. In the period the related cash is paid.
- B. In the same period as the income it helps to earn. ✅
- C. In the period the supplier's invoice is received.
- D. In whichever period gives the smoothest profit trend.

*Rationale:* Matching ties expense recognition to the income the cost helps generate, which is what makes
profit a measure of performance rather than cash timing. A is the cash basis; C confuses recognition with
document arrival; D describes profit smoothing, which the concept exists to prevent, not permit.

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

**MCQ 1.4-F `[1.4.6 · Application]`** A discounted provision is carried at USD 100,000 at the start of the
year; the discount rate is 6 % and the estimate is unchanged. Its carrying amount at the year-end is:
- A. USD 6,000
- B. USD 94,000
- C. USD 100,000
- D. USD 106,000 ✅

*Rationale:* The discount unwinds as settlement nears: `100,000 × 6 % = 6,000` is charged as a finance cost
and added to the provision, giving `100,000 + 6,000 = 106,000`. A is the finance cost alone, not the
carrying amount; B applies the unwind with the wrong sign; C ignores the unwind entirely.

**MCQ 1.4-G `[1.4.1 · Analysis]`** Which of the following is an **accrual** rather than a provision?
- A. A warranty obligation on units already sold, based on expected failure rates.
- B. Electricity consumed last month for which no invoice has yet arrived. ✅
- C. A probable legal claim whose settlement amount is uncertain.
- D. A legal obligation to decommission a facility in several years' time.

*Rationale:* The electricity has been received and its amount and timing are readily estimable — a classic
accrual. A, C and D are all liabilities of uncertain timing or amount, so they fall under IAS 37 as
provisions and must pass its recognition and measurement tests.

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

**From payment certificate to the ledgers.** The document behind most of that subcontract accrual is the
**interim payment certificate**, and its structure drives a posting flow every contractor controller
reconciles monthly. A certificate is built as `gross value of work done to date − retention − previously
certified = net payable now`: with gross work done to date of **USD 500,000**, retention at 5 %
(`500,000 × 5 % = 25,000`) and **USD 380,000** previously certified net, this certificate is
`500,000 − 25,000 − 380,000 = 95,000`. One document then produces four ledger effects. The *movement* in
gross work done — this period's certified work — drives **cost**: Dr project cost/WIP. The retention
movement sits as a **retention receivable/payable** depending on which side of the certificate the entity
stands (Domain 7, KA 7.2.4). The net certificate becomes the **payables** entry the subcontractor's invoice
will clear. And any certified-but-not-invoiced balance is exactly the GRNI accrual of this mini-case. The
reconciliation discipline follows: the certificate register, the retention ledger and the payables ledger
must agree with the cost ledger every period (KA 1.5.2; Domain 5, KA 5.2.3) — a certificate posted to cost
but missing from payables (or vice versa) is exactly the kind of one-sided error the trial balance cannot
catch (1.1.4).

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

**MCQ 1.5-D `[1.5.3 · Recall]`** Which question does a **CBS** view of project cost answer that a WBS view
does not?
- A. How much has been spent on the Foundations work package?
- B. How much has been spent on subcontract labour across the whole project? ✅
- C. Which organisational unit is accountable for a piece of scope?
- D. Whether total debits equal total credits.

*Rationale:* The CBS decomposes cost by *element/type*, so it answers cross-project questions such as total
subcontract spend. A is a WBS (scope) view; C is the OBS; D is what the trial balance tests, not a coding
structure.

**MCQ 1.5-E `[1.5.2 · Application]`** At month-end a subcontractor has performed USD 150,000 of work on a
package, but invoices on file total only USD 110,000. For project cost-to-date to reflect work performed,
the accrual to raise is:
- A. USD 40,000 ✅
- B. USD 110,000
- C. USD 150,000
- D. USD 260,000

*Rationale:* The accrual covers the performed-but-uninvoiced gap: `150,000 − 110,000 = 40,000`. B is the
invoiced cost already recorded; C would double-count the invoiced portion; D adds the two figures instead of
taking the difference.

### Self-check — KA 1.5

1. Give one thing a WBS view of cost tells you that a CBS view does not, and vice versa. *(WBS: spend by
   deliverable/work package, e.g. Foundations; CBS: spend by cost type, e.g. total subcontract, across the
   project.)*
2. Why is coding correctly *at source* better than reclassifying later? *(Mis-coding propagates into every
   downstream report and distorts project cost/CPI before it is caught; source discipline prevents it.)*
3. How does a control account link this domain to earned value? *(It is the WBS×OBS point where budget, cost
   and schedule integrate — the level at which EV is measured in Domain 6.)*

---

## Advanced topics — Domain 1

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 1.A.1 — Multi-currency project accounting

**The working-level rules.** An entity keeps its books in its **functional currency** — the currency of the
primary economic environment in which it operates — and may present its statements in a different
**presentation currency**. A foreign-currency transaction is recorded at the exchange rate on the
**transaction date**; at each period-end, **monetary balances** (cash, receivables, payables, accruals) are
**retranslated at the closing rate**, with the difference taken to profit or loss; non-monetary items
(equipment, prepayments) remain at their historic rate.

**Worked example 1.A.1 — retranslating a foreign-currency receivable.**

1. **Setup.** Meridian (functional currency USD) invoices a Saudi client **SAR 375,000** when USD 1 =
   SAR 3.75. At period-end the invoice is unpaid and the closing rate is USD 1 = SAR 3.80.
2. **Formula.** Record at the transaction-date rate; retranslate the monetary receivable at the closing
   rate; take the difference to profit or loss.
3. **Substitution.** On invoicing: `375,000 / 3.75 = 100,000` → `Dr Accounts receivable 100,000 / Cr
   Service revenue 100,000`. At the close: `375,000 / 3.80 = 98,684`.
4. **Result.** A retranslation loss of **USD 1,316**: `Dr Foreign exchange loss 1,316 / Cr Accounts
   receivable 1,316`.
5. **Interpretation.** Nothing about the work or the client changed — the movement is currency, not
   performance.

**The controls angle.** On an international project, period-on-period cost movements mix genuine variance
with FX movement, and the reconciliation discipline of 1.5.2 must separate them: restate the comparison at
a constant rate (or isolate the retranslation line) before attributing cause. A variance analysis that
ignores FX **misattributes cause** — a "cost increase" that is wholly a rate movement gets chased as a
productivity or price problem, exactly the attribution failure variance analysis exists to prevent
(cross-ref Domain 4, KA 4.2).

**The currency of control.** The accounting above leaves a controls decision open: which currency the
budget, EAC and cost report are *managed* in. The rule that works: control in the currency of the
**contract's dominant cash flows**, report in the group's presentation currency, and never mix the two
inside one number. The exposure that matters to the project — not just to the accounts — is that where
revenue and cost currencies differ, margin moves with the rate. A USD-priced contract with
`EAC = 10,000,000` of which **USD 4,000,000 is EUR-denominated cost**: if the EUR strengthens **8 %**, that
cost rises `4,000,000 × 8 % = 320,000` in USD terms — **3.2 % of EAC** (`320,000 / 10,000,000`), often more
than the contingency's headroom — with no change in scope, productivity or quantity. The controls response:
state each package's currency in the estimate basis (Domain 3, KA 3.2.3); forecast in the currency of spend
and convert at forecast rates; log rate movements as their own EAC bridge line (Domain 3, KA 3.4.3 — never
blended into "escalation"); and flag material mismatches for treasury hedging — a decision that belongs to
treasury, not the project (cross-ref Domain 7, Advanced 7.A.4 for the contractual side).

### Advanced 1.A.2 — Intercompany and joint-venture cost flows

**Recharges and the elimination principle.** Large projects are rarely delivered by a single legal entity:
a parent incurs cost (staff, insurance, licences) and **recharges** it to the project entity. Each entity
records its own side of the flow — but **group accounts eliminate intra-group balances and profit**: a sale
from one group member to another is not income to the group, so any margin in a recharge rate is
**unrealised at group level** until earned from an outside party.

**Worked example 1.A.2 — a recharge with embedded margin.**

1. **Setup.** Meridian (parent) seconds engineers to its project subsidiary, incurring **USD 100,000** of
   payroll, and recharges at **cost + 10 %**.
2. **Formula.** `Recharge = cost × 1.10`; each entity records its own side; consolidation eliminates the
   intra-group pair and the unrealised margin.
3. **Substitution.** `100,000 × 1.10 = 110,000`. Parent: `Dr Intercompany receivable 110,000 / Cr Recharge
   income 110,000` (having borne 100,000 of payroll cost). Subsidiary: `Dr Project cost 110,000 / Cr
   Intercompany payable 110,000`.
4. **Result.** The subsidiary's project cost is **110,000**; the group's cost is **100,000** — the 10,000
   margin is eliminated on consolidation.
5. **Interpretation.** "Project cost" is entity-dependent: each ledger is right for its own entity (the
   entity concept, 1.1.1), but only one basis matches a group-level budget.

**Joint ventures.** Where the project vehicle is a JV, the operator incurs cost and bills each venturer its
**proportionate share** — a 60/40 venture splits a USD 1,000,000 cost as 600,000/400,000 — and each
venturer records only its share.

**The controls trap.** Two failures recur: **double-counting** recharged cost (once in the parent's ledger,
again when the recharge lands in the project entity — reconciliation per 1.5.2 must net the pair, or
cost-to-date is overstated) and **missing the margin embedded in intercompany rates** (comparing an
at-cost-plus project ledger to a baseline set at group cost manufactures an apparent overrun equal to the
margin). The basis of every recharge rate belongs in the coding rules so comparisons are like-for-like.

### Advanced 1.A.3 — Accounting policy versus estimate — and why restatement differs

Under **IAS 8 (accounting policies, changes in accounting estimates and errors)** three superficially
similar changes receive three different treatments:

- A change in **accounting policy** — the principles and bases applied — is made **retrospectively**:
  comparatives are **restated** as if the new policy had always applied, so like is compared with like.
- A change in **accounting estimate** — a useful life (1.3.4), a provision amount (1.4.6), a total contract
  cost — applies **prospectively**: it changes the current and future periods only, never the past.
- An **error** — a misapplication of policy, a mistake in the numbers — is corrected by **restatement** of
  the prior-period figures.

The logic of the asymmetry matters more than the labels. An estimate made honestly on the information then
available was not *wrong*; new information produces a *new estimate*, not a corrected old one. A policy
change or an error, by contrast, means the prior figures were prepared on a basis now abandoned, or were
simply misstated — so the past is rewritten to restore comparability.

The controls professional's forecasts feed **estimates**. A revised cost-to-complete changes the
total-contract-cost estimate that drives percentage-of-completion revenue, and the effect is absorbed
through the current period's margin — the prospective catch-up of 2.2.6 — never by restating last year. A
remeasured provision moves through the current period (1.4.6); a revised useful life changes *future*
depreciation only. That is why estimate revisions flow through current and future periods without rewriting
the past — why a board demanding that prior margins be "restated" after a forecast deterioration is making a
category error, and, conversely, why a genuine mis-posting is an error to correct visibly, not an estimate
revision to absorb quietly.

### Advanced 1.A.4 — Period-end discipline at scale

**The close calendar.** At scale, the month-end close is run as a **controlled process**, not a monthly
scramble: a published **close calendar** sequences cut-off (1.3.5), the accrual pack (1.5.6),
reconciliations to independent sources (1.1.4, 1.5.2) and review/sign-off, each step with a named owner and
a deadline. Repeatability is the control: the same steps, in the same order, evidenced the same way, so an
omission is visible as a missed step rather than discovered later as a misstatement.

**Materiality-tiered accrual thresholds.** Not every uninvoiced cost deserves the same effort. A tiered
policy accrues large items individually from evidence and estimates the small tail in aggregate, so effort
concentrates where misstatement matters.

**Worked example 1.A.4 — a tiered accrual pack.**

1. **Setup.** Close policy: items **≥ USD 25,000** are individually evidenced and accrued; below that, an
   aggregate run-rate estimate. This month: **14** large items totalling **USD 1,340,000**; the small tail
   is estimated at **USD 90,000** from a three-month run rate.
2. **Formula.** `Total accrual = Σ individually evidenced items + aggregate estimate`.
3. **Substitution.** `1,340,000 + 90,000 = 1,430,000`.
4. **Result.** **USD 1,430,000**: `Dr Project costs 1,430,000 / Cr Accrued liabilities 1,430,000`, coded to
   the relevant control accounts (1.5.4).
5. **Interpretation.** About 94 % of the accrual value sits in 14 evidenced items; the tiering buys accuracy
   where it matters and speed where it does not.

**What a 3-day close needs that a 10-day close does not.** Compression comes from removing manual work, not
from working faster: automated **coding and matching** of invoices and goods receipts (cross-ref 13.5.4),
standing accrual templates driven by open commitments, earlier sub-cut-offs for low-risk feeds, and
exception-based review against thresholds instead of line-by-line checking. The prize is a controls
dividend: the fresher the close, the earlier the true cost position reaches the forecast (1.3.5).

---

## Case study — Domain 1: a groundworks contractor's month-end close (construction)

### Background

*Terrafirm Groundworks* is a fictional groundworks subcontractor — earthworks, drainage and site
establishment packages delivered for main contractors on commercial developments. Its work is certified
monthly by the main contractor's surveyor, its subcontractors invoice when their own paperwork allows, and
its plant is hired in from external suppliers whose invoices routinely arrive weeks after the machines have
left site. In other words, it lives with exactly the gap this domain keeps returning to: the gap between
when work is *performed* and when documents *arrive*.

This case study follows Terrafirm's finance-and-controls team through one month-end close. Nothing in it is
exotic; that is the point. The close is where Domain 1's foundations stop being theory and start doing real
work: the accrual basis and matching concept (KA 1.3) decide what the month's revenue and cost actually are;
cost coding and control accounts (KA 1.5) decide *where* those costs land; and reconciliation (1.1.4, 1.5.2)
is the control that catches what the trial balance alone never could. Alongside the close that was actually
run, the case shows the close that *would* have been reported if the accrual pass had been skipped — because
the difference between the two is the clearest demonstration in this domain of why any of it matters.

### The raw position

At the close of the month, before any period-end adjustments, the ledger shows:

| Raw ledger position (before adjustments) | USD |
|---|---:|
| Certified valuations invoiced this month (revenue) | 780,000 |
| Supplier and subcontractor invoices received (costs) | 520,000 |
| **Naive profit for the month** | **260,000** |
| **Naive margin** | **33.3 %** |

This is the "documents on file" view of the month: revenue is what has been certified and invoiced, cost is
what suppliers have got around to invoicing. It is arithmetically sound — the trial balance behind it
balances — and, as 1.1.4 warned, that proves nothing about whether it is *right*. A 33.3 % margin on a
groundworks package would be remarkable; the controls team's first instinct is not celebration but
suspicion, and the accrual pass tells them why.

### The accrual pass (KA 1.3)

The cost engineer and the accountant work through the month's cut-off together, applying the accrual basis
(1.3.1): recognition follows the **economic event** — work performed, plant used — not the arrival of a
certificate or an invoice. Three adjustments emerge:

| # | Adjustment | Type (1.3.3) | USD | Journal entry |
|---|---|---|---:|---|
| 1 | Work performed but not yet certified/billed | Accrued income | 60,000 | `Dr Accrued income / Cr Revenue 60,000` |
| 2 | Subcontractor work performed, not yet invoiced | Accrued expense | 95,000 | `Dr Subcontract cost / Cr Accrued liabilities 95,000` |
| 3 | Plant hire used on site, invoice missing | Accrued expense | 22,000 | `Dr Plant cost / Cr Accrued liabilities 22,000` |

Adjustment 1 is the revenue-side mirror of the cost accruals: earthworks completed in the last days of the
month sit outside this month's certificate, but the work has been performed and the entitlement earned, so
accrued income is recognised (the "earned, not yet billed" quadrant of the 1.3.3 matrix). Adjustments 2 and
3 are the classic contractor's cut-off items — the paperwork is behind the shovels, exactly as in the
sector mini-case at 1.5.6, and the accrual is what keeps cost-to-date honest.

The adjusted position:

- **Adjusted revenue** = 780,000 + 60,000 = **USD 840,000**.
- **Cost accruals raised** = 95,000 + 22,000 = **USD 117,000**.
- **Adjusted cost** = 520,000 + 117,000 = **USD 637,000**.
- **Adjusted profit** = 840,000 − 637,000 = **USD 203,000**.
- **Adjusted margin** = 203,000 / 840,000 = **24.2 %**.

A 24.2 % margin is a number the team can defend: it reflects the work performed and the resources consumed
in earning it, matched into the same period (1.3.2), regardless of which documents happen to have arrived.

### What skipping the accruals would have said

1. **Setup.** Compare the naive month (no accrual pass) with the adjusted month, and quantify what the
   skipped close would have told the business.
2. **Formula.** `Overstatement = naive profit − adjusted profit`; margin distortion = naive margin −
   adjusted margin.
3. **Substitution.** `Overstatement = 260,000 − 203,000 = 57,000`; margin distortion = `33.3 % − 24.2 %`
   ≈ 9 percentage points.
4. **Result.** The naive close overstates the month's true profit by **USD 57,000** and flatters the margin
   by nearly **9 points** — and the error is not permanent, only *borrowed*: next month, when the missing
   subcontractor and plant invoices land, the same 117,000 hits cost with no matching work, and the reported
   margin swings violently the other way.
5. **Interpretation.** Matching (1.3.2) is not pedantry; it is the difference between a real margin and an
   artefact of invoice timing. A board shown 33.3 % this month and something far below trend next month
   would chase a "deterioration" that never happened — the performance was 24.2 % all along. The earned-value
   echo makes the stakes concrete: the 117,000 of unaccrued cost would also have understated `AC` (actual
   cost), flattering `CPI` and corrupting the forecast built on it, exactly the failure mode described at
   1.3.5 and 1.5.6 and developed in Domain 6. The accrual pass is one month-end discipline protecting two
   sets of numbers at once — the financial result and the earned-value result.

### The coding-and-reconciliation pass (KA 1.5)

Getting the *total* right is only half the close. The adjusted cost of USD 637,000 must also sit in the
right places: each posting coded at source to its project, work package and cost element (1.5.5), rolling
up into the control accounts (1.5.4) against which the project is actually managed. Terrafirm's month
resolves into three control-account totals:

| Control account | USD |
|---|---:|
| Earthworks | 302,000 |
| Drainage | 214,000 |
| Site establishment | 121,000 |
| **Total (ties to adjusted cost)** | **637,000** |

Reconciling the project cost ledger back to the general ledger — the tie-to-independent-source discipline of
1.5.2 — surfaces one exception: a cost of **USD 18,000** coded to the right account class but the **wrong
project** — the classic 1.5.2 pitfall. The trial balance never flinched, and the statutory accounts would
have been perfectly correct, because the cost is the right *type*; but one job was carrying another job's
cost, its margin understated and the other's flattered, and any `CPI` derived from either would have been
wrong. The item is **re-coded at source** before any report is cut — correction at the point of entry, not
reclassification after the reports have propagated the error downstream.

The bank reconciliation (in the 1.1.4 pattern) closes cleanly: the differences between the ledger cash
balance and the bank statement are timing items only — cheques written but not yet presented, and a receipt
banked but not yet credited — each one listed, explained and expected to clear. No unexplained differences
means no omitted or duplicated cash entries hiding behind a balanced trial balance.

### The close, reported

| Month-end close — reported | USD |
|---|---:|
| Revenue (certified 780,000 + accrued income 60,000) | 840,000 |
| Cost (invoiced 520,000 + accruals 117,000) | 637,000 |
| **Profit for the month** | **203,000** |
| **Margin** | **24.2 %** |
| Cost accruals raised | 117,000 |
| Accrued income raised | 60,000 |
| Miscodes corrected (re-coded at source) | 18,000 |

The narrative that accompanies the pack is two sentences, in the decision-ready style of Domain 4: *The
month closed at a 24.2 % margin on USD 840,000 of revenue, stated on an accrual basis with USD 117,000 of
uninvoiced subcontract and plant cost recognised; the margin is genuine and comparable with prior months,
not a timing artefact. One USD 18,000 cross-project miscode was identified and corrected at source before
reporting; no provisions were required; recommend no action beyond continued monitoring of subcontractor
invoicing lag.*

### What the credential expects

Every knowledge area in this domain did a shift in this close, and a candidate should be able to name each
one at work. The **double-entry model and the ledger (KA 1.1)** carried every posting — each accrual a
balanced entry, the trial balance arithmetically tight, and the bank reconciliation covering the blind spots
the trial balance cannot see. The **accrual basis, matching and cut-off (KA 1.3)** converted a
documents-on-file position into a performance measure: recognition followed the economic event, and the
57,000 the naive view would have overstated is the measurable cost of getting that wrong. **Provisions
awareness (KA 1.4)** appears in this close precisely as an *absence*: the team reviewed the position and
concluded that no obligation of uncertain timing or amount — no probable claim, no onerous contract —
required recognition this month, and recording that conclusion is itself a control; an unconsidered nil and
a considered nil look identical on the face of the accounts and are entirely different in substance.
**Coding, control accounts and reconciliation (KA 1.5)** put the right total in the right places and caught
the 18,000 that classification alone would never have caught. And the whole exercise feeds the **statements
of KA 1.2**: the accruals become current assets and current liabilities on the statement of financial
position, the adjusted revenue and cost flow through profit or loss into equity, and the profit-to-cash gap
the accruals create is exactly what the cash-flow statement will explain.

One sentence on AI completes the picture: accrual-proposal tools scanning goods-received-not-invoiced
reports and auto-coding assistants mapping invoice narratives to cost codes would accelerate every step of
this close, with the professional owning the service dates, the coding rules and the sign-off (13.5.4) —
**AI proposes, the professional disposes.**

---

## Case study B — Domain 1: capitalising the canning line (manufacturing)

### Background

*Keldan Foods* is a fictional mid-sized food manufacturer. Its capital programme this year is the
installation of a second high-speed canning line — **Line 2** — in an existing plant: a filler-seamer
bought from a European vendor, new conveyors and guarding, a control system, and the civil works to carry
it all. The project is run by the engineering function, but its quarter-end lands on the controls
professional's desk, because every decision the quarter forces is an accounting decision wearing overalls:
which costs become an **asset** and which become this quarter's **expense** (the capitalise-vs-expense
boundary of IAS 16, developed at Domain 2, KA 2.4.2, resting on the matching concept of KA 1.3); how the
capitalised total is **componentised** and what depreciation that sets (1.3.4); what happens when an
existing asset's **useful life is revised** (Advanced 1.A.3); what the same vendor announcement does to a
store of **spares** (IAS 2, cross-ref 2.4.1); and how the quarter's **accrual pack** keeps all of it in the
right period (1.3.5, 1.5.6). Where the first Domain 1 case study was a contractor's revenue-and-cost close,
this one is the mirror image: a quarter in which almost nothing is revenue and every judgement is about
*which side of the balance sheet* a cost belongs on.

### Capitalise or expense — drawing the boundary (KA 1.3; IAS 16)

The project ledger holds seven cost lines for Line 2 at quarter-end. Under IAS 16 an item of plant is
capitalised at its purchase price **plus the costs directly attributable to bringing it to the location and
condition needed to operate** — and nothing else. The team's boundary call, line by line:

| Cost line | USD | Decision | Why |
|---|---:|---|---|
| Filler-seamer purchase price | 1,800,000 | Capitalise | The asset itself |
| Freight and transit insurance | 60,000 | Capitalise | Getting it to location |
| Installation labour (riggers + own engineers) | 140,000 | Capitalise | Getting it to working condition |
| Foundations and civil works | 100,000 | Capitalise | Site preparation, directly attributable |
| Commissioning trial runs | 40,000 | Capitalise | Testing that the asset functions |
| Operator retraining for the line crew | 35,000 | Expense | Trained staff are not a controlled asset |
| Relocating the old conveyor to clear the bay | 25,000 | Expense | Reorganisation, not attributable to Line 2 |

1. **Setup.** Sum the lines that pass the directly-attributable test; expense the rest in the quarter.
2. **Formula.** `Capitalised cost = purchase price + Σ directly attributable costs`.
3. **Substitution.** `1,800,000 + 60,000 + 140,000 + 100,000 + 40,000 = 2,140,000`; expensed:
   `35,000 + 25,000 = 60,000`.
4. **Result.** Line 2 is capitalised at **USD 2,140,000**; **USD 60,000** hits this quarter's profit.
5. **Interpretation.** The two rejected lines are the ones candidates (and project managers) most want to
   capitalise. Training fails because the entity does not *control* the future benefit — the trained
   operator can resign; relocation of existing kit is a cost of rearranging the factory, not of readying
   the new asset. Note what mis-capitalising the 60,000 would do: it would not save the cost, only smear it
   forward as roughly **USD 500 a month** of extra depreciation for a decade — a small permanent lie in
   every future period in exchange for one flattered quarter. The boundary is matching (1.3.2) applied to
   the balance sheet.

### Componentisation — the depreciation the boundary sets (KA 1.3.4)

The 2,140,000 is not one asset for depreciation purposes. Its significant parts have different lives, so
under IAS 16 they are depreciated **separately** (componentisation — worked in the same pattern at 2.4.2):

| Component | Cost (USD) | Life | Annual depreciation (USD) |
|---|---:|---:|---:|
| Filler-seamer unit | 1,200,000 | 8 years | 150,000 |
| Conveyors and guarding | 500,000 | 10 years | 50,000 |
| Control system (PLC and drives) | 340,000 | 5 years | 68,000 |
| Foundations and civils | 100,000 | 20 years | 5,000 |
| **Total** | **2,140,000** | | **273,000** |

The component costs re-sum to the capitalised total ✓, and the annual charge is **USD 273,000** — **USD
22,750 a month** once the line is available for use, which it becomes in the final month of the quarter, so
this quarter bears one month: **22,750**. A single blended "10-year plant life" would have charged only
`2,140,000 / 10 = 214,000` a year — under-depreciating by **59,000** annually and, worse, hiding the fact
that the control system will need replacing at year 5, an event the componentised schedule *plans for* and
the blended one discovers as a surprise. Depreciation is an estimate built from other estimates, and the
component structure is what makes each estimate visible and reviewable.

### A life revised — the estimate change (Advanced 1.A.3)

Mid-quarter, the control-system vendor announces **end of support** for the PLC platform running the
plant's existing **Line 1**. Line 1's control system cost **USD 300,000** with a six-year life
(`300,000 / 6 = 50,000` a year); it is exactly three years old, so accumulated depreciation is **150,000**
and the carrying amount **150,000**, with three years originally remaining. Engineering now judges it will
be replaced in **two** years, not three.

1. **Setup.** Carrying amount **USD 150,000**; remaining useful life revised from 3 years to **2 years**
   at the start of the quarter.
2. **Formula.** A revised life is a **change in accounting estimate** (IAS 8, per 1.A.3): apply
   **prospectively** — `new annual depreciation = carrying amount / revised remaining life`. No restatement.
3. **Substitution.** `150,000 / 2 = 75,000` a year; quarterly `75,000 / 4 = 18,750`, against `12,500`
   under the old life.
4. **Result.** This quarter's Line 1 charge is **USD 18,750** — an uplift of **USD 6,250** per quarter for
   the remaining two years.
5. **Interpretation.** Nothing was *wrong* with the old estimate: on the information then available, six
   years was honest. New information produces a **new estimate**, absorbed in current and future periods —
   never a rewriting of the past (1.A.3). A board member demanding prior quarters be "corrected" for the
   shorter life is making the category error that section exists to prevent; equally, a team quietly using
   the revision to explain away an unrelated overspend is misusing it. The estimate changes; the audit
   trail says why.

### The stranded spares — cost or NRV (IAS 2)

The same vendor announcement strands the stores: Keldan holds **USD 90,000** (at cost) of spares specific
to the end-of-life PLC platform. Under IAS 2 inventories are carried at the **lower of cost and net
realisable value**, and these spares now fail the test: a broker will pay an estimated **USD 30,000**, with
**USD 6,000** of costs to sell (testing, certification, carriage). `NRV = 30,000 − 6,000 = 24,000`, which
is below cost, so the spares are written down: `Dr Inventory write-down expense 66,000 / Cr Inventories
66,000` (`90,000 − 24,000`). The write-down belongs in **this** quarter — the period the obsolescence
became known — not the period the spares are eventually sold; and it is the third consequence of a single
economic event, which is the case study's quiet lesson: one vendor letter moved a depreciation estimate, an
inventory value and (next section) an accrual reviewer's checklist. Events, not documents, drive the books.

### The quarter's accrual pack (KAs 1.3.5, 1.5.6)

Cut-off closes the quarter. Work performed for Line 2 but not yet invoiced is accrued in the 1.5.6 pattern
— and here the pack has a twist worth examining: most of it is **capital**, not expense.

| # | Accrual | USD | Treatment |
|---|---|---:|---|
| 1 | Rigging contractor — final installation milestone, uninvoiced | 48,000 | Capital: `Dr Line 2 asset / Cr Accrued liabilities` |
| 2 | Commissioning consultants, uninvoiced | 12,000 | Capital: `Dr Line 2 asset / Cr Accrued liabilities` |
| 3 | Trial-run utilities, estimated | 8,000 | Expense |
| 4 | Small-item tail (run-rate estimate) | 7,000 | Expense |
| | **Total accruals** | **75,000** | |

Items 1 and 2 (**60,000**) are already inside the 2,140,000 capitalised above — the 140,000 of installation
labour includes the accrued 48,000, and the 40,000 of commissioning includes the accrued 12,000. Skipping
them would not have flattered profit (they are balance-sheet costs); it would have **understated the
asset**, and with it every future period's depreciation — proof that cut-off discipline protects both sides
of the equation, not just the P&L. The quarter's income-statement effect assembles as:

| Quarter charge to profit | USD |
|---|---:|
| Training and relocation expensed | 60,000 |
| Line 2 depreciation (one month) | 22,750 |
| Line 1 depreciation (revised life) | 18,750 |
| Spares write-down to NRV | 66,000 |
| Accrued utilities and small-item tail | 15,000 |
| **Total** | **182,500** |

A naive close — everything capitalised, life unrevised, spares at cost — would have charged
`22,750 + 12,500 + 15,000 = ` **USD 50,250**: a difference of **USD 132,250**, none of it avoided, all of
it deferred into future periods where it would surface as unexplained depreciation and a disposal loss.

### What the credential expects

A candidate should be able to walk this quarter end-to-end and name the principle behind each call. The
**capitalise-vs-expense boundary** is the matching concept (KA 1.3.2) applied through IAS 16's
directly-attributable test — installation labour in, training and relocation out — and the candidate should
be able to defend each line, not just total them. **Componentisation** (1.3.4; 2.4.2) turns one capitalised
figure into a depreciation schedule with reviewable estimates, and the **life revision** exercises Advanced
1.A.3: a change in estimate, applied prospectively, never a restatement. The **NRV write-down** (IAS 2;
2.4.1) shows the same event-driven recognition discipline reaching inventory, and the **accrual pack**
(1.3.5, 1.5.6) shows cut-off protecting the balance sheet as well as the P&L — an unaccrued capital cost
understates the asset and every future depreciation charge built on it. Every entry posted was a balanced
double entry through the coding structure of KA 1.5, and the quarter's 182,500 charge reconciles to its
components exactly. On AI: invoice-classification assistants can propose the capitalise-vs-expense split
from purchase-order text and flag spares whose parent asset has been end-of-lifed — genuinely useful — but
the directly-attributable judgement, the revised life and the NRV estimate are professional calls to be
evidenced and signed (13.5.4): **AI proposes, the professional disposes.**

---

## Executive perspective — Domain 1

**What the executive must hold onto.** Two ideas in this domain cannot be delegated. First, **profit is a
judgement-laden construct**: the month's margin depends on accruals, cut-off calls and provision decisions
someone made — and **profit is not cash** (1.2.6). Second, the **ledger's integrity is the foundation of
every number the board sees**: double-entry, the trial balance, reconciliation and cost coding (KA 1.5) are
not clerical hygiene but the reason a reported margin can be believed at all — a single miscode corrupts two
projects' margins at once.

**Six questions to ask from the chair.**

1. What accruals are in this month's number, and what would the margin be without them?
2. When was the cost ledger last reconciled to the project reports, and what unexplained differences remain?
3. Were provisions and onerous contracts actively considered this period, or is the nil an unconsidered nil?
4. What is the gap between this month's profit and its cash movement, and what explains it?
5. Who signed off the cut-off — and how much of the margin would move if service dates slipped a week?
6. Are miscodes corrected at source before reports are cut, or reclassified after the error has propagated?

**The traps at board level.**

- **Profit/cash conflation.** A profitable month is read as a funded one; the accrual basis guarantees the
  two diverge, and only the cash-flow statement (1.2.3) says by how much.
- **A balanced trial balance read as assurance.** Arithmetic balance cannot see omitted, duplicated or
  miscoded entries (1.1.4); only reconciliation — bank, ledger-to-report, code-to-WBS — closes those blind
  spots, and a board that never asks about reconciliations is trusting balance alone.
- **A margin that is a timing artefact.** A "good month" manufactured by unbooked subcontractor and plant
  accruals (1.3.5) reverses in the next period — the flattering number is borrowed, not earned.
- **The unconsidered nil.** No provision on the face of the accounts is read as no exposure; a considered
  nil and an unconsidered nil look identical and are entirely different in substance (1.4).

**What good looks like.** The month-end close runs to a fixed timetable and produces a documented accrual
pack — each accrual sourced to a delivery record or valuation, reviewed and reversed on schedule — with
bank and ledger reconciliations showing explained timing differences only. Cost codes map one-for-one to
the WBS and control accounts, so aggregation is automatic and miscodes are caught and corrected at source.
The pack the board receives carries a short decision-ready narrative that states the accrual content of the
margin, and the provision review is minuted even when — especially when — the answer is nil.

---

## Calculation exercises — Domain 1

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 1.1** — Kestrel Surveys Ltd begins trading on 1 June with seven transactions: (1) owners
inject USD 80,000 cash as share capital; (2) equipment is bought for USD 30,000 cash; (3) materials
costing USD 12,000 are bought on credit; (4) a client is invoiced USD 25,000 for completed survey work;
(5) wages of USD 9,000 are paid in cash; (6) the client pays USD 15,000 on account; (7) the supplier is
paid USD 7,000. Post each transaction, balance the ledger accounts, and prepare the trial balance at
30 June.

**Solution 1.1.** Step 1 — post: (1) `Dr Cash 80,000 / Cr Share capital 80,000`; (2) `Dr Equipment
30,000 / Cr Cash 30,000`; (3) `Dr Materials 12,000 / Cr Accounts payable 12,000`; (4) `Dr Accounts
receivable 25,000 / Cr Revenue 25,000`; (5) `Dr Wages expense 9,000 / Cr Cash 9,000`; (6) `Dr Cash
15,000 / Cr Accounts receivable 15,000`; (7) `Dr Accounts payable 7,000 / Cr Cash 7,000`. Step 2 —
balance: Cash `80,000 − 30,000 − 9,000 + 15,000 − 7,000 = 49,000 Dr`; receivables `25,000 − 15,000 =
10,000 Dr`; payables `12,000 − 7,000 = 5,000 Cr`. Step 3 — trial balance:

| Account | Dr | Cr |
|---|---|---|
| Cash | 49,000 | |
| Equipment | 30,000 | |
| Materials | 12,000 | |
| Accounts receivable | 10,000 | |
| Wages expense | 9,000 | |
| Accounts payable | | 5,000 |
| Share capital | | 80,000 |
| Revenue | | 25,000 |
| **Total** | **110,000** | **110,000** |

`Σ Dr = Σ Cr = 110,000` — the ledger balances.

**Exercise 1.2** — A contractor's draft profit for the year to 31 December is USD 40,000, before three
period-end items. (a) Subcontractor work of USD 6,000 was performed in December, but no invoice has
arrived. (b) On 1 March, USD 12,000 was paid for twelve months' insurance cover from that date, and the
whole amount was expensed. (c) Plant costing USD 60,000, with a USD 12,000 residual value and an
eight-year life, held throughout the year, has not been depreciated. Compute the adjusted profit,
showing each journal entry.

**Solution 1.2.** Step 1 — accrual: `Dr Subcontractor expense 6,000 / Cr Accrued liabilities 6,000`;
profit falls by 6,000. Step 2 — prepayment: cover runs 1 March to 28 February, so `12,000 × 10/12 =
10,000` belongs to this year and `12,000 − 10,000 = 2,000` is prepaid: `Dr Prepaid insurance 2,000 /
Cr Insurance expense 2,000`; profit rises by 2,000. Step 3 — depreciation: `(60,000 − 12,000) / 8 =
6,000`: `Dr Depreciation expense 6,000 / Cr Accumulated depreciation 6,000`; profit falls by 6,000.
Step 4 — adjusted profit: `40,000 − 6,000 + 2,000 − 6,000 =` **USD 30,000**.

**Exercise 1.3** — At 31 December, counsel assesses a defect claim expected to be settled in exactly
two years. The possible outcomes are: 50% probability of paying USD 100,000; 30% probability of paying
USD 35,000; 20% probability of paying nil. A 10% discount rate applies and the time value of money is
material. Compute the expected value, the initial provision, and the unwinding of the discount in each
of the two years.

**Solution 1.3.** Step 1 — expected value: `0.50 × 100,000 = 50,000`; `0.30 × 35,000 = 10,500`;
`0.20 × 0 = 0`; total `50,000 + 10,500 = 60,500`. Step 2 — discount two years at 10%: `1.10² = 1.21`;
`60,500 / 1.21 = 50,000`. Recognise `Dr Provision expense 50,000 / Cr Provision 50,000`. Step 3 —
year-1 unwinding: `50,000 × 10% = 5,000`, `Dr Finance cost 5,000 / Cr Provision 5,000`; carrying
amount `50,000 + 5,000 = 55,000`. Step 4 — year-2 unwinding: `55,000 × 10% = 5,500`; carrying amount
`55,000 + 5,500 = 60,500`, which equals the expected settlement — the schedule proves itself.

**Exercise 1.4** — A fixed-price contract has a price of USD 500,000. At the reporting date, costs
incurred to date are USD 320,000 and revenue recognised to date is USD 300,000. The estimate of cost
to complete has just been revised to USD 240,000. Test whether the contract is onerous and compute the
provision required under IAS 37.

**Solution 1.4.** Step 1 — total forecast cost: `320,000 + 240,000 = 560,000`. Step 2 — total forecast
loss: `500,000 − 560,000 = −60,000`; the contract is onerous. Step 3 — loss already in the P&L:
`300,000 − 320,000 = −20,000`. Step 4 — future loss: remaining revenue `500,000 − 300,000 = 200,000`
against cost to complete `240,000` gives `200,000 − 240,000 = −40,000`, so a provision of
**USD 40,000** is required: `Dr Onerous contract loss 40,000 / Cr Provision 40,000`. Check: loss booked
to date 20,000 plus provision 40,000 equals the 60,000 total forecast loss.

**Exercise 1.5** — A contractor's profit before tax is USD 75,000. The year's non-cash and
working-capital items are: depreciation USD 12,000; a loss on disposal of plant USD 3,000; receivables
increased by USD 18,000; inventory decreased by USD 6,000; payables increased by USD 9,000. Interest
paid was USD 4,000 and tax paid USD 11,000. Compute net cash from operating activities by the indirect
method.

**Solution 1.5.** Step 1 — add back non-cash items: `75,000 + 12,000 + 3,000 = 90,000`. Step 2 —
working-capital movements: receivables up absorbs cash, `90,000 − 18,000 = 72,000`; inventory down
releases cash, `72,000 + 6,000 = 78,000`; payables up releases cash, `78,000 + 9,000 = 87,000` — cash
generated from operations. Step 3 — deduct interest and tax paid: `87,000 − 4,000 − 11,000 = ` **USD
72,000** net cash from operating activities. Note the gap: profit 75,000 versus operating cash 72,000
— the 3,000 difference is the net effect of the add-backs and the working-capital absorption.

**Exercise 1.6** — At month-end, the general ledger shows total costs booked to project P-201 of
USD 1,240,000. The project cost ledger shows: direct labour USD 780,000; subcontract USD 310,000;
plant USD 95,000; and a suspense/unallocated code holding USD 40,000. Investigation also finds
USD 15,000 of P-201 costs posted in the GL to sister project P-202's cost centre. (a) Reconcile the
cost ledger to the GL and isolate the difference. (b) State the correcting actions for the suspense
balance and the misposting. (c) In one sentence: why must this reconciliation net to zero every period
before the cost report is issued?

**Solution 1.6.** Step 1 — (a) cost ledger total: `780,000 + 310,000 + 95,000 + 40,000 = 1,225,000`;
the GL shows `1,240,000`; difference `= 1,240,000 − 1,225,000 = 15,000` — exactly the P-201 cost
sitting in P-202's cost centre, so the ledgers reconcile once the misposting is identified:
`1,225,000 + 15,000 = 1,240,000`. Step 2 — (b) the misposting is corrected by a coded journal
transferring 15,000 from P-202 to P-201 (with narrative and approval — the journal discipline of
Domain 5, Advanced 5.A.3); the 40,000 suspense must be investigated and recoded to its true cost codes
before close — a suspense balance is a question, not a home (worked example 1.1.4b). Step 3 — (c)
because the cost report inherits the coding: an unreconciled difference means either the GL or the
cost ledger is wrong, and every downstream number — actual cost, CPI, the forecast — is built on
whichever one it is (1.5.2; Domain 5, KA 5.2.3).

---

## Practitioner's toolkit — Domain 1

Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.

### Toolkit 1.T.1 — Month-end close checklist

| Step | What | Owner | Done |
|---|---|---|---|
| 1 | **Cut-off** — confirm the period-end date; hold late postings to the next period (1.3.5) | Financial accountant | [ ] |
| 2 | **Accrual pack — subcontract/GRNI** — identify work performed but not invoiced from GRNI reports and site measures; raise accruals (1.5.6) | Cost engineer | [ ] |
| 3 | **Accrual pack — payroll** — accrue days worked but not yet paid from timesheets | Payroll / project accountant | [ ] |
| 4 | **Prepayment release** — release this period's share of insurances, licences and other prepaid cover (1.3.3) | Financial accountant | [ ] |
| 5 | **Depreciation run** — post the period's depreciation on plant and equipment (1.3.4) | Financial accountant | [ ] |
| 6 | **Provision review** — IAS 37 pass over claims, defects and onerous contracts; record a *considered* nil where nothing is required (1.4) | Project accountant | [ ] |
| 7 | **Bank reconciliation** — list, explain and age every difference between ledger cash and the bank statement (1.1.4) | Financial accountant | [ ] |
| 8 | **Cost-to-ledger reconciliation** — tie the project cost ledger back to the general ledger by control account (1.5.2, 1.5.4) | Cost engineer | [ ] |
| 9 | **Coding-exception clearance** — re-code miscoded items *at source* before any report is cut (1.5.2) | Cost engineer | [ ] |
| 10 | **Sign-off** — accountant and controls professional jointly sign the close pack | Project accountant + cost engineer | [ ] |

**Usage note.** This is the Terrafirm close (the Domain 1 case study) reduced to a standing sequence: cut-off
first, because every accrual depends on a fixed period boundary (1.3.5); the adjustment steps (2–6) before
the reconciliations (7–8), so what is reconciled is the *adjusted* position; and coding-exception clearance
(9) before sign-off, because a miscode corrected after reports are cut has already propagated (1.5.2). Name
individuals, not departments, in the owner column, and keep the sequence stable month to month so a skipped
step is visible. The pack this checklist produces is what keeps both the financial result and the
earned-value `AC` honest at the same time (1.5.6).

### Toolkit 1.T.2 — Accrual pack template

| Control account | Basis (GRNI / timesheet / assessment) | Service period | Amount (USD) | Reversal date | Approved by |
|---|---|---|---:|---|---|
| CA-Civils-Foundations — subcontract | GRNI: certified measure 240,000 less invoiced 180,000 (1.5.6) | 1–30 June | 60,000 | 1 July | R. Adeyemi, project accountant |
| CA-Civils-Foundations — plant | Assessment: hire days on site × contracted day rate, invoice missing | 16–30 June | 22,000 | 1 July | R. Adeyemi, project accountant |
| CA-Site establishment — labour | Timesheet: 12 site-operative days worked, unpaid at cut-off | 26–30 June | 18,000 | 1 July | S. Haddad, financial accountant |

**Usage note.** Every accrual is coded to a **control account** (1.5.4), not merely to an expense class, so
the project cost view and `CPI` stay honest as well as the statutory totals — the 1.5.2 pitfall in reverse.
The **basis** column is the audit trail: a reviewer challenges the GRNI report, the timesheet count or the
assessment method, not a bare number. The **reversal date** commits each accrual to automatic reversal on the
first day of the next period, so the arriving invoice does not double-count the cost (1.3.3). The first row
echoes the 1.5.6 mini-case: work performed USD 240,000, invoiced USD 180,000, accrual USD 60,000.

---

## Exam preparation — Domain 1

**How this domain is examined.** Domain 1 items span the full cognitive range: **recall** of the
debit/credit rules, the normal balances and the three IAS 37 recognition tests; **application** of the
mechanics — posting entries, reconciling a bank statement, computing period-end adjustments and measuring
provisions; and **analysis** of what the numbers mean — what a balanced trial balance cannot prove, accrual
versus provision, why profit is not cash. Numerical items concentrate in KAs 1.1, 1.3 and 1.4: transaction
cycles and reconciliations, depreciation and adjustment arithmetic, and expected-value and discounted
provisions. The sample MCQs and calculation exercises in this domain are drawn from the same blueprint as —
but kept strictly separate from — the live examination bank.

**Calculation traps.** The distractors in this domain's items punish specific, recurring mistakes:

- **Applying a debit as an increase to a liability** — or reading "debit" as "increase" everywhere. The
  sign depends on account type (the trap in MCQs 1.1-A and 1.1-B).
- **Mis-signing a bank reconciliation** — adding outstanding cheques instead of deducting them, or
  deducting the deposit in transit (MCQ 1.1-F).
- **Striking profit before the accruals** — recognising a cost on the invoice date rather than the economic
  event, so cost-to-date and the margin are both wrong (MCQ 1.3-C; exercise 1.2; the Terrafirm case).
- **Expensing the whole prepayment** at payment, or reporting the expense recognised to date when the
  question asks for the *remaining asset* (MCQ 1.3-E).
- **Discounting the wrong direction** — compounding `20,000 × 1.08³` instead of dividing by `1.08³`, or
  unwinding a discount with the wrong sign (MCQs 1.4-D and 1.4-F).
- **Confusing the onerous-contract loss** with cost-to-complete or cost-to-date — and forgetting to net off
  the loss already booked before sizing the provision (MCQ 1.4-C; exercise 1.4).

**Time management.** Recall items on rules and terms should take seconds; bank reconciliations, adjusted
profits and discounted provisions with an unwind are multi-step and reward care over speed. Write the
formula down first — `Adjusted bank = statement − outstanding cheques + deposits in transit`;
`PV = amount / (1 + r)^n` — and only then substitute; most distractors are correct arithmetic applied to the
wrong formula.

**Reflection questions.**

1. Which accruals in your current project's cost-to-date are assessments rather than documents, and who
   reviews them before the close?
2. When was your project cost report last reconciled to the general ledger, and what unexplained differences
   remain open?
3. Which provisions on your project were *considered nils* this period — and where is that consideration
   recorded?
4. If this month's accrual pass were skipped, how far would the reported margin move, and who would notice
   first?

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

