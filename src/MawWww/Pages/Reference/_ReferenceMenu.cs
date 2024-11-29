using MawWww.Models;

namespace MawWww.Pages.Reference;

public static class ReferenceMenu
{
    public static readonly Menu DotnetGuidelinesMenu = new (
        [
            new MenuItem("/Reference/Dotnet/Guidelines/Fundamentals",           "Fundamentals"),
            new MenuItem("/Reference/Dotnet/Guidelines/Naming",                 "Naming"),
            new MenuItem("/Reference/Dotnet/Guidelines/TypeDesign",             "Type Design"),
            new MenuItem("/Reference/Dotnet/Guidelines/MemberDesign",           "Member Design"),
            new MenuItem("/Reference/Dotnet/Guidelines/DesignForExtensibility", "Design for Extensibility"),
            new MenuItem("/Reference/Dotnet/Guidelines/Exceptions",             "Exceptions"),
            new MenuItem("/Reference/Dotnet/Guidelines/UsageGuidelines",        "Usage Guidelines"),
            new MenuItem("/Reference/Dotnet/Guidelines/CommonPatterns",         "Common Patterns")
        ],
        ".Net Design Guidelines"
    );

    public static readonly Menu HtmlMenu = new (
        [
            new MenuItem("/Reference/Html/ColorCodes", "Color Codes"),
            new MenuItem("/Reference/Html/Doctypes",   "Doctypes"),
            new MenuItem("/Reference/Html/Entities",   "Entities")
        ],
        "HTML"
    );

    public static readonly Menu HttpMenu = new (
        [
            new MenuItem("/Reference/Http/StatusCodes", "Status Codes"),
            new MenuItem("/Reference/Http/Rfcs",        "RFCs")
        ],
        "HTTP"
    );

    public static readonly IEnumerable<Menu> Menus = [
        DotnetGuidelinesMenu,
        HtmlMenu,
        HttpMenu
    ];
}
