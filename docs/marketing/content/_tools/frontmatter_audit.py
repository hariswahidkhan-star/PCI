#!/usr/bin/env python3
"""Validate the URLs that live in front matter — canonical and cta_link.

link_audit.py deliberately ignores front matter, because cta_link and canonical are not links
on a page: they are operational fields for whoever posts the piece. That exclusion is right for
counting link density and wrong for correctness, and a remediation agent found why — a broken
/eac-formulas sitting in a canonical field, which would have shipped a rel=canonical pointing
at a page that does not exist. That is worse than a broken body link: it tells a search engine
the authoritative copy of the article is somewhere unreachable, so the article can lose its own
ranking rather than merely wasting a click.
"""
import json, re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "pciglobal.ai",
           "pciworld.org", "credentialfinder.org"]
FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
URL_RE = re.compile(r"https?://[^\s)\"'`<>,]+")

try:
    VALID = set(json.load(open(ROOT / "_tools/valid_urls.json")))
except Exception:
    VALID = set()


def check(path):
    text = path.read_text(encoding="utf-8")
    m = FM_RE.match(text)
    if not m:
        return {"file": str(path.relative_to(ROOT)), "issues": ["no front matter"]}
    fm = m.group(1)
    issues, found = [], {}
    for field in ("canonical", "cta_link", "permalink"):
        v = re.search(rf"^{field}:\s*(.+)$", fm, re.M)
        if not v:
            continue
        raw = v.group(1).strip()
        found[field] = raw
        for u in URL_RE.findall(raw):
            u = u.rstrip(".,;:)")
            host = re.sub(r"^https?://", "", u).split("/")[0].lower()
            if not any(host == d or host.endswith("." + d) for d in DOMAINS):
                continue
            stem = u.rstrip("/")
            pp = re.sub(r"^https?://[^/]+", "", stem)
            if pp and stem not in VALID and stem + "/" not in VALID:
                issues.append(f"{field} points at a page that does not exist: {u}")
    return {"file": str(path.relative_to(ROOT)), "fields": found, "issues": issues}


def main():
    rows = []
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            rows.append(check(f))
    bad = [r for r in rows if r["issues"]]
    print(f"checked {len(rows)} | clean {len(rows)-len(bad)} | issues {len(bad)}")
    for r in bad:
        print(f"  {r['file']}: {'; '.join(r['issues'])}")
    (ROOT / "_tools" / "frontmatter_audit.json").write_text(json.dumps(rows, indent=1))
    return 1 if bad else 0


if __name__ == "__main__":
    sys.exit(main())
