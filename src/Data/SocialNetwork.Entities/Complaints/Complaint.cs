using System.ComponentModel.DataAnnotations.Schema;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Groups;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;

namespace SocialNetwork.Entities.Complaints;

/// <summary>
/// Жалоба на что либо. Может иметь несколько причин жалобы (порнография, оскорбление, терроризм и тд).
/// Тип жалобы - на что жалоба (пост, группа, пользователь, комментарий).
/// В зависимости от типа, смотрим необходимый Id.
/// </summary>
public class Complaint : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
    
    public ComplaintType Type { get; set; }
    
    public virtual ICollection<ReasonComplaint> Reasons { get; set; }

    public Guid CreatorId { get; set; }
    public virtual AppUser Creator { get; set; }
    
    public Guid? PostId { get; set; }
    public virtual Post? Post { get; set; }
    
    public Guid? CommentId { get; set; }
    public virtual Comment? Comment { get; set; }
    
    public Guid? GroupId { get; set; }
    public virtual Group? Group { get; set; }
    
    public Guid? UserId { get; set; }
    public virtual AppUser? User { get; set; }
}