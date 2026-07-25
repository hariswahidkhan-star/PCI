# Deploying the PCI Platform (making your website live)

One deployment gives you all three surfaces on the same URL:

| Surface | URL |
|---|---|
| Public website (216 pages) | `https://your-domain/` |
| Student panel | `https://your-domain/student.html` |
| Admin dashboard | `https://your-domain/admin.html` |

They share one backend and one database — content you edit in the admin dashboard appears on the
website; everything students do appears in the dashboard. There is nothing separate to connect.

---

## Option A — Render (recommended, ~15 minutes, no server admin)

1. **Merge PR #1** so `main` has the deployable code (or deploy from the branch).
2. Create an account at https://render.com and connect your GitHub.
3. Dashboard → **New → Blueprint** → pick the `PCI` repository. Render reads `render.yaml`
   and pre-configures the service, the health check, **external managed MySQL settings** (`DB_PROVIDER=mysql`), and a
   **persistent disk at `/data`** for uploaded files (not the primary database).
   - Provision MySQL separately; `render.yaml` does not create the database. Production **requires MySQL** — the app refuses to boot on SQLite in Production (see
     `backend/MYSQL.md` and `docs/MYSQL_MIGRATION.md`). SQLite is local/dev and CI smoke only.
   - Choose the **Starter** plan or above. **Not the free tier** — free instances have no disk,
     so uploads on `/data` would be wiped on every restart.
4. Fill in the environment variables it asks for:
   - `APP_BASE_URL` and `ALLOWED_ORIGIN`: your public URL, e.g. `https://pci-platform.onrender.com`
     (update both later if you attach a custom domain — no trailing slash).
   - `MYSQL_*` / `DB_PROVIDER=mysql` (from the blueprint) — required for Production.
   - `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD`: your real admin login for first boot.
   - Leave the Stripe/SMTP ones empty for now if you just want to see the site (payments answer
     503 and emails print to the logs until configured).
5. Click **Apply**. First build takes a few minutes. When it's live, open the URL — that's your
   website. Then `/admin.html`, log in, and change the seeded password in Settings → Security.
6. Verify: log into `/admin.html` and open **System check** (owner-only) — everything should be
   green except Stripe/SMTP if you skipped them.

### Deploys suddenly failing with “production requires DB_PROVIDER=mysql”?

The platform now **fails closed on MySQL in production**: a Production boot refuses to open a
database unless `DB_PROVIDER=mysql` (with the `MYSQL_*` settings) is configured. An existing
service that was deployed on the earlier SQLite-on-persistent-disk posture will therefore fail
every new deploy at the health check (the container exits with code 78 before serving traffic),
while the last successful deploy stays live.

Two ways forward:

1. **Recommended — move to managed MySQL**: provision MySQL 8 / MariaDB (PlanetScale, Aiven, RDS,
   DigitalOcean…), set `DB_PROVIDER=mysql` + the `MYSQL_*` variables, and run the one-time data
   migration `backend/tools/migrate_sqlite_to_mysql.py` (see `docs/MYSQL_MIGRATION.md`).
2. **Interim — keep SQLite on the persistent disk, explicitly**: set **one** environment variable
   on the service (Settings → Environment): `ALLOW_SQLITE_IN_PRODUCTION=true`. This waives ONLY
   the MySQL requirement, still requires the database to live under the mounted `/data` disk, and
   keeps every other production check (HTTPS base URL, explicit CORS origin,
   `CREDENTIAL_ENCRYPTION_KEY`, …) fully active. The boot log prints a warning on every start so
   the posture stays visible. Do not confuse it with `ALLOW_INSECURE_PRODUCTION=true`, which
   disables **all** checks and is not recommended.

### Already running WITHOUT the blueprint (service created by hand)?
If the service was created manually — especially on the **free tier** — the database and every
uploaded file are erased on each deploy or restart. Fixing it is two dashboard steps, no
environment variables needed:

1. Service → **Settings → Instance Type** → choose **Starter** (disks need a paid instance).
2. Service → **Disks → Add Disk** → name it anything, **Mount Path `/data`**, size 5 GB → Save.

The service restarts and the app **detects the disk automatically** for uploads under
`/data/storage` (the boot log prints `persistent disk detected at /data`). With
`DB_PROVIDER=mysql`, the app does **not** invent a SQLite file on `/data`. Data created before
the disk existed was on the ephemeral filesystem and cannot be recovered — do this before
inviting real users.

### Enabling email (two options)
- **Easiest — Resend:** create a free account at https://resend.com, add an API key, and set one
  environment variable: `RESEND_API_KEY`. To send from your own address, verify your domain in
  Resend and set `MAIL_FROM` (e.g. `PCI Global <no-reply@yourdomain.org>`); until then a built-in
  test sender is used, which only delivers to the Resend account owner's inbox.
