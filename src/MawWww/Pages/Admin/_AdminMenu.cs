using MawWww.Models;

namespace MawWww.Pages.Admin;

public static class AdminMenu
{
    public static readonly Menu Menu = new (
        [
            new MenuItem("./CreateBlogPost",     "Create Blog Post"),
            new MenuItem("./RequestInspector",   "Request Inspector")
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
