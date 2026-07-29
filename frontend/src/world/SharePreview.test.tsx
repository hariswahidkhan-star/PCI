import { describe, expect, it } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import ShareSheet, {
  CROPS, OG_IMAGE_URL, PREVIEW_DISCLAIMER, PREVIEW_NOTE,
  previewDescription, previewTitle,
} from './ShareSheet'

// §8.6 crop previews: pure CSS mock cards built ONLY from the public fields the OG metadata
// carries (display name, counts, disclaimer) over the real static og:image. The contract under
// test: fields-only rendering (nothing private, not even the share URL, reaches a card), the
// four crops with true pixel labels, the honest "approximation" note, and back-compat (no
// preview prop → the sheet is exactly what it was before).

const OPAQUE = '/world/p/a1b2c3d4e5f6'
const FIELDS = { displayName: 'Zoë Rivéra', completedCount: 7, industryCount: 3 }

function sheet(preview?: typeof FIELDS) {
  return (
    <ShareSheet open onClose={() => undefined} url={OPAQUE} qrHref={`${OPAQUE}#verify`} preview={preview} />
  )
}

describe('preview text helpers mirror the server OG derivation', () => {
  it('title is "{name} — PCI World Passport", with a neutral fallback for a missing name', () => {
    expect(previewTitle('Zoë Rivéra')).toBe('Zoë Rivéra — PCI World Passport')
    expect(previewTitle(null)).toBe('PCI World participant — PCI World Passport')
    expect(previewTitle('   ')).toBe('PCI World participant — PCI World Passport')
  })

  it('description carries the counts with server-matching pluralisation', () => {
    expect(previewDescription(7, 3)).toBe(
      'Verified virtual project experience: 7 completed PCI World challenges across 3 industries.')
    expect(previewDescription(1, 1)).toBe(
      'Verified virtual project experience: 1 completed PCI World challenge across 1 industry.')
  })
})

describe('Preview tab', () => {
  it('does not exist without the preview prop — the sheet is unchanged for old callers', () => {
    render(sheet())
    expect(screen.queryByRole('tab')).toBeNull()
    expect(screen.queryByText('Preview')).toBeNull()
    expect(screen.getByText('LinkedIn')).toBeInTheDocument()
  })

  it('shows all four crops with true pixel dimensions and the approximation label', () => {
    render(sheet(FIELDS))
    fireEvent.click(screen.getByRole('tab', { name: 'Preview' }))

    expect(screen.getByText(PREVIEW_NOTE)).toBeInTheDocument()
    expect(PREVIEW_NOTE).toMatch(/approximation/i)
    expect(PREVIEW_NOTE).toMatch(/each network renders its own crop/i)

    expect(CROPS.map(c => `${c.w}×${c.h}`)).toEqual(['1200×630', '1080×1080', '1080×1350', '1080×1920'])
    for (const c of CROPS) {
      expect(screen.getByText(`${c.label} · ${c.w}×${c.h}`)).toBeInTheDocument()
    }
    expect(screen.getByText('LinkedIn landscape · 1200×630')).toBeInTheDocument()
  })

  it('every card is built from the SAME public OG fields, over the real og:image URL', () => {
    const { container } = render(sheet(FIELDS))
    fireEvent.click(screen.getByRole('tab', { name: 'Preview' }))

    const cards = Array.from(container.querySelectorAll<HTMLElement>('.ss-crop-card'))
    expect(cards).toHaveLength(4)
    for (const card of cards) {
      // the static branded fallback IS the background — no per-entity artwork is faked
      // (jsdom normalises url(x) to url("x"), so compare on the contained URL)
      expect(card.style.backgroundImage).toContain(OG_IMAGE_URL)
      expect(card.textContent).toContain('Zoë Rivéra — PCI World Passport')
      expect(card.textContent).toContain(
        'Verified virtual project experience: 7 completed PCI World challenges across 3 industries.')
      expect(card.textContent).toContain(PREVIEW_DISCLAIMER)
      // fields-only: the opaque share URL/token never reaches a card
      expect(card.textContent).not.toContain('a1b2c3d4e5f6')
      expect(card.textContent).not.toContain('/world/p/')
    }
    expect(OG_IMAGE_URL).toBe('/assets/og-image.jpg?v=1')
  })

  it('renders a hostile display name as inert text — never markup', () => {
    const hostile = '"><img src=x onerror="alert(1)"><script>alert(2)</script>'
    const { container } = render(sheet({ ...FIELDS, displayName: hostile }))
    fireEvent.click(screen.getByRole('tab', { name: 'Preview' }))

    // the name appears literally…
    expect(screen.getAllByText((t) => t.includes(hostile)).length).toBeGreaterThan(0)
    // …and nothing it "wrote" exists as an element
    expect(container.querySelector('img')).toBeNull()
    expect(container.querySelector('script')).toBeNull()
  })

  it('no canvas, no download/export button — no fake fidelity claims', () => {
    const { container } = render(sheet(FIELDS))
    fireEvent.click(screen.getByRole('tab', { name: 'Preview' }))
    expect(container.querySelector('canvas')).toBeNull()
    expect(screen.queryByText(/download/i)).toBeNull()
    expect(screen.queryByText(/export/i)).toBeNull()
  })

  it('tabs switch back to the share panel and stay accessible (roles + aria-selected)', () => {
    render(sheet(FIELDS))
    const shareTab = screen.getByRole('tab', { name: 'Share' })
    const previewTab = screen.getByRole('tab', { name: 'Preview' })
    expect(shareTab).toHaveAttribute('aria-selected', 'true')

    fireEvent.click(previewTab)
    expect(previewTab).toHaveAttribute('aria-selected', 'true')
    expect(screen.queryByText('LinkedIn')).toBeNull()          // channels hidden on preview

    fireEvent.click(shareTab)
    expect(screen.getByText('LinkedIn')).toBeInTheDocument()   // channels back on share
    expect(screen.getByRole('tablist')).toBeInTheDocument()
  })
})
