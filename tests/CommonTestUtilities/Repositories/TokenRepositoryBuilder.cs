using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories;
public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repository;

    public TokenRepositoryBuilder() => _repository = new Mock<ITokenRepository>();

    public TokenRepositoryBuilder Get(RefreshToken? refreshToken)
    {
        if(refreshToken is not null)
            _repository.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public ITokenRepository Build() => _repository.Object;
}
