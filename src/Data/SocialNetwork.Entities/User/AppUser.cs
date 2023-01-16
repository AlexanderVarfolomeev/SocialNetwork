using Microsoft.AspNetCore.Identity;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Group;

namespace SocialNetwork.Entities.User;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public DateTimeOffset Birthday { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public bool IsBanned { get; set; }
    
    public virtual ICollection<AppUserRole> Roles { get; set; }
    public virtual ICollection<UserInGroup> InGroups { get; set; }
}