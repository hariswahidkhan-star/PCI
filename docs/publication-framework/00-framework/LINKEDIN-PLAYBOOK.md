# LinkedIn Playbook

How a document in this framework becomes public teaching, without becoming a lie on the way.

The tonal precedent is `docs/marketing/linkedin-launch-20-posts.md` — twenty launch posts in the
Institute's voice. This playbook generalises what made them work so the remaining hundred documents can
follow the same pattern.

---

## 1. The two rules that override everything

**1. The post must be true standing alone.** A hook that works only because the reader has not yet reached
the caveat is a defect, not a technique. If the honest version of the hook is less exciting, publish the
honest version.

**2. The teaching goes in the post, not behind the click.** We publish; we do not tease. A post that says
"most people get EAC wrong — download to find out" gives nothing and takes something. A post that shows
the three EAC methods on one data set and lands the point has already paid the reader, and the download
becomes a courtesy rather than a toll.

Everything below is subordinate to these two.

---

## 2. Why the Institute posts at all

Not for reach. For a specific, narrow purpose: to demonstrate that the standard behind the credential is
real, and that the people who wrote it know the work. Every post is evidence for that claim or it is
noise.

This has a practical consequence. **The Institute's account teaches in the Institute's voice** — the
authority comes from the content, not from a personal brand. That is deliberate: it means the programme
does not depend on any individual, and it means a post is judged on whether it is correct.

---

## 3. Format selection

The `linkedin.format` field in every document's front matter picks one of five.

| Format | Use when | Shape | Source document becomes |
|---|---|---|---|
| `post` | One idea, one number, one takeaway | 120–250 words, one calculation, soft CTA, 3–5 tags | A single section, extracted |
| `carousel` | A structure the eye should walk through | 8–12 frames, one idea per frame, readable without sound or context | A list, taxonomy or process, one item per frame |
| `article` | A method that needs its reasoning shown | 800–1,400 words on-platform | The document, tightened — not pasted whole |
| `newsletter` | A series the reader should subscribe to | Same as article, but committed to a cadence | Sequential documents in one series |
| `document` | The artefact *is* the value | PDF upload, 6–15 pages | The document itself, typeset |

**Do not upgrade a format to chase reach.** A template posted as a carousel it does not suit is worse than
a plain document post that is what it says it is.

---

## 4. The post pattern that works

From the launch pack, generalised:

```
1. The claim, stated flat.              "Your CPI can look great and still be wrong."
2. The concrete situation.              Named quantities, one control account, no story.
3. The arithmetic, shown.               Both cases, substituted, with the result.
4. The consequence.                     What the wrong number does next month.
5. The principle, in one line.          The sentence a reader can quote back.
6. Soft CTA.                            One line, no urgency, no scarcity.
7. Three to five tags.
```

**Never step 1 without steps 2 and 3.** An assertion without arithmetic is an opinion, and the feed is
already full of those.

### 4.1 Hook patterns that are honest

| Pattern | Example shape |
|---|---|
| The measurement that misleads | "This indicator is flattering you, and here is the arithmetic" |
| The false choice | "Three teams, same data, three forecasts. All three are correct." |
| The definition that matters | "This is not delay. It is disruption, and the distinction decides the claim." |
| The sequencing correction | "You cannot fix this with a dashboard. The problem is upstream of the dashboard." |
| The quiet admission | "Here is what our own blueprint has not fixed yet, and why." |

### 4.2 Hook patterns that are banned

Engagement bait ("Agree?"), false scarcity, manufactured controversy, the humble-brag origin story,
"most professionals don't know", anything that withholds the point to force a comment, and any hook whose
claim the document does not actually support.

---

## 5. Adapting long to short without lying

A 3,000-word guide does not compress. It **decomposes**.

- Choose **one** section. Post that section properly rather than the whole document badly.
- Carry the assumptions with the number. If the worked example assumed a stable burn rate, the post says
  so — that clause is the difference between teaching and misinformation.
- Keep every `*Illustrative figures.*` label. It survives the trip to the feed.
- Never let compression turn a qualified claim into an absolute one. "Often" does not become "always"
  because the shorter sentence reads better.
- A guide can yield several posts over months. That is the intended economics of the library: 100
  documents are a year of teaching, not a year of announcements.

---

## 6. The call to action

One line. No urgency, no scarcity, no countdown.

- Ungated document → *"Free template in the comments."*
- Gated document → *"The candidate pack is on the site."*
- Neither → nothing. A post is allowed to end.

**Link discipline:** the link goes in the first comment, and the post stands without it.

**Gating discipline:** the default is ungated. Only the candidate handbook (`CER-01`), the annual report
template (`SAL-05`) and three templates (`TPL-06`, `TPL-14`, `TPL-15`) are gated, and every one of them is
marked `gated: true` in the registry. Teaching is never gated. If a document teaches, it is free.

---

## 7. Tags

Three to five, PascalCase, no spaces. Draw from a stable set so the archive stays coherent:

`#ProjectControls` `#CostEngineering` `#EarnedValue` `#ProjectManagement` `#Scheduling` `#RiskManagement`
`#ProjectFinance` `#Forecasting` `#Estimating` `#ConstructionClaims` `#EPC` `#Infrastructure`
`#AIinProjects` `#ResponsibleAI` `#ProfessionalDevelopment` `#Certification`

Always include `#ProjectControls`. Never tag a sector the post does not actually address.

---

## 8. Cadence

Three posts a week, from the sequence in `PUBLISHING-CALENDAR.md`:

| Slot | Content |
|---|---|
| Early week | The teaching document — a guide, a method, a worked example |
| Mid week | The companion template or checklist |
| Late week | The lighter piece — a definition, a distinction, a governance note, or an honest admission |

Two documents ship per week for fifty weeks. The third slot is amplification: a section from an earlier
document, a reader question answered, or a correction.

---

## 9. Comments and corrections

- **Answer technical challenges with arithmetic**, not authority. If someone says the EAC method is wrong,
  show the case where each is right.
- **When we are wrong, we say so in the thread and fix the document.** The correction is posted to the
  same channel that carried the error — see `GOVERNANCE-AND-REVIEW.md` §6. This is not damage control; it
  is the single most credible thing a standards body can do in public.
- **Do not argue about scope of practice, credentials or rival bodies.** Publish the standard and let it
  be compared.
- Never claim accreditation, recognition or outcomes in a comment that the documents do not claim. The
  comment box is where discipline usually fails first.

---

## 10. What we never post

Client or project data, however anonymised it feels. Screenshots of real schedules or cost reports.
Named criticism of a company, tool or individual. Salary figures or market statistics that are not output
from the Institute's own survey with a stated sample size — see `SAL-06`. Anything implying accreditation,
government recognition or guaranteed employment. Anything a candidate could mistake for examination
content.

---

## 11. Measuring whether this is working

Reach is the least useful signal available. The ones that matter:

| Signal | Why |
|---|---|
| Technical replies from practitioners who clearly do the work | The content is credible to the audience that counts |
| Template downloads that turn into repeat visits | The artefact is genuinely useful |
| Corrections offered by readers | We are being read closely — the strongest signal in this list |
| Employers citing the competency framework | The standard is being adopted, which is the actual goal |
| Candidate applications | Downstream, and lagging; never the weekly metric |

A post that reaches a hundred thousand people and teaches none of them has failed at the thing this
account exists to do.
