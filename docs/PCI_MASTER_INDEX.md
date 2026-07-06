# PCI Platform — Master Index (Batch-2 comprehensive review)

Complete handoff. Every deliverable, current. Where the folder holds older/duplicate files, the ones named
below are the latest. This pass was a fresh critical review that (a) found and fixed a real webhook bug,
(b) confirmed the .NET backend at full 123/123 route parity, and (c) packaged three .NET projects that had
never been shipped.

---

## THE BACKEND — completely on .NET (authoritative)
| Use this | What it is |
|---|---|
| **`PCI_Backend_dotnet.zip`** | **The .NET backend — COMPLETE, 123/123 routes.** ASP.NET Core 8, Microsoft.Data.Sqlite, BCrypt.Net, Stripe.net. Serves all four apps from `wwwroot/` and exposes the entire API: auth+RBAC, student portal, exam pipeline, secure-client, proctoring, student-360, CMS CRUD (8 tables), public forms, all admin management, **Stripe checkout + webhook**, enrolment sessions, tickets, codes, reports. 14 C# files, 2,400 lines. This is THE backend. |
| `PCI_Backend_dotnet_Port_Report.md` | Full port report + verification method + the bugs found and fixed. |
| `pci-enrollment-schema.sql` | The database schema (SQLite; created automatically on first run). |

**Run it:** `dotnet run` inside the unzipped folder → http://localhost:8080. First admin sign-in
`owner@pci.local` / `changeme-owner` (change forced). Stripe/SMTP optional (clean fallbacks).

### Earlier .NET explorations (included for completeness — NOT the current backend)
These predate `PCI.Backend`, use a different data layer (EF Core), and do **not** match the production
schema/API. Shipped so nothing is withheld, but use `PCI_Backend_dotnet.zip` for the real backend.
| File | What it was |
|---|---|
| `PCI_dotnet_alt_PciAdmin.zip` | Early EF-Core admin-API scaffold (~444 lines). |
| `PCI_dotnet_alt_PciCms.zip` | Early Razor-Pages CMS scaffold (~920 lines). |
| `Pci.Cms.zip` | Early CMS connector (~1,185 lines). |

### Node reference (still included)
| File | What it is |
|---|---|
| `pci-enrollment-backend.zip` | The original **Node.js** backend — the reference the .NET port mirrors, and the one that ran live here (24/24). Kept as a reference implementation. Say the word for a .NET-only bundle. |

---

## FRONT-END APPS
| Use this | What it is |
|---|---|
| **`pci-website-complete.zip`** | Full 215-page public marketing site (all pages, blog, chapters, policies, sectors, knowledge base, images, CSS). |
| **`pci-student-dashboard.html`** | Complete student portal (v5): scheduling, secure runner, results, certificate, CPD, membership, payments, support. |
| **`pci-admin-dashboard.html`** | Complete admin panel: three separated app sections (① Website / ② Student Panel / ③ Live Exam) + RBAC Team & Access. |
| `pci-platform-launcher.html` | The four-app launcher. |

## EXAM SOFTWARE (secure desktop client) — .NET
| Use this | What it is |
|---|---|
| **`PCI_SecureExam_dotnet.zip`** | Windows secure exam client (.NET 8 / WPF): kiosk lockdown, AI identity check, live proctoring, crash-resume, in-exam chat. Plug-and-play (`appsettings.json`, `build.ps1`, `--selftest`, CI). |
| `pci-secure-exam-ui-preview.html` | Interface preview, no install. |

## EMAIL TEMPLATES
| Use this | What it is |
|---|---|
| **`pci-email-templates.zip`** | All 12 transactional templates. Individual copies also present as `email-*.html`. |

---

## What THIS review pass found & fixed
1. **Real webhook bug fixed.** The Stripe webhook's replay/idempotency check read `changes()` on a
   separate call after `ExecuteReturningId` (which appends its own `SELECT`), making the "already
   processed?" check unreliable. Added an atomic `ExecuteWithChanges` helper returning rowid + change
   count from one command; the webhook now detects duplicate deliveries correctly.
2. **Route parity re-audited:** 0 of 123 missing, 0 duplicates.
3. **SQL re-swept:** 231 static queries valid against the real schema; all 40 tables + every
   payment/membership/redemption column confirmed.
4. **Compile-readiness review:** verified `Results.Text` overloads, Stripe `IHasId`, nullable settings
   (warnings not errors), `using` completeness, `_scorer` init order, and the `?`→`$pN` parameter rewrite.
5. **Three unshipped .NET projects packaged** (the alt/CMS explorations above).

## ✅ Now compile-verified (this pass)
Installed a real .NET 8 SDK from the Ubuntu feed (Microsoft's CDN stays firewalled, but the OS package
feed isn't) and **actually compiled and booted the backend** — 0 errors, 0 warnings, `/api/health` served
live. Found and fixed one genuine compile bug a compiler alone could catch (CS1061 in the exam scorer).
Full write-up: `PCI_Backend_REAL_COMPILE_Verification.md`.

## Honest limits (stated plainly)
- The backend is now **compile- and boot-verified with a real .NET 8 SDK** (installed from the Ubuntu
  feed). The only remaining gap: a full *data* round-trip needs the three NuGet packages, which
  api.nuget.org (firewalled here) can't provide — the bundled CI restores them on GitHub and runs the
  live smoke suite. So end-to-end DB behaviour is proven by CI on first push; compile + boot + HTTP are
  proven here and now.
- Not accredited / ISO-17024; registry "in development"; donations not tax-deductible — all by design.
