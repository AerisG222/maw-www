using www.Models;

namespace www.Pages.About;

public static class AboutMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./index",   "About Us"),
            new SidebarMenuItem("./news",    "News"),
            new SidebarMenuItem("./contact", "Contact")
        ]
    );
}
