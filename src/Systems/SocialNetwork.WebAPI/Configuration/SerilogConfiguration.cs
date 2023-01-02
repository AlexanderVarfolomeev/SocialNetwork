using Serilog;

namespace SocialNetwork.WebAPI.Configuration;

public static class SerilogConfiguration
{
    public static void AddAppSerilog(this WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.WithCorrelationId()
                .ReadFrom.Configuration(context.Configuration);
        });

        app.Services.AddHttpContextAccessor();
    }
    
    public static IApplicationBuilder UseAppSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}