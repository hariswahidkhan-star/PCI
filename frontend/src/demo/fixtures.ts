import { Rand, CERTS, person, ref } from './synth'

/* Hand-built fixtures for the payloads whose SHAPE the UI genuinely depends on.
 *
 * Everything else is synthesised from column names (synth.ts). These are the ones where a
 * generic guess would produce a broken or misleading screen: the student `me` envelope drives
 * the entire portal, the admin overview drives the dashboard's KPIs and charts, and the
 * readiness payload has to look like a real deployment report rather than random booleans.
 *
 * The candidate is deliberately mid-journey — membership active, exam fee paid, exam booked,
 * one credential already held — because a demo of an empty account shows none of the product. */

const ME_PERSON = { first_name: 'Amara', last_name: 'Okafor', email: 'amara.okafor@example.org' }

export function studentMe() {
  return {
    user: {
      id: 1,
      ...ME_PERSON,
      registration_no: 'PCI-100482',
      created_at: '2026-01-04T09:12:00Z',
      status: 'active',
      impersonated: false,
    },
    profile: {
      current_role: 'Senior Planner',
      company: 'Meridian Infrastructure',
      country: 'United Kingdom',
      city: 'Manchester',
      industry_sector: 'Rail',
      years_experience: 9,
      highest_qualification: 'MSc Construction Management',
      project_controls_area: 'Planning & scheduling',
      profile_completion_percentage: 85,
    },
    lifecycle: {
      membership_status: 'active',
      candidate_status: 'exam_fee_paid',
      blocking_items: [],
      exam_status: 'booked',
      result_status: null,
      credential_status: 'active',
      next_step: 'prepare_launch',
    },
    credentials: [
      {
        id: 1,
        credential_id: 'PCL-AI-2025-0311',
        credential: 'PCL-AI',
        certification_name: 'PCI Project Controls Leader – AI',
        holder_name: 'Amara Okafor',
        status: 'active',
        issued_at: '2025-11-18T00:00:00Z',
        expires_at: '2028-11-18T00:00:00Z',
      },
    ],
    exams: [
      {
        id: 1,
        certification_code: 'PML-AI',
        certification_name: 'PCI Project Management Leader – AI',
        entitlement_status: 'available',
        booked_at: '2026-08-14T09:00:00Z',
        expires_at: '2026-12-31T00:00:00Z',
      },
    ],
    attempts: [
      { id: 3, kind: 'mock_exam', status: 'submitted', result_status: 'pass', started_at: '2026-06-02T10:00:00Z', submitted_at: '2026-06-02T12:05:00Z' },
      { id: 2, kind: 'practice', status: 'submitted', result_status: 'pass', started_at: '2026-05-19T14:00:00Z', submitted_at: '2026-05-19T14:48:00Z' },
      { id: 1, kind: 'practice', status: 'submitted', result_status: 'fail', started_at: '2026-04-27T18:00:00Z', submitted_at: '2026-04-27T18:52:00Z' },
    ],
    tickets: [
      { id: 1, reference: 'TKT-4471C2', subject: 'Rescheduling my exam window', status: 'awaiting_student', updated_at: '2026-06-20T11:02:00Z', msg_count: 3 },
    ],
    experiences: [
      { id: 1, title: 'Senior Planner', company: 'Meridian Infrastructure', start_date: '2022-03', end_date: null, is_current: true, country: 'United Kingdom', summary: 'Schedule ownership across a £400m rail upgrade programme.' },
      { id: 2, title: 'Project Planner', company: 'Northgate Rail', start_date: '2018-09', end_date: '2022-02', is_current: false, country: 'United Kingdom' },
    ],
    qualifications: [
      { id: 1, degree: 'MSc Construction Management', institution: 'University of Manchester', year_completed: 2018, country: 'United Kingdom' },
    ],
    certifications_held: [
      { id: 1, name: 'PRINCE2 Practitioner', issuer: 'PeopleCert', issued_year: 2020, expires_year: 2026 },
    ],
    membership: {
      id: 1, membership_type: 'founding', status: 'active',
      start_date: '2026-01-04', expiry_date: '2027-01-04', auto_renew: 1,
    },
    payments: [
      { id: 3, product_type: 'exam', final_amount: 0, currency: 'USD', payment_status: 'paid', payment_date: '2026-05-30T10:12:00Z', reference: 'PCI-PAY-7741B2', exam_schedule_deadline: '2026-12-31T00:00:00Z' },
      { id: 2, product_type: 'membership', final_amount: 0, currency: 'USD', payment_status: 'paid', payment_date: '2026-01-04T09:15:00Z', reference: 'PCI-PAY-5510A9' },
      { id: 1, product_type: 'study', final_amount: 0, currency: 'USD', payment_status: 'paid', payment_date: '2026-01-04T09:15:00Z', reference: 'PCI-PAY-5510A8' },
    ],
    identity_document: { id: 1, doc_kind: 'passport', filename: 'identity.pdf', mime: 'application/pdf', size_bytes: 184320, status: 'verified', created_at: '2026-01-06T10:00:00Z' },
    cpd: { total: 42, target: 60, pending: 3 },
    consents: { outstanding: [] },
    unread: 4,
    two_factor: false,
    two_factor_coming_soon: true,
    enrollment: null,
    site_base_url: 'https://pci-platform.onrender.com',
    referral: { code: 'AMARA-2026' },
    membership_grade: {
      current: 'member', label: 'Member', post_nominal: 'MPCI', rank: 2,
      eligible_upgrade: { key: 'fellow', label: 'Fellow', post_nominal: 'FPCI' },
      can_apply_fellow: true, pending_application: null,
    },
    membership_dues: { available: true, subscribed: true, status: 'active', cancel_at_period_end: false },
    founding_member: true,
    honorary: [],
  }
}

export function studentMessages() {
  return {
    rows: [
      { id: 5, category: 'Support', title: 'Support replied to your ticket', body: 'We have reviewed your rescheduling request and your exam window is now open until 31 December.', cta_label: 'View reply', cta_route: '/support', created_at: '2026-06-20T11:02:00Z', read_at: null },
      { id: 4, category: 'Documents', title: 'New document available', body: 'Your certificate of membership has been issued to My Documents.', cta_label: 'Open My Documents', cta_route: '/documents', created_at: '2026-06-11T08:30:00Z', read_at: null },
      { id: 3, category: 'Exam', title: 'Exam booking confirmed', body: 'Your PML-AI examination is booked for 14 August 2026, 09:00 UTC.', cta_label: 'View certifications', cta_route: '/certifications', created_at: '2026-06-02T16:20:00Z', read_at: null },
      { id: 2, category: 'Practice', title: 'Mock examination marked', body: 'You passed the PML-AI mock examination with 78%.', cta_label: 'Open Certuvo', cta_route: '/certuvo', created_at: '2026-06-02T12:40:00Z', read_at: null },
      { id: 1, category: 'Founding', title: 'Founding access granted', body: 'Your founding code has been applied — membership, study and exam fees are waived.', created_at: '2026-01-04T09:20:00Z', read_at: '2026-01-04T10:00:00Z' },
    ],
  }
}

/* The PCI World Passport panel on the student dashboard. This one MUST be an explicit fixture:
 * PassportCard reads `evidenceHighlights.length` unconditionally, so the generic `{ok, rows}`
 * answer takes the whole portal down behind the error boundary. */
