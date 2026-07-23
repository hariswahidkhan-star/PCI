#!/usr/bin/env python3
"""SQ-5 gate: fail the build on any vulnerable NuGet package that is NOT an explicitly
allow-listed, justified residual.

Why a wrapper instead of raw `dotnet list package --vulnerable`?
  `dotnet list package --vulnerable` has no built-in allow-list. We want the gate to BLOCK on any
  *new* vulnerable package (so a regression can never slip in) while still permitting a small set of
  documented residuals — transitive advisories with no upstream patch available yet. Each residual is
  recorded in nuget-vuln-allowlist.json with a package name, the advisory URL, a justification, and a
  review date, and is cross-referenced in docs/testing/SUPPRESSION_PROCESS.md.

Behaviour:
  * Any vulnerable package whose advisory URL is not allow-listed -> non-zero exit (build fails).
  * Allow-listed advisories are reported as acknowledged residuals and do NOT fail the build.
  * An allow-list entry that no longer matches any live advisory is reported as STALE (informational,
    non-failing) so the list gets pruned when a fix finally ships.

Usage:
  python3 tools/check_nuget_vulns.py                 # runs dotnet in the backend project dir
  python3 tools/check_nuget_vulns.py --json vulns.json   # parse a pre-captured report (tests/CI)
"""
import argparse
import json
import os
import subprocess
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND_DIR = os.path.dirname(HERE)
ALLOWLIST = os.path.join(HERE, "nuget-vuln-allowlist.json")


def load_allowlist():
    if not os.path.exists(ALLOWLIST):
        return {}
    with open(ALLOWLIST, encoding="utf-8") as fh:
        data = json.load(fh)
    # keyed by advisory URL (normalised, trailing slash stripped, lower-cased)
    return {norm(e["advisory"]): e for e in data.get("allow", [])}


def norm(url):
    return (url or "").strip().rstrip("/").lower()


def run_dotnet():
    """Return the parsed JSON report from `dotnet list package --vulnerable`."""
    cmd = [
        "dotnet", "list", "package",
        "--vulnerable", "--include-transitive", "--format", "json",
    ]
    proc = subprocess.run(cmd, cwd=BACKEND_DIR, capture_output=True, text=True)
    if proc.returncode != 0:
        sys.stderr.write(proc.stdout + "\n" + proc.stderr + "\n")
        sys.exit("::error::`dotnet list package --vulnerable` failed to run (restore first?)")
    return json.loads(proc.stdout)


def collect(report):
    """Flatten to a list of (package_id, version, severity, advisory_url)."""
    out = []
    for project in report.get("projects", []):
        for fw in project.get("frameworks", []):
            for bucket in ("topLevelPackages", "transitivePackages"):
                for pkg in fw.get(bucket, []) or []:
                    for v in pkg.get("vulnerabilities", []) or []:
                        out.append((
                            pkg.get("id", "?"),
                            pkg.get("resolvedVersion", "?"),
                            v.get("severity", "?"),
                            v.get("advisoryurl", ""),
                        ))
    return out


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--json", help="parse a pre-captured `--format json` report instead of running dotnet")
    args = ap.parse_args()

    if args.json:
        with open(args.json, encoding="utf-8") as fh:
            report = json.load(fh)
    else:
        report = run_dotnet()

    allow = load_allowlist()
    found = collect(report)

    blocking = [f for f in found if norm(f[3]) not in allow]
    acknowledged = [f for f in found if norm(f[3]) in allow]
    matched_keys = {norm(f[3]) for f in acknowledged}
    stale = [k for k in allow if k not in matched_keys]

    if acknowledged:
        print("Acknowledged residual vulnerabilities (allow-listed — see SUPPRESSION_PROCESS.md):")
        for pid, ver, sev, url in acknowledged:
            entry = allow[norm(url)]
            print(f"  - {pid} {ver} [{sev}] {url}")
            print(f"      reason: {entry.get('reason', '(none given)')}")
            print(f"      review_by: {entry.get('review_by', '(unset)')}")

    if stale:
        print("\nNOTE: allow-list entries with no matching live advisory (prune these — a fix may have shipped):")
        for k in stale:
            print(f"  - {allow[k].get('advisory')}  ({allow[k].get('package', '?')})")

    if blocking:
        print("\n::error::Vulnerable NuGet packages NOT in the allow-list:")
        for pid, ver, sev, url in blocking:
            print(f"  - {pid} {ver} [{sev}] {url}")
        print("\nEither upgrade to a patched version (preferred), or, if no patch exists, add a justified")
        print("entry to backend/tools/nuget-vuln-allowlist.json following SUPPRESSION_PROCESS.md.")
        sys.exit(1)

    print("\nOK: no un-acknowledged vulnerable NuGet packages." if found
          else "OK: no vulnerable NuGet packages.")


if __name__ == "__main__":
    main()
