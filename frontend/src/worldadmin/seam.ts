// §10.2 share administration — the ONE typed seam every console screen reads through.
//
// HONESTY FIRST: the admin share-settings backend DOES NOT EXIST YET. There are no
// /api/world-admin/share/* endpoints in backend/Endpoints/WorldAdmin.cs today. This module is
// the single place that fact lives:
//
//   · 'live' mode calls the PROPOSED endpoints (documented on each method) with the world-admin
//     bearer (localStorage 'world_admin_token' — the same key the /world-admin shell uses) and
//     translates the inevitable 404 into ShareAdminNotEnabledError, which the console renders
//     as a designed "not yet enabled" state rather than a broken screen.
//   · 'fixtures' mode (?preview=1) serves an in-memory copy of ./fixtures so the console can be
//     exercised, tested and reviewed end-to-end before the backend lands.
//
// When the backend ships, only this file changes; every screen already speaks these types.

import { CHANNELS } from '../world/ShareSheet'

// ── types ───────────────────────────────────────────────────────────────────────────────────

export interface ShareChannel {
  /** Stable key matching the participant ShareSheet's channel keys ('linkedin', 'x', …). */
  key: string
  label: string
  enabled: boolean
}

export type TemplateState = 'draft' | 'published' | 'superseded'

export interface TemplateVersion {
  id: number
  locale: string
  /** Caption template — free text plus whitelisted {placeholders} (see ./template.ts). */
  caption: string
  title: string
  hashtags: string
  version: number
  state: TemplateState
  created_at: string
}

export type ShareLinkKind = 'passport' | 'result' | 'invitation'

export interface ShareLinkRow {
  id: number
  kind: ShareLinkKind
  /** The public code/slug — the server stores raw tokens only as hashes, so this is all there is. */
  code: string
  owner_display_name: string
  created_at: string
  revoked: boolean
  revoke_reason: string | null
}

export interface ShareAdminSeam {
  readonly mode: 'fixtures' | 'live'
  channels(): Promise<ShareChannel[]>
  setChannelEnabled(key: string, enabled: boolean): Promise<ShareChannel[]>
  /** Reorder by one step. Up/down buttons, deliberately not drag — keyboard/AT accessible. */
  moveChannel(key: string, direction: 'up' | 'down'): Promise<ShareChannel[]>
  templates(locale: string): Promise<TemplateVersion[]>
  saveDraft(locale: string, fields: { caption: string; title: string; hashtags: string }): Promise<TemplateVersion[]>
  publishTemplate(id: number): Promise<TemplateVersion[]>
  /** Roll back = republish a superseded version (the prior published one becomes superseded). */
  rollbackTemplate(id: number): Promise<TemplateVersion[]>
  shareLinks(): Promise<ShareLinkRow[]>
  /** Revocation always carries an operator reason — it is an audit event, never a silent delete. */
  revokeShareLink(id: number, reason: string): Promise<ShareLinkRow[]>
}

/** Thrown when the backend answers 404 for the share-admin surface: it has not shipped. */
export class ShareAdminNotEnabledError extends Error {
  constructor() {
    super('The share-administration backend is not enabled on this deployment.')
    this.name = 'ShareAdminNotEnabledError'
  }
}

const FIXTURES_KEY = 'pci.worldadmin.fixtures'

/** Sticky fixtures switch for the "explore with sample data" action on the not-enabled card. */
export function enableFixtures(): void {
  try { sessionStorage.setItem(FIXTURES_KEY, '1') } catch { /* ?preview=1 remains the fallback */ }
}

/** ?preview=1 — same convention the task of previewing unbuilt surfaces uses elsewhere. */
export function isFixturesMode(search: string): boolean {
  if (new URLSearchParams(search).get('preview') === '1') return true
  try { return sessionStorage.getItem(FIXTURES_KEY) === '1' } catch { return false }
}

// ── fixtures mode — in-memory, mutable copy of ./fixtures ───────────────────────────────────

import {
  FIXTURE_CHANNEL_STATE, FIXTURE_SHARE_LINKS, FIXTURE_TEMPLATES,
} from './fixtures'

function clone<T>(v: T): T { return JSON.parse(JSON.stringify(v)) as T }

