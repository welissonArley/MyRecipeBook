using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FullName)
            .RuleFor(request => request.Email, (f, user) => f.Internet.Email(user.Name));
    }
}
