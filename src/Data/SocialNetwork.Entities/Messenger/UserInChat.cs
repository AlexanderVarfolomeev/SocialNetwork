using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Messenger;

public class UserInChat : BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public bool IsCreator { get; set; }
    
    public DateTimeOffset EntryDate { get; set; }
    
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
}