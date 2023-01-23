using SocialNetwork.WebAPI.Middlewares;

namespace SocialNetwork.WebAPI.Configuration;

public static class AppMiddlewaresConfiguration
{
    public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
        return app;
    }
}