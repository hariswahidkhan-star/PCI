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

Environment variables: **none are required.** The image is zero-config preview-grade — it boots
with no variables and no disk (the platform's production config blockers are downgraded to
logged warnings in PCI World-only mode, because the subsystems they protect — payments, exams,
credentials — are unreachable on this deployment). Optional variables:

| Key | Effect |
|---|---|
| `PCIWORLD_OWNER_PASSWORD` | sets the initial admin password (otherwise `changeme-world-owner` — change it in the console immediately) |
| `DATABASE_FILE` / `STORAGE_ROOT` | override the data paths (default `/data/...` when a disk is mounted, container-local otherwise) |

Without a disk, data resets on every deploy/restart — fine for a preview, add the `/data` disk
when you want persistence.

> If you prefer to set **Root Directory = `PCIWorld`**, you must also set the advanced
> **Docker Build Context Directory** to the repository root (`.`) — the Dockerfile copies
> `backend/` from the repo. Leaving Root Directory empty is the simpler, recommended path.

## After the deploy goes live

- `https://<service>.onrender.com/` → PCI World (root redirects to `/world`)
- `https://<service>.onrender.com/world-admin` → PCI World administration
  (`owner@pciworld.local` + your `PCIWORLD_OWNER_PASSWORD` — change it after first sign-in)

Production hardening before public launch (MySQL 8, email provider, custom domains
`pciworld.org` / `admin.pciworld.org`): `docs/pciworld/DEPLOY_RENDER.md`.
