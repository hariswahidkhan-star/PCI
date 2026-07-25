import { describe, it, expect } from 'vitest'
import { escapeHtml } from './print'

// Security — `escapeHtml` is the client-side escaper that guards the printable receipt/certificate
// (openPrintable builds an HTML document from data the app holds — the member's name, email and
// product all flow through it). If it didn't escape, a crafted profile value could inject markup or
// script into the generated print tab. This pins the escaping the receipt relies on.
describe('escapeHtml (printable receipt/certificate escaper)', () => {
  it('escapes the HTML metacharacters that could break out of the markup', () => {
    expect(escapeHtml('<script>alert(1)</script>')).toBe('&lt;script&gt;alert(1)&lt;/script&gt;')
    expect(escapeHtml('a & b')).toBe('a &amp; b')
    expect(escapeHtml('say "hi"')).toBe('say &quot;hi&quot;')
  })

  it('neutralises a crafted name so it cannot inject into the receipt table', () => {
    const out = escapeHtml('</td></tr><script>steal()</script>')
    expect(out).not.toContain('<script')
    expect(out).not.toContain('</td>')
    expect(out).toContain('&lt;')
  })

  it('escapes & first, so a literal entity is not mistaken for a produced one', () => {
    // A literal "&lt;" in the data becomes "&amp;lt;" — the ampersand is escaped exactly once.
    expect(escapeHtml('&lt;')).toBe('&amp;lt;')
  })

  it('coerces null / undefined to an empty string', () => {
    expect(escapeHtml(null)).toBe('')
    expect(escapeHtml(undefined)).toBe('')
  })

  it('leaves ordinary text untouched', () => {
    expect(escapeHtml('Sam Rivera')).toBe('Sam Rivera')
  })
})
