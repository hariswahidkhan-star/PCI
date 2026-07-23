import { test, expect } from '@playwright/test'
import { apiLoginAsDemoStudent, apiLoginAsE2EAdmin, captureStoryEvidence, preparePublicJourney, DEMO_STUDENT } from './util'

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
