
export function watchMobileSidebar() {
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
