---
platform:      PM World Journal (featured paper, submitted to the editor for a monthly edition)
type:          paper
title:         "Two clocks, one index: the cut-off differential between the ledger and the data date"
subtitle:      What happens to CPI, to the forecast at completion and to recognised revenue when posted cost is substituted for incurred cost, and why the substitution survives professional examination
meta:          n/a — PM World Journal publishes as PDF and web in academic house style and writes no meta description. The Keywords line below is the field its index and every downstream search read.
primary_kw:    cut-off differential
secondary_kw:  cost performance index, accrual for work not yet invoiced, estimate at completion, cost-to-cost input method
pillar:        Cost control and estimating
ab_id:         n/a — flagship launch asset, not an Article Bank brief
when_to_post:  "Launch week + 2, or the first edition that will take it. PM World Journal publishes monthly, so this asset runs on an editorial calendar rather than a posting time: send the manuscript and the author photograph to the editor by email, in Word, and check the journal's current author guidelines for the closing date of the edition you are aiming at before you promise anyone a month. Allow for editorial review and for a request to change length or house style, and do not announce the paper anywhere until the edition is live. Sequence it after the own-site pillar and after the LinkedIn article: a practitioner journal read is the slowest and the most durable of the launch assets, and it is the one that gives the others something citable to point at."
word_count:    2,530 (title block to the end of the About the Author section, including table cells and references, excluding front matter and the linking note)
hashtags:      None. PM World Journal is a PDF and web journal in academic house style and carries no tags; the Keywords line does that work and is the field the journal's own index and every downstream search reads. Do not add hashtags to the manuscript.
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
notes: |
  ORIGINAL, NOT A REPUBLICATION. PM World Journal expects original work and asks authors to declare
  prior publication. Nothing in this paper is lifted from the own-site pillar: the pillar argues from
  the syllabus outwards, this derives a closed-form result and then explains why the result survives.
  Submitting a rewritten blog post to a practitioner journal is the single fastest way to be declined
  by an editor who reads three hundred of them a year.
  Register: refereed practitioner journal. No marketing voice anywhere in the body. The Institute is
  named twice, in a declaration of interest that a journal expects and that does the persuasive work
  better than persuasion would, because it is a list of the claims PCI could have made and has not.
  Hook B (contrast first) per _STORY.md §2, which names articles as its home. Not blended with A or C.
  THE CONTRIBUTION, which is why this is a paper and not an advert: the reported cost performance
  index is overstated by a factor of exactly one plus the accrual ratio, the CPI-based forecast at
  completion is understated by the reciprocal of that factor whatever the budget is, and revenue
  measured by a cost-to-cost input method moves in the opposite direction from the same omission. All
  of it is derived from the launch example's own figures. No new figure is introduced anywhere.
  Numbers audit: 2,200,000 / 1,850,000 / 240,000 / 2,090,000 / 1.19 / 1.05 from the worked example,
  with every division shown. 240,000 ÷ 1,850,000 = 0.1297 and 2,090,000 ÷ 1,850,000 = 1.13 are
  arithmetic on those same figures, not new data. Register figures: 13/61, 16/61, 16/63, 113
  Standards, 532 process requirements, and 40/40/20 labelled as the Body of Knowledge and explicitly
  denied as an examination weighting, and 15,613 in a sentence that names PFL-AI and PML-AI and
  states that PCL-AI has no equivalent suite. Nothing else. No currency symbol, no client, no sector,
  no date, and no claim about how often this occurs, because no researched frequency exists to cite.
  Section 5's reverse test is phrased as a question to ask, not as a frequency, for the same reason.
  References are six real documents, cited only where the argument uses them, with no text, table or
  figure reproduced from any of them and no URL to anything off the five domains. Five are external
  and the sixth is the Institute's own material, declared as such in the entry and in Section 8. A
  journal reference list is checked line by line, so a padded one costs more than it earns.
  Three links, three domains, one each, all in body sentences that raise the question the target
  answers. Anchor text differs from every other launch asset pointing at the same three pages. No
  pciworld.org and no pciglobal.ai link: this paper raises no career and no regional question.
  The author block is a placeholder because the byline is a person, not the Institute. PM World
  Journal prints an author photograph and a regional line, so both are marked for supply. The
  manuscript does not label itself a featured paper or assign itself to a series: the editor decides
  the category, and a submission that has already awarded itself one reads badly on the first page.
  When converting to Word for submission, set Cp and A in Section 4 as a subscript and an italic
  respectively, keep the three displayed calculations on their own lines, and leave the two tables as
  Word tables rather than images so the journal can typeset them.
