import {
    defineConfig,
    presetTypography,
    presetUno
} from "unocss";
import { presetDaisy } from "@ameinhardt/unocss-preset-daisy";
import { allThemes } from '../MawWww/theme';

export default defineConfig({
    presets: [
        presetUno({ prefix: 'maw-', safeList: [
            'maw-bg-content'
        ]}),
        presetDaisy({
            themes: allThemes,
            prefix: 'maw-',
            safeList: [
                'maw-bg-content'
            ]
        }),
        presetTypography()
    ]
});
