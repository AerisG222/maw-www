using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Calculators;

public class TimeModel
    : MawFormPageModel<TimeForm>
{
    public List<Result> Results { get; private set; } = [];
    public IEnumerable<string> AllUnits = TimeScale.AllScales.Select(x => x.Name);

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            SubmitSuccess = CalculateResults();
        }

        return Page();
    }

    bool CalculateResults()
    {
        double timeInSeconds = 0;

        var timeScale = TimeScale.AllScales
            .SingleOrDefault(x => string.Equals(x.Name, Form.Unit, StringComparison.OrdinalIgnoreCase));

        if (timeScale == null)
        {
            return false;
        }

        timeInSeconds = Form.LengthOfTime * timeScale.SecondsInUnit;

        foreach (var scale in TimeScale.AllScales)
        {
            Results.Add(new Result(scale.Name, timeInSeconds / scale.SecondsInUnit));
        }

        return true;
    }
}

public class TimeForm
{
    [Required(ErrorMessage = "Please enter length of time.")]
    public double LengthOfTime { get; set; } = 0;

    [Required(ErrorMessage = "Please select unit of time.")]
    public string Unit { get; set; } = TimeScale.Seconds.Name;
}

public record TimeScale(
    string Name,
    double SecondsInUnit
) {
    public static readonly TimeScale Nanoseconds =  new("Nanoseconds",  1d / 1_000_000_000d);
    public static readonly TimeScale Microseconds = new("Microseconds", 1d / 1_000_000d);
    public static readonly TimeScale Milliseconds = new("Milliseconds", 1d / 1_000d);
    public static readonly TimeScale Seconds =      new("Seconds",      1);
    public static readonly TimeScale Minutes =      new("Minutes",      60);
    public static readonly TimeScale Hours =        new("Hours",        60 * 60);
    public static readonly TimeScale Days =         new("Days",         60 * 60 * 24);
    public static readonly TimeScale Weeks =        new("Weeks",        60 * 60 * 24 * 7);
    public static readonly TimeScale Months =       new("Months",       60 * 60 * 730);  // 730h ~ 30.4d
    public static readonly TimeScale Years =        new("Years",        60 * 60 * 24 * 365.242);
    public static readonly TimeScale Decades =      new("Decades",      60 * 60 * 24 * 365.242 * 10);
    public static readonly TimeScale Centuries =    new("Centuries",    60 * 60 * 24 * 365.242 * 100);
    public static readonly TimeScale Millenias =    new("Millenias",    60 * 60 * 24 * 365.242 * 1000);

    public static readonly TimeScale[] AllScales = [
        Nanoseconds,
        Microseconds,
        Milliseconds,
        Seconds,
        Minutes,
        Hours,
        Days,
        Weeks,
        Months,
        Years,
        Decades,
        Centuries,
        Millenias
    ];
}

public record Result (
    string TimeUnit,
    double LengthOfTime
);
