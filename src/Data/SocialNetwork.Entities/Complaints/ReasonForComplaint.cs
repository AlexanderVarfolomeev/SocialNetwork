using System.ComponentModel.DataAnnotations.Schema;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonForComplaint : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
    
    public string Name { get; set; }
    
    public virtual ICollection<ReasonComplaint> Complaints { get; set; }
}