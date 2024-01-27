namespace MyRecipeBook.Domain.Tokens;
public interface IAccesTokenGenerator
{
    public string Generate(Guid userIdentifier);
}
