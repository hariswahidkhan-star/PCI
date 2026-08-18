"""Grammar helpers for the article-title generator.

The independent content audit found 223 titles broken by naive templating:
"What is APIs...", "How to build a drawdown schedules", "a estimator",
"...on projects on megaprojects", "data centres projects". Titles are the
product; these rules make the templates agreement-aware.
"""
import re

# words that end in -s but are singular
_SINGULAR_S = {
    "analysis", "basis", "status", "process", "loss", "class", "access",
    "progress", "business", "synthesis", "crisis", "bias", "gas", "focus",
    "bonus", "census", "campus", "virus", "apparatus", "consensus", "series",
    "means", "news", "ethics", "economics", "logistics", "mechanics",
}
# last word is a participle/adjective, so the head noun is earlier
_TRAILING_MOD = re.compile(r"(ed|ing|able|ible)$")


def _strip(term):
    """Remove parentheticals so the head noun is visible."""
    return re.sub(r"\s*\([^)]*\)", "", term).strip()


def head_word(term):
    t = _strip(term)
    # cut at the first preposition/conjunction — the head is before it
    t = re.split(r"\s+(?:in|on|for|of|between|with|to|by|and|vs|versus)\s+", t)[0].strip()
    words = t.split()
    if not words:
        return ""
    last = words[-1].lower().strip(",.")
    if len(words) > 1 and _TRAILING_MOD.search(last):
        return words[0].lower().strip(",.")
    return last


def is_plural(term):
    h = head_word(term)
    if not h or not h.endswith("s"):
        return False
    if h in _SINGULAR_S:
        return False
    if h.endswith(("ss", "us", "sis", "ics")):
        return False
    return True


# acronym letters whose name starts with a vowel sound -> "an"
_AN_LETTERS = set("AEFHILMNORSX")
_A_EXCEPTIONS = {"unit", "user", "unique", "european", "one", "united", "useful",
                 "uniform", "universal", "usable"}


def article(phrase):
    """'a' or 'an' for the phrase that follows."""
    w = _strip(phrase).split()
    if not w:
        return "a"
    first = w[0].strip("([\"'")
    if not first:
        return "a"
    # acronym (all caps, short) -> judge by the first letter's spoken name
    core = first.replace("-", "")
    if core.isupper() and len(core) <= 6:
        return "an" if core[0] in _AN_LETTERS else "a"
    low = first.lower()
    if low in _A_EXCEPTIONS:
        return "a"
    return "an" if low[0] in "aeiou" else "a"


def with_article(phrase):
    return f"{article(phrase)} {phrase}"


def strip_project_tail(term):
    """'master data management on projects' -> 'master data management'.

    Stops the doubled preposition in '... on projects on megaprojects'.
    """
    return re.sub(r"\s+(?:on|for|in)\s+projects$", "", term.strip(), flags=re.I)


# sectors that must be singular when used attributively before "projects"
_SECTOR_SINGULAR = {
    "data centres": "data centre", "airports": "airport", "ports": "port",
    "pharmaceutical plants": "pharmaceutical plant", "hospitals": "hospital",
    "stadiums and mega-events": "stadium and mega-event",
    "semiconductor fabs": "semiconductor fab",
    "solar and renewables": "solar and renewable",
}


def sector_attr(industry):
    """Attributive form: 'data centres' -> 'data centre' (before 'projects')."""
    return _SECTOR_SINGULAR.get(industry.lower(), industry)


def cap(term):
    """Capitalise the first character without touching the rest (nPlan safe)."""
    t = term.strip()
    if not t:
        return t
    if t[0].islower() and not t.startswith(("nPlan", "iOS", "eLearning")):
        return t[0].upper() + t[1:]
    return t


# verb selection for process seeds — "How to do work breakdown structure" was
# the single most-reported grammar defect
_METHODY = ("method", "system", "analysis technique", "critical path")
_BUILDY = ("structure", "plan", "register", "schedule", "model", "budget",
           "forecast", "estimate", "baseline", "matrix", "curve", "dictionary",
           "breakdown")
_RUNNY = ("review", "check", "audit", "assessment", "workshop", "meeting",
          "simulation", "analysis", "test", "close")


def how_to(term):
    """A grammatical 'how to ...' opening for a process/doc seed."""
    t = _strip(term).lower()
    if t.endswith("management"):
        stem = term[: term.lower().rfind("management")].strip()
        # only when the stem is a real noun phrase: "project risk management"
        # -> "How to manage project risk"; "trend management" stays "How to do"
        if len(stem.split()) >= 2:
            return f"How to manage {stem}"
        return f"How to do {term}"
    if any(k in t for k in _METHODY):
        return f"How to use the {term}"
    # "run" before "build": a *schedule health check* is run, not built
    if any(t.endswith(k) or f"{k} " in t for k in _RUNNY):
        return f"How to run {term if is_plural(term) else with_article(term)}"
    if any(t.endswith(k) for k in _BUILDY):
        return f"How to build {term if is_plural(term) else with_article(term)}"
    if t.endswith("ing"):
        return f"How to do {term}"          # gerunds are correct with "do"
    return f"How to run {term}" if is_plural(term) else f"How to run {with_article(term)}"
