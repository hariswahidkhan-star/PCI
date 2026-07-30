#!/usr/bin/env python3
"""
PCI World launch board — live HTTP suite.

Boots the REAL built backend and drives the real routes. The board's whole job is to be the place
a feature gets switched on, which makes it the one screen where a mistake is a launch. Three
failure modes matter and none of them throw:

  1. A non-owner turns a feature on. The generic settings PATCH already scopes World flags to the
     owner permission; a new endpoint is a new way to get it wrong, so it is probed directly.
  2. The image flag moves without the moderation prerequisite recorded. This is the one flag whose
     harm lands on somebody who is not the operator, and the refusal has to live in the SERVER —
     a greyed-out switch protects nobody from a curl.
  3. The endpoint becomes a general settings write. It is owner-gated and takes a key/value pair,
     which is exactly the shape that quietly turns into "owner can PATCH anything". C09 pins that.

Every assertion below is checked at the DATABASE, not only in the response, because the response is
this endpoint's own account of what it did.

Runs against SQLite and MySQL (TEST_DB_PROVIDER=mysql). Exit code 0 iff every assertion passes.
Run from backend/:  python3 tests/world_launch_test.py
"""
import json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_worldlaunch.db")
STORAGE = os.path.join(HERE, "_worldlaunch_storage")

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_wlaunch") else _base + "_wlaunch")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, body=None, headers=None):
    hdr = dict(headers or {})
    data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp: return resp.status, resp.read()
    except urllib.error.HTTPError as e: return e.code, e.read()
    except urllib.error.URLError as e: return 0, str(e).encode()

def js(path, method="GET", body=None, headers=None):
    code, raw = req(method, path, body, headers)
    text = raw.decode(errors="replace") if isinstance(raw, bytes) else raw
    try: return code, json.loads(text) if text else {}
    except Exception: return code, {"_raw": text}

def sql(statement, args=()):
    if PROVIDER == "mysql":
        import pymysql
        con = pymysql.connect(**MYSQL); cur = con.cursor()
        # Two MySQL-only traps documented in forum_test.py: `args or None` (pymysql renders via
        # `query % args` even for an empty tuple), and the blind ?->%s replacement, which turns a
        # '?' inside a string literal into an extra placeholder. Keep literals free of '?'.
        try:
            cur.execute(statement.replace("INSERT OR REPLACE INTO", "REPLACE INTO").replace("?", "%s"), args or None)
        except Exception as e:
            raise SystemExit(f"SQL FAILED: {statement!r} args={args!r}: {e}")
        con.commit(); rows = cur.fetchall(); con.close(); return rows
    con = sqlite3.connect(DB); cur = con.cursor()
    cur.execute(statement, args); con.commit()
    rows = cur.fetchall(); con.close(); return rows

def setting(key):
    rows = sql("SELECT svalue FROM site_settings WHERE skey=?", (key,))
    return rows[0][0] if rows else None

def _reset_mysql():
    import pymysql
    root = pymysql.connect(host=MYSQL["host"], port=MYSQL["port"], user=MYSQL["user"], password=MYSQL["password"])
    cur = root.cursor()
    cur.execute(f"DROP DATABASE IF EXISTS `{MYSQL['database']}`")
    cur.execute(f"CREATE DATABASE `{MYSQL['database']}` CHARACTER SET utf8mb4")
    root.commit(); root.close()

