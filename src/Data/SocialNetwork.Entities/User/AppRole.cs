using Microsoft.AspNetCore.Identity;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class AppRole : IdentityRole<Guid>, IBaseEntity
{
    public Permissions Permissions { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
  
    public virtual ICollection<AppUserRole> Users { get; set; }
    
    public void Init()
    {
        CreationDateTime = DateTimeOffset.Now;
        ModificationDateTime = CreationDateTime;
    }

    public void Touch()
    {
        ModificationDateTime = DateTimeOffset.Now;
    }

}