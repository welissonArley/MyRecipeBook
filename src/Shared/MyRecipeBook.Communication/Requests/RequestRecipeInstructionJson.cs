namespace MyRecipeBook.Communication.Requests;

public class RequestRecipeInstructionJson
{
    public int Order { get; set; }
    public string Description { get; set; } = string.Empty;
}
