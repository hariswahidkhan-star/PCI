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
