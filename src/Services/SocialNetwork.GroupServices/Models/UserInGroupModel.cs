using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.User;

namespace SocialNetwork.GroupServices.Models;

// TODO avatar?
public class UserInGroupModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}

public class UserInGroupModelProfile : Profile
{
    public UserInGroupModelProfile()
    {
        CreateMap<AppUser, UserInGroupModel>();
    }
}