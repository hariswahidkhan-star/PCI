"""PML-AI Domain 12 — Leadership, teams and organizational behaviour: golden checks.

Every number printed as a result in `domain-12-leadership-teams-behaviour.md` is recomputed here
from its definition rather than from its printed output. Master-thread values come from ctx and are
never re-typed: `MERIDIAN_COST_OF_DELAY` (USD 14,280/week, Domain 1), `AURIGA_BAC`, and the shared
`mesh(n)` helper that Domain 4 registered for `n(n - 1)/2`. Auriga's cost of delay (USD 45,000/week,
Domain 6) and blended engineering rate (USD 130.625/hour = USD 5,225 per engineer-week, Domain 7
KA 7.4.1) are declared once below and reused.

The domain has five engines, each checked from first principles:

    attention      Σ recurring commitments against a stated working week; discretionary residual and
                   people-work capacity, and their shares of the week.
    coordination   paths = mesh(n); overhead = c x paths; share = c(n-1)/(2h), which collapses to
                   the identity (n-1)/160 at c = 0.5, h = 40 — asserted against the general form at
                   every printed n rather than assumed. Structured (pod) path counts are built from
                   the pod formula. Net capacity hn - c x mesh(n) is checked to be EQUAL at 80 and
                   81, and marginal capacity h - c(n-1) to be exactly zero at 81; the 50 % share
                   crossing is solved for c in {0.25, 0.5, 0.8, 1.0} rather than asserted.
    turnover       recruitment + ramp-up + lost productivity, with the ramp loss derived from the
                   summed effectiveness curve and the vacancy priced as the OVERTIME PREMIUM only
                   (the saved pay of a vacant post is not a cost). The saved-pay double count is
                   itself checked, because it is a published MCQ distractor. Breakeven overtime
                   premium and breakeven prevented-departures are solved, not asserted.
    span           coaching time per report (R - e(n - b))/n in minutes at every printed n; the
                   sustainable-span threshold (R + eb)/(T + e) solved algebraically and re-checked
                   by evaluating the curve either side of it; the naive 1/n curve carried in
                   parallel so the one-report overstatement is a computed difference.
    overlap        working windows converted to a common reference (UTC) and overlapped by
                   max(0, min(end) - max(start)); round-trip latency at zero overlap priced per
                   working day; the shift redesign re-checked against EVERY pair, so the collateral
                   loss of supplier overlap is a computed value.

Delegation economics is the sixth, smaller engine: two labour totals, the released-hour count, the
breakeven value of a released hour, and the breakeven error probability — all solved.
"""


