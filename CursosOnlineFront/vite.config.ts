import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,       // el puerto donde correrá React, cambia si quieres otro
    watch: {
      usePolling: true, // <--- importante para evitar EMFILE
      interval: 100     // revisa cambios cada 100ms
    }
  }
})
