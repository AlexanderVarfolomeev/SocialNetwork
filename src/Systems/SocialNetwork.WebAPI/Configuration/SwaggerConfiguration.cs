using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using SocialNetwork.Constants.Security;
using SocialNetwork.Settings.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SocialNetwork.WebAPI.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services, IAppSettings apiSettings)
    {
         services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                opts.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Version = description.GroupName,
                    Title = "Social network api"
                });

            opts.ResolveConflictingActions(apiDesc => apiDesc.First());

            var xmlFile = "api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opts.IncludeXmlComments(xmlPath);

            opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(apiSettings.Identity.Url + "/connect/token"), 
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.NetworkRead, "Social network read data."},
                            {AppScopes.NetworkWrite, "Social network write data."}
                        }
                    },
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(apiSettings.Identity.Url + "/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.NetworkRead, "Social network read data."}
                        }
                    }
                }
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options => { });
        app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);

            options.DocExpansion(DocExpansion.List);
            options.DefaultModelsExpandDepth(-1);
            options.OAuthAppName("SocNet_api");

            options.OAuthClientId("frontend");
            options.OAuthClientSecret("secret");
        });
        return app;
    }
}