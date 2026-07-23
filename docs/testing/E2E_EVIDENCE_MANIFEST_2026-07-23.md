# PCI E2E evidence manifest — 23 July 2026

All entries below are Playwright attachment names produced by `captureStoryEvidence`. They are **expected CI artifacts**, not locally claimed screenshots: this workspace can discover the tests but has no browser engines or runnable .NET host.

| Journey group | Spec | Evidence produced |
|---|---|---|
| Admin access and exports | `admin-console.spec.ts` | G2 analytics export, G3 test-user creation, authenticated cross-browser console smoke |
| Credential operator → holder → verifier | `admin-credentials.spec.ts` | G6/E6 linked issue, custom PDF, download, verify, revoke and reinstate states |
| Student support view and finance | `admin-operations.spec.ts` | D9 impersonation banner; G3 student drawer; G4 settlement/reversal; G17 audit attribution |
| Live proctor ↔ candidate | `admin-proctoring.spec.ts` | D7/G7 live event stream, chat and reviewed session |
| Admin RBAC, TOTP and settings | `admin-security-rbac.spec.ts` | G1 MFA/recovery; G18 least privilege; G19 settings round trip |
| Student recovery, onboarding and TOTP | `portal-account-security.spec.ts` | B4 reset request; B5 setup completion; B7 onboarding completion; F5 factor lifecycle |
| Discounts, founding and receipts | `portal-billing-founding.spec.ts` | C4 scoped preview; C5/G10 application approval; C6 printable receipt |
| Full examined certification | `portal-certification-lifecycle.spec.ts` | D3–D6/E1 payment, booking, sitting, result, credential, verification and download |
| Documents and CPD | `portal-documents-cpd.spec.ts` | E4 assignment/acknowledgement/download; F2 student submission/admin approval |
| Three-certification isolation | `portal-multicert.spec.ts` | C2/E1 PCL-AI, PFL-AI and PML-AI isolation |
| Support and privacy | `portal-support-privacy.spec.ts` | F1/F3 ticket/notification; F4 erasure; F5 session revocation |
| Deterministic test personas | `portal-test-users.spec.ts` | C1/D1–D2 seven scenario stops and isolated blockers |
| Honorary, provider and marketing forms | `public-applications.spec.ts` | A5/E5/G9 full honorary lifecycle; A6/G13 provider application; A7/G15 inquiry/newsletter |
| Catalogue and enrolment | `public-catalogue.spec.ts` | A2 catalogue/detail consistency and enrolment hand-off |
| Languages and announcements | `public-i18n.spec.ts` | A3 all seven languages/RTL/persistence; A4 translated announcement |
| Public policy library | `public-policies.spec.ts` | A10 one attachment for every named crawlable policy |
| Institution partner | `partner-portal.spec.ts` | H1–H5 provisioning, password change, private document, code, sponsorship and isolation |

Runtime evidence is authoritative only when the corresponding CI test passes. Automatic failure screenshots, trace and video remain enabled independently of these success attachments.
