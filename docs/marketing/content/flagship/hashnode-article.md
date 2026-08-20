---
platform:      Hashnode (personal blog on the PCI Hashnode domain, published into the tag feeds)
type:          guide
title:         The project cost data model needs two dates, not one
subtitle:      Fourteen CPI points moved on a fact that was true on the 30th and posted on the 4th. One date column cannot hold that.
meta:          A project cost data model with one date column cannot say what you knew at cut-off. Two dates, the SQL, and the accrual that moved CPI 1.19 to 1.05.
primary_kw:    project cost data model *
secondary_kw:  bitemporal data model, cost performance index, project accruals, period cut-off
pillar:        Cost control and estimating
credential:    PCL-AI (suite named once)
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> https://projectcontrolsinstitute.org/month-end-close-for-projects
schema:        Article + FAQPage
word_count:    1,507 prose (H1 to the end of the FAQ, including table cells and the subtitle, excluding the three SQL blocks, the production front matter, the canonical instruction, the first-published line and the linking note); 1,606 counting the SQL as well.
hashtags:      Hashnode tags, five (the maximum): databases, sql, data-engineering, system-design, analytics
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
canonical_instruction: |
  SET THE CANONICAL BEFORE THE FIRST PUBLISH, NOT AFTER. In the Hashnode editor, open the post
  settings panel, find the republishing section ("Are you republishing this article?" / original
  article URL) and paste
  https://projectcontrolsinstitute.org/month-end-close-for-projects
  Hashnode renders <link rel="canonical"> from that field. Publish, then view source on the live
  post and confirm the canonical carries the projectcontrolsinstitute.org URL and not the Hashnode
  one. Hashnode's domain authority sits above this estate's, so a version published without the
  canonical competes with the page it was adapted from, and the fix after the first crawl is slower
  than the thirty seconds it takes now.
