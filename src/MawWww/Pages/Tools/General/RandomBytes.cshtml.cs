using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;
using System.Security.Cryptography;
using System.Text;

namespace MawWww.Pages.Tools.General;

public class RandomBytesModel
    : MawFormPageModel<RandomBytesForm>
{
    public string? RandomBytes { get; private set; }
    public string? RandomBytesBase64 { get; private set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            GenerateRandomness();
            SubmitSuccess = true;
        }

        return Page();
    }

    void GenerateRandomness()
    {
        var randomBytes = GenerateRandom(Form.Size);

        RandomBytes = ToHexString(randomBytes)[..Form.Size];
        RandomBytesBase64 = Convert.ToBase64String(randomBytes);
    }

    public static byte[] GenerateRandom(int size)
    {
        var randomBytes = new byte[size];

        using (var rand = RandomNumberGenerator.Create())
        {
            rand.GetBytes(randomBytes);
        }

        return randomBytes;
    }

    public static string ToHexString(byte[] byteArray)
    {
        ArgumentNullException.ThrowIfNull(byteArray);

        StringBuilder builder = new(byteArray.Length);

        foreach (byte b in byteArray)
        {
            builder.Append(FormattableString.Invariant($"{b:X}"));
        }

        return builder.ToString();
    }
}

public class RandomBytesForm
{
    [Required(ErrorMessage = "Please enter the number of bytes")]
    [Range(1, 8192)]
    public int Size { get; set; }
}
