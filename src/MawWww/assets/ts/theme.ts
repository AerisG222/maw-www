import { getPreferences, setPreferences } from "./preferences";
import { isString } from "./util";

const themeDark = "dark";
const themeLight = "light";

const getTheme = () => getPreferences().theme;
const prefersDarkMode = () => window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

export const getDefaultTheme = () => prefersDarkMode() ? themeDark : themeLight;

export function watchThemeSwitcher() {
    var themeSwitcher = document.getElementById("maw-theme-switcher");

    if(themeSwitcher) {
        themeSwitcher.onclick = () => {
            nextTheme();
            return false;
        }
    }
}

export function validateTheme(theme: unknown) {
    if(!isString(theme)) {
        return false;
    }

    return theme === themeDark ||
        theme === themeLight;
}

function setTheme(theme: string) {
    if(!validateTheme(theme)) {
        theme = getTheme();
    }

    document.documentElement.setAttribute("data-theme", theme);

    setPreferences({...getPreferences(), theme});
}

function nextTheme() {
    var currTheme = getTheme();

    switch(currTheme) {
        case themeDark:
            setTheme(themeLight);
            break;
        default:
            setTheme(themeDark);
    }
}
