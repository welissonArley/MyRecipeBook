using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRegiteredRecipeJson> Execute(RequestRecipeJson request)
    {
        ValidateAndThrowOnFailures(request);

        var recipe = request.Adapt<Domain.Entities.Recipe>();
        recipe.UserId = _loggedUser.GetUserId();

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return new ResponseRegiteredRecipeJson
        {
            Id = recipe.Id,
            Title = recipe.Title,
        };
    }

    private static void ValidateAndThrowOnFailures(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}