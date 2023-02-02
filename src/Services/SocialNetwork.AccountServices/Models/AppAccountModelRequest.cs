using AutoMapper;
using SocialNetwork.Entities.User;

namespace SocialNetwork.AccountServices.Models;
public class AppAccountModelRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public DateTimeOffset Birthday { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Password { get; set; } = String.Empty;
}

public class AppAccountModelRequestProfile : Profile
{
    public AppAccountModelRequestProfile()
    {
        CreateMap<AppAccountModelRequest, AppUser>();
    }
}