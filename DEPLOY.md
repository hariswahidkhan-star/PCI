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
   and pre-configures the service, the health check, the database (**SQLite at `/data/pci.db`**),
   and the **persistent disk at `/data`** that holds it along with uploaded files.
   - **No separate database to provision.** The blueprint deploys with nothing to sign up for:
     Render offers no managed MySQL, so requiring one meant every deploy failed its health check
     until a database had been bought elsewhere. The disk survives deploys and restarts, so the
     database survives with it — but it is deleted with the service, so keep backups (below).
   - Choose the **Starter** plan or above. **Not the free tier** — free instances have no disk,
     so the database *and* uploads on `/data` would be wiped on every restart.
   - MySQL remains the recommended database once traffic or a second instance justifies it; it is
     a dashboard change, not an edit to this repo. See “Moving to MySQL later” below.
4. Fill in the environment variables it asks for:
   - `APP_BASE_URL` and `ALLOWED_ORIGIN`: your public URL, e.g. `https://pci-platform.onrender.com`
     (update both later if you attach a custom domain — no trailing slash).
   - `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD`: your real admin login for first boot.
   - Leave the Stripe/SMTP ones empty for now if you just want to see the site (payments answer
     503 and emails print to the logs until configured).
5. Click **Apply**. First build takes a few minutes. When it's live, open the URL — that's your
   website. Then `/admin.html`, log in, and change the seeded password in Settings → Security.
6. Verify: log into `/admin.html` and open **System check** (owner-only) — everything should be
   green except Stripe/SMTP if you skipped them.

### Deploys failing at the health check with exit 78?

The platform **fails closed in production**: a Production boot refuses to open a database unless
one of the three supported postures below applies. The refusal happens *before* the database is
opened or seeded (the container exits with code 78 before serving traffic), while the last
successful deploy stays live.

The three supported production postures:

1. **Recommended — managed MySQL**: provision MySQL 8 / MariaDB (PlanetScale, Aiven, RDS,
   DigitalOcean…), set `DB_PROVIDER=mysql` + the `MYSQL_*` variables, and run the one-time data
   migration `backend/tools/migrate_sqlite_to_mysql.py` (see `docs/MYSQL_MIGRATION.md`).
2. **Interim — SQLite on the persistent disk**: a SQLite database whose file lives under a
   *writable mounted* `/data` keeps deploying **automatically, with no flag** — the boot log
   prints a `[config:warn] production is running SQLite at … (supported interim posture)` warning
   on every start so the posture stays visible. A legacy hand-created service with only the disk
   and Render's own `RENDER_EXTERNAL_URL` boots with zero further config: the base URL is adopted
   from `RENDER_EXTERNAL_URL`, CORS defaults to same-origin, and remaining gaps print as
   `[config:warn]`. If auto-detection can't see your disk, `ALLOW_SQLITE_IN_PRODUCTION=true`
   claims the posture explicitly — it still requires `DATABASE_FILE` under `/data`, and keeps
   every other production check (HTTPS base URL, explicit CORS origin,
   `CREDENTIAL_ENCRYPTION_KEY`, …) fully active. Do not confuse it with
   `ALLOW_INSECURE_PRODUCTION=true`, which disables **all** checks and is not recommended.
3. **PCI World-only** (`PCIWORLD_ONLY=true`): a deployment serving only the PCI World surfaces.
   SQLite is allowed only with the explicit bridge `PCIWORLD_ALLOW_SQLITE=true` **and**
   `DATABASE_FILE` under `/data`; the payment/exam/credential blockers this host doesn't serve
   are downgraded to warnings. See `docs/pciworld/DEPLOY_RENDER.md` for the full hardening list
   before a public launch.

### Already running WITHOUT the blueprint (service created by hand)?
If the service was created manually — especially on the **free tier** — the database and every
uploaded file are erased on each deploy or restart. Fixing it is two dashboard steps, no
environment variables needed:

1. Service → **Settings → Instance Type** → choose **Starter** (disks need a paid instance).
2. Service → **Disks → Add Disk** → name it anything, **Mount Path `/data`**, size 5 GB → Save.

The service restarts and the app **detects the disk automatically** — the database at
`/data/pci.db` and uploads under `/data/storage` (the boot log prints
`persistent disk detected at /data`). If the service is set to `DB_PROVIDER=mysql`, the app does
**not** invent a SQLite file on `/data`; it holds out for the MySQL settings instead. Data created
before the disk existed was on the ephemeral filesystem and cannot be recovered — do this before
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

### Exit codes — what a failed deploy is telling you

