import {
    defineConfig,
    presetAttributify,
    presetIcons,
    presetTypography,
    presetUno,
    presetWebFonts
} from 'unocss';
import { presetDaisy } from "@ameinhardt/unocss-preset-daisy";

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
        presetUno(),
        presetAttributify(),
        presetIcons({
            extraProperties: {
                "display": "inline-block",
                "vertical-align": "middle",
            }
        }),
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
    ],
    safelist: [
        "rotate-180",
        "alert-success", "alert-info", "alert-warning", "alert-error",
        "i-mdi-success-circle-outline", "i-mdi-information-outline", "i-mdi-warning-circle-outline", "i-mdi-error-outline",
        "input-validation-error",
        "mx-8", "my-8", "table-zebra"
    ],
    shortcuts: {
        "maw-primary-nav-link": "text-8 px-4 py-2 color-primary hover:color-primary-content hover:bg-primary-focus",
        // jquery validation emits the below, struggled to find a better way, so settling for the below for now
        "input-validation-error": "!border-color-error",
        //https://stackoverflow.com/questions/71296535/how-to-remove-arrow-on-input-type-number-with-tailwind-css
        "input-number": "[appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
    }
});
