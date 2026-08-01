#!/usr/bin/env python3
"""Build each volume's appendices from the registries and the verified check suite.

Like the glossary, the appendices are **derived** rather than maintained, so they cannot drift from
the chapters or from the golden-answer suite. Five appendices per volume:

  A. Formula and symbol sheet — from registries/FORMULAS.md, filtered to the symbols that volume
     actually uses, with the verification status carried through. A formula that is registered but
     not verified is shown as such rather than quietly listed alongside verified ones.
  B. Notation and conventions — from registries/TERMINOLOGY.md plus the conventions the corpus
     applies everywhere (datetime, currency, rounding, the D.K.T hierarchy, the context-flag rule).
  C. Verification record — a per-domain table of how many golden checks stand behind the volume,
     read live from _build/checks/ and verify_formulas.py. This is the appendix a professional
     reader will actually use to decide how much to trust the arithmetic, and it is generated so it
     cannot be overstated.
  D. Figure index — every figure specification in the volume, with whether artwork has actually
     been drawn for it. A figure without artwork prints as its own specification, so the reader
     sees no gap; the distinction is still stated rather than glossed.
  E. Practitioner's toolkit library — the toolkits indexed by reference, not reprinted. Two copies
     of a template is how a template starts disagreeing with itself.

  The appendix letters here are this volume's own. Where the family's volume plan names an
  appendix that is deliberately not printed (consolidated self-check answers, which would be a
  verbatim second copy) or not yet publishable (a standards register, pending an edition check on
  every entry), Appendix E says so explicitly instead of leaving an apparent omission.

Usage:  python3 make_appendices.py            # write both volumes
        python3 make_appendices.py --check    # report only
"""
import pathlib
import re
import subprocess
import sys

HERE = pathlib.Path(__file__).resolve().parent
ROOT = HERE.parent
REG = ROOT / "registries"

BOOKS = {"pml-ai": ("PML-AI", "PML-AI Body of Knowledge"),
         "pfl-ai": ("PFL-AI", "PFL-AI Body of Knowledge")}


def read_rows(path: pathlib.Path, section_re: str) -> list:
    """Rows of the first markdown table inside the matched section."""
    text = path.read_text(encoding="utf-8")
    m = re.search(section_re + r"[\s\S]*?\n(\|[\s\S]*?)(?=\n##|\Z)", text)
    if not m:
        return []
    rows = []
    for line in m.group(1).strip().split("\n"):
        if not line.startswith("|") or re.match(r"^\|[-| :]+\|$", line):
            continue
        cells = [c.strip() for c in line.strip().strip("|").split("|")]
        if cells and not cells[0].lower().startswith(("symbol", "term", "formula")):
            rows.append(cells)
    return rows


def formula_rows(tag: str) -> list:
    """Inherited symbols plus this volume's own, de-duplicated, with status preserved."""
    out, seen = [], set()
    for section in (r"## 1\. Symbols inherited", rf"## \d\. New symbols — {tag}"):
        for cells in read_rows(REG / "FORMULAS.md", section):
            sym = cells[0]
            key = re.sub(r"[`*]", "", sym).strip().lower()
            if key in seen:
                continue
            seen.add(key)
            # Inherited table has 4 columns (no "first home"); own tables have 5.
            if len(cells) >= 5:
                out.append((sym, cells[1], cells[2], cells[3], cells[4]))
            elif len(cells) == 4:
                out.append((sym, cells[1], cells[2], "inherited (PCP-AI master table)", cells[3]))
    return out


