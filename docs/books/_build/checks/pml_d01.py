"""Golden checks — PML-AI Domain 1, The Project Leadership Profession.

Every number printed in `pml-ai/manuscript/domain-01-profession.md` is recomputed here. Master-thread
values are read from ctx rather than re-typed.
"""


def run(ctx):
    check, D = ctx["check"], ctx["D"]
    COST = ctx["MERIDIAN_BASELINE"]            # 2,400,000
    POT = ctx["MERIDIAN_BENEFIT_FULL"]         # 979,200
    REAL = ctx["MERIDIAN_BENEFIT_70PCT"]       # 685,440
    COD = ctx["MERIDIAN_COST_OF_DELAY"]        # 14,280
    CL, HRS, RATE, WKS = D(40), D(6), D(85), D(48)
    PERCL = HRS * RATE                         # 510 a week per adopting clinic

    # ---- master-thread reconstruction ----
    check("D1 potential benefit 40x6x85x48", CL * HRS * RATE * WKS, POT)
    check("D1 realistic benefit at 70 %", CL * D("0.70") * HRS * RATE * WKS, REAL)
    check("D1 per-clinic weekly value 6x85", PERCL, 510)
    check("D1 cost of delay 28x510", D(28) * PERCL, COD)
    check("D1 1.3.2 overstatement", POT - REAL, 293760)
    check("D1 1.3.2 overstatement share", (POT - REAL) / POT * 100, D("30.0"))
    check("D1 1.3.2 honest/claim ratio is the adoption term", REAL / POT, D("0.70"))
    check("D1 1.3.2 per-clinic potential 979,200/40", POT / CL, 24480)
    check("D1 1.3.2 per-clinic realistic", POT / CL * D("0.70"), 17136)
    check("D1 1.3.2 five-point definition difference", D("0.05") * POT, 48960)
    check("D1 1.3.2 sensitivity 50 %", CL * D("0.50") * PERCL * WKS, 489600)
    check("D1 1.3.2 sensitivity 90 %", CL * D("0.90") * PERCL * WKS, 881280)

    # ---- WE 1.1.1 the two verdicts, priced ----
    UNDER = COST * D("0.03")
    B40 = CL * D("0.40") * PERCL * WKS
    SHORT = REAL - B40
    check("WE 1.1.1 3 % underspend", UNDER, 72000)
    check("WE 1.1.1 benefit at 40 % adoption", B40, 391680)
    check("WE 1.1.1 annual shortfall", SHORT, 293760)
    check("WE 1.1.1 shortfall via potential x 0.30", POT * (D("0.70") - D("0.40")), 293760)
    check("WE 1.1.1 ratio shortfall/underspend", SHORT / UNDER, D("4.08"))
    check("WE 1.1.1 shortfall per week", SHORT / WKS, 6120)
    check("WE 1.1.1 twelve clinics x 510", D(12) * PERCL, 6120)
    check("WE 1.1.1 eight years undiscounted", SHORT * 8, 2350080)
    check("WE 1.1.1 undiscounted as % of approved cost", SHORT * 8 / COST * 100, D("97.92"))
    check("WE 1.1.1 PV at AF 5.971299", SHORT * D("5.971299"), D("1754128.79"))
    check("WE 1.1.1 PV as % of approved cost", SHORT * D("5.971299") / COST * 100, D("73.09"))
    check("WE 1.1.1 overstatement of quoting undiscounted",
          SHORT * 8 - SHORT * D("5.971299"), D("595951.21"))
    check("WE 1.1.1 underspend % needed to offset year one", SHORT / COST * 100, D("12.24"))
    check("WE 1.1.1 outturn implied by 3 % under", COST * D("0.97"), 2328000)
    check("WE 1.1.1 swing to D16 outturn", D(2514000) - COST * D("0.97"), 186000)

    # ---- WE 1.2.2 the price of a late escalation ----
    GAP = CL * D("0.70") - CL * D("0.40")            # 12 clinics
    RUN = GAP * PERCL                                # 6,120 a week
    RECCL = CL * D("0.68") - CL * D("0.40")          # 11.2 clinics
    RECRUN = RECCL * PERCL                           # 5,712 a week
    REMEDY = D(11) * D(12000)                        # 132,000
    check("WE 1.2.2 gap clinics 28-16", GAP, D("12.0"))
    check("WE 1.2.2 shortfall run rate", RUN, 6120)
    check("WE 1.2.2 clinics recovered 27.2-16", RECCL, D("11.2"))
    check("WE 1.2.2 recovered run rate", RECRUN, 5712)
    check("WE 1.2.2 timing value over 26 weeks", D(26) * RECRUN, 148512)
    check("WE 1.2.2 remedy cost", REMEDY, 132000)
    check("WE 1.2.2 timing value / remedy", D(26) * RECRUN / REMEDY, D("1.13"), tol=D("0.005"))
    check("WE 1.2.2 weeks of earliness that fund the remedy", REMEDY / RECRUN, D("23.11"))
    check("WE 1.2.2 margin weeks", D(26) - REMEDY / RECRUN, D("2.89"))
    check("WE 1.2.2 recovered run rate as % of cost of delay", RECRUN / COD * 100, D("40.00"))
    check("WE 1.2.2 overstatement from using the gap rate", D(26) * RUN - D(26) * RECRUN, 10608)
    check("WE 1.2.2 full-gap upper bound", D(26) * RUN, 159120)
    check("Case A recovered benefit a year", RECRUN * WKS, 274176)
    # Fig 1.2.1 plotted values
    check("Fig 1.2.1 residual run rate 0.8 clinics", D("0.8") * PERCL, 408)
    check("Fig 1.2.1 crimson at week 39", D(26) * RUN, 159120)
    check("Fig 1.2.1 crimson at week 52", D(26) * RUN + D(13) * D("0.8") * PERCL, 164424)
    check("Fig 1.2.1 blue at week 52", D(39) * D("0.8") * PERCL, 15912)
    check("Fig 1.2.1 gap at week 52", D(26) * RUN + D(13) * D("0.8") * PERCL - D(39) * D("0.8") * PERCL,
          148512)
    check("Fig 1.2.1 week the gap equals the remedy cost", 13 + REMEDY / RECRUN, D("36.11"))

    # ---- WE 1.3.1 the reinforcement trough ----
    PER, CREW, NEW = D("0.1"), D(6), D(3)
    BASE = CREW * PER                                 # 0.6 clinics a week
    LOSS = NEW * D("0.5") * PER                       # 0.15
    NEWCAP = NEW * D("0.25") * PER                    # 0.075
    RAMPR = BASE - LOSS + NEWCAP                      # 0.525
    POST = BASE + NEW * PER                           # 0.9
    REM = D(8)
    BASEDUR = REM / BASE                              # 13.3333
    RAMPOUT = D(4) * RAMPR                            # 2.1
    NEWDUR = D(4) + (REM - RAMPOUT) / POST            # 10.5556
    SAVE = BASEDUR - NEWDUR                           # 2.7778
    INCSW = (CREW + NEW) * NEWDUR - CREW * BASEDUR    # 15.0
    check("WE 1.3.1 baseline rate", BASE, D("0.6"))
    check("WE 1.3.1 supervision loss", LOSS, D("0.15"))
    check("WE 1.3.1 newcomer ramp capacity", NEWCAP, D("0.075"))
    check("WE 1.3.1 ramp-period rate", RAMPR, D("0.525"))
    check("WE 1.3.1 post-ramp rate", POST, D("0.9"))
    check("WE 1.3.1 baseline duration", BASEDUR, D("13.3333"))
    check("WE 1.3.1 output in ramp weeks", RAMPOUT, D("2.1"))
    check("WE 1.3.1 duration with reinforcement", NEWDUR, D("10.5556"))
    check("WE 1.3.1 weeks saved", SAVE, D("2.7778"))
    check("WE 1.3.1 delay value saved", SAVE * COD, D("39666.67"))
    check("WE 1.3.1 specialist-weeks baseline", CREW * BASEDUR, D("80.0"))
    check("WE 1.3.1 specialist-weeks reinforced", (CREW + NEW) * NEWDUR, D("95.0"))
    check("WE 1.3.1 incremental specialist-weeks", INCSW, D("15.0"))
    check("WE 1.3.1 incremental cost at 4,200", INCSW * D(4200), 63000)
    check("WE 1.3.1 net", SAVE * COD - INCSW * D(4200), D("-23333.33"))
    check("WE 1.3.1 breakeven cost of delay", INCSW * D(4200) / SAVE, 22680)
    check("WE 1.3.1 maximum cost of delay at 100 % adoption", CL * PERCL, 20400)
    check("WE 1.3.1 breakeven over the ceiling %",
          (INCSW * D(4200) / SAVE) / (CL * PERCL) * 100 - 100, D("11.18"))
    check("WE 1.3.1 breakeven specialist-week rate", SAVE * COD / INCSW, D("2644.44"))
    check("WE 1.3.1 cumulative at week 4, crew of 6", D(4) * BASE, D("2.4"))
    check("WE 1.3.1 trough depth in clinics", D(4) * BASE - RAMPOUT, D("0.3"))
    check("WE 1.3.1 trough depth %", (D(4) * BASE - RAMPOUT) / (D(4) * BASE) * 100, D("12.5"))
    check("WE 1.3.1 crossover horizon", D(4) * (1 + (LOSS - NEWCAP) / (NEW * PER)), D("5.00"))
    check("WE 1.3.1 crossover with a 2-week ramp",
          D(2) * (1 + (LOSS - NEWCAP) / (NEW * PER)), D("2.50"))
    check("WE 1.3.1 crossover with supervision halved",
          D(4) * (1 + (NEW * D("0.25") * PER - NEWCAP) / (NEW * PER)), D("4.00"))
    check("WE 1.3.1 no-trough rate at 25 % supervision, 50 % ramp",
          BASE - NEW * D("0.25") * PER + NEW * D("0.5") * PER, D("0.675"))

    # ---- WE 1.3.2b payback and the adoption hurdle ----
    PBOUT, PBHON = COST / POT, COST / REAL
    REQB = COST / 3
    REQA = REQB / POT * 100
    check("WE 1.3.2b payback on output claim", PBOUT, D("2.4510"))
    check("WE 1.3.2b payback honest", PBHON, D("3.5014"))
    check("WE 1.3.2b ratio of paybacks", PBHON / PBOUT, D("1.428571"))
    check("WE 1.3.2b reciprocal-of-adoption identity", 1 / D("0.70"), D("1.428571"))
    check("WE 1.3.2b benefit needed for a 3-year payback", REQB, 800000)
    check("WE 1.3.2b adoption a 3-year rule requires %", REQA, D("81.70"))
    check("WE 1.3.2b gap above the case assumption, points", REQA - 70, D("11.70"))
    check("WE 1.3.2b gap in clinics", (REQA / 100 - D("0.70")) * CL, D("4.68"))
    check("WE 1.3.2b payback at 40 %", COST / (POT * D("0.40")), D("6.1275"))
    check("WE 1.3.2b payback at 50 %", COST / (POT * D("0.50")), D("4.9020"))
    check("WE 1.3.2b payback at 80 %", COST / (POT * D("0.80")), D("3.0637"))
    check("WE 1.3.2b payback at 90 %", COST / (POT * D("0.90")), D("2.7233"))
    check("WE 1.3.2b payback at 30 % (figure)", COST / (POT * D("0.30")), D("8.1699"))
    check("WE 1.3.2b payback at 100 % (figure)", COST / POT, D("2.4510"))
    D1 = COST / (POT * D("0.40")) - COST / (POT * D("0.50"))
    D2 = COST / (POT * D("0.80")) - COST / (POT * D("0.90"))
    check("WE 1.3.2b years cut, 40 to 50 %", D1, D("1.2255"))
    check("WE 1.3.2b years cut, 80 to 90 %", D2, D("0.3404"))
    check("WE 1.3.2b marginal-value ratio", D1 / D2, D("3.60"))
    check("WE 1.3.2b net of D16 run cost", REAL - D(108000), 577440)
    check("WE 1.3.2b payback including run cost", COST / (REAL - D(108000)), D("4.1563"))
    check("WE 1.3.2b years added by the omitted run cost",
          COST / (REAL - D(108000)) - PBHON, D("0.6549"))
    check("WE 1.3.2b gap to D2 breakeven adoption, points", REQA - D("41.05"), D("40.65"))
    check("Case A ratio of paybacks 6.1275/2.4510", COST / (POT * D("0.40")) / PBOUT, D("2.50"))

    # ---- WE 1.3.3 deeper interpretation ----
    SPEND, ACCW = D(60000), D(8)
    check("WE 1.3.3 weekly benefit needed over 8 weeks", SPEND / ACCW, 7500)
    check("WE 1.3.3 clinics needed", SPEND / ACCW / PERCL, D("14.7059"))
    check("WE 1.3.3 breakeven adoption %", SPEND / ACCW / PERCL / CL * 100, D("36.76"))
    check("WE 1.3.3 cost of delay at 40 % adoption", D(16) * PERCL, 8160)
    check("WE 1.3.3 breakeven weeks at 40 %", SPEND / (D(16) * PERCL), D("7.3529"))
    check("WE 1.3.3 margin weeks at 40 %", ACCW - SPEND / (D(16) * PERCL), D("0.6471"))
    check("WE 1.3.3 net at 40 % adoption", ACCW * D(16) * PERCL - SPEND, 5280)
    check("WE 1.3.3 net at 70 % adoption", ACCW * D(28) * PERCL - SPEND, 54240)
    check("WE 1.3.3 collapse in net % (display rounding to one decimal)",
          (1 - (ACCW * D(16) * PERCL - SPEND) / (ACCW * D(28) * PERCL - SPEND)) * 100, D("90.3"),
          tol=D("0.05"))
    check("WE 1.3.3 maximum justified spend at 70 %", ACCW * COD, 114240)
    check("WE 1.3.3 maximum justified spend at 40 %", ACCW * D(16) * PERCL, 65280)
    check("WE 1.3.3 breakeven weeks", SPEND / COD, D("4.20"))

    # ---- WE 1.4.3 proportionate verification ----
    def pstar(cv, q, loss):
        return cv / ((1 - q) * loss) * 100

    check("WE 1.4.3 glance cost 0.5 h at 85", D("0.5") * 85, D("42.50"))
    check("WE 1.4.3 recompute cost 6 h at 85", D(6) * 85, 510)
    check("WE 1.4.3 summary exposed loss", D("0.10") * 5000, 500)
    check("WE 1.4.3 board exposed loss", D("0.75") * D(293760), 220320)
    check("WE 1.4.3 safety exposed loss", D("0.95") * D(4000000), 3800000)
    check("WE 1.4.3 P* summary %", pstar(D("42.50"), D("0.90"), D(5000)), D("8.5000"))
    check("WE 1.4.3 P* board figure %", pstar(D(510), D("0.25"), D(293760)), D("0.2315"))
    check("WE 1.4.3 P* safety assessment %", pstar(D(12000), D("0.05"), D(4000000)), D("0.3158"))
    check("WE 1.4.3 ratio summary/board",
          pstar(D("42.50"), D("0.90"), D(5000)) / pstar(D(510), D("0.25"), D(293760)), D("36.72"))
    check("WE 1.4.3 ratio safety/board",
          pstar(D(12000), D("0.05"), D(4000000)) / pstar(D(510), D("0.25"), D(293760)), D("1.36"))
    check("WE 1.4.3 verification cost ratio safety/board", D(12000) / D(510), D("23.53"))
    check("WE 1.4.3 uniform recompute on the summary %", D(510) / D(500) * 100, D("102.00"))
    check("WE 1.4.3 P* summary with no downstream control %", D("42.50") / D(5000) * 100, D("0.8500"))
    check("MCQ 1.4-D distractor A %", D(510) / D(293760) * 100, D("0.1736"))
    check("MCQ 1.4-D distractor C %", D(510) / (D("0.25") * D(293760)) * 100, D("0.6944"))
    check("MCQ 1.4-D distractor D %", D(510) * D("0.75") / D(293760) * 100, D("0.1302"))

    # ---- Case study B (financial services) ----
    CODB = D(26000) + D(12000)
    VCOST = 2 * 4 * D(145)
    check("Case B cost of delay", CODB, 38000)
    check("Case B seven-week slip", 7 * CODB, 266000)
    check("Case B verification cost", VCOST, 1160)
    check("Case B loss-to-check ratio", 7 * CODB / VCOST, D("229.31"))
    check("Case B breakeven error probability %", VCOST / (7 * CODB) * 100, D("0.4361"))

    # ---- Calculation exercises ----
    check("Ex 1.1 adopting sites", D(60) * D("0.65"), 39)
    check("Ex 1.1 defensible benefit", D(39) * D(5) * D(90) * D(46), 807300)
    check("Ex 1.1 output-based claim", D(60) * D(5) * D(90) * D(46), 1242000)
    check("Ex 1.1 overstatement", D(1242000) - D(807300), 434700)
    check("Ex 1.1 overstatement %", (D(1242000) - D(807300)) / D(1242000) * 100, D("35.0"))
    check("Ex 1.2 five weeks of delay", 5 * COD, 71400)
    check("Ex 1.3 spread across 40 adoption points", D(881280) - D(489600), 391680)
    check("Ex 1.4 underspend", D(1800000) * D("0.02"), 36000)
    check("Ex 1.4 benefit at plan", D(540000) * D("0.75"), 405000)
    check("Ex 1.4 benefit at outturn", D(540000) * D("0.45"), 243000)
    check("Ex 1.4 shortfall", D(540000) * D("0.30"), 162000)
    check("Ex 1.4 ratio", D(162000) / D(36000), D("4.50"))
    check("Ex 1.4 illegitimate net (the common error)", D(162000) - D(36000), 126000)
    check("Ex 1.5 benefit at 60 %", D(1050000) * D("0.60"), 630000)
    check("Ex 1.5 payback", D(3150000) / D(630000), D("5.00"))
    check("Ex 1.5 benefit needed for 4 years", D(3150000) / 4, 787500)
    check("Ex 1.5 adoption the rule requires %", D(787500) / D(1050000) * 100, D("75.00"))
    check("Ex 1.5 output-based payback (the common error)", D(3150000) / D(1050000), D("3.00"))
    check("Ex 1.5 understatement factor", 1 / D("0.60"), D("1.6667"))
    check("Ex 1.6 supervision loss", 2 * D("0.6") * D("0.08"), D("0.096"))
    check("Ex 1.6 newcomer output", 2 * D("0.2") * D("0.08"), D("0.032"))
    check("Ex 1.6 deficit rate", D("0.096") - D("0.032"), D("0.064"))
    check("Ex 1.6 gain rate", 2 * D("0.08"), D("0.16"))
    check("Ex 1.6 crossover horizon", 3 * (1 + D("0.064") / D("0.16")), D("4.20"))
    check("Ex 1.6 rate before reinforcement", 5 * D("0.08"), D("0.40"))
    check("Ex 1.6 rate after reinforcement", 7 * D("0.08"), D("0.56"))
    check("Ex 1.7 exposed loss", D("0.60") * D(250000), 150000)
    check("Ex 1.7 breakeven %", D(720) / D(150000) * 100, D("0.48"))
    check("Ex 1.7 breakeven with no control %", D(720) / D(250000) * 100, D("0.288"))
    check("Ex 1.7 breakeven at half the check cost %", D(360) / D(150000) * 100, D("0.24"))
