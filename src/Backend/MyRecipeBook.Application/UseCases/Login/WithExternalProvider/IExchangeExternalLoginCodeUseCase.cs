using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Login.WithExternalProvider;

public interface IExchangeExternalLoginCodeUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestExternalLoginJson request);
}
