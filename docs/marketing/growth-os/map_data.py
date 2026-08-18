"""MAP copy — what every sheet is, in plain English.

A readability review rated the old MAP 7/10: its descriptions were terse noun
phrases with no verbs ("Tracked links — the only links we share"), which read
as labels rather than explanations. A new joiner could not tell from them who
touches a sheet or what they are supposed to do on it.

Each row: (sheet, what it is, who types on it, what you do here and when).
"""

EVERYONE = "Everyone"
MANAGER = "Manager only"
READONLY = "Nobody — it calculates"
READING = "Nobody — read it"

MAP_GROUPS = [
 ("READ FIRST  —  what a new joiner reads, in this order", [
  ("START HERE",
   "The rulebook. It sets out how the team works: what to log, the daily targets "
   "everyone is measured against, the golden rules that protect PCI's reputation, "
   "the team roster and the five web domains we own.",
   "Manager fills the yellow settings once",
   "Read it end to end on your first day — it takes about five minutes. Come back "
   "whenever you are unsure what a rule actually says."),
  ("TEAM GUIDE",
   "The onboarding sheet. It answers the one question everyone asks — 'I just did "
   "this thing, where do I log it?' — for every kind of work, and explains how your "
   "score is calculated.",
   READING,
   "Read it after START HERE. Keep it open for your first week; the 'where do I log "
   "what' table settles almost every question."),
  ("GROWTH PLAYBOOK",
   "The 23 growth techniques PCI actually uses, each written out in working detail: "
   "why it works, the exact steps, how often, and the number that proves it worked.",
   READING,
   "Read the technique before you start that kind of work. It also lists what NOT "
   "to do — tactics that get accounts penalised."),
  ("PLATFORM GUIDE",
   "One row for every platform we are on, saying what to do there each week, how "
   "often, and the single number that proves it is working. Setting the account up "
   "is on Platform Setup; this is what you do once it exists.",
   READING,
   "Look up your platform before your first session on it. Gold rows are the "
   "highest-value platforms — if time is short, work down from the top."),
  ("PR & Target Directory",
   "The named routes out: which publications accept contributed articles, which "
   "podcasts take guests, which boards and directories are worth applying to — and "
   "which ones we have deliberately decided to skip, with the reason.",
   MANAGER,
   "Check it before you approach anyone. If a route is marked as a skip, do not "
   "spend time on it."),
  ("Glossary",
   "Every term this workbook uses without stopping to explain it — ICP, canonical "
   "link, dwell time, EVM, person-day — in plain English, grouped by where you meet "
   "the word.",
   READING,
   "Look a word up the moment it stops you. If a term is missing, say so in the "
   "Monday review."),
  ("UPGRADE NOTES",
   "The changelog. Every change made to this workbook, what was tested, what was "
   "found and what was fixed.",
   READING,
   "Read it when the file changes, so you know what moved and why."),
 ]),

 ("DAILY WORK  —  the sheets you type on", [
  ("DAILY ENTRY",
   "The one sheet everybody fills in. One row for each thing you do: the date, your "
   "name, which platform, what kind of work, how many, how long it took, and a link "
   "that proves it. Every management page in the file is built from these rows.",
   EVERYONE,
   "Add the row when you FINISH the task, not at 5pm. Ten rows a day is normal. Set "
   "both tags — Objective and For (brand) — on every row."),
  ("LinkedIn Outreach",
   "One row per PERSON you contact on LinkedIn. It carries the lead from research "
   "through the connection request, the message, the reply and the meeting to the "
   "sale. This is the honorary-certification engine.",
   EVERYONE,
   "Fill columns A-L when you research the lead, M-T when you send and hear back, "
   "AD onward only if it becomes a meeting or a sale. Column AN warns you if the "
   "person is already in the sheet."),
  ("Partnership Pipeline",
   "One row per ORGANISATION rather than per person: associations, universities, "
   "employers and training partners, from first contact to a signed agreement.",
   EVERYONE,
   "Open a row when you first approach them. A deal only counts as revenue once a "
   "real contract-signed date is in the sheet."),
  ("Content Calendar",
   "Every piece of content, from the idea to the published URL and the numbers it "
   "produced — impressions, engagements, clicks and leads.",
   EVERYONE,
   "Add a row when you plan the piece; set Status to Published AND fill the "
   "published date on the day it goes live. Missing either one breaks the reporting."),
  ("Content Scheduler",
   "One row per running schedule — a platform, at a cadence, between two dates. It "
   "works out how many posts that schedule promised and how many actually went out, "
   "so you can see which schedules are slipping.",
   EVERYONE,
   "Set one up when you commit to a cadence. Coverage under 100% means you are "
   "behind on that schedule. The table below the grid says which platforms can "
   "schedule natively and with what limits."),
  ("Community & PR",
   "Everywhere you show up as a helpful expert rather than a seller: forum answers, "
   "community threads, press mentions, podcast appearances and journalist requests.",
   EVERYONE,
   "One row per thread started or substantive answer given — never one per like or "
   "one-line comment."),
  ("Job Postings",
   "Every open PCI role, on every board it is posted to. One row per platform per "
   "position, with the applicant count.",
   EVERYONE,
   "Add a row each time you post a role somewhere. Update the applicant count when "
   "you check the board."),
  ("Link Building",
   "Off-page SEO in one place: every site we want a link from, who we contacted, "
   "what we offered, and whether the link actually went live.",
   EVERYONE,
   "One row per prospect. Fill the date-live column only when you can see the link "
   "on the page — that is what the weekly backlink number counts."),
  ("Experiments",
   "A/B tests written down honestly: what you changed, what you expected, what "
   "happened, and what you will do about it.",
   EVERYONE,
   "Start a row before you run the test, not after. Record the result even when it "
   "is disappointing — that is the point of the sheet."),
  ("UTM Builder",
   "Builds the tracked links we share. A link without one of these tags is invisible "
   "in analytics, so nobody can tell where an enquiry came from.",
   EVERYONE,
   "Build the link here first, then paste it into the post. Never share a bare URL "
   "in a campaign."),
  ("SEO Clusters",
   "The seven big subjects we intend to own in Google. Each has one long pillar page "
   "and the supporting articles that link up to it, with the Search Console numbers "
   "beside them.",
   "Whoever owns SEO",
   "Use it to see how a piece of content fits the plan before you write it. Column F "
   "gives the Article Bank ID of the brief for each supporting article."),
  ("Keyword Plan",
   "76 researched search terms, graded Easy, Medium or Hard from what actually ranks "
   "today, with the ten to attack first and the reason each one was chosen.",
   "Whoever owns SEO",
   "Pick your next article from here, Easy first — a new site does not beat PMI or "
   "AACE on a hard term. Column P gives the Article Bank ID of its brief."),
  ("Article Bank",
   "5,683 ready-to-write article briefs. Each one carries the title, the keywords to "
   "use, who it is for, how long it should be, and a full AI writing prompt you can "
   "copy straight into your writing tool.",
   "Writers claim rows",
   "Filter to your pillar or cluster, pick a P1 row first, put your name in Owner, "
   "paste the prompt into your AI tool — then EDIT it like an expert. The prompt "
   "bans invented statistics; anything factual must be checked before it is published."),
  ("Daily Log",
   "An optional one-line-per-day summary. Everything on it is calculated from DAILY "
   "ENTRY, so it is a convenience, not a second place to type.",
   READONLY,
   "Ignore it unless you like a daily digest. Nothing depends on it."),
 ]),

 ("RESULTS  —  read-only pages. Nobody types on these", [
  ("Weekly Pulse",
   "This week against last week, and against the average of the four completed weeks "
   "before it. Green means up, red means down.",
   READONLY,
   "The manager's first stop on Monday. If a high-value effort is falling week on "
   "week, rebalance before month-end."),
  ("Dashboard",
   "The cumulative record — everything achieved since the programme started, with "
   "headline KPI tiles down the right-hand side and a DATA HEALTH block at the "
   "bottom that counts logging mistakes.",
   READONLY,
   "Check DATA HEALTH before quoting any number on this page to anyone. Every "
   "figure in that block should read zero."),
  ("Summary",
   "One page, readable in sixty seconds: the six numbers that matter, volume "
   "delivered, what needs a decision, and the daily rhythm expected of the team.",
   READONLY,
   "This is the page to forward upwards. Print it for a board or investor update."),
  ("Objective Performance",
   "The same results split two ways: by campaign (honorary outreach, certification "
   "sales, authority building…) and by brand (the institute, each certification, PCI "
   "World, Certuvo). It also shows minutes per person per campaign.",
   READONLY,
   "Compare Value rank against Share of minutes. If the highest-value campaigns are "
   "getting the smallest share of the team's hours, move people."),
  ("Team Scorecard",
   "Every person's outreach numbers side by side — volume, acceptance rate, reply "
   "rate, meetings — with a one-sentence verdict per person.",
   READONLY,
   "Read column Q first: it says in plain words what each person should fix."),
  ("Employee Score",
   "The weighted score out of 100 used in reviews. Quality counts far more than "
   "volume, and no score appears until there is enough history to be fair.",
   READONLY,
   "Look at the 'What to fix first' column, not just the number. A blank grade means "
   "not enough data yet, not a bad result."),
  ("Weekly Review",
   "The Friday page: what went well, what did not, and next week's focus, with the "
   "week's numbers already filled in beside your notes.",
   "Everyone writes their own notes",
   "Fill it in every Friday. Use the dropdown at the top right to see just your own "
   "week."),
  ("Platform Progress",
   "How much has actually happened on each of the 133 platforms, how long ago, and "
   "who touched it last — set against how valuable that platform is.",
   READONLY,
   "Use it to find neglected high-value platforms. The Attention column tells you "
   "which ones have gone quiet."),
  ("Who Did What",
   "A grid of every platform against every person, so you can see at a glance who "
   "has been carrying which channel.",
   READONLY,
   "Use it when reallocating platforms, or when someone is away and their channels "
   "need covering."),
  ("Accounts Register",
   "Every account we hold: who owns it, whether two-factor is on, and the reference "
   "to the entry in the password manager.",
   MANAGER,
   "Passwords are NEVER stored here — only the vault reference. Check it when "
   "somebody joins or leaves."),
 ]),

 ("MANAGEMENT  —  the manager sets these up", [
  ("Master Tasks",
   "The full workstream list: every task needed to build the marketing function, "
   "with an owner, a due date and what 'done' looks like.",
   MANAGER,
   "Assign an owner and a due date to every row before rollout. Update Status as "
   "tasks move."),
  ("Platform Setup",
   "All 133 platforms with the steps to create and finish each account, its priority "
   "and value rank, which countries it is strongest in, and which brand it serves.",
   MANAGER,
   "Work down it when setting the estate up. Mark Status = Complete only when the "
   "profile is genuinely finished — logo, bio, link and two-factor."),
  ("Publishing Plan",
   "The ten publishing platforms, ranked, with the rule for each: what may be "
   "published there and what must be set (such as the canonical link).",
   MANAGER,
   "Read the rule for a platform before publishing there the first time."),
  ("Channel Costs",
   "What each paid tool and channel costs per month, and the economics that fall out "
   "of it: cost per meeting and revenue per pound of channel cost.",
   MANAGER,
   "Fill the yellow cost cells. The block underneath then computes the return on "
   "its own."),
  ("QA & Compliance",
   "The fifteen checks that protect PCI's reputation and keep the accounts alive — "
   "honesty of claims, message limits, data protection, platform rules — several "
   "with a live pass or fail signal.",
   MANAGER,
   "Work through it monthly and sign it off. An unowned checklist is not a control."),
  ("Message Bank",
   "The approved outreach messages — connection notes, first messages and follow-ups "
   "— each with a live character count against the platform's limit, and a signed "
   "approval. Nothing that is not on this sheet may be sent.",
   MANAGER,
   "Copy the approved text and replace every [bracket] with something real from that "
   "person's profile. Never write outreach from scratch."),
 ]),

 ("REFERENCE  —  look these up when you need them", [
  ("LinkedIn Playbook",
   "The outreach method in order, from building the saved searches to handing a warm "
   "lead over — the detail behind what the Outreach sheet asks you to log.",
   READING,
   "Read it before your first day of outreach, then dip back into it when a step "
   "stops working."),
  ("How-To Guides",
   "A short training manual per workstream: what you do each week on it and what "
   "good looks like.",
   READING,
   "Read the row for a workstream when you pick it up for the first time."),
  ("Benchmarks",
   "What good numbers actually look like in 2026, with the source named for every "
   "figure and an honest note about where those figures come from.",
   READING,
   "Use it to judge whether a rate is genuinely poor or simply normal. Treat vendor "
   "benchmarks as a sanity check, not a law."),
  ("Lists",
   "The source values behind every dropdown in the workbook, plus the rule for how "
   "each platform should be logged so the same work is never counted twice.",
   MANAGER,
   "Only the manager changes these. Renaming a value here disconnects every row "
   "already logged against it."),
 ]),
]
