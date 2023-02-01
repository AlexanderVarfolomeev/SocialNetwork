using FluentValidation;

namespace SocialNetwork.Common.Validator;

public class ModelValidator<T> : IModelValidator<T> where T : class
{
    private readonly FluentValidation.IValidator<T> validator;

    public ModelValidator(FluentValidation.IValidator<T> validator)
    {
        this.validator = validator;
    }

    public void Check(T model)
    {
        var result = validator.Validate(model);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}