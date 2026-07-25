import { test, expect } from '@playwright/test'
import { apiLoginAsDemoStudent, apiLoginAsE2EAdmin, captureStoryEvidence, createTestUser, preparePublicJourney, uniqueEmail, DEMO_STUDENT } from './util'

// Simulation Lab — student API journey + admin studio. Practice must never touch formal exam records.

test.describe('Simulation Lab student journey', () => {
  test('start GL-EVM-001, autosave, submit, coach', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)

    const login = await request.post('/api/login', { data: DEMO_STUDENT })
    expect(login.ok(), `demo login ${login.status()}`).toBeTruthy()
    const { token } = (await login.json()) as { token: string }
    expect(token).toBeTruthy()
    const auth = { Authorization: `Bearer ${token}` }

    await apiLoginAsDemoStudent(request, page)
    await page.goto('/lab')
    // Catalogue may gate on entitlement; still capture whatever the student sees.
    await captureStoryEvidence(page, testInfo, 'simlab-student', 'catalogue')

    const start = await request.post('/api/me/lab/attempts', {
      headers: auth,
      data: { scenario_code: 'GL-EVM-001', mode: 'training' },
    })
    if (start.status() === 403) {
      testInfo.annotations.push({
        type: 'note',
        description: 'Demo student lacks Lab entitlement; covered by integration_test member path.',
      })
      test.skip()
      return
    }
    expect(start.ok(), `start ${start.status()} ${await start.text()}`).toBeTruthy()
    const started = await start.json() as {
      attempt_id: number
      task: { ask: { key: string; type: string }[]; given: Record<string, number> }
    }

    const g = started.task.given
    const pv = Number(g.pv), ev = Number(g.ev), ac = Number(g.ac), bac = Number(g.bac)
    const spi = ev / pv, cpi = ev / ac, eac = bac / cpi
    const bank: Record<string, number> = {
      sv: ev - pv, cv: ev - ac, spi, cpi, eac,
      etc: eac - ac, vac: bac - eac, tcpi: (bac - ev) / (bac - ac),
      percent_complete: ev / bac, percent_spent: ac / bac,
      eac_cpi: eac, eac_composite: ac + (bac - ev) / (cpi * spi), eac_budget: ac + (bac - ev),
    }
    const payload: Record<string, number> = {}
    for (const a of started.task.ask) if (a.key in bank) payload[a.key] = bank[a.key]

    const saved = await request.post(`/api/me/lab/attempts/${started.attempt_id}/autosave`, {
      headers: auth, data: { answers: payload },
    })
    expect(saved.ok(), `autosave ${saved.status()}`).toBeTruthy()

    // Resume must round-trip the autosaved answers (not only the attempt id).
    const resume = await request.post('/api/me/lab/attempts', {
      headers: auth,
      data: { scenario_code: 'GL-EVM-001', mode: 'training' },
    })
    expect(resume.ok(), `resume ${resume.status()}`).toBeTruthy()
    const resumed = await resume.json() as {
      attempt_id: number
      resumed: boolean
      answers?: Record<string, number>
    }
    expect(resumed.attempt_id).toBe(started.attempt_id)
    expect(resumed.resumed).toBeTruthy()
    expect(resumed.answers?.spi).toBeCloseTo(spi, 5)

    const hint = await request.post(`/api/me/lab/attempts/${started.attempt_id}/coach`, {
      headers: auth,
      data: { answers: { ...payload, spi: 9 }, coach_mode: 'guided', hint_level: 2 },
    })
    expect(hint.ok(), `coach ${hint.status()}`).toBeTruthy()
    const hintBody = await hint.json() as { ok: boolean; message: string }
    expect(hintBody.ok).toBeTruthy()
    expect(hintBody.message.toLowerCase()).not.toContain('the correct value is')

    const submit = await request.post(`/api/me/lab/attempts/${started.attempt_id}/submit`, {
      headers: auth, data: { answers: payload },
    })
    expect(submit.ok(), `submit ${submit.status()} ${await submit.text()}`).toBeTruthy()
    const grade = await submit.json() as { score: number; total: number }
    expect(grade.total).toBeGreaterThan(0)
    expect(grade.score).toBe(100)

    const dup = await request.post(`/api/me/lab/attempts/${started.attempt_id}/submit`, {
      headers: auth, data: { answers: payload },
    })
    expect(dup.status()).toBe(409)

    await page.goto('/lab/GL-EVM-001')
    await expect(page.getByText(/Practice never affects|Earned|SPI|Schedule/i).first())
      .toBeVisible({ timeout: 20_000 })
    await captureStoryEvidence(page, testInfo, 'simlab-student', 'runner')
  })

  test('runs the linked multi-step recovery scenario end-to-end', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)
    const login = await request.post('/api/login', { data: DEMO_STUDENT })
    const { token } = (await login.json()) as { token: string }
    const auth = { Authorization: `Bearer ${token}` }
    await apiLoginAsDemoStudent(request, page)

    const start = await request.post('/api/me/lab/attempts', {
      headers: auth, data: { scenario_code: 'MS-RECOVERY-001', mode: 'training' },
    })
    if (start.status() === 403) {
      testInfo.annotations.push({ type: 'note', description: 'Demo student lacks Lab entitlement; multi-step member path covered by integration_test.' })
      test.skip()
      return
    }
    expect(start.ok(), `start ${start.status()} ${await start.text()}`).toBeTruthy()
    const started = await start.json() as {
      attempt_id: number
      task: { multistep?: boolean; steps?: { id: string; task: string; given: Record<string, number>; ask: { key: string }[]; decision?: { key: string; options: { value: string; effects: { step: string; path: string; op: string; value: number }[] }[] } | null }[] }
    }
    expect(started.task.multistep, 'served task is multi-step').toBeTruthy()
    expect((started.task.steps ?? []).length).toBeGreaterThanOrEqual(4)

    // Correct per-step answers under chosen decisions — mirror of Core/SimStep.EffectiveGiven + EVM solver.
    const decisions: Record<string, string> = { recovery: 'crash', funding: 'request' }
    const steps = started.task.steps!
    const effGiven = (id: string, base: Record<string, number>) => {
      const g = { ...base }
      for (const s of steps) {
        const chosen = s.decision ? decisions[s.decision.key] : undefined
        const opt = s.decision?.options.find((o) => o.value === chosen)
        for (const e of opt?.effects ?? []) if (e.step === id) g[e.path] = e.op === 'add' ? (g[e.path] ?? 0) + e.value : e.op === 'mul' ? (g[e.path] ?? 0) * e.value : e.value
      }
      return g
    }
    const evm = (g: Record<string, number>): Record<string, number> => {
      const { pv, ev, ac, bac } = g
      const cpi = ac === 0 ? 0 : ev / ac, spi = pv === 0 ? 0 : ev / pv, eac = cpi === 0 ? 0 : bac / cpi
      return { sv: ev - pv, cv: ev - ac, spi, cpi, eac, vac: bac - eac, tcpi: (bac - ac) === 0 ? 0 : (bac - ev) / (bac - ac), percent_complete: bac === 0 ? 0 : ev / bac }
    }
    const stepAns: Record<string, Record<string, number>> = {}
    for (const s of steps) {
      const solved = evm(effGiven(s.id, s.given))
      stepAns[s.id] = Object.fromEntries(s.ask.map((a) => [a.key, solved[a.key]]))
    }
    const submit = await request.post(`/api/me/lab/attempts/${started.attempt_id}/submit`, {
      headers: auth, data: { answers: { steps: stepAns, decisions } },
    })
    expect(submit.ok(), `submit ${submit.status()} ${await submit.text()}`).toBeTruthy()
    const grade = await submit.json() as { score: number }
    expect(grade.score).toBe(100)

    await page.goto('/lab/MS-RECOVERY-001')
    await expect(page.getByText(/recover|baseline|decision|forecast/i).first()).toBeVisible({ timeout: 20_000 })
    await captureStoryEvidence(page, testInfo, 'simlab-student', 'multistep-runner')
  })
})

