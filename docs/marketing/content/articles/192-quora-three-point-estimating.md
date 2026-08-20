---
platform:      Quora
type:          qa-list
title:         What is three-point estimating in project management?
meta:          Three-point estimating uses optimistic, most likely and pessimistic values to produce a weighted duration or cost. Worked PERT arithmetic and where it breaks.
primary_kw:    three-point estimating
secondary_kw:  PERT formula, triangular distribution, estimate uncertainty, standard deviation
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1496
hashtags:      n/a (Quora)
ab_id:         AB-00273
---

# What is three-point estimating in project management?

Three-point estimating replaces a single guessed number with three: optimistic (O), most likely (M) and pessimistic (P). Those are combined into a weighted expected value, either as the simple average, (O + M + P) ÷ 3, or as the PERT weighting, (O + 4M + P) ÷ 6. PERT is what most estimating procedures specify.

The technique applies to durations and to costs. The arithmetic is identical; only the units change.

## Why is a single-point estimate not enough?

A single number carries no information about how wrong it might be. "Fourteen days" and "fourteen days, but anywhere between eight and twenty-two" describe very different pieces of work, and only the second lets anyone set a contingency.

Three-point estimating forces the estimator to say the range out loud. That is most of the value, before any formula is applied.

It also exposes skew. Work that can go badly wrong but cannot go spectacularly right — most construction and commissioning work — has a long right tail, and the average of that tail sits above the most likely value.

## How do you calculate a three-point estimate?

Take a single activity: install and terminate switchgear. The estimator gives O = 8 days, M = 12 days, P = 22 days.

Triangular (simple average): (8 + 12 + 22) ÷ 3 = 42 ÷ 3 = **14.0 days**.

PERT (beta weighting): (8 + 4 × 12 + 22) ÷ 6 = (8 + 48 + 22) ÷ 6 = 78 ÷ 6 = **13.0 days**.

Standard deviation, under the usual PERT assumption that the range spans about six standard deviations: (P − O) ÷ 6 = (22 − 8) ÷ 6 = **2.33 days**. Variance is the square of that, 5.44.

Note what the numbers say. The most likely value is 12 days, but the expected value is 13 days, because the downside is ten days long and the upside only four. Planning to 12 is planning to be late more often than not.

## Which three-point estimating formula should you use?

| Method | Formula | Result here | What it assumes | When it fails |
|---|---|---:|---|---|
| Single point | one number | 12.0 days | Nothing, and admits nothing | Always, silently |
| Triangular | (O + M + P) ÷ 3 | 14.0 days | All three values equally informative | The pessimistic value is a worst case nobody believes, and it drags the mean up |
| PERT / beta | (O + 4M + P) ÷ 6 | 13.0 days | The most likely value carries four times the weight | M is anchored on the original single-point guess |
| Monte Carlo | simulate the network | distribution | Distributions and correlations are stated | Inputs are invented, or correlation is set to zero |

PERT is the sensible default for an individual activity because it respects the estimator's judgement about the most likely case while still pricing the tail. Triangular is defensible when the three points come from data rather than opinion.

Neither is a substitute for simulation across a network. Both are a very good substitute for a single number.

## How do three-point estimates combine across several activities?

This is where most people get it wrong, and where the technique earns its keep. Expected values add. Standard deviations do not — variances add, and you take the square root at the end.

Three activities in sequence:

| Activity | O | M | P | PERT mean | SD | Variance |
|---|---:|---:|---:|---:|---:|---:|
| A. Switchgear install | 8 | 12 | 22 | 13.0 | 2.33 | 5.44 |
| B. Cable pulling | 5 | 6 | 13 | 7.0 | 1.33 | 1.78 |
| C. Testing and energisation | 10 | 14 | 18 | 14.0 | 1.33 | 1.78 |
| **Total** | 23 | 32 | 53 | **34.0** | **3.00** | **9.00** |

The arithmetic: means 13.0 + 7.0 + 14.0 = 34.0 days. Variances 5.44 + 1.78 + 1.78 = 9.00, and the square root of 9.00 is exactly 3.00 days.

