import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// The app is served by the ASP.NET backend under /app/ (see backend/Program.cs SPA fallback),
// so every asset URL must be prefixed accordingly. In dev, /api is proxied to the running backend.
export default defineConfig({
  plugins: [react()],
  base: '/app/',
  build: {
    outDir: 'dist',
    emptyOutDir: true,
    sourcemap: false,
  },
  server: {
    port: 5173,
    proxy: {
      '/api': { target: 'http://localhost:8080', changeOrigin: true },
    },
  },
})
