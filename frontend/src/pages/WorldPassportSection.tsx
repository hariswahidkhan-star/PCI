import { useEffect, useState } from 'react'
import { api } from '../api/client'
import type { PassportSummary } from '../api/types'
import { PassportCard, PassportDownloadSheet, PassportEmptyState, PassportSkeleton } from '../components/passport'

/**
 * The first-class "PCI World Passport" module on the MyPCI dashboard (Phase 4, spec §6): the SAME
 * authoritative Passport read through GET /api/me/world-passport/summary — no iframe, no copied
 * data. Buttons cross into PCI World through the existing SSO bridge (the one-time fragment
 * handoff Profile.tsx already uses), and a fetch failure degrades to a quiet fallback card so
 * this module can never break the rest of MyPCI.
 */
export default function WorldPassportSection() {
  const [summary, setSummary] = useState<PassportSummary | null>(null)
  const [failed, setFailed] = useState(false)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [showDownloads, setShowDownloads] = useState(false)

  useEffect(() => {
    let live = true
    api
      .get<PassportSummary>('/api/me/world-passport/summary')
      .then((s) => { if (live) setSummary(s) })
      .catch(() => { if (live) setFailed(true) })
    return () => { live = false }
  }, [])

  // The existing SSO card behaviour (see Profile.tsx PassportCard): the response carries a
  // one-time handoff code in the URL fragment — never a reusable token through this origin.
  async function open(returnTo: string) {
    setBusy(true)
    setErr(null)
    try {
      const r = await api.post<{ url?: string }>('/api/me/world-passport/sso', {
        world_session: localStorage.getItem('world_session') || undefined,
        return_to: returnTo,
      })
      window.location.assign(r.url || '/world/account')
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not open PCI World.')
      setBusy(false)
    }
  }

  if (failed) {
    // Quiet degradation: a small note in the Passport frame, nothing thrown, nothing blocking.
    return (
      <PassportEmptyState
        variant="unavailable"
        actions={
          <button className="pp-btn pp-btn--ghost" disabled={busy} onClick={() => { void open('/world/account') }}>
            Open PCI World
          </button>
        }
      />
    )
  }
  if (!summary) return <PassportSkeleton />

  if (summary.participantState === 'link_pending') {
    return (
      <PassportEmptyState
        variant="link"
        actions={
          <>
            <button className="pp-btn" disabled={busy} onClick={() => { void open('/world/account') }}>
              {busy ? 'Opening…' : 'Link my PCI World account'}
            </button>
            {err && <p className="pp-err" role="alert">{err}</p>}
          </>
        }
      />
    )
  }

  if (summary.passportState === 'not_created') {
    return (
      <PassportEmptyState
        variant="create"
        actions={
          <>
            <button className="pp-btn" disabled={busy} onClick={() => { void open('/world/account') }}>
              {busy ? 'Opening…' : 'Create my Passport'}
            </button>
            {err && <p className="pp-err" role="alert">{err}</p>}
          </>
        }
      />
    )
  }

  return (
    <PassportCard
      summary={summary}
      actions={
        <>
          <button className="pp-btn" disabled={busy} onClick={() => { void open('/world/account') }}>
            {busy ? 'Opening…' : 'View full Passport'}
          </button>
          <button className="pp-btn pp-btn--ghost" disabled={busy} onClick={() => { void open('/world') }}>
            Manage in PCI World
          </button>
          {/* Phase 5A: the printable verification documents (wallet card + event badge). The
              sheet mirrors the server's publication gate — downloads only while published. */}
          <button
            className="pp-btn pp-btn--ghost"
            aria-expanded={showDownloads}
            onClick={() => { setShowDownloads((v) => !v) }}
          >
            Download PCI Passport
          </button>
        </>
      }
      footer={
        <>
          {err && <p className="pp-err" role="alert">{err}</p>}
          {showDownloads && <PassportDownloadSheet summary={summary} />}
        </>
      }
    />
  )
}
