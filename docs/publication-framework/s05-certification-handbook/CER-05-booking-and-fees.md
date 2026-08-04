---
id: CER-05
series: S05
series_name: Certification Handbook
title: Booking, fees, rescheduling and cancellation
subtitle: Every date and every amount that governs a sitting, including the ones not yet settled
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, employer]
level: foundation
reading_time_min: 12
summary: >
  Payment opens a twelve-month window in which to schedule and sit; a slot may be chosen up to 365 days
  ahead; launch opens 15 minutes before the slot with a 30-minute grace period; a booking may be
  rescheduled free of charge up to 72 hours before, a maximum of three times, and locks 24 hours before
  the start. This document sets those dates out as one timeline, states the examination fee conflict
  rather than choosing a side, and is explicit about the refund, retake and mid-window reschedule terms
  that are not yet published.
linkedin:
  format: post
  hook: >
    Two published figures for our examination fee disagree. Our booking document prints both and picks
    neither, because the amount at checkout is the only one that can bind you.
  tags: [Certification, ProjectControls, Transparency, ProfessionalDevelopment]
  asset: one-pager
gated: false
related: [CER-01, CER-03, CER-06, CER-07]
sources:
  - "PCI platform booking, entitlement and pricing settings (backend/schema.sql site_settings and pricing_rules seeds), verified August 2026"
  - "PCI Candidate Handbook (docs/downloads/candidate-handbook.md), 2026"
  - "PCI Publication Framework — CANONICAL-FACTS.md §§4.1, 4.3, August 2026"
placeholders: 4
---

# Booking, fees, rescheduling and cancellation

> The dates that bind you, the amounts that bind you, and an honest account of which of them are still open.

**In one paragraph.** Payment opens a twelve-month window in which to schedule and sit; a slot may be
chosen up to 365 days ahead; launch opens 15 minutes before the slot with a 30-minute grace period; a
booking may be rescheduled free of charge up to 72 hours before, a maximum of three times, and locks 24
hours before the start. This document sets those dates out as one timeline, states the examination fee
conflict rather than choosing a side, and is explicit about the refund, retake and mid-window reschedule
terms that are not yet published.

**Who this is for.** Candidates planning a sitting around a working life; employers and sponsors
scheduling a cohort; and anyone who needs to know the exact moment a change stops being possible.

---

## 1. The dates, in one table

| Event | Rule |
|---|---|
| Scheduling deadline | **12 months** from payment. The entitlement expires on that date. |
| Booking horizon | A slot may be scheduled within a **365-day** window |
| Launch opens | **15 minutes** before the scheduled start |
| Grace period | **30 minutes** after the scheduled start |
| Free reschedule | Up to **72 hours** before the scheduled start |
| Maximum reschedules | **3** |
| Booking locks | **24 hours** before the scheduled start |

All times are in the timezone recorded on the booking. Record that timezone yourself when you book; a
candidate who travels between booking and sitting is the classic case of a slot missed by exactly the
number of hours nobody checked.

---

## 2. The two clocks

There are two, and confusing them is the most expensive mistake in this document.

**The entitlement clock** starts when you pay and runs for **12 months**. It governs whether you may hold
a booking at all. When it expires, the entitlement is spent whether or not you sat, and a fresh
examination fee is required.

**The booking clock** governs a single sitting: when launch opens, when the grace period ends, when the
free-reschedule cut-off passes and when the booking locks. It restarts every time you reschedule.

Rescheduling resets the booking clock. It does **not** extend the entitlement clock. Three reschedules,
each pushing a sitting a month later, will move you past a scheduling deadline that has not moved at all.

---

## 3. Before a booking will open

A booking will not open until your entitlement is valid, your profile is complete, a government-issued
photo identification document is on file, all seven policy consents are accepted at their current
version, and your account is in good standing. Those six conditions, the order in which they are checked
and how to clear each one are owned by `CER-03` §5.1.

Booking can also be closed platform-wide, in which case nothing you do to your own account will open it.

