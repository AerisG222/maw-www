// https://oklch.com/
// https://color.adobe.com/create/color-wheel

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
    "base-100":      "#ffffff",
    "base-200":      "#fafafe",
    "base-300":      "#f1f1f7",
    "base-content":  "#444444",

    primary:         "#eb4927",
    secondary:       "#ffb842",
    accent:          "#ffb842",
    neutral:         "#dfdfdf",
};

export const allThemes: Record<string, Record<string, string>>[] = [
    { "dark": themeDark },
    { "light": themeLight }
];

export const defaultTheme = "dusk";
