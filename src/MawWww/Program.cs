using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.FeatureManagement;
using Fluid;
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
    .AddBlogServices()
    .AddCaptchaFeature(
        builder.Configuration.GetSection("CloudflareTurnstile"),
        builder.Configuration.GetSection("GoogleRecaptcha")
    )
    .AddEmailServices(builder.Configuration.GetSection("Gmail"))
    .AddSingleton<FluidParser>()
    .AddAuth0Authentication(builder.Configuration)
    .AddRazorPages(options =>
        {
            options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            options.Conventions.AuthorizePage("/Account/Logout");
            options.Conventions.AuthorizeFolder("/Admin");
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
    app
        .UseExceptionHandler("/Error")
        .UseHsts();
}

app
    .UseDefaultSecurityHeaders()
    .UseForwardedHeaders()
    .UseHttpsRedirection()
    .UseCustomStaticFiles()
    .UseCookiePolicy()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints => {
        endpoints.MapRazorPages();
    });

app.Run();
