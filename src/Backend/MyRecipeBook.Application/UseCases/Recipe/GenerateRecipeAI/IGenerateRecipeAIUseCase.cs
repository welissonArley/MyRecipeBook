using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GenerateRecipeAI;

public interface IGenerateRecipeAIUseCase
{
    Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
}
