#!/usr/bin/env python3
"""
PCI World Careers (Phase 4) — live HTTP suite.

Boots the REAL built backend and drives the real routes. The design doc's §9 test plan calls this
"the phase's whole point", and the reason is that every serious failure mode here is a DISCLOSURE,
not a crash: a fake employer that publishes, a suspended employer that keeps reading, one tenant
seeing another's applicants, a CV served after consent was withdrawn. None of those throw. All of
them return 200 with a body, which is exactly why they need assertions rather than review.

So the suite is organised around the four things §2 says must never happen, and each is probed by
MORE THAN ONE ROUTE — the publish transition, a re-publish after suspension, the public read path —
because a control that exists on the happy path and not on the second route is the defect this
whole increment keeps finding.

Runs against SQLite and MySQL (TEST_DB_PROVIDER=mysql). Exit code 0 iff every assertion passes.
Run from backend/:  python3 tests/careers_test.py
"""
import base64, json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_careers.db")
STORAGE = os.path.join(HERE, "_careers_storage")
OWNER_PW = "CareersSuiteOwner!99"

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_careers") else _base + "_careers")

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
        # Same two MySQL-only traps documented in forum_test.py: `args or None` (pymysql renders
        # via `query % args` even for an empty tuple), and the blind ?->%s replacement, which turns
        # a '?' inside a string literal into an extra placeholder. Keep literals free of '?'.
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

def member(email, pw="CareersMemberPass!1"):
    code, r = js("/api/world/account/register", "POST",
                 {"email": email, "password": pw, "display_name": email.split("@")[0]})
    assert code == 200 and r.get("token"), f"register failed: {code} {r}"
    sql("UPDATE pciworld_users SET email_verified=1 WHERE email=?", (email,))
    return {"X-World-Account": r["token"]}

def admin_headers():
    code, r = js("/api/world-admin/auth/login", "POST",
                 {"email": "owner@pciworld.local", "password": OWNER_PW})
    assert code == 200 and r.get("token"), f"world-admin login failed: {code} {r}"
    return {"Authorization": f"Bearer {r['token']}"}

# A minimal but genuinely well-formed PDF. The point is that it passes a MAGIC-BYTE sniff, so a
# test that swaps it for a renamed executable exercises the real rejection rather than a filename
# check that would have let the executable through.
PDF = base64.b64encode(
    b"%PDF-1.4\n1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj\n"
    b"2 0 obj<</Type/Pages/Count 0/Kids[]>>endobj\ntrailer<</Root 1 0 R>>\n%%EOF\n").decode()
EXE = base64.b64encode(b"MZ\x90\x00" + b"\x00" * 128).decode()


