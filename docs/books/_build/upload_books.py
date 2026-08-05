#!/usr/bin/env python3
"""Upload the built volumes into a PCI instance as certification documents.

The admin panel's Books & study materials section is backed by `cert_documents`, and its upload
route stores the file bytes privately, snapshots any file it replaces into `cert_document_versions`,
and serves students a per-recipient watermarked copy when the row is flagged for it. This script
drives that route so the four volumes can be published — or re-published after a rebuild — without
clicking through four upload dialogs and without anyone hand-copying a 1,257-page PDF.

Re-running is safe and is the intended way to publish a rebuild: pass `--replace` with the document
ids and the outgoing files are versioned rather than lost.

    # publish for the first time
    python3 upload_books.py --base-url https://example.org --email owner@example.org

    # publish a rebuild over existing rows
    python3 upload_books.py --base-url https://example.org --email owner@example.org \
        --replace pcl=12 pfl=13 pml=14 standards=15

The password is read from PCI_ADMIN_PASSWORD, or prompted for. It is never taken from argv, where
it would land in shell history and the process table.
"""
import argparse
import base64
import getpass
import json
import os
import pathlib
import sys
import urllib.error
import urllib.request

ROOT = pathlib.Path(__file__).resolve().parents[3]

# certification codes resolve server-side via Certs.TryResolve; ids 1/2/3 on a seeded install.
# `kind` is "bok" for the three volumes, matching the rows a fresh install already seeds — a
# migration seeds one Body of Knowledge per certification, two of them carrying an early, much
# smaller file. Uploading under a different kind or title would leave those seeded rows in place and
# students would see two Bodies of Knowledge per credential, one of them stale. See adopt().
VOLUMES = {
    "pcl": {
        "path": ROOT / "docs/bok/build/pcl-ai-bok.pdf",
        "certification_id": "PCL-AI",
        "kind": "bok",
        "title": "PCI PCL-AI Body of Knowledge",
        "filename": "PCI-PCL-AI-Body-of-Knowledge.pdf",
        "description": "The complete Body of Knowledge for the PCI AI Project Controls Leader "
                       "credential: 13 domains, 61 knowledge areas.",
    },
    "pfl": {
        "path": ROOT / "docs/books/pfl-ai/build/pfl-ai-bok-draft.pdf",
        "certification_id": "PFL-AI",
        "kind": "bok",
        "title": "PCI PFL-AI Body of Knowledge",
        "filename": "PCI-PFL-AI-Body-of-Knowledge.pdf",
        "description": "The complete Body of Knowledge for the PCI AI Project Finance Leader "
                       "credential: 16 domains, 61 knowledge areas.",
    },
    "pml": {
        "path": ROOT / "docs/books/pml-ai/build/pml-ai-bok-draft.pdf",
        "certification_id": "PML-AI",
        "kind": "bok",
        "title": "PCI PML-AI Body of Knowledge",
        "filename": "PCI-PML-AI-Body-of-Knowledge.pdf",
        "description": "The complete Body of Knowledge for the PCI Project Management Leader – AI "
                       "credential: 16 domains, 63 knowledge areas.",
    },
    # The Standards volume governs all three credentials, so it belongs to none of them. "all" stores
    # a NULL certification_id, which /api/me/cert-documents matches for every candidate. Omitting the
    # field instead would fold it onto the founding certification and hide it from PFL-AI and PML-AI.
    # Nothing seeds it, so this one is genuinely new.
    "standards": {
        "path": ROOT / "docs/books/laws/PCI-Standards.pdf",
        "certification_id": "all",
        "kind": "book",
        "title": "PCI Standards",
        "filename": "PCI-Standards.pdf",
        "description": "The 113 mandatory PCI Standards and their 532 process requirements, with "
                       "the Charter and Drafting Manual that govern them.",
    },
}

# Certification code → id, for matching the seeded rows. Fixed by the migration in Data/MultiCert.cs.
CERT_IDS = {"PCL-AI": 1, "PFL-AI": 2, "PML-AI": 3}

# Kestrel's per-request ceiling on the upload route, and DocStore's decoded-bytes cap. Checked here
# so an oversized volume fails with a sentence instead of a bare 413 after a long upload.
REQUEST_CAP = 40_000_000
DOCSTORE_CAP = 26_214_400


def post(url: str, payload: dict, token: str | None = None) -> dict:
    body = json.dumps(payload).encode()
    req = urllib.request.Request(url, data=body, method="POST")
    req.add_header("Content-Type", "application/json")
    if token:
        req.add_header("Authorization", f"Bearer {token}")
    try:
        with urllib.request.urlopen(req) as r:
            return json.loads(r.read() or b"{}")
    except urllib.error.HTTPError as e:
        detail = (e.read() or b"").decode(errors="replace")[:400]
        raise SystemExit(f"  HTTP {e.code} from {url}\n  {detail}")
    except urllib.error.URLError as e:
        raise SystemExit(f"  cannot reach {url}: {e.reason}")


def login(base: str, email: str, password: str) -> str:
    """Sign in, prompting for the second factor only if the account has TOTP enrolled.

    An account with a TOTP secret answers 401 `totp_required` rather than issuing a token, so the
    code is asked for on demand instead of up front — most accounts never see the prompt.
    """
    body = {"email": email, "password": password}
    try:
        res = post(f"{base}/api/admin/auth/login", body)
    except SystemExit as first:
        if "totp_required" not in str(first):
            raise
        body["totp"] = input("6-digit authenticator code: ").strip()
        res = post(f"{base}/api/admin/auth/login", body)

    token = res.get("token")
    if not token:
        raise SystemExit(f"  login returned no token: {json.dumps(res)[:300]}")
    perms = res.get("permissions") or []
    role = (res.get("admin") or {}).get("role")
    if role != "owner" and "resources" not in perms:
        raise SystemExit("  this account cannot upload books: the upload route needs the "
                         "`resources` permission, or the owner role.")
    if (res.get("admin") or {}).get("must_change_pw"):
        print("  note: this account must change its password at next sign-in.")
    return token


