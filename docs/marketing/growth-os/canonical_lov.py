"""Canonical list-of-values data for the Growth OS V7 build.

Produced by the platform-audit pass. Single source for the build script:
PLATFORMS drives Lists!Platform, Platform Setup, Platform Progress, Who Did What.
"""

# (canonical name, area, priority, new_in_v7, note-for-lists / dedup rule)
PLATFORMS = [
    ("LinkedIn Company Page",       "LinkedIn",         "Critical", False, "One row per PAGE post. A reshare from a personal profile is logged on LinkedIn Personal Profile, not here."),
    ("LinkedIn Personal Profile",   "LinkedIn",         "Critical", False, "All outreach (connect / message / follow-up) logs here. There is no plain 'LinkedIn'."),
    ("LinkedIn Sales Navigator",    "LinkedIn",         "Critical", False, "Lead research only."),
    ("LinkedIn Articles",           "LinkedIn",         "Critical", False, "Only items made in the Write-article editor. Newsletter issues go to LinkedIn Newsletter."),
    ("LinkedIn Newsletter",         "LinkedIn",         "High",     False, "Newsletter issues win over LinkedIn Articles."),
    ("LinkedIn Groups",             "LinkedIn",         "High",     False, "One row per group discussion started or answered — not per like."),
    ("LinkedIn Live",               "LinkedIn",         "High",     True,  "Native live video; repurpose to YouTube after."),
    ("YouTube",                     "Social Media",     "Critical", False, "Shorts are YouTube + Content Type = Short Video. Structure exam-prep playlists as YouTube Courses for the EDU shelf. Vimeo retired."),
    ("Facebook Page",               "Social Media",     "High",     False, "One row per page post. Boosting a post is paid media, logged separately."),
    ("Instagram",                   "Social Media",     "High",     False, "One row per feed post or Reel. A day's Stories are one row, not one per frame."),
    ("X (Twitter)",                 "Social Media",     "Medium",   False, "One row per post or thread — a thread is ONE row, not one per tweet."),
    ("Threads",                     "Social Media",     "Medium",   False, "One row per post. Replies to other people are engagement, logged as 'Engagement (commented on someone else)'."),
    ("TikTok",                      "Social Media",     "Medium",   False, "One row per video published."),
    ("Pinterest",                   "Social Media",     "Low",      False, "One row per batch session, not per Pin — record how many Pins in 'How many'."),
    ("Bluesky",                     "Social Media",     "Low",      True,  "Watching brief; thin project-controls density today."),
    ("Website / Blog",              "Publishing",       "Critical", False, "Originals publish here. Each syndicated copy = own row, Content Status Repurposed."),
    ("Medium",                      "Publishing",       "High",     False, "Syndication surface — set the canonical link."),
    ("Substack",                    "Publishing",       "Medium",   False, "Only issues natively published on Substack."),
    ("SlideShare",                  "Publishing",       "Medium",   False, "One row per deck uploaded."),
    ("WordPress.com",               "Publishing",       "Medium",   False, "One row per article published or syndicated."),
    ("Blogger",                     "Publishing",       "Low",      False, "One row per article published or syndicated."),
    ("Tumblr",                      "Publishing",       "Low",      False, "One row per post published."),
    ("Vocal Media",                 "Publishing",       "Low",      False, "One row per story published."),
    ("DEV Community",               "Publishing",       "Low",      False, "One row per article. Comments on other people's posts are engagement."),
    ("Hashnode",                    "Publishing",       "Low",      False, "One row per article published."),
    ("Quora",                       "Community",        "High",     False, "One Quora value. Space vs answer is the Activity type, not the platform."),
    ("Reddit",                      "Community",        "High",     False, "One row per thread started or substantive answer given — never per upvote or one-line comment."),
    ("Planning Planet",             "Community",        "High",     True,  "The project controls forum (incl. Guild of Project Controls). Exactly our audience."),
    ("PMI Community (ProjectManagement.com)", "Community", "High",  True,  "Largest adjacent professional audience."),
    ("Facebook Groups",             "Community",        "Medium",   False, "One row per discussion started or substantive answer given."),
    ("Discord",                     "Community",        "Medium",   False, "One row per session of helping, not per message. Record minutes honestly."),
    ("Slack Communities",           "Community",        "Medium",   False, "One row per session of helping, not per message."),
    ("WhatsApp Channel",            "Community",        "Medium",   False, "Broadcast channel. The 1:1 reminder tool is WhatsApp Business API / SMS."),
    ("AACE Communities",            "Community",        "Medium",   True,  "Cost-engineering overlap; engage respectfully — peer body."),
    ("Telegram Channel",            "Community",        "Low",      False, "One row per broadcast sent — put the subscriber count in 'How many'."),
    ("Project Management Stack Exchange", "Community",  "Low",      True,  "Small but evergreen Q&A footprint."),
    ("GitHub",                      "Community",        "Low",      True,  "Only if Certuvo open-sources tooling."),
    ("Credly / Digital Badges",     "Directory/Review", "High",     True,  "Badge shares on LinkedIn are organic reach — core channel for a credentialing body. Issue as Open Badges 3.0 (verify the issuer is on the 1EdTech certified list)."),
    ("Google Business Profile",     "Directory/Review", "Medium",   False, "One row per post, or one row per review-answering session."),
    ("Trustpilot",                  "Directory/Review", "Medium",   False, "One row per review-answering session — never one per review invitation sent."),
    ("Course Aggregators (findcourses etc.)", "Directory/Review", "Medium", True, "Course directories send qualified lead-gen traffic."),
    ("CPD Directory Listings",      "Directory/Review", "Medium",   True,  "Legitimacy checkpoints for this exact buyer."),
    ("Bing Places",                 "Directory/Review", "Low",      False, "Setup and maintenance work only. There is nothing to publish here weekly."),
    ("Crunchbase",                  "Directory/Review", "Low",      False, "Profile setup and update work only — one row per update session."),
    ("G2",                          "Directory/Review", "Low",      False, "Certuvo brand only — PCI has no genuine category here."),
    ("Capterra",                    "Directory/Review", "Low",      False, "Certuvo brand only."),
    ("Clutch",                      "Directory/Review", "Low",      False, "Certuvo brand only."),
    ("Wikipedia / Wikidata",        "Directory/Review", "Low",      True,  "Never self-edit; notability-dependent."),
    ("Product Hunt",                "Directory/Review", "Low",      True,  "One-off Certuvo launch venue."),
    ("Zoom Webinars",               "Events",           "Critical", True,  "The event lives here; Eventbrite/Luma/Meetup rows are listings only."),
    ("Eventbrite",                  "Events",           "Medium",   False, "Listing/distribution row for an event delivered on Zoom Webinars."),
    ("Luma",                        "Events",           "Medium",   False, "Listing/distribution row."),
    ("Meetup",                      "Events",           "Low",      False, "Listing/distribution row."),
    ("StreamYard",                  "Events",           "Low",      True,  "Multistream studio (LinkedIn Live + YouTube at once)."),
    ("Podcast Guesting",            "Podcast",          "High",     True,  "Guest appearances on others' shows. Own show = Spotify for Creators."),
    ("Spotify for Creators",        "Podcast",          "Medium",   False, "Log each episode once, here. Apple = directory admin only."),
    ("Apple Podcasts",              "Podcast",          "Low",      False, "Directory setup/maintenance work only — never episode rows."),
    ("YouTube Podcasts",            "Podcast",          "Low",      True,  "Free RSS syndication once the show exists."),
    ("Partnership / PR Outreach",   "Partnership/PR",   "Critical", True,  "All partnership work. Who it targets = Org Type column, never the Platform."),
    ("Journalist Requests (Qwoted / Featured)", "Partnership/PR", "Medium", True, "Expert-source services; authority backlinks + press mentions."),
    ("Press Release Distribution",  "Partnership/PR",   "Medium",   True,
     "Credential-launch news only. Verified free stack (Aug 2026): PRLog (unlimited, fast Google indexing) + openPR (1 per 30 days, Google News). Value = indexed third-party references AI engines read — journalists do not browse free wires; links are nofollow by Google policy. Own newsroom + NewsArticle schema first, wires second."),
    ("LinkedIn Ads",                "Paid Media",       "High",     True,  "Event promotion + retargeting engaged followers."),
    ("Google Ads",                  "Paid Media",       "High",     True,  "High-intent certification queries + brand-term protection."),
    ("Meta Ads",                    "Paid Media",       "Medium",   True,  "Retargeting + early-career B2C (Certuvo)."),
    ("Microsoft Ads",               "Paid Media",       "Low",      True,  "Cheap B2B desktop-search overflow."),
    ("Email Marketing (ESP)",       "Email & CRM",      "Critical", True,  "The owned list. ESP sends always log here, whatever the newsletter surface."),
    ("WhatsApp Business API / SMS", "Email & CRM",      "Medium",   True,  "Exam, webinar and application reminders (Gulf-heavy audience)."),
    ("Affiliate / Referral Programme", "Email & CRM",   "Medium",   True,  "Alumni referrals; influencer codes for Certuvo."),
    ("Bing Webmaster Tools + IndexNow", "Analytics", "Critical", True,
     "ChatGPT retrieves from Bing's index — unindexed on Bing means invisible in ChatGPT answers. Verify domain, submit sitemap, enable IndexNow."),
    ("AI Answer Engines (ChatGPT / Perplexity / AI Overviews)", "SEO & Syndication", "Critical", True,
     "Run the monthly 20-prompt citation audit; log which sources the engines cite and target those exact pages."),
    ("PM World Journal", "Publishing", "Critical", True,
     "The project-management journal that accepts practitioner papers; pitch a named-author series via pmworldjournal.com/authors."),
    ("Project Controls Expo (UK / USA / AUS)", "Events", "Critical", True,
     "The niche's flagship events (London Nov, DC Oct, Melbourne Nov 2026) + Awards. Apply to speak, enter the Awards."),
    ("Source of Sources (SOS)", "Partnership/PR", "High", True,
     "Free highest-volume journalist-request service post-HARO. 3 on-expertise answers weekly."),
    ("Help a B2B Writer", "Partnership/PR", "High", True,
     "Free, B2B-only writer requests; respond same day."),
    ("ResponseSource (UK)", "Partnership/PR", "High", True,
     "UK journalist enquiry service (national + trade press); paid; 2-hour response rule."),
    ("Project Times", "Publishing", "High", True,
     "Contributed PM articles via projecttimes.com/contribute."),
    ("eLearning Industry", "Publishing", "High", True,
     "High-authority education portal, guest articles + author profile via /post-here."),
    ("Training Industry", "Publishing", "High", True,
     "Corporate L&D readership; self-service article submission; Top Training Companies lists."),
    ("Coursecheck", "Directory/Review", "High", True,
     "UK training-specific verified reviews; more credible to course buyers than generic review sites."),
    ("Project Control Summit", "Events", "High", True,
     "Dedicated summit for planners/cost controllers/risk. Apply to present."),
    ("Google Publisher Center (News / Discover)", "SEO & Syndication", "Medium", True,
     "News inclusion is algorithmic now; consistent cadence + NewsArticle schema earns Top Stories/Discover eligibility."),
    ("The Digital Project Manager", "Publishing", "Medium", True,
     "Large PM media brand; expert contributions + podcast; target their certification roundups."),
    ("PBC Today (UK construction)", "Publishing", "Medium", True,
     "UK construction publication taking stakeholder opinion pieces."),
    ("Construction Week ME + CBNME", "Partnership/PR", "Medium", True,
     "Gulf construction trade press; build the 10-name journalist list, offer expert commentary."),
    ("Amazon KDP (authority book)", "Publishing", "Medium", True,
     "A niche handbook is a compounding authority asset; Amazon is its own search engine."),
    ("ResearchGate / SSRN / Academia.edu", "Publishing", "Medium", True,
     "Preprints of PCI frameworks under named authors; the direct channel to the university audience."),
    ("BrightTALK", "Events", "Medium", True,
     "B2B webinar network with built-in audience discovery; test one quarter against Zoom-only baseline."),
    ("Sessionize + SpeakerHub", "Events", "Medium", True,
     "Speaker directories + open CFP boards; 3 applications per quarter."),
    ("PodMatch + MatchMaker.fm", "Podcast", "Medium", True,
     "Host-guest matching tools that operationalise Podcast Guesting; 2 bookings/month."),
    ("Skool", "Community", "Medium", True,
     "Community platform WITH discovery marketplace (unlike Discord/Slack); free exam-prep group."),
    ("Digg (2026 relaunch)", "Community", "Low", True,
     "Open beta closed Mar 2026 pending rebuild - watching brief only; re-verify before spending any effort."),
    ("Apple Business Connect", "Directory/Review", "Medium", True,
     "Feeds Apple Maps/Siri and AI entity verification; free; critical only if physical exam centres exist."),
    ("beehiiv Recommendations", "Email & CRM", "Medium", True,
     "Cross-newsletter recommendations marketplace as a subscriber-acquisition surface."),
    ("Eng-Tips Forums", "Community", "Low", True,
     "Alive in 2026; engineering-heavy rather than controls; occasional answers only."),
    ("Scribd + Issuu", "SEO & Syndication", "Low", True,
     "Still Google-indexed for reports/brochures; nofollow links; visibility only, never bulk submission."),
    ("Flipboard / Surf", "Social Media", "Low", True,
     "Federated 'social websites' launched Apr 2026; connect the blog RSS, revisit in 6 months."),
    ("GPT Store (custom GPT)", "SEO & Syndication", "Low", True,
     "Exam-coach GPT linking to Certuvo; brand experiment, measure clicks before investing."),
    ("Credential Engine Registry", "Authority & Listings", "Critical", True,
     "THE US credential-transparency registry (free) — machine-readable credential data consumed by state systems and AI pipelines; publishing auto-creates the credentialfinder.org listing."),
    ("Google Knowledge Panel & Entity Graph", "Authority & Listings", "Critical", True,
     "No application exists: entity home + Organization schema with sameAs + Wikidata + matched profiles until the panel appears, then claim it. The scoreboard for all authority work."),
    ("Education Schema Markup (Course List + Credential)", "SEO & Syndication", "Critical", True,
     "Course List rich results are live; the old Course Info format was retired 2025 — do not chase it. EducationalOccupationalCredential on each credential page."),
    ("CareerOneStop Certification Finder", "Authority & Listings", "High", True,
     "US Dept of Labor-sponsored certification directory that syndicates via public API — email additions to info@careeronestop.org."),
    ("D&B D-U-N-S Number", "Authority & Listings", "High", True,
     "Free (about 30 business days). Required by app stores and enterprise procurement; one more machine-readable proof the entity is real. Get for PCI and Certuvo."),
    ("Tracxn", "Authority & Listings", "High", True,
     "Best data broker for AI citation — Perplexity observed citing it directly. Free analyst-reviewed listing at tracxn.com/listyourstartup; list both entities."),
    ("AlternativeTo", "Directory/Review", "High", True,
     "Certuvo. Free dofollow listing; the alternatives pages are exactly what AI engines pull for tool-recommendation queries."),
    ("Udemy (funnel course)", "Publishing", "High", True,
     "Certuvo: a low-priced intro course as top-of-funnel. Respect the Promotions Policy — external links only in allowed places."),
    ("Dealroom", "Authority & Listings", "Medium", True,
     "Reputable startup data broker; free self-serve at dealroom.co/for-builders."),
    ("Magnitt", "Authority & Listings", "Medium", True,
     "The MENA startup dataset used by Gulf governments and investors — create/claim the company profile. Core to the Gulf entry story."),
    ("F6S", "Authority & Listings", "Medium", True,
     "Free profile with real ecosystem utility (grants, accelerators). Treat links as nofollow; the value is the indexed profile."),
    ("SaaSHub", "Directory/Review", "Medium", True,
     "Certuvo. Free listing includes a dofollow link; alternatives pages get AI-cited."),
    ("AI Tool Directories (TAAFT, Toolify, FutureTools)", "Directory/Review", "Medium", True,
     "Certuvo, and ONLY if it ships genuine AI features — listing a non-AI product invites removal. TAAFT first, the others as a second wave. Skip $497-class paid listings."),
    ("ISNI", "Authority & Listings", "Medium", True,
     "ISO name identifier, ~$5 one-time via Bowker. Add the ISNI to Wikidata and the Organization schema identifier field."),
    ("PitchBook (passive)", "Authority & Listings", "Medium", True,
     "No self-serve listing — analysts build profiles from visible filings and news. Wired directly into Perplexity and ChatGPT apps; make funding/incorporation news findable."),
    ("Bayt.com (employer profile)", "Directory/Review", "Medium", True,
     "Largest Arab-world job platform; free employer profile. Seed 'PCL-AI preferred' language in partner job posts."),
    ("OpenCorporates (verify-only)", "Authority & Listings", "Low", True,
     "Delaware corps are auto-ingested from the state registry — verify the record exists and the name matches the canonical NAP exactly. Ten minutes, once."),
    ("LEI (GLEIF)", "Authority & Listings", "Low", True,
     "About €60-70/yr. Machine-readable legal-entity proof; no evidence consumer AI engines check it yet — cheap hygiene, not a growth lever."),
    ("StartupBlink", "Authority & Listings", "Low", True,
     "Free map listing inside a government-cited dataset. Entity corroboration; expect zero traffic."),
    ("Startup Ranking", "Authority & Listings", "Low", True,
     "Free tier only (about 80-day queue). Do not pay to fast-track."),
    ("Wellfound (AngelList)", "Authority & Listings", "Low", True,
     "Certuvo, and only when actually hiring — a jobless profile is pointless."),
    ("Glassdoor / Indeed Employer Pages", "Authority & Listings", "Low", True,
     "One-time setup for entity corroboration; no ongoing investment."),
    ("Naukrigulf (employer)", "Directory/Review", "Low", True,
     "The India-to-Gulf planner/scheduler pipeline is exactly the Gulf candidate base. Light presence."),
    ("GulfTalent (employer)", "Directory/Review", "Low", True,
     "Third Gulf board — only after Bayt shows demand."),
    ("LinkedIn Learning Instructor", "Publishing", "Low", True,
     "Selective application (founder/SME as individual). Long shot, zero cost, category-level authority if accepted."),
    ("Google Scholar (named authors)", "Publishing", "Low", True,
     "Personal profiles only — useful once PCI staff publish papers with the Institute as affiliation. Useless before then."),
    ("Snapchat", "Social Media", "Medium", True,
     "KSA's biggest reach channel: 25M Saudi users (74% of internet users, >85% of 15-34s). Ads-first — organic is secondary; use for KSA brand + lead-gen campaigns at ~1/3 LinkedIn's CPA."),
    ("Careers Page + Google for Jobs", "SEO & Syndication", "High", True,
     "Free and fully live 2026: one JobPosting JSON-LD per job page on the careers page surfaces roles in Google's job box. The default posting channel — do this before paying any board."),
    ("Naukri.com (employer)", "Directory/Review", "Medium", True,
     "India's #1 job platform (parent pool of Naukrigulf). Free plan; posts from ~Rs 400-1,650. Employer/institute page feeds the India-to-Gulf planner pipeline."),
    ("Jobberman (employer)", "Directory/Review", "Low", True,
     "Nigeria + Ghana's largest job site. Free employer presence only — Nigeria's oil & gas planning community is a genuine certification pool. Do not spend."),
    ("Zenodo (DOI for frameworks)", "Authority & Listings", "Medium", True,
     "CERN's free repository: every PCI framework/whitepaper gets a citable DOI under named authors; create the PCI community. Org accounts fine; never use it as a bulk DOI mill (policy). OSF is the moderated alternative — no AI-generated content."),
    ("OER Commons + MERLOT", "Publishing", "Medium", True,
     "Free open-educational-resource libraries; non-university organisations can contribute (free membership). Publish CC-licensed exam-prep primers and glossaries; the academic/faculty audience finds them. MERLOT requires CC BY-NC-SA."),
    ("TrainingZone + HRZone (UK L&D)", "Publishing", "Medium", True,
     "UK L&D/HR community sites that welcome contributor articles (editor@hrzone.com; 2026 'practical insights' category). Byline + link; editorial can edit freely. Different audience from eLearning Industry — do not cross-post the same piece."),
    ("Google Search Console",       "Analytics",        "Critical", False, "Weekly analytics review = one row, primary tool; name others in Notes."),
    ("Google Analytics",            "Analytics",        "Critical", False, "One row per analysis session, logged as 'Analytics review'. Never log a page view."),
    ("Microsoft Clarity",           "Analytics",        "High",     False, "One row per analysis session, logged as 'Analytics review'."),
]

