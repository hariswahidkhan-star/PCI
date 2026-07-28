"""ID-09 + Phase 6 (Open Graph half) — per-artifact canonical/og:url and the full share-card set.

Verifies Core/WorldPages.cs two ways, because the .NET SDK is not available here:

  1. A faithful Python transcription of the rendering logic (WebUtility.HtmlEncode, Clip,
     WorldUrl.Base's no-request fallback, the Layout head template, and the three shareable
     renderers' metadata derivation), executed against hostile and oversized inputs.
  2. A source contract scan of the C# itself, pinning the load-bearing strings: every interpolation
     in the <head> is E()-escaped, the canonical never comes from the request Host, each shareable
     page passes its OWN opaque path, the share image is a real versioned asset in wwwroot, and the
     advertised og:image:width/height match the actual JPEG bytes on disk.

What is under test is the safety contract:

  • a token-specific Passport / result / invitation advertises ITS OWN absolute URL as
    canonical and og:url — never the World homepage, never a forged Host header
  • the full metadata set (og:image + secure_url/type/width/height/alt, twitter
    summary_large_image card) is emitted, image versioned for cache refresh
  • every meta value is HTML-escaped and length-controlled — a hostile display name cannot
    break out of a content attribute in ANY tag it reaches
  • metadata derives only from fields that are public for the artifact: disclosure-gated
    scores/profiles/dates and the photograph never reach a meta tag
  • a revoked, expired, unpublished or suspended artifact resolves to nothing — the viewer
    gets the neutral not-available surface, with no rich metadata about the dead artifact

Run:  python3 tests/world_og_metadata_test.py
"""
import hashlib
import os
import re
import sqlite3
import struct
import sys
import urllib.parse
from datetime import datetime, timedelta

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.join(HERE, "..")
FAILURES = []

CANONICAL_BASE = "https://projectcontrolsinstitute.org"   # Redirects.CanonicalBase default

SRC = open(os.path.join(BACKEND, "Core", "WorldPages.cs"), encoding="utf-8").read()
SRC_WORLD = open(os.path.join(BACKEND, "Endpoints", "World.cs"), encoding="utf-8").read()
SRC_WACC = open(os.path.join(BACKEND, "Endpoints", "WorldAccount.cs"), encoding="utf-8").read()
SRC_WPASS = open(os.path.join(BACKEND, "Core", "WorldPassport.cs"), encoding="utf-8").read()


def check(label, ok, detail=""):
    print(f"{'PASS' if ok else 'FAIL'}  {label}{(' — ' + detail) if detail else ''}")
    if not ok:
        FAILURES.append(label)


# ── transcription of the C# helpers ────────────────────────────────────────────────────────

def html_encode(s):
    """WebUtility.HtmlEncode: &, <, >, \", ' (as &#39;), and 0xA0–0xFF as numeric refs."""
    out = []
    for ch in (s or ""):
        o = ord(ch)
        if ch == "&":
            out.append("&amp;")
        elif ch == "<":
            out.append("&lt;")
        elif ch == ">":
            out.append("&gt;")
        elif ch == '"':
            out.append("&quot;")
        elif ch == "'":
            out.append("&#39;")
        elif 0xA0 <= o <= 0xFF:
            out.append(f"&#{o};")
        else:
            out.append(ch)
    return "".join(out)


META_TITLE_MAX = 140
META_DESC_MAX = 300


