using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestResetPasswordJsonBuilder
{
    public static RequestResetPasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestResetPasswordJson>()
            .RuleFor(request => request.Email, f => f.Internet.Email())
            .RuleFor(request => request.Code, f => f.Random.Int(1, 999_999).ToString("D6"))
            .RuleFor(request => request.NewPassword, f => f.Internet.Password(length: passwordLength));
    }
}
