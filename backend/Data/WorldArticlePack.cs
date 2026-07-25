using PCI.Backend.Core;

namespace PCI.Backend.Data;

/// <summary>
/// PCI World — the founding article batch (EXPANSION_GOVERNANCE §5, delivery batch 1 of the blog
/// programme).
///
/// Every article here is original practice writing about the discipline itself. That choice is
/// deliberate and it is what makes the batch publishable without a research desk: these pieces
/// explain how a technique behaves and where practitioners get caught by it, so they rest on the
/// same body of knowledge the challenge library is built from. There are no statistics, no
/// research citations, no company names and no claims about current events — those require saved,
/// verifiable sources, which is the newsroom's job and its separate obligation.
///
/// Attribution is the transparent "PCI World Editorial" byline. The governance permits exactly two
/// options — a real named person or this — because an invented author is a lie about provenance
/// however good the article is.
///
/// Seeding follows the same discipline as the challenge pack: house rows (author_id NULL) upsert
/// with a NEW immutable version when the text changes, so a deploy can never silently rewrite what
/// a reader was served, and operator-edited rows are never touched.
/// </summary>
public static class WorldArticlePack
{
    sealed record Article(string Slug, string Title, string Dek, string Tags, string SeoDesc, string Body);

    public static int Count => Articles.Length;

    public static void Seed(Db db)
    {
        foreach (var a in Articles) Upsert(db, a);
    }

