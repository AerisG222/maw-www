using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class DateFormatsPageModel
    : MawPageModel
{
    // https://www.youtube.com/watch?v=cwZb2mqId0A
    public static readonly DateTime DATE = new(1969, 7, 20, 20, 17, 52, 789, DateTimeKind.Utc);

    public static readonly IEnumerable<FormatExampleGroup> Examples = [
        new(
            "Standard Formatting Methods",
            [
                new("date.ToLongDateString()", DATE.ToLongDateString()),
                new("date.ToLongTimeString()", DATE.ToLongTimeString()),
                new("date.ToShortDateString()", DATE.ToShortDateString()),
                new("date.ToShortTimeString()", DATE.ToShortTimeString()),
                new("date.ToString()", DATE.ToString(CultureInfo.InvariantCulture))
            ]
        ),
        new FormatExampleGroup(
            "Predefined Formats",
            [
                new("d", DATE.ToString("d", CultureInfo.InvariantCulture)),
                new("D", DATE.ToString("D", CultureInfo.InvariantCulture)),
                new("f", DATE.ToString("f", CultureInfo.InvariantCulture)),
                new("F", DATE.ToString("F", CultureInfo.InvariantCulture)),
                new("g", DATE.ToString("g", CultureInfo.InvariantCulture)),
                new("G", DATE.ToString("G", CultureInfo.InvariantCulture)),
                new("m -or- M", DATE.ToString("M", CultureInfo.InvariantCulture)),
                new("o -or- O", DATE.ToString("O", CultureInfo.InvariantCulture)),
                new("r -or- R", DATE.ToString("r", CultureInfo.InvariantCulture)),
                new("s", DATE.ToString("s", CultureInfo.InvariantCulture)),
                new("t", DATE.ToString("t", CultureInfo.InvariantCulture)),
                new("T", DATE.ToString("T", CultureInfo.InvariantCulture)),
                new("u", DATE.ToString("u", CultureInfo.InvariantCulture)),
                new("U", DATE.ToString("U", CultureInfo.InvariantCulture)),
                new("y -or- Y", DATE.ToString("y", CultureInfo.InvariantCulture))
            ]
        ),
        new FormatExampleGroup(
            "Custom Format Strings",
            [
                new("d -or- %d", DATE.ToString("%d", CultureInfo.InvariantCulture)),
                new("dd", DATE.ToString("dd", CultureInfo.InvariantCulture)),
                new("ddd", DATE.ToString("ddd", CultureInfo.InvariantCulture)),
                new("dddd", DATE.ToString("dddd", CultureInfo.InvariantCulture)),
                new("M -or- %M", DATE.ToString("%M", CultureInfo.InvariantCulture)),
                new("MM", DATE.ToString("MM", CultureInfo.InvariantCulture)),
                new("MMM", DATE.ToString("MMM", CultureInfo.InvariantCulture)),
                new("MMMM", DATE.ToString("MMMM", CultureInfo.InvariantCulture)),
                new("y -or- %y", DATE.ToString("%y", CultureInfo.InvariantCulture)),
                new("yy", DATE.ToString("yy", CultureInfo.InvariantCulture)),
                new("yyy", DATE.ToString("yyy", CultureInfo.InvariantCulture)),
                new("yyyy", DATE.ToString("yyyy", CultureInfo.InvariantCulture)),
                new("yyyyy", DATE.ToString("yyyyy", CultureInfo.InvariantCulture)),
                new("gg", DATE.ToString("gg", CultureInfo.InvariantCulture)),
                new("h -or- %h", DATE.ToString("%h", CultureInfo.InvariantCulture)),
                new("hh", DATE.ToString("hh", CultureInfo.InvariantCulture)),
                new("H -or- %H", DATE.ToString("%H", CultureInfo.InvariantCulture)),
                new("HH", DATE.ToString("HH", CultureInfo.InvariantCulture)),
                new("m -or- %m", DATE.ToString("%m", CultureInfo.InvariantCulture)),
                new("mm", DATE.ToString("mm", CultureInfo.InvariantCulture)),
                new("s -or- %s", DATE.ToString("%s", CultureInfo.InvariantCulture)),
                new("ss", DATE.ToString("ss", CultureInfo.InvariantCulture)),
                new("f -or- %f", DATE.ToString("%f", CultureInfo.InvariantCulture)),
                new("ff", DATE.ToString("ff", CultureInfo.InvariantCulture)),
                new("fff", DATE.ToString("fff", CultureInfo.InvariantCulture)),
                new("ffff", DATE.ToString("ffff", CultureInfo.InvariantCulture)),
                new("fffff", DATE.ToString("fffff", CultureInfo.InvariantCulture)),
                new("ffffff", DATE.ToString("ffffff", CultureInfo.InvariantCulture)),
                new("fffffff", DATE.ToString("fffffff", CultureInfo.InvariantCulture)),
                new("F -or- %F", DATE.ToString("%F", CultureInfo.InvariantCulture)),
                new("FF", DATE.ToString("FF", CultureInfo.InvariantCulture)),
                new("FFF", DATE.ToString("FFF", CultureInfo.InvariantCulture)),
                new("FFFF", DATE.ToString("FFFF", CultureInfo.InvariantCulture)),
                new("FFFFF", DATE.ToString("FFFFF", CultureInfo.InvariantCulture)),
                new("FFFFFF", DATE.ToString("FFFFFF", CultureInfo.InvariantCulture)),
                new("FFFFFFF", DATE.ToString("FFFFFFF", CultureInfo.InvariantCulture)),
                new("t -or- %t", DATE.ToString("%t", CultureInfo.InvariantCulture)),
                new("tt", DATE.ToString("tt", CultureInfo.InvariantCulture)),
                new("z -or- %z", DATE.ToString("%z", CultureInfo.InvariantCulture)),
                new("zz", DATE.ToString("zz", CultureInfo.InvariantCulture)),
                new("zzz", DATE.ToString("zzz", CultureInfo.InvariantCulture))
            ]
        )
    ];

    public IActionResult OnGet()
    {
        return Page();
    }
}
