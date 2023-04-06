using AutoMapper;
using FluentValidation;
using SocialNetwork.AccountServices.Models;
using SocialNetwork.Constants.Errors;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class RegisterAccountRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Password { get; set; } = String.Empty;
}

public class RegisterAccountValidator : AbstractValidator<RegisterAccountRequest>
{
    public RegisterAccountValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name {ValidationErrorMessages.NotEmpty}");
        
        RuleFor(x=>x.Birthday)
            .NotNull()
            .WithMessage("Birthday {ValidationErrorMessages.NotEmpty}");
        
        RuleFor(x=>x.UserName)
            .NotNull()
            .WithMessage("Username {ValidationErrorMessages.NotEmpty}");
        
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email {ValidationErrorMessages.NotEmpty}")
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage($"Password {ValidationErrorMessages.NotEmpty}")
            .MinimumLength(6)
            .WithMessage($"{ValidationErrorMessages.MinLen} password is 6.")
            .MaximumLength(16)
            .WithMessage($"{ValidationErrorMessages.MaxLen} password is 16.");
    }
}

public class RegisterAccountRequestProfile : Profile
{
    public RegisterAccountRequestProfile()
    {
        CreateMap<RegisterAccountRequest, AppAccountModelRequest>();
    }
}