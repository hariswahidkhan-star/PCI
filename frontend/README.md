# PCI Frontend — React apps (Stage 3)

React + TypeScript (Vite) for the **interactive** screens, running on the existing JSON API and
served by the ASP.NET backend. Two independent apps share this project (and its components/helpers):

- **Student portal** → served under **`/app/`** (`index.html` → `src/main.tsx`)
- **Admin console** → served under **`/admin/`** (`admin.html` → `src/admin/main.tsx`)

They build as **separate bundles** (own base path, own bearer token) so students never download admin
code and vice versa. This is the Stage 3 "convert the frontend to React" work: it covers the
*logged-in application* screens — where a component model genuinely helps — and leaves the ~210
SEO-critical marketing/info pages on the fast, server-rendered content system (Stage 2). The classic
`student.html` and `admin.html` panels also stay in place and reachable.

The admin console covers the full operator surface (≈29 sections): Dashboard, Students, Enrolments,
Payments, Credentials, Support tickets, Certifications, Exam registrations, Proctoring & sessions,
Discount codes, Pages & content, Site content, Enquiries, Form submissions, Reviews, Newsletter,
Reports (with CSV export), Email log, Audit log, Settings, Team & Access, and the content collections
(Question bank, Media library, FAQs, Resources, News, Body of Knowledge, Governance, Navigation menus).
The classic `admin.html` panel remains available and reachable; nothing is lost. Section visibility
follows the same role permissions the server enforces.

The content collections are driven by a single reusable `CrudSection` component (`crudConfigs.ts`) over
the backend's uniform `/api/admin/{name}` CRUD factory, so adding another collection is one config entry.

## What's here

| Route (`/app/…`) | Screen |
|---|---|
| `/login` | Email + password sign-in (`POST /api/login`, bearer session token) |
| `/` | Overview — candidate journey, key stats, next-step guidance |
| `/certifications` | Per-certification exam entitlements + exam scheduling (`POST /api/me/exam/book`) |
| `/credentials` | Issued credentials, validity, certificate/verify links |
| `/cpd` | CPD log with add/remove (`/api/me/cpd`) and progress to target |
| `/billing` | Payment history and receipts |
| `/messages` | Notifications, mark-as-read |
| `/profile` | Editable profile (`PATCH /api/me/profile`) |

Auth mirrors the classic portal exactly: a bearer session token from `/api/login`, kept in
`sessionStorage`, sent as `Authorization: Bearer …`. A 401 anywhere clears the token and returns to login.

## Develop

```bash
cd frontend
npm install
npm run dev        # Vite dev server on :5173, proxies /api → http://localhost:8080
```

Run the backend separately (`cd backend && dotnet run`) so `/api` calls resolve.

## Build & how it ships

```bash
npm run typecheck  # tsc --noEmit
npm run build      # → dist/  (base path /app/)
```

- **Docker** (production): the root `Dockerfile` builds this in a Node stage and copies `dist/`
  into the backend image at `wwwroot/app/`. The backend serves it under `/app/` with a client-side
  routing fallback (see `backend/Program.cs`).
- **Local against the real backend**: `npm run build` then copy `dist/*` into `backend/wwwroot/app/`
  (git-ignored) — the backend serves it exactly as production does.

## Notes

- `base: '/app/'` in `vite.config.ts` must match the backend mount path.
- `backend/wwwroot/app/` is a build artifact and is git-ignored — never edit it by hand.
- The marketing site logo/favicon are reused from `/assets/…` (the existing static assets).
