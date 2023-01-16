using Microsoft.AspNetCore.Identity;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class AppRole : IdentityRole<Guid>, IBaseEntity
{
    public Permissions Permissions { get; set; }

    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    
    public virtual ICollection<AppUserRole> Users { get; set; }
}