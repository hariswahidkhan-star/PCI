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

    # ===================== Capstone Four — Northgate Point (re-contracting) =============
    # The capstone's argument is that a tenor extension past the contracted period buys almost
    # nothing once the tail is honestly stressed. That convergence is the claim, so it is pinned as
    # an invariant rather than left as a sentence.
    CAPEX_N, MW = D(720000000), D(60)
    ANCHOR_MW, ANCHOR_RATE = D(40000), D(120)      # kW, per kW-month
    REST_MW, REST_RATE = D(20000), D(130)
    OPEX_N, RATE_N, REQ_N = D(22800000), D("0.06"), D("1.35")
    LEASE, TENOR_N = 7, 12

    REV_N = ANCHOR_MW * ANCHOR_RATE * 12 + REST_MW * REST_RATE * 12
    check("G.4 annual revenue", REV_N, D(88800000))
    check("G.4 capex per MW", CAPEX_N / MW, D(12000000))
    NOI = REV_N - OPEX_N
    check("G.4.1 net operating income", NOI, D(66000000))
    AF12, AF7, AF5 = af(RATE_N, 12), af(RATE_N, 7), af(RATE_N, 5)
    check("G.4.1 AF(0.06,12)", AF12, D("8.383844"), tol=D("0.0000005"))
    check("G.4.1 AF(0.06,7)", AF7, D("5.582381"), tol=D("0.0000005"))
    SVC_N = NOI / REQ_N
    check("G.4.1 service on contracted cash", SVC_N, D("48888888.89"), tol=D("0.01"))
    FAC2 = SVC_N * AF12
    check("G.4.1 12-year facility on contracted cash", FAC2, D("409876814.86"), tol=D("0.05"))
    check("G.4.1 loan to cost (%)", FAC2 / CAPEX_N * 100, D("56.93"), tol=D("0.005"))
    BAL7 = SVC_N * AF5
    check("G.4.1 balance entering year eight", BAL7, D("205937785.07"), tol=D("0.05"))
    check("G.4.1 that balance as a share of the facility (%)", BAL7 / FAC2 * 100, D("50.24"),
          tol=D("0.005"))
    # zero tolerance is a CONSEQUENCE of the sizing, and is derived rather than asserted
    BREAKEVEN_F = (SVC_N * REQ_N + OPEX_N) / REV_N
    check("G.4.1 breakeven re-let factor on the contracted-cash facility", BREAKEVEN_F, D(1),
          tol=D("0.0000001"))
    check("G.4.1 INVARIANT the tolerance is exactly zero, by construction",
          (1 - BREAKEVEN_F) * 100, D(0), tol=D("0.0000001"))
    LEV_N = REV_N / NOI
    check("G.4.1 operating leverage", LEV_N, D("1.3455"), tol=D("0.00005"))
    check("G.4.1 a 15 % rent fall as an NOI fall (%)", D("0.15") * LEV_N * 100, D("20.1818"),
          tol=D("0.00005"))
    for rent, occ, noi_v, dscr in ((D(1), D(1), D(66000000), D("1.3500")),
                                   (D("0.95"), D("0.95"), D(57342000), D("1.1729")),
                                   (D("0.90"), D("0.92"), D(50726400), D("1.0376")),
                                   (D("0.85"), D("0.90"), D(45132000), D("0.9232"))):
        check(f"G.4.1 NOI at rent {rent} occupancy {occ}", REV_N * rent * occ - OPEX_N, noi_v)
        check(f"G.4.1 DSCR at rent {rent} occupancy {occ}",
              (REV_N * rent * occ - OPEX_N) / SVC_N, dscr, tol=D("0.00005"))
    NOI_STRESS = REV_N * D("0.85") * D("0.90") - OPEX_N
    check("G.4.1 INVARIANT the stressed case is a payment default, not a breach",
          D(1) if NOI_STRESS / SVC_N < 1 else D(0), D(1))

    # G.4.2 the three structures, and the convergence that is the finding
    FAC1 = SVC_N * AF7
    check("G.4.2 facility with tenor matched to the lease", FAC1, D("272916425.94"),
          tol=D("0.05"))
    check("G.4.2 its loan to cost (%)", FAC1 / CAPEX_N * 100, D("37.91"), tol=D("0.005"))
    SVC3 = NOI_STRESS / REQ_N
    check("G.4.2 service sized on the stressed re-let", SVC3, D("33431111.11"), tol=D("0.01"))
    FAC3 = SVC3 * AF12
    check("G.4.2 12-year facility sized on the stressed re-let", FAC3, D("280281218.31"),
          tol=D("0.05"))
    check("G.4.2 its loan to cost (%)", FAC3 / CAPEX_N * 100, D("38.93"), tol=D("0.005"))
    check("G.4.2 its cover on today's cash", NOI / SVC3, D("1.9742"), tol=D("0.00005"))
    BREAK3 = (SVC3 * REQ_N + OPEX_N) / REV_N
    check("G.4.2 its breakeven re-let factor (%)", BREAK3 * 100, D("76.5000"), tol=D("0.00005"))
    check("G.4.2 its tolerance in points of revenue", (1 - BREAK3) * 100, D("23.5000"),
          tol=D("0.00005"))
    check("G.4.2 what the extra tenor really buys", FAC3 - FAC1, D("7364792.37"), tol=D("0.05"))
    check("G.4.2 as a share of the matched-tenor facility (%)", (FAC3 - FAC1) / FAC1 * 100,
          D("2.70"), tol=D("0.005"))
    check("G.4.2 what the naive facility appears to buy", FAC2 - FAC1, D("136960388.93"),
          tol=D("0.05"))
    check("G.4.2 INVARIANT the honest 12-year facility converges on the 7-year one",
          D(1) if (FAC3 - FAC1) / FAC1 < D("0.05") else D(0), D(1))
    check("G.4.2 INVARIANT and is far below what contracted-cash sizing claims",
          D(1) if FAC3 < FAC2 else D(0), D(1))
    check("G.4.2 the position taken by choosing structure 2 over structure 3",
          FAC2 - FAC3, D("129595596.55"), tol=D("0.05"))

    # ---- the closing synthesis: no two capstones bind in the same place -------------------
    # Each figure is the binding value established in its own capstone above (or, for Kestrel, in
    # pfl_d17_capstone.py). What is asserted here is the RELATION the closing table claims.
    BINDING = {"Kestrel distribution test": D("1.9103"),      # % of headroom
               "Aurora Ridge year one": D("0.5343"),           # DSCR
               "Helios Flats year eighteen": D("1.0595"),      # DSCR if sized on year one
               "Northgate Point year eight": D("0.9232")}      # DSCR on the stressed re-let
    check("Synthesis: four distinct binding tests", D(len(BINDING)), D(4))
    check("Synthesis: four distinct binding values", D(len(set(BINDING.values()))), D(4))
    DEFAULTS = [D("0.5343"), D("1.0595"), D("0.9232")]
    check("Synthesis INVARIANT three of the four reach a payment or coverage failure below 1.20",
          D(1) if all(v < D("1.20") for v in DEFAULTS) else D(0), D(1))
    check("Synthesis INVARIANT two of the three fall below 1.00 — payment default, not breach",
          D(sum(1 for v in DEFAULTS if v < 1)), D(2))
