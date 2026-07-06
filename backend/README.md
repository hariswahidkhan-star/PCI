# PCI Backend (.NET port)

ASP.NET Core Minimal API port of the PCI platform backend. Serves the website, student panel, admin
panel and exam-preview from `wwwroot/`, and exposes the same JSON API as the Node original — same
`schema.sql`, same routes, same contracts.

## Run
```bash
cp .env.example .env      # optional; every value has a working default
dotnet run
# → http://localhost:8080  (website, /student.html, /admin.html, /exam-ui.html)
```
First admin sign-in: `owner@pci.local` / `changeme-owner` (change forced on first login).

## Verify
CI (`.github/workflows/build.yml`) builds the project, boots it, and runs `smoke-test.sh` — 24 live
HTTP checks covering health, static apps, admin auth, RBAC gating, settings, and maintenance mode.

## Status
**Complete — 123 of 123 routes ported and wired.** Boot path, auth + RBAC, settings, content, student
login, full student portal, exam pipeline, secure-client endpoints, admin proctoring, student-360,
CMS CRUD (8 tables), public forms, all admin management, **Stripe checkout + webhook**, enrolment
sessions, tickets, codes v2/generate/redemptions, and reports. Verified: 232 SQL queries valid against
the real schema, all 40 tables + webhook columns confirmed, zero missing routes, zero duplicates,
all 14 C# files brace-balanced. CI compiles + boots + runs 46 live smoke checks.
