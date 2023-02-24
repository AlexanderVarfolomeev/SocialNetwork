using AutoMapper;
using SocialNetwork.Entities.Groups;

namespace SocialNetwork.GroupServices.Models;

public class GroupModelRequest
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class GroupModelRequestProfile : Profile
{
    public GroupModelRequestProfile()
    {
        CreateMap<GroupModelRequest, Group>();
    }
}