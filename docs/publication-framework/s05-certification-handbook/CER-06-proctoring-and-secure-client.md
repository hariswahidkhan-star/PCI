---
id: CER-06
series: S05
series_name: Certification Handbook
title: Online proctoring and the secure exam client
subtitle: What is checked, what is watched, what is kept — and what the software honestly cannot do
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, employer]
level: practitioner
reading_time_min: 13
summary: >
  A PCI sitting is proctored: you complete a readiness check, an identity check against government-issued
  photo identification and a room scan, then sit in a locked-down environment that blocks a second monitor,
  detects prohibited applications and records integrity events against a server-owned clock. This document
  explains each step, states plainly what the downloadable secure client can and cannot enforce — it cannot
  block Ctrl+Alt+Del, and says so — describes what evidence is captured and kept, and explains why a result
  held for integrity review shows you no score at all until it is resolved.
linkedin:
  format: article
  hook: >
    Our secure exam client cannot block Ctrl+Alt+Del. We publish that in the candidate handbook rather than
    letting a candidate discover the gap between the marketing and the software.
  tags: [Certification, ExamSecurity, Proctoring, ResponsibleAI]
  asset: checklist-pdf
gated: false
related: [CER-01, CER-05, CER-07, EXB-07]
sources:
  - "PCI Secure Exam client documentation (secureexam/README-SECUREEXAM.md), 2026"
  - "PCI platform proctoring, evidence and result-publication settings (backend/schema.sql site_settings seeds), verified August 2026"
  - "PCI Publication Framework — CANONICAL-FACTS.md §4.1, August 2026"
placeholders: 1
---

# Online proctoring and the secure exam client

> The whole supervised sitting, described by someone who has read the source rather than the brochure.

**In one paragraph.** A PCI sitting is proctored: you complete a readiness check, an identity check
against government-issued photo identification and a room scan, then sit in a locked-down environment that
blocks a second monitor, detects prohibited applications and records integrity events against a
server-owned clock. This document explains each step, states plainly what the downloadable secure client
can and cannot enforce — it cannot block Ctrl+Alt+Del, and says so — describes what evidence is captured
and kept, and explains why a result held for integrity review shows you no score at all until it is
resolved.

**Who this is for.** Candidates preparing to sit; IT and information-security staff being asked to approve
the client on a corporate machine; and employers who want to know what their sponsored candidates are
consenting to.

---

## 1. What proctoring is for

Proctoring protects one thing: that a pass means the same for everyone who earns it. A credential that can
be obtained by a candidate with a second screen, an open browser or a colleague in the room is not worth
holding, and the people it damages most are the ones who passed honestly.

That is the whole justification, and it sets the limit of what the Institute may reasonably do. Proctoring
is entitled to establish that the right person sat the examination unaided. It is not entitled to anything
else about your life, your household or your machine.

---

## 2. Consent comes first

The **proctoring consent** is one of the seven policy consents you accept before you may book (`CER-03`
§5.2). Each acceptance is recorded with the policy version and the date. There is no sitting without it,
and there is no proctoring before it.

---

## 3. Before you launch

### 3.1 The readiness check

A short **system readiness check** confirms that your camera, microphone, network connection, full-screen
behaviour and environment are workable, and records the browser and screen configuration it found. **The
readiness check itself records nothing about you** — it establishes that the equipment works. Camera and
microphone access are requested to confirm they function and to support the proctored session that
follows.

Run it on the **actual machine, network and room** you will use, well before exam day. A check passed on
a personal laptop tells you nothing about a work machine with a managed security policy, a VPN and an
endpoint agent.

### 3.2 The identity check

Valid **government-issued photo identification** is required. The name on your booking must match it
**exactly, character for character**. Your face is captured and compared against the identification
document.

One property of that comparison deserves stating, because it is unusual: **the identity check never
fabricates a match.** Where the comparison cannot be made with confidence, the result is recorded as
*inconclusive* rather than resolved in either direction, and a human decides what happens next. An
inconclusive check is not an accusation and does not, by itself, stop you sitting.

### 3.3 The room scan

A **room scan** is required. You will be asked to show your working area. Clear it first: notes,
reference material, personal devices and second screens away, not merely out of shot. The scan exists to
establish the state of the room at the start, which protects you as much as it protects the examination.

---

## 4. During the sitting

### 4.1 The environment

The examination runs full screen in a secured environment. Copy, cut and paste are disabled. Leaving the
examination window or losing focus is **recorded as an integrity event** on your attempt. A **second
monitor** connected during the session is detected and blocked. Prohibited applications — browsers, chat,
screen-sharing, recording and virtual-machine tools — are detected before launch and while you sit.

You **cannot quit** the examination. You submit when you are ready, or it submits automatically when time
expires.

### 4.2 The clock belongs to the server

