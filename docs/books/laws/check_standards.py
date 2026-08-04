#!/usr/bin/env python3
"""check_standards.py — reference-integrity and drafting-conformance gate for the PCI Standards.

Plain Python 3, no third-party imports. Run from anywhere:

    python3 docs/books/laws/check_standards.py

Exits 0 when every check passes, 1 otherwise. Checks, in the order reported:

  1. Duplicate standard identifiers.
  2. Manual §5 structure — all twenty-five elements present, in order, correctly named.
  3. Foundational citations — every `PCI-FND-STD-NN` resolves to a published foundational standard.
  4. Certification citations — every `PCI-<CRED>-STD-DD.NN` resolves, in either direction.
  5. Process-requirement citations — every `<parent>-PR-NN` resolves to a defined requirement.
  6. Manual §1 normative language — no `shall` inside a standard element or a process requirement.
     Front matter that names the word in order to explain the convention is permitted (Manual §1).
  7. Anchor domains — within each credential's Body of Knowledge range.
  8. Concordance currency — STANDARDS_CONCORDANCE.md §1 and §2 match the published standard files.

What this cannot check is the defect it was written after: a citation whose number resolves but whose
subject is not the one the citing sentence describes. Only reading the sentence catches that. The
concordance exists so that a human can scan the mapping in one place.
"""

import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))

FOUNDATIONAL = 'PCI_FOUNDATIONAL_STANDARDS.md'
CERT_FILES = {'PCL': 'PCL_AI_STANDARDS.md', 'PFL': 'PFL_AI_STANDARDS.md', 'PML': 'PML_AI_STANDARDS.md'}
CONCORDANCE = 'STANDARDS_CONCORDANCE.md'
STANDARD_FILES = [FOUNDATIONAL] + [CERT_FILES[c] for c in ('PCL', 'PFL', 'PML')]

# Manual §5 — the twenty-five elements, in the order they must appear.
ELEMENTS = [
    'Normative requirement', 'Purpose', 'Scope', 'Defined terms', 'Required actions',
    'Prohibited actions', 'Required evidence', 'Responsible role', 'Approval authority',
    'Independence requirement', 'Materiality or threshold', 'Exception and waiver',
    'Escalation trigger', 'AI application', 'AI prohibition', 'AI verification',
    'External reference', 'Jurisdictional caution', 'Related PCI Standards',
    'Related Body of Knowledge content', 'Compliance test', 'Breach indicators',
    'Consequence within PCI authority', 'Examination application', 'Version and status',
]

# Body of Knowledge domain count per credential — an anchor domain outside it cannot exist.
DOMAIN_LIMIT = {'PCL': 13, 'PFL': 16, 'PML': 16}

FND_ID = re.compile(r'PCI-FND-STD-(\d{2})(?![\d])')
CERT_ID = re.compile(r'PCI-(PCL|PFL|PML)-STD-(\d{2})\.(\d{2})(?![\d])')
PR_ID = re.compile(r'PCI-[A-Z]{3}-STD-[\d.]+-PR-\d{2}')
PR_DEF = re.compile(r'^(?:-\s+)?\*\*`?(PCI-[A-Z]{3}-STD-[\d.]+-PR-\d{2})`?\s*(?:—|\.)', re.M)
HEADING = re.compile(r'^(#{2,6})\s+(.*)$', re.M)
STANDARD_HEADING = re.compile(r'^(#{2,6})\s+PCI STANDARD\s+(\S+)\s+—\s+(.*)$', re.M)
ELEMENT_MARK = re.compile(r'^\*\*(\d{1,2})\.\s+([^*]+?)\.\*\*', re.M)
SHALL = re.compile(r'\bshall\b', re.I)


