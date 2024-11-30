import {
    defineConfig,
    presetAttributify,
    presetTypography,
    presetUno,
    presetWebFonts
} from 'unocss';
import { presetDaisy } from "@aerisg222/unocss-preset-daisyui";
import { allThemes } from '../MawWww/theme';

export default defineConfig({
    cli: {
        entry: {
            patterns: [
                './Pages/**/*.cshtml'
            ],
            outFile: './wwwroot/css/styles.css'
        }
    },
    presets: [
        presetUno(),
        presetAttributify(),
        presetDaisy({
            themes: allThemes
        }),
        presetTypography(),
        presetWebFonts({
            provider: "google",
            fonts: {
                brand: "Tangerine"
            }
        }),
    ]
});
