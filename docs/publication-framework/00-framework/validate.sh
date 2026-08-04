#!/usr/bin/env bash
# Validate the PCI publication framework against the rules in
# GOVERNANCE-AND-REVIEW.md §3 and §5.
#
# Usage:  docs/publication-framework/00-framework/validate.sh
# Exit:   0 = all gates pass · 1 = at least one hard failure
#
# Hard failures block publication. Warnings are reported and do not block,
# because a draft is allowed to be unfinished — it is only forbidden to be
# unfinished *and* marked approved.

set -uo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
FRAMEWORK_DIR="$ROOT/00-framework"
FAIL=0
WARN=0

red()   { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }
amber() { printf '\033[33m%s\033[0m\n' "$*"; }
head_() { printf '\n\033[1m%s\033[0m\n' "$*"; }

fail() { red   "  FAIL  $*"; FAIL=$((FAIL + 1)); }
warn() { amber "  WARN  $*"; WARN=$((WARN + 1)); }
pass() { green "  PASS  $*"; }

# Manuscripts are files named PREFIX-NN-slug.md inside a series directory.
manuscripts() {
  find "$ROOT" -type f -name '[A-Z][A-Z][A-Z]-[0-9][0-9]-*.md' | sort
}

MANUSCRIPTS=$(manuscripts)
COUNT=$(printf '%s\n' "$MANUSCRIPTS" | grep -c . || true)

# ---------------------------------------------------------------------------
head_ "1. Document count"
# ---------------------------------------------------------------------------
if [ "$COUNT" -eq 100 ]; then
  pass "100 manuscripts found"
else
  fail "expected 100 manuscripts, found $COUNT"
fi

# ---------------------------------------------------------------------------
head_ "2. Series coverage"
# ---------------------------------------------------------------------------
check_series() {
  local dir="$1" prefix="$2" expected="$3"
  local n
  n=$(find "$ROOT/$dir" -type f -name "$prefix-[0-9][0-9]-*.md" 2>/dev/null | wc -l | tr -d ' ')
  if [ "$n" -eq "$expected" ]; then
    pass "$dir — $n/$expected"
  else
    fail "$dir — $n/$expected ($prefix)"
  fi
}

check_series s01-body-of-knowledge      PCB  6
check_series s02-ai-in-project-controls AIG 12
check_series s03-competency-frameworks  CMP 10
check_series s04-code-of-ethics         ETH  6
check_series s05-certification-handbook CER  8
check_series s06-exam-blueprints        EXB  8
check_series s07-career-roadmap         CAR  8
check_series s08-salary-and-skills      SAL  6
check_series s09-best-practice-guides   BPG 20
check_series s10-free-templates         TPL 16

# ---------------------------------------------------------------------------
head_ "3. Retired credential codes (hard fail — CANONICAL-FACTS.md §1.1)"
# ---------------------------------------------------------------------------
# The canonical-facts and governance files legitimately name the retired codes
# in order to prohibit them; every other file must not.
RETIRED=$(printf '%s\n' "$MANUSCRIPTS" \
  | xargs grep -l -E 'PCP-AI|PFIP|CPMD|PDL-AI' 2>/dev/null || true)
if [ -z "$RETIRED" ]; then
  pass "no retired codes in any manuscript"
else
  while IFS= read -r f; do
    [ -n "$f" ] && fail "retired credential code in ${f#"$ROOT/"}"
  done <<< "$RETIRED"
fi

# ---------------------------------------------------------------------------
head_ "4. Front matter present and schema-shaped"
# ---------------------------------------------------------------------------
MISSING_FM=0
for f in $MANUSCRIPTS; do
  if [ "$(head -n 1 "$f")" != "---" ]; then
    fail "no front matter: ${f#"$ROOT/"}"
    MISSING_FM=$((MISSING_FM + 1))
    continue
  fi
  for field in id series title version status date summary related placeholders; do
    grep -qE "^${field}:" "$f" || {
      fail "missing '${field}:' in ${f#"$ROOT/"}"
      MISSING_FM=$((MISSING_FM + 1))
    }
  done
done
[ "$MISSING_FM" -eq 0 ] && pass "all manuscripts carry schema-shaped front matter"

# ---------------------------------------------------------------------------
head_ "5. ID matches filename and series directory"
# ---------------------------------------------------------------------------
ID_ERRORS=0
for f in $MANUSCRIPTS; do
  base=$(basename "$f")
  file_id="${base%%-*}-$(echo "$base" | cut -d- -f2)"
  decl_id=$(grep -m1 -E '^id:' "$f" | sed -E 's/^id:[[:space:]]*//; s/[[:space:]]*$//')
  if [ "$file_id" != "$decl_id" ]; then
    fail "id '$decl_id' does not match filename '$base'"
    ID_ERRORS=$((ID_ERRORS + 1))
  fi
done
[ "$ID_ERRORS" -eq 0 ] && pass "every declared id matches its filename"

