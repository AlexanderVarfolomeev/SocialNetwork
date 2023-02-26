using AutoMapper;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.WebAPI.Controllers.CommonModels;

public class AttachmentResponse : BaseEntity
{ 
    public string Name { get; set; }
    public FileType FileType { get; set; }
}

public class AttachmentResponseProfile : Profile
{
    public AttachmentResponseProfile()
    {
        CreateMap<AttachmentModel, AttachmentResponse>();
    }
}