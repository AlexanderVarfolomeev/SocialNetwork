using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonComplaint : BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
    
    public Guid ReasonId { get; set; }
    public virtual ReasonForComplaint Reason { get; set; }
    
    public Guid ComplaintId { get; set; }
    public virtual Complaint Complaint { get; set; }
}