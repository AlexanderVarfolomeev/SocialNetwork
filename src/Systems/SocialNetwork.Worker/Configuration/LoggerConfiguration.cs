using Serilog;

namespace SocialNetwork.Worker.Configuration;

public static class LoggerConfiguration
{
    public static void AddAppLogger(this WebApplicationBuilder builder)
    {
        var logger = new Serilog.LoggerConfiguration()
            .Enrich.WithCorrelationIdHeader()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(logger, true);
    }
}