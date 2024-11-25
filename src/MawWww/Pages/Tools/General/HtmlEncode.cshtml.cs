using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;
using System.Net;

namespace MawWww.Pages.Tools.General;

public class HtmlEncodePageModel
    : MawFormPageModel<HtmlEncodeForm>
{
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            Form.RunConversion();
            SubmitSuccess = true;
        }

        return Page();
    }
}

public enum HtmlEncodeMode
{
    None = 0,
    Encode,
    Decode
}

public class HtmlEncodeForm
    : IValidatableObject
{
    public string? EncodedString { get; set; }
    public string? DecodedString { get; set; }
    public HtmlEncodeMode Mode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errorList = new List<ValidationResult>();

        switch (Mode)
        {
            case HtmlEncodeMode.Decode:
                if (string.IsNullOrEmpty(EncodedString))
                {
                    errorList.Add(new ValidationResult("Encoded string must be provided.", [nameof(EncodedString)]));
                }
                break;
            case HtmlEncodeMode.Encode:
                if (string.IsNullOrEmpty(DecodedString))
                {
                    errorList.Add(new ValidationResult("Decoded string must be provided.", [nameof(DecodedString)]));
                }
                break;
            default:
                throw new InvalidOperationException("A valid encoding mode was not specified.");
        }

        return errorList;
    }

    public void RunConversion()
    {
        switch(Mode)
        {
            case HtmlEncodeMode.Encode:
                Encode();
                break;
            case HtmlEncodeMode.Decode:
                Decode();
                break;
            default:
                throw new InvalidOperationException("Invalid mode specified!");
        };
    }

    public void Encode()
    {
        EncodedString = WebUtility.HtmlEncode(DecodedString);
    }

    public void Decode()
    {
        DecodedString = WebUtility.HtmlDecode(EncodedString);
    }
}
