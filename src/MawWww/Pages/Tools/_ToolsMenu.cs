using MawWww.Models;

namespace MawWww.Pages.Tools;

public static class ToolsMenu
{
    public static readonly Menu DotnetMenu = new (
        [
            new MenuItem("/Tools/Dotnet/RegularExpressions", "Regular Expressions"),
            new MenuItem("/Tools/Dotnet/DateFormats",        "Date Formats"),
            new MenuItem("/Tools/Dotnet/NumberFormats",      "Number Formats"),
            new MenuItem("/Tools/Dotnet/GuidGenerator",      "GUID Generator"),
            new MenuItem("/Tools/Dotnet/Paths",              "Paths"),
            new MenuItem("/Tools/Dotnet/XmlValidator",       "XML Validator"),
            new MenuItem("/Tools/Dotnet/XsdValidator",       "XSD Validator"),
            new MenuItem("/Tools/Dotnet/XslTransform",       "XSL Transform")
        ],
        ".NET"
    );

    public static readonly Menu GeneralMenu = new (
        [
            new MenuItem("/Tools/General/RollTheDice",      "Roll the Dice"),
            new MenuItem("/Tools/General/GoogleMaps",       "Google Maps"),
            new MenuItem("/Tools/General/ColorConverter",   "Color Converter"),
            new MenuItem("/Tools/General/HtmlEncode",       "Html Encode / Decode"),
            new MenuItem("/Tools/General/UrlEncode",        "Url Encode / Decode"),
            new MenuItem("/Tools/General/RandomBytes",      "Random Bytes"),
            new MenuItem("/Tools/General/WeekendCountdown", "Weekend Countdown"),
            new MenuItem("/Tools/General/Weather",          "Weather"),
            new MenuItem("/Tools/General/GpsConverter",     "GPS Converter")
        ],
        "General"
    );

    public static readonly Menu CalculatorMenu = new (
        [
            new MenuItem("/Tools/Calculators/DateDifference", "Date Difference"),
            new MenuItem("/Tools/Calculators/Bandwidth",      "Bandwidth"),
            new MenuItem("/Tools/Calculators/FileSize",       "File Size"),
            new MenuItem("/Tools/Calculators/Time",           "Time"),
            new MenuItem("/Tools/Calculators/ByteCounter",    "Byte Counter")
        ],
        "Calculators"
    );

    public static readonly Menu BinaryClockMenu = new (
        [
            new MenuItem("/Tools/BinaryClock/BinaryClock", "Binary Clock"),
            new MenuItem("/Tools/BinaryClock/About",       "About Binary Clock")
        ],
        "Binary Clock"
    );

    public static readonly IEnumerable<Menu> Menus = [
        GeneralMenu,
        CalculatorMenu,
        DotnetMenu,
        BinaryClockMenu
    ];
}
