using AutoMapper;
using SocialNetwork.ComplaintsServices.Model;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.WebAPI.Controllers.Complaints.Models;

public class ReasonResponse : BaseEntity
{
    public string Name { get; set; }
}

public class ReasonResponseProfile : Profile
{
    public ReasonResponseProfile()
    {
        CreateMap<ReasonModelResponse, ReasonResponse>();
    }
}