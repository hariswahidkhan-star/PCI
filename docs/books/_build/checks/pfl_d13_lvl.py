"""PFL-AI Domain 13 — Evaluation and Comprehension MCQ batch: golden checks.

Every number appearing in an option or a rationale of the items added to
`domain-13-diligence-financial-close.md` in this batch — MCQ 13.1-G, 13.2-G, 13.3-G, 13.3-H,
13.4-G and 13.4-H — is recomputed here from first principles. Master-thread values are read from
ctx and never re-typed.

Two engines are exercised.

  1. The conditions-precedent conjunction (KA 13.3.2). The close date is the maximum over five
     dependency chains of their realised durations, so the close-date distribution is enumerated
     over the eight binding outcomes rather than copied from the manuscript: chains A and E are
     shown to be unable to bind even when slipped, `E[close]` is shown to exceed the maximum of the
     chains' expectations, and `E[close]` is shown to be *linear* in a single chain's slip
     probability — which is what makes the value of de-risking chain B a budget rather than a
     forecast.
  2. Finding classification and close-cost arithmetic (KA 13.2, 13.3.4, 13.4.1-13.4.2): a Class 2
     finding tested against the stated materiality threshold expressed in the transaction's own
     metric, the close-cost budget as a share of debt raised, and the praecipium/pool split with the
     arranger's yield differential.

Distractors are recomputed as well as keys, because a distractor that is arithmetically impossible
gives the answer away.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    DEBT = ctx["KESTREL_DEBT"]                     # 42,000,000
    CAPEX = ctx["KESTREL_CAPEX"]                   # 60,000,000
    DS = ctx["KESTREL_INSTALMENT"]                 # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                      # 6,384,000
    COV = D("1.20")


    # ---- MCQ 13.2-G: a Class 2 finding against the stated materiality threshold ----
    # The threshold is 0.01x of DSCR, which in annual CFADS is 0.01 x debt service.
    THRESH_CASH = DS * D("0.01")
    C2 = D(180000)                                  # the finding's effect on year-one CFADS
    check("MCQ 13.2-G materiality threshold in CFADS (0.01x DSCR)",
          THRESH_CASH.quantize(D("0.01")), D("50096.35"))
    check("MCQ 13.2-G finding effect in DSCR terms",
          (C2 / DS).quantize(D("0.0001")), D("0.0359"))
    check("MCQ 13.2-G finding is above the threshold, so Class 2 not Class 3",
          1 if C2 > THRESH_CASH else 0, 1)
    check("MCQ 13.2-G finding changes no conclusion: DSCR still clears the covenant",
          1 if (CF - C2) / DS > COV else 0, 1)
    check("MCQ 13.2-G DSCR after the finding", ((CF - C2) / DS).quantize(D("0.0001")),
          D("1.2384"))

    # ---- MCQ 13.3-G: the close-date distribution, re-derived from the chains ----
    # Chains: (base, slip, p).  A and E can never bind: even slipped they fall short of 20.
    CHAINS = {"A": (D(9), D(3), D("0.20")), "B": (D(18), D(6), D("0.35")),
              "C": (D(20), D(4), D("0.30")), "D": (D(17), D(5), D("0.25")),
              "E": (D(7), D(2), D("0.15"))}
    # Cost of delay: Domain 5's 17,733.33 a day on a 30/360 basis, carried as a calendar week.
    # The exact weekly figure is CFADS/360 x 7; the manuscript displays it as 124,133.33.
    DELAY = CF / 360 * 7
    check("MCQ 13.3-G cost of delay per calendar week as displayed",
          DELAY.quantize(D("0.01")), D("124133.33"))
    BASE_CLOSE = max(b for b, _, _ in CHAINS.values())
    check("MCQ 13.3-G base close = max of chain base durations", BASE_CLOSE, 20)
    for k in ("A", "E"):
        b, s, _ = CHAINS[k]
        check(f"MCQ 13.3-G chain {k} cannot bind even when slipped", 1 if b + s < BASE_CLOSE else 0, 1)

    def close_distribution(pb=None):
        """Exact distribution of max(base close, realised B, C, D) over the 8 binding outcomes."""
        dist = {}
        rows = []
        for k in ("B", "C", "D"):
            b, s, p = CHAINS[k]
            rows.append((b, b + s, CHAINS[k][2] if (k != "B" or pb is None) else pb))
        for i in (0, 1):
            for j in (0, 1):
                for m in (0, 1):
                    prob = D(1)
                    dur = BASE_CLOSE
                    for idx, flag in ((0, i), (1, j), (2, m)):
                        base, slipped, p = rows[idx]
                        prob *= p if flag else (1 - p)
                        dur = max(dur, slipped if flag else base)
                    dist[dur] = dist.get(dur, D(0)) + prob
        return dist

    DIST_BASE = close_distribution()
    check("MCQ 13.3-G probabilities sum to one", sum(DIST_BASE.values()), 1)
    for wk, pct in ((20, "34.125"), (22, "11.375"), (24, "54.500")):
        check(f"MCQ 13.3-G probability of closing at {wk} weeks %",
              (DIST_BASE[D(wk)] * 100).quantize(D("0.001")), D(pct))
    check("MCQ 13.3-G the distribution has only three outcomes", len(DIST_BASE), 3)
    e_close = sum(k * v for k, v in DIST_BASE.items())
    check("MCQ 13.3-G E[close] weeks", e_close.quantize(D("0.0001")), D("22.4075"))
    max_of_exp = max(b + p * s for b, s, p in CHAINS.values())
    check("MCQ 13.3-G max of expectations (the standard error) weeks",
          max_of_exp.quantize(D("0.0001")), D("21.2000"))
    check("MCQ 13.3-G expected maximum exceeds the maximum of expectations",
          1 if e_close > max_of_exp else 0, 1)
    check("MCQ 13.3-G expected slip cost", ((e_close - BASE_CLOSE) * DELAY).quantize(D("1")),
          D("298851"))
    # The bimodality the item turns on: almost no mass at the mean.
    check("MCQ 13.3-G mass at the two modes %",
          ((DIST_BASE[D(20)] + DIST_BASE[D(24)]) * 100).quantize(D("0.001")), D("88.625"))
    # E[close] is linear in chain B's slip probability — quoted in the rationale of 13.3-G.
    E_AT = {p: sum(k * v for k, v in close_distribution(D(p)).items())
            for p in ("0.35", "0.10", "0.00")}
    check("MCQ 13.3-G E[close] at p_B = 0.10", E_AT["0.10"].quantize(D("0.0001")), D("21.7950"))
    check("MCQ 13.3-G probability of the base date at p_B = 0.10 %",
          (close_distribution(D("0.10"))[D(20)] * 100).quantize(D("0.001")), D("47.250"))
    check("MCQ 13.3-G value of taking p_B to 0.10",
          ((E_AT["0.35"] - E_AT["0.10"]) * DELAY).quantize(D("0.01")), D("76031.67"))
    check("MCQ 13.3-G E[close] is linear in p_B (slope per unit of p)",
          ((E_AT["0.35"] - E_AT["0.00"]) / D("0.35")).quantize(D("0.0001")), D("2.4500"))

    # ---- MCQ 13.4-G: what the close-cost budget is, and what it is at risk against ----
    CLOSE_COSTS = D(2709000)
    LENDER_FEES = D(544000)
    check("MCQ 13.4-G close costs as % of debt raised",
          (CLOSE_COSTS / DEBT * 100).quantize(D("0.01")), D("6.45"))
    check("MCQ 13.4-G close costs as % of the envelope",
          (CLOSE_COSTS / CAPEX * 100).quantize(D("0.001")), D("4.515"))
    check("MCQ 13.4-G third-party and sponsor-side share", CLOSE_COSTS - LENDER_FEES, 2165000)
    # A best-efforts route risks the whole budget being spent before the book is known to fill;
    # the arrangement fee is the part of it that buys certainty of funds.
    check("MCQ 13.4-G arrangement fee at 1.20 %", DEBT * D("0.0120"), 504000)
    check("MCQ 13.4-G budget at risk as a multiple of the arrangement fee",
          (CLOSE_COSTS / (DEBT * D("0.0120"))).quantize(D("0.0001")), D("5.3750"))

    # ---- MCQ 13.4-H: praecipium and pool, and what each pays for ----
    PRAE = DEBT * D("0.0025")
    POOL = DEBT * D("0.0120") - PRAE
    HOLD = D(15000000)
    check("MCQ 13.4-H praecipium", PRAE, 105000)
    check("MCQ 13.4-H participation pool", POOL, 399000)
    check("MCQ 13.4-H pool rate %", (POOL / DEBT * 100).quantize(D("0.01")), D("0.95"))
    check("MCQ 13.4-H arranger fee earned", PRAE + POOL / DEBT * HOLD, 247500)
    check("MCQ 13.4-H arranger yield on final hold %",
          ((PRAE + POOL / DEBT * HOLD) / HOLD * 100).quantize(D("0.001")), D("1.650"))
    check("MCQ 13.4-H participant yield %", (POOL / DEBT * 100).quantize(D("0.001")), D("0.950"))
    check("MCQ 13.4-H yield differential in basis points",
          (((PRAE + POOL / DEBT * HOLD) / HOLD - POOL / DEBT) * 10000).quantize(D("0.1")),
          D("70.0"))
