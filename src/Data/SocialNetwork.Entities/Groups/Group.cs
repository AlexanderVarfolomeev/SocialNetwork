using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.Entities.Groups;

public class Group : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<UserInGroup> Users { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    
    public virtual ICollection<Complaint> Complaints { get; set; }
}