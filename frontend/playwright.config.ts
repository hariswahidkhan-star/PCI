import { defineConfig, devices } from '@playwright/test'
import { E2E_ADMIN, E2E_CREDENTIAL_KEY, E2E_STORAGE_ROOT } from './e2e/util'

// Browser end-to-end + accessibility (axe) over the public site, which the ASP.NET backend serves
// directly. Playwright boots the built backend DLL as its webServer and waits for /api/health.
//
// The CI `e2e` job is GATING: every spec here must be deterministic (auto-waiting locators and
// expect() polls only — no fixed sleeps) and independent, so the suite passes repeatedly against
// the same reused dev database as well as a fresh CI one.
const PORT = process.env.E2E_PORT || '8080'
const BASE = process.env.E2E_BASE_URL || `http://127.0.0.1:${PORT}`
const requestedProvider = (process.env.E2E_DB_PROVIDER || process.env.DB_PROVIDER || 'sqlite').toLowerCase()
if (!['sqlite', 'mysql', 'mariadb'].includes(requestedProvider)) {
  throw new Error(`Unknown E2E_DB_PROVIDER '${requestedProvider}'`)
}
const mysqlParity = requestedProvider === 'mysql' || requestedProvider === 'mariadb'

// When Chromium is preinstalled, point only Chromium-based projects at it. Firefox/WebKit retain
// their own engines; CI installs all three browser families.
const executablePath = process.env.PW_CHROMIUM_PATH || undefined


// The database the suite drives the browser against. A SQLite file by default — no service to stand
// up, which is what keeps the ordinary `e2e` job cheap. `DB_PROVIDER=mysql` points the SAME specs at
// a real MariaDB, because that is what production runs: every layer below this one (the xUnit suite,
// the live-HTTP integration suite, the migration-integrity checks) is already exercised on both, and
// the browser journeys were the last that never had been.
const database =
  mysqlParity
    ? {
        DB_PROVIDER: 'mysql',
        MYSQL_HOST: process.env.MYSQL_HOST || '127.0.0.1',
        MYSQL_PORT: process.env.MYSQL_PORT || '3306',
        MYSQL_USER: process.env.MYSQL_USER || 'pci',
        MYSQL_PASSWORD: process.env.MYSQL_PASSWORD || '',
        MYSQL_DATABASE: process.env.MYSQL_DATABASE || 'pci',
        MYSQL_SSL: process.env.MYSQL_SSL || 'false',
      }
    : { DATABASE_FILE: './e2e_ci.db' }

export default defineConfig({
  testDir: './e2e',
  // Stages the built React SPAs into backend/wwwroot when absent (fresh CI checkout) — the
  // portal/admin specs need the backend to actually serve /app/ and /admin/. No-op locally.
  globalSetup: './e2e/global-setup.ts',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  workers: process.env.CI || mysqlParity ? 1 : undefined,
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
        // The store is emptied as part of starting the server, for the same reason the database
        // starts empty: the two are one state. It is content-addressed and skips writing a blob
        // whose hash is already on disk, so a store that outlives its database hands the next run
        // bytes no row accounts for. This belongs in the COMMAND, not in globalSetup (which runs
        // after the server has already seeded) and not at config load (which every worker re-runs).
        command: `node -e "require('fs').rmSync('${E2E_STORAGE_ROOT}',{recursive:true,force:true})" `
          + '&& dotnet bin/Release/net8.0/PCI.Backend.dll',
        cwd: '../backend',
        url: `http://127.0.0.1:${PORT}/api/health`,
        env: {
          PORT,
          ...database,
          // Pinned, not derived: the fallback key is built from configuration that DIFFERS between
          // the two legs (DATABASE_FILE vs MYSQL_DATABASE), so it would silently change with the
          // provider and leave a surviving object store unreadable.
          CREDENTIAL_ENCRYPTION_KEY: E2E_CREDENTIAL_KEY,
          STORAGE_ROOT: E2E_STORAGE_ROOT,
          ASPNETCORE_ENVIRONMENT: 'Development',
          STRIPE_SECRET_KEY: 'sk_test_e2e_browser_suite',
          STRIPE_WEBHOOK_SECRET: 'whsec_e2e_browser_suite',
          // The world-admin owner password, so the community a11y spec can enable the feature
          // through the real operator API rather than reaching into the database.
          PCIWORLD_OWNER_PASSWORD: process.env.PCIWORLD_OWNER_PASSWORD ?? 'E2EWorldOwner!99',
          E2E_ADMIN_EMAIL: E2E_ADMIN.email,
          E2E_ADMIN_PASSWORD: E2E_ADMIN.password,
          SEED_DEMO_EXAM: 'true',
          E2E_EXAM_OPEN_BEFORE_MINUTES: '100000',
        },
        reuseExistingServer: !process.env.CI && !mysqlParity,
        timeout: 120_000,
      },
})
