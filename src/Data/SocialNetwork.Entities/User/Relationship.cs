using System.ComponentModel.DataAnnotations.Schema;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.User;

public class Relationship : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }

    public RelationshipType RelationshipType { get; set; }
    
    public Guid FirstUserId { get; set; }
    public virtual AppUser FirstUser { get; set; }
    
    public Guid SecondUserId { get; set; }
    public virtual AppUser SecondUser { get; set; }
}