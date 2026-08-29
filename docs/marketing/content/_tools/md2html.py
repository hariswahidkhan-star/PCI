#!/usr/bin/env python3
"""A Markdown-to-HTML converter for exactly the constructs this content run uses.

Nothing general-purpose is available in this environment, and nothing general-purpose is
needed: a survey of all 347 files found tables, headings, horizontal rules, blockquotes,
bullets, ordered lists, fenced code, and inline bold, italic, code and links. That is the
whole grammar, so that is what this handles.

Tables matter most — 4,153 rows across the run, because _BRIEF.md asks for a comparison
table wherever the subject has honest axes, and tables are among the most-cited formats
there is. They get their own scroll container so a wide one never makes the page scroll
sideways.
"""
import html
import re

_ESC = {"&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;"}


def esc(s):
    return "".join(_ESC.get(c, c) for c in s)


def inline(s):
    """Inline spans. Code is extracted first and reinserted last, so a backtick span
    containing an asterisk or a bracket is never re-parsed as emphasis or a link — the
    mistake that silently swallowed formulas in an earlier tool on this project."""
    holds = []

    def hold(m):
        holds.append(m.group(1))
        return f"\x00{len(holds)-1}\x00"

    s = re.sub(r"`([^`\n]+)`", hold, s)
    s = esc(s)
    s = re.sub(r"\[([^\]]*)\]\((https?://[^\s)]+)\)",
               lambda m: f'<a href="{m.group(2)}" target="_blank" rel="noopener">{m.group(1)}</a>', s)
    s = re.sub(r"\*\*([^*]+)\*\*", r"<strong>\1</strong>", s)
    s = re.sub(r"(?<!\*)\*([^*\n]+)\*(?!\*)", r"<em>\1</em>", s)
    s = re.sub(r"(?<![\w/])_([^_\n]+)_(?![\w/])", r"<em>\1</em>", s)
    s = re.sub(r"\x00(\d+)\x00", lambda m: f"<code>{esc(holds[int(m.group(1))])}</code>", s)
    return s


def convert(md, heading_base=3):
    lines = md.split("\n")
    out, i = [], 0
    while i < len(lines):
        ln = lines[i]

        if re.match(r"^\s*```", ln):
            i += 1
            buf = []
            while i < len(lines) and not re.match(r"^\s*```", lines[i]):
                buf.append(lines[i]); i += 1
            i += 1
            out.append(f'<pre><code>{esc(chr(10).join(buf))}</code></pre>')
            continue

        m = re.match(r"^(#{1,6})\s+(.*)$", ln)
        if m:
            lvl = min(6, len(m.group(1)) + heading_base - 1)
            txt = m.group(2).strip()
            out.append(f'<h{lvl} id="{re.sub(r"[^a-z0-9]+", "-", txt.lower()).strip("-")[:80]}">'
                       f'{inline(txt)}</h{lvl}>')
            i += 1
            continue

        if re.match(r"^\s*(-{3,}|={3,})\s*$", ln):
            out.append("<hr />"); i += 1; continue

        if re.match(r"^\s*\|", ln):
            rows = []
            while i < len(lines) and re.match(r"^\s*\|", lines[i]):
                rows.append(lines[i]); i += 1
            cells = [[c.strip() for c in r.strip().strip("|").split("|")] for r in rows]
            sep = next((k for k, r in enumerate(cells)
                        if all(re.fullmatch(r":?-{2,}:?", c or "") for c in r if c != "")), None)
            head = cells[:sep] if sep else []
            body = cells[sep + 1:] if sep is not None else cells
            t = ['<div class="tw"><table>']
            if head:
                t.append("<thead>" + "".join(
                    "<tr>" + "".join(f"<th>{inline(c)}</th>" for c in r) + "</tr>" for r in head) + "</thead>")
            t.append("<tbody>" + "".join(
                "<tr>" + "".join(f"<td>{inline(c)}</td>" for c in r) + "</tr>" for r in body) + "</tbody>")
            t.append("</table></div>")
            out.append("".join(t))
            continue

        if re.match(r"^\s*>", ln):
            buf = []
            while i < len(lines) and (re.match(r"^\s*>", lines[i]) or lines[i].strip() == ""):
                if lines[i].strip() == "":
                    if i + 1 < len(lines) and re.match(r"^\s*>", lines[i + 1]):
                        buf.append(""); i += 1; continue
                    break
                buf.append(re.sub(r"^\s*>\s?", "", lines[i])); i += 1
            out.append(f"<blockquote>{convert(chr(10).join(buf), heading_base)}</blockquote>")
            continue

        m = re.match(r"^\s*([-*])\s+(.*)$", ln)
        if m:
            items = []
            while i < len(lines) and re.match(r"^\s*[-*]\s+", lines[i]):
                items.append(re.sub(r"^\s*[-*]\s+", "", lines[i])); i += 1
            out.append("<ul>" + "".join(f"<li>{inline(x)}</li>" for x in items) + "</ul>")
            continue

        if re.match(r"^\s*\d+\.\s+", ln):
            items = []
            while i < len(lines) and re.match(r"^\s*\d+\.\s+", lines[i]):
                items.append(re.sub(r"^\s*\d+\.\s+", "", lines[i])); i += 1
            out.append("<ol>" + "".join(f"<li>{inline(x)}</li>" for x in items) + "</ol>")
            continue

        if ln.strip() == "":
            i += 1; continue

        buf = []
        while i < len(lines) and lines[i].strip() != "" and not re.match(
                r"^\s*(\||>|#{1,6}\s|```|[-*]\s|\d+\.\s|-{3,}\s*$)", lines[i]):
            buf.append(lines[i].strip()); i += 1
        if buf:
            out.append(f"<p>{inline(' '.join(buf))}</p>")
    return "\n".join(out)
