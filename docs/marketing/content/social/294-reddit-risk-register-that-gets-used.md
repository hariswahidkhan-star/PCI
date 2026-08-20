---
platform:      Reddit / forum — r/projectmanagement
type:          forum-post
title:         What a risk register is for, and why yours is ignored
meta:          Five priced risks came to £1.30m of expected value against £900k of contingency. That gap is what a risk register is for, and most never show it.
primary_kw:    risk register that gets used *
secondary_kw:  expected value, contingency, risk owner, cause event effect
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    1187
hashtags:      n/a (Reddit)
ab_id:         AB-01033
---

# What a risk register is for, and why yours is ignored

I priced a register on a £34m job last spring. Five live risks came to £1.30m of expected value against £900k of contingency held. That £400k gap had existed for seven months, in a document everybody had "reviewed" every month, because nobody had ever multiplied the two columns together.

Short answer: a risk register is a funding and decision document, not a list. If it does not change what money is held, who does what next, and by when, it is a compliance artefact and your team is right to ignore it.

## The register that was ignored, priced

| Risk | Probability | Impact if it lands | Expected value | Owner | Next decision |
|---|---:|---:|---:|---|---|
| Discharge permit late | 0.35 | £1,200k | **£420k** | Environmental lead | 14 Mar — pre-app meeting |
| Dewatering deeper than borehole log | 0.20 | £850k | **£170k** | Temporary works engineer | 28 Feb — trial pit result |
| Steel escalation above allowance | 0.50 | £640k | **£320k** | Procurement | 07 Mar — fix or float decision |
| Client-supplied switchgear late | 0.15 | £2,400k | **£360k** | Project director | 21 Feb — vendor schedule review |
| Archaeology in the north plot | 0.10 | £300k | **£30k** | Site manager | 03 Mar — watching brief closes |
| **Total** | | | **£1,300k** | | |

Contingency held: **£900k**. Shortfall against the mean: **£400k**.

Two things fall out of that table that a list of hazards cannot give you.

First, the shortfall itself. Expected value totals £1.30m, so if you fund £900k you are funding below the average outcome before considering anything unlisted.

Second, and more useful: the switchgear risk has the second-largest expected value but by far the largest single downside. If it lands, the entire contingency covers 900 ÷ 2,400 = **37.5%** of it.

A register sorted by expected value would put it fourth. Sort by impact as well as by expected value, always, because expected value hides the tail and the tail is what ends careers.

## Expected value is a mean, not a funding level

This is the part that gets misused most. Multiplying probability by impact gives you the average of many outcomes. It does not give you a confidence level, and no single project is run many times.

On a typical cost risk distribution — bounded below, long-tailed above — the mean sits above the median. Funding the mean therefore buys you somewhat better than even odds, and nowhere near the P80 that most owners think they are funding. If your governance says P80, expected value alone cannot get you there; you need a simulation over the same register, with correlation set deliberately rather than left at zero.

Say that out loud in the meeting. "This is the mean, not the P80" is the single most valuable sentence in risk management and it takes four seconds.

## Why yours is ignored — the five failures

**The entries are conditions, not risks.** "Bad weather." "Supply chain." "Covid." A risk has a cause, an event and an effect: *because the discharge consent depends on a regulator with a 12-week service standard, the permit may arrive after the planned dewatering start, which would push the sub-structure into the winter window and cost £1.2m.* You cannot price the first version. You can price the second, and the sentence itself tells the owner what to do.

**No owner, or an owner who does not know.** A department is not an owner. "Commercial" cannot be phoned. One name, and they must have been told.

**No next decision date.** A risk without a date is a worry. A risk with a date is an agenda item. The right-hand column above is the one that makes people turn up.

**Nothing is ever closed.** Registers grow. A register with 140 open lines on a job with 8 real exposures is unreadable, and unreadable means unread. Close things loudly.

**It is never reconciled to contingency.** This is the fatal one. If contingency moves and the register does not, or the register moves and contingency does not, the register is decorative.

## The drawdown test

Run this every month; it takes two minutes and it is the fastest way to tell whether a register is alive.

At 45% complete, contingency drawn should be broadly in step with risk retired. If you are 45% through and have drawn 6% of contingency, one of two things is true: the risks are all still in front of you, or the register no longer reflects the project. It is almost never the first.

Plot contingency remaining against risk exposure remaining, month by month, on one chart. A healthy job shows both falling together. A job in trouble shows exposure flat and contingency falling, which means you are paying for things that were never on the register at all.

## What a register that gets used looks like

Eight to fifteen live lines on a mid-sized job. Every line phrased cause-event-effect. Every line with one named owner, a probability, an impact range and a next decision date. Priced monthly, reconciled to contingency monthly, closed items removed to an archive tab with the outcome recorded.

Then one page at the front: total expected value, contingency held, the gap, and the three lines that account for most of the exposure. That page is what a board reads. Everything behind it is working paper.

## Common follow-ups

**Should I use a 5×5 matrix?**
For screening, yes. For funding, no. A matrix tells you which risks to work on; it cannot tell you how much money to hold, because "high/high" is not a number. Use the matrix to sort and expected value or simulation to fund.

**How do I get impact ranges when nobody will commit?**
Ask for the three points separately and never at once: what does this cost if it goes about as expected, what if it goes well, what if it goes badly. People will not give you a range but they will answer three questions, and the range is what you wanted.

**What about opportunities?**
Price them the same way, with negative impact, and hold them in the same register. An opportunity with a probability and an owner behaves like a risk; one without either is a wish, and it should not be netting off your contingency.

**How often should it be reviewed?**
Monthly with the full team, and immediately whenever a decision date on the register arrives. Weekly reviews of an unchanged register are how registers become ignored in the first place.

---

*Disclosure: I write for the Project Controls Institute. One link, at the end, and the table above is the whole point of the post: [how to build a risk register stakeholders actually use](https://projectcontrolsinstitute.org/risk-register-that-gets-used).*

*Internal links: the in-post link uses the anchor "how to build a risk register stakeholders actually use". Comment replies should use [how to run a Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) and [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) with those anchors.*
