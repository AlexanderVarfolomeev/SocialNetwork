using Microsoft.AspNetCore.Identity;
using SocialNetwork.Context;
using SocialNetwork.Entities.User;
using SocialNetwork.IdentityServer.Configuration.IdentitySettings;

namespace SocialNetwork.IdentityServer.Configuration;

public static class IdentityConfiguration
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<AppUser>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders();


        services.AddIdentityServer(options => { options.EmitStaticAudienceClaim = true; })
            .AddAspNetIdentity<AppUser>()
            //.AddProfileService<ProfileService>()
            .AddInMemoryApiScopes(AppApiScopes.Scopes)
            .AddInMemoryIdentityResources(AppIdentityResources.Resources)
            .AddInMemoryClients(AppClients.Clients)
            .AddDeveloperSigningCredential();

        return services;
    }

    public static WebApplication UseAppIdentity(this WebApplication app)
    {
        app.UseIdentityServer();
        return app;
    }
}