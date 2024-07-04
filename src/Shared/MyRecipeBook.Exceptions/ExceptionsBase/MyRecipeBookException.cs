using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public abstract class MyRecipeBookException : SystemException
{
    public MyRecipeBookException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
