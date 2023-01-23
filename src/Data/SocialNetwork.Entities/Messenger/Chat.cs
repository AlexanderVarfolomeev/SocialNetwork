using System.Collections;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Messenger;

public class Chat : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public bool IsDialog { get; set; }
    public string ChatName { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<UserInChat> Users { get; set; }
}