namespace MyRecipeBook.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier);
}
