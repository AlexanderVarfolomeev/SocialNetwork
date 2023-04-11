using AutoMapper;
using SocialNetwork.Entities.User;

namespace SocialNetwork.AccountServices.Models;

public class AppAccountUpdateModel
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class AppAccountUpdateModelProfile : Profile
{
    public AppAccountUpdateModelProfile()
    {
        CreateMap<AppAccountUpdateModel, AppUser>();
    }
}
