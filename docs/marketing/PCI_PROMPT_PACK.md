# PCI AI — the operator prompt pack

Prompts that work in **ChatGPT** and in **Claude** without rewriting, because they negotiate their
own capabilities before promising anything and end in a report you can check.

This does not replace `PCI_AI_Agent_Execution_Prompts.pdf`, which already covers 27 platforms one at
a time. It sits underneath it and fixes the four things that pack cannot do on its own: it tells the
agent what it is *actually* allowed to claim, it carries the brand instead of describing it, it
picks the day's work instead of waiting to be told, and it refuses to report success without
evidence.

---

## Read this once: what an AI can and cannot do to your accounts

You asked for prompts that make ChatGPT log into your platforms and post. Two of the three ways to
do that are real. One is not, and it is the one most people try first.

**Never give an agent your password.** Not in a prompt, not in a file, not "just this once". An
agent that asks for a credential has already failed the task, and a prompt that supplies one is a
security incident whatever it produces. Nothing in this pack asks for one.

| Route | What it is | Works? | Use it for |
|---|---|---|---|
| **API via connector** | ChatGPT talks to an MCP connector, Zapier, Make or Buffer, which posts through the platform's *official* API on a token you granted once | **Yes — the reliable one** | Everything scheduled and repeatable |
| **Agent drives a browser** | ChatGPT agent mode / Atlas operates a browser where *you* are already signed in | **Partly** — breaks on 2FA, CAPTCHA, and mobile-only uploads | One-off posts, research, engagement |
| **Agent logs in itself** | You hand over credentials | **No** | Never |

There is no first-party LinkedIn connector in ChatGPT or Claude. Every working method is a third
party sitting on LinkedIn's official API. Set that up once — **prompt S1 below does it** — and the
rest of this pack drives it.

**The tell that an agent is lying to you:** it reports "posted successfully" without a URL you can
open. Every prompt here ends in a report that makes that failure visible.

---

## The prompts

| | Prompt | When |
|---|---|---|
| **S1** | Connector setup | Once, before anything else |
| **P0** | Standing context | Paste once per session — or save as Custom Instructions / `CLAUDE.md` |
| **P1** | Capability handshake | First message of any working session |
| **P2** | Daily orchestrator | Start of each working day |
| **P3** | Brand and design brief | Before any asset is made |
| **P4** | Document/carousel builder | When the day's format is a carousel |
| **P5** | Post composer | Every post |
| **P6** | Claims audit | Before every publish, without exception |
| **P7** | Publish and verify | The publishing step |
| **P8** | Engagement | Daily, before posting |
| **P9** | Weekly close | Fridays |

---

## S1 — Connector setup (run once)

> You are setting up publishing automation for Project Controls Institute. I want to post to
> LinkedIn (company page and personal), and later Instagram, Facebook, Threads, YouTube and
> Medium, from inside this chat — without ever giving you a password.
>
> Do this in order and stop at the first thing that needs a decision from me.
>
> 1. Tell me which of these you can actually reach from this session **right now**: custom MCP
>    connectors, browser control, file upload, image generation, scheduled runs. Answer as a plain
>    list of yes/no. Do not guess — if you are unsure whether a capability is enabled, say unsure.
> 2. Based only on what you answered yes to, recommend **one** publishing route: a hosted MCP
>    connector, a Zapier/Make automation, a scheduler like Buffer, or browser-driven posting.
>    Give me the specific service name, what it costs, and what it can and cannot post.
> 3. Give me the exact click-path to authorise it — where I go, what I grant, what token or
>    permission scope is created, and where it is stored. I will do this part myself.
> 4. Tell me the failure modes: what breaks when the token expires, what needs a human, and what
>    that looks like from your side so you can report it instead of silently failing.
> 5. Write me a one-line test: the smallest possible post that proves the pipe works end to end.
>
> Do not write any marketing content in this session. Setup only.

---

## P0 — Standing context

Paste once per session. Better: save it as ChatGPT **Custom Instructions**, or as a project file /
`CLAUDE.md` in Claude, so it applies to everything without re-pasting.

