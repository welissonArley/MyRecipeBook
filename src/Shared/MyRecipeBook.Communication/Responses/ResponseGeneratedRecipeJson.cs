using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Responses;
public class ResponseGeneratedRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<ResponseGeneratedInstructionJson> Instructions { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
}