# Student Documents & Resources — Implementation Report

> A complete admin-upload / student-download document module: an administrator uploads a document, assigns
> it to the correct student or group, publishes it, sees it reflected in the student panel, the student
> downloads it securely, and permissions, versioning, notifications and audit all work end to end.
> MySQL throughout. All prior functionality preserved — this **extends**, it does not rebuild.

---

## 1. Existing functionality reviewed (and preserved)

- The existing **`resources`** table + admin CRUD + student `GET /api/me/downloads` is a **public, URL-based
  link list** (the same library shown on the public site). It is **untouched** — it stays exactly as is.
- **`Storage`** already provides private, content-addressed object storage (local or S3), magic-byte
  sniffing, a size cap and a SHA-256 reference. The new module builds on it rather than duplicating it.
- **RBAC** (`Rbac.Sections`), the `gate(...)`/`Deny(...)` helpers, the audit `log(...)` callback, the
  in-app `notifications` table and the configurable `Notify` service (email + ledger) are all reused.

The gap: there was **no per-student assigned FILE** mechanism — no way for an admin to upload a document,
restrict it to specific students/groups, and have them download it privately with versioning, acknowledgement
and audit. That is what this module adds, as a distinct subsystem.

## 2. What this delivers

### Secure admin upload
- **`Core/DocStore.cs`** — a document-aware intake layer on top of `Storage`: an 18-type allow-list
  (PDF, Word, Excel, PowerPoint, CSV, text, PNG/JPG/WebP, ZIP), a **25 MB** cap, **magic-byte signature
  verification** (%PDF, PK for OOXML/zip, OLE for legacy Office, PNG/JPEG/WebP signatures, a textual guard
  for CSV/TXT), a **malware-scan seam** (`ScanClean` — rejects MZ/ELF executables today; a documented drop-in
  point for ClamAV/VirusTotal), a **content SHA-256**, and a **safe display-filename** sanitiser. Bytes land
  in **private** storage under the `documents` category; the stored key is content-addressed and unrelated to
  the display name, so no storage path is ever exposed.
- The admin upload endpoints raise the request-body cap to accommodate a 25 MB file **only on those routes**
  (the global 6 MB cap stays tight everywhere else).

### Assignment to a student or a group
- **`Core/DocAccess.cs`** resolves an audience to an explicit set of `user_id`s and materialises one
  **`document_assignments`** row per student. Supported audiences: **all**, single **student**, **multiple
  students / custom group**, by **membership** (optionally a type), by **certification** (exam/course), by
  **passed-exam**, **honorary**, by **institution** (training-partner-linked redemptions), by **discount
  code**, by **country**, and **test users**. Broad groups exclude test accounts unless `include_test` is set.
- **Recipient preview** (`POST …/preview-recipients`) returns the resolved count + a sample so an admin can
  confirm *who will receive this* before publishing. Re-assigning a live document re-materialises grants so
  newly-qualifying students gain access immediately; a previously **revoked** grant is never silently resurrected.

### Publish + lifecycle
- Eleven statuses: **draft, pending, published, active, scheduled, expired, archived, replaced, suspended,
  rejected, test**. Publishing materialises the per-student grants and notifies. A future `publish_at` yields
  **scheduled**; `expires_at` in the past reads as **expired**. A generic status action covers archive /
  suspend / reject etc.

### Versioning (never overwrites)
- **`POST …/{id}/version`** stores a brand-new object, increments the version, links the chain
  (`root_id`/`supersedes_id`/`superseded_by`), marks the old row **replaced**, and — if the prior version was
  live — publishes the new one and re-grants recipients so access is seamless. **History is retained; nothing
  is ever overwritten.** The detail view returns the full version history.

### Student "My Documents"
- **`GET /api/me/documents`** returns **only the documents assigned to that student** (active grant, visible
  status), newest first, with per-row flags: `downloadable`, `locked`/`lock_reason`, `restricted`
  (+`restricted_until`), `view_only`, `ack_required`/`acknowledged`, version.
