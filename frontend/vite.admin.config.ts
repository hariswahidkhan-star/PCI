import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Admin console — a separate app/bundle from the student portal, served by the backend under /admin/.
// Building it independently (its own base + output dir) keeps admin code out of the student bundle.
export default defineConfig({
  plugins: [react()],
  base: '/admin/',
  build: {
    outDir: 'dist-admin',
    emptyOutDir: true,
    sourcemap: false,
    rollupOptions: {
      input: 'admin.html',
    },
  },
  server: {
    port: 5174,
    proxy: {
      '/api': { target: 'http://localhost:8080', changeOrigin: true },
    },
  },
})
