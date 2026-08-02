import { describe, it, expect } from 'vitest'
import { CATALOG } from './catalog'
import { LANGS } from './index'

// The multilingual launch guarantee for the exam journey: every string on the exam-day check-in
// (xd.*), the secure runner (xr.*) and the results page (rr.*) must exist in ALL supported
// languages — a missing language would silently fall back to English mid-exam.
const PREFIXES = ['xd.', 'xr.', 'rr.']

describe('exam-journey catalog coverage', () => {
  const keys = Object.keys(CATALOG).filter((k) => PREFIXES.some((p) => k.startsWith(p)))

  it('has the exam-journey keys at all', () => {
    expect(keys.length).toBeGreaterThan(80)
  })

  for (const lang of LANGS.map((l) => l.code)) {
    it(`every exam-journey string exists in ${lang}`, () => {
      const missing = keys.filter((k) => {
        const v = (CATALOG[k] as Record<string, string | undefined>)[lang]
        return !v || v.trim().length === 0
      })
      expect(missing).toEqual([])
    })
  }

  it('placeholders survive every translation (a {var} lost in translation renders broken text)', () => {
    for (const k of keys) {
      const entry = CATALOG[k] as Record<string, string | undefined>
      const vars = [...(entry.en ?? '').matchAll(/\{(\w+)\}/g)].map((m) => m[1])
      for (const lang of LANGS.map((l) => l.code)) {
        const v = entry[lang]
        if (!v) continue
        for (const varName of vars) {
          expect(v.includes(`{${varName}}`), `${k} [${lang}] lost {${varName}}`).toBe(true)
        }
      }
    }
  })
})
