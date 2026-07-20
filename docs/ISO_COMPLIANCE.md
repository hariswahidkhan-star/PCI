# PCI Platform — ISO Compliance Readiness Pack

> **What this is.** A control-by-control mapping of the PCI platform against the ISO standards in
> scope, showing where the software already satisfies a control, where the *organization* must act,
> and what remains open. Technical detail is in [`SECURITY.md`](./SECURITY.md).
>
> **What this is not.** ISO certification. Certification to any ISO standard is granted only by an
> **accredited certification body** after auditing your organization's management system, policies,
> risk assessment and records — software cannot confer it. This pack makes the platform
> *certification-ready* and provides the technical evidence an auditor will ask for.
>
> **Standards in scope (all confirmed by the owner):**
> - **ISO/IEC 27001 + 27002** — Information Security Management System (ISMS) and controls.
> - **ISO/IEC 27701** — Privacy Information Management (PII) extension.
> - **ISO/IEC 27017 / 27018** — cloud security and protection of PII in the cloud.
> - **ISO/IEC 17024** — bodies operating certification of persons (the certification *scheme*).
>
> Status legend: ✅ implemented in the platform · ◒ partial / operator action needed · ○ organizational (outside software).

---

## A. How certification actually works (read first)

An ISO 27001 certificate requires the organization to run an **ISMS**: a defined scope, a
**risk assessment + treatment plan**, a **Statement of Applicability** (which Annex A controls
apply and why), documented policies, assigned roles, internal audits, a management review, and
records showing the controls operate over time. The platform provides the **technical controls and
evidence**; PCI (the organization) must provide the **management system**. The realistic path:

1. Define ISMS scope (this platform + the team that runs it).
2. Run a risk assessment; produce the Statement of Applicability using Section C below.
3. Adopt the policy set (PCI already publishes most — see Section B).
4. Operate for a period (typically 3+ months) keeping records (audit log, access reviews, incidents).
5. Engage an accredited certification body for Stage 1 (documentation) then Stage 2 (audit).

27701 rides on top of a 27001 ISMS; 27017/27018 are cloud-control extensions assessed in the same
audit; 17024 is a **separate** accreditation (e.g. via ANAB/UKAS) about the certification scheme,
not data security — the platform supports it but the scheme rules are organizational.

---

## B. Policies already published by the platform

These public/records pages map directly to required ISMS/PIMS documentation (served from
`backend/wwwroot/*.html` and seeded in `Data/PublicDocsSeed.cs`):

| Policy | Page |
| --- | --- |
| Global Privacy Policy / notice | `privacy.html` |
| Data Protection Policy | `data-protection-policy.html` |
| Data Retention & Secure Deletion | `records-retention.html` |
| Confidentiality Policy | `confidentiality-policy.html` |
| Impartiality Policy | `impartiality-policy.html` |
| Certification Decision Policy | `certification-decision-policy.html` |
| Examination Security & Integrity | `examination-security.html`, `certification-integrity.html` |
| Appeals & Complaints | `appeals.html`, `appeals-form.html` |
| Disciplinary / Sanctions | `disciplinary-policy.html`, `sanctions-policy.html` |
| Professional / Membership Conduct | `professional-conduct.html`, `membership-conduct.html` |
| Recertification & CPD | `cpd-policy.html` |
| Accessibility Statement | (seeded) Reasonable Accommodation & Accessibility Policy |
| Honorary IDV Privacy Notice | (seeded) + `honorary-verification.html` |

**Still to author (organizational):** Information Security Policy (ISMS top-level), Access Control
Policy, Incident Response Plan, Business Continuity/Backup Policy, Supplier/Cloud Security Policy,
Cryptographic Key Management Policy, Acceptable Use Policy. Templates should reference the technical
controls in `SECURITY.md`.

---

## C. ISO/IEC 27001 Annex A control mapping (2022 control set)

