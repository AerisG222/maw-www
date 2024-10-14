const keyTheme = "maw-theme";
const themeDusk = "dusk";
const themeLight = "light";
const keyPrimaryNavCollapsed = "maw-primary-nav-collapsed";

document.addEventListener("DOMContentLoaded", function() {
    setTheme(getTheme());
    setPrimaryNavCollapseState(getPrimaryNavCollapseState());
});

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
        case themeDusk:
            setTheme(themeLight);
            break;
        default:
            setTheme(themeDusk);
    }
}

function validateTheme(theme: string) {
    switch(theme) {
        case themeDusk:
        case themeLight:
            return theme;
        default:
            return themeDusk;
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
