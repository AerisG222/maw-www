using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class RegularExpressionsPageModel
    : MawFormPageModel<RegularExpressionsForm>
{
    public MatchCollection? RegexMatches { get; private set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            RegexMatches = Form.RegEx!.Matches(Form.ExampleInput);
            SubmitSuccess = true;
        }

        return Page();
    }
}

public class RegularExpressionsForm
    : IValidatableObject
{
    [Required(ErrorMessage = "Please enter a pattern.")]
    public string RegExPattern { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter example input.")]
    public string ExampleInput { get; set; } = string.Empty;

    public bool OptionCultureInvariant { get; set; }
    public bool OptionEcmaScript { get; set; }
    public bool OptionExplicitCapture { get; set; }
    public bool OptionIgnoreCase { get; set; }
    public bool OptionIgnorePatternWhitespace { get; set; }
    public bool OptionMultiline { get; set; }
    public bool OptionNone { get; set; }
    public bool OptionRightToLeft { get; set; }
    public bool OptionSingleLine { get; set; }

    [BindNever]
    public Regex? RegEx { get; private set; } = null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        RegexOptions options = RegexOptions.None;

        if (OptionCultureInvariant)
        {
            options |= RegexOptions.CultureInvariant;
        }
        //ECMA Script can only be combined with Ignore Case, Multiline, and Culture Invariant
        if (OptionEcmaScript)
        {
            options |= RegexOptions.ECMAScript;
        }
        if (OptionExplicitCapture)
        {
            options |= RegexOptions.ExplicitCapture;
        }
        if (OptionIgnoreCase)
        {
            options |= RegexOptions.IgnoreCase;
        }
        if (OptionIgnorePatternWhitespace)
        {
            options |= RegexOptions.IgnorePatternWhitespace;
        }
        if (OptionMultiline)
        {
            options |= RegexOptions.Multiline;
        }
        if (OptionRightToLeft)
        {
            options |= RegexOptions.RightToLeft;
        }
        if (OptionSingleLine)
        {
            options |= RegexOptions.Singleline;
        }

        try
        {
            RegEx = new Regex(RegExPattern, options);
        }
        catch(RegexParseException ex)
        {
            results.Add(new ValidationResult($"Invalid regex: {ex.Error}", [nameof(RegExPattern)]));
        }
        catch(ArgumentOutOfRangeException)
        {
            var selectedOps = new List<string>();

            // if(OptionCultureInvariant)        { selectedOps.Add(nameof(OptionCultureInvariant));        }
            // if(OptionEcmaScript)              { selectedOps.Add(nameof(OptionEcmaScript));              }
            // if(OptionExplicitCapture)         { selectedOps.Add(nameof(OptionExplicitCapture));         }
            // if(OptionIgnoreCase)              { selectedOps.Add(nameof(OptionIgnoreCase));              }
            // if(OptionIgnorePatternWhitespace) { selectedOps.Add(nameof(OptionIgnorePatternWhitespace)); }
            // if(OptionMultiline)               { selectedOps.Add(nameof(OptionMultiline));               }
            // if(OptionNone)                    { selectedOps.Add(nameof(OptionNone));                    }
            // if(OptionRightToLeft)             { selectedOps.Add(nameof(OptionRightToLeft));             }
            // if(OptionSingleLine)              { selectedOps.Add(nameof(OptionSingleLine));              }

            selectedOps.Add(nameof(OptionNone));

            results.Add(new ValidationResult($"Incompatible options specified", selectedOps));
        }

        return results;
    }
}
