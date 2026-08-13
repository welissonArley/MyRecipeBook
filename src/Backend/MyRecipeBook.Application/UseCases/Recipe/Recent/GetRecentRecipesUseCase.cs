using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Application.UseCases.Recipe.Recent;

public class GetRecentRecipesUseCase : IGetRecentRecipesUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IStorageService _storageService;

    public GetRecentRecipesUseCase(ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IStorageService storageService)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _storageService = storageService;
    }

    public async Task<ResponseRecipesJson> Execute()
    {
        var recipes = await _repository.GetRecentRecipes(_loggedUser.GetUserId());

        var response = new ResponseRecipesJson
        {
            Recipes = recipes.ToResponseJson(_loggedUser.GetUserId(), _storageService)
        };

        return response;
    }
}
