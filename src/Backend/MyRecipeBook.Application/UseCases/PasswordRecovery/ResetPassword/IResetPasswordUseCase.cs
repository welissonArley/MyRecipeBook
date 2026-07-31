using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;

public interface IResetPasswordUseCase
{
    Task Execute(RequestResetPasswordJson request);
}
