using AutoMapper;
using SocialNetwork.GroupServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Groups.Models;

public class GroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class GroupRequestProfile : Profile
{
    public GroupRequestProfile()
    {
        CreateMap<GroupRequest, GroupModelRequest>();
    }
}