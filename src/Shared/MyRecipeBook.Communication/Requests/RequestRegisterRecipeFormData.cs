using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Communication.Requests;
public class RequestRegisterRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}
