# Domain 13 — Due Diligence and Financial Close

> **Group:** Executing the transaction (Domain 13 of 4 in Part Three). **Target:** ~75 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain closes Part Three. It consumes Domain 5's condition
> register, Domain 6's model and model-audit economics, Domain 9's tranche structures, Domain 10's
> coverage machinery and Domain 12's contract set, and converts them into a dated event: money in
> an account. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domains 10 to 12 sized the debt, allocated the risk and drafted the contracts — all on the
assumption that the facts are as represented. Nothing so far has tested that assumption, and
nothing so far has turned an agreed set of terms into a funded facility. Both jobs belong here.
**Due diligence** is the organised purchase of information about a project before irrevocable
commitment; **financial close** is the moment at which every condition has been satisfied, every
document signed and the first drawdown is available. Between them sits the most compressed and
most expensive period in a project financing, and the period in which the largest number of
avoidable errors are made — not because practitioners do not know what to check, but because they
treat checking as a procedural obligation rather than as an investment with a computable return.

The domain's central claim is that **diligence and close are economic decisions under a deadline,
and both are governed by the price of elapsed time rather than the price of advice.** Three
consequences follow, one per Knowledge Area and one across them. A diligence stream is worth
running when the loss it can avoid exceeds what it costs, and because fees are small beside the
cost of delay, the decisive variable is whether the stream sits inside a parallel envelope or on
the critical path (KA 13.1). A model audit produces findings whose value lies entirely in their
**class** — whether a finding changes a conclusion — so a finding count is not a measure of
anything (KA 13.2). Conditions precedent form a **conjunction with a critical path**, which means
the close date is the expected *maximum* of several chains and not the expected duration of the
longest one, a distinction that is invisible in every close timetable drawn as a single bar
(KA 13.3). And syndication converts the closed transaction into held positions, with a fee
architecture and a market-flex mechanism whose value routinely exceeds the fee that sponsors spend
their negotiating capital on (KA 13.4).

**Learning objectives.** After this domain a candidate can: name the seven standard diligence
streams and state what each is uniquely able to find; price a diligence stream as an information
purchase and compute its breakeven detection rate; demonstrate why running the same streams in
parallel rather than in series can be the difference between creating and destroying value;
classify model-audit findings by whether they change a conclusion, a reported output or nothing,
and compute the debt-capacity consequence of each class; explain why a finding count and a
"findings closed" percentage are misleading control metrics; build a conditions-precedent register
as a dependency network with float, compute the expected close date as the expected maximum of its
chains, and identify the chain whose de-risking is worth most; build an itemised close-cost budget,
express it as a share of debt raised, reconcile it to the sources-and-uses statement and compute
the effective cost of debt it implies; explain the underwriting, club and best-efforts syndication
routes and compute the fee split, the arranger's yield differential and the cost of a failed
sell-down; quantify market flex and test it against the coverage covenant; and govern AI use across
data-room review, model scanning, condition tracking and investor allocation without letting a
machine conclude anything that a named professional must own.

**The master transaction.** Kestrel Water SPC reaches the diligence phase of its financing.
The mandate is for **USD 42,000,000** of senior debt at **6.0 % over 12 years** against a
**USD 60,000,000** envelope funded 70/30 (Domain 6, KA 6.2.1), with an annual instalment of
**USD 5,009,635.23**, first-year interest of **2,520,000** and principal of **2,489,635**
(Domain 3). Documented first-year `CFADS` is **USD 6,384,000** (Domain 2), giving `DSCR`
**1.2743** and, on level cash, an identical `LLCR`; `PLCR` is **1.9431** (Domain 10). The
concession's expiry is fixed, so every day between mandate and close destroys one day of operating
life: Domain 5 (KA 5.4.2) priced that at **USD 17,733.33 per day** of forgone `CFADS` on a 30/360
basis, which this domain uses as **USD 124,133.33 per calendar week** — the single number that
governs almost every decision in it. Seven diligence streams will be commissioned for
**USD 1,500,000** of fees; the full close-cost budget will come to **USD 2,709,000**; the model
audit will return **34 findings**; and the conditions-precedent register will resolve into five
chains whose base close date is 20 weeks away.

---

## Knowledge Area 13.1 — The diligence streams

*Topics: 13.1.1 what diligence is for · 13.1.2 the seven streams and what each uniquely sees ·
13.1.3 diligence as an information purchase · 13.1.4 the parallel envelope · 13.1.5 the data room
and the diligence trail.*

### 13.1.1 What diligence is for

**Definition.** Due diligence is the systematic, evidenced examination of a project's technical,
financial, legal, tax, insurance, environmental, social and market characteristics, undertaken
before commitment, by parties who will state their findings in writing and be identifiable for
them.

Three properties distinguish it from ordinary analysis, and each has a practical consequence.
**It is adversarial in structure and cooperative in conduct** — the lenders' advisers are paid to
find what the sponsor's team has missed or has an interest in not seeing, while everyone depends on
the sponsor's cooperation to find anything at all; a diligence process that becomes genuinely
adversarial in conduct simply stops producing information. **It is evidenced rather than
concluded** — a diligence report's value is its trail from an assertion to a document, which is
why "the technical adviser is comfortable" is not a diligence output and a numbered finding
against a specific drawing revision is. And **it is time-boxed by a commercial deadline**, which
is the property that makes it an economic problem rather than a research problem: unlimited
diligence is available at a price nobody will pay, so the real question is always *which* checks,
in *what* order, inside *how long*.

**What diligence cannot do.** It cannot transfer risk. A report identifies a ground condition; it
does not pay for it (Domain 11's allocation does, and Domain 12's contract writes it down). It
cannot make an unbankable project bankable (Domain 5). And it does not discharge the accountability
of the person relying on it — the liability caps of 13.A.1 make that arithmetically obvious.
Diligence buys **information**, and information is valuable only if a decision changes as a result.
The disciplined test to apply to any proposed piece of work is therefore: *what decision would a
different answer change, and what is that decision worth?* A stream that cannot answer that
question is being commissioned for comfort, and comfort is the most expensive thing in a close
timetable.

### 13.1.2 The seven streams and what each uniquely sees

The market has converged on seven streams. The useful way to hold them is not by discipline but by
**the class of error each is uniquely able to detect** — because that is what makes them
non-substitutable.

| Stream | Core question | The error only this stream finds |
|---|---|---|
| **Technical** | Will the asset do what the model assumes, for as long as it assumes? | Design, capacity, availability, degradation and lifecycle-cost assumptions that no financial reviewer can challenge |
| **Financial** | Does the model implement the documents, and do the historical accounts support the forecast? | Definitional and structural defects in the model (KA 13.2) |
| **Legal** | Is the security package enforceable, are the contracts as summarised, is title good? | Enforceability, perfection and consent gaps behind a correct-looking contract summary |
| **Tax** | What cash tax will this structure actually pay, in this jurisdiction, on this timetable? | Withholding, thin capitalisation, loss-relief limits, indirect tax on construction |
| **Insurance** | Is the programme in place, and does it match the loss the documents assume is insured? | Exclusions, deductibles, sub-limits and the gap between a broker's slip and a lender's endorsement requirement |
| **Environmental and social** | What obligations, liabilities and stakeholder exposures attach to the site and the works? | Legacy contamination, resettlement, biodiversity and consent-condition obligations that survive close |
| **Market** | Will the revenue exist at the price and volume the model assumes? | Demand, price and competitive assumptions — the largest single source of realised project failure |

Two structural points about the set. **The streams overlap at exactly the places where projects
fail**, and the overlaps must be managed rather than eliminated: the technical adviser's
availability assumption is the financial model's revenue driver and the offtake contract's
liquidated-damages trigger, so three streams touch one number and the risk is that each assumes
another owns it. The countermeasure is a **cross-stream interface list** — a short register naming
every assumption that more than one adviser touches, with a single named owner (Toolkit 13.T.1).
And **the streams are not equally reliable**. Technical, legal and tax diligence examine facts that
exist; market diligence examines a forecast that does not, which is why a market consultant's
report is evidence about a method rather than evidence about the future, and why Domain 7's
stress-testing discipline is the proper use of it.

### 13.1.3 Diligence as an information purchase

The shape of the arithmetic is PML-AI's gate economics (PML-AI KA 3.3.1), reapplied to a diligence
stream. A stream costs a fee and, if it sits on the critical path, elapsed time; in return it
converts some probability of a costly post-close discovery into a cheap pre-close correction.

```
Expected cost without the stream = p × C
Expected cost with the stream    = fee + (weeks on the critical path × cost of delay)
                                   + p × [ d × F + (1 − d) × C ]
Net value                        = the difference
```

where `p` is the probability that a material issue of this stream's class exists, `d` the
probability the stream detects it, `C` the cost if it reaches close undetected and `F` the cost of
correcting it pre-close. Rearranging for the detection rate at which the stream just breaks even
gives the single most useful expression in this Knowledge Area:

```
Breakeven detection rate  d* = (fee + priced elapsed time) ÷ [ p × (C − F) ]
```

Read it in words: **the breakeven detection rate is the diligence spend divided by the expected
avoidable loss.** Everything about the economics of diligence follows from the fact that the
numerator contains elapsed time.

**Worked example 13.1.3 — Kestrel's seven streams, priced.**

1. **Setup.** Illustrative parameters an organisation must estimate from its own transaction
   record; the arithmetic, not the parameters, is transferable. Kestrel commissions seven streams
   with the fees and durations below. `p`, `C` and `F` are the sponsor's own estimates from
   comparable closings; `C` for the financial stream is Domain 6's independently derived
   **2,691,071** for a material model error reaching close (KA 6.4.3), which anchors the scale of
   the others. Detection is assumed at **d = 0.80** for every stream. Cost of delay
   **124,133.33 per week**.

   | Stream | Fee | Weeks | `p` | `C` | `F` | `p × C` | `p × (C − F)` |
   |---|---|---|---|---|---|---|---|
   | Technical | 260,000 | 8 | 0.30 | 6,200,000 | 400,000 | 1,860,000.00 | 1,740,000.00 |
   | Financial | 180,000 | 5 | 0.35 | 2,691,071 | 60,000 | 941,874.85 | 920,874.85 |
   | Legal | 420,000 | 10 | 0.25 | 9,000,000 | 250,000 | 2,250,000.00 | 2,187,500.00 |
   | Tax | 150,000 | 6 | 0.20 | 4,800,000 | 300,000 | 960,000.00 | 900,000.00 |
   | Insurance | 60,000 | 4 | 0.15 | 1,400,000 | 120,000 | 210,000.00 | 192,000.00 |
   | Environmental and social | 240,000 | 12 | 0.22 | 7,500,000 | 900,000 | 1,650,000.00 | 1,452,000.00 |
   | Market | 190,000 | 9 | 0.28 | 5,600,000 | 500,000 | 1,568,000.00 | 1,428,000.00 |
   | **Total** | **1,500,000** | **54** | | | | **9,439,874.85** | |

2. **Formula.** As above, per stream and in aggregate. The aggregate residual expected cost is
   `Σ p × [d × F + (1 − d) × C]`. Elapsed time is priced **once for the envelope** when streams run
   in parallel and **once per stream** when they run in series.
3. **Substitution.** Aggregate `Σ p × C = 9,439,874.85`. Residual
   `Σ p × [0.80 × F + 0.20 × C] = 2,383,574.97`. Parallel envelope
   `12 × 124,133.33 = 1,489,600`. Serial sequencing `54 × 124,133.33 = 6,703,200`.
4. **Result.** **Parallel:** expected cost `1,500,000 + 1,489,600 + 2,383,574.97 = 5,373,174.97`;
   net value **+USD 4,066,699.88**. **Serial:** expected cost
   `1,500,000 + 6,703,200 + 2,383,574.97 = 10,586,774.97`; net value **−USD 1,146,900.12**.
   The difference is **5,213,600**, exactly `42 weeks × 124,133.33`.
5. **Interpretation.** The same seven reviews, the same seven fees, the same detection rates —
   worth **+4,066,700** run inside a twelve-week envelope and **−1,146,900** run one after another.
   Not one hour of diligence differs between the two readings; **the entire 5,213,600 is
   scheduling.** That is the domain's headline result and it reframes the standard argument: a
   sponsor negotiating adviser fees is negotiating over 1,500,000 while the sequencing decision is
   worth three and a half times as much, and a lender insisting on serial sequencing "so each
   stream can rely on the last" is destroying value it cannot see because its own cost of delay is
   zero. The per-stream breakeven detection rates make the point sharper still. Inside the
   envelope, where only the fee is charged, the highest breakeven of the seven is **insurance at
   31.25 %** and the lowest is **market at 13.31 %** — every stream pays comfortably, because a
   competent adviser detects far more than a third of the issues in their own discipline.
   Sequenced serially, the same streams need **72.02 %** (technical), **75.95 %** (legal),
   **86.95 %** (financial), **91.54 %** (market) and **99.42 %** (tax) — that last figure being a
   review that must be effectively infallible to be worth holding — while **environmental and
   social (119.12 %)** and **insurance (289.86 %)** cannot pay at *any* detection rate, because
   their elapsed time alone exceeds the loss they could avoid. Three cautions belong with this
   result. First, `d` is the least defensible parameter in the set and nobody has measured it on a
   sample of one project; the sensitivity of the answer to it is the reason the breakevens, not the
   net values, are the output a leader should carry. Second, `C` has a long right tail that
   expected value averages away — a legal finding can be existential rather than expensive — so a
   stream may be worth running below its breakeven precisely because the distribution is not
   symmetric. Third, several streams are **conditions precedent** rather than choices (KA 13.3):
   the model audit and the lenders' legal and technical reviews will be run whatever the arithmetic
   says, and the arithmetic's job is then to argue about **timing and scope**, which is exactly the
   conclusion Domain 6 reached about the audit alone.

> **Fig 13.1.1 — The breakeven detection rate of each diligence stream, in parallel and in series.**
> Horizontal dumbbell chart, seven streams down the left with fee and duration, x-axis breakeven
> detection rate 0–130 % with a dashed 100 % ceiling. A blue marker per stream shows the breakeven
> when the stream sits inside the common twelve-week envelope (fee only): market **13.31**,
> technical **14.94**, environmental and social **16.53**, tax **16.67**, legal **19.20**,
> financial **19.55**, insurance **31.25**. A crimson marker shows the breakeven when the stream is
> sequenced serially and carries its own elapsed weeks at 124,133.33 each: technical **72.02 %**,
> legal **75.95 %**, financial **86.95 %**, market **91.54 %**, tax **99.42 %**; environmental and
> social **119.12 %** and insurance **289.86 %** are drawn as arrows through the ceiling and
> labelled "impossible". Header states the aggregate: **+4,066,700** in parallel against
> **−1,146,900** in series, the 5,213,600 difference being 42 weeks of scheduling. Source: PCI
> original. Alt text: seven paired markers showing that every diligence stream breaks even at a
> low detection rate when run in parallel, but requires an implausible or impossible detection rate
> when sequenced one after another.

### 13.1.4 The parallel envelope

If elapsed time dominates the economics, then **designing the envelope is the diligence manager's
principal task**, and it is a scheduling problem with four levers.

**Genuine parallelism.** Most streams do not depend on each other's outputs and can be
commissioned on the same day. The dependencies that do exist are few and specific: the model audit
needs a frozen model, the insurance adviser needs the technical report's loss scenarios, the tax
adviser needs the final structure chart. Everything else is convention.

**Staged deliverables instead of a single report.** A stream that reports once, at week twelve, is
a stream whose findings cannot be acted on inside the envelope. A stream that issues a red-flag
memorandum at week three, a draft at week eight and a final report at week twelve produces the same
information and gives the transaction nine weeks to respond. This is the single highest-return
change available in most close processes and it costs nothing.

**Scope discipline.** The envelope's length is set by the longest stream — here environmental and
social at twelve weeks. Shortening *that* stream shortens the envelope; shortening any other
shortens nothing. Compression effort spent off the binding stream is spent for no return, which is
the same lesson KA 13.3 draws about conditions.

