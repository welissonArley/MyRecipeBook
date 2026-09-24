using Bogus;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class RefreshTokenBuilder
{
    public static RefreshToken Build(User user)
    {
        return new Faker<RefreshToken>()
            .RuleFor(refreshToken => refreshToken.Value, faker => faker.Random.String2(64))
            .RuleFor(refreshToken => refreshToken.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(refreshToken => refreshToken.UserId, _ => user.Id);
    }
}
