# Universal Document Viewing, Download, Upload & Replacement — Delivery Report

> Increment on top of the existing document subsystem (see `docs/DOCUMENT_MANAGEMENT.md` for the
> base module and `docs/documents/PHASE0_AUDIT.md` for the full Phase 0 inventory, traceability
> matrix and defect list). This report covers what THIS increment changed, the tests that prove it,
> and what remains.

## 1. What was delivered

### Backend (.NET, MySQL + SQLite parity via `Db.TranslateFor`)

| Change | Where | Why |
|---|---|---|
| **Restore endpoint** — `POST /api/admin/documents/{id}/restore` `{version_id, reason?}` creates a NEW head version whose file is the chosen older version's file; refuses cross-chain ids; never rewrites or erases newer history | `Endpoints/Documents.cs` | required "Restore previous version" with auditable restoration |
| **Replacement provenance** — `documents.replace_reason` + `documents.restored_from_id` (additive `AddCol` migration); `/version` and `/restore` accept `reason`; version history returns reason/restored-from/uploader | `Data/Migrate.cs`, `Endpoints/Documents.cs` | "record who replaced it, when and why" |
| **In-app viewing** — `?inline=1` on the student, admin and partner document serve routes and the books route: serves `Content-Disposition: inline` and audits the access as a **view**, not a download (watermarks still applied) | `Endpoints/Documents.cs`, `Endpoints/Books.cs` | honest audit trail for the new viewer |
| **Publish-locked public-document bytes** — `POST /api/admin/public-documents/{id}/file` now returns **409 `published_file_immutable`** once a version has left the pre-publication pipeline; replacing requires `/replace` (new version, history kept) | `Endpoints/PublicDocuments.cs` | P0: a published, legally-reviewed PDF could be byte-swapped in place with no history |
| **Retention-sweep protection** for `public-docs` and `cv` storage categories | `Core/Storage.cs` | P0: published governance PDFs (and applicant CVs) could silently age out |
| **Staff ticket-attachment routes** — `GET /api/support/tickets/{id}/attachments` (+ `/{aid}` stream, inline-capable), gated `inbox`, every staff read audit-logged | `Endpoints/Casework.cs` | support staff previously could not see what students attached |
| **Student evidence view-back** — `GET /api/me/appeals/{id}/evidence`, `/api/me/accommodations/{id}/evidence`, `/api/me/cpd/{id}/evidence` (own rows only, legacy inline data-URIs still served) | `Endpoints/Casework.cs` | students could upload evidence but never see it again |
| **Admin book file route** — `GET /api/admin/cert-documents/{id}/file` (cert-scoped, logged, inline-capable); book replacement now logs the previous sha256 → new sha256 | `Endpoints/Books.cs` | admins couldn't verify uploads; replacement had zero provenance |
| **Sensitive-read audit logging** on: student government-ID views, appeal / accommodation / CPD evidence views, founding evidence, honorary application documents, training-partner documents, careers CVs, admin public-document file (any-status route) | `AdminStudents.cs`, `Casework.cs`, `Founding.cs`, `HonoraryApplication.cs`, `TrainingPartners.cs`, `Careers.cs`, `PublicDocuments.cs` | those reads were previously invisible |

Migrations are additive (`AddCol` guard, idempotent, both providers). No table/column is renamed,
dropped or rewritten; every existing document record, assignment, grant, acknowledgement, audit row
and storage reference is preserved. Historical signed links keep working (path and semantics
unchanged).

### Frontend (React + TypeScript, both SPAs)

