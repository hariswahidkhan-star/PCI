import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { demoAnswers } from './demo-exam'
import { captureStoryEvidence, settleExamPurchase, uniqueEmail } from './util'

const tinyPng = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=='

/** Drive the portal's scheduling window (month calendar + slot grid): find the day `daysAhead`
 * days out — advancing months until it appears ENABLED, so this needs no clone of the window's
 * own initial-month rule — click it, then the slot. A past day with the same number is disabled
 * and therefore never matched. A slot `daysAhead` days out at 09:00 is always ≥ (24·daysAhead −
 * 15) h away, so daysAhead = 4 and 5 both clear the seeded 72 h reschedule cutoff whatever the
 * wall-clock time. */
async function pickCalendarSlot(page: import('@playwright/test').Page, daysAhead: number, slot: string): Promise<void> {
  const targetDay = await page.evaluate((days) => new Date(Date.now() + days * 86_400_000).getDate(), daysAhead)
  const dayButton = page.locator('.schedm-day:not([disabled])').filter({ hasText: new RegExp(`^${targetDay}$`) })
  for (let hops = 0; hops < 2 && (await dayButton.count()) === 0; hops += 1)
    await page.getByRole('button', { name: 'Next month', exact: true }).click()
  await dayButton.click()
  await page.getByRole('button', { name: slot, exact: true }).click()
}

