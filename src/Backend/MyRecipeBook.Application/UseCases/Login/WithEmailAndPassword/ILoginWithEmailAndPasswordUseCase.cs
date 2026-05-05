using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}