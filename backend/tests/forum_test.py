#!/usr/bin/env python3
"""
PCI World forum (Phase 3) — live HTTP suite.

Boots the REAL built backend and drives the real routes: the authenticated composer, the
server-rendered crawlable page, the moderation queue, and the member flag.

The assertions that carry the weight are the ones about WHAT REACHES THE CRAWLABLE PAGE, because
that output is cached by search engines: a post that escapes onto it can outlive the deletion that
was supposed to fix it. So most of what follows writes something and then fetches the public HTML to
check whether it is there.

The other group is the flag. A flag is a member's opinion, and the whole design rests on it being
unable to do anything except hide a post pending human review — never sanction, never delete, and
never on one account's say-so.

Exit code 0 iff every assertion passes.  Run from backend/:  python3 tests/forum_test.py
"""
import datetime, json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_forum.db")
STORAGE = os.path.join(HERE, "_forum_storage")
OWNER_PW = "ForumSuiteOwner!99"

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_forum") else _base + "_forum")

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
        with urllib.request.urlopen(r) as resp: return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e: return e.code, e.read().decode()
    except urllib.error.URLError as e: return 0, str(e)

def js(path, method="GET", body=None, headers=None):
    code, text = req(method, path, body, headers)
    try: return code, json.loads(text) if text else {}
    except Exception: return code, {"_raw": text}

def sql(statement, args=()):
    if PROVIDER == "mysql":
        import pymysql
        con = pymysql.connect(**MYSQL); cur = con.cursor()
        # Two MySQL-only traps this helper has to dodge, both of which pass silently on SQLite:
        #
        #   1. `args or None`, not `args`. pymysql renders via `query % args` whenever args is not
        #      None, so an EMPTY tuple still triggers substitution and a literal % blows up.
        #   2. The `?` -> `%s` replacement is BLIND. A question mark inside a string literal — a
        #      thread title like 'How do you model SPI recovery?' — silently becomes an extra
        #      placeholder. Keep every literal free of '?' and pass such text as a parameter.
        #      This cost a real debugging cycle; the error ("not enough arguments for format
        #      string") points at pymysql and says nothing about the title.
        try:
            cur.execute(statement.replace("INSERT OR REPLACE INTO", "REPLACE INTO").replace("?", "%s"), args or None)
        except Exception as e:
            # Name the statement. A bare pymysql traceback sends you hunting through the file.
            raise SystemExit(f"SQL FAILED: {statement!r} args={args!r}: {e}")
        con.commit(); rows = cur.fetchall(); con.close(); return rows
    con = sqlite3.connect(DB); cur = con.cursor()
    cur.execute(statement, args); con.commit()
    rows = cur.fetchall(); con.close(); return rows

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
                  PCIWORLD_OWNER_PASSWORD=OWNER_PW, SEED_DEMO_EXAM="", CANONICAL_HOST="", CANONICAL_REDIRECT="")
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

def member(email, pw="ForumMemberPass!1"):
    """Register a world account and return its bearer header. Email verification is set directly —
    the mail loop is WorldAccount's business and is tested there; here it is a precondition."""
    code, r = js("/api/world/account/register", "POST",
                 {"email": email, "password": pw, "display_name": email.split("@")[0]})
    assert code == 200 and r.get("token"), f"register failed: {code} {r}"
    sql("UPDATE pciworld_users SET email_verified=1 WHERE email=?", (email,))
    return {"X-World-Account": r["token"]}

def uid(email):
    return sql("SELECT id FROM pciworld_users WHERE email=?", (email,))[0][0]

def promote(email, accepted=10, days=30, flags=5):
    """Set a member's recorded standing directly. The ladder itself is unit-tested; what this suite
    needs is an account AT a level, without waiting thirty days for one."""
    u = uid(email)
    # The timestamp is computed in PYTHON, not with datetime('now','-90 days'). SQLite's date
    # arithmetic is not MySQL's, and this suite's raw SQL gets no translation — the same class of
    # mistake the backend avoids by using H.StampInMinutes instead of literal modifiers.
    since = (datetime.datetime.now(datetime.timezone.utc)
             - datetime.timedelta(days=90)).strftime("%Y-%m-%d %H:%M:%S")
    sql("INSERT OR REPLACE INTO pciworld_forum_standing(user_id,level,accepted_posts,accurate_flags,first_post_at)"
        " VALUES(?,'member',?,?,?)", (u, accepted, flags, since))

