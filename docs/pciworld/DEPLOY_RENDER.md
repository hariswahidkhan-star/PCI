# Deploying PCI World on its own Render service

> Current dedicated image posture: external MySQL is required from first boot. Any interim
> disk-backed SQLite bridge described below applies only to an explicitly custom deployment using
> `PCIWORLD_ALLOW_SQLITE=true`; it is not the `PCIWorld/Dockerfile` default.

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
   | `DATABASE_FILE` | `/var/data/pciworld.db` — **must be an absolute path on the mounted disk** (step 4). A relative path lives in the container filesystem, which Render replaces on every deploy: every account, attempt and Passport would be destroyed. Boot refuses to start on a relative path when `PCIWORLD_ALLOW_SQLITE=true`. |
   | `CREDENTIAL_ENCRYPTION_KEY` | any strong 32-byte secret (Render → Generate) |
   | `PCIWORLD_OWNER_PASSWORD` | the initial world-admin owner password you want |
   | `PCIWORLD_ALLOW_SQLITE` | `true` — the explicit, temporary bridge: run on disk-backed SQLite until MySQL 8 is provisioned (see below) |

   Leave Stripe/SMTP unset: payments stay disabled (PCI World is free) and email falls back to
   the console sink until you configure Resend/SMTP (needed later for account verification
   mail — set `RESEND_API_KEY` when ready).

6. **Create Web Service** and wait for the first deploy (the Docker build takes a few minutes).

Then:

- **PCI World** → `https://<your-service>.onrender.com/` (redirects to `/world`)
- **Admin** → `https://<your-service>.onrender.com/world-admin`
  — sign in `owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD`, change it in the console.

## Durability comes first (PCI World's product law)

**Learner history is never lost.** Attempts, accounts and Passport evidence must survive every
deploy. Two things make that true, in this order:

1. **A mounted disk** (step 4) with `DATABASE_FILE` pointing onto it. Without this, the database
   is a file in the container filesystem and every redeploy wipes it. The app now says so loudly
   in the boot log — look for `[pciworld] EPHEMERAL STORAGE` and fix it before letting anyone
   register.
2. **MySQL 8** for production, which is what the programme requires. The disk-backed SQLite step
   is an honest bridge, not the destination.

`PCIWORLD_ALLOW_SQLITE=true` is what makes the bridge explicit: it downgrades the MySQL
requirement to a warning **and** hard-fails the boot if `DATABASE_FILE` is not an absolute path,
so the waiver can never be used to run on ephemeral storage by accident. It replaces the old
blanket `ALLOW_INSECURE_PRODUCTION=true`, which silently waived CORS, the encryption key and the
public base URL as well.

### MySQL 8 and MariaDB are both supported

The app is written once in the SQLite dialect and translated at runtime, and it now detects which
engine it is talking to, because the two disagree about DDL in ways that break a boot rather than a
page:

| | MariaDB | MySQL 8 |
|---|---|---|
| `CREATE INDEX IF NOT EXISTS` | supported | **syntax error** — the clause is stripped and a duplicate-index error is absorbed instead |
| index on a lone `TEXT` column | silently prefixed | **rejected** (1170) — retried with an explicit prefix |
| `TEXT` inside a composite key | rejected | rejected (1071) — those columns are bounded `VARCHAR` |

Verified on MariaDB 10.11: clean install, all 25 world tables, 50 challenges, 10 articles, the
rotation ledger, and every admin endpoint answering 200.

### Moving to MySQL 8 (the launch gate)

1. Provision MySQL 8 (Render has no managed MySQL — use PlanetScale/Aiven/RDS etc.).
2. Set `DB_PROVIDER=mysql` + `MYSQL_CONNECTION_STRING` (or `MYSQL_HOST`/`MYSQL_USER`/
   `MYSQL_PASSWORD`/`MYSQL_DATABASE`), remove `PCIWORLD_ALLOW_SQLITE`, redeploy — the boot
   validator then enforces the safe configuration for you.
3. Configure `RESEND_API_KEY` (or SMTP) so verification and reset email really sends.

## Security settings that are NOT waived

A `PCIWORLD_ONLY` deployment waives only the payment, exam and object-storage checks — the
subsystems it genuinely does not serve. These stay required in production and the service will
refuse to boot without them:

| Key | Why |
|---|---|
| `ALLOWED_ORIGIN` | otherwise CORS answers `*` and any site can call the world APIs from a visitor's browser |
| `CREDENTIAL_ENCRYPTION_KEY` | data-at-rest encryption falls back to a derivable key without it |
| `APP_BASE_URL` (or `PCIWORLD_BASE_URL`) | the origin used to build verification and password-reset links. These are **never** built from the request's `Host` header: a forged host would mail a real reset token pointing at an attacker. Render's own `RENDER_EXTERNAL_URL` is used automatically when neither is set. |

Two more worth setting deliberately:

- `PCIWORLD_OWNER_PASSWORD` — when unset, a production boot mints a random owner password and
  prints it **once** in the deploy log (`ONE-TIME PASSWORD:`). There is no published default in
  production. If the owner is ever left on the development default, boot warns on every start.
- `RESEND_API_KEY`/SMTP — until set, verification and reset mail prints to the log instead of
  sending.

## Rotation settings (world admin → Rotation)

The daily challenge is a recorded rotation period, not a computed guess. Operators control it in
the admin console rather than through environment variables:

| Setting | Default | Meaning |
|---|---|---|
| Timezone | `UTC` | whose midnight is the boundary — an IANA id (`Europe/London`) or a fixed offset (`+04:00`) |
| Shuffle | on | deterministic reshuffle each cycle; off plays the catalogue in order |
| Flag threshold | 3 | open content reports at which a challenge stops being featured |
| Pause | off | freezes the featured challenge without taking PCI World offline |

The boundary job runs inside the web service on the platform's worker-lease pattern, so running
multiple Render instances is safe — exactly one of them opens each day.

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
