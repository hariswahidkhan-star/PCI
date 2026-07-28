import { describe, expect, it, vi, afterEach } from 'vitest'
import { act, fireEvent, render, screen, waitFor } from '@testing-library/react'
import ShareSheet, { CHANNELS, DISCLOSURE_LINE, FALLBACK_CAPTION, SHARE_CAPABILITIES, capabilityFootnote } from './ShareSheet'

// The premium share sheet's contract: channels carry the EXISTING opaque URL correctly encoded
// (Unicode captions included), the dialog traps focus and closes on Escape, native share is
// feature-detected and treated as a handoff, the clipboard-denied path degrades to select-the-
// text, and the capability matrix keeps the sheet honest (no direct posting, no Instagram link).

const OPAQUE = '/world/p/a1b2c3d4e5f6'
const ABS = new URL(OPAQUE, window.location.origin).toString()
const UNICODE_CAPTION = 'Zoë Rivéra 中文 — PCI World Passport. Practice evidence only — never a credential.'

function defineOnNavigator(key: string, value: unknown) {
  Object.defineProperty(window.navigator, key, { value, configurable: true, writable: true })
  return () => { delete (window.navigator as unknown as Record<string, unknown>)[key] }
}

function sheet(overrides: Partial<React.ComponentProps<typeof ShareSheet>> = {}) {
  return (
    <ShareSheet
      open
      onClose={overrides.onClose ?? (() => undefined)}
      url={OPAQUE}
      qrHref={`${OPAQUE}#verify`}
      getCaption={async () => UNICODE_CAPTION}
      {...overrides}
    />
  )
}

afterEach(() => {
  delete (window.navigator as unknown as Record<string, unknown>).share
  delete (window.navigator as unknown as Record<string, unknown>).clipboard
})

describe('ShareSheet channels', () => {
  it('every channel shares the existing opaque URL — correctly encoded, never a minted one', async () => {
    render(sheet())
    // wait for the server caption (with Unicode) to replace the fallback
    await waitFor(() => expect(screen.getByText('X').closest('a')!.getAttribute('href')).toContain(encodeURIComponent(UNICODE_CAPTION)))

    const href = (label: string) => screen.getByText(label).closest('a')!.getAttribute('href')!
    expect(href('LinkedIn')).toBe(`https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(ABS)}`)
    expect(href('X')).toBe(`https://twitter.com/intent/post?text=${encodeURIComponent(UNICODE_CAPTION)}&url=${encodeURIComponent(ABS)}`)
    expect(href('Facebook')).toBe(`https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(ABS)}`)
    expect(href('WhatsApp')).toBe(`https://wa.me/?text=${encodeURIComponent(`${UNICODE_CAPTION} ${ABS}`)}`)
    expect(href('Telegram')).toBe(`https://t.me/share/url?url=${encodeURIComponent(ABS)}&text=${encodeURIComponent(UNICODE_CAPTION)}`)
    expect(href('Email')).toBe(`mailto:?subject=${encodeURIComponent('PCI World Passport')}&body=${encodeURIComponent(`${UNICODE_CAPTION}\n\n${ABS}`)}`)

    // Unicode survives round-trip decoding — no double encoding, no mangling.
    const xUrl = new URL(href('X'))
    expect(xUrl.searchParams.get('text')).toBe(UNICODE_CAPTION)
    expect(xUrl.searchParams.get('url')).toBe(ABS)
  })

  it('the disclosure line sits in the sheet and the QR links to the server-rendered code', async () => {
    render(sheet())
    await act(async () => {})
    expect(screen.getByText(DISCLOSURE_LINE)).toBeInTheDocument()
    expect(screen.getByText('View QR code').closest('a')!.getAttribute('href')).toBe(`${OPAQUE}#verify`)
  })

  it('capability honesty: url-share only — no direct-post button, no Instagram channel', async () => {
    render(sheet())
    await act(async () => {})
    expect(SHARE_CAPABILITIES.supported).toEqual(['url-share'])
    expect(SHARE_CAPABILITIES.unsupported).toEqual(['direct-posting', 'comment-sync'])
    // the footnote derives from the matrix and is rendered
    expect(screen.getByText(capabilityFootnote())).toBeInTheDocument()
    expect(capabilityFootnote()).toContain('never posts on your behalf')
    // no channel claims to post directly, and Instagram is deliberately absent
    expect(screen.queryByText(/post directly/i)).toBeNull()
    for (const c of CHANNELS) expect(c.href(ABS, 'x')).not.toContain('instagram')
    // Instagram users are pointed at the caption route instead, with a visible explanation
    expect(screen.getByText(/For Instagram/)).toHaveTextContent(/Copy caption/)
  })

  it('falls back to the neutral caption when the server caption cannot be fetched', async () => {
    render(sheet({ getCaption: async () => { throw new Error('offline') } }))
    const x = screen.getByText('X').closest('a')!
    expect(x.getAttribute('href')).toContain(encodeURIComponent(FALLBACK_CAPTION))
    await waitFor(() => expect(x.getAttribute('href')).toContain(encodeURIComponent(FALLBACK_CAPTION)))
  })
})

