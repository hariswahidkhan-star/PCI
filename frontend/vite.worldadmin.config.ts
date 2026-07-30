import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// PCI World ADMIN share console — the fourth independent bundle (/world-admin-app/). Its own build
// for the same reason the others have theirs: World-admin code must ship in neither the participant
// bundle nor the PCI Institute admin bundle. The realm separation the server enforces (own users,
// own sessions at /world-admin) is mirrored here as bundle separation.
export default defineConfig({
  plugins: [react()],
  base: '/world-admin-app/',
  build: {
    outDir: 'dist-worldadmin',
    emptyOutDir: true,
    sourcemap: false,
    rollupOptions: {
      input: 'worldadmin.html',
    },
  },
  server: {
    port: 5176,
    proxy: {
      '/api': { target: 'http://localhost:8080', changeOrigin: true },
    },
  },
})