---

# Two clocks, one index: the cut-off differential between the ledger and the data date

**[AUTHOR NAME]**
[Role], Project Controls Institute Global
[City, Country]

## Abstract

A cost performance index is read as a statement about productivity. It is also, silently, a statement about which ledger was open and on what date. This paper examines the cut-off differential: the interval between the date a project's cost ledger closes and the date its progress is measured, together with the work performed inside it and not yet invoiced. From one arithmetical demonstration it derives three results. Where posted cost stands in for incurred cost, the reported index is overstated by a factor of exactly one plus the accrual ratio. The forecast at completion taken from that index is understated by the reciprocal of the same factor, independently of the budget. Revenue measured by a cost-to-cost input method moves the opposite way, so the two reports are wrong in opposite senses and each looks locally plausible. The schedule indices stay silent throughout, having no cost term. The exposure is structural: it survives because it falls between two syllabuses, neither of which examines the handover.

**Keywords:** earned value management; cut-off; accruals; cost performance index; estimate at completion; revenue recognition; input methods; project controls; governed use of machine output.

## 1. Introduction

Accountancy examinations rarely test float. Engineering examinations rarely test cut-off. A project requires both every month, and it requires them of the same number.

The cost performance index is treated as an operational measure, and half of it is. Earned value is produced inside the project, under rules of credit the project sets and can defend [1], [2]. The denominator is not produced there. Actual cost is an accounting quantity assembled to an accounting calendar, and in a great many reporting systems it is assembled from what has been posted rather than what has been incurred.

That substitution is rarely deliberate and almost never written down. It happens because the invoice is the artefact both functions can see, and the quantity both functions can see is the one that reaches the report.

## 2. Three clocks, one report

A project closes three times a month and calls it one close.

| Clock | Set by | What it fixes | What moves it |
|---|---|---|---|
| Ledger close | The finance calendar | Which transactions fall in the period | Invoice receipt and posting cycles |
| Data date | Project controls | How much work counts as performed | The site reporting cycle |
| Valuation date | The contract | What may be applied for and certified | Contractual certification dates |

The cut-off differential is the interval between the ledger close and the data date, together with the work performed inside it and not yet invoiced. It is not an error in either system. Each has closed to the date it is required to close to.

The accrual is the only figure belonging to neither. It exists where somebody has noticed that the two systems disagree about what month it is, and it is raised on evidence held by the project, since only the project knows work was performed after the last valuation.

Accrual accounting requires the cost in the period the work was done, not the period the invoice arrives. The failure examined here is not a dispute about that principle but a quiet substitution, inside the reporting pipeline, of a payables-driven quantity for an economic one.

## 3. A demonstration

The figures below are illustrative arithmetic. They carry no currency, no sector and no client, and no frequency is claimed for them.

Earned value is 2,200,000. Invoiced cost is 1,850,000.

> 2,200,000 ÷ 1,850,000 = 1.19

A cost performance index of 1.19 says every unit of cost bought 1.19 units of work. On the evidence in the system it is correct, and a competent reviewer would sign it.

Now the figure the system has not seen. There is 240,000 of work performed and not yet invoiced at the reporting date. Accrue it.

> 1,850,000 + 240,000 = 2,090,000
> 2,200,000 ÷ 2,090,000 = 1.05

Fourteen index points moved on one journal entry. Nothing happened on site. The job at the end of the month was the job it had been at the start of it.

## 4. The general form

Let EV be earned value at the reporting date, *C*p the cost posted to the ledger at that date, and *A* the accrual for work performed and not yet invoiced. Cost incurred is *C*p + *A*.

The reported index is EV ÷ *C*p. The index on incurred cost is EV ÷ (*C*p + *A*). Their ratio is:

> (*C*p + *A*) ÷ *C*p = 1 + *r*, where *r* = *A* ÷ *C*p

Earned value cancels. **The factor by which the cost performance index is overstated does not depend on the schedule, on the rules of credit, or on how much work has been done. It is one plus the accrual ratio, and nothing else.** In the demonstration, 240,000 ÷ 1,850,000 = 0.1297, so the reported index stood 12.97 per cent above the index on incurred cost, which is what carried 1.19 to 1.05.

The consequence for the forecast is graver, because the forecast is the number that leaves the room. Where the estimate at completion is budget divided by the cost performance index, it inherits the reciprocal:

> EAC on incurred cost ÷ EAC on posted cost = 1 + *r* = 2,090,000 ÷ 1,850,000 = 1.13

