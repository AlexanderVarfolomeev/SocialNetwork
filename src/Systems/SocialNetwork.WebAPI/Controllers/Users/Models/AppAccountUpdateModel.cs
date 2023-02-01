using AutoMapper;
using FluentValidation;
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

public class AccountUpdateValidator : AbstractValidator<AppAccountUpdateRequest>
{
    public AccountUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name {ValidationErrorMessages.NotEmpty}");
        
        RuleFor(x=>x.UserName)
            .NotNull()
            .WithMessage("Username {ValidationErrorMessages.NotEmpty}");
    }
}

public class AppAccountUpdateRequestProfile : Profile
{
    public AppAccountUpdateRequestProfile()
    {
        CreateMap<AppAccountUpdateRequest, AppAccountUpdateModel>();
    }
}