Your remaining time is issued by the server, not counted by your machine, and it is confirmed on a
regular heartbeat while you sit. Answers are saved continuously against that same heartbeat.

The practical consequence is the one candidates most need to hear: **a disconnection, a crash or a
restart does not lose your work and does not reset your clock.** You resume where you left off, with your
answers painted back and the correct remaining time. Nor can a slow or manipulated client buy extra
minutes — the time is the server's to give.

### 4.3 Talking to a proctor

A proctor supervises the session and can message you during it; you can reply. Messages travel on the
same heartbeat as everything else, so the channel works without a separate connection. If anything is
disrupted, **tell the proctor immediately**, follow their instructions, and report what happened to PCI
as soon as the session ends, while the detail is fresh and while the session record can be matched to
your account.

---

## 5. Two ways to sit, one set of rules

The examination is delivered **online with remote proctoring** — in the browser, or through the
downloadable **PCI Secure Exam** desktop client — or **at a test centre**. The content, the rules, the
timing and the scoring are identical on every route. Choosing the desktop client does not change what is
asked of you or how it is marked.

---

## 6. The secure exam client, described honestly

### 6.1 What it does

The desktop client is a kiosk application for Windows. It:

- runs a borderless, always-on-top window covering the whole virtual screen, with the system menu and the
  minimise and maximise controls removed;
- intercepts keystrokes at a low level and swallows the usual escapes — Alt+Tab, Alt+F4, Ctrl+Esc, the
  Windows key, PrintScreen and their relatives;
- checks running processes against a denylist of browsers, chat, screen-sharing, recording and
  virtual-machine tools, both before launch and repeatedly while you sit;
- detects a **second monitor connected mid-examination**;
- flags remote-desktop sessions and virtual-machine indicators;
- asks the operating system to **exclude the examination window from screen capture and broadcast**;
- captures webcam evidence at a regular interval and runs face detection continuously;
- monitors microphone level and voice likelihood;
- keeps a locally encrypted cache of your answers and any recorded violations, so a crash loses nothing;
- registers itself for the `pciexam://` link the portal uses to launch it, without needing an
  administrator or an installer.

### 6.2 What it cannot do, stated plainly

**The client cannot block Ctrl+Alt+Del.** The secure attention sequence is reserved by the operating
system, and intercepting it would require a signed kernel driver and operating-system notarisation. The
client does everything possible in user space and **degrades honestly**: where it cannot enforce
something, it logs and warns rather than pretending it has.

Three further limits, in the same spirit:

1. **Capture exclusion is best-effort.** The client asks Windows to exclude its window from capture. It
   cannot guarantee that a camera pointed at the screen, or hardware outside the operating system, sees
   nothing.
2. **Virtual-machine and remote-session detection is a hint, not proof.** It raises a flag for a human to
   consider. It does not decide anything on its own.
3. **The automated proctoring analysis is a baseline.** The default analyser is rule-based — no face,
   multiple faces, prolonged absence, loud audio, speech — and its findings are events for review, not
   verdicts. The Institute's governing principle applies to its own tooling: **AI proposes; the
   professional disposes.** No automated component fabricates a high-confidence identity match, and
   **no scoring ever happens on your machine**.

### 6.3 Why the client is pinned to PCI, and what that protects

The client will only talk to an approved PCI host. A launch link that names some other server is
**ignored**, and the client refuses to start against an untrusted host. The portal hands the client a
**short-lived, single-use launch code** rather than a session token, and that code is redeemed against the
pinned host.

That design is doing something for you, not only for the Institute: a link that arrives by email claiming
to start your examination cannot point the client at somebody else's server, and a launch code captured in
transit is worth nothing once it has been redeemed.

### 6.4 What IT departments usually ask

The client is a per-user application that registers a link handler for the current user; it needs a
camera, a microphone and outbound HTTPS to the pinned PCI host. It runs a machine-readiness self-test that
exits cleanly with a pass or fail, which is usually the fastest way to settle an approval conversation.
Run that self-test on the managed build before exam day, not on the morning.

---

## 7. Evidence: what is captured, and what happens to it

### 7.1 What is captured

- **Image evidence** — webcam snapshots at a regular interval, currently configured at 15 seconds, and
  the frames from your room scan. Both intervals and thresholds are administrator-configurable and may
  change; the principle does not.
- **Integrity events** — a timestamped timeline: no face detected, more than one face, an absence beyond
  a configured threshold (currently 8 seconds), loud audio, detected speech, focus loss, a prohibited
  process, a second display, a remote-session indicator.
- **The identity check result** — verified, inconclusive or failed, with its supporting images.
- **Session telemetry** — connection health, client type, answers and remaining time on each heartbeat.

The microphone is monitored to raise events about sound level and voice likelihood; the evidence stored
is image evidence.

### 7.2 How long it is kept

