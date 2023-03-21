using AutoMapper;
using SocialNetwork.Common.Enum;
using SocialNetwork.ComplaintsServices.Model;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.WebAPI.Controllers.Complaints.Models;

public class ComplaintResponse : BaseEntity
{
    public ComplaintType Type { get; set; }

    public Guid CreatorId { get; set; }

    public Guid? PostId { get; set; }

    public Guid? CommentId { get; set; }

    public Guid? GroupId { get; set; }
    public Guid? UserId { get; set; }
    public List<string> ReasonsStrings { get; set; }
}

public class ComplaintResponseProfile : Profile
{
    public ComplaintResponseProfile()
    {
        CreateMap<ComplaintModelResponse, ComplaintResponse>();
    }
}