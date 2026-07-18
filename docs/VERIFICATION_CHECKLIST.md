# Feature Verification Checklist — Documents, Marketing, Certificates

A click-through script to confirm every feature works on a deployed build. Run it yourself, hand it to a
tester, or paste it into an AI agent as a prompt. Take a screenshot at every ✅ step if you want an
evidence pack. **Pre-requisite:** PR #52 merged to `main` and the site redeployed; an owner admin login;
one real (or test) student account.

---

## A. Documents module — admin side (Admin → Students → **Documents**)

1. Open **/admin/documents**. ✅ Page loads with status/category filters, an **Upload document** button,
   a documents table (Title/Category/Type/Status/Ver/Assigned/Acks/Downloads/Created) and a
   **Document categories** manager underneath.
2. In **Document categories**: add a category (e.g. "Onboarding"). ✅ It appears in the list; Deactivate
   and Delete work.
3. Click **Upload document**. ✅ The drawer shows Title, Description, Category, Document type, a file
   picker (PDF/Word/Excel/PowerPoint/CSV/text/PNG/JPG/ZIP, max 25 MB), an **Audience** picker
   (all / one student / multiple / membership / certification / passed / honorary / institution /
   discount code / country / test users), **Preview recipients**, and the option flags
   (View-only, Acknowledgement required, Watermark, Restricted until, Publish at, Expires at).
4. Choose a PDF, set Audience = a single student (their user id), click **Preview recipients**.
   ✅ It shows exactly 1 recipient with their name/email.
5. Try uploading a renamed `.exe` (or any file whose bytes don't match its extension).
   ✅ It is rejected with a clear message — nothing is stored.
6. Upload a real PDF and click **Publish now**. ✅ The document appears in the table as **published**
   with Assigned = 1.
7. Open the row **Detail**. ✅ You see version history (v1), the recipient list with
   acknowledged/last-download columns, Revoke buttons, an Upload-new-version input, status actions,
   an admin Download button, and the Audit summary (grants / acks / downloads / views).

## B. Documents module — student side (student portal → **Documents**)

8. Log in as that student, open **/app/documents**. ✅ "My Documents" lists ONLY the documents assigned
   to this student, grouped by category.
9. Click **Download**. ✅ The exact file downloads (authenticated — copy the URL into a logged-out tab
   and it must return 401, not the file).
10. Publish an **Acknowledgement required** document to the student. ✅ The row shows **Acknowledge**
    and download is blocked until they acknowledge; after acknowledging, Download works and the admin
    Detail view shows the acknowledgement timestamp.
11. Publish a document with **Restricted until** a future date. ✅ The student sees it listed but locked
    ("Available <date>"); download attempts are refused.
12. Upload a **new version** from the admin Detail view. ✅ The old version flips to *replaced* (still in
    history — nothing overwritten), and the student now downloads the NEW bytes under the same entry.
13. **Revoke** the student's access on any document. ✅ It disappears from their panel, downloads are
    refused, and the admin Detail still shows the revoked grant with actor + reason (audit preserved).
14. Log in as a second student. ✅ They see none of the first student's documents (isolation).
15. From Admin → Students → a student profile, call the **student documents** view
    (`/api/admin/students/{id}/documents` — surfaces as the profile Documents data). ✅ It lists that
    student's documents; a customer-service (inbox) login can read it too, but cannot upload.

## C. Marketing dashboard (Admin → Marketing → **Marketing dashboard**)

16. Open **/admin/marketing**. ✅ Acquisition KPIs render (visitors, page views, registrations,
    purchases, revenue, memberships) with a 7/30/90-day selector, plus Traffic-by-source,
    Conversions-&-revenue-by-source, and UTM-campaign-traffic tables.
17. Create a campaign draft (name, subject, audience, HTML body). ✅ It appears in the Campaigns table
    as **draft**. **Preview audience** shows deliverable vs suppressed counts before any send.
18. Open its **Detail** and send a **test** to your own address. ✅ You receive it with the `[TEST]`
    subject prefix and the auto-appended compliance footer containing a working one-click unsubscribe.
19. Click **Send** (confirm). ✅ Status moves draft → sending → sent with live sent/failed counts;
    the Detail recipient list shows per-address status.
20. Add an address to the **Suppression list**, preview the audience again. ✅ The suppressed count
    increases; that address is never mailed. Clicking an unsubscribe link in a received email adds the
    address here automatically.

## D. Certificates (already in PR #52 — spot-check)

21. As a student with a passed exam (or an admin-issued credential), open Credentials → **Download PDF**.
    ✅ A real PDF downloads with the certificate ID, holder name and a QR code.
22. Scan the QR (or open the verify link), then on the public verify page use
    **"Check a downloaded certificate file"** and pick the downloaded PDF. ✅ "Authentic — matches the
    official record". Alter one byte of a copy and re-check. ✅ Rejected.
23. Revoke the credential in Admin → Credentials. ✅ The student can no longer download it and the
    public verify page shows revoked/not valid.

## E. Watermark rendering

24. Upload a real PDF with **Watermark** ticked, assign + publish to a student, then download it AS the
    student. ✅ Every page carries a semi-transparent diagonal "{student name} – {email}" plus a footer
    "Issued to … via the PCI student portal … not for redistribution". The file's bytes differ from the
    upload (it's a stamped copy).
25. Download the same document from Admin → Documents → Detail → Download. ✅ You get the ORIGINAL,
    unstamped master — the stored file is never modified.
26. Upload a corrupted/odd PDF with Watermark ticked and download as the student. ✅ It still downloads
    (original bytes) and the document's audit shows the download as *unwatermarked* — no silent claims.

## F. Partner (institution) portal documents

27. In Admin → Documents, upload a PDF with Audience = **By institution** (pick the partner), Watermark
    ticked, and publish. ✅ Publish succeeds (student grants = that partner's registered students, which
    may be 0 — fine).
28. Log in to the partner portal (`/partner.html`) as that institution. ✅ A **Documents** tab lists the
    document with a Download button.
29. Download it. ✅ The PDF is stamped "Licensed to {institution name}" diagonally + a footer, and the
    admin audit for the document shows the download with the *partner* role.
30. Log in as a DIFFERENT institution. ✅ The document is not listed, and fetching it by id is refused.

---

## Known gaps (by design, scoped for later phases — do NOT expect these to pass)

- **View-only viewer**: view-only documents open inline in the browser; there is no dedicated
  no-download viewer (true copy-prevention of a delivered file isn't possible — the protections are
  authentication, watermark traceability and the audit trail).
- **Scheduled auto-publish**: a future-dated document sits in *scheduled* until an admin publishes at
  that time (no background scheduler yet).
- **Watermarking is PDF-only**: Office/CSV/image/ZIP files are delivered as uploaded.
