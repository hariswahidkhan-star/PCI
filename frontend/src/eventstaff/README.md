# Gate-staff scanner (`src/eventstaff/`) — spec §7A.8

A self-contained component tree for the event-admission gate scanner: event + gate selection,
camera QR scanning (feature-detected, manual entry is the primary path), the four oversized
result states (ADMIT / ALREADY CHECKED IN / MANUAL REVIEW / DO NOT ADMIT — text + icon + colour,
never colour alone), an explicit confirm step, rapid per-attendee reset that clears prior-attendee
PII, duplicate-scan debounce, an offline banner, and manual identity lookup (exact registration
reference or Student Number — a lookup, not authentication).

## Mount decision: deferred to integration

This tree is intentionally **not wired into any app shell**. It is neither a student-portal route
(gate staff are not students) nor part of the admin console (owned by another workstream), and
whether it gets its own Vite entry (like `vite.world.config.ts`) or mounts inside an operator
surface is an integration decision that depends on how staff authentication lands. Until then:

- Root component: `EventStaffApp.tsx` — render it anywhere; it needs no router and no global CSS
  (styles are self-contained in `styles.ts`).
- Staff auth placeholder: the seam uses `makeClient('pci.eventstaff.token')`, so the eventual host
  shell only has to put a staff bearer token under that sessionStorage key.

## The API seam

All data comes from the single seam module `src/api/eventAdmission.ts` (typed by
`src/api/types.ts`). The backend does not exist yet: the seam maps 404/501 onto
`EventAdmissionUnavailableError`, which these screens render as a designed
"Event passes are not yet enabled on this server" state. When the backend ships, the seam is the
only frontend file to wire.

**Security invariants** (do not regress):

- On the real seam path, every admission decision comes from the server. There is **no**
  client-side admit path; the mock decision function exists only behind fixtures mode.
- Camera frames go straight to the platform `BarcodeDetector` in memory. No frame is drawn,
  stored, or uploaded.
- "Next attendee" clears all prior-attendee PII from the screen.
- The scanner blocks pass checks while offline (decisions cannot be made without the server).

## Reviewing today (fixtures mode)

Append `?preview=1` to the URL (or set sessionStorage `pci.eventAdmission.fixtures` = `'1'`).
Sample events/gates load, and the mock decision service makes every state reachable:

| Input (scan code or lookup) | Result |
|---|---|
| anything containing `OK`, or `PCI-EV-2026-000731`, or `PCI-2024-018233` | ADMIT |
| anything containing `USED`, or `PCI-EV-2026-000214` | ALREADY CHECKED IN |
| anything containing `REVIEW` | MANUAL REVIEW |
| anything else | DO NOT ADMIT |


## Mount decision (made)

Mounted in the PCI admin app at `/event-scanner` (Operations group), gated on
`events_checkin`/`events_read` with `content` as the same migration fallback the backend
accepts — the spec places the scanner in the PCI AI operations realm (§7A.8). Staff
authentication is therefore the admin session; a dedicated device-enrollment flow remains
future backend work.
