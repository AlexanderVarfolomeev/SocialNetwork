using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Common.Security;
using SocialNetwork.Context;
using SocialNetwork.Entities.User;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.WebAPI.Configuration;

public static class AuthConfiguration
{
     public static IServiceCollection AddAppAuth(this IServiceCollection services, IAppSettings apiSettings)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                // options.User.RequireUniqueEmail
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<AppUser>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddRoles<AppRole>()
            .AddDefaultTokenProviders();


        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = apiSettings.Identity.Url;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Audience = "api";
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppScopes.NetworkRead, policy => policy.RequireClaim("scope", AppScopes.NetworkRead));
            options.AddPolicy(AppScopes.NetworkWrite, policy => policy.RequireClaim("scope", AppScopes.NetworkWrite));
        });

        return services;
    }
     
     public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
     {
         app.UseAuthentication();
         app.UseAuthorization();
         return app;
     }
}