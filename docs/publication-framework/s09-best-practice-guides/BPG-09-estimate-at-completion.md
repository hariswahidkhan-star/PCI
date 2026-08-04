---
id: BPG-09
series: S09
series_name: Best Practice Guides
title: Estimate at completion — choosing and defending a method
subtitle: The formula was never the hard part
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: professional
reading_time_min: 15
summary: >
  Every estimate at completion is a claim about the work that has not been done yet, and the formula only
  encodes which claim you are making. This guide runs the four standard methods over one shared data set so
  the spread is visible, gives the criteria for selecting between them, shows how to test a forecast for
  credibility before a board does, and sets out the four sentences a defensible forecast has to survive.
linkedin:
  format: article
  hook: >
    Four teams, one data set, four estimates at completion two million apart — and every one of them
    arithmetically correct. The formula was never the hard part.
  tags: [ProjectControls, Forecasting, EarnedValue, CostEngineering]
  asset: carousel-8
gated: false
related: [BPG-08, BPG-10, BPG-14, BPG-16, TPL-08]
bok_domains: [3, 6]
sources: []
placeholders: 1
---

# Estimate at completion — choosing and defending a method

> The formula was never the hard part.

**In one paragraph.** Every estimate at completion is a claim about the work that has not been done yet, and
the formula only encodes which claim you are making. This guide runs the four standard methods over one
shared data set so the spread is visible, gives the criteria for selecting between them, shows how to test a
forecast for credibility before a board does, and sets out the four sentences a defensible forecast has to
survive.

**Who this is for.** Cost engineers, project controls managers and project managers who own a monthly
forecast, and the sponsors and portfolio managers who have to decide whether to believe one.

---

## 1. A forecast is a decision, not an output

Three competent people can take the same control account data, apply three standard methods, and produce
three estimates at completion (EAC) that differ by more than ten per cent of the budget. None of them has
made an arithmetic error. They have made different assumptions about the remaining work, and the formulae
are simply the shorthand in which those assumptions are written down.

This is the point most forecasting practice misses. The question a forecast answers is not "what does the
data say" — the data says several things at once — but "what do we believe about the work ahead, and on what
grounds". A team that cannot state its assumption in a sentence has not made a forecast. It has run a
spreadsheet.

The professional obligation follows directly: **choose the method for a stated reason, disclose the reason,
and say what would change it.** A forecast you cannot defend is a guess with decimals.

## 2. The identity underneath every method

Every EAC obeys one identity:

```
EAC = AC + ETC
```

where **AC** is actual cost to date and **ETC** is the estimate to complete — the forecast cost of the work
remaining. Nothing else in the family is new. The methods differ *only* in how ETC is derived, and each
derivation is an assumption about how the remaining work will behave relative to the work already done.

Two supporting quantities:

```
VAC  = BAC − EAC              Variance at completion; negative = forecast overrun
TCPI = (BAC − EV) ÷ (BAC − AC)  The cost efficiency the remaining work must achieve to land on BAC
```

`BAC` is the budget at completion and `EV` the earned value; both are defined and measured in
`BPG-08 — Earned value in practice`, which this guide assumes.

## 3. The four methods, and the claim each one makes

| Method | ETC derivation | The claim being made |
|---|---|---|
| **A — Remaining work at budget** | `ETC = BAC − EV` | The variance to date came from a discrete, closed event. The rest of the job runs to budget. |
| **B — Remaining work at current cost performance** | `ETC = (BAC − EV) ÷ CPI`, so `EAC = BAC ÷ CPI` | The inefficiency is systemic and persists at its present rate for the remainder. |
| **C — Remaining work dragged by cost *and* schedule** | `ETC = (BAC − EV) ÷ (CPI × SPI)` | Being late is itself inflating cost — extended preliminaries, disruption, acceleration — so the two compound. |
| **D — Bottom-up re-estimate** | `ETC` estimated directly from the remaining scope | Past performance is not representative of what is left, because the remaining work is different in kind. |

`CPI` is the cost performance index and `SPI` the schedule performance index.

Two remarks that save arguments later.

**Method B has two forms that are algebraically identical.** `AC + (BAC − EV) ÷ CPI` reduces to `BAC ÷ CPI`
whenever CPI is computed as `EV ÷ AC` on cumulative figures, because `AC = EV ÷ CPI`. If your two forms
disagree, you have mixed cumulative and period indices, or you are dividing by a CPI that was rounded before
use.

**Method C is not "the conservative one".** It embeds a specific causal story — that schedule slippage is
converting into cost — and it is wrong, not merely cautious, on a project that is behind schedule for
reasons that cost nothing extra. Reaching for C because it gives the largest number is as unprofessional as
reaching for A because it gives the smallest.

## 4. Criteria for choosing

