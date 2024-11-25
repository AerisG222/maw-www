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
    readonly FluidParser _fluidParser;
    readonly object _contactUsTemplateLock = new();
    ICaptchaService? _captchaService;
    IFluidTemplate? _contactUsTemplate;

    static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "EmailTemplates");

    public bool IsHuman { get; private set; }
    public string CaptchaSiteKey => _captchaService?.SiteKey ?? string.Empty;

    public ContactPageModel(
        ILogger<ContactPageModel> log,
        IOptions<ContactConfig> contactOpts,
        ICaptchaFeature captchaFeature,
        IEmailService emailService,
        FluidParser fluidParser
    ) {
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(contactOpts);
        ArgumentNullException.ThrowIfNull(captchaFeature);
        ArgumentNullException.ThrowIfNull(emailService);
        ArgumentNullException.ThrowIfNull(fluidParser);

        _log = log;
        _config = contactOpts.Value;
        _captchaFeature = captchaFeature;
        _emailService = emailService;
        _fluidParser = fluidParser;
    }

    public async Task<IActionResult> OnGet()
    {
        await GetCaptchaService();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(IFormCollection collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        await GetCaptchaService();
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            var response = collection[_captchaService!.ResponseFormFieldName].FirstOrDefault();

            if(string.IsNullOrEmpty(response))
            {
                ModelState.AddModelError(nameof(IsHuman), "Please solve the Captcha");
            }
            else
            {
                IsHuman = await _captchaService.VerifyAsync(response);

                if (!IsHuman)
                {
                    ModelState.AddModelError(nameof(IsHuman), "The Captcha was not solved correctly, please try again");
                }
                else
                {
                    SubmitSuccess = await SendEmailAsync();
                }
            }
        }

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

    async Task<bool> SendEmailAsync()
    {
        try
        {
            var template = GetContactUsTemplate();

            if(template == null)
            {
                return false;
            }

            var options = new TemplateOptions()
            {
                FileProvider = new PhysicalFileProvider(TemplateDir)
            };

            var model = new {
                _config.Subject,
                Form.Email,
                Form.Name,
                Form.Message
            };

            var body = template.Render(new TemplateContext(model, options));

            await _emailService.SendHtmlAsync(
                _config.To,
                _config.To,  // from
                _config.Subject,
                body
            );

            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "There was an error sending an email: {ErrorMessage}", ex.Message);

            return false;
        }
    }

    IFluidTemplate GetContactUsTemplate()
    {
        if(_contactUsTemplate == null)
        {
            lock(_contactUsTemplateLock)
            {
                if(_contactUsTemplate == null)
                {
                    var templatePath = Path.Combine(TemplateDir, "ContactUs.liquid");
                    var templateSource = System.IO.File.ReadAllText(templatePath);

                    _contactUsTemplate = _fluidParser.Parse(templateSource);
                }
            }
        }

        return _contactUsTemplate;
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
