"""PFL-AI Appendix G, Capstone Three — Helios Flats. Golden-answer checks.

Kept in its own module so that Capstone Three's arithmetic can be verified in isolation while
pfl_d17_capstone.py (Capstones One and Two) is being edited, and so that neither file becomes a
contention point for two writers.

The capstone's argument is a comparison, and comparisons are where this appendix has been wrong
before, so the comparison itself is pinned as an invariant: **a 1.20x requirement on P90 cash in the
degraded final year is MORE demanding than Aurora Ridge's 1.40x on a level P50-equivalent base.**
If that ever stops being true of these figures, the chapter's title stops being true with it.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    CAPEX, SOLAR, BATT = D(180000000), D(140000000), D(40000000)
    E50, PPA = D(460000), D("42.00")
    OM, RES = D(4320000), D(1000000)
    SIGMA, Z90, DEG = D("0.070"), D("1.2816"), D("0.005")
    RATE, TENOR, REQ = D("0.065"), 18, D("1.20")

    def af(r, n):
        r = D(str(r))
        return (1 - (1 + r) ** -n) / r

    AF = af(RATE, TENOR)
    check("G.3 AF(0.065,18)", AF, D("10.432466"), tol=D("0.0000005"))
    check("G.3 capex splits to the stated total", SOLAR + BATT, CAPEX)

    # ---- G.3.1 the risk is priced in the quantity ----------------------------------------
    REV50 = E50 * PPA
    check("G.3.1 P50 revenue", REV50, D(19320000))
    check("G.3.1 P50 EBITDA", REV50 - OM, D(15000000))
    C50 = REV50 - OM - RES
    check("G.3.1 P50 first-year CFADS", C50, D(14000000))
    F90 = 1 - Z90 * SIGMA
    check("G.3.1 P90 factor", F90, D("0.910288"), tol=D("0.0000005"))
    check("G.3.1 P90 energy (MWh)", E50 * F90, D("418732.5"), tol=D("0.05"))
    REV90 = E50 * F90 * PPA
    check("G.3.1 P90 revenue", REV90, D("17586764.16"))
    C90 = REV90 - OM - RES
    check("G.3.1 P90 first-year CFADS", C90, D("12266764.16"))
    check("G.3.1 energy shortfall at P90 (%)", Z90 * SIGMA * 100, D("8.9712"), tol=D("0.00005"))
    check("G.3.1 CFADS shortfall at P90 (%)", (C50 - C90) / C50 * 100, D("12.3803"),
          tol=D("0.00005"))
    LEV = ((C50 - C90) / C50) / (Z90 * SIGMA)
    check("G.3.1 operating leverage", LEV, D("1.3800"), tol=D("0.00005"))
    # the leverage is exactly revenue/CFADS, which is why fixed cost drives it
    check("G.3.1 INVARIANT leverage equals revenue over CFADS", LEV, REV50 / C50,
          tol=D("0.0000001"))
    check("G.3.1 INVARIANT cash is more sensitive than the resource",
          D(1) if LEV > 1 else D(0), D(1))

    # ---- G.3.2 the binding year is the last -----------------------------------------------
    def c90_year(y):
        return E50 * (1 - DEG) ** (y - 1) * F90 * PPA - OM - RES

    for y, e, c in ((1, D("418732.5"), D("12266764.16")),
                    (5, D("410420.4"), D("11917658.11")),
                    (10, D("400262.0"), D("11491004.58")),
                    (18, D("384528.9"), D("10830215.15"))):
        check(f"G.3.2 year {y} P90 energy (MWh)", E50 * (1 - DEG) ** (y - 1) * F90, e,
              tol=D("0.05"))
        check(f"G.3.2 year {y} P90 CFADS", c90_year(y), c, tol=D("0.005"))
    check("G.3.2 cumulative degradation factor at year 18", (1 - DEG) ** 17, D("0.918316"),
          tol=D("0.0000005"))
    C18 = c90_year(18)
    check("G.3.2 decline from year 1 to year 18 (%)", (C90 - C18) / C90 * 100, D("11.7109"),
          tol=D("0.00005"))
    check("G.3.2 INVARIANT coverage falls monotonically across the tenor",
          D(1) if all(c90_year(y) > c90_year(y + 1) for y in range(1, 18)) else D(0), D(1))
    check("G.3.2 INVARIANT the minimum-coverage year is the last, not the first",
          D(1) if min(range(1, 19), key=c90_year) == 18 else D(0), D(1))

    SVC1, SVC18 = C90 / REQ, C18 / REQ
    check("G.3.2 service if sized on year one", SVC1, D("10222303.47"))
    check("G.3.2 capacity if sized on year one", SVC1 * AF, D("106643837.27"), tol=D("0.05"))
    check("G.3.2 service if sized on the binding year", SVC18, D("9025179.29"))
    DEBT = SVC18 * AF
    check("G.3.2 capacity sized on the binding year", DEBT, D("94154879.59"), tol=D("0.05"))
    check("G.3.2 capacity given up by sizing correctly", SVC1 * AF - DEBT,
          D("12488957.68"), tol=D("0.05"))
    check("G.3.2 year-18 DSCR if sized on year one", C18 / SVC1, D("1.0595"), tol=D("0.00005"))
    check("G.3.2 INVARIANT sizing on year one breaches the 1.20x covenant by year 18",
          D(1) if C18 / SVC1 < REQ else D(0), D(1))
    check("G.3.2 INVARIANT sizing on the binding year holds the covenant in every year",
          D(1) if all(c90_year(y) / SVC18 >= REQ for y in range(1, 19)) else D(0), D(1))

    # ---- G.3.3 the ratio restated — the capstone's title ---------------------------------
    # The restatement must be done on CASH. Scaling the ratio by the two ENERGY factors is the
    # intuitive move and is wrong, because fixed operations mean CFADS does not fall in proportion
    # to generation — G.3.1's operating leverage again. The reversibility invariant below is what
    # caught that: it failed against the energy-scaled figure by 7.6 million of facility.
    EQ_RES = REQ * C50 / C90
    check("G.3.3 restated onto first-year P50 cash, resource only", EQ_RES, D("1.3696"),
          tol=D("0.00005"))
    EQ_DEG = REQ * C50 / C18
    check("G.3.3 restated onto first-year P50 cash, resource and degradation", EQ_DEG,
          D("1.5512"), tol=D("0.00005"))
    AURORA = D("1.40")            # Capstone Two, on a level P50-equivalent base
    check("G.3.3 INVARIANT the quoted 1.20x is MORE demanding than Aurora Ridge's 1.40x",
          D(1) if EQ_DEG > AURORA else D(0), D(1))
    check("G.3.3 by how much, in turns of cover", EQ_DEG - AURORA, D("0.1512"),
          tol=D("0.00005"))
    check("G.3.3 apparent advantage on the quoted ratios (turns)", AURORA - REQ, D("0.20"))
    # the energy-scaled shortcut, and the size of its error
    EQ_WRONG = REQ / F90 / (1 - DEG) ** 17
    check("G.3.3 the energy-scaled shortcut", EQ_WRONG, D("1.4355"), tol=D("0.00005"))
    check("G.3.3 by how much the shortcut understates the requirement", EQ_DEG - EQ_WRONG,
          D("0.1157"), tol=D("0.00005"))
    # The shortcut lands ABOVE 1.40, so it reaches the right conclusion — it understates the size
    # of it. Written as the invariant it actually is, after the first version asserted the opposite
    # and failed.
    check("G.3.3 INVARIANT the shortcut still exceeds Aurora Ridge, so the direction survives",
          D(1) if EQ_WRONG > AURORA else D(0), D(1))
    check("G.3.3 margin the shortcut reports", EQ_WRONG - AURORA, D("0.0355"), tol=D("0.00005"))
    check("G.3.3 share of the true margin the shortcut reports (%)",
          (EQ_WRONG - AURORA) / (EQ_DEG - AURORA) * 100, D("23.49"), tol=D("0.005"))
    check("G.3.3 cash the energy-scaled route wrongly predicts for year 18",
          C50 * F90 * (1 - DEG) ** 17, D("11703054.46"), tol=D("0.05"))
    check("G.3.3 the fixed cost that explains the gap",
          C50 * F90 * (1 - DEG) ** 17 - C18, D("872839.31"), tol=D("0.05"))
    # the restatement must be reversible: applying the equivalent ratio to P50 year-1 cash
    # reproduces the same facility the correct sizing produced
    check("G.3.3 INVARIANT the restatement is reversible to the same facility",
          C50 / EQ_DEG * AF, DEBT, tol=D("0.5"))

    # ---- G.3.4 the battery earns no contracted revenue -----------------------------------
    check("G.3.4 gearing against the contracted (solar) asset (%)", DEBT / SOLAR * 100,
          D("67.25"), tol=D("0.005"))
    check("G.3.4 gearing against total capex (%)", DEBT / CAPEX * 100, D("52.31"),
          tol=D("0.005"))
    check("G.3.4 equity or merchant requirement", CAPEX - DEBT, D("85845120.41"), tol=D("0.05"))
    check("G.3.4 that requirement as a share of capex (%)", (CAPEX - DEBT) / CAPEX * 100,
          D("47.69"), tol=D("0.005"))
    check("G.3.4 blended gearing dilution from the battery (points)",
          DEBT / SOLAR * 100 - DEBT / CAPEX * 100, D("14.9452"), tol=D("0.00005"))
    check("G.3.4 INVARIANT the two gearing shares differ by exactly the battery's weight",
          DEBT / SOLAR - DEBT / CAPEX, DEBT * BATT / (SOLAR * CAPEX), tol=D("0.0000001"))
