import { describe, it, expect, vi, afterEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { DocumentRow, ViewDownloadActions } from './DocumentActions'

// FE — the universal document row + View/Download pair and the secure viewer's core states.
// Rendered WITHOUT an I18nProvider on purpose: the components must also work in the admin SPA,
// which has no provider (useTSafe falls back to the English catalog).

afterEach(() => vi.unstubAllGlobals())

const pdfResponse = () => new Response('%PDF-1.7 fake', { status: 200, headers: { 'Content-Type': 'application/pdf' } })

describe('DocumentRow', () => {
  it('renders icon chip, title, meta and description', () => {
    render(
      <DocumentRow
        info={{ title: 'Fee policy', description: 'How fees work', filename: 'fees.pdf', mime: 'application/pdf', sizeBytes: 2048, meta: ['policy'] }}
        actions={<button>Download</button>}
      />,
    )
    expect(screen.getByText('PDF')).toBeInTheDocument()
    expect(screen.getByText('Fee policy')).toBeInTheDocument()
    expect(screen.getByText(/2\.0 KB · policy/)).toBeInTheDocument()
    expect(screen.getByText('How fees work')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download' })).toBeInTheDocument()
  })

  it('shows the version badge only past v1', () => {
    const { rerender } = render(<DocumentRow info={{ title: 'A', version: 1 }} actions={null} />)
    expect(screen.queryByText(/Version/)).toBeNull()
    rerender(<DocumentRow info={{ title: 'A', version: 4 }} actions={null} />)
    expect(screen.getByText('Version 4')).toBeInTheDocument()
  })
})

describe('ViewDownloadActions', () => {
  it('renders both actions by default, and hides what is not permitted', () => {
    const { rerender } = render(
      <ViewDownloadActions info={{ title: 'Doc' }} inlineUrl="/api/x?inline=1" downloadUrl="/api/x" />,
    )
    expect(screen.getByRole('button', { name: 'View' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download' })).toBeInTheDocument()
    rerender(<ViewDownloadActions info={{ title: 'Doc' }} inlineUrl="/api/x?inline=1" canDownload={false} />)
    expect(screen.getByRole('button', { name: 'View' })).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Download' })).toBeNull()
    rerender(<ViewDownloadActions info={{ title: 'Doc' }} inlineUrl="/api/x?inline=1" canView={false} />)
    expect(screen.queryByRole('button', { name: 'View' })).toBeNull()
    expect(screen.getByRole('button', { name: 'Download' })).toBeInTheDocument()
  })

  it('opens the viewer dialog on View, fetching the inline URL with the token', async () => {
    const user = userEvent.setup()
    const fetchMock = vi.fn(async () => pdfResponse())
    vi.stubGlobal('fetch', fetchMock)
    vi.stubGlobal('URL', Object.assign(URL, { createObjectURL: vi.fn(() => 'blob:x'), revokeObjectURL: vi.fn() }))
    render(
      <ViewDownloadActions info={{ title: 'Handbook', mime: 'application/pdf' }} inlineUrl="/api/doc?inline=1" downloadUrl="/api/doc" token="tok" />,
    )
    await user.click(screen.getByRole('button', { name: 'View' }))
    expect(await screen.findByRole('dialog', { name: 'Handbook' })).toBeInTheDocument()
    expect(fetchMock).toHaveBeenCalledWith('/api/doc?inline=1', { headers: { Authorization: 'Bearer tok' } })
    // Esc closes and focus handling does not crash
    await user.keyboard('{Escape}')
    await waitFor(() => expect(screen.queryByRole('dialog')).toBeNull())
  })

  it('shows an inline error (never an alert) when the download fails', async () => {
    const user = userEvent.setup()
    vi.stubGlobal('fetch', vi.fn(async () => new Response(JSON.stringify({ error: 'file_missing' }), { status: 404 })))
    render(<ViewDownloadActions info={{ title: 'Doc' }} inlineUrl="/api/x?inline=1" downloadUrl="/api/x" />)
    await user.click(screen.getByRole('button', { name: 'Download' }))
    expect(await screen.findByRole('alert')).toHaveTextContent(/Could not download/)
  })

  it('renders a CSV as a safe table preview inside the viewer', async () => {
    const user = userEvent.setup()
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response('col1,col2\n<b>x</b>,2', { status: 200, headers: { 'Content-Type': 'text/csv' } })))
    render(<ViewDownloadActions info={{ title: 'Data', mime: 'text/csv', filename: 'data.csv' }} inlineUrl="/api/csv?inline=1" />)
    await user.click(screen.getByRole('button', { name: 'View' }))
    expect(await screen.findByRole('columnheader', { name: 'col1' })).toBeInTheDocument()
    // Rendered as TEXT, not injected markup — the tag is visible verbatim.
    expect(screen.getByText('<b>x</b>')).toBeInTheDocument()
  })

  it('falls back honestly for unsupported types, keeping download available', async () => {
    const user = userEvent.setup()
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response('PK...', { status: 200, headers: { 'Content-Type': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' } })))
    render(<ViewDownloadActions info={{ title: 'Spec', filename: 'spec.docx' }} inlineUrl="/api/docx?inline=1" />)
    await user.click(screen.getByRole('button', { name: 'View' }))
    expect(await screen.findByText(/preview is not available/i)).toBeInTheDocument()
    const dialog = screen.getByRole('dialog')
    expect(dialog).toHaveTextContent('Download')
  })

  it('surfaces a denial from the serve endpoint as the viewer error state', async () => {
    const user = userEvent.setup()
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response(JSON.stringify({ error: 'not_assigned', message: 'You do not have access to this document.' }), { status: 403 })))
    render(<ViewDownloadActions info={{ title: 'Restricted' }} inlineUrl="/api/no?inline=1" />)
    await user.click(screen.getByRole('button', { name: 'View' }))
    expect(await screen.findByRole('alert')).toHaveTextContent(/Could not open/i)
  })
})
