import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, within, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — the PCI World launch board.
//
// This screen's failure modes are not crashes. Every one of them is a screen that looks fine and
// says something untrue: a switch that appears usable when the server will refuse it, a feature
// reported live when its flag never moved, a server refusal replaced by "Something went wrong", or
// a prerequisite that can be recorded with an empty box. Each is pinned below by driving the real
// controls rather than by asserting on props.
//
// The one rule this component must never break: it does not decide. `can_enable` and every refusal
// come from the server, and the tests here check the component RELAYS them — including the case
// where a POST is refused despite the switch having looked available.

const h = vi.hoisted(() => ({
  data: null as unknown,
  loading: false,
  error: null as string | null,
  refetch: vi.fn(),
  post: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: () => ({ data: h.data, loading: h.loading, error: h.error, refetch: h.refetch }),
}))
// Delegate rather than pass `h.post` directly: the factory runs once, so a captured reference
// would freeze the mock as it was at module-eval time and silently ignore every per-test
// reassignment below — the component would call a stub returning undefined and the failures would
// look like component bugs.
vi.mock('../api', () => ({ adminApi: { post: (...a: unknown[]) => h.post(...a) } }))

import WorldLaunch from './WorldLaunch'

const forum = {
  flag: 'pciworld_forum_enabled',
  title: 'Professional forum',
  turns_on: 'The public forum at /world/forum.',
  url: '/world/forum',
  when_off: 'That address returns 404 — the forum is a server-rendered route and does not exist while this is off.',
  on: false,
  advisory: 'Forum posts are public and indexed.',
  depends_on: null,
  depends_on_met: true,
  depends_on_title: null,
  requires: null,
  can_enable: true,
}

const images = {
  flag: 'pciworld_community_images_enabled',
  title: 'Images in community rooms',
  turns_on: 'Lets members and guests attach images to room messages.',
  url: '/world-app/community',
  when_off: 'Rooms work as usual; the composer offers no attachment control and the upload endpoint answers 404.',
  on: false,
  advisory: null,
  depends_on: 'world_community_enabled',
  depends_on_met: true,
  depends_on_title: 'Community rooms',
  requires: {
    key: 'pciworld_ack_image_moderation',
    label: 'Image moderation provider is contracted',
    detail: 'No general classifier detects every illegal image.',
    recorded: null,
  },
  can_enable: false,
}

const card = (title: string) => screen.getByText(title).closest('.card') as HTMLElement

describe('PCI World launch board', () => {
  beforeEach(() => {
    h.data = { rows: [forum, images] }
    h.loading = false
    h.error = null
    h.refetch = vi.fn()
    h.post = vi.fn().mockResolvedValue({ on: true })
  })

  it('says where a feature will be served, and what that address does until then', () => {
    render(<WorldLaunch />)
    // Per-feature, not a blanket "404": /world/forum really does not exist while off, whereas the
    // React shell at /world-app/* answers 200 and gates itself. Saying 404 for both would make a
    // working deployment look broken.
    expect(within(card('Professional forum')).getByText(/does not exist while this is off/)).toBeInTheDocument()
    expect(within(card('Images in community rooms')).getByText(/Rooms work as usual/)).toBeInTheDocument()
  })

  it('switches a feature on and reports it live', async () => {
    render(<WorldLaunch />)
    await userEvent.click(within(card('Professional forum')).getByRole('button', { name: 'Switch on' }))
    expect(h.post).toHaveBeenCalledWith('/api/admin/world-launch', { flag: 'pciworld_forum_enabled', on: true })
    await waitFor(() => expect(screen.getByRole('status')).toHaveTextContent('Professional forum is live.'))
    // The board must re-read rather than trust its own optimistic guess about the new state.
    expect(h.refetch).toHaveBeenCalled()
  })

  it('will not offer to switch on a feature the server says cannot be enabled', () => {
    render(<WorldLaunch />)
    expect(within(card('Images in community rooms')).getByRole('button', { name: 'Switch on' })).toBeDisabled()
  })

  it('shows the server refusal verbatim when a POST is rejected anyway', async () => {
    // The switch being disabled is a courtesy. If the state changed under us — or the request came
    // from anywhere else — the server still refuses, and that refusal is written to be read.
    h.data = { rows: [{ ...images, can_enable: true }] }
    h.post = vi.fn().mockRejectedValue(new Error('prerequisite_not_recorded'))
    render(<WorldLaunch />)
    await userEvent.click(within(card('Images in community rooms')).getByRole('button', { name: 'Switch on' }))
    await waitFor(() =>
      expect(within(card('Images in community rooms')).getByText('prerequisite_not_recorded')).toBeInTheDocument())
  })

  it('will not record an empty or one-word prerequisite', async () => {
    render(<WorldLaunch />)
    const c = card('Images in community rooms')
    const record = within(c).getByRole('button', { name: 'Record it' })
    expect(record).toBeDisabled()
    await userEvent.type(within(c).getByLabelText(/What was done/), 'yes')
    expect(record).toBeDisabled()
    expect(h.post).not.toHaveBeenCalled()
  })

  it('records a substantive prerequisite and re-reads the board', async () => {
    render(<WorldLaunch />)
    const c = card('Images in community rooms')
    await userEvent.type(within(c).getByLabelText(/What was done/), 'Contract MOD-2026-114 with Hive.')
    await userEvent.click(within(c).getByRole('button', { name: 'Record it' }))
    expect(h.post).toHaveBeenCalledWith('/api/admin/world-launch/ack', {
      key: 'pciworld_ack_image_moderation',
      note: 'Contract MOD-2026-114 with Hive.',
    })
    await waitFor(() => expect(h.refetch).toHaveBeenCalled())
  })

  it('shows who recorded a prerequisite, when, and what they said', () => {
    h.data = {
      rows: [{
        ...images,
        can_enable: true,
        requires: {
          ...images.requires,
          recorded: { by: 'owner@pci.local', at: '2026-07-30 09:00:00', note: 'Contract MOD-2026-114.' },
        },
      }],
    }
    render(<WorldLaunch />)
    const c = card('Images in community rooms')
    expect(within(c).getByText(/owner@pci.local/)).toBeInTheDocument()
    expect(within(c).getByText(/2026-07-30 09:00:00/)).toBeInTheDocument()
    expect(within(c).getByText(/Contract MOD-2026-114\./)).toBeInTheDocument()
    expect(within(c).getByRole('button', { name: 'Switch on' })).toBeEnabled()
  })

  it('explains a dependency instead of just disabling the switch', () => {
    h.data = { rows: [{ ...images, depends_on_met: false, can_enable: false }] }
    render(<WorldLaunch />)
    expect(within(card('Images in community rooms')).getByText(/Community rooms/)).toBeInTheDocument()
  })

  it('warns when switching a feature off leaves another one inert', async () => {
    h.data = { rows: [{ ...forum, on: true }] }
    h.post = vi.fn().mockResolvedValue({ on: false, orphaned: ['Images in community rooms'] })
    render(<WorldLaunch />)
    await userEvent.click(within(card('Professional forum')).getByRole('button', { name: 'Switch off' }))
    await waitFor(() =>
      expect(screen.getByRole('status')).toHaveTextContent(/Images in community rooms is still switched on/))
  })

  it('surfaces a 403 rather than rendering an empty board', () => {
    h.error = 'owner_only'
    render(<WorldLaunch />)
    expect(screen.getByText('owner_only')).toBeInTheDocument()
  })
})
