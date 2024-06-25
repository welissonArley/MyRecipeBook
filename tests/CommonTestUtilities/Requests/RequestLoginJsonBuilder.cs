using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(u => u.Email, (f) => f.Internet.Email())
            .RuleFor(u => u.Password, (f) => f.Internet.Password());
    }
}