- **`GET /api/me/documents/{id}/download`** — an **authenticated, audited** download that independently
  enforces: an active assignment, a live status, the schedule/expiry window, the **restriction window**, and
  **acknowledgement** (when required). View-only documents are served **inline** (no forced attachment).
- **Time-limited signed links** — `POST …/{id}/link` mints a **5-minute HMAC-signed** link usable without an
  `Authorization` header (for opening in a new tab / a view-only viewer); redemption re-checks access.
- **Acknowledgement** — `POST …/{id}/acknowledge` records who + when + IP (idempotent), and gates download
  when the document requires it.

### Admin surface & customer-service read access
- Admin: list (filters: status/category/search), detail (versions + recipients + ack/download status),
  edit metadata, upload version, assign, preview recipients, publish, status change, **revoke** (one student
  or all — keeps the audit row), admin download, and an **audit report** (grants, acknowledgements, distinct
  downloaders, total downloads/views + a recent-activity feed). Configurable **document categories** CRUD.
- **Student-profile Documents tab** — `GET /api/admin/students/{userId}/documents` (readable by
  **`documents`**, member managers, or **customer-service `inbox`** — CS gets read access), and
  `POST /api/admin/students/{userId}/documents` to create a **student-specific** document straight from the
  profile (upload → auto-assign to that student → publish).

### Notifications
- On publish, every active assignee gets an **in-app notification** (reliable, for all) and, best-effort and
  capped (`doc_email_notify_cap`, default 500), an **email** via the configurable `Notify` service. Governed
  by `notify_documents_enabled`. The in-app channel is the guaranteed one; email never blocks the request.

## 3. Database migrations (additive, MySQL + SQLite, idempotent)

New tables (indexed columns use VARCHAR — MySQL cannot index bare TEXT):
`document_categories`, `documents` (metadata + stored file + lifecycle + version chain),
`document_assignments` (per-student grants, UNIQUE(document_id,user_id)), `document_downloads` (immutable
view/download audit), `document_acknowledgements` (UNIQUE(document_id,user_id)). Default categories are
seeded only when empty. Two settings seeded: `notify_documents_enabled`, `doc_email_notify_cap`. All via
`CREATE TABLE IF NOT EXISTS` / idempotent index creation — nothing dropped or renamed.

## 4. Security controls

- Files stored in **private** object storage; the `documents` (and `certificates`) subtree is now **excluded
  from the retention purge** so an admin-uploaded document can never be silently deleted (it cannot be
  regenerated, unlike a certificate).
- Intake validates extension **and** declared MIME **and** magic-byte signature **and** size, plus an
  executable-header/malware seam. A renamed `.exe` is rejected (`content_mime_mismatch`); a disallowed type is
  rejected (`file_type_not_allowed`).
- Every download/view is per-request authorised (assignment + status + restriction + acknowledgement) and
  written to the immutable `document_downloads` audit. Revocation preserves the audit row (status → revoked,
  with actor + reason).
- Granular **RBAC**: all admin management is gated `documents`; the student-profile read also accepts member
  managers and customer service; students only ever see their own assigned documents.
- Signed links are HMAC-signed with a boot-stable secret and expire in 5 minutes; a tampered/expired token is
  rejected (401) and access is re-checked on redemption.
- A staff "view as student" (impersonation) session cannot acknowledge on the student's behalf.

## 5. Test cases executed & results

New integration **section 17 (39 assertions)**, on **both SQLite and MySQL**: category seed + create; secure
upload (draft); recipient preview; draft invisible to students; publish grants + in-app notification;
assignee sees a downloadable document; **isolation** (a non-assignee cannot see or download it); authenticated
download returns the exact bytes; unauthenticated download is 401; signed link works and a tampered token is
rejected; **acknowledgement gating** (blocked → acknowledge → allowed, recorded); **versioning** (old marked
replaced + retained, student gets the new bytes, history lists both); **restriction window** (visible but
locked, download 403); **group ('all')** assignment reaches many students; **revocation** removes access and
retains the audit; admin reads a student's documents; student-specific upload auto-publishes to that student;
audit report totals; **file-validation rejection** (bad signature, disallowed type); student token refused the
admin surface.