Proctoring evidence is retained for audit for a defined period and then purged automatically.
`[CONFIRM: the published proctoring-evidence retention period — the platform currently seeds 365 days]`.

Personal data, identity documents and proctoring evidence are handled under the Institute's privacy
notice — the fourth of the seven consents — and retained only as long as needed for certification and
audit.

### 7.3 Who sees it

Staff with the relevant permission, for the purpose of running and auditing examinations: a live
proctoring console during the sitting, and a session record afterwards showing the identity result, the
event timeline, the evidence gallery and the server-calculated score. Access is permissioned by role, and
privileged actions on a session are written to an audit log.

---

## 8. Evidence is audit-only by default

This is the point where PCI's configuration differs from what candidates expect, and it is in the
candidate's favour.

**Proctoring and identity evidence does not, by itself, delay or block your result.** The automatic rules
that could hard-block a result on a tampered attempt, a critical violation or a failed identity check are
**switched off by default**, deliberately, so that evidence is collected for audit and reviewed by people
rather than acted on automatically by a threshold. Immediate publication is the default (`CER-07` §2).

An event on your timeline is therefore not a penalty. It is a record that a human may look at.

---

## 9. When a result is held

Where there is a genuine question about the validity of an attempt, the result is **held** pending review.
While it is held:

- **you see no score;**
- **you see no pass or fail;**
- **you see no domain breakdown.**

You are told plainly that the result is pending and why. This is deliberate. Showing a provisional number
that may later be withdrawn is worse for the candidate than showing none: it invites a decision — telling
an employer, accepting a role, ordering business cards — that the review may reverse. A held result is
resolved by people, with a right of appeal against the outcome. `CER-07` §§4 and 6.

---

## 10. What is expected of you

- Work **alone**, in a quiet room you can close, on a **single screen**.
- Notes, reference material and personal devices **away**, not merely closed.
- A **second monitor physically disconnected**, not switched off.
- Government-issued photo identification **physically present**.
- The proctor's instructions followed throughout.
- **No AI tool, assistant or service of any kind**, on any device, for any purpose. In preparation AI is
  welcome; in the examination it is prohibited, and its use is misconduct.
- **No disclosure of examination content**, during or after the sitting.

---

## 11. How this goes wrong

- **Testing on the wrong machine.** The single most common cause of an exam-day failure, and entirely
  preventable. §3.1.
- **A second screen switched off but still connected.** Detection is of the connection, not the power
  state. §10.
- **A room "tidied" out of shot.** The scan establishes the state of the room; material pushed just
  outside the frame is exactly what a reviewer will ask about.
- **Panicking at a disconnection.** The clock belongs to the server and your answers are saved. Reconnect,
  resume, and report it afterwards. §4.2.
- **Treating an inconclusive identity check as a rejection.** It means the comparison was not confident
  enough to assert anything. A human decides. §3.2.
- **Assuming an integrity event has failed you.** Evidence is audit-only by default. §8.
- **Expecting a provisional score while a result is held.** There is none, on purpose. §9.
- **Installing the client for the first time on exam day.** Install and self-test in the week before, on
  the machine you will use, especially on a managed corporate build. §6.4.

---

## 12. The room checklist

Run this the evening before, in the room you will sit in.

- [ ] Readiness check completed **on this machine, this network, this room**
- [ ] Secure client installed and self-test passed, if you are using it
- [ ] Second monitor physically disconnected
- [ ] Desk clear: no notes, no reference material, no phone, no smartwatch
- [ ] Door closed, household informed of the time and the duration
- [ ] Government-issued photo identification on the desk, name matched to the booking
- [ ] Browsers, chat, screen-sharing and recording applications closed
- [ ] Power connected; laptop not relying on battery for 90 minutes
- [ ] Backup network known — a phone hotspot you have actually tested
- [ ] Booking time confirmed in the booking's recorded timezone
- [ ] Launch planned for the 15-minute early window, not the grace period

---

## Related

- `CER-01 — Certification handbook — master` — where the sitting fits in the whole journey
- `CER-05 — Booking, fees, rescheduling and cancellation` — the launch window and grace period referred to throughout
- `CER-07 — Results, scoring, appeals and complaints` — what happens to the result, held or otherwise
- `EXB-07 — Examination security and integrity` — the item-bank and scheme-level security this sits inside

## Sources and standards

- PCI Secure Exam client documentation (`secureexam/README-SECUREEXAM.md`), 2026 — including its published
  capability statement on user-space lockdown and the secure attention sequence.
- PCI platform proctoring, evidence and result-publication settings, verified August 2026 — identity
  check, room scan, second-monitor block, evidence interval, absence threshold, and the default-off
  automatic result-blocking rules described in §8.
- PCI Publication Framework, `CANONICAL-FACTS.md` §4.1, August 2026.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
