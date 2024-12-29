using Microsoft.AspNetCore.Authorization;
using MawWww.Models;

namespace MawWww.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddMaWAuthorizationPolicies(
        this IServiceCollection services
    ) {
        var adminPolicy = new AuthorizationPolicyBuilder()
            .RequireRole("Administrator")
            .Build();

        services
            .AddAuthorization(opts => {
                opts.AddPolicy(AuthorizationPolicies.Administrator, adminPolicy);
            });

        return services;
    }
}
