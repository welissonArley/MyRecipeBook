namespace MyRecipeBook.Domain.Security.Tokens;
public interface IRefreshTokenGenerator
{
    public string Generate();
}
