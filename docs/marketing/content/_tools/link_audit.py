#!/usr/bin/env python3
"""Audit link discipline across the content run, host-aware.

Only the *publishable body* is audited. Three regions are excluded because none of them
is a link on a published page: the YAML front matter (cta_link / canonical are operational
metadata for whoever posts it), the trailing internal-linking note (instructions to the
publisher), and fenced code blocks (canonical-tag snippets, JSON-LD).

Host-awareness is the point. On an own-site article, a link to its own domain is one of the
internal links `_LINK_ARCHITECTURE.md` §3 *requires*; only links to the other four domains
are cross-estate and carry footprint risk. Counting the two together was what produced the
first, wrong, reading of this run.
"""
import json, re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "pciglobal.ai",
           "pciworld.org", "credentialfinder.org"]
HUB = "projectcontrolsinstitute.org"

FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
FENCE_RE = re.compile(r"^```.*?^```", re.S | re.M)
# The trailing publisher note goes by several labels, and more of them once the agents rewrote it:
# "Internal links (first comment)...", "**Linking note.**", "Internal links, as placed in the body".
# Missing a variant makes that file's instruction sheet read as body links, and the file is then
# Labels the trailing publisher note goes by. Agents keep coining new ones — "Internal links",
# "**Linking note.**", "Internal links, as placed in the body", and now "Estate links" — and a
# missed label makes that file's instruction sheet read as body links, which is what put files
# on the over-linked list that were never over-linked. A general "any phrase containing link"
# pattern was tried and is worse: it matches ordinary prose ("a party to link", "calendar link")
# while still missing "Internal links now in the body:", where words sit between the label and
# its colon. So the list is explicit, and adding to it is the maintenance cost.
NOTE_RE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Estate[- ]?links?|Internal linking note|Linking note|Links?\s*[:(.,])", re.I | re.M)
MD_LINK = re.compile(r"\[([^\]]*)\]\(\s*(https?://[^\s)]+)\s*\)")
# A trailing *, _ or ` is a markdown delimiter closing a span, not part of the address. Letting
# the path swallow one turned every URL at the end of an italic run into a broken link — 16 of
# them, all correctly written.
BARE_URL = re.compile(r"(?<![\(\]/\w])(?:https?://)?((?:[a-z0-9-]+\.)*(?:%s))(/[^\s,;)\"'<>*_`]*)?"
                      % "|".join(d.replace(".", r"\.") for d in DOMAINS))
WEAK = {"click here", "here", "read more", "this link", "learn more", "this page", "link"}

# Every URL form the estate can legitimately serve: the 100 own-site pages authored in this run,
# every real page on the live hub in both its .html and extensionless form, and the server-rendered
# /certifications/{slug} pages. A link outside this set is broken, and a broken link in a published
# article is worse than no link — it wastes the crawl and reads as carelessness to a human.
try:
    VALID = set(json.load(open(Path(__file__).resolve().parent / "valid_urls.json")))
except Exception:
    VALID = set()


def split(text):
    fm, rest = "", text
    m = FM_RE.match(text)
    if m:
        fm, rest = m.group(1), text[m.end():]
    note = ""
    n = NOTE_RE.search(rest)
    if n:
        note, rest = rest[n.start():], rest[:n.start()]
    return fm, FENCE_RE.sub("", rest), note


def fm_get(fm, key):
    m = re.search(rf"^{key}:\s*(.+)$", fm, re.M)
    return m.group(1).strip() if m else ""


def host_of(fm):
    """The domain the piece is published ON, or None for an off-estate platform."""
    p = fm_get(fm, "platform")
    m = re.match(r"Own site\s*[—\-–]\s*([a-z0-9.]+)", p, re.I)
    return m.group(1).lower() if m else None


def domain_of(url):
    u = re.sub(r"^https?://", "", url).split("/")[0].lower()
    u = u[4:] if u.startswith("www.") else u
    return next((d for d in DOMAINS if u == d or u.endswith("." + d)), None)


def canonical_origin(canon):
    """The domain a republication points home to.

    The canonical field is prose, not a URL — "canonical -> https://credentialfinder.org/x"
    or "canonical -> /x (credentialfinder.org original)". Passing the whole string to
    domain_of returns nothing, so every Medium republication was scored as though it had no
    origin, and the internal links it inherits from its own original were counted as
    cross-estate placements. The piece looked over its cap while being exactly right.
    """
    m = re.search(r"https?://[^\s)]+", canon)
    if m:
        return domain_of(m.group(0))
    for d in DOMAINS:                      # relative form names the domain in prose
        if d in canon:
            return d
    return None


