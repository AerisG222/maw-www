import { createSignal, onCleanup, type Accessor } from "solid-js";

export type SiteTheme = "light" | "dark";

// the host page owns the theme: the server sets data-theme on <html> from the preferences
// cookie and the site's theme switcher changes it in place.  with no preference saved yet the
// attribute is empty and daisyui falls back to its default theme, so rather than parse the
// attribute, read the color-scheme each daisyui theme sets - that is what is actually shown
const readSiteTheme = (): SiteTheme =>
    getComputedStyle(document.documentElement).colorScheme === "dark" ? "dark" : "light";

// follows the page's resolved theme.  must be called under an owner (a component), which
// stops watching when it is disposed
export function createSiteTheme(): Accessor<SiteTheme> {
    const [theme, setTheme] = createSignal(readSiteTheme());
    const update = () => setTheme(readSiteTheme());

    const observer = new MutationObserver(update);
    observer.observe(document.documentElement, { attributes: true, attributeFilter: ["data-theme"] });

    // the fallback theme can be chosen by the os setting, so follow that too
    const prefersDark = window.matchMedia("(prefers-color-scheme: dark)");
    prefersDark.addEventListener("change", update);

    onCleanup(() => {
        observer.disconnect();
        prefersDark.removeEventListener("change", update);
    });

    return theme;
}
