using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
