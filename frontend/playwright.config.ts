import { defineConfig, devices } from '@playwright/test'

// Browser end-to-end + accessibility (axe) over the public site, which the ASP.NET backend serves
// directly. Playwright boots the built backend DLL as its webServer and waits for /api/health.
//
// The CI `e2e` job is GATING: every spec here must be deterministic (auto-waiting locators and
// expect() polls only — no fixed sleeps) and independent, so the suite passes repeatedly against
// the same reused dev database as well as a fresh CI one.
const PORT = process.env.E2E_PORT || '8080'
const BASE = process.env.E2E_BASE_URL || `http://127.0.0.1:${PORT}`

// When Chromium is preinstalled, point only Chromium-based projects at it. Firefox/WebKit retain
// their own engines; CI installs all three browser families.
const executablePath = process.env.PW_CHROMIUM_PATH || undefined

export default defineConfig({
  testDir: './e2e',
  // Stages the built React SPAs into backend/wwwroot when absent (fresh CI checkout) — the
  // portal/admin specs need the backend to actually serve /app/ and /admin/. No-op locally.
  globalSetup: './e2e/global-setup.ts',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: process.env.CI ? [['github'], ['list'], ['html', { open: 'never' }]] : [['list'], ['html', { open: 'never' }]],
  timeout: 30_000,
  use: {
    baseURL: BASE,
    trace: 'retain-on-failure',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'], ...(executablePath ? { launchOptions: { executablePath } } : {}) } },
    { name: 'firefox-smoke', grep: /@cross-browser/, use: { ...devices['Desktop Firefox'] } },
    { name: 'webkit-smoke', grep: /@cross-browser/, use: { ...devices['Desktop Safari'] } },
    { name: 'mobile-chrome-smoke', grep: /@cross-browser/, use: { ...devices['Pixel 7'], ...(executablePath ? { launchOptions: { executablePath } } : {}) } },
    { name: 'mobile-safari-smoke', grep: /@cross-browser/, use: { ...devices['iPhone 15'] } },
  ],
  // Boot the built backend DLL and wait for health. Skipped when E2E_NO_SERVER is set (e.g. a run
  // against an already-running server) or when only listing tests.
  webServer: process.env.E2E_NO_SERVER
    ? undefined
    : {
        command: 'dotnet bin/Release/net8.0/PCI.Backend.dll',
        cwd: '../backend',
        url: `http://127.0.0.1:${PORT}/api/health`,
        env: {
          PORT,
          DATABASE_FILE: './e2e_ci.db',
          ASPNETCORE_ENVIRONMENT: 'Development',
          STRIPE_SECRET_KEY: 'sk_test_e2e_browser_suite',
          STRIPE_WEBHOOK_SECRET: 'whsec_e2e_browser_suite',
          SEED_DEMO_EXAM: 'true',
          E2E_EXAM_OPEN_BEFORE_MINUTES: '100000',
        },
        reuseExistingServer: !process.env.CI,
        timeout: 120_000,
      },
})
