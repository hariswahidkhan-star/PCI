#!/usr/bin/env python3
"""Define the comparison content matrix and emit the worklist plus a table of contents.

Every pairing here is one a real candidate would actually weigh. Nothing is paired for the
sake of volume: CIA and CISA sit against PFL-AI because they are audit and finance
credentials, and comparing an audit credential against a scheduling credential would help
nobody and read as padding.
"""
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent

PCI = {
    "PCL-AI": ("PCI AI Project Controls Leader", "13 domains, 61 knowledge areas"),
    "PFL-AI": ("PCI AI Project Finance Leader", "16 domains, 61 knowledge areas"),
    "PML-AI": ("PCI Project Management Leader – AI", "16 domains, 63 knowledge areas"),
}

# (acronym, full name, awarding body, family, the scope line that is true of it)
EXTERNAL = [
    ("CCP", "Certified Cost Professional", "AACE International", "controls",
     "cost engineering across the asset life cycle — estimating, cost control, planning, economic analysis"),
    ("PSP", "Planning and Scheduling Professional", "AACE International", "controls",
     "schedule development, maintenance, analysis and control"),
    ("EVP", "Earned Value Professional", "AACE International", "controls",
     "earned value management systems and their application"),
    ("CEP", "Certified Estimating Professional", "AACE International", "controls",
     "estimating practice across the asset life cycle"),
    ("DRMP", "Decision and Risk Management Professional", "AACE International", "controls",
     "decision analysis and project risk management"),
    ("PMI-SP", "PMI Scheduling Professional", "Project Management Institute", "controls",
     "schedule development and maintenance within the PMI framework"),
    ("PMI-RMP", "PMI Risk Management Professional", "Project Management Institute", "controls",
     "project risk identification, analysis and response"),
    ("RICS", "RICS professional routes (project and quantity surveying)", "RICS", "controls",
     "surveying practice, professional ethics and assessed competence"),
    ("P6", "Oracle Primavera P6 product certification", "Oracle", "controls",
     "operating a specific scheduling product"),
    ("CFA", "Chartered Financial Analyst", "CFA Institute", "finance",
     "investment analysis, valuation, portfolio management and ethics"),
    ("CMA", "Certified Management Accountant", "IMA", "finance",
     "management accounting, planning, analysis, control and decision support"),
    ("ACCA", "ACCA qualification", "ACCA", "finance",
     "financial reporting, audit, tax and management accounting"),
    ("CIMA", "CIMA professional qualification", "CIMA", "finance",
     "management accounting and business strategy"),
    ("CPA", "Certified Public Accountant", "state boards / AICPA", "finance",
     "accounting, audit, regulation and business environment"),
    ("CIA", "Certified Internal Auditor", "The Institute of Internal Auditors", "finance",
     "internal auditing across governance, risk and control"),
    ("CISA", "Certified Information Systems Auditor", "ISACA", "finance",
     "information systems auditing, IT governance and the protection of information assets"),
    ("CTP", "Certified Treasury Professional", "AFP", "finance",
     "corporate treasury, liquidity and working capital"),
    ("PMP", "Project Management Professional", "Project Management Institute", "pm",
     "managing projects across people, process and the business environment"),
    ("CAPM", "Certified Associate in Project Management", "Project Management Institute", "pm",
     "project management fundamentals for those early in the discipline"),
    ("PRINCE2", "PRINCE2 Practitioner", "PeopleCert", "pm",
     "a defined project method with named processes, themes and principles"),
    ("APM-PMQ", "APM Project Management Qualification", "Association for Project Management", "pm",
     "project management knowledge against the APM Body of Knowledge"),
    ("ChPP", "Chartered Project Professional", "Association for Project Management", "pm",
     "assessed professional competence and chartered standing"),
    ("IPMA", "IPMA four-level certification", "IPMA", "pm",
     "competence assessed at four levels against a competence baseline"),
    ("PMI-ACP", "PMI Agile Certified Practitioner", "Project Management Institute", "pm",
     "agile principles and practice across several approaches"),
]

FAMILY_TO_PCI = {"controls": "PCL-AI", "finance": "PFL-AI", "pm": "PML-AI"}

