using AutoMapper;
using SocialNetwork.CommentService.Models;

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