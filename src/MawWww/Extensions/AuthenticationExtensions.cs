using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MawWww.Extensions;

// https://auth0.com/blog/securing-razor-pages-applications-with-auth0/
public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuth0Authentication(
        this IServiceCollection services,
        IConfiguration config
    ) {
        services
            .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opts.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(opts =>
                {
                    opts.LoginPath = "/account/login";
                    opts.LogoutPath = "/account/logout";
                })
            .Services
            .AddAuth0WebAppAuthentication(options => {
                options.Domain = config["Auth0:Domain"];
                options.ClientId = config["Auth0:ClientId"];
                options.ClientSecret = config["Auth0:ClientSecret"];
                options.Scope = "openid profile email";
                options.CallbackPath = "/signin-oidc";

                // https://github.com/auth0/auth0-aspnetcore-authentication/issues/54
                options.SkipCookieMiddleware = true;
            });

        return services;
    }
}
