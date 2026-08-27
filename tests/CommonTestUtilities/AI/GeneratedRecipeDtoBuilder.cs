using Bogus;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.AI;

public class GeneratedRecipeDtoBuilder
{
    public static GeneratedRecipeDto Build()
    {
        var instructionOrder = 1;

        return new Faker<GeneratedRecipeDto>()
            .RuleFor(recipe => recipe.Title, faker => faker.Lorem.Word())
            .RuleFor(recipe => recipe.Description, faker => faker.Lorem.Sentence())
            .RuleFor(recipe => recipe.Difficulty, faker => faker.PickRandom("Easy", "Medium", "Difficult"))
            .RuleFor(recipe => recipe.Servings, faker => $"{faker.Random.Int(1, 8)} people")
            .RuleFor(recipe => recipe.CookTime, faker => faker.PickRandom<CookTime>())
            .RuleFor(recipe => recipe.Ingredients, faker => faker.Make(3, () => new GeneratedIngredientDto
            {
                Quantity = faker.Random.Int(1, 3).ToString(),
                Unit = faker.PickRandom("cup", "tsp", "unit"),
                Name = faker.Commerce.ProductName()
            }))
            .RuleFor(recipe => recipe.Instructions, faker => faker.Make(3, () => new GeneratedInstructionDto
            {
                Order = instructionOrder++,
                Description = faker.Lorem.Sentence()
            }))
            .RuleFor(recipe => recipe.Image, faker => faker.Random.Bytes(10));
    }
}
