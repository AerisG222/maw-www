import type { Config } from 'tailwindcss'
import daisyui from "daisyui";

export default {
    content: [
        './Pages/**/*.cshtml'
    ],
    theme: {
        extend: {},
    },
    plugins: [
        daisyui
    ],
} satisfies Config
