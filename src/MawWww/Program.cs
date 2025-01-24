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

var builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration
    .AddEnvironmentVariables("MAW_WWW_");

builder.Services
    .ConfigureDataProtection(builder.Configuration)
    .ConfigureForwardedHeaders()
    .Configure<ContactConfig>(builder.Configuration.GetSection("ContactUs"))
    .ConfigureSameSiteNoneCookies()
    .AddSystemd()
    .AddFeatureManagement()
        .Services
    .AddNpgsql(builder.Configuration)
    .AddBlogServices()
    .AddCaptchaFeature(
        builder.Configuration.GetSection("CloudflareTurnstile"),
        builder.Configuration.GetSection("GoogleRecaptcha")
    )
    .AddEmailServices(builder.Configuration.GetSection("Gmail"))
    .AddSingleton<IClock>(services => SystemClock.Instance)
    .AddSingleton<FluidParser>()
    .AddAuth0Authentication(builder.Configuration)
    .AddMaWAuthorizationPolicies()
    .AddFusionCache()
        .AsHybridCache()
        .Services
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

if(app.Environment.IsDevelopment())
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
    .UseCustomStaticFiles()
    .UseCookiePolicy()
    .UseMawUserPreferences()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints => {
        endpoints.MapRazorPages();
    });

app.Run();
