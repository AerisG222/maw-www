import { defineConfig, loadEnv } from 'vite';
import solidPlugin from '@solidjs/vite-plugin';
import tailwindcssPlugin from '@tailwindcss/vite';

export default defineConfig(({ mode }) => {
    // SOLID_DEV=true (MawWww sets it for debug builds) bundles solid's development builds,
    // so its diagnostics reach the browser console - unminified and with source maps, so
    // they point at the source
    const solidDev = loadEnv(mode, '.', 'SOLID_').SOLID_DEV === 'true';

    return {
        envDir: "environments",
        plugins: [
            solidPlugin({ dev: solidDev || undefined }),
            tailwindcssPlugin()
        ],
        server: {
            port: 3000,
        },
        build: {
            target: 'esnext',
            minify: !solidDev,
            sourcemap: solidDev,
        },
    };
});
