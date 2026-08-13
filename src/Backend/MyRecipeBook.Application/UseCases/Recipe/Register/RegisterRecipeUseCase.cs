using Mapster;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IStorageService _storageService;
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _storageService = storageService;
    }

    public async Task<ResponseRegiteredRecipeJson> Execute(RequestRecipeJson request, Stream? recipeIllustration)
    {
        ValidateAndThrowOnFailures(request);

        var recipe = request.Adapt<Domain.Entities.Recipe>();
        recipe.UserId = _loggedUser.GetUserId();

        if (recipeIllustration is not null)
        {
            var contentType = recipeIllustration.DetectImageContentType();
            if (contentType.IsEmpty())
                throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED]);

            recipe.HasImage = true;

            await _storageService.UploadIllustration(recipe, recipeIllustration, contentType);
        }

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return new ResponseRegiteredRecipeJson
        {
            Id = recipe.Id,
            Title = recipe.Title,
            ImageUrl = recipe.HasImage ? _storageService.GetRecipeIllustrationUrl(userId: recipe.UserId, recipeId: recipe.Id) : string.Empty
        };
    }

    private static void ValidateAndThrowOnFailures(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}