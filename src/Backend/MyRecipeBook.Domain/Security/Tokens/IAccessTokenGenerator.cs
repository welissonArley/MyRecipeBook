using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}