| Exit | Meaning | Typical causes | Fix |
|---|---|---|---|
| **78** | Configuration refused (`EX_CONFIG`) | Missing/invalid `APP_BASE_URL` or `ALLOWED_ORIGIN`, no `CREDENTIAL_ENCRYPTION_KEY`, SQLite off the persistent disk, MySQL selected but `MYSQL_HOST`/`MYSQL_PASSWORD` unset, `ENABLE_LEGACY_ADMIN_TOKEN=true`, `STORAGE_PROVIDER=s3` without `S3_BUCKET`, `STRIPE_SECRET_KEY` without `STRIPE_WEBHOOK_SECRET` | Set the variable the log names; redeploy |
| **75** | Temporarily unavailable (`EX_TEMPFAIL`) | The configured database cannot be opened — wrong host/port, firewall, TLS, bad credentials, database not created yet, or MySQL still starting up. Also: an older binary meeting a newer schema (deploy a matching build) | Check `MYSQL_HOST`/`MYSQL_PORT` reachability, `MYSQL_USER`/`MYSQL_PASSWORD`, that `MYSQL_DATABASE` exists, and `MYSQL_SSL` (managed providers usually need `required`). Raise `MYSQL_CONNECT_RETRIES` if the DB is still provisioning |
| **70** | Software/migration failure (`EX_SOFTWARE`) | A schema migration failed part-way | Read the `[migrate]` log lines; the service never serves on a half-migrated database |

The app never reports healthy when the database or a migration failed — the health check only
answers after a successful open + migration.

### Blockers checked before the database opens (exit 78 unless a posture downgrades them)

`APP_BASE_URL` must be an **absolute, non-loopback `https://` URL** (malformed and `http://`
values are refused — the boot preflight and the admin System-check use the same rule);
`ALLOWED_ORIGIN` must be explicit (no wildcard; keep it exactly equal to `APP_BASE_URL`, no
trailing slash); `CREDENTIAL_ENCRYPTION_KEY` must be set; the database must satisfy one of the
three postures above; `ENABLE_LEGACY_ADMIN_TOKEN` must be off; `STORAGE_PROVIDER=s3` requires
`S3_BUCKET` (no silent local-disk fallback); `STRIPE_WEBHOOK_SECRET` is required once
`STRIPE_SECRET_KEY` is set. On Render, an unset `APP_BASE_URL` is adopted from
`RENDER_EXTERNAL_URL`, and an unset `ALLOWED_ORIGIN` defaults to the same origin. Fix the
variable the log names and redeploy; do not use `ALLOW_INSECURE_PRODUCTION` outside an emergency.

### Log prefixes to search for in a failed deploy

| Prefix | Emitted when |
|---|---|
| `[config]` | A hard refusal (`Refusing to open database:` / `Refusing to start:`) — exit 78 |
| `[config:warn]` / `[config:error]` / `[config:info]` | The full configuration report (System-check shows the same items) |
| `[db] refusing to start` | The database could not be opened — exit 75 |
| `[migrate] refusing to start` | Schema compatibility (exit 75) or migration failure (exit 70) |
| `[boot]` | Normal startup decisions (provider, adopted URLs, detected `/data` disk) |

### Encryption key — preserve it

`CREDENTIAL_ENCRYPTION_KEY` encrypts stored identity documents and displayable credentials at
rest. `render.yaml` generates it once at service creation (`generateValue: true`) and never
overwrites an existing value — an existing service keeps the key its data was encrypted under.
**Losing or replacing the key orphans everything encrypted with it**; rotating to a
vault-managed key means setting the new value explicitly *and* re-encrypting existing artefacts.
On the legacy persistent-disk posture with no explicit key, a derived key is used and the gap is
warned about — set a dedicated key when you can, but note the derived key keeps decrypting the
artefacts it originally encrypted (a generated replacement would not).

### Render first provision

The Blueprint (`render.yaml`) sets the database (`DB_PROVIDER=sqlite`,
`DATABASE_FILE=/data/pci.db`, `ALLOW_SQLITE_IN_PRODUCTION=true`), concrete
`APP_BASE_URL`/`ALLOWED_ORIGIN` values derived from the service name, and generates
`CREDENTIAL_ENCRYPTION_KEY`. **Nothing is required before the first deploy** — a new service boots
and serves on those alone. The remaining `sync: false` variables (`ADMIN_OWNER_*`, Stripe, email)
are blank until you fill them in on Settings → Environment; without them the site still runs, with
the seeded owner login, payments answering 503 and emails printing to the logs.

`DB_PROVIDER` is declared with an explicit value rather than omitted on purpose: a blueprint only
overwrites the keys it actually names, so a service that already carries `DB_PROVIDER=mysql` from
an earlier configuration would otherwise keep it and keep failing.

### Moving to MySQL later

MySQL stays the recommended database once traffic or a second instance justifies it — SQLite is a
single-writer file on one disk, which one instance handles well and two cannot share. The switch is
a dashboard change with no edit to this repo:

1. Provision MySQL 8 / MariaDB 10.11+ (PlanetScale, Aiven, RDS, DigitalOcean…).
2. Run the one-time data migration: `backend/tools/migrate_sqlite_to_mysql.py`
   (see `docs/MYSQL_MIGRATION.md`).
3. Settings → Environment: set `DB_PROVIDER=mysql`, `MYSQL_HOST`, `MYSQL_USER`, `MYSQL_PASSWORD`,
   `MYSQL_DATABASE`, `MYSQL_SSL=required`. `DB_PROVIDER=mysql` takes precedence over the SQLite
   settings, which can be left in place.
4. Keep the `/data` disk — uploaded evidence and attachments still live there.

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
