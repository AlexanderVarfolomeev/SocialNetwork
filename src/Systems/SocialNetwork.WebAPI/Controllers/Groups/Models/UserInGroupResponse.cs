using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.GroupServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Groups.Models;

public class UserInGroupResponse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}

public class UserInGroupResponseProfile : Profile
{
    public UserInGroupResponseProfile()
    {
        CreateMap<UserInGroupModel, UserInGroupResponse>();
    }
}