using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Token.RefreshToken;
public interface IUseRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}
