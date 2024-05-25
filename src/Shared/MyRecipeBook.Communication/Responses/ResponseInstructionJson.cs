namespace MyRecipeBook.Communication.Responses;
public class ResponseInstructionJson
{
    public string Id { get; set; } = string.Empty;
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
}
