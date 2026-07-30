import { describe, it, expect } from 'vitest'
import { makeScanDebouncer } from './debounce'

// The duplicate-scan debounce is safety logic at the gate: a QR held to the camera fires the same
// token many times a second and must reach the decision seam exactly once, while the window slides
// so a continuously-presented pass never re-fires.

describe('makeScanDebouncer', () => {
  it('accepts the first read of a token', () => {
    const d = makeScanDebouncer(3000)
    expect(d.accept('EVT-1', 1000)).toBe(true)
  })

  it('rejects the same token inside the window', () => {
    const d = makeScanDebouncer(3000)
    expect(d.accept('EVT-1', 1000)).toBe(true)
    expect(d.accept('EVT-1', 1500)).toBe(false)
    expect(d.accept('EVT-1', 3900)).toBe(false) // window slid to 1500 on the rejection
  })

  it('slides the window on every duplicate, so a held-up pass never re-fires', () => {
    const d = makeScanDebouncer(1000)
    expect(d.accept('EVT-1', 0)).toBe(true)
    for (let t = 300; t <= 6000; t += 300) {
      expect(d.accept('EVT-1', t)).toBe(false)
    }
  })

  it('accepts the same token again once the window has fully elapsed', () => {
    const d = makeScanDebouncer(1000)
    expect(d.accept('EVT-1', 0)).toBe(true)
    expect(d.accept('EVT-1', 1500)).toBe(true)
  })

  it('accepts a different token immediately', () => {
    const d = makeScanDebouncer(3000)
    expect(d.accept('EVT-1', 1000)).toBe(true)
    expect(d.accept('EVT-2', 1001)).toBe(true)
  })

  it('reset forgets the last token', () => {
    const d = makeScanDebouncer(3000)
    expect(d.accept('EVT-1', 1000)).toBe(true)
    d.reset()
    expect(d.accept('EVT-1', 1001)).toBe(true)
  })
})
