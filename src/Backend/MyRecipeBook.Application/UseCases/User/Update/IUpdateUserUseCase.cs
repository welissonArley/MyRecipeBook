using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Update;
public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
