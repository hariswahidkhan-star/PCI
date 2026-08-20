#!/usr/bin/env python3
"""Contract item 21 — generate the copy-ready HTML body for each pilot from its Markdown.

Generating rather than hand-maintaining a second copy is deliberate: two hand-written versions of
the same article drift, and the drift is invisible until a reader sees the wrong one. Output is
CMS-paste safe — semantic tags, no classes, no wrapper page, every link preserved.

Blocked articles (those whose front matter carries "PUBLICATION BLOCKED") are skipped: emitting
paste-ready HTML for an article that must not be published is an accident waiting to happen.

Run from this directory:  python3 md_to_html.py
"""
import html as _html
import os, re, sys

HERE = os.path.dirname(os.path.abspath(__file__))
PILOTS = os.path.join(HERE, "..", "pilots")
OUT = os.path.join(PILOTS, "html")

INLINE = (
    (re.compile(r"\[([^\]]+)\]\((https?://[^)]+)\)"), r'<a href="\2">\1</a>'),
    (re.compile(r"\*\*([^*]+)\*\*"), r"<strong>\1</strong>"),
    (re.compile(r"(?<![*\w])\*([^*]+)\*(?!\w)"), r"<em>\1</em>"),
    (re.compile(r"`([^`]+)`"), r"<code>\1</code>"),
)

def inline(text):
    text = _html.escape(text, quote=False)
    # unescape the markdown link syntax the escape above mangled, then apply inline rules
    text = text.replace("&lt;", "<").replace("&gt;", ">") if "<" not in text else text
    for pattern, repl in INLINE:
        text = pattern.sub(repl, text)
    return text

def body_of(md):
    """The publishable body: the article section plus its CTA and ecosystem block."""
    start = re.search(r"^## 9.14\..*$", md, re.M)
    if not start:
        return None
    end = re.search(r"^\*\*17\. External source list\*\*", md[start.end():], re.M)
    section = md[start.end(): start.end() + end.start()] if end else md[start.end():]
    # drop the contract's own scaffolding lines from the publishable copy
    section = re.sub(r"^\*\*1[56]\. .*?\*\*\s*", "", section, flags=re.M)
    return section

def convert(section):
    out, lines, i = [], section.split("\n"), 0
    while i < len(lines):
        line = lines[i].rstrip()
        if not line.strip():
            i += 1; continue
        if line.startswith("> "):                       # blockquote (incl. multi-line)
            quote = []
            while i < len(lines) and (lines[i].startswith("> ") or lines[i].strip() == ">"):
                quote.append(lines[i][2:].strip()); i += 1
            out.append("<blockquote><p>" + inline(" ".join(q for q in quote if q)) + "</p></blockquote>")
            continue
        if line.startswith("```"):                      # fenced code
            i += 1; code = []
            while i < len(lines) and not lines[i].startswith("```"):
                code.append(lines[i]); i += 1
            i += 1
            out.append("<pre><code>" + _html.escape("\n".join(code)) + "</code></pre>")
            continue
        if re.match(r"^\|.*\|$", line):                 # table
            rows = []
            while i < len(lines) and re.match(r"^\|.*\|$", lines[i].rstrip()):
                rows.append([c.strip() for c in lines[i].strip().strip("|").split("|")]); i += 1
            if len(rows) >= 2 and set("".join(rows[1]).replace("|", "")) <= set("-: "):
                head, body = rows[0], rows[2:]
            else:
                head, body = None, rows
            t = ["<table>"]
            if head:
                t.append("<thead><tr>" + "".join(f"<th>{inline(c)}</th>" for c in head) + "</tr></thead>")
            t.append("<tbody>")
            for r in body:
                t.append("<tr>" + "".join(f"<td>{inline(c)}</td>" for c in r) + "</tr>")
            t.append("</tbody></table>")
            out.append("".join(t))
            continue
        if re.match(r"^#{2,4} ", line):                 # heading (## -> h2)
            level = len(line) - len(line.lstrip("#"))
            out.append(f"<h{level}>{inline(line[level:].strip())}</h{level}>")
            i += 1; continue
        # Lists. A wrapped item continues on an indented line; folding those back into the item is
        # the difference between one <li> and an <li> followed by a stray <p> of its own second half.
        for marker, tag in ((r"^[-*] ", "ul"), (r"^\d+\. ", "ol")):
            if re.match(marker, line):
                items = []
                while i < len(lines):
                    cur = lines[i].rstrip()
                    if re.match(marker, cur):
                        items.append(re.sub(marker, "", cur, count=1)); i += 1
                    elif items and cur.startswith("  ") and cur.strip():
                        items[-1] += " " + cur.strip(); i += 1
                    else:
                        break
                out.append(f"<{tag}>" + "".join(f"<li>{inline(x)}</li>" for x in items) + f"</{tag}>")
                break
        else:
            pass
        if re.match(r"^[-*] ", line) or re.match(r"^\d+\. ", line):
            continue
        if line.startswith("---"):
            i += 1; continue
        para = [line]                                   # paragraph
        i += 1
        while i < len(lines) and lines[i].strip() and not re.match(r"^(#{2,4} |[-*] |\d+\. |\||> |```|---)", lines[i]):
            para.append(lines[i].rstrip()); i += 1
        out.append("<p>" + inline(" ".join(para)) + "</p>")
    return "\n".join(out)

def main():
    os.makedirs(OUT, exist_ok=True)
    written, skipped = 0, []
    for name in sorted(os.listdir(PILOTS)):
        if not name.endswith(".md"):
            continue
        md = open(os.path.join(PILOTS, name), encoding="utf-8").read()
        if "PUBLICATION BLOCKED" in md:
            skipped.append(name); continue
        section = body_of(md)
        if section is None:
            skipped.append(name + " (no article section)"); continue
        target = os.path.join(OUT, name.replace(".md", ".html"))
        open(target, "w", encoding="utf-8").write(convert(section) + "\n")
        written += 1
        print(f"  wrote {os.path.relpath(target, PILOTS)}")
    for s in skipped:
        print(f"  SKIPPED (blocked or no body): {s}")
    print(f"\n  == {written} HTML bodies generated, {len(skipped)} skipped ==")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
