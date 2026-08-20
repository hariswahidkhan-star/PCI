---
platform:      Hashnode
type:          how-to
title:         Build a risk register that gets used: the ten columns
meta:          A risk register that gets used carries a cause, an event, an owner and a number on every row. The ten-column schema, the maths and the review cycle.
primary_kw:    risk register that gets used
secondary_kw:  risk register template, expected value contingency, risk breakdown structure, qualitative risk analysis
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/risk-register-that-gets-used
schema:        HowTo
word_count:    1798
hashtags:      #productivity #datascience #python #tutorial
ab_id:         AB-01037
---

# Build a risk register that gets used: the ten columns

A risk register that gets used has rows that each name a cause, an event and an effect, carry one named owner, hold money or days, and come back on a fixed date with a decision attached. Registers get ignored when they hold two hundred unowned, unquantified rows nobody can act on.

This is the schema, the arithmetic and the cadence. About a day to set up, about two hours a month to run.

## What makes a risk register that gets used

A risk register is the controlled list of uncertain events that would change a project's cost, schedule, scope or safety if they occurred, each with an assessed likelihood, an assessed effect, an accountable owner and an agreed response.

It is a decision tool, not an archive: a row that cannot change what anybody does this month does not belong near the top of it.

Two things get filed in registers that are not risks. An issue has already happened, so it belongs in the issues log. An assumption becomes a risk only when somebody writes down what happens if it fails.

## The ten-column schema

Anything beyond these ten columns will mostly not be maintained past month three.

| Column | Type | What goes in it | Why it earns its place |
|---|---|---|---|
| `id` | text, immutable | Stable reference, never reused | Lets minutes, change papers and drawdowns cite one row |
| `statement` | text | Cause, event, effect in one sentence | Forces the row to describe something manageable |
| `category` | enum | Node from the risk breakdown structure | Shows where risk clusters; the total never does |
| `owner` | text | One named person, never a team | An unowned risk is a note |
| `probability` | float 0–1 | A probability you would actually bet on | Feeds the arithmetic |
| `cost_impact` | currency | Range or single figure | Turns a colour into a decision |
| `schedule_impact` | integer days | Working days on the affected path | Separates a delay from a nuisance |
| `response` | enum + text | Avoid, reduce, transfer or accept, plus the action | The only column that changes the future |
| `next_action_date`, `status` | date, enum | A date and a state | Makes review possible, not ceremonial |
| `movement` | derived | Value at last review against value now | The most-read line on the page |

Store `probability` as a float, not a category. Once likelihood is held as "High", expected value cannot be computed and the register degrades into colour.

Store `schedule_impact` in working days on the affected path, not calendar days on the activity; whether that path drives handover is what schedule risk analysis tests.

Movement is the column senior people read first. A risk that has not moved in four months is either well managed or not being looked at, and the register should show which.

## How to write a risk somebody can act on

Use cause, event, effect: because *cause*, *event* may happen, which would result in *effect*. "Ground conditions" is a topic, not a risk, and cannot be owned, priced or closed.

"Because the ground investigation covered eleven boreholes across a fourteen-hectare site, obstructions may be found outside the investigated area, which would add excavation and disposal cost to the substructure package" can be owned. It also names its own responses: extend the investigation, price the disposal now, or hold contingency against it.

Read the row aloud and ask what you would do on Monday. If there is no answer, the row is not finished.

## Three ways of sizing a risk, compared

| Method | What it gives you | What it assumes | Effort | Where it fails |
|---|---|---|---|---|
| Five-by-five matrix | A rank order and a colour | That ordinal scores are enough to triage | An hour in a workshop | Ranks get multiplied and read as values; ties hide differences |
| Expected value (p × impact) | A mean per risk and a total | That you can state a probability and an impact you would defend | Half a day | A mean never happens; ignores correlation and the tail |
| Three-point ranges plus Monte Carlo | A distribution with confidence levels such as P50 and P80 | Ranges and correlations from somewhere real | Two to five days | Weak inputs give a confident-looking wrong curve |

The five-by-five matrix is a triage tool, not arithmetic. An impact scored 4 is not twice an impact scored 2, so multiplying ranks gives a number with no units, and two risks scoring 12 can differ by an order of magnitude in money.

Run all three in order. The matrix picks the rows worth quantifying, expected value sets the opening contingency, and [a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) tests whether it survives the tail.

## The arithmetic, shown

Six quantified risks on a substructure and steel package, with the probabilities their owners signed up to and the most likely cost if each occurs.

| Risk | Probability | Cost impact | Expected value |
|---|---:|---:|---:|
| Obstructions outside the investigated area | 0.30 | £900,000 | £270,000 |
| Vendor drawings late, steel erection delayed 3 weeks | 0.45 | £260,000 | £117,000 |
| Consent condition requires an acoustic barrier | 0.20 | £140,000 | £28,000 |
| Third-party utility diversion slips past the window | 0.35 | £320,000 | £112,000 |
| Roof package pushed into winter working | 0.55 | £90,000 | £49,500 |
| Commissioning spares shortfall | 0.15 | £60,000 | £9,000 |
| **Total** | | | **£585,500** |

