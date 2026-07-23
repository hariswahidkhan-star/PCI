import { test, expect } from '@playwright/test'
import { apiLoginAsDemoStudent, apiLoginAsE2EAdmin, captureStoryEvidence, preparePublicJourney } from './util'

// Simulation Lab — student catalogue → start → autosave → submit → coach, plus practice isolation.
// Uses the seeded house catalogue (GL-EVM-001). Practice must never touch formal exam records.

test.describe('Simulation Lab student journey', () => {
  test('catalogue, run GL-EVM-001, autosave, submit, coach, isolation', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)
    await apiLoginAsDemoStudent(request, page)

    const beforeExam = await request.get('/api/me')
    expect(beforeExam.ok()).toBeTruthy()
    const beforeBody = await beforeExam.json() as { exam_attempts?: unknown[]; credentials?: unknown[] }
    const examCountBefore = Array.isArray(beforeBody.exam_attempts) ? beforeBody.exam_attempts.length : undefined

    await page.goto('/lab')
    await expect(page.getByRole('heading', { name: /practice lab|simulation lab/i }).first()).toBeVisible({ timeout: 20_000 })
    await captureStoryEvidence(page, testInfo, 'simlab-student', 'catalogue')

    const access = await request.get('/api/me/lab/access')
    expect(access.ok(), `lab access ${access.status()}`).toBeTruthy()

    const start = await request.post('/api/me/lab/attempts', { data: { scenario_code: 'GL-EVM-001', mode: 'training' } })
    expect(start.ok(), `start attempt ${start.status()} ${await start.text()}`).toBeTruthy()
    const started = await start.json() as { attempt_id: number; task: { ask: { key: string }[] } }
    expect(started.attempt_id).toBeTruthy()

    const answers: Record<string, number> = {
      sv: -10000, cv: -5000, spi: 0.9, cpi: 0.9474, eac: 211111.11,
      etc: 116111.11, vac: -11111.11, tcpi: 1.0476,
      percent_complete: 0.45, percent_spent: 0.475,
      eac_cpi: 211111.11, eac_composite: 222222.22, eac_budget: 210000,
    }
    // Only send keys the densified scenario actually asks.
    const payload: Record<string, number> = {}
    for (const a of started.task.ask) if (a.key in answers) payload[a.key] = answers[a.key]

    const saved = await request.post(`/api/me/lab/attempts/${started.attempt_id}/autosave`, { data: { answers: payload } })
    expect(saved.ok(), `autosave ${saved.status()}`).toBeTruthy()

    const hint = await request.post(`/api/me/lab/attempts/${started.attempt_id}/coach`, {
      data: { answers: payload, coach_mode: 'guided', hint_level: 2 },
    })
    expect(hint.ok(), `coach ${hint.status()}`).toBeTruthy()
    const hintBody = await hint.json() as { ok: boolean; message: string; coach_mode?: string }
    expect(hintBody.ok).toBeTruthy()
    expect(hintBody.message.toLowerCase()).not.toContain('the correct value is')

    const submit = await request.post(`/api/me/lab/attempts/${started.attempt_id}/submit`, { data: { answers: payload } })
    expect(submit.ok(), `submit ${submit.status()} ${await submit.text()}`).toBeTruthy()
    const grade = await submit.json() as { score: number; total: number; passed: boolean }
    expect(grade.total).toBeGreaterThan(0)
    expect(grade.score).toBeGreaterThanOrEqual(0)

    const dup = await request.post(`/api/me/lab/attempts/${started.attempt_id}/submit`, { data: { answers: payload } })
    expect(dup.status()).toBe(409)

    await page.goto(`/lab/GL-EVM-001`)
    await expect(page.getByText(/Schedule Variance|SPI|Earned/i).first()).toBeVisible({ timeout: 20_000 })
    await captureStoryEvidence(page, testInfo, 'simlab-student', 'runner')

    const afterExam = await request.get('/api/me')
    expect(afterExam.ok()).toBeTruthy()
    const afterBody = await afterExam.json() as { exam_attempts?: unknown[] }
    if (examCountBefore !== undefined && Array.isArray(afterBody.exam_attempts)) {
      expect(afterBody.exam_attempts.length).toBe(examCountBefore)
    }
  })
})

test.describe('Simulation Lab admin studio', () => {
  test('list, validate, create draft, unauthorized student blocked', async ({ page, request }, testInfo) => {
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

    // Student bearer must not reach admin lab APIs.
    await apiLoginAsDemoStudent(request, page)
    const studentTok = await page.evaluate(() => sessionStorage.getItem('pci.session.token'))
    const denied = await request.get('/api/admin/lab/scenarios', {
      headers: { Authorization: `Bearer ${studentTok}` },
    })
    expect([401, 403]).toContain(denied.status())
  })
})
