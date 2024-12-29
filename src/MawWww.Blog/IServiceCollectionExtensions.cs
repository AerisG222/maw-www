using Microsoft.Extensions.DependencyInjection;

namespace MawWww.Blog;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBlogServices (
        this IServiceCollection services
    ) {
        //DefaultTypeMap.MatchNamesWithUnderscores = true;
        //SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);

        services
            .AddScoped<IBlogRepository, BlogRepository>()
            .AddScoped<IBlogService, BlogService>();

        return services;
    }
}
