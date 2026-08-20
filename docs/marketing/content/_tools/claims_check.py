#!/usr/bin/env python3
"""Mechanical claims-register conformance across the run.

The claims register in _BRIEF.md section 3 is the highest-stakes rule in this programme: a
link mistake costs ranking, an unfounded claim costs a certifying body its standing. Most of
it needs judgement, but four things are checkable exactly, and those are the four that have
actually gone wrong before:

  * 15,613 quoted without naming the two volumes it covers. The figure is real for PFL-AI and
    PML-AI; PCL-AI has no equivalent suite, so the number unscoped is simply false.
  * 40/40/20 presented as an examination weighting. It describes the Body of Knowledge's
    proportions. The examination blueprint records its weighting as an open decision.
  * Any other percentage attached to the examination.
  * Accreditation, recognition or endorsement asserted rather than disclaimed.
"""
import json, re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FM_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---\r?\n", re.S)

SCOPED = re.compile(r"PFL-AI|PML-AI")
WEIGHT_4040 = re.compile(r"40\s*/\s*40\s*/\s*20|40[/-]40[/-]20|forty[ /-]forty[ /-]twenty", re.I)

# A weighting claim is a percentage said to be a SHARE OF THE EXAM. Money is not a weighting,
# and this corpus is full of money: fees, discounts, refund tiers, cost variances, margins.
# Requiring an explicit weighting word and excluding the money senses is what separates
# "the exam is 40% finance" from "the examination is USD 500 with a 30% discount".
WEIGHTING_WORD = re.compile(r"\b(weight|weighting|weighted|share|proportion|makes up|made up of|"
                            r"consists? of|comprises?|split|breakdown|allocation)\b", re.I)
EXAM_WORD = re.compile(r"\b(exam|examination|blueprint|question paper)\b", re.I)
MONEY_CTX = re.compile(r"\b(USD|GBP|EUR|\$|£|€|fee|fees|price|pricing|discount|refund|refundable|"
                       r"cost|costs|payment|pay|paid|membership|instal)\w*", re.I)
# A denial is correct handling, not a breach: "what does 40/40/20 mean for the exam? Nothing yet."
DENIAL = re.compile(r"\b(not an exam|nothing yet|is not a weighting|not the exam|no exam weighting|"
                    r"not published|open decision|not yet (?:set|decided|published)|never an exam)\b", re.I)

# An accreditation claim needs PCI or a credential as its SUBJECT. Without that anchor the
# pattern fires on ordinary accounting prose — "revenue is recognised over time", "the invoice
# is approved" — which is the discipline's own vocabulary and appears on nearly every page.
SUBJECT = re.compile(r"\b(PCI|Project Controls Institute|PCL-AI|PFL-AI|PML-AI|the Institute|"
                     r"the credential|the certification|the scheme|the programme|the qualification)\b")
ACCRED_POS = re.compile(r"\b(?:is|are|has been|have been|was|were)\s+(?:fully\s+|formally\s+|"
                        r"officially\s+|internationally\s+|globally\s+)?"
                        r"(accredited|endorsed|recognised as (?:an? )?(?:accredited|approved)|"
                        r"recognized as (?:an? )?(?:accredited|approved)|affiliated with|"
                        r"approved by|certified by)\b", re.I)
ACCRED_NEG = re.compile(r"\b(not|never|no|nor|without|pending|seeking|pursuing|towards|toward|"
                        r"in progress|cannot|neither|yet to be|has yet)\b", re.I)


def sentences(text):
    return re.split(r"(?<=[.!?])\s+", text)


def check(path):
    text = path.read_text(encoding="utf-8")
    m = FM_RE.match(text)
    body = text[m.end():] if m else text
    issues = []

    sents = sentences(body)
    for idx, s in enumerate(sents):
        flat = " ".join(s.split())
        # A rhetorical question carries its answer in the NEXT sentence — "What does 40/40/20 mean
        # for the examination? Nothing yet; the blueprint records it as an open decision." Splitting
        # on the question mark hides the denial from the sentence that needs it, so a question is
        # judged together with what follows it.
        if flat.endswith("?") and idx + 1 < len(sents):
            flat_ctx = flat + " " + " ".join(sents[idx + 1].split())
        else:
            flat_ctx = flat
        if "15,613" in flat or "15613" in flat:
            if not SCOPED.search(flat):
                issues.append(f"15,613 unscoped: \"{flat[:150]}\"")
        low = flat.lower()
        if WEIGHT_4040.search(flat) and EXAM_WORD.search(flat):
            if "body of knowledge" not in low and not DENIAL.search(flat_ctx):
                issues.append(f"40/40/20 tied to the examination: \"{flat[:150]}\"")
        if (re.search(r"\d{1,3}\s*(?:%|per cent|percent)", flat) and EXAM_WORD.search(flat)
                and WEIGHTING_WORD.search(flat) and not MONEY_CTX.search(flat)
                and not DENIAL.search(flat_ctx) and "body of knowledge" not in low):
            issues.append(f"an examination weighting: \"{flat[:150]}\"")
        if ACCRED_POS.search(flat) and SUBJECT.search(flat) and not ACCRED_NEG.search(flat):
            issues.append(f"accreditation/recognition asserted: \"{flat[:150]}\"")

    return {"file": str(path.relative_to(ROOT)), "issues": issues}


def main():
    rows = []
    for t in (sys.argv[1:] or ["flagship", "articles", "social"]):
        for f in sorted((ROOT / t).glob("*.md")):
            if f.name.startswith("_") or f.name == "INDEX.md":
                continue
            rows.append(check(f))
    bad = [r for r in rows if r["issues"]]
    print(f"checked {len(rows)} | clean {len(rows)-len(bad)} | flagged {len(bad)}")
    for r in bad:
        for i in r["issues"]:
            print(f"  {r['file']}\n      {i}")
    (ROOT / "_tools" / "claims_check.json").write_text(json.dumps(rows, indent=1))
    return 1 if bad else 0




def selftest():
    """Calibration check. A claims checker that flags nothing is indistinguishable from one
    that sees nothing, so the two fixtures beside this file pin both ends: every sentence in
    claims_violations.md must be caught, and every sentence in claims_compliant.md must pass.
    The compliant fixture is the harder half — it is full of the accounting senses of
    "recognised" and "approved", exam fees quoted with a discount, and a rhetorical question
    whose denial lands in the following sentence.

    Run:  python3 _tools/claims_check.py --selftest
    """
    here = Path(__file__).resolve().parent / "fixtures"
    bad = check(here / "claims_violations.md")
    good = check(here / "claims_compliant.md")
    ok = len(bad["issues"]) == 5 and len(good["issues"]) == 0
    print(f"selftest: violations caught {len(bad['issues'])}/5, "
          f"false positives {len(good['issues'])}/0 -> {'PASS' if ok else 'FAIL'}")
    for i in good["issues"]:
        print("  false positive:", i[:140])
    for i in bad["issues"]:
        print("  caught:", i[:140])
    return 0 if ok else 1


if __name__ == "__main__":
    sys.exit(selftest() if "--selftest" in sys.argv else main())
