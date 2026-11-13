using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.FeatureManagement;
using Fluid;
using NodaTime;
using ZiggyCreatures.Caching.Fusion;
using MawWww;
using MawWww.Blog;
using MawWww.Captcha;
using MawWww.Email;
using MawWww.Extensions;
using MawWww.Models;
using MawWww.Pages.About;

var builder = WebApplication.CreateSlimBuilder(args);

// CreateSlimBuilder leaves out the static web assets loader that CreateBuilder turns on in
// development.  without it a dotnet run cannot see the compressed (.gz) copies the build keeps
// under obj/, and MapStaticAssets answers every gzip-accepting request with an empty 200.  the
// published app is unaffected - publish puts the compressed files in wwwroot itself
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}

builder.WebHost
    .UseKestrelHttpsConfiguration();

builder.Configuration
    .AddEnvironmentVariables("MAW_WWW_");

builder.Services
    .ConfigureDataProtection(builder.Configuration)
    .ConfigureForwardedHeaders(builder.Configuration)
    .Configure<ContactConfig>(builder.Configuration.GetSection("ContactUs"))
    .Configure<BlogConfig>(builder.Configuration.GetSection("Blog"))
    .ConfigureSameSiteNoneCookies()
    .AddSystemd()
    .AddFeatureManagement()
        .Services
    .AddNpgsql(builder.Configuration)
    .AddFusionCache()
        .AsHybridCache()
        .Services
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomAuthorizationPolicies()
    .AddCustomRateLimiting()
    .AddBlogServices()
    .AddCaptchaFeature(
        builder.Configuration.GetSection("CloudflareTurnstile"),
        builder.Configuration.GetSection("GoogleRecaptcha")
    )
    .AddEmailServices(builder.Configuration.GetSection("Gmail"))
    .AddSingleton<IClock>(services => SystemClock.Instance)
    .AddSingleton<FluidParser>()
    .AddSingleton<ContactEmailTemplateProvider>()
    .AddRazorPages(options =>
        {
            options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            options.Conventions.AllowAnonymousToPage("/Account/Login");
            options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
            options.Conventions.AuthorizeFolder("/Account");
            options.Conventions.AuthorizeFolder("/Admin", AuthorizationPolicies.Administrator);
        })
        .Services
    .AddRouting();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app
    .UseStatusCodePagesWithReExecute("/error/{0}")
    .UseDefaultSecurityHeaders()
    .UseForwardedHeaders()
    .UseHttpsRedirection()
    .UseCookiePolicy()
    .UseMawUserPreferences()
    .UseRouting()
    // static files are served by MapStaticAssets below.  this stays as a fallback for anything in
    // wwwroot the build manifest does not know about - the middleware skips any request that
    // already matched an endpoint, so after UseRouting it only ever sees the leftovers
    .UseCustomStaticFiles()
    .UseRateLimiter()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapStaticAssets();
        endpoints.MapRazorPages()
            .WithStaticAssets();
    });

app.Run();
