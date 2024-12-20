import {
    defineConfig,
    presetUno,
    presetWebFonts
} from 'unocss';
import { presetDaisy } from "@ameinhardt/unocss-preset-daisy";
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