# ---------------------------------------------------------------------------
head_ "6. Duplicate IDs"
# ---------------------------------------------------------------------------
DUPES=$(for f in $MANUSCRIPTS; do basename "$f" | cut -d- -f1,2; done | sort | uniq -d)
if [ -z "$DUPES" ]; then
  pass "all IDs unique"
else
  while IFS= read -r d; do
    [ -n "$d" ] && fail "duplicate id: $d"
  done <<< "$DUPES"
fi

# ---------------------------------------------------------------------------
head_ "7. Cross-reference integrity (related: IDs must exist in the registry)"
# ---------------------------------------------------------------------------
# Resolve against the ASSET REGISTRY, not against the files on disk. The registry is the
# contract: a document may legitimately cite a sibling that has not been authored yet, and
# that is a scheduling fact, not a broken reference. Gates 1 and 2 catch a missing manuscript.
KNOWN=$(grep -oE '\b[A-Z]{3}-[0-9]{2}\b' "$FRAMEWORK_DIR/ASSET-REGISTRY.md" 2>/dev/null | sort -u)
if [ -z "$KNOWN" ]; then
  fail "could not read IDs from ASSET-REGISTRY.md"
  KNOWN=$(for f in $MANUSCRIPTS; do basename "$f" | cut -d- -f1,2; done | sort -u)
fi
BROKEN=0
for f in $MANUSCRIPTS; do
  rel=$(grep -m1 -E '^related:' "$f" | sed -E 's/^related:[[:space:]]*//; s/[][]//g')
  for id in $(echo "$rel" | tr ',' ' '); do
    id=$(echo "$id" | tr -d ' "'"'"'')
    [ -z "$id" ] && continue
    echo "$id" | grep -qE '^[A-Z]{3}-[0-9]{2}$' || continue
    if ! grep -qx "$id" <<< "$KNOWN"; then
      fail "${f#"$ROOT/"} references unknown id '$id'"
      BROKEN=$((BROKEN + 1))
    fi
  done
done
[ "$BROKEN" -eq 0 ] && pass "every related: id resolves to a real manuscript"

# ---------------------------------------------------------------------------
head_ "8. Placeholders vs declared status (approved/published must be clean)"
# ---------------------------------------------------------------------------
PH_ERRORS=0
TOTAL_PH=0
for f in $MANUSCRIPTS; do
  n=$(grep -c 'CONFIRM:' "$f" || true)
  TOTAL_PH=$((TOTAL_PH + n))
  status=$(grep -m1 -E '^status:' "$f" | sed -E 's/^status:[[:space:]]*//; s/[[:space:]]*$//')
  declared=$(grep -m1 -E '^placeholders:' "$f" | sed -E 's/^placeholders:[[:space:]]*//; s/[[:space:]]*$//')
  case "$status" in
    approved|published)
      if [ "$n" -ne 0 ]; then
        fail "${f#"$ROOT/"} is '$status' but has $n unresolved placeholder(s)"
        PH_ERRORS=$((PH_ERRORS + 1))
      fi
      ;;
  esac
  if [ -n "$declared" ] && [ "$declared" != "$n" ]; then
    warn "${f#"$ROOT/"} declares placeholders: $declared but contains $n"
  fi
done
[ "$PH_ERRORS" -eq 0 ] && pass "no approved or published document carries a placeholder"
printf '        %s unresolved placeholder(s) across the framework (drafts may carry these)\n' "$TOTAL_PH"

# ---------------------------------------------------------------------------
head_ "9. Mandatory template sections"
# ---------------------------------------------------------------------------
SECTION_ERRORS=0
for f in $MANUSCRIPTS; do
  grep -q '^\*\*In one paragraph\.\*\*'  "$f" || { warn "${f#"$ROOT/"} missing 'In one paragraph'"; SECTION_ERRORS=$((SECTION_ERRORS+1)); }
  grep -q '^\*\*Who this is for\.\*\*'   "$f" || { warn "${f#"$ROOT/"} missing 'Who this is for'"; SECTION_ERRORS=$((SECTION_ERRORS+1)); }
  grep -q '^## Related'                  "$f" || { warn "${f#"$ROOT/"} missing '## Related'"; SECTION_ERRORS=$((SECTION_ERRORS+1)); }
  grep -q '^## Sources and standards'    "$f" || { warn "${f#"$ROOT/"} missing '## Sources and standards'"; SECTION_ERRORS=$((SECTION_ERRORS+1)); }
  grep -q '^## Status and version'       "$f" || { warn "${f#"$ROOT/"} missing '## Status and version'"; SECTION_ERRORS=$((SECTION_ERRORS+1)); }
done
[ "$SECTION_ERRORS" -eq 0 ] && pass "all manuscripts carry the mandatory template sections"

