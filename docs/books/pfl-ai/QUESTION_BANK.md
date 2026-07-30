# Question bank — PFL-AI Body of Knowledge

> **Derived, not duplicated.** Every item is the question as it appears in its Knowledge
> Area, consolidated here by `_build/make_question_bank.py`. Answer keys and rationales are
> the chapters' own. To change an item, change it in its Knowledge Area and regenerate —
> which is why there is no second copy to fall out of step.

**450 items** across 16 domains. Every numeric option in every item is
independently recomputed by the golden-answer suite, not only the correct one, so a
distractor cannot be arithmetically impossible without the gate failing.

## Coverage by cognitive level

| Level | Items | Share |
|---|---|---|
| Recall | 13 | 2.9 % |
| Comprehension | 63 | 14.0 % |
| Application | 117 | 26.0 % |
| Analysis | 131 | 29.1 % |
| Evaluation | 126 | 28.0 % |

A bank weighted heavily to recall tests memory rather than competence; one weighted
heavily to Evaluation is unanswerable under time pressure. The distribution above is a fact
to be reviewed against the examination blueprint, not a claim that it is correctly balanced —
the blueprint weightings are an open decision (see `CORPUS_GATE_REPORT.md` §9).

## Coverage by domain

| Domain | Items | Levels represented |
|---|---|---|
| 1 | 24 | Recall, Comprehension, Application, Analysis, Evaluation |
| 2 | 30 | Recall, Comprehension, Application, Analysis, Evaluation |
| 3 | 32 | Recall, Comprehension, Application, Analysis, Evaluation |
| 4 | 24 | Recall, Comprehension, Application, Analysis, Evaluation |
| 5 | 28 | Comprehension, Application, Analysis, Evaluation |
| 6 | 28 | Comprehension, Application, Analysis, Evaluation |
| 7 | 28 | Comprehension, Application, Analysis, Evaluation |
| 8 | 29 | Comprehension, Application, Analysis, Evaluation |
| 9 | 28 | Recall, Comprehension, Application, Analysis, Evaluation |
| 10 | 32 | Recall, Comprehension, Application, Analysis, Evaluation |
| 11 | 26 | Comprehension, Application, Analysis, Evaluation |
| 12 | 27 | Recall, Comprehension, Application, Analysis, Evaluation |
| 13 | 30 | Recall, Comprehension, Application, Analysis, Evaluation |
| 14 | 28 | Recall, Comprehension, Application, Analysis, Evaluation |
| 15 | 28 | Recall, Comprehension, Application, Analysis, Evaluation |
| 16 | 28 | Comprehension, Application, Analysis, Evaluation |

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


**1.1-G** `[1.1.4 · Analysis]` An investment committee paper recommends a 25-year availability concession to an insurer whose liabilities have an average duration of 14 years, on the ground that "extending the tenor from 15 years to 25 brings the asset's duration into line with the liability". The stream is level and the discount rate is 8.0 %. The reviewer should:

- A. accept the recommendation — a 25-year asset is the closest available match to a 14-year liability
- B. reject the reasoning as arithmetically impossible: at 8 % a level stream's duration is capped at `(1 + r)/r` = 13.5000 years, and 25 years reaches only 9.2254, so no tenor closes the gap ✅
- C. correct the reasoning but keep the recommendation — 9.2254 years is the longest duration the concession can offer, so tenor is still the right lever and the residual gap belongs to the liability side
- D. reject the recommendation because duration is not a meaningful measure for infrastructure assets

*Rationale:* extending 15 years to 25 buys **2.6309** years of duration (6.5945 → 9.2254) and the ceiling is never reached at any finite tenor, so the paper's stated mechanism cannot deliver what it claims (WE 1.1.4); duration is added by deferral (+3.0000 years), escalation (+0.4398 at 2.5 %) or a lower rate (the ceiling is 17.6667 at 6 %), and the residual gap is closed with instruments rather than asset selection. A confuses the asset's life with its duration, the error the example exists to disarm. C is the strongest of the wrong answers and gets halfway: it drops the paper's mechanism and then repeats its conclusion, when 9.2254 is *not* the longest duration available — deferring the same 15-year stream by three years reaches **9.5945**, which is why WE 1.1.4 ranks the greenfield position ahead of the long concession. D discards a first-order measure the profession uses correctly; the defect is the claim made with it, not the measure.


**1.1-H** `[1.1.2 · Comprehension]` Kestrel's breakeven failure probability `p*` falls from 93.7893 % on a 15,000,000 facility to 51.6392 % at 42,000,000 and approaches 28.2224 % on very large ones. Which statement restates the reason correctly?

- A. lenders charge a lower margin on larger facilities, so the incremental cost of the project route shrinks
- B. the close-cost premium is broadly fixed while the exposure the ring-fence removes scales with the facility, so a partly fixed cost divided by a proportional benefit falls toward the margin differential alone ✅
- C. larger projects fail more often, so the probability required to justify the structure is lower
- D. enforcement recoveries improve with project size, which enlarges the exposure the structure removes

*Rationale:* the cost has a fixed element (the **2,359,000** close-cost premium) and a proportional one (140 basis points, worth **6.7694 %** of the debt in present value), while the exposure removed is proportional (**23.9857 %** of debt on these assumptions) — which is exactly why the curve is a hyperbola with 28.2224 % as its asymptote (1.1.2). A asserts a pricing pattern the example does not contain and the arithmetic does not need. C confuses the probability *required* with the probability *expected*. D reverses the direction: a better recovery reduces the exposure removed and therefore *raises* `p*` — at a 50 % recovery it reaches 127.6910 %.


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


**1.2-G** `[1.2.3 · Evaluation]` An analyst's paper on the WE 1.2.3 project — 100,000,000 of capital cost, 12,000,000 of steady operating cash, 70,000,000 of senior debt at 6.0 % and 30,000,000 of equity — reports that the structure "absorbs a 65 % fall in project cash before the equity's own cash runs out", having modelled the debt as interest-only when the facility amortises over 12 years and carries a 1.20× `DSCR` covenant. Each objection below is a fair one. Which is the more decisive for the credit committee?

- A. the paper overstates the cushion that matters: on the amortising facility the equity's cash exhausts at a 30.42 % decline and the 1.20× covenant engages at 16.51 %, so the reported resilience is roughly four times the distance to the first consequence ✅
- B. the paper's 26.0000 % cash-on-cash return is overstated, since the amortising structure returns 12.1687 %
- C. the paper should have measured the equity with an `IRR` over the whole life rather than a single-period return
- D. the paper omits the tax shield on the interest charge

*Rationale:* A and B follow from the same substitution, but the committee is buying downside protection, and only A quantifies it — the covenant bites at −16.51 %, the equity's cash runs out at −30.42 %, and the paper reported −65.00 % (WE 1.2.3b). B is true and second-order, and on its own it is a *misleading* correction: the **4,149,392.06** of principal repaid is a return of capital rather than a cost, so the amortising equity is better off than 12.1687 % implies. C is a sound methodological point that quantifies nothing by itself. D is true of both columns equally — year-one interest is identical at 4,200,000 — so it cannot be an objection to this paper.


**1.2-H** `[1.2.3 · Comprehension]` The levered-return identity is `r_e = r_u + (D/E) × (r_u − r_d)`. Which statement restates what it says about a structure geared 70/30 against debt costing 6.0 %?

- A. gearing adds return in every state of the world, because debt is cheaper than equity
- B. gearing adds 2.333333 times whatever the project earns above 6.0 % and subtracts 2.333333 times whatever it earns below it, so one structure amplifies in both directions ✅
- C. gearing raises the equity return by the difference between the cost of equity and the cost of debt
- D. gearing raises the equity return wherever the project's unlevered return exceeds the equity holders' required return

*Rationale:* the identity multiplies the **spread over the debt rate** by the debt-to-equity ratio, so its sign follows the spread: at an unlevered 12.0 % it adds **14.0000** points to reach 26.0000 %, and at an unlevered 4.0 % it subtracts **4.6667** points to −0.6667 % (WE 1.2.3). A ignores the sign; below the crossover — project cash of 6,000,000 on this structure — leverage subtracts. C names the wrong spread: the identity uses `r_u − r_d`, not `k_e − k_d`. D substitutes the equity's required return for the debt rate and so puts the crossover in the wrong place.


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


**1.3-G** `[1.3.3 · Evaluation]` After an unverified benchmark cost Kestrel **1,117,200** of delay (WE 1.3.3), four controls are proposed. Which should the leader adopt first?

- A. prohibit general-purpose AI tools for market and precedent research
- B. require that no benchmark enter a negotiating position without a source line naming the transaction, the document and the date ✅
- C. procure an enterprise AI tool whose licence carries a vendor indemnity
- D. require a second analyst to review every AI-generated summary before it is circulated

*Rationale:* the loss arose because an unsourced number became a negotiating position, and a stale internal spreadsheet produces the identical loss — so the control that matches the failure is a rule about numbers rather than about tools (1.3.3). A is defensible and narrower than the risk: it forgoes a genuine accelerant while leaving the human failure mode untouched. C buys a commercial remedy the profession cannot rely on — none of the 1,117,200 would have been recoverable from a vendor, and tool approval governs *which* tools may be used, never *who* answers for the output (MCQ 1.3-C). D would probably have caught this instance and is the right *second* control, applied to material items; as a blanket rule it charges a second analyst against every summary, most of which never reach a negotiation.


**1.3-H** `[1.3.1 · Evaluation]` A board asks for "one number" for the incremental cost of the limited-recourse route, having been shown **5,202,128** built on a 140-basis-point margin differential and a 40 % enforcement recovery. The most professional response is:

- A. give the 5,202,128 without qualification, since the board asked for a single figure
- B. give 5,202,128 as the base case, name the two parameters that move it most, and state the consequence — at a 50 % recovery the breakeven rises to 127.6910 % and the route cannot pay at any probability ✅
- C. decline to give a single figure, because the enforcement recovery is unknowable
- D. give the figure computed on the 50 % recovery, as the more prudent of the two

*Rationale:* candour about numbers means presenting a forecast with its assumptions and sensitivities, never as a certainty (1.3.1), and B does what the board asked while disclosing what would change the answer. A presents an estimate as a fact. C is a defensible instinct that fails the duty from the other side — declining to quantify leaves the judgment to whoever will. D substitutes one point estimate for another, and an unlabelled prudence misrepresents a forecast exactly as an unlabelled optimism does (MCQ 1.3-A).


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


**2.1-F** `[2.1.1 · Comprehension]` Kestrel's first year reports accrual `EBITDA` of 7,500,000 and cash-basis `EBITDA` of 6,900,000, and 2.1.1 concludes that cash accounting is "not the conservative choice — merely late". Which statement restates that claim correctly?

- A. the cash basis reports a lower result in every period, which is why it is the prudent basis for a lender
- B. the 600,000 the cash basis has not yet reported is the closing net working-capital balance, and it is reported in whichever later period the trading cycle stops growing or unwinds — over the project's life the two bases sum to the same figure ✅
- C. the cash basis is late because customers pay in arrears, so the 600,000 difference is permanent
- D. the two bases differ because accrual accounting charges depreciation, which a cash basis cannot recognise

*Rationale:* the cumulative gap *is* a balance-sheet position — receivables 900,000 less payables 300,000 — and balance-sheet positions unwind, so what separates the bases is timing rather than prudence (2.1.1). A is wrong in both directions: a project releasing working capital reports **more** on a cash basis, which is how a declining project flatters its cash result (Domain 15, KA 15.1.2). C treats a reversing balance as permanent. D names a real difference that is irrelevant here, because the comparison is struck at `EBITDA`, above any depreciation charge.


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


**2.2-G** `[2.2.1 · Evaluation]` A management pack reports Kestrel's 1.20× `DSCR` covenant headroom correctly — a 3.8796 % revenue fall — and then adds that "the project breaks even at a 21.50 % fall in revenue". Each statement below is true on Kestrel's first-year figures. Which is the more decisive objection to that second sentence?

- A. the profit breakeven sits at revenue of 9,420,000 — a 21.50 % fall — which is the figure quoted
- B. the cash breakeven, where `DSCR` reaches 1.00, sits at revenue of 10,282,044 — a 14.32 % fall — so it binds **7.18** revenue points earlier, and it, not the profit breakeven, is the point at which the facility stops being paid ✅
- C. net income is 4.6512 times as revenue-elastic as revenue itself, so profit falls 4.6512 % for every 1 % of revenue lost
- D. the sentence is harmless, because the reader has already been given the covenant test, which bites long before either breakeven

*Rationale:* both breakevens are correctly computed, and only one of them is the point at which cash stops covering debt service, so the pack has quoted the slacker of the two and told the reader the project is 21.5 points from trouble when on its own chosen measure it is 14.3 (2.2.1). A restates the pack's own figure and objects to nothing. C is true and is a *sensitivity* rather than a threshold — it says how fast profit moves, not where anything breaks. D is the strongest of the wrong answers and is a good point aimed at the wrong target: the covenant does bite first, at 3.8796 % (KA 2.4.2), and a correctly stated covenant elsewhere in the pack does not license a misstated breakeven beside it — a breakeven is not invalidated by the existence of a tighter test above it, nor excused by one.


**2.2-H** `[2.2.4 · Comprehension]` Kestrel's cash conversion — operating cash flow ÷ net income — is 1.8721. Which statement shows the ratio has been understood?

- A. collection is efficient: the project turns each unit of profit into 1.87 units of cash
- B. a figure above 1.0 is close to guaranteed on a capital-intensive asset, because depreciation of 2,400,000 exceeds the working-capital absorption of 600,000; the diagnostic content is in those two components separately, not in the ratio ✅
- C. the ratio exceeds 1.0 because the project is profitable, and would fall below 1.0 in a loss-making year
- D. the ratio measures earnings quality, and 1.87 indicates a low risk of revenue being recognised early

*Rationale:* the numerator adds back a charge fixed by a depreciation policy set years earlier and deducts this period's working-capital movement, so the ratio measures capital intensity and would stay above 1.0 through a year of collapsing collections (2.2.4). A and D read a structural arithmetic property as an operational or a quality signal — the specific misreading the passage exists to correct. C offers a mechanism the arithmetic does not have: the add-back does not depend on profitability at all.


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


**2.3-H** `[2.3.1 · Evaluation]` At financial close a sponsor presses for `CFADS` to be defined *before* working-capital movements, which on the first year's figures would report `DSCR` of 1.3941 rather than 1.2743. Advising the sponsor, the soundest position is:

- A. press for the exclusion: it buys 0.1198 of reported coverage on identical cash, and a further 18.25 days of collections of covenant headroom with it
- B. press for the definition that is the more *stable* rather than the higher today — exclusion flatters a growing trading cycle and turns against the sponsor in the first year of decline, when a shrinking cycle releases cash the covenant would then ignore ✅
- C. accept the lenders' inclusive definition, because cash-based tests are always the more conservative and conservatism protects the sponsor
- D. leave `CFADS` undefined and rely on the financial model's treatment, which both parties have reviewed

*Rationale:* the definitional choice is not neutral across the life of the facility — excluding working capital raises the ratio while the cycle grows and lowers it while the cycle unwinds — so a sponsor optimising the first test period is buying a definition that bites later (2.3.1), and stability is what a lender is actually paying for. A is the arithmetic correctly done against the wrong objective, treating one period as the covenant's life. C reaches a defensible destination by an argument that is not true, and an untrue argument will not survive the negotiation. D is the weakest of the four: an undefined term is settled later by whoever is enforcing it, and Domain 13's model audit tests the model against the *documented* definition, which must therefore exist. The finance leader's contribution here is the arithmetic across the facility's life; the drafting of the definition, and what it will mean on enforcement, are matters for the facility's counsel.


**2.3-I** `[2.3.3 · Evaluation]` Kestrel's 1,200,000 control-system overhaul restores existing performance rather than extending capability. The finance director observes that capitalising it would protect a profit-based covenant; the tax adviser observes that, in a regime where the deduction follows the accounting treatment, expensing is worth **78,958** of present value. The finance leader should recommend:

- A. capitalise — the covenant is the larger financial exposure, and the classification is a judgment either way
- B. expense — the 78,958 is a genuine cash benefit, and this book's own discipline puts cash ahead of profit
- C. classify on the facts: the works restore performance, so expense them, and manage the covenant consequence separately and transparently with the lender ✅
- D. capitalise, and disclose the judgment in the notes so that users can adjust for it

*Rationale:* the classification follows the facts and the tax follows the classification, never the reverse (2.3.3). A and B both let a desired outcome choose the accounting, and B is the more seductive because its objective — cash — is the one this book otherwise privileges; the argument is still inverted. D bolts good disclosure practice onto an unsupportable treatment, and disclosure does not cure a classification the facts do not support. Where the capex/opex boundary falls for a particular project is a matter for the entity's finance function and auditors, and the deductibility a matter for qualified tax advisers in the relevant jurisdiction.


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


**2.4-G** `[2.4.2 · Evaluation]` A sponsor has one amendment's worth of negotiating capital left on a facility carrying a 1.20× `DSCR` covenant and a 2.00× interest-cover covenant whose numerator is not defined. On Kestrel's figures the `DSCR` test tolerates a 3.8796 % revenue fall and the interest-cover test 0.5000 %. The amendment worth pursuing is:

- A. reduce the `DSCR` covenant to 1.15×, the largest single ratio concession available
- B. address the interest-cover test — delete it, or define its numerator as `EBITDA`, which is worth 0.9524 of coverage — because it is the binding covenant and tolerates under a seventh of the revenue miss the `DSCR` test does ✅
- C. seek a longer cure period on both covenants, which protects the sponsor whichever test fails
- D. leave both thresholds and negotiate the margin instead, since covenant levels are conventional while pricing is not

*Rationale:* restated in revenue units the interest-cover test bites at 60,000 of headroom against `DSCR`'s 465,547 — a factor of **7.7591** — so the remaining capital belongs on the clause that will actually be breached, and an undefined numerator is the cheapest defect in the facility to fix (2.4.2). A concedes ground where none is needed: even at 1.15× the coverage test remains far slacker than the interest-cover test. C is genuinely useful and second-best — a cure period buys time after a breach rather than preventing one, and changes nothing about which test binds. D asserts a convention the arithmetic contradicts, and trades away protection on a covenant that can default the facility for basis points that cannot.


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


**3.1-I** `[3.1.2 · Evaluation]` Kestrel will receive a USD 500,000 connection rebate in five years from a creditworthy utility under a signed connection agreement. A board member proposes discounting it at the project's 8.0 % appraisal rate "for consistency"; the treasurer proposes 6.0 %, the rate on the project's own senior debt. The values are **340,291.60** at 8 % and **373,629.09** at 6 %. The sound recommendation is:

- A. use 8.0 % — one project, one discount rate, and consistency is the stronger discipline
- B. use a rate that reflects *this* cash flow's risk, which is a contracted receivable from a strong counterparty rather than the project's equity risk; state the rate chosen, and put any doubt about payment into the cash flow or an explicit credit adjustment rather than inside `r` ✅
- C. use 6.0 %, because the rebate will be applied to reduce senior debt, which makes the debt rate the opportunity cost
- D. use 8.0 % and additionally reduce the 500,000 for the risk of non-payment, capturing both effects

*Rationale:* the discount rate must match the risk of the flow being discounted, and discounting handles timing rather than credit (3.1.2) — the two hundred basis points between the proposals move this single receipt by **33,337.49**, or **9.3515 %** of its value at 7 %. A applies a portfolio convention to an instrument of different risk and so understates it. C is the most defensible of the wrong answers and still picks its rate from the *use* of the money rather than the risk of receiving it. D uses the right technique for credit — an explicit haircut — and then leaves in place a rate set for a riskier flow, charging the same risk twice.


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


**3.2-J** `[3.2.2 · Comprehension]` The closed form `B_k = A × AF(r, n − k)` gives Kestrel's principal outstanding after seven of twelve years as **21,102,406**. Which statement shows what the formula is, and is not?

- A. it is the total of the instalments still to be paid, so after year 7 Kestrel owes 25,048,176
- B. it is the present value of the payments that remain, which makes it a check on a schedule that has run exactly as contracted and simply wrong on one that has been restructured, swept or capitalised ✅
- C. it is the prepayment price, so a borrower settling early pays `B_k` and nothing further
- D. it equals the original principal less seven years of principal at the loan's average rate of retirement

*Rationale:* the formula discounts the remaining contractual payments, which is why it agrees with the schedule recursion to a few cents and why any departure from the contract breaks the formula rather than the schedule (3.2.2). A confuses cash still to be paid with principal outstanding: the difference, **3,945,770.13**, is interest not yet accrued and is not a liability. C is nearly right and materially wrong — `B_k` is the *base*, and break costs or prepayment fees sit on top of it. D describes level-principal retirement; an annuity's principal is back-loaded, which is why **50.2438 %** of the loan is still outstanding after 58.33 % of the term.


**3.2-K** `[3.2.3 · Analysis]` Two like-for-like offers on the same 42,000,000, 12-year, annuity-shaped, equally secured facility: 6.00 % with an 840,000 arrangement fee deducted from proceeds — an all-in cost of 6.3704 % — or 6.15 % with no fee. The treasurer recommends the first, "because the coupon is lower and the fee is a one-off". The best response is:

- A. agree: the 6.00 % coupon governs debt service, and the fee is a transaction cost outside the cost of funds
- B. take the 6.15 % facility, which is cheaper by **22.04** basis points — and put a number on the table, since the fee that would equalise the two is **343,244**, so the arranger is asking **496,756** more than the market-equivalent fee ✅
- C. take the 6.15 % facility, because an upfront fee is always a more expensive way to pay a lender than margin
- D. reject both and require the arranger to convert the fee to margin at 16.67 basis points, being the 2.0 % fee spread across the twelve-year tenor

*Rationale:* a fee is paid once and undiscounted while a margin is paid across a declining balance, so each 1.00 % of upfront fee costs **18.38** basis points of margin on this facility — making 2.0 % of fee worth about 37 and the 15-basis-point coupon difference the cheaper of the two (WE 3.2.3b). A treats the instalment as the cost of funds and ignores that the project receives only 41,160,000. C reaches the right verdict from an overstated rule: the exchange rate is computable, and a fee below 343,244 would make the 6.00 % facility the cheaper one. D is the "amortise the fee" error — dividing a fee by the tenor omits the time value of paying it at time zero, and 16.67 basis points is under half the 37.04 the stream actually gives.


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


**3.3-J** `[3.3.1 · Analysis]` A model audit finds that a project's real model values Kestrel's 25-year support stream at **55,006,446** while its nominal model values the same stream at **72,791,113**. The modeller proposes to "reconcile the two bases and present the pair as a range". The reviewer should:

- A. accept — a real and a nominal view of one project legitimately differ, and a range is the honest presentation
- B. reject the proposal and require the defect to be found: the two consistent treatments are arithmetically identical to the cent, so a **17,784,667** difference is an error, and this one carries the signature of real flows discounted at the nominal 9.0 % rate ✅
- C. accept the nominal figure and delete the real model, since covenants, tax and depreciation are nominal constructs
- D. average the two, document the choice as an assumption, and proceed

*Rationale:* the Fisher relation makes the consistent nominal and consistent real valuations equal, so a difference is not a basis to be reconciled but a defect to be located — and its size, **−24.4325 %**, identifies which defect it is (WE 3.3.1b). A dignifies an error as a perspective. C reaches a defensible destination by the wrong route: the model must indeed be nominal before any covenant or tax line can be computed, but deleting the real model conceals the error instead of fixing it, and the real model may well be the correct one. D averages a right number with a wrong one and calls the result an assumption.


**3.3-K** `[3.3.2 · Evaluation]` Kestrel's 3.0 % cap on the indexation of a 2,700,000 O&M base is worth **4,258,610** in present value at the sponsor's 4.0 % index forecast — a level annual equivalent of **398,941**. The contractor offers to remove the cap in exchange for cutting the base price by 500,000 a year. The soundest recommendation is:

- A. accept: 500,000 a year exceeds the cap's level annual equivalent by **101,059** a year, so the trade creates value
- B. refuse on these terms: the cap is an option on the index and its value is convex in the outturn, so a single-point valuation understates it — at a 5.0 % outturn the cap is worth **9,157,382**, a level annual equivalent of **857,852**, and the trade must be priced against a stressed index ✅
- C. refuse, because a project should never exchange a contractual protection for a price concession
- D. accept, provided the 500,000 reduction is itself indexed at 3.0 % so that the two legs escalate together

*Rationale:* the cap pays nothing at or below 3.0 % and more the further the index runs above it, so its expected value exceeds its value at the mean forecast, and 500,000 a year buys away protection worth 857,852 a year in precisely the state the protection exists for (WE 3.3.2b). A is the arithmetic correctly done at one point of a convex payoff — the commonest way an option is given away. C forgoes a class of trade that is frequently value-creating; the objection is to the price, not the principle. D improves the fixed leg — an indexed 500,000 is worth **7,150,991** against **5,337,388** level — and still leaves the payer short in the stressed states, so it changes the price without answering the objection.


**3.3-L** `[3.3.4 · Comprehension]` A facility accrues interest on an **actual/360** basis. Which statement restates what that convention does?

- A. it charges interest on actual days elapsed, so the cost depends on how many days each period happens to contain and averages out across a full year
- B. over a full year it charges `365/360` of the interest a 365-day basis would — a **1.3889 %** uplift on all interest, independent of the rate, the balance and the tenor, and equivalent on a 6.0 % facility to a quoted **6.0833 %** ✅
- C. it is the market standard for floating-rate lending, so it carries no cost relative to the quoted rate
- D. it lowers the effective cost, because dividing by 360 rather than 365 produces a smaller daily rate

*Rationale:* the denominator is short by five days while the numerator counts them, so the uplift is exactly `365/360` over a full year and does not average away (WE 3.3.4b) — **8.33** basis points at 6 %, **35,000** on Kestrel's 42,000,000 balance for one year and **274,947** across the twelve-year schedule. A describes the numerator effect, which does reverse between a short February and a 92-day quarter, and misses the denominator effect, which does not. C confuses prevalence with price. D inverts the arithmetic: a smaller denominator produces a *larger* daily rate.


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


**4.1-E** `[4.1.1 · Comprehension]` Why are interest and loan repayments excluded from the cash flows that a project discount rate is applied to?

