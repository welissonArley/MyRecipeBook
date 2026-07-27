using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public interface IFilterRecipesUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipesJson? request);
}
