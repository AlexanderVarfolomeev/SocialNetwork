using FluentValidation;
using SocialNetwork.Constants.Errors;

namespace SocialNetwork.WebAPI.Controllers.Users.Models;

public class ChangePasswordModel
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotNull()
            .WithMessage($"Password {ValidationErrorMessages.NotEmpty}")
            .MinimumLength(6)
            .WithMessage($"{ValidationErrorMessages.MinLen} password is 6.")
            .MaximumLength(16)
            .WithMessage($"{ValidationErrorMessages.MaxLen} password is 16.");
    }
}