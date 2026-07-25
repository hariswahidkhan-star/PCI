# PCIWorld — the dedicated PCI World deployment root

This directory builds a service that serves **PCI World exclusively** (`PCIWORLD_ONLY=true` is
baked into the image): the challenge platform at `/world`, its separate admin at `/world-admin`,
and nothing else — the Institute website and portals are unreachable on this deployment.

## Render — exact settings

Create the service with **New → Web Service** on this repository, then:

| Setting | Value |
|---|---|
| Language | **Docker** |
| Branch | `main` |
| **Root Directory** | **leave empty** — the build context must be the whole repository |
| **Dockerfile Path** | `PCIWorld/Dockerfile` |
| Instance type | Starter or above |
| Disk | mount path `/data`, sized for uploaded evidence |

**Environment variables: none are required to boot.** With no configuration the image runs the
validator's explicit SQLite bridge (`PCIWORLD_ALLOW_SQLITE` on an absolute `/data` path): mount
the `/data` disk and learner history is durable; without a disk the boot log prints the
`EPHEMERAL STORAGE` banner and data resets on every deploy — a posture for looking at the
product, never for inviting anyone in. **MySQL 8 is the production destination**: the moment
`MYSQL_HOST` (or `MYSQL_CONNECTION_STRING`) is set, the entrypoint switches to fail-closed
MySQL and the SQLite bridge is not engaged.

| Key | Value / effect |
|---|---|
| `MYSQL_HOST`, `MYSQL_DATABASE`, `MYSQL_USER`, `MYSQL_PASSWORD` | external MySQL 8 / MariaDB 10.11 connection — setting these is what flips the image to production MySQL |
| `MYSQL_SSL` | `required` for managed production |
| `APP_BASE_URL`, `ALLOWED_ORIGIN` | public HTTPS origin — recommended before public launch; on a world-only deployment their absence downgrades to logged warnings because the surfaces they guard are same-origin and server-rendered |
| `CREDENTIAL_ENCRYPTION_KEY` | dedicated 32-byte key/passphrase — recommended; the stores it encrypts (credentials, identity documents) are unreachable on this deployment, so its absence warns rather than blocks |
| `PCIWORLD_OWNER_PASSWORD` | sets the initial admin password (otherwise `changeme-world-owner`, and boot warns on every start until it is changed — in a production posture a random one is minted and printed once instead) |
| `STORAGE_ROOT` | uploaded-file path (default `/data/storage`) |
| `PCIWORLD_BASE_URL` | the public origin used in verification and reset emails. Render's `RENDER_EXTERNAL_URL` is used automatically, so this is only needed behind a custom domain. Links are never built from the request's `Host` header. |

Incomplete MySQL settings (host set, credentials missing) still terminate with exit 78 **before
any database is opened** — there is no silent fallback from a configured MySQL to SQLite.

> If you prefer to set **Root Directory = `PCIWorld`**, you must also set the advanced
> **Docker Build Context Directory** to the repository root (`.`) — the Dockerfile copies
> `backend/` from the repo. Leaving Root Directory empty is the simpler, recommended path.

## After the deploy goes live

- `https://<service>.onrender.com/` → PCI World (root redirects to `/world`)
- `https://<service>.onrender.com/world-admin` → PCI World administration
  (`owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD` — change it after first sign-in)

Production hardening before public launch (MySQL 8, email provider, custom domains
`pciworld.org` / `admin.pciworld.org`): `docs/pciworld/DEPLOY_RENDER.md`.
