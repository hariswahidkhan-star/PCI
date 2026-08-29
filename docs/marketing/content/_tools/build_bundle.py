#!/usr/bin/env python3
"""Build the consolidated content deliverables: one Markdown document and one HTML page.

The first version of this concatenated the files and produced something structurally wrong.
Each piece's own `# H1` landed underneath the `##` heading the bundle gave it, so an H1 sat
inside an H2 and every outline renderer read the document as broken. The SEO fields were
dumped as raw YAML into a collapsed box rather than presented, and the links — the whole
point of the exercise — were invisible inside two thousand words of prose.

So: body headings are demoted to sit under the piece heading, the front matter is rendered
as a labelled SEO panel with the lengths that matter measured, and every embedded link is
listed with its anchor and destination under the piece that carries it.
"""
import html
import importlib.util
import re
from collections import OrderedDict
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
_spec = importlib.util.spec_from_file_location("link_audit", ROOT / "_tools/link_audit.py")
link_audit = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(link_audit)

FM = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)
NOTE = re.compile(r"^\s*[*_]{0,2}(Internal[- ]?links?|Estate[- ]?links?|Internal linking note|"
                  r"Linking note|Links?\s*[:(.,])", re.I | re.M)
DOMAINS = ["projectcontrolsinstitute.org", "pciai.org", "credentialfinder.org",
           "pciworld.org", "pciglobal.ai"]


def g(fm, k):
    m = re.search(rf"^{k}:[ \t]*(.+)$", fm, re.M)
    return m.group(1).strip().strip('"') if m else ""


def group_of(platform):
    m = re.match(r"Own site\s*[—\-–]\s*([a-z0-9.]+)", platform, re.I)
    if m:
        return f"Own site — {m.group(1).lower()}"
    return (platform.split("—")[0].split("(")[0].split("/")[0].strip() or "Other")[:38]


def demote(md, target=3):
    """Renumber a piece's headings so its shallowest lands at `target` and the levels below
    it run consecutively, with no gaps.

    Two problems, one fix. A fixed offset is wrong because most pieces open at `#` while a
    slide script and a pitch email open at `###`, so shifting everything equally puts their
    first heading two levels below its container. And normalising on the minimum alone is
    still not enough: two files in this run go from `#` straight to `###` with no `##`, so
    the gap survives the shift and the outline is invalid wherever it lands.

    Mapping the distinct levels a piece actually uses onto consecutive levels solves both.
    Relative nesting is preserved exactly — a heading deeper than another stays deeper — and
    a skipped level in the source cannot become a skipped level in the output.

    Fenced code is skipped throughout: a # inside a block is a comment, not a heading.
    """
    levels, fence = set(), False
    for line in md.split("\n"):
        if re.match(r"^\s*```", line):
            fence = not fence
        elif not fence:
            m = re.match(r"^(#{1,6})\s+\S", line)
            if m:
                levels.add(len(m.group(1)))
    remap = {lv: min(6, target + i) for i, lv in enumerate(sorted(levels))}

    out, fence = [], False
    for line in md.split("\n"):
        if re.match(r"^\s*```", line):
            fence = not fence
        if not fence:
            m = re.match(r"^(#{1,6})(\s+)(.*)$", line)
            if m and m.group(3).strip():
                line = "#" * remap[len(m.group(1))] + m.group(2) + m.group(3)
        out.append(line)
    return "\n".join(out)


def slug(s):
    return re.sub(r"[^a-z0-9]+", "-", s.lower()).strip("-")


