namespace MyRecipeBook.Domain.Repositories.RefreshToken;

public interface IRefreshTokenWriteOnlyRepository
{
    Task Replace(Entities.RefreshToken refreshToken);
}
