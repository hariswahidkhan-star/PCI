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
    coverage: {
      provider: 'v8',
      reporter: ['text', 'html', 'lcov', 'cobertura'],
      reportsDirectory: './coverage',
      include: ['src/**/*.{ts,tsx}'],
      exclude: ['src/**/*.{test,spec}.{ts,tsx}', 'src/test/**', 'src/main.tsx', 'src/admin/main.tsx'],
      // Risk-based floors for the logic that is already covered. Deliberately avoid a cosmetic global
      // percentage while the principal page/component tests are still being added.
      thresholds: {
        'src/format.ts': { statements: 90, branches: 80, functions: 100, lines: 90 },
        'src/api/client.ts': { statements: 80, branches: 60, functions: 80, lines: 80 },
        'src/api/checkout.ts': { statements: 40, branches: 90, functions: 50, lines: 40 },
        'src/components/RequireAuth.tsx': { statements: 100, branches: 100, functions: 100, lines: 100 },
        'src/data/wizardFields.ts': { statements: 95, branches: 80, functions: 100, lines: 95 },
      },
    },
  },
})
