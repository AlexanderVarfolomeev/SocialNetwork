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
    
    public Guid? MessageId { get; set; }
    public Message Message { get; set; }

    public Guid? UserId { get; set; }
    public AppUser User { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;

    public Guid? CommentId { get; set; }
    public Comment Comment { get; set; }

    public Guid? PostId { get; set; }
    public Post Post { get; set; }
}

public class AttachmentModelProfile : Profile
{
    public AttachmentModelProfile()
    {
        CreateMap<Attachment, AttachmentModel>();
    }
}