using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserAccountJsonBuilder
{
    public static RequestRegisterUserAccountJson Build()
    {
        return new Faker<RequestRegisterUserAccountJson>()
            .RuleFor(request => request.Name, f => f.Person.FullName)
            .RuleFor(request => request.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(request => request.Password, f => f.Internet.Password());
    }
}
