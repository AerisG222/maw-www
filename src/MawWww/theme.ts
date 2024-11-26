// https://oklch.com/
// https://color.adobe.com/create/color-wheel
// https://coolors.co/palettes/popular

const themeDark: Record<string, string> = {
    "base-100":        "#15151f",
    "base-200":        "#20202b",
    "base-300":        "#2c2d38",
    "base-content":    "#8a8b98",

    primary:           "#efb300",
    secondary:         "#7f75b8",
    accent:            "#7f75b8",
    neutral:           "#2c2d38",
};

const themeLight: Record<string, string> = {
    "base-100":      "#fff",
    "base-200":      "#f6f6f6",
    "base-300":      "#e9e9e9",
    "base-content":  "#333333",

    primary:         "#001845",
    secondary:       "#0353a4",
    accent:          "#33415c",
    neutral:         "#979dac",
};

export const allThemes: Record<string, Record<string, string>>[] = [
    { "dark": themeDark },
    { "light": themeLight }
];

export const defaultTheme = "dusk";
