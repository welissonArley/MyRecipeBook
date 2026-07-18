using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.DeleteById;

public class DeleteRecipeByIdUseCase : IDeleteRecipeByIdUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public DeleteRecipeByIdUseCase(IRecipeWriteOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid recipeId)
    {
        var deleted = await _repository.DeleteById(recipeId, _loggedUser.GetUserId());
        if (deleted == false)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
    }
}
