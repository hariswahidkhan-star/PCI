import { makeClient } from '../api/client'

// Admin uses its own bearer token (from POST /api/admin/auth/login, 12h sessions) under a
// distinct storage key, fully separate from the student portal's session.
export const adminApi = makeClient('pci.admin.token')

export interface AdminMe {
  id: number
  email: string
  name: string
  role: string
  is_owner: boolean
  must_change_pw: boolean
  permissions: string[]
}

export interface AdminLoginResponse {
  token: string
  admin: { id: number; email: string; name: string; role: string; must_change_pw: boolean }
  permissions: string[]
}

export interface Kpis {
  members: number
  activeMembers: number
  pendingMembers: number
  inProgress: number
  paidSessions: number
  abandoned: number
  payCount: number
  revenue: number
  revenueMonth: number
  refunded: number
  failedPay: number
  credActive: number
  credTotal: number
  openInq: number
  examsDue: number
}

export interface Overview {
  kpis: Kpis
  revSeries: { ym: string; amount: number; n: number }[]
  productMix: { product_type: string; n: number; amount: number }[]
  funnel: { started: number; in_progress: number; paid: number }
  recent: { ts: string; action: string; details: string }[]
}

export interface MemberRow {
  id: number
  first_name: string
  last_name: string
  email: string
  status: string
  created_at: string
  membership_type?: string | null
  membership_status?: string | null
  expiry_date?: string | null
  profile?: number | null
  paid_total?: number | null
  credentials?: number | null
}

export interface MemberDetail {
  user: Record<string, unknown>
  profile: Record<string, unknown> | null
  membership: Record<string, unknown> | null
  payments: Record<string, unknown>[]
  credentials: Record<string, unknown>[]
  sessions: Record<string, unknown>[]
  emails: Record<string, unknown>[]
}

export interface PaymentRow {
  id: number
  reference?: string | null
  product_type: string
  final_amount: number
  currency?: string | null
  payment_status: string
  payment_date?: string | null
  email?: string | null
  first_name?: string | null
  last_name?: string | null
}

export interface CertRow {
  id: number
  code: string
  name: string
  description?: string | null
  credential_prefix?: string | null
  pass_mark_pct?: number | null
  duration_minutes?: number | null
  expiry_years?: number | null
  exam_price?: number | null
  active?: number | null
  sort_order?: number | null
  bank_size?: number
  entitlements?: number
  credentials?: number
}

export interface PageRow {
  id: number
  slug: string
  title?: string | null
  meta_description?: string | null
  nav_group?: string | null
  noindex?: number | null
  published?: number | null
}

export interface PageBlock {
  id: number
  slug: string
  block_key: string
  label?: string | null
  ctype?: string | null
  cvalue?: string | null
  sort_order?: number | null
}

export interface CredentialRow {
  id: number
  credential_id: string
  holder_name?: string | null
  credential?: string | null
  status: string
  issued_at?: string | null
  expires_at?: string | null
  user_id?: number | null
}

export interface TicketRow {
  id: number
  reference?: string | null
  subject?: string | null
  category?: string | null
  status: string
  updated_at?: string | null
  email?: string | null
  first_name?: string | null
  last_name?: string | null
  msg_count?: number
}

export interface TicketMessage {
  sender: string
  body: string
  created_at?: string | null
}

export interface TicketDetail extends TicketRow {
  messages: TicketMessage[]
  [k: string]: unknown
}

export interface EnrollmentRow {
  id: number
  email: string
  current_step?: string | null
  session_status: string
  selected_product?: string | null
  selected_membership?: string | null
  last_activity_at?: string | null
  created_at?: string | null
  reminders_sent?: number | null
  last_reminder_at?: string | null
  resume_link_issued?: number | null
}

export interface DiscountCode {
  id: number
  code: string
  discount_type: string
  discount_value: number
  applies_to?: string | null
  start_date?: string | null
  end_date?: string | null
  max_uses?: number | null
  used_count?: number | null
  single_use_per_email?: number | null
  active?: number | null
  code_type?: string | null
}

