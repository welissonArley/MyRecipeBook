using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new PasswordEncripter("abc1234");
}
