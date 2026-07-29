import { describe, expect, it } from 'vitest'
import { fireEvent, render, screen, waitFor, within } from '@testing-library/react'
import ShareConsole, { ANALYTICS_PLACEHOLDER, NOT_ENABLED_TITLE, PROVIDER_DISABLED_NOTE } from './ShareConsole'
import { PLACEHOLDER_WHITELIST, renderTemplate, unknownPlaceholders } from './template'
import { createShareAdminSeam, isFixturesMode, ShareAdminNotEnabledError, type ShareAdminSeam } from './seam'
import { HOSTILE_SAMPLE_NAME } from './fixtures'
import { SHARE_CAPABILITIES, capabilityFootnote } from '../world/ShareSheet'

// The §10.2 console's contract: the bounded placeholder whitelist escapes by construction (a
// hostile display name is always inert text), revocation demands a reason before it confirms,
// the capability panel can only say what the participant sheet's matrix says, channel ordering
// is accessible up/down (no drag), and a missing backend is a DESIGNED state, not a crash.

function fixturesConsole() {
  const seam = createShareAdminSeam({ fixtures: true })
  return { seam, ...render(<ShareConsole seam={seam} />) }
}

describe('template whitelist', () => {
  const values = { display_name: 'Zoë', challenge_title: 'Bridge Programme Rescue' }

  it('is bounded to exactly the two documented placeholders', () => {
    expect(PLACEHOLDER_WHITELIST).toEqual(['display_name', 'challenge_title'])
  })

  it('substitutes whitelisted placeholders and leaves every other token verbatim', () => {
    expect(renderTemplate('{display_name} finished {challenge_title}!', values))
      .toBe('Zoë finished Bridge Programme Rescue!')
    expect(renderTemplate('{email} {display_name} {secret_token}', values))
      .toBe('{email} Zoë {secret_token}')
    expect(unknownPlaceholders('{email} {display_name} {secret_token} {email}'))
      .toEqual(['email', 'secret_token'])
  })

  it('returns plain text even for hostile values — no interpretation happens here', () => {
    const out = renderTemplate('{display_name}', { ...values, display_name: HOSTILE_SAMPLE_NAME })
    expect(out).toBe(HOSTILE_SAMPLE_NAME) // verbatim string; the DOM layer renders it as text
  })
})

describe('template editor preview — hostile-name escaping proven in the DOM', () => {
  it('renders the hostile sample name as literal text, never as elements', async () => {
    const { container } = fixturesConsole()
    await screen.findByText('Caption, title & hashtag templates')

    // the preview declares its own adversarial posture
    expect(screen.getByText(/rendered with a hostile sample name to prove escaping/)).toBeInTheDocument()
    // the hostile name appears as text (title + caption previews at least)…
    const hits = screen.getAllByText((t) => t.includes(HOSTILE_SAMPLE_NAME))
    expect(hits.length).toBeGreaterThan(0)
    // …and nothing it "wrote" became markup anywhere in the console (the moderation table
    // also carries a hostile owner name from the fixtures)
    expect(container.querySelector('img')).toBeNull()
    expect(container.querySelector('script')).toBeNull()
  })

  it('shows the whitelist as chips and warns on non-whitelisted tokens', async () => {
    fixturesConsole()
    await screen.findByText('Caption, title & hashtag templates')
    for (const p of PLACEHOLDER_WHITELIST) {
      expect(within(screen.getByLabelText('Available placeholders')).getByText(`{${p}}`)).toBeInTheDocument()
    }
    fireEvent.change(screen.getByLabelText('Caption template'), { target: { value: 'Hi {email} and {display_name}' } })
    expect(screen.getByText(/Not on the whitelist/)).toHaveTextContent('{email}')
    // the unknown token is NOT substituted in the preview — it stays as typed
    expect(screen.getAllByText((t) => t.includes('Hi {email} and')).length).toBeGreaterThan(0)
  })
})

describe('template versioning', () => {
  it('draft → publish, and roll back republishes a superseded version', async () => {
    fixturesConsole()
    await screen.findByText('Caption, title & hashtag templates')

    // en fixtures: v2 published, v1 superseded → roll back offered on v1
    fireEvent.click(await screen.findByText('Roll back to this'))
    await screen.findByText('Rolled back to v1.')

    // draft path: save a draft, then publish it
    fireEvent.change(screen.getByLabelText('Caption template'), { target: { value: 'New {display_name} caption' } })
    fireEvent.click(screen.getByText('Save draft'))
    await screen.findByText('Draft saved.')
    fireEvent.click(await screen.findByText('Publish'))
    await screen.findByText(/published\.$/)
  })
})

