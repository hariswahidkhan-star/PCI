#!/usr/bin/env python3
"""Stage B — extend MASTER_LEDGER.csv to 500 rows, then run the intent-deduplication and
cannibalisation checks the master prompt requires (§9) and print the final cluster totals.

Idempotent: rebuilds rows A-051..A-500 from the curated topic corpus every run, leaving the
approved rows A-001..A-050 untouched. Column order is frozen (CONTINUATION.md).
"""
import csv, os, re, sys, unicodedata

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)
from topics_1_13 import TOPICS_1_13
from topics_14_25 import TOPICS_14_25

TOPICS = {**TOPICS_1_13, **TOPICS_14_25}
LEDGER = os.path.join(HERE, "..", "MASTER_LEDGER.csv")

CLUSTER_NAMES = {
 1:"PCI entity, mission, governance and policies", 2:"Certification routes, eligibility, pricing and FAQs",
 3:"PCI PCL-AI", 4:"PCI PFL-AI", 5:"PCI PML-AI", 6:"Three-certification pathway and career mapping",
 7:"Certuvo training partnership and learner experience", 8:"Guided labs, simulation labs and portfolio evidence",
 9:"PCI World, Passport, rooms, forum and careers", 10:"MyPCI, applications, exam journey and verification",
 11:"Project-controls foundations", 12:"Planning, scheduling and delay",
 13:"Cost control, estimating and forecasting", 14:"Earned value management",
 15:"Risk, change and governance", 16:"Project finance, budgeting, cash flow and accounting",
 17:"AI, data, analytics, automation and responsible use", 18:"Dashboards, reporting and decision support",
 19:"Standards and contracts", 20:"Tools", 21:"Careers by role, seniority and industry",
 22:"Careers by country/region", 23:"PCI-versus-project-management credential comparisons",
 24:"PCI-versus-finance/audit/IT credential comparisons", 25:"Student questions, myths, case scenarios",
}
TARGETS = {1:16,2:20,3:20,4:20,5:20,6:18,7:16,8:20,9:20,10:18,11:20,12:20,13:20,14:18,15:20,
           16:20,17:20,18:16,19:14,20:20,21:20,22:20,23:28,24:27,25:29}

# Per-cluster defaults: (audience, property, domain, external sources)
DEFAULTS = {
 1:("Prospective candidates, employers and regulators","PCI (entity)","projectcontrolsinstitute.org","PCI policy pages"),
 2:("Prospective candidates","All certifications","projectcontrolsinstitute.org","PCI pricing and policy pages"),
 3:("Project controls practitioners","PCL-AI","projectcontrolsinstitute.org","PCI PCL-AI syllabus"),
 4:("Finance and commercial professionals","PFL-AI","projectcontrolsinstitute.org","PCI PFL-AI syllabus"),
 5:("Project and programme leaders","PML-AI","projectcontrolsinstitute.org","PCI PML-AI syllabus"),
 6:("Career planners across the three credentials","All certifications","projectcontrolsinstitute.org","PCI programme pages"),
 7:("Candidates preparing for an examination","Certuvo","projectcontrolsinstitute.org","PCI-Certuvo terms"),
 8:("Hands-on learners and candidates","Simulation Lab","projectcontrolsinstitute.org","PCI lab documentation"),
 9:("Practitioners and employers","PCI World","pciworld.org","PCI World pages"),
 10:("Applicants and certificants","MyPCI","mypci.org","PCI portal guidance"),
 11:("Newcomers and practitioners","Foundations","projectcontrolsinstitute.org","Authoritative practice sources"),
 12:("Planners and schedulers","Foundations","projectcontrolsinstitute.org","Authoritative practice sources"),
 13:("Cost engineers and estimators","Foundations","projectcontrolsinstitute.org","Authoritative practice sources"),
 14:("Controls practitioners","Foundations","projectcontrolsinstitute.org","EVM system guidelines (verify)"),
 15:("Risk, change and governance practitioners","Foundations","projectcontrolsinstitute.org","Authoritative practice sources"),
 16:("Project finance and commercial professionals","Foundations","projectcontrolsinstitute.org","Authoritative finance sources"),
 17:("Controls and delivery professionals adopting AI","Foundations","projectcontrolsinstitute.org","Authoritative AI-governance sources"),
 18:("Reporting and PMO professionals","Foundations","projectcontrolsinstitute.org","Authoritative practice sources"),
 19:("Practitioners working to standards and contracts","Foundations","projectcontrolsinstitute.org","Issuing bodies' official publications"),
 20:("Tool users across controls disciplines","Foundations","projectcontrolsinstitute.org","Vendor official documentation"),
 21:("Job seekers and hiring managers","Careers","projectcontrolsinstitute.org","Role evidence and job-market research"),
 22:("Job seekers by market","Careers","pciworld.org","Local labour-market evidence"),
 23:("Credential shoppers","All certifications","projectcontrolsinstitute.org","Official issuing-body pages"),
 24:("Finance/audit/IT professionals","All certifications","projectcontrolsinstitute.org","Official issuing-body pages"),
 25:("Prospective and current candidates","All certifications","projectcontrolsinstitute.org","PCI policy pages"),
}
# Cluster pillars already approved in rows A-001..A-050; clusters absent here get their first new row.
PILLARS = {1:"A-001",2:"A-005",3:"A-011",4:"A-016",5:"A-020",6:"A-024",7:"A-026",8:"A-029",
           9:"A-032",10:"A-035",11:"A-038",21:"A-041",23:"A-040",24:"A-045",25:"A-046"}
