using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}