import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, apiRegisterStudent, certificationId, seedStudentSession, storyScreenshot } from './util'

test.describe('credential issuing and verification', () => {
  test('admin issues, revokes and reinstates a student credential', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const student = await apiRegisterStudent(request, 'credential')
    const certId = await certificationId(request, 'pml-ai')
    const credentialId = `E2E-PML-${Date.now()}-${process.pid}`.toUpperCase()

    const issue = await request.post('/api/admin/credentials', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: {
        credential_id: credentialId,
        user_id: student.user.id,
        certification_id: certId,
        holder_name: 'E2E Student',
        credential: 'PML-AI',
        expires_at: '2099-12-31',
      },
    })
    expect(issue.ok(), `credential issue should succeed (got ${issue.status()})`).toBeTruthy()
    const issued = (await issue.json()) as { id: number }

    await seedStudentSession(page, student.token)
    await page.goto('/app/credentials')
    await expect(page.getByRole('heading', { name: /credentials/i })).toBeVisible()
    await expect(page.getByText(credentialId)).toBeVisible()
    await expect(page.getByText(/PML-AI/).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'student-pml-ai-credential')

    const verifyActive = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(verifyActive.ok()).toBeTruthy()
    expect(await verifyActive.json()).toMatchObject({ found: true, valid: true, state: 'active' })

    const revoke = await request.post(`/api/admin/credentials/${issued.id}/status`, {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { status: 'revoked' },
    })
    expect(revoke.ok(), `credential revoke should succeed (got ${revoke.status()})`).toBeTruthy()
    const verifyRevoked = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(await verifyRevoked.json()).toMatchObject({ found: true, valid: false, state: 'revoked' })

    const reinstate = await request.post(`/api/admin/credentials/${issued.id}/status`, {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { status: 'active' },
    })
    expect(reinstate.ok(), `credential reinstate should succeed (got ${reinstate.status()})`).toBeTruthy()
    const verifyReinstated = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(await verifyReinstated.json()).toMatchObject({ found: true, valid: true, state: 'active' })

    const verifyPage = await page.goto(`/verify.html?id=${encodeURIComponent(credentialId)}`)
    expect(verifyPage?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
  })
})
