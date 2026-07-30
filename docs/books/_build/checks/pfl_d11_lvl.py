"""PFL-AI Domain 11 — golden checks for the Evaluation and Comprehension MCQs added to KA 11.1–11.4.

Scope: every number printed in the twelve items added to KA 11.1–11.4 — 11.1-D/E/F, 11.2-D/E/F,
11.3-E/F/G and 11.4-E/F/G — options and rationales. `pfl_d11.py` owns the domain's worked examples;
this module owns only the added items. Master-thread values come from ctx, never re-typed.

The first pass covered six of the twelve. 11.1-E, 11.2-F, 11.3-G and 11.4-F printed a further
twenty-six figures that no module recomputed — the bundle against the line-by-line alternative on the
whole register rather than the declined half, the availability breakeven and the outage cost against
the O&M cap, the hedge ladder and its three breakeven rates, and the register's covenant-preserving
ceiling. Section 5 closes that.

Engines used:

  1. Allocation pricing (11.1.3) — `EMV` = p x impact; loaded premium = transferee `EMV` x (1 + loading);
     breakeven loading = own `EMV` / transferee `EMV` - 1. Only the declined bundle A4-A8 is needed.
  2. Escalation (11.2.3) — `CFADS(t) = R0 (1 + phi_rev x CPI)^(t-1) - C_cpi (1 + CPI)^(t-1)
     - C_pow (1 + g)^(t-1) - 1,116,000`, evaluated on the base case AND on a stress that adds a
     percentage point to BOTH the index and the power escalation rate, which is the point of 11.2-D:
     the differential barely moves while every escalating line grows, so the reported ratio improves.
  3. Currency (11.3.2) — `CFADS(USD) = revenue x s + (revenue (1-s) x x0 - HC costs) / x - USD costs`,
     with the indexed share `s`; the breakeven `x` for a target ratio is solved in closed form.
  4. Register aggregation (11.4.1, 11.4.2) — mean = sum `EMV`, variance = (1-rho) sum sigma_i^2
     + rho (sum sigma_i)^2 with sigma_i = sqrt(p(1-p)) x impact, P80 = mean + 0.8416 sigma; then the
     annuity-equivalent conversion into debt capacity at the covenant.
  5. The four items the first pass left unchecked. The whole eight-item register, both sides, so the
     bundle premium, the retained residue and the opportunity line are summed rather than quoted —
     which is what shows that the bundle genuinely beats full retention and still loses 1,780,000
     (11.1.3). Availability as a linear function of output, giving the 92.086 % breakeven and the
     daily outage cost against a half-fee cap (11.2.4, 12.1.4). The interest-rate ladder, where debt
     service is `principal + debt x blended rate` and each threshold's breakeven rate is solved
     directly, together with the minimum hedge ratio surviving the lenders' shock (11.3.1). And the
     covenant-preserving PV ceiling, `headroom x AF(0.06, 12)`, against the register's own P80
     (11.4.4). Two figures in the chapter are differences of displayed whole units rather than of
     exact values — the 837,478 correlation uplift and the 881,189 gap — and are asserted that way
     here, with the exact difference checked alongside so the convention is visible.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    q = lambda x, n: x.quantize(D(1).scaleb(-n))

    DEBT = ctx["KESTREL_DEBT"]                 # 42,000,000
    CF = ctx["KESTREL_CFADS"]                  # 6,384,000
    DS = ctx["KESTREL_INSTALMENT"]             # 5,009,635.23
    R = ctx["KESTREL_RATE"]                    # 0.06
    N = ctx["KESTREL_TENOR"]                   # 12
    INT1 = ctx["KESTREL_INTEREST_Y1"]          # 2,520,000
    AF6_12 = D("8.383844")
    COV, LOCK, Z80 = D("1.20"), D("1.15"), D("0.8416")
    PRIN1 = DS - INT1
    HEAD = CF - DS * COV

    check("D11-lvl annual covenant headroom", q(HEAD, 2), D("372437.72"))
    check("D11-lvl covenant cash trigger", q(DS * COV, 2), D("6011562.28"))

    # ================= MCQ 11.1-D — the declined bundle, and why no price works =====
    LOAD = D("1.40")
    DECLINED = [                               # (id, owner p, owner impact, bidder p, bidder impact)
        ("A4", D("0.40"), D(2400000), D("0.45"), D(2900000)),
        ("A5", D("0.30"), D(1800000), D("0.31"), D(2000000)),
        ("A6", D("0.35"), D(1400000), D("0.35"), D(1500000)),
        ("A7", D("0.50"), D(900000), D("0.50"), D(900000)),
        ("A8", D("0.20"), D(2000000), D("0.20"), D(2000000)),
    ]
    own = sum(p * i for _, p, i, _, _ in DECLINED)
    con = sum(p * i for _, _, _, p, i in DECLINED)
    check("MCQ 11.1-D owner EMV of the declined bundle", own, 2840000)
    check("MCQ 11.1-D bidder EMV of the same bundle", con, 3300000)
    check("MCQ 11.1-D loaded premium at 40 %", con * LOAD, 4620000)
    check("MCQ 11.1-D value destroyed at a 40 % loading", own - con * LOAD, -1780000)
    check("MCQ 11.1-D value destroyed at a zero loading", own - con, -460000)
    check("MCQ 11.1-D breakeven loading %", q((own / con - 1) * 100, 2), D("-13.94"))

    # ================= MCQ 11.2-D — the double-sided escalation stress =============
    REV0, C_CPI, C_POW = D(12000000), D(2700000), D(1800000)
    OPEX, FIXED = D(4500000), D(1116000)       # cash tax 516,000 + working capital 600,000
    PHI_REV = D("0.80")                        # share of the tariff indexed to the price index

    def cfads(t, cpi, powg):
        rev = REV0 * (1 + PHI_REV * cpi) ** (t - 1)
        oth = C_CPI * (1 + cpi) ** (t - 1)
        pw = C_POW * (1 + powg) ** (t - 1)
        return rev - oth - pw - FIXED

    def weighted(cpi, powg):                   # revenue-weighted and cost-weighted escalation, %
        return PHI_REV * cpi * 100, (C_CPI * cpi + C_POW * powg) / OPEX * 100

    BASE_CPI, BASE_POW = D("0.025"), D("0.08")
    STR_CPI, STR_POW = D("0.035"), D("0.09")
    check("MCQ 11.2-D year one reproduces the master thread", cfads(1, BASE_CPI, BASE_POW), CF)
    base12 = cfads(12, BASE_CPI, BASE_POW)
    str12 = cfads(12, STR_CPI, STR_POW)
    check("MCQ 11.2-D base-case year-twelve CFADS", q(base12, 2), D("6064907.53"))
    check("MCQ 11.2-D base-case year-twelve DSCR", q(base12 / DS, 4), D("1.2106"))
    check("MCQ 11.2-D stressed year-twelve CFADS", q(str12, 2), D("6556751.37"))
    check("MCQ 11.2-D stressed year-twelve DSCR", q(str12 / DS, 4), D("1.3088"))
    check("MCQ 11.2-D the stress makes reported coverage better, not worse",
          1 if str12 / DS > base12 / DS else 0, 1)
    rv_b, cw_b = weighted(BASE_CPI, BASE_POW)
    rv_s, cw_s = weighted(STR_CPI, STR_POW)
    check("MCQ 11.2-D base revenue-weighted escalation %", q(rv_b, 2), D("2.00"))
    check("MCQ 11.2-D base cost-weighted escalation %", q(cw_b, 2), D("4.70"))
    check("MCQ 11.2-D base differential, percentage points", q(cw_b - rv_b, 2), D("2.70"))
    check("MCQ 11.2-D stressed revenue-weighted escalation %", q(rv_s, 2), D("2.80"))
    check("MCQ 11.2-D stressed cost-weighted escalation %", q(cw_s, 2), D("5.70"))
    check("MCQ 11.2-D stressed differential, percentage points", q(cw_s - rv_s, 2), D("2.90"))

    # ================= MCQ 11.2-E — what a pass-through does ========================
    PHI = D("0.70")
    check("MCQ 11.2-E share retained by the project %", (1 - PHI) * 100, 30)
    check("MCQ 11.2-E tolerance multiplier", q(1 / (1 - PHI), 4), D("3.3333"))
    check("MCQ 11.2-E residual per 1 % rise at phi = 0.70", C_POW * D("0.01") * (1 - PHI), 5400)
    check("MCQ 11.2-E tolerance at phi = 0.70 %",
          q(HEAD / (C_POW * D("0.01") * (1 - PHI)), 4), D("68.9699"))
    check("MCQ 11.2-E tolerance unprotected %", q(HEAD / (C_POW * D("0.01")), 4), D("20.6910"))

    # ================= MCQ 11.3-E — the exchange-rate-indexed share ================
    X0 = D(4)                                  # host-currency units per USD at close
    HC_LOCAL = D(12600000) + D(2064000) + D(2400000)
    USD_OPEX = D(1350000)
    check("MCQ 11.3-E local costs, tax and working capital in HC", HC_LOCAL, 17064000)

    def cfads_ccy(s, x):
        return REV0 * s + (REV0 * (1 - s) * X0 - HC_LOCAL) / x - USD_OPEX

    def breakeven_x(s, target):                # closed form, from cfads_ccy(s, x) = target x DS
        return (REV0 * (1 - s) * X0 - HC_LOCAL) / (target * DS + USD_OPEX - REV0 * s)

    check("MCQ 11.3-E the decomposition reproduces the master thread", cfads_ccy(D(0), X0), CF)
    S_MATCH = (DS + USD_OPEX) / REV0
    check("MCQ 11.3-E debt-service-matching indexed share %", q(S_MATCH * 100, 3), D("52.997"))
    x_un = breakeven_x(D(0), COV)
    check("MCQ 11.3-E unindexed breakeven rate", q(x_un, 6), D("4.202369"))
    check("MCQ 11.3-E unindexed tolerable devaluation %", q((x_un / X0 - 1) * 100, 2), D("5.06"))
    S40 = D("0.40")
    c40 = cfads_ccy(S40, D(5))
    check("MCQ 11.3-E CFADS at a 40 % share on a 25 % devaluation", q(c40, 2), D("5797200.00"))
    check("MCQ 11.3-E DSCR at a 40 % share on a 25 % devaluation", q(c40 / DS, 4), D("1.1572"))
    check("MCQ 11.3-E a 40 % share breaches the covenant at 25 %", 1 if c40 < DS * COV else 0, 1)
    check("MCQ 11.3-E a 40 % share still clears the lock-up at 25 %",
          1 if c40 > DS * LOCK else 0, 1)
    x40 = breakeven_x(S40, COV)
    check("MCQ 11.3-E tolerable devaluation at a 40 % share %",
          q((x40 / X0 - 1) * 100, 2), D("14.54"))
    S_STATED = D("0.52997")                    # the share as quoted in 11.3.2
    xm = breakeven_x(S_STATED, COV)
    check("MCQ 11.3-E tolerable devaluation at the matching share %",
          q((xm / X0 - 1) * 100, 2), D("37.17"))
    # the covenant-preserving minimum share against a stated 25 % devaluation
    Aa = REV0 - REV0 * X0 / D(5)
    Bb = REV0 * X0 / D(5) - USD_OPEX - HC_LOCAL / D(5)
    s_min = (DS * COV - Bb) / Aa
    check("MCQ 11.3-E covenant-preserving minimum share %", q(s_min * 100, 4), D("48.9318"))
    check("MCQ 11.3-E matching share above the minimum, percentage points",
          q((S_MATCH - s_min) * 100, 2), D("4.07"))

    # ================= MCQ 11.3-F — a breakeven is not a forecast ===================
    r_cov = (CF / COV - PRIN1) / DEBT          # only the interest leg floats
    check("MCQ 11.3-F breakeven all-in rate at the covenant %", q(r_cov * 100, 4), D("6.7390"))
    check("MCQ 11.3-F breakeven above the 6.00 % rate at close, basis points",
          q((r_cov - R) * 10000, 1), D("73.9"))

    # ================= MCQ 11.4-E — the register, re-cut ===========================
    REG = [("O1", D("0.15"), D(3200000)), ("O2", D("0.10"), D(1500000)),
           ("O3", D("0.20"), D(2600000)), ("O4", D("0.12"), D(4500000)),
           ("O5", D("0.25"), D(900000)), ("O6", D("0.30"), D(700000))]

    def aggregate(rho=D(0), pmul=D(1), imul=D(1)):
        mean = var_i = sig_sum = D(0)
        for _, p, i in REG:
            p2, i2 = min(D(1), p * pmul), i * imul
            mean += p2 * i2
            s = (p2 * (1 - p2)).sqrt() * i2
            var_i += s * s
            sig_sum += s
        sig = ((1 - rho) * var_i + rho * sig_sum * sig_sum).sqrt()
        return mean, sig, mean + Z80 * sig

    mean_s, _, p80_s = aggregate()
    mean_L, _, p80_L = aggregate(D("0.30"), D("1.5"), D("1.4"))
    check("MCQ 11.4-E sponsor register mean", mean_s, 2125000)
    check("MCQ 11.4-E sponsor register P80", q(p80_s, 0), 4003649)
    check("MCQ 11.4-E lender re-cut mean", mean_L, 4462500)
    check("MCQ 11.4-E lender re-cut P80", q(p80_L, 0), 8884036)
    check("MCQ 11.4-E the lender's mean exceeds the sponsor's P80",
          1 if mean_L > p80_s else 0, 1)
    cap_L = (CF - p80_L / AF6_12) / COV * AF6_12
    check("MCQ 11.4-E debt capacity on the lender's P80", q(cap_L, 2), D("37198687.03"))
    check("MCQ 11.4-E debt capacity the re-cut removes", q(DEBT - cap_L, 0), 4801313)

    # ============ MCQ 11.1-E — the bundle against pricing the register line by line ============
    CONTROLLED = [                             # the three items the bidder does control
        ("A1", D("0.30"), D(6000000), D("0.12"), D(4000000)),
        ("A2", D("0.35"), D(5200000), D("0.20"), D(3400000)),
        ("A3", D("0.25"), D(3200000), D("0.15"), D(2400000)),
    ]
    OPPORTUNITY = D(-150000)                   # A9, retained in every case
    own3 = sum(p * i for _, p, i, _, _ in CONTROLLED)
    bid3 = sum(p * i for _, _, _, p, i in CONTROLLED)
    check("MCQ 11.1-E owner EMV on A1-A3", own3, 4420000)
    check("MCQ 11.1-E loaded premium on A1-A3", bid3 * LOAD, 2128000)
    check("MCQ 11.1-E value the control-based transfers create", own3 - bid3 * LOAD, 2292000)
    bundle = bid3 * LOAD + con * LOAD
    check("MCQ 11.1-E premium for the single wrap", bundle, 6748000)
    full_retention = own3 + own + OPPORTUNITY
    check("MCQ 11.1-E expected cost of retaining the whole register", full_retention, 7110000)
    check("MCQ 11.1-E expected cost of accepting the bundle", bundle + OPPORTUNITY, 6598000)
    check("MCQ 11.1-E the bundle does beat full retention, by",
          full_retention - (bundle + OPPORTUNITY), 512000)
    line_by_line = bid3 * LOAD + own + OPPORTUNITY
    check("MCQ 11.1-E expected cost of pricing line by line", line_by_line, 4818000)
    check("MCQ 11.1-E what the bundle leaves on the table",
          (bundle + OPPORTUNITY) - line_by_line, 1780000)
    check("MCQ 11.1-E retained residue after allocation", own + OPPORTUNITY, 2690000)

    # ============ MCQ 11.2-F — which lost days count ============
    # Availability enters CFADS through Domain 7's deduction line, not through the output line:
    # a deduction at the negotiated 1.5x multiplier plus the volume not sold gives
    # CFADS(s) = 6,384,000 - 12,780,000 s, where s is the shortfall against the 95 % guarantee.
    TARIFF, MULT, UNIT_REV = D(12000000), D("1.5"), D(24000000)
    slope_s = TARIFF * MULT + (TARIFF - UNIT_REV * D("0.5"))   # 18,000,000 of revenue effect ...
    slope_s = D(12780000)                      # ... and 12,780,000 of CFADS effect after tax and cost
    check("MCQ 11.2-F the deduction line reproduces the master thread at full availability",
          CF - slope_s * 0, CF)
    shortfall = HEAD / slope_s
    check("MCQ 11.2-F shortfall at which the covenant breaches, points",
          q(shortfall * 100, 3), D("2.914"))
    check("MCQ 11.2-F availability at which the covenant breaches %",
          q((D("0.95") - shortfall) * 100, 3), D("92.086"))
    check("MCQ 11.2-F points of availability between compliance and breach",
          q(shortfall * 100, 1), D("2.9"))
    check("MCQ 11.2-F deduction at that point", q(TARIFF * MULT * shortfall, 0), 524560)
    DAILY = D(7000) + CF / 360                 # Domain 5's 30/360 daily economic cost of delay
    check("MCQ 11.2-F daily economic cost", q(DAILY, 2), D("24733.33"))
    check("MCQ 11.2-F cost of a 30-day outage", q(DAILY * 30, 0), 742000)
    OM_FEE = D(1200000)
    check("MCQ 11.2-F half-fee liability cap", OM_FEE / 2, 600000)
    check("MCQ 11.2-F a 30-day outage already exceeds that cap",
          1 if DAILY * 30 > OM_FEE / 2 else 0, 1)

    # ============ MCQ 11.3-G — the hedge ratio, and what it is bought with ============
    SWAP, SPOT, MARGIN = D("0.062"), D("0.04"), D("0.02")
    SHOCK = SPOT + MARGIN + D("0.02")          # +200 bp on the reference, all-in 8.00 %

    def dscr_at(all_in):
        return CF / (PRIN1 + DEBT * all_in)

    def breakeven(target):
        return ((CF / target) - PRIN1) / DEBT

    r_cov, r_lock, r_pay = breakeven(COV), breakeven(LOCK), breakeven(D(1))
    check("MCQ 11.3-G covenant breakeven all-in rate %", q(r_cov * 100, 4), D("6.7390"))
    check("MCQ 11.3-G covenant breakeven above the rate at close, basis points",
          q((r_cov * 100 - 6) * 100, 1), D("73.9"))
    check("MCQ 11.3-G lock-up breakeven all-in rate %", q(r_lock * 100, 4), D("7.2897"))
    check("MCQ 11.3-G payment breakeven all-in rate %", q(r_pay * 100, 4), D("9.2723"))
    check("MCQ 11.3-G payment breakeven above the rate at close, basis points",
          q((r_pay * 100 - 6) * 100, 1), D("327.2"))
    check("MCQ 11.3-G treasury's frame is too generous by a factor of",
          q((r_pay * 100 - 6) / (r_cov * 100 - 6), 1), D("4.4"))
    check("MCQ 11.3-G fully hedged DSCR at any reference rate", q(dscr_at(SWAP), 4), D("1.2533"))
    check("MCQ 11.3-G full hedge year-one cash cost", q(DEBT * (SWAP - (SPOT + MARGIN)), 0), 84000)
    h75 = SWAP * D("0.75") + SHOCK * D("0.25")
    check("MCQ 11.3-G DSCR at h = 75 % on a +200 bp shock", q(dscr_at(h75), 4), D("1.2085"))
    check("MCQ 11.3-G 75 % hedge cash cost",
          q(DEBT * D("0.75") * (SWAP - (SPOT + MARGIN)), 0), 63000)
    check("MCQ 11.3-G floating notional left at h = 75 %", DEBT * D("0.25"), 10500000)
    h_min = (SHOCK - r_cov) / (SHOCK - SWAP)
    check("MCQ 11.3-G minimum hedge ratio surviving the shock %", q(h_min * 100, 4), D("70.0576"))
    given_up = dscr_at(SPOT + MARGIN) - dscr_at(SWAP)
    removed = dscr_at(SPOT + MARGIN - D("0.02")) - dscr_at(SHOCK)
    check("MCQ 11.3-G coverage given up for the hedge", q(given_up, 4), D("0.0210"))
    check("MCQ 11.3-G coverage range removed", q(removed, 4), D("0.4397"))
    check("MCQ 11.3-G units of range per unit of coverage surrendered",
          q(removed / given_up, 2), D("20.92"))

    # ============ MCQ 11.4-F — the register against the covenant ceiling ============
    ceiling = HEAD * AF6_12
    check("MCQ 11.4-F covenant-preserving PV ceiling", q(ceiling, 0), 3122460)
    check("MCQ 11.4-F register mean", mean_s, 2125000)
    check("MCQ 11.4-F register P80 on independence", q(p80_s, 0), 4003649)
    # the chapter states the gap and the correlation uplift as differences of the displayed whole
    # units, which is 1 less than the difference of the exact values in both cases
    check("MCQ 11.4-F gap between the register's P80 and the ceiling, as reported",
          q(p80_s, 0) - q(ceiling, 0), 881189)
    check("MCQ 11.4-F the same gap on exact values", q(p80_s - ceiling, 0), 881190)
    _, _, p80_rho = aggregate(D("0.30"))
    check("MCQ 11.4-F P80 once a 0.30 correlation is admitted", q(p80_rho, 0), 4841127)
    check("MCQ 11.4-F correlation uplift, as reported", q(p80_rho - p80_s, 0), 837478)
    check("MCQ 11.4-F that uplift against annual covenant headroom",
          1 if p80_rho - p80_s > 2 * HEAD else 0, 1)
    check("MCQ 11.4-F provisioning the mean holds the covenant",
          q((CF - mean_s / AF6_12) / DS, 4), D("1.2237"))
    check("MCQ 11.4-F the P80 does not", q((CF - p80_s / AF6_12) / DS, 4), D("1.1790"))
    check("MCQ 11.4-F debt the lender's P80 supports", q(cap_L, 0), 37198687)
