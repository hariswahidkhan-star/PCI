# PCI World — share-management console (spec §10.2)

A self-contained React component tree for administering the World share experience:
channel enable/disable + ordering, localized caption/title/hashtag templates with a bounded
placeholder whitelist, template versioning (draft → publish → roll back), share-link moderation
(revoke with a recorded reason), the provider-capability honesty panel, and an analytics
placeholder.

## Deliberately NOT mounted anywhere

This tree is **not wired into any app shell** — no route, no Vite entry, no bundle imports it.
The PCI World admin surface today is the **server-rendered shell** at `GET /world-admin`
(`backend/Endpoints/WorldAdmin.cs` — a separate realm with its own `pciworld_admin_users` /
`pciworld_admin_sessions`, bearer token in `localStorage['world_admin_token']`); there is no
React world-admin app yet. Whether this console becomes a new Vite entry behind that realm's
login, or the shell grows a mounting point for it, is an **integration decision deferred on
purpose** — deciding it here would silently create a fourth frontend app.

To integrate later: render `<ShareConsole />` (default export of `ShareConsole.tsx`) inside a
world-admin-authenticated page. It needs nothing else.

## Honesty about missing backends

- The admin share-settings API (`/api/world-admin/share/*`) **does not exist yet**. Everything
  reads through the one typed seam in `seam.ts`: live mode calls the *proposed* endpoints and
  turns the 404 into a designed "not yet enabled" state; fixtures mode (`?preview=1`, or
  `createShareAdminSeam({ fixtures: true })`) runs the whole console against `fixtures.ts`.
- The capability panel mirrors `SHARE_CAPABILITIES` from the participant `ShareSheet` — URL
  share is supported; direct posting and comment sync are shown as
  "requires provider approval — disabled". It imports the matrix rather than restating it.
- The analytics card is a labelled placeholder ("requires backend analytics — not yet enabled")
  because no share analytics are collected anywhere yet.

## Files

| File | What |
|---|---|
| `ShareConsole.tsx` | The console (channels / templates / moderation / capabilities / analytics) |
| `seam.ts` | The ONE typed seam: types, fixtures + live implementations, `ShareAdminNotEnabledError`, `isFixturesMode` |
| `template.ts` | Bounded placeholder whitelist (`{display_name}`, `{challenge_title}`) + text-only renderer |
| `fixtures.ts` | Sample data, including a hostile display name that proves escaping on sight |
| `worldadmin.css` | `wa-`-prefixed styles matching the `/world-admin` shell's visual language |
| `ShareConsole.test.tsx` | Vitest suite: escaping, revoke confirm, capability honesty, ordering, 404 state |


## Mount decision (made)

Shipped as the fourth independent bundle at `/world-admin-app/` (vite.worldadmin.config.ts →
dist-worldadmin), keeping World-admin code out of both the participant and PCI-admin bundles —
the bundle-level mirror of the server's realm separation. The mount gate renders a sign-in
pointer when `localStorage['world_admin_token']` is absent; real authorization stays server-side
on every API the console calls.
