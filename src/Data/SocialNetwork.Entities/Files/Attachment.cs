using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Files;

public class Attachment : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public string Name { get; set; }
    public FileType FileType { get; set; }

    public Guid? MessageId { get; set; }
    public virtual Message Message { get; set; }

    public Guid? UserId { get; set; }
    public virtual AppUser User { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;
    
    public Guid? CommentId { get; set; }
    public virtual Comment Comment { get; set; }
    
    public Guid? PostId { get; set; }
    public virtual Post Post { get; set; }
}