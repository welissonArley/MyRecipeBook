using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipeJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    public IList<ResponseInstructionJson> Instructions { get; set; } = [];
    public IList<string> Ingredients { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}