using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auth0.AspNetCore.Authentication;

namespace MawWww.Pages.Account;

public class LogoutModel
    : PageModel
{
    public async Task OnGet(string returnUrl = "/")
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

        // logout from Auth0
        await HttpContext.SignOutAsync(
            Auth0Constants.AuthenticationScheme,
            authenticationProperties
        );

        // logout from the application
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}
