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
        <Route path="payments" element={<Perm section="payments"><Payments /></Perm>} />
        <Route path="certifications" element={<Perm section="exams"><Certifications /></Perm>} />
        <Route path="pages" element={<Perm section="pages"><Pages /></Perm>} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
