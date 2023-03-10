using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Messenger;

public class Message : BaseEntity
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsModification { get; set; }
    
    public Guid SenderId { get; set; }
    public virtual AppUser Sender { get; set; }
    
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; }
    
    public virtual ICollection<Attachment> Attachments { get; set; }
}