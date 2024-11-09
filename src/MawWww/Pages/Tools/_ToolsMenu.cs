using MawWww.Models;

namespace MawWww.Pages.Tools;

public static class ToolsMenu
{
    public static SidebarMenu DotnetMenu = new (
        [
            new SidebarMenuItem("/Tools/Dotnet/RegularExpressions", "Regular Expressions"),
            new SidebarMenuItem("/Tools/Dotnet/DateFormatting",     "Date Formatting"),
            new SidebarMenuItem("/Tools/Dotnet/NumberFormatting",   "Number Formatting"),
            new SidebarMenuItem("/Tools/Dotnet/ByteCounter",        "Byte Counter"),
            new SidebarMenuItem("/Tools/Dotnet/DateDifference",     "Date Difference"),
            new SidebarMenuItem("/Tools/Dotnet/ColorConverter",     "Color Converter"),
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
            new SidebarMenuItem("/Tools/General/RollTheDice",              "Roll the Dice"),
            new SidebarMenuItem("/Tools/General/GoogleMaps",               "Google Maps"),
            new SidebarMenuItem("/Tools/General/WeekendCountdown",         "Weekend Countdown"),
            new SidebarMenuItem("/Tools/General/Weather",                  "Weather"),
            new SidebarMenuItem("/Tools/General/GpsCoordinateConversions", "GPS Coordinate Conversions"),
            new SidebarMenuItem("/Tools/General/GuidGenerator",            "GUID Generator")
        ],
        "General"
    );

    public static SidebarMenu NetworkingMenu = new (
        [
            new SidebarMenuItem("/Tools/Networking/BandwidthCalculator", "Bandwidth Calculator"),
            new SidebarMenuItem("/Tools/Networking/FileSize",            "File Size"),
            new SidebarMenuItem("/Tools/Networking/Time",                "Time")
        ],
        "Networking"
    );

    public static SidebarMenu BinaryClockMenu = new (
        [
            new SidebarMenuItem("/Tools/BinaryClock/BinaryClock", "Binary Clock"),
            new SidebarMenuItem("/Tools/BinaryClock/About",       "About Binary Clock")
        ],
        "Binary Clock"
    );
}