export function passportSummary() {
  return {
    schemaVersion: 1,
    studentNumber: 'PCI-100482',
    displayName: 'Amara Okafor',
    participantState: 'active',
    passportState: 'published',
    completedChallengeCount: 27,
    selectedEvidenceCount: 3,
    evidenceHighlights: [
      { title: 'Recovering a slipped critical path', reference: 'WC-2026-188', industry: 'Rail', track: 'Schedule', difficulty: 'intermediate', score: 88, profile: 'Planner', completedOn: '18 Jun 2026' },
      { title: 'Rebaselining after a scope change', reference: 'WC-2026-142', industry: 'Energy', track: 'Cost', difficulty: 'advanced', score: 81, profile: 'Cost engineer', completedOn: '2 May 2026' },
      { title: 'Quantifying schedule risk with a three-point estimate', reference: 'WC-2026-097', industry: 'Marine', track: 'Risk', difficulty: 'intermediate', score: 92, profile: 'Risk analyst', completedOn: '21 Mar 2026' },
    ],
    disclosureSettings: { scores: true, profiles: true, dates: true },
    publicUrl: null,
    canShowPublicUrl: false,
    expiresAt: null,
    lastUpdatedAt: '2026-06-18T09:00:00Z',
    availableActions: ['view', 'share'],
  }
}

export function adminOverview() {
  const months = ['2025-08', '2025-09', '2025-10', '2025-11', '2025-12', '2026-01',
    '2026-02', '2026-03', '2026-04', '2026-05', '2026-06', '2026-07']
  const amounts = [10400, 12850, 11900, 15600, 14200, 19800, 21450, 20100, 24900, 23400, 28650, 31200]
  return {
    kpis: {
      members: 1204, activeMembers: 903, pendingMembers: 41, inProgress: 18, paidSessions: 612,
      abandoned: 23, payCount: 1558, revenue: 412900, revenueMonth: 31200, refunded: 2,
      failedPay: 4, credActive: 337, credTotal: 401, openInq: 12, examsDue: 37,
    },
    revSeries: months.map((ym, i) => ({ ym, amount: amounts[i], n: Math.round(amounts[i] / 260) })),
    productMix: [
      { product_type: 'exam', n: 620, amount: 248000 },
      { product_type: 'membership', n: 710, amount: 106500 },
      { product_type: 'bundle', n: 148, amount: 58400 },
    ],
    funnel: { started: 1480, in_progress: 18, paid: 612 },
    recent: Array.from({ length: 10 }, (_, i) => {
      const r = new Rand(`audit:${i}`)
      const p = person(r)
      return {
        ts: r.pastISO(40),
        action: r.pick(['cred_issue', 'payment_recorded', 'member_activated', 'exam_booked',
          'ticket_replied', 'code_created', 'settings_update', 'cpd_approved']),
        details: `${p.name} · ${ref(r, 'PCI')}`,
      }
    }),
  }
}

export function adminSystemCheck() {
  return {
    ok: false,
    environment: 'Production',
    checks: {
      stripe_configured: true, stripe_webhook_configured: true, smtp_configured: false,
      base_url_set: true, cors_locked: true, legacy_admin_token_disabled: true,
      persistent_db: false, owner_password_changed: true, migrations_applied: true,
      storage_local: true, storage_s3_misconfigured: false, stripe_webhook_path: '/api/webhook',
      stripe_webhook_url_ok: true, meta_leads_secret_configured: false, security_headers: true,
      csp_enforced: true, request_body_capped: true, retention_scheduled: true,
    },
    issues: [
      { sev: 'error', key: 'MYSQL_HOST', msg: 'MySQL is selected but connection settings are incomplete (need MYSQL_HOST + MYSQL_PASSWORD, or MYSQL_CONNECTION_STRING).' },
      { sev: 'warn', key: 'SMTP_HOST', msg: 'No mail provider configured — transactional email is written to the log instead of delivered.' },
    ],
  }
}

export function adminNotifications() {
  return {
    recipients: 'operations@example.org, registrar@example.org',
    admin_email: 'owner@example.org',
    resolved: ['operations@example.org', 'registrar@example.org', 'owner@example.org'],
    events: [
      { key: 'chat', label: 'Live chat requests' },
      { key: 'enrollment', label: 'Enrolments & payments' },
      { key: 'inquiry', label: 'Website inquiries' },
      { key: 'finance', label: 'Manual payments, waivers & mismatches' },
      { key: 'certuvo', label: 'Certuvo provisioning failures' },
      { key: 'partners', label: 'Institution allocation warnings' },
      { key: 'support', label: 'Escalated support cases' },
      { key: 'fraud', label: 'Suspicious discount activity' },
      { key: 'exam_exception', label: 'Exam exceptions & incident reports' },
      { key: 'exam_booking', label: 'Exam booking & reschedule confirmations' },
      { key: 'content', label: 'Blog & content publishing' },
    ],
    toggles: {
      chat: true, enrollment: true, inquiry: true, finance: true, certuvo: true, partners: false,
      support: true, fraud: true, exam_exception: true, exam_booking: false, content: false,
    },
    recent: [
      { channel: 'email', recipient: 'operations@example.org', subject: 'Exam exception raised — PCI-4471C2', status: 'sent', related_type: 'exam_exception', created_at: '2026-06-28T14:20:00Z' },
      { channel: 'email', recipient: 'registrar@example.org', subject: 'New enrolment payment received', status: 'sent', related_type: 'enrollment', created_at: '2026-06-28T09:05:00Z' },
      { channel: 'email', recipient: 'owner@example.org', subject: 'Discount code used 14 times in an hour', status: 'failed', related_type: 'fraud', created_at: '2026-06-27T22:41:00Z' },
    ],
    mail_configured: false,
  }
}

export function adminIdentityMerges() {
  return {
    ok: true,
    rows: [
      { id: 3, source_user_id: 811, target_user_id: 204, reason: 'Same person registered twice with work and personal email', status: 'pending', requested_by: 5, requested_at: '2026-06-25T10:14:00Z' },
      { id: 2, source_user_id: 640, target_user_id: 118, reason: 'Duplicate created during the founding import', status: 'approved', requested_by: 5, decided_by: 2, requested_at: '2026-05-02T08:00:00Z', decided_at: '2026-05-02T15:30:00Z', decision_note: 'Verified against passport on file.', correlation_id: 'cor_481502' },
      { id: 1, source_user_id: 512, target_user_id: 512, reason: 'Raised in error', status: 'rejected', requested_by: 2, decided_by: 5, requested_at: '2026-04-11T12:00:00Z', decided_at: '2026-04-11T12:20:00Z', decision_note: 'Same account on both sides.' },
    ],
  }
}

export function adminIdentityMergeDetail(id: number) {
  const rows = adminIdentityMerges().rows
  const request = rows.find((r) => r.id === id) ?? rows[0]
  return {
    ok: true,
    merge: {
      request,
      preview: {
        source: { payments: 2, exam_attempts: 1, issued_credentials: 0, cpd_entries: 4, tickets: 1 },
        target: { payments: 9, exam_attempts: 3, issued_credentials: 1, cpd_entries: 21, tickets: 2 },
      },
    },
  }
}

export function certifications() {
  return {
    rows: CERTS.map((c, i) => ({
      id: i + 1,
      code: c.code,
      acronym: c.code,
      name: c.name,
      description: 'A leadership credential examining applied project-controls judgement, assessed by proctored examination.',
      exam_price: [750, 750, 690, 690][i],
      currency: 'USD',
      duration_minutes: 180,
      question_count: 120,
      pass_mark: 70,
      published: 1,
    })),
  }
}

