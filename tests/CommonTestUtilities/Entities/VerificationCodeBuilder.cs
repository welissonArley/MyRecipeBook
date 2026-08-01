using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class VerificationCodeBuilder
{
    public static VerificationCode Build(User user)
    {
        return new Faker<VerificationCode>()
            .RuleFor(code => code.Code, f => f.Random.Int(1, 999_999).ToString("D6"))
            .RuleFor(code => code.Type, _ => VerificationCodeType.PasswordRecovery)
            .RuleFor(code => code.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(code => code.UserId, _ => user.Id);
    }
}
