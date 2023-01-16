using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialNetwork.Context;

namespace SocialNetwork.WebAPI.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseNpgsql("Server=localhost;Port=6000;Database=SocialNetwork;User Id=postgres;Password=pgpass;");
        });
        
        return services;
    }
    
}