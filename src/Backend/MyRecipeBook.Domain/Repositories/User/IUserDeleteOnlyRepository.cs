namespace MyRecipeBook.Domain.Repositories.User;
public interface IUserDeleteOnlyRepository
{
    Task DeleteAccount(Guid userIdentifier);
}
