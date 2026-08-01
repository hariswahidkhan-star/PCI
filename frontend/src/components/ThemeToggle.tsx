import { useEffect, useState } from 'react'
import { applyTheme, effectiveTheme, readTheme, type ThemeChoice } from '../theme'

const ORDER: ThemeChoice[] = ['system', 'light', 'dark']

const LABEL: Record<ThemeChoice, string> = {
  system: 'Theme: follows your device',
  light: 'Theme: light',
  dark: 'Theme: dark',
}

/** Sun, moon, or a half-filled disc for "follow the device" — the third state needs its own mark,
 *  because showing a sun while the device is dark would describe the setting and not the result. */
function Glyph({ choice }: { choice: ThemeChoice }) {
  if (choice === 'light') {
    return (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
           strokeLinecap="round" aria-hidden="true">
        <circle cx="12" cy="12" r="4.2" />
        <path d="M12 2.6v2.2M12 19.2v2.2M2.6 12h2.2M19.2 12h2.2M5.4 5.4l1.6 1.6M17 17l1.6 1.6M18.6 5.4L17 7M7 17l-1.6 1.6" />
      </svg>
    )
  }
  if (choice === 'dark') {
    return (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
           strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
        <path d="M20.5 14.3A8.6 8.6 0 1 1 9.7 3.5a6.9 6.9 0 0 0 10.8 10.8Z" />
      </svg>
    )
  }
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" aria-hidden="true">
      <circle cx="12" cy="12" r="8.4" />
      <path d="M12 3.6v16.8A8.4 8.4 0 0 0 12 3.6Z" fill="currentColor" stroke="none" />
    </svg>
  )
}

export default function ThemeToggle() {
  const [choice, setChoice] = useState<ThemeChoice>(() => readTheme())

  // When following the device, re-render as the device changes so the control and the page agree.
  useEffect(() => {
    if (choice !== 'system') return
    const mq = window.matchMedia?.('(prefers-color-scheme: dark)')
    if (!mq) return
    const onChange = () => setChoice('system')
    mq.addEventListener('change', onChange)
    return () => mq.removeEventListener('change', onChange)
  }, [choice])

  function cycle() {
    const next = ORDER[(ORDER.indexOf(choice) + 1) % ORDER.length]
    applyTheme(next)
    setChoice(next)
  }

  const showing = effectiveTheme(choice)
  return (
    <button
      type="button"
      className="theme-toggle"
      onClick={cycle}
      aria-label={`${LABEL[choice]}. Currently showing ${showing}. Activate to change.`}
      title={LABEL[choice]}
    >
      <Glyph choice={choice} />
    </button>
  )
}
