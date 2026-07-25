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

The image is production/fail-closed and requires an external managed MySQL service. `render.yaml`
does not provision the database. Required environment variables:

| Key | Value / effect |
|---|---|
| `MYSQL_HOST`, `MYSQL_DATABASE`, `MYSQL_USER`, `MYSQL_PASSWORD` | external MySQL 8 / MariaDB 10.11 connection |
| `MYSQL_SSL` | `required` for managed production |
| `APP_BASE_URL`, `ALLOWED_ORIGIN` | public HTTPS origin |
| `CREDENTIAL_ENCRYPTION_KEY` | dedicated 32-byte/long passphrase |
| `PCIWORLD_OWNER_PASSWORD` | sets the initial admin password (otherwise `changeme-world-owner`, and boot warns on every start until it is changed — in a production posture a random one is minted and printed once instead) |
| `STORAGE_ROOT` | uploaded-file path (default `/data/storage`) |
| `PCIWORLD_BASE_URL` | the public origin used in verification and reset emails. Render's `RENDER_EXTERNAL_URL` is used automatically, so this is only needed behind a custom domain. Links are never built from the request's `Host` header. |

Missing MySQL or security settings terminate with exit 78 **before any database is opened**.
SQLite is not the container's preview fallback. For a local preview, run the main image explicitly
in Development as documented in `DEPLOY.md`.

> If you prefer to set **Root Directory = `PCIWorld`**, you must also set the advanced
> **Docker Build Context Directory** to the repository root (`.`) — the Dockerfile copies
> `backend/` from the repo. Leaving Root Directory empty is the simpler, recommended path.

## After the deploy goes live

- `https://<service>.onrender.com/` → PCI World (root redirects to `/world`)
- `https://<service>.onrender.com/world-admin` → PCI World administration
  (`owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD` — change it after first sign-in)

Production hardening before public launch (MySQL 8, email provider, custom domains
`pciworld.org` / `admin.pciworld.org`): `docs/pciworld/DEPLOY_RENDER.md`.
