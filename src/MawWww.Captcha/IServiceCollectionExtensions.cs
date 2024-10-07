using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MawWww.Captcha;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaFeature(
        this IServiceCollection services,
        IConfiguration cloudflareTurnstileConfiguration,
        IConfiguration googleCaptchaConfiguration
    ) {
        services
            .Configure<CloudflareTurnstileConfig>(cloudflareTurnstileConfiguration)
            .Configure<GoogleCaptchaConfig>(googleCaptchaConfiguration)
            .AddScoped<ICaptchaService, GoogleCaptchaService>()
            .AddScoped<ICaptchaService, CloudflareTurnstileCaptchaService>()
            .AddScoped<ICaptchaFeature, CaptchaFeature>();

        return services;
    }
}
