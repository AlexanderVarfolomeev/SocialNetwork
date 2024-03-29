using AutoMapper;
using SocialNetwork.Entities.Base;
using SocialNetwork.Entities.Groups;

namespace SocialNetwork.GroupServices.Models;

public class UserInGroupModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    
    public bool IsCreator { get; set; }
    public bool IsAdmin { get; set; }

    public DateTime DateOfEntry { get; set; }

}

public class UserInGroupModelProfile : Profile
{
    public UserInGroupModelProfile()
    {
        CreateMap<UserInGroup, UserInGroupModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(y => y.User.Name))
            .ForMember(x => x.Surname, opt => opt.MapFrom(y => y.User.Surname));
    }
}