using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangeProfilePicture;

public class ChangeProfilePictureUseCase : IChangeProfilePictureUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IStorageService _storageService;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public ChangeProfilePictureUseCase(
        ILoggedUser loggedUser,
        IStorageService storageService,
        IUserUpdateOnlyRepository userUpdateOnlyRepository)
    {
        _loggedUser = loggedUser;
        _storageService = storageService;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
    }

    public async Task Execute(Stream profilePicture)
    {
        var contentType = profilePicture.DetectImageContentType();
        if (contentType.IsEmpty())
            throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED]);

        var loggedUser = await _loggedUser.Get();

        await _storageService.UploadProfilePicture(loggedUser, profilePicture, contentType);

        await _userUpdateOnlyRepository.UpdateProfilePictureStatus(loggedUser.Id, hasProfilePicture: true);
    }
}
