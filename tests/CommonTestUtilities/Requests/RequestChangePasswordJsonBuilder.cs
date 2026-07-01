using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int newPasswordLength = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.CurrentPassword, f => f.Internet.Password())
            .RuleFor(request => request.NewPassword, f => f.Internet.Password(length: newPasswordLength));
    }
}
