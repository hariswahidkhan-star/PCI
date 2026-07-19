import { Routes, Route, Navigate } from 'react-router-dom'
import RequireAdmin from './RequireAdmin'
import AdminLayout from './AdminLayout'
import { useAdminAuth } from './AdminAuth'
import AdminLogin from './pages/AdminLogin'
import AdminResetPassword from './pages/AdminResetPassword'
import Dashboard from './pages/Dashboard'
import Students from './pages/Students'
import Payments from './pages/Payments'
import Certifications from './pages/Certifications'
import Pages from './pages/Pages'
import Credentials from './pages/Credentials'
import Tickets from './pages/Tickets'
import Documents from './pages/Documents'
import Books from './pages/Books'
import SupportInbox from './pages/SupportInbox'
import ErrorReports from './pages/ErrorReports'
import Enrollments from './pages/Enrollments'
import Codes from './pages/Codes'
import Founding from './pages/Founding'
import Applications from './pages/Applications'
import Honorary from './pages/Honorary'
import HonoraryApplications from './pages/HonoraryApplications'
import Translations from './pages/Translations'
import Seo from './pages/Seo'
import Analytics from './pages/Analytics'
import AiVisibility from './pages/AiVisibility'
import TrainingPartners from './pages/TrainingPartners'
import Integrations from './pages/Integrations'
import Marketing from './pages/Marketing'
import ExamDelivery from './pages/ExamDelivery'
import Enquiries from './pages/Enquiries'
import Submissions from './pages/Submissions'
import Reviews from './pages/Reviews'
import Subscribers from './pages/Subscribers'
import Audit from './pages/Audit'
import Reports from './pages/Reports'
import Emails from './pages/Emails'
import Content from './pages/Content'
import Announcement from './pages/Announcement'
import Team from './pages/Team'
import Settings from './pages/Settings'
import Exams from './pages/Exams'
import Proctoring from './pages/Proctoring'
import CrudSection from './CrudSection'
import { CRUD_SECTIONS } from './crudConfigs'
import { ErrorNote } from '../components/ui'
import type { ReactNode } from 'react'

// Section-level permission gate (defence in depth; the server enforces the same on every call).
function Perm({ section, children }: { section: string; children: ReactNode }) {
  const { can } = useAdminAuth()
  if (!can(section)) return <ErrorNote>You do not have permission to view this section.</ErrorNote>
  return <>{children}</>
}

function OwnerOnly({ children }: { children: ReactNode }) {
  const { me } = useAdminAuth()
  if (!me?.is_owner) return <ErrorNote>This section is available to owners only.</ErrorNote>
  return <>{children}</>
}

function AnyPerm({ sections, children }: { sections: string[]; children: ReactNode }) {
  const { me, can } = useAdminAuth()
  if (!me?.is_owner && !sections.some((s) => can(s))) return <ErrorNote>You do not have permission to view this section.</ErrorNote>
  return <>{children}</>
}

export default function AdminApp() {
  return (
    <Routes>
      <Route path="/login" element={<AdminLogin />} />
      <Route path="/reset-password" element={<AdminResetPassword />} />
      <Route
        element={
          <RequireAdmin>
            <AdminLayout />
          </RequireAdmin>
        }
      >
        <Route index element={<Dashboard />} />
        <Route path="students" element={<Perm section="members"><Students /></Perm>} />
        <Route path="enrollments" element={<Perm section="enrollments"><Enrollments /></Perm>} />
        <Route path="payments" element={<Perm section="payments"><Payments /></Perm>} />
        <Route path="credentials" element={<Perm section="credentials"><Credentials /></Perm>} />
        <Route path="tickets" element={<Perm section="tickets"><Tickets /></Perm>} />
        <Route path="documents" element={<Perm section="documents"><Documents /></Perm>} />
        <Route path="books" element={<Perm section="resources"><Books /></Perm>} />
        <Route path="support-inbox" element={<Perm section="inbox"><SupportInbox /></Perm>} />
        <Route path="errors" element={<Perm section="inbox"><ErrorReports /></Perm>} />
        <Route path="certifications" element={<Perm section="exams"><Certifications /></Perm>} />
        <Route path="applications" element={<Perm section="members"><Applications /></Perm>} />
        <Route path="codes" element={<Perm section="codes"><Codes /></Perm>} />
        <Route path="founding" element={<AnyPerm sections={['members', 'codes']}><Founding /></AnyPerm>} />
        <Route path="honorary" element={<OwnerOnly><Honorary /></OwnerOnly>} />
        <Route path="honorary-applications" element={<OwnerOnly><HonoraryApplications /></OwnerOnly>} />
        <Route path="pages" element={<Perm section="pages"><Pages /></Perm>} />
        <Route path="enquiries" element={<Perm section="inquiries"><Enquiries /></Perm>} />
        <Route path="submissions" element={<Perm section="submissions"><Submissions /></Perm>} />
        <Route path="reviews" element={<Perm section="content"><Reviews /></Perm>} />
        <Route path="content" element={<Perm section="content"><Content /></Perm>} />
        <Route path="announcement" element={<Perm section="content"><Announcement /></Perm>} />
        <Route path="translations" element={<OwnerOnly><Translations /></OwnerOnly>} />
        <Route path="seo" element={<Perm section="pages"><Seo /></Perm>} />
        <Route path="analytics" element={<Perm section="reports"><Analytics /></Perm>} />
        <Route path="ai-visibility" element={<Perm section="pages"><AiVisibility /></Perm>} />
        <Route path="training-partners" element={<Perm section="partners"><TrainingPartners /></Perm>} />
        <Route path="integrations" element={<Perm section="integrations"><Integrations /></Perm>} />
        <Route path="marketing" element={<AnyPerm sections={['subscribers', 'reports']}><Marketing /></AnyPerm>} />
        <Route path="exam-delivery" element={<Perm section="exam_delivery"><ExamDelivery /></Perm>} />
        <Route path="subscribers" element={<Perm section="subscribers"><Subscribers /></Perm>} />
        <Route path="reports" element={<Perm section="reports"><Reports /></Perm>} />
        <Route path="emails" element={<Perm section="emails"><Emails /></Perm>} />
        <Route path="audit" element={<Perm section="audit"><Audit /></Perm>} />
        <Route path="exams" element={<Perm section="exams"><Exams /></Perm>} />
        <Route path="proctoring" element={<Perm section="proctoring"><Proctoring /></Perm>} />
        <Route path="settings" element={<AnyPerm sections={['settings', 'set_web', 'set_sp', 'set_exam']}><Settings /></AnyPerm>} />
        <Route path="team" element={<OwnerOnly><Team /></OwnerOnly>} />
        {CRUD_SECTIONS.map((c) => (
          <Route key={c.path} path={c.path} element={<Perm section={c.perm}><CrudSection config={c} /></Perm>} />
        ))}
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
