// Google Identity Services loader. The official button renders only when the backend reports a
// configured GOOGLE_CLIENT_ID (/api/auth/config); nothing Google-related loads otherwise.
let gsiPromise: Promise<void> | null = null

function loadGsi(): Promise<void> {
  if (!gsiPromise) {
    gsiPromise = new Promise((resolve, reject) => {
      const s = document.createElement('script')
      s.src = 'https://accounts.google.com/gsi/client'
      s.async = true
      s.defer = true
      s.onload = () => resolve()
      s.onerror = () => { gsiPromise = null; reject(new Error('Google sign-in is unavailable right now.')) }
      document.head.appendChild(s)
    })
  }
  return gsiPromise
}

export async function renderGoogleButton(
  el: HTMLElement,
  clientId: string,
  onCredential: (credential: string) => void,
): Promise<void> {
  await loadGsi()
  const g = (window as unknown as { google?: any }).google
  if (!g?.accounts?.id) throw new Error('Google sign-in is unavailable right now.')
  g.accounts.id.initialize({ client_id: clientId, callback: (r: { credential: string }) => onCredential(r.credential) })
  g.accounts.id.renderButton(el, { theme: 'outline', size: 'large', width: 320, text: 'continue_with' })
}