def get(url: str, token: str) -> dict:
    req = urllib.request.Request(url)
    req.add_header("Authorization", f"Bearer {token}")
    try:
        with urllib.request.urlopen(req) as r:
            return json.loads(r.read() or b"{}")
    except urllib.error.HTTPError as e:
        raise SystemExit(f"  HTTP {e.code} from {url}\n  {(e.read() or b'').decode()[:300]}")


def adopt(base: str, token: str) -> dict:
    """Map each volume key to the id of the row it should replace, where one already exists.

    A fresh install seeds a Body of Knowledge row per certification, and two of them already carry an
    early file. Creating new rows beside those leaves every candidate looking at two Bodies of
    Knowledge for their credential, one of them stale and much smaller — so match on
    (certification, kind) and replace in place. Replacing versions the outgoing file rather than
    discarding it, so the seeded copy stays recoverable from the row's history.
    """
    rows = get(f"{base}/api/admin/cert_documents", token).get("rows") or []
    found = {}
    for key, v in VOLUMES.items():
        want_cert = CERT_IDS.get(v["certification_id"])   # None for the "all" row
        for r in rows:
            if r.get("kind") == v["kind"] and r.get("certification_id") == want_cert:
                found[key] = r["id"]
                break
    return found


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--base-url", required=True, help="instance root, e.g. https://example.org")
    ap.add_argument("--email", required=True, help="admin account with the `resources` permission")
    ap.add_argument("--only", nargs="*", choices=sorted(VOLUMES), metavar="KEY",
                    help="upload only these volumes (default: all four)")
    ap.add_argument("--replace", nargs="*", default=[], metavar="KEY=ID",
                    help="replace the file on an existing cert_documents row; the outgoing file is "
                         "snapshotted into cert_document_versions first")
    ap.add_argument("--reason", default="rebuilt from source",
                    help="recorded against a replaced file in the version history")
    ap.add_argument("--no-adopt", action="store_true",
                    help="always create new rows instead of replacing the matching existing one "
                         "(the default matches on certification and kind, so a re-run updates in "
                         "place rather than leaving two copies of a volume on the shelf)")
    ap.add_argument("--watermark", action="store_true",
                    help="stamp each student's copy with their name and student number")
    ap.add_argument("--draft", action="store_true",
                    help="create the rows unpublished, to review before students can see them")
    ap.add_argument("--dry-run", action="store_true", help="check sizes and exit without uploading")
    args = ap.parse_args()

    base = args.base_url.rstrip("/")
    replace = dict(p.split("=", 1) for p in args.replace)
    keys = args.only or list(VOLUMES)

    # Check every file before authenticating, so a missing or oversized volume is reported up front
    # rather than half way through a sequence of uploads.
    jobs = []
    for key in keys:
        v = VOLUMES[key]
        path = v["path"]
        if not path.exists():
            raise SystemExit(f"  missing: {path}\n  build the volume first.")
        raw = path.read_bytes()
        if len(raw) > DOCSTORE_CAP:
            raise SystemExit(f"  {path.name} is {len(raw)/1e6:.1f} MB, over DocStore's "
                             f"{DOCSTORE_CAP/1e6:.1f} MB cap.")
        b64 = base64.b64encode(raw).decode()
        if len(b64) + 2048 > REQUEST_CAP:
            raise SystemExit(f"  {path.name} is {len(b64)/1e6:.1f} MB as base64, over the "
                             f"{REQUEST_CAP/1e6:.0f} MB request cap on the upload route.")
        jobs.append((key, v, raw, b64))
        print(f"  {key:9} {path.name:34} {len(raw)/1e6:6.2f} MB → {len(b64)/1e6:6.2f} MB encoded")

    if args.dry_run:
        print("\ndry run — nothing uploaded.")
        return

    password = os.environ.get("PCI_ADMIN_PASSWORD") or getpass.getpass("admin password: ")
    print(f"\nsigning in to {base} as {args.email}")
    token = login(base, args.email, password)

    # An explicit --replace always wins; otherwise adopt whatever row already holds this volume, so a
    # re-run updates the book in place instead of stacking a second copy beside it.
    if not args.no_adopt:
        for key, doc_id in adopt(base, token).items():
            replace.setdefault(key, str(doc_id))

    print()
    for key, v, raw, b64 in jobs:
        payload = {
            "file": f"data:application/pdf;base64,{b64}",
            "filename": v["filename"],
            "title": v["title"],
            "description": v["description"],
            "kind": v["kind"],
            "watermark": bool(args.watermark),
            "published": not args.draft,
        }
        if v["certification_id"]:
            payload["certification_id"] = v["certification_id"]
        if key in replace:
            payload["id"] = int(replace[key])
            payload["reason"] = args.reason
        res = post(f"{base}/api/admin/cert-documents/upload", payload, token)
        row = res.get("row") or {}
        verb = "replaced" if key in replace else "created"
        print(f"  {verb:9} id={row.get('id')}  {v['title']}  "
              f"sha256={str(row.get('sha256'))[:12]}…  {row.get('size_bytes')} bytes")

    print("\nDone. Admin panel → Books & study materials.")
    if args.draft:
        print("Rows are unpublished — publish them there when you are ready.")


if __name__ == "__main__":
    sys.exit(main())
