namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class NotFoundException : MyRecipeBookException
{
    public NotFoundException(string message) : base(message)
    {
    }
}