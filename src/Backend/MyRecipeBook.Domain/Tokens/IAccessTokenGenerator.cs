namespace MyRecipeBook.Domain.Tokens;
public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier);
}