export interface Inquiry {
  id: number
  type?: string | null
  email?: string | null
  first_name?: string | null
  topic?: string | null
  seats?: string | null
  org?: string | null
  message?: string | null
  reference?: string | null
  status: string
  created_at?: string | null
}

export interface FormSubmission {
  id: number
  form_type?: string | null
  name?: string | null
  email?: string | null
  subject?: string | null
  message?: string | null
  reference?: string | null
  status?: string | null
  created_at?: string | null
}

export interface Review {
  id: number
  name?: string | null
  designation?: string | null
  company?: string | null
  country?: string | null
  title?: string | null
  body?: string | null
  rating?: number | null
  status: string
  featured?: number | null
  created_at?: string | null
}

export interface Subscriber {
  id: number
  email: string
  status?: string | null
  created_at?: string | null
}

export interface AuditRow {
  id: number
  admin_id?: number | null
  user_id?: number | null
  action?: string | null
  details?: string | null
  created_at?: string | null
}

export interface EmailLog {
  id: number
  user_id?: number | null
  email?: string | null
  email_type?: string | null
  subject?: string | null
  status?: string | null
  sent_at?: string | null
}

export interface SiteContentRow {
  id: number
  ckey: string
  cgroup?: string | null
  label?: string | null
  ctype?: string | null
  cvalue?: string | null
}

export interface ReportData {
  from: string
  to: string
  totals: { payments: number; revenue: number; code_discounts: number; standard_discounts: number; avg_order: number }
  revenue_daily: { d: string; revenue: number; n: number }[]
  by_product: { product_type: string; n: number; revenue: number }[]
  by_country: { country: string; n: number; revenue: number }[]
  funnel: { started: number; paid: number }
  new_members: number
}

export interface TeamMember {
  id: number
  email: string
  name?: string | null
  role: string
  permissions: string[]
  effective: string[]
  status: string
  must_change_pw?: number | null
  last_login_at?: string | null
  created_at?: string | null
}

export interface TeamResponse {
  rows: TeamMember[]
  roles: string[]
  sections: string[]
  role_grants: Record<string, string[]>
}

export interface ExamReg {
  id: number
  reference?: string | null
  product_type: string
  payment_date?: string | null
  exam_schedule_deadline?: string | null
  email?: string | null
  first_name?: string | null
  last_name?: string | null
  days_left?: number | null
}

export type Settings = Record<string, string>

export interface ExamSessionRow {
  id: number
  user_id?: number | null
  kind?: string | null
  started_at?: string | null
  submitted_at?: string | null
  percent?: number | null
  result?: string | null
  status?: string | null
  violations?: number | null
  identity_result?: string | null
  evidence_count?: number | null
  review_status?: string | null
  client_kind?: string | null
  result_status?: string | null
  hold_reason?: string | null
  released_at?: string | null
  email?: string | null
  first_name?: string | null
  last_name?: string | null
  high_events?: number | null
  // live view only
  remaining_s?: number | null
  seconds_since_beat?: number | null
  candidate_msgs?: number | null
  last_heartbeat_at?: string | null
  duration_minutes?: number | null
}

export interface ProctorEvent {
  type?: string | null
  severity?: string | null
  detail?: string | null
  evidence_ref?: string | null
  at?: string | null
}

export interface ProctorMessage {
  sender: string
  body: string
  created_at?: string | null
  delivered_at?: string | null
}

export interface ExamSessionDetail {
  attempt: Record<string, unknown>
  user: Record<string, unknown>
  booking: Record<string, unknown> | null
  events: ProctorEvent[]
  evidence: Record<string, unknown>[]
  identity: Record<string, unknown>[]
  credential: Record<string, unknown> | null
  messages: ProctorMessage[]
  summary: { total_events: number; by_severity: Record<string, number>; answered: number }
}