PILLARS = [
    ("pcl-ai-vs-aace-certifications", "PCL-AI", ["CCP", "PSP", "EVP", "CEP", "DRMP"],
     "PCL-AI vs the AACE certifications: what each examines"),
    ("pcl-ai-vs-pmi-scheduling-risk", "PCL-AI", ["PMI-SP", "PMI-RMP"],
     "PCL-AI vs PMI-SP and PMI-RMP: scope compared"),
    ("pfl-ai-vs-finance-certifications", "PFL-AI", ["CFA", "CMA", "ACCA", "CIMA", "CPA"],
     "PFL-AI vs CFA, CMA, ACCA and CIMA: what each examines"),
    ("pfl-ai-vs-audit-certifications", "PFL-AI", ["CIA", "CISA"],
     "PFL-AI vs CIA and CISA: audit and project finance compared"),
    ("pml-ai-vs-pmp", "PML-AI", ["PMP", "CAPM"],
     "PML-AI vs PMP and CAPM: what each examines"),
    ("pml-ai-vs-prince2-apm-ipma", "PML-AI", ["PRINCE2", "APM-PMQ", "ChPP", "IPMA"],
     "PML-AI vs PRINCE2, APM and IPMA: scope compared"),
    ("pci-credentials-vs-all", "suite", [e[0] for e in EXTERNAL],
     "The PCI credentials compared with every major alternative"),
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
    ("internal-auditor", "an internal auditor moving into projects"),
    ("finance-manager", "a finance manager on capital projects"),
    ("project-manager", "a project manager who owns the numbers"),
    ("graduate", "a graduate choosing a first credential"),
]

PLATFORMS = [
    ("linkedin-post", "LinkedIn post", "1,300-1,900 characters, no link in body, 3-5 hashtags"),
    ("linkedin-article", "LinkedIn Article", "original, 1,200-1,800 words, never a copy"),
    ("x-thread", "X / Threads", "5-9 posts, 280 chars each, link in first reply"),
    ("carousel", "LinkedIn carousel", "8-14 slides, 1080x1350, one idea per slide"),
    ("quora", "Quora answer", "300-600 words, answer fully before any link"),
    ("medium", "Medium", "republished under canonical to the credentialfinder original"),
]


def main():
    pieces = []

    for slug, cred, against, title in PILLARS:
        pieces.append({"kind": "pillar", "slug": slug, "credential": cred,
                       "against": against, "title": title,
                       "platform": "Own site — credentialfinder.org", "words": "1800-2600"})

    for acr, name, body, fam, scope in EXTERNAL:
        pci = FAMILY_TO_PCI[fam]
        pieces.append({"kind": "head-to-head", "slug": f"{pci.lower()}-vs-{acr.lower()}",
                       "credential": pci, "against": [acr],
                       "title": f"{pci} vs {acr}: what each one examines",
                       "platform": "Own site — credentialfinder.org", "words": "1200-1800",
                       "external_name": name, "external_body": body, "external_scope": scope})

    for slug, who in ROLES:
        pieces.append({"kind": "decision-guide", "slug": f"best-certification-for-{slug}",
                       "credential": "suite", "against": [],
                       "title": f"Which certification suits {who}?",
                       "platform": "Own site — credentialfinder.org", "words": "1200-1800",
                       "role": who})

    core = list(pieces)
    for c in core:
        for pslug, pname, spec in PLATFORMS:
            pieces.append({"kind": "platform-variant", "slug": f"{c['slug']}-{pslug}",
                           "credential": c["credential"], "against": c["against"],
                           "title": c["title"], "platform": pname, "spec": spec,
                           "source": c["slug"], "words": "n/a"})

    SIZE = 16
    batches = [pieces[i:i + SIZE] for i in range(0, len(pieces), SIZE)]
    out = {"pieces": pieces, "batches": [{"id": i + 1, "pieces": b} for i, b in enumerate(batches)],
           "external": EXTERNAL, "pci": PCI}
    (ROOT / "_tools/comparison_plan.json").write_text(json.dumps(out, indent=1))

    print(f"core pieces        : {len(core)}  ({len(PILLARS)} pillars, {len(EXTERNAL)} head-to-heads, {len(ROLES)} decision guides)")
    print(f"platform variants  : {len(pieces)-len(core)}  ({len(core)} x {len(PLATFORMS)} platforms)")
    print(f"TOTAL              : {len(pieces)} pieces in {len(batches)} batches of <= {SIZE}")


if __name__ == "__main__":
    main()
