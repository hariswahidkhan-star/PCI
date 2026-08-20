---
platform:      Blogger (Blogspot, repurposed post carrying a manual canonical to the own-site original)
type:          guide
title:         Project accruals: the cost your CPI has not seen yet
meta:          Project accruals are costs incurred but not yet invoiced. Why one missing entry moves CPI from 1.19 to 1.05, and why the correction lands next month.
permalink:     project-accruals-cpi
primary_kw:    project accruals *
secondary_kw:  accrual reversal, cost performance index, goods received not invoiced, month-end cut-off
pillar:        Cost control and estimating
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> https://projectcontrolsinstitute.org/earned-value-worked-example
schema:        Article + FAQPage
word_count:    1,210 (H1 to the end of the questions block, including the five-row table's cells, excluding the front matter, the canonical instruction, the first-published line and the linking note)
hashtags:      None. Blogger uses labels, not hashtags. Labels, six: Earned value, Cost control, Project accruals, Month-end reporting, Project controls, Certification
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
when_to_post:  Launch week + 3, and deliberately last of the long-form set. Publish only once the own-site earned value worked example has been live and indexed for at least two weeks, because a repurposed copy that goes up before its original is indexed is the one way this post can do harm. Leave a clear seven days after the Medium story: both carry the same arithmetic under a canonical, and a reader who meets the same division twice in a week stops reading the second one. Any weekday morning, UK time. Blogger has no feed to catch and no engagement window to hit, so timing here is about sequence, not about the hour.
canonical_instruction: |
  BLOGGER EMITS ITS OWN CANONICAL, SO THE MANUAL ONE HAS TO REPLACE IT, NOT JOIN IT. Two rel=canonical
  tags on one page means Google ignores both, and a post that ends up with two is worse off than one
  with none. Do this before the post is published, not after the first crawl.
  ROUTE A, preferred, in Theme > Edit HTML. Blogger's auto canonical comes from
  <b:include data='blog' name='all-head-content'/> in the <head>. Immediately after that include, add
  a conditional keyed to this post's own URL:
    <b:if cond='data:view.url.canonical == &quot;https://YOURBLOG.blogspot.com/2026/XX/project-accruals-cpi.html&quot;'>
      <link href='https://projectcontrolsinstitute.org/earned-value-worked-example' rel='canonical'/>
    </b:if>
  Then suppress the automatic tag for that same URL, either by wrapping the include in the inverse
  condition or by replacing it with the individual head items your theme actually needs. Save, open
  the live post, view source, and count: exactly one <link rel='canonical'>, carrying the
  projectcontrolsinstitute.org URL. If the count is two, the edit has failed and the post must come
  down until it is one.
  ROUTE B, the fallback, and take it without embarrassment if the theme cannot be verified. Open the
  post > Search Description panel > Custom Robots Tags > tick noindex and follow. The Blogger copy
  then never competes with the page it was adapted from, the links still pass, and the reader is
  unaffected. A verified noindex beats an unverified canonical every time.
  ALSO ENABLE THE META FIELD. Settings > Meta tags > Enable search description must be on before the
  post's own Search Description box appears. Paste the meta line above into it. Without that step
  Blogger writes its own description from the first sentences and the click-through drops.
notes: |
  REPURPOSED, WHICH IS THE ONLY THING BLOGGER SHOULD EVER CARRY. It is rank 6 of the ten platforms,
  Google-owned, free, and carries little editorial weight, so no original writing time belongs here
  while the higher ranks are under-served. This is an adaptation of the hub's earned value worked
  example: that page runs a full month end to end with every figure shown, this takes one entry out
  of it and explains it to somebody who was never taught accounting. Different reader, same source,
  canonical set.
  A FOURTH CANONICAL TARGET, ON PURPOSE. Medium canonicalises to the hub's finance-and-project-
  management-certification pillar, Hashnode to month-end-close-for-projects, DEV to pciai.org's AI
  governance page. Pointing this at earned-value-worked-example means no two flagship assets push
  authority at the same page, and it is the page this post is genuinely a cut-down of.
  PLAINER REGISTER, DELIBERATELY. Blogger's traffic is long-tail search by people learning the job:
  planners, cost controllers, quantity surveyors, graduates in their first month-end. So every term
  is defined where it first appears, sentences are short, and nothing is assumed. What is not done is
  writing down to that reader. The arithmetic is identical to the one a board sees.
  Hook C (consequence first), which _STORY.md §2 assigns to anywhere the reader is cold to PCI, and
  a Blogger reader is the coldest in the set. It sits on line three rather than line one because AEO
  needs the title's question answered inside the first 60 words, and the answer here is 51. Medium
  and DEV also run C, and no reader of this post is reading either: Medium reaches a publication
  audience, DEV a tag feed, this reaches a search query about accruals.
  THE MECHANISM IS THE REVERSAL, AND NOTHING ELSE IN THE RUN HAS IT. The LinkedIn article owns the
  gap between two closing dates and its four-question test. Hashnode owns the data model. WordPress
  owns how the error propagates into the four EAC methods. Vocal owns the two calendars. This owns
  what happens next month: the cost does not vanish, it lands in the following period, so the swing
  between two months is wider than the single error, and the forecast was signed in between. The
  five-moment table exists nowhere else in the run and is the reason a third party would link here.
  NUMBERS: the locked worked example, unchanged, plus 13/61, 16/61, 16/63, 113 Standards, 532 process
  requirements, 40/40/20 labelled as the Body of Knowledge, and 15,613 inside a sentence naming
  PFL-AI and PML-AI with PCL-AI's exclusion in the next breath. No month-two figures are invented:
  the swing is described, never quantified, because a second month's arithmetic would mean adding
  numbers to a worked example that is locked. No currency, no client, no sector, no date, no claim
  about how often this happens.
  NO EXAMINATION WEIGHTING, STATED OUTRIGHT. 40/40/20 is the figure a reader is most likely to
  misquote as an exam blueprint, and a plainer register makes that misreading likelier, not less
  likely. So the post says in terms that no exam weighting is published and that any weighting seen
  quoted did not come from PCI.
  THREE LINKS, THREE DOMAINS, ONE EACH, all in the body where the sentence raises the question the
  target answers. The credentialfinder.org page is one no other flagship asset links to, and it is
  chosen for this audience specifically: a planner part-way through another certification. Blogger's
  own rule in the plan says to link to the original article; the canonical does that at head level,
  and the closing first-published line names the page in words rather than as a URL, so the post
  carries exactly one hub link. Two links to one domain in one post is the footprint
  _LINK_ARCHITECTURE.md §2 exists to prevent, and the rule does not bend because the platform is a
  small one, or because the second one was only a URL sitting in a sign-off line.
  AEO: the title's question is answered in 51 words, every H2 except the sign-off and the questions
  header is one a person would actually type, and four questions close the post answered in 40 to 80
  words each. None of the four repeats the LinkedIn article's FAQ block, which already owns what an
  accrual is, why CPI reads high, whether the cut-off dates should match and whose problem it is.
  Mark the closing block up as FAQPage; Blogger will not do it for you, so it goes in the post's HTML
  view. The primary keyword sits in the H1, the opening sentence, one H2 and the meta description,
  and nowhere else on purpose.
---

# Project accruals: the cost your CPI has not seen yet

Project accruals are costs you have already incurred but have not yet been invoiced for. Until one is posted, actual cost is short by that amount, so every ratio built on actual cost reads better than the job is. The entry catches up later, and that is where the damage is.

Fourteen CPI points went missing in an accrual, not a schedule.

## What are project accruals?

A ledger records cost when an invoice is approved. A project incurs cost when the work is done. On a project those two days can be weeks apart, and the accrual is the entry that puts the cost back into the month it belongs to.

Every set of accounts uses them. On a project the gap is wider: the site works to a programme, the paperwork follows its own calendar.

Four kinds recur. Subcontract work done but not yet valued. Materials delivered without an invoice, which finance calls goods received not invoiced. Plant on hire billed in arrears. Timesheets posted late.

## What does one missing accrual do to CPI?

Round numbers, and they belong to no project.

Earned value is 2,200,000. Invoiced cost is 1,850,000.

**2,200,000 ÷ 1,850,000 = 1.19**

Cost performance index reads 1.19, so the report says the job is running under budget. But 240,000 of work is done and not yet invoiced. Accrue it, and cost becomes 2,090,000.

**2,200,000 ÷ 2,090,000 = 1.05**

Fourteen points moved on one missing accrual. The error is accounting. The damage is delivery. Neither training alone catches it.

## When is a project cost actually counted?

A single cost passes five moments, and the two systems reading it are not looking at the same one.

| Moment | What has happened | What earned value shows | What the ledger shows |
|---|---|---|---|
| The work is done | Cost is incurred | Earned, this month | Nothing yet |
| Progress cuts off | The site's month closes | Earned and reported | Nothing yet |
| The ledger closes | Finance's month closes | Already reported | Nothing, unless somebody accrues it |
| The invoice arrives | Paperwork catches up | Earned last month | Cost posted, this month |
| The invoice is paid | Cash leaves | No change | No change to cost |

Row four is the whole problem. Earned value counted the work in the first month, the ledger counts its cost in the second, and neither system made a mistake.

The accrual bridges them, and it needs a number only delivery can produce, entered by somebody who works in the ledger. Miss that handover and the accrual becomes a guess or an omission. The report never complains. It is arithmetically perfect on both sides of a number that is wrong.

## Why does CPI drop the month after a good report?

Because the invoice arrives. The first month earned the work and carried none of its cost. The second carries the cost and earns nothing for it, because the work was earned already.

Nothing changed on site. The two months are reporting halves of one event, and the swing between them is wider than the single error that caused it.

By then the first month's forecast has gone out, built on the first month's ratio.

## Why does neither professional examination cover this?

An accountant is examined on when revenue may be recognised and what a provision has to satisfy. Rarely on float. An engineer is examined on progress measurement and the critical path. Rarely on cut-off.

This is not an argument about competence. Either could learn the other's half. Neither is examined on the handover between them, so neither is assumed to have covered it, and the handover is where the money goes.

If you are working towards a certification, read [which certifications examine cost and which examine only the schedule](https://credentialfinder.org/best-certification-for-planning-engineers) rather than comparing names.

## Will an AI spot a missing accrual?

Give a model 2,200,000 of earned value and 1,850,000 of cost and it returns 1.19, correctly, with a fluent paragraph explaining the favourable variance. It cannot see a cost that was never entered. There is no row for it.

Which makes the governance question a narrow one: which figures a model may produce, which it may only repeat, and whose name is on the output. Agree [what AI can and cannot decide in project controls](https://pciai.org/ai-in-project-controls) before the tool is switched on, not after the first board pack.

## What examines both sides?

The Project Controls Institute exists to examine the crossing. The PCI AI Project Controls Leader (PCL-AI) examines 13 domains and 61 knowledge areas. The PCI AI Project Finance Leader (PFL-AI) examines 16 domains and 61 knowledge areas. The PCI Project Management Leader – AI (PML-AI) examines 16 domains and 63 knowledge areas.

Underneath them sit 113 mandatory PCI Standards carrying 532 process requirements. Those are certification requirements set by the Institute. They are not law, and nothing here is accounting advice.

The Body of Knowledge is built 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed AI. That describes the Body of Knowledge, not the examination. No exam weighting is published, because the syllabus is settled while the blueprint is still open. If you see one quoted, it did not come from us.

A post built on arithmetic should say how its own is checked. 15,613 machine calculation checks run against PFL-AI and PML-AI, all passing. PCL-AI has no equivalent suite yet, and saying so is the only reason the figure is worth quoting.

## Before your next month-end

You sign the forecast. Miss the accrual and the number you defended was wrong before you saw it. A credential that examines only one half leaves you accountable for a gap that nobody taught you.

So do not take our syllabus on trust either. Read [the 13 domains and 61 knowledge areas of PCL-AI](https://projectcontrolsinstitute.org/body-of-knowledge) and hold them against the last report you put your name to.

Then find out what was delivered in the final week of that month, and whether its cost was in it.

## Questions people ask

**What is the difference between an accrual and a commitment?**
A commitment is money promised: an order placed, nothing received. An accrual is work or goods received with no invoice yet. Commitments belong in the forecast, not in actual cost. Accruals belong in actual cost, in the period the work happened. Confuse them and CPI moves in opposite directions.

**Does an accrual change earned value?**
No. Earned value comes from physical progress and the earning rules, so it does not care when an invoice arrives. The accrual sits on the cost side of the ratio. That asymmetry is why one missing entry moves CPI while both systems feeding it stay internally correct.

**What if the accrual is too big?**
Then actual cost is overstated, CPI reads low, and a job performing well gets a recovery plan it does not need. Estimating on the same basis every month, and correcting openly when the invoice lands, matters more than getting the figure right first time.

**Who should work out the project accrual?**
The number comes from whoever knows what was delivered. The entry is posted by whoever owns the ledger. Where those two people never speak, the accrual becomes an estimate of an estimate, and the report carries it without ever saying so.

---

*First published as the earned value worked example on projectcontrolsinstitute.org, where the same month is worked end to end with every figure shown.*

---

*Internal links: three, one per domain, each sitting in a sentence that raises the question the target answers. [Which certifications examine cost and which examine only the schedule](https://credentialfinder.org/best-certification-for-planning-engineers) on credentialfinder.org answers "does the certification I am already studying for cover this?", raised by the examination section, and it is a page no other flagship asset links to. [What AI can and cannot decide in project controls](https://pciai.org/ai-in-project-controls) on pciai.org answers "so who is allowed to let the model produce this number?", raised in the AI section. [The 13 domains and 61 knowledge areas of PCL-AI](https://projectcontrolsinstitute.org/body-of-knowledge) on the hub answers "what would examining both sides actually cover?", raised by the ask. No pciworld.org or pciglobal.ai link: this post raises no career and no regional question, and a link with no question behind it is the footprint we are avoiding. The closing first-published line now names the origin page in words instead of printing its URL: the canonical already carries that relationship, and a second hub address in one post breaks the one-link-per-domain rule whether or not it is marked up as a link. In comments, point anyone asking about the two closing dates at [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects) and anyone asking which forecast to use at [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).*
