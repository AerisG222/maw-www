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
            //.AddScoped<IBlogRepository>(x => new BlogRepository(connString))
            .AddScoped<IBlogService, DummyBlogService>();

        return services;
    }
}
