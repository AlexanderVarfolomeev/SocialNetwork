using System.ComponentModel.DataAnnotations.Schema;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.Entities.Groups;

public class Group : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<UserInGroup> Users { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    
    public virtual ICollection<Complaint> Complaints { get; set; }
}