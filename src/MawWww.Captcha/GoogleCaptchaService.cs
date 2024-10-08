﻿using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MawWww.Captcha;

//https://www.google.com/recaptcha/admin#site/318682987?setup
public class GoogleCaptchaService
    : ICaptchaService
{
    static readonly Uri URL = new("https://www.google.com/recaptcha/api/siteverify");
    readonly GoogleCaptchaConfig _config;
    readonly ILogger _log;

    public GoogleCaptchaService(
        IOptions<GoogleCaptchaConfig> config,
        ILogger<GoogleCaptchaService> log)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(log);

        _config = config.Value;
        _log = log;
    }

    public virtual string ResponseFormFieldName => "g-recaptcha-response";
    public virtual string SiteKey => _config.SiteKey;

    public virtual async Task<bool> VerifyAsync(string recaptchaResponse)
    {
        if (string.IsNullOrEmpty(recaptchaResponse))
        {
            return false;
        }

        var parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("secret", _config.SecretKey),
                new KeyValuePair<string, string>("response", recaptchaResponse)
            };

        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        var response = await client.PostAsync(URL, content);
        var val = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GoogleCaptchaResponse>(val)?.success ?? false;

        _log.LogDebug("google recaptcha returned: {CaptchaResult}", result);

        response.Dispose();

        return result;
    }
}
