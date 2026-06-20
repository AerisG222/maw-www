using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;
using System.Security.Cryptography;

namespace MawWww.Pages.Tools.General;

public class RandomBytesPageModel
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

        RandomBytes = Utils.ToHexString(randomBytes);
        RandomBytesBase64 = Convert.ToBase64String(randomBytes);
    }

    static byte[] GenerateRandom(int size)
    {
        var randomBytes = new byte[size];
        RandomNumberGenerator.Fill(randomBytes);
        return randomBytes;
    }
}

public class RandomBytesForm
{
    [Required(ErrorMessage = "Please enter the number of bytes")]
    [Range(1, 8192)]
    public int Size { get; set; } = 100;
}
