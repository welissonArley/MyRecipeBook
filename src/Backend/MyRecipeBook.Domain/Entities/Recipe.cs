using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;
public class Recipe : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<Ingredient> Ingredients { get; set; } = [];
    public IList<Instruction> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
    public string? ImageIdentifier { get; set; }
    public long UserId { get; set; }
}