The corrected forecast is about 13 per cent above the reported one, and the size of the error is knowable without knowing the budget.

The third consequence runs the other way. Where progress is measured for revenue by an input method comparing costs incurred to date against total expected costs [3], the same omission understates the numerator, and so understates both progress and the revenue recognised with it. One missing entry has made the cost report optimistic and the revenue figure conservative in the same month, from the same cause.

There is a caveat a practitioner should raise. If total expected cost was itself drawn from the same posted-cost run rate, part of the effect cancels, and how much cannot be known without inspecting how the estimate was built. That does not rescue the position. It means the two figures are no longer on a common basis, and no reconciliation between them will close.

When the accrual is finally posted, the reversal arrives next period as an adverse cost variance with no operational cause, beside a revenue catch-up with no operational cause. Both are explained by a date. Neither is explainable to a board already told the job was ahead.

## 5. Why the schedule indices stay silent

Schedule variance and the schedule performance index are computed from earned value and planned value. Neither contains a cost term [1]. A cut-off error is invisible to both, and schedule review will not surface it.

That yields a diagnostic. A period in which the cost performance index moves materially while the schedule performance index is stationary, and in which no operational change has been recorded, should be treated as a question about cut-off before it is treated as a question about productivity.

It applies in reverse to good news. An index that improves in a month when nothing changed on site is a question about posting dates before it is a gain in performance, and the improvement is the version nobody challenges.

## 6. Computation is not completeness

Give a model earned value of 2,200,000 and a posted cost of 1,850,000 and it returns 1.19, correctly, every time, with a variance narrative around the answer that is more fluent than most written by hand. Arithmetic is not where the risk sits.

What no model can do is observe a cost that has not been recorded anywhere. There is no field for an invoice that does not exist, and absence has no feature representation.

The point deserves the language of evaluation, because it is often mishandled. Precision and recall are estimated over a labelled population of observed records. An anomaly detector trained on posted transactions has no positive class for a transaction that was never posted, so its recall over unrecorded events is not high; it is undefined.

