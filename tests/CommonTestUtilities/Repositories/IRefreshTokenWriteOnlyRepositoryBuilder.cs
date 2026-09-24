using Moq;
using MyRecipeBook.Domain.Repositories.RefreshToken;

namespace CommonTestUtilities.Repositories;

public class IRefreshTokenWriteOnlyRepositoryBuilder
{
    public static IRefreshTokenWriteOnlyRepository Build()
    {
        var mock = new Mock<IRefreshTokenWriteOnlyRepository>();

        return mock.Object;
    }
}
