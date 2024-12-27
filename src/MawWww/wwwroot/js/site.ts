import Cookies from "js-cookie";

document.addEventListener("DOMContentLoaded", function() {
    watchThemeSwitcher();
    watchPrimaryNavCollapse();
    watchMobileSidebar();
});

const themeDark = "dark";
const themeLight = "light";
const prefCookieName = "maw-preferences";

// we will track the preferences in a cookie so the server can set the initial classes based on the user's preferences.
// this will avoid flash of content if we just relied on client side code.  please be sure to update the server code
// as needed when changes are made below.
type MawPreferences = {
    theme: string,
    primaryNavCollapsed: boolean
};

const getTheme = () => getPreferences().theme;
const getDefaultTheme = () => prefersDarkMode() ? themeDark : themeLight;
const prefersDarkMode = () => window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

const getPrimaryNavCollapseState = () => getPreferences().primaryNavCollapsed;
const nextPrimaryNavCollapseState = () => !getPrimaryNavCollapseState();
const isString = (value: unknown) => typeof value === "string";
const isBoolean = (value: unknown) => typeof value === "boolean";

const togglePrimaryNavCollapseState = () => setPrimaryNavCollapseState(nextPrimaryNavCollapseState());

const buildDefaultPreferences = (): MawPreferences => ({
    theme: getDefaultTheme(),
    primaryNavCollapsed: false
});

function watchThemeSwitcher() {
    var themeSwitcher = document.getElementById("maw-theme-switcher");

    if(themeSwitcher) {
        themeSwitcher.onclick = () => {
            nextTheme();
            return false;
        }
    }
}

function watchPrimaryNavCollapse() {
    var collapseButton = document.getElementById("maw-primary-nav-collapse-button") as HTMLButtonElement;

    if(collapseButton) {
        collapseButton.onclick = () => {
            togglePrimaryNavCollapseState();
            return false;
        }
    }
}

function setTheme(theme: string) {
    if(!validateTheme(theme)) {
        theme = getTheme();
    }

    document.documentElement.setAttribute("data-theme", theme);

    setPreferences({...getPreferences(), theme});
}

export function nextTheme() {
    var currTheme = getTheme();

    switch(currTheme) {
        case themeDark:
            setTheme(themeLight);
            break;
        default:
            setTheme(themeDark);
    }
}

function validateTheme(theme: unknown) {
    if(!isString(theme)) {
        return false;
    }

    return theme === themeDark ||
        theme === themeLight;
}

function setPrimaryNavCollapseState(isCollapsed: boolean) {
    const els = document.getElementsByClassName("maw-primary-nav-title");

    for(const el of els) {
        // only force display to none when requesting collapse
        // otherwise, allow (responsive) class styles to determine display type
        (el as HTMLElement).style.display = isCollapsed ? "none" : "";
    }

    var btn = document.getElementById("maw-primary-nav-title");

    if(btn) {
        if(isCollapsed) {
            btn.classList.add("i-mdi-chevron-double-right");
            btn.classList.remove("i-mdi-chevron-double-left");
        } else {
            btn.classList.add("i-mdi-chevron-double-left");
            btn.classList.remove("i-mdi-chevron-double-right");
        }
    }

    setPreferences({...getPreferences(), primaryNavCollapsed: isCollapsed});
}

function watchMobileSidebar() {
    var menuToggleCheckbox = document.getElementById("sidebar-menu-toggle") as HTMLInputElement;

    if(menuToggleCheckbox) {
        menuToggleCheckbox.onchange = toggleMobileMenuVisibility
    }
}

function toggleMobileMenuVisibility(ev: Event) {
    var sidebarMenu = document.getElementById("sidebar-menu") as HTMLElement;

    if(sidebarMenu) {
        if((ev.target as HTMLInputElement).checked) {
            sidebarMenu.classList.add("show");
            sidebarMenu.classList.remove("hidden");
        } else {
            sidebarMenu.classList.add("hidden");
            sidebarMenu.classList.remove("show");
        }
    }
}

function getPreferences(): MawPreferences {
    var cookie = Cookies.get(prefCookieName);

    if(cookie) {
        try {
            var dirty = false;
            const prefs = JSON.parse(cookie) as MawPreferences;

            if(!validateTheme(prefs.theme)) {
                prefs.theme = getDefaultTheme();
                dirty = true;
            }

            if(!isBoolean(prefs.primaryNavCollapsed)) {
                prefs.primaryNavCollapsed = false;
                dirty = true;
            }

            if(dirty) {
                setPreferences(prefs);
            }

            return prefs;
        } catch(err) {
            console.error(`Unable to load preferences from cookie: ${err}`);
        }
    }

    const defaultPreferences = buildDefaultPreferences();

    setPreferences(defaultPreferences);

    return defaultPreferences;
}

function setPreferences(preferences: MawPreferences) {
    Cookies.set(
        prefCookieName,
        JSON.stringify(preferences),
        {
            expires: 365,
            secure: window.location.protocol === "https:",
            sameSite: "strict"
        }
    );
}
