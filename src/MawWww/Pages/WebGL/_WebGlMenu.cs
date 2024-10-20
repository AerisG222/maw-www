using MawWww.Models;

namespace MawWww.Pages.WebGL;

public static class WebGlMenu
{
    public static readonly SidebarMenu Menu = new (
        [
            new SidebarMenuItem("./blender",   "Blender Model"),
            new SidebarMenuItem("./cube",      "Cube"),
            new SidebarMenuItem("./shader",    "Shader"),
            new SidebarMenuItem("./text",      "Text")
        ],
        "WebGL Demos"
    );
}
