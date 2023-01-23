using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Messenger;

public class UserInChat : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public bool IsCreator { get; set; }
    public DateTimeOffset EntryDate { get; set; }
    
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
}