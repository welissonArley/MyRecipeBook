using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;
public class UserDeleteOnlyRepository
{
    public static IUserDeleteOnlyRepository Build()
    {
        var mock = new Mock<IUserDeleteOnlyRepository>();

        return mock.Object;
    }
}
