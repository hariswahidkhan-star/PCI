"""Article Bank generator — rebuilt after an independent content audit.

Fixes applied (audit findings 1-8):
 * agreement-aware titles (plural/singular, a/an, verb choice, no doubled
   prepositions, attributive sectors)
 * keywords derived from the SEED, never from the title; capped and cleaned
 * prompt fragment omitted when there are no supporting keywords
 * families restricted to seeds where the angle is genuine (no "AI and
   performance bonds", no "risk workshops in Excel", no exec one-pagers on P6
   keystrokes)
 * unsourceable and non-compliant families removed (salary-uplift claims for a
   credential with no holders; city-level visa articles; certifications that do
   not exist)
 * Difficulty and Priority separated into their own columns
Row: (title, cluster, fmt, effort, priority, primary_kw, supporting_kw,
      audience, funnel, words, prompt)
"""
import re

from grammar import (article, cap, how_to, is_plural, sector_attr,
                     strip_project_tail, with_article)
from article_bank_data import (CROSS_PAIRS, COMPARISON_ROUNDUPS, CURATED, FAQS,
                               FIN_CERTS, FLAGSHIPS, GEOS, GEO_TOPICS, INDUSTRIES,
                               INDUSTRY_ANGLES, LISTS_TRENDS, PCI_CERTS, PM_CERTS,
                               PROJECTS, ROLES, ROLE_ANGLES, STANDARDS_TOPICS,
                               TERMS, TERM_VS, TOOLS, TOOL_ANGLES, TOOL_PAIRS)
try:
    from article_bank_extra import EXTRA_SEEDS
except ImportError:
    EXTRA_SEEDS = []

WORDS = {"pillar": "2,500-3,500", "comparison": "1,800-2,400", "guide": "1,200-1,800",
         "how-to": "1,200-1,800", "glossary": "800-1,200", "template": "1,000-1,500",
         "data-study": "1,500-2,200", "faq": "500-900", "practice": "1,200-1,800",
         "process-guide": "1,500-2,000", "qa-list": "1,200-1,800"}
FUNNEL = {"pillar": "TOFU", "comparison": "MOFU", "guide": "TOFU", "how-to": "MOFU",
          "glossary": "TOFU", "template": "MOFU", "data-study": "TOFU", "faq": "TOFU",
          "practice": "BOFU", "process-guide": "MOFU", "qa-list": "TOFU"}
# "Effort" — how much work the piece is, NOT SERP difficulty (that lives on the
# Keyword Plan). The audit found these two meanings conflated in one column.
EFFORT = {"pillar": "Heavy", "comparison": "Medium", "guide": "Medium",
          "how-to": "Medium", "glossary": "Light", "template": "Light",
          "data-study": "Heavy", "faq": "Light", "practice": "Medium",
          "process-guide": "Medium", "qa-list": "Light"}
AUDIENCE = {
    "Brand & Authority": "prospective candidates and employers evaluating PCI AI",
    "Comparisons": "professionals choosing between certifications",
    "Finance Certifications": "project professionals weighing finance credentials",
    "Careers & Jobs": "practitioners planning their next career move",
    "EVM & Performance": "planners, cost engineers and PMO analysts",
    "Project Accounting & IFRS": "cost engineers and project accountants",
    "Budgeting & Cash Flow": "cost controllers and project finance teams",
    "Reporting & Dashboards": "controls teams reporting to leadership",
    "AI & The Future": "project professionals adapting to AI",
    "PM Fundamentals": "early-career and cross-training professionals",
    "Scheduling & WBS": "planning engineers and schedulers",
    "Cost & Estimating": "estimators and cost engineers",
    "Risk": "risk analysts and project controls teams",
    "Contracts & Claims": "commercial teams, QSs and claims specialists",
    "PMO & Governance": "PMO leads and governance teams",
    "Agile & Hybrid": "teams running hybrid delivery",
    "Data & BI": "controls professionals building data skills",
    "Procurement": "project procurement and expediting teams",
    "QS & Measurement": "quantity surveyors and commercial teams",
    "Project Finance": "project finance and commercial teams",
    "Turnarounds & Ops": "turnaround planners and maintenance teams",
    "Exam Prep": "certification candidates preparing with Certuvo",
    "Tools & Software": "hands-on users of planning and cost tools",
    "Industries": "practitioners in sector-specific delivery",
    "Case Studies": "practitioners learning from real megaprojects",
    "Keyword Plan targets": "searchers PCI has decided to win — see the Keyword Plan",
    "Cluster spokes": "readers arriving from a pillar page on SEO Clusters",
}