The method should match the **cause** of the variance, and cause is established by investigation, not by
inspecting the index. Four questions, in order:

**1. What actually caused the variance to date, and is it closed?**
If the variance came from a discrete event that has finished — a rework episode now complete, a rate spike
now locked by a signed order, a one-off remediation — then the remaining work has no reason to inherit it,
and method A is defensible. The test is evidence of closure: an executed contract, a completed scope, a
signed-off inspection. "We think it was a one-off" is not closure.

**2. Is the inefficiency structural?**
Productivity shortfalls, rate pressure in a tight labour market, an under-resourced supervision model and
poor buildability are all conditions rather than events. They persist until something changes them, and
method B is the natural forecast. Ask what specific, funded intervention would break the pattern; if nobody
can name one, B is the honest answer.

**3. Is lateness costing money?**
Where the project is behind and the delay is drawing time-related cost — extended site establishment,
standing plant, retained supervision, acceleration measures — cost and schedule performance genuinely
compound and method C reflects the mechanism. Where the project is behind on float-rich work with no
time-related consequence, C overstates.

**4. Does the remaining work look like the work done?**
This is the question that overrides the other three. A project moving from bulk construction into
commissioning, from civils into fit-out, or from a first-of-a-kind unit into repeat units has a remainder
whose cost behaviour is not predicted by the past at all. Extrapolation methods are then structurally
unsuited, whatever their arithmetic, and a bottom-up ETC (method D) is the only sound basis.

A fifth consideration cuts across all four: **the stability of the CPI trend.** A cumulative CPI that has
been flat for three or four periods is a settled measurement, and a forecast that assumes it will improve is
making a claim that needs evidence. Published research on large programmes has examined how early cumulative
cost performance stabilises and how rarely it recovers materially thereafter
[CONFIRM: citation and date for the cumulative-CPI stability research, before publication]. Whatever the
precise finding, the practical rule is defensible on its own: once a CPI has settled, "the variance was
atypical" is the claim that carries the burden of proof, not the default.

## 5. Reporting a range, not a point

A single EAC implies a precision the data does not support. Better practice is to report:

- the **selected** EAC and its method;
- the **assumption** it rests on, in one sentence;
- a **range** bounded by the plausible methods, with the assumption at each end;
- what would move the forecast, and by roughly how much.

A range is not evasion. It is a statement about which uncertainty is real and which is resolvable. If the
range is wide because nobody has re-estimated the remaining scope, say so — that is a resource decision the
sponsor can take. If it is wide because a major commercial position is unresolved, that is a different
decision and belongs to a different person.

The **to-complete performance index (TCPI)** is the credibility test that should accompany any forecast that
is better than method B. It states the cost efficiency the remaining work must achieve. Comparing it with
the efficiency actually achieved so far converts an optimistic forecast into a specific, answerable claim:
*this forecast requires the team to perform 20 % better for the rest of the project than it has to date —
here is why that is possible.* If no such answer exists, the forecast is not a forecast.

## 6. Defending it to a board

A board does not want the method; it wants to know whether to act. Four sentences carry a forecast:

1. **The number and its movement.** "The forecast at completion is USD 19.2 million, up 0.4 million on last
   month."
2. **The method and the assumption in the same breath.** "It is a bottom-up re-estimate of the remaining
   scope, because the commissioning phase ahead does not resemble the construction behind us."
3. **The credibility test.** "It assumes the remaining work runs about 7 % more efficiently than the work to
   date, which the recovery plan is built to deliver."
4. **The trigger.** "If the commissioning schedule slips beyond the end of next quarter, extended
   preliminaries add roughly 0.6 million and the forecast moves to the top of the range."

What must not happen is the reverse order — the method explained first, the number last, the assumption
never. And a forecast should never be presented as having "improved" because the method changed. Changing
method between periods is legitimate when the cause changed; it is a reporting failure when it is not
disclosed. State the previous method, the new method, and the reason, every time.

Automation makes this discipline more important, not less. A tool can compute the whole family in
milliseconds and rank the results; it cannot know whether the commissioning phase resembles the civils. AI
proposes; the professional disposes — and signs.

## 7. How this goes wrong

**One method, applied forever, because it is in the template.** The most common failure. `BAC ÷ CPI` becomes
the house forecast, and the assumption embedded in it is never revisited even when the project changes
character entirely.

**Method chosen by the answer it gives.** Selecting downwards to protect a position, or upwards to build
private contingency, is the same defect in opposite directions. Both are visible over time as forecast bias
— a project whose EAC only ever moves in one direction was not forecasting, it was conceding.

**Rounding the index before dividing by it.** `CPI = 0.9` used in place of 0.897 shifts a USD 20 million EAC
by more than a quarter of a million. Carry full precision through the calculation and round only the
presented result.

