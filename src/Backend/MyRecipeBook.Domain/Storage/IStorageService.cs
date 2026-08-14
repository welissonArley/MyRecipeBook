using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Storage;

public interface IStorageService
{
    Task UploadProfilePicture(User user, Stream file, string contentType);
    Task UploadIllustration(Recipe recipe, Stream file, string contentType);
    string GetProfilePictureUrl(User user);
    string GetRecipeIllustrationUrl(Guid userId, Guid recipeId);
    Task DeleteUserFiles(User user);
    Task DeleteRecipeIllustration(Guid userId, Guid recipeId);
}