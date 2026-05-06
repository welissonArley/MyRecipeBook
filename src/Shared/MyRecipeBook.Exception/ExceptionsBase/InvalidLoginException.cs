using System.Net;

namespace MyRecipeBook.Exception.ExceptionsBase;

public class InvalidLoginException : MyRecipeBookException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
