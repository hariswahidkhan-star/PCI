#!/usr/bin/env python3
"""Anchor-text diversity across the estate.

_LINK_ARCHITECTURE.md section 2: an anchor that never varies is itself a detection signal.
Natural editorial linking produces a long tail — many phrasings for the same destination,
because each was written to fit its own sentence. A generated link profile produces the
opposite: one anchor repeated because it was pasted. This measures which shape the estate has,
per destination, since that is the unit a search engine evaluates.
"""
import json, re, sys
from collections import defaultdict
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "pciglobal.ai",
           "pciworld.org", "credentialfinder.org"]
FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
FENCE_RE = re.compile(r"^```.*?^```", re.S | re.M)
# The trailing publisher note goes by several labels. It was headed "Links:" in a handful of
# flagship files, which this pattern did not match, so their instruction sheets were read as
# body links and the files were reported as over-linked when they were not.
NOTE_RE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Internal linking note|Linking note|Links?\s*[:(.,])", re.I | re.M)
MD_LINK = re.compile(r"\[([^\]]*)\]\(\s*(https?://[^\s)]+)\s*\)")


def body_of(text):
    m = FM_RE.match(text)
    rest = text[m.end():] if m else text
    n = NOTE_RE.search(rest)
    return FENCE_RE.sub("", rest[:n.start()] if n else rest)


def main():
    per = defaultdict(lambda: defaultdict(list))     # url -> anchor -> [files]
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            for m in MD_LINK.finditer(body_of(f.read_text(encoding="utf-8"))):
                url, anchor = m.group(2), " ".join(m.group(1).split()).lower()
                host = re.sub(r"^https?://", "", url).split("/")[0].lower()
                if not any(host == d or host.endswith("." + d) for d in DOMAINS):
                    continue
                per[url.rstrip("/")][anchor].append(f.name)

    rows, worst = [], []
    for url, anchors in sorted(per.items()):
        uses = sum(len(v) for v in anchors.values())
        if uses < 3:
            continue
        top, topn = max(anchors.items(), key=lambda kv: len(kv[1]))
        share = len(top[1] if isinstance(top, tuple) else topn) / uses
        share = len(topn) / uses
        rows.append({"url": url, "uses": uses, "distinct_anchors": len(anchors),
                     "top_anchor": top if isinstance(top, str) else top,
                     "top_share": round(share, 2)})
        if share >= 0.5 and uses >= 4:
            worst.append((uses, len(anchors), round(share, 2), url, top))

    print(f"destinations linked 3+ times: {len(rows)}")
    print(f"of those, {len(worst)} have one anchor on 50%+ of their links\n")
    for uses, n, share, url, top in sorted(worst, reverse=True)[:20]:
        print(f"  {uses:3d} links, {n:2d} distinct anchors, top {share:.0%}  {url.split('//')[1]}")
        print(f"        \"{top}\"")
    (ROOT / "_tools" / "anchor_audit.json").write_text(json.dumps(rows, indent=1))


if __name__ == "__main__":
    main()
