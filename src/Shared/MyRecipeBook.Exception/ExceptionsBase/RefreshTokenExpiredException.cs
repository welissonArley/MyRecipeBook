using System.Net;

namespace MyRecipeBook.Exception.ExceptionsBase;

public class RefreshTokenExpiredException : MyRecipeBookException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.EXPIRED_REFRESH_TOKEN];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
