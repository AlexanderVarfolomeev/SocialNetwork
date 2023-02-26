using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.CommentService.Models;

public class CommentModelResponse : BaseEntity
{
    public string Text { get; set; }

    public Guid CreatorId { get; set; }

    public Guid PostId { get; set; }
}

public class CommentModelResponseProfile : Profile
{
    public CommentModelResponseProfile()
    {
        CreateMap<Comment, CommentModelResponse>();
    }
}