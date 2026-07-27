using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public record RecipeFilterDto
{
    public string? SearchTerm { get; init; }
    public CookTime? CookTime { get; init; }
    public IList<DishType> DishTypes { get; init; } = [];
}