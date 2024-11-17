using MawWww.Models;

namespace MawWww.Pages.Tools;

public static class ToolsMenu
{
    public static SidebarMenu DotnetMenu = new (
        [
            new SidebarMenuItem("/Tools/Dotnet/RegularExpressions", "Regular Expressions"),
            new SidebarMenuItem("/Tools/Dotnet/DateFormats",        "Date Formats"),
            new SidebarMenuItem("/Tools/Dotnet/NumberFormats",      "Number Formats"),
            new SidebarMenuItem("/Tools/Dotnet/HtmlEncodeDecode",   "Html Encode / Decode"),
            new SidebarMenuItem("/Tools/Dotnet/Paths",              "Paths"),
            new SidebarMenuItem("/Tools/Dotnet/RandomBytes",        "Random Bytes"),
            new SidebarMenuItem("/Tools/Dotnet/UrlEncodeDecode",    "Url Encode / Decode"),
            new SidebarMenuItem("/Tools/Dotnet/XmlValidator",       "Xml Validator"),
            new SidebarMenuItem("/Tools/Dotnet/XsdValidator",       "Xsd Validator"),
            new SidebarMenuItem("/Tools/Dotnet/XslTransform",       "Xsl Transform")
        ],
        ".NET"
    );

    public static SidebarMenu GeneralMenu = new (
        [
            new SidebarMenuItem("/Tools/General/RollTheDice",      "Roll the Dice"),
            new SidebarMenuItem("/Tools/General/GoogleMaps",       "Google Maps"),
            new SidebarMenuItem("/Tools/General/ColorConverter",   "Color Converter"),
            new SidebarMenuItem("/Tools/General/WeekendCountdown", "Weekend Countdown"),
            new SidebarMenuItem("/Tools/General/Weather",          "Weather"),
            new SidebarMenuItem("/Tools/General/GpsConverter",     "GPS Converter"),
            new SidebarMenuItem("/Tools/General/GuidGenerator",    "GUID Generator"),
        ],
        "General"
    );

    public static SidebarMenu CalculatorMenu = new (
        [
            new SidebarMenuItem("/Tools/Calculators/DateDifference", "Date Difference"),
            new SidebarMenuItem("/Tools/Calculators/Bandwidth",      "Bandwidth"),
            new SidebarMenuItem("/Tools/Calculators/FileSize",       "File Size"),
            new SidebarMenuItem("/Tools/Calculators/Time",           "Time"),
            new SidebarMenuItem("/Tools/Calculators/ByteCounter",    "Byte Counter"),
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
