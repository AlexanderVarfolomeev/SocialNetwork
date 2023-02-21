using AutoMapper;
using SocialNetwork.PostServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Posts.Models;

public class PostRequest
{
    public string Text { get; set; } = string.Empty;
}

public class PostRequestProfile : Profile
{
    public PostRequestProfile()
    {
        CreateMap<PostRequest, PostModelUpdate>();
    }
}