> **PCI STANDING CONTEXT — applies to everything you produce for me.**
>
> **Who this is.** Project Controls Institute Global (PCI). An independent certifying body. Three
> AI-era credentials, each with its own Body of Knowledge and its own examination:
>
> - **PCL-AI** — PCI AI Project Controls Leader — 13 domains, 61 knowledge areas
> - **PFL-AI** — PCI AI Project Finance Leader — 16 domains, 61 knowledge areas
> - **PML-AI** — PCI Project Management Leader – AI — 16 domains, 63 knowledge areas
>
> Use those names exactly. PML-AI's is worded differently from the other two on purpose.
>
> **The one idea everything ladders to.** A chartered accountant is examined on when revenue may be
> recognised and what a provision must satisfy, and almost never on a critical path or an earning
> rule. An engineer is examined on float and progress measurement, and almost never on cut-off or a
> contract asset. A project lives in the overlap, and the overlap is where the money is lost. PCL-AI
> is the credential that examines both sides. If a post does not connect to that, ask whether it is
> worth publishing.
>
> **Audience.** Project controls managers, planners and schedulers, cost engineers, EVM specialists,
> risk managers, PMO leads, project finance professionals, megaproject directors. Senior, 8+ years,
> in the UK, US, the Gulf, India, Nigeria and Australia. They have seen every piece of LinkedIn
> hype. They respond to arithmetic they can check.
>
> **NUMBERS YOU MAY PUBLISH.** These are verified. Do not round them, do not "improve" them, do not
> add to this list:
>
> - 13 / 16 / 16 domains and 61 / 61 / 63 knowledge areas across PCL / PFL / PML
> - 92 sector case studies across the three volumes (26 + 33 + 33)
> - 113 mandatory PCI Standards carrying 532 process requirements
> - 40 / 40 / 20 — the *Body of Knowledge's* proportions: finance and reporting, project management,
>   governed AI
> - 15,613 machine calculation checks, all passing — **and you must say in the same sentence that
>   this covers PFL-AI and PML-AI only.** PCL-AI has no equivalent suite. Quoting the figure without
>   that scope is a false claim.
>
> **NUMBERS YOU MAY NOT PUBLISH.** Not because they are secret — because they cannot currently be
> defended if a reader asks:
>
> - Any examination weighting. The syllabus is settled; the exam blueprint is an open decision.
> - Worked-example counts, question-bank counts, student numbers, pass rates, salary uplift.
> - Anything about how many people hold a PCI credential.
>
> **NEVER, under any framing:**
>
> 1. Claim accreditation, recognition, endorsement, affiliation or partnership that has not
>    happened. Naming an external standard implies nothing about its publisher's view of PCI.
> 2. Tell anyone they have been awarded, approved or guaranteed anything. You may invite someone to
>    be *considered* and state the criteria. No outcome promises, ever.
> 3. Describe the PCI Standards as law. They are certification requirements established by the
>    Institute. Nothing PCI publishes is legal, tax or accounting advice.
> 4. Reproduce protected text, tables, figures or questions from ISO, IFRS, IAS, PMI, AACE or any
>    other publisher. Name them and describe them in PCI's own words. Never quote them.
> 5. Invent a statistic, a testimonial, a case study, a project detail or a source. If you cannot
>    point at where a number came from, cut the sentence. This is the rule that matters most.
>
> **Links.** Every link must land on `projectcontrolsinstitute.org`. Any other domain is off-estate
> and does not get published.
>
> **Tone.** British English. Short sentences. Specific over clever. No em-dash pile-ups, no "delve",
> no "in today's fast-paced world", no three-item lists used as rhythm, no emoji as bullets. Write
> like a practitioner who has run a month-end, not like a brand.
>
> Confirm you have read this and state the one idea everything ladders to, in your own words. Then
> wait.

---

## P1 — Capability handshake

The prompt that makes the rest portable. Run it first, every session, in either tool.