test.describe('Simulation Lab admin studio', () => {
  test('list, validate, create draft, student blocked from admin APIs', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)
    const token = await apiLoginAsE2EAdmin(request, page)

    const list = await request.get('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${token}` },
    })
    expect(list.ok()).toBeTruthy()
    const body = await list.json() as { total: number; rows: { id: number; scenario_code: string }[] }
    expect(body.total).toBeGreaterThanOrEqual(30)

    const row = body.rows.find((r) => r.scenario_code === 'GL-EVM-001') ?? body.rows[0]
    const validate = await request.get(`/api/admin/lab/scenarios/${row.id}/validate`, {
      headers: { Authorization: `Bearer ${token}` },
    })
    expect(validate.ok()).toBeTruthy()
    const v = await validate.json() as { publishable: boolean }
    expect(v.publishable).toBeTruthy()

    const code = `E2E-DRAFT-${Date.now()}`
    const create = await request.post('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${token}` },
      data: {
        scenario_code: code,
        title: 'E2E draft scenario',
        kind: 'scenario',
        industry: 'Infrastructure',
        difficulty: 'foundation',
        synthetic_declared: true,
        competencies: ['earned_value'],
        config_json: {
          task: 'evm',
          prompt: 'E2E draft',
          given: { pv: 100, ev: 90, ac: 95, bac: 200 },
          ask: [{ key: 'spi', label: 'SPI', type: 'number' }],
          tolerance: 0.01,
          pass_pct: 70,
          competencies: ['earned_value'],
        },
      },
    })
    expect(create.ok(), await create.text()).toBeTruthy()

    // A multi-step draft authored through the studio must also pass the publication validator (every step
    // and every decision branch reference-solves).
    const msCode = `E2E-MS-${Date.now()}`
    const createMs = await request.post('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${token}` },
      data: {
        scenario_code: msCode, title: 'E2E multi-step draft', kind: 'scenario', industry: 'Infrastructure',
        difficulty: 'intermediate', synthetic_declared: true, competencies: ['earned_value', 'decision_making'],
        config_json: {
          multistep: true, prompt: 'E2E linked scenario', pass_pct: 70, competencies: ['earned_value', 'decision_making'],
          steps: [
            { id: 's1', task: 'evm', prompt: 'CPI', given: { pv: 100, ev: 90, ac: 95, bac: 200 }, ask: [{ key: 'cpi', label: 'CPI', type: 'number' }],
              decision: { key: 'd', prompt: 'Recover?', options: [
                { value: 'a', label: 'Crash (+20 AC downstream)', effects: [{ step: 's2', path: 'ac', op: 'add', value: 20 }] },
                { value: 'b', label: 'No change', effects: [] }] } },
            { id: 's2', task: 'evm', prompt: 'EAC', given: { pv: 100, ev: 90, ac: 100, bac: 200 }, ask: [{ key: 'eac', label: 'EAC', type: 'number' }] },
          ],
        },
      },
    })
    expect(createMs.ok(), await createMs.text()).toBeTruthy()
    const msId = ((await createMs.json()) as { id: number }).id
    const validateMs = await request.get(`/api/admin/lab/scenarios/${msId}/validate`, { headers: { Authorization: `Bearer ${token}` } })
    expect(validateMs.ok()).toBeTruthy()
    expect(((await validateMs.json()) as { publishable: boolean }).publishable, 'multi-step draft is publishable').toBeTruthy()

    await page.goto('/admin/lab')
    await expect(page.getByRole('heading', { name: /simulation lab/i }).first()).toBeVisible({ timeout: 20_000 })
    await captureStoryEvidence(page, testInfo, 'simlab-admin', 'studio')

    const studentLogin = await request.post('/api/login', { data: DEMO_STUDENT })
    const studentTok = ((await studentLogin.json()) as { token?: string }).token
    const denied = await request.get('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${studentTok}` },
    })
    expect([401, 403]).toContain(denied.status())
  })
})

