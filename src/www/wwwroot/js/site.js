setTheme(getTheme());
function getTheme() {
    return validateTheme(localStorage.getItem("maw-theme"));
}
function setTheme(theme) {
    theme = validateTheme(theme);
    localStorage.setItem("maw-theme", theme);
    document.documentElement.setAttribute("data-theme", theme);
}
function nextTheme() {
    var currTheme = getTheme();
    switch (currTheme) {
        case 'dusk':
            setTheme('light');
            break;
        default:
            setTheme('dusk');
    }
}
function validateTheme(theme) {
    switch (theme) {
        case 'dusk':
        case 'light':
            return theme;
        default:
            return 'dusk';
    }
}
