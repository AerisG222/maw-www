using MawWww.Models;

namespace MawWww.Pages.About;

public static class AboutMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./Index",   "About Us"),
            new SidebarMenuItem("./News",    "News"),
            new SidebarMenuItem("./Contact", "Contact")
        ]
    );
}
