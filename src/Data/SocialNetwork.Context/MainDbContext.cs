using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Context;

public class MainDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts)
    {
    }

    public override DbSet<AppUser> Users { get; set; }
    public override DbSet<AppRole> Roles { get; set; }
    public new DbSet<AppUserRole> UserRoles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserInGroup> UsersInGroups { get; set; }

    public DbSet<Post> Posts { get; set; }
    public DbSet<UserLikePost> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<ReasonComplaint> ReasonComplaints { get; set; }
    public DbSet<ReasonForComplaint> ReasonForComplaints { get; set; }

    public DbSet<Message> Messages { get; set; }
    public DbSet<Relationship> Relationships { get; set; }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<UserInChat> UsersInChats { get; set; }

    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>().HasKey(x => x.Id);
        builder.Entity<AppRole>().HasKey(x => x.Id);
        builder.Entity<Group>().HasKey(x => x.Id);
        builder.Entity<UserInGroup>().HasKey(x => x.Id);
        builder.Entity<Post>().HasKey(x => x.Id);
        builder.Entity<Comment>().HasKey(x => x.Id);
        builder.Entity<Complaint>().HasKey(x => x.Id);
        builder.Entity<Message>().HasKey(x => x.Id);
        builder.Entity<Relationship>().HasKey(x => x.Id);
        builder.Entity<Chat>().HasKey(x => x.Id);
        builder.Entity<UserInChat>().HasKey(x => x.Id);
        builder.Entity<Attachment>().HasKey(x => x.Id);

        builder.Entity<Complaint>()
            .Property(x => x.PostId)
            .IsRequired(false);
        builder.Entity<Complaint>()
            .Property(x => x.UserId)
            .IsRequired(false);
        builder.Entity<Complaint>()
            .Property(x => x.CommentId)
            .IsRequired(false);
        builder.Entity<Complaint>()
            .Property(x => x.GroupId)
            .IsRequired(false);

        builder.Entity<Attachment>()
            .Property(x => x.CommentId)
            .IsRequired(false);

        builder.Entity<Attachment>()
            .Property(x => x.UserId)
            .IsRequired(false);

        builder.Entity<Attachment>()
            .Property(x => x.PostId)
            .IsRequired(false);

        builder.Entity<Attachment>()
            .Property(x => x.MessageId)
            .IsRequired(false);

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

        builder.Entity<Post>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Group>()
            .HasMany(x => x.Posts)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLikePost>()
            .HasOne(x => x.Post)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserLikePost>()
            .HasOne(x => x.User)
            .WithMany(x => x.LikedPosts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.CreatedPosts)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedPosts)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(x => x.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Post>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReasonComplaint>()
            .HasOne(x => x.Complaint)
            .WithMany(x => x.Reasons)
            .HasForeignKey(x => x.ComplaintId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ReasonComplaint>()
            .HasOne(x => x.Reason)
            .WithMany(x => x.Complaints)
            .HasForeignKey(x => x.ReasonId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Post>()
            .HasMany(x => x.Complaints)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Complaint>()
            .HasOne(x => x.Post)
            .WithMany(x => x.Complaints)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasMany(x => x.Complaints)
            .WithOne(x => x.Comment)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Complaint>()
            .HasOne(x => x.Comment)
            .WithMany(x => x.Complaints)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Group>()
            .HasMany(x => x.Complaints)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Complaint>()
            .HasOne(x => x.Group)
            .WithMany(x => x.Complaints)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.Complaints)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Complaint>()
            .HasOne(x => x.User)
            .WithMany(x => x.Complaints)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.CreatedComplaints)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Complaint>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedComplaints)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Relationship>()
            .HasOne(x => x.FirstUser)
            .WithMany(x => x.MyRelationships)
            .HasForeignKey(x => x.FirstUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.MyRelationships)
            .WithOne(x => x.FirstUser)
            .HasForeignKey(x => x.FirstUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Relationship>()
            .HasOne(x => x.SecondUser)
            .WithMany(x => x.ToMeRelationships)
            .HasForeignKey(x => x.SecondUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.ToMeRelationships)
            .WithOne(x => x.SecondUser)
            .HasForeignKey(x => x.SecondUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
            .HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Chat>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserInChat>()
            .HasOne(x => x.Chat)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserInChat>()
            .HasOne(x => x.User)
            .WithMany(x => x.Chats)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Chat>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attachment>()
            .HasOne(x => x.Comment)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasMany(x => x.Attachments)
            .WithOne(x => x.Comment)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attachment>()
            .HasOne(x => x.User)
            .WithMany(x => x.Avatars)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUser>()
            .HasMany(x => x.Avatars)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attachment>()
            .HasOne(x => x.Post)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Post>()
            .HasMany(x => x.Attachments)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attachment>()
            .HasOne(x => x.Comment)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasMany(x => x.Attachments)
            .WithOne(x => x.Comment)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attachment>()
            .HasOne(x => x.Message)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasMany(x => x.Attachments)
            .WithOne(x => x.Message)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AppUser>()
            .HasMany(x => x.Roles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}