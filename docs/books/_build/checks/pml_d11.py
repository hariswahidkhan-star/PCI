"""PML-AI Domain 11 — Stakeholders, communication and influence: golden checks.

Every number printed as a result in `domain-11-stakeholders-communication-influence.md` is
recomputed here from its definition rather than from its printed output. Master-thread values
(`MERIDIAN_COST_OF_DELAY`, `MERIDIAN_BENEFIT_70PCT`, `MERIDIAN_BENEFIT_FULL`) and the shared
helpers `ew(M, L)` (governance latency, Domain 3) and `mesh(n)` (pairwise channels, Domain 4)
come from ctx and are never re-typed. Two rates declared once below and reused: the blended
internal hour rate USD 110 and the neutral facilitator rate USD 150, both stated in the chapter's
binding blockquote. Domain 4's interface unit cost USD 18,000 is likewise declared once.

The domain has six engines, each checked from first principles:

    objection cost   = (ew(M, L) + rework weeks) x cost of delay + k x interface unit cost +
                       application rework; the breakeven probability p* = avoidance / consequence
                       is solved, not asserted, and the sensitivity to a changed committee design
                       is recomputed through ew(4, 1) rather than by adjusting the printed total.
    allocation       = capacity x salience / sum(salience) for the conventional rule, against a
                       three-term design (value core + consent floors + salience residual). Both
                       are asserted to sum exactly to capacity. The benefit gain, the value per
                       engagement hour, the gain forgone by the salience rule and the breakeven
                       adoption uplift are each derived; the cost-of-delay interaction is rebuilt
                       from the per-clinic weekly rate implied by MERIDIAN_BENEFIT_FULL.
    channel load     = mesh(n) or n, times hours per channel, plus hub overhead; the sustainable
                       party counts are found by search over n rather than by quoting 7 and 18,
                       and the one-more-party figure is checked so the boundary is genuine.
    report age       = C + L (newest), C + L + P/2 (mean), P + C + L (maximum blindness) for two
                       designs, with every saving priced at the cost of delay.
    negotiation      = RV(buyer) from a fully priced alternative (invoice + weeks x cost of delay +
                       effort + EMV) and RV(seller) from cost to serve + forgone contribution; the
                       ZOPA, midpoint, surpluses and shares follow, and the improved-alternative
                       case is rebuilt end to end so that the surplus-versus-cash trap is a
                       computed contrast. Joint gain is computed for the split-the-difference
                       compromise and for the whole-issue trade, with the conservation identity
                       buyer net == seller net asserted at the equal share.
    consent + AI     = EMV with and without a mitigation, the breakeven mitigation cost and the
                       required probability reduction solved; then verification hours, exposure,
                       return and breakeven error rate for AI-drafted communications.

Also checked: every numeric MCQ distractor (each is the result of one nameable error), all five
calculation exercises, Case study B's energy arithmetic, and the blocking-minority and
register-decay figures of the advanced topics.
"""


