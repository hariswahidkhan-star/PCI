import { test, expect } from '@playwright/test'
import { apiLoginAsE2EAdmin, uniqueEmail } from './util'

test.describe('institution partner persona', () => {
  test('admin provisions a partner who changes password, creates a code and sponsors a PML-AI candidate', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const headers = { Authorization: `Bearer ${adminToken}` }
    const institution = `Browser Institute ${Date.now()}`
    const partnerEmail = uniqueEmail('partner-admin')
    const candidateEmail = uniqueEmail('partner-candidate')
    const isolatedCandidateEmail = uniqueEmail('partner-isolated')
    const newPassword = 'PartnerBrowser-2026!'
    const code = `E2E${Date.now().toString().slice(-9)}`

    const createPartnerResponse = await request.post('/api/admin/training-partners', {
      headers,
      data: {
        name: institution,
        tier: 'Gold',
        country: 'United Kingdom',
        city: 'London',
        website: 'https://partner.example.test',
        contact_email: partnerEmail,
        listed: false,
      },
    })
    expect(createPartnerResponse.ok()).toBeTruthy()
    const partner = (await createPartnerResponse.json()) as { id: number }
    expect(partner.id).toBeGreaterThan(0)

    const configurePartnerResponse = await request.patch(`/api/admin/training-partners/${partner.id}`, {
      headers,
      data: {
        status: 'active',
        partner_type: 'institution',
        sponsor_enabled: true,
        commission_pct: 7.5,
        max_discount_percent: 25,
        max_codes: 10,
        max_uses_per_code: 50,
        total_allocation: 500,
        allow_full_sponsorship: false,
        auto_approve_codes: true,
      },
    })
    expect(configurePartnerResponse.ok()).toBeTruthy()

    const createUserResponse = await request.post(`/api/admin/training-partners/${partner.id}/users`, {
      headers,
      data: { email: partnerEmail, name: 'Browser Partner Admin', role: 'admin' },
    })
    expect(createUserResponse.ok()).toBeTruthy()
    const partnerUser = (await createUserResponse.json()) as { id: number; temp_password: string }
    expect(partnerUser.temp_password).toBeTruthy()

    // Establish another device before the mandatory password change. The change must revoke this
    // token while preserving the browser session that performed it.
    const otherDeviceLoginResponse = await request.post('/api/partner/auth/login', {
      data: { email: partnerEmail, password: partnerUser.temp_password },
    })
    expect(otherDeviceLoginResponse.ok()).toBeTruthy()
    const otherDeviceLogin = (await otherDeviceLoginResponse.json()) as { token: string }

    await page.goto('/partner.html')
    await page.getByLabel('Email', { exact: true }).fill(partnerEmail)
    await page.getByLabel('Password', { exact: true }).fill(partnerUser.temp_password)
    const loginResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/partner/auth/login') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Sign in', exact: true }).click()
    expect((await loginResponse).ok()).toBeTruthy()
    await expect(page.getByRole('heading', { name: 'Set a new password' })).toBeVisible()

    await page.getByLabel('New password', { exact: true }).fill(newPassword)
    await page.getByLabel('Confirm new password', { exact: true }).fill(newPassword)
    const passwordResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/partner/auth/password') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Save and continue' }).click()
    expect((await passwordResponse).ok()).toBeTruthy()
    await expect(page.locator('#app')).toBeVisible()
    await expect(page.locator('#instName')).toHaveText(institution)
    await expect(page.getByRole('navigation', { name: 'Portal sections' }).getByRole('button')).toHaveCount(6)
    const partnerToken = await page.evaluate(() => sessionStorage.getItem('pci.partner.token'))
    expect(partnerToken).toBeTruthy()
    const revokedDeviceProbe = await request.get('/api/partner/me', {
      headers: { Authorization: `Bearer ${otherDeviceLogin.token}` },
    })
    expect(revokedDeviceProbe.status(), 'the password change must revoke every other partner session').toBe(401)
    const currentDeviceProbe = await request.get('/api/partner/me', {
      headers: { Authorization: `Bearer ${partnerToken}` },
    })
    expect(currentDeviceProbe.ok(), 'the partner session that changed the password must remain active').toBeTruthy()

    // Partner-managed discount code, constrained by the agreement the PCI operator configured.
    await page.getByRole('button', { name: 'Codes', exact: true }).click()
    await page.getByRole('button', { name: 'New code' }).click()
    await page.getByLabel('Discount %').fill('15')
    await page.getByLabel('Maximum uses').fill('10')
    await page.getByLabel(/Custom code/).fill(code)
    await page.getByLabel(/Campaign name/).fill('Browser cohort')
    const codeResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/partner/codes') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Create code' }).click()
    const createdCode = await codeResponse
    expect(createdCode.ok()).toBeTruthy()
    expect(await createdCode.json()).toMatchObject({ code, status: 'active' })
    await expect(page.locator('#codesTbl')).toContainText(code)
    await expect(page.locator('#codesTbl')).toContainText('active')

    // Direct sponsorship creates the student account, approved application and certification-scoped
    // entitlement as one institution action, then exposes the same progress to PCI administrators.
    await page.getByRole('button', { name: 'Sponsorships', exact: true }).click()
    await expect(page.getByRole('button', { name: 'Sponsor candidates' })).toBeVisible()
    await page.getByRole('button', { name: 'Sponsor candidates' }).click()
    await page.locator('#spRows').fill(`${candidateEmail}, Browser, Candidate`)
    await expect(page.locator('#spCert option[value="PML-AI"]')).toHaveCount(1)
    await page.locator('#spCert').selectOption('PML-AI')
    const sponsorResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/partner/candidates') && response.request().method() === 'POST')
    await page.locator('#btnSponsorGo').click()
    const sponsored = await sponsorResponse
    expect(sponsored.ok()).toBeTruthy()
    const sponsorResult = (await sponsored.json()) as { results: Array<Record<string, unknown>> }
    expect(sponsorResult.results).toEqual([
      expect.objectContaining({ email: candidateEmail, status: 'sponsored', account: 'created', certification: 'PML-AI' }),
    ])
    await expect(page.locator('#sponsorTbl')).toContainText(candidateEmail)
    await expect(page.locator('#sponsorTbl')).toContainText('PML-AI')

    await page.getByRole('button', { name: 'Commissions', exact: true }).click()
    await expect(page.locator('#commTiles')).toContainText('7.5%')
    await expect(page.locator('#commTiles')).toContainText('Balance due')

    const candidatesResponse = await request.get(`/api/admin/training-partners/${partner.id}/candidates`, { headers })
    expect(candidatesResponse.ok()).toBeTruthy()
    const candidates = (await candidatesResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(candidates.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ candidate_email: candidateEmail, cert_code: 'PML-AI', application_status: 'approved', entitlement_status: 'available' }),
    ]))

    // Build a second institution and sponsor a different candidate, then prove both API views are
    // mutually exclusive. This is the cross-tenant check that a single happy-path partner cannot give.
    const isolatedInstitution = `Isolated Browser Institute ${Date.now()}`
    const isolatedPartnerResponse = await request.post('/api/admin/training-partners', {
      headers,
      data: { name: isolatedInstitution, country: 'Saudi Arabia', contact_email: uniqueEmail('isolated-partner'), listed: false },
    })
    expect(isolatedPartnerResponse.ok()).toBeTruthy()
    const isolatedPartner = (await isolatedPartnerResponse.json()) as { id: number }
    const isolatedConfigureResponse = await request.patch(`/api/admin/training-partners/${isolatedPartner.id}`, {
      headers,
      data: { status: 'active', partner_type: 'institution', sponsor_enabled: true, auto_approve_codes: true },
    })
    expect(isolatedConfigureResponse.ok()).toBeTruthy()
    const isolatedEmail = uniqueEmail('isolated-admin')
    const isolatedUserResponse = await request.post(`/api/admin/training-partners/${isolatedPartner.id}/users`, {
      headers,
      data: { email: isolatedEmail, name: 'Isolated Partner Admin', role: 'admin' },
    })
    expect(isolatedUserResponse.ok()).toBeTruthy()
    const isolatedUser = (await isolatedUserResponse.json()) as { temp_password: string }
    const isolatedLoginResponse = await request.post('/api/partner/auth/login', {
      data: { email: isolatedEmail, password: isolatedUser.temp_password },
    })
    expect(isolatedLoginResponse.ok()).toBeTruthy()
    const isolatedLogin = (await isolatedLoginResponse.json()) as { token: string }
    const isolatedHeaders = { Authorization: `Bearer ${isolatedLogin.token}` }
    const isolatedPasswordResponse = await request.post('/api/partner/auth/password', {
      headers: isolatedHeaders,
      data: { new_password: 'IsolatedPartner-2026!' },
    })
    expect(isolatedPasswordResponse.ok()).toBeTruthy()
    const isolatedSponsorResponse = await request.post('/api/partner/candidates', {
      headers: isolatedHeaders,
      data: { candidates: [{ email: isolatedCandidateEmail, first_name: 'Isolated', last_name: 'Candidate', certification: 'PML-AI' }] },
    })
    expect(isolatedSponsorResponse.ok()).toBeTruthy()

    const [primaryCandidateResponse, isolatedCandidateResponse, isolatedCodesResponse] = await Promise.all([
      request.get('/api/partner/candidates', { headers: { Authorization: `Bearer ${partnerToken}` } }),
      request.get('/api/partner/candidates', { headers: isolatedHeaders }),
      request.get('/api/partner/codes', { headers: isolatedHeaders }),
    ])
    expect(primaryCandidateResponse.ok()).toBeTruthy()
    expect(isolatedCandidateResponse.ok()).toBeTruthy()
    expect(isolatedCodesResponse.ok()).toBeTruthy()
    const primaryCandidateRows = ((await primaryCandidateResponse.json()) as { rows: Array<{ candidate_email: string }> }).rows
    const isolatedCandidateRows = ((await isolatedCandidateResponse.json()) as { rows: Array<{ candidate_email: string }> }).rows
    const isolatedCodeRows = ((await isolatedCodesResponse.json()) as { rows: Array<{ code: string }> }).rows
    expect(primaryCandidateRows.map((row) => row.candidate_email)).toContain(candidateEmail)
    expect(primaryCandidateRows.map((row) => row.candidate_email)).not.toContain(isolatedCandidateEmail)
    expect(isolatedCandidateRows.map((row) => row.candidate_email)).toContain(isolatedCandidateEmail)
    expect(isolatedCandidateRows.map((row) => row.candidate_email)).not.toContain(candidateEmail)
    expect(isolatedCodeRows.map((row) => row.code)).not.toContain(code)

    await page.getByRole('button', { name: 'Sign out' }).click()
    await expect(page.locator('#viewLogin')).toBeVisible()
    await page.getByLabel('Email', { exact: true }).fill(partnerEmail)
    await page.getByLabel('Password', { exact: true }).fill(newPassword)
    await page.getByRole('button', { name: 'Sign in', exact: true }).click()
    await expect(page.locator('#app')).toBeVisible()
    await expect(page.getByRole('heading', { name: 'Set a new password' })).toBeHidden()
  })
})
