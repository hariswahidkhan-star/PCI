// Registers @testing-library/jest-dom matchers (toBeInTheDocument, toHaveTextContent, …) on
// Vitest's global expect, and clears the DOM between tests.
import '@testing-library/jest-dom/vitest'
import { afterEach, beforeEach } from 'vitest'
import { cleanup } from '@testing-library/react'
import { noteSuccess, setDemo } from '../demo/mode'

afterEach(() => cleanup())

/* Every test in this suite stubs a WORKING backend and asserts what the app does with its
 * answers — including its error answers. Demonstration mode's automatic fallback exists for the
 * opposite premise (a deployment whose backend has never once replied), and if it were live here
 * it would quietly substitute sample data for the failure a test is trying to observe: the
 * "a network failure offers retry" assertions would see a cheerful fabricated success instead.
 *
 * So the harness states its premise once, here: the backend is reachable, demo mode is off.
 * The fallback's own behaviour is tested in src/demo/fallback.test.ts, which resets the module
 * graph so it can start from the untouched state the rule is actually about.
 */
beforeEach(() => {
  setDemo(false)
  noteSuccess()
})
