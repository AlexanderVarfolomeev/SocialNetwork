using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class Relationship : BaseEntity
{
    public RelationshipType RelationshipType { get; set; }
    
    public Guid FirstUserId { get; set; }
    public virtual AppUser FirstUser { get; set; }
    
    public Guid SecondUserId { get; set; }
    public virtual AppUser SecondUser { get; set; }
}