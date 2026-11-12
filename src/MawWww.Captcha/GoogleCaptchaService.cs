using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MawWww.Captcha;

//https://www.google.com/recaptcha/admin#site/318682987?setup
public sealed class GoogleCaptchaService
    : ICaptchaService
{
    static readonly Uri URL = new("https://www.google.com/recaptcha/api/siteverify");
    readonly GoogleCaptchaConfig _config;
    readonly ILogger _log;
    readonly IHttpClientFactory _httpClientFactory;

    public GoogleCaptchaService(
        IOptions<GoogleCaptchaConfig> config,
        ILogger<GoogleCaptchaService> log,
        IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _config = config.Value;
        _log = log;
        _httpClientFactory = httpClientFactory;
    }

    public string ResponseFormFieldName => "g-recaptcha-response";
    public string SiteKey => _config.SiteKey;

    public async Task<bool> VerifyAsync(string recaptchaResponse, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(recaptchaResponse))
        {
            return false;
        }

        var parameters = new List<KeyValuePair<string, string>>()
            {
                new("secret", _config.SecretKey),
                new("response", recaptchaResponse)
            };

        var result = false;

        try
        {
            using var client = _httpClientFactory.CreateClient();
            using var content = new FormUrlEncodedContent(parameters);
            using var response = await client.PostAsync(URL, content, cancellationToken);
            var captchaResponse = await response.Content.ReadFromJsonAsync<GoogleCaptchaResponse>(cancellationToken);
            result = captchaResponse?.Success ?? false;

            _log.LogDebug("google recaptcha returned: {CaptchaResult}", result);
        }
        catch(Exception ex)
        {
            _log.LogError(ex, "Error validating Google reCAPTCHA response");
        }

        return result;
    }
}
