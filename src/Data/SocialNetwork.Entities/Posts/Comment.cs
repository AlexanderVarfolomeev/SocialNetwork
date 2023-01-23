using System.Collections;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Posts;

public class Comment : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public string Text { get; set; }

    public Guid CreatorId { get; set; }
    public virtual AppUser Creator { get; set; }
    
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; }
    
    public virtual ICollection<Complaint> Complaints { get; set; }
    
    public virtual ICollection<Attachment> Attachments { get; set; }
}