// Maker-checker is the control that gates publication of assessment content: the admin who authored a
// scenario must not be the one who approves it. The integration suite asserts this over the API; this
// proves it where an operator actually meets it — two real admin sessions in the Studio, one refused
// and one allowed on the same row.
test.describe('Simulation Lab maker-checker', () => {
  test('the author is refused approval in the Studio and a second admin can approve the same scenario', async ({ page, context, request }, testInfo) => {
    const authorToken = await apiLoginAsE2EAdmin(request, page)

    // A publishable scenario authored by admin A, walked up to the stage before approval. Those
    // earlier moves are the author's to make — only the approval is gated.
    const code = `E2E-MC-${Date.now()}`
    const create = await request.post('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${authorToken}` },
      data: {
        scenario_code: code,
        title: 'E2E maker-checker scenario',
        kind: 'scenario',
        industry: 'Infrastructure',
        difficulty: 'foundation',
        synthetic_declared: true,
        competencies: ['earned_value'],
        config_json: {
          task: 'evm', prompt: 'Maker-checker', given: { pv: 100, ev: 90, ac: 95, bac: 200 },
          ask: [{ key: 'spi', label: 'SPI', type: 'number' }],
          tolerance: 0.01, pass_pct: 70, competencies: ['earned_value'],
        },
      },
    })
    expect(create.ok(), await create.text()).toBeTruthy()
    const id = ((await create.json()) as { id: number }).id
    for (const to of ['calc_review', 'learning_review', 'safety_review', 'pilot']) {
      const step = await request.post(`/api/admin/lab/scenarios/${id}/review`, {
        headers: { Authorization: `Bearer ${authorToken}` }, data: { to },
      })
      expect(step.ok(), `author advances to ${to}: ${await step.text()}`).toBeTruthy()
    }

    // Admin A meets the gate in the UI: the row offers Approved, and the Studio refuses it by name.
    await page.goto('/admin/lab')
    const authorRow = page.getByRole('row', { name: new RegExp(code) })
    await expect(authorRow).toBeVisible({ timeout: 20_000 })
    await authorRow.getByRole('button', { name: '→ Approved' }).click()
    await expect(page.getByRole('alert')).toContainText('a second admin has to approve this')
    await expect(authorRow).toContainText('Pilot')
    await captureStoryEvidence(page, testInfo, 'simlab-maker-checker', 'author-refused')

    // A second, independent admin — sim_lab only, so this proves the permission carries approval
    // rights without owner privileges.
    const checkerEmail = uniqueEmail('sim-checker')
    const checkerPassword = 'BrowserLabChecker-2026!'
    const invite = await request.post('/api/admin/team', {
      headers: { Authorization: `Bearer ${authorToken}` },
      data: {
        email: checkerEmail, name: 'Browser Lab Checker', role: 'custom',
        permissions: ['sim_lab'], password: checkerPassword, force_password_change: false,
      },
    })
    expect(invite.ok(), await invite.text()).toBeTruthy()

    const checkerPage = await context.newPage()
    await checkerPage.goto('/admin/login')
    await checkerPage.getByLabel('Email address').fill(checkerEmail)
    await checkerPage.getByLabel('Password', { exact: true }).fill(checkerPassword)
    await checkerPage.getByRole('button', { name: 'Sign in' }).click()
    await expect(checkerPage.getByText('Browser Lab Checker')).toBeVisible({ timeout: 30_000 })

    await checkerPage.goto('/admin/lab')
    const checkerRow = checkerPage.getByRole('row', { name: new RegExp(code) })
    await expect(checkerRow).toBeVisible({ timeout: 20_000 })
    await checkerRow.getByRole('button', { name: '→ Approved' }).click()
    await expect(checkerPage.getByRole('alert')).toHaveCount(0)
    await expect(checkerRow).toContainText('Approved')
    await captureStoryEvidence(checkerPage, testInfo, 'simlab-maker-checker', 'second-admin-approved')

    // The approval is attributed to the checker, not to the author who tried first.
    const after = await request.get('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${authorToken}` },
    })
    const row = ((await after.json()) as { rows: Array<{ scenario_code: string; review_state: string }> })
      .rows.find((r) => r.scenario_code === code)
    expect(row?.review_state, 'the second admin moved it to approved').toBe('approved')

    const audit = await request.get('/api/admin/audit', { headers: { Authorization: `Bearer ${authorToken}` } })
    const entries = ((await audit.json()) as { rows: Array<{ action?: string; details?: string; actor?: string }> }).rows
    expect(entries).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'sim_scenario_review', details: expect.stringContaining(`${code} pilot→approved`), actor: checkerEmail }),
    ]))
  })
})

