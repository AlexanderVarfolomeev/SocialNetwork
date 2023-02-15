using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Common.Enum;

namespace SocialNetwork.AttachmentServices.Models;

public class AttachmentModelRequest
{
    public FileType FileType { get; set; }
    public IEnumerable<IFormFile> Files { get; set; }
    
    public Guid? MessageId { get; set; }

    public Guid? UserId { get; set; }
    public bool IsCurrentAvatar { get; set; } = false;

    public Guid? CommentId { get; set; }

    public Guid? PostId { get; set; }
}

/*public class AttachmentModelRequestProfile : Profile
{
    public AttachmentModelRequestProfile()
    {
        CreateMap<AttachmentModelRequest, AttachmentModel>();
    }
}*/