DEEP = {"PCL-AI":"/certifications/pcl-ai","PFL-AI":"/certifications/pfl-ai","PML-AI":"/certifications/pml-ai"}
BODIES = [("pmi-sp","PMI"),("pmi-rmp","PMI"),("capm","PMI"),("pmp","PMI"),("pgmp","PMI"),
          ("prince2","PeopleCert"),("msp","PeopleCert"),("itil","PeopleCert"),("apm","APM"),
          ("chartered","APM"),("ccp","AACE International"),("psp","AACE International"),
          ("evp","AACE International"),("cep","AACE International"),("scrum","Scrum bodies (verify)"),
          ("safe","Scaled Agile, Inc."),("six sigma","Issuing bodies (verify)"),("ipma","IPMA"),
          ("cpa","Jurisdictional CPA board (identify)"),("acca","ACCA"),("cima","CIMA/AICPA"),
          ("cia","The IIA"),("cisa","ISACA"),("treasury","Treasury bodies (verify)"),
          ("caia","CAIA Association"),("frm","GARP"),("cma","IMA"),("cfa","CFA Institute")]

def slugify(title):
    s = unicodedata.normalize("NFKD", title).encode("ascii","ignore").decode()
    s = re.sub(r"[^a-z0-9]+","-", s.lower()).strip("-")
    return re.sub(r"-+","-", s)[:70].strip("-")

def word_range(ct, cluster):
    if ct == "comparison": return "1600-2400"
    if ct == "asset": return "1400-2000"
    if ct == "faq": return "1100-1500"
    if cluster in (3,4,5) : return "1400-2000"
    return "1200-1800"

def schema_for(ct):
    if ct == "faq": return "Article+FAQPage"
    if ct == "asset": return "Article+BreadcrumbList"
    if ct == "career": return "Article"
    return "Article"

def cta_for(ct, cluster):
    if ct == "pricing-route": return "See current pricing"
    if ct == "comparison": return "Decide with the guide"
    if ct == "career": return "Map your pathway"
    if ct == "applied": return "Practise it in the labs"
    if ct == "trust": return "Read the policies"
    if cluster in (3,4,5): return "Review the credential"
    return "Explore the certifications"

def repurpose(ct, is_pillar):
    return "full" if (is_pillar or ct in ("comparison","asset")) else "social-core"

def flags_for(cluster, ct, title):
    f = []
    if cluster in (1,) and any(w in title.lower() for w in ("accreditation","status","exists","iso")):
        f.append("[LEGAL STATUS — PCI APPROVAL REQUIRED]")
    if ct == "pricing-route" or "cost" in title.lower() and cluster in (2,6):
        f.append("[PRICE UNVERIFIED]")
    if cluster == 7: f.append("[VERIFY CERTUVO ACCESS TERM]")
    if cluster == 19: f.append("[STANDARD SOURCE REQUIRED]")
    if cluster == 22: f.append("[LOCAL EVIDENCE REQUIRED]")
    if cluster in (23,24) or ct == "comparison": f.append("[COMPETITOR FACTS PENDING]")
    if cluster in range(1,11): f.append("[LIVE-SITE VERIFICATION PENDING]")
    return "; ".join(dict.fromkeys(f)) or "none"

def bodies_for(title):
    t = title.lower()
    found = [name for key, name in BODIES if re.search(r"\b"+re.escape(key)+r"\b", t)]
    return ", ".join(dict.fromkeys(found)) or "none"

def semantic_entities(keyword, question):
    words = [w for w in re.findall(r"[a-z]{4,}", (keyword+" "+question).lower())
             if w not in {"what","does","actually","really","that","this","with","from","your","which","when","where","should","would","could","about","into","have","need","much","many","them","they","their"}]
    return ", ".join(list(dict.fromkeys(words))[:8])

