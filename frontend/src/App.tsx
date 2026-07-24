import { Routes, Route, Navigate, Outlet } from 'react-router-dom'
import RequireAuth from './components/RequireAuth'
import Layout from './components/Layout'
import { MeProvider } from './data/MeContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Onboarding from './pages/Onboarding'
import Overview from './pages/Overview'
import Certifications from './pages/Certifications'
import Credentials from './pages/Credentials'
import Cpd from './pages/Cpd'
import Certuvo from './pages/Certuvo'
import Lab from './pages/Lab'
import LabRunner from './pages/LabRunner'
import Billing from './pages/Billing'
import Resources from './pages/Resources'
import Templates from './pages/Templates'
import Documents from './pages/Documents'
import Messages from './pages/Messages'
import Support from './pages/Support'
import Appeals from './pages/Appeals'
import Events from './pages/Events'
import Profile from './pages/Profile'

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route
        element={
          <RequireAuth>
            <MeProvider>
              <Outlet />
            </MeProvider>
          </RequireAuth>
        }
      >
        {/* full-screen first-run wizard (no sidebar chrome) */}
        <Route path="onboarding" element={<Onboarding />} />
        <Route element={<Layout />}>
          <Route index element={<Overview />} />
          <Route path="certifications" element={<Certifications />} />
          <Route path="credentials" element={<Credentials />} />
          <Route path="cpd" element={<Cpd />} />
          <Route path="certuvo" element={<Certuvo />} />
          <Route path="lab" element={<Lab />} />
          <Route path="lab/:code" element={<LabRunner />} />
          <Route path="billing" element={<Billing />} />
          <Route path="resources" element={<Resources />} />
          <Route path="templates" element={<Templates />} />
          <Route path="events" element={<Events />} />
          <Route path="documents" element={<Documents />} />
          <Route path="messages" element={<Messages />} />
          <Route path="support" element={<Support />} />
          <Route path="appeals" element={<Appeals />} />
          <Route path="profile" element={<Profile />} />
        </Route>
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
