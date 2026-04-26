using Microsoft.AspNetCore.Authorization;
using MawWww.Models;

namespace MawWww.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorizationPolicies(
        this IServiceCollection services
    )
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.Administrator, policy =>
                policy.RequireRole("Administrator")
            );

        return services;
    }
}