- A. because lenders require their flows to be kept confidential
- B. because the cost of the financing is already represented inside the discount rate, so including it in the cash flows charges the project for its debt twice ✅
- C. because interest is not a cash flow
- D. because interest is a sunk cost

*Rationale:* The discount rate *is* the cost of capital, debt included; putting debt service in the numerator as well double-counts it and rejects viable projects with arithmetic that is individually correct in every cell (4.1.1). C is plainly false — interest is paid in cash. D confuses a cost that cannot be avoided by deciding no with one that recurs because of the decision. The mirror discipline: equity cash flow, after debt service, is discounted at the cost of equity instead.


**4.1-F** `[4.1.3 · Comprehension]` MIRR differs from IRR principally because MIRR:

- A. always produces a higher rate
- B. states the reinvestment rate explicitly instead of assuming interim cash earns the project's own return, and is single-valued even when cash-flow signs change more than once ✅
- C. discounts at the risk-free rate
- D. requires no forecast of cash flows

*Rationale:* Both properties belong to MIRR's construction (4.1.3). A is false in the usual direction — for a project whose IRR exceeds the reinvestment rate, MIRR is *lower*, which is the whole point. C names a rate MIRR does not use; D is true of no measure in this domain.


**4.1-G** `[4.1.3 · Evaluation]` A sponsor's paper reports IRR 12.19 %, MIRR 7.09 % on a 4 % treasury reinvestment rate, and NPV +12,937,747 at the 8 % project rate. A committee member moves to reject, on the grounds that MIRR is below the hurdle. The soundest response is that:

- A. the motion is correct — any return measure below the hurdle disqualifies the project
- B. MIRR is not a discounted value measure and cannot be read against a discount rate; the accept/reject test is the positive NPV at the owned rate, and MIRR's role here is to show how much of the 12.19 % was the reinvestment assumption ✅
- C. the motion is correct, because the treasury rate is the true opportunity cost
- D. MIRR should be removed from the paper to avoid confusing the committee

*Rationale:* MIRR is built from a terminal value, not a present value, so a hurdle comparison is a category error even though both are quoted as percentages (4.1.3). B also identifies MIRR's legitimate contribution — quantifying the reinvestment fiction — which is exactly why D is the wrong remedy: the answer to a misread disclosure is a better-explained disclosure, not less of it. C substitutes the reinvestment rate for the cost of capital, which are different quantities answering different questions.


**4.1-H** `[4.1.1 · Evaluation]` Two teams appraise competing plants. Team A reports NPV +16,179,360 on a level 15-year inflow discounted at year-end; Team B reports +19,167,914 on the same 8 % rate using the mid-period convention. The board must choose one plant. The professionally sound handling is to:

- A. prefer Team B's plant, which shows the higher NPV
- B. average the two conventions to be even-handed
- C. restate both appraisals on a single declared convention before comparing, and record the convention in the assumption register — the 2,988,553 gap here is a modelling choice, not a difference in value ✅
- D. prefer Team A's plant, because year-end discounting is conservative

*Rationale:* `(1 + r)^0.5` at 8 % is 1.0392305, so mid-period reporting adds about 3.9 % of present value to *any* project modelled that way; comparing across conventions ranks the modellers, not the plants (4.1.1c). A and D each let the convention decide. B produces a number that describes neither project and belongs to no convention — the appearance of fairness with none of the substance.


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


**4.2-E** `[4.2.2 · Comprehension]` A screening pack reports one project at "PI 1.27" and another at "PI 0.31". Before any comparison, the analyst must establish that:

- A. both indices use the same definition — gross `PV/I₀` (threshold 1.0) or net `NPV/I₀` (threshold 0.0) — because the two differ by exactly 1.000 ✅
- B. the second project has been rejected, since its index is below 1.0
- C. both projects have the same life
- D. the discount rate exceeds the IRR in both cases

*Rationale:* The two published forms differ by a constant, so a mixed table corrupts the accept threshold while leaving the ranking intact — which is why the defect survives review (4.2.2). B commits exactly that error: 0.31 on the net definition is a healthy project. C matters for EAV, not PI. D is unrelated to either definition.


**4.2-F** `[4.2.1 · Comprehension]` Payback and discounted payback are best described as measures of:

- A. value created by the project
- B. how long the invested capital remains at risk before it has been recovered — nominally in the first case, in present-value terms in the second ✅
- C. the project's return per unit of capital
- D. the project's sensitivity to the discount rate

*Rationale:* Both are exposure measures and neither is a value measure, which is why they screen rather than decide (4.2.1). A is NPV's job; C is PI's or IRR's; D is what the NPV profile shows.


**4.2-G** `[4.2.3 · Evaluation]` An asset manager must choose between a 3-year and a 5-year dosing system. EAC favours the 5-year system by 336,699 a year; the 15-year replacement chain favours it by 2,881,964 in present value; the two agree exactly. The plant, however, is on a concession with **seven years** left to run and no renewal right. The soundest position is that:

- A. choose the 5-year system — both methods agree, and agreement is the strongest evidence available
- B. choose the 3-year system, because it is cheaper to buy
- C. neither number answers the question asked: both price perpetual like-for-like replacement, and over a seven-year duty the relevant comparison is the actual cost of each option to concession end, including residual value or removal ✅
- D. average the two methods and choose the cheaper

*Rationale:* The exact agreement of EAC and the chain method is real but proves only internal consistency — they encode the *same* assumption, so agreeing cannot validate it (4.2.3c). A mistakes consistency for applicability. Over a seven-year duty the 3-year system implies two full cycles and one stranded year, the 5-year implies one cycle and two stranded years, and residual value decides; that is a different calculation, not a correction to these. D averages two answers to a question nobody asked.


**4.2-H** `[4.2.1 · Evaluation]` A sponsor's board holds a standing rule: reject anything with discounted payback beyond nine years. The Kestrel appraisal shows NPV +16,179,360, IRR 12.19 % and discounted payback 10.07 years. The professionally sound recommendation is to:

- A. reject the project, since the rule is clear
- B. recommend the project on its value, and put the rule itself to the board as the decision it actually is — a stated tolerance for exposure duration that here costs 16.18 million of value, with the exposure disclosed rather than hidden ✅
- C. reprofile the model until payback falls under nine years
- D. reject the project but note the NPV in an appendix

*Rationale:* Payback screens and NPV decides (4.2.1); a screening rule that vetoes a positive-NPV project is a policy choice about exposure, and the leader's obligation is to surface it with the value at stake attached, not to let a threshold decide silently. C is model manipulation to satisfy a rule — the dishonesty of MCQ 4.3-C's distractor D in a different costume. A and D both apply the rule as though it were an appraisal result; D adds the pretence of disclosure while burying the finding where it changes nothing.


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


**4.3-E** `[4.3.2 · Comprehension]` Why does ranking indivisible projects by descending profitability index sometimes fund a portfolio worth less than the best feasible one?

- A. because the index is computed at the wrong discount rate
- B. because the index maximises value per dollar *committed*, while the budget constraint asks for value per dollar *available* — so a high-index small project can consume capital that a larger project then cannot use, stranding a fragment ✅
- C. because the index ignores the discount rate entirely
- D. because indivisible projects always have lower NPVs

*Rationale:* The mechanism, not an arithmetic error: greedy selection is exact for divisible projects and only a heuristic for lumpy ones (4.3.2). A and C misdescribe the index, which is built from a present value at the project rate. D is simply untrue.


**4.3-F** `[4.3.3 · Comprehension]` In the two-way NPV table of 4.3.3, why must the analyst read the joint cells rather than the two single-assumption sensitivities?

- A. because joint cells are easier to compute
- B. because the assumptions are correlated in reality — the conditions that depress availability revenue also tend to raise the cost of capital — so the combined case is a coherent scenario rather than a remote corner ✅
- C. because single-assumption sensitivities are arithmetically invalid
- D. because the discount rate has no effect on NPV in isolation

*Rationale:* Individually the project survives a 20 % revenue shortfall and survives a 9 % rate; jointly it does not, and correlation makes that combination realistic rather than extreme (4.3.3). C is false — one-way sensitivities are valid, merely insufficient. D contradicts the table's own rows.


**4.3-G** `[4.3.2 · Evaluation]` A committee's rationing pack presents one funded set, ranked by PI, spending 18,000,000 of a 20,000,000 budget for NPV +4,580,000. Enumeration shows a feasible set spending 20,000,000 for +5,400,000. The soundest professional criticism of the pack is that:

- A. the discount rate must be wrong, since the sets disagree
- B. the pack presents a heuristic result as though it were the optimum, and omits both the unspent 2,000,000 and the runner-up set — so the board cannot see that its approval costs 820,000 ✅
- C. the pack should have used IRR instead of PI
- D. nothing is wrong: PI ranking is the accepted method

*Rationale:* The defect is disclosure, not arithmetic — every NPV in the pack may be right while the recommendation is still 820,000 short, and the two omitted facts are exactly the ones that would reveal it (4.3.2). A misattributes the gap to the rate. C swaps one scale-blind ratio for another. D treats convention as sufficient, which is the position the worked example refutes.


**4.3-H** `[4.3.3 · Evaluation]` A sponsor's appraisal shows Kestrel surviving a 21.24 % revenue shortfall at the board's rate. In tariff negotiation the offtaker asks for a 15 % reduction. The soundest use of that headroom figure is:

- A. concede up to 21.24 %, since the project remains value-positive throughout
- B. treat 21.24 % as the point at which the *investment case* fails, not as negotiating room — a 15 % concession leaves 6.24 points of headroom against every other adverse assumption combined, which the two-way table shows is not enough to absorb a rate rise as well ✅
- C. refuse any reduction, since the base case is the only defensible position
- D. concede 15 % and re-run the model to show a positive NPV afterwards

*Rationale:* Breakeven headroom is a buffer against *all* remaining uncertainty, not a budget to spend on one counterparty; consuming three-quarters of it in a negotiation leaves the project exposed to the correlated rate movement the table already prices (4.3.3). A spends the entire buffer. C mistakes a forecast for a position. D is B's arithmetic with the judgement removed — the model will indeed show a positive NPV, which is precisely why the number alone cannot settle it.


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


**5.1-E** `[5.1.2 · Comprehension]` A colleague who has only worked on projects after financial close asks why the development budget is called a premium rather than a cost. The statement that best conveys the idea is:

- A. it is money already spent, and therefore irrelevant to any forward-looking decision
- B. each stage buys the right, but not the obligation, to commit the next and larger tranche, so it is judged across everything pursued rather than against the deal that closed ✅
- C. it is part of the closed project's true capital cost and belongs in that project's capital expenditure
- D. it is the fee paid to a landowner for an option over a site, held until the project proceeds

*Rationale:* the word marks what the money buys — optionality across a portfolio in which the closes pay for the abandonments, which is why the honest unit is 7,400,000 per close and not 2,400,000. A states the sunk-cost rule, which governs whether to continue but says nothing about what the spend purchased; C is the separate and jurisdiction-sensitive question of capitalisation (5.1.1); D names one at-risk commitment inside the budget and mistakes the part for the whole.


**5.1-F** `[5.1.2 · Evaluation]` A development director defends next year's budget with the programme's own record: cost per closed project 7,400,000, an achieved close rate of 5.0 % against a breakeven close rate of 2.29 %, and therefore 2.19 times of margin. Value per close is the deal teams' appraisal `NPV` of 16,179,360. The soundest position to put to the board is that:

- A. the budget is justified as presented, because 2.19 times of margin is comfortable
- B. the ranking and the margin are the right instruments, but a breakeven computed on appraised value is itself a forecast, so it must be restated against **realised** value per close before it is relied on ✅
- C. the budget cannot be defended, because most of the spend is written off
- D. screening should be cut first, since it is the stage that funds the projects later abandoned

*Rationale:* treating a modelled value per close as an observed one is the judgement failure, and 5.1.2 requires the programme to be re-tested against realised value — a programme justified on optimistic deal `NPV`s has hidden its true breakeven. C mistakes the design of an option portfolio for waste: the closes are meant to pay for the abandonments. D attacks the 1,000,000 of screening, the smallest of the four stages and the one that buys the whole funnel, when the governance conclusion is to screen more widely and kill earlier.


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


**5.2-E** `[5.2.3 · Evaluation]` Kestrel's lenders ask that the several cost-overrun support rise from 10 % to 15 % of the 60,000,000 capital cost, taking group committed capital from 24,000,000 (40.0 % of capex) to 27,000,000 (45.0 %) and the industrial partner's commitment from 2,400,000 to 2,700,000 — an amount its own credit already requires to be backed by a letter of credit. The best recommendation to the sponsor board is:

- A. accept the 15 %, since the liability basis is unchanged and each sponsor still funds only its own share
- B. accept it only against a priced concession on coverage, tenor or margin, because the 3,000,000 is 600,000 a support point and is worth trading against the levers the lenders control ✅
- C. offer joint and several liability over the existing 6,000,000 pool instead, holding group committed capital at 40.0 % of capital cost
- D. refuse, since committed capital above 40 % of capital cost makes a limited-recourse structure uncommercial

*Rationale:* the request is a transfer of construction risk to the sponsors whose price is exactly knowable — 600,000 a point (5.2.3) — which makes it tradeable against the coverage, tenor, margin and sizing levers of Domain 10; conceding 3,000,000 unpriced is the failure. A is true, and is why the request reads as modest, but treating an unpriced 3,000,000 as costless is the error. C is genuinely defensible and some lenders prefer it, but it holds the pool flat by moving the partner's worst case from 2,400,000 to 7,800,000, a 3.25× rise on the least creditworthy member whose letter of credit is already a timetable risk, while the operator's rises only 20.5 %. D invents a threshold: 45.0 % is high and negotiable, not uncommercial.


**5.2-F** `[5.2.4 · Evaluation]` Kestrel's equity bridge at 5.5 % adds 1,512,225 of interest to project cost and lifts the sponsors' equity `IRR`, because their money goes in later. The finance lead is asked how to put it to the investment committee. The soundest presentation:

- A. leads with the improved equity `IRR`, since equity is what the committee is being asked to commit
- B. reports the 1,480,688 of saving as a financing return earned at the sponsors' own required return, states that the project return is unchanged, and discloses both the 1,512,225 added to project cost and the firm commitments the bridge depends on ✅
- C. omits the bridge from the return presentation altogether, because it creates no project value
- D. adds the 1,480,688 to the project `NPV` as a financing benefit

*Rationale:* the two funding profiles are identical in present value at the bridge rate — 17,530,806 either way — so the bridge is an arbitrage between that rate and the sponsors' 12 % requirement, and it is reported as what it is (5.2.4). A presents a change in timing as project performance, the flattery Domain 4 (KA 4.1.2) warns of. C suppresses a real cost that consumes coverage if it is capitalised. D books a financing return inside a project measure, which is the same error with a decimal point.


**5.2-G** `[5.2.2 · Comprehension]` An `SPV` is said to deliver both **credit separation** and **bankruptcy remoteness**. The two differ in that:

- A. they are one property, described from the lender's side and the sponsor's side
- B. credit separation means the project is assessed on its own contracts rather than on the weakest sponsor's rating, while bankruptcy remoteness is engineered by restrictions in the constitutional and finance documents and does not follow from incorporating a vehicle ✅
- C. credit separation is a legal state and bankruptcy remoteness is a rating outcome
- D. credit separation applies during construction and bankruptcy remoteness during operations

*Rationale:* 5.2.2 lists credit separation among the things the vehicle achieves and bankruptcy remoteness among the things it does **not** achieve by itself. A and C collapse a commercial assessment into a documentary construct, and it is the confusion behind the belief that a single-purpose entity is insolvency-proof; D invents a timing distinction the structure does not make.


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


**5.3-E** `[5.3.1 · Evaluation]` A gate paper reports: "The six conditions average 90.5 %, so the project is about ninety per cent likely to close. Expected value of continuing is +6,453,191 against a breakeven close probability of 14.83 %, and the remaining effort will be shared evenly across the six." Each of the four observations below is defensible. Which should the reviewer press first?

- A. the 90.5 %, which overstates the 54.72 % joint probability by 35.78 points
- B. the even effort split, since marginal gain ranks the financing condition 3.7255× above the land condition
- C. the expected-value verdict, because a 14.83 % breakeven makes "continue" the answer on almost anything, so the decision actually turns on whether any of the six conditions is fatal and unresolvable ✅
- D. the independence assumption, since these six conditions are positively correlated

*Rationale:* A and B are both right and both must be corrected, but neither can change the decision the paper asks for — A changes what is reported and B changes the work plan, and on either the arithmetic still says continue. Only C reaches the commitment of the remaining 2,400,000, which expected value cannot govern in a conjunctive test and which the fatal-condition rule exists to decide (5.1.3, 5.3.1). D points the wrong way: positive correlation raises the true joint probability above the product, so it strengthens the case for continuing rather than weakening it.


**5.3-F** `[5.3.4 · Comprehension]` A supplier describes its new membrane arrangement as "fully proven — three pilot units have run for two years". A colleague asks what more the word bankable could require. The best restatement is:

- A. bankable means technically demonstrated, so a two-year pilot record settles it
- B. bankable is a financing category rather than a technical description: it is the state in which lenders will lend against the technology's performance without recourse to the sponsors ✅
- C. bankable means the technology is the lowest lifecycle-cost option available
- D. bankable means the supplier has issued a performance guarantee

*Rationale:* the question the word answers is whose money is at risk if performance disappoints, which is why the test is operating references at comparable scale and duty, a supplier able to stand behind its guarantees, and an independent technical adviser's opinion the lenders accept (5.3.4). A is the supplier's sense of the word, and pilot scale is precisely what the financing definition excludes; C is the engineering question, and the domain's own caution is that bankable and good are not the same; D names one necessary component, and a guarantee is worth only the guarantor's balance sheet.


**5.3-G** `[5.3.4 · Evaluation]` The novel membrane variant would cut operating cost, but the credit committee requires 1.45 × rather than 1.30 × for want of operating references: debt capacity falls from 41,171,123 to 36,912,041, so 4,259,082 must be found as equity at an annual substitution cost of about 255,545. The engineering team argues the variant is the better plant. The soundest position is that:

- A. the variant should be adopted, because a 25-year lifecycle decision must not be made on a 12-year lender's view
- B. the variant should be rejected, because project finance is a poor place to innovate
- C. the coverage premium is one of several priced levers — an extended supplier guarantee, a larger maintenance reserve, output insurance or a first-loss contribution can each buy the ratio back down — so the decision is whether the operating saving plus any mitigant beats 4,259,082 of equity, not whether to defer to the committee ✅
- D. the target ratio should be negotiated back to 1.30 ×, since the underlying cash flow is unchanged

*Rationale:* 5.3.4 states both halves and neither settles it alone: bankable is not the same as good, so A's warning is real, and the arithmetic is real too. What makes C sounder is that it prices the choice and keeps the engineering decision with the engineers, which is exactly the negotiation Domain 9 (KA 9.3–9.4) supplies instruments for. A hands a financing constraint no answer; B is the maxim used as a substitute for the calculation; D misreads the ratio as an opinion rather than an output of the credit — the references are missing whatever the sponsors would prefer.


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


**5.4-E** `[5.4.2 · Evaluation]` Kestrel's daily economic cost of delay is 24,733.33 against delay damages of 20,000 per day capped at 4,800,000, so damages recover 80.86 % of the daily cost and the cap binds on day 240. Lenders stress a 360-day slip, in which the SPV bears 4,104,000 — 1,704,000 of daily shortfall plus 2,400,000 for the 120 days beyond the cap. The contractor will concede either the rate or the cap, not both. The better recommendation is:

- A. take the rate to 24,733.33, removing the 4,733.33 per day the calibration analysis identifies
- B. take the cap, because with the cap held at 4,800,000 a full-cost daily rate merely brings the cap forward from day 240 to day 194.07 and recovers not one dollar more in the 360-day case the lenders test ✅
- C. take neither and rely on the 6,000,000 of cost-overrun support, which covers the 4,104,000
- D. take the rate, since a damages cap is a matter for counsel and cannot be priced

*Rationale:* `4,800,000/24,733.33 = 194.07` days, so at 360 days recovery is the cap on either concession and the uncovered 4,104,000 does not move; the rate helps only for slips shorter than 194 days. A is the right calibration aimed at the wrong lever for the scenario the credit committee actually runs — a defensible answer that buys nothing in the case being tested. C is available and is part of what the support exists for, but it converts a contractor obligation into sponsor cash at par and consumes 68.4 % of a pool that must also absorb overruns (5.2.3). D is false: the cap is a number, and it has just been priced.


**5.4-F** `[5.4.4 · Evaluation]` COD falls due in 30 days. The readiness gate reports the revenue meter not yet accepted by the offtaker and the permit to operate outstanding. The commercial team wants to declare COD on the contractual date to start the 17,733.33 per day of `CFADS` the delay is costing. Holding COD for 30 days costs 742,000 of economic cost, of which delay damages recover 600,000, leaving 142,000 with the SPV. The better recommendation is:

- A. declare COD, since 532,000 of forgone `CFADS` over 30 days outweighs a 142,000 net holding cost
- B. hold COD until the readiness gate clears, because 142,000 is the entire price of holding while declaring transfers a contractor-owned, damages-covered problem to equity at the moment the covenant regime begins ✅
- C. declare COD and agree with the contractor that delay damages continue to accrue until the readiness items close
- D. hold COD and capitalise the 210,000 of construction interest into the facility, so equity funds nothing

*Rationale:* `30 × 24,733.33 = 742,000` against `30 × 20,000 = 600,000` leaves 142,000, while an unaccepted meter and a missing permit to operate put the tariff and the first covenant test at risk on a plant that is otherwise performing (5.4.4). A compares the revenue with a cost the contractor is largely paying, which is the arithmetic that makes early declaration look attractive. C is the incoherent option: damages run to COD, so declaring it extinguishes the recovery being relied on. D is a real structural choice and the weaker one — capitalising 210,000 lifts the instalment to 5,034,683.41, takes `DSCR` from 1.2743 to 1.2680 and cuts annual covenant headroom from 372,438 to 342,380, a loss of 30,058 a year for the whole twelve-year loan life, to avoid a one-off 142,000.


**5.4-G** `[5.4.3 · Evaluation]` Kestrel completes at 97 % of guaranteed output: `CFADS` falls from 6,384,000 to 6,112,200, `DSCR` to 1.2201, and a buy-down of 1,788,158 restores the originally sized 1.2743. The project director proposes reporting the position as restored. The soundest professional judgement is that:

- A. it is restored: sized coverage is back to 1.2743, so the project is as underwritten
- B. the buy-down restores the **lenders'** position only; equity has permanently lost 271,800 a year and holds a smaller loan, and the output shortfall at which the performance-damages cap is exhausted must be established and bankability tested there ✅
- C. the buy-down over-compensates, because debt falls by 1,788,158 while annual cash falls by only 271,800
- D. no buy-down is needed, because 1.2201 still clears the 1.20 × covenant

*Rationale:* a buy-down is calibrated to the lenders' coverage, not to the sponsors' return (5.4.3). C compares a capital sum with an annual flow: at constant coverage debt is proportional to `CFADS`, so 42,000,000 × 271,800/6,384,000 is exactly the adjustment that restores the ratio. D is the dangerous answer because it is arithmetically true and leaves the project 100,638 of annual cash from a breach, 73.0 % of its headroom consumed, with the cap question unasked.


**5.4-H** `[5.4.1 · Comprehension]` A sponsor asks what an EPC wrap changes, given that the same works are being built either way. The best statement of what it achieves is that it:

- A. reduces the cost of the works, because one contractor buys the packages more cheaply
- B. makes completion risk bankable by placing fixed-price, date-certain responsibility for the whole works with one contractor, so interface risk sits there rather than with the SPV — at the price of a wrap premium for risk the contractor does not wholly control ✅
- C. removes completion risk from the project entirely
- D. transfers completion risk to the lenders, who price it in the margin

*Rationale:* the wrap is read here only as what makes completion risk bankable — lenders face a single counterparty (5.4.1) — and it is not free: a contractor pricing interface and schedule risk charges for it, which is why a multi-package alternative must be compared on interface exposure and price together. A inverts the usual outcome, since splitting packages is what tends to price lower. C forgets that damages and guarantees are worth only the guarantor's balance sheet. D describes no part of a limited-recourse structure, in which lenders take completion risk only where support runs out.


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


**6.1-D** `[6.1.1 · Comprehension]` A modeller adds a subtotal formula to a summary page and defends it: "it is only a sum". The statement that best conveys why the three-block rule forbids it is:

- A. spreadsheets round subtotals differently from the cells beneath them
- B. an output block that computes anything can disagree with the engine behind it, and a summary page that no longer ties to the model is the most damaging class of audit finding ✅
- C. it enlarges the file and slows recalculation
- D. subtotals belong in the inputs block, where they can be changed in one place

*Rationale:* the separation exists so that each block can be read for one kind of defect — inputs for wrong assumptions, calculations for embedded constants, outputs for nothing at all, because they compute nothing (6.1.1). A invents a rounding problem; C is a performance claim, not the principle; D inverts the architecture, since the inputs block holds no formulae.


**6.1-E** `[6.1.3 · Evaluation]` A board paper reports "`NPV` +16,179,360, `IRR` 12.19 %" with no labels. The financing model shows +2,767,684 post-tax unlevered over 25 years on the flat case, and an unlevered post-tax `IRR` of 8.54 % against an 8 % hurdle. All four corrections below are legitimate. Which most changes the decision the board is being asked to take?

- A. attach basis, horizon and case to both figures, as the standing rule requires
- B. state that basis alone — cash tax and working capital — costs 17,221,195 of present value over the fifteen-year horizon
- C. put the twenty-five-year post-tax flat case in front of them — +2,767,684, and an asset return of 8.54 % against an 8 % hurdle — because that is the basis on which the decision is close ✅
- D. disclose the full defensible spread of 29,545,516, from −9,670,265 to +19,875,251

*Rationale:* A is the standing rule and its breach is why the defect arose, but a *labelled* +16,179,360 still tells the board a comfortable story; B is one of the three components of the gap and leaves horizon and case unaddressed; D discloses without recommending and invites a board to choose its own number from a range. Only C changes what the board is deciding, by showing the basis on which the asset barely clears its hurdle (6.1.3, 6.3.3) — after which the bridge, the labels and the spread are all supporting material.


