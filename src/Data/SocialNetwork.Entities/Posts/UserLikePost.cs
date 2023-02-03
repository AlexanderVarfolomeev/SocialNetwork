using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Posts;

public class UserLikePost : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
    
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; }
}