using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Messenger;

public class UserInChat : BaseEntity
{
    public bool IsCreator { get; set; }
    
    public DateTime EntryDate { get; set; }
    
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; }
    
    public Guid UserId { get; set; }
    public virtual AppUser User { get; set; }
}