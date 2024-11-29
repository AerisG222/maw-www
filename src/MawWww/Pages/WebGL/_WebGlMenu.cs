using MawWww.Models;

namespace MawWww.Pages.WebGL;

public static class WebGlMenu
{
    public static readonly Menu Menu = new(
        [
            new MenuItem("./Blender", "Blender Model"),
            new MenuItem("./Cube",    "Cube"),
            new MenuItem("./Shader",  "Shader"),
            new MenuItem("./Text",    "Text")
        ]
    );

    public static readonly IEnumerable<Menu> Menus = [Menu];
}
