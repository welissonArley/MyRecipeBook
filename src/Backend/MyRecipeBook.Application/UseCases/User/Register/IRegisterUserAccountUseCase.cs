using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    void Execute(RequestRegisterUserAccountJson request);
}
