#!/usr/bin/env python3
"""
Founding-Stage access suite (spec: founding codes over the paid flow).

Boots the real backend (same harness as integration_test.py) and proves:
  • Route 1 (founding_member): free membership + study, NO exam entitlement; uses_count++
  • Route 2 (founding_candidate): application + evidence + criteria + auto-approve →
    free membership AND exam entitlement through the SAME settlement rows as a paid order
  • redeem-twice is idempotent (no double grant, no duplicate payment)
  • expired window → invalid; the normal PAID flow is untouched (fallback proven)
  • max_total_uses cap enforced
  • auto_approve OFF → pending_review; nothing granted until an admin decides
  • board flips auto→manual → past approvals stay approved; new ones queue
  • founding entitlement passes BookingBlockers exactly like a paid one (books + sits + passes)
  • revoke kills an UNUSED grant; an EARNED credential is never revoked
  • evidence is mandatory and validated (missing / wrong MIME rejected)
  • RBAC: code creation + queues are admin-gated

Run from backend/:  python3 tests/founding_test.py
"""
import json, sys, time, urllib.error, urllib.request
import integration_test as it


def binget(path, token):
    """Fetch raw bytes (it.req decodes utf-8, which breaks on binary evidence)."""
    r = urllib.request.Request(it.BASE + path, headers={"Authorization": "Bearer " + token})
    try:
        with urllib.request.urlopen(r) as resp:
            return resp.status, resp.read()
    except urllib.error.HTTPError as e:
        return e.code, b""


def free_user(email):
    """A signed-up student with NO payment (mirrors integration_test's direct-mint pattern)."""
    con = it.dbconn()
    con.execute("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?,?,?)",
                (email, "Founding", "Tester", "student", "active"))
    uid = con.execute("SELECT id FROM users WHERE email=?", (email,)).fetchone()[0]
    con.execute("INSERT INTO student_profiles(user_id) VALUES(?)", (uid,))
    tok = "sess_" + it.sha256hex(email)[:24]
    con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                (uid, it.sha256hex(tok)))
    con.commit(); con.close()
    return tok, uid


def waiver_payments(uid):
    con = it.dbconn()
    n = con.execute("SELECT COUNT(*) FROM payments WHERE user_id=? AND payment_provider='founding_waiver'", (uid,)).fetchone()[0]
    con.close(); return n


