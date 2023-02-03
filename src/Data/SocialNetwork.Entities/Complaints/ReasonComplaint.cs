using SocialNetwork.Entities.Base;

namespace SocialNetwork.Entities.Complaints;

public class ReasonComplaint : BaseEntity
{
    public Guid ReasonId { get; set; }
    public virtual ReasonForComplaint Reason { get; set; }
    
    public Guid ComplaintId { get; set; }
    public virtual Complaint Complaint { get; set; }
}