/** The admin member list, rich enough that search, sort and pagination all have something to do. */
export function adminStudents(total = 48) {
  const rows = Array.from({ length: total }, (_, i) => {
    const r = new Rand(`member:${i}`)
    const p = person(r)
    return {
      id: i + 1, ...p,
      status: r.pick(['active', 'active', 'active', 'pending', 'inactive']),
      created_at: r.pastISO(500),
      membership_type: r.pick(['member', 'founding', 'student']),
      membership_status: r.pick(['active', 'active', 'lapsed']),
      expiry_date: r.futureISO(700),
      profile: r.int(20, 100),
      paid_total: r.int(0, 9) * 250,
      credentials: r.int(0, 2),
      is_test: 0,
    }
  })
  return { rows, total }
}

/* ── PCI World ───────────────────────────────────────────────────────────────────────────────
 * World is a separate product with its own session carrier and its own client, and its screens
 * read deeply nested objects (`d.today.state`, `d.passport.state`, …) with no guards. A generic
 * `{ ok, rows }` answer is a crash on every one of them, so the World reads that matter are
 * spelled out here. Same participant as the portal fixture — Amara — so crossing between the two
 * surfaces tells one continuous story. */

const WORLD_CHALLENGES = [
  { code: 'WC-2026-188', title: 'Recovering a slipped critical path', track: 'Schedule', industry: 'Rail', difficulty: 'intermediate', score: 88 },
  { code: 'WC-2026-142', title: 'Rebaselining after a scope change', track: 'Cost', industry: 'Energy', difficulty: 'advanced', score: 81 },
  { code: 'WC-2026-097', title: 'Quantifying schedule risk with a three-point estimate', track: 'Risk', industry: 'Marine', difficulty: 'intermediate', score: 92 },
  { code: 'WC-2026-061', title: 'Reconciling an earned-value variance', track: 'Cost', industry: 'Infrastructure', difficulty: 'foundation', score: 76 },
  { code: 'WC-2026-034', title: 'Building a resource-levelled plan under a labour cap', track: 'Schedule', industry: 'Utilities', difficulty: 'advanced', score: 84 },
]

export function worldDashboard() {
  return {
    email: ME_PERSON.email,
    display_name: 'Amara Okafor',
    email_verified: true,
    primary_action: 'continue_today',
    primary_code: 'WC-2026-201',
    today: {
      day: '2026-07-29', timezone: 'Europe/London', paused: false,
      code: 'WC-2026-201', title: 'Closing out a contract with open change orders',
      difficulty: 'intermediate', est_minutes: 25, state: 'in_progress', attempt_id: 4417,
    },
    passport: { state: 'published', is_public: true, visible_evidence: 3 },
    progress: { completed: 27, industries: 6, tracks: 4 },
    streak_days: 12,
    week_done: 4,
    week_target: 5,
    onboarding_state: 'complete',
    products: { pci_world: { state: 'active' }, pci_ai: { state: 'available' } },
    recent: WORLD_CHALLENGES.slice(0, 3).map((c, i) => ({
      attempt_id: 4400 - i, code: c.code, title: c.title, score: c.score,
      completed_at: ['2026-07-27T18:22:00Z', '2026-07-24T08:05:00Z', '2026-07-21T19:40:00Z'][i],
    })),
    in_progress: [{ attempt_id: 4417, code: 'WC-2026-201', title: 'Closing out a contract with open change orders', updated_at: '2026-07-29T07:55:00Z' }],
    recommended: { code: 'WC-2026-155', title: 'Forecasting at completion when productivity is falling' },
  }
}

export function worldPassport() {
  const evidence = WORLD_CHALLENGES.map((c, i) => ({
    attempt_id: 4400 - i, title: c.title, code: c.code, version: 1,
    difficulty: c.difficulty, score: c.score,
    completed_at: ['2026-07-27T18:22:00Z', '2026-07-24T08:05:00Z', '2026-07-21T19:40:00Z',
      '2026-06-18T12:10:00Z', '2026-05-30T16:45:00Z'][i],
    passport_visible: i < 3,
  }))
  return {
    display_name: 'Amara Okafor',
    email_verified: true,
    passport_public: true,
    completed: 27, industries: 6, tracks: 4,
    evidence_total: evidence.length, evidence_offset: 0, evidence,
    show_scores: true, show_profiles: true, show_dates: true,
    expires_at: null, expired: false,
  }
}

export function worldShares() {
  return {
    results: WORLD_CHALLENGES.slice(0, 3).map((c, i) => ({
      attempt_id: 4400 - i, code: c.code, title: c.title,
      completed_at: ['2026-07-27T18:22:00Z', '2026-07-24T08:05:00Z', '2026-07-21T19:40:00Z'][i],
      created_at: ['2026-07-27T18:30:00Z', '2026-07-24T08:20:00Z', '2026-07-21T20:00:00Z'][i],
      revoked: i === 2,
    })),
    invitations: [
      { invite_id: 91, code: 'WC-2026-188', title: WORLD_CHALLENGES[0].title, version: 1, created_at: '2026-07-20T10:00:00Z', inviter_name: 'Daniel Osei', revoked: false },
      { invite_id: 88, code: 'WC-2026-097', title: WORLD_CHALLENGES[2].title, version: 1, created_at: '2026-06-11T14:30:00Z', inviter_name: null, revoked: true },
    ],
  }
}

export function worldPreferences() {
  return {
    linked: true,
    preferences: { goal: 'certification', timezone: 'Europe/London', weekly_target: 5, reminders_enabled: true, reminder_hour: 8 },
    goals: ['certification', 'interview', 'promotion', 'curiosity'],
  }
}

export function worldSessions() {
  return {
    rows: [
      { id: 3, created_at: '2026-07-29T07:40:00Z', expires_at: '2026-08-28T07:40:00Z', current: true },
      { id: 2, created_at: '2026-07-18T19:02:00Z', expires_at: '2026-08-17T19:02:00Z', current: false },
      { id: 1, created_at: '2026-06-30T09:15:00Z', expires_at: '2026-07-30T09:15:00Z', current: false },
    ],
  }
}

export function forumCategories() {
  return {
    categories: [
      { slug: 'schedule', title: 'Planning & scheduling', description: 'Critical path, resourcing, recovery and rebaselining.', min_trust: 'member', read_only: false },
      { slug: 'cost', title: 'Cost & earned value', description: 'Estimating, forecasting, variance analysis and EVM practice.', min_trust: 'member', read_only: false },
      { slug: 'risk', title: 'Risk & contingency', description: 'Quantitative risk analysis, contingency drawdown, escalation.', min_trust: 'member', read_only: false },
      { slug: 'certification', title: 'Certification & study', description: 'Preparing for PCI examinations and keeping credentials current.', min_trust: 'guest', read_only: false },
      { slug: 'announcements', title: 'Announcements', description: 'Institute news. Read-only.', min_trust: 'guest', read_only: true },
    ],
  }
}

export function careerEmployers() {
  return {
    employers: [
      { id: 4, slug: 'northgate-infrastructure', name: 'Northgate Infrastructure', state: 'verified', website: 'https://northgate.example.org', version: 3, role: 'owner', may_publish: true },
      { id: 9, slug: 'meridian-energy-projects', name: 'Meridian Energy Projects', state: 'pending_verification', website: null, version: 1, role: 'member', may_publish: false },
    ],
  }
}

export function careerApplications() {
  const consent = (granted: string | null, withdrawn: string | null) => ({
    granted_at: granted, withdrawn_at: withdrawn, policy_version: '2026-01', active: !!granted && !withdrawn,
  })
  return {
    applications: [
      { id: 512, state: 'shortlisted', title: 'Senior Planning Engineer', employer: 'Northgate Infrastructure', submitted_at: '2026-07-11T09:30:00Z', withdrawn_at: null, cv_name: 'Amara-Okafor-CV.pdf', consent: consent('2026-07-11T09:30:00Z', null) },
      { id: 498, state: 'submitted', title: 'Project Controls Lead — Offshore Wind', employer: 'Meridian Energy Projects', submitted_at: '2026-06-28T15:02:00Z', withdrawn_at: null, cv_name: 'Amara-Okafor-CV.pdf', consent: consent('2026-06-28T15:02:00Z', null) },
      { id: 455, state: 'withdrawn', title: 'Cost Manager (Rail)', employer: 'Northgate Infrastructure', submitted_at: '2026-05-14T11:20:00Z', withdrawn_at: '2026-06-02T08:00:00Z', cv_name: null, consent: consent('2026-05-14T11:20:00Z', '2026-06-02T08:00:00Z') },
    ],
  }
}

