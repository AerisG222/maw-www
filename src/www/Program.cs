using MawWww.Blog;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddBlogServices()
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
