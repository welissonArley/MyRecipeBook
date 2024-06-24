namespace MyRecipeBook.Domain.Repositories.Token;
public interface ITokenRepository
{
    Task<Entities.RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken(Entities.RefreshToken refreshToken);
}
