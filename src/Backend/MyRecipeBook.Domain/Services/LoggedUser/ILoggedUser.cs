using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    public Task<User> User();
}
