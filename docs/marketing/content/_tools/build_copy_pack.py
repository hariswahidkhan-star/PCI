#!/usr/bin/env python3
"""Build the copy pack: every piece's text, ready to paste, with nothing a poster does not need.

The front matter, the audit trail and the linking rationale are all stripped. What survives is
the copy itself, the one link to place, the hashtags, and any timing note — laid out so
somebody can work down it with the tracker open beside them.
"""
import importlib.util
import re
import subprocess
from pathlib import Path

HERE = Path(__file__).resolve().parent
_b = importlib.util.spec_from_file_location("bb", HERE / "build_bundle.py")
bb = importlib.util.module_from_spec(_b); _b.loader.exec_module(bb)
_p = importlib.util.spec_from_file_location("mp", HERE / "build_marketing_pack.py")
mp = importlib.util.module_from_spec(_p); _p.loader.exec_module(mp)


def build():
    files = bb.load()
    for r in files:
        m = bb.FM.match(r["path"].read_text(encoding="utf-8"))
        fm = m.group(1) if m else ""
        r["cta"] = bb.g(fm, "cta_link")
        r["when"] = bb.g(fm, "when_to_post")
        r["phase"], r["phase_name"] = mp.phase_of(r)
    files.sort(key=lambda r: (r["phase"], r["group"], r["path"].name))

    o = ["% PCI content copy pack", "% Project Controls Institute Global", "",
         "# How to use this", "",
         "Every piece is here in the order to post it, with the copy ready to paste. The "
         "posting tracker spreadsheet has the same order, so keep the two side by side: the "
         "tracker tells you what is due, this tells you what to say.", "",
         "Each entry gives you the copy, the one link that piece carries, and its hashtags. "
         "Nothing else needs to travel with it.", "",
         "**Four things that must not change on any platform.** The 15,613 calculation checks "
         "may only appear in a sentence that also says it covers PFL-AI and PML-AI. The "
         "40/40/20 split describes the Body of Knowledge and never the examination. No "
         "accreditation, recognition or endorsement may be claimed. No pass rates, student "
         "numbers or salary figures.", "",
         "**Do not add links.** Each piece carries one, chosen so that no piece points at all "
         "five domains and none points twice at the same one. That restraint is what keeps the "
         "five sites from being read as a link network.", "", "\\newpage", ""]

    phase, grp = None, None
    n = 0
    for r in files:
        if r["phase_name"] != phase:
            phase = r["phase_name"]
            o += ["", f"# {phase}", ""]
            grp = None
        if r["group"] != grp:
            grp = r["group"]
            o += ["", f"## {grp}", ""]
        n += 1
        o += ["", f"### {n}. {r['title']}", ""]
        bits = []
        if r["kw"]:
            bits.append(f"**Keyword:** {r['kw']}")
        bits.append(f"**Length:** {r['words']:,} words")
        # Same rule as the tracker, so the two never disagree about which link to place:
        # the declared call to action first, the first estate link in the prose only as a
        # fallback.
        cta = ""
        if r["cta"]:
            m2 = re.search(r"https?://\S+", r["cta"])
            if m2:
                cta = m2.group(0).rstrip(".,;)")
        if not cta and r["links"]:
            l = r["links"][0]
            cta = f"https://{l['domain']}{l['path']}"
        bits.append(f"**Link to place:** {cta}" if cta
                    else "**No link** — deliberate on this platform")
        o += [" · ".join(bits), ""]
        if r["hashtags"] and not r["hashtags"].lower().startswith(("n/a", "none")):
            o += [f"**Hashtags:** {r['hashtags'][:300]}", ""]
        if r["when"]:
            o += [f"**When:** {mp.clean(r['when'], 700)}", ""]
        o += ["---", "", bb.demote(r["body"], 4), ""]

    src = bb.ROOT.parent / "_copy_pack.md"
    src.write_text("\n".join(o), encoding="utf-8")
    dest = bb.ROOT.parent / "PCI-content-copy-pack.docx"
    subprocess.run(["pandoc", str(src), "-o", str(dest),
                    "--toc", "--toc-depth=2", "-V", "geometry:margin=2cm"],
                   check=True, capture_output=True)
    src.unlink()
    return dest, n


if __name__ == "__main__":
    d, n = build()
    print(f"{d}  ({d.stat().st_size/1024:.0f} KB)  {n} pieces")
