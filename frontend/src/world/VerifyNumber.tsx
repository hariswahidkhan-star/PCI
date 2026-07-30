import { useState } from 'react'
import { ApiError, makeClient } from '../api/client'
import DemoBanner from '../components/DemoBanner'

// Phase 5 — public verification of a PCI World Passport by PCI Student Number.
//
// Privacy posture, mirrored from the backend contract:
//   • POST only — the number rides in the request body, never in a URL, query string or the
//     browser history. The form never navigates; there is nothing for a proxy log to keep.
//   • One neutral answer — the server replies identically for unknown, private, withdrawn,
//     expired and suspended numbers, and this page repeats that without embroidering on it.
//   • Owner's disclosure — a successful answer contains only what the Passport owner switched
//     on; fields the owner kept private are absent from the payload and therefore from this page.
//
// The client is the shared typed wrapper (makeClient). The token key below is never written to,
// so no Authorization header is ever attached — this is a public, unauthenticated surface.
const client = makeClient('pci.world.verify.public')

const SHAPE = /^PCI-\d{4}-\d{6,}$/

interface EvidenceItem {
  title: string | null
  reference: string
  industry: string | null
  track: string | null
  difficulty: string | null
  /** present only when the owner discloses scores */
  score?: number | null
  /** present only when the owner discloses decision profiles */
  profile?: string | null
  /** present only when the owner discloses dates */
  completed_on?: string
}

interface PassportSummary {
  status: string
  discloses: string
  expires_on: string | null
  completed: number
  industries: number
  tracks: number
}

type VerifyResult =
  | { found: false; message: string }
  | {
      found: true
      student_number: string
      display_name: string
      passport: PassportSummary
      evidence: EvidenceItem[]
      evidence_total: number
      photo: string | null
      verify_page: string
      verified_at: string
      disclaimer: string
    }

