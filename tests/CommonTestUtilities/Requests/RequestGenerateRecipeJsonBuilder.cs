using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build()
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(request => request.Prompt, faker => faker.Lorem.Sentence());
    }
}
