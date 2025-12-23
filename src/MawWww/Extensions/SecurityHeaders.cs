namespace MawWww.Extensions;

public static class SecurityHeadersExtensions
{
    public static IApplicationBuilder UseDefaultSecurityHeaders(this IApplicationBuilder app)
    {
        app
            .UseSecurityHeaders(policies => {
                policies
                    .AddDefaultSecurityHeaders()
                    .AddContentSecurityPolicy(builder => {
                        builder.AddFrameAncestors().Self();
                    })
                    .AddCrossOriginResourcePolicy(opts =>
                    {
                        opts.CrossOrigin();
                    });
            });

        return app;
    }
}
