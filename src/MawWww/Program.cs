using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FeatureManagement;
using Fluid;
using MawWww;
using MawWww.Blog;
using MawWww.Captcha;
using MawWww.Email;
using MawWww.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration
    .AddEnvironmentVariables("MAW_WWW_");

builder.Services
    .Configure<ForwardedHeadersOptions>(opts => {
        opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    })
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
    .UseForwardedHeaders()
    .UseHttpsRedirection()
    .UseStaticFiles(new StaticFileOptions {
        ContentTypeProvider = GetCustomMimeTypeProvider()
    })
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints => {
        endpoints.MapRazorPages();
    });

app.Run();

static FileExtensionContentTypeProvider GetCustomMimeTypeProvider()
{
    var provider = new FileExtensionContentTypeProvider();

    provider.Mappings[".gltf"] = "model/gltf+json";

    return provider;
}
