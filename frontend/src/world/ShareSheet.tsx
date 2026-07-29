import { useCallback, useEffect, useRef, useState } from 'react'

// Premium share sheet (Phase 6 Release-1, spec §8.7–8.8) — a bottom sheet on small screens, a
// centred dialog on desktop. URL-SHARING ONLY: every channel opens that network's own compose
// surface pre-filled with the EXISTING opaque public URL (/world/p/… — minted server-side at
// publication). Nothing here mints a token, and no name, email or number ever enters a URL.
//
// Provider capability honesty: the matrix below is the single statement of what this sheet can
// and cannot do, and it drives the footnote. There is deliberately no "post directly" button —
// no provider API is integrated — and Instagram is intentionally absent from the channel list
// (it does not accept link posts from the web); Instagram users are pointed at Copy caption +
// the native device share instead.

export const SHARE_CAPABILITIES = {
  supported: ['url-share'],
  unsupported: ['direct-posting', 'comment-sync'],
} as const

export const DISCLOSURE_LINE = 'Anyone with this link sees exactly what your public Passport shows.'

/** Fallback caption when the server caption has not arrived — same product line + disclaimer the
 *  server builds, minus the display name (which only the server should vouch for). */
export const FALLBACK_CAPTION = 'PCI World Passport. Practice evidence only — never a credential.'

// ── §8.6 share crop previews — pure CSS mock cards, honest by construction. ──────────────────
//
// The cards are built ONLY from the same public fields the server's OG metadata carries
// (display name, challenge/industry counts, the practice disclaimer — WorldPages.OgHead) plus
// the real og:image, which today is the static branded 1200×630 fallback at OG_IMAGE_URL —
// rendered here as the card background via its public URL. There is no canvas export and no
// per-network fidelity claim: each network re-crops and re-renders the link card its own way,
// which is exactly what PREVIEW_NOTE says on screen.

export const OG_IMAGE_URL = '/assets/og-image.jpg?v=1'

export const PREVIEW_NOTE = 'Preview approximation — each network renders its own crop.'

/** The practice disclaimer the public metadata carries — same line the caption ends with. */
export const PREVIEW_DISCLAIMER = 'Practice evidence only — never a credential.'

/** The four crops networks commonly cut a landscape link card into. */
export const CROPS = [
  { key: 'landscape', label: 'LinkedIn landscape', w: 1200, h: 630 },
  { key: 'square', label: 'Square', w: 1080, h: 1080 },
  { key: 'portrait', label: 'Portrait', w: 1080, h: 1350 },
  { key: 'story', label: 'Story', w: 1080, h: 1920 },
] as const

/** The public fields the OG metadata is derived from — nothing more may reach a preview card. */
export interface SharePreviewFields {
  displayName: string | null
  completedCount: number
  industryCount: number
}

/** Mirrors the server's og:title for a Passport: "{name} — PCI World Passport". */
export function previewTitle(displayName: string | null): string {
  return `${(displayName ?? '').trim() || 'PCI World participant'} — PCI World Passport`
}

/** Mirrors the server's og:description, pluralisation included (WorldPages.cs Passport head). */
export function previewDescription(completed: number, industries: number): string {
  return `Verified virtual project experience: ${completed} completed PCI World challenge${completed === 1 ? '' : 's'} across ${industries} industr${industries === 1 ? 'y' : 'ies'}.`
}

/** The one-line honesty footnote, derived from the capability matrix rather than hand-written so
 *  it can never drift from what the sheet actually does. */
export function capabilityFootnote(): string {
  const un: readonly string[] = SHARE_CAPABILITIES.unsupported
  const parts = ['These buttons open each network’s own compose screen with your link.']
  if (un.includes('direct-posting')) parts.push('PCI never posts on your behalf.')
  if (un.includes('comment-sync')) parts.push('Replies and comments stay on the network — they are never brought back here.')
  return parts.join(' ')
}

