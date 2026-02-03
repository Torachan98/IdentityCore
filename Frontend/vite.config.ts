import { defineConfig, loadEnv } from "vite";
import { imagetools } from "vite-imagetools";
import react from "@vitejs/plugin-react";
import path from "path";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");

  return {
    plugins: [react(), imagetools()],
    server: {
      proxy: {
        "/api": {
          target: `${env.VITE_API_BASE_URL}`,
          changeOrigin: true,
          secure: false,
        },
      },
    },
    resolve: {
      alias: {
        "@": path.resolve(__dirname, "src"),
      },
    },
  };
});