**Shared primitives (new)**
- `src/files.ts` — `fmtBytes`, `fileToDataUri`, `fetchBlob` (authenticated; never surfaces an HTML
  error page as a "file" — non-2xx becomes a typed error with the backend's message), `saveBlob`,
  `downloadFile`, `fileKind` / `fileTypeLabel`, `parseCsvPreview` (quote-aware, capped). Replaces
  12+ page-local copies of the blob-download dance and 7 copies of the FileReader data-URI reader.
- `src/components/documents/DocumentViewer.tsx` — the secure in-platform viewer: fetches over the
  authenticated inline endpoint into a Blob (no storage URL ever appears in markup), renders PDF in
  the browser's native viewer (page navigation, zoom, text search, print), images, plain text, and
  CSV as a safe tabular preview (rendered as text — markup in cells cannot inject); Office/ZIP/other
  get an honest fallback with Download when permitted. Full-page modal with title/version/type,
  full-screen toggle, print (PDF, when permitted), download (when permitted), Esc/backdrop close,
  focus management, loading and corrupt/unsupported error states.
- `src/components/documents/DocumentActions.tsx` — `DocumentRow` (icon chip, title, version badge,
  meta line, description, actions area) and `ViewDownloadActions` (the universal **View + Download**
  pair: self-contained viewer state, busy labels, inline — not `alert()` — errors, `canView` /
  `canDownload` / `canPrint` flags).
- `src/components/documents/FileUploadField.tsx` — shared picker with drag-and-drop, accept list,
  client-side size validation (mirroring the backend cap), chosen-file echo and remove.
- `useTSafe` in `src/i18n/index.tsx` — the components translate in the student SPA and fall back to
  English in the admin SPA (which has no i18n provider).
- New **`doc.*` i18n namespace** (~45 keys) translated in all 7 locales (en, ko, **ar**, es, fr, zh,
  ru) + RTL CSS rules for document rows/actions and the viewer header; new `.docviewer-*` and
  `.upload-drop` styles are responsive (full-screen on mobile).

**Student panel integration**
- **My Documents** (`pages/Documents.tsx`) — rewritten on the universal components; every
  downloadable document now has **View AND Download** (view-only ⇒ View only, no Download/Print);
  acknowledge / restricted / locked flows preserved; books section gets View + Download (watermarked
  inline views stay watermarked and audited); whole page fully translated (was 100% hardcoded EN).
- **Appeals & Accommodations** (`pages/Appeals.tsx`) — submitted evidence is now viewable/
  downloadable back by its owner; upload switched to the shared drag-drop field; client cap fixed to
  the real backend limit (was advertising 5 MB against a 3 MB server cap).
- **CPD** (`pages/Cpd.tsx`) — new Evidence column: attach evidence (returns the entry to review, as
  the backend already did) and view/download what's on file. This UI simply didn't exist.
- **Certifications → identity document** (`pages/Certifications.tsx`) — students can now View the ID
  they have on file (view-only; no download button) next to Replace.

**Admin panel integration**
- **Documents** (`admin/pages/Documents.tsx`) — View joins Download at document level; the version
  history gains per-version **View / Download / Restore** (restore prompts for a reason), a checksum
  column and the recorded replacement reason; "upload new version" gains an optional reason field.
- **Downloads Centre** (`admin/pages/PublicDownloads.tsx`) — fixed the broken Preview (was a plain
  `<a href>` on a bearer-gated endpoint → guaranteed 401); now the universal View + Download pair;
  the in-place "Replace file" button is hidden once a version is published (matching the new 409).
- **Books** (`admin/pages/Books.tsx`) — View/Download of the stored file (previously upload-only,
  blind).
- **Support Inbox** (`admin/pages/SupportInbox.tsx`) — new Attachments card in the conversation
  drawer over the new staff routes.
- **Casework** and **CPD review** — evidence buttons upgraded to the universal View + Download pair
  over the (now audited) admin evidence routes.

## 2. Security model (unchanged guarantees, extended coverage)

- Every serve endpoint re-authorises per request (assignment/ownership/entitlement + status +
  restriction + acknowledgement); a guessed id or copied link cannot bypass (existing IDOR tests +
  new ones for evidence view-back and staff attachments).
- The viewer holds bytes only as a transient object URL from an authenticated fetch; no permanent
  object-storage URL is ever placed in markup; CSV/text render as text (no HTML injection).
- Intake validation is server-side (allow-list, size, magic bytes, malware seam) — client checks are
  UX only.
- Watermarked documents stay watermarked in the viewer (inline serve runs the same stamp), and the
  audit distinguishes view vs download.
- Sensitive reads (government IDs, case evidence, application files, CVs, internal public-doc
  versions) are now all logged with actor + subject.

## 3. Tests

Backend (`tests/integration_test.py`, runs on SQLite and MySQL in CI):
- §17 additions: restore creates v3 from v1's bytes with history intact (`17va–17vd`), cross-chain
  restore refused (`17ve`), student cannot restore (`17vf`), inline view returns identical bytes and
  audits as a **view** (`17vg–17vh`), replacement reasons recorded per version (`17vd`).
- §30 additions: staff list/stream ticket attachments + audit log + student refused staff routes
  (`30f–30i`).
- New §30B: appeal/accommodation/CPD evidence view-back — exact bytes, own-rows-only isolation,
  anonymous refused, admin reads audit-logged (`30B-a–30B-k`).
- §57 addition: published public-document bytes are immutable in place — 409 + bytes unchanged
  (`57k2`).

Frontend (vitest):
- `src/files.test.ts` — byte formatting, kind detection, CSV parsing (quotes/CRLF/cap), fetchBlob
  success/denial/network paths (a JSON error body never becomes a "file").
- `src/components/documents/DocumentActions.test.tsx` — row rendering, version badge, permission
  flags hide actions, viewer opens/fetches with token/closes on Esc, inline error on failed
  download, CSV safe-table rendering, unsupported-type fallback with download, denial → viewer error
  state. Rendered **without** an i18n provider on purpose (the admin-SPA case).
- `src/pages/Documents.test.tsx` — extended: View+Download pairing, view-only ⇒ no Download,
  viewer dialog opens from the row, books section actions, plus all prior state-machine branches.

Results are recorded in the PR description for the exact run.

## 4. Deployment & rollback

- Deploy is a normal build; migrations self-apply at boot (additive, idempotent, both providers).
- No new environment variables. No new dependencies.
- Rollback: redeploy the previous build. The two new columns and new routes are inert to the old
  code; no data transformation occurred. The 409 publish-lock and category protections disappear on
  rollback (they are code-level guards), everything else degrades cleanly.

## 5. Remaining work (recorded, not silently dropped)

Tracked from the Phase 0 audit; in rough priority order:
1. **`cert_documents` (books) true version chain** — replacement now logs old→new checksum and old
   bytes remain recoverable in content-addressed storage, but there is no first-class version table
   or restore UI for books.
2. **Certificate custom-PDF vs regenerate** still share one `pdf_ref` with no version chain (each
   overwrite is at least audited in `certificate_downloads`). An explicit reissue workflow is the
   right long-term shape.
3. **Server-generated Office previews** — DOCX/XLSX/PPTX currently get the honest download fallback;
   a server-side conversion (e.g. LibreOffice worker) would enable inline preview without exposing
   files to a third-party viewer.
4. **Upload progress via XHR** — uploads are JSON/base64 (backend contract); progress is currently
   indeterminate busy-state. Real per-byte progress needs an XHR/stream path.
5. **Comms/email attachments and Events resources** — no attachment columns exist (`comm_outbox`,
   `events`); building them is new-feature work, inventoried in the matrix.
6. **GDPR erasure gaps** for `documents`/`job_applications`/honorary+partner application files, and
   content-addressed dedupe vs per-record delete (needs refcounting) — pre-existing, documented in
   the audit (§3.12).
7. **Receipt/invoice PDFs** — receipts remain client-rendered printable HTML; no stored artefact.
8. **Admin SPA i18n** — the admin panel remains English-only (pre-existing; the shared components
   are ready via `useTSafe`).
