import { defineConfig } from 'vite';

export default defineConfig({
    build: {
        emptyOutDir: false,
        outDir: 'wwwroot/js',
        lib: {
            entry: 'assets/ts/site.ts',
            name: 'MaW',
            fileName: (format, name) => `${name}.min.js`,
            formats: ['es']
        },
        sourcemap: true
    }
});
