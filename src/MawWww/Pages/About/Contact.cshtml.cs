using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Fluid;
using MawWww.Captcha;
using MawWww.Email;
using MawWww.Models;

namespace MawWww.Pages.About;

public class ContactPageModel
    : MawFormPageModel<ContactForm>
{
    readonly ILogger _log;
    readonly ICaptchaFeature _captchaFeature;
    readonly ContactConfig _config;
    readonly IEmailService _emailService;
    readonly ContactEmailTemplateProvider _templateProvider;
    ICaptchaService? _captchaService;

    static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "EmailTemplates");

    public bool IsHuman { get; private set; }
    public string CaptchaSiteKey => _captchaService?.SiteKey ?? string.Empty;

    public ContactPageModel(
        ILogger<ContactPageModel> log,
        IOptions<ContactConfig> contactOpts,
        ICaptchaFeature captchaFeature,
        IEmailService emailService,
        ContactEmailTemplateProvider templateProvider
    )
    {
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(contactOpts);
        ArgumentNullException.ThrowIfNull(captchaFeature);
        ArgumentNullException.ThrowIfNull(emailService);
        ArgumentNullException.ThrowIfNull(templateProvider);

        _log = log;
        _config = contactOpts.Value;
        _captchaFeature = captchaFeature;
        _emailService = emailService;
        _templateProvider = templateProvider;
    }

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        await GetCaptchaService(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(IFormCollection collection, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(collection);
        await GetCaptchaService(cancellationToken);
        SubmitAttempted = true;

        if (ModelState.IsValid)
        {
            var response = collection[_captchaService!.ResponseFormFieldName].FirstOrDefault();

            if (string.IsNullOrEmpty(response))
            {
                ModelState.AddModelError(nameof(IsHuman), "Please solve the Captcha");
            }
            else
            {
                IsHuman = await _captchaService.VerifyAsync(response, cancellationToken);

                if (!IsHuman)
                {
                    ModelState.AddModelError(nameof(IsHuman), "The Captcha was not solved correctly, please try again");
                }
                else
                {
                    SubmitSuccess = await SendEmailAsync(cancellationToken);
                }
            }
        }

        return Page();
    }

    async Task GetCaptchaService(CancellationToken cancellationToken)
    {
        _captchaService = await _captchaFeature.GetServiceAsync(cancellationToken);

        if (_captchaService == null)
        {
            throw new ApplicationException("Captcha service should not be null!");
        }
    }

    async Task<bool> SendEmailAsync(CancellationToken cancellationToken)
    {
        try
        {
            var options = new TemplateOptions()
            {
                FileProvider = new PhysicalFileProvider(TemplateDir)
            };

            var model = new
            {
                _config.Subject,
                Form.Email,
                Form.Name,
                Form.Message
            };

            var body = _templateProvider.Template.Render(new TemplateContext(model, options));

            await _emailService.SendHtmlAsync(
                _config.To,
                _config.To,  // from
                _config.Subject,
                body,
                cancellationToken
            );

            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "There was an error sending an email: {ErrorMessage}", ex.Message);

            return false;
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
