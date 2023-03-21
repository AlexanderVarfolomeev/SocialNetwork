using AutoMapper;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;

namespace SocialNetwork.ComplaintsServices.Model;

public class ComplaintModelResponse : BaseEntity
{
    public ComplaintType Type { get; set; }

    public Guid CreatorId { get; set; }

    public Guid? PostId { get; set; }

    public Guid? CommentId { get; set; }

    public Guid? GroupId { get; set; }
    public Guid? UserId { get; set; }

    public List<string> ReasonsStrings { get; set; } = new List<string>();
}

public class ComplaintModelResponseProfile : Profile
{
    public ComplaintModelResponseProfile()
    {
        CreateMap<Complaint, ComplaintModelResponse>();
    }
}