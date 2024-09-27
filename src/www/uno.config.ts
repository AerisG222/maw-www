import { defineConfig, presetUno } from 'unocss';
import presetIcons from '@unocss/preset-icons';
import presetWebFonts from "@unocss/preset-web-fonts";
import { presetDaisy } from "@logs404/unocss-preset-daisy";

import { allThemes } from './theme';

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
        presetIcons({
            extraProperties: {
                "display": "inline-block",
                "vertical-align": "middle",
            }
        }),
        presetWebFonts({
            provider: "google",
            fonts: {
                brand: "Tangerine"
            }
        }),
        presetUno(),
        presetDaisy({
            themes: allThemes
        }),
    ],
    shortcuts: {
        'maw-primary-nav-link': 'text-8 px-4 py-2 color-primary hover:color-primary-content hover:bg-primary-focus'
    }
});
