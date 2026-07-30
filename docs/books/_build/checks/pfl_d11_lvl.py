"""PFL-AI Domain 11 — golden checks for the Evaluation and Comprehension MCQs added to KA 11.1–11.4.

Scope: every number printed in MCQ 11.1-D, 11.2-D, 11.2-E, 11.3-E, 11.3-F and 11.4-E — options and
rationales. `pfl_d11.py` owns the domain's worked examples; this module owns only the added items.
Master-thread values come from ctx, never re-typed.

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