export default function VerifyNumber() {
  const [number, setNumber] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState('')
  const [result, setResult] = useState<VerifyResult | null>(null)

  const normalized = number.trim().toUpperCase()

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setErr('')
    setResult(null)
    if (!SHAPE.test(normalized)) {
      setErr('Enter the full PCI Student Number, e.g. PCI-2026-000123.')
      return
    }
    setBusy(true)
    try {
      setResult(await client.post<VerifyResult>('/api/public/world-passports/verify', { number: normalized }))
    } catch (e2) {
      if (e2 instanceof ApiError && e2.status === 429)
        setErr('Too many checks from this connection — please wait a minute and try again.')
      else if (e2 instanceof ApiError && e2.status === 0)
        setErr(e2.message)
      else setErr('The verification service could not answer just now — please try again.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="world-shell">
      <DemoBanner />
      <header>
        <span className="brand">PCI <b>World</b></span>
        <span className="tag">passport verification</span>
      </header>
      <main>
        <div className="card" style={{ borderTop: '3px solid var(--gilt)' }}>
          <p className="kicker">Project Controls Institute · PCI World</p>
          <h1>Verify a PCI World Passport</h1>
          <p>
            Enter a <b>PCI Student Number</b> to check whether its holder currently publishes a
            PCI World Passport, and to see exactly what they chose to share.
          </p>
          <form onSubmit={(e) => { void submit(e) }}>
            <label htmlFor="vn_number">PCI Student Number</label>
            <input
              id="vn_number"
              placeholder="PCI-2026-000123"
              autoComplete="off"
              spellCheck={false}
              maxLength={32}
              value={number}
              onChange={(e) => { setNumber(e.target.value); setErr('') }}
              style={{ fontFamily: 'ui-monospace, Menlo, Consolas, monospace', letterSpacing: '0.06em' }}
            />
            <p><button disabled={busy}>{busy ? 'Checking…' : 'Verify this number'}</button></p>
          </form>
          {err && <p role="alert" className="err">{err}</p>}
          <p><small>
            The number is sent in the request body over HTTPS — it never appears in the page
            address, your browser history or our access logs. Checks are rate-limited.
          </small></p>
        </div>

        <div aria-live="polite">
          {result && !result.found && <NeutralCard message={result.message} onReset={() => { setResult(null); setNumber('') }} />}
          {result && result.found && <ResultCard r={result} onReset={() => { setResult(null); setNumber('') }} />}
        </div>

        {!result && (
          <div className="card">
            <h2>Were you handed a Passport link instead?</h2>
            <p>
              A Passport&rsquo;s own link (or the QR code on its PDF) can be checked directly on the{' '}
              <a href="/world/verify">Passport link verification page</a> — the live record is
              always the authority, never the document in your hand.
            </p>
          </div>
        )}
      </main>
      <footer><small>Operated by the Project Controls Institute. Practice evidence only — never a credential.</small></footer>
    </div>
  )
}

function NeutralCard({ message, onReset }: { message: string; onReset: () => void }) {
  return (
    <div className="card">
      <p className="kicker">Verification result</p>
      <h2>{message}</h2>
      <p><small>
        For privacy, PCI does not say why: the number may not be recognised, or its holder may
        simply not publish a Passport at this time. The two are deliberately indistinguishable.
      </small></p>
      <p><button className="secondary" onClick={onReset}>Check another number</button></p>
    </div>
  )
}

function ResultCard({ r, onReset }: { r: Extract<VerifyResult, { found: true }>; onReset: () => void }) {
  const p = r.passport
  return (
    <div className="card" style={{ borderColor: 'var(--gilt)', borderWidth: 2 }}>
      <p className="kicker">Verified · live record</p>
      <div style={{ display: 'flex', gap: 18, alignItems: 'flex-start', flexWrap: 'wrap' }}>
        {r.photo && (
          <img
            src={r.photo}
            alt={`Passport photograph of ${r.display_name}`}
            style={{ width: 96, height: 96, objectFit: 'cover', borderRadius: 6, border: '1px solid var(--line)' }}
          />
        )}
        <div>
          <h1 style={{ marginBottom: 4 }}>{r.display_name}</h1>
          <p style={{ margin: '0 0 6px', fontFamily: 'ui-monospace, Menlo, Consolas, monospace' }}>
            {r.student_number}
          </p>
          <p style={{ margin: 0 }}>
            <b>PCI World Passport: {p.status}</b>
            {p.expires_on && <small> · link expires {p.expires_on}</small>}
          </p>
        </div>
      </div>

      <p className="stats" style={{ marginTop: 14 }}>
        <span><b>{p.completed}</b> challenge{p.completed === 1 ? '' : 's'} published</span>
        <span><b>{p.industries}</b> industr{p.industries === 1 ? 'y' : 'ies'}</span>
        <span><b>{p.tracks}</b> track{p.tracks === 1 ? '' : 's'}</span>
      </p>
      <p><small>The owner discloses: {p.discloses}.</small></p>

      {r.evidence.length > 0 && (
        <>
          <h2 style={{ marginTop: 12 }}>Published practice evidence</h2>
          <ul>
            {r.evidence.map((it) => (
              <li key={it.reference}>
                <b>{it.title ?? 'Challenge'}</b>
                <small>
                  {' '}· {it.reference}
                  {it.industry && <> · {it.industry}</>}
                  {it.difficulty && <> · {it.difficulty}</>}
                  {it.profile != null && <> · {it.profile}</>}
                  {it.score != null && <> · score {it.score}</>}
                  {it.completed_on && <> · completed {it.completed_on}</>}
                </small>
              </li>
            ))}
          </ul>
          {r.evidence_total > r.evidence.length && (
            <p><small>Showing {r.evidence.length} of {r.evidence_total} published challenges.</small></p>
          )}
        </>
      )}

      <p><small>
        Verified against the live record at {r.verified_at}. A Passport can be withdrawn by its
        owner at any time — re-check rather than relying on a saved copy. Holding the owner&rsquo;s
        own Passport link? Confirm it on the <a href={r.verify_page}>link verification page</a>.
      </small></p>

      <p style={{ borderLeft: '3px solid var(--crimson)', paddingLeft: 12 }}>
        <small>{r.disclaimer}</small>
      </p>
      <p><button className="secondary" onClick={onReset}>Check another number</button></p>
    </div>
  )
}