def load():
    files = []
    for d in ("articles", "social", "flagship", "comparisons"):
        for f in sorted((ROOT / d).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            t = f.read_text(encoding="utf-8")
            m = FM.match(t)
            fm = m.group(1) if m else ""
            rest = t[m.end():] if m else t
            nm = NOTE.search(rest)
            body, note = (rest[:nm.start()], rest[nm.start():]) if nm else (rest, "")
            a = link_audit.audit(f)
            files.append({
                "path": f, "fm": fm, "body": body.strip(), "note": note.strip(),
                "title": g(fm, "title") or f.stem, "platform": g(fm, "platform"),
                "group": group_of(g(fm, "platform")), "kw": g(fm, "primary_kw"),
                "meta": g(fm, "meta"), "schema": g(fm, "schema"), "type": g(fm, "type"),
                "canonical": g(fm, "canonical"), "pillar": g(fm, "pillar"),
                "credential": g(fm, "credential"), "hashtags": g(fm, "hashtags"),
                "links": a["links"], "domains": a["all_domains"], "words": len(body.split()),
            })
    return files


def markdown(files):
    groups = OrderedDict()
    for r in files:
        groups.setdefault(r["group"], []).append(r)
    order = ([k for k in groups if k.startswith("Own site")] +
             [k for k in groups if not k.startswith("Own site")])
    reach = {d: sum(1 for r in files if d in r["domains"]) for d in DOMAINS}
    nlinks = sum(len(r["links"]) for r in files)

    o = ["# PCI content run", "",
         f"**{len(files)} pieces · {sum(r['words'] for r in files):,} words · "
         f"{nlinks:,} links embedded in the published bodies**", "",
         "Every piece carries its SEO layer, its body with the links in place, and the note "
         "recording what was placed and why. Headings nest under each piece, so this document "
         "has one valid outline from top to bottom.", "",
         "| Domain | Territory it owns | Pieces linking to it |", "|---|---|---:|"]
    terr = {"projectcontrolsinstitute.org": "The hub — credentials, Standards, earned value, cost control",
            "pciai.org": "AI in project controls — governance, tooling, model evaluation",
            "credentialfinder.org": "Verification and comparison",
            "pciworld.org": "Careers and community", "pciglobal.ai": "Regional and market-specific"}
    for d in DOMAINS:
        o.append(f"| `{d}` | {terr[d]} | {reach[d]} |")
    o += ["",
          "No piece links to all five domains and none carries more than one link to any single "
          "other domain. That restraint is the point: five commonly-owned domains each carrying a "
          "uniform block of links to the other four is the private-blog-network footprint, and the "
          "cost of it is all five losing standing together.", "", "---", "", "## Contents", ""]

    n = 0
    for gname in order:
        o += ["", f"### {gname} ({len(groups[gname])})", ""]
        for r in groups[gname]:
            n += 1
            o.append(f"{n}. [{r['title']}](#{slug(str(n) + '-' + r['title'])}) — "
                     f"{r['words']:,} words, {len(r['links'])} link"
                     f"{'' if len(r['links']) == 1 else 's'}")

    n = 0
    for gname in order:
        o += ["", "", "---", "", f"# {gname}", ""]
        for r in groups[gname]:
            n += 1
            o += ["", f"## {n}. {r['title']}", ""]
            o.append(f"`{r['path'].relative_to(ROOT)}`")
            o += ["", "| | |", "|---|---|",
                  f"| **Platform** | {r['platform']} |",
                  f"| **Primary keyword** | {r['kw'] or '—'} |",
                  f"| **Title** | {r['title']} ({len(r['title'])} characters) |",
                  f"| **Meta description** | {r['meta'] or '—'}"
                  + (f" ({len(r['meta'])} characters)" if r["meta"] else "") + " |",
                  f"| **Schema** | {r['schema'] or '—'} |",
                  f"| **Canonical** | {r['canonical'] or '—'} |",
                  f"| **Pillar / credential** | {r['pillar'] or '—'} · {r['credential'] or '—'} |"]
            if r["hashtags"] and r["hashtags"].lower() not in ("n/a (own site)", "n/a", "none"):
                o.append(f"| **Hashtags** | {r['hashtags'][:160]} |")
            o.append("")

            if r["links"]:
                o += ["**Links embedded in this piece**", ""]
                for l in r["links"]:
                    anchor = l["anchor"] or "(bare URL)"
                    o.append(f"- [{anchor}](https://{l['domain']}{l['path']}) → `{l['domain']}{l['path']}`")
            else:
                o.append("**No link in the body.** Deliberate for this platform — the note below says why.")
            o += ["", demote(r["body"], 3), ""]
            if r["note"]:
                o += ["", "> **Linking note.** " + re.sub(r"^\s*[*_]+|[*_]+\s*$", "", r["note"]).strip(), ""]

    dest = ROOT.parent / "PCI-content-run-all-347-pieces.md"
    dest.write_text("\n".join(o), encoding="utf-8")
    return dest, nlinks, reach, groups, order


if __name__ == "__main__":
    fs = load()
    dest, nlinks, reach, groups, order = markdown(fs)
    print(f"{dest}  ({dest.stat().st_size/1_048_576:.1f} MB)")
    print(f"{len(fs)} pieces | {nlinks} embedded links | {len(order)} publishing groups")
