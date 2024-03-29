using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Groups;

public class UserInGroup : BaseEntity
{
    public bool IsCreator { get; set; }
    public bool IsAdmin { get; set; }

    public DateTime DateOfEntry { get; set; }

    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }

    public Guid GroupId { get; set; }
    public virtual Group Group { get; set; }
}