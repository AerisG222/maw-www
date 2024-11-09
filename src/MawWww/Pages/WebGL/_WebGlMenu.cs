using MawWww.Models;
using Microsoft.AspNetCore.Mvc;

namespace MawWww.Pages.WebGL;

public static class WebGlMenu
{
    public static SidebarMenu GenerateMenu(IUrlHelper urlHelper) {
        return new([
            new SidebarMenuItem("./Blender", "Blender Model"),
            new SidebarMenuItem("./Cube",    "Cube"),
            new SidebarMenuItem("./Shader",  "Shader"),
            new SidebarMenuItem("./Text",    "Text")
        ]);
    }
}