# ---------------------------------------------------------------- curation
# Seeds removed as duplicates of a richer sibling (audit 4a)
DROP_TERMS = {"Near-critical path", "Physical percent complete",
              "P6 percent complete types", "vendor document control",
              "Critical path"}
# The "AI and X" angle is only real where data, forecasting or repetitive
# analysis is involved (audit 6c).
AI_OK = {"EVM & Performance", "Scheduling & WBS", "Cost & Estimating", "Risk",
         "Reporting & Dashboards", "Data & BI", "Budgeting & Cash Flow",
         "Agile & Hybrid", "Procurement"}
# Board-visible topics only — no executive one-pager on P6 F9 settings (6d/5d).
BOARD_OK = {"EVM & Performance", "Budgeting & Cash Flow", "Risk",
            "Reporting & Dashboards", "PMO & Governance",
            "Project Accounting & IFRS", "Project Finance"}
# Tools that actually run a certification programme (audit 6a).
TOOL_CERTS = {"Primavera P6", "Microsoft Project", "Power BI", "Excel", "Python",
              "Deltek Cobra", "Primavera Risk Analysis"}
# Countries only — there is no Dubai work visa or London jurisdiction (audit 4b).
COUNTRIES = {"UAE", "Saudi Arabia", "Qatar", "Kuwait", "Oman", "Bahrain", "UK",
             "USA", "Canada", "Australia", "India", "Singapore", "Malaysia",
             "Nigeria", "South Africa", "Ireland", "Germany", "Netherlands",
             "Norway", "New Zealand", "Egypt"}
COUNTRY_ONLY_TOPICS = ("Work visas", "Interview culture", "PCL-AI certification",
                       "PFL-AI certification", "PML-AI certification",
                       "construction market outlook")
# Titles that would promise data the client cannot cite (audit 2).
BANNED_TITLE_BITS = ("salary uplift",)

STOP = {"the", "a", "an", "and", "or", "of", "for", "to", "in", "on", "with",
        "why", "what", "how", "which", "that", "your", "you", "is", "are",
        "it", "its", "explained", "guide", "complete", "honest", "real",
        "practical", "working", "step", "by"}


def kw_from_seed(seed, extra=""):
    """A search-like keyword from the SEED (never the title). <= 5 words."""
    s = re.sub(r"\s*\([^)]*\)", " ", str(seed)).lower()
    s = re.sub(r"[^a-z0-9\- ]+", " ", s)
    words = [w for w in s.split() if w]
    if extra:
        words += [w for w in re.sub(r"[^a-z0-9\- ]+", " ", extra.lower()).split()]
    out = []
    for w in words:
        if w not in out:
            out.append(w)
    return " ".join(out[:5]).strip()


