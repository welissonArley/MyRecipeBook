namespace MyRecipeBook.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate();
}
