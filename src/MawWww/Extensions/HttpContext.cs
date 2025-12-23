using MawWww.Middleware;
using MawWww.Models;

namespace MawWww.Extensions;

public static class HttpContextExtensions
{
    public static MawUserPreferences? GetMawUserPreferences(this HttpContext context)
    {
        return context.Items[MawUserPreferencesMiddleware.KEY_HTTP_CONTEXT_ITEMS_USER_PREFERENCES] as MawUserPreferences;
    }
}
