using Microsoft.Extensions.DependencyInjection;

namespace MawWww.Blog;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBlogServices (
        this IServiceCollection services
    ) {
        services
            .AddScoped<IBlogRepository, BlogRepository>()
            .AddScoped<IBlogService, BlogService>();

        return services;
    }
}
