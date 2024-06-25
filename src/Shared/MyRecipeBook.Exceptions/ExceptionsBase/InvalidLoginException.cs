namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
    {
    }
}