Results: **integration suite 365/365 on SQLite and 365/365 on MySQL**; **500-sweep 0 server errors** (1125
calls across 376 routes × 3 auth contexts); both SPAs build.

## 6. Watermark rendering (delivered)

**`Core/PdfWatermark.cs`** (PDFsharp — pure managed, no native libs) stamps every page of a
watermark-flagged PDF at download time with the recipient's identity: a semi-transparent diagonal
"{name} – {email}" plus a footer line "Issued to {name} (member #id) via the PCI student portal –
{date} – not for redistribution", so a leaked copy is traceable to who received it. The stored
**master is never modified and never exposed** — each download renders a fresh copy. The watermark is
drawn as raw content-stream operators with standard Helvetica (no font files needed on the server);
a prepended/appended `q`/`Q` pair keeps placement correct even when the original content leaves the
graphics state unbalanced, and inherited page resources are preserved. Institution downloads are
stamped with the institution's name instead ("Licensed to {institution}"). **Best-effort by design**:
an encrypted/unparseable PDF falls back to the original bytes and the download audit honestly records
`ok_unwatermarked` (never a silent claim). Non-PDF types are served unmodified.

## 7. Institution (partner) portal documents (delivered)

A document with the **institution** audience now reaches the partner two ways at once: the partner's
registered students get personal grants on publish (unchanged), and the institution's own portal
logins see it under a new **Documents** tab in the partner portal (`partner.html`) — so agreements,
invoices and marketing kits reach a partner even before it has students.
`GET /api/partner/documents` lists only documents whose config targets that partner's id (exact JSON
parse — never a substring match), with the same status/schedule/expiry gating as students;
`GET /api/partner/documents/{id}/download` serves the file (watermarked with the institution name when
flagged) and audits with `role='partner'`. Another institution can neither list nor fetch it.

## 8. Known limitations / scoped for a later phase

> Update: the universal-document increment (see `docs/documents/UNIVERSAL_DOCUMENTS.md`) has since
> delivered the in-app secure viewer (student + admin), per-version download + restore with a
> recorded reason, `?inline=1` view auditing, and the shared React document components.

- **In-browser view-only viewer** — DELIVERED: `src/components/documents/DocumentViewer.tsx` renders
  PDF/image/text/CSV in-app over the authenticated inline endpoint. (True copy-prevention is still not
  achievable for a downloaded file; the security guarantees are private storage + per-request
  authorisation + per-recipient watermarking + full audit.)
- **Scheduled auto-publish** and **background bulk-assignment jobs** — a scheduled document is created and
  locked until its date; going live at that moment (and very large group fan-outs) would benefit from a
  background worker, deferred. Assignment resolution + notification currently run synchronously with a capped
  email fan-out.
- **Automatic workflow-rule engine** — assignment is by explicit audience today; a fully configurable
  event-driven rule engine ("on exam pass, auto-attach document X") is a later phase.
- **Watermarking applies to PDFs** — Office/CSV/image/zip files are served as uploaded (stamping those formats
  is a different problem per format and out of scope).

## 7. Configuration & deployment

- Settings (Admin-configurable): `notify_documents_enabled` (email on/off), `doc_email_notify_cap` (max emails
  per publish). Optional `DOC_LINK_SECRET` env pins the signed-link key (otherwise derived from existing
  secrets, stable across restarts on the same install).
- Migrations are additive and idempotent (safe to re-run). No new native dependencies.
- **Rollback**: degrades safely — the added tables/columns are inert when unused; remove/redeploy the previous
  build. Existing `resources` downloads keep working.
