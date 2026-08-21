#!/usr/bin/env python3
"""Mechanical SEO / AEO / GEO / AIO conformance checks across the content run.

Everything here is countable, so a script does it across all 347 pieces rather than an
LLM doing it approximately across a sample. What is left over — whether a link is
genuinely contextual, whether an answer is actually the answer, whether the prose reads
as written by a practitioner — is what the judges are for.
"""
import json, re, statistics, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
FENCE_RE = re.compile(r"^```.*?^```", re.S | re.M)
# The trailing publisher note goes by several labels. It was headed "Links:" in a handful of
# flagship files, which this pattern did not match, so their instruction sheets were read as
# body links and the files were reported as over-linked when they were not.
# Labels the trailing publisher note goes by. Agents keep coining new ones — "Internal links",
# "**Linking note.**", "Internal links, as placed in the body", and now "Estate links" — and a
# missed label makes that file's instruction sheet read as body links, which is what put files
# on the over-linked list that were never over-linked. A general "any phrase containing link"
# pattern was tried and is worse: it matches ordinary prose ("a party to link", "calendar link")
# while still missing "Internal links now in the body:", where words sit between the label and
# its colon. So the list is explicit, and adding to it is the maintenance cost.
NOTE_RE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Estate[- ]?links?|Internal linking note|Linking note|Links?\s*[:(.,])", re.I | re.M)
BANNED = ["in today's fast-paced", "delve", "unlock the", "game-changer", "game changer",
          "seamless", "tapestry", "testament to", "it's important to note",
          "navigate the complexities", "robust solution", "ever-evolving", "in conclusion",
          "dive deep", "moreover,", "furthermore,"]
# _BRIEF.md bans "leverage" as a VERB, which is the generated-text tell. The noun is ordinary
# English and ordinary finance: "its leverage is in the timing", "a highly leveraged balance
# sheet". Matching the stem flags both, so the verb senses are matched specifically — the
# inflections that can only be verbs, and the bare form when it follows a subject or an
# auxiliary rather than a determiner or possessive.
LEVERAGE = re.compile(r"\bleverag(es|ing)\b"
                      r"|\b(?:to|can|could|will|would|should|must|may|might|helps?|lets?|"
                      r"we|they|you|it|teams?|firms?|and|then)\s+leverage\b", re.I)
LANDSCAPE = re.compile(r"\blandscape\b", re.I)


def fm_get(fm, key):
    m = re.search(rf"^{key}:\s*(.+)$", fm, re.M)
    return m.group(1).strip() if m else ""


def split(text):
    fm, rest = "", text
    m = FM_RE.match(text)
    if m:
        fm, rest = m.group(1), text[m.end():]
    n = NOTE_RE.search(rest)
    note = rest[n.start():] if n else ""
    body = FENCE_RE.sub("", rest[:n.start()] if n else rest)
    return fm, body, note


STOP = {"a", "an", "the", "of", "for", "to", "in", "on", "as", "and", "or", "is", "are",
        "be", "with", "by", "at", "from", "how", "what", "which", "your", "you", "it"}


def kw_present(kw, text):
    """Is the keyword's subject expressed here?

    Exact substring matching is the wrong test. "Choosing between two credentials" and "How to
    choose between two credentials, done properly" are the same subject, and a search engine
    treats them as such — but a checker demanding the literal string reports a miss, and the
    only way a writer clears it is to bend a good headline around the keyword. That trade is
    backwards.

    So: strip stopwords, stem crudely, and require every remaining content word to appear.
    "Read a Body of Knowledge" against "Ninety minutes with a Body of Knowledge" still fails,
    because "read" is genuinely absent — which is the miss worth reporting.
    """
    def toks(x):
        out = set()
        for w in re.findall(r"[a-z0-9]+", x.lower()):
            if w in STOP or len(w) < 3:
                continue
            for suf in ("ing", "ed", "es", "s"):
                if len(w) > 4 and w.endswith(suf):
                    w = w[: -len(suf)]
                    break
            # "choosing" stems to "choos" while "choose" keeps its e, so the same verb in two
            # forms would not match itself. Normalise the trailing e on both sides.
            if len(w) > 3 and w.endswith("e"):
                w = w[:-1]
            out.add(w)
        return out
    want = toks(kw)
    return bool(want) and want <= toks(text)


def paragraphs(body):
    stripped = re.sub(r"^\|.*$", "", body, flags=re.M)          # tables
    stripped = re.sub(r"^\s*[-*>#].*$", "", stripped, flags=re.M)  # lists, quotes, headings
    return [p.strip() for p in re.split(r"\n\s*\n", stripped) if len(p.split()) > 12]