def main():
    boot()
    try:
        A = admin_headers()

        # ── Ships dark ─────────────────────────────────────────────────────────────────────
        code, _ = js("/api/world/careers/employers", "POST", {"name": "Acme"})
        chk("C01 the careers surface 404s while the flag is off", code == 404, f"got {code}")

        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_careers_enabled','1')")

        # ── An employer is a tenant, and starts unverified ─────────────────────────────────
        BOSS = member("boss@acme.test")
        code, emp = js("/api/world/careers/employers", "POST",
                       {"name": "Acme Controls", "website": "https://acme.example", "slug": "acme"}, BOSS)
        chk("C02 an employer can be created", code == 200 and emp.get("employer_id"), str(emp))
        EMP = emp.get("employer_id")
        chk("C03 and it is created in draft, never verified", emp.get("state") == "draft", str(emp))

        # No route may mint the assertion PCI makes to candidates.
        code, _ = js("/api/world/careers/employers", "POST",
                     {"name": "Fake Co", "slug": "fake", "state": "verified"}, BOSS)
        forged = sql("SELECT state FROM pciworld_employers WHERE slug=?", ("fake",))
        chk("C04 a client cannot create itself verified by sending state=verified",
            forged and forged[0][0] == "draft", str(forged))

        code, post = js(f"/api/world/careers/employers/{EMP}/postings", "POST",
                        {"title": "Senior Planner", "slug": "senior-planner",
                         "description": "Own the schedule.", "location": "Remote",
                         "employment_type": "full_time",
                         "salary_min_minor": 7000000, "salary_max_minor": 9000000, "currency": "gbp"}, BOSS)
        chk("C05 an UNVERIFIED employer may draft a posting", code == 200 and post.get("posting_id"), str(post))
        JOB = post.get("posting_id")

        code, q = js(f"/api/world/careers/postings/{JOB}/questions", "POST",
                     {"kind": "text", "prompt": "Describe a recovery plan you owned.", "required": True}, BOSS)
        chk("C06 a screening question can be added", code == 200 and q.get("question_id"), str(q))
        QID = q.get("question_id")

        code, _ = js(f"/api/world/careers/postings/{JOB}/questions", "POST",
                     {"kind": "consent", "prompt": "Do you agree"}, BOSS)
        chk("C07 the legacy 'consent' question kind is refused — consent is a record, not a checkbox",
            code == 400, f"got {code}")

        # ── THE GATE: an unverified employer publishes nothing, by any route ───────────────
        code, r = js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, BOSS)
        chk("C08 an unverified employer cannot publish", code == 403 and r.get("error") == "employer_not_verified", str(r))
        state = sql("SELECT state FROM pciworld_job_postings WHERE id=?", (JOB,))[0][0]
        chk("C09 and the posting really is still a draft afterwards", state == "draft", state)

        # The tenant asks; a person decides. Requesting verification must not BE verification.
        code, r = js(f"/api/world/careers/employers/{EMP}/request-verification", "POST", None, BOSS)
        chk("C10 an owner may request verification", code == 200 and r.get("state") == "pending_verification", str(r))
        code, r = js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, BOSS)
        chk("C11 requesting verification does not grant it", code == 403, f"got {code} {r}")

        # An employer member must not be able to verify their own employer through the admin route.
        code, r = js(f"/api/world-admin/careers/employers/{EMP}/state", "POST",
                     {"state": "verified"}, BOSS)
        chk("C12 an employer member cannot verify their own employer via the admin route",
            code in (401, 403), f"got {code} {r}")

        ver = sql("SELECT state,verified_by_admin_id FROM pciworld_employers WHERE id=?", (EMP,))[0]
        chk("C13 and the employer is still unverified after that attempt", ver[0] == "pending_verification", str(ver))

        code, r = js(f"/api/world-admin/careers/employers/{EMP}/state", "POST", {"state": "verified"}, A)
        chk("C14 a World admin can verify", code == 200 and r.get("state") == "verified", str(r))
        ver = sql("SELECT verified_at,verified_by_admin_id FROM pciworld_employers WHERE id=?", (EMP,))[0]
        chk("C15 verification is ATTRIBUTABLE — who and when are recorded",
            ver[0] and ver[1], str(ver))

        code, r = js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, BOSS)
        chk("C16 a verified employer can publish", code == 200 and r.get("state") == "published", str(r))

        # ── Applying: Passport only, consent explicit, snapshot frozen ─────────────────────
        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {str(QID): "x"}})
        chk("C17 anonymous apply is refused", code == 401, f"got {code} {r}")

        APPL = member("applicant@people.test")

        # The policy version pins the words the applicant actually saw. With none published, an
        # application would record a consent to nothing — so the door is shut rather than defaulted.
        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {str(QID): "Recovered a 14-week slip."},
                      "cv_base64": PDF, "cv_name": "cv.pdf"}, APPL)
        chk("C18 applying is refused while no consent policy version is published",
            code == 503 and r.get("error") == "consent_policy_unavailable", f"{code} {r}")

        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_careers_policy_version','v1-test')")

        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": False, "answers": {str(QID): "no consent"}}, APPL)
        chk("C19 applying without consent is refused", code == 400 and r.get("error") == "consent_required", str(r))

        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {str(QID): "Recovered a 14-week slip."},
                      "cv_base64": EXE, "cv_name": "cv.pdf"}, APPL)
        chk("C20 a renamed executable is refused — the sniff is on bytes, not on the filename",
            code == 400 and r.get("error") in ("cv_type_not_allowed", "cv_rejected"), str(r))

        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {}}, APPL)
        chk("C21 a required question must be answered", code == 400 and r.get("error") == "answer_required", str(r))

        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {str(QID): "Recovered a 14-week slip."},
                      "cv_base64": PDF, "cv_name": "my cv.pdf"}, APPL)
        chk("C22 a complete application is accepted", code == 200 and r.get("application_id"), str(r))
        APP = r.get("application_id")

        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "answers": {str(QID): "second try"}}, APPL)
        chk("C23 a second application to the same posting is refused", code == 409, f"got {code} {r}")
        n = sql("SELECT COUNT(*) FROM pciworld_applications WHERE posting_id=?", (JOB,))[0][0]
        chk("C24 and there is still exactly one application row", n == 1, str(n))

        # ── The snapshot does not move when the working copy does ─────────────────────────
        before = sql("SELECT prompt_snapshot,answer_snapshot FROM pciworld_application_answers WHERE application_id=?", (APP,))
        sql("UPDATE pciworld_job_questions SET prompt=? WHERE id=?", ("A completely different question", QID))
        after = sql("SELECT prompt_snapshot,answer_snapshot FROM pciworld_application_answers WHERE application_id=?", (APP,))
        chk("C25 editing the question does not change what the applicant appears to have been asked",
            before == after and before[0][0] == "Describe a recovery plan you owned.", f"{before} -> {after}")

        sql("DELETE FROM pciworld_job_questions WHERE id=?", (QID,))
        orphan = sql("SELECT prompt_snapshot FROM pciworld_application_answers WHERE application_id=?", (APP,))
        chk("C26 deleting the question leaves the answer readable — the row is self-contained evidence",
            orphan and orphan[0][0] == "Describe a recovery plan you owned.", str(orphan))

        cv_before = sql("SELECT cv_ref,cv_sha256 FROM pciworld_applications WHERE id=?", (APP,))[0]
        chk("C27 the CV reference and its digest are frozen on the application row",
            cv_before[0] and cv_before[1] and len(cv_before[1]) == 64, str(cv_before))

        # ── Tenant isolation ──────────────────────────────────────────────────────────────
        RIVAL = member("boss@rival.test")
        code, r2 = js("/api/world/careers/employers", "POST", {"name": "Rival Ltd", "slug": "rival"}, RIVAL)
        EMP2 = r2.get("employer_id")
        js(f"/api/world-admin/careers/employers/{EMP2}/state", "POST", {"state": "pending_verification"}, A)
        js(f"/api/world-admin/careers/employers/{EMP2}/state", "POST", {"state": "verified"}, A)

        code, r = js(f"/api/world/careers/postings/{JOB}/applications", "GET", None, RIVAL)
        chk("C28 a rival employer's owner cannot list this posting's applications", code == 403, f"got {code} {r}")

        code, raw = req("GET", f"/api/world/careers/applications/{APP}/cv", None, RIVAL)
        chk("C29 nor download its CV", code == 403, f"got {code}")
        logged = sql("SELECT COUNT(*) FROM pciworld_cv_access_log WHERE application_id=? AND allowed=0", (APP,))[0][0]
        chk("C30 and the REFUSAL is audited — a run of refusals is what probing ids looks like",
            logged >= 1, str(logged))

        code, r = js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, RIVAL)
        chk("C31 a rival cannot publish another tenant's posting", code == 403, f"got {code} {r}")

        # ── The employer's legitimate view, and what suspension does to it ─────────────────
        code, r = js(f"/api/world/careers/postings/{JOB}/applications", "GET", None, BOSS)
        chk("C32 the consented employer sees the application", code == 200 and len(r.get("applications", [])) == 1, str(r))
        code, raw = req("GET", f"/api/world/careers/applications/{APP}/cv", None, BOSS)
        chk("C33 and can download the CV", code == 200 and raw[:4] == b"%PDF", f"got {code}")
        allowed = sql("SELECT COUNT(*) FROM pciworld_cv_access_log WHERE application_id=? AND allowed=1", (APP,))[0][0]
        chk("C34 the successful view is audited too", allowed == 1, str(allowed))

        js(f"/api/world-admin/careers/employers/{EMP}/state", "POST", {"state": "suspended", "reason": "test"}, A)
        code, raw = req("GET", f"/api/world/careers/applications/{APP}/cv", None, BOSS)
        chk("C35 suspension cuts CV access in the SAME session that just succeeded", code == 403, f"got {code}")
        code, r = js(f"/api/world/careers/postings/{JOB}/applications", "GET", None, BOSS)
        chk("C36 and the applicant list with it", code == 403, f"got {code} {r}")
        code, r = js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, BOSS)
        chk("C37 a suspended employer cannot re-publish", code in (400, 403), f"got {code} {r}")

        js(f"/api/world-admin/careers/employers/{EMP}/state", "POST", {"state": "verified"}, A)
        code, raw = req("GET", f"/api/world/careers/applications/{APP}/cv", None, BOSS)
        chk("C38 re-verification restores access", code == 200, f"got {code}")

        # ── Consent withdrawal: access stops, the record survives ──────────────────────────
        code, r = js(f"/api/world/careers/applications/{APP}/consent/withdraw", "POST", None, APPL)
        chk("C39 an applicant can withdraw consent", code == 200, str(r))
        code, raw = req("GET", f"/api/world/careers/applications/{APP}/cv", None, BOSS)
        chk("C40 the employer's CV access stops at that instant", code == 403, f"got {code}")
        code, r = js(f"/api/world/careers/postings/{JOB}/applications", "GET", None, BOSS)
        chk("C41 and the application leaves the employer's list", code == 200 and r.get("applications") == [], str(r))
        rows = sql("SELECT granted_at,withdrawn_at,policy_version FROM pciworld_application_consents WHERE application_id=?", (APP,))
        chk("C42 the consent row SURVIVES — withdrawal is a timestamp, never a delete",
            len(rows) == 1 and rows[0][0] and rows[0][1] and rows[0][2] == "v1-test", str(rows))
        code, r = js("/api/world/careers/my/applications", "GET", None, APPL)
        mine = r.get("applications", [])
        chk("C43 the applicant still sees their own consent history, marked inactive",
            code == 200 and len(mine) == 1 and mine[0]["consent"]["active"] is False
            and mine[0]["consent"]["granted_at"], str(r))

        # ── The closed-posting honesty rule ───────────────────────────────────────────────
        code, r = js(f"/api/world/careers/postings/{JOB}/close", "POST", None, BOSS)
        chk("C44 a posting can be closed", code == 200 and r.get("state") == "closed", str(r))
        OTHER = member("other@people.test")
        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "cv_base64": PDF, "cv_name": "cv.pdf"}, OTHER)
        chk("C45 applying to a CLOSED posting is refused by the server, whatever a cached page said",
            code == 409 and r.get("error") == "not_accepting_applications", str(r))

        # A posting whose closes_at has passed is closed in FACT even if no sweep has stamped it.
        js(f"/api/world/careers/postings/{JOB}/publish", "POST", None, BOSS)
        sql("UPDATE pciworld_job_postings SET state='published', closes_at='2020-01-01 00:00:00' WHERE id=?", (JOB,))
        code, r = js(f"/api/world/careers/postings/{JOB}/apply", "POST",
                     {"consent": True, "cv_base64": PDF, "cv_name": "cv.pdf"}, OTHER)
        chk("C46 a posting past closes_at refuses applications without waiting for a sweep",
            code == 409, f"got {code} {r}")

        # ── Money is never a float, and never an amount without a currency ────────────────
        code, r = js(f"/api/world/careers/employers/{EMP}/postings", "POST",
                     {"title": "Cost Engineer", "slug": "cost-engineer", "salary_min_minor": 5000000}, BOSS)
        chk("C47 a salary with no currency is refused", code == 400 and r.get("error") == "currency_required", str(r))
        code, r = js(f"/api/world/careers/employers/{EMP}/postings", "POST",
                     {"title": "Cost Engineer", "slug": "cost-engineer",
                      "salary_min_minor": 9000000, "salary_max_minor": 5000000, "currency": "GBP"}, BOSS)
        chk("C48 an inverted salary range is refused", code == 400 and r.get("error") == "salary_range_inverted", str(r))

        # ── Notes are events, never an overwritten column ─────────────────────────────────
        APPL2 = member("second@people.test")
        code, r = js(f"/api/world/careers/employers/{EMP}/postings", "POST",
                     {"title": "Scheduler", "slug": "scheduler"}, BOSS)
        JOB2 = r.get("posting_id")
        js(f"/api/world/careers/postings/{JOB2}/publish", "POST", None, BOSS)
        code, r = js(f"/api/world/careers/postings/{JOB2}/apply", "POST",
                     {"consent": True, "cv_base64": PDF, "cv_name": "cv.pdf"}, APPL2)
        APP2 = r.get("application_id")
        js(f"/api/world/careers/applications/{APP2}/decision", "POST", {"decision": "shortlisted", "note": "first note"}, BOSS)
        js(f"/api/world/careers/applications/{APP2}/decision", "POST", {"decision": "rejected", "note": "second note"}, BOSS)
        notes = [n[0] for n in sql("SELECT note FROM pciworld_application_events WHERE application_id=? AND note IS NOT NULL", (APP2,))]
        chk("C49 both decision notes survive — the second does not overwrite the first",
            "first note" in notes and "second note" in notes, str(notes))

        # ── No ranking, anywhere. §10.7 forbids it, and the control is ABSENCE. ───────────
        src = ""
        for f in ("Endpoints/WorldCareers.cs", "Core/CareersState.cs", "Core/CareersEmployers.cs"):
            p = os.path.join(BACKEND, f)
            if os.path.exists(p): src += open(p, encoding="utf-8").read()
        # Looking for a scoring COLUMN or a ranking ORDER BY, not the word in prose: the design doc
        # discusses ranking at length precisely to say it is not built, so a prose match would fire
        # on the documentation of the prohibition rather than on a violation of it.
        chk("C50 no candidate score/rank column is written anywhere in the careers code",
            not re.search(r"(match_score|candidate_score|rank_score|ORDER BY\s+score)", src, re.I), "found a ranking seam")

        # ── The flag really is a kill switch ─────────────────────────────────────────────
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_careers_enabled','0')")
        code, _ = js(f"/api/world/careers/postings/{JOB2}/applications", "GET", None, BOSS)
        chk("C51 turning the flag off closes the surface again", code == 404, f"got {code}")
        rows = sql("SELECT COUNT(*) FROM pciworld_applications")[0][0]
        chk("C52 and touches no existing rows", rows >= 2, str(rows))

    finally:
        shutdown()

    print(f"\n{passed} passed, {failed} failed  [{PROVIDER}]")
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
