using Bogus;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Dtos;
public class GeneratedRecipeDtoBuilder
{
    public static GeneratedRecipeDto Build()
    {
        return new Faker<GeneratedRecipeDto>()
            .RuleFor(r => r.Title, faker => faker.Lorem.Word())
            .RuleFor(r => r.CookingTime, faker => faker.PickRandom<CookingTime>())
            .RuleFor(r => r.Ingredients, faker => faker.Make(1, () => faker.Commerce.ProductName()))
            .RuleFor(r => r.Instructions, faker => faker.Make(1, () => new GeneratedInstructionDto
            {
                Step = 1,
                Text = faker.Lorem.Paragraph()
            }));
    }
}