def _prompt(fmt, title, primary, supporting, audience, words):
    kw = f'Primary keyword "{primary}".'
    if supporting:
        kw += f" Work in naturally where they fit: {supporting}."
    base = (f'Write "{title}" for projectcontrolsinstitute.org (PCI AI - Project '
            f"Controls Institute). Audience: {audience}. Length {words} words, "
            f"British English. {kw} ")
    body = {
     "glossary": ("Structure: a direct one-paragraph definition first, then a worked "
                  "example, why it matters on real projects, related terms, and a "
                  "3-question FAQ block. "),
     "how-to": ("Structure: numbered steps a practitioner can follow today, one worked "
                "example with illustrative numbers clearly labelled as illustrative, "
                "common mistakes, and a short checklist recap. "),
     "comparison": ("Structure: an honest side-by-side table on the axes that actually "
                    "decide this choice, who each option suits, and a scenario-based "
                    "recommendation with its conditions stated. Never disparage "
                    "alternatives - credibility comes from fairness. "),
     "data-study": ("Use only real, cited statistics with linked sources - NEVER invent "
                    "or estimate a number. Lead with the most striking figures you can "
                    "actually cite; if fewer than three exist, lead with the gap in the "
                    "evidence and name who publishes what. "),
     "template": ("Provide the full template structure inline, explain every section "
                  "briefly, and describe how to adapt it. Offer a downloadable version "
                  "as the CTA. "),
     "faq": ("Answer the question directly in the first two sentences, then give the "
             "nuance, one example, and a related-questions block. "),
     "practice": ("Provide exam-style questions with fully worked solutions and the "
                  "reasoning behind each distractor. Pitch at professional "
                  "certification level. "),
     "qa-list": ("Structure: each question as an H2, a model answer of 100-150 words "
                 "under it, and a one-line note on what the interviewer is testing. "),
     "pillar": ("This is a definitive pillar page: cover the whole topic with H2 "
                "sections that each answer one search intent, a table of contents, and "
                "internal-link stubs to spoke articles. "),
     "process-guide": ("Walk the process end to end with roles, inputs, outputs and "
                       "timing; include one realistic scenario and the failure modes. "),
     "guide": ("Structure: an opening that names the reader's problem, clear H2 "
               "sections, one worked example or scenario with any figures labelled as "
               "illustrative, and practical takeaways. "),
    }.get(fmt, "Structure: clear H2 sections and practical takeaways. ")
    tail = ("Cite named sources for every factual claim about the world; figures inside "
            "a worked example are illustrative and must be labelled as such. Never "
            "invent statistics, and never claim a certification outcome (salary, "
            "recognition or employment) that PCI AI cannot evidence. Include an FAQ "
            "block, a meta description under 155 characters, and one CTA to the "
            "relevant PCI AI credential or Certuvo. Write with practitioner "
            "experience (E-E-A-T), not generic filler.")
    return base + body + tail


def _row(title, cluster, fmt, primary=None, supporting="", audience=None,
         priority="P3"):
    title = cap(title.strip())
    primary = (primary or kw_from_seed(title)).strip()
    audience = audience or AUDIENCE.get(cluster, "project controls professionals")
    words = WORDS.get(fmt, "1,200-1,800")
    return (title, cluster, fmt, EFFORT.get(fmt, "Medium"), priority, primary,
            supporting, audience, FUNNEL.get(fmt, "TOFU"), words,
            _prompt(fmt, title, primary, supporting, audience, words))


def _term_rows(t, cat, kind):
    b = kw_from_seed(t)
    plural = is_plural(t)
    rows = []
    q = "What are" if plural else "What is"
    m = "why they matter" if plural else "why it matters"
    rows.append(_row(f"{q} {t}? Definition, examples and {m}", cat, "glossary",
                     b, f"{b} definition, {b} meaning, {b} in project management",
                     priority="P2"))
    if kind == "metric":
        rows.append(_row(f"How to calculate {t}: formula, worked example and Excel steps",
                         cat, "how-to", f"{b} formula",
                         f"how to calculate {b}, {b} example, {b} in excel", priority="P2"))
        rows.append(_row(f"{cap(t)}: a worked example from a real project", cat, "guide",
                         f"{b} example", f"{b} calculation, {b} interpretation"))
        rows.append(_row(f"Interpreting {t}: what the number is really telling you", cat,
                         "guide", f"{b} interpretation", f"{b} meaning, good {b} value"))
    if kind == "process":
        rows.append(_row(f"{how_to(t)}: a step-by-step guide", cat, "how-to",
                         f"how to {b}", f"{b} steps, {b} process, {b} best practices",
                         priority="P2"))
        rows.append(_row(f"{cap(t)} checklist: what good looks like", cat, "template",
                         f"{b} checklist", f"{b} template, {b} review"))
        rows.append(_row(f"{cap(t)} KPIs: measuring whether it is working", cat, "guide",
                         f"{b} kpis", f"{b} metrics, measure {b}"))
    if kind == "doc":
        rows.append(_row(f"{cap(t)} template: structure, example and how to use it",
                         cat, "template", f"{b} template",
                         f"{b} example, {b} format, free {b}", priority="P2"))
        build = t if plural else with_article(t)
        rows.append(_row(f"How to build {build} stakeholders actually use", cat, "how-to",
                         f"how to create {kw_from_seed(t)}", f"{b} best practices, {b} example"))
        rows.append(_row(f"{cap(t)}: an annotated real-world sample", cat, "guide",
                         f"{b} example", f"{b} sample, {b} in practice"))
    if kind == "concept":
        rows.append(_row(f"{cap(strip_project_tail(t))} on construction projects: how it "
                         f"plays out on site", cat, "guide", f"{b} construction",
                         f"{b} on site, {b} in projects"))
    if kind in ("concept", "metric"):
        rows.append(_row(f"Common mistakes with {t} (and how to avoid them)", cat, "guide",
                         f"{b} mistakes", f"{b} problems, {b} misuse, {b} pitfalls"))
    return rows