export function careerApplicants() {
  return {
    applications: [
      { id: 512, state: 'shortlisted', submitted_at: '2026-07-11T09:30:00Z', applicant: 'Amara Okafor', has_cv: true },
      { id: 507, state: 'submitted', submitted_at: '2026-07-09T13:44:00Z', applicant: 'Tomás Delgado', has_cv: true },
      { id: 501, state: 'rejected', submitted_at: '2026-07-02T10:05:00Z', applicant: 'Wei Lin', has_cv: false },
    ],
  }
}

const ROOMS = [
  { slug: 'planning-clinic', title: 'Planning clinic', description: 'Bring a schedule problem, leave with an approach.', topic: 'Schedule recovery', category: 'practice', locale: 'en', room_type: 'clinic', state: 'open', capacity: 40, guests_welcome: true, slow_mode_seconds: 5, participants: 17 },
  { slug: 'evm-help', title: 'Earned value help desk', description: 'Variance, forecasting and the arithmetic behind them.', topic: 'EVM', category: 'practice', locale: 'en', room_type: 'help', state: 'open', capacity: 40, guests_welcome: true, slow_mode_seconds: 5, participants: 9 },
  { slug: 'exam-study', title: 'Certification study room', description: 'Preparing for a PCI examination, together.', topic: 'Study', category: 'certification', locale: 'en', room_type: 'study', state: 'open', capacity: 60, guests_welcome: true, slow_mode_seconds: 0, participants: 23 },
]

export function communityRooms() {
  return { rooms: ROOMS }
}

export function communityRoom(slug: string) {
  const base = ROOMS.find((r) => r.slug === slug) ?? ROOMS[0]
  return {
    ...base,
    accepting: true, images_allowed: true, rules_version: '2026-01',
    pinned_welcome: 'Be specific, be kind, and never post commercially confidential project data.',
    retention_class: 'short',
  }
}

export function communityMessages() {
  const at = (m: number) => new Date(Date.parse('2026-07-29T09:00:00Z') + m * 60000).toISOString()
  return {
    messages: [
      { sequence: 1, body: 'Morning all — has anyone dealt with a levelled schedule where the labour cap moves mid-period?', author: 'Priya', reply_to: null, at: at(0), kind: 'text' },
      { sequence: 2, body: 'Yes, more than once. Are you re-levelling the whole plan or just the affected window?', author: 'Daniel', reply_to: 1, at: at(3), kind: 'text' },
      { sequence: 3, body: 'Just the window — the rest of the baseline is committed.', author: 'Priya', reply_to: 2, at: at(5), kind: 'text' },
      { sequence: 4, body: 'Then hold the milestones and let float absorb it, and record the cap change as an assumption so the variance is explainable later.', author: 'Amara', reply_to: 3, at: at(8), kind: 'text' },
    ],
  }
}

/* The billing screen's price table. Explicit because it is five NESTED blocks read without a guard
 * (`pricing.membership.final`) — a flat generic answer is a crash, not a thin screen. Figures match
 * the certification catalogue above so the two pages agree. */
export function pricing(cert?: string | null) {
  const block = (standard: number, discountPct = 0) => ({
    standard,
    defaultDiscount: discountPct,
    final: Math.round(standard * (1 - discountPct / 100)),
  })
  const known = CERTS.find((c) => c.code === cert)
  return {
    currency: 'USD',
    membership: block(150),
    exam: block(known ? 750 : 750, 10),
    bundle: block(850, 15),
    renewal: block(120),
    recert: block(350),
    cert: known ? { code: known.code, name: known.name } : null,
  }
}

/* ── Admin aggregates ────────────────────────────────────────────────────────────────────────
 * The console's summary endpoints. Each of these is read as a NESTED object with named keys
 * (`data.totals.paid`, `data.issues.missing_title`, `data.coverage.langs`), so the generic
 * `{ ok, rows }` answer is a crash behind the error boundary rather than a thin screen. They are
 * spelled out here, consistent with the member and payment volumes elsewhere in this file. */

const money = (n: number) => Math.round(n)

export function paymentTotals() {
  const rows = Array.from({ length: 34 }, (_, i) => {
    const r = new Rand(`pay:${i}`)
    const p = person(r)
    return {
      id: 900 - i, ...p,
      reference: ref(r, 'PCI-PAY'),
      product_type: r.pick(['exam', 'membership', 'bundle', 'renewal', 'recert']),
      final_amount: r.pick([150, 690, 750, 850, 120, 350]),
      currency: 'USD',
      payment_status: r.pick(['paid', 'paid', 'paid', 'paid', 'refunded', 'pending']),
      payment_date: r.pastISO(300),
    }
  })
  const paid = rows.filter((r) => r.payment_status === 'paid')
  return {
    rows,
    totals: {
      paid: money(paid.reduce((s, r) => s + r.final_amount, 0)),
      n: paid.length,
      refunded: money(rows.filter((r) => r.payment_status === 'refunded').reduce((s, r) => s + r.final_amount, 0)),
    },
  }
}

export function paymentReconciliation() {
  const rows = Array.from({ length: 12 }, (_, i) => {
    const r = new Rand(`recon:${i}`)
    return {
      id: 880 - i, reference: ref(r, 'PCI-PAY'), ...person(r),
      final_amount: r.pick([150, 690, 750, 850]), currency: 'USD',
      payment_status: 'paid', gateway_status: i < 10 ? 'succeeded' : 'requires_action',
      payment_date: r.pastISO(60), stripe_payment_intent: 'pi_' + ref(r, '').toLowerCase(),
      exception: i >= 10 ? 'Gateway and ledger disagree — needs an operator decision.' : null,
    }
  })
  return { rows, exceptions: 2 }
}

/** `/api/admin/codes` answers with a BARE ARRAY, not an envelope — the one endpoint in the console
 *  that does, and the reason a generic `{ ok, rows }` here reads as "t.map is not a function". */
export function discountCodes() {
  return Array.from({ length: 11 }, (_, i) => {
    const r = new Rand(`code:${i}`)
    const founding = i < 4
    return {
      id: i + 1,
      code: founding ? `FOUNDING-${1000 + i}` : `PCI${r.int(10, 40)}OFF`,
      discount_type: founding ? 'percent' : r.pick(['percent', 'fixed']),
      discount_value: founding ? 100 : r.pick([10, 15, 20, 25]),
      applies_to: r.pick(['all', 'exam', 'membership']),
      certification_id: null, route_key: null, min_transaction: null, max_discount: null,
      partner_id: null, start_date: r.pastISO(200), end_date: r.futureISO(200),
      max_uses: founding ? 1 : 200, used_count: r.int(0, founding ? 1 : 90),
      single_use_per_email: 1, active: i < 9 ? 1 : 0,
      campaign_name: founding ? 'Founding cohort' : 'Spring intake',
    }
  })
}

