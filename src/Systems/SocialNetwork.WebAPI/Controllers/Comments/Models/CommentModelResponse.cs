using AutoMapper;
using SocialNetwork.CommentService.Models;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.WebAPI.Controllers.Comments.Models;

public class CommentResponse : BaseEntity
{
    public string Text { get; set; }

    public Guid CreatorId { get; set; }

    public Guid PostId { get; set; }
}

public class CommentResponseProfile : Profile
{
    public CommentResponseProfile()
    {
        CreateMap<CommentModelResponse, CommentResponse>();
    }
}