#!/usr/bin/env python3
"""Audit the trailing internal-linking NOTES, not the bodies.

link_audit.py checks what is published. This checks what the note tells a publisher to add,
which is a different and equally real exposure: a note proposing three links to one domain
becomes three links to one domain the moment someone follows it. The bodies can be perfectly
clean while the instructions attached to them are not.
"""
import json, re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "pciglobal.ai",
           "pciworld.org", "credentialfinder.org"]
FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
# The trailing publisher note goes by several labels. It was headed "Links:" in a handful of
# flagship files, which this pattern did not match, so their instruction sheets were read as
# body links and the files were reported as over-linked when they were not.
NOTE_RE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Internal linking note|Linking note|Links?\s*[:(.,])", re.I | re.M)
URL_RE = re.compile(r"https?://[^\s)\"'`<>]+")

try:
    VALID = set(json.load(open(ROOT / "_tools/valid_urls.json")))
except Exception:
    VALID = set()


def host_of(text):
    m = FM_RE.match(text)
    if not m:
        return None
    p = re.search(r"^platform:\s*(.+)$", m.group(1), re.M)
    if not p:
        return None
    h = re.match(r"Own site\s*[—\-–]\s*([a-z0-9.]+)", p.group(1).strip(), re.I)
    return h.group(1).lower() if h else None


def domain_of(url):
    u = re.sub(r"^https?://", "", url).split("/")[0].lower()
    u = u[4:] if u.startswith("www.") else u
    return next((d for d in DOMAINS if u == d or u.endswith("." + d)), None)


def audit(path):
    text = path.read_text(encoding="utf-8")
    n = NOTE_RE.search(text)
    if not n:
        return None
    note, host = text[n.start():], host_of(text)

    # A note holds two different kinds of link, and counting them together is wrong.
    # Some are to be PLACED ("this should link to A, to B and to C") — those are the piece's
    # links and the caps apply. Others are CONDITIONAL alternatives for a reply or a comment
    # ("if a reply asks about cut-off, answer with X"), where exactly one is ever used and
    # often none is. Bluesky's note names three hub pages and puts one link in the post.
    # Only the first kind is counted.
    COND = re.compile(r"\b(if (a |an |someone|somebody|anyone|the )?(reply|repl|comment|question|thread|asks?|turns?|wants?)"
                      r"|if asked|should (?:anyone|someone)|in the (?:thread|comments)|per reply|one link per)", re.I)
    per, broken, conditional = {}, [], set()
    # split on sentence boundaries so a conditional clause does not suppress the whole note
    for sent in re.split(r"(?<=[.;])\s+", note):
        cond = bool(COND.search(sent))
        for raw in URL_RE.findall(sent):
            u = raw.rstrip(".,;:)`*")
            d = domain_of(u)
            if not d:
                continue
            if cond:
                conditional.add(u)
                continue
            per.setdefault(d, set()).add(u)
    # broken-link checking still covers conditional targets: a reply link that 404s is a
    # broken link whoever eventually posts it.
    for u in sorted(conditional):
        if VALID:
            stem = u.rstrip("/")
            pp = re.sub(r"^https?://[^/]+", "", stem)
            if pp and stem not in VALID and stem + "/" not in VALID:
                broken.append(u)
        if VALID:
            stem = u.rstrip("/")
            path_part = re.sub(r"^https?://[^/]+", "", stem)
            if path_part and stem not in VALID and stem + "/" not in VALID:
                broken.append(u)

    cross = {d: v for d, v in per.items() if d != host}
    issues = []
    if len(per) == 5:
        issues.append("note proposes links to all five domains")
    for d, v in cross.items():
        if len(v) > 1:
            issues.append(f"note proposes {len(v)} links to {d}")
    if sum(len(v) for v in cross.values()) > 3:
        issues.append(f"note proposes {sum(len(v) for v in cross.values())} cross-estate links (max 3)")
    for b in sorted(set(broken)):
        issues.append(f"note proposes BROKEN {b}")
    for d, v in per.items():
        for u in v:
            if VALID:
                stem = u.rstrip("/")
                pp = re.sub(r"^https?://[^/]+", "", stem)
                if pp and stem not in VALID and stem + "/" not in VALID:
                    issues.append(f"note proposes BROKEN {u}")
    return {"file": str(path.relative_to(ROOT)), "host": host,
            "proposed": {d: sorted(v) for d, v in per.items()},
            "conditional": sorted(conditional), "issues": issues}


def main():
    rows = []
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            r = audit(f)
            if r:
                rows.append(r)
    bad = [r for r in rows if r["issues"]]
    print(f"notes found {len(rows)} | clean {len(rows)-len(bad)} | issues {len(bad)}")
    for r in bad:
        print(f"  {r['file']}: {'; '.join(r['issues'])}")
    (ROOT / "_tools" / "note_audit.json").write_text(json.dumps(rows, indent=1))
    return 1 if bad else 0


if __name__ == "__main__":
    sys.exit(main())
