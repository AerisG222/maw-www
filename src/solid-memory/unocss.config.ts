import { presetUno } from "@unocss/preset-uno";
import { defineConfig } from 'unocss/vite';
import presetWebFonts from "@unocss/preset-web-fonts";
import { Style } from './src/Style';

export default defineConfig({
    presets: [
        presetUno(),
        presetWebFonts({
            provider: "google",
            fonts: {
                cursive: "Permanent Marker"
            }
        })
    ],
    safelist: [
        ...Object.values(Style)
    ]
});
