using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.PostServices.Models;

public class PostModelResponse : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
    
    
}

public class PostModelResponseProfile : Profile
{
    public PostModelResponseProfile()
    {
        CreateMap<Post, PostModelResponse>();
    }
}
