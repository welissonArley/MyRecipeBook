using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;

public interface IRequestPasswordRecoveryCodeUseCase
{
    Task Execute(RequestPasswordRecoveryJson request);
}