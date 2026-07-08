using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        var instructionOrder = 1;

        return new Faker<RequestRecipeJson>()
            .RuleFor(request => request.Title, f => f.Lorem.Word())
            .RuleFor(request => request.CookTime, f => f.PickRandom<CookTime>())
            .RuleFor(request => request.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(request => request.DishTypes, f => f.Make(2, () => f.PickRandom<DishType>()).Distinct().ToList())
            .RuleFor(request => request.Instructions, f => f.Make(3, () => new RequestRecipeInstructionJson
            {
                Order = instructionOrder++,
                Description = f.Lorem.Sentence(),
            }));
    }
}
