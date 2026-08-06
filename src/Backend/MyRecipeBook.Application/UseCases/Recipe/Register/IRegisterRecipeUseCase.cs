using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegiteredRecipeJson> Execute(RequestRecipeJson request, Stream? recipeIllustration);
}