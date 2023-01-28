using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class AppUserRole : IBaseEntity
{
    public Guid RoleId { get; set; }
    public virtual AppRole Role { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
    
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
}