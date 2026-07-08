import { Routes, Route, Navigate } from 'react-router-dom'
import RequireAuth from './components/RequireAuth'
import Layout from './components/Layout'
import { MeProvider } from './data/MeContext'
import Login from './pages/Login'
import Overview from './pages/Overview'
import Certifications from './pages/Certifications'
import Credentials from './pages/Credentials'
import Cpd from './pages/Cpd'
import Billing from './pages/Billing'
import Messages from './pages/Messages'
import Profile from './pages/Profile'

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route
        element={
          <RequireAuth>
            <MeProvider>
              <Layout />
            </MeProvider>
          </RequireAuth>
        }
      >
        <Route index element={<Overview />} />
        <Route path="certifications" element={<Certifications />} />
        <Route path="credentials" element={<Credentials />} />
        <Route path="cpd" element={<Cpd />} />
        <Route path="billing" element={<Billing />} />
        <Route path="messages" element={<Messages />} />
        <Route path="profile" element={<Profile />} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
