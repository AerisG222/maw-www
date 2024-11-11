using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Calculators;

public class ByteCounterModel
    : MawFormPageModel<ByteCounterForm>
{
    public List<ByteCounterResult> Results { get; private set; } = [];

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
        if(string.IsNullOrEmpty(Form.Text))
        {
            return false;
        }

        Results.Add(new("Bytes",     "0",        Form.Text.Length));
        Results.Add(new("Kilobytes", "#,##0.000", Form.Text.Length / 1024d));
        Results.Add(new("Megabytes", "#,##0.000000", Form.Text.Length / (1024d * 1024d)));

        return true;
    }
}

public class ByteCounterForm
{
    [Required(ErrorMessage = "Please enter text.")]
    public string Text { get; set; } = string.Empty;
}


public record ByteCounterResult (
    string Unit,
    string Format,
    double Size
);