def check_counts() -> dict:
    """book -> {domain: n_checks}, read from the suite's own runtime tally.

    Nothing here counts source occurrences. Several sections of the suite assert inside loops, so a
    source count understated the record by more than a thousand; and attributing modules by position
    credited one domain with 6,595 of another's checks. The suite therefore tallies itself as it runs
    and prints a TALLY line, which this reads. Appendix C states the size of the verification record to
    a reader deciding how much to trust the arithmetic, so it is measured by the thing it describes.
    """
    r = subprocess.run([sys.executable, str(HERE / "verify_formulas.py")],
                       capture_output=True, text=True, cwd=str(HERE), timeout=1800)
    if r.returncode != 0:
        raise SystemExit("the golden-answer suite does not pass — refusing to write a verification "
                         "appendix for a failing suite:\n" + r.stdout[-2000:])
    tally = [l for l in r.stdout.split("\n") if l.startswith("TALLY ")]
    if not tally:
        raise SystemExit("the suite printed no TALLY line — cannot state a verified total without it.")
    counts = {"pml-ai": {}, "pfl-ai": {}}
    for part in tally[0][len("TALLY "):].split(";"):
        if not part.strip():
            continue
        key, n = part.split("=")
        tag, dom = key.split(":")
        counts["pml-ai" if tag == "PML-AI" else "pfl-ai"][int(dom)] = int(n)

    emitted = len([l for l in r.stdout.split("\n") if l.startswith(("PASS  ", "FAIL  "))])
    attributed = sum(sum(v.values()) for v in counts.values())
    # The unattributed remainder is the loader self-test, which is real but is not book content.
    if not 0 <= emitted - attributed <= 10:
        raise SystemExit(f"appendix C reconciliation failed: {attributed} attributed against "
                         f"{emitted} emitted. Fix before claiming a total.")
    return counts


def domain_titles(book: str) -> dict:
    out = {}
    for f in sorted((ROOT / book / "manuscript").glob("domain-*.md")):
        dn = int(re.match(r"domain-(\d+)-", f.name).group(1))
        first = f.read_text(encoding="utf-8").split("\n", 1)[0]
        t = re.sub(r"^#\s*Domain\s*\d+\s*—\s*", "", first)
        out[dn] = re.sub(r"\s*\*\(.*?\)\*\s*$", "", t).strip()
    # Check modules above the domain count belong to the back matter, not to a chapter. They are
    # named here so Appendix C's table reconciles to its own total instead of quietly dropping them.
    BACK_MATTER = {17: "Appendix G — integrated capstones"}
    highest = max(out) if out else 0
    for dn, label in BACK_MATTER.items():
        if dn > highest:
            out[dn] = label
    # Domain 0 is the cross-domain bucket: check modules that span several domains and so cannot be
    # attributed to one of them honestly. It is a row rather than a silent remainder.
    out[0] = "*Cross-domain check modules (span several domains)*"
    return out


def figure_rows(book: str) -> list:
    """Every figure specification in the volume, with whether artwork exists for it.

    The manuscripts describe each figure in a blockquote that `build_book.py` replaces with the
    rendered SVG when one has been drawn. A figure with no artwork still prints — as its own
    description — so the reader is never shown a gap, but the distinction matters to anyone
    working on the volume and is therefore stated rather than hidden.
    """
    figdir = ROOT / book / "build" / "figures"
    out = []
    for f in sorted((ROOT / book / "manuscript").glob("domain-*.md")):
        for line in f.read_text(encoding="utf-8").splitlines():
            m = re.match(r"> \*\*Fig (\d+\.\d+\.\d+) — ([^.*]+)", line)
            if m:
                fid, title = m.group(1), m.group(2).strip()
                drawn = (figdir / f"fig_{fid.replace('.', '_')}.svg").exists()
                out.append((fid, title, drawn))
    return sorted(out, key=lambda r: [int(p) for p in r[0].split(".")])


def toolkit_rows(book: str) -> list:
    """The practitioner's toolkits, in order, with the first line of what each one is for.

    Consolidated by reference rather than copied: the appendix points at the toolkit in its
    domain instead of reproducing it, because two copies of a template is how a template starts
    disagreeing with itself.
    """
    out = []
    for f in sorted((ROOT / book / "manuscript").glob("domain-*.md")):
        lines = f.read_text(encoding="utf-8").splitlines()
        for i, line in enumerate(lines):
            m = re.match(r"### Toolkit (\d+\.T\.\d+) — (.+)$", line)
            if m:
                lead = ""
                for nxt in lines[i + 1:i + 8]:
                    s = nxt.strip()
                    if s and not s.startswith(("#", ">", "|", "-", "*")):
                        lead = re.sub(r"[*`]", "", s)
                        break
                out.append((m.group(1), m.group(2).strip().rstrip("."), lead))
    return sorted(out, key=lambda r: [int(p) for p in r[0].replace(".T.", ".").split(".")])


