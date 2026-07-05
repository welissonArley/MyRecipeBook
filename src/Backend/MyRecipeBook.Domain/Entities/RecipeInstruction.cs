namespace MyRecipeBook.Domain.Entities;

public class RecipeInstruction : EntityBase
{
    public int Order { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid RecipeId { get; private set; }
}