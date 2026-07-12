#!/usr/bin/env python3
"""
Honorary Route public application suite.

Proves the end-to-end honorary-application flow that is SEPARATE from student registration:
  • a public applicant submits personal/professional/qualification details + document uploads and
    gets a PCI-HONAPP reference; required-field, declaration, resume and file-type validation all bite
  • the board (owner only) lists, reads, downloads the uploaded file, and decides
  • approval confers a real, verifiable PCI-HON honorary award and marks the application approved
  • every notification attempt is recorded in notification_history (reusable notification ledger)
  • non-owner admins are refused (403); unauthenticated is refused (401)
  • registration confirm-password mismatch is rejected server-side (400 password_mismatch)

Run from backend/:  python3 tests/honorary_application_test.py
"""
import base64
import sys
import integration_test as it

# A minimal but valid PDF (starts with the %PDF magic bytes Storage sniffs for).
PDF = "data:application/pdf;base64," + base64.b64encode(
    b"%PDF-1.4\n1 0 obj<</Type/Catalog>>endobj\ntrailer<</Root 1 0 R>>\n%%EOF").decode()

BASE_APP = {
    "first_name": "Grace", "last_name": "Hopper", "email": "grace.app@example.com",
    "mobile": "+1 5551234567", "country": "United States", "city": "Arlington",
    "nationality": "American", "job_title": "Rear Admiral", "employer": "US Navy",
    "years_experience": 40, "industry": "Computing", "highest_qualification": "PhD",
    "relevant_experience": "Pioneered compilers and standardisation.",
    "professional_summary": "A foundational contributor to the profession.",
    "declaration": True,
    "documents": [{"doc_kind": "resume", "filename": "cv.pdf", "data_uri": PDF}],
}


