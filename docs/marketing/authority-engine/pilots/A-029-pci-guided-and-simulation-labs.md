# A-029 — Guided labs and simulation labs: how PCI candidates practise real project control

**1. Ledger ID and fact-check status** — `A-029` · Cluster 8 · Pillar. Status: **DRAFT** ·
`[LIVE-SITE VERIFICATION PENDING]`. Facts from `FACT_DOSSIER.md` §7. Official product name used
throughout: **PCI AI Project Controls Simulation Lab**. No external claim.

**2. SEO title** — `Inside PCI's Guided & Simulation Labs` (37 chars)

**3. H1** — Guided labs and simulation labs: how PCI candidates practise real project control

**4. Slug** — `pci-guided-and-simulation-labs`

**5. Meta description** (153 chars) — What actually happens inside PCI's guided labs and simulation labs: the exercises, how grading works, what the AI coach refuses to do, and who can access them.

**6. Primary keyword** — `pci simulation lab`
**Supporting cluster** — guided labs · project controls practice · skill drill · capstone · deterministic grading · portfolio evidence
**Semantic entities** — guided lab, skill drill, scenario, capstone, team scenario, work breakdown structure, earned value, cash flow, change control, deterministic grading, AI coach

**7. Search intent and target reader** — Informational, consideration. A candidate or a capability lead asking whether "labs" means real practice or a marketing word for a quiz.

**8. Featured-snippet answer capsule** (67 words)

> The PCI AI Project Controls Simulation Lab provides five kinds of practice artifact: guided labs, skill drills, scenarios, capstones and team exercises. Guided labs cover work breakdown structures, earned value, scheduling, cost structures, progress measurement, change control and cash-flow forecasting. Grading is deterministic and the answer key is never stored — it is derived at grade time — and the AI coach critiques method without computing answers.

---

## 9–14. Article (~1,400 words)

"Hands-on" is the most abused word in professional education. It usually means a multiple-choice
quiz with a screenshot. This page describes what PCI's labs actually consist of, including two
design decisions that make them harder — and more useful — than they would otherwise be.

## Five kinds of practice, not one

The **PCI AI Project Controls Simulation Lab** distinguishes five artifact types, and the difference
matters when you are planning your time:

| Type | What it is | When to use it |
|---|---|---|
| **Guided lab** | A complete exercise on one competence, with structure and feedback | Learning a technique properly for the first time |
| **Skill drill** | A short, repeatable exercise on one mechanic | Building speed and accuracy once you know the method |
| **Scenario** | A project situation with constraints that forces a decision | Testing judgement, not calculation |
| **Capstone** | A full project run under constraint | Proving you can hold everything at once |
| **Team** | A scenario involving other people, including disagreement | The thing solo exercises cannot assess |

Exercises are banded **foundation → intermediate → advanced → expert**, and most guided labs are
designed to run in roughly a quarter of an hour. That is deliberate: the constraint keeps them
sittable in a lunch break rather than requiring a cleared weekend.

## What the guided labs actually cover

The seeded catalogue maps directly onto the competences the credentials examine:

- **Work breakdown structure** — building a WBS that reconciles with how the work is actually let and delivered
- **Earned value** — computing EV, CPI and SPI from raw project data rather than from a worked example
- **Schedule development** — turning scope into logic-driven sequence
- **Cost breakdown structure** — structuring cost so it ties to the WBS instead of fighting it
- **Progress measurement** — arriving at a percent complete you could defend to an auditor
- **Pareto analysis** — finding the vital few drivers in a cost or delay dataset
- **Change control** — moving a change through a control process, with the consequences that follow
- **Cash-flow forecasting** — building a cash curve from a schedule and a cost structure

Skill drills sit alongside them on earned value, forecasting and risk — the mechanics you want to be
able to execute without thinking, so that your thinking is available for the judgement.

## The two design decisions that make it real

### 1. Grading is deterministic, and the answer key is never stored

