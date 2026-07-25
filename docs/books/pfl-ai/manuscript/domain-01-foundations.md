# Domain 1 — Foundations of Project Finance Leadership

> **Group:** Foundations (Domain 1 of 4 in Part One). **Target:** ~66 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain fixes the book's core vocabulary — recourse, SPV,
> sponsor, bankability, CFADS (introduced by name, built fully in Domain 10) — and the
> stakeholder map every later domain assumes. British English; USD (+SAR where useful,
> indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Project finance is a distinctive answer to a distinctive problem: how to fund a large,
long-lived, single-purpose asset whose only security is *itself* — its contracts and the cash
they will produce. This domain establishes what makes that answer work, and what the leader at
the centre of it actually does. It maps the role across the project lifecycle (KA 1.1), builds
the discipline's three-cornered logic — value, cash and risk (KA 1.2) — and grounds the
profession's obligations: fiduciary awareness, independence, and the governed use of AI
(KA 1.3). Everything later in the book is a specialisation of this domain: the mathematics
(Domains 3–4), the structures (Domains 5, 9, 12), the lender's machinery (Domains 10, 13–15)
and the AI curriculum (Domain 16) all stand on the concepts fixed here. A reader who finishes
only this domain should already reason like the profession: *follow the cash, price the risk,
know who bears it, and stay accountable.*

**Learning objectives.** After this domain a candidate can: describe the project finance
leader's role at each lifecycle stage; place a financing on the recourse spectrum and explain
what limited recourse buys and costs; explain the SPV's purpose and the interests of each
party around it; describe the infrastructure-finance market's asset classes and investors;
explain why cash, not profit, is the binding constraint and demonstrate the difference;
explain leverage's amplification of equity returns and of risk with a computed example; state
the risk-return-bankability logic; and apply the profession's ethical and responsible-AI
obligations to realistic situations.

**The master thread.** Kestrel Water SPC — whose loan, availability stream and investment case
Domains 3 and 4 priced — began here: a sponsor group weighing *how* to finance a desalination
plant at all. This domain tells that part of the story; Domain 5 takes the project through
development to bankability.

---

## Knowledge Area 1.1 — The project finance leader and the financing landscape

*Topics: 1.1.1 the role across the lifecycle · 1.1.2 corporate versus project finance — the
recourse spectrum · 1.1.3 the SPV and its stakeholders · 1.1.4 the infrastructure-finance
market.*

### 1.1.1 The role across the lifecycle

The project finance leader is the person accountable for a project's **financial integrity
end to end** — not the deal-closer alone, and not the accountant alone. The role changes
costume by stage while keeping one spine:

| Stage | What the finance leader owns |
|---|---|
| Development | Screening economics (Domain 4); funding development spend at risk; shaping a *financeable* concept (Domain 5) |
| Structuring | Capital structure and funding sources (Domain 9); risk allocation into contracts (Domains 11–12) |
| Execution (financial close) | Due diligence, model audit, documentation, conditions precedent (Domain 13) |
| Construction | Drawdowns, cost-to-complete, lender reporting (Domain 14) |
| Operations | Covenant compliance, waterfall management, distributions, refinancing (Domain 15) |
| Maturity/exit | Handback, sale, restructuring where needed (Domain 15) |

The spine is a single question asked at every stage: **will the cash arrive, and who is
exposed if it does not?** The leader's authority rests on being the person in the room who can
answer it with evidence.

### 1.1.2 Corporate versus project finance — the recourse spectrum

**Definitions.** In **corporate (balance-sheet) finance**, lenders lend to a company and are
repaid from its whole cash flow; every asset stands behind every debt. In **project finance**,
lenders lend to a ring-fenced project and are repaid **only from that project's cash flows**,
with security over its assets and contracts — **non-recourse** to the sponsors, or **limited
recourse** where sponsors give bounded support (a completion guarantee, a cost-overrun
facility). Real deals sit on a spectrum between the poles.

What limited recourse *buys* sponsors: risk containment (a failed project cannot sink the
parent), balance-sheet capacity, the ability to share a mega-project among partners, and
discipline — lenders' due diligence becomes a second pair of eyes on every assumption. What it
*costs*: higher margins and fees (lenders carry risk they cannot chase a parent for), heavy
transaction and diligence costs, long documentation timelines, and covenant control over the
project's cash (Domain 10). The break-even is scale and risk-shape: single-asset,
contract-backed, capital-intensive projects with long lives are where the machinery pays.

> **Fig 1.1.1 — The recourse spectrum.** Horizontal spectrum diagram. Left pole: "Corporate /
> full recourse — lender looks to the whole balance sheet"; right pole: "Non-recourse — lender
> looks only to project cash flows and security". Between them, marked positions: guaranteed
> project loan · limited recourse (completion support) · non-recourse with reserves. Beneath
> each position, two mini-bars: sponsor risk retained (shrinking left to right) and financing
> cost/complexity (growing left to right). Source: PCI original. Alt text: a spectrum from
> full-recourse corporate lending to non-recourse project lending, showing sponsor risk
> falling and financing cost rising toward the non-recourse pole.

### 1.1.3 The SPV and its stakeholders

**Definition.** The **special-purpose vehicle (SPV)** is a company created to do exactly one
thing — own, build, finance and operate the project — and legally *incapable* of doing
anything else. Ring-fencing is what makes non-recourse lending possible: the SPV's contracts
are its assets, and every major relationship is written down (Domain 12 builds the contract
matrix in full).

The parties and what each optimises:

| Party | Wants | Watches |
|---|---|---|
| **Sponsors** (equity) | Return on equity; contained risk; distributions | Equity IRR, distribution tests |
| **Lenders** | Repayment with margin; downside protection | DSCR/LLCR, covenants, security (Domain 10) |
| **Offtaker / grantor** | Reliable service at agreed price | Availability, tariffs, handback condition |
| **EPC contractor** | Construction margin | Variations, delay LDs (Domain 12) |
| **Operator (O&M)** | Fee; performance regime it can meet | Availability/output guarantees |
| **Government / regulator** | Delivery of policy outcomes; compliance | Permits, obligations, public interest |
| **Community & environment** | Benefit without harm | E&S performance (Domain 11) |

The finance leader's daily craft is reconciling these optimisations *inside one cash flow* —
which is why the cash waterfall (Domain 15) reads like a peace treaty.

> **Fig 1.1.2 — The SPV at the centre of its contracts.** Hub-and-spoke diagram. Centre node:
> "Project SPV". Spokes to: Sponsors (equity subscription, shareholder agreement) · Lenders
> (facility agreement, security) · Offtaker (offtake/concession agreement) · EPC contractor
> (turnkey construction contract) · O&M contractor (operating agreement) · Government
> (permits, direct agreement). Each spoke labelled with the money/service flowing each way.
> Source: PCI original. Alt text: a central project company connected by labelled contract
> spokes to sponsors, lenders, offtaker, contractors and government.

### 1.1.4 The infrastructure-finance market

The asset classes and their financing habits, in one professional sweep: **transport** (toll
roads, rail, airports — patronage or availability models); **power and renewables** (PPAs,
capacity markets; the energy transition's build-out is the market's largest engine);
**water** (desalination and treatment concessions — Kestrel's world); **digital
infrastructure** (data centres, towers, fibre — shorter refresh cycles, credit-tenant
leases); **social infrastructure** (hospitals, schools, housing under availability PPPs); and
**natural resources** (commodity-linked, price-hedged). The capital comes from commercial
banks (construction-phase specialists), institutional investors and infrastructure funds
(long-dated operations-phase capital), export credit agencies and development banks
(Domain 9), and bond markets (refinancing bankable operating assets). The leader's market
literacy is matching **asset shape to capital shape**: construction risk to banks and ECAs,
stabilised cash flows to institutions — mismatches are expensive at best and fatal at close.

### AI in this KA

Market screening and precedent research are natural AI accelerants — summarising comparable
transactions, extracting terms from public disclosures, drafting stakeholder maps. The
governed habits start in Domain 1 because the failure modes do: a fluent but invented
"precedent transaction" is this profession's textbook hallucination case. Sources are
verified against the registry discipline (every claimed deal traced to a public record), and
the stakeholder analysis an AI drafts is walked, party by party, by someone who has sat
across from those parties. **AI proposes; the professional verifies, decides and remains
accountable.**

### Key terms — KA 1.1

| Term | Meaning |
|---|---|
| **Recourse / non-recourse / limited recourse** | Whom the lender can pursue: the sponsor's balance sheet; the project only; the project plus bounded sponsor support. |
| **SPV** | Ring-fenced single-purpose project company; the borrower and contract hub. |
| **Sponsor** | Equity investor promoting the project (this book's project-finance sense). |
| **Offtaker** | The buyer of the project's output or service under contract. |
| **Ring-fencing** | Legal isolation of the project's assets, contracts and cash. |
| **Asset-capital matching** | Pairing project risk phases with the capital suited to hold them. |

### Sample MCQs — KA 1.1

**MCQ 1.1-A `[1.1.2 · Application]`** A sponsor gives lenders a guarantee that covers cost
overruns until the plant passes its completion test, after which lenders may look only to
project cash flows. This financing is best described as:
- A. full recourse
- B. non-recourse
- C. limited recourse ✅
- D. unsecured corporate lending

*Rationale:* Bounded sponsor support (here, to completion) between the poles is the defining
shape of limited recourse. A would expose the sponsor for the loan's life; B would mean no
sponsor support at all; D abandons both the security package and the ring-fence.

**MCQ 1.1-B `[1.1.3 · Analysis]`** Which single feature of the SPV makes non-recourse lending
possible?
- A. its tax registration
- B. legal ring-fencing: the SPV can conduct only the project, so its contracts and cash are isolated and chargeable ✅
- C. its sponsors' credit ratings
- D. the size of its share capital

*Rationale:* Lenders can accept project-only recourse because the ring-fence guarantees no
other business can dilute, encumber or divert the cash they are lending against. C reverses
the concept (sponsor credit is what non-recourse lending does *without*); A and D are
administrative facts, not the mechanism.

**MCQ 1.1-C `[1.1.4 · Application]`** A fund holding long-dated pension liabilities wants
infrastructure exposure. The asset-capital matching principle points it toward:
- A. construction-phase risk in a greenfield project
- B. stabilised operating assets with contracted cash flows ✅
- C. development-stage equity at risk
- D. short-term bridge lending

*Rationale:* Long-dated stable liabilities match long-dated stable cash flows. A and C sit
where construction and development specialists (banks, ECAs, developers) hold the risk;
D matches a treasury desk, not a pension profile.

### Self-check — KA 1.1

1. *State the finance leader's one recurring question.* — Will the cash arrive, and who is
   exposed if it does not?
2. *Name two things limited recourse buys a sponsor and two things it costs.* — Buys: risk
   containment, balance-sheet capacity (also partnering, lender discipline). Costs: pricing
   and fees, transaction complexity (also covenant control, time).
3. *Why does every party around the SPV get a contract?* — Non-recourse credit is built from
   contracts: each relationship must be enforceable because the cash flow is the only
   security.

---

## Knowledge Area 1.2 — Value, cash and risk: the discipline's logic

*Topics: 1.2.1 value creation in projects · 1.2.2 cash as the binding constraint · 1.2.3
leverage, risk and the bankability triangle.*

### 1.2.1 Value creation in projects

A project creates value when the present value of what it will produce exceeds what it costs
to build and run — Domain 4's NPV, stated in words. The foundation point is *where* value can
be created or destroyed by *financing*: structure does not conjure value from a bad project,
but it can (1) allocate each risk to the party who bears it cheapest — lowering the priced-in
premiums; (2) match capital to risk phase — lowering the blended cost of funds; and (3)
impose diligence and covenant discipline that keeps forecast value from leaking in execution.
The corollary the profession lives by: **financing engineering amplifies project quality; it
never substitutes for it.**

### 1.2.2 Cash, not profit, is the binding constraint

**The principle.** Profit is an opinion about periods (Domain 2's accrual model); **cash pays
debt service**. Projects die of cash exhaustion, usually while reporting profits.

**Worked example 1.2.2 — profitable and out of cash.**

1. **Setup.** In its first operating quarter a project company recognises revenue of
   USD 10,000,000 against costs of USD 8,000,000. But customers have paid only 7,000,000 of
   the revenue (receivables +3,000,000); spare-parts inventory was built up by 1,000,000; and
   suppliers extended 500,000 of additional credit (payables +500,000).
2. **Formula.** Operating cash flow = profit − Δreceivables − Δinventory + Δpayables.
3. **Substitution.** `2,000,000 − 3,000,000 − 1,000,000 + 500,000`.
4. **Result.** Profit **+USD 2,000,000**; operating cash flow **−USD 1,500,000**.
5. **Interpretation.** The same quarter is a success in the income statement and a crisis in
   the bank account: a 2.0m "profitable" company is 1.5m short of the cash its debt service
   assumed. This is why lenders size and test debt against **CFADS** — cash flow available
   for debt service (defined fully in Domain 10) — and why every model in Domain 6 is a *cash*
   model first. Domain 2 builds the full accrual-to-cash bridge.

### 1.2.3 Leverage, risk and the bankability triangle

**Leverage amplifies.** Debt is cheaper than equity, and fixed: whatever the project earns,
debt service is owed. That fixity cuts both ways.

**Worked example 1.2.3 — the two faces of leverage.**

1. **Setup.** A project costs USD 100,000,000 and produces steady operating cash of
   USD 12,000,000 per year. Compare an all-equity structure with 70 % debt
   (USD 70,000,000, interest-only at 6.0 % = 4,200,000 per year), in the base case and with
   cash down 25 % and 50 %.
2. **Formula.** Unlevered return = cash / 100,000,000. Equity cash = cash − 4,200,000;
   levered return = equity cash / 30,000,000.
3. **Substitution.** Base: `12.0 − 4.2 = 7.8` on 30. Down 25 %: `9.0 − 4.2 = 4.8`. Down 50 %:
   `6.0 − 4.2 = 1.8`.
4. **Result.**

   | Scenario | Project cash | Unlevered return | Equity cash | Levered return |
   |---|---|---|---|---|
   | Base | 12,000,000 | 12.0 % | 7,800,000 | **26.0 %** |
   | −25 % | 9,000,000 | 9.0 % | 4,800,000 | **16.0 %** |
   | −50 % | 6,000,000 | 6.0 % | 1,800,000 | **6.0 %** |
   | −65 % | 4,200,000 | 4.2 % | 0 | **0.0 %** |

5. **Interpretation.** Leverage more than doubles the base-case equity return (26 % vs 12 %)
   — and makes the downside three times steeper (26 → 6 % as cash halves, versus 12 → 6 %
   unlevered). At a 65 % cash decline the equity earns nothing and the lender is next in
   line. Gearing is chosen, not maximised: Domain 9 structures it, Domain 10 shows how
   lenders cap it with coverage ratios sized precisely against scenarios like this table.

**The bankability triangle.** Three tests every financing must pass simultaneously:
**value** (the project is worth doing — Domain 4), **cash** (the flows arrive in the periods
that need them — Domains 6–8), and **risk allocation** (each hazard sits with a party able
and bound to bear it — Domains 11–12). A project strong on two corners and weak on one is
not two-thirds bankable; it is unbankable until the corner is fixed. **Bankability** (built
fully in Domain 5) is precisely the state of passing all three tests in the eyes of the
capital being asked to commit.

> **Fig 1.2.1 — The bankability triangle.** Equilateral triangle diagram. Corners: "VALUE —
> worth doing (NPV, Domain 4)" · "CASH — arrives when needed (Domains 6–8)" · "RISK — sits
> with those who can bear it (Domains 11–12)". Centre label: "BANKABLE — all three, together
> (Domain 5)". Each edge annotated with the failure mode of the missing corner: valuable but
> cash-mistimed → liquidity failure; cash-rich but mispriced risk → repricing at close;
> well-allocated but low value → no equity. Source: PCI original. Alt text: triangle whose
> corners are value, cash and risk allocation, with bankability at the centre and each
> missing corner's failure mode noted along the edges.

### AI in this KA

Scenario tables like 1.2.3's are ideal machine work — and the place where a subtly wrong
fixed-charge assumption (interest-only vs amortising; Domain 3's shapes) silently reshapes
every row. The governed pattern: the machine drafts the grid; the analyst re-derives one row
by hand and checks the boundary case (where equity cash crosses zero) analytically; the
leader reads the *downside* rows first, because that is what the structure is actually being
designed against.

### Key terms — KA 1.2

| Term | Meaning |
|---|---|
| **CFADS** | Cash flow available for debt service (name fixed here; machinery in Domain 10). |
| **Working capital drag** | Cash absorbed by receivables and inventory ahead of profit. |
| **Leverage / gearing** | Debt's share of funding; amplifier of equity return and risk. |
| **Fixed charge** | Debt service owed regardless of performance. |
| **Bankability triangle** | Value + cash + risk allocation, passed together. |

### Sample MCQs — KA 1.2

**MCQ 1.2-A `[1.2.2 · Application]`** A company reports quarterly profit of 2,000,000 while
receivables rose 3,000,000, inventory rose 1,000,000 and payables rose 500,000. Its operating
cash flow is:
- A. +2,000,000
- B. −1,500,000 ✅
- C. +500,000
- D. −2,500,000

*Rationale:* `2.0 − 3.0 − 1.0 + 0.5 = −1.5m`. A stops at profit; C nets only the payables
against profit and forgets the asset build; D subtracts the payables increase instead of
adding it — supplier credit is a cash *source*.

**MCQ 1.2-B `[1.2.3 · Application]`** In the leverage example (70m debt, interest-only 6 %;
equity 30m), project cash of 9,000,000 produces a levered equity return of:
- A. 9.0 %
- B. 16.0 % ✅
- C. 26.0 %
- D. 30.0 %

*Rationale:* `(9.0 − 4.2)/30 = 16.0 %`. A is the unlevered return; C is the base-case levered
return; D divides project cash by equity without paying the lender first.

**MCQ 1.2-C `[1.2.3 · Analysis]`** A project shows strong NPV and well-allocated risks, but
its revenue arrives seasonally while debt service is quarterly and level. The bankability
verdict is:
- A. bankable — two of three corners suffice
- B. unbankable as structured: the cash corner fails; reshape the debt profile or add liquidity support ✅
- C. unbankable permanently — reject the project
- D. bankable if the sponsors accept a higher equity IRR

*Rationale:* The triangle is conjunctive: mistimed cash defeats value and allocation. The
cure is structural (sculpted or seasonal debt service, reserve accounts — Domains 9–10), not
rejection (C) and not a return adjustment that changes nothing about timing (D).

### Self-check — KA 1.2

1. *Why do lenders test CFADS rather than profit?* — Debt service is paid in cash; accrual
   profit can coexist with cash exhaustion (WE 1.2.2).
2. *State leverage's two faces in one sentence each.* — It multiplies the equity return on
   the same project cash; it makes every downside steeper because debt service is fixed.
3. *Why is the triangle conjunctive?* — Each corner defeats the financing alone: no value →
   no equity; no cash timing → default risk regardless of value; no allocation → risk premia
   or collapse at close.

---

## Knowledge Area 1.3 — Ethics, fiduciary awareness and responsible AI

*Topics: 1.3.1 obligations and duties · 1.3.2 conflicts and independence · 1.3.3 the
responsible-AI principle in finance.*

### 1.3.1 Obligations and duties

The project finance leader acts inside a lattice of duties: **fiduciary-type duties** to the
employer or client (loyalty, care, confidentiality); **contractual duties** under mandates
and finance documents; **statutory duties** (companies law, anti-bribery and corruption,
sanctions, market conduct); and **professional duties** — competence, candour, and records
that let others check the work. Two standing disciplines follow. *Candour about numbers*:
forecasts are presented with their assumptions and sensitivities, never as certainties
(Domain 4, KA 4.3.3); an optimistic case knowingly presented as a base case is a
misrepresentation, whatever the spreadsheet says. *Candour about limits*: this book's own
rule — educational reference, not individualized advice; jurisdiction-specific matters go to
qualified counsel and advisers — is the same professional humility applied to oneself.

### 1.3.2 Conflicts and independence

Project finance concentrates conflicts because the same institutions recur in many roles:
the adviser who would earn a success fee on close; the sponsor-affiliated contractor pricing
the EPC; the bank advising the government while lending to bidders. The professional
machinery is disclosure and separation: conflicts are declared before engagement, managed
with information barriers or declined; advice and self-interest are never silently blended.
The leader's test for any arrangement is the *daylight test* — would every party, seeing the
full fee and relationship map, still regard the advice as independent? Where the answer
wavers, independence has already failed. (Case study B applies this to a live tender;
Domain 13's diligence streams exist partly to give lenders advice that passes the test.)

### 1.3.3 The responsible-AI principle in finance

The suite principle — **AI proposes; the professional verifies, decides and remains
accountable** — lands hardest in finance, where machine output *looks* like the work product
itself (a model, a memo, a covenant summary). Domain 16 builds the full governance
architecture; the foundations fixed here:

- **Verification is not optional and not delegable to the tool.** Golden-answer checks for
  calculations (the discipline this book applies to itself); source-tracing for claims;
  document-against-summary checks for AI-read contracts.
- **Accountability cannot be transferred.** "The model said so" is never a defence — the
  signing professional owns the output as if hand-made.
- **Confidentiality travels with the data.** Deal information entering an AI tool is a
  disclosure; it happens only within approved, contracted environments.
- **Material AI use is disclosed** within the team and, where it touches deliverables,
  to the client — the daylight test again, applied to method.

### Key terms — KA 1.3

| Term | Meaning |
|---|---|
| **Fiduciary awareness** | Acting in the principal's interest with loyalty, care and confidentiality. |
| **Conflict of interest** | An interest that could bias judgment; declared, managed or declined. |
| **Daylight test** | Would full disclosure of interests leave the advice trusted? |
| **Responsible-AI principle** | AI proposes; the professional verifies, decides, remains accountable. |
| **Verification duty** | The named human's obligation to check machine output before reliance. |

### Sample MCQs — KA 1.3

**MCQ 1.3-A `[1.3.1 · Analysis]`** An analyst is asked to present the upside case as the
base case "because the committee needs confidence". The professional response is:
- A. comply — labels are a presentation choice
- B. decline: presenting a knowingly optimistic case as the base misrepresents the forecast; offer the honest base with sensitivities instead ✅
- C. comply, but keep a private note of the true base
- D. resign immediately without discussion

*Rationale:* Candour about numbers is a duty, not a style (A); a private note documents the
misrepresentation without preventing it (C); B both refuses the breach and offers the
legitimate route to confidence — evidence. D skips the professional obligation to fix the
problem before escalating personal exits.

**MCQ 1.3-B `[1.3.2 · Application]`** A bank advising a grantor on a tender also wishes to
lend to one of the bidders. The minimum acceptable handling is:
- A. proceed — different departments are involved
- B. disclose the dual role to the grantor, and either obtain informed consent with effective information barriers or decline one role ✅
- C. keep the lending discussion confidential until after award
- D. advise the grantor to select that bidder

*Rationale:* Disclosure plus genuine separation (or declination) is the standing machinery;
department labels alone are not barriers (A); concealment converts a conflict into
misconduct (C); D is the conflict operating in the open.

**MCQ 1.3-C `[1.3.3 · Recall]`** Under the PCI responsible-AI principle, responsibility for
an AI-drafted covenant summary used in a credit paper rests with:
- A. the AI vendor
- B. the model itself
- C. the professional who verified, signed and used it ✅
- D. nobody, if the tool was approved

*Rationale:* Accountability cannot be delegated to software or its supplier; tool approval
governs *which* tools may be used, never *who* answers for the output.

### Self-check — KA 1.3

1. *State the daylight test.* — Would every party, seeing the full relationship and fee map,
   still trust the advice as independent?
2. *What three checks make AI-assisted work professionally usable?* — Recomputed numbers
   (golden checks), traced sources, document-against-summary verification — by a named human.
3. *Why does confidentiality bind AI use?* — Data entering a tool is a disclosure; it must
   stay within approved, contracted environments.

---

## Industry variations — Domain 1

The foundations flex by sector mainly in *who* the counterparties are: in **power and
water**, the offtaker is often a state utility — sovereign credit and political risk enter
the stakeholder map (Domain 11); in **transport**, the "offtaker" may be the travelling
public — demand risk reshapes the whole triangle; in **social PPPs**, the grantor's
availability payment makes government the cash engine and handback condition a first-order
obligation; in **digital infrastructure**, corporate credit-tenants replace state offtakers
and refresh cycles shorten every horizon; in **natural resources**, the market itself is the
offtaker and hedging policy joins the foundations. The leader's first map in any new sector:
who pays, under what compulsion, and what can stop them.

## Case study — Domain 1: how Kestrel chose project finance (water)

**Situation.** Kestrel's two sponsors — an international water operator and a regional
infrastructure fund — faced a USD 100,000,000 plant with a 25-year availability offtake.
Corporate borrowing was available to the operator at attractive rates; the fund could not
guarantee anything beyond its equity.

**Analysis.** Corporate route: cheapest debt, fastest close — but the operator alone carries
100 % of construction and performance risk on its balance sheet, the fund cannot participate
on equal terms, and a project failure would impair the operator's whole credit. Project
route: an SPV with limited-recourse debt (completion support only), 70/30 gearing — pricier
debt and eighteen months of structuring, but risk contained at the ring-fence, the partners
aligned through one shareholders' agreement, and lender diligence pressure-testing every
assumption (the discipline dividend of 1.2.1). The leverage table of WE 1.2.3 gave the
equity story; the triangle gave the test — value (Domain 4's +16.18m NPV), cash (the
availability stream priced in Domain 3), risk (allocated through the contract matrix built
later in Domain 12).

**The decision.** Project finance — not because it was cheaper (it was not), but because it
made the partnership possible, contained the downside, and converted lender scrutiny into
project quality. The minute records the recourse position, the support obligations and their
sunset at completion — the exact vocabulary of KA 1.1.2.

**What the domain teaches here.** The financing route is a *risk and partnership* decision
before it is a cost decision; the cheapest debt attached to the wrong recourse shape is the
expensive option.

## Case study B — Domain 1: the adviser with two hats (transport tender)

**Situation.** A grantor's financial adviser on a toll-road tender was discovered — after
preferred-bidder selection — to hold an advisory mandate for the winning consortium's lead
sponsor on an unrelated deal, undisclosed. The losing bidders challenged; the award was
suspended pending review.

**What happened.** The review found no evidence the evaluation was slanted — and it did not
matter. The undisclosed relationship alone failed the daylight test: the tender was re-run
with a new adviser at a cost of fourteen months and the grantor's credibility, and the
adviser's firm lost its public-sector practice in the jurisdiction. A one-line disclosure at
engagement, with barriers or a declined mandate, would have cost nothing.

**What the domain teaches here.** Conflicts are priced at *discovery*, not at occurrence —
and the price is paid in time, trust and franchise, not fees. Independence is an asset that
only disclosure can insure.

## Executive perspective — Domain 1

What a project finance director cannot delegate in this domain:

- **The recourse position.** Exactly what the sponsor stands behind, until when, capped at
  what — the director signs this sentence personally and re-reads it before every support
  call.
- **The stakeholder map's honesty.** Every party's real incentive, including the
  uncomfortable ones, on one page the board has seen.
- **The cash question.** Asked in every meeting until it embarrasses no one: *will the cash
  arrive, and who is exposed if it does not?*
- **The conflicts register.** Kept current, disclosed early, tested against daylight —
  because the director's own relationships are usually the largest entries.
- **The AI accountability line.** Named humans own machine output; the director owns the
  culture that makes that real (Domain 16 gives the machinery; Domain 1 gives the law).

## Calculation exercises — Domain 1

**Exercise 1.1** Profit 3,500,000; receivables +2,200,000; inventory +900,000; payables
+600,000. Operating cash flow?
*Solution.* `3.5 − 2.2 − 0.9 + 0.6 =` **+USD 1,000,000**. Common error: sign on payables
(supplier credit is a source: +0.6, not −0.6; the wrong sign gives −0.2m and a false alarm).

**Exercise 1.2** The WE 1.2.3 project refinances to 80 % debt (80,000,000 interest-only at
6.5 %). Rebuild the base-case levered return and the cash decline at which equity income
reaches zero.
*Solution.* Debt service `80 × 0.065 = 5,200,000`; equity 20,000,000; base equity cash
`12.0 − 5.2 = 6.8` → **34.0 %**. Zero at project cash = 5,200,000 — a **56.7 % decline**
(from 12.0). Versus the 70/30 case: two points more base return (34 vs 26 %... on a third
less equity) bought a materially nearer cliff (−56.7 % vs −65 %). Common error: comparing
levered percentages without comparing the cliffs.

**Exercise 1.3** Classify: (a) parent guarantees debt until completion test, then released;
(b) parent comfort letter, non-binding; (c) no support, reserves funded from cash flow.
*Solution.* (a) limited recourse; (b) effectively non-recourse in law — comfort letters are
generally not enforceable guarantees (jurisdiction-specific; counsel confirms); (c)
non-recourse with structural mitigation. Common error: treating a comfort letter as
recourse — lenders price it as goodwill, not security.

## Practitioner's toolkit — Domain 1

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 1.T.1 — Stakeholder map (one page per project)

Per party: name and role · what they optimise · contract binding them (Domain 12 reference) ·
cash flows to/from the SPV · veto points and consents · relationship owner. Rule: the map is
board-visible and includes the uncomfortable incentives.

### Toolkit 1.T.2 — Financing-route decision record

Options considered (corporate / limited recourse / non-recourse / hybrid) · recourse sentence
per option (who stands behind what, until when, capped at what) · pricing and cost deltas ·
partnership and balance-sheet effects · risk containment analysis (triangle test per
option) · decision, rationale, decision-maker, date.

### Toolkit 1.T.3 — Conflicts and AI-use register

Conflicts: relationship · parties affected · disclosure date · handling (barriers/consent/
declined) · review date. AI use: tool and environment · data classification cleared ·
verification steps and named verifier · disclosure status. One register, one owner, standing
agenda item.

## Exam preparation — Domain 1

**The traps.** Recourse classifications (comfort letter ≠ guarantee — Exercise 1.3) ·
payables sign in the cash bridge (Exercise 1.1) · levered-return arithmetic that skips debt
service (MCQ 1.2-B distractor D) · reading the triangle as two-out-of-three ·
"sponsor" meaning equity investor in this book's project-finance chapters (terminology
registry) · assigning AI accountability anywhere but the signing professional.

**Reflection questions.**
1. Take a project you know: write its recourse sentence in under 25 words. Who stands behind
   what, until when, capped at what?
2. Which corner of the bankability triangle does your current project stress most — and what
   structural (not cosmetic) fix would close it?
3. What in your team's current AI usage would fail the daylight test if disclosed in full —
   and what changes tomorrow because you asked?

## Domain 1 summary

Project finance funds single-purpose assets against their own cash, made possible by the
ring-fenced SPV and priced along the recourse spectrum; the leader's role is the financial
integrity of that machine across the whole lifecycle, under one recurring question — will
the cash arrive, and who is exposed if it does not? The discipline's logic triangulates
value, cash and risk: financing amplifies project quality but never substitutes for it;
cash, not profit, binds (a profitable quarter can be a cash crisis); leverage multiplies
returns and steepens every downside, which is why lenders will later cap it with coverage
machinery. Around the technique stands the profession: fiduciary-grade candour about
numbers and limits, conflicts managed in daylight, and machine assistance governed by the
suite principle — AI proposes; the professional verifies, decides and remains accountable.
Domain 2 builds the accounting the cash bridge assumed; Domain 5 takes Kestrel from concept
to bankability.