proc = None
def boot():
    global proc
    common = dict(PORT=str(PORT), STORAGE_ROOT=STORAGE, ASPNETCORE_ENVIRONMENT="Development",
                  SEED_DEMO_EXAM="", CANONICAL_HOST="", CANONICAL_REDIRECT="")
    if PROVIDER == "mysql":
        _reset_mysql()
        env = dict(os.environ, DB_PROVIDER="mysql", MYSQL_DATABASE=MYSQL["database"], **common)
        env.pop("MYSQL_CONNECTION_STRING", None)
    else:
        for p in (DB, DB + "-wal", DB + "-shm"):
            if os.path.exists(p): os.remove(p)
        env = dict(os.environ, DB_PROVIDER="sqlite", DATABASE_FILE=DB, **common)
    proc = subprocess.Popen([os.environ.get("DOTNET", "dotnet"), DLL], cwd=BACKEND, env=env,
                            stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    for _ in range(240):
        if req("GET", "/api/health")[0] == 200: return
        time.sleep(0.5)
    raise SystemExit("backend did not become healthy")

def shutdown():
    if proc:
        proc.terminate()
        try: proc.wait(timeout=15)
        except Exception: proc.kill()

def clear_must_change(token, newpw="Op3rator!Pw"):
    js("/api/admin/me/password", "POST", {"password": newpw},
       {"Authorization": f"Bearer {token}"})
    return {"Authorization": f"Bearer {token}"}

def owner_headers():
    code, b = js("/api/admin/auth/login", "POST", {"email": "owner@pci.local", "password": "changeme-owner"})
    assert code == 200 and b.get("token"), f"owner login failed: {code} {b}"
    return clear_must_change(b["token"])

def viewer_headers():
    """A non-owner admin. Provisioned through the real Team endpoint so its permissions are
    whatever the product actually grants a viewer, not a hand-built row."""
    O = owner_headers()
    code, b = js("/api/admin/team", "POST",
                 {"email": "wlview@pci.test", "name": "Launch Viewer", "role": "viewer"}, O)
    assert code == 200, f"team create failed: {code} {b}"
    code, l = js("/api/admin/auth/login", "POST",
                 {"email": "wlview@pci.test", "password": b.get("temp_password", "")})
    assert code == 200 and l.get("token"), f"viewer login failed: {code} {l}"
    return {"Authorization": f"Bearer {l['token']}"}

FLAGS = ["world_community_enabled", "pciworld_forum_enabled", "pciworld_careers_enabled",
         "pciworld_contributors_enabled", "pciworld_community_images_enabled"]


def main():
    boot()
    try:
        O = owner_headers()

        # ── The board reports the shipped-dark state truthfully ───────────────────────────
        code, board = js("/api/admin/world-launch", headers=O)
        rows = board.get("rows") or []
        chk("C01 the board lists every World feature flag",
            code == 200 and sorted(r["flag"] for r in rows) == sorted(FLAGS), f"{code} {[r.get('flag') for r in rows]}")
        chk("C02 every feature reports off on a fresh install",
            all(r["on"] is False for r in rows), str([(r["flag"], r["on"]) for r in rows]))
        chk("C03 and the database agrees — the board is not reporting a default it invented",
            all(setting(f) in ("0", None) for f in FLAGS), str({f: setting(f) for f in FLAGS}))

        by_flag = {r["flag"]: r for r in rows}

        # Every feature names the URL it turns on. Without this the board is a list of settings
        # keys again, which is the thing it exists to stop being.
        chk("C04 each feature states what it turns on and where",
            all(r.get("url") and r.get("turns_on") for r in rows), str([(r["flag"], r.get("url")) for r in rows]))

        # And what that address does WHILE OFF, per feature. The server-rendered routes really 404;
        # the React shell at /world-app/* answers 200 and gates itself on the API. The board said
        # "returns 404" for all five, which was wrong for three of them — and wrong in the direction
        # that makes a correctly-deployed site look broken.
        chk("C04b each feature says what its address does while the feature is off",
            all(r.get("when_off") for r in rows), str([(r["flag"], r.get("when_off")) for r in rows]))
        shell_404 = [r["flag"] for r in rows if r["url"].startswith("/world-app/") and "returns 404" in (r["when_off"] or "").lower()[:40]]
        chk("C04c and does not claim the React shell 404s, because it does not", not shell_404, str(shell_404))

        # ── Authorisation ─────────────────────────────────────────────────────────────────
        code, _ = js("/api/admin/world-launch")
        chk("C05 anonymous cannot read the board", code in (401, 403), f"got {code}")

        V = viewer_headers()
        code, _ = js("/api/admin/world-launch", headers=V)
        chk("C06 a non-owner admin cannot read the board", code == 403, f"got {code}")

        code, _ = js("/api/admin/world-launch", "POST", {"flag": "pciworld_forum_enabled", "on": True}, V)
        chk("C07 a non-owner admin cannot switch a feature on", code == 403, f"got {code}")
        chk("C08 and the flag did not move", setting("pciworld_forum_enabled") in ("0", None), setting("pciworld_forum_enabled"))

        # ── The endpoint is not a general settings write ──────────────────────────────────
        # An owner-gated POST that takes a key and a value is one careless line away from being a
        # write to any setting at all — including the ones the generic PATCH scopes by prefix.
        before = setting("exam_pass_mark_pct")
        code, _ = js("/api/admin/world-launch", "POST", {"flag": "exam_pass_mark_pct", "on": True}, O)
        chk("C09 an owner cannot write a NON-World setting through this endpoint", code == 400, f"got {code}")
        chk("C10 and that setting is untouched", setting("exam_pass_mark_pct") == before,
            f"{before!r} -> {setting('exam_pass_mark_pct')!r}")

        code, _ = js("/api/admin/world-launch", "POST", {"flag": "pciworld_forum_enabled"}, O)
        chk("C11 a POST with no on/off is rejected rather than guessed", code == 400, f"got {code}")

        # ── A feature with no third-party obligation switches on directly ─────────────────
        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_forum_enabled", "on": True}, O)
        chk("C12 the owner can switch the forum on", code == 200 and r.get("on") is True, f"{code} {r}")
        chk("C13 and the flag really moved in the database", setting("pciworld_forum_enabled") == "1", setting("pciworld_forum_enabled"))

        # The flag is not decoration: the public route must actually answer now. A launch board
        # that reports 'on' while the feature still 404s is worse than no board.
        chk("C14 the public forum, which 404'd on boot, now serves",
            req("GET", "/world/forum")[0] == 200, req("GET", "/world/forum")[0])

        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_forum_enabled", "on": False}, O)
        chk("C15 and it switches back off", code == 200 and setting("pciworld_forum_enabled") == "0", f"{code} {r}")
        chk("C16 the public route 404s again once off", req("GET", "/world/forum")[0] == 404, req("GET", "/world/forum")[0])

        # ── The prerequisite gate ─────────────────────────────────────────────────────────
        img = by_flag["pciworld_community_images_enabled"]
        chk("C17 the image feature declares a prerequisite", img.get("requires") is not None, str(img))
        chk("C18 which is not recorded on a fresh install",
            (img.get("requires") or {}).get("recorded") is None, str(img.get("requires")))
        chk("C19 so the board reports it cannot be enabled", img.get("can_enable") is False, str(img))

        # Rooms first, so the refusal below is about the prerequisite and not the dependency.
        js("/api/admin/world-launch", "POST", {"flag": "world_community_enabled", "on": True}, O)
        chk("C20 community rooms switch on without a prerequisite", setting("world_community_enabled") == "1",
            setting("world_community_enabled"))

        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": True}, O)
        chk("C21 the OWNER is refused the image flag while the prerequisite is unrecorded",
            code == 409 and r.get("error") == "prerequisite_not_recorded", f"{code} {r}")
        chk("C22 and the flag did not move — the refusal is real, not a warning",
            setting("pciworld_community_images_enabled") in ("0", None), setting("pciworld_community_images_enabled"))

        # The refusal must say WHICH prerequisite, or it is an error message nobody can act on.
        chk("C23 the refusal names the prerequisite key",
            r.get("requires_key") == "pciworld_ack_image_moderation", str(r))

        # ── Recording the prerequisite ────────────────────────────────────────────────────
        code, r = js("/api/admin/world-launch/ack", "POST",
                     {"key": "pciworld_ack_image_moderation", "note": "x"}, O)
        chk("C24 an empty-ish acknowledgement is rejected — the record has to say something",
            code == 400, f"{code} {r}")
        chk("C25 and nothing was recorded", setting("pciworld_ack_image_moderation") in ("", None),
            repr(setting("pciworld_ack_image_moderation")))

        code, r = js("/api/admin/world-launch/ack", "POST",
                     {"key": "pciworld_ack_image_moderation",
                      "note": "Contract MOD-2026-114 with Hive; counsel settled 16+ and EU/UK only."}, O)
        chk("C26 a substantive acknowledgement is accepted", code == 200, f"{code} {r}")

        rec = setting("pciworld_ack_image_moderation")
        parsed = json.loads(rec) if rec else {}
        chk("C27 it records WHO recorded it", parsed.get("by") == "owner@pci.local", str(parsed))
        chk("C28 and WHEN", bool(parsed.get("at")), str(parsed))
        chk("C29 and what they said", "MOD-2026-114" in (parsed.get("note") or ""), str(parsed))

        rows = sql("SELECT action, detail FROM pciworld_audit WHERE action=?", ("world_launch_ack",))
        chk("C30 the acknowledgement is written to the append-only audit", len(rows) >= 1, str(rows))

        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": True}, O)
        chk("C31 with the prerequisite recorded, the image flag moves",
            code == 200 and setting("pciworld_community_images_enabled") == "1", f"{code} {r}")

        # ── Withdrawing an acknowledgement re-locks the feature ───────────────────────────
        js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": False}, O)
        code, _ = js("/api/admin/world-launch/ack", "POST",
                     {"key": "pciworld_ack_image_moderation", "withdraw": True}, O)
        chk("C32 an acknowledgement can be withdrawn", code == 200, f"got {code}")
        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": True}, O)
        chk("C33 and the feature locks again", code == 409, f"{code} {r}")
        chk("C34 the withdrawal is audited too — both halves of the decision survive",
            len(sql("SELECT id FROM pciworld_audit WHERE action=?", ("world_launch_ack_withdraw",))) >= 1)

        code, _ = js("/api/admin/world-launch/ack", "POST",
                     {"key": "exam_pass_mark_pct", "note": "trying to acknowledge something else"}, O)
        chk("C35 an acknowledgement cannot be recorded against an arbitrary settings key",
            code == 400, f"got {code}")

        # ── The dependency gate ───────────────────────────────────────────────────────────
        js("/api/admin/world-launch/ack", "POST",
           {"key": "pciworld_ack_image_moderation", "note": "Contract MOD-2026-114 reinstated."}, O)
        js("/api/admin/world-launch", "POST", {"flag": "world_community_enabled", "on": False}, O)
        code, r = js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": True}, O)
        chk("C36 images cannot be switched on while rooms are closed", code == 409 and r.get("error") == "dependency_off", f"{code} {r}")
        chk("C37 and again the flag did not move", setting("pciworld_community_images_enabled") in ("0", None),
            setting("pciworld_community_images_enabled"))

        # Turning rooms OFF while images are on leaves an inert flag reading '1'. The endpoint must
        # SAY so rather than silently flipping a flag nobody asked it to touch.
        js("/api/admin/world-launch", "POST", {"flag": "world_community_enabled", "on": True}, O)
        js("/api/admin/world-launch", "POST", {"flag": "pciworld_community_images_enabled", "on": True}, O)
        code, r = js("/api/admin/world-launch", "POST", {"flag": "world_community_enabled", "on": False}, O)
        chk("C38 switching rooms off reports what is left enabled-but-inert",
            code == 200 and r.get("orphaned") and "Images in community rooms" in r["orphaned"], str(r))

        # ── Careers and contributors carry their own prerequisites ────────────────────────
        for flag, ackkey in (("pciworld_careers_enabled", "pciworld_ack_careers_privacy"),
                             ("pciworld_contributors_enabled", "pciworld_ack_contributor_terms")):
            code, r = js("/api/admin/world-launch", "POST", {"flag": flag, "on": True}, O)
            chk(f"C39 {flag} refuses until its prerequisite is recorded",
                code == 409 and r.get("requires_key") == ackkey, f"{code} {r}")
            chk(f"C40 {flag} did not move", setting(flag) in ("0", None), setting(flag))
            js("/api/admin/world-launch/ack", "POST",
               {"key": ackkey, "note": "Published at https://example.test/policy on 2026-07-30."}, O)
            code, _ = js("/api/admin/world-launch", "POST", {"flag": flag, "on": True}, O)
            chk(f"C41 {flag} switches on once recorded", code == 200 and setting(flag) == "1", setting(flag))

        chk("C42 careers, which 404'd on boot, now serves", req("GET", "/world/careers")[0] == 200,
            req("GET", "/world/careers")[0])

        # ── The board is a mirror, not a second source of truth ───────────────────────────
        # A flag changed by any other route must show up here, or two screens will disagree about
        # whether a feature is live.
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_forum_enabled','1')")
        code, board = js("/api/admin/world-launch", headers=O)
        fr = next(r for r in board["rows"] if r["flag"] == "pciworld_forum_enabled")
        chk("C43 the board reflects a flag changed outside it", code == 200 and fr["on"] is True, str(fr))

    finally:
        shutdown()

    print(f"\n{passed} passed, {failed} failed  ({PROVIDER})")
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