**6.1-F** `[6.1.2 · Evaluation]` An assistant is asked to build Kestrel's construction model and, unprompted, chooses an annual timeline with interest on the opening balance. Capitalised interest comes out at 1,247,352 against the quarterly opening-balance figure of 2,114,597 — an understatement of 867,245, or 41.01 %. The modeller observes that every check in the model passes. The soundest position is that:

- A. the choice is acceptable, because both conventions are defensible and the checks pass
- B. periodicity and the interest accrual base are economic choices with a named owner, must be stated in the conventions sheet, and here have understated the depreciable base by 867,245 as well as the interest — a defect no check in the model can see ✅
- C. the annual timeline is wrong and a quarterly one is always required
- D. the difference is immaterial at 3.52 % of the envelope

*Rationale:* an assistant must not choose the model's conventions, and this is the choice it makes (6.1.2, and the AI boundary in KA 6.1): the 41 % error needs the coarse timeline *and* the opening-balance base together, since an annual model on average balances lands within 367,022 of the quarterly answer. B is sounder than C because the pairing, not the period length, is the defect — and the understatement propagates, cutting annual depreciation by 34,690 and the present value of the tax it shelters by 74,061. A is the reasoning that lets a convention error survive review; D misquotes a share of the envelope as a measure of a 41 % error in one line.


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


**6.2-E** `[6.2.1 · Evaluation]` A comparable water project is modelled exactly as Kestrel was, inside a fixed 60,000,000 envelope, but with higher committed uses; the balancing contingency solves to 1,152,000 — **2.4 %** of its 48,000,000 EPC price, against Kestrel's 3,645,403 at 7.59 %. Sources equal uses to the cent. The soundest recommendation is:

- A. accept the table, since sources equal uses and the identity is satisfied
- B. report that a 60,000,000 envelope does not fund this project on a defensible contingency — a financing conversation about the envelope, not an adjustment to the table ✅
- C. hold contingency at 7.59 % of the EPC price and let capitalised interest become the balancing line instead
- D. present the contingency as 1,200,000, a cleaner figure, with the approximation noted

*Rationale:* the identity is satisfied by construction and therefore proves nothing; what makes the balancing line informative is testing it against policy, and 2.4 % sits below the band a lender would expect for this technology and contract structure (6.2.1). A treats an identity as a check. C is the defensible-looking alternative and is the more dangerous answer: capitalised interest is *computed* from the drawdown profile, the rate and the interest convention, so making it the plug converts a derived quantity into an assumption and buries the funding gap in the one line nobody re-derives. D destroys the single tell that catches a plugged table, since a balancing line is never a round number.


**6.2-F** `[6.2.2 · Evaluation]` A sponsor's investment-committee paper shows first-operating-year distributions struck as `CFADS` less debt service — 1,374,365 — because the DSRA is described in the paper as "a balance-sheet item". Modelled through the waterfall the distribution is 121,956, or 0.68 % of the 18,000,000 contributed, after 1,252,409 of reserve funding. The soundest position is that:

- A. the paper is acceptable, since the reserve is indeed on the balance sheet and is repaid at maturity
- B. the paper overstates the first distribution by an order of magnitude: reserve funding ranks above distributions, so a distribution forecast struck before it is an aspiration, and the 1,252,409 held in the DSRA is restricted cash unavailable to the business ✅
- C. the paper is acceptable if a footnote records the reserve requirement
- D. the paper is wrong because it ignores the lock-up test, which is the operative constraint at this coverage

*Rationale:* early cash is all the equity has, and the reserve is a claim on it ranking above distributions (6.2.2, 6.3.2). A and C treat a ranking in the waterfall as a disclosure matter. D names a real test that does not bite here — at a `DSCR` of 1.2743 the 1.15 × lock-up is not engaged, so the paper's error is the omitted reserve, not the omitted test; picking the wrong reason is how a correct objection gets dismissed.


**6.2-G** `[6.2.3 · Comprehension]` Kestrel's accounting depreciation is 2,400,000 a year and its year-one cash tax is 516,000. Accounting depreciation, tax depreciation and cash tax are best described as:

- A. three names for one charge, presented on different bases
- B. three different numbers — an allocation of cost under the reporting framework, a deduction on the tax authority's own base and profile, and the amount actually paid — of which only the last enters `CFADS` ✅
- C. three deductions that all enter `CFADS`, at different points in the waterfall
- D. two accounting measures and one forecast, so only the accounting figures are auditable

*Rationale:* only cash actually paid reduces cash available for debt service, which is why cash tax and accounting tax are separate rows even in years when they are equal (6.2.3). A is the conflation Domain 2 (KA 2.A.1) names as the standard finding; C puts non-cash charges inside a cash measure; D invents an audit distinction — every one of the three is evidenced, and the tax line traces to a dated written opinion.


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


**6.3-E** `[6.3.3 · Comprehension]` An investment committee member asks why one project has two reported returns, and which of them is the real one. The best restatement is:

- A. the project return is struck before tax and the equity return after it
- B. they answer different questions — the project return values the asset before financing, the equity return values the sponsors' position after every prior claim in the waterfall — so both are real and neither substitutes for the other ✅
- C. the equity return is the project return plus the debt margin
- D. the project return is the lenders' return and the equity return the sponsors'

*Rationale:* the distinction is *whose* cash is being measured and after which claims, not a basis difference: on Kestrel both figures are post-tax, and the 8.54 % and 9.83 % differ because leverage reorders the cash (6.3.3). A names a real labelling axis (6.1.3) that is not this one; C invents an arithmetic relationship, when the gap depends on gearing, tenor and the shape of distributions; D miscasts the project return, which measures the asset and not any lender, whose return is its margin.


**6.3-F** `[6.3.1 · Evaluation]` A construction model charges interest on the average debt balance and resolves the resulting circularity by iteration, with no stated convergence criterion and no named owner. On Kestrel that convention is worth 2,427,554 of capitalised interest against 2,114,597 on the opening-balance convention — a difference of 312,957. The reviewer's best recommendation is:

- A. switch to opening-balance interest, which removes the circularity outright and is the convention the master model uses
- B. keep the average-balance convention but require the resolution to be documented, with a tested convergence criterion and a named owner, because the defect is the undocumented resolution rather than the convention ✅
- C. solve the interest algebraically instead, since a deterministic solution is always preferable
- D. correct the 312,957, which is an overstatement of interest

*Rationale:* all three resolutions in 6.3.1 are honest, and average-balance interest is the more accurate measure of what the facility will actually charge; what is unacceptable is a resolution whose convergence nobody has tested, because the printed answer then depends on the order in which somebody pressed the keys. A is genuinely defensible and would restore reproducibility, but it buys it by surrendering 312,957 of correctly measured interest when documentation delivers both. C is also defensible and is the better answer for simple structures, but it is a rebuild rather than a control and is not available for a sculpted or swept schedule. D is wrong: 312,957 is the priced cost of a convention, not an error.


**6.3-G** `[6.3.3 · Evaluation]` A board paper presents Kestrel's equity return as "5.028 times money, 13.52 % `IRR`". The bank case returns 9.83 %, and the whole 3.69-point difference comes from a 2.967 % escalation assumption; the multiple covers the whole twenty-five-year concession. The soundest presentation:

- A. leads with the 5.028 times multiple, because it counts every dollar actually distributed
- B. reports the sponsor case, since it is the sponsors' central expectation and they are the investor
- C. reports both cases with their labels, states the horizon the multiple covers, and states whether the 2.967 % escalation is contracted, indexed to a published index or merely assumed ✅
- D. reports the bank case alone, because that is the case the lenders underwrite

*Rationale:* a multiple has no time dimension, so 5.028 times says nothing about when the cash arrives and a multiple shown without its horizon is a long horizon dressed as a return, while an equity `IRR` without its case is worthless (6.3.3). A does exactly that; B presents the more flattering case unlabelled, when the habit the domain prescribes is to state the escalation at which the return meets the hurdle and ask whether it is a right or a hope; D discards the sponsors' own economics, which is the case the equity decision turns on.


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


**6.4-F** `[6.4.1 · Evaluation]` On the bank case Kestrel's `DSCR` falls from 1.2743 to 1.1851 in year twelve, averaging 1.2340 against a 1.20× covenant, on the requested 42,000,000. The credit committee's sizing of 41,171,123 gives a year-twelve `DSCR` of 1.2087, and 41,472,081 is the largest facility whose year-twelve `DSCR` is exactly 1.20×. The soundest recommendation is:

- A. keep 42,000,000, disclose the year-twelve minimum, and rely on the 1.2340 average
- B. size at 41,472,081 — the largest facility that holds the covenant in every period — and record that the 828,877 the committee withheld was already protecting a year-twelve exposure nobody in that negotiation had modelled ✅
- C. size at 41,171,123 as the committee proposed, since 1.2087 clears the covenant with margin
- D. keep 42,000,000 and negotiate the covenant down to 1.15×, matching the lock-up

*Rationale:* covenants are tested in periods, so the constraint is the minimum and the binding period is year twelve; the correct facility is therefore the largest that satisfies it everywhere (6.4.1, 6.4.1b). A relies on an average that passes while the project breaches. C is defensible and safe, and it is the answer the committee will accept — but it was sized on year-one coverage and clears year twelve by accident, forgoing 300,958 of debt capacity for coverage the covenant does not ask for. D looks like an equivalent trade and is the weakest option: collapsing the covenant onto the 1.15 × lock-up removes the tier between a distribution trap and an event of default, which is the early warning the whole structure depends on.


**6.4-G** `[6.4.4 · Evaluation]` On the domain's parameters an AI pre-check is worth 498,481, a late model audit 317,547, an early audit 602,744, and the pre-check with an early audit 670,716. A sponsor proposes replacing the audit with the pre-check, since the pre-check ranks above the late audit on expected value. The soundest position is that:

- A. the substitution is right: 498,481 exceeds 317,547, so the cheaper control is the better one
- B. it should be refused, and both funded — the audit brought earlier and the pre-check added — because the audit is the lenders' condition precedent and the scanner's detection rate is not independent of the error classes that carry the largest cost ✅
- C. both should be refused, because a measured material-error rate below the 20.10 % breakeven means neither pays
- D. the substitution is right provided the pre-check's detection rate is first validated on this asset class

*Rationale:* machine checks are additive to review and never substitutive (6.4.4): a scanner finds structural defects and systematically misses the definitional ones — whether `CFADS` matches the facility's clause, whether the tax treatment is the jurisdiction's, whether the waterfall follows the drafted priority — which are precisely the errors with the largest cost, so treating detection as constant across error classes biases the comparison toward the cheap control. A does that. C ignores that the audit is a condition the sponsor cannot trade away, and that the tail of the cost distribution is averaged out of the expectation. D fixes the parameter and leaves the governance objection untouched.


**6.4-H** `[6.4.2 · Comprehension]` Sensitivity analysis and scenario analysis differ in that:

- A. sensitivity moves inputs in percentages while a scenario moves them in absolute amounts
- B. sensitivity is reported on value and a scenario on coverage
- C. sensitivity moves one input at a time to measure the model's response, while a scenario moves a coherent set of inputs together to represent a state of the world — so only the second can carry a view about correlation ✅
- D. they are the same procedure, a scenario being a sensitivity reported on more than one output

*Rationale:* 6.4.2 defines both, and identifies correlation and threshold behaviour — not the arithmetic of combining moves — as what one-at-a-time analysis cannot see. A and D describe presentation rather than construction. B assigns outputs to techniques, when the domain's insistence is the opposite: both must be run on coverage as well as value, because a table reporting only `NPV` ranks interest-rate risk last on Kestrel.


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


**7.1-E** `[7.1.1 · Evaluation]` Kestrel's offtaker will take volume risk under Structure A, but only for a capacity charge of **11,600,000** instead of 12,000,000. `CFADS` becomes 6,084,000, `DSCR` 1.2145, and debt capacity at 1.30 × becomes **39,236,390** — against 41,171,123 at the full charge and 30,491,396 under the volume tariff sized on its low case. The 400,000 a year has a present value of 4,269,910 over the 25-year concession at 8 %. The soundest recommendation is:

- A. reject the reduction: 4,269,910 of present value is surrendered permanently for a one-off financing benefit
- B. accept it: the offtaker's price for taking volume risk is 4,269,910 of concession-life value against 8,744,994 of debt capacity released, so it is well below what the risk transfer is worth ✅
- C. accept it, because an availability structure is always more bankable than a volume tariff
- D. reject it and negotiate the 1.30 × target down instead, since the ratio is the variable that matters

*Rationale:* this is the question KA 7.1.1 says is the professional act — is the offtaker's price for volume risk less than the equity the transfer releases? — and here it is, by a factor of more than two, with 1.2145 still clear of the covenant. A performs half the calculation: the capacity it buys is not a one-off benefit but equity permanently not subscribed. C is the unsupported generality, and 7.1.3 shows what the structure actually does — converts demand risk into operational risk, with the covenant breaking at 92.09 % availability. D inverts the domain's central finding: the case moves as well as the ratio, and the case is the larger of the two variables.


**7.1-F** `[7.1.3 · Evaluation]` A contract summary renders the deduction clause — the capacity charge multiplied by the availability shortfall and by a 1.5 × liquidated-damage multiplier — as a pro-rata reduction of the charge, and the modeller builds it that way. The model balances and every check in it passes. The soundest professional position is that:

- A. the difference is immaterial while the model is internally consistent and its checks pass
- B. one period's revenue must be recomputed from the clause text by hand, at full and at reduced availability, before any coverage ratio derived from it is quoted: the paraphrase cuts the slope of `CFADS` against availability from 12,780,000 to 8,280,000 — 35.21 % — and moves the covenant breakpoint from 92.086 % to 90.502 % ✅
- C. the deduction schedule belongs to the technical adviser, so the commercial team should work from the summary it was given
- D. the target `DSCR` should be raised to restore the coverage the paraphrase removed

*Rationale:* the slope is `9,000,000 × multiplier − 720,000`, so a multiplier read as 1.0 turns 127,800 of `CFADS` a point into 82,800, and **no downstream check catches it** because the model is internally consistent and wrong (7.1.3). A is precisely the reasoning that lets the error survive review. C misallocates a first-order commercial term to a technical annexe. D prices a definitional error as though it were a risk, which buries it inside a ratio instead of correcting it.


**7.1-G** `[7.1.2 · Comprehension]` A take-or-pay minimum and an availability payment differ in what each leaves with the project:

- A. they are equivalent, both being contracted revenue from a creditworthy payer
- B. a take-or-pay floor covers volume below a contracted minimum — and is frequently a make-up right, so the payer may take the volume later at no further charge — while an availability payment removes volume risk altogether and substitutes an obligation to be available ✅
- C. take-or-pay transfers operating risk to the payer, while an availability payment retains it
- D. take-or-pay protects against price and an availability payment against cost

*Rationale:* a floor is worth what the party writing it is good for and often protects cash timing far less than a summary suggests, while an availability structure converts demand risk into operational risk the project must then genuinely manage (7.1.2, 7.1.3). A ignores that the two leave different risks behind. C reverses the availability mechanism, whose whole point is that the project answers for availability. D names risks neither instrument addresses.


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


**7.2-E** `[7.2.3 · Comprehension]` A sponsor complains that its lenders "refuse to count our sales pipeline". The statement that best conveys what a run-off case is:

- A. lenders assume the business stops selling, which is conservatism for its own sake
- B. it values only revenue already under contract, declining at the retention rate, so debt is sized on promises that exist rather than on sales not yet made ✅
- C. it is the sponsor case with a standard haircut applied to forecast new sales
- D. it assumes every customer leaves at the end of its current term

*Rationale:* at 93 % net revenue retention the run-off case takes 24,000,000 of `ARR` to 22,320,000 in the following year — a statement about what is signed, not a prediction that selling stops (7.2.3). A reads a sizing basis as a forecast; C describes a haircut to a sponsor case, which is a different and weaker discipline because it still lends against unsigned revenue; D describes nil retention rather than 93 %.


**7.2-F** `[7.2.1 · Evaluation]` A sponsor team proposes to present its central forecast as the base case and the same forecast with a flat 10 % haircut as "the bank case". On Kestrel the escalation assumption alone separates the sponsor and bank cases by 17,107,567 of `NPV`. The soundest position is that:

- A. the approach is acceptable, since a 10 % haircut is a conservative adjustment in the sponsors' own favour to concede
- B. the bank case is a negotiating position built from specified haircuts, a slower ramp, no unindexed growth and a conservative price deck, and a haircut with no stated composition cannot be defended line by line — conceding the case unopposed concedes more than the coverage ratio does ✅
- C. the work is unnecessary, because the lenders' market adviser will produce the bank case
- D. only the sponsor case should be presented, leaving the lenders to make their own adjustments

*Rationale:* the case moves as well as the ratio, and it is the larger of the two sizing variables (7.1.1, 7.2.1); a bank case is also a different object from a downside case, with a different purpose and owner. A dresses an arbitrary percentage as conservatism and cannot survive a question about which line it applies to. C and D hand the case to the counterparty that gains from choosing it, which is how sponsors arrive in credit committee with no position of their own.


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


**7.3-E** `[7.3.1 · Comprehension]` A colleague asks what the "indexed share" of a tariff is and why it is recorded separately from the index. The best explanation is:

- A. it is the proportion by which the tariff rises each year
- B. it is the proportion of the tariff's value that escalates; the remainder is a fixed nominal amount eroding in real terms, so the tariff's effective compound rate is lower than its headline index ✅
- C. it is the share of the tariff denominated in the indexed currency
- D. it is the ceiling on annual escalation the contract permits

*Rationale:* Kestrel indexes 80 % of its tariff to an index assumed at 2.5 %, which compounds to an **effective** 2.101 % over 24 years rather than 2.5 % (7.3.1). A describes the index rate itself; C invents a currency mechanic; D names a different negotiated parameter — a cap limits the index in a period, while the indexed share limits the base the index applies to.


**7.3-F** `[7.3.1 · Evaluation]` Kestrel's O&M contractor offers a swap: it will index the currently fixed 30 % of the cost base at the same 3.2 %, and in exchange the offtaker will index 100 % of the tariff at 2.5 % rather than 80 %. On the assumed rates the year-25 `EBITDA` margin moves from 59.23 % to **55.85 %** — a loss of 3.38 points — while the present value of `EBITDA` over the 25-year concession at 8 % **rises by 1,516,002**, full tariff indexation adding 6,204,143 and full cost indexation removing 4,688,141. The soundest recommendation is:

- A. reject: the fixed 30 % of the cost base saves 7.7164 margin points by year 25, more than twice the 3.6462 that full tariff indexation adds
- B. accept: 1,516,002 of present value at 8 % over the concession is a real gain, and margin points at a single year are not the decision metric
- C. accept, but only against a cap on the O&M index, because the 1,516,002 holds only at the assumed 3.2 % and the swap exchanges a fixed obligation for an unbounded indexed one ✅
- D. accept, and seek a matching cap on the tariff index so that both sides are symmetrical

*Rationale:* the two metrics genuinely disagree — the year-25 margin falls while discounted value rises, because the cost escalation compounds on a smaller base and lands later, where discounting bites — and B is right to prefer the discounted figure. What B misses, and C supplies, is that 3.2 % is a *forecast* while indexation is a *term* (7.3.1): the tariff's gain is bounded by its index, the cost's loss is not, so the swap is sound conditioned on a cap and speculative without one. A applies the correct decomposition to the wrong horizon, comparing snapshot margin points instead of value over the concession. D concedes the one thing worth keeping, capping the escalation the project receives in order to cap the escalation it pays.


**7.3-G** `[7.3.2 · Evaluation]` A credit paper presents two projects as equivalent credits: both report a base `DSCR` of 1.30 × against a 1.20 × covenant. Project M's `CFADS` is 80 % of revenue and Project N's is 40 %, so the same 7.6923 % `CFADS` fall is reached by a 6.15 % demand fall in M and a 3.08 % fall in N. The soundest position is that:

- A. they are equivalent for credit purposes, since coverage and covenant are identical
- B. cost structure must be reported beside the ratio, and high operating leverage matched by structure — lower gearing, a larger reserve, a floor mechanism or sculpting — rather than treated as a defect ✅
- C. Project N is simply the weaker credit and should be declined
- D. Project N's covenant should be raised to 1.30 × so that the two tolerances are equalised

*Rationale:* two projects with the same ratio can have demand tolerances differing fourfold, and the ratio does not show it (7.3.2) — which is why cost structure belongs in a revenue-risk discussion. A is the error the generalised table exists to correct. C treats leverage as a fault, when it is what makes infrastructure profitable while demand holds; the requirement is that it be known, disclosed and matched. D moves the wrong lever: raising the covenant reduces N's tolerance further, since it shortens the distance from base coverage to the trigger.


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


**7.4-E** `[7.4.1 · Evaluation]` Kestrel's offtaker exposure can be measured three defensible ways: the receivable of **2,958,904** on 90-day terms, the present value of contracted `CFADS` over the twelve loan years at 6 % (**53,522,460**), or the present value over the whole 25-year concession, which is larger again. The credit committee is deciding whether to advance 41,171,123. The exposure it should be shown is:

- A. the receivable, since that is the amount actually owed at any moment
- B. 53,522,460, because the loss event for this decision is the loss of the contracted stream over the period being lent against, not an unpaid invoice ✅
- C. the whole-concession figure, being the largest and therefore the most prudent
- D. all three, averaged, so that the committee neither over- nor understates

*Rationale:* `EAD` follows from what the loss event is, and choosing it is a judgment tied to a decision (7.4.1). A is the working-capital exposure, trivial by comparison, and quoting it is exactly how a concentration problem gets presented as a working-capital matter. C is genuinely defensible and is the right number for a *different* decision — the sponsors' own exposure across the concession — but selecting it here because it is the largest substitutes an instinct for prudence for the question asked, and it overstates what the lenders are relying on. D is arithmetic without meaning: three exposures answer three questions and their mean answers none.


**7.4-F** `[7.4.3 · Evaluation]` The stress matrix clears the 1.20 × covenant in only three of sixteen cells and falls below 1.00 × in five, and the unstressed bank case already reaches a year-twelve minimum of 1.1851 (Domain 6, Fig 6.4.1). Four requirements are proposed. Which should the committee impose first?

- A. extend the matrix to the loan's minimum year, since every cell is a year-one snapshot and therefore optimistic about the loan's worst year ✅
- B. widen the tariff and despatch ranges beyond the market adviser's credible bounds
- C. attach a probability to each cell so that the committee can weigh the outcomes
- D. increase the debt-service reserve from six months to twelve, because five cells cannot pay debt service from operating cash

*Rationale:* the matrix understates the problem before any stress is applied: coverage already falls to 1.1851 by year twelve on the unstressed bank case, so thirteen failing cells is a floor rather than a finding (7.4.3). D is a real mitigant applied in the wrong order — a reserve sized against a mis-stated worst year is sized against the wrong number, and the right sequence is measure, then mitigate. C is what committees usually ask for and is the more dangerous request: joint probabilities here would have to be invented, and a probability-weighted matrix built on assumed correlations implies knowledge nobody has (Domain 6, KA 6.A.1). B changes nothing, since the ranges are already the adviser's credible bounds and extending them past that only adds cells nobody will underwrite.


**7.4-G** `[7.4.2 · Evaluation]` A credit paper records that the sole offtaker is investment grade, with an annual `PD` of 0.60 %, a twelve-year cumulative `PD` of 6.9671 % and an expected loss of 1,678,031 on exposure of 53,522,460. A single regional authority is the only buyer of the water. The soundest assessment of the paper is that it:

- A. is adequate: expected loss is the standard measure and the counterparty's grade is strong
- B. is inadequate because a 0.60 % annual `PD` is implausibly low for a sub-sovereign counterparty
- C. is incomplete: expected loss is identical whether one counterparty or four carry the same revenue, so the paper must also state the probability of losing more revenue than the structure can survive and what has been structured against it ✅
- D. is inadequate, and the remedy is to split the offtake among four independent payers

*Rationale:* expected loss is linear in exposure and therefore blind to concentration — four payers would show the same 1,678,031 while the probability of losing half or more of the revenue falls from 6.9671 % to 2.6489 % and of losing all of it to 0.0024 % (7.4.2). A lets the expectation stand alone on a concentrated base. B attacks the one parameter the paper at least sources, and misses the omission. D prescribes a remedy this market does not offer: where concentration cannot be diversified away it must be structured against — guarantees, letters of credit, escrow, step-in rights, termination compensation that repays debt, or lower gearing.


**7.4-H** `[7.4.1 · Comprehension]` Kestrel's `LGD` of 45 % rests on an expectation of recovering 55 % of contracted value. What that figure embeds is:

- A. the proportion of an unpaid invoice that is written off
- B. a market judgment — that a merchant market exists to re-sell into — together with a legal one, that termination compensation, step-in rights and the security package work as drafted ✅
- C. a figure fixed for each counterparty class by regulation, and therefore not an assumption at all
- D. the probability that recovery efforts fail

*Rationale:* `LGD` is the proportion of exposure not recovered, and on a project the recovery depends on both a market and a set of documents, so an `LGD` not traced to them is a guess wearing a decimal point (7.4.1). A describes a receivable write-off, not the loss of a contracted stream. C states as universal a treatment that is institution- and jurisdiction-specific. D is `PD`'s territory — likelihood — not the severity `LGD` measures.


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


**8.1-E** `[8.1.2 · Comprehension]` A sponsor asks how two estimates of the same plant, both totalling 48,000,000, can carry defensible contingencies differing by a factor of 6.25. The best explanation is:

- A. one estimator is more conservative than the other
- B. contingency provides for the range that the estimate's own definitional maturity implies, and that range narrows from −30 %/+50 % at screening to −5 %/+8 % once scope is contracted ✅
- C. contingency is a policy percentage, so the difference reflects two companies' policies
- D. the two estimates must be stated at different base dates

*Rationale:* the class records how well scope was defined when the estimate was priced, and the provision to the upper bound on the same 48,000,000 base moves from 24,000,000 to 3,840,000 (8.1.2). A treats a structural property as a personal trait; C is the belief the whole KA exists to dislodge, since a percentage that names no class states nothing; D names a real and separate parameter (8.2.3) that governs escalation exposure rather than accuracy.


**8.1-F** `[8.1.3 · Evaluation]` Kestrel's year-seven membrane replacement costs 4,098,909 in nominal terms, and without a reserve the year-seven `DSCR` is 0.4561 — a payment default. The level annual charge equivalent to the whole lifecycle programme is 644,606. The sponsors accept that a reserve is required and ask what to deposit. The soundest recommendation is:

