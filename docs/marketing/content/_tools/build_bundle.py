#!/usr/bin/env python3
"""Concatenate the whole content run into one readable document.

Grouped by where each piece is published rather than by filename, because that is how
somebody actually works through it: everything going on one domain together, then each
off-site platform. Every piece keeps its front matter (the publisher needs it), its body
with the links embedded, and its trailing note.
"""
import importlib.util
import re
from collections import OrderedDict
from pathlib import Path

# Reuse link_audit's own counting rather than reimplementing it. It knows things this file
# should not have to rediscover: that a bare URL on its own line is a link (most social
# pieces put theirs in a first comment, unmarked), that the trailing publisher note is not
# published prose, and which host each piece sits on. Counting it twice in two places is how
# one number becomes two.
_spec = importlib.util.spec_from_file_location("link_audit", Path(__file__).resolve().parent / "link_audit.py")
link_audit = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(link_audit)

ROOT = Path(__file__).resolve().parent.parent
FM = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
LINK = re.compile(r"\[([^\]]*)\]\((https?://[^\s)]+)\)")
# Count links the way link_audit.py does — in the published body only, not in the trailing
# publisher note. The note names URLs it is recording rather than placing, so counting it
# inflates every figure and would put two different numbers in circulation for one thing.
NOTE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Estate[- ]?links?|Internal linking note|"
                  r"Linking note|Links?\s*[:(.,])", re.I | re.M)
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "credentialfinder.org",
           "pciworld.org", "pciglobal.ai"]


def g(fm, k):
    m = re.search(rf"^{k}:\s*(.+)$", fm, re.M)
    return m.group(1).strip().strip('"') if m else ""


def group_of(platform):
    m = re.match(r"Own site\s*[—\-–]\s*([a-z0-9.]+)", platform, re.I)
    if m:
        return f"Own site — {m.group(1).lower()}"
    return (platform.split("—")[0].split("(")[0].split("/")[0].strip() or "Other")[:40]


files = []
for d in ("articles", "social", "flagship"):
    for f in sorted((ROOT / d).glob("*.md")):
        if f.name.startswith("_") or f.name == "INDEX.md":
            continue
        t = f.read_text(encoding="utf-8")
        m = FM.match(t)
        fm = m.group(1) if m else ""
        files.append({"path": f, "dir": d, "fm": fm, "body": t[m.end():] if m else t,
                      "title": g(fm, "title") or f.stem, "platform": g(fm, "platform"),
                      "group": group_of(g(fm, "platform")), "kw": g(fm, "primary_kw")})
        audit = link_audit.audit(f)
        files[-1]["links"] = [(l["anchor"], f"https://{l['domain']}{l['path']}") for l in audit["links"]]
        files[-1]["domains"] = audit["all_domains"]

groups = OrderedDict()
for r in files:
    groups.setdefault(r["group"], []).append(r)
order = [k for k in groups if k.startswith("Own site")] + \
        [k for k in groups if not k.startswith("Own site")]

total_links = sum(len(r["links"]) for r in files)
reach = {d: sum(1 for r in files if d in r["domains"]) for d in DOMAINS}

out = []
out.append("# PCI content run — all 347 pieces, with links embedded\n")
out.append("Every article, post, carousel and platform asset in the run, in one document. Each piece "
           "carries its front matter, its body with the links in place, and the note recording what "
           "was placed and why.\n")
out.append(f"**{len(files)} pieces · {sum(len(r['body'].split()) for r in files):,} words · "
           f"{total_links:,} links embedded in the published bodies**\n")
out.append("Pieces linking to each domain:\n")
out.append("| Domain | Pieces linking to it |\n|---|---:|")
for d in DOMAINS:
    out.append(f"| `{d}` | {reach[d]} |")
out.append("")
out.append("Links are chosen so that no piece links to all five domains and none carries more than "
           "one link to any single other domain — the pattern that would otherwise read as a private "
           "blog network and cost all five domains at once. The reasoning is in "
           "`_LINK_ARCHITECTURE.md`.\n")

out.append("---\n\n## Contents\n")
n = 0
for gname in order:
    out.append(f"\n**{gname}** ({len(groups[gname])})\n")
    for r in groups[gname]:
        n += 1
        anchor = re.sub(r"[^a-z0-9]+", "-", f"{n}-{r['title']}".lower()).strip("-")
        out.append(f"{n}. [{r['title']}](#{anchor})")

n = 0
for gname in order:
    out.append(f"\n\n---\n\n# {gname}\n")
    for r in groups[gname]:
        n += 1
        out.append(f"\n\n## {n}. {r['title']}\n")
        out.append(f"*{r['path'].relative_to(ROOT)} · {r['platform']}*\n")
        if r["kw"]:
            out.append(f"*Primary keyword: {r['kw']}*\n")
        if r["links"]:
            out.append(f"*Embedded links: {len(r['links'])}*\n")
        else:
            out.append("*No link in the body — deliberate for this platform; the note says why.*\n")
        out.append("\n<details><summary>Publishing brief (front matter)</summary>\n\n```yaml\n"
                   + r["fm"] + "\n```\n\n</details>\n")
        out.append("\n" + r["body"].strip() + "\n")

dest = ROOT.parent / "PCI-content-run-all-347-pieces.md"
dest.write_text("\n".join(out), encoding="utf-8")
print(f"{dest}")
print(f"{len(files)} pieces | {total_links:,} embedded links | {dest.stat().st_size/1_048_576:.1f} MB")
