import { execSync } from 'node:child_process'
import fs from 'node:fs'
import path from 'node:path'

// The portal/admin specs exercise the React SPAs the backend serves from wwwroot/app and
// wwwroot/admin. Those directories are build outputs (gitignored — assembled by the Dockerfile in
// production), so on a fresh CI checkout they do not exist yet. This setup makes the suite
// self-sufficient: when either shell is missing, build the frontend once and drop the bundles in,
// exactly as the Dockerfile does (dist → wwwroot/app, dist-admin → wwwroot/admin with admin.html
// copied to index.html). Locally the shells are normally already present, so this is a no-op.
// The backend reads the shells from disk per request, so boot order vs the webServer is irrelevant.
export default async function globalSetup(): Promise<void> {
  const frontendRoot = process.cwd() // `npm run e2e` always runs from frontend/ (locally and on CI)
  const wwwroot = path.resolve(frontendRoot, '..', 'backend', 'wwwroot')
  const appIndex = path.join(wwwroot, 'app', 'index.html')
  const adminIndex = path.join(wwwroot, 'admin', 'index.html')
  if (!fs.existsSync(appIndex) || !fs.existsSync(adminIndex)) {
    console.log('[e2e setup] built SPAs missing from backend/wwwroot — building the frontend once…')
    execSync('npm run build', { cwd: frontendRoot, stdio: 'inherit' })

    fs.rmSync(path.join(wwwroot, 'app'), { recursive: true, force: true })
    fs.cpSync(path.join(frontendRoot, 'dist'), path.join(wwwroot, 'app'), { recursive: true })
    fs.rmSync(path.join(wwwroot, 'admin'), { recursive: true, force: true })
    fs.cpSync(path.join(frontendRoot, 'dist-admin'), path.join(wwwroot, 'admin'), { recursive: true })
    fs.copyFileSync(path.join(wwwroot, 'admin', 'admin.html'), adminIndex)
    console.log('[e2e setup] SPAs staged into backend/wwwroot (app + admin)')
  }

  // Provider attestation prevents a MySQL-labelled run from silently reusing or booting SQLite.
  const port = process.env.E2E_PORT || '8080'
  const health = await fetch(`http://127.0.0.1:${port}/api/health`)
  const payload = await health.json() as { database_provider?: string }
  const expected = (process.env.E2E_DB_PROVIDER || 'sqlite').toLowerCase()
  const actual = payload.database_provider
  const matches = expected === 'sqlite'
    ? actual === 'sqlite'
    : actual === 'mysql' || actual === 'mariadb'
  if (!matches) {
    throw new Error(`E2E database-provider attestation failed: expected ${expected}, got ${actual || 'missing'}`)
  }
}
