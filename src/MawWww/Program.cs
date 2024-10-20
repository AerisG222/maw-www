using Microsoft.FeatureManagement;
using MawWww.Blog;
using MawWww.Captcha;
using MawWww.Email;
using MawWww.Models;
using Fluid;
using Microsoft.AspNetCore.StaticFiles;

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
app.UseStaticFiles(new StaticFileOptions {
    ContentTypeProvider = GetCustomMimeTypeProvider()
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static FileExtensionContentTypeProvider GetCustomMimeTypeProvider()
{
    var provider = new FileExtensionContentTypeProvider();

    provider.Mappings[".gltf"] = "model/gltf+json";

    return provider;
}
