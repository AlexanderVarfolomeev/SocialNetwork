using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Common.Enum;
using SocialNetwork.Common.Extensions;
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
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        Task.Run(async () => { await AddRolesAndAdmin(context, userManager, roleManager); });
    }

    private static async Task AddRolesAndAdmin(MainDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new AppRole
            {
                Name = Permissions.User.GetName(),
                Permissions = Permissions.User,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            await roleManager.CreateAsync(new AppRole
            {
                Name = Permissions.Admin.GetName(),
                Permissions = Permissions.Admin,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            await roleManager.CreateAsync(new AppRole
            {
                Name = Permissions.GodAdmin.GetName(),
                Permissions = Permissions.GodAdmin,
                CreationDateTime = DateTimeOffset.Now,
                ModificationDateTime = DateTimeOffset.Now
            });
            await context.SaveChangesAsync();

            var user = new AppUser
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

            await userManager.CreateAsync(user, AdminPassword);
            await userManager.AddToRoleAsync(user, (await roleManager.FindByNameAsync(Permissions.GodAdmin.GetName()))!.ToString());
            await context.SaveChangesAsync();
        }
        await context.DisposeAsync();
    }
}