> Before you do any work, tell me exactly what you can do in **this** session. Answer this list and
> nothing else. "Unsure" is an acceptable answer and is far better than a guess:
>
> - Can you open a URL and read the live page?
> - Can you control a browser where I am already logged in?
> - Can you call an external API or a connector that publishes on my behalf? Name it.
> - Can you generate an image file? Can you generate a multi-page PDF?
> - Can you read a file I upload? Can you give me a file back?
> - Can you run on a schedule without me present?
>
> Then say, in one line: **"This session can publish: [yes, via X / no — I can prepare only]."**
>
> From here on, you never claim to have done something outside that list. If a later instruction
> needs a capability you just said no to, stop and tell me which one, rather than producing a
> plausible substitute.

---

## P2 — Daily orchestrator

The prompt the existing pack is missing: it decides *what* to do, then hands off.

> You are running PCI's publishing for today. Do not wait to be told what to post.
>
> **Look first.**
> 1. Open the PCI LinkedIn company page and the founder profile. Read the last 10 posts on each:
>    topic, format, date, and how each performed.
> 2. List every comment from the last 7 days with no reply.
> 3. Name the three topics from the PCI syllabus that have **not** been posted about in 21 days.
>
> **Then decide, and show your reasoning in three lines.**
> - Which format today, and why that format for that topic. LinkedIn documents outperform
>   everything else, so a carousel is the default and anything else needs a reason.
> - Which credential it ladders to — PCL-AI, PFL-AI or PML-AI. One of them, not all three.
> - What the single takeaway is, in one sentence a reader could repeat to a colleague.
>
> **Rules on what to choose.** Rotate the teaching, do not rotate the message. Pick topics where the
> arithmetic does the persuading: a CPI that reads differently before and after an accrual, the four
> EAC methods that give four answers from one month's data, earned value over a backlog, a contract
> asset that exists on a fully billed project, what an AI model's precision and recall have to clear
> before it is worth running. Never post twice on the same idea in a fortnight, and never post the
> same content to the company page and the personal profile — it suppresses both.
>
> **Then produce, in order:** the engagement pass (P8), the asset (P3 and P4), the copy (P5), the
> claims audit (P6), and only then publish (P7).
>
> Stop after your three-line decision and let me confirm before you build anything.

---

## P3 — Brand and design brief

This is the prompt that turns "design it well" into something an agent can execute. Give it every
time an asset is made — in ChatGPT, in Canva's AI, in any image tool, to a designer.

> **PCI visual identity — follow this exactly. Do not improvise a look.**
>
> The identity comes from the PCI logo: a navy field, the letters AI in gold, and a crimson bar
> beneath the wordmark.
>
> **Colour — these hex values, no others:**
> - Navy field: `#1D3C92` to `#13245A`, gradient at 158°
> - Gold: `#E7CB82` — the accent, and the only colour a headline word is ever picked out in
> - Crimson: `#C13329` — the bar under every heading. It is the mark that says PCI. It never
>   changes width and it is never used for anything else
> - Ink `#0F172A`, slate `#475569`, mist `#64748B`, light ground `#F5F7FA`
> - **Do not use `#1D4ED8`.** It appears all over the PCI website but it is the *link* blue, not the
>   brand. Built on it, a design looks like a generic SaaS template.
>
> **Type:**
> - Display: **Archivo**, weight 800–900, letter-spacing `-0.022em`. Tight and heavy.
> - Body: **Inter**, 400–700.
> - Numbers always tabular. Figures are the point of this subject; they must line up.
>
> **Format:** LinkedIn documents are **1080 × 1350** (4:5). Not A4 — a tall page renders at about
> 9 px body text on a phone and nobody reads it. 4:5 is the tallest ratio the feed shows uncropped.
>
> **Composition:**
> - One idea per slide. If a slide needs two, it is two slides.
> - Eyebrow in small caps, then the headline, then the crimson bar, then the content. Every slide.
> - Body type no smaller than 16 pt at 1080 wide. Read it at phone size before you accept it.
> - Generous margins — 80 pt or more. Crowding reads as cheap.
> - Alternate navy slides and light slides so the deck has rhythm rather than 15 identical panels.
> - Every slide footer carries **PCI PCL-AI** (or the relevant credential). Single slides get
>   screenshotted out of a deck and must still say what they belong to.
>
> **Forbidden:** stock photography of people in hard hats pointing at things. Clip art. Emoji as
> section markers. Gradients other than the navy field. Drop shadows on type. More than one
> typeface family. Any colour outside the list above.
>
> **If the subject is quantitative, draw it.** Project controls is curves, fans and variances. A
> deck about it with no figure in it is a text document cut into slides. But a bad chart is worse
> than none: label every series directly, never rely on colour alone to tell two things apart, and
> put the number the reader needs on the mark rather than in a legend.

