"""PML-AI Appendix G — capstones Two, Three and Four. Golden-answer checks.

Capstone One (`pml_d17_capstone.py`) assembles the Meridian master thread: its arithmetic is the
arithmetic of putting sixteen chapters' results side by side. These three are different. Each is a
NEW programme with a different failure shape, so nothing here is a re-statement of a domain result —
every figure is produced by this appendix and has no other owner, which is exactly why it needs its
own module.

  * G.2  Calderhall Metro Line 3 — physical interface count against an immovable date.
  * G.3  Sable Plains Energy — a pipeline, not a project, as the unit of management.
  * G.4  the National Entitlement Service — decision rights split across bodies that each hold a
         veto and none of which holds the outcome.

Two rules govern what is written here. **Inputs are not checked.** A case fact — an interface unit
cost, a committee's meeting interval, a gate's pass rate — is an assumption of the case, and a line
comparing it to itself reports coverage it does not provide; the suite fails on that shape. So each
input appears once, as a named constant, and only what FOLLOWS from it is asserted. **Invariants are
checked, not only values.** Each capstone's argument depends on identities — a sum that must close, a
bound that must hold, a comparison that must run one way — and those are pinned so the argument
cannot silently stop being true when a figure is edited.

Attributed to domain 17 with Capstone One so that Appendix C reports the capstone record separately
from the sixteen domains' rather than inflating one of them.
"""