**Honest reliance boundaries.** Parallelism creates the risk that two advisers assume different
values for the same input, so each stream's report must state the inputs it took from another
stream and their version. Without that, parallel diligence produces a set of internally consistent
reports that are mutually inconsistent — the failure mode Domain 6's input-provenance rule exists
to prevent, appearing at transaction level.

The residual honest point: parallelism has a cost of its own. Running seven streams at once
requires management bandwidth the sponsor may not have, and a red-flag finding at week three
against six streams already in flight produces expensive rework. The right conclusion is not
"always parallel" but **"parallel by default, sequenced only where a dependency is real and priced
when it is"** — and the price is 124,133.33 a week.

### 13.1.5 The data room and the diligence trail

A **data room** is the controlled repository through which the sponsor discloses documents to
advisers and lenders, and its quality is a leading indicator of everything else: a project whose
data room is complete, indexed and version-controlled is a project whose sponsor has the discipline
the finance documents will require for the next twelve years. Three disciplines earn their place.
**A disclosure index that is itself a document** — numbered, dated, listing what was disclosed and
when — because it later determines what a lender was told, which matters if a representation is
challenged. **Version control with supersession marked, not deleted** — a superseded traffic
forecast left in the room without a supersession note is the exact failure Domain 6 (Case study B)
priced, and a superseded forecast *removed* is worse, because the trail disappears. And **a
question-and-answer log**, because the answers are representations in substance whether or not they
are representations in form.

The **diligence trail** is what remains after close: reports, the disclosure index, the Q-and-A
log, the finding registers and their close-out evidence. It is not archive material — it is the
evidence base for the first covenant dispute, the first warranty claim, the first refinancing and
the first sale, and Case study B shows what its absence costs.

### AI in this KA

**Where it earns its place.** Data-room triage is now genuinely strong machine work and it attacks
exactly the constraint that matters: elapsed time. Three uses are robust. **Completeness checking**
— reconciling the data-room index against a required-document list and reporting what is absent,
which is a set operation humans perform badly at scale. **Extraction into a comparable register** —
pulling parties, dates, tenors, defined terms, termination triggers, caps, indexation formulae and
governing law out of several hundred documents into the structured table Domain 5 (KA 5.3) and
Domain 12 (KA 12.4.1) both depend on. And **inconsistency flagging** across documents: the same
defined term with two definitions, a date that differs between the offtake and the concession, a
capacity figure that differs between the technical report and the model.

**Where it must not go.** It must not produce a conclusion in any stream. Whether security is
perfected, whether withholding applies, whether a consent condition survives close, whether an
exclusion defeats an insured loss — each is a professional opinion in a named jurisdiction with a
named accountable author, and a plausible machine answer to any of them is more dangerous than no
answer, because it will be relied on without a liability position behind it. It must not be
presented as diligence to a lender: a machine extraction is an input to a stream, never the
stream. And it must not be used to shorten the envelope by removing a stream — the 13.1.3
arithmetic shows machine pre-checks are additive to review, the same conclusion Domain 6 reached
about model scanning (KA 6.4.4).

**Verification, concretely.** Sample the extraction: for a stated sample size — twenty documents
or ten per cent, whichever is larger — reconcile every extracted field to the source document and
record the error rate, by field type, in the diligence file. Never accept an extraction whose error
rate has not been measured on this transaction's own documents, because extraction accuracy varies
by document quality and this project's documents are the only relevant population. Every flagged
inconsistency is confirmed by a human before it enters a finding register. And the estimated
parameters in the stream economics — `p`, `d`, `C` — are recorded with their basis, because a
machine-assisted process that changes `d` without measuring it has changed the answer without
evidence. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 13.1

| Term | Meaning |
|---|---|
| **Due diligence** | Evidenced pre-commitment examination of a project, reported in writing by identifiable authors. |
| **Diligence stream** | One discipline's review — technical, financial, legal, tax, insurance, E&S, market. |
| **Breakeven detection rate `d*`** | `(fee + priced elapsed time) ÷ [p × (C − F)]`; the diligence spend divided by the expected avoidable loss. |
| **Parallel envelope** | The period inside which streams run concurrently, so elapsed time is priced once, not per stream. |
| **Red-flag memorandum** | An early partial deliverable that lets findings be acted on inside the envelope. |
| **Cross-stream interface list** | Register of assumptions touched by more than one adviser, each with a single named owner. |
| **Data room / disclosure index** | The controlled disclosure repository and the numbered, dated record of what was disclosed. |

### Sample MCQs — KA 13.1

**MCQ 13.1-A `[13.1.3 · Application]`** A diligence stream costs a 260,000 fee, `p` = 0.30,
`C` = 6,200,000 and `F` = 400,000. Run inside a parallel envelope so that it adds no elapsed time,
its breakeven detection rate is:
- A. 13.98 %
- B. 14.94 % ✅
- C. 4.19 %
- D. 72.02 %

*Rationale:* `260,000 / [0.30 × (6,200,000 − 400,000)] = 260,000/1,740,000 = 14.94 %`. A divides by
`p × C` and omits the pre-close correction cost `F`, overstating the avoidable loss and so
understating the breakeven; C divides the fee by `C` alone and ignores `p`; D is the same stream's
breakeven when sequenced serially with eight weeks of delay priced in.

**MCQ 13.1-B `[13.1.4 · Analysis]`** Seven diligence streams with 1,500,000 of fees and durations
totalling 54 weeks are worth **+4,066,700** in a twelve-week parallel envelope and **−1,146,900**
sequenced serially, at a cost of delay of 124,133.33 a week. The correct reading is:
- A. the streams are marginal and some should be dropped
- B. the entire 5,213,600 difference is the 42 weeks of sequencing, so the sequencing decision matters roughly three and a half times more than the fee negotiation ✅
- C. the fees are too high and should be reduced by 5,213,600
- D. serial sequencing is preferable because each stream can rely on the last

*Rationale:* `42 × 124,133.33 = 5,213,600` and the fees, probabilities and detection rates are
identical in both readings (13.1.3). A misreads a scheduling result as a scope result; C is
arithmetically impossible against 1,500,000 of fees; D describes a real but rare dependency that
13.1.4 requires to be priced rather than assumed.

**MCQ 13.1-C `[13.1.3 · Evaluation]`** On the parameters above, insurance diligence sequenced
serially has a breakeven detection rate of 289.86 %. The professional conclusion is:
- A. insurance diligence should be abandoned
- B. the insurance adviser is overpriced
- C. insurance diligence must sit inside the envelope, where its breakeven is 31.25 % — the finding is about sequencing, not about the stream ✅
- D. the calculation is invalid because a probability cannot exceed one

*Rationale:* A breakeven above 100 % says the configuration cannot pay, not that the review is
worthless; inside the envelope the same stream breaks even at 31.25 % (13.1.3). B is wrong because
the 60,000 fee is the smallest of the seven — 496,533.33 of delay is what makes it impossible. D
mistakes an impossible-configuration signal for an arithmetic error.

**MCQ 13.1-D `[13.1.2 · Analysis]`** Three parties rely on the same plant availability figure: the
technical adviser who derives it, the financial model that earns revenue on it and the offtake
agreement that pays damages against it. The specific diligence control for this is:
- A. asking each adviser to confirm they are comfortable
- B. a cross-stream interface list naming every multi-adviser assumption with one named owner and one version ✅
- C. running the streams serially so each can rely on the previous report
- D. relying on the model audit to catch any inconsistency

*Rationale:* The overlap is where projects fail, and the countermeasure is single ownership of the
shared assumption (13.1.2). A produces no evidence; C costs 124,133.33 a week for a control a
register achieves free; D is out of scope — the audit tests the model against documents, not
whether two advisers assumed the same thing.

**MCQ 13.1-E `[13.1.4 · Evaluation]`** Kestrel's diligence envelope is twelve weeks, set by the
environmental and social stream; the next-longest streams are legal at ten weeks and market at
nine. Delay costs 124,133.33 a week. The environmental and social consultant offers to compress its
stream to nine weeks for a 200,000 acceleration fee. The sponsor should:
- A. accept — three weeks at 124,133.33 is 372,399.99 against a 200,000 fee, a clear gain
- B. accept, but recognise that the envelope then falls only to ten weeks on the legal stream, so the acceleration buys two weeks (248,266.66) for 200,000 — a thin 48,266.66, and worth pairing with legal compression before the fee is agreed ✅
- C. decline — 200,000 exceeds the 150,000 that a ten per cent reduction across the whole 1,500,000 fee book would save
- D. decline — compression off the binding stream returns nothing

*Rationale:* The envelope is the maximum of the stream durations, so compressing the binding stream
returns weeks only until the second-longest stream becomes binding: `max(8, 5, 10, 6, 4, 9, 9) = 10`,
giving `2 × 124,133.33 = 248,266.66` and a net **48,266.66**, not the 372,399.99 A assumes by
crediting all three weeks. C compares an acceleration purchase against a fee negotiation as though
the two were alternatives when both are available, and prefers the smaller saving. D applies 13.1.4's
rule to the wrong stream — environmental and social *is* the binding stream; the rule bites on the
legal ceiling, which is why the recommendation is conditional rather than negative.

**MCQ 13.1-F `[13.1.1 · Comprehension]`** Which restatement most closely captures the test 13.1.1
sets before any diligence stream is commissioned?
- A. every discipline should be reviewed to a comparable depth, so that the file is complete
- B. a stream is worth commissioning when a different answer would change a decision, and that decision is worth more than the stream costs ✅
- C. a stream is worth commissioning when the adviser will accept liability for its conclusions
- D. a stream is worth commissioning when the lenders require it as a condition precedent

*Rationale:* Diligence buys information, and information has value only where a decision turns on it
(13.1.1). A is the comfort purchase the topic warns against — uniform depth is a filing standard, not
a value test; C confuses reliance with information value, and 13.A.1 shows the cap covers a small
fraction of the exposure in any case; D is a real procurement fact for several streams, but 13.1.3
notes the arithmetic then argues about timing and scope rather than about whether to run the stream.

**MCQ 13.1-G `[13.1.5 · Evaluation]`** During diligence a demand study commissioned early in
development is superseded by a second study with a lower central case. The sponsor's development
team proposes removing the first study from the data room, on the ground that no party relies on it
and its presence will only raise questions the second study has already answered. The soundest
professional position is that the first study:
- A. should be removed — a superseded document is not a disclosure, and leaving it in invites questions
  about a forecast nobody relies on
- B. should stay, marked superseded with a date and a note, and appear on the numbered disclosure index —
  because the index later determines what the lenders were told, and the trail is the evidence base for
  the first covenant dispute, warranty claim and refinancing ✅
- C. should stay unmarked, since both studies carry dates and a reader can see which is later
- D. should be moved to a folder visible only to the sponsor's own advisers, so that it remains available
  internally without being disclosed

*Rationale:* Version control with supersession **marked** rather than deleted is the discipline, and
removal is the worse of the two failures because the trail disappears (13.1.5) — a superseded forecast
left in a room without a supersession note is what Domain 6 (Case study B) priced. A treats disclosure
as a presentational choice, and it is the choice a challenged representation will later be tested
against. C leaves two live-looking central cases and no record of which was relied on. D is the same act
as A with a record that the omission was deliberate.

### Self-check — KA 13.1

1. *State the breakeven detection rate in words.* — The diligence spend divided by the expected
   avoidable loss, `p × (C − F)`; elapsed time is part of the spend.
2. *Why does shortening the tax stream not shorten the envelope?* — The envelope is set by the
   longest stream (environmental and social, twelve weeks); compression off the binding stream
   returns nothing.
3. *What does diligence buy, and what does it not?* — Information, and only where a decision would
   change; it never transfers risk and never discharges the reliant party's accountability.

---

## Knowledge Area 13.2 — Model audit

*Topics: 13.2.1 scope and what the audit is not · 13.2.2 findings by class · 13.2.3 the class-one
findings priced · 13.2.4 closing findings out.*

### 13.2.1 Scope and what the audit is not

Domain 6 (KA 6.4.3) defined the model audit, established the governance apparatus that makes it
possible and **priced it** — finding that its elapsed time rather than its fee decides whether it
pays, that its breakeven error rate falls from 20.10 % to 8.05 % when it moves early, and that a
weak audit is worse than none below a 48.81 % detection rate. That arithmetic is not repeated here.
What Domain 13 adds is the transaction-side treatment: what the audit's scope must cover, how its
output must be read, and what happens to a financing when a finding lands.

