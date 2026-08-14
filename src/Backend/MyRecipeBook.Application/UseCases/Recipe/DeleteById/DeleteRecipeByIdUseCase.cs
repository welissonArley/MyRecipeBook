using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.DeleteById;

public class DeleteRecipeByIdUseCase : IDeleteRecipeByIdUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IStorageService _storageService;

    public DeleteRecipeByIdUseCase(
        IRecipeWriteOnlyRepository repository,
        ILoggedUser loggedUser,
        IStorageService storageService)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _storageService = storageService;
    }

    public async Task Execute(Guid recipeId)
    {
        var deleted = await _repository.DeleteById(recipeId, _loggedUser.GetUserId());
        if (deleted == false)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        await _storageService.DeleteRecipeIllustration(_loggedUser.GetUserId(), recipeId);
    }
}
