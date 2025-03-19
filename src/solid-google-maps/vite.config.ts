import { defineConfig } from 'vite';
import solidPlugin from 'vite-plugin-solid';
import tailwindcssPlugin from '@tailwindcss/vite';

export default defineConfig({
    plugins: [
        solidPlugin(),
        tailwindcssPlugin()
    ],
    server: {
        port: 3000,
    },
    build: {
        target: 'esnext',
    },
});
