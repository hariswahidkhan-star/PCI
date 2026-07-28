import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import PassportCard from './PassportCard'
import PassportStatus from './PassportStatus'
import PassportEmptyState from './PassportEmptyState'
import PassportSkeleton from './PassportSkeleton'
import type { PassportSummary } from '../../api/types'

// The shared Passport family's presentational contract: one PassportSummary in, honest labels
// out. What matters: disclosure-shaped highlights (absent fields stay absent), the hash-only
// public-link rule (published without canShowPublicUrl never renders a link), and accessible
// naming on every state.

function summary(over: Partial<PassportSummary> = {}): PassportSummary {
  return {
    schemaVersion: 1,
    studentNumber: 'PCI-2026-000042',
    displayName: 'Jordan Q.',
    participantState: 'active',
    passportState: 'private',
    completedChallengeCount: 7,
    selectedEvidenceCount: 3,
    evidenceHighlights: [],
    disclosureSettings: { scores: false, profiles: false, dates: false },
    canShowPublicUrl: false,
    expiresAt: null,
    lastUpdatedAt: null,
    availableActions: ['manage', 'publish'],
    ...over,
  }
}

describe('PassportCard', () => {
  it('renders the identity band: name, Student Number (labelled), and status', () => {
    render(<PassportCard summary={summary()} />)
    expect(screen.getByRole('region', { name: 'PCI World Passport' })).toBeInTheDocument()
    expect(screen.getByText('Jordan Q.')).toBeInTheDocument()
    expect(screen.getByLabelText('PCI Student Number PCI-2026-000042')).toHaveTextContent('PCI-2026-000042')
    expect(screen.getByText('Private — ready to publish')).toBeInTheDocument()
    expect(screen.getByText('7')).toBeInTheDocument()   // completed
    expect(screen.getByText('3')).toBeInTheDocument()   // selected
  })

  it('renders highlights with disclosed fields only — switched-off fields stay absent', () => {
    render(
      <PassportCard
        summary={summary({
          evidenceHighlights: [
            { title: 'Earned value under pressure', reference: 'WC-EVM-001 · v1', difficulty: 'professional', score: 82.5, completedOn: '2026-06-01' },
            { title: 'Titles only', reference: 'WC-RSK-002 · v1' },
          ],
        })}
      />,
    )
    const list = screen.getByRole('list', { name: 'Selected evidence highlights' })
    expect(list).toHaveTextContent('Earned value under pressure')
    expect(list).toHaveTextContent('score 82.5')
    expect(list).toHaveTextContent('2026-06-01')
    expect(list).toHaveTextContent('Titles only')
    // exactly one score chip — the undisclosed item gained nothing
    expect(screen.getAllByText(/score /)).toHaveLength(1)
  })

  it('published WITHOUT the raw link renders guidance, never a reconstructed URL', () => {
    render(<PassportCard summary={summary({ passportState: 'published', canShowPublicUrl: false })} />)
    expect(screen.getByText(/Open it in PCI World to view or share/)).toBeInTheDocument()
    expect(screen.queryByRole('link')).toBeNull()
  })

  it('published WITH a known raw link renders it', () => {
    render(
      <PassportCard
        summary={summary({ passportState: 'published', canShowPublicUrl: true, publicUrl: '/world/p/tok123' })}
      />,
    )
    expect(screen.getByRole('link', { name: '/world/p/tok123' })).toHaveAttribute('href', '/world/p/tok123')
  })

  it('a suspended state explains itself without touching the student account', () => {
    render(<PassportCard summary={summary({ passportState: 'suspended' })} />)
    expect(screen.getByText(/participation is suspended/)).toBeInTheDocument()
    expect(screen.getByText(/student account is unaffected/)).toBeInTheDocument()
  })

  it('renders the host-supplied actions and footer', () => {
    render(<PassportCard summary={summary()} actions={<button>Publish my Passport</button>} footer={<p role="alert">boom</p>} />)
    expect(screen.getByRole('button', { name: 'Publish my Passport' })).toBeInTheDocument()
    expect(screen.getByRole('alert')).toHaveTextContent('boom')
  })
})

describe('PassportStatus', () => {
  it.each([
    ['not_created', 'Not created yet'],
    ['draft', 'Private draft'],
    ['private', 'Private — ready to publish'],
    ['published', 'Published'],
    ['expired', 'Public link expired'],
    ['suspended', 'Suspended'],
  ] as const)('labels %s as "%s" in text, never colour alone', (state, label) => {
    render(<PassportStatus state={state} />)
    const badge = screen.getByText(label)
    expect(badge).toHaveAttribute('data-state', state)
  })
})

describe('PassportEmptyState', () => {
  it('create variant renders onboarding copy and the host action', () => {
    render(<PassportEmptyState variant="create" actions={<button>Create my Passport</button>} />)
    expect(screen.getByText('Start your PCI World Passport')).toBeInTheDocument()
    expect(screen.getByText(/never a certification claim/)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Create my Passport' })).toBeInTheDocument()
  })

  it('unavailable variant reassures that the rest of MyPCI is unaffected', () => {
    render(<PassportEmptyState variant="unavailable" />)
    expect(screen.getByText('Passport temporarily unavailable')).toBeInTheDocument()
    expect(screen.getByText(/Everything else in MyPCI is unaffected/)).toBeInTheDocument()
  })

  it('link variant explains the pending link without claiming a merge happened', () => {
    render(<PassportEmptyState variant="link" />)
    expect(screen.getByText('A PCI World account is waiting to be linked')).toBeInTheDocument()
    expect(screen.getByText(/nothing merges until you do/)).toBeInTheDocument()
  })
})

describe('PassportSkeleton', () => {
  it('announces itself as busy while loading', () => {
    render(<PassportSkeleton />)
    const region = screen.getByLabelText('Loading your PCI World Passport')
    expect(region).toHaveAttribute('aria-busy', 'true')
  })
})
