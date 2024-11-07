using MawWww.Models;

namespace MawWww.Pages.Games;

public static class GamesMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./memory",     "Memory"),
            new SidebarMenuItem("./money-spin", "Money Spin"),
        ],
        "Games"
    );
}
