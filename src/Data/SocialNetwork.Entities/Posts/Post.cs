using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Posts;

public class Post : BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public virtual AppUser Creator { get; set; }

    public Guid? GroupId { get; set; }
    public virtual Group? Group { get; set; }
    
    public virtual ICollection<UserLikePost> Likes { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    
    public virtual ICollection<Complaint> Complaints { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; }

}