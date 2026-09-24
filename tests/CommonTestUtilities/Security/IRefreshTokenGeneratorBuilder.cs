using Moq;
using MyRecipeBook.Domain.Security.Tokens;

namespace CommonTestUtilities.Security;

public class IRefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build()
    {
        var mock = new Mock<IRefreshTokenGenerator>();

        mock.Setup(generator => generator.Generate()).Returns("refresh-token");

        return mock.Object;
    }
}
