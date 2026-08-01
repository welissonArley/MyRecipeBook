using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.VerificationCode;

namespace CommonTestUtilities.Repositories;

public class IVerificationCodeReadOnlyRepositoryBuilder
{
    private readonly Mock<IVerificationCodeReadOnlyRepository> _mock = new();

    public IVerificationCodeReadOnlyRepositoryBuilder Get(VerificationCode verificationCode)
    {
        _mock
            .Setup(repository => repository.Get(verificationCode.UserId, verificationCode.Code, verificationCode.Type))
            .ReturnsAsync(verificationCode);

        return this;
    }

    public IVerificationCodeReadOnlyRepository Build() => _mock.Object;
}
