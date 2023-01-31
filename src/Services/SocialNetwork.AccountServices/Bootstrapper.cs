using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.AccountServices.Implementations;
using SocialNetwork.AccountServices.Interfaces;

namespace SocialNetwork.AccountServices;

public static class Bootstrapper
{
    public static IServiceCollection AddAccountService(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAdminService, AdminService>();
        return services;
    }
}