function fixturesSeam(): ShareAdminSeam {
  // Channel identity comes from the participant ShareSheet's own list so the two surfaces can
  // never disagree about what channels exist; only enabled/order state is fixture data.
  let channels: ShareChannel[] = CHANNELS.map(c => ({
    key: c.key, label: c.label, enabled: FIXTURE_CHANNEL_STATE[c.key] ?? true,
  }))
  let templates: TemplateVersion[] = clone(FIXTURE_TEMPLATES)
  let links: ShareLinkRow[] = clone(FIXTURE_SHARE_LINKS)
  let nextId = 1000

  return {
    mode: 'fixtures',
    channels: async () => clone(channels),
    setChannelEnabled: async (key, enabled) => {
      channels = channels.map(c => (c.key === key ? { ...c, enabled } : c))
      return clone(channels)
    },
    moveChannel: async (key, direction) => {
      const i = channels.findIndex(c => c.key === key)
      const j = direction === 'up' ? i - 1 : i + 1
      if (i >= 0 && j >= 0 && j < channels.length) {
        const next = [...channels]
        ;[next[i], next[j]] = [next[j], next[i]]
        channels = next
      }
      return clone(channels)
    },
    templates: async (locale) => clone(templates.filter(t => t.locale === locale)),
    saveDraft: async (locale, fields) => {
      const maxVersion = Math.max(0, ...templates.filter(t => t.locale === locale).map(t => t.version))
      templates = [
        ...templates.filter(t => !(t.locale === locale && t.state === 'draft')),
        {
          id: nextId++, locale, ...fields, version: maxVersion + 1,
          state: 'draft', created_at: new Date().toISOString().slice(0, 19).replace('T', ' '),
        },
      ]
      return clone(templates.filter(t => t.locale === locale))
    },
    publishTemplate: async (id) => {
      const target = templates.find(t => t.id === id)
      if (target) {
        templates = templates.map(t =>
          t.id === id ? { ...t, state: 'published' as const }
          : t.locale === target.locale && t.state === 'published' ? { ...t, state: 'superseded' as const }
          : t)
      }
      return clone(templates.filter(t => t.locale === target?.locale))
    },
    rollbackTemplate: async (id) => {
      const target = templates.find(t => t.id === id)
      if (target && target.state === 'superseded') {
        templates = templates.map(t =>
          t.id === id ? { ...t, state: 'published' as const }
          : t.locale === target.locale && t.state === 'published' ? { ...t, state: 'superseded' as const }
          : t)
      }
      return clone(templates.filter(t => t.locale === target?.locale))
    },
    shareLinks: async () => clone(links),
    revokeShareLink: async (id, reason) => {
      links = links.map(l => (l.id === id ? { ...l, revoked: true, revoke_reason: reason } : l))
      return clone(links)
    },
  }
}

// ── live mode — PROPOSED endpoints; 404 → ShareAdminNotEnabledError ─────────────────────────

async function call<T>(path: string, body?: unknown, method?: string): Promise<T> {
  const headers: Record<string, string> = {
    // Same carrier and storage key as the server-rendered /world-admin shell.
    Authorization: 'Bearer ' + (localStorage.getItem('world_admin_token') ?? ''),
  }
  if (body !== undefined) headers['Content-Type'] = 'application/json'
  const res = await fetch(path, {
    method: method ?? (body !== undefined ? 'POST' : 'GET'),
    headers,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  })
  if (res.status === 404) throw new ShareAdminNotEnabledError()
  const text = await res.text()
  let parsed: unknown = null
  try { parsed = text ? JSON.parse(text) : null } catch { /* non-JSON body */ }
  if (!res.ok) {
    const o = (parsed ?? {}) as { error?: string; message?: string }
    throw new Error(o.message ?? o.error ?? `HTTP ${res.status}`)
  }
  return parsed as T
}

function liveSeam(): ShareAdminSeam {
  const base = '/api/world-admin/share'
  return {
    mode: 'live',
    channels: () => call(`${base}/channels`),
    setChannelEnabled: (key, enabled) => call(`${base}/channels/${key}`, { enabled }, 'PATCH'),
    moveChannel: (key, direction) => call(`${base}/channels/${key}/move`, { direction }),
    templates: (locale) => call(`${base}/templates?locale=${encodeURIComponent(locale)}`),
    saveDraft: (locale, fields) => call(`${base}/templates`, { locale, ...fields }),
    publishTemplate: (id) => call(`${base}/templates/${id}/publish`, {}),
    rollbackTemplate: (id) => call(`${base}/templates/${id}/rollback`, {}),
    shareLinks: () => call(`${base}/links`),
    revokeShareLink: (id, reason) => call(`${base}/links/${id}/revoke`, { reason }),
  }
}

/** The one factory the console uses. Fixtures when asked (?preview=1), live otherwise. */
export function createShareAdminSeam(opts?: { fixtures?: boolean }): ShareAdminSeam {
  const fixtures = opts?.fixtures ?? (typeof window !== 'undefined' && isFixturesMode(window.location.search))
  return fixtures ? fixturesSeam() : liveSeam()
}
