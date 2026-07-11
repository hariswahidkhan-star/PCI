#!/usr/bin/env python3
"""
Honorary Fellow (PCI) suite — Route C of the three-route access model.

Proves the hard separation between the board-conferred honorary recognition and the examined
PCP-AI credential:
  • confer creates an honorary_awards row with a PCI-HON number — and NOTHING else (no
    issued_credentials row, no entitlement, no exam attempt, no lifecycle change)
  • /api/verify on a PCI-HON id returns type:'honorary', clearly not a passed exam; a PCP-AI id
    still returns the exam credential; the two id spaces never collide
  • conferral/revocation is owner-only (401 unauthenticated, 403 for every non-owner role)
  • revoking an honorary award never touches an exam-earned credential
  • the linked account shows a read-only honorary badge in /api/me with all exam fields untouched

Run from backend/:  python3 tests/honorary_test.py
"""
import sys
import integration_test as it


def run(admin):
    chk, req, jget = it.chk, it.req, it.jget

    # a linked recipient account (no payments, no exam history)
    con = it.dbconn()
    con.execute("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?,?,?)",
                ("hon-recipient@ex.co", "Ayesha", "Rahman", "student", "active"))
    huid = con.execute("SELECT id FROM users WHERE email=?", ("hon-recipient@ex.co",)).fetchone()[0]
    con.execute("INSERT INTO student_profiles(user_id) VALUES(?)", (huid,))
    htok = "sess_" + it.sha256hex("hon-recipient@ex.co")[:24]
    con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                (huid, it.sha256hex(htok)))
    con.commit(); con.close()

    # ---------- RBAC: board/owner only ----------
    print("\n=== H1. RBAC — honorary is board/owner only ===")
    chk("h1a confer without auth → 401", req("POST", "/api/admin/honorary", body={"recipient_name": "X Y"})[0] == 401)
    c, b = jget("POST", "/api/admin/team", token=admin, body={"email": "smgr-hon@pci.test", "name": "smgr", "role": "student_manager"})
    c, lb = jget("POST", "/api/admin/auth/login", body={"email": "smgr-hon@pci.test", "password": b.get("temp_password")})
    smgr = lb.get("token")
    chk("h1b confer as student_manager → 403", jget("POST", "/api/admin/honorary", token=smgr, body={"recipient_name": "X Y"})[0] == 403)
    chk("h1c list as student_manager → 403", jget("GET", "/api/admin/honorary", token=smgr)[0] == 403)

    # ---------- confer ----------
    print("\n=== H2. Confer — separate registry, nothing exam-shaped is created ===")
    con = it.dbconn()
    before = {t: con.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0]
              for t in ("issued_credentials", "exam_entitlements", "exam_attempts", "memberships", "payments")}
    con.close()
    c, hw = jget("POST", "/api/admin/honorary", token=admin,
                 body={"recipient_name": "Prof. Ayesha Rahman", "citation": "For distinguished service to the profession.", "user_email": "hon-recipient@ex.co"})
    chk("h2a owner confers → PCI-HON award number", c == 200 and str(hw.get("award_no", "")).startswith("PCI-HON-"), hw)
    award_no = hw["award_no"]
    con = it.dbconn()
    after = {t: con.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0]
             for t in ("issued_credentials", "exam_entitlements", "exam_attempts", "memberships", "payments")}
    hrow = con.execute("SELECT recipient_name,user_id,status,conferred_by FROM honorary_awards WHERE award_no=?", (award_no,)).fetchone()
    con.close()
    chk("h2b honorary row exists, linked to the account, with the conferring admin recorded",
        hrow is not None and hrow[1] == huid and hrow[2] == "active" and hrow[3] is not None, hrow)
    chk("h2c NOTHING exam-shaped created (credentials/entitlements/attempts/memberships/payments unchanged)",
        before == after, (before, after))
    chk("h2d unknown linked email refused (404)",
        jget("POST", "/api/admin/honorary", token=admin, body={"recipient_name": "No Body", "user_email": "ghost@ex.co"})[0] == 404)

    # ---------- verify: two id spaces, never confused ----------
    print("\n=== H3. Verification — honorary is never a passed exam ===")
    c, v = jget("GET", f"/api/verify?id={award_no}")
    chk("h3a verify PCI-HON → type honorary + designation, valid",
        c == 200 and v.get("type") == "honorary" and v.get("designation") == "Honorary Fellow (PCI)"
        and v.get("valid") is True and v.get("state") == "active", v)
    chk("h3b honorary verify carries the explicit not-an-exam note and NO exam fields",
        "not an examined" in str(v.get("note", "")) and "result" not in v and "percent" not in v and "credential_id" not in v, v)
    # a real PCP-AI credential id still verifies as an exam credential (distinct number space).
    # It belongs to a DIFFERENT user so the honorary recipient's portal state stays clean for H4.
    con = it.dbconn()
    con.execute("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?,?,?)",
                ("hon-other@ex.co", "Other", "Holder", "student", "active"))
    ouid = con.execute("SELECT id FROM users WHERE email=?", ("hon-other@ex.co",)).fetchone()[0]
    con.execute("INSERT INTO issued_credentials(credential_id,user_id,attempt_id,holder_name,credential,certification_id,status,expires_at) VALUES(?,?,?,?,?,1,'active',datetime('now','+3 year'))",
                ("PCP-AI-2026-99999", ouid, 424242, "Other Holder", "PCP-AI"))
    con.commit(); con.close()
    c, v2 = jget("GET", "/api/verify?id=PCP-AI-2026-99999")
    chk("h3c verify PCP-AI id → exam credential shape, untouched by honorary logic",
        c == 200 and v2.get("found") and v2.get("credential_id") == "PCP-AI-2026-99999" and "type" not in v2, v2)
    chk("h3d unknown PCI-HON id → found:false", jget("GET", "/api/verify?id=PCI-HON-1900-0001")[1].get("found") is False)

    # ---------- linked portal account: read-only badge, exam surface untouched ----------
    print("\n=== H4. Portal state — read-only badge, exam fields untouched ===")
    c, me = jget("GET", "/api/me", token=htok)
    chk("h4a /api/me lists the honorary award", c == 200 and any(h["award_no"] == award_no for h in me.get("honorary", [])), me.get("honorary"))
    chk("h4b exam/lifecycle surface untouched by the honour (no status, no result, no credential)",
        me["lifecycle"]["exam_status"] == "not_booked" and me["lifecycle"]["candidate_status"] == "none"
        and me["lifecycle"]["credential_status"] is None and len(me["credentials"]) == 0 and len(me["attempts"]) == 0,
        me["lifecycle"])
    chk("h4c honorary alone grants NO exam entitlement and no fee waiver",
        len(me["exams"]) == 0 and me.get("founding_member") is False, me["exams"])

    # ---------- revoke ----------
    print("\n=== H5. Revoke — honorary only, never the earned credential ===")
    c, hl = jget("GET", "/api/admin/honorary", token=admin)
    hid = next(r["id"] for r in hl["rows"] if r["award_no"] == award_no)
    chk("h5a revoke as student_manager → 403", jget("POST", f"/api/admin/honorary/{hid}/revoke", token=smgr, body={"reason": "x"})[0] == 403)
    c, rv = jget("POST", f"/api/admin/honorary/{hid}/revoke", token=admin, body={"reason": "governance test"})
    chk("h5b owner revoke succeeds", c == 200 and rv.get("ok") is True, rv)
    c, v3 = jget("GET", f"/api/verify?id={award_no}")
    chk("h5c verify now shows the honorary award revoked", v3.get("state") == "revoked" and v3.get("valid") is False, v3)
    con = it.dbconn()
    cred = con.execute("SELECT status FROM issued_credentials WHERE credential_id=?", ("PCP-AI-2026-99999",)).fetchone()
    con.close()
    chk("h5d the exam-earned credential is untouched by the honorary revocation", cred is not None and cred[0] == "active", cred)
    chk("h5e double revoke refused (409)", jget("POST", f"/api/admin/honorary/{hid}/revoke", token=admin, body={})[0] == 409)


def main():
    proc = it.boot()
    try:
        admin = it.admin_login()
        run(admin)
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    print(f"\n  ══ {it.passed}/{it.passed + it.failed} PASSED ══")
    sys.exit(0 if it.failed == 0 else 1)


if __name__ == "__main__":
    main()
