using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Storage;

public interface IStorageService
{
    Task UploadProfilePicture(User user, Stream file, string contentType);
    Task UploadIllustration(Recipe recipe, Stream file, string contentType);
    string GetProfilePictureUrl(User user);
    string GetIllustrationUrl(Recipe recipe);
}