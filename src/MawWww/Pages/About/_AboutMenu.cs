using MawWww.Models;

namespace MawWww.Pages.About;

public static class AboutMenu
{
    public static readonly Menu Menu = new (
        [
            new MenuItem("./Index",   "About Us"),
            new MenuItem("./News",    "News"),
            new MenuItem("./Contact", "Contact")
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
