using AutoMapper;
using SocialNetwork.Common.Enum;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Files;
using SocialNetwork.Entities.Messenger;
using SocialNetwork.Entities.Posts;
using SocialNetwork.Entities.User;

namespace SocialNetwork.AttachmentServices.Models;

public class AttachmentModel : BaseEntity
{
    public string Name { get; set; }
    public FileType FileType { get; set; }
}

public class AttachmentModelProfile : Profile
{
    public AttachmentModelProfile()
    {
        CreateMap<Attachment, AttachmentModel>();
    }
}