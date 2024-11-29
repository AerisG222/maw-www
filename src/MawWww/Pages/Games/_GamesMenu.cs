using MawWww.Models;

namespace MawWww.Pages.Games;

public static class GamesMenu
{
    public static readonly Menu Menu = new (
        [
            new MenuItem("./Learning",   "Learning"),
            new MenuItem("./Memory",     "Memory"),
            new MenuItem("./MoneySpin",  "Money Spin"),
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
