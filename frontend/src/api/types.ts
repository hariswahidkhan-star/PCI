// Types mirroring the backend JSON contract (backend/Endpoints/StudentExam.cs, Program.cs).
// Only the fields the portal actually renders are typed strictly; open-ended nested rows are
// kept permissive so a backend addition never breaks the build.

export interface LoginResponse {
  ok: boolean
  token: string
  user: { id: number; email: string; firstName: string; lastName: string }
}

export interface MeUser {
  id: number
  email: string
  first_name: string
  last_name: string
  registration_no: string
  created_at: string
  /** true when this session is a staff support view (admin impersonation) — the portal shows a permanent banner. */
  impersonated?: boolean
}

export interface Certification {
  id: number
  code: string
  name: string
  description?: string | null
  expiry_years?: number | null
}

/** One paid exam entitlement, per certification (multi-cert view). */
export interface ExamEntry {
  certification_id: number
  certification_code?: string | null
  certification_name?: string | null
  payment_id: number
  reference?: string | null
  deadline?: string | null
  entitlement_status?: string | null
  booking?: Record<string, unknown> | null
  latest_attempt?: Record<string, unknown> | null
  credential?: { credential_id: string; status: string; expires_at?: string | null } | null
  /** Recert CPD status for this credential (present only when the certification requires CPD). */
  recert_cpd?: { required: number; approved: number; met: boolean; ai_required?: number; ai_approved?: number; ai_met?: boolean } | null
  // Exam-exceptions surface: admin extensions, attempt allowances, waivers and scheduling state.
  authorization?: Record<string, unknown> | null
  extended?: boolean
  original_deadline?: string | null
  days_left?: number | null
  attempts_used?: number | null
  attempts_permitted?: number | null
  retake_wait_until?: string | null
  waiver?: Record<string, unknown> | null
  scheduling_status?: string | null
}

export interface Attempt {
  id: number
  kind: string
  certification_id?: number | null
  certification_code?: string | null
  certification_name?: string | null
  started_at?: string | null
  submitted_at?: string | null
  percent?: number | null
  result?: string | null
  status?: string | null
  result_status?: string | null
  hold_reason?: string | null
  released_at?: string | null
  duration_minutes?: number | null
}

export interface Credential {
  credential_id: string
  credential?: string | null
  certification_id?: number | null
  certification_code?: string | null
  status: string
  issued_at?: string | null
  expires_at?: string | null
  holder_name?: string | null
  certification_name?: string | null
  certification_acronym?: string | null
  certificate_wording?: string | null
}

export interface Payment {
  id: number
  product_type: string
  final_amount: number
  currency: string
  payment_status: string
  payment_date?: string | null
  reference?: string | null
  exam_schedule_deadline?: string | null
}

export interface Ticket {
  id: number
  reference: string
  subject: string
  category?: string | null
  status: string
  updated_at?: string | null
}

// Lifecycle state as computed by Lifecycle.BuildLifecycle (backend). The portal derives a visual
// candidate journey from these fields (see pages/Overview.tsx).
export interface Lifecycle {
  membership_status: string
  candidate_status: string
  exam_status: string
  result_status: string | null
  credential_status: string | null
  next_step: string
  blocking_items: string[]
}

/** Latest government-issued photo ID on file (metadata only; the file lives in Storage). */
export interface IdentityDocument {
  id: number
  doc_kind?: string | null
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  status: string // submitted | verified | rejected
  review_note?: string | null
  created_at?: string | null
}

export interface Me {
  user: MeUser
  profile: Record<string, unknown> | null
  lifecycle: Lifecycle
  consents: { required: { type: string; version: string }[]; outstanding: unknown[] }
  membership: Record<string, unknown> | null
  payments: Payment[]
  exam: {
    entitled: boolean
    deadline?: string | null
    payment_ref?: string | null
    booking?: Record<string, unknown> | null
    passed: boolean
    certification_id?: number | null
    certification?: string | null
    certification_code?: string | null
    certification_name?: string | null
    certification_acronym?: string | null
  }
  exams: ExamEntry[]
  attempts: Attempt[]
  credentials: Credential[]
  tickets: Ticket[]
  referral: { code: string } | null
  membership_grade?: {
    current: string
    label: string
    post_nominal: string
    rank: number
    eligible_upgrade?: { key: string; label: string; post_nominal: string } | null
    can_apply_fellow: boolean
    pending_application?: { to_grade: string; created_at?: string | null } | null
  } | null
  membership_dues?: { available: boolean; subscribed: boolean; status?: string | null; cancel_at_period_end?: boolean } | null
  cpd: { total: number; target: number; pending?: number }
  two_factor: boolean
  two_factor_coming_soon: boolean
  unread: number
  enrollment: Record<string, unknown> | null
  site_base_url: string
  experiences: Experience[]
  qualifications: Qualification[]
  certifications_held: HeldCertification[]
  identity_document: IdentityDocument | null
  /** Route B: fees were waived by a founding code (a $0 founding_waiver payment exists). */
  founding_member: boolean
  /** Route C: read-only board-conferred recognition — never an exam credential. */
  honorary: HonoraryAward[]
}

export interface HonoraryAward {
  award_no: string
  designation?: string | null
  citation?: string | null
  status?: string | null
  conferred_at?: string | null
}

export interface Message {
  id: number
  title?: string | null
  body?: string | null
  created_at?: string | null
  read_at?: string | null
  [k: string]: unknown
}

// ---- profile wizard collections ----
export interface Experience {
  id: number
  company: string
  title: string
  start_date?: string | null
  end_date?: string | null
  is_current?: number
  country?: string | null
  industry?: string | null
  hours_per_week?: string | null
  summary?: string | null
}

export interface Qualification {
  id: number
  institution: string
  degree: string
  field?: string | null
  year_completed?: string | null
  country?: string | null
}

export interface HeldCertification {
  id: number
  name: string
  issuer?: string | null
  credential_ref?: string | null
  issued_year?: string | null
  expires_year?: string | null
}

export interface AuthConfig {
  googleClientId: string | null
}