# The seven SEO Clusters pillars are the one shared vocabulary between the
# Keyword Plan, this bank and the cluster map. Every brief carries one, so a
# writer can move between the three sheets without translating.
PILLARS = ["Project controls fundamentals", "Planning and scheduling",
           "Cost control and estimating", "Earned value management",
           "Risk management", "AI in project controls",
           "Certification and careers"]
CLUSTER_TO_PILLAR = {
 "Brand & Authority": "Certification and careers",
 "Comparisons": "Certification and careers",
 "Finance Certifications": "Certification and careers",
 "Careers & Jobs": "Certification and careers",
 "Exam Prep": "Certification and careers",
 "EVM & Performance": "Earned value management",
 "Project Accounting & IFRS": "Cost control and estimating",
 "Budgeting & Cash Flow": "Cost control and estimating",
 "Cost & Estimating": "Cost control and estimating",
 "QS & Measurement": "Cost control and estimating",
 "Project Finance": "Cost control and estimating",
 "Procurement": "Cost control and estimating",
 "Reporting & Dashboards": "Project controls fundamentals",
 "PMO & Governance": "Project controls fundamentals",
 "PM Fundamentals": "Project controls fundamentals",
 "Industries": "Project controls fundamentals",
 "Case Studies": "Project controls fundamentals",
 "Turnarounds & Ops": "Project controls fundamentals",
 "Contracts & Claims": "Project controls fundamentals",
 "Scheduling & WBS": "Planning and scheduling",
 "Agile & Hybrid": "Planning and scheduling",
 "Tools & Software": "Planning and scheduling",
 "Risk": "Risk management",
 "AI & The Future": "AI in project controls",
 "Data & BI": "AI in project controls",
}


_ROW_PILLAR = {}          # title -> pillar, for rows whose pillar is known exactly


def pillar_for(cluster, title=None):
    if title and title in _ROW_PILLAR:
        return _ROW_PILLAR[title]
    return CLUSTER_TO_PILLAR.get(cluster, "Project controls fundamentals")


# Format to build for an "Asset to build" line on the Keyword Plan.
def _asset_fmt(asset):
    a = (asset or "").lower()
    if "pillar" in a:
        return "pillar"
    if "compar" in a or " vs " in a:
        return "comparison"
    if "calculator" in a or "quiz" in a or "tool" in a:
        return "template"
    if "landing" in a or "homepage" in a or "hub" in a:
        return "guide"
    if "faq" in a or "question" in a:
        return "faq"
    return "guide"


KP_PILLAR = {"Brand": "Certification and careers", "Core": "Certification and careers",
             "Role": "Certification and careers", "Conquest": "Certification and careers",
             "Skills": "Project controls fundamentals", "AI": "AI in project controls",
             "Geo": "Certification and careers", "Comparison": "Certification and careers",
             "Exam prep": "Certification and careers",
             "Honorary": "Certification and careers"}