def run(ctx):
    check, D, ew, mesh = ctx["check"], ctx["D"], ctx["ew"], ctx["mesh"]
    COD = ctx["MERIDIAN_COST_OF_DELAY"]                 # USD 14,280 per week (Domain 1)
    BEN70, BENFULL = ctx["MERIDIAN_BENEFIT_70PCT"], ctx["MERIDIAN_BENEFIT_FULL"]

    RATE = D(110)                    # blended internal hour rate, stated in the chapter
    FAC = D(150)                     # neutral facilitator hour rate, stated in the chapter
    IFACE = D(18000)                 # one interface, specified/built/tested/documented (D4 WE 4.2.3)
    CLINICS = D(40)

    # Per-clinic value rebuilt from the master thread rather than re-typed: the full-potential
    # benefit is all 40 clinics, so one clinic is BENFULL/40 a year, and a 48-week operating year
    # gives the weekly rate that COD is built from.
    per_clinic_year = BENFULL / CLINICS
    check("D11 per adopting clinic per year (from MERIDIAN_BENEFIT_FULL)", per_clinic_year, 24480)
    per_clinic_week = per_clinic_year / 48
    check("D11 per adopting clinic per week", per_clinic_week, 510)
    check("D11 cost of delay reconciles at 70 % adoption",
          CLINICS * D("0.70") * per_clinic_week, COD)
    check("D11 benefit at 70 % adoption reconciles", CLINICS * D("0.70") * per_clinic_year, BEN70)

    # ---------------- WE 11.1.2 — the objection that arrived in week 34 ----------------
    gov = ew(4, 2)                                       # Meridian steering committee (D3 3.2.3)
    check("WE 11.1.2 governance latency ew(4, 2)", gov, D("4.0"))
    delay_w = gov + 5
    check("WE 11.1.2 delay weeks", delay_w, D("9.0"))
    delay_cost = delay_w * COD
    check("WE 11.1.2 delay cost", delay_cost, 128520)
    iface = 3 * IFACE
    check("WE 11.1.2 interface re-verification", iface, 54000)
    direct = iface + D(42000)
    check("WE 11.1.2 direct cost", direct, 96000)
    total_obj = delay_cost + direct
    check("WE 11.1.2 assessed total", total_obj, 224520)
    check("WE 11.1.2 assessed total in SAR at 3.75", total_obj * D("3.75"), 841950)
    avoid = (D(24) + D(6)) * RATE
    check("WE 11.1.2 avoidance cost, 30 h", avoid, 3300)
    check("WE 11.1.2 consequence : avoidance ratio", total_obj / avoid, D("68.04"), D("0.005"))
    check("WE 11.1.2 breakeven probability, %", avoid / total_obj * 100, D("1.47"), D("0.005"))
    # Sensitivity: the same objection under Domain 3's redesigned committee (M = 4, L = 1).
    check("WE 11.1.2 total at ew(4, 1) = 3.0 weeks", (ew(4, 1) + 5) * COD + direct, 210240)
    # MCQ 11.1-B distractors, each a nameable error.
    check("MCQ 11.1-B A direct only", direct, 96000)
    check("MCQ 11.1-B B delay only", delay_cost, 128520)
    check("MCQ 11.1-B C omits governance latency", D(5) * COD + direct, 167400)

    # ---------------- WE 11.1.3 — allocating 480 engagement hours ----------------
    CAP_M, MONTHS = D(40), D(12)
    CAP = CAP_M * MONTHS
    check("WE 11.1.3 engagement capacity", CAP, 480)
    SAL = {"authority": D(5) * D(3), "directors": D(4) * D(5), "clinicians": D(2) * D(5),
           "admins": D(2) * D(4), "reporting": D(5) * D(2), "patients": D(3) * D(3),
           "supplier": D(2) * D(4)}
    tot_sal = sum(SAL.values())
    check("WE 11.1.3 total salience", tot_sal, 80)
    per_pt = CAP / tot_sal
    check("WE 11.1.3 hours per salience point", per_pt, 6)
    grid = {k: per_pt * v for k, v in SAL.items()}
    for k, printed in (("authority", 90), ("directors", 120), ("clinicians", 60), ("admins", 48),
                       ("reporting", 60), ("patients", 54), ("supplier", 48)):
        check(f"WE 11.1.3 salience allocation {k}", grid[k], printed)
    check("WE 11.1.3 salience allocation sums to capacity", sum(grid.values()), CAP)

    core = D(8) * CLINICS
    check("WE 11.1.3 value-led core, 8 h x 40 clinics", core, 320)
    floors = {"authority": D(60), "reporting": D(40), "patients": D(30)}
    check("WE 11.1.3 consent-risk floors total", sum(floors.values()), 130)
    resid = CAP - core - sum(floors.values())
    check("WE 11.1.3 residual", resid, 30)
    resid_sal = SAL["admins"] + SAL["supplier"]
    final = dict(floors)
    final["directors"] = final["clinicians"] = core / 2
    final["admins"] = resid * SAL["admins"] / resid_sal
    final["supplier"] = resid * SAL["supplier"] / resid_sal
    for k, printed in (("authority", 60), ("directors", 160), ("clinicians", 160), ("admins", 15),
                       ("reporting", 40), ("patients", 30), ("supplier", 15)):
        check(f"WE 11.1.3 three-term allocation {k}", final[k], printed)
    check("WE 11.1.3 three-term allocation sums to capacity", sum(final.values()), CAP)

    UPLIFT_PP = D("12.5")
    uplift_clinics = CLINICS * UPLIFT_PP / 100
    check("WE 11.1.3 additional adopting clinics", uplift_clinics, 5)
    gain = uplift_clinics * per_clinic_year
    check("WE 11.1.3 annual benefit gain", gain, 122400)
    check("WE 11.1.3 benefit per engagement hour", gain / core, D("382.50"))
    grid_core = grid["directors"] + grid["clinicians"]
    check("WE 11.1.3 salience hours to the adoption groups", grid_core, 180)
    covered = (grid_core / 8).to_integral_value(rounding="ROUND_FLOOR")
    check("WE 11.1.3 clinics the salience hours cover", covered, 22)
    check("WE 11.1.3 salience uplift, pp", covered * UPLIFT_PP / CLINICS, D("6.875"))
    check("WE 11.1.3 salience extra clinics", covered * uplift_clinics / CLINICS, D("2.75"))
    grid_gain = covered * uplift_clinics / CLINICS * per_clinic_year
    check("WE 11.1.3 annual gain under salience rule", grid_gain, 67320)
    check("WE 11.1.3 annual gain forgone", gain - grid_gain, 55080)
    opp = core * RATE
    check("WE 11.1.3 opportunity cost of the core", opp, 35200)
    be_clinics = opp / per_clinic_year
    check("WE 11.1.3 breakeven uplift, clinics", be_clinics, D("1.4379"), D("0.00005"))
    check("WE 11.1.3 breakeven uplift, pp", be_clinics / CLINICS * 100, D("3.59"), D("0.005"))
    adopt825 = CLINICS * D("0.825")
    check("WE 11.1.3 adopting clinics at 82.5 %", adopt825, 33)
    cod2 = adopt825 * per_clinic_week
    check("WE 11.1.3 cost of delay at 82.5 %", cod2, 16830)
    check("WE 11.1.3 rise in cost of delay", cod2 - COD, 2550)
    check("WE 11.1.3 rise in cost of delay, %", (cod2 - COD) / COD * 100, D("17.86"), D("0.005"))
    check("WE 11.1.3 D1 compression net at 70 %", D(8) * COD - D(60000), 54240)
    check("WE 11.1.3 D1 compression net at 82.5 %", D(8) * cod2 - D(60000), 74640)
    # MCQ 11.1-E distractors.
    check("MCQ 11.1-E A one clinic's value / 320 h", per_clinic_year / core, D("76.50"))
    check("MCQ 11.1-E C gain / 40 clinics", gain / CLINICS, D("3060.00"))
    check("MCQ 11.1-E D whole benefit / 320 h", BEN70 / core, D("2142.00"))

    # ---------------- WE 11.2.1 — the communication load ----------------
    n, HPC, HUB = 14, D("1.5"), D(12)
    check("WE 11.2.1 mesh channels for 14 parties", mesh(n), 91)
    mesh_h = mesh(n) * HPC
    lay_h = D(n) * HPC + HUB
    check("WE 11.2.1 mesh hours per month", mesh_h, D("136.5"))
    check("WE 11.2.1 routed hours per month", lay_h, D("33.0"))
    check("WE 11.2.1 mesh utilisation, %", mesh_h / CAP_M * 100, D("341.25"))
    check("WE 11.2.1 routed utilisation, %", lay_h / CAP_M * 100, D("82.5"))
    k = 2
    while mesh(k + 1) * HPC <= CAP_M:
        k += 1
    check("WE 11.2.1 sustainable parties unrouted", k, 7)
    check("WE 11.2.1   hours at that count", mesh(k) * HPC, D("31.5"))
    check("WE 11.2.1   hours at one more party", mesh(k + 1) * HPC, D("42.0"))
    k2 = 1
    while D(k2 + 1) * HPC + HUB <= CAP_M:
        k2 += 1
    check("WE 11.2.1 sustainable parties routed", k2, 18)
    check("WE 11.2.1   hours at that count", D(k2) * HPC + HUB, D("39.0"))
    check("WE 11.2.1 monthly hours saved", mesh_h - lay_h, D("103.5"))
    check("WE 11.2.1 annual hours saved", (mesh_h - lay_h) * 12, 1242)
    check("WE 11.2.1 annual saving at 110/h", (mesh_h - lay_h) * 12 * RATE, 136620)
    hyb_ch = mesh(3) + n
    check("WE 11.2.1 hybrid channels", hyb_ch, 17)
    check("WE 11.2.1 hybrid hours per month", hyb_ch * HPC + HUB, D("37.5"))
    check("WE 11.2.1 hybrid utilisation, %", (hyb_ch * HPC + HUB) / CAP_M * 100, D("93.75"))
    check("MCQ 11.2-A C each pair twice", D(n) * (D(n) - 1), 182)
    check("MCQ 11.2-A D n squared", D(n) * D(n), 196)
    check("MCQ 11.2-B D capacity / unit cost, floored",
          (CAP_M / HPC).to_integral_value(rounding="ROUND_FLOOR"), 26)

    # ---------------- WE 11.2.2 — report age ----------------
    def ages(P, C, L):
        P, C, L = D(P), D(C), D(L)
        return C + L, C + L + P / 2, P + C + L

    a1 = ages(4, 1, 2)
    a2 = ages(1, "0.5", 1)
    for lab, got, printed in (("newest", a1[0], "3.0"), ("mean", a1[1], "5.0"),
                              ("blindness", a1[2], "7.0")):
        check(f"WE 11.2.2 monthly {lab}", got, D(printed))
    for lab, got, printed in (("newest", a2[0], "1.5"), ("mean", a2[1], "2.0"),
                              ("blindness", a2[2], "2.5")):
        check(f"WE 11.2.2 weekly-cut {lab}", got, D(printed))
    for lab, x, y, wk, money in (("newest", a1[0], a2[0], "1.5", 21420),
                                 ("mean", a1[1], a2[1], "3.0", 42840),
                                 ("blindness", a1[2], a2[2], "4.5", 64260)):
        check(f"WE 11.2.2 weeks saved on {lab}", x - y, D(wk))
        check(f"WE 11.2.2 saving priced, {lab}", (x - y) * COD, money)
    check("WE 11.2.2 seven weeks of blindness priced", a1[2] * COD, 99960)

    # ---------------- WE 11.3.1 — reservation values and the ZOPA ----------------
    ALT_INVOICE, ALT_EFFORT = D(640000), D(74320)
    alt_delay = D(6) * COD
    check("WE 11.3.1 alternative delay component", alt_delay, 85680)
    RVb = ALT_INVOICE + alt_delay + ALT_EFFORT + D(45000)
    RVs = D(520000) + D(95000)
    check("WE 11.3.1 buyer reservation value", RVb, 845000)
    check("WE 11.3.1 seller reservation value", RVs, 615000)
    zopa = RVb - RVs
    mid = (RVb + RVs) / 2
    check("WE 11.3.1 ZOPA width", zopa, 230000)
    check("WE 11.3.1 midpoint settlement", mid, 730000)
    check("WE 11.3.1 buyer surplus at midpoint", RVb - mid, 115000)
    check("WE 11.3.1 seller surplus at midpoint", mid - RVs, 115000)
    check("WE 11.3.1 midpoint surpluses are equal", (RVb - mid) - (mid - RVs), 0)
    unprep = D(780000)
    check("WE 11.3.1 buyer surplus if unprepared", RVb - unprep, 65000)
    # 65,000/230,000 = 28.2608...% and 165,000/230,000 = 71.7391...%: tolerance widened to
    # 0.05 because the manuscript displays these two shares to one decimal place.
    check("WE 11.3.1 buyer share if unprepared, % (1 dp display)",
          (RVb - unprep) / zopa * 100, D("28.3"), D("0.05"))
    check("WE 11.3.1 seller share if unprepared, % (1 dp display)",
          (unprep - RVs) / zopa * 100, D("71.7"), D("0.05"))
    check("WE 11.3.1 shares sum to 100 %",
          ((RVb - unprep) + (unprep - RVs)) / zopa * 100, 100)
    check("WE 11.3.1 value transferred vs midpoint", unprep - mid, 50000)
    check("WE 11.3.1 transfer : engagement cost of 11.1.2", (unprep - mid) / avoid,
          D("15.15"), D("0.005"))
    RVb2 = ALT_INVOICE + D(2) * COD + ALT_EFFORT + D(12000)
    check("WE 11.3.1 improved alternative delay component", D(2) * COD, 28560)
    check("WE 11.3.1 improved buyer reservation value", RVb2, 754880)
    check("WE 11.3.1 improved ZOPA width", RVb2 - RVs, 139880)
    mid2 = (RVb2 + RVs) / 2
    check("WE 11.3.1 improved midpoint settlement", mid2, 684940)
    check("WE 11.3.1 cash saved", mid - mid2, 45060)
    check("WE 11.3.1 net of 25,000 pre-qualification", mid - mid2 - D(25000), 20060)
    check("WE 11.3.1 breakeven pre-qualification spend", mid - mid2, 45060)
    check("WE 11.3.1 buyer surplus after improvement", RVb2 - mid2, 69940)
    check("WE 11.3.1 risk removed (EMV)", D(45000) - D(12000), 33000)
    check("MCQ 11.3-A A cost to serve less forgone", D(520000) - D(95000), 425000)
    check("MCQ 11.3-B C seller surplus at 780,000", unprep - RVs, 165000)

    # ---------------- WE 11.3.2 — creating value by trading whole issues ----------------
    ISSUES = (("A", D(84000), D(30000)), ("B", D(36000), D(60000)), ("C", D(40000), D(12000)))
    half_v = sum(v for _, v, _ in ISSUES) / 2
    half_c = sum(c for _, _, c in ISSUES) / 2
    check("WE 11.3.2 compromise buyer value", half_v, 80000)
    check("WE 11.3.2 compromise seller cost", half_c, 51000)
    check("WE 11.3.2 compromise joint gain", half_v - half_c, 29000)
    trade_v = ISSUES[0][1] + ISSUES[2][1]
    trade_c = ISSUES[0][2] + ISSUES[2][2]
    check("WE 11.3.2 trade buyer value", trade_v, 124000)
    check("WE 11.3.2 trade seller cost", trade_c, 42000)
    joint = trade_v - trade_c
    check("WE 11.3.2 trade joint gain", joint, 82000)
    check("WE 11.3.2 improvement over compromise", joint - (half_v - half_c), 53000)
    check("WE 11.3.2 trade : compromise ratio", joint / (half_v - half_c),
          D("2.83"), D("0.005"))
    check("WE 11.3.2 issue B joint value", ISSUES[1][1] - ISSUES[1][2], -24000)
    check("WE 11.3.2 issue C value : cost", ISSUES[2][1] / ISSUES[2][2], D("3.33"), D("0.005"))
    share = joint / 2
    price_up = trade_c + share
    check("WE 11.3.2 equal share of joint gain", share, 41000)
    check("WE 11.3.2 price increase", price_up, 83000)
    check("WE 11.3.2 buyer net", trade_v - price_up, 41000)
    check("WE 11.3.2 seller net", price_up - trade_c, 41000)
    check("WE 11.3.2 conservation: buyer net == seller net",
          (trade_v - price_up) - (price_up - trade_c), 0)
    check("WE 11.3.2 new contract price", mid2 + price_up, 767940)
    check("MCQ 11.3-D A trades all three issues",
          sum(v for _, v, _ in ISSUES) - sum(c for _, _, c in ISSUES), 58000)

    # ---------------- WE 11.4.1 — consent risk priced ----------------
    conseq = D(12) * COD + D(60000)
    check("WE 11.4.1 twelve weeks of delay priced", D(12) * COD, 171360)
    check("WE 11.4.1 consequence total", conseq, 231360)
    CONS = D(48000)
    emv0 = D("0.35") * conseq
    emv1 = CONS + D("0.10") * conseq
    check("WE 11.4.1 EMV without consultation", emv0, D("80976"))
    check("WE 11.4.1 EMV with consultation", emv1, D("71136"))
    check("WE 11.4.1 consultation is worth", emv0 - emv1, 9840)
    check("WE 11.4.1 breakeven consultation cost", emv0 - D("0.10") * conseq, 57840)
    check("WE 11.4.1 required probability reduction, pp", CONS / conseq * 100,
          D("20.75"), D("0.005"))
    check("WE 11.4.1 probability it must reach", D("0.35") - CONS / conseq,
          D("0.1425"), D("0.00005"))

    # ---------------- WE 11.4.3 — verifying AI-drafted communications ----------------
    items = D(48) + D(24) + D(12) + D(12)
    check("WE 11.4.3 external communications a year", items, 96)
    save_h, ver_h = items * D("1.2"), items * D("0.5")
    check("WE 11.4.3 drafting hours saved", save_h, D("115.2"))
    check("WE 11.4.3 verification hours added", ver_h, D("48.0"))
    net_h = save_h - ver_h
    check("WE 11.4.3 net hours saved", net_h, D("67.2"))
    check("WE 11.4.3 net hours valued", net_h * RATE, 7392)
    check("WE 11.4.3 net hours per week over 48 weeks", net_h / 48, D("1.4"))
    # 48.0/115.2 = 41.666...%: tolerance widened to 0.05 for the one-decimal display.
    check("WE 11.4.3 verification share of the saving, % (1 dp display)",
          ver_h / save_h * 100, D("41.7"), D("0.05"))
    ver_cost = ver_h * RATE
    exposure = items * D("0.04") * D(6000)
    check("WE 11.4.3 verification cost", ver_cost, 5280)
    check("WE 11.4.3 expected defective items", items * D("0.04"), D("3.84"))
    check("WE 11.4.3 unverified exposure", exposure, 23040)
    check("WE 11.4.3 return on verification", exposure / ver_cost, D("4.36"), D("0.005"))
    check("WE 11.4.3 breakeven error rate, %", ver_cost / (items * D(6000)) * 100,
          D("0.92"), D("0.005"))
    check("WE 11.4.3 accuracy implied by the breakeven, %",
          100 - ver_cost / (items * D(6000)) * 100, D("99.08"), D("0.005"))
    check("MCQ 11.4-D A one hour per item", exposure / (items * D(1) * RATE),
          D("2.18"), D("0.005"))
    check("MCQ 11.4-D C inverted ratio", ver_cost / exposure, D("0.23"), D("0.005"))
    check("MCQ 11.4-D D net saving as the cost", exposure / (net_h * RATE),
          D("3.12"), D("0.005"))

    # ---------------- Advanced topics ----------------
    seats, need = D(9), D(6)
    check("11.A.1 two-thirds of nine seats", (seats * 2 / 3).to_integral_value(), need)
    check("11.A.1 smallest blocking coalition", seats - need + 1, 4)
    entries, changes = D(62), D(19)
    turn = changes / entries
    check("11.A.2 annual role-holder turnover, %", turn * 100, D("30.6"), D("0.05"))
    check("11.A.2 mean error, annual refresh, %", turn / 2 * 100, D("15.3"), D("0.05"))
    check("11.A.2 mean error, quarterly refresh, %", turn / 8 * 100, D("3.8"), D("0.05"))
    check("11.A.2 entries wrong under annual refresh", turn / 2 * entries, D("9.5"), D("0.05"))

    # ---------------- Case study B — the consultation held twice (energy) ----------------
    CODB = D(62000)
    check("Case B 22 weeks of delay priced", D(22) * CODB, 1364000)
    check("Case B 4 weeks of delay priced", D(4) * CODB, 248000)
    late = D(180000) + D(240000) + D(410000) + D(22) * CODB
    early = D(260000) + D(55000) + D(4) * CODB
    check("Case B total, consult after the decision", late, 2194000)
    check("Case B total, consult before the decision", early, 563000)
    check("Case B net saving", late - early, 1631000)
    check("Case B cost ratio", late / early, D("3.90"), D("0.005"))
    check("Case B incremental consultation spend", D(260000) - D(180000), 80000)
    avoided = (late - D(180000)) - (early - D(260000))
    check("Case B everything else avoided", avoided, 1711000)
    check("Case B avoided per extra consultation dollar", avoided / D(80000),
          D("21.39"), D("0.005"))
    check("Case B design-change escalation ratio", D(410000) / D(55000), D("7.45"), D("0.005"))

    # ---------------- Exercise 11.1 ----------------
    ne, hpc, cape, hube = 11, D(2), D(45), D(10)
    check("Ex 11.1 mesh channels", mesh(ne), 55)
    check("Ex 11.1 mesh hours", mesh(ne) * hpc, 110)
    check("Ex 11.1 mesh utilisation, %", mesh(ne) * hpc / cape * 100, D("244.4"), D("0.05"))
    check("Ex 11.1 routed hours", D(ne) * hpc + hube, 32)
    check("Ex 11.1 routed utilisation, %", (D(ne) * hpc + hube) / cape * 100,
          D("71.11"), D("0.005"))
    kk = 2
    while mesh(kk + 1) * hpc <= cape:
        kk += 1
    check("Ex 11.1 sustainable parties unrouted", kk, 7)
    check("Ex 11.1   hours there", mesh(kk) * hpc, 42)
    check("Ex 11.1   hours at one more", mesh(kk + 1) * hpc, 56)
    k3 = 1
    while D(k3 + 1) * hpc + hube <= cape:
        k3 += 1
    check("Ex 11.1 sustainable parties routed", k3, 17)
    check("Ex 11.1   hours there", D(k3) * hpc + hube, 44)
    check("Ex 11.1 routed boundary n <= 17.5", (cape - hube) / hpc, D("17.5"))
    check("Ex 11.1 annual hours saved", (mesh(ne) * hpc - (D(ne) * hpc + hube)) * 12, 936)
    check("Ex 11.1 common error n squared", D(ne) * D(ne), 121)

    # ---------------- Exercise 11.2 ----------------
    cap2 = D(600)
    S2 = {"A": D(5) * D(4), "B": D(3) * D(5), "C": D(4) * D(2), "D": D(2) * D(4),
          "E": D(3) * D(3)}
    ts2 = sum(S2.values())
    check("Ex 11.2 total salience", ts2, 60)
    check("Ex 11.2 hours per point", cap2 / ts2, 10)
    for k4, printed in (("A", 200), ("B", 150), ("C", 80), ("D", 80), ("E", 90)):
        check(f"Ex 11.2 salience allocation {k4}", cap2 / ts2 * S2[k4], printed)
    floorC, coreD = D(120), D(260)
    res2 = cap2 - floorC - coreD
    check("Ex 11.2 residual", res2, 220)
    rs2 = S2["A"] + S2["B"] + S2["E"]
    check("Ex 11.2 residual salience base", rs2, 44)
    check("Ex 11.2 residual hours per point", res2 / rs2, 5)
    for k4, printed in (("A", 100), ("B", 75), ("E", 45)):
        check(f"Ex 11.2 three-term allocation {k4}", res2 * S2[k4] / rs2, printed)
    check("Ex 11.2 three-term allocation sums to capacity",
          floorC + coreD + res2 * (S2["A"] + S2["B"] + S2["E"]) / rs2, cap2)
    sites, site_val = D(50), D(19200)
    extra = sites * D(10) / 100
    check("Ex 11.2 extra adopting sites", extra, 5)
    check("Ex 11.2 annual gain", extra * site_val, 96000)
    check("Ex 11.2 value per engagement hour", extra * site_val / coreD,
          D("369.23"), D("0.005"))
    check("Ex 11.2 core hours per site", coreD / sites, D("5.20"))

    # ---------------- Exercise 11.3 ----------------
    CODE = D(9500)
    e1 = ages(4, "1.5", 1)
    e2 = ages(2, "0.5", "0.5")
    for lab, got, printed in (("newest", e1[0], "2.5"), ("mean", e1[1], "4.5"),
                              ("blindness", e1[2], "6.5")):
        check(f"Ex 11.3 monthly {lab}", got, D(printed))
    for lab, got, printed in (("newest", e2[0], "1.0"), ("mean", e2[1], "2.0"),
                              ("blindness", e2[2], "3.0")):
        check(f"Ex 11.3 fortnightly {lab}", got, D(printed))
    for lab, x, y, wk, money in (("newest", e1[0], e2[0], "1.5", 14250),
                                 ("mean", e1[1], e2[1], "2.5", 23750),
                                 ("blindness", e1[2], e2[2], "3.5", 33250)):
        check(f"Ex 11.3 weeks saved on {lab}", x - y, D(wk))
        check(f"Ex 11.3 saving priced, {lab}", (x - y) * CODE, money)
    check("Ex 11.3 common error: P/2 omitted, priced", D(2) * CODE, 19000)

    # ---------------- Exercise 11.4 ----------------
    RVb3, RVs3 = D(1200000), D(820000) + D(130000)
    check("Ex 11.4 seller reservation value", RVs3, 950000)
    z3 = RVb3 - RVs3
    mid3 = (RVb3 + RVs3) / 2
    check("Ex 11.4 ZOPA width", z3, 250000)
    check("Ex 11.4 midpoint", mid3, 1075000)
    check("Ex 11.4 surplus each at midpoint", z3 / 2, 125000)
    set3 = D(1090000)
    check("Ex 11.4 buyer surplus at 1,090,000", RVb3 - set3, 110000)
    check("Ex 11.4 buyer share, %", (RVb3 - set3) / z3 * 100, D("44.0"), D("0.005"))
    check("Ex 11.4 seller share, %", (set3 - RVs3) / z3 * 100, D("56.0"), D("0.005"))
    check("Ex 11.4 value transferred vs midpoint", set3 - mid3, 15000)
    check("Ex 11.4 common error: ZOPA floor without forgone contribution", D(820000), 820000)

    # ---------------- Exercise 11.5 ----------------
    incurred = D(3) * COD + D(4) * D(5) * RATE
    check("Ex 11.5 three weeks of delay", D(3) * COD, 42840)
    check("Ex 11.5 escalation preparation", D(4) * D(5) * RATE, 2200)
    check("Ex 11.5 total incurred", incurred, 45040)
    fac_cost = D(6) * FAC + D(14) * RATE
    check("Ex 11.5 facilitator fee alone (the error)", D(6) * FAC, 900)
    check("Ex 11.5 participant time", D(14) * RATE, 1540)
    check("Ex 11.5 facilitation cost", fac_cost, 2440)
    check("Ex 11.5 ratio", incurred / fac_cost, D("18.46"), D("0.005"))
    check("Ex 11.5 breakeven success probability, %", fac_cost / incurred * 100,
          D("5.42"), D("0.005"))
