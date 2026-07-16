using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GetRecipeByIdUseCase(IRecipeReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRecipeJson> Execute(Guid recipeId)
    {
        var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId());
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        return recipe.Adapt<ResponseRecipeJson>();
    }
}