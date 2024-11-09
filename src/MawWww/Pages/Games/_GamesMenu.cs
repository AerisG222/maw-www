using MawWww.Models;

namespace MawWww.Pages.Games;

public static class GamesMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./Memory",     "Memory"),
            new SidebarMenuItem("./MoneySpin",  "Money Spin"),
        ]
    );
}