describe('ShareSheet dialog behaviour', () => {
  it('is a focus-trapped modal dialog: focus moves in, Tab wraps both ways, Esc closes', async () => {
    const onClose = vi.fn()
    render(sheet({ onClose }))
    await act(async () => {})
    const dialog = screen.getByRole('dialog')
    expect(dialog).toHaveAttribute('aria-modal', 'true')
    // initial focus lands inside the dialog
    await waitFor(() => expect(dialog.contains(document.activeElement)).toBe(true))

    const focusables = Array.from(dialog.querySelectorAll<HTMLElement>('button:not([disabled]), a[href], textarea, input, select'))
    const first = focusables[0]
    const last = focusables[focusables.length - 1]

    last.focus()
    fireEvent.keyDown(dialog, { key: 'Tab' })
    expect(document.activeElement).toBe(first)

    first.focus()
    fireEvent.keyDown(dialog, { key: 'Tab', shiftKey: true })
    expect(document.activeElement).toBe(last)

    fireEvent.keyDown(dialog, { key: 'Escape' })
    expect(onClose).toHaveBeenCalled()
  })

  it('renders nothing when closed', () => {
    render(sheet({ open: false }))
    expect(screen.queryByRole('dialog')).toBeNull()
  })
})

describe('ShareSheet native Web Share', () => {
  it('is absent when the platform has no navigator.share', async () => {
    render(sheet())
    await act(async () => {})
    expect(screen.queryByText('Share via device')).toBeNull()
  })

  it('hands off from the click, and a resolve is reported as a handoff — never a publication', async () => {
    const share = vi.fn(async () => undefined)
    defineOnNavigator('share', share)
    render(sheet())
    await act(async () => {})
    fireEvent.click(screen.getByText('Share via device'))
    await waitFor(() => expect(share).toHaveBeenCalledWith({
      title: 'PCI World Passport', text: UNICODE_CAPTION, url: ABS,
    }))
    const status = await screen.findByRole('status')
    expect(status.textContent).toMatch(/Handed to your device/)
    expect(status.textContent).toMatch(/nothing is posted until you finish there/i)
  })

  it('a dismissed native share stays silent', async () => {
    defineOnNavigator('share', vi.fn(async () => { throw new DOMException('cancelled', 'AbortError') }))
    render(sheet())
    await act(async () => {})
    fireEvent.click(screen.getByText('Share via device'))
    await waitFor(() => expect(screen.queryByRole('status')).toBeNull())
  })
})

describe('ShareSheet clipboard', () => {
  it('Copy link uses the clipboard when it is granted', async () => {
    const writeText = vi.fn(async () => undefined)
    defineOnNavigator('clipboard', { writeText })
    render(sheet())
    await act(async () => {})
    fireEvent.click(screen.getByText('Copy link'))
    await waitFor(() => expect(writeText).toHaveBeenCalledWith(ABS))
    expect((await screen.findByRole('status')).textContent).toBe('Link copied.')
  })

  it('a denied clipboard degrades to select-the-text with the exact value', async () => {
    defineOnNavigator('clipboard', { writeText: vi.fn(async () => { throw new DOMException('denied', 'NotAllowedError') }) })
    render(sheet())
    await act(async () => {})
    fireEvent.click(screen.getByText('Copy link'))
    const box = await screen.findByLabelText<HTMLTextAreaElement>(/Copying was blocked/)
    expect(box.value).toBe(ABS)
    expect(box).toHaveAttribute('readonly')
  })

  it('a missing clipboard API (no HTTPS) takes the same fallback for the caption', async () => {
    render(sheet())
    await act(async () => {})
    fireEvent.click(screen.getByText('Copy caption'))
    const box = await screen.findByLabelText<HTMLTextAreaElement>(/Copying was blocked/)
    // the fallback carries whatever caption the sheet currently holds
    expect([FALLBACK_CAPTION, UNICODE_CAPTION]).toContain(box.value)
  })
})
