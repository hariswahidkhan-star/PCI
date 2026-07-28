#!/usr/bin/env python3
"""
PCI World contributor publishing (Phase 6) — live HTTP suite.

The phase has exactly one obligation that outranks everything else: **a contributor may never
approve or publish their own article.** So most of this file attacks that from several directions,
because the interesting failure is not that the check is missing — it is that the check EXISTS and
silently never fires.

That is not hypothetical here. The editorial engine's maker-checker compares
`pciworld_articles.author_id` (a `pciworld_admin_users` id) with the acting admin. A contributor is
a `pciworld_users` row. The two id spaces are disjoint, so for a contributor's manuscript the
original comparison could never match: a staff editor who was also the contributor would sail
straight through a control that looks, in code review, exactly like a control. C10–C13 below are the
tests that would have caught that, and they are written to fail if the fix is removed.

The second theme is what a contributor can SEE. Internal review notes and other people's
manuscripts are not filtered out of these responses — they are never in the queries — and a test
that only checked the happy path would not tell the difference.

Runs against SQLite and MySQL (TEST_DB_PROVIDER=mysql). Exit 0 iff every assertion passes.
Run from backend/:  python3 tests/contributors_test.py
"""
import json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_contrib.db")
STORAGE = os.path.join(HERE, "_contrib_storage")
OWNER_PW = "ContribSuiteOwner!99"

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_contrib") else _base + "_contrib")

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
        # forum_test.py documents both traps: `args or None` (pymysql substitutes even for an empty
        # tuple) and the blind ?->%s replacement mangling a '?' inside a string literal.
        try:
            cur.execute(statement.replace("INSERT OR REPLACE INTO", "REPLACE INTO").replace("?", "%s"), args or None)
        except Exception as e:
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

def member(email, pw="ContribMemberPass!1"):
    code, r = js("/api/world/account/register", "POST",
                 {"email": email, "password": pw, "display_name": email.split("@")[0]})
    assert code == 200 and r.get("token"), f"register failed: {code} {r}"
    sql("UPDATE pciworld_users SET email_verified=1 WHERE email=?", (email,))
    return {"X-World-Account": r["token"]}, sql("SELECT id FROM pciworld_users WHERE email=?", (email,))[0][0]

def admin_headers():
    code, r = js("/api/world-admin/auth/login", "POST",
                 {"email": "owner@pciworld.local", "password": OWNER_PW})
    assert code == 200 and r.get("token"), f"world-admin login failed: {code} {r}"
    return {"Authorization": f"Bearer {r['token']}"}

DECL = {"conflict": "none", "sponsorship": "none", "ai_assistance": "none", "originality": "own work"}

# A publishable body. The editorial engine refuses anything under 200 words — "thin pages are
# prohibited by the SEO policy, not merely discouraged" — and that rule applies to contributor
# manuscripts exactly as it does to house content, which is the point. A one-line fixture would
# have tested nothing except that the validator exists.
BODY = (" ".join([
    "Schedule recovery begins with admitting the baseline no longer describes the work.",
    "The temptation is to compress the remaining durations until the arithmetic lands on the",
    "committed date, which produces a plan nobody believes and a forecast nobody can defend.",
    "A recovery plan is instead a re-sequencing argument: which paths actually drive the finish,",
    "what float remains on the rest, and which of the driving activities can be shortened without",
    "moving the risk somewhere less visible. We rebaselined quarterly rather than continuously,",
    "because a baseline that moves every week is a record of optimism rather than a measurement",
    "instrument. Between rebaselines we tracked performance against a blended index instead of a",
    "single cost performance index, since one index conceals the difference between a package that",
    "is late and a package that was mis-estimated. The blended view made that distinction visible",
    "early enough to act on it. None of this is novel and none of it is difficult; what makes it",
    "rare is that it requires saying out loud, in a governance forum, that the committed date is",
    "no longer achievable on the current sequence. Teams that can hold that conversation recover.",
    "Teams that cannot spend the remaining schedule discovering the same fact more expensively.",
]) + " ") * 3


