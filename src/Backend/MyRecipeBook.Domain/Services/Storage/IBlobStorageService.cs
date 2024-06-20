using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.Storage;
public interface IBlobStorageService
{
    Task Upload(User user, Stream file, string fileName);
    Task<string> GetImageUrl(User user, string fileName);
}
