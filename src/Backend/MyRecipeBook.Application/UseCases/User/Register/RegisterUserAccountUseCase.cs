using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase
{
    public void Execute(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);
    }
}
