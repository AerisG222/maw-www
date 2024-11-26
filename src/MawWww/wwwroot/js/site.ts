const keyTheme = "maw-theme";
const themeDark = "dark";
const themeLight = "light";
const keyPrimaryNavCollapsed = "maw-primary-nav-collapsed";

document.addEventListener("DOMContentLoaded", function() {
    setTheme(getTheme());
    setPrimaryNavCollapseState(getPrimaryNavCollapseState());
});

function prefersDarkMode() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
}

function getTheme() {
    return validateTheme(localStorage.getItem(keyTheme));
}

function setTheme(theme: string) {
    theme = validateTheme(theme);

    localStorage.setItem(keyTheme, theme);

    document.documentElement.setAttribute("data-theme", theme);
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

function validateTheme(theme: string) {
    switch(theme) {
        case themeDark:
        case themeLight:
            return theme;
        default:
            return prefersDarkMode() ? themeDark : themeLight;
    }
}

function getPrimaryNavCollapseState() {
    return localStorage.getItem(keyPrimaryNavCollapsed) == true.toString();
}

function setPrimaryNavCollapseState(isCollapsed: boolean) {
    localStorage.setItem(keyPrimaryNavCollapsed, isCollapsed.toString());

    var els = document.getElementsByClassName("maw-primary-nav-title");

    for(var el of els) {
        (el as HTMLElement).style.display = isCollapsed ? "none" : "block";
    }

    var btn = document.getElementById("maw-primary-nav-title");

    if(isCollapsed) {
        btn.classList.add("rotate-180");
    } else {
        btn.classList.remove("rotate-180");
    }
}

function nextPrimaryNavCollapseState() {
    return !getPrimaryNavCollapseState();
}

function togglePrimaryNavCollapseState() {
    setPrimaryNavCollapseState(nextPrimaryNavCollapseState());
}
