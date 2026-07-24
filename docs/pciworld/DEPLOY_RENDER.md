# Deploying PCI World on its own Render service

Two supported shapes:

- **A — One platform deployment** (the existing `pci-platform` service): PCI World is already
  live at `<platform-url>/world` and its admin at `<platform-url>/world-admin`. Nothing to do.
- **B — A dedicated "pciworld" Render service** (its own URL, its own database — a standalone
  PCI World instance). This is the setup below. PCI World never touches Institute exam or
  credential records, so a fully separate instance is coherent by design.

## B — Dedicated service, step by step

In the Render dashboard, inside the project you created:

1. **New → Web Service** → connect this GitHub repo (`hariswahidkhan-star/PCI`).
2. **Runtime**: Docker (the repo's `Dockerfile` builds the whole app — backend + SPAs).
3. **Name**: `pciworld` → Render gives you `https://pciworld.onrender.com` or
   `https://pciworld-XXXX.onrender.com`.
4. **Disk** (needed for preview-grade SQLite + uploads): Add Disk → mount path `/var/data`,
   1 GB is plenty to start.
5. **Environment variables**:

   | Key | Value |
   |---|---|
   | `PCIWORLD_ONLY` | `true` — **the service serves PCI World exclusively**: every other page redirects to `/world`, every other API returns 404; the Institute site and portals are unreachable on this deployment |
   | `APP_BASE_URL` | the exact service URL, e.g. `https://pciworld.onrender.com` |
   | `ALLOWED_ORIGIN` | same value as `APP_BASE_URL` |
   | `DATABASE_FILE` | `/var/data/pciworld.db` |
   | `CREDENTIAL_ENCRYPTION_KEY` | any strong 32-byte secret (Render → Generate) |
   | `PCIWORLD_OWNER_PASSWORD` | the initial world-admin owner password you want |
   | `ALLOW_INSECURE_PRODUCTION` | `true` — **preview-grade only**: acknowledges SQLite instead of MySQL (see below) |

   Leave Stripe/SMTP unset: payments stay disabled (PCI World is free) and email falls back to
   the console sink until you configure Resend/SMTP (needed later for account verification
   mail — set `RESEND_API_KEY` when ready).

6. **Create Web Service** and wait for the first deploy (the Docker build takes a few minutes).

Then:

- **PCI World** → `https://<your-service>.onrender.com/` (redirects to `/world`)
- **Admin** → `https://<your-service>.onrender.com/world-admin`
  — sign in `owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD`, change it in the console.

## Production hardening (before public launch — not optional)

The master programme requires **MySQL 8 for production data**. The SQLite + 
`ALLOW_INSECURE_PRODUCTION=true` combination above is preview-grade. Before launch:

1. Provision MySQL 8 (Render has no managed MySQL — use PlanetScale/Aiven/RDS etc.).
2. Set `DB_PROVIDER=mysql` + `MYSQL_CONNECTION_STRING` (or `MYSQL_HOST`/`MYSQL_USER`/
   `MYSQL_PASSWORD`/`MYSQL_DATABASE`), remove `ALLOW_INSECURE_PRODUCTION`, redeploy — the boot
   validator then enforces the safe configuration for you.
3. Configure `RESEND_API_KEY` (or SMTP) so verification and reset email really sends.

## Custom domains (when you have them)

On the service → Settings → Custom Domains: add `pciworld.org` (and optionally
`admin.pciworld.org`), follow Render's DNS instructions, then set:

- `PCIWORLD_HOSTS=pciworld.org,www.pciworld.org` — these hosts land on `/world`
- `PCIWORLD_ADMIN_HOSTS=admin.pciworld.org` — this host lands on `/world-admin`

## Mode reference

| Variable | Effect |
|---|---|
| `PCIWORLD_ONLY=true` | Strict: the deployment serves ONLY PCI World (`/world*`, `/world-admin*`, their APIs, health). Everything else → redirect to `/world` (pages) or 404 (APIs). Implies the root redirect. Use this for a dedicated PCI World service. |
| `PCIWORLD_STANDALONE=true` | Soft: only `/` redirects to `/world`; the rest of the platform stays reachable on this host. |
| `PCIWORLD_HOSTS` / `PCIWORLD_ADMIN_HOSTS` | Host-based mapping of `/` for a combined deployment serving both sites (e.g. `pciworld.org` / `admin.pciworld.org`). |