---

## P4 — Document/carousel builder

> Build the complete LinkedIn document now, to the P3 brief. 1080 × 1350, 8–14 slides.
>
> **Structure:**
> 1. Cover — the credential, the promise, one line. It must be legible as a thumbnail.
> 2. The problem — a specific situation the reader has been in.
> 3. The evidence — the arithmetic, worked. This is the slide people screenshot.
> 4–9. The teaching, one idea per slide.
> 10. What it means on Monday morning.
> 11. Where this sits in the PCI syllabus — the domain, named.
> 12. The close — the principle, and where to go next.
>
> **Before you show it to me, check each slide yourself:**
> - Does any line of text touch or overlap another element?
> - Is any text below 16 pt at 1080 wide?
> - Does every slide footer name the credential?
> - Is there a claim on any slide that is not in the approved numbers list in P0?
> - Would slide 3 make sense to someone who saw only slide 3?
>
> Fix anything that fails, then render each slide as an image and **look at them** before you tell
> me it is done. Describe what you see, not what you intended. If you cannot render and look, say
> so — do not tell me a layout is clean when you have not seen it.

---

## P5 — Post composer

> Write the post body for the asset you just built.
>
> - The first two lines must earn the "see more" without being clickbait. Lead with the specific
>   thing — a number, a situation, a claim someone might argue with. Never "Here's why…".
> - 1,300–1,900 characters. Short paragraphs. Line breaks that let the eye move.
> - One insight, worked. Not five pieces of general advice.
> - Close with a real question — one a practitioner would actually answer, not "thoughts?"
> - **No link in the body.** It suppresses reach. The link goes in your first comment, with the UTM.
> - 3–5 hashtags at the end, no more.
> - British English. No em-dash pile-ups. No "in today's landscape". No emoji bullets.
>
> Then write the first comment separately: one line of context and the UTM link.
>
> Show me both. Do not publish yet.

---

## P6 — Claims audit

**Run this before every publish.** It is the cheapest insurance in the pack.

> Audit the post and every slide of the asset against the PCI rules, as a hostile reader would.
>
> Go claim by claim. For each factual statement, output one line:
>
> `CLAIM: [the sentence] → SOURCE: [where it comes from] → VERDICT: keep / scope / cut`
>
> Apply these tests:
> 1. Is the number on the approved list in P0? If not, cut it.
> 2. If it is 15,613, does the same sentence say PFL-AI and PML-AI only? If not, scope it.
> 3. Does anything imply accreditation, endorsement, recognition, affiliation or a partnership?
> 4. Does anything promise an outcome, an award, or a guarantee?
> 5. Does anything describe the PCI Standards as law, or read as legal, tax or accounting advice?
> 6. Is any external standard's actual text, table or question reproduced rather than described?
> 7. Is there an examination weighting anywhere?
> 8. Does every link land on projectcontrolsinstitute.org?
>
> End with one line: **AUDIT: pass** or **AUDIT: fail — [n] items to fix.**
>
> If it fails, fix them and run the audit again. Do not publish on a failed audit, and do not ask me
> whether to proceed anyway.

---

## P7 — Publish and verify

