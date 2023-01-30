using AutoMapper;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class AppAccountUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class AppAccountUpdateRequestProfile : Profile
{
    public AppAccountUpdateRequestProfile()
    {
        CreateMap<AppAccountUpdateRequest, AppAccountUpdateModel>();
    }
}
