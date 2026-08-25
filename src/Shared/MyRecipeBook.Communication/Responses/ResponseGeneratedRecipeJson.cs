using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses;

public class ResponseGeneratedRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Difficulty { get; init; } = string.Empty;
    public string Servings { get; init; } = string.Empty;
    public CookTime CookTime { get; set; }
    public IList<ResponseGeneratedIngredientRecipeJson> Ingredients { get; set; } = [];
    public IList<ResponseInstructionJson> Instructions { get; set; } = [];
    public byte[] Image { get; set; } = [];
}

public class ResponseGeneratedIngredientRecipeJson
{
    public string Quantity { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}