export function foundingStats() {
  const code = (id: number, name: string, route: string, redeemed: number, cap: number | null,
                grants: [boolean, boolean, boolean], open: boolean, apps: Record<string, number>) => ({
    id, code: name, route,
    grants: { membership: grants[0], exam: grants[1], study: grants[2] },
    auto_approve: id !== 3,
    window: { from: '2026-01-01', until: open ? '2026-12-31' : '2026-06-30', days_left: open ? 155 : null, open },
    uses: { redeemed, cap, remaining: cap === null ? null : cap - redeemed },
    applications: apps,
    usable: open && (cap === null || redeemed < cap),
    state: open ? 'open' : 'closed',
  })
  return {
    rows: [
      code(1, 'FOUNDING-2026', 'founding', 41, 100, [true, true, true], true,
        { auto_approved: 34, approved: 5, pending_review: 2, rejected: 0, revoked: 0 }),
      code(2, 'PARTNER-NORTHGATE', 'partner', 12, 25, [true, true, false], true,
        { auto_approved: 9, approved: 2, pending_review: 1, rejected: 0, revoked: 0 }),
      code(3, 'PILOT-COHORT-A', 'pilot', 18, 20, [false, true, true], false,
        { auto_approved: 0, approved: 12, pending_review: 2, rejected: 3, revoked: 1 }),
    ],
  }
}

export function supportInbox() {
  const t = (i: number, over: Record<string, unknown> = {}) => {
    const r = new Rand(`sup:${i}`)
    return {
      id: 300 + i, reference: ref(r, 'PCI-SUP'), ...person(r),
      subject: r.pick(['Exam reschedule request', 'Certificate not showing', 'Invoice copy needed',
        'CPD evidence rejected', 'Cannot sign in to Certuvo']),
      category: r.pick(['exam', 'billing', 'credential', 'account']),
      status: r.pick(['open', 'open', 'waiting', 'resolved']),
      priority: r.pick(['normal', 'normal', 'high']),
      assigned_to: i % 3 === 0 ? null : 'operations@example.org',
      created_at: r.pastISO(30), last_activity_at: r.pastISO(5),
      ...over,
    }
  }
  return {
    tickets: Array.from({ length: 9 }, (_, i) => t(i)),
    chats: Array.from({ length: 3 }, (_, i) => ({
      id: 40 + i, ...person(new Rand(`chat:${i}`)), status: 'waiting',
      started_at: new Rand(`chat:${i}`).pastISO(1), last_message: 'Is the exam fee refundable?',
    })),
    enquiries: Array.from({ length: 4 }, (_, i) => ({
      id: 70 + i, ...person(new Rand(`enq:${i}`)), subject: 'Corporate training enquiry',
      created_at: new Rand(`enq:${i}`).pastISO(14), status: 'new',
    })),
    unassigned: 3,
  }
}

export function supportMetrics() {
  return {
    by_status: [{ status: 'open', n: 9 }, { status: 'waiting', n: 4 }, { status: 'resolved', n: 61 }],
    escalated: 2, unassigned: 3, waiting_chats: 3,
    overdue_first_response: 1, overdue_resolution: 2,
    avg_first_response_mins: 46, avg_resolution_mins: 512,
    csat_avg: 4.6, csat_count: 38,
    sla: { first_response_mins: 120, resolution_mins: 1440 },
    agent_workload: [
      { email: 'operations@example.org', n: 7 },
      { email: 'credentials@example.org', n: 4 },
      { email: 'exams@example.org', n: 2 },
    ],
  }
}

export function forumQueue() {
  return {
    posts: Array.from({ length: 5 }, (_, i) => {
      const r = new Rand(`fpost:${i}`)
      const p = person(r)
      return {
        id: 700 + i, thread_id: 120 + i, author_name: `${p.first_name} ${p.last_name}`,
        body: 'Held for review: the post contains an external link and the author is below the standing this category asks for.',
        status: 'held', flags: i < 2 ? 1 : 0, created_at: r.pastISO(6),
        thread_title: 'Rebaselining after a scope change',
      }
    }),
    threads: Array.from({ length: 3 }, (_, i) => {
      const r = new Rand(`fthr:${i}`)
      const p = person(r)
      return {
        id: 120 + i, category: 'schedule', title: 'Recovering a slipped critical path',
        author_name: `${p.first_name} ${p.last_name}`, status: 'held', reply_count: r.int(0, 8),
        created_at: r.pastISO(10), last_post_at: r.pastISO(2),
      }
    }),
  }
}

export function adminReviews() {
  return {
    reviews: Array.from({ length: 8 }, (_, i) => {
      const r = new Rand(`rev:${i}`)
      const p = person(r)
      return {
        id: 200 + i, ...p, rating: r.pick([5, 5, 4, 4, 3]),
        title: r.pick(['Worth every hour', 'A genuinely rigorous examination', 'Clear and well run']),
        body: 'The syllabus matched the examination closely, and the result came back within the published window.',
        status: i < 2 ? 'pending' : 'published', featured: i === 2 ? 1 : 0,
        credential: r.pick(['PCL-AI', 'PFL-AI', 'PML-AI']), created_at: r.pastISO(200),
      }
    }),
    counts: { pending: 2, published: 6, featured: 1 },
  }
}

export function adminReports(from = '2026-06-29', to = '2026-07-29') {
  const daily = Array.from({ length: 30 }, (_, i) => {
    const r = new Rand(`rd:${i}`)
    const d = new Date(Date.parse(from + 'T00:00:00Z') + i * 86400000).toISOString().slice(0, 10)
    return { d, revenue: r.int(600, 5200), n: r.int(1, 8) }
  })
  return {
    from, to,
    totals: {
      payments: daily.reduce((s, x) => s + x.n, 0),
      revenue: daily.reduce((s, x) => s + x.revenue, 0),
      code_discounts: 4150, standard_discounts: 1890, avg_order: 612,
    },
    revenue_daily: daily,
    by_product: [
      { product_type: 'exam', n: 61, revenue: 44_910 },
      { product_type: 'membership', n: 88, revenue: 13_200 },
      { product_type: 'bundle', n: 24, revenue: 20_400 },
      { product_type: 'recert', n: 9, revenue: 3_150 },
    ],
    by_country: [
      { country: 'United Kingdom', n: 38, revenue: 24_600 },
      { country: 'Nigeria', n: 31, revenue: 18_400 },
      { country: 'United Arab Emirates', n: 22, revenue: 15_900 },
      { country: 'India', n: 19, revenue: 11_200 },
      { country: 'Canada', n: 14, revenue: 9_800 },
    ],
    by_certification: CERTS.map((c, i) => ({ certification: c.code, n: [24, 19, 12, 6][i], revenue: [17_800, 14_100, 8_300, 4_200][i] })),
    certificates_by_certification: CERTS.map((c, i) => ({ certification: c.code, issued: [19, 15, 9, 4][i] })),
    funnel: { started: 214, paid: 182 },
    new_members: 88,
  }
}

function seriesRows(keys: [string, string], entries: Array<[string, number]>) {
  return entries.map(([a, b]) => ({ [keys[0]]: a, [keys[1]]: b }))
}

