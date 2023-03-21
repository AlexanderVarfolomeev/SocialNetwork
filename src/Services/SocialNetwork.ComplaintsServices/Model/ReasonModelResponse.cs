using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Complaints;

namespace SocialNetwork.ComplaintsServices.Model;

public class ReasonModelResponse : BaseEntity
{
     public string Name { get; set; }
}

public class ReasonModelResponseProfile : Profile
{
     public ReasonModelResponseProfile()
     {
          CreateMap<ReasonForComplaint, ReasonModelResponse>();
     }
}