// ── channel glyphs — the same self-authored 24×24 currentColor paths as the server's bundled
//    SocialIcons registry (backend/Core/SocialIcons.cs), so both surfaces draw one icon set and
//    nothing is fetched from a CDN. Static constants from our own source, never user input. ──
const GLYPHS: Record<string, string> = {
  linkedin: "<circle cx='5' cy='5' r='2.2'/><rect x='3' y='9' width='4' height='12'/><path d='M10 9h3.8v1.7h.06c.6-1.05 1.9-2 3.74-2 3.9 0 4.6 2.4 4.6 5.7V21h-4v-4.9c0-1.2 0-2.7-1.7-2.7s-1.9 1.3-1.9 2.6V21h-4z'/>",
  x: "<path d='M17.7 3H21l-7.2 8.2L22.3 21H16l-4.6-6-5.3 6H2.8l7.7-8.8L1.9 3h6.4l4.2 5.5L17.7 3zm-1.1 16h1.8L7.5 4.9H5.6z'/>",
  facebook: "<path d='M14 8.5V7c0-.8.2-1.3 1.4-1.3H17V2.3C16.4 2.2 15.5 2 14.4 2 11.9 2 10.3 3.5 10.3 6.3v2.2H7.5V12h2.8v9h3.7v-9h2.8l.5-3.5H14z'/>",
  whatsapp: "<path d='M12 2a10 10 0 00-8.6 15L2 22l5.2-1.4A10 10 0 1012 2zm0 2a8 8 0 11-4.2 14.8l-.4-.2-2.4.6.6-2.3-.3-.4A8 8 0 0112 4zm-3 4.4c-.2 0-.5.1-.7.4-.3.3-.9.9-.9 2.2s1 2.6 1.1 2.7c.1.2 2 3 4.7 4.1 2.2.9 2.7.7 3.2.7.6-.1 1.7-.7 2-1.4.2-.7.2-1.2.1-1.4l-1.9-.9c-.3-.1-.5-.2-.7.1l-.7 1c-.2.1-.3.2-.6 0-.3-.1-1.1-.4-2-1.2-.7-.6-1.2-1.4-1.4-1.7-.1-.3 0-.4.1-.6l.5-.5c.1-.2.2-.3.3-.5s0-.4-.1-.5l-.9-2.1c-.2-.5-.4-.4-.6-.4z'/>",
  telegram: "<path d='M21.9 4.3L2.9 11.6c-.9.4-.9 1.5 0 1.8l4.7 1.5 1.8 5.7c.2.6.9.8 1.4.4l2.6-2.1 4.6 3.4c.6.4 1.4.1 1.6-.6l3.3-16c.2-.9-.7-1.6-1.6-1.4zM8.9 14.2l8.6-5.4c.2-.1.4.2.2.3l-7 6.3-.3 3-1.5-4.2z'/>",
  email: "<path d='M4 4h16a2 2 0 012 2v12a2 2 0 01-2 2H4a2 2 0 01-2-2V6a2 2 0 012-2zm8 7L4 6.2V7l8 5 8-5v-.8L12 11z'/>",
  link: "<path d='M7 12a3 3 0 013-3h3V7h-3a5 5 0 000 10h3v-2h-3a3 3 0 01-3-3zm2 1h6v-2H9v2zm5-6h-3v2h3a3 3 0 010 6h-3v2h3a5 5 0 000-10z'/>",
  caption: "<path d='M4 5h16v2H4zm0 4h16v2H4zm0 4h10v2H4zm0 4h7v2H4z'/>",
  device: "<path d='M14 3l4.5 4.5-1.4 1.4L15 6.8V14h-2V6.8L10.9 8.9 9.5 7.5 14 3zM6 11h4v2H8v7h12v-7h-2v-2h4v11H6V11z' transform='translate(-2 0)'/>",
  qr: "<path d='M3 3h8v8H3zm2 2v4h4V5zm8-2h8v8h-8zm2 2v4h4V5zM3 13h8v8H3zm2 2v4h4v-4zm8-2h3v3h-3zm5 0h3v3h-3zm-5 5h3v3h-3zm5 0h3v3h-3z'/>",
}

function Glyph({ name }: { name: string }) {
  return (
    <svg viewBox="0 0 24 24" width="20" height="20" fill="currentColor" aria-hidden="true" focusable="false"
      dangerouslySetInnerHTML={{ __html: GLYPHS[name] ?? GLYPHS.link }} />
  )
}

// ── URL-share channel builders. Each takes the ABSOLUTE opaque URL + the caption and returns the
//    network's own compose URL. The shared URL is always the existing opaque public URL. ──
const e = encodeURIComponent
interface Channel { key: string; label: string; href: (url: string, caption: string) => string }
export const CHANNELS: Channel[] = [
  { key: 'linkedin', label: 'LinkedIn', href: (u) => `https://www.linkedin.com/sharing/share-offsite/?url=${e(u)}` },
  { key: 'x', label: 'X', href: (u, c) => `https://twitter.com/intent/post?text=${e(c)}&url=${e(u)}` },
  { key: 'facebook', label: 'Facebook', href: (u) => `https://www.facebook.com/sharer/sharer.php?u=${e(u)}` },
  { key: 'whatsapp', label: 'WhatsApp', href: (u, c) => `https://wa.me/?text=${e(`${c} ${u}`)}` },
  { key: 'telegram', label: 'Telegram', href: (u, c) => `https://t.me/share/url?url=${e(u)}&text=${e(c)}` },
  { key: 'email', label: 'Email', href: (u, c) => `mailto:?subject=${e('PCI World Passport')}&body=${e(`${c}\n\n${u}`)}` },
]

