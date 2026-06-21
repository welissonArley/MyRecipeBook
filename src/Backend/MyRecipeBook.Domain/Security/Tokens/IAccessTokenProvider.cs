namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenProvider
{
    string GetToken();
}