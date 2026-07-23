import { test, expect, type APIRequestContext } from '@playwright/test'
import { apiLoginAsE2EAdmin, captureStoryEvidence, createTestUser, type TestUserScenario } from './util'

interface JourneyStage {
  key: string
  label: string
  status: 'done' | 'pending' | 'blocked' | 'action_required'
  detail: string
}

interface Journey {
  user: { id: number; email: string; is_test: boolean }
  stages: JourneyStage[]
  stuck_at?: string | null
  membership_status: string
}

async function journeyFor(request: APIRequestContext, adminToken: string, userId: number): Promise<Journey> {
  const response = await request.get(`/api/admin/members/${userId}/journey`, {
    headers: { Authorization: `Bearer ${adminToken}` },
  })
  expect(response.ok(), `journey probe should succeed (got ${response.status()})`).toBeTruthy()
  return response.json() as Promise<Journey>
}

function stagesByKey(journey: Journey): Record<string, JourneyStage['status']> {
  return Object.fromEntries(journey.stages.map((stage) => [stage.key, stage.status]))
}

test.describe('admin-controlled test-user journeys', () => {
  test('all seven scenarios stop at the intended stage and stay excluded as test data', async ({ request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)

    // The failed-integration scenario is meaningful only when the operator has enabled Certuvo.
    // Configure a harmless sign-in URL; provisioning is never called for the prebuilt failure row.
    const certuvoConfig = await request.post('/api/admin/certuvo', {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: { enabled: true, login_url: 'https://certuvo.example.test/login' },
    })
    expect(certuvoConfig.ok()).toBeTruthy()

    const expected: Record<TestUserScenario, {
      stuck: string
      stages: Partial<Record<string, JourneyStage['status']>>
    }> = {
      ready: {
        stuck: 'Exam scheduled',
        stages: { account: 'done', consents: 'done', profile: 'done', identity: 'done', membership: 'done', payment: 'done', booking: 'action_required' },
      },
      unpaid: {
        stuck: 'Consents',
        stages: { account: 'done', consents: 'action_required', profile: 'action_required', identity: 'blocked', payment: 'blocked' },
      },
      member: {
        stuck: 'Exam fee',
        stages: { account: 'done', consents: 'done', profile: 'done', identity: 'done', membership: 'done', payment: 'blocked' },
      },
      waived: {
        stuck: 'Exam scheduled',
        stages: { account: 'done', consents: 'done', profile: 'done', identity: 'done', payment: 'done', booking: 'action_required' },
      },
      incomplete_profile: {
        stuck: 'Profile',
        stages: { account: 'done', consents: 'done', profile: 'action_required', identity: 'done', membership: 'done', payment: 'done', booking: 'pending' },
      },
      no_id: {
        stuck: 'Government ID',
        stages: { account: 'done', consents: 'done', profile: 'done', identity: 'blocked', membership: 'done', payment: 'done', booking: 'pending' },
      },
      certuvo_failed: {
        stuck: 'Exam fee',
        stages: { account: 'done', membership: 'done', payment: 'blocked', certuvo: 'action_required' },
      },
    }

    for (const scenario of Object.keys(expected) as TestUserScenario[]) {
      const user = await createTestUser(request, adminToken, scenario)
      const journey = await journeyFor(request, adminToken, user.id)
      expect(journey.user).toMatchObject({ id: user.id, email: user.email, is_test: true })
      expect(journey.stuck_at, `${scenario} should stop at its intended first actionable stage`).toBe(expected[scenario].stuck)
      expect(stagesByKey(journey), `${scenario} should isolate the documented state`).toMatchObject(expected[scenario].stages)
    }
  })

  test('ready PML-AI candidate can schedule and is marked enrolled', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'ready', page)

    await page.goto('/app/certifications')
    await expect(page.getByRole('heading', { name: 'PCI Project Management Leader – AI' }).first()).toBeVisible()
    await expect(page.getByText('PML-AI', { exact: true }).first()).toBeVisible()
    await expect(page.getByText('Enrolled', { exact: true })).toBeVisible()
    await expect(page.getByRole('button', { name: 'Schedule exam', exact: true }).first()).toBeEnabled()
    await captureStoryEvidence(page, testInfo, 'C1', 'ready-test-user')
  })

  test('waived candidate sees the waiver and can schedule without a checkout', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'waived', page)

    await page.goto('/app/certifications')
    await expect(page.getByText('A fee waiver has been applied by the institute.')).toBeVisible()
    await expect(page.getByRole('button', { name: 'Schedule exam', exact: true }).first()).toBeEnabled()
  })

  test('profile-only blocker disables scheduling without asking for another ID or payment', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'incomplete_profile', page)

    await page.goto('/app/certifications')
    await expect(page.getByText(/complete your profile \(country is required\)/i)).toBeVisible()
    await expect(page.getByRole('button', { name: 'Schedule exam', exact: true }).first()).toBeDisabled()
    await expect(page.getByText(/upload your government-issued ID/i)).toHaveCount(0)
    await captureStoryEvidence(page, testInfo, 'D1', 'profile-only-blocker')
  })

  test('ID-only blocker offers upload and keeps the paid entitlement visible', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'no_id', page)

    await page.goto('/app/certifications')
    await expect(page.getByRole('heading', { name: 'Identity verification' })).toBeVisible()
    await expect(page.getByLabel(/File \(JPG, PNG, WEBP or PDF/i)).toBeVisible()
    await expect(page.getByText(/upload your government-issued ID/i)).toBeVisible()
    await expect(page.getByRole('button', { name: 'Schedule exam', exact: true }).first()).toBeDisabled()
    await captureStoryEvidence(page, testInfo, 'D1-D2', 'id-only-blocker')
  })

  test('Certuvo failure is friendly to the member while the admin journey keeps diagnostics', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const config = await request.post('/api/admin/certuvo', {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: { enabled: true, login_url: 'https://certuvo.example.test/login' },
    })
    expect(config.ok()).toBeTruthy()
    const user = await createTestUser(request, adminToken, 'certuvo_failed', page)

    await page.goto('/app/certuvo')
    await expect(page.getByRole('heading', { name: 'Certuvo' }).first()).toBeVisible()
    await expect(page.getByText(/still setting up your Certuvo practice access/i)).toBeVisible()
    await expect(page.getByText(/Simulated provisioning failure/i)).toHaveCount(0)

    const journey = await journeyFor(request, adminToken, user.id)
    const certuvo = journey.stages.find((stage) => stage.key === 'certuvo')
    expect(certuvo).toMatchObject({ status: 'action_required' })
    expect(certuvo?.detail).toContain('Simulated provisioning failure')
  })
})
