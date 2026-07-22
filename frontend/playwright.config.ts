import { defineConfig, devices } from '@playwright/test'

// Browser end-to-end + accessibility (axe) over the public site, which the ASP.NET backend serves
// directly. Playwright boots the built backend DLL as its webServer and waits for /api/health.
//
// Local note: this sandbox blocks a local server bind, so these specs are exercised on CI runners
// (which boot the backend exactly as the `backend` CI job does). The CI `e2e` job is non-blocking
// (continue-on-error) until it has a green run history — mirroring how the repo already treats the
// optional S3/moto step — so an unproven browser run can never turn the pipeline red.
const PORT = process.env.E2E_PORT || '8080'
const BASE = process.env.E2E_BASE_URL || `http://127.0.0.1:${PORT}`

// When a Chromium is preinstalled (this sandbox: /opt/pw-browsers), point at it; on CI the browser
// is fetched with `playwright install chromium` and this is unset.
const executablePath = process.env.PW_CHROMIUM_PATH || undefined

export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: process.env.CI ? [['github'], ['list']] : [['list']],
  timeout: 30_000,
  use: {
    baseURL: BASE,
    trace: 'on-first-retry',
    ...(executablePath ? { launchOptions: { executablePath } } : {}),
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'], ...(executablePath ? { launchOptions: { executablePath } } : {}) } },
  ],
  // Boot the built backend DLL and wait for health. Skipped when E2E_NO_SERVER is set (e.g. a run
  // against an already-running server) or when only listing tests.
  webServer: process.env.E2E_NO_SERVER
    ? undefined
    : {
        command: 'dotnet bin/Release/net8.0/PCI.Backend.dll',
        cwd: '../backend',
        url: `http://127.0.0.1:${PORT}/api/health`,
        env: { PORT, DATABASE_FILE: './e2e_ci.db', ASPNETCORE_ENVIRONMENT: 'Development' },
        reuseExistingServer: !process.env.CI,
        timeout: 120_000,
      },
})