export interface ShareSheetProps {
  open: boolean
  onClose: () => void
  /** The EXISTING opaque public URL (/world/p/…, /world/r/… or /world/i/…) — path or absolute. */
  url: string
  /** Where the server-rendered QR already lives (the public Passport page's verification block).
   *  There is no client-side QR renderer in this app, so the sheet links rather than re-draws. */
  qrHref?: string | null
  /** Server-truth caption fetcher (Copy caption). Falls back to the neutral product caption. */
  getCaption?: () => Promise<string>
  /** Public OG fields for the §8.6 crop previews. Absent → no Preview tab (sheet unchanged). */
  preview?: SharePreviewFields
}

/** One pure-CSS mock card: the real og:image as background, the OG text fields on top. */
function CropCard({ crop, fields }: { crop: (typeof CROPS)[number]; fields: SharePreviewFields }) {
  return (
    <figure className="ss-crop" data-crop={crop.key}>
      <div
        className="ss-crop-card"
        style={{ aspectRatio: `${crop.w} / ${crop.h}`, backgroundImage: `url(${OG_IMAGE_URL})` }}
      >
        <div className="ss-crop-scrim">
          <p className="ss-crop-title">{previewTitle(fields.displayName)}</p>
          <p className="ss-crop-desc">{previewDescription(fields.completedCount, fields.industryCount)}</p>
          <p className="ss-crop-disclaimer">{PREVIEW_DISCLAIMER}</p>
        </div>
      </div>
      <figcaption className="ss-crop-caption">{crop.label} · {crop.w}×{crop.h}</figcaption>
    </figure>
  )
}