- A. deposit the level annual charge of 644,606 from year one, since that is the economically equivalent annual cost
- B. deposit 683,152 a year over the six years before the replacement, because the account must hold 4,098,909 before year seven and the level charge is an equivalence, not a deposit plan ✅
- C. deposit nothing and fund the replacement from a standby facility drawn in year seven
- D. defer the replacement to year eight, when the outstanding debt balance is lower

*Rationale:* `4,098,909/6 = 683,151.48` must be in the account before the money is spent, with no credit taken for interest on the balance; six deposits of 644,606 total 3,867,636 and leave the account **231,273** short in the one year it is needed (8.1.3). A is the right economic measure used as a funding plan — precisely the confusion the worked example warns against. C is genuinely available and is the weaker structure: it converts a funding certainty into a drawing risk in the year the project would otherwise be at 0.4561, which is the year a lender is least willing to be relied on. D subordinates a maintenance requirement to a credit calendar and does not remove the cliff — it moves it and escalates it to 4,246,470.


**8.1-G** `[8.1.2 · Evaluation]` A team declines the fixed-price wrap because six separate packages priced lower, and proposes retaining the funded contingency of 3,645,403 — 7.59 % of the 48,000,000 base, which covers 94.93 % of a Stage E band and 25.32 % of a Stage C one. The soundest position is that:

- A. the provision should stand: the packages priced lower, so the expected outturn is lower
- B. the provision should stand, and any difference should fall to the sponsors' cost-overrun support if it is needed
- C. declining the wrap returns the base-estimate uncertainty to the owner, so the provision must be resized on the range — 14,400,000 at the estimate's upper bound, 6,512,795 at P80 — and if the envelope cannot fund that, the envelope is wrong rather than the table ✅
- D. the provision should be raised to the 10 % rule, 4,800,000, as a policy compromise

*Rationale:* the contracting strategy and the contingency line are one decision taken twice, usually by different people in different months (8.1.2); 7.59 % is a Stage E number and only a wrap earns it. A confuses the lowest tendered price with the lowest outturn — the interfaces the packages exclude are now the owner's. B converts funded provision into a contingent commitment worth less than cash (8.3.1) and does so without resizing anything. D swaps one unreasoned percentage for another, and 4,800,000 is still a third of the range-based upper bound.


**8.1-H** `[8.1.1 · Comprehension]` Operating cost never appears in the sources-and-uses statement. The reason is that it:

- A. is not a cost of the project, being incurred by the operator rather than the SPV
- B. is a recurring deduction inside `CFADS`, so it reduces coverage every period rather than creating a funding requirement before revenue exists ✅
- C. is met from the maintenance reserve, which is funded separately
- D. is funded by equity, which is why it sits outside the facility

*Rationale:* the sources-and-uses statement funds what must be paid before the asset earns, while operating cost is a coverage driver — which is why a 500,000 understatement is 1.34 times the entire annual headroom and recurs every year (8.1.1). A misstates who bears it: the SPV pays the operator's fee and the costs the contract leaves with it. C confuses routine operating cost with periodic major maintenance (8.1.3). D invents a funding route for a cost that is not funded at all.


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


**8.2-E** `[8.2.2 · Evaluation]` A modeller proposes replacing Kestrel's S-curve (area 3.1900) with the back-loaded profile (area 2.6700) to cut capitalised interest by 262,080 on identical total spend over an identical duration, and asks the contractor to resequence accordingly. The soundest response is:

- A. accept: 262,080 is a real saving produced by the area rule, on the same money over the same time
- B. reject the proposal as framed: at 3.6 % escalation the back-loaded profile's escalation is 231,119 higher and its interest 267,744 lower, so the total funded construction costs differ by only 36,626 — 0.07 % of a 51.7 million spend — and the constraint that makes a sequence legitimate lives in the construction logic, not in the cost model ✅
- C. reject: once escalation is counted, back-loading always costs more than front-loading
- D. accept, and extend the programme by a further quarter to reduce the area again

*Rationale:* escalation and interest run in opposite directions and nearly cancel near `e ≈ g × r = 4.20 %`, so at 3.6 % the whole prize is 36,626 — set against a steeper, riskier finish that no cost model prices (8.2.2, 8.2.3). A quotes the zero-escalation isolation as though it were the answer, which is the trap the isolation exists to expose. C overstates the correction: below the breakeven of about 4.17 % back-loading genuinely is cheaper, by exactly the 36,626 computed here. D compounds the error, because lowering the area by extending the duration defers commercial operations at 532,000 a month of `CFADS` (KA 8.4.2) — against a total interest saving of 262,080 for the entire build.


**8.2-F** `[8.2.3 · Evaluation]` Asked for "a reasonable construction escalation assumption", an assistant returns 3.6 % per annum with no source. The modeller notes that 3.6 % is within the range of published construction indices, and that the model already carries Domain 6's 2.967 % revenue escalation. The soundest position is that:

- A. 3.6 % may be used, since it is within the range of published indices for this class of work
- B. the revenue escalation of 2.967 % should be used for both, so that the model is internally consistent
- C. no escalation rate may be relied on until it names an index, a source, a base date and a human owner in the assumption register, because the rate propagates into the funding envelope, the depreciable base and the delay arithmetic ✅
- D. one blended index should be used for all trades, since trade-level differences average out

*Rationale:* an assistant must not select the escalation rate, which is a forecast of input prices for a specific build in a specific market (the AI boundary in KA 8.2), and construction and revenue escalation are distinct assumptions — labour, steel, cement and specialist plant move differently from consumer prices and from each other. A accepts plausibility as provenance. B imports a revenue assumption into a cost line, which is the silent error 8.2.3 names. D discards the trade mix that makes the rate mean anything. The related discipline is that escalation is applied period-by-period to the profile: escalating the 48,000,000 total by 1.036² gives 51,518,208 against the profile-correct 50,093,393, an overstatement of 1,424,815.


**8.2-G** `[8.2.2 · Comprehension]` The area rule states that `IDC = r_q × g × S × Σ cum(t−1)`. What it tells a reader is that capitalised interest:

- A. depends on total spend and duration, so the shape of the drawdown is a presentational matter
- B. is proportional to the area under the cumulative drawdown curve, so two profiles spending the same money over the same duration produce different interest bills ✅
- C. is proportional to the peak debt balance reached during construction
- D. equals the average of the opening and closing balances multiplied by the rate

*Rationale:* the summation is the discrete area under the cumulative curve, which is precisely what "shape" measures — on Kestrel, 1,345,680 back-loaded against 2,000,880 front-loaded on identical spend and duration, a 655,200 spread and 40.75 % of the S-curve's own bill (8.2.2). A is the belief the rule refutes. C and D describe quantities the rule does not use: the same peak balance is reached on every profile that draws the full facility, and an average-of-two-balances calculation ignores the whole interior of the curve.


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


**8.3-E** `[8.3.2 · Evaluation]` Kestrel's retained register supports a P80 contingency of 4,246,095 (8.85 % of base) while the Stage C accuracy range supports 6,512,795 (13.57 %); the funded balancing line is 3,645,403, a P69.7 provision on the register. The works are let under a fixed-price, date-certain wrap. The soundest recommendation is:

- A. fund 6,512,795, following the convention of taking the higher of two defensible provisions
- B. size the provision at 4,246,095 on the retained register, and settle in the funding documents whether the additional 600,692 comes from debt or from equity — the range-based 6,512,795 measures base-estimate uncertainty the wrap has transferred to the contractor ✅
- C. leave the funded 3,645,403 unchanged and disclose it as a P69.7 provision
- D. fund the 600,692 by capitalising it into senior debt, the coverage cost being only 23.1 % of headroom

*Rationale:* "take the higher" is where the reconciliation starts, not where it ends: the next question is which of the two uncertainties the contracting structure has eliminated, and a full wrap transfers base-estimate uncertainty, leaving the owner the discrete retained register (8.3.2, 8.1.2). A applies half the rule and would fund 2,266,700 against a risk the owner no longer carries. C is honest and insufficient — a named P69.7 beats an unnamed percentage but is still 600,692 short of the confidence the register supports. D is a real funding route that pre-empts the decision it should present: capitalising 600,692 takes the instalment to 5,081,284.04, the `DSCR` to 1.2564 and annual headroom from 372,438 to 286,459 for the whole twelve-year loan life, which is a choice about where the money lands and belongs to the sponsors before close.


**8.3-F** `[8.3.1 · Comprehension]` A delivery manager asks where management reserve sits in a project financing. The best explanation is:

- A. it is contingency under another name, relabelled for the lenders
- B. lenders will not fund undefined scope, so its financing equivalent sits outside the base case as contingent support — a cost-overrun undertaking, standby equity or a standby tranche — called on a trigger rather than drawn on certification ✅
- C. it is the contractor's own contingency inside the contract price
- D. it is the unused balance of contingency remaining at the end of the build

