using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.ChangeIllustration;

public class ChangeIllustrationUseCase : IChangeIllustrationUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IStorageService _storageService;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeIllustrationUseCase(
        ILoggedUser loggedUser,
        IStorageService storageService,
        IRecipeUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _storageService = storageService;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid recipeId, Stream recipeIllustration)
    {
        var contentType = recipeIllustration.DetectImageContentType();
        if (contentType.IsEmpty())
            throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED]);

        var userId = _loggedUser.GetUserId();

        var recipe = await _repository.GetById(recipeId, userId);
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);

        await _storageService.UploadIllustration(recipe, recipeIllustration, contentType);

        recipe.HasImage = true;

        await _unitOfWork.Commit();
    }
}
