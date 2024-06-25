namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;
public interface IDeleteUserAccountUseCase
{
    Task Execute(Guid userIdentifier);
}
