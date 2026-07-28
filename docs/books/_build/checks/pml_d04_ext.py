"""PML-AI Domain 4 (extension) — Integration and delivery architecture: golden checks.

Every number printed as a *result*, *substitution*, in-text figure, numeric MCQ option, exercise
answer or figure-spec value by the depth expansion of
`domain-04-integration-delivery-architecture.md` is recomputed here from its definition rather than
from its printed output. The domain's ORIGINAL arithmetic (the hundred-per-cent test on the five
level-2 elements, the mesh/layer interface economics, the assessed-total-impact stack, WE 4.3.2's
44-item audit, WE 4.3.3's 291,176 of drift and its cumulative-window derivation, Case study A's
558,000/216,000/342,000 and Case study B's `CPI` 0.87) lives in `verify_formulas.py` and is NOT
duplicated here. What is added are the new engines:

    plan-set audit  each of WE 4.1.2's findings priced from its own driver (person-days at a blended
                    day rate derived from Domain 14's hourly rate, elapsed weeks at Meridian's cost
                    of delay), the audit's own cost from the same day rate, the return ratio, and the
                    float-sensitivity variant. The three published MCQ 4.1-D distractors are each
                    recomputed from the named error that produces them, so a distractor that is not
                    the stated error fails here.

    cross-match     WE 4.2.2's coverage percentages from their counts, the exception classification
                    (raw = real + legitimate, checked as a partition), the false-positive rate, and
                    the DIRECTIONAL invariant the example turns on: removing orphan work WIDENS the
                    hundred-per-cent gap. That is proved as an inequality over the arithmetic, not
                    asserted, and MCQ 4.2-F's wrong-side distractor is recomputed.

    stale PV        WE 4.3.1 and Exercise 4.8 built from the earned-value definitions on both a
                    stale and a re-phased `PV`. Three invariants are PROVED rather than quoted:
                    (i) `CPI` and both CPI-only forecasts are byte-identical across all PV variants
                    — swept over a range of mis-phasings, not just the published two;
                    (ii) the composite `EAC` is LINEAR in `PV`, so its two published deltas are
                    equal and both equal sensitivity × mis-phasing, where the sensitivity is derived
                    as `(BAC − EV)/(CPI × EV)`;
                    (iii) deferral flatters nothing — it makes reported `SPI` worse than the truth —
                    which is the trap the exam-prep list now names.

    pairwise p^k    WE 4.3.2b, Exercise 4.5, Fig 4.3.1 and the industry-variation bullet. The
                    exponent law is checked at k = 1, 2, 3, the inversion `p = q^(1/k)` is verified
                    by round-tripping it back through `p^k` (not by comparing two printed numbers),
                    the floor-to-integer item tolerances are recomputed from the raw fractional
                    counts, the derived audit interval is rebuilt from the register's accumulation
                    rate, and the monotonic invariant `p^3 < p^2 < p` is swept over 0 < p < 1.
                    MCQ 4.3-F's linear-approximation distractor is recomputed and shown to overstate.

    reconciliation  WE 4.3.3b, Exercise 4.6 and Fig 4.3.2. Original + Σ approved = expected, the
                    residual, and the class decomposition — with the CONCEALMENT IDENTITY
                    `gross − |net| = 2 × min(class)` proved as algebra over a sweep of both classes
                    and both sign orders, since the manuscript now carries it out of the domain as a
                    general rule. Exercise 4.6's negative residual exercises the opposite sign.

    screening       WE 4.4.1 and Exercise 4.7: the error rate, both misclassification classes, the
                    gross, the per-error mean, the net, the corrected drift against WE 4.3.3's
                    original figure, the emergency-route share, and the same concealment identity on
                    wholly independent data. MCQ 4.4-F's three distractors are the two classes and
                    the net, each recomputed.

    rejections      WE 4.4.3: the recorded share, the recurrence rate, duplicated assessment, the
                    reversed decision, the mean-versus-modal ratio, and both published ratios of
                    loss to recording cost — with the tail-dominance invariant (mean > modal) proved
                    from the four-case distribution rather than quoted.

    case study A    the eight weeks of delay attributed to the nine unowned interfaces, priced at
                    the master cost of delay, per interface and as a share of the 31 required, tied
                    back to the p^k expectation.

Master-thread values are read from ctx and never re-typed: MERIDIAN_COST_OF_DELAY (USD 14,280/week),
MERIDIAN_BASELINE (USD 2,400,000 approved cost), AURIGA_BAC (USD 4,000,000). Auriga's week-13
`PV`/`EV`/`AC` and Domain 14's USD 110/hour blended rate are the two cross-domain values the new
material needs that ctx does not carry; each is declared once below and every derived figure
(the USD 880 person-day, the USD 27.50 log entry, the USD 1,320 annual reconciliation cost) is
computed from it rather than restated.
"""


