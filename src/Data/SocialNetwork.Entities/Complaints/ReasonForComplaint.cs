using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonForComplaint : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<ReasonComplaint> Complaints { get; set; }
}