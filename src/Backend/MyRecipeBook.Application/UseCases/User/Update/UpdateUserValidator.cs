using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);

        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
    }
}