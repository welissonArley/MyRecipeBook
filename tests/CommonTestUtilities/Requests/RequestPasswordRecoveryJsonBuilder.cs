using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestPasswordRecoveryJsonBuilder
{
    public static RequestPasswordRecoveryJson Build()
    {
        return new Faker<RequestPasswordRecoveryJson>()
            .RuleFor(request => request.Email, f => f.Internet.Email());
    }
}
