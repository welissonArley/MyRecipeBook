using FluentValidation;
using MyRecipeBook.Application.UseCases.Shared.Validators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<RequestResetPasswordJson>
{
    public ResetPasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}