notes: |
  REPUBLISHED, CANONICAL SET. Hashnode's republishing field is a real canonical, so this may
  legitimately carry a near-copy of the hub's month-end close page. It is an adaptation, not a
  paste: the site page argues from the close calendar, this argues from the schema, which is the
  only order a Hashnode reader will finish.
  THE ANGLE IS THE DATA MODEL, AND IT IS DELIBERATELY NOT THE DEV PIECE. The DEV article argues
  from a confusion matrix outwards: precision, recall, F1, and why an absent row cannot be a false
  negative; its code is a set-difference join that finds progress without invoices. This argues from
  the schema outwards: valid time against transaction time, and an as-of query that reproduces a
  signed report by parameter so the accrual becomes a subtraction rather than a recollection. Two
  different mechanisms, two different claims, two different canonical targets. Beyond the locked
  arithmetic, the locked stake, the locked ask and the locked three facts, the two pieces share
  nothing, and no sentence is carried across from either DEV or Medium.
  A THIRD CANONICAL TARGET ON PURPOSE. Medium canonicalises to the hub's finance-and-project-
  management-certification pillar and DEV to pciai.org's AI governance page. Pointing this at
  month-end-close-for-projects means no two flagship assets push authority at one page, and it is
  the page this subject genuinely belongs to.
  Hook A (number first), per _STORY.md §2. Medium and DEV both hold Hook C, and a third consecutive
  consequence-first opener across the technical set would read as one campaign. The AEO answer
  follows inside the same 60 words, so the hook is not left hanging. No blended hooks.
  THE SQL IS THE REASON A DEVELOPER BOOKMARKS THIS. All three blocks are deliberately plain, and no
  engine tag is used because none of them needs one. The single portability caveat is the date
  subtraction in the lag query, which returns an integer on some engines and needs datediff or an
  interval on others; that is a one-line change a reader makes without being told, and spelling it
  out in the body would cost more than it buys.
  The as-of block is the artefact: one query, two bind parameters, and the difference between the
  two runs is the accrual. Nothing in the run targets that idea and it is the reason a third party
  would link here.
  SQL:2011 is named and described in PCI's own words. No standard's text, table or figure is
  reproduced, here or anywhere.
  The advance-payment caveat is kept in the FAQ on purpose. The obvious check constraint
  (recorded_on >= occurred_on) is false for prepayments, so the DDL deliberately does not ship it,
  and the piece says why. A senior reader finds that hole in ten seconds and the article is worth
  more for having found it first.
  Numbers about PCI: 13/61, 16/61, 16/63, 113 Standards, 532 process requirements, 40/40/20 labelled
  as the Body of Knowledge's proportions and stated outright not to be the examination's, and 15,613
  inside a sentence naming PFL-AI and PML-AI, with PCL-AI's exclusion in the next breath. No
  examination weighting anywhere. The 92 sector case studies are in the register and are left out:
  this audience reads a calculation-check count as evidence and a case-study count as marketing.
  The scope disclosure on 15,613 is tied back to the article's own thesis rather than apologised for,
  because a count without its scope is exactly the kind of number this piece is about.
  The project arithmetic carries no currency, no client, no sector, no date and no claim about how
  often it happens, because there is no researched frequency to cite. The bind parameters in the SQL
  are named rather than dated for the same reason.
  Three links, three domains, one each, all in the body where the sentence raises the question the
  target answers. Anchors differ from every other asset pointing at those pages. No pciworld.org and
  no pciglobal.ai link: this piece raises no career and no regional question, and a link with no
  question behind it is the footprint _LINK_ARCHITECTURE.md exists to avoid.
  Tags: Hashnode allows five and fewer than five is wasted distribution. databases, sql and
  data-engineering carry the reach; system-design and analytics pull two different feeds. No "ai"
  tag, because the piece is not primarily about AI and tagging it into that feed would be
  tag-stuffing an audience that did not ask for cut-off dates.
  ENGAGEMENT IS THE DISTRIBUTION MECHANISM HERE. Hashnode surfaces posts in tag feeds partly on
  early comment activity, so the author must be free to answer for the first six hours. The closing
  question is written to be answerable by someone who has done this work, not to be rhetorical.
  No cover image is specified. If one is added it must not contain a fabricated chart, and its alt
  text must describe what is in it.
when_to_post:  Launch week + 3. Publish only after the hub original has been live and indexed for at least a full week, and leave at least five clear days after the DEV article: both are technical, both carry the same locked arithmetic under a canonical, and the two audiences overlap at the edges. Tuesday, 08:00–09:00 US Eastern (13:00–14:00 UK), when the tag feeds are busiest and a post gets its longest run near the top. Hashnode weights early engagement, so the author has to be available to answer comments for the first six hours; an unanswered technical post here is a dead post. Avoid Fridays and US public holidays.
---

Paste the canonical into the republishing field in the Hashnode post settings before the first publish. It is not optional, and it cannot be done properly afterwards.

---

# The project cost data model needs two dates, not one

*Fourteen CPI points moved on a fact that was true on the 30th and posted on the 4th. One date column cannot hold that.*

Your CPI reads 1.19. The missing accrual says 1.05. Which number reaches the board?

Whichever one the schema can represent. A project cost data model needs two dates on every fact: the date the cost was incurred, and the date the system found out about it. Store one and the distance between those numbers has nowhere to live.

## Where the fourteen points went

Earned value is 2,200,000. Invoiced cost is 1,850,000.

**2,200,000 ÷ 1,850,000 = 1.19**

The cost performance index says every unit of cost bought 1.19 units of work. On the evidence in the system that is correct, and most people sign it.

Now the part the system has not seen. There is 240,000 of work done and not yet invoiced. Accrue it.

1,850,000 + 240,000 = 2,090,000

**2,200,000 ÷ 2,090,000 = 1.05**

Fourteen points, on one entry. The figures are illustrative arithmetic: no project, no client, no sector, no published frequency.

The error is accounting. The damage is delivery. Neither training alone catches it.

