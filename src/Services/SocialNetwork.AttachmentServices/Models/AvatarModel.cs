using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Files;

namespace SocialNetwork.AttachmentServices.Models;

public class AvatarModel : BaseEntity
{
    public Guid? UserId { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;
    public string Content { get; set; } // картинка в base64
    
}

public class AvatarModelProfile : Profile
{
    public AvatarModelProfile()
    {
        CreateMap<Attachment, AvatarModel>();
    }
}