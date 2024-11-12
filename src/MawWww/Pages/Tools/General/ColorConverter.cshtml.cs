using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.General;

public class ColorConverterModel
    : MawFormPageModel<ColorConverterForm>
{
    public string? HtmlColorCode => Form.GetHtmlColorCodeFromComponents();

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            SubmitSuccess = Form.RunConversion();
        }

        return Page();
    }
}

public class ColorConverterForm
    : IValidatableObject
{
    public const string METHOD_FROM_HEX = "FromHex";
    public const string METHOD_FROM_RGB = "FromRgb";

    public string ConversionMode { get; set; } = string.Empty;
    public string? HexCode { get; set; }
    public byte? Red { get; set; }
    public byte? Green { get; set; }
    public byte? Blue { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        switch (ConversionMode)
        {
            case METHOD_FROM_HEX:
                if (string.IsNullOrEmpty(HexCode))
                {
                    yield return new ValidationResult("Color code must be provided.", [nameof(HexCode)]);
                }
                else if (!ValidateHexCode())
                {
                    yield return new ValidationResult("Please enter the color code as #RRGGBB", [nameof(HexCode)]);
                }

                break;
            case METHOD_FROM_RGB:
                if (Red == null)
                {
                    yield return new ValidationResult("Red component must be provided.", [nameof(Red)]);
                }
                if (Green == null)
                {
                    yield return new ValidationResult("Green component must be provided.", [nameof(Green)]);
                }
                if (Blue == null)
                {
                    yield return new ValidationResult("Blue component must be provided.", [nameof(Blue)]);
                }
                break;
            default:
                throw new InvalidOperationException("A valid encoding mode was not specified.");
        }
    }

    public bool RunConversion()
    {
        switch(ConversionMode)
        {
            case METHOD_FROM_HEX:
                ConvertFromHex();
                break;
            case METHOD_FROM_RGB:
                ConvertFromRgb();
                break;
            default:
                return false;
        }

        return true;
    }

    public string? GetHtmlColorCodeFromComponents()
    {
        if(Red == null || Green == null || Blue == null)
        {
            return null;
        }

        return string.Concat("#", ToHex((byte)Red), ToHex((byte)Green), ToHex((byte)Blue));
    }

    bool ValidateHexCode()
    {
        var code = NormalizeHexCode();
        var max = int.Parse("FFFFFF", NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        if (int.TryParse(code, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out int val))
        {
            if (val < 0 || val > max)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    string NormalizeHexCode()
    {
        if(HexCode == null)
        {
            throw new InvalidOperationException($"{nameof(HexCode)} should not be null");
        }

        if (HexCode.StartsWith('#'))
        {
            return HexCode[1..];
        }

        return HexCode;
    }

    void ConvertFromHex()
    {
        var code = NormalizeHexCode();

        switch (code.Length)
        {
            case 3:
                Red = ToByte(string.Concat(code[0], code[0]));
                Green = ToByte(string.Concat(code[1], code[1]));
                Blue = ToByte(string.Concat(code[2], code[2]));
                break;
            case 6:
                Red = ToByte(string.Concat(code[0], code[1]));
                Green = ToByte(string.Concat(code[2], code[3]));
                Blue = ToByte(string.Concat(code[4], code[5]));
                break;
        }
    }

    void ConvertFromRgb()
    {
        HexCode = GetHtmlColorCodeFromComponents();
    }

    static byte ToByte(string hex)
    {
        return (byte)int.Parse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    static string ToHex(byte val)
    {
        return val.ToString("X2", CultureInfo.InvariantCulture).ToLowerInvariant();
    }
}
