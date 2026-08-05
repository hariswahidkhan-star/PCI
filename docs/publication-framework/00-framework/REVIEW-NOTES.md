# Review Notes — items flagged by authors for subject-matter-expert attention

Authors submit their assumptions and unresolved questions with every draft
(`GOVERNANCE-AND-REVIEW.md` §4). This file collects the items that need a decision from someone with
authority over the subject, rather than an editorial fix.

**Nothing here blocks a draft.** Each item blocks *approval* of the document it touches.

---

## 1. Technical claims needing subject-matter-expert review

### 1.1 `AIG-06` §8 — P80 below the sum of expected monetary values

**Flagged by:** the S02 author, during drafting.

An early draft asserted that a simulated P80 falling below the arithmetic sum of expected monetary values
indicates a modelling error. **That is false**, and the document was corrected before submission.

Where a single rare, high-impact risk dominates the register, the mean is lifted by a tail that the 80th
percentile never reaches, so P80 < ΣEMV is a legitimate result rather than a defect. The published text now
teaches it as a result to investigate and explain rather than a fault to fix.

**Why it is here:** the correction is believed right, but it is exactly the kind of claim that should not
rest on one author's judgement. A reviewer with quantitative risk experience should confirm the framing
before `AIG-06` moves past `in-review`. The underlying Domain 13 material treats simulation output more
confidently than this, which is itself worth a look.

---

## 2. Operator decisions the documents cannot make

These are recorded in `CANONICAL-FACTS.md` §9 and appear as `[CONFIRM: …]` placeholders in the affected
manuscripts. They need an owner, not an editor.

| # | Item | Affects | Why it cannot be resolved editorially |
|---|---|---|---|
| 2.1 | **Examination fee** — platform seeds USD 500, legacy candidate pack states USD 350 | `CER-01`, `CER-05` | Two published sources disagree on a price. Only the Institute can say which is real. |
| 2.2 | **CPD categories** — the portal serves five, the public pages describe four different ones | `CER-08` | A candidate reading the website and then using the portal meets two vocabularies. One surface has to change. |
| 2.3 | **Binding CPD hours** and **mandatory AI-currency hours** | `CER-01`, `CER-08` | 30 hours is a portal *target*; `cpd_required_hours` defaults to 0. The binding rule is unpublished. |
| 2.4 | **Retake waiting period** — platform default 0 days vs "a short waiting period applies" | `CER-05` | Policy contradiction between code and handbook. |
| 2.5 | **The reschedule dead zone** — free reschedule ends at 72 h, bookings lock at 24 h | `CER-05` | The published rules simply do not say what happens in the intervening 48 hours. A genuine gap. |
| 2.6 | **Refund and cancellation terms** | `CER-05` | Candidates must consent to a refund policy before booking, yet its terms exist nowhere in the platform settings. |
| 2.7 | **Two scheduling clocks** — 12-month payment-to-schedule entitlement vs 365-day booking horizon | `CER-05` | Rescheduling moves the booking without extending entitlement, so a candidate can hold a valid booking for a date their entitlement no longer covers. Needs a rule, not a caveat. |
| 2.8 | **Reasonable-adjustment notice period** | `CER-03` | Unpublished. |
| 2.9 | **Founding-route window status** — open, closing, or closed | `CER-04` | Time-sensitive and unstated. |
| 2.10 | **Appeal deadline, acknowledgement and decision timescales** | `CER-07` | Unpublished. An appeals process without timescales is not yet a process. |
| 2.11 | **Post-expiry grace and reinstatement requirements** | `CER-08` | Unpublished. |
| 2.12 | **Proctoring evidence retention** beyond the seeded 365 days | `CER-06` | Interacts with privacy obligations; not an editorial call. |
| 2.13 | **Honorary Fellow (PCI) and the FPCI membership grade** | `CER-04` | Whether the honorary designation interacts with the membership ladder is undecided anywhere. `CER-04` §5.3 explicitly declines to decide it. |
| 2.14 | **Credential level** — database records `Leader`, `certification.html` renders `Professional` | `CMP-02`, `CER-02` | A public page contradicts the system of record. |