### 5. Organizational controls
| Control | Status | Evidence / action |
| --- | --- | --- |
| 5.7 Threat intelligence | ◒ | AI-crawler/bot policy + audit log; formal threat-intel process is organizational. |
| 5.10 Acceptable use of information | ○ | Publish an Acceptable Use Policy. |
| 5.15 Access control | ✅ | RBAC deny-by-default, per-user scoping, partner isolation (`SECURITY.md` §2). |
| 5.16 Identity management | ✅ | Server-derived identity from hashed bearer sessions; no client-supplied role. |
| 5.17 Authentication information | ✅ | bcrypt, CSPRNG tokens hashed at rest, TOTP, recovery flows (§1). |
| 5.18 Access rights (review) | ◒ | Team & Access console + audit log; schedule periodic access reviews (org). |
| 5.23 Cloud services security | ◒ | See 27017/27018 (Section E); provider config is operator-set. |
| 5.28 Collection of evidence | ✅ | Immutable audit log, impersonation ledger, login/failed-login events. |
| 5.30 ICT readiness for continuity | ○ | Managed-DB backups + Render redeploy; document a BC/DR plan. |
| 5.34 Privacy & PII protection | ✅ | See 27701 (Section D). |

### 6. People controls
| Control | Status | Evidence / action |
| --- | --- | --- |
| 6.3 Security awareness/training | ○ | Organizational — train staff who operate the console. |
| 6.7 Remote working | ○ | Covered by admin MFA + session controls; document policy. |
| 6.8 Reporting security events | ◒ | Error-reference + audit surfaces exist; publish a reporting channel. |

### 7. Physical controls
| Control | Status | Evidence / action |
| --- | --- | --- |
| 7.x Physical security | ○ | Inherited from the cloud provider (Render + managed MySQL) — obtain their SOC 2 / ISO 27001 attestations. |

### 8. Technological controls
| Control | Status | Evidence / action |
| --- | --- | --- |
| 8.1 User endpoint devices | ○ | Organizational device policy for operators. |
| 8.2 Privileged access rights | ✅ | Owner-only gates; least-privilege RBAC; impersonation is read-only + audited. |
| 8.3 Information access restriction | ✅ | Per-user/tenant query scoping; owner-only IDV access. |
| 8.5 Secure authentication | ✅ | bcrypt + MFA + per-account lockout + timing-equalisation (§1). |
| 8.9 Configuration management | ✅ | Fail-closed production preflight (`Program.cs`); IaC in `render.yaml`. |
| 8.12 Data leakage prevention | ✅ | PII minimisation; no secrets/PII in logs or client error bodies. |
| 8.13 Information backup | ◒ | Enable managed-MySQL automated encrypted backups (operator). |
| 8.15 Logging | ✅ | Audit log, login events, impersonation ledger, integration delivery ledger. |
| 8.16 Monitoring activities | ◒ | Failed-login + audit events captured; wire alerting (operator). |
| 8.20 Network security | ✅ | HTTPS/HSTS, CORS lockdown, egress SSRF guard (§4–5). |
| 8.23 Web filtering | ✅ | Outbound egress guard blocks internal/metadata targets. |
| 8.24 Use of cryptography | ✅ | AES-256-GCM at rest (IDs/credentials), bcrypt, TLS in transit, `MYSQL_SSL=required`. Author a Key Management Policy (org). |
| 8.25 Secure development lifecycle | ✅ | 5-dimension security audit, 406-assertion suite (SQLite+MySQL), 500-route crash sweep, CI gates. |
| 8.26 Application security requirements | ✅ | Injection/upload/SSRF/authz controls (§2, §4). |
| 8.28 Secure coding | ✅ | Parameterised SQL, output sanitisation, content-addressed storage. |

---

## D. ISO/IEC 27701 (Privacy / PIMS)

