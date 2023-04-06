using AutoMapper;
using SocialNetwork.Entities.User;

namespace SocialNetwork.AccountServices.Models;

public class AppAccountModel
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

public class AppAccountModelProfile : Profile
{
    public AppAccountModelProfile()
    {
        CreateMap<AppUser, AppAccountModel>();
    }
}