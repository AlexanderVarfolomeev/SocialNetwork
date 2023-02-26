using AutoMapper;
using SocialNetwork.CommentService.Models;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.WebAPI.Controllers.Comments.Models;

public class CommentRequest
{
    public string Text { get; set; }
}

public class CommentRequestProfile : Profile
{
    public CommentRequestProfile()
    {
        CreateMap<CommentRequest, CommentModelRequest>();
    }
}