# One title shape per search intent. Repeating "the complete guide" 76 times
# would have told a writer nothing and told Google less.
_INTENT_TITLE = {
 "Navigational": "{k}: what it is, who it is for, and how to start",
 "Commercial": "{k}: what it covers, what it costs and who it suits",
 "Transactional": "{k}: entry routes, fees and what you actually get",
 "Informational": "{k} explained: a practitioner's guide",
}


def planned_rows():
    """One brief for every keyword on the Keyword Plan, and one for every
    supporting article named on SEO Clusters.

    An independent review found that 70% of the planned keywords — including
    all ten of the P1 attack terms the plan says to start with — had no brief
    anywhere in 5,000 rows, because the plan and the bank were built by two
    processes that never met. Generating the bank FROM the plan makes that
    impossible rather than merely fixed.
    """
    from keywords_data import KEYWORDS
    out = []
    for (kwd, cluster, intent, funnel, vol, diff, who, asset, prio, sampled) in KEYWORDS:
        fmt = _asset_fmt(asset)
        if fmt == "comparison":
            title = f"{cap(kwd)}: an honest comparison"
        elif fmt == "pillar":
            title = f"{cap(kwd)}: everything the search actually asks"
        elif fmt == "faq":
            title = f"{cap(kwd)}: the questions people really ask"
        elif fmt == "template":
            title = f"{cap(kwd)}: a free tool and how to use it"
        else:
            title = _INTENT_TITLE.get(
                intent, "{k} explained: a practitioner's guide").format(k=cap(kwd))
        support = ", ".join(dict.fromkeys(
            [kw_from_seed(kwd, "guide"), kw_from_seed(kwd, "cost"),
             kw_from_seed(kwd, "requirements")]))
        row = _row(title, "Keyword Plan targets", fmt, primary=kwd.lower(),
                   supporting=support, priority=prio,
                   audience=f"people searching \"{kwd}\" — see the Keyword Plan for who "
                            f"ranks today")
        out.append((row, kwd, KP_PILLAR.get(cluster, "Certification and careers")))
    return out


SPOKES = [
 ("Project controls fundamentals",
  "What is project controls and what does a project controls engineer do"),
 ("Planning and scheduling",
  "How to build a realistic project schedule in Primavera P6"),
 ("Cost control and estimating",
  "Cost control methods that catch overruns early"),
 ("Earned value management",
  "Earned value management explained with a worked example"),
 ("Risk management",
  "Quantitative schedule risk analysis for beginners"),
 ("AI in project controls",
  "How AI is changing forecasting in project controls"),
 ("Certification and careers",
  "Project controls certification routes compared"),
]


def spoke_rows():
    """The seven supporting articles the SEO Clusters map already names."""
    return [(_row(t, "Cluster spokes", "guide", primary=kw_from_seed(t),
                  supporting=kw_from_seed(t, "explained"), priority="P1"), pil)
            for pil, t in SPOKES]