def main():
    boot()
    try:
        A = admin_headers()

        # ── Ships dark ─────────────────────────────────────────────────────────────────────
        code, _ = js("/api/world/contributor/me")
        chk("K01 the contributor surface 404s while the flag is off", code == 404, f"got {code}")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_contributors_enabled','1')")

        WRITER, WRITER_ID = member("writer@people.test")

        # ── Terms must be authored words, not a placeholder ────────────────────────────────
        code, r = js("/api/world/contributor/apply", "POST", {"statement": "I plan schedules."}, WRITER)
        chk("K02 applying is refused while no contributor terms are published",
            code == 503 and r.get("error") == "terms_unavailable", f"{code} {r}")
        n = sql("SELECT COUNT(*) FROM pciworld_contributors")[0][0]
        chk("K03 and nothing is recorded — acceptance of a placeholder is acceptance of nothing", n == 0, str(n))

        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_contributor_terms_version','v1-test')")

        # ── The grant is a human act, never a threshold ───────────────────────────────────
        code, r = js("/api/world/contributor/apply", "POST", {"statement": "I plan schedules."}, WRITER)
        chk("K04 an application is accepted", code == 200 and r.get("state") == "applied", str(r))
        state = sql("SELECT state,terms_version FROM pciworld_contributors WHERE user_id=?", (WRITER_ID,))[0]
        chk("K05 applying does not grant — and the terms they saw are pinned",
            state[0] == "applied" and state[1] == "v1-test", str(state))

        code, r = js("/api/world/contributor/articles", "POST", {"title": "Schedule recovery in practice"}, WRITER)
        chk("K06 an applicant who has not been granted cannot start a manuscript",
            code == 403 and r.get("error") == "not_a_contributor", f"{code} {r}")

        ver = sql("SELECT version FROM pciworld_contributors WHERE user_id=?", (WRITER_ID,))[0][0]
        code, r = js(f"/api/world-admin/editorial/contributors/{WRITER_ID}/decision", "POST",
                     {"decision": "granted", "version": ver}, A)
        chk("K07 an editor grants standing", code == 200, str(r))
        g = sql("SELECT granted_at,granted_by_admin_id FROM pciworld_contributors WHERE user_id=?", (WRITER_ID,))[0]
        chk("K08 and the grant is ATTRIBUTABLE — who and when", g[0] and g[1], str(g))

        code, r = js("/api/world/contributor/articles", "POST",
                     {"title": "Schedule recovery in practice", "dek": "What worked.",
                      "body_md": BODY}, WRITER)
        chk("K09 a granted contributor can start a manuscript", code == 200 and r.get("article_id"), str(r))
        ART = r.get("article_id")

        js(f"/api/world/contributor/articles/{ART}/submit", "POST", {"declarations": DECL}, WRITER)

        # ── THE MAKER-CHECKER ACROSS TWO IDENTITY SYSTEMS ─────────────────────────────────
        # This is the phase's whole obligation. The editorial engine compares author_id (an
        # admin id) with the acting admin; a contributor is a pciworld_users row. Without the
        # explicit link the comparison is between disjoint id spaces and can NEVER fire — a
        # control that reads like a control and does nothing.
        sql("UPDATE pciworld_admin_users SET world_user_id=? WHERE email=?", (WRITER_ID, "owner@pciworld.local"))
        sql("UPDATE pciworld_articles SET status='approved' WHERE id=?", (ART,))

        code, r = js(f"/api/world-admin/articles/{ART}/publish", "POST", None, A)
        chk("K10 an editor who IS the contributor cannot publish their own article",
            code in (400, 403, 409) and "maker_checker" in json.dumps(r), f"{code} {r}")
        st = sql("SELECT status,current_version FROM pciworld_articles WHERE id=?", (ART,))[0]
        chk("K11 and the article really did not publish", st[0] != "published" and st[1] == 0, str(st))

        sql("UPDATE pciworld_articles SET status='technical_review' WHERE id=?", (ART,))
        code, r = js(f"/api/world-admin/articles/{ART}/approve", "POST", None, A)
        chk("K12 nor approve it", code in (400, 403, 409) and "maker_checker" in json.dumps(r), f"{code} {r}")
        code, r = js(f"/api/world-admin/editorial/articles/{ART}/assign", "POST", None, A)
        chk("K13 nor even take the assignment — the conflict is refused where a human would see it",
            code == 403 and r.get("error") == "maker_checker", f"{code} {r}")

        # With the link cleared, the SAME admin is a different person as far as the record goes.
        # This is the residue the design states plainly rather than pretending code closes it.
        sql("UPDATE pciworld_admin_users SET world_user_id=NULL WHERE email=?", ("owner@pciworld.local",))
        code, r = js(f"/api/world-admin/editorial/articles/{ART}/assign", "POST", None, A)
        chk("K14 an undeclared link is not blocked by code — the design says so; policy owns it",
            code == 200, f"{code} {r}")

        # ── Publication needs current standing, checked at publish and not only at approve ──
        code, r = js(f"/api/world-admin/articles/{ART}/approve", "POST", None, A)
        chk("K15 an unconflicted editor can approve", code == 200, str(r))
        ver = sql("SELECT version FROM pciworld_contributors WHERE user_id=?", (WRITER_ID,))[0][0]
        code, r = js(f"/api/world-admin/editorial/contributors/{WRITER_ID}/decision", "POST",
                     {"decision": "revoked", "version": ver}, A)
        chk("K16 revoking without a reason is refused — an unexplained revocation cannot be appealed",
            code == 400 and r.get("error") == "reason_required", f"{code} {r}")
        code, r = js(f"/api/world-admin/editorial/contributors/{WRITER_ID}/decision", "POST",
                     {"decision": "revoked", "reason": "editorial standards", "version": ver}, A)
        chk("K17 revocation with a reason is recorded", code == 200, str(r))

        code, r = js(f"/api/world-admin/articles/{ART}/publish", "POST", None, A)
        chk("K18 an approved article does NOT publish once its author's standing is gone",
            code in (400, 403, 409) and "contributor_not_eligible" in json.dumps(r), f"{code} {r}")

        # ── Revocation is not retroactive content destruction ─────────────────────────────
        WRITER2, WRITER2_ID = member("writer2@people.test")
        js("/api/world/contributor/apply", "POST", {"statement": "Cost engineering."}, WRITER2)
        v2 = sql("SELECT version FROM pciworld_contributors WHERE user_id=?", (WRITER2_ID,))[0][0]
        js(f"/api/world-admin/editorial/contributors/{WRITER2_ID}/decision", "POST",
           {"decision": "granted", "version": v2}, A)
        code, r = js("/api/world/contributor/articles", "POST",
                     {"title": "Earned value without the theatre", "dek": "Plain measurement.",
                      "body_md": BODY}, WRITER2)
        ART2 = r.get("article_id")
        js(f"/api/world/contributor/articles/{ART2}/submit", "POST", {"declarations": DECL}, WRITER2)
        sql("UPDATE pciworld_articles SET status='approved' WHERE id=?", (ART2,))
        code, r = js(f"/api/world-admin/articles/{ART2}/publish", "POST", None, A)
        chk("K19 an eligible contributor's approved article publishes", code == 200, str(r))
        v2 = sql("SELECT version FROM pciworld_contributors WHERE user_id=?", (WRITER2_ID,))[0][0]
        js(f"/api/world-admin/editorial/contributors/{WRITER2_ID}/decision", "POST",
           {"decision": "revoked", "reason": "conduct", "version": v2}, A)
        st = sql("SELECT status FROM pciworld_articles WHERE id=?", (ART2,))[0][0]
        chk("K20 revoking standing does NOT unpublish already published work — conduct is not content",
            st == "published", st)

        # ── A published article is never edited in place ──────────────────────────────────
        code, r = js(f"/api/world/contributor/articles/{ART2}", "PATCH", {"body_md": "rewritten silently"}, WRITER2)
        chk("K21 the contributor cannot edit a published article", code == 409, f"{code} {r}")
        body = sql("SELECT body_md FROM pciworld_articles WHERE id=?", (ART2,))[0][0]
        chk("K22 and the text is untouched", "rewritten silently" not in (body or ""), str(body)[:80])
        vcount = sql("SELECT COUNT(*) FROM pciworld_article_versions WHERE article_id=?", (ART2,))[0][0]
        chk("K23 the published snapshot exists as an immutable version row", vcount == 1, str(vcount))

        # ── What a contributor can see ───────────────────────────────────────────────────
        code, r = js(f"/api/world/contributor/articles/{ART2}", "GET", None, WRITER)
        chk("K24 one contributor cannot read another's manuscript", code == 404, f"got {code}")
        code, r = js("/api/world/contributor/articles", "GET", None, WRITER)
        ids = [a["id"] for a in r.get("articles", [])]
        chk("K25 nor see it in their own list", ART2 not in ids, str(ids))

        sql("INSERT INTO pciworld_article_reviews(article_id,kind,reviewer_id,outcome,note)"
            " VALUES(?,'technical',1,'fail',?)", (ART2, "internal: weak evidence, do not tell the author"))
        code, r = js(f"/api/world/contributor/articles/{ART2}", "GET", None, WRITER2)
        chk("K26 a contributor never sees internal review notes — they are not in the query at all",
            code == 200 and "do not tell the author" not in json.dumps(r), "internal note leaked")

        # ── Declarations freeze at submission ────────────────────────────────────────────
        ev = sql("SELECT declarations_json FROM pciworld_contributor_events WHERE article_id=? AND event='submitted'", (ART2,))
        chk("K27 what was declared AT SUBMISSION is frozen into the event",
            ev and ev[0][0] and "originality" in ev[0][0], str(ev))
        code, r = js(f"/api/world/contributor/articles/{ART}/submit", "POST", {}, WRITER)
        chk("K28 submitting without declarations is refused", code in (400, 403), f"{code} {r}")

        # ── Standing cannot be self-restored ────────────────────────────────────────────
        code, r = js("/api/world/contributor/apply", "POST", {"statement": "let me back in"}, WRITER2)
        chk("K29 a revoked contributor cannot re-apply their way back in",
            code == 409 and r.get("error") == "revoked", f"{code} {r}")
        rows = sql("SELECT COUNT(*) FROM pciworld_contributors WHERE user_id=?", (WRITER2_ID,))[0][0]
        chk("K30 and there is still exactly one standing row for them", rows == 1, str(rows))

        # ── The link is owner-only ──────────────────────────────────────────────────────
        code, r = js("/api/world-admin/editorial/admins/1/world-link", "POST", {"world_user_id": WRITER_ID}, A)
        chk("K31 an owner can declare an admin's World account", code == 200, str(r))
        code, r = js("/api/world-admin/editorial/admins/1/world-link", "POST", {"world_user_id": 999999}, A)
        chk("K32 pointing it at a non-existent account is refused", code == 404, f"{code} {r}")

        # ── The flag is a kill switch ───────────────────────────────────────────────────
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_contributors_enabled','0')")
        code, _ = js("/api/world/contributor/articles", "GET", None, WRITER2)
        chk("K33 turning the flag off closes the surface", code == 404, f"got {code}")
        n = sql("SELECT COUNT(*) FROM pciworld_articles WHERE contributor_user_id IS NOT NULL")[0][0]
        chk("K34 and touches no existing manuscripts", n >= 2, str(n))

    finally:
        shutdown()

    print(f"\n{passed} passed, {failed} failed  [{PROVIDER}]")
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
