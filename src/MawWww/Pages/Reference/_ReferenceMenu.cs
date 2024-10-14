using MawWww.Models;

namespace MawWww.Pages.Reference;

public static class ReferenceMenu
{
    public static SidebarMenu DotnetGuidelinesMenu = new (
        [
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/Fundamentals",           "Fundamentals"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/Naming",                 "Naming"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/TypeDesign",             "Type Design"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/MemberDesign",           "Member Design"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/DesignForExtensibility", "Design for Extensibility"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/Exceptions",             "Exceptions"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/UsageGuidelines",        "Usage Guidelines"),
            new SidebarMenuItem("/Reference/Dotnet/Guidelines/CommonPatterns",         "Common Patterns")
        ],
        ".Net Design Guidelines"
    );

    public static SidebarMenu HtmlMenu = new (
        [
            new SidebarMenuItem("/Reference/Html/ColorCodes", "Color Codes"),
            new SidebarMenuItem("/Reference/Html/Doctypes",   "Doctypes"),
            new SidebarMenuItem("/Reference/Html/Entities",   "Entities")
        ],
        "HTML"
    );

    public static SidebarMenu HttpMenu = new (
        [
            new SidebarMenuItem("/Reference/Http/StatusCodes", "Status Codes"),
            new SidebarMenuItem("/Reference/Http/Rfcs",        "RFCs")
        ],
        "HTTP"
    );
}
