using Moq;
using MyRecipeBook.Domain.Repositories.VerificationCode;

namespace CommonTestUtilities.Repositories;

public class IVerificationCodeWriteOnlyRepositoryBuilder
{
    public static IVerificationCodeWriteOnlyRepository Build()
    {
        var mock = new Mock<IVerificationCodeWriteOnlyRepository>();

        return mock.Object;
    }
}
