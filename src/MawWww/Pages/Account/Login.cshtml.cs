using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MawWww.Pages.Account;

public class LoginModel
    : PageModel
{
    public async Task OnGet(string returnUrl = "/")
    {
        var authenticationProperties = new AuthenticationProperties {
            RedirectUri = returnUrl,
            Items =
            {
                { "returnUrl", returnUrl },
                { "scheme", OpenIdConnectDefaults.AuthenticationScheme }
            }
        };

        await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            authenticationProperties
        );
    }
}
