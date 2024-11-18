using MawWww.Models;

namespace MawWww.Pages.Tools;

public static class ToolsMenu
{
    public static SidebarMenu DotnetMenu = new (
        [
            new SidebarMenuItem("/Tools/Dotnet/RegularExpressions", "Regular Expressions"),
            new SidebarMenuItem("/Tools/Dotnet/DateFormats",        "Date Formats"),
            new SidebarMenuItem("/Tools/Dotnet/NumberFormats",      "Number Formats"),
            new SidebarMenuItem("/Tools/Dotnet/GuidGenerator",      "GUID Generator"),
            new SidebarMenuItem("/Tools/Dotnet/Paths",              "Paths"),
            new SidebarMenuItem("/Tools/Dotnet/XmlValidator",       "XML Validator"),
            new SidebarMenuItem("/Tools/Dotnet/XsdValidator",       "XSD Validator"),
            new SidebarMenuItem("/Tools/Dotnet/XslTransform",       "XSL Transform")
        ],
        ".NET"
    );

    public static SidebarMenu GeneralMenu = new (
        [
            new SidebarMenuItem("/Tools/General/RollTheDice",      "Roll the Dice"),
            new SidebarMenuItem("/Tools/General/GoogleMaps",       "Google Maps"),
            new SidebarMenuItem("/Tools/General/ColorConverter",   "Color Converter"),
            new SidebarMenuItem("/Tools/General/HtmlEncode",       "Html Encode / Decode"),
            new SidebarMenuItem("/Tools/General/UrlEncode",        "Url Encode / Decode"),
            new SidebarMenuItem("/Tools/General/RandomBytes",      "Random Bytes"),
            new SidebarMenuItem("/Tools/General/WeekendCountdown", "Weekend Countdown"),
            new SidebarMenuItem("/Tools/General/Weather",          "Weather"),
            new SidebarMenuItem("/Tools/General/GpsConverter",     "GPS Converter")
        ],
        "General"
    );

    public static SidebarMenu CalculatorMenu = new (
        [
            new SidebarMenuItem("/Tools/Calculators/DateDifference", "Date Difference"),
            new SidebarMenuItem("/Tools/Calculators/Bandwidth",      "Bandwidth"),
            new SidebarMenuItem("/Tools/Calculators/FileSize",       "File Size"),
            new SidebarMenuItem("/Tools/Calculators/Time",           "Time"),
            new SidebarMenuItem("/Tools/Calculators/ByteCounter",    "Byte Counter")
        ],
        "Calculators"
    );

    public static SidebarMenu BinaryClockMenu = new (
        [
            new SidebarMenuItem("/Tools/BinaryClock/BinaryClock", "Binary Clock"),
            new SidebarMenuItem("/Tools/BinaryClock/About",       "About Binary Clock")
        ],
        "Binary Clock"
    );
}
