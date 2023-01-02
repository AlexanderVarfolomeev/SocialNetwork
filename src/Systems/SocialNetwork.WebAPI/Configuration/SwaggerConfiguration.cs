using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SocialNetwork.WebAPI.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                opts.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Version = description.GroupName,
                    Title = "Twitter api"
                });

            opts.ResolveConflictingActions(apiDesc => apiDesc.First());

                var xmlFile = "api.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
              opts.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);
            ;

            options.DocExpansion(DocExpansion.List);
            options.DefaultModelsExpandDepth(-1);
        });

        return app; 
    }
}