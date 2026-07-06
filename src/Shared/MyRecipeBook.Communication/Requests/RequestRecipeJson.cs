using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests;

public class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestRecipeInstructionJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
