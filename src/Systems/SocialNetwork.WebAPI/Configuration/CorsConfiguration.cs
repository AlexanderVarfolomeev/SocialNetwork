namespace SocialNetwork.WebAPI.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddCors(builder =>
        {
            builder.AddDefaultPolicy(pol =>
            {
                pol.AllowAnyHeader();
                pol.AllowAnyMethod();
                pol.AllowAnyOrigin();
                pol.WithOrigins("http://host.docker.internal:7000/connect/token");

            });
        });

        return services;
    }

    public static void UseAppCors(this IApplicationBuilder app)
    {
        app.UseCors();
    }
}