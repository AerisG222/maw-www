import Cookies from "js-cookie";
import { getDefaultTheme, validateTheme } from './theme';
import { isBoolean } from './util';

const prefCookieName = "maw-preferences";

// we will track the preferences in a cookie so the server can set the initial classes based on the user's preferences.
// this will avoid flash of content if we just relied on client side code.  please be sure to update the server code
// as needed when changes are made below.
export type MawPreferences = {
    theme: string,
    primaryNavCollapsed: boolean
};

export function getPreferences(): MawPreferences {
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

export function setPreferences(preferences: MawPreferences) {
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

const buildDefaultPreferences = (): MawPreferences => ({
    theme: getDefaultTheme(),
    primaryNavCollapsed: false
});