**Forecasting the whole project as one number.** EAC computed on project totals hides offsetting movements:
a control account 400,000 under and another 900,000 over produce a comfortable-looking aggregate. Forecast
at control account level, then aggregate.

**Mixing the reserves into the forecast.** Contingency drawn down for a materialised risk belongs in the
forecast; contingency still held against open risk does not belong in an EAC as though it were spent, and
management reserve is outside the baseline altogether. `BPG-10 — Contingency and management reserve` sets
out the treatment; confusing the two is how a project reports an overrun it does not have, or hides one it
does.

**A bottom-up ETC that was not actually built bottom-up.** "We re-estimated" frequently means the previous
number was adjusted by a judgement. A genuine bottom-up ETC has a scope list, quantities, rates and a
basis-of-estimate note. Anything else is method A with a different label.

**Silent method changes.** The forecast improves month on month and the reason is that the method rotated.
This destroys the credibility of the whole reporting line, because a reader who spots it once will discount
every subsequent forecast.

**Ignoring what the forecast implies about the schedule.** An EAC that assumes recovery of cost efficiency
while the programme assumes the current durations is two documents describing different projects.

## 8. Worked example

*Illustrative figures.* Currency USD; data date the end of Month 10 of an 18-month baseline; all figures
cumulative; indices carried at full precision and presented to four decimal places; a single control
account portfolio.

**The shared data set.**

| Quantity | Value |
|---|---:|
| Budget at completion (BAC) | 18,000,000 |
| Planned value (PV) | 7,680,000 |
| Earned value (EV) | 7,200,000 |
| Actual cost (AC), including accruals | 8,000,000 |

Derived:

```
CPI      = EV ÷ AC = 7,200,000 ÷ 8,000,000 = 0.9000
SPI      = EV ÷ PV = 7,200,000 ÷ 7,680,000 = 0.9375
CPI × SPI = 0.9000 × 0.9375 = 0.84375
Percent complete = EV ÷ BAC = 7,200,000 ÷ 18,000,000 = 40.0 %
Work remaining at budget = BAC − EV = 18,000,000 − 7,200,000 = 10,800,000
Budget remaining in cash  = BAC − AC = 18,000,000 − 8,000,000 = 10,000,000
```

**Method A — remaining work at budget.**

```
ETC = BAC − EV = 10,800,000
EAC = AC + ETC = 8,000,000 + 10,800,000 = 18,800,000
VAC = BAC − EAC = 18,000,000 − 18,800,000 = (800,000)
```

**Method B — remaining work at current cost performance.**

```
ETC = (BAC − EV) ÷ CPI = 10,800,000 ÷ 0.9000 = 12,000,000
EAC = AC + ETC = 8,000,000 + 12,000,000 = 20,000,000
Cross-check: EAC = BAC ÷ CPI = 18,000,000 ÷ 0.9000 = 20,000,000 ✓
VAC = 18,000,000 − 20,000,000 = (2,000,000)
```

**Method C — cost and schedule compounding.**

```
ETC = (BAC − EV) ÷ (CPI × SPI) = 10,800,000 ÷ 0.84375 = 12,800,000
EAC = 8,000,000 + 12,800,000 = 20,800,000
VAC = 18,000,000 − 20,800,000 = (2,800,000)
```

**Method D — bottom-up re-estimate of the remainder.** The remaining scope is re-estimated from quantities
and rates rather than extrapolated:

| Remaining scope element | ETC |
|---|---:|
| Remaining installation work | 6,900,000 |
| Commissioning and handover | 2,400,000 |
| Extended site preliminaries for the forecast two-month slip | 1,300,000 |
| Escalation on subcontract packages not yet let | 600,000 |
| **ETC (bottom-up)** | **11,200,000** |

```
EAC = AC + ETC = 8,000,000 + 11,200,000 = 19,200,000
VAC = 18,000,000 − 19,200,000 = (1,200,000)
```

**The spread, side by side.**

| Method | Assumption | EAC | VAC |
|---|---|---:|---:|
| A — remaining work at budget | The variance was a closed, one-off event | 18,800,000 | (800,000) |
| D — bottom-up re-estimate | The remainder is different work, priced directly | 19,200,000 | (1,200,000) |
| B — current cost performance persists | The inefficiency is structural | 20,000,000 | (2,000,000) |
| C — cost and schedule compound | Lateness is converting into cost | 20,800,000 | (2,800,000) |

The four forecasts span `20,800,000 − 18,800,000 = 2,000,000`, which is `2,000,000 ÷ 18,000,000 = 11.1 %` of
the budget. That spread is not imprecision. It is the range of futures consistent with the same past, and
choosing within it is the professional act.

**The credibility test.**

