import { test, expect } from '@playwright/test'
import {
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  preparePublicJourney,
  registerStudent,
  totpCode,
  uniqueEmail,
} from './util'

test.describe('student account recovery, onboarding and security', () => {
  test('forgot-password is non-enumerating and records delivery for a real account', async ({ page, request }, testInfo) => {
    const student = await registerStudent(request, undefined, 'forgot')
    const adminToken = await apiLoginAsE2EAdmin(request)
    await preparePublicJourney(page)

    const submit = async (email: string) => {
      await page.goto('/forgot-password.html')
      await page.getByLabel('Email', { exact: true }).fill(email)
      const responsePromise = page.waitForResponse((response) =>
        response.url().endsWith('/api/forgot-password') && response.request().method() === 'POST')
      await page.getByRole('button', { name: 'Send reset link' }).click()
      const response = await responsePromise
      expect(response.ok()).toBeTruthy()
      await expect(page.locator('#forgotMsg')).toContainText('If an account exists')
      const text = (await page.locator('#forgotMsg').innerText()).replace(email, '<email>')
      return text
    }

    const realMessage = await submit(student.email)
    const unknownMessage = await submit(uniqueEmail('forgot-unknown'))
    expect(unknownMessage).toBe(realMessage)

    const deliveries = await request.get(`/api/admin/emails?q=${encodeURIComponent(student.email)}`, {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(deliveries.ok()).toBeTruthy()
    const rows = ((await deliveries.json()) as { rows: Array<{ email_type?: string; email?: string }> }).rows
    expect(rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ email: student.email, email_type: 'password_reset' }),
    ]))
    await captureStoryEvidence(page, testInfo, 'B4', 'non-enumerating-forgot-password')
  })

  test('admin-provisioned setup link sets a password once and reaches the real portal login', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const email = uniqueEmail('admin-provisioned')
    const createdResponse = await request.post('/api/admin/members', {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: { email, first_name: 'Provisioned', last_name: 'Candidate' },
    })
    expect(createdResponse.ok()).toBeTruthy()
    const created = (await createdResponse.json()) as { id: number; setup_url: string }
    expect(created.id).toBeGreaterThan(0)
    const setup = new URL(created.setup_url)
    const setupPath = setup.pathname + setup.search
    const password = 'ProvisionedBrowser-2026!'

    await preparePublicJourney(page)
    await page.goto(setupPath)
    await page.getByLabel('New password', { exact: true }).fill(password)
    await page.getByLabel('Confirm new password', { exact: true }).fill('does-not-match')
    await page.getByRole('button', { name: 'Reset password' }).click()
    await expect(page.locator('#resetMsg')).toContainText('The two passwords do not match')

    await page.getByLabel('Confirm new password', { exact: true }).fill(password)
    const setResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/set-password') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Reset password' }).click()
    expect((await setResponsePromise).ok()).toBeTruthy()
    await expect(page.locator('#resetMsg')).toContainText('Your password has been set')
    await captureStoryEvidence(page, testInfo, 'B5', 'password-set')
    await expect(page).toHaveURL(/\/app\/login$/, { timeout: 5_000 })

    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password', { exact: true }).fill(password)
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()

    // The same token is single-use and cannot replace the password a second time.
    await page.goto(setupPath)
    await page.getByLabel('New password', { exact: true }).fill('ReplacementBrowser-2026!')
    await page.getByLabel('Confirm new password', { exact: true }).fill('ReplacementBrowser-2026!')
    await page.getByRole('button', { name: 'Reset password' }).click()
    await expect(page.locator('#resetMsg')).toContainText('invalid_or_expired_token')
  })

  test('a candidate completes every onboarding step and reaches a 100% profile', async ({ page }, testInfo) => {
    const email = uniqueEmail('onboarding')
    await page.goto('/app/register')
    await page.getByLabel('First name').fill('Journey')
    await page.getByLabel('Last name').fill('Candidate')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password (8+ characters)').fill('OnboardingBrowser-2026!')
    await page.getByLabel('Confirm password').fill('OnboardingBrowser-2026!')
    await page.locator('#rcountry').selectOption({ label: 'United Kingdom' })
    await page.locator('#rphone').fill('7700900001')
    await page.getByRole('button', { name: /create free account/i }).click()

    await expect(page).toHaveURL(/\/app\/onboarding$/)
    await page.getByRole('button', { name: /let.*go/i }).click()
    await page.locator('#ob_country').selectOption({ label: 'United Kingdom' })
    await page.locator('#ob_city').fill('London')
    await page.locator('#ob_mobile').fill('+44 7700 900001')
    await page.locator('#ob_current_role').fill('Project Controls Lead')
    await page.locator('#ob_company').fill('Browser Controls Ltd')
    await page.locator('#ob_industry_sector').fill('Infrastructure')
    await page.locator('#ob_years_experience').fill('8')
    await page.locator('#ob_project_controls_area').fill('Planning and cost control')
    await page.locator('#ob_highest_qualification').fill('BSc Engineering')
    await page.locator('#ob_linkedin_url').fill('https://www.linkedin.com/in/browser-candidate')
    await page.getByRole('button', { name: 'Save & continue' }).click()

    await page.getByRole('button', { name: /add experience/i }).first().click()
    await page.getByLabel('Company / organisation *').fill('Browser Controls Ltd')
    await page.getByLabel('Job title *').fill('Project Controls Lead')
    await page.getByLabel('Start').fill('2018-01')
    await page.getByLabel('I currently work here').check()
    await page.getByRole('button', { name: 'Add', exact: true }).click()
    await expect(page.getByText('Project Controls Lead — Browser Controls Ltd')).toBeVisible()
    await page.getByRole('button', { name: 'Continue', exact: true }).click()

    await page.getByRole('button', { name: /add qualification/i }).first().click()
    await page.getByLabel('Institution *').fill('Browser University')
    await page.getByLabel('Degree / award *').fill('BSc Engineering')
    await page.getByLabel('Field of study').fill('Civil Engineering')
    await page.getByLabel('Year completed').fill('2017')
    await page.getByRole('button', { name: 'Add', exact: true }).click()
    await expect(page.getByText('BSc Engineering', { exact: true })).toBeVisible()
    await page.getByRole('button', { name: 'Continue', exact: true }).click()

    await page.getByRole('button', { name: /add certification/i }).first().click()
    await page.getByLabel('Certification *').fill('PMP')
    await page.getByLabel('Issuing body').fill('PMI')
    await page.getByLabel('Year obtained').fill('2020')
    await page.getByRole('button', { name: 'Add', exact: true }).click()
    await expect(page.getByText('PMP', { exact: true })).toBeVisible()
    await page.getByRole('button', { name: 'Continue', exact: true }).click()

    await expect(page.getByRole('heading', { name: 'Your profile is live' })).toBeVisible()
    await expect(page.getByRole('img', { name: 'complete: 100%' })).toBeVisible()
    await captureStoryEvidence(page, testInfo, 'B7', 'onboarding-complete')
    await page.getByRole('button', { name: 'Go to my dashboard' }).click()
    await expect(page).toHaveURL(/\/app\/?$/)
    await expect(page.getByText(email).first()).toBeVisible()
  })

  test('student enables TOTP, signs in with it, then disables it using a recovery code', async ({ page, request }, testInfo) => {
    test.slow()
    const student = await registerStudent(request, page, 'student-totp')
    await page.goto('/app/profile')
    await expect(page.getByRole('heading', { name: 'Profile' })).toBeVisible()

    const setupResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/2fa/setup') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Set up 2FA' }).click()
    const setupResponse = await setupResponsePromise
    expect(setupResponse.ok()).toBeTruthy()
    const setup = (await setupResponse.json()) as { secret: string }
    await expect(page.getByText(setup.secret, { exact: true })).toBeVisible()

    const verifyResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/2fa/verify') && response.request().method() === 'POST')
    await page.getByPlaceholder('6-digit code').fill(totpCode(setup.secret))
    await page.getByRole('button', { name: 'Verify & enable' }).click()
    const verifyResponse = await verifyResponsePromise
    expect(verifyResponse.ok()).toBeTruthy()
    const verified = (await verifyResponse.json()) as { recovery_codes: string[] }
    expect(verified.recovery_codes).toHaveLength(10)
    await expect(page.getByText('2FA is on. Save your recovery codes now.')).toBeVisible()

    await page.getByRole('button', { name: 'Sign out', exact: true }).click()
    await page.getByLabel('Email address').fill(student.email)
    await page.getByLabel('Password', { exact: true }).fill(student.password)
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByLabel('Authentication code')).toBeVisible()
    await page.getByLabel('Authentication code').fill('000000')
    await page.getByRole('button', { name: 'Verify & sign in' }).click()
    await expect(page.getByRole('alert')).toContainText('not valid')
    await page.getByLabel('Authentication code').fill(totpCode(setup.secret))
    await page.getByRole('button', { name: 'Verify & sign in' }).click()
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()

    await page.goto('/app/profile')
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible({ timeout: 30_000 })
    await expect(page.getByRole('heading', { name: 'Two-factor authentication (2FA)' })).toBeVisible({ timeout: 30_000 })
    await expect(page.getByRole('button', { name: 'Turn off 2FA' })).toBeVisible({ timeout: 30_000 })
    page.once('dialog', async (dialog) => {
      expect(dialog.type()).toBe('prompt')
      await dialog.accept(verified.recovery_codes[0])
    })
    await page.getByRole('button', { name: 'Turn off 2FA' }).click()
    await expect(page.getByRole('button', { name: 'Set up 2FA' })).toBeVisible({ timeout: 30_000 })
    await captureStoryEvidence(page, testInfo, 'F5', 'student-totp-lifecycle')

    await page.getByRole('button', { name: 'Sign out', exact: true }).click()
    await page.getByLabel('Email address').fill(student.email)
    await page.getByLabel('Password', { exact: true }).fill(student.password)
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()
    await expect(page.getByLabel('Authentication code')).toHaveCount(0)
  })
})