def run(admin):
    chk, req, jget = it.chk, it.req, it.jget

    # ---------- setup: board creates the founding codes from the admin console API ----------
    print("\n=== F0. Founding code creation (admin console API) ===")
    def make_code(body):
        c, r = jget("POST", "/api/admin/codes", token=admin, body=body)
        if c != 200: raise SystemExit(f"code create failed: {c} {r}")
        return r["id"]
    m1 = make_code({"code": "FOUND-M1", "founding_route": "founding_member", "grants_membership": True,
                    "grants_study_access": True, "end_date": "2099-01-01", "max_uses": 100, "active": True,
                    "membership_months": 12})
    c1 = make_code({"code": "FOUND-C1", "founding_route": "founding_candidate", "grants_membership": True,
                    "grants_exam": True, "end_date": "2099-01-01", "max_uses": 100, "active": True,
                    "criteria_json": json.dumps({"min_experience_years": 3, "require_qualification": True})})
    exp = make_code({"code": "FOUND-EXP", "founding_route": "founding_member", "grants_membership": True,
                     "end_date": "2020-01-01", "active": True})
    cap = make_code({"code": "FOUND-CAP", "founding_route": "founding_member", "grants_membership": True,
                     "end_date": "2099-01-01", "max_uses": 1, "active": True})
    man = make_code({"code": "FOUND-MAN", "founding_route": "founding_candidate", "grants_membership": True,
                     "grants_exam": True, "end_date": "2099-01-01", "auto_approve": False, "active": True})
    chk("f0a five founding codes created", all([m1, c1, exp, cap, man]))
    chk("f0b RBAC: creating a code without admin auth → 401",
        req("POST", "/api/admin/codes", body={"code": "NOPE"})[0] == 401)
    chk("f0c RBAC: applications queue without admin auth → 401",
        req("GET", "/api/admin/founding-applications")[0] == 401)

    # ---------- Route 1: founding member ----------
    print("\n=== F1. Route 1 — founding member (membership + study, NO exam) ===")
    c, v = jget("POST", "/api/founding/validate", body={"code": "found-m1"})
    chk("f1a validate: valid, route, grants, no application",
        c == 200 and v["valid"] and v["route"] == "founding_member"
        and v["grants"]["membership"] and not v["grants"]["exam"] and not v["requires_application"], v)
    atok, auid = free_user("found-a@ex.co")
    c, r = jget("POST", "/api/founding/redeem", token=atok, body={"code": "FOUND-M1"})
    chk("f1b redeem grants membership (FOUND ref)", c == 200 and r.get("ok") and str(r.get("reference", "")).startswith("FOUND-"), r)
    c, me = jget("GET", "/api/me", token=atok)
    chk("f1c membership active, candidate_status=member, NO exam entitlement",
        me["lifecycle"]["membership_status"] == "active" and me["lifecycle"]["candidate_status"] == "member"
        and len(me["exams"]) == 0, me["lifecycle"])
    chk("f1d USD 0 waiver payment on record", any(p["final_amount"] == 0 and str(p["reference"]).startswith("FOUND-") for p in me["payments"]))
    con = it.dbconn()
    uses = con.execute("SELECT used_count FROM discount_codes WHERE id=?", (m1,)).fetchone()[0]; con.close()
    chk("f1e uses_count incremented to 1", uses == 1, uses)
    c, r2 = jget("POST", "/api/founding/redeem", token=atok, body={"code": "FOUND-M1"})
    chk("f1f second redeem by same user refused (409, no double grant)", c == 409 and r2.get("error") == "already_redeemed", (c, r2))
    chk("f1g still exactly one waiver payment", waiver_payments(auid) == 1)

    # ---------- Route 2: founding candidate (application + evidence + auto-approve) ----------
    print("\n=== F2. Route 2 — founding candidate (application → membership + exam) ===")
    btok, buid = free_user("found-b@ex.co")
    c, r = jget("POST", "/api/me/founding-application", token=btok,
                body={"code": "FOUND-C1", "experience_years": 6, "role": "Project Controls Lead",
                      "qualification": "BSc Engineering", "evidence": it.TINY_PNG, "evidence_name": "cv.png"})
    chk("f2a met criteria + auto-approve → auto_approved with exam grant",
        c == 200 and r.get("status") == "auto_approved" and r["granted"]["exam"], r)
    c, me = jget("GET", "/api/me", token=btok)
    chk("f2b membership active AND exam entitlement present (exam_fee_paid)",
        me["lifecycle"]["membership_status"] == "active" and me["lifecycle"]["candidate_status"] == "exam_fee_paid"
        and len(me["exams"]) == 1, me["lifecycle"])
    chk("f2c exactly one waiver payment row", waiver_payments(buid) == 1)
    c, st = jget("GET", "/api/me/founding-application", token=btok)
    chk("f2d own application shows auto_approved", c == 200 and st["latest"]["status"] == "auto_approved", st)

    # evidence is mandatory + validated
    b2tok, _ = free_user("found-b2@ex.co")
    c, r = jget("POST", "/api/me/founding-application", token=b2tok,
                body={"code": "FOUND-C1", "experience_years": 6, "qualification": "MSc", "role": "planner"})
    chk("f2e missing evidence rejected", c == 400 and r.get("error") == "evidence_required", r)
    c, r = jget("POST", "/api/me/founding-application", token=b2tok,
                body={"code": "FOUND-C1", "experience_years": 6, "qualification": "MSc", "role": "planner",
                      "evidence": "data:text/html;base64,PGI+aGk8L2I+"})
    chk("f2f wrong MIME rejected", c == 400 and r.get("error") == "file_type_not_allowed", r)
    # unmet criteria (1 year < 3) while auto-approve ON → queued, nothing granted
    c, r = jget("POST", "/api/me/founding-application", token=b2tok,
                body={"code": "FOUND-C1", "experience_years": 1, "qualification": "MSc", "role": "planner",
                      "evidence": it.TINY_PNG})
    c2, me2 = jget("GET", "/api/me", token=b2tok)
    chk("f2g unmet criteria → pending_review, nothing granted",
        c == 200 and r.get("status") == "pending_review" and me2["lifecycle"]["membership_status"] != "active"
        and len(me2["exams"]) == 0, (r, me2["lifecycle"]))

    # ---------- founding entitlement passes the SAME eligibility gates as a paid one ----------
    print("\n=== F3. Founding entitlement behaves exactly like paid in BookingBlockers ===")
    it.accept_all_consents(btok); it.complete_profile(btok)
    code_b, bk, st = it.book_and_start(btok, admin)
    chk("f3a founding entitlement books and starts a real exam", code_b == 200 and st and "attempt_id" in st, (code_b, bk))
    key = it.answer_key([i["id"] for i in st["items"]])
    c, sub = jget("POST", "/api/me/exam/submit", token=btok, body={"attempt_id": st["attempt_id"], "answers": key})
    chk("f3b passes the REAL exam → credential earned the normal way",
        c == 200 and sub.get("result") == "pass" and sub.get("credential"), sub if c != 200 else sub.get("result"))
    b_credential = sub.get("credential")

    # ---------- expired window + paid-flow fallback ----------
    print("\n=== F4. Window closes → codes stop; the paid flow is untouched ===")
    c, v = jget("POST", "/api/founding/validate", body={"code": "FOUND-EXP"})
    chk("f4a expired code validates as not-valid (no internals leaked)", c == 200 and v["valid"] is False, v)
    dtok, duid = free_user("found-d@ex.co")
    c, r = jget("POST", "/api/founding/redeem", token=dtok, body={"code": "FOUND-EXP"})
    chk("f4b expired code cannot be redeemed", c == 400 and r.get("error") == "invalid_code", (c, r))
    chk("f4c nothing was granted", waiver_payments(duid) == 0)
    # the standard paid flow still settles normally for the same person
    it.sign_and_send_webhook("cs_found_d", "found-d@ex.co", "bundle", "pi_found_d")
    c, me = jget("GET", "/api/me", token=dtok)
    chk("f4d normal payment still grants membership + entitlement (fallback proven)",
        me["lifecycle"]["membership_status"] == "active" and len(me["exams"]) == 1, me["lifecycle"])

    # ---------- total-uses cap ----------
    print("\n=== F5. max_total_uses cap ===")
    etok, _ = free_user("found-e@ex.co")
    ftok, fuid = free_user("found-f@ex.co")
    c, _r = jget("POST", "/api/founding/redeem", token=etok, body={"code": "FOUND-CAP"})
    chk("f5a first redemption within cap succeeds", c == 200)
    c, r = jget("POST", "/api/founding/redeem", token=ftok, body={"code": "FOUND-CAP"})
    chk("f5b cap reached → further redemption rejected", c == 400 and r.get("error") == "fully_redeemed", (c, r))
    chk("f5c rejected user got nothing", waiver_payments(fuid) == 0)

    # ---------- manual review queue ----------
    print("\n=== F6. auto_approve OFF → queue → admin decision ===")
    gtok, guid = free_user("found-g@ex.co")
    c, r = jget("POST", "/api/me/founding-application", token=gtok,
                body={"code": "FOUND-MAN", "experience_years": 8, "role": "cost engineer",
                      "qualification": "BEng", "evidence": it.TINY_PNG})
    chk("f6a manual code → pending_review", c == 200 and r.get("status") == "pending_review", r)
    c, me = jget("GET", "/api/me", token=gtok)
    chk("f6b nothing granted while pending", me["lifecycle"]["membership_status"] != "active" and len(me["exams"]) == 0)
    c, q = jget("GET", "/api/admin/founding-applications?status=pending_review", token=admin)
    g_app = next((a for a in q["rows"] if a["user_id"] == guid), None)
    chk("f6c application visible in the admin queue with evidence", c == 200 and g_app is not None and g_app["has_evidence"] == 1, q if c != 200 else "")
    c, ev = binget(f"/api/admin/founding-applications/{g_app['id']}/evidence", admin)
    chk("f6d admin can stream the evidence (PNG magic)", c == 200 and ev[:4] == b"\x89PNG", c)
    c, r = jget("POST", f"/api/admin/founding-applications/{g_app['id']}/decide", token=admin,
                body={"status": "approved", "admin_note": "board approved"})
    chk("f6e admin approve grants via the settlement path", c == 200 and r.get("status") == "approved", r)
    c, me = jget("GET", "/api/me", token=gtok)
    chk("f6f approved applicant now has membership + exam entitlement",
        me["lifecycle"]["membership_status"] == "active" and len(me["exams"]) == 1, me["lifecycle"])

    # ---------- board flips auto → manual: no clawback ----------
    print("\n=== F7. auto→manual flip: past approvals stand; new ones queue ===")
    c, _r = jget("PATCH", f"/api/admin/codes/{c1}", token=admin, body={"auto_approve": False})
    chk("f7a board can flip auto_approve from the console", c == 200)
    c, st = jget("GET", "/api/me/founding-application", token=btok)
    chk("f7b earlier auto-approved application is untouched", st["latest"]["status"] == "auto_approved", st)
    c, me = jget("GET", "/api/me", token=btok)
    chk("f7c earlier grant is untouched (membership still active)", me["lifecycle"]["membership_status"] == "active")
    htok, _ = free_user("found-h@ex.co")
    c, r = jget("POST", "/api/me/founding-application", token=htok,
                body={"code": "FOUND-C1", "experience_years": 9, "role": "project controls manager",
                      "qualification": "MSc", "evidence": it.TINY_PNG})
    chk("f7d new application after the flip goes to pending_review", c == 200 and r.get("status") == "pending_review", r)

    # ---------- revoke: unused grant dies; earned credential survives ----------
    print("\n=== F8. Revoke policy ===")
    c, q = jget("GET", "/api/admin/founding-applications", token=admin)
    g_app = next(a for a in q["rows"] if a["user_id"] == guid)
    c, r = jget("POST", f"/api/admin/founding-applications/{g_app['id']}/decide", token=admin,
                body={"status": "revoked", "admin_note": "policy test"})
    chk("f8a revoke an UNUSED grant deactivates the entitlement", c == 200 and r.get("entitlements_revoked") == 1, r)
    c, me = jget("GET", "/api/me", token=gtok)
    chk("f8b revoked student has no entitlement and no active membership",
        len(me["exams"]) == 0 and me["lifecycle"]["membership_status"] != "active", me["lifecycle"])
    # B passed the exam through this code — revoking B must NOT touch the earned credential
    c, q = jget("GET", "/api/admin/founding-applications", token=admin)
    b_app = next(a for a in q["rows"] if a["user_id"] == buid)
    c, r = jget("POST", f"/api/admin/founding-applications/{b_app['id']}/decide", token=admin,
                body={"status": "revoked", "admin_note": "credential must survive"})
    chk("f8c revoking a consumed grant revokes no entitlement", c == 200 and r.get("entitlements_revoked") == 0, r)
    con = it.dbconn()
    cred = con.execute("SELECT status FROM issued_credentials WHERE credential_id=?", (b_credential,)).fetchone()
    pay = con.execute("SELECT payment_status FROM payments WHERE user_id=? AND payment_provider='founding_waiver'", (buid,)).fetchone()
    con.close()
    chk("f8d earned credential is NOT revoked", cred is not None and cred[0] == "active", cred)
    chk("f8e consumed waiver payment keeps its paid audit trail", pay is not None and pay[0] == "paid", pay)

    # ---------- stats ----------
    print("\n=== F9. Founding stats for the console ===")
    c, s = jget("GET", "/api/admin/founding-stats", token=admin)
    m1_row = next((x for x in s["rows"] if x["code"] == "FOUND-M1"), None)
    cap_row = next((x for x in s["rows"] if x["code"] == "FOUND-CAP"), None)
    chk("f9a stats list every founding code with uses + window",
        c == 200 and m1_row and m1_row["uses"]["redeemed"] == 1 and cap_row["uses"]["remaining"] == 0, s if c != 200 else "")
    chk("f9b applications-by-status included", any((x.get("applications") or {}) for x in s["rows"]))


def main():
    proc = it.boot()
    try:
        admin = it.admin_login()
        it.widen_window(admin)
        run(admin)
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    print(f"\n  ══ {it.passed}/{it.passed + it.failed} PASSED ══")
    sys.exit(0 if it.failed == 0 else 1)


if __name__ == "__main__":
    main()