> Publish now, using the route you declared in P1.
>
> 1. Upload the asset. Confirm the page count and that the thumbnail is the cover.
> 2. Paste the body. Preview it. Check the first two lines are not truncated mid-word.
> 3. Publish.
> 4. Post the first comment with the link immediately — within 60 seconds.
> 5. **Open the live URL and read the post back to me.** Quote the first line of what you see.
>
> Then report exactly this and nothing else:
>
> ```
> PUBLISHED:  [format] — [topic]
> URL:        [live URL]
> READ BACK:  [first line as it appears on the live page]
> ASSET:      [n slides / image / none]
> COMMENT:    [UTM link, posted yes/no]
> AUDIT:      pass
> LOG LINE:   Platform=[ ] | Type=[ ] | Topic=[ ] | Brand=[ ] | Status=Published | Date=[ ] | URL=[ ]
> NEXT:       [one line]
> ```
>
> If anything blocked you — login, 2FA, CAPTCHA, a permission, an upload limit — stop at that point,
> keep the finished content and asset so nothing is lost, and report:
>
> ```
> BLOCKED AT:  [exact step]
> REASON:      [exact reason]
> I NEED:      [one single action from me]
> HOLDING:     [where the finished asset is]
> ```
>
> Do not report a URL you have not opened. Do not paraphrase the read-back.

---

## P8 — Engagement (before posting, not after)

> Before publishing anything today, do the engagement pass. Reach follows this, not the other way
> round.
>
> 1. Reply to every unanswered comment on PCI posts from the last 7 days. Every reply adds
>    something: a clarification, a number, a follow-up question, a caveat. Never "Thanks for
>    sharing". Never argue in public.
> 2. Comment substantively on 15 posts from the target audience — at least 5 from actual prospects
>    (project controls managers, PMO heads, planning leads, cost managers). A substantive comment is
>    2–4 sentences, adds a distinct point or a counter-example, and would stand alone as useful.
>    Never pitch in a comment.
> 3. Escalate to me rather than answering: complaints, anything legal, partnership approaches,
>    disputes about certification status, and any question about accreditation.
>
> Report: replies made, comments made, and the three most interesting things you read — those are
> next week's topics.

---

## P9 — Weekly close

> Close the week against the Growth OS workbook.
>
> 1. List every post published this week: platform, topic, format, URL, engagement.
> 2. Name the best and the worst performer and say, in one line each, why — grounded in what
>    actually differed, not in a general theory.
> 3. Compare against last week: posts, engagement, followers, enquiries.
> 4. Give me the log rows to paste into **Content Calendar** and **DAILY ENTRY**, formatted as the
>    workbook's columns, ready to paste.
> 5. Name one thing to stop, one to start, one to keep.
> 6. Propose next week's five topics with the format for each and a one-line reason.
>
> Be blunt about what did not work. A weekly review that says everything went well is useless.

---

## Using this with Claude Code

The prompts above assume an agent with a browser. Claude Code has the repository, which makes it
better at the part ChatGPT is worst at — building the asset properly and keeping it reproducible.

> Build this week's LinkedIn document in the repo, not by hand.
>
> - Follow `docs/marketing/build_outline_post.py` for the pattern: brand tokens at the top, slides
>   as functions, brand fonts embedded from `backend/wwwroot/assets/fonts/`.
> - Pull every number and formula from the manuscripts at build time and assert it, the way
>   `formulas()` does. A figure typed by hand is a figure that can go stale silently.
> - Run `assert_on_brand()` — the build fails on any glyph the brand fonts do not carry, which is
>   how off-brand fallbacks get caught before they reach a slide.
> - Render every slide to PNG and look at them. Report what you see. Layout bugs live in the gap
>   between the source and the output, and only looking finds them.
> - Then run the P6 claims audit against the rendered text.
> - Commit with a message that says what changed and why, and push.

---

## What this pack cannot fix

The credibility problem is not a content problem. PCI has no accreditation yet, and the corpus has
no named human reviewer — Gates 3 and 13 are open in `docs/books/reports/SIGN_OFF_REGISTER.md`.

That costs roughly half your conversion rate against an established body, which is the single
largest number in the growth model. No prompt closes it. It closes when accreditation progresses,
when reviewers are named, when employer logos exist and when pass rates can be published.

Until then the honest position is the one these prompts enforce: lead with the rigour that *is*
demonstrable — the arithmetic, the standards, the compliance tests — and say nothing about review
status unless someone asks, at which point answer plainly.
