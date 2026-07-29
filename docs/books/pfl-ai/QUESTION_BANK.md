# Question bank — PFL-AI Body of Knowledge

> **Derived, not duplicated.** Every item is the question as it appears in its Knowledge
> Area, consolidated here by `_build/make_question_bank.py`. Answer keys and rationales are
> the chapters' own. To change an item, change it in its Knowledge Area and regenerate —
> which is why there is no second copy to fall out of step.

**276 items** across 16 domains. Every numeric option in every item is
independently recomputed by the golden-answer suite, not only the correct one, so a
distractor cannot be arithmetically impossible without the gate failing.

## Coverage by cognitive level

| Level | Items | Share |
|---|---|---|
| Recall | 13 | 4.7 % |
| Comprehension | 2 | 0.7 % |
| Application | 117 | 42.4 % |
| Analysis | 128 | 46.4 % |
| Evaluation | 16 | 5.8 % |

A bank weighted heavily to recall tests memory rather than competence; one weighted
heavily to Evaluation is unanswerable under time pressure. The distribution above is a fact
to be reviewed against the examination blueprint, not a claim that it is correctly balanced —
the blueprint weightings are an open decision (see `CORPUS_GATE_REPORT.md` §9).

## Coverage by domain

| Domain | Items | Levels represented |
|---|---|---|
| 1 | 18 | Recall, Comprehension, Application, Analysis, Evaluation |
| 2 | 24 | Recall, Application, Analysis, Evaluation |
| 3 | 26 | Recall, Comprehension, Application, Analysis, Evaluation |
| 4 | 12 | Recall, Application, Analysis |
| 5 | 16 | Application, Analysis |
| 6 | 16 | Application, Analysis, Evaluation |
| 7 | 16 | Application, Analysis |
| 8 | 16 | Application, Analysis |
| 9 | 16 | Recall, Application, Analysis |
| 10 | 21 | Recall, Application, Analysis, Evaluation |
| 11 | 14 | Application, Analysis |
| 12 | 15 | Recall, Application, Analysis |
| 13 | 18 | Recall, Application, Analysis, Evaluation |
| 14 | 16 | Recall, Application, Analysis |
| 15 | 16 | Recall, Application, Analysis |
| 16 | 16 | Application, Analysis |

---

## Domain 1

**1.1-A** `[1.1.2 · Application]` A sponsor gives lenders a guarantee that covers cost overruns until the plant passes its completion test, after which lenders may look only to project cash flows. This financing is best described as:

- A. full recourse
- B. non-recourse
- C. limited recourse ✅
- D. unsecured corporate lending

*Rationale:* Bounded sponsor support (here, to completion) between the poles is the defining shape of limited recourse. A would expose the sponsor for the loan's life; B would mean no sponsor support at all; D abandons both the security package and the ring-fence.


**1.1-B** `[1.1.3 · Analysis]` Which single feature of the SPV makes non-recourse lending possible?

- A. its tax registration
- B. legal ring-fencing: the SPV can conduct only the project, so its contracts and cash are isolated and chargeable ✅
- C. its sponsors' credit ratings
- D. the size of its share capital

*Rationale:* Lenders can accept project-only recourse because the ring-fence guarantees no other business can dilute, encumber or divert the cash they are lending against. C reverses the concept (sponsor credit is what non-recourse lending does *without*); A and D are administrative facts, not the mechanism.


**1.1-C** `[1.1.4 · Application]` A fund holding long-dated pension liabilities wants infrastructure exposure. The asset-capital matching principle points it toward:

- A. construction-phase risk in a greenfield project
- B. stabilised operating assets with contracted cash flows ✅
- C. development-stage equity at risk
- D. short-term bridge lending

*Rationale:* Long-dated stable liabilities match long-dated stable cash flows on the axis that dominates — risk-holding capability. A and C sit where construction and development specialists (banks, ECAs, developers) hold the risk; D matches a treasury desk, not a pension profile. Note that on the *timing* axis alone a deferred greenfield stream is the closer duration match (WE 1.1.4) — which is why the risk axis must be settled first.


**1.1-D** `[1.1.2 · Application]` The limited-recourse route costs 5,202,128 more in present value than the corporate route and removes an enforcement exposure of 10,073,997. The breakeven probability of a parent-impairing failure is:

- A. 12.39 %
- B. 23.42 %
- C. 51.64 % ✅
- D. 28.22 %

*Rationale:* `5,202,128/10,073,997 = 51.64 %` (WE 1.1.2). A divides the incremental cost by the 42,000,000 of debt instead of by the exposure — a cost intensity, not a breakeven; B uses only the 2,359,000 close-cost premium and drops the 2,843,128 margin differential; D is the large-facility asymptote, which omits the fixed close-cost premium altogether and therefore applies to no actual facility.


**1.1-E** `[1.1.4 · Analysis]` A level availability stream pays 8,900,000 a year for 15 years, discounted at 8.0 %. Its Macaulay duration is closest to:

- A. 6.59 years ✅
- B. 8.00 years
- C. 13.50 years
- D. 15.00 years

*Rationale:* `D = 1.08/0.08 − 15/(1.08¹⁵ − 1) = 6.5945` (WE 1.1.4). B is the *unweighted* mean of the payment dates 1 to 15 — the duration you get by forgetting to discount the weights; C is the `(1 + r)/r` ceiling, which a level stream approaches only as the tenor approaches infinity; D confuses the asset's life with its duration, the error the example exists to disarm.


**1.1-F** `[1.1.2 · Evaluation]` A sponsor's board is shown a breakeven failure probability of 51.64 % and concludes that limited recourse "fails its own test" and should be abandoned. The best professional response is:

- A. agree — the arithmetic is decisive
- B. the expected-value test prices a mean while the sponsor is insuring a correlated tail, and it values none of the partnership or balance-sheet capacity the structure delivers; the breakeven is one input, not the decision ✅
- C. recompute at a higher discount rate until the answer changes
- D. the calculation is invalid because probabilities of project failure cannot be estimated

*Rationale:* WE 1.1.2's own interpretation: the exposure term is an expected shortfall, not the correlated loss that would arise in the bad state, and the structure additionally buys an option (the fund's participation) the table does not contain. A treats one input as the decision; C is assumption-shopping, which is the misconduct 1.3.1 names; D overstates the objection — the parameter is uncertain, which is an argument for stating it and testing it, not for discarding the frame.


**1.2-A** `[1.2.2 · Application]` A company reports quarterly profit of 2,000,000 while receivables rose 3,000,000, inventory rose 1,000,000 and payables rose 500,000. Its operating cash flow is:

- A. +2,000,000
- B. −1,500,000 ✅
- C. +500,000
- D. −2,500,000

*Rationale:* `2.0 − 3.0 − 1.0 + 0.5 = −1.5m`. A stops at profit; C nets only the payables against profit and forgets the asset build; D subtracts the payables increase instead of adding it — supplier credit is a cash *source*.


**1.2-B** `[1.2.3 · Application]` In the leverage example (70m debt, interest-only 6 %; equity 30m), project cash of 9,000,000 produces a levered equity return of:

- A. 9.0 %
- B. 16.0 % ✅
- C. 26.0 %
- D. 30.0 %

*Rationale:* `(9.0 − 4.2)/30 = 16.0 %`. A is the unlevered return; C is the base-case levered return; D divides project cash by equity without paying the lender first.


**1.2-C** `[1.2.3 · Analysis]` A project shows strong NPV and well-allocated risks, but its revenue arrives seasonally while debt service is quarterly and level. The bankability verdict is:

- A. bankable — two of three corners suffice
- B. unbankable as structured: the cash corner fails; reshape the debt profile or add liquidity support ✅
- C. unbankable permanently — reject the project
- D. bankable if the sponsors accept a higher equity IRR

*Rationale:* The triangle is conjunctive: mistimed cash defeats value and allocation. The cure is structural (sculpted or seasonal debt service, reserve accounts — Domains 9–10), not rejection (C) and not a return adjustment that changes nothing about timing (D).


**1.2-D** `[1.2.3 · Application]` The same project (100,000,000 cost, 12,000,000 of annual operating cash, 70,000,000 of senior debt at 6.0 %, 30,000,000 of equity) is financed with debt **amortising over 12 years** rather than interest-only. `AF(0.06, 12) = 8.383844`. The base-case cash-on-cash equity return is:

- A. 12.1687 % ✅
- B. 26.0000 %
- C. 40.0000 %
- D. 12.0000 %

*Rationale:* Instalment `70,000,000/8.383844 = 8,349,392.06`; `(12,000,000 − 8,349,392.06)/ 30,000,000 = 12.1687 %` (WE 1.2.3b). B is the interest-only reading, which charges only the 4,200,000 of interest; C divides project cash by equity and never pays the lender at all; D concludes that amortisation makes leverage exactly neutral — close, and wrong by the 17 basis points that are the whole of the remaining spread benefit.


**1.2-E** `[1.2.2 · Application]` Kestrel's documented `CFADS` is 6,384,000, debt service is 5,009,635 and the covenant is 1.20×; annual revenue is 12,000,000. Expressed in days of revenue, the remaining covenant headroom is:

- A. 11.33 days ✅
- B. 18.25 days
- C. 29.58 days
- D. 1.52 days

*Rationale:* Headroom `6,384,000 − 6,011,562 = 372,438`; `372,438/12,000,000 × 365 = 11.33 days` (WE 1.2.2b). B is the working capital *already* absorbed, which is history rather than headroom; C is the total tolerance measured from the pre-working-capital `CFADS` of 6,984,000, so it double-counts the 600,000 already spent; D is the sensitivity — the days worth 0.01× of `DSCR` — mistaken for the headroom itself.


**1.2-F** `[1.2.1 · Evaluation]` A contractor offers to take a construction risk for 1,350,000; the sponsors' expected cost of the risk is 900,000, and lenders have said they will de-gear from 70 % to 62 % of a 60,000,000 envelope if it is retained. `k_e` = 15.42 %, `k_d` = 6.0 %, `AF(0.08, 12)` = 7.536078. The sound conclusion is:

- A. refuse: the quote is loaded 50 % above expected cost
- B. accept: retention costs 3,407,513 in present value through the capital structure, so the transfer creates 2,057,513 of value ✅
- C. accept only if the contractor reduces the price to 900,000
- D. indifferent: risk transfer is value-neutral by construction

*Rationale:* `4,800,000 × (0.1542 − 0.0600) × 7.536078 = 3,407,513.04` (WE 1.2.1). A compares the quote with the risk's own expected cost and ignores where a retained risk is actually paid for; C demands a price at which no contractor would accept the risk, since the transferee must be paid for uncertainty as well as expectation; D denies the gain from trade that the equity-debt spread creates.


**1.3-A** `[1.3.1 · Analysis]` An analyst is asked to present the upside case as the base case "because the committee needs confidence". The professional response is:

- A. comply — labels are a presentation choice
- B. decline: presenting a knowingly optimistic case as the base misrepresents the forecast; offer the honest base with sensitivities instead ✅
- C. comply, but keep a private note of the true base
- D. resign immediately without discussion

*Rationale:* Candour about numbers is a duty, not a style (A); a private note documents the misrepresentation without preventing it (C); B both refuses the breach and offers the legitimate route to confidence — evidence. D skips the professional obligation to fix the problem before escalating personal exits.


**1.3-B** `[1.3.2 · Application]` A bank advising a grantor on a tender also wishes to lend to one of the bidders. The minimum acceptable handling is:

- A. proceed — different departments are involved
- B. disclose the dual role to the grantor, and either obtain informed consent with effective information barriers or decline one role ✅
- C. keep the lending discussion confidential until after award
- D. advise the grantor to select that bidder

*Rationale:* Disclosure plus genuine separation (or declination) is the standing machinery; department labels alone are not barriers (A); concealment converts a conflict into misconduct (C); D is the conflict operating in the open.


**1.3-C** `[1.3.3 · Recall]` Under the PCI responsible-AI principle, responsibility for an AI-drafted covenant summary used in a credit paper rests with:

- A. the AI vendor
- B. the model itself
- C. the professional who verified, signed and used it ✅
- D. nobody, if the tool was approved

*Rationale:* Accountability cannot be delegated to software or its supplier; tool approval governs *which* tools may be used, never *who* answers for the output.


**1.3-D** `[1.3.2 · Application]` Disclosing a second mandate would cost a firm the 250,000 fee at risk on it. If concealed and later discovered, the firm loses 900,000 of unpaid fees, 12,750,000 of jurisdictional franchise and 400,000 of legal cost. The breakeven probability of discovery is:

- A. 1.7794 % ✅
- B. 27.7778 %
- C. 1.9608 %
- D. there is no breakeven — concealment can never pay at any probability

*Rationale:* `250,000/14,050,000 = 1.7794 %` (WE 1.3.2). B divides by the 900,000 of forfeited fees alone and ignores the franchise, which is the largest term; C counts the franchise and drops the forfeited fees and legal cost. D is the answer a reader gives from the duty rather than from the arithmetic — the duty is indeed unconditional, but the calculation does have a finite breakeven, and being able to state it is what closes a commercial argument.


**1.3-E** `[1.3.3 · Application]` A negotiating position taken on an unverified AI-reported benchmark held Kestrel's close nine weeks at a cost of delay of 124,133.33 per week. The cost of the episode was:

- A. USD 1,117,200 ✅
- B. USD 2,112,729
- C. USD 252,000
- D. USD 124,133

*Rationale:* `9 × 124,133.33 = 1,117,200` (WE 1.3.3). B is the present value of the 60-basis-point prize the false benchmark appeared to offer — the amount at stake, not the amount spent; C is one year of that margin difference undiscounted; D prices a single week.


**1.3-F** `[1.3.3 · Comprehension]` Under the responsible-AI principle as fixed in this domain, which statement is correct?

- A. an approved tool's output may be relied on without further checking, since approval is the control
- B. verifying a number and verifying a claim are different acts: the first is independent recomputation, the second is tracing the assertion to a source that exists and is the right version ✅
- C. AI output must be disclosed to the client in every case, however immaterial
- D. confidentiality obligations do not attach to data entered into an AI tool, because no third party reads it

*Rationale:* 1.3.3 separates the two verification acts precisely because they fail differently and are caught by different habits. A confuses tool governance with output accountability (MCQ 1.3-C); C overstates the rule, which attaches to *material* use touching deliverables; D is wrong on the foundational point that entering deal data into a tool is itself a disclosure.


## Domain 2

**2.1-A** `[2.1.1 · Analysis]` A project reports rising profit and falling operating cash flow over three quarters, with no change in accounting policy. The most likely explanation is:

- A. the profit figures must be erroneous
- B. working capital is absorbing cash — receivables and/or inventory are growing faster than payables ✅
- C. depreciation has increased
- D. the two measures are unrelated, so no explanation is needed

*Rationale:* Accrual profit and operating cash diverge principally through working-capital movements (KA 2.3.1). C would *raise* operating cash relative to profit (a non-cash charge added back), and D denies the articulation the statements are built on.


**2.1-B** `[2.1.2 · Application]` A sponsor board has firmly resolved to fund a plant upgrade next year. At this year end this is:

- A. a liability, because the decision is certain
- B. not a liability — there is no present obligation from a past event; an intention is not an obligation ✅
- C. a provision, because the amount is estimable
- D. a contingent asset

*Rationale:* Liability recognition requires a *present* obligation arising from a past event (2.1.2). Certainty of intent is irrelevant; a provision (C) still requires an obligation to exist, and D inverts the direction entirely.


**2.1-C** `[2.1.3 · Recall]` A statement set where the cash-flow statement's closing cash does not equal the balance sheet's cash indicates:

- A. a normal difference in presentation
- B. an error or omission — the statements articulate to one record, so they must reconcile ✅
- C. the use of accrual rather than cash accounting
- D. a foreign-currency effect that requires no action

*Rationale:* Articulation is an identity (2.1.3). FX and presentation differences are disclosed and reconciled, not left unbalanced — an unreconciled set is a defect, and locating the break is the reader's fastest diagnostic.


**2.1-D** `[2.1.1 · Analysis]` Over its first year a project reports accrual `EBITDA` of 7,500,000 and cash-basis `EBITDA` of 6,900,000. Receivables closed at 900,000 and payables at 300,000, both having opened at nil. The 600,000 difference is best described as:

- A. an error, since the two measures should agree over a full year
- B. the closing net working-capital balance — the divergence is the balance sheet, and it reverses when the trading cycle stops growing ✅
- C. evidence that the accrual figures are optimistic
- D. a timing difference that will never reverse

*Rationale:* Cumulative accrual result less cumulative cash result equals net working capital (2.1.1), so the divergence is locatable in named accounts and is a growth profile rather than a quality-of-earnings verdict. A denies the identity; C reads a timing effect as a judgment; D is wrong because the balances unwind — cash accounting is late, not conservative.


**2.1-E** `[2.1.2 · Evaluation]` An insurer has indicated it will probably meet a claim, and the amount is reliably estimable. The entity also faces a counterclaim it assesses as probable and estimable. The correct treatment of the pair is:

- A. recognise both, since both are probable and estimable
- B. recognise neither, since both are disputed
- C. recognise the counterclaim as a provision; disclose the insurance recovery, which requires virtual certainty to be recognised ✅
- D. net the two and recognise the difference

*Rationale:* The recognition thresholds are deliberately asymmetric — probable for an obligation, virtually certain for a contingent asset (2.1.2) — so an entity in the same factual position on both sides reports the downside first. A applies one threshold to both; B ignores that the counterclaim passes its tests; D offsets two items that arise from different events, which the recognition tests are applied to individually.


**2.2-A** `[2.2.1 · Application]` Revenue 12,000,000; cash operating costs 4,500,000; depreciation 2,400,000; interest 2,520,000; tax 20 %. Net income is:

- A. USD 2,580,000
- B. USD 2,064,000 ✅
- C. USD 4,080,000
- D. USD 7,500,000

*Rationale:* `PBT 2,580,000 × (1 − 0.20) = 2,064,000`. A is PBT before tax; C taxes `EBIT` instead of PBT; D is `EBITDA`.


**2.2-B** `[2.2.4 · Application]` Net income 2,064,000; depreciation 2,400,000; receivables +900,000; payables +300,000. Operating cash flow is:

- A. USD 4,464,000
- B. USD 3,864,000 ✅
- C. USD 3,264,000
- D. USD 2,064,000

*Rationale:* `2,064,000 + 2,400,000 − 900,000 + 300,000 = 3,864,000`. A omits the working-capital movements; C subtracts the payables increase instead of adding it (supplier credit is a cash *source*); D stops at profit.


**2.2-C** `[2.2.2 · Analysis]` Kestrel's annual instalment is 5,009,635, of which 2,520,000 is interest. The income statement expense and the balance-sheet effect are:

- A. expense 5,009,635; debt falls by 5,009,635
- B. expense 2,520,000; debt falls by 2,489,635 ✅
- C. expense 2,489,635; debt falls by 2,520,000
- D. expense 5,009,635; debt unchanged

*Rationale:* Only interest is an expense; principal is a balance-sheet movement. A expenses the whole instalment (the classic error), C reverses the two components, D ignores repayment.


**2.2-D** `[2.2.3 · Recall]` Repayment of debt principal appears in the cash-flow statement under:

- A. operating activities
- B. investing activities
- C. financing activities ✅
- D. it does not appear, being a balance-sheet movement

*Rationale:* Principal flows are financing. D confuses the *income statement's* silence on principal with absence from the cash-flow statement, where every cash movement appears.


**2.2-E** `[2.2.2 · Application]` Opening cash nil; operating cash flow 3,864,000; principal repaid 2,489,635.23; no capex, distributions or contributions. Plant is 57,600,000 net, receivables 900,000, payables 300,000 and closing debt 39,510,364.77. Closing cash and total assets are:

- A. cash 1,374,364.77; total assets 59,874,364.77 ✅
- B. cash 3,864,000.00; total assets 62,364,000.00
- C. cash (1,145,635.23); total assets 57,354,364.77
- D. cash 1,374,364.77; total assets 58,500,000.00

*Rationale:* Cash = 3,864,000 − 2,489,635.23, and total assets are the three lines summed. B omits the principal repayment (treating debt service as invisible to cash); C deducts the whole 5,009,635.23 instalment, double-counting the interest already inside operating cash flow; D omits the cash balance from the asset total.


**2.2-F** `[2.2.3 · Evaluation]` Two otherwise identical projects report operating cash flow of 3,864,000 and 6,384,000. The second classifies interest paid within financing activities. The soundest conclusion is:

- A. the second project generates 2,520,000 more cash from operations
- B. the figures are not comparable: the 2,520,000 difference is a classification of interest, and restating one presentation makes them identical ✅
- C. the second project must have lower interest costs
- D. the first project has a working-capital problem

*Rationale:* The classification of interest paid moves reported operating cash flow by the whole interest figure without changing any cash (2.2.3). A and C read a presentation choice as an economic difference — the specific error the `CFADS` identity is exposed to; D invents a cause the statements do not support.


**2.3-A** `[2.3.1 · Application]` `EBITDA` 7,500,000; tax 516,000; working-capital increase 600,000; debt service 5,009,635. `DSCR` including the working-capital movement is:

- A. 1.39
- B. 1.27 ✅
- C. 1.50
- D. 1.20

*Rationale:* `CFADS = 7,500,000 − 516,000 − 600,000 = 6,384,000`; `÷ 5,009,635 = 1.27`. A excludes the working-capital movement (the definitional point of the example); C ignores tax as well; D is a typical covenant threshold, not this calculation.


**2.3-B** `[2.3.3 · Application]` USD 1,200,000 is spent on an overhaul. Capitalised over 10 years versus expensed immediately, the year-one differences are:

- A. profit lower by 1,200,000 if capitalised; cash differs
- B. profit lower by 1,080,000 if expensed; cash identical ✅
- C. profit identical; cash lower by 1,080,000 if expensed
- D. profit lower by 120,000 if expensed; cash identical

*Rationale:* Expensing charges 1,200,000 against capitalising's 120,000 — a 1,080,000 difference — and the cash outflow is the same either way. A inverts which treatment charges more; C confuses which statement is affected; D states the capitalised charge as the difference.


**2.3-C** `[2.3.4 · Analysis]` A contractor faces a claim it considers *possible* but not probable, with a reliably estimable amount. The treatment is:

- A. recognise a provision, since the amount is estimable
- B. disclose as a contingent liability; recognition requires probable settlement ✅
- C. no disclosure, since settlement is not probable
- D. recognise a contingent asset

*Rationale:* Estimability alone is insufficient — probability of settlement is the failed test, so disclosure rather than recognition (2.3.4). C hides information users need; D reverses the direction.


**2.3-D** `[2.3.2 · Analysis]` A contractor recognises revenue on 60 % completion using a cost-input measure while its earned-value system reports 48 % complete. The soundest reading is:

- A. no issue — the two systems serve different purposes
- B. the divergence needs explaining: an input measure inflated by inefficiency can recognise revenue ahead of performance ✅
- C. the earned-value figure must be wrong
- D. revenue should be restated to 48 % automatically

*Rationale:* Cost-input measures reward spending, so inefficiency can raise apparent progress (2.3.2) — the gap is a signal to investigate, not to dismiss (A) or resolve by assumption (C, D).


**2.3-E** `[2.3.1 · Application]` `CFADS` before working capital 6,984,000; payables held at 300,000; revenue 12,000,000; receivables opened at nil; debt service 5,009,635.23. The maximum days sales outstanding consistent with a 1.20× covenant, at a 365-day convention, is:

- A. 27.3750 days
- B. 31.0845 days
- C. 38.7033 days ✅
- D. 45.0000 days

*Rationale:* The 1.20× trigger is `CFADS` 6,011,562.28, allowing 972,437.72 of absorption and therefore receivables of 1,272,437.72 — `1,272,437.72/12,000,000 × 365`. A is the actual position, not the limit; B is the 1.25× *distribution* threshold, the commonest confusion because both appear in the same clause; D is the scenario that breaches, at `DSCR` 1.1587.