describe('share-link moderation', () => {
  it('revoke demands a reason: confirm is disabled until one is typed, then records it', async () => {
    fixturesConsole()
    await screen.findByText('Share-link moderation')

    const revokeButtons = screen.getAllByText('Revoke')
    expect(revokeButtons.length).toBe(2) // fixture rows 11 and 12 are live; 13 already revoked
    fireEvent.click(revokeButtons[0])

    const confirm = screen.getByText('Confirm revoke')
    expect(confirm).toBeDisabled()
    fireEvent.change(screen.getByLabelText(/Reason \(required/), { target: { value: '   ' } })
    expect(screen.getByText('Confirm revoke')).toBeDisabled()   // whitespace is not a reason
    fireEvent.change(screen.getByLabelText(/Reason \(required/), { target: { value: 'Employer takedown request.' } })
    expect(screen.getByText('Confirm revoke')).toBeEnabled()
    fireEvent.click(screen.getByText('Confirm revoke'))

    await screen.findByText(/revoked — Employer takedown request\./)
    expect(screen.getAllByText('Revoke').length).toBe(1)        // one live row left
  })

  it('cancel closes the confirm without revoking', async () => {
    fixturesConsole()
    await screen.findByText('Share-link moderation')
    fireEvent.click(screen.getAllByText('Revoke')[0])
    fireEvent.click(screen.getByText('Cancel'))
    expect(screen.queryByText('Confirm revoke')).toBeNull()
    expect(screen.getAllByText('Revoke').length).toBe(2)
  })
})

describe('capability honesty — mirrors the participant ShareSheet matrix', () => {
  it('url-share supported; direct posting and comment sync disabled pending provider approval', async () => {
    fixturesConsole()
    await screen.findByText('Provider capabilities')

    expect(SHARE_CAPABILITIES.supported).toEqual(['url-share'])
    expect(screen.getByText(/URL share/)).toBeInTheDocument()
    expect(screen.getByText('supported')).toBeInTheDocument()

    // both unsupported capabilities carry the exact disabled note
    expect(screen.getAllByText(PROVIDER_DISABLED_NOTE).length).toBe(SHARE_CAPABILITIES.unsupported.length)
    expect(screen.getByText('Direct posting from PCI')).toBeInTheDocument()
    expect(screen.getByText(/Comment \/ reply sync/)).toBeInTheDocument()

    // the same footnote the participant sheet derives — imported, not restated
    expect(screen.getByText(capabilityFootnote())).toBeInTheDocument()
    // and no button anywhere pretends to post
    expect(screen.queryByText(/post directly/i)).toBeNull()
    expect(screen.queryByRole('button', { name: /post/i })).toBeNull()
  })
})

describe('channel management', () => {
  it('reorders with accessible up/down buttons and toggles enable/disable', async () => {
    fixturesConsole()
    await screen.findByText('Share channels')

    const names = () => Array.from(document.querySelectorAll('.wa-channel-name')).map(el => el.textContent)
    expect(names()).toEqual(['LinkedIn', 'X', 'Facebook', 'WhatsApp', 'Telegram', 'Email'])
    expect(screen.getByLabelText('Move LinkedIn up')).toBeDisabled()      // already first
    expect(screen.getByLabelText('Move Email down')).toBeDisabled()       // already last

    fireEvent.click(screen.getByLabelText('Move X up'))
    await waitFor(() => expect(names()).toEqual(['X', 'LinkedIn', 'Facebook', 'WhatsApp', 'Telegram', 'Email']))

    // fixture state: facebook starts disabled → enable it
    fireEvent.click(screen.getByLabelText('Enable Facebook'))
    await screen.findByLabelText('Disable Facebook')
  })
})

describe('missing backend — designed state, not a crash', () => {
  const notEnabledSeam: ShareAdminSeam = {
    mode: 'live',
    channels: async () => { throw new ShareAdminNotEnabledError() },
    setChannelEnabled: async () => { throw new ShareAdminNotEnabledError() },
    moveChannel: async () => { throw new ShareAdminNotEnabledError() },
    templates: async () => { throw new ShareAdminNotEnabledError() },
    saveDraft: async () => { throw new ShareAdminNotEnabledError() },
    publishTemplate: async () => { throw new ShareAdminNotEnabledError() },
    rollbackTemplate: async () => { throw new ShareAdminNotEnabledError() },
    shareLinks: async () => { throw new ShareAdminNotEnabledError() },
    revokeShareLink: async () => { throw new ShareAdminNotEnabledError() },
  }

  it('a 404 backend renders the designed not-yet-enabled card, capabilities still shown honestly', async () => {
    render(<ShareConsole seam={notEnabledSeam} />)
    await screen.findByText(NOT_ENABLED_TITLE)
    expect(screen.getByText(/the console in front of you is finished, the service behind it is not/i)).toBeInTheDocument()
    expect(screen.getByText(/\?preview=1/)).toBeInTheDocument()
    // the honesty panel needs no backend — it still renders
    expect(screen.getByText('Provider capabilities')).toBeInTheDocument()
    // and no fake management UI appears
    expect(screen.queryByText('Share channels')).toBeNull()
    expect(screen.queryByText('Share-link moderation')).toBeNull()
  })

  it('?preview=1 selects fixtures mode; anything else stays live', () => {
    expect(isFixturesMode('?preview=1')).toBe(true)
    expect(isFixturesMode('?preview=0')).toBe(false)
    expect(isFixturesMode('')).toBe(false)
    expect(isFixturesMode('?other=1')).toBe(false)
  })

  it('fixtures mode announces itself so sample data is never mistaken for real settings', async () => {
    fixturesConsole()
    await screen.findByText(/Preview mode — sample data only/)
  })
})

describe('analytics placeholder', () => {
  it('is labelled as requiring a backend that is not yet enabled — no fake numbers', async () => {
    const { container } = fixturesConsole()
    await screen.findByText('Share analytics')
    expect(screen.getByText(ANALYTICS_PLACEHOLDER)).toBeInTheDocument()
    expect(ANALYTICS_PLACEHOLDER).toBe('requires backend analytics — not yet enabled')
    expect(container.querySelector('canvas')).toBeNull()
    expect(screen.queryByText(/\d+ clicks/)).toBeNull()
  })
})
