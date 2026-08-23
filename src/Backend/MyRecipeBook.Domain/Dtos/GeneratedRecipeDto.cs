using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public record GeneratedRecipeDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Difficulty { get; init; } = string.Empty;
    public string Servings { get; init; } = string.Empty;
    public CookTime CookTime { get; init; }
    public IList<GeneratedIngredientDto> Ingredients { get; init; } = [];
    public IList<GeneratedInstructionDto> Instructions { get; init; } = [];
    public byte[]? Image { get; init; }
}

public record GeneratedInstructionDto
{
    public int Order { get; init; }
    public string Description { get; init; } = string.Empty;
}

public record GeneratedIngredientDto
{
    public string Quantity { get; init; } = string.Empty;   // "3/4", "1", "2 1/2", "a gosto"
    public string Unit { get; init; } = string.Empty;   // "cup", "tsp", "unit"...
    public string Name { get; init; } = string.Empty;   // "flour", "banana"...
}