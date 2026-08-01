# PCIWorld — the dedicated PCI World deployment root

This directory builds a service that serves **PCI World exclusively** (`PCIWORLD_ONLY=true` is
baked into the image): the challenge platform at `/world`, the React participant app at
`/world-app`, World admin at `/world-admin` + `/world-admin-app`, and nothing else — the
Institute website and portals are unreachable on this deployment.

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

**Environment variables: none are required to boot.** With no configuration the image forces
SQLite on `/data/pciworld.db` (`PCIWORLD_ALLOW_SQLITE=true`). Mount the `/data` disk so learner
history is durable; without a disk the boot log prints the `EPHEMERAL STORAGE` banner and data
resets on every deploy — fine for looking at the product, never for inviting anyone in.

**You do not need an external MySQL database** to compile, merge, or deploy. If the dashboard
still has `DB_PROVIDER=mysql` or blank `MYSQL_*` keys from an earlier attempt, the entrypoint
ignores them and boots SQLite. Set real MySQL credentials only when you are ready to cut over.

| Key | Value / effect |
|---|---|
| `MYSQL_HOST`, `MYSQL_DATABASE`, `MYSQL_USER`, `MYSQL_PASSWORD` | external MySQL 8 / MariaDB 10.11 — **both host and password required** before the image switches to MySQL |
| `MYSQL_SSL` | `required` for managed production |
| `APP_BASE_URL`, `ALLOWED_ORIGIN` | public HTTPS origin — recommended before public launch; on a world-only deployment their absence downgrades to logged warnings |
| `CREDENTIAL_ENCRYPTION_KEY` | dedicated 32-byte key — recommended; absence warns rather than blocks on world-only |
| `PCIWORLD_OWNER_PASSWORD` | sets the initial admin password (otherwise `changeme-world-owner`) |
| `STORAGE_ROOT` | uploaded-file path (default `/data/storage`) |
| `PCIWORLD_BASE_URL` | public origin for verification/reset emails; Render's `RENDER_EXTERNAL_URL` is used automatically |

> If you prefer to set **Root Directory = `PCIWorld`**, you must also set the advanced
> **Docker Build Context Directory** to the repository root (`.`) — the Dockerfile copies
> `backend/` and `frontend/` from the repo. Leaving Root Directory empty is simpler.

## After the deploy goes live

- `https://<service>.onrender.com/` → redirects to `/world`
- `https://<service>.onrender.com/world-app/` → React participant app
- `https://<service>.onrender.com/world-admin` → PCI World administration
  (`owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD` — change it after first sign-in)

Production hardening before public launch (MySQL 8, email provider, custom domains
`pciworld.org` / `admin.pciworld.org`): `docs/pciworld/DEPLOY_RENDER.md`.

## Local verify

```bash
# from the repository root
docker build -f PCIWorld/Dockerfile -t pciworld .
docker run --rm -p 8080:8080 -v pciworld-data:/data \
  -e RENDER_EXTERNAL_URL=https://localhost \
  pciworld
curl -fsS http://localhost:8080/api/health
curl -s -o /dev/null -w '%{http_code}\n' http://localhost:8080/world-app/
```
