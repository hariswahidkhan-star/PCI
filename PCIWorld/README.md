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
| Instance type | Starter (allows the persistent disk) or Free (data resets on each deploy) |
| Disk (Starter) | mount path `/data`, 1 GB |

Environment variables:

| Key | Value |
|---|---|
| `APP_BASE_URL` | the service URL, e.g. `https://pciworld.onrender.com` |
| `ALLOWED_ORIGIN` | same as `APP_BASE_URL` |
| `CREDENTIAL_ENCRYPTION_KEY` | click **Generate** |
| `PCIWORLD_OWNER_PASSWORD` | your admin password |
| `ALLOW_INSECURE_PRODUCTION` | `true` — preview-grade only, until MySQL 8 is attached (see `docs/pciworld/DEPLOY_RENDER.md`) |

No `PCIWORLD_ONLY` variable is needed — the image sets it. `DATABASE_FILE` defaults to
`/data/pciworld.db` inside the image, matching the disk mount above.

> If you prefer to set **Root Directory = `PCIWorld`**, you must also set the advanced
> **Docker Build Context Directory** to the repository root (`.`) — the Dockerfile copies
> `backend/` from the repo. Leaving Root Directory empty is the simpler, recommended path.

## After the deploy goes live

- `https://<service>.onrender.com/` → PCI World (root redirects to `/world`)
- `https://<service>.onrender.com/world-admin` → PCI World administration
  (`owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD` — change it after first sign-in)

Production hardening before public launch (MySQL 8, email provider, custom domains
`pciworld.org` / `admin.pciworld.org`): `docs/pciworld/DEPLOY_RENDER.md`.