# old string (anywhere in the workbook) -> canonical platform string
RENAMES = {
    "LinkedIn":                 "LinkedIn Personal Profile",
    "YouTube Shorts":           "YouTube",
    "Quora Spaces":             "Quora",
    "Quora / Quora Spaces":     "Quora",
    "Vimeo":                    "YouTube",
    "Email / Newsletter Tool":  "Email Marketing (ESP)",
    "Professional Association": "Partnership / PR Outreach",
    "University":               "Partnership / PR Outreach",
    "Employer / Enterprise":    "Partnership / PR Outreach",
    "Podcast":                  "Podcast Guesting",
    "Industry Media":           "Partnership / PR Outreach",
    "Conference":               "Partnership / PR Outreach",
    "Community / Group":        "Partnership / PR Outreach",
}

AREAS = ["LinkedIn", "Social Media", "Publishing", "Community", "Directory/Review",
         "Events", "Podcast", "Partnership/PR", "Paid Media", "Email & CRM",
         "SEO & Syndication", "Authority & Listings", "Analytics"]

# Daily-entry activity types — now including the direct channels (email,
# WhatsApp/Telegram/SMS) and job posting, so "how many" is loggable for them.
# The first seven strings are load-bearing: Dashboard §4 and Team Scorecard
# SUMIFS key on them verbatim — never rename without sweeping consumers.
ACTIVITY_TYPES = [
    "Lead researched (Sales Navigator)",
    "Connection request sent",
    "First message sent",
    "Follow-up message sent",
    "Post / content published",
    "Community answer or comment",
    "Partnership / PR contact",
    "Email campaign sent",
    "WhatsApp / Telegram / SMS sent",
    "Job post published",
    "Link building / outreach (off-page)",
    "Engagement (commented on someone else)",
    "Account created / profile completed",
    "Analytics review",
    "Other (explain in notes)",
]

