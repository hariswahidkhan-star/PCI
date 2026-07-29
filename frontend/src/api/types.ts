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
  /** Grouping written by the backend ("Support", "Documents", "Exam Exception", …). */
  category?: string | null
  /** Optional call to action the backend attached — the step that resolves the notification. */
  cta_label?: string | null
  cta_route?: string | null
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

// ---- PCI World Passport summary (GET /api/me/world-passport/summary) ----
//
// The ONE versioned read model both surfaces consume (backend/Endpoints/PassportSummary.cs):
// the MyPCI dashboard module and the World Passport manager render the same contract, so the
// Passport a student sees in MyPCI can never drift from the one PCI World shows.

export type PassportParticipantState = 'none' | 'active' | 'link_pending'
export type PassportState = 'not_created' | 'draft' | 'private' | 'published' | 'expired' | 'suspended'

/** One disclosed evidence item. Score/profile/date are ABSENT (not null) when the owner's
 *  field-level disclosure switches them off — mirroring the public Passport page. */
export interface PassportEvidenceHighlight {
  title: string
  reference: string
  industry?: string | null
  track?: string | null
  difficulty?: string | null
  score?: number | null
  profile?: string | null
  completedOn?: string | null
}

export interface PassportSummary {
  schemaVersion: number
  studentNumber: string | null
  displayName: string | null
  participantState: PassportParticipantState
  passportState: PassportState
  completedChallengeCount: number
  selectedEvidenceCount: number
  evidenceHighlights: PassportEvidenceHighlight[]
  disclosureSettings: { scores: boolean; profiles: boolean; dates: boolean }
  /** Present ONLY when the raw public link is known to this surface. The backend stores a hash
   *  and never re-mints a token on a read, so it omits this and sets canShowPublicUrl false. */
  publicUrl?: string | null
  canShowPublicUrl: boolean
  expiresAt: string | null
  lastUpdatedAt: string | null
  availableActions: string[]
}

// ---- Event admission (spec §7A: entry passes, gates, staff scanner) ----
//
// Typed contract for the event-admission backend, which DOES NOT EXIST YET. These types mirror the
// §7A.6/§7A.7 vocabulary so the later SQL/endpoint build lands against a fixed wire shape. All
// frontend access goes through the ONE seam module (src/api/eventAdmission.ts) — when the backend
// ships, that file is the only place that changes.

/** How attendees are admitted at the gate (§7A.6). */
export type AdmissionMode = 'qr' | 'manual' | 'hybrid'

/** Registration lifecycle of the underlying event registration (§7A.6). */
export type RegistrationState = 'registered' | 'waitlisted' | 'cancelled'

/** Payment lifecycle for the registration behind a pass (§7A.6). */
export type PaymentState = 'not_required' | 'pending' | 'paid' | 'refunded' | 'waived'

/** Lifecycle of one entry pass (§7A.6). Distinct from PassportState — an event pass is NOT the
 *  public Passport and never shares its QR. */
export type PassState = 'issued' | 'active' | 'used' | 'expired' | 'revoked' | 'replaced'

export interface EventPassEvent {
  id: number
  title: string
  /** 'YYYY-MM-DD HH:MM:SS' wall-clock in the event's own timezone. */
  startsAt: string
  endsAt: string | null
  /** IANA timezone name, always shown beside times (attendees travel). */
  timezone: string
  venueName: string
  venueAddress?: string | null
}

/** The window during which the pass admits (§7A.7) — NOT the same as the event start/end. */
export interface EntryValidityWindow {
  opensAt: string
  closesAt: string
}

export type EntryHistoryKind = 'entry' | 'checkout'

/** One gate movement recorded against a pass (§7A.7). */
export interface EntryHistoryItem {
  at: string
  kind: EntryHistoryKind
  gateName: string
  method: 'qr' | 'manual'
}

export interface EventPass {
  passId: string
  /** Human-readable reference printed on the pass; what manual gate lookup matches on. */
  passReference: string
  state: PassState
  event: EventPassEvent
  registrationState: RegistrationState
  paymentState: PaymentState
  admissionMode: AdmissionMode
  validity: EntryValidityWindow
  /** Opaque single-purpose entry token behind the event QR. NEVER the public Passport QR/URL,
   *  never a session token. Null until the pass is active (e.g. unpaid / waitlisted). */
  entryToken: string | null
  history: EntryHistoryItem[]
  /** Set when this pass was replaced — the replacement is the live one. */
  replacedByPassId?: string | null
}

/** The four gate outcomes (§7A.8). The SERVER decides; the client only renders. */
export type AdmissionResult = 'admit' | 'already_checked_in' | 'manual_review' | 'do_not_admit'

/** What gate staff may see about an attendee — deliberately minimal (mirrors the wallet page's
 *  privacy note). No Passport contents, scores, or contact details. */
export interface AdmissionAttendee {
  displayName: string
  registrationReference: string
  studentNumber: string | null
  passState: PassState | null
}

export interface AdmissionDecision {
  /** Server-issued id the explicit confirm step is recorded against. */
  decisionId: string
  result: AdmissionResult
  /** Operator-facing reason (e.g. 'pass_revoked', 'outside_entry_window'). */
  reason: string | null
  attendee: AdmissionAttendee | null
  /** For already_checked_in: when/where the first entry happened. */
  firstEntryAt?: string | null
  firstEntryGate?: string | null
}

export interface EventGate {
  id: string
  name: string
}

/** An event as seen by gate staff when picking event + gate (§7A.8). */
export interface StaffEvent {
  id: number
  title: string
  startsAt: string
  timezone: string
  gates: EventGate[]
}