export function analyticsSummary(days = 30) {
  const daily = Array.from({ length: days }, (_, i) => {
    const r = new Rand(`an:${i}`)
    return {
      day: new Date(Date.parse('2026-07-29T00:00:00Z') - (days - 1 - i) * 86400000).toISOString().slice(0, 10),
      page_views: r.int(900, 2600), visitors: r.int(380, 1100),
    }
  })
  return {
    page_views: daily.reduce((s, d) => s + d.page_views, 0),
    visitors: daily.reduce((s, d) => s + d.visitors, 0),
    registrations: 143, logins: 2_318, purchases: 182, revenue: 81_660, memberships_activated: 88,
    top_pages: seriesRows(['path', 'views'], [
      ['/', 18_402], ['/certifications', 9_118], ['/pml-ai', 6_240],
      ['/about', 4_115], ['/verify', 3_902], ['/contact', 2_640],
    ]),
    sources: seriesRows(['source', 'n'], [['organic', 12_400], ['direct', 6_810], ['linkedin', 3_240], ['referral', 1_905], ['email', 1_120]]),
    campaigns: seriesRows(['campaign', 'n'], [['spring-intake', 1_840], ['founding-cohort', 960], ['recert-reminder', 410]]),
    devices: seriesRows(['device', 'n'], [['desktop', 14_900], ['mobile', 9_100], ['tablet', 1_200]]),
    browsers: seriesRows(['browser', 'n'], [['Chrome', 15_200], ['Safari', 5_400], ['Edge', 2_900], ['Firefox', 1_700]]),
    countries: seriesRows(['country', 'n'], [['United Kingdom', 7_200], ['Nigeria', 5_100], ['United Arab Emirates', 3_800], ['India', 3_100], ['Canada', 2_400]]),
    daily,
    conversions_by_source: [
      { source: 'organic', registrations: 71, purchases: 92, revenue: 41_200 },
      { source: 'linkedin', registrations: 34, purchases: 41, revenue: 19_800 },
      { source: 'email', registrations: 22, purchases: 28, revenue: 12_900 },
      { source: 'direct', registrations: 16, purchases: 21, revenue: 7_760 },
    ],
  }
}

export function adminTeam() {
  const sections = ['content', 'members', 'exams', 'credentials', 'payments', 'codes', 'subscribers',
    'reports', 'support', 'settings', 'team']
  return {
    rows: [
      { id: 1, email: 'owner@example.org', name: 'Site Owner', role: 'owner', permissions: [], effective: sections, cert_scope: [], status: 'active', last_login_at: '2026-07-29T07:12:00Z' },
      { id: 2, email: 'website@example.org', name: 'Ines Marchetti', role: 'website_manager', permissions: [], effective: ['content', 'subscribers'], cert_scope: [], status: 'active', last_login_at: '2026-07-28T16:40:00Z' },
      { id: 3, email: 'students@example.org', name: 'Kofi Mensah', role: 'student_manager', permissions: [], effective: ['members', 'support', 'credentials'], cert_scope: [], status: 'active', last_login_at: '2026-07-29T08:02:00Z' },
      { id: 4, email: 'exams@example.org', name: 'Hana Suzuki', role: 'exam_manager', permissions: ['reports'], effective: ['exams', 'credentials', 'reports'], cert_scope: [1, 3], status: 'active', last_login_at: '2026-07-27T11:25:00Z' },
      { id: 5, email: 'audit@example.org', name: 'Roberto Silva', role: 'viewer', permissions: [], effective: [], cert_scope: [], status: 'suspended', last_login_at: null },
    ],
    roles: ['owner', 'website_manager', 'student_manager', 'exam_manager', 'viewer', 'custom'],
    sections,
    role_grants: {
      owner: sections,
      website_manager: ['content', 'subscribers'],
      student_manager: ['members', 'support', 'credentials'],
      exam_manager: ['exams', 'credentials'],
      viewer: [],
      custom: [],
    },
  }
}

export function adminTemplates() {
  const rows = Array.from({ length: 14 }, (_, i) => {
    const r = new Rand(`tpl:${i}`)
    return {
      id: i + 1,
      slug: ['risk-register', 'cost-breakdown-structure', 'schedule-basis-memo', 'evm-dashboard',
        'change-log', 'cashflow-forecast', 'wbs-dictionary', 'lookahead-plan', 'variance-report',
        'contingency-drawdown', 'benefits-map', 'quality-checklist', 'raci-matrix', 'closeout-report'][i],
      title: ['Risk register', 'Cost breakdown structure', 'Schedule basis memo', 'EVM dashboard',
        'Change log', 'Cashflow forecast', 'WBS dictionary', 'Look-ahead plan', 'Variance report',
        'Contingency drawdown', 'Benefits map', 'Quality checklist', 'RACI matrix', 'Close-out report'][i],
      category: ['risk', 'cost', 'schedule', 'evm', 'change', 'cashflow', 'scope', 'schedule',
        'cost', 'risk', 'delivery', 'quality', 'delivery', 'delivery'][i],
      certification_id: r.pick([null, 1, 2, 3]),
      download_count: r.int(12, 480), published: i < 12 ? 1 : 0,
      updated_at: r.pastISO(180), format: 'xlsx',
    }
  })
  return { rows, total: rows.length, published: rows.filter((r) => r.published).length }
}

export function templateAnalytics() {
  const series = Array.from({ length: 30 }, (_, i) => ({
    day: new Date(Date.parse('2026-07-29T00:00:00Z') - (29 - i) * 86400000).toISOString().slice(0, 10),
    count: new Rand(`tpd:${i}`).int(4, 42),
  }))
  const rows = adminTemplates().rows
  return {
    total_downloads: rows.reduce((s, r) => s + r.download_count, 0),
    unique_downloaders: 412, templates: rows.length, published: rows.filter((r) => r.published).length,
    window_days: 30, window_downloads: series.reduce((s, d) => s + d.count, 0),
    series,
    top: [...rows].sort((a, b) => b.download_count - a.download_count).slice(0, 6).map((r) => ({
      slug: r.slug, title: r.title, download_count: r.download_count,
      downloaders: Math.round(r.download_count * 0.72), published: !!r.published,
    })),
  }
}

export function seoOverview() {
  return {
    canonical_host: 'https://www.projectcontrolsinstitute.org',
    pages: 216, published: 208, noindex: 8, sitemap_urls: 208, redirects: 14,
    issues: { missing_title: 3, missing_meta: 11, duplicate_titles: 2, duplicate_descriptions: 4 },
  }
}

export function seoAudit() {
  return {
    page_count: 216,
    missing_title: ['/legacy/exam-faq', '/partners/archive', '/news/2024-agm'],
    missing_meta: ['/bok/scope', '/bok/quality', '/governance/committees'],
    duplicate_titles: [{ title: 'Certifications', count: 3 }, { title: 'Contact', count: 2 }],
    missing_h1: ['/legal/cookies'],
    missing_canonical: ['/news/2024-agm'],
    missing_structured_data: ['/bok/quality', '/governance/committees'],
    images_missing_alt: [{ page: '/about', images: 2 }, { page: '/news/2026-cohort', images: 1 }],
    broken_internal_links: [{ page: '/partners/archive', targets: ['/partners/legacy-list'] }],
    redirect_chains: [{ from: '/exam', to: '/certifications' }],
  }
}

export function seoIntegrations() {
  return {
    ga4_measurement_id: 'G-DEMO00000', gtm_container_id: '', clarity_project_id: '',
    google_site_verification: 'demo-verification-token', bing_site_verification: '',
    psi_has_key: false, indexnow_key: '', indexnow_key_url: '',
    submissions: [
      { engine: 'IndexNow', url_count: 208, status: 'accepted', detail: 'Submitted after the July sitemap rebuild.', created_at: '2026-07-21T09:00:00Z' },
      { engine: 'Bing', url_count: 208, status: 'accepted', detail: '', created_at: '2026-07-21T09:00:00Z' },
    ],
  }
}

export function aiVisibilityOverview() {
  return {
    canonical_host: 'https://www.projectcontrolsinstitute.org',
    llms_url: '/llms.txt', robots_url: '/robots.txt', sitemap_url: '/sitemap.xml',
    structured_data: { pages: 216, with_json_ld: 191, coverage_pct: 88 },
    crawlers: { known: 14, blocked: 3, allowed: 11 },
    traffic: { hits_30d: 4_812, distinct_bots: 9 },
  }
}