---

## 4. Fees

### 4.1 The examination fee

`[CONFIRM: exam fee — platform seeds USD 500, legacy pack states USD 350]`

Two published figures disagree, and this handbook prints both rather than choosing between them. Neither
is quoted here as the price. **The amount shown at checkout is the amount that binds you**, and it is the
only figure that governs a transaction.

### 4.2 What the fee is and is not

All fees are stated in **US dollars** and paid as a **single one-time payment**. The examination fee buys
examination registration access and opens the 12-month scheduling window. It does not buy a credential:
certification is a decision taken after eligibility, assessment, verification and policy requirements are
met, and no purchase confers it.

### 4.3 Fees are route-dependent and discountable

What you pay depends on the route you enter by and on any discount in force at the time. The founding
route waives the examination fee inside its window; sponsored, complimentary and partial-waiver
arrangements change who pays and how much (`CER-04` §§4 and 6). Discount codes exist, are
administrator-managed, and may be limited by product, by number of uses or to one use per person. None of
this changes the examination.

### 4.4 Membership and renewal

Membership is a **separate purchase on a separate ladder**, seeded in the platform at **USD 99**, with
renewal seeded at **USD 99**. Buying membership does not certify you and is not the examination fee.
`CER-01` §2.3 explains the distinction; `CER-08` covers what renewal maintains.

### 4.5 The retake fee

**No retake figure is quoted here.** The platform currently carries the retake product as inactive, and
the published policy is that the retake fee is confirmed and displayed **before any retake booking is
completed**. If you are offered a retake price by any other source, it is not ours.

---

## 5. Booking a sitting

Choose a date and time inside your window. Launch opens **15 minutes before** your scheduled start and
stays open for a **30-minute grace period**.

Treat the grace period as an insurance policy, not a plan. It exists so that a five-minute problem with a
camera driver does not cost you a sitting; it does not exist so that you can start half an hour late with
half an hour less composure. Candidates who launch in the early window and fail the readiness check still
have time to fix it. Candidates who launch at minute 29 do not.

If you do not launch within the grace period, the booking is **marked missed**. A documented incident —
a genuine technical failure, for instance — is considered under the examination administration policy;
open a support case with evidence, promptly, while the evidence still exists.

---

## 6. Rescheduling

### 6.1 The rule

You may reschedule **free of charge up to 72 hours before** your scheduled start, **up to three times**.
Bookings **lock 24 hours before** the start, after which the booking cannot be changed.

### 6.2 The gap between the two cut-offs

Between 72 hours and 24 hours before a sitting, the free-reschedule right has passed but the booking has
not yet locked. `[CONFIRM: whether a reschedule requested inside 72 hours but before the 24-hour lock is
permitted, and on what terms]`. Until that is published, plan on the 72-hour figure as your real deadline.

### 6.3 Reschedules are counted, not renewed

Three is the maximum across the entitlement, not three per booking. The count travels with you. Before you
move a sitting for convenience, ask whether you would rather still have that move available in eleven
months' time when the reason is not convenience.

---

## 7. Cancellation and refunds

`[CONFIRM: the published refund and cancellation terms — a refund policy consent is required of every
candidate before booking, but its terms are not stated in the platform settings]`.

Two things are confirmed and worth stating in the meantime. Accepting the refund policy is one of the
seven consents required before you may book, so you will have read the operative terms at their current
version before any money is at risk. And the scheduling window **cannot be extended**: if the 12-month
entitlement lapses without a sitting, a new examination fee is required. Lapse is not a cancellation and
does not create a refund.

---

## 8. Retakes

Not passing first time is not unusual, and the retake rules are built to be fair rather than punitive.

- **You may resit.** Eligibility is unchanged between attempts — you do not requalify to try again,
  subject only to conduct requirements.
- **An authorisation carries one attempt by default.** A further sitting requires a further
  authorisation.
