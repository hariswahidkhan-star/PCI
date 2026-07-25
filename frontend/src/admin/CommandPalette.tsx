import { useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Icon, type IconName } from '../components/icons'

export interface PaletteItem {
  to: string
  label: string
  group: string
  icon: IconName
}

// Ctrl/Cmd+K quick navigator for the admin console. Purely client-side: it searches the
// sections the signed-in admin is already allowed to see (the same RBAC-filtered list that
// builds the sidebar), so it can never surface a route the sidebar would hide.
export default function CommandPalette({ items, open, onClose }: { items: PaletteItem[]; open: boolean; onClose: () => void }) {
  const navigate = useNavigate()
  const [q, setQ] = useState('')
  const [active, setActive] = useState(0)
  const inputRef = useRef<HTMLInputElement>(null)
  const listRef = useRef<HTMLDivElement>(null)

  const matches = useMemo(() => {
    const needle = q.trim().toLowerCase()
    if (!needle) return items
    return items.filter((i) => i.label.toLowerCase().includes(needle) || i.group.toLowerCase().includes(needle))
  }, [items, q])

  useEffect(() => {
    if (open) {
      setQ('')
      setActive(0)
      // autofocus after the panel mounts
      const id = window.setTimeout(() => inputRef.current?.focus(), 0)
      return () => window.clearTimeout(id)
    }
  }, [open])

  useEffect(() => setActive(0), [q])

  // keep the highlighted row in view while arrowing through a long result list
  useEffect(() => {
    listRef.current?.querySelector('[data-active="true"]')?.scrollIntoView?.({ block: 'nearest' })
  }, [active])

  if (!open) return null

  const go = (item: PaletteItem) => {
    onClose()
    navigate(item.to)
  }

  const onKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'ArrowDown') {
      e.preventDefault()
      setActive((a) => Math.min(a + 1, matches.length - 1))
    } else if (e.key === 'ArrowUp') {
      e.preventDefault()
      setActive((a) => Math.max(a - 1, 0))
    } else if (e.key === 'Enter') {
      e.preventDefault()
      if (matches[active]) go(matches[active])
    } else if (e.key === 'Escape') {
      e.preventDefault()
      onClose()
    }
  }

  return (
    <div className="cmdk-backdrop" onClick={onClose}>
      <div className="cmdk" role="dialog" aria-modal="true" aria-label="Go to section" onClick={(e) => e.stopPropagation()}>
        <div className="cmdk-input-row">
          <Icon name="search" size={17} />
          <input
            ref={inputRef}
            value={q}
            onChange={(e) => setQ(e.target.value)}
            onKeyDown={onKeyDown}
            placeholder="Go to section…"
            aria-label="Search sections"
            role="combobox"
            aria-expanded="true"
            aria-controls="cmdk-results"
          />
          <button className="btn ghost sm" onClick={onClose} aria-label="Close">
            <Icon name="x" size={16} />
          </button>
        </div>
        <div className="cmdk-list" id="cmdk-results" role="listbox" ref={listRef}>
          {matches.length === 0 && <div className="cmdk-empty">No section matches “{q}”.</div>}
          {matches.map((m, i) => (
            <button
              key={m.to}
              type="button"
              role="option"
              aria-selected={i === active}
              data-active={i === active || undefined}
              className={'cmdk-item' + (i === active ? ' active' : '')}
              onMouseEnter={() => setActive(i)}
              onClick={() => go(m)}
            >
              <Icon name={m.icon} size={16} className="nav-ic" />
              <span className="cmdk-label">{m.label}</span>
              <span className="cmdk-group">{m.group}</span>
            </button>
          ))}
        </div>
        <div className="cmdk-foot">
          <span><kbd>↑</kbd><kbd>↓</kbd> navigate</span>
          <span><kbd>↵</kbd> open</span>
          <span><kbd>esc</kbd> close</span>
        </div>
      </div>
    </div>
  )
}
