import { watchMobileSidebar } from "./mobile-sidebar";
import { watchThemeSwitcher } from "./theme";
import { watchPrimaryNavCollapse } from "./primary-nav-collapse";

document.addEventListener("DOMContentLoaded", function() {
    watchThemeSwitcher();
    watchPrimaryNavCollapse();
    watchMobileSidebar();
});
