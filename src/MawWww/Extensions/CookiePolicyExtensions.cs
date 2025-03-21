using Microsoft.AspNetCore.CookiePolicy;

namespace MawWww.Extensions;

// https://github.com/auth0-samples/auth0-aspnetcore-mvc-samples/blob/master/Quickstart/Sample/Support/SameSiteServiceCollectionExtensions.cs
public static class CookiePolicyExtensions
{
    public static IServiceCollection ConfigureSameSiteNoneCookies(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.HttpOnly = HttpOnlyPolicy.Always;
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
        });

        return services;
    }

    private static void CheckSameSite(CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None && options.Secure == false)
        {
            options.SameSite = SameSiteMode.Unspecified;
        }
    }
}
