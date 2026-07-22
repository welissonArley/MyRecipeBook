namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipesJson
{
    public IList<ResponseRecipeSummaryJson> Recipes { get; set; } = [];
}