- **Classic SMTP:** set `SMTP_HOST`, `SMTP_PORT` (587), `SMTP_USER`, `SMTP_PASS`, `SMTP_FROM`.

Without either, emails print to the service logs (visible in Admin → Email log as `console`).

### Custom domain
Render service → Settings → Custom Domains → add `www.yourdomain.org` and follow the DNS
instructions. Then update `APP_BASE_URL` and `ALLOWED_ORIGIN` to the new URL and redeploy.
TLS certificates are automatic. (Render also terminates TLS and sends `X-Forwarded-Proto`,
so the app emits HSTS correctly — nothing to configure.)

### Enabling payments (when ready)
1. Stripe dashboard (test mode first) → copy the **secret key** (`sk_test_…`) into `STRIPE_SECRET_KEY`.
2. Stripe → Developers → Webhooks → **Add endpoint**: `https://your-domain/api/webhook`,
   event `checkout.session.completed` (plus `payment_intent.payment_failed`).
3. Copy the endpoint's **signing secret** (`whsec_…`) into `STRIPE_WEBHOOK_SECRET`. Redeploy.
4. Run one full test checkout end-to-end before switching to live keys.

---

## Option B — any server with Docker (VPS, on-prem)

```bash
docker build -t pci-platform .
docker run -d --name pci --restart unless-stopped \
  -p 8080:8080 \
  -v /srv/pci-data:/data \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DB_PROVIDER=mysql \
  -e MYSQL_HOST=db.internal -e MYSQL_DATABASE=pci \
  -e MYSQL_USER=pci -e MYSQL_PASSWORD='a-strong-db-password' \
  -e MYSQL_SSL=required \
  -e APP_BASE_URL=https://www.yourdomain.org \
  -e ALLOWED_ORIGIN=https://www.yourdomain.org \
  -e CREDENTIAL_ENCRYPTION_KEY='a-separate-32-byte-or-longer-secret' \
  -e ADMIN_OWNER_EMAIL=you@yourdomain.org \
  -e ADMIN_OWNER_PASSWORD='a-strong-password' \
  pci-platform
```

Put a TLS-terminating reverse proxy (Caddy/nginx/Traefik) in front, forwarding to `:8080` with
`X-Forwarded-Proto` set — see `backend/RUN.md` §8. Back up MySQL with
`backend/tools/mysql_backup.sh` and back up `/srv/pci-data` (uploaded evidence/attachments) in the
same restore set.

**Quick local look without any production config:**
```bash
docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development pci-platform
# then open http://localhost:8080/
```

---

## Boot-time guard rails (expected behaviour, not errors)

In `Production` the app **refuses to start** (exit 78, each problem named in the logs) until the
config is safe: `APP_BASE_URL` must be a public https URL, `ALLOWED_ORIGIN` must be explicit (no
wildcard), the database path must not be temporary, and `STRIPE_WEBHOOK_SECRET` is required once
`STRIPE_SECRET_KEY` is set. Fix the variable it names and redeploy; do not use
`ALLOW_INSECURE_PRODUCTION` outside of an emergency.

## After first login — 3-minute checklist

1. Change the owner password (Settings → Security) and open **System check** — aim for all green.
2. Admin → Settings: confirm pricing, pass mark, retention days.
3. Create your real team accounts (roles: website/student/exam manager, viewer) and stop using
   the owner account for daily work.

## Testing the exam software end-to-end

The exam runs two ways off the same backend: **in the browser** (the exam-day check-in page
`/student.html`, via `/api/me/exam/start` → heartbeat → submit) and via the **Windows desktop
client** (SecureExam: `pciexam://` launch → `/api/exam/authorize` → heartbeat → submit). Both draw
their questions from the *live* exam bank (`published`, not marked *practice*). A fresh install ships
with a handful of live questions, so the exam is sittable immediately — but it is thin.

- **To try a realistic sitting on a test deployment**, set the environment variable
  `SEED_DEMO_EXAM=true` before boot. This loads ~24 extra generic project-controls questions into the
  live bank (bringing it to ~30 across ~15 domains) so you can complete a full, meaningful exam and see
  a real domain breakdown. To also create a login-able test candidate, set `DEMO_STUDENT_PASSWORD` (the
  account is `student@pci.local`). Both flags are **opt-in and never fire in `Production` by default**.
- **For a real certification launch**, leave `SEED_DEMO_EXAM` unset and author the confidential live
  bank privately in **Admin Console → Questions** (leave *Practice question* unchecked, tick
  *Published*). Real exam answers must never live in source control — the demo pack is deliberately
  generic, publicly-known fundamentals, not the certification's live items.
- **The desktop client is Windows-only** (WPF); it cannot run on Render/Linux. Render hosts the
  backend + browser exam. To exercise the desktop client, build `secureexam/PCI.SecureExam.App` on
  Windows and point its `ApiBaseUrl` (or the `api=` launch parameter) at your deployed URL.
