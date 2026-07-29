import { useCallback, useEffect, useState } from 'react'
import './worldadmin.css'
import { SHARE_CAPABILITIES, capabilityFootnote } from '../world/ShareSheet'
import { HOSTILE_SAMPLE_NAME, SAMPLE_CHALLENGE_TITLE } from './fixtures'
import {
  PLACEHOLDER_HELP, PLACEHOLDER_WHITELIST, renderTemplate, unknownPlaceholders,
} from './template'
import {
  ShareAdminNotEnabledError, createShareAdminSeam,
  type ShareAdminSeam, type ShareChannel, type ShareLinkRow, type TemplateVersion,
} from './seam'

// §10.2 PCI World share-management console — a self-contained component tree, NOT mounted in any
// app shell (see this folder's README.md: the mount decision is deferred to integration). Every
// screen reads through the ONE seam in ./seam.ts; the backing endpoints do not exist yet, so the
// live seam's 404 renders the designed "not yet enabled" state, and ?preview=1 (fixtures mode)
// exercises the whole console against ./fixtures.

export const NOT_ENABLED_TITLE = 'Share administration is not enabled yet'
export const ANALYTICS_PLACEHOLDER = 'requires backend analytics — not yet enabled'
export const PROVIDER_DISABLED_NOTE = 'requires provider approval — disabled'

// ── channels ────────────────────────────────────────────────────────────────────────────────

function ChannelsCard({ seam, channels, onChange }: {
  seam: ShareAdminSeam
  channels: ShareChannel[]
  onChange: (next: ShareChannel[]) => void
}) {
  const [err, setErr] = useState('')
  const act = (fn: () => Promise<ShareChannel[]>) => {
    setErr('')
    void fn().then(onChange).catch(() => setErr('Could not save the channel change.'))
  }
  return (
    <section className="wa-card" aria-labelledby="wa-channels-h">
      <h2 id="wa-channels-h">Share channels</h2>
      <p className="wa-help">
        Order and availability of the buttons participants see in the share sheet. Reordering uses
        up/down buttons on purpose — they work with a keyboard and a screen reader; drag does not.
      </p>
      <ol className="wa-channels">
        {channels.map((c, i) => (
          <li key={c.key} className={c.enabled ? '' : 'wa-channel--off'}>
            <span className="wa-channel-name">{c.label}</span>
            <span className="wa-channel-state">{c.enabled ? 'enabled' : 'disabled'}</span>
            <button type="button" className="wa-btn wa-btn--ghost" disabled={i === 0}
              aria-label={`Move ${c.label} up`}
              onClick={() => act(() => seam.moveChannel(c.key, 'up'))}>↑</button>
            <button type="button" className="wa-btn wa-btn--ghost" disabled={i === channels.length - 1}
              aria-label={`Move ${c.label} down`}
              onClick={() => act(() => seam.moveChannel(c.key, 'down'))}>↓</button>
            <button type="button" className="wa-btn wa-btn--ghost"
              aria-label={`${c.enabled ? 'Disable' : 'Enable'} ${c.label}`}
              onClick={() => act(() => seam.setChannelEnabled(c.key, !c.enabled))}>
              {c.enabled ? 'Disable' : 'Enable'}
            </button>
          </li>
        ))}
      </ol>
      {err && <p role="alert" className="wa-err">{err}</p>}
    </section>
  )
}

// ── templates ───────────────────────────────────────────────────────────────────────────────

const LOCALES = ['en', 'fr', 'es', 'ar'] as const