from decimal import ROUND_HALF_UP


def run(ctx):
    check, D = ctx["check"], ctx["D"]

    def disp(x, n=0):
        """Round as the manuscript prints — half-up, not banker's. Used only where the
        manuscript prints a whole-dollar figure that the arithmetic produces as a fraction
        (the CPI-based and composite `EAC` forms, and the per-interface unit cost)."""
        return D(x).quantize(D(1).scaleb(-n), rounding=ROUND_HALF_UP)

    COD = ctx["MERIDIAN_COST_OF_DELAY"]        # USD 14,280/week (Domain 1)
    MB = ctx["MERIDIAN_BASELINE"]              # USD 2,400,000 approved cost (Domain 2)
    BAC = ctx["AURIGA_BAC"]                    # USD 4,000,000 (Domains 6-8)

    # Cross-domain values the new material imports. Declared once; everything else is derived.
    HOURLY = D(110)                            # Domain 14 KA 14.2.1 blended rate
    PV0, EV0, AC0 = D(2080000), D(1920000), D(2120000)   # Auriga week 13 (Domain 7 KA 7.3)

    # =====================================================================================
    # Rate derivations used throughout the new material
    # =====================================================================================
    day = HOURLY * 8
    check("WE 4.1.2 Setup — blended person-day = 110/hr x 8 hr", day, 880)
    check("WE 4.4.3 Setup — rejection entry = quarter hour at the blended rate",
          HOURLY / 4, D("27.50"))

    # =====================================================================================
    # KA 4.1.2 — WE 4.1.2, the plan-set consistency audit
    # =====================================================================================
    verif_labour = 46 * day
    check("WE 4.1.2 Result — unfunded verification labour (46 person-days)", verif_labour, 40480)

    cp_weeks = D(12) / 5                       # 12 working days over a 5-day week
    check("WE 4.1.2 Substitution — 12 working days in weeks", cp_weeks, D("2.4"))
    cp_expo = cp_weeks * COD
    check("WE 4.1.2 Result — critical-path verification exposure (2.4 weeks)", cp_expo, 34272)

    lead_gap_weeks = D(14) - 6                 # 14-week longest lead time vs 6 allowed
    proc_expo = lead_gap_weeks * COD
    check("WE 4.1.2 Result — procurement lead-time exposure (8-week gap)", proc_expo, 114240)

    risk_resp = D(118000)                      # 6 responses, stated plan value
    audit_total = verif_labour + risk_resp + proc_expo + cp_expo
    check("WE 4.1.2 Result — quantified total", audit_total, 306992)
    check("WE 4.1.2 Result — total as % of the approved baseline",
          audit_total / MB * 100, D("12.79"))

    audit_cost = 6 * day
    check("WE 4.1.2 Result — audit cost (6 person-days)", audit_cost, 5280)
    check("WE 4.1.2 Result — return ratio, exposure to audit cost",
          audit_total / audit_cost, D("58.14"))

    # WE 4.3.3's drift is an ORIGINAL figure; recomputed here from its own drivers so the
    # comparison ratio is not anchored on a copied literal.
    drift = 34 * D(6800) + 14 * D("0.3") * COD
    check("WE 4.3.3 drift, re-derived as the comparison base", drift, 291176)
    check("WE 4.1.2 Interpretation — audit finds more than a year of drift (ratio)",
          audit_total / drift, D("1.0543"))
    check("WE 4.1.2 Interpretation — the audit-vs-drift ratio exceeds one",
          1 if audit_total > drift else 0, 1)

    no_float = audit_total - proc_expo
    check("WE 4.1.2 Interpretation — total if the procured item has float", no_float, 192752)
    check("WE 4.1.2 Interpretation — that total as % of baseline", no_float / MB * 100, D("8.03"))
    check("WE 4.1.2 Interpretation — headline drop when the float variant applies (%)",
          proc_expo / audit_total * 100, D("37.21"))
    check("WE 4.1.2 Interpretation — the float variant still exceeds 5 % of baseline",
          1 if no_float / MB * 100 > 5 else 0, 1)

    # MCQ 4.1-D distractors, each rebuilt from the error the rationale names.
    check("MCQ 4.1-D distractor A — money-only classes", verif_labour + risk_resp, 158480)
    check("MCQ 4.1-D distractor B — omits critical-path verification exposure",
          verif_labour + risk_resp + proc_expo, 272720)
    check("MCQ 4.1-D distractor D — prices the whole 14-week lead time, not the 8-week gap",
          verif_labour + risk_resp + 14 * COD + cp_expo, 392672)

    # 4.1.2 commitment lead-time rule
    check("4.1.2 'Setting the horizon' — horizon shortfall in weeks (14 lead vs 12 horizon)",
          D(14) - 12, 2)

    # =====================================================================================
    # KA 4.2.2 — WE 4.2.2, the product-to-work cross-match
    # =====================================================================================
    PRODUCTS, PACKAGES = 148, 172
    check("WE 4.2.2 Setup — traceability matrix cells", PRODUCTS * PACKAGES, 25456)

    prod_orphan, wp_orphan = 12, 9
    raw = prod_orphan + wp_orphan
    check("WE 4.2.2 Substitution — raw exceptions", raw, 21)
    check("WE 4.2.2 Result — products with work", PRODUCTS - prod_orphan, 136)
    check("WE 4.2.2 Result — product coverage %",
          D(PRODUCTS - prod_orphan) / PRODUCTS * 100, D("91.89"))
    check("WE 4.2.2 Result — work packages with a product", PACKAGES - wp_orphan, 163)
    check("WE 4.2.2 Result — work-package coverage %",
          D(PACKAGES - wp_orphan) / PACKAGES * 100, D("94.77"))

    legit = 7 + 7                              # granularity artefacts + product-free work
    real = 5 + 2                               # omitted product set + genuine orphans
    check("WE 4.2.2 classification partitions the raw exceptions exactly", legit + real, raw)
    check("WE 4.2.2 Result — legitimate exceptions", legit, 14)
    check("WE 4.2.2 Result — real findings", raw - legit, 7)
    check("WE 4.2.2 Result — false-positive rate %", D(legit) / raw * 100, D("66.67"))

    omitted, orphan_budget = D(214000), D(26000)
    check("WE 4.2.2 Result — classified findings total", omitted + orphan_budget, 240000)
    check("WE 4.2.2 Result — findings as % of baseline",
          (omitted + orphan_budget) / MB * 100, D("10.00"))

    allocated0 = D(2332000)                    # WE 4.2.1's original level-2 sum
    allocated1 = allocated0 - orphan_budget
    check("WE 4.2.2 Interpretation — allocated cost after removing orphans", allocated1, 2306000)
    check("WE 4.2.2 Interpretation — widened unallocated gap", MB - allocated1, 94000)
    check("WE 4.2.2 Interpretation — widened gap as % of parent",
          (MB - allocated1) / MB * 100, D("3.92"))
    # The directional claim, proved rather than asserted: removing orphan work always widens the gap.
    check("WE 4.2.2 invariant — removing orphan work widens the 100 % gap",
          1 if (MB - allocated1) > (MB - allocated0) else 0, 1)
    check("MCQ 4.2-F distractor A — subtracts on the wrong side",
          (MB - allocated0) - orphan_budget, 42000)

    # =====================================================================================
    # KA 4.3.1 — WE 4.3.1, a stale time-phased baseline
    # =====================================================================================
    CPI_A = EV0 / AC0
    check("WE 4.3.1 Result — Auriga CPI (master thread, shown 0.91)", CPI_A, D("0.9057"))
    MIS = D(240000)

    def spi(pv):
        return EV0 / pv

    def composite(pv):
        return AC0 + (BAC - EV0) / (CPI_A * spi(pv))

    check("WE 4.3.1 Result — reported SPI on stale PV 2,080,000", spi(PV0), D("0.9231"))
    check("WE 4.3.1 Result — reported SV", EV0 - PV0, -160000)
    check("WE 4.3.1 Result — variant (a) true PV (deferral)", PV0 - MIS, 1840000)
    check("WE 4.3.1 Result — variant (a) SPI", spi(PV0 - MIS), D("1.0435"))
    check("WE 4.3.1 Result — variant (a) SV", EV0 - (PV0 - MIS), 80000)
    check("WE 4.3.1 Result — variant (b) true PV (acceleration)", PV0 + MIS, 2320000)
    check("WE 4.3.1 Result — variant (b) SPI", spi(PV0 + MIS), D("0.8276"))
    check("WE 4.3.1 Result — variant (b) SV", EV0 - (PV0 + MIS), -400000)
    check("WE 4.3.1 Result — SPI spread across the two variants",
          spi(PV0 - MIS) - spi(PV0 + MIS), D("0.2159"))

    check("WE 4.3.1 Result — EAC = BAC + AC - EV (all three columns)", BAC + AC0 - EV0, 4200000)
    check("WE 4.3.1 Result — EAC = BAC/CPI, printed to the whole dollar (display rounding)",
          disp(BAC / CPI_A), 4416667)
    check("WE 4.3.1 Result — composite EAC reported, whole dollar (display rounding)",
          disp(composite(PV0)), 4608056)
    check("WE 4.3.1 Result — composite EAC variant (a), whole dollar (display rounding)",
          disp(composite(PV0 - MIS)), 4320972)
    check("WE 4.3.1 Result — composite EAC variant (b), whole dollar (display rounding)",
          disp(composite(PV0 + MIS)), 4895139)

    # Invariant (i): every cost-only measure is untouched by PV, and the composite is not.
    # Each measure is rebuilt as a function of pv (so the sweep tests the FORMULA, not a literal),
    # then compared against the value the report published at the stale PV.
    def cpi_of(pv, _ev=EV0, _ac=AC0):
        return _ev / _ac                        # no pv term — the whole point

    def eac_a_of(pv):
        return BAC + AC0 - EV0                  # no pv term

    def eac_b_of(pv):
        return BAC / cpi_of(pv)                 # pv reaches it only through CPI, i.e. not at all

    for k in range(-6, 7):
        pv = PV0 + k * D(40000)
        check(f"WE 4.3.1 invariant — CPI(pv) equals the published 0.9057 at PV {pv}",
              cpi_of(pv), CPI_A, tol=D("1e-24"))
        check(f"WE 4.3.1 invariant — EAC(BAC+AC-EV)(pv) equals 4,200,000 at PV {pv}",
              eac_a_of(pv), D(4200000), tol=D("1e-24"))
        check(f"WE 4.3.1 invariant — EAC(BAC/CPI)(pv) equals the reported form at PV {pv}",
              disp(eac_b_of(pv)), D(4416667), tol=D("1e-24"))
        if k:
            check(f"WE 4.3.1 invariant — the composite DOES move at PV {pv}",
                  1 if composite(pv) != composite(PV0) else 0, 1)

    sens = (BAC - EV0) / (CPI_A * EV0)
    check("WE 4.3.1 Interpretation — composite sensitivity (BAC-EV)/(CPI x EV)", sens, D("1.1962"))
    check("WE 4.3.1 Interpretation — USD 1.20 per USD mis-phased (2 dp, display rounding)",
          disp(sens, 2), D("1.20"))
    check("WE 4.3.1 Interpretation — composite move for 240,000 mis-phased, whole dollar "
          "(display rounding)", disp(sens * MIS), 287083)
    check("WE 4.3.1 Interpretation — that move as % of BAC", sens * MIS / BAC * 100, D("7.18"))
    # Invariant (ii): linearity — the two deltas are equal and both equal sensitivity x mis-phasing.
    check("WE 4.3.1 invariant — deferral delta equals sensitivity x mis-phasing",
          composite(PV0) - composite(PV0 - MIS), sens * MIS, tol=D("1e-18"))
    check("WE 4.3.1 invariant — acceleration delta equals the same amount",
          composite(PV0 + MIS) - composite(PV0), sens * MIS, tol=D("1e-18"))
    # Invariant (iii): deferral does NOT flatter — reported SPI sits below the true SPI.
    check("WE 4.3.1 invariant — under deferral, reported SPI < true SPI",
          1 if spi(PV0) < spi(PV0 - MIS) else 0, 1)

    # =====================================================================================
    # KA 4.3.2 — WE 4.3.2b, pairwise configuration integrity; Fig 4.3.1
    # =====================================================================================
    ITEMS, CONFORM, IFACES = 340, 296, 31
    p = D(CONFORM) / ITEMS
    check("WE 4.3.2b Result — item conformance %", p * 100, D("87.06"))
    check("WE 4.3.2b Result — two-sided interface conformance p^2 %", p ** 2 * 100, D("75.79"))
    check("WE 4.3.2b Result — two-sided interface failure 1-p^2 %", (1 - p ** 2) * 100, D("24.21"))
    check("WE 4.3.2b Result — three-party conformance p^3 %", p ** 3 * 100, D("65.98"))
    check("WE 4.3.2b Result — three-party failure 1-p^3 %", (1 - p ** 3) * 100, D("34.02"))
    check("WE 4.3.2b Result — expected at-risk interfaces of 31",
          IFACES * (1 - p ** 2), D("7.50"))

    item_fail = D(ITEMS - CONFORM) / ITEMS
    check("WE 4.3.2 item failure rate, re-derived (44 of 340)", item_fail * 100, D("12.94"))
    check("WE 4.3.2b Result — interface failure is 1.87x item failure",
          (1 - p ** 2) / item_fail, D("1.87"))
    check("MCQ 4.3-F distractor C — linear approximation 2 x item rate", 2 * item_fail * 100,
          D("25.88"))
    check("MCQ 4.3-F distractor C overstates the true pairwise rate",
          1 if 2 * item_fail > (1 - p ** 2) else 0, 1)

    # Monotonicity of the exponent law: p^3 < p^2 < p strictly for 0 < p < 1.
    for hundredth in range(1, 100):
        pk = D(hundredth) / 100
        check(f"p^k invariant — p^3 < p^2 < p at p={pk}",
              1 if pk ** 3 < pk ** 2 < pk else 0, 1)

    # The inversion, verified by round-tripping rather than by comparing printed values.
    tol_1of31 = (D(IFACES - 1) / IFACES).sqrt()
    check("WE 4.3.2b Result — item conformance for one at-risk interface in 31 %",
          tol_1of31 * 100, D("98.37"))
    check("WE 4.3.2b inversion round-trips: 31 x (1 - p^2) = 1",
          IFACES * (1 - tol_1of31 ** 2), 1, tol=D("1e-20"))
    raw_items_1of31 = ITEMS * (1 - tol_1of31)
    check("WE 4.3.2b Result — non-conforming items permitted (raw, before flooring)",
          raw_items_1of31, D("5.5288"))
    check("WE 4.3.2b Result — non-conforming items permitted (floored)",
          int(raw_items_1of31), 5)

    p95 = D("0.95").sqrt()
    check("WE 4.3.2b Interpretation — item conformance for a 95 % interface target %",
          p95 * 100, D("97.47"))
    check("WE 4.3.2b inversion round-trips: p^2 = 0.95", p95 ** 2, D("0.95"), tol=D("1e-24"))
    raw_items_95 = ITEMS * (1 - p95)
    check("WE 4.3.2b Interpretation — items permitted at a 95 % target (raw)",
          raw_items_95, D("8.6090"))
    check("WE 4.3.2b Interpretation — items permitted at a 95 % target (floored)",
          int(raw_items_95), 8)
    check("WE 4.3.2b Interpretation — a 10 % item tolerance permits this interface failure %",
          (1 - D("0.90") ** 2) * 100, 19)

    per_month = D(ITEMS - CONFORM) / 6         # 44 non-conforming items over 6 months
    check("WE 4.3.2b Interpretation — register defect accumulation, items/month",
          per_month, D("7.33"))
    interval_months = 5 / per_month
    check("WE 4.3.2b Interpretation — derived audit interval, months",
          interval_months, D("0.68"))
    check("WE 4.3.2b Interpretation — derived audit interval, weeks (about 3)",
          interval_months * D(52) / 12, D("2.95"))

    # Fig 4.3.1's plotted markers are the same two values as the worked example.
    check("Fig 4.3.1 — Meridian marker, item axis %", p * 100, D("87.06"))
    check("Fig 4.3.1 — Meridian marker, p^2 axis %", p ** 2 * 100, D("75.79"))
    check("Fig 4.3.1 — 95 % interface target marker %", p95 * 100, D("97.47"))

    # =====================================================================================
    # KA 4.3.3 — WE 4.3.3b, original-to-current reconciliation; Fig 4.3.2
    # =====================================================================================
    small_changes = 34 * D(6800)
    check("WE 4.3.3b Setup — the 34 small changes, re-derived", small_changes, 231200)
    steering = D(86000) + D(142000) + D(57500)
    check("WE 4.3.3b Substitution — the three steering-approved changes", steering, 285500)
    approved = small_changes + steering
    check("WE 4.3.3b Result — approved changes total", approved, 516700)
    expected = MB + approved
    check("WE 4.3.3b Result — expected current baseline", expected, 2916700)

    reported_current = D(2948900)
    residual = reported_current - expected
    check("WE 4.3.3b Result — net residual", residual, 32200)
    check("WE 4.3.3b Result — net residual as % of the original", residual / MB * 100, D("1.34"))

    unposted = D(18400) + D(26500)             # approved changes with no baseline effect
    unreferenced = D(77100)                    # baseline movement with no change reference
    check("WE 4.3.3b Result — unposted-approval class", unposted, 44900)
    check("WE 4.3.3b Result — the decomposition closes on the residual",
          unreferenced - unposted, residual)
    gross = unposted + unreferenced
    check("WE 4.3.3b Result — gross error", gross, 122000)
    check("WE 4.3.3b Result — gross as % of the approved changes", gross / approved * 100, D("23.61"))
    check("WE 4.3.3b Result — gross as % of the original baseline", gross / MB * 100, D("5.08"))
    check("WE 4.3.3b Result — concealed by netting", gross - residual, 89800)
    check("WE 4.3.3b Interpretation — gross/net ratio", gross / residual, D("3.79"))
    check("WE 4.3.3b Interpretation — annual reconciliation cost, 1 hr/month",
          12 * HOURLY, 1320)

    # The concealment identity the domain now carries as a general rule, proved as algebra over a
    # sweep of both classes and BOTH sign orders (Exercise 4.6 exercises the negative residual).
    def conceal_ok(a, b):
        net = abs(a - b)
        return (a + b) - net == 2 * min(a, b)

    for a in range(1, 40):
        for b in range(1, 40):
            check(f"concealment identity — gross - |net| = 2 x min at ({a},{b})",
                  1 if conceal_ok(D(a) * 1000, D(b) * 1000) else 0, 1)
    check("WE 4.3.3b Interpretation — concealment equals twice the smaller class",
          2 * min(unposted, unreferenced), gross - residual)

    # Fig 4.3.2's bridge must walk to the reported current baseline.
    check("Fig 4.3.2 — bridge walks 2,400,000 + 516,700 = 2,916,700", MB + approved, 2916700)
    check("Fig 4.3.2 — bridge walks 2,916,700 + 77,100 - 44,900 = 2,948,900",
          expected + unreferenced - unposted, 2948900)
    check("Fig 4.3.2 — header net residual", residual, 32200)
    check("Fig 4.3.2 — header gross error", gross, 122000)
    check("Fig 4.3.2 — header hidden amount", gross - residual, 89800)

    # =====================================================================================
    # KA 4.4.1 — WE 4.4.1, screening misclassification in both directions
    # =====================================================================================
    ruled_changes, clarifications, defects = 51, 16, 9
    requests = ruled_changes + clarifications + defects
    check("WE 4.4.1 Setup — requests logged in year one", requests, 76)
    check("WE 4.4.1 Setup — the 51 changes split 37 approved / 14 refused", 37 + 14, ruled_changes)
    check("WE 4.4.1 Setup — the 37 approvals are 34 delegated + 3 steering", 34 + 3, 37)

    errors = 5 + 3
    check("WE 4.4.1 Substitution — misclassifications found", errors, 8)
    check("WE 4.4.1 Result — screening error rate %", D(errors) / requests * 100, D("10.53"))

    defect_as_change = 5 * D(6800)
    check("WE 4.4.1 Result — defects paid as changes", defect_as_change, 34000)
    check("WE 4.4.1 Result — that as % of WE 4.3.3's direct drift",
          defect_as_change / small_changes * 100, D("14.71"))

    direct = D(9200) + D(14600) + D(5400)
    check("WE 4.4.1 Substitution — direct work of the three mis-ruled changes", direct, 29200)
    sched = D("0.5") * COD
    check("WE 4.4.1 Substitution — schedule impact, 0.5 weeks at the cost of delay", sched, 7140)
    change_as_clar = direct + sched
    check("WE 4.4.1 Result — changes taken as clarifications", change_as_clar, 36340)

    gross_screen = defect_as_change + change_as_clar
    check("WE 4.4.1 Result — gross misclassification cost", gross_screen, 70340)
    check("WE 4.4.1 Result — cost per error", gross_screen / errors, D("8792.50"))
    net_screen = change_as_clar - defect_as_change
    check("WE 4.4.1 Result — net effect on the recorded change total", net_screen, 2340)
    check("WE 4.4.1 Result — net as % of baseline (printed 0.10)",
          net_screen / MB * 100, D("0.0975"))
    check("WE 4.4.1 Result — corrected drift", drift + net_screen, 293516)
    check("WE 4.4.1 Result — corrected drift as % of baseline",
          (drift + net_screen) / MB * 100, D("12.23"))
    check("WE 4.4.1 Interpretation — concealed by netting", gross_screen - net_screen, 68000)
    check("WE 4.4.1 Interpretation — concealment is twice the smaller class",
          2 * min(defect_as_change, change_as_clar), gross_screen - net_screen)
    ruling_cost = requests * HOURLY
    check("WE 4.4.1 Interpretation — recorded rulings across 76 requests", ruling_cost, 8360)
    check("WE 4.4.1 Interpretation — ratio of error to ruling cost",
          gross_screen / ruling_cost, D("8.41"))
    check("4.4.1 emergency-route note — share of requests %", D(5) / requests * 100, D("6.58"))
    # The asymmetry claim: the silent direction is the larger of the two on these figures.
    check("WE 4.4.1 invariant — the undetectable direction costs more here",
          1 if change_as_clar > defect_as_change else 0, 1)

    check("MCQ 4.4-F distractor A — defect-as-change class alone", defect_as_change, 34000)
    check("MCQ 4.4-F distractor B — change-as-clarification class alone", change_as_clar, 36340)
    check("MCQ 4.4-F distractor D — the net", net_screen, 2340)

    # =====================================================================================
    # KA 4.4.3 — WE 4.4.3, pricing untraced rejections
    # =====================================================================================
    refused, logged = 14, 5
    untraced = refused - logged
    check("WE 4.4.3 Setup — untraced rejections", untraced, 9)
    check("WE 4.4.3 Result — rejections recorded %", D(logged) / refused * 100, D("35.71"))
    recur = 4
    check("WE 4.4.3 Result — recurrence among the untraced %",
          D(recur) / untraced * 100, D("44.44"))

    assess = 5 * day
    check("WE 4.4.3 Setup — full impact assessment (5 person-days)", assess, 4400)
    duplicated = recur * assess
    check("WE 4.4.3 Result — duplicated assessment", duplicated, 17600)
    reversed_impact = D(63000)
    loss = duplicated + reversed_impact
    check("WE 4.4.3 Result — total loss", loss, 80600)
    check("WE 4.4.3 Result — mean loss per recurrence", loss / recur, 20150)
    recording = refused * (HOURLY / 4)
    check("WE 4.4.3 Result — recording all 14 rejections", recording, 385)
    check("WE 4.4.3 Interpretation — mean is this multiple of the modal case",
          (loss / recur) / assess, D("4.58"))
    check("WE 4.4.3 Interpretation — ratio excluding the reversed decision",
          duplicated / recording, D("45.71"))
    check("WE 4.4.3 Interpretation — ratio including it", loss / recording, D("209.35"))
    # Tail dominance, built from the four-case distribution rather than quoted.
    cases = [assess, assess, assess, assess + reversed_impact]
    check("WE 4.4.3 invariant — the four cases sum to the published total", sum(cases), loss)
    check("WE 4.4.3 invariant — the mean exceeds the modal case",
          1 if loss / recur > assess else 0, 1)

    # =====================================================================================
    # Case study A — pricing the nine unowned interfaces
    # =====================================================================================
    weeks_lost = 4 + 4                         # four-week late start + four of six overrun weeks
    cs_cost = weeks_lost * COD
    check("Case study A — eight weeks of delay at the cost of delay", cs_cost, 114240)
    check("Case study A — cost per unowned interface, whole dollar (display rounding)",
          disp(cs_cost / 9), 12693)
    check("Case study A — unowned share of the 31 required %", D(9) / IFACES * 100, D("29.03"))
    check("Case study A — p^k expectation of unverifiable interfaces",
          IFACES * (1 - p ** 2), D("7.50"))

    # =====================================================================================
    # Exercise 4.5 — pairwise integrity on a 500-item register
    # =====================================================================================
    e5_items, e5_conf = 500, 468
    pe = D(e5_conf) / e5_items
    check("Ex 4.5 — item conformance %", pe * 100, D("93.60"))
    check("Ex 4.5 — two-party conformance %", pe ** 2 * 100, D("87.6096"))
    check("Ex 4.5 — three-party conformance %", pe ** 3 * 100, D("82.0026"))
    e5_two = 44 - 6
    check("Ex 4.5 — two-sided interface count", e5_two, 38)
    a2 = e5_two * (1 - pe ** 2)
    a3 = 6 * (1 - pe ** 3)
    check("Ex 4.5 — expected at-risk two-party interfaces", a2, D("4.7084"))
    check("Ex 4.5 — expected at-risk three-party interfaces", a3, D("1.0798"))
    check("Ex 4.5 — expected at-risk total", a2 + a3, D("5.7882"))
    p98 = D("0.98").sqrt()
    check("Ex 4.5 — item conformance for a 98 % two-party target %", p98 * 100, D("98.99"))
    check("Ex 4.5 — inversion round-trips: p^2 = 0.98", p98 ** 2, D("0.98"), tol=D("1e-24"))
    check("Ex 4.5 — items permitted at that target (raw)", e5_items * (1 - p98), D("5.0253"))
    check("Ex 4.5 — items permitted at that target (floored)", int(e5_items * (1 - p98)), 5)
    check("Ex 4.5 — items currently non-conforming", e5_items - e5_conf, 32)
    naive = 44 * (1 - pe)
    check("Ex 4.5 — naive linear reading", naive, D("2.8160"))
    check("Ex 4.5 — the naive reading understates by %", (1 - naive / (a2 + a3)) * 100, D("51.35"))

    # =====================================================================================
    # Exercise 4.6 — reconciliation with a negative residual
    # =====================================================================================
    e6_orig, e6_appr, e6_tool = D(8600000), D(742300), D(9318600)
    e6_exp = e6_orig + e6_appr
    check("Ex 4.6 — expected current baseline", e6_exp, 9342300)
    e6_res = e6_tool - e6_exp
    check("Ex 4.6 — net residual (negative)", e6_res, -23700)
    e6_unposted, e6_unref = D(61400), D(37700)
    check("Ex 4.6 — decomposition closes on the residual", e6_unref - e6_unposted, e6_res)
    check("Ex 4.6 — net residual as % of the original", abs(e6_res) / e6_orig * 100, D("0.2756"))
    e6_gross = e6_unposted + e6_unref
    check("Ex 4.6 — gross error", e6_gross, 99100)
    check("Ex 4.6 — gross as % of the approved changes", e6_gross / e6_appr * 100, D("13.35"))
    check("Ex 4.6 — gross/net ratio", e6_gross / abs(e6_res), D("4.18"))
    check("Ex 4.6 — the identity holds with the signs reversed",
          e6_gross - abs(e6_res), 2 * min(e6_unposted, e6_unref))

    # =====================================================================================
    # Exercise 4.7 — screening misclassification on independent figures
    # =====================================================================================
    e7_req = 46 + 24 + 14
    check("Ex 4.7 — requests", e7_req, 84)
    e7_err = 7 + 4
    check("Ex 4.7 — errors", e7_err, 11)
    check("Ex 4.7 — error rate %", D(e7_err) / e7_req * 100, D("13.10"))
    e7_d2c = 7 * D(9400)
    check("Ex 4.7 — defects paid as changes", e7_d2c, 65800)
    e7_direct = D(12000) + D(8500) + D(6200) + D(15300)
    check("Ex 4.7 — direct work of the mis-ruled changes", e7_direct, 42000)
    e7_sched = D("0.75") * D(9500)
    check("Ex 4.7 — schedule impact, 0.75 weeks at 9,500", e7_sched, 7125)
    e7_c2c = e7_direct + e7_sched
    check("Ex 4.7 — changes taken as clarifications", e7_c2c, 49125)
    e7_gross = e7_d2c + e7_c2c
    check("Ex 4.7 — gross total", e7_gross, 114925)
    check("Ex 4.7 — cost per error", e7_gross / e7_err, D("10447.73"))
    e7_net = e7_c2c - e7_d2c
    check("Ex 4.7 — net effect (negative)", e7_net, -16675)
    check("Ex 4.7 — gross/net ratio", e7_gross / abs(e7_net), D("6.8921"))
    check("Ex 4.7 — twice the smaller class", 2 * min(e7_d2c, e7_c2c), 98250)
    check("Ex 4.7 — the concealment identity holds", e7_gross - abs(e7_net),
          2 * min(e7_d2c, e7_c2c))

    # =====================================================================================
    # Exercise 4.8 — stale PV on an independent project
    # =====================================================================================
    e8_bac, e8_pv, e8_ev, e8_ac, e8_mis = (D(12000000), D(6300000), D(5900000),
                                          D(6150000), D(450000))
    e8_cpi = e8_ev / e8_ac
    check("Ex 4.8 — reported SPI", e8_ev / e8_pv, D("0.9365"))
    check("Ex 4.8 — reported SV", e8_ev - e8_pv, -400000)
    check("Ex 4.8 — true PV", e8_pv - e8_mis, 5850000)
    check("Ex 4.8 — true SPI", e8_ev / (e8_pv - e8_mis), D("1.0085"))
    check("Ex 4.8 — true SV", e8_ev - (e8_pv - e8_mis), 50000)
    check("Ex 4.8 — CPI (both bases)", e8_cpi, D("0.9593"))
    check("Ex 4.8 — EAC = BAC + AC - EV", e8_bac + e8_ac - e8_ev, 12250000)
    check("Ex 4.8 — EAC = BAC/CPI, whole dollar (display rounding)", disp(e8_bac / e8_cpi), 12508475)

    def e8_comp(pv):
        return e8_ac + (e8_bac - e8_ev) / (e8_cpi * (e8_ev / pv))

    check("Ex 4.8 — composite EAC reported, whole dollar (display rounding)",
          disp(e8_comp(e8_pv)), 12939558)
    check("Ex 4.8 — composite EAC true, whole dollar (display rounding)",
          disp(e8_comp(e8_pv - e8_mis)), 12454589)
    check("Ex 4.8 — overstatement, whole dollar (display rounding)",
          disp(e8_comp(e8_pv) - e8_comp(e8_pv - e8_mis)), 484968)
    e8_sens = (e8_bac - e8_ev) / (e8_cpi * e8_ev)
    check("Ex 4.8 — composite sensitivity", e8_sens, D("1.0777"))
    check("Ex 4.8 — sensitivity x mis-phasing equals the overstatement",
          e8_sens * e8_mis, e8_comp(e8_pv) - e8_comp(e8_pv - e8_mis), tol=D("1e-18"))
    check("Ex 4.8 invariant — deferral makes reported performance look worse",
          1 if e8_ev / e8_pv < e8_ev / (e8_pv - e8_mis) else 0, 1)
