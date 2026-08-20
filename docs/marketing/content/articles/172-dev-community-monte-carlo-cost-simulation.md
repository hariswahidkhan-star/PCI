---
platform:      DEV Community
type:          how-to
title:         Monte Carlo cost simulation in Python, step by step
meta:          How to run a Monte Carlo cost simulation in Python: ranging an estimate, setting correlation with a copula, and reading contingency off the P80 result.
primary_kw:    Monte Carlo cost simulation
secondary_kw:  cost contingency, P80 estimate, Gaussian copula, three-point estimate
pillar:        Risk management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/monte-carlo-cost-simulation
schema:        HowTo
word_count:    1747
hashtags:      #python #datascience #statistics #tutorial
ab_id:         AB-01051
---

# Monte Carlo cost simulation in Python, step by step

A Monte Carlo cost simulation samples every uncertain line in an estimate thousands of times, records the total each time, and reads contingency off the resulting distribution at a chosen confidence, usually P80. The code is short. The work is in the ranges, the correlation and the discrete risk events, and this walks through all three.

Everything below runs on an eight-line estimate, with arithmetic you can check on paper and output from an actual run.

## Step 1 — Write the question down before opening an editor

One sentence, fixed before any code: "How much contingency does this estimate need to be 80% likely to hold, at sanction, excluding scope change?"

That sentence pins four things: the confidence level, the decision point, the base estimate being tested, and what is deliberately outside the model.

State the exclusions explicitly. Scope growth, foreign exchange and force majeure are usually funded separately, and a reader who is not told will assume they are inside your number.

## Step 2 — Build a risk model, not a copy of the estimate

Roll the estimate up to between 15 and 40 lines. Every line must be one a human can range with a straight face.

A 4,000-line estimate cannot be ranged honestly in the time available, so its inputs get guessed, and a simulation over guessed inputs returns a precise answer to a question nobody asked.

Keep the structure recognisable to the estimator. If the risk model does not reconcile line by line to the base estimate, the first challenge in review is about your mapping rather than about risk.

## Step 3 — Range each line

Each line needs optimistic, most likely and pessimistic values from evidence: tender returns, historic outturn, benchmark rates, or a structured interview with the package owner.

The PERT mean is (O + 4M + P) / 6, and the common σ shortcut is (P − O) / 6. Values in £m.

| Estimate line | O | M | P | PERT mean | σ |
|---|---:|---:|---:|---:|---:|
| Civils and earthworks | 8.00 | 9.50 | 14.00 | 10.000 | 1.000 |
| Structural steel | 5.00 | 6.00 | 9.00 | 6.333 | 0.667 |
| Mechanical | 11.00 | 13.00 | 18.00 | 13.500 | 1.167 |
| Electrical, instrumentation and control | 6.50 | 7.50 | 10.50 | 7.833 | 0.667 |
| Commissioning | 2.00 | 2.50 | 4.50 | 2.750 | 0.417 |
| Site preliminaries (time-related) | 4.00 | 4.80 | 7.20 | 5.067 | 0.533 |
| Design and engineering | 3.00 | 3.40 | 4.60 | 3.533 | 0.267 |
| Owner's costs | 1.50 | 1.80 | 2.60 | 1.883 | 0.183 |
| **Total** | | **48.50** | | **50.900** | |

The base estimate — the sum of most likely values — is **£48.50m**. The sum of the PERT means is **£50.90m**.

That £2.40m gap exists before a single risk event is added. The most likely value is the mode of a right-skewed line, the mean of a right-skewed line sits above its mode, and summing modes across eight lines compounds the error eight times.

## Step 4 — Discrete risks are not ranges

Ranging captures how much a line varies when work goes normally. It does not capture an event that either happens or does not.

| Risk event | Probability | Impact range (£m) | Mean impact | Expected value |
|---|---:|---|---:|---:|
| Ground conditions worse than the boreholes indicated | 35% | 1.20 / 2.00 / 4.00 | £2.20m | £0.770m |
| Permit delay of 6–14 weeks at £0.11m a week | 25% | 0.66 / 0.99 / 1.54 | £1.027m | £0.257m |
| Long-lead vendor fails and the package is re-let | 10% | 2.40 / 3.40 / 5.00 | £3.50m | £0.350m |
| **Total expected value** | | | | **£1.377m** |

