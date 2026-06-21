using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Identity;

public interface ILoggedUser
{
    Task<User> Get();
    Guid GetUserId();
}