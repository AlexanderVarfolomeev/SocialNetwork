using AutoMapper;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class RegisterAccountRequest
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

public class RegisterAccountRequestProfile : Profile
{
    public RegisterAccountRequestProfile()
    {
        CreateMap<RegisterAccountRequest, AppAccountModelRequest>();
    }
}