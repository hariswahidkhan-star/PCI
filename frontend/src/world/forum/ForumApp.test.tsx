import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import ForumApp from './ForumApp'

// PCI World forum — the composer surface's contract.
//
// Two properties matter more than the rest and are asserted first: a held post is never rendered
// as though it published — the person gets the server's own notice, in text — and a REFUSED post
// does not cost the author what they typed. The accessibility semantics are asserted as SEMANTICS
// (role, labels) rather than as class names, because a screen reader reads the former and ignores
// the latter.

const CATS = {
  categories: [
    { slug: 'general', title: 'General', description: 'Anything project controls', min_trust: 'new', read_only: false },
    { slug: 'announcements', title: 'Announcements', description: null, min_trust: 'staff', read_only: false },
    { slug: 'archive', title: 'Archive', description: null, min_trust: 'new', read_only: true },
  ],
}

const ME_NEW = {
  level: 'new', posts_are_reviewed_first: true, may_post_links: false,
  may_edit: false, may_flag: false, hourly_post_budget: 3,
}
const ME_MEMBER = {
  level: 'member', posts_are_reviewed_first: false, may_post_links: true,
  may_edit: true, may_flag: true, hourly_post_budget: 30,
}

describe('World forum composer', () => {
  const calls: { url: string; method: string; headers: Record<string, string>; body: unknown }[] = []
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    // The account session the composer must ride (the same key the classic World pages use).
    localStorage.setItem('pciworld_account', 'tok-account')
    responder = url => {
      if (url.includes('/forum/me')) return ME_NEW
      if (url.includes('/forum/categories')) return CATS
      return {}
    }
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const method = init?.method ?? 'GET'
      calls.push({
        url, method,
        headers: (init?.headers ?? {}) as Record<string, string>,
        body: init?.body ? JSON.parse(String(init.body)) : undefined,
      })
      const payload = responder(url, method)
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  async function openComposer(meFixture: unknown = ME_NEW) {
    responder = (url, method) => {
      if (url.includes('/forum/me')) return meFixture
      if (url.includes('/forum/categories') && method === 'GET') return CATS
      return {}
    }
    render(<ForumApp />)
    await screen.findByLabelText('Title')
  }

  function fillThread(title: string, body: string) {
    fireEvent.click(screen.getByRole('radio', { name: 'General' }))
    fireEvent.change(screen.getByLabelText('Title'), { target: { value: title } })
    fireEvent.change(screen.getByLabelText('Your post'), { target: { value: body } })
  }

  // ── Session and identity ──

  it('rides the World account header, never a portal bearer', async () => {
    await openComposer()
    const meCall = calls.find(c => c.url.includes('/forum/me'))
    expect(meCall?.headers['X-World-Account']).toBe('tok-account')
    expect(meCall?.headers['Authorization']).toBeUndefined()
  })

  it('tells a signed-out visitor to sign in, and offers no composer', async () => {
    responder = url => (url.includes('/forum/me') ? { _status: 401, error: 'no_session' } : CATS)
    render(<ForumApp />)
    await screen.findByText(/sign in to your PCI World account/i)
    // A composer whose every submit would 401 must not be rendered at all.
    expect(screen.queryByLabelText('Title')).toBeNull()
  })

  // ── The review-first rule, told up front ──

  it('tells a new member their posts are reviewed before others see them', async () => {
    await openComposer(ME_NEW)
    expect(screen.getByText(/reviewed by a moderator before other people can see them/i)).toBeTruthy()
  })

  it('does not warn a member whose posts publish immediately', async () => {
    await openComposer(ME_MEMBER)
    expect(screen.queryByText(/reviewed by a moderator before other people/i)).toBeNull()
  })

  // ── Category gating explains itself ──

  it('explains WHY a category cannot be posted to, in text', async () => {
    await openComposer(ME_NEW)
    // The trust floor: disabled, with the standing that would open it named.
    expect(screen.getByRole('radio', { name: 'Announcements' })).toBeDisabled()
    expect(screen.getByText(/needs staff standing to post; your account is new/i)).toBeTruthy()
    // Read-only is a different why and must say so, not share the trust wording.
    expect(screen.getByRole('radio', { name: 'Archive' })).toBeDisabled()
    expect(screen.getByText(/read only — new discussions cannot be started here/i)).toBeTruthy()
    // And an open category the person can use stays usable.
    expect(screen.getByRole('radio', { name: 'General' })).not.toBeDisabled()
  })

  // ── The safety rule: held is never shown as published ──

  it('reports a held thread with the server notice and no published echo', async () => {
    await openComposer(ME_NEW)
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') {
        return {
          ok: true, thread_id: 5, slug: 'how-we-forecast', url: '/world/forum/general/how-we-forecast',
          state: 'pending', published: false,
          notice: 'Your thread is waiting for a moderator. New accounts are reviewed before publication.',
        }
      }
      return url.includes('/forum/me') ? ME_NEW : CATS
    }
    fillThread('How we forecast escalation', 'A body of reasonable length.')
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))

    const status = await screen.findByRole('status')
    // The server's OWN words, verbatim — the interface must not soften "waiting for a moderator".
    expect(status.textContent).toContain('Your thread is waiting for a moderator.')
    // And no confirmation link: a held thread's page does not exist for anyone yet.
    expect(screen.queryByRole('link', { name: /view your discussion/i })).toBeNull()
  })

  it('links to the discussion only when it actually published', async () => {
    await openComposer(ME_MEMBER)
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') {
        return {
          ok: true, thread_id: 5, slug: 'how-we-forecast', url: '/world/forum/general/how-we-forecast',
          state: 'published', published: true, notice: null,
        }
      }
      return url.includes('/forum/me') ? ME_MEMBER : CATS
    }
    fillThread('How we forecast escalation', 'A body of reasonable length.')
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))

    const link = await screen.findByRole('link', { name: 'View your discussion' })
    expect(link.getAttribute('href')).toBe('/world/forum/general/how-we-forecast')
    // Published means done: the form clears so the next discussion starts clean.
    expect((screen.getByLabelText('Title') as HTMLInputElement).value).toBe('')
  })

  it('keeps the draft when the server refuses the post', async () => {
    await openComposer(ME_NEW)
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') return { _status: 400, error: 'links_not_allowed_yet' }
      return url.includes('/forum/me') ? ME_NEW : CATS
    }
    const body = screen.getByLabelText('Your post') as HTMLTextAreaElement
    fillThread('A perfectly good title', 'see https://example.com for details')
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toMatch(/cannot include links yet/i)
    // A refusal must not also cost the author what they wrote.
    expect(body.value).toBe('see https://example.com for details')
  })

  it('explains a rate limit in words a person can act on', async () => {
    await openComposer(ME_NEW)
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') return { _status: 429, error: 'rate_limited', retry_after_minutes: 60 }
      return url.includes('/forum/me') ? ME_NEW : CATS
    }
    fillThread('A perfectly good title', 'A body of reasonable length.')
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))
    const status = await screen.findByRole('status')
    expect(status.textContent).toMatch(/posting limit/i)
  })

  // ── Replies ──

  it('reports a held reply with the server notice and never echoes it as posted', async () => {
    responder = (url, method) => {
      if (url.includes('/threads/9/posts') && method === 'POST') {
        return {
          ok: true, post_id: 12, state: 'pending', published: false, reason: 'none',
          notice: 'Your post is waiting for a moderator. New accounts are reviewed before publication.',
        }
      }
      return url.includes('/forum/me') ? ME_NEW : {}
    }
    render(<ForumApp threadId={9} threadTitle="Escalation practice" />)
    const box = await screen.findByLabelText('Your reply') as HTMLTextAreaElement
    fireEvent.change(box, { target: { value: 'my careful reply' } })
    fireEvent.click(screen.getByRole('button', { name: 'Post reply' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toContain('Your post is waiting for a moderator.')
    // Nowhere on the page may the reply appear as content — that is the interface half of the
    // guarantee the backend enforces, and an optimistic echo would defeat a correct server.
    expect(screen.queryByText('my careful reply')).toBeNull()
  })

  it('confirms a published reply in words and clears the draft', async () => {
    responder = (url, method) => {
      if (url.includes('/threads/9/posts') && method === 'POST') {
        return { ok: true, post_id: 12, state: 'published', published: true, reason: 'ok', notice: null }
      }
      return url.includes('/forum/me') ? ME_MEMBER : {}
    }
    render(<ForumApp threadId={9} threadTitle="Escalation practice" />)
    const box = await screen.findByLabelText('Your reply') as HTMLTextAreaElement
    fireEvent.change(box, { target: { value: 'a normal reply' } })
    fireEvent.click(screen.getByRole('button', { name: 'Post reply' }))
    await waitFor(() => expect(box.value).toBe(''))
    expect(screen.getByRole('status').textContent).toMatch(/reply is published/i)
  })

  // ── Acting on existing posts ──

  it('edits through a new revision and reports a held edit distinctly', async () => {
    responder = (url, method) => {
      if (method === 'PATCH' && url.includes('/forum/posts/4')) {
        return { ok: true, state: 'pending', published: false, reason: 'none' }
      }
      return url.includes('/forum/me') ? ME_MEMBER : {}
    }
    render(<ForumApp threadId={9} posts={[{ id: 4, mine: true, body: 'original text' }]} />)
    fireEvent.click(await screen.findByRole('button', { name: 'Edit post 4' }))

    // The edit starts from what is actually there, not an empty box.
    const box = screen.getByLabelText('New text for post 4') as HTMLTextAreaElement
    expect(box.value).toBe('original text')
    fireEvent.change(box, { target: { value: 'better text' } })
    fireEvent.change(screen.getByLabelText('Reason for the edit (optional)'), { target: { value: 'typo' } })
    fireEvent.click(screen.getByRole('button', { name: 'Save edit' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toMatch(/waiting for a moderator/i)
    expect(status.textContent).toMatch(/not visible to others/i)
    const patchCall = calls.find(c => c.method === 'PATCH')
    expect(patchCall?.url).toBe('/api/world/forum/posts/4')
    expect(patchCall?.body).toEqual({ body: 'better text', edit_reason: 'typo' })
  })

  it('offers a new member neither edit nor flag, but still their own delete', async () => {
    responder = url => (url.includes('/forum/me') ? ME_NEW : {})
    render(<ForumApp threadId={9} posts={[
      { id: 4, mine: true, body: 'mine' },
      { id: 5, mine: false, body: '' },
    ]} />)
    await screen.findByLabelText('Your reply')
    // GET /me says may_edit / may_flag are false — a control that can only 403 is not offered.
    expect(screen.queryByRole('button', { name: 'Edit post 4' })).toBeNull()
    expect(screen.queryByRole('button', { name: 'Flag post 5' })).toBeNull()
    // Withdrawing your own words needs no earned standing.
    expect(screen.getByRole('button', { name: 'Delete post 4' })).toBeTruthy()
  })

  it('deletes only after stating the consequence, then relays the server notice', async () => {
    responder = (url, method) => {
      if (method === 'DELETE' && url.includes('/forum/posts/4')) {
        return { ok: true, notice: 'Your post is no longer visible. Its moderation record is kept while any report or appeal is open.' }
      }
      return url.includes('/forum/me') ? ME_MEMBER : {}
    }
    render(<ForumApp threadId={9} posts={[{ id: 4, mine: true, body: 'mine' }]} />)
    fireEvent.click(await screen.findByRole('button', { name: 'Delete post 4' }))

    // Two steps: the consequence — hiding, not erasing — is on screen before the commit.
    expect(screen.getByText(/history is kept while any report or appeal is open/i)).toBeTruthy()
    expect(calls.some(c => c.method === 'DELETE')).toBe(false)
    fireEvent.click(screen.getByRole('button', { name: 'Yes, delete it' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toContain('Its moderation record is kept')
    // The actions go with the post — offering "edit" on a hidden post would imply it is still there.
    expect(screen.getByText('Post 4 is no longer visible.')).toBeTruthy()
    expect(screen.queryByRole('button', { name: 'Edit post 4' })).toBeNull()
  })

  it('flags with a reason and relays the server acknowledgement', async () => {
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/flag')) {
        return { ok: true, notice: 'Thanks. A moderator will look at this.' }
      }
      return url.includes('/forum/me') ? ME_MEMBER : {}
    }
    render(<ForumApp threadId={9} posts={[{ id: 5, mine: false, body: '' }]} />)
    fireEvent.click(await screen.findByRole('button', { name: 'Flag post 5' }))
    fireEvent.change(screen.getByLabelText('Why should a moderator look at post 5?'), { target: { value: 'spam' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send flag' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toContain('A moderator will look at this.')
    const flagCall = calls.find(c => c.url.includes('/flag'))
    expect(flagCall?.url).toBe('/api/world/forum/posts/5/flag')
    expect(flagCall?.body).toEqual({ reason: 'spam' })
    // Your own post carries no flag control at all — the server would refuse it anyway.
    expect(screen.queryByRole('button', { name: 'Flag post 4' })).toBeNull()
  })

  // ── Accessibility semantics ──

  it('announces outcomes via status, never alert', async () => {
    await openComposer(ME_NEW)
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') {
        return { ok: true, thread_id: 5, slug: 't', url: '/world/forum/general/t', state: 'pending', published: false, notice: 'Held.' }
      }
      return url.includes('/forum/me') ? ME_NEW : CATS
    }
    fillThread('A perfectly good title', 'A body.')
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))
    // An outcome is a result, not an emergency: "status" lets a screen reader finish its sentence,
    // where "alert" would interrupt it.
    await screen.findByRole('status')
    expect(screen.queryByRole('alert')).toBeNull()
  })
})