| Requirement | Status | Evidence / action |
| --- | --- | --- |
| Lawful basis, transparency (privacy notices) | ✅ | Global Privacy Policy + honorary IDV privacy notice; consent capture with timestamps. |
| Data minimisation | ✅ | Honorary IDV stores image + attestation booleans only — no ID numbers/criminal detail. |
| Consent management | ✅ | Consent records (`/api/me/consents`), T&C acceptance on honorary application. |
| Data subject rights (access, erasure) | ✅ | `/api/me/account-data` export, `/api/me/delete-request`, targeted IDV deletion. |
| Retention & secure deletion | ✅ | Retention service performs real deletion of expired evidence/IDV; policy published. |
| PII encryption at rest & in transit | ✅ | AES-256-GCM for ID documents + credentials; TLS + `MYSQL_SSL`. |
| International transfers (KSA / US / Pakistan) | ◒ | Cross-border framework + notices present; complete a transfer impact assessment per jurisdiction (org + legal). |
| Records of processing (RoPA) | ○ | Draft a Record of Processing Activities using the data map in `Data/Migrate.cs`. |
| DPO / privacy roles | ○ | Assign a privacy owner. |

> Jurisdiction note (unchanged): this is **not legal advice**. Jurisdiction-specific legal review
> of the honorary/IDV wording and cross-border transfers (KSA PDPL, US state laws, Pakistan PDPB)
> remains recommended before production launch.

---

## E. ISO/IEC 27017 & 27018 (cloud + cloud PII)

| Control theme | Status | Evidence / action |
| --- | --- | --- |
| Shared-responsibility clarity | ◒ | App-layer controls here; obtain Render + managed-MySQL security attestations for the infra layer. |
| Encryption of customer data in the cloud (27018) | ✅ | Envelope encryption of stored PII + S3 SSE + `MYSQL_SSL=required`. |
| Data location / return / deletion | ✅ | Retention/deletion + export APIs; choose a MySQL region that meets residency needs. |
| Admin operations logging | ✅ | Audit log + impersonation ledger. |
| Key management | ◒ | `CREDENTIAL_ENCRYPTION_KEY` is operator-held; document rotation and consider a managed KMS. |
| No use of PII for provider's own purposes (27018) | ○ | Ensure DPA terms with Render / MySQL / email providers. |

---

## F. ISO/IEC 17024 (certification scheme — separate accreditation)

The platform *supports* a 17024-aligned scheme; the scheme rules and impartiality governance are
organizational.

| Requirement | Status | Evidence |
| --- | --- | --- |
| Impartiality & conflict management | ✅/○ | Impartiality Policy + committee terms published; governance is organizational. |
| Exam security & integrity | ✅ | SecureExam, proctoring, exam-misconduct policy, held-results, delivery-vendor controls. |
| Certification decisions & records | ✅ | Decision policy; certificate PDFs with QR verification; credential lookup + audit. |
| Appeals & complaints | ✅ | Appeals workflow + forms. |
| Recertification / CPD | ✅ | CPD policy + tracking. |
| Confidentiality of candidate information | ✅ | Encryption, access control, minimisation (Sections C/D). |
| Non-discrimination / accessibility | ✅ | Accommodation & accessibility policy. |

> The platform currently presents ISO/IEC 17024 as a **framework reference**, not a claim of
> accreditation — the disclaimer in the site footer states PCI is not yet accredited. Keep that
> wording until an accreditation body grants it.

---

## G. Priority actions to reach certification-readiness

**Operator / environment (do now):**
1. Set `CREDENTIAL_ENCRYPTION_KEY` (prod won't boot without it) and store it in a vault.
2. Confirm `MYSQL_SSL=required` and enable encrypted automated DB backups.
3. Enrol admin TOTP 2FA for owner + all privileged admins.
4. Obtain security attestations + signed DPAs from Render, the MySQL provider, and the email provider.

**Organizational (ISMS/PIMS):**
5. Write the ISMS top-level policies (Section B "still to author").
6. Produce the risk assessment, Statement of Applicability (use Section C), and RoPA (Section D).
7. Complete jurisdiction transfer impact assessments (KSA/US/Pakistan) with legal counsel.
8. Stand up incident-response + access-review + internal-audit cadences, then engage a certification body.

**Software roadmap (tracked in `SECURITY.md` §8):** nonce-based CSP, field-level encryption of
integration secrets, current-password on voluntary change, enforced owner 2FA + backup codes.

_Last reviewed 2026-07. Prepared as internal readiness documentation; not a certificate, audit
opinion, or legal advice._