# Other LOV corrections from the audit
OUTCOME = ["Awaiting Reply", "Interested", "Info Requested", "Meeting Booked",
           "Application Started", "Converted", "Referred to colleague", "Declined",
           "Not Relevant", "No Response", "Do Not Contact / Unsubscribed"]

FUNNEL_STAGE = ["1 Awareness", "2 Engaged", "3 Qualified", "4 Contacted", "5 Interested",
                "6 Meeting / Application", "7 Certification / Enterprise opportunity",
                "8 Won / Certified", "9 Closed - Lost"]

RESULT = ["Done", "In progress", "Waiting on someone", "Blocked", "No reply yet"]
# outcome-flavoured values move to the Outcome list; Result is task-state only

LEAD_SEGMENT = ["Project Controls Manager", "Planning / Scheduling", "Cost Control / Estimating",
                "Quantity Surveying / Commercial", "Risk Management", "PMO Leader",
                "Programme Director", "Engineering Manager", "Construction Director",
                "Academic / Faculty", "L&D / HR", "Consultant", "Recruiter / Staffing",
                "Data / AI Specialist", "Student / Early Career", "Software / Tech", "Other"]

CONTENT_TYPE = ["Text Post", "Image Post", "Carousel", "Short Video", "Long Video", "Article",
                "Newsletter", "Poll", "Infographic", "Case Study", "Credential Story",
                "Guide / Lead Magnet (PDF)", "Podcast Episode", "Live Session / Webinar",
                "Practice Questions / Quiz", "Webinar Promo", "Event Recap", "Press Release"]