def generate(target=5000):
    rows = []
    rows += [r for r, _k, _p in planned_rows()]
    rows += [r for r, _p in spoke_rows()]
    for _r7, _k7, _p7 in planned_rows():
        _ROW_PILLAR[_r7[0]] = _p7
    for _r7, _p7 in spoke_rows():
        _ROW_PILLAR[_r7[0]] = _p7
    for title, cluster, fmt in FLAGSHIPS:
        rows.append(_row(title, cluster, fmt, supporting=kw_from_seed(title),
                         priority="P1"))
    rows += [_row(t, c, f, supporting=kw_from_seed(t), priority="P2")
             for t, c, f in CURATED]
    rows += [_row(t, c, f, supporting=kw_from_seed(t), priority="P2")
             for t, c, f in EXTRA_SEEDS]

    # ---- certification comparisons
    for pci in PCI_CERTS:
        for other in PM_CERTS:
            rows.append(_row(
                f"{pci} vs {other}: which should you take?", "Comparisons", "comparison",
                f"{pci.lower()} vs {other.lower()}",
                f"{other.lower()} alternative, {other.lower()} comparison", priority="P2"))
        for other in FIN_CERTS:
            # never framed as an either/or against a chartered qualification
            rows.append(_row(
                f"{pci} and {other}: how they differ and when each is worth it",
                "Finance Certifications", "comparison",
                f"{pci.lower()} and {other.lower()}",
                f"{other.lower()} for project professionals, {other.lower()} comparison"))
    for a, b in CROSS_PAIRS:
        rows.append(_row(f"{a} vs {b}: an honest comparison for project professionals",
                         "Comparisons", "comparison", f"{a.lower()} vs {b.lower()}",
                         f"{a.lower()} or {b.lower()}, {b.lower()} difference", priority="P2"))
    rows += [_row(t, "Comparisons", "comparison", supporting=kw_from_seed(t),
                  priority="P2") for t in COMPARISON_ROUNDUPS]

    terms = [(t, c, k) for t, c, k in TERMS if t not in DROP_TERMS]
    for t, c, k in terms:
        rows += _term_rows(t, c, k)
    for a, b in TERM_VS:
        rows.append(_row(f"{cap(a)} vs {b}: the difference explained with examples",
                         "EVM & Performance", "glossary", f"{kw_from_seed(a)} vs {kw_from_seed(b)}",
                         f"difference between {a.lower()} and {b.lower()}", priority="P2"))

    # ---- roles
    for role in ROLES:
        for tpl, fmt in ROLE_ANGLES:
            title = tpl.replace("{r}", role).replace("{R}", cap(role))
            title = re.sub(r"\ba (?=[aeiouAEIOU]|EVM|PMO)", "an ", title)
            rows.append(_row(title, "Careers & Jobs", fmt, kw_from_seed(role, "career"),
                             f"{role} jobs, {role} career, {role} requirements"))
        rows.append(_row(f"Your first 90 days as {with_article(role)}: the plan",
                         "Careers & Jobs", "guide", kw_from_seed(role, "first 90 days"),
                         f"{role} onboarding, new {role}"))

    # ---- geography (countries only where the topic is country-level)
    for g in GEOS:
        for tpl, fmt in GEO_TOPICS:
            if any(b in tpl for b in BANNED_TITLE_BITS):
                continue
            if any(tpl.startswith(p) or p in tpl for p in COUNTRY_ONLY_TOPICS) \
                    and g not in COUNTRIES:
                continue
            rows.append(_row(tpl.replace("{g}", g), "Careers & Jobs", fmt,
                             kw_from_seed(f"project controls {g}"),
                             f"project controls {g.lower()}, jobs in {g.lower()}"))
    for role in ROLES:
        for g in ("UAE", "Saudi Arabia", "UK", "USA", "India"):
            rows.append(_row(f"{cap(role)} salary in {g}: what the published sources show",
                             "Careers & Jobs", "data-study",
                             kw_from_seed(f"{role} salary {g}"),
                             f"{role} pay {g.lower()}, {role} jobs {g.lower()}"))
            rows.append(_row(f"{cap(role)} jobs in {g}: who is hiring and what they pay",
                             "Careers & Jobs", "data-study",
                             kw_from_seed(f"{role} jobs {g}"),
                             f"{role} vacancies {g.lower()}"))

    # ---- industries (attributive singular before "projects")
    for ind in INDUSTRIES:
        sec = sector_attr(ind)
        for tpl, fmt in INDUSTRY_ANGLES:
            title = tpl.replace("{i} projects", f"{sec} projects").replace(
                "{i}", ind).replace("{I}", cap(ind))
            rows.append(_row(title, "Industries", fmt, kw_from_seed(f"{sec} projects"),
                             f"{ind} project management, {ind} construction"))

    # ---- tools
    for tool in TOOLS:
        for tpl, fmt in TOOL_ANGLES:
            if "certification worth it" in tpl and tool not in TOOL_CERTS:
                continue
            rows.append(_row(tpl.replace("{t}", tool), "Tools & Software", fmt,
                             kw_from_seed(tool, "training"),
                             f"{tool.lower()} tutorial, learn {tool.lower()}"))
    for a, b in TOOL_PAIRS:
        rows.append(_row(f"{a} vs {b}: which fits your projects?", "Tools & Software",
                         "comparison", f"{kw_from_seed(a)} vs {kw_from_seed(b)}",
                         f"{a.lower()} comparison, {b.lower()} alternative"))

    rows += [_row(t, "PMO & Governance", "guide", supporting=kw_from_seed(t),
                  priority="P2") for t in STANDARDS_TOPICS]
    rows += [_row(t, "Reporting & Dashboards" if "KPI" in t or "report" in t.lower()
                  else "Careers & Jobs" if "book" in t.lower() or "podcast" in t.lower()
                  else "PM Fundamentals",
                  "data-study" if "statistic" in t.lower() or "rate" in t.lower()
                  else "guide", supporting=kw_from_seed(t)) for t in LISTS_TRENDS]
    rows += [_row(q, "PM Fundamentals", "faq", kw_from_seed(q),
                  supporting=kw_from_seed(q, "explained"), priority="P2") for q in FAQS]
    for p in PROJECTS:
        rows.append(_row(f"What {p} teaches us about project controls", "Case Studies",
                         "guide", kw_from_seed(p, "lessons"),
                         "megaproject lessons, project controls case study"))

    # ---- restricted extension families
    for t, c, k in terms:
        if c in AI_OK and k in ("process", "metric", "concept"):
            b = kw_from_seed(t)
            rows.append(_row(f"AI and {t}: what changes in the age of machine assistance",
                             "AI & The Future", "guide", kw_from_seed(f"ai {t}"),
                             f"ai in {b}, machine learning {b}"))
    for t, c, k in terms:
        if k in ("metric", "doc"):
            b = kw_from_seed(t)
            rows.append(_row(f"{cap(t)} in Excel: a practical build", "Data & BI",
                             "how-to", kw_from_seed(t, "excel"),
                             f"{b} spreadsheet, {b} template excel"))
    for t, c, k in terms:
        if k in ("metric", "process"):
            b = kw_from_seed(t)
            rows.append(_row(f"{cap(t)} practice questions with worked solutions",
                             "Exam Prep", "practice", kw_from_seed(t, "questions"),
                             f"{b} exam questions, {b} quiz"))
    for t, c, k in terms:
        if k in ("process", "metric"):
            b = kw_from_seed(t)
            rows.append(_row(f"{cap(t)}: 10 interview questions and strong answers",
                             "Careers & Jobs", "qa-list", kw_from_seed(t, "interview"),
                             f"{b} interview questions, {b} answers"))
    for t, c, k in terms:
        if c in BOARD_OK and k in ("process", "concept"):
            b = kw_from_seed(t)
            rows.append(_row(f"Explaining {t} to executives: the one-slide version", c,
                             "guide", kw_from_seed(t, "executives"),
                             f"explain {b}, {b} for leadership"))
    for t, c, k in terms:
        if k in ("process", "concept"):
            base = strip_project_tail(t)
            b = kw_from_seed(base)
            rows.append(_row(f"{cap(base)} on megaprojects: how scale changes the game",
                             c, "guide", kw_from_seed(base, "megaprojects"),
                             f"{b} large projects, {b} capital projects"))
    for t, c, k in terms:
        if k == "process":
            b = kw_from_seed(t)
            rows.append(_row(f"Standing up {t} on a new project: the first 30 days", c,
                             "process-guide", kw_from_seed(t, "implementation"),
                             f"{b} rollout, {b} setup"))

    seen, out = set(), []
    for r in rows:
        if any(x in r[0].lower() for x in BANNED_TITLE_BITS):
            continue
        key = re.sub(r"[^a-z0-9]+", " ", r[0].lower()).strip()
        if key in seen:
            continue
        seen.add(key)
        out.append(r)
    return out


if __name__ == "__main__":
    from collections import Counter
    rows = generate()
    print(len(rows), "articles")
    print(Counter(r[1] for r in rows).most_common(8))
    print(Counter(r[2] for r in rows))
    print(Counter(r[4] for r in rows))