The steel impact of £260,000 is built, not guessed. Three weeks is fifteen working days, time-related site costs run at £14,000 a day, and the erection subcontractor quoted £50,000 for an extended shift: 15 × 14,000 + 50,000 = £260,000.

Three lines do the rest, and writing them forces the probability column to hold numbers.

```python
risks = [(0.30, 900_000), (0.45, 260_000), (0.20, 140_000),
         (0.35, 320_000), (0.55, 90_000), (0.15, 60_000)]
ev = sorted((p * impact for p, impact in risks), reverse=True)
print(sum(ev), sum(ev[:2]) / sum(ev))   # 585500.0  0.661...
```

The £585,500 total is a mean, and a mean is the one outcome that will not occur. It is a reasonable opening contingency figure and a poor closing one.

Concentration matters more than the total. Two rows carry £387,000 of the £585,500, which is 66 per cent, so the management effort belongs on two risks rather than six.

## Ownership, and what it actually means

One person. Named. Present at the review, or represented by somebody who can answer.

Ownership means four things: maintaining the probability and impact, executing the agreed response, raising it when the response stops working, and closing it when the exposure has gone.

A risk owned by "the project team" is unowned, and so is one owned by the risk manager, who administers the register and delivers none of its responses.

## The review cycle

| Tier | Which rows | Cadence | Who is in the room |
|---|---|---|---|
| Top exposures | Ten highest by expected value | Fortnightly | Project manager, package leads, risk owner |
| Quantified register | Rows carrying money or days | Monthly, with the cost report | Project controls, package leads |
| Full register | Everything, accepted rows included | Quarterly, and at every gate | Sponsor, project manager, controls |
| Emergent | New rows | On arrival | Whoever raised it, plus the named owner |

Align the monthly review with the cost report date rather than holding a separate risk meeting. The forecast and the contingency position are one conversation.

## Where the register meets the money

Contingency comes out of the register, and that is where project controls and finance must agree on vocabulary.

Contingency held inside a project budget against identified risks is a control account. An accounting provision is something else, with recognition criteria of its own under the accounting standards, tested by finance rather than set in a risk workshop.

This is the overlap the discipline keeps failing at. An engineer is examined on probability and float and almost never on when an obligation may be recognised; an accountant, the reverse. The contingency line sits in both, which is why the PCI AI Project Controls Leader (PCL-AI) examines risk inside the same 13 domains and 61 knowledge areas as cost control and reporting.

Publish the drawdown curve beside the register: contingency at sanction, drawn to date, remaining, and remaining exposure. If contingency has fallen faster than exposure, the project is in trouble and the register shows it first.

## Frequently asked questions

**How many rows should a risk register have?**
Enough to cover the material exposure and few enough to review properly. On a mid-sized capital project that is twenty to forty live rows, ten to fifteen of them quantified. Registers of a hundred and forty rows are filing exercises: nobody reads past row twenty.

**What is the difference between a risk and an issue?**
A risk has not happened yet and carries a probability below 1. An issue has happened, so it needs an action and a date rather than a score. Keeping both in one table corrupts the arithmetic, because a certain event inflates the expected-value total that contingency is set from.

**Should the register include opportunities as well as threats?**
Yes, scored the same way with a negative cost impact. Opportunities get dropped because nobody owns them and nobody is measured on capturing them. Give each one an owner and a date in the same review, or they become decoration on a page about threats.

**How do you set contingency from the register?**
Expected value gives the opening figure and a Monte Carlo simulation across the same inputs gives the distribution you commit against. Most owners hold contingency between the mean and the P80, keeping the P50 to P80 gap as management reserve so that using it is a governance decision.

**Who should chair the risk review?**
The project manager, not the risk manager. Whoever chairs is the person the room believes will act. A risk manager can prepare the register and challenge the numbers, but if they chair, the meeting reads as an administrative return rather than a decision.

---

*First published on projectcontrolsinstitute.org; the republishing field in Draft Settings carries the canonical home, so this copy exists for the tag feeds.*

*Internal links: one is now in the body. "A Monte Carlo cost simulation" points at projectcontrolsinstitute.org/monte-carlo-cost-simulation, kept because it sits in the strongest sentence in the piece: the three sizing methods run in order, and that sentence raises how the tail gets tested once expected value has set the opening contingency. The second link to the same domain, on schedule risk analysis in the column-schema section, was removed and the sentence left standing; one link per domain per piece is the cap, and that aside was the weaker placement. No second domain earns a link here. Reciprocal: the Monte Carlo cost simulation page should point back for the register that supplies its inputs, with an anchor about probability and impact columns rather than about risk registers generally.*
