using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MawWww.Captcha;

// https://developers.cloudflare.com/turnstile/get-started/server-side-validation/
public class CloudflareTurnstileCaptchaService
    : ICaptchaService
{
    static readonly Uri URL = new("https://challenges.cloudflare.com/turnstile/v0/siteverify");
    readonly CloudflareTurnstileConfig _config;
    readonly ILogger _log;
    readonly IHttpClientFactory _httpClientFactory;

    public CloudflareTurnstileCaptchaService(
        IOptions<CloudflareTurnstileConfig> config,
        ILogger<CloudflareTurnstileCaptchaService> log,
        IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _config = config.Value;
        _log = log;
        _httpClientFactory = httpClientFactory;
    }

    public virtual string ResponseFormFieldName => "cf-turnstile-response";
    public virtual string SiteKey => _config.SiteKey;

    public virtual async Task<bool> VerifyAsync(string recaptchaResponse, CancellationToken cancellationToken = default)
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
            var val = await response.Content.ReadAsStringAsync(cancellationToken);
            result = JsonSerializer.Deserialize<CloudflareTurnstileResponse>(val)?.Success ?? false;

            _log.LogDebug("Cloudflare Turnstile returned: {CaptchaResult}", result);
        }
        catch(Exception ex)
        {
            _log.LogError(ex, "Error validating Cloudflare Turnstile response");
        }

        return result;
    }
}
