using SocialNetwork.Common.Enum;

namespace SocialNetwork.ComplaintsServices.Model;

public class ComplaintModelRequest
{
    public ComplaintType Type { get; set; }

    public Guid CreatorId { get; set; }
    public Guid ContentId { get; set; }
}