test.describe('complete certification lifecycle', () => {
  test('student registers, pays, schedules, sits, passes, verifies and downloads', async ({ page, request }, testInfo) => {
    test.slow()
    const email = uniqueEmail('lifecycle')
    await page.goto('/app/register')
    await page.getByLabel('First name').fill('Browser')
    await page.getByLabel('Last name').fill('Lifecycle')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password (8+ characters)').fill('e2e-pass-123')
    await page.getByLabel('Confirm password').fill('e2e-pass-123')
    await page.locator('#rcountry').selectOption({ label: 'United Kingdom' })
    await page.locator('#rphone').fill('7700900002')
    await page.getByRole('button', { name: /create free account/i }).click()
    await expect(page).toHaveURL(/\/app\/onboarding$/)
    await page.getByRole('link', { name: /skip for now/i }).click()

    const token = await page.evaluate(() => sessionStorage.getItem('pci.session.token'))
    expect(token, 'registration should create a browser session').toBeTruthy()
    const headers = { Authorization: `Bearer ${token}` }

    // The Stripe event traverses the real signature-validation and provisioning path. Candidate-only
    // prerequisites use authenticated APIs so this test can spend its browser time on the journeys
    // that need actual rendering and interaction (registration, booking, sitting and credentials).
    await settleExamPurchase(request, email, 'PCL-AI', 'bundle')
    for (const [label, response] of [
      ['consents', await request.post('/api/me/consents', { headers, data: { accept_all: true } })],
      ['profile', await request.patch('/api/me/profile', { headers, data: { country: 'United Kingdom', city: 'London' } })],
      ['identity', await request.post('/api/me/identity-document', { headers, data: { doc_kind: 'passport', filename: 'id.png', data_uri: tinyPng } })],
    ] as const)
      expect(response.ok(), `${label} prerequisite should succeed (got ${response.status()})`).toBeTruthy()

    // Book and then reschedule through the React portal. Seeded sp_reschedule_cutoff_hours is 72, so
    // both slots must sit beyond that window or /reschedule returns { error: "locked" }.
    await page.goto('/app/certifications')
    const schedule = page.getByRole('button', { name: 'Schedule exam', exact: true })
    await expect(schedule).toBeEnabled()
    await schedule.click()
    await pickCalendarSlot(page, 4, '09:00')
    const bookedResponse = page.waitForResponse((r) => r.url().endsWith('/api/me/exam/book') && r.request().method() === 'POST')
    await page.getByRole('button', { name: 'Confirm slot', exact: true }).click()
    expect((await bookedResponse).ok()).toBeTruthy()
    await expect(page.getByText(/Your exam is scheduled for/i)).toBeVisible()

    await page.getByRole('button', { name: 'Reschedule', exact: true }).click()
    await pickCalendarSlot(page, 5, '14:00')
    const rescheduledResponse = page.waitForResponse((r) => r.url().endsWith('/api/me/exam/reschedule') && r.request().method() === 'POST')
    await page.getByRole('button', { name: 'Confirm slot', exact: true }).click()
    const rescheduled = await rescheduledResponse
    expect(rescheduled.ok(), `reschedule should succeed (got ${rescheduled.status()}: ${await rescheduled.text()})`).toBeTruthy()
    expect((await rescheduled.json()) as { reschedule_count?: number }).toMatchObject({ reschedule_count: 1 })
    await captureStoryEvidence(page, testInfo, 'D3-D4', 'booked-and-rescheduled')

    const readiness = await request.post('/api/me/readiness', {
      headers,
      data: { camera: true, microphone: true, network: true, fullscreen: true, environment: true, browser: 'Playwright', screen: '1280x720' },
    })
    expect(readiness.ok(), `readiness should succeed (got ${readiness.status()})`).toBeTruthy()

    // Sit the real seeded examination in the legacy secure browser runner. The seed is opt-in in the
    // Playwright server only; production never loads these published demo answers.
    await page.goto(`/student.html#t=${token}`)
    await expect(page.locator('#app')).toBeVisible()
    await page.locator('#nav a[data-id="exam"]').click()
    // Wait for the booked view to finish painting — `#ckl` is absent until `renderBooked` runs
    // (after the readiness GET). Checking too early makes `.all()` return [] and leaves Launch disabled.
    const boxes = page.locator('#ckl input[type="checkbox"]')
    await expect(boxes).toHaveCount(4, { timeout: 15_000 })
    for (let i = 0; i < 4; i += 1) await boxes.nth(i).check()
    // Mirror into the shell's S.check store so the 1s paint timer cannot miss a late change event.
    await page.evaluate(() => {
      const root = (window as unknown as { S?: { check: Record<string, boolean> } }).S
        ?? (document.querySelector('#app') as unknown as { __s?: { check: Record<string, boolean> } })
      // student.html keeps S in module scope; drive the wired inputs' change handlers instead.
      document.querySelectorAll<HTMLInputElement>('#ckl input[type="checkbox"]').forEach((el) => {
        if (!el.checked) el.checked = true
        el.dispatchEvent(new Event('change', { bubbles: true }))
      })
      void root
    })
    const launch = page.locator('#launchGo')
    await expect(launch).toBeEnabled({ timeout: 10_000 })
    await launch.click()
    await expect(page.locator('.qcard h3')).toBeVisible()

    const itemCount = await page.locator('.pgrid button').count()
    expect(itemCount).toBe(demoAnswers.size)
    for (let i = 0; i < itemCount; i += 1) {
      const question = (await page.locator('.qcard h3').innerText()).trim()
      const answer = demoAnswers.get(question)
      expect(answer, `demo answer should exist for question ${i + 1}: ${question}`).toBeDefined()
      await page.locator('.opt').nth(answer!).click()
      if (i < itemCount - 1) await page.locator('#rNext').click()
    }
    await page.locator('#rSubmit').click()
    await page.locator('#mOK').waitFor({ state: 'attached', timeout: 15_000 })
    // Modal animation / overlay makes Playwright's actionability click flaky; fire the bound handler
    // directly and wait for the real submit round-trip before asserting the PASS verdict.
    const examSubmit = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/exam/submit') && response.request().method() === 'POST', { timeout: 60_000 })
    await page.evaluate(() => {
      const btn = document.getElementById('mOK') as HTMLButtonElement | null
      if (btn) btn.click()
    })
    const submitted = await examSubmit
    expect(submitted.ok(), `exam submit should succeed (got ${submitted.status()})`).toBeTruthy()
    await expect(page.locator('.verdict')).toHaveText(/PASS/i, { timeout: 60_000 })
    await captureStoryEvidence(page, testInfo, 'D5-D6', 'exam-pass')
    const issued = await page.locator('.tag.gold').innerText()
    const credential = issued.match(/PCI-PCLAI-\d{4}-\d+/)?.[0]
    expect(credential, `result should expose the issued credential (got ${issued})`).toBeTruthy()

    await page.locator('#rvCert').click()
    await expect(page.locator('#certWrap')).toContainText('PCI AI Project Controls Leader')
    await expect(page.locator('#certWrap')).toContainText(credential!)

    // Download the authoritative server-generated certificate from the React credential screen and
    // verify the browser received a real PDF, not a placeholder HTML print view.
    await page.goto('/app/credentials')
    await expect(page.getByText(credential!, { exact: true })).toBeVisible()
    const downloadPromise = page.waitForEvent('download')
    await page.getByRole('button', { name: 'Download PDF', exact: true }).click()
    const download = await downloadPromise
    expect(download.suggestedFilename()).toContain(credential!)
    const downloadedPath = await download.path()
    expect(downloadedPath).toBeTruthy()
    expect(readFileSync(downloadedPath!).subarray(0, 4).toString()).toBe('%PDF')

    await page.goto(`/verify.html?id=${encodeURIComponent(credential!)}`)
    await expect(page.locator('#vres')).toContainText('Active & in good standing')
    await expect(page.locator('#vres')).toContainText(credential!)
    await expect(page.locator('#vres')).toContainText('PCI AI Project Controls Leader')
    await captureStoryEvidence(page, testInfo, 'A9-E1', 'credential-verified')
  })
})