def run(admin):
    chk, req, jget = it.chk, it.req, it.jget

    # ---------- public submission + validation ----------
    print("\n=== HA1. Public submission + validation ===")
    c, b = jget("POST", "/api/honorary-application", body=BASE_APP)
    chk("ha1a valid submit → PCI-HONAPP reference", c == 200 and str(b.get("reference", "")).startswith("PCI-HONAPP-"), b)
    ref = b.get("reference")

    no_resume = dict(BASE_APP, email="x1@ex.co", documents=[])
    chk("ha1b missing résumé → 400 resume_required", jget("POST", "/api/honorary-application", body=no_resume) == (400, {"error": "resume_required", "message": "A résumé / CV (PDF, JPG or PNG, up to 3 MB) is required."}) or jget("POST", "/api/honorary-application", body=no_resume)[1].get("error") == "resume_required")

    no_decl = dict(BASE_APP, email="x2@ex.co", declaration=False)
    chk("ha1c missing declaration → 400", jget("POST", "/api/honorary-application", body=no_decl)[1].get("error") == "declaration_required")

    bad_file = dict(BASE_APP, email="x3@ex.co", documents=[{"doc_kind": "resume", "filename": "x.txt", "data_uri": "data:text/plain;base64,aGVsbG8="}])
    chk("ha1d disallowed file type → 400 file_type_not_allowed", jget("POST", "/api/honorary-application", body=bad_file)[1].get("error") == "file_type_not_allowed")

    missing_field = dict(BASE_APP, email="x4@ex.co"); del missing_field["city"]
    chk("ha1e missing required field → 400 city_required", jget("POST", "/api/honorary-application", body=missing_field)[1].get("error") == "city_required")

    bad_email = dict(BASE_APP, email="not-an-email")
    chk("ha1f invalid email → 400 invalid_email", jget("POST", "/api/honorary-application", body=bad_email)[1].get("error") == "invalid_email")

    # ---------- RBAC: board/owner only ----------
    print("\n=== HA2. Admin review is owner-only ===")
    chk("ha2a list without auth → 401", req("GET", "/api/admin/honorary-applications")[0] == 401)
    c, tb = jget("POST", "/api/admin/team", token=admin, body={"email": "smgr-hona@pci.test", "name": "sm", "role": "student_manager"})
    c, lb = jget("POST", "/api/admin/auth/login", body={"email": "smgr-hona@pci.test", "password": tb.get("temp_password")})
    smgr = it.clear_must_change(lb.get("token"))
    chk("ha2b list as student_manager → 403", jget("GET", "/api/admin/honorary-applications", token=smgr)[0] == 403)

    # ---------- owner: list / detail / download / approve ----------
    print("\n=== HA3. Board review → approve confers a verifiable award ===")
    c, lst = jget("GET", "/api/admin/honorary-applications?status=pending_review", token=admin)
    row = next((r for r in lst.get("rows", []) if r.get("reference") == ref), None)
    chk("ha3a application appears in pending list with a doc", row is not None and row.get("doc_count") == 1, row)
    aid = row["id"]
    c, det = jget("GET", f"/api/admin/honorary-applications/{aid}", token=admin)
    chk("ha3b detail returns applicant + document metadata", c == 200 and det["application"]["job_title"] == "Rear Admiral" and len(det["documents"]) == 1, det.get("documents"))
    docid = det["documents"][0]["id"]
    dc, _ = req("GET", f"/api/admin/honorary-applications/{aid}/documents/{docid}/file", token=admin)
    chk("ha3c uploaded document downloads (200)", dc == 200)

    # snapshot exam-shaped tables — approval must NOT create any of them
    con = it.dbconn()
    before = {t: con.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] for t in ("issued_credentials", "exam_entitlements", "exam_attempts")}
    con.close()
    c, dec = jget("POST", f"/api/admin/honorary-applications/{aid}/decide", token=admin, body={"status": "approved", "note": "Distinguished lifetime contribution."})
    award = dec.get("award_no", "")
    chk("ha3d approve → PCI-HON award conferred", c == 200 and str(award).startswith("PCI-HON-"), dec)
    c, v = jget("GET", f"/api/verify?id={award}")
    chk("ha3e award is publicly verifiable as honorary", v.get("type") == "honorary" and v.get("valid") is True and v.get("recipient") == "Grace Hopper", v)
    con = it.dbconn()
    after = {t: con.execute(f"SELECT COUNT(*) FROM {t}").fetchone()[0] for t in ("issued_credentials", "exam_entitlements", "exam_attempts")}
    con.close()
    chk("ha3f approval created NOTHING exam-shaped (no credential/entitlement/attempt)", after == before, {"before": before, "after": after})

    c, det2 = jget("GET", f"/api/admin/honorary-applications/{aid}", token=admin)
    chk("ha3g application now marked approved with the award number", det2["application"]["status"] == "approved" and det2["application"]["award_no"] == award, det2["application"].get("status"))
    chk("ha3h re-approve refused (409 already decided)", jget("POST", f"/api/admin/honorary-applications/{aid}/decide", token=admin, body={"status": "approved"})[0] == 409)

    # ---------- notification ledger ----------
    print("\n=== HA4. Reusable notification ledger records every attempt ===")
    con = it.dbconn()
    n = con.execute("SELECT COUNT(*) FROM notification_history WHERE related_type='honorary_application'").fetchone()[0]
    chans = [r[0] for r in con.execute("SELECT DISTINCT channel FROM notification_history").fetchall()]
    con.close()
    chk("ha4a notification_history has ≥3 rows (applicant ack, admin alert, approval)", n >= 3, n)
    chk("ha4b ledger channel is email (seam for sms/in_app)", chans == ["email"], chans)

    # ---------- registration confirm-password (server-side) ----------
    print("\n=== HA5. Registration confirm-password is enforced server-side ===")
    mism = {"firstName": "Mis", "lastName": "Match", "email": "mismatch-pw@ex.co", "password": "abcd1234", "confirmPassword": "different1"}
    chk("ha5a mismatched confirm → 400 password_mismatch", jget("POST", "/api/register", body=mism)[1].get("error") == "password_mismatch")
    ok = {"firstName": "Good", "lastName": "Pw", "email": "match-pw@ex.co", "password": "abcd1234", "confirmPassword": "abcd1234"}
    chk("ha5b matching confirm → account created", jget("POST", "/api/register", body=ok)[1].get("ok") is True)


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
