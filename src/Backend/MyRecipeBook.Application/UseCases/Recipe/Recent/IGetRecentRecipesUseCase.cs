using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Recent;

public interface IGetRecentRecipesUseCase
{
    Task<ResponseRecipesJson> Execute();
}
