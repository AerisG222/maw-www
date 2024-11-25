using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;
using System.Net;

namespace MawWww.Pages.Tools.General;

public class UrlEncodePageModel
    : MawFormPageModel<UrlEncodeForm>
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

public enum UrlEncodeMode
{
    None = 0,
    Encode,
    Decode
}

public class UrlEncodeForm
    : IValidatableObject
{
    public string? EncodedString { get; set; }
    public string? DecodedString { get; set; }
    public UrlEncodeMode Mode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errorList = new List<ValidationResult>();

        switch (Mode)
        {
            case UrlEncodeMode.Decode:
                if (string.IsNullOrEmpty(EncodedString))
                {
                    errorList.Add(new ValidationResult("Encoded string must be provided.", [nameof(EncodedString)]));
                }
                break;
            case UrlEncodeMode.Encode:
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
            case UrlEncodeMode.Encode:
                Encode();
                break;
            case UrlEncodeMode.Decode:
                Decode();
                break;
            default:
                throw new InvalidOperationException("Invalid mode specified!");
        };
    }

    public void Encode()
    {
        EncodedString = WebUtility.UrlEncode(DecodedString);
    }

    public void Decode()
    {
        DecodedString = WebUtility.UrlDecode(EncodedString);
    }
}
