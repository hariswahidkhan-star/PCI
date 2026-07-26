import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// PCI World participant app — the third independent bundle (student portal /app/, admin console
// /admin/, World /world-app/). A separate build keeps World code out of both other bundles and is
// the stepping stone to the dedicated-domain deployment: this same app moves origins by changing
// `base` and registering the new redirect URI in the OAuth client registry — no code change.
export default defineConfig({
  plugins: [react()],
  base: '/world-app/',
  build: {
    outDir: 'dist-world',
    emptyOutDir: true,
    sourcemap: false,
    rollupOptions: {
      input: 'world.html',
    },
  },
  server: {
    port: 5175,
    proxy: {
      '/api': { target: 'http://localhost:8080', changeOrigin: true },
    },
  },
})
