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
    .AddRazorPages(options => {
        options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
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
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app
    .UseDefaultSecurityHeaders()
    .UseForwardedHeaders()
    .UseHttpsRedirection()
    .UseCustomStaticFiles()
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints => {
        endpoints.MapRazorPages();
    });

app.Run();
