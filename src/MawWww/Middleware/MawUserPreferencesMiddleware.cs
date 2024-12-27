using System.Text.Json;
using MawWww.Models;

namespace MawWww.Middleware;

public class MawUserPreferencesMiddleware
{
    public const string KEY_HTTP_CONTEXT_ITEMS_USER_PREFERENCES = "maw-preferences";
    static readonly string[] _validThemes = [ "light", "dark" ];

    readonly RequestDelegate _next;

    public MawUserPreferencesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LoadPreferences(context);

        await _next(context);
    }

    static void LoadPreferences(HttpContext context)
    {
        var cookie = context.Request.Cookies["maw-preferences"];

        if(cookie == null)
        {
            return;
        }

        try
        {
            var preferences = JsonSerializer.Deserialize<MawUserPreferences>(
                cookie,
                JsonSerializerOptions.Web
            );

            if(ValidatePreferences(preferences))
            {
                context.Items[KEY_HTTP_CONTEXT_ITEMS_USER_PREFERENCES] = preferences;
            }
        }
        catch
        {
            // ignore invalid cookies
            return;
        }
    }

    static bool ValidatePreferences(MawUserPreferences? preferences)
    {
        if(preferences == null)
        {
            return false;
        }

        if(!_validThemes.Contains(preferences.Theme))
        {
            return false;
        }

        return true;
    }
}
