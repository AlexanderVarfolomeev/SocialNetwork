using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Posts;

public class UserLikePost : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
    
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; }
}