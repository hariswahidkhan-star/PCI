"""Phase 6 Release-1 (premium share experience, §8.7–8.8) — the server-truth share caption.

Verifies Endpoints/WorldAccount.cs two ways, because the .NET SDK is not available here:

  1. A faithful Python transcription of ShareCaption/NeutralCaptionText, executed against a real
     SQLite database built from the real pciworld_users DDL, with hostile display names.
  2. A source contract scan of the C# itself (and of the frontend ShareSheet), pinning the
     load-bearing strings: the caption reads ONLY display_name, the listing gained per-link
     created_at, no token is minted anywhere in the share sheet or the listing, and Instagram is
     honestly absent from the URL-share channels.

The safety contract under test:

  • the caption is built from PUBLIC fields only — the display name the public Passport already
    shows, the product name, and the disclaimer sentence. Never the email, never the Student
    Number, never a token hash, never anything else on the row.
  • a hostile display name arrives NEUTRAL: no angle brackets survive (no markup wherever the
    caption is pasted), control characters cannot smuggle extra lines, whitespace collapses,
    and the 80-character display-name cap holds.
  • the share sheet only ever carries the EXISTING opaque public URL — nothing client-side can
    mint a token, and no name/email/number enters a URL.

Run:  python3 tests/world_share_caption_test.py
"""
import os
import re
import sqlite3
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.join(HERE, "..")
FAILURES = []

SRC = open(os.path.join(BACKEND, "Endpoints", "WorldAccount.cs"), encoding="utf-8").read()
SHEET = open(os.path.join(BACKEND, "..", "frontend", "src", "world", "ShareSheet.tsx"),
             encoding="utf-8").read()


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


# ── transcription of the C# ────────────────────────────────────────────────────────────────

DISCLAIMER = "Practice evidence only — never a credential."


def neutral_caption_text(raw):
    """WorldAccount.NeutralCaptionText: drop <>, controls→space, collapse ws, trim, cap 80."""
    out = []
    for ch in (raw or ""):
        if ch in "<>":
            continue
        # C# char.IsControl: Cc category — C0/C1 controls incl. \t \n \r
        out.append(" " if (ord(ch) < 0x20 or 0x7F <= ord(ch) <= 0x9F) else ch)
    t = re.sub(r"\s+", " ", "".join(out)).strip()
    return t[:80].rstrip() if len(t) > 80 else t


def share_caption(d, user_id):
    """WorldAccount.ShareCaption: SELECT display_name only; name — product line + disclaimer."""
    row = d.execute("SELECT display_name FROM pciworld_users WHERE id=?", (user_id,)).fetchone()
    name = neutral_caption_text(row["display_name"] if row else None)
    return (name + " — " if name else "") + "PCI World Passport. " + DISCLAIMER


# the REAL DDL, lifted verbatim from Data/WorldSchema.cs (same columns, same defaults)
PCIWORLD_USERS = """
CREATE TABLE pciworld_users(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email VARCHAR(190) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    display_name TEXT,
    status VARCHAR(16) NOT NULL DEFAULT 'active',
    email_verified INTEGER DEFAULT 0,
    passport_public INTEGER DEFAULT 0,
    passport_token_sha VARCHAR(64),
    passport_show_scores INTEGER DEFAULT 1,
    passport_show_profiles INTEGER DEFAULT 1,
    passport_show_dates INTEGER DEFAULT 1,
    passport_expires_at TEXT,
    passport_photo_ref VARCHAR(255),
    passport_photo_mime VARCHAR(32),
    student_user_id INTEGER,
    failed_logins INTEGER DEFAULT 0,
    lockout_until TEXT,
    last_login_at TEXT,
    created_at TEXT DEFAULT (datetime('now')))
"""

d = sqlite3.connect(":memory:")
d.row_factory = sqlite3.Row
d.executescript(PCIWORLD_USERS)

TOKEN_SHA = "f" * 64
STUDENT_NO = "PCI-2026-004217"


def mk(email, name, **extra):
    cols = {"email": email, "password_hash": "x", "display_name": name,
            "passport_token_sha": TOKEN_SHA, "passport_public": 1, "student_user_id": 4217}
    cols.update(extra)
    keys = ",".join(cols)
    return d.execute(f"INSERT INTO pciworld_users({keys}) VALUES({','.join('?' * len(cols))})",
                     tuple(cols.values())).lastrowid


# ── A. the caption is public-fields-only ───────────────────────────────────────────────────

uid = mk("sam.rivera@example.org", "Sam Rivera")
cap = share_caption(d, uid)
check("A1 caption = name — product line + disclaimer",
      cap == f"Sam Rivera — PCI World Passport. {DISCLAIMER}", cap)
check("A2 the email never reaches the caption", "sam.rivera" not in cap and "@" not in cap)
check("A3 the passport token (even hashed) never reaches the caption", TOKEN_SHA not in cap)
check("A4 no student/registration number reaches the caption",
      STUDENT_NO not in cap and "4217" not in cap)

