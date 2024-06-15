using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;
public record GeneratedRecipeDto
{
    public string Title { get; init; } = string.Empty;
    public IList<string> Ingredients { get; init; } = [];
    public IList<GeneratedInstructionDto> Instructions { get; init; } = [];
    public CookingTime CookingTime { get; init; }
}
