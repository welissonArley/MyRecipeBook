using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class RecipeBuilder
{
    public static Recipe Build(User user)
    {
        var instructionOrder = 1;

        return new Faker<Recipe>()
            .RuleFor(entity => entity.Title, f => f.Lorem.Word())
            .RuleFor(entity => entity.CookTime, f => f.PickRandom<CookTime>())
            .RuleFor(entity => entity.Ingredients, f => f.Make(3, () => new RecipeIngredient
            {
                Item = f.Commerce.ProductName()
            }))
            .RuleFor(entity => entity.DishTypes, f => f.Make(2, () => new RecipeDishType
            {
                Type = f.PickRandom<DishType>()
            }))
            .RuleFor(entity => entity.Instructions, f => f.Make(3, () => new RecipeInstruction
            {
                Order = instructionOrder++,
                Description = f.Lorem.Sentence(),
            }))
            .RuleFor(entity => entity.UserId, _ => user.Id);
    }
}
