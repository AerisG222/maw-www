using Microsoft.AspNetCore.HttpOverrides;

namespace MawWww.Extensions;

public static class ForwardedHeadersExtensions
{
    public static IServiceCollection ConfigureForwardedHeaders(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .Configure<ForwardedHeadersOptions>(opts => {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                var knownNetworks = configuration.GetSection("ForwardedHeaders:KnownNetworks").Get<string[]>();

                if (knownNetworks == null || knownNetworks.Length == 0)
                {
                    Console.WriteLine("No KnownNetworks found when configuring ForwardedHeaders!");
                }
                else
                {
                    foreach (var network in knownNetworks)
                    {
                        opts.KnownIPNetworks.Add(System.Net.IPNetwork.Parse(network));
                    }
                }
            });

        return services;
    }
}