**2.3-F** `[2.3.4 · Application]` A restoration obligation of 4,500,000 falls due in 25 years and is discounted at 5.0 %. The amount recognised and the first-year charge against profit are:

- A. provision 4,500,000; charge 180,000
- B. provision 1,328,862.47; charge 119,597.62 ✅
- C. provision 1,328,862.47; charge 66,443.12
- D. provision 1,328,862.47; charge nil, since no cash moves

*Rationale:* The provision is recognised at present value, and the first year carries both accretion 66,443.12 and depreciation of the capitalised restoration asset 53,154.50. A recognises the undiscounted amount; C takes the accretion alone and forgets the asset it created; D confuses a nil cash effect with a nil charge.


**2.3-G** `[2.3.2 · Analysis]` A contractor has recognised 3,857,142.86 of cumulative profit on a 48,000,000 contract when a re-forecast makes expected total costs 49,500,000. The charge in the period is:

- A. 1,500,000 — the expected loss
- B. 5,357,142.86 — the expected loss plus the reversal of profit already recognised ✅
- C. 1,500,000 spread over the remaining life of the contract
- D. nil until the loss is actually incurred

*Rationale:* The cumulative position must move from +3,857,142.86 to −1,500,000, so the period charge is the whole swing (2.3.2). A forgets the reversal; C is precisely what immediate recognition exists to prevent; D applies a cash-basis instinct to an accrual test.


**2.4-A** `[2.4.2 · Application]` `EBIT` 5,100,000 and interest 2,520,000. Interest cover is:

- A. 1.27×
- B. 2.02× ✅
- C. 2.98×
- D. 0.49×

*Rationale:* `5,100,000/2,520,000 = 2.02`. A is the `DSCR` from 2.3.1; C uses `EBITDA` in the numerator; D inverts the ratio.


**2.4-B** `[2.4.2 · Analysis]` A project shows interest cover 2.02× and debt/`EBITDA` 5.27×. The soundest interpretation is:

- A. the ratios contradict each other, so one must be wrong
- B. earnings service the interest comfortably while leverage is high — tolerable given contracted revenue, but dependent on that certainty ✅
- C. the project is over-leveraged regardless of revenue structure
- D. interest cover is the only relevant measure for a lender

*Rationale:* The two measure different things and both are true (2.4.2); what makes high leverage acceptable is revenue certainty. D ignores principal, which is why `DSCR` is covenanted.


**2.4-C** `[2.4.3 · Analysis]` A sponsor asks "how much has the project spent?". The professional response is:

- A. quote the paid figure, as it is the most conservative
- B. ask which measure is meant — committed, incurred, invoiced, paid or capitalised — since they differ materially and all are defensible ✅
- C. quote the committed figure, as it is the most complete
- D. quote the capitalised figure, since it appears in the accounts

*Rationale:* Five defensible figures exist (2.4.3); answering without establishing which one is meant guarantees a later dispute. Each of A, C and D picks one arbitrarily.


**2.4-D** `[2.4.1 · Evaluation]` A project's first operating year shows after-tax return on capital employed of 6.8000 % against a `WACC` of 7.9860 %, while the appraisal recorded an `NPV` of +16,179,360 and an `IRR` of 12.19 %. The soundest conclusion is:

- A. the appraisal was over-optimistic and should be revisited
- B. a single early-year accounting return on a fully-carried asset is not comparable with a lifetime discounted return; both figures are correct ✅
- C. the project is destroying value and should be restructured
- D. the `WACC` must be wrong, since the project was approved

*Rationale:* Accounting `ROCE` in year one is measured on the asset at its maximum carrying amount and rises mechanically as it depreciates (2.4.1) — year two is already 6.8486 % on unchanged `EBIT`. A and C treat a single-period ratio as a verdict on lifetime economics, the error Domains 3 and 4 exist to prevent; D reverses the logic of appraisal.


**2.4-E** `[2.4.2 · Analysis]` A facility carries both a 1.20× `DSCR` covenant and a 2.00× interest-cover covenant. `CFADS` is 6,384,000, debt service 5,009,635.23, `EBIT` 5,100,000, interest 2,520,000, revenue 12,000,000 and the cash-to-revenue gearing 0.80. The binding covenant is:

- A. the `DSCR` covenant, because coverage tests are stricter in project finance
- B. the interest-cover covenant: it tolerates a 0.5000 % revenue fall against the `DSCR` covenant's 3.8796 % ✅
- C. neither — they bite at the same point by construction
- D. it cannot be determined without the lock-up threshold

*Rationale:* Restating both in revenue units gives 60,000 of headroom on interest cover against 465,547.16 on `DSCR`, a factor of 7.7591 (2.4.2). A substitutes convention for arithmetic; C asserts a relationship that does not exist; D treats a separate distribution test as necessary to a covenant comparison.


**2.4-F** `[2.4.3 · Analysis]` At a construction data date a project has incurred 33,945,403 and capitalised 34,003,326. The most likely explanation is:

- A. an error, since capitalised cost cannot exceed cost incurred
- B. capitalised interest entered the asset without a supplier invoice, more than offsetting costs that failed the capitalisation test ✅
- C. the asset has been revalued upwards
- D. retention withheld has been added to the asset

*Rationale:* Capitalised interest of 677,923 exceeds the 620,000 of non-capitalisable owner's costs, so the asset legitimately carries 57,923 more than the cost report shows (2.4.3). A applies an intuition the reconciliation disproves; C invents a transaction; D confuses a payment timing item with a cost.


## Domain 3

**3.1-A** `[3.1.2 · Application]` A payment of USD 500,000 is due in 5 years; the discount rate is 7 %. Its present value is closest to:

- A. USD 370,370
- B. USD 356,493 ✅
- C. USD 381,448
- D. USD 701,276

*Rationale:* `500,000 / 1.07⁵ = 356,493`. A discounts with simple interest `(1 + 0.35)`; C uses four years instead of five; D compounds forward instead of discounting back.


**3.1-B** `[3.1.3 · Application]` At a 10 % discount rate, the discount factor for year 3 is:

- A. 0.7000
- B. 0.8264
- C. 0.7513 ✅
- D. 0.6830

*Rationale:* `1/1.10³ = 0.7513`. A subtracts 10 % three times (simple); B is the year-2 factor; D is the year-4 factor — the two most common off-by-one-period errors.


**3.1-C** `[3.1.1 · Analysis]` A lender quotes 25-year money and a borrower's analyst tests affordability using simple interest "as a conservative shortcut". The analyst's error is that:

- A. simple interest overstates the debt cost, so the test is merely too strict
- B. compound growth exceeds simple growth over long horizons, so the shortcut materially understates the true accumulation ✅
- C. the two methods converge over long horizons, so the shortcut is harmless
- D. simple interest cannot be computed for horizons beyond ten years

*Rationale:* Compounding produces growth on growth: at 8 % over 25 years the compound multiple is ≈ 6.85 versus 3.0 simple — the shortcut is anti-conservative, the opposite of the analyst's claim (so A is wrong); the methods diverge rather than converge (C); D is arithmetic nonsense.


**3.1-D** `[3.1.3 · Recall]` Which statement about discount factors is an invariant a reviewer can test without knowing the project?

- A. `DF(t)` rises when cash flows are contracted
- B. `DF(t+1) = DF(t) / (1 + r)` for every `t` ✅
- C. `DF(t)` equals `1 − r × t`
- D. factors below 0.5 indicate an error

*Rationale:* Each factor is the prior factor discounted one more period — B holds for any `r`. A confuses risk of flows with the factor row; C is the simple-interest approximation; D is false — any long horizon at a normal rate passes 0.5 (see Fig 3.1.2).


**3.1-E** `[3.1.2 · Application]` At 9 %, which is worth more today: USD 400,000 in 2 years, or USD 470,000 in 4 years?

- A. the 470,000 — larger amounts always win
- B. the 400,000: PV 336,672 vs 332,960 ✅
- C. the 470,000: PV 395,590 vs 336,672
- D. they are equal at 9 %

*Rationale:* `400,000/1.09² = 336,672`; `470,000/1.09⁴ = 332,960` — the earlier, smaller amount wins by USD 3,712. A ignores discounting entirely; C discounts the 470,000 across only two periods instead of four; D asserts an equality the arithmetic denies.


**3.1-F** `[3.1.2 · Application]` USD 250,000 is invested at 7 % compound for 6 years. Its future value is closest to:

- A. USD 355,000
- B. USD 350,638
- C. USD 375,183 ✅
- D. USD 166,586

*Rationale:* `250,000 × 1.07⁶ = 375,183`. A is simple interest (`1 + 0.07 × 6`); B compounds only 5 years; D divides instead of multiplying — discounting when the question asks for growth.


**3.1-G** `[3.1.2 · Analysis]` A counterparty offers **USD 380,000 today** to extinguish an obligation to pay USD 500,000 in exactly five years. The annual rate of return the offer implies for the party giving up the wait is closest to:

- A. 4.80 %
- B. 5.64 % ✅
- C. 6.32 %
- D. 7.00 %

*Rationale:* `(500,000/380,000)^(1/5) − 1 = 5.6422 %`. A divides the USD 120,000 premium by the *future* amount and by five (`120,000/500,000/5`) — a simple return computed on the wrong base; C annualises the 31.5789 % total premium by dividing by five, the simple-interest shortcut of KA 3.1.1 applied to a rate; D is the board's own required return, which is what the implied rate must be *compared against*, not the answer to the question asked.


**3.1-H** `[3.1.3 · Comprehension]` A reviewer wants one arithmetic test that ties a twelve-row discount-factor column to the single annuity-factor cell that summarises it, without re-adding the column. The correct relationship is:

- A. `AF(r,n) × r = 1 − DF(n)` ✅
- B. `AF(r,n) = n × DF(n)`
- C. `AF(r,n) × DF(n) = 1`
- D. `AF(r,n) = 1/r − DF(n)`

*Rationale:* At 6 % over 12 years, `8.383844 × 0.06 = 0.503031 = 1 − 0.496969` (WE 3.1.3b). B averages the row by its last term and gives 5.963632; C inverts a relationship that holds for no `r` (here 4.166514); D drops the division of `DF(n)` by `r` from the closed form and gives 16.169697 — each a plausible-looking rearrangement, and each falsifiable in one cell.


**3.2-A** `[3.2.2 · Application]` A USD 42,000,000 loan is repaid by 12 equal annual instalments at 6 %. The instalment is closest to:

- A. USD 3,500,000
- B. USD 2,520,000
- C. USD 5,009,635 ✅
- D. USD 5,706,454

*Rationale:* `42,000,000 × 0.06/(1 − 1.06⁻¹²) = 5,009,635`. A divides principal by 12 and ignores interest; B is interest-only on the full balance; D uses a 10-year annuity factor — the wrong-tenor error.


**3.2-B** `[3.2.3 · Application]` A 6 % nominal rate compounded quarterly has an effective annual rate of:

- A. 6.00 %
- B. 6.09 %
- C. 6.14 % ✅
- D. 6.17 %

*Rationale:* `(1 + 0.015)⁴ − 1 = 6.136 % ≈ 6.14 %`. A is the nominal itself; B is semi-annual compounding; D is monthly — each a frequency misread.


**3.2-C** `[3.2.1 · Analysis]` Two otherwise identical concession bids value the same 20-year availability stream. Bid X used the full-precision annuity factor; Bid Y rounded the factor to four decimals. The most defensible statement is:

- A. the bids differ by an amount that grows with the size of the stream, and only X's practice passes model audit ✅
- B. rounding the factor is conservative, so Y understates value and is safer
- C. the difference is always immaterial because four decimals is industry standard
- D. Y's approach is wrong only if the discount rate exceeds 10 %

*Rationale:* Factor rounding error scales linearly with the cash flows and has no consistent conservative direction (B, D wrong); materiality depends on scale, not convention (C wrong); model audit standards require full-precision arithmetic with display-only rounding.


**3.2-D** `[3.2.1 · Application]` A 5-year, USD 500,000-per-year lease payable **in advance** is valued at 8 %. Its present value is closest to:

- A. USD 1,996,355
- B. USD 2,156,063 ✅
- C. USD 2,500,000
- D. USD 1,848,477

*Rationale:* Annuity-due = ordinary annuity × `(1+r)`: `500,000 × 3.992710 × 1.08 = 2,156,063`. A prices it as an ordinary annuity (the classic misread); C is the undiscounted total; D divides the ordinary value by 1.08 — the adjustment applied backwards.


**3.2-E** `[3.2.2 · Analysis]` For the same principal, tenor and rate, which repayment shape has the lowest lifetime interest, and why?

- A. the annuity — its instalments are level, so interest is averaged down
- B. the bullet — deferral shrinks the money's time exposure
- C. level-principal — the balance falls fastest, so less principal is outstanding for less time ✅
- D. all three are equal because rate and tenor are equal

*Rationale:* Interest is rate × outstanding balance × time; level-principal retires balance fastest (Kestrel: 16.38m vs 18.12m annuity vs 30.24m bullet). A's "averaging" is not a mechanism; B reverses the truth — the bullet maximises exposure; D ignores the balance path entirely.


**3.2-F** `[3.2.1 · Analysis]` A wayleave pays USD 800,000 per year; the discount rate is 8 %. Modelling it as a 30-year annuity instead of a perpetuity understates its value by:

- A. USD 993,773 — the present value of the post-year-30 tail ✅
- B. nothing material — thirty years is effectively forever
- C. USD 4,000,000 — one-third of the perpetuity value
- D. it overstates value, because perpetuities are riskier

*Rationale:* Perpetuity `800,000/0.08 = 10,000,000`; 30-year annuity `800,000 × 11.257783 = 9,006,227`; the gap — 9.9 % of value — is the discounted tail. B waves away a million dollars; C invents a fraction; D confuses valuation arithmetic with a risk adjustment that belongs in the rate, not the formula choice.


**3.2-G** `[3.2.1 · Application]` A 25-year availability payment of USD 5,600,000 in year-0 terms is **fully indexed at 2.5 %** a year; the discount rate is 8.0 %. Its present value is closest to:

- A. USD 59,778,747
- B. USD 76,111,457 ✅
- C. USD 99,768,254
- D. USD 104,363,636

*Rationale:* `r* = 1.08/1.025 − 1 = 5.3659 %`, `AF(r*, 25) = 13.591332`, `PV = 5,600,000 × 13.591332 = 76,111,457` (WE 3.2.1d). A values the stream as level and ignores the indexation entirely; C escalates the flows **and** discounts at `r*`, the double-count of WE 3.2.1d; D applies the growing-perpetuity form `A(1+g)/(r − g)` and so values a stream that never ends.


**3.2-H** `[3.2.2 · Evaluation]` Kestrel's sponsor obtains a three-year repayment holiday on the unchanged USD 42,000,000 / 12-year / 6.0 % facility, against a **1.30× minimum `DSCR`** covenant. The instalment for years 4 to 12 becomes USD 6,174,934 and year-4 `DSCR` falls to 1.0339 against documented `CFADS` of 6,384,000. The soundest professional conclusion is:

- A. the holiday is cash-neutral, since the same principal is repaid over the same maturity and the lender's return is unchanged
- B. the holiday is prudent: a year-1 `DSCR` of 2.5333 against a 1.30× covenant shows the structure is comfortably covered
- C. the holiday concentrates rather than reduces debt service, costs USD 3,018,782 of extra interest and misses the covenant by USD 1,643,414 of `CFADS` ✅
- D. the holiday reduces lifetime interest, because principal repayment is deferred and interest accrues on a smaller average balance

*Rationale:* Lifetime interest rises from 18,115,623 to 21,134,405 and the binding test moves from year 1 to year 4, where `6,174,933.87 × 1.30 = 8,027,414.03` is needed against 6,384,000 available (WE 3.2.2c), so the holiday must be paired with a maturity extension or a smaller drawing. A ignores the three extra years of interest on an undiminished balance; B reads a single non-binding period as the covenant position, the specific error a holiday induces; D reverses the direction — deferring principal leaves the balance *larger*, not smaller, so interest rises.


**3.2-I** `[3.2.3 · Application]` Kestrel's USD 42,000,000 / 12-year / 6.0 % facility carries an USD 840,000 arrangement fee deducted from proceeds. Its all-in effective cost is closest to:

- A. 6.0000 %
- B. 6.1667 %
- C. 6.3704 % ✅
- D. 8.0000 %

*Rationale:* Solving `Σ 5,009,635.23/(1 + r)^t = 41,160,000` over 12 years gives 6.3704 % (WE 3.2.3b). A quotes the headline and ignores the fee; B spreads the 2.0 % fee straight-line across twelve years and adds 16.67 basis points to the coupon — the "amortise the fee" error, which omits the time value of paying it at once; D adds the whole 2.0 % to the rate, an error of a full 163 basis points in the same direction.


**3.3-A** `[3.3.1 · Application]` Nominal return 9 %, inflation 3 %. The real return is closest to:

- A. 6.00 %
- B. 5.83 % ✅
- C. 12.27 %
- D. 3.00 %

*Rationale:* `1.09/1.03 − 1 = 5.83 %`. A is the subtraction approximation; C multiplies `1.09 × 1.03 − 1` (compounding inflation on instead of off); D confuses the inflation rate itself with the real return.


**3.3-B** `[3.3.2 · Application]` A USD 10,000,000 cost escalating at 4 % per year is, in year 3:

- A. USD 11,200,000
- B. USD 10,400,000
- C. USD 12,486,400
- D. USD 11,248,640 ✅

*Rationale:* `10,000,000 × 1.04³ = 11,248,640`. A escalates simply (3 × 4 %); B stops at one year; C applies four periods' escalation with a decimal slip.


**3.3-C** `[3.3.1 · Analysis]` A model discounts real (uninflated) cash flows at the sponsor's nominal 9 % hurdle. The result:

- A. overstates value, because inflation is counted twice
- B. understates value, because inflation is removed from the flows but still charged in the rate ✅
- C. is correct if inflation is below 5 %
- D. is correct because discount rates are always nominal

*Rationale:* Real flows with a nominal rate deduct inflation twice — once from the flows, once inside the rate — so value is systematically understated. The consistency rule admits no threshold (C) and no default (D); A reverses the direction of the error.


**3.3-D** `[3.3.4 · Application]` A USD 42,000,000 balance accrues at 6 % over a 92-day quarter. Under **actual/360** the interest is:

- A. USD 630,000
- B. USD 635,178
- C. USD 644,000 ✅
- D. USD 620,548

*Rationale:* `42,000,000 × 0.06 × 92/360 = 644,000`. A is 30/360 (90/360); B is actual/365; D uses 90/365 — mixing the two conventions' halves.


**3.3-E** `[3.3.2 · Analysis]` A 25-year operating model escalates O&M at "4 % simple" to be "conservative". The effect is:

- A. conservative — simple escalation always overstates cost
- B. anti-conservative — compound escalation exceeds simple by an amount that grows every year, so late-life costs are materially understated ✅
- C. neutral — the choice only redistributes cost between years
- D. correct — operating contracts always escalate simply

