// Duplicate-scan debounce for the gate scanner (§7A.8). A QR held in front of the camera fires
// the same token many times a second; only the first read within the window may reach the seam.
// The window is SLIDING: every rejected duplicate refreshes the timer, so a pass held up
// continuously never re-fires until it is taken away for a full window.

export interface ScanDebouncer {
  /** True when this token should be processed; false when it is a rapid duplicate. */
  accept(token: string, now?: number): boolean
  /** Forget the last token (e.g. when the operator changes gate). */
  reset(): void
}

export const DEFAULT_SCAN_DEBOUNCE_MS = 3000

export function makeScanDebouncer(windowMs: number = DEFAULT_SCAN_DEBOUNCE_MS): ScanDebouncer {
  let lastToken: string | null = null
  let lastAt = 0
  return {
    accept(token: string, now: number = Date.now()): boolean {
      if (lastToken === token && now - lastAt < windowMs) {
        lastAt = now // sliding window — keep suppressing while the same code keeps arriving
        return false
      }
      lastToken = token
      lastAt = now
      return true
    },
    reset() {
      lastToken = null
      lastAt = 0
    },
  }
}
