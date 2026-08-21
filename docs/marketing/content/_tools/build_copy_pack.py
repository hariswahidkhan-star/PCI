#!/usr/bin/env python3
"""Build the copy pack: one self-contained block per post, to select and paste.

Written for somebody who has not read any of the briefing and should not have to. Each entry
says which platform and which account, gives one block that already contains the text, the
URL and the hashtags, and then says what to do after pasting. Nothing is assembled by the
poster and nothing is looked up in another file.

How the URL appears depends on where the copy is going, because the two destinations behave
differently. Long articles are pasted into a CMS, which accepts rich text, so their links
stay as real clickable hyperlinks and read as published prose. Social copy is pasted into a
plain-text composer that discards formatting, so a clickable link would arrive as bare
anchor text with the address gone — those get the URL written out literally instead.
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

# Where the link goes on each surface, in plain words. A poster should never have to know
# that LinkedIn suppresses reach on posts with an outbound link in the body.
PLACEMENT = {
    "linkedin post": "Post the text above. Then add a comment on your own post containing "
                     "only this link:",
    "linkedin carousel": "Upload the slides as a document post with the caption above. Then "
                         "add a comment on your own post containing only this link:",
    "linkedin company page post": "Post from the PCI company page. Then add a comment "
                                  "containing only this link:",
    "linkedin personal/founder post": "Post from your personal profile, not the company page. "
                                      "Then add a comment containing only this link:",
    "instagram": "Put this link in the bio before posting, because Instagram captions "
                 "cannot contain a clickable link:",
    "instagram / facebook": "On Instagram put this link in the bio first. On Facebook it can "
                            "go in the post itself:",
    "instagram / facebook carousel": "On Instagram put this link in the bio first. On "
                                     "Facebook it can go in the post itself:",
    "pinterest": "Set this as the Pin's destination link:",
    "youtube shorts": "Put this link in the video description:",
}


def placement_note(platform):
    p = platform.lower()
    for k, v in PLACEMENT.items():
        if p.startswith(k):
            return v
    return None


def fm_value(fm, key):
    """Read a front-matter value, including YAML block scalars.

    bb.g reads to the end of the line, which is right for 43 of the 47 pieces that carry a
    posting note and useless for the other four: they write it as a block scalar, so the line
    holds only the "|" and the entry rendered as "When: |". Reading the indented block that
    follows is the difference between a timing instruction and a stray pipe character.
    """
    m = re.search(rf"^{key}:[ \t]*(.*)$", fm, re.M)
    if not m:
        return ""
    head = m.group(1).strip()
    if head not in ("|", ">", "|-", ">-", "|+", ">+"):
        return head.strip('"')
    lines = fm[m.end():].split("\n")[1:] if False else fm[m.end():].lstrip("\n").split("\n")
    out = []
    for ln in lines:
        if ln.strip() and not ln.startswith((" ", "\t")):
            break
        out.append(ln.strip())
    return " ".join(x for x in out if x)


def social_links(md):
    """Write links out literally, for copy that lands in a plain-text composer."""
    return re.sub(r"\[([^\]]*)\]\((https?://[^)]*)\)", r"\1: \2", md)


def build():
    files = bb.load()
    for r in files:
        m = bb.FM.match(r["path"].read_text(encoding="utf-8"))
        fm = m.group(1) if m else ""
        r["cta"] = bb.g(fm, "cta_link")
        r["when"] = fm_value(fm, "when_to_post")
        r["phase"], r["phase_name"] = mp.phase_of(r)
    files.sort(key=lambda r: (r["phase"], r["group"], r["path"].name))

    o = ["% PCI content — ready to post", "% Project Controls Institute Global", "",
         "# Read this once, then you can work down the list", "",
         "Every post is below in the order to publish it. Each one gives you the platform, "
         "the account to post from, and one block of text to select and paste. The links and "
         "the hashtags are already inside that block. You do not need any other file.", "",
         "Where a platform wants the link somewhere other than the post itself — LinkedIn "
         "wants it in the first comment, Instagram wants it in the bio — the entry says so "
         "underneath, with the link on its own line to copy.", "",
         "**Four things that must never be changed, on any platform.**", "",
         "1. Wherever the figure 15,613 appears, the same sentence must also say it covers "
         "PFL-AI and PML-AI. Never quote it on its own.", "",
         "2. The 40/40/20 split describes the Body of Knowledge. It is never an exam "
         "weighting, because the exam blueprint has not been decided.", "",
         "3. Never say or imply that PCI is accredited, recognised, endorsed, affiliated or "
         "partnered with anyone. It is not, and it says so openly.", "",
         "4. Never add a pass rate, a student number, a salary figure or a success statistic. "
         "If a number is not already in the text, it does not go in.", "",
         "**Do not add links.** Each post carries the one link it is meant to carry. Adding "
         "more can cost all five PCI websites their standing at once.", "", "\\newpage", ""]

    # A hand-built contents page. Pandoc's own table of contents lists the section and platform
    # headings and none of the 577 posts, which tells a reader the shape of the document and
    # nothing about what is in it. This lists every post under the wave it belongs to, grouped
    # by platform, so somebody can find their week's work without scrolling the body.
    WAVE_NOTE = {
        "Launch — flagship assets":
            "The launch set. Run these first, in the order given; several name their own timing.",
        "Own site — publish and let it index":
            "PCI's own pages. These must go live and be indexed BEFORE anything that "
            "canonicalises to them, or a platform with more authority ranks for our article.",
        "Off-site originals":
            "Written for someone else's platform and never published on ours. No canonical, so "
            "these must not duplicate an own-site page.",
        "Republish with canonical (only after its origin has indexed)":
            "Copies of own-site pages, each carrying a canonical home. Do not post any of these "
            "until its origin has been live and indexed for at least two weeks.",
        "Social amplification":
            "Short-form. Run alongside everything above, once the page each points at exists.",
        "Comparisons — publish on credentialfinder.org first":
            "The comparison cluster's own pages. Same rule as the other own-site work: live and "
            "indexed before their platform variants go anywhere.",
        "Comparisons — platform variants":
            "The comparison cluster across every platform. Each derives from a page in the wave "
            "above and waits for it.",
    }

    o += ["# Contents", ""]
    seen_phase, seen_grp, cn = None, None, 0
    for r in files:
        if r["phase_name"] != seen_phase:
            seen_phase = r["phase_name"]
            n_in = sum(1 for x in files if x["phase_name"] == seen_phase)
            o += ["", f"## {seen_phase}  ({n_in} posts)", "",
                  WAVE_NOTE.get(seen_phase, ""), ""]
            seen_grp = None
        if r["group"] != seen_grp:
            seen_grp = r["group"]
            o += ["", f"**{seen_grp}**", ""]
        cn += 1
        o.append(f"- [Post {cn} — {r['title']}](#post-{cn})")
    o += ["", "\\newpage", ""]

    phase, grp, n = None, None, 0
    for r in files:
        if r["phase_name"] != phase:
            phase = r["phase_name"]
            o += ["", f"# {phase}", ""]
            grp = None
        if r["group"] != grp:
            grp = r["group"]
            o += ["", f"## {grp}", ""]
        n += 1
        long_form = r["words"] > 700 and r["phase"] in (2, 3, 4)

        o += ["", f"### Post {n} — {r['title']} {{#post-{n}}}", "",
              f"**Where it goes:** {r['platform']}", ""]
        if r["when"]:
            o += [f"**When:** {mp.clean(r['when'], 420, keep_urls=False)}", ""]
        # Match whole words. A plain substring test classified anything whose platform
        # mentioned a "description" as designer copy, because "description" contains
        # "script" — which swept 43 pieces into the wrong bucket, including the webinar
        # listing that should have had its own.
        t = (r["type"] + " " + r["platform"]).lower()
        def has(*words):
            return any(re.search(rf"\b{w}\b", t) for w in words)
        if has("boilerplate", "directory"):
            action = ("**This one is a reference sheet, not a post.** It gives you the wording "
                      "to use when filling in a company profile on Crunchbase, Google Business "
                      "Profile, Credly and the rest. Use the longest version that fits each "
                      "field, and do not write anything that is not here.**")
        elif has("pitch", "email", "haro", "sos"):
            action = ("**This one is an email, not a post.** Send it to one named person. "
                      "Copy everything between the two lines into the message body.**")
        elif has("script", "deck", "carousel", "pin"):
            action = ("**This one is the words for a designer, not a post.** Everything "
                      "between the two lines goes onto the slides or the graphic. The caption "
                      "to post with it is inside.**")
        elif has("listing", "event"):
            action = ("**This one is a listing, not a post.** Copy everything between the two "
                      "lines into the event or resource form.**")
        else:
            action = "**Copy everything between the two lines and paste it.**"
        o += [action, "", "---", ""]

        body = bb.demote(r["body"], 4)
        o += [body if long_form else social_links(body), ""]

        tags = r["hashtags"]
        if tags and not tags.lower().startswith(("n/a", "none")) and "#" in tags:
            o += ["", tags.split("(")[0].strip(), ""]
        o += ["---", ""]

        cta = ""
        if r["cta"]:
            mm = re.search(r"https?://\S+", r["cta"])
            if mm:
                cta = mm.group(0).rstrip(".,;)")
        if not cta and r["links"]:
            l = r["links"][0]
            cta = f"https://{l['domain']}{l['path']}"
        note = placement_note(r["platform"])
        if note and cta:
            o += [f"**After posting.** {note}", "", cta, ""]
        elif not r["links"] and not cta:
            o += ["**No link on this one.** This platform removes posts that carry a "
                  "promotional link, so it deliberately has none. Do not add one.", ""]

    src = bb.ROOT.parent / "_copy.md"
    src.write_text("\n".join(o), encoding="utf-8")
    dest = bb.ROOT.parent / "PCI-ready-to-post.docx"
    subprocess.run(["pandoc", str(src), "-o", str(dest),
                    "--from=markdown+autolink_bare_uris",
                    "-V", "geometry:margin=2cm"], check=True, capture_output=True)
    src.unlink()
    return dest, n


if __name__ == "__main__":
    d, n = build()
    print(f"{d}  ({d.stat().st_size/1024:.0f} KB)  {n} posts")
