using AutoMapper;
using SocialNetwork.AttachmentServices.Models;

namespace SocialNetwork.WebAPI.Controllers.CommonModels;

public class AttachmentViewResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; } // картинка в base64
}

public class AttachmentViewResponseProfile : Profile
{
    public AttachmentViewResponseProfile()
    {
        CreateMap<AttachmentViewModel, AttachmentViewResponse>();
    }
}