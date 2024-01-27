using MyRecipeBook.Application.Services.Token.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static JwtTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 1000, signingKey: "tttttttttttttttttttttttttttttttt");
}
