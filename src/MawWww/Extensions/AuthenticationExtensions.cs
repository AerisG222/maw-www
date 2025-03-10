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
                    opts.AccessDeniedPath = "/account/access-denied";
                })
            .Services
            .AddAuth0WebAppAuthentication(options => {
                var domain = config["Auth0:Domain"];
                var clientId = config["Auth0:ClientId"];
                var clientSecret = config["Auth0:ClientSecret"];

                ArgumentNullException.ThrowIfNull(domain, nameof(domain));
                ArgumentNullException.ThrowIfNull(clientId, nameof(clientId));
                ArgumentNullException.ThrowIfNull(clientSecret, nameof(clientSecret));

                options.Domain = domain;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.Scope = "openid profile email";
                options.CallbackPath = "/signin-oidc";

                // https://github.com/auth0/auth0-aspnetcore-authentication/issues/54
                options.SkipCookieMiddleware = true;
            });

        return services;
    }
}
