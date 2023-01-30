using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Context.Setup;

public static class DbSeed
{
    private const string AdminPassword = "pass";

    public static void Execute(IServiceProvider services)
    {
        var scope = services.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        Task.Run(async () => { await AddRolesAndAdmin(context, manager); });
    }

    private static async Task AddRolesAndAdmin(MainDbContext context, UserManager<AppUser> manager)
    {
        if (!context.Roles.Any(x => x.Permissions == Permissions.Admin)
            || !context.Roles.Any(x => x.Permissions == Permissions.User)
            || !context.Roles.Any(x => x.Permissions == Permissions.GodAdmin))
        {
            context.Roles.Add(new AppRole()
            {
                Name = "User",
                Permissions = Permissions.User,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            context.Roles.Add(new AppRole()
            {
                Name = "Admin",
                Permissions = Permissions.Admin,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            context.Roles.Add(new AppRole()
            {
                Name = "GodAdmin",
                Permissions = Permissions.GodAdmin,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            await context.SaveChangesAsync();

            var user = new AppUser()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Surname = "",
                Birthday = DateTimeOffset.Now,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now,
                UserName = "admin",
                Email = "admin",
                EmailConfirmed = true
            };

            await manager.CreateAsync(user, AdminPassword);
            await context.SaveChangesAsync();

            context.UserRoles.Add(new AppUserRole()
            {
                RoleId = context.Roles.First(x => x.Permissions == Permissions.GodAdmin).Id,
                UserId = user.Id,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });

            await context.SaveChangesAsync();
        }
        await context.DisposeAsync();
    }
}