def run(ctx):
    check, D, mesh = ctx["check"], ctx["D"], ctx["mesh"]
    COD_MER = ctx["MERIDIAN_COST_OF_DELAY"]          # USD 14,280 per week (Domain 1)
    BAC = ctx["AURIGA_BAC"]                          # USD 4,000,000

    RATE = D("130.625")                              # Auriga blended engineering rate, D7 KA 7.4.1
    EW = RATE * 40                                   # engineer-week = USD 5,225
    COD_AUR = D(45000)                               # Auriga cost of delay per week (Domain 6)
    check("D12 threads: Auriga engineer-week from blended rate", EW, 5225)
    check("D12 threads: Meridian benefit at 70 % adoption",
          D(40) * D("0.70") * 6 * 85 * 48, ctx["MERIDIAN_BENEFIT_70PCT"])

    # ---------- WE 12.1.4 — the leader's attention budget ----------
    WEEK = D("45.0")
    COMMIT = [D("6.0"), D("5.5"), D("4.0"), D("7.5"), D("5.0"), D("9.0")]
    fixed = sum(COMMIT)
    disc = WEEK - fixed
    R = disc - D("1.5")                              # people-work capacity
    check("WE 12.1.4 recurring commitments", fixed, "37.0")
    check("WE 12.1.4 committed share %", fixed / WEEK * 100, "82.2", D("0.05"))
    check("WE 12.1.4 discretionary attention h", disc, "8.0")
    check("WE 12.1.4 discretionary share %", disc / WEEK * 100, "17.8", D("0.05"))
    check("WE 12.1.4 people-work capacity h", R, "6.5")
    check("WE 12.1.4 people-work share %", R / WEEK * 100, "14.4", D("0.05"))
    check("WE 12.1.4 interruption share %", D("9.0") / WEEK * 100, "20.0")
    check("WE 12.1.4 uplift from recovering 3.0 h %", D("3.0") / R * 100, "46.2", D("0.05"))
    check("WE 12.1.4 recovered people-work capacity h", R + 3, "9.5")

    # ---------- WE 12.2.2 — coordination overhead ----------
    C, H = D("0.5"), D(40)

    def share(n, c=C, h=H):
        return c * mesh(n) / (h * D(n))

    for n, paths, hours, pct, fte in (
            (6, 15, "7.5", "3.125", "0.1875"),
            (12, 66, "33.0", "6.875", "0.825"),
            (25, 300, "150.0", "15.0", "3.75"),
            (40, 780, "390.0", "24.375", "9.75"),
            (81, 3240, "1620.0", "50.0", "40.5")):
        check(f"WE 12.2.2 paths n={n}", mesh(n), paths)
        check(f"WE 12.2.2 overhead h n={n}", C * mesh(n), hours)
        check(f"WE 12.2.2 share % n={n}", share(n) * 100, pct)
        check(f"WE 12.2.2 identity (n-1)/160 n={n}", D(n - 1) / 160 * 100, pct)
        check(f"WE 12.2.2 FTE n={n}", C * mesh(n) / H, fte)

    def podpaths(n, s=8):
        p = D(n) // D(s)
        return p * D(s) * (D(s) - 1) / 2 + p * (p - 1) / 2

    check("WE 12.2.2 pod paths 5x8", podpaths(40), 150)
    check("WE 12.2.2 pod overhead h", C * podpaths(40), "75.0")
    check("WE 12.2.2 pod share %", C * podpaths(40) / (H * 40) * 100, "4.6875")
    check("WE 12.2.2 pod FTE", C * podpaths(40) / H, "1.875")
    check("WE 12.2.2 pod share % at n=8", C * podpaths(8) / (H * 8) * 100, "4.375")
    check("WE 12.2.2 pod share % at n=88", C * podpaths(88) / (H * 88) * 100, "5.156", D("0.001"))
    check("WE 12.2.2 paths released", mesh(40) - podpaths(40), 630)
    check("WE 12.2.2 hours released", C * (mesh(40) - podpaths(40)), "315.0")
    check("WE 12.2.2 FTE released", C * (mesh(40) - podpaths(40)) / H, "7.875")
    check("WE 12.2.2 percentage points released",
          (share(40) - C * podpaths(40) / (H * 40)) * 100, "19.6875")

    def net(n):
        return H * D(n) - C * mesh(n)

    check("WE 12.2.2 net capacity h at n=80", net(80), 1620)
    check("WE 12.2.2 net capacity h at n=81", net(81), 1620)
    check("WE 12.2.2 net capacity equal at 80 and 81 (difference)", net(81) - net(80), 0)
    check("WE 12.2.2 net capacity FTE at the maximum", net(81) / H, "40.5")
    check("WE 12.2.2 marginal capacity of the 81st member h", H - C * D(80), 0)
    check("WE 12.2.2 marginal capacity of the 7th member h", H - C * D(6), "37.0")
    # 50 % share crossing solved for c: c(n-1)/(2h) = 1/2  ->  n = h/c + 1
    for c, n50, pct40 in (("0.25", 161, "12.1875"), ("0.5", 81, "24.375"),
                          ("0.8", 51, "39.0"), ("1.0", 41, "48.75")):
        c = D(c)
        check(f"WE 12.2.2 n at 50 % share, c={c}", H / c + 1, n50)
        check(f"WE 12.2.2 share % at n=40, c={c}", share(40, c=c) * 100, pct40)
    check("WE 12.2.2 Auriga coordination USD/week", C * mesh(6) * RATE, "979.6875", D("0.005"))
    check("WE 12.2.2 Auriga coordination USD over 25 weeks",
          C * mesh(6) * RATE * ctx["AURIGA_WEEKS"], "24492.1875", D("0.005"))
    check("WE 12.2.2 paths n=7", mesh(7), 21)
    check("WE 12.2.2 overhead h n=7", C * mesh(7), "10.5")

    # MCQ 12.2-A distractors, each the result of a named error
    check("MCQ 12.2-A correct share %", C * mesh(12) / (H * 12) * 100, "6.875")
    check("MCQ 12.2-A distractor one-side-only %",
          C / 2 * mesh(12) / (H * 12) * 100, "3.4375")
    check("MCQ 12.2-A distractor pairs counted twice %",
          C * D(12) * 11 / (H * 12) * 100, "13.75")
    check("MCQ 12.2-A distractor all on the leader %", C * mesh(12) / H * 100, "82.5")
    check("MCQ 12.2-B distractor n^2 path count (Ex 12.1)", D(25) ** 2, 625)

    # ---------- WE 12.2.4 — the full cost of one resignation (Auriga) ----------
    recruit = D(24000) + D(20) * RATE
    eff = [D("0.30") + D("0.10") * k for k in range(8)]
    check("WE 12.2.4 summed ramp effectiveness", sum(eff), "5.20")
    lost_weeks = D(8) - sum(eff)
    check("WE 12.2.4 ramp-up lost engineer-weeks", lost_weeks, "2.80")
    ramp = lost_weeks * EW + D(32) * RATE
    premium = D("0.50") * 6 * EW
    lostprod = D("0.8") * EW + D(12) * RATE + premium
    total = recruit + ramp + lostprod
    check("WE 12.2.4 recruitment USD", recruit, "26612.50")
    check("WE 12.2.4 ramp-up lost effectiveness USD", lost_weeks * EW, 14630)
    check("WE 12.2.4 ramp-up mentoring USD", D(32) * RATE, 4180)
    check("WE 12.2.4 ramp-up USD", ramp, "18810.00")
    check("WE 12.2.4 leaver disengagement USD", D("0.8") * EW, 4180)
    check("WE 12.2.4 handover USD", D(12) * RATE, "1567.50")
    check("WE 12.2.4 overtime premium USD", premium, 15675)
    check("WE 12.2.4 lost productivity USD", lostprod, "21422.50")
    check("WE 12.2.4 total USD", total, "66845.00")
    check("WE 12.2.4 total in engineer-weeks", total / EW, "12.79", D("0.005"))
    check("WE 12.2.4 annual charge-out value of the post USD", D(48) * EW, 250800)
    check("WE 12.2.4 total as share of annual post value %",
          total / (D(48) * EW) * 100, "26.65", D("0.005"))
    check("WE 12.2.4 total as share of BAC %", total / BAC * 100, "1.671", D("0.0005"))
    check("WE 12.2.4 two departures USD", 2 * total, 133690)
    check("WE 12.2.4 recruitment share of total %", recruit / total * 100, "39.8", D("0.05"))
    check("WE 12.2.4 ramp-up + lost productivity share %",
          (ramp + lostprod) / total * 100, "60.2", D("0.05"))
    check("WE 12.2.4 ramp-up share of total %", ramp / total * 100, "28.1", D("0.05"))
    check("WE 12.2.4 uncovered vacancy delay weeks", D(6) / 6, "1.0")
    check("WE 12.2.4 uncovered vacancy delay USD", D(6) / 6 * COD_AUR, 45000)
    check("WE 12.2.4 delay / premium ratio", COD_AUR / premium, "2.87", D("0.005"))
    check("WE 12.2.4 breakeven overtime premium %", COD_AUR / (D(6) * EW) * 100,
          "143.54", D("0.005"))
    check("WE 12.2.4 vacant weeks at full pay (the excluded figure)", D(6) * EW, 31350)
    check("MCQ 12.2-C distractor saved-pay double count USD",
          total - premium + D(6) * EW, "82520.00")
    check("MCQ 12.2-C distractor recruitment + ramp-up only USD", recruit + ramp, "45422.50")
    check("WE 12.2.4 breakeven departures prevented for USD 20,000",
          D(20000) / total, "0.2992", D("0.00005"))

    # ---------- WE 12.3.1 — delegation economics ----------
    LR, ER = D(185), D(140)
    sq_hours = D("3.5") * 12
    sq = sq_hours * LR
    del_hours = D("5.0") * 4 + D("3.5") * 8
    ov_hours = D("1.0") * 4 + D("0.5") * 8
    dele = del_hours * ER + ov_hours * LR
    extra = dele - sq
    released = sq_hours - ov_hours
    check("WE 12.3.1 status-quo leader hours", sq_hours, "42.0")
    check("WE 12.3.1 status-quo USD", sq, "7770.00")
    check("WE 12.3.1 delegate hours", del_hours, "48.0")
    check("WE 12.3.1 delegate USD", del_hours * ER, 6720)
    check("WE 12.3.1 retained oversight hours", ov_hours, "8.0")
    check("WE 12.3.1 oversight USD", ov_hours * LR, 1480)
    check("WE 12.3.1 delegated total USD", dele, "8200.00")
    check("WE 12.3.1 incremental direct cost USD", extra, "430.00")
    check("WE 12.3.1 released leader hours", released, "34.0")
    check("WE 12.3.1 breakeven value per released hour USD", extra / released,
          "12.65", D("0.005"))
    check("WE 12.3.1 breakeven as share of leader rate %",
          extra / released / LR * 100, "6.836", D("0.0005"))
    check("WE 12.3.1 task as share of people-work capacity %",
          D("3.5") / R * 100, "53.85", D("0.005"))
    check("WE 12.3.1 released hours in discretionary weeks", released / R, "5.23", D("0.005"))
    check("WE 12.3.1 released hours valued at leader rate USD", released * LR, 6290)
    check("WE 12.3.1 net benefit USD", released * LR - extra, 5860)
    check("WE 12.3.1 breakeven error probability %",
          (released * LR - extra) / D(6000) * 100, "97.67", D("0.005"))
    check("MCQ 12.3-B distractor divide by original hours USD",
          extra / sq_hours, "10.24", D("0.005"))

    # ---------- WE 12.3.2 — span of control and coaching capacity ----------
    E, B, T = D("0.5"), 6, D("0.75")           # 45 minutes = 0.75 h

    def per_report(n, r=R, e=E, b=B):
        return (r - e * (D(n) - b)) / D(n)

    for n, minutes in ((6, "65.00"), (7, "51.43"), (8, "41.25"), (9, "33.33"),
                       (10, "27.00"), (11, "21.82"), (12, "17.50"), (19, "0.00")):
        check(f"WE 12.3.2 coaching minutes per report n={n}",
              per_report(n) * 60, minutes, D("0.005"))
    threshold = (R + E * B) / (T + E)
    check("WE 12.3.2 sustainable-span threshold", threshold, "7.6")
    check("WE 12.3.2 threshold holds at n=7 (minutes >= 45)", per_report(7) * 60, "51.43",
          D("0.005"))
    check("WE 12.3.2 threshold fails at n=8 (minutes < 45)", per_report(8) * 60, "41.25")
    check("WE 12.3.2 span at which coaching capacity is zero", (R + E * B) / E, 19)
    check("WE 12.3.2 naive threshold R/T", R / T, "8.667", D("0.001"))
    check("WE 12.3.2 naive minutes at n=8", R / 8 * 60, "48.75")
    check("WE 12.3.2 naive minutes at n=9", R / 9 * 60, "43.33", D("0.005"))
    check("WE 12.3.2 overstatement in reports (8 - 7)", D(8) - 7, 1)
    check("WE 12.3.2 threshold with R raised to 9.5", (R + 3 + E * B) / (T + E), 10)
    check("WE 12.3.2 minutes per report at the raised threshold",
          per_report(10, r=R + 3) * 60, 45)

    # ---------- WE 12.4.1 — working-hour overlap (Meridian) ----------
    def window(offset, start=D(9), end=D(17)):     # local window converted to UTC
        return (start - D(str(offset)), end - D(str(offset)))

    def overlap(a, b):
        lo, hi = max(a[0], b[0]), min(a[1], b[1])
        return max(D(0), hi - lo)

    core, supplier, advisers = window(3), window("5.5"), window(-5)
    check("WE 12.4.1 core window start UTC", core[0], 6)
    check("WE 12.4.1 core window end UTC", core[1], 14)
    check("WE 12.4.1 supplier window start UTC", supplier[0], "3.5")
    check("WE 12.4.1 advisers window start UTC", advisers[0], 14)
    check("WE 12.4.1 core-supplier overlap h", overlap(core, supplier), "5.5")
    check("WE 12.4.1 core-advisers overlap h", overlap(core, advisers), 0)
    check("WE 12.4.1 supplier-advisers overlap h", overlap(supplier, advisers), 0)
    day = COD_MER / 5
    check("WE 12.4.1 cost of delay per working day USD", day, 2856)
    check("WE 12.4.1 clarification cost as-is USD", 3 * day, 8568)
    check("WE 12.4.1 annual cost as-is USD", 26 * 3 * day, 222768)
    core2, adv2 = (core[0] + 2, core[1] + 2), (advisers[0] - 1, advisers[1] - 1)
    check("WE 12.4.1 shifted core window start UTC", core2[0], 8)
    check("WE 12.4.1 shifted advisers window start UTC", adv2[0], 13)
    check("WE 12.4.1 overlap after redesign h", overlap(core2, adv2), "3.0")
    check("WE 12.4.1 core-supplier overlap after redesign h", overlap(core2, supplier), "3.5")
    check("WE 12.4.1 supplier overlap lost h", overlap(core, supplier) - overlap(core2, supplier),
          "2.0")
    check("WE 12.4.1 clarification cost after redesign USD", day, 2856)
    check("WE 12.4.1 annual cost after redesign USD", 26 * day, 74256)
    check("WE 12.4.1 annual saving USD", 26 * 3 * day - 26 * day, 148512)
    check("WE 12.4.1 saving per clarification USD", 3 * day - day, 5712)
    check("WE 12.4.1 unsocial burden core h/week", D(2) * 5, 10)
    check("WE 12.4.1 unsocial burden advisers h/week", D(1) * 5, 5)
    check("WE 12.4.1 unsocial burden unshared h/week", D(3) * 5, 15)
    check("WE 12.4.1 unsocial burden rotated h/week each", D(3) * 5 / 2, "7.5")
    # MCQ 12.4-A distractor: offset taken as 6 hours rather than 8
    check("MCQ 12.4-A distractor wrong offset overlap h",
          overlap(window(3), (D(9) + 3, D(17) + 3)), "2.0")

    # ---------- 12.A.2 — the cost of double membership ----------
    base_paths = podpaths(40)
    extra_paths = D(10) * 7
    tot_paths = base_paths + extra_paths
    check("12.A.2 extra paths from 10 double-membered", extra_paths, 70)
    check("12.A.2 total paths with double membership", tot_paths, 220)
    check("12.A.2 overhead h with double membership", C * tot_paths, "110.0")
    check("12.A.2 share % with double membership", C * tot_paths / (H * 40) * 100, "6.875")
    check("12.A.2 FTE with double membership", C * tot_paths / H, "2.75")
    check("12.A.2 extra hours h/week", C * extra_paths, "35.0")
    check("12.A.2 extra FTE", C * extra_paths / H, "0.875")
    check("12.A.2 percentage points added",
          (C * tot_paths / (H * 40) - C * base_paths / (H * 40)) * 100, "2.1875")
    check("12.A.2 equals the share of a 12-person mesh %", share(12) * 100, "6.875")

    # ---------- 12.A.1 — three departures during a recovery ----------
    check("12.A.1 three departures USD", 3 * total, 200535)

    # ---------- Case study A — Meridian, the programme that tripled ----------
    MPW = D(96) * 40                             # Meridian person-week
    check("Case A Meridian person-week USD", MPW, 3840)
    m_recruit = D(15000) + D(20) * D(96)
    m_ramp = D("2.80") * MPW + D(32) * D(96)
    m_lost = D("0.8") * MPW + D(12) * D(96) + D("0.5") * 6 * MPW
    m_total = m_recruit + m_ramp + m_lost
    check("Case A recruitment USD", m_recruit, 16920)
    check("Case A ramp-up USD", m_ramp, 13824)
    check("Case A lost productivity USD", m_lost, 15744)
    check("Case A cost per departure USD", m_total, 46488)
    check("Case A five departures USD", 5 * m_total, 232440)
    check("Case A unrecorded components USD", 5 * (m_ramp + m_lost), 147840)
    check("Case A unrecorded share %", (m_ramp + m_lost) / m_total * 100, "63.6", D("0.05"))
    check("Case A ramp-up across five departures USD", 5 * m_ramp, 69120)
    check("Case A released capacity USD/week", D("7.875") * 40 * D(96), 30240)
    check("Case A released capacity USD/year (48 weeks)", D("7.875") * 40 * D(96) * 48, 1451520)
    check("Case A realised acceleration USD", 6 * COD_MER, 85680)
    check("Case A theoretical / realised ratio",
          D("7.875") * 40 * D(96) * 48 / (6 * COD_MER), "16.94", D("0.005"))
    check("Case A eight more people: gross hours added", D(8) * H, 320)
    check("Case A eight more people: new coordination hours",
          C * sum(D(k) for k in range(40, 48)), "174.0")

    # ---------- Case study B — the conversation deferred ----------
    DR, MR, DD = D(115), D(165), D(28000)
    shortfall = D(3) * 14
    displaced = shortfall * D("4.5") * DR
    delivered = D(9) * 14
    excess_rw = (D("0.18") - D("0.06")) * delivered
    rework = excess_rw * 2 * DR
    delay_weeks = shortfall / 12
    delay = delay_weeks * DD
    conseq = displaced + rework + delay
    conv = D(3) * MR + D("1.5") * 6 * MR
    check("Case B shortfall in drawings", shortfall, 42)
    check("Case B displaced work USD", displaced, 21735)
    check("Case B drawings delivered", delivered, 126)
    check("Case B excess rework drawings", excess_rw, "15.12")
    check("Case B excess rework USD", rework, "3477.60")
    check("Case B delay weeks", delay_weeks, "3.5")
    check("Case B delay exposure USD", delay, 98000)
    check("Case B total consequence USD", conseq, "123212.60")
    check("Case B cost of the conversation USD", conv, 1980)
    check("Case B ratio", conseq / conv, "62.23", D("0.005"))
    check("Case B half-attribution consequence USD", conseq / 2, "61606.30")
    check("Case B half-attribution ratio", conseq / 2 / conv, "31.11", D("0.005"))
    check("Case B coaching capacity at 11 reports h", R - E * (D(11) - B), "4.0")
    check("Case B coaching minutes per report at 11", per_report(11) * 60, "21.82", D("0.005"))
    check("Case B coaching minutes per report after the split to 6",
          per_report(6) * 60, "65.00")

    # ---------- Exercise 12.1 — a 25-person team, and pods of five ----------
    check("Ex 12.1 mesh paths", mesh(25), 300)
    check("Ex 12.1 mesh overhead h", C * mesh(25), "150.0")
    check("Ex 12.1 capacity h", H * 25, 1000)
    check("Ex 12.1 mesh share %", C * mesh(25) / (H * 25) * 100, "15.0")
    ex1_pods = D(5) * mesh(5) + mesh(5)
    check("Ex 12.1 pod paths", ex1_pods, 60)
    check("Ex 12.1 pod overhead h", C * ex1_pods, "30.0")
    check("Ex 12.1 pod share %", C * ex1_pods / (H * 25) * 100, "3.0")
    check("Ex 12.1 hours released", C * (mesh(25) - ex1_pods), "120.0")
    check("Ex 12.1 FTE released", C * (mesh(25) - ex1_pods) / H, "3.0")
    check("Ex 12.1 percentage points released", C * (mesh(25) - ex1_pods) / (H * 25) * 100, "12.0")
    check("Ex 12.1 mesh share % at c=0.8", D("0.8") * mesh(25) / (H * 25) * 100, "24.0")
    check("Ex 12.1 pod share % at c=0.8", D("0.8") * ex1_pods / (H * 25) * 100, "4.8")

    # ---------- Exercise 12.2 — span with R = 7.0, e = 0.4, T = 40 minutes ----------
    R2, E2, B2, T2 = D("7.0"), D("0.4"), 5, D(2) / 3
    for n, minutes in ((5, "84.00"), (8, "43.50"), (9, "36.00")):
        check(f"Ex 12.2 coaching minutes per report n={n}",
              per_report(n, r=R2, e=E2, b=B2) * 60, minutes, D("0.005"))
    check("Ex 12.2 sustainable-span threshold", (R2 + E2 * B2) / (T2 + E2), "8.4375")
    check("Ex 12.2 span at which coaching capacity is zero", (R2 + E2 * B2) / E2, "22.5")
    check("Ex 12.2 naive minutes at n=9 (the wrong answer)", R2 / 9 * 60, "46.67", D("0.005"))

    # ---------- Exercise 12.3 — turnover on a five-person team ----------
    PW = D(92) * 40
    check("Ex 12.3 person-week USD", PW, 3680)
    e3_recruit = D(12500) + D(16) * D(92)
    eff3 = [D("0.40") + D("0.12") * k for k in range(6)]
    check("Ex 12.3 summed ramp effectiveness", sum(eff3), "4.20")
    lost3 = D(6) - sum(eff3)
    check("Ex 12.3 ramp-up lost person-weeks", lost3, "1.80")
    e3_ramp = lost3 * PW + D(18) * D(92)
    e3_prem = D("0.40") * 4 * PW
    e3_lost = D("0.9") * PW + D(10) * D(92) + e3_prem
    e3_total = e3_recruit + e3_ramp + e3_lost
    check("Ex 12.3 recruitment USD", e3_recruit, 13972)
    check("Ex 12.3 ramp-up lost effectiveness USD", lost3 * PW, 6624)
    check("Ex 12.3 ramp-up mentoring USD", D(18) * D(92), 1656)
    check("Ex 12.3 ramp-up USD", e3_ramp, 8280)
    check("Ex 12.3 disengagement USD", D("0.9") * PW, 3312)
    check("Ex 12.3 handover USD", D(10) * D(92), 920)
    check("Ex 12.3 overtime premium USD", e3_prem, 5888)
    check("Ex 12.3 lost productivity USD", e3_lost, 10120)
    check("Ex 12.3 total USD", e3_total, 32372)
    check("Ex 12.3 total in person-weeks", e3_total / PW, "8.80", D("0.005"))
    check("Ex 12.3 annual post value USD (46 weeks)", D(46) * PW, 169280)
    check("Ex 12.3 total as share of annual post value %",
          e3_total / (D(46) * PW) * 100, "19.12", D("0.005"))
    check("Ex 12.3 uncovered delay weeks", D(4) / 5, "0.8")
    check("Ex 12.3 uncovered delay USD", D(4) / 5 * D(22000), 17600)
    check("Ex 12.3 breakeven overtime premium %",
          D(4) / 5 * D(22000) / (D(4) * PW) * 100, "119.57", D("0.005"))
    check("Ex 12.3 saved-pay double count USD", e3_total - e3_prem + D(4) * PW, 41204)
    check("Ex 12.3 vacant weeks at full pay (the excluded figure)", D(4) * PW, 14720)

    # ---------- Exercise 12.4 — three groups, P/Q/R ----------
    P, Q, Rz = window(2), window(8), window(-6)
    check("Ex 12.4 P window start UTC", P[0], 7)
    check("Ex 12.4 Q window end UTC", Q[1], 9)
    check("Ex 12.4 R window start UTC", Rz[0], 15)
    check("Ex 12.4 P-Q overlap h", overlap(P, Q), "2.0")
    check("Ex 12.4 P-R overlap h", overlap(P, Rz), 0)
    check("Ex 12.4 Q-R overlap h", overlap(Q, Rz), 0)
    day4 = D(22000) / 5
    check("Ex 12.4 cost of delay per working day USD", day4, 4400)
    check("Ex 12.4 clarification cost as-is USD", 4 * day4, 17600)
    check("Ex 12.4 annual cost as-is USD", 18 * 4 * day4, 316800)
    P2, R2z = (P[0] + D("1.5"), P[1] + D("1.5")), (Rz[0] - D("1.5"), Rz[1] - D("1.5"))
    check("Ex 12.4 shifted P window start UTC", P2[0], "8.5")
    check("Ex 12.4 shifted R window start UTC", R2z[0], "13.5")
    check("Ex 12.4 P-R overlap after shift h", overlap(P2, R2z), "3.0")
    check("Ex 12.4 P-Q overlap after shift h", overlap(P2, Q), "0.5")
    check("Ex 12.4 annual cost after shift USD", 18 * day4, 79200)
    check("Ex 12.4 annual saving USD", 18 * 4 * day4 - 18 * day4, 237600)
    check("Ex 12.4 unsocial burden h/week each", D("1.5") * 5, "7.5")

    # ---------- Exercise 12.5 — delegation over 15 weeks ----------
    LR5, ER5 = D(175), D(120)
    sq5_h = D("4.0") * 15
    sq5 = sq5_h * LR5
    d5_h = D("6.0") * 5 + D("4.0") * 10
    o5_h = D("1.5") * 5 + D("0.5") * 10
    d5 = d5_h * ER5 + o5_h * LR5
    check("Ex 12.5 status-quo leader hours", sq5_h, "60.0")
    check("Ex 12.5 status-quo USD", sq5, "10500.00")
    check("Ex 12.5 delegate hours", d5_h, "70.0")
    check("Ex 12.5 delegate USD", d5_h * ER5, "8400.00")
    check("Ex 12.5 oversight hours", o5_h, "12.5")
    check("Ex 12.5 oversight USD", o5_h * LR5, "2187.50")
    check("Ex 12.5 delegated total USD", d5, "10587.50")
    check("Ex 12.5 incremental direct cost USD", d5 - sq5, "87.50")
    check("Ex 12.5 released leader hours", sq5_h - o5_h, "47.5")
    check("Ex 12.5 breakeven value per released hour USD",
          (d5 - sq5) / (sq5_h - o5_h), "1.8421", D("0.00005"))
    check("Ex 12.5 breakeven as share of leader rate %",
          (d5 - sq5) / (sq5_h - o5_h) / LR5 * 100, "1.053", D("0.0005"))
    check("Ex 12.5 released hours valued at leader rate USD", (sq5_h - o5_h) * LR5, "8312.50")
    check("Ex 12.5 net benefit USD", (sq5_h - o5_h) * LR5 - (d5 - sq5), "8225.00")
    check("Ex 12.5 breakeven error probability %",
          ((sq5_h - o5_h) * LR5 - (d5 - sq5)) / D(9000) * 100, "91.39", D("0.005"))
