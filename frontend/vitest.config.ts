import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'

// Component/unit tests for the React SPA. Kept separate from vite.config.ts (which is the
// student build) and vite.admin.config.ts (the admin build) so the app builds are untouched.
// jsdom gives the DOM APIs React Testing Library needs; globals:true exposes describe/it/expect
// without per-file imports; the setup file registers the jest-dom matchers.
export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: ['./src/test/setup.ts'],
    include: ['src/**/*.{test,spec}.{ts,tsx}'],
    css: false,
    restoreMocks: true,
  },
})