export function aiVisibilityCrawlers() {
  return {
    total: 4_812,
    by_bot: seriesRows(['bot', 'hits'], [['GPTBot', 1_640], ['ClaudeBot', 1_180], ['PerplexityBot', 820], ['Google-Extended', 640], ['Bingbot', 532]]),
    by_page: seriesRows(['path', 'hits'], [['/certifications', 980], ['/', 870], ['/pml-ai', 610], ['/bok', 480]]),
    daily: Array.from({ length: 30 }, (_, i) => ({
      day: new Date(Date.parse('2026-07-29T00:00:00Z') - (29 - i) * 86400000).toISOString().slice(0, 10),
      hits: new Rand(`ai:${i}`).int(80, 260),
    })),
  }
}

export function aiVisibilityPolicy() {
  const bots: Array<[string, string, string, string, boolean]> = [
    ['GPTBot', 'GPTBot', 'OpenAI', 'Training and answers', false],
    ['ClaudeBot', 'ClaudeBot', 'Anthropic', 'Answers with citation', false],
    ['PerplexityBot', 'PerplexityBot', 'Perplexity', 'Answers with citation', false],
    ['Google-Extended', 'Google-Extended', 'Google', 'Gemini grounding', false],
    ['CCBot', 'CCBot', 'Common Crawl', 'Bulk corpus collection', true],
    ['Bytespider', 'Bytespider', 'ByteDance', 'Bulk corpus collection', true],
    ['anthropic-ai', 'anthropic-ai (legacy)', 'Anthropic', 'Legacy token', true],
  ]
  return {
    rows: bots.map(([token, label, vendor, purpose, blocked]) => ({ token, label, vendor, purpose, detectable: true, blocked })),
    blocked_count: bots.filter((b) => b[4]).length,
    robots_preview: 'User-agent: CCBot\nDisallow: /\n\nUser-agent: Bytespider\nDisallow: /\n\nUser-agent: *\nAllow: /\nSitemap: https://www.projectcontrolsinstitute.org/sitemap.xml',
  }
}

export function marketingCampaigns() {
  return {
    rows: [
      { id: 3, name: 'Spring intake', subject: 'Applications close on 30 September', audience: 'all', status: 'sent', total: 4_120, sent: 4_096, failed: 24, suppressed: 61, created_at: '2026-06-02T09:00:00Z', sent_at: '2026-06-03T08:00:00Z' },
      { id: 2, name: 'Recertification reminder', subject: 'Your credential expires in 90 days', audience: 'students', status: 'sending', total: 318, sent: 210, failed: 2, suppressed: 4, created_at: '2026-07-20T10:00:00Z', sent_at: '2026-07-28T09:00:00Z' },
      { id: 1, name: 'Founding cohort welcome', subject: 'Welcome to the founding cohort', audience: 'students', status: 'draft', total: 0, sent: 0, failed: 0, suppressed: 0, created_at: '2026-07-26T14:30:00Z', sent_at: null },
    ],
  }
}

export function marketingSuppression() {
  return {
    rows: Array.from({ length: 6 }, (_, i) => {
      const r = new Rand(`sup2:${i}`)
      return { id: i + 1, email: person(r).email, reason: r.pick(['unsubscribed', 'bounced', 'complaint']), created_at: r.pastISO(300) }
    }),
  }
}

export function socialMedia() {
  const svg = '<svg viewBox="0 0 24 24"><circle cx="12" cy="12" r="10"/></svg>'
  const accounts = [
    ['linkedin', 'LinkedIn', 'project-controls-institute'],
    ['youtube', 'YouTube', '@projectcontrolsinstitute'],
    ['x', 'X', '@pci_ai'],
  ]
  return {
    enabled: true, analytics: true,
    accounts: accounts.map(([platform, display_name, handle], i) => ({
      id: i + 1, platform, display_name, handle,
      url: `https://${platform}.example.org/${String(handle).replace('@', '')}`,
      icon_type: 'builtin', custom_icon: null,
      aria_label: `${display_name} — Project Controls Institute`, tooltip: display_name,
      locations: 'footer,header', display_order: i + 1, active: 1,
    })),
    platforms: accounts.map(([key, label]) => ({ key, label, color: '#1d4ed8', domains: [`${key}.com`], svg })),
    locations: [{ key: 'header', label: 'Header' }, { key: 'footer', label: 'Footer' }, { key: 'contact', label: 'Contact page' }],
    share_targets: [{ key: 'linkedin', label: 'LinkedIn' }, { key: 'x', label: 'X' }, { key: 'email', label: 'Email' }],
    share: [
      { content_type: 'news', buttons: 'linkedin,x,email', enabled: 1 },
      { content_type: 'blog', buttons: 'linkedin,email', enabled: 1 },
      { content_type: 'event', buttons: 'linkedin', enabled: 0 },
    ],
    audit: Array.from({ length: 5 }, (_, i) => {
      const r = new Rand(`soc:${i}`)
      const p = person(r)
      return {
        id: i + 1, account_id: (i % 3) + 1, actor_name: `${p.first_name} ${p.last_name}`,
        action: r.pick(['updated', 'created', 'deactivated']),
        detail: 'Display order changed', created_at: r.pastISO(120),
      }
    }),
  }
}

export function labScenarios() {
  const rows = Array.from({ length: 9 }, (_, i) => {
    const r = new Rand(`lab:${i}`)
    return {
      id: i + 1,
      scenario_code: `SIM-${2026}-${String(100 + i)}`,
      title: ['Contingency drawdown under pressure', 'A late long-lead procurement', 'Recovering a slipped milestone',
        'Reconciling an earned-value variance', 'Resource-levelling under a labour cap', 'Cashflow with delayed certification',
        'Scope change without a baseline', 'Quantifying a three-point estimate', 'Closing out with open change orders'][i],
      kind: r.pick(['exercise', 'assessment']),
      industry: r.pick(['Rail', 'Energy', 'Marine', 'Infrastructure']),
      difficulty: r.pick(['foundation', 'intermediate', 'advanced']),
      competencies: ['schedule', 'cost', 'risk'].slice(0, r.int(1, 3)),
      status: i < 6 ? 'published' : 'draft',
      review_state: i < 6 ? 'approved' : r.pick(['in_review', 'draft']),
      version: r.int(1, 4), interactive: i % 2 === 0, attempts: r.int(0, 220),
      updated_at: r.pastISO(180),
    }
  })
  return { rows, total: rows.length, published: rows.filter((r) => r.status === 'published').length }
}

export function labTemplates() {
  return {
    rows: [
      { task: 'evm_variance', label: 'Earned-value variance', hint: 'Give BCWS/BCWP/ACWP and ask for CV, SV and the narrative.', config: '{"tolerance":0.02}' },
      { task: 'critical_path', label: 'Critical path', hint: 'Supply an activity network and ask for the path and total float.', config: '{"units":"days"}' },
      { task: 'three_point', label: 'Three-point estimate', hint: 'Optimistic/most-likely/pessimistic to a PERT mean and σ.', config: '{"method":"pert"}' },
    ],
  }
}

export function i18nCoverage() {
  const langs = { en: { done: 216, total: 216 }, ko: { done: 178, total: 216 }, ar: { done: 141, total: 216 },
    es: { done: 196, total: 216 }, fr: { done: 188, total: 216 }, zh: { done: 122, total: 216 }, ru: { done: 96, total: 216 } }
  return {
    coverage: { total: 216, langs },
    languages: [
      { code: 'en', name: 'English' }, { code: 'ko', name: 'Korean' }, { code: 'ar', name: 'Arabic' },
      { code: 'es', name: 'Spanish' }, { code: 'fr', name: 'French' }, { code: 'zh', name: 'Chinese' },
      { code: 'ru', name: 'Russian' },
    ],
    provider: { provider: 'none', model: '', endpoint: '', has_key: false, configured: false },
    pages: ['/', '/certifications', '/about', '/contact', '/verify', '/bok', '/governance', '/news'],
  }
}

