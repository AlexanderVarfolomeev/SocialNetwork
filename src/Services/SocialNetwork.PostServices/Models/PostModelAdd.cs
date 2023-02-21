using AutoMapper;
using SocialNetwork.Entities.Posts;

namespace SocialNetwork.PostServices.Models;

public class PostModelAdd
{
    public string Text { get; set; } = string.Empty;
    public bool IsInGroup { get; set; }

    public Guid CreatorId { get; set; }
    public Guid? GroupId { get; set; }
}

public class PostModelAddProfile : Profile
{
    public PostModelAddProfile()
    {
        CreateMap<PostModelAdd, Post>();
    }
}