def build(book: str) -> str:
    tag, title = BOOKS[book]
    rows = formula_rows(tag)
    verified = sum(1 for r in rows if "✅" in r[4])
    counts = check_counts()[book]
    titles = domain_titles(book)
    total = sum(counts.values())

    L = [f"# Appendices — {title}", "",
         "> **Derived, not maintained.** Every table in these appendices is generated by",
         "> `_build/make_appendices.py` from the shared registries and from the golden-answer suite",
         "> itself, so none of it can drift from the chapters or overstate what has been verified.", ""]

    # ---- Appendix A ----
    L += ["## Appendix A — Formula and symbol sheet", "",
          f"{len(rows)} symbols, of which **{verified} carry a verified worked example** in this volume or its",
          "companion. A symbol marked *pending* is registered and used but does not yet have a golden-answer",
          "check behind a printed result — treat its appearances with the same care you would any unchecked",
          "number. Units are stated because a unit error is the commonest arithmetic failure in practice and the",
          "hardest to see in a finished sentence.", "",
          "| Symbol | Meaning | Unit | First home | Verified |",
          "|---|---|---|---|---|"]
    for sym, meaning, unit, home, status in rows:
        v = "yes" if "✅" in status else "*pending*"
        L.append(f"| {sym} | {meaning} | {unit} | {home} | {v} |")
    L += ["",
          "**Conventions that apply to every formula above.** Financial arithmetic is decimal, never binary",
          "floating point. Intermediate values carry full precision; rounding happens only at display, which is",
          "why a printed figure may not reproduce if you round as you go. Rates and periods must agree — an",
          "annual rate against monthly periods is the error the golden suite catches most often. Datetimes are",
          "`YYYY-MM-DD HH:MM:SS` in UTC and are compared as instants, never lexically.", ""]

    # ---- Appendix B ----
    L += ["## Appendix B — Notation, terminology and conventions", "",
          "Terms fixed at programme level, which the chapters use without redefining:", "",
          "| Term | As used in both volumes |", "|---|---|"]
    for cells in read_rows(REG / "TERMINOLOGY.md", r"## 2\. Programme-level terms"):
        if len(cells) >= 2:
            L.append(f"| {cells[0]} | {cells[1]} |")
    L += ["",
          "### Conventions", "",
          "**The hierarchy.** Content is addressed as `D.K.T` — Domain, Knowledge Area, Topic. A cross-reference",
          "of the form *KA 7.3* or *10.2.2* is precise and is the authority; a page number is not used, because",
          "the corpus is regenerated.", "",
          "**Context flags.** Where one word carries two genuinely different concepts, the later use states the",
          "collision explicitly rather than relying on the reader to infer it — *tailoring* of method against",
          "*tailoring* of a message, `PV` as present value against `PV` as planned value, `A` as annuity payment",
          "against `A` as assets. A flag is a signal that both meanings are live in the corpus, not a warning",
          "that one is wrong.", "",
          "**Citation over restatement.** A result established in one domain is cited from the others rather than",
          "re-derived. If two domains appear to derive the same thing, that is a defect and worth reporting.", "",
          "**Currency and units.** USD throughout, with SAR shown where it aids a Gulf reader at an indicative",
          "`USD 1 ≈ SAR 3.75`. That rate is illustrative and is not a forecast or a quotation. Every worked",
          "example states its units, its currency, its rounding and its date basis; where one is missing, treat",
          "it as a drafting defect.", "",
          "**Fictitious entities.** Every organisation, project, person and case in this volume is invented. Any",
          "resemblance to a real entity is coincidental, and no real project's data is used.", "",
          "**The responsible-AI principle**, which governs every *AI in this KA* section: *AI proposes; the",
          "professional verifies, decides and remains accountable.*", ""]

    # ---- Appendix C ----
    L += ["## Appendix C — Verification record", "",
          f"**{total:,} golden-answer checks** stand behind this volume. Every number printed as a *result* —",
          "in worked examples, in-text calculations, multiple-choice options (all of them, not only the correct",
          "one), exercise solutions, case studies and figure specifications — is recomputed independently with",
          "decimal arithmetic at 28-digit precision and compared to the printed value. Each domain's module also",
          "pins the *invariants* the domain claims: an identity, a breakeven, a bound, an inequality, so that a",
          "claim cannot silently stop being true.", "",
          "| Domain | | Checks |", "|---|---|---|"]
    for dn in sorted(titles):
        L.append(f"| {dn} | {titles[dn]} | {counts.get(dn, 0):,} |")
    L += [f"| | **Total** | **{total:,}** |", "",
          "Reproduce it with `python3 _build/verify_formulas.py`, or check one domain with",
          "`python3 _build/run_checks.py checks/<module>.py`. Both loaders are fail-closed: a check module that",
          "raises is a failure, not a skip.", "",
          "**What this record does and does not establish.** It establishes that the arithmetic is right — that",
          "each number is the one its stated method produces from its stated inputs. It does **not** establish",
          "that the method chosen was the right method for the situation, that the professional judgements are",
          "ones an experienced practitioner would endorse, that the emphasis across topics is well calibrated, or",
          "that nothing important is missing. Those are questions for editorial and technical review, and a",
          "reader should not read a large number in this appendix as an answer to them.", ""]

    # ---- Appendix D ----
    figs = figure_rows(book)
    drawn = sum(1 for _, _, d in figs if d)
    L += ["## Appendix D — Figure index", ""]
    if drawn == len(figs):
        L += [f"**{len(figs)} figures**, all of them drawn as PCI-original artwork generated from source in",
              "`_build/figures_src/`. Each is also specified in words in the chapter, stating every plotted value,"]
    else:
        L += [f"**{len(figs)} figures**, of which **{drawn}** are drawn as PCI-original artwork generated from",
              f"source in `_build/figures_src/`. The remaining {len(figs) - drawn} print as their own written",
              "specifications, stating every plotted value,"]
    L += ["so that a figure can be read — and redrawn — without the artwork, and so that a reader using assistive",
          "technology loses nothing. No figure in either volume reproduces a diagram from another publisher, and",
          "every annotated value in a drawn figure is a literal verified by the golden-answer suite.", "",
          "| Figure | Subject | Artwork |", "|---|---|---|"]
    for fid, title, d in figs:
        L.append(f"| {fid} | {title} | {'drawn' if d else '*specification only*'} |")
    L += [""]

    # ---- Appendix E ----
    tk = toolkit_rows(book)
    L += ["## Appendix E — Practitioner's toolkit library", "",
          f"**{len(tk)} adoption-ready artefacts**, indexed here and defined in the Knowledge Areas they belong",
          "to. They are indexed rather than reprinted deliberately: a template that exists twice is a template",
          "that will disagree with itself, and the version in the chapter is the one the surrounding text",
          "explains. Adapt the headings to your organisation, then keep them stable — a checklist whose wording",
          "changes every quarter records nothing over time.", "",
          "| Toolkit | Artefact | What it is for |", "|---|---|---|"]
    for tid, name, lead in tk:
        L.append(f"| {tid} | {name} | {lead[:180]} |")
    L += ["",
          "**On the one appendix the volume plan lists and this volume does not carry.** The plan for this",
          "family also names a consolidated *self-check answers* appendix. It is not printed, and the reason is",
          "worth stating rather than leaving as an apparent omission: each self-check in this volume already",
          "carries its own answer beside the question, so a consolidated set would be a verbatim second copy —",
          "precisely the duplication this programme's editorial rules forbid. The questions are indexed by their",
          "Knowledge Area instead.", "",
          "The *standards and frameworks referenced* register the plan calls for **is** published, as Appendix F.",
          "It states no edition years, deliberately and for a stated reason, and that single outstanding",
          "verification is recorded in `CORPUS_GATE_REPORT.md` rather than glossed over.", "",
          "The consolidated question bank the plan calls for is published separately as `QUESTION_BANK.md`,",
          "and the global glossary as `GLOSSARY.md`; both are generated from the chapters by the same principle",
          "as these appendices.", ""]

    return "\n".join(L).rstrip() + "\n"


def main() -> int:
    check_only = "--check" in sys.argv
    for book in BOOKS:
        text = build(book)
        out = ROOT / book / "APPENDICES.md"
        if not check_only:
            out.write_text(text, encoding="utf-8")
        syms = text.count("\n| `") + text.count("\n| **")
        print(f"{book}: appendices A-E, {len(text.split())} words"
              f"{' (not written, --check)' if check_only else ' -> ' + str(out.relative_to(ROOT.parent))}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