    static void Upsert(Db db, Article a)
    {
        var existing = db.QueryOne("SELECT id,author_id,status,current_version,body_md FROM pciworld_articles WHERE slug=?", a.Slug);
        if (existing is null)
        {
            var id = db.ExecuteReturningId(@"INSERT INTO pciworld_articles
                    (slug,kind,title,dek,body_md,author_name,tags_json,seo_desc,status,current_version,published_at)
                VALUES(?, 'blog', ?,?,?,?,?,?, 'published', 1, datetime('now'))",
                a.Slug, a.Title, a.Dek, a.Body, WorldEditorial.EditorialByline, a.Tags, a.SeoDesc);
            db.Execute(@"INSERT INTO pciworld_article_versions
                    (article_id,version,title,dek,body_md,author_name,seo_desc,tags_json)
                VALUES(?,1,?,?,?,?,?,?)",
                id, a.Title, a.Dek, a.Body, WorldEditorial.EditorialByline, a.SeoDesc, a.Tags);
            return;
        }
        // Never touch an operator-authored article or one that is mid-workflow.
        if (existing["author_id"] is not null || H.Str(existing["status"]) != "published") return;
        if ((H.Str(existing["body_md"]) ?? "") == a.Body)
        {
            db.Execute(@"UPDATE pciworld_articles SET title=?, dek=?, tags_json=?, seo_desc=?, updated_at=datetime('now')
                WHERE slug=? AND author_id IS NULL", a.Title, a.Dek, a.Tags, a.SeoDesc, a.Slug);
            return;
        }
        var id2 = H.L(existing["id"]);
        var next = H.L(existing["current_version"]) + 1;
        db.Execute(@"UPDATE pciworld_articles SET title=?, dek=?, body_md=?, tags_json=?, seo_desc=?,
                current_version=?, updated_at=datetime('now') WHERE id=? AND author_id IS NULL AND status='published'",
            a.Title, a.Dek, a.Body, a.Tags, a.SeoDesc, next, id2);
        db.Execute(@"INSERT OR IGNORE INTO pciworld_article_versions
                (article_id,version,title,dek,body_md,author_name,seo_desc,tags_json)
            VALUES(?,?,?,?,?,?,?,?)",
            id2, next, a.Title, a.Dek, a.Body, WorldEditorial.EditorialByline, a.SeoDesc, a.Tags);
    }

    static readonly Article[] Articles =
    {
        new("why-your-spi-recovers-as-the-project-ends",
            "Why your SPI recovers as the project ends",
            "The schedule performance index always returns to 1.0 at completion — including on projects that finish late. Here is why, and what to report instead.",
            """["earned_value","schedule","reporting"]""",
            "The classic SPI converges to 1.0 at completion regardless of lateness. Why that happens, and how earned schedule answers the question SPI cannot.",
            """
Every project controls professional eventually watches the same thing happen. A project is months late. The recovery plan has not worked. And the schedule performance index — the number the steering group has been watching all year — quietly drifts back towards 1.0.

Nobody is cheating. The index is behaving exactly as its arithmetic requires.

## The arithmetic, briefly

The classic schedule performance index is earned value divided by planned value. Both are measured in money.

At completion, a project has earned its entire budget: every work package is done, so earned value equals the budget at completion. And the plan has also run out — planned value equals the budget at completion too, because the plan finished spending on its final planned day.

So at completion, EV and PV are the same number, and the index is 1.0. It does not matter whether the project finished on time, a month late, or a year late. The index converges on 1.0 because both of its inputs converge on the same value.

## Why this is worse than a harmless quirk

The convergence does not happen suddenly at the finish line. It starts as soon as the plan begins to tail off, which on most projects is the final third — precisely the period when a late project most needs an honest signal.

The result is a report that gets more reassuring as the situation gets less recoverable. A steering group watching the index sees improvement. What they are actually seeing is the denominator running out.

Worse, the improvement is easy to attribute to the recovery plan. It looks like the intervention worked. It arrives at the exact moment somebody is looking for evidence that it did.

## Earned schedule: the same data, a better question

Earned schedule answers a different question. Instead of asking *how much value have we earned against how much we planned to have earned*, it asks *at what point in the plan was this much value supposed to have been earned*.

You take the cumulative earned value, and you find where that amount sits on the planned value curve. If you have earned £2.6m, and the plan said you would have earned £2.2m by the end of period four and £3.0m by the end of period five, then you are somewhere in period four — about halfway through it, by interpolation. That point is your earned schedule: 4.5 periods.

Compare it with actual time elapsed. If five periods have passed and you have earned 4.5 periods' worth, your time-based index is 0.9. You are running at ninety per cent of planned pace.

That number does not converge on anything. A project that finishes late finishes with a time-based index below one, because the comparison is between two durations, not two amounts of money.

## What to do on Monday

You do not need new data. Earned schedule uses the planned value curve and the earned value figure you already report.

Three practical notes:

1. **Do not switch silently.** If your steering group has watched the classic index all year, changing the number without explanation costs more credibility than the better measure buys. Show both once, explain the convergence, then move.
2. **Forecast in time.** Dividing planned duration by the time-based index gives an estimate of the duration at completion. That is a date, which is what people actually asked for when they asked about schedule.
3. **Expect the first honest report to be uncomfortable.** The classic index was flattering the position; the corrected view will look like a deterioration even though nothing changed except the measurement.

## The general lesson

A measure that cannot express the thing you care about will still produce a number every month, and the number will be trusted in proportion to how long it has been reported.

The schedule performance index is not broken. It is a cost-denominated measure being asked a question about time, and it answers the question it can answer. The failure is not in the arithmetic. It is in the reporting habit that never checked whether the question and the measure matched.

That is worth generalising. When a metric behaves in a way that seems too convenient, the first hypothesis should not be that things are improving. It should be that you have found the boundary of what the metric can say.
"""),

        new("the-hundred-percent-rule-is-not-bureaucracy",
            "The 100% rule is not bureaucracy",
            "A work breakdown structure exists to make unfunded scope impossible to hide. Every argument against the 100% rule is an argument for finding that scope later, at someone else's price.",
            """["scope","wbs","cost_control"]""",
            "The WBS 100% rule explained as a control, not a formality: why every parent must equal the sum of its children, and what goes wrong when it does not.",
            """
The 100% rule says that a work breakdown structure must contain all of the work, and that every parent element must equal exactly the sum of the elements beneath it. No more, no less.

It gets treated as a formality. It is closer to a smoke alarm.

## What the rule actually detects

Consider a fibre rollout. The network build package was budgeted early at one million. Later, the detail was developed: duct and chamber at seven hundred thousand, cable and splice at two hundred and fifty thousand. Nine hundred and fifty.

The parent and its children now disagree by fifty thousand.

That difference is not an accounting nuisance. It is information, and it has exactly two possible meanings:

- **The detail is incomplete.** There is work inside "network build" that nobody has written down yet. It will still have to be done, and it will still be paid for.
- **The original figure was optimistic.** The early estimate was wrong, and the project is fifty thousand better off than it thought.

Both are things you want to know before the baseline is set. Neither is visible if nobody checks that the numbers reconcile.

## The three ways teams get this wrong

**Keeping the parent because it was approved.** Approval makes a number official. It does not make it achievable. If the parent stands and the detail is short, the work packages that must actually deliver the scope are underfunded from day one, and the variance will appear later as a delivery failure rather than as the estimating error it was.

**Overwriting the parent with the roll-up.** This makes the structure consistent immediately, which is why it is tempting. It also destroys the only evidence that a discrepancy existed. Reconciling by deletion removes the problem and the information at the same time.

**Covering scope "spiritually".** The engineer says the tooling is covered under systems, broadly speaking. Six months later, two packages each assume the other one paid for it. Scope covered spiritually is billed literally.

## Where unfunded scope actually goes

Work that is not in the structure does not stop existing. It arrives anyway, and it arrives at the worst possible commercial moment: after the tender, outside the competitive process, priced by whoever happens to be on site.

That is the real function of the 100% rule. It is not about tidy hierarchies. It is about ensuring that every piece of scope has an address, a budget and an owner **before** it is bought.

## Doing it properly without drowning in structure

The rule is often resisted because people imagine it demands enormous decomposition. It does not. It demands completeness at whatever level you have chosen to work.

Some practical guidance:

1. **Decompose to the level at which you will actually manage.** A work package should be small enough to have a clear finish and an accountable owner, and no smaller. Decomposing past the point where you will collect progress creates structure you cannot maintain.
2. **Put deliverables in the structure, not activities.** A WBS describes what will exist; the schedule describes what will be done. Mixing the two produces a structure that changes every time the sequence changes.
3. **Include the unglamorous scope.** Project management effort, commissioning, spares, training, documentation, decommissioning. These are the elements most often left out, and they are not small.
4. **Use a dictionary.** One or two sentences per package saying what is in and what is explicitly out. Most scope arguments are boundary arguments, and the boundary is only ever written down in the dictionary.

## The test worth running

Before any baseline is agreed, run the reconciliation deliberately: roll every leaf up and compare each parent to its children. It takes minutes, and it is the last cheap opportunity to find the difference between what you have budgeted and what you have described.

After that point, the same discovery has a different name. It is called a variance, and it arrives with an invoice attached.
"""),

        new("contingency-is-not-a-number-you-inherit",
            "Contingency is not a number you inherit",
            "Ten per cent is not an analysis. If you cannot say which risks your contingency covers, you are protecting a precedent rather than a project.",
            """["risk","contingency","estimating"]""",
            "Why percentage-based contingency fails, and how expected monetary value gives a contingency figure you can defend and adjust.",
            """
Ask where a project's contingency came from and you will usually get one of three answers: it is ten per cent, it is what we carried last time, or it is what the budget could absorb.

None of those is an analysis. All three are precedents, and a precedent protects itself rather than the project.

## What contingency is for

Contingency exists to cover the cost of things that are identified as possible but not certain. That is a specific job. It is not the same as management reserve, which covers what nobody identified, and it is not a margin for optimism.

If contingency has a job, it has a size, and the size can be worked out.

## The simplest defensible method

Take the risk register. For each entry, multiply the probability by the impact. Sum them. That total — the expected monetary value — is the amount you would expect the register to cost you, on average, across many runs of this project.

Three worked notes:

**Sign your impacts.** Threats are negative, opportunities positive. A register with only threats in it is not a register, it is a worry list, and it will systematically overstate exposure.

**Do not net an opportunity you do not control against a threat you own.** If a saving depends on another organisation's goodwill, it is not available to fund your exposure. Show it separately.

**Expected value is not a forecast of this project.** No single project experiences the average. A £180,000 expected value on a risk that either costs £1.2m or nothing will never cost £180,000. That is a reason to look at the distribution as well, not a reason to abandon the arithmetic.

## What the number is actually for

The value of doing this is rarely the total. It is the conversation the total starts.

When contingency is a percentage, the only available conversation is whether the percentage should be higher. When contingency is the expected value of a named register, four better conversations open:

- Which two or three entries drive most of the exposure?
- Can any of them be **bought down** more cheaply than they cost? A vessel-slot option, an early survey, a second supplier qualified in advance — each has a price that can be compared with the exposure it removes.
- Which entries are we carrying because we genuinely accept them, and which because nobody has been asked to own them?
- What does the register look like at the next stage gate, when several of these will have resolved one way or the other?

That last question matters more than it looks. Contingency should draw down as risks close out. A contingency line that stays flat all the way to completion was never linked to the register in the first place.

## When the number is bigger than the money

This is the common and uncomfortable case: the analysis says the exposure exceeds the contingency held.

There are three honest responses, and one dishonest one.

Honest: ask for more contingency, with the analysis attached. Reduce the exposure by treating the top entries. Or accept the gap explicitly, at the level of the organisation entitled to accept it, and record that you did.

Dishonest: report that contingency is adequate because the total is uncomfortable. An exposure you have quantified and then declined to mention does not become smaller. It becomes a surprise, and it arrives with your name on the analysis that saw it coming.

## The habit worth building

Bring the register's expected value to every contingency conversation, even when nobody asked. It reframes the discussion from *how much should we hold* to *what are we holding it against*, which is the question that actually has an answer.

And when someone proposes ten per cent, the useful reply is not that ten per cent is wrong. It is: ten per cent of what, covering which entries, drawing down when?
"""),

        new("float-is-an-asset-decide-who-owns-it",
            "Float is an asset — decide who owns it",
            "Total float gets consumed by whoever needs it first. On most projects that is an accident. It should be a decision.",
            """["schedule","float","commercial"]""",
            "Float is a project asset that gets spent by whoever reaches it first. How to allocate it deliberately instead of losing it by default.",
            """
Every network has float. Very few projects have an answer to the question of who owns it.

That matters, because float is spent whether or not anyone decides to spend it. The first party who needs it takes it, and by the time a second party needs it, it is gone.

## The three ways float disappears

**Given away.** A supplier asks to use the float on a delivery path for a cheaper shipping route. The saving is real, the float looks spare, and it is agreed in a phone call. Two months later the works that path feeds slip, and the buffer that would have absorbed the slip is on a ship.

**Consumed silently.** A contractor sequences their own work to suit their resources, using float that the network happened to have. Nothing is wrong, nothing is recorded, and the schedule is quietly less resilient than the last version anyone looked at.

**Hoarded.** The planner refuses to release any float on principle. A real saving is declined to protect a buffer that nothing is currently threatening. Protecting float unconditionally is not control; it is hoarding, and it costs money too.

## Float is not spare capacity

The word "spare" does most of the damage. Float is not spare. It is the amount of delay a path can absorb before it starts moving the completion date. Calling it spare is describing what has not happened yet as if it were a property of the schedule.

A useful reframing: float is the project's insurance policy against the delays nobody has identified. Spending it buys something else and cancels part of the policy. That can be an excellent trade. It should be a trade, made by someone who knew they were making it.

## Making it a decision

Four practices, in rough order of value:

1. **Report float, not just criticality.** "Non-critical" describes today. Float describes how long that stays true. A path with three weeks of float and a supplier who is habitually two weeks late is a different object from a path with three months.
2. **Name the near-critical paths.** Any path within a stated threshold of critical gets reported alongside the critical path, with its float stated. When it slips past that threshold, everyone already knows it has become a completion-date issue.
3. **Record float transfers.** When float is released — to a supplier, to a subcontractor's sequencing preference, to a client-driven change — write down who now has it and what the schedule looks like without it. This is a two-line note, and it is the difference between a documented commercial position and an argument in twelve months.
4. **Settle ownership in the contract where you can.** Many standard forms are silent or ambiguous on who owns float. Silence does not mean nobody owns it; it means whoever gets there first does.

## The trap in acceleration

Float ownership has a sharp edge during acceleration. Take four weeks out of the critical path and you have not simply finished four weeks earlier. You may have promoted a near-critical path to critical, and the float you thought you were protecting elsewhere has become the constraint.

Every successful acceleration creates a new critical path. Re-run the network before the next decision, not after it. Managing to a critical path you have not recalculated is worse than managing to none, because it is trusted.

## The one-sentence version

Float is a project asset with no default owner and a queue of people who will spend it. Decide who owns it, report how much of it there is, and record it every time it changes hands.
"""),

        new("three-eac-methods-and-the-assumption-behind-each",
            "Three EAC methods and the assumption behind each",
            "The forecasts disagree because they answer different questions. Publishing one without its assumption is publishing nothing.",
            """["forecasting","earned_value","governance"]""",
            "The three standard estimate-at-completion methods, what each one assumes, and how to present a forecast that survives challenge.",
            """
A project is a third of the way through its budget, behind on cost and behind on schedule. Three standard estimate-at-completion methods are available. They produce three materially different numbers.

This is not a defect. They are answering different questions, and the disagreement between them is the most useful thing on the page.

## The three methods

**Budgeted-rate.** Actual cost so far, plus the remaining work at its original budget. This assumes the overrun to date was caused by something that has finished happening — a one-off event, now closed — and that the rest of the project will run as planned.

**CPI-based.** Budget at completion divided by the cost performance index. This assumes the efficiency demonstrated so far is the efficiency you will get. Whatever is causing the overrun is a property of how this project executes, not an event.

**Composite.** Actual cost, plus the remaining work divided by the product of the cost and schedule indices. This assumes both cost and schedule performance persist, and that being late costs money — extended preliminaries, prolonged supervision, resource inefficiency.

## Choosing is an evidence question

The methods are not ranked by sophistication. Each is correct given its assumption, so the choice is a question about the project.

Ask: *what would have to be true for this forecast to be right?*

For the budgeted-rate method, the remaining work would have to run at a rate the project has not yet achieved in any period. That is a strong claim. It is sometimes true — a genuine one-off, a design problem now resolved, a supplier replaced — and when it is, you should be able to name the event and show that performance recovered after it, rather than merely stopped deteriorating.

For the CPI-based method, the claim is that recent history predicts the remainder. On a project with stable scope and a settled team, this is usually the safest single number.

For the composite, the claim is that lateness has a cost. On projects with heavy time-related overheads, it often forecasts best.

## Presenting a forecast that survives challenge

The strongest position in front of an assurance panel, a board or a lender is not a confident single number. It is a range with the assumptions named, and a statement of which one the evidence currently supports.

Three practical rules:

1. **Never present a forecast without its condition.** A forecast is a conditional statement. Delete the condition and you have published a number with no meaning and full authority, which is the worst combination available.
2. **Know your weakest assumption before the panel does.** If the optimistic method requires unprecedented performance, say so first. The honest answer costs one uncomfortable meeting and buys credibility for the remainder of the programme.
3. **Do not convert a forecast you doubt into a commitment.** Adopting an optimistic figure and promising a recovery plan that delivers it makes the project accountable for an outcome its own data does not support.

## The related index

The to-complete performance index answers a matched question: what efficiency would the remaining work have to achieve to still land on the budget?

It is the test for any recovery claim. If the index says the remaining work needs to run at a level the project has never reached, the recovery plan is a wish with a schedule attached — and the conversation should move to what specific, named, costed change would make that level achievable.

## Why this matters more than the arithmetic

None of this is difficult mathematics. All three methods are one line each.

The difficulty is institutional. Somebody has to be willing to present three numbers when the meeting wanted one, and to say which assumption they think is weakest. That is a harder skill than the calculation, and it is the one that makes the calculation worth doing.
"""),

        new("read-the-histogram-before-you-promise-the-date",
            "Read the histogram before you promise the date",
            "A schedule that assumes resource you do not have is a wish list with dates on it. The histogram is where that becomes visible.",
            """["resource","schedule","planning"]""",
            "Resource levelling explained: how to quantify an overload, and why a compressed schedule without the crews is a decision to fail on time.",
            """
It is possible to build a schedule that is logically perfect and completely undeliverable. The logic is sound, every dependency is right, the critical path is correct — and week three needs two hundred and sixty-five qualified people when the site has a hundred and fifty.

The network will not tell you this. Only the resource histogram will.

## What the histogram shows

Lay demand against available capacity, period by period. The overload in any period is simply demand minus capacity, floored at zero.

Three numbers matter:

- **Peak overload** — the worst single period. This is what determines whether the plan is achievable at all.
- **Total unfulfilled demand** — the sum across every overloaded period, in person-periods. This is roughly what you would have to hire, or what will slip.
- **Number of overloaded periods** — whether you are looking at one bad week or a structural problem.

A short, sharp peak and a sustained overload are different problems with different remedies, and the aggregate hides which one you have.

## Why compressed schedules produce this

Schedules get compressed towards the shortest logically possible duration, because that is what the software optimises and what the date pressure rewards. The shortest logical duration assumes infinite resource. Nothing in the critical path method models a crew that does not exist.

The result is systematic: the more schedule pressure a project is under, the more likely its plan silently assumes resource it cannot get.

## The three responses, honestly compared

**Level to available capacity.** Extend the duration until demand fits. This produces the honest date. It is usually unwelcome, and it is the only option that produces a plan the site can execute.

**Hire to the peak.** Bring in the extra crews. This keeps the date and costs money — often less than the recovery would, and the comparison is worth making explicitly.

**Overtime.** Reasonable for a small peak. At scale it buys fatigue and rework, which in a safety-critical environment is not a trade worth making. Overtime is a buffer with a declining exchange rate.

There is a fourth option that gets chosen most often, usually without being chosen: keep the dates and manage the shortfall week by week. This is not a plan. It is a decision to fail on schedule, made in advance and without the accompanying budget.

## Presenting it

The useful presentation is not "the schedule is not achievable". It is two costed options.

*Levelled to available crews, the shutdown takes N weeks. Hiring to the peak keeps the current date and costs X.*

That format hands the business a genuine choice with both prices visible, which is what a controls function is for. It also converts an argument about optimism into a decision about money, which resolves considerably faster.

## Small practices that prevent the problem

1. **Level before you baseline, not after you slip.** The histogram costs nothing to produce while the plan is still a draft.
2. **Model the constrained resource, not all of them.** Most projects have one or two genuinely scarce skills. Modelling everything produces noise; modelling the constraint produces a decision.
3. **Check the histogram after every acceleration.** Compressing the schedule concentrates demand. The acceleration that fixed the date frequently creates the overload.
4. **Be specific about capacity.** "Available crews" should mean crews that are qualified, inducted and actually allocated to this project — not the headcount on a regional org chart.

## The underlying point

A date is a claim about the future that depends on resources arriving. The schedule expresses the claim; the histogram tests it. Publishing the first without checking the second is publishing a hope in the format of a plan.
"""),

        new("what-a-good-change-log-looks-like",
            "What a good change log looks like",
            "A baseline that moves with expectation cannot measure anything. Approved changes belong in it; likely ones belong in the report.",
            """["change_control","baseline","governance"]""",
            "How to apply a change log correctly: what belongs in the baseline, where pending changes go, and why the distinction protects every variance you report.",
            """
A treatment works upgrade has four change requests: one approved, one rejected, one approved deletion, one pending. The monthly report has been adding all of them to the forecast.

This is a common failure and it is more damaging than it looks, because it does not produce one wrong number. It quietly invalidates every variance in the report.

## What a baseline is for

A baseline is a fixed reference. Its entire value comes from being fixed: variance is the difference between what happened and what was agreed, and if the agreement moves whenever expectations move, the difference measures nothing.

So the rule is narrow and worth stating plainly: **the baseline contains approved changes and nothing else.**

Not likely changes. Not changes that are certain to be approved. Not changes the sponsor has verbally agreed. Approved ones.

## Where the other categories go

They go in the report, clearly labelled, outside the baseline.

- **Pending changes** are an exposure. Show them as a separate line: *approved baseline X, pending changes Y, potential position Z.* The board sees what is coming and every variance stays measurable.
- **Rejected changes** disappear from the forecast entirely, and stay in the log. A rejected change that keeps reappearing in a different form is a governance signal worth noticing.
- **Deletions** are changes too. A descoped spare pump reduces the baseline exactly as an addition increases it, and it needs the same approval.

## The argument you will have

Someone will point out that a pending change is very likely to be approved, and that excluding it makes the forecast look better than it is.

They are right about the forecast, and wrong about the baseline. Both concerns are satisfied by putting the pending change in the report and out of the baseline. Excluding something from the baseline is never a reason to exclude it from the report.

The month a "certain" change is rejected is the month this discipline pays for itself. Everything in the report stays correct, and nobody has to work out which historical variances need restating.

## Practical mechanics

1. **One log, every request, every status.** Including the ones raised informally and never written up — those are the ones that appear later as claims.
2. **Cost and schedule impact on every entry.** A change with a cost impact and a blank schedule column has not been assessed; it has been priced.
3. **Show the net approved position separately from the gross.** Approved additions and approved deletions netting to a small number hides two real decisions.
4. **Date the approval, not the request.** The baseline changed when it was approved. That is the date variance analysis needs.
5. **Re-baseline rarely and visibly.** A re-baseline is legitimate after a major scope change and corrosive as a routine response to variance. When you do it, keep the previous baseline and be able to show performance against both.

## The tell-tale

Here is a quick diagnostic for any project you inherit: ask what the baseline was at the last three monthly reports, and what changed between them.

If the answer is a clean list of approved change references, the control environment is working. If the answer is that the baseline has been "updated to reflect the current position", then there is no baseline — there is only the current position, reported as though something were being measured against it.
"""),

        new("weight-before-percentage",
            "Weight before percentage",
            "Averaging package percentages measures packages. Projects need the weighted number, and the difference is not small.",
            """["progress","measurement","reporting"]""",
            "Why weighted progress is the only honest roll-up, how to choose weights, and the reporting habits that keep progress measurement credible.",
            """
Three packages report progress: enabling works ninety per cent, teaching block fifty-five per cent, external works twenty per cent. The steering group wants one number.

The tempting answer is the average: fifty-five per cent. It is easy to explain, easy to calculate, and it is answering a different question from the one that was asked.

## What the average actually measures

A straight average treats each package as equally important. It measures the average state of your packages, not the state of your project.

If the small external works package were at ninety per cent and the large teaching block at twenty, the average would be identical — and the project would be in a completely different position.

Weight each package by its share of the work, multiply by its percentage, and divide by the total weight. In the example above, with weights of thirty-five, forty and twenty-five, the weighted figure is 58.5 per cent. Not far from the average here, and that closeness is luck rather than a general property.

## Choosing weights

Weights should reflect share of the work, and there are three common bases:

- **Cost** — the default, and usually the most defensible. It reflects what the project is actually consuming.
- **Effort or hours** — better where the work is labour-dominated and costs are distorted by materials.
- **Value or deliverable count** — occasionally right, frequently a way to make progress look better than it is.

Pick one, write it down, and do not change it mid-project. Changing the weighting basis is the easiest way to manufacture progress, which is exactly why it needs to be visible when it happens.

## The harder problem: the percentages themselves

Weighting fixes the roll-up. It does nothing about the quality of the inputs, and the inputs are where progress measurement usually fails.

Some rules that help:

1. **Prefer rules to judgement.** Units completed, milestones achieved, or fixed-formula methods (0/100, 20/80) beat a percentage someone estimated. A rule cannot be optimistic.
2. **Beware the ninety per cent plateau.** Work that reports ninety per cent for three months was never at ninety per cent. It is the signature of judgement-based measurement meeting a package with a long tail.
3. **Measure what is done, not what is started.** Progress claims based on activity started reliably overstate, because starting is the easy part of most packages.
4. **Reconcile with cost.** If progress is running well ahead of spend, or well behind it, one of them is wrong. Which one is a useful question.

## What to do about the steering group

If a member proposes the straight average because it is simpler, show both numbers once, explain what the weighting represents, and then report one.

Publishing both every month is a worse habit than it looks. Two progress figures with no guidance invites each reader to select the one they prefer, which moves the judgement to the least-informed person in the room.

## The general principle

Every roll-up embeds an assumption about relative importance. Averaging does not avoid that assumption — it makes one, silently, that all things matter equally.

There is no neutral way to combine numbers. There is only a stated assumption and an unexamined one.
"""),

        new("when-the-data-is-wrong-before-the-project-is",
            "When the data is wrong before the project is",
            "A dashboard that is confidently wrong does more damage than one that is obviously broken. Report the quality of the data alongside the data.",
            """["data_quality","reporting","assurance"]""",
            "Practical data-quality control for project reporting: completeness, anomaly thresholds, correlated failures, and why cleaning outliers is editing.",
            """
A distribution programme reports weekly throughput from an automated feed. Six depots were expected. Five values arrived. Two of them are wildly outside plan, one high and one low.

The weekly pack is due in an hour.

## The choice that matters

There are three available responses, and the difference between them defines whether reporting is a control function or a publishing function.

**Publish with the quality stated.** The numbers go out with the completeness figure, the anomalies flagged, and a note on what is being checked. Readers can weigh the data properly, and the correction lands as follow-through rather than as a retraction.

**Clean it.** Drop the missing depot and the two outliers so the trend is readable. This is the response that feels most professional and is the most damaging: the cleanest-looking week in the report becomes the week the data was worst. Silently removing inconvenient records is not cleaning, it is editing.

**Hold the pack.** Defensible once. As a habit it means the report appears only when the news is good, and readers learn that quickly.

## The three measurements worth having

You do not need a data-quality platform to control this. Three numbers, computed alongside every reporting cycle:

- **Completeness** — records received against records expected. This is the single most valuable data-quality metric in project reporting and almost nobody publishes it.
- **Anomaly count** — records outside an agreed threshold from plan. The threshold is a decision, made in advance, written down. Deciding what counts as anomalous after seeing the data is not analysis.
- **Mean absolute error** — average distance from expected across the records received. It moves before individual records become obviously wrong.

## Correlated failures point at the pipeline

The most useful diagnostic instinct in this example is not about the depots at all.

One record missing, one value far high, one far low — in the same week. Three independent depots do not usually fail in three different directions simultaneously. The thing they share is the feed.

**When several independent things fail together, suspect the thing they have in common.** Investigating each depot separately eventually converges on one broken integration, having spent three managers' time and a week of goodwill explaining numbers they never reported.

## Why this is urgent rather than tidy

Bad data does not stay in the report. It becomes a plan.

The variance from a broken feed gets absorbed into next quarter's targets. An overstated throughput becomes a baseline someone is held to. A missing depot becomes an apparent decline when it reappears. By the time anyone traces it back, the number has been used to make decisions and the decisions have consequences.

That is why data quality belongs in the reporting cycle rather than in a separate improvement initiative. The cost of an unflagged bad number is not the number. It is everything built on it afterwards.

## The habit

Put the quality of the data next to the data, every time, whether or not it is good. A completeness figure of one hundred per cent takes one line and establishes that the line means something on the weeks when it does not.

Readers assume data is complete unless told otherwise. That assumption is the reader's, but it is your responsibility.
"""),

        new("the-path-nobody-watches",
            "The path nobody watches",
            "Projects are rarely surprised by the critical path. They are surprised by the one that was three weeks behind it.",
            """["schedule","critical_path","risk"]""",
            "Near-critical path analysis: why managing a single critical path is insufficient, and how to report float thresholds that make delays visible early.",
            """
Ask a delayed project which path caused the delay, and it is surprisingly often not the one they were managing.

The critical path gets attention, resource and daily reporting. The path with three weeks of float gets a status colour. Then the supplier on that path slips four weeks, and the project discovers it has been watching the wrong thing since spring.

## Why single-path management fails

Three structural reasons, none of them anybody's fault:

**Float is consumed invisibly.** A near-critical path can lose weeks of float without a single report changing colour, because nothing about it is late in the sense the report measures. It is simply less protected each month.

**Acceleration promotes paths.** Take four weeks out of the critical path and one of the near-critical paths may now be critical. Every successful acceleration creates a new critical path, and the network has to be re-run to find it. Managing to an out-of-date critical path is worse than managing to none, because it is trusted.

**Risk and criticality are different properties.** The highest-risk supplier is often not on the critical path. Accelerating them reduces risk on a path with float and does not move the completion date at all. Reducing risk and reducing duration are different objectives, and they usually have different targets.

## What to report instead

Report float, with a threshold.

Pick a value that reflects the project's volatility — two weeks on a fast fit-out, two months on a multi-year programme — and report every path within it as near-critical, with its float stated.

The practical effect is that the report now names the number that matters: *the baggage system route has five weeks of float; a supplier slip larger than that becomes a completion-date issue.* When the supplier notifies a six-week delay, nobody has to work out whether it matters. Everyone already knows the threshold.

## Two failure modes to avoid

**Reporting everything as critical to keep pressure on.** If everything is critical, nothing is, and the schedule stops informing any decision. Criticality inflation destroys the only signal a schedule sends.

**Reporting a near-critical path as simply non-critical.** Technically accurate, and it invites the reader to stop watching a route that is weeks away from becoming the constraint. "Non-critical" describes today; float describes how long that remains true.

## A short routine

1. After every schedule update, list paths by total float, ascending. Look at the top five, not just the top one.
2. Compare against the previous update. A path whose float has halved is more interesting than a path that is merely tight.
3. Cross-reference with the risk register. A path with modest float and a high-risk supplier is where the next surprise lives.
4. Re-run this immediately after any acceleration decision, before the next one.

## The generalisation

Most schedule surprises are not analytical failures. The information was in the network the whole time. They are reporting failures: the analysis was run, the result was summarised down to a single path, and the summary threw away the thing that mattered.

A schedule report that says only "the critical path is X" has answered a narrower question than the one the reader is actually asking, which is: *what could make us late?*
"""),
    };
}
