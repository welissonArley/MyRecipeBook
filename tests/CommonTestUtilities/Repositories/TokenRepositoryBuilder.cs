using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories;
public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repository;

    public TokenRepositoryBuilder() => _repository = new Mock<ITokenRepository>();

    public void Get(RefreshToken refreshToken)
    {
        _repository.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);
    }

    public ITokenRepository Build() => _repository.Object;
}