test.describe('Simulation Lab catalogue — server-side filtering', () => {
  // The catalogue is filtered/searched SERVER-side (the capacity path). An entitled member sees the
  // real catalogue; selecting a facet refetches only the matched rows, while the whole-catalogue KPIs
  // (Published labs) and the "of N" total stay put — proving the summary is full-set, not the match.
  test('a facet filter refetches matched rows and leaves the full-set KPIs unchanged', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'member', page)

    await page.goto('/app/lab')
    await expect(page.getByRole('heading', { name: /Project Controls Practice Lab/i })).toBeVisible({ timeout: 20_000 })
    await expect(page.getByRole('status', { name: 'Loading' })).toHaveCount(0)

    // Baseline: the "Showing X of N" line and the "Published labs" KPI.
    const results = page.getByText(/Showing \d+ of \d+ labs/)
    await expect(results).toBeVisible()
    const parse = async () => {
      const m = (await results.textContent())?.match(/Showing (\d+) of (\d+) labs/)
      expect(m, 'results line should report counts').toBeTruthy()
      return { shown: Number(m![1]), total: Number(m![2]) }
    }
    const before = await parse()
    expect(before.total).toBeGreaterThan(1)
    const publishedKpi = page.locator('.sl-kpi', { hasText: 'Published labs' }).locator('.v')
    const publishedBefore = (await publishedKpi.textContent())?.trim()
    expect(Number(publishedBefore)).toBe(before.total)

    // Filter to Expert only — a strict subset of the catalogue.
    await page.getByLabel('Filter by difficulty').selectOption('expert')
    await expect(page).toHaveURL(/difficulty=expert/)
    // The server round-trip settles: fewer rows shown, same full-set total, unchanged KPI.
    await expect
      .poll(async () => (await parse()).shown, { timeout: 10_000 })
      .toBeLessThan(before.total)
    const after = await parse()
    expect(after.total, 'the "of N" total is the full published set, not the match').toBe(before.total)
    expect((await publishedKpi.textContent())?.trim(), 'the Published-labs KPI is full-set').toBe(publishedBefore)

    // Every visible card is Expert (the server filtered, the browser did not).
    const metas = page.locator('.sl-scenario .sl-meta')
    const count = await metas.count()
    expect(count).toBeGreaterThan(0)
    for (let i = 0; i < count; i++) {
      expect((await metas.nth(i).textContent())?.toLowerCase()).toContain('expert')
    }
  })
})
