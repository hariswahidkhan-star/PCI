"""Per-platform usage playbooks — the step-by-step 'how to use it' guide.

USAGE[name] = (steps, kpi, time_per_week). Covers every canonical platform;
finish_v8 asserts full coverage so a renamed platform fails the build.
Setup (accounts, profiles) lives in Platform Setup column E — these are the
WEEKLY PLAYS once the account exists.
"""

USAGE = {
 "LinkedIn Company Page": (
  "1) 3 posts/week from Content Calendar (carousel, credential story, poll). 2) Reply to every comment within 60 minutes of posting (first-hour velocity gates reach). 3) Reshare each post from personal profiles next morning. 4) Log each post in Content Calendar with URL.",
  "Follower growth + engagement rate per post", "3 h"),
 "LinkedIn Personal Profile": (
  "1) Work the daily targets: research leads, send personalised connects, message accepted ones (Message Bank approved text only). A FREE LinkedIn account allows only a handful of personalised connection notes per month — check the current cap on the account before planning the week; Premium/Sales Navigator seats are what make the note-led targets reachable. 2) Work the daily engagement target from START HERE §3 (15 likes + comments on other people's posts), of which at least 5 are substantive comments on prospect posts. 3) Log every touch in LinkedIn Outreach same day.",
  "Acceptance % and reply % (Dashboard §3)", "5 h/day team-wide"),
 "LinkedIn Sales Navigator": (
  "1) Build one saved search per segment (Lead Segment list). 2) Pull 30 qualified leads/day per person into LinkedIn Outreach with the why-them note. 3) Refresh saved-search alerts each Monday.",
  "Qualified leads logged/day vs target", "1 h/day"),
 "LinkedIn Articles": (
  "1) One article/month per named leader (repurpose a Website/Blog original, canonical to site). 2) 1,200+ words, credential CTA at the end. 3) Log in Content Calendar as Repurposed.",
  "Article views + profile visits", "2 h/month"),
 "LinkedIn Newsletter": (
  "1) One issue every fortnight from the Company Page. 2) Subject = a question practitioners ask (steal from Keyword Plan Easy queries). 3) End every issue with one credential CTA.",
  "Subscriber growth per issue", "2 h/fortnight"),
 "LinkedIn Groups": (
  "1) 3 useful answers/week in the 10 joined groups — answer, never pitch. 2) Note engaged members as leads in LinkedIn Outreach. 3) Log in Community & PR.",
  "Answers logged + leads sourced from groups", "1.5 h"),
 "LinkedIn Live": (
  "1) One live/month: expert interview or exam-prep clinic, promoted 10 days ahead. 2) Multistream via StreamYard to YouTube. 3) Clip 3 shorts from the recording within 48h.",
  "Live viewers + replay views", "4 h/month"),
 "YouTube": (
  "1) One long-form/week (webinar cut, expert interview, how-to) + 3 Shorts clipped from it. 2) Titles from Keyword Plan questions; chapters + description links with UTMs. 3) Playlists per certification; exam-prep playlist as a YouTube Course.",
  "Watch time + subscribers; clicks to site (UTM)", "4 h"),
 "Facebook Page": (
  "1) 3 posts/week mirrored from LinkedIn (reformat, don't cross-post links only). 2) Boost nothing organically weak. 3) Reply to comments/DMs daily — India/MENA audience is active here.",
  "Reach + profile actions", "1 h"),
 "Instagram": (
  "1) 3 posts/week: carousel tips, credential stories, reels from YouTube Shorts. 2) Stories on exam dates/webinars. 3) Bio link via UTM to the current campaign page. Gulf audience is here — use Arabic captions where relevant.",
  "Reach + link taps", "1.5 h"),
 "X (Twitter)": (
  "1) One insight post/day + replies in industry conversations. 2) Thread each new article into 5-7 tweets ending with the link. 3) Watch #journorequest daily (technique 4).",
  "Impressions + profile clicks", "1 h"),
 "Threads": (
  "1) Mirror the X cadence (1 post/day) — India + US audience. 2) Native text, no link-dumping; link in reply. 3) Engage 5 PM-adjacent threads daily.",
  "Followers + engagement", "30 min"),
 "TikTok": (
  "1) 3 educational shorts/week (reuse YouTube Shorts, native captions). 2) Hook in the first 2 seconds: a mistake, a number, a question. 3) KSA/Gulf hashtags — this is a Gulf reach channel.",
  "Views + follows from KSA/Gulf", "1.5 h"),
 "Pinterest": (
  "1) Pin every infographic/carousel to topic boards monthly (Idea Pins were retired in 2023 — use standard image or video Pins). 2) One checklist Pin per published article. 3) UTM every destination link.",
  "Outbound clicks", "30 min/month"),
 "Bluesky": (
  "1) Watching brief: mirror 2 posts/week. 2) Revisit quarterly — move up only if PM community density appears.",
  "Follower trend (quarterly review)", "15 min"),
 "Website / Blog": (
  "1) Publish per Keyword Plan: one spoke/week, Easy difficulty first, mapped in SEO Clusters. 2) Every post: named author, sources, FAQ block, credential CTA, internal links to pillar. 3) Originals here FIRST; syndicate once indexed (2-10 days) with canonical.",
  "Search Console clicks + positions on target keywords", "4 h"),
 "Medium": (
  "1) Republish site originals once indexed (2-10 days) via the Import tool — canonical set automatically. 2) Submit to 1-2 relevant publications. 3) One native answer-style piece/month targeting an Easy question keyword.",
  "Referral visits + follows", "1 h"),
 "Substack": (
  "1) Only if the newsletter lives natively here — otherwise skip (ESP owns email). 2) If used: fortnightly issue, cross-recommend via beehiiv-style recommendations.",
  "Subscriber growth", "2 h/fortnight"),
 "SlideShare": (
  "1) Upload each deck/one-pager monthly with keyword-rich title + description. 2) Link back to the source page. 3) No bulk dumping.",
  "Views + clickthroughs", "30 min/month"),
 "WordPress.com": (
  "1) Satellite blog: one repurposed article/month with canonical. 2) Never let it outrank the main site — always canonical + link home.",
  "Referral visits", "30 min/month"),
 "Blogger": (
  "1) Same satellite rules as WordPress.com — one repurpose/month, canonical set, link home.",
  "Referral visits", "20 min/month"),
 "Tumblr": (
  "1) Low priority: reblog visual content monthly. 2) Tag by topic; skip if time-pressed.",
  "Referral visits", "15 min/month"),
 "Vocal Media": (
  "1) One career-story piece/quarter (human angle, credential mention). 2) Link to author page.",
  "Reads + referrals", "1 h/quarter"),
 "DEV Community": (
  "1) Only for Certuvo engineering content (if tooling ships). 2) One technical post/quarter max.",
  "Followers + referrals", "1 h/quarter"),
 "Hashnode": (
  "1) Mirror of DEV Community play — Certuvo technical content only, quarterly.",
  "Referrals", "1 h/quarter"),
 "Quora": (
  "1) Answer 3 high-traffic questions/week from saved topic feeds (project controls, planning, EVM). 2) Genuine answer first, one relevant link max. 3) Log answers in Community & PR with URL.",
  "Answer views + upvotes; referral visits", "1.5 h"),
 "Reddit": (
  "1) Contribute helpfully in the subreddits named on Platform Setup — r/projectmanagement, r/construction, r/civilengineering, r/PMP, r/artificial — value first, weeks before any link. 2) Never paste identical answers. 3) AMA once credibility exists. Reddit is the #1 AI-cited domain — presence here feeds AI answers.",
  "Karma trend + non-penalised mentions", "1.5 h"),
 "Planning Planet": (
  "1) 3 forum answers/week — this is EXACTLY our audience. 2) Build named-expert reputation; signature link once established. 3) Note active senior practitioners as honorary-outreach candidates.",
  "Answers + honorary leads sourced", "1.5 h"),
 "PMI Community (ProjectManagement.com)": (
  "1) 2 discussion contributions/week. 2) Respect house rules — no direct promotion; expertise only. 3) Watch for content-contribution openings (they take articles).",
  "Contributions + profile views", "1 h"),
 "Facebook Groups": (
  "1) 3 useful answers/week across the 10 joined groups (Gulf/India planner groups). 2) Value first; link only when asked or clearly helpful. 3) Log in Community & PR.",
  "Answers + leads sourced", "1 h"),
 "Discord": (
  "1) Be present in 2-3 PM/engineering servers; answer when pinged-relevant. 2) No drive-by promo — servers ban fast.",
  "Helpful answers logged", "45 min"),
 "Slack Communities": (
  "1) Same as Discord: 2-3 communities, answer genuinely, DM only when invited.",
  "Answers + DMs earned", "45 min"),
 "WhatsApp Channel": (
  "1) One value broadcast/week max (exam tip, deadline reminder, new resource). 2) Grow the channel via site + email footers. 3) Log sends in DAILY ENTRY ('WhatsApp / Telegram / SMS sent').",
  "Subscribers + view rate", "30 min"),
 "AACE Communities": (
  "1) Respectful peer-body presence: answer technically, never conquest inside their house. 2) Note engaged members for individual outreach OUTSIDE the community.",
  "Reputation (no complaints) + leads noted", "45 min"),
 "Telegram Channel": (
  "1) Mirror WhatsApp Channel content (India/MENA audience). 2) One value post/week; pin the current campaign. 3) Log sends in DAILY ENTRY.",
  "Subscribers + views per post", "20 min"),
 "Project Management Stack Exchange": (
  "1) One thorough answer/week on scheduling/EVM questions. 2) Evergreen answers rank in Google for years — pick questions matching Keyword Plan terms.",
  "Answer score + cumulative views", "45 min"),
 "GitHub": (
  "1) Only if Certuvo open-sources tooling: keep README + examples current, answer issues weekly.",
  "Stars + issue response time", "1 h (if active)"),
 "Credly / Digital Badges": (
  "1) Issue a badge with every credential same-day. 2) Prompt every earner to share to LinkedIn (template in Message Bank). 3) Track share rate monthly.",
  "Badge share rate (target 40%+)", "30 min"),
 "Google Business Profile": (
  "1) Weekly: one post (update/event). 2) Answer every review + question within 48h. 3) Keep NAP identical to site footer.",
  "Profile actions + review score", "30 min"),
 "Trustpilot": (
  "1) Invite every certified candidate to review (email in the completion flow). 2) Reply to every review within 48h — negatives first. 3) Never gate or incentivise.",
  "Review count + rating trend", "30 min"),
 "Course Aggregators (findcourses etc.)": (
  "1) Keep listings current (dates, prices, UTM links). 2) Compare cost-per-lead across directories quarterly in Channel Costs; cut the losers.",
  "Leads per directory per month", "1 h/month"),
 "CPD Directory Listings": (
  "1) Keep the accreditation listing current. 2) Cite 'CPD-accredited' on every course page + campaign. 3) Renew on time.",
  "Referrals from directory", "30 min/month"),
 "Bing Places": (
  "1) Quarterly: verify listing matches GBP exactly.",
  "Listing live + consistent", "10 min/quarter"),
 "Crunchbase": (
  "1) Quarterly: refresh both profiles (funding, headcount, news links).",
  "Profile completeness", "20 min/quarter"),
 "G2": (
  "1) Certuvo: ask every active user cohort for a review each quarter. 2) Respond to all reviews. 3) Keep the profile's screenshots current.",
  "Review count + category rank", "1 h/month"),
 "Capterra": (
  "1) Certuvo: mirror the G2 play — quarterly review asks, respond to all.",
  "Reviews + referrals", "30 min/month"),
 "Clutch": (
  "1) Certuvo: maintain profile; request 2 client reviews/quarter.",
  "Reviews", "30 min/quarter"),
 "Wikipedia / Wikidata": (
  "1) NEVER self-edit articles. 2) Keep the Wikidata item accurate (identifiers, sameAs, ISNI) — that IS allowed. 3) Build notability via press until an independent editor writes the article.",
  "Wikidata item completeness", "30 min/quarter"),
 "Product Hunt": (
  "1) One-off Certuvo launch: prepare 3 weeks (teaser page, hunter, launch-day comment plan). 2) Launch Tuesday-Thursday; all-hands replies on the day.",
  "Upvotes + signups on launch day", "one-off"),
 "Zoom Webinars": (
  "1) One webinar/month: topic from Keyword Plan questions; registration page with UTM. 2) Reminder sequence via ESP + WhatsApp. 3) Recording to YouTube within 48h; leads into LinkedIn Outreach.",
  "Registrations, attendance rate, leads", "4 h/month"),
 "Eventbrite": (
  "1) List every webinar here for discovery reach. 2) Use Eventbrite's reminder emails. 3) One row per event in Content Calendar.",
  "Registrations sourced from listing", "30 min/event"),
 "Luma": (
  "1) Mirror the Eventbrite listing play; Luma's calendar-subscribe builds a repeat audience.",
  "Calendar subscribers", "20 min/event"),
 "Meetup": (
  "1) List events only if a recurring local/online group exists; otherwise skip.",
  "RSVPs", "20 min/event"),
 "StreamYard": (
  "1) Use for every live: brand the overlay, multistream LinkedIn Live + YouTube simultaneously. 2) Save ISO recordings for clipping.",
  "Concurrent viewers across streams", "included in event time"),
 "Podcast Guesting": (
  "1) 2 bookings/month via PodMatch + direct pitches (PR & Target Directory list). 2) Prep 3 stories + 1 offer (free resource) per show. 3) Log each appearance in Community & PR with episode URL.",
  "Appearances + referral visits", "3 h/month"),
 "Spotify for Creators": (
  "1) If/when PCI runs its own show: fortnightly episode, guest-led. 2) Log each episode here once; Apple is directory admin only.",
  "Listens + follows", "4 h/fortnight"),
 "Apple Podcasts": (
  "1) Directory upkeep only: artwork, categories, description quarterly.",
  "Directory listing healthy", "10 min/quarter"),
 "YouTube Podcasts": (
  "1) Connect the RSS once the show exists; verify episodes appear.",
  "YouTube podcast plays", "10 min/month"),
 "Partnership / PR Outreach": (
  "1) 5 association + 3 university + 5 employer approaches/week (Partnership Pipeline rows). 2) Lead with what THEY get (member discount, guest content, data). 3) Move stages honestly; managers join at Meeting stage.",
  "Meetings booked + agreements signed", "3 h"),
 "Journalist Requests (Qwoted / Featured)": (
  "1) Check daily; answer only on-expertise requests within 2h. 2) 80-120 words, data-led, quotable. 3) Log placements in Community & PR.",
  "Placements + backlinks earned", "45 min/day scan"),
 "Press Release Distribution": (
  "1) Real news only: newsroom post first (NewsArticle schema), then PRLog (free), then openPR (1/30 days). 2) Facts, quotes, numbers — no keyword stuffing (links are nofollow). 3) Search the headline after a week; log indexed pickups.",
  "Indexed pickups on non-PCI domains", "1 h/release"),
 "LinkedIn Ads": (
  "1) Only: event promotion + retargeting engaged followers. 2) $20/day test budgets; kill anything above target CPL in 2 weeks. 3) Log spend in Channel Costs weekly.",
  "Cost per lead vs organic benchmark", "1 h"),
 "Google Ads": (
  "1) Brand-term protection + 2-3 high-intent exact keywords from Keyword Plan (certification cost, courses in Dubai). 2) Landing pages must match query intent. 3) Weekly search-term negatives.",
  "CPL + brand impression share", "1.5 h"),
 "Meta Ads": (
  "1) Retargeting site visitors + Certuvo B2C tests only. 2) Creative from top organic posts. 3) Weekly spend log in Channel Costs.",
  "CPL / ROAS", "1 h"),
 "Microsoft Ads": (
  "1) Import the Google Ads brand + high-intent campaigns monthly. 2) Cheap B2B desktop overflow — cap budgets low.",
  "CPL vs Google", "30 min/month"),
 "Email Marketing (ESP)": (
  "1) Newsletter every fortnight; automated nurture sequences always-on (playbook technique 9). 2) Every lead magnet feeds the welcome automation. 3) Log every campaign in DAILY ENTRY ('Email campaign sent', How many = delivered). 4) Prune non-openers quarterly.",
  "Open >35%, click >2.5%, list growth", "3 h"),
 "WhatsApp Business API / SMS": (
  "1) Reminders only: exam dates, webinar starts, application deadlines — consented contacts. 2) Log sends in DAILY ENTRY. 3) Review opt-outs monthly (<1%).",
  "Delivery + opt-out rate", "45 min"),
 "Affiliate / Referral Programme": (
  "1) Every certified alumnus gets a referral code at completion. 2) Monthly leaderboard email to active referrers. 3) Pay out on schedule, publicly thank top referrers.",
  "Referred applications/month", "1 h/month"),
 "Bing Webmaster Tools + IndexNow": (
  "1) Weekly: check indexation of new pages; submit any missing URL. 2) IndexNow fires on publish — verify monthly. ChatGPT retrieves from Bing's index; unindexed = invisible there.",
  "Pages indexed in Bing vs published", "20 min"),
 "AI Answer Engines (ChatGPT / Perplexity / AI Overviews)": (
  "1) Monthly: run the 20-prompt citation audit (who do engines cite for our Keyword Plan queries?). 2) Target the exact cited pages/domains with our content + entity work. 3) Log movement in SEO Clusters notes.",
  "PCI citations in the monthly audit", "2 h/month"),
 "PM World Journal": (
  "1) One named-author practitioner paper per quarter (pitch the editor first). 2) Cite PCI frameworks with their Zenodo DOIs. 3) Share each published paper across LinkedIn + email.",
  "Papers published + citations", "4 h/quarter"),
 "Project Controls Expo (UK / USA / AUS)": (
  "1) Apply to speak at each 2026 edition NOW (deadlines pass). 2) Enter the Awards. 3) If attending: book 10 meetings ahead via LinkedIn Outreach; log all in Partnership Pipeline.",
  "Speaking slots + meetings booked", "seasonal"),
 "Source of Sources (SOS)": (
  "1) Scan daily; answer 3 on-expertise requests/week. 2) Same-day, 80-120 words, credential in the bio line.",
  "Placements", "45 min"),
 "Help a B2B Writer": (
  "1) Same-day answers to B2B writer requests matching our expertise; 2/week target.",
  "Placements", "30 min"),
 "ResponseSource (UK)": (
  "1) Paid — answer UK trade/national requests within 2h. 2) Only keep if it lands 1+ placement/quarter (review in Channel Costs).",
  "UK placements/quarter", "45 min"),
 "Project Times": (
  "1) One contributed article/quarter (evergreen career/methodology angle). 2) Author bio links to the PCI author page.",
  "Published articles + referrals", "3 h/quarter"),
 "eLearning Industry": (
  "1) One L&D-angle article/quarter via the author profile. 2) No HR content — L&D/education angle only.",
  "Published articles + referrals", "3 h/quarter"),
 "Training Industry": (
  "1) One bylined piece/quarter; track the Top Training Companies list criteria annually and apply when eligible.",
  "Articles + list inclusion", "3 h/quarter"),
 "Coursecheck": (
  "1) Capture reviews at the end of every cohort (link in completion email). 2) Respond to each; showcase the widget on course pages.",
  "Review volume + score", "30 min/month"),
 "Project Control Summit": (
  "1) Submit a talk each cycle. 2) If speaking: repurpose the talk into article + clips within 2 weeks.",
  "Accepted talks", "seasonal"),
 "Google Publisher Center (News / Discover)": (
  "1) Keep the newsroom cadence (2+ dated NewsArticle posts/month). 2) Check Discover/News traffic monthly in Search Console.",
  "News/Discover impressions", "20 min/month"),
 "The Digital Project Manager": (
  "1) Pitch one expert contribution/quarter; target their certification roundup pages for PCL-AI inclusion.",
  "Placement + roundup inclusion", "2 h/quarter"),
 "PBC Today (UK construction)": (
  "1) One 700-word UK opinion piece/quarter (skills gap, AI in controls).",
  "Published pieces", "2 h/quarter"),
 "Construction Week ME + CBNME": (
  "1) Maintain the 10-name Gulf journalist list; offer expert comment monthly. 2) Pitch one feature/quarter (Gulf giga-projects + controls skills).",
  "Gulf press mentions", "1 h/month"),
 "Amazon KDP (authority book)": (
  "1) Ship the niche handbook once; update annually. 2) Every author bio + talk cites it. 3) Run free-promo days around campaigns.",
  "Copies + reviews", "project, then 1 h/quarter"),
 "ResearchGate / SSRN / Academia.edu": (
  "1) Upload each PCI framework/paper under named authors with institute affiliation. 2) Answer questions on your papers monthly.",
  "Reads + citations", "1 h/month"),
 "BrightTALK": (
  "1) Run one quarter's webinars here in parallel with Zoom; compare audience quality, then decide.",
  "Registrations vs Zoom baseline", "1 h/event"),
 "Sessionize + SpeakerHub": (
  "1) Keep 2 leader profiles current. 2) 3 CFP applications/quarter from the open boards.",
  "CFP acceptances", "1 h/quarter"),
 "PodMatch + MatchMaker.fm": (
  "1) Weekly: accept/decline matches within 48h; pitch 3 shows/week until 2 bookings/month is steady.",
  "Bookings/month", "45 min"),
 "Skool": (
  "1) Free exam-prep group: one discussion prompt/week + weekly office-hours thread. 2) Promote inside content CTAs. 3) Graduates funnel to Certuvo.",
  "Members + weekly active %", "2 h"),
 "Digg (2026 relaunch)": (
  "1) Watching brief only — re-verify status at the 6-monthly review before ANY effort.",
  "n/a (watch)", "0"),
 "Apple Business Connect": (
  "1) Quarterly: verify the listing (only critical if physical exam centres exist).",
  "Listing accurate", "10 min/quarter"),
 "beehiiv Recommendations": (
  "1) Recommend 3-5 adjacent newsletters; request reciprocal recommendations quarterly.",
  "Subscribers from recommendations", "30 min/quarter"),
 "Eng-Tips Forums": (
  "1) Occasional expert answers (2/month) on controls-adjacent engineering threads.",
  "Answers + profile views", "30 min/month"),
 "Scribd + Issuu": (
  "1) Upload each public report/brochure once (nofollow links; visibility only). 2) Never bulk-submit.",
  "Document views", "15 min/month"),
 "Flipboard / Surf": (
  "1) RSS connected — verify it flows monthly; revisit value at the 6-monthly review.",
  "Referral visits", "10 min/month"),
 "GPT Store (custom GPT)": (
  "1) Keep the exam-coach GPT's knowledge current each quarter. 2) Measure clicks to Certuvo before investing more.",
  "Sessions + clicks out", "1 h/quarter"),
 "Snapchat": (
  "1) KSA campaigns: run ads (lead-gen to Arabic/English landing pages) around enrolment windows. 2) Organic: reuse vertical video 2/week during campaigns. 3) Compare CPA to LinkedIn in Channel Costs.",
  "KSA cost-per-lead vs LinkedIn", "1 h (campaign periods)"),
 "Careers Page + Google for Jobs": (
  "1) One page per open role with JobPosting schema (validate on publish). 2) Remove the page the day a role closes. 3) Mirror every role in the Job Postings tab.",
  "Roles indexed in Google's job box", "30 min/role"),
 "Naukri.com (employer)": (
  "1) Keep the institute page current. 2) Post India-relevant roles (₹400-1,650); refresh monthly. 3) Watch applicant quality vs Naukrigulf in the Job Postings tab.",
  "Applicants per role", "30 min/month"),
 "Jobberman (employer)": (
  "1) Free profile current; post only if a West Africa role opens. Do not spend.",
  "Profile live", "10 min/quarter"),
 "Zenodo (DOI for frameworks)": (
  "1) Every framework/whitepaper gets a DOI on publish (named authors, abstract, PCI community). 2) Cite the DOI in every article that references the framework.",
  "DOIs issued + citations", "30 min/publish"),
 "OER Commons + MERLOT": (
  "1) One CC-licensed exam-prep primer/quarter via Open Author (MERLOT: CC BY-NC-SA). 2) Link back to the credential page as source.",
  "Resources approved + views", "2 h/quarter"),
 "TrainingZone + HRZone (UK L&D)": (
  "1) One practical article/quarter pitched to editor@hrzone.com against their themes. 2) Keep the contributor directory entry current.",
  "Published articles + referrals", "2 h/quarter"),
 "Credential Engine Registry": (
  "1) Publish all three credentials in CTDL; update on any change (fees, requirements). 2) Verify the credentialfinder.org listing quarterly.",
  "Listings live + accurate", "30 min/quarter"),
 "Google Knowledge Panel & Entity Graph": (
  "1) Keep Organization schema + sameAs list complete on the site. 2) Claim the panel the day it appears. 3) Monthly: search the brand, screenshot, log changes.",
  "Panel present + accurate", "30 min/month"),
 "Education Schema Markup (Course List + Credential)": (
  "1) EducationalOccupationalCredential markup on each credential page; Course List where eligible. 2) Validate after every site change; watch Search Console enhancements.",
  "Rich results valid + impressions", "30 min/month"),
 "CareerOneStop Certification Finder": (
  "1) Email listing updates to info@careeronestop.org when anything changes. 2) Verify the listing quarterly.",
  "Listing live + accurate", "15 min/quarter"),
 "D&B D-U-N-S Number": (
  "1) One-time per entity; verify the record annually.",
  "Numbers issued + accurate", "annual check"),
 "Tracxn": (
  "1) Keep both profiles current (quarterly). Perplexity cites Tracxn — accuracy matters.",
  "Profiles current", "20 min/quarter"),
 "AlternativeTo": (
  "1) Keep the Certuvo listing current; ask genuine users to mark 'like' — never fake it.",
  "Likes + alternatives-page rank", "15 min/month"),
 "Udemy (funnel course)": (
  "1) Keep the intro course reviews healthy (respond to all). 2) Update content each quarter; in-course CTAs only where the Promotions Policy allows.",
  "Enrolments + funnel-through to Certuvo", "1 h/month"),
 "Dealroom": (
  "1) Quarterly profile refresh.",
  "Profile current", "10 min/quarter"),
 "Magnitt": (
  "1) Quarterly profile refresh — the MENA dataset governments cite.",
  "Profile current", "15 min/quarter"),
 "F6S": (
  "1) Quarterly refresh; scan grants/accelerator deadlines while there.",
  "Profile current + opportunities noted", "20 min/quarter"),
 "SaaSHub": (
  "1) Keep the Certuvo listing current quarterly.",
  "Listing current", "10 min/quarter"),
 "AI Tool Directories (TAAFT, Toolify, FutureTools)": (
  "1) Only while Certuvo ships genuine AI features: keep TAAFT current; others second wave. 2) Never pay the $497-class listings.",
  "Referral signups", "20 min/quarter"),
 "ISNI": (
  "1) One-time registration; add the ISNI to Wikidata + Organization schema. Verify annually.",
  "Identifier propagated", "annual check"),
 "PitchBook (passive)": (
  "1) No self-serve — keep funding/incorporation news publicly findable so analysts build the profile. Check annually.",
  "Profile exists + accurate", "annual check"),
 "Bayt.com (employer profile)": (
  "1) Keep the employer profile current. 2) Post Gulf roles (~$158); seed 'PCL-AI preferred' in partner job posts. 3) Track applicants in Job Postings tab.",
  "Applicants + profile followers", "30 min/month"),
 "OpenCorporates (verify-only)": (
  "1) Verified once — re-check after any legal-entity change.",
  "Record matches canonical NAP", "on change"),
 "LEI (GLEIF)": (
  "1) Renew annually if held; no weekly work.",
  "LEI active", "annual"),
 "StartupBlink": (
  "1) Listing set once; verify annually.",
  "Listing live", "annual"),
 "Startup Ranking": (
  "1) Free-tier listing; verify after the ~80-day queue, then annually.",
  "Listing live", "annual"),
 "Wellfound (AngelList)": (
  "1) Post remote roles free when hiring (built-in ATS). 2) Keep the company profile warm while roles are open.",
  "Applicants per role", "30 min/role"),
 "Glassdoor / Indeed Employer Pages": (
  "1) Pages claimed + branded; respond to any review within a week. 2) Post via Indeed dashboard directly (feed posts lost organic visibility Mar 2026).",
  "Page rating + applicants", "30 min/month"),
 "Naukrigulf (employer)": (
  "1) Use the 5 free posts for Gulf roles; refresh monthly. 2) Compare applicant quality vs Bayt.",
  "Applicants per role", "30 min/month"),
 "GulfTalent (employer)": (
  "1) Only for senior Gulf hires: request a quote per role; otherwise dormant.",
  "Senior applicants", "on demand"),
 "LinkedIn Learning Instructor": (
  "1) One founder/SME application — long shot; revisit yearly.",
  "Application status", "annual"),
 "Google Scholar (named authors)": (
  "1) Once staff publish: keep profiles + affiliations current quarterly.",
  "Citations", "15 min/quarter"),
 "Google Search Console": (
  "1) Weekly: review new queries, positions on Keyword Plan targets, coverage errors. 2) Numbers into SEO Clusters; one insight into Weekly Review.",
  "Clicks + average position trend", "1 h"),
 "Google Analytics": (
  "1) Weekly: traffic by source/medium (UTM discipline pays here), conversions, top pages. 2) Flag anomalies in Weekly Review.",
  "Conversions by channel", "45 min"),
 "Microsoft Clarity": (
  "1) Fortnightly: watch 5 session recordings on key landing pages; log one UX fix.",
  "Rage-clicks + fixes shipped", "30 min/fortnight"),
}
