using Microsoft.AspNetCore.HttpOverrides;

namespace MawWww.Extensions;

public static class ForwardedHeadersExtensions
{
    public static IServiceCollection ConfigureForwardedHeaders(this IServiceCollection services)
    {
        services
            .Configure<ForwardedHeadersOptions>(opts => {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

        return services;
    }
}
