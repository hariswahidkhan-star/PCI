# Universal Document System — Phase 0 Audit & Traceability Matrix

> Inventory of every document-bearing module, route, API, MySQL table and storage location on the
> PCI platform, taken before the universal document-experience work. Sources: full sweep of
> `backend/Endpoints/*` (60 files), `backend/Core/*` storage/watermark/access primitives,
> `backend/Data/*` schema installers, both React SPAs (`frontend/src/pages`, `frontend/src/admin`),
> `schema.sql` / `schema.mysql.sql`, seeds and `wwwroot` static assets.

## 1. Existing foundation (strong — reused, not rebuilt)

The platform already has a real, private, audited document subsystem:

| Capability | Where | Notes |
|---|---|---|
| Private content-addressed storage (local/S3) | `Core/Storage.cs` | `provider:rel` refs, AES-256-GCM at rest, SHA-256 addressing, path-traversal guards |
| Document intake (25 MiB, 13 types, magic bytes, malware seam) | `Core/DocStore.cs` | scan seam `ScanClean` rejects MZ/ELF; safe display-filename sanitiser |
| Assigned-documents module w/ version chain | `Endpoints/Documents.cs` | `root_id/supersedes_id/superseded_by`, replace never overwrites |
| Audience resolution + per-student grants | `Core/DocAccess.cs` | `document_assignments`, UNIQUE(document_id,user_id), preview, revoke |
| Access audit incl. denials | `document_downloads` | every view/download attempt with result + IP + version |
| Acknowledgement gating | `document_acknowledgements` | idempotent, IP-recorded |
| Per-recipient PDF watermark at serve time | `Core/PdfWatermark.cs` | master never modified; honest `ok_unwatermarked` fallback |
| Short-lived signed links (5 min HMAC) | `Documents.cs` | access re-checked on redemption |
| Public Downloads Centre w/ legal-review lifecycle | `Endpoints/PublicDocuments.cs` | `doc_group` + string version + `is_current` chain |
| Books/BoK with entitlement + watermark + copy id | `Endpoints/Books.cs`, `cert_documents` | `cert_document_downloads` audit |
| Certificates (generated + custom PDF) | `Endpoints/Certificates.cs`, `Core/CertIssue.cs` | `certificate_downloads` audit |
| Retention sweep w/ protected categories | `Core/RetentionService.cs`, `Storage.PurgeOlderThan` | metadata always kept; only bytes age out |

## 2. Traceability matrix

Legend for **Actions**: V=View, D=Download, U=Upload, R=Replace, H=Version history, A=Assign, P=Publish, X=Archive/Delete.
**Bold** = required by the universal-document programme but missing at audit time.