Read that total carefully, because it is the most misused number in risk work. The project will never spend £0.770m on ground conditions. It spends nothing, 65% of the time, or between £1.20m and £4.00m.

Expected value is a portfolio number. The distribution is what you fund, which is the entire reason the simulation exists.

## Step 5 — Correlation is the assumption that moves the answer

If every line is sampled independently, the total's variance is the sum of the line variances. Squaring and summing the eight σ values gives 3.813, so σ ≈ **£1.95m**.

If the lines move together — one steel market, one labour market, one weather window — σ becomes the simple sum of the line values, **£4.90m**.

| Correlation assumption | σ of the total | P80 ≈ mean + 0.84σ | Contingency over £48.50m base |
|---|---:|---:|---:|
| Fully independent | £1.95m | £52.54m | £4.04m (8.3%) |
| Fully correlated | £4.90m | £55.02m | £6.52m (13.4%) |

The same estimate, the same ranges, and **£2.48m** of difference from an assumption most models never state. Reality sits between: commodity-linked lines usually carry a positive correlation of roughly 0.3 to 0.5 with each other, and owner's costs mostly do not.

## Step 6 — The code

PERT is a four-parameter beta. Convert the three points into beta shapes, then sample.

```python
import numpy as np
from scipy.stats import beta as beta_dist, norm

# line: (optimistic, most likely, pessimistic) in £m
LINES = {
    "civils":        (8.00,  9.50, 14.00),
    "steel":         (5.00,  6.00,  9.00),
    "mechanical":   (11.00, 13.00, 18.00),
    "eic":           (6.50,  7.50, 10.50),
    "commissioning": (2.00,  2.50,  4.50),
    "preliminaries": (4.00,  4.80,  7.20),
    "design":        (3.00,  3.40,  4.60),
    "owners_costs":  (1.50,  1.80,  2.60),
}

# (probability, optimistic, most likely, pessimistic) impact in £m
RISKS = [
    (0.35, 1.20, 2.00, 4.00),   # ground conditions
    (0.25, 0.66, 0.99, 1.54),   # permit delay, 6-14 weeks at £0.11m/week
    (0.10, 2.40, 3.40, 5.00),   # long-lead vendor fails, package re-let
]

def pert_shape(o, m, p, lam=4.0):
    """Beta shape parameters for a three-point PERT line."""
    mean = (o + lam * m + p) / (lam + 2)
    if np.isclose(mean, m):
        return 3.0, 3.0
    a = ((mean - o) * (2 * m - o - p)) / ((m - mean) * (p - o))
    return a, a * (p - mean) / (mean - o)

def simulate(rho_val, n_iter=100_000, seed=20260420):
    rng = np.random.default_rng(seed)
    names = list(LINES)
    n = len(names)

    # Gaussian copula: correlate uniforms, then push each through its own PERT
    rho = np.full((n, n), rho_val)
    np.fill_diagonal(rho, 1.0)
    u = norm.cdf(np.linalg.cholesky(rho) @ rng.standard_normal((n, n_iter)))

    total = np.zeros(n_iter)
    for i, name in enumerate(names):
        o, m, p = LINES[name]
        a, b = pert_shape(o, m, p)
        total += o + beta_dist.ppf(u[i], a, b) * (p - o)

    for prob, o, m, p in RISKS:
        a, b = pert_shape(o, m, p)
        impact = o + beta_dist.ppf(rng.random(n_iter), a, b) * (p - o)
        total += (rng.random(n_iter) < prob) * impact

    return total

total = simulate(0.40)
p50, p80 = np.percentile(total, [50, 80])
print(f"mean {total.mean():.2f}  P50 {p50:.2f}  P80 {p80:.2f}")
```

Two notes on the copula. The Cholesky factor requires the correlation matrix to be positive semi-definite, so a hand-built matrix of arbitrary pairwise guesses will sometimes fail to factor — that failure is information, not a bug. And a Gaussian copula with parameter ρ produces a rank correlation close to ρ but not identical to it.

## Step 7 — Check it has settled

Run at increasing iteration counts and re-run each with a different seed. Actual P80 output from the code above, in £m, at ρ = 0.40.