def links_in(body):
    out, spans = [], []
    for m in MD_LINK.finditer(body):
        spans.append((m.start(), m.end()))
        d = domain_of(m.group(2))
        if d:
            out.append((d, re.sub(r"^https?://[^/]+", "", m.group(2)) or "/", m.group(1).strip(), "md"))
    for m in BARE_URL.finditer(body):
        if any(s <= m.start() < e for s, e in spans):
            continue
        d = domain_of(m.group(1))
        if not d:
            continue
        path = m.group(2) or ""
        nl = body.find("\n", m.end())
        line = body[body.rfind("\n", 0, m.start()) + 1: nl if nl != -1 else len(body)].strip()
        if path or line == m.group(0).strip() or m.group(0).startswith("http"):
            out.append((d, path, "", "bare"))
    return out


def audit(path):
    text = path.read_text(encoding="utf-8")
    fm, body, note = split(text)
    host = host_of(fm)
    canon = fm_get(fm, "canonical")
    republished = canon.startswith("canonical")
    ls = links_in(body)

    # A declared exemption, with its reason recorded in the front matter. directory-boilerplate.md
    # is the case that forced it: it is a field-value reference sheet listing which URL belongs in
    # a directory's Website field, its "learn more" field and its verification field. Those are
    # three different forms, never three links on one page, so the per-domain cap does not describe
    # it. Exempt files are reported separately rather than counted clean, so the exemption stays
    # visible and has to keep earning itself.
    exempt = fm_get(fm, "link_audit_exempt")

    internal = [l for l in ls if host and l[0] == host]
    external = [l for l in ls if not (host and l[0] == host)]
    # A canonical republication mirrors its original, so links back to the origin domain are
    # that original's internal links travelling with it, not cross-estate placements.
    origin = canonical_origin(canon) if republished else None
    mirrored = [l for l in external if origin and l[0] == origin]
    cross = [l for l in external if l not in mirrored]

    per_cross = {}
    for d, p, a, k in cross:
        per_cross.setdefault(d, []).append((p, a))

    issues = []
    if len({l[0] for l in ls}) == 5:
        issues.append("links all five domains")
    if len(per_cross) >= 4:
        issues.append(f"{len(per_cross)} cross-estate domains")
    for d, e in per_cross.items():
        if len(e) > 1:
            issues.append(f"{len(e)}x cross-link to {d}" + (" (same page)" if len({p for p, _ in e}) == 1 else ""))
    if len(cross) > 3:
        issues.append(f"{len(cross)} cross-estate links (max 3)")
    if host and len(internal) < 2 and len(body.split()) > 700:
        issues.append(f"only {len(internal)} internal links (need 2-3)")
    if len({p for p, _ in [(l[1], l[2]) for l in mirrored]}) == 1 and len(mirrored) > 2:
        issues.append(f"{len(mirrored)} links to one page on {origin}")
    for d, p, a, k in ls:
        if a and a.lower() in WEAK:
            issues.append(f"weak anchor '{a}'")
    if VALID:
        for d, p, a, k in ls:
            u = f"https://{d}{p}".rstrip("/")
            if not p or p == "/":
                continue                      # the bare origin is the home page, always valid
            if u not in VALID and u + "/" not in VALID:
                issues.append(f"BROKEN LINK {u}")

    if exempt:
        issues = []
    return {
        "file": str(path.relative_to(ROOT)), "words": len(body.split()), "exempt": exempt,
        "platform": fm_get(fm, "platform")[:40], "host": host, "canonical": canon[:60],
        "n_internal": len(internal), "n_mirrored": len(mirrored), "n_cross": len(cross),
        "cross_domains": sorted(per_cross), "all_domains": sorted({l[0] for l in ls}),
        "links": [{"domain": d, "path": p, "anchor": a} for d, p, a, _ in ls],
        "has_note": bool(note), "issues": issues,
    }


def main():
    rows = []
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            rows.append(audit(f))
    bad = [r for r in rows if r["issues"]]
    ex = [r for r in rows if r.get("exempt")]
    print(f"audited {len(rows)} | clean {len(rows)-len(bad)-len(ex)} | issues {len(bad)} | exempt {len(ex)}")
    for r in bad:
        print(f"  {r['file']}: {'; '.join(r['issues'])}")
    for r in ex:
        print(f"  EXEMPT {r['file']}: {r['exempt']}")

    reach = {}
    for r in rows:
        for d in r["all_domains"]:
            reach[d] = reach.get(d, 0) + 1
    print("\ndomain reach (pieces linking to each):")
    for d in DOMAINS:
        print(f"  {reach.get(d,0):4d}  {d}")
    (ROOT / "_tools" / "link_audit.json").write_text(json.dumps(rows, indent=1))
    return 1 if bad else 0


if __name__ == "__main__":
    sys.exit(main())
