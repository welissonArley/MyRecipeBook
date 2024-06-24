namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class RefreshTokenExpiredException : MyRecipeBookException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
    {
    }
}
