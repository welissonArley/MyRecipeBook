using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateById;

public class UpdateRecipeByIdUseCase : IUpdateRecipeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRecipeByIdUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository repository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid recipeId, RequestRecipeJson request)
    {
        ValidateAndThrowOnFailures(request);

        var recipe = await _repository.GetById(recipeId, _loggedUser.GetUserId());
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        //TODO: ATUALIZAÇÃO DE PROPRIEDADES DO RECEITA AQUI

        await _unitOfWork.Commit();
    }

    private static void ValidateAndThrowOnFailures(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