That narrows the governance question, and it is the one the recognised management frameworks put to any organisation deploying such systems [4], [5]. It is not whether the tool computes correctly. It is which figures a model may originate, which it may only restate from a tested source, and whose name stands on the output when it reaches a board pack. [The boundary between figures a model may originate and figures it may only restate](https://pciai.org/ai-policy-for-project-controls) is a shorter document than the procurement exercise that preceded the tool.

## 7. The examination boundary

An error this tractable persists in organisations staffed by qualified people, and not for want of competence. A chartered accountant knows what an accrual is and when it must be recognised. A planner knows what earned value counts and what it does not.

The gap is in what each profession is *examined* on, and so in what each may assume the other has covered.

| Object | Where it is examined | Who produces it on a project | Who consumes it |
|---|---|---|---|
| Cut-off and revenue recognition | Accountancy syllabuses | Finance, from the project's own numbers | The financial statements |
| Float, logic and the critical path | Engineering and project management syllabuses | Planning | Delivery management |
| Rules of credit and progress measurement | Engineering and project management syllabuses | Project controls | Both functions |
| Estimate at completion | Produced in one syllabus, consumed in the other | Project controls | Finance, unchanged, into an audited statement |
| The cut-off differential itself | Neither | Nobody | Everybody |

The last two rows carry the argument. One profession produces the forecast at completion; the other consumes it, unaltered, in a statement that will be audited. Neither is examined on the crossing, and it is at the crossing that the money is lost.

This is a structural claim, not a complaint about people. Each of these boundaries was drawn for good reasons and is defensible on its own terms. What is not defensible is that the interface between two well-drawn boundaries is examined by nobody at all, while a named individual signs the number crossing it.

## 8. Declaration of interest, and what is not claimed here

The author is affiliated with the Project Controls Institute, which certifies against three credentials built for that interface. The facts, and their limits, follow.

The credentials are the PCI AI Project Controls Leader (PCL-AI), examining 13 domains and 61 knowledge areas; the PCI AI Project Finance Leader (PFL-AI), examining 16 domains and 61 knowledge areas; and the PCI Project Management Leader – AI (PML-AI), examining 16 domains and 63 knowledge areas. They sit on 113 mandatory PCI Standards carrying 532 process requirements. Those Standards are certification requirements established by the Institute. They are not law, and nothing PCI publishes is legal, tax or accounting advice.

The Bodies of Knowledge are built in proportions of 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed use of machine output. That describes the Body of Knowledge and not the examination. No examination weighting is published, because the syllabus is settled while the exam blueprint remains an open decision, and a weighting attributed to a PCI examination did not come from the Institute.

A paper built on arithmetic should say how its own arithmetic is checked. There are 15,613 machine calculation checks running against the PFL-AI and PML-AI materials, all passing. PCL-AI has no equivalent suite, and the figure should not be quoted without that scope.

What is not claimed: no accreditation, recognition, endorsement, affiliation or partnership, none of which has occurred; no pass rate, no student numbers, no salary effect, no guaranteed outcome; and no suggestion that any of this substitutes for a statutory or chartered qualification where work requires one. Readers already holding an established credential may find [a side-by-side reading of what the main controls certifications examine](https://credentialfinder.org/best-project-controls-certification) more useful than a further description of this one.

## 9. A test on your own last close

The result in Section 4 is testable on one month of your own reporting.

1. Take last month's reported cost performance index and identify the cost figure in its denominator. Establish whether it is posted cost or incurred cost. In many pipelines that cannot be answered from the report and must be traced to the extract.
2. Obtain the accrual raised at the same date for work performed and not yet invoiced, and divide it by posted cost. That proportion is how far the index was overstated, and one plus it is the multiple by which a corrected forecast at completion exceeds the reported one.
3. Compare the ledger close date with the data date. The interval between them is the population the accrual should have been drawn from.
4. Ask whether the cost basis in the index is the basis used to measure progress for revenue. Where it is not, the two reports disagreed all month and neither of them said so.

Readers who would rather test the boundary than the argument can read [the 13 domains and 61 knowledge areas of the PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) [6] against their own last month-end, and judge for themselves whether the crossing is covered anywhere else.

## 10. Conclusion

The cost performance index is a ratio of a project quantity to an accounting quantity, reported as though both were project quantities. Where posted cost stands in for incurred cost, the index is overstated by a factor of exactly one plus the accrual ratio, the forecast at completion is understated by the reciprocal of that factor, and revenue is understated in the month the cost report is flattered.

None of it is difficult arithmetic, and none of it is visible from the schedule, from the index itself, or from an automated review of records that were never created. It is visible from a reconciliation of two dates, performed by somebody taught to expect the difference.

You sign the forecast. Miss the accrual and the number you defended was wrong before you saw it. Seniority means owning both ledgers, and a credential examining only one half leaves a professional accountable for a gap that no syllabus prepared them for.

## References

[1] International Organization for Standardization. *ISO 21508:2018, Earned value management in project and programme management.* Geneva: ISO.

[2] AACE International. *Recommended Practice No. 10S-90, Cost Engineering Terminology.* Morgantown, WV: AACE International.

[3] International Accounting Standards Board. *IFRS 15, Revenue from Contracts with Customers.* London: IFRS Foundation.

[4] International Organization for Standardization and International Electrotechnical Commission. *ISO/IEC 42001:2023, Information technology — Artificial intelligence — Management system.* Geneva: ISO.

[5] National Institute of Standards and Technology. *Artificial Intelligence Risk Management Framework (AI RMF 1.0).* Gaithersburg, MD: NIST, 2023.

[6] Project Controls Institute. *PCI AI Project Controls Leader (PCL-AI) Body of Knowledge.* The author's affiliation is declared in Section 8.

## About the author

**[AUTHOR NAME]**
[City, Country]

[PHOTOGRAPH TO BE SUPPLIED. PM World Journal prints one beside the biography.]

[AUTHOR NAME] is [role] at the Project Controls Institute. [Two or three sentences of verifiable career detail: years in the discipline, sectors, functions held, memberships actually held. Nothing that has not occurred, and no qualification that cannot be produced on request.] [AUTHOR NAME] can be contacted at [EMAIL].

---

*Internal links: three, one per domain, each in a body sentence that raises the question the target answers. [The boundary between figures a model may originate and figures it may only restate](https://pciai.org/ai-policy-for-project-controls) on pciai.org answers "so what should the policy actually say", raised at the end of Section 6. [The 13 domains and 61 knowledge areas of the PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) on the hub answers "what would examining the crossing actually cover", raised by Section 9. [A side-by-side reading of what the main controls certifications examine](https://credentialfinder.org/best-project-controls-certification) on credentialfinder.org answers "how does this differ from the credential I hold", raised in the declaration of interest. No pciworld.org and no pciglobal.ai link, because this paper raises no career and no regional question and a link without a question behind it is the footprint we avoid. The own-site companion piece should link out to this paper once it is published, with the anchor "the cut-off differential derived in full", and the hub's earned value and month-end pages are the two same-domain internal links that companion piece carries.*
