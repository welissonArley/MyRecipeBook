namespace MyRecipeBook.Application.UseCases.User.DeleteAccount;

public interface IDeleteUserAccountPermanentlyUseCase
{
    Task Execute(Guid userId);
}
