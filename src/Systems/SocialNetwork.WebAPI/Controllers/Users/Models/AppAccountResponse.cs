using AutoMapper;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class AppAccountResponse
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }
    public string Status { get; set; } = string.Empty;

    public bool IsBanned { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class AppAccountResponseProfile : Profile
{
    public AppAccountResponseProfile()
    {
        CreateMap<AppAccountModel, AppAccountResponse>();
    }
}