| Module | Page/route | Doc type | Owner | Audience | Actions (audit) | API | MySQL table(s) | Storage (category) | Permission | Retention | Watermark | Defect/status |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| My Documents | `/app/documents` → `pages/Documents.tsx` | assigned private docs | admin | student | V(view_only only) D Ack — **V+D pairing, viewer** | `/api/me/documents*` | documents, document_assignments, document_downloads, document_acknowledgements | Storage `documents` (protected) | active grant + status gates | never purged | per-recipient PDF | no in-app viewer; hardcoded EN |
| Books & study materials | same page, `BooksSection` | handbook/bok/study guide | admin | entitled/credentialed student | D — **V** | `/api/me/cert-documents*` | cert_documents, cert_document_downloads | Storage `books`/`documents` (protected) | entitlement or credential + route_key | never purged | per-copy id | replace overwrites `storage_ref` in place (no history) |
| Student resources (links) | `/app/resources` | public URL list | admin | student | open link | `/api/me/downloads` | resources | external URLs | session | n/a | n/a | URL-only by design |
| Free templates | `/app/templates` | CSV templates | admin | student | D | `/api/me/templates*` | templates (+download aggregates) | inline `body` TEXT | session, published only | n/a | n/a | PATCH overwrites body (no versions) |
| Credentials/certificates | `/app/credentials` | credential PDF, honorary PDF | system/credential admin | holder | D verify badge | `/api/me/certificate/pdf` etc. | issued_credentials(pdf_ref,pdf_sha256), honorary_awards, certificate_downloads | Storage `certificates` (protected) | own credential, revocation blocked | never purged | none (policy) | upload-certificate/regenerate clobber one `pdf_ref` |
| Identity document | `/app/certifications` | passport/ID | student | student + members admin | U R(list of rows) — **V/D of own doc** | `/api/me/identity-document*` | identity_documents | Storage `idd` (purgeable) | own rows; admin `members` | evidence window | none | student cannot view own ID; admin view unaudited |
| Appeals & accommodations | `/app/appeals` | evidence | student | student + tickets admin | U — **V/D back** | `/api/me/appeals`, `/api/me/accommodations` | appeals, accommodation_requests (`evidence_data` = ref or legacy inline) | Storage `appeal`/`accommodation` (purgeable) | own rows; admin `tickets` | evidence window | none | student cannot see own evidence after submit; admin reads unaudited |
| CPD evidence | `/app/cpd` | evidence | student | student + members admin | (backend U existed; **no student UI, no student D**) | `/api/me/cpd/{id}/evidence` | cpd_entries (`evidence_data`) | Storage `cpd` (purgeable) | own entry; admin `members` | evidence window | none | upload overwrites prior evidence; UI missing |
| Support tickets | `/app/support` | attachments | student | student + inbox staff | U(student) V/D(student) — **staff V/D** | `/api/me/tickets/{id}/attachments*` | support_attachments | Storage `support` (purgeable) | own ticket | evidence window | none | **staff inbox cannot read attachments at all** |
| Founding application | Billing → FoundingCard | CV/certificate/reference | student | student + members admin | U; admin V | `/api/me/founding-application`, admin `/evidence` | founding_applications (evidence_ref…) | Storage `founding` (protected) | own; admin `members` | never purged | none | admin evidence view unaudited |
| Honorary applications | public form + admin | résumé, academic, supporting | applicant | owner-only admin | U(public) V(owner) | `/api/honorary-applications*` | honorary_application_documents | Storage `honorary` (protected) | owner-only | never purged | none | file reads unlogged |
| Honorary IDV | tokenised link + admin | photo + government ID | applicant | owner-only admin | U(one-shot) V D X(policy delete) | `/api/honorary-idv/*` | honorary_idv_documents | Storage `honorary-idv` (protected) | one-time token; owner | dedicated idv window | none | model citizen — views ARE logged |
| Training partner applications | public form + admin | accreditation/curriculum docs | applicant | partners admin | U(public) V(admin) | `/api/training-partner-applications*` | training_partner_application_documents | Storage `partners` (protected) | admin `partners` | never purged | none | reads unaudited |
| Careers | public site + admin | applicant CVs | applicant | content admin | U(public) D(admin) | `/api/careers/{id}/apply`, admin `/cv` | job_applications (cv_ref) | Storage `cv` (**purgeable**) | admin `content` | swept at evidence window | none | CV download unaudited; posting delete orphans blobs |
| Payments | `/app/billing` | receipts (HTML print) | system | student | print view | none (client render) | payments (invoice_url/receipt_url) | none | own payments | n/a | no (policy) | no stored PDF artefact |
| Partner statements | partner portal + admin | statements/remittance PDFs/CSVs | system | partner finance | D (generated) | `/api/partner/statement` etc. | partner_settlements + ledgers | generated per request | partner role / `pf_view` | n/a | none | derived docs; no download log |
| Partner/institution documents | partner portal | agreements, invoices, kits | admin | institution | V D | `/api/partner/documents*` | documents (audience `institution`) | Storage `documents` | exact partner_id config match | never purged | institution watermark | OK |
| Public Downloads Centre | `/downloads` + admin `PublicDownloads.tsx` | policies, handbooks, legal | admin | public | V D U R H P X | `/api/public/documents*`, `/api/admin/public-documents*` | public_documents, public_document_downloads | Storage `public-docs` (**was purgeable**) | public: published+public only; admin `documents` | **was at risk** | none | `/file` overwrote published bytes in place; admin Preview link 401s; admin file reads unaudited |
| Static wwwroot downloads | `/downloads/*.pdf` | 8 governance PDFs | repo | public | D | static files | none | wwwroot on disk | anonymous | n/a | none | unmetered duplicate of public_documents copies |
| Admin Documents | `/admin/documents` | assigned docs | admin | documents admin | V? D U R(H table) A P X — **inline View, per-version D, Restore, replace reason** | `/api/admin/documents*` | (as My Documents) | | `documents` gate | | | no restore; no per-version download; no viewer |
| Admin Books | `/admin/books` | handbooks/BoK | admin | resources admin | U R P X — **V/D** | `/api/admin/cert-documents/*`, cert_documents CRUD | cert_documents | | `resources` gate | | | admin can't verify what they uploaded |
| Admin student profile | `/admin/students` | ID docs | admin | members admin | V review | `/api/admin/students/{id}/identity-document/*` | identity_documents | | `members` | | | view unaudited |
| Simulation Lab | `/admin/lab` | JSON manifests | admin | sim_lab admin | export/import w/ checksum | `/api/admin/lab/*manifest*` | simulation_scenarios | in-memory JSON | `sim_lab` | n/a | n/a | OK (content, not files) |
| PCI World | `/world` | passport PDF | system | public-by-token | V D | `/world/p/{token}.pdf` | pciworld_users | generated | token + published | n/a | n/a | derived |
| Exam evidence/proctoring | secure exam + admin | webcam frames, ID checks | system | proctoring admin | V | `/api/admin/evidence/{id}` | exam_evidence, identity_checks | Storage `evidence` (purgeable by design) | `proctoring` + cert scope | evidence window | none | view unaudited |
| Comms/email attachments | — | none | — | — | — | — | comm_outbox has no attachment column | — | — | — | — | not built (roadmap) |
| Events resources | `/app/events` | none (join_url only) | — | — | — | — | events | — | — | — | — | not built (roadmap) |

