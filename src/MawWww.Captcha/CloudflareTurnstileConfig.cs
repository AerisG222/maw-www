namespace MawWww.Captcha;

public sealed class CloudflareTurnstileConfig
{
    public string SiteKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}