ORG_TYPE = ["Professional Association", "University", "Employer / Enterprise",
            "Training Provider", "Podcast", "Industry Media", "Conference",
            "Community / Group", "Influencer / Expert", "Software Vendor",
            "Government / Public Body", "Recruitment / Staffing Agency",
            "Certification Body (peer)"]

FREQUENCY = ["Daily", "3x Weekly", "Weekly", "Fortnightly", "Monthly", "Quarterly", "One-off"]

SCORE_1_5 = ["1 - poor", "2 - below average", "3 - average", "4 - good", "5 - excellent"]


# Brand / property — WHO the effort is for: the institute itself, one specific
# certification, PCI World, or Certuvo. Every logged row carries one, so
# performance is testable per property as well as per campaign.
BRANDS = [
    "PCI AI - Institute (umbrella)",
    "PCL-AI certification",
    "PFL-AI certification",
    "PML-AI certification",
    "PCI World",
    "Certuvo (exam prep)",
    "All / shared",
]

# The web estate — every UTM link, bio link and press release lands on one of
# these. (Property, brand it serves, what it is for — editable on START HERE.)
DOMAINS = [
    ("projectcontrolsinstitute.org", "PCI AI - Institute (umbrella)",
     "Main institute website - certifications, honorary programme, credential verification"),
    ("pciai.org", "PCI AI - Institute (umbrella)",
     "Short institute domain + email domain (admin@pciai.org applications)"),
    ("pciglobal.ai", "PCI AI - Institute (umbrella)",
     "Global / AI-brand domain"),
    ("pciworld.org", "PCI World",
     "PCI World community - rooms, forum, careers"),
    ("mypci.org", "PCI AI - Institute (umbrella)",
     "Candidate portal (My PCI)"),
]

