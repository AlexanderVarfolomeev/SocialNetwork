using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Context.Setup;

public class DbInit
{
    public static void Execute(IServiceProvider service)
    {
        using var scope = service.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        using var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

        context.Database.Migrate();
    }
}