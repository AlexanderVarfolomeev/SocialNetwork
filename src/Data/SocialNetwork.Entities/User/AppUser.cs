using Microsoft.AspNetCore.Identity;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.Entities.User;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    public DateTime CreationDateTime { get; set; }
    
    public DateTime ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Ключ для подтверждения почты.
    /// Генерируется при регистрации.
    /// Чтобы подтвердить почту, необходими перейти по ссылке содержащей этот ключ.
    /// </summary>
    public string EmailConfirmationKey { get; set; } = string.Empty;
    
    public DateTime Birthday { get; set; }
    public string Status { get; set; } = string.Empty;

    public bool IsBanned { get; set; }
    
    public virtual ICollection<AppUserRole> Roles { get; set; }
    public virtual ICollection<UserInGroup> InGroups { get; set; }
    
    public virtual ICollection<Post> CreatedPosts { get; set; }
    
    public virtual ICollection<UserLikePost> LikedPosts { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Complaint> Complaints { get; set; }
    public virtual ICollection<Complaint> CreatedComplaints { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; }
    
    public virtual ICollection<Relationship> MyRelationships { get; set; }
    public virtual ICollection<Relationship> ToMeRelationships { get; set; }
    
    public virtual ICollection<UserInChat> Chats { get; set; }
    
    
    public virtual ICollection<Attachment> Avatars { get; set; }

    
    public void Init()
    {
        CreationDateTime = DateTime.Now;
        ModificationDateTime = CreationDateTime;
    }

    public void Touch()
    {
        ModificationDateTime = DateTime.Now;
    }
}