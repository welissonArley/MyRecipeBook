using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IStorageService _storageService;

    public GetRecipeByIdUseCase(
        IRecipeReadOnlyRepository repository,
        ILoggedUser loggedUser,
        IStorageService storageService)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _storageService = storageService;
    }

    public async Task<ResponseRecipeJson> Execute(Guid recipeId)
    {
        var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId());
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        var response = recipe.Adapt<ResponseRecipeJson>();
        response.ImageUrl = recipe.HasImage ? _storageService.GetRecipeIllustrationUrl(userId: recipe.UserId, recipeId: recipe.Id) : string.Empty;

        return response;
    }
}