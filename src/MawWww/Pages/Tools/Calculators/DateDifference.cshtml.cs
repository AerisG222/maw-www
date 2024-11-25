using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Calculators;

public class DateDifferencePageModel
    : MawFormPageModel<DateDifferenceForm>
{
    public List<DateDifference> Results { get; set; } = [];

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            var diff = Form.EndDate.ToDateTime(TimeOnly.MinValue) - Form.StartDate.ToDateTime(TimeOnly.MinValue);

            Results.AddRange([
                new DateDifference("Nanoseconds",  diff.TotalNanoseconds),
                new DateDifference("Microseconds", diff.TotalMicroseconds),
                new DateDifference("Milliseconds", diff.TotalMilliseconds),
                new DateDifference("Seconds",      diff.TotalSeconds),
                new DateDifference("Minutes",      diff.TotalMinutes),
                new DateDifference("Hours",        diff.TotalHours),
                new DateDifference("Days",         diff.TotalDays),
                new DateDifference("Weeks",        diff.TotalDays / 7d),
                new DateDifference("Years",        diff.TotalDays / 365.242d),
                new DateDifference("Decades",      diff.TotalDays / 365.242d / 10),
                new DateDifference("Centuries",    diff.TotalDays / 365.242d / 100),
                new DateDifference("Millenias",    diff.TotalDays / 365.242d / 1000)
            ]);

            SubmitSuccess = true;
        }

        return Page();
    }
}

public class DateDifferenceForm
    : IValidatableObject
{
    [Required(ErrorMessage = "Please enter start date.")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Please enter end date.")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(StartDate > EndDate)
        {
            yield return new ValidationResult("Start date must precede end date.", [nameof(StartDate)]);
        }
    }
}

public record DateDifference (
    string TimeScale,
    double Amount
);
