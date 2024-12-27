import { getPreferences, setPreferences } from "./preferences";

const getPrimaryNavCollapseState = () => getPreferences().primaryNavCollapsed;
const nextPrimaryNavCollapseState = () => !getPrimaryNavCollapseState();
const togglePrimaryNavCollapseState = () => setPrimaryNavCollapseState(nextPrimaryNavCollapseState());

export function watchPrimaryNavCollapse() {
    var collapseButton = document.getElementById("maw-primary-nav-collapse-button") as HTMLButtonElement;

    if(collapseButton) {
        collapseButton.onclick = () => {
            togglePrimaryNavCollapseState();
            return false;
        }
    }
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