def check(path):
    text = path.read_text(encoding="utf-8")
    fm, body, note = split(text)
    words = body.split()
    typ = fm_get(fm, "type")
    # Only prose meant to rank is held to the article rules. A carousel, a thread, a pitch
    # email or a set of directory blurbs fails every one of them by design, and flagging
    # those buries the misses that matter.
    NOT_ARTICLE = ("carousel", "post", "thread", "caption", "pin", "script", "boilerplate",
                   "pitch", "release", "listing", "record", "abstract", "deck", "intro",
                   "answer", "response", "story", "issue", "outline", "email", "profile")
    # Platform decides this before type does. A Quora answer is typed qa-list or qa-answer,
    # which are legitimate article types for an own-site Q&A page — but on Quora the spec is
    # 300-600 words with no FAQ block, so judging one against the article rules flags correct
    # work and pushes a writer to break the platform spec to satisfy a checker.
    plat_lower = fm_get(fm, "platform").lower()
    NOT_ARTICLE_PLATFORM = ("quora", "reddit", "stack exchange", "x /", "x (", "threads",
                            "bluesky", "telegram", "whatsapp", "discord", "instagram",
                            "pinterest", "tiktok", "youtube", "eng-tips", "planning planet",
                            "linkedin post", "linkedin company", "linkedin personal",
                            "facebook", "slideshare")
    if plat_lower.startswith(NOT_ARTICLE_PLATFORM):
        is_article = False
    else:
        is_article = (typ in {"pillar", "guide", "how-to", "data-study", "glossary", "qa-list",
                              "comparison", "listicle", "explainer", "case-study", "faq", "article"}
                      or (len(words) > 700 and not any(n in typ.lower() for n in NOT_ARTICLE)))

    title, meta = fm_get(fm, "title"), fm_get(fm, "meta")
    kw = fm_get(fm, "primary_kw").rstrip(" *").strip().lower()
    h1s = re.findall(r"^#\s+(.+)$", body, re.M)
    h2s = re.findall(r"^##\s+(.+)$", body, re.M)

    # AEO: the answer must land inside the first 60 words after the H1. Flagship files open
    # with an italic note to whoever posts it and sometimes a horizontal rule; the article
    # proper starts after that, so the lead is taken from the first real prose paragraph.
    after_h1 = body.split("\n", 1)[1] if h1s else body
    lead = ""
    for p in re.split(r"\n\s*\n", after_h1):
        p = p.strip()
        if not p or p.startswith(("#", "|", ">", "-", "---")):
            continue
        # Skip the publisher's standfirst — an italic block telling somebody how to post the
        # piece. The earlier test required it to be a single line, which missed every Medium
        # file, where the instruction wraps and carries a canonical URL. Judged on content
        # instead: an italic block that talks to the publisher is not the article's opening.
        if p.startswith("*") and p.rstrip().endswith("*"):
            continue
        if re.match(r"^[*_]*(Republished|Republish|Paste|Post |Upload|Set the|Canonical|"
                    r"Cross-post|Publish )", p, re.I):
            continue
        lead = " ".join(p.split()[:60])
        break

    paras = paragraphs(body)
    plens = [len(p.split()) for p in paras] or [0]
    long_paras = sum(1 for n in plens if n > 90)

    low = body.lower()
    banned_hits = sorted({b for b in BANNED if b in low})
    if LEVERAGE.search(body):
        banned_hits.append("leverage(verb)")
    if LANDSCAPE.search(body):
        banned_hits.append("landscape")

    flags = []
    if is_article:
        if len(h1s) != 1:
            flags.append(f"H1 count {len(h1s)}")
        if title and not (48 <= len(title) <= 62):
            flags.append(f"title {len(title)}ch (50-60)")
        if meta and not (135 <= len(meta) <= 162):
            flags.append(f"meta {len(meta)}ch (140-158)")
        kw_core = re.sub(r"\s*[—\-–].*$", "", kw).strip()   # "kw — inherited via canonical"
        if kw_core and h1s and not kw_present(kw_core, h1s[0]):
            flags.append("primary_kw not in H1")
        if kw_core and not kw_present(kw_core, lead):
            flags.append("primary_kw not in first 60 words")
        if kw_core and meta and not kw_present(kw_core, meta):
            flags.append("primary_kw not in meta")
        if "|" not in body:
            flags.append("no table (GEO: tables are the most cited format)")
        if not re.search(r"(?im)^#{2,3}\s*.*(question|faq|asked)", body) and not re.search(r"\*\*[A-Z][^*]{10,120}\?\*\*", body):
            flags.append("no FAQ block")
        if long_paras:
            flags.append(f"{long_paras} paragraphs over 90 words (GEO)")
        if len(h2s) < 3:
            flags.append(f"only {len(h2s)} H2s")
        if not fm_get(fm, "schema"):
            flags.append("no schema type declared")
        if len(words) < 700:
            flags.append(f"{len(words)} words")
    if banned_hits:
        flags.append("banned phrasing: " + ", ".join(banned_hits))

    return {"file": str(path.relative_to(ROOT)), "type": typ, "is_article": is_article,
            "words": len(words), "title_len": len(title), "meta_len": len(meta),
            "h1": len(h1s), "h2": len(h2s), "tables": body.count("\n|"),
            "median_para": statistics.median(plens), "max_para": max(plens),
            "primary_kw": kw, "lead": lead, "flags": flags}


def main():
    rows = []
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            rows.append(check(f))
    bad = [r for r in rows if r["flags"]]
    print(f"checked {len(rows)} | clean {len(rows)-len(bad)} | flagged {len(bad)}")
    from collections import Counter
    c = Counter(f.split(":")[0].split(" over")[0].split(" (")[0] for r in bad for f in r["flags"])
    print("\nflag frequency:")
    for k, v in c.most_common():
        print(f"  {v:4d}  {k}")
    (ROOT / "_tools" / "quality_check.json").write_text(json.dumps(rows, indent=1))
    return 0


if __name__ == "__main__":
    sys.exit(main())
