using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.GroupServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Groups.Models;

public class GroupResponse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
}

public class GroupResponseProfile : Profile
{
    public GroupResponseProfile()
    {
        CreateMap<GroupModelResponse, GroupResponse>();
    }
}