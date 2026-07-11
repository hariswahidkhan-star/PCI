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
import Billing from './pages/Billing'
import Resources from './pages/Resources'
import Messages from './pages/Messages'
import Support from './pages/Support'
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
          <Route path="billing" element={<Billing />} />
          <Route path="resources" element={<Resources />} />
          <Route path="messages" element={<Messages />} />
          <Route path="support" element={<Support />} />
          <Route path="profile" element={<Profile />} />
        </Route>
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
