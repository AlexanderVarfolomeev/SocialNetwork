using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SocialNetwork.Entities.Group;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Context;

public class MainDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) { }

    public override DbSet<AppUser> Users { get; set; }
    public override DbSet<AppRole> Roles { get; set; }
    
    public DbSet<AppUserRole> UserRoles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserInGroup> UsersInGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>().HasKey(x => x.Id);
        builder.Entity<AppRole>().HasKey(x => x.Id);
        builder.Entity<AppUserRole>().HasKey(x => x.Id);
        builder.Entity<Group>().HasKey(x => x.Id);
        builder.Entity<UserInGroup>().HasKey(x => x.Id);

        builder.Entity<AppUserRole>()
            .HasOne(x => x.User)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUserRole>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Group>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserInGroup>()
            .HasOne(x => x.User)
            .WithMany(x => x.InGroups)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}