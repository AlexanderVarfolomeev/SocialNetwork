using AutoMapper;
using SocialNetwork.AttachmentServices.Models;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class AvatarResponse : BaseEntity
{
    public Guid? UserId { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;
    public string Content { get; set; } // картинка в base64
}

public class AvatarResponseProfile : Profile
{
    public AvatarResponseProfile()
    {
        CreateMap<AvatarModel, AvatarResponse>();
    }
}