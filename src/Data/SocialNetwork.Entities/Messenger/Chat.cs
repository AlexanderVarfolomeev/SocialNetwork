using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Messenger;

public class Chat : BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public bool IsDialog { get; set; }
    public string ChatName { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<UserInChat> Users { get; set; }
}