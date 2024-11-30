import {
    defineConfig,
    presetUno,
    presetWebFonts
} from 'unocss';
import { presetDaisy } from "@aerisg222/unocss-preset-daisyui";
import { allThemes } from '../MawWww/theme';

export default defineConfig({
    presets: [
        presetUno(),
        presetDaisy({
            themes: allThemes
        }),
        presetWebFonts({
            provider: "google",
            fonts: {
                cursive: "Permanent Marker"
            }
        }),
    ]
});
