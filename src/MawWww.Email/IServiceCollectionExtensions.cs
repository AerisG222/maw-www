using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MawWww.Email;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration gmailApiConfiguration
    ) {
        services
            .Configure<GmailApiEmailConfig>(gmailApiConfiguration)
            .AddScoped<IEmailService, GmailApiEmailService>();

        return services;
    }
}
