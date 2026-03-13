using FluentValidation;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
    public RegisterUserAccountValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("O nome nao pode ser vazio");
        RuleFor(user => user.Email).NotEmpty().WithMessage("O email nao pode ser vazio");
        RuleFor(user => user.Password).NotEmpty().WithMessage("A senha nao pode ser vazia");
        When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("O email deve ser valido");
        });
    }
}