The key does not exist as a stored artefact. It is **derived at grade time from the data you were
given**. Two consequences follow, and both are the point:

- **You cannot pattern-match.** There is no leaked key, because there is nothing to leak.
- **Marking is consistent.** The same work gets the same mark on a Tuesday as it does on a Friday,
  which is not true of most human-marked practice.

### 2. The AI coach never computes numbers

It will tell you your method is wrong. It will not tell you the answer.

This is the single most common complaint from candidates and the single strongest feature of the
system. Being told *"your progress measurement is mixing two methods across the same work
package"* forces you to find the number yourself. Being handed the number teaches you nothing you
will still have in six weeks.

It reflects PCI's whole stance — *AI proposes. The professional disposes.* A coach that computed for
you would model the opposite behaviour to the one the credentials examine.

## A worked example of what "defensible" means

Take progress measurement, the lab that catches the most people. An illustrative situation:

A work package worth **USD 400,000** has three activities. The site reports:

- Activity A: complete
- Activity B: "about 70% done"
- Activity C: not started

The tempting arithmetic: two of three activities substantially done → call the package ~57%.

The defensible version asks three questions first:

1. **What is each activity worth?** If A is USD 40,000, B is USD 60,000 and C is USD 300,000, then
   the package is `(40,000 + 0.7 × 60,000) ÷ 400,000` = **20.5%**, not 57%. Activity count is not
   value.
2. **What method was agreed for B?** "About 70%" is an opinion unless the method — units complete,
   milestones, level of effort — was fixed in advance. Mixed methods inside one package are how
   percent complete quietly becomes fiction.
3. **Could a stranger reproduce it?** If the answer depends on asking the person who reported it,
   it is not a measurement.

*Illustrative example — the figures are invented to demonstrate the mechanic.* The lab version gives
you the raw data and marks the reasoning, which is why people find it harder than a quiz and
remember it longer.

## Turning lab work into evidence

Practice that vanishes when you close the tab is a hobby. Lab work produces two durable things:

**Interview material.** "I have used earned value" is unfalsifiable and everyone says it. "I worked a
progress-measurement exercise where the activity-count answer was 57% and the value-weighted answer
was 20.5%, and the lesson was that mixed measurement methods inside a package destroy the number" is
a specific claim about your judgement that an interviewer can probe.

**A PCI World Passport entry**, if you choose to publish it. The Passport is *"a page of verified
practice evidence that its owner controls entirely"* — you decide what appears, you can rotate,
expire or withdraw the link, and answers are never published under any setting. Its own mandated
disclaimer is unambiguous: a Passport *"is not, by itself, a PCI certification, examination result,
accreditation, licence, or guarantee of professional competence."* It is evidence of practice, which
is a real thing and a different thing.

## Who can access the labs

Access is governed by the setting `simlab_requires`, currently `membership_or_exam` — access follows
either an active membership or an examination entitlement. `[LIVE-SITE VERIFICATION PENDING]` —
confirm the current rule and any published access period before publication.

## Student FAQ

**Are these the exam questions?** No. Labs rehearse the reasoning the examination assesses; they are
not the examination's items.

**Will the coach give me the answer if I am stuck?** No. It critiques your method. It never computes
numbers.

**Can I see the answer key?** There is no stored key — it is derived at grade time from your data.

**How long does a guided lab take?** Most are designed around a quarter of an hour.

**Where should a beginner start?** Foundation band, guided labs before skill drills — learn the
method, then build speed.

**Is lab work assessed for certification?** No. Certification is decided by the examination. Labs are
practice, and optionally evidence.

**Do labs appear on my Passport?** Only if you choose to publish them; publication is consent at
every level and reversible.

---

