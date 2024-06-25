using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        var step = 1;

        return new Faker<RequestRecipeJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(r => r.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()))
            .RuleFor(r => r.Instructions, f => f.Make(3, () => new RequestInstructionJson
            {
                Text = f.Lorem.Paragraph(),
                Step = step++,
            }));
    }
}
