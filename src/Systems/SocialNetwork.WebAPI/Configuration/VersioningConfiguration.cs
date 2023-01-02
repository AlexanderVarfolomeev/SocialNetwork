using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebAPI.Configuration;

public static class VersioningConfiguration
{
    public static IServiceCollection AddAppVersioning(this IServiceCollection services)
    {
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            }
        );


        return services;
    }
}