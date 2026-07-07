import { Routes, Route, Navigate } from 'react-router-dom'
import RequireAdmin from './RequireAdmin'
import AdminLayout from './AdminLayout'
import { useAdminAuth } from './AdminAuth'
import AdminLogin from './pages/AdminLogin'
import Dashboard from './pages/Dashboard'
import Students from './pages/Students'
import Payments from './pages/Payments'
import Certifications from './pages/Certifications'
import Pages from './pages/Pages'
import Credentials from './pages/Credentials'
import Tickets from './pages/Tickets'
import Enrollments from './pages/Enrollments'
import Codes from './pages/Codes'
import Enquiries from './pages/Enquiries'
import Submissions from './pages/Submissions'
import Reviews from './pages/Reviews'
import Subscribers from './pages/Subscribers'
import Audit from './pages/Audit'
import Reports from './pages/Reports'
import Emails from './pages/Emails'
import Content from './pages/Content'
import { ErrorNote } from '../components/ui'
import type { ReactNode } from 'react'

// Section-level permission gate (defence in depth; the server enforces the same on every call).
function Perm({ section, children }: { section: string; children: ReactNode }) {
  const { can } = useAdminAuth()
  if (!can(section)) return <ErrorNote>You do not have permission to view this section.</ErrorNote>
  return <>{children}</>
}

export default function AdminApp() {
  return (
    <Routes>
      <Route path="/login" element={<AdminLogin />} />
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
        <Route path="certifications" element={<Perm section="exams"><Certifications /></Perm>} />
        <Route path="codes" element={<Perm section="codes"><Codes /></Perm>} />
        <Route path="pages" element={<Perm section="pages"><Pages /></Perm>} />
        <Route path="enquiries" element={<Perm section="inquiries"><Enquiries /></Perm>} />
        <Route path="submissions" element={<Perm section="submissions"><Submissions /></Perm>} />
        <Route path="reviews" element={<Perm section="content"><Reviews /></Perm>} />
        <Route path="content" element={<Perm section="content"><Content /></Perm>} />
        <Route path="subscribers" element={<Perm section="subscribers"><Subscribers /></Perm>} />
        <Route path="reports" element={<Perm section="reports"><Reports /></Perm>} />
        <Route path="emails" element={<Perm section="emails"><Emails /></Perm>} />
        <Route path="audit" element={<Perm section="audit"><Audit /></Perm>} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