uid = mk("noname@example.org", None)
check("A5 no display name → the neutral product caption alone",
      share_caption(d, uid) == f"PCI World Passport. {DISCLAIMER}")
uid = mk("blank@example.org", "   ")
check("A6 a whitespace-only name is treated as absent",
      share_caption(d, uid) == f"PCI World Passport. {DISCLAIMER}")
check("A7 an unknown account still yields the neutral caption (no crash, no leak)",
      share_caption(d, 999999) == f"PCI World Passport. {DISCLAIMER}")

# ── B. a hostile display name arrives neutral ──────────────────────────────────────────────

uid = mk("evil@example.org", '"><script>alert(1)</script><img src=x onerror=alert(2)>')
cap = share_caption(d, uid)
check("B1 no angle bracket survives into the caption", "<" not in cap and ">" not in cap, cap)
check("B2 no executable markup shape survives", "<script" not in cap.lower() and "<img" not in cap.lower())
check("B3 the disclaimer still closes the caption", cap.endswith(DISCLAIMER))

uid = mk("multiline@example.org", "Line One\r\nFAKE SENTENCE.\tTabbed\x00\x1b[31m")
cap = share_caption(d, uid)
check("B4 control characters cannot smuggle line breaks into the caption",
      "\n" not in cap and "\r" not in cap and "\t" not in cap and "\x00" not in cap
      and "\x1b" not in cap, repr(cap))
check("B5 collapsed to single spaces", "  " not in cap)

uid = mk("long@example.org", "N" * 500)
cap = share_caption(d, uid)
name_part = cap.split(" — ")[0]
check("B6 the name part honours the 80-character cap", len(name_part) == 80, str(len(name_part)))
check("B7 the product line + disclaimer survive the cap intact",
      cap.endswith(f"PCI World Passport. {DISCLAIMER}"))

uid = mk("unicode@example.org", "Zoë Rivéra 中文 🌍")
check("B8 legitimate Unicode passes through untouched",
      share_caption(d, uid).startswith("Zoë Rivéra 中文 🌍 — "))

# ── C. source contract — the C# builds what the transcription builds ───────────────────────

for lit, label in [
    ('public const string ShareCaptionDisclaimer = "Practice evidence only — never a credential.";',
     "the disclaimer constant"),
    ('"SELECT display_name FROM pciworld_users WHERE id=?"',
     "ShareCaption reads ONLY display_name — no email/token/number column in reach"),
    ('+ "PCI World Passport. " + ShareCaptionDisclaimer', "caption assembly order"),
    ("if (ch is '<' or '>') continue;", "angle brackets dropped"),
    ("char.IsControl(ch) ? ' ' : ch", "controls neutralised to spaces"),
    ('Regex.Replace(sb.ToString(), @"\\s+", " ").Trim()', "whitespace collapsed"),
    ('t.Length > 80 ? t[..80].TrimEnd() : t', "80-char cap"),
    ('caption = ShareCaption(db, u.Id)', "the listing serves the caption"),
    ('created_at = H.Str(r["updated_at"])', "per-result-link created_at (mint stamps updated_at)"),
]:
    check(f"C source: {label}", lit in SRC, lit)

# the /me/shares listing must stay read-only: no token mint anywhere in its handler chain
seg = SRC[SRC.find('app.MapGet("/api/world/me/shares"'):]
seg = seg[:seg.find('app.MapPost')]
check("C10 the share listing mints nothing (no RandomHex, no INSERT in the GET handler)",
      "RandomHex" not in seg and "INSERT" not in seg.upper())
check("C11 invitations already carry created_at (unchanged contract)",
      'version = H.L(r["version"]), created_at = H.Str(r["created_at"])' in SRC)

# ── D. the frontend sheet is honest and URL-safe (source contract) ─────────────────────────

check("D1 the sheet's fallback caption equals the server's nameless caption",
      f"'PCI World Passport. {DISCLAIMER}'" in SHEET)
check("D2 the disclosure line is the required sentence",
      "'Anyone with this link sees exactly what your public Passport shows.'" in SHEET)
check("D3 capability matrix: url-share supported; direct posting and comment sync are not",
      "supported: ['url-share']" in SHEET
      and "unsupported: ['direct-posting', 'comment-sync']" in SHEET)
check("D4 Instagram is intentionally absent from the channel URLs",
      "instagram.com" not in SHEET.lower().replace("instagram does not accept", "")
      and "For Instagram" in SHEET)
check("D5 the sheet never mints: no token generation, URL comes from props",
      "RandomHex" not in SHEET and "crypto.getRandomValues" not in SHEET
      and "new URL(url, window.location.origin)" in SHEET)
check("D6 every channel href is encodeURIComponent-built",
      SHEET.count("${e(") >= 7 and "const e = encodeURIComponent" in SHEET)
check("D7 native share resolve is worded as a handoff, not a publication",
      "nothing is posted until you finish there" in SHEET)
check("D8 clipboard denial degrades to select-the-text",
      "select()" in SHEET and "Copying was blocked" in SHEET)

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All share-caption assertions passed.")
