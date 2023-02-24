using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Files;

namespace SocialNetwork.AttachmentServices.Models;


public class AttachmentViewModel : BaseEntity
{
    public string Content { get; set; } // картинка в base64
}

public class AttachmentViewModelProfile : Profile
{
    public AttachmentViewModelProfile()
    {
        CreateMap<Attachment, AttachmentViewModel>();
    }
}