using Microsoft.FeatureManagement;
using MawWww.Blog;
using MawWww.Captcha;
using MawWww.Email;
using www.Models;
using Fluid;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration
    .AddEnvironmentVariables("MAW_WWW_");

builder.Services
    .Configure<ContactConfig>(builder.Configuration.GetSection("ContactUs"))
    .AddFeatureManagement()
        .Services
    .AddBlogServices()
    .AddCaptchaFeature(
        builder.Configuration.GetSection("CloudflareTurnstile"),
        builder.Configuration.GetSection("GoogleRecaptcha")
    )
    .AddEmailServices(builder.Configuration.GetSection("Gmail"))
    .AddSingleton<FluidParser>()
    .AddRazorPages()
        .Services
    .AddRouting(opts => opts.LowercaseUrls = true);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
