import { test, expect } from '@playwright/test'
import {
  E2E_ADMIN,
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  registerStudent,
  settleExamPurchase,
} from './util'

const tinyPng = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=='

test.describe('live proctor operations', () => {
  test('proctor monitors a live candidate, exchanges messages and clears the submitted session', async ({ page, request }, testInfo) => {
    test.slow()
    const student = await registerStudent(request, undefined, 'proctor')
    const headers = { Authorization: `Bearer ${student.token}` }
    await settleExamPurchase(request, student.email, 'PCL-AI', 'bundle')

    for (const [label, response] of [
      ['consents', await request.post('/api/me/consents', { headers, data: { accept_all: true } })],
      ['profile', await request.patch('/api/me/profile', { headers, data: { country: 'United Kingdom', city: 'London' } })],
      ['identity', await request.post('/api/me/identity-document', {
        headers,
        data: { doc_kind: 'passport', filename: 'proctor-id.png', data_uri: tinyPng },
      })],
    ] as const) expect(response.ok(), `${label} prerequisite should succeed`).toBeTruthy()

    const meResponse = await request.get('/api/me', { headers })
    expect(meResponse.ok()).toBeTruthy()
    const me = (await meResponse.json()) as {
      exams: Array<{ certification_id: number; certification_code: string }>
    }
    const certificationId = me.exams.find((exam) => exam.certification_code === 'PCL-AI')?.certification_id
    expect(certificationId).toBeTruthy()

    const booking = await request.post('/api/me/exam/book', {
      headers,
      data: {
        certification_id: certificationId,
        scheduled_at: new Date(Date.now() + 30 * 3_600_000).toISOString(),
        timezone: 'UTC',
      },
    })
    expect(booking.ok()).toBeTruthy()
    const readiness = await request.post('/api/me/readiness', {
      headers,
      data: { camera: true, microphone: true, network: true, fullscreen: true, environment: true },
    })
    expect(readiness.ok()).toBeTruthy()
    const startResponse = await request.post('/api/me/exam/start', {
      headers,
      data: { certification_id: certificationId },
    })
    expect(startResponse.ok()).toBeTruthy()
    const started = (await startResponse.json()) as { attempt_id: number }

    const candidateMessage = `Candidate check-in ${Date.now()}`
    const firstHeartbeat = await request.post('/api/me/exam/heartbeat', {
      headers,
      data: {
        attempt_id: started.attempt_id,
        events: [{
          type: 'window_blur',
          severity: 'Critical',
          detail: 'E2E candidate changed focus during the live sitting.',
          at: new Date().toISOString(),
        }],
        chat_out: [candidateMessage],
      },
    })
    expect(firstHeartbeat.ok()).toBeTruthy()

    const adminToken = await apiLoginAsE2EAdmin(request, page)
    await page.goto('/admin/proctoring')
    await page.getByRole('button', { name: 'Live' }).click()
    const liveRow = page.locator('tbody tr').filter({ hasText: student.email })
    await expect(liveRow).toContainText('live')
    await expect(liveRow).toContainText('1')
    await liveRow.getByRole('button', { name: 'Monitor' }).click()

    let drawer = page.locator('.drawer')
    await expect(drawer.getByRole('heading', { name: `Exam session #${started.attempt_id}` })).toBeVisible()
    await expect(drawer).toContainText(student.email)
    await expect(drawer).toContainText('Window Blur')
    await expect(drawer).toContainText(/critical/i)
    await expect(drawer).toContainText(candidateMessage)

    const proctorMessage = `Please remain in the exam window ${Date.now()}`
    await drawer.getByPlaceholder('Message the candidate…').fill(proctorMessage)
    const messageResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/exam-sessions/${started.attempt_id}/message`)
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Send' }).click()
    expect((await messageResponsePromise).ok()).toBeTruthy()
    await expect(drawer).toContainText(proctorMessage)

    const deliveredHeartbeat = await request.post('/api/me/exam/heartbeat', {
      headers,
      data: { attempt_id: started.attempt_id },
    })
    expect(deliveredHeartbeat.ok()).toBeTruthy()
    const delivered = (await deliveredHeartbeat.json()) as {
      messages: Array<{ from: string; body: string; at: string }>
    }
    expect(delivered.messages).toEqual([
      expect.objectContaining({
        from: 'PCI Support',
        body: proctorMessage,
        at: expect.stringMatching(/T/),
      }),
    ])
    await captureStoryEvidence(page, testInfo, 'D7-G7', 'live-proctor-event-and-two-way-message')

    const submitResponse = await request.post('/api/me/exam/submit', {
      headers,
      data: { attempt_id: started.attempt_id, answers: {} },
    })
    expect(submitResponse.ok()).toBeTruthy()
    expect(await submitResponse.json()).toMatchObject({ result: 'fail', held: false })

    await drawer.getByRole('button', { name: 'Close' }).click()
    await page.getByRole('button', { name: 'All sessions' }).click()
    const submittedRow = page.locator('tbody tr').filter({ hasText: student.email })
    await expect(submittedRow).toContainText('released fail')
    await submittedRow.click()
    drawer = page.locator('.drawer')
    await expect(drawer).toContainText('0%')
    await drawer.getByPlaceholder('Reason / context for the decision').fill('E2E proctor evidence reviewed.')
    const reviewResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/exam-sessions/${started.attempt_id}/review`)
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Mark reviewed' }).click()
    expect((await reviewResponsePromise).ok()).toBeTruthy()
    await expect(drawer).toContainText('Action “clear” applied')
    await captureStoryEvidence(page, testInfo, 'G7', 'submitted-session-reviewed')

    const detailResponse = await request.get(`/api/admin/exam-sessions/${started.attempt_id}`, {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(detailResponse.ok()).toBeTruthy()
    expect(await detailResponse.json()).toMatchObject({
      attempt: { review_status: 'cleared' },
      messages: expect.arrayContaining([
        expect.objectContaining({ sender: 'candidate', body: candidateMessage }),
        expect.objectContaining({ sender: 'proctor', body: proctorMessage }),
      ]),
    })

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'proctor_message', details: `attempt ${started.attempt_id}`, actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'exam_reviewed', details: `attempt ${started.attempt_id}`, actor: E2E_ADMIN.email }),
    ]))
  })
})