Nothing was concealed. The work was measured, the progress was reported, and the supplier had not billed. The fact was true inside the period and known outside it, and the table it went into had one date column.

**Fourteen CPI points were not lost in the arithmetic. They were lost in a column that did not exist.**

## What a project cost data model has to store

Two dates on every row, kept apart for the life of the record.

`occurred_on` is when the cost was incurred on the ground: the shift worked, the plant on hire. `recorded_on` is when the finance system learned of it: the posting date.

On a routine row the two sit days apart and nobody notices. On the rows that move a forecast they sit on opposite sides of a cut-off.

This is not a project controls invention. Temporal modelling calls the first valid time and the second transaction time, and SQL:2011 put period definitions into the standard so a database could hold both. Ledgers draw the distinction under their own names. It is lost downstream, when a cost table is flattened into a reporting model and one date looks redundant.

| Question | One date column | `occurred_on` + `recorded_on` |
|---|---|---|
| What is cost to date? | yes | yes |
| What did we publish last month? | only if nothing was restated | yes, by parameter |
| What did we know at the closing date? | no | yes |
| How much was true but not yet posted? | no | yes, as a subtraction |
| Which figures changed after the report went out? | no | yes, with dates |
| How long is our own posting lag? | no | yes, measured |

Four of the six are unanswerable on one date column, and they are the four an auditor asks for.

## The schema, and the query that reproduces a signed report

The table is unremarkable, which is the point.

```sql
create table cost_fact (
  fact_id       bigint        primary key,
  work_package  varchar(64)   not null,
  amount        decimal(18,2) not null,
  occurred_on   date          not null,  -- when the cost was incurred
  recorded_on   date          not null,  -- when the ledger learned of it
  source        varchar(32)   not null   -- invoice, accrual, timesheet, journal
);
```

The query is where the second date earns its keep.

```sql
select sum(amount) as cost_to_date
from   cost_fact
where  work_package = :wp
and    occurred_on <= :period_end
and    recorded_on <= :as_at;
```

Set `:as_at` to the period end and you get the report as it was signed: 1,850,000, CPI 1.19. Set it to today and you get the same period as now understood: 2,090,000, CPI 1.05. One query, one changed parameter.

The difference between those two runs is the accrual. It stops being something a controller has to remember on the last working day and becomes a subtraction that runs every period over a defined population.

A report you cannot reproduce is not a report; it is a recollection with a total at the bottom. With `recorded_on` in the table, the version you defended in June is still recoverable in December, along with every row that arrived after it.

## Measure your own lag instead of guessing at it

The second column turns an argument into a measurement. There is no industry figure for how late your suppliers invoice, and you do not need one: your own rows carry it.

```sql
select   work_package,
         count(*)                       as postings,
         avg(recorded_on - occurred_on) as mean_lag_days,
         max(recorded_on - occurred_on) as worst_lag_days
from     cost_fact
where    source = 'invoice'
group by work_package
order by mean_lag_days desc;
```

Run that across a year of history and you have a lag profile for your own supply chain. Apply it to the closing fortnight of a period and the accrual becomes an exposure derived from evidence rather than habit.

One limit. The lag you measure belongs to invoices that eventually arrived, so it says nothing about work still unbilled when you run it.

## What this does not fix

It does not tell you the accrual estimate is right. It tells you one is owed, a smaller claim and a more useful one.

And it does not make anybody ask the question. The schema makes "what closed when, and on whose calendar" answerable in one query. Somebody still has to know the question exists, and that is not a database problem.

## Who is examined on the second date?

The question sits between two professions. A chartered accountant is examined on when a cost may be recognised and what a provision has to satisfy, rarely on float or progress measurement. An engineer is examined on float and progress measurement, rarely on cut-off or a contract asset. A project lives in the overlap, and the overlap is where the money goes.

None of that is a comment on the people. Say the word "accrual" out loud and any controller books it correctly. The gap is in what each profession is *examined* on, and therefore in what each is assumed to have covered.

