using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Groups;

namespace SocialNetwork.GroupServices.Models;

public class GroupModelResponse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class GroupModelResponseProfile : Profile
{
    public GroupModelResponseProfile()
    {
        CreateMap<Group, GroupModelResponse>();
    }
}