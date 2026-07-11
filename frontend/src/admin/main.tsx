import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import AdminApp from './AdminApp'
import { AdminAuthProvider } from './AdminAuth'
import '../styles.css'

// Last-resort boundary: an uncaught render exception (e.g. an unexpected null API shape) would
// otherwise unmount the whole tree to a blank page. Catch it and offer a recoverable reload.
class ErrorBoundary extends React.Component<{ children: React.ReactNode }, { hasError: boolean }> {
  state = { hasError: false }
  static getDerivedStateFromError() {
    return { hasError: true }
  }
  componentDidCatch(error: unknown) {
    console.error('Unhandled render error:', error)
  }
  render() {
    if (this.state.hasError) {
      return (
        <div style={{ display: 'grid', placeItems: 'center', minHeight: '100vh', padding: '2rem', textAlign: 'center' }}>
          <div style={{ maxWidth: 420 }}>
            <h1 style={{ fontSize: '1.35rem', marginBottom: '.5rem' }}>Something went wrong</h1>
            <p style={{ color: '#64748b', marginBottom: '1.25rem' }}>
              An unexpected error interrupted the admin console. Reloading usually fixes it.
            </p>
            <button className="btn" onClick={() => window.location.reload()}>Reload the page</button>
          </div>
        </div>
      )
    }
    return this.props.children
  }
}

// Served by the backend under /admin/ (see backend/Program.cs SPA fallback).
ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ErrorBoundary>
      <BrowserRouter basename="/admin">
        <AdminAuthProvider>
          <AdminApp />
        </AdminAuthProvider>
      </BrowserRouter>
    </ErrorBoundary>
  </React.StrictMode>,
)
