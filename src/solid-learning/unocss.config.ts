import {
    defineConfig,
    presetUno,
    presetWebFonts
} from "unocss";
import { presetDaisy } from "@aerisg222/unocss-preset-daisyui";
import { allThemes } from '../MawWww/theme';

export default defineConfig({
    presets: [
        presetUno({ prefix: 'maw-' }),
        presetDaisy({
            themes: allThemes,
            prefix: 'maw-'
        }),
        presetWebFonts({
            provider: "google",
            fonts: {
                brand: "Inter"
            }
        })
    ]
});
