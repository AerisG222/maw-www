using System.Threading.RateLimiting;

namespace MawWww.Extensions;

public static class RateLimitingExtensions
{
    // applied to tools that execute visitor supplied code (regex, xslt)
    public const string ToolExecutionPolicy = "tool-execution";

    const int PermitsPerWindow = 10;
    static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(ToolExecutionPolicy, context =>
            {
                // only the post executes anything - viewing the form is not limited
                if (!HttpMethods.IsPost(context.Request.Method))
                {
                    return RateLimitPartition.GetNoLimiter("get");
                }

                // RemoteIpAddress reflects X-Forwarded-For, as UseForwardedHeaders runs earlier
                var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = PermitsPerWindow,
                    Window = Window,
                    QueueLimit = 0
                });
            });
        });

        return services;
    }
}