export default function ShareSheet({ open, onClose, url, qrHref, getCaption, preview }: ShareSheetProps) {
  const [caption, setCaption] = useState(FALLBACK_CAPTION)
  const [tab, setTab] = useState<'share' | 'preview'>('share')
  const [note, setNote] = useState('')
  const [fallback, setFallback] = useState<{ kind: 'link' | 'caption'; value: string } | null>(null)
  const panelRef = useRef<HTMLDivElement>(null)
  const fallbackRef = useRef<HTMLTextAreaElement>(null)
  const openerRef = useRef<Element | null>(null)

  // The absolute form of the SAME opaque URL — resolved, never minted.
  const absUrl = new URL(url, window.location.origin).toString()

  useEffect(() => {
    if (!open) return
    setNote(''); setFallback(null); setTab('share')
    let stale = false
    void getCaption?.().then(c => { if (!stale && c) setCaption(c) }).catch(() => { /* fallback stands */ })
    return () => { stale = true }
  }, [open, getCaption])

  // Focus management: remember the opener, move focus in, trap Tab, close on Escape, restore.
  useEffect(() => {
    if (!open) return
    openerRef.current = document.activeElement
    panelRef.current?.querySelector<HTMLElement>('button, a[href], textarea')?.focus()
    return () => { (openerRef.current as HTMLElement | null)?.focus?.() }
  }, [open])

  const onKeyDown = useCallback((ev: React.KeyboardEvent) => {
    if (ev.key === 'Escape') { ev.stopPropagation(); onClose(); return }
    if (ev.key !== 'Tab' || !panelRef.current) return
    const focusables = Array.from(panelRef.current.querySelectorAll<HTMLElement>(
      'button:not([disabled]), a[href], textarea, input, select'))
    if (focusables.length === 0) return
    const first = focusables[0]
    const last = focusables[focusables.length - 1]
    if (ev.shiftKey && document.activeElement === first) { ev.preventDefault(); last.focus() }
    else if (!ev.shiftKey && document.activeElement === last) { ev.preventDefault(); first.focus() }
  }, [onClose])

  // Select-the-text fallback when the clipboard is denied or absent.
  useEffect(() => { if (fallback) { fallbackRef.current?.focus(); fallbackRef.current?.select() } }, [fallback])

  if (!open) return null

  const copy = async (kind: 'link' | 'caption') => {
    const value = kind === 'link' ? absUrl : caption
    try {
      if (!navigator.clipboard?.writeText) throw new Error('no clipboard')
      await navigator.clipboard.writeText(value)
      setFallback(null)
      setNote(kind === 'link' ? 'Link copied.' : 'Caption copied.')
    } catch {
      setNote('')
      setFallback({ kind, value })
    }
  }

  // Feature-detected native Web Share — invoked only from this click (a user gesture). A resolved
  // promise means the OS took the handoff, NOT that anything was published anywhere.
  const canNativeShare = typeof navigator !== 'undefined' && typeof navigator.share === 'function'
  const nativeShare = async () => {
    try {
      await navigator.share({ title: 'PCI World Passport', text: caption, url: absUrl })
      setNote('Handed to your device’s share menu — nothing is posted until you finish there.')
    } catch { /* dismissed — say nothing */ }
  }

  return (
    <div className="ss-backdrop" onMouseDown={(ev) => { if (ev.target === ev.currentTarget) onClose() }}>
      <div ref={panelRef} role="dialog" aria-modal="true" aria-labelledby="ss-title"
        className="ss-panel" onKeyDown={onKeyDown}>
        <div className="ss-head">
          <h2 id="ss-title">Share your Passport</h2>
          <button type="button" className="ss-close" onClick={onClose} aria-label="Close share sheet">&#215;</button>
        </div>

        <p className="ss-disclosure">{DISCLOSURE_LINE}</p>

        {preview && (
          <div role="tablist" aria-label="Share or preview" className="ss-tabs">
            <button type="button" role="tab" id="ss-tab-share" aria-selected={tab === 'share'}
              aria-controls="ss-panel-share" className="ss-tab" onClick={() => setTab('share')}>
              Share
            </button>
            <button type="button" role="tab" id="ss-tab-preview" aria-selected={tab === 'preview'}
              aria-controls="ss-panel-preview" className="ss-tab" onClick={() => setTab('preview')}>
              Preview
            </button>
          </div>
        )}

        {preview && tab === 'preview' ? (
          <div role="tabpanel" id="ss-panel-preview" aria-labelledby="ss-tab-preview" className="ss-previews">
            <p className="ss-preview-note">{PREVIEW_NOTE}</p>
            <div className="ss-crops">
              {CROPS.map(c => <CropCard key={c.key} crop={c} fields={preview} />)}
            </div>
          </div>
        ) : (
          <div {...(preview ? { role: 'tabpanel', id: 'ss-panel-share', 'aria-labelledby': 'ss-tab-share' } : {})}>
        <ul className="ss-channels">
          {CHANNELS.map(c => (
            <li key={c.key}>
              <a className="ss-channel" href={c.href(absUrl, caption)}
                {...(c.key === 'email' ? {} : { target: '_blank', rel: 'noopener noreferrer' })}>
                <Glyph name={c.key} /><span className="ss-label">{c.label}</span>
              </a>
            </li>
          ))}
          <li>
            <button type="button" className="ss-channel" onClick={() => { void copy('link') }}>
              <Glyph name="link" /><span className="ss-label">Copy link</span>
            </button>
          </li>
          <li>
            <button type="button" className="ss-channel" onClick={() => { void copy('caption') }}>
              <Glyph name="caption" /><span className="ss-label">Copy caption</span>
            </button>
          </li>
          {canNativeShare && (
            <li>
              <button type="button" className="ss-channel" onClick={() => { void nativeShare() }}>
                <Glyph name="device" /><span className="ss-label">Share via device</span>
              </button>
            </li>
          )}
          {qrHref && (
            <li>
              <a className="ss-channel" href={qrHref} target="_blank" rel="noopener noreferrer">
                <Glyph name="qr" /><span className="ss-label">View QR code</span>
              </a>
            </li>
          )}
        </ul>

        <p className="ss-instagram">
          {canNativeShare
            ? 'For Instagram: use “Share via device” or “Copy caption”, then post from the Instagram app — Instagram does not accept link posts from the web.'
            : 'For Instagram: use “Copy caption” and “Copy link”, then post from the Instagram app — Instagram does not accept link posts from the web.'}
        </p>

        {note && <p role="status" className="ss-note">{note}</p>}
        {fallback && (
          <div className="ss-fallback">
            <label htmlFor="ss-fallback-text">
              Copying was blocked by your browser — the text below is selected, copy it yourself.
            </label>
            <textarea id="ss-fallback-text" ref={fallbackRef} readOnly rows={fallback.kind === 'link' ? 2 : 3}
              value={fallback.value} onFocus={(ev) => ev.currentTarget.select()} />
          </div>
        )}

        <p className="ss-footnote">{capabilityFootnote()}</p>
        </div>
        )}
      </div>
    </div>
  )
}
