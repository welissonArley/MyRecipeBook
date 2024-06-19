using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;
public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public AddUpdateImageCoverUseCase(
        ILoggedUser loggedUser,
        IRecipeUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(long recipeId, IFormFile file)
    {
        var loggedUser = await _loggedUser.User();
        
        var recipe = await _repository.GetById(loggedUser, recipeId);
        
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var fileStream = file.OpenReadStream();

        if(
            fileStream.Is<PortableNetworkGraphic>().IsFalse()
            && fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
        {
            throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
        }

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            _repository.Update(recipe);

            await _unitOfWork.Commit();
        }

        fileStream.Position = 0;

        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
    }
}
