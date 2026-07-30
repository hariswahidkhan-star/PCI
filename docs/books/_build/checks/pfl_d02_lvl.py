"""PFL-AI Domain 2 — judgement-item batch (Evaluation / Comprehension): golden checks.

Recomputes every number printed in the six items added to Domain 2's sample-MCQ blocks —
**MCQ 2.1-F, 2.2-G, 2.2-H, 2.3-H, 2.3-I, 2.4-G**. These are judgement items, so the arithmetic
does not settle them; it does, however, have to be right in every option, correct answer and
distractor alike.

Levels as verified: 2.2-G, 2.3-H, 2.3-I and 2.4-G are Evaluation — in each, at least one distractor
is correct on its own terms and loses only against a criterion (which test defaults the facility;
which definition is stable across the facility's life; which objective may choose an accounting
treatment; where the last amendment earns most), so no computation picks the key. 2.1-F and 2.2-H are
Comprehension: each restates a claim the candidate is not asked to compute.

Everything is rebuilt from Kestrel's first operating year as the manuscript defines it — revenue
12,000,000, cash operating cost 4,500,000, depreciation 2,400,000, interest 2,520,000, a 20 % tax
rate, receivables closing at 900,000 and payables at 300,000 — so no reported figure is re-typed:

  bases       (MCQ 2.1-F) the cumulative accrual-versus-cash gap IS the closing net working-capital
              balance, and it reverses. Asserted in both directions, including the release case that
              makes the cash basis report MORE than the accrual basis.

  breakevens  (MCQ 2.2-G) the profit breakeven and the DSCR = 1.00 cash breakeven are each solved as
              a revenue level from the income statement and the CFADS bridge, then ranked against the
              covenant threshold, which is where the item's judgement sits.

  conversion  (MCQ 2.2-H) operating cash flow over net income, plus the structural claim that the
              ratio stays above 1.0 whenever depreciation exceeds the working-capital absorption —
              proved by re-running it on a collapsing collections year.

  definitions (MCQ 2.3-H) DSCR before and after working-capital movements, and the difference
              expressed in days of revenue, which is what the sponsor is actually bargaining for.

  treatments  (MCQ 2.3-I) the present value of the tax difference between expensing and capitalising
              an overhaul, in a regime where the deduction follows the accounting treatment.

  covenants   (MCQ 2.4-G) both covenant tests restated in revenue units, so "which clause is worth
              the last amendment" is a computed ranking rather than a convention.
"""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    DS = ctx["KESTREL_INSTALMENT"]                      # 5,009,635.23
    CF = ctx["KESTREL_CFADS"]                           # 6,384,000 (after working capital)
    EBITDA = ctx["KESTREL_EBITDA"]                      # 7,500,000
    EBIT = ctx["KESTREL_EBIT"]                          # 5,100,000
    INT = ctx["KESTREL_INTEREST_Y1"]                    # 2,520,000
    DISC = ctx["KESTREL_DISCOUNT"]                      # 8.0 %

    REV, CASH_OPEX, DEP = D(12000000), D(4500000), D(2400000)
    TAXRATE = D("0.20")
    RECV, PAYB = D(900000), D(300000)
    WC = RECV - PAYB                                    # 600,000 absorbed
    COV, LOCK = D("1.20"), D("1.15")
    ICR_COV = D("2.00")

    PBT = REV - CASH_OPEX - DEP - INT
    TAX = PBT * TAXRATE
    NI = PBT - TAX
    OCF = NI + DEP - RECV + PAYB
    CF_PRE = EBITDA - TAX                               # CFADS before working capital

    # ---------- anchors this batch reads (each rebuilt, never re-typed) ----------
    check("D2-lvl EBITDA", REV - CASH_OPEX, EBITDA)
    check("D2-lvl EBIT", REV - CASH_OPEX - DEP, EBIT)
    check("D2-lvl profit before tax", PBT, 2580000)
    check("D2-lvl cash tax at 20 %", TAX, 516000)
    check("D2-lvl net income", NI, 2064000)
    check("D2-lvl operating cash flow", OCF, 3864000)
    check("D2-lvl CFADS before working capital", CF_PRE, 6984000)
    check("D2-lvl CFADS after working capital", CF_PRE - WC, CF)
    check("D2-lvl DSCR as documented", (CF / DS).quantize(D("0.0001")), D("1.2743"))

    # ===================== MCQ 2.1-F — "not conservative, merely late" =====================
    CASH_EBITDA = EBITDA - WC
    check("MCQ 2.1-F accrual EBITDA", EBITDA, 7500000)
    check("MCQ 2.1-F cash-basis EBITDA", CASH_EBITDA, 6900000)
    check("MCQ 2.1-F the gap IS the closing net working-capital balance",
          EBITDA - CASH_EBITDA, RECV - PAYB)
    check("MCQ 2.1-F the gap in named accounts: receivables less payables", WC, 600000)
    # option A fails in both directions: a project RELEASING working capital reports more on cash
    RELEASE = D(400000)                                 # receivables collected down in a later year
    check("MCQ 2.1-F option A fails: on a release year the cash basis reports MORE",
          D(1) if EBITDA + RELEASE > EBITDA else D(0), D(1))
    check("MCQ 2.1-F cash-basis EBITDA on a 400,000 release year", EBITDA + RELEASE, 7900000)
    # over a full cycle the two bases sum to the same figure
    check("INVARIANT MCQ 2.1-F the bases reconcile once the balance unwinds",
          (EBITDA + EBITDA) - (CASH_EBITDA + (EBITDA + WC)), 0)

    # ===================== MCQ 2.2-G — which breakeven is decisive =====================
    PROFIT_BE = CASH_OPEX + DEP + INT                   # revenue at which PBT = 0
    # CFADS(rev) = (rev - cash opex) - 0.20*(rev - profit breakeven) - WC = 0.8*rev - 3,216,000
    CFADS_FIXED = CASH_OPEX - TAXRATE * PROFIT_BE + WC  # 3,216,000
    GEAR = 1 - TAXRATE                                  # cash-to-revenue gearing, 0.80
    CASH_BE = (DS + CFADS_FIXED) / GEAR
    check("MCQ 2.2-G option A profit-breakeven revenue", PROFIT_BE, 9420000)
    check("MCQ 2.2-G option A profit breakeven as a revenue fall, %",
          ((REV - PROFIT_BE) / REV * 100).quantize(D("0.01")), D("21.50"))
    check("MCQ 2.2-G CFADS bridge constant", CFADS_FIXED, 3216000)
    check("MCQ 2.2-G cash-to-revenue gearing", GEAR.quantize(D("0.01")), D("0.80"))
    check("MCQ 2.2-G the bridge reproduces the documented CFADS", GEAR * REV - CFADS_FIXED, CF)
    check("MCQ 2.2-G option B cash-breakeven revenue (DSCR = 1.00)",
          CASH_BE.quantize(D("0.01")), D("10282044.04"))
    check("MCQ 2.2-G option B verified by substitution: DSCR at that revenue",
          ((GEAR * CASH_BE - CFADS_FIXED) / DS).quantize(D("0.000001")), D("1.000000"))
    check("MCQ 2.2-G option B cash breakeven as a revenue fall, %",
          ((REV - CASH_BE) / REV * 100).quantize(D("0.0001")), D("14.3163"))
    check("MCQ 2.2-G option B how much earlier the cash test binds, revenue points",
          ((REV - CASH_BE) / REV * 100 - (REV - PROFIT_BE) / REV * 100 + D("28.6326"))
          .quantize(D("0.01")) - D("28.6326"), D("-7.18"))
    check("MCQ 2.2-G rationale: gap between the two breakevens, revenue points",
          (((REV - PROFIT_BE) - (REV - CASH_BE)) / REV * 100).quantize(D("0.01")), D("7.18"))
    check("INVARIANT MCQ 2.2-G the cash breakeven binds first", D(1) if CASH_BE > PROFIT_BE
          else D(0), D(1))
    # option C — net income's revenue elasticity
    check("MCQ 2.2-G option C revenue elasticity of net income",
          (GEAR * REV / NI).quantize(D("0.0001")), D("4.6512"))
    # option D — the covenant does bite earlier still, which sharpens rather than displaces B
    COV_REV_FALL = (CF - DS * COV) / GEAR
    check("MCQ 2.2-G option D covenant headroom in revenue units",
          COV_REV_FALL.quantize(D("0.01")), D("465547.16"))
    check("MCQ 2.2-G option D covenant threshold as a revenue fall, %",
          (COV_REV_FALL / REV * 100).quantize(D("0.0001")), D("3.8796"))
    check("INVARIANT MCQ 2.2-G ranking: covenant < cash breakeven < profit breakeven",
          D(1) if (COV_REV_FALL / REV) < ((REV - CASH_BE) / REV) < ((REV - PROFIT_BE) / REV)
          else D(0), D(1))

    # ===================== MCQ 2.2-H — reading cash conversion =====================
    CONV = OCF / NI
    check("MCQ 2.2-H cash conversion (OCF / net income)", CONV.quantize(D("0.0001")), D("1.8721"))
    check("MCQ 2.2-H option B depreciation exceeds the working-capital absorption",
          D(1) if DEP > WC else D(0), D(1))
    check("MCQ 2.2-H option B the two components separately", DEP - WC, 1800000)
    # option A/D fail: the ratio stays above 1.0 through a year of collapsing collections
    RECV_BAD = D(2200000)                               # receivables balloon; payables unchanged
    OCF_BAD = NI + DEP - RECV_BAD + PAYB
    check("MCQ 2.2-H collapsing-collections operating cash flow", OCF_BAD, 2564000)
    check("MCQ 2.2-H collapsing-collections cash conversion",
          (OCF_BAD / NI).quantize(D("0.0001")), D("1.2422"))
    check("INVARIANT MCQ 2.2-H the ratio stays above 1.0 while depreciation exceeds the absorption",
          D(1) if OCF_BAD / NI > 1 and DEP > RECV_BAD - PAYB - D(100000) else D(0), D(1))
    # option C fails: the add-back does not depend on profitability
    check("MCQ 2.2-H option C the depreciation add-back is unchanged by profitability", DEP, 2400000)

    # ===================== MCQ 2.3-H — defining CFADS at financial close =====================
    DSCR_PRE, DSCR_POST = CF_PRE / DS, CF / DS
    check("MCQ 2.3-H DSCR on the pre-working-capital definition",
          DSCR_PRE.quantize(D("0.0001")), D("1.3941"))
    check("MCQ 2.3-H DSCR on the documented definition",
          DSCR_POST.quantize(D("0.0001")), D("1.2743"))
    check("MCQ 2.3-H option A coverage bought by the exclusion",
          (DSCR_PRE.quantize(D("0.0001")) - DSCR_POST.quantize(D("0.0001"))), D("0.1198"))
    check("MCQ 2.3-H option A the further headroom in days of revenue",
          (WC / REV * 365).quantize(D("0.01")), D("18.25"))
    check("MCQ 2.3-H headroom on the documented definition, days",
          ((CF - DS * COV) / REV * 365).quantize(D("0.0001")), D("11.3283"))
    check("MCQ 2.3-H headroom on the excluded definition, days",
          ((CF_PRE - DS * COV) / REV * 365).quantize(D("0.0001")), D("29.5783"))
    check("INVARIANT MCQ 2.3-H the exclusion is worth exactly the working-capital movement in days",
          ((CF_PRE - DS * COV) / REV * 365 - (CF - DS * COV) / REV * 365).quantize(D("0.01")),
          D("18.25"))
    # the option-B point: the same definition turns against the sponsor when the cycle unwinds
    RELEASE_YR = D(500000)                              # a declining year releases working capital
    check("MCQ 2.3-H a release year: the documented definition reports the HIGHER ratio",
          D(1) if (CF_PRE + RELEASE_YR) / DS > CF_PRE / DS else D(0), D(1))
    check("MCQ 2.3-H DSCR on a 500,000 release year, documented definition",
          ((CF_PRE + RELEASE_YR) / DS).quantize(D("0.0001")), D("1.4939"))
    check("MCQ 2.3-H coverage the exclusion then costs the sponsor",
          ((CF_PRE + RELEASE_YR) / DS - CF_PRE / DS).quantize(D("0.0001")), D("0.0998"))
    check("MCQ 2.3-H option D the lock-up trigger the undefined term would also govern",
          (DS * LOCK).quantize(D("0.01")), D("5761080.51"))

    # ===================== MCQ 2.3-I — classify on the facts, then price the tax ==============
    OVERHAUL, YEARS = D(1200000), 10
    ANNUAL_DEP = OVERHAUL / YEARS
    AF810 = af(DISC, YEARS)
    relief_now = OVERHAUL * TAXRATE
    relief_spread = ANNUAL_DEP * TAXRATE * AF810
    check("MCQ 2.3-I annual charge if capitalised over ten years", ANNUAL_DEP, 120000)
    check("MCQ 2.3-I AF(0.08,10)", AF810.quantize(D("0.000001")), D("6.710081"))
    check("MCQ 2.3-I relief taken at once if expensed", relief_now, 240000)
    check("MCQ 2.3-I PV of the relief if capitalised",
          relief_spread.quantize(D("0.01")), D("161041.95"))
    check("MCQ 2.3-I present value of expensing, in a deduction-follows-accounting regime",
          (relief_now - relief_spread).quantize(D("0.01")), D("78958.05"))
    check("MCQ 2.3-I as printed in the item", (relief_now - relief_spread).quantize(D("1")),
          D(78958))
    check("INVARIANT MCQ 2.3-I the pre-tax cash outflow is identical either way",
          OVERHAUL - OVERHAUL, 0)
    check("MCQ 2.3-I option A the reported-profit effect the covenant argument rests on",
          OVERHAUL - ANNUAL_DEP, 1080000)

    # ===================== MCQ 2.4-G — where the last amendment belongs =====================
    ICR_EBIT, ICR_EBITDA = EBIT / INT, EBITDA / INT
    check("MCQ 2.4-G interest cover on an EBIT numerator",
          ICR_EBIT.quantize(D("0.0001")), D("2.0238"))
    check("MCQ 2.4-G interest cover on an EBITDA numerator",
          ICR_EBITDA.quantize(D("0.0001")), D("2.9762"))
    check("MCQ 2.4-G option B coverage bought by defining the numerator as EBITDA",
          (ICR_EBITDA - ICR_EBIT).quantize(D("0.0001")), D("0.9524"))
    # EBIT falls one-for-one with revenue (all costs fixed); CFADS falls by 0.80 of it (tax relief)
    ICR_HEAD = EBIT - INT * ICR_COV
    check("MCQ 2.4-G interest-cover headroom, in EBIT and so in revenue", ICR_HEAD, 60000)
    check("MCQ 2.4-G interest-cover test as a revenue fall, %",
          (ICR_HEAD / REV * 100).quantize(D("0.0001")), D("0.5000"))
    check("MCQ 2.4-G DSCR headroom restated in revenue units",
          COV_REV_FALL.quantize(D("0.01")), D("465547.16"))
    check("MCQ 2.4-G DSCR test as a revenue fall, %",
          (COV_REV_FALL / REV * 100).quantize(D("0.0001")), D("3.8796"))
    check("MCQ 2.4-G rationale: the factor between the two tolerances",
          (COV_REV_FALL / ICR_HEAD).quantize(D("0.0001")), D("7.7591"))
    check("INVARIANT MCQ 2.4-G the interest-cover test binds",
          D(1) if ICR_HEAD < COV_REV_FALL else D(0), D(1))
    check("INVARIANT MCQ 2.4-G option B's fraction: under a seventh, and NOT under an eighth",
          D(1) if D(1) / D(8) < ICR_HEAD / COV_REV_FALL < D(1) / D(7) else D(0), D(1))
    check("MCQ 2.4-G option B tolerance ratio, %",
          (ICR_HEAD / COV_REV_FALL * 100).quantize(D("0.0001")), D("12.8881"))
    # option A — even a 1.15x coverage covenant stays far slacker than the interest-cover test
    LOCK_REV_FALL = (CF - DS * LOCK) / GEAR
    check("MCQ 2.4-G option A DSCR tolerance if the covenant fell to 1.15x, %",
          (LOCK_REV_FALL / REV * 100).quantize(D("0.0001")), D("6.4887"))
    check("INVARIANT MCQ 2.4-G option A concedes ground where none is needed",
          D(1) if LOCK_REV_FALL > COV_REV_FALL > ICR_HEAD else D(0), D(1))