Now compare two ways of answering "how long, to be safe?". Adding the pessimistic values gives 22 + 13 + 18 = **53 days**. Taking the expected value plus 0.84 standard deviations, which is roughly the 80th percentile of a normal distribution, gives 34.0 + (0.84 × 3.00) = **36.5 days**.

The sum of worst cases is about 45% longer than a genuine P80. Adding pessimistic values across a chain assumes every activity goes wrong at once, and that is not a plan, it is a fear.

## What does correlation do to the answer?

The variance-addition rule above assumes the three activities are independent. On a real site they rarely are: the same crew, the same weather, the same access constraint, the same drawing package.

If the activities were perfectly correlated, the standard deviations would add instead: 2.33 + 1.33 + 1.33 = 5.00 days. The P80 then becomes 34.0 + (0.84 × 5.00) = **38.2 days**, not 36.5.

That is a 1.7-day difference on three activities. Across a hundred-activity programme, assuming independence is the single largest source of false confidence in a risk analysis, and it always understates the tail.

## Where does the estimate meet the accounts?

The expected value from a three-point estimate is a mean, not a target, and the two get budgeted differently. A budget set at the mean will be exceeded roughly half the time by design, which is why the contingency sits above it and is drawn down against named risks.

On the cost side the same arithmetic runs on money. Given O = £180,000, M = £220,000 and P = £340,000: PERT gives (180 + 880 + 340) ÷ 6 = 1,400 ÷ 6 = **£233,000**, with a standard deviation of (340 − 180) ÷ 6 = **£26,700**.

Carrying £233,000 rather than £220,000 changes total expected costs, and total expected costs is the denominator in a cost-based measure of progress. An estimating convention therefore changes reported revenue on a contract measured over time, which is why finance has a legitimate interest in which weighting the estimating procedure specifies.

The PCI AI Project Finance Leader (PFL-AI) credential covers 16 domains and 61 knowledge areas across exactly that overlap, and its calculation content is verified by a suite of 15,613 machine calculation checks covering PFL-AI and PML-AI, all passing.

## Frequently asked questions

**Is three-point estimating the same as PERT?**
No. PERT is one weighting of a three-point estimate, the one that multiplies the most likely value by four. The simple triangular average is also a three-point estimate. PERT additionally supplies a standard deviation from the range, which is what makes it useful for building contingency rather than just a duration.

**What should the pessimistic value actually represent?**
A bad but credible outcome, not a catastrophe. If P includes a flood, a strike and a supplier collapsing, the estimate stops being a distribution and becomes a risk register in disguise. Named, low-probability events belong in the risk register with their own quantification; P covers ordinary variation in the same work.

**Does three-point estimating replace a schedule risk analysis?**
It does not. Three-point estimates are the inputs a quantitative schedule risk analysis consumes, and simulation is what handles merge bias, correlation and path convergence. Doing three-point estimates well makes a later simulation credible; doing them badly makes the simulation confidently wrong.

**How do you stop estimators anchoring on their original number?**
Ask for the optimistic and pessimistic values first, and the most likely last. Asking for M first means O and P get set as a comfortable band around it, and the range collapses. Also ask what would have to be true for O to happen — the answer usually reveals whether O is real or decorative.

**Why is the expected value higher than the most likely value?**
Because the distribution is skewed to the right. Work can overrun by far more than it can under-run, so the long tail pulls the mean above the mode. Whenever P − M is larger than M − O, expect the calculated estimate to exceed the estimator's instinct, and expect them to object to it.

**Can AI produce the three points?**
It can propose ranges from comparable historical activities and flag where an estimator's range is far narrower than the outcomes their organisation has actually delivered. It should not set the final values, because the estimator has to defend them. A range nobody will sign is not an estimate.

---

*Internal links: this answer should link once, at the end, to [quantitative schedule risk analysis](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) with the anchor "how these ranges feed a schedule risk analysis", and to [Monte Carlo cost simulation](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) with the anchor "running the same inputs through a cost simulation"; Quora links are nofollow, so judge this on qualified traffic, not backlinks.*
