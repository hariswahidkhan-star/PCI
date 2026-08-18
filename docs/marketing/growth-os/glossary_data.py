"""Glossary — every term the workbook uses without stopping to explain it.

An independent readability audit rated three sheets 4-5/10 for a non-native
English reader and found 73 unexplained jargon terms on PLATFORM GUIDE alone.
The terms are correct and worth keeping; what was missing was one place that
says what they mean. Grouped so a reader can find a term by where they met it.
"""

GLOSSARY = [
 # ---- how the workbook itself talks
 ("Workbook terms", "Objective", "The campaign a piece of work belongs to — WHY you did it. Set on every logged row."),
 ("Workbook terms", "For (brand)", "Which PCI property the work serves — the institute, one certification, PCI World or Certuvo. Set on every logged row."),
 ("Workbook terms", "Value rank", "1 is the most valuable. Ranks say where to spend the next hour when time runs out."),
 ("Workbook terms", "Person-day", "One person working one day. Four people for two days is eight person-days — this is what targets are measured against."),
 ("Workbook terms", "Cadence", "How often something is published: daily, three times a week, weekly, fortnightly, monthly."),
 ("Workbook terms", "Coverage", "Posts actually published in a window, divided by the posts that schedule planned."),
 ("Workbook terms", "Pillar", "One of the seven big subjects. Keyword Plan, Article Bank and SEO Clusters all use the same seven."),
 ("Workbook terms", "Spoke", "A supporting article that links up to its pillar page."),
 ("Workbook terms", "Data health", "The Dashboard block that counts logging mistakes. Every number there should read zero."),

 # ---- outreach and sales
 ("Outreach", "ICP", "Ideal Customer Profile — how closely a lead matches the people PCI most wants to reach. Scored 1-5."),
 ("Outreach", "Lead score", "A number built from ICP fit and intent signals. It sorts the list; it does not decide anything on its own."),
 ("Outreach", "Intent signal", "Evidence the person is already thinking about this — a recent post, a job change, a course enquiry."),
 ("Outreach", "InMail", "A LinkedIn message you can send to someone you are not connected to. Premium and Sales Navigator seats get a small monthly allowance."),
 ("Outreach", "Connection note", "The short message attached to a LinkedIn connection request. LinkedIn allows 300 characters; shorter converts better."),
 ("Outreach", "Acceptance rate", "Connection requests accepted, divided by connection requests sent."),
 ("Outreach", "Reply rate", "Positive replies, divided by messages sent."),
 ("Outreach", "Funnel stage", "Where a contact currently sits, from first awareness through to certified. Computed, never typed."),
 ("Outreach", "TOFU / MOFU / BOFU", "Top, middle and bottom of funnel. TOFU is someone learning the subject; BOFU is someone ready to buy."),
 ("Outreach", "Handoff", "The moment a lead is passed to a PCI closer. Log who took it, or the trail ends."),

 # ---- SEO and content
 ("SEO & content", "SEO", "Search Engine Optimisation — the work that makes a page findable in Google."),
 ("SEO & content", "SERP", "Search Engine Results Page — the page of results Google shows for one search."),
 ("SEO & content", "Keyword difficulty", "How hard it would be to reach page one for a search. Judged here from who ranks today, not from a paid tool."),
 ("SEO & content", "On-page SEO", "Everything you control on your own page: title, headings, content, internal links, speed."),
 ("SEO & content", "Off-page SEO", "Everything that happens elsewhere and points back: links, mentions, listings, reviews."),
 ("SEO & content", "Backlink", "A link from someone else's website to yours. The strongest off-page signal there is."),
 ("SEO & content", "Dofollow / nofollow", "A dofollow link passes ranking credit; a nofollow link does not. Both still bring readers."),
 ("SEO & content", "Canonical link", "A tag that tells Google which copy of an article is the original. Always set it when republishing elsewhere."),
 ("SEO & content", "Syndication", "Republishing an article you already own on another platform, with a canonical link back."),
 ("SEO & content", "Anchor text", "The visible words a link is wrapped around. It tells search engines what the target page is about."),
 ("SEO & content", "Orphan page", "A page nothing else on the site links to. Search engines struggle to find it; readers never do."),
 ("SEO & content", "Pillar page", "A long page covering a whole subject, linking out to the spokes that cover each part in depth."),
 ("SEO & content", "Money page", "A page that leads directly to an enrolment or an enquiry."),
 ("SEO & content", "Dwell time", "How long a reader stays before going back. Long dwell time tells the platform the content was worth showing."),
 ("SEO & content", "E-E-A-T", "Experience, Expertise, Authoritativeness, Trust — what Google's raters look for. In practice: write from real practice and say who wrote it."),
 ("SEO & content", "AEO", "Answer Engine Optimisation — being the source an AI assistant quotes, not just a blue link in Google."),
 ("SEO & content", "GEO", "Generative Engine Optimisation — another name for the same work as AEO."),
 ("SEO & content", "AI Overview", "The AI-written answer Google now puts above the ordinary results."),
 ("SEO & content", "IndexNow", "A free service that tells Bing (and so ChatGPT) a page has changed, instead of waiting to be crawled."),
 ("SEO & content", "Search Console", "Google's free tool showing which searches brought people to your site, and at what position."),
 ("SEO & content", "Schema / JSON-LD", "Hidden, structured labels on a page that tell search engines what it is — an organisation, a course, an FAQ."),
 ("SEO & content", "Entity", "How search engines recognise an organisation as a real, distinct thing — through consistent names, profiles and records."),
 ("SEO & content", "Wikidata", "A free public database search engines read to confirm an organisation exists and what it is."),
 ("SEO & content", "robots.txt", "A file at the root of a website saying which automated crawlers may read it."),
 ("SEO & content", "CDN / WAF", "Content Delivery Network / Web Application Firewall — services in front of a website. They can block search crawlers by accident."),
 ("SEO & content", "UTM", "Tags added to a link so analytics can say which post or channel an enquiry came from. Build them on the UTM Builder tab."),
 ("SEO & content", "OER", "Open Educational Resources — freely licensed teaching material. Publishing there puts PCI in academic records."),
 ("SEO & content", "DOI", "Digital Object Identifier — a permanent reference number for an academic paper or dataset."),
 ("SEO & content", "Free wire", "A press-release distribution service that costs nothing. Journalists rarely read them; search engines index them, and that is the point."),

 # ---- email and messaging
 ("Email & messaging", "ESP", "Email Service Provider — the tool that sends campaigns and reports opens and clicks."),
 ("Email & messaging", "SPF, DKIM, DMARC", "Three settings on your domain that prove an email really came from you. Without them, campaigns land in spam."),
 ("Email & messaging", "Double opt-in", "Asking a new subscriber to confirm by clicking a link in a first email. Required in practice for EU contacts."),
 ("Email & messaging", "Broadcast", "A one-off message to a whole list or channel, as opposed to an automated sequence."),
 ("Email & messaging", "Deliverability", "Whether your email reaches the inbox rather than the spam folder."),

 # ---- platforms and publishing
 ("Platforms", "Native scheduler", "A scheduling feature built into the platform itself, so no third-party tool is needed."),
 ("Platforms", "Idea Pin", "A retired Pinterest format (ended 2023). Use standard image or video Pins."),
 ("Platforms", "AMA", "'Ask Me Anything' — a scheduled session where you answer a community's questions live."),
 ("Platforms", "Karma", "Reddit's public score for how useful the community has found your contributions."),
 ("Platforms", "Engagement pod", "A group that agrees to like each other's posts. Platforms detect and penalise them — never use one."),
 ("Platforms", "Reach", "How many people actually saw a post, as opposed to how many follow the account."),
 ("Platforms", "Impressions", "How many times a post was displayed. One person can produce several impressions."),
 ("Platforms", "Engagement rate", "Reactions, comments and shares divided by impressions."),

 # ---- credentials and standards
 ("Credentials & standards", "Honorary certification", "A credential offered in recognition of existing experience, on stated terms. Never describe it as already awarded before it is."),
 ("Credentials & standards", "CPD", "Continuing Professional Development — the ongoing learning a credential holder must record to stay current."),
 ("Credentials & standards", "EVM", "Earned Value Management — measuring progress in money terms so cost and schedule performance can be compared."),
 ("Credentials & standards", "WBS", "Work Breakdown Structure — a project split into progressively smaller, deliverable-shaped pieces."),
 ("Credentials & standards", "EAC", "Estimate At Completion — the forecast of what the whole project will finally cost."),
 ("Credentials & standards", "IFRS", "International Financial Reporting Standards — the accounting rules most non-US projects report under."),
 ("Credentials & standards", "AACE", "The Association for the Advancement of Cost Engineering — publisher of the recommended practices project controls work follows."),
 ("Credentials & standards", "EIA-748", "The standard that sets out what an earned value management system must do."),
]
