using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class NumberFormatsPageModel
    : MawPageModel
{
    public static readonly double NUMBER = 1234.125678;

    public static readonly IEnumerable<FormatExampleGroup> Examples = [
        new(
            "Standard Formatting Methods",
            [
                new("value.ToString()", NUMBER.ToString(CultureInfo.InvariantCulture))
            ]
        ),
        new FormatExampleGroup(
            "Predefined Formats",
            [
                new("c -or- C", NUMBER.ToString("c", CultureInfo.InvariantCulture)),
                new("d -or- D (int only)", ((int)NUMBER).ToString("d", CultureInfo.InvariantCulture)),
                new("d2 -or- D2 (int only)", ((int)NUMBER).ToString("d2", CultureInfo.InvariantCulture)),
                new("d6 -or- D6 (int only)", ((int)NUMBER).ToString("d6", CultureInfo.InvariantCulture)),
                new("e", NUMBER.ToString("e", CultureInfo.InvariantCulture)),
                new("e2", NUMBER.ToString("e2", CultureInfo.InvariantCulture)),
                new("e6", NUMBER.ToString("e6", CultureInfo.InvariantCulture)),
                new("E", NUMBER.ToString("E", CultureInfo.InvariantCulture)),
                new("E2", NUMBER.ToString("E2", CultureInfo.InvariantCulture)),
                new("E6", NUMBER.ToString("E6", CultureInfo.InvariantCulture)),
                new("f -or- F", NUMBER.ToString("f", CultureInfo.InvariantCulture)),
                new("f2 -or- F2", NUMBER.ToString("f2", CultureInfo.InvariantCulture)),
                new("f6 -or- F6", NUMBER.ToString("f6", CultureInfo.InvariantCulture)),
                new("g -or- G", NUMBER.ToString("g", CultureInfo.InvariantCulture)),
                new("n -or- N", NUMBER.ToString("n", CultureInfo.InvariantCulture)),
                new("n2 -or- N2", NUMBER.ToString("n2", CultureInfo.InvariantCulture)),
                new("n6 -or- N6", NUMBER.ToString("n6", CultureInfo.InvariantCulture)),
                new("p -or- P", NUMBER.ToString("p", CultureInfo.InvariantCulture)),
                new("p2 -or- P2", NUMBER.ToString("p2", CultureInfo.InvariantCulture)),
                new("p6 -or- P6", NUMBER.ToString("p6", CultureInfo.InvariantCulture)),
                new("r -or- R", NUMBER.ToString("R", CultureInfo.InvariantCulture)),
                new("x (int only)", ((int)NUMBER).ToString("x", CultureInfo.InvariantCulture)),
                new("x2 (int only)", ((int)NUMBER).ToString("x2", CultureInfo.InvariantCulture)),
                new("x6 (int only)", ((int)NUMBER).ToString("x6", CultureInfo.InvariantCulture)),
                new("X (int only)", ((int)NUMBER).ToString("X", CultureInfo.InvariantCulture)),
                new("X2 (int only)", ((int)NUMBER).ToString("X2", CultureInfo.InvariantCulture)),
                new("X6 (int only)", ((int)NUMBER).ToString("X6", CultureInfo.InvariantCulture))
            ]
        ),
        new FormatExampleGroup(
            "Custom Format Strings",
            [
                new("0.00", NUMBER.ToString("0.00", CultureInfo.InvariantCulture)),
                new("#.00", NUMBER.ToString("#.00", CultureInfo.InvariantCulture)),
                new("#,###.##", NUMBER.ToString("#,###.##", CultureInfo.InvariantCulture)),
                new("0,000,000.00", NUMBER.ToString("0,000,000.00", CultureInfo.InvariantCulture)),
                new("#,###,###.00", NUMBER.ToString("#,###,###.00", CultureInfo.InvariantCulture)),
                new("(#,###.##)", NUMBER.ToString("(#,###.##)", CultureInfo.InvariantCulture)),
                new("0.00%", NUMBER.ToString("0.00%", CultureInfo.InvariantCulture)),
                new("#.##;(#.##);--", NUMBER.ToString("#.##;(#.##);--", CultureInfo.InvariantCulture))
            ]
        )
    ];

    public IActionResult OnGet()
    {
        return Page();
    }
}
