using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Messenger;

public class Chat : BaseEntity
{
    public bool IsDialog { get; set; }
    public string ChatName { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<UserInChat> Users { get; set; }
}