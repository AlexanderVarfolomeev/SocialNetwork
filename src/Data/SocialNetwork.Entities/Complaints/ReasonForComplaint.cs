using System.Collections;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonForComplaint : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    
    public string Name { get; set; }
    
    public virtual ICollection<ReasonComplaint> Complaints { get; set; }
}