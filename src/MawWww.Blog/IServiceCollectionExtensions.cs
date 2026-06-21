using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace MawWww.Blog;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBlogServices(
        this IServiceCollection services
    )
    {
        SqlMapper.AddTypeHandler(InstantHandler.Default);

        services
            .AddScoped<IBlogRepository, BlogRepository>()
            .AddScoped<IBlogService, BlogService>();

        return services;
    }
}