| Iterations | Seed A | Seed B | Spread |
|---:|---:|---:|---:|
| 1,000 | 55.65 | 55.37 | 0.28 |
| 5,000 | 55.51 | 55.57 | 0.06 |
| 10,000 | 55.59 | 55.55 | 0.04 |
| 100,000 | 55.66 | 55.68 | 0.02 |

Stop when the P80 moves by less than the precision you will report. More iterations improve precision and never accuracy: a poor model run 100,000 times is still a poor model, to four decimal places.

## Step 8 — Read the result and set contingency

At 100,000 iterations the run returns a mean of **£52.29m**, which sits within £0.01m of the hand figure of 50.90 + 1.38 = £52.28m. That agreement is the check that the sampling is doing what the table says.

At ρ = 0.40 the P50 is **£52.03m** and the P80 is **£55.66m**. Against the £48.50m base, contingency is **£7.16m**, or **14.8%**.

Run it independently and the P80 falls to **£54.46m**, contingency **£5.96m** or 12.3%. The correlation setting is worth £1.20m on this model, which is more than any single mitigation in the register.

The simulated σ is also wider than the shortcut suggested: £2.09m for the ranged base alone at ρ = 0, against £1.95m from the (P − O) / 6 rule. That rule understates the spread of a PERT line by roughly 7%, which is fine for a sanity check and not fine for a funding paper.

The tornado is the output that changes behaviour. Mechanical contributes 1.361 of the base variance of 3.813 — **36%** of the ordinary uncertainty — so that is where mitigation money buys most, whatever the register says is scariest.

## Step 9 — Turn the number into a drawdown plan

Contingency with no owner is spent by month two and argued about in month nine.

Set the governance in one line: the team plans to the P50, the sponsor holds the gap to the P80, and release happens against defined trigger events rather than against optimism.

Then track drawdown as a curve against time. If contingency is falling faster than the work is progressing, the forecast is already wrong and the monthly report should say so first.

Contingency is capital held against a probability. It lands in the funding requirement, in [the cash profile the project draws against](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) and eventually in reported margin, which is why it belongs in a finance syllabus rather than a scheduling one. The calculation content behind the PCI AI Project Finance Leader (PFL-AI) and PCI Project Management Leader – AI (PML-AI) volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

## Frequently asked questions

**How many iterations does a Monte Carlo cost simulation need?**
Enough that the answer stops moving. The table above shows the P80 on this model stable to within £0.06m at 5,000 iterations and within £0.02m at 100,000. For a 15 to 40 line cost model, 10,000 is a reasonable landing point, and the seed re-run matters more than the raw count.

**Should contingency be held at P50 or P80?**
Hold both, for different purposes. The P50 is a working target the team can plan against. The P80 is a funding position, covering most of the realistic downside without pricing the extreme tail. What breaks governance is committing to the P50 externally while reporting against it internally as though it were funded.

**Why use a copula rather than correlated normals directly?**
Because the marginals are not normal. A copula lets you keep each line's PERT shape, including its skew, while imposing a dependence structure across lines. Sampling correlated normals and rescaling them throws away the skew, which is the property the whole exercise exists to model.

**Can this run in a spreadsheet?**
Yes, for a model of this size. A random draw per line, a few thousand recalculations and a sorted output produce a usable distribution. Spreadsheets get difficult at correlation between lines and at discrete risks with conditional impacts, which is the point where purpose-built tools earn their fee.

**Does the result guarantee the project lands inside it?**
No, and any claim that it does should be corrected. The output is conditional on the ranges, the correlation and the risks modelled, and it says nothing about what was left out. That is why the step 1 exclusions are published beside the number rather than buried in an appendix.

---

*First published on projectcontrolsinstitute.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the full method including the simulation code lives here.*

*Linking note — the links now in the body: "the cash profile the project draws against" points at projectcontrolsinstitute.org/project-cash-flow-forecasting from step 9, because saying contingency lands in the funding requirement raises where that money shows up in the cash curve. One cross-estate link only — the earlier note proposed three to the same host, which is the pattern that gets a group of sites discounted together, and the rest of this piece is method and code that needs no reference. Reciprocal: the cash flow forecasting guide could cite this simulation where it needs a P80 funding figure to profile.*