- **A waiting period applies between attempts.** `[CONFIRM: the retake waiting period — the platform
  default is zero days while the published candidate policy describes a short waiting period]`.
- **The fee is confirmed before booking.** §4.5.
- **Confirmed misconduct may remove the right to resit.** See `CER-07` and `ETH-06`.

A resit is a complete, fresh examination drawn to the same blueprint and the same passing standard, so
memorising a previous sitting is neither possible nor useful. A pass after previous attempts confers
exactly the same credential as a first-time pass — there is no annotation, and no one can tell from the
credential how many attempts it took.

---

## 9. Worked example — one entitlement, one timeline

*Illustrative figures. The dates are invented to show how the two clocks interact; they describe no real
candidate.*

**The facts.** A candidate pays on **3 September 2026** and books a sitting for **14 May 2027 at 09:00**
in the timezone recorded on the booking.

| Milestone | Date and time | How it is derived |
|---|---|---|
| Payment | 3 Sep 2026 | — |
| Scheduling deadline | **3 Sep 2027** | Payment + 12 months |
| Free-reschedule cut-off | **11 May 2027, 09:00** | Scheduled start − 72 hours |
| Booking locks | **13 May 2027, 09:00** | Scheduled start − 24 hours |
| Launch opens | **14 May 2027, 08:45** | Scheduled start − 15 minutes |
| Grace period ends | **14 May 2027, 09:30** | Scheduled start + 30 minutes |

**The trap.** On 10 May the candidate reschedules — legitimately, free, and within the rules — to
**20 September 2027**. The booking clock restarts and every cut-off above recalculates. The scheduling
deadline does not move: it is still **3 September 2027**, seventeen days before the new sitting. The
entitlement expires before the examination happens, and a new examination fee is required.

**The assumption the answer depends on.** That the 12-month deadline runs from the payment date and is not
extended by rescheduling — which is the published rule. Check the deadline on your own account before
every reschedule, not after.

---

## 10. How this goes wrong

- **Rescheduling past the entitlement deadline.** §9. The single most expensive error available to a
  candidate, and it is invisible at the moment it is made.
- **Booking in one timezone and living in another.** The booking's recorded timezone governs.
- **Treating the grace period as a start window.** §5.
- **Spending reschedules on convenience.** Three, in total, for the whole entitlement. §6.3.
- **Planning around the 24-hour lock.** The lock is when change becomes impossible, not when it becomes
  free. The free deadline is 72 hours. §6.
- **Quoting a fee from an older document.** Two published figures disagree; checkout governs. §4.1.
- **Assuming a lapsed window will be extended on request.** It will not. §7.

---

## 11. The dates worksheet

Fill this in the day you pay, in your own calendar, with reminders — not in this document.

| Field | Your value |
|---|---|
| Payment date | |
| Scheduling deadline (payment + 12 months) | |
| Booked date, time and **timezone** | |
| Free-reschedule cut-off (start − 72 h) | |
| Booking lock (start − 24 h) | |
| Launch opens (start − 15 min) | |
| Reschedules used, of 3 | |
| Reminder set for 60 days before the scheduling deadline | |

---

## Related

- `CER-01 — Certification handbook — master` — the journey these dates sit inside
- `CER-03 — Eligibility, application and reasonable adjustments` — the six conditions that must clear before a booking opens, and why adjustments must be requested first
- `CER-06 — Online proctoring and the secure exam client` — what happens after launch
- `CER-07 — Results, scoring, appeals and complaints` — the outcome, and what to do if you dispute it

## Sources and standards

- PCI platform booking, entitlement and pricing settings — reschedule cut-off, maximum reschedules,
  launch window, grace period, scheduling deadline and seeded prices, verified August 2026.
- PCI Candidate Handbook (`docs/downloads/candidate-handbook.md`), 2026.
- PCI Publication Framework, `CANONICAL-FACTS.md` §§4.1 and 4.3, August 2026 — including the recorded
  conflict between the platform's seeded examination fee and the legacy candidate pack.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