class Report:
    def __init__(self):
        self.errors = []
        self.warnings = []
        self.lines = []

    def check(self, name, failures, note=''):
        if failures:
            self.errors.extend('%s: %s' % (name, f) for f in failures)
            self.lines.append('  FAIL  %-50s %d problem%s' %
                              (name, len(failures), '' if len(failures) == 1 else 's'))
            for f in failures[:40]:
                self.lines.append('          - %s' % f)
            if len(failures) > 40:
                self.lines.append('          … and %d more' % (len(failures) - 40))
        else:
            self.lines.append('  ok    %-50s %s' % (name, note))

    def warn(self, name, items):
        for i in items:
            self.warnings.append('%s: %s' % (name, i))
            self.lines.append('  note  %-50s %s' % (name, i))


def line_of(text, pos):
    return text.count('\n', 0, pos) + 1


def read(fname):
    with open(os.path.join(HERE, fname), encoding='utf-8') as fh:
        return fh.read()


def parse_standards(fname, text):
    """Return (standards, front_matter_end). A standard's region runs from its heading to the next
    standard heading, or to the next heading at the same or higher level that is not one."""
    headings = []
    for m in HEADING.finditer(text):
        level = len(m.group(1))
        lm = STANDARD_HEADING.match(m.group(0))
        headings.append({
            'pos': m.start(), 'end': m.end(), 'level': level,
            'standard': (lm.group(2), lm.group(3).strip()) if lm else None,
        })
    standards = []
    for i, h in enumerate(headings):
        if not h['standard']:
            continue
        stop = len(text)
        for j in range(i + 1, len(headings)):
            n = headings[j]
            if n['standard'] or n['level'] <= h['level']:
                stop = n['pos']
                break
        standards.append({
            'file': fname, 'id': h['standard'][0], 'title': h['standard'][1],
            'start': h['pos'], 'stop': stop, 'body': text[h['end']:stop],
            'line': line_of(text, h['pos']),
        })
    front_end = standards[0]['start'] if standards else len(text)
    return standards, front_end