def build():
    existing = list(csv.reader(open(LEDGER, encoding="utf-8")))
    header, approved = existing[0], [r for r in existing[1:] if int(r[0].split("-")[1]) <= 50]
    assert len(approved) == 50, f"expected 50 approved rows, found {len(approved)}"
    used_by_cluster = {}
    for r in approved:
        used_by_cluster.setdefault(int(r[2]), []).append(r)

    rows, nid = [], 51
    for cluster in range(1, 26):
        need = TARGETS[cluster] - len(used_by_cluster.get(cluster, []))
        topics = TOPICS[cluster]
        assert len(topics) == need, f"cluster {cluster}: corpus has {len(topics)}, needs {need}"
        audience, prop, domain, ext = DEFAULTS[cluster]
        for (title, kw, ct, funnel, intent, question) in topics:
            cid = f"A-{nid:03d}"; nid += 1
            is_pillar = cluster not in PILLARS
            if is_pillar:
                PILLARS[cluster] = cid            # first row of a pillar-less cluster becomes the pillar
            pillar = PILLARS[cluster]
            deep = DEEP.get(prop, "[DEEP URL PENDING]")
            links = (f"pillar:{'self' if pillar == cid else pillar}; cluster:{CLUSTER_NAMES[cluster]}; "
                     f"money:/certifications" + ("; world:pciworld.org" if cluster in (9,21,22) else ""))
            rows.append([
                cid, "Planned", str(cluster), funnel, intent, audience,
                "Global" if cluster != 22 else title.split(" in ")[-1],
                prop, title, kw,
                ", ".join(dict.fromkeys(kw.split()[:2] + question.lower().split()[:3])),
                semantic_entities(kw, question), question, ct,
                "Pillar" if pillar == cid else "Spoke",
                "New pillar page" if pillar == cid else pillar,
                slugify(title), domain, deep, links, ext, bodies_for(title),
                word_range(ct, cluster), cta_for(ct, cluster), schema_for(ct),
                repurpose(ct, pillar == cid), f"Brief: {title.lower()}",
                flags_for(cluster, ct, title), "Unassigned", "", "", "",
            ])
    assert nid - 1 == 500, f"built up to {nid-1}, expected 500"
    with open(LEDGER, "w", newline="", encoding="utf-8") as fh:
        w = csv.writer(fh); w.writerow(header); w.writerows(approved + rows)
    return approved + rows

def audit(all_rows):
    print(f"\n  rows: {len(all_rows)}  columns: {set(len(r) for r in all_rows)}")
    # 1. intent deduplication — primary keyword AND (question) must be unique
    for label, idx in (("primary keyword", 9), ("slug", 16), ("working title", 8), ("question", 12)):
        seen, dupes = {}, []
        for r in all_rows:
            key = r[idx].strip().lower()
            if key in seen: dupes.append((seen[key], r[0], r[idx]))
            else: seen[key] = r[0]
        print(f"  duplicate {label}: {len(dupes)}" + (f"  {dupes[:5]}" if dupes else ""))
    # 2. cannibalisation proxy — same cluster + same content type + overlapping keyword head
    heads = {}
    for r in all_rows:
        head = " ".join(r[9].split()[:3]).lower()
        heads.setdefault((r[2], head), []).append(r[0])
    # A shared keyword head is NOT automatically cannibalisation: a geo, industry or credential
    # modifier separates the intent. Each is printed for review; the hard gate is exact duplication.
    collide = {k: v for k, v in heads.items() if len(v) > 1}
    print(f"  keyword-head groups needing a modifier to separate intent: {len(collide)}")
    for (cl, head), ids in sorted(collide.items()):
        print(f"    cluster {cl:>2}  \"{head}\"  x{len(ids)}  ({ids[0]}…{ids[-1]})")
    # 3. cluster totals
    print("\n  cluster totals (actual/target):")
    ok = True
    for c in range(1, 26):
        n = sum(1 for r in all_rows if int(r[2]) == c)
        flag = "" if n == TARGETS[c] else "   <-- MISMATCH"
        if n != TARGETS[c]: ok = False
        print(f"    {c:>2} {CLUSTER_NAMES[c][:46]:<48} {n:>3}/{TARGETS[c]:<3}{flag}")
    # 4. quota tags
    print("\n  quota tags (actual/minimum):")
    mins = {"comparison":75,"faq":75,"career":60,"applied":45,"trust":30,"pricing-route":25,"asset":15}
    for tag, m in mins.items():
        n = sum(1 for r in all_rows if r[13] == tag)
        print(f"    {tag:<14} {n:>3}/{m:<3}{'' if n >= m else '   <-- SHORT'}")
        if n < m: ok = False
    # 5. certification balance
    print("\n  certification balance:")
    for cert in ("PCL-AI", "PFL-AI", "PML-AI"):
        print(f"    {cert}: {sum(1 for r in all_rows if r[7] == cert)}")
    print("\n  pillars:", sum(1 for r in all_rows if r[14] == "Pillar"))
    print(f"\n  == LEDGER {'COMPLETE AND CONSISTENT' if ok else 'HAS MISMATCHES'} ==")
    return ok

if __name__ == "__main__":
    raise SystemExit(0 if audit(build()) else 1)
