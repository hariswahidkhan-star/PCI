# Verifiable PDF Certificates — Implementation Report (Phase 1)

> This is **Phase 1** of the certificate-issuance master spec: real, downloadable, QR-bearing, tamper-evident
> PDF certificates, wired into the existing examination and honorary issuance flows. Later phases
> (certificate-template visual editor; certificate lifecycle depth — replace/supersede/version-history and
> manual exam-result recovery; personalized-book watermarking) are scoped in §"Remaining phases" below.
> MySQL throughout. All prior functionality preserved.

---

## 1. Existing functionality reviewed

- **Credentials** live in `issued_credentials`, issued on exam pass via `Lifecycle.IssueCredential`
  (SecureExam, proctoring, vendor delivery, and admin manual issue).
- **Public verification** (`GET /api/verify` + `/verify.html`) was already tamper-aware at the data level:
  it computes active/expired/revoked, treats honorary awards as a distinct type that **never** claims a
  passed exam, and hides test-account credentials.
- **Honorary awards** live in a separate registry (`honorary_awards`, `PCI-HON-*`), independently verified.
- **Storage** (`Storage`) already provides private object storage with a MIME allow-list, magic-byte
  sniffing, size caps, and content-addressed SHA-256 references.

## 2. Gap identified

Certificates existed only as **data + an HTML verify page**. There was **no PDF library and no PDF/QR
generation** — no downloadable certificate, no QR code, and no tamper-evident file hash on the record.

## 3. What Phase 1 delivers

- **`Core/CertPdf.cs`** — a dependency-free certificate PDF generator. It writes a standards-compliant PDF
  (landscape A4, the 14 standard Helvetica fonts, no font embedding) and draws the QR code as vector modules
  directly into the content stream. **No native libraries** (no SkiaSharp/System.Drawing) — output is
  byte-for-byte deterministic, which is what makes the tamper hash meaningful. QR encoding uses **QRCoder**
  (pure-managed).
- **`Core/CertIssue.cs`** — renders the PDF, stores it privately, and stamps the record with the storage
  reference, a **SHA-256 tamper hash**, a non-guessable verify token and a timestamp. Idempotent and
  best-effort: the record is authoritative, so a render failure never blocks issuance, and a missing PDF is
  produced on first download (covers historical credentials too).
- **Wiring** — `Lifecycle.IssueCredential` (every exam-pass path) and `Honorary.ConferAward` now render the
  PDF at issuance.
- **`Endpoints/Certificates.cs`** — authenticated, audited downloads:
  - `GET /api/me/certificate/pdf[?id=]` — the student's examination certificate (revoked/suspended blocked).
  - `GET /api/me/honorary-certificate/pdf` — the student's honorary certificate.
  - `POST /api/admin/credentials/{id}/regenerate-pdf` and `GET /api/admin/credentials/{id}/pdf` — admin
    regenerate / fetch (gated `credentials`). The PDF is never emailed by default; delivery is the portal link.
- **Verification** — `GET /api/verify` now returns `document_hash` (the SHA-256) and `has_pdf`, so anyone can
  independently confirm a downloaded PDF is exactly the one PCI issued (recompute + compare) without trusting
  the visual — tamper-evidence per the spec.
- **Honorary correctness** — the honorary PDF is titled **"Honorary Certificate"**, shows the recognition,
  and **never** states a passed examination. The examination PDF states the examination was satisfied.
- **Test isolation** — a test-account certificate carries a diagonal **"TEST CERTIFICATE"** watermark and is
  never publicly verifiable.
- **Frontend** — the student *My Certificates* page gains a **Download PDF** button (exam + honorary) that
  fetches the authenticated PDF as a blob.

## 4. Database migrations (additive, MySQL + SQLite)

`issued_credentials`: `pdf_ref`, `pdf_sha256`, `verify_token`, `pdf_generated_at`.
`honorary_awards`: `pdf_ref`, `pdf_sha256`, `pdf_generated_at`.
New table `certificate_downloads` (immutable download audit: credential, actor, role, ip, kind, result).
All via idempotent `AddCol`/`CREATE TABLE IF NOT EXISTS`; no columns dropped or renamed.

## 5. Security controls

- PDF stored in **private** object storage; served only through authenticated, per-request-authorised
  endpoints; revoked/suspended certificates are not downloadable as valid.
- **Tamper-evidence**: content-addressed SHA-256 stored with the record and exposed by the public verifier.
- **Deterministic render** → stable hash (verified byte-stable across downloads).
- Every download is written to the immutable `certificate_downloads` audit; admin actions also hit the main
  audit log. Test certificates are isolated from public verification.

## 6. Test cases executed & results

New integration **section 16 (18 assertions)**, on **both SQLite and MySQL**:
auto-generation at issuance; real PDF download (200 / `application/pdf` / `%PDF` magic); PDF carries the
certificate id + recipient; **public verify hash matches the downloaded file**; verify valid/active; byte-
stable re-download; admin regenerate; auth required (401); revoked → download blocked (403) + verify flips;
honorary PDF says *Honorary* and never *passed the examination*; honorary verify typed + hashed; test-cert
watermark; test cert not publicly verifiable; download audited.

Results: **integration suite 326/326 on SQLite and 326/326 on MySQL**; 500-sweep **0 server errors**;
founding/honorary/lifecycle/settings/casework/release suites green; both SPAs build.

## 7. Known limitations

- Standard Helvetica fonts cover Latin scripts; a non-Latin name (Arabic/CJK) renders as placeholders and is
  flagged (`?`) rather than embedded — full script coverage needs a font-embedding pass (a later phase).
- The certificate layout is a single built-in design; the configurable **template visual editor** is a later
  phase (see below). The current design uses only approved PCI data — no hard-coded names/dates/claims.

## 8. Configuration & deployment

- Set **`APP_BASE_URL`** (or the `public_base_url` setting) so the QR encodes an absolute verification URL.
- Migrations are additive and idempotent (safe to re-run). No new native dependencies to install — the only
  new package is the pure-managed **QRCoder**.
- **Rollback**: the feature degrades safely — remove/redeploy the previous build; the added columns are inert
  when unused and need no down-migration. Existing (data-only) verification keeps working.

## 9. Remaining phases (not in this PR)

1. **Certificate lifecycle depth** — replace/supersede/reissue with full version history; manual exam-result
   recovery for missing/failed callbacks; manual issuance UI polish.
2. **Certificate template management** — secure template upload, a field-mapping/positioning editor,
   template versioning, and sample preview.
3. **Personalized book watermarking** — a separate Books module (upload a master PDF; per-student watermark
   on every page; private authenticated download). Needs a PDF-manipulation library.

## 10. Acceptance criteria addressed by Phase 1

14 (view/download active certificates) · 17 (unique certificate ID — already present, now on the PDF) ·
18 (QR verification code) · 19 (verification page reflects status) · 20 (revoked/replaced no longer valid —
revoke path) · 22 (new template affects new certificates only — the record stores its render; regeneration is
explicit) · 24 (missing-callback recovery — download regenerates) · 27 (test certificates isolated) ·
33–34 (secure private storage + authenticated download) · plus honorary correctness (6, 7). Remaining criteria
map to the later phases above.
