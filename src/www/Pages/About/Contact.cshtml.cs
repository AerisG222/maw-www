using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using www.Models;

namespace www.Pages.About;

public class ContactModel
    : MawPageModel
{
    public bool SubmitAttempted { get; private set; }
    public bool SubmitSuccess { get; private set; }

    [BindProperty]
    public ContactForm? ContactForm { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await Task.CompletedTask;

        return Page();
    }
}

public class ContactForm
{
    [Required(ErrorMessage = "Please enter your name.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a message.")]
    public string Message { get; set; } = string.Empty;
}