## 3. Defects & gaps found (Phase 0 output)

### P0 — data-loss / integrity
1. **Published public documents could be byte-overwritten in place.** `POST /api/admin/public-documents/{id}/file` replaced `storage_ref` on the live row at any status — including `published` after legal review — leaving no version, no history, stale `sha256` semantics. (`PublicDocuments.cs`)
2. **`public-docs` storage category was subject to the age-based retention sweep** (`Storage.ProtectedCategories` omitted it): published governance PDFs served to the anonymous public could silently vanish after `evidence_retention_days`.
3. **No restore path** for the assigned-documents version chain: an admin who replaced a document with a bad file had no auditable way back.

### P1 — missing required actions
4. **Support staff cannot read ticket attachments** — students upload into `support_attachments`, students can stream their own, but no staff route existed and the admin Support Inbox never showed them.
5. **Students cannot view/download back their own submissions** — appeal evidence, accommodation evidence, CPD evidence and identity documents were upload-only (or entirely missing UI, for CPD).
6. **No in-app viewer anywhere** — every "view" was `window.open(blobUrl)`; admin Documents had download only; no View+Download pairing on student rows.
7. **No per-version download / no replace-reason** in the admin version history.

### P2 — audit & consistency
8. **Unaudited sensitive file reads**: admin views of government IDs, appeal/accommodation/CPD evidence, founding evidence, honorary application docs, training-partner docs, careers CVs, admin public-documents file.
9. **Admin PublicDownloads "Preview" link 401s** — plain `<a href>` on a bearer-gated endpoint.
10. **12+ copy-pasted authenticated-blob download implementations** and 7 copies of the File→data-URI reader across the SPAs; 3 different byte formatters.
11. **Zero `doc.*` i18n keys** — the student Documents page is 100% hardcoded English despite 7 supported locales (incl. Arabic RTL); RTL CSS lacked rules for the document list layout.
12. Two intake pipelines with different policy (3 MB unscanned `Storage.DecodeDataUri` vs 25 MiB scanned `DocStore.Decode`); books replace overwrites in place; certificate upload/regenerate fight over one `pdf_ref`; content-addressed dedupe means per-record delete can break a twin record (no refcount); GDPR erasure misses several document tables.

## 4. Migration-risk notes

- All document tables are created via idempotent `CREATE TABLE IF NOT EXISTS` in `Data/Migrate.cs`
  (SQLite dialect, translated for MySQL at runtime by `Db.TranslateFor`); columns are added via the
  `AddCol` guard. Any new column must be `AddCol`-based and any indexed string column must be
  `VARCHAR(n)`, never bare `TEXT` (MySQL cannot index TEXT).
- Datetimes are stored as strings on both providers; `datetime('now')` is rewritten for MySQL.
- `INSERT OR IGNORE` + `ExecuteWithChanges` is the sanctioned idempotent-upsert pattern.
- The retention sweep only deletes bytes, never metadata rows — protecting a category is purely
  additive and cannot orphan rows.

## 5. What Phase 1–4 changed (see `UNIVERSAL_DOCUMENTS.md` for the full delivery report)

- P0 items 1–3 fixed (publish-locked byte replace, protected category, restore endpoint + reason).
- P1 items 4–7 delivered (staff attachment routes, student view-back routes for appeals /
  accommodations / CPD / identity docs, universal viewer + View/Download pairing, per-version
  download + restore + replace-reason in admin).
- P2 items 8–11 delivered (read audits, Preview fix, shared `files.ts` + `DocumentViewer` /
  `DocumentActions` components, `doc.*` i18n namespace across all 7 locales + RTL rules).
- Remaining roadmap items are listed in the delivery report §Remaining work.
