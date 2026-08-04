using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangeProfilePicture;

public class ChangeProfilePictureUseCase : IChangeProfilePictureUseCase
{
    private readonly ILoggedUser _loggedUser;

    public ChangeProfilePictureUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task Execute(Stream profilePicture)
    {
        var isImage = profilePicture.Is<PortableNetworkGraphic>() || profilePicture.Is<JointPhotographicExpertsGroup>();
        if (isImage == false)
            throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED]);
    }
}
