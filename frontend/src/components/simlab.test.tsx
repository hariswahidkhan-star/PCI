import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import {
  LabHeader, KpiRow, SegmentedControl, Chip, SaveIndicator, InsightCallout,
  SkeletonCards, LevelDots, ScoreSummary, ConfirmDialog,
} from './simlab'

// FE — the Simulation Lab shared primitives (premium UI layer). Pins the accessibility
// contracts: segmented control = real radios, chips expose aria-pressed, the save
// indicator is a polite status region, the confirm dialog is a labelled modal that
// cancels on Escape/backdrop, and skeletons are announced as a loading status.

describe('SimLab primitives', () => {
  it('LabHeader renders eyebrow, title and lede', () => {
    render(<LabHeader eyebrow="Simulation Lab" title="Practice" lede="One orientation sentence." />)
    expect(screen.getByRole('heading', { name: 'Practice' })).toBeInTheDocument()
    expect(screen.getByText('Simulation Lab')).toBeInTheDocument()
    expect(screen.getByText('One orientation sentence.')).toBeInTheDocument()
  })

  it('KpiRow shows tabular metrics and hides when empty', () => {
    const { container, rerender } = render(<KpiRow items={[{ k: 'Labs', v: 12 }, { k: 'Completed', v: 3 }]} />)
    expect(screen.getByText('Labs')).toBeInTheDocument()
    expect(screen.getByText('12')).toBeInTheDocument()
    rerender(<KpiRow items={[]} />)
    expect(container.querySelector('.sl-kpis')).toBeNull()
  })

  it('SegmentedControl is a radio group and reports changes', () => {
    const onChange = vi.fn()
    render(
      <SegmentedControl
        legend="Mode"
        value="training"
        onChange={onChange}
        options={[{ value: 'training', label: 'Training' }, { value: 'assessment', label: 'Assessment' }]}
      />,
    )
    const training = screen.getByRole('radio', { name: 'Training' })
    const assessment = screen.getByRole('radio', { name: 'Assessment' })
    expect(training).toBeChecked()
    expect(assessment).not.toBeChecked()
    fireEvent.click(assessment)
    expect(onChange).toHaveBeenCalledWith('assessment')
  })

  it('Chip exposes its pressed state', () => {
    const onClick = vi.fn()
    const { rerender } = render(<Chip pressed={false} onClick={onClick}>Construction</Chip>)
    const chip = screen.getByRole('button', { name: 'Construction' })
    expect(chip).toHaveAttribute('aria-pressed', 'false')
    fireEvent.click(chip)
    expect(onClick).toHaveBeenCalled()
    rerender(<Chip pressed onClick={onClick}>Construction</Chip>)
    expect(chip).toHaveAttribute('aria-pressed', 'true')
  })

  it('SaveIndicator announces each save state politely', () => {
    const { rerender } = render(<SaveIndicator state="saving" />)
    expect(screen.getByRole('status')).toHaveTextContent('Saving…')
    rerender(<SaveIndicator state="saved" />)
    expect(screen.getByRole('status')).toHaveTextContent('Saved')
    rerender(<SaveIndicator state="saved" note="Resumed your saved answers" />)
    expect(screen.getByRole('status')).toHaveTextContent('Resumed your saved answers')
  })

  it('InsightCallout renders tone and title', () => {
    render(<InsightCallout tone="warning" title="Attention">Check the float.</InsightCallout>)
    expect(screen.getByText('Attention')).toBeInTheDocument()
    expect(screen.getByText('Check the float.')).toBeInTheDocument()
  })

  it('SkeletonCards is announced as a loading status', () => {
    render(<SkeletonCards count={3} />)
    expect(screen.getByRole('status', { name: 'Loading the catalogue' })).toBeInTheDocument()
  })

  it('LevelDots describes the hint progression', () => {
    render(<LevelDots level={2} max={5} />)
    expect(screen.getByRole('img', { name: 'Hint level 2 of 5' })).toBeInTheDocument()
  })

  it('ScoreSummary leads with the outcome', () => {
    render(<ScoreSummary score={80} heading="Result — 80%" sub="4 of 5 correct" />)
    expect(screen.getByText('Result — 80%')).toBeInTheDocument()
    expect(screen.getByText('4 of 5 correct')).toBeInTheDocument()
  })

  it('ConfirmDialog confirms, cancels on Escape, and is a labelled modal', () => {
    const onConfirm = vi.fn()
    const onCancel = vi.fn()
    render(
      <ConfirmDialog
        open
        title="Switch to Assessment Mode?"
        body="Switching mode starts a fresh attempt."
        confirmLabel="Switch mode"
        onConfirm={onConfirm}
        onCancel={onCancel}
      />,
    )
    const dialog = screen.getByRole('dialog', { name: 'Switch to Assessment Mode?' })
    expect(dialog).toHaveAttribute('aria-modal', 'true')
    // Focus lands on the safe (cancel) action first.
    expect(screen.getByRole('button', { name: 'Cancel' })).toHaveFocus()
    fireEvent.click(screen.getByRole('button', { name: 'Switch mode' }))
    expect(onConfirm).toHaveBeenCalled()
    fireEvent.keyDown(dialog, { key: 'Escape' })
    expect(onCancel).toHaveBeenCalled()
  })

  it('ConfirmDialog renders nothing when closed', () => {
    const { container } = render(
      <ConfirmDialog open={false} title="t" body="b" confirmLabel="c" onConfirm={() => {}} onCancel={() => {}} />,
    )
    expect(container.firstChild).toBeNull()
  })
})
