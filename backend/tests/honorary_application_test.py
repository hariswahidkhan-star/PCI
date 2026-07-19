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
    "declaration": True, "eligibility_confirmed": True, "terms_accepted": True,
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

    no_elig = dict(BASE_APP, email="x2b@ex.co", eligibility_confirmed=False)
    chk("ha1c2 missing eligibility confirmation → 400", jget("POST", "/api/honorary-application", body=no_elig)[1].get("error") == "eligibility_required")

    no_terms = dict(BASE_APP, email="x2c@ex.co", terms_accepted=False)
    chk("ha1c3 missing terms acceptance → 400", jget("POST", "/api/honorary-application", body=no_terms)[1].get("error") == "terms_required")

    bad_file = dict(BASE_APP, email="x3@ex.co", documents=[{"doc_kind": "resume", "filename": "x.txt", "data_uri": "data:text/plain;base64,aGVsbG8="}])
    chk("ha1d disallowed file type → 400 file_type_not_allowed", jget("POST", "/api/honorary-application", body=bad_file)[1].get("error") == "file_type_not_allowed")

    missing_field = dict(BASE_APP, email="x4@ex.co"); del missing_field["city"]
    chk("ha1e missing required field → 400 city_required", jget("POST", "/api/honorary-application", body=missing_field)[1].get("error") == "city_required")

    bad_email = dict(BASE_APP, email="not-an-email")
    chk("ha1f invalid email → 400 invalid_email", jget("POST", "/api/honorary-application", body=bad_email)[1].get("error") == "invalid_email")

    # ---------- structured qualifications / certifications / career history ----------
    print("\n=== HA1s. Repeatable qualification/certification/experience rows ===")
    sq = dict(BASE_APP, email="structured@example.com")
    sq.pop("highest_qualification", None); sq.pop("professional_certifications", None)
    sq["qualifications"] = [
        {"qualification": "PhD Engineering", "institution": "MIT", "year": 2001},
        {"qualification": "MSc Project Management", "institution": "UCL", "year": "1898"},  # invalid year → dropped
    ]
    sq["certifications"] = [{"name": "PMP", "issuer": "PMI", "year": 2010}, {"name": "CCP", "issuer": "AACE"}]
    sq["experience"] = [
        {"role": "Programme Director", "employer": "Acme Infrastructure", "from_year": 2015},
        {"role": "", "employer": ""},  # empty row → skipped
    ]
    c, sb = jget("POST", "/api/honorary-application", body=sq)
    chk("ha1s-a structured submission accepted", c == 200 and sb.get("ok") is True, sb)
    c, slst = jget("GET", "/api/admin/honorary-applications?status=pending_review", token=admin)
    srow = next(r for r in slst["rows"] if r.get("reference") == sb.get("reference"))
    c, sdet = jget("GET", f"/api/admin/honorary-applications/{srow['id']}", token=admin)
    sapp = sdet["application"]
    import json as _j
    qj = _j.loads(sapp.get("qualifications_json") or "[]")
    cj = _j.loads(sapp.get("certifications_json") or "[]")
    ej = _j.loads(sapp.get("experience_json") or "[]")
    chk("ha1s-b two qualification rows stored; invalid year dropped to null",
        len(qj) == 2 and qj[0]["year"] == 2001 and qj[1]["year"] is None, qj)
    chk("ha1s-c certification rows stored with issuer + year", len(cj) == 2 and cj[0] == {"name": "PMP", "issuer": "PMI", "year": 2010}, cj)
    chk("ha1s-d empty career row skipped, real one kept", len(ej) == 1 and ej[0]["role"] == "Programme Director" and ej[0]["from_year"] == 2015, ej)
    chk("ha1s-e flat highest_qualification composed from rows", "PhD Engineering, MIT (2001)" in (sapp.get("highest_qualification") or ""), sapp.get("highest_qualification"))
    chk("ha1s-f flat professional_certifications composed from rows", "PMP, PMI (2010)" in (sapp.get("professional_certifications") or ""), sapp.get("professional_certifications"))

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

    # After approval the board can still send the ID-verification link; the page is told the stage.
    c, apsl = jget("POST", f"/api/admin/honorary-applications/{aid}/shortlist", token=admin)
    chk("ha3h2 verification link can be sent AFTER approval", c == 200 and "token=" in str(apsl.get("link", "")), apsl)
    aptok = str(apsl["link"]).split("token=")[1]
    c, apctx = jget("GET", f"/api/honorary-idv/{aptok}")
    chk("ha3h3 token GET reports stage=approved for congratulatory wording", c == 200 and apctx.get("stage") == "approved", apctx)

    # ---------- shortlist-gated identity verification ----------
    print("\n=== HA3i. Shortlist → secure identity verification (photo + gov ID + background declaration) ===")
    idv_app = dict(BASE_APP, email="idv.candidate@example.com")
    c, ib = jget("POST", "/api/honorary-application", body=idv_app)
    iref = ib.get("reference")
    c, ilst = jget("GET", "/api/admin/honorary-applications?status=pending_review", token=admin)
    iaid = next(r["id"] for r in ilst["rows"] if r.get("reference") == iref)

    chk("ha3i-a public IDV with a bogus token → 404", req("GET", "/api/honorary-idv/deadbeefdeadbeefdeadbeef")[0] == 404)
    chk("ha3i-b shortlist requires owner (student_manager 403)", jget("POST", f"/api/admin/honorary-applications/{iaid}/shortlist", token=smgr)[0] == 403)
    chk("ha3i-c shortlist requires auth (401)", req("POST", f"/api/admin/honorary-applications/{iaid}/shortlist")[0] == 401)

    c, sl = jget("POST", f"/api/admin/honorary-applications/{iaid}/shortlist", token=admin)
    chk("ha3i-d owner shortlist returns a secure link", c == 200 and "token=" in str(sl.get("link", "")), sl)
    tok = str(sl["link"]).split("token=")[1]

    c, ctx = jget("GET", f"/api/honorary-idv/{tok}")
    chk("ha3i-e candidate opens the link (name returned, not submitted)", c == 200 and ctx.get("first_name") == "Grace" and ctx.get("already_submitted") is False, ctx)

    base_idv = {"photo": PDF, "government_id": PDF, "declaration_truthful": True, "background_declaration": True, "consent": True}
    chk("ha3i-f missing photo → 400", jget("POST", f"/api/honorary-idv/{tok}", body=dict(base_idv, photo=""))[1].get("error") == "photo_required")
    chk("ha3i-g missing government ID → 400", jget("POST", f"/api/honorary-idv/{tok}", body=dict(base_idv, government_id=""))[1].get("error") == "government_id_required")
    chk("ha3i-h missing background declaration → 400", jget("POST", f"/api/honorary-idv/{tok}", body=dict(base_idv, background_declaration=False))[1].get("error") == "background_required")
    chk("ha3i-i missing consent → 400", jget("POST", f"/api/honorary-idv/{tok}", body=dict(base_idv, consent=False))[1].get("error") == "consent_required")

    c, sub = jget("POST", f"/api/honorary-idv/{tok}", body=base_idv)
    chk("ha3i-j valid submission accepted", c == 200 and sub.get("ok") is True, sub)
    chk("ha3i-k the one-time token is burned (reuse → 404)", jget("POST", f"/api/honorary-idv/{tok}", body=base_idv)[0] == 404)

    c, idet = jget("GET", f"/api/admin/honorary-applications/{iaid}", token=admin)
    kinds = sorted(d["doc_kind"] for d in idet.get("idv_documents", []))
    chk("ha3i-l admin sees exactly the photo + government ID", kinds == ["government_id", "photo"], kinds)
    chk("ha3i-m application marked idv submitted with background declaration", idet["application"]["idv_status"] == "submitted" and idet["application"]["background_declaration"] == 1, idet["application"].get("idv_status"))
    idvdoc = idet["idv_documents"][0]["id"]
    chk("ha3i-n owner downloads an IDV document (200)", req("GET", f"/api/admin/honorary-applications/{iaid}/idv/{idvdoc}/file", token=admin)[0] == 200)
    chk("ha3i-o non-owner refused the IDV document (403)", req("GET", f"/api/admin/honorary-applications/{iaid}/idv/{idvdoc}/file", token=smgr)[0] == 403)
    chk("ha3i-p anon refused the IDV document (401)", req("GET", f"/api/admin/honorary-applications/{iaid}/idv/{idvdoc}/file")[0] == 401)

    c, dl = jget("POST", f"/api/admin/honorary-applications/{iaid}/idv/delete", token=admin)
    chk("ha3i-q owner deletes identity documents (data minimisation)", c == 200 and dl.get("deleted") == 2, dl)
    c, idet2 = jget("GET", f"/api/admin/honorary-applications/{iaid}", token=admin)
    chk("ha3i-r documents gone + status 'deleted' after deletion", len(idet2.get("idv_documents", [])) == 0 and idet2["application"]["idv_status"] == "deleted", idet2["application"].get("idv_status"))

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
