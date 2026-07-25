import { useId, useRef, useState, type ReactNode } from 'react'
import { fmtBytes } from '../../files'
import { useTSafe } from '../../i18n'

// Shared upload picker: drag-and-drop + file picker + client-side size validation + chosen-file
// echo with a remove action. Client checks are UX only — every backend endpoint re-validates
// type, size, magic bytes and the malware seam.

interface Props {
  label: ReactNode
  accept?: string
  maxBytes: number
  file: File | null
  onPick: (f: File | null) => void
  hint?: ReactNode
  disabled?: boolean
}

export default function FileUploadField({ label, accept, maxBytes, file, onPick, hint, disabled }: Props) {
  const t = useTSafe()
  const id = useId()
  const inputRef = useRef<HTMLInputElement>(null)
  const [err, setErr] = useState<string | null>(null)
  const [over, setOver] = useState(false)

  const pick = (f: File | null) => {
    setErr(null)
    if (f && f.size > maxBytes) {
      setErr(t('doc.fileTooLarge', { size: fmtBytes(maxBytes) ?? `${maxBytes} B` }))
      if (inputRef.current) inputRef.current.value = ''
      onPick(null)
      return
    }
    onPick(f)
  }

  return (
    <div style={{ display: 'grid', gap: '.3rem' }}>
      <label htmlFor={id}>{label} {hint && <span className="muted small">{hint}</span>}</label>
      <div
        className={'upload-drop' + (over ? ' over' : '') + (disabled ? ' disabled' : '')}
        onDragOver={(e) => { e.preventDefault(); if (!disabled) setOver(true) }}
        onDragLeave={() => setOver(false)}
        onDrop={(e) => {
          e.preventDefault(); setOver(false)
          if (disabled) return
          pick(e.dataTransfer.files?.[0] ?? null)
        }}
      >
        <input
          ref={inputRef}
          id={id}
          type="file"
          accept={accept}
          disabled={disabled}
          onChange={(e) => pick(e.target.files?.[0] ?? null)}
          style={{ width: 'auto' }}
        />
        <span className="muted small">{t('doc.dropHere')} · {t('doc.maxSize', { size: fmtBytes(maxBytes) ?? '' })}</span>
      </div>
      {file && (
        <span className="muted small">
          {t('doc.attached', { name: `${file.name} (${fmtBytes(file.size) ?? ''})` })}{' '}
          <button
            type="button"
            className="btn ghost sm"
            disabled={disabled}
            onClick={() => { if (inputRef.current) inputRef.current.value = ''; pick(null) }}
          >
            {t('doc.remove')}
          </button>
        </span>
      )}
      {err && <span className="small" role="alert" style={{ color: 'var(--err, #c2410c)' }}>{err}</span>}
    </div>
  )
}
