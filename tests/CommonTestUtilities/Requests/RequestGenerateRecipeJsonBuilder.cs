using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build(int count = 5)
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(user => user.Ingredients, faker => faker.Make(count, () => faker.Commerce.ProductName()));
    }
}