export function contentOverview() {
  return {
    posts_total: 64, published: 51, drafts: 9, in_review: 3, scheduled: 1,
    authors: 6, categories: 8,
    ai: { openai: false, anthropic: false },
    recent: Array.from({ length: 6 }, (_, i) => {
      const r = new Rand(`cc:${i}`)
      return {
        id: 60 - i,
        title: ['What a schedule basis memo should actually say', 'Contingency is not a slush fund',
          'Reading an EVM dashboard honestly', 'The change log nobody maintains',
          'Cashflow forecasting for delayed certification', 'Float is a shared asset'][i],
        status: i < 4 ? 'published' : 'draft',
        author_name: 'Editorial team',
        updated_at: r.pastISO(60),
      }
    }),
  }
}

/* Sub-tab endpoints. A route walk never reaches these — they load on a tab click — and each one
 * reads a nested field or calls a string/array method on a value the generic answer leaves absent
 * (`f.kind.replace`, `p.issues.missing_title`, `cap.capability.startsWith`). */

export function fraudFlags() {
  return {
    rows: Array.from({ length: 6 }, (_, i) => {
      const r = new Rand(`fraud:${i}`)
      return {
        id: i + 1,
        kind: r.pick(['velocity_same_ip', 'shared_email_domain', 'code_reuse_attempt', 'mismatched_country']),
        code_id: r.int(1, 9), code: `PCI${r.int(10, 40)}OFF`,
        user_id: r.int(100, 900), email: person(r).email,
        detail: 'Four redemptions from one address inside ten minutes.',
        status: i < 4 ? 'open' : 'dismissed', created_at: r.pastISO(40),
      }
    }),
  }
}

export function seoPages() {
  const slugs = ['/', '/certifications', '/pml-ai', '/pfl-ai', '/pcl-ai', '/about', '/contact',
    '/verify', '/bok', '/bok/scope', '/bok/quality', '/governance', '/governance/committees',
    '/news', '/legal/cookies', '/legacy/exam-faq']
  return {
    rows: slugs.map((slug, i) => {
      const r = new Rand(`seo:${i}`)
      const missingTitle = i >= 14
      const missingMeta = i === 9 || i === 10 || i === 12
      return {
        id: i + 1, slug,
        title: missingTitle ? null : slug === '/' ? 'Project Controls Institute' : titleFromSlug(slug),
        meta_description: missingMeta ? null : 'Independent, examination-based certification for project controls professionals.',
        noindex: i >= 14 ? 1 : 0, published: i >= 15 ? 0 : 1,
        canonical_url: i === 13 ? null : 'https://www.projectcontrolsinstitute.org' + slug,
        og_image: r.pick(['/assets/images/og-default.png', null]),
        issues: {
          missing_title: missingTitle, missing_meta: missingMeta,
          duplicate_title: i === 3 || i === 4, duplicate_meta: i === 6 || i === 7,
        },
      }
    }),
  }
}

function titleFromSlug(slug: string): string {
  const last = slug.split('/').filter(Boolean).pop() ?? 'Home'
  return last.replace(/-/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
}

export function backlinksOverview() {
  return {
    prospects: [
      { status: 'prospect', n: 24 }, { status: 'contacted', n: 11 }, { status: 'in_talks', n: 5 },
      { status: 'agreed', n: 3 }, { status: 'live', n: 9 }, { status: 'declined', n: 6 }, { status: 'dormant', n: 4 },
    ],
    links: [{ status: 'live', n: 31 }, { status: 'lost', n: 4 }, { status: 'pending', n: 7 }],
    follow_ups_due: 5, live: 31, lost: 4,
  }
}

export function contentAnalyticsOverview() {
  return {
    sources: [
      { id: 1, provider: 'gsc', label: 'Search Console', property: 'sc-domain:example.org', auth_kind: 'service_account', range_days: 90, status: 'connected', last_synced_at: '2026-07-29T04:00:00Z', last_error: null, has_secret: true },
      { id: 2, provider: 'ga4', label: 'Analytics 4', property: 'properties/000000', auth_kind: 'service_account', range_days: 90, status: 'needs_secret', last_synced_at: null, last_error: 'No credential configured for this deployment.', has_secret: false },
    ],
    totals: [
      { source_id: 1, provider: 'gsc', label: 'Search Console', status: 'connected', clicks: 8_412, impressions: 241_900, ctr: 0.035, position: 12.4, last_synced_at: '2026-07-29T04:00:00Z' },
      { source_id: 2, provider: 'ga4', label: 'Analytics 4', status: 'needs_secret', sessions: null, users: null, pageviews: null, last_synced_at: null },
    ],
  }
}

export function contentCapabilities() {
  const rows: Array<[string, string, string, string, boolean]> = [
    ['linkedin', 'LinkedIn', 'social', 'Direct Publishing Available (organisation pages)', true],
    ['x', 'X', 'social', 'Direct Publishing Available (v2 API)', false],
    ['medium', 'Medium', 'syndication', 'Read Only', false],
    ['devto', 'DEV', 'syndication', 'Direct Publishing Available', false],
    ['mastodon', 'Mastodon', 'social', 'Direct Publishing Available', false],
    ['substack', 'Substack', 'syndication', 'Manual export only — no publishing API', false],
  ]
  return {
    rows: rows.map(([platform_key, platform, kind, capability, connected], i) => ({
      id: i + 1, platform_key, platform, kind, capability,
      publish_mode: capability.startsWith('Direct') ? 'api' : 'manual',
      requires_approval: 1, official_api: capability.startsWith('Direct') ? 1 : 0,
      connected,
      notes: connected ? 'Connected in this deployment.' : 'Not connected — credentials are configured per environment.',
      doc_url: 'https://example.org/docs/' + platform_key,
    })),
  }
}

export function certuvoConfig() {
  return {
    enabled: true,
    api_base: 'https://certuvo.example.org/api', provision_path: '/v1/accounts',
    deactivate_path: '/v1/accounts/{id}/deactivate', login_url: 'https://certuvo.example.org/login',
    auth_header: 'Authorization', has_api_key: true, has_webhook_secret: false,
    requires: 'membership', retry_max: 5,
    username_prefix: 'pci', password_length: 16, email_conflict: 'suffix',
    honorary_grants_membership: true,
    accounts: Array.from({ length: 8 }, (_, i) => {
      const r = new Rand(`certuvo:${i}`)
      const p = person(r)
      const failed = i === 6
      return {
        user_id: 100 + i, email: p.email, is_test: 0,
        status: failed ? 'failed' : i === 7 ? 'pending' : 'active',
        username: failed ? null : `pci.${p.first_name.toLowerCase()}`,
        external_id: failed ? null : 'CV-' + ref(r, ''),
        member_type: r.pick(['member', 'founding']), email_conflict: 0, must_change_password: i < 2 ? 1 : 0,
        provisioned_at: failed ? null : r.pastISO(200),
        activated_at: failed || i === 7 ? null : r.pastISO(190),
        credentials_sent_at: failed ? null : r.pastISO(190),
        last_error: failed ? 'Upstream returned 409: an account already exists for this address.' : null,
        retry_count: failed ? 3 : 0, next_retry_at: failed ? '2026-07-30T02:00:00Z' : null,
        suspended_at: null, revoked_at: null,
      }
    }),
  }
}
