using Moq;
using MyRecipeBook.Domain.Security.PasswordHashing;

namespace CommonTestUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock;

    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns("hashed-password");
    }

    public IPasswordHasherBuilder VerifyPassword(string password)
    {
        _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordHasher Build() => _mock.Object;
}
