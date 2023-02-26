using AutoMapper;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.CommentService.Models;

public class CommentModelRequest
{
    public string Text { get; set; }
}

public class CommentModelRequestProfile : Profile
{
    public CommentModelRequestProfile()
    {
        CreateMap<CommentModelRequest, Comment>();
    }
}