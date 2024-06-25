using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;
public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
