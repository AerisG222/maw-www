using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Flurl;

namespace MawWww.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddKeycloakAuthentication(
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
                        var url = config["Keycloak:Url"];
                        var realm = config["Keycloak:Realm"];
                        var clientId = config["Keycloak:ClientId"];
                        var clientSecret = config["Keycloak:ClientSecret"];
                        var requireHttps = config.GetValue("Keycloak:RequireHttps", true);

                        ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));
                        ArgumentException.ThrowIfNullOrEmpty(realm, nameof(realm));
                        ArgumentException.ThrowIfNullOrEmpty(clientId, nameof(clientId));
                        ArgumentException.ThrowIfNullOrEmpty(clientSecret, nameof(clientSecret));

                        opts.Authority = Url.Combine(url, "realms", realm);
                        opts.ClientId = clientId;
                        opts.ClientSecret = clientSecret;
                        opts.RequireHttpsMetadata = requireHttps;
                        opts.CallbackPath = "/signin-oidc";
                        opts.SignedOutCallbackPath = "/signout-callback-oidc";
                        opts.MetadataAddress = Url.Combine(opts.Authority, ".well-known/openid-configuration");

                        opts.ResponseType = OpenIdConnectResponseType.Code;
                        opts.UsePkce = true;
                        opts.SaveTokens = true;
                        opts.GetClaimsFromUserInfoEndpoint = true;
                        opts.MapInboundClaims = false;

                        opts.Scope.Add(OpenIdConnectScope.OpenIdProfile);
                        opts.Scope.Add(OpenIdConnectScope.Email);
                        opts.Scope.Add("roles");

                        opts.TokenValidationParameters.NameClaimType = "preferred_username";
                        opts.TokenValidationParameters.RoleClaimType = "roles";
                    }
                );

        return services;
    }
}