**The four scope layers**, in the order a competent auditor works through them and in the reverse
order of how often they are the problem. **Arithmetic** — do the formulae compute what they claim?
This is the layer everybody expects and the layer that almost never contains the expensive error.
**Structure** — do the mechanics work under stress: does the waterfall behave when a tier fails,
does the tax computation behave when losses exhaust, does the schedule close at zero? **Documents**
— does the model implement the *finance documents*: the `CFADS` definition clause by clause, the
covenant test dates, the reserve mechanics, the interest convention, the distribution tests? And
**assumptions and provenance** — is each input traced to a source document of the correct version,
and are the conventions internally consistent (Domain 3's timing and day-count discipline, Domain
6's convention list)?

**What the audit is not.** It is not a valuation opinion, and it is not an endorsement of the
assumptions: an auditor confirms that a tariff escalation of 2.5 % is implemented correctly and
sourced to clause 14.3, not that 2.5 % is right. It is not a substitute for the technical, market
or tax streams — it tests whether the model implements what those streams concluded. It is not a
sponsor deliverable; it is a lenders' control, and a sponsor who treats it as a compliance exercise
has misunderstood who is buying. And, critically, **it is not a warranty**: its liability position
is capped and its reliance is addressed (13.A.1), so a signed audit report reduces the probability
of an error and does not transfer the cost of one.

### 13.2.2 Findings by class, not totalled

A model audit reports findings. The universal reporting failure — in transaction status reports, in
credit papers and in sponsor board packs alike — is to **total them**. "Thirty-four findings, of
which thirty-one closed" is a sentence with no information content, because the findings are not
commensurable. The remedy is a **class scheme applied before any count is quoted**, and the classes
must be defined by *consequence*, not by the auditor's sense of importance:

| Class | Definition | What it changes | Who must see it |
|---|---|---|---|
| **Class 1 — fundamental** | Changes a financing **conclusion**: debt capacity, a covenant outcome, the funding plan's sufficiency, or a distribution test | The transaction | Credit committee and sponsor board, individually and by name |
| **Class 2 — material** | Changes a reported output beyond the agreed materiality threshold, without changing a conclusion | The reported numbers | Deal team, listed individually with quantified effect |
| **Class 3 — presentational or procedural** | Changes no number: labelling, formatting, unused rows, documentation, print ranges, non-output hardcoding | Nothing | Reported in aggregate only |

**The class scheme requires a stated materiality threshold**, and it should be expressed in the
metric the transaction is judged on rather than in currency. Kestrel's was set at **0.01× of
`DSCR`** — roughly 50,000 of annual `CFADS` at its debt-service level — which is the kind of
threshold a credit committee can reason about, unlike "USD 100,000" whose significance depends on
which line it lands in.

**Kestrel's 34 findings resolved as 3 Class 1, 7 Class 2 and 24 Class 3.** Three consequences that
generalise:

- **Class 3 is 70.6 % of the count and 0 % of the impact.** Any metric driven by count is therefore
  driven by the class that does not matter, which is why "31 of 34 closed" — **91.2 %** — is a
  progress figure that can be true while every finding that changes the transaction remains open.
  The honest metric is **class-weighted**: three of three Class 1 findings open is 0 % of the
  impact closed, whatever the count says.
- **The seven Class 2 findings netted to +18,000 of year-one `CFADS`** — they very nearly cancelled.
  That is a coincidence and must never be reported as a control: each was wrong, each was corrected,
  and the netting tells the reader nothing except that this particular set of errors happened to
  point in opposing directions. A model whose Class 2 findings net to zero is not a model with no
  Class 2 findings.
- **Only Class 1 findings are re-audited.** Correcting a fundamental finding changes dependent
  lines, so the corrected model must be re-run against the audit's own test set and the golden
  answers re-verified (Domain 6's regression discipline). Class 2 corrections are checked; Class 3
  corrections are accepted on the modeller's confirmation. Treating all three the same is how a
  close timetable loses two weeks to formatting.

### 13.2.3 The class-one findings priced

**Worked example 13.2.3 — what three findings did to Kestrel's financing.**

1. **Setup.** The model audit of Kestrel's financing model returns three Class 1 findings.
   **F-01, definitional:** the facility defines `CFADS` after the annual maintenance-reserve
   top-up of **260,000**; the model struck it after debt service instead, so reported `CFADS` of
   6,384,000 overstates the defined figure. **F-02, convention:** the debt-service schedule was
   built as an **annuity due** (payments in advance) rather than in arrears as the facility
   provides — Domain 3 (KA 3.2.1) named this the commonest annuity mistake; here it is inside a
   financing model. **F-03, funding plan:** the six-month DSRA
   of **2,504,818** (Domain 10, KA 10.3.2) appears as an operating-period cash outflow but not in
   the uses of funds, so the funding plan does not fund it. The lender's requirement is a
   base-case `DSCR` of **1.30×**; `AF(0.06, 12) = 8.383844`.
2. **Formula.** Corrected `CFADS` = reported − the misplaced line. `DSCR` = `CFADS` ÷ debt service.
   `AF` for an annuity due = `AF(r, n) × (1 + r)`. Debt capacity =
   `CFADS ÷ target DSCR × AF(r, n)` (Domain 10, KA 10.1.2).
3. **Substitution.** F-01: `6,384,000 − 260,000`, then `÷ 5,009,635.23`. F-02:
   `AF_due = 8.383844 × 1.06 = 8.886875`; instalment `42,000,000 ÷ 8.886875`; then
   `6,384,000 ÷ that instalment`. Capacity: `6,124,000 ÷ 1.30 = 4,710,769.23`, `× 8.383844`.
4. **Result.**

   | Finding | The error's effect on what was reported | The corrected figure |
   |---|---|---|
   | **F-02** convention | Instalment **4,726,071** — understated by **283,564**; `DSCR` reported as **1.3508**, apparently clearing the 1.30× requirement | Instalment **5,009,635.23**; `DSCR` **1.2743** |
   | **F-01** definition | `CFADS` overstated by 260,000 | `CFADS` **6,124,000**; `DSCR` **1.2224** |
   | **F-01 + F-02** together | Reported `DSCR` **1.2958** on the wrong instalment | Debt capacity at 1.30× **USD 39,494,354** against 42,000,000 mandated — a resize of **USD 2,505,646** |
   | **F-03** funding plan | Uses of funds understated by **2,504,818** | The DSRA must be funded, from debt, equity or first operating cash |

5. **Interpretation.** Take F-02 first, because it is the finding that should frighten a reader.
   A single timing convention — payments in advance instead of in arrears — understated the
   instalment by **283,564**, which flattered the `DSCR` from 1.2743 to **1.3508** and made the
   model report a project that comfortably cleared a 1.30× requirement it in fact fails. Nothing
   about the model looked wrong: the schedule closed at zero, total principal equalled the loan and
   every check block was green, because an annuity due is a perfectly valid amortisation of
   42,000,000 — just not the one the facility documents. **This is the characteristic shape of an
   expensive model error: internally consistent, arithmetically correct and wrong against the
   document**, which is precisely why the audit's document layer matters more than its arithmetic
   layer and why a machine scanner would not have found it (Domain 6, KA 6.4.4). Second, note the
   **direction of the combination**. F-01 and F-02 both flatter the ratio, so they compound rather
   than offset: together they take the reported 1.3508 down to a true 1.2224, and the debt the
   project can actually carry at the lender's requirement falls from Domain 10's **41,171,123** on
   uncorrected `CFADS` to **39,494,354** — a resize of **2,505,646** that must be funded as
   equity or not funded at all. Third, **F-03 is the finding that would have caused an event, not
   an argument**. A `DSCR` shortfall is negotiated; a funding plan that does not fund a mandatory
   reserve produces a failed condition at first drawdown or a covenant breach in the first
   operating year, and it is the class of error that reaches close most often, because sources-and-
   uses statements are reviewed for arithmetic (they always balance) rather than for completeness.
   Finally, the **professional caution about ordering**: had the auditor reported "34 findings,
   3 fundamental" without quantifying them, the credit committee would have received a status
   update rather than a decision paper. The quantification — 2,505,646 of resize and 2,504,818
   of unfunded reserve, on a mandate of 42,000,000 — is what converts an audit into a transaction
   event, and producing it is the sponsor's job as much as the auditor's.

### 13.2.4 Closing findings out

A finding is closed when the model has been corrected, the correction has been verified by the
auditor, and the **dependent consequences have been followed through**. The third limb is where
close processes fail, because a Class 1 correction propagates: correcting F-01 changes `CFADS`,
which changes every coverage ratio, the sculpting profile if there is one, the distribution
forecast, the lock-up test dates and the equity return. A finding closed in the model and not in
the credit paper, the term sheet and the base case is not closed.

The disciplines that make close-out real: **one register, owned by one named person**, with each
finding's class, quantified effect, correction, verifier and date; **a re-run of the golden-answer
set after every Class 1 correction**, with any changed figure explained before it is accepted;
**a final auditor's letter that lists the findings and their disposition** rather than issuing a
clean opinion that conceals what was found; and **a stated position on anything not closed** —
because findings are sometimes accepted rather than corrected, and an accepted finding with a
written rationale is professional whereas an accepted finding recorded as "closed" is not.

### AI in this KA

**Where it earns its place.** Domain 6 (KA 6.4.4) covered workbook scanning, version diffing,
adversarial test generation and documentation drafting, and priced a pre-check at 8,000 with a
55 % detection rate. The transaction-side addition here is narrower and genuinely useful:
**mapping the finance documents' defined terms onto model lines**, producing a candidate
reconciliation of every `CFADS` definition limb to the cell that implements it. That is the input
to Toolkit 13.T.2 and it attacks the layer where the expensive errors live.

**Where it must not go.** It must not assign the class. Classification is a judgment about whether
a financing conclusion changes — it requires knowing the lender's requirement, the covenant level
and what the credit committee approved — and a machine that grades findings by severity will grade
by textual signal, systematically under-rating the quiet definitional finding that resizes the
debt. It must not draft the auditor's letter or the certificate of satisfaction. And it must not be
credited with detection it did not achieve: F-02 above is a document-conformance error in a
structurally valid schedule, exactly the class scanners miss.

**Verification, concretely.** Every machine-proposed definition-to-line mapping is confirmed
against the clause by a named person, with the clause reference recorded. The golden-answer set is
re-run after every accepted correction, machine-assisted or not. And the finding register records,
per finding, whether it was found by machine, by the auditor or by the sponsor's own review — which
is the only way an organisation ever learns what its `d` actually is.

### Key terms — KA 13.2

| Term | Meaning |
|---|---|
| **Model audit** | Independent review of a model against the transaction documents, its own logic and its arithmetic (Domain 6, KA 6.4.3). |
| **Scope layers** | Arithmetic · structure · documents · assumptions and provenance; the expensive errors sit in the last two. |
| **Class 1 / 2 / 3 finding** | Changes a financing conclusion · changes a reported output beyond materiality · changes no number. |
| **Materiality threshold** | The stated size below which a difference is not a finding; best expressed in the transaction's own metric (0.01× of `DSCR`). |
| **Class-weighted closure** | Progress measured by impact closed, not findings closed. |
| **Finding close-out** | Corrected, verified, and every dependent consequence followed through into papers and the base case. |

### Sample MCQs — KA 13.2

**MCQ 13.2-A `[13.2.3 · Application]`** A model builds the debt-service schedule as an annuity due
rather than in arrears. On 42,000,000 at 6 % over 12 years (`AF` = 8.383844) with `CFADS` of
6,384,000, the reported `DSCR` and the truth are:
- A. reported 1.2743, true 1.3508
- B. reported 1.3508, true 1.2743 ✅
- C. reported and true both 1.2743 — the convention does not affect the ratio
- D. reported 1.2224, true 1.2743

*Rationale:* `AF_due = 8.383844 × 1.06 = 8.886875`, instalment `42,000,000/8.886875 = 4,726,071`
and `6,384,000/4,726,071 = 1.3508`; the correct in-arrears instalment of 5,009,635.23 gives
1.2743. A reverses the direction — an annuity due has a *smaller* instalment, so it flatters the
ratio; C denies that the denominator moved by 283,564; D is the `DSCR` after the separate
`CFADS`-definition finding, on the correct instalment.

**MCQ 13.2-B `[13.2.2 · Evaluation]`** A model audit returns 34 findings — 3 Class 1, 7 Class 2,
24 Class 3 — and the status report says "31 of 34 closed, 91.2 %". The correct challenge is:
- A. the count is wrong
- B. the 91.2 % is driven by the 24 Class 3 findings, which are 70.6 % of the count and none of the impact; if the three open findings are the Class 1 findings, 0 % of the impact is closed ✅
- C. Class 3 findings should not be reported at all
- D. the audit should be re-run

*Rationale:* Findings are not commensurable, so a count-based metric measures the class that changes
nothing (13.2.2). A is not the defect; C loses the aggregate record that shows model hygiene; D
confuses a reporting failure with an audit failure.

**MCQ 13.2-C `[13.2.3 · Application]`** After correcting `CFADS` to 6,124,000, debt capacity at a
1.30× requirement over 12 years at 6 % (`AF` = 8.383844) is closest to:
- A. USD 41,171,123
- B. USD 39,494,354 ✅
- C. USD 42,000,000
- D. USD 51,342,661

*Rationale:* `6,124,000/1.30 = 4,710,769.23`, `× 8.383844 = 39,494,354`. A is Domain 10's capacity on
the *uncorrected* 6,384,000; C is the mandated amount, which the calculation rejects by 2,505,646;
D omits the coverage divisor entirely (`6,124,000 × 8.383844`).

**MCQ 13.2-D `[13.2.1 · Analysis]`** Which audit scope layer would have caught a model that
implements a valid amortisation the finance documents do not provide for?
- A. arithmetic — the formulae are checked
- B. structure — the schedule closes at zero
- C. documents — the model is tested against what the facility actually says ✅
- D. assumptions and provenance — the inputs are traced

*Rationale:* An annuity due computes correctly and closes at zero, so layers A and B both pass
(13.2.3); only conformance to the document detects it. D would trace the rate and tenor, both of
which are right.

**MCQ 13.2-E `[13.2.3 · Evaluation]`** Two Class 1 findings are almost identical in size: the
definitional and convention findings together resize the facility by **2,505,646**, and the funding
plan omits the **2,504,818** debt-service reserve — a difference of 828. The credit paper can lead
with one. Which is the more decisive finding, and why?
- A. the resize, because 2,505,646 is the larger number
- B. the unfunded reserve, because a coverage shortfall is negotiated between the parties whereas a funding plan that does not fund a mandatory reserve produces a failed condition at first drawdown or a first-year breach ✅
- C. neither, because at 828 apart they very nearly cancel
- D. the resize, because `CFADS` is the model's most important line

*Rationale:* Class 1 findings are ranked by the kind of consequence they create, not by magnitude:
one changes a negotiating position, the other creates an event the documents will treat as a failure
(13.2.3). A ranks by size where the two sizes are indistinguishable anyway. C is the netting fallacy
13.2.2 forbids, applied more crudely still — these findings do not offset, they are simply similar in
scale, and one of them is not a cash amount at all. D substitutes a slogan for the consequence test.

**MCQ 13.2-F `[13.2.2 · Comprehension]`** A colleague new to model audit asks why the auditor will
not report a single "findings closed" percentage. The best short explanation is:
- A. the auditor prefers not to be measured on a number it does not control
- B. the three classes are not commensurable, so one percentage adds findings that change the transaction to findings that change nothing; progress has to be measured by impact closed ✅
- C. the percentage is never final, because new findings keep arriving until the model is frozen
- D. the percentage understates progress, because Class 3 findings are the quickest to close

*Rationale:* The objection is to combining unlike things, and the remedy is class-weighted closure
(13.2.2). A imputes a motive where there is a measurement defect; C describes a practical nuisance
that would apply equally to a class-weighted metric; D inverts the direction — Class 3 findings being
quick and numerous is exactly what makes a count *overstate* progress, as "31 of 34, 91.2 %" does.

**MCQ 13.2-G `[13.2.4 · Evaluation]`** A finding moves year-one `CFADS` by **180,000** —
**0.0359×** of `DSCR`, against the stated materiality threshold of 0.01×, about **50,096** of annual
`CFADS` — and the sponsor disputes it, arguing the auditor has misread the clause. The auditor does not
agree. The deal team proposes recording it as "closed — no adjustment required", so that the register
shows nothing outstanding at close. The soundest treatment is to record it:
- A. as closed, since no adjustment was made and nothing further is to be done
- B. as **accepted** rather than corrected, with its quantified effect, the written rationale, the name of
  the person accepting it, and its disposition listed on the auditor's final letter ✅
- C. as withdrawn, because a finding the auditor cannot substantiate against the sponsor's reading of the
  clause is not a finding
- D. as escalated to the credit committee, which alone can accept a finding the parties cannot resolve

*Rationale:* Findings are sometimes accepted rather than corrected, and an accepted finding with a
written rationale is professional whereas the same finding recorded as "closed" is not (13.2.4). A
produces a register that misdescribes the position to every later reader — the defect Case study B shows
costs money two years afterwards. C lets the party whose reading is disputed close the item. D is the
right route for a Class 1 finding, but this one changes no conclusion — `DSCR` after it is **1.2384**,
still clear of the 1.20 covenant, so it is Class 2 — and routing it upwards substitutes a governance
step for the record the register exists to hold.

### Self-check — KA 13.2

1. *Why is a finding count uninformative?* — Findings are not commensurable; Class 3 dominates the
   count and contributes nothing to impact.
2. *Which audit layer holds the expensive errors, and why?* — Documents and assumptions: a model can
   be arithmetically perfect and structurally sound while implementing something the facility does
   not provide for.
3. *When is a Class 1 finding closed?* — When corrected, verified by the auditor, and followed
   through into every dependent output, paper and base case.

---

## Knowledge Area 13.3 — Conditions precedent and documentation

*Topics: 13.3.1 the condition set and its categories · 13.3.2 the conjunction with a critical path ·
13.3.3 float, criticality risk and where to spend · 13.3.4 the close-cost budget and sources and
uses · 13.3.5 waivers, long-stop dates and post-close undertakings.*

### 13.3.1 The condition set and its categories

**Definition.** A **condition precedent** is a requirement that must be satisfied — and evidenced
to the lenders' satisfaction — before an obligation arises, most importantly before first drawdown.
Domain 5 (KA 5.A.2) established that the bankability conditions become the CP schedule, that CPs
are ordered by dependency rather than importance, that sponsor CPs behave differently from
third-party CPs, and that a waived CP is not a satisfied CP. This Knowledge Area computes what that
schedule costs.

The condition set divides into five practical groups, and the division matters because each group
fails differently. **Corporate and authority** conditions — board and shareholder resolutions,
constitutional documents, powers of attorney, authorised-signatory evidence — are within the
sponsor group's control, fail through inattention rather than uncertainty, and are therefore
forgivable in timetable terms. **Documentary** conditions — executed finance, security, project and
direct agreements, and the legal opinions that support them — fail through negotiation, and their
duration is a function of how many parties must agree. **Third-party** conditions — permits,
licences, land instruments, offtaker and grantor consents, letters of credit from banks that are
not the lenders — are outside the group's control and are the standard source of close slippage.
**Adviser and process** conditions — the diligence reports of KA 13.1, the model audit of KA 13.2,
final credit approvals — have durations the transaction can influence but not compress at will.
And **financial** conditions — equity funded or committed on acceptable terms, accounts opened,
insurance placed and endorsed, fees paid, the base-case model agreed and locked — are the ones most
often discovered late, because they look administrative.

The professional discipline that this Knowledge Area exists to install: **a CP schedule is a
dependency network, and it must be built and read as one.** A close timetable drawn as a single bar
labelled "conditions precedent — 20 weeks" contains no information about what will actually make
the project late.

### 13.3.2 The conjunction with a critical path

Close occurs when **every** condition is satisfied. That is a conjunction, and conjunctions of
uncertain durations behave in a way that timetables systematically misrepresent: the close date is
the **maximum** of the chains' realised durations, and the expected maximum is greater than the
maximum of the expectations. The gap is not a rounding error; it is often the whole of the
contingency a transaction should have carried and did not.

**Worked example 13.3.2 — when will Kestrel actually close?**

1. **Setup.** Kestrel's 40-odd conditions resolve into five dependency chains. Each chain's base
   duration is the sum of its links; each carries an independent risk of a single slip, estimated
   from the sponsor's own record of comparable closings. Cost of delay **124,133.33 per week**.

   | Chain | Contents | Base duration | Slip probability | Slip if it occurs |
   |---|---|---|---|---|
   | **A** | Board and shareholder resolutions → shareholders' agreement amendment → equity commitments and credit support | 9 weeks | 0.20 | 3 weeks |
   | **B** | Land instrument registration → construction permit → abstraction licence | 18 weeks | 0.35 | 6 weeks |
   | **C** | Diligence reports (12-week envelope) → model audit → final credit approvals | 20 weeks | 0.30 | 4 weeks |
   | **D** | Finance documents agreed → security documents and perfection → legal opinions | 17 weeks | 0.25 | 5 weeks |
   | **E** | Insurance adviser's report → placement and lender endorsements | 7 weeks | 0.15 | 2 weeks |

2. **Formula.** Base close = `max(base durations)`. Each chain's expected duration =
   `base + probability × slip`. The close date = `max` over chains of the realised duration;
   `E[close] = Σ over all 32 outcomes of (probability × maximum duration)`. Cost of slippage =
   `(E[close] − base close) × cost of delay`.
3. **Substitution.** Base close = **20 weeks** (chain C). Expected durations: A 9.60, B 20.10,
   C 21.20, D 18.25, E 7.30. Chains A and E can never bind, since even their slipped durations
   (12 and 9 weeks) fall short of 20; the maximum is therefore decided by B, C and D over eight
   outcomes.
4. **Result.** **`E[close] = 22.4075 weeks`** against a **max of expectations of 21.2000**. The
   close-date distribution is **20 weeks with probability 34.125 %**, **22 weeks with 11.375 %** and
   **24 weeks with 54.500 %**. Expected slip **2.4075 weeks**, costing
   `2.4075 × 124,133.33 =` **USD 298,851**. Chain C's own expected slip of 1.2 weeks costs
   **USD 148,960**, so the **conjunction premium is USD 149,891** — the expected cost of delay is
   **2.0062×** the cost of the critical chain's own expected slip.
5. **Interpretation.** Three results, in ascending order of usefulness. The first is the one that
   changes behaviour: **the probability that this transaction closes on its own stated date is
   34.125 %** — and every individual chain owner can truthfully report that their chain is more
   likely than not to hold its date. That is the arithmetic behind the observation that project
   financings are almost never early and usually late, and it is not pessimism, incompetence or
   optimism bias; it is what a conjunction does. The second is the pricing consequence: **the
   expected cost of the CP schedule is just over double the cost of the critical chain's expected
   slip**, because any of three near-critical chains can become the binding one. A transaction that
   provisions for delay by looking at the critical path provisions for roughly half its exposure,
   and 149,891 is what that omission is worth here. The third is the reporting consequence: the
   distribution is **bimodal** — 34.1 % at 20 weeks, 54.5 % at 24 weeks, and almost nothing in
   between — because a slip, if it happens, is a discrete four- to six-week event rather than a
   smooth drift. The right output for a board is therefore not "expected close 22.4 weeks" but
   **"one chance in three of 20 weeks, and better than one chance in two of 24"**, which is a
   sentence people can plan against. The honest caveats are two: the slips are modelled as
   independent, and they are not — a slow grantor delays the permit chain and the consent inside the
   documentary chain together, which makes the true `E[close]` *later* than 22.4075 and the
   probability of the base date *lower* than 34.125 %; and single-slip modelling understates the
   right tail, since a permit that slips six weeks can slip twelve. Both errors point the same way,
   which is the useful thing to know about them.

> **Fig 13.3.1 — Kestrel's conditions-precedent conjunction.** Horizontal chain chart: five chains
> down the left (C 20 weeks, B 18, D 17, A 9, E 7), each drawn as a blue duration bar with its
> float shaded grey to the 20-week base close and its possible slip shown beyond, in crimson where
> float cannot absorb it. Annotations per chain: float **0 / 2 / 3 / 11 / 13** weeks, slip
> probability and size, risk weight (probability × slip) **1.20 / 2.10 / 1.25 / 0.60 / 0.30**, and
> the exposed slip **4 / 4 / 2 / 0 / 0** weeks. Solid ink vertical at the 20-week base close;
> dashed crimson vertical at **E[close] = 22.4075 weeks**. Inset panel gives the close-date
> distribution — **20 weeks 34.125 %**, **22 weeks 11.375 %**, **24 weeks 54.500 %** — and the
> header states the expected slip cost of **298,851** against chain C's own **148,960**, a
> conjunction premium of **149,891**. Source: PCI original. Alt text: five horizontal condition
> chains with float and slip exposure marked, beside a close-date distribution showing only a one
> in three chance of closing on the base date.

### 13.3.3 Float, criticality risk and where to spend

The chart above contains the management answer, and it is counter-intuitive. **Float is real and
computable**: chain A has 11 weeks of it and a 3-week slip, so its slip is absorbed entirely and
costs nothing; chain E has 13 weeks of float against a 2-week slip and likewise cannot delay close.
Effort spent accelerating either returns zero, however loudly the sponsor's corporate secretariat
is being chased.

**Criticality risk, not criticality, ranks the work.** Weighting each chain by
`slip probability × slip size` gives **B 2.10**, **D 1.25**, **C 1.20**, A 0.60 and E 0.30 — and
adjusting for float removes A and E entirely. The nominal critical path is chain C. The chain most
likely to *become* critical is **B**, the permit and land chain, which sits two weeks inside the
base date and carries the largest slip risk in the register.

**Worked example 13.3.3 — what is it worth to de-risk a chain that is not critical?**

1. **Setup.** The sponsor can put a dedicated permitting team, local counsel and a pre-agreed
   submission protocol behind chain B, and estimates this reduces its slip probability from
   **0.35 to 0.10**. Nothing else changes. Cost of delay 124,133.33 per week.
2. **Formula.** Recompute `E[close]` over the same outcome set with chain B's probability changed;
   the value of the intervention is the reduction in expected slip cost.
3. **Substitution.** `E[close]` with `p_B = 0.10`, then `(E[close] − 20) × 124,133.33`, compared
   with the base 298,851.
4. **Result.** `E[close]` falls from **22.4075** to **21.7950 weeks**; expected slip cost falls from
   **298,851** to **USD 222,819.33**; the intervention is worth **USD 76,031.67**. The probability
   of closing on the base 20-week date rises from **34.125 %** to **47.250 %**, and the probability
   of the 24-week outcome falls from **54.500 %** to **37.000 %**.
5. **Interpretation.** The highest-value acceleration available on this transaction is on a chain
   that is **not** the critical path, and no critical-path analysis would have found it — which is
   the practical case for modelling the conjunction rather than the longest bar. Two further
   readings. The 76,031.67 is a **budget**, and it is the number the sponsor should compare a
   permitting adviser's fee against; below it the intervention pays, above it, it does not, and
   that is a far more useful procurement position than "permits are important". And the shift in
   the *distribution* is worth more to a board than the shift in the mean: moving the probability of
   the base date from roughly one in three to roughly one in two changes what can honestly be told
   to an offtaker, a contractor holding a price and an equity committee with a quarter-end. The
   caution is that reducing `p_B` to 0.10 is itself an estimate, and the honest way to present the
   result is as a sensitivity — *if* the intervention halves the risk it is worth about 38,000, *if*
   it removes two-thirds of it, about 76,000 — rather than as a single figure implying a precision
   the parameters do not support.

### 13.3.4 The close-cost budget and sources and uses

Close costs are the transaction's own price, and they are routinely provided for by a rule of thumb
in the financial model — a percentage of the facility labelled "fees" — which is where the error
enters. The discipline is a **line-itemised close-cost budget, reconciled to the sources-and-uses
statement**, and it separates naturally into what is payable to lenders and what is not.

**Worked example 13.3.4 — Kestrel's close-cost budget, and the hole it found.**

1. **Setup.** The diligence fees of KA 13.1 plus the financing, sponsor-side and perfection costs.
   Domain 6's funding envelope (KA 6.2.1) provided **840,000** for "arrangement and financing fees"
   at 2.0 % of the facility and **1,800,000** for capitalised development costs, both funded at
   close.
2. **Formula.** Total close costs = Σ line items. Share of debt raised = total ÷ debt. Reconciliation
   = the envelope's provision − the itemised total.
3. **Substitution and result.**

   | Close-cost item | USD |
   |---|---|
   | Lenders' technical adviser | 260,000 |
   | Lenders' legal counsel | 420,000 |
   | Model auditor | 180,000 |
   | Insurance adviser | 60,000 |
   | Environmental and social consultant | 240,000 |
   | Market adviser | 190,000 |
   | Tax adviser | 150,000 |
   | *Diligence subtotal (KA 13.1)* | *1,500,000* |
   | Arrangement fee, 1.20 % of 42,000,000 | 504,000 |
   | Agency and account-bank set-up | 40,000 |
   | Sponsor's legal counsel | 380,000 |
   | Sponsor's financial adviser, success fee 0.45 % | 189,000 |
   | Security perfection, registration and notarial costs | 96,000 |
   | **Total close costs** | **2,709,000** |

   Total close costs are **6.45 % of debt raised** and **4.515 %** of the 60,000,000 envelope. Only
   **544,000** — the arrangement and agency fees — is payable to the lenders; **2,165,000** goes to
   third parties and the sponsor's own advisers. Against the envelope's provision of
   `840,000 + 1,800,000 = 2,640,000`, the budget is **69,000 short**: the fee line has
   **296,000 spare** (2.0 % of the facility against 544,000 of actual lender fees) while the
   development-cost line is **365,000 short** of the 2,165,000 it must carry.
4. **Interpretation.** Start with the number nobody expects: **6.45 % of debt raised**. That is not
   an error and it is not extravagance — it is what it costs to project-finance a 60,000,000 asset,
   and it is the strongest quantitative argument in this domain for why small projects are
   aggregated, standardised or financed on balance sheet instead. Split the total into a **fixed
   component** — the seven adviser fees, the sponsor's counsel, agency set-up and perfection,
   **2,016,000**, which barely varies with deal size — and a **proportional component** of
   **1.65 %** (the 1.20 % arrangement fee plus the 0.45 % success fee). Close cost as a share of
   debt is then `2,016,000 ÷ D + 1.65 %`, which is **6.45 %** at 42,000,000, **2.994 %** at
   150,000,000 and **2.130 %** at 420,000,000. A facility must reach about
   **USD 149,333,333** before total close costs fall below **3.0 %** of debt raised — an upper bound
   on the scale economy, since the fixed component does grow with complexity, but the shape is
   right and the implication is real. Second, the **reconciliation is the finding**. A 2.0 %
   "financing fees" line implicitly assumed transaction costs were the arranger's fee; they are
   20 % of it. The 69,000 shortfall is small in itself and must be absorbed by the balancing
   contingency line, taking it from Domain 6's **3,645,403** to **3,576,403**, or from 7.59 % to
   **7.45 %** of the EPC price — still inside the band a lender would expect, so no financing
   consequence follows here. But the *mechanism* is the one that hurts on larger transactions, and
   there is a second-order effect the model must pick up: a cost moved from a spend-profile line to
   a close-funded line carries interest for the whole construction period rather than part of it, so
   the reallocation also adds capitalised interest, which the same balancing line must absorb. That
   is why a close-cost budget belongs **inside** the model rather than beside it. Third, the
   **effective cost of debt**. Netting the 544,000 of lender fees from proceeds against the
   unchanged instalment of 5,009,635.23 gives an all-in rate of **6.2386 %** on the facility;
   netting the whole 2,709,000 gives **7.2376 %** — **123.8 basis points** above the 6.00 % headline.
   Both figures are correct and they answer different questions, which is exactly the discipline
   Domain 9 (KA 9.3.1) established for comparing tranches: **6.2386 % is the cost of the lender's
   money and 7.2376 % is the cost of raising it**, and only costs that *differ between the routes
   being compared* belong in a route comparison. Quoting the second as though the lender charged it
   is as misleading as quoting the headline 6.00 % as though closing were free.

### 13.3.5 Waivers, long-stop dates and post-close undertakings

Three instruments handle a condition that will not be satisfied in time, and all three are priced
decisions rather than administrative fixes.

A **waiver** is the lenders' agreement to close without a condition, usually for a fee and often
with a compensating term. Domain 5's rule stands — a waived CP is not a satisfied CP — and the
professional requirement is that every waiver carries **a deadline, a named owner, evidence
requirements and a stated consequence of non-satisfaction**. Case study B shows the same instrument
used well and badly on one transaction.

A **long-stop date** (or availability-period expiry) is the date after which commitments lapse if
close has not occurred. Its economic content is that it converts slippage into a **cliff**: up to
the long-stop, delay costs the cost of delay; past it, the transaction needs re-approval, and
re-approval prices at today's market rather than the market of the mandate. A leader should know
the distance between `E[close]` and the long-stop in the same terms as the coverage headroom of
Domain 10 — as a number of weeks, on the dashboard. Kestrel's `E[close]` of 22.4075 weeks against a
32-week long-stop leaves **9.5925 weeks** of margin, and the 24-week outcome that carries
54.5 % probability leaves 8.

A **post-close undertaking** converts a condition into a covenant: the obligation survives close
with a deadline and, on breach, the consequences of Domain 10's covenant regime. This is the right
answer for conditions that are genuinely mechanical and merely slow — a registration awaiting a
registry, a final certificate awaiting an inspector — and the wrong answer for anything whose
outcome is uncertain, because it converts a diligence question into a default risk.

### AI in this KA

**Where it earns its place.** CP registers are long, repetitive and derived from documents, which
makes three uses strong. **Extraction** of the condition list from the facility agreement's
schedule into a register with clause references — the version-controlled starting point most
transactions build by hand. **Dependency and duration tracking** — maintaining the register as
evidence arrives, flagging conditions whose evidence is stale or whose owner has not reported, and
recomputing the conjunction arithmetic of 13.3.2 whenever a duration estimate moves, which is the
only way that arithmetic stays current inside a live close. And **evidence completeness checks**
against each condition's stated documentary requirement.

**Where it must not go.** It must not determine that a condition is satisfied. Satisfaction is a
determination the lenders make, on advice, against a documentary standard, and a machine's view
that a condition "appears satisfied" has no standing and considerable capacity to mislead a
transaction into a certificate it cannot support. It must not draft or sign the certificate of
satisfaction. It must not decide whether a condition is waivable, or what a waiver should cost —
that is a negotiation informed by counsel. And it must not generate the slip probabilities: `p` and
the slip sizes come from an organisation's own closing record, and a plausible machine estimate
would put a fabricated number at the centre of the close timetable.

**Verification, concretely.** The extracted register is reconciled clause by clause against the
schedule by a named person before it is used, because a missing condition is invisible in a
register that looks complete. Every satisfaction determination is recorded with the human who made
it and the evidence relied on. The conjunction arithmetic is recomputed and reviewed at each
milestone, and the estimated slip parameters are recorded with their basis and their source
transaction count — so that the register carries, in the open, how much evidence sits behind its
own forecast.

### Key terms — KA 13.3

| Term | Meaning |
|---|---|
| **Condition precedent (CP)** | Requirement satisfied and evidenced before an obligation arises, principally first drawdown. |
| **CP chain** | A dependency sequence of conditions; its duration is the sum of its links. |
| **Conjunction premium** | The excess of the expected close slip cost over the cost of the critical chain's own expected slip. |
| **Float** | Weeks by which a chain may over-run before it affects close; absorbs slip at no cost. |
| **Criticality risk** | `slip probability × slip size`, net of float; ranks where acceleration is worth buying. |
| **Close-cost budget** | Line-itemised transaction cost, reconciled to sources and uses; fixed component plus a proportional fee rate. |
| **Long-stop date** | Date after which commitments lapse; converts slippage into re-approval at today's market. |
| **Post-close undertaking** | A condition converted into a dated covenant surviving close. |

### Sample MCQs — KA 13.3

**MCQ 13.3-A `[13.3.2 · Analysis]`** Five CP chains have expected durations of 9.60, 20.10, 21.20,
18.25 and 7.30 weeks against a 20-week base close. The expected close date is:
- A. 21.2000 weeks — the longest chain's expected duration
- B. 22.4075 weeks — the expected maximum of the chains ✅
- C. 20.0000 weeks — the base close, since each chain is more likely than not to hold its date
- D. 76.4500 weeks — the sum of the chains

*Rationale:* Close is a conjunction, so the date is the expected *maximum*, which exceeds the
maximum of the expectations (13.3.2). A takes the maximum of the expectations, the standard error;
C mistakes each chain's individual likelihood for the joint one — the probability of the base date
is 34.125 %; D adds chains that run in parallel.

**MCQ 13.3-B `[13.3.3 · Evaluation]`** Chain B (18 weeks, slip 6 weeks at probability 0.35) is not
the critical path; chain C (20 weeks, slip 4 at 0.30) is. Reducing chain B's slip probability to
0.10 moves `E[close]` from 22.4075 to 21.7950 weeks at a cost of delay of 124,133.33 a week. The
intervention is worth:
- A. nothing — chain B is not on the critical path
- B. USD 76,031.67, and it is the highest-value acceleration available ✅
- C. USD 744,800 — six weeks of delay avoided
- D. USD 298,851 — the whole expected slip cost

*Rationale:* `(22.4075 − 21.7950) × 124,133.33 = 76,031.67` (13.3.3). A is the critical-path fallacy
the conjunction exists to correct; C prices the full slip as if it were certain and unavoidable; D
is the total expected slip cost, which the intervention reduces but does not eliminate.

**MCQ 13.3-C `[13.3.4 · Application]`** Close costs total 2,709,000 on a 42,000,000 facility funding
a 60,000,000 envelope, of which 544,000 is payable to lenders. Close costs as a share of debt raised
are:
- A. 4.515 %
- B. 6.45 % ✅
- C. 1.295 %
- D. 2.00 %

*Rationale:* `2,709,000/42,000,000 = 6.45 %`. A divides by the 60,000,000 envelope rather than the
debt; C counts only the fees payable to lenders; D is the model's original rule-of-thumb provision,
which the itemised budget shows to be 20 % of the true figure.

**MCQ 13.3-D `[13.3.4 · Analysis]`** With a fixed close-cost component of 2,016,000 and a
proportional component of 1.65 % of debt, the facility size at which total close costs fall to 3.0 %
of debt raised is closest to:
- A. USD 67,200,000
- B. USD 149,333,333 ✅
- C. USD 122,181,818
- D. USD 90,300,000

*Rationale:* `2,016,000/(0.030 − 0.0165) = 2,016,000/0.0135 = 149,333,333`. A divides the fixed
component by 3.0 % and ignores the proportional component; C divides by 0.0165; D applies 3.0 % to
the wrong base.

**MCQ 13.3-E `[13.3.5 · Recall]`** A condition converted into a post-close undertaking is
appropriately handled that way when:
- A. its outcome is uncertain and diligence could not resolve it
- B. it is mechanical and merely slow, so the only open question is timing ✅
- C. the sponsor does not wish to disclose it
- D. the lenders have waived it without a deadline

*Rationale:* Post-close undertakings convert timing risk into a dated covenant; they convert
*outcome* risk into default risk, which is why A is the case for not closing rather than for an
undertaking (13.3.5). D describes the defect Case study B prices.

**MCQ 13.3-F `[13.3.4 · Evaluation]`** Kestrel's headline rate is 6.00 %; netting the 544,000 of
lender fees from proceeds gives an all-in **6.2386 %**, and netting the whole 2,709,000 close-cost
budget gives **7.2376 %**. The sponsor is choosing between this bank facility and a bond alternative
that would require substantially the same diligence, sponsor-side legal and perfection costs. Which
rate belongs in that comparison?
- A. 6.00 %, because the other two include amounts that are not interest
- B. 6.2386 %, because only costs that differ between the routes discriminate between them, and the diligence and sponsor-side costs are common to both ✅
- C. 7.2376 %, because it is the full cost of raising the money and the more conservative figure to put to a board
- D. all three, because each is correct and answers a different question

*Rationale:* A route comparison is decided by the costs that differ, which is Domain 9's rule
(KA 9.3.1) applied to close costs: the lender-payable 544,000 differs between a bank and a bond
route, the 2,165,000 of adviser and perfection cost largely does not (13.3.4). C is defensible and
commonly seen, but conservatism is not a selection rule — loading both routes with the same
2,165,000 changes the level and not the ranking, while inviting the reader to treat a sunk common
cost as a reason to prefer one lender over another. A is wrong because a fee paid to the lender for
its money is part of the price of that money. D is true as a general statement and evades the
question asked, which is a decision.

**MCQ 13.3-G `[13.3.2 · Evaluation]`** Kestrel's conjunction gives a base close of **20 weeks**,
`E[close]` of **22.4075 weeks**, and a close-date distribution of **34.125 %** at 20 weeks,
**11.375 %** at 22 and **54.500 %** at 24. The EPC contractor is holding a price and the grantor is
sequencing its own approvals; both have asked for the close date. The soundest thing to tell them is:
- A. 20 weeks — the base date every chain owner is committed to, because publishing a later date is how a
  later date is achieved
- B. the distribution — roughly one chance in three of 20 weeks and better than one in two of 24 — because
  a single date is a forecast the transaction knows to be 34 % likely, the counterparties' own decisions
  turn on which outcome they should plan for, and the modelled independence of the chains makes even that
  distribution optimistic ✅
- C. 22.4075 weeks, the expected value, because it is the unbiased single figure and a single figure is
  what was asked for
- D. 24 weeks, the most likely single outcome, because under-promising protects the transaction and the
  parties relying on it

*Rationale:* The distribution is bimodal — **88.625 %** of the probability sits at 20 or 24 weeks and
almost none near the mean — so C is arithmetically respectable and describes an outcome that will
almost certainly not occur. A states as a plan a date known to be 34.125 % likely. D is defensible as a
commitment and conceals a one-in-three chance of being four weeks early, which is exactly what a
contractor holding a price and a grantor sequencing approvals need to know; it also presents as near
certain a figure whose own basis — independent chains, one slip each — understates the right tail
(13.3.2).

**MCQ 13.3-H `[13.3.1 · Comprehension]`** Which restatement best captures why a close timetable treats
**third-party** conditions differently from **corporate and authority** conditions?
- A. third-party conditions are more numerous, so they consume more of the timetable
- B. the two groups differ in who controls satisfaction: corporate conditions sit inside the sponsor group
  and fail through inattention, so effort recovers them, whereas third-party conditions sit with
  permitting authorities, offtakers and grantors and can be influenced but not compressed at will ✅
- C. third-party conditions matter more to the lenders, so they are evidenced to a higher standard
- D. corporate conditions may be waived by the lenders whereas third-party conditions may not

*Rationale:* The five groups are distinguished by **how each fails**, and the corporate/third-party
distinction is one of control rather than of importance or of evidentiary standard (13.3.1) — which is
why third-party conditions are the standard source of close slippage, and why chain B, the permit and
land chain, carries the largest slip risk in Kestrel's register. A may be true of a given transaction
and is not the reason. C confuses a condition's ownership with its evidence requirement. D invents a
rule: waiver is a lenders' decision available in principle to any condition, and a waived CP is not a
satisfied CP whichever group it came from.

### Self-check — KA 13.3

1. *Why is the expected close date later than the longest chain's expected duration?* — Close is a
   conjunction: the expected maximum exceeds the maximum of the expectations, here by 1.2075 weeks
   and 149,891.
2. *How is acceleration effort ranked?* — By criticality risk (slip probability × slip size) net of
   float, not by position on the critical path.
3. *What two questions does the effective cost of debt answer differently?* — What the lender's
   money costs (6.2386 % on lender fees alone) and what raising it costs (7.2376 % on the full
   close-cost budget).

---

## Knowledge Area 13.4 — Syndication and financial close

*Topics: 13.4.1 the three routes to a syndicate · 13.4.2 the fee architecture and the arranger's
yield · 13.4.3 the failed sell-down · 13.4.4 market flex · 13.4.5 signing, close and funds flow.*

### 13.4.1 The three routes to a syndicate

**Definition.** **Syndication** is the process by which the arranging bank or banks distribute a
facility to other lenders, so that no single institution holds a position larger than its appetite
or its limits allow. Three routes, distinguished entirely by **who carries the risk that the market
does not turn up**:

- **Underwritten.** One or more banks commit the full amount and then sell down. The borrower has
  certainty of funds from the mandate; the arranger carries the distribution risk and charges for
  it (13.4.3 prices what that risk costs when it goes wrong).
- **Best-efforts.** The arranger undertakes to use reasonable efforts to raise the amount but does
  not commit it. Cheaper, and the borrower carries the risk that the book does not fill — which in
  a project financing means the risk that close does not happen at all, after the whole close-cost
  budget has been spent.
- **Club.** A small group of banks, assembled before documentation, each committing its final hold
  directly. No distribution risk, no underwriting fee, and — the point most often missed — no
  arranger with an economic interest in flexing terms, because nobody has a position to sell.

The choice is a trade between **certainty of funds**, **cost** and **flexibility**, and the
sponsor's decision variable is usually the first. A transaction with a long-stop date, a contractor
holding a price and a grantor's timetable cannot tolerate a book that does not fill, and will pay
underwriting economics for certainty. A repeat sponsor with relationship banks and no hard deadline
will club the deal and keep the fee. Kestrel's 42,000,000 sits at the size where either works, and
this Knowledge Area follows it as an underwritten facility because that is the structure whose
arithmetic must be understood.

### 13.4.2 The fee architecture and the arranger's yield

Fees in a syndicated facility are not one number. The standard architecture, and the reason for
each part:

| Fee | Paid to | Purpose |
|---|---|---|
| **Praecipium (arrangement fee proper)** | The arranger, exclusively | Structuring, documentation and underwriting risk; not shared with participants |
| **Participation (pool) fee** | Every lender pro rata on final allocations | Compensation for committing capital |
| **Underwriting fee** | The underwriter(s) | Explicit price of the commitment where separated from the praecipium |
| **Commitment fee** | Every lender, periodically | The undrawn balance's carrying cost (Domain 9, KA 9.2.4) |
| **Agency fee** | The facility and security agents | Ongoing administration; annual, not a close cost |

**Worked example 13.4.2 — who earns what on Kestrel's syndication.**

1. **Setup.** A total arrangement fee of **1.20 %** on 42,000,000, of which **0.25 %** is
   praecipium retained by the arranger and the balance is a participation pool paid pro rata on
   final allocations. The arranger's target final hold is **15,000,000**; two participants take
   **13,500,000** each.
2. **Formula.** Total fee = facility × 1.20 %. Praecipium = facility × 0.25 %. Pool = total −
   praecipium, paid at `pool ÷ facility` on each lender's allocation. Fee yield = fee earned ÷
   final hold.
3. **Substitution.** `42,000,000 × 1.20 % = 504,000`; `× 0.25 % = 105,000`; pool `399,000`, a rate
   of **0.95 %**. Arranger `105,000 + 0.95 % × 15,000,000`; each participant
   `0.95 % × 13,500,000`.
4. **Result.** Arranger **USD 247,500**, a fee yield of **1.650 %** on its 15,000,000 hold. Each
   participant **USD 128,250**, a yield of **0.950 %**. Total `247,500 + 2 × 128,250 = 504,000` ✓.
   The arranger earns **70.0 basis points** more per dollar held than the banks it sold to.
5. **Interpretation.** The 70 basis points is the price of two things participants do not do:
   structure and document the transaction, and stand behind the amount before anyone else has
   agreed to it. Reading it as a margin is the mistake — it is a **risk premium on distribution**,
   and 13.4.3 shows how quickly it is consumed. Three practical consequences for a sponsor. The fee
   split is **not the sponsor's business but it is the sponsor's information**: a praecipium that is
   a large share of the total tells you the arranger is being paid mostly for underwriting, which
   in turn tells you how it will behave if the book is slow — it will reach for flex. The
   **allocation policy is negotiable**, and a sponsor with a view on which lenders it wants in its
   syndicate for the next twelve years (Domain 10's waiver arithmetic: a tight syndicate consents
   in weeks) should express that view before the book opens, not when allocations are announced.
   And **fee yield, not fee, is how a lender sees the transaction**: a participant asked to take
   13,500,000 at 0.950 % is comparing that yield against every other asset on its desk, which is
   why fees rise in a busy market even when margins do not move.

### 13.4.3 The failed sell-down

**Worked example 13.4.3 — what an underwriter loses when the market does not turn up.**

1. **Setup.** The arranger underwrites 42,000,000 with a target final hold of **15,000,000**,
   intending to place 27,000,000. The market takes only **20,000,000**. The arranger's internal
   charge for holding a project-loan position above its target hold is **90 basis points a year**
   over the facility's 12-year life. `AF(0.06, 12) = 8.383844`.
2. **Formula.** Residual hold = facility − amount placed. Excess = residual − target hold. Present
   value of the carry = excess × charge × `AF(r, n)`. Breakeven excess = the praecipium ÷
   (charge × `AF`).
3. **Substitution.** Residual `42,000,000 − 20,000,000 = 22,000,000`; excess
   `22,000,000 − 15,000,000 = 7,000,000`; carry `7,000,000 × 0.0090 × 8.383844`.
4. **Result.** Excess hold **7,000,000**; present value of the carry **USD 528,182.17** against
   total fees earned of 247,500 — a **net loss of USD 280,682.17** on a transaction the arranger
   won, documented and closed successfully. The excess hold that exactly consumes the
   **praecipium** is **USD 1,391,565**, or **3.31 %** of the facility; the excess that consumes
   the arranger's **entire fee** is **USD 3,280,118**.
5. **Interpretation.** An underwriter can lose money on a deal that closes, and the loss threshold
   is startlingly low: a **3.31 % over-hold wipes out the whole underwriting premium**. That single
   figure explains most of the behaviour a sponsor encounters in a syndication and should be read
   as intelligence rather than as bad faith. It explains why arrangers insist on **market flex**
   (13.4.4) — flex is the instrument that lets them re-price to fill the book rather than carry the
   position. It explains why they **soft-sound the market before committing**, and why a sponsor who
   restricts pre-mandate sounding pays for that restriction in the underwriting fee. It explains why
   arrangers prefer **clubs in difficult markets**, where the economics of underwriting simply do
   not work. And it explains why an arranger's own hold level is worth watching: a bank that has
   just over-held on three transactions is a bank whose appetite for the fourth is about limits, not
   about the project. The professional caution for a sponsor is not to over-read the arithmetic — the
   90 basis points is an internal capital charge, not a cash cost, and a bank may be entirely content
   to hold a good project-finance asset at scale. But when it is not, this is the arithmetic behind
   its position, and a sponsor negotiating flex without knowing it is negotiating blind against
   someone who does.

### 13.4.4 Market flex

**Definition.** **Market flex** is the arranger's contractual right, granted in the mandate or
commitment letter, to change specified terms — most commonly to increase the margin or fees, and
sometimes to shorten tenor, alter amortisation or reallocate between tranches — to the extent
necessary to achieve a successful syndication. It is normally capped, sometimes ordered (fees
before margin), and occasionally subject to a reverse flex that returns the benefit to the borrower
if the book is oversubscribed.

**Worked example 13.4.4 — what flex is worth, and what it costs.**

1. **Setup.** Kestrel's mandate grants flex of up to **50 basis points** on the margin. Facility
   42,000,000, 12 years, base rate 6.00 %, `AF(0.06, 12) = 8.383844`, `CFADS` 6,384,000, `DSCR`
   covenant **1.20×**.
2. **Formula.** Present value of the flex, from the lenders' side, = facility × flex × `AF(r, n)`.
   Its effect on the borrower = the new instalment `facility ÷ AF(r_flexed, n)` and the resulting
   `DSCR`. The flex the covenant survives solves `CFADS ÷ 1.20 = facility ÷ AF(r*, n)` for `r*`.
3. **Substitution.** PV of flex `42,000,000 × 0.0050 × 8.383844`. Flexed:
   `AF(0.065, 12) = 8.158725`; instalment `42,000,000 ÷ 8.158725`; `DSCR = 6,384,000 ÷` that.
   Ceiling: `6,384,000 ÷ 1.20 = 5,320,000`; `AF` required `42,000,000 ÷ 5,320,000 = 7.894737`;
   solve for `r*`.
4. **Result.** The 50 basis points of flex is worth **USD 1,760,607** in present value —
   **3.4933×** the entire 504,000 arrangement fee. Exercised in full, the instalment rises by
   **138,228** to **5,147,862.98** and the base-case `DSCR` falls from **1.2743** to **1.2401**.
   The 1.20× covenant survives up to a rate of **7.1138 %**, a flex ceiling of **111.4 basis
   points**, so the contractual 50 points consumes **44.9 %** of the available headroom.
5. **Interpretation.** **Flex is the most valuable term in the mandate letter, and it is almost
   never the term sponsors negotiate hardest.** At 3.49 times the arrangement fee, a sponsor who
   trades 10 basis points of flex for a 25 basis-point fee reduction has given away roughly
   352,000 of present value to save 105,000 — and will describe the outcome as a fee win. The
   second reading is the covenant one, and it is the reason flex belongs in the financial model
   before the mandate is signed: **flex is a coverage event in disguise.** The fully-flexed
   `DSCR` of 1.2401 still clears the 1.20 covenant, so this transaction survives its own flex — but
   it does so with 44.9 % of its rate headroom consumed and nothing left for the interest-rate
   exposure Domain 11 (KA 11.3) priced separately, which is why the two must be stressed together
   rather than in sequence. The disciplined position a sponsor should hold: agree flex, **cap it at
   a level the model demonstrates the covenant survives with margin**, insist on ordering so fees
   flex before margin (a fee is a one-off, a margin is 8.38 years of annuity factor), require a
   **reverse flex** so that a strong book benefits the borrower, and require the arranger to
   evidence market conditions before exercising. What a sponsor must not do is treat flex as
   boilerplate, or model the base case only. And the honest note on the other side: an arranger
   without flex, facing the 13.4.3 arithmetic, will either not underwrite or will price the
   underwriting as though flex had already been exercised — so a sponsor who wins the flex
   negotiation outright may simply have moved the cost into the fee.

### 13.4.5 Signing, close and funds flow

The final mechanics, in the order they occur, and the specific way each is got wrong.

**Signing is not close.** Documents are executed; conditions may still be outstanding. The gap
between the two is where the long-stop date lives, and a transaction that announces "financial
close" on signing has misdescribed its own position — to its board, and sometimes to a market.

**Satisfaction and certification.** Each condition is evidenced to the agent, who confirms
satisfaction against the documentary standard. The document to insist on is a **numbered
certificate of satisfaction cross-referring to each condition and the evidence delivered**, because
it is the record that determines, later, what was and was not satisfied. Waived and deferred
conditions appear on it as such, never as satisfied (13.3.5).

**The funds flow.** A single statement showing every payment at close: sources by lender and by
sponsor, uses by payee, net movements per account, and the day's timing. Kestrel's day-one
requirement was **2,640,000** — fees and capitalised development costs, funded 70/30 as
**1,848,000** of debt and **792,000** of equity (Domain 6, KA 6.2.1) — which is precisely the figure
KA 13.3.4 showed must actually carry **2,709,000**. **The funds flow is where a close-cost budget
error becomes visible, and it becomes visible on the day it is too late to fix.** The disciplines:
reconcile the funds flow to the close-cost budget line by line before close, not on the day; confirm
account signatories and payment instructions through a verified channel; and have the agent, not the
sponsor's finance team, own the statement.

**Equity first, or pro rata.** Whether equity funds ahead of debt, alongside it or behind an
equity-support instrument is a structuring decision with real consequences for the sponsor's
exposure and the lenders' comfort (Domain 9, KA 9.1) — and whatever the answer, it must be the same
in the documents, the model and the funds flow. **Conditions subsequent and the first drawdown.**
Availability usually begins at close, but the first drawdown has its own conditions: the drawdown
notice, the certification of spend, the confirmation that no default has occurred. Domain 14 takes
the transaction from here.

### AI in this KA

**Where it earns its place.** Three uses. **Term-sheet and commitment-letter normalisation** across
several banks into one comparable table, which Domain 9 (KA 9.2) established and which extends
naturally to flex provisions, fee structures and conditions — a genuinely tedious comparison that
machines do well and humans do inconsistently. **Investor and allocation analysis**: which
institutions have taken comparable paper in this sector, tenor and jurisdiction, assembled from
disclosed transaction records, as a starting list for the book. And **funds-flow reconciliation**:
matching a close-cost budget line by line to a funds-flow statement and reporting the differences,
which is exactly the check that 13.4.5 says is done too late.

**Where it must not go.** It must not state market appetite or price as though it were a quote — an
indicative margin generated from historical transactions is a research output, and presenting one to
a sponsor as market feedback misrepresents to whoever relies on it. It must not draft or approve the
certificate of satisfaction, the funds flow or any payment instruction: close-day payment
instructions are the single highest-value fraud target in a project financing, and no automated
channel should originate or alter one. And it must not be used to decide allocations, which are a
relationship judgment with consequences over the facility's whole life.

**Verification, concretely.** Every normalised term-sheet field is checked against the source letter
by a named person before the comparison is used, because a mis-normalised flex cap is worth
1,760,607 here. Payment instructions are verified out of band, by voice, against a
previously-confirmed record — an operational control that no model output substitutes for. And the
funds-flow reconciliation is signed by the agent and the sponsor's finance director, whatever
produced the first draft.

### Key terms — KA 13.4

| Term | Meaning |
|---|---|
| **Underwritten / best-efforts / club** | Arranger commits and sells down · uses reasonable efforts only · pre-assembled group each taking its final hold. |
| **Praecipium** | The arranger's exclusive share of the arrangement fee; the price of structuring and underwriting. |
| **Participation (pool) fee** | Fee paid pro rata on final allocations to every lender. |
| **Final hold** | The position a lender intends to retain after syndication. |
| **Market flex** | Contractual right to change specified terms to achieve syndication; capped, often ordered, sometimes reversible. |
| **Signing vs financial close** | Execution of documents vs satisfaction of all conditions and availability of funds. |
| **Certificate of satisfaction** | Numbered record of each condition, its evidence and its disposition, including waivers. |
| **Funds flow** | The statement of every payment at close, reconciled to the close-cost budget. |

### Sample MCQs — KA 13.4

**MCQ 13.4-A `[13.4.2 · Application]`** A 42,000,000 facility carries a 1.20 % arrangement fee, of
which 0.25 % is praecipium; the balance is a pool paid pro rata on allocations. The arranger's final
hold is 15,000,000. It earns:
- A. USD 180,000
- B. USD 247,500 ✅
- C. USD 105,000
- D. USD 504,000

*Rationale:* `105,000 + 0.95 % × 15,000,000 = 247,500`, a yield of 1.650 % against participants'
0.950 %. A applies the full 1.20 % to the hold and omits the praecipium's exclusivity; C is the
praecipium alone; D is the whole fee, most of which is paid away to participants.

**MCQ 13.4-B `[13.4.3 · Evaluation]`** An arranger underwrites 42,000,000 with a 15,000,000 target
hold, places 20,000,000, and charges itself 90 basis points a year on the excess over 12 years
(`AF` = 8.383844). Against fees of 247,500 the outcome is:
- A. a profit of 184,500
- B. a loss of USD 280,682.17 — the 528,182.17 carry on a 7,000,000 excess exceeds the whole fee ✅
- C. a loss of 508,500, since the entire residual hold is charged
- D. break-even

*Rationale:* `7,000,000 × 0.0090 × 8.383844 = 528,182.17`; `247,500 − 528,182.17 = −280,682.17`
(13.4.3). A charges the carry for one year only (63,000); C charges the full 22,000,000 residual
rather than the excess over target hold.

**MCQ 13.4-C `[13.4.4 · Application]`** Fifty basis points of market flex on a 42,000,000 facility
over 12 years at 6 % (`AF` = 8.383844) is worth, in present value:
- A. USD 210,000
- B. USD 1,760,607 ✅
- C. USD 2,520,000
- D. USD 504,000

*Rationale:* `42,000,000 × 0.0050 × 8.383844 = 1,760,607`, some 3.49 times the arrangement fee. A is
one year of the flex, undiscounted; C sums twelve years without discounting; D is the arrangement
fee the sponsor negotiates instead.

**MCQ 13.4-D `[13.4.4 · Analysis]`** With `CFADS` of 6,384,000, a 42,000,000 facility over 12 years
and a 1.20× `DSCR` covenant, the margin increase the covenant just survives is:
- A. 50.0 basis points
- B. 111.4 basis points ✅
- C. 25.0 basis points
- D. unlimited, since the covenant is tested on `CFADS`

*Rationale:* Maximum instalment `6,384,000/1.20 = 5,320,000`, requiring `AF = 7.894737`, which
solves at 7.1138 % — 111.4 basis points above 6.00 %. A is the contractual flex cap, which consumes
44.9 % of that headroom; D forgets that debt service is the denominator and rises with the rate.

**MCQ 13.4-E `[13.4.5 · Recall]`** A transaction has executed all its documents but two third-party
consents remain outstanding. Its correct description is:
- A. financially closed
- B. signed, not closed — close occurs when every condition is satisfied and funds are available ✅
- C. closed subject to conditions subsequent
- D. in default

*Rationale:* Signing and close are distinct, and the gap is where the long-stop date operates
(13.4.5). C misuses a term for post-close obligations; D confuses an unsatisfied condition with a
breach of an existing obligation.

**MCQ 13.4-F `[13.4.4 · Evaluation]`** The arranger offers to cut the arrangement fee from 1.20 % to
0.95 % of the 42,000,000 facility — a certain saving of 105,000 — in exchange for lifting the market-
flex cap from 50 to 60 basis points. Twelve years, `AF(0.06, 12) = 8.383844`; the 1.20× covenant
survives a rate rise of 111.38 basis points. The recommendation is:
- A. accept — 105,000 is certain, and flex may never be exercised at all
- B. reject — the extra ten basis points of flex is worth 352,121.45 in present value, 3.3535 times the fee saved, and it takes the cap from 44.89 % to 53.87 % of the covenant headroom ✅
- C. reject — flex should not be granted at all, since it prices the sponsor's protection away
- D. accept, provided the fee reduction is credited in cash at close rather than netted from proceeds

*Rationale:* `42,000,000 × 0.0010 × 8.383844 = 352,121.45` against 105,000, so the trade would have
to be exercised with probability below `105,000 ÷ 352,121.45 =` **29.82 %** to break even — and
13.4.3 shows the arranger exercises precisely when the book is slow, which is the state in which the
sponsor can least afford the coverage loss (13.4.4). A is the defensible version of the argument and
is the one sponsors actually make; it fails because it prices a contingent claim at zero and ignores
the 111.38 basis points of headroom being consumed. C is not available — an arranger without flex
will either decline to underwrite or price the underwriting as if flex were already exercised. D
negotiates the mechanics of the wrong side of the trade.

**MCQ 13.4-G `[13.4.1 · Evaluation]`** Kestrel has a long-stop date, an EPC contractor holding a price
and a grantor's fixed timetable. An arranger offers a **best-efforts** mandate at a materially lower fee
than the underwritten alternative, observing that books for contracted water projects of this size rarely
fail to fill. The itemised close-cost budget is **2,709,000** — **6.45 %** of debt raised — of which only
**544,000** is payable to lenders. The soundest recommendation is to:
- A. accept best-efforts, because the fee saving is certain while the risk it transfers has a low
  probability
- B. reject best-efforts here: what it transfers to the borrower is the risk that close does not happen at
  all after the whole 2,709,000 has been spent, of which **2,165,000** is irrecoverable third-party and
  sponsor-side cost, and certainty of funds against a fixed timetable is what underwriting economics buy ✅
- C. accept best-efforts and manage the exposure by bringing the long-stop date forward
- D. club the facility instead, since a club carries no distribution risk and no underwriting fee, and no
  arranger then has an economic interest in flexing terms

*Rationale:* The three routes differ in **who carries the risk that the market does not turn up**, and a
transaction with a hard timetable is buying certainty (13.4.1). A prices a contingent claim at its
probability and ignores its size: the budget at risk is **5.375 times** the 504,000 arrangement fee, and
it is spent before the book is known to fill. C tightens the very constraint the sponsor is trying to
satisfy. D is the genuine alternative and the right answer for a repeat sponsor with relationship banks
and no hard date; it fails here because a club must be assembled before documentation and each bank
commits only its own final hold, so it does not deliver certainty of funds against a fixed date — and
the flex point, though correct, is a benefit the sponsor cannot collect if the group does not assemble in
time.

**MCQ 13.4-H `[13.4.2 · Comprehension]`** In a syndicated facility the **praecipium** is retained
exclusively by the arranger while the **participation fee** is paid pro rata on final allocations. Which
statement best explains the difference?
- A. the split is a market convention with no economic content, which is why it is not disclosed to
  borrowers
- B. the two fees pay for different things: the praecipium prices structuring, documentation and standing
  behind the whole amount before any other lender had agreed to lend, while the pool prices the capital
  each lender ultimately commits ✅
- C. the praecipium compensates the arranger for the margin it forgoes by selling part of its position down
- D. the praecipium is the arranger's own pro-rata share of the pool, computed on its final hold

*Rationale:* The praecipium prices work and risk only the arranger bears; the pool prices committed
capital, which every lender provides — which is why the arranger's yield on its 15,000,000 hold is
**1.650 %** against participants' **0.950 %**, a differential of **70.0 basis points** (13.4.2). A denies
the economics and misstates practice: the total fee is disclosed, and the split is information a sponsor
should ask for, because a large praecipium signals an arranger paid mostly for underwriting and therefore
one that will reach for flex. C describes something that does not happen — a lender selling down transfers
the margin with the position. D is the pool fee itself, which the arranger receives **in addition** to the
praecipium.

### Self-check — KA 13.4

1. *What does the arranger's 70 basis-point yield differential pay for?* — Structuring,
   documentation and distribution risk — the risk 13.4.3 shows a 3.31 % over-hold consumes entirely.
2. *Why is flex worth more than the arrangement fee?* — A margin is an annuity over the facility's
   life: 50 basis points is 1,760,607 of present value against a 504,000 one-off fee.
3. *What is the one thing a funds flow must be reconciled to, and when?* — The line-itemised
   close-cost budget, before close — not on the day, when the 2,640,000 provision meets a 2,709,000
   requirement.

---

## Advanced topics — Domain 13

### 13.A.1 Reliance, duty of care and the arithmetic of a liability cap

A diligence report is only useful to a party that can rely on it, and reliance is a legal
arrangement rather than a natural consequence of reading. Three mechanisms carry it. **Addressing**
— a report addressed to the sponsor gives lenders nothing; the standard remedy is a report
addressed to the lenders and the agent, or a **reliance letter** extending a duty of care to named
additional parties, usually for a fee and usually on the original engagement's terms. **Assignment
and transferability**, which matters for syndication: a report on which only the original syndicate
may rely constrains sell-down, and a secondary purchaser two years later needs a re-addressed report
or its own. And **the liability cap**, which is where the arithmetic bites.

Advisers' engagement terms almost universally cap liability, commonly at a multiple of fees. Take
Kestrel's technical stream: a fee of **260,000** and a cap at **three times fees** gives
**780,000** against a `C` of **6,200,000** — the cap covers **12.58 %** of the exposure the stream
exists to address. Across all seven streams, caps of three times fees total **4,500,000** against an
aggregate expected exposure of **9,439,874.85**, or **47.67 %** — and that comparison flatters the
position, because it sets the sum of *maximum* recoveries against the sum of *expected* losses, and
because recovery requires proving negligence, not merely showing the finding was missed.

The conclusion is the one 13.1.1 stated and this arithmetic proves: **diligence buys information,
not indemnity.** Two professional consequences follow. Negotiating a cap upwards is worth doing and
is not a risk-transfer strategy — moving Kestrel's technical cap from three to five times fees adds
520,000 of theoretical recovery against a 6,200,000 exposure. And **the reliance package is a
diligence deliverable in its own right**, to be listed on the CP schedule with the reports
themselves; a transaction that discovers at syndication that its reports cannot be re-addressed has
found a condition it cannot satisfy.

### 13.A.2 Vendor diligence, reliance and the price of time

A **vendor due diligence** report is commissioned by a seller and made available to bidders,
typically with reliance extended for a fee. It is the standard mechanism in secondary
infrastructure sales and increasingly in primary financings where a sponsor pre-packages diligence
to compress a lender's timetable. The question it poses is exact: is it better to rely on someone
else's diligence quickly, or to run your own slowly?

**Worked example 13.A.2 — own diligence or reliance?**

1. **Setup.** A buyer faces an aggregate probability **p = 0.65** that at least one material issue
   exists, with `C` = **9,000,000** if it reaches completion undetected and `F` = **600,000** to
   resolve it beforehand. **Own diligence:** 1,500,000 of fees, **12 weeks**, detection
   `d` = **0.90**. **Reliance on vendor diligence:** a 240,000 reliance fee, **3 weeks** of
   confirmatory work, detection `d` = **0.50** — lower because the scope was the seller's, the
   questions were the seller's and the report's cap is shared across every relying party.
2. **Formula.** Expected cost = fee + weeks × cost of delay + `p × [d × F + (1 − d) × C]`. The
   breakeven cost of delay is the weekly figure at which the two routes cost the same.
3. **Substitution.** Own residual `0.65 × (0.90 × 600,000 + 0.10 × 9,000,000) = 936,000`; reliance
   residual `0.65 × (0.50 × 600,000 + 0.50 × 9,000,000) = 3,120,000`. Breakeven cost of delay
   `[(1,500,000 + 936,000) − (240,000 + 3,120,000)] ÷ (3 − 12)`.
4. **Result.** At **zero** cost of delay, own diligence is better by **USD 924,000**: the
   1,260,000 of fees saved does not cover the 2,184,000 of detection lost. The **breakeven cost of
   delay is USD 102,666.67 per week**. At Kestrel's actual **124,133.33**, reliance wins: total
   expected cost **3,732,400** against **3,925,600**, an advantage of **USD 193,200**.
5. **Interpretation.** The decision has no general answer and one general rule: **it is decided by
   the price of time, and the breakeven is computable.** An organisation that always runs its own
   diligence is implicitly asserting a low cost of delay; one that always relies is asserting a
   high one; neither has done the arithmetic. Three cautions, all about the parameters rather than
   the sums. The **0.50 detection rate is the weakest number here** and it is doing most of the
   work — it should be estimated from the vendor report's actual scope against the buyer's own
   question list, not assumed. The reliance route's **liability position is systematically worse**
   (13.A.1): a cap shared among relying parties may be worth a fraction of its face amount, which
   the expected-value arithmetic does not see at all. And in a **competitive process** the cost of
   delay is not 124,133.33 a week but the entire value of the opportunity, in which case reliance
   wins by a margin no amount of detection can overturn — which is why vendor diligence exists, and
   why a seller's decision to commission it is a decision about the sale price.

### 13.A.3 The reviewer's closing eye

Invariants to test on any diligence and close package. **Every diligence stream has a stated `p`,
`d`, `C` and `F` with a recorded basis**, and its breakeven detection rate is on the page (13.1.3).
**The envelope is drawn and the binding stream identified**, with the elapsed cost priced once, not
per stream — and any serial dependency justified in writing at 124,133.33 a week (13.1.4). **Every
assumption touched by more than one adviser has one named owner and one version** (13.1.2).
**The model audit's findings are classified before they are counted**, with a stated materiality
threshold in the transaction's own metric, and no progress metric is quoted that is not
class-weighted (13.2.2). **Every Class 1 finding is quantified in `DSCR` and debt-capacity terms**,
and its correction is followed through into the credit paper, the term sheet and the base case
(13.2.3, 13.2.4). **The CP register is a dependency network, not a list**, with a duration, an
owner, an evidence requirement and a verifier per condition; float is computed per chain; and the
close date is stated as the expected maximum with its distribution, not as a single date (13.3.2).
**Acceleration effort is ranked by criticality risk net of float**, and the value of de-risking the
highest-weighted chain is computed before any fee is paid for it (13.3.3). **The close-cost budget
is line-itemised, split into fixed and proportional components, expressed as a share of debt raised,
and reconciled to sources and uses and to the funds flow** — before close (13.3.4, 13.4.5). **The
effective cost of debt is quoted twice**, on lender fees and on total close costs, with the
question each answers stated (13.3.4). **Every waived or deferred condition carries a deadline, an
owner, an evidence requirement and a consequence**, and appears as waived on the certificate of
satisfaction (13.3.5). **Market flex is capped at a level the model demonstrates the coverage
covenant survives with margin**, stressed jointly with interest-rate exposure, and its present
value is stated beside the arrangement fee (13.4.4). **Reliance and transferability are on the CP
schedule**, and every liability cap is expressed as a percentage of the exposure it addresses
(13.A.1). And **every AI-assisted extraction has a measured error rate on this transaction's own
documents**, with the sample size stated and a named human verifier (13.1, 13.2, 13.3, 13.4).

---

## Industry variations — Domain 13

- **Contracted power and renewables.** Diligence is standardised and comparatively fast: technology
  is proven, the offtake is a known form, and the binding stream is usually grid connection and
  land consents rather than technical review. Model audits are routine, findings are typically
  Class 2 and below, and the close timetable is dominated by permit chains — so the conjunction
  arithmetic of 13.3.2 matters more than the stream economics of 13.1.3.
- **Merchant power, commodities and industrial.** Market diligence dominates and cannot be
  compressed: the report is about a method for forecasting prices, so the diligence question is the
  consultant's methodology and the stress cases it supports rather than its central case.
  Syndication is harder, flex is wider, and lenders size on stressed cases, which makes the
  fully-flexed coverage test of 13.4.4 the binding constraint at mandate stage.
- **Transport concessions.** Traffic and revenue diligence is the single largest stream and often
  the longest, so it sets the envelope; grantor-side conditions add a chain nobody in the sponsor
  group controls; and the documentary chain lengthens because concession, direct agreement and
  finance documents must be negotiated three-cornered. Expect a lower probability of the base close
  date than 34 % and provision accordingly.
- **Water and regulated utilities.** Environmental and social diligence is frequently the binding
  stream — abstraction, discharge, biodiversity and community consents — as it is for Kestrel, and
  regulatory reset cycles put a tariff-determination date into the CP schedule that no amount of
  effort moves. Legacy contamination on brownfield sites is the characteristic Class 1 E&S finding.
- **Digital infrastructure.** Diligence is tenant-credit and power-availability work as much as
  technical work; asset lives are short so lifecycle and technology-obsolescence assumptions carry
  unusual weight; and close timetables are compressed by tenant commitment dates, which raises the
  cost of delay and therefore favours reliance and vendor diligence (13.A.2). Case study B is drawn
  from this sector.
- **Social infrastructure and availability PPPs.** Documentation is closest to standardised, which
  compresses the documentary chain and shifts the binding constraint onto public-sector approval
  chains with fixed committee calendars — the governance-latency arithmetic of PML-AI (KA 3.3)
  applied to a close timetable. Diligence fees as a share of a small facility are the highest of any
  sector, making the scale arithmetic of 13.3.4 decisive: these are the transactions that are
  aggregated or standardised or do not get financed.

---

## Case study — Domain 13: the audit that resized the debt (water / desalination)

**Situation.** Kestrel Water SPC entered diligence with a mandate for 42,000,000 of senior debt at
6.0 % over 12 years against a 60,000,000 envelope, a reported base-case `DSCR` of **1.3508** and a
credit committee requiring **1.30×**. Seven diligence streams were commissioned for 1,500,000 of
fees. The sponsor's first close plan sequenced them, on the arranger's preference that each stream
should be able to rely on the last.

**What happened.** The sequencing decision was challenged with the 13.1.3 arithmetic: the same seven
streams, the same fees and the same detection rates were worth **+4,066,699.88** inside a twelve-week
envelope and **−1,146,900.12** run serially, the 5,213,600 difference being 42 weeks at
124,133.33. Two dependencies were real — the model audit needed a frozen model and the insurance
adviser needed the technical loss scenarios — and both were accommodated inside the envelope with
staged deliverables. The envelope was set at twelve weeks by the environmental and social stream and
the close date at 20 weeks, and the conjunction arithmetic put the probability of holding it at
**34.125 %**, with an expected close of **22.4075 weeks** and an expected slip cost of **298,851**
against chain C's own **148,960**. The sponsor funded a dedicated permitting effort on chain B —
which was not the critical path — reducing its slip probability from 0.35 to 0.10, worth **76,031.67**
and lifting the probability of the base date to **47.250 %**.

The model audit returned **34 findings: 3 Class 1, 7 Class 2, 24 Class 3.** Two of the three
fundamental findings compounded. The debt-service schedule had been built as an **annuity due**,
understating the instalment by **283,564** and flattering the `DSCR` from 1.2743 to the reported
1.3508 — a model that was arithmetically correct, closed at zero, and did not implement the facility.
The `CFADS` definition placed the **260,000** maintenance-reserve top-up above `CFADS`; the model
placed it below, so true `CFADS` was **6,124,000** and the true `DSCR` **1.2224**. Debt capacity at
the committee's 1.30× was therefore `6,124,000 ÷ 1.30 × 8.383844 =` **39,494,354**, against
Domain 10's 41,171,123 on the uncorrected figure and the 42,000,000 mandated — a resize of
**2,505,646**. The third finding was worse in kind if not in size: the six-month DSRA of
**2,504,818** appeared as an operating outflow but not in the uses of funds, so the funding plan did
not fund a mandatory reserve.

Separately, the itemised close-cost budget came to **2,709,000** — **6.45 %** of debt raised, of
which only **544,000** was payable to lenders — against the model's provision of 2,640,000, a
**69,000** shortfall absorbed by the contingency line (3,645,403 to **3,576,403**, 7.59 % to
**7.45 %** of the EPC price).

**How it resolved.** The facility closed at **USD 39,494,354** of senior debt — an instalment of
**4,710,769.23** and a base-case `DSCR` of exactly **1.3000** on corrected `CFADS` — with equity
rising from 18,000,000 to **20,505,646**, gearing moving from 70/30 to **65.82/34.18** and
debt-to-equity from 2.33:1 to **1.926:1**. The DSRA was funded from equity at commercial operations
rather than from the construction envelope. Close costs fell by **41,343** on the smaller
proportional base, to **2,667,657** — which, on the smaller facility, is **6.7545 %** of debt
raised against the original 6.4500 %, because the fixed 2,016,000 does not shrink when the debt
does. The base case was re-run and the golden-answer set re-verified before the credit paper was
finalised.

**What the domain teaches here.** Three things, in order of how often they are got wrong. **The
expensive model error is a document error, not an arithmetic error** — an annuity due is a valid
amortisation and would have passed every check block, every scanner and every reasonable review that
did not read the facility. **Diligence value is destroyed by sequencing, not by fees** — the
5,213,600 that separated the two close plans was more than three times the entire diligence budget
and would never have appeared in a fee negotiation. And **the numbers that matter are the ones that
change a conclusion**: three of thirty-four findings resized the debt by 2,505,646 and exposed an
unfunded 2,504,818, while the other thirty-one, closed and reported as 91.2 % progress, changed
nothing at all.

## Case study B — Domain 13: the same waiver, used well and badly (digital infrastructure)

**Situation.** A 180,000,000 facility for a colocation data centre reached its long-stop date with
two conditions unsatisfied. **CP-14** was the registration of a land instrument, complete in
substance and awaiting a registry whose published processing time was 210 days. **CP-22** was a
third-party interface consent from the operator of an adjacent utility corridor, which had been
requested, acknowledged and not progressed. The anchor tenant's commitment expired in eleven weeks
and the sponsor's cost of a day's delay to close, in forgone contracted revenue against a fixed
lease term, was **240,000**.

**What happened.** Both conditions were waived into post-close undertakings at the same meeting, on
the same paper, for the same fee. **CP-14 was waived well.** It carried a 90-day deadline, a named
owner in the sponsor's legal team, a documentary standard (the registered instrument, certified), and
a consequence: a **25 basis-point margin step-up** until satisfaction. Registration in fact took the
registry's full 210 days, so the undertaking ran **120 days late**. The cost was the waiver fee of
**0.25 % of 180,000,000 = 450,000** plus the step-up of
`180,000,000 × 0.0025 × 120/360 =` **150,000**, a total of **600,000**. The counterfactual — waiting
to close until registration completed — was **210 days × 240,000 = 50,400,000**. **The waiver was
worth 49,800,000**, and it was the correct decision by a margin of eighty-three to one.

**CP-22 was waived badly.** Because it was mechanical in the sponsor's view and small in the
lenders', it was recorded as waived with no deadline, no consequence and no owner — a single line in
the certificate of satisfaction reading "waived". It fell out of every register within a month.
Twenty-six months later, on a refinancing, the incoming lenders' legal diligence found the corridor
consent outstanding and treated it as a title and access defect. Curing it — a negotiated easement,
a survey, and a payment to the corridor operator now negotiating with a counterparty that had no
alternative — cost **3,400,000**, **5.667 times** the entire cost of the CP-14 waiver, and delayed
the refinancing by a quarter.

**How it resolved.** The sponsor's post-mortem changed one artefact rather than one policy: the
certificate of satisfaction was rebuilt so that no condition could be marked waived without a
deadline, an owner, an evidence standard and a consequence, and the waived-condition register became
a standing item on the same monthly report as the covenant dashboard (Domain 10, Toolkit 10.T.2) —
so that a waived condition was reported in the same place as a covenant until it was satisfied.

**What the domain teaches here.** The instrument was not the problem. **A waiver is a priced
decision and here it was overwhelmingly right** — 600,000 against 50,400,000 — and a project finance
leader who refuses to waive on principle will destroy far more value than one who waives carelessly.
What distinguished the two conditions was entirely the **discipline attached to the waiver**:
deadline, owner, evidence standard, consequence. Domain 5's rule that a waived CP is not a satisfied
CP is not an accounting nicety; it is the difference between 600,000 and 4,000,000, and the interval
over which the difference emerges is two years, which is why nobody is present to learn the lesson
at the time.

---

## Executive perspective — Domain 13

What a project finance director cannot delegate in this domain:

- **The sequencing decision.** Whether diligence runs in parallel or in series is worth
  **5,213,600** on Kestrel — three and a half times the entire diligence budget — and it will be
  decided by advisers' scheduling preferences unless the director decides it. Own the envelope, name
  the binding stream, and require every serial dependency to be justified at 124,133.33 a week.
- **The `CFADS` definition and the model's conformance to it.** Domain 10 made this a standing
  obligation; here it acquires a price. Two document-conformance findings moved Kestrel's reported
  `DSCR` from 1.3508 to a true 1.2224 and its debt capacity by 2,505,646, and neither was an
  arithmetic error. Read the definition, then read the model line that implements it.
- **The finding classification, before any count is reported.** The director's question on receiving
  a model audit is never "how many findings?" but "which findings change a conclusion, and by how
  much?" A 91.2 % closure metric with three Class 1 findings open is a report that must be sent
  back.
- **The close date as a distribution.** One chance in three of the stated date, and better than one
  in two of four weeks later, is what a board, an offtaker and a contractor holding a price need to
  be told. A single-date close timetable is a forecast the director knows to be 34 % likely and has
  chosen to present as a plan.
- **The close-cost budget and the funds flow, reconciled before close.** 6.45 % of debt raised is
  the real number, only 20 % of it is the lender's, and a 69,000 hole between a rule-of-thumb
  provision and an itemised budget becomes visible on the day it cannot be fixed.
- **The flex cap, tested against the covenant.** Fifty basis points of flex is worth 1,760,607 —
  3.49 times the arrangement fee the director's team is negotiating — and consumes 44.9 % of the
  rate headroom the coverage covenant allows. Flex is signed at mandate stage, months before anyone
  models it; the director is the only person who can insist that the order be reversed.

## Calculation exercises — Domain 13

**Exercise 13.1** A diligence stream costs a **320,000** fee and takes **7 weeks**. The probability
of a material issue in its class is **0.25**, the cost if it reaches close is **7,400,000** and
pre-close correction costs **380,000**. The cost of delay is **96,000 per week** and detection is
estimated at **0.75**. Compute the breakeven detection rate inside a parallel envelope and on the
critical path, and the net value in each case.
*Solution.* Expected avoidable loss `0.25 × (7,400,000 − 380,000) =` **1,755,000**. Inside the
envelope `d* = 320,000/1,755,000 =` **18.23 %**; on the critical path the priced delay is
`7 × 96,000 =` **672,000**, so `d* = 992,000/1,755,000 =` **56.52 %**. Net value: without the stream
`0.25 × 7,400,000 =` **1,850,000**; with it inside the envelope
`320,000 + 0.25 × (0.75 × 380,000 + 0.25 × 7,400,000) = 320,000 + 533,750 =` **853,750**, so net
value **+996,250**; on the critical path add the 672,000, giving net value **+324,250**.
*Common error:* computing `d*` as `fee ÷ (p × C)` = 17.30 %, which ignores that a detected issue
still costs `F` to fix and so overstates the loss the stream can avoid.

**Exercise 13.2** Three CP chains: **A** 16 weeks, slip 4 weeks at probability 0.30; **B** 14 weeks,
slip 5 at 0.40; **C** 11 weeks, slip 6 at 0.25. Cost of delay **80,000 per week**. Compute the base
close, the expected close, the probability of the base date, and the conjunction premium.
*Solution.* Base close **16 weeks** (chain A). Realised durations: A 16 or 20, B 14 or 19, C 11 or
17. Over the eight outcomes: `E[close] =` **18.1450 weeks**, with probabilities **31.50 %** at 16,
**10.50 %** at 17, **28.00 %** at 19 and **30.00 %** at 20. Expected slip **2.1450 weeks**, costing
**171,600**. Chain A's own expected slip is `0.30 × 4 =` 1.2 weeks, costing **96,000**, so the
conjunction premium is **75,600**. Note that the max of expectations is **17.200** weeks, well
inside the expected maximum.
*Common error:* taking the longest chain's expected duration (17.200 weeks) as the expected close,
which understates the slip by 0.945 weeks and the cost by 75,600 — precisely the premium the
conjunction creates.

**Exercise 13.3** A transaction's close costs comprise a fixed component of **1,650,000** and a
proportional component of **1.85 %** of debt raised. Debt is **55,000,000**. Compute total close
costs and their share of debt, and the facility size at which they would fall to 2.5 % of debt.
*Solution.* Total `1,650,000 + 0.0185 × 55,000,000 = 1,650,000 + 1,017,500 =` **2,667,500**, which
is **4.850 %** of debt raised. Setting `1,650,000/D + 0.0185 = 0.025` gives
`D = 1,650,000/0.0065 =` **USD 253,846,153.85**.
*Common error:* solving `1,650,000/D = 0.025` and reporting 66,000,000, which forgets that the
proportional component consumes 1.85 of the 2.5 percentage points before the fixed component is
spread at all.

**Exercise 13.4** A model audit on a **36,000,000** facility at **6.5 %** over **14 years** finds
that a **320,000** annual line included in `CFADS` is excluded by the facility's definition.
Reported `CFADS` is **5,600,000**; the covenant is **1.25×**. Compute the reported and corrected
`DSCR`, the debt capacity at 1.25× on corrected `CFADS`, and classify the finding.
*Solution.* `AF(0.065, 14) =` **9.013842**; instalment `36,000,000/9.013842 =` **3,993,857.30**.
Reported `DSCR` `5,600,000/3,993,857.30 =` **1.4022**; corrected `CFADS` **5,280,000** gives
**1.3220**. Debt capacity `5,280,000/1.25 × 9.013842 =` **38,074,470.00**, which **exceeds** the
36,000,000 drawn by **2,074,470**. The finding therefore moves a reported output well beyond any
sensible materiality threshold but changes **no conclusion**: the covenant still holds and the debt
is still supportable. It is a **Class 2** finding, not Class 1.
*Common error:* classifying by the size of the movement (0.08 of `DSCR`, 320,000 of `CFADS`) rather
than by whether a conclusion changes — the distinction on which KA 13.2.2 rests.

**Exercise 13.5** A **96,000,000** facility carries a **1.35 %** arrangement fee of which
**0.30 %** is praecipium; the balance is a pool paid pro rata on final allocations. The arranger's
target hold is **30,000,000** and three participants take **22,000,000** each. Compute the fee
split and yields. Then: the market takes only **54,000,000** and the arranger charges itself
**85 basis points** a year on the excess over target hold for the facility's **14 years** at 6.5 %
(`AF` = 9.013842). Compute the outcome.
*Solution.* Total fee `96,000,000 × 1.35 % =` **1,296,000**; praecipium **288,000**; pool
**1,008,000**, a rate of **1.0500 %**. Arranger `288,000 + 1.05 % × 30,000,000 =` **603,000**, a
yield of **2.010 %**; each participant **231,000**, a yield of **1.050 %**; total
`603,000 + 3 × 231,000 =` 1,296,000 ✓. On the failed sell-down the arranger holds
`96,000,000 − 54,000,000 =` 42,000,000, an excess of **12,000,000**; the carry is
`12,000,000 × 0.0085 × 9.013842 =` **919,411.92**, against fees of 603,000 — a **net loss of
316,411.92**. The excess hold that exactly consumes the praecipium is
`288,000/(0.0085 × 9.013842) =` **3,758,924.52**, **3.92 %** of the facility.
*Common error:* charging the carry on the whole 42,000,000 residual rather than on the excess over
the target hold — the arranger always intended to hold 30,000,000 and earns a fee for doing so.

## Practitioner's toolkit — Domain 13

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 13.T.1 — Diligence stream economics and interface sheet (one per transaction)

**Part A, one row per stream:** stream · adviser and named partner · fee · duration in weeks ·
inside the envelope or on the critical path (and if the latter, the justification and its weekly
price) · `p`, `d`, `C`, `F` with the basis of each estimate and the number of comparable
transactions behind it · **breakeven detection rate** · net value · deliverable schedule (red-flag
date, draft date, final date) · addressee and reliance position · liability cap, and the cap as a
percentage of `C`. **Part B, the cross-stream interface list:** every assumption touched by more
than one adviser — availability, capacity, degradation, price, volume, tariff indexation, insured
loss, tax rate, construction programme — with its single named owner, its value, its version and
the streams that consume it. **Front line:** the envelope length, the binding stream, and the
aggregate net value in parallel against in series.

### Toolkit 13.T.2 — Model-audit finding register (one per model version)

Per finding: number · date raised · raised by (auditor / sponsor review / machine scan) · scope
layer (arithmetic / structure / documents / assumptions) · description · **class 1, 2 or 3 against
the stated materiality threshold** · quantified effect on `CFADS`, `DSCR`, minimum `DSCR`, debt
capacity and the funding plan · correction made · dependent outputs updated (credit paper, term
sheet, base case, sources and uses) · re-run of the golden-answer set (date and result) · verifier
and date · disposition if accepted rather than corrected, with the written rationale. **Header
block, mandatory before any count is quoted:** materiality threshold; counts by class;
**class-weighted closure**; and the list of open Class 1 findings with their quantified effect. A
finding count without this header block is not a reportable metric.

### Toolkit 13.T.3 — Close register: conditions, costs and funds flow (one per transaction)

**Part A, conditions:** condition · clause reference · category (corporate / documentary /
third-party / adviser / financial) · chain and predecessor · duration estimate · owner · evidence
requirement · evidence received and verified by whom · status (open / satisfied / waived /
deferred) · for waivers and deferrals: deadline, consequence and owner. Computed per chain: base
duration, float, slip probability, slip size, **criticality risk net of float**. Computed for the
transaction: base close, **`E[close]` and its distribution**, expected slip cost, the conjunction
premium, weeks of margin to the long-stop, and the value of de-risking the highest-weighted chain.
**Part B, costs:** every close-cost line by payee, split into fixed and proportional components,
with the total as a share of debt raised and of the envelope; the reconciliation to the model's
provision and to sources and uses; and the effective cost of debt on lender fees and on total close
costs. **Part C, funds flow:** every payment at close by payee and account, reconciled line by line
to Part B, with the reconciliation signed by the agent and the finance director **before** close
day, and payment instructions verified out of band.

## Exam preparation — Domain 13

**What is assessed.** The economics of an information purchase and the breakeven detection rate;
the difference between parallel and serial diligence and how to price it; the seven streams and the
error class each uniquely detects; the model audit's four scope layers and its three finding
classes, with the debt-capacity consequence of a Class 1 finding computed; conditions precedent as
a dependency network — float, criticality risk, the expected maximum and the conjunction premium;
the close-cost budget, its fixed and proportional decomposition, its share of debt raised and its
reconciliation to sources and uses and to the funds flow; the three syndication routes, the fee
architecture, the arranger's yield differential and the cost of a failed sell-down; market flex and
its coverage-covenant ceiling; waivers, long-stop dates and post-close undertakings; and the
governance of AI across all four Knowledge Areas.

**The calculations to do under time pressure.** A breakeven detection rate from fee, delay, `p`, `C`
and `F` (13.1.3). A stream's net value inside and outside the envelope (Exercise 13.1). A `DSCR` and
debt capacity from a corrected `CFADS` at a target coverage (13.2.3, Exercise 13.4). An annuity-due
instalment and the ratio it flatters (13.2.3). The expected maximum of three or four chains, with
the probability of the base date (13.3.2, Exercise 13.2). Close costs as a share of debt, and the
deal size at which a target share is reached (13.3.4, Exercise 13.3). A syndication fee split and
the arranger's yield (13.4.2, Exercise 13.5). A carry cost on an excess hold, and the excess that
consumes the praecipium (13.4.3). The present value of flex, and the margin at which a coverage
covenant fails (13.4.4).

**The traps.** Computing `d*` as `fee ÷ (p × C)` and omitting the pre-close correction cost
(Exercise 13.1) · pricing elapsed time once per stream when the streams run in parallel, or once for
the envelope when they do not (13.1.3) · treating a stream's breakeven above 100 % as an arithmetic
error rather than as evidence that the configuration cannot pay (MCQ 13.1-C) · quoting a finding
count or a "findings closed" percentage as progress (13.2.2, MCQ 13.2-B) · classifying a finding by
the size of the movement rather than by whether a conclusion changes (Exercise 13.4) · assuming an
annuity due and an annuity in arrears give the same debt service — they differ by 283,564 here,
and in the flattering direction (13.2.3, MCQ 13.2-A) · taking the longest chain's expected duration
as the expected close (13.3.2, Exercise 13.2, MCQ 13.3-A) · spending acceleration effort on a chain
with float (13.3.3, MCQ 13.3-B) · dividing close costs by the capital envelope rather than by debt
raised (13.3.4, MCQ 13.3-C) · omitting the proportional component when solving for the deal size at
a target close-cost share (Exercise 13.3, MCQ 13.3-D) · quoting the all-in cost of raising debt as
though the lender charged it (13.3.4) · applying the full arrangement fee to the arranger's hold and
ignoring the praecipium's exclusivity (MCQ 13.4-A) · charging an underwriter's carry on its residual
hold rather than on the excess over target (Exercise 13.5, MCQ 13.4-B) · pricing flex as one year or
as an undiscounted sum instead of an annuity (MCQ 13.4-C) · describing a signed transaction as
closed (MCQ 13.4-E) · recording a waiver without a deadline, owner, evidence standard and
consequence (Case study B).

**How the domain connects.** Domain 5's condition register becomes this domain's CP schedule and its
close-cost provision; Domain 6's model and model-audit economics become KA 13.2's finding classes;
Domain 9's tranche structures and effective-rate discipline become KA 13.4's syndication and
close-cost arithmetic; Domain 10's `CFADS`, coverage ratios and covenant thresholds are what every
Class 1 finding is measured in and what market flex is tested against; Domain 11's risk allocation
and Domain 12's contracts are what the legal, technical and insurance streams verify. Forward,
Domain 14 begins at the first drawdown this domain makes available and monitors the conditions this
domain waived or deferred; Domain 15 operates the covenant regime the diligence file will be argued
over; and Domain 16 systematises the AI controls each Knowledge Area here specified.

## Domain 13 summary
Diligence and close are economic decisions under a deadline, and elapsed time — **124,133.33 per
week** for Kestrel — governs both. A diligence stream is worth running when the loss it avoids
exceeds what it costs, and the breakeven detection rate `d* = (fee + priced elapsed time) ÷
[p × (C − F)]` states the condition compactly: the diligence spend divided by the expected avoidable
loss. Kestrel's seven streams, 1,500,000 of fees and 54 weeks of duration, are worth
**+4,066,699.88** inside a twelve-week parallel envelope and **−1,146,900.12** run serially, the
**5,213,600** difference being 42 weeks of scheduling and nothing else; inside the envelope no
stream needs better than a **31.25 %** detection rate to pay, while in series five need **72.02 %**
to **99.42 %** and two — environmental and social at **119.12 %**, insurance at **289.86 %** —
cannot pay at any detection rate. Model audits are read by **class, never by count**: Kestrel's 34
findings resolved as 3 Class 1, 7 Class 2 and 24 Class 3, so the 24 that changed nothing were
**70.6 %** of the count and the reported **91.2 %** closure could be true with every consequential
finding open. Two document-conformance findings — an annuity-due schedule understating the instalment
by **283,564** and a misplaced 260,000 reserve line — moved the reported `DSCR` from **1.3508** to
a true **1.2224** and debt capacity at 1.30× to **39,494,354**, a resize of **2,505,646**,
while a third left the **2,504,818** DSRA unfunded; none of the three was an arithmetic error, which
is why the audit's document layer matters more than its formulae. Conditions precedent are a
conjunction with a critical path, so the close date is the expected **maximum** of the chains:
Kestrel's five chains give `E[close]` **22.4075 weeks** against a base of 20 and a critical chain
whose own expected duration is 21.2000, a **34.125 %** probability of holding the stated date, an
expected slip cost of **298,851** against the critical chain's **148,960**, and a **conjunction
premium of 149,891** — and the highest-value intervention, worth **76,031.67**, is on chain B, which
is not the critical path. Close costs of **2,709,000** are **6.45 %** of debt raised, only 544,000 of
it payable to lenders, decomposing into a **2,016,000** fixed component and **1.65 %** proportional —
so a facility must reach about **149,333,333** before close costs fall below 3.0 % of debt — and the
budget reconciles to a model provision 69,000 short, implying an effective cost of debt of
**6.2386 %** on lender fees and **7.2376 %** on the whole cost of raising it. Syndication pays the
arranger **247,500** on a 15,000,000 hold, a **1.650 %** yield against participants' **0.950 %**, and
a **3.31 %** over-hold consumes that entire premium — which is why market flex, worth
**1,760,607** or **3.4933×** the arrangement fee and consuming **44.9 %** of the coverage
covenant's **111.4 basis points** of rate headroom, is the term that should be negotiated hardest and
almost never is. Throughout, AI shortens the envelope by extracting, reconciling and tracking, and is
permitted to conclude nothing: not a legal, tax or environmental opinion, not a finding's class, not
a condition's satisfaction, not a market price, and never a payment instruction. Domain 14 takes the
first drawdown this domain made available.