*Rationale:* contingency is funded provision for identified risks within agreed scope, drawn against certification; management reserve covers what the register does not contain, which a facility will not lend against (8.3.1). A collapses a distinction the funding structure depends on; C names a third and separate pot the owner cannot draw at all (8.1.2's pitfall); D describes a release rather than a reserve.


**8.3-G** `[8.3.3 · Evaluation]` Kestrel's ground-conditions risk retires with no impact: the register's P80 falls from 4,246,095 to 2,930,955 while the 10 % rule stays at 4,800,000, leaving 1,869,045 of excess provision. The project director resists reducing it while construction is running, and the finance function does not press the point because the excess costs only the 11,214 a year of commitment fee. The soundest position is that:

- A. the director is right: cover should not be reduced while construction continues, and 11,214 is immaterial
- B. the excess should be released, and the release relied on as a matter of judgement at each quarterly review
- C. a recalculation of the required provision at defined milestones should be written into the finance documents, because the asymmetry is 11,214 a year undrawn against 187,265 of annual covenant headroom — 50.3 % — permanently lost if the excess is drawn ✅
- D. the excess should be drawn now and held as project cash, so that it is available if needed

*Rationale:* released contingency is rarely given back because nobody is rewarded for handing money back, so the answer is mechanical rather than behavioural (8.3.3, 8.A.2): funded 70/30 the excess adds 1,308,332 of senior debt and 156,054 to the annual instalment, taking the `DSCR` to 1.2358 and headroom to 185,173 for the whole twelve-year loan life. A prices the visible cost and ignores the permanent one. B is the arrangement that has just failed. D converts an option into the exposure the release exists to avoid, and does so at once.


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


**8.4-E** `[8.4.1 · Evaluation]` At Auriga's week-13 data date the facility is in balance by +100,000 on the budgeted-rate forecast, out of balance by 116,667 on `BAC/CPI` and by 308,056 on `CPI × SPI`; separately, remaining contingency of 120,000 stands against a remaining-register P80 of 286,185. All four courses below have been proposed. Which should the finance leader take?

- A. argue for the budgeted-rate `EAC` of 4,200,000, which leaves the facility in balance with 100,000 to spare
- B. accept `BAC/CPI`, prepare the 116,667 cash call, and treat the contingency position as a consequence of it
- C. take both tests to the sponsor now, leading with the 166,185 contingency shortfall, because that test rests on the register and the draw history alone and is therefore due whichever `EAC` is certified ✅
- D. revise the remaining register down so that the 120,000 still available is adequate

*Rationale:* the adequacy test needs no forecast at all and fails independently and earlier, so it is the finding that survives whatever the certifier accepts about the `EAC` — and it exposes the real defect, a project funded at sanction to a P53.5 provision nobody named (8.4.1). A is the sponsor's negotiating position and is defensible as a position, but a `CPI` of 0.906 sustained over thirteen weeks is evidence about the remaining work unless somebody can name what has changed, and it leaves the 166,185 untouched. B is the right answer to the second test first: it makes the cash call turn on a forecast method that is negotiable while the register shortfall is not. D is the corruption the adequacy test exists to prevent.


**8.4-F** `[8.4.2 · Evaluation]` A three-month slip is declared at construction quarter six. Capitalising its escalation and interest components — 488,032 — takes debt to 42,488,032, the instalment to 5,067,846.24, the `DSCR` from 1.2743 to 1.2597 and annual headroom from 372,438 to 302,585, a fall of 18.8 % for the whole twelve-year loan life. The soundest position is that:

- A. it should be capitalised: 488,032 is a rounding error against a 60,000,000 envelope
- B. this is a choice about whether the slip lands on coverage for twelve years or on equity return now, and it belongs to the sponsors before the event, in the funding documents ✅
- C. it should be funded with equity in every case, because lenders do not permit delay costs to be capitalised
- D. the decision should be left to the project director in the month the slip is declared, when the amount is known

*Rationale:* a construction event becomes an operating constraint the moment it is capitalised (8.4.2, and Domain 5 KA 5.4.2). A prices the cash and ignores twelve years of coverage. C presents as universal a lender position that is a negotiated term of the funding documents. D leaves a structural choice to the person with the least room to negotiate it and the strongest reason to close it out quickly — which is how the answer gets made by default rather than chosen.


**8.4-G** `[8.4.3 · Comprehension]` A controls function already produces earned value, the `EAC` family, the register and the contingency draw history. What the financing adds to that monthly pack is:

- A. a second set of numbers, prepared to the lenders' definitions
- B. three transformations of the same data — cost to complete, the funds sufficiency position, and the coverage consequence — plus the discipline of stating which confidence level each provision represents ✅
- C. an independent estimate of the cost to complete, prepared by the lenders' technical adviser
- D. the same information recast onto a different cost breakdown structure

*Rationale:* one data spine, two sets of questions: every line the financing annexe needs derives from data the controls function already holds (8.4.3). A is the failure the principle exists to prevent — two sets of numbers create a reconciliation burden that fails exactly when it matters. C describes a diligence activity, not what the pack adds. D is a coding exercise that answers neither question the financing asks.


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


**9.1-E** `[9.1.2 · Evaluation]` A sponsor's model carries the 144,000 annual tax shield on a 6,000,000 shareholder loan — 1,537,168 of present value, 25.62 % of the tranche — inside the base case that will be shown to lenders and to the investment committee. The soundest professional position is that the shield:

- A. belongs in the base case, because interest on a shareholder loan is deductible
- B. should be excluded from the base case until a written tax opinion on this specific structure confirms deductibility, thin-capitalisation headroom and withholding treatment, and should then be disclosed as a documented upside rather than embedded in the cash flow ✅
- C. should be halved, as a prudent allowance for jurisdictional uncertainty
- D. means the shareholder loan should be replaced with ordinary share capital

*Rationale:* A quarter of a tranche's face value created by a documentation choice is the most jurisdiction-dependent figure in this domain: thin-capitalisation limits, interest-deduction caps expressed as a share of `EBITDA`, transfer-pricing constraints on the rate and withholding tax on cross-border interest can each reduce it to nothing, so it is not an input until counsel says it is (9.1.2). A states one jurisdiction's treatment as though it were universal. C substitutes an arbitrary haircut for a determination that is obtainable and close to binary. D discards a legitimate instrument to avoid a question of evidence — and the instrument's real danger is the ranking clause, not the shield.


**9.1-F** `[9.1.3 · Comprehension]` The build-up states `r_f`, `β_e × ERP`, `CRP` and `SP` separately rather than quoting a single required return because:

- A. accounting standards require a disclosed decomposition of a discount rate
- B. the cost of equity is constructed rather than observed, so separating the terms is what makes each judgment reviewable — and what allows the leverage term to be re-levered when gearing changes ✅
- C. the components must be summed in that order for the total to be correct
- D. only the risk-free rate is a judgment; the remaining terms are market data

*Rationale:* `k_e` is not observed and never will be; stating the premiums separately is what turns an assertion into something a reviewer can challenge term by term, and the beta term is the one that must move with the structure or every structure comparison is invalid (9.1.3). A invents a reporting requirement. C confuses a sum with a sequence. D reverses the position — `ERP`, `β_a`, `CRP` and `SP` are all judgments, which is exactly why each needs a named, dated, owned source.


**9.1-G** `[9.1.4 · Evaluation]` The sponsors argue for **75 %** gearing on the exchange rate computed in 9.1.4: moving from 70 % to 75 % buys **48.82 basis points** of equity `IRR` for 8.50 points of `DSCR`, about **5.75 basis points of equity return per hundredth of coverage surrendered**, which they describe as the best-value structural trade available. The lenders size on a 1.30× target (41,171,123, 68.6185 % gearing, `WACC` 8.0001 %); the 1.20× covenant binds at 44,602,050 (74.3367 % gearing, `WACC` 7.9418 %). The soundest response is that the exchange-rate argument:

- A. is correct and decisive — 5.75 basis points per hundredth is a favourable rate and the board should mandate 75 %
- B. is the right frame and stops applying at 74.34 %: above that gearing there is no coverage left to sell, and at 75 % the covenant fails on the base case, so the trade should be priced inside the feasible region and the recommendation made at the 68.62 % the sizing target permits ✅
- C. is invalid, because coverage and equity return are not commensurable quantities
- D. should be resolved by mandating 74.34 %, the highest gearing the covenant permits

*Rationale:* At 75 % debt service of 5,367,466.32 requires `CFADS` of **6,440,960** against 6,384,000 available — short by **56,960** before any stress — so the marginal rate is being quoted across a boundary at which the structure ceases to exist (9.1.4). A applies a valid marginal calculation outside its domain. C rejects the only honest way to argue a gearing: the exchange rate is exactly how the trade should be framed, and framing it is what reveals where it ends. D is the defensible weaker course — it satisfies the covenant arithmetically, with nil headroom, and lenders set a target above the covenant precisely so that a covenant is not a base-case condition; the constraint's price against the sponsors' proposal is **6.51 basis points** of `WACC`, which is what belongs in the paper.


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


**9.2-E** `[9.2.3 · Evaluation]` A board paper discounts Kestrel's cash flows at 7.8840 % — the `WACC` of the 80 % gearing structure — on the ground that it is the lowest cost of capital available to the project. The soundest position is that the appraisal should use:

- A. 7.8840 %, because minimising `WACC` is the objective of a capital-structure decision
- B. the `WACC` of a structure the project can actually raise — 8.0001 % at the coverage-binding gearing — quoted together with the structure it belongs to ✅
- C. the sponsors' corporate `WACC`, since it is the sponsors' shareholders who set the hurdle
- D. any rate between 7.8840 % and 8.0001 %, since the whole range is 11.61 basis points

*Rationale:* A cost of capital is a property of one specific structure, and the 80 % structure fails the 1.20× covenant on the base case, so its rate prices a financing that does not exist; the coverage constraint costs 11.61 basis points of `WACC`, and that is the honest price of bankability (9.1.4, 9.2.3). A imports the corporate-finance objective into a constrained problem. C discounts a single ring-fenced asset at a parent's blended risk. D is right about materiality and wrong about discipline: the difference is small only because someone checked it, and an unlabelled rate is reused in places where the difference is not small.


**9.2-F** `[9.2.4 · Evaluation]` The board is attracted by a 20-year project bond at close: it matches institutional appetite to a 25-year asset and removes the refinancing question. Drawn in full at close against a two-year construction spend, the bond incurs **1,260,000** of negative arbitrage against **252,000** of commitment fees on a progressively drawn bank facility — **35.78 basis points** a year on 42,000,000 — and it would carry make-whole prepayment protection. The soundest recommendation is:

- A. issue the bond: 35.78 basis points is a modest price for twenty years of committed tenor
- B. bank facility during construction, refinanced into bonds at or after completion — capturing progressive drawdown while the spend is uncertain and institutional tenor once the risk profile has changed; and the bond's make-whole terms must be settled at that point, since they can eliminate the refinancing gain the sequencing exists to capture ✅
- C. issue the bond and negotiate a delayed-draw structure, which removes the negative arbitrage
- D. bank facility only: bonds are inappropriate for project financings because amendments are impracticable across a dispersed holder base

*Rationale:* The two instruments are strong in different phases, so the answer is sequencing rather than selection — and 1,008,000 of avoidable cost on this dimension alone is the same order as the entire `WACC` benefit of twenty points of gearing (9.2.4). A treats a first-order structuring decision as an administrative detail. C is the defensible weaker course: a delayed-draw or forward-purchase structure does address negative arbitrage, at a commitment cost of its own, and it leaves the amendment inflexibility of a bond in place through the phase in which amendments are most likely. D promotes one true asymmetry into a prohibition and forgoes the tenor that makes a long concession financeable.


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


**9.3-E** `[9.3.3 · Evaluation]` A funding paper describes a 12,000,000 DFI tranche as "5.25 %, the cheapest money in the structure". Its all-in economic cost, once a 1.00 % front-end fee, 350,000 of advisory spend at close and 120,000 a year of monitoring across an 18-year tenor are counted, is 7.2465 %. The soundest way to report the tranche is:

- A. at 5.25 %, which is the contractual rate the facility agreement will carry
- B. at 7.2465 % and, in a separate column, the capacity effect — six extra years of tenor lifting debt capacity at a 1.30× target from 41,171,123 to 56,299,948 — with a statement of which compliance costs the project would have incurred in any event ✅
- C. as rejected, because 7.2465 % exceeds the commercial market's 6.00 %
- D. as one net figure combining the cost penalty and the capacity benefit

*Rationale:* Cost and capacity are different effects with different causes and belong in separate columns; charging the whole of an environmental and social management system against the tranche also overstates its cost, because a well-run project wants the system and other lenders price the comfort it provides (9.3.3). A repeats the headline the worked example exists to destroy. C optimises cost while ignoring that coverage is the binding constraint. D collapses two decisions into one number and conceals which of them is driving it.


**9.3-F** `[9.3.1 · Comprehension]` In economic terms, an istisna'a facility differs from an ijara facility in that:

- A. istisna'a is a lease and ijara a cost-plus sale with deferred payment
- B. istisna'a finances commissioned construction, the financier procuring the asset to specification and holding delivery risk during the build, while ijara is a lease of a completed asset whose rentals comprise a capital and a return element ✅
- C. they differ only in name, both being loans at interest under another label
- D. istisna'a funds working capital and ijara funds procurement of specific inputs

*Rationale:* The two structures cover the two phases of a project's life, which is why the istisna'a-to-ijara conversion — commonly through a forward lease — is the standard project shape (9.3.1). A inverts them. C denies the asset-based mechanics that determine title, security and the lessor's ownership obligations, which is where economic equivalence with conventional debt is achieved or lost. D describes murabaha and wakala uses. Whether any structure is compliant with Shariah is a determination for the relevant supervisory board and is outside this book's scope.


**9.3-G** `[9.3.4 · Evaluation]` A project whose availability payments are denominated in the host currency is offered two ways to fund the same 15,000,000: a hard-currency export-credit tranche at a materially lower margin and longer tenor, or a local-currency commercial tranche at the market margin. The structuring team recommends the export-credit tranche on the strength of its all-in cost and tenor. The soundest position is:

- A. accept the recommendation: the all-in cost and the capacity effect both favour the export-credit tranche, and cost and capacity are the two tests this Knowledge Area sets
- B. the comparison is incomplete: the cheaper tranche creates a currency mismatch against host-currency revenue, so the margin saving must be set against the cost of hedging or bearing that exposure — the natural hedge is worth paying for, and its price is exactly the margin forgone ✅
- C. reject the export-credit tranche: a currency mismatch is unmanageable at any margin
- D. accept it and rely on the local cost base as a natural hedge, since local costs fall in hard-currency terms as the currency weakens

*Rationale:* The cheapest tranche is frequently the one that creates the exposure, and a margin saving bought with an unhedged devaluation risk is not a saving (9.3.4). A applies the right two tests to the wrong currency. C forgoes a tranche that is perfectly usable where the revenue can be indexed or the exposure hedged — Domain 11 (KA 11.3.2) prices both. D is the seductive error: local costs do provide a partial offset, and it is far too small, because debt service and hard-currency operating costs do not devalue at all — on Kestrel's numbers a **5.06 %** movement breaches the covenant even with the whole local cost base offsetting.


**9.3-H** `[9.3.2 · Comprehension]` An export credit agency's exposure premium is described as "capitalised into the loan". In cash-flow terms that means:

- A. the premium is deducted from the amount advanced, so proceeds fall and repayments are unchanged
- B. the premium is added to the amount borrowed, so cash proceeds are unchanged while the sum repaid rises — which is why the headline rate understates the cost and only the rate solved from proceeds against instalments measures it ✅
- C. the premium is treated as a capital cost of the project and depreciated
- D. the premium is waived in exchange for a higher margin

*Rationale:* Capitalisation grosses the loan up: 15,000,000 of eligible equipment financed with a 6.0 % premium becomes a 15,900,000 loan against 15,000,000 of proceeds, and the all-in cost is **4.6895 %** against a 3.80 % headline — **88.95 basis points** the ranking would otherwise miss (9.3.2). A describes a deducted arrangement fee, which reduces proceeds instead. C is an accounting treatment, not a cash-flow mechanic, and it does not change what is repaid. D describes a different bargain altogether.


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


**9.4-E** `[9.4.3 · Evaluation]` A 15 basis point sustainability ratchet on a 42,000,000 facility is worth 528,182 in present value against 712,627 of verification and reporting cost — net **−184,445** — even assuming both key performance indicators are met every year. The soundest professional response is:

- A. take the label: 15 basis points is a saving, and the reporting would be done anyway
- B. ask the arranger what the label does to the **base** margin and to the size of the club, because a 10 basis point base-margin reduction turns the combined position positive by 167,677 — and if the case rests on the ratchet alone there is no case ✅
- C. reject sustainability-linked structures generally, since the ratchet is value-negative
- D. accept the ratchet and set key performance indicators the project would meet in any event, so that the reduction is certain

*Rationale:* A ratchet of this size cannot pay for a credible verification apparatus, so the negotiation belongs on the base margin and the lender pool, where the value actually is (9.4.3). A weighs a contingent benefit against a permanent cost and omits the cost. C generalises one arithmetic result into a policy and forgoes a benefit the same arithmetic shows is available. D is the greenwashing failure in one sentence: a target the project would have met is not a target, it carries reputational and increasingly regulatory exposure, and because the ratchet is symmetric in most drafting a KPI set chosen without headroom converts the initiative into a cost.


**9.4-F** `[9.4.2 · Comprehension]` A finance ministry official describes a concessional loan as "part loan and part gift". Expressed in this domain's terms, that means:

- A. part of the principal will be forgiven at maturity and the remainder repaid
- B. the loan's subsidy content is the shortfall between its face value and the present value of its debt service discounted at a market rate — 40.80 % of a 12,000,000 tranche at 2.0 %, so about 7.1 million of borrowing and 4.9 million of support, with every dollar repaid ✅
- C. the gift is the total interest saved over the loan's life relative to a market rate, before discounting
- D. the description is loose talk: a loan repayable in full contains no grant

*Rationale:* The grant element is `1 − PV(debt service at a market rate) ÷ face value`, which measures subsidy in present-value terms without any principal being written off (9.4.2). A describes forgiveness, a different instrument. C omits discounting, which is where a seven-year grace period does most of its work. D confuses legal form with economic substance and would report 12,000,000 of borrowing where 7.1 million exists — misstating both the leverage and the support received.


**9.4-G** `[9.4.1 · Evaluation]` A grantor with a fixed fiscal envelope can direct its support at a project either as a capital grant reducing the amount to be financed or as a minimum revenue guarantee capping the downside on a demand-based revenue line. Its stated objective is to get an otherwise unfinanceable project financed at the least fiscal cost. The soundest advice is:

- A. give the guarantee: no cash leaves the budget, so the fiscal cost is nil
- B. direct the support at the coverage face and, whichever instrument is chosen, value the guarantee as the written option it is — support that improves coverage buys more financeability per unit of fiscal cost than support of equal value that improves return, and an unvalued guarantee is an unmeasured liability rather than a free one ✅
- C. give the grant, because grants are always cheaper for a grantor than contingent commitments
- D. give whichever the sponsors prefer, since they are the party that must raise the financing

*Rationale:* Coverage is the binding constraint, so support aimed at the coverage face converts fiscal cost into financeability most efficiently — and a guarantee is a written option whose value can be estimated, so "no cash moves" is a budgeting statement and not a cost statement (9.4.1, 9.1.4). A is the error the topic exists to correct. C reverses it and asserts a universal ranking: a guarantee that is never called costs nothing, and its expected cost is what has to be compared. D delegates the public-purpose judgement to the party whose interest lies in the return face — on the grant arithmetic of 9.4.2, the answer will be the structure that lifts equity `IRR` by 429.20 basis points and leaves `DSCR` untouched.


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


**10.1-F** `[10.1.3 · Evaluation]` Sizing Kestrel at a 1.30× target gives 41,171,123 on the base-case test, 38,253,896 on the minimum-period test and 39,915,812 sculpted. An arranger asks the sponsor's adviser to confirm "the debt capacity". The soundest answer is:

- A. 41,171,123, since the base-case test is what the market conventionally applies
- B. that none of the three is "the" capacity: the figure is meaningless until the term states whether coverage is tested on the base case or in every period, and whether service is level or sculpted — so the sizing basis must be agreed before any number circulates ✅
- C. 39,915,812, because sculpting is the technically superior structure
- D. 38,253,896, because prudence requires the lowest of the available answers

*Rationale:* The three answers differ by 2,917,226 on identical cash flows, and the whole difference is definitional — it belongs in the term sheet, not in a footnote (10.1.3, Toolkit 10.T.4). A concedes the tested period without negotiating it, which is the easy half of the term. C recommends a structure before its documentation, modelling and re-cutting cost has been weighed, and before the facility has said who re-cuts a sculpted profile when the forecast moves. D mistakes conservatism for analysis and surrenders the 1,661,916 that sculpting recovers without relaxing any period's requirement by a basis point.


**10.1-G** `[10.1.2 · Comprehension]` A 1.30× target tested on the base case and the same 1.30× required in every period differ in that:

- A. they are the same requirement expressed in two ways
- B. the first requires the coverage only in the period tested — conventionally the first full operating year — while the second requires it in the weakest period, so on any uneven profile the second is the tighter constraint on the same cash flow ✅
- C. the minimum-period test is the looser of the two, because it disregards the early years
- D. the distinction matters only where cash flow is level

*Rationale:* One ratio, two tests; the gap between them is a property of the shape of the cash flow rather than of the ratio (10.1.2, 10.1.3). A ignores that a single-period test says nothing about the other eleven periods. C inverts the definition — the minimum-period test binds precisely because it is measured on the worst period. D reverses the condition: where cash is level the two coincide, and it is unevenness that separates them.


**10.1-H** `[10.1.2 · Evaluation]` The sponsors have asked for 42,000,000 and the 1.30× target supports 41,171,123, leaving the **828,877** gap. Kestrel's offtake runs 25 years inside a 27-year concession and the facility is drawn for 12. Of the resolutions available, the one a leader should test **first** is:

- A. contribute the 828,877 as additional equity, which is the arithmetic residual and closes the gap with certainty
- B. one additional year of tenor: at 13 years the same 1.30× target supports **43,473,483**, which clears the request with 1,473,483 to spare, and the concession and offtake terms plainly accommodate a 13-year facility ✅
- C. argue the target down from 1.30× to the 1.2743 the requested amount delivers, since that is the coverage the project actually produces
- D. raise `CFADS`, which is the only lever that improves the lender's position as well as the sponsors'

*Rationale:* Debt capacity depends on cash, coverage, rate and tenor and on nothing else, so the question is which of the four is genuinely available — and tenor is available here, bounded by the offtake and concession rather than by appetite, while equity is the residual that is contributed when the other levers fail (10.1.2). A is defensible and is the answer of last resort: it closes the gap by funding it, and it should be priced against a lever that costs the sponsors nothing. C asks a credit committee to abandon the margin its target exists to create, which is the request least likely to succeed and the one that damages the negotiation elsewhere. D is the defensible weaker course on the right principle: it does help both parties, and the uplift required is **128,526** a year — **2.0132 %** of `CFADS` — which has to come from a revenue or cost commitment somebody will have to make good, whereas the thirteenth year is a drafting change. The two cautions on B belong with it: the extra year must sit comfortably inside the tail lenders require, and at 42,000,000 over 13 years the year-one `DSCR` becomes **1.3456**, which is the number the committee will actually test.


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


**10.2-G** `[10.2.3 · Evaluation]` A sponsor's credit paper reports a `DSCR` of 1.4551 on a 25 % balloon structure, against 1.2743 fully amortising, as evidence of a stronger credit. The soundest professional position is:

- A. the paper is right: `DSCR` is the covenanted ratio, and 1.4551 is more comfortable than 1.2743
- B. the coverage is deferred rather than earned — `DSCR ÷ LLCR` is 1.1419 and the year-twelve obligation is 14,887,226, against which that year's coverage on the same cash is 0.4288 — so the balloon is defensible only if it is sized against a stated refinancing plan, stress-tested, and reported with the maturity obligation beside the ratio ✅
- C. balloons should not be used, because they convert a credit question into a market question
- D. the two structures are equivalent, since `LLCR`, `PLCR` and `ICR` are identical in both

*Rationale:* Nothing about the project, the cash it generates or the amount owed has changed, so a higher period ratio is information about the schedule and not about the credit (10.2.3). A reports an arithmetically correct figure that misdescribes the risk. C is the opposite failure of judgment: a balloon matched to a genuine cash profile is cheaper than the equity it displaces, and 10.1.3's 1,667,864 balloon would have closed Kestrel's sizing gap for a deferral of 3.97 % of principal. D uses the horizon ratios' immunity to conclude that nothing differs, when the thing that differs — 14,887,226 falling due on one date — is the entire exposure.


**10.2-H** `[10.2.2 · Comprehension]` `PLCR` exceeds `LLCR` whenever a project outlives its loan because:

- A. `PLCR` discounts the same cash flow at a lower rate
- B. `PLCR` discounts `CFADS` to the end of the project's economic life while `LLCR` stops at loan maturity, so `PLCR` counts the tail — cash the lenders have no contractual claim on, which is why they rely on it least ✅
- C. `PLCR` is computed on `EBITDA` and `LLCR` on `CFADS`
- D. `PLCR` adds the asset's residual value to its numerator

*Rationale:* The two ratios differ only in the horizon of the numerator, and the extra cash is exactly the cash beyond the lenders' claim (10.2.2). A invents a rate difference; both discount at the loan rate. C confuses `PLCR` with `ICR`, which is the accounting measure. D adds a terminal value that neither ratio contains.


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


**10.3-F** `[10.3.3 · Evaluation]` Lenders ask for a 50 % cash sweep. The sponsor's treasury recommends conceding it, on the ground that the present-value cost to equity is only 315,488 at 8 % against a 3,871,824 nominal diversion. The soundest position is:

- A. concede it: 315,488 is immaterial beside the 1,821,314 of interest the sweep saves the lenders
- B. concede it only in exchange for something, and only on drafting settled first: the present-value figure understates the cost because a swept structure de-gears faster, so the equity return falls even where present value barely moves ✅
- C. refuse any sweep, since diverting distributable cash always destroys equity value
- D. concede it and present the 0.7227-year reduction in weighted average life as a benefit shared with the lenders

*Rationale:* Prepaying 6 % debt while discounting equity cash at 8 % is close to value-neutral in present value, which is precisely why present value is the wrong test here; the real costs are leverage, flexibility and cash unavailable in the year it arises — and whether the sweep is taken before or after reserve top-ups will move the answer by more than raising the share from 50 % to 60 % would (10.3.3). A misattributes the interest saving, which accrues to equity: the lenders forgo it. C denies a trade routinely worth making against a covenant reset, a longer tenor or a lower margin. D gives away the consideration the concession should have bought — the shorter average life is the lenders' benefit, not a mutual one.


**10.3-G** `[10.3.2 · Comprehension]` A newly appointed director is told that a debt service reserve "protects the covenant". Restated correctly, what the reserve does is:

- A. buy time — it keeps scheduled debt service being paid through a short cash shortfall, converting a liquidity failure into a negotiation, while the coverage ratio, computed on `CFADS`, fails regardless ✅
- B. raise `CFADS` in the year it is drawn, because the cash reaches the lenders
- C. reduce the required coverage ratio in proportion to the months funded
- D. stand in place of a covenant, which is why facilities carrying a reserve covenant looser ratios

*Rationale:* A reserve is a liquidity instrument and a covenant is a cash-flow test, so a fully funded six-month reserve preserves payment and does not prevent breach — Kestrel can absorb a collapse of `CFADS` to 2,504,818, some 39 % of base case, and still pay in full while the ratio is far below 1.20 (10.3.2). B is the error the definition of `CFADS` excludes: it counts cash the project found rather than cash it generated. C and D assert relationships between reserves and ratios that no facility creates — the month count is negotiated for how long lenders want before they must act.


**10.3-H** `[10.3.2 · Evaluation]` In final negotiation the lenders ask for a **twelfth** month of debt service reserve in place of six. Each month buys **6.5393 percentage points** of single-year cash tolerance and costs **417,470** of funded cash — **63,840** per percentage point — and the facility permits an LC-backed reserve at a market fee of 1.25 % against a breakeven of 12.42 %. The soundest response is:

- A. refuse: six months is the market convention and twelve is excessive for an operating asset
- B. accept the twelve months and satisfy them with a letter of credit: the quantity question is worth arguing only if cash must be funded, and at 1.25 % against a 12.42 % breakeven the incremental 2,504,818 of cover costs 31,310 a year rather than 311,098 ✅
- C. accept the twelve months and fund them from the senior facility, since after-tax debt at 4.80 % is the cheapest source
- D. offer nine months as a midpoint, which buys 80.3821 % of tolerance

*Rationale:* Twelve months takes single-year tolerance from **60.7641 %** to **100 %** — 39.2359 points for a further 2,504,818 — and the instrument, not the month count, is what determines whether that is expensive (10.3.2, 10.3.2b). A defends a convention against a request whose price the sponsor can make trivial. C is the trap the worked example exists to expose: inside a binding coverage constraint every use of debt competes with every other at par, so the reserve displaces capex borrowing one for one and enlarges the equity requirement. D is the defensible weaker course — a genuine midpoint, correctly computed, which spends negotiating capital on a quantity that has stopped mattering once the instrument question is settled, and buys back a month the lenders may value more than the sponsor gives up.


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


**10.4-F** `[10.4.1 · Evaluation]` A facility tests `DSCR` on both a historic and a forward-looking basis. At the end of year ten the historic test passes at 1.2058 and the forward test fails at 1.1957. The compliance certificate reports the historic figure only. The soundest professional position is:

- A. the certificate is adequate: the historic figure is the only one that can be observed
- B. a facility with both tests effectively has the earlier of the two as its covenant, so a certificate reporting only the historic figure certifies compliance in the very period the facility fails; both figures must be reported, on an agreed basis of preparation for the forward test ✅
- C. only the forward figure should be reported, since the forward test is the tighter covenant on any project
- D. the forward test should be resisted altogether, because it tests a forecast nobody can observe

*Rationale:* On a declining coverage profile the forward test breaches a full test date earlier, and that test date is exactly when a cure, a waiver or an operational fix is still cheap — the design intent of the covenant set, made arithmetic (10.4.1). A treats observability as the criterion for disclosure. C overgeneralises: which basis binds follows the slope of the project's cash, and on Domain 6's rising sponsor case the historic test is the only one that could ever fail. D discards the year of warning the forward test buys; the answer to a test on a forecast is a defined basis of preparation with stated prevailing assumptions, not removal of the test.


**10.4-G** `[10.4.3 · Evaluation]` Kestrel's mandated 42,000,000 facility breaches its 1.20× covenant in years eleven and twelve of the bank case. Curing both breaches costs **96,196** if the cash is deemed to be `CFADS` and **61,485** if it is applied to prepayment. Counsel proposes spending the remaining negotiating capital on securing the prepayment treatment. The better judgement is:

- A. agree: 34,711 is a 36.08 % saving, and `P = C ÷ λ` makes the drafting worth more the tighter the covenant
- B. the drafting point is real and minor: a facility whose **base case** needs a cure has consumed an option that should be held for a downside, so the capital belongs on the sizing — the year-twelve shortfall of 74,849 is the 828,877 resizing question in another form ✅
- C. disagree: cure cash is always deemed to be `CFADS`, so there is nothing to negotiate
- D. disagree: cure rights are unlimited in number, so their cost is immaterial

*Rationale:* The cure arithmetic is correct and answers the smaller question, while the facility is being sized to breach on its own base case — which is a capacity problem the cure conceals, and the ground on which Domain 6's model auditor rejected an otherwise trivial 96,196 of curing (10.4.3, 10.1.2). A is the defensible weaker course: the identity and the saving are both real, and both are small beside a structure that starts in breach. C asserts a single treatment where the domain describes two standard ones, and the negotiation is ordinarily about both limbs — cash in, and where it goes. D is false: cures are limited in number and in consecutive periods, and each one consumed is unavailable later.


**10.4-H** `[10.4.2 · Comprehension]` A sponsor's board is told that the facility has "a 1.20× covenant and a 1.15× lock-up, so there are two levels to watch". The 1.25× distribution condition is not mentioned. The clearest statement of what the three thresholds do is:

- A. the distribution condition decides whether this period's cash may leave the structure at all, the covenant decides whether the facility is in breach, and the lock-up decides whether retained cash stops being the sponsor's — three different consequences at three different levels ✅
- B. the three are alternative drafting formulations of the same test, and only the lowest binds
- C. the distribution condition and the lock-up trigger are the same mechanism, one expressed as a ratio and the other as a cash figure
- D. the covenant is the operative constraint and the other two are consequences of breaching it

*Rationale:* The thresholds engage in order and do different things, which is what makes the design graduated: on Kestrel they sit at 6,262,044, 6,011,562 and 5,761,081 of annual `CFADS` (10.4.2). B denies the sequencing the structure depends on. C conflates two mechanisms whose difference is the whole point — failing the distribution condition delays this period's cash, while falling through the lock-up trigger stops the cash being the sponsor's. D reverses the order of engagement: cash is retained long before a breach becomes an event of default, and on Kestrel's own bank case the distribution condition catches eight of twelve years while the lock-up trigger never engages at all.


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


**11.1-D** `[11.1.3 · Evaluation]` The allocation arithmetic shows the five uncontrollable items destroying 1,780,000 of value at a 40 % loading. A colleague proposes reducing the owner's retained probabilities so that the full wrap can be recommended to the board on price grounds. The soundest professional position is:

- A. adjust them: the register is a negotiating instrument and the full wrap has strategic value
- B. hold the retained probabilities to their evidence base and negotiate item by item on that evidence, because the register's probabilities *are* the negotiation and an input adjusted to reach a conclusion has inverted the analysis ✅
- C. adopt the bidder's probabilities throughout, since the bidder is the party pricing the risk
- D. abandon the arithmetic, since probabilities are subjective and cannot support a decision

*Rationale:* The whole result rests on the honesty of the two `EMV` columns: a sponsor who understates its retained probabilities will "prove" that every transfer destroys value, and a bidder who overstates its own will justify any premium (11.1.3). A produces a recommendation the lender's diligence will reverse, at the cost of the register's credibility everywhere else. C imports the less-informed party's assumptions about a ground investigation the owner commissioned, which is how an information advantage becomes a price disadvantage. D discards a rule that is transparent and challengeable in favour of instinct, which is neither.


**11.1-E** `[11.1.3 · Evaluation]` The preferred bidder declines to price Kestrel's register items separately and offers a single wrap covering all eight threats for a premium of **6,748,000** — the sum of the 2,128,000 it quoted on A1–A3 and the 4,620,000 it quoted on A4–A8. Retaining the whole register has an expected cost of 7,110,000; transferring A1–A3 alone and retaining the rest costs 4,818,000. The recommendation is:

- A. accept the bundle: at an expected cost of **6,598,000** it beats full retention by **512,000**, and a single wrap removes every argument about which item a loss belongs to
- B. refuse the bundle and require the items to be priced line by line — A1–A3 transferred and A4–A8 retained costs 4,818,000, which is **1,780,000** better than the bundle ✅
- C. refuse all transfer: the bidder's own expected cost on A4–A8 exceeds the owner's, so no wrap creates value
- D. accept the bundle and negotiate the 40 % loading down, since the loading is where the value destruction sits

*Rationale:* The bundle genuinely beats full retention, which is exactly what makes A the trap — it is defensible on its own comparison and leaves 1,780,000 on the table, because it buys five items the bidder cannot influence at the same time as three it can (11.1.3). C generalises the A4–A8 result across the register and forgoes the 2,292,000 the control-based transfers create. D misplaces the defect: stripping the loading out entirely still destroys 460,000 on A4–A8, because the bidder's own expected cost there (3,300,000) exceeds the owner's (2,840,000). The negotiating point that follows from B is that unbundling is itself the ask — a bidder that will not price line by line is charging for the items it would rather not discuss.


**11.1-F** `[11.1.2 · Comprehension]` An insurer accepts a risk it cannot influence in any way. Expressed in this domain's terms, that transfer rests on:

- A. control, since the insurer's loss-prevention requirements change the project's behaviour
- B. capacity: the insurer cannot change the distribution but holds it more cheaply, being diversified across many such exposures — so value is created by moving the exposure to a cheaper holder rather than by improving it ✅
- C. bargaining power, since the project has no alternative
- D. no recognised ground, which is why insurance is a cost rather than a transfer

*Rationale:* The two defensible grounds are control, which changes the underlying distribution, and capacity, which changes only who holds it; diversification is the classic capacity case (11.1.2). A describes a real secondary effect and names the wrong ground — the insurer's requirements do not put it in charge of the welding. C describes the indefensible ground and does not apply: the insurer is a willing party pricing an exposure it can bear. D denies a transfer that the pricing test of 11.1.3 values in the ordinary way, as expected cost retained less loaded premium.


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


**11.2-D** `[11.2.3 · Evaluation]` Asked by a credit committee to test escalation, a model adds a percentage point to both the consumer price index and the power escalation rate, and reports that year-twelve `DSCR` rises from 1.2106 to 1.3088. The soundest reading is:

- A. the structure is insensitive to escalation: both drivers were stressed and coverage improved
- B. the test is not evidence — the exposure is the differential, which widens only from 2.70 to 2.90 percentage points under this stress while every escalating line simply grows, so the case must be re-run on the spread between the cost driver and the revenue index ✅
- C. the model contains an error, since escalation must reduce coverage
- D. the test is adequate once a volume stress is added alongside it

*Rationale:* Kestrel's revenue-weighted escalation of 2.00 % against a cost-weighted 4.70 % *is* the exposure; a stress that lifts both leaves that gap almost unchanged and makes the reported ratio look better, so it invites the opposite of the correct conclusion (11.2.3). A accepts a favourable output without asking what was varied — the level of power prices matters far less here than the spread. C mistakes a modelling artefact for an arithmetic defect. D adds a second variable without repairing the first, and the joint table it implies would still test the wrong thing.


**11.2-E** `[11.2.2 · Comprehension]` A pass-through of 70 % of movements in an input price differs from a fixed-price supply contract for the same input in that the pass-through:

- A. removes the exposure entirely, as the fixed price does
- B. divides the exposure rather than removing it — the project keeps 30 % of every movement, and what it keeps also depends on the reference index and the reset frequency — whereas a fixed price replaces the price exposure with the supplier's willingness and ability to hold the price ✅
- C. removes the exposure while a fixed price merely defers it
- D. has no effect on coverage, since the cost is incurred either way

*Rationale:* A pass-through divides an exposure and multiplies coverage tolerance by 1/(1 − `φ`); it eliminates nothing, and a share indexed to a published tariff the plant does not actually pay leaves basis risk inside the protected portion (11.2.2). A and C misstate what each instrument does — and a fixed price substitutes a counterparty credit question for a market one. D ignores that the retained residual falls straight through to `CFADS`, which is the quantity the covenant divides.


**11.2-F** `[11.2.4 · Evaluation]` Kestrel's O&M agreement carries a 95 % availability guarantee with damages attached, and the project breaches its 1.20× covenant at **92.086 %** availability — so 2.9 percentage points of availability separate compliance from breach. With one negotiating session left on the O&M agreement, the finance leader should spend it on:

- A. the damages rate for missed availability, which is the operator's financial incentive to perform
- B. the definition of an excusable outage — the availability/force-majeure boundary decides whether a lost month counts against the guarantee at all, and it is worth more than the rate attached to it because force majeure suspends performance obligations and never suspends debt service ✅
- C. the liability cap, which is scaled to the fee and therefore too small whatever the rate
- D. the fee at risk, which gives the operator a running stake rather than a terminal liability

*Rationale:* With 2.9 points of availability between compliance and breach, the question that decides the covenant is which lost days are counted, not what is paid for the days that are — and an outage reclassified as excusable lands squarely on coverage with no recovery at all (11.2.4, 11.3.4). A funds the consequence rather than preventing it, and an operator's damages are capped on its fee in any case. C and D are both sound and both weaker: the cap is genuinely too small — a 30-day outage costs 742,000 against a half-fee cap of 600,000 — and fee at risk is genuinely the better incentive design, but each allocates money after the event, while the boundary definition determines whether there is a claim to make. The wider lesson is the one 11.3.4 states: insurance waiting periods, cure periods and availability carve-outs are calibrated in time while covenants are calibrated in cash, and somebody has to perform the translation.


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


**11.3-E** `[11.3.2 · Evaluation]` The offtaker will accept a host-currency tariff with 40 % of it indexed to the exchange rate. The debt-service-matching share is 52.997 % and the covenant-preserving minimum against a 25 % devaluation is 48.9318 %. The bid team argues that partial indexation is better than none. The soundest position is:

- A. accept 40 %: partial protection is better than none, and the offtaker has moved once already
- B. hold for a share at or above 48.9318 %: at 40 % the covenant fails on a **14.54 %** devaluation and a 25 % devaluation leaves a `DSCR` of **1.1572**, while the clean structural ask of 52.997 % sits only 4.07 points above the minimum defensible one ✅
- C. refuse any host-currency tariff, since a twelve-year currency mismatch cannot be managed
- D. accept 40 % and hedge the residual exposure in the swap market

*Rationale:* The tolerable devaluation runs 5.06 % unindexed, 14.54 % at a 40 % share and 37.17 % at the matching share, and because the matching share is barely more expensive than the minimum there is little to be gained by conceding to a partial one (11.3.2). A treats any movement as progress without testing it against the covenant. C forgoes a transfer that is well grounded on **capacity** — a payer with local-currency revenue and sovereign-adjacent standing bears a devaluation that would destroy the SPV — and it will be paid for in the tariff. D assumes a market that does not exist for a twelve-year tenor in most host economies at any price a project can pay.


**11.3-F** `[11.3.1 · Comprehension]` Saying "the covenant fails at an all-in rate of 6.7390 %" is a different kind of statement from forecasting the reference rate because a breakeven:

- A. is a more accurate forecast, being derived from the schedule rather than from the market
- B. is a fact about the structure — the level at which a named test fails, given the schedule, the `CFADS` definition and the covenant — while a forecast is a claim about the world ✅
- C. rests on no assumptions at all
- D. is the same statement expressed in different units

*Rationale:* This is why the governed use of a model here is to compute breakevens rather than to predict rates: a breakeven can be monitored against a document, whereas a prediction can only be owned (11.3.1). A collapses the two categories into one. C overstates — the 6.7390 % still depends on the `CFADS` figure and the fixed-principal schedule it is computed from, which is why it must be recomputed after any amendment. D ignores that one statement is conditional and the other predictive.


**11.3-G** `[11.3.1 · Evaluation]` Treasury proposes to leave Kestrel's floating facility unhedged, on the ground that scheduled debt service can be paid up to an all-in rate of **9.2723 %** and the reference rate stands at 4.00 %. A full hedge fixes coverage at **1.2533** at a year-one cash cost of **84,000**; a 75 % hedge holds **1.2085** at +200 basis points for **63,000**; and the minimum hedge ratio surviving that shock at the covenant is **70.0576 %**. The recommendation should be:

- A. leave it unhedged: **327 basis points** of reference-rate headroom to payment failure is ample, and the 84,000 is a certain cost against a contingent exposure
- B. hedge at not less than 70.06 %, and in practice at 75 %: the exposure that binds is the covenant at **+73.9 basis points**, not payment at +327.2, and 0.0210 of coverage buys the removal of 0.4397 of coverage range — 20.92 units of range per unit surrendered ✅
- C. hedge fully: a single covenanted coverage figure at any reference rate is the only defensible position for a project financing
- D. leave it unhedged and rely on the debt service reserve, which covers a rate shock as readily as a cash shortfall

*Rationale:* Interest-rate exposure is a covenant exposure long before it is a payment exposure, and treasury has answered the question four and a half times too generously (11.3.1). A is the under-hedging error in its usual form — the certain 84,000 is visible and the contingent range is not. C is the defensible weaker course and a common covenanted outcome: it does remove the whole range, and it pays 84,000 rather than 63,000, forgoes every benefit of falling rates, and enlarges the mark-to-market break cost that a later refinancing must pay. D misreads what a reserve does: it buys payment continuity and time, not compliance, and the breach at +73.9 basis points happens with the reserve fully funded. Two disciplines belong with B — the hedge profile should amortise with the outstanding balance rather than sit flat, and the hedge counterparty's own credit is now inside the structure.


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


**11.4-E** `[11.4.2 · Evaluation]` A sponsor prepares "the bank case" by multiplying its own register probabilities by 1.5 and its impacts by 1.4 and admitting a 0.30 correlation, reproducing the order of magnitude the lender's advisers apply. The soundest professional position is:

- A. the case is adequate, since it reproduces the multipliers the lender's advisers use
- B. multipliers illustrate direction and order of magnitude only: the re-cut must be item-specific and evidenced, because the disagreement is evidential and will be settled line by line — this probability on this ground investigation, this impact on this remediation quotation ✅
- C. present only the sponsor's own mean, since the lender will produce its own case regardless
- D. adopt the lender's P80 of 8,884,036 as the sponsor's base case, to remove the argument

*Rationale:* A bank case produced by multiplication is the intellectually empty version of the exercise: it concedes the arithmetic without contesting a single input, and multipliers illustrate while evidence decides (11.4.2). A mistakes agreement on a multiplier for agreement on evidence. C arrives at a credit committee unable to reproduce the calculation that will set the debt quantum — the lender's re-cut removes 4,801,313 of capacity. D over-concedes: adopting a tail as a central case mis-sizes contingency, reserves and distributions, when the gap between the two cases is a financeable quantity to be closed by a reserve, a sponsor commitment or less debt.


**11.4-F** `[11.4.4 · Evaluation]` Kestrel's operating register has a mean of **2,125,000** and a P80 of **4,003,649** on an independence assumption, against a covenant-preserving exposure ceiling of **3,122,460**. The team proposes to provision the mean, on the ground that it is the expected case. The soundest position is:

- A. agree: the mean is the unbiased estimate, and the P80 is a construction-side convention that does not belong in an operating provision
- B. the **881,189** gap between the register's own P80 and the ceiling is the size of the problem, and it is closed by one of the four coverage levers or by equity — with the correlation assumption stated on the face of the output, since admitting ρ = 0.30 raises the P80 by a further **837,478** ✅
- C. adopt the lender's P80 of 8,884,036 and resize the facility to 37,198,687
- D. the two figures are not comparable, since the register is a present value and the ceiling an annual headroom figure

*Rationale:* Both quantities are present values — the ceiling is headroom × `AF(0.06, 12)` precisely so that the comparison can be made — so the gap is a financeable quantity rather than a choice of percentile (11.4.4). A selects the measure that makes the problem disappear, against the project's own contingency policy on the construction side and against a covenant that is a fixed claim rather than an expectation. C is defensible and over-conservative: the lender's re-cut is a negotiating position to be argued item by item on the evidence (11.4.2), and adopting it unexamined surrenders 4,801,313 of debt capacity before the argument has been had. D is simply wrong about the units, and it is the reason the comparison is so often not performed.


**11.4-G** `[11.4.3 · Comprehension]` On the shared registry's definition, **model risk** is:

- A. the risk that a model contains a coding error
- B. the risk of loss from decisions or actions taken on the output of a model — because the model was flawed, because it was used outside the conditions it was validated for, or because its output was misunderstood by whoever acted on it ✅
- C. the risk that a model's forecast differs from the outcome, which is inherent in any forecast
- D. the risk that a model or its training data is compromised by an intruder

*Rationale:* The definition turns on the decision taken and covers three distinct failures — a wrong model, a sound model used in the wrong envelope, and a correct output misread (11.4.3). A is one cause of the first failure only. C describes forecast uncertainty, which is a property of the future rather than a defect of the model, and treating the two as the same makes the register line unmanageable — part of why `p` here is a function of governance rather than of nature. D is cybersecurity risk, which this domain treats primarily as an availability exposure on the operational-technology network.


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


**12.1-E** `[12.1.2 · Evaluation]` Kestrel's delay damages run at 20,000 per day under a 4,800,000 cap. With limited negotiating capital left before signature, the soundest priority is to:

- A. press for a higher daily rate, since a higher rate raises recovery on every day of delay
- B. compare the cap-binding day — 4,800,000 ÷ 20,000, day 240 — with the credible worst-case delay from the schedule risk analysis, and pair the cap with a termination-for-delay right at a long-stop date, because beyond the cap the contractor's marginal cost of a further day is zero ✅
- C. press for the 20 % aggregate cap to be raised, since the aggregate is the real limit and a higher aggregate extends delay recovery
- D. accept the regime, since a 10 % delay cap under a 20 % aggregate is conventional

*Rationale:* The rate governs recovery only to the cap-binding day, after which further delay is wholly uncompensated — 2,620,000 on a 300-day slip — so a cap that binds before the P80 delay does not cover the risk it was bought for (12.1.2). A spends leverage where sponsors habitually spend it and where it buys least. C is a true point misapplied: the aggregate binds only when a *third* head of claim arises, and raising it extends the delay sub-cap by not one day. D substitutes market convention for the project's own schedule evidence, which is the only thing that can calibrate a cap.


**12.1-F** `[12.1.4 · Comprehension]` The "liability asymmetry" in an O&M agreement means that:

- A. the operator's bonuses exceed the deductions it can suffer
- B. the operator's liability cap is scaled to its fee while the loss its failure causes is scaled to the project's revenue, so the two are measured on different bases and the cap is smaller than the loss by construction ✅
- C. the operator's liability outlasts its appointment
- D. the cap binds the project company but not the operator

*Rationale:* A cap expressed as one year's fee, or half of it, stands against an outage cost measured on the same daily basis as a construction delay, so a 30-day outage already exceeds a half-fee cap (12.1.4). That is why lenders price operating risk in the coverage ratio and the maintenance reserve rather than relying on the O&M contract. A describes an incentive regime, not a cap. C describes a survival period. D misstates whom the cap protects.


**12.1-G** `[12.1.3 · Evaluation]` Kestrel's negotiator proposes to accept performance damages on the sized-coverage basis, **2,980,263**, describing it as the market standard. The value of the sponsors' loss on a permanent 5 % output shortfall is **4,835,674**; the bare covenant-restoring figure is **562,851**; the performance sub-cap is 4,800,000. The recommendation to the investment committee should be:

- A. accept 2,980,263: it restores the 1.2743 the debt was sized on, which is the standard the financing was built to
- B. ask for the value basis and, if the sized basis is conceded, direct part of the proceeds away from mandatory prepayment — because the sized basis gives up **1,855,410** of value loss, and even the full 4,800,000 applied wholly to prepayment under-compensates equity by **521,045** ✅
- C. accept 562,851: the covenant is the only contractual test, so anything above it is a windfall
- D. insist on 4,835,674, which the sub-cap makes deliverable

*Rationale:* The three bases restore three different things, and only the value basis restores the sponsors' loss, so the ask and the fallback should each be stated with what it concedes (12.1.3). A is the defensible weaker course — it is the common drafting and the coverage argument supports it — and it transfers 1,855,410 of value loss to equity without saying so. C adopts the lenders' interest as the equity case, an understatement of 8.591×. D overstates what is available: 4,835,674 exceeds the 4,800,000 sub-cap, so the value basis requires the cap to move as well as the calibration, and asking for the number without the cap is asking for 4,800,000. The application point in B is the one that costs nothing at signature and cannot be made afterwards: a 25-year loss compensated with 12 years of debt-service relief is short by construction, however generous the headline.


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


**12.2-E** `[12.2.2 · Evaluation]` The commercial team has agreed a take-or-pay floor of 90 % of capacity — a `DSCR` of 1.0935 against a 1.20× covenant that requires 95.8892 % — and asks the finance lead to sign it off so the bid can go in. The soundest professional response is:

- A. sign it off: 90 % is a strong commercial outcome and a covenant can be reset at close
- B. decline to treat the floor as a commercial term, restate it as a financing constraint with its derivation attached, and price the alternatives — 3,727,752 of additional equity, a compensating floor price, or a volume-shortfall payment that is take-or-pay under another name ✅
- C. decline the transaction, since any floor below 100 % is unbankable
- D. sign it off and rely on the 1.15× lock-up trigger, which sits at 93.1245 % of capacity

*Rationale:* The floor is a financing deliverable computable before anyone sits down: each point of contracted volume is worth 0.0181 of coverage, so 372,438 of headroom buys 4.11 points and not ten (12.2.2). A concedes a breach and an automatic distribution lock-up from the first test date, on a plant performing exactly to specification and an offtaker performing exactly to contract. C is the opposite failure of judgment — the covenant requires 95.8892 %, not the sized case. D is self-defeating as well as misdirected: 90 % sits below the lock-up floor too, and a covenant breach is an event of default whether or not cash is trapped.


**12.2-F** `[12.2.1 · Evaluation]` A draft water purchase agreement has been reviewed by the commercial team, which reports the tariff, its indexation and the take-or-pay level as agreed and the deduction and abatement schedule as "operational detail for the O&M team". The financier's first intervention should be to:

- A. accept the division of labour: deductions are operational, and the O&M team is closer to the metering and quality regime than the finance function
- B. compute the maximum annual deduction the regime permits and compare it with covenant headroom — a deduction regime is a liability cap in reverse, uncapped and running annually, and on Kestrel a deduction exceeding **372,438** in any year breaches the 1.20× covenant however well the plant performed in every other respect ✅
- C. require the deduction regime to be capped at the covenant headroom figure, since anything larger is unbankable
- D. re-open the indexation schedule instead, since an indexation mismatch is the larger structural risk

*Rationale:* Of the four load-bearing terms of a revenue contract, the deduction regime is the one with no ceiling and the one that bites on revenue before any of the project's own protections engage, so its worst annual case is a financing number and not an operating one (12.2.1). A hands the covenant to a team that is not measured on it. C is the defensible weaker course and the right thing to *ask* for second: a cap on aggregate annual deductions is a legitimate negotiating position, and it is not achievable at the headroom figure in most markets, so computing the exposure has to come first. D names a genuine and larger risk that Domain 11 (KA 11.2.3) prices — and the indexation schedule is reported as agreed, while the deduction schedule has not been read by anyone with a covenant to protect.


**12.2-G** `[12.2.3 · Comprehension]` A grantor's adviser describes a compensation-on-termination formula measured on senior debt outstanding as one that "makes everybody whole". The accurate restatement is:

- A. it makes the lenders whole and pays equity nothing — it is a lender-recovery formula, and any return of, or return on, the equity base has to be provided for separately ✅
- B. it makes everybody whole, since equity ranks behind the debt and is paid from the same sum
- C. it makes equity whole and leaves the lenders exposed for their breakage costs
- D. it makes nobody whole, because debt outstanding is always less than the amount originally advanced

*Rationale:* The formula is measured on the debt and stops there: on a force-majeure termination at the end of year five Kestrel's lenders recover 27,965,695 and the sponsors' unreturned **11,128,176** is lost (12.2.3). That is why sponsors negotiate the definition of the equity base, whether a return accrues on it, and the treatment of subordinated debt. B assumes a residual the formula does not create. C reverses the ranking. D confuses amortisation with impairment — the sum tracks what is owed on the date, which is exactly what a lender needs. Whether any such formula is enforceable as drafted, and how it interacts with local public-procurement and insolvency rules, is a matter for qualified local counsel.


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


**12.3-D** `[12.3.2 · Evaluation]` A bankability memorandum states that Kestrel's EPC exposure is "fully covered up to the aggregate cap of 9,600,000". Risk-adjusted cover, on a 0.70 assessment of the parent guarantor, is 8,160,000 against a stress exposure of 12,255,674. The soundest reporting position is:

- A. the memorandum is right: 9,600,000 is the contractual cover, and a credit assessment is not contractual
- B. report exposure and risk-adjusted cover side by side, with the residue stated in currency and as a share of equity — 4,095,674, or 22.75 % of the 18,000,000 cheque — and the 0.70 recorded as a dated range owned by the credit function ✅
- C. report the nominal residue of 2,655,674 only, since a probability of payment is speculative
- D. raise the assumed probability to 0.85 to reflect the size of the parent's balance sheet

*Rationale:* Netting cover against exposure without stating both is the reporting failure this Knowledge Area exists to prevent, and it is the residue that invites the questions which improve the package — a larger bond, a bank guarantee in place of the parent, an uncapped indemnity for defined heads, or a smaller stress accepted with eyes open (12.3.2, 12.3.4). A treats a promise as a recovery. C discards the credit dimension altogether, when even the single-point 0.70 already understates the problem by concealing that distress and non-payment are correlated. D answers a correlated exposure with a more confident number rather than a structural response: at 0.70 one dollar of unconditional bank cover is worth 1.4286 dollars of parent guarantee, so the remedy is an unconditional instrument or a larger face amount, not a kinder assumption.


**12.3-E** `[12.3.2 · Evaluation]` The contractor offers to replace the 4,800,000 on-demand bank bond with a parent company guarantee of **5,500,000**, pointing out that the face amount is 700,000 higher and that the bond's fee of **172,800** over the construction period is priced into the contract sum in any event. The credit function assesses the parent at a 0.70 probability of paying in full. The response should be:

- A. accept: a larger face amount from a substantial group is better cover than a bank instrument, and the fee saving is real money
- B. accept, provided the guarantee is drafted in on-demand form
- C. reject as offered: at 0.70 the guarantee is worth **3,850,000**, which is **950,000** less than the bond, and the equivalent face is 6,857,143 — accept at that face or keep the bond ✅
- D. reject: a parent company guarantee is never acceptable in place of bank cover

*Rationale:* Cover is face amount multiplied by the probability of payment, so the comparison is 3,850,000 against 4,800,000 and the fee saving is less than a fifth of the 950,000 being given up (12.3.2). B is the defensible weaker answer and the instructive one: demand form addresses **conditionality and timing** — money in ten days rather than two years — and leaves the obligor's **credit** exactly where it was, so a 5,500,000 on-demand parent guarantee is still worth 3,850,000. A confuses balance-sheet size with payment certainty. D states a rule the arithmetic does not support: the guarantee is acceptable at 6,857,143 or above, which is what converts an argument about instrument preference into a priced trade. Whether an instrument is callable as modelled, and the effect of amendments to the underlying contract on a guarantor's liability, are questions for qualified counsel in the governing jurisdiction.


**12.3-F** `[12.3.1 · Comprehension]` A performance bond and a parent company guarantee of the same face amount differ in the way a financier prices them because:

- A. they differ only in cost, the bond carrying a fee and the guarantee none
- B. an instrument is priced on three separate attributes — how much (face amount), how certain (the obligor's credit and the conditions on payment) and how long (expiry against the exposure it covers) — and the two instruments are alike only on the first ✅
- C. a bond is security over the asset while a guarantee is a contractual promise
- D. a guarantee covers the defects-liability period and a bond does not

*Rationale:* Face amount, certainty and duration are three independent questions, and the third is the most frequently missed: a bond expiring at provisional completion does not reach the defects-liability period, and a guarantee expiring on a fixed date rather than on discharge of the underlying obligation is a gap in the stack with a date on it (12.3.1). A reduces three attributes to one — and the fee is the price of the certainty, not an extra. C confuses credit support with the lenders' own asset security. D asserts as a rule what is a drafting question in each instrument.


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


**12.4-E** `[12.4.3 · Evaluation]` A claims policy states that the organisation "never settles above its own assessment" — 1,050,000 on a disputed sum of 1,520,000, where the present value of fighting is 1,347,115 and the settlement ceiling 84.68 % of the disputed sum. The soundest professional position is:

- A. keep the policy: paying more than the merits justify rewards an inflated claim
- B. keep it only as a stated and quantified choice — it costs 527,115 against a midpoint settlement — and confirm the cost-shifting position with counsel first, because it can move the arithmetic by more than the disputed sum ✅
- C. settle at the ceiling of 1,287,115, since the arithmetic identifies the rational price
- D. arbitrate: a 0.40 probability that the project company's own assessment is upheld is a strong position

*Rationale:* Own costs of 800,000 are certain and immediate while the award is contingent and 26 months away, so the process rather than the merits dominates the answer; holding to the assessment is a legitimate choice — usually for precedent across a portfolio of similar claims — provided its price is computed and stated (12.4.3). A defends a principle without pricing it. C mistakes an indifference point for an opening position: the ceiling is a maximum, and settling at the midpoint saves 527,115. D reads a probability as a position — on these assumptions fighting beats a midpoint settlement only once the disputed sum exceeds 6,901,234.


**12.4-F** `[12.4.1 · Comprehension]` A risk allocation matrix differs from the priced risk register in that:

- A. it is the same document at a coarser level of aggregation
- B. the register records each risk's probability, impact and owner, while the matrix maps each allocation to the clause that effects it, the financial limit and the instrument standing behind that limit — which is why only the matrix reveals an orphan or a doubly covered risk ✅
- C. the register is the legal document and the matrix the commercial one
- D. the matrix replaces the register once the contracts are signed

*Rationale:* An allocation with no clause is an intention, and one cap committed to three heads of claim is one cap and not three; both defects are invisible in a register whose only allocation column is an owner's name (12.4.1). A misses that the matrix adds columns the register does not have. C reverses the character of both documents. D discards the quantification the register carries and its continuing role in sizing contingency and reserves.


**12.4-G** `[12.4.2 · Evaluation]` Kestrel's EPC contract fixes the contractor's delay liability at **20,000 per day** and leaves the project company's exposure to prolongation on an owner-caused delay to be proved after the event; on the claim of 12.4.3 it is asserted at **12,500 per day** of site overhead before disruption and additional plant. Variations are valued on a cost-plus basis where the schedule of rates is silent, and changes below a consent threshold need no lender approval. With the change mechanics still open, the priority is to:

- A. reduce the contractor's daily damages rate to 12,500, so that the regime is symmetrical
- B. pre-agree the daily prolongation rate on the same evidence as the delay damages rate, and add a cumulative consent threshold alongside the individual one — the asymmetry is **7,500 per day** of quantification argument, and serial changes below a threshold are how a fixed-price contract stops being fixed ✅
- C. remove the cost-plus fallback and require all variations to be valued at contract rates
- D. accept the mechanics: prolongation is proved on actual cost, which is the fairest measure available

*Rationale:* Change mechanics decide who funds a change and whether the funding exists, and a pre-agreed prolongation rate removes the largest single area of claim-quantification argument — which is why it belongs beside the delay damages rate and on the same evidence (12.4.2). A achieves symmetry by weakening the project's own recovery, which is the wrong direction from a rate that already recovers only 80.86 % of the daily economic cost of delay. C is the defensible weaker course and only protective if the schedule of rates is complete: where it is not, contract rates simply relocate the argument, so the ask is a complete schedule with a defined route for genuinely new work. D mistakes a measurement principle for a mechanism — proving actual cost after the event is precisely the exercise the pre-agreed rate exists to avoid, and it is conducted while the covenant is being tested.


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


**13.1-E** `[13.1.4 · Evaluation]` Kestrel's diligence envelope is twelve weeks, set by the environmental and social stream; the next-longest streams are legal at ten weeks and market at nine. Delay costs 124,133.33 a week. The environmental and social consultant offers to compress its stream to nine weeks for a 200,000 acceleration fee. The sponsor should:

- A. accept — three weeks at 124,133.33 is 372,399.99 against a 200,000 fee, a clear gain
- B. accept, but recognise that the envelope then falls only to ten weeks on the legal stream, so the acceleration buys two weeks (248,266.66) for 200,000 — a thin 48,266.66, and worth pairing with legal compression before the fee is agreed ✅
- C. decline — 200,000 exceeds the 150,000 that a ten per cent reduction across the whole 1,500,000 fee book would save
- D. decline — compression off the binding stream returns nothing

*Rationale:* The envelope is the maximum of the stream durations, so compressing the binding stream returns weeks only until the second-longest stream becomes binding: `max(8, 5, 10, 6, 4, 9, 9) = 10`, giving `2 × 124,133.33 = 248,266.66` and a net **48,266.66**, not the 372,399.99 A assumes by crediting all three weeks. C compares an acceleration purchase against a fee negotiation as though the two were alternatives when both are available, and prefers the smaller saving. D applies 13.1.4's rule to the wrong stream — environmental and social *is* the binding stream; the rule bites on the legal ceiling, which is why the recommendation is conditional rather than negative.


**13.1-F** `[13.1.1 · Comprehension]` Which restatement most closely captures the test 13.1.1 sets before any diligence stream is commissioned?

- A. every discipline should be reviewed to a comparable depth, so that the file is complete
- B. a stream is worth commissioning when a different answer would change a decision, and that decision is worth more than the stream costs ✅
- C. a stream is worth commissioning when the adviser will accept liability for its conclusions
- D. a stream is worth commissioning when the lenders require it as a condition precedent

*Rationale:* Diligence buys information, and information has value only where a decision turns on it (13.1.1). A is the comfort purchase the topic warns against — uniform depth is a filing standard, not a value test; C confuses reliance with information value, and 13.A.1 shows the cap covers a small fraction of the exposure in any case; D is a real procurement fact for several streams, but 13.1.3 notes the arithmetic then argues about timing and scope rather than about whether to run the stream.


**13.1-G** `[13.1.5 · Evaluation]` During diligence a demand study commissioned early in development is superseded by a second study with a lower central case. The sponsor's development team proposes removing the first study from the data room, on the ground that no party relies on it and its presence will only raise questions the second study has already answered. The soundest professional position is that the first study:

- A. should be removed — a superseded document is not a disclosure, and leaving it in invites questions about a forecast nobody relies on
- B. should stay, marked superseded with a date and a note, and appear on the numbered disclosure index — because the index later determines what the lenders were told, and the trail is the evidence base for the first covenant dispute, warranty claim and refinancing ✅
- C. should stay unmarked, since both studies carry dates and a reader can see which is later
- D. should be moved to a folder visible only to the sponsor's own advisers, so that it remains available internally without being disclosed

*Rationale:* Version control with supersession **marked** rather than deleted is the discipline, and removal is the worse of the two failures because the trail disappears (13.1.5) — a superseded forecast left in a room without a supersession note is what Domain 6 (Case study B) priced. A treats disclosure as a presentational choice, and it is the choice a challenged representation will later be tested against. C leaves two live-looking central cases and no record of which was relied on. D is the same act as A with a record that the omission was deliberate.


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


**13.2-E** `[13.2.3 · Evaluation]` Two Class 1 findings are almost identical in size: the definitional and convention findings together resize the facility by **2,505,646**, and the funding plan omits the **2,504,818** debt-service reserve — a difference of 828. The credit paper can lead with one. Which is the more decisive finding, and why?

- A. the resize, because 2,505,646 is the larger number
- B. the unfunded reserve, because a coverage shortfall is negotiated between the parties whereas a funding plan that does not fund a mandatory reserve produces a failed condition at first drawdown or a first-year breach ✅
- C. neither, because at 828 apart they very nearly cancel
- D. the resize, because `CFADS` is the model's most important line

*Rationale:* Class 1 findings are ranked by the kind of consequence they create, not by magnitude: one changes a negotiating position, the other creates an event the documents will treat as a failure (13.2.3). A ranks by size where the two sizes are indistinguishable anyway. C is the netting fallacy 13.2.2 forbids, applied more crudely still — these findings do not offset, they are simply similar in scale, and one of them is not a cash amount at all. D substitutes a slogan for the consequence test.


**13.2-F** `[13.2.2 · Comprehension]` A colleague new to model audit asks why the auditor will not report a single "findings closed" percentage. The best short explanation is:

- A. the auditor prefers not to be measured on a number it does not control
- B. the three classes are not commensurable, so one percentage adds findings that change the transaction to findings that change nothing; progress has to be measured by impact closed ✅
- C. the percentage is never final, because new findings keep arriving until the model is frozen
- D. the percentage understates progress, because Class 3 findings are the quickest to close

*Rationale:* The objection is to combining unlike things, and the remedy is class-weighted closure (13.2.2). A imputes a motive where there is a measurement defect; C describes a practical nuisance that would apply equally to a class-weighted metric; D inverts the direction — Class 3 findings being quick and numerous is exactly what makes a count *overstate* progress, as "31 of 34, 91.2 %" does.


**13.2-G** `[13.2.4 · Evaluation]` A finding moves year-one `CFADS` by **180,000** — **0.0359×** of `DSCR`, against the stated materiality threshold of 0.01×, about **50,096** of annual `CFADS` — and the sponsor disputes it, arguing the auditor has misread the clause. The auditor does not agree. The deal team proposes recording it as "closed — no adjustment required", so that the register shows nothing outstanding at close. The soundest treatment is to record it:

- A. as closed, since no adjustment was made and nothing further is to be done
- B. as **accepted** rather than corrected, with its quantified effect, the written rationale, the name of the person accepting it, and its disposition listed on the auditor's final letter ✅
- C. as withdrawn, because a finding the auditor cannot substantiate against the sponsor's reading of the clause is not a finding
- D. as escalated to the credit committee, which alone can accept a finding the parties cannot resolve

*Rationale:* Findings are sometimes accepted rather than corrected, and an accepted finding with a written rationale is professional whereas the same finding recorded as "closed" is not (13.2.4). A produces a register that misdescribes the position to every later reader — the defect Case study B shows costs money two years afterwards. C lets the party whose reading is disputed close the item. D is the right route for a Class 1 finding, but this one changes no conclusion — `DSCR` after it is **1.2384**, still clear of the 1.20 covenant, so it is Class 2 — and routing it upwards substitutes a governance step for the record the register exists to hold.


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


**13.3-F** `[13.3.4 · Evaluation]` Kestrel's headline rate is 6.00 %; netting the 544,000 of lender fees from proceeds gives an all-in **6.2386 %**, and netting the whole 2,709,000 close-cost budget gives **7.2376 %**. The sponsor is choosing between this bank facility and a bond alternative that would require substantially the same diligence, sponsor-side legal and perfection costs. Which rate belongs in that comparison?

- A. 6.00 %, because the other two include amounts that are not interest
- B. 6.2386 %, because only costs that differ between the routes discriminate between them, and the diligence and sponsor-side costs are common to both ✅
- C. 7.2376 %, because it is the full cost of raising the money and the more conservative figure to put to a board
- D. all three, because each is correct and answers a different question

*Rationale:* A route comparison is decided by the costs that differ, which is Domain 9's rule (KA 9.3.1) applied to close costs: the lender-payable 544,000 differs between a bank and a bond route, the 2,165,000 of adviser and perfection cost largely does not (13.3.4). C is defensible and commonly seen, but conservatism is not a selection rule — loading both routes with the same 2,165,000 changes the level and not the ranking, while inviting the reader to treat a sunk common cost as a reason to prefer one lender over another. A is wrong because a fee paid to the lender for its money is part of the price of that money. D is true as a general statement and evades the question asked, which is a decision.


**13.3-G** `[13.3.2 · Evaluation]` Kestrel's conjunction gives a base close of **20 weeks**, `E[close]` of **22.4075 weeks**, and a close-date distribution of **34.125 %** at 20 weeks, **11.375 %** at 22 and **54.500 %** at 24. The EPC contractor is holding a price and the grantor is sequencing its own approvals; both have asked for the close date. The soundest thing to tell them is:

- A. 20 weeks — the base date every chain owner is committed to, because publishing a later date is how a later date is achieved
- B. the distribution — roughly one chance in three of 20 weeks and better than one in two of 24 — because a single date is a forecast the transaction knows to be 34 % likely, the counterparties' own decisions turn on which outcome they should plan for, and the modelled independence of the chains makes even that distribution optimistic ✅
- C. 22.4075 weeks, the expected value, because it is the unbiased single figure and a single figure is what was asked for
- D. 24 weeks, the most likely single outcome, because under-promising protects the transaction and the parties relying on it

*Rationale:* The distribution is bimodal — **88.625 %** of the probability sits at 20 or 24 weeks and almost none near the mean — so C is arithmetically respectable and describes an outcome that will almost certainly not occur. A states as a plan a date known to be 34.125 % likely. D is defensible as a commitment and conceals a one-in-three chance of being four weeks early, which is exactly what a contractor holding a price and a grantor sequencing approvals need to know; it also presents as near certain a figure whose own basis — independent chains, one slip each — understates the right tail (13.3.2).


**13.3-H** `[13.3.1 · Comprehension]` Which restatement best captures why a close timetable treats **third-party** conditions differently from **corporate and authority** conditions?

- A. third-party conditions are more numerous, so they consume more of the timetable
- B. the two groups differ in who controls satisfaction: corporate conditions sit inside the sponsor group and fail through inattention, so effort recovers them, whereas third-party conditions sit with permitting authorities, offtakers and grantors and can be influenced but not compressed at will ✅
- C. third-party conditions matter more to the lenders, so they are evidenced to a higher standard
- D. corporate conditions may be waived by the lenders whereas third-party conditions may not

*Rationale:* The five groups are distinguished by **how each fails**, and the corporate/third-party distinction is one of control rather than of importance or of evidentiary standard (13.3.1) — which is why third-party conditions are the standard source of close slippage, and why chain B, the permit and land chain, carries the largest slip risk in Kestrel's register. A may be true of a given transaction and is not the reason. C confuses a condition's ownership with its evidence requirement. D invents a rule: waiver is a lenders' decision available in principle to any condition, and a waived CP is not a satisfied CP whichever group it came from.


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


**13.4-F** `[13.4.4 · Evaluation]` The arranger offers to cut the arrangement fee from 1.20 % to 0.95 % of the 42,000,000 facility — a certain saving of 105,000 — in exchange for lifting the market- flex cap from 50 to 60 basis points. Twelve years, `AF(0.06, 12) = 8.383844`; the 1.20× covenant survives a rate rise of 111.38 basis points. The recommendation is:

- A. accept — 105,000 is certain, and flex may never be exercised at all
- B. reject — the extra ten basis points of flex is worth 352,121.45 in present value, 3.3535 times the fee saved, and it takes the cap from 44.89 % to 53.87 % of the covenant headroom ✅
- C. reject — flex should not be granted at all, since it prices the sponsor's protection away
- D. accept, provided the fee reduction is credited in cash at close rather than netted from proceeds

*Rationale:* `42,000,000 × 0.0010 × 8.383844 = 352,121.45` against 105,000, so the trade would have to be exercised with probability below `105,000 ÷ 352,121.45 =` **29.82 %** to break even — and 13.4.3 shows the arranger exercises precisely when the book is slow, which is the state in which the sponsor can least afford the coverage loss (13.4.4). A is the defensible version of the argument and is the one sponsors actually make; it fails because it prices a contingent claim at zero and ignores the 111.38 basis points of headroom being consumed. C is not available — an arranger without flex will either decline to underwrite or price the underwriting as if flex were already exercised. D negotiates the mechanics of the wrong side of the trade.


**13.4-G** `[13.4.1 · Evaluation]` Kestrel has a long-stop date, an EPC contractor holding a price and a grantor's fixed timetable. An arranger offers a **best-efforts** mandate at a materially lower fee than the underwritten alternative, observing that books for contracted water projects of this size rarely fail to fill. The itemised close-cost budget is **2,709,000** — **6.45 %** of debt raised — of which only **544,000** is payable to lenders. The soundest recommendation is to:

- A. accept best-efforts, because the fee saving is certain while the risk it transfers has a low probability
- B. reject best-efforts here: what it transfers to the borrower is the risk that close does not happen at all after the whole 2,709,000 has been spent, of which **2,165,000** is irrecoverable third-party and sponsor-side cost, and certainty of funds against a fixed timetable is what underwriting economics buy ✅
- C. accept best-efforts and manage the exposure by bringing the long-stop date forward
- D. club the facility instead, since a club carries no distribution risk and no underwriting fee, and no arranger then has an economic interest in flexing terms

*Rationale:* The three routes differ in **who carries the risk that the market does not turn up**, and a transaction with a hard timetable is buying certainty (13.4.1). A prices a contingent claim at its probability and ignores its size: the budget at risk is **5.375 times** the 504,000 arrangement fee, and it is spent before the book is known to fill. C tightens the very constraint the sponsor is trying to satisfy. D is the genuine alternative and the right answer for a repeat sponsor with relationship banks and no hard date; it fails here because a club must be assembled before documentation and each bank commits only its own final hold, so it does not deliver certainty of funds against a fixed date — and the flex point, though correct, is a benefit the sponsor cannot collect if the group does not assemble in time.


**13.4-H** `[13.4.2 · Comprehension]` In a syndicated facility the **praecipium** is retained exclusively by the arranger while the **participation fee** is paid pro rata on final allocations. Which statement best explains the difference?

- A. the split is a market convention with no economic content, which is why it is not disclosed to borrowers
- B. the two fees pay for different things: the praecipium prices structuring, documentation and standing behind the whole amount before any other lender had agreed to lend, while the pool prices the capital each lender ultimately commits ✅
- C. the praecipium compensates the arranger for the margin it forgoes by selling part of its position down
- D. the praecipium is the arranger's own pro-rata share of the pool, computed on its final hold

*Rationale:* The praecipium prices work and risk only the arranger bears; the pool prices committed capital, which every lender provides — which is why the arranger's yield on its 15,000,000 hold is **1.650 %** against participants' **0.950 %**, a differential of **70.0 basis points** (13.4.2). A denies the economics and misstates practice: the total fee is disclosed, and the split is information a sponsor should ask for, because a large praecipium signals an arranger paid mostly for underwriting and therefore one that will reach for flex. C describes something that does not happen — a lender selling down transfers the margin with the position. D is the pool fee itself, which the arranger receives **in addition** to the praecipium.


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


**14.1-E** `[14.1.3 · Evaluation]` The facility agreement's funding-order sub-clause is still open. Equity-first capitalises **1,338,006** of interest against **2,114,597** pro rata and **2,804,070** debt-first; at a fixed 60,000,000 envelope, equity-first re-solves the balancing contingency from 3,645,403 to **4,388,050**. Deferring the equity cheque is worth **183,013** of present value to the sponsor at 8 %. What should the transaction lead recommend to the sponsor's board?

- A. debt-first — it is worth 183,013 of present value to the shareholders, which is the only party the board represents
- B. equity-first — it converts the 776,591 of interest saving into 742,647 of additional funded contingency, 9.14 % of the EPC price against 7.59 %, provided the sponsor can carry the larger irrecoverable exposure if the works are abandoned early ✅
- C. pro rata — it is neutral between the parties and is what the lender's draft provides
- D. debt-first — capitalised interest is not a cash cost during construction, so the order is presentational

*Rationale:* At a fixed envelope the sequencing buys funded protection with no additional money, which is the form of the argument a credit committee and a board can both act on (14.1.3) — and the condition attached to it is the real one, because equity-first maximises the sponsor's exposure to an abandoned works. A is defensible and is what sponsors argue: it is weaker because 183,013 of present value is bought for 689,473 of nominal cost that the sponsor itself then funds as additional equity, so it is only rational at a sponsor cost of capital high enough to justify the trade — a test the recommendation must state rather than assume. C accepts a settlement in place of an analysis. D is false: capitalised interest is drawn, enters the debt balance and is repaid with interest over the loan life.


**14.1-F** `[14.1.1 · Comprehension]` Which statement best restates why the restated sources-and-uses statement is a **test** where the financial-close statement is only an **identity**?

- A. the close statement was reviewed by the model auditor and the restated one is not
- B. at close one line is solved so that the columns agree, whereas at a data date the two columns are built from independent sources — the agent's records, certified progress and a commitment schedule — so a gap between them carries information ✅
- C. the restated statement replaces forecasts with actual costs
- D. the restated statement adds remaining contingency as a source, which the close statement omits

*Rationale:* An identity can always be satisfied by choosing the balancing line; a test can fail because nothing is free to move (14.1.1). C is only partly true and misleads on the important part — three of the four lines in Kestrel's remaining column are still forecasts. D describes the double-count 14.1.1 exists to forbid: contingency is a *use* funded by the same commitment.


**14.1-G** `[14.1.2 · Evaluation]` The quarter-five draw request must repeat the representation that the project is **in balance**. The certified cost report supporting it shows the columns reconciling; the finance team's own rolling funding forecast, issued internally the same week, shows a prospective shortfall of **1,927,740** on a commitment basis. The soundest course is to:

- A. sign the request on the certified cost report, because a representation is made on certified information and a forecast is not certified
- B. not sign the in-balance representation on a basis the team knows to be superseded: disclose the forecast position to the agent, name the cure and the party funding it, and submit the request with the cure — because the cure is dated by the next draw and not by the reporting date ✅
- C. sign the request now and disclose the forecast position in next month's report, when the cost report will have caught up with it
- D. withhold the draw request altogether until the cost report has been reissued on the forecast basis

*Rationale:* "In balance" and "no potential default" are the two limbs of a draw request that are judgments rather than facts, and both are certifications by named officers with consequences attached (14.1.2); a certification cannot rest on a basis its signer knows to be out of date. A treats a representation as a document-assembly step. C is the commonest course and puts the misstatement and the delay together — the shortfall surfaces a month later, having consumed part of the time available to raise the money (14.2.3). D avoids the misstatement by converting a disclosure problem into a schedule problem at 415,000 a month of funded cost and 947,000 of economic cost (14.4.1); the shortfall is **4.6452 months** of that funded cost, so the delay route can consume the whole amount in a quarter and a half without curing any of it.


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


**14.2-E** `[14.2.2 · Evaluation]` A certifier reports contingency coverage on the remainder of **0.2893** — 1,500,000 against 3,420,000 of known committed claims plus a 1,764,289 P80 on the open register — and concludes: "coverage is thin; we recommend monthly monitoring of the contingency position." Should that conclusion be accepted?

- A. yes — 0.2893 is thin, and closer monitoring is the proportionate response to a thin ratio
- B. no — a ratio below one means contingency is already fully committed and 3,684,289 short, so the finding is a funding requirement, corroborated by the independently derived 1,927,740 in-balance shortfall; monitoring a known deficit does not fund it ✅
- C. no — the 3,420,000 of known claims does not belong in the denominator, and on open risk alone coverage is 0.8502
- D. yes, provided the ratio is recomputed at every data date with its denominator itemised

*Rationale:* `1,500,000 − 5,184,289 = −3,684,289`: the number does not describe a margin that is narrow, it describes protection that has run out, and the in-balance route reaches the same conclusion from different inputs (14.2.1, 14.2.2). A and D are the defensible-sounding responses and both fail for the same reason — they answer a funding event with a reporting action. C is the argument the sponsor will make and 14.2.2 disposes of it: approved variations, assessed claim exposure and a bottom-up re-estimate are not risks that might happen, they are claims that have.


**14.2-F** `[14.2.1 · Comprehension]` A project controller's cost report and the lender's monitor produce cost-to-complete figures differing by 3,536,674 on identical underlying data. The explanation a board should be given is:

- A. one of the two contains an error, and reconciliation will identify which
- B. each discipline is structurally blind to items the other counts — earned value cannot see capitalised interest, approved but unbaselined variations or claim exposure, while a commitment basis cannot see cost-performance trend on scope that is fixed-price ✅
- C. the monitor has applied a more conservative contingency allowance
- D. the difference is the unallocated contingency balance

*Rationale:* `840,000 + 1,260,000 + 1,436,674 = 3,536,674`, or 14.34 % of the lender's number, and every line of it is outside the control accounts by construction rather than by anyone's error (14.2.1). A is the instinct the bridge exists to correct. C invents a judgment difference where the difference is one of scope. D names a line that appears in neither cost-to-complete.


**14.2-G** `[14.2.1 · Evaluation]` The lender's cost-to-complete of **24,656,674** exceeds available commitment of **22,728,934** by **1,927,740**. The sponsor disputes two lines: the **1,260,000** of assessed exposure on notified but unagreed claims ("worth far less, and unagreed in any case") and the **2,400,000** bottom-up re-estimate of remaining owner-retained scope ("pessimistic"). Both arguments may have merit. The soundest treatment in the monitoring report is to:

- A. adopt the sponsor's figures, since the SPV knows its own contracts and its own scope better than the monitor does
- B. hold both lines at the assessor's figures, name the assessor, and record the sponsor's case and its basis beside them — because the disagreement is about the quantum of an exposure rather than about whether it exists, and a report that omits a line instead of arguing about it cannot be relied on by anyone ✅
- C. exclude the claim exposure until the claims are agreed, since an unagreed claim is not yet a liability, and hold the owner-scope line
- D. publish two cost-to-completes, the monitor's and the sponsor's, and let the lenders choose between them

*Rationale:* The choice of basis is a negotiation rather than a technique, and both sponsor arguments may be right — what cannot survive is a report that drops the lines rather than stating them with their basis and their assessor (14.2.1). C is the accrual instinct, and it is arithmetically insufficient as well: removing the 1,260,000 closes **65.36 %** of the gap and still leaves the project **667,740** out of balance. A hands the assessment to the party that will fund the cure. D looks even-handed and evades the purpose of a monitoring report, which is a number a drawing can be conditioned on; two numbers and no recommendation is the same omission as one number with a line missing.


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


**14.3-E** `[14.3.1 · Evaluation]` The contractor asks for **690,000** of membrane racks, paid for and stored at the vendor's works, to be certified on a cost-incurred basis. The SPV's finance team supports the request, noting that the only consequence is a small amount of additional capitalised interest. Reviewing the request, what should be challenged first?

- A. the additional capitalised interest, which the contractor rather than the SPV should bear
- B. whether vesting of title in the SPV, identification and segregation at the vendor's works, and insurance in the SPV's name are all in place — the certification advances 458,850 of senior debt against goods in a third party's possession, which is a security question, not a cost question ✅
- C. the independent engineer's 92 % assessment of the incomplete milestone, since an assessed percentage is an opinion that drifts upward under schedule pressure
- D. nothing — the materials are paid for, identifiable and represent real value to the project

*Rationale:* `690,000 × 0.95 × 0.70 = 458,850` of debt advanced against work not in place, and absent vesting, segregation and insurance the money has bought a claim in someone else's liquidation (14.3.1). A is the finance team's own framing and is the specific error the topic names: pricing a security question as a cost question is how certification drift gets conceded, one certificate at a time. C raises a real and separate concern about the measured basis, but it is not what *this* request changes. D confuses the vendor's title with the SPV's.


**14.3-F** `[14.3.2 · Evaluation]` The contractor requests the contracted **10 %** advance payment — **4,800,000** on the 48,000,000 price, secured by an advance payment bond — and declines any reduction in the contract price. Re-running the funding model shows the advance costs **254,597** of capitalised interest: **5.30 %** of the advance and **0.53 %** of the EPC price. The SPV's finance lead proposes refusing the advance. The soundest position is to:

- A. refuse — an advance payment is a loan from the project to the contractor, and a project financing exists to fund the works rather than the contractor's balance sheet
- B. treat the 254,597 as the price of the advance and negotiate against it: where the contractor's own cost of funds exceeds the project's the advance creates value for the transaction as a whole, so the defensible outcomes are a price reduction of at least 0.53 %, a smaller advance, or an accepted and recorded cost — not a refusal on principle ✅
- C. accept — the advance payment bond covers the amount advanced, so the SPV carries neither cost nor exposure
- D. accept, and recover the 254,597 from the delay-damages account if the contractor completes late

*Rationale:* An advance is a priced loan, and pricing it converts a posture into a position the contractor can check and argue about honestly (14.3.2). A is the reflex and forgoes a genuine joint gain wherever the contractor's marginal cost of funds is the higher of the two. C confuses a bond, which secures **recovery** if the advance is not earned, with the **carrying cost** of having advanced it — the 254,597 is incurred whether or not the bond is ever called, and it is **4.068 times** the 62,589 that retention saves in the other direction. D applies damages calibrated to delay against a cost caused by a payment term, and helps itself to a cap Domain 12 reserves for a different head.


**14.3-G** `[14.3.1 · Comprehension]` Setting the amounts aside, which statement best explains why **milestone** certification is the basis most closely aligned with the lenders' security?

- A. it produces the smallest certified figure, so the least money leaves the account
- B. a milestone has either been achieved or has not, so certified value corresponds to a completed thing the security attaches to; a measured percentage is an opinion, and cost incurred may be goods in a third party's possession ✅
- C. it is the only basis an independent engineer is professionally qualified to certify
- D. it removes the need for retention, since nothing is certified until it is complete

*Rationale:* The three bases differ in **what they measure** — an achieved contractual event, an assessed percentage, and the contractor's spend — and only the first is binary and contractually defined, which is why 92 % of a milestone is worth exactly nothing and why that harshness *is* the alignment (14.3.1). A states a consequence rather than the reason, and a smaller figure is not by itself a virtue. C is false: certifiers apply all three, and the measured basis is the standard one on long linear works. D confuses the certification basis with retention, which secures defect rectification and is withheld on any basis.


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


**14.4-E** `[14.4.2 · Evaluation]` The term sheet provides a calendar-fixed first repayment date. On Kestrel's figures the 1.20× covenant fails beyond **0.7001 months** of slip, and the delivery team regards a slip of up to four months as a live risk. Four amendments are available and the sponsor can press for one. Which should be pressed first?

- A. size the debt service reserve on the four-month slip scenario rather than on the six-month convention
- B. set the first repayment date by reference to the actual commercial operations date, with a long-stop — it removes the mismatch between the test period and the obligation period instead of funding its consequences ✅
- C. rely on delay damages being creditable to `CFADS`, which at a four-month slip is worth 0.4791 of coverage and turns a breach into a 1.3286 ratio
- D. accept the calendar-fixed date and request a covenant waiver if the slip occurs

*Rationale:* The defect is that the first test period contains less operating time than the obligation it is tested against, and only B removes it (14.4.2). A is defensible and cheap, but a reserve pays an instalment — it does not raise `CFADS`, so the covenant still fails at 0.7001 months and the reserve must then be replenished ahead of any distribution. C is genuinely valuable and worth negotiating alongside B, but it is a protection that expires: once the 4,800,000 cap binds at eight months the damages line stops growing while `CFADS` keeps shrinking, so the damages-credited ratio peaks at 1.3829 and re-crosses 1.20 at 9.7226 months. D is the worst available course — a waiver requested inside a delayed project is read as news about the project.


**14.4-F** `[14.4.1 · Evaluation]` The works are a month late at the commercial operations date. The contractor's negotiator argues that the contracted **20,000 a day** — **600,000** a month — is generous, because it recovers **144.58 %** of the **415,000** the project must actually raise while it is not yet earning. The soundest response is to:

- A. accept the point — a damages rate that over-recovers the cash the project must raise is by definition adequate, and the drawdown and in-balance tests confirm it
- B. reject the framing: the funded cost excludes the **532,000** a month of forgone `CFADS`, so against the full economic cost of **947,000** the same rate recovers **63.36 %** — the signature of a project whose funding tests keep passing while equity value drains, which is the loss a damages rate exists to compensate ✅
- C. reject the rate on the ground that a recovery above 100 % shows the sum to be a penalty rather than a genuine pre-estimate of loss
- D. quote **80.86 %**, the recovery on interest plus forgone `CFADS`, as the neutral middle figure between the two positions

*Rationale:* One rate produces three recovery percentages, and the choice of basis is the whole argument: the economic cost is the correct calibration basis, while the drawdown and in-balance tests see only the funded one, which is why a delayed project can pass every test while destroying value (14.4.1). A mistakes a passing funding test for an absence of loss. C imports a legal conclusion the arithmetic cannot support — whether and how a contractual damages provision is enforceable is a question of the law governing the particular contract, on which this book states no jurisdiction's position, and a recovery above 100 % of *one* cost basis says nothing about the character of the sum. D is honest arithmetic on Domain 5's narrower basis and understates the monthly loss by the **205,000** of prolongation cost the SPV bears directly — **49.4 %** of the funded cost — so it is a figure to disclose, not the figure to negotiate on.


**14.4-G** `[14.4.3 · Comprehension]` Which statement best describes what a **buy-down** does and does not do when a plant completes on time at less than guaranteed output?

- A. it compensates equity for the output permanently lost, restoring the return the sponsors priced
- B. it is a lump sum, usually applied to prepay debt and calibrated so the financing survives a permanently smaller plant — repairing the lenders' coverage directly and equity's only through the headroom it restores; the equity return still falls, because the plant is smaller ✅
- C. it is a reserve the contractor funds against the possibility of future underperformance
- D. it replaces the delay-damages head once the commercial operations date has passed

*Rationale:* A buy-down is a **prepayment**, not compensation: it shrinks the debt so that a smaller `CFADS` still supports it, which is why the drafting question — restore the covenant level, or restore the base-case coverage — is worth the difference between the two amounts (14.4.3). A is the misreading that makes a buy-down look like a settlement of equity's claim. C describes a reserve, which is funded before an event rather than paid after a failed test. D confuses two heads of damages, each with its own sub-cap under one aggregate cap (Domain 12, KA 12.1.2).


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


**15.1-E** `[15.1.4 · Evaluation]` At the year-one test date the sponsors' own honest re-forecast of **5,500,000** gave a forward `DSCR` of **1.0979** and stopped a dividend of 774,364.77. Year two in fact delivered **5,963,894.11** — **463,894.11 above** the re-forecast. A director argues at the year-two board that the re-forecast should have been left unrevised. What is the sound response?

- A. the director is right — the outturn shows the re-forecast was wrong, so the distribution was wrongly withheld
- B. the withholding was correct on the information available and the outturn does not change that; the defence against a test that punishes candour is a documented re-forecast basis with a named owner and independent review, plus a forecast-to-outturn reconciliation in every pack ✅
- C. the director is right in principle — a sponsor should not be required to produce the forecast that stops its own dividend, and forward tests should be resisted in negotiation
- D. the re-forecast should be prepared on the lenders' model rather than the sponsors'

*Rationale:* A decision is judged on the information available when it was taken, and the honest answer to the perverse incentive is process, not a worse forecast (15.1.4). A is outcome bias, and accepting it institutionalises optimism in a number that has contractual force. C is a defensible negotiating position in the abstract but answers a question that is already closed — the test is in the documents, and it is what closed the four-quarter lag a rolling backward test cannot avoid. D moves the same unowned judgment to a different party's spreadsheet.


**15.1-F** `[15.1.2 · Comprehension]` Which statement best restates why a deteriorating project reports a covenant ratio *better* than its trading, and a recovering one *worse*?

- A. because depreciation does not move with revenue, so `EBIT` falls faster than `EBITDA`
- B. because receivables and payables move with revenue, so a decline releases working capital into `CFADS` in the year it happens and a recovery reabsorbs it in the year of recovery ✅
- C. because cash tax falls when revenue falls, so only eighty cents of each lost dollar reaches `CFADS`
- D. because the covenant is measured on a rolling window, which smooths a decline across four quarters

*Rationale:* The asymmetry is a working-capital effect and nothing else (15.1.2). C is true and is a different mechanism — the 0.80 cash-to-revenue gearing dampens movements in *both* directions symmetrically, so it cannot flatter one and penalise the other. D is a real lag (15.1.4) but describes when the test registers a change, not why the reported figure is flattered. A concerns an accrual measure, and the covenant is computed on cash.


**15.1-G** `[15.1.3 · Evaluation]` The operations director's monthly pack reports availability against the 95.0 % guarantee, and the finance director wants the covenant regime on it. Kestrel's thresholds bind at **1.9103 %** of `CFADS` (the 1.25× distribution condition) and **5.8339 %** (the 1.20× covenant), and translate into availability floors of **93.0161 %** and **88.9415 %**. What belongs on the operations dashboard?

- A. the 1.20× covenant in ratio terms, because that is the test the facility agreement contains and the one a default turns on
- B. both thresholds as availability floors — 93.0161 % and 88.9415 % — led by the distribution condition, because it binds first and availability is the quantity the operator actually controls ✅
- C. the volume tolerance of **1,862,188.62 m³**, **18.6219 %** of output, because volume is the larger revenue line
- D. the covenant headroom of **372,437.72** in cash, because cash is the unit the waterfall works in

*Rationale:* A trigger becomes a control only when it is expressed in the units of the person who can move it, and the translation runs ratio → cash → driver (15.1.3); leading with the distribution condition follows from its binding three times closer than the covenant. A hands an operations meeting a ratio it cannot act on. C is correctly computed and useless as a control: a 19 % volume loss is not a plausible consequence of membrane fouling whereas six availability points is, so ranking the drivers by revenue size ranks them by the wrong property. D is Domain 10's translation and stops one step short of the dashboard — it is the right figure for the finance pack and the wrong one for the operator.


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


**15.2-E** `[15.2.4 · Evaluation]` Two draft board papers describe the same six operating years. Paper 1 writes the **3,182,311.16** off as lost equity value. Paper 2 calls it a timing matter of no consequence, since **2,963,877.46** of distributions were ultimately paid. Which position is sounder, and what should the paper say?

- A. paper 1 — cash not received when it was due is value lost, and calling it timing is how distressed projects are misreported
- B. neither: the present-value loss is 2,709,838.79 — **53.90 %** of the entire 25-year equity `NPV` of 5,027,733.03 — so deferral is expensive without being destruction, and the paper should split the 3,182,311.16 into 1,682,311.16 of trading shortfall and a 1,500,000 capital call with different owners ✅
- C. paper 2 — the block account is pre-funding rather than a penalty, so on the facts nothing was lost
- D. paper 1, provided the loss is measured at the 6 % loan rate rather than the 8 % equity rate

*Rationale:* Both papers state a half-truth and the arithmetic disposes of each: 2,963,877.46 came back, so it is not a write-off, and 2,709,838.79 is more than half the project's whole equity value, so it is not immaterial (15.2.4). C quotes a correct description of the mechanism (15.2.2) to reach a false conclusion about value — pre-funding still costs the time value of money and, here, a 1,500,000 injection. D changes the discount rate to change the answer, which is the least defensible move available; the rate belongs to the board, not to the conclusion it wants.


**15.2-F** `[15.2.3 · Comprehension]` A shareholder asks how a project appraised at a 12.19 % `IRR` can pay a cash yield of only **4.3020 %**. The clearest explanation is:

- A. the appraisal was optimistic, and the cash yield is the figure to believe
- B. while the loan runs, debt service consumes 78.47 % of `CFADS`, so equity is the residual claimant on a leveraged asset; the same `CFADS` yields 32.13 % once the loan retires ✅
- C. the difference is the 600,000 maintenance-reserve charge, which the appraisal did not carry
- D. an `IRR` and a cash yield measure the same quantity on different bases, so one of the two has been computed incorrectly

*Rationale:* The gap is leverage and timing, not error or optimism (15.2.3). C names a real charge that is genuinely large against the dividend — 77.5 % of it — but cannot explain the shape: add it back and the yield is still only 7.6354 %. D is the misconception the topic exists to remove: a return over a whole life and a level annual yield are different measures, and both are correct. A treats a leveraged residual as evidence about the appraisal.


**15.2-G** `[15.2.2 · Evaluation]` A sponsor reviewing the term sheet argues for a distribution condition set at the **1.20×** covenant level rather than **1.25×**, on the ground that a test stricter than the covenant traps cash for no lender benefit. On Kestrel's outturn the base-case distributable amount is **774,364.77** a year, year one's was trapped in full, and year three's residual after debt service was **193,301.25** against a 600,000 maintenance-reserve charge — a shortfall of **406,698.75** drawn from the block account. Is the argument sound?

- A. yes — cash in shareholders' hands is worth more than the same cash later, the block account earns equity nothing, and the lenders already have a covenant
- B. not on these facts: the year-one dividend the 1.25× test trapped is what funded the 406,698.75 shortfall — **52.52 %** of the 774,364.77 — so a weaker test would have paid that cash out and then called it back as new equity; the block account is pre-funding rather than a penalty, and the real question is whether the sponsor would rather hold the cash or avoid the call ✅
- C. yes — a distribution condition and a covenant test the same ratio on the same cash, so the stricter of the two adds nothing
- D. no — 1.25× is a market standard for a contracted water project and is not negotiable

*Rationale:* Cash trapped by a distribution test is not lost but pre-committed, and here it paid for a restoration that ranks above distributions and has no negotiating partner (15.2.1, 15.2.2). A is the strongest form of the sponsor's case and names a real cost — deferral is expensive — but it prices the timing benefit while ignoring the capital call the same cash would have funded. C is the misconception the domain opens by correcting: the distribution condition is a **permission**, tested forwards as well as backwards, and it binds at 1.9103 % of `CFADS` where the covenant binds at 5.8339 %. D substitutes an assertion about the market for an analysis; nothing in a facility is un-negotiable at a price.


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


**15.3-E** `[15.3.2 · Evaluation]` The 7-year option at 4.45 % all-in is worth **(206,835.69)** against a breakeven all-in rate of **4.2206 %** — the market's offer is 22.94 basis points short. The treasurer proposes going back to the banks to press for a further **25 basis points** of margin, and nothing else. Assess that course.

- A. sound — 22.94 basis points is the entire gap and 25 basis points closes it, so the transaction becomes positive
- B. sound in arithmetic and the weaker course: at 4.20 % all-in the 7-year option is worth only about +18,542, while the same 4.45 % over 10 years is worth +799,481.12, so the negotiation belongs on tenor rather than on the last basis points of price ✅
- C. unsound — a refinancing with a negative net present value should be abandoned rather than renegotiated
- D. unsound — the breakeven rate depends on the discount rate chosen, so it cannot support a negotiating position

*Rationale:* A is arithmetically correct and is the trap: winning the price argument converts a small loss into a small gain and leaves 780,939 of tenor value on the table, because the margin saving is a thin flow on a rapidly amortising balance while the extension lends into the 1.9431 `PLCR` tail (15.3.1, 15.3.2). C generalises one priced structure into a rejection of the transaction. D is a real caution — the paper should state and test its rate — but the sign does not turn on it here, so it is a reason to disclose an assumption, not a reason to abandon a computable negotiating position.


**15.3-F** `[15.3.3 · Evaluation]` At the year-two test date the backward `DSCR` is **1.1905** against a 1.20× covenant; nine months later it is **1.0386**. The lenders will grant an amendment. The sponsor's adviser proposes resetting the covenant to **1.10×**, which clears the current ratio with margin and prices below a deeper reset. The soundest recommendation is to:

- A. reset to 1.10×, which the current 1.1905 clears with margin and which costs less in fee and margin uplift than a deeper reset
- B. reset to **1.00×** for two test dates before stepping to 1.10× and then 1.20×, because a reset sized on the current ratio is breached again at the next test's 1.0386 — a second breach, a second fee and a second negotiation from a materially worse position ✅
- C. reset to 1.00× for the remaining life, since a covenant that has been breached has been shown to be set too high
- D. take a waiver at each test date instead: at **55,307.03** a waiver is far cheaper than the **743,436.62** the amendment costs

*Rationale:* An amendment must be sized against the **stressed** case rather than the current one, and the deterioration was already visible — a 1.10× reset leaves a cash shortfall of **307,662.27** at the year-three test (15.3.3). A optimises the fee against the ratio in front of it, which is the same error as spending a cure on a marginal breach. C gives away the control permanently to solve a defined period of weakness, and no lender prices that kindly. D is right about a single date and wrong about a persistent profile: each waiver is a fresh consent sought from a worse position, and a deterioration that persists is not a waiver problem — the amendment Kestrel took resets four test dates.


**15.3-G** `[15.3.1 · Comprehension]` Which statement best explains why a refinancing opportunity exists in an operating project financing at all?

- A. market interest rates fall over time, so any long-dated facility eventually becomes expensive
- B. the facility was priced for completion and technology risks that the completion tests retired, while a term loan's price is fixed at signing; and the project life beyond the loan's maturity is lendable capacity that no margin reduction can reach ✅
- C. an operating project generates more cash than one under construction, so it can carry more debt
- D. lenders are required to reprice a facility once the independent engineer certifies completion

*Rationale:* The driver is a mismatch between a price fixed at signing and a risk profile that has changed, and the value has three sources — margin, tenor into the tail, and the covenant package — which is why a `PLCR` of 1.9431 against an `LLCR` of 1.2743 measures something a margin cut cannot deliver (15.3.1). A is sometimes true and is not the mechanism: a refinancing can pay in a rising market where the risk retired is large enough. C explains why the tail exists, not why the facility is mispriced. D invents an obligation — a refinancing is a new transaction, negotiated.


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


**15.4-E** `[15.4.2 · Evaluation]` A credit committee paper recommends the maturity extension on the ground that lender recovery is **100.0000 %** of the 34,073,997.28 outstanding, measured at the 6.00 % contract rate. Is that recommendation supported by the evidence it cites?

- A. yes — the present value of a longer annuity at the same rate returns the same principal, so recovery is par
- B. no — par at the contract rate is an arithmetic identity that holds however far the credit has deteriorated, so it carries no information; at the 7.00 % the lenders now require the extension recovers **95.0779 %**, a real loss of 4.92 points the paper does not disclose ✅
- C. no — the sponsor injection recovers 96.3549 % and should therefore be recommended instead
- D. yes, provided the paper also shows the restored `DSCR` of 1.2043 clearing the 1.20 requirement

*Rationale:* A states a true fact and mistakes it for evidence; because the identity is insensitive to the very deterioration the committee is being asked to approve, quoting it is what conceals the concession (15.4.2). C names the option lenders prefer but it is not theirs to grant — the sponsor rationally refuses, the injection costing 4,583,353.23 today for a saving worth 825,718.65 less. D adds a necessary condition and treats it as a sufficient one: coverage restored says the loan can be serviced, not what the lenders gave up to get there.


**15.4-F** `[15.4.4 · Evaluation]` The sponsors sell at the end of operating year eight to a holder requiring **7.50 %**, and the board paper reports the realised `IRR` of **12.3059 %** as evidence of operating performance against a hold-to-maturity **9.8591 %**. The soundest assessment of that paper is:

- A. it is sound — a realised `IRR` is the return the shareholders actually earned, and 12.3059 % is a fact
- B. it misattributes: a sale at the sponsors' own 8.00 % would have lifted the reported `IRR` to **11.7666 %** while creating exactly nothing, so the genuine gain from selling at 7.50 % is the **828,232.87** of yield compression — a market view — and it belongs in the paper separately from the **5,027,733.03** the asset itself created ✅
- C. it understates performance: the present value at 8 % of the 7.50 % sale is **5,855,965.90** against 5,027,733.03 on a hold, so the paper should claim the larger figure
- D. it is sound provided the buyer's required return is disclosed alongside the `IRR`

*Rationale:* A sale at the seller's own discount rate is a swap of equal value at a different date, so **1.9075** points of the uplift is early crystallisation and none of it is performance (15.4.4). A states a true number and lets it carry a claim it cannot support. C makes the mirror error to the paper's: the 828,232.87 is real value and it is a view about the capital market rather than about the asset, so claiming it as performance is the same misattribution in a larger figure. D is necessary and not sufficient — disclosing the rate does not separate the three things a board needs kept apart: the value the asset created, the compression, and the `IRR`, which explains neither.


**15.4-G** `[15.4.1 · Comprehension]` A covenant breach, financial distress and insolvency are three distinct conditions. Which statement distinguishes them correctly?

- A. they are the same condition at three degrees of severity, so the response differs only in urgency
- B. a breach is a test failure at a date, which is cured, waived or amended; insolvency is an inability to pay; distress is a forecast that cannot sustain the contracted debt service for the remaining life, which is a capital-structure problem and is restructured ✅
- C. distress is a breach that has occurred at two or more consecutive test dates
- D. distress is present once the debt service reserve has been drawn

*Rationale:* The three are separated by what they are conditions *of* — a test, a profile and a payment — and the diagnostic between the second and third is whether scheduled service exceeds sustainable service for one period or for the remaining life (15.4.1). C counts breaches, which says nothing about the forecast. D describes a liquidity event reserves exist to absorb: Kestrel's debt service reserve was never drawn across six deteriorating years, while the distribution test bound throughout.


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


**16.1-E** `[16.1.1 · Evaluation]` The paper recommending the financial-data spine leads with a **+1,221,674** net present value from retiring 36 pairwise reconciliations, a 316,192 annual labour saving against a 900,000 build. What should a reviewer require before it goes to the board?

- A. nothing further — a +1,221,674 net present value at the board's 8 % over ten years clears the hurdle on its own
- B. the definitional case stated alongside it — one `CFADS` divergence is worth 600,000 a period and 4,026,049 in present value over the same appraisal, 3.2955 times the labour case — together with a commitment to retire the 36 reconciliations, without which the 316,192 saving becomes a 181,472 annual cost ✅
- C. that the appraisal be re-run at the fifteen-system estate the group expects, where the mesh costs 1,451,520 a year against the spine's 69,120
- D. that the 900,000 build be competitively tendered, since it is the largest single figure in the paper

*Rationale:* B supplies both the reason the decision actually turns and the condition the quoted benefit depends on: a spine acquired *beside* an unretired mesh converts a saving into a cost, and that is a governance failure the paper must foreclose (16.1.1). C is a genuine improvement the topic recommends, but it strengthens a case that already passes and speaks to neither the real driver nor the realisation risk. D optimises a one-off number while leaving a recurring 600,000 exposure unaddressed. A accepts a business case whose stated benefit is conditional on a commitment nobody has made.


**16.1-F** `[16.1.2 · Comprehension]` Cost per reviewed item has two parts. Which restatement best captures why the second cannot be left out?

- A. because the platform's committed fixed cost must be spread over the volume before any per-item figure means anything
- B. because a process is only as cheap as the mistakes it leaves behind, so the residual undetected-error rate multiplied by the consequence of an undetected error belongs in the price of the process that produced it ✅
- C. because errors that are found still cost money to put right
- D. because the pipeline exists to improve forecast accuracy, and accuracy belongs in its cost

*Rationale:* The second part monetises the errors that *survive* the process, which is what makes two processes with different accuracies comparable (16.1.2). A names a real and separate term, which the breakeven-volume calculation handles — the per-item figures of 19.36 and 9.36 are variable costs by construction. C describes detected errors, whose handling already sits inside the processing cost as adjudication minutes. D confuses a benefit with a cost.


**16.1-G** `[16.1.2 · Evaluation]` A finance director proposes automating the estate's highest-consequence review work first, because that is where an error costs most and a machine is more consistent than a person. On the measured figures — manual review **13.60** a record leaving **1.80 %** of records with an undetected error, the pipeline **1.04** a record leaving **2.60 %**, against a committed **148,000** a year — the soundest sequencing advice is to:

- A. automate the highest-consequence work first, since that is where an error costs most and consistency is worth most
- B. automate the high-volume, low-consequence work and spend the freed capacity on the low-volume, high-consequence work, because the breakeven **rises** with the consequence of an error — 14,800 records at 320 an error, 50,000 at 1,200 — while the manual 1.80 % beats the pipeline's 2.60 % ✅
- C. automate nothing until the pipeline's residual error rate falls below the manual rate, since automating a less accurate process cannot be justified
- D. automate everything at once, because the 148,000 is committed whatever the scope and the marginal record then costs only 1.04 to process

*Rationale:* The automated process carries the **higher** residual error rate, so a larger consequence per error erodes its advantage instead of enlarging it: repricing an undetected error from 320 to 1,200 moves the breakeven from 14,800 records to 50,000, at which the estate's 56,400 records leave the whole programme worth **18,944** a year — arithmetically positive and professionally indistinguishable from nil (16.1.2). A is the intuition the arithmetic reverses. C sets a defensible-sounding but wrong condition: at 56,400 records and 320 an error the case tolerates an automated rate up to **4.9050 %**, nearly double the 2.60 % modelled, because volume and consequence decide the comparison and not the rate alone. D compares 1.04 with 13.60 and so omits the error-cost term from both sides — the defect that makes automation look **20.3822 %** better than it is.


**16.1-H** `[16.1.1 · Comprehension]` A data spine has four properties: grain, golden source, lineage and a definitional layer. Which statement best explains what the **definitional layer** adds that the other three do not?

- A. it stores the finance documents beside the data, so a defined term can be looked up whenever a figure is questioned
- B. it implements each defined term once, as code, with its clause reference, so a term means in the data what it means in the document instead of being re-implemented — and drifting — wherever it is needed ✅
- C. it lets every reported figure be traced back to the transactions beneath it without human reconstruction
- D. it names the one system that is authoritative for each fact, so that all others are derived

*Rationale:* C and D are the spine's other two properties — lineage and golden source — and neither prevents `CFADS` from being implemented differently in nine systems; the definitional layer is the finance-specific property, and it is why a divergence worth 600,000 of reported cash does not misreport once but every period until somebody finds it (16.1.1). A describes a document repository, which resolves nothing: a term that can be looked up is still implemented separately wherever it is used.


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


**16.2-E** `[16.2.1 · Evaluation]` A credit paper is being assembled. One analyst offers the **minimum `DSCR` across 500 generated scenarios** as the downside case; another offers the lenders' defined flat case. Which should carry the covenant test?

- A. the generated minimum — a 1-in-500 outcome is a more conservative test than any case somebody chose
- B. the defined case — the generated minimum is about the 0.1996th percentile of input tails nobody has validated, while a defined case is a chosen set of assumptions with an owner; the generated set belongs in the paper, labelled, describing shape ✅
- C. the generated minimum, provided `k` is reported alongside it so the reader can judge coverage
- D. neither — the mean `DSCR` across the 500 scenarios is the balanced figure to test

*Rationale:* Conservatism located in an unvalidated tail is not conservatism, and a generated extreme is a percentile rather than a stress (16.2.1). A is the more seductive error precisely because it sounds prudent. C is the right discipline applied to the wrong sentence: reporting `k` repairs a coverage claim, but no value of `k` converts a percentile into a case with an owner. D reports the statistic covenants never test — Domain 10 tests the period, not the average.


**16.2-F** `[16.2.3 · Evaluation]` A modelling team reports a **70.8333 %** saving from machine-assisted construction of a sculpted debt module: 40 hours of build replaced by 6, with review left at the 8 hours the hand-built module carried. Reviewing a machine-built module properly takes **26** hours, because the reviewer must reconstruct construction logic nobody holds. The soundest response is to:

- A. accept the 70.8333 % — the 8 hours were sized for this module, and an assisted module is no harder to read than a hand-built one
- B. book a third rather than two thirds: the honest comparison is 32 hours against 48, and the 18 omitted review hours cost 2,700 against an expected defect cost of 1,137,623.20 — a certain cost converted into an expected one, invisibly, because the omission leaves no artefact ✅
- C. accept the 70.8333 %, but require the assistant to produce the audit trail, which removes the need for the additional review
- D. stop using assistance on financing models: a 0.35 probability of a material defect is not an acceptable exposure on a debt module

*Rationale:* Assistance moves the verification burden rather than removing it, and the governable quantity is the review ratio — here about **4.3333** review hours per assisted build hour, against one per five hand-built (16.2.3). A assumes the reviewer holds knowledge only the builder acquires. C is the seductive half-truth: requiring the assistant to state what each block does, the invariants it should satisfy and the check block itself is exactly how the 26 hours are made smaller *honestly*, but it shortens reconstruction rather than removing the review. D discards a real **33.3333 %** saving, and the 0.35 is the defect probability of an **unreviewed** module — the figure the review exists to remove.


**16.2-G** `[16.2.2 · Comprehension]` An extraction of a facility agreement's **340** defined terms is reported at 92 % per-item accuracy, and **26** of the terms are load-bearing. Which statement best explains the **superseded-draft** failure mode and why no accuracy statistic can see it?

- A. it is the risk that a term is extracted correctly but recorded against the wrong clause number
- B. it is extraction from the wrong version of the agreement: the register can be 100 per cent accurate against the document it read, so the error lies in which document was read rather than in the reading, and accuracy is measured against that same document ✅
- C. it is the risk that a term the tool marked low-confidence is not checked
- D. it is the risk that the load-bearing subset is chosen after the tool has run

*Rationale:* Accuracy is measured against the source the tool was given, so an extraction from a data-room draft rather than the executed agreement scores perfectly while being wrong throughout — which is why the verification of the 26 load-bearing terms, about **3,120** of specialist time and **192.3077 times** cheaper than one 600,000 definitional error, must be against the **executed** document with its version identifier recorded (16.2.2). C and D are the topic's other two cautions, and both are real: a model is least confident where it can detect its own error, and a subset chosen after the run is chosen by the tool. A describes a mis-citation, which a reconciliation to the clause would catch.


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


**16.3-E** `[16.3.1 · Comprehension]` Which restatement best captures the distinction 16.3.1 draws between an explanation and a justification?

- A. an explanation is qualitative, while a justification is quantitative
- B. an explanation is a faithful account of how a model reached a number and says nothing about whether the number is right; establishing that is validation's job ✅
- C. an explanation is what a lender is shown, while a justification is what an auditor is shown
- D. an explanation is a ranking of which inputs mattered most to the output

*Rationale:* Faithfulness and correctness are separate properties, and conflating them is how a persuasive account of a wrong number survives review (16.3.1, 16.3.2). D describes an importance ranking, which the topic rejects for a different reason again — a ranking cannot be reconciled to the output and so cannot be audited. A and C invent a form and an audience the distinction does not turn on.


**16.3-F** `[16.3.2 · Evaluation]` A validation suite of **299** cases returns **one** failure. The team fixes the defect, re-runs the failing case so that all 299 now pass, and proposes to deploy. Assess the proposal.

- A. acceptable — the defect is fixed and 299 passing cases meet the stated 1 % bound at 95 % confidence
- B. not acceptable — permitting one observed failure at the same bound and confidence requires 473 cases, **58.19 %** more, so the suite must grow rather than be repaired; and even 299 clean passes admit about 564 errors a year at 56,400 uses ✅
- C. not acceptable — one failure in 299 shows the defect rate exceeds 1 %, so the model should be withdrawn
- D. acceptable, provided the failing case joins a regression set and the production error rate is monitored

*Rationale:* The 299 figure is the size of a **zero-failure** sample; once a failure has been observed, the evidence the claim rests on is a different calculation, and repairing the case does not restore it (16.3.2). A is the standard move and the specific error the topic names. C over-reads: one failure in 299 does not establish a rate above 1 %, it enlarges the sample the claim needs. D prescribes exactly where the assurance should sit — monitoring reaches 16,895 observations in about four months, which no test programme will — but it does not license a validation claim the sample no longer supports, and the two are separate questions.


**16.3-G** `[16.3.1 · Evaluation]` An automated forecast publishes `CFADS` of **6,384,000** and attributes it across drivers summing to **6,190,000**. The modelling team proposes labelling the **194,000** residual "other — model complexity" and proceeding to a recommendation about which driver to manage. The soundest position is to:

- A. accept it — at **3.0388 %** of the forecast the residual is within any reasonable reporting tolerance, and no attribution reconciles perfectly
- B. refuse the attribution until the residual is found: 194,000 is **52.0892 %** of the annual covenant headroom of 372,438, so no conclusion about which driver to manage is safe, and an explanation whose components do not reconcile to the output is a commentary rather than an explanation ✅
- C. replace the currency attribution with ranked driver-importance scores, which convey the same management message without needing to reconcile
- D. accept it — explanation is not justification, so the completeness of the attribution is a separate question from whether the forecast is right

*Rationale:* The hundred-per-cent rule applies unchanged, and the residual has to be measured against the quantity the project is judged on rather than against the forecast's own size (16.3.1). A is the tolerance argument, and it fails on the denominator rather than on the principle. C is the commonest substitution and is worse than the defect: a ranking cannot be reconciled and therefore cannot be audited, which is why attribution must be in currency. D quotes a true statement of the topic — a faithful account of how a model reached a number says nothing about whether the number is right — and uses it to excuse the opposite failure, an account that is not faithful.


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


**16.4-E** `[16.4.2 · Evaluation]` The finance director accepts the derived **14,118** threshold — worth **80,180** a year against the blanket rule's **400** — and proposes to remove dual approval from the 3,650 sub-threshold payments altogether and publish the threshold in the payments policy. Assess the proposal.

- A. sound as it stands — the arithmetic is unambiguous and it removes 79,780 of destroyed value
- B. sound in its main move but incomplete: a published threshold is an instruction to an adversary to stay below it, so the sub-threshold population needs a sampling control the arithmetic does not model, and the released 87,600 of approver time is better spent on the blind audit that returned 18.4471 times its cost ✅
- C. unsound — the blanket rule nets a positive 400 a year and should be retained
- D. unsound — the 85 % caught rate is an assessment rather than a measurement, so no threshold derived from it can be relied on

*Rationale:* The calculation is right and is not a complete policy: it prices detection and is silent on deterrence, which is precisely what publishing a threshold changes (16.4.2). A treats a correct figure as a finished decision. C defends a control that is value-neutral to three significant figures by combining a strongly positive rule on large payments with a strongly negative one on small. D raises the input that most deserves defending — a second approver who signs without looking makes the whole 115,200 waste — but at any plausible caught rate the blanket rule remains the weaker policy, so uncertainty in that input is a reason to test it, not to abandon the derivation.


**16.4-F** `[16.4.3 · Evaluation]` Shown that the quarterly AI governance committee forgoes **45,589.04** of value per change against the delegated panel's **6,268.49** — a difference of **39,320.55** a change — the head of finance proposes abolishing the committee and routing every model change to the weekly panel. The soundest recommendation is to:

- A. abolish the committee: at 39,320.55 a change its latency destroys more value than its scrutiny creates, and the panel can escalate anything it finds
- B. tier the approvals rather than remove them: changes to the models where one bad output consumes the 372,438 of annual covenant headroom keep the committee, threshold parameters go to the panel under a written mandate, and anything that alters what is reported to a lender is the finance director's own decision ✅
- C. keep the committee for every change: latency is not a cost the finance function pays in cash, and a gate that applies to some changes and not others is no gate at all
- D. route everything to the panel for a year and measure the outcome before deciding, since the committee's catch rate has never been measured

*Rationale:* The latency cost is real and is an argument for putting the gate where the consequence sits, not for removing it — 45,589.04 is **12.24 %** of the headroom a top-tier model can consume in a year, so it is well spent there and wasted on a threshold change, which 16.1.3 showed to be a cost-ratio judgment a standing panel can take competently (16.4.3). A is the plausible cost-driven overcorrection and mistakes an escalation route for a control: a panel not told which changes are top-tier will not escalate them. C denies a cost that is being paid in forgone benefit. D names the right missing measurement — a gate whose catch rate is nil is pure cost, and what the committee actually changes should be measured — but proposes to obtain it by removing the control from precisely the changes whose consequence justifies it, when the measurement can be made with the tiering in place.