```
TCPI to BAC = (BAC − EV) ÷ (BAC − AC) = 10,800,000 ÷ 10,000,000 = 1.0800
```

Landing on budget requires the remaining 60 % of the work to run at a cost efficiency of 1.08 when the first
40 % achieved 0.90 — an improvement of `1.0800 ÷ 0.9000 = 1.20`, or 20 %, sustained to completion. Absent a
named and funded change, that is not a forecast; it is a wish.

Apply the same test to the selected forecast:

```
TCPI to EAC (method D) = (BAC − EV) ÷ (EAC − AC)
                       = 10,800,000 ÷ (19,200,000 − 8,000,000)
                       = 10,800,000 ÷ 11,200,000 = 0.9643
```

Even the bottom-up forecast assumes the remaining work runs at 0.9643 against 0.9000 achieved — an
improvement of `0.9643 ÷ 0.9000 = 1.0714`, about 7 %. That is a much smaller claim than 20 %, but it is
still a claim, and it belongs in the narrative. A forecast whose embedded improvement is never stated is
optimistic by default.

**Selecting, and saying why.** The remaining scope includes commissioning and handover — work of a different
character from the installation performed to date — so extrapolation methods are structurally unsuited, and
method D is chosen. The bottom-up ETC lands at 19,200,000, inside the range set by the formula methods,
which is a reassurance rather than a validation: it means the re-estimate has not drifted away from what
performance to date implies.

**The board paragraph.** "Forecast at completion USD 19.2 million against a budget of 18.0 million, a
projected overrun of 1.2 million. The basis is a bottom-up re-estimate of the remaining scope, chosen
because the commissioning phase ahead does not resemble the installation work behind us, so performance to
date does not predict it. The forecast assumes the remaining work runs about 7 % more cost-efficiently than
the work to date, which the revised supervision model is intended to deliver. If that improvement does not
materialise, the persisting-performance forecast is 20.0 million; if the two-month slip extends, extended
preliminaries take it towards 20.8 million. Recovery to the 18.0 million budget would require a 20 %
sustained improvement in cost efficiency and is not credible on current evidence."

**Assumptions this example depends on.** Earning rules are objective and unchanged; accruals are booked so
AC is complete at the data date; no instructed variation is awaiting incorporation into the baseline; the
contingency held against open risk is excluded from all four EAC figures and reported separately; escalation
is included only in the bottom-up ETC, where it is explicit.

## 9. Checklist

Use this before the forecast leaves your desk.

**Before calculating**

- [ ] The cost ledger is closed at the data date and accruals are booked.
- [ ] Instructed variations awaiting incorporation are identified and quantified separately.
- [ ] Contingency drawn down is separated from contingency still held.
- [ ] The cause of the cumulative variance has been investigated, not inferred from the index.

**Calculating**

- [ ] The forecast is built at control account level and aggregated, not computed on project totals.
- [ ] Indices are carried at full precision and rounded only for presentation.
- [ ] Where method B is used, both forms agree.
- [ ] Where method D is used, there is a scope list, quantities, rates and a basis-of-estimate note.
- [ ] Escalation and currency exposure are either in the ETC explicitly or excluded explicitly.

**Testing**

- [ ] TCPI to BAC computed, and the required improvement expressed as a percentage of performance to date.
- [ ] TCPI to the selected EAC computed, so the improvement embedded in your own forecast is visible.
- [ ] The forecast is consistent with the current schedule — the same slip, the same durations.
- [ ] A range is stated with the assumption at each end.

**Reporting**

- [ ] The method is named, and the assumption stated in one sentence.
- [ ] Any change of method since last period is disclosed with its reason.
- [ ] The movement since last period is explained by cause, not by method.
- [ ] The trigger that would move the forecast is named, with its approximate value.

---

## Related

- `BPG-08 — Earned value in practice` — the measurement discipline every EAC method depends on; read it
  first if your CPI is not trusted.
- `BPG-10 — Contingency and management reserve` — what belongs in the forecast, what sits beside it, and
  what sits outside the baseline entirely.
- `BPG-16 — Risk registers that work` — where the quantified exposure behind a forecast range comes from.
- `BPG-14 — Monthly reporting that gets read` — how the forecast and its assumption reach a decision-maker
  intact.
- `TPL-08 — Estimate at completion scenario comparison` — the instrument for running §8 on your own data.

## Sources and standards

The EAC family and the to-complete performance index are described in published earned value frameworks,
including the AACE International Total Cost Management framework and the PMBOK Guide; their principles are
explained here in our own words and no text or table is reproduced. The internal reference is BoK Domain 6
(EVM/EAC) and BoK Domain 3 (Budgeting & Forecasting). All figures in §8 are illustrative and were computed
for this document. One citation remains outstanding, marked in §4.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
