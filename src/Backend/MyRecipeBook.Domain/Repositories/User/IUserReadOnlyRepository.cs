namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithId(Guid userId);
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Entities.User?> GetByEmail(string email);
}
