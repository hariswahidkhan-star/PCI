"""PML-AI Domain 9 — Quality, assurance and continuous improvement: golden checks.

Every number printed as a result in `domain-09-quality-assurance-improvement.md` is recomputed here
from first principles. Master-thread values (`AURIGA_BAC`) are read from ctx, never re-typed.

The domain has five engines, and each is checked from its definition rather than from its printed
output:

    CoQ            = prevention + appraisal + internal failure + external failure, over five
                     candidate regimes; the interior minimum and the range of external-failure unit
                     cost over which it holds are both solved, not asserted.
    escape fraction = product over layers of (1 - d_i); the defect flow is walked layer by layer so
                     that the internal failure cost reconciles to the CoQ table's R2 row.
    duration        = first-pass content / (crew * (1 - r))   — the rework duration multiplier
    p_upper         = 1 - alpha ** (1/n)                      — clean-sample confidence bound
    p*              = c / u                                   — breakeven defective fraction, which
                     the algebra shows is independent of n; that independence is itself checked.
    RTY             = product of stage yields, with first-pass failures walked down the flow.

Powers and logarithms use Decimal.ln()/exp() so that no binary-float artefact reaches a printed
value. Where the manuscript prints a value at four decimal places of a repeating decimal (4.7619
weeks, 55.3074 %) the tolerance is left at the default and the computed value is quantized to the
printed precision, which is stated in the check name.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    BAC = ctx["AURIGA_BAC"]                     # USD 4,000,000
    COD = D(45000)                              # Auriga cost of delay per week (Domain 6)
    EWCOST = D("130.625") * 40                  # engineer-week, from Domain 7 blended rate
    UE = D(12000)                               # Auriga external-failure unit cost

    check("D9 Auriga engineer-week from D7 blended rate", EWCOST, 5225)

    # ---------------------------------------------------------------- 9.1.2 cost of quality
    # (name, prevention, appraisal, introduced, detected, internal unit cost)
    regimes = [
        ("R0", D(12000), D(48000), D(160), D("0.75"), D(1000)),
        ("R1", D(40000), D(60000), D(120), D("0.85"), D(1300)),
        ("R2", D(96000), D(64000), D(80), D("0.90"), D(1500)),
        ("R3", D(170000), D(80000), D(64), D("0.9375"), D(1650)),
        ("R4", D(285000), D(100000), D(50), D("0.96"), D(1750)),
    ]
    rows = []
    for name, prev, app, intro, det, iu in regimes:
        found = intro * det
        esc = intro - found
        itf = found * iu
        ext = esc * UE
        rows.append((name, prev, app, found, esc, itf, ext, prev + app + itf + ext))

    for name, esc, itf, ext, tot in (
        ("R0", 40, 120000, 480000, 660000),
        ("R1", 18, 132600, 216000, 448600),
        ("R2", 8, 108000, 96000, 364000),
        ("R3", 4, 99000, 48000, 397000),
        ("R4", 2, 84000, 24000, 493000),
    ):
        r = next(x for x in rows if x[0] == name)
        check(f"WE 9.1.2 {name} escapes", r[4], esc)
        check(f"WE 9.1.2 {name} internal failure", r[5], itf)
        check(f"WE 9.1.2 {name} external failure", r[6], ext)
        check(f"WE 9.1.2 {name} total CoQ", r[7], tot)

    R0, R1, R2, R3, R4 = rows
    check("WE 9.1.2 minimum is R2", min(x[7] for x in rows), 364000)
    check("WE 9.1.2 R2 CoQ as % of BAC (1 dp)", (R2[7] / BAC * 100).quantize(D("0.1")), D("9.1"))
    check("WE 9.1.2 R0 CoQ as % of BAC (1 dp)", (R0[7] / BAC * 100).quantize(D("0.1")), D("16.5"))
    check("WE 9.1.2 R1 CoQ as % of BAC (1 dp)", (R1[7] / BAC * 100).quantize(D("0.1")), D("11.2"))
    check("WE 9.1.2 R3 CoQ as % of BAC (1 dp)", (R3[7] / BAC * 100).quantize(D("0.1")), D("9.9"))
    check("WE 9.1.2 R4 CoQ as % of BAC (1 dp)", (R4[7] / BAC * 100).quantize(D("0.1")), D("12.3"))

    # marginal step analysis
    def step(a, b):
        return (b[1] + b[2]) - (a[1] + a[2]), b[5] - a[5], b[6] - a[6], b[7] - a[7]

    dC, dI, dE, dT = step(R0, R1)
    check("WE 9.1.2 R0->R1 total CoQ change", dT, -211400)
    check("WE 9.1.2 R1 internal failure rises above R0", R1[5] - R0[5], 12600)
    dC, dI, dE, dT = step(R1, R2)
    check("WE 9.1.2 R1->R2 conformance increment", dC, 60000)
    check("WE 9.1.2 R1->R2 internal failure saved", -dI, 24600)
    check("WE 9.1.2 R1->R2 external failure saved", -dE, 120000)
    check("WE 9.1.2 R1->R2 net gain", -dT, 84600)
    dC, dI, dE, dT = step(R2, R3)
    check("WE 9.1.2 R2->R3 conformance increment", dC, 90000)
    check("WE 9.1.2 R2->R3 internal failure saved", -dI, 9000)
    check("WE 9.1.2 R2->R3 external failure saved", -dE, 48000)
    check("WE 9.1.2 R2->R3 net loss", dT, 33000)

    # range of optimality on the external-failure unit cost u:
    # a step a->b pays iff  dConf <= -dInternal + (esc_a - esc_b) * u
    def flip(a, b):
        dc = (b[1] + b[2]) - (a[1] + a[2])
        di = b[5] - a[5]
        return (dc + di) / (a[4] - b[4])

    check("WE 9.1.2 lower bound of R2 optimality (R1->R2 flip)", flip(R1, R2), 3540)
    check("WE 9.1.2 upper bound of R2 optimality (R2->R3 flip)", flip(R2, R3), 20250)
    check("WE 9.1.2 R3->R4 flip", flip(R3, R4), 60000)
    check("WE 9.1.2 range-of-optimality ratio (2 dp)",
          (flip(R2, R3) / flip(R1, R2)).quantize(D("0.01")), D("5.72"))
    check("WE 9.1.2 assumed u as multiple of lower bound (2 dp)",
          (UE / flip(R1, R2)).quantize(D("0.01")), D("3.39"))
    check("WE 9.1.2 assumed u as % of upper bound (1 dp)",
          (UE / flip(R2, R3) * 100).quantize(D("0.1")), D("59.3"))

    # cost of chasing escapes from 8 to 2
    check("WE 9.1.2 R4 minus R2 total CoQ", R4[7] - R2[7], 129000)
    per = (R4[7] - R2[7]) / (R2[4] - R4[4])
    check("WE 9.1.2 cost per escape avoided R2->R4", per, 21500)
    check("WE 9.1.2 ratio to the 12,000 harm prevented (2 dp)",
          (per / UE).quantize(D("0.01")), D("1.79"))

    # conformance / nonconformance split of the chosen regime
    check("9.1.2 R2 conformance", R2[1] + R2[2], 160000)
    check("9.1.2 R2 nonconformance", R2[5] + R2[6], 204000)

    # ---------------------------------------------------------------- 9.2.2 containment chain
    c1, c2, c3 = D(800), D(2000), D(3500)
    d1, d2, d3 = D("0.50"), D("0.60"), D("0.50")

    def chain(intro, det=(d1, d2, d3), cost=(c1, c2, c3)):
        rem, found, itf = intro, [], D(0)
        for di, ci in zip(det, cost):
            fi = rem * di
            found.append(fi)
            itf += fi * ci
            rem -= fi
        return found, rem, itf

    check("WE 9.2.2 escape fraction (3 dp)",
          ((1 - d1) * (1 - d2) * (1 - d3)).quantize(D("0.001")), D("0.100"))
    check("WE 9.2.2 detection rate % (1 dp)",
          ((1 - (1 - d1) * (1 - d2) * (1 - d3)) * 100).quantize(D("0.1")), D("90.0"))
    found, esc, itf = chain(D(80))
    check("WE 9.2.2 L1 finds", found[0], 40)
    check("WE 9.2.2 L2 finds", found[1], 24)
    check("WE 9.2.2 L3 finds", found[2], 8)
    check("WE 9.2.2 escapes", esc, 8)
    check("WE 9.2.2 internal failure cost", itf, 108000)
    check("WE 9.2.2 average internal cost per defect found", itf / sum(found), 1500)
    check("WE 9.2.2 external failure cost", esc * UE, 96000)
    NC = itf + esc * UE
    check("WE 9.2.2 total nonconformance", NC, 204000)
    CPI_DEFECT = NC / D(80)
    check("WE 9.2.2 expected containment cost per introduced defect", CPI_DEFECT, 2550)
    check("WE 9.2.2 reconciles to the R2 row of 9.1.2", itf + esc * UE, R2[5] + R2[6])
    # ladder ratios
    check("9.2.2 ladder ratio L2 (3 dp)", (c2 / c1).quantize(D("0.001")), D("2.500"))
    check("9.2.2 ladder ratio L3 (3 dp)", (c3 / c1).quantize(D("0.001")), D("4.375"))
    check("9.2.2 ladder ratio escape (3 dp)", (UE / c1).quantize(D("0.001")), D("15.000"))
    # removing L3
    check("9.2.2 escape fraction without L3 (3 dp)",
          ((1 - d1) * (1 - d2)).quantize(D("0.001")), D("0.200"))
    check("9.2.2 escapes without L3", D(80) * (1 - d1) * (1 - d2), 16)
    check("9.2.2 minimum running cost that would justify removing L3",
          D(8) * UE - D(8) * c3, 68000)

    # 9.2.2b prevention vs appraisal on the same 40,000
    SPEND = D(40000)
    fa, ea, ifa = chain(D(80), det=(d1, d2, D("0.75")))
    check("WE 9.2.2b (a) L3 finds", fa[2], 12)
    check("WE 9.2.2b (a) escapes", ea, 4)
    check("WE 9.2.2b (a) internal failure", ifa, 122000)
    check("WE 9.2.2b (a) external failure", ea * UE, 48000)
    check("WE 9.2.2b (a) spend + nonconformance", SPEND + ifa + ea * UE, 210000)
    check("WE 9.2.2b (a) worse than doing nothing by", (SPEND + ifa + ea * UE) - NC, 6000)
    fb, eb, ifb = chain(D(60))
    check("WE 9.2.2b (b) L1 finds", fb[0], 30)
    check("WE 9.2.2b (b) L2 finds", fb[1], 18)
    check("WE 9.2.2b (b) L3 finds", fb[2], 6)
    check("WE 9.2.2b (b) escapes", eb, 6)
    check("WE 9.2.2b (b) internal failure", ifb, 81000)
    check("WE 9.2.2b (b) external failure", eb * UE, 72000)
    check("WE 9.2.2b (b) spend + nonconformance", SPEND + ifb + eb * UE, 193000)
    check("WE 9.2.2b (b) better than doing nothing by", NC - (SPEND + ifb + eb * UE), 11000)
    check("WE 9.2.2b prevention beats appraisal by",
          (ifa + ea * UE) - (ifb + eb * UE), 17000)
    check("WE 9.2.2b identity: 20 defects avoided x 2,550", D(20) * CPI_DEFECT, 51000)
    check("WE 9.2.2b identity net of the 40,000 spend", D(20) * CPI_DEFECT - SPEND, 11000)
    check("WE 9.2.2b breakeven defects to avoid (3 dp)",
          (SPEND / CPI_DEFECT).quantize(D("0.001")), D("15.686"))
    check("WE 9.2.2b full CoQ under (b)", R2[1] + R2[2] + SPEND + ifb + eb * UE, 353000)
    check("WE 9.2.2b full CoQ under (a)", R2[1] + R2[2] + SPEND + ifa + ea * UE, 370000)

    # 9.2.3 case-study option: halving L3 coverage
    fh, eh, ifh = chain(D(80), det=(d1, d2, D("0.25")))
    check("9.2.3 escapes with L3 detection at 0.25", eh, 12)
    check("9.2.3 added external failure", (eh - esc) * UE, 48000)
    check("9.2.3 internal correction saved", D(4) * c3, 14000)
    check("9.2.3 net loss of halving L3 coverage", (eh - esc) * UE - D(4) * c3, 34000)

    # ---------------------------------------------------------------- 9.2.3 rework and capacity
    CREW, NOM, WORK = D(6), D(24), D(20)
    ALLOW = NOM - WORK
    check("WE 9.2.3 nominal capacity (engineer-weeks)", CREW * 4, 24)
    check("WE 9.2.3 rework allowance (engineer-weeks)", ALLOW, 4)
    check("WE 9.2.3 rework allowance % of capacity (2 dp)",
          (ALLOW / NOM * 100).quantize(D("0.01")), D("16.67"))
    check("WE 9.2.3 allowance from 1 - content/capacity (4 dp)",
          (1 - WORK / NOM).quantize(D("0.0001")), D("0.1667"))

    def dur(r):
        return WORK / (CREW * (1 - r))

    for r, mult, duration, over, cost in (
        (D(1) / D(6), "1.2000", "4.0000", "0.0000", 0),
        (D("0.25"), "1.3333", "4.4444", "0.4444", 20000),
        (D("0.30"), "1.4286", "4.7619", "0.7619", D("34285.71")),
        (D("0.40"), "1.6667", "5.5556", "1.5556", 70000),
        (D("0.50"), "2.0000", "6.6667", "2.6667", 120000),
    ):
        tag = f"{(r * 100).quantize(D('0.01'))}%"
        check(f"WE 9.2.3 multiplier at r={tag} (4 dp)",
              (1 / (1 - r)).quantize(D("0.0001")), D(mult))
        check(f"WE 9.2.3 duration at r={tag} (4 dp)", dur(r).quantize(D("0.0001")), D(duration))
        check(f"WE 9.2.3 overrun at r={tag} (4 dp)",
              (dur(r) - 4).quantize(D("0.0001")), D(over))
        check(f"WE 9.2.3 delay cost at r={tag}", (dur(r) - 4) * COD, D(cost))

    r30 = D("0.30")
    d30 = dur(r30)
    ew30 = CREW * d30
    rw30 = r30 * ew30
    check("WE 9.2.3 total engineer-weeks at 30 % (4 dp)", ew30.quantize(D("0.0001")), D("28.5714"))
    check("WE 9.2.3 rework engineer-weeks at 30 % (4 dp)", rw30.quantize(D("0.0001")), D("8.5714"))
    check("WE 9.2.3 engineer-weeks above allowance (4 dp)",
          (rw30 - ALLOW).quantize(D("0.0001")), D("4.5714"))
    check("WE 9.2.3 unbudgeted labour at 30 %", (rw30 - ALLOW) * EWCOST, D("23885.71"))
    check("WE 9.2.3 total consequence at 30 %",
          (rw30 - ALLOW) * EWCOST + (d30 - 4) * COD, D("58171.43"))
    check("WE 9.2.3 delay share of the consequence % (0 dp)",
          ((d30 - 4) * COD / ((rw30 - ALLOW) * EWCOST + (d30 - 4) * COD) * 100).quantize(D("1")),
          59)
    check("WE 9.2.3 marginal cost 30 %->40 %", (dur(D("0.40")) - d30) * COD, D("35714.29"))
    check("WE 9.2.3 marginal cost 40 %->50 %",
          (dur(D("0.50")) - dur(D("0.40"))) * COD, 50000)
    check("WE 9.2.3 marginal cost 10 %->20 %",
          (dur(D("0.20")) - dur(D("0.10"))) * COD, D("20833.33"))
    check("MCQ 9.2-E weeks added 10 %->20 % (4 dp)",
          (dur(D("0.20")) - dur(D("0.10"))).quantize(D("0.0001")), D("0.4630"))
    check("MCQ 9.2-E weeks added 40 %->50 % (4 dp)",
          (dur(D("0.50")) - dur(D("0.40"))).quantize(D("0.0001")), D("1.1111"))

    # ---------------------------------------------------------------- 9.3.2 acceptance sampling
    ALPHA = D("0.05")

    def bound(n):
        return 1 - (ALPHA.ln() / D(n)).exp()

    def npass(p, n):
        return (1 - p) ** n

    N120, C, U = D(120), D(1400), UE
    check("WE 9.3.2 P(clean 20 | p=2 %) % (2 dp)",
          (npass(D("0.02"), 20) * 100).quantize(D("0.01")), D("66.76"))
    check("WE 9.3.2 P(clean 20 | p=5 %) % (2 dp)",
          (npass(D("0.05"), 20) * 100).quantize(D("0.01")), D("35.85"))
    check("WE 9.3.2 P(clean 20 | p=10 %) % (2 dp)",
          (npass(D("0.10"), 20) * 100).quantize(D("0.01")), D("12.16"))
    check("WE 9.3.2 95 % bound at n=20 % (2 dp)",
          (bound(20) * 100).quantize(D("0.01")), D("13.91"))
    check("WE 9.3.2 bound applied to 120 loops (2 dp)",
          (bound(20) * N120).quantize(D("0.01")), D("16.69"))
    PSTAR = C / U
    check("WE 9.3.2 breakeven defective fraction % (2 dp)",
          (PSTAR * 100).quantize(D("0.01")), D("11.67"))
    check("WE 9.3.2 bound at n=24 % (2 dp)", (bound(24) * 100).quantize(D("0.01")), D("11.73"))
    check("WE 9.3.2 bound at n=25 % (2 dp)", (bound(25) * 100).quantize(D("0.01")), D("11.29"))
    nstar = ALPHA.ln() / (1 - PSTAR).ln()
    check("WE 9.3.2 minimum self-consistent n (3 dp)", nstar.quantize(D("0.001")), D("24.149"))
    check("WE 9.3.2 cost of the extra 5 loops", D(5) * C, 7000)
    n59 = ALPHA.ln() / D("0.95").ln()
    check("WE 9.3.2 n to bound p at 5 % (3 dp)", n59.quantize(D("0.001")), D("58.404"))
    check("WE 9.3.2 rule of three at p=5 %", D(3) / D("0.05"), 60)

    def plan_cost(n, p):
        return C * n + U * p * (N120 - n)

    check("WE 9.3.2 plan cost n=20 at p=5 %", plan_cost(D(20), D("0.05")), 88000)
    check("WE 9.3.2 plan cost n=59 at p=5 %", plan_cost(D(59), D("0.05")), 119200)
    check("WE 9.3.2 plan cost full verification", plan_cost(N120, D("0.05")), 168000)
    # the breakeven is independent of n: two different n give the same crossing point
    for n in (D(20), D(25), D(59)):
        pcross = (C * N120 - C * n) / (U * (N120 - n))
        check(f"WE 9.3.2 breakeven independent of n (n={n})", pcross, PSTAR)

    # ---------------------------------------------------------------- 9.3.3 root cause
    check("WE 9.3.3 cause concentration % (1 dp)",
          (D(5) / D(8) * 100).quantize(D("0.1")), D("62.5"))
    check("WE 9.3.3 expected avoided cost", D(5) * UE, 60000)
    check("WE 9.3.3 net value of removal", D(5) * UE - D(18000), 42000)
    check("WE 9.3.3 payback ratio (2 dp)",
          (D(5) * UE / D(18000)).quantize(D("0.01")), D("3.33"))
    check("WE 9.3.3 breakeven recurrences", D(18000) / UE, D("1.5"))

    # ---------------------------------------------------------------- 9.4.2 rolled throughput yield
    ys = [D("0.95"), D("0.92"), D("0.90"), D("0.94"), D("0.88"), D("0.85")]
    rty = D(1)
    for y in ys:
        rty *= y
    check("WE 9.4.2 RTY % (4 dp)", (rty * 100).quantize(D("0.0001")), D("55.3074"))
    check("WE 9.4.2 arithmetic mean of the six yields % (4 dp)",
          (sum(ys) / 6 * 100).quantize(D("0.0001")), D("90.6667"))

    def loops(yields, items=D(60)):
        flow, tot, per = items, D(0), []
        for y in yields:
            fail = flow * (1 - y)
            per.append(fail)
            tot += fail
            flow *= y
        return per, tot, flow

    per, tot, clean = loops(ys)
    for i, want in enumerate(["3.0000", "4.5600", "5.2440", "2.8318", "5.3237", "5.8561"]):
        check(f"WE 9.4.2 first-pass failures at step {i+1} (4 dp)",
              per[i].quantize(D("0.0001")), D(want))
    check("WE 9.4.2 packages clean through the chain (2 dp)",
          clean.quantize(D("0.01")), D("33.18"))
    check("WE 9.4.2 packages needing rework (2 dp)", tot.quantize(D("0.01")), D("26.82"))
    check("WE 9.4.2 rework cost at 2,600 per loop", tot * D(2600), D("69720.43"))
    ys_weak = ys[:5] + [D("0.95")]
    rty_w = D(1)
    for y in ys_weak:
        rty_w *= y
    per_w, tot_w, _ = loops(ys_weak)
    check("WE 9.4.2 RTY with weakest step at 0.95 % (4 dp)",
          (rty_w * 100).quantize(D("0.0001")), D("61.8142"))
    check("WE 9.4.2 gain from fixing the weakest step, points (3 dp)",
          ((rty_w - rty) * 100).quantize(D("0.001")), D("6.507"))
    check("WE 9.4.2 rework loops after fixing the weakest step (2 dp)",
          tot_w.quantize(D("0.01")), D("22.91"))
    check("WE 9.4.2 saving from fixing the weakest step", (tot - tot_w) * D(2600), D("10150.54"))
    ys_strong = [D("0.98")] + ys[1:]
    rty_s = D(1)
    for y in ys_strong:
        rty_s *= y
    check("WE 9.4.2 RTY with strongest step at 0.98 % (4 dp)",
          (rty_s * 100).quantize(D("0.0001")), D("57.0540"))
    check("WE 9.4.2 gain from fixing the strongest step, points (3 dp)",
          ((rty_s - rty) * 100).quantize(D("0.001")), D("1.747"))
    check("WE 9.4.2 ratio of the two gains (2 dp)",
          ((rty_w - rty) / (rty_s - rty)).quantize(D("0.01")), D("3.73"))
    check("WE 9.4.2 uniform yield for RTY 80 % over 6 steps % (2 dp)",
          ((D("0.80").ln() / 6).exp() * 100).quantize(D("0.01")), D("96.35"))
    check("WE 9.4.2 uniform yield for RTY 90 % over 6 steps % (2 dp)",
          ((D("0.90").ln() / 6).exp() * 100).quantize(D("0.01")), D("98.26"))
    # figure 9.4.1 cumulative series
    cum = D(100)
    for y, want in zip(ys, ["95.0000", "87.4000", "78.6600", "73.9404", "65.0676", "55.3074"]):
        cum *= y
        check(f"Fig 9.4.1 cumulative yield after {want[:5]} step (4 dp)",
              cum.quantize(D("0.0001")), D(want))

    # ---------------------------------------------------------------- 9.4.3 data quality
    dims = [D("0.960"), D("0.930"), D("0.980"), D("0.910"), D("0.990"), D("0.995")]
    comp = D(1)
    for v in dims:
        comp *= v
    RECS = D(3200)
    check("WE 9.4.3 composite fitness % (2 dp)", (comp * 100).quantize(D("0.01")), D("78.43"))
    check("WE 9.4.3 arithmetic mean of the six dimensions % (2 dp)",
          (sum(dims) / 6 * 100).quantize(D("0.01")), D("96.08"))
    check("WE 9.4.3 conforming records (0 dp)", (comp * RECS).quantize(D("1")), 2510)
    check("WE 9.4.3 non-conforming records (0 dp)", (RECS - comp * RECS).quantize(D("1")), 690)
    comp2 = comp / D("0.910") * D("0.980")
    check("WE 9.4.3 composite after consistency 0.91->0.98 % (2 dp)",
          (comp2 * 100).quantize(D("0.01")), D("84.46"))
    check("WE 9.4.3 gain in points (2 dp)", ((comp2 - comp) * 100).quantize(D("0.01")), D("6.03"))
    check("WE 9.4.3 extra conforming records (0 dp)",
          ((comp2 - comp) * RECS).quantize(D("1")), 193)

    # ---------------------------------------------------------------- 9.4.4 AI-output verification
    NA, CA, UA = D(240), D(40), D(1800)
    check("WE 9.4.4 95 % bound at n=20 % (2 dp)", (bound(20) * 100).quantize(D("0.01")), D("13.91"))
    check("WE 9.4.4 bound applied to 240 items (2 dp)",
          (bound(20) * NA).quantize(D("0.01")), D("33.39"))
    check("WE 9.4.4 breakeven error fraction % (2 dp)",
          (CA / UA * 100).quantize(D("0.01")), D("2.22"))
    n2 = ALPHA.ln() / D("0.98").ln()
    check("WE 9.4.4 n to bound the error rate at 2 % (3 dp)",
          n2.quantize(D("0.001")), D("148.284"))
    check("WE 9.4.4 149 items as % of the population (1 dp)",
          (D(149) / NA * 100).quantize(D("0.1")), D("62.1"))
    check("WE 9.4.4 review cost of 149 items", D(149) * CA, 5960)
    check("WE 9.4.4 full review cost", NA * CA, 9600)
    check("WE 9.4.4 exposure on the unreviewed remainder at the bound",
          bound(20) * (NA - 20) * UA, D("55086.90"))

    # ---------------------------------------------------------------- Case study A
    consumed = CREW * 2
    rem_fp = WORK - (consumed - r30 * consumed)
    check("Case A remaining first-pass content at week 15 (1 dp)",
          rem_fp.quantize(D("0.1")), D("11.6"))
    rem6 = rem_fp / (CREW * (1 - r30))
    rem8 = rem_fp / (D(8) * (1 - r30))
    check("Case A remaining duration at crew 6 (4 dp)", rem6.quantize(D("0.0001")), D("2.7619"))
    check("Case A remaining duration at crew 8 (4 dp)", rem8.quantize(D("0.0001")), D("2.0714"))
    check("Case A total phase duration at crew 8 (4 dp)",
          (2 + rem8).quantize(D("0.0001")), D("4.0714"))
    over8 = 2 + rem8 - 4
    check("Case A overrun at crew 8 (4 dp)", over8.quantize(D("0.0001")), D("0.0714"))
    check("Case A delay cost at crew 8", over8 * COD, D("3214.29"))
    check("Case A delay saved by buying capacity", (d30 - 4) * COD - over8 * COD, D("31071.43"))
    extra = D(2) * rem8
    check("Case A extra engineer-weeks purchased (4 dp)", extra.quantize(D("0.0001")), D("4.1429"))
    check("Case A extra capacity at the blended rate", extra * EWCOST, D("21646.43"))
    check("Case A off-shift premium at 25 %", extra * EWCOST * D("0.25"), D("5411.61"))
    check("Case A total cost of the extra capacity", extra * EWCOST * D("1.25"), D("27058.04"))
    check("Case A net value of buying capacity",
          (d30 - 4) * COD - over8 * COD - extra * EWCOST * D("1.25"), D("4013.39"))
    # prevention counterfactual: 2 fewer SAT defects at 0.75 engineer-weeks of rework each
    rw_prev = rw30 - D(2) * D("0.75")
    dur_prev = (WORK + rw_prev) / CREW
    check("Case A rework engineer-weeks under the prevention counterfactual (4 dp)",
          rw_prev.quantize(D("0.0001")), D("7.0714"))
    check("Case A duration under the prevention counterfactual (4 dp)",
          dur_prev.quantize(D("0.0001")), D("4.5119"))
    check("Case A delay saved by prevention", (d30 - 4) * COD - (dur_prev - 4) * COD, 11250)
    check("Case A total value of the 40,000 prevention option",
          D(11000) + (d30 - 4) * COD - (dur_prev - 4) * COD, 22250)
    check("Case A SAT-attributable rework engineer-weeks", D(8) * D("0.75"), 6)

    # ---------------------------------------------------------------- Case study B
    NB, CB, UB = D(480), D(900), D(21000)
    pb = CB / UB
    check("Case B breakeven defective fraction % (4 dp)",
          (pb * 100).quantize(D("0.0001")), D("4.2857"))
    check("Case B 95 % bound at n=30 % (2 dp)", (bound(30) * 100).quantize(D("0.01")), D("9.50"))
    check("Case B bound exceeds breakeven by points (2 dp)",
          ((bound(30) - pb) * 100).quantize(D("0.01")), D("5.22"))
    nb = ALPHA.ln() / (1 - pb).ln()
    check("Case B self-consistent n (3 dp)", nb.quantize(D("0.001")), D("68.392"))
    check("Case B bound at n=68 % (3 dp)", (bound(68) * 100).quantize(D("0.001")), D("4.310"))
    check("Case B bound at n=69 % (3 dp)", (bound(69) * 100).quantize(D("0.001")), D("4.249"))
    check("Case B sampling cost n=30", D(30) * CB, 27000)
    check("Case B sampling cost n=69", D(69) * CB, 62100)
    check("Case B extra cost of self-consistency", (D(69) - D(30)) * CB, 35100)
    check("Case B full verification cost", NB * CB, 432000)
    ptrue = D(21) / D(450)
    check("Case B observed defective fraction % (3 dp)",
          (ptrue * 100).quantize(D("0.001")), D("4.667"))
    p30 = npass(ptrue, 30)
    p69 = npass(ptrue, 69)
    check("Case B P(clean 30 at that rate) % (2 dp)", (p30 * 100).quantize(D("0.01")), D("23.84"))
    check("Case B P(clean 69 at that rate) % (2 dp)", (p69 * 100).quantize(D("0.01")), D("3.70"))
    check("Case B realised escape cost", D(21) * UB, 441000)
    check("Case B expected saving from n=69", (p30 - p69) * D(21) * UB, D("88838.10"))
    check("Case B net of the extra sampling",
          (p30 - p69) * D(21) * UB - (D(69) - D(30)) * CB, D("53738.10"))
    check("Case B realised escape cost less full verification", D(21) * UB - NB * CB, 9000)

    # ---------------------------------------------------------------- Exercise 9.1
    BUD = D(6000000)
    UQ = D(9000)
    qs = [("Q1", D(15000), D(45000), D(150), D("0.80"), D(1000)),
          ("Q2", D(55000), D(65000), D(110), D("0.90"), D(1200)),
          ("Q3", D(120000), D(85000), D(80), D("0.95"), D(1400)),
          ("Q4", D(220000), D(110000), D(50), D("0.96"), D(1500))]
    qrows = []
    for name, prev, app, intro, det, iu in qs:
        fnd = intro * det
        e = intro - fnd
        i_ = fnd * iu
        x = e * UQ
        qrows.append((name, prev, app, fnd, e, i_, x, prev + app + i_ + x))
    for name, e, i_, x, tot, pct in (
        ("Q1", 30, 120000, 270000, 450000, "7.50"),
        ("Q2", 11, 118800, 99000, 337800, "5.63"),
        ("Q3", 4, 106400, 36000, 347400, "5.79"),
        ("Q4", 2, 72000, 18000, 420000, "7.00"),
    ):
        row = next(z for z in qrows if z[0] == name)
        check(f"Ex 9.1 {name} escapes", row[4], e)
        check(f"Ex 9.1 {name} internal failure", row[5], i_)
        check(f"Ex 9.1 {name} external failure", row[6], x)
        check(f"Ex 9.1 {name} total CoQ", row[7], tot)
        check(f"Ex 9.1 {name} % of budget (2 dp)",
              (row[7] / BUD * 100).quantize(D("0.01")), D(pct))
    check("Ex 9.1 minimum is Q2", min(z[7] for z in qrows), 337800)

    def qflip(a, b):
        dc = (b[1] + b[2]) - (a[1] + a[2])
        di = b[5] - a[5]
        return (dc + di) / (a[4] - b[4])

    check("Ex 9.1 Q2->Q3 conformance increment",
          (qrows[2][1] + qrows[2][2]) - (qrows[1][1] + qrows[1][2]), 85000)
    check("Ex 9.1 Q2->Q3 internal failure saved", qrows[1][5] - qrows[2][5], 12400)
    check("Ex 9.1 Q2->Q3 flip point (2 dp)",
          qflip(qrows[1], qrows[2]).quantize(D("0.01")), D("10371.43"))
    check("Ex 9.1 Q1->Q2 flip point (2 dp)",
          qflip(qrows[0], qrows[1]).quantize(D("0.01")), D("3094.74"))
    check("Ex 9.1 Q3->Q4 flip point", qflip(qrows[2], qrows[3]), 45300)
    check("Ex 9.1 headroom below the flip point % (2 dp)",
          ((qflip(qrows[1], qrows[2]) - UQ) / qflip(qrows[1], qrows[2]) * 100).quantize(D("0.01")),
          D("13.22"))

    # ---------------------------------------------------------------- Exercise 9.2
    de = (D("0.40"), D("0.50"), D("0.60"))
    ce = (D(700), D(2200), D(5000))
    UEX = D(16000)
    rem, itf2, fnd2 = D(150), D(0), []
    for di, ci in zip(de, ce):
        fi = rem * di
        fnd2.append(fi)
        itf2 += fi * ci
        rem -= fi
    check("Ex 9.2 escape fraction (3 dp)",
          ((1 - de[0]) * (1 - de[1]) * (1 - de[2])).quantize(D("0.001")), D("0.120"))
    check("Ex 9.2 detection % (1 dp)",
          ((1 - (1 - de[0]) * (1 - de[1]) * (1 - de[2])) * 100).quantize(D("0.1")), D("88.0"))
    check("Ex 9.2 L1 finds", fnd2[0], 60)
    check("Ex 9.2 L2 finds", fnd2[1], 45)
    check("Ex 9.2 L3 finds", fnd2[2], 27)
    check("Ex 9.2 escapes", rem, 18)
    check("Ex 9.2 internal failure", itf2, 276000)
    check("Ex 9.2 external failure", rem * UEX, 288000)
    check("Ex 9.2 total nonconformance", itf2 + rem * UEX, 564000)
    per2 = (itf2 + rem * UEX) / D(150)
    check("Ex 9.2 expected containment cost per introduced defect", per2, 3760)
    check("Ex 9.2 prevention return on 30 defects avoided", D(30) * per2, 112800)
    check("Ex 9.2 net value of the prevention programme", D(30) * per2 - D(95000), 17800)
    check("Ex 9.2 breakeven defects avoided (3 dp)",
          (D(95000) / per2).quantize(D("0.001")), D("25.266"))

    # ---------------------------------------------------------------- Exercise 9.3
    crew3, nom3, work3 = D(8), D(48), D(40)
    cod3, ew3 = D(30000), D(5000)
    check("Ex 9.3 allowance (engineer-weeks)", nom3 - work3, 8)
    check("Ex 9.3 allowance % of capacity (2 dp)",
          ((nom3 - work3) / nom3 * 100).quantize(D("0.01")), D("16.67"))
    r35 = D("0.35")
    d35 = work3 / (crew3 * (1 - r35))
    check("Ex 9.3 duration at r=35 % (4 dp)", d35.quantize(D("0.0001")), D("7.6923"))
    check("Ex 9.3 overrun at r=35 % (4 dp)", (d35 - 6).quantize(D("0.0001")), D("1.6923"))
    check("Ex 9.3 delay cost at r=35 %", (d35 - 6) * cod3, D("50769.23"))
    rw35 = r35 * crew3 * d35
    check("Ex 9.3 rework engineer-weeks (4 dp)", rw35.quantize(D("0.0001")), D("21.5385"))
    check("Ex 9.3 engineer-weeks above allowance (4 dp)",
          (rw35 - (nom3 - work3)).quantize(D("0.0001")), D("13.5385"))
    check("Ex 9.3 unbudgeted labour", (rw35 - (nom3 - work3)) * ew3, D("67692.31"))
    check("Ex 9.3 total consequence",
          (rw35 - (nom3 - work3)) * ew3 + (d35 - 6) * cod3, D("118461.54"))
    check("Ex 9.3 rework share that doubles the phase (4 dp)",
          (1 - work3 / (crew3 * 12)).quantize(D("0.0001")), D("0.5833"))

    # ---------------------------------------------------------------- Exercise 9.4
    N4, C4, U4 = D(300), D(260), D(6500)
    check("Ex 9.4 P(clean 30 | p=4 %) % (2 dp)",
          (npass(D("0.04"), 30) * 100).quantize(D("0.01")), D("29.39"))
    check("Ex 9.4 95 % bound at n=30 % (2 dp)", (bound(30) * 100).quantize(D("0.01")), D("9.50"))
    check("Ex 9.4 bound applied to 300 welds (2 dp)",
          (bound(30) * N4).quantize(D("0.01")), D("28.51"))
    p4 = C4 / U4
    check("Ex 9.4 breakeven defective fraction % (2 dp)",
          (p4 * 100).quantize(D("0.01")), D("4.00"))
    n4 = ALPHA.ln() / (1 - p4).ln()
    check("Ex 9.4 minimum self-consistent n (3 dp)", n4.quantize(D("0.001")), D("73.385"))
    check("Ex 9.4 bound at n=74 % (3 dp)", (bound(74) * 100).quantize(D("0.001")), D("3.967"))
    check("Ex 9.4 bound at n=73 % (3 dp)", (bound(73) * 100).quantize(D("0.001")), D("4.021"))
    check("Ex 9.4 cost of the 74-weld plan", D(74) * C4, 19240)
    check("Ex 9.4 cost of the 30-weld plan", D(30) * C4, 7800)
    check("Ex 9.4 cost of full verification", N4 * C4, 78000)
    check("Ex 9.4 self-consistent plan as % of full verification (1 dp)",
          (D(74) * C4 / (N4 * C4) * 100).quantize(D("0.1")), D("24.7"))

    # ---------------------------------------------------------------- Exercise 9.5
    y5 = [D("0.98"), D("0.94"), D("0.91"), D("0.96"), D("0.89")]
    rty5 = D(1)
    for y in y5:
        rty5 *= y
    check("Ex 9.5 RTY % (4 dp)", (rty5 * 100).quantize(D("0.0001")), D("71.6237"))
    check("Ex 9.5 arithmetic mean of the five yields % (2 dp)",
          (sum(y5) / 5 * 100).quantize(D("0.01")), D("93.60"))
    per5, tot5, clean5 = loops(y5, items=D(200))
    for i, want in enumerate(["4.0000", "11.7600", "16.5816", "6.7063", "17.7047"]):
        check(f"Ex 9.5 first-pass failures at step {i+1} (4 dp)",
              per5[i].quantize(D("0.0001")), D(want))
    check("Ex 9.5 total first-pass failures (4 dp)", tot5.quantize(D("0.0001")), D("56.7527"))
    check("Ex 9.5 items clean through the chain (4 dp)",
          clean5.quantize(D("0.0001")), D("143.2473"))
    check("Ex 9.5 rework cost at 1,900 per failure", tot5 * D(1900), D("107830.06"))
    y5w = y5[:4] + [D("0.96")]
    r5w = D(1)
    for y in y5w:
        r5w *= y
    per5w, tot5w, _ = loops(y5w, items=D(200))
    check("Ex 9.5 RTY with weakest step at 0.96 % (4 dp)",
          (r5w * 100).quantize(D("0.0001")), D("77.2570"))
    check("Ex 9.5 gain from fixing the weakest step, points (3 dp)",
          ((r5w - rty5) * 100).quantize(D("0.001")), D("5.633"))
    check("Ex 9.5 failures after fixing the weakest step (4 dp)",
          tot5w.quantize(D("0.0001")), D("45.4860"))
    check("Ex 9.5 saving from fixing the weakest step (0 dp)",
          ((tot5 - tot5w) * D(1900)).quantize(D("1")), 21407)
    y5s = [D("0.99")] + y5[1:]
    r5s = D(1)
    for y in y5s:
        r5s *= y
    check("Ex 9.5 RTY with strongest step at 0.99 % (4 dp)",
          (r5s * 100).quantize(D("0.0001")), D("72.3545"))
    check("Ex 9.5 gain from fixing the strongest step, points (3 dp)",
          ((r5s - rty5) * 100).quantize(D("0.001")), D("0.731"))
    check("Ex 9.5 ratio of the two gains (2 dp)",
          ((r5w - rty5) / (r5s - rty5)).quantize(D("0.01")), D("7.71"))
    check("Ex 9.5 uniform yield for RTY 90 % over 5 steps % (2 dp)",
          ((D("0.90").ln() / 5).exp() * 100).quantize(D("0.01")), D("97.91"))
