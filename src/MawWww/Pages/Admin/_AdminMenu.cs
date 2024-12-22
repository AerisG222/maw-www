using MawWww.Models;

namespace MawWww.Pages.Admin;

public static class AdminMenu
{
    public static readonly Menu Menu = new (
        [
            new MenuItem("./Index",   "Admin"),
            new MenuItem("./Blog",    "Blog")
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
