// Fixture data for the §10.2 share console's fixtures mode (?preview=1) and its tests.
// Deliberately includes a HOSTILE display name so every preview and table proves, on sight,
// that free text is escaped — if the fixtures were all polite, escaping would be untested.

import type { ShareLinkRow, TemplateVersion } from './seam'

/** A display name that is an XSS attempt. It must always render as inert literal text. */
export const HOSTILE_SAMPLE_NAME = '"><img src=x onerror="alert(1)"><script>alert(2)</script>'

/** A polite co-star so lists show both shapes. */
export const SAMPLE_CHALLENGE_TITLE = 'Bridge Programme Rescue'

/** enabled-state per channel key; order comes from the participant ShareSheet's CHANNELS. */
export const FIXTURE_CHANNEL_STATE: Record<string, boolean> = {
  linkedin: true,
  x: true,
  facebook: false,
  whatsapp: true,
  telegram: true,
  email: true,
}

export const FIXTURE_TEMPLATES: TemplateVersion[] = [
  {
    id: 1, locale: 'en',
    caption: 'I completed {challenge_title} on PCI World. Practice evidence only — never a credential.',
    title: '{display_name} — PCI World Passport',
    hashtags: '#PCIWorld #ProjectControls',
    version: 1, state: 'superseded', created_at: '2026-06-02 09:15:00',
  },
  {
    id: 2, locale: 'en',
    caption: '{display_name} completed {challenge_title} on PCI World. Practice evidence only — never a credential.',
    title: '{display_name} — PCI World Passport',
    hashtags: '#PCIWorld #ProjectControls #PlanTheWork',
    version: 2, state: 'published', created_at: '2026-06-20 14:02:00',
  },
  {
    id: 3, locale: 'fr',
    caption: '{display_name} a terminé {challenge_title} sur PCI World. Preuve d’entraînement uniquement — jamais une certification.',
    title: '{display_name} — Passeport PCI World',
    hashtags: '#PCIWorld',
    version: 1, state: 'draft', created_at: '2026-07-11 10:40:00',
  },
]

export const FIXTURE_SHARE_LINKS: ShareLinkRow[] = [
  {
    id: 11, kind: 'passport', code: 'p/9f3ac2…', owner_display_name: 'Zoë Rivéra',
    created_at: '2026-07-01 08:30:00', revoked: false, revoke_reason: null,
  },
  {
    id: 12, kind: 'result', code: 'r/41bb08…', owner_display_name: HOSTILE_SAMPLE_NAME,
    created_at: '2026-07-14 19:12:00', revoked: false, revoke_reason: null,
  },
  {
    id: 13, kind: 'invitation', code: 'i/77cd15…', owner_display_name: 'Mateo Okonkwo',
    created_at: '2026-06-25 12:00:00', revoked: true, revoke_reason: 'Impersonation report upheld.',
  },
]
