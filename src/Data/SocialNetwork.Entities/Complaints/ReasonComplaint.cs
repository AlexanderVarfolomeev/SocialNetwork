using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonComplaint : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    
    public Guid ReasonId { get; set; }
    public virtual ReasonForComplaint Reason { get; set; }
    
    public Guid ComplaintId { get; set; }
    public virtual Complaint Complaint { get; set; }
}