def run(ctx):
    check, D, mesh = ctx["check"], ctx["D"], ctx["mesh"]
    ew = ctx["ew"]

    # =====================================================================================
    # G.2  CALDERHALL METRO LINE 3 — the interface count and the immovable date
    # =====================================================================================
    # ---- inputs (case facts, stated once) ----
    N_BASE, N_GROWN = 16, 19          # controlled subsystems, baseline and after design growth
    REG_BASE, REG_GROWN = D(80), D(126)   # interface register: interfaces genuinely required
    IF_COST = D(145000)              # per bilateral interface: specify, build, test, witness, document
    LAYER_COST = D(5760000)          # systems-integration authority + integration test facility
    CAPEX = D(2880000000)            # programme capital expenditure
    TD_BILATERAL = D(10)             # witnessed test-days per bilateral interface
    TD_LAYERED = D(20)               # witnessed test-days per conformance test against the baseline
    STREAMS, TEST_DAYS_MONTH = D(5), D(20)   # concurrent test streams; working days a month
    CIVILS, INSTALL, TRIAL, AUTHORISE = D(32), D(14), D(4), D(3)   # critical path, months
    FIXED_DATE = D(63)               # committed opening, months from notice to proceed
    FARE_REVENUE_M = D(1890000)      # deferred farebox and commercial revenue, per month
    PROG_CARRY_M = D(1260000)        # extended programme management and site establishment, per month
    BOARDINGS, FARE = D(45000), D("1.40")     # boardings a day; average fare, USD
    POSS_HOURS, DAY_HOURS = D("2.5"), D("10.0")   # productive hours: night possession vs day shift
    POSS_NIGHTS_YEAR = D(200)        # 4 possession nights a week x 50 weeks
    TEAM_NOW, TEAM_DOUBLED = D(60), D(120)    # verification engineers
    LINK_COST, WORK_WEEK = D("0.5"), D(40)    # D12's calibrated link cost c and hours h
    ENG_DAY_RATE = D(1450)
    OUTSIDE_SAFETY = D(18)           # of the deferral candidates, those outside the safety boundary

    # ---- G.2.1 the count, and the architecture ----
    POSS_BASE, POSS_GROWN = D(mesh(N_BASE)), D(mesh(N_GROWN))
    check("G.2.1 possible pairwise interfaces at 16 subsystems", POSS_BASE, D(120))
    check("G.2.1 possible pairwise interfaces at 19 subsystems", POSS_GROWN, D(171))
    check("G.2.1 possible interfaces added by three subsystems", POSS_GROWN - POSS_BASE, D(51))
    # each arrival adds one more pair than the last, so 17 is the mean and none of the three
    check("G.2.1 possible interfaces added by the 17th subsystem", D(mesh(17)) - D(mesh(16)), D(16))
    check("G.2.1 possible interfaces added by the 18th subsystem", D(mesh(18)) - D(mesh(17)), D(17))
    check("G.2.1 possible interfaces added by the 19th subsystem", D(mesh(19)) - D(mesh(18)), D(18))
    check("G.2.1 INVARIANT the three increments sum to the interfaces added",
          D(mesh(17)) - D(mesh(16)) + D(mesh(18)) - D(mesh(17)) + D(mesh(19)) - D(mesh(18)),
          POSS_GROWN - POSS_BASE)
    check("G.2.1 mean possible interfaces added per subsystem", (POSS_GROWN - POSS_BASE) / 3, D(17))
    check("G.2.1 register density at baseline (%)", REG_BASE / POSS_BASE * 100, D("66.6667"),
          tol=D("0.00005"))
    check("G.2.1 register density after growth (%)", REG_GROWN / POSS_GROWN * 100, D("73.6842"),
          tol=D("0.00005"))
    check("G.2.1 INVARIANT the register is bounded by the pairwise count, baseline",
          D(1) if REG_BASE < POSS_BASE else D(0), D(1))
    check("G.2.1 INVARIANT the register is bounded by the pairwise count, after growth",
          D(1) if REG_GROWN < POSS_GROWN else D(0), D(1))
    check("G.2.1 required interfaces added by three subsystems", REG_GROWN - REG_BASE, D(46))

    BIL_BASE = REG_BASE * IF_COST
    LAY_BASE = D(N_BASE) * IF_COST + LAYER_COST
    check("G.2.1 bilateral interface cost at baseline", BIL_BASE, D(11600000))
    check("G.2.1 layered interface cost at baseline", LAY_BASE, D(8080000))
    check("G.2.1 layered saving at baseline", BIL_BASE - LAY_BASE, D(3520000))
    check("G.2.1 breakeven layer cost at baseline", BIL_BASE - D(N_BASE) * IF_COST, D(9280000))
    BIL_GROWN = REG_GROWN * IF_COST
    LAY_GROWN = D(N_GROWN) * IF_COST + LAYER_COST
    check("G.2.1 bilateral interface cost after growth", BIL_GROWN, D(18270000))
    check("G.2.1 layered interface cost after growth", LAY_GROWN, D(8515000))
    check("G.2.1 layered saving after growth", BIL_GROWN - LAY_GROWN, D(9755000))
    check("G.2.1 cost of the growth, bilateral", BIL_GROWN - BIL_BASE, D(6670000))
    check("G.2.1 cost of the growth, layered", LAY_GROWN - LAY_BASE, D(435000))
    check("G.2.1 ratio of the two costs of growth",
          (BIL_GROWN - BIL_BASE) / (LAY_GROWN - LAY_BASE), D("15.3333"), tol=D("0.00005"))
    # the unit cost cancels: the ratio is the ratio of the interface counts the growth becomes
    check("G.2.1 INVARIANT the cost ratio is the interface-count ratio, 46 to 3",
          (BIL_GROWN - BIL_BASE) / (LAY_GROWN - LAY_BASE),
          (REG_GROWN - REG_BASE) / (D(N_GROWN) - D(N_BASE)), tol=D("0.0000001"))
    check("G.2.1 INVARIANT the layered architecture is cheaper at both scopes",
          D(1) if (BIL_BASE > LAY_BASE and BIL_GROWN > LAY_GROWN) else D(0), D(1))
    check("G.2.1 baseline interface work as a share of capital expenditure (%)",
          BIL_BASE / CAPEX * 100, D("0.4028"), tol=D("0.00005"))
    check("G.2.1 the integration layer as a share of capital expenditure (%)",
          LAYER_COST / CAPEX * 100, D("0.2000"), tol=D("0.00005"))
    check("G.2.1 the integration layer as one part in N of capital expenditure", CAPEX / LAYER_COST,
          D(500))

    # ---- G.2.2 verification duration, and the date ----
    CAP_MONTH = STREAMS * TEST_DAYS_MONTH
    check("G.2.2 witnessed test-days available a month", CAP_MONTH, D(100))
    check("G.2.2 bilateral interfaces verifiable a month", CAP_MONTH / TD_BILATERAL, D(10))
    VER_BASE = REG_BASE * TD_BILATERAL / CAP_MONTH
    VER_GROWN = REG_GROWN * TD_BILATERAL / CAP_MONTH
    VER_LAYER = D(N_GROWN) * TD_LAYERED / CAP_MONTH
    check("G.2.2 witnessed test-days, baseline bilateral", REG_BASE * TD_BILATERAL, D(800))
    check("G.2.2 witnessed test-days, grown bilateral", REG_GROWN * TD_BILATERAL, D(1260))
    check("G.2.2 witnessed test-days, grown layered", D(N_GROWN) * TD_LAYERED, D(380))
    check("G.2.2 months of critical path consumed by one bilateral interface",
          TD_BILATERAL / CAP_MONTH, D("0.10"))
    check("G.2.2 verification campaign, baseline bilateral (months)", VER_BASE, D("8.00"))
    check("G.2.2 verification campaign, grown bilateral (months)", VER_GROWN, D("12.60"))
    check("G.2.2 verification campaign, grown layered (months)", VER_LAYER, D("3.80"))
    check("G.2.2 verification months added by the growth", VER_GROWN - VER_BASE, D("4.60"))
    FIXED_PATH = CIVILS + INSTALL + TRIAL + AUTHORISE
    check("G.2.2 critical path outside verification (months)", FIXED_PATH, D(53))
    CP_BASE, CP_GROWN, CP_LAYER = FIXED_PATH + VER_BASE, FIXED_PATH + VER_GROWN, FIXED_PATH + VER_LAYER
    check("G.2.2 critical path, baseline (months)", CP_BASE, D("61.00"))
    check("G.2.2 float against the committed opening, baseline (months)", FIXED_DATE - CP_BASE,
          D("2.00"))
    check("G.2.2 critical path, after growth (months)", CP_GROWN, D("65.60"))
    OVERRUN = CP_GROWN - FIXED_DATE
    check("G.2.2 overrun against the committed opening (months)", OVERRUN, D("2.60"))
    check("G.2.2 critical path, after growth on a layered architecture (months)", CP_LAYER,
          D("56.80"))
    check("G.2.2 float on a layered architecture (months)", FIXED_DATE - CP_LAYER, D("6.20"))
    # the same advantage two ways: as a verification saving, and as a change in float
    check("G.2.2 layered advantage as a verification saving (months)", VER_BASE - VER_LAYER,
          D("4.20"))
    check("G.2.2 INVARIANT the advantage is the same read as float",
          (FIXED_DATE - CP_LAYER) - (FIXED_DATE - CP_BASE), VER_BASE - VER_LAYER)
    check("G.2.2 INVARIANT the grown bilateral path breaches the date and the layered one does not",
          D(1) if (CP_GROWN > FIXED_DATE and CP_LAYER < FIXED_DATE) else D(0), D(1))

    # ---- G.2.3 the price of the date ----
    COD_MONTH = FARE_REVENUE_M + PROG_CARRY_M
    check("G.2.3 deferred farebox revenue a month", BOARDINGS * 30 * FARE, FARE_REVENUE_M)
    check("G.2.3 cost of delay a month", COD_MONTH, D(3150000))
    check("G.2.3 cost of delay a week (30-day month, 7-day week)", COD_MONTH * 7 / 30, D(735000))
    check("G.2.3 cost of delay against Meridian's, per week",
          COD_MONTH * 7 / 30 / ctx["MERIDIAN_COST_OF_DELAY"], D("51.4706"), tol=D("0.00005"))
    check("G.2.3 delay cost of the overrun", OVERRUN * COD_MONTH, D(8190000))
    DESCOPE = OVERRUN * CAP_MONTH / TD_BILATERAL
    check("G.2.3 interfaces that must leave the opening scope", DESCOPE, D(26))
    check("G.2.3 INVARIANT the descope equals the growth it repays, interface for interface",
          DESCOPE, (VER_GROWN - VER_BASE - (FIXED_DATE - CP_BASE)) * CAP_MONTH / TD_BILATERAL)
    COD_IF = TD_BILATERAL / CAP_MONTH * COD_MONTH
    check("G.2.3 delay cost of one deferred interface", COD_IF, D(315000))
    PROD_FACTOR = POSS_HOURS / DAY_HOURS
    check("G.2.3 possession productivity factor", PROD_FACTOR, D("0.25"))
    check("G.2.3 possession multiplier on day-shift effort", 1 / PROD_FACTOR, D(4))
    NIGHTS = DESCOPE * TD_BILATERAL / PROD_FACTOR
    check("G.2.3 possession nights to verify the deferred interfaces", NIGHTS, D(1040))
    check("G.2.3 elapsed years of possession working", NIGHTS / POSS_NIGHTS_YEAR, D("5.20"))
    IN_PROGRAMME = DESCOPE * IF_COST
    IN_POSSESSION = DESCOPE * IF_COST / PROD_FACTOR
    check("G.2.3 deferred interfaces delivered inside the programme", IN_PROGRAMME, D(3770000))
    check("G.2.3 the same interfaces delivered in possessions", IN_POSSESSION, D(15080000))
    check("G.2.3 extra cost of deferring rather than delivering", IN_POSSESSION - IN_PROGRAMME,
          D(11310000))
    check("G.2.3 extra cost per deferred interface", IF_COST / PROD_FACTOR - IF_COST, D(435000))
    # THE CENTRAL FINDING: holding the date costs more than moving it, and the ratio does not
    # depend on how many interfaces are deferred.
    RATIO_UNIT = (IF_COST / PROD_FACTOR - IF_COST) / COD_IF
    check("G.2.3 cost of holding the date against the cost of moving it, per interface",
          RATIO_UNIT, D("1.3810"), tol=D("0.00005"))
    check("G.2.3 INVARIANT the ratio is the same computed on the totals",
          (IN_POSSESSION - IN_PROGRAMME) / (OVERRUN * COD_MONTH), RATIO_UNIT)
    check("G.2.3 INVARIANT holding the date costs more than moving it",
          D(1) if RATIO_UNIT > 1 else D(0), D(1))
    # what unpriced consequences of a slipped date would have to be worth for holding it to pay
    check("G.2.3 net extra cost of holding the date",
          (IN_POSSESSION - IN_PROGRAMME) - OVERRUN * COD_MONTH, D(3120000))

    # ---- G.2.4 the compression that is not available ----
    LINKS_NOW = TEAM_NOW * (TEAM_NOW - 1) / 2
    LINKS_DOUBLED = TEAM_DOUBLED * (TEAM_DOUBLED - 1) / 2
    # printed against the manuscript's own figures, not against a second call to mesh(): comparing
    # `TEAM*(TEAM-1)/2` to `mesh(TEAM)` is the same formula twice and can never fail.
    check("G.2.4 communication paths in the verification team of 60", LINKS_NOW, D(1770))
    check("G.2.4 communication paths at 120", LINKS_DOUBLED, D(7140))
    check("G.2.4 paths multiply when the team doubles", LINKS_DOUBLED / LINKS_NOW, D("4.0339"),
          tol=D("0.00005"))
    SHARE_NOW = LINK_COST * (TEAM_NOW - 1) / (2 * WORK_WEEK)
    SHARE_DOUBLED = LINK_COST * (TEAM_DOUBLED - 1) / (2 * WORK_WEEK)
    check("G.2.4 coordination share of capacity at 60 (%)", SHARE_NOW * 100, D("36.8750"),
          tol=D("0.00005"))
    check("G.2.4 coordination share of capacity at 120 (%)", SHARE_DOUBLED * 100, D("74.3750"),
          tol=D("0.00005"))
    NET_NOW = WORK_WEEK * TEAM_NOW - LINK_COST * LINKS_NOW
    NET_DOUBLED = WORK_WEEK * TEAM_DOUBLED - LINK_COST * LINKS_DOUBLED
    check("G.2.4 net team capacity at 60 (hours a week)", NET_NOW, D(1515))
    check("G.2.4 net team capacity at 120 (hours a week)", NET_DOUBLED, D(1230))
    check("G.2.4 net capacity lost by doubling the team (hours a week)", NET_NOW - NET_DOUBLED,
          D(285))
    check("G.2.4 net capacity lost by doubling the team (%)",
          (NET_NOW - NET_DOUBLED) / NET_NOW * 100, D("18.8119"), tol=D("0.00005"))
    check("G.2.4 INVARIANT doubling the team reduces net capacity",
          D(1) if NET_DOUBLED < NET_NOW else D(0), D(1))
    def net(n):
        return WORK_WEEK * D(n) - LINK_COST * D(mesh(n))

    # the peak is a TIE at 80 and 81 — the continuous optimum is (h + c/2)/c = 80.5 — which is how
    # Domain 12 records it; "peaks at 81" would imply a uniqueness the arithmetic does not have
    NET_PEAK = net(81)
    check("G.2.4 net capacity at a team of 80 (hours a week)", net(80), D(1620))
    check("G.2.4 net capacity at a team of 81 (hours a week)", NET_PEAK, D(1620))
    check("G.2.4 INVARIANT the peak net capacity is a tie at 80 and 81", net(80), net(81))
    check("G.2.4 INVARIANT 82 is past the peak", D(1) if net(82) < NET_PEAK else D(0), D(1))
    check("G.2.4 continuous optimum team size", (WORK_WEEK + LINK_COST / 2) / LINK_COST, D("80.5"))
    check("G.2.4 INVARIANT neither team size reaches the peak net capacity",
          D(1) if (NET_NOW < NET_PEAK and NET_DOUBLED < NET_PEAK) else D(0), D(1))
    check("G.2.4 net capacity forgone at 120 against the peak (hours a week)",
          NET_PEAK - NET_DOUBLED, D(390))
    check("G.2.4 cost of the sixty additional engineers over the overrun",
          (TEAM_DOUBLED - TEAM_NOW) * ENG_DAY_RATE * TEST_DAYS_MONTH * OVERRUN, D(4524000))

    # ---- G.2.5 what the descope can actually buy ----
    BOUGHT = OUTSIDE_SAFETY * TD_BILATERAL / CAP_MONTH
    check("G.2.5 months bought by deferring the interfaces outside the safety boundary", BOUGHT,
          D("1.80"))
    check("G.2.5 residual overrun with nowhere to go (months)", OVERRUN - BOUGHT, D("0.80"))
    check("G.2.5 residual overrun priced", (OVERRUN - BOUGHT) * COD_MONTH, D(2520000))
    check("G.2.5 INVARIANT the descope cannot repay the whole overrun",
          D(1) if BOUGHT < OVERRUN else D(0), D(1))
    # the central ratio has to hold on the descope the programme may ACTUALLY take, not only on the
    # hypothetical 26, or it is an artefact of the round number
    check("G.2.5 possession cost of the 18 the programme may defer",
          OUTSIDE_SAFETY * (IF_COST / PROD_FACTOR - IF_COST), D(7830000))
    check("G.2.5 delay the same 18 avoid", BOUGHT * COD_MONTH, D(5670000))
    check("G.2.5 INVARIANT the 1.3810 is unchanged on the deferrable 18",
          OUTSIDE_SAFETY * (IF_COST / PROD_FACTOR - IF_COST) / (BOUGHT * COD_MONTH), RATIO_UNIT,
          tol=D("0.0000001"))
    check("G.2.5 interfaces inside the safety boundary", DESCOPE - OUTSIDE_SAFETY, D(8))
    # `D(2)/D(5)*100 == 40.0` stood here: no case quantity enters it, so it asserted only that two
    # fifths is 40 per cent. The scope lever's real extent is what the count of five was standing for.
    check("G.2.5 share of the descope the programme is actually free to take (%)",
          OUTSIDE_SAFETY / DESCOPE * 100, D("69.2308"), tol=D("0.00005"))

    # =====================================================================================
    # G.3  SABLE PLAINS ENERGY — the pipeline is the unit of management
    # =====================================================================================
    # ---- inputs ----
    GATE_PASS = [D("0.72"), D("0.65"), D("0.55"), D("0.80"), D("0.85")]   # five development gates
    GATE_COST = [D(40000), D(120000), D(260000), D(640000), D(400000)]    # cost of each gate stage
    GATE_YEARS = [D("0.25"), D("0.50"), D("1.25"), D("1.75"), D("0.75")]  # elapsed years per stage
    BUILD_RATE = D(4)                # measured energisations a year (12 in three years)
    TECH_YEARS = D(1)                # technical build duration with the resource a build needs
    MW, BUILD_CAPEX = D(50), D(60000000)      # per build
    CONTRIB = D(7200000)             # annual net contribution per build, USD
    CAP_FACTOR, HOURS_YEAR = D("0.35"), D(8760)
    RATE, LIFE = D("0.08"), 25       # discount rate; contracted operating life, years
    OFFERS, OFFER_DEPOSIT = D(12), D(1400000)   # connection offers maturing together; deposit each
    DEV_WIP_RESOURCED = D(24)        # prospects the development team can carry at once
    DEV_QUEUE = D(24)                # prospects waiting for a development slot
    PROSPECTS_PER_MANAGER = D(3)
    MANAGER_COST = D(240000)         # loaded annual cost of a development manager
    OPTION_LIFE = D(3)               # land option life, years
    PIPELINE = D(48)

    # ---- G.3.1 the funnel ----
    CONV = D(1)
    for p in GATE_PASS:
        CONV *= p
    check("G.3.1 pipeline conversion, origination to energisation (%)", CONV * 100, D("17.5032"),
          tol=D("0.00005"))
    GATE_MEAN = sum(GATE_PASS) / 5
    check("G.3.1 arithmetic mean of the five gate pass rates (%)", GATE_MEAN * 100, D("71.4000"),
          tol=D("0.00005"))
    check("G.3.1 gap between the mean and the conjunction (points)", (GATE_MEAN - CONV) * 100,
          D("53.8968"), tol=D("0.00005"))
    check("G.3.1 INVARIANT the mean overstates the conjunction",
          D(1) if GATE_MEAN > CONV else D(0), D(1))
    GEO = (CONV.ln() / 5).exp()
    # printed to seven places, not six: 0.705705 to the fifth is 17.5031 %, so the manuscript's
    # round-trip claim needed the extra digit to be true on the digits it shows
    check("G.3.1 geometric mean gate pass rate", GEO, D("0.7057055"), tol=D("0.00000005"))
    check("G.3.1 INVARIANT the geometric mean reproduces the conjunction", GEO ** 5, CONV,
          tol=D("0.0000001"))
    # deliberately constant-only: it tests the manuscript's DISPLAYED digits, which is the claim the
    # manuscript makes about itself. At six places (0.705705) it fails, returning 17.5031 %.
    check("G.3.1 INVARIANT the PRINTED geometric mean round-trips to the printed conversion (%)",
          D("0.7057055") ** 5 * 100, D("17.5032"), tol=D("0.00005"))
    ORIGINATIONS = BUILD_RATE / CONV
    check("G.3.1 originations a year needed for four builds", ORIGINATIONS, D("22.8530"),
          tol=D("0.00005"))
    # survival into each stage, and entrants per built project
    SURVIVE = [D(1)]
    for p in GATE_PASS[:-1]:
        SURVIVE.append(SURVIVE[-1] * p)
    check("G.3.1 survival into the grid-offer stage", SURVIVE[2], D("0.468"))
    check("G.3.1 survival into the consent stage", SURVIVE[3], D("0.2574"))
    check("G.3.1 survival into the offtake stage", SURVIVE[4], D("0.20592"))
    ENTRANTS = [s / CONV for s in SURVIVE]
    check("G.3.1 prospects screened per built project", ENTRANTS[0], D("5.7132"), tol=D("0.00005"))
    check("G.3.1 prospects taking a land option per built project", ENTRANTS[1], D("4.1135"),
          tol=D("0.00005"))
    check("G.3.1 prospects reaching the grid-offer stage per built project", ENTRANTS[2],
          D("2.6738"), tol=D("0.00005"))
    check("G.3.1 prospects reaching offtake and investment decision per built project",
          ENTRANTS[4], D("1.1765"), tol=D("0.00005"))
    check("G.3.1 prospects reaching consent per built project", ENTRANTS[3], D("1.4706"),
          tol=D("0.00005"))
    check("G.3.1 INVARIANT entrants to a stage are the reciprocal of the downstream conjunction",
          ENTRANTS[3], 1 / (GATE_PASS[3] * GATE_PASS[4]), tol=D("0.0000001"))
    DEV_PER_BUILT = sum(e * c for e, c in zip(ENTRANTS, GATE_COST))
    OWN_PATH = sum(GATE_COST)
    check("G.3.1 a closing project's own development path", OWN_PATH, D(1460000))
    check("G.3.1 development cost per BUILT project, portfolio basis", DEV_PER_BUILT,
          D("2829105.53"), tol=D("0.05"))
    check("G.3.1 understatement multiple of the project charge code", DEV_PER_BUILT / OWN_PATH,
          D("1.9377"), tol=D("0.00005"))
    check("G.3.1 development spend on prospects that never build, per built project",
          DEV_PER_BUILT - OWN_PATH, D("1369105.53"), tol=D("0.05"))
    check("G.3.1 that spend as a share of portfolio development cost (%)",
          (DEV_PER_BUILT - OWN_PATH) / DEV_PER_BUILT * 100, D("48.3936"), tol=D("0.00005"))
    check("G.3.1 unabsorbed development cost a year at four builds",
          (DEV_PER_BUILT - OWN_PATH) * BUILD_RATE, D("5476422.14"), tol=D("0.05"))
    check("G.3.1 unabsorbed development cost per megawatt", (DEV_PER_BUILT - OWN_PATH) / MW,
          D("27382.11"), tol=D("0.005"))
    check("G.3.1 INVARIANT the portfolio cost exceeds the project's own path",
          D(1) if DEV_PER_BUILT > OWN_PATH else D(0), D(1))
    check("G.3.1 pipeline nameplate capacity (MW)", PIPELINE * MW, D(2400))

    # ---- G.3.2 twelve good projects, one bad sequencing decision ----
    AF25 = (1 - (1 + RATE) ** -LIFE) / RATE
    check("G.3.2 AF(0.08,25)", AF25, D("10.674776"), tol=D("0.0000005"))
    check("G.3.2 capital cost per megawatt", BUILD_CAPEX / MW, D(1200000))
    check("G.3.2 annual generation per build (MWh)", MW * HOURS_YEAR * CAP_FACTOR, D(153300))
    check("G.3.2 net contribution per megawatt-hour", CONTRIB / (MW * HOURS_YEAR * CAP_FACTOR),
          D("46.9667"), tol=D("0.00005"))
    PV_CONTRIB = CONTRIB * AF25
    check("G.3.2 present value of contribution at energisation", PV_CONTRIB, D("76858388.56"),
          tol=D("0.05"))
    # a one-year build: capex drawn at t=1, contribution years 2..26
    check("G.3.2 present value of capital drawn at year one", BUILD_CAPEX / (1 + RATE),
          D("55555555.56"), tol=D("0.05"))
    check("G.3.2 present value of contribution from year two",
          PV_CONTRIB / (1 + RATE), D("71165174.59"), tol=D("0.05"))
    NPV_FAST = (PV_CONTRIB - BUILD_CAPEX) / (1 + RATE)
    check("G.3.2 NPV of a one-year build", NPV_FAST, D("15609619.04"), tol=D("0.05"))
    check("G.3.2 INVARIANT the one-year NPV is the same netted before discounting",
          PV_CONTRIB / (1 + RATE) - BUILD_CAPEX / (1 + RATE), NPV_FAST, tol=D("0.0000001"))
    # Little's Law at portfolio level: the work in progress a one-year build implies, and the cycle
    # time twelve in flight produce against the same measured throughput (D13 KA 13.2.3, D15 KA 15.3.2)
    COHORT = BUILD_RATE * TECH_YEARS       # work in progress a one-year build is consistent with
    check("G.3.2 work in progress consistent with a one-year build", COHORT, D(4))
    check("G.3.2 cycle time with twelve in flight (years)", OFFERS / BUILD_RATE, D("3.00"))
    check("G.3.2 excess cycle time bought by the concurrency policy (years)",
          OFFERS / BUILD_RATE - TECH_YEARS, D("2.00"))
    # a three-year build: capex in three equal instalments, contribution years 4..28
    check("G.3.2 annual capital instalment on a three-year build", BUILD_CAPEX / 3, D(20000000))
    AF3 = (1 - (1 + RATE) ** -3) / RATE
    check("G.3.2 AF(0.08,3)", AF3, D("2.577097"), tol=D("0.0000005"))
    PV_CAPEX_SLOW = BUILD_CAPEX / 3 * AF3
    check("G.3.2 present value of capital drawn over three years", PV_CAPEX_SLOW,
          D("51541939.74"), tol=D("0.05"))
    check("G.3.2 present value of contribution deferred to year three",
          PV_CONTRIB / (1 + RATE) ** 3, D("61012666.83"), tol=D("0.05"))
    NPV_SLOW = PV_CONTRIB / (1 + RATE) ** 3 - PV_CAPEX_SLOW
    check("G.3.2 NPV of a three-year build", NPV_SLOW, D("9470727.09"), tol=D("0.05"))
    check("G.3.2 NPV lost per build to stretched cycle time", NPV_FAST - NPV_SLOW,
          D("6138891.95"), tol=D("0.05"))
    check("G.3.2 NPV lost per build (%)", (NPV_FAST - NPV_SLOW) / NPV_FAST * 100, D("39.3276"),
          tol=D("0.00005"))
    check("G.3.2 INVARIANT nothing about the project changed but its duration",
          D(1) if NPV_SLOW < NPV_FAST else D(0), D(1))
    # the twelve offers: all at once against three cohorts of four
    PV_PARALLEL = OFFERS * NPV_SLOW
    check("G.3.2 twelve builds run together, present value", PV_PARALLEL, D("113648725.02"),
          tol=D("0.5"))
    COHORTS = OFFERS / COHORT              # cohorts of four the twelve offers make
    check("G.3.2 cohorts the twelve offers make at a cohort size of four", COHORTS, D(3))
    DEFER = sum(1 / (1 + RATE) ** k for k in range(int(COHORTS)))
    check("G.3.2 three-cohort deferral factor", DEFER, D("2.783265"), tol=D("0.0000005"))
    PV_COHORTS = COHORT * NPV_FAST * DEFER
    check("G.3.2 three cohorts of four, present value", PV_COHORTS, D("173782809.45"),
          tol=D("0.5"))
    check("G.3.2 value released by sequencing the same twelve builds",
          PV_COHORTS - PV_PARALLEL, D("60134084.43"), tol=D("0.5"))
    check("G.3.2 uplift from sequencing (%)", (PV_COHORTS - PV_PARALLEL) / PV_PARALLEL * 100,
          D("52.9122"), tol=D("0.00005"))
    check("G.3.2 average present value per build, sequenced", PV_COHORTS / OFFERS,
          D("14481900.79"), tol=D("0.05"))
    check("G.3.2 sequenced against simultaneous, per build",
          (PV_COHORTS / OFFERS) / NPV_SLOW, D("1.5291"), tol=D("0.00005"))
    # the 1.5291 is a RATIO of values per build, so the gain per build is 0.5291 of a simultaneous
    # build and not 1.5291 of anything — the manuscript said the latter and now says this
    check("G.3.2 gain per build from sequencing", (PV_COHORTS - PV_PARALLEL) / OFFERS,
          D("5011173.70"), tol=D("0.05"))
    check("G.3.2 gain per build as a fraction of a simultaneous build",
          (PV_COHORTS - PV_PARALLEL) / OFFERS / NPV_SLOW, D("0.5291"), tol=D("0.00005"))
    check("G.3.2 INVARIANT the gain per build is the ratio less one, times the simultaneous build",
          (PV_COHORTS - PV_PARALLEL) / OFFERS,
          ((PV_COHORTS / OFFERS) / NPV_SLOW - 1) * NPV_SLOW, tol=D("0.0000001"))
    check("G.3.2 INVARIANT sequencing beats simultaneity on identical projects",
          D(1) if PV_COHORTS > PV_PARALLEL else D(0), D(1))
    # the two invariants that stop this being a free lunch: the sequenced plan uses no more
    # capacity and finishes no earlier than the simultaneous one
    check("G.3.2 INVARIANT capacity available over three years equals the twelve offers",
          BUILD_RATE * 3 * TECH_YEARS, OFFERS * TECH_YEARS)
    check("G.3.2 INVARIANT the last build energises in the same year under both policies",
          (COHORTS - 1) + TECH_YEARS, OFFERS / BUILD_RATE)
    check("G.3.2 year the last build energises under either policy", OFFERS / BUILD_RATE, D(3))
    check("G.3.2 builds that finish earlier under sequencing", OFFERS - COHORT, D(8))
    check("G.3.2 breakeven cost of re-timing one connection offer",
          (PV_COHORTS - PV_PARALLEL) / (OFFERS - COHORT), D("7516760.55"), tol=D("0.5"))
    check("G.3.2 breakeven against the deposit at risk",
          (PV_COHORTS - PV_PARALLEL) / (OFFERS - COHORT) / OFFER_DEPOSIT, D("5.3691"),
          tol=D("0.00005"))
    # the steady-state reading of the same fact
    check("G.3.2 value forgone a year at four builds", (NPV_FAST - NPV_SLOW) * BUILD_RATE,
          D("24555567.80"), tol=D("0.5"))
    check("G.3.2 the same flow capitalised at the discount rate",
          (NPV_FAST - NPV_SLOW) * BUILD_RATE / RATE, D("306944597.50"), tol=D("5"))
    check("G.3.2 annual capital deployed at four builds", BUILD_CAPEX * BUILD_RATE, D(240000000))
    check("G.3.2 value forgone as a share of annual capital deployed (%)",
          (NPV_FAST - NPV_SLOW) * BUILD_RATE / (BUILD_CAPEX * BUILD_RATE) * 100, D("10.2315"),
          tol=D("0.00005"))
    # balance-sheet cross-check on a different convention (uniform draw): average capital in build
    check("G.3.2 average capital in construction at four in flight", D(4) * BUILD_CAPEX / 2,
          D(120000000))
    check("G.3.2 average capital in construction at twelve in flight", OFFERS * BUILD_CAPEX / 2,
          D(360000000))
    check("G.3.2 extra capital tied up by the concurrency policy",
          (OFFERS - D(4)) * BUILD_CAPEX / 2, D(240000000))
    check("G.3.2 carry on that capital at the discount rate",
          (OFFERS - COHORT) * BUILD_CAPEX / 2 * RATE, D(19200000))
    # the two conventions are NOT a reconciliation: the gap between them is unbridged, and the
    # manuscript now says so rather than claiming the agreement of a basis bridge
    check("G.3.2 gap between the two conventions a year",
          (NPV_FAST - NPV_SLOW) * BUILD_RATE - (OFFERS - COHORT) * BUILD_CAPEX / 2 * RATE,
          D("5355567.80"), tol=D("0.5"))
    check("G.3.2 INVARIANT the two conventions are within a factor of two of each other",
          D(1) if (NPV_FAST - NPV_SLOW) * BUILD_RATE
          < 2 * (OFFERS - COHORT) * BUILD_CAPEX / 2 * RATE else D(0), D(1))

    # ---- G.3.3 the two queues ----
    TIS = sum(s * y for s, y in zip(SURVIVE, GATE_YEARS))
    check("G.3.3 expected development time per prospect originated (years)", TIS, D("1.79989"))
    DEV_WIP_NEEDED = ORIGINATIONS * TIS
    check("G.3.3 development work in progress needed to feed four builds", DEV_WIP_NEEDED,
          D("41.1328"), tol=D("0.00005"))
    DEV_THROUGHPUT = DEV_WIP_RESOURCED / TIS
    check("G.3.3 prospects completing development a year as resourced", DEV_THROUGHPUT,
          D("13.3341"), tol=D("0.00005"))
    BUILDS_FED = DEV_THROUGHPUT * CONV
    check("G.3.3 builds a year the development pipeline can feed", BUILDS_FED, D("2.3339"),
          tol=D("0.00005"))
    check("G.3.3 INVARIANT development, not construction, is the binding constraint",
          D(1) if BUILDS_FED < BUILD_RATE else D(0), D(1))
    SHORTFALL = BUILD_RATE - BUILDS_FED
    check("G.3.3 builds a year the build capacity cannot be given", SHORTFALL, D("1.6661"),
          tol=D("0.00005"))
    check("G.3.3 megawatts a year not built", SHORTFALL * MW, D("83.3049"), tol=D("0.00005"))
    check("G.3.3 INVARIANT the shortfall exceeds two fifths of build capacity (G.3 opening claim)",
          D(1) if SHORTFALL / BUILD_RATE > D("0.40") else D(0), D(1))
    check("G.3.3 value a year the constraint forgoes", SHORTFALL * NPV_FAST, D("26007145.26"),
          tol=D("0.5"))
    EXTRA_WIP = DEV_WIP_NEEDED - DEV_WIP_RESOURCED
    check("G.3.3 additional prospects that must be in development", EXTRA_WIP, D("17.1328"),
          tol=D("0.00005"))
    check("G.3.3 required increase in development capacity (%)",
          EXTRA_WIP / DEV_WIP_RESOURCED * 100, D("71.3868"), tol=D("0.00005"))
    EXTRA_MANAGERS = EXTRA_WIP / PROSPECTS_PER_MANAGER
    check("G.3.3 additional development managers", EXTRA_MANAGERS, D("5.7109"), tol=D("0.00005"))
    EXTRA_COST = EXTRA_MANAGERS * MANAGER_COST
    check("G.3.3 cost of the additional development capacity a year", EXTRA_COST,
          D("1370625.71"), tol=D("0.05"))
    check("G.3.3 breakeven additional builds a year", EXTRA_COST / NPV_FAST, D("0.0878"),
          tol=D("0.00005"))
    check("G.3.3 breakeven stated as one additional build every N years",
          NPV_FAST / EXTRA_COST, D("11.3887"), tol=D("0.00005"))
    check("G.3.3 INVARIANT the breakeven is far below the shortfall",
          D(1) if EXTRA_COST / NPV_FAST < SHORTFALL else D(0), D(1))
    QUEUE_WAIT = DEV_QUEUE / DEV_THROUGHPUT
    check("G.3.3 wait for a development slot (years)", QUEUE_WAIT, D("1.7999"), tol=D("0.00005"))
    TOTAL_TIS = QUEUE_WAIT + TIS
    check("G.3.3 average time in the pipeline, queue and development (years)", TOTAL_TIS,
          D("3.5998"), tol=D("0.00005"))
    # the average is a mix: each prospect that leaves costs and lasts less than one that closes,
    # which is what licenses calling the 3.5998 an average and not a duration
    check("G.3.3 INVARIANT a prospect that leaves costs less than one that closes",
          D(1) if (DEV_PER_BUILT - OWN_PATH) / (ENTRANTS[0] - 1) < OWN_PATH else D(0), D(1))
    check("G.3.3 INVARIANT a prospect that leaves also leaves sooner than a survivor closes",
          D(1) if (TIS - CONV * sum(GATE_YEARS)) / (1 - CONV) < sum(GATE_YEARS) else D(0), D(1))
    # TIS is survival-weighted, so it is an average over prospects that mostly die: it is NOT the
    # time a prospect that reaches a decision takes, and the land option belongs to the survivors.
    SURVIVOR_DEV = sum(GATE_YEARS)
    OPTION_AT = GATE_YEARS[0] + GATE_YEARS[1]
    OPTION_SPAN = SURVIVOR_DEV - OPTION_AT
    check("G.3.3 active development of a prospect that passes all five gates (years)", SURVIVOR_DEV,
          D("4.50"))
    check("G.3.3 point at which the land option is secured (years in)", OPTION_AT, D("0.75"))
    check("G.3.3 span the option must cover, option to decision (years)", OPTION_SPAN, D("3.75"))
    check("G.3.3 the option expires this long before the decision (years)",
          OPTION_SPAN - OPTION_LIFE, D("0.75"))
    check("G.3.3 INVARIANT every prospect reaching a decision outlives its land option",
          D(1) if OPTION_SPAN > OPTION_LIFE else D(0), D(1))
    check("G.3.3 INVARIANT the survivor's exposure exceeds the average time in the pipeline",
          D(1) if QUEUE_WAIT + SURVIVOR_DEV > TOTAL_TIS else D(0), D(1))
    check("G.3.3 INVARIANT the queue wait equals the average development time only because the "
          "queue is the resourced caseload", QUEUE_WAIT, TIS, tol=D("0.0000001"))

    # =====================================================================================
    # G.4  THE NATIONAL ENTITLEMENT SERVICE — decision rights, counted
    # =====================================================================================
    # ---- inputs: six approval bodies, each (meeting interval M weeks, paper lead L weeks,
    #      first-pass approval probability q), from the programme's own decision log ----
    BODIES = [(D(4), D(2), D("0.80")),     # policy ministry
              (D(13), D(3), D("0.90")),    # funding committee
              (D(6), D(2), D("0.70")),     # national data authority
              (D(8), D(3), D("0.75")),     # regional operations council
              (D(3), D(1), D("0.85")),     # digital service standards body
              (D(10), D(2), D("0.95"))]    # accessibility and equality panel
    ASSESSMENTS = D(2600000)         # entitlement assessments a year
    UNIT_MANUAL, UNIT_DIGITAL = D("41.60"), D("12.30")
    REGIONS = D(14)                  # regional operating offices, none of them the delivery agency
    ENG_HOURS, ENG_RATE = D(180), D(95)    # readiness programme per region: hours, blended rate
    ADOPT_P = D("0.85")              # probability a given region adopts in year one
    COVERAGE_Y1 = D("0.62")          # share of national volume handled by the regions that switched
    TARGET_PASS = D("0.95")          # the best first-pass rate any of these bodies has achieved

    # ---- G.4.1 the latency of six decision rights ----
    BASE = [ew(M, L) for M, L, _ in BODIES]
    check("G.4.1 policy ministry base latency (weeks)", BASE[0], D("4.0"))
    check("G.4.1 funding committee base latency (weeks)", BASE[1], D("9.5"))
    check("G.4.1 data authority base latency (weeks)", BASE[2], D("5.0"))
    check("G.4.1 regional operations council base latency (weeks)", BASE[3], D("7.0"))
    check("G.4.1 standards body base latency (weeks)", BASE[4], D("2.5"))
    check("G.4.1 accessibility panel base latency (weeks)", BASE[5], D("7.0"))
    BASE_TOTAL = sum(BASE)
    check("G.4.1 summed base latency of the approval path (weeks)", BASE_TOTAL, D("35.0"))
    EWAIT = [ew(M, L) + (1 / p - 1) * M for M, L, p in BODIES]
    check("G.4.1 policy ministry expected latency with returns (weeks)", EWAIT[0], D("5.0000"),
          tol=D("0.00005"))
    check("G.4.1 funding committee expected latency with returns (weeks)", EWAIT[1],
          D("10.9444"), tol=D("0.00005"))
    check("G.4.1 data authority expected latency with returns (weeks)", EWAIT[2], D("7.5714"),
          tol=D("0.00005"))
    check("G.4.1 regional operations council expected latency with returns (weeks)", EWAIT[3],
          D("9.6667"), tol=D("0.00005"))
    check("G.4.1 standards body expected latency with returns (weeks)", EWAIT[4], D("3.0294"),
          tol=D("0.00005"))
    check("G.4.1 accessibility panel expected latency with returns (weeks)", EWAIT[5],
          D("7.5263"), tol=D("0.00005"))
    TOTAL = sum(EWAIT)
    check("G.4.1 expected latency of the whole approval path (weeks)", TOTAL, D("43.7383"),
          tol=D("0.00005"))
    check("G.4.1 latency added by returns (weeks)", TOTAL - BASE_TOTAL, D("8.7383"),
          tol=D("0.00005"))
    check("G.4.1 latency added by returns (%)", (TOTAL - BASE_TOTAL) / BASE_TOTAL * 100,
          D("24.9665"), tol=D("0.00005"))
    check("G.4.1 INVARIANT every body's expected latency is at least its base latency",
          D(1) if all(e >= b for e, b in zip(EWAIT, BASE)) else D(0), D(1))
    # the serial path is an ASSUMPTION: independent bodies would be a maximum, not a sum, and that
    # lever is larger than either lever G.4.2 prices — which the manuscript now states
    check("G.4.1 saving on a fully parallel path (weeks)", TOTAL - max(EWAIT), D("32.7938"),
          tol=D("0.00005"))
    FIRST_PASS = D(1)
    for _, _, p in BODIES:
        FIRST_PASS *= p
    check("G.4.1 probability a decision clears all six bodies first time (%)", FIRST_PASS * 100,
          D("30.5235"), tol=D("0.00005"))
    PASS_MEAN = sum(p for _, _, p in BODIES) / 6
    check("G.4.1 arithmetic mean first-pass rate (%)", PASS_MEAN * 100, D("82.5000"),
          tol=D("0.00005"))
    check("G.4.1 gap between the mean and the conjunction (points)",
          (PASS_MEAN - FIRST_PASS) * 100, D("51.9765"), tol=D("0.00005"))
    check("G.4.1 INVARIANT the mean overstates the conjunction here too",
          D(1) if PASS_MEAN > FIRST_PASS else D(0), D(1))
    GEO_PASS = (FIRST_PASS.ln() / 6).exp()
    # seven places again: 0.820551 to the sixth is 30.5234 %, not the 30.5235 % the manuscript prints
    check("G.4.1 geometric mean first-pass rate", GEO_PASS, D("0.8205513"), tol=D("0.00000005"))
    check("G.4.1 INVARIANT the geometric mean reproduces the conjunction", GEO_PASS ** 6,
          FIRST_PASS, tol=D("0.0000001"))
    # constant-only for the same reason as G.3.1's: at six places (0.820551) it returns 30.5234 %
    check("G.4.1 INVARIANT the PRINTED geometric mean round-trips to the printed product (%)",
          D("0.8205513") ** 6 * 100, D("30.5235"), tol=D("0.00005"))

    # ---- G.4.2 the cost of delay, and of one decision right ----
    UNIT_SAVING = UNIT_MANUAL - UNIT_DIGITAL
    ANNUAL_SAVING = ASSESSMENTS * UNIT_SAVING
    check("G.4.2 operating saving per assessment", UNIT_SAVING, D("29.30"))
    check("G.4.2 annual operating saving at full adoption", ANNUAL_SAVING, D(76180000))
    COD_WEEK = ANNUAL_SAVING / 52
    check("G.4.2 cost of delay a week", COD_WEEK, D(1465000))
    check("G.4.2 the operating-model decision, priced at its own latency", TOTAL * COD_WEEK,
          D("64076561.50"), tol=D("0.5"))
    PER_RIGHT = TOTAL / 6
    check("G.4.2 average latency per decision right (weeks)", PER_RIGHT, D("7.2897"),
          tol=D("0.00005"))
    check("G.4.2 average cost of one decision right", PER_RIGHT * COD_WEEK, D("10679426.92"),
          tol=D("0.5"))
    # the behavioural lever, at its ceiling
    PERFECT = BASE_TOTAL + sum((1 / TARGET_PASS - 1) * M for M, _, _ in BODIES)
    check("G.4.2 summed meeting intervals of the six bodies (weeks)",
          sum(M for M, _, _ in BODIES), D(44))
    check("G.4.2 return penalty at a 0.95 first-pass rate, as a fraction of one interval",
          1 / TARGET_PASS - 1, 1 / D(19), tol=D("0.0000001"))
    check("G.4.2 return penalty across all six intervals (weeks)",
          sum(M for M, _, _ in BODIES) / D(19), D("2.3158"), tol=D("0.00005"))
    check("G.4.2 latency with every body at its best observed first-pass rate (weeks)", PERFECT,
          D("37.3158"), tol=D("0.00005"))
    BEHAVIOUR = TOTAL - PERFECT
    check("G.4.2 weeks saved by perfecting every body's conduct", BEHAVIOUR, D("6.4225"),
          tol=D("0.00005"))
    check("G.4.2 that saving priced", BEHAVIOUR * COD_WEEK, D("9408929.92"), tol=D("0.5"))
    check("G.4.2 first-pass probability with every body at 0.95 (%)", TARGET_PASS ** 6 * 100,
          D("73.5092"), tol=D("0.00005"))
    # the count lever
    THREE = EWAIT[0] + EWAIT[1] + EWAIT[2]
    check("G.4.2 latency of a three-body path (weeks)", THREE, D("23.5159"), tol=D("0.00005"))
    COUNT = TOTAL - THREE
    check("G.4.2 weeks saved by halving the number of decision rights", COUNT, D("20.2224"),
          tol=D("0.00005"))
    check("G.4.2 that saving priced", COUNT * COD_WEEK, D("29625807.53"), tol=D("0.5"))
    check("G.4.2 the same decision on a three-body path, priced", THREE * COD_WEEK,
          D("34450753.97"), tol=D("0.5"))
    check("G.4.2 first-pass probability on a three-body path (%)",
          BODIES[0][2] * BODIES[1][2] * BODIES[2][2] * 100, D("50.4000"), tol=D("0.00005"))
    # THE CENTRAL FINDING: the count dominates the conduct
    check("G.4.2 the count lever against the conduct lever", COUNT / BEHAVIOUR, D("3.1487"),
          tol=D("0.00005"))
    check("G.4.2 INVARIANT the count lever exceeds the conduct lever",
          D(1) if COUNT > BEHAVIOUR else D(0), D(1))
    check("G.4.2 best observed conduct as a share of one decision right (%)",
          BEHAVIOUR / PER_RIGHT * 100, D("88.1033"), tol=D("0.00005"))
    check("G.4.2 INVARIANT the BEST OBSERVED conduct lever cannot pay for one more decision right",
          D(1) if BEHAVIOUR < PER_RIGHT else D(0), D(1))
    # the absolute ceiling on conduct is a path with no returns at all (q = 1 everywhere), which is
    # the WHOLE return penalty and does exceed one decision right: 19.87 % of headroom the manuscript
    # originally claimed did not exist
    MAX_CONDUCT = TOTAL - BASE_TOTAL
    check("G.4.2 the absolute conduct ceiling, a path with no returns at all (weeks)", MAX_CONDUCT,
          D("8.7383"), tol=D("0.00005"))
    check("G.4.2 the absolute ceiling as a share of one decision right (%)",
          MAX_CONDUCT / PER_RIGHT * 100, D("119.8712"), tol=D("0.00005"))
    check("G.4.2 INVARIANT the absolute ceiling DOES exceed one decision right, unlike the "
          "observed one", D(1) if MAX_CONDUCT > PER_RIGHT else D(0), D(1))
    check("G.4.2 the count lever against the absolute conduct ceiling", COUNT / MAX_CONDUCT,
          D("2.3142"), tol=D("0.00005"))
    check("G.4.2 INVARIANT the count lever wins against either conduct ceiling",
          D(1) if (COUNT > BEHAVIOUR and COUNT > MAX_CONDUCT) else D(0), D(1))
    check("G.4.2 INVARIANT the parallel-path lever beats the count lever on latency, and so is "
          "not claimed to be smaller", D(1) if TOTAL - max(EWAIT) > COUNT else D(0), D(1))
    # deleting one body, and adding one
    check("G.4.2 latency with the accessibility sign-off taken once, not per change (weeks)",
          TOTAL - EWAIT[5], D("36.2120"), tol=D("0.00005"))
    check("G.4.2 value of that single deletion", EWAIT[5] * COD_WEEK, D("11026052.63"),
          tol=D("0.5"))
    check("G.4.2 first-pass probability after that deletion (%)", FIRST_PASS / BODIES[5][2] * 100,
          D("32.1300"), tol=D("0.00005"))
    check("G.4.2 latency with a seventh body resembling the six (weeks)", TOTAL + PER_RIGHT,
          D("51.0280"), tol=D("0.00005"))
    check("G.4.2 first-pass probability with a seventh such body (%)",
          FIRST_PASS * GEO_PASS * 100, D("25.0461"), tol=D("0.00005"))

    # ---- G.4.3 the fourteen offices the delivery agency does not run ----
    PER_REGION = ENG_HOURS * ENG_RATE
    NATIONAL = PER_REGION * REGIONS
    check("G.4.3 readiness programme per region", PER_REGION, D(17100))
    check("G.4.3 readiness programme nationally", NATIONAL, D(239400))
    BREAKEVEN_SHARE = NATIONAL / ANNUAL_SAVING
    check("G.4.3 breakeven share of national assessment volume (%)", BREAKEVEN_SHARE * 100,
          D("0.3143"), tol=D("0.00005"))
    check("G.4.3 breakeven expressed in assessments a year", BREAKEVEN_SHARE * ASSESSMENTS,
          D("8170.6"), tol=D("0.05"))
    check("G.4.3 breakeven assessments for one region's programme", PER_REGION / UNIT_SAVING,
          D("583.62"), tol=D("0.005"))
    check("G.4.3 average assessments a region", ASSESSMENTS / REGIONS, D("185714.29"),
          tol=D("0.005"))
    check("G.4.3 INVARIANT the national and regional breakevens are the same number",
          PER_REGION / UNIT_SAVING / (ASSESSMENTS / REGIONS), BREAKEVEN_SHARE, tol=D("0.0000001"))
    check("G.4.3 probability all fourteen regions adopt in year one (%)", ADOPT_P ** 14 * 100,
          D("10.2770"), tol=D("0.00005"))
    check("G.4.3 expected regions adopting in year one", REGIONS * ADOPT_P, D("11.90"))
    check("G.4.3 honest benefit case at the expected coverage", ADOPT_P * ANNUAL_SAVING,
          D(64753000))
    OVERSTATED = ANNUAL_SAVING - ADOPT_P * ANNUAL_SAVING
    check("G.4.3 overstatement in a business case assuming universal adoption", OVERSTATED,
          D(11427000))
    check("G.4.3 INVARIANT the overstatement is the complement of the adoption probability (%)",
          OVERSTATED / ANNUAL_SAVING * 100, D("15.0000"), tol=D("0.00005"))
    check("G.4.3 saving realised at year-one coverage", COVERAGE_Y1 * ANNUAL_SAVING,
          D(47231600))
    check("G.4.3 year-one shortfall", (1 - COVERAGE_Y1) * ANNUAL_SAVING, D(28948400))
    check("G.4.3 shortfall against the whole national readiness programme",
          (1 - COVERAGE_Y1) * ANNUAL_SAVING / NATIONAL, D("120.9206"), tol=D("0.00005"))
    # the realised shortfall and the business-case overstatement are ONE flow measured twice, ex post
    # and ex ante — they are not two losses to add, which is what the manuscript now says
    check("G.4.3 INVARIANT the realised shortfall contains the forecast overstatement",
          D(1) if (1 - COVERAGE_Y1) * ANNUAL_SAVING > OVERSTATED else D(0), D(1))
    check("G.4.3 realised shortfall as a share of national volume (%)",
          (1 - COVERAGE_Y1) * 100, D("38.0000"), tol=D("0.00005"))
    # the finding stated as a count: vetoes against accountability
    OUTCOME_HOLDERS = D(1)           # the policy ministry, which holds no delivery lever
    VETOES = D(len(BODIES)) + REGIONS
    check("G.4.3 parties able to stop or blunt the outcome", VETOES, D(20))
    check("G.4.3 INVARIANT vetoes outnumber accountability",
          D(1) if VETOES > OUTCOME_HOLDERS else D(0), D(1))
    check("G.4.3 INVARIANT the operating vetoes alone outnumber the approval bodies",
          D(1) if REGIONS > D(len(BODIES)) else D(0), D(1))