def main():
    boot()
    try:
        # ── Ships dark ─────────────────────────────────────────────────────────────────────
        code, _ = js("/api/world/forum/categories")
        chk("F01 the forum surface 404s while the flag is off", code == 404, f"got {code}")

        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_forum_enabled','1')")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_moderator','deterministic')")
        sql("INSERT INTO pciworld_forum_categories(slug,title,min_trust_to_post,state)"
            " VALUES('estimating','Estimating','new','open')")
        cat = sql("SELECT id FROM pciworld_forum_categories WHERE slug='estimating'")[0][0]
        sql("INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id,state)"
            " VALUES(?,'spi-recovery',?,1,'open')", (cat, "How do you model SPI recovery?"))
        thread = sql("SELECT id FROM pciworld_forum_threads WHERE slug='spi-recovery'")[0][0]

        # A hidden category exists alongside the open one, so "the list is public" is checked
        # against something that must NOT appear rather than only against a count. The suite used
        # to assert exactly one category, which quietly encoded "nothing is seeded"; the forum now
        # ships a starting taxonomy, and a count assertion would have said the endpoint broke when
        # what changed was the fixture.
        sql("INSERT INTO pciworld_forum_categories(slug,title,min_trust_to_post,state)"
            " VALUES('backstage','Backstage','new','hidden')")

        code, cats = js("/api/world/forum/categories")
        listed = {c["slug"]: c for c in cats.get("categories", [])}
        chk("F02 the category list is public and carries the open category",
            code == 200 and "estimating" in listed, str(cats))
        chk("F02b and a hidden category is not in it", "backstage" not in listed, str(sorted(listed)))
        chk("F03 and it carries min_trust so a composer can explain itself",
            listed.get("estimating", {}).get("min_trust") == "new", str(cats))

        # ── Writing needs a Passport session ───────────────────────────────────────────────
        code, _ = js(f"/api/world/forum/threads/{thread}/posts", "POST", {"body": "anonymous attempt"})
        chk("F04 posting without an account is refused", code == 401, f"got {code}")

        NEW = member("newcomer@forum.test")
        code, me = js("/api/world/forum/me", headers=NEW)
        chk("F05 a fresh account is told its posts are reviewed first",
            code == 200 and me.get("level") == "new" and me.get("posts_are_reviewed_first") is True, str(me))

        # ── A new account's post is held, and is NOT crawlable ─────────────────────────────
        code, r = js(f"/api/world/forum/threads/{thread}/posts", "POST",
                     {"body": "Float on activity B is eight days on our job."}, NEW)
        chk("F06 a new account's post is accepted but held",
            code == 200 and r.get("state") == "pending" and r.get("published") is False, str(r))
        held_id = r.get("post_id")

        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F07 a thread with nothing published is not a page at all", code == 404, f"got {code}")

        # ── A member publishes immediately ─────────────────────────────────────────────────
        MEM = member("member@forum.test"); promote("member@forum.test")
        code, r = js(f"/api/world/forum/threads/{thread}/posts", "POST",
                     {"body": "We rebaselined and tracked SPI weekly after that."}, MEM)
        chk("F08 an established member publishes immediately",
            code == 200 and r.get("published") is True, str(r))

        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F09 the published post is on the crawlable page",
            code == 200 and "rebaselined" in html, f"{code}")
        chk("F10 the HELD post is NOT on the crawlable page — the assertion this suite exists for",
            "Float on activity B" not in html, "a held post leaked onto a cacheable page")
        chk("F11 the page carries structured data", "DiscussionForumPosting" in html)

        # ── Refused words are refused whatever the standing ────────────────────────────────
        code, r = js(f"/api/world/forum/threads/{thread}/posts", "POST",
                     {"body": "you are an idiot and a loser"}, MEM)
        chk("F12 refused words are refused for an established member too",
            code == 200 and r.get("published") is False, str(r))
        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F13 and they never reach the crawlable page", "idiot" not in html)

        # ── Links ──────────────────────────────────────────────────────────────────────────
        code, r = js(f"/api/world/forum/threads/{thread}/posts", "POST",
                     {"body": "See https://spam.example.com for cheap certs"}, NEW)
        chk("F14 a brand-new account cannot post links",
            code == 400 and r.get("error") == "links_not_allowed_yet", str(r))

        # ── Moderation: release ────────────────────────────────────────────────────────────
        code, login = js("/api/world-admin/auth/login", "POST",
                         {"email": "owner@pciworld.local", "password": OWNER_PW})
        OWNER = {"Authorization": "Bearer " + login["token"]} if login.get("token") else {}
        chk("F15 the world admin signs in", code == 200 and login.get("token"), f"{code} {login}")

        code, q = js("/api/world-admin/forum/queue", headers=OWNER)
        ids = [p["id"] for p in q.get("posts", [])]
        chk("F16 the held post is in the review queue", code == 200 and held_id in ids, str(q)[:250])

        code, _ = js("/api/world-admin/forum/queue")
        chk("F17 the queue is not readable without a token", code == 401)

        code, r = js(f"/api/world-admin/forum/posts/{held_id}/release", "POST", {"reason": "fine"}, OWNER)
        chk("F18 a moderator can release a held post", code == 200 and r.get("state") == "published", str(r))

        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F19 the released post becomes crawlable", "Float on activity B" in html)

        rows = sql("SELECT accepted_posts FROM pciworld_forum_standing WHERE user_id=?", (uid("newcomer@forum.test"),))
        chk("F20 releasing credits the author's standing — otherwise nobody ever leaves pre-moderation",
            rows and rows[0][0] >= 1, str(rows))

        rows = sql("SELECT COUNT(*) FROM pciworld_moderation_decisions WHERE scope='forum_post' AND outcome='allow'")
        chk("F21 a release is recorded as a decision, not just a state change", rows and rows[0][0] >= 1, str(rows))

        # ── Moderation: withhold ───────────────────────────────────────────────────────────
        code, r = js(f"/api/world-admin/forum/posts/{held_id}/withhold", "POST", {}, OWNER)
        chk("F22 a takedown without a stated reason is refused",
            code == 400 and r.get("error") == "reason_required", str(r))

        code, r = js(f"/api/world-admin/forum/posts/{held_id}/withhold", "POST",
                     {"reason": "off topic for this category"}, OWNER)
        chk("F23 a moderator can withhold with a reason", code == 200, str(r))
        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F24 a withheld post leaves the crawlable page", "Float on activity B" not in html)

        # ── Flags ──────────────────────────────────────────────────────────────────────────
        target = sql("SELECT id FROM pciworld_forum_posts WHERE state='published' ORDER BY id LIMIT 1")[0][0]

        code, r = js(f"/api/world/forum/posts/{target}/flag", "POST", {"reason": "low quality"}, NEW)
        chk("F25 a brand-new account may not flag",
            code == 403 and r.get("error") == "flagging_not_allowed_yet", f"{code} {r}")

        code, r = js(f"/api/world/forum/posts/{target}/flag", "POST", {"reason": "mine"}, MEM)
        chk("F26 nobody can flag their own post",
            code == 400 and r.get("error") == "cannot_flag_your_own_post", f"{code} {r}")

        F1 = member("flagger1@forum.test"); promote("flagger1@forum.test")
        code, r = js(f"/api/world/forum/posts/{target}/flag", "POST", {"reason": "low quality"}, F1)
        chk("F27 an established member may flag", code == 200 and r.get("ok"), str(r))
        chk("F28 the response reveals neither the running total nor the threshold",
            not any(k in json.dumps(r) for k in ("weight", "threshold", "total")), str(r))

        code, r = js(f"/api/world/forum/posts/{target}/flag", "POST", {"reason": "again"}, F1)
        chk("F29 a repeated flag is idempotent rather than an error",
            code == 200 and r.get("already_flagged") is True, str(r))

        rows = sql("SELECT state FROM pciworld_forum_posts WHERE id=?", (target,))
        chk("F30 ONE member's flags cannot hide a post — the cap is the whole control",
            rows[0][0] == "published", f"state={rows[0][0]}")

        code, html = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F31 and the post is still readable", "rebaselined" in html)

        # Enough distinct members to cross the threshold. Weight 1 each at 'member', threshold 5.
        for i in range(2, 7):
            e = f"flagger{i}@forum.test"
            H_ = member(e); promote(e)
            js(f"/api/world/forum/posts/{target}/flag", "POST", {"reason": "low quality"}, H_)

        rows = sql("SELECT state FROM pciworld_forum_posts WHERE id=?", (target,))
        chk("F32 enough independent members DO hide it, pending review", rows[0][0] == "pending", f"state={rows[0][0]}")

        rows = sql("SELECT COUNT(*) FROM pciworld_forum_standing_events WHERE reason='flagged'")
        chk("F33 flagging sanctions nobody — no standing event was written", rows[0][0] == 0, str(rows))
        rows = sql("SELECT level FROM pciworld_forum_standing WHERE user_id=?", (uid("member@forum.test"),))
        chk("F34 and the author's standing is untouched", rows[0][0] == "member", str(rows))

        code, _ = req("GET", "/world/forum/estimating/spi-recovery")
        chk("F35 a hidden post leaves the crawlable page immediately", code == 404, f"got {code}")

        # ── Starting a thread ──────────────────────────────────────────────────────────────
        code, r = js("/api/world/forum/categories/estimating/threads", "POST",
                     {"title": "Short", "body": "A body long enough to pass."}, MEM)
        chk("F39 a too-short title is refused", code == 400 and r.get("error") == "bad_title", str(r))

        code, r = js("/api/world/forum/categories/estimating/threads", "POST",
                     {"title": "Modelling escalation on a five-year job", "body": ""}, MEM)
        chk("F40 a thread with no opening post is refused", code == 400, str(r))
        rows = sql("SELECT COUNT(*) FROM pciworld_forum_threads WHERE title=?",
                   ("Modelling escalation on a five-year job",))
        chk("F41 and no empty thread is left behind — a title with nothing under it is not a discussion",
            rows[0][0] == 0, str(rows))

        code, r = js("/api/world/forum/categories/estimating/threads", "POST",
                     {"title": "Modelling escalation on a five-year job",
                      "body": "We used a blended index rather than a single CPI series."}, MEM)
        chk("F42 a member can start a thread",
            code == 200 and r.get("published") is True and r.get("slug"), str(r))
        chk("F43 the slug is derived from the title",
            r.get("slug") == "modelling-escalation-on-a-five-year-job", str(r))

        code, html = req("GET", r["url"])
        chk("F44 the new thread is immediately crawlable at its own URL",
            code == 200 and "blended index" in html, f"{code}")

        code, r2 = js("/api/world/forum/categories/estimating/threads", "POST",
                      {"title": "Modelling escalation on a five-year job",
                       "body": "A different discussion that happens to share a title."}, MEM)
        chk("F45 a duplicate title gets a distinct slug rather than a refusal",
            code == 200 and r2.get("slug") != r.get("slug"), str(r2))

        code, r3 = js("/api/world/forum/categories/estimating/threads", "POST",
                      {"title": "A perfectly reasonable question about float",
                       "body": "Float on activity B keeps moving between updates."}, NEW)
        chk("F46 a new account's thread is held, not published",
            code == 200 and r3.get("published") is False, str(r3))
        code, _ = req("GET", r3["url"])
        chk("F47 and it leaves no crawlable husk behind while held", code == 404, f"got {code}")

        # ── Category floor ─────────────────────────────────────────────────────────────────
        code, r = js("/api/world-admin/forum/categories", "POST",
                     {"slug": "announcements", "title": "Announcements", "min_trust_to_post": "trusteed"}, OWNER)
        chk("F36 a mistyped trust level is refused rather than defaulted to open",
            code == 400 and r.get("error") == "unknown_trust_level", str(r))

        code, r = js("/api/world-admin/forum/categories", "POST",
                     {"slug": "announcements", "title": "Announcements", "min_trust_to_post": "trusted"}, OWNER)
        chk("F37 a category can be created with a trust floor", code == 200 and r.get("ok"), str(r))

        acat = sql("SELECT id FROM pciworld_forum_categories WHERE slug='announcements'")[0][0]
        sql("INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id,state)"
            " VALUES(?,'notice','A notice',1,'open')", (acat,))
        athread = sql("SELECT id FROM pciworld_forum_threads WHERE slug='notice'")[0][0]

        code, r = js(f"/api/world/forum/threads/{athread}/posts", "POST",
                     {"body": "Replying to an announcement."}, MEM)
        chk("F38 the floor applies to REPLIES too — otherwise it is trivially bypassed",
            code == 400 and r.get("error") == "insufficient_trust_for_category", f"{code} {r}")

        code, r = js("/api/world/forum/categories/announcements/threads", "POST",
                     {"title": "Trying to start a thread here", "body": "Should not be allowed."}, MEM)
        chk("F48 and to starting a thread, before any thread row is created",
            code == 403 and r.get("error") == "insufficient_trust_for_category", f"{code} {r}")
        rows = sql("SELECT COUNT(*) FROM pciworld_forum_threads WHERE slug LIKE ?", ("trying-to-start%",))
        chk("F49 no orphan thread row was left by the refusal", rows[0][0] == 0, str(rows))

        # ── The front door ───────────────────────────────────────────────────────────────
        # Added after somebody asked where the forum actually was. Phase 3 shipped thread pages,
        # their JSON-LD and their accessibility coverage, and no index — so a thread was reachable
        # only by already knowing its URL. Every test passed the whole time, because a suite asks
        # whether the thing works, not whether anyone can find it.
        code, raw = req("GET", "/world/forum")
        html = raw.decode(errors="replace") if isinstance(raw, bytes) else raw
        chk("F60 the forum has an index page", code == 200, f"got {code}")
        chk("F61 which lists the category", "Estimating" in html, "category missing from the index")
        chk("F62 and links to a published thread", "/world/forum/estimating/" in html, "no thread link")
        chk("F63 as a complete crawlable document",
            "<!DOCTYPE html" in html and 'rel="canonical"' in html, "not a whole document")

    finally:
        shutdown()

    print(f"\n  [{PROVIDER}] ══ {passed}/{passed + failed} PASSED ══" if not failed
          else f"\n  [{PROVIDER}] {passed} passing, {failed} FAILING")
    sys.exit(1 if failed else 0)

if __name__ == "__main__":
    main()