# S09 and S10 additionally require a worked example and a checklist.
WORKED_ERRORS=0
for f in $(find "$ROOT/s09-best-practice-guides" -name 'BPG-*.md' 2>/dev/null); do
  # Headings are numbered per DOCUMENT-TEMPLATE.md ("## 9. How this goes wrong"), so match
  # the text rather than anchoring on the word immediately after the hashes.
  grep -qiE '^## .*worked example'      "$f" || { warn "${f#"$ROOT/"} missing worked example"; WORKED_ERRORS=$((WORKED_ERRORS+1)); }
  grep -qiE '^## .*checklist'           "$f" || { warn "${f#"$ROOT/"} missing checklist"; WORKED_ERRORS=$((WORKED_ERRORS+1)); }
  grep -qiE '^## .*how this goes wrong' "$f" || { warn "${f#"$ROOT/"} missing 'How this goes wrong'"; WORKED_ERRORS=$((WORKED_ERRORS+1)); }
done
[ "$WORKED_ERRORS" -eq 0 ] && pass "every best-practice guide has a worked example, checklist and failure section"

# ---------------------------------------------------------------------------
head_ "10. Illustrative-figures labelling"
# ---------------------------------------------------------------------------
# A document containing a currency-scale worked number should label it.
LABEL_WARN=0
for f in $MANUSCRIPTS; do
  if grep -qE '[0-9]{1,3},[0-9]{3},[0-9]{3}|[0-9]{1,3},[0-9]{3}' "$f"; then
    grep -qi 'illustrative' "$f" || { warn "${f#"$ROOT/"} has worked figures but no 'illustrative' label"; LABEL_WARN=$((LABEL_WARN+1)); }
  fi
done
[ "$LABEL_WARN" -eq 0 ] && pass "worked figures are labelled illustrative"

# ---------------------------------------------------------------------------
head_ "11. Salary and market data confined to S08 (README.md §4)"
# ---------------------------------------------------------------------------
# S08 itself must contain no figures at all; nowhere else may state pay data.
SAL_HITS=$(find "$ROOT/s08-salary-and-skills" -name 'SAL-*.md' 2>/dev/null \
  | xargs grep -nE '(USD|EUR|GBP|\$|£|€)[[:space:]]*[0-9]{4,}|[0-9]{2,3},[0-9]{3}[[:space:]]*(per annum|a year|salary)' 2>/dev/null || true)
if [ -z "$SAL_HITS" ]; then
  pass "no salary figure appears in S08 (correct — the survey has not been run)"
else
  while IFS= read -r line; do
    [ -n "$line" ] && fail "salary figure in S08: $line"
  done <<< "$SAL_HITS"
fi

# ---------------------------------------------------------------------------
head_ "12. Accreditation language"
# ---------------------------------------------------------------------------
# Sentence-scoped, not line-scoped. A prohibition ("must not describe the credential as
# accredited") and a negation split across a line break ("is not\naccredited by any body")
# both read as claims to a line-based grep, and both are the opposite of a claim.
ACC=$(printf '%s\n' "$MANUSCRIPTS" | python3 -c '
import re, sys
CLAIM = re.compile(r"is accredited|fully accredited|accredited by|government[- ]recognised"
                   r"|guaranteed (job|employment|salary)", re.I)
NEGATED = re.compile(r"\bnot\b|\bnever\b|\bno\b|\bnor\b|\bcannot\b|\bwithout\b|\bprohibit"
                     r"|\bmust not\b|\bdoes not\b|\bmakes no\b|\bimplying\b|\bimply\b", re.I)
for path in (p.strip() for p in sys.stdin if p.strip()):
    text = re.sub(r"\s+", " ", open(path, encoding="utf-8").read())
    for sentence in re.split(r"(?<=[.;:!?])\s+", text):
        if CLAIM.search(sentence) and not NEGATED.search(sentence):
            print(f"{path}: {sentence[:150]}")
' 2>/dev/null || true)
if [ -z "$ACC" ]; then
  pass "no unqualified accreditation or guaranteed-outcome claim"
else
  while IFS= read -r line; do
    [ -n "$line" ] && fail "accreditation claim: $line"
  done <<< "$ACC"
fi

# ---------------------------------------------------------------------------
head_ "13. Trademark symbols in PCI branding"
# ---------------------------------------------------------------------------
TM=$(printf '%s\n' "$MANUSCRIPTS" | xargs grep -ln '™\|®' 2>/dev/null || true)
if [ -z "$TM" ]; then
  pass "no ™/® symbols"
else
  while IFS= read -r f; do
    [ -n "$f" ] && fail "trademark symbol in ${f#"$ROOT/"}"
  done <<< "$TM"
fi

# ---------------------------------------------------------------------------
head_ "Summary"
# ---------------------------------------------------------------------------
printf '  documents: %s\n  failures:  %s\n  warnings:  %s\n' "$COUNT" "$FAIL" "$WARN"

echo
if [ "$FAIL" -gt 0 ]; then
  red "Framework does not pass the publication gate."
  exit 1
fi
green "Framework passes the publication gate."
exit 0
