using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.PostServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Posts.Models;

public class PostResponse : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
}

public class PostViewResponseProfile : Profile
{
    public PostViewResponseProfile()
    {
        CreateMap<PostModelResponse, PostResponse>();
    }
}