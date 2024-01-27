namespace MyRecipeBook.Communication.Responses;
public class ResponseRegisteredUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokensJson Token { get; set; } = default!;
}
