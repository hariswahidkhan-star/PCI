import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, apiRegisterStudent, seedStudentSession, storyScreenshot } from './util'

const samplePdf = `data:application/pdf;base64,${Buffer.from('%PDF-1.4 e2e document').toString('base64')}`

test.describe('student documents and CPD', () => {
  test('student submits CPD, admin approves it, and approved total updates', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const student = await apiRegisterStudent(request, 'cpd')
    const auth = { Authorization: `Bearer ${student.token}` }
    const description = `E2E CPD ${Date.now()}`
    const hours = 4.5

    const submit = await request.post('/api/me/cpd', {
      headers: auth,
      data: { description, category: 'Structured learning', hours, activity_date: '2026-07-23' },
    })
    expect(submit.ok(), `student CPD submit should succeed (got ${submit.status()})`).toBeTruthy()

    const cpdList = await request.get('/api/me/cpd', { headers: auth })
    expect(cpdList.ok()).toBeTruthy()
    const entry = ((await cpdList.json()) as { rows: Array<{ id: number; description?: string }> }).rows
      .find((r) => r.description === description)
    expect(entry, 'new CPD entry should be listed').toBeTruthy()

    const review = await request.post(`/api/admin/cpd/${entry!.id}/review`, {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { status: 'approved', admin_note: 'E2E approval' },
    })
    expect(review.ok(), `CPD approval should succeed (got ${review.status()})`).toBeTruthy()

    await expect.poll(async () => {
      const me = await request.get('/api/me', { headers: auth })
      const body = (await me.json()) as { cpd?: { total?: number } }
      return body.cpd?.total ?? 0
    }).toBe(hours)

    await seedStudentSession(page, student.token)
    await page.goto('/app/cpd')
    await expect(page.getByText(description)).toBeVisible()
    await expect(page.getByText(String(hours)).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'approved-cpd-total')
  })

  test('assigned document can be acknowledged by the student', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const student = await apiRegisterStudent(request, 'document')
    const title = `E2E policy acknowledgement ${Date.now()}`
    const create = await request.post('/api/admin/documents', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: {
        title,
        description: 'E2E acknowledgement document',
        filename: 'e2e-policy.pdf',
        file: samplePdf,
        assignment_type: 'student',
        user_id: student.user.id,
        ack_required: true,
        publish: true,
      },
    })
    expect(create.ok(), `document create/publish should succeed (got ${create.status()})`).toBeTruthy()
    const doc = (await create.json()) as { id: number; status: string }
    expect(doc.status).toMatch(/published|active/)

    const docs = await request.get('/api/me/documents', { headers: { Authorization: `Bearer ${student.token}` } })
    expect(docs.ok()).toBeTruthy()
    const assigned = ((await docs.json()) as { rows: Array<{ id: number; title?: string }> }).rows.find((r) => r.id === doc.id)
    expect(assigned?.title).toBe(title)

    const ack = await request.post(`/api/me/documents/${doc.id}/acknowledge`, {
      headers: { Authorization: `Bearer ${student.token}` },
    })
    expect(ack.ok(), `document acknowledgement should succeed (got ${ack.status()})`).toBeTruthy()
    expect(await ack.json()).toMatchObject({ ok: true })

    await seedStudentSession(page, student.token)
    await page.goto('/app/documents')
    await expect(page.getByText(title)).toBeVisible()
    await storyScreenshot(page, testInfo, 'document-acknowledged')
  })
})
