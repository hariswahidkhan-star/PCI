#!/usr/bin/env python3
"""Merge the link audit, the quality check and the URL map into one per-file task list."""
import json, re
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
links = {r["file"]: r for r in json.load(open(ROOT / "_tools/link_audit.json"))}
qual = {r["file"]: r for r in json.load(open(ROOT / "_tools/quality_check.json"))}
urls = json.load(open(ROOT / "_tools/url_map.json"))

tasks = []
for f, L in links.items():
    Q = qual.get(f, {})
    jobs = []
    if not L["links"] and L["has_note"]:
        jobs.append("EMBED: the piece carries a linking note but no link in the body. "
                    "Place the links inside the prose, in the sentences that raise the question each answers, "
                    "then rewrite the note to record what was placed and why.")
    elif not L["links"]:
        jobs.append("EMBED: no links at all. Add them per the architecture.")
    for i in L["issues"]:
        if i.startswith(("2x", "3x", "4x", "5x")) or "cross-estate" in i or "all five" in i:
            jobs.append(f"RATIONALISE: {i}")
        elif "internal links" in i:
            jobs.append(f"INTERNAL: {i}")
        elif "weak anchor" in i:
            jobs.append(f"ANCHOR: {i}")
    for fl in Q.get("flags", []):
        jobs.append(f"QUALITY: {fl}")
    if jobs:
        tasks.append({"file": f, "platform": L["platform"], "host": L["host"],
                      "canonical": L["canonical"], "words": L["words"],
                      "type": Q.get("type", ""), "primary_kw": Q.get("primary_kw", ""),
                      "current_links": L["links"], "jobs": jobs})

# Batch so one agent sees related pieces: same host or same directory, contiguous ids.
tasks.sort(key=lambda t: (t["host"] or "zz-offsite", t["file"]))
SIZE = 11
batches = [tasks[i:i + SIZE] for i in range(0, len(tasks), SIZE)]
out = {"url_map": urls, "batches": [{"id": i + 1, "files": b} for i, b in enumerate(batches)]}
(ROOT / "_tools/worklist.json").write_text(json.dumps(out, indent=1))
print(f"{len(tasks)} files need work, in {len(batches)} batches of <= {SIZE}")
from collections import Counter
print(Counter(j.split(":")[0] for t in tasks for j in t["jobs"]))
