import { useState } from 'react'
import type { PassportSummary } from '../../api/types'
import { getToken } from '../../api/client'
import './passport.css'

/**
 * Phase 5A (Release 1) — the printable Passport documents sheet (spec §7A.3–§7A.5).
 *
 * Lists the two verification-QR documents (wallet card, event badge) with their physical
 * dimensions and print guidance, and downloads them from
 * GET /api/me/world-passport/documents/{format} as authenticated blobs (a bare <a href> would not
 * carry the bearer token). The QR on both documents opens PUBLIC Passport verification — it is not
 * an event entry pass, and the sheet says so; the event-admission system is a later slice.
 *
 * The backend refuses (404) unless the Passport is currently PUBLISHED and unexpired, so the
 * sheet disables the downloads in every other state and explains what unlocks them — the UI
 * mirrors the server gate, it is never the gate.
 */

type Format = 'wallet' | 'badge'

const FORMATS: Array<{ format: Format; file: string; title: string; dims: string; note: string }> = [
  {
    format: 'wallet',
    file: 'Wallet',
    title: 'Wallet card',
    dims: 'CR80 · 85.60 × 53.98 mm — standard bank-card size',
    note: 'Two pages: identity front, verification QR back. Print double-sided or fold.',
  },
  {
    format: 'badge',
    file: 'Event-Badge',
    title: 'Event badge',
    dims: '4 × 6 in portrait · 101.6 × 152.4 mm',
    note: 'Large "Scan to verify" QR for lanyard sleeves and event check-in desks.',
  },
]

function lockedReason(state: PassportSummary['passportState']): string {
  switch (state) {
    case 'expired':
      return 'Your public Passport link has expired — renew it in PCI World to restore verification, then download again.'
    case 'suspended':
      return 'Your PCI World participation is suspended, so no verifiable document can be issued right now.'
    default:
      return 'Publish your Passport in PCI World first — a printed document must point at a live public record, so downloads unlock once your Passport is published.'
  }
}

export default function PassportDownloadSheet({ summary }: { summary: PassportSummary }) {
  const published = summary.passportState === 'published'
  const [busy, setBusy] = useState<Format | null>(null)
  const [err, setErr] = useState<string | null>(null)

  async function download(format: Format, file: string) {
    setBusy(format)
    setErr(null)
    try {
      const token = getToken()
      const res = await fetch(`/api/me/world-passport/documents/${format}`, {
        headers: token ? { Authorization: 'Bearer ' + token } : {},
      })
      if (!res.ok) {
        setErr(
          res.status === 404
            ? 'Documents are only available while your Passport is published and current.'
            : 'Could not prepare the document — please try again.',
        )
        return
      }
      const blob = await res.blob()
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `PCI-Passport-${summary.studentNumber ?? 'document'}-${file}.pdf`
      document.body.appendChild(a)
      a.click()
      a.remove()
      URL.revokeObjectURL(url)
    } catch {
      setErr('Could not prepare the document — please check your connection and try again.')
    } finally {
      setBusy(null)
    }
  }

  return (
    <section className="pp-downloads" aria-label="Printable Passport documents">
      <p className="pp-dl-eyebrow">Printable Passport documents</p>

      {!published && (
        <p className="pp-note" role="note">
          {lockedReason(summary.passportState)}
        </p>
      )}

      <ul className="pp-dl-list">
        {FORMATS.map((f) => (
          <li key={f.format} className="pp-dl-item">
            <div className="pp-dl-info">
              <div className="pp-dl-title">{f.title}</div>
              <div className="pp-dl-meta">{f.dims}</div>
              <div className="pp-dl-meta">{f.note}</div>
            </div>
            <button
              className="pp-btn"
              disabled={!published || busy !== null}
              onClick={() => {
                void download(f.format, f.file)
              }}
            >
              {busy === f.format ? 'Preparing…' : 'Download PDF'}
            </button>
          </li>
        ))}
      </ul>

      <p className="pp-dl-guidance">
        Print at actual size (100% scale — never “fit to page” or “shrink to fit”) on white stock.
      </p>
      <p className="pp-dl-guidance">
        The QR code on each document opens public Passport verification. It is not an event entry
        pass, and it never encodes your Student Number, email or any personal data — only an opaque
        verification link that stops working the moment you unpublish your Passport.
      </p>

      {err && (
        <p className="pp-err" role="alert">
          {err}
        </p>
      )}
    </section>
  )
}
