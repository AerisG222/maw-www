using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MawWww.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddCustomAuthentication(
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
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    opts =>
                    {
                        opts.LoginPath = "/account/login";
                        opts.LogoutPath = "/account/logout";
                        opts.AccessDeniedPath = "/account/access-denied";
                    }
                )
                .AddOpenIdConnect(
                    OpenIdConnectDefaults.AuthenticationScheme,
                    opts =>
                    {
                        var domain = config["Auth0:Domain"];
                        var clientId = config["Auth0:ClientId"];
                        var clientSecret = config["Auth0:ClientSecret"];

                        ArgumentException.ThrowIfNullOrEmpty(domain);
                        ArgumentException.ThrowIfNullOrEmpty(clientId);
                        ArgumentException.ThrowIfNullOrEmpty(clientSecret);

                        opts.Authority = $"https://{domain}";
                        opts.ClientId = clientId;
                        opts.ClientSecret = clientSecret;
                        opts.CallbackPath = "/signin-oidc";
                        opts.SignedOutCallbackPath = "/signout-callback-oidc";

                        opts.ResponseType = OpenIdConnectResponseType.Code;
                        opts.UsePkce = true;
                        opts.SaveTokens = true;
                        opts.GetClaimsFromUserInfoEndpoint = true;
                        opts.MapInboundClaims = false;

                        opts.Scope.Add(OpenIdConnectScope.OpenIdProfile);
                        opts.Scope.Add(OpenIdConnectScope.Email);
                        opts.Scope.Add("roles");

                        opts.TokenValidationParameters.NameClaimType = "name";
                    }
                );

        return services;
    }
}
