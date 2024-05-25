using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;
public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repositoryRead;
    private readonly IRecipeWriteOnlyRepository _repositoryWrite;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repositoryRead,
        IRecipeWriteOnlyRepository repositoryWrite,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repositoryRead = repositoryRead;
        _repositoryWrite = repositoryWrite;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repositoryRead.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        await _repositoryWrite.Delete(recipeId);

        await _unitOfWork.Commit();
    }
}