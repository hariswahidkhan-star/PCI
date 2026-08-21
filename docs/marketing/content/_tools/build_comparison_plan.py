#!/usr/bin/env python3
"""Define the comparison content matrix and emit the worklist.

No external certifying body, awarding organisation or credential is named anywhere in this
plan, and none may appear in the content built from it. Comparisons run at category level —
a category described by what it examines — because that argument is checkable against any
syllabus a reader chooses to open, cannot go stale when somebody else revises a credential,
and carries none of the exposure that naming institutions does.

An earlier version of this file paired PCI's credentials against 24 named credentials. It was
designed in full and rejected. The reasoning is in ../_COMPARISON_FRAMEWORK.md §1.
"""
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent

PCI = {
    "PCL-AI": ("PCI AI Project Controls Leader", "13 domains, 61 knowledge areas"),
    "PFL-AI": ("PCI AI Project Finance Leader", "16 domains, 61 knowledge areas"),
    "PML-AI": ("PCI Project Management Leader – AI", "16 domains, 63 knowledge areas"),
}

# Each category is described by what it examines, at a level of generality no reader could
# dispute and no revision elsewhere can falsify.
CATEGORIES = [
    ("cost-and-scheduling-credentials", "cost and scheduling credentials", "PCL-AI",
     "estimating, cost control, planning, schedule analysis and the economics of the asset life cycle"),
    ("project-controls-certifications", "the established project controls certifications", "PCL-AI",
     "measurement, progress, forecasting and the control of delivery"),
    ("accountancy-and-finance-qualifications", "accountancy and finance qualifications", "PFL-AI",
     "recognition, measurement, disclosure, financial analysis and control"),
    ("audit-credentials", "audit credentials", "PFL-AI",
     "governance, risk, control and the assurance process"),
    ("project-management-certifications", "project management certifications", "PML-AI",
     "managing projects across people, process and the business environment"),
    ("scheduling-software-certifications", "product certifications for scheduling software", "PCL-AI",
     "operating a specific tool rather than the discipline behind it"),
    ("chartered-professional-routes", "chartered routes in surveying and engineering", "suite",
     "assessed professional competence and chartered standing"),
]

# Pure authority content. Names nobody, disputes nobody, and is the strongest thing a new body
# can publish: how to judge any credential, including its own.
EVALUATION = [
    ("what-makes-a-credential-examinable", "What makes a credential examinable"),
    ("how-to-read-a-body-of-knowledge", "How to read a Body of Knowledge before you pay"),
    ("what-accreditation-means", "What accreditation means, and what it does not"),
    ("how-to-verify-a-credential", "How to verify a credential"),
    ("questions-to-ask-a-certifying-body", "Ten questions to ask any certifying body"),
    ("how-a-pass-mark-is-set", "How a pass mark is set, and why it matters"),
    ("separation-of-training-and-certification", "Why preparation and the certification decision are separated"),
    ("what-an-appeal-process-should-look-like", "What an appeal process should look like"),
    ("recertification-and-what-it-is-for", "Recertification, and what it is actually for"),
    ("credential-or-certificate-of-attendance", "How to tell a credential from a certificate of attendance"),
    ("what-a-syllabus-should-tell-you", "What a syllabus should tell you before you enrol"),
    ("judging-a-certifying-body-that-is-new", "How to judge a certifying body that is new"),
    ("scope-versus-prestige", "Scope or prestige: which should decide your choice"),
    ("the-finance-and-delivery-overlap", "The finance and delivery overlap, and who examines it"),
    ("choosing-between-two-credentials", "Choosing between two credentials without a league table"),
]

ROLES = [
    ("planning-engineer", "a planning engineer"),
    ("cost-controller", "a cost controller"),
    ("quantity-surveyor", "a quantity surveyor"),
    ("project-accountant", "a project accountant"),
    ("pmo-lead", "a PMO lead"),
    ("project-controls-manager", "a project controls manager"),
    ("cost-engineer", "a cost engineer"),
    ("scheduler", "a scheduler moving into controls"),
    ("auditor", "an auditor moving into projects"),
    ("finance-manager", "a finance manager on capital projects"),
    ("project-manager", "a project manager who owns the numbers"),
    ("graduate", "a graduate choosing a first credential"),
]

PLATFORMS = [
    ("linkedin-post", "LinkedIn post", "1,300-1,900 characters, no link in body, 3-5 hashtags"),
    ("linkedin-article", "LinkedIn Article", "original, 1,200-1,800 words, never a copy"),
    ("x-thread", "X / Threads", "5-9 posts, 280 characters each, link in the first reply"),
    ("carousel", "LinkedIn carousel", "8-14 slides, 1080x1350, one idea per slide"),
    ("quora", "Quora answer", "300-600 words, answer fully before any link"),
    ("medium", "Medium", "republished under canonical to the credentialfinder original"),
]


def main():
    pieces = []

    for slug, label, pci, scope in CATEGORIES:
        pieces.append({"kind": "category-comparison", "slug": f"{pci.lower()}-and-{slug}",
                       "credential": pci, "category": label, "category_scope": scope,
                       "title": f"{label[0].upper()}{label[1:]}: where {pci} sits",
                       "platform": "Own site — credentialfinder.org", "words": "1600-2400"})

    for slug, title in EVALUATION:
        pieces.append({"kind": "evaluation-guide", "slug": slug, "credential": "suite",
                       "category": None, "title": title,
                       "platform": "Own site — credentialfinder.org", "words": "1200-1800"})

    for slug, who in ROLES:
        pieces.append({"kind": "decision-guide", "slug": f"choosing-a-credential-as-{slug}",
                       "credential": "suite", "category": None, "role": who,
                       "title": f"Choosing a credential as {who}",
                       "platform": "Own site — credentialfinder.org", "words": "1200-1800"})

    core = list(pieces)
    for c in core:
        for pslug, pname, spec in PLATFORMS:
            pieces.append({"kind": "platform-variant", "slug": f"{c['slug']}-{pslug}",
                           "credential": c["credential"], "category": c.get("category"),
                           "title": c["title"], "platform": pname, "spec": spec,
                           "source": c["slug"], "words": "n/a"})

    SIZE = 16
    batches = [pieces[i:i + SIZE] for i in range(0, len(pieces), SIZE)]
    out = {"rule": "Name no external certifying body, awarding organisation or credential, "
                   "anywhere, in any field. Compare categories described by what they examine.",
           "pieces": pieces,
           "batches": [{"id": i + 1, "pieces": b} for i, b in enumerate(batches)],
           "categories": CATEGORIES, "pci": PCI}
    (ROOT / "_tools/comparison_plan.json").write_text(json.dumps(out, indent=1))

    print(f"core pieces       : {len(core)}  ({len(CATEGORIES)} category comparisons, "
          f"{len(EVALUATION)} evaluation guides, {len(ROLES)} decision guides)")
    print(f"platform variants : {len(pieces)-len(core)}  ({len(core)} x {len(PLATFORMS)})")
    print(f"TOTAL             : {len(pieces)} pieces in {len(batches)} batches of <= {SIZE}")


if __name__ == "__main__":
    main()