# Where each platform is strongest geographically (researched Aug 2026;
# platforms not listed here are "Global"). Shown on Platform Setup so the
# team picks channels country-by-country.
GEO_DEFAULT = "Global"
GEO = {
    "LinkedIn Company Page": "Global — the professional default",
    "LinkedIn Personal Profile": "Global — the professional default",
    "LinkedIn Sales Navigator": "Global — the professional default",
    "TikTok": "KSA + Gulf strongest; global",
    "Snapchat": "KSA (+ UAE) — 74% of Saudi internet users",
    "Telegram Channel": "India, Russia, MENA",
    "WhatsApp Channel": "India + Gulf; near-universal outside the US",
    "WhatsApp Business API / SMS": "India + Gulf; near-universal outside the US",
    "Facebook Page": "India, SE Asia, MENA — weak for UK/US professionals",
    "Facebook Groups": "India, SE Asia, MENA — weak for UK/US professionals",
    "Instagram": "Gulf-strong global (KSA 77% penetration)",
    "X (Twitter)": "US, Japan, KSA (top Arab market)",
    "Threads": "India, Brazil, US",
    "Bluesky": "US + UK niche",
    "Reddit": "US, UK, anglosphere",
    "Quora": "US + India",
    "Bayt.com (employer profile)": "Pan-MENA: KSA, UAE, Egypt, Jordan",
    "Naukrigulf (employer)": "India-to-Gulf pipeline",
    "GulfTalent (employer)": "Gulf white-collar: UAE, KSA, Qatar",
    "Naukri.com (employer)": "India",
    "Jobberman (employer)": "Nigeria + Ghana",
    "Planning Planet": "Global (UK-run; heavy Gulf / oil & gas)",
    "Eng-Tips Forums": "US-lean global",
    "Course Aggregators (findcourses etc.)": "UK + global",
    "Coursecheck": "UK",
    "CPD Directory Listings": "UK",
    "PBC Today (UK construction)": "UK",
    "Construction Week ME + CBNME": "Gulf",
    "ResponseSource (UK)": "UK",
    "TrainingZone + HRZone (UK L&D)": "UK",
    "Magnitt": "MENA",
    "Project Controls Expo (UK / USA / AUS)": "UK, US, Australia",
    "Bing Places": "US + UK",
    "Apple Business Connect": "US-lean global",
    "Credential Engine Registry": "US",
    "CareerOneStop Certification Finder": "US",
    "openPR": "EU-strong global",
}

