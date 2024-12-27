import { defineConfig } from 'vite';

export default defineConfig({
    build: {
        emptyOutDir: false,
        outDir: 'wwwroot/js',
        lib: {
            entry: 'wwwroot/js/site.ts',
            name: 'MaW',
            fileName: (format, name) => `${name}.js`,
            formats: ['es']
        },
        sourcemap: true
    }
});