---

## 3. Legacy source material carrying the retired credential code

Not defects in this framework — defects in the material it draws on. Authors were instructed to draw on
the substance without propagating the code, and **not** to edit these files.

| File | Issue |
|---|---|
| `docs/bok/` (all domain manuscripts, README, style spine, PDF) | Authored throughout as `PCP-AI` |
| `docs/publications/01`–`04` and the compiled PDF pack | `PCP-AI` in filenames, titles and body |
| `docs/downloads/candidate-ai-use-policy.md` | `PCP-AI` in the subtitle, "Who this is for" and §2 |
| `docs/downloads/examination-blueprint.md`, `candidate-handbook.md` | Retired naming and the legacy A/B/C domain-group names |

**Recommendation:** a separate, single-purpose change should rename these consistently. Doing it as part of
authoring would have mixed a mechanical rename into a content review, and would have made both harder to
check.

---

## 3a. Conventions the documents refused to settle

Where two conventions are genuinely in live use, the manuscripts name both and require the *project* to
declare which it uses, rather than the Institute quietly picking one. Recorded so a reviewer knows these
were decisions, not oversights.

| Convention | Where | Treatment |
|---|---|---|
| **Index rounding** — 2 dp (`docs/downloads/master-formula-sheet.md`) vs 3 dp (`EDITORIAL-STANDARD.md` §5) | `TPL-07` §3.4 | Both named; 3 dp recommended with a reason; the project must state its choice |
| **Whether contingency sits inside budget at completion** | `TPL-01`, `TPL-04`, `TPL-07`, `TPL-08` | Both conventions in live use; each template requires the project to state its own |
| **Schedule-quality thresholds** | `BPG-05`, `TPL-14` | Measurement supplied as arithmetic; the threshold is a parameter the project agrees and records. No third-party checklist reproduced, no threshold attributed to a published standard |
| **Concurrency in delay analysis** | `BPG-12` | Three competing approaches presented; which applies depends on contract, governing law and forum |

## 3b. Further items for the coordinator

1. **`CMP-10` title variance.** The registry row reads "Assessing competence — evidence, rubrics,
   moderation"; the manuscript uses "…rubrics and moderation". Align one to the other.
2. **American spelling inside a platform string.** The seeded PML-AI competency is
   *"Benefits realization"*. `CMP-05` renders it *realisation* for a British English series and says so
   openly. The platform string is the thing that should change.
3. **Word-count basis needs a ruling.** Several table- and clause-heavy documents (`AIG-08`, `AIG-10`,
   `AIG-11`, `CAR-07`, `CAR-08`, `SAL-02`) sit inside the length band on prose but above it on a raw
   `wc -w` that counts table pipes and list markers. A series lead should rule on which basis the ceiling
   is measured before anyone trims substance to hit a number.
4. **S07 depends on S08 being real.** `CAR-01` and `CAR-06` route every market question to `SAL-01` and
   `SAL-06` rather than estimating. If S08 slips, S07 publishes a promise it cannot redeem — the calendar
   sequencing (S07 in weeks 44–47, S08 in 48–50) should be reversed or the cross-references softened.
5. **The Body of Knowledge names commercial AI products** in KA 13.4.1's tool-category table. Series S02
   deliberately did not carry those forward, referring to capability classes instead. The BoK table is
   worth revisiting on its own terms.

## 4. Deliberate omissions

Things an author could have written and correctly did not.

| Omitted | Why |
|---|---|
| The on-screen calculator described in the legacy candidate pack | Not among the platform-confirmed examination settings. Claiming a tool that may not exist in the delivered exam is a candidate-harm risk. |
| Any credential-ID format | The schema's example still carries a retired code, so no format could be stated safely. |
| Any examination item count | None exists in the platform. A job-task analysis will set it. |
| Any salary, pay or market figure | The survey has not been run — the defining constraint of series S08. |