**15. CTA** — Pick the lab that matches the number you least want to defend in a meeting, and work
it. [Explore PCI's certifications and practice](https://projectcontrolsinstitute.org/certifications).

**16. Explore the PCI ecosystem**
- [Project Controls Institute](https://projectcontrolsinstitute.org/) — certifications, standards, verification
- [PCI World](https://pciworld.org/) — challenges, community and the PCI World Passport
- [MyPCI](https://mypci.org/) — applications, exams, results, renewal

*(Three domains — see `MISSING_INFO_APPROVALS.md` M3.)*

**17. External source list** — None.

**18. Internal-link map**

| Target | Anchor | Placement |
|---|---|---|
| A-030 (lab work → portfolio) | "Interview material" | evidence section |
| A-031 (decision cycle) | "forces a decision" | artifact table |
| A-032 (PCI World/Passport) | "PCI World Passport entry" | evidence section |
| A-011 (PCL-AI) | "the competences the credentials examine" | coverage section |
| A-014 (prep plan) | "planning your time" | artifact table |
| A-247 (progress measurement) | "progress measurement" | worked example |

**19. Recommended schema** — `Article` + `FAQPage` + `BreadcrumbList`. Not `Course`: these are
practice artifacts inside a platform, not an enrollable course with published instances — using
`Course` markup would misdescribe them.

**20. JSON-LD draft**

```json
{
  "@context": "https://schema.org",
  "@type": "Article",
  "headline": "Guided labs and simulation labs: how PCI candidates practise real project control",
  "about": { "@type": "Thing", "name": "PCI AI Project Controls Simulation Lab" },
  "publisher": { "@type": "Organization", "name": "Project Controls Institute Global, Inc." },
  "inLanguage": "en-GB"
}
```

**21. Clean HTML** — generated by `_build/md_to_html.py`.

**22. Editorial notes**
- Product name is **PCI AI Project Controls Simulation Lab** — never "simulation lap", and never
  "sim lab" in published copy.
- The worked example carries its **Illustrative example** label. Keep it.
- Labs are never described as assessed-for-certification. That line must not soften.
- The Passport disclaimer is quoted verbatim; do not paraphrase.
- `[LIVE-SITE VERIFICATION PENDING]` on the access rule and catalogue contents.

---

## §11 Platform repurposing package

### LinkedIn — company post
COPY START
A work package worth USD 400,000. Three activities. Site reports: A complete, B "about 70%", C not started.

Tempting answer: two of three substantially done, call it 57%.

Now ask what each activity is worth. A is 40k, B is 60k, C is 300k.

(40,000 + 0.7 × 60,000) ÷ 400,000 = 20.5%.

Not 57%. Activity count is not value — and that gap is where a lot of "on track" reporting quietly dies.

Two more questions before that 20.5% is defensible: was B's measurement method fixed in advance, and could a stranger reproduce the number from the records alone?

This is one exercise from PCI's guided labs. Grading is deterministic, the answer key is never stored — it's derived from your data at grade time — and the AI coach will tell you your method is wrong without telling you the answer.

(Figures illustrative.)
COPY END
Link: https://projectcontrolsinstitute.org/certifications
Hashtags: #ProjectControls #EarnedValue #CostEngineering #ProgressMeasurement

### LinkedIn — personal/expert post
COPY START
The most expensive two words in project reporting: "about seventy".

Nobody lies. The foreman genuinely thinks it's about seventy. The problem is that "about seventy" of what — activities, hours, units, value — was never fixed, so the number means something different to everyone who reads it.

I once watched a package reported at 57% complete come in at 20.5% on a value-weighted basis. Same facts, same people, no dishonesty anywhere. Two of three activities were done; they were just the cheap two.

The fix isn't more reporting. It's deciding the measurement method before the work starts, and being able to hand your records to a stranger who can rebuild the number without asking you anything.
COPY END

### LinkedIn — newsletter teaser
COPY START
57% or 20.5%? Same work package, same site report, no dishonesty. This week: why activity count isn't value, and what "defensible" actually requires.
COPY END

### X — standalone
COPY START
Work package, 400k, three activities.
A done (40k), B "about 70%" (60k), C not started (300k).

Activity count says ~57% complete.
Value says (40k + 42k)/400k = 20.5%.

Nobody lied. The two finished activities were just the cheap ones.
(Illustrative figures.)
COPY END

### X — thread (6)
COPY START
1/ How a package reported at 57% complete is actually at 20.5%, with nobody lying. (Illustrative figures.)

2/ Package worth USD 400,000. Three activities. Site reports A complete, B "about 70%", C not started. Two of three substantially done → feels like ~57%.

3/ Now weight by value. A = 40k. B = 60k. C = 300k. (40,000 + 0.7 × 60,000) ÷ 400,000 = 20.5%.

4/ The two finished activities were the cheap ones. Activity count is not value, and "number of things done" is the most common accidental measurement method in the industry.

5/ Second problem: "about 70%" is an opinion unless the method — units, milestones, level of effort — was fixed BEFORE the work started. Mixed methods inside one package are how percent complete becomes fiction.

6/ Test for defensibility: could a stranger reproduce your number from the records alone? If they have to ask the person who reported it, it isn't a measurement. It's a memory.
COPY END

### Facebook
COPY START
Why "we're 57% complete" and "we're 20.5% complete" can both be honest descriptions of the same work package.

If two of three activities are done but they're the two cheap ones, counting activities flatters you badly. Weight by value instead: a 400k package where the finished work is worth 40k and a 60k activity is 70% done is at 20.5%, not 57%.

Then ask the harder question: could someone who wasn't there rebuild that number from your records alone?

(Illustrative figures.)
COPY END

### Instagram caption
COPY START
57% or 20.5%? 📊
Same package. Same report. Nobody lied.
Activity count ≠ value.
(Illustrative figures.)
COPY END

### Instagram carousel (8 slides)
1. **Same package. Two answers.** 57% or 20.5%?
2. **Package: USD 400,000.** Three activities
3. **A: complete · B: "about 70%" · C: not started**
4. **Count activities** → feels like ~57% ✅✅⬜
5. **Weight by value:** A=40k · B=60k · C=**300k**
6. **(40k + 42k) ÷ 400k = 20.5%**
7. **The finished ones were the cheap ones**
8. **Could a stranger rebuild your number?** If not, it's a memory · *illustrative figures*

### Threads
COPY START
"Could a stranger reproduce this number from the records alone?"

If they'd have to ask the person who reported it, you don't have a measurement. You have a memory with a percentage sign on it.
COPY END

### YouTube — 60-second Short
COPY START
[0-8] Here's how a work package reported at 57% complete is really at 20.5%, with nobody lying. Figures are illustrative.
[8-20] Package worth four hundred thousand. Three activities. A is complete, B is "about seventy percent", C hasn't started. Two of three substantially done — feels like fifty-seven percent.
[20-36] Now weight by value. A is worth forty thousand. B is sixty. C is three hundred. Forty thousand plus seventy percent of sixty thousand, over four hundred thousand. Twenty point five percent.
[36-48] The two finished activities were the cheap ones. Counting activities is not measuring value — and it's the most common accidental measurement method in the industry.
[48-60] Then the real test: could a stranger rebuild that number from your records alone? If they'd have to ask you, it isn't a measurement.
COPY END

### YouTube — long-form outline (7 min)
Title: `Why Your Percent Complete Is Probably Wrong`
Chapters: 00:00 The honest 57% · 01:00 Weighting by value · 02:30 "About seventy" and mixed methods · 04:00 The stranger test · 05:15 Fixing it before work starts · 06:15 Practising it in the labs
Pinned comment: The worked figures in text, clearly labelled illustrative, plus the three defensibility questions.

### TikTok/Reels
COPY START
Two of three activities done = 57%? Only if they cost the same.
They didn't. Real answer: 20.5%.
Nobody lied. (Illustrative figures.)
COPY END

### Medium/Substack
Lead with the progress-measurement worked example as a standalone piece on measurement honesty; the labs enter only in the final section. Canonical to the PCI original. Keep the illustrative label and the Passport disclaimer.

### Quora
COPY START
Because "percent complete" is a family of different measurements wearing one name.

Illustrative case: a USD 400,000 work package with three activities. A is complete, B is "about 70%", C hasn't started. Count activities and you feel about 57% done. Weight by value — A is 40k, B is 60k, C is 300k — and you get (40,000 + 0.7 × 60,000) ÷ 400,000 = 20.5%.

Nobody was dishonest. The two finished activities were simply the cheap ones.

Two further problems worth knowing. First, "about 70%" isn't a measurement unless the method — units complete, milestones, level of effort — was fixed before the work started; mixed methods inside one package are how a number quietly becomes fiction. Second, the defensibility test: could a stranger reproduce your percentage from the records alone? If they'd have to ask the person who reported it, it's a memory, not a measurement.

Disclosure: I work with PCI, whose guided labs drill exactly this.
COPY END

### Reddit/community
COPY START
**Disclosure: I work with PCI. Illustrative figures, no link.**

Recurring source of "how did we lose 30% overnight" — activity-count progress vs value-weighted progress.

400k package, three activities. A complete, B ~70%, C not started. Counting: ~57%. Weighting (A=40k, B=60k, C=300k): (40k + 42k)/400k = **20.5%**.

Nobody lied. The finished ones were cheap.

Two follow-ups that matter more than the arithmetic:
- Was B's measurement method (units / milestones / LOE) fixed *before* work started? If not, "70%" is an opinion.
- Could someone who wasn't there rebuild the number from records alone?

Curious how many people here have inherited packages where the method was never actually agreed.
COPY END

### Email newsletter
**Subject:** 57% or 20.5%? Both honest.
**Preview:** Activity count is not value
COPY START
A USD 400,000 work package, three activities: A complete, B "about 70%", C not started. Counting activities suggests around 57% complete. Weighting by value — A worth 40k, B worth 60k, C worth 300k — gives (40,000 + 0.7 × 60,000) ÷ 400,000 = 20.5%.

Nobody lied; the finished activities were the cheap ones. Two questions make the smaller number defensible: was B's measurement method fixed before work started, and could a stranger rebuild the figure from the records alone? If they'd have to ask the reporter, it isn't a measurement.

(Figures illustrative.)
COPY END

### Google Business post
Eligible only if profile and feature are available — confirm first.

### Employer/university excerpt
COPY START
For capability leads: PCI's Simulation Lab provides guided labs, skill drills, scenarios, capstones and team exercises across WBS, earned value, scheduling, cost structures, progress measurement, change control and cash-flow forecasting. Grading is deterministic with no stored answer key, and the AI coach critiques method without computing answers. Lab work is practice and optional evidence — it is not assessed for certification, which is decided solely by examination.
COPY END

## §13 Carousel and image system

**Carousel** — 8 slides above; arithmetic is the hero; slide 8 carries the illustrative label.
**Design brief** — 1080×1350, safe margins 80px, PCI palette/fonts/logo only, figures ≥64px, contrast ≥4.5:1.
**OG image (1200×630)** — "57% vs 20.5%" with the two calculations beneath. Alt: "Comparison of activity-count and value-weighted progress for the same work package."
**Hero** — a site engineer with a tablet and a printed package breakdown, checking work against a list. Alt: "Engineer checking completed work against a printed work package breakdown."
**Diagram** — three activity bars sized by value, with the completed ones shaded, showing visually why counting misleads. Alt: "Bar diagram showing three activities sized by value with the completed low-value activities shaded."
**Image prompt** — "Documentary photograph of a project engineer in a hi-vis vest holding a tablet and a printed document, checking progress on an industrial site, overcast daylight, muted palette, shallow depth of field, 3:2. Negative: no legible text, no logos, no certificates, no readable screens, no robots, no glowing brains, no posed thumbs-up."