def main():
    rep = Report()
    texts = {f: read(f) for f in STANDARD_FILES}
    standards = []
    fronts = {}
    for f in STANDARD_FILES:
        got, front_end = parse_standards(f, texts[f])
        standards.extend(got)
        fronts[f] = front_end

    print('PCI Standards — reference-integrity check')
    print('=' * 78)
    print('Files: %s' % ', '.join(STANDARD_FILES))
    print('Standards parsed: %d (%s)' % (
        len(standards), ', '.join('%s %d' % (f.split('_')[0], sum(1 for l in standards if l['file'] == f))
                                  for f in STANDARD_FILES)))
    print()

    # ---------------------------------------------------------------- 1. duplicates
    seen = {}
    dup = []
    for l in standards:
        if l['id'] in seen:
            dup.append('`%s` defined twice — %s:%d and %s:%d'
                       % (l['id'], seen[l['id']]['file'], seen[l['id']]['line'], l['file'], l['line']))
        else:
            seen[l['id']] = l
    pr_defs = {}
    for f in STANDARD_FILES:
        for m in PR_DEF.finditer(texts[f]):
            pid = m.group(1)
            if pid in pr_defs:
                dup.append('`%s` defined twice — %s:%d and %s'
                           % (pid, f, line_of(texts[f], m.start()), pr_defs[pid]))
            else:
                pr_defs[pid] = '%s:%d' % (f, line_of(texts[f], m.start()))
    rep.check('duplicate identifiers', dup,
              '%d standards, %d process requirements, all unique' % (len(seen), len(pr_defs)))

    # ---------------------------------------------------------------- 2. structure
    struct = []
    for l in standards:
        found = ELEMENT_MARK.findall(l['body'])
        nums = [int(n) for n, _ in found]
        names = [n.strip() for _, n in found]
        if nums != list(range(1, 26)):
            missing = [i for i in range(1, 26) if i not in nums]
            if missing:
                struct.append('`%s` (%s) is missing element%s %s'
                              % (l['id'], l['file'], '' if len(missing) == 1 else 's',
                                 ', '.join(str(m) for m in missing)))
            else:
                struct.append('`%s` (%s) has its elements out of order: %s'
                              % (l['id'], l['file'], nums))
            continue
        for i, want in enumerate(ELEMENTS):
            got = names[i]
            if got != want and not got.startswith(want):
                struct.append('`%s` (%s) element %d is named "%s", expected "%s"'
                              % (l['id'], l['file'], i + 1, got, want))
    rep.check('twenty-five elements, in Manual §5 order', struct,
              'all %d standards carry every element in order' % len(standards))

    # ---------------------------------------------------------------- 3. foundational citations
    fnd_ids = {l['id'][-2:] for l in standards if l['id'].startswith('PCI-FND-STD-')}
    bad_fnd = []
    n_fnd = 0
    for f in STANDARD_FILES + [CONCORDANCE]:
        text = texts.get(f) or read(f)
        for m in FND_ID.finditer(text):
            n_fnd += 1
            if m.group(1) not in fnd_ids:
                bad_fnd.append('%s:%d cites `PCI-FND-STD-%s`, which does not exist'
                               % (f, line_of(text, m.start()), m.group(1)))
    rep.check('foundational citations resolve', bad_fnd,
              '%d citations, all within `PCI-FND-STD-01`–`PCI-FND-STD-%s`'
              % (n_fnd, max(fnd_ids)))

    # ---------------------------------------------------------------- 4. certification citations
    cert_ids = {l['id'] for l in standards if not l['id'].startswith('PCI-FND-STD-')}
    bad_cert = []
    n_cert = 0
    for f in STANDARD_FILES + [CONCORDANCE]:
        text = texts.get(f) or read(f)
        for m in CERT_ID.finditer(text):
            n_cert += 1
            if m.group(0) not in cert_ids:
                bad_cert.append('%s:%d cites `%s`, which is not a published standard'
                                % (f, line_of(text, m.start()), m.group(0)))
    rep.check('certification-standard citations resolve', bad_cert,
              '%d citations against %d published standards' % (n_cert, len(cert_ids)))

    # ---------------------------------------------------------------- 5. process requirements
    bad_pr = []
    n_pr = 0
    for f in STANDARD_FILES + [CONCORDANCE]:
        text = texts.get(f) or read(f)
        for m in PR_ID.finditer(text):
            n_pr += 1
            pid = m.group(0)
            if pid in pr_defs:
                continue
            # A range such as `-PR-01` to `-PR-03` is written out in full at both ends only.
            bad_pr.append('%s:%d cites `%s`, which is not defined'
                          % (f, line_of(text, m.start()), pid))
    rep.check('process-requirement citations resolve', bad_pr,
              '%d citations against %d defined requirements' % (n_pr, len(pr_defs)))

    # ---------------------------------------------------------------- 6. `shall`
    shall_err, shall_note = [], []
    for f in STANDARD_FILES:
        text = texts[f]
        spans = [(l['start'], l['stop'], l['id']) for l in standards if l['file'] == f]
        for m in SHALL.finditer(text):
            ln = line_of(text, m.start())
            inside = next((lid for a, b, lid in spans if a <= m.start() < b), None)
            if inside:
                shall_err.append('%s:%d uses "%s" inside `%s`'
                                 % (f, ln, m.group(0), inside))
            elif m.start() < fronts[f]:
                shall_note.append('%s:%d names "%s" in front matter — permitted where it explains '
                                  'the convention (Manual §1)' % (f, ln, m.group(0)))
            else:
                shall_note.append('%s:%d names "%s" outside any standard — check it is explanatory'
                                  % (f, ln, m.group(0)))
    rep.check('no `shall` in a standard element or process requirement', shall_err,
              'zero occurrences inside any of the %d standards' % len(standards))
    rep.warn('`shall` outside a standard', shall_note)

    # ---------------------------------------------------------------- 7. anchor domains
    bad_dom = []
    for l in standards:
        m = CERT_ID.match(l['id'])
        if not m:
            continue
        cred, dd = m.group(1), int(m.group(2))
        if not 1 <= dd <= DOMAIN_LIMIT[cred]:
            bad_dom.append('`%s` anchors to domain %d; %s-AI has domains 1–%d'
                           % (l['id'], dd, cred, DOMAIN_LIMIT[cred]))
    rep.check('anchor domains within the credential range', bad_dom,
              'PCL ≤ %d, PFL ≤ %d, PML ≤ %d' % (DOMAIN_LIMIT['PCL'], DOMAIN_LIMIT['PFL'],
                                                DOMAIN_LIMIT['PML']))

    # ---------------------------------------------------------------- 8. concordance currency
    conc_problems = []
    try:
        conc = read(CONCORDANCE)
    except OSError as exc:
        conc_problems.append('cannot be read: %s' % exc)
        conc = ''

    if conc:
        # §1 — subjects must match the foundational titles.
        titles = {l['id'][-2:]: l['title'] for l in standards if l['id'].startswith('PCI-FND-STD-')}
        for n, subject in re.findall(r'^\| `PCI-FND-STD-(\d\d)` \| ([^|]+?) \|', conc, re.M):
            want = titles.get(n)
            if want and subject.strip() != want:
                conc_problems.append('§1 gives `PCI-FND-STD-%s` the subject "%s"; the standard is '
                                     'titled "%s"' % (n, subject.strip(), want))
        # §2 — the citation map must match what the standard files actually say.
        actual = {n: {c: [] for c in CERT_FILES} for n in titles}
        for cred, fname in CERT_FILES.items():
            for l in [x for x in standards if x['file'] == fname]:
                for n in sorted(set(FND_ID.findall(l['body']))):
                    if n in actual:
                        actual[n][cred].append(l['id'])
        body = conc.split('## 2. Which certification standards cite which foundational standard', 1)
        published = {}
        if len(body) == 2:
            for row in re.findall(r'^\| `PCI-FND-STD-(\d\d)` \|([^\n]*)$', body[1], re.M):
                n, rest = row
                cells = [c.strip() for c in rest.split('|')]
                cells = cells[1:4] if len(cells) >= 4 else []
                published[n] = {cred: CERT_ID.findall(cells[i]) and
                                [m.group(0) for m in CERT_ID.finditer(cells[i])] or []
                                for i, cred in enumerate(('PCL', 'PFL', 'PML'))} if cells else {}
        else:
            conc_problems.append('§2 heading not found')
        for n in sorted(titles):
            if n not in published:
                conc_problems.append('§2 has no row for `PCI-FND-STD-%s`' % n)
                continue
            for cred in ('PCL', 'PFL', 'PML'):
                pub = published[n].get(cred, [])
                act = actual[n][cred]
                if pub != act:
                    missing = [x for x in act if x not in pub]
                    extra = [x for x in pub if x not in act]
                    conc_problems.append(
                        '§2 row `PCI-FND-STD-%s` / %s-AI is out of date%s%s'
                        % (n, cred,
                           ' — missing ' + ', '.join('`%s`' % x for x in missing) if missing else '',
                           ' — lists ' + ', '.join('`%s`' % x for x in extra) +
                           ' which no longer cites it' if extra else ''))
    rep.check('STANDARDS_CONCORDANCE.md matches the standard files', conc_problems,
              'subjects and citation map current')

    # ---------------------------------------------------------------- report
    for line in rep.lines:
        print(line)
    print()
    print('-' * 78)
    if rep.errors:
        print('FAILED — %d problem%s found%s.'
              % (len(rep.errors), '' if len(rep.errors) == 1 else 's',
                 ', %d note%s' % (len(rep.warnings), '' if len(rep.warnings) == 1 else 's')
                 if rep.warnings else ''))
        return 1
    print('PASSED — %d standards, %d process requirements, %d foundational citations and %d '
          'certification citations all resolve.' % (len(standards), len(pr_defs), n_fnd, n_cert))
    if rep.warnings:
        print('         %d note%s recorded above.'
              % (len(rep.warnings), '' if len(rep.warnings) == 1 else 's'))
    return 0


if __name__ == '__main__':
    sys.exit(main())