function TemplatesCard({ seam }: { seam: ShareAdminSeam }) {
  const [locale, setLocale] = useState<string>('en')
  const [versions, setVersions] = useState<TemplateVersion[]>([])
  const [caption, setCaption] = useState('')
  const [title, setTitle] = useState('')
  const [hashtags, setHashtags] = useState('')
  const [err, setErr] = useState('')
  const [msg, setMsg] = useState('')

  const load = useCallback(async (loc: string) => {
    const vs = await seam.templates(loc)
    setVersions(vs)
    const editable = vs.find(v => v.state === 'draft') ?? vs.find(v => v.state === 'published')
    setCaption(editable?.caption ?? '')
    setTitle(editable?.title ?? '')
    setHashtags(editable?.hashtags ?? '')
  }, [seam])

  useEffect(() => { void load(locale).catch(() => setErr('Could not load templates.')) }, [load, locale])

  const act = (fn: () => Promise<TemplateVersion[]>, done: string) => {
    setErr(''); setMsg('')
    void fn().then(vs => { setVersions(vs); setMsg(done) }).catch(() => setErr('Could not save.'))
  }

  // The preview is deliberately rendered with a HOSTILE sample name: if this screen ever showed
  // it as markup instead of literal text, the failure would be visible immediately.
  const sampleValues = { display_name: HOSTILE_SAMPLE_NAME, challenge_title: SAMPLE_CHALLENGE_TITLE }
  const unknown = [
    ...unknownPlaceholders(caption), ...unknownPlaceholders(title), ...unknownPlaceholders(hashtags),
  ].filter((v, i, a) => a.indexOf(v) === i)

  return (
    <section className="wa-card" aria-labelledby="wa-templates-h">
      <h2 id="wa-templates-h">Caption, title &amp; hashtag templates</h2>
      <p className="wa-help">
        Free text plus the whitelisted placeholders below. Anything else in braces is left exactly
        as typed, and every substituted value is rendered as plain text — never markup.
      </p>

      <div className="wa-chips" aria-label="Available placeholders">
        {PLACEHOLDER_WHITELIST.map(p => (
          <span key={p} className="wa-chip" title={PLACEHOLDER_HELP[p]}>{`{${p}}`}</span>
        ))}
      </div>

      <label htmlFor="wa-locale">Locale</label>
      <select id="wa-locale" value={locale} onChange={e => setLocale(e.target.value)}>
        {LOCALES.map(l => <option key={l} value={l}>{l}</option>)}
      </select>

      <label htmlFor="wa-title">Title template</label>
      <input id="wa-title" value={title} onChange={e => setTitle(e.target.value)} />
      <label htmlFor="wa-caption">Caption template</label>
      <textarea id="wa-caption" rows={3} value={caption} onChange={e => setCaption(e.target.value)} />
      <label htmlFor="wa-hashtags">Hashtags</label>
      <input id="wa-hashtags" value={hashtags} onChange={e => setHashtags(e.target.value)} />

      {unknown.length > 0 && (
        <p className="wa-warn" role="status">
          Not on the whitelist (left as typed): {unknown.map(u => `{${u}}`).join(', ')}
        </p>
      )}

      <div className="wa-preview" aria-label="Template preview">
        <h3>Preview <small>— rendered with a hostile sample name to prove escaping</small></h3>
        <p className="wa-preview-title">{renderTemplate(title, sampleValues)}</p>
        <p className="wa-preview-caption">{renderTemplate(caption, sampleValues)}</p>
        <p className="wa-preview-hashtags">{renderTemplate(hashtags, sampleValues)}</p>
      </div>

      <p>
        <button type="button" className="wa-btn"
          onClick={() => act(() => seam.saveDraft(locale, { caption, title, hashtags }), 'Draft saved.')}>
          Save draft
        </button>
      </p>

      <h3>Versions</h3>
      <table className="wa-table">
        <thead><tr><th>v</th><th>State</th><th>Created</th><th>Caption</th><th aria-label="Actions" /></tr></thead>
        <tbody>
          {[...versions].sort((a, b) => b.version - a.version).map(v => (
            <tr key={v.id}>
              <td>v{v.version}</td>
              <td><span className={`wa-state wa-state--${v.state}`}>{v.state}</span></td>
              <td>{v.created_at}</td>
              <td className="wa-tpl-body">{v.caption}</td>
              <td>
                {v.state === 'draft' && (
                  <button type="button" className="wa-btn"
                    onClick={() => act(() => seam.publishTemplate(v.id), `v${v.version} published.`)}>
                    Publish
                  </button>
                )}
                {v.state === 'superseded' && (
                  <button type="button" className="wa-btn wa-btn--ghost"
                    onClick={() => act(() => seam.rollbackTemplate(v.id), `Rolled back to v${v.version}.`)}>
                    Roll back to this
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {msg && <p role="status" className="wa-ok">{msg}</p>}
      {err && <p role="alert" className="wa-err">{err}</p>}
    </section>
  )
}

// ── share-link moderation ───────────────────────────────────────────────────────────────────

function ModerationCard({ seam, links, onChange }: {
  seam: ShareAdminSeam
  links: ShareLinkRow[]
  onChange: (next: ShareLinkRow[]) => void
}) {
  const [confirming, setConfirming] = useState<number | null>(null)
  const [reason, setReason] = useState('')
  const [err, setErr] = useState('')

  const revoke = (id: number) => {
    setErr('')
    void seam.revokeShareLink(id, reason.trim())
      .then(next => { onChange(next); setConfirming(null); setReason('') })
      .catch(() => setErr('Could not revoke the link.'))
  }

  return (
    <section className="wa-card" aria-labelledby="wa-links-h">
      <h2 id="wa-links-h">Share-link moderation</h2>
      <p className="wa-help">
        Revoking stops a public link resolving immediately. It is an audit event: a reason is
        required and stored. Raw tokens never appear here — the server keeps only their hashes.
      </p>
      <table className="wa-table">
        <thead><tr><th>Kind</th><th>Code</th><th>Owner</th><th>Created</th><th>Status</th><th aria-label="Actions" /></tr></thead>
        <tbody>
          {links.map(l => (
            <tr key={l.id}>
              <td>{l.kind}</td>
              <td><code>{l.code}</code></td>
              <td className="wa-owner">{l.owner_display_name}</td>
              <td>{l.created_at}</td>
              <td>{l.revoked ? <span className="wa-state wa-state--revoked">revoked{l.revoke_reason ? ` — ${l.revoke_reason}` : ''}</span> : 'live'}</td>
              <td>
                {!l.revoked && confirming !== l.id && (
                  <button type="button" className="wa-btn wa-btn--danger"
                    onClick={() => { setConfirming(l.id); setReason('') }}>
                    Revoke
                  </button>
                )}
                {confirming === l.id && (
                  <div className="wa-confirm">
                    <label htmlFor={`wa-reason-${l.id}`}>Reason (required, recorded in the audit log)</label>
                    <input id={`wa-reason-${l.id}`} value={reason} autoFocus
                      onChange={e => setReason(e.target.value)} />
                    <button type="button" className="wa-btn wa-btn--danger" disabled={reason.trim() === ''}
                      onClick={() => revoke(l.id)}>
                      Confirm revoke
                    </button>{' '}
                    <button type="button" className="wa-btn wa-btn--ghost"
                      onClick={() => { setConfirming(null); setReason('') }}>
                      Cancel
                    </button>
                  </div>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {err && <p role="alert" className="wa-err">{err}</p>}
    </section>
  )
}

// ── provider capabilities — mirrors the participant ShareSheet's honesty matrix ─────────────

function CapabilityCard() {
  // Read straight from the participant sheet's SHARE_CAPABILITIES so the two surfaces can never
  // drift: what the sheet honestly does is exactly what this panel reports.
  const supported = SHARE_CAPABILITIES.supported as readonly string[]
  const unsupported = SHARE_CAPABILITIES.unsupported as readonly string[]
  const LABELS: Record<string, string> = {
    'url-share': 'URL share (each network’s own compose screen)',
    'direct-posting': 'Direct posting from PCI',
    'comment-sync': 'Comment / reply sync back to PCI',
  }
  return (
    <section className="wa-card" aria-labelledby="wa-caps-h">
      <h2 id="wa-caps-h">Provider capabilities</h2>
      <table className="wa-table">
        <thead><tr><th>Capability</th><th>Status</th></tr></thead>
        <tbody>
          {supported.map(k => (
            <tr key={k}>
              <td>{LABELS[k] ?? k}</td>
              <td><span className="wa-state wa-state--published">supported</span></td>
            </tr>
          ))}
          {unsupported.map(k => (
            <tr key={k}>
              <td>{LABELS[k] ?? k}</td>
              <td><span className="wa-state wa-state--disabled">{PROVIDER_DISABLED_NOTE}</span></td>
            </tr>
          ))}
        </tbody>
      </table>
      <p className="wa-help">{capabilityFootnote()}</p>
    </section>
  )
}

// ── analytics — honest placeholder ──────────────────────────────────────────────────────────

function AnalyticsCard() {
  return (
    <section className="wa-card wa-card--placeholder" aria-labelledby="wa-analytics-h">
      <h2 id="wa-analytics-h">Share analytics</h2>
      <p className="wa-placeholder">{ANALYTICS_PLACEHOLDER}</p>
      <p className="wa-help">
        No click, referral or conversion numbers are shown because none are collected yet. This
        card exists so the absence is a stated decision, not an empty chart.
      </p>
    </section>
  )
}

// ── console root ────────────────────────────────────────────────────────────────────────────

export default function ShareConsole({ seam: injected }: { seam?: ShareAdminSeam } = {}) {
  const [seam] = useState<ShareAdminSeam>(() => injected ?? createShareAdminSeam())
  const [channels, setChannels] = useState<ShareChannel[] | null>(null)
  const [links, setLinks] = useState<ShareLinkRow[] | null>(null)
  const [notEnabled, setNotEnabled] = useState(false)
  const [err, setErr] = useState('')

  useEffect(() => {
    let stale = false
    void Promise.all([seam.channels(), seam.shareLinks()])
      .then(([cs, ls]) => { if (!stale) { setChannels(cs); setLinks(ls) } })
      .catch((e: unknown) => {
        if (stale) return
        if (e instanceof ShareAdminNotEnabledError) setNotEnabled(true)
        else setErr('Could not load share administration.')
      })
    return () => { stale = true }
  }, [seam])

  if (notEnabled) {
    return (
      <div className="wa-console">
        <section className="wa-card wa-card--placeholder" aria-labelledby="wa-ne-h">
          <h2 id="wa-ne-h">{NOT_ENABLED_TITLE}</h2>
          <p>
            This deployment has no <code>/api/world-admin/share</code> endpoints yet — the console
            in front of you is finished, the service behind it is not. Nothing is broken and
            nothing was lost; there is simply no share configuration to manage here yet.
          </p>
          <p className="wa-help">
            To explore the console against sample data, open it with <code>?preview=1</code>.
          </p>
        </section>
        <CapabilityCard />
      </div>
    )
  }

  if (!channels || !links) {
    return <div className="wa-console"><p>{err || 'Loading…'}</p></div>
  }

  return (
    <div className="wa-console">
      {seam.mode === 'fixtures' && (
        <p className="wa-banner" role="status">
          Preview mode — sample data only. Nothing here reads or writes real share settings.
        </p>
      )}
      <ChannelsCard seam={seam} channels={channels} onChange={setChannels} />
      <TemplatesCard seam={seam} />
      <ModerationCard seam={seam} links={links} onChange={setLinks} />
      <CapabilityCard />
      <AnalyticsCard />
    </div>
  )
}
