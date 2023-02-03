using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class AppUserRole : BaseEntity
{
    public Guid RoleId { get; set; }
    public virtual AppRole Role { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
    
}