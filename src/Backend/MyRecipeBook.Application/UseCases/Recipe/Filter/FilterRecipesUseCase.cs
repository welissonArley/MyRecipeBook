using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipesUseCase : IFilterRecipesUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;

    public FilterRecipesUseCase(ILoggedUser loggedUser, IRecipeReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipesJson? request)
    {
        var filter = request is null ? new RecipeFilterDto() :
            new RecipeFilterDto
            {
                SearchTerm = request.SearchTerm,
                CookTime = (Domain.Enums.CookTime?)request.CookTime,
                DishTypes = request.DishTypes.Select(dishType => (Domain.Enums.DishType)dishType).ToList()
            };

        var recipes = await _repository.FilterRecipes(_loggedUser.GetUserId(), filter);

        return new ResponseRecipesJson
        {
            Recipes = recipes.Adapt<IList<ResponseRecipeSummaryJson>>()
        };
    }
}