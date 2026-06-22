using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}