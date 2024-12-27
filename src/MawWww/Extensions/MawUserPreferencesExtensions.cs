using MawWww.Middleware;

namespace MawWww.Extensions;

public static class MawUserPreferencesExtensions
{
    public static IApplicationBuilder UseMawUserPreferences(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MawUserPreferencesMiddleware>();
    }
}
