using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Extensions;
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
        var contentType = profilePicture.DetectImageContentType();
        if (contentType.IsEmpty())
            throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED]);
    }
}