# Platforms whose account/effort belongs to the Certuvo brand by design
CERTUVO_PLATFORMS = {
    "G2", "Capterra", "Clutch", "Product Hunt", "AlternativeTo", "SaaSHub",
    "AI Tool Directories (TAAFT, Toolify, FutureTools)", "Udemy (funnel course)",
    "Wellfound (AngelList)", "GitHub", "GPT Store (custom GPT)",
}

# Value ranking of the effort categories — what an hour of effort is worth,
# so the team spends time where it pays. 1 = most valuable. Direct revenue
# first, then the strategic wedge, then multipliers/compounders, then support.
# Management judgment, editable in the yellow cells on Objective Performance.
OBJECTIVE_RANKS = {
    "Certification Sales - PCL-AI": (1, "Revenue driver - flagship credential"),
    "Certification Sales - PML-AI": (2, "Revenue driver - premium tier"),
    "Certification Sales - PFL-AI": (3, "Revenue driver - entry funnel"),
    "Honorary Certification Outreach": (4, "Strategic pipeline - fellows legitimise, refer, open doors"),
    "Partnerships & PR": (5, "Multiplier - one deal moves whole cohorts"),
    "Authority & Entity Building": (6, "Compounding - lifts every other conversion"),
    "Content & SEO Growth": (7, "Compounding - owned traffic that keeps paying"),
    "Events & Webinars": (8, "Pipeline builder - concentrated lead capture"),
    "Certuvo (Exam Prep)": (9, "Adjacent revenue + feeder into certifications"),
    "Community Presence": (10, "Trust builder - slow burn, defends reputation"),
    "General Brand Awareness": (11, "Support - spend spare capacity only"),
}

# Platform value rank 1..N: priority tier first (Critical > High > Medium >
# Low), curated list order within a tier. Deterministic, so the ranking
# updates itself whenever the estate changes.
_TIER = {"Critical": 0, "High": 1, "Medium": 2, "Low": 3}
PLATFORM_VALUE_RANKS = {
    PLATFORMS[i][0]: pos
    for pos, i in enumerate(
        sorted(range(len(PLATFORMS)), key=lambda i: (_TIER[PLATFORMS[i][2]], i)), 1)
}

# Effort objectives — the campaign dimension. Every logged row carries one, so
# performance is testable per category, not only per platform.
OBJECTIVES = [
    "Honorary Certification Outreach",
    "Certification Sales - PCL-AI",
    "Certification Sales - PFL-AI",
    "Certification Sales - PML-AI",
    "Authority & Entity Building",
    "Content & SEO Growth",
    "Community Presence",
    "Partnerships & PR",
    "Events & Webinars",
    "Certuvo (Exam Prep)",
    "General Brand Awareness",
]
