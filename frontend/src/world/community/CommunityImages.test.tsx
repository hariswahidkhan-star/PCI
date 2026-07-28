import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import CommunityApp from './CommunityApp'

// PCI World community images — the guest surface's half of the Phase 2 contract.
//
// The property that matters most, and is asserted first: choosing a file must NEVER put the picture
// into the room. A local preview would be the image equivalent of an optimistic echo — the uploader
// would see their own photo sitting in the transcript while the server had not yet decided, and
// would reasonably conclude everyone else could see it too. What they must get instead is an
// explicit "not visible yet", in words rather than by styling.

const ROOM = {
  slug: 'gallery', title: 'Gallery', description: 'Images', topic: null,
  category: null, locale: 'en', room_type: 'community', state: 'open',
  capacity: 50, guests_welcome: true, slow_mode_seconds: 0, participants: 2,
  accepting: true, images_allowed: true, rules_version: 'v1',
  pinned_welcome: null, retention_class: 'standard',
}

const TEXT_ONLY_ROOM = { ...ROOM, images_allowed: false }

// A 1x1 PNG. `File` in jsdom is enough for FileReader.readAsDataURL, which is all the composer uses.
function pngFile(name = 'chart.png') {
  const bytes = Uint8Array.from(atob(
    'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=='
  ), c => c.charCodeAt(0))
  return new File([bytes], name, { type: 'image/png' })
}

describe('World community images', () => {
  const calls: { url: string; method: string; body: unknown }[] = []
  let responder: (url: string, method: string) => unknown
  let room = ROOM
  // The transcript the fake server returns. Mutated BEFORE entering the room, because the first
  // poll fires immediately on entry and the next is 2.5s later — swapping the responder afterwards
  // would make each of these tests a race against a timer rather than a test of the render.
  let transcript: unknown[] = []

  beforeEach(() => {
    calls.length = 0
    room = ROOM
    transcript = []
    sessionStorage.clear()
    responder = url => {
      if (url.includes('/messages')) return { messages: [] }
      if (url.includes('/community/rooms/gallery')) return room
      return { rooms: [room] }
    }
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const method = init?.method ?? 'GET'
      calls.push({ url, method, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      const payload = responder(url, method)
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  async function enterRoom() {
    render(<CommunityApp />)
    await screen.findByText(room.title)
    fireEvent.click(screen.getByRole('button', { name: new RegExp(`Enter ${room.title}`) }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ayesha Iqbal' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = url => {
      if (url.includes('guest-sessions')) return { ok: true, token: 'tok-1', display_name: 'Ayesha Iqbal' }
      if (url.includes('/messages')) return { messages: transcript }
      if (url.includes('/community/rooms/gallery')) return room
      return { rooms: [room] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))
    await screen.findByLabelText('Your message')
  }

  // ── The safety rule, in the interface ──

  it('does not put the chosen image into the room', async () => {
    await enterRoom()
    fireEvent.change(screen.getByLabelText('Image file'), { target: { files: [pngFile()] } })
    fireEvent.change(screen.getByLabelText('Describe the image'), { target: { value: 'A Gantt chart' } })

    // Before any send, and therefore before any verdict, there is nothing to look at.
    expect(screen.queryByRole('img')).toBeNull()
  })

  it('says the image is not visible yet, in words', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('/images')) return { ok: true, media_id: 7, state: 'pending', reason: 'queued_for_review', notice: null }
      if (url.includes('/messages')) return { messages: [] }
      return room
    }
    fireEvent.change(screen.getByLabelText('Image file'), { target: { files: [pngFile()] } })
    fireEvent.change(screen.getByLabelText('Describe the image'), { target: { value: 'A Gantt chart' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send image' }))

    // role="status", so the outcome is announced without interrupting — and the text says plainly
    // that the room cannot see it. "Uploaded" would read as "posted".
    const status = await screen.findByText(/not visible to the room yet/i)
    expect(status).toBeTruthy()
    expect(screen.queryByRole('img')).toBeNull()
  })

  it('refuses to send without a description', async () => {
    // Enforced here as well as on the server, so the person finds out before waiting for an upload
    // rather than after. An image nobody can describe excludes every blind participant.
    await enterRoom()
    fireEvent.change(screen.getByLabelText('Image file'), { target: { files: [pngFile()] } })
    expect(screen.getByRole('button', { name: 'Send image' })).toBeDisabled()

    fireEvent.change(screen.getByLabelText('Describe the image'), { target: { value: '  ' } })
    expect(screen.getByRole('button', { name: 'Send image' })).toBeDisabled()

    fireEvent.change(screen.getByLabelText('Describe the image'), { target: { value: 'A Gantt chart' } })
    expect(screen.getByRole('button', { name: 'Send image' })).not.toBeDisabled()
  })

  it('reports a refusal in the participant\'s own words rather than a reason code', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('/images')) return { _status: 400, ok: false, media_id: null, state: 'rejected', reason: 'svg_not_accepted', notice: null }
      if (url.includes('/messages')) return { messages: [] }
      return room
    }
    fireEvent.change(screen.getByLabelText('Image file'), { target: { files: [pngFile('logo.svg')] } })
    fireEvent.change(screen.getByLabelText('Describe the image'), { target: { value: 'A logo' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send image' }))

    await waitFor(() => expect(screen.getByRole('status')).toBeTruthy())
    expect(screen.queryByText('svg_not_accepted')).toBeNull()
  })

  // ── Rendering a published image ──

  it('renders a published image with the description as its alternative text', async () => {
    transcript = [{ sequence: 1, body: 'A Gantt chart', author: 'Ayesha Iqbal', reply_to: null, at: null, kind: 'image', media_id: 7 }]
    await enterRoom()
    const img = await screen.findByRole('img')
    expect(img.getAttribute('alt')).toBe('A Gantt chart')
    expect(img.getAttribute('src')).toContain('/api/world/community/media/7')
  })

  it('still shows the description as text, so the message is not empty if the image fails', async () => {
    transcript = [{ sequence: 1, body: 'A Gantt chart', author: 'Ayesha Iqbal', reply_to: null, at: null, kind: 'image', media_id: 7 }]
    await enterRoom()
    await screen.findByRole('img')
    expect(screen.getByText('A Gantt chart')).toBeTruthy()
  })

  it('renders a text message from a server that has never heard of images', async () => {
    // `kind` is absent on an older server. Defaulting to an image would blank the transcript.
    transcript = [{ sequence: 1, body: 'Morning all', author: 'Omar Farooq', reply_to: null, at: null }]
    await enterRoom()
    expect(await screen.findByText('Morning all')).toBeTruthy()
    expect(screen.queryByRole('img')).toBeNull()
  })

  // ── The room gate ──

  it('offers no image control in a room that does not allow images', async () => {
    // Rendering a disabled control in every room would advertise a feature most rooms lack.
    room = TEXT_ONLY_ROOM
    await enterRoom()
    expect(screen.queryByLabelText('Image file')).toBeNull()
    expect(screen.getByLabelText('Your message')).toBeTruthy()
  })
})
