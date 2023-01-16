using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Group;

public class Group : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<UserInGroup> Users { get; set; }
}