*Rationale:* Escalation compounds (contracts index on last year's indexed price): by year 25 the compound multiple `1.04²⁵ ≈ 2.67` far exceeds the simple `2.00`. A claims the reverse; C ignores the widening gap; D asserts a contractual universal that KA 3.3.2's indexation discipline contradicts.


**3.3-F** `[3.3.1 · Application]` A model shows a nominal cash flow of USD 2,000,000 in year 5; inflation is 3 %. Its value in today's purchasing power (real terms) is:

- A. USD 2,000,000 — real and nominal are equal at year 5
- B. USD 1,725,218 ✅
- C. USD 1,700,000
- D. USD 2,318,548

*Rationale:* `2,000,000 / 1.03⁵ = 1,725,218` — deflating by the price level, not discounting for time value (that happens separately, at a real rate). A ignores five years of inflation; C deflates simply (`2,000,000 × (1 − 0.03 × 5) = 1,700,000`); D multiplies by `1.03⁵` — inflating instead of deflating.


**3.3-G** `[3.3.1 · Analysis]` A 25-year stream worth USD 5,600,000 a year **in year-0 purchasing power** is valued against a 9.0 % nominal hurdle with inflation at 3.0 %. Which statement is correct?

- A. discounting the nominal (escalated) flows at 9.0 % and discounting the level real flows at 5.8252 % both give USD 72,791,113 — they are arithmetically identical ✅
- B. discounting the level real flows at 9.0 % is correct, and gives USD 55,006,446
- C. discounting the nominal flows at 5.8252 % is correct, and gives USD 100,366,400
- D. the real treatment is an approximation of the nominal treatment and the two differ by the inflation cross term

*Rationale:* The Fisher relation makes the two consistent treatments equal to the cent (WE 3.3.1b). B is the double-deduction defect, understating value by 24.4325 %; C is the mirror defect, overstating it by 37.8828 %; D asserts an approximation where an identity holds — the cross term is *inside* `i_real`, which is why the subtraction shortcut, and not the exact relation, is the approximation.


**3.3-H** `[3.3.2 · Evaluation]` An O&M obligation of USD 2,700,000 at base date runs 25 years and is fully indexed; the forecast index is 4.0 % and the discount rate 8.0 %. A negotiator secures a **3.0 % cap** on annual indexation. The value of that concession to the payer is closest to:

- A. nil — the cap only bites if the index exceeds 3.0 %, which is a future event
- B. USD 398,941
- C. USD 1,544,558
- D. USD 4,258,610 ✅

*Rationale:* `2,700,000 × [AF(3.846154 %, 25) − AF(4.854369 %, 25)] = 42,873,960 − 38,615,349 = 4,258,610` (WE 3.3.2b). A contradicts its own premise — the forecast *is* 4.0 %, so on the stated assumptions the cap bites in every period; B is the correct value expressed as a **level annual equivalent** (`÷ AF(0.08, 25)`), a right number answering a different question; C is the **year-25 single-period** saving, undiscounted and counted once.


**3.3-I** `[3.3.4 · Application]` A USD 42,000,000 balance accrues at 6.0 % on an **actual/360** basis across a full 365-day year. The interest charged, and the quoted rate the convention is equivalent to, are:

- A. 2,520,000 and 6.0000 %
- B. 2,555,000 and 6.0833 % ✅
- C. 2,562,000 and 6.1000 %
- D. 2,485,479 and 5.9178 %

*Rationale:* `42,000,000 × 0.06 × 365/360 = 2,555,000`, and `0.06 × 365/360 = 6.0833 %` (WE 3.3.4b). A is the 30/360 figure, which charges 360 days over 360; C is a 366-day leap year on the same convention; D inverts the fraction to `360/365`, the direction error that makes an expensive convention look cheap.


## Domain 4

**4.1-A** `[4.1.1 · Application]` A project costs USD 60,000,000 and returns USD 8,900,000 per year for 15 years; the discount rate is 8 % (`AF = 8.559479`). Its NPV is:

- A. +USD 16,179,360 ✅
- B. +USD 73,500,000
- C. −USD 16,179,360
- D. +USD 76,179,360

*Rationale:* `8,900,000 × 8.559479 − 60,000,000 = +16,179,360`. B is the undiscounted surplus (8.9 × 15 − 60); C reverses the sign (investment minus value); D forgets to deduct the investment at all.


**4.1-B** `[4.1.2 · Analysis]` A project's cash flows are −1,000,000, +2,300,000, −1,320,000. Which statement is correct?

- A. its IRR is 10 %
- B. its IRR is 20 %
- C. it has two IRRs (10 % and 20 %), so decision by IRR is indeterminate and NPV at the cost of capital must decide ✅
- D. it has no IRR, so it must be rejected

*Rationale:* Two sign changes admit two roots — both 10 % and 20 % zero the NPV, which is positive between them. A and B are each half the truth and therefore wrong as "the" IRR; D confuses indeterminate ranking with non-viability — at a 15 % cost of capital the NPV is (slightly) positive.


**4.1-C** `[4.1.3 · Application]` Using an 8 % finance and reinvestment rate, the master appraisal's terminal value of inflows is USD 241,653,814 on an investment of USD 60,000,000 over 15 years. MIRR is closest to:

- A. 12.19 %
- B. 9.73 % ✅
- C. 8.00 %
- D. 26.85 %

*Rationale:* `(241,653,814/60,000,000)^(1/15) − 1 = 9.73 %`. A is the unmodified IRR; C is the reinvestment rate itself; D divides the 4.03× money multiple by 15 as if returns were simple.


**4.1-D** `[4.1.1 · Analysis]` Which property justifies NPV's primacy over IRR for accept/reject *and* ranking decisions?

- A. NPV is easier to compute
- B. NPV is expressed as a percentage
- C. NPV is additive and scale-aware, and assumes reinvestment at the opportunity cost ✅
- D. NPV never requires a forecast

*Rationale:* The three structural properties of 4.1.1. A is irrelevant and untrue at scale; B describes IRR, not NPV; D is false — NPV consumes the same forecasts as every measure.


**4.2-A** `[4.2.1 · Application]` For the master appraisal (60m in, 8.9m/yr, 8 %), the discounted payback is closest to:

- A. 6.74 years
- B. 10.07 years ✅
- C. 8.00 years
- D. 15.00 years

*Rationale:* Cumulative PV reaches 59.72m after year 10; the 0.28m shortfall is 7 % of year 11's 3.82m discounted flow → 10.07. A is the simple payback; C confuses the discount rate with a duration; D is the whole life.


**4.2-B** `[4.2.3 · Application]` System A (PV of costs 7,061,678 over 3 years, `AF = 2.577097`) versus System B (PV 9,596,355 over 5 years, `AF = 3.992710`). The correct comparison and choice is:

- A. raw PV: A is cheaper, choose A
- B. equivalent annual cost: A 2,740,168 vs B 2,403,469 — choose B ✅
- C. equivalent annual cost: A 2,353,893 vs B 1,919,271 — choose B
- D. purchase price: A is cheaper, choose A

*Rationale:* Unequal lives require EAC: `7,061,678/2.577097 = 2,740,168` vs `9,596,355/3.992710 = 2,403,469`. A and D compare unlike horizons; C divides each PV by its raw life in years (÷3 and ÷5), annualising without discounting — the right instinct with the wrong arithmetic.


**4.2-C** `[4.2.1 · Analysis]` A board adopts "payback under 5 years" as its sole investment criterion. The predictable portfolio distortion is:

- A. none — payback is conservative, so the portfolio is safe
- B. systematic bias against long-lived infrastructure and toward short-cycle projects, regardless of value created ✅
- C. excessive investment in high-NPV projects
- D. elimination of all risk

*Rationale:* Payback ignores everything beyond its cut-off, so a 15-year concession with NPV +16m loses to a 4-year project with NPV +1m. That is a value distortion, not conservatism (A); C reverses the effect; D confuses shorter exposure with no risk.


**4.2-D** `[4.2.2 · Recall]` The profitability index earns its place in appraisal when:

- A. projects are mutually exclusive and differ in scale
- B. capital is rationed and the question is which portfolio of positive-NPV projects fits the budget ✅
- C. cash flows change sign more than once
- D. lives are unequal

*Rationale:* PI ranks value per scarce dollar — the rationing question exactly. A is where PI (like IRR) mis-ranks by scale; C is MIRR's territory; D is EAV's.


**4.3-A** `[4.3.1 · Application]` P: NPV +2,985,420, IRR 28.65 %. Q: NPV +3,956,260, IRR 15.24 %. Cost of capital 8 %; only one may proceed. The correct choice is:

- A. P — higher IRR
- B. Q — higher NPV, confirmed by an incremental IRR of 10.42 % above the hurdle ✅
- C. P — lower investment is always safer
- D. both, split the budget

*Rationale:* Exclusive choices rank by money added: Q adds USD 970,840 more, and the extra 15m earns 10.42 % > 8 %. A is the scale-blindness pathology; C prices fear, not value; D violates the premise.


**4.3-B** `[4.3.2 · Application]` Budget USD 20m. Projects (I₀, NPV): W (8, 2.4), X (12, 3.0), Y (10, 2.2), Z (6, 1.02). The value-maximising funded set is:

- A. X and Y
- B. W and X: NPV +5.40m ✅
- C. W, Y and Z
- D. X and Z

*Rationale:* PI order W (1.300), X (1.250) packs the budget exactly for +5.40m. A needs 22m; C needs 24m; D fits (18m) but yields +4.02m — a 1.38m sacrifice for leaving W unfunded.


**4.3-C** `[4.3.3 · Analysis]` A pilot plant shows NPV −800,000, but building it creates the option to deploy at 20× scale if the technology proves. The appraisal-sound treatment is:

- A. reject — negative NPV is disqualifying
- B. approve by overriding the NPV silently
- C. present the static NPV alongside an explicit valuation (or structured judgment) of the scaling option, and decide on the combined case, recorded as such ✅
- D. raise the forecast cash flows until NPV turns positive

*Rationale:* Static NPV cannot see option value; the remedy is to price or judge the option *in the open*. A discards real value; B and D are the same dishonesty at different altitudes — one hides the judgment, the other disguises it as a forecast.


**4.3-D** `[4.3.1 · Recall]` The crossover rate of two NPV profiles is:

- A. the rate at which both projects' NPVs equal zero
- B. the rate at which the two NPVs are equal — above it, the ranking flips ✅
- C. the average of the two IRRs
- D. the cost of capital

*Rationale:* Crossover is the intersection of profiles (equivalently the incremental project's IRR). A describes two separate IRRs; C is arithmetic superstition; D is a property of the firm, not of the pair.


## Domain 5

**5.1-A** `[5.1.2 · Application]` A programme spends 1,000,000 screening, 3,000,000 on concept, 6,000,000 on feasibility and bid and 4,800,000 carrying two projects to close. Development cost per closed project is:

- A. USD 2,400,000
- B. USD 7,400,000 ✅
- C. USD 4,800,000
- D. USD 14,800,000

*Rationale:* `14,800,000/2 = 7,400,000`. A is the closing-stage unit cost, excluding the portfolio that produced the winners; C is the whole closing stage undivided; D attributes the entire programme to one project.


**5.1-B** `[5.1.2 · Analysis]` A programme spends 14,800,000 across a funnel of 40 screened opportunities and delivers 16,179,360 of value per close. Its breakeven close rate is closest to:

- A. 5.00 %
- B. 2.29 % ✅
- C. 45.74 %
- D. 91.47 %

*Rationale:* breakeven closes `= 14,800,000/16,179,360 = 0.9147`; `÷ 40 = 2.29 %`. A is the achieved close rate; C divides by the 2 closes achieved rather than the 40 screened; D quotes the breakeven closes as if it were a percentage.


**5.1-C** `[5.1.3 · Application]` A fatal flaw is present with probability 0.40 and costs 3,300,000 if it survives to diligence. A gate costing 180,000 detects it with probability 0.75. The gate's net value, ignoring elapsed time, is:

- A. USD 1,320,000
- B. USD 810,000 ✅
- C. USD 990,000
- D. USD 330,000

*Rationale:* `1,320,000 − [180,000 + 0.40 × 0.25 × 3,300,000] = 810,000`. A is the expected waste without the gate; C omits the gate's 180,000 cost; D is the residual expected waste after it.


**5.1-D** `[5.1.3 · Analysis]` A gate worth **+810,000** per project before elapsed time is counted adds 8 weeks and so carries a 10 % chance of missing a bid window on a project worth 16,179,360. The correct conclusion is:

- A. the gate still pays, since 810,000 is positive
- B. as designed it destroys 807,936 of value, so it should be run concurrently or staged rather than abolished ✅
- C. it should be abolished, since delay always dominates in competitive procurement
- D. bid-window risk is not a financial cost and is excluded

*Rationale:* `810,000 − 0.10 × 16,179,360 = −807,936` against a 5.01 % breakeven window-miss probability — the design, not the review, is at fault. A ignores the option cost; C forfeits 810,000 of detection value; D is the omission the arithmetic exists to prevent.


**5.2-A** `[5.2.3 · Application]` Equity of 18,000,000 is split 55/35/10, with a several cost-overrun support of 10 % of a 60,000,000 capital cost subscribed pro rata. The 35 % sponsor's total committed capital is:

- A. USD 6,300,000
- B. USD 8,400,000 ✅
- C. USD 2,100,000
- D. USD 12,300,000

*Rationale:* `18,000,000 × 0.35 = 6,300,000` plus `6,000,000 × 0.35 = 2,100,000`. A is base equity only, **25.0 % below** the committed figure — the omission the example corrects, and the same gap seen the other way round as the 33.3 % that support adds to every sponsor's equity share; C is the support alone; D reads the support as joint and several, adding the whole 6,000,000 pool.


**5.2-B** `[5.2.3 · Analysis]` The 55 % sponsor holds 9,900,000 of an 18,000,000 equity ticket and subscribes pro rata to a 6,000,000 cost-overrun support pool. The agreement is amended from several to joint and several liability for that support. The 55 % sponsor's worst-case exposure becomes:

- A. unchanged at 13,200,000
- B. 15,900,000 — its own 9,900,000 of equity plus the whole 6,000,000 pool ✅
- C. 24,000,000
- D. 6,000,000

*Rationale:* the sponsor becomes pursuable for the entire support commitment: `9,900,000 + 6,000,000`, a 20.5 % rise against a 3.25× rise for the 10 % holder. A is the several answer; C is the group's total committed capital, applicable only if equity subscriptions were also joint and several; D omits the sponsor's own equity.


**5.2-C** `[5.2.4 · Analysis]` An equity bridge at 5.5 % replaces pro-rata equity of 9,000,000 at t = 0 and t = 1 with 19,512,225 at t = 2. At 5.5 % both profiles are worth 17,530,806. The correct conclusion is:

- A. the bridge creates 1,512,225 of value
- B. it creates no value at the bridge rate; its benefit is entirely the spread between the bridge rate and the sponsors' required return ✅
- C. it destroys 1,512,225 of value
- D. it is value-neutral at every discount rate

*Rationale:* identical present values prove neutrality *at that rate*; at 12 % the saving is 1,480,688. A treats accrued interest as a gain; C treats it as a pure cost and ignores the deferral; D generalises the identity beyond the rate at which it holds.


**5.2-D** `[5.2.2 · Analysis]` A sponsor states that the SPV caps its exposure at its equity subscription. The most accurate correction is:

- A. correct — that is the purpose of the ring-fence
- B. the fence is punctured by every support obligation given, and consolidation, tax and reputational consequences are decided outside it ✅
- C. incorrect — sponsors are always liable for all project debt
- D. correct, provided the vehicle is bankruptcy-remote

*Rationale:* committed capital, not subscribed equity, is the exposure (5.2.3), and consolidation and tax turn on control and framework. C describes full recourse, which limited-recourse structures exist to avoid; D confuses insolvency engineering with the scope of contractual support.


**5.3-A** `[5.3.1 · Application]` Six bankability conditions have probabilities 0.92, 0.90, 0.95, 0.88, 0.93 and 0.85. The joint probability of close, assuming independence, is closest to:

- A. 90.5 %
- B. 54.72 % ✅
- C. 85.0 %
- D. 43.0 %

*Rationale:* the product is 0.5472. A is the arithmetic mean — the error the example corrects; C quotes the weakest condition as though the others were certain; D sums the six shortfalls (0.57) and subtracts the total from one, over-counting the failures by treating them as mutually exclusive.


**5.3-B** `[5.3.1 · Analysis]` In a six-condition set whose joint probability is 0.5472, one week of effort can lift either the financing condition from 0.85 to 0.95 or the land condition from 0.95 to 0.98. The value-maximising choice is:

- A. land, because 0.98 is the higher absolute probability
- B. financing, which adds 6.44 points against land's 1.73 — a factor of 3.73 ✅
- C. either, since both raise one condition by a similar amount
- D. neither, because correlation makes the calculation meaningless

*Rationale:* marginal gain is the joint probability times the proportional lift: `0.5472 × (0.95/0.85 − 1) = 6.4375` points against `0.5472 × (0.98/0.95 − 1) = 1.7280`. A ranks by level rather than gain; C ignores that a proportional lift on a low base moves the product much more; D discards a ranking correlation does not reverse.


**5.3-C** `[5.3.4 · Application]` A credit committee raises the target `DSCR` from 1.30× to 1.45× for lack of operating references. On `CFADS` of 6,384,000 over 12 years at 6 % (`AF` = 8.383844), the additional equity required is closest to:

- A. USD 4,259,082 ✅
- B. USD 6,300,000
- C. USD 508,011
- D. USD 4,910,769

*Rationale:* capacity falls from 41,171,123 to 36,912,041. B applies the 0.15 ratio increase to the 42,000,000 of debt as though ratio points were percentages of principal; C is the fall in annual debt service; D is the maximum debt *service* at 1.30×, a per-period figure mistaken for a capital sum.


**5.3-D** `[5.3.2 · Analysis]` A project has a signed 20-year offtake with a counterparty whose payment obligations are unsupported and whose credit lenders assess as weak. The correct conclusion is:

- A. bankable — a signed long-tenor offtake is the strongest possible condition
- B. the revenue condition fails on counterparty credit; it is repaired by credit support or by resizing, not by the contract's length ✅
- C. bankable if the tariff is indexed
- D. unbankable permanently

*Rationale:* an offtake is worth the offtaker's ability to pay, so tenor and indexation do not cure credit (A, C). D overstates: credit support and resizing are the standard structural remedies.


**5.4-A** `[5.4.2 · Application]` Debt of 42,000,000 is fully drawn at 6.0 %; annual `CFADS` would be 6,384,000; 30/360 applies. The daily economic cost of a COD slip is:

- A. USD 17,733.33
- B. USD 24,733.33 ✅
- C. USD 7,000.00
- D. USD 26,400.00

*Rationale:* `42,000,000 × 0.06/360 = 7,000` of interest plus `6,384,000/360 = 17,733.33` of forgone `CFADS`. A is the forgone-`CFADS` side alone — the calibration error leaving 28.3 % uncovered; C is the interest alone; D uses the pre-working-capital `CFADS` of 6,984,000 (Domain 2's other definition, 19,400 per day).


**5.4-B** `[5.4.2 · Analysis]` The daily economic cost of a COD slip is 24,733.33. Delay damages are 20,000 per day, capped at 10 % of a 48,000,000 EPC price. For a 360-day slip the SPV bears:

- A. nothing — the damages cover the delay
- B. USD 4,104,000, because the cap binds at day 240 against an economic cost of 8,904,000 ✅
- C. USD 1,704,000
- D. USD 8,904,000

*Rationale:* cap `= 4,800,000`, binding at `4,800,000/20,000 = 240` days; cost `= 360 × 24,733.33 = 8,904,000`; uncovered `= 4,104,000`. A ignores the cap; C computes damages as though all 360 days were payable (7,200,000) and subtracts; D omits recovery altogether.


**5.4-C** `[5.4.2 · Analysis]` A facility of 42,000,000 over 12 years at 6 % (`AF` = 8.383844) carries a 1.20× cash covenant against `CFADS` of 6,384,000, leaving annual headroom of 372,438. 1,260,000 of extra construction interest is capitalised, taking debt to 43,260,000 at the same tenor and rate. The most important consequence for the operating period is:

- A. none — the debt is repaid over the same period
- B. annual covenant headroom falls from 372,438 to 192,090.85, roughly halving it for the whole loan life ✅
- C. the loan tenor extends
- D. `CFADS` falls by 1,260,000

*Rationale:* the instalment rises to `43,260,000/8.383844 = 5,159,924.29`, `DSCR` falls to 1.2372 and the 1.20× trigger rises to 6,191,909. A ignores that debt service rises; C is a possible structural response, not an automatic consequence; D confuses a financing cost with cash generation, which is unchanged.


**5.4-D** `[5.4.3 · Application]` A 3 % output shortfall reduces `CFADS` from 6,384,000 to 6,112,200 against debt of 42,000,000. The buy-down restoring the originally sized coverage is:

- A. USD 271,800
- B. USD 1,788,158 ✅
- C. USD 1,260,000
- D. USD 4,259,082

*Rationale:* at constant coverage debt is proportional to `CFADS`, so the buy-down is `42,000,000 × 271,800/6,384,000`. A is the annual `CFADS` shortfall, not the debt adjustment; C is the COD-slip interest of 5.4.2; D is the technology premium of 5.3.4 — correct numbers in the wrong place.


## Domain 6

**6.1-A** `[6.1.3 · Analysis]` Kestrel's appraisal shows `NPV` +16,179,360 (pre-tax operating cash, 15 years) and its financing model shows +2,767,684 (post-tax unlevered, flat, 25 years). The correct conclusion is:

- A. the financing model contains an error of 13,411,676
- B. the appraisal is optimistic and should be discarded
- C. both are correct on their stated basis, horizon and case, and the required deliverable is the bridge between them ✅
- D. the difference is the interest tax shield

*Rationale:* Worked example 6.1.3 reconciles them exactly — the appraisal's 8,900,000 is the fifteen-year level equivalent of an `EBITDA` stream escalating at 2.967 %, and the gap is basis, horizon and case. A and B assume one number must be wrong; D names a term that is absent from both, since each figure here is unlevered.


**6.1-B** `[6.1.2 · Application]` Kestrel's construction spend is modelled with the same profile and the same 6.0 % rate on an annual rather than a quarterly timeline, interest accruing on the opening balance in both. Capitalised interest changes from 2,114,597 to:

- A. 2,114,597 — periodicity does not affect a total
- B. 1,247,352, an understatement of 867,245 ✅
- C. 2,427,554
- D. 8,458,388

*Rationale:* Coarse periods ignore intra-period draws, so the opening balance on which interest accrues is far too small (6.1.2, Worked example 6.2.1). A assumes periodicity is presentational; C is the quarterly figure on the *average*-balance convention; D multiplies the quarterly figure by four, confusing periods with rates.


**6.1-C** `[6.1.1 · Analysis]` A reviewer finds a 20 % tax rate typed inside three calculation formulae rather than referenced from the inputs block. The most serious consequence is:

- A. the model is harder to read
- B. the tax rate cannot be changed
- C. scenario switches silently leave the tax rate unchanged, so every case in the model is internally inconsistent in a way no output reveals ✅
- D. the model will not balance

*Rationale:* The hard-coded constant survives the scenario switch, so the downside case is run at the base-case tax rate and nothing on any output page says so (6.1.1). A understates it; B is false — it can be changed, three times, which is the problem; D is wrong, because a consistent wrong number balances perfectly (6.4.1).


**6.2-A** `[6.2.1 · Application]` Kestrel's committed uses are 48,000,000 of EPC, 3,600,000 of owner's costs, 1,800,000 of development costs and 840,000 of fees, inside a 60,000,000 envelope, with capitalised interest computed at 2,114,597. The contingency is:

- A. 3,600,000
- B. 3,645,403 ✅
- C. 5,760,000
- D. 6,000,000

*Rationale:* `60,000,000 − 48,000,000 − 3,600,000 − 1,800,000 − 840,000 − 2,114,597 = 3,645,403` (6.2.1). A is a plausible round number and therefore the tell-tale of a plugged table; C omits capitalised interest from the deduction; D applies a 10 % rule of thumb to the envelope instead of computing the residual.


**6.2-B** `[6.2.2 · Application]` Operating cash flow is 3,864,000 and interest paid, included in operating cash flow, is 2,520,000. `CFADS` is:

- A. 1,344,000
- B. 3,864,000
- C. 6,384,000 ✅
- D. 8,904,000

*Rationale:* `CFADS` = operating cash flow + interest paid (6.2.2). A deducts interest a second time; B forgets that operating cash flow is already struck after interest; D adds interest a second time to a `CFADS` already struck before debt service, a figure no definition produces — and principal, a financing flow, never enters operating cash flow at all.


**6.2-C** `[6.2.3 · Analysis]` A model of a project with flat revenue and level annuity debt service shows cash tax of 516,000 against taxable profit of 2,580,000 in year one and, in year eight, cash tax of 766,771 against taxable profit of 3,833,856. The statutory rate is 20 %. A reviewer should conclude:

- A. the tax line is wrong, because cash tax has risen while revenue is flat
- B. the effective rate is 20.0 % in both years, so the tax line is consistent; cash tax rises because the interest deduction falls as the loan amortises ✅
- C. the model has omitted deferred tax
- D. the loss carry-forward has been applied incorrectly

*Rationale:* `516,000/2,580,000 = 766,771/3,833,856 = 20.0 %`; the rise is the amortising interest deduction, the mechanism behind the year-12 minimum of 6.4.1 (6.2.3, KA 6.4.1). A mistakes a correct behaviour for an error; C and D name plausible defects for which there is no evidence here.


**6.2-D** `[6.2.3 · Analysis]` Under 15 % declining-balance allowances Kestrel pays no cash tax for five years, lifting year-one `DSCR` from 1.2743 to 1.3773 and debt capacity at 1.30× from 41,171,123 to 44,498,864. The sound treatment is:

- A. size the debt at 44,498,864, since the allowance exists
- B. ignore the allowance, since lenders will not credit it
- C. model both, state which case governs which decision, and put the 3,327,741 of capacity difference — and the legislative risk in it — in front of the credit committee ✅
- D. average the two capacities

*Rationale:* The difference is an assumption about a jurisdiction over a twelve-year loan, so it is disclosed and owned rather than banked or suppressed (6.2.3). A embeds a legislative forecast in a financing; B discards a real economic benefit; D is arithmetic without meaning.


**6.3-A** `[6.3.3 · Analysis]` Kestrel's unlevered post-tax `IRR` is 8.54 %, its bank-case equity `IRR` is 9.83 %, and Domain 4's appraisal reported 12.19 %. The correct reading is:

- A. the appraisal was wrong by 3.65 points
- B. the asset earns 8.54 %, the equity earns 9.83 % because of leverage, and 12.19 % is a pre-tax fifteen-year figure describing neither party's position ✅
- C. the equity return should exceed the project return by the debt margin
- D. the three should be equal once tax is removed

*Rationale:* Each measure is correct on its own basis and horizon; conflating them is the defect (6.3.3, 6.1.3). C invents a relationship — the gap depends on gearing, tenor and the shape of distributions; D is false, since horizon alone separates them.


**6.3-B** `[6.3.2 · Application]` Kestrel's year-twelve distribution is 3,431,895 against year eleven's 980,580, on `CFADS` that falls between the two years. The explanation is:

- A. a cash sweep
- B. the DSRA of 2,504,818 releasing at final repayment ✅
- C. an error, since `CFADS` fell
- D. the final principal instalment being smaller

*Rationale:* The reserve is released when the debt it secures is repaid, a source in the waterfall (6.3.2). C mistakes a modelled contractual event for an inconsistency; D is the opposite of the truth — year twelve's principal, 4,726,071, is the largest of the twelve.


**6.3-C** `[6.3.1 · Analysis]` A construction model charges interest on the average debt balance and the workbook's iterative calculation is switched off. The consequence is:

- A. interest is understated by a known amount
- B. the model returns a stale or unconverged figure whose value depends on calculation order, so the result is not reproducible ✅
- C. the model will not open
- D. capitalised interest becomes zero

*Rationale:* Average-balance interest is genuinely circular; without a resolution the answer is whatever the last pass left behind (6.3.1). A describes the *opening*-balance convention, which is deliberate and quantified at 312,957 on Kestrel; C and D are not how circular references behave.


**6.3-D** `[6.3.3 · Application]` Kestrel returns 90,507,502 of distributions on 18,000,000 of equity over 25 years — a 5.028× multiple — at an equity `IRR` of 9.83 %. A board shown only the multiple has been shown:

- A. a complete picture, since the multiple includes every distribution
- B. a long horizon dressed as a return, because the multiple is blind to when the cash arrives ✅
- C. an understatement, since multiples ignore reinvestment
- D. the same information as the `IRR`

*Rationale:* A multiple has no time dimension; 5.028× over 25 years is 9.83 % a year (6.3.3, and Domain 4's insistence that rates and ratios explain rather than decide). C reverses the bias; D is false — the same multiple over ten years would be a materially higher rate.


**6.4-A** `[6.4.1 · Analysis]` A modeller taxes `EBIT` of 5,100,000 at 20 % instead of taxable profit of 2,580,000. The balance sheet still balances. The check that catches it is:

- A. the balance-sheet check, once the period is recalculated
- B. sources equal uses
- C. the implied effective tax rate — 1,020,000/2,580,000 = 39.53 % against a statutory 20 % ✅
- D. closing debt nil at maturity

*Rationale:* A consistently propagated error balances, so invariant 1 is necessary and not sufficient (6.4.1). B and D test construction funding and the debt schedule, neither of which the error touches.


**6.4-B** `[6.4.1 · Application]` Kestrel's bank case shows `DSCR` of 1.2743 in year one, 1.1851 in year twelve and an average of 1.2340 against a 1.20× covenant. The reportable position is:

- A. compliant, since the average exceeds the covenant
- B. compliant, since year one exceeds the covenant
- C. a breach in year twelve; the minimum and its year must be reported, because covenants are tested in periods ✅
- D. indeterminate without the sponsor case

*Rationale:* Domain 10's rule, earned here: an average conceals the period that breaches (6.4.1b, KA 10.A.3). D inverts the logic — the sponsor case, in which coverage rises to 1.5940, is the case that hides the problem.


**6.4-C** `[6.4.2 · Analysis]` Kestrel's unlevered `NPV` has an elasticity of 0.00 to the interest rate, while a 10 % rate rise takes the minimum `DSCR` to 1.1485 — below the 1.15× lock-up. The correct inference is:

- A. interest-rate risk is immaterial
- B. the model has an error, since a higher rate must reduce value
- C. sensitivity must be run on the outputs the decision turns on: financing does not change the asset's cash, but it changes coverage, which is what the covenant tests ✅
- D. the discount rate and the interest rate should be equal

*Rationale:* Unlevered cash is financing-independent by construction (6.4.2). A reads only the `NPV` column, which is exactly the mistake; B misunderstands what unlevered means; D confuses the opportunity cost of capital with the cost of debt.


**6.4-D** `[6.4.2 · Application]` Domain 10 reported covenant headroom of 372,438 of annual `CFADS`, 5.83 % of base case. Expressed as revenue, the headroom is:

- A. 5.83 %
- B. 4.14 % ✅
- C. 8.22 %
- D. 15.27 %

*Rationale:* `CFADS` amplifies revenue by 1.4098, so the covenant breaks at revenue of 11,503,416, a 4.14 % fall (6.4.2). A carries the `CFADS` percentage across as if the two were interchangeable — overstating the operations team's room by 41 %; C multiplies rather than divides by the elasticity; D is the fall at which payment itself fails.


**6.4-E** `[6.4.3 · Evaluation]` A model audit costs 180,000 and two weeks of delay worth 248,267; `p` = 0.35, `d` = 0.85, `C` = 2,691,071, pre-close correction 184,133. Moving the audit early, so it adds no delay and correction costs only 60,000, changes its net value and breakeven error rate to:

- A. 317,547 and 20.10 %
- B. 602,744 and 8.05 % ✅
- C. 941,875 and nil
- D. 497,547 and 20.10 %

*Rationale:* `941,875 − [180,000 + 0.35 × (0.85 × 60,000 + 0.15 × 2,691,071)] = 602,744`, and `180,000/(0.85 × (2,691,071 − 60,000)) = 8.05 %` (6.4.3). A is the late-audit answer; C is the expected cost of no control at all; D pairs the late audit's breakeven fee with its breakeven error rate.


## Domain 7

**7.1-A** `[7.1.1 · Analysis]` Two structures for the same plant have identical expected `CFADS` of 6,384,000 and identical expected `DSCR` of 1.2743. Structure A pays that amount with certainty; Structure B's outcomes are 4,728,000 / 6,384,000 / 8,040,000 with probabilities 0.25 / 0.50 / 0.25. The soundest conclusion is:

- A. the structures are equivalent, since expected coverage is the same
- B. Structure B supports materially less debt, because a lender underwrites a stressed or low case and requires higher coverage for dispersion ✅
- C. Structure B supports more debt, because its upside outcome is higher
- D. the difference can be resolved by raising the target `DSCR` alone

*Rationale:* Sizing on the low case at 1.30 × gives 30,491,396 against 41,171,123 — a 10,679,727 gap (7.1.1). A treats a mean as a coverage ratio, which coverage tests never are; C mistakes an upside the lender has no claim on for capacity; D captures only part of the effect, since the case moves as well as the ratio.


**7.1-B** `[7.1.3 · Application]` A capacity charge of 12,000,000 carries a 95 % availability guarantee and a deduction equal to the charge multiplied by the shortfall and by 1.5. At 91 % availability the deduction is:

- A. USD 480,000
- B. USD 720,000 ✅
- C. USD 1,620,000
- D. USD 600,000

*Rationale:* `12,000,000 × 1.5 × 0.04 = 720,000`. A omits the 1.5 × multiplier; C applies the multiplier to total unavailability (9 points, `1 − 0.91`) instead of the shortfall against the 95 % guarantee; D applies a 5 % reduction, reading the guarantee as the deduction base.


**7.1-C** `[7.1.2 · Analysis]` A project recovers all of its fixed cash costs and its entire debt service through a per-unit volume charge with no floor. The structural defect is:

- A. the unit rate will be too low to be commercial
- B. fixed obligations are funded by a variable receipt, so any volume shortfall falls entirely on coverage ✅
- C. the tariff cannot be indexed
- D. it prevents the use of an availability standard

*Rationale:* Matching discipline requires fixed-cost and debt-service recovery in a charge that does not flex with volume (7.1.2). A is a pricing observation, not a structural one; C and D are simply untrue of volume tariffs.


**7.1-D** `[7.1.3 · Analysis]` Kestrel breaches its 1.20 × covenant at 92.086 % availability. Reducing the negotiated deduction multiplier from 1.5 to 1.0 would move that breakpoint to 90.502 %. The correct reading is:

- A. the multiplier is a technical schedule with no financial consequence
- B. the multiplier costs 1.584 percentage points of operating tolerance and is therefore a first-order commercial term ✅
- C. the multiplier only matters if availability falls below 90 %
- D. the breakpoint depends on the tariff level, not the multiplier

*Rationale:* The multiplier scales the slope of the `CFADS` line against availability, so it scales headroom directly (7.1.3). C inverts the logic — the multiplier is what brings the breakpoint *closer*; D ignores that the deduction is computed on the charge and the multiplier together.


**7.2-A** `[7.2.2 · Application]` A banded tariff pays 0.55 per m³ on the first 18,000,000 m³, 0.35 on the next 6,000,000 and 0.10 above 24,000,000. At despatch of 19,200,000 m³ revenue is:

- A. USD 9,600,000
- B. USD 10,320,000 ✅
- C. USD 10,560,000
- D. USD 12,000,000

*Rationale:* `18,000,000 × 0.55 + 1,200,000 × 0.35 = 9,900,000 + 420,000 = 10,320,000`. A applies the flat 0.50 tariff; C applies 0.55 to the whole volume; D is the base-case revenue, which the band does not guarantee.


**7.2-B** `[7.2.3 · Analysis]` An SPV has `ARR` of 24,000,000, net revenue retention of 93 % and wholly fixed cash costs of 13,200,000. In the first year of run-off, `EBITDA` falls by approximately:

- A. 7 %
- B. 15.6 % ✅
- C. 18.7 %
- D. it does not fall, because retention is above 90 %

*Rationale:* `EBITDA` falls from 10,800,000 to 9,120,000, a fall of **15.56 %** — the 7 % revenue decline amplified by the fixed cost base. A mistakes the revenue decline for the earnings decline; C is the **`CFADS`** decline (9,000,000 to 7,320,000, −18.67 %), the right amplification applied to the wrong measure; D misreads a retention rate as a growth rate.


**7.2-C** `[7.2.3 · Analysis]` A service-revenue SPV has a `WARCT` of 3.30 years and seeks a seven-year loan. The lender's most likely structural response is:

- A. to accept the tenor, since `ARR` covers debt service in year one
- B. to size on the contracted run-off case and require a shorter tenor, a re-contracting reserve, a cash sweep or contract extensions before close ✅
- C. to size on the sponsor case including new sales, discounted by 10 %
- D. to require an availability guarantee

*Rationale:* Less than half the loan's life is contracted, so the loan is being asked to bridge uncontracted years (7.2.3). A relies on year one alone; C lends against unsigned revenue; D applies a mechanism from a different revenue architecture.


**7.2-D** `[7.2.2 · Analysis]` A volume band raises the low-case `DSCR` from 0.9438 to 1.0516 and lowers the high-case `DSCR` from 1.6049 to 1.3175, reducing expected revenue by 300,000 a year. The correct characterisation is:

- A. the band destroys value, since expected revenue falls
- B. the band transfers value from equity's upside to debt capacity, and the trade is computable: 2,515,153 of present value surrendered for 3,482,520 of capacity ✅
- C. the band has no effect on debt capacity, since expected `CFADS` still exceeds debt service
- D. the band removes the covenant risk

*Rationale:* Lenders price the low tail and equity owns the high one, so compressing both is a transfer, not a loss (7.2.2). A ignores the capacity gain; C treats expected cash as the sizing basis; D is false — the low case at 1.0516 still breaches the 1.20 × covenant.


**7.3-A** `[7.3.1 · Application]` A tariff indexes 80 % of its value at 2.5 % a year; the remaining 20 % is fixed. Over 24 years the tariff's **effective** compound escalation rate is closest to:

- A. 2.500 %
- B. 2.101 % ✅
- C. 2.000 %
- D. 3.125 %

*Rationale:* Year-25 revenue is `12,000,000 × (0.80 × 1.025²⁴ + 0.20) = 19,763,769`, giving `(19,763,769/12,000,000)^(1/24) − 1 = 2.101 %`. A is the headline index applied as though the whole tariff escalated; C is the naive `0.80 × 2.5 %`, which is an approximation valid only over one period; D divides the index by the indexed share instead of multiplying.


**7.3-B** `[7.3.2 · Application]` Price 0.50 per m³, variable cost 0.0375 per m³, volume 24,000,000 m³, `EBITDA` 7,500,000. The degree of operating leverage is:

- A. 1.4800 ✅
- B. 1.6000
- C. 1.2970
- D. 0.6757

*Rationale:* `contribution 11,100,000 ÷ EBITDA 7,500,000 = 1.4800`. B uses revenue rather than contribution (`12,000,000/7,500,000`); C is the `CFADS` elasticity to volume, a different measure computed after tax and working capital; D inverts the ratio.


**7.3-C** `[7.3.2 · Analysis]` Two projects each report a base `DSCR` of 1.30 × against a 1.20 × covenant. Project M's `CFADS` is 80 % of revenue; Project N's is 40 %. Their demand tolerances are:

- A. identical, since the coverage ratio and covenant are identical
- B. M 6.15 %, N 3.08 % — cost structure halves the tolerance at identical coverage ✅
- C. M 3.08 %, N 6.15 %
- D. indeterminate without the debt amount

*Rationale:* Both need a 7.6923 % `CFADS` fall; dividing by elasticities of 1.25 and 2.50 gives 6.15 % and 3.08 % (7.3.2). A is the error the table exists to correct; C reverses the relationship — a thinner `CFADS` margin means higher elasticity and less tolerance; D is wrong because the tolerance is a ratio property, independent of scale.


**7.3-D** `[7.3.3 · Analysis]` Kestrel's `CFADS` tolerance to its covenant is 5.83 %. The figure that should be given to the operations team, which manages despatch, is:

- A. 5.83 %, the `CFADS` tolerance
- B. 4.50 %, the volume tolerance, being 5.83 % divided by the `CFADS` elasticity to volume of 1.2970 ✅
- C. 4.14 %, the revenue tolerance
- D. 7.52 %, the lock-up tolerance

*Rationale:* Each team needs the tolerance in the driver it controls; quoting the `CFADS` figure to a volume-managing team overstates their room by about 30 % (7.3.3). C is the correct figure for the commercial team, which sets tariff, not for despatch; D is the correct volume figure for the *lock-up*, a different and later threshold.


**7.4-A** `[7.4.1 · Application]` Annual `PD` 0.60 %, exposure 53,522,460, `LGD` 45 %, over a twelve-year loan life. Expected loss is closest to:

- A. USD 144,511
- B. USD 1,678,031 ✅
- C. USD 3,728,957
- D. USD 24,085,107

*Rationale:* Twelve-year cumulative `PD` is `1 − 0.994¹² = 6.9671 %`, so `0.069671 × 53,522,460 × 0.45 = 1,678,031`. A applies the single-year `PD`, understating the twelve-year exposure by a factor of eleven; C omits `LGD`, treating default as total loss; D omits `PD` entirely.


**7.4-B** `[7.4.2 · Analysis]` A single offtaker taking 100 % of output is replaced by four independent offtakers taking 25 % each, with identical `PD` and `LGD`. Expected loss:

- A. falls by 75 %
- B. is unchanged, while the probability of losing at least half the revenue falls from 6.9671 % to 2.6489 % ✅
- C. falls to one quarter of its previous value
- D. rises, because there are four counterparties who might default

*Rationale:* Expected loss is linear in exposure, so splitting it changes nothing; the loss *distribution* changes profoundly (7.4.2). A and C confuse per-counterparty exposure with total expected loss; D confuses the probability of *some* default — which does rise, to 25.09 % — with expected loss.


**7.4-C** `[7.4.2 · Analysis]` A sovereign guarantee reduces expected loss by 1,106,304 at a fee with a present value of 805,901, and would allow the lenders to size at 1.20 × rather than 1.30 ×, raising debt capacity from 41,171,123 to 44,602,050. The correct basis for the decision is:

- A. reject it: the expected-loss saving of 300,403 net is immaterial
- B. accept it: it releases 3,430,927 of debt capacity, or 2,625,026 net of the fee's present value, which is what credit enhancement is bought for ✅
- C. accept it because guarantees always improve bankability
- D. the two effects cannot be compared

*Rationale:* Enhancement is priced against the coverage and case it unlocks, not the provision it reduces (7.4.2). A applies the wrong metric to the decision, though it is the right metric for the provision; C is an unsupported generality; D is false — both effects are in present-value terms.


**7.4-D** `[7.4.3 · Analysis]` A stress matrix shows that only three of sixteen tariff and despatch combinations clear the 1.20 × covenant, and that a 5 % tariff cut requires despatch 0.99 % above forecast to remain compliant. The most valuable observation for management is:

- A. the matrix should be widened until more cells comply
- B. the project has almost no joint tolerance, and it does not hold the lever — despatch is the offtaker's choice — that would recover a tariff cut ✅
- C. the year-one figures understate the risk, so the matrix should be discarded
- D. the covenant should be renegotiated to 1.00 ×

*Rationale:* A reverse stress test must identify both the failure point and whether the project controls the driver that would avoid it (7.4.3). A is presentational dishonesty; C is half right — year-one figures are optimistic, so the matrix should be *extended* to the minimum year, not discarded; D mistakes a covenant for the problem.


## Domain 8

**8.1-A** `[8.1.2 · Application]` A 48,000,000 base estimate carries a funded contingency of 3,645,403. Which statement is defensible?

- A. 7.59 % is a normal contingency, so the provision is adequate
- B. 7.59 % is a Stage E (control-estimate) provision, adequate only because a fixed-price wrap moved the estimate to that class ✅
- C. 7.59 % is adequate for any estimate class, since contingency is a policy percentage
- D. the provision is inadequate at every class, because contingency should be 10 %

*Rationale:* 3,645,403/48,000,000 = 7.59 %, which sits inside the −5/+8 control band and covers 94.93 % of it; the same money covers 25.32 % of a Stage C +30 % band. A appeals to a norm that does not exist; C is the error the whole KA exists to remove; D substitutes a different unreasoned percentage for the first one.


**8.1-B** `[8.1.2 · Analysis]` A project is procured as six separate packages against a Stage C feasibility estimate of 48,000,000 carrying a stated accuracy range of −15 % / +30 %, with the owner managing interfaces. The contingency implied by the estimate's own upper bound is:

- A. 3,840,000
- B. 8,640,000
- C. 14,400,000 ✅
- D. 24,000,000

*Rationale:* Stage C's +30 % upper bound on 48,000,000 is 14,400,000. A is the Stage E provision (the answer only a full wrap earns); B is Stage D's +18 %; D is Stage A's +50 %.


**8.1-C** `[8.1.3 · Application]` Membranes costing 3,200,000 at base-date prices are replaced in year 7; escalation is 3.6 % per annum. `CFADS` is 6,384,000 and debt service 5,009,635.23. With no maintenance reserve, the year-seven `DSCR` is closest to:

- A. 1.2743
- B. 0.4561 ✅
- C. 0.6356
- D. 1.1457

*Rationale:* Nominal cost `3,200,000 × 1.036⁷ = 4,098,909`; `(6,384,000 − 4,098,909)/5,009,635.23 = 0.4561`. A is the ratio with a funded reserve; C forgets to escalate (using 3,200,000, giving 3,184,000/5,009,635.23 = 0.6356); D deducts only the level annual charge of 644,606 rather than the actual spend (`5,739,394/5,009,635.23 = 1.1457`).


**8.1-D** `[8.1.1 · Analysis]` An operating-cost forecast understates annual cost by 500,000. Against Kestrel's 1.20× covenant with annual headroom of 372,438, the consequence is:

- A. a funding shortfall in the sources-and-uses statement
- B. no consequence, since operating cost is not a funded use
- C. a covenant breach — the error is 1.34 times the entire annual headroom, and it recurs every year ✅
- D. a one-off reduction in the contingency line

*Rationale:* Operating cost is a deduction inside `CFADS`, so a 500,000 understatement removes 500,000 of `CFADS` annually against 372,438 of headroom (500,000/372,438 = 1.34). A and D put an operating error in a capital line; B confuses "not funded" with "no effect".


**8.2-A** `[8.2.2 · Application]` 48,000,000 of spend over eight quarters, 70 % debt-funded at 1.5 % per quarter on opening balances. The profile's cumulative-before-period fractions sum to 3.9700. Capitalised interest is:

- A. USD 1,607,760
- B. USD 2,000,880 ✅
- C. USD 2,858,400
- D. USD 504,000

*Rationale:* `0.015 × 0.70 × 48,000,000 × 3.9700 = 2,000,880`. A is the S-curve's area of 3.1900; C omits the gearing, applying interest to full spend (`0.015 × 48,000,000 × 3.9700 = 2,858,400`); D is the per-unit-of-area coefficient mistaken for the answer.


**8.2-B** `[8.2.3 · Analysis]` A model escalates a 48,000,000 two-year construction estimate by multiplying it by `1.036²`, giving 51,518,208, where the profile-correct figure for the same S-curve is 50,093,393. The error is:

- A. 1,424,815 of understatement
- B. 1,424,815 of overstatement, because escalating a total prices every dollar as though spent on the final day ✅
- C. immaterial, since the rate is correct
- D. offset by the interest calculation

*Rationale:* 51,518,208 − 50,093,393 = 1,424,815 too much; escalation must be applied period-by-period to the profile. C mistakes a correct rate for a correct method; D is wrong in direction — a higher spend also raises draws and therefore IDC.


**8.2-C** `[8.2.3 · Analysis]` At 70 % gearing and a 6.0 % debt rate, the construction escalation rate above which front-loading spend becomes cheaper than back-loading it is closest to:

- A. 6.0 %
- B. 4.2 % ✅
- C. 3.6 %
- D. 1.8 %

*Rationale:* Deferral costs escalation on 100 % of spend and saves interest on the geared 70 %, so neutrality is at `e ≈ g × r = 4.20 %` — computed as **4.1352 %** between Kestrel's front- and back-loaded profiles on the quarterly convention (8.2.3 quotes 4.1659 % for the same effect measured between the S-curve and the back-loaded profile; every pairwise breakeven sits just below `g × r`). A ignores gearing; C is the assumed escalation rate, not the breakeven; D halves the rate for no stated reason.


**8.2-D** `[8.2.2 · Analysis]` A reviewer wants one calculation that validates a construction model's entire capitalised-interest line. The best choice is:

- A. confirm the total spend equals the contract price
- B. recompute IDC from the area rule and require agreement to the dollar ✅
- C. confirm the closing debt balance equals the facility amount
- D. compare the IDC percentage against other projects

*Rationale:* The area rule reproduces IDC from the profile, rate and gearing, so agreement validates all four inputs and disagreement localises the defect. A and C are necessary but pass with a wrong interest convention; D is benchmarking, not verification.


**8.3-A** `[8.3.2 · Application]` A retained register has mean exposure 2,690,000 and standard deviation 1,848,973. The P80 contingency is closest to:

- A. USD 2,690,000
- B. USD 4,246,095 ✅
- C. USD 8,500,000
- D. USD 5,731,375

*Rationale:* `2,690,000 + 0.8416 × 1,848,973 = 4,246,095`. A is the mean, which by construction is exceeded about half the time; C is the worst-case sum of threats; D is the P95 (z = 1.6449), a different policy choice.


**8.3-B** `[8.3.3 · Analysis]` A 10 %-of-base contingency of 4,800,000 is a P87.3 provision against the risk register and a P70.4 provision against the estimate's accuracy range. The correct conclusion is:

- A. the rule is validated, since both figures exceed P50
- B. the rule states no confidence level, so the provision cannot be compared with a covenant, a support commitment or another project ✅
- C. the register must be wrong, since the two bases disagree
- D. the average of the two, P78.9, is the provision's true confidence

*Rationale:* The two percentiles measure different uncertainties (discrete events versus systemic estimate error), so a single percentage of base cannot express a confidence at all — which is the defect. A mistakes "above the median" for "sized"; C misreads a difference in basis as an error; D averages two probabilities that are not on the same scale.


**8.3-C** `[8.3.3 · Application]` The largest register item (p 0.40, impact 2,400,000) retires with no impact. Register P80 falls from 4,246,095 to 2,930,955 while the 10 % rule stays at 4,800,000. If the resulting excess is drawn and funded 70/30, the effect on annual covenant headroom of 372,438 is a fall of:

- A. nil — contingency is not carried at a cost
- B. 187,265, or 50.3 %, because 1,308,332 of extra senior debt raises the instalment by 156,054 ✅
- C. 11,214, the commitment fee on the excess
- D. 1,869,045, the excess itself

*Rationale:* `4,800,000 − 2,930,955 = 1,869,045`; `× 0.70 = 1,308,332`; `/8.383844 = 156,054` of extra instalment; the 1.20× trigger rises to 6,198,827 and headroom falls to 185,173. A ignores that drawn contingency is debt serviced for the loan life; C is the cost while the excess is *undrawn*, not drawn; D confuses the provision with its coverage effect.


**8.3-D** `[8.3.1 · Analysis]` Why do lenders treat 2,085,972 of contingent equity support as worth less than 2,085,972 of funded contingency?

- A. it is a smaller amount in present-value terms
- B. it is only drawable on certification
- C. it depends on a sponsor's willingness and ability to pay when called, which is a credit exposure rather than cash in the structure ✅
- D. it cannot be documented

*Rationale:* Funded contingency is money the facility will lend against certification; contingent support is a commitment whose value is the obligor's credit (8.3.1, and Domain 5 KA 5.2.3 on several versus joint-and-several liability). B describes funded contingency; A confuses timing with credit; D is false.


**8.4-A** `[8.4.1 · Application]` `BAC` 4,000,000, contingency 300,000, `AC` 2,120,000, `EAC` on `BAC/CPI` 4,416,667. The funds sufficiency position is:

- A. a surplus of 100,000
- B. a shortfall of 116,667, requiring an injection before the next drawdown ✅
- C. a shortfall of 416,667
- D. in balance, since the `EAC` is within the total funding of 4,300,000

*Rationale:* Available `= 4,300,000 − 2,120,000 = 2,180,000`; `CTC = 4,416,667 − 2,120,000 = 2,296,667`; shortfall 116,667. A is the position on the budgeted-rate forecast; C is the overrun against `BAC`, not against funding; D is false — 4,416,667 exceeds 4,300,000.


**8.4-B** `[8.4.1 · Analysis]` Remaining contingency is 120,000 and the P80 of the remaining register is 286,185. A cost report shows the forecast inside total funding. The correct reading is:

- A. no action is required, since the forecast is inside funding
- B. the contingency-adequacy test fails by 166,185 and does so independently of any `EAC` forecast ✅
- C. the register must be revised downwards to match the contingency
- D. the shortfall is 120,000

*Rationale:* Adequacy compares remaining provision with remaining exposure and needs only the register and the draw history; 286,185 − 120,000 = 166,185. A misses the second test entirely; C is the corruption the test exists to prevent; D subtracts in the wrong direction.


**8.4-C** `[8.4.2 · Application]` Debt drawn 31,990,655 at 6.0 %; remaining owner-retained scope 922,906 escalating at 3.6 % per annum; annual `CFADS` 6,384,000. One month of slip costs closest to:

- A. USD 532,000
- B. USD 694,677 ✅
- C. USD 162,677
- D. USD 730,998

*Rationale:* `922,906 × (1.036^(1/12) − 1) = 2,724`; `31,990,655 × 0.06/12 = 159,953`; `6,384,000/12 = 532,000`; total 694,677. A is the deferred revenue alone; C omits the revenue row; D is the unwrapped variant, where escalation applies to the whole remaining scope (39,045).


**8.4-D** `[8.4.2 · Analysis]` Flat delay damages of 20,000 per day recover 86.37 % of the cost of a month's slip at quarter six, over-recover at financial close and recover 80.86 % at COD. The best negotiating response is:

- A. accept the rate — it recovers most of the cost
- B. seek a higher flat rate calibrated to the COD figure
- C. seek a stepped rate rising through the programme, since the exposure rises monotonically as the drawn balance builds ✅
- D. remove damages and rely on contingency

*Rationale:* A flat rate cannot fit a cost that moves 34.3 % across the programme; a stepped rate matches exposure and is cheaper for a contractor to accept than a flat rate set at the maximum. A ignores the tail; B over-recovers early and is resisted for that reason; D abandons the transfer altogether.


## Domain 9

**9.1-A** `[9.1.3 · Application]` With `β_a` = 0.60, `D/E` = 42,000,000 / 18,000,000, `T` = 20 %, `r_f` = 4.10 %, `ERP` = 6.00 %, `CRP` = 0.50 % and `SP` = 0.50 %, the cost of equity is:

- A. 15.42 % ✅
- B. 8.70 %
- C. 17.10 %
- D. 14.42 %

*Rationale:* `β_e = 0.60 × (1 + 0.80 × 2.333333) = 1.72`; `k_e = 4.10 + 1.72 × 6.00 + 0.50 + 0.50 = 15.42 %`. B uses the **unlevered** beta throughout (`4.10 + 0.60 × 6.00 + 1.00 = 8.70`) — the error of not re-levering at all. C re-levers **without** the tax adjustment (`β_e = 0.60 × 3.333333 = 2.00` → `4.10 + 12.00 + 1.00 = 17.10`). D omits the country and single-asset premiums (`4.10 + 10.32 = 14.42`) — building the beta term correctly and then forgetting the rest of the build-up.


**9.1-B** `[9.1.4 · Analysis]` Moving Kestrel from 70 % to 80 % gearing changes `WACC` from 7.9860 % to 7.8840 % and equity `IRR` from 12.5311 % to 13.6146 %, while `DSCR` falls from 1.2743 to 1.1151 against a 1.20× covenant. The correct conclusion is:

- A. gear to 80 % — the `WACC` is lower and `WACC` minimisation is the objective
- B. the 80 % structure is not financeable: the 1.20× covenant fails on the base case, and the 10.2 basis points of `WACC` saved are irrelevant beside a breach ✅
- C. gear to 80 % and negotiate a 1.10× covenant, since the equity gain is large
- D. the two structures are equivalent because total capital is unchanged

*Rationale:* At 80 % the covenant requires `CFADS` of `5,725,297.41 × 1.20 = 6,870,357` against 6,384,000 available — a base-case breach, so the structure does not exist to be optimised (9.1.4). A applies the corporate-finance objective to a constrained problem; C proposes a covenant no lender sizing at 1.30× would grant and ignores that the lock-up also fails; D confuses the funding total with its risk allocation.


**9.1-C** `[9.1.1 · Analysis]` A sponsor's equity `IRR` on Kestrel is 10.6696 % without an equity bridge. An EBL is offered at exactly 10.6696 %, interest capitalised over the two-year build. The effect on the reported equity `IRR` is:

- A. it rises, because the equity outflow is deferred
- B. it is unchanged: a bridge priced exactly at the equity `IRR` it defers is value-neutral ✅
- C. it falls, because the sponsor pays interest it would otherwise not have paid
- D. it rises by 94.61 basis points

*Rationale:* Deferring an outflow at exactly the rate at which that outflow discounts leaves the `IRR` identical — the indifference identity of 9.1.1, which the bisection confirms to the tested precision. A states the general direction of a *cheap* bridge but ignores its pricing; C confuses a cash cost with a rate effect, since the capitalised interest is exactly compensated by the deferral; D quotes the uplift computed at a **5.0 %** bridge rate and applies it as though the bridge rate were irrelevant.


**9.1-D** `[9.1.2 · Analysis]` A 6,000,000 shareholder loan at 12 % is drafted so that its interest ranks **above** `CFADS` rather than being paid from distributions. The consequence is:

- A. an additional tax shield of 144,000 a year and no other effect
- B. `DSCR` falls from 1.2743 to 1.1142, breaching both the 1.20× covenant and the 1.15× lock-up ✅
- C. the senior lenders are unaffected because the loan is subordinated in name
- D. the loan becomes senior debt for all purposes

*Rationale:* `6,384,000 / (5,009,635.23 + 720,000) = 1.1142` (9.1.2). A is the shield without the ranking consequence; C mistakes a label for a waterfall position; D overstates — the ranking clause changes the coverage calculation without converting the instrument.


**9.2-A** `[9.2.3 · Application]` A 150,000,000 project is funded with senior 80,000,000 at 5.2 %, mezzanine 20,000,000 at 10.8 % (both deductible) and equity 50,000,000 at 14.5 %; tax 30 %. The `WACC` is:

- A. 7.7827 % ✅
- B. 9.0467 %
- C. 8.5667 %
- D. 6.3327 %

*Rationale:* `(80 × 3.64 + 20 × 7.56 + 50 × 14.5)/150 = 7.7827 %`. B omits the tax shield on both debt tranches; C takes a simple average of the three costs, ignoring the weights; D tax-adjusts the **equity** cost as well, which no jurisdiction permits.


**9.2-B** `[9.2.3 · Analysis]` Adding 9,000,000 of 11.50 % mezzanine in place of senior debt raises Kestrel's senior-only `DSCR` from 1.2743 to 1.5115 and its `WACC` from 7.9860 % to 8.4510 %. The correct reading is:

- A. the structure is superior because coverage improved
- B. the structure is inferior because `WACC` rose
- C. mezzanine bought 23.72 points of senior coverage for 46.50 basis points of `WACC` and 44.87 basis points of equity `IRR`; whether that is worth paying depends on whether the senior tranche is otherwise unavailable ✅
- D. `WACC` and `DSCR` cannot both be affected by one tranche

*Rationale:* Both faces move and the decision is the exchange rate between them (9.2.3). A and B each optimise one number in isolation — the specific error this domain exists to prevent; D is simply false, since the mezzanine changes both the weighted cost and the debt service.


**9.2-C** `[9.2.4 · Application]` A 42,000,000 bond at a 6.0 % coupon is drawn in full at close and spent evenly over two years; idle proceeds earn 3.0 %. The negative arbitrage is:

- A. USD 2,520,000
- B. USD 1,260,000 ✅
- C. USD 630,000
- D. nil, because the proceeds are invested

*Rationale:* Average idle balance 21,000,000 × 3.0 % spread × 2 years = 1,260,000. A applies the spread to the **full** 42,000,000 for two years, ignoring the drawdown profile; C covers one year only; D confuses earning a return with earning enough of one.


**9.2-D** `[9.2.2 · Analysis]` Why do senior lenders usually prohibit mezzanine debt at SPV level while tolerating it at HoldCo level?

- A. HoldCo debt is cheaper
- B. SPV mezzanine consumes the `CFADS` their covenant measures, while HoldCo mezzanine is serviced from distributions that already rank behind every senior test ✅
- C. HoldCo debt carries no security
- D. accounting standards require it

*Rationale:* The placement determines whether the junior service appears in the coverage calculation (9.2.2). A is not generally true — HoldCo debt is usually dearer, being further from the cash; C is incidental; D confuses drafting practice with reporting requirements.


**9.3-A** `[9.3.2 · Application]` A 40,000,000 tranche carries 4.10 % over 12 years with a 5.5 % exposure premium capitalised into the loan. The all-in cost on the 40,000,000 of proceeds is closest to:

- A. 4.10 %
- B. 5.0378 % ✅
- C. 4.5583 %
- D. 9.60 %

*Rationale:* Loan `42,200,000`; `AF(0.041, 12) = 9.330854`; instalment `4,522,630.17`; solve `40,000,000 = 4,522,630.17 × AF(r, 12)` → **5.0378 %**. A ignores the premium entirely; C spreads the premium straight-line over the tenor and adds it to the rate (`4.10 + 5.5/12`), which understates because it ignores that the premium is also financed; D adds the premium to the rate as though it were annual.


**9.3-B** `[9.3.3 · Analysis]` A DFI tranche at 5.25 % over 18 years has an all-in economic cost of 7.2465 % against a commercial market at 6.00 % over 12 years. The soundest conclusion is:

- A. reject the DFI tranche — it is more expensive
- B. accept it — the headline rate is lower
- C. the cost comparison favours the commercial market by about 125 basis points, while the six extra years of tenor raise debt capacity at a 1.30× target from 41,171,123 to 56,299,948; the decision turns on whether coverage or cost is the binding constraint ✅
- D. the two are equivalent because tenor and rate offset

*Rationale:* Cost and capacity are different effects with different causes (9.3.3). A optimises cost while ignoring the constraint that actually binds; B is the headline-rate error the worked example exists to destroy; D asserts an offset without computing either side.


**9.3-C** `[9.3.1 · Application]` A conventional tranche of 21,000,000 at 6.00 % over 12 years carries a 1.20 % fee; an ijara tranche of the same size and tenor carries a 6.15 % profit rate and a 0.60 % fee. The difference in all-in cost is closest to:

- A. 15 basis points in favour of the conventional tranche
- B. 4 basis points in favour of the conventional tranche ✅
- C. 60 basis points in favour of the ijara tranche
- D. nil, since the structures are economically identical

*Rationale:* All-in 6.2209 % against 6.2604 % — **3.95 basis points** (9.3.1). A is the headline difference before fees; C confuses the fee saving with a rate advantage; D asserts an equivalence the arithmetic disproves, small though the gap is.


**9.3-D** `[9.3.4 · Recall]` The primary purpose of a common terms agreement in a multi-source financing is to:

- A. reduce legal fees
- B. ensure every tranche tests coverage on the same definitions and shares one covenant and default architecture ✅
- C. give the DFI a veto
- D. permit tranches to be drawn in any order

*Rationale:* One set of shared definitions and covenants prevents the same project being measured differently by different lenders (9.3.4). A is a by-product; C describes an intercreditor outcome to be negotiated, not the purpose; D is a drawdown question the agreement constrains rather than liberates.


**9.4-A** `[9.4.2 · Analysis]` A 6,000,000 grant applied to reduce **equity** leaves `DSCR` at 1.2743 and lifts equity `IRR` from 12.5311 % to 16.8231 %; applied to reduce **debt** it lifts `DSCR` to 1.4867 and equity `IRR` to 14.9940 %. If the grantor's stated objective is to make an otherwise unfinanceable project financeable, it should:

- A. apply the grant to reduce equity, maximising private-sector return
- B. apply the grant to reduce debt, because coverage is the binding constraint on financeability and 1.4867 clears the 1.30× sizing target ✅
- C. split the grant evenly, as a neutral position
- D. either — the `WACC` reduction is what matters

*Rationale:* Financeability is a coverage question (9.1.4, 9.4.2). A serves a different objective and should be stated as such; C is a decision avoided rather than made; D optimises the number that this domain shows is not binding — and `WACC` actually falls *further* under the equity-displacing case (6.4440 % against 6.7860 %), which is exactly why `WACC` is the wrong test here.


**9.4-B** `[9.4.3 · Application]` A 42,000,000 facility offers a 15 basis point sustainability ratchet; the verification and reporting apparatus costs 85,000 a year; the facility runs 12 years and `AF(0.06, 12) = 8.383844`. On margin alone the arrangement is:

- A. value-positive by 528,182
- B. value-negative by 184,445 ✅
- C. value-neutral
- D. value-positive by 63,000 a year

*Rationale:* `63,000 × 8.383844 = 528,182` of benefit against `85,000 × 8.383844 = 712,627` of cost → **−184,445**. A counts the benefit and omits the cost; D quotes the annual benefit gross, also omitting cost; C asserts an offset that the arithmetic contradicts.


**9.4-C** `[9.4.4 · Analysis]` A refinancing that extends maturity shows an equity gain of 3,723,616 at a 15.42 % cost of equity, 2,076,800 at 8.00 % and 566,832 at the new 4.75 % loan rate. The correct board disclosure is:

- A. a gain of 3,723,616
- B. a gain of 3,723,616, since the cost of equity is the correct discount rate for equity cash flows
- C. that most of the reported gain is deferral rather than saving — the rate component is 298,757 and the extension component 3,424,859 at 15.42 % — and that the package is worth only 566,832 discounted at the loan rate ✅
- D. no gain, since the total cash paid over the loan's life increases

*Rationale:* The decomposition and the rate sensitivity are the disclosure (9.4.4). A and B report a correct arithmetic result while concealing that it is an artefact of the discount rate; D swings to the opposite error, ignoring the genuine 195,040 a year of rate saving and the coverage improvement.


**9.4-D** `[9.4.2 · Application]` A 12,000,000 concessional loan at 2.0 % over 25 years with 7 years' grace has a debt-service stream worth 7,103,613 at a 6.0 % market rate. Its grant element is:

- A. 40.80 % ✅
- B. 4.00 %
- C. 59.20 %
- D. nil — it is a loan, not a grant

*Rationale:* `1 − 7,103,613/12,000,000 = 40.80 %`, a subsidy worth 4,896,387 (9.4.2). B quotes the interest-rate difference as though it were the subsidy; C is the complement — the debt content, not the grant content; D confuses legal form with economic substance.


## Domain 10

**10.1-A** `[10.1.2 · Application]` `CFADS` is 6,384,000 per year; the lender requires a 1.30× `DSCR`; the loan runs 12 years at 6 % (`AF` = 8.383844). Maximum debt is closest to:

- A. USD 42,000,000
- B. USD 41,171,123 ✅
- C. USD 53,522,460
- D. USD 36,143,689

*Rationale:* `6,384,000/1.30 = 4,910,769`; `× 8.383844 = 41,171,123`. A is the requested amount, which the calculation rejects; C omits the coverage divisor (sizing on full `CFADS`, `6,384,000 × 8.383844`); D uses a 10-year tenor (`AF = 7.360087`) instead of the 12-year tenor.


**10.1-B** `[10.1.1 · Analysis]` Two advisers compute Kestrel's `DSCR` as 1.39 and 1.27 from the same audited year. The most likely explanation is:

- A. one has made an arithmetic error
- B. they are applying different `CFADS` definitions — one before and one after working-capital movements — and only the facility's definition governs ✅
- C. they are using different interest rates
- D. one has used accounting profit instead of cash

*Rationale:* Domain 2's demonstration exactly: 6,984,000 versus 6,384,000 of `CFADS` on the same trading. The documented definition decides which is enforceable (10.1.1). D would produce a much larger discrepancy and is a different error.


**10.1-C** `[10.1.3 · Analysis]` A project's cash flow ramps over its first five years. Against the same forecast, sculpted debt service compared with level debt service will:

- A. support less total debt, being more complex
- B. support more total debt, because no period is over-covered merely to protect the weakest one ✅
- C. support the same debt, since total cash is unchanged
- D. eliminate refinancing risk

*Rationale:* Level service is constrained by the weakest period; sculpting holds coverage constant and so uses the stronger periods (10.1.3). C ignores that capacity is set period by period, not in total; D confuses sculpting with tenor structure.


**10.1-D** `[10.1.3 · Application]` `CFADS` before the interest tax shield is 5,880,000, the loan rate is 6.0 %, tax is 20 %, the tenor 12 years and the target coverage 1.30×. The maximum sculpted debt is closest to:

- A. USD 41,171,123
- B. USD 39,915,812 ✅
- C. USD 37,920,771
- D. USD 38,253,896

*Rationale:* `r* = 0.06 × (1 − 0.20/1.30) = 0.05076923`; `AF(0.05076923, 12) = 8.824924`; `5,880,000/1.30 × 8.824924 = 39,915,812` (10.1.3). A is the level, base-case answer on year-one `CFADS` of 6,384,000, which does not hold 1.30× in later periods; C ignores the tax shield's feedback and discounts at the full 6.0 % (`4,523,076.92 × 8.383844`); D is level service sized on the minimum period, which sculpting beats by 1,661,916.


**10.1-E** `[10.1.2 · Evaluation]` A lender offers to reduce the target `DSCR` from 1.30× to 1.25× provided the test is run on a case 5 % below base. The sponsor should conclude that:

- A. the offer is a concession worth 5 basis points of coverage
- B. the offer destroys 494,053 of capacity, because the indifference stress at these two ratios is only 3.8462 % ✅
- C. the offer is neutral, since ratio and case move in opposite directions
- D. the offer cannot be evaluated without knowing the rate and tenor

*Rationale:* `s* = 1 − 1.25/1.30 = 3.8462 %`, so a 5 % stress more than absorbs the relaxation: 40,677,069 against 41,171,123 (10.1.2b). A prices the ratio and ignores the case; C asserts an offset without testing its size; D is wrong because `s*` depends only on the two ratios — rate and tenor scale both sides equally.


**10.2-A** `[10.2.1 · Application]` `CFADS` 6,384,000; debt service 5,009,635. The `DSCR` is:

- A. 1.2743 ✅
- B. 0.7847
- C. 1.3941
- D. 2.5333

*Rationale:* `6,384,000/5,009,635 = 1.2743`. B inverts the ratio; C uses the pre-working-capital `CFADS` of 6,984,000 (Domain 2's other definition); D divides by interest only.


**10.2-B** `[10.2.1 · Analysis]` With a 1.20× covenant and `DSCR` at 1.2743 on `CFADS` of 6,384,000, the most useful figure for a board is:

- A. the `DSCR` of 1.2743
- B. the annual cash headroom of USD 372,438 — 5.8 % of `CFADS` — before the covenant is breached ✅
- C. the debt outstanding of 42,000,000
- D. the loan's remaining tenor

*Rationale:* Headroom states what may be lost before consequence, which management can monitor (10.2.1). The ratio alone does not convey magnitude, and C and D are facts, not exposures.


**10.2-C** `[10.2.2 · Analysis]` A model with level `CFADS` and annuity debt service reports `DSCR` 1.27 and `LLCR` 1.41. The soundest conclusion is:

- A. the project has a strong tail
- B. there is an inconsistency: with level cash and annuity service at the loan rate the two must be equal ✅
- C. the `LLCR` is correct and the `DSCR` is stale
- D. this is normal and requires no investigation

*Rationale:* The identity of 10.2.2 makes divergence a defect indicator — typically a discount rate that is not the loan rate, or differing `CFADS` lines. A describes `PLCR`, not `LLCR`.


**10.2-D** `[10.2.3 · Analysis]` Why do project lenders covenant primarily on `DSCR` rather than `ICR`?

- A. `ICR` is harder to compute
- B. `ICR` ignores principal repayment and rests on accounting figures exposed to classification judgment ✅
- C. `ICR` is only used for equity investors
- D. `DSCR` is required by accounting standards

*Rationale:* `ICR` can look healthy while principal is unpaid, and Domain 2 showed accounting classification flipping a profit-based covenant with no cash change. D confuses covenant drafting with accounting requirements.


**10.2-E** `[10.2.3 · Analysis]` A 42,000,000 facility is restructured from a full amortisation to a 25 % balloon. Against level `CFADS` of 6,384,000 the `DSCR` rises from 1.2743 to 1.4551. The `LLCR` will:

- A. rise in the same proportion
- B. be unchanged at 1.2743, because it discounts all the cash to maturity against all the debt outstanding and is blind to when principal is scheduled ✅
- C. fall, because more debt is outstanding for longer
- D. become undefined, since there is no level instalment

*Rationale:* `LLCR = 6,384,000 × 8.383844/42,000,000 = 1.2743` on any repayment profile (10.2.3). A confuses the period test with the horizon test; C describes the interest cost, which `LLCR` does not measure; D confuses `LLCR` with `DSCR`, which does need a periodic debt-service figure.


**10.2-F** `[10.2.4 · Evaluation]` A facility carries `DSCR` ≥ 1.20× (breached by a 4.1382 % revenue fall), debt/`EBITDA` ≤ 6.00× (4.4444 %), `LLCR` ≥ 1.15× (4.9833 %) and `ICR` ≥ 2.50× (10.6667 %). The most useful conclusion for the treasury function is:

- A. the `ICR` covenant is redundant and should be removed
- B. `DSCR` binds first, so monitoring `DSCR` is sufficient
- C. `DSCR` binds first but debt/`EBITDA` follows only 0.31 percentage points of revenue behind it, so both must be monitored — and the ordering would reverse if `ICR` were drafted at 2.90× ✅
- D. the covenants are inconsistent and one of them must be wrong

*Rationale:* The gap between the first and second triggers is what determines whether monitoring one covenant is enough, and the ordering is a drafting outcome — at 2.90× the `ICR` binds at 1.7067 % (10.2.4). A treats a loose covenant as a useless one; B ignores the 0.31-point gap; D mistakes different thresholds for an inconsistency.


**10.3-A** `[10.3.2 · Application]` Annual debt service is 5,009,635 and the facility requires a six-month DSRA. The amount to be funded is:

- A. USD 5,009,635
- B. USD 2,504,818 ✅
- C. USD 1,252,409
- D. USD 417,470

*Rationale:* `5,009,635 × 6/12 = 2,504,818`. A is twelve months; C is three; D is one month.


**10.3-B** `[10.3.2 · Analysis]` In a year when `CFADS` falls to 3,000,000 against debt service of 5,009,635, a fully funded six-month DSRA means:

- A. no covenant breach occurs, since the reserve covers the gap
- B. scheduled debt service is paid in full from cash plus reserve, but the `DSCR` covenant is still breached ✅
- C. the lenders must accelerate the loan
- D. distributions may continue, since payment was made

*Rationale:* The reserve preserves payment (gap 2,009,635, within the 2,504,818 reserve) but the ratio is computed on `CFADS`, so the covenant fails (10.3.2). D is wrong because a breach triggers lock-up (KA 10.4), and C overstates the automatic consequence.


**10.3-C** `[10.3.3 · Recall]` In the cash waterfall, reserve-account top-ups rank:

- A. below distributions to equity
- B. above distributions to equity ✅
- C. above senior debt service
- D. at the same level as operating costs

*Rationale:* Reserves are replenished before equity is paid (10.3.3); senior service ranks above the top-up, and operating costs above both.


**10.3-D** `[10.3.2 · Evaluation]` A sponsor must satisfy a 2,504,818 DSRA. Cash funding costs the spread between a 15.42 % cost of equity and a 3.00 % deposit rate; an LC is available at 1.25 %. The strongest professional conclusion is:

- A. fund it from the senior facility, since after-tax debt at 4.80 % is the cheapest source
- B. use the LC: the breakeven fee is 12.42 % a year, so 1.25 % saves 279,788 a year, and the residual questions are the issuer's standing and the LC's drafting ✅
- C. fund it in cash, because lenders will not accept an LC
- D. the routes are economically equivalent because the reserve is returned at maturity

*Rationale:* Breakeven fee `= 0.1542 − 0.0300 = 12.42 %`; at 1.25 % the LC costs 31,310 against an equity carry of 311,098 (10.3.2b). A is the trap: inside a binding coverage constraint the debt route displaces capex borrowing one for one and enlarges the equity requirement to 3,333,695. C asserts a market position that is simply untrue where the facility permits an LC. D confuses the return of principal with the cost of carrying it for twelve years.


**10.3-E** `[10.3.3 · Application]` A 42,000,000 loan at 6.0 % with a 5,009,635.23 level instalment over 12 years is subjected to a 50 % sweep of 774,364.77 of annual distributable cash. The metric that best states what the lenders have gained is:

- A. the tenor, which is unchanged at 12 years
- B. the reduction in weighted average life from 7.1887 to 6.4660 years — 0.7227 years, or 10.05 % of the exposure period ✅
- C. the 1,821,314 of interest saved
- D. the retirement in year 11 rather than year 12

*Rationale:* Average life is the measure of exposure that margin is priced against and swap notionals are set from (10.3.3). A is true and uninformative; C is a benefit to *equity* — the lenders forgo that interest; D is a single date rather than the exposure profile, and it moves with the cash case.


**10.4-A** `[10.4.2 · Analysis]` A facility has a 1.20× `DSCR` covenant and a 1.15× lock-up trigger. Why is the lock-up set *below* the covenant?

- A. it is a drafting convention with no economic effect
- B. so that cash is trapped only after a breach has occurred
- C. so that cash begins to be retained as coverage deteriorates, reducing exposure before a breach becomes an event of default ✅
- D. because lock-up and covenant tests use different `CFADS` definitions

*Rationale:* Graduated triggers act early while the project is fixable (10.4.2). B misreads the ordering; a lock-up *below* the covenant level means it engages as the ratio falls through it.


**10.4-B** `[10.4.3 · Application]` A `DSCR` covenant is breached and the sponsors have an unused equity cure. The realistic sequence is:

- A. lenders accelerate and enforce security
- B. sponsors decide whether to inject cure cash; failing that, waiver or amendment negotiations follow, with acceleration a remedy of last resort ✅
- C. the breach is disregarded if payment was made
- D. the loan converts automatically to equity

*Rationale:* Acceleration destroys value for lenders too, so cure, waiver and amendment are the normal path (10.4.3). C ignores that breach and payment failure are distinct (10.2.1).


**10.4-C** `[10.4.4 · Analysis]` Which figure best belongs on a management dashboard for covenant management?

- A. the current `DSCR`
- B. the `CFADS` level at which the binding covenant fails — 6,011,562, or 5.8 % below base case ✅
- C. the debt outstanding
- D. the loan's maturity date

*Rationale:* The operational trigger is the cash level, which management can influence and monitor (10.4.4). The ratio alone conveys no magnitude of headroom.


**10.4-D** `[10.4.3 · Application]` A `DSCR` covenant of 1.20× is breached with `CFADS` of 5,990,216 against debt service of 5,009,635.23. Under the two standard treatments the cure required is:

- A. 21,346.73 whether the cash is deemed `CFADS` or applied to prepayment
- B. 21,346.73 if deemed `CFADS`; 17,788.94 if applied to prepayment, since the prepayment reduces the denominator and `P = C ÷ λ` ✅
- C. 21,346.73 if deemed `CFADS`; 25,616.08 if applied to prepayment
- D. the breach cannot be cured with cash, only waived

*Rationale:* `C = 1.20 × 5,009,635.23 − 5,990,215.54 = 21,346.73`, and `P = 5,009,635.23 − 5,990,215.54/1.20 = 17,788.94 = C/1.20` (10.4.3). A ignores that the two treatments act on different sides of the ratio; C multiplies by the covenant instead of dividing — a nameable sign error that makes the prepayment route look dearer; D confuses a cure right with a waiver.


**10.4-E** `[10.4.2 · Evaluation]` A facility has a 1.25× distribution condition, a 1.20× covenant and a 1.15× lock-up trigger. On the lenders' bank case the project's `DSCR` runs from 1.2743 down to 1.1851. The sponsor's negotiating priority should be:

- A. the 1.15× lock-up trigger, since it has the most severe consequence
- B. the 1.25× distribution condition, which catches eight of the twelve years and 3,956,574 of dividend, while the 1.15× trigger is never reached on this case ✅
- C. the 1.20× covenant, since breach is the event that matters
- D. all three equally, since they are tested on the same ratio

*Rationale:* The threshold that binds on the case being tested is the one worth negotiating capital on; here the lock-up trigger never engages (10.4.2). A optimises for a state the project does not reach on this case; C is the two-year problem rather than the eight-year one; D ignores that the same ratio crosses three different levels at three different times.


## Domain 11

**11.1-A** `[11.1.3 · Application]` An owner's `EMV` on an item is 960,000. The contractor assesses the same item at `p` 0.45 and impact 2,900,000 and applies a 40 % loading. The net value of transferring is:

- A. +573,000
- B. −345,000
- C. −867,000 ✅
- D. −1,827,000

*Rationale:* Premium = 0.45 × 2,900,000 × 1.40 = 1,827,000; net = 960,000 − 1,827,000 = −867,000. B is the net at a **zero** loading (960,000 − 1,305,000) and understates the destruction; D is the premium alone, ignoring the retained cost avoided; A compares the owner's gross **impact** (2,400,000) with the premium, omitting probability from the retained side.


**11.1-B** `[11.1.3 · Analysis]` A bundle of items has an owner `EMV` of 2,840,000 and a contractor `EMV` of 3,300,000. The correct conclusion is:

- A. transfer if the loading can be negotiated below 40 %
- B. transfer, because the contractor is better placed to manage construction
- C. do not transfer: the breakeven loading is −13.94 %, so even at a zero margin the transfer destroys 460,000 ✅
- D. transfer, and recover the premium through the contingency

*Rationale:* The transferee's own expected cost exceeds the transferor's, so the loading is not the problem — the distribution is (11.1.3). A treats a structural result as a pricing negotiation; B asserts control where the items are ground, interface, index and permit risks the contractor does not control; D funds a value destruction twice.


**11.1-C** `[11.1.2 · Analysis]` An SPV with weak negotiating position accepts a risk it neither controls nor can absorb. The most accurate description of what has happened is:

- A. an efficient transfer, since the price was zero
- B. the risk has not been transferred but hidden — it will reappear as a claim, a private contingency, a failed bid or a counterparty default ✅
- C. a transfer on capacity grounds
- D. a transfer on control grounds

*Rationale:* Neither ground applies, and an unpriced allocation is not a costless one (11.1.2). C and D name grounds that are absent by the stem's own terms.


**11.2-A** `[11.2.2 · Application]` An input costs 1,800,000 a year; 70 % of its price movement passes through to revenue; covenant headroom is 372,437.72. The input-price rise that breaches the covenant is closest to:

- A. 20.7 %
- B. 69.0 % ✅
- C. 29.5 %
- D. 100.0 %

*Rationale:* Residual per 1 % = 1,800,000 × 0.01 × 0.30 = 5,400; 372,437.72 / 5,400 = 68.97 %. A ignores the pass-through (the `φ` = 0 answer); C uses the **passed-through** share 0.70 in place of the retained share 0.30 (372,437.72 / 12,600 = 29.56 %) — the commonest sign error here; D assumes only a doubling of the input can breach, which no calculation supports.


**11.2-B** `[11.2.3 · Analysis]` A project's `DSCR` is 1.2743 in year one, rises to 1.2863 in year five and falls to 1.2106 in year twelve. The soundest reading is:

- A. the structure is robust — coverage never breaches
- B. an indexation mismatch is consuming headroom; 85.7 % of it is gone by year twelve and the breach falls just outside the loan life ✅
- C. the model contains an error, since coverage cannot both rise and fall
- D. the improvement to year five shows revenue growth exceeding costs

*Rationale:* The shape is diagnostic of a cost driver escalating faster than the revenue index (11.2.3). A reads compliance as robustness and ignores that twelve basis points of forecast separate the two; C mistakes a normal profile for a defect; D is true only for the first five years and misses the trend.


**11.2-C** `[11.2.1 · Analysis]` Why is asking an O&M contractor to bear input-price risk usually a mis-allocation?

- A. O&M contractors are not creditworthy
- B. it neither controls the price nor can hedge it, so it prices the worst case and the premium exceeds the retained expected cost ✅
- C. input prices are always passed through by law
- D. the risk is immaterial

*Rationale:* This is the 11.1.3 result applied to operations — neither ground for transfer is present. A is a separate (and secondary) objection; C asserts a universal legal position that does not exist; D is contradicted by 11.2.2's arithmetic.


**11.3-A** `[11.3.1 · Application]` Debt 42,000,000; fixed year-one principal 2,489,635.23; `CFADS` 6,384,000; covenant 1.20×. The all-in interest rate at which the covenant fails is closest to:

- A. 6.00 %
- B. 6.74 % ✅
- C. 8.00 %
- D. 9.27 %

*Rationale:* Maximum debt service = 6,384,000/1.20 = 5,320,000; maximum interest = 2,830,364.77; ÷ 42,000,000 = 6.7390 %. A is the rate at close; C is a +200 bp shock, at which the ratio is already 1.0914; D is the rate at which debt service cannot be paid at all — a different and much later threshold.


**11.3-B** `[11.3.1 · Analysis]` A swap moves year-one `DSCR` from 1.2743 to 1.2533 and replaces a 1.0914–1.5311 range with a single value. The correct characterisation is:

- A. the swap is uneconomic, since coverage falls
- B. 0.0210 of coverage is given up to remove 0.4397 of coverage range — 20.92 units of range per unit surrendered ✅
- C. the swap eliminates all interest-rate exposure and creates no new exposure
- D. the swap is unnecessary because the project can pay debt service up to 9.27 %

*Rationale:* The trade is certainty for a small, certain cost (11.3.1). A counts the cost and not the benefit; C ignores mark-to-market and hedge-counterparty exposure; D confuses the payment threshold with the covenant threshold.


**11.3-C** `[11.3.2 · Application]` Local numerator `HC` 30,936,000, USD operating costs 1,350,000, debt service 5,009,635.23, rate at close `HC` 4.00 = USD 1, covenant 1.20×. The devaluation at which the covenant fails is closest to:

- A. 5.1 % ✅
- B. 15.9 %
- C. 21.6 %
- D. 25.0 %

*Rationale:* Covenant `CFADS` = 5,009,635.23 × 1.20 = 6,011,562.28; `x` = 30,936,000 ÷ (6,011,562.28 + 1,350,000) = 4.202369, i.e. +5.06 %. B divides `CFADS` by the covenant instead of multiplying debt service by it — the commonest covenant-trigger error; C is the point at which the `DSCR` reaches 1.00; D is the illustrative stress case, at which the ratio is already 0.9656.


**11.3-D** `[11.3.4 · Analysis]` Business-interruption cover has a 60-day waiting period; daily `CFADS` is 17,733.33 and covenant headroom is 372,437.72. The most useful statement for the finance committee is:

- A. the cover is adequate, since the maximum uninsured loss of 1,064,000 is within the DSRA
- B. the covenant survives 21 days of outage while the waiting period is 60 — so any outage beyond three weeks breaches, and a carve-out or a bought-down waiting period is required ✅
- C. the waiting period should be extended to reduce premium
- D. force majeure relief will suspend debt service during the outage

*Rationale:* The gap between a time-calibrated insurance term and a cash-calibrated covenant is the finding (11.3.4). A is true about *payment* and silent about *compliance* — the distinction of Domain 10, KA 10.2.1; C widens the gap; D is false, since force majeure suspends performance obligations and not debt service.


**11.4-A** `[11.4.1 · Application]` A register of independent items has a mean of 2,125,000 and a variance of 4,982,875,000,000. Its P80 is closest to:

- A. 2,125,000
- B. 4,003,649 ✅
- C. 4,357,235
- D. 4,841,128

*Rationale:* σ = 2,232,235; P80 = 2,125,000 + 0.8416 × 2,232,235 = 4,003,649. A is the mean, which is exceeded roughly half the time; C applies a 1.0-σ (P84) factor instead of 0.8416; D is the P80 once a 0.30 correlation is admitted — right method, different stated assumption.


**11.4-B** `[11.4.2 · Analysis]` A sponsor's register P80 is 4,003,649 and its lender's re-cut *mean* is 4,462,500. The correct inference is:

- A. the lender is applying a higher confidence level than the sponsor
- B. the parties are working from different input sets, not different percentiles — the lender's expected case already exceeds the sponsor's 80th ✅
- C. the sponsor's arithmetic is wrong
- D. the difference is immaterial at 458,851

*Rationale:* The lender re-cuts probabilities, impacts and correlation; the disagreement is evidential and must be negotiated item by item (11.4.2). A misdiagnoses the disagreement as a percentile choice; D ignores that 458,851 exceeds the whole annual covenant headroom of 372,437.72.


**11.4-C** `[11.4.4 · Application]` Annual covenant headroom is 372,437.72 and `AF(0.06, 12)` is 8.383844. The present-value operating-risk exposure the structure can absorb before its covenant fails is:

- A. USD 372,438
- B. USD 3,122,460 ✅
- C. USD 4,003,649
- D. USD 44,423

*Rationale:* Headroom × `AF(0.06, 12)` = 3,122,460 — the present value of losing that much cash every year of the loan life. A is the annual figure, not its present value; C is the register's own P80, which is the exposure being tested against the ceiling, not the ceiling; D divides (44,423) rather than multiplies by the annuity factor.


**11.4-D** `[11.4.3 · Analysis]` Which property makes AI model risk different in kind from membrane degradation risk?

- A. its impact is always larger
- B. its probability is a function of governance — validation, monitoring, approval and rollback — so the project can genuinely change it ✅
- C. it can be transferred to the software vendor in full
- D. it is uninsurable

*Rationale:* 11.4.3: `p` is set by the control set rather than by nature, which by 11.1.2's logic makes the project the right holder and controls worth more than transfer. A is unsupported; C is false — vendor liability caps are small relative to the exposure; D overstates a market position.


## Domain 12

**12.1-A** `[12.1.2 · Application]` Delay damages are 20,000 per day, capped at 10 % of a 48,000,000 EPC price, against a daily economic cost of delay of 24,733.33. For a 300-day delay the amount borne by the project company is:

- A. USD 1,420,000
- B. USD 2,620,000 ✅
- C. USD 1,484,000
- D. nil — the damages regime covers it

*Rationale:* Economic cost `24,733.33 × 300 = 7,420,000`; recovery is capped at 4,800,000 (the cap binds at day 240), so 2,620,000 is uncovered. A applies the 4,733.33 per day uncovered *rate* to 300 days and ignores that the cap stops recovery entirely after day 240. C is the 60-day interface figure of 12.2.4. D assumes a cap covers any delay.


**12.1-B** `[12.1.2 · Analysis]` An EPC contract has a 10 % delay-damages sub-cap, a 10 % performance sub-cap and a 20 % aggregate liability cap. The correct reading is:

- A. total recoverable liability is 40 % of the contract price
- B. the aggregate cap adds protection above the sub-caps
- C. the two sub-caps can exhaust the aggregate cap exactly, leaving no room for any third head of claim ✅
- D. the aggregate cap applies only to indemnities

*Rationale:* 10 % + 10 % = 20 %, so once both sub-caps are drawn the aggregate is fully consumed and a later defect or indemnity claim recovers nothing (12.1.1, 12.1.2). A double-counts; B is the common misreading the arithmetic disproves; D asserts a carve-out the structure does not contain.


**12.1-C** `[12.1.3 · Application]` Kestrel's `CFADS` falls by 453,000 per year for 25 years on a 5 % output shortfall; the appraisal rate is 8 % (`AF(0.08, 25) = 10.674776`); debt is 42,000,000 against base `CFADS` of 6,384,000 and an instalment of 5,009,635.23. Which figure is the **value-basis** performance damages amount?

- A. USD 562,851 — the prepayment that restores the 1.20 covenant
- B. USD 2,980,263 — the buy-down that restores the sized 1.2743 `DSCR`
- C. USD 4,835,674 ✅
- D. USD 453,000 — one year of lost `CFADS`

*Rationale:* `453,000 × 10.674776 = 4,835,673.53`. A restores only the covenant, B only the sized coverage — both are lender-facing measures, not the sponsors' loss; D omits discounting and the remaining 24 years.


**12.1-D** `[12.1.3 · Analysis]` A 4,800,000 performance damages receipt is applied wholly to mandatory prepayment, cutting debt to 37,200,000 and the instalment to 4,437,105, and lifting the `DSCR` to 1.3367. The soundest conclusion is:

- A. equity has been over-compensated, since the `DSCR` now exceeds the sized 1.2743
- B. equity remains short by about 521,000, because a 25-year loss has been compensated with 12 years of debt-service relief ✅
- C. equity is exactly compensated, since the amount equals the cap
- D. the prepayment is irrelevant to equity

*Rationale:* Relief of 572,529.77 × `AF(0.08, 12) = 7.536078` is 4,314,629 against a loss of 4,835,674 — a residual gap of 521,045 (12.1.3). A confuses a coverage ratio with value; C confuses the cap with the loss; D ignores that prepayment raises distributions.


**12.2-A** `[12.2.2 · Application]` `CFADS(x) = 9,060,000x − 2,676,000`; debt service is 5,009,635.23; the covenant is 1.20×. The minimum contracted volume that holds the covenant is:

- A. 90.0000 %
- B. 84.8304 %
- C. 95.8892 % ✅
- D. 100.0000 %

*Rationale:* `(1.20 × 5,009,635.23 + 2,676,000)/9,060,000 = 0.958892`. A is the commercially negotiated floor, which delivers only 1.0935; B is the volume at which `DSCR` = 1.00, i.e. cash merely equals debt service; D is the sized case, which the covenant does not require.


**12.2-B** `[12.2.2 · Analysis]` Why does a 10 % reduction in contracted volume cut `DSCR` by far more than 10 %?

- A. because the covenant is tested on revenue, not `CFADS`
- B. because 85 % of cash operating cost is fixed, so `CFADS` falls 14.19 %, and debt service does not fall at all ✅
- C. because interest rates rise with lower volume
- D. because the tariff falls with volume

*Rationale:* Operating leverage compounded by a fixed denominator: 906,000 of `CFADS` lost on 6,384,000 is 14.19 %, and all of it lands on the ratio (12.2.2, and the 1.510× leverage of Domain 5, KA 5.4.3). A misstates the test basis; C and D invent mechanisms.


**12.2-C** `[12.2.3 · Analysis]` A concession pays 85 % of senior debt outstanding on project-company default. Kestrel's debt outstanding is 39,510,365 at the end of year 1 and 27,965,695 at the end of year 5. The most important observation is:

- A. the formula is adequate, since 85 % is a high recovery
- B. the shortfall is largest early — 5,926,555 at the end of year 1 — precisely when default risk is highest ✅
- C. the shortfall is largest late, as the debt amortises
- D. the formula protects equity but not lenders

*Rationale:* 15 % of a declining balance is largest at the start (12.2.3). C inverts the profile; D reverses the ranking — a debt-based formula protects lenders first and leaves equity at zero.


**12.2-D** `[12.2.4 · Application]` Two packages are let without an interface regime; a 60-day handover dispute delays completion. The daily economic cost of delay is 24,733.33. The project company's most likely position is:

- A. it recovers 1,484,000 from the contractor whose scope was late
- B. it bears 1,484,000, recovers nothing, and faces prolongation claims from both contractors ✅
- C. it recovers from its insurers under a delay-in-start-up policy in all cases
- D. it suffers no loss, because the offtake date moves with the works

*Rationale:* Each contractor shows performance of its own scope and claims for the other's delay (12.2.4). A assumes an allocation the documents do not make; C assumes cover that depends entirely on policy wording and a triggering insured peril; D assumes an offtake flexibility that date-certain revenue contracts do not grant.


**12.3-A** `[12.3.2 · Application]` Cover comprises a 4,800,000 on-demand bank bond (payment effectively certain) and a parent guarantee taking total nominal cover to 9,600,000, the parent assessed at a 0.70 probability of paying in full. Risk-adjusted cover is:

- A. USD 9,600,000
- B. USD 8,160,000 ✅
- C. USD 6,720,000
- D. USD 3,360,000

*Rationale:* `4,800,000 + 4,800,000 × 0.70 = 8,160,000`. A ignores credit quality; C applies 0.70 to the whole 9,600,000, haircutting the bank bond as well; D counts only the guarantee increment.


**12.3-B** `[12.3.2 · Analysis]` A contractor offers to replace a 4,800,000 on-demand bond with a parent guarantee, the parent assessed at 0.70. The face amount that leaves the project company no worse off is:

- A. USD 4,800,000
- B. USD 3,360,000
- C. USD 6,857,143 ✅
- D. any amount, since a guarantee from a large group is stronger than a bond

*Rationale:* `4,800,000 ÷ 0.70 = 6,857,142.86`, a 1.4286× multiple (12.3.2). A treats unequal certainty as equal; B applies the haircut in the wrong direction; D confuses balance-sheet size with payment certainty and ignores conditionality and timing.


**12.3-C** `[12.3.3 · Recall]` The primary commercial purpose of a direct agreement is to:

- A. increase the liability cap of the counterparty
- B. give lenders notice, an extended cure period and the right to step in, so the project's contracts survive the project company's default ✅
- C. transfer the offtake obligation to the lenders
- D. provide additional security over the asset

*Rationale:* Direct agreements preserve the contract, not the cap (12.3.3). C misstates step-in, which is a right to assume the contract, usually through a nominated transferee; D describes asset security, a separate instrument.


**12.4-A** `[12.4.3 · Application]` A contractor claims 90 days and 1,870,000; delay damages are 20,000 per day; the project company's daily economic cost of delay is 24,733.33. The total exposure of the event to the project company is closest to:

- A. USD 1,870,000
- B. USD 3,670,000
- C. USD 4,096,000 ✅
- D. USD 2,226,000

*Rationale:* Quantum 1,870,000 + own economic cost `24,733.33 × 90 = 2,226,000` gives 4,096,000 (12.4.3). A counts only the money claim; B adds the forgone damages of 1,800,000 to the quantum but omits the project company's own cost; D counts only the economic cost.


**12.4-B** `[12.4.3 · Analysis]` The disputed sum is 1,520,000; the present value of the expected award is 611,110 and of own costs 736,005. The rational settlement ceiling, allowing 60,000 of negotiation cost, is:

- A. USD 760,000 — the midpoint
- B. USD 1,287,115 ✅
- C. USD 611,110
- D. USD 1,520,000 — the full claim

*Rationale:* Fighting costs a present value of 1,347,115; deducting the 60,000 that settling itself costs gives an indifference point of 1,287,115, or 84.68 % of the disputed sum (12.4.3). A is one possible settlement, not the ceiling; C omits own costs, which are the larger component; D assumes no defence has value.


**12.4-C** `[12.4.3 · Analysis]` Why does a 0.40 probability of complete victory still leave a settlement ceiling at 84.68 % of the disputed sum?

- A. because the expected award is larger than the disputed sum
- B. because own costs are certain and immediate while the award is contingent and 26 months away ✅
- C. because the discount rate is too high
- D. because liquidated damages are excluded from the calculation

*Rationale:* PV of own costs (736,005) exceeds the PV of the expected award (611,110); certainty and timing dominate probability (12.4.3). A is arithmetically false; C confuses a parameter with the mechanism; D is untrue — the forgone damages are the largest part of the disputed sum.


**12.4-D** `[12.4.1 · Analysis]` A risk register shows five construction risks as transferred; the allocation matrix shows no clause reference for any of them. The correct conclusion is:

- A. the matrix is incomplete but the allocation stands
- B. these are orphan risks: priced as transferred, retained in fact, and the register overstates protection ✅
- C. the risks are doubly covered
- D. the contractor's aggregate cap covers them

*Rationale:* An allocation without a clause is not an allocation (12.4.1); the register and the model are both wrong until the drafting follows. C is the opposite defect; D assumes a cap can cover a liability the contract never created.


## Domain 13

**13.1-A** `[13.1.3 · Application]` A diligence stream costs a 260,000 fee, `p` = 0.30, `C` = 6,200,000 and `F` = 400,000. Run inside a parallel envelope so that it adds no elapsed time, its breakeven detection rate is:

- A. 13.98 %
- B. 14.94 % ✅
- C. 4.19 %
- D. 72.02 %

*Rationale:* `260,000 / [0.30 × (6,200,000 − 400,000)] = 260,000/1,740,000 = 14.94 %`. A divides by `p × C` and omits the pre-close correction cost `F`, overstating the avoidable loss and so understating the breakeven; C divides the fee by `C` alone and ignores `p`; D is the same stream's breakeven when sequenced serially with eight weeks of delay priced in.


**13.1-B** `[13.1.4 · Analysis]` Seven diligence streams with 1,500,000 of fees and durations totalling 54 weeks are worth **+4,066,700** in a twelve-week parallel envelope and **−1,146,900** sequenced serially, at a cost of delay of 124,133.33 a week. The correct reading is:

- A. the streams are marginal and some should be dropped
- B. the entire 5,213,600 difference is the 42 weeks of sequencing, so the sequencing decision matters roughly three and a half times more than the fee negotiation ✅
- C. the fees are too high and should be reduced by 5,213,600
- D. serial sequencing is preferable because each stream can rely on the last

*Rationale:* `42 × 124,133.33 = 5,213,600` and the fees, probabilities and detection rates are identical in both readings (13.1.3). A misreads a scheduling result as a scope result; C is arithmetically impossible against 1,500,000 of fees; D describes a real but rare dependency that 13.1.4 requires to be priced rather than assumed.


**13.1-C** `[13.1.3 · Evaluation]` On the parameters above, insurance diligence sequenced serially has a breakeven detection rate of 289.86 %. The professional conclusion is:

- A. insurance diligence should be abandoned
- B. the insurance adviser is overpriced
- C. insurance diligence must sit inside the envelope, where its breakeven is 31.25 % — the finding is about sequencing, not about the stream ✅
- D. the calculation is invalid because a probability cannot exceed one

*Rationale:* A breakeven above 100 % says the configuration cannot pay, not that the review is worthless; inside the envelope the same stream breaks even at 31.25 % (13.1.3). B is wrong because the 60,000 fee is the smallest of the seven — 496,533.33 of delay is what makes it impossible. D mistakes an impossible-configuration signal for an arithmetic error.


**13.1-D** `[13.1.2 · Analysis]` Three parties rely on the same plant availability figure: the technical adviser who derives it, the financial model that earns revenue on it and the offtake agreement that pays damages against it. The specific diligence control for this is:

- A. asking each adviser to confirm they are comfortable
- B. a cross-stream interface list naming every multi-adviser assumption with one named owner and one version ✅
- C. running the streams serially so each can rely on the previous report
- D. relying on the model audit to catch any inconsistency

*Rationale:* The overlap is where projects fail, and the countermeasure is single ownership of the shared assumption (13.1.2). A produces no evidence; C costs 124,133.33 a week for a control a register achieves free; D is out of scope — the audit tests the model against documents, not whether two advisers assumed the same thing.


**13.2-A** `[13.2.3 · Application]` A model builds the debt-service schedule as an annuity due rather than in arrears. On 42,000,000 at 6 % over 12 years (`AF` = 8.383844) with `CFADS` of 6,384,000, the reported `DSCR` and the truth are:

- A. reported 1.2743, true 1.3508
- B. reported 1.3508, true 1.2743 ✅
- C. reported and true both 1.2743 — the convention does not affect the ratio
- D. reported 1.2224, true 1.2743

*Rationale:* `AF_due = 8.383844 × 1.06 = 8.886875`, instalment `42,000,000/8.886875 = 4,726,071` and `6,384,000/4,726,071 = 1.3508`; the correct in-arrears instalment of 5,009,635.23 gives 1.2743. A reverses the direction — an annuity due has a *smaller* instalment, so it flatters the ratio; C denies that the denominator moved by 283,564; D is the `DSCR` after the separate `CFADS`-definition finding, on the correct instalment.


**13.2-B** `[13.2.2 · Evaluation]` A model audit returns 34 findings — 3 Class 1, 7 Class 2, 24 Class 3 — and the status report says "31 of 34 closed, 91.2 %". The correct challenge is:

- A. the count is wrong
- B. the 91.2 % is driven by the 24 Class 3 findings, which are 70.6 % of the count and none of the impact; if the three open findings are the Class 1 findings, 0 % of the impact is closed ✅
- C. Class 3 findings should not be reported at all
- D. the audit should be re-run

*Rationale:* Findings are not commensurable, so a count-based metric measures the class that changes nothing (13.2.2). A is not the defect; C loses the aggregate record that shows model hygiene; D confuses a reporting failure with an audit failure.


**13.2-C** `[13.2.3 · Application]` After correcting `CFADS` to 6,124,000, debt capacity at a 1.30× requirement over 12 years at 6 % (`AF` = 8.383844) is closest to:

- A. USD 41,171,123
- B. USD 39,494,354 ✅
- C. USD 42,000,000
- D. USD 51,342,661

*Rationale:* `6,124,000/1.30 = 4,710,769.23`, `× 8.383844 = 39,494,354`. A is Domain 10's capacity on the *uncorrected* 6,384,000; C is the mandated amount, which the calculation rejects by 2,505,646; D omits the coverage divisor entirely (`6,124,000 × 8.383844`).


**13.2-D** `[13.2.1 · Analysis]` Which audit scope layer would have caught a model that implements a valid amortisation the finance documents do not provide for?

- A. arithmetic — the formulae are checked
- B. structure — the schedule closes at zero
- C. documents — the model is tested against what the facility actually says ✅
- D. assumptions and provenance — the inputs are traced

*Rationale:* An annuity due computes correctly and closes at zero, so layers A and B both pass (13.2.3); only conformance to the document detects it. D would trace the rate and tenor, both of which are right.


**13.3-A** `[13.3.2 · Analysis]` Five CP chains have expected durations of 9.60, 20.10, 21.20, 18.25 and 7.30 weeks against a 20-week base close. The expected close date is:

- A. 21.2000 weeks — the longest chain's expected duration
- B. 22.4075 weeks — the expected maximum of the chains ✅
- C. 20.0000 weeks — the base close, since each chain is more likely than not to hold its date
- D. 76.4500 weeks — the sum of the chains

*Rationale:* Close is a conjunction, so the date is the expected *maximum*, which exceeds the maximum of the expectations (13.3.2). A takes the maximum of the expectations, the standard error; C mistakes each chain's individual likelihood for the joint one — the probability of the base date is 34.125 %; D adds chains that run in parallel.


**13.3-B** `[13.3.3 · Evaluation]` Chain B (18 weeks, slip 6 weeks at probability 0.35) is not the critical path; chain C (20 weeks, slip 4 at 0.30) is. Reducing chain B's slip probability to 0.10 moves `E[close]` from 22.4075 to 21.7950 weeks at a cost of delay of 124,133.33 a week. The intervention is worth:

- A. nothing — chain B is not on the critical path
- B. USD 76,031.67, and it is the highest-value acceleration available ✅
- C. USD 744,800 — six weeks of delay avoided
- D. USD 298,851 — the whole expected slip cost

*Rationale:* `(22.4075 − 21.7950) × 124,133.33 = 76,031.67` (13.3.3). A is the critical-path fallacy the conjunction exists to correct; C prices the full slip as if it were certain and unavoidable; D is the total expected slip cost, which the intervention reduces but does not eliminate.


**13.3-C** `[13.3.4 · Application]` Close costs total 2,709,000 on a 42,000,000 facility funding a 60,000,000 envelope, of which 544,000 is payable to lenders. Close costs as a share of debt raised are:

- A. 4.515 %
- B. 6.45 % ✅
- C. 1.295 %
- D. 2.00 %

*Rationale:* `2,709,000/42,000,000 = 6.45 %`. A divides by the 60,000,000 envelope rather than the debt; C counts only the fees payable to lenders; D is the model's original rule-of-thumb provision, which the itemised budget shows to be 20 % of the true figure.


**13.3-D** `[13.3.4 · Analysis]` With a fixed close-cost component of 2,016,000 and a proportional component of 1.65 % of debt, the facility size at which total close costs fall to 3.0 % of debt raised is closest to:

- A. USD 67,200,000
- B. USD 149,333,333 ✅
- C. USD 122,181,818
- D. USD 90,300,000

*Rationale:* `2,016,000/(0.030 − 0.0165) = 2,016,000/0.0135 = 149,333,333`. A divides the fixed component by 3.0 % and ignores the proportional component; C divides by 0.0165; D applies 3.0 % to the wrong base.


**13.3-E** `[13.3.5 · Recall]` A condition converted into a post-close undertaking is appropriately handled that way when:

- A. its outcome is uncertain and diligence could not resolve it
- B. it is mechanical and merely slow, so the only open question is timing ✅
- C. the sponsor does not wish to disclose it
- D. the lenders have waived it without a deadline

*Rationale:* Post-close undertakings convert timing risk into a dated covenant; they convert *outcome* risk into default risk, which is why A is the case for not closing rather than for an undertaking (13.3.5). D describes the defect Case study B prices.


**13.4-A** `[13.4.2 · Application]` A 42,000,000 facility carries a 1.20 % arrangement fee, of which 0.25 % is praecipium; the balance is a pool paid pro rata on allocations. The arranger's final hold is 15,000,000. It earns:

- A. USD 180,000
- B. USD 247,500 ✅
- C. USD 105,000
- D. USD 504,000

*Rationale:* `105,000 + 0.95 % × 15,000,000 = 247,500`, a yield of 1.650 % against participants' 0.950 %. A applies the full 1.20 % to the hold and omits the praecipium's exclusivity; C is the praecipium alone; D is the whole fee, most of which is paid away to participants.


**13.4-B** `[13.4.3 · Evaluation]` An arranger underwrites 42,000,000 with a 15,000,000 target hold, places 20,000,000, and charges itself 90 basis points a year on the excess over 12 years (`AF` = 8.383844). Against fees of 247,500 the outcome is:

- A. a profit of 184,500
- B. a loss of USD 280,682.17 — the 528,182.17 carry on a 7,000,000 excess exceeds the whole fee ✅
- C. a loss of 508,500, since the entire residual hold is charged
- D. break-even

*Rationale:* `7,000,000 × 0.0090 × 8.383844 = 528,182.17`; `247,500 − 528,182.17 = −280,682.17` (13.4.3). A charges the carry for one year only (63,000); C charges the full 22,000,000 residual rather than the excess over target hold.


**13.4-C** `[13.4.4 · Application]` Fifty basis points of market flex on a 42,000,000 facility over 12 years at 6 % (`AF` = 8.383844) is worth, in present value:

- A. USD 210,000
- B. USD 1,760,607 ✅
- C. USD 2,520,000
- D. USD 504,000

*Rationale:* `42,000,000 × 0.0050 × 8.383844 = 1,760,607`, some 3.49 times the arrangement fee. A is one year of the flex, undiscounted; C sums twelve years without discounting; D is the arrangement fee the sponsor negotiates instead.


**13.4-D** `[13.4.4 · Analysis]` With `CFADS` of 6,384,000, a 42,000,000 facility over 12 years and a 1.20× `DSCR` covenant, the margin increase the covenant just survives is:

- A. 50.0 basis points
- B. 111.4 basis points ✅
- C. 25.0 basis points
- D. unlimited, since the covenant is tested on `CFADS`

*Rationale:* Maximum instalment `6,384,000/1.20 = 5,320,000`, requiring `AF = 7.894737`, which solves at 7.1138 % — 111.4 basis points above 6.00 %. A is the contractual flex cap, which consumes 44.9 % of that headroom; D forgets that debt service is the denominator and rises with the rate.


**13.4-E** `[13.4.5 · Recall]` A transaction has executed all its documents but two third-party consents remain outstanding. Its correct description is:

- A. financially closed
- B. signed, not closed — close occurs when every condition is satisfied and funds are available ✅
- C. closed subject to conditions subsequent
- D. in default

*Rationale:* Signing and close are distinct, and the gap is where the long-stop date operates (13.4.5). C misuses a term for post-close obligations; D confuses an unsatisfied condition with a breach of an existing obligation.


## Domain 14

**14.1-A** `[14.1.1 · Application]` At a data date, undrawn senior debt is 15,910,254 and uncalled equity is 6,818,680; remaining unallocated contingency is 1,500,000. Available commitment for the in-balance test is:

- A. USD 24,228,934
- B. USD 22,728,934 ✅
- C. USD 15,910,254
- D. USD 21,228,934

*Rationale:* Available commitment is `15,910,254 + 6,818,680 = 22,728,934`. A adds the 1,500,000 of contingency, which is a **use** funded by that same commitment — the double-count of 14.1.1, and it flatters the test by exactly the contingency balance. C counts only debt and ignores the equity commitment. D deducts the contingency instead of ignoring it, an error in the opposite direction.


**14.1-B** `[14.1.3 · Analysis]` On identical scope and contingency, Kestrel's capitalised interest is 1,338,006 under equity-first funding and 2,804,070 under debt-first. The soundest reading is:

- A. debt-first is a modelling error, since total funding is fixed at 60,000,000
- B. the 1,466,064 spread is a real cost of the sequencing clause; equity-first is cheapest for the project and safest for the lender, while debt-first defers the sponsor's cheque and is worth 183,013 of present value to the sponsor ✅
- C. the orders are economically equivalent because the sponsor funds the difference either way
- D. the difference is capitalised, so it has no effect on the project's economics

*Rationale:* The sequencing changes the drawn balance on which interest accrues, so it changes total uses (59,223,409 / 60,000,000 / 60,689,473) — the arithmetic of 14.1.3. A misreads a fixed envelope as a fixed requirement. C ignores that the sponsor's *present value* improves while the project's nominal cost worsens. D is the commonest form of the error: capitalised interest enters the depreciable base and the coverage arithmetic and is therefore paid, with interest, over the loan life.


**14.1-C** `[14.1.2 · Application]` Kestrel's quarter-eight draw carries certified spend of 6,076,994 and accrued interest of 560,308. As a share of the period requirement, the interest line is closest to:

- A. 0.83 %
- B. 8.44 % ✅
- C. 9.22 %
- D. 3.52 %

*Rationale:* `560,308 / (6,076,994 + 560,308) = 8.44 %`. A is the quarter-one share, the point being that the interest row grows as the spend row falls. C divides interest by certified **spend** rather than by the period requirement, the commonest slip. D is total capitalised interest as a share of the 60,000,000 envelope (Domain 6), a different denominator entirely.


**14.1-D** `[14.1.2 · Recall]` Which condition to a drawing is a *judgment* rather than a verifiable fact?

- A. the drawing is within the availability period
- B. the requested amount is within the undrawn commitment
- C. no potential event of default is continuing, and the project remains in balance ✅
- D. the technical adviser's certificate is attached

*Rationale:* A, B and D are checkable against a calendar, a ledger and a document schedule. C requires a forecast of remaining cost and a view on what constitutes a *potential* default — which is why those two limbs are where the monitoring relationship is conducted (14.1.2).


**14.2-A** `[14.2.1 · Application]` At a data date the remaining committed EPC value is 18,720,000, approved but uncertified variations are 840,000, assessed claim exposure is 1,260,000, the bottom-up remaining owner-retained scope is 2,400,000 and remaining capitalised interest is 1,436,674. The lender's cost-to-complete is:

- A. USD 21,120,000
- B. USD 24,656,674 ✅
- C. USD 23,220,000
- D. USD 20,846,038

*Rationale:* All five lines sum to 24,656,674. A is the bottom-up `CTC(d)`, which omits the three lines earned value cannot see. C omits remaining capitalised interest — the single most commonly dropped line, because it is not work. D is `CTC` on the `BAC/CPI` method, a different basis entirely.


**14.2-B** `[14.2.1 · Analysis]` A project's blended `CPI` is 0.949821 across a fixed-price EPC scope of 48,000,000 (certified against milestones) and owner-retained scope of 3,600,000 running at a scope `CPI` of 0.60. Applying `BAC/CPI` to the blended index:

- A. is correct, since `BAC/CPI` is the standard persistence forecast
- B. attributes 2,535,849 of uplift to a scope that cannot overrun and only 190,189 to the scope that is overrunning, so the total is right only by coincidence ✅
- C. understates the forecast, because fixed-price scope carries the greater risk
- D. is invalid because `CPI` cannot be computed on a milestone-certified contract

*Rationale:* The composition of the 2,726,038 uplift is the defect (14.2.1). A treats a method as valid irrespective of the scope mix it is applied to. C inverts the risk ownership under a wrap. D overstates: `CPI` on that scope is computable and equals 1.000 by construction — which is exactly why blending destroys the signal.


**14.2-C** `[14.2.2 · Analysis]` Remaining unallocated contingency is 1,500,000. Known committed claims on contingency total 3,420,000 and the open risk register has a mean of 1,085,000 and a P80 of 1,764,289. The defensible coverage ratio on the remainder is:

- A. 1.3825
- B. 0.8502
- C. 0.2893 ✅
- D. 0.4386

*Rationale:* `1,500,000 / (3,420,000 + 1,764,289) = 0.2893`. A tests contingency against the *mean* of open risk only — covering the mean is a coin-flip and it also ignores the 3,420,000 already committed. B is the P80 test on open risk only, which is defensible arithmetic on the wrong denominator. D divides by the 3,420,000 of known committed claims alone (`1,500,000/3,420,000`), omitting the open register entirely.


**14.2-D** `[14.2.2 · Analysis]` A monthly report states that 58.85 % of contingency has been drawn at 61.00 % certified progress and concludes that contingency consumption is "in line". The correct professional response is:

- A. accept it; the draw rate is below progress, so consumption is favourable
- B. reject the inference — contingency is not consumed pro rata to progress, and the only meaningful test is coverage on the remainder ✅
- C. accept it if the draw rate has tracked progress for three consecutive periods
- D. recompute the draw rate on physical rather than certified progress

*Rationale:* The draw rate is coincidental because risk is not uniformly distributed through a programme (14.2.2); here the honest coverage on the remainder is 0.2893. C makes a trend out of a coincidence. D refines a measure that should not be relied on at all.


**14.3-A** `[14.3.1 · Application]` A quarter's work comprises achieved milestones of 5,760,000 and one milestone worth 1,440,000 assessed at 92 % complete; the contractor has also procured 690,000 of off-site materials. Retention is 5 %. The senior debt advanced at 70 % gearing under a cost-incurred basis exceeds that under a milestone basis by:

- A. USD 2,014,800
- B. USD 1,914,060
- C. USD 1,339,842 ✅
- D. USD 690,000

*Rationale:* `(7,774,800 − 5,760,000) × 0.95 × 0.70 = 1,339,842`. A is the gross certified spread, before retention and before the gearing split. B applies retention but not gearing. D counts only the off-site materials and omits the 92 % milestone.


**14.3-B** `[14.3.2 · Analysis]` Kestrel's second retention tranche of 600,000 falls due twelve months after the commercial operations date and is paid from operating cash. The consequence is:

- A. none; retention is part of the EPC price already funded
- B. `CFADS` falls to 5,784,000 and the `DSCR` to 1.1546, breaching the 1.20 covenant, because the release exceeds the 372,438 of annual headroom by 227,562 ✅
- C. the `DSCR` is unaffected, since retention is a balance-sheet movement
- D. the release is deducted from debt service, so coverage improves

*Rationale:* The release is a cash outflow in an operating period, and coverage is computed on cash (Domain 10). A confuses being *funded* with being *available* — the availability period has ended. C misstates a cash payment as an accrual. D reverses the direction of the effect.


**14.3-C** `[14.3.3 · Application]` A variation costs 1,850,000 and adds 240,000 a year of `CFADS`. The loan factor is `AF(0.06, 12) = 8.383844` and base `DSCR` is 1.2743. The maximum debt-funded amount that leaves base coverage unchanged is closest to:

- A. USD 1,850,000
- B. USD 1,578,947 ✅
- C. USD 1,295,000
- D. USD 2,012,123

*Rationale:* `(240,000/1.274303) × 8.383844 = 1,578,947`, being 85.35 % of the cost. A is full debt funding, which yields a marginal `DSCR` of 1.0876. C is the 70 % pro-rata debt share — inside the limit but not the limit. D applies `AF` to the full `ΔCFADS` without the coverage divisor, the sizing error of Domain 10, Exercise 10.1.


**14.3-D** `[14.3.3 · Analysis]` A variation has an `NPV` of +711,946 at the board's 8 % rate and a marginal `DSCR` of 1.0876 against a 1.20× covenant if fully debt-funded. The correct conclusion is:

- A. approve and fund from the facility; a positive `NPV` is decisive
- B. reject; a marginal `DSCR` below the covenant disqualifies the variation
- C. approve, but cap debt funding at the coverage-neutral 1,578,947 and fund the residual 271,053 from equity or contingency ✅
- D. approve and renegotiate the covenant

*Rationale:* Value and coverage answer different questions and both bind; the funding mix is the lever that satisfies both (14.3.3). A ignores the covenant. B discards 711,946 of value that a funding decision can capture. D proposes to renegotiate a covenant over 271,053, which no lender would price kindly.


**14.4-A** `[14.4.2 · Application]` Kestrel's first instalment of 5,009,635.23 falls on a fixed calendar date twelve months after the scheduled commercial operations date. `CFADS` accrues evenly from actual completion at 6,384,000 a year. If completion slips four months, the `DSCR` at the first test is:

- A. 1.2743
- B. 0.8496 ✅
- C. 1.1681
- D. 0.4248

*Rationale:* `6,384,000 × 8/12 = 4,256,000`; `÷ 5,009,635.23 = 0.8496`. A assumes the test date moves with completion. C is a one-month slip. D is an eight-month slip.


**14.4-B** `[14.4.2 · Analysis]` With a 1.20× covenant, debt service of 5,009,635.23 and `CFADS` of 6,384,000 a year accruing from actual completion, the slip at which the first covenant test fails is closest to:

- A. six months
- B. three weeks ✅
- C. two months
- D. it cannot fail, since annualised `CFADS` is unchanged

*Rationale:* `12 × (1 − 6,011,562.28/6,384,000) = 0.7001` months, 21.00 days. A and C overstate the tolerance by an order of magnitude. D is the error the calculation exists to kill: the covenant is tested on the *period*, not on an annualised run rate.


**14.4-C** `[14.4.1 · Analysis]` Kestrel's funded cost of a month of slip at completion is 415,000 and its full economic cost 947,000; delay damages are 600,000 a month. The most useful observation for a board is:

- A. damages recover 80.86 % of the cost of delay
- B. damages over-recover the funded cost by 44.58 % while recovering only 63.36 % of the economic cost, so the drawdown tests keep passing while equity value is destroyed ✅
- C. damages fully cover the delay, so no action is required
- D. the delay is cost-neutral because damages exceed the funded cost

*Rationale:* The two bases give 144.58 % and 63.36 % recovery, and only the second is a statement about value (14.4.1). A quotes Domain 5's narrower basis, which omits the 205,000 of monthly prolongation cost. C and D mistake a passing funding test for an absence of loss.


**14.4-D** `[14.4.3 · Application]` A performance test settles `CFADS` at 5,900,000 against debt of 42,000,000, `AF(0.06, 12) = 8.383844`. The buy-down required to restore the base-case `DSCR` of 1.2743 is closest to:

- A. USD 779,434
- B. USD 3,184,211 ✅
- C. USD 4,800,000
- D. nil, since 1.1777 exceeds 1.00

*Rationale:* `42,000,000 − (5,900,000/1.274303) × 8.383844 = 3,184,211`. A restores only the 1.20× covenant, leaving zero headroom. C is the performance damages cap, not the calibrated amount. D confuses paying debt service with satisfying a covenant (Domain 10, KA 10.2.1).


## Domain 15

**15.1-A** `[15.1.3 · Application]` Kestrel's debt service is 5,009,635.23 and `CFADS` is 6,384,000. Which figure states the headroom to the **distribution condition** of 1.25×?

- A. USD 372,437.72
- B. USD 121,955.96 ✅
- C. USD 622,919.49
- D. 0.0243 of a ratio point

*Rationale:* `6,384,000 − 5,009,635.23 × 1.25 = 121,955.96`, or 1.9103 % of `CFADS`. A is the headroom to the 1.20× covenant and is the standard confusion this KA exists to remove; C is the headroom to the 1.15× lock-up; D expresses the gap in ratio points, which conveys no magnitude (Domain 10, KA 10.2.1).


**15.1-B** `[15.1.2 · Analysis]` Kestrel's revenue falls by 500,000 in a year. Holding working capital constant, `CFADS` falls by:

- A. USD 500,000
- B. USD 400,000 ✅
- C. USD 625,000
- D. USD 100,000

*Rationale:* Revenue falls to `EBITDA` one-for-one, profit before tax falls by the same amount and cash tax falls by 20 % of it, so `CFADS` falls by `0.80 × 500,000 = 400,000` — the cash-to-revenue gearing of 15.1.2. A ignores the tax shield entirely; C divides by 0.80 instead of multiplying, which is the correct arithmetic run backwards (it converts a `CFADS` gap into a revenue gap); D is the tax saving mistaken for the cash effect.


**15.1-C** `[15.1.4 · Analysis]` A project's `CFADS` begins declining in the second quarter of year one. Its covenant is a rolling four-quarter `DSCR`. The earliest date on which that covenant can fail, and the reason, is:

- A. the same quarter, because the test is continuous
- B. up to four quarters later, because a trailing window dilutes each weak quarter with three earlier stronger ones ✅
- C. never, provided debt service is paid
- D. immediately, because rolling tests are more sensitive than annual tests

*Rationale:* Kestrel's window carries the decline for four quarters before the sum falls through the threshold (15.1.4). A misdescribes a trailing measure; C confuses breach with payment default (Domain 10, KA 10.2.1); D reverses the effect — smoothing reduces sensitivity, which is why the forward test is needed alongside.


**15.1-D** `[15.1.3 · Application]` Kestrel's capacity payment is 7,300,000 at a guaranteed availability of 95.0 %, abated pro rata. Availability alone must fall to roughly what level before the 1.20× covenant is breached?

- A. 93.02 %
- B. 88.94 % ✅
- C. 90.15 %
- D. 87.43 %

*Rationale:* Covenant headroom 372,437.72 ÷ 0.80 = 465,547.16 of revenue, ÷ 76,842.11 per point = 6.0585 points below 95.0 %. A is the floor for the 1.25× distribution condition (1.9839 points); C omits the 0.80 cash-to-revenue gearing altogether (372,437.72 ÷ 76,842.11 = 4.8468 points); D applies the gearing twice (465,547.16 ÷ 0.80 = 581,933.95, giving 7.5731 points).


**15.2-A** `[15.2.3 · Application]` Kestrel's `CFADS` is 6,384,000, debt service 5,009,635.23 and the maintenance-reserve charge 600,000. The base-case distributable amount is:

- A. USD 1,374,364.77
- B. USD 774,364.77 ✅
- C. USD 6,384,000
- D. USD 774,364.77 less the six-month debt service reserve of 2,504,817.62

*Rationale:* `6,384,000 − 5,009,635.23 − 600,000 = 774,364.77`. A omits the reserve top-up, which is the single most common distribution-forecasting error (15.2.2); C is `CFADS` itself; D double-counts a reserve that was funded at close and requires no top-up while debt service is level.


**15.2-B** `[15.2.2 · Analysis]` In Kestrel's year one the backward `DSCR` is 1.2743 against a 1.20× covenant and a 1.25× distribution condition, and 774,364.77 of cash remains after debt service and the reserve charge. Nothing is distributed. The correct explanation is:

- A. the covenant was breached
- B. the distribution condition also requires a forward-looking test, which failed at 1.0979 on the sponsors' re-forecast ✅
- C. the debt service reserve had to be topped up first
- D. distributions are prohibited in the first operating year

*Rationale:* Both backward tests passed; the forward leg of the distribution condition failed (15.1.4, 15.2.2). A is contradicted by 1.2743 > 1.20; C is false — a level annuity's six-month reserve never needs topping up; D invents a term.


**15.2-C** `[15.2.4 · Analysis]` Kestrel's equity lost 3,182,311.16 of cash over six years but only 2,709,838.79 of present value at 8 %. The reason is:

- A. an arithmetic inconsistency between the two measures
- B. 1,463,877.46 of the withheld cash was eventually distributed, so part of the loss is deferral rather than destruction ✅
- C. the discount rate should have been the loan rate of 6 %
- D. the 1,500,000 injection should not be counted as a cost to equity

*Rationale:* Deferral costs the time value, not the principal (15.2.4). C would change both figures without changing the relationship; D is wrong because an injection is cash equity provides and does not get back other than through later distributions already counted.


**15.2-D** `[15.2.1 · Recall]` In the operating waterfall, a handback or decommissioning reserve top-up ranks:

- A. above senior debt service, being a statutory obligation
- B. below distributions to equity, being a long-dated liability
- C. above distributions to equity and below senior debt service, alongside the other reserve restorations ✅
- D. at the same level as cash taxes

*Rationale:* Restorations sit between obligations and permissions (15.2.1). A inverts the security architecture; B is the error that leaves a handback obligation unfunded (Case study B); D confuses a reserve with a period cost.


**15.3-A** `[15.3.2 · Application]` Kestrel's 7-year refinancing at 4.45 % saves 272,495.82 a year with a present value of 1,418,714.07 against total costs of 1,625,549.77. The correct conclusion is:

- A. proceed — a 155 basis point margin saving is material
- B. reject on these terms: net present value is (206,835.69), and the margin must fall 177.94 basis points to break even ✅
- C. proceed — the `DSCR` improves from 1.2743 to 1.3476
- D. reject — refinancing an operating asset is never economic

*Rationale:* `1,418,714.07 − 1,625,549.77 = (206,835.69)`; the breakeven all-in rate is 4.2206 %, a 122.06 bp margin (15.3.2). A prices the headline and not the transaction; C cites a coverage improvement that has no cash value to equity; D overgeneralises — the 10-year variant is worth +799,481.12.


**15.3-B** `[15.3.2 · Analysis]` Of the 2,425,030.89 present value in Kestrel's 10-year refinancing, how much is attributable to the margin reduction measured alone?

- A. USD 2,425,030.89
- B. USD 1,418,714.07 ✅
- C. USD 586,108.78
- D. USD 420,208.04

*Rationale:* The margin component is the 7-year option's present value (15.3.2). A is the total, which treats the whole gain as margin; C is the tenor component measured alone; D is the interaction term that exists only because the extension is priced at the lower rate.


**15.3-C** `[15.3.3 · Analysis]` The sponsors hold two equity cures. At a test date the `DSCR` is 1.1905 against a 1.20× covenant, requiring a cure of 47,668.16; a waiver is available for 55,307.03. The better decision, and why, is:

- A. cure — it is 7,638.87 cheaper in cash
- B. waive — paying 7,638.87 more preserves an option worth 808,625.80 of cover nine months later, or 0.9447 % of the value preserved ✅
- C. neither — a marginal breach may be disregarded
- D. cure — cure cash counts as `CFADS` and so also improves the reported ratio

*Rationale:* Cures are scarce options and must be spent in proportion to what they buy (15.3.3). A optimises one test date in isolation, which is precisely the error; C invents a materiality threshold facilities do not contain; D is true of the mechanics and irrelevant to the choice.


**15.3-D** `[15.3.4 · Application]` The lenders consent to a 10-year refinancing worth 544,113.42 to equity at time zero, in exchange for a 50 % sweep of distributable cash that costs equity 646,327.86 of present value. Equity should:

- A. accept — the tenor extension improves coverage to 1.8108
- B. decline or renegotiate: net of the sweep the transaction destroys 102,214.44, and the breakeven sweep share is 40.3334 % ✅
- C. accept — a sweep only accelerates repayment and does not reduce total distributions
- D. accept — the `IRR` cost of 39.76 basis points is immaterial

*Rationale:* The sweep exceeds the gain (15.3.4). A cites coverage, which is the lenders' benefit; C is false — accelerated prepayment reduces the present value of distributions even where the undiscounted total is similar; D judges materiality on the wrong measure, since 646,327.86 exceeds the whole gain.


**15.4-A** `[15.4.2 · Analysis]` A lender extends a distressed loan at its existing contract rate so that coverage is restored. Measured at the contract rate, its recovery is:

- A. below par, because payment is deferred
- B. exactly par, because the present value of a longer annuity at the same rate is the same principal ✅
- C. above par, because more interest is received in total
- D. indeterminate without the sponsor's discount rate

*Rationale:* The extension identity of 15.4.2 — which is why "amend and extend" is the default and why a committee measuring recovery only at the contract rate sees no loss. The real cost appears at a higher required return: 95.0779 % at 7.00 % for Kestrel. C confuses total nominal interest with present value; D is irrelevant to the lender's measure.


**15.4-B** `[15.4.3 · Analysis]` Kestrel's enforcement floor is a 91.4291 % recovery. A proposed haircut recovers 82.9037 % on the lenders' 7.00 % required return. The correct characterisation is:

- A. an aggressive but negotiable proposal
- B. outside the feasible set — enforcement pays the lenders 8.5254 points more ✅
- C. acceptable, because enforcement destroys value for everyone
- D. acceptable, because equity value of 18,656,183.22 is the highest of the options

*Rationale:* No lender accepts less than the floor (15.4.3). A misreads infeasibility as negotiating distance; C is true of the parties jointly and irrelevant to the lender's individual choice; D states equity's preference, which is not a constraint on lenders.


**15.4-C** `[15.4.4 · Analysis]` A sponsor holding a 9.8591 % `IRR` project sells at the end of year eight at exactly its own 8.0 % discount rate and reports an `IRR` of 11.7666 %. The value created is:

- A. the 1.9075 percentage point `IRR` uplift
- B. nil — the `NPV` at 8 % is identical at 5,027,733.03; the uplift is the arithmetic of early crystallisation ✅
- C. USD 828,232.87
- D. USD 34,386,097.04

*Rationale:* A sale at the discount rate is a swap of equal value at a different date (15.4.4). A mistakes a rate for value; C is the value of yield compression at 7.50 % rather than 8.00 %, which is a different transaction; D is the price, not the gain.


**15.4-D** `[15.4.5 · Application]` An 8,000,000 handback obligation at year 25 can be funded by a reserve earning 3.00 % over years 16–25 (697,844.05 a year) or years 6–25 (297,725.66 a year). At an 8 % equity discount rate, the earlier profile:

- A. is cheaper, because the total contributed is 1,023,927.31 lower
- B. costs equity 513,274.78 more in present value, because the reserve earns 500 basis points less than equity requires ✅
- C. costs the same, because both reach 8,000,000 at year 25
- D. is cheaper, because contributions are smaller

*Rationale:* Pre-funding is equity lending to itself at the reserve rate (15.4.5). A and D compare undiscounted amounts across different decades; C would be true only if the reserve earned the 8 % discount rate — the indifference-rate invariant, at which every profile costs `8,000,000 × DF(0.08, 25) = 1,168,143.24`.


## Domain 16

**16.1-A** `[16.1.2 · Application]` Manual review costs 13.60 a record and leaves 1.80 % of records with an undetected error; automated processing costs 1.04 a record and leaves 2.60 %; an undetected error costs 320; the automated platform's committed fixed cost is 148,000 a year. The breakeven volume is:

- A. 11,783 records a year
- B. 14,800 records a year ✅
- C. 15,812 records a year
- D. 142,308 records a year

*Rationale:* All-in costs are `13.60 + 0.018 × 320 = 19.36` and `1.04 + 0.026 × 320 = 9.36`; the advantage is 10.00 and `148,000 ÷ 10.00 = 14,800`. A omits the error-cost term from both sides (advantage 12.56, `148,000 ÷ 12.56 = 11,783`) — the specific defect of 16.1.2. C divides the fixed cost by the automated all-in cost per record (`148,000 ÷ 9.36`) instead of by the advantage, treating the comparison as an absorption problem rather than a differential one. D makes the same error against the automated *processing* cost alone (`148,000 ÷ 1.04`), ignoring the manual alternative entirely.


**16.1-B** `[16.1.2 · Analysis]` An organisation reprices an undetected error from USD 320 to USD 1,200 on the same two processes — manual review 13.60 a record leaving 1.80 % of records with an undetected error, automated processing 1.04 a record leaving 2.60 %, on a committed fixed cost of 148,000 a year. The breakeven volume:

- A. falls, because errors are now more expensive and automation catches more of them
- B. is unchanged, because the error rates are unchanged
- C. rises from 14,800 to 50,000, because the automated process has the higher error rate so a larger consequence erodes its advantage ✅
- D. becomes irrelevant, because at a high enough consequence neither process is acceptable

*Rationale:* Manual becomes `13.60 + 0.018 × 1,200 = 35.20` and automated `1.04 + 0.026 × 1,200 = 32.24`; the advantage falls from 10.00 to 2.96 and `148,000 ÷ 2.96 = 50,000`. A assumes the automation is the more accurate process, which the given rates contradict. B confuses the rates with their monetised effect. D is a governance observation, not the arithmetic asked for — and at 56,400 records the automation is still positive, by 18,944.


**16.1-C** `[16.1.3 · Analysis]` A detector's five thresholds give accuracies of 98.5208, 98.5729, 98.4167, 97.5208 and 93.7083 per cent and total misclassification costs of 196,400, 161,800, 131,200, 114,800 and 154,400. The threshold that should be operated, and why:

- A. the second — it maximises accuracy, which is the standard classification metric
- B. the fifth — it maximises recall, and missing an error is the expensive outcome
- C. the fourth — it minimises total cost, which is the only objective that reflects the different consequences of the two error types ✅
- D. the first — its precision of 84.5070 % gives the investigation team the most reliable queue

*Rationale:* Accuracy weights all items equally and is dominated by the 46,800 clean ones; cost weights each by consequence, and the minimum is 114,800 at T4 — choosing T2 on accuracy costs 47,000 a year. B over-corrects: T5's extra 120 catches cost 78,000 of investigation to save 38,400. D optimises the queue's comfort rather than the firm's loss.


**16.1-D** `[16.1.3 · Analysis]` A false positive costs 40 and a missed error 320. Loosening the threshold from T4 to T5 would add 120 true positives and 1,950 false positives. The correct conclusion is:

- A. accept — the marginal alerts still have 27.1357 % precision overall, well above the 12.5 % break-even
- B. refuse — marginal precision is 5.7971 %, below the 12.5 % break-even, and the step costs 78,000 to save 38,400 ✅
- C. accept — recall rises from 80 % to 90 %, and recall is the measure lenders ask about
- D. indeterminate without knowing the anomaly base rate

*Rationale:* The test is on the *marginal* alerts: `120 ÷ (120 + 1,950) = 5.7971 %`, and the net is −39,600, which reconciles exactly to the cost column rising from 114,800 to 154,400. A quotes T5's *average* precision, which is the standing error in threshold decisions. C treats a ratio as an objective. D is false — the counts given already embed the base rate.


**16.2-A** `[16.2.2 · Application]` A tool extracts defined terms at 92 % per-item accuracy. Twenty-six of them are load-bearing. The probability that all 26 are correct is closest to:

- A. 92.00 %
- B. 11.44 % ✅
- C. 88.56 %
- D. 99.80 %

*Rationale:* `0.92²⁶ = 0.114415`. A mistakes per-item accuracy for deliverable accuracy — the error the topic exists to correct. C is the complement, the probability that at least one is wrong. D is the per-item accuracy that *would* be required for a 95 % clean sweep, `0.95^(1/26)`.


**16.2-B** `[16.2.2 · Analysis]` Given that result, the defensible professional response is:

- A. reject machine extraction and read all 340 terms manually
- B. procure a tool with 99.8029 % per-item accuracy
- C. use the tool for all 340 terms, then verify the 26 load-bearing terms against the executed document by a named person on a recorded date — about USD 3,120 against a single known 600,000 exposure ✅
- D. accept the register and rely on the tool's confidence scores to flag which entries to check

*Rationale:* The remedy is a change of scope, not of tool: verification of the load-bearing subset is 192.3077 times cheaper than one 600,000 definitional error. A discards the tool's genuine value on the other 314 terms. B specifies an unachievable target. D lets the model choose its own verification scope, and a model is least confident where it can detect its own error — not where it cannot.


**16.2-C** `[16.2.1 · Analysis]` An analyst reports "40 scenarios run, 3 breached, worst-case `DSCR` 1.04". The soundest critique is:

- A. 40 is too few to demonstrate the absence of a low-probability mode — 230 are needed for 90 % confidence on a 1 % mode — and the worst of 40 draws is a percentile, not a defined stress case ✅
- B. 3 breaches out of 40 is a 7.5 % breach probability, which should be reported as the covenant risk
- C. the run is adequate; scenario counts above 30 are conventionally sufficient
- D. the analyst should have reported the mean `DSCR` across the 40 scenarios

*Rationale:* Coverage and labelling are the two defects: `ln(0.10) ÷ ln(0.99) = 230`, and a generated minimum is roughly the `1/(k+1)` percentile of an unvalidated tail. B treats a sample frequency from an unvalidated generator as a probability. C invents a convention. D reports the statistic covenants never test (Domain 10: the minimum, not the average).


**16.2-D** `[16.2.3 · Application]` A module takes 40 hours to build and 8 to review by hand, or 6 hours assisted with 26 hours of review. At USD 150 an hour, the saving from assistance is:

- A. 70.8333 %
- B. 33.3333 % ✅
- C. 85.0000 %
- D. nil, since total hours are similar

*Rationale:* `(48 − 32) × 150 = 2,400` on 7,200, or 33.3333 %. A is the apparent saving when the review is left at the hand-built 8 hours — the omission worth 2,700 of review against 1,137,623.20 of expected defect cost. C compares build hours only (6 against 40). D ignores the 16-hour difference.


**16.3-A** `[16.3.2 · Application]` How many independent, representative test cases, all passing, are needed to be 95 % confident that a model's defect rate is below 1 %?

- A. 100
- B. 299 ✅
- C. 95
- D. 459

*Rationale:* `n ≥ ln(0.05) ÷ ln(0.99) = 298.0729`, so 299 — the "rule of three" at `3/p`. A is the reciprocal of the bound, which gives only a 63 % confidence. C confuses the confidence level with a count. D is the requirement at 99 % confidence, not 95 %.


**16.3-B** `[16.3.2 · Analysis]` That model passes its 299 cases and is then used 56,400 times a year. The correct statement is:

- A. the validation shows the model will produce fewer than 10 errors a year
- B. the validation is consistent with up to about 564 errors a year, so monitoring, human approval at concentrated-consequence points and rollback must carry the assurance ✅
- C. the validation is invalid because the test volume is smaller than the production volume
- D. the validation guarantees a 99 % success rate in production

*Rationale:* A 1 % bound at 56,400 uses admits `0.01 × 56,400 = 564` errors; bounding expected errors at 10 would need 16,895 passing cases. A inverts the bound. C misstates the requirement — test volume need not match production volume, it must be independent and representative. D reads a one-sided confidence bound as a point guarantee.


**16.3-C** `[16.3.3 · Analysis]` A detector reports 80 % recall at a total cost of 114,800. A blind audit of 1,000 of the 46,090 unflagged items finds 17 genuine errors. The most important consequence is:

- A. the threshold should be loosened until recall reaches 90 %
- B. the true anomaly population is about 1,744 rather than 1,200, true recall is 55.0459 %, and the restated cost is 288,880 — the missed errors are the kinds the labels never contained, so the answer is a model change, not a threshold change ✅
- C. the audit sample is too small to act on
- D. the detector should be withdrawn

*Rationale:* Extrapolating 1.70 % over 46,090 gives 784 missed errors; recall and cost restate to 55.0459 % and 288,880. Loosening (A) does not reach errors the model cannot see — scaling the T4→T5 benefit by 1.4533 gives 55,808 against 78,000 of cost, so T4 remains optimal. C is wrong: 937 would suffice for a ±1-point bound at 95 % confidence. D discards a control that still avoids substantial loss.


**16.3-D** `[16.3.4 · Application]` A model is used 4 times a year, fails with probability 0.885585 per use, and a failure costs 600,000. With a tolerance of 50,000 of accumulated expected loss between validations, the revalidation interval is:

- A. annual, since four uses a year is low volume
- B. about 8.59 days — shorter than the interval between uses, so the model must be verified at every use ✅
- C. quarterly, matching the use frequency
- D. about 3.26 years, since the annual `EMV` is small

*Rationale:* `EMV = 4 × 0.885585 × 600,000 = 2,125,404`; `50,000 ÷ 2,125,404 = 0.023525` years. A and C tier by volume or convenience rather than by expected loss — the inversion the topic exists to correct. D is the interval for the payment detector (M2), whose `EMV` is 15,360.


**16.4-A** `[16.4.2 · Application]` A second approver costs USD 24 a payment; 0.20 % of payments are bad and a second approval catches 85 % of those. The payment value above which dual approval pays is closest to:

- A. USD 12,000
- B. USD 14,118 ✅
- C. USD 120,000
- D. USD 80,000

*Rationale:* Caught rate `0.0020 × 0.85 = 0.0017`; `24.00 ÷ 0.0017 = 14,117.65`. A omits the catch rate and divides by 0.0020. C misplaces a decimal in the bad-payment rate, using 0.02 % rather than 0.20 % (`24.00 ÷ 0.0002`). D uses the **15 %** a second approval misses instead of the 85 % it catches (`24.00 ÷ 0.0003`) — the sign-of-the-control error.


**16.4-B** `[16.4.2 · Analysis]` On the same figures, 4,800 payments a year total 68,000,000, of which 1,150 payments worth 63,400,000 exceed the threshold. Comparing blanket dual approval with thresholded dual approval:

- A. blanket is better, because it catches more bad payments in total
- B. they are equivalent, because the same catch rate applies
- C. thresholded is worth 80,180 a year against blanket's 400, because the 3,650 small payments cost 87,600 to approve and protect only 7,820 ✅
- D. neither is worthwhile, since the blanket policy nets only 400

*Rationale:* Blanket costs 115,200 and avoids 115,600; thresholded costs 27,600 and avoids 107,780. A is true and irrelevant — the extra catches cost more than they save. B ignores that the value protected varies with payment size while the approver's cost does not. D draws the wrong conclusion from the blanket result: the control is strongly positive where it is targeted.


**16.4-C** `[16.4.1 · Analysis]` A managed service costs 148,000 a year with an assessed 0.80 % annual breach probability; a private deployment costs 410,000 at 0.15 %. The most defensible use of the arithmetic is:

- A. compute the `EMV` avoided of 42,250, conclude the managed service wins, and close the question
- B. state the breakeven consequence of 40,307,692 and then debate whether the information at risk is worth that — which segments the answer: operating pipeline managed, live-transaction data private ✅
- C. choose the private deployment, because confidentiality cannot be quantified
- D. choose the managed service, because the probability differential is only 0.65 percentage points

*Rationale:* The arithmetic's role is to locate the argument, not to end it: 262,000 ÷ 0.0065 = 40,307,692, a figure implausible for a routine pipeline and entirely plausible for a live transaction. A over-reads an assessment as a measurement. C abandons a usable structure. D confuses a small differential with a small consequence.


**16.4-D** `[16.4.3 · Application]` A change worth 416,000 a year waits for a committee with a 60-day effective interval and a 10-day paper lead time. The value forgone while it waits is closest to:

- A. USD 6,268
- B. USD 45,589 ✅
- C. USD 68,384
- D. nil — the benefit accrues once approved

*Rationale:* `E[wait] = 60/2 + 10 = 40` days at `416,000 ÷ 365 = 1,139.7260` a day. A is the delegated panel's 5.5-day wait. C prices the full 60-day meeting interval in place of `M/2 + L`, dropping both the expected-half rule and the paper lead time. D ignores that a delayed benefit is a forgone benefit.
