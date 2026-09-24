using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RefreshToken;

namespace CommonTestUtilities.Repositories;

public class IRefreshTokenReadOnlyRepositoryBuilder
{
    private readonly Mock<IRefreshTokenReadOnlyRepository> _mock;

    public IRefreshTokenReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IRefreshTokenReadOnlyRepository>();
    }

    public IRefreshTokenReadOnlyRepositoryBuilder Get(RefreshToken refreshToken)
    {
        _mock.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public IRefreshTokenReadOnlyRepository Build() => _mock.Object;
}