The Project Controls Institute exists for that handover. Three credentials, each with its own Body of Knowledge and examination: the PCI AI Project Controls Leader (PCL-AI) examines 13 domains and 61 knowledge areas, the PCI AI Project Finance Leader (PFL-AI) 16 domains and 61 knowledge areas, and the PCI Project Management Leader – AI (PML-AI) 16 domains and 63 knowledge areas.

Under them sit 113 mandatory PCI Standards carrying 532 process requirements. Those are certification requirements set by the Institute, not law, and nothing PCI publishes is legal, tax or accounting advice.

The Bodies of Knowledge run 40 per cent finance and reporting, 40 project management, 20 governed AI. Those are the Body of Knowledge's proportions, not the examination's. No examination weighting is published: the syllabus is settled, the exam blueprint is not, and any PCI exam weighting quoted at you did not come from us.

Reproducibility is a fair test to turn back on us: 15,613 machine calculation checks run against PFL-AI and PML-AI, all passing. PCL-AI has no equivalent suite yet. The scope belongs in that sentence every time, because a check count published without its scope is the kind of number this article is about.

## The part that is yours

You sign the forecast. Miss the accrual and the number you defended was wrong before you saw it. Seniority means owning both ledgers, and a credential that examines only one half leaves you accountable for a gap that nobody taught you.

Do not take this article's word for the syllabus. Read [the domains and knowledge areas PCL-AI examines](https://projectcontrolsinstitute.org/body-of-knowledge) against your own last month-end.

Then set `:as_at` to your own closing date and find out what your report said on the day you signed it.

If your cost tables already carry both dates, tell me what broke on the way. That is the comment worth reading here.

## FAQ

**Isn't this just a Type 2 slowly changing dimension?**
No. Type 2 versions attributes over system time, so it tells you what a supplier's category was in June. This is the fact table, where the money is: `occurred_on` and `recorded_on` are two independent axes on one measurement, and you query them separately rather than versioning either.

**Why not snapshot the report every month instead?**
A snapshot preserves what you published, not what you knew, and the two differ the moment anything is restated. It cannot be joined against corrected rows either, so you see that a figure moved but not which entries moved it. Two dates give every version by parameter, from live data.

**What about advance payments, where the money moves before the work?**
Then `recorded_on` precedes `occurred_on`, which is why the table above ships without the obvious constraint. Prepaid cost is a separate modelling decision, not an exception to the two-date model. It is the same model with the sign of the gap reversed.

**Can a model just estimate the accrual for us?**
It can rank candidates and propose a figure. Something still has to record the date it was produced on and whose name is against it. A machine-generated estimate is a fact with an author. [Who owns a figure a model produced](https://pciai.org/ai-policy-for-project-controls) is settled before the model runs, not after.

**Does a PCI credential replace a chartered qualification or a CCP?**
No, and it is not built to. Where statutory or chartered status is required, the relevant qualification meets it. These credentials examine the crossing between finance and delivery, a different question. If you already hold one, [how the main controls certifications line up against each other](https://credentialfinder.org/best-project-controls-certification) beats another brochure.

---

*First published on projectcontrolsinstitute.org.*

---

*Internal links: three, one per domain, each sitting in a sentence that raises the question the target answers. [The domains and knowledge areas PCL-AI examines](https://projectcontrolsinstitute.org/body-of-knowledge) answers "what would examining both sides actually cover", raised by the ask. [Who owns a figure a model produced](https://pciai.org/ai-policy-for-project-controls) answers "so who signs a number a model wrote", raised by the accrual-estimation question. [How the main controls certifications line up against each other](https://credentialfinder.org/best-project-controls-certification) answers "how is this different from the credential I already hold". The canonical carries the relationship to the hub's month-end close page, so that page is deliberately not linked in the body as well: one link per domain, per _LINK_ARCHITECTURE.md §2. In the comments, point anyone arguing about temporal joins at [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects), and anyone who wants the full period worked through at [the earned value worked example](https://projectcontrolsinstitute.org/earned-value-worked-example).*