def clip(s, mx):
    """WorldPages.Clip: word-boundary cut in the back half, ≤ mx chars including the ellipsis."""
    s = (s or "").strip()
    if len(s) <= mx:
        return s
    cut = s.rfind(" ", 0, mx)          # C# LastIndexOf(' ', mx-1): indices 0..mx-1
    return (s[:cut] if cut > mx // 2 else s[:mx - 1]).rstrip() + "…"


def esc_seg(token):
    """Uri.EscapeDataString: RFC 3986 — only unreserved characters survive."""
    return urllib.parse.quote(token or "", safe="")


SHARE_IMAGE_PATH = "/assets/og-image.jpg?v=1"
SHARE_IMAGE_ALT = "PCI World — a free daily project challenge from the Project Controls Institute."


def layout_head(title, meta_desc, canonical_path, og_title=None, og_desc=None, noindex=False):
    """The metadata-bearing lines of WorldPages.Layout's <head>, transcribed exactly."""
    e = html_encode
    page_url = CANONICAL_BASE + canonical_path
    image = CANONICAL_BASE + SHARE_IMAGE_PATH
    share_title = clip(og_title if og_title is not None else title, META_TITLE_MAX)
    share_desc = clip(og_desc if og_desc is not None else meta_desc, META_DESC_MAX)
    robots = ('<meta name="robots" content="noindex, nofollow">' if noindex
              else '<meta name="robots" content="index, follow, max-image-preview:large, max-snippet:-1">')
    return "\n".join([
        f"<title>{e(title)}</title>",
        f'<meta name="description" content="{e(clip(meta_desc, META_DESC_MAX))}">',
        robots,
        f'<link rel="canonical" href="{e(page_url)}">',
        '<meta property="og:site_name" content="PCI World">',
        f'<meta property="og:title" content="{e(share_title)}">',
        f'<meta property="og:description" content="{e(share_desc)}">',
        '<meta property="og:type" content="website">',
        f'<meta property="og:url" content="{e(page_url)}">',
        '<meta property="og:locale" content="en">',
        f'<meta property="og:image" content="{e(image)}">',
        f'<meta property="og:image:secure_url" content="{e(image)}">',
        '<meta property="og:image:type" content="image/jpeg">',
        '<meta property="og:image:width" content="1200">',
        '<meta property="og:image:height" content="630">',
        f'<meta property="og:image:alt" content="{e(SHARE_IMAGE_ALT)}">',
        '<meta name="twitter:card" content="summary_large_image">',
        f'<meta name="twitter:title" content="{e(share_title)}">',
        f'<meta name="twitter:description" content="{e(share_desc)}">',
        f'<meta name="twitter:image" content="{e(image)}">',
        f'<meta name="twitter:image:alt" content="{e(SHARE_IMAGE_ALT)}">',
    ])


def passport_head(name, completed_total, industries, token):
    """WorldPages.PublicPassport → Layout call, metadata arguments only (public fields only)."""
    plural = "" if completed_total == 1 else "s"
    ind = "y" if industries == 1 else "ies"
    return layout_head(
        f"{name} — PCI World Passport",
        f"Verified virtual project experience: {completed_total} completed PCI World "
        f"challenge{plural} across {industries} industr{ind}.",
        "/world" if not token else "/world/p/" + esc_seg(token),
        og_title=f"{name} — PCI World Passport",
        og_desc=f"Verified virtual project experience: {completed_total} completed PCI World "
                f"challenge{plural} across {industries} industr{ind}.",
        noindex=True)


def result_head(who, title, share, token):
    """WorldPages.PublicResult → Layout call, metadata arguments only."""
    who = who if (who or "").strip() else "A PCI World participant"
    return layout_head(
        f"Verified result — {title} — PCI World",
        f"{who} completed the PCI World challenge “{title}”.",
        "/world" if not token else "/world/r/" + esc_seg(token),
        og_title=f"{who} — {title} — PCI World Challenge",
        og_desc=share if share else f"Verified PCI World challenge result: {title}.",
        noindex=True)


def invite_head(who, title, token):
    """WorldPages.InvitePage → Layout call, metadata arguments only."""
    who = who if (who or "").strip() else "Someone"
    return layout_head(
        f"You've been challenged — {title} — PCI World",
        f"{who} completed “{title}” on PCI World. Can you improve the project outcome?",
        "/world/i/" + esc_seg(token),
        noindex=True)


def notfound_head():
    """WorldPages.NotFound — the neutral not-available surface."""
    return layout_head("Page not found — PCI World", "That page does not exist on PCI World.",
                       "/world", noindex=True)


# ── A. every shareable artifact advertises its OWN absolute canonical/og:url ───────────────

TOK = "a" * 64
h = passport_head("Sam Rivera", 7, 3, TOK)
want = f"{CANONICAL_BASE}/world/p/{TOK}"
check("A1 Passport canonical is its own absolute opaque URL",
      f'<link rel="canonical" href="{want}">' in h, want)
check("A2 Passport og:url equals the canonical",
      f'<meta property="og:url" content="{want}">' in h)
check("A3 Passport canonical is not the World homepage",
      f'href="{CANONICAL_BASE}/world">' not in h)

h = result_head("Sam Rivera", "Recover the Baseline", "", TOK)
want = f"{CANONICAL_BASE}/world/r/{TOK}"
check("A4 result canonical is its own absolute opaque URL",
      f'<link rel="canonical" href="{want}">' in h and f'content="{want}">' in h)

h = invite_head("Sam Rivera", "Recover the Baseline", TOK)
want = f"{CANONICAL_BASE}/world/i/{TOK}"
check("A5 invitation canonical is its own absolute opaque URL",
      f'<link rel="canonical" href="{want}">' in h and f'content="{want}">' in h)

check("A6 the absolute base is HTTPS and constant, never a request host",
      CANONICAL_BASE.startswith("https://"))
check("A7 WorldPages.cs never touches the request (no HttpRequest / Host in the renderer)",
      "HttpRequest" not in SRC and "req.Host" not in SRC and "Request.Host" not in SRC)

# a hostile token cannot break the URL or the attribute: reserved chars are %-escaped first
h = passport_head("Sam", 1, 1, 'x"/><script>t')
check("A8 a hostile token is URI-escaped before it reaches the canonical",
      "%22%2F%3E%3Cscript%3Et" in h and "<script>t" not in h)

# ── B. the full metadata set, image versioned and true to the asset on disk ────────────────

h = passport_head("Sam Rivera", 7, 3, TOK)
REQUIRED = [
    'property="og:site_name"', 'property="og:title"', 'property="og:description"',
    'property="og:type"', 'property="og:url"', 'property="og:image"',
    'property="og:image:secure_url"', 'property="og:image:type"', 'property="og:image:width"',
    'property="og:image:height"', 'property="og:image:alt"',
    '<meta name="twitter:card" content="summary_large_image">',
    'name="twitter:title"', 'name="twitter:description"', 'name="twitter:image"',
    'name="twitter:image:alt"',
]
missing = [t for t in REQUIRED if t not in h]
check("B1 the full og/twitter set is emitted", not missing, "; ".join(missing))
img = f"{CANONICAL_BASE}/assets/og-image.jpg?v=1"
check("B2 og:image is absolute HTTPS with a version query segment",
      f'<meta property="og:image" content="{img}">' in h)
check("B3 og:image:secure_url matches og:image",
      f'<meta property="og:image:secure_url" content="{img}">' in h)
check("B4 twitter:image matches og:image", f'<meta name="twitter:image" content="{img}">' in h)

# the C# constants, straight from the source
m = re.search(r'ShareImagePath = "([^"]+)"', SRC)
check("B5 C# ShareImagePath matches the transcription",
      m is not None and m.group(1) == SHARE_IMAGE_PATH, m.group(1) if m else "missing")
path_only = SHARE_IMAGE_PATH.split("?")[0]
asset = os.path.join(BACKEND, "wwwroot", path_only.lstrip("/"))
check("B6 the share image actually exists in wwwroot (no invented 404 endpoint)",
      os.path.isfile(asset), asset)
check("B7 the version segment is a cache key (?v=N)",
      re.search(r"\?v=\d+$", SHARE_IMAGE_PATH) is not None)


def jpeg_size(path):
    data = open(path, "rb").read()
    i = 2
    while i < len(data) - 9:
        if data[i] != 0xFF:
            i += 1
            continue
        marker = data[i + 1]
        if marker in (0xC0, 0xC1, 0xC2, 0xC3, 0xC5, 0xC6, 0xC7,
                      0xC9, 0xCA, 0xCB, 0xCD, 0xCE, 0xCF):
            hgt, wid = struct.unpack(">HH", data[i + 5:i + 9])
            return wid, hgt
        i += 2 + struct.unpack(">H", data[i + 2:i + 4])[0]
    return None


size = jpeg_size(asset) if os.path.isfile(asset) else None
mw = re.search(r"ShareImageWidth = (\d+)", SRC)
mh = re.search(r"ShareImageHeight = (\d+)", SRC)
check("B8 advertised og:image:width/height match the real JPEG bytes",
      size is not None and mw and mh and (int(mw.group(1)), int(mh.group(1))) == size,
      f"declared {mw.group(1) if mw else '?'}x{mh.group(1) if mh else '?'}, actual {size}")
check("B9 og:image:type matches the asset", 'ShareImageType = "image/jpeg"' in SRC
      and path_only.endswith(".jpg"))

# ── C. XSS: a hostile display name is escaped in EVERY meta tag it reaches ─────────────────

EVIL = '"><script>alert(1)</script><meta property="og:x" content="p0wned'
META_LINE = re.compile(r'^<meta (?:property|name)="[\w:]+" content="[^"<>]*">$')


def audit(label, head, must_reach):
    bad_script = "<script>" in head
    bad_break = '"><' in head.replace('">\n<', "")     # a value closing its own attribute
    # every meta line must still parse as ONE tag with a quote-free, angle-free content
    malformed = [ln for ln in head.splitlines()
                 if ln.startswith("<meta ") and not META_LINE.match(ln)]
    reached = [t for t in must_reach if f'{t} content="' in head
               and "&lt;script&gt;" in head.split(f'{t} content="', 1)[1].split("\n", 1)[0]]
    check(f"{label}: no live <script> and no attribute breakout",
          not bad_script and not bad_break and not malformed,
          "; ".join(malformed[:2]))
    check(f"{label}: payload arrives escaped in every tag it reaches ({len(must_reach)})",
          reached == must_reach, f"escaped in {reached}")


audit("C1 Passport (hostile name)", passport_head(EVIL, 2, 1, TOK),
      ['property="og:title"', 'name="twitter:title"'])
audit("C2 result (hostile name)", result_head(EVIL, "Recover the Baseline", "", TOK),
      ['property="og:title"', 'name="twitter:title"'])
audit("C3 result (hostile challenge title)", result_head("Sam", EVIL, "", TOK),
      ['property="og:title"', 'property="og:description"',
       'name="twitter:title"', 'name="twitter:description"'])
# the invitation's og:title carries the challenge title, not the inviter — the hostile name
# reaches the two descriptions, and a hostile challenge title reaches all four
audit("C4a invitation (hostile inviter name)", invite_head(EVIL, "Recover the Baseline", TOK),
      ['property="og:description"', 'name="twitter:description"'])
audit("C4b invitation (hostile challenge title)", invite_head("Sam", EVIL, TOK),
      ['property="og:title"', 'property="og:description"',
       'name="twitter:title"', 'name="twitter:description"'])
audit("C5 result (hostile authored share line)",
      result_head("Sam", "Recover the Baseline", EVIL, TOK),
      ['property="og:description"', 'name="twitter:description"'])

# quotes can never survive raw inside a content attribute
head = passport_head(EVIL, 2, 1, TOK)
check("C6 raw double-quote from the payload never survives in a value",
      '&quot;&gt;&lt;script&gt;' in head)

# ── D. length control ──────────────────────────────────────────────────────────────────────

long_name = "Bartholomew Montgomery Fitzgerald van der Berg " * 20   # ~940 chars
head = passport_head(long_name, 3, 2, TOK)
og_title = re.search(r'property="og:title" content="([^"]*)"', head).group(1)
check("D1 og:title is clipped to the cap with an ellipsis",
      len(og_title) <= META_TITLE_MAX + 60 and og_title.endswith("…"),
      f"len={len(og_title)}")
check("D2 clip cuts on a word boundary in the back half",
      not og_title[:-1].endswith(" ") and " " in og_title)
check("D3 clip itself honours the cap pre-escaping",
      len(clip(long_name + " — PCI World Passport", META_TITLE_MAX)) <= META_TITLE_MAX)
check("D4 short values pass through unclipped", clip("Sam Rivera", META_TITLE_MAX) == "Sam Rivera")
check("D5 description is length-controlled too",
      len(clip("x" * 1000, META_DESC_MAX)) <= META_DESC_MAX)
# escaping happens AFTER clipping in the C# (E(shareTitle)), so an entity is never cut in half
check("D6 C# clips before escaping (never truncates mid-entity)",
      "Clip(ogTitle ?? title, MetaTitleMax)" in SRC and "E(shareTitle)" in SRC)

# ── E. metadata derives ONLY from public fields ────────────────────────────────────────────

# a Passport whose owner disclosed nothing: score/profile/date exist on the row but are private
head = passport_head("Priva Te", 2, 1, TOK)
for leak, label in [("88.5", "score"), ("Evidence led", "profile"),
                    ("evidence_led", "profile key"), ("2026-05-04", "completion date")]:
    check(f"E1 undisclosed {label} never reaches a meta tag", leak not in head)

# the C# call site itself: the og arguments reference only name + whole-history public counts
seg = SRC[SRC.find("public static string PublicPassport"):]
seg = seg[:seg.find("editorial surfaces")]
ogdesc_line = [ln for ln in seg.splitlines() if "ogDesc:" in ln]
check("E2 C# Passport ogDesc derives from public counts only",
      len(ogdesc_line) == 1 and "completedTotal" in ogdesc_line[0]
      and "score" not in ogdesc_line[0] and "profile" not in ogdesc_line[0]
      and "completed_at" not in ogdesc_line[0] and "photoUrl" not in ogdesc_line[0])
check("E3 C# og:image can only be the branded constant, never the photograph",
      "photoUrl" not in SRC[SRC.find("public static string Layout"):SRC.find("<body>")]
      and 'og:image" content="{E(image)}' in SRC
      and "WorldUrl.Base() + ShareImagePath" in SRC)

# every interpolation in the Layout head is escaped or a compile-time constant
head_src = SRC[SRC.find("<!doctype html>"):SRC.find("</head>")]
raw_interp = [m2 for m2 in re.findall(r'(?:content|href)="\{([^}]+)\}', head_src)
              if not m2.startswith("E(")
              and m2 not in ("ShareImageType", "ShareImageWidth", "ShareImageHeight")]
check("E4 every head interpolation is E()-escaped or a constant",
      raw_interp == [], "; ".join(raw_interp))

# the result page's metadata carries who + title + authored share line — never the score
seg = SRC[SRC.find("public static string PublicResult"):]
seg = seg[:seg.find("public static string VerifyEmail")]
og_lines = "".join(ln for ln in seg.splitlines() if "ogTitle:" in ln or "ogDesc:" in ln)
check("E5 C# result og metadata never interpolates the score or dimensions",
      "score" not in og_lines and "dims" not in og_lines and "dimensions" not in og_lines)

# ── F. a dead artifact yields the neutral surface, never rich metadata ─────────────────────

sha = lambda s: hashlib.sha256(s.encode()).hexdigest()

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
PCIWORLD_ATTEMPTS = """
CREATE TABLE pciworld_attempts(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id INTEGER NOT NULL,
    challenge_id INTEGER NOT NULL,
    version INTEGER NOT NULL,
    status VARCHAR(16) NOT NULL DEFAULT 'in_progress',
    answers_json TEXT,
    score REAL,
    dimensions_json TEXT,
    profile_key TEXT,
    display_name TEXT,
    result_token_sha VARCHAR(64),
    result_revoked INTEGER DEFAULT 0,
    invite_id INTEGER,
    parent_attempt_id INTEGER,
    started_at TEXT DEFAULT (datetime('now')),
    completed_at VARCHAR(32),
    updated_at TEXT DEFAULT (datetime('now')))
"""


def expired(u):
    """WorldPassport.Expired: NULL/blank = no expiry; otherwise parsed <= now (UTC strings)."""
    s = u["passport_expires_at"]
    if not s or not s.strip():
        return False
    try:
        when = datetime.strptime(s.strip(), "%Y-%m-%d %H:%M:%S")
    except ValueError:
        return False
    return when <= datetime.utcnow()


def passport_by_token(d, token):
    """WorldAccount.PassportByToken: absent, unpublished, suspended and expired → one NULL."""
    u = d.execute("SELECT * FROM pciworld_users WHERE passport_token_sha=? "
                  "AND passport_public=1 AND status='active'", (sha(token),)).fetchone()
    return None if (u is None or expired(u)) else u


def result_by_token(d, token):
    return d.execute("SELECT * FROM pciworld_attempts WHERE result_token_sha=? "
                     "AND result_revoked=0 AND status='completed'", (sha(token),)).fetchone()


d = sqlite3.connect(":memory:")
d.row_factory = sqlite3.Row
d.executescript(PCIWORLD_USERS + ";" + PCIWORLD_ATTEMPTS + ";")

past = (datetime.utcnow() - timedelta(days=1)).strftime("%Y-%m-%d %H:%M:%S")
future = (datetime.utcnow() + timedelta(days=90)).strftime("%Y-%m-%d %H:%M:%S")
rows = [
    ("live@x.t", "Live Owner", 1, "active", sha("tok-live"), None),
    ("priv@x.t", "Private Owner", 0, "active", sha("tok-unpub"), None),
    ("susp@x.t", "Suspended Owner", 1, "suspended", sha("tok-susp"), None),
    ("exp@x.t", "Expired Owner", 1, "active", sha("tok-expired"), past),
    ("fut@x.t", "Future Owner", 1, "active", sha("tok-future"), future),
]
for email, name, pub, status, tsha, exp in rows:
    d.execute("INSERT INTO pciworld_users(email,password_hash,display_name,status,"
              "passport_public,passport_token_sha,passport_expires_at) VALUES(?,?,?,?,?,?,?)",
              (email, "x", name, status, pub, tsha, exp))
d.execute("INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,"
          "display_name,result_token_sha,result_revoked) VALUES(1,1,1,'completed',91.5,"
          "'Revoked Sharer',?,1)", (sha("tok-rev"),))
d.execute("INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,"
          "display_name,result_token_sha,result_revoked) VALUES(1,1,1,'completed',77.0,"
          "'Live Sharer',?,0)", (sha("tok-ok"),))

check("F1 a live published Passport resolves", passport_by_token(d, "tok-live") is not None)
check("F2 an unpublished Passport resolves to nothing", passport_by_token(d, "tok-unpub") is None)
check("F3 a suspended owner's Passport resolves to nothing", passport_by_token(d, "tok-susp") is None)
check("F4 an expired Passport resolves to nothing", passport_by_token(d, "tok-expired") is None)
check("F5 a future expiry still resolves", passport_by_token(d, "tok-future") is not None)
check("F6 a rotated-away token resolves to nothing", passport_by_token(d, "tok-old") is None)
check("F7 a revoked result link resolves to nothing", result_by_token(d, "tok-rev") is None)
check("F8 a live result link resolves", result_by_token(d, "tok-ok") is not None)

# what the viewer of a dead link gets instead: the neutral surface, with nothing of the artifact
nf = notfound_head()
for leak in ("Expired Owner", "Revoked Sharer", "91.5", "tok-expired", "tok-rev"):
    check(f"F9 not-available page carries nothing of the dead artifact ({leak})", leak not in nf)
check("F10 not-available page is noindex with neutral canonical",
      'content="noindex, nofollow"' in nf
      and f'<link rel="canonical" href="{CANONICAL_BASE}/world">' in nf)

# and the endpoints really do refuse before any renderer runs (source contract)
check("F11 result endpoint filters revoked+incomplete in SQL before rendering",
      "result_revoked=0 AND status='completed'" in SRC_WORLD)
check("F12 invite endpoint filters revoked in SQL before rendering",
      "i.token_sha=? AND i.revoked=0" in SRC_WORLD)
check("F13 Passport endpoint collapses dead states to NotFound",
      "passport_public=1 AND status='active'" in SRC_WACC
      and "WorldPassport.Expired(u) ? null : u" in SRC_WACC)
check("F14 C# Expired treats NULL as no expiry (transcription anchor)",
      'if (string.IsNullOrWhiteSpace(s)) return false;' in SRC_WPASS)

# ── G. source contract: the C# emits what the transcription emits ──────────────────────────

for lit, label in [
    ('<meta name="twitter:card" content="summary_large_image">', "twitter card literal"),
    ('<meta property="og:image:secure_url" content="{E(image)}">', "secure_url from base"),
    ('<meta property="og:image:width" content="{ShareImageWidth}">', "width constant"),
    ('<meta property="og:image:height" content="{ShareImageHeight}">', "height constant"),
    ('<meta property="og:image:alt" content="{E(ShareImageAlt)}">', "image alt"),
    ('<meta name="twitter:image:alt" content="{E(ShareImageAlt)}">', "twitter image alt"),
    ('<link rel="canonical" href="{E(pageUrl)}">', "canonical from pageUrl"),
    ('<meta property="og:url" content="{E(pageUrl)}">', "og:url from the same pageUrl"),
    ('var pageUrl = WorldUrl.Base() + canonicalPath;', "pageUrl from WorldUrl.Base()"),
    ('"/world/p/" + Uri.EscapeDataString(token)', "Passport per-token canonical"),
    ('"/world/r/" + Uri.EscapeDataString(token)', "result per-token canonical"),
    ('"/world/i/" + Uri.EscapeDataString(token)', "invitation per-token canonical"),
]:
    check(f"G source: {label}", lit in SRC, lit)

check("G source: the old fixed homepage canonical is gone from the Passport call",
      'ogTitle: $"{name} — PCI World Passport"' in SRC
      and '"/world",\n            ogTitle: $"{name} — PCI World Passport"' not in SRC.replace("    ", ""))

print("=" * 72)
if FAILURES:
    print(f"{len(FAILURES)} FAILED: " + "; ".join(FAILURES))
    sys.exit(1)
print("All per-artifact canonical / Open Graph metadata assertions passed.")
