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
   and pre-configures the service, the health check, and a **5 GB persistent disk at `/data`**
   (the SQLite database + uploaded files live there and survive deploys).
   - Choose the **Starter** plan or above. **Not the free tier** — free instances have no disk,
     so the database would be wiped on every restart.
4. Fill in the environment variables it asks for:
   - `APP_BASE_URL` and `ALLOWED_ORIGIN`: your public URL, e.g. `https://pci-platform.onrender.com`
     (update both later if you attach a custom domain — no trailing slash).
   - `ADMIN_OWNER_EMAIL` / `ADMIN_OWNER_PASSWORD`: your real admin login for first boot.
   - Leave the Stripe/SMTP ones empty for now if you just want to see the site (payments answer
     503 and emails print to the logs until configured).
5. Click **Apply**. First build takes a few minutes. When it's live, open the URL — that's your
   website. Then `/admin.html`, log in, and it forces a password change.
6. Verify: log into `/admin.html` and open **System check** (owner-only) — everything should be
   green except Stripe/SMTP if you skipped them.

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
  -e APP_BASE_URL=https://www.yourdomain.org \
  -e ALLOWED_ORIGIN=https://www.yourdomain.org \
  -e ADMIN_OWNER_EMAIL=you@yourdomain.org \
  -e ADMIN_OWNER_PASSWORD='a-strong-password' \
  pci-platform
```

Put a TLS-terminating reverse proxy (Caddy/nginx/Traefik) in front, forwarding to `:8080` with
`X-Forwarded-Proto` set — see `backend/RUN.md` §8 for the nginx/HSTS notes. Back up `/srv/pci-data`
(it contains the SQLite DB and all uploaded evidence/attachments).

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

1. Change the owner password (forced) and open **System check** — aim for all green.
2. Admin → Settings: confirm pricing, pass mark, retention days.
3. Create your real team accounts (roles: website/student/exam manager, viewer) and stop using
   the owner account for daily work.
