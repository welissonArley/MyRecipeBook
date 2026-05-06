using System.Net;

namespace MyRecipeBook.Exception.ExceptionsBase;

public abstract class MyRecipeBookException : System.Exception
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract List<string> GetErrorMessages();
}
