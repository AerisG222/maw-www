using System.ComponentModel.DataAnnotations;
using MawWww.Captcha;
using Microsoft.AspNetCore.Mvc;
using www.Models;

namespace www.Pages.About;

public class ContactModel
    : MawPageModel
{
    readonly ILogger _log;
    readonly ICaptchaFeature _captchaFeature;
    ICaptchaService? _captchaService;

    public bool SubmitAttempted { get; private set; }
    public bool SubmitSuccess { get; private set; }
    public bool IsHuman { get; private set; }
    public string CaptchaSiteKey => _captchaService?.SiteKey ?? string.Empty;


    [BindProperty]
    public ContactForm ContactForm { get; set; } = new();

    public ContactModel(
        ILogger<ContactModel> log,
        ICaptchaFeature captchaFeature
    ) {
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(captchaFeature);

        _log = log;
        _captchaFeature = captchaFeature;
    }

    public async Task<IActionResult> OnGet()
    {
        await GetCaptchaService();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await GetCaptchaService();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await Task.CompletedTask;

        return Page();
    }

    async Task GetCaptchaService()
    {
        _captchaService = await _captchaFeature.GetServiceAsync();

        if(_captchaService == null)
        {
            throw new ApplicationException("